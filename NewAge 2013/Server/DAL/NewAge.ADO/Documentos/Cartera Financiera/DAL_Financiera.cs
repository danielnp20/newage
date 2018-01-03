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
using NewAge.DTO.Resultados;

namespace NewAge.ADO
{
    public class DAL_Financiera : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_Financiera(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId) : base(c, tx, empresa, userId) { }

        #region Funciones Generales

        /// <summary>
        /// Funcion que trae los saldos de los componentes de un credito
        /// </summary>
        /// <param name="numeroDoc">Numero de documento de consulta (pk)</param>
        /// <param name="periodo">Periodo de consulta para saber el saldo</param>
        /// <param name="fechaCorte">Fecha de corte para conocer la mora</param>
        /// <returns>Retorna os saldos de los componentes de un credito</returns>
        public List<DTO_ccSaldosComponentes> DAL_Financiera_GetComponentes(int numeroDoc, DateTime periodo, DateTime fechaCorte)
        {
            try
            {
                List<DTO_ccSaldosComponentes> result = new List<DTO_ccSaldosComponentes>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.SmallDateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@FechaCorte"].Value = fechaCorte;
                #endregion

                mySqlCommandSel.CommandText = "SELECT * FROM CarteraFin_CreditoComponentes (@NumeroDoc,@EmpresaID,@PeriodoID,@FechaCorte)";

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    if (string.IsNullOrWhiteSpace(dr["TotalSaldo"].ToString()))
                    {
                        result = null;
                        break;
                    }
                    else
                    {
                        DTO_ccSaldosComponentes saldosComp = new DTO_ccSaldosComponentes(dr);
                        saldosComp.Editable.Value = false;
                        result.Add(saldosComp);
                    }

                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_Cartera_GetComponentesStorePrecedure", false);
                throw exception;
            }
        }

        #endregion
    }
}
