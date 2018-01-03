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
using System.Data.SqlTypes;

namespace NewAge.ADO
{
    public class DAL_noLiquidacionesDetalle : DAL_Base
    {
      /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_noLiquidacionesDetalle(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
        
        /// <summary>
        /// Adiciona un registro a la tabla noLiquidacionDetalle
        /// </summary>
        /// <param name="detalle">detalle liquidacion</param>
        /// <returns>true si la transaccion es exitosa</returns>
        public void DAL_noLiquidacionesDetalle_Add(DTO_noLiquidacionPreliminar detalle)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =  "  INSERT INTO noLiquidacionesDetalle    " +
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
                                               "  ,[NumeroPagoEmpleado]    " +
                                               "  ,[NumeroPagoTercero]    " +
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
                                               "  ,@NumeroPagoEmpleado    " +
                                               "  ,@NumeroPagoTercero    " +
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
                mySqlCommandSel.Parameters.Add("@FondoNOID", SqlDbType.Char, UDT_FondoNOID.MaxLength );
                mySqlCommandSel.Parameters.Add("@ContratoNONovID", SqlDbType.Char, UDT_ContratoNONovID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Numero", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroPagoEmpleado", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroPagoTercero", SqlDbType.Int);
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
                mySqlCommandSel.Parameters["@NumeroPagoEmpleado"].Value = SqlInt32.Null;
                mySqlCommandSel.Parameters["@NumeroPagoTercero"].Value = SqlInt32.Null;
                mySqlCommandSel.Parameters["@eg_noConceptoNOM"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConceptoNOM, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noFondo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noFondo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noContratoNov"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noContratoNov, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@DatoAdd1"].Value = detalle.DatoAdd1.Value;
                mySqlCommandSel.Parameters["@DatoAdd2"].Value =  detalle.DatoAdd2.Value;
                mySqlCommandSel.Parameters["@DatoAdd3"].Value =  detalle.DatoAdd3.Value;
                mySqlCommandSel.Parameters["@DatoAdd4"].Value =  detalle.DatoAdd4.Value;
                mySqlCommandSel.Parameters["@DatoAdd5"].Value =  detalle.DatoAdd5.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDetalle_Add");
                throw exception;
            }        
        }

        /// <summary>
        /// Copia la informacion del Preliminar en la tabla de Detalle
        /// </summary>
        /// <param name="numeroDoc">numero de documento</param>
        public void DAL_noLiquidacionesDetalle_Copy(int numeroDoc)
        {
            try
            {               
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =   " INSERT INTO noLiquidacionesDetalle  " +
                                                " SELECT [EmpresaID]  " +
                                                        " ,[NumeroDoc]  " +
                                                        " ,[ConceptoNOID]  " +
                                                        " ,[Dias]  " +
                                                        " ,[Base]  " +
                                                        " ,[Valor]  " +
                                                        " ,[OrigenConcepto]  " +
                                                        " ,[FondoNOID]  " +
                                                        " ,[ContratoNONovID]  " +
                                                        " ,[Numero]  " +
                                                        " ,NULL  " +
                                                        " ,NULL  " +
                                                        " ,[eg_noConceptoNOM]  " +
                                                        " ,[eg_noFondo]  " +
                                                        " ,[eg_noContratoNov]       " +
                                                        " ,[DatoAdd1]  " +
                                                        " ,[DatoAdd2]  " +
                                                        " ,[DatoAdd3]  " +
                                                        " ,[DatoAdd4]  " +
                                                        " ,[DatoAdd5]  " +
                                                        " ,[ValorAdd1]  " +
                                                        " ,[ValorAdd2]  " +
                                                        " ,[ValorAdd3]  " +
                                                        " ,[ValorAdd4]  " +
                                                        " ,[ValorAdd5]  " +
                                                        " ,[Documento1]  " +
                                                        " ,[Documento2]  " +
                                                        " ,[Documento3]  " +
                                                        " ,[Documento4]  " +
                                                        " ,[Documento5]  " +
                                                    " FROM noLiquidacionesPreliminar  " +
                                                    " WHERE NumeroDoc = @NumeroDoc "; 

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;                

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDetalle_Copy");
                throw exception;
            }
        }

        /// <summary>
        /// listado del detalle de la liquidacipón según en número de documento asociado
        /// </summary>
        /// <param name="numDoc">numero Documento</param>
        /// <returns>listado de liquidaciones</returns>
        public List<DTO_noLiquidacionesDetalle> DAL_noLiquidacionesDetalle_Get(int numDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "   SELECT noConceptoNom.Descriptivo as ConceptoNODesc,  noLiquidacionesDetalle.*  " +
                                              "   FROM noLiquidacionesDetalle  " +
                                              "   INNER JOIN noConceptoNom on noLiquidacionesDetalle.ConceptoNOID = noConceptoNom.ConceptoNOID  " +
                                              "   AND noConceptoNom.EmpresaGrupoID = @EmpresaID " +
                                              "   WHERE noLiquidacionesDetalle.NumeroDoc =  @NumeroDoc  ";
                                             ;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDoc;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                List<DTO_noLiquidacionesDetalle> result = new List<DTO_noLiquidacionesDetalle>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_noLiquidacionesDetalle dto = new DTO_noLiquidacionesDetalle(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDetalle_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el valor de la liquidacion por empleado en el periodo especificado
        /// </summary>
        /// <param name="documentID">identificador del documento</param>
        /// <param name="periodo">periodo</param>
        /// <param name="contrato">numero de contrato</param>
        /// <returns>valor liquidado</returns>
        public decimal DAL_noLiquidacionesDetalle_GetValorByEmpleado(int documentID, DateTime periodo, int contrato)
        {
            try
            {
                decimal result = 0;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "   SELECT SUM(noLiquidacionesDetalle.Valor) as Valor     " +
                    "   FROM glDocumentoControl with(nolock)     " +
                    "   INNER JOIN noLiquidacionesDocu on glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc     " +
                    "   INNER JOIN noLiquidacionesDetalle on noLiquidacionesDocu.NumeroDoc = noLiquidacionesDetalle.NumeroDoc   " +
                    "   WHERE glDocumentoControl.EmpresaID = @EmpresaID " +
                    "   AND  glDocumentoControl.DocumentoID = @DocumentoID     " +
                    "   AND glDocumentoControl.PeriodoDoc = @Periodo     " +
                    "   AND noLiquidacionesDocu.ContratoNOID = @ContratoNOID";


                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ContratoNOID", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentID;
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                mySqlCommand.Parameters["@ContratoNOID"].Value = contrato;

                var r = mySqlCommand.ExecuteScalar();
                if (r.ToString() == string.Empty)
                    result = 0;
                else
                    result = (decimal)r;

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDetalle_GetValorByEmpleado");
                throw exception;
            }
        }

        /// <summary>
        ///  Obtiene el detalle para efectos de Pago de Nomina
        /// </summary>
        /// <param name="periodo">Periodo de Nomina</param>
        /// <param name="empleadoId">Identificador de Empleado</param>
        /// <returns>Listado Detalle</returns>
        public List<DTO_noLiquidacionesDetalle> DAL_noLiquidacionesDetalle_GetDetallePago(int documentoID, DateTime periodo, string empleadoId)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "     SELECT noLiquidacionesDetalle.*, noConceptoNOM.Descriptivo as ConceptoNODesc   " +
                                                "     FROM glDocumentoControl   " +
                                                "     INNER JOIN noLiquidacionesDocu on glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc   " +
                                                "     INNER JOIN noLiquidacionesDetalle on noLiquidacionesDocu.NumeroDoc = noLiquidacionesDetalle.NumeroDoc   " +
                                                "     INNER JOIN noEmpleado on noLiquidacionesDocu.ContratoNOID = noEmpleado.ContratoNOID   " +
                                                "     INNER JOIN noConceptoNOM on noLiquidacionesDetalle.ConceptoNOID = noConceptoNOM.ConceptoNOID   " +
                                                "     AND noConceptoNOM.EmpresaGrupoID = @EmpresaID  " +
                                                "     WHERE glDocumentoControl.EmpresaID = @EmpresaID   " +
                                                "     AND glDocumentoControl.PeriodoDoc = @Periodo   " +
                                                "     AND glDocumentoControl.Estado = 3   " +
                                                "     AND noEmpleado.EmpleadoID = @EmpleadoID   " +
                                                "     AND glDocumentoControl.DocumentoID = @DocNomina ";
	                                           

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocNomina", SqlDbType.Int);
 
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Periodo"].Value = periodo;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoId;
                mySqlCommandSel.Parameters["@DocNomina"].Value = documentoID;

                List<DTO_noLiquidacionesDetalle> result = new List<DTO_noLiquidacionesDetalle>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_noLiquidacionesDetalle dto = new DTO_noLiquidacionesDetalle(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDetalle_Get");
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
        public void DAL_noLiquidacionPreliminar_MigracionNomina(DTO_noMigracionNomina reg, int numeroDoc, decimal tasaCambio, out int errorInd, out string errorDesc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Nomina_MigracionNomina", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TasaCambio", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TipoLiquidacion", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ConceptoNOID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Dias", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Base", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@OrigenConcepto", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FondoNOID", SqlDbType.Char, UDT_FondoNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DiasVacacionesPagos", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DiasVacacionesTomados", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaSalidaVacaciones", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaLLegadaVacaciones", SqlDbType.SmallDateTime);
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
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = reg.EmpleadoID.Value;;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@TasaCambio"].Value = tasaCambio;
                mySqlCommandSel.Parameters["@TipoLiquidacion"].Value = reg.TipoLiquidacion.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = reg.PeriodoID.Value;
                mySqlCommandSel.Parameters["@ConceptoNOID"].Value = reg.ConceptoNOID.Value;
                mySqlCommandSel.Parameters["@Dias"].Value = reg.Dias.Value;
                mySqlCommandSel.Parameters["@Base"].Value = reg.Base.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = reg.Valor.Value;
                mySqlCommandSel.Parameters["@OrigenConcepto"].Value = reg.OrigenConcepto.Value;
                mySqlCommandSel.Parameters["@FondoNOID"].Value = reg.FondoNOID.Value;
                mySqlCommandSel.Parameters["@DiasVacacionesPagos"].Value = reg.DiasVacacionesPagos.Value;
                mySqlCommandSel.Parameters["@DiasVacacionesTomados"].Value = reg.DiasVacacionesTomados.Value;
                mySqlCommandSel.Parameters["@FechaSalidaVacaciones"].Value = reg.FechaSalidaVacaciones.Value;
                mySqlCommandSel.Parameters["@FechaLLegadaVacaciones"].Value = reg.FechaLLegadaVacaciones.Value;
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


                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();

                errorInd = (int)mySqlCommandSel.Parameters["@ErrorInd"].Value;
                errorDesc = (string)mySqlCommandSel.Parameters["@ErrorDesc"].Value;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionPreliminar_MigracionNomina");
                throw exception;
            }
        }

        /// <summary>
        /// Filtro de fecha por empleado para Documento de Vacaciones
        /// </summary>
        /// <param name="_empleadoID"></param>
        /// <param name="_vacaciones"></param>
        /// <returns></returns>
        public List<DTO_ReportVacacionesDocumento> DAL_Report_No_GetVacacionesByEmpleado(string _empleadoID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                //_vacaciones = this.no

                mySqlCommandSel.CommandText =

                    "   SELECT  " +
                    "   	DISTINCT  " +
                    "   	DET.NumeroDoc,	  " +
                    "   	EPL.TerceroID AS Cédula,  " +
                    "   	DOC.FechaIni2 AS PeriodoDescansoInicial,  " +
                    "   	DOC.FechaFin2 AS PeriodoDescasonFinal   " +
                    "   FROM		noLiquidacionesDetalle	DET   " +
                    "   INNER JOIN noLiquidacionesDocu		DOC ON DOC.NumeroDoc = DET.NumeroDoc   " +
                    "   INNER JOIN glDocumentoControl		CTL ON CTL.NumeroDoc = DOC.NumeroDoc 	  " +
                    "   INNER JOIN noEmpleado					EPL ON EPL.TerceroID = CTL.TerceroID						AND EPL.eg_coTercero = CTL.EmpresaID   " +
                    "   INNER JOIN noConceptoNOM			NOM ON NOM.ConceptoNOID = DET.ConceptoNOID	AND NOM.eg_noConceptoGrupoNOM = DET.eg_noConceptoNOM   " +
                    "   INNER JOIN rhCargos CAR					ON  CAR.CargoEmpID = DOC.CargoEmpID					AND  CAR.EmpresaGrupoID = @EmpresaID  " +
                    "   WHERE   CTL.EmpresaID = @EmpresaID  " +
                            "   AND CTL.DocumentoID = @DocumentoID  " +
                            "   AND EPL.TerceroID = CASE WHEN @empleadoID = '' THEN EPL.TerceroID ELSE @empleadoID END   " +
                            "   AND DOC.PagadoInd = 0  " +
                    "   ORDER BY EPL.TerceroID,NumeroDoc  ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@empleadoID", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Char);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@empleadoID"].Value = _empleadoID;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.Vacaciones;

                List<DTO_ReportVacacionesDocumento> result = new List<DTO_ReportVacacionesDocumento>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ReportVacacionesDocumento dto = new DTO_ReportVacacionesDocumento(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Report_No_VacacionesDocumento");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta de por empleado para traer las fechas y utilizarlas como filtro.
        /// </summary>
        /// <param name="_empleadoID"></param>
        /// <returns></returns>
        public List<DTO_ReportVacacionesDocumento> DAL_Report_No_GetLiquidaContratoByEmpleado(string _empleadoID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                //_vacaciones = this.no

                mySqlCommandSel.CommandText =

                "   SELECT      " +
                "   	DISTINCT      " +
                "   	DET.NumeroDoc,      " +
                "   	DET.EmpresaID,      " +
                "   	EPL.Descriptivo AS Nombre,      " +
                "   	EPL.TerceroID AS Cédula,      " +
                "   	DOC.Fecha1 AS PeriodoDescansoInicial,      " +
                "       EPL.FechaRetiro AS PeriodoDescasonFinal   " +
                "   FROM	noLiquidacionesDetalle		DET WITH (NOLOCK)      " +
                "   INNER JOIN noLiquidacionesDocu		DOC WITH (NOLOCK) ON DOC.NumeroDoc = DET.NumeroDoc       " +
                "   INNER JOIN glDocumentoControl		CTL WITH (NOLOCK) ON CTL.NumeroDoc = DOC.NumeroDoc 				      " +
                "   INNER JOIN noEmpleado				EPL WITH (NOLOCK) ON EPL.TerceroID = CTL.TerceroID AND EPL.eg_coTercero = CTL.EmpresaID       " +
                "   INNER JOIN noConceptoNOM			NOM WITH (NOLOCK) ON NOM.ConceptoNOID = DET.ConceptoNOID	AND NOM.eg_noConceptoGrupoNOM = DET.eg_noConceptoNOM       " +
                "   INNER JOIN noConceptoGrupoNOM		GRP WITH (NOLOCK) ON NOM.ConceptoNOGrupoID = GRP.ConceptoNOGrupoID      " +
                "   WHERE   CTL.EmpresaID = @EmpresaID      " +
                        "   AND CTL.DocumentoID = @DocumentoID      " +
                        "   AND EPL.TerceroID = CASE WHEN @empleadoID = '' THEN EPL.TerceroID ELSE @empleadoID END       " +
                "   ORDER BY       " +
                    "   DET.NumeroDoc,       " +
                    "   EPL.TerceroID      ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@empleadoID", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Char);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@empleadoID"].Value = _empleadoID;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.LiquidacionContrato;

                List<DTO_ReportVacacionesDocumento> result = new List<DTO_ReportVacacionesDocumento>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ReportVacacionesDocumento dto = new DTO_ReportVacacionesDocumento(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Report_No_VacacionesDocumento");
                throw exception;
            }
        }

        //
    }
}
