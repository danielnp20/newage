using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;
using System.ComponentModel;
using System.Reflection;
using System.Data.SqlTypes;

namespace NewAge.ADO
{
    public class DAL_noLiquidacionPreliminar : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_noLiquidacionPreliminar(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Adiciona un registro a la tabla noLiquidacionPreliminar
        /// </summary>
        /// <param name="detalle">detalle liquidacion</param>
        /// <returns>true si la transaccion es exitosa</returns>
        public void DAL_noLiquidacionPreliminar_Add(DTO_noLiquidacionPreliminar detalle)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  INSERT INTO noLiquidacionesDetalle    " +
                                               "  ([EmpresaID]    " +
                                               "  ,[NumeroDoc]    " +
                                               "  ,[ConceptoNOID]    " +
                                               "  ,[Dias]    " +
                                               "  ,[Base]    " +
                                               "  ,[Valor]    " +
                                               "  ,[OrigenConcepto]    " +
                                               "  ,[FondoNOID]    " +
                                               "  ,[ContratoNONovID]    " +
                                               "  ,[Numero]    " +
                                               "  ,[eg_noConceptoNOM]    " +
                                               "  ,[eg_noFondo]    " +
                                               "  ,[eg_noContratoNov]    " +
                                               "  ,[DatoAdd1]    " +
                                               "  ,[DatoAdd2]    " +
                                               "  ,[DatoAdd3]    " +
                                               "  ,[DatoAdd4]    " +
                                               "  ,[DatoAdd5]    " +
                                               "  ,[ValorAdd1]    " +
                                               "  ,[ValorAdd2]    " +
                                               "  ,[ValorAdd3]    " +
                                               "  ,[ValorAdd4]    " +
                                               "  ,[ValorAdd5]    " +
                                               "  ,[Documento1]    " +
                                               "  ,[Documento2]    " +
                                               "  ,[Documento3]    " +
                                               "  ,[Documento4]    " +
                                               "  ,[Documento5])    " +
                                               "  VALUES    " +
                                               "  (@EmpresaID    " +
                                               "  ,@NumeroDoc    " +
                                               "  ,@ConceptoNOID    " +
                                               "  ,@Dias    " +
                                               "  ,@Base    " +
                                               "  ,@Valor    " +
                                               "  ,@OrigenConcepto    " +
                                               "  ,@FondoNOID    " +
                                               "  ,@ContratoNONovID    " +
                                               "  ,@Numero    " +
                                               "  ,@eg_noConceptoNOM    " +
                                               "  ,@eg_noFondo    " +
                                               "  ,@eg_noContratoNov    " +
                                               "  ,@DatoAdd1    " +
                                               "  ,@DatoAdd2    " +
                                               "  ,@DatoAdd3    " +
                                               "  ,@DatoAdd4    " +
                                               "  ,@DatoAdd5    " +
                                               "  ,@ValorAdd1    " +
                                               "  ,@ValorAdd2    " +
                                               "  ,@ValorAdd3    " +
                                               "  ,@ValorAdd4    " +
                                               "  ,@ValorAdd5    " +
                                               "  ,@Documento1    " +
                                               "  ,@Documento2    " +
                                               "  ,@Documento3    " +
                                               "  ,@Documento4    " +
                                               "  ,@Documento5)   ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConceptoNOID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Dias", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Base", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@OrigenConcepto", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FondoNOID", SqlDbType.Char, UDT_FondoNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ContratoNONovID", SqlDbType.Char, UDT_ContratoNONovID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Numero", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@eg_noConceptoNOM", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noFondo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noContratoNov", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@ValorAdd1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorAdd2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorAdd3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorAdd4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorAdd5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Documento1", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Documento2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Documento3", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Documento4", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Documento5", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = detalle.EmpresaID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = detalle.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ConceptoNOID"].Value = detalle.ConceptoNOID.Value;
                mySqlCommandSel.Parameters["@Dias"].Value = detalle.Dias.Value;
                mySqlCommandSel.Parameters["@Base"].Value = detalle.Base.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = detalle.Valor.Value;
                mySqlCommandSel.Parameters["@OrigenConcepto"].Value = detalle.OrigenConcepto.Value;
                mySqlCommandSel.Parameters["@FondoNOID"].Value = detalle.FondoNOID.Value;
                mySqlCommandSel.Parameters["@ContratoNONovID"].Value = detalle.ContratoNONovID.Value;
                mySqlCommandSel.Parameters["@Numero"].Value = detalle.Numero.Value;
                mySqlCommandSel.Parameters["@eg_noConceptoNOM"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConceptoNOM, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noFondo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noFondo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noContratoNov"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noContratoNov, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@DatoAdd1"].Value = detalle.DatoAdd1.Value;
                mySqlCommandSel.Parameters["@DatoAdd2"].Value = detalle.DatoAdd2.Value;
                mySqlCommandSel.Parameters["@DatoAdd3"].Value = detalle.DatoAdd3.Value;
                mySqlCommandSel.Parameters["@DatoAdd4"].Value = detalle.DatoAdd4.Value;
                mySqlCommandSel.Parameters["@DatoAdd5"].Value = detalle.DatoAdd5.Value;
                mySqlCommandSel.Parameters["@ValorAdd1"].Value = detalle.ValorAdd1.Value;
                mySqlCommandSel.Parameters["@ValorAdd2"].Value = detalle.ValorAdd2.Value;
                mySqlCommandSel.Parameters["@ValorAdd3"].Value = detalle.ValorAdd3.Value;
                mySqlCommandSel.Parameters["@ValorAdd4"].Value = detalle.ValorAdd4.Value;
                mySqlCommandSel.Parameters["@ValorAdd5"].Value = detalle.ValorAdd5.Value;
                mySqlCommandSel.Parameters["@Documento1"].Value = detalle.Documento1.Value;
                mySqlCommandSel.Parameters["@Documento2"].Value = detalle.Documento2.Value;
                mySqlCommandSel.Parameters["@Documento3"].Value = detalle.Documento3.Value;
                mySqlCommandSel.Parameters["@Documento4"].Value = detalle.Documento4.Value;
                mySqlCommandSel.Parameters["@Documento5"].Value = detalle.Documento5.Value;

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionPreliminar_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene listado de detalle liquidacion preliminar (Prenomina)
        /// </summary>
        /// <returns>Listado de detalles liquidacion</returns>
        public List<DTO_noLiquidacionPreliminar> DAL_noLiquidacionPreliminar_GetAll(int numDoc)
        {

            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  SELECT noLiquidacionesPreliminar.*, noConceptoNOM.ConceptoNOID,  noConceptoNOM.Descriptivo    " +
                                              "  FROM noLiquidacionesPreliminar     " +
                                              "  INNER JOIN noConceptoNOM ON noConceptoNOM.ConceptoNOID = noLiquidacionesPreliminar.ConceptoNOID    " +
                                              "  AND  noConceptoNOM.EmpresaGrupoID = @EmpresaID " +
                                              "  WHERE noLiquidacionesPreliminar.NumeroDoc =   @NumeroDoc ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDoc;

                List<DTO_noLiquidacionPreliminar> result = new List<DTO_noLiquidacionPreliminar>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_noLiquidacionPreliminar dto = new DTO_noLiquidacionPreliminar(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionPreliminar_GetAll");
                throw exception;
            }

        }

        /// <summary>
        /// Limpia los registros de la tabla preliminar
        /// </summary>
        /// <param name="numDoc">numero Documento</param>
        public void DAL_noLiquidacionPreliminar_Delete(int numDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =  "    delete from noLiquidacionesPreliminar   " +
                                               "    where NumeroDoc = @NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionPreliminar_Delete");
                throw exception;
            }
       
        }

        /// <summary>
        /// Limpia los registros de la tabla preliminar
        /// </summary>
        /// <param name="numDoc">numero Documento</param>
        public void DAL_noLiquidacionPreliminar_DeleteByPk(int concecutivo)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "    delete from noLiquidacionesPreliminar   " +
                                               "    where Consecutivo = @Consecutivo ";
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Value = concecutivo;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionPreliminar_DeleteByPk");
                throw exception;
            }

        }
        
        /// <summary>
        /// Realiza una copia masiva de las liquidaciones de cada empleado
        /// </summary>
        /// <param name="lLiquidacionPreliminar">listado de liquidaciones</param>
        public void DAL_noLiquidacionPreliminar_BulkCopy(List<DTO_noLiquidacionPreliminar> lLiquidacionPreliminar)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                using (var bulkCopy = new SqlBulkCopy(base.MySqlConnection, SqlBulkCopyOptions.KeepIdentity, base.MySqlConnectionTx))
                {
                   bulkCopy.BatchSize = lLiquidacionPreliminar.Count;
                   bulkCopy.DestinationTableName = "noLiquidacionesPreliminar";

                   var table = new DataTable();
                   var props = TypeDescriptor.GetProperties(typeof(DTO_noLiquidacionPreliminar))
                                             .Cast<PropertyDescriptor>()
                                             .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("NewAge.DTO.UDT") && 
                                                                    propertyInfo.Name != "ConceptoNODesc" && 
                                                                    propertyInfo.Name != "Concecutivo"
                                                                    )
                                             .ToArray();

                   string tipo = string.Empty;
                   foreach (var propertyInfo in props)
                   {
                       PropertyDescriptor newP = propertyInfo;      
                       bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                       table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType.GetProperty("Value").PropertyType) ?? propertyInfo.PropertyType.GetProperty("Value").PropertyType);
                   }                                              

                   var values = new object[props.Length];

                   foreach (var item in lLiquidacionPreliminar)
                   {
                       item.eg_noConceptoNOM.Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConceptoNOM, this.Empresa, egCtrl);
                       item.eg_noContratoNov.Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noContratoNov, this.Empresa, egCtrl);
                       item.eg_noFondo.Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noFondo, this.Empresa, egCtrl);

                       for (var i = 0; i < values.Length; i++)
                       {
                           UDT udt = (UDT)item.GetType().GetProperty(props[i].Name).GetValue(item, null);
                           values[i] = !string.IsNullOrEmpty(udt.ToString()) ? udt.ToString() : null;
                       }
                       table.Rows.Add(values);
                   }

                   bulkCopy.WriteToServer(table);
                   bulkCopy.Close();
               }
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionPreliminar_BulkCopy");
                throw exception;
            }
            
        }

        /// <summary>
        /// Procedimiento Almacenado que liquida la Prenomina 
        /// </summary>
        /// <param name="numDoc">número Documento</param>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="errorInd">indicador de error</param>
        /// <param name="errorDesc">descripcion del error</param>
        public void DAL_noLiquidacionPreliminar_LiquidarNomina(string empleadoID, int numeroDoc, decimal tasaCambio, out int errorInd, out string errorDesc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Nomina_LiquidarNomina", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;
                               
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);     
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TasaCambio", SqlDbType.Decimal);
                             
                mySqlCommandSel.Parameters.Add("@eg_noConceptoNOM", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noFondo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noContratoNov", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glAreaFuncional", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glMoneda", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noOperacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noConvencion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noCaja", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRiesgo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noBrigada", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noTurnoCompensatorio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRol", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_rhCargos", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_tsBanco", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                SqlParameter ErrorInd = new SqlParameter("@ErrorInd", SqlDbType.Int); ErrorInd.Direction = ParameterDirection.Output;
                SqlParameter ErrorDesc = new SqlParameter("@ErrorDesc", SqlDbType.VarChar, 200); ErrorDesc.Direction = ParameterDirection.Output;
                mySqlCommandSel.Parameters.Add(ErrorInd);
                mySqlCommandSel.Parameters.Add(ErrorDesc);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@TasaCambio"].Value = tasaCambio;
       
                mySqlCommandSel.Parameters["@eg_noConceptoNOM"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConceptoNOM, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noFondo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noFondo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noContratoNov"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noContratoNov, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glMoneda"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glMoneda, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);                
                mySqlCommandSel.Parameters["@eg_noOperacion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noOperacion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noConvencion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConvencion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noCaja"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noCaja, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRiesgo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRiesgo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noBrigada"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noBrigada, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noTurnoCompensatorio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noTurnoCompensatorio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRol"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRol, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_rhCargos"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.rhCargos, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_tsBanco"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsBanco, this.Empresa, egCtrl);
                
                
                mySqlCommandSel.ExecuteNonQuery();

                errorInd = (int)mySqlCommandSel.Parameters["@ErrorInd"].Value;
                errorDesc = (string)mySqlCommandSel.Parameters["@ErrorDesc"].Value;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionPreliminar_LiquidarNomina");
                throw exception;
            }
        }

        /// <summary>
        /// Procedimiento Almacenado que liquida la Prenomina 
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="numeroDoc">número Doc</param>
        /// <param name="fechaRetiro">fecha de retiro empleado</param>
        /// <param name="causa">causa liquidación</param>
        /// <param name="tasaCambio">tasa de cambio</param>
        /// <param name="errorInd">indicador de error</param>
        /// <param name="errorDesc">descripcion del error</param>
        public void DAL_noLiquidacionPreliminar_LiquidarContrato(string empleadoID, int numeroDoc, DateTime fechaRetiro, decimal tasaCambio, int causa, out int errorInd, out string errorDesc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Nomina_LiquidarContrato", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaRetiro", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@TasaCambio", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CausaLiquidacion", SqlDbType.TinyInt);

                mySqlCommandSel.Parameters.Add("@eg_noConceptoNOM", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noFondo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noContratoNov", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glAreaFuncional", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glMoneda", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noOperacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noConvencion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noCaja", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRiesgo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noBrigada", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noTurnoCompensatorio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRol", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_rhCargos", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_tsBanco", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                SqlParameter ErrorInd = new SqlParameter("@ErrorInd", SqlDbType.Int); ErrorInd.Direction = ParameterDirection.Output;
                SqlParameter ErrorDesc = new SqlParameter("@ErrorDesc", SqlDbType.VarChar, 200); ErrorDesc.Direction = ParameterDirection.Output;
                mySqlCommandSel.Parameters.Add(ErrorInd);
                mySqlCommandSel.Parameters.Add(ErrorDesc);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaRetiro"].Value = fechaRetiro;
                mySqlCommandSel.Parameters["@TasaCambio"].Value = tasaCambio;
                mySqlCommandSel.Parameters["@CausaLiquidacion"].Value = causa;

                mySqlCommandSel.Parameters["@eg_noConceptoNOM"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConceptoNOM, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noFondo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noFondo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noContratoNov"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noContratoNov, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glMoneda"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glMoneda, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noOperacion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noOperacion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noConvencion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConvencion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noCaja"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noCaja, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRiesgo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRiesgo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noBrigada"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noBrigada, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noTurnoCompensatorio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noTurnoCompensatorio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRol"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRol, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_rhCargos"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.rhCargos, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_tsBanco"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsBanco, this.Empresa, egCtrl);


                mySqlCommandSel.ExecuteNonQuery();

                errorInd = (int)mySqlCommandSel.Parameters["@ErrorInd"].Value;
                errorDesc = (string)mySqlCommandSel.Parameters["@ErrorDesc"].Value;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionPreliminar_LiquidarContrato");
                throw exception;
            }
        }

        /// <summary>
        /// Procedimiento Almacenado que liquida las Vacaciones
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="fechaIni">fecha inicial vacaciones</param>
        /// <param name="fechaFin">fecha final vacaciones</param>
        /// <param name="diasVacDinero">días vacaciones en dinero</param>
        /// <param name="indIncNomina">indica si incluye en nómina</param>
        /// <param name="indPrima">indica si se incluye la prima</param>
        /// <param name="errorInd">indicador de error</param>
        /// <param name="errorDesc">descripcion del error</param>
        public void DAL_noLiquidacionPreliminar_LiquidarVacaciones(string empleadoID, int numeroDoc, DateTime fechaIni, DateTime fechaFin, 
                                                                   int diasVacTiempo, int diasVacDinero, bool indIncNomina, bool indPrima,
                                                                   string resolucion, decimal tasaCambio, DateTime fechaIniPago, DateTime fechaFinPago, DateTime fechaIniPendVac, DateTime fechaFinPendVac, out int errorInd, out string errorDesc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Nomina_LiquidarVacaciones", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaIniVacaciones", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinVacaciones", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaIniPago", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinPago", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaIniPendVacaciones", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinPendVacaciones", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@DiasVacacionesDinero", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DiasVacacionesTiempo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@IncNomina", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@IndPrima", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Resolucion", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@TasaCambio", SqlDbType.Char, 20);
                

                mySqlCommandSel.Parameters.Add("@eg_noEmpleado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noConceptoNOM", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noFondo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noContratoNov", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glAreaFuncional", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glMoneda", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noOperacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noConvencion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noCaja", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRiesgo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noBrigada", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noTurnoCompensatorio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRol", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_rhCargos", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_tsBanco", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                SqlParameter ErrorInd = new SqlParameter("@ErrorInd", SqlDbType.Int); ErrorInd.Direction = ParameterDirection.Output;
                SqlParameter ErrorDesc = new SqlParameter("@ErrorDesc", SqlDbType.VarChar, 200); ErrorDesc.Direction = ParameterDirection.Output;
                mySqlCommandSel.Parameters.Add(ErrorInd);
                mySqlCommandSel.Parameters.Add(ErrorDesc);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                mySqlCommandSel.Parameters["@FechaIniVacaciones"].Value = fechaIni;
                mySqlCommandSel.Parameters["@FechaFinVacaciones"].Value = fechaFin;
                mySqlCommandSel.Parameters["@FechaIniPago"].Value = fechaIniPago;
                mySqlCommandSel.Parameters["@FechaFinPago"].Value = fechaFinPago;
                mySqlCommandSel.Parameters["@FechaIniPendVacaciones"].Value = fechaIniPendVac;
                mySqlCommandSel.Parameters["@FechaFinPendVacaciones"].Value = fechaFinPendVac;
                mySqlCommandSel.Parameters["@DiasVacacionesDinero"].Value = diasVacDinero;
                mySqlCommandSel.Parameters["@DiasVacacionesTiempo"].Value = diasVacTiempo;
                mySqlCommandSel.Parameters["@IncNomina"].Value = indIncNomina;
                mySqlCommandSel.Parameters["@IndPrima"].Value = indPrima;
                mySqlCommandSel.Parameters["@Resolucion"].Value = resolucion;
                mySqlCommandSel.Parameters["@TasaCambio"].Value = tasaCambio;

                mySqlCommandSel.Parameters["@eg_noEmpleado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noEmpleado, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noConceptoNOM"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConceptoNOM, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noFondo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noFondo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noContratoNov"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noContratoNov, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glMoneda"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glMoneda, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noOperacion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noOperacion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noConvencion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConvencion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noCaja"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noCaja, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRiesgo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRiesgo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noBrigada"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noBrigada, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noTurnoCompensatorio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noTurnoCompensatorio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRol"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRol, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_rhCargos"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.rhCargos, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_tsBanco"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsBanco, this.Empresa, egCtrl);


                mySqlCommandSel.ExecuteNonQuery();

                errorInd = (int)mySqlCommandSel.Parameters["@ErrorInd"].Value;
                errorDesc = (string)mySqlCommandSel.Parameters["@ErrorDesc"].Value;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionPreliminar_LiquidarVacaciones");
                throw exception;
            }
        }

        /// <summary>
        /// Procedimiento Almacenado que liquida la Prenomina 
        /// </summary>
        /// <param name="numDoc">número Documento</param>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="errorInd">indicador de error</param>
        /// <param name="errorDesc">descripcion del error</param>
        public void DAL_noLiquidacionPreliminar_LiquidarCesantias(string empleadoID, int numeroDoc, DateTime fechaIniLiq, DateTime fechaFinLiq, DateTime fechaPago, string resolucion, TipoLiqCesantias tliqCesantias, decimal tasaCambio, out int errorInd, out string errorDesc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Nomina_LiquidarCesantias", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaIniCorte", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinCorte", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaPago", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@Resolucion", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@TasaCambio", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@IsAnual", SqlDbType.Char, 20);

                mySqlCommandSel.Parameters.Add("@eg_noConceptoNOM", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noFondo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noContratoNov", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glAreaFuncional", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glMoneda", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noOperacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noConvencion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noCaja", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRiesgo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noBrigada", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noTurnoCompensatorio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRol", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_rhCargos", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_tsBanco", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                SqlParameter ErrorInd = new SqlParameter("@ErrorInd", SqlDbType.Int); ErrorInd.Direction = ParameterDirection.Output;
                SqlParameter ErrorDesc = new SqlParameter("@ErrorDesc", SqlDbType.VarChar, 200); ErrorDesc.Direction = ParameterDirection.Output;
                mySqlCommandSel.Parameters.Add(ErrorInd);
                mySqlCommandSel.Parameters.Add(ErrorDesc);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaIniCorte"].Value = fechaIniLiq;
                mySqlCommandSel.Parameters["@FechaFinCorte"].Value = fechaFinLiq;
                mySqlCommandSel.Parameters["@FechaPago"].Value = fechaPago;
                mySqlCommandSel.Parameters["@Resolucion"].Value = resolucion;
                mySqlCommandSel.Parameters["@TasaCambio"].Value = tasaCambio;
                mySqlCommandSel.Parameters["@IsAnual"].Value = ((int)tliqCesantias).ToString();

                mySqlCommandSel.Parameters["@eg_noConceptoNOM"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConceptoNOM, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noFondo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noFondo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noContratoNov"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noContratoNov, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glMoneda"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glMoneda, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noOperacion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noOperacion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noConvencion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConvencion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noCaja"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noCaja, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRiesgo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRiesgo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noBrigada"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noBrigada, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noTurnoCompensatorio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noTurnoCompensatorio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRol"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRol, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_rhCargos"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.rhCargos, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_tsBanco"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsBanco, this.Empresa, egCtrl);


                mySqlCommandSel.ExecuteNonQuery();

                errorInd = (int)mySqlCommandSel.Parameters["@ErrorInd"].Value;
                errorDesc = (string)mySqlCommandSel.Parameters["@ErrorDesc"].Value;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionPreliminar_LiquidarNomina");
                throw exception;
            }
        }
        
        /// <summary>
        /// Procedimiento Almacenado que liquida la Prima
        /// </summary>
        /// <param name="numDoc">número Documento</param>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="fechaIniLiq">Fecha Inicial del Corte</param>
        /// <param name="fechaFinLiq">Fecha Final de Corte</param>
        /// <param name="incNomina">determina si se incluye en la Nomina</param>
        /// <param name="errorInd">indicador de error</param>
        /// <param name="errorDesc">descripcion del error</param>
        public void DAL_noLiquidacionPreliminar_LiquidarPrima(string empleadoID, int numeroDoc, DateTime fechaIniLiq, DateTime fechaFinLiq, 
            decimal tasaCambio, bool incNomina, out int errorInd, out string errorDesc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Nomina_LiquidarPrima", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaIniCorte", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinCorte", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@IncNomina", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TasaCambio", SqlDbType.Char, 20);


                mySqlCommandSel.Parameters.Add("@eg_noEmpleado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noConceptoNOM", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noFondo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noContratoNov", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glAreaFuncional", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glMoneda", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noOperacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noConvencion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noCaja", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRiesgo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noBrigada", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noTurnoCompensatorio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRol", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_rhCargos", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_tsBanco", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                SqlParameter ErrorInd = new SqlParameter("@ErrorInd", SqlDbType.Int); ErrorInd.Direction = ParameterDirection.Output;
                SqlParameter ErrorDesc = new SqlParameter("@ErrorDesc", SqlDbType.VarChar, 200); ErrorDesc.Direction = ParameterDirection.Output;
                mySqlCommandSel.Parameters.Add(ErrorInd);
                mySqlCommandSel.Parameters.Add(ErrorDesc);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaIniCorte"].Value = fechaIniLiq;
                mySqlCommandSel.Parameters["@FechaFinCorte"].Value = fechaFinLiq;
                mySqlCommandSel.Parameters["@IncNomina"].Value = incNomina;
                mySqlCommandSel.Parameters["@TasaCambio"].Value = tasaCambio;


                mySqlCommandSel.Parameters["@eg_noEmpleado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noEmpleado, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noConceptoNOM"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConceptoNOM, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noFondo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noFondo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noContratoNov"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noContratoNov, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glMoneda"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glMoneda, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noOperacion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noOperacion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noConvencion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConvencion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noCaja"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noCaja, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRiesgo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRiesgo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noBrigada"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noBrigada, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noTurnoCompensatorio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noTurnoCompensatorio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRol"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRol, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_rhCargos"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.rhCargos, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_tsBanco"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsBanco, this.Empresa, egCtrl);


                mySqlCommandSel.ExecuteNonQuery();

                errorInd = (int)mySqlCommandSel.Parameters["@ErrorInd"].Value;
                errorDesc = (string)mySqlCommandSel.Parameters["@ErrorDesc"].Value;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionPreliminar_LiquidarPrima");
                throw exception;
            }
        }

        /// <summary>
        /// Procedimiento encargardo de liquidar Novedad de Nomina
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="conceptoNOID">identificador de concepto</param>
        /// <param name="ValorFormula">valor si se liquida por formula</param>
        /// <param name="errorInd">indicador de error</param>
        /// <param name="errorDesc">descripcion de erro</param>
        public void DAL_noLiquidacionPreliminar_LiquidarNovedadesNomina(string empleadoID, int numeroDoc,  string conceptoNOID, double ValorFormula, out int errorInd, out string errorDesc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Nomina_LiquidarNovedadesNomina", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConceptoNOID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ValorFormula", SqlDbType.Decimal);
 
                mySqlCommandSel.Parameters.Add("@eg_noConceptoNOM", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noFondo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);               

                SqlParameter ErrorInd = new SqlParameter("@ErrorInd", SqlDbType.Int); ErrorInd.Direction = ParameterDirection.Output;
                SqlParameter ErrorDesc = new SqlParameter("@ErrorDesc", SqlDbType.VarChar, 200); ErrorDesc.Direction = ParameterDirection.Output;
                mySqlCommandSel.Parameters.Add(ErrorInd);
                mySqlCommandSel.Parameters.Add(ErrorDesc);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ConceptoNOID"].Value = conceptoNOID;
                mySqlCommandSel.Parameters["@ValorFormula"].Value = ValorFormula;
    
                mySqlCommandSel.Parameters["@eg_noConceptoNOM"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConceptoNOM, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noFondo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noFondo, this.Empresa, egCtrl);
             
                mySqlCommandSel.ExecuteNonQuery();

                errorInd = (int)mySqlCommandSel.Parameters["@ErrorInd"].Value;
                errorDesc = (string)mySqlCommandSel.Parameters["@ErrorDesc"].Value;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionPreliminar_LiquidarNovedadesNomina");
                throw exception;
            }
        }
    
    }
}
