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

namespace NewAge.ADO
{
    /// <summary>
    /// DAL_inFisicoInventario
    /// </summary>
    public class DAL_inFisicoInventario : DAL_Base
    {
       /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_inFisicoInventario(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="fisicoInventario"></param>
        /// <returns>Lista de items de inventario fisico</returns>
        public List<DTO_inFisicoInventario> DAL_inFisicoInventario_GetByParameter(DTO_inFisicoInventario fisicoInventario)
        {
            try
            {
                List<DTO_inFisicoInventario> result = new List<DTO_inFisicoInventario>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;

                query = "select * from inFisicoInventario with(nolock) " +
                                           "where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                if (!string.IsNullOrEmpty(fisicoInventario.BodegaID.Value))
                {
                    query += "and BodegaID = @BodegaID ";
                    mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                    mySqlCommand.Parameters["@BodegaID"].Value = fisicoInventario.BodegaID.Value;
                }
                if (!string.IsNullOrEmpty(fisicoInventario.inReferenciaID.Value))
                {
                    query += "and inReferenciaID = @inReferenciaID ";
                    mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_ReferenciaID.MaxLength);
                    mySqlCommand.Parameters["@inReferenciaID"].Value = fisicoInventario.inReferenciaID.Value;
                }
                if (!string.IsNullOrEmpty(fisicoInventario.ActivoID.Value.ToString()))
                {
                    query += "and ActivoID = @ActivoID ";
                    mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                    mySqlCommand.Parameters["@ActivoID"].Value = fisicoInventario.ActivoID.Value;
                }
                if (!string.IsNullOrEmpty(fisicoInventario.EstadoInv.Value.ToString()))
                {
                    query += "and EstadoInv = @EstadoInv ";
                    mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@EstadoInv"].Value = fisicoInventario.EstadoInv.Value;
                }
                if (!string.IsNullOrEmpty(fisicoInventario.Parametro1.Value.ToString()))
                {
                    query += "and Parametro1 = @Parametro1 ";
                    mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                    mySqlCommand.Parameters["@Parametro1"].Value = fisicoInventario.Parametro1.Value;
                }
                if (!string.IsNullOrEmpty(fisicoInventario.Parametro2.Value.ToString()))
                {
                    query += "and Parametro2 = @Parametro2 ";
                    mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                    mySqlCommand.Parameters["@Parametro2"].Value = fisicoInventario.Parametro2.Value;
                }
                if (!string.IsNullOrEmpty(fisicoInventario.Periodo.Value.ToString()))
                {
                    query += "and Periodo = @Periodo ";
                    mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@Periodo"].Value = fisicoInventario.Periodo.Value;
                }

                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_inFisicoInventario fisico = new DTO_inFisicoInventario(dr);
                    result.Add(fisico);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inFisicoInventario_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="bodegaID">Bodega a revisar</param>
        /// <param name="periodo">Perido a revisar el inventario</param>
        /// <returns>True si el inventario fisico de la bodega existe, si no False</returns>
        public bool DAL_inFisicoInventario_Exist(string bodegaID, DateTime periodo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText = "Select COUNT (*)  from inFisicoInventario with(nolock) " +
                                           "where EmpresaID = @EmpresaID " +
                                           "and BodegaID = @BodegaID " +
                                           "and Periodo = @Periodo ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                mySqlCommand.Parameters["@BodegaID"].Value = bodegaID;
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommand.Parameters["@Periodo"].Value = periodo;

                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inFisicoInventario_Exist");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona en tabla inFisicoInventario
        /// </summary>
        /// <param name="invFisico">items a agregar a inv fisico</param>
        /// <returns>Numero Doc</returns>
        public int DAL_inFisicoInventario_Add(DTO_inFisicoInventario invFisico)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = " INSERT INTO inFisicoInventario " +
                                           "(EmpresaID " +
                                           ",BodegaID " +
                                           ",inReferenciaID " +
                                           ",ActivoID " +
                                           ",EstadoInv " +
                                           ",Parametro1 " +
                                           ",Parametro2 " +
                                           ",Periodo " +
                                           ",CantKardex " +
                                           ",CantFisico " +
                                           ",CantEntradaDoc " +
                                           ",CantSalidaDoc " +
                                           ",CantAjuste " +
                                           ",FobLocal " +
                                           ",CostoLocal " +
                                           ",FobExtra " +
                                           ",CostoExtra " +
                                           ",NumeroDoc " +
                                           ",Observacion " +
                                           ",eg_inBodega " +
                                           ",eg_inReferencia " +
                                           ",eg_inRefParametro1" +
                                           ",eg_inRefParametro2 )" +
                                           "VALUES" +
                                           "(@EmpresaID " +
                                           ",@BodegaID " +
                                           ",@inReferenciaID " +
                                           ",@ActivoID " +
                                           ",@EstadoInv " +
                                           ",@Parametro1 " +
                                           ",@Parametro2 " +
                                           ",@Periodo " +
                                           ",@CantKardex " +
                                           ",@CantFisico " +
                                           ",@CantEntradaDoc " +
                                           ",@CantSalidaDoc " +
                                           ",@CantAjuste " +
                                           ",@FobLocal " +
                                           ",@CostoLocal " +
                                           ",@FobExtra " +
                                           ",@CostoExtra " +
                                           ",@NumeroDoc " +
                                           ",@Observacion " +
                                           ",@eg_inBodega " +
                                           ",@eg_inReferencia " +
                                           ",@eg_inRefParametro1" +
                                           ",@eg_inRefParametro2)";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@CantKardex", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantFisico", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantEntradaDoc", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantSalidaDoc", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantAjuste", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobLocal", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CostoLocal", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobExtra", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CostoExtra", SqlDbType.Decimal);              
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inBodega", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inRefParametro1", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inRefParametro2", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@BodegaID"].Value = invFisico.BodegaID.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = invFisico.inReferenciaID.Value;
                mySqlCommand.Parameters["@ActivoID"].Value = invFisico.ActivoID.Value;
                mySqlCommand.Parameters["@EstadoInv"].Value = invFisico.EstadoInv.Value;
                mySqlCommand.Parameters["@Parametro1"].Value = invFisico.Parametro1.Value;
                mySqlCommand.Parameters["@Parametro2"].Value = invFisico.Parametro2.Value;
                mySqlCommand.Parameters["@Periodo"].Value = invFisico.Periodo.Value;
                mySqlCommand.Parameters["@CantKardex"].Value = invFisico.CantKardex.Value.Value;
                mySqlCommand.Parameters["@CantFisico"].Value = invFisico.CantFisico.Value.Value;
                mySqlCommand.Parameters["@CantEntradaDoc"].Value = invFisico.CantEntradaDoc.Value.Value;
                mySqlCommand.Parameters["@CantSalidaDoc"].Value = invFisico.CantSalidaDoc.Value.Value;
                mySqlCommand.Parameters["@CantAjuste"].Value = invFisico.CantAjuste.Value.Value;
                mySqlCommand.Parameters["@FobLocal"].Value = invFisico.FobLocal.Value.Value;
                mySqlCommand.Parameters["@CostoLocal"].Value = invFisico.CostoLocal.Value.Value;
                mySqlCommand.Parameters["@FobExtra"].Value = invFisico.FobExtra.Value.Value;
                mySqlCommand.Parameters["@CostoExtra"].Value = invFisico.CostoExtra.Value.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = string.IsNullOrEmpty(invFisico.NumeroDoc.Value.ToString()) ? 0 : invFisico.NumeroDoc.Value.Value;
                mySqlCommand.Parameters["@Observacion"].Value = invFisico.Observacion.Value;
                mySqlCommand.Parameters["@eg_inBodega"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inBodega, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inRefParametro1"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inRefParametro1, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inRefParametro2"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inRefParametro2, this.Empresa, egCtrl);
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
                return numDoc;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inFisicoInventario_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Elimina registros de la tabla de inFisicoInventario
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public void DAL_inFisicoInventario_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText = "DELETE FROM inFisicoInventario where EmpresaID = @EmpresaID and " +
                "NumeroDoc = @NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inFisicoInventario_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de inventarios fisico pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <param name="actividadFlujoID">actividad actual</param>
        /// <returns>Lista de Inventario pendientes a aprobar</returns>
        public List<DTO_InvFisicoAprobacion> DAL_inFisicoInventario_GetPendientesByModulo(ModulesPrefix mod, string actividadFlujoID, string usuarioID)
        {
            try
            {
                List<DTO_InvFisicoAprobacion> result = new List<DTO_InvFisicoAprobacion>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select distinct ctrl.NumeroDoc, PeriodoDoc as PeriodoID, ctrl.DocumentoID, ctrl.ComprobanteID, ctrl.ComprobanteIDNro as ComprobanteNro, " + 
                    "   ctrl.DocumentoNro, ctrl.Observacion as BodegaID, usr.UsuarioID " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "   inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "	    and act.CerradoInd=@CerradoInd and act.ActividadFlujoID=@ActividadFlujoID " +
                    "   inner join inFisicoInventario header with(nolock) on ctrl.NumeroDoc = header.NumeroDoc" +
                    "	inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                    "   inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID " +
                    "	inner join glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    "       and perm.UsuarioID = @UsuarioID and perm.AreaFuncionalID = ctrl.AreaFuncionalID  " +
                    "where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado and perm.ActividadFlujoID = @ActividadFlujoID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.ParaAprobacion;
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuarioID;

                SqlDataReader dr;

                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_InvFisicoAprobacion dto = new DTO_InvFisicoAprobacion(dr);
                    dto.Aprobado.Value = false;
                    dto.Rechazado.Value = false;
                    dto.Observacion.Value = string.Empty;
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inFisicoInventario_GetPendientesByModulo");
                throw exception;
            }
        }

        #endregion
    }
}
