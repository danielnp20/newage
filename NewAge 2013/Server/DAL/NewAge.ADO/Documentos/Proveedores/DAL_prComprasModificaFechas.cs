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
    /// DAL_prComprasModificaFechas
    /// </summary>
    public class DAL_prComprasModificaFechas : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prComprasModificaFechas(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Consulta un Orden de Compra segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>

        public DTO_prComprasModificaFechas DAL_prComprasModificaFechas_Load(int NumeroDoc)
        {
            try
            {
                DTO_prComprasModificaFechas result = new DTO_prComprasModificaFechas();
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prComprasModificaFechas with(nolock) where NumeroDoc = @NumeroDoc ";//and consecutivo=@Consecutivo

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
  //              mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
//                mySqlCommand.Parameters["@Consecutivo"].Value = Consecutivo;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
               if (dr.Read())
                {
                    result = new DTO_prComprasModificaFechas(dr);
                }
                dr.Close();
                return result;
            
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prComprasModificaFechas_Get");
                throw exception;
            }
        }

        public List<DTO_prComprasModificaFechas> DAL_prComprasModificaFechas_GetByNumeroDoc(int NumeroDoc,bool aprobadoDocInd)
        {
            try
            {
                List<DTO_prComprasModificaFechas> results = new List<DTO_prComprasModificaFechas>();
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                
                mySqlCommand.CommandText = "select * from prComprasModificaFechas with(nolock) where NumeroDoc = @NumeroDoc and AprobadoDocInd = @AprobadoDocInd order by consecutivo desc";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                mySqlCommand.Parameters.Add("@AprobadoDocInd", SqlDbType.Bit);
                mySqlCommand.Parameters["@AprobadoDocInd"].Value = aprobadoDocInd;

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_prComprasModificaFechas tarea = new DTO_prComprasModificaFechas(dr);
                    results.Add(tarea);
                }
                dr.Close(); 
                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prComprasModificaFechas_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla prComprasModificaFechas 
        /// </summary>
        /// <param name="sol">Orden de Compra</param>
        /// <returns></returns>
        public void DAL_prComprasModificaFechas_Add(DTO_prComprasModificaFechas datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prComprasModificaFechas " +
                                           "    ( NumeroDoc, ProyectoID, Codigo,FechaEntrega, FechaNueva, UsuarioDigita, FechaDigita," +
                                           "    UsuarioAprueba, FechaAprueba, ApruebaInd,AprobadoDocInd, Observaciones, eg_coProyecto,eg_prJustificaModificacion) " +
                                            "    VALUES" +
                                           "    ( @NumeroDoc, @ProyectoID,@Codigo, @FechaEntrega, @FechaNueva, @UsuarioDigita, @FechaDigita," +
                                           "    @UsuarioAprueba, @FechaAprueba, @ApruebaInd, @AprobadoDocInd, @Observaciones,@eg_coProyecto,@eg_prJustificaModificacion) ";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char,UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@Codigo", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommand.Parameters.Add("@FechaEntrega", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaNueva", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@UsuarioDigita", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaDigita", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@UsuarioAprueba", SqlDbType.Char,UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaAprueba", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ApruebaInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@AprobadoDocInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@Observaciones", SqlDbType.Char,UDT_DescripTExt.MaxLength);
  //              mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@eg_coProyecto",SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_prJustificaModificacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = datos.NumeroDoc.Value;
                mySqlCommand.Parameters["@ProyectoID"].Value = datos.ProyectoID.Value;
                mySqlCommand.Parameters["@Codigo"].Value = datos.Codigo.Value;
                mySqlCommand.Parameters["@FechaEntrega"].Value = datos.FechaEntrega.Value;
                mySqlCommand.Parameters["@FechaNueva"].Value = datos.FechaNueva.Value;
                mySqlCommand.Parameters["@UsuarioDigita"].Value = datos.UsuarioDigita.Value;
                mySqlCommand.Parameters["@FechaDigita"].Value = datos.FechaDigita.Value;
                mySqlCommand.Parameters["@UsuarioAprueba"].Value = datos.UsuarioAprueba.Value;
                mySqlCommand.Parameters["@FechaAprueba"].Value = datos.FechaAprueba.Value;
                mySqlCommand.Parameters["@ApruebaInd"].Value = datos.ApruebaInd.Value;
                mySqlCommand.Parameters["@AprobadoDocInd"].Value = datos.AprobadoDocInd.Value;
                mySqlCommand.Parameters["@Observaciones"].Value = datos.Observaciones.Value;
//                mySqlCommand.Parameters["@Consecutivo"].Value = Datos.Consecutivo.Value;
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_prJustificaModificacion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prJustificaModificacion, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prComprasModificaFechas_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Actualizar el registro en tabla prOrdenCompraDocu y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">Orden de Compra</param>
        public void DAL_prComprasModificaFechas_Upd(DTO_prComprasModificaFechas datos)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                //Actualiza Tabla prOrdenCompraDocu
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE prComprasModificaFechas set " +                                                    
                                                    " ProyectoID=@ProyectoID" +
                                                    ", Codigo=@Codigo"+
                                                    ", FechaEntrega=@FechaEntrega" +
                                                    ", FechaNueva=@FechaNueva" +
                                                    ", UsuarioDigita=@UsuarioDigita" +
                                                    ", FechaDigita=@FechaDigita" +
                                                    ", UsuarioAprueba=@UsuarioAprueba" +
                                                    ", FechaAprueba=@FechaAprueba" +
                                                    ", ApruebaInd=@ApruebaInd" +
                                                    ", AprobadoDocInd=@AprobadoDocInd" +
                                                    ", Observaciones=@Observaciones" +
                                                    ",eg_coProyecto=@eg_coProyecto" +
                                                    ",eg_prJustificaModificacion=@eg_prJustificaModificacion " +
                                           "    WHERE Consecutivo = @Consecutivo";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@Codigo", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommand.Parameters.Add("@FechaEntrega", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaNueva", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@UsuarioDigita", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaDigita", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@UsuarioAprueba", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaAprueba", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ApruebaInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@AprobadoDocInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_prJustificaModificacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@Consecutivo"].Value = datos.Consecutivo.Value;
                mySqlCommand.Parameters["@ProyectoID"].Value = datos.ProyectoID.Value;
                mySqlCommand.Parameters["@Codigo"].Value = datos.Codigo.Value;
                mySqlCommand.Parameters["@FechaEntrega"].Value = datos.FechaEntrega.Value;
                mySqlCommand.Parameters["@FechaNueva"].Value = datos.FechaNueva.Value;
                mySqlCommand.Parameters["@UsuarioDigita"].Value = datos.UsuarioDigita.Value;
                mySqlCommand.Parameters["@FechaDigita"].Value = datos.FechaDigita.Value;
                mySqlCommand.Parameters["@UsuarioAprueba"].Value = datos.UsuarioAprueba.Value;
                mySqlCommand.Parameters["@FechaAprueba"].Value = datos.FechaAprueba.Value;
                mySqlCommand.Parameters["@ApruebaInd"].Value = datos.ApruebaInd.Value;
                mySqlCommand.Parameters["@AprobadoDocInd"].Value = datos.AprobadoDocInd.Value;
                mySqlCommand.Parameters["@Observaciones"].Value = datos.Observaciones.Value;
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_prJustificaModificacion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prJustificaModificacion, this.Empresa, egCtrl);
                
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prComprasModificaFechas_Upd");
                throw exception;
            }
        }

        #endregion
    }
}
