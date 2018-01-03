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

namespace NewAge.ADO
{
    public class DAL_noCompensatorio : DAL_Base
    {
      /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_noCompensatorio(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
        
        /// <summary>
        /// Obtiene el historico de compensatorios
        /// </summary>
        /// <returns>listado de compensatorios</returns>
        public List<DTO_noCompensatorios> DAL_noCompensatorio_GetCompensatorios()
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "   SELECT [EmpresaID]    " +
                                              "   ,[Periodo]    " +
                                              "   ,[ContratoNOID]   " +
                                              "   ,[Dia1]    " +
                                              "   ,[Dia2]    " +
                                              "   ,[Dia3]    " +
                                              "   ,[Dia4]    " +
                                              "   ,[Dia5]    " +
                                              "   ,[Dia6]    " +
                                              "   ,[Dia7]    " +
                                              "   ,[Dia8]    " +
                                              "   ,[Dia9]    " +
                                              "   ,[Dia10]    " +
                                              "   ,[Dia11]    " +
                                              "   ,[Dia12]    " +
                                              "   ,[Dia13]    " +
                                              "   ,[Dia14]    " +
                                              "   ,[Dia15]    " +
                                              "   ,[Dia16]    " +
                                              "   ,[Dia17]    " +
                                              "   ,[Dia18]    " +
                                              "   ,[Dia19]    " +
                                              "   ,[Dia20]    " +
                                              "   ,[Dia21]    " +
                                              "   ,[Dia22]    " +
                                              "   ,[Dia23]    " +
                                              "   ,[Dia24]    " +
                                              "   ,[Dia25]    " +
                                              "   ,[Dia26]    " +
                                              "   ,[Dia27]    " +
                                              "   ,[Dia28]    " +
                                              "   ,[Dia29]    " +
                                              "   ,[Dia30]    " +
                                              "   ,[Dia31]    " +
                                              "   ,[DiasTrabajoMes]    " +
                                              "   ,[DiasDescansoMes]    " +
                                              "   ,[DiasSaldoAnt]    " +
                                              "   ,[DiasMes]    " +
                                              "   ,[DiasPagados]    " +
                                              "   ,[DiasAjustados]    " +
                                              "   ,[DIasNuevoSaldo]    " +
                                              "   FROM noCompensatoriosHistoria with(nolock)    ";
                                                       
               
                List<DTO_noCompensatorios> result = new List<DTO_noCompensatorios>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_noCompensatorios dto = new DTO_noCompensatorios(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noCompensatorio_GetCompensatorios");
                throw exception;
            }
        }
        
        /// <summary>
        /// Actualiza la informacion del compensatorio
        /// </summary>
        /// <param name="compesatorio">objeto compensatorio</param>
        /// <returns>true si la operacion es exitosa</returns>
        public bool DAL_noCompesatorios_UpdCompensatorio(DTO_noCompensatorios compesatorio)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  UPDATE noCompensatoriosHistoria    " +
                                              "  SET [Dia1] = @Dia1    " +
                                              "  ,[Dia2] = @Dia2    " +
                                              "  ,[Dia3] = @Dia3    " +
                                              "  ,[Dia4] = @Dia4    " +
                                              "  ,[Dia5] = @Dia5    " +
                                              "  ,[Dia6] = @Dia6    " +
                                              "  ,[Dia7] = @Dia7    " +
                                              "  ,[Dia8] = @Dia8    " +
                                              "  ,[Dia9] = @Dia9    " +
                                              "  ,[Dia10] = @Dia10    " +
                                              "  ,[Dia11] = @Dia11    " +
                                              "  ,[Dia12] = @Dia12    " +
                                              "  ,[Dia13] = @Dia13    " +
                                              "  ,[Dia14] = @Dia14    " +
                                              "  ,[Dia15] = @Dia15    " +
                                              "  ,[Dia16] = @Dia16    " +
                                              "  ,[Dia17] = @Dia17    " +
                                              "  ,[Dia18] = @Dia18    " +
                                              "  ,[Dia19] = @Dia19    " +
                                              "  ,[Dia20] = @Dia20    " +
                                              "  ,[Dia21] = @Dia21    " +
                                              "  ,[Dia22] = @Dia22    " +
                                              "  ,[Dia23] = @Dia23    " +
                                              "  ,[Dia24] = @Dia24    " +
                                              "  ,[Dia25] = @Dia25    " +
                                              "  ,[Dia26] = @Dia26    " +
                                              "  ,[Dia27] = @Dia27    " +
                                              "  ,[Dia28] = @Dia28    " +
                                              "  ,[Dia29] = @Dia29    " +
                                              "  ,[Dia30] = @Dia30    " +
                                              "  ,[Dia31] = @Dia31    " +
                                              "  ,[DiasTrabajoMes] = @DiasTrabajoMes    " +
                                              "  ,[DiasDescansoMes] = @DiasDescansoMes    " +
                                              "  ,[DiasSaldoAnt] = @DiasSaldoAnt    " +
                                              "  ,[DiasMes] = @DiasMes    " +
                                              "  ,[DiasPagados] = @DiasPagados    " +
                                              "  ,[DiasAjustados] = @DiasAjustados    " +
                                              "  ,[DIasNuevoSaldo] = @DIasNuevoSaldo    " +
                                              "   WHERE EmpresaID = @EmpresaID    " +
                                                "   AND Periodo = @Periodo    " +
                                                "   AND ContratoNOID = @ContratoNOID    ";


                mySqlCommandSel.Parameters.Add("@Dia1", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia2", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia3", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia4", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia5", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia6", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia7", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia8", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia9", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia10", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia11", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia12", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia13", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia14", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia15", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia16", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia17", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia18", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia19", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia20", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia21", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia22", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia23", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia24", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia25", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia26", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia27", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia28", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia29", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia30", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Dia31", SqlDbType.Char);

                mySqlCommandSel.Parameters.Add("@DiasTrabajoMes", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DiasDescansoMes", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DiasSaldoAnt", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DiasMes", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DiasPagados", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DiasAjustados", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DIasNuevoSaldo", SqlDbType.TinyInt);

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@ContratoNOID", SqlDbType.Int);


                mySqlCommandSel.Parameters["@Dia1"].Value = compesatorio.Dia1.Value;
                mySqlCommandSel.Parameters["@Dia2"].Value = compesatorio.Dia2.Value;
                mySqlCommandSel.Parameters["@Dia3"].Value = compesatorio.Dia3.Value;
                mySqlCommandSel.Parameters["@Dia4"].Value = compesatorio.Dia4.Value;
                mySqlCommandSel.Parameters["@Dia5"].Value = compesatorio.Dia5.Value;
                mySqlCommandSel.Parameters["@Dia6"].Value = compesatorio.Dia6.Value;
                mySqlCommandSel.Parameters["@Dia7"].Value = compesatorio.Dia7.Value;
                mySqlCommandSel.Parameters["@Dia8"].Value = compesatorio.Dia8.Value;
                mySqlCommandSel.Parameters["@Dia9"].Value = compesatorio.Dia9.Value;
                mySqlCommandSel.Parameters["@Dia10"].Value = compesatorio.Dia10.Value;
                mySqlCommandSel.Parameters["@Dia11"].Value = compesatorio.Dia11.Value;
                mySqlCommandSel.Parameters["@Dia12"].Value = compesatorio.Dia12.Value;
                mySqlCommandSel.Parameters["@Dia13"].Value = compesatorio.Dia13.Value;
                mySqlCommandSel.Parameters["@Dia14"].Value = compesatorio.Dia14.Value;
                mySqlCommandSel.Parameters["@Dia15"].Value = compesatorio.Dia15.Value;
                mySqlCommandSel.Parameters["@Dia16"].Value = compesatorio.Dia16.Value;
                mySqlCommandSel.Parameters["@Dia17"].Value = compesatorio.Dia17.Value;
                mySqlCommandSel.Parameters["@Dia18"].Value = compesatorio.Dia18.Value;
                mySqlCommandSel.Parameters["@Dia19"].Value = compesatorio.Dia19.Value;
                mySqlCommandSel.Parameters["@Dia20"].Value = compesatorio.Dia20.Value;
                mySqlCommandSel.Parameters["@Dia21"].Value = compesatorio.Dia21.Value;
                mySqlCommandSel.Parameters["@Dia22"].Value = compesatorio.Dia22.Value;
                mySqlCommandSel.Parameters["@Dia23"].Value = compesatorio.Dia23.Value;
                mySqlCommandSel.Parameters["@Dia24"].Value = compesatorio.Dia24.Value;
                mySqlCommandSel.Parameters["@Dia25"].Value = compesatorio.Dia25.Value;
                mySqlCommandSel.Parameters["@Dia26"].Value = compesatorio.Dia26.Value;
                mySqlCommandSel.Parameters["@Dia27"].Value = compesatorio.Dia27.Value;
                mySqlCommandSel.Parameters["@Dia28"].Value = compesatorio.Dia28.Value;
                mySqlCommandSel.Parameters["@Dia29"].Value = compesatorio.Dia29.Value;
                mySqlCommandSel.Parameters["@Dia30"].Value = compesatorio.Dia30.Value;
                mySqlCommandSel.Parameters["@Dia31"].Value = compesatorio.Dia31.Value;


                mySqlCommandSel.Parameters["@DiasTrabajoMes"].Value = compesatorio.DiasTrabajoMes.Value;
                mySqlCommandSel.Parameters["@DiasDescansoMes"].Value = compesatorio.DiasDescansoMes.Value;
                mySqlCommandSel.Parameters["@DiasSaldoAnt"].Value = compesatorio.DiasSaldoAnt.Value;
                mySqlCommandSel.Parameters["@DiasMes"].Value = compesatorio.DiasMes.Value;
                mySqlCommandSel.Parameters["@DiasPagados"].Value = compesatorio.DiasPagados.Value;
                mySqlCommandSel.Parameters["@DiasAjustados"].Value = compesatorio.DiasAjustados.Value;
                mySqlCommandSel.Parameters["@DIasNuevoSaldo"].Value = compesatorio.DIasNuevoSaldo.Value;

                mySqlCommandSel.Parameters["@EmpresaID"].Value = compesatorio.EmpresaID.Value;
                mySqlCommandSel.Parameters["@Periodo"].Value = compesatorio.Periodo.Value;
                mySqlCommandSel.Parameters["@ContratoNOID"].Value = compesatorio.ContratoNOID.Value;
              

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noCompesatorios_UpdCompensatorio");
                throw exception;
            }
        }
    }
}
