using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Threading;
using System.Configuration;
using System.Net;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL con las operaciones de negocio para el manejo de las tareas
    /// </summary>
    public class DAL_Alarmas : DAL_Base
    {
       
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_Alarmas(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Trae la informacion de un documetno que envia para aprobacion
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        public DTO_Alarma DAL_Alarma_GetDocumentMail(int numeroDoc)
        {
            try
            {
                DTO_Alarma alarma = new DTO_Alarma();
                alarma.NumeroDoc = numeroDoc.ToString();
 
                #region Carga la informacion de la alarma

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommand.CommandText =
                    "select docOrigen.DocumentoID, docOrigen.Descriptivo as DocumentoDesc, ter.TerceroID, ter.Descriptivo as TerceroDesc, " +
                    "	ctrl.PrefijoID, ctrl.DocumentoNro as Consecutivo " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "	inner join glDocumento docOrigen with(nolock) on docOrigen.DocumentoID = ctrl.DocumentoID " +
                    "	left join coTercero ter with(nolock) on ter.TerceroID = ctrl.TerceroID " +
                    "where NumeroDoc = @NumeroDoc";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    alarma.DocumentoID = dr["DocumentoID"].ToString();
                    alarma.DocumentoDesc = dr["DocumentoDesc"].ToString();
                    alarma.TerceroID = string.IsNullOrWhiteSpace(dr["TerceroID"].ToString()) ? string.Empty : dr["TerceroID"].ToString();
                    alarma.TerceroDesc = string.IsNullOrWhiteSpace(dr["TerceroDesc"].ToString()) ? string.Empty : dr["TerceroDesc"].ToString();
                    alarma.PrefijoID = dr["PrefijoID"].ToString();
                    alarma.Consecutivo = dr["Consecutivo"].ToString();
                }
                dr.Close();

                #endregion

                return alarma;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Alarma_GetDocumentMail");
                throw exception;
            }
        }

        /// <summary>
        /// Dice si un usuario tiene alarmas pendientes
        /// </summary>
        /// <returns>Devuelve verdadero si el usuario tiene alarmas</returns>
        public bool DAL_Alarmas_HasAlarms(string userName)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.DateTime);

                mySqlCommand.Parameters["@UserName"].Value = userName;
                mySqlCommand.Parameters["@Fecha"].Value = DateTime.Now;

                mySqlCommand.CommandText =
                    "select COUNT(*) " +
                    "from " +
                    "( " +
                    "	select ctrl.EmpresaID, ctrl.AreaFuncionalID, act.ActividadFlujoID, act.FechaAlarma1, act.FechaAlarma2, act.FechaAlarma3 " +
                    "	from glActividadEstado act with(nolock) " +
                    "		inner join glDocumentoControl ctrl with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "	where AlarmaInd = 1 " +
                    ")  as ActEstado " +
                    "	inner join glActividadPermiso Perm with(nolock) on Perm.EmpresaGrupoID = ActEstado.EmpresaID " +
                    "		and Perm.ActividadFlujoID = ActEstado.ActividadFlujoID and Perm.AreaFuncionalID = ActEstado.AreaFuncionalID " +
                    "where 	(Perm.AlarmaUsuario1=@UserName and ActEstado.FechaAlarma1 < @Fecha) or " +
                    "		(Perm.AlarmaUsuario2=@UserName and ActEstado.FechaAlarma2 < @Fecha) or " +
                    "		(Perm.AlarmaUsuario3=@UserName and ActEstado.FechaAlarma3 < @Fecha) ";

                int userTasks = (int)mySqlCommand.ExecuteScalar();
                return userTasks > 0 ? true : false;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Alarmas_HasAlarms");
                throw exception;
            }
        }

        /// <summary>
        /// Trae el listado de tareas pendientes para envio de correos
        /// </summary>
        /// <returns>Retorna el listado de tareas pendientes</returns>
        public IEnumerable<DTO_Alarma> DAL_Alarmas_GetAll(string userName = null)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommand.Parameters["@Fecha"].Value = DateTime.Now;

                mySqlCommand.CommandText =
                    "select ActEstado.EmpresaID, ActEstado.NumeroDoc, ActEstado.AreaFuncionalID, ActEstado.PrefijoID, ActEstado.Consecutivo, ActEstado.TerceroID, " +
                    "	ActEstado.TerceroDesc, ActEstado.ActividadFlujoID, ActEstado.Actividad, ActEstado.Procedimiento, " +
                    "	ActEstado.DocumentoID,ActEstado.DocumentoDesc, ActEstado.UsuarioRESPID, ActEstado.UsuarioRESP, " +
                    "	Perm.AlarmaUsuario1, ActEstado.FechaAlarma1, Perm.AlarmaUsuario2, ActEstado.FechaAlarma2, Perm.AlarmaUsuario3, ActEstado.FechaAlarma3 " +
                    "from " +
                    "( " +
                    "	select ctrl.EmpresaID, ctrl.NumeroDoc, ctrl.AreaFuncionalID, ctrl.PrefijoID, ctrl.DocumentoNro as Consecutivo, ter.TerceroID, " +
                    "		ter.Descriptivo as TerceroDesc, flujo.ActividadFlujoID, flujo.Descriptivo as Actividad, proced.Descriptivo as Procedimiento, " +
                    "		flujo.DocumentoID, doc.Descriptivo as DocumentoDesc, usr.ReplicaID as UsuarioRESPID, usr.UsuarioID as UsuarioRESP, " +
                    "		act.FechaAlarma1, act.FechaAlarma2, act.FechaAlarma3 " +
                    "	from glActividadEstado act with(nolock) " +
                    "		inner join seUsuario usr with(nolock) on act.seUsuarioID = usr.ReplicaID " +
                    "		inner join glDocumentoControl ctrl with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "		inner join glActividadFlujo flujo with(nolock) on act.ActividadFlujoID = flujo.ActividadFlujoID " +
                    "		inner join glDocumento doc with(nolock) on flujo.DocumentoID = doc.DocumentoID " +
                    "		inner join glProcedimiento proced with(nolock) on flujo.ProcedimientoID = proced.ProcedimientoID " +
                    "		left join coTercero ter with(nolock) on ter.TerceroID = ctrl.TerceroID " +
                    "	where AlarmaInd = 1 " +
                    ")  as ActEstado " +
                    "	inner join glActividadPermiso Perm with(nolock) on Perm.EmpresaGrupoID = ActEstado.EmpresaID " +
                    "		and Perm.ActividadFlujoID = ActEstado.ActividadFlujoID " +
                    "		and Perm.AreaFuncionalID = ActEstado.AreaFuncionalID ";

                if (userName != null)
                {
                    mySqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                    mySqlCommand.Parameters["@UserName"].Value = userName;
                  
                    mySqlCommand.CommandText +=
                        "where (Perm.AlarmaUsuario1=@UserName and ActEstado.FechaAlarma1 < @Fecha) " +
                        "	or (Perm.AlarmaUsuario2=@UserName and ActEstado.FechaAlarma2 < @Fecha) " +
                        "	or (Perm.AlarmaUsuario3=@UserName and ActEstado.FechaAlarma3 < @Fecha) ";
                }
                else
                {
                    mySqlCommand.CommandText += 
                        " where ActEstado.FechaAlarma1 < @Fecha or ActEstado.FechaAlarma2 < @Fecha or ActEstado.FechaAlarma3 < @Fecha";
                }

                DTO_Alarma tarea;
                List<DTO_Alarma> tareas = new List<DTO_Alarma>();

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    tarea = new DTO_Alarma(dr);
                    tareas.Add(tarea);
                }
                dr.Close();
               
                return tareas;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Alarmas_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Envia una alarma a un usuario
        /// </summary>
        /// <param name="numeroDoc">Pk del documento (glDocumentoControl)</param>
        /// <param name="usuario1">Usuario 1 al que se le envio la alarma</param>
        /// <param name="usuario2">Usuario 2 al que se le envio la alarma</param>
        /// <param name="usuario3">Usuario 3 al que se le envio la alarma</param>
        /// <returns></returns>
        public void DAL_Alarmas_SendAlarm(int numeroDoc, string actividadFlujoID, string usuario1, string usuario2, string usuario3)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioAlarma1", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioAlarma2", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioAlarma3", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);

                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;
                mySqlCommand.Parameters["@UsuarioAlarma1"].Value = usuario1;
                mySqlCommand.Parameters["@UsuarioAlarma2"].Value = usuario2;
                mySqlCommand.Parameters["@UsuarioAlarma3"].Value = usuario3;

                string setQuery = "UsuarioAlarma1=@UsuarioAlarma1,UsuarioAlarma2=@UsuarioAlarma2,UsuarioAlarma3=@UsuarioAlarma3";

                mySqlCommand.CommandText = "UPDATE glActividadEstado SET " + setQuery + " WHERE NumeroDoc = @NumeroDoc and ActividadFlujoID = @ActividadFlujoID";
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Alarmas_SendAlarm");
                throw exception;
            }
        }
        
        #endregion

    }
}
