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
    public class DAL_pyServicioPrograma : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyServicioPrograma(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Obtiene detalle Proyectos
        /// </summary>
        /// <param name="periodo">periodo (opcional)</param>
        /// <returns></returns>
        public List<DTO_pyServicioPrograma> DAL_pyServicioPrograma_Get(int numeroDoc)
        {
            try
            {

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "    select  Distinct pyServicioPrograma.*,   " +
                                                 "    pyEtapa.Descriptivo as ActividadEtapaIDDesc,   " +
                                                 "    pyTarea.Descriptivo as TareaIDDesc,   " +
                                                 "    pyLineaFlujo.Descriptivo as LineaFlujoIDDesc,   " +
                                                 "    pyTrabajo.Descriptivo as TrabajoIDDesc,   " +
                                                 "    coCentroCosto.Descriptivo as CentroCostoIDDesc   " +
                                                 "    from pyServicioPrograma   " +
                                                 "    inner join glDocumentoControl on glDocumentoControl.NumeroDoc = pyServicioPrograma.NumeroDoc    " +
                                                 "    inner join pyEtapa on pyEtapa.ActividadEtapaID = pyServicioPrograma.ActividadEtapaID    " +
                                                 "    inner join pyTarea on pyTarea.TareaID = pyServicioPrograma.TareaID    " +
                                                 "    inner join pyLineaFlujo on pyLineaFlujo.LineaFlujoID = pyServicioPrograma.LineaFlujoID    " +
                                                 "    inner join pyTrabajo on pyTrabajo.TrabajoID = pyServicioPrograma.TrabajoID    " +
                                                 "    inner join coCentroCosto on coCentroCosto.CentroCostoID = pyServicioPrograma.CentroCostoID    " +
                                              "    WHERE glDocumentoControl.EmpresaID = @EmpresaID AND  glDocumentoControl.NumeroDoc = @NumeroDoc ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;


                List<DTO_pyServicioPrograma> result = new List<DTO_pyServicioPrograma>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_pyServicioPrograma dto = new DTO_pyServicioPrograma(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyServicioPrograma_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Ingresa el detalle de la solicitud de orden
        /// </summary>
        /// <param name="numeroDocSoli">numero doc Solicitud</param>
        /// <param name="numeroDocOrden">numero doc Orden</param>
        public void DAL_pyServicioPrograma_GenerarOrden(int numeroDocSoli, int numeroDocOrden)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Proyectos_CargaOrdenServicio", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDocOrden", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDocSoli", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@UsuarioResponable", SqlDbType.Int);                

                mySqlCommandSel.Parameters.Add("@eg_pyClaseServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyLineaFlujo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyEtapa", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyTrabajo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyTarea", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyRecurso", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_inUnidad", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_inEmpaque", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDocSoli"].Value = numeroDocSoli;
                mySqlCommandSel.Parameters["@NumeroDocOrden"].Value = numeroDocOrden;
                mySqlCommandSel.Parameters["@UsuarioResponable"].Value = this.UserId;

                mySqlCommandSel.Parameters["@eg_pyClaseServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyClaseServicio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_pyLineaFlujo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyLineaFlujo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_pyEtapa"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyEtapa, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_pyTrabajo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyTrabajo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_pyTarea"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyTarea, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_pyRecurso"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyRecurso, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_inUnidad"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inUnidad, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_inEmpaque"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inEmpaque, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyServicioPrograma_Get");
                throw exception;
            }

        }
    }
}
