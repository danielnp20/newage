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
using SentenceTransformer;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO 
{
    public class DAL_MasterComplex : DAL_MasterSimple
    {
        #region Variables
        private Dictionary<string, string> _pkFields;
        private string _idsSelect;

        SqlConnection ct;
        SqlTransaction txt;
        #endregion

        #region Propiedades

        /// <summary>
        /// Propiedad que asigna el codigo de la tabla
        /// </summary>
        public int DocumentID
        {
            get
            {
                return base.DocumentID;
            }
            set
            {
                this._idsSelect = string.Empty;
                this._pkFields = new Dictionary<string, string>();
                
                base.DocumentID = value;
                foreach (DTO_aplMaestraCampo mf in this._extraFields)
                {
                    if (mf.PKInd)
                    {
                        _idsSelect += mf.NombreColumna + ",";
                        this._pkFields.Add(mf.NombreColumna, string.Empty);
                    }
                }
                this._idsSelect = this._idsSelect.Remove(_idsSelect.LastIndexOf(','));
            }
        }

        /// <summary>
        /// Retorna la lista de llaves primarias
        /// </summary>
        public virtual Dictionary<string, string> Pks
        {
            get { return this._pkFields; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_MasterComplex(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn)
            : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Privadas

        /// <summary>
        /// Retorna una lista de DTO_TxResultDetailFields 
        /// </summary>
        /// <param name="oldGp">Gp antes de consultado</param>
        /// <param name="newGp">Gp dado</param>
        /// <returns>Lista de DTO_TxResultDetailFields</returns>
        private List<DTO_TxResultDetailFields> GetDiferentFields(DTO_MasterComplex oldGp, DTO_MasterComplex newGp)
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
            if (oldGp.ActivoInd.Value != newGp.ActivoInd.Value)
            {
                DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                field.Field = "ActivoInd";
                field.OldValue = oldGp.ActivoInd.Value.ToString();
                field.NewValue = newGp.ActivoInd.Value.ToString();
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

            foreach (DTO_aplMaestraCampo mf in this._extraFields)
            {
                if (mf.ActualizableInd)
                {
                    object valueOld = null;
                    object valueNew = null;
                    PropertyInfo property = this._dtoType.GetProperty(mf.NombreColumna);
                    if (property != null)
                    {
                        UDT oldUdt = (UDT)property.GetValue(oldGp, null);
                        UDT newUdt = (UDT)property.GetValue(newGp, null);
                        valueOld = property.PropertyType.GetProperty("Value").GetValue(oldUdt, null);
                        valueNew = property.PropertyType.GetProperty("Value").GetValue(newUdt, null);
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

            return response;
        }

        /// <summary>
        /// Crea un dto a partir de el data reader y las porpiedades de la maestra
        /// </summary>
        /// <param name="dr">datareader</param>
        /// <param name="props">maestra</param>
        /// <returns></returns>
        private DTO_MasterComplex CreateDto(IDataReader dr, DTO_aplMaestraPropiedades props, bool isReplica)
        {
            try
            {
                return(DTO_MasterComplex)Activator.CreateInstance(this._dtoType, new object[] { dr, props, isReplica });
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_Data, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterComplex(" + this._tableName + ")_CreateDTO");
                return new DTO_MasterComplex();
            }
        }

        /// <summary>
        /// Genera el query (where) para una consulta teniedo en cuenta solo las llaves primarias
        /// </summary>
        /// <param name="mySqlCommand">Comando que va hacer el query</param>
        /// <param name="pks">Lista de llaver primarias y valores</param>
        /// <returns>Retirna el query para las llaves primarias (where)</returns>
        private string CreatePKQuery(SqlCommand mySqlCommand, Dictionary<string, string> pks)
        {
            string ids = string.Empty;
            int i = 0;
            //Llena la info del query para las PKs
            foreach (var pk in pks)
            {
                string nomCol = pk.Key;
                string paramID = "@ID" + i.ToString();

                ids += " baseTable." + nomCol + " = " + paramID;
                if (i != pks.Count - 1)
                    ids += " AND";

                if (nomCol == "DocumentoID" || nomCol == "seUsuarioID")
                    mySqlCommand.Parameters.Add(paramID, SqlDbType.Int);
                else if (nomCol == "Fecha" || nomCol == "Periodo" || nomCol == "PeriodoID" || nomCol == "DiasFestivoID" || nomCol == "PeriodoDoc")
                    mySqlCommand.Parameters.Add(paramID, SqlDbType.SmallDateTime);
                else if (nomCol == "Valor" || nomCol == "BaseUVT" || nomCol == "BaseUVTID")
                    mySqlCommand.Parameters.Add(paramID, SqlDbType.Decimal);
                else
                    mySqlCommand.Parameters.Add(paramID, SqlDbType.Char);

                if (nomCol == "seUsuarioID")
                {
                    try
                    {
                        int usrRep = Convert.ToInt32(pk.Value);
                        mySqlCommand.Parameters[paramID].Value = pk.Value;
                    }
                    catch (Exception)
                    {
                        DAL_seUsuario usrDAL = new DAL_seUsuario(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        DTO_seUsuario usrDTO = usrDAL.DAL_seUsuario_GetUserbyID(pk.Value);
                        mySqlCommand.Parameters[paramID].Value = usrDTO.ReplicaID.Value.Value;
                    }
                }
                else if (nomCol == "Fecha" || nomCol == "PeriodoID")
                {
                    DateTime dt = Convert.ToDateTime(pk.Value);
                    mySqlCommand.Parameters[paramID].Value = dt;
                }
                else
                {
                    mySqlCommand.Parameters[paramID].Value = pk.Value;
                }

                ++i;
            }

            return ids;
        }

        /// <summary>
        /// Obtiene las llaves primarias y sus valores para mostrarlos
        /// </summary>
        /// <param name="dto">Objeto con los valores</param>
        /// <returns>Devuelve un string con las llaver primarias y sus valores</returns>
        private string GetPkValues(DTO_MasterComplex dto)
        {
            string retVal = string.Empty;
            int cont = 1;
            foreach (var f in dto.PKValues)
            {
                retVal += f.Key + " [" + f.Value + "]";
                if (cont < dto.PKValues.Count)
                    retVal += " - ";
            }

            return retVal;
        }

        /// <summary>
        /// Devuelve un arreglo de strings a partir de un objeto de llave primaria multiple
        /// </summary>
        /// <param name="dto">Objeto con los valores</param>
        /// <returns>Retorna un arreglo de strings con los valores de las llaves primarias</returns>
        private string[] GetBitacoraPkValues(DTO_MasterComplex dto)
        {
            string[] ret = new string[6];
            int cont = 0;

            foreach (var p in dto.PKValues)
            {
                ret[cont] = p.Value;
                cont++;
            }

            for (int i = cont; i < 6; ++i)
            {
                ret[i] = string.Empty;
            }

            return ret;
        }

        /// <summary>
        /// Devuelve un arreglo de strings a partir de un objeto de llave primaria multiple
        /// </summary>
        /// <param name="pks">Lista con las llaves primarias</param>
        /// <returns>Retorna un arreglo de strings con los valores de las llaves primarias</returns>
        private string[] GetBitacoraPkValues(Dictionary<string, string> pks)
        {
            string[] ret = new string[6];
            int cont = 0;

            foreach (var p in pks)
            {
                ret[cont] = p.Value;
                cont++;
            }

            for (int i = cont; i < 6; ++i)
            {
                ret[i] = string.Empty;
            }

            return ret;
        }        

        /// <summary>
        ///Actualiza una maestra básica
        /// </summary>
        /// <param name="dto">MasterBasic</param>
        /// <returns>Retorna un detalle con el resultado de la operacion</returns>
        private DTO_TxResultDetail DAL_MasterComplex_UpdateItem(DTO_MasterComplex dto)
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
                int cont = 0;
                foreach (DTO_aplMaestraCampo mf in this._extraFields)
                {
                    cont++;
                    if (mf.ActualizableInd)
                    {
                        bool validExtraParam = true;

                        fields += ", " + mf.NombreColumna + "=" + "@" + mf.NombreColumna;
                        object value = null;
                        PropertyInfo pi = dto.GetType().GetProperty(mf.NombreColumna);
                        UDT udt = null;

                        #region saca el valor de la columna
                        if (pi != null)
                        {
                            udt = (UDT)pi.GetValue(dto, null);
                            value = udt.GetType().GetProperty("Value").GetValue(udt, null);
                        }
                        else
                        {
                            FieldInfo fi = dto.GetType().GetField(mf.NombreColumna);
                            if (fi != null)
                            {
                                udt = (UDT)fi.GetValue(dto);
                                value = udt.GetType().GetProperty("Value").GetValue(udt, null);
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

                                if (string.IsNullOrWhiteSpace(dto.EmpresaGrupoID.Value) && fkProp.GrupoEmpresaInd)
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
                                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterComplex(" + this._tableName + ")_UpdateItem");
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

                                if (this.DocumentID == AppMasters.glActividadPermiso)
                                {
                                    mySqlCommand.Parameters.Add("@" + mf.NombreColumna, SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                                    if (usrDTO == null || string.IsNullOrEmpty(usrDTO.IdName))
                                        mySqlCommand.Parameters["@" + mf.NombreColumna].Value = DBNull.Value;
                                    else
                                        mySqlCommand.Parameters["@" + mf.NombreColumna].Value = usrDTO.ID.Value;
                                }
                                else
                                {
                                    mySqlCommand.Parameters.Add("@" + mf.NombreColumna, SqlDbType.Int);
                                    if (usrDTO == null || string.IsNullOrEmpty(usrDTO.IdName))
                                        mySqlCommand.Parameters["@" + mf.NombreColumna].Value = DBNull.Value;
                                    else
                                        mySqlCommand.Parameters["@" + mf.NombreColumna].Value = usrDTO.ReplicaID.Value.Value;
                                }
                            }
                            else
                            {
                                SqlParameter param = this.GetParameter(mf, udt);
                                mySqlCommand.Parameters.Add(param);
                                mySqlCommand.Parameters[mf.NombreColumna].Value = (value == null || string.IsNullOrWhiteSpace(value.ToString())) ? DBNull.Value : value;
                            }
                        }
                    }
                    if (cont == this._extraFields.Count)
                    {
                        //fields = fields.Remove(fields.LastIndexOf(","));
                    }
                }
                #endregion
                #region Validacion de restricciones
                List<DTO_TxResultDetailFields> invRules = DTO_Validations.CheckRules(base.loggerConnectionStr, base.MySqlConnection, base.MySqlConnectionTx, dto, this.Empresa, this.EmpresaGrupoID, this.UserId, false);
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

                mySqlCommand.CommandText =
                  "UPDATE " + this._tableName +
                   " SET ActivoInd = @ActivoInd, CtrlVersion = @CtrlVersion " + fields +
                   "WHERE ReplicaID = @ReplicaID";

                if (validDTO)
                {
                    mySqlCommand.Parameters.Add("@ReplicaID", SqlDbType.Int);
                    mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.SmallInt);
                    mySqlCommand.Parameters.Add("@CtrlVersion", SqlDbType.SmallInt);

                    mySqlCommand.Parameters["@ReplicaID"].Value = dto.ReplicaID.Value;
                    mySqlCommand.Parameters["@ActivoInd"].Value = dto.ActivoInd.Value;
                    mySqlCommand.Parameters["@CtrlVersion"].Value = dto.CtrlVersion.Value;

                    mySqlCommand.ExecuteNonQuery();
                }

                return rd;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterComplex(" + this._tableName + ")_UpdateItem");
                throw exception;
            }
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        ///  Retorna una maestra compleja a partir del identificador
        /// </summary>
        /// <param name="replicaID">Identificador</param>
        /// <returns>Devuelve la maestra</returns>
        public DTO_MasterComplex DAL_MasterComplex_GetByReplicaID(int replicaID)
        {
            try
            {
                DTO_MasterComplex dto = null;

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterComplex(" + this._tableName + ")_GetByID");
                throw exception;
            }
        }
        
        /// <summary>
        /// Retorna una maestra básica a partir del identificador
        /// </summary>
        /// <param name="pks">Identificador de la maestra</param>
        /// <param name="EmpresaGrupoID">Identificador por el cual se filtra</param>
        /// <returns>Devuelve la maestra basica</returns>
        public DTO_MasterComplex DAL_MasterComplex_GetByID(Dictionary<string, string> pks, bool active)
        {
            try
            {
                DTO_MasterComplex result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string tablesFKs = string.Empty;
                string descFields = string.Empty;
                string ids = this.CreatePKQuery(mySqlCommand, pks);

                //Llena la info del query para las FKs
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
                        tablesFKs += " left join " + fk.TableName + " " + fkAlias + " with(nolock) on baseTable." + fk.NombreColumna + " = " + fkAlias + ".ReplicaID";
                        contUsuario++;
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
                "SELECT baseTable.*" + descFields + " FROM " + this._tableName + " baseTable with(nolock) " + tablesFKs + " WHERE" + ids;

                if (this._hasCompany)
                {
                    if (this._tableName != "seUsuarioGrupo")
                        mySqlCommand.CommandText += " AND baseTable.EmpresaGrupoID = @EmpresaGrupoID ";
                    else
                        mySqlCommand.CommandText += " AND baseTable.EmpresaID = @EmpresaID ";
                }

                if (active)
                    mySqlCommand.CommandText += " AND baseTable.ActivoInd = 1 ";

                if (this._hasCompany)
                {
                    if (this._tableName != "seUsuarioGrupo")
                    {
                        mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, UDT_EmpresaGrupoID.MaxLength);
                        mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.EmpresaGrupoID;
                    }
                    else
                    {
                        mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                        mySqlCommand.Parameters["@EmpresaID"].Value = this.EmpresaGrupoID;
                    }
                }

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterComplex(" + this._tableName + ")_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Cantidad de registros de una maestra
        /// </summary>
        /// <param name="consulta">Filtro</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Retorna el numero de registros de la consulta</returns>
        public long DAL_MasterComplex_Count(DTO_glConsulta consulta, bool? active)
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
                        if (this.DocumentID == AppMasters.glActividadPermiso)
                            tablesFKs += " left join " + fk.TableName + " " + fkAlias + " with(nolock) on baseTable." + fk.NombreColumna + " = " + fkAlias + ".UsuarioID";
                        else
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

                string where = (consulta == null || consulta.Filtros == null) ? string.Empty : Transformer.WhereSql(consulta.Filtros, this._dtoType);

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterComplex(" + this._tableName + ")_Count");
                throw exception;
            }
        }

        /// <summary>
        /// Trae los registros de una maestra compleja
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de página</param>
        /// <param name="consulta">Filtro</param>
        /// <returns>Devuelve los registros de una maestra compleja</returns>
        public IEnumerable<DTO_MasterComplex> DAL_MasterComplex_GetPaged(long pageSize, int pageNum, DTO_glConsulta consulta, bool? active)
        {
            try
            {
                long ini = (pageNum - 1) * pageSize + 1;
                long fin = pageNum * pageSize;

                List<DTO_MasterComplex> dtos = new List<DTO_MasterComplex>();
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
                        if (this.DocumentID == AppMasters.glActividadPermiso)
                            tablesFKs += " left join " + fk.TableName + " " + fkAlias + " with(nolock) on baseTable." + fk.NombreColumna + " = " + fkAlias + ".UsuarioID";
                        else
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

                string where = (consulta == null || consulta.Filtros == null) ? string.Empty : Transformer.WhereSql(consulta.Filtros, this._dtoType);

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
                    DTO_MasterComplex dto = this.CreateDto(dr, props, false);

                    dtos.Add(dto);
                }
                dr.Close();

                return dtos;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterComplex(" + this._tableName + ")_GetPaged");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona una lista a la maestra
        /// </summary>
        /// <param name="bItems">Lista de registros</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="progress">Progreso de insercion de los datos</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult DAL_MasterComplex_Add(byte[] bItems, int accion, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                List<DTO_MasterComplex> items = CompressedSerializer.Decompress<List<DTO_MasterComplex>>(bItems);

                if (items.Equals(null))
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

                            DTO_TxResultDetail txD = this.DAL_MasterComplex_AddItem(items[i]);
                            txD.line = i + 1;
                            txD.Key = this.GetPkValues(items[i]);

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
                            txD.Key = this.GetPkValues(items[i]);

                            var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                            txD.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterComplex(" + this._tableName + ")_Add");
                            result.Details.Add(txD);
                        }
                    }

                    if (result.Result.Equals(ResultValue.OK))
                    {
                        foreach (DTO_MasterComplex gr in items)
                        {
                            DAL_aplBitacora bt = new DAL_aplBitacora(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            bt.DAL_aplBitacora_Add(this.Empresa.ID.Value, this.DocumentID, Convert.ToInt16(Math.Pow(2, accion)), DateTime.Now, this.UserId,
                                this.GetBitacoraPkValues(gr), 0, 0, 0);
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterComplex(" + this._tableName + ")_Add");

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
        /// Adiciona una maestra básica
        /// </summary>
        /// <param name="dto">MasterBasic</param>
        /// <returns>Retorna un detalle con el resultado de la operacion</returns>
        public DTO_TxResultDetail DAL_MasterComplex_AddItem(DTO_MasterComplex dto)
        {
            try
            {
                bool validDTO = true;
                DTO_TxResultDetail rd = new DTO_TxResultDetail();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Variables para verificar la pk
                SqlCommand cmdPK = base.MySqlConnection.CreateCommand();
                cmdPK.Transaction = base.MySqlConnectionTx;
                Dictionary<string, string> udt_Pks = new Dictionary<string, string>();

                bool validatePK = true;

                string fields = "";
                string commandParams = "";

                //Mensajes de error
                string msgExistingItem = DictionaryMessages.PkInUse;
                string msgEmptyField = DictionaryMessages.EmptyField;
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                #region Validacion de nulls campos basicos
                if (!dto.ActivoInd.Value.HasValue)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "ActivoInd";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                #endregion
                #region validacion de campos extras
                foreach (DTO_aplMaestraCampo mf in this._extraFields)
                {
                    if (mf.ActualizableInd)
                    {
                        bool validExtraParam = true;

                        fields += ", " + mf.NombreColumna;
                        commandParams += ", @" + mf.NombreColumna;
                        object value = null;
                        PropertyInfo pi = dto.GetType().GetProperty(mf.NombreColumna);
                        UDT udt = null;

                        #region Saca el valor de la columna
                        if (pi != null)
                        {
                            udt = (UDT)pi.GetValue(dto, null);
                            value = udt.GetType().GetProperty("Value").GetValue(udt, null);
                        }
                        else
                        {
                            FieldInfo fi = dto.GetType().GetField(mf.NombreColumna);
                            if (fi != null)
                            {
                                udt = (UDT)fi.GetValue(dto);
                                value = udt.GetType().GetProperty("Value").GetValue(udt, null);
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

                                if (string.IsNullOrWhiteSpace(dto.EmpresaGrupoID.Value) && fkProp.GrupoEmpresaInd)
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
                                var exception = new Exception(DictionaryMessages.Err_Data, eFK);
                                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterComplex(" + this._tableName + ")_AddItem");
                                throw exception;
                            }
                        }
                        #endregion
                        #region Datos para la Pk
                        if (validatePK && mf.PKInd)
                        {
                            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                            {
                                validatePK = false;
                            }
                            else
                            {
                                //udt_Pks.Add(new UDT_BasicID() 
                                //{ 
                                //     IsInt = mf.DocumentoID == Masters.glDocumento ? true : false,
                                //     Value = value.ToString()
                                //});
                                udt_Pks.Add(mf.NombreColumna, value.ToString());
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

                                if (this.DocumentID == AppMasters.glActividadPermiso)
                                {
                                    mySqlCommand.Parameters.Add("@" + mf.NombreColumna, SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                                    if (usrDTO == null || string.IsNullOrEmpty(usrDTO.IdName))
                                        mySqlCommand.Parameters["@" + mf.NombreColumna].Value = DBNull.Value;
                                    else
                                        mySqlCommand.Parameters["@" + mf.NombreColumna].Value = usrDTO.ID.Value;
                                }
                                else
                                {
                                    mySqlCommand.Parameters.Add("@" + mf.NombreColumna, SqlDbType.Int);
                                    if (usrDTO == null || string.IsNullOrEmpty(usrDTO.IdName))
                                        mySqlCommand.Parameters["@" + mf.NombreColumna].Value = DBNull.Value;
                                    else
                                        mySqlCommand.Parameters["@" + mf.NombreColumna].Value = usrDTO.ReplicaID.Value.Value;
                                }
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
                #region Validacion Pk inexistente

                if (validatePK)
                {
                    string ids = this.CreatePKQuery(cmdPK, udt_Pks);
                    ids = ids.Replace("baseTable", this._tableName);

                    cmdPK.CommandText =
                    "SELECT " + this._idsSelect + " FROM " + this._tableName + " with(nolock) " +
                    "WHERE " + ids;

                    if (this._hasCompany)
                    {
                        cmdPK.CommandText += " AND EmpresaGrupoID = @EmpresaGrupoID";
                        cmdPK.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                        cmdPK.Parameters["@EmpresaGrupoID"].Value = dto.EmpresaGrupoID.Value;
                    }

                    object firstID = cmdPK.ExecuteScalar();

                    if (firstID != null)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = this._colId;
                        rdF.Message = msgExistingItem;
                        rd.DetailsFields.Add(rdF);

                        validDTO = false;
                    }

                }

                #endregion

                fields += " ";
                commandParams += " ";

                #region creacion del query
                if (!this._hasCompany)
                {
                    mySqlCommand.CommandText =
                      "INSERT INTO " + this._tableName + " (" +
                      " ActivoInd, CtrlVersion " + fields +
                      ") VALUES (" +
                      " @ActivoInd, @CtrlVersion " + commandParams +
                      ")";
                }
                else
                {
                    mySqlCommand.CommandText =
                      "INSERT INTO " + this._tableName + " (" +
                      "  EmpresaGrupoID, ActivoInd, CtrlVersion " + fields +
                      ") VALUES (" +
                      "  @EmpresaGrupoID, @ActivoInd, @CtrlVersion " + commandParams +
                      ")";


                    mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                    mySqlCommand.Parameters["@EmpresaGrupoID"].Value = dto.EmpresaGrupoID.Value;

                    if (string.IsNullOrEmpty(dto.EmpresaGrupoID.Value))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "EmpresaGrupoID";
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);

                        validDTO = false;
                    }
                }
                #endregion

                if (validDTO)
                {
                    mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.SmallInt);
                    mySqlCommand.Parameters.Add("@CtrlVersion", SqlDbType.SmallInt);

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
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterComplex(" + this._tableName + ")_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza una maestra
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult DAL_MasterComplex_Update(DTO_MasterComplex item, bool insideAnotherTx)
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
                DTO_MasterComplex pf = this.DAL_MasterComplex_GetByReplicaID(item.ReplicaID.Value.Value);

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
                    result.ResultMessage = DictionaryMessages.Err_Data;
                    return result;
                }
                else if (pf.CtrlVersion.Value > item.CtrlVersion.Value)
                {
                    //Baje la ultima versión
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_UpdateGrid;
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
                    if (pf.ActivoInd != item.ActivoInd)
                    {
                        pf.ActivoInd = item.ActivoInd;
                    }

                    txD = this.DAL_MasterComplex_UpdateItem(item);
                    txD.line = 1;
                    txD.Key = this.GetPkValues(item);
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
                    txD.Key = this.GetPkValues(item);

                    var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                    txD.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterComplex_Update");
                    result.Details.Add(txD);
                }

                if (result.Result.Equals(ResultValue.OK) && result.Details.Count > 0 && validUpdate)
                {
                    DAL_aplBitacora bt = new DAL_aplBitacora(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    int bId = bt.DAL_aplBitacora_Add(this.Empresa.ID.Value, this.DocumentID, Convert.ToInt16(Math.Pow(2, Convert.ToInt32(FormsActions.Edit))),
                        System.DateTime.Now, this.UserId, this.GetBitacoraPkValues(item), 0, 0, 0);

                    if (txD.DetailsFields != null && txD.DetailsFields.Count > 0)
                    {
                        foreach (DTO_TxResultDetailFields field in txD.DetailsFields)
                        {
                            DAL_aplBitacoraAct btAct = new DAL_aplBitacoraAct(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            btAct.DAL_aplBitacoraAct_Add(bId, this.DocumentID, field.Field, field.OldValue);
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DTO_MasterComplex (" + this._tableName + ")_Update");

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
        /// Borra una maestra a partir de su id
        /// </summary>
        /// <param name="pks">Llaves primarias de la maestra</param>
        /// <returns>Devuelve el resultado de la operacion</returns>  
        public DTO_TxResult DAL_MasterComplex_Delete(Dictionary<string, string> pks, bool insideAnotherTx )
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

                string ids = this.CreatePKQuery(mySqlCommandSel, pks);
                ids = ids.Replace("baseTable", this._tableName);

                mySqlCommandSel.CommandText =
                "SELECT " + this._idsSelect + " FROM " + this._tableName + " with(nolock) " +
                "WHERE " + ids;

                if (this._hasCompany)
                {
                    mySqlCommandSel.CommandText += " AND EmpresaGrupoID = @EmpresaGrupoID";
                    mySqlCommandSel.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                    mySqlCommandSel.Parameters["@EmpresaGrupoID"].Value = this.EmpresaGrupoID;
                }

                SqlDataReader drSel;
                drSel = mySqlCommandSel.ExecuteReader();
                while (drSel.Read())
                {
                    //Meterlos en el Result
                    DTO_TxResultDetail txD = new DTO_TxResultDetail();
                    txD.Key = this._idsSelect;
                    result.Details.Add(txD);
                }
                drSel.Close();

                //Borrar los registros a borrar para tenerlos en el objeto de respuesta
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string idsDel = this.CreatePKQuery(mySqlCommand, pks);
                idsDel = idsDel.Replace("baseTable", this._tableName);

                mySqlCommand.CommandText =
                "DELETE FROM " + this._tableName + " " +
                "WHERE " + idsDel;
                if (this._hasCompany)
                {
                    mySqlCommand.CommandText += " AND EmpresaGrupoID = @EmpresaGrupoID";
                    mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                    mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.EmpresaGrupoID;
                }

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                dr.Close();

                result.Result = ResultValue.OK;

                if (result.Result.Equals(ResultValue.OK) && result.Details.Count > 0)
                {
                    foreach (DTO_TxResultDetail dtl in result.Details)
                    {
                        DAL_aplBitacora bt = new DAL_aplBitacora(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        bt.DAL_aplBitacora_Add(this.Empresa.ID.Value, this.DocumentID, Convert.ToInt16(Math.Pow(2, Convert.ToInt32(FormsActions.Delete))), System.DateTime.Now, 
                            this.UserId, this.GetBitacoraPkValues(pks), 0, 0, 0);

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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_MasterComplex(" + this._tableName + ")_Delete");

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
        /// Borra una maestra a partir de su id
        /// </summary>
        /// <param name="pks">Llaves primarias de la maestra</param>
        /// <returns>Devuelve el resultado de la operacion</returns>  
        public DTO_TxResult DAL_MasterComplex_DeleteAll(bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base.MySqlConnectionTx = base.MySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            try
            {             

                //Borrar los registros a borrar para tenerlos en el objeto de respuesta
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =  "DELETE FROM " + this._tableName;                

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                dr.Close();
                result.Result = ResultValue.OK;                

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_MasterComplex(" + this._tableName + ")_DeleteAll");

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
        public string DAL_MasterComplex_Export(string colsRsx, DTO_glConsulta consulta)
        {
            try
            {
                string fileName;
                string str = this.GetCvsName(out fileName);
                long count = this.DAL_MasterComplex_Count(consulta, null);
                List<DTO_MasterComplex> list = this.DAL_MasterComplex_GetPaged(count, 1, consulta, null).ToList();
                CsvExport<DTO_MasterComplex> csv = new CsvExport<DTO_MasterComplex>(list, this._dtoType);

                #region Carga la info de las columnas

                List<string> importableCols = new List<string>();
                List<string> importableColsDesc = new List<string>();
                this.props.Campos.ForEach(f =>
                {
                    if (f.ImportacionInd)
                        importableCols.Add(f.NombreColumna);
                    else if (f.Tipo == "UDT_Descriptivo") // Columnas paar los descriptivos
                        importableColsDesc.Add(f.NombreColumna);

                });

                //Activo
                importableCols.Add("ActivoInd");
                importableCols.AddRange(importableColsDesc);

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
        /// Trae el numero de la fila de una lista de Pks
        /// </summary>
        /// <param name="pks">Idenfiticador de la maestra</param>
        /// <param name="consulta">Filtro</param>
        /// <param name="active">Indicador de activo</param>
        /// <returns>Devuelve el número de fila ordenando por id</returns>
        public long DAL_MasterComplex_Rownumber(Dictionary<string, string> pks, DTO_glConsulta consulta, bool? active)
        {
            try
            {
                List<DTO_MasterComplex> dtos = new List<DTO_MasterComplex>();
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
                        if (this.DocumentID == AppMasters.glActividadPermiso)
                            tablesFKs += " left join " + fk.TableName + " " + fkAlias + " with(nolock) on baseTable." + fk.NombreColumna + " = " + fkAlias + ".UsuarioID";
                        else
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
                {
                    andActivo = " AND ActivoInd = " + Convert.ToInt16(active);
                }

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string ids = this.CreatePKQuery(mySqlCommand, pks);
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

                string where = (consulta == null || consulta.Filtros == null) ? string.Empty : Transformer.WhereSql(consulta.Filtros, this._dtoType);

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

                query = "SELECT ROW_NUMBER()Over(Order by " + orderby + ") As RowNum,* FROM (" + query + ") baseTable ";
                query = "SELECT RowNum FROM (" + query + ") baseTable WHERE " + ids;

                mySqlCommand.CommandText = query;

                long total = Convert.ToInt64(mySqlCommand.ExecuteScalar());

                return total;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterComplex(" + this._tableName + ")_RowNumber");
                throw exception;
            }
        }

        #endregion

    }
}
