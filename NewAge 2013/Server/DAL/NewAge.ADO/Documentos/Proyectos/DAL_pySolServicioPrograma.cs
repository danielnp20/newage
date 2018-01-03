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
    public class DAL_pySolServicioPrograma : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pySolServicioPrograma(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
  
        /// <summary>
        /// Trae programacion de servicio
        /// </summary>
        /// <param name="numeroDoc">numero de documento</param>
        /// <param name="claseServicio">clase de servicio</param>
        /// <returns>lista de programacion</returns>
        public List<DTO_pySolServicioPrograma> DAL_pySolServicioPrograma_Get(int numeroDoc, string claseServicio)
        {        
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Proyectos_CargarSolServicioPrograma", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ClaseServicioID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@ClaseServicioID"].Value = claseServicio;

                IDataReader dr = mySqlCommandSel.ExecuteReader();

                List<DTO_pySolServicioPrograma> programacion = new List<DTO_pySolServicioPrograma>();
                DTO_pySolServicioPrograma prog = null;
                while (dr.Read())
                {
                    prog = new DTO_pySolServicioPrograma(dr);
                    programacion.Add(prog);
                }
                dr.Close();

                return programacion;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pySolServicioPrograma_Get");
                throw exception;
            }           
        }

        /// <summary>
        /// Trae listado de Aprobación
        /// </summary>
        /// <param name="actividadFlujoID">actividad de aprobación</param>
        /// <param name="claseServicioID">clase de servicio</param>
        /// <returns></returns>
        public List<DTO_pySolServicioPrograma> DAL_pySolServicioPrograma_GetAprob(string claseServicioID, string actividadFlujoID)
        {
            try
            {

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  SELECT pySolServicioPrograma.NumeroDoc,    " +
		                                        "  pySolServicioPrograma.ClaseServicioID,    " +
		                                        "  pySolServicioPrograma.LineaFlujoID,    " +
		                                        "  pyLineaFlujo.Descriptivo LineaFlujoIDDesc,    " +
		                                        "  pySolServicioPrograma.ActividadEtapaID,    " +
		                                        "  pyEtapa.Descriptivo ActividadEtapaIDDesc,    " +
		                                        "  pySolServicioPrograma.TareaID,    " +
		                                        "  pyTarea.Descriptivo TareaIDDesc,    " +
		                                        "  pySolServicioPrograma.TrabajoID,    " +
		                                        "  pyTrabajo.Descriptivo TrabajoIDDesc,    " +
		                                        "  pySolServicioPrograma.SemanaPrograma,    " +
		                                        "  pySolServicioPrograma.SemanaProgramaFin,    " +
		                                        "  pySolServicioPrograma.CantidadTR,    " +
		                                        "  pySolServicioPrograma.Observaciones,    " +
		                                        "  plLineaPresupuesto.LineaPresupuestoID,    " +
		                                        "  plLineaPresupuesto.Descriptivo as LineaPresupuestoIDDesc    " +
		                                        "  FROM pySolServicioPrograma      " +
		                                        "  INNER JOIN pyTarea ON pyTarea.TareaID = pySolServicioPrograma.TareaID    " +
	   	                                        "  INNER JOIN pyTrabajo ON pyTrabajo.TrabajoID = pySolServicioPrograma.TrabajoID    " +				  
		                                        "  INNER JOIN pyEtapa ON pyEtapa.ActividadEtapaID = pySolServicioPrograma.ActividadEtapaID    " +				   
		                                        "  INNER JOIN pyLineaFlujo ON pyLineaFlujo.LineaFlujoID = pySolServicioPrograma.LineaFlujoID	    " +			    		   
    	                                        "  INNER JOIN plLineaPresupuesto ON plLineaPresupuesto.LineaPresupuestoID = pyTrabajo.LineaPresupuestoID    " +	
    	                                        "  INNER JOIN glActividadEstado ON pySolServicioPrograma.NumeroDoc = glActividadEstado.NumeroDoc    " +
                                                "  WHERE glActividadEstado.ActividadFlujoID = @ActividadFlujoID ";


                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;


                List<DTO_pySolServicioPrograma> result = new List<DTO_pySolServicioPrograma>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_pySolServicioPrograma dto = new DTO_pySolServicioPrograma(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pySolServicioPrograma_GetAprob");
                throw exception;
            }           
        }
              
        /// <summary>
        /// Ingresa ingformación en la programación 
        /// </summary>
        /// <param name="programa">programación</param>
        /// <returns>retorna idenfiticador</returns>
        public int DAL_pySolServicioDeta_Add(DTO_pySolServicioPrograma programa)
        {
            try
            {

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  INSERT INTO pySolServicioPrograma  " +
                                                    "  ([NumeroDoc]  " +
                                                    "  ,[LineaFlujoID]  " +
                                                    "  ,[ActividadEtapaID]  " +
                                                    "  ,[TareaID]  " +
                                                    "  ,[TrabajoID]  " +
                                                    "  ,[CantidadTR]  " +
                                                    "  ,[SemanaPrograma]  " +
                                                    "  ,[SemanaProgramaFin]  " +
                                                    "  ,[Observaciones]  " +
                                                    "  ,[eg_pyClaseServicio]  " +
                                                    "  ,[eg_pyLineaFlujo]  " +
                                                    "  ,[eg_pyEtapa]  " +
                                                    "  ,[eg_pyTarea]  " +
                                                    "  ,[eg_pyTrabajo]  " +
                                                    "  ,[ClaseServicioID])  " +
                                              "  VALUES  " +
                                                    "  (@NumeroDoc  " +
                                                    "  ,@LineaFlujoID  " +
                                                    "  ,@ActividadEtapaID  " +
                                                    "  ,@TareaID  " +
                                                    "  ,@TrabajoID  " +
                                                    "  ,@CantidadTR  " +
                                                    "  ,@SemanaPrograma  " +
                                                    "  ,@SemanaProgramaFin  " +
                                                    "  ,@Observaciones  " +
                                                    "  ,@eg_pyClaseServicio  " +
                                                    "  ,@eg_pyLineaFlujo  " +
                                                    "  ,@eg_pyEtapa  " +
                                                    "  ,@eg_pyTarea  " +
                                                    "  ,@eg_pyTrabajo  " +
                                                    "  ,@ClaseServicioID)   " +
                                                    "   SET @Consecutivo = SCOPE_IDENTITY()";


                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LineaFlujoID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@ActividadEtapaID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@TareaID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TrabajoID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@CantidadTR", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@SemanaPrograma", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@SemanaProgramaFin", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ClaseServicioID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTBase.MaxLength);

                mySqlCommandSel.Parameters.Add("@eg_pyClaseServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyLineaFlujo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyEtapa", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyTarea", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyTrabajo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = programa.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@LineaFlujoID"].Value = programa.LineaFlujoID.Value;
                mySqlCommandSel.Parameters["@ActividadEtapaID"].Value = programa.ActividadEtapaID.Value;
                mySqlCommandSel.Parameters["@TareaID"].Value = programa.TareaID.Value;
                mySqlCommandSel.Parameters["@TrabajoID"].Value = programa.TrabajoID.Value;
                mySqlCommandSel.Parameters["@CantidadTR"].Value = programa.CantidadTR.Value;
                mySqlCommandSel.Parameters["@SemanaPrograma"].Value = programa.SemanaPrograma.Value;
                mySqlCommandSel.Parameters["@SemanaProgramaFin"].Value = programa.SemanaProgramaFin.Value;
                mySqlCommandSel.Parameters["@ClaseServicioID"].Value = programa.ClaseServicioID.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = programa.Observaciones.Value;

                mySqlCommandSel.Parameters["@eg_pyClaseServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyClaseServicio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_pyLineaFlujo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyLineaFlujo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_pyEtapa"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyEtapa, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_pyTarea"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyTarea, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_pyTrabajo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyTrabajo, this.Empresa, egCtrl);

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
                int consecutivo = Convert.ToInt32(mySqlCommandSel.Parameters["@Consecutivo"].Value);
                return consecutivo;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyServicioDeta_AddDetalle");
                throw exception;
            }
        }

        #region Aprobaciones

        /// <summary>
        /// Trae los documentos para aprobación pendientes 
        /// </summary>
        /// <param name="documentoID">documentoID</param>
        /// <param name="actividadFLujoId">actividadFlujoID</param>
        /// <returns></returns>
        public List<DTO_pySolServicioAprob> DAL_pySolServicioPrograma_GetSolicitudesAprob(int documentoID, string actividadFLujoId)
        {
            try
            {

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "      SELECT    glDocumentoControl.NumeroDoc,   glDocumentoControl.PrefijoID,  glDocumentoControl.PeriodoDoc,  " +
		                                        "              glDocumentoControl.DocumentoNro,   " +
                                                "              glDocumentoControl.Observacion,	   " +
                                                "              glDocumento.Descriptivo as DocumentoDesc   " +
                                                "     FROM glDocumentoControl   " +
                                                "     INNER JOIN glActividadEstado ON glDocumentoControl.NumeroDoc = glActividadEstado.NumeroDoc   " +
                                                "     INNER JOIN glDocumento ON glDocumento.DocumentoID = glDocumentoControl.DocumentoID   " +
                                                "     WHERE glDocumentoControl.EmpresaID = @EmpresaID   " +
                                                "     AND glDocumentoControl.DocumentoID = @DocumentoID   " +
                                                "     AND glDocumentoControl.Estado = 2   " +
                                                "     AND glActividadEstado.ActividadFlujoID = @ActividadFlujoID ";


                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = documentoID;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actividadFLujoId;


                List<DTO_pySolServicioAprob> result = new List<DTO_pySolServicioAprob>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_pySolServicioAprob dto = new DTO_pySolServicioAprob(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pySolServicioPrograma_GetSolicitudesAprob");
                throw exception;
            }

        }

        #endregion

    }
}
