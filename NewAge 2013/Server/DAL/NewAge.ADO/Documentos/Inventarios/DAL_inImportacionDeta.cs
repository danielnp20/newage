using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL_inImportacionDeta
    /// </summary>
    public class DAL_inImportacionDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_inImportacionDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Adiciona en tabla inImportacionDeta
        /// </summary>
        /// <param name="importacionDeta">items a agregar a inImportacionDeta</param>
        /// <returns>Numero Doc</returns>
        public void DAL_inImportacionDeta_Add(DTO_inImportacionDeta importacionDeta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = " INSERT INTO inImportacionDeta " +
                                            "(NumeroDoc " +
                                            ",ConsSaldoExistencia " +
                                            ",NumeroDocNotaEnv " +
                                            ",NumeroDocFactura " +
                                            ",Cantidad " +
                                            ",ValorUnidadUS " +
                                            ",ValorCostoUS " +  
                                            ",ValorOtrosUS " +
                                            ",ValorTotalUS " +
                                            ",ValorTotalPS " +
                                            ",PosArancelaria " +
                                            ",PorArancel " +
                                            ",PorIVA " +
                                            ",ValorArancel " +
                                            ",ValorIVA " +
                                            ",ValorOtrosPS " +
                                            ",CostoTotalUS " +
                                            ",CostoTotalPS) " +
                                            "VALUES" +
                                            "(@NumeroDoc " +
                                            ",@ConsSaldoExistencia " +
                                            ",@NumeroDocNotaEnv " +
                                            ",@NumeroDocFactura " +
                                            ",@Cantidad " +
                                            ",@ValorUnidadUS " +
                                            ",@ValorCostoUS " +     
                                            ",@ValorOtrosUS " +
                                            ",@ValorTotalUS " +
                                            ",@ValorTotalPS " +
                                            ",@PosArancelaria " +
                                            ",@PorArancel " +
                                            ",@PorIVA " +
                                            ",@ValorArancel " +
                                            ",@ValorIVA " +
                                            ",@ValorOtrosPS " +
                                            ",@CostoTotalUS " +
                                            ",@CostoTotalPS) " +
                                            " SET @Consecutivo = SCOPE_IDENTITY()";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ConsSaldoExistencia", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NumeroDocNotaEnv", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NumeroDocFactura", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorUnidadUS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorCostoUS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorOtrosUS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorTotalUS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorTotalPS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PosArancelaria", SqlDbType.Char, 25);
                mySqlCommand.Parameters.Add("@PorArancel", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorIVA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorIVA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorArancel", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorOtrosPS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CostoTotalUS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CostoTotalPS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int, 1);
                mySqlCommand.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = importacionDeta.NumeroDoc.Value;
                mySqlCommand.Parameters["@ConsSaldoExistencia"].Value = importacionDeta.ConsSaldoExistencia.Value;
                mySqlCommand.Parameters["@NumeroDocNotaEnv"].Value = importacionDeta.NumeroDocNotaEnv.Value;
                mySqlCommand.Parameters["@NumeroDocFactura"].Value = importacionDeta.NumeroDocFactura.Value;
                mySqlCommand.Parameters["@Cantidad"].Value = importacionDeta.Cantidad.Value;
                mySqlCommand.Parameters["@ValorUnidadUS"].Value = importacionDeta.ValorUnidadUS.Value;
                mySqlCommand.Parameters["@ValorCostoUS"].Value = importacionDeta.ValorCostoUS.Value;
                mySqlCommand.Parameters["@ValorOtrosUS"].Value = importacionDeta.ValorOtrosUS.Value;
                mySqlCommand.Parameters["@ValorTotalUS"].Value = importacionDeta.ValorTotalUS.Value;
                mySqlCommand.Parameters["@ValorTotalPS"].Value = importacionDeta.ValorTotalPS.Value;
                mySqlCommand.Parameters["@PosArancelaria"].Value = importacionDeta.PosArancelaria.Value;
                mySqlCommand.Parameters["@PorArancel"].Value = importacionDeta.PorArancel.Value;
                mySqlCommand.Parameters["@PorIVA"].Value = importacionDeta.PorIVA.Value;
                mySqlCommand.Parameters["@ValorIVA"].Value = importacionDeta.ValorIVA.Value;
                mySqlCommand.Parameters["@ValorArancel"].Value = importacionDeta.ValorArancel.Value;
                mySqlCommand.Parameters["@ValorOtrosPS"].Value = importacionDeta.ValorOtrosPS.Value;
                mySqlCommand.Parameters["@CostoTotalUS"].Value = importacionDeta.CostoTotalUS.Value;
                mySqlCommand.Parameters["@CostoTotalPS"].Value = importacionDeta.CostoTotalPS.Value;
                #endregion
                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommand.ExecuteNonQuery();
                int numDoc = Convert.ToInt32(mySqlCommand.Parameters["@Consecutivo"].Value);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inImportacionDeta_Add");
                throw exception;
            }

        }

        ///// <summary>
        ///// Trae informacion de acuerdo al filtro
        ///// </summary>
        ///// <param name="fisicoInventario"></param>
        ///// <returns>una liquidacion Deta</returns>
        //public DTO_inImportacionDeta DAL_inImportacionDeta_GetByNumeroDoc(int numeroDoc)
        //{
        //    try
        //    {
        //        SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
        //        mySqlCommand.Transaction = base.MySqlConnectionTx;

        //        mySqlCommand.CommandText = "select * from inImportacionDeta with(nolock) where inImportacionDeta.NumeroDoc = @NumeroDoc ";

        //        mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
        //        mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

        //        DTO_inImportacionDeta result = null;
        //        SqlDataReader dr = mySqlCommand.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            result = new DTO_inImportacionDeta(dr);
        //        }
        //        dr.Close();
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
        //        Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inImportacionDeta_GetByNumeroDoc");
        //        throw exception;
        //    }
        //}

        /// <summary>
        /// Consulta una importacion Header segun un filtro de parametros
        /// </summary>
        /// <param name="numeroDoc">Numero Doc</param>
        /// <returns>Dto de importacion </returns>
        public List<DTO_inImportacionDeta> DAL_inImportacionDeta_GetByNumeroDoc(int numeroDoc)
        {
            try
            {
                List<DTO_inImportacionDeta> result = new List<DTO_inImportacionDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommand.CommandText = "select * from inImportacionDeta with(nolock) " +
                                           "where NumeroDoc = @NumeroDoc "; 

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_inImportacionDeta fisico = new DTO_inImportacionDeta(dr);
                    result.Add(fisico);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inImportacionDeta_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Actualizar inImportacionDeta
        /// </summary>
        /// <param name="mvtoHeader">importacion</param>
        public void DAL_inImportacionDeta_Upd(DTO_inImportacionDeta importacionDeta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla inImportacionDeta
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE inImportacionDeta " +
                                           "    SET ConsSaldoExistencia  = @ConsSaldoExistencia  " +
                                           "    ,NumeroDocNotaEnv  = @NumeroDocNotaEnv  " +
                                           "    ,NumeroDocFactura  = @NumeroDocFactura  " +
                                           "    ,Cantidad  = @Cantidad " +
                                           "    ,ValorUnidadUS  = @ValorUnidadUS " +
                                           "    ,ValorCostoUS  = @ValorCostoUS " +
                                           "    ,ValorOtrosUS  = @ValorOtrosUS " +        
                                           "    ,ValorTotalUS  = @ValorTotalUS " +
                                           "    ,ValorTotalPS  = @ValorTotalPS " +
                                           "    ,PosArancelaria  = @PosArancelaria " +
                                           "    ,PorArancel  = @PorArancel " +
                                           "    ,PorIVA  = @PorIVA " +
                                           "    ,ValorArancel  = @ValorArancel " +
                                           "    ,ValorIVA  = @ValorIVA " +
                                           "    ,ValorOtrosPS  = @ValorOtrosPS " +
                                           "    ,CostoTotalUS  = @CostoTotalUS " +
                                           "    ,CostoTotalPS  = @CostoTotalPS " +
                                           "    WHERE Consecutivo = @Consecutivo";                
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@ConsSaldoExistencia", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NumeroDocNotaEnv", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NumeroDocFactura", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorUnidadUS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorCostoUS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorOtrosUS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorTotalUS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorTotalPS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PosArancelaria", SqlDbType.Char, 25);
                mySqlCommand.Parameters.Add("@PorArancel", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorArancel", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorIVA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorArancel", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorOtrosPS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CostoTotalUS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CostoTotalPS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asignacion de valores               
                mySqlCommand.Parameters["@ConsSaldoExistencia"].Value = importacionDeta.ConsSaldoExistencia.Value;
                mySqlCommand.Parameters["@NumeroDocNotaEnv"].Value = importacionDeta.NumeroDocNotaEnv.Value;
                mySqlCommand.Parameters["@NumeroDocFactura"].Value = importacionDeta.NumeroDocFactura.Value;
                mySqlCommand.Parameters["@Cantidad"].Value = importacionDeta.Cantidad.Value;
                mySqlCommand.Parameters["@ValorUnidadUS"].Value = importacionDeta.ValorUnidadUS.Value;
                mySqlCommand.Parameters["@ValorCostoUS"].Value = importacionDeta.ValorCostoUS.Value;
                mySqlCommand.Parameters["@ValorOtrosUS"].Value = importacionDeta.ValorOtrosUS.Value;
                mySqlCommand.Parameters["@ValorTotalUS"].Value = importacionDeta.ValorTotalUS.Value;
                mySqlCommand.Parameters["@ValorTotalPS"].Value = importacionDeta.ValorTotalPS.Value;
                mySqlCommand.Parameters["@PosArancelaria"].Value = importacionDeta.PosArancelaria.Value;
                mySqlCommand.Parameters["@PorArancel"].Value = importacionDeta.PorArancel.Value;
                mySqlCommand.Parameters["@ValorArancel"].Value = importacionDeta.ValorArancel.Value;
                mySqlCommand.Parameters["@ValorIVA"].Value = importacionDeta.ValorIVA.Value;
                mySqlCommand.Parameters["@ValorArancel"].Value = importacionDeta.ValorArancel.Value;
                mySqlCommand.Parameters["@ValorOtrosPS"].Value = importacionDeta.ValorOtrosPS.Value;
                mySqlCommand.Parameters["@CostoTotalUS"].Value = importacionDeta.CostoTotalUS.Value;
                mySqlCommand.Parameters["@CostoTotalPS"].Value = importacionDeta.CostoTotalPS.Value;
                mySqlCommand.Parameters["@Consecutivo"].Value = importacionDeta.Consecutivo.Value;
                #endregion
                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inImportacionDeta_Upd");
                throw exception;
            }

        }

        #endregion
    }
}
