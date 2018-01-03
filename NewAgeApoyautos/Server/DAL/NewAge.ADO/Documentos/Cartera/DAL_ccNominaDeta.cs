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
    public class DAL_ccNominaDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccNominaDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccNominaDeta
        /// </summary>
        /// <returns>retorna una lista de DTO_ccNominaDeta</returns>
        public List<DTO_ccNominaDeta> DAL_ccNominaDeta_GetByID(int NumDocNomina)
        {
            try
            {
                List<DTO_ccNominaDeta> result = new List<DTO_ccNominaDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumDocNomina", SqlDbType.Int);
                mySqlCommand.Parameters["@NumDocNomina"].Value = NumDocNomina;

                mySqlCommand.CommandText = "SELECT * FROM ccNominaDeta with(nolock)  " +
                                           "WHERE NumDocNomina = @NumDocNomina";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccNominaDeta r = new DTO_ccNominaDeta(dr);
                    result.Add(r);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaDeta_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega los registros de nominaDeta a nomina preliminar
        /// </summary>
        /// <param name="numeroDoc"></param>
        public void DAL_ccNominaDeta_UpdateNumDocRecibo(int numDocCredito, int numDocRecibo)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocRCaja", SqlDbType.Int);

                mySqlCommandSel.Parameters["@NumDocCredito"].Value = numDocCredito;
                mySqlCommandSel.Parameters["@NumDocRCaja"].Value = numDocRecibo;

                mySqlCommandSel.CommandText = "Update ccNominaDeta set NumDocRCaja = @NumDocRCaja Where NumDocCredito = @NumDocCredito and NumDocRCaja is null ";
                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaDeta_UpdateNumDocRecibo");
                throw exception;
            }
        }
        #endregion

        #region Otras

        /// <summary>
        /// Indica si existe una incorporacoion en un periodo
        /// </summary>
        /// <param name="centroPagoID">CIdentifidcador del centro de pago</param>
        /// <param name="fechaIni">Fecha inicial de consulta</param>
        /// <param name="fechaFin">Fecha final de consulta</param>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <returns>Verdadero si existe, de lo contrario falso</returns>
        public bool DAL_ccNominaDeta_HasMigracionCentroPago(string centroPagoID, DateTime fechaAplica)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@fechaAplica", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@eg_ccCentroPagoPAG", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = centroPagoID;
                mySqlCommandSel.Parameters["@fechaAplica"].Value = fechaAplica;
                mySqlCommandSel.Parameters["@eg_ccCentroPagoPAG"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCentroPagoPAG, this.Empresa, egCtrl);

                mySqlCommandSel.CommandText =
                    "select COUNT(*) " +
                    "from ccNominaDeta nom with(nolock) " +
                    "	inner join ccCreditoDocu cred with(nolock) on cred.NumeroDoc = nom.NumDocCredito and cred.EmpresaID = @EmpresaID " +
                    "where nom.CentroPagoID = @CentroPagoID and nom.eg_ccCentroPagoPAG = @eg_ccCentroPagoPAG and nom.FechaNomina = @fechaAplica " + 
                    "   and EstadoCruce not in (2, 3, 6)";

                int count = (int)mySqlCommandSel.ExecuteScalar();
                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaDeta_HasMigracionCliente");
                throw exception;
            }
        }

        /// <summary>
        /// Indica si existe un recaudo masivo en una fecha
        /// </summary>
        /// <param name="fecha">Fecha de la consulta</param>
        /// <returns>Verdadero si existe, de lo contrario falso</returns>
        public bool DAL_ccNominaDeta_HasMigracionByDay(DateTime fecha)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaNomina", SqlDbType.SmallDateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaNomina"].Value = fecha;

                mySqlCommandSel.CommandText =
                    "select COUNT(*) " +
                    "from ccNominaDeta nom with(nolock) " +
                    "	inner join glDocumentoControl ctrl with(nolock) on nom.NumDocCredito = ctrl.NumeroDoc and ctrl.EmpresaID = @EmpresaID " +
                    "where nom.FechaNomina = @FechaNomina";

                int count = (int)mySqlCommandSel.ExecuteScalar();
                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaDeta_HasMigracionCliente");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega los registros de nominaDeta a nomina preliminar
        /// </summary>
        /// <param name="numeroDoc"></param>
        public void DAL_ccNominaDeta_AddFromPreliminar(string centroPagoID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = centroPagoID;

                mySqlCommandSel.CommandText =
                    "Insert into ccNominaDeta( " +
                    "    NumDocCredito, FechaNomina, ValorNomina, ValorCuota, EstadoCruce, IndInconsistencia, InconsistenciaIncID, " +
                    "   CentroPagoID, FechaIncorpora, eg_ccNominaINC,eg_ccCentroPagoPAG " +
                    ") " +
                    "    SELECT NumDocCredito, FechaNomina, ValorNomina, ValorCuota, EstadoCruce, IndInconsistencia, InconsistenciaIncID, " +
                    "        CentroPagoID, FechaIncorpora, eg_ccNominaINC,eg_ccCentroPagoPAG " +
                    "    FROM ccNominaPreliminar with(nolock) " +
                    "    where CentroPagoID = @CentroPagoID";
                mySqlCommandSel.ExecuteNonQuery();

                mySqlCommandSel.CommandText = "delete from ccNominaPreliminar where CentroPagoID = @CentroPagoID";
                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaDeta_HasMigracionCliente");
                throw exception;
            }
        }

        #endregion
    }

}
