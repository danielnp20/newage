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
using System.ComponentModel;
using System.Reflection;

namespace NewAge.ADO
{
    public class DAL_ccSolicitudCompraCartera : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccSolicitudCompraCartera(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudComponentes
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        public void DAL_ccSolicitudCompraCartera_Add(DTO_ccSolicitudCompraCartera compraCartera)
        {
            try
            {
                List<DTO_ccSolicitudComponentes> result = new List<DTO_ccSolicitudComponentes>();
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query Coorporativa
                mySqlCommandSel.CommandText = "INSERT INTO ccSolicitudCompraCartera " +
                                                      "  ([NumeroDoc]   " +
                                                      "  ,[FinancieraID]   " +
                                                      "  ,[Documento]   " +
                                                      "  ,[DocCompra]   " +
                                                      "  ,[VlrCuota]   " +
                                                      "  ,[VlrSaldo]   " +
                                                      "  ,[DocAnticipo] " +
                                                      "  ,[IndRecibePazySalvo] " +
                                                      "  ,[FechaPazySalvo] " +
                                                      "  ,[UsuarioID] " +
                                                      "  ,[ExternaInd] " +
                                                      "  ,[eg_ccFinanciera])   " +
                                                      "VALUES    " +
                                                      "  (@NumeroDoc    " +
                                                      "  ,@FinancieraID   " +
                                                      "  ,@Documento   " +
                                                      "  ,@DocCompra   " +
                                                      "  ,@VlrCuota   " +
                                                      "  ,@VlrSaldo   " +
                                                      "  ,@DocAnticipo " +
                                                      "  ,@IndRecibePazySalvo " +
                                                      "  ,@FechaPazySalvo " +
                                                      "  ,@UsuarioID " +
                                                      "  ,@ExternaInd " +
                                                      "  ,@eg_ccFinanciera)   ";
                #endregion

                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FinancieraID", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@Documento", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocCompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSaldo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DocAnticipo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@IndRecibePazySalvo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FechaPazySalvo", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ExternaInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@eg_ccFinanciera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = compraCartera.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@FinancieraID"].Value = compraCartera.FinancieraID.Value;
                mySqlCommandSel.Parameters["@Documento"].Value = compraCartera.Documento.Value;
                mySqlCommandSel.Parameters["@DocCompra"].Value = compraCartera.DocCompra.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = compraCartera.VlrCuota.Value;
                mySqlCommandSel.Parameters["@VlrSaldo"].Value = compraCartera.VlrSaldo.Value;
                mySqlCommandSel.Parameters["@DocAnticipo"].Value = compraCartera.DocAnticipo.Value;
                mySqlCommandSel.Parameters["@IndRecibePazySalvo"].Value = compraCartera.IndRecibePazySalvo.Value;
                mySqlCommandSel.Parameters["@FechaPazySalvo"].Value = compraCartera.FechaPazySalvo.Value;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = compraCartera.UsuarioID.Value;
                mySqlCommandSel.Parameters["@ExternaInd"].Value = compraCartera.ExternaInd.Value;
                mySqlCommandSel.Parameters["@eg_ccFinanciera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                mySqlCommandSel.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudCompraCartera_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccSolicitudComponentes
        /// </summary>
        /// <returns>retorna una lista de DTO_ccSolicitudComponentes</returns>
        public List<DTO_ccSolicitudCompraCartera> DAL_ccSolicitudCompraCartera_GetByNumeroDoc(int NumeroDoc, bool allEmpresas)
        {
            try
            {
                List<DTO_ccSolicitudCompraCartera> result = new List<DTO_ccSolicitudCompraCartera>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT scc.*, fi.Descriptivo " +
                                           "FROM ccSolicitudCompraCartera scc with(nolock) " +
                                           "    inner join ccFinanciera fi  with(nolock) on scc.FinancieraID = fi.FinancieraID " +
                                           "WHERE scc.NumeroDoc = @NumeroDoc and DocCompra = 0 ";

                if (!allEmpresas)
                {
                    mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char);
                    mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                    mySqlCommand.CommandText += " and scc.EmpresaID=@EmpresaID";
                }

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                int index = 0;
                while (dr.Read())
                {
                    DTO_ccSolicitudCompraCartera compraCartera = new DTO_ccSolicitudCompraCartera(dr);
                    if (compraCartera.DocAnticipo.Value != null)
                        compraCartera.AnticipoInd.Value = true;
                    else
                        compraCartera.AnticipoInd.Value = false;

                    result.Add(compraCartera);
                    index++;
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudCompraCartera_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudComponentes
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccSolicitudCompraCartera_Update(DTO_ccSolicitudCompraCartera compraCartera)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FinancieraID", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@Documento", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocCompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSaldo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DocAnticipo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@IndRecibePazySalvo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FechaPazySalvo", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ExternaInd", SqlDbType.TinyInt);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@Consecutivo"].Value = compraCartera.Consecutivo.Value;
                mySqlCommandSel.Parameters["@FinancieraID"].Value = compraCartera.FinancieraID.Value;
                mySqlCommandSel.Parameters["@Documento"].Value = compraCartera.Documento.Value;
                mySqlCommandSel.Parameters["@DocCompra"].Value = compraCartera.DocCompra.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = compraCartera.VlrCuota.Value;
                mySqlCommandSel.Parameters["@VlrSaldo"].Value = compraCartera.VlrSaldo.Value;
                mySqlCommandSel.Parameters["@DocAnticipo"].Value = compraCartera.DocAnticipo.Value;
                mySqlCommandSel.Parameters["@IndRecibePazySalvo"].Value = compraCartera.IndRecibePazySalvo.Value;
                mySqlCommandSel.Parameters["@FechaPazySalvo"].Value = compraCartera.FechaPazySalvo.Value;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = compraCartera.UsuarioID.Value;
                mySqlCommandSel.Parameters["@ExternaInd"].Value = compraCartera.ExternaInd.Value;
                #endregion
                #region Query
                mySqlCommandSel.CommandText =
                                                "UPDATE ccSolicitudCompraCartera SET" +
                                                " FinancieraID = @FinancieraID  " +
                                                " ,Documento = @Documento " +
                                                " ,DocCompra = @DocCompra   " +
                                                " ,VlrCuota = @VlrCuota   " +
                                                " ,VlrSaldo = @VlrSaldo   " +
                                                " ,DocAnticipo = @DocAnticipo " +
                                                " ,IndRecibePazySalvo = @IndRecibePazySalvo " +
                                                " ,FechaPazySalvo = @FechaPazySalvo " +
                                                " ,UsuarioID = @UsuarioID " +
                                                " ,ExternaInd = @ExternaInd " +
                                                " WHERE  Consecutivo = @Consecutivo";
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudCompraCartera_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los componentes asociados a un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_ccSolicitudCompraCartera_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "DELETE FROM ccSolicitudCompraCartera WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudCompraCartera_Delete");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Trae la solicitud de compra de cartera, con base al numero doc del credito comprado
        /// </summary>
        /// <param name="NumeroDoc">Numero Doc del credito comprado</param>
        /// <returns>retorna la solicitud de compra de cartera</returns>
        public Tuple<int, byte, string> DAL_ccSolicitudCompraCartera_GetEstadoByDocCompra(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
             
                mySqlCommand.CommandText =
                    "SELECT ctrl.DocumentoTercero, ctrl.Estado, est.ActividadFlujoID " +
                    "FROM ccSolicitudCompraCartera scc with(nolock) " +
                    "    INNER JOIN glDocumentoControl ctrl with(nolock) ON scc.NumeroDoc = ctrl.NumeroDoc and Estado in (2,3) " +
                    "    INNER JOIN ccCreditoDocu crediDocu with(nolock) ON crediDocu.NumeroDoc = scc.DocCompra " +
                    "    LEFT JOIN glActividadEstado est with(nolock) ON scc.NumeroDoc = est.NumeroDoc and est.CerradoInd = 0 " + 
                    "WHERE scc.DocCompra = @NumeroDoc ";

                Tuple<int, byte, string> result = new Tuple<int, byte, string>(0, 0, string.Empty);
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new Tuple<int, byte, string>(Convert.ToInt32(dr["DocumentoTercero"]),  Convert.ToByte(dr["Estado"]), dr["ActividadFlujoID"].ToString());
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudCompraCartera_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna el listado de las compra de cartera para solicitud de anticipos
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="numDoc">Numero doc de la libranza</param>
        /// <returns>Retorna las compra de cartera para realizar la solicitud de anticipo</returns>     
        public List<DTO_ccSolicitudCompraCartera> DAL_ccSolicitudCompraCartera_GetForAnticipo(string actFlujoID, int numDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CerradoInd", SqlDbType.Bit);
                #endregion
                #region Asignacion Campos
                mySqlCommandSel.Parameters["@NumDoc"].Value = numDoc;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommandSel.Parameters["@CerradoInd"].Value = true;
                #endregion
                #region CommandText - Compra de cartera
                mySqlCommandSel.CommandText =
                    "SELECT compCartera.* , fin.Descriptivo , soliDocu.ClienteID " +
                    "FROM ccSolicitudCompraCartera compCartera with(nolock) " +
                    "	INNER JOIN ccFinanciera fin with(nolock) ON compCartera.FinancieraID = fin.FinancieraID " +
                    "	INNER JOIN ccSolicitudDocu soliDocu with(nolock) ON soliDocu.NumeroDoc = compCartera.NumeroDoc " +
                    "	INNER JOIN glActividadEstado act with(nolock) on act.NumeroDoc = compCartera.NumeroDoc " +
                    "	    AND act.CerradoInd=@CerradoInd and act.ActividadFlujoID=@ActividadFlujoID " +
                    "WHERE compCartera.NumeroDoc=@NumDoc ";
                #endregion
                #region Trae la lista de compras de cartera
                List<DTO_ccSolicitudCompraCartera> results = new List<DTO_ccSolicitudCompraCartera>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudCompraCartera dto = new DTO_ccSolicitudCompraCartera(dr);
                    dto.AnticipoInd.Value = false;
                    dto.ClienteID.Value = dr["ClienteID"].ToString();
                    if (dto.IndRecibePazySalvo.Value.Value)
                        dto.NuevoPyS.Value = false;
                    else
                        dto.NuevoPyS.Value = true;

                    results.Add(dto);
                }
                dr.Close();
                #endregion
                #region Revisa si tiene anticipos asignados
                foreach (DTO_ccSolicitudCompraCartera c in results)
                {
                    if (!c.ExternaInd.Value.Value || c.DocAnticipo.Value != null)
                        c.AnticipoInd.Value = true;
                    else
                        c.AnticipoInd.Value = false;
                }
                #endregion

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudCompraCartera_GetForAnticipo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public DTO_ccSolicitudCompraCartera DAL_ccSolicitudCompraCartera_GetByDocAnticipo(int docAnticipo)
        {
            try
            {
                DTO_ccSolicitudCompraCartera result = new DTO_ccSolicitudCompraCartera();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;

                query = " SELECT solComp.* ,fi.Descriptivo FROM   ccSolicitudCompraCartera solComp with(nolock) " +
                      "    inner join ccFinanciera fi  with(nolock) on solComp.FinancieraID = fi.FinancieraID and solComp.eg_ccFinanciera = fi.EmpresaGrupoID   " +
                        " WHERE DocAnticipo = @DocAnticipo";

                    mySqlCommand.Parameters.Add("@DocAnticipo", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocAnticipo"].Value = docAnticipo;
                
                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    result= new DTO_ccSolicitudCompraCartera(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudCompraCartera_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public DTO_ccSolicitudCompraCartera DAL_ccSolicitudCompraCartera_GetByDocCompra(int docCompra)
        {
            try
            {
                DTO_ccSolicitudCompraCartera result = new DTO_ccSolicitudCompraCartera();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = " SELECT solComp.*, fi.Descriptivo FROM  ccSolicitudCompraCartera solComp with(nolock) " +
                                "    left join ccFinanciera fi  with(nolock) on solComp.FinancieraID = fi.FinancieraID and solComp.eg_ccFinanciera = fi.EmpresaGrupoID   " +
                                " WHERE solComp.DocCompra = @DocCompra";

                mySqlCommand.Parameters.Add("@DocCompra", SqlDbType.Int);
                mySqlCommand.Parameters["@DocCompra"].Value = docCompra;

                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    result = new DTO_ccSolicitudCompraCartera(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudCompraCartera_GetByParameter");
                throw exception;
            }
        }

        #endregion

    }
}
