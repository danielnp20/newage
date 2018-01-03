using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Diagnostics;

namespace NewAge.ADO
{
    public class DAL_ccComponenteEdad : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccComponenteEdad(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        public DTO_ccComponenteEdad DAL_ccComponenteEdad_GetByEdad(string componenteID, int edad)
        {
            try
            {
                DTO_ccComponenteEdad result = null;
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommand.Parameters.Add("@Edad", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@ComponenteCarteraID"].Value =componenteID;
                mySqlCommand.Parameters["@Edad"].Value = edad;
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glEmpresaGrupo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@ActivoInd"].Value = true;

                mySqlCommand.CommandText =
                    "select top(1) * from ccComponenteEdad with(nolock) " +
                    "where ComponenteCarteraID = @ComponenteCarteraID and Edad >= @Edad " +
                    "   and EmpresaGrupoID = @EmpresaGrupoID and eg_ccCarteraComponente = @eg_ccCarteraComponente " +
                    "   and ActivoInd = @ActivoInd " +
                    "order by Edad";

                SqlDataReader dr;

                dr = mySqlCommand.ExecuteReader();
                if(dr.Read())
                {
                    result = new DTO_ccComponenteEdad();

                    result.ComponenteCarteraID.Value = dr["ComponenteCarteraID"].ToString();
                    result.Edad.Value = Convert.ToInt32(dr["Edad"]);

                    result.Valor.Value = 0;
                    if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                        result.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                    
                    if (!string.IsNullOrEmpty(dr["PorcentajeID"].ToString()))
                        result.PorcentajeID.Value = Convert.ToDecimal(dr["PorcentajeID"]);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccComponenteEdad_GetByEdad");
                throw exception;
            }
        }
    }
}
