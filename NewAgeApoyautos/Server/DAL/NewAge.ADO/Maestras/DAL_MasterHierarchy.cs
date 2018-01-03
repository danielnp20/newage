using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.UDT;
using System.Reflection;
using SentenceTransformer;

namespace NewAge.ADO
{
    public class DAL_MasterHierarchy : DAL_MasterSimple
    {
        #region Variables
        private int[] LevelsLength = new int[DTO_glTabla.MaxLevels];
        DTO_glTabla _table; 
        #endregion

        #region Propiedades

        /// <summary>
        /// Propiedad que asigna el nombre de la tabla
        /// </summary>
        public int DocumentID
        {
            get
            {
                return base.DocumentID;
            }
            set
            {
                base.DocumentID = value;
                DAL_glTabla DALtabla = new DAL_glTabla(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._table = DALtabla.DAL_glTabla_GetByTablaNombre(this._tableName, base.EmpresaGrupoID);
                IEnumerable<DTO_aplMaestraCampo> lista = (from c in props.Campos where c.NombreColumna.Equals(DummyMovInd.NombreColumna) select c);
                if (lista.Count() == 0)
                    this.props.Campos.Add(DummyMovInd);
            }
        }

        /// <summary>
        /// Campo de MovInd que se le agregan a todas las configuraciones jerarquicas
        /// </summary>
        private static DTO_aplMaestraCampo DummyMovInd = new DTO_aplMaestraCampo("UDT_SiNo")
        {
            ActualizableInd = true,
            CambiosEnCascada = false,
            EditableInd = false,
            GrillaEdicionInd = true,
            GrillaInd = true,
            ImportacionInd = false,
            NombreColumna = "MovInd",
            VacioInd = false
        }; 
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_MasterHierarchy(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn)
            : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Privadas

        /// <summary>
        /// Conveirte una lista de dtos de maestra basicos en dtos con jerarquia
        /// </summary>
        /// <param name="basics">Lista de maestras basicas</param>
        /// <returns>Retorna una lista de maestras jerarquicas</returns>
        private IEnumerable<DTO_MasterHierarchyBasic> ConvertFromBasic(IEnumerable<DTO_MasterBasic> basics)
        {
            List<DTO_MasterHierarchyBasic> result = new List<DTO_MasterHierarchyBasic>();
            foreach (DTO_MasterBasic basic in basics)
            {
                result.Add((DTO_MasterHierarchyBasic)basic);
            }
            return result;
        }

        /// <summary>
        /// Retorna una lista de campos que se deben aplicar en cascada dado un nivel de jerarquia
        /// </summary>
        /// <param name="level">Nivel del objeto que se esta cambiando</param>
        /// <returns>Lista de campos que se deben actualizar en lso hijos</returns>
        private List<DTO_aplMaestraCampo> CascadeFields(int level)
        {
            List<DTO_aplMaestraCampo> res = new List<DTO_aplMaestraCampo>();
            foreach (DTO_aplMaestraCampo mf in this._extraFields)
            {
                if (mf.CambiosEnCascada && mf.NivelJerarquia == level)
                    res.Add(mf);
            }
            return res;
        }

        /// <summary>
        /// Devuelve la longitud
        /// </summary>
        /// <param name="level">Nivel</param>
        /// <returns>Retorna la longitud del campo</returns>
        private int CodeLength(int level)
        {
            return _table.CodeLength(level);
        }

        /// <summary>
        /// Devuelve el numero del nivel al que corresponde un codigo
        /// Si el codigo no corresponde devuelve 0
        /// </summary>
        /// <param name="code">Codigo a evaluar</param>
        /// <returns>del 1 al 5 si corresponde con algun nivel de la jerarquia, 0 si no</returns>
        private int CodeLevel(string code)
        {
            return _table.LengthToLevel(code.Length);
        }

        #endregion

        #region Funciones Internal

        /// <summary>
        /// Obtiene los padres de una tabla jerarquica
        /// </summary>
        /// <param name="id">Identificador del registro</param>
        /// <returns>Retorna la lista de padres</returns>
        public List<string> GetParents(string id)
        {
            try
            {
                List<string> result = new List<string>();

                //Validar sus padres
                List<string> parts = Utility.CustomSplit(id, this._table.LevelLengths());
                string cId = string.Empty;
                for (int i = 0; i < parts.Count; ++i)
                {
                    cId += parts[i];
                    result.Add(cId);
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterHierarchy(" + this._tableName + ")_GetParents");
                throw exception;
            }
        }

        #endregion

        #region Funciones Públicas

        /// <summary>
        /// Trae la cantidad de hijos de un padre determinado
        /// </summary>
        /// <param name="parentId">id del padre</param>
        /// <param name="idFilter">filtro de id para los hijos</param>
        /// <param name="descFilter">filtro de descripcion</param>
        /// <param name="active">filtro de activo</param>
        /// <returns>Retorna la cantidad de hijos</returns>
        public long DAL_MasterHierarchy_CountChildren(UDT_BasicID parentId, string idFilter, string descFilter, bool? active, List<DTO_glConsultaFiltro> filtros)
        {
            try
            {
                string and = "";

                if (!idFilter.Equals(string.Empty))
                    and = " AND " + this._colId + " LIKE '%" + idFilter + "%'";
                
                if (!descFilter.Equals(string.Empty))
                    and = and + " AND Descriptivo LIKE '%" + descFilter + "%'";

                string andActivo = "";

                if (active != null)
                    andActivo = " AND ActivoInd = " + Convert.ToInt16(active);

                string andEmpresa = "";
                if (this._hasCompany)
                    andEmpresa = " AND EmpresaGrupoID='" + this.EmpresaGrupoID + "'";

                int childrenLength = 0;
                int parentLevel = this._table.LengthToLevel(parentId.Value.Length);
                int childLevel = parentLevel + 1;
                childrenLength = this._table.CodeLength(childLevel);
                //Total de Hijos
                long total = 0;
                string whereFiltros = string.Empty;
                if (filtros != null && filtros.Count > 0)
                {

                    string baseTable = "childs";
                    List<DTO_glConsultaFiltro> simples = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltroComplejo> complejos = new List<DTO_glConsultaFiltroComplejo>();
                    foreach (DTO_glConsultaFiltro fc in filtros)
                    {
                        if (fc is DTO_glConsultaFiltroComplejo)
                            complejos.Add(fc as DTO_glConsultaFiltroComplejo);
                        else
                            simples.Add(fc);
                    }
                    string whereSimples = Transformer.WhereSql(simples, this._dtoType);
                    if (!string.IsNullOrEmpty(whereSimples))
                        whereFiltros += " AND (" + whereSimples + ")";
                    int i = 1;
                    string whereComplejos = string.Empty;
                    foreach (DTO_glConsultaFiltroComplejo fc in complejos)
                    {
                        string fkTable = this._tableName + fc.DocumentoIDFK + "_" + i.ToString();
                        if (!string.IsNullOrWhiteSpace(whereComplejos))
                            whereComplejos += " AND ";
                        whereComplejos += WhereIn(fc, baseTable, fkTable);
                        i++;
                    }
                    if (!string.IsNullOrEmpty(whereComplejos))
                        whereFiltros += " AND (" + whereComplejos + ")";

                    string colIdBase = this._tableName + "." + this._colId;
                    string colIdChild = baseTable + "." + this._colId;
                    whereFiltros = " AND EXISTS(SELECT * FROM " + this._tableName + " " + baseTable + " with(nolock) WHERE 1=1 " + whereFiltros + " AND LEFT(" + colIdChild + ",LEN(RTRIM(" + colIdBase + ")))=RTRIM(" + colIdBase + "))";
                }

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                if (parentId.Value.Equals(string.Empty))
                {
                    //retorno todos los que tienen id logitud lon1
                    mySqlCommand.CommandText =
                        "SELECT COUNT (*) FROM " + this._tableName + " with(nolock) WHERE (LEN(" + this._colId + ") = @lon1) " + and + andActivo + andEmpresa + whereFiltros;

                    mySqlCommand.Parameters.Add("@lon1", SqlDbType.Int);
                    mySqlCommand.Parameters["@lon1"].Value = this._table.CodeLength(1);

                    total = Convert.ToInt64(mySqlCommand.ExecuteScalar());
                }
                else
                {
                    mySqlCommand.CommandText =
                        "SELECT Count(*) FROM " + this._tableName + " with(nolock) WHERE " + this._colId + " LIKE'" + parentId.Value + "_%' AND " + this._colId + " <> '" + parentId.Value + "' AND (LEN(" + this._colId + ") = " + childrenLength + ") " + and + andActivo + andEmpresa + whereFiltros;

                    total = Convert.ToInt64(mySqlCommand.ExecuteScalar());
                }

                return total;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterHierarchy(" + this._tableName + ")_CountChildren");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna una lista de hijos de una maestra de jerarquica
        /// </summary>
        /// <param name="pageSize">Número de registros por página</param>
        /// <param name="pageNum">Número de página</param>
        /// <param name="orderDirection">Ordenamiento</param>
        /// <param name="parentId">Identificador del padre</param>
        /// <param name="idFilter">Filtro para la columna del ID</param>
        /// <param name="descFilter">Filtro para la columna de la descripción</param>
        /// <param name="active">Indicador si se pueden traer solo datos activos</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Retorna la lista de resultados</returns>
        public IEnumerable<DTO_MasterBasic> DAL_MasterHierarchy_GetChindrenPaged(int pageSize, int pageNum, OrderDirection orderDirection, UDT_BasicID parentId, string idFilter, string descFilter, bool? active, List<DTO_glConsultaFiltro> filtros = null)
        {
            try
            {
                List<DTO_MasterBasic> items = new List<DTO_MasterBasic>();

                string and = "";

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                int ini = (pageNum - 1) * pageSize + 1;
                int fin = pageNum * pageSize;

                if (!idFilter.Equals(string.Empty))
                {
                    and = " AND baseTable." + this._colId + " LIKE'%" + idFilter + "%'";
                }
                if (!descFilter.Equals(string.Empty))
                {
                    and = and + " AND baseTable.Descriptivo LIKE'%" + descFilter + "%'";
                }

                string andActivo = "";

                if (active != null)
                {
                    andActivo = " AND basetable.ActivoInd = " + Convert.ToInt16(active);
                }

                string tablesFKs = string.Empty;
                string descFields = string.Empty;

                int cont = 1;
                foreach (ForeignKeyField fk in this._foreignKeys)
                {
                    DTO_aplMaestraPropiedades p = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, fk.FKDocumentoID, this.loggerConnectionStr);
                    bool fComp = p.GrupoEmpresaInd;

                    string fkAlias = "fk" + cont.ToString();
                    descFields += "," + fkAlias + "." + fk.ForeignDescField + " as " + fk.LocalDescField;

                    if (fk.TableName == "seUsuario")
                        tablesFKs += " left join " + fk.TableName + " " + fkAlias + " with(nolock) on baseTable." + fk.NombreColumna + " = " + fkAlias + ".ReplicaID";
                    else
                        tablesFKs += " left join " + fk.TableName + " " + fkAlias + " with(nolock) on baseTable." + fk.NombreColumna + " = " + fkAlias + "." + fk.ForeignField;

                    if (fComp)
                    {
                        string egFk = (fk.FKNombreTabla.Equals(this._tableName) ? "EmpresaGrupoID" : (EgFkPrefix + fk.FKNombreTabla));
                        tablesFKs += " and baseTable." + egFk + " = " + fkAlias + ".EmpresaGrupoID";
                    }

                    cont++;
                }

                int childrenLength = 0;
                int parentLevel = this._table.LengthToLevel(parentId.Value.Length);
                int childLevel = parentLevel + 1;
                childrenLength = this._table.CodeLength(childLevel);

                string andChildren = " AND baseTable." + this._colId + " LIKE'" + parentId.Value + "_%' AND baseTable." + this._colId + " <> '" + parentId.Value + "' AND (LEN(baseTable." + this._colId + ") = " + childrenLength + ")";
                string whereEmpresa = "1=1 ";

                if (this._hasCompany)
                {
                    whereEmpresa = "baseTable.EmpresaGrupoID = @EmpresaGrupoID ";
                    mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                    mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.EmpresaGrupoID;
                }

                string whereFiltros = string.Empty;
                List<string> colIdList = new List<string>();
                colIdList.Add(this._colId);

                if (filtros != null && filtros.Count > 0)
                {
                    string baseTable = "childs";
                    List<DTO_glConsultaFiltro> simples = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltroComplejo> complejos = new List<DTO_glConsultaFiltroComplejo>();
                    foreach (DTO_glConsultaFiltro fc in filtros)
                    {
                        if (fc is DTO_glConsultaFiltroComplejo)
                            complejos.Add(fc as DTO_glConsultaFiltroComplejo);
                        else
                            simples.Add(fc);
                    }
                    string whereSimples = Transformer.WhereSql(simples, this._dtoType);
                    if (!string.IsNullOrEmpty(whereSimples))
                        whereFiltros += " AND (" + whereSimples + ")";
                    int i = 1;
                    string whereComplejos = string.Empty;
                    foreach (DTO_glConsultaFiltroComplejo fc in complejos)
                    {
                        string fkTable = this._tableName + fc.DocumentoIDFK + "_" + i.ToString();
                        if (!string.IsNullOrWhiteSpace(whereComplejos))
                            whereComplejos += " AND ";
                        whereComplejos += WhereIn(fc, baseTable, fkTable);
                        i++;
                    }
                    if (!string.IsNullOrEmpty(whereComplejos))
                        whereFiltros += " AND (" + whereComplejos + ")";

                    string colIdBase = "baseTable." + this._colId;
                    string colIdChild = baseTable + "." + this._colId;
                    whereFiltros = " AND EXISTS(SELECT * FROM " + this._tableName + " with(nolock) " + baseTable + " WHERE 1=1 " + whereFiltros + " AND LEFT(" + colIdChild + ",LEN(RTRIM(" + colIdBase + ")))=RTRIM(" + colIdBase + "))";

                }

                mySqlCommand.CommandText = " SELECT * FROM ( " +
                "SELECT ROW_NUMBER()Over(Order by baseTable." + _colId + " " + orderDirection + ") As RowNum, baseTable.*" + descFields +
                " FROM " + this._tableName + " baseTable with(nolock) " + tablesFKs + " WHERE " + whereEmpresa + and + andActivo + andChildren + whereFiltros + " ) AS ResultadoPaginado  " +
                "WHERE RowNum BETWEEN @Ini AND @Fin";

                mySqlCommand.Parameters.Add("@Ini", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Fin", SqlDbType.Int);
                mySqlCommand.Parameters["@Ini"].Value = ini;
                mySqlCommand.Parameters["@Fin"].Value = fin;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    var item = this.CreateDto(dr, props, false);

                    items.Add(item);
                }
                dr.Close();

                return items;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterHierarchy(" + this._tableName + ")_GetChildrenPaged");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona una lista de dtos
        /// </summary>
        /// <param name="bItems">Lista de datos</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="progress">Información con el progreso de la transacción</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult DAL_MasterHierarchy_Add(byte[] bItems, int accion, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                List<DTO_MasterHierarchyBasic> items = CompressedSerializer.Decompress<List<DTO_MasterHierarchyBasic>>(bItems);

                if (items.Equals(null))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "ERR_AGN_0007";
                }
                else
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        int percent = ((i + 1) * 100) / items.Count;
                        batchProgress[tupProgress] = percent;

                        DTO_MasterHierarchyBasic item = items[i];
                        try
                        {
                            if (!this.DAL_MasterHierarchy_CheckParents(item.ID))
                            {
                                int longPadre = this.CodeLength(this.CodeLevel(item.ID.Value) - 1);
                                string padre = item.ID.Value.Substring(0, longPadre);
                                throw new MentorDataException("No existe el padre para el item dado", padre, null, string.Empty);
                            }

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
                            txD.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterHierarchy(" + this._tableName + ")_Add");
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterHierarchy(" + this._tableName + ")_Add");

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
        /// Actualiza un registro de una maestra jerarquica
        /// </summary>
        /// <param name="item">Registro para actualizar</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult DAL_MasterHierarchy_Update(DTO_MasterBasic item, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base.MySqlConnectionTx = base.MySqlConnection.BeginTransaction();

            //Objeto respuesta
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            List<DTO_aplMaestraCampo> cascada = this.CascadeFields(this.CodeLevel(item.ID.Value));
            List<DTO_MasterBasic> dtosToUpdate = new List<DTO_MasterBasic>();

            bool validUpdate = true;

            try
            {
                result.Result = ResultValue.OK;

                //Traer el result existente
                DTO_MasterBasic oldDto = this.DAL_MasterSimple_GetByReplicaID(item.ReplicaID.Value.Value);

                //Consultar por el id para determinar si existe
                if (oldDto == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.InvalidCode;
                    return result;
                }

                //Verificar las versiones de result
                if (oldDto.CtrlVersion.Value < item.CtrlVersion.Value)
                {
                    //Error catatrofico de datos inconsistencia de datos
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Data; ;
                    return result;
                }
                else if (oldDto.CtrlVersion.Value > item.CtrlVersion.Value)
                {
                    //Baje la ultima versión
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_UpdateGrid;
                    return result;
                }

                result.Result = ResultValue.OK;

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                string idLike = oldDto.ID.Value;
                if ((item.ActivoInd.Value != oldDto.ActivoInd.Value) || (!item.ID.Value.Equals(oldDto.ID.Value)))
                    idLike += "%";
                else
                {
                    foreach (DTO_aplMaestraCampo campoCascada in cascada)
                    {
                        object valueOld = null;
                        object valueNew = null;
                        PropertyInfo property = this._dtoType.GetProperty(campoCascada.NombreColumna);
                        if (property != null)
                        {
                            UDT oldUdt = (UDT)property.GetValue(oldDto, null);
                            UDT newUdt = (UDT)property.GetValue(item, null);
                            valueOld = property.PropertyType.GetProperty("Value").GetValue(oldUdt, null);
                            valueNew = property.PropertyType.GetProperty("Value").GetValue(newUdt, null);
                        }
                        else
                        {
                            FieldInfo field = this._dtoType.GetField(campoCascada.NombreColumna);
                            if (field != null)
                            {
                                UDT oldUdt = (UDT)field.GetValue(oldDto);
                                UDT newUdt = (UDT)field.GetValue(item);
                                valueOld = field.FieldType.GetProperty("Value").GetValue(oldUdt, null);
                                valueNew = field.FieldType.GetProperty("Value").GetValue(newUdt, null);
                            }
                        }
                        if ((valueOld != null && !valueOld.Equals(valueNew)) || (valueOld == null && valueNew != null))
                        {
                            idLike += "%";
                        }
                    }
                }

                if (this._hasCompany)
                {
                    mySqlCommandSel.CommandText = "SELECT * FROM " + this._tableName + " with(nolock) " +
                            "WHERE " + this._colId + " LIKE'" + idLike + "' AND EmpresaGrupoID=@EmpresaGrupoID";
                    mySqlCommandSel.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, 10);
                    mySqlCommandSel.Parameters["@EmpresaGrupoID"].Value = item.EmpresaGrupoID.Value;
                }
                else
                {
                    mySqlCommandSel.CommandText = "SELECT * FROM " + this._tableName + " with(nolock) " +
                            "WHERE " + this._colId + " LIKE'" + idLike + "'";
                }

                SqlDataReader drSel;
                drSel = mySqlCommandSel.ExecuteReader();
                while (drSel.Read())
                {
                    dtosToUpdate.Add(this.CreateDto(drSel, props, true));
                }
                drSel.Close();

                if (dtosToUpdate.Count == 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.InvalidCode;
                    return result;
                }

                validUpdate = true;

                //Recorre los DTOs que debe actualizar
                for (int i = 0; i < dtosToUpdate.Count; i++)
                {
                    DTO_TxResultDetail txD = new DTO_TxResultDetail();
                    List<DTO_TxResultDetailFields> txdfs = new List<DTO_TxResultDetailFields>();

                    //Cambiar el id
                    var len = item.ID.Value.Trim().Length;
                    var conservar = "";
                    if (len != dtosToUpdate[i].ID.Value.Length)
                    {
                        conservar = dtosToUpdate[i].ID.Value.Substring(len, dtosToUpdate[i].ID.Value.Length - len);
                    }
                    dtosToUpdate[i].ID.Value = item.ID.Value.Trim() + conservar;
                    string oldLGID = oldDto.ID.Value.Trim() + conservar;

                    dtosToUpdate[i].CtrlVersion.Value++;
                    item.CtrlVersion.Value++;

                    if (item.ID.Value.Equals(dtosToUpdate[i].ID.Value))
                    {
                        dtosToUpdate[i] = item;

                        //Determinar los campos que cambian
                        txdfs = GetDiferentFields(oldDto, item);
                    }
                    else
                    {
                        if (item.ActivoInd.Value != oldDto.ActivoInd.Value)
                        {
                            DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                            field.Field = "ActivoInd";
                            field.OldValue = Convert.ToInt16(oldDto.ActivoInd.Value).ToString();
                            field.NewValue = Convert.ToInt16(item.ActivoInd.Value).ToString();
                            txdfs.Add(field);
                            dtosToUpdate[i].ActivoInd = item.ActivoInd;
                        }

                        if (!dtosToUpdate[i].ID.Value.Trim().Equals(oldLGID.Trim()))
                        {
                            DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                            field.Field = this._colId;
                            field.OldValue = oldLGID;
                            field.NewValue = dtosToUpdate[i].ID.Value;
                            txdfs.Add(field);
                        }

                        foreach (DTO_aplMaestraCampo campoCascada in cascada)
                        {
                            PropertyInfo pi = item.GetType().GetProperty(campoCascada.NombreColumna);
                            UDT udtItem = null;
                            if (pi != null)
                            {
                                udtItem = (UDT)pi.GetValue(item, null);
                                pi.SetValue(dtosToUpdate[i], udtItem, null);
                            }
                            else
                            {
                                FieldInfo fi = item.GetType().GetField(campoCascada.NombreColumna);
                                if (fi != null)
                                {
                                    udtItem = (UDT)fi.GetValue(item);
                                    fi.SetValue(dtosToUpdate[i], udtItem);
                                }
                            }
                        }
                    }

                    try
                    {
                        txD = this.DAL_MasterSimple_UpdateItem(dtosToUpdate[i]);
                        txD.line = i + 1;
                        txD.Key = dtosToUpdate[i].ID.Value.ToString();
                        result.Details.Add(txD);

                        if (txD.DetailsFields != null && txD.DetailsFields.Count > 0)
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

                        txD.line = i + 1;
                        txD.Key = dtosToUpdate[i].ID.Value.ToString();

                        var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                        txD.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterHierarchy(" + this._tableName + ")_Update");
                        result.Details.Add(txD);
                    }

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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterHierarchy(" + this._tableName + ")_Update");

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
                        base.MySqlConnectionTx.Rollback();
                }
            }
        }

        /// <summary>
        /// Borra un registro
        /// </summary>
        /// <param name="id">Identificador del registro</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult DAL_MasterHierarchy_Delete(UDT_BasicID id, bool insideAnotherTx)
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
                "WHERE " + this._colId + " LIKE '" + id.Value + "%'";

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
                    txD.Key = drSel[this._colId].ToString();
                    result.Details.Add(txD);
                }
                drSel.Close();

                //Borrar los registros a borrar para tenerlos en el objeto de respuesta
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText =
                "DELETE FROM " + this._tableName + " " +
                "WHERE " + this._colId + " LIKE '" + id.Value + "%'";
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterHierarchy(" + this._tableName + ")_Delete");

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
        /// Valida si los padres de un id existen
        /// </summary>
        /// <param name="id">id del hijo</param>
        /// <returns>True si existen los padres</returns>
        public bool DAL_MasterHierarchy_CheckParents(UDT_BasicID id)
        {
            try
            {
                //Validar sus padres
                List<string> parts = Utility.CustomSplit(id.Value, this._table.LevelLengths());

                if (parts.Count == 0 || parts.Count == 1)
                {
                    // En este caso es un padre por eso se asume que debe adicioanrse
                    return true;
                }

                parts.RemoveRange(parts.Count() - 1, 1);//Quita el ultimo que corresponde al elemnto actual

                UDT_BasicID bid = new UDT_BasicID();
                bid.MaxLength = id.MaxLength;
                bid.Value = string.Join(string.Empty, parts);
                if (this.DAL_MasterSimple_GetByID(bid, false) == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterHierarchy(" + this._tableName + ")_ValidatePC");
                throw exception;
            }
        }

        #endregion

        #region Funciones WCF

        /// <summary>
        /// Completa la informacion de jerarquia a una maestra simple
        /// </summary>
        /// <param name="dto">Maestra</param>
        /// <returns>Retorna la maestra con la información de la jerarquia</returns>
        public DTO_MasterHierarchyBasic CompleteHierarchy(DTO_MasterBasic dto)
        {
            List<DTO_MasterBasic> list = new List<DTO_MasterBasic>();
            list.Add(dto);
            IEnumerable<DTO_MasterHierarchyBasic> aux = this.CompleteHierarchyList(list);
            return aux.First();
        }

        /// <summary>
        /// Completa la informacion de jerarquia a una lista de maestras simples
        /// </summary>
        /// <param name="list">lista de maestras</param>
        /// <returns>Retorna la lista de maestras con los datos de jerarquias</returns>
        public IEnumerable<DTO_MasterHierarchyBasic> CompleteHierarchyList(IEnumerable<DTO_MasterBasic> list)
        {
            List<DTO_MasterHierarchyBasic> listRes = new List<DTO_MasterHierarchyBasic>();
            IEnumerable<DTO_MasterHierarchyBasic> listCast = this.ConvertFromBasic(list);
            try
            {
                List<UDT_BasicID> toQuery = new List<UDT_BasicID>();
                int[] longTotal = this._table.CompleteLevelLengths();
                int lonLeaf = longTotal.Max();
                int cantidadNiveles = this._table.LevelsUsed();
                int maxIdLength;
                foreach (DTO_MasterHierarchyBasic dto in listCast)
                {
                    maxIdLength = dto.ID.MaxLength;
                    dto.Jerarquia = new DTO_hierarchy();

                    int[] arr = this._table.LevelLengths();
                    List<string> parts = Utility.CustomSplit(dto.ID.Value, arr);
                    dto.Jerarquia.NivelInstancia = parts.Count;
                    dto.Jerarquia.NivelesJerarquia = cantidadNiveles;
                    if (longTotal[parts.Count - 1] != dto.ID.Value.Length)
                    {
                        throw new MentorDataException("No contiene padre", dto.ID.Value, null, string.Empty);
                    }
                    int count = 1;
                    string codIncr = "";
                    foreach (string cod in parts)
                    {
                        codIncr += cod;
                        dto.Jerarquia.Codigos[count - 1] = cod;
                        UDT_BasicID bId = new UDT_BasicID();
                        bId.MaxLength = maxIdLength;
                        bId.Value = codIncr;
                        List<DTO_MasterBasic> localSearch = list.Where(x => x.ID.Value == bId.Value).ToList();
                        DTO_MasterHierarchyBasic tempDto = null;
                        if (localSearch.Count > 0)
                        {
                            tempDto = (DTO_MasterHierarchyBasic)localSearch.First();
                            dto.Jerarquia.Descripciones[count - 1] = tempDto.Descriptivo.Value;
                        }
                        else
                        {
                            if ((toQuery.Where(x => x.Value == bId.Value).Count()) == 0)
                                toQuery.Add(bId);
                        }
                        dto.MovInd.Value = (dto.ID.Value.Length == lonLeaf);
                        count++;
                    }
                    listRes.Add(dto);
                }
                foreach (UDT_BasicID codQuery in toQuery)
                {
                    List<DTO_MasterBasic> localSearch = list.Where(x => x.ID.Value == codQuery.Value).ToList();
                    DTO_MasterHierarchyBasic tempDto = (DTO_MasterHierarchyBasic)this.DAL_MasterSimple_GetByID(codQuery, false);

                    if (tempDto == null)
                    {
                        // mi papa no existe exception
                        throw new Exception("ERR_DAT_0025&&" + codQuery);
                    }

                    if (list != null)
                    {
                        foreach (DTO_MasterHierarchyBasic dto in listRes)
                        {
                            for (int i = 0; i < _table.LevelsUsed(); i++)
                            {
                                int level = i + 1;
                                if (codQuery.Value.Length == this._table.CodeLength(level))
                                {
                                    string[] temp = new string[DTO_glTabla.MaxLevels];
                                    Array.Copy(dto.Jerarquia.Codigos, temp, i + 1);
                                    string codDto = string.Concat(temp);
                                    if (codDto.Trim().Equals(codQuery.Value))
                                        dto.Jerarquia.Descripciones[i] = tempDto.Descriptivo.Value;
                                }
                            }
                        }
                    }
                }
                return listRes;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_HierarchyData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MasterHierarchy(" + this._tableName + ")_CompleteHierarchyList");
                throw exception;
            }
        }

        #endregion

    }
}
