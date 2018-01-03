using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data.SqlTypes;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL_pyProyectoModificaFechas
    /// </summary>
    public class DAL_pyProyectoModificaFechas : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyProyectoModificaFechas(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Consulta un Orden de Compra segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>

        public DTO_pyProyectoModificaFechas DAL_pyProyectoModificaFechas_Load(int NumeroDoc)
        {
            try
            {
                DTO_pyProyectoModificaFechas result = new DTO_pyProyectoModificaFechas();
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from pyProyectoModificaFechas with(nolock) where NumeroDoc = @NumeroDoc ";//and consecutivo=@Consecutivo

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
  //              mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
//                mySqlCommand.Parameters["@Consecutivo"].Value = Consecutivo;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
               if (dr.Read())
                {
                    result = new DTO_pyProyectoModificaFechas(dr);
                }
                dr.Close();
                return result;
            
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoModificaFechas_Get");
                throw exception;
            }
        }

        public List<DTO_pyProyectoModificaFechas> DAL_pyProyectoModificaFechas_GetByNumeroDoc(int NumeroDoc, string Tareas)
        {
            try
            {
                List<DTO_pyProyectoModificaFechas> results = new List<DTO_pyProyectoModificaFechas>();
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from pyProyectoModificaFechas with(nolock) where NumeroDoc = @NumeroDoc and TareaID=@Tareas order by consecutivo desc";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                mySqlCommand.Parameters.Add("@Tareas", SqlDbType.Char);
                mySqlCommand.Parameters["@Tareas"].Value = Tareas;


                SqlDataReader dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_pyProyectoModificaFechas tarea = new DTO_pyProyectoModificaFechas(dr);
                    results.Add(tarea);
                }
                dr.Close(); 
                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoModificaFechas_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla pyProyectoModificaFechas 
        /// </summary>
        /// <param name="sol">Orden de Compra</param>
        /// <returns></returns>
        public void DAL_pyProyectoModificaFechas_Add(DTO_pyProyectoModificaFechas datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO pyProyectoModificaFechas " +
                                           "    (NumeroDoc,TareaID,ProyectoID,TipoAjuste,Codigo,FechaActual,FechaNueva,UsuarioDigita," +
                                           "    FechaDigita,UsuarioAprueba,FechaAprueba,ApruebaInd,Observaciones,eg_pyTarea," +
                                           "    eg_coProyecto,eg_pyJustificaModificacion)" +
                                           "    VALUES" +
                                           "    (@NumeroDoc,@TareaID,@ProyectoID,@TipoAjuste,@Codigo,@FechaActual,@FechaNueva,@UsuarioDigita," +
                                           "    @FechaDigita,@UsuarioAprueba,@FechaAprueba,@ApruebaInd,@Observaciones,@eg_pyTarea," +
                                           "    @eg_coProyecto,@eg_pyJustificaModificacion)";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TareaID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char,UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@TipoAjuste", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Codigo", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommand.Parameters.Add("@FechaActual", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaNueva", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@UsuarioDigita", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaDigita", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@UsuarioAprueba", SqlDbType.Char,UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaAprueba", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ApruebaInd", SqlDbType.Bit);                
                mySqlCommand.Parameters.Add("@Observaciones", SqlDbType.Char,UDT_DescripTExt.MaxLength);
//                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@eg_pyTarea", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto",SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_pyJustificaModificacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = datos.NumeroDoc.Value;
                mySqlCommand.Parameters["@TareaID"].Value = datos.TareaID.Value;
                mySqlCommand.Parameters["@ProyectoID"].Value = datos.ProyectoID.Value;
                mySqlCommand.Parameters["@TipoAjuste"].Value = datos.TipoAjuste.Value;
                mySqlCommand.Parameters["@Codigo"].Value = datos.Codigo.Value;
                mySqlCommand.Parameters["@FechaActual"].Value = datos.FechaActual.Value;
                mySqlCommand.Parameters["@FechaNueva"].Value = datos.FechaNueva.Value;
                mySqlCommand.Parameters["@UsuarioDigita"].Value = datos.UsuarioDigita.Value;
                mySqlCommand.Parameters["@FechaDigita"].Value = datos.FechaDigita.Value;
                mySqlCommand.Parameters["@UsuarioAprueba"].Value = datos.UsuarioAprueba.Value;
                mySqlCommand.Parameters["@FechaAprueba"].Value = datos.FechaAprueba.Value;
                mySqlCommand.Parameters["@ApruebaInd"].Value = datos.ApruebaInd.Value;                
                mySqlCommand.Parameters["@Observaciones"].Value = datos.Observaciones.Value;
//                mySqlCommand.Parameters["@Consecutivo"].Value = Datos.Consecutivo.Value;
                mySqlCommand.Parameters["@eg_pyTarea"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyTarea, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_pyJustificaModificacion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyJustificaModificacion, this.Empresa, egCtrl);
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
                int numDoc = Convert.ToInt32(mySqlCommand.Parameters["@NumeroDoc"].Value);

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoModificaFechas_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Actualizar el registro en tabla prOrdenCompraDocu y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">Orden de Compra</param>
        public void DAL_pyProyectoModificaFechas_Upd(DTO_pyProyectoModificaFechas datos)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                //Actualiza Tabla prOrdenCompraDocu
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE pyProyectoModificaFechas set " +
                                                    " TareaID=@TareaID" +
                                                    ", ProyectoID=@ProyectoID" +
                                                    ", TipoAjuste=@TipoAjuste" +
                                                    ", Codigo=@Codigo"+
                                                    ", FechaActual=@FechaActual" +
                                                    ", FechaNueva=@FechaNueva" +
                                                    ", UsuarioDigita=@UsuarioDigita" +
                                                    ", FechaDigita=@FechaDigita" +
                                                    ", UsuarioAprueba=@UsuarioAprueba" +
                                                    ", FechaAprueba=@FechaAprueba" +
                                                    ", ApruebaInd=@ApruebaInd" +                                                    
                                                    ", Observaciones=@Observaciones" +
                                                    ",eg_pyTarea=@eg_pyTarea" +
                                                    ",eg_coProyecto=@eg_coProyecto" +
                                                    ",eg_pyJustificaModificacion=@eg_pyJustificaModificacion " +
                                           "    WHERE Consecutivo = @Consecutivo";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TareaID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@TipoAjuste", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Codigo", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommand.Parameters.Add("@FechaActual", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaNueva", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@UsuarioDigita", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaDigita", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@UsuarioAprueba", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaAprueba", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ApruebaInd", SqlDbType.Bit);                
                mySqlCommand.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@eg_pyTarea", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_pyJustificaModificacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@Consecutivo"].Value = datos.Consecutivo.Value;
                mySqlCommand.Parameters["@TareaID"].Value = datos.TareaID.Value;
                mySqlCommand.Parameters["@ProyectoID"].Value = datos.ProyectoID.Value;
                mySqlCommand.Parameters["@TipoAjuste"].Value = datos.TipoAjuste.Value;
                mySqlCommand.Parameters["@Codigo"].Value = datos.Codigo.Value;
                mySqlCommand.Parameters["@FechaActual"].Value = datos.FechaActual.Value;
                mySqlCommand.Parameters["@FechaNueva"].Value = datos.FechaNueva.Value;
                mySqlCommand.Parameters["@UsuarioDigita"].Value = datos.UsuarioDigita.Value;
                mySqlCommand.Parameters["@FechaDigita"].Value = datos.FechaDigita.Value;
                mySqlCommand.Parameters["@UsuarioAprueba"].Value = datos.UsuarioAprueba.Value;
                mySqlCommand.Parameters["@FechaAprueba"].Value = datos.FechaAprueba.Value;
                mySqlCommand.Parameters["@ApruebaInd"].Value = datos.ApruebaInd.Value;                
                mySqlCommand.Parameters["@Observaciones"].Value = datos.Observaciones.Value;
                mySqlCommand.Parameters["@eg_pyTarea"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyTarea, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_pyJustificaModificacion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyJustificaModificacion, this.Empresa, egCtrl);
                
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
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoModificaFechas_Upd");
                throw exception;
            }
        }

        #endregion
    }
}
