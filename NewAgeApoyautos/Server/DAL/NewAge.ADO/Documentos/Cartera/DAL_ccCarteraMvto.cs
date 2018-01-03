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
    public class DAL_ccCarteraMvto : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCarteraMvto(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Añade los un registro a ccCarteraMvto
        /// </summary>
        /// <param name="mvto">Movimiento de la caretra</param>
        public void DAL_ccCarteraMvto_Add(DTO_ccCarteraMvto mvto)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                
                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO ccCarteraMvto (NumCredito,NumeroDoc,EmpresaID,ComponenteCarteraID,CompradorCarteraID,Tasa,VlrComponente,VlrAbono,eg_ccCarteraComponente,eg_ccCompradorCartera)" +
                    "VALUES (@NumCredito,@NumeroDoc,@EmpresaID,@ComponenteCarteraID,@CompradorCarteraID,@Tasa,@VlrComponente,@VlrAbono,@eg_ccCarteraComponente,@eg_ccCompradorCartera)";
                #endregion

                #region Creacion Parametros

                mySqlCommandSel.Parameters.Add("@NumCredito", SqlDbType.Int); 
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Tasa", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrComponente", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrAbono", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumCredito"].Value = mvto.NumCredito.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = mvto.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ComponenteCarteraID"].Value = mvto.ComponenteCarteraID.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = mvto.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@Tasa"].Value = mvto.Tasa.Value;
                mySqlCommandSel.Parameters["@VlrComponente"].Value = mvto.VlrComponente.Value;
                mySqlCommandSel.Parameters["@VlrAbono"].Value = mvto.VlrAbono.Value;
                mySqlCommandSel.Parameters["@eg_ccCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = !string.IsNullOrWhiteSpace(mvto.CompradorCarteraID.Value) ? 
                    this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl) : string.Empty;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCarteraMvto_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Añade los un registro a ccCarteraMvto
        /// </summary>
        /// <param name="mvto">Movimiento de la caretra</param>
        public void DAL_ccCarteraMvto_Revertir(int numDocCredito, int numDocReversion, int numDocNuevo)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;


                mySqlCommandSel.CommandText =
                    "insert into ccCarteraMvto(NumCredito, NumeroDoc, EmpresaID, ComponenteCarteraID, CompradorCarteraID, Tasa, VlrComponente, " +
                    "                VlrAbono, eg_ccCarteraComponente, eg_ccCompradorCartera) " +
                    "	SELECT NumCredito, @NumeroDocNew, EmpresaID, ComponenteCarteraID, CompradorCarteraID, Tasa, VlrComponente * -1 AS VlrComponente, " + 
                    "       VlrAbono * -1 AS VlrAbono, eg_ccCarteraComponente, eg_ccCompradorCartera " +
                    "	FROM ccCarteraMvto with(nolock) where NumCredito = @NumCredito and NumeroDoc = @NumeroDocOld ";

                mySqlCommandSel.Parameters.Add("@NumCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDocOld", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDocNew", SqlDbType.Int);

                mySqlCommandSel.Parameters["@NumCredito"].Value = numDocCredito;
                mySqlCommandSel.Parameters["@NumeroDocOld"].Value = numDocReversion;
                mySqlCommandSel.Parameters["@NumeroDocNew"].Value = numDocNuevo;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCarteraMvto_Revertir");
                throw exception;
            }
        }

        #endregion

        #region Otros

        /// <summary>
        /// Funcion que trae los movimientos de cartera de una libranza
        /// </summary>
        /// <param name="libranza">libranza</param>
        /// <param name="CompCAP">Componente CApital</param>
        /// <param name="CompINT">Componente Interes</param>
        /// <param name="CompSEG">Componente Seguro</param>
        /// <param name="CompINS">Componente Interes Seguro</param>
        /// <param name="CompMOR">Componente Interes mora</param>
        /// <param name="CompPRJ">Componente Prefuridico</param>
        /// <param name="CompFAV">Componente Saldo a favor</param>
        /// <returns>Lista</returns>
        public List<DTO_QueryCarteraMvto> DAL_CarteraMvto_QueryByLibranza(int? libranza,string clienteID, bool byCuota, string CompCAP, string CompINT, string CompSEG, string CompINS, string CompMOR, string CompPRJ, string CompFAV,bool abonoInd)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                List<DTO_QueryCarteraMvto> result = new List<DTO_QueryCarteraMvto>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompCAP", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompINT", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompSEG", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompINS", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompMOR", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompPRJ", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompFAV", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AbonoInd", SqlDbType.Bit); 

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Libranza"].Value = libranza;
                mySqlCommandSel.Parameters["@ClienteID"].Value = clienteID;
                mySqlCommandSel.Parameters["@CompCAP"].Value = CompCAP;
                mySqlCommandSel.Parameters["@CompINT"].Value = CompINT;
                mySqlCommandSel.Parameters["@CompSEG"].Value = CompSEG;
                mySqlCommandSel.Parameters["@CompINS"].Value = CompINS;
                mySqlCommandSel.Parameters["@CompMOR"].Value = CompMOR;
                mySqlCommandSel.Parameters["@CompPRJ"].Value = CompPRJ;
                mySqlCommandSel.Parameters["@CompFAV"].Value = CompFAV;
                mySqlCommandSel.Parameters["@AbonoInd"].Value = abonoInd;

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                #endregion

                if (!byCuota)
                {
                    mySqlCommandSel.CommandText =
                           "SELECT * FROM Cartera_MovimientosByLibranza (@EmpresaID,@Libranza,@ClienteID,@CompCAP, @CompINT, @CompSEG, @CompINS,@CompMOR, @CompPRJ, @CompFAV,@AbonoInd)";

                    SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                    while (dr.Read())
                    {
                        DTO_QueryCarteraMvto mvto = new DTO_QueryCarteraMvto(dr, true);

                        mvto.PrefDoc.Value = mvto.PrefijoID.Value + "-" + mvto.DocumentoNro.Value.ToString();
                        mvto.Comprobante.Value = mvto.ComprobanteID.Value + "-" + mvto.ComprobanteIDNro.Value.ToString();
                        result.Add(mvto);
                    }
                    dr.Close();
                }
                else
                {
                    mySqlCommandSel.CommandText =
                           "SELECT * FROM Cartera_MovimientosByLibranzaAndCuota (@EmpresaID,@Libranza,@ClienteID,@CompCAP, @CompINT, @CompSEG, @CompINS,@CompMOR, @CompPRJ, @CompFAV,@AbonoInd)";

                    SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                    while (dr.Read())
                    {
                        DTO_QueryCarteraMvto mvto = new DTO_QueryCarteraMvto(dr, true);

                        mvto.PrefDoc.Value = mvto.PrefijoID.Value + "-" + mvto.DocumentoNro.Value.ToString();
                        mvto.Comprobante.Value = mvto.ComprobanteID.Value + "-" + mvto.ComprobanteIDNro.Value.ToString();
                        mvto.CuotaID = dr["CuotaID"].ToString();
                        result.Add(mvto);
                    }
                    dr.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CarteraMvto_QueryByLibranza");
                throw exception;
            }
        }

        #endregion
    }
}
