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

        /// <summary>
        /// Agrega los registros de nominaDeta a nomina preliminar
        /// </summary>
        /// <param name="numeroDoc"></param>
        public void DAL_ccNominaDeta_UpdateNumDocRecibo(int numDocCredito, int numDocRecibo,string observacion)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocRCaja", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char);

                mySqlCommandSel.Parameters["@NumDocCredito"].Value = numDocCredito;
                mySqlCommandSel.Parameters["@NumDocRCaja"].Value = numDocRecibo;
                mySqlCommandSel.Parameters["@Observacion"].Value = observacion;

                mySqlCommandSel.CommandText = "Update ccNominaDeta set NumDocRCaja = @NumDocRCaja,Observacion = @Observacion Where NumDocCredito = @NumDocCredito and NumDocRCaja is null ";
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

        /// <summary>
        /// Agrega los registros de nominaDeta a nomina preliminar
        /// </summary>
        /// <param name="numeroDoc"></param>
        public void DAL_ccNominaDeta_AddFromPreliminar(string PagaduriaID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommandSel.Parameters["@PagaduriaID"].Value = PagaduriaID;

                mySqlCommandSel.CommandText =
                    "Insert into ccNominaDeta(NumDocCredito, FechaNomina, ValorNomina, ValorCuota, EstadoCruce,PagaduriaID, FechaIncorpora,eg_ccPagaduria ) " +
                    "    SELECT NumDocCredito, FechaNomina, ValorNomina, ValorCuota, EstadoCruce,PagaduriaID, FechaIncorpora,eg_ccPagaduria " +
                    "    FROM ccNominaPreliminar with(nolock) " +
                    "    where PagaduriaID = @PagaduriaID";
                mySqlCommandSel.ExecuteNonQuery();

                mySqlCommandSel.CommandText = "delete from ccNominaPreliminar where PagaduriaID = @PagaduriaID";
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

        /// <summary>
        /// Indica si existe una incorporacoion en un periodo
        /// </summary>
        /// <param name="centroPagoID">CIdentifidcador del centro de pago</param>
        /// <param name="fechaIni">Fecha inicial de consulta</param>
        /// <param name="fechaFin">Fecha final de consulta</param>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <returns>Verdadero si existe, de lo contrario falso</returns>
        public bool DAL_ccNominaDeta_ExistNomina(string pagaduria, DateTime fechaNom,int? numDocCredito)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                string where = string.Empty;

                if(!string.IsNullOrEmpty(pagaduria))
                {
                    mySqlCommandSel.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                    mySqlCommandSel.Parameters["@PagaduriaID"].Value = pagaduria;
                    mySqlCommandSel.Parameters.Add("@eg_ccPagaduria", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                    mySqlCommandSel.Parameters["@eg_ccPagaduria"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccPagaduria, this.Empresa, egCtrl);

                    where = " and nom.PagaduriaID = @PagaduriaID and nom.eg_ccPagaduria = @eg_ccPagaduria ";
                }
                if (fechaNom != null)
                {
                    mySqlCommandSel.Parameters.Add("@FechaNomina", SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters["@FechaNomina"].Value = fechaNom;
                    where += " and nom.FechaNomina = @FechaNomina ";
                }
                if (numDocCredito != null)
                {
                    mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                    mySqlCommandSel.Parameters["@NumDocCredito"].Value = numDocCredito;
                    where += " and nom.NumDocCredito = @NumDocCredito ";
                }

                mySqlCommandSel.CommandText =
                    "select COUNT(*) " +
                    "from ccNominaDeta nom with(nolock) " +
                    "	inner join glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = nom.NumDocRCaja  " +
                     "	inner join ccCreditoDocu cred with(nolock) on cred.NumeroDoc = nom.NumDocCredito  " +
                    "where ctrl.Estado = 3 and cred.EmpresaID = @EmpresaID  " + where ;

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
                    "where ctrl.Estado = 3 and nom.FechaNomina = @FechaNomina";

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
    }

}
