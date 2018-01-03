using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.ExceptionHandler;
using System.IO;
using System.Data.SqlTypes;
using SentenceTransformer;

namespace NewAge.ADO
{
    public class DAL_MasterSimple : DAL_Base
    {
        #region Variables
        private int _documentID;
        protected string _tableName;
        protected string _colId;
        protected bool _hasCompany;
        protected Type _dtoType = typeof(DTO_MasterBasic);
        protected List<DTO_aplMaestraCampo> _extraFields;
        protected List<ForeignKeyField> _foreignKeys;

        protected DTO_aplMaestraPropiedades props;
        protected SqlConnection ct;
        protected SqlTransaction txt;
        #endregion

        #region Propiedades

        /// <summary>
        /// Propiedad que asigna el codigo de la tabla
        /// </summary>
        public int DocumentID
        {
            get
            {
                return this._documentID;
            }         
            set
            {
                this.props = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, value, this.loggerConnectionStr);

                this._documentID = value;
                this._extraFields = props.Campos;
                this._foreignKeys = new List<ForeignKeyField>();

                foreach (DTO_aplMaestraCampo mf in this._extraFields)
                {
                    if (mf is ForeignKeyField)
                    {
                        ForeignKeyField fk = (ForeignKeyField)mf;
                        this._foreignKeys.Add(fk);
                    }
                }
                this._tableName = props.NombreTabla;
                this._colId = props.ColumnaID;
                this._hasCompany = props.GrupoEmpresaInd;
                this._dtoType = Type.GetType("NewAge.DTO.Negocio." + props.DTOTipo + ", NewAge.DTO");

                if (this.Empresa != null)
                {
                    string egCtrl = StaticMethods.GetGrupoEmpresasControl(this.MySqlConnection, this.MySqlConnectionTx, this.loggerConnectionStr);
                    this.EmpresaGrupoID = this.GetMaestraEmpresaGrupoByDocumentID(value, this.Empresa, egCtrl);
                }
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_MasterSimple(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn)
            : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Protected

        /// <summary>
        ///  Retorna una maestra básica a partir del identificador
        /// </summary>
        /// <param name="replicaID">Identificador</param>
        /// <returns>Devuelve la maestra basica</returns>
        protected DTO_MasterBasic DAL_MasterSimple_GetByReplicaID(int replicaID)
        {
            try
            {
                DTO_MasterBasic dto = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                "SELECT * FROM " + this._tableName + " with(nolock) " +
                "WHERE ReplicaID = @ReplicaID";

                mySqlCommand.Parameters.Add("@ReplicaID", SqlDbType.Int);
                mySqlCommand.Parameters["@ReplicaID"].Value = replicaID;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    dto = this.CreateDto(dr, props, true);
                }
                dr.Close();

                return dto;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_GetByReplicaID");
                throw exception;
            }
        }

        /// <summary>
        ///Actualiza una maestra básica
        /// </summary>
        /// <param name="dto">MasterBasic</param> 
        protected DTO_TxResultDetail DAL_MasterSimple_UpdateItem(DTO_MasterBasic dto)
        {
            try
            {
                bool validDTO = true;
                DTO_TxResultDetail rd = new DTO_TxResultDetail();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Mensajes de error
                string msgEmptyField = DictionaryMessages.EmptyField;
                string msg_FkNotFound = DictionaryMessages.FkNotFound;
                //Texto para campos extras
                string fields = "";

                #region Validacion de nulls campos basicos
                if (string.IsNullOrEmpty(dto.ID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = this._colId;
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (string.IsNullOrEmpty(dto.Descriptivo.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "Descriptivo";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (!dto.ActivoInd.Value.HasValue)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "ActivoInd";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                #endregion
                #region Validacion de los campos extras
                bool isEmp = this._tableName == "glEmpresa";

                foreach (DTO_aplMaestraCampo mf in this._extraFields)
                {
                    if (mf.ActualizableInd)
                    {
                        bool validExtraParam = true;

                        if (mf.NombreColumna.Equals("Contrasena"))
                            continue;

                        fields += ", " + mf.NombreColumna + "=" + "@" + mf.NombreColumna;
                        object value = null;

                        string colName = isEmp && mf.NombreColumna == "EmpresaGrupoID" ? "EmpresaGrupoID_" : mf.NombreColumna;
                        PropertyInfo pi = this._dtoType.GetProperty(colName);
                        UDT udt = null;

                        #region Saca el valor de la columna
                        if (pi != null)
                        {
                            udt = (UDT)pi.GetValue(dto, null);
                            if (udt != null && udt.GetType().GetProperty("Value").GetValue(udt, null) != null)
                                value = udt.GetType().GetProperty("Value").GetValue(udt, null);
                            else
                                value = string.Empty;
                        }
                        else
                        {
                            FieldInfo fi = dto.GetType().GetField(mf.NombreColumna);
                            if (fi != null)
                            {
                                udt = (UDT)fi.GetValue(dto);
                                if (udt.GetType().GetProperty("Value").GetValue(udt, null) != null)
                                    value = udt.GetType().GetProperty("Value").GetValue(udt, null);
                                else
                                    value = string.Empty;
                            }
                        }
                        #endregion
                        #region Validacion de nulls
                        if (!mf.VacioInd && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = mf.NombreColumna;
                            rdF.Message = msgEmptyField;
                            rd.DetailsFields.Add(rdF);

                            validDTO = false;
                            validExtraParam = false;
                        }
                        #endregion
                        #region Validacion FKS
                        if (mf.FKInd && !string.IsNullOrWhiteSpace(value.ToString()))
                        {
                            try
                            {
                                string fkQuery = string.Empty;
                                SqlCommand commFK = base.MySqlConnection.CreateCommand();
                                commFK.Transaction = base.MySqlConnectionTx;

                                DTO_aplMaestraPropiedades fkProp = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, mf.FKDocumentoID, this.loggerConnectionStr);
                                bool egValid = true;

                                if (!isEmp && dto.EmpresaGrupoID != null && string.IsNullOrWhiteSpace(dto.EmpresaGrupoID.Value) && fkProp.GrupoEmpresaInd)
                                {
                                    egValid = false;
                                    validDTO = false;
                                    validExtraParam = false;

                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = mf.NombreColumna;
                                    rdF.Message = msg_FkNotFound + "&&" + value.ToString();
                                    rd.DetailsFields.Add(rdF);
                                }
                                else if (!fkProp.GrupoEmpresaInd)
                                {
                                    fkQuery = "select * from " + mf.FKNombreTabla + " with(nolock) where " + mf.FKColumnaID + " = @FKColumnaID AND ActivoInd = 1";
                                }
                                else
                                {
                                    fkQuery = "select * from " + mf.FKNombreTabla + " with(nolock) where " + mf.FKColumnaID
                                        + " = @FKColumnaID and EmpresaGrupoID = @EmpresaGrupoID AND ActivoInd = 1";

                                    string egFk = string.Empty;
                                    DAL_glControl ctrlDAL = new DAL_glControl (base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                                    DTO_glControl controlDTO = ctrlDAL.DAL_glControl_GetById(AppControl.GrupoEmpresaGeneral);

                                    string egControl = controlDTO.Data.Value;
                                    egFk = ctrlDAL.GetMaestraEmpresaGrupoByDocumentID(mf.FKDocumentoID, this.Empresa, egControl);

                                    string colEgFk = EgFkPrefix + mf.FKNombreTabla;
                                    string paramColEgFk = "@" + colEgFk;
                                    if (!fields.Contains(", " + colEgFk + "=" + paramColEgFk) && !mf.FKNombreTabla.Equals(this._tableName))
                                    {
                                        mySqlCommand.Parameters.Add(paramColEgFk, SqlDbType.VarChar, UDT_EmpresaGrupoID.MaxLength);
                                        mySqlCommand.Parameters[paramColEgFk].Value = egFk;

                                        fields += ", " + colEgFk + "=" + paramColEgFk;
                                    }

                                    commFK.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                                    commFK.Parameters["@EmpresaGrupoID"].Value = egFk;
                                }

                                if (mf.FKDocumentoID == AppMasters.glDocumento)
                                {
                                    commFK.Parameters.Add("@FKColumnaID", SqlDbType.Int);
                                    commFK.Parameters["@FKColumnaID"].Value = Convert.ToInt32(value);
                                }
                                else
                                {
                                    commFK.Parameters.Add("@FKColumnaID", SqlDbType.Char, 30); // un valor maximo para los ids
                                    commFK.Parameters["@FKColumnaID"].Value = value.ToString();
                                }
                                commFK.CommandText = fkQuery;

                                object fkRes = null;
                                if (egValid)
                                {
                                    fkRes = commFK.ExecuteScalar();
                                }

                                if (fkRes == null && egValid)
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = mf.NombreColumna;
                                    rdF.Message = msg_FkNotFound + "&&" + value.ToString();
                                    rd.DetailsFields.Add(rdF);

                                    validDTO = false;
                                    validExtraParam = false;
                                }
                            }
                            catch (Exception eFK)
                            {
                                var exception = new Exception(DictionaryMessages.Err_UpdateData, eFK);
                                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_UpdateItem");
                                throw exception;
                            }
                        }
                        #endregion

                        if (validDTO)
                        {
                            if (mf.FKNombreTabla == "seUsuario")
                            {
                                DAL_seUsuario usrDAL = new DAL_seUsuario(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                                string usrId = (value == null || string.IsNullOrWhiteSpace(value.ToString())) ? string.Empty : value.ToString();
                                DTO_seUsuario usrDTO = usrDAL.DAL_seUsuario_GetUserbyID(usrId);

                                mySqlCommand.Parameters.Add("@" + mf.NombreColumna, SqlDbType.Int);
                                if (usrDTO == null || string.IsNullOrEmpty(usrDTO.IdName))
                                    mySqlCommand.Parameters["@" + mf.NombreColumna].Value = DBNull.Value;
                                else
                                    mySqlCommand.Parameters["@" + mf.NombreColumna].Value = usrDTO.ReplicaID.Value.Value;
                            }
                            else
                            {
                                 SqlParameter param = this.GetParameter(mf, udt);
                                 mySqlCommand.Parameters.Add(param);
                                 mySqlCommand.Parameters[mf.NombreColumna].Value = (value == null || string.IsNullOrWhiteSpace(value.ToString())) ? DBNull.Value : value;
                            }
                        }
                    }
                }
                #endregion
                #region Validacion de restricciones
                List<DTO_TxResultDetailFields> invRules = DTO_Validations.CheckRules(this.loggerConnectionStr, base.MySqlConnection, base.MySqlConnectionTx, dto, this.Empresa, this.EmpresaGrupoID, this.UserId, false);
                if (invRules.Count > 0)
                {
                    foreach (DTO_TxResultDetailFields resDetail in invRules)
                    {
                        rd.DetailsFields.Add(resDetail);
                    }

                    validDTO = false;
                }
                #endregion

                fields += " ";

                //Creacion del query
                mySqlCommand.CommandText =
                  "UPDATE " + this._tableName +
                   " SET " + this._colId + " = @ID, Descriptivo = @Descriptivo, ActivoInd = @ActivoInd, CtrlVersion = @CtrlVersion " + fields +
                   "WHERE ReplicaID = @ReplicaID";

                if (validDTO)
                {
                    mySqlCommand.Parameters.Add("@ReplicaID", SqlDbType.Int);
                    mySqlCommand.Parameters.Add("@ID", SqlDbType.Char, dto.ID.MaxLength);
                    mySqlCommand.Parameters.Add("@Descriptivo", SqlDbType.VarChar, UDT_Descriptivo.MaxLength);
                    mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.SmallInt);
                    mySqlCommand.Parameters.Add("@CtrlVersion", SqlDbType.SmallInt);

                    mySqlCommand.Parameters["@ReplicaID"].Value = dto.ReplicaID.Value;
                    mySqlCommand.Parameters["@ID"].Value = dto.ID.Value.Trim();
                    mySqlCommand.Parameters["@Descriptivo"].Value = dto.Descriptivo.Value.Trim();
                    mySqlCommand.Parameters["@ActivoInd"].Value = dto.ActivoInd.Value;
                    mySqlCommand.Parameters["@CtrlVersion"].Value = dto.CtrlVersion.Value;

                    mySqlCommand.ExecuteNonQuery();
                    rd.Message = "OK";
                }
                else
                {
                    rd.Message = "NOK";
                }

                return rd;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_UpdateItem");
                throw exception;
            }
        }

        /// <summary>
        /// Método que genera una condición(consulta-where) basado en un filtro complejo
        /// </summary>
        /// <param name="filtro">Filtro</param>
        /// <param name="baseTableName">Alias de la tabla base</param>
        /// <param name="fkTableAlias">Alias de la tabla referenciada</param>
        /// <param name="tablas">Tablas del sistema. Si el valor es null la función las determinará</param>
        /// <returns>Consulta para el query</returns>
        protected string WhereIn(DTO_glConsultaFiltroComplejo filtro, string baseTableName, string fkTableAlias, List<DTO_glTabla> tablas = null)
        {
            #region Trae la información de tablas
            if (tablas == null)
            {
                DAL_glTabla dalTablas = new DAL_glTabla(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(this.MySqlConnection, this.MySqlConnectionTx, this.loggerConnectionStr);
                dalTablas.SetCallParameters(AppMasters.glTabla, this.Empresa);
                Dictionary<int, string> empGrupo = new Dictionary<int, string>();
                DTO_glEmpresa emp = this.Empresa;

                empGrupo.Add((int)GrupoEmpresa.Automatico, emp.ID.Value);
                empGrupo.Add((int)GrupoEmpresa.Individual, emp.EmpresaGrupoID_.Value);
                empGrupo.Add((int)GrupoEmpresa.General, egCtrl);

                tablas = dalTablas.DAL_glTabla_GetAllByEmpresaGrupo(empGrupo).ToList();
            }
            #endregion
            #region Distribuye los filtros de la tabla referenciada entre simples y complejos
            DTO_glTabla tabla = tablas.Where(x => x.DocumentoID.Value.Value == filtro.DocumentoIDFK).First();
            List<string> whereIns = new List<string>();
            List<DTO_glConsultaFiltro> filtrosSimples = new List<DTO_glConsultaFiltro>();
            foreach (DTO_glConsultaFiltro f in filtro.FiltrosDetalle)
            {
                if (f is DTO_glConsultaFiltroComplejo)
                {
                    DTO_glTabla tablaFK = tablas.Where(x => x.DocumentoID.Value.Value == (f as DTO_glConsultaFiltroComplejo).DocumentoIDFK).First();
                    string relName = tabla.TablaNombre.Value + tablaFK.TablaNombre.Value;
                    whereIns.Add(this.WhereIn(f as DTO_glConsultaFiltroComplejo, tabla.TablaNombre.Value, relName, tablas));
                }
                else
                {
                    filtrosSimples.Add(f);
                }
            }
            #endregion
            Type t = typeof(DTO_MasterBasic);
            #region Where de los simples
            string where = string.Empty;
            if (filtrosSimples.Count > 0)
            {
                List<ConsultasFields> fields = new List<ConsultasFields>();
                foreach (DTO_glConsultaFiltro fil in filtrosSimples)
                {
                    if (filtro.FieldTypes.ContainsKey(fil.CampoFisico))
                    {
                        fields.Add(new ConsultasFields()
                        {
                            Field = fil.CampoFisico,
                            Tipo = filtro.FieldTypes[fil.CampoFisico]
                        });
                    }
                }
                where = Transformer.WhereSql(filtrosSimples, t, fields);
            }

            string basetableColumns = string.Empty;
            string selectFkColumns = string.Empty;
            string condition = string.Empty;
            foreach (KeyValuePair<string, string> kvp in filtro.LlavesFK)
            {
                if (!string.IsNullOrWhiteSpace(condition))
                {
                    condition += " AND ";
                }
                if (!string.IsNullOrWhiteSpace(basetableColumns))
                {
                    basetableColumns += ",";
                }
                if (!string.IsNullOrWhiteSpace(selectFkColumns))
                {
                    selectFkColumns += ",";
                }
                basetableColumns += baseTableName + "." + kvp.Key;
                selectFkColumns += tabla.TablaNombre.Value + "." + kvp.Value;
                condition += baseTableName + "." + kvp.Key + "=" + tabla.TablaNombre.Value + "." + kvp.Value;
            }
            #endregion
            #region Where de los complejos
            string queryTabla = "SELECT " + selectFkColumns + " FROM " + tabla.TablaNombre.Value + " with(nolock) ";
            string where2 = string.Empty;
            foreach (string whereIn in whereIns)
            {
                if (!string.IsNullOrWhiteSpace(where2))
                    where2 += " AND ";
                where2 += whereIn;
            }
            #endregion
            string whereTotal = string.Empty;
            if (!string.IsNullOrWhiteSpace(where))
            {
                whereTotal += " WHERE (" + where + ") ";
            }
            if (!string.IsNullOrWhiteSpace(where2))
            {
                if (!string.IsNullOrWhiteSpace(whereTotal))
                    whereTotal += " AND (" + where2 + ") ";
                else
                    whereTotal += " WHERE (" + where2 + ") ";
            }
            if (!string.IsNullOrWhiteSpace(whereTotal))
                whereTotal += " AND (" + condition + ") ";
            else
                whereTotal += " WHERE (" + condition + ") ";

            queryTabla += whereTotal;

            string whereConditionTotal = " EXISTS(" + queryTabla + ")";

            string queryJoinTotal = " JOIN " + "(" + queryTabla + ")" + fkTableAlias + " ON " + condition + " ";
            return whereConditionTotal;
        }

        /// <summary>
        /// Crea un dto a partir de el data reader y las porpiedades de la maestra
        /// </summary>
        /// <param name="dr">datareader</param>
        /// <param name="props">maestra</param>
        /// <returns>Maestra básica</returns>
        protected DTO_MasterBasic CreateDto(IDataReader dr, DTO_aplMaestraPropiedades props, bool isReplica)
        {
            try
            {
                return (DTO_MasterBasic)Activator.CreateInstance(this._dtoType, new object[] { dr, props, isReplica });
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_Data, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_CreateDTO");
                return new DTO_MasterBasic();
            }
        }

        /// <summary>
        /// Compara 2 maestras y verifica que campos son diferentes
        /// </summary>
        /// <param name="oldGp">Gp antes de consultado</param>
        /// <param name="newGp">Gp dado</param>
        /// <returns>Lista de DTO_TxResultDetailFields</returns>
        protected List<DTO_TxResultDetailFields> GetDiferentFields(DTO_MasterBasic oldGp, DTO_MasterBasic newGp)
        {
            List<DTO_TxResultDetailFields> response = new List<DTO_TxResultDetailFields>();

            if (this._hasCompany && oldGp.EmpresaGrupoID.Value != newGp.EmpresaGrupoID.Value)
            {
                DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                field.Field = "EmpresaGrupoID";
                field.OldValue = oldGp.EmpresaGrupoID.Value.ToString();
                field.NewValue = newGp.EmpresaGrupoID.Value.ToString();
                response.Add(field);
            }
            if (oldGp.ID.Value != newGp.ID.Value)
            {
                DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                field.Field = this._colId;
                field.OldValue = oldGp.ID.Value;
                field.NewValue = newGp.ID.Value;
                response.Add(field);
            }
            if (oldGp.ActivoInd.Value != newGp.ActivoInd.Value)
            {
                DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                field.Field = "ActivoInd";
                field.OldValue = oldGp.ActivoInd.Value.ToString();
                field.NewValue = newGp.ActivoInd.Value.ToString();
                response.Add(field);
            }
            if (oldGp.Descriptivo.Value != newGp.Descriptivo.Value)
            {
                DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                field.Field = "Descriptivo";
                field.OldValue = oldGp.Descriptivo.Value.ToString();
                field.NewValue = newGp.Descriptivo.Value.ToString();
                response.Add(field);
            }
            if (oldGp.CtrlVersion.Value != newGp.CtrlVersion.Value)
            {
                DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                field.Field = "CtrlVersion";
                field.OldValue = oldGp.CtrlVersion.Value.ToString();
                field.NewValue = newGp.CtrlVersion.Value.ToString();
                response.Add(field);
            }

            bool isEmp = this._tableName == "glEmpresa";

            foreach (DTO_aplMaestraCampo mf in this._extraFields)
            {
                try
                {
                    if (mf.ActualizableInd)
                    {
                        object valueOld = null;
                        object valueNew = null;

                        string colName = isEmp && mf.NombreColumna == "EmpresaGrupoID" ? "EmpresaGrupoID_" : mf.NombreColumna;

                        PropertyInfo property = this._dtoType.GetProperty(colName);
                        if (property != null)
                        {
                            UDT oldUdt = (UDT)property.GetValue(oldGp, null);
                            UDT newUdt = (UDT)property.GetValue(newGp, null);
                            if (!mf.NombreColumna.Equals("Contrasena"))
                            {
                                valueOld = property.PropertyType.GetProperty("Value").GetValue(oldUdt, null);
                                valueNew = property.PropertyType.GetProperty("Value").GetValue(newUdt, null);
                            }
                        }
                        else
                        {
                            FieldInfo field = this._dtoType.GetField(mf.NombreColumna);
                            if (field != null)
                            {
                                UDT oldUdt = (UDT)field.GetValue(oldGp);
                                UDT newUdt = (UDT)field.GetValue(newGp);
                                valueOld = field.FieldType.GetProperty("Value").GetValue(oldUdt, null);
                                valueNew = field.FieldType.GetProperty("Value").GetValue(newUdt, null);
                            }
                        }
                        if ((valueOld != null && !valueOld.Equals(valueNew)) || (valueOld == null && valueNew != null))
                        {
                            DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                            field.Field = mf.NombreColumna;
                            field.OldValue = valueOld == null ? string.Empty : valueOld.ToString();
                            field.NewValue = valueNew == null ? string.Empty : valueNew.ToString();
                            response.Add(field);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                    Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_GetDiferentFields");
                    throw exception;
                }
            }

            return response;
        }

        /// <summary>
        /// Crea un SqlParamter a partir de un nombre y un tipo
        /// </summary>
        /// <param name="mf">Campo de la maestra</param>
        /// <param name="value">Valor del campo</param>
        /// <returns>Retorna un parametro de sql</returns>
        protected SqlParameter GetParameter(DTO_aplMaestraCampo mf, object value = null)
        {
            Type t = Type.GetType("NewAge.DTO.UDT." + mf.Tipo + ", NewAge.DTO");
            if (t == typeof(UDT_SiNo) || t == typeof(bool) || t == typeof(UDT_CtrlVersion) || t==typeof(UDTSQL_smallint))
            {
                return new SqlParameter(mf.NombreColumna, SqlDbType.SmallInt);
            }
            if (t == typeof(UDTSQL_tinyint) || t == typeof(UDT_coSaldoControl) || t == typeof(UDT_PeriodoCert) || t == typeof(UDT_SiNo))
            {
                return new SqlParameter(mf.NombreColumna, SqlDbType.TinyInt);
            }
            if (t == typeof(UDTSQL_int) || t == typeof(UDT_BitacoraID) || t == typeof(UDT_Consecutivo) || t == typeof(UDT_DocumentoID) || t == typeof(UDT_ReplicaID) || t == typeof(UDT_seUsuarioID) || t == typeof(UDT_ContratoNOID))
            {
                return new SqlParameter(mf.NombreColumna, SqlDbType.Int);
            }
            if (t == typeof(UDTSQL_bigint))
            {
                return new SqlParameter(mf.NombreColumna, SqlDbType.BigInt);
            }
            if (t == typeof(UDT_Valor) || t == typeof(UDT_PorcentajeID) || t == typeof(UDT_PorcentajeCarteraID) || t == typeof(UDT_TasaID) || t == typeof(UDTSQL_numeric) || t == typeof(UDTSQL_decimal) || t == typeof(UDT_Cantidad)
                || t == typeof(UDT_BaseUVT) || t == typeof(UDT_FactorID))
            {
                return new SqlParameter(mf.NombreColumna, SqlDbType.Decimal);
            }
            if (t == typeof(UDT_Imagen))
            {
                return new SqlParameter(mf.NombreColumna, SqlDbType.VarBinary);
            }
            if (t == typeof(UDT_BasicID))
            {
                UDT_BasicID vo = (UDT_BasicID)value;
                SqlParameter param = new SqlParameter(mf.NombreColumna, SqlDbType.Char, vo.MaxLength);
                return param;
            }
            if (t == typeof(UDTSQL_varchar))
            {
                UDTSQL_varchar vo = (UDTSQL_varchar)value;
                SqlParameter param = new SqlParameter(mf.NombreColumna, SqlDbType.Char, vo.MaxLength);
                return param;
            }
            if (t == typeof(UDTSQL_char) )
            {
                UDTSQL_char vo = (UDTSQL_char)value;
                SqlParameter param = new SqlParameter(mf.NombreColumna, SqlDbType.Char, vo.MaxLength);
                return param;
            }
            if (t == typeof(UDTSQL_datetime) || t == typeof(UDTSQL_smalldatetime) || t == typeof(UDT_PeriodoID))
            {
                return new SqlParameter(mf.NombreColumna, SqlDbType.DateTime);
            }
            if (t == typeof(UDTSQL_varcharMAX))
            {
                return new SqlParameter(mf.NombreColumna, SqlDbType.VarChar, UDTSQL_varcharMAX.RichTextMaxLength);
            }
            if (t == typeof(UDT_LineaNegocioID) || t == typeof(UDT_AreaFuncionalID) || t == typeof(UDT_CentroCostoID) || 
                t == typeof(UDT_CuentaID) || t == typeof(UDT_DescripTBase) || t == typeof(UDT_DescripTExt) || t == typeof(UDT_DescripUnFormat) ||
                t == typeof(UDT_ConceptoCargoID) || t == typeof(UDT_EmpresaGrupoID) || t == typeof(UDT_EmpresaID) || 
                t == typeof(UDT_IdiomaID) || t == typeof(UDT_LineaPresupuestoID) || t == typeof(UDT_LocFisicaID) || 
                t == typeof(UDT_LugarGeograficoID) || t == typeof(UDT_ModuloID) || t == typeof(UDT_MonedaID) || 
                t == typeof(UDT_PaisID) || t == typeof(UDT_ProyectoID) || t == typeof(UDT_seGrupoID) || t == typeof(UDT_BancoID) || 
                t == typeof(UDT_SerialID) || t == typeof(UDT_TerceroID) || t == typeof(UDT_UsuarioID) ||
                t == typeof(UDT_RepLineaID) || t == typeof(UDT_Renglon) || t == typeof(UDT_ImpuestoDeclID) || t == typeof(UDT_UbicacionID) ||
                t == typeof(UDT_FuenteUsoID) || t == typeof(UDT_indFinaciero) || t == typeof(UDT_Descriptivo) || t == typeof(UDT_CodigoGrl) ||
                t == typeof(UDT_CodigoGrl2) || t == typeof(UDT_CodigoGrl5) || t == typeof(UDT_CodigoGrl10) || t == typeof(UDT_CodigoGrl20))
            {
                FieldInfo fi = t.GetField("MaxLength");
                if (fi != null)
                {
                    int ml = Convert.ToInt32(fi.GetValue(value));
                    SqlParameter param = new SqlParameter(mf.NombreColumna, SqlDbType.Char, ml);
                    return param;
                }
                else
                {
                    throw new Exception(DictionaryMessages.Err_UdtLength);//msg6
                }
            }

            var exception = new Exception(DictionaryMessages.Err_UdtToSQL);
            Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_GetParameter");
            throw exception;
        }

        #endregion

        #region Funciones Internal

        /// <summary>
        /// Adiciona una maestra básica
        /// </summary>
        /// <param name="dto">MasterBasic</param>
        public DTO_TxResultDetail DAL_MasterSimple_AddItem(DTO_MasterBasic dto)
        {
            try
            {
                bool validDTO = true;
                DTO_TxResultDetail rd = new DTO_TxResultDetail();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string fields = "";
                string commandParams = "";

                //Mensajes de error
                string msgExistingItem = DictionaryMessages.PkInUse;
                string msgEmptyField = DictionaryMessages.EmptyField;
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                #region Validacion de nulls campos basicos
                if (string.IsNullOrEmpty(dto.ID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = this._colId;
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (string.IsNullOrEmpty(dto.Descriptivo.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "Descriptivo";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (!dto.ActivoInd.Value.HasValue)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "ActivoInd";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                #endregion
                #region Validacion Pk inexistente
                if (!this._hasCompany)
                {
                    mySqlCommand.CommandText = "select * from " + this._tableName + " with(nolock) where " + this._colId + " = @ID";
                }
                else
                {
                    mySqlCommand.CommandText = "select * from " + this._tableName + " with(nolock) where " + this._colId
                        + " = @ID and EmpresaGrupoID = @EmpresaGrupoID";

                    mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                    mySqlCommand.Parameters["@EmpresaGrupoID"].Value = dto.EmpresaGrupoID.Value;
                }

                if (this.DocumentID == AppMasters.glDocumento)
                {
                    mySqlCommand.Parameters.Add("@ID", SqlDbType.Int);
                    mySqlCommand.Parameters["@ID"].Value = Convert.ToInt32(dto.ID.Value);
                }
                else
                {
                    mySqlCommand.Parameters.Add("@ID", SqlDbType.Char, dto.ID.MaxLength);
                    mySqlCommand.Parameters["@ID"].Value = dto.ID.Value;
                }

                object firstID = mySqlCommand.ExecuteScalar();
                mySqlCommand.Parameters.Clear();

                if (firstID != null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = this._colId;
                    rdF.Message = msgExistingItem;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }

                #endregion
                #region validaciones de campos extras
                bool isEmp = this._tableName == "glEmpresa";

                foreach (DTO_aplMaestraCampo mf in this._extraFields)
                {
                    if (mf.ActualizableInd)
                    {
                        bool validExtraParam = true;

                        if (mf.NombreColumna.Equals("Contrasena"))
                            continue;

                        fields += ", " + mf.NombreColumna;
                        commandParams += ", @" + mf.NombreColumna;
                        object value = null;

                        string colName = isEmp && mf.NombreColumna == "EmpresaGrupoID" ? "EmpresaGrupoID_" : mf.NombreColumna;
                        PropertyInfo pi = this._dtoType.GetProperty(colName);
                        UDT udt = null;

                        #region Saca el valor de la columna
                        if (pi != null)
                        {
                            udt = (UDT)pi.GetValue(dto, null);
                            if (udt.GetType().GetProperty("Value").GetValue(udt, null) != null)
                                value = udt.GetType().GetProperty("Value").GetValue(udt, null);
                            else
                                value = string.Empty;
                        }
                        else
                        {
                            FieldInfo fi = dto.GetType().GetField(mf.NombreColumna);
                            if (fi != null)
                            {
                                udt = (UDT)fi.GetValue(dto);
                                if (udt.GetType().GetProperty("Value").GetValue(udt, null) != null)
                                    value = udt.GetType().GetProperty("Value").GetValue(udt, null);
                                else
                                    value = string.Empty;
                            }
                        }
                        #endregion
                        #region Validacion de nulls
                        if (!mf.VacioInd && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = mf.NombreColumna;
                            rdF.Message = msgEmptyField;
                            rd.DetailsFields.Add(rdF);

                            validDTO = false;
                            validExtraParam = false;
                        }
                        #endregion
                        #region Validacion FKS
                        if (mf.FKInd && !string.IsNullOrWhiteSpace(value.ToString()))
                        {
                            try
                            {
                                string fkQuery = string.Empty;
                                SqlCommand commFK = base.MySqlConnection.CreateCommand();
                                commFK.Transaction = base.MySqlConnectionTx;

                                DTO_aplMaestraPropiedades fkProp = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, mf.FKDocumentoID, this.loggerConnectionStr);
                                bool egValid = true;

                                if (!isEmp && dto.EmpresaGrupoID != null && string.IsNullOrWhiteSpace(dto.EmpresaGrupoID.Value) && fkProp.GrupoEmpresaInd)
                                {
                                    egValid = false;
                                    validDTO = false;
                                    validExtraParam = false;

                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = mf.NombreColumna;
                                    rdF.Message = msg_FkNotFound + "&&" + value.ToString();
                                    rd.DetailsFields.Add(rdF);
                                }
                                else if (!fkProp.GrupoEmpresaInd)
                                {
                                    fkQuery = "select * from " + mf.FKNombreTabla + " with(nolock) where " + mf.FKColumnaID + " = @FKColumnaID AND ActivoInd = 1";
                                }
                                else
                                {
                                    fkQuery = "select * from " + mf.FKNombreTabla + " with(nolock) where " + mf.FKColumnaID
                                        + " = @FKColumnaID and EmpresaGrupoID = @EmpresaGrupoID AND ActivoInd = 1";

                                    DAL_glControl ctrlDAL = new DAL_glControl(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                                    DTO_glControl controlDTO = ctrlDAL.DAL_glControl_GetById(AppControl.GrupoEmpresaGeneral);

                                    string egControl = controlDTO.Data.Value;
                                    string egFk = ctrlDAL.GetMaestraEmpresaGrupoByDocumentID(mf.FKDocumentoID, this.Empresa, egControl);

                                    string colEgFk = EgFkPrefix + mf.FKNombreTabla;
                                    string paramColEgFk = "@" + colEgFk;
                                    if (!fields.Contains(", " + colEgFk) && !mf.FKNombreTabla.Equals(this._tableName))
                                    {
                                        mySqlCommand.Parameters.Add(paramColEgFk, SqlDbType.VarChar, UDT_EmpresaGrupoID.MaxLength);
                                        mySqlCommand.Parameters[paramColEgFk].Value = egFk;

                                        fields += ", " + colEgFk;
                                        commandParams += ", " + paramColEgFk;
                                    }

                                    commFK.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, UDT_EmpresaGrupoID.MaxLength);
                                    commFK.Parameters["@EmpresaGrupoID"].Value = egFk;

                                }

                                if (mf.FKDocumentoID == AppMasters.glDocumento)
                                {
                                    commFK.Parameters.Add("@FKColumnaID", SqlDbType.Int);
                                    commFK.Parameters["@FKColumnaID"].Value = Convert.ToInt32(value);
                                }
                                else
                                {
                                    commFK.Parameters.Add("@FKColumnaID", SqlDbType.Char, 30); // un valor maximo para los ids
                                    commFK.Parameters["@FKColumnaID"].Value = value.ToString();
                                }
                                commFK.CommandText = fkQuery;

                                object fkRes = null;
                                if (egValid)
                                {
                                    fkRes = commFK.ExecuteScalar();
                                }

                                if (fkRes == null && egValid)
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = mf.NombreColumna;
                                    rdF.Message = msg_FkNotFound + "&&" + value.ToString();
                                    rd.DetailsFields.Add(rdF);

                                    validDTO = false;
                                    validExtraParam = false;
                                }
                            }
                            catch (Exception eFK)
                            {
                                var exception = new Exception(DictionaryMessages.Err_ValidateData, eFK);
                                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_AddItem");
                                throw exception;
                            }
                        }
                        #endregion

                        if (validDTO)
                        {
                            if (mf.FKNombreTabla == "seUsuario")
                            {
                                DAL_seUsuario usrDAL = new DAL_seUsuario(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                                string usrId = (value == null || string.IsNullOrWhiteSpace(value.ToString())) ? string.Empty : value.ToString();
                                DTO_seUsuario usrDTO = usrDAL.DAL_seUsuario_GetUserbyID(usrId);

                                mySqlCommand.Parameters.Add("@" + mf.NombreColumna, SqlDbType.Int);
                                if (usrDTO == null || string.IsNullOrEmpty(usrDTO.IdName))
                                    mySqlCommand.Parameters["@" + mf.NombreColumna].Value = DBNull.Value;
                                else
                                    mySqlCommand.Parameters["@" + mf.NombreColumna].Value = usrDTO.ReplicaID.Value.Value;
                            }
                            else
                            {
                                SqlParameter param = this.GetParameter(mf, udt);
                                mySqlCommand.Parameters.Add(param);
                                mySqlCommand.Parameters[mf.NombreColumna].Value = (value == null || string.IsNullOrWhiteSpace(value.ToString())) ? DBNull.Value : value;
                            }
                        }
                    }
                }
                #endregion
                #region Validacion de restricciones
                List<DTO_TxResultDetailFields> invRules = DTO_Validations.CheckRules(this.loggerConnectionStr, base.MySqlConnection, base.MySqlConnectionTx, dto, this.Empresa, this.EmpresaGrupoID, this.UserId, true);
                if (invRules.Count > 0)
                {
                    foreach (DTO_TxResultDetailFields resDetail in invRules)
                    {
                        rd.DetailsFields.Add(resDetail);
                    }

                    validDTO = false;
                }
                #endregion

                fields += " ";
                commandParams += " ";

                #region Creacion del query
                if (!this._hasCompany)
                {
                    mySqlCommand.CommandText =
                      "INSERT INTO " + this._tableName + " (" +
                      " " + this._colId + " , Descriptivo, ActivoInd, CtrlVersion " + fields +
                      ") VALUES (" +
                      " @ID, @Descriptivo, @ActivoInd, @CtrlVersion " + commandParams +
                      ")";
                }
                else
                {
                    mySqlCommand.CommandText =
                      "INSERT INTO " + this._tableName + " (" +
                      "  EmpresaGrupoID, " + this._colId + " , Descriptivo, ActivoInd, CtrlVersion " + fields +
                      ") VALUES (" +
                      "  @EmpresaGrupoID, @ID, @Descriptivo, @ActivoInd, @CtrlVersion " + commandParams +
                      ")";

                    if (string.IsNullOrEmpty(dto.EmpresaGrupoID.Value))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "EmpresaGrupoID";
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);

                        validDTO = false;
                    }

                    mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                    mySqlCommand.Parameters["@EmpresaGrupoID"].Value = dto.EmpresaGrupoID.Value;
                }
                mySqlCommand.Parameters.Add("@ID", SqlDbType.Char, dto.ID.MaxLength);
                mySqlCommand.Parameters.Add("@Descriptivo", SqlDbType.VarChar, UDT_Descriptivo.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.SmallInt);
                mySqlCommand.Parameters.Add("@CtrlVersion", SqlDbType.SmallInt);

                #endregion

                if (validDTO)
                {
                    mySqlCommand.Parameters["@ID"].Value = dto.ID.Value.Trim();
                    mySqlCommand.Parameters["@Descriptivo"].Value = dto.Descriptivo.Value.Trim();
                    mySqlCommand.Parameters["@ActivoInd"].Value = dto.ActivoInd.Value;
                    mySqlCommand.Parameters["@CtrlVersion"].Value = dto.CtrlVersion.Value;

                    mySqlCommand.ExecuteNonQuery();

                    rd.Message = "OK";
                }
                else
                {
                    rd.Message = "NOK";
                }

                #region creaciones adicionales en el caso de que la maestra sea empresa grupo

                if (this.DocumentID.Equals(AppMasters.glEmpresaGrupo))
                {
                    #region Agregar tablas en glTabla

                    DAL_glTabla dalGlTabla = new DAL_glTabla(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    Dictionary<int, string> eGs = new Dictionary<int, string>();

                    DAL_glControl dalctrl = new DAL_glControl(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    eGs.Add((int)GrupoEmpresa.Automatico, dalctrl.DAL_glControl_GetById(AppControl.GrupoEmpresaGeneral).Data.Value);
                    eGs.Add((int)GrupoEmpresa.General, dalctrl.DAL_glControl_GetById(AppControl.GrupoEmpresaGeneral).Data.Value);
                    eGs.Add((int)GrupoEmpresa.Individual, dalctrl.DAL_glControl_GetById(AppControl.GrupoEmpresaGeneral).Data.Value);

                    IEnumerable<DTO_glTabla> glTs = dalGlTabla.DAL_glTabla_GetAllByEmpresaGrupo(eGs);

                    List<DTO_glTabla> tablasNuevas = new List<DTO_glTabla>();

                    DAL_MasterSimple dalTabla = new DAL_MasterSimple(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    dalTabla.SetCallParameters(AppMasters.glTabla, this.Empresa);
                    dalTabla.DocumentID = AppMasters.glTabla;

                    foreach (DTO_glTabla glT in glTs)
                    {
                        DTO_glTabla cloneT = glT;
                        glT.lonNivel1.Value = null;
                        glT.lonNivel2.Value = null;
                        glT.lonNivel3.Value = null;
                        glT.lonNivel4.Value = null;
                        glT.lonNivel5.Value = null;
                        glT.lonNivel6.Value = null;
                        glT.lonNivel7.Value = null;
                        glT.lonNivel8.Value = null;
                        glT.lonNivel9.Value = null;
                        glT.lonNivel10.Value = null;
                        glT.descrNivel1.Value = string.Empty;
                        glT.descrNivel2.Value = string.Empty;
                        glT.descrNivel3.Value = string.Empty;
                        glT.descrNivel4.Value = string.Empty;
                        glT.descrNivel5.Value = string.Empty;
                        glT.descrNivel6.Value = string.Empty;
                        glT.descrNivel7.Value = string.Empty;
                        glT.descrNivel8.Value = string.Empty;
                        glT.descrNivel9.Value = string.Empty;
                        glT.descrNivel10.Value = string.Empty;
                        glT.ReplicaID.Value = null;
                        glT.CtrlVersion.Value = 1;
                        glT.EmpresaGrupoID.Value = dto.ID.Value;
                        if (dalTabla.DAL_MasterSimple_AddItem(glT).Message == "NOK")
                            validDTO = false;
                        else
                            validDTO = true;
                    }

                    #endregion
                }

                #endregion

                if (validDTO)
                    rd.Message = "OK";
                else
                    rd.Message = "NOK";

                return rd;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_AddItem");
                throw exception;
            }
        }

        #endregion

        #region Funciones Públicas

        /// <summary>
        /// Retorna una maestra básica a partir del identificador
        /// </summary>
        /// <param name="id">Identificador de la maestra</param>
        /// <param name="active">indica si el registro debe estar activo</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Devuelve la maestra basica</returns>
        public DTO_MasterBasic DAL_MasterSimple_GetByID(UDT_BasicID id, bool active, List<DTO_glConsultaFiltro> filtros = null)
        {
            try
            {
                 DTO_MasterBasic result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string tablesFKs = string.Empty;
                string descFields = string.Empty;

                int cont = 1;
                int contUsuario = 1;

                foreach (ForeignKeyField fk in this._foreignKeys)
                {
                    DTO_aplMaestraPropiedades p = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, fk.FKDocumentoID, this.loggerConnectionStr);
                    bool fComp = p.GrupoEmpresaInd;

                    string fkAlias = "fk" + cont.ToString();
                    descFields += "," + fkAlias + "." + fk.ForeignDescField + " as " + fk.LocalDescField;

                    if (fk.TableName == "seUsuario")
                    {
                        descFields += "," + fkAlias + ".UsuarioID UsuarioID" + contUsuario;
                        contUsuario++;
                        tablesFKs += " left join " + fk.TableName + " " + fkAlias + " with(nolock) on baseTable." + fk.NombreColumna + " = " + fkAlias + ".ReplicaID";
                    }
                    else
                        tablesFKs += " left join " + fk.TableName + " " + fkAlias + " with(nolock) on baseTable." + fk.NombreColumna + " = " + fkAlias + "." + fk.ForeignField;

                    if (fComp)
                    {
                        string egFk = (fk.FKNombreTabla.Equals(this._tableName) ? "EmpresaGrupoID" : (EgFkPrefix + fk.FKNombreTabla));
                        tablesFKs += " and baseTable." + egFk + " = " + fkAlias + ".EmpresaGrupoID";
                    }

                    cont++;
                }

                mySqlCommand.CommandText =
                "SELECT baseTable.*" + descFields + " FROM " + this._tableName + " baseTable with(nolock) " + tablesFKs +
                " WHERE baseTable." + this._colId + " = @ID ";

                if (this._hasCompany)
                    mySqlCommand.CommandText += " AND baseTable.EmpresaGrupoID = @EmpresaGrupoID ";

                if (active)
                    mySqlCommand.CommandText += " AND baseTable.ActivoInd = 1 ";

                if (!string.IsNullOrEmpty(id.Value) && id.IsInt)
                {
                    mySqlCommand.Parameters.Add("@ID", SqlDbType.Int);
                    mySqlCommand.Parameters["@ID"].Value = Convert.ToInt32(id.Value);
                }
                else
                {
                    mySqlCommand.Parameters.Add("@ID", SqlDbType.Char, id.MaxLength);
                    mySqlCommand.Parameters["@ID"].Value = id.Value;
                }

                if (this._hasCompany)
                {
                    mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                    mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.EmpresaGrupoID;
                }

                string whereFiltros = string.Empty;

                if (filtros != null && filtros.Count > 0)
                {
                    List<DTO_glConsultaFiltro> simples = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltroComplejo> complejos = new List<DTO_glConsultaFiltroComplejo>();
                    Dictionary<string, string> baseTables = new Dictionary<string, string>();
                    foreach (DTO_glConsultaFiltro fc in filtros)
                    {
                        if (fc is DTO_glConsultaFiltroComplejo)
                            complejos.Add(fc as DTO_glConsultaFiltroComplejo);
                        else
                        {
                            simples.Add(fc);
                            if (!baseTables.ContainsKey(fc.CampoFisico))
                                baseTables.Add(fc.CampoFisico, "baseTable");
                        }
                    }
                    ConsultasFields fieldId = new ConsultasFields()
                    {
                        Field = this._colId,
                        Tipo = (this.DocumentID == AppMasters.glDocumento) ? typeof(int) : typeof(string)
                    };

                    List<ConsultasFields> listCF = new List<ConsultasFields>();
                    listCF.Add(fieldId);

                    if (simples != null )
                    {
                        foreach (DTO_glConsultaFiltro fil in simples)
                        {
                            if (fil.CampoFisico == "ID")
                                fil.CampoFisico = this._colId;
                        }
                    }

                    string whereSimples = Transformer.WhereSql(simples, this._dtoType, listCF,baseTables);

                    if (!string.IsNullOrEmpty(whereSimples))
                        whereFiltros += " AND (" + whereSimples + ")";

                    int i = 1;
                    string whereComplejos = string.Empty;
                    foreach (DTO_glConsultaFiltroComplejo fc in complejos)
                    {
                        string baseTable = "baseTable";
                        string fkTable = this._tableName + fc.DocumentoIDFK + "_" + i.ToString();
                        if (!string.IsNullOrWhiteSpace(whereComplejos))
                            whereComplejos += " AND ";
                        whereComplejos += WhereIn(fc, baseTable, fkTable);
                        i++;
                    }
                    if (!string.IsNullOrEmpty(whereComplejos))
                        whereFiltros += " AND (" + whereComplejos + ")";
                }

                mySqlCommand.CommandText += whereFiltros;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    result = this.CreateDto(dr, props, false);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna una maestra básica a partir de su descripcion
        /// En caso de encontrar mas de un resultado devolvera el primero
        /// </summary>
        /// <param name="desc">Descriptivo de la maestra</param>
        /// <returns>Devuelve la maestra basica</returns>
        public DTO_MasterBasic DAL_MasterSimple_GetByDesc(string desc)
        {
            try
            {
                DTO_MasterBasic result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string tablesFKs = string.Empty;
                string descFields = string.Empty;

                int cont = 1;

                foreach (ForeignKeyField fk in this._foreignKeys)
                {
                    DTO_aplMaestraPropiedades p = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, fk.FKDocumentoID, this.loggerConnectionStr);
                    bool fComp = p.GrupoEmpresaInd;

                    string fkAlias = "fk" + cont.ToString();
                    descFields += "," + fkAlias + "." + fk.ForeignDescField + " as " + fk.LocalDescField;
                    tablesFKs += " left join " + fk.TableName + " " + fkAlias + " with(nolock) on baseTable." + fk.NombreColumna + " = " + fkAlias + "." + fk.ForeignField;

                    if (fComp)
                    {
                        string egFk = (fk.FKNombreTabla.Equals(this._tableName) ? "EmpresaGrupoID" : (EgFkPrefix + fk.FKNombreTabla));
                        tablesFKs += " and baseTable." + egFk + " = " + fkAlias + ".EmpresaGrupoID";
                    }

                    cont++;
                }

                mySqlCommand.CommandText =
                "SELECT baseTable.*" + descFields + " FROM " + this._tableName + " baseTable with(nolock) " + tablesFKs +
                " WHERE baseTable.Descriptivo = @Descriptivo ";

                if (this._hasCompany)
                    mySqlCommand.CommandText += " AND baseTable.EmpresaGrupoID = @EmpresaGrupoID ";

                if (this._hasCompany)
                {
                    mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                    mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.EmpresaGrupoID;
                }

                mySqlCommand.Parameters.Add("@Descriptivo", SqlDbType.Char, UDT_Descriptivo.MaxLength);
                mySqlCommand.Parameters["@Descriptivo"].Value = desc;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    result = this.CreateDto(dr, props, false);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_GetByDesc");
                throw exception;
            }
        }

        /// <summary>
        /// Cuenta la cantidad de resultados dado un filtro
        /// </summary>
        /// <param name="consulta">Filtros</param>
        /// <param name="FiltrosExtra">Filtros de codigo y descripción</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Retorna el numero de registros encontrados</returns>
        public long DAL_MasterSimple_Count(DTO_glConsulta consulta, List<DTO_glConsultaFiltro> FiltrosExtra, bool? active)
        {
            try
            {
                string andActivo = string.Empty;

                string tablesFKs = string.Empty;
                string descFields = string.Empty;

                int cont = 1;
                int contUsuario = 1;

                foreach (ForeignKeyField fk in this._foreignKeys)
                {
                    DTO_aplMaestraPropiedades p = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, fk.FKDocumentoID, this.loggerConnectionStr);
                    bool fComp = p.GrupoEmpresaInd;

                    string fkAlias = "fk" + cont.ToString();
                    descFields += "," + fkAlias + "." + fk.ForeignDescField + " as " + fk.LocalDescField;

                    if (fk.TableName == "seUsuario")
                    {
                        descFields += "," + fkAlias + ".UsuarioID UsuarioID" + contUsuario;
                        contUsuario++;
                        tablesFKs += " left join " + fk.TableName + " " + fkAlias + " with(nolock) on baseTable." + fk.NombreColumna + " = " + fkAlias + ".ReplicaID";
                    }
                    else
                        tablesFKs += " left join " + fk.TableName + " " + fkAlias + " with(nolock) on baseTable." + fk.NombreColumna + " = " + fkAlias + "." + fk.ForeignField;

                    if (fComp)
                    {
                        string egFk = (fk.FKNombreTabla.Equals(this._tableName) ? "EmpresaGrupoID" : (EgFkPrefix + fk.FKNombreTabla));
                        tablesFKs += " and baseTable." + egFk + " = " + fkAlias + ".EmpresaGrupoID";
                    }

                    cont++;
                }

                if (active != null)
                    andActivo = " AND baseTable.ActivoInd = " + Convert.ToInt16(active);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = string.Empty;

                if (this._hasCompany)
                {
                    query =
                    "SELECT baseTable.*" + descFields +
                    " FROM " + this._tableName + " baseTable with(nolock) " + tablesFKs +
                    " WHERE baseTable.EmpresaGrupoID = @EmpresaGrupoID " + andActivo;

                    mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                    mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.EmpresaGrupoID;
                }
                else
                {
                    query =
                      "SELECT baseTable.*" + descFields +
                      " FROM " + this._tableName + " baseTable with(nolock) " + tablesFKs +
                      " WHERE 1 = 1 " + andActivo;
                }

                ConsultasFields fieldId = new ConsultasFields()
                {
                    Field = this._colId,
                    Tipo = (this.DocumentID == AppMasters.glDocumento) ? typeof(int) : typeof(string)
                };

                List<ConsultasFields> listCF = new List<ConsultasFields>();
                listCF.Add(fieldId);

                if (consulta != null)
                {
                    foreach (DTO_glConsultaFiltro fil in consulta.Filtros)
                    {
                        if (fil.CampoFisico == "ID")
                            fil.CampoFisico = this._colId;
                    }
                }

                string where = (consulta == null || consulta.Filtros == null) ? string.Empty : Transformer.WhereSql(consulta.Filtros, this._dtoType, listCF);
                string where2 = (FiltrosExtra == null) ? string.Empty : Transformer.WhereSql(FiltrosExtra, this._dtoType, listCF);

                if (!string.IsNullOrWhiteSpace(where2))
                {
                    if (!string.IsNullOrWhiteSpace(where))
                        where = "(" + where + ") AND (" + where2 + ")";
                    else
                        where = where2;
                }

                if (!string.IsNullOrWhiteSpace(where))
                    query = "SELECT COUNT(*) FROM (" + query + ") resultTable WHERE (" + where + ") ";
                else
                    query = "SELECT COUNT(*) FROM (" + query + ") resultTable";

                mySqlCommand.CommandText = query;

                long res = Convert.ToInt64(mySqlCommand.ExecuteScalar());

                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_Count");
                throw exception;
            }
        }

        /// <summary>
        /// Trae los registros de una maestra 
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de página</param>
        /// <param name="consulta">Filtros</param>
        /// <param name="FiltrosExtra">Filtros de codigo y descripción</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Devuelve los registros de una maestra</returns>
        public IEnumerable<DTO_MasterBasic> DAL_MasterSimple_GetPaged(long pageSize, int pageNum, DTO_glConsulta consulta, List<DTO_glConsultaFiltro> FiltrosExtra, bool? active)
        {
            try
            {
                long ini = (pageNum - 1) * pageSize + 1;
                long fin = pageNum * pageSize;

                List<DTO_MasterBasic> dtos = new List<DTO_MasterBasic>();
                string andActivo = string.Empty;

                string tablesFKs = string.Empty;
                string descFields = string.Empty;

                int cont = 1;
                int contUsuario = 1;

                foreach (ForeignKeyField fk in this._foreignKeys)
                {
                    DTO_aplMaestraPropiedades p = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, fk.FKDocumentoID, this.loggerConnectionStr);
                    bool fComp = p.GrupoEmpresaInd;

                    string fkAlias = "fk" + cont.ToString();
                    descFields += "," + fkAlias + "." + fk.ForeignDescField + " as " + fk.LocalDescField;

                    if (fk.ForeignField == "UsuarioID")
                    {
                        descFields += "," + fkAlias + ".UsuarioID UsuarioID" + contUsuario;
                        contUsuario++;
                        tablesFKs += " left join " + fk.TableName + " " + fkAlias + " with(nolock) on baseTable." + fk.NombreColumna + " = " + fkAlias + ".ReplicaID";
                    }
                    else
                        tablesFKs += " left join " + fk.TableName + " " + fkAlias + " with(nolock) on baseTable." + fk.NombreColumna + " = " + fkAlias + "." + fk.ForeignField;

                    if (fComp)
                    {
                        string egFk = (fk.FKNombreTabla.Equals(this._tableName) ? "EmpresaGrupoID" : (EgFkPrefix + fk.FKNombreTabla));
                        tablesFKs += " and baseTable." + egFk + " = " + fkAlias + ".EmpresaGrupoID";
                    }

                    cont++;
                }

                if (active != null)
                    andActivo = " AND baseTable.ActivoInd = " + Convert.ToInt16(active);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = string.Empty;

                if (this._hasCompany)
                {
                    query =
                    "SELECT baseTable.*" + descFields +
                    " FROM " + this._tableName + " baseTable with(nolock) " + tablesFKs +
                    " WHERE baseTable.EmpresaGrupoID = @EmpresaGrupoID " + andActivo;

                    mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                    mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.EmpresaGrupoID;
                }
                else
                {
                    query =
                      "SELECT baseTable.*" + descFields +
                      " FROM " + this._tableName + " baseTable with(nolock) " + tablesFKs +
                      " WHERE 1 = 1 " + andActivo;
                }

                ConsultasFields fieldId = new ConsultasFields()
                {
                    Field = this._colId,
                    Tipo = (this.DocumentID == AppMasters.glDocumento) ? typeof(int) : typeof(string)
                };

                List<ConsultasFields> listCF = new List<ConsultasFields>();
                listCF.Add(fieldId);

                if (consulta != null)
                {
                    foreach (DTO_glConsultaFiltro fil in consulta.Filtros)
                    {
                        if (fil.CampoFisico == "ID")
                            fil.CampoFisico = this._colId;
                    }
                }

                string where = (consulta == null || consulta.Filtros == null) ? string.Empty : Transformer.WhereSql(consulta.Filtros, this._dtoType, listCF);
                string where2 = (FiltrosExtra == null) ? string.Empty : Transformer.WhereSql(FiltrosExtra, this._dtoType, listCF);

                if (!string.IsNullOrWhiteSpace(where2))
                {
                    if (!string.IsNullOrWhiteSpace(where))
                        where = "(" + where + ") AND (" + where2 + ")";
                    else
                        where = where2;
                }

                if (!string.IsNullOrWhiteSpace(where))
                    query = "SELECT * FROM (" + query + ") resultTable WHERE (" + where + ") ";

                string orderby = this._colId;

                if (consulta != null && consulta.Selecciones != null)
                {
                    List<DTO_glConsultaSeleccion> seleccionestmp = new List<DTO_glConsultaSeleccion>(consulta.Selecciones);
                    seleccionestmp = seleccionestmp.Where(x => (x.OrdenIdx > 0 && (x.OrdenTipo.Equals("ASC") || x.OrdenTipo.Equals("DESC")))).OrderBy(x => x.OrdenIdx).ToList();
                    if (seleccionestmp.Count > 0)
                        orderby = string.Empty;
                    foreach (DTO_glConsultaSeleccion sel in seleccionestmp)
                    {
                        string campoSel = sel.CampoFisico;
                        if (campoSel.Equals("ID"))
                            campoSel = this._colId;
                        if (!string.IsNullOrWhiteSpace(orderby))
                            orderby += ",";
                        orderby += campoSel + " " + sel.OrdenTipo;
                    }
                }

                query = "SELECT ROW_NUMBER()Over(Order by " + orderby + ") As RowNum,* FROM (" + query + ") resultTable ";
                query = "SELECT * FROM (" + query + ") tempRes WHERE RowNum BETWEEN " + ini + " AND " + fin;

                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_MasterBasic dto = this.CreateDto(dr, props, false);
                    dtos.Add(dto);
                }
                dr.Close();

                return dtos;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_GetPaged");
                throw exception;
            }
        }

        /// <summary>
        ///  Adiciona una lista a la maestra básica
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <param name="bItems">Lista de registros</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="batchProgress">Progreso de la operacion "usuario,progreso"</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult DAL_MasterSimple_Add(byte[] bItems, int accion, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base.MySqlConnectionTx = base.MySqlConnection.BeginTransaction();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, this.DocumentID); 
            
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            DTO_TxResult resultTotal = new DTO_TxResult();
            resultTotal.Details = new List<DTO_TxResultDetail>();

            try
            {
                List<DTO_MasterBasic> items = CompressedSerializer.Decompress<List<DTO_MasterBasic>>(bItems);

                if (items == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "ERR_AGN_0007";
                }
                else
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        try
                        {
                            int percent = ((i + 1) * 100) / items.Count;
                            batchProgress[tupProgress] = percent;

                            DTO_TxResultDetail txD = this.DAL_MasterSimple_AddItem(items[i]);
                            txD.line = i + 1;
                            txD.Key = items[i].ID.ToString();

                            resultTotal.Details.Add(txD);
                            result.Details.Add(txD);

                            if (txD != null && txD.DetailsFields.Count > 0)
                            {
                                result.Result = ResultValue.NOK;
                            }
                        }
                        catch (Exception ex)
                        {
                            result.Result = ResultValue.NOK;
                            DTO_TxResultDetail txD = new DTO_TxResultDetail();
                            txD.line = i + 1;
                            txD.Key = items[i].ID.ToString();

                            var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                            txD.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterBasic (" + this._tableName + ") _Add");
                            result.Details.Add(txD);
                        }
                    }

                    if (result.Result.Equals(ResultValue.OK))
                    {
                        foreach (DTO_MasterBasic gr in items)
                        {
                            DAL_aplBitacora bt = new DAL_aplBitacora(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            bt.DAL_aplBitacora_Add(this.Empresa.ID.Value, this.DocumentID, Convert.ToInt16(Math.Pow(2, accion)), DateTime.Now, this.UserId, gr.ID.Value, 
                                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);
                        }

                        result.Result = ResultValue.OK;
                    }
                    else
                    {
                        result.Result = ResultValue.NOK;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;

                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_Add"); 
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base.MySqlConnectionTx.Commit();
                }
                else
                {
                    if (base.MySqlConnectionTx != null && !insideAnotherTx)
                    {
                        base.MySqlConnectionTx.Rollback();
                    }
                }
            }
        }

        /// <summary>
        /// Actualiza una maestra básica
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult DAL_MasterSimple_Update(DTO_MasterBasic item, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base.MySqlConnectionTx = base.MySqlConnection.BeginTransaction();

            //Objeto respuesta
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail txD = new DTO_TxResultDetail();
            List<DTO_TxResultDetailFields> txdfs = new List<DTO_TxResultDetailFields>();

            bool validUpdate = true;
            try
            {
                result.Result = ResultValue.OK;

                //Traer el result existente
                DTO_MasterBasic pf = this.DAL_MasterSimple_GetByReplicaID(item.ReplicaID.Value.Value);

                //Consultar por el id para determinar si existe
                if (pf == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.InvalidCode;
                    return result;
                }

                //Determinar los campos que cambian
                txdfs = this.GetDiferentFields(pf, item);

                //Verificar las versiones de result
                if (pf.CtrlVersion.Value < item.CtrlVersion.Value)
                {
                    //Error catatrofico de datos inconsistencia de datos
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Data; //msg3
                    return result;
                }
                else if (pf.CtrlVersion.Value > item.CtrlVersion.Value)
                {
                    //Baje la ultima versión
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_UpdateGrid; //msg4
                    return result;
                }

                //Iguales debe seguir
                //Incrementar la versión
                pf.CtrlVersion.Value = Convert.ToInt16(pf.CtrlVersion.Value + 1);

                try
                {
                    if (pf.EmpresaGrupoID != item.EmpresaGrupoID)
                    {
                        pf.EmpresaGrupoID = item.EmpresaGrupoID;
                    }
                    if (pf.ID != item.ID)
                    {
                        pf.ID = item.ID;
                    }
                    if (pf.Descriptivo != item.Descriptivo)
                    {
                        pf.Descriptivo = item.Descriptivo;
                    }
                    if (pf.ActivoInd != item.ActivoInd)
                    {
                        pf.ActivoInd = item.ActivoInd;
                    }

                    txD = this.DAL_MasterSimple_UpdateItem(item);
                    txD.line = 1;
                    txD.Key = item.ID.ToString();
                    result.Details.Add(txD);

                    if (txD.DetailsFields.Count > 0)
                    {
                        validUpdate = false;
                    }
                    else
                    {
                        txD.DetailsFields = txdfs;
                    }
                }
                catch (Exception ex)
                {

                    result.ResultMessage = "Error ";
                    result.Result = ResultValue.NOK;

                    result.Details = new List<DTO_TxResultDetail>();

                    txD.line = 1;
                    txD.Key = item.ID.ToString();

                    var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                    txD.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple_Update");
                    result.Details.Add(txD);
                }

                if (result.Result.Equals(ResultValue.OK) && result.Details.Count > 0 && validUpdate)
                {
                    foreach (DTO_TxResultDetail lgB in result.Details)
                    {
                        DAL_aplBitacora bt = new DAL_aplBitacora(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        int bId = bt.DAL_aplBitacora_Add(this.Empresa.ID.Value, this.DocumentID, Convert.ToInt16(Math.Pow(2, Convert.ToInt32(FormsActions.Edit))),
                            System.DateTime.Now, this.UserId, lgB.Key, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);

                        if (lgB.DetailsFields != null && lgB.DetailsFields.Count > 0)
                        {
                            foreach (DTO_TxResultDetailFields field in lgB.DetailsFields)
                            {
                                DAL_aplBitacoraAct btAct = new DAL_aplBitacoraAct(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                                btAct.DAL_aplBitacoraAct_Add(bId, this.DocumentID, field.Field, field.OldValue);
                            }
                        }
                    }

                    result.Result = ResultValue.OK;
                    result.Details = null;
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;

                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_Update");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK && validUpdate)
                {
                    if (!insideAnotherTx)
                        base.MySqlConnectionTx.Commit();
                }
                else
                {
                    if (!validUpdate)
                        result.Result = ResultValue.NOK;

                    if (base.MySqlConnectionTx != null && !insideAnotherTx)
                    {
                        base.MySqlConnectionTx.Rollback();
                    }
                }
            }
        }

        /// <summary>
        /// Borra una maestra básica a partir de su id
        /// </summary>
        /// <param name="id">Identificador de la maestra</param>
        /// <returns>Resultado</returns>  
        public DTO_TxResult DAL_MasterSimple_Delete(UDT_BasicID id, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base.MySqlConnectionTx = base.MySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                //Seleccionar los registros a borrar para tenerlos en el objeto de respuesta
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandText =
                "SELECT " + this._colId + " FROM " + this._tableName + " with(nolock) " +
                "WHERE " + this._colId + " = @ID";

                if (this._hasCompany)
                {
                    mySqlCommandSel.CommandText += " AND EmpresaGrupoID = @EmpresaGrupoID";
                    mySqlCommandSel.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                    mySqlCommandSel.Parameters["@EmpresaGrupoID"].Value = this.EmpresaGrupoID;
                }

                mySqlCommandSel.Parameters.Add("@ID", SqlDbType.Char, id.MaxLength);
                mySqlCommandSel.Parameters["@ID"].Value = id.Value;

                SqlDataReader drSel;
                drSel = mySqlCommandSel.ExecuteReader();
                while (drSel.Read())
                {
                    //Meterlos en el Result
                    DTO_TxResultDetail txD = new DTO_TxResultDetail();
                    txD.Key = drSel[this._colId].ToString();
                    result.Details.Add(txD);
                }
                drSel.Close();

                //Borrar los registros a borrar para tenerlos en el objeto de respuesta
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText = "DELETE FROM " + this._tableName + " " + "WHERE " + this._colId + " = @ID ";

                if (this._hasCompany)
                {
                    mySqlCommand.CommandText += " AND EmpresaGrupoID = @EmpresaGrupoID";
                    mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                    mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.EmpresaGrupoID;
                }

                mySqlCommand.Parameters.Add("@ID", SqlDbType.Char, id.MaxLength);
                mySqlCommand.Parameters["@ID"].Value = id.Value;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                dr.Close();

                result.Result = ResultValue.OK;

                if (result.Result.Equals(ResultValue.OK) && result.Details.Count > 0)
                {
                    foreach (DTO_TxResultDetail dtl in result.Details)
                    {
                        DAL_aplBitacora bt = new DAL_aplBitacora(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        bt.DAL_aplBitacora_Add(this.Empresa.ID.Value, this.DocumentID, Convert.ToInt16(Math.Pow(2, Convert.ToInt32(FormsActions.Delete))),
                            System.DateTime.Now, this.UserId, dtl.Key, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);

                        result.Result = ResultValue.OK;
                    }
                }
                else
                {
                    result.Result = ResultValue.NOK;
                }


                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;

                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_Delete");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base.MySqlConnectionTx.Commit();
                }
                else
                {
                    if (base.MySqlConnectionTx != null && !insideAnotherTx)
                    {
                        base.MySqlConnectionTx.Rollback();
                    }
                }
            }
        }

        /// <summary>
        /// Exporta los registros de una maestra
        /// </summary>
        /// <param name="colsRsx">Nombres de las columnas con los recursos</param>
        /// <param name="consulta">Filtro</param>
        /// <param name="filtrosExtra">Filtro especial</param>
        /// <returns>Retorna el nombre del archivo</returns>
        public string DAL_MasterSimple_Export(string colsRsx, DTO_glConsulta consulta, List<DTO_glConsultaFiltro> filtrosExtra)
        {
            try
            {
                string fileName;
                string str = this.GetCvsName(out fileName);
                long count = this.DAL_MasterSimple_Count(consulta, filtrosExtra, null);
                List<DTO_MasterBasic> list = this.DAL_MasterSimple_GetPaged(count, 1, consulta, filtrosExtra, null).ToList();
                CsvExport<DTO_MasterBasic> csv = new CsvExport<DTO_MasterBasic>(list, this._dtoType);

                #region Carga la info de las columnas

                List<string> importableCols = new List<string>();

                //Codigo y descripcion
                importableCols.Add("ID");
                if (this.DocumentID != (int)AppMasters.coTercero)
                    importableCols.Add("Descriptivo");

                //Campos Extras
                this.props.Campos.ForEach(f =>
                {
                    if (f.ImportacionInd)
                        importableCols.Add(f.NombreColumna);
                });

                //Activo
                importableCols.Add("ActivoInd");

                #endregion

                csv.ExportToFile_Master(str, colsRsx, importableCols);

                return fileName;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_Export");
                return string.Empty;
            }
        }

        /// <summary>
        /// Trae el numero de la fila de un ID
        /// </summary>
        /// <param name="id">Idenfiticador de la maestra</param>
        /// <param name="consulta">Filtro</param>
        /// <param name="active">Indicador de activo</param>
        /// <returns>Devuelve el número de fila ordenando por id</returns>
        public long DAL_MasterSimple_Rownumber(UDT_BasicID id, DTO_glConsulta consulta, bool? active)
        {
            try
            {
                string andActivo = string.Empty;

                string tablesFKs = string.Empty;
                string descFields = string.Empty;

                int cont = 1;
                int contUsuario = 1;

                foreach (ForeignKeyField fk in this._foreignKeys)
                {
                    DTO_aplMaestraPropiedades p = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, fk.FKDocumentoID, this.loggerConnectionStr);
                    bool fComp = p.GrupoEmpresaInd;

                    string fkAlias = "fk" + cont.ToString();
                    descFields += "," + fkAlias + "." + fk.ForeignDescField + " as " + fk.LocalDescField;

                    if (fk.ForeignField == "UsuarioID")
                    {
                        descFields += "," + fkAlias + ".UsuarioID UsuarioID" + contUsuario;
                        contUsuario++;
                        tablesFKs += " left join " + fk.TableName + " " + fkAlias + " with(nolock) on baseTable." + fk.NombreColumna + " = " + fkAlias + ".ReplicaID";
                    }
                    else
                    {
                        tablesFKs += " left join " + fk.TableName + " " + fkAlias + " with(nolock) on baseTable." + fk.NombreColumna + " = " + fkAlias + "." + fk.ForeignField;
                    }

                    if (fComp)
                    {
                        string egFk = (fk.FKNombreTabla.Equals(this._tableName) ? "EmpresaGrupoID" : (EgFkPrefix + fk.FKNombreTabla));
                        tablesFKs += " and baseTable." + egFk + " = " + fkAlias + ".EmpresaGrupoID";
                    }

                    cont++;
                }

                if (active != null)
                {
                    andActivo = " AND baseTable.ActivoInd = " + Convert.ToInt16(active);
                }

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = string.Empty;

                if (this._hasCompany)
                {
                    query =
                    "SELECT baseTable.*" + descFields +
                    " FROM " + this._tableName + " baseTable with(nolock) " + tablesFKs +
                    " WHERE baseTable.EmpresaGrupoID = @EmpresaGrupoID " + andActivo;

                    mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                    mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.EmpresaGrupoID;
                }
                else
                {
                    query =
                      "SELECT baseTable.*" + descFields +
                      " FROM " + this._tableName + " baseTable with(nolock) " + tablesFKs +
                      " WHERE 1 = 1 " + andActivo;
                }

                ConsultasFields fieldId = new ConsultasFields()
                {
                    Field = this._colId,
                    Tipo = (this.DocumentID == AppMasters.glDocumento) ? typeof(int) : typeof(string)
                };

                List<ConsultasFields> listCF = new List<ConsultasFields>();
                listCF.Add(fieldId);

                if (consulta != null)
                {
                    foreach (DTO_glConsultaFiltro fil in consulta.Filtros)
                    {
                        if (fil.CampoFisico == "ID")
                            fil.CampoFisico = this._colId;
                    }
                }

                string where = (consulta == null || consulta.Filtros == null) ? string.Empty : Transformer.WhereSql(consulta.Filtros, this._dtoType, listCF);

                if (!string.IsNullOrWhiteSpace(where))
                    query = "SELECT * FROM (" + query + ") resultTable WHERE (" + where + ") ";

                string orderby = this._colId;

                if (consulta != null && consulta.Selecciones != null)
                {
                    List<DTO_glConsultaSeleccion> seleccionestmp = new List<DTO_glConsultaSeleccion>(consulta.Selecciones);
                    seleccionestmp = seleccionestmp.Where(x => (x.OrdenIdx > 0 && (x.OrdenTipo.Equals("ASC") || x.OrdenTipo.Equals("DESC")))).OrderBy(x => x.OrdenIdx).ToList();
                    if (seleccionestmp.Count > 0)
                        orderby = string.Empty;
                    foreach (DTO_glConsultaSeleccion sel in seleccionestmp)
                    {
                        string campoSel = sel.CampoFisico;
                        if (campoSel.Equals("ID"))
                            campoSel = this._colId;
                        if (!string.IsNullOrWhiteSpace(orderby))
                            orderby += ",";
                        orderby += campoSel + " " + sel.OrdenTipo;
                    }
                }

                if (!string.IsNullOrEmpty(id.Value) && id.IsInt)
                {
                    mySqlCommand.Parameters.Add("@ID", SqlDbType.Int);
                    mySqlCommand.Parameters["@ID"].Value = Convert.ToInt32(id.Value);
                }
                else
                {
                    mySqlCommand.Parameters.Add("@ID", SqlDbType.Char, id.MaxLength);
                    mySqlCommand.Parameters["@ID"].Value = id.Value;
                }

                query = "SELECT ROW_NUMBER()Over(Order by " + orderby + ") As RowNum,* FROM (" + query + ") resultTable ";
                query = "SELECT RowNum FROM (" + query + ") tempRes WHERE "+this._colId+"=@ID";

                mySqlCommand.CommandText = query;

                long res = Convert.ToInt64(mySqlCommand.ExecuteScalar());

                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_RowNumber");
                throw exception;
            }
        }

        /// <summary>
        /// Metodo de TODAS las maestras para traer un campo de imagen
        /// </summary>
        /// <param name="replicaId">Replica Id de la fila a buscar</param>
        /// <param name="fieldName">Nombre del campo que contiene la imagen</param>
        /// <returns>arreglo de bytes con la imagen del logo</returns>
        public byte[] DAL_MasterSimple_GetImage(int replicaId, string fieldName)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "SELECT " + fieldName + " FROM " + this._tableName + " with(nolock) " +
                  "WHERE ReplicaID = @ReplicaID";

                mySqlCommand.Parameters.Add("@ReplicaID", SqlDbType.Int);
                mySqlCommand.Parameters["@ReplicaID"].Value = replicaId;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                const int bufferSize = 1000000;

                byte[] logo = new byte[bufferSize];

                while (dr.Read())
                {
                    if (!string.IsNullOrEmpty(dr[fieldName].ToString()))
                    {
                        try
                        {
                            logo = (byte[])dr[fieldName];
                        }
                        catch (SqlNullValueException sqlex)
                        {
                            logo = new byte[0];
                        }
                    }
                }
                dr.Close();

                return logo;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterSimple(" + this._tableName + ")_GetImage");
                throw exception;
            }
        }

        #endregion

    }
}
