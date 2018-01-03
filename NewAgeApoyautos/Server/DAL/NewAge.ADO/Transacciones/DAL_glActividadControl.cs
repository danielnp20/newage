using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.UDT;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    public class DAL_glActividadControl : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glActividadControl(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Dal para Consulta de registros en la tabla glActividadControl
        /// </summary>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <returns>listado de registros de la tabla glActividadControl por numero de documento</returns>
        public IEnumerable<DTO_glActividadControl> DAL_glActividadControl_Get(int numeroDoc)
        {
            try
            {
                List<DTO_glActividadControl> bitacora = new List<DTO_glActividadControl>();
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select ctrl.*, act.Descriptivo, seUsuario.UsuarioID as UsuarioDesc " +
                    "from glActividadControl ctrl with(nolock) " +
                    "   inner join glActividadFlujo act with(nolock) on ctrl.ActividadFlujoID = act.ActividadFlujoID " +
                    "   inner join seUsuario with(nolock) on seUsuario.ReplicaID = ctrl.UsuarioID " +
                    "where ctrl.NumeroDoc = @NumeroDoc";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                SqlDataReader drSel;
                drSel = mySqlCommand.ExecuteReader();

                while (drSel.Read())
                {
                    DTO_glActividadControl btAct = new DTO_glActividadControl(drSel);
                    bitacora.Add(btAct);
                }

                drSel.Close();
                return bitacora;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glActividadControl_Get");
                throw exception;  
            }
        }

        /// <summary>
        /// Inserta un registro en la bitacora de tareas (glActividadControl)
        /// </summary>
        public void AgregarTareasControl(DTO_glActividadControl tareaCtrl)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "INSERT INTO glActividadControl (" +
                  "  EmpresaID, DocumentoID, NumeroDoc, ActividadFlujoID, UsuarioID, Fecha, Periodo, Observacion, AlarmaInd, CompAnula, CompNroAnula" +
                  ") VALUES (" +
                  "  @EmpresaID, @DocumentoID, @NumeroDoc, @ActividadFlujoID, @UsuarioID, @Fecha, @Periodo, @Observacion, @AlarmaInd, @CompAnula, @CompNroAnula" +
                  ")";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.VarChar, UDT_DescripUnFormat.MaxLength);
                mySqlCommand.Parameters.Add("@AlarmaInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CompAnula", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@CompNroAnula", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = tareaCtrl.DocumentoID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = tareaCtrl.NumeroDoc.Value;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = tareaCtrl.ActividadFlujoID.Value;              
                mySqlCommand.Parameters["@UsuarioID"].Value = this.UserId;
                mySqlCommand.Parameters["@Fecha"].Value = DateTime.Now;
                mySqlCommand.Parameters["@Periodo"].Value = tareaCtrl.Periodo.Value;
                mySqlCommand.Parameters["@Observacion"].Value = tareaCtrl.Observacion.Value;
                mySqlCommand.Parameters["@AlarmaInd"].Value = tareaCtrl.AlarmaInd.Value;

                //Comprobante
                if (!string.IsNullOrWhiteSpace(tareaCtrl.CompAnula.Value))
                {
                    mySqlCommand.Parameters["@CompAnula"].Value = tareaCtrl.CompAnula.Value;
                    mySqlCommand.Parameters["@CompNroAnula"].Value = tareaCtrl.CompNroAnula.Value;
                }
                else
                {
                    mySqlCommand.Parameters["@CompAnula"].Value = DBNull.Value;
                    mySqlCommand.Parameters["@CompNroAnula"].Value = DBNull.Value;
                }

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glActividadControl_AgregarTareasControl");
                throw exception;  
            }
        }
        
        #endregion
    }
}
