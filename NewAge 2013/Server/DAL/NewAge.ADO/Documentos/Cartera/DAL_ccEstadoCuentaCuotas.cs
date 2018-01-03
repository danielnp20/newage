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

namespace NewAge.ADO
{
    public class DAL_ccEstadoCuentaCuotas : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccEstadoCuentaCuotas(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trae todos los registros de DTO_ccEstadoCuentaComponentes
        /// </summary>
        /// <returns>retorna una lista de DTO_ccEstadoCuentaComponentes</returns>
        public List<DTO_ccEstadoCuentaCuotas> DAL_ccEstadoCuentaCuotas_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                List<DTO_ccEstadoCuentaCuotas> result = new List<DTO_ccEstadoCuentaCuotas>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccEstadoCuentaCuotas WITH(NOLOCK) where NumeroDoc = @NumeroDoc ";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccEstadoCuentaCuotas r = new DTO_ccEstadoCuentaCuotas(dr);
                    result.Add(r);
                }

                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccEstadoCuentaCuotas_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccEstadoCuentaComponetes
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        public void DAL_ccEstadoCuentaCuotas_Add(DTO_ccEstadoCuentaCuotas estadoCuentaCuotas)
        {
            try
            {
                List<DTO_ccEstadoCuentaCuotas> result = new List<DTO_ccEstadoCuentaCuotas>();
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query
                mySqlCommandSel.CommandText = "INSERT INTO ccEstadoCuentaCuotas   " +
                                                  "  ([NumeroDoc]   " +
                                                  "  ,[CuotaID] " +
                                                  "  ,[FechaCuota] " +
                                                  "  ,[CompradorCarteraID] " +
                                                  "  ,[VlrCuota] " +
                                                  "  ,[VlrCapitalSDO] " +
                                                  "  ,[VlrInteresSDO] " +
                                                  "  ,[VlrSeguroSDO] " +
                                                  "  ,[VlrOtro1SDO] " +
                                                  "  ,[VlrOtro2SDO] " +
                                                  "  ,[VlrOtro3SDO] " +
                                                  "  ,[VlrOtrosfijosSDO] " +
                                                  "  ,[VlrCapitalABO] " +
                                                  "  ,[VlrInteresABO] " +
                                                  "  ,[VlrSeguroABO] " +
                                                  "  ,[VlrOtro1ABO] " +
                                                  "  ,[VlrOtro2ABO] " +
                                                  "  ,[VlrOtro3ABO] " +
                                                  "  ,[VlrOtrosfijosABO] " +
                                                  "  ,[VlrCapitalPAG] " +
                                                  "  ,[VlrInteresPAG] " +
                                                  "  ,[VlrSeguroPAG] " +
                                                  "  ,[VlrOtro1PAG] " +
                                                  "  ,[VlrOtro2PAG] " +
                                                  "  ,[VlrOtro3PAG] " +
                                                  "  ,[VlrOtrosfijosPAG] " +
                                                  "  ,[VlrPRJ] " +
                                                  "  ,[VlrMora] " +
                                                  "  ,[VlrGastos] " +
                                                  "  ,[VlrMoraSDO] " +
                                                  "  ,[IndCuotaVencida] " +
                                                  "  ,[eg_ccCompradorCartera] )" +
                                                  "VALUES    " +
                                                  "  (@NumeroDoc    " +
                                                  "  ,@CuotaID " +
                                                  "  ,@FechaCuota " +
                                                  "  ,@CompradorCarteraID " +
                                                  "  ,@VlrCuota " +
                                                  "  ,@VlrCapitalSDO " +
                                                  "  ,@VlrInteresSDO " +
                                                  "  ,@VlrSeguroSDO " +
                                                  "  ,@VlrOtro1SDO " +
                                                  "  ,@VlrOtro2SDO " +
                                                  "  ,@VlrOtro3SDO " +
                                                  "  ,@VlrOtrosfijosSDO " +
                                                  "  ,@VlrCapitalABO " +
                                                  "  ,@VlrInteresABO " +
                                                  "  ,@VlrSeguroABO " +
                                                  "  ,@VlrOtro1ABO " +
                                                  "  ,@VlrOtro2ABO " +
                                                  "  ,@VlrOtro3ABO " +
                                                  "  ,@VlrOtrosfijosABO " +
                                                  "  ,@VlrCapitalPAG " +
                                                  "  ,@VlrInteresPAG " +
                                                  "  ,@VlrSeguroPAG " +
                                                  "  ,@VlrOtro1PAG " +
                                                  "  ,@VlrOtro2PAG " +
                                                  "  ,@VlrOtro3PAG " +
                                                  "  ,@VlrOtrosfijosPAG " +
                                                  "  ,@VlrPRJ " +
                                                  "  ,@VlrMora " +
                                                  "  ,@VlrGastos " +
                                                  "  ,@VlrMoraSDO " +
                                                  "  ,@IndCuotaVencida " +
                                                  "  ,@eg_ccCompradorCartera)";
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CuotaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaCuota" , SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID" , SqlDbType.Char,UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@VlrCuota" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCapitalSDO" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrInteresSDO" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSeguroSDO" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro1SDO" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro2SDO" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro3SDO" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtrosfijosSDO" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCapitalABO" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrInteresABO" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSeguroABO" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro1ABO" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro2ABO" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro3ABO" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtrosfijosABO" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCapitalPAG" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrInteresPAG" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSeguroPAG" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro1PAG" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro2PAG" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro3PAG" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtrosfijosPAG" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPRJ" , SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrMora", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrGastos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrMoraSDO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IndCuotaVencida", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera" , SqlDbType.Char,UDT_EmpresaGrupoID.MaxLength);               
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = estadoCuentaCuotas.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@CuotaID"].Value = estadoCuentaCuotas.CuotaID.Value;
                mySqlCommandSel.Parameters["@FechaCuota"].Value = estadoCuentaCuotas.FechaCuota.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = estadoCuentaCuotas.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = estadoCuentaCuotas.VlrCuota.Value;
                mySqlCommandSel.Parameters["@VlrCapitalSDO"].Value = estadoCuentaCuotas.VlrCapitalSDO.Value;
                mySqlCommandSel.Parameters["@VlrInteresSDO"].Value = estadoCuentaCuotas.VlrInteresSDO.Value;
                mySqlCommandSel.Parameters["@VlrSeguroSDO"].Value = estadoCuentaCuotas.VlrSeguroSDO.Value;
                mySqlCommandSel.Parameters["@VlrOtro1SDO"].Value = estadoCuentaCuotas.VlrOtro1SDO.Value;
                mySqlCommandSel.Parameters["@VlrOtro2SDO"].Value = estadoCuentaCuotas.VlrOtro2SDO.Value;
                mySqlCommandSel.Parameters["@VlrOtro3SDO"].Value = estadoCuentaCuotas.VlrOtro3SDO.Value;
                mySqlCommandSel.Parameters["@VlrOtrosfijosSDO"].Value = estadoCuentaCuotas.VlrOtrosfijosSDO.Value;
                mySqlCommandSel.Parameters["@VlrCapitalABO"].Value = estadoCuentaCuotas.VlrCapitalABO.Value;
                mySqlCommandSel.Parameters["@VlrInteresABO"].Value = estadoCuentaCuotas.VlrInteresABO.Value;
                mySqlCommandSel.Parameters["@VlrSeguroABO"].Value = estadoCuentaCuotas.VlrSeguroABO.Value;
                mySqlCommandSel.Parameters["@VlrOtro1ABO"].Value = estadoCuentaCuotas.VlrOtro1ABO.Value;
                mySqlCommandSel.Parameters["@VlrOtro2ABO"].Value = estadoCuentaCuotas.VlrOtro2ABO.Value;
                mySqlCommandSel.Parameters["@VlrOtro3ABO"].Value = estadoCuentaCuotas.VlrOtro3ABO.Value;
                mySqlCommandSel.Parameters["@VlrOtrosfijosABO"].Value = estadoCuentaCuotas.VlrOtrosfijosABO.Value;
                mySqlCommandSel.Parameters["@VlrCapitalPAG"].Value = estadoCuentaCuotas.VlrCapitalPAG.Value;
                mySqlCommandSel.Parameters["@VlrInteresPAG"].Value = estadoCuentaCuotas.VlrInteresPAG.Value;
                mySqlCommandSel.Parameters["@VlrSeguroPAG"].Value = estadoCuentaCuotas.VlrSeguroPAG.Value;
                mySqlCommandSel.Parameters["@VlrOtro1PAG"].Value = estadoCuentaCuotas.VlrOtro1PAG.Value;
                mySqlCommandSel.Parameters["@VlrOtro2PAG"].Value = estadoCuentaCuotas.VlrOtro2PAG.Value;
                mySqlCommandSel.Parameters["@VlrOtro3PAG"].Value = estadoCuentaCuotas.VlrOtro3PAG.Value;
                mySqlCommandSel.Parameters["@VlrOtrosfijosPAG"].Value = estadoCuentaCuotas.VlrOtrosfijosPAG.Value;
                mySqlCommandSel.Parameters["@VlrPRJ"].Value = estadoCuentaCuotas.VlrPRJ.Value;
                mySqlCommandSel.Parameters["@VlrMora"].Value = estadoCuentaCuotas.VlrMora.Value;
                mySqlCommandSel.Parameters["@VlrGastos"].Value = estadoCuentaCuotas.VlrGastos.Value;
                mySqlCommandSel.Parameters["@VlrMoraSDO"].Value = estadoCuentaCuotas.VlrMoraSDO.Value;
                mySqlCommandSel.Parameters["@IndCuotaVencida"].Value = estadoCuentaCuotas.IndCuotaVencida.Value;
                mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
                #endregion

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
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccEstadoCuentaCuotas_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccEstadoCuentaComponetes
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccEstadoCuentaCuotas_Update(DTO_ccEstadoCuentaCuotas estadoCuentaCuotas)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query
                mySqlCommandSel.CommandText =
                                "UPDATE ccEstadoCuentaCuotas" +
                                "   NumeroDoc = @NumeroDoc "   +
                                "  ,EC_FijadoInd =	@EC_FijadoInd " +
                                "  ,CuotaID =	@CuotaID " +
                                "  ,FechaCuota =	@FechaCuota " +
                                "  ,CompradorCarteraID =	@CompradorCarteraID " +
                                "  ,VlrCuota =	@VlrCuota " +
                                "  ,VlrCapitalSDO =	@VlrCapitalSDO " +
                                "  ,VlrInteresSDO =	@VlrInteresSDO " +
                                "  ,VlrSeguroSDO =	@VlrSeguroSDO " +
                                "  ,VlrOtro1SDO =	@VlrOtro1SDO " +
                                "  ,VlrOtro2SDO =	@VlrOtro2SDO " +
                                "  ,VlrOtro3SDO =	@VlrOtro3SDO " +
                                "  ,VlrOtrosfijosSDO =	@VlrOtrosfijosSDO " +
                                "  ,VlrCapitalABO =	@VlrCapitalABO " +
                                "  ,VlrInteresABO =	@VlrInteresABO " +
                                "  ,VlrSeguroABO =	@VlrSeguroABO " +
                                "  ,VlrOtro1ABO =	@VlrOtro1ABO " +
                                "  ,VlrOtro2ABO =	@VlrOtro2ABO " +
                                "  ,VlrOtro3ABO =	@VlrOtro3ABO " +
                                "  ,VlrOtrosfijosABO =	@VlrOtrosfijosABO " +
                                "  ,VlrCapitalPAG =	@VlrCapitalPAG " +
                                "  ,VlrInteresPAG =	@VlrInteresPAG " +
                                "  ,VlrSeguroPAG =	@VlrSeguroPAG " +
                                "  ,VlrOtro1PAG =	@VlrOtro1PAG " +
                                "  ,VlrOtro2PAG =	@VlrOtro2PAG " +
                                "  ,VlrOtro3PAG =	@VlrOtro3PAG " +
                                "  ,VlrOtrosfijosPAG =	@VlrOtrosfijosPAG " +
                                "  ,VlrPRJ =	@VlrPRJ " +
                                "  ,VlrMora =	@VlrMora " +
                                "  ,VlrGastos =	@VlrGastos " +
                                "  ,VlrMoraSDO = @VlrMoraSDO " +
                                "  ,IndCuotaVencida = @IndCuotaVencida " +
                                "  ,eg_ccCompradorCartera = @eg_ccCompradorCartera " +
                                " WHERE  NumeroDoc = @NumeroDoc";
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CuotaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaCuota", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCapitalSDO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrInteresSDO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSeguroSDO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro1SDO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro2SDO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro3SDO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtrosfijosSDO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCapitalABO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrInteresABO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSeguroABO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro1ABO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro2ABO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro3ABO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtrosfijosABO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCapitalPAG", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrInteresPAG", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSeguroPAG", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro1PAG", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro2PAG", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro3PAG", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtrosfijosPAG", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPRJ", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrMora", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrGastos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrMoraSDO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IndCuotaVencida", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = estadoCuentaCuotas.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@CuotaID"].Value = estadoCuentaCuotas.CuotaID.Value;
                mySqlCommandSel.Parameters["@FechaCuota"].Value = estadoCuentaCuotas.FechaCuota.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = estadoCuentaCuotas.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = estadoCuentaCuotas.VlrCuota.Value;
                mySqlCommandSel.Parameters["@VlrCapitalSDO"].Value = estadoCuentaCuotas.VlrCapitalSDO.Value;
                mySqlCommandSel.Parameters["@VlrInteresSDO"].Value = estadoCuentaCuotas.VlrInteresSDO.Value;
                mySqlCommandSel.Parameters["@VlrSeguroSDO"].Value = estadoCuentaCuotas.VlrSeguroSDO.Value;
                mySqlCommandSel.Parameters["@VlrOtro1SDO"].Value = estadoCuentaCuotas.VlrOtro1SDO.Value;
                mySqlCommandSel.Parameters["@VlrOtro2SDO"].Value = estadoCuentaCuotas.VlrOtro2SDO.Value;
                mySqlCommandSel.Parameters["@VlrOtro3SDO"].Value = estadoCuentaCuotas.VlrOtro3SDO.Value;
                mySqlCommandSel.Parameters["@VlrOtrosfijosSDO"].Value = estadoCuentaCuotas.VlrOtrosfijosSDO.Value;
                mySqlCommandSel.Parameters["@VlrCapitalABO"].Value = estadoCuentaCuotas.VlrCapitalABO.Value;
                mySqlCommandSel.Parameters["@VlrInteresABO"].Value = estadoCuentaCuotas.VlrInteresABO.Value;
                mySqlCommandSel.Parameters["@VlrSeguroABO"].Value = estadoCuentaCuotas.VlrSeguroABO.Value;
                mySqlCommandSel.Parameters["@VlrOtro1ABO"].Value = estadoCuentaCuotas.VlrOtro1ABO.Value;
                mySqlCommandSel.Parameters["@VlrOtro2ABO"].Value = estadoCuentaCuotas.VlrOtro2ABO.Value;
                mySqlCommandSel.Parameters["@VlrOtro3ABO"].Value = estadoCuentaCuotas.VlrOtro3ABO.Value;
                mySqlCommandSel.Parameters["@VlrOtrosfijosABO"].Value = estadoCuentaCuotas.VlrOtrosfijosABO.Value;
                mySqlCommandSel.Parameters["@VlrCapitalPAG"].Value = estadoCuentaCuotas.VlrCapitalPAG.Value;
                mySqlCommandSel.Parameters["@VlrInteresPAG"].Value = estadoCuentaCuotas.VlrInteresPAG.Value;
                mySqlCommandSel.Parameters["@VlrSeguroPAG"].Value = estadoCuentaCuotas.VlrSeguroPAG.Value;
                mySqlCommandSel.Parameters["@VlrOtro1PAG"].Value = estadoCuentaCuotas.VlrOtro1PAG.Value;
                mySqlCommandSel.Parameters["@VlrOtro2PAG"].Value = estadoCuentaCuotas.VlrOtro2PAG.Value;
                mySqlCommandSel.Parameters["@VlrOtro3PAG"].Value = estadoCuentaCuotas.VlrOtro3PAG.Value;
                mySqlCommandSel.Parameters["@VlrOtrosfijosPAG"].Value = estadoCuentaCuotas.VlrOtrosfijosPAG.Value;
                mySqlCommandSel.Parameters["@VlrPRJ"].Value = estadoCuentaCuotas.VlrPRJ.Value;
                mySqlCommandSel.Parameters["@VlrMora"].Value = estadoCuentaCuotas.VlrMora.Value;
                mySqlCommandSel.Parameters["@VlrGastos"].Value = estadoCuentaCuotas.VlrGastos.Value;
                mySqlCommandSel.Parameters["@VlrMoraSDO"].Value = estadoCuentaCuotas.VlrMoraSDO.Value;
                mySqlCommandSel.Parameters["@IndCuotaVencida"].Value = estadoCuentaCuotas.IndCuotaVencida.Value;
                mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);

                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccEstadoCuentaCuotas_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los componentes asociados a un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_ccEstadoCuentaCuotas_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandText = "DELETE FROM ccEstadoCuentaCuotas WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccEstadoCuentaCuotas_Delete");
                throw exception;
            }
        }

    }
}
