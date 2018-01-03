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
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_glIncumpleCambioEstado
    /// </summary>
    public class DAL_glIncumpleCambioEstado : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glIncumpleCambioEstado(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
        
        #region CRUD

        /// <summary>
        /// Agrega un registro al control de garantias
        /// </summary>
        public int DAL_glIncumpleCambioEstado_AddItem(DTO_glIncumpleCambioEstado incump)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "INSERT INTO glIncumpleCambioEstado" +
                            "           (EmpresaID" +                          
                            "           ,DocumentoID" +
                            "           ,TerceroID" +
                            "           ,EtapaIncumplimiento" +
                            "           ,Abogado" +
                            "           ,FechaINI" +
                            "           ,FechaFIN" +
                            "           ,FechaCompromisoIni" + 
                            "           ,FechaCompromiso" +
                            "           ,TerminaIncumplimientoInd" +
                            "           ,Observaciones" +
                            "           ,Numero1" +
                            "           ,Numero2" +
                            "           ,Numero3" +
                            "           ,Numero4" +
                            "           ,Numero5" +
                            "           ,Valor1" +
                            "           ,Valor2" +
                            "           ,Valor3" +
                            "           ,Valor4" +
                            "           ,Valor5" +
                            "           ,Dato1" +
                            "           ,Dato2" +
                            "           ,Dato3" +
                            "           ,Dato4" +
                            "           ,Dato5" +
                            "           ,eg_coTercero" +
                            "           ,eg_glIncumplimientoEtapa)" +
                            "     VALUES" +
                            "           (@EmpresaID" +
                            "           ,@DocumentoID" +
                            "           ,@TerceroID" +
                            "           ,@EtapaIncumplimiento" +
                            "           ,@Abogado" +
                            "           ,@FechaINI" +
                            "           ,@FechaFIN" +
                            "           ,@FechaCompromisoIni" +
                            "           ,@FechaCompromiso" +
                            "           ,@TerminaIncumplimientoInd" +
                            "           ,@Observaciones" +
                            "           ,@Numero1" +
                            "           ,@Numero2" +
                            "           ,@Numero3" +
                            "           ,@Numero4" +
                            "           ,@Numero5" +
                            "           ,@Valor1" +
                            "           ,@Valor2" +
                            "           ,@Valor3" +
                            "           ,@Valor4" +
                            "           ,@Valor5" +
                            "           ,@Dato1" +
                            "           ,@Dato2" +
                            "           ,@Dato3" +
                            "           ,@Dato4" +
                            "           ,@Dato5" +
                            "           ,@eg_coTercero" +
                            "           ,@eg_glIncumplimientoEtapa)" +
                             " SET @Consecutivo = SCOPE_IDENTITY()";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@EtapaIncumplimiento", SqlDbType.Char, 10);
                mySqlCommand.Parameters.Add("@Abogado", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaINI", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaFIN", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaCompromisoIni", SqlDbType.SmallDateTime); 
                mySqlCommand.Parameters.Add("@FechaCompromiso", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@TerminaIncumplimientoInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@Observaciones", SqlDbType.Char,UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@Numero1", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Numero2", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Numero3", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Numero4", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Numero5", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Valor1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor3", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor4", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor5", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Dato1", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@Dato2", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@Dato3", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@Dato4", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@Dato5", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glIncumplimientoEtapa", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int, 1);
                mySqlCommand.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = incump.DocumentoID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = incump.TerceroID.Value;
                mySqlCommand.Parameters["@EtapaIncumplimiento"].Value = incump.EtapaIncumplimiento.Value;
                mySqlCommand.Parameters["@Abogado"].Value = incump.Abogado.Value;
                mySqlCommand.Parameters["@FechaINI"].Value = incump.FechaINI.Value;
                mySqlCommand.Parameters["@FechaFIN"].Value = incump.FechaFIN.Value;
                mySqlCommand.Parameters["@FechaCompromisoIni"].Value = incump.FechaCompromisoIni.Value;
                mySqlCommand.Parameters["@FechaCompromiso"].Value = incump.FechaCompromiso.Value;
                mySqlCommand.Parameters["@TerminaIncumplimientoInd"].Value = incump.TerminaIncumplimientoInd.Value;
                mySqlCommand.Parameters["@Observaciones"].Value = incump.Observaciones.Value;
                mySqlCommand.Parameters["@Numero1"].Value = incump.Numero1.Value;
                mySqlCommand.Parameters["@Numero2"].Value = incump.Numero2.Value;
                mySqlCommand.Parameters["@Numero3"].Value = incump.Numero3.Value;
                mySqlCommand.Parameters["@Numero4"].Value = incump.Numero4.Value;
                mySqlCommand.Parameters["@Numero5"].Value = incump.Numero5.Value;
                mySqlCommand.Parameters["@Valor1"].Value = incump.Valor1.Value;
                mySqlCommand.Parameters["@Valor2"].Value = incump.Valor2.Value;
                mySqlCommand.Parameters["@Valor3"].Value = incump.Valor3.Value;
                mySqlCommand.Parameters["@Valor4"].Value = incump.Valor4.Value;
                mySqlCommand.Parameters["@Valor5"].Value = incump.Valor5.Value;
                mySqlCommand.Parameters["@Dato1"].Value = incump.Dato1.Value;
                mySqlCommand.Parameters["@Dato2"].Value = incump.Dato2.Value;
                mySqlCommand.Parameters["@Dato3"].Value = incump.Dato3.Value;
                mySqlCommand.Parameters["@Dato4"].Value = incump.Dato4.Value;
                mySqlCommand.Parameters["@Dato5"].Value = incump.Dato5.Value;
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glIncumplimientoEtapa"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glIncumplimientoEtapa, this.Empresa, egCtrl);
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
                int numDoc = Convert.ToInt32(mySqlCommand.Parameters["@Consecutivo"].Value);

                return numDoc;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glIncumpleCambioEstado_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Edita un registro al control de documentos
        /// </summary>
        /// <param name="incump">Documento que se va a editar</param>
        public int DAL_glIncumpleCambioEstado_UpdateItem(DTO_glIncumpleCambioEstado incump)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "UPDATE glIncumpleCambioEstado SET DocumentoID = @DocumentoID" +
                    "     , TerceroID = @TerceroID" +
                    "     , EtapaIncumplimiento = @EtapaIncumplimiento" +
                    "     , Abogado = @Abogado" +
                    "     , FechaINI = @FechaINI" +
                    "     , FechaFIN = @FechaFIN" +
                    "     , FechaCompromisoIni = @FechaCompromisoIni" +
                    "     , FechaCompromiso = @FechaCompromiso" +
                    "     , TerminaIncumplimientoInd = @TerminaIncumplimientoInd" +
                    "     , Observaciones = @Observaciones" +
                    "     , Numero1 = @Numero1" +
                    "     , Numero2 = @Numero2" +
                    "     , Numero3 = @Numero3" +
                    "     , Numero4 = @Numero4" +
                    "     , Numero5 = @Numero5" +
                    "     , Valor1 = @Valor1" +
                    "     , Valor2 = @Valor2" +
                    "     , Valor3 = @Valor3" +
                    "     , Valor4 = @Valor4" +
                    "     , Valor5 = @Valor5" +
                    "     , Dato1 = @Dato1" +
                    "     , Dato2 = @Dato2" +
                    "     , Dato3 = @Dato3" +
                    "     , Dato4 = @Dato4" +
                    "     , Dato5 = @Dato5" +
                        "WHERE Consecutivo = @Consecutivo";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int); 
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@EtapaIncumplimiento", SqlDbType.Char, 10);
                mySqlCommand.Parameters.Add("@Abogado", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaINI", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaFIN", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaCompromisoIni", SqlDbType.SmallDateTime); 
                mySqlCommand.Parameters.Add("@FechaCompromiso", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@TerminaIncumplimientoInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@Numero1", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Numero2", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Numero3", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Numero4", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Numero5", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Valor1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor3", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor4", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor5", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Dato1", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@Dato2", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@Dato3", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@Dato4", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@Dato5", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glIncumplimientoEtapa", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Consecutivo"].Value = incump.Consecutivo.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = incump.DocumentoID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = incump.TerceroID.Value;
                mySqlCommand.Parameters["@EtapaIncumplimiento"].Value = incump.EtapaIncumplimiento.Value;
                mySqlCommand.Parameters["@Abogado"].Value = incump.Abogado.Value;
                mySqlCommand.Parameters["@FechaINI"].Value = incump.FechaINI.Value;
                mySqlCommand.Parameters["@FechaFIN"].Value = incump.FechaFIN.Value;
                mySqlCommand.Parameters["@FechaCompromisoIni"].Value = incump.FechaCompromisoIni.Value; 
                mySqlCommand.Parameters["@FechaCompromiso"].Value = incump.FechaCompromiso.Value;
                mySqlCommand.Parameters["@TerminaIncumplimientoInd"].Value = incump.TerminaIncumplimientoInd.Value;
                mySqlCommand.Parameters["@Observaciones"].Value = incump.Observaciones.Value;
                mySqlCommand.Parameters["@Numero1"].Value = incump.Numero1.Value;
                mySqlCommand.Parameters["@Numero2"].Value = incump.Numero2.Value;
                mySqlCommand.Parameters["@Numero3"].Value = incump.Numero3.Value;
                mySqlCommand.Parameters["@Numero4"].Value = incump.Numero4.Value;
                mySqlCommand.Parameters["@Numero5"].Value = incump.Numero5.Value;
                mySqlCommand.Parameters["@Valor1"].Value = incump.Valor1.Value;
                mySqlCommand.Parameters["@Valor2"].Value = incump.Valor2.Value;
                mySqlCommand.Parameters["@Valor3"].Value = incump.Valor3.Value;
                mySqlCommand.Parameters["@Valor4"].Value = incump.Valor4.Value;
                mySqlCommand.Parameters["@Valor5"].Value = incump.Valor5.Value;
                mySqlCommand.Parameters["@Dato1"].Value = incump.Dato1.Value;
                mySqlCommand.Parameters["@Dato2"].Value = incump.Dato2.Value;
                mySqlCommand.Parameters["@Dato3"].Value = incump.Dato3.Value;
                mySqlCommand.Parameters["@Dato4"].Value = incump.Dato4.Value;
                mySqlCommand.Parameters["@Dato5"].Value = incump.Dato5.Value;
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glIncumplimientoEtapa"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glIncumplimientoEtapa, this.Empresa, egCtrl);
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

                return incump.Consecutivo.Value.Value;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glIncumpleCambioEstado_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de glIncumpleCambioEstado
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public int DAL_glIncumpleCambioEstado_AddOrUpdate(DTO_glIncumpleCambioEstado incump)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT COUNT (*) from glIncumpleCambioEstado with(nolock) " +
                    "WHERE Consecutivo = @Consecutivo";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@Consecutivo"].Value = incump.Consecutivo.Value;
                #endregion

                //Verifica si agrega o actualiza el registro
                int count = Convert.ToInt32(mySqlCommandSel.ExecuteScalar());
                int consecutivo = 0;
                if (count == 0)
                    consecutivo = this.DAL_glIncumpleCambioEstado_AddItem(incump);
                else
                    consecutivo = this.DAL_glIncumpleCambioEstado_UpdateItem(incump);

                return consecutivo;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glIncumpleCambioEstado_Add");
                throw exception;
            }
        }

        #endregion       
 
        #region Otras

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"> filtro</param>
        /// <returns>Lista </returns>
        public List<DTO_glIncumpleCambioEstado> DAL_glIncumpleCambioEstado_GetByParameter(DTO_glIncumpleCambioEstado filter)
        {
            try
            {
                List<DTO_glIncumpleCambioEstado> result = new List<DTO_glIncumpleCambioEstado>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;

                query = "select * " +
                        "from glIncumpleCambioEstado with(nolock) " +
                        "where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                //if (!string.IsNullOrEmpty(filter.NumeroDoc.Value.ToString()))
                //{
                //    query += "and NumeroDoc = @NumeroDoc ";
                //    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                //    mySqlCommand.Parameters["@NumeroDoc"].Value = filter.NumeroDoc.Value.Value;
                //}
                if (!string.IsNullOrEmpty(filter.DocumentoID.Value.ToString()))
                {
                    query += "and DocumentoID = @DocumentoID ";
                    mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocumentoID"].Value = filter.DocumentoID.Value;
                }
                if (!string.IsNullOrEmpty(filter.TerceroID.Value.ToString()))
                {
                    query += "and TerceroID = @TerceroID ";
                    mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommand.Parameters["@TerceroID"].Value = filter.TerceroID.Value;
                }              
                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_glIncumpleCambioEstado ctrl = new DTO_glIncumpleCambioEstado(dr);
                    result.Add(ctrl);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glIncumpleCambioEstado_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un documento relacionado a un comprobante
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Identificador del comprobante</param>
        /// <param name="compNro">Numeor de comprobante</param>
        /// <param name="estado">Estado del comprobante</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Retorna </returns>
        public DTO_glIncumpleCambioEstado DAL_glIncumpleCambioEstado_GetByConsecutivo(int consecutivo)
        {
            try
            {
                DTO_glIncumpleCambioEstado result = new DTO_glIncumpleCambioEstado();
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "SELECT * from glIncumpleCambioEstado with(nolock) WHERE Consecutivo = @Consecutivo";
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consecutivo;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_glIncumpleCambioEstado(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glIncumpleCambioEstado_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Edita un registro al control de documentos
        /// </summary>
        /// <param name="incump">Documento que se va a editar</param>
        public void DAL_glIncumpleCambioEstado_CierraEstado(int consecutivo, DateTime fechaFin)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "UPDATE glIncumpleCambioEstado SET FechaFIN = @FechaFIN WHERE Consecutivo = @Consecutivo";

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaFIN", SqlDbType.SmallDateTime);

                mySqlCommand.Parameters["@Consecutivo"].Value = consecutivo;
                mySqlCommand.Parameters["@FechaFIN"].Value = fechaFin;

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glIncumpleCambioEstado_CierraEstado");
                throw exception;
            }
        }

        #endregion
    }
}
