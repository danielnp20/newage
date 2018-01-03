using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    public class DAL_glActividadEstado : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glActividadEstado(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Agrega un nuevo registro
        /// </summary>
        /// <param name="act">Registro</param>
        public void DAL_glActividadEstado_Add(DTO_glActividadEstado act)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "INSERT INTO glActividadEstado(NumeroDoc, ActividadFlujoID, seUsuarioID, TerceroID, FechaInicio, FechaFin, FechaCerrado, FechaAlarma1, UsuarioAlarma1, " +
                    "   FechaAlarma2, UsuarioAlarma2, FechaAlarma3, UsuarioAlarma3, UsuarioDelegado, Observaciones, ActividadBase,Valor, CerradoInd, AlarmaInd, eg_glActividadFlujo) " +
                    "VALUES(@NumeroDoc, @ActividadFlujoID, @seUsuarioID, @TerceroID, @FechaInicio, @FechaFin, @FechaCerrado, @FechaAlarma1, @UsuarioAlarma1, " +
                    "   @FechaAlarma2, @UsuarioAlarma2, @FechaAlarma3, @UsuarioAlarma3, @UsuarioDelegado, @Observaciones, @ActividadBase, @Valor,@CerradoInd, @AlarmaInd, @eg_glActividadFlujo)";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@seUsuarioID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength); 
                mySqlCommand.Parameters.Add("@FechaInicio", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaCerrado", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAlarma1", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@UsuarioAlarma1", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaAlarma2", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@UsuarioAlarma2", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaAlarma3", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@UsuarioAlarma3", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioDelegado", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@ActividadBase", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@AlarmaInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_glActividadFlujo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = act.NumeroDoc.Value;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = act.ActividadFlujoID.Value;
                mySqlCommand.Parameters["@seUsuarioID"].Value = act.seUsuarioID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = act.TerceroID.Value; 
                mySqlCommand.Parameters["@FechaInicio"].Value = act.FechaInicio.Value;
                mySqlCommand.Parameters["@FechaFin"].Value = act.FechaFin.Value;
                mySqlCommand.Parameters["@FechaCerrado"].Value = act.FechaCerrado.Value;
                mySqlCommand.Parameters["@FechaAlarma1"].Value = act.FechaAlarma1.Value;
                mySqlCommand.Parameters["@UsuarioAlarma1"].Value = act.UsuarioAlarma1.Value;
                mySqlCommand.Parameters["@FechaAlarma2"].Value = act.FechaAlarma2.Value;
                mySqlCommand.Parameters["@UsuarioAlarma2"].Value = act.UsuarioAlarma2.Value;
                mySqlCommand.Parameters["@FechaAlarma3"].Value = act.FechaAlarma3.Value;
                mySqlCommand.Parameters["@UsuarioAlarma3"].Value = act.UsuarioAlarma3.Value;
                mySqlCommand.Parameters["@UsuarioDelegado"].Value = act.UsuarioDelegado.Value;
                mySqlCommand.Parameters["@Observaciones"].Value = act.Observaciones.Value;
                mySqlCommand.Parameters["@ActividadBase"].Value = act.ActividadBase.Value;
                mySqlCommand.Parameters["@CerradoInd"].Value = act.CerradoInd.Value;
                mySqlCommand.Parameters["@AlarmaInd"].Value = act.AlarmaInd.Value;
                mySqlCommand.Parameters["@Valor"].Value = act.Valor.Value;
                mySqlCommand.Parameters["@eg_glActividadFlujo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glActividadFlujo, this.Empresa, egCtrl);

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
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glActividadEstado_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un nuevo registro
        /// </summary>
        /// <param name="act">Registro</param>
        public void DAL_glActividadEstado_Upd(DTO_glActividadEstado act)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "UPDATE glActividadEstado " +
                    "   Set FechaInicio = @FechaInicio, " +
                    "       FechaFin = @FechaFin, " +
                    "       Observaciones = @Observaciones, " +
                    "       CerradoInd = @CerradoInd,  " +
                    "       FechaAlarma1 = @FechaAlarma1,  " +
                    "       UsuarioAlarma1 = @UsuarioAlarma1,  " +
                    "       AlarmaInd = @AlarmaInd,  " +
                    "       Valor = @Valor  " +
                    " where NumeroDoc = @NumeroDoc and ActividadFlujoID = @ActividadFlujoID";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);  
                mySqlCommand.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength); 
                mySqlCommand.Parameters.Add("@FechaInicio", SqlDbType.DateTime);  
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAlarma1", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@UsuarioAlarma1", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@AlarmaInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = act.NumeroDoc.Value;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = act.ActividadFlujoID.Value;
                mySqlCommand.Parameters["@FechaInicio"].Value = act.FechaInicio.Value;
                mySqlCommand.Parameters["@Observaciones"].Value = act.Observaciones.Value;
                mySqlCommand.Parameters["@CerradoInd"].Value = act.CerradoInd.Value;
                mySqlCommand.Parameters["@FechaFin"].Value = act.FechaFin.Value;
                mySqlCommand.Parameters["@FechaAlarma1"].Value = act.FechaAlarma1.Value;
                mySqlCommand.Parameters["@UsuarioAlarma1"].Value = act.UsuarioAlarma1.Value;
                mySqlCommand.Parameters["@AlarmaInd"].Value = act.AlarmaInd.Value.HasValue? act.AlarmaInd.Value : false;
                mySqlCommand.Parameters["@Valor"].Value = act.Valor.Value;
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
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glActividadEstado_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// deshabilita las alarmas de una tarea
        /// </summary>
        /// <param name="numeroDoc">Identificador unico del documento</param>
        /// <param name="actividadFlujoID">Identificador de la actividad (Si se deja vacio deshabilita todas las alarmas de un documento)</param>
        public void DAL_glActividadEstado_UpdateAlarmStatus(int numeroDoc, string actividadFlujoID, bool enable, DTO_glActividadEstado act)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@AlarmaInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                string where = " Where NumeroDoc = @NumeroDoc ";
                string set = " Set CerradoInd=@CerradoInd,AlarmaInd=@AlarmaInd,FechaFin=@FechaFin ";
                if (!string.IsNullOrWhiteSpace(actividadFlujoID))
                {
                    mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                    mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;

                    where += " and ActividadFlujoID = @ActividadFlujoID";
                }

                if (enable)
                {
                    mySqlCommandSel.Parameters.Add("@FechaAlarma1", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@UsuarioAlarma1", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@FechaAlarma2", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@UsuarioAlarma2", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@FechaAlarma3", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@UsuarioAlarma3", SqlDbType.Char, UDT_UsuarioID.MaxLength);

                    mySqlCommandSel.Parameters["@UsuarioAlarma1"].Value = DBNull.Value;
                    mySqlCommandSel.Parameters["@UsuarioAlarma2"].Value = DBNull.Value;
                    mySqlCommandSel.Parameters["@UsuarioAlarma3"].Value = DBNull.Value;
                    mySqlCommandSel.Parameters["@AlarmaInd"].Value = true;
                    mySqlCommandSel.Parameters["@CerradoInd"].Value = false;
                    mySqlCommandSel.Parameters["@FechaFin"].Value = DBNull.Value;

                    //Alarma1
                    if (act.FechaAlarma1.Value != null)
                        mySqlCommandSel.Parameters["@FechaAlarma1"].Value = act.FechaAlarma1.Value.Value;
                    else
                        mySqlCommandSel.Parameters["@FechaAlarma1"].Value = DBNull.Value;
                    //Alarma2 
                    if (act.FechaAlarma2.Value != null)
                        mySqlCommandSel.Parameters["@FechaAlarma2"].Value = act.FechaAlarma2.Value.Value;
                    else
                        mySqlCommandSel.Parameters["@FechaAlarma2"].Value = DBNull.Value;
                    //Alarma3
                    if (act.FechaAlarma3.Value != null)
                        mySqlCommandSel.Parameters["@FechaAlarma3"].Value = act.FechaAlarma3.Value.Value;
                    else
                        mySqlCommandSel.Parameters["@FechaAlarma3"].Value = DBNull.Value;

                    set += ",UsuarioAlarma1=@UsuarioAlarma1,UsuarioAlarma2=@UsuarioAlarma2,UsuarioAlarma3=@UsuarioAlarma3," +
                        "FechaAlarma1=@FechaAlarma1,FechaAlarma2=@FechaAlarma2,FechaAlarma3=@FechaAlarma3 ";
                }
                else
                {
                    mySqlCommandSel.Parameters["@AlarmaInd"].Value = false;
                    mySqlCommandSel.Parameters["@CerradoInd"].Value = true;
                    mySqlCommandSel.Parameters["@FechaFin"].Value = DateTime.Now;
                }

                mySqlCommandSel.CommandText = "Update glActividadEstado " + set + where;
                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glActividadEstado_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Revisa si 
        /// </summary>
        /// <param name="numeroDoc"></param>
        /// <returns></returns>
        public bool DAL_glActividadEstado_TieneActividadesPendientes(int numeroDoc, string actividadFlujoID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);                
                mySqlCommandSel.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;
                mySqlCommandSel.Parameters["@CerradoInd"].Value = false;

                mySqlCommandSel.CommandText =
                    "Select count(*) from glActividadEstado with(nolock) " +
                    "where NumeroDoc = @NumeroDoc and CerradoInd = @CerradoInd and ActividadFlujoID <> @ActividadFlujoID";

                int count = Convert.ToInt32(mySqlCommandSel.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glActividadEstado_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Revisa si 
        /// </summary>
        /// <param name="numeroDoc"></param>
        /// <returns></returns>
        public bool DAL_glActividadEstado_HasAlarm(int numeroDoc, string actividadFlujoID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;

                mySqlCommandSel.CommandText =
                    "Select count(*) from glActividadEstado with(nolock) " +
                    "where NumeroDoc = @NumeroDoc and ActividadFlujoID = @ActividadFlujoID";

                int count = Convert.ToInt32(mySqlCommandSel.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glActividadEstado_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene lista de actividades con llamada pendientes
        /// </summary>
        /// <param name="documentoID">documento filtro</param>
        /// <param name="actFlujoID">actividad o tarea filtro</param>
        /// <param name="fechaIni">fecha inicial de consulta</param>
        /// <param name="fechaFin">fecha final de consulta</param>
        /// <param name="terceroID">tercero filtro</param>
        /// <param name="prefijoID">prefijo filtro</param>
        /// <param name="docNro">nro de documento filtro</param>
        /// <returns></returns>
        public List<DTO_InfoTarea> DAL_glActividadEstado_GetPendientesByParameter(int? numeroDoc, int? documentoID, string actFlujoID, DateTime? fechaIni, 
            DateTime? fechaFin, string terceroID, string prefijoID, int? docNro, EstadoTareaIncumplimiento estadoTarea)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                List<DTO_InfoTarea> result = new List<DTO_InfoTarea>();

                string where = string.Empty;
                if (estadoTarea != EstadoTareaIncumplimiento.Todas)
                {
                    where += " and actEst.CerradoInd = @CerradoInd ";
                    mySqlCommandSel.Parameters.Add("@CerradoInd", SqlDbType.Bit);
                    mySqlCommandSel.Parameters["@CerradoInd"].Value = estadoTarea == EstadoTareaIncumplimiento.Cerradas ? true : false;
                }
                if (numeroDoc != null)
                {
                    where += " and actEst.NumeroDoc = @NumeroDoc ";
                    mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc.Value;
                }
                if (documentoID != null)
                {
                    where += "and doc.DocumentoID = @DocumentoID ";
                    mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                    mySqlCommandSel.Parameters["@DocumentoID"].Value = documentoID.Value;
                }
                if (!string.IsNullOrEmpty(actFlujoID))
                {
                    where += "and actFlujo.ActividadFlujoID = @ActividadFlujoID ";
                    mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                    mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                }
                if (fechaIni != null && fechaFin != null)
                {
                    where += "and FechaInicio between @FechaInicio and @FechaFin ";
                    mySqlCommandSel.Parameters.Add("@FechaInicio", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters["@FechaInicio"].Value = fechaIni;
                    mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;
                }

                if (!string.IsNullOrEmpty(terceroID))
                {
                    where += "and doc.TerceroID = @TerceroID ";
                    mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@TerceroID"].Value = terceroID;
                }
                if (!string.IsNullOrEmpty(prefijoID))
                {
                    where += "and doc.PrefijoID = @PrefijoID ";
                    mySqlCommandSel.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                    mySqlCommandSel.Parameters["@PrefijoID"].Value = prefijoID;
                }
                if (docNro != null)
                {
                    where = "and doc.DocumentoNro = @DocumentoNro ";
                    mySqlCommandSel.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                    mySqlCommandSel.Parameters["@DocumentoNro"].Value = docNro;
                }
                mySqlCommandSel.CommandText =
                " Select actEst.NumeroDoc,actEst.ActividadFlujoID,actFlujo.LlamadaID,actFlujo.Descriptivo as ActividadFlujoDesc, actEst.TerceroID as TerceroIDActEstado," +
                "        actEst.FechaInicio, actEst.FechaFin, actEst.FechaCerrado,  actEst.FechaAlarma1, actEst.Observaciones,actEst.Valor,actEst.CerradoInd," +
                "        doc.DocumentoID, doc.DocumentoTipo,doc.TerceroID, tercero.Descriptivo as TerceroDesc,doc.PrefijoID, doc.DocumentoNro,doc.DocumentoTercero, actFlujo.UnidadTiempo,usr.UsuarioID " +
                " From  glActividadEstado actEst with(nolock) " +
                "       inner join glActividadFlujo actFlujo on actFlujo.ActividadFlujoID = actEst.ActividadFlujoID  " +
                "       inner join glDocumentoControl doc on doc.NumeroDoc = actEst.NumeroDoc  " +
                "       left join coTercero tercero on tercero.TerceroID = doc.TerceroID and tercero.EmpresaGrupoID = doc.eg_coTercero " +
                "       inner join seUsuario usr on usr.ReplicaID = actEst.seUsuarioID  " +
                " Where actFlujo.TipoActividad = 1 " + where; 

                SqlDataReader dr;
                dr = mySqlCommandSel.ExecuteReader();

                while (dr.Read())
                {
                    DTO_InfoTarea tarea = new DTO_InfoTarea(dr);
                    result.Add(tarea);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glActividadEstado_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene la lista de los NumeroDoc del formulario Solicictudes
        /// </summary>
        /// <param name="NumeroDoc"></param>
        /// <returns></returns>
        public string DAL_glActividadEstado_GetStatedbyNumeroDoc(string _numeroLibranza)
        {
            try
            {
                string result = string.Empty;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText

                mySqlCommand.CommandText =

                "    SELECT TOP(1) CASE WHEN ac.Estado <> 0 Then ( CASE WHEN ac.CerradoInd = 0 THEN FLJ.Descriptivo ELSE 'Liquidado' END) Else 'Rechazado' END AS EstadoSol FROM   " +
                "   (  " +
                "   	SELECT * FROM  " +
                "   	(  " +
                "   		SELECT TOP(1) est.*,CTL.Estado  " +
                "   		FROM ccSolicitudDocu	sol WITH(NOLOCK)  " +
                "   			INNER JOIN glDocumentoControl	CTL WITH(NOLOCK) ON SOL.NumeroDoc = CTL.NumeroDoc   " +
                "   			INNER JOIN glActividadEstado	EST WITH(NOLOCK) ON CTL.NumeroDoc = EST.NumeroDoc  " +
                "   		WHERE sol.numeroDoc = @NumeroDoc AND CerradoInd = 0  " +
                "   	) AS q1  " +
                "   	UNION ALL  " +
                "   	SELECT * FROM  " +
                "   	(  " +
                "   		SELECT TOP(1) est.*,CTL.Estado  " +
                "   		FROM ccSolicitudDocu	sol WITH(NOLOCK)  " +
                "   			INNER JOIN glDocumentoControl	CTL WITH(NOLOCK) ON SOL.NumeroDoc = CTL.NumeroDoc   " +
                "   			INNER JOIN glActividadEstado	EST WITH(NOLOCK) ON CTL.NumeroDoc = EST.NumeroDoc  " +
                "   		WHERE sol.numeroDoc = @NumeroDoc AND CerradoInd = 1  " +
                "   		ORDER BY est.consecutivo desc   " +
                "   	) AS q2  " +
                "   ) AS ac  " +
                "   	INNER JOIN glActividadFlujo	FLJ WITH(NOLOCK) ON ac.ActividadFlujoID = FLJ.ActividadFlujoID AND ac.eg_glActividadFlujo = FLJ.EmpresaGrupoID  ";

                #endregion

                #region Parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Char);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = _numeroLibranza; 
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    result = Convert.ToString(dr["EstadoSol"]);
                }

                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glSolicitudes_GetStatedbyNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene la actividad actual del documento solicitado y 
        /// </summary>
        /// <param name="NumeroDoc"></param>
        /// <param name="onlyAbiertas"></param>
        /// <returns>ActividadFlujo</returns>
        public string DAL_glActividadEstado_GetActFlujoByNumeroDoc(int numeroDoc, bool onlyAbiertas = false)
        {
            try
            {
                string result = string.Empty;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string filtro = string.Empty;
                if (onlyAbiertas)
                    filtro = " and CerradoInd = 0 ";

                #region CommandText

                mySqlCommand.CommandText =
                "  Select top(1) * from  glActividadEstado  with(nolock) Where NumeroDoc = @NumeroDoc " + filtro +
                "   order by FechaInicio desc ";
                #endregion

                #region Parametros

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);                
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    result = Convert.ToString(dr["ActividadFlujoID"]);
                }

                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glSolicitudes_GetStatedbyNumeroDoc");
                throw exception;
            }
        }

    }
}
