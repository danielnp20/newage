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
    public class DAL_glLlamadasControl : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glLlamadasControl(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trae todos los registros de glLlamadasControl
        /// </summary>
        /// <returns>retorna una lista de DTO_glLlamadasControl</returns>
        public List<DTO_glLlamadasControl> DAL_glLlamadasControl_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                List<DTO_glLlamadasControl> result = new List<DTO_glLlamadasControl>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "select llamadaCtrl.* , terceroRef.TipoReferencia, terceroRef.Nombre, llamPreg.Pregunta " +
                                           "from glLlamadasControl llamadaCtrl with(nolock) " + 
                                           "	INNER JOIN glTerceroReferencia terceroRef with(nolock) ON terceroRef.ReplicaID = llamadaCtrl.NumReferencia " +
                                           "    INNER JOIN glLLamadaPregunta llamPreg with(nolock) ON llamPreg.ReplicaID = llamadaCtrl.IdentificadorPrg " + 
                                           "where NumeroDoc = @NumeroDoc order by NumeroDoc ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_glLlamadasControl llamada = new DTO_glLlamadasControl(dr);
                    llamada.Pregunta.Value = dr["Pregunta"].ToString();
                    llamada.TipoReferencia.Value = Convert.ToByte(dr["TipoReferencia"]);
                    llamada.NombreReferencia.Value = dr["Nombre"].ToString();
                    result.Add(llamada);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glLlamadasControl_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega un nuevo registro
        /// </summary>
        /// <param name="act">Registro</param>
        public void DAL_glLlamadasControl_Add(DTO_glLlamadasControl llamadaCtrl)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "    INSERT INTO glLlamadasControl   " +
                                               "    ([NumeroDoc]   " +
                                               "    ,[ActividadFlujoID]   " +
                                               "    ,[NumReferencia]   " +
                                               "    ,[IdentificadorPrg]   " +
                                               "    ,[TerceroID]   " +
                                               "    ,[CodPregunta]   " +
                                               "    ,[UsuarioID]   " +
                                               "    ,[Fecha]   " +
                                               "    ,[PersonaConsulta]   " +
                                               "    ,[RelacionTitular]   " +
                                               "    ,[Observaciones]   " +
                                               "    ,[LlamadaREF]   " +
                                               "    ,[CodLLamada]   " +
                                               "    ,[NuevallamadaInd]   " +
                                               "    ,[FechaProxllamada]   " +
                                               "    ,[FechaCompromiso]   " +
                                               "    ,[ValorCompromiso1]   " +
                                               "    ,[ValorCompromiso2]   " +
                                               "    ,[ValorCompromiso3]   " +
                                               "    ,[ValorCompromiso4]   " +
                                               "    ,[ValorCompromiso5]   " +
                                               "    ,[eg_glActividadFlujo]   " +
                                               "    ,[eg_coTercero]   " +
                                               "    ,[eg_glLLamadaCodigo]  )" +
                                               "  VALUES    " +
                                               "  (@NumeroDoc    " +
                                               "  ,@ActividadFlujoID    " +
                                               "  ,@NumReferencia   " +
                                               "  ,@IdentificadorPrg    " +
                                               "  ,@TerceroID    " +                                               
                                               "  ,@CodPregunta    " +
                                               "  ,@UsuarioID    " +
                                               "  ,@Fecha    " +
                                               "  ,@PersonaConsulta    " +
                                               "  ,@RelacionTitular    " +
                                               "  ,@Observaciones    " +
                                               "  ,@LlamadaREF   " +
                                               "  ,@CodLLamada   " +
                                               "  ,@NuevallamadaInd    " +
                                               "  ,@FechaProxllamada    " +
                                               "  ,@FechaCompromiso    " +
                                               "  ,@ValorCompromiso1    " +
                                               "  ,@ValorCompromiso2    " +
                                               "  ,@ValorCompromiso3    " +
                                               "  ,@ValorCompromiso4    " +
                                               "  ,@ValorCompromiso5    " +
                                               "  ,@eg_glActividadFlujo    " +
                                               "  ,@eg_coTercero   "+
                                               "  ,@eg_glLLamadaCodigo )   ";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@NumReferencia", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@IdentificadorPrg", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CodPregunta", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@PersonaConsulta", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@RelacionTitular", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommand.Parameters.Add("@LlamadaREF", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@CodLLamada", SqlDbType.Char, UDT_CodigoGrl5.MaxLength);
                mySqlCommand.Parameters.Add("@LLamadaEfectivaInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@NuevallamadaInd", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaProxllamada", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaCompromiso", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ValorCompromiso1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorCompromiso2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorCompromiso3", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorCompromiso4", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorCompromiso5", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_glActividadFlujo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glLLamadaCodigo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = llamadaCtrl.NumeroDoc.Value;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = llamadaCtrl.ActividadFlujoID.Value;
                mySqlCommand.Parameters["@NumReferencia"].Value = llamadaCtrl.NumReferencia.Value;
                mySqlCommand.Parameters["@IdentificadorPrg"].Value = llamadaCtrl.IdentificadorPrg.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = llamadaCtrl.TerceroID.Value;
                mySqlCommand.Parameters["@CodPregunta"].Value = llamadaCtrl.CodPregunta.Value;
                mySqlCommand.Parameters["@UsuarioID"].Value = llamadaCtrl.UsuarioID.Value;
                mySqlCommand.Parameters["@Fecha"].Value = llamadaCtrl.Fecha.Value;
                mySqlCommand.Parameters["@PersonaConsulta"].Value = llamadaCtrl.PersonaConsulta.Value;
                mySqlCommand.Parameters["@RelacionTitular"].Value = llamadaCtrl.RelacionTitular.Value;
                mySqlCommand.Parameters["@Observaciones"].Value = llamadaCtrl.Observaciones.Value;
                mySqlCommand.Parameters["@LLamadaREF"].Value = llamadaCtrl.LLamadaREF.Value;
                mySqlCommand.Parameters["@CodLLamada"].Value = llamadaCtrl.CodLLamada.Value;
                mySqlCommand.Parameters["@NuevallamadaInd"].Value = llamadaCtrl.NuevallamadaInd.Value;
                mySqlCommand.Parameters["@FechaProxllamada"].Value = llamadaCtrl.FechaProxllamada.Value;
                mySqlCommand.Parameters["@FechaCompromiso"].Value = llamadaCtrl.FechaCompromiso.Value;
                mySqlCommand.Parameters["@ValorCompromiso1"].Value = llamadaCtrl.ValorCompromiso1.Value;
                mySqlCommand.Parameters["@ValorCompromiso2"].Value = llamadaCtrl.ValorCompromiso2.Value;
                mySqlCommand.Parameters["@ValorCompromiso3"].Value = llamadaCtrl.ValorCompromiso3.Value;
                mySqlCommand.Parameters["@ValorCompromiso4"].Value = llamadaCtrl.ValorCompromiso4.Value;
                mySqlCommand.Parameters["@ValorCompromiso5"].Value = llamadaCtrl.ValorCompromiso5.Value;
                mySqlCommand.Parameters["@eg_glActividadFlujo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glActividadFlujo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glLLamadaCodigo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLLamadaCodigo, this.Empresa, egCtrl);

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_gLlamadasControl_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los registros asociados a un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_glLlamadasControl_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "DELETE FROM glLlamadasControl WHERE NumeroDoc=@NumeroDoc ";
                
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glLlamadasControl_Delete");
                throw exception;
            }
        }

    }
}
