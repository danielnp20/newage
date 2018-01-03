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
    /// DAL de DAL_glGestionDocumentalBitacora
    /// </summary>
    public class DAL_glGestionDocumentalBitacora : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glGestionDocumentalBitacora(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega un registro al a la gestion documental
        /// </summary>
        public int DAL_glGestionDocumentalBitacora_Add(DTO_glGestionDocumentalBitacora doc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "INSERT INTO glGestionDocumentalBitacora( " +
                    "	EmpresaID,DocumentoTipoID,DocumentoClaseID,DocTipoMovimientoID,NumeroDoc,Fecha,TerceroID,Nombre, " +
                    "	DocumentoTercero,CodIdentificador,Observacion,eg_glDocumentoTipo,eg_glDocumentoClase,eg_glMovimientoTipo " +
                    ")VALUES( " +
                    "	@EmpresaID,@DocumentoTipoID,@DocumentoClaseID,@DocTipoMovimientoID,@NumeroDoc,@Fecha,@TerceroID,@Nombre, " +
                    "	@DocumentoTercero,@CodIdentificador,@Observacion,@eg_glDocumentoTipo,@eg_glDocumentoClase,@eg_glMovimientoTipo " +
                    ")  SET @Consecutivo = SCOPE_IDENTITY()";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoTipoID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoClaseID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommand.Parameters.Add("@DocTipoMovimientoID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@Nombre", SqlDbType.Char, 70);
                mySqlCommand.Parameters.Add("@DocumentoTercero", SqlDbType.Char, 30);
                mySqlCommand.Parameters.Add("@CodIdentificador", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glDocumentoTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glDocumentoClase", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glMovimientoTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int, 1);
                mySqlCommand.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoTipoID"].Value = doc.DocumentoTipoID.Value;
                mySqlCommand.Parameters["@DocumentoClaseID"].Value = doc.DocumentoClaseID.Value;
                mySqlCommand.Parameters["@DocTipoMovimientoID"].Value = doc.DocTipoMovimientoID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = doc.NumeroDoc.Value;
                mySqlCommand.Parameters["@Fecha"].Value = doc.Fecha.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = doc.TerceroID.Value;
                mySqlCommand.Parameters["@DocumentoTercero"].Value = doc.DocumentoTercero.Value;
                mySqlCommand.Parameters["@CodIdentificador"].Value = doc.CodIdentificador.Value;
                mySqlCommand.Parameters["@Observacion"].Value = doc.Observacion.Value;
                mySqlCommand.Parameters["@eg_glDocumentoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glDocumentoTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glDocumentoClase"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glDocumentoClase, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glMovimientoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glDocumentoMovimientoTipo, this.Empresa, egCtrl);
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
                int cons = Convert.ToInt32(mySqlCommand.Parameters["@Consecutivo"].Value);

                return cons;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glGestionDocumentalBitacora_Add");
                throw exception;
            }
        }

        #endregion
    }
}
