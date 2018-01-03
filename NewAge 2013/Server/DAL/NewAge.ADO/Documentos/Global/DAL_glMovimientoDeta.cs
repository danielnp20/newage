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
using SentenceTransformer;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_glMovimientoDeta
    /// </summary>
    public class DAL_glMovimientoDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glMovimientoDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region glMovimientoDeta

        #region CRUD

        /// <summary>
        /// Consulta un movimientoDeta segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns>lista de Movimientos</returns>
        public List<DTO_glMovimientoDeta> DAL_glMovimientoDeta_GetByNumeroDoc(int NumeroDoc, bool trasladoNotaEnvio = false)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = " Select det.*,serv.ServicioID,con.ConceptoCargoID from glMovimientoDeta det with(nolock) " +
                                            " left join faServicios serv on serv.ServicioID = det.ServicioID and serv.EmpresaGrupoID = det.eg_faServicios " +
                                            " left join faConceptos con on con.ConceptoIngresoID = serv.ConceptoIngresoID and con.EmpresaGrupoID = serv.eg_faConceptos " +
                                            " where det.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                //Filtra la consulta por movimientos de salida
                if (trasladoNotaEnvio)
                {
                    mySqlCommand.Parameters.Add("@EntradaSalida", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@EntradaSalida"].Value = (byte)EntradaSalida.Entrada;
                    mySqlCommand.CommandText += " and EntradaSalida = @EntradaSalida";
                }

                List<DTO_glMovimientoDeta> footer = new List<DTO_glMovimientoDeta>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_glMovimientoDeta detail = new DTO_glMovimientoDeta(dr);
                    detail.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                    detail.Index = index;
                    footer.Add(detail);
                    index++;
                }
                dr.Close();
                return footer;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDeta_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta un movimientoDeta segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns>lista de Movimientos</returns>
        public List<DTO_glMovimientoDeta> DAL_glMovimientoDeta_GetByConsecutivo(int Consecutivo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from glMovimientoDeta with(nolock) where Consecutivo = @Consecutivo ";

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = Consecutivo;

                List<DTO_glMovimientoDeta> footer = new List<DTO_glMovimientoDeta>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_glMovimientoDeta detail = new DTO_glMovimientoDeta(dr);
                    detail.Index = index;
                    footer.Add(detail);
                    index++;
                }
                dr.Close();
                return footer;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDeta_GetByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona en tabla glMovimientoDeta
        /// </summary>
        /// <param name="footer">Movimientos</param>
        public void DAL_glMovimientoDeta_Add(List<DTO_glMovimientoDeta> footer)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO glMovimientoDeta " +
                                           "    (EmpresaID    " +
                                           "    ,NumeroDoc    " +
                                           "    ,BodegaID " +
                                           "    ,EntradaSalida " +
                                           "    ,Kit " +
                                           "    ,inReferenciaID " +
                                           "    ,EstadoInv " +
                                           "    ,Parametro1 " +
                                           "    ,Parametro2 " +
                                           "    ,IdentificadorTr " +
                                           "    ,CodigoBSID " +
                                           "    ,ServicioID " +
                                           "    ,SerialID " +
                                           "    ,ActivoID " +
                                           "    ,MvtoTipoInvID " +
                                           "    ,MvtoTipoActID " +
                                           "    ,DocSoporte " +
                                           "    ,DocSoporteTER " +
                                           "    ,AsesorID " +
                                           "    ,ProyectoID " +
                                           "    ,CentroCostoID " +
                                           "    ,LineaPresupuestoID " +
                                           "    ,TerceroID " +
                                           "    ,DatoAdd1 " +
                                           "    ,DatoAdd2 " +
                                           "    ,DatoAdd3 " +
                                           "    ,DatoAdd4 " +
                                           "    ,DatoAdd5 " +
                                           "    ,DescripTExt " +
                                           "    ,EmpaqueInvID " +
                                           "    ,CantidadEMP " +
                                           "    ,CantidadDoc " +
                                           "    ,CantidadUNI " +
                                           "    ,ValorUNI " +
                                           "    ,Valor1LOC " +
                                           "    ,Valor2LOC " +
                                           "    ,Valor3LOC " +
                                           "    ,Valor4LOC " +
                                           "    ,Valor5LOC " +
                                           "    ,Valor6LOC " +
                                           "    ,Valor7LOC " +
                                           "    ,Valor8LOC " +
                                           "    ,Valor9LOC " +
                                           "    ,Valor10LOC " +
                                           "    ,Valor1EXT " +
                                           "    ,Valor2EXT " +
                                           "    ,Valor3EXT " +
                                           "    ,Valor4EXT " +
                                           "    ,Valor5EXT " +
                                           "    ,Valor6EXT " +
                                           "    ,Valor7EXT " +
                                           "    ,Valor8EXT " +
                                           "    ,Valor9EXT " +
                                           "    ,Valor10EXT " +
                                           "    ,InReferenciaCodID" +
                                           "    ,CantidadDEV" +
                                           "    ,ConsecutivoPrestamo" +
                                           "    ,ConsecutivoOrdCompra" +
                                           "    ,NroItem" +
                                           "    ,ImprimeInd" +
                                           "    ,eg_inBodega " +
                                           "    ,eg_inReferencia " +
                                           "    ,eg_inRefParametro1 " +
                                           "    ,eg_inRefParametro2 " +
                                           "    ,eg_prBienServicio " +
                                           "    ,eg_faServicios " +
                                           "    ,eg_inMovimientoTipo " +
                                           "    ,eg_acMovimientoTipo " +
                                           "    ,eg_faAsesor " +
                                           "    ,eg_coProyecto " +
                                           "    ,eg_coCentroCosto " +
                                           "    ,eg_coTercero " +
                                           "    ,eg_inEmpaque " +
                                           "    ,eg_plLineaPresupuesto) " +
                                           "    VALUES          " +
                                           "    (@EmpresaID     " +
                                           "    ,@NumeroDoc     " +
                                           "    ,@BodegaID " +
                                           "    ,@EntradaSalida " +
                                           "    ,@Kit " +
                                           "    ,@inReferenciaID " +
                                           "    ,@EstadoInv " +
                                           "    ,@Parametro1 " +
                                           "    ,@Parametro2 " +
                                           "    ,@IdentificadorTr " +
                                           "    ,@CodigoBSID " +
                                           "    ,@ServicioID " +
                                           "    ,@SerialID " +
                                           "    ,@ActivoID " +
                                           "    ,@MvtoTipoInvID " +
                                           "    ,@MvtoTipoActID " +
                                           "    ,@DocSoporte " +
                                           "    ,@DocSoporteTER " +
                                           "    ,@AsesorID " +
                                           "    ,@ProyectoID " +
                                           "    ,@CentroCostoID " +
                                           "    ,@LineaPresupuestoID " +
                                           "    ,@TerceroID " +
                                           "    ,@DatoAdd1 " +
                                           "    ,@DatoAdd2 " +
                                           "    ,@DatoAdd3 " +
                                           "    ,@DatoAdd4 " +
                                           "    ,@DatoAdd5 " +
                                           "    ,@DescripTExt " +
                                           "    ,@EmpaqueInvID " +
                                           "    ,@CantidadEMP " +
                                           "    ,@CantidadDoc " +
                                           "    ,@CantidadUNI " +
                                           "    ,@ValorUNI " +
                                           "    ,@Valor1LOC " +
                                           "    ,@Valor2LOC " +
                                           "    ,@Valor3LOC " +
                                           "    ,@Valor4LOC " +
                                           "    ,@Valor5LOC " +
                                           "    ,@Valor6LOC " +
                                           "    ,@Valor7LOC " +
                                           "    ,@Valor8LOC " +
                                           "    ,@Valor9LOC " +
                                           "    ,@Valor10LOC " +
                                           "    ,@Valor1EXT " +
                                           "    ,@Valor2EXT " +
                                           "    ,@Valor3EXT " +
                                           "    ,@Valor4EXT " +
                                           "    ,@Valor5EXT " +
                                           "    ,@Valor6EXT " +
                                           "    ,@Valor7EXT " +
                                           "    ,@Valor8EXT " +
                                           "    ,@Valor9EXT " +
                                           "    ,@Valor10EXT " +
                                           "    ,@InReferenciaCodID " +
                                           "    ,@CantidadDEV" +
                                           "    ,@ConsecutivoPrestamo" +
                                           "    ,@ConsecutivoOrdCompra" +
                                           "    ,@NroItem" +
                                           "    ,@ImprimeInd" +
                                           "    ,@eg_inBodega " +
                                           "    ,@eg_inReferencia " +
                                           "    ,@eg_inRefParametro1 " +
                                           "    ,@eg_inRefParametro2 " +
                                           "    ,@eg_prBienServicio " +
                                           "    ,@eg_faServicios " +
                                           "    ,@eg_inMovimientoTipo " +
                                           "    ,@eg_acMovimientoTipo " +
                                           "    ,@eg_faAsesor " +
                                           "    ,@eg_coProyecto " +
                                           "    ,@eg_coCentroCosto " +
                                           "    ,@eg_coTercero " +
                                           "    ,@eg_inEmpaque" +
                                           "    ,@eg_plLineaPresupuesto) ";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@EntradaSalida", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Kit", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@IdentificadorTr", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@ServicioID", SqlDbType.Char, UDT_ServicioID.MaxLength);
                mySqlCommand.Parameters.Add("@SerialID", SqlDbType.Char, UDT_SerialID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MvtoTipoInvID", SqlDbType.Char, UDT_MvtoTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@MvtoTipoActID", SqlDbType.Char, UDT_MvtoTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocSoporte", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocSoporteTER", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DescripTExt", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@EmpaqueInvID", SqlDbType.Char, UDT_EmpaqueInvID.MaxLength);
                mySqlCommand.Parameters.Add("@CantidadEMP", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadDoc", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadUNI", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorUNI", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor1LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor2LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor3LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor4LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor5LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor6LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor7LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor8LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor9LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor10LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor1EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor2EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor3EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor4EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor5EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor6EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor7EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor8EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor9EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor10EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@InReferenciaCodID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@CantidadDEV", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ConsecutivoPrestamo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ConsecutivoOrdCompra", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NroItem", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ImprimeInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@eg_inBodega", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inRefParametro1", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inRefParametro2", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_faServicios", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inMovimientoTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_acMovimientoTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_faAsesor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inEmpaque", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                foreach (DTO_glMovimientoDeta det in footer)
                {
                    #region Asignacion de valores
                    mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommand.Parameters["@NumeroDoc"].Value = det.NumeroDoc.Value;
                    mySqlCommand.Parameters["@BodegaID"].Value = det.BodegaID.Value;
                    mySqlCommand.Parameters["@EntradaSalida"].Value = det.EntradaSalida.Value;
                    mySqlCommand.Parameters["@Kit"].Value = det.Kit.Value;
                    mySqlCommand.Parameters["@inReferenciaID"].Value = det.inReferenciaID.Value;
                    mySqlCommand.Parameters["@EstadoInv"].Value = det.EstadoInv.Value;
                    mySqlCommand.Parameters["@Parametro1"].Value = det.Parametro1.Value;
                    mySqlCommand.Parameters["@Parametro2"].Value = det.Parametro2.Value;
                    mySqlCommand.Parameters["@IdentificadorTr"].Value = det.IdentificadorTr.Value;
                    mySqlCommand.Parameters["@CodigoBSID"].Value = det.CodigoBSID.Value;
                    mySqlCommand.Parameters["@ServicioID"].Value = det.ServicioID.Value;
                    mySqlCommand.Parameters["@SerialID"].Value = det.SerialID.Value;
                    mySqlCommand.Parameters["@ActivoID"].Value = det.ActivoID.Value ?? 0;
                    mySqlCommand.Parameters["@MvtoTipoInvID"].Value = det.MvtoTipoInvID.Value;
                    mySqlCommand.Parameters["@MvtoTipoActID"].Value = det.MvtoTipoActID.Value;
                    mySqlCommand.Parameters["@DocSoporte"].Value = det.DocSoporte.Value;
                    mySqlCommand.Parameters["@DocSoporteTER"].Value = det.DocSoporteTER.Value;
                    mySqlCommand.Parameters["@AsesorID"].Value = det.AsesorID.Value;
                    mySqlCommand.Parameters["@ProyectoID"].Value = det.ProyectoID.Value;
                    mySqlCommand.Parameters["@CentroCostoID"].Value = det.CentroCostoID.Value;
                    mySqlCommand.Parameters["@LineaPresupuestoID"].Value = det.LineaPresupuestoID.Value;
                    mySqlCommand.Parameters["@TerceroID"].Value = det.TerceroID.Value;
                    mySqlCommand.Parameters["@DatoAdd1"].Value = det.DatoAdd1.Value;
                    mySqlCommand.Parameters["@DatoAdd2"].Value = det.DatoAdd2.Value;
                    mySqlCommand.Parameters["@DatoAdd3"].Value = det.DatoAdd3.Value;
                    mySqlCommand.Parameters["@DatoAdd4"].Value = det.DatoAdd4.Value;
                    mySqlCommand.Parameters["@DatoAdd5"].Value = det.DatoAdd5.Value;
                    mySqlCommand.Parameters["@DescripTExt"].Value = det.DescripTExt.Value;
                    mySqlCommand.Parameters["@EmpaqueInvID"].Value = det.EmpaqueInvID.Value;
                    mySqlCommand.Parameters["@CantidadEMP"].Value = det.CantidadEMP.Value;
                    mySqlCommand.Parameters["@CantidadDoc"].Value = det.CantidadDoc.Value ?? 0;
                    mySqlCommand.Parameters["@CantidadUNI"].Value = det.CantidadUNI.Value ?? 0;
                    mySqlCommand.Parameters["@ValorUNI"].Value = det.ValorUNI.Value ?? 0;
                    mySqlCommand.Parameters["@Valor1LOC"].Value = det.Valor1LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor2LOC"].Value = det.Valor2LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor3LOC"].Value = det.Valor3LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor4LOC"].Value = det.Valor4LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor5LOC"].Value = det.Valor5LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor6LOC"].Value = det.Valor6LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor7LOC"].Value = det.Valor7LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor8LOC"].Value = det.Valor8LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor9LOC"].Value = det.Valor9LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor10LOC"].Value = det.Valor10LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor1EXT"].Value = det.Valor1EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor2EXT"].Value = det.Valor2EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor3EXT"].Value = det.Valor3EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor4EXT"].Value = det.Valor4EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor5EXT"].Value = det.Valor5EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor6EXT"].Value = det.Valor6EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor7EXT"].Value = det.Valor7EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor8EXT"].Value = det.Valor8EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor9EXT"].Value = det.Valor9EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor10EXT"].Value = det.Valor10EXT.Value ?? 0;
                    mySqlCommand.Parameters["@InReferenciaCodID"].Value = det.InReferenciaCodID.Value;
                    mySqlCommand.Parameters["@CantidadDEV"].Value = det.CantidadDEV.Value ?? 0;
                    mySqlCommand.Parameters["@ConsecutivoPrestamo"].Value = det.ConsecutivoPrestamo.Value;
                    mySqlCommand.Parameters["@ConsecutivoOrdCompra"].Value = det.ConsecutivoOrdCompra.Value;
                    mySqlCommand.Parameters["@NroItem"].Value = det.NroItem.Value ?? 0;
                    mySqlCommand.Parameters["@ImprimeInd"].Value = det.ImprimeInd.Value ?? true;                  
                    mySqlCommand.Parameters["@eg_inBodega"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inBodega, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_inRefParametro1"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inRefParametro1, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_inRefParametro2"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inRefParametro2, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_faServicios"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faServicios, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_inMovimientoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inMovimientoTipo, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_acMovimientoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acMovimientoTipo, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_faAsesor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faAsesor, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_inEmpaque"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inEmpaque, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);

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
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDeta_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona en tabla glMovimientoDeta
        /// </summary>
        /// <param name="footer">Movimientos</param>
        public int DAL_glMovimientoDeta_AddItem(DTO_glMovimientoDeta footer)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO glMovimientoDeta " +
                                           "    (EmpresaID    " +
                                           "    ,NumeroDoc    " +
                                           "    ,BodegaID " +
                                           "    ,EntradaSalida " +
                                           "    ,Kit " +
                                           "    ,inReferenciaID " +
                                           "    ,EstadoInv " +
                                           "    ,Parametro1 " +
                                           "    ,Parametro2 " +
                                           "    ,IdentificadorTr " +
                                           "    ,CodigoBSID " +
                                           "    ,ServicioID " +
                                           "    ,SerialID " +
                                           "    ,ActivoID " +
                                           "    ,MvtoTipoInvID " +
                                           "    ,MvtoTipoActID " +
                                           "    ,DocSoporte " +
                                           "    ,DocSoporteTER " +
                                           "    ,AsesorID " +
                                           "    ,ProyectoID " +
                                           "    ,CentroCostoID " +
                                           "    ,LineaPresupuestoID " +
                                           "    ,TerceroID " +
                                           "    ,DatoAdd1 " +
                                           "    ,DatoAdd2 " +
                                           "    ,DatoAdd3 " +
                                           "    ,DatoAdd4 " +
                                           "    ,DatoAdd5 " +
                                           "    ,DescripTExt " +
                                           "    ,EmpaqueInvID " +
                                           "    ,CantidadEMP " +
                                           "    ,CantidadDoc " +
                                           "    ,CantidadUNI " +
                                           "    ,ValorUNI " +
                                           "    ,Valor1LOC " +
                                           "    ,Valor2LOC " +
                                           "    ,Valor3LOC " +
                                           "    ,Valor4LOC " +
                                           "    ,Valor5LOC " +
                                           "    ,Valor6LOC " +
                                           "    ,Valor7LOC " +
                                           "    ,Valor8LOC " +
                                           "    ,Valor9LOC " +
                                           "    ,Valor10LOC " +
                                           "    ,Valor1EXT " +
                                           "    ,Valor2EXT " +
                                           "    ,Valor3EXT " +
                                           "    ,Valor4EXT " +
                                           "    ,Valor5EXT " +
                                           "    ,Valor6EXT " +
                                           "    ,Valor7EXT " +
                                           "    ,Valor8EXT " +
                                           "    ,Valor9EXT " +
                                           "    ,Valor10EXT " +
                                           "    ,InReferenciaCodID " +
                                           "    ,CantidadDEV" +
                                           "    ,ConsecutivoPrestamo" +
                                           "    ,ConsecutivoOrdCompra" +
                                           "    ,NroItem" +
                                           "    ,ImprimeInd" +
                                           "    ,eg_inBodega " +
                                           "    ,eg_inReferencia " +
                                           "    ,eg_inRefParametro1 " +
                                           "    ,eg_inRefParametro2 " +
                                           "    ,eg_prBienServicio " +
                                           "    ,eg_faServicios " +
                                           "    ,eg_inMovimientoTipo " +
                                           "    ,eg_acMovimientoTipo " +
                                           "    ,eg_faAsesor " +
                                           "    ,eg_coProyecto " +
                                           "    ,eg_coCentroCosto " +
                                           "    ,eg_coTercero " +
                                           "    ,eg_inEmpaque " +
                                           "    ,eg_plLineaPresupuesto) " +
                                           "    VALUES          " +
                                           "    (@EmpresaID     " +
                                           "    ,@NumeroDoc     " +
                                           "    ,@BodegaID " +
                                           "    ,@EntradaSalida " +
                                           "    ,@Kit " +
                                           "    ,@inReferenciaID " +
                                           "    ,@EstadoInv " +
                                           "    ,@Parametro1 " +
                                           "    ,@Parametro2 " +
                                           "    ,@IdentificadorTr " +
                                           "    ,@CodigoBSID " +
                                           "    ,@ServicioID " +
                                           "    ,@SerialID " +
                                           "    ,@ActivoID " +
                                           "    ,@MvtoTipoInvID " +
                                           "    ,@MvtoTipoActID " +
                                           "    ,@DocSoporte " +
                                           "    ,@DocSoporteTER " +
                                           "    ,@AsesorID " +
                                           "    ,@ProyectoID " +
                                           "    ,@CentroCostoID " +
                                           "    ,@LineaPresupuestoID " +
                                           "    ,@TerceroID " +
                                           "    ,@DatoAdd1 " +
                                           "    ,@DatoAdd2 " +
                                           "    ,@DatoAdd3 " +
                                           "    ,@DatoAdd4 " +
                                           "    ,@DatoAdd5 " +
                                           "    ,@DescripTExt " +
                                           "    ,@EmpaqueInvID " +
                                           "    ,@CantidadEMP " +
                                           "    ,@CantidadDoc " +
                                           "    ,@CantidadUNI " +
                                           "    ,@ValorUNI " +
                                           "    ,@Valor1LOC " +
                                           "    ,@Valor2LOC " +
                                           "    ,@Valor3LOC " +
                                           "    ,@Valor4LOC " +
                                           "    ,@Valor5LOC " +
                                           "    ,@Valor6LOC " +
                                           "    ,@Valor7LOC " +
                                           "    ,@Valor8LOC " +
                                           "    ,@Valor9LOC " +
                                           "    ,@Valor10LOC " +
                                           "    ,@Valor1EXT " +
                                           "    ,@Valor2EXT " +
                                           "    ,@Valor3EXT " +
                                           "    ,@Valor4EXT " +
                                           "    ,@Valor5EXT " +
                                           "    ,@Valor6EXT " +
                                           "    ,@Valor7EXT " +
                                           "    ,@Valor8EXT " +
                                           "    ,@Valor9EXT " +
                                           "    ,@Valor10EXT " +
                                           "    ,@InReferenciaCodID " +
                                           "    ,@CantidadDEV" +
                                           "    ,@ConsecutivoPrestamo" +
                                           "    ,@ConsecutivoOrdCompra" +
                                           "    ,@NroItem" +
                                           "    ,@ImprimeInd" +
                                           "    ,@eg_inBodega " +
                                           "    ,@eg_inReferencia " +
                                           "    ,@eg_inRefParametro1 " +
                                           "    ,@eg_inRefParametro2 " +
                                           "    ,@eg_prBienServicio " +
                                           "    ,@eg_faServicios " +
                                           "    ,@eg_inMovimientoTipo " +
                                           "    ,@eg_acMovimientoTipo " +
                                           "    ,@eg_faAsesor " +
                                           "    ,@eg_coProyecto " +
                                           "    ,@eg_coCentroCosto " +
                                           "    ,@eg_coTercero " +
                                           "    ,@eg_inEmpaque" +
                                           "    ,@eg_plLineaPresupuesto) "+
                                           "     SET @Consecutivo = SCOPE_IDENTITY()";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@EntradaSalida", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Kit", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@IdentificadorTr", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@ServicioID", SqlDbType.Char, UDT_ServicioID.MaxLength);
                mySqlCommand.Parameters.Add("@SerialID", SqlDbType.Char, UDT_SerialID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MvtoTipoInvID", SqlDbType.Char, UDT_MvtoTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@MvtoTipoActID", SqlDbType.Char, UDT_MvtoTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocSoporte", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocSoporteTER", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DescripTExt", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@EmpaqueInvID", SqlDbType.Char, UDT_EmpaqueInvID.MaxLength);
                mySqlCommand.Parameters.Add("@CantidadEMP", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadDoc", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadUNI", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorUNI", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor1LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor2LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor3LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor4LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor5LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor6LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor7LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor8LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor9LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor10LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor1EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor2EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor3EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor4EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor5EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor6EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor7EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor8EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor9EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor10EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@InReferenciaCodID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@CantidadDEV", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ConsecutivoPrestamo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ConsecutivoOrdCompra", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NroItem", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ImprimeInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@eg_inBodega", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inRefParametro1", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inRefParametro2", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_faServicios", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inMovimientoTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_acMovimientoTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_faAsesor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inEmpaque", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int, 1);
                mySqlCommand.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = footer.NumeroDoc.Value;
                mySqlCommand.Parameters["@BodegaID"].Value = footer.BodegaID.Value;
                mySqlCommand.Parameters["@EntradaSalida"].Value = footer.EntradaSalida.Value;
                mySqlCommand.Parameters["@Kit"].Value = footer.Kit.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = footer.inReferenciaID.Value;
                mySqlCommand.Parameters["@EstadoInv"].Value = footer.EstadoInv.Value;
                mySqlCommand.Parameters["@Parametro1"].Value = footer.Parametro1.Value;
                mySqlCommand.Parameters["@Parametro2"].Value = footer.Parametro2.Value;
                mySqlCommand.Parameters["@IdentificadorTr"].Value = footer.IdentificadorTr.Value;
                mySqlCommand.Parameters["@CodigoBSID"].Value = footer.CodigoBSID.Value;
                mySqlCommand.Parameters["@ServicioID"].Value = footer.ServicioID.Value;
                mySqlCommand.Parameters["@SerialID"].Value = footer.SerialID.Value;
                mySqlCommand.Parameters["@ActivoID"].Value = footer.ActivoID.Value ?? 0;
                mySqlCommand.Parameters["@MvtoTipoInvID"].Value = footer.MvtoTipoInvID.Value;
                mySqlCommand.Parameters["@MvtoTipoActID"].Value = footer.MvtoTipoActID.Value;
                mySqlCommand.Parameters["@DocSoporte"].Value = footer.DocSoporte.Value;
                mySqlCommand.Parameters["@DocSoporteTER"].Value = footer.DocSoporteTER.Value;
                mySqlCommand.Parameters["@AsesorID"].Value = footer.AsesorID.Value;
                mySqlCommand.Parameters["@ProyectoID"].Value = footer.ProyectoID.Value;
                mySqlCommand.Parameters["@CentroCostoID"].Value = footer.CentroCostoID.Value;
                mySqlCommand.Parameters["@LineaPresupuestoID"].Value = footer.LineaPresupuestoID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = footer.TerceroID.Value;
                mySqlCommand.Parameters["@DatoAdd1"].Value = footer.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd2"].Value = footer.DatoAdd2.Value;
                mySqlCommand.Parameters["@DatoAdd3"].Value = footer.DatoAdd3.Value;
                mySqlCommand.Parameters["@DatoAdd4"].Value = footer.DatoAdd4.Value;
                mySqlCommand.Parameters["@DatoAdd5"].Value = footer.DatoAdd5.Value;
                mySqlCommand.Parameters["@DescripTExt"].Value = footer.DescripTExt.Value;
                mySqlCommand.Parameters["@EmpaqueInvID"].Value = footer.EmpaqueInvID.Value;
                mySqlCommand.Parameters["@CantidadEMP"].Value = footer.CantidadEMP.Value;
                mySqlCommand.Parameters["@CantidadDoc"].Value = footer.CantidadDoc.Value ?? 0;
                mySqlCommand.Parameters["@CantidadUNI"].Value = footer.CantidadUNI.Value ?? 0;
                mySqlCommand.Parameters["@ValorUNI"].Value = footer.ValorUNI.Value ?? 0;
                mySqlCommand.Parameters["@Valor1LOC"].Value = footer.Valor1LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor2LOC"].Value = footer.Valor2LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor3LOC"].Value = footer.Valor3LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor4LOC"].Value = footer.Valor4LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor5LOC"].Value = footer.Valor5LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor6LOC"].Value = footer.Valor6LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor7LOC"].Value = footer.Valor7LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor8LOC"].Value = footer.Valor8LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor9LOC"].Value = footer.Valor9LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor10LOC"].Value = footer.Valor10LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor1EXT"].Value = footer.Valor1EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor2EXT"].Value = footer.Valor2EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor3EXT"].Value = footer.Valor3EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor4EXT"].Value = footer.Valor4EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor5EXT"].Value = footer.Valor5EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor6EXT"].Value = footer.Valor6EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor7EXT"].Value = footer.Valor7EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor8EXT"].Value = footer.Valor8EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor9EXT"].Value = footer.Valor9EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor10EXT"].Value = footer.Valor10EXT.Value ?? 0;
                mySqlCommand.Parameters["@eg_inBodega"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inBodega, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inRefParametro1"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inRefParametro1, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inRefParametro2"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inRefParametro2, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_faServicios"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faServicios, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inMovimientoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inMovimientoTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_acMovimientoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acMovimientoTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_faAsesor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faAsesor, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inEmpaque"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inEmpaque, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDeta_AddItem");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina registros de la tabla de glMovimientoDeta
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public void DAL_glMovimientoDeta_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText = "DELETE FROM glMovimientoDeta where EmpresaID = @EmpresaID " +
                " and NumeroDoc = @NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDeta_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Edita un registro de recibidos
        /// </summary>
        /// <param name="docCtrl">Documento que se va a editar</param>
        public void DAL_glMovimientoDeta_Update(DTO_glMovimientoDeta deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "UPDATE glMovimientoDeta SET " +
                                           "    NumeroDoc = @NumeroDoc " +
                                           "    ,BodegaID = @BodegaID " +
                                           "    ,EntradaSalida = @EntradaSalida " +
                                           "    ,Kit = @Kit " +
                                           "    ,inReferenciaID = @inReferenciaID " +
                                           "    ,EstadoInv = @EstadoInv " +
                                           "    ,Parametro1  = @Parametro1 " +
                                           "    ,Parametro2  = @Parametro2 " +
                                           "    ,IdentificadorTr = @IdentificadorTr " +
                                           "    ,CodigoBSID = @CodigoBSID " +
                                           "    ,ServicioID  = @ServicioID " +
                                           "    ,SerialID  = @SerialID " +
                                           "    ,ActivoID = @ActivoID " +
                                           "    ,MvtoTipoInvID = @MvtoTipoInvID " +
                                           "    ,MvtoTipoActID  = @MvtoTipoActID " +
                                           "    ,DocSoporte = @DocSoporte " +
                                           "    ,DocSoporteTER = @DocSoporteTER " +
                                           "    ,AsesorID  = @AsesorID " +
                                           "    ,ProyectoID  = @ProyectoID " +
                                           "    ,CentroCostoID  = @CentroCostoID " +
                                           "    ,LineaPresupuestoID  = @LineaPresupuestoID " +
                                           "    ,TerceroID  = @TerceroID " +
                                           "    ,DatoAdd1  = @DatoAdd1 " +
                                           "    ,DatoAdd2  = @DatoAdd2 " +
                                           "    ,DatoAdd3  = @DatoAdd3 " +
                                           "    ,DatoAdd4  = @DatoAdd4 " +
                                           "    ,DatoAdd5  = @DatoAdd5 " +
                                           "    ,DescripTExt  = @DescripTExt " +
                                           "    ,EmpaqueInvID  = @EmpaqueInvID " +
                                           "    ,CantidadEMP = @CantidadEMP " +
                                           "    ,CantidadDoc = @CantidadDoc " +
                                           "    ,CantidadUNI  = @CantidadUNI " +
                                           "    ,ValorUNI  = @ValorUNI " +
                                           "    ,Valor1LOC  = @Valor1LOC " +
                                           "    ,Valor2LOC = @Valor2LOC " +
                                           "    ,Valor3LOC  = @Valor3LOC " +
                                           "    ,Valor4LOC  = @Valor4LOC " +
                                           "    ,Valor5LOC = @Valor5LOC " +
                                           "    ,Valor6LOC = @Valor6LOC " +
                                           "    ,Valor7LOC = @Valor7LOC " +
                                           "    ,Valor8LOC  = @Valor8LOC " +
                                           "    ,Valor9LOC = @Valor9LOC " +
                                           "    ,Valor10LOC  = @Valor10LOC " +
                                           "    ,Valor1EXT  = @Valor1EXT " +
                                           "    ,Valor2EXT  = @Valor2EXT " +
                                           "    ,Valor3EXT  = @Valor3EXT " +
                                           "    ,Valor4EXT  = @Valor4EXT " +
                                           "    ,Valor5EXT  = @Valor5EXT " +
                                           "    ,Valor6EXT  = @Valor6EXT " +
                                           "    ,Valor7EXT  = @Valor7EXT " +
                                           "    ,Valor8EXT  = @Valor8EXT " +
                                           "    ,Valor9EXT  = @Valor9EXT " +
                                           "    ,Valor10EXT  = @Valor10EXT " +
                                           "    ,InReferenciaCodID  = @InReferenciaCodID " +
                                           "    ,CantidadDEV  = @CantidadDEV" +
                                           "    ,ConsecutivoPrestamo  = @ConsecutivoPrestamo " +
                                           "    ,ConsecutivoOrdCompra  = @ConsecutivoOrdCompra" +
                                           "    ,NroItem  = @NroItem " +
                                           "    ,ImprimeInd  = @ImprimeInd " +
                                           " WHERE Consecutivo = @Consecutivo "; 
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@EntradaSalida", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Kit", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@IdentificadorTr", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@ServicioID", SqlDbType.Char, UDT_ServicioID.MaxLength);
                mySqlCommand.Parameters.Add("@SerialID", SqlDbType.Char, UDT_SerialID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MvtoTipoInvID", SqlDbType.Char, UDT_MvtoTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@MvtoTipoActID", SqlDbType.Char, UDT_MvtoTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocSoporte", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocSoporteTER", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DescripTExt", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@EmpaqueInvID", SqlDbType.Char, UDT_EmpaqueInvID.MaxLength);
                mySqlCommand.Parameters.Add("@CantidadEMP", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadDoc", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadUNI", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorUNI", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor1LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor2LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor3LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor4LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor5LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor6LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor7LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor8LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor9LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor10LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor1EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor2EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor3EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor4EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor5EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor6EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor7EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor8EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor9EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor10EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@InReferenciaCodID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@CantidadDEV", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ConsecutivoPrestamo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ConsecutivoOrdCompra", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NroItem", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ImprimeInd", SqlDbType.Bit);
                #endregion
                #region Asignacion de valores
                    mySqlCommand.Parameters["@NumeroDoc"].Value = deta.NumeroDoc.Value;
                    mySqlCommand.Parameters["@Consecutivo"].Value = deta.Consecutivo.Value;
                    mySqlCommand.Parameters["@BodegaID"].Value = deta.BodegaID.Value;
                    mySqlCommand.Parameters["@EntradaSalida"].Value = deta.EntradaSalida.Value;
                    mySqlCommand.Parameters["@Kit"].Value = deta.Kit.Value;
                    mySqlCommand.Parameters["@inReferenciaID"].Value = deta.inReferenciaID.Value;
                    mySqlCommand.Parameters["@EstadoInv"].Value = deta.EstadoInv.Value;
                    mySqlCommand.Parameters["@Parametro1"].Value = deta.Parametro1.Value;
                    mySqlCommand.Parameters["@Parametro2"].Value = deta.Parametro2.Value;
                    mySqlCommand.Parameters["@IdentificadorTr"].Value = deta.IdentificadorTr.Value;
                    mySqlCommand.Parameters["@CodigoBSID"].Value = deta.CodigoBSID.Value;
                    mySqlCommand.Parameters["@ServicioID"].Value = deta.ServicioID.Value;
                    mySqlCommand.Parameters["@SerialID"].Value = deta.SerialID.Value;
                    mySqlCommand.Parameters["@ActivoID"].Value = deta.ActivoID.Value ?? 0;
                    mySqlCommand.Parameters["@MvtoTipoInvID"].Value = deta.MvtoTipoInvID.Value;
                    mySqlCommand.Parameters["@MvtoTipoActID"].Value = deta.MvtoTipoActID.Value;
                    mySqlCommand.Parameters["@DocSoporte"].Value = deta.DocSoporte.Value;
                    mySqlCommand.Parameters["@DocSoporteTER"].Value = deta.DocSoporteTER.Value;
                    mySqlCommand.Parameters["@AsesorID"].Value = deta.AsesorID.Value;
                    mySqlCommand.Parameters["@ProyectoID"].Value = deta.ProyectoID.Value;
                    mySqlCommand.Parameters["@CentroCostoID"].Value = deta.CentroCostoID.Value;
                    mySqlCommand.Parameters["@LineaPresupuestoID"].Value = deta.LineaPresupuestoID.Value;
                    mySqlCommand.Parameters["@TerceroID"].Value = deta.TerceroID.Value;
                    mySqlCommand.Parameters["@DatoAdd1"].Value = deta.DatoAdd1.Value;
                    mySqlCommand.Parameters["@DatoAdd2"].Value = deta.DatoAdd2.Value;
                    mySqlCommand.Parameters["@DatoAdd3"].Value = deta.DatoAdd3.Value;
                    mySqlCommand.Parameters["@DatoAdd4"].Value = deta.DatoAdd4.Value;
                    mySqlCommand.Parameters["@DatoAdd5"].Value = deta.DatoAdd5.Value;
                    mySqlCommand.Parameters["@DescripTExt"].Value = deta.DescripTExt.Value;
                    mySqlCommand.Parameters["@EmpaqueInvID"].Value = deta.EmpaqueInvID.Value;
                    mySqlCommand.Parameters["@CantidadEMP"].Value = deta.CantidadEMP.Value;
                    mySqlCommand.Parameters["@CantidadDoc"].Value = deta.CantidadDoc.Value ?? 0;
                    mySqlCommand.Parameters["@CantidadUNI"].Value = deta.CantidadUNI.Value ?? 0;
                    mySqlCommand.Parameters["@ValorUNI"].Value = deta.ValorUNI.Value ?? 0;
                    mySqlCommand.Parameters["@Valor1LOC"].Value = deta.Valor1LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor2LOC"].Value = deta.Valor2LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor3LOC"].Value = deta.Valor3LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor4LOC"].Value = deta.Valor4LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor5LOC"].Value = deta.Valor5LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor6LOC"].Value = deta.Valor6LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor7LOC"].Value = deta.Valor7LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor8LOC"].Value = deta.Valor8LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor9LOC"].Value = deta.Valor9LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor10LOC"].Value = deta.Valor10LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor1EXT"].Value = deta.Valor1EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor2EXT"].Value = deta.Valor2EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor3EXT"].Value = deta.Valor3EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor4EXT"].Value = deta.Valor4EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor5EXT"].Value = deta.Valor5EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor6EXT"].Value = deta.Valor6EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor7EXT"].Value = deta.Valor7EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor8EXT"].Value = deta.Valor8EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor9EXT"].Value = deta.Valor9EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor10EXT"].Value = deta.Valor10EXT.Value ?? 0;
                    mySqlCommand.Parameters["@InReferenciaCodID"].Value = deta.InReferenciaCodID.Value;
                    mySqlCommand.Parameters["@CantidadDEV"].Value = deta.CantidadDEV.Value ?? 0;
                    mySqlCommand.Parameters["@ConsecutivoPrestamo"].Value = deta.ConsecutivoPrestamo.Value;
                    mySqlCommand.Parameters["@ConsecutivoOrdCompra"].Value = deta.ConsecutivoOrdCompra.Value;
                    mySqlCommand.Parameters["@NroItem"].Value = deta.NroItem.Value ?? 0;
                    mySqlCommand.Parameters["@ImprimeInd"].Value = deta.ImprimeInd.Value ?? true;        
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDeta_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene la cantidad de registros de la tabla glMovimientoDeta
        /// </summary>
        /// <param name="consulta">Filtros</param>
        /// <returns>Retorna la cantidad de registros de la consulta</returns>
        public long DAL_glMovimientoDeta_Count(DTO_glConsulta consulta, string where)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                where += string.IsNullOrEmpty(where) ? " where EmpresaID = @EmpresaID " : " and EmpresaID = @EmpresaID ";
                mySqlCommand.CommandText = "SELECT COUNT(*) FROM glMovimientoDeta " + where + "";

                long res = Convert.ToInt64(mySqlCommand.ExecuteScalar());
                return res;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDeta_Count");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene una lista de registros de la tabla glMovimientoDeta
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de la pagina de consulta</param>
        /// <param name="consulta">Filtros</param>
        /// <returns>Retorna los registros de la consulta</returns>
        public List<DTO_glMovimientoDeta> DAL_glMovimientoDeta_GetPaged(int pageSize, int pageNum, int ini, int fin,DTO_glConsulta consulta, string where)
        {
            try
            {
                List<DTO_glMovimientoDeta> dtos = new List<DTO_glMovimientoDeta>();
                string andActivo = string.Empty;

                string tablesFKs = string.Empty;
                string descFields = string.Empty;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = string.Empty;

                if (!string.IsNullOrWhiteSpace(where))
                    query = "SELECT * FROM (" + query + ") resultTable (" + where + ") ";

                string orderby = "NumeroDoc";

                if (consulta != null && consulta.Selecciones != null)
                {
                    List<DTO_glConsultaSeleccion> seleccionestmp = new List<DTO_glConsultaSeleccion>(consulta.Selecciones);
                    seleccionestmp = seleccionestmp.Where(x => (x.OrdenIdx > 0 && (x.OrdenTipo.Equals("ASC") || x.OrdenTipo.Equals("DESC")))).OrderBy(x => x.OrdenIdx).ToList();
                    if (seleccionestmp.Count > 0)
                        orderby = string.Empty;
                    foreach (DTO_glConsultaSeleccion sel in seleccionestmp)
                    {
                        string campoSel = sel.CampoFisico;
                        if (campoSel.Equals("ID"))
                            campoSel = "NumeroDoc";
                        if (!string.IsNullOrWhiteSpace(orderby))
                            orderby += ",";
                        orderby += campoSel + " " + sel.OrdenTipo;
                    }
                }

                query = "SELECT * FROM ( " +
                        "SELECT ROW_NUMBER()Over(Order by " + orderby + ") As RowNum,* FROM glMovimientoDeta baseTable " + where + " ) AS ResultadoPaginado  " +
                        "WHERE RowNum BETWEEN " + ini + " AND " + fin;
                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                int index = 0;
                while (dr.Read())
                {
                    DTO_glMovimientoDeta detail = new DTO_glMovimientoDeta(dr);
                    detail.Index = index;
                    dtos.Add(detail);
                    index++;
                }
                dr.Close();
                return dtos;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDeta_GetPaged");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Funcion que trae del glMovimientoDeta la informacion de un activo
        /// </summary>
        /// <param name="activoID">El numero del Activo</param>
        /// <returns>Lista de Dto glemovimientDeta</returns>
        public List<DTO_glMovimientoDeta> DAL_glMovimientoDeta_GetBy_ActivoFind(int activoID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@activoID", SqlDbType.Int);
                mySqlCommand.Parameters["@activoID"].Value = activoID;

                mySqlCommand.CommandText =
                    " SELECT  " +
                            " glDetalle.MvtoTipoActID, " +
                            " acMvto.Descriptivo, " +
                            " glDetalle.ProyectoID, " +
                            " glDetalle.CentroCostoID, " +
                            " glDetalle.DescripTExt, " +
                            " glDetalle.SerialID, " +
                            " gldetalle.datoAdd2, "+
                            " RTRIM(Cast(glControl.PrefijoID as nvarchar)) + '-' + cast(glcontrol.DocumentoNro as nvarchar) as Prefijo_Documento,  " +
                            " glcontrol.DocumentoNro, " +
                            " CONVERT(VARCHAR,glcontrol.Fecha,103)as Fecha, "  +
                            " activoControl.PlaquetaID, " +
                            " glDetalle.Valor1LOC, "+
                            " glDetalle.Valor2LOC, "+
                            " glDetalle.Valor1EXT, "+
                            " glDetalle.Valor2EXT, "+ 
                            " gldetalle.NumeroDoc, " +
                            " gldetalle.IdentificadorTr " +
                    "FROM glMovimientoDeta glDetalle  with(nolock) " +
                    "INNER JOIN glDocumentoControl as glControl  with(nolock) on glControl.NumeroDoc = glDetalle.NumeroDoc " +
                    "INNER JOIN acMovimientoTipo as acMvto  with(nolock) on glDetalle.MvtoTipoActID = acMvto.MvtoTipoActID " +
                    "INNER JOIN acActivoControl as activoControl  with(nolock) on activoControl.ActivoID = glDetalle.ActivoID " +
                    "WHERE activoControl.ActivoID = @activoID ORDER BY gldetalle.NumeroDoc asc";

                List<DTO_glMovimientoDeta> detalle = new List<DTO_glMovimientoDeta>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_glMovimientoDeta detail = new DTO_glMovimientoDeta(dr, true);
                    detail.Index = index;
                    detalle.Add(detail);
                    index++;
                }
                dr.Close();
                return detalle;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDeta_GetBy_ActivoFind");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Mvto detalle</returns>
        public List<DTO_glMovimientoDeta> DAL_glMovimientoDeta_GetByParameter(DTO_glMovimientoDeta filter,bool isPre,bool onlyInventario)
        {
            try
            {
                List<DTO_glMovimientoDeta> result = new List<DTO_glMovimientoDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string tabla = isPre ? " glMovimientoDetaPRE " : " glMovimientoDeta ";
                string query = string.Empty;
                bool filterInd = false;

                if (onlyInventario)
                {
                    query = " Select mvto.*,ctrl.PrefijoID,ctrl.Estado, ctrl.DocumentoNro,ctrl.FechaDoc as Fecha,ctrl.DocumentoID, refer.RefProveedor, " +
                            "  marca.Descriptivo as MarcaDesc,docuFact.FacturaTipoID,docuInv.TipoTraslado as TipoTrasladoInv " +
                            "  from " + tabla + " mvto with(nolock) " +
                            " Inner join glDocumentoControl as ctrl  with(nolock) on ctrl.NumeroDoc = mvto.NumeroDoc " +
                            " left join  glDocumento     doM with(nolock) on ctrl.Documentoid = doM.DocumentoID" +
                            " Left join faFacturaDocu as docuFact  with(nolock) on docuFact.NumeroDoc = ctrl.NumeroDoc " +
                            " Left join inMovimientoDocu as docuInv  with(nolock) on docuInv.NumeroDoc = ctrl.NumeroDoc " +
                            " Left join inReferencia as refer  with(nolock) on refer.inReferenciaID = mvto.inReferenciaID " +
                            " Left join inMarca as marca  with(nolock) on marca.MarcaInvID = refer.MarcaInvID " +
                            " Where ctrl.EmpresaID = @EmpresaID and ctrl.Estado in(3,4) and doM.ModuloID = 'in' ";
                }
                else
                {
                    query = " Select mvto.*,ctrl.PrefijoID,ctrl.Estado, ctrl.DocumentoNro,ctrl.FechaDoc as Fecha,ctrl.DocumentoID, refer.RefProveedor, " +
                            "  marca.Descriptivo as MarcaDesc,docuFact.FacturaTipoID,docuInv.TipoTraslado as TipoTrasladoInv " +
                            "  from " + tabla + " mvto with(nolock) " +
                            " Inner join glDocumentoControl as ctrl  with(nolock) on ctrl.NumeroDoc = mvto.NumeroDoc " +
                            " Left join faFacturaDocu as docuFact  with(nolock) on docuFact.NumeroDoc = ctrl.NumeroDoc " +
                            " Left join inMovimientoDocu as docuInv  with(nolock) on docuInv.NumeroDoc = ctrl.NumeroDoc " +
                            " Left join inReferencia as refer  with(nolock) on refer.inReferenciaID = mvto.inReferenciaID " +
                            " Left join inMarca as marca  with(nolock) on marca.MarcaInvID = refer.MarcaInvID " +
                            " Where ctrl.EmpresaID = @EmpresaID and ctrl.Estado not in(0,4) ";
                }


                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
           
                if (!string.IsNullOrEmpty(filter.NumeroDoc.Value.ToString()))
                {
                    query += "and mvto.NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = filter.NumeroDoc.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.PrefijoID.Value.ToString()))
                {
                    query += "and ctrl.PrefijoID = @PrefijoID ";
                    mySqlCommand.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                    mySqlCommand.Parameters["@PrefijoID"].Value = filter.PrefijoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DocumentoNro.Value.ToString()))
                {
                    query += "and ctrl.DocumentoNro = @DocumentoNro ";
                    mySqlCommand.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocumentoNro"].Value = filter.DocumentoNro.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DocumentoID.Value.ToString()))
                {
                    query += "and ctrl.DocumentoID = @DocumentoID ";
                    mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocumentoID"].Value = filter.DocumentoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.BodegaID.Value))
                {
                    query += "and mvto.BodegaID = @BodegaID ";
                    mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                    mySqlCommand.Parameters["@BodegaID"].Value = filter.BodegaID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.inReferenciaID.Value))
                {
                    query += "and mvto.inReferenciaID = @inReferenciaID ";
                    mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_ReferenciaID.MaxLength);
                    mySqlCommand.Parameters["@inReferenciaID"].Value = filter.inReferenciaID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.RefProveedor.Value))
                {
                    query += "and refer.RefProveedor = @RefProveedor ";
                    mySqlCommand.Parameters.Add("@RefProveedor", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                    mySqlCommand.Parameters["@RefProveedor"].Value = filter.RefProveedor.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.MarcaInvID.Value))
                {
                    query += "and refer.MarcaInvID = @MarcaInvID ";
                    mySqlCommand.Parameters.Add("@MarcaInvID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                    mySqlCommand.Parameters["@MarcaInvID"].Value = filter.MarcaInvID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.TipoTrasladoInv.Value.ToString()))
                {
                    query += "and docuInv.TipoTraslado = @TipoTraslado ";
                    mySqlCommand.Parameters.Add("@TipoTraslado", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@TipoTraslado"].Value = filter.TipoTrasladoInv.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ActivoID.Value.ToString()))
                {
                    query += "and mvto.ActivoID = @ActivoID ";
                    mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                    mySqlCommand.Parameters["@ActivoID"].Value = filter.ActivoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.EstadoInv.Value.ToString()))
                {
                    query += "and mvto.EstadoInv = @EstadoInv ";
                    mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@EstadoInv"].Value = filter.EstadoInv.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Parametro1.Value.ToString()))
                {
                    query += "and mvto.Parametro1 = @Parametro1 ";
                    mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                    mySqlCommand.Parameters["@Parametro1"].Value = filter.Parametro1.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Parametro2.Value.ToString()))
                {
                    query += "and mvto.Parametro2 = @Parametro2 ";
                    mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                    mySqlCommand.Parameters["@Parametro2"].Value = filter.Parametro2.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.EntradaSalida.Value.ToString()))
                {
                    query += "and mvto.EntradaSalida = @EntradaSalida ";
                    mySqlCommand.Parameters.Add("@EntradaSalida", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                    mySqlCommand.Parameters["@EntradaSalida"].Value = filter.EntradaSalida.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DocSoporte.Value.ToString()))
                {
                    query += "and mvto.DocSoporte = @DocSoporte ";
                    mySqlCommand.Parameters.Add("@DocSoporte", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocSoporte"].Value = filter.DocSoporte.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.MvtoTipoInvID.Value.ToString()))
                {
                    query += "and mvto.MvtoTipoInvID = @MvtoTipoInvID ";
                    mySqlCommand.Parameters.Add("@MvtoTipoInvID", SqlDbType.Char, 10);
                    mySqlCommand.Parameters["@MvtoTipoInvID"].Value = filter.MvtoTipoInvID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ProyectoID.Value.ToString()))
                {
                    query += "and mvto.ProyectoID = @ProyectoID ";
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@ProyectoID"].Value = filter.ProyectoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.CentroCostoID.Value.ToString()))
                {
                    query += "and mvto.CentroCostoID = @CentroCostoID ";
                    mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommand.Parameters["@CentroCostoID"].Value = filter.CentroCostoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DatoAdd1.Value.ToString()))
                {
                    query += "and mvto.DatoAdd1 = @DatoAdd1 ";
                    mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                    mySqlCommand.Parameters["@DatoAdd1"].Value = filter.DatoAdd1.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DatoAdd2.Value.ToString()))
                {
                    query += "and mvto.DatoAdd2 = @DatoAdd2 ";
                    mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                    mySqlCommand.Parameters["@DatoAdd2"].Value = filter.DatoAdd2.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DatoAdd3.Value.ToString()))
                {
                    query += "and mvto.DatoAdd3 = @DatoAdd3 ";
                    mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                    mySqlCommand.Parameters["@DatoAdd3"].Value = filter.DatoAdd3.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DatoAdd4.Value.ToString()))
                {
                    query += "and mvto.DatoAdd4 = @DatoAdd4 ";
                    mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                    mySqlCommand.Parameters["@DatoAdd4"].Value = filter.DatoAdd4.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DatoAdd5.Value.ToString()))
                {
                    query += "and mvto.DatoAdd5 = @DatoAdd5 ";
                    mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                    mySqlCommand.Parameters["@DatoAdd5"].Value = filter.DatoAdd5.Value;
                    filterInd = true;
                }
                mySqlCommand.CommandText = query;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                int index = 0;

                while (dr.Read())
                {
                    DTO_glMovimientoDeta mvto = new DTO_glMovimientoDeta(dr);
                    mvto.PrefijoID.Value = dr["PrefijoID"].ToString();
                    mvto.RefProveedor.Value = dr["RefProveedor"].ToString();
                    mvto.MarcaDesc.Value = dr["MarcaDesc"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["DocumentoNro"].ToString()))
                        mvto.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                    if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                        mvto.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                    if (!string.IsNullOrWhiteSpace(dr["DocumentoID"].ToString()))
                        mvto.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                    if (!string.IsNullOrWhiteSpace(dr["Estado"].ToString()))
                        mvto.EstadoDocCtrl.Value = Convert.ToInt16(dr["Estado"]);
                    mvto.FacturaTipoID.Value = dr["FacturaTipoID"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["TipoTrasladoInv"].ToString()))
                        mvto.TipoTrasladoInv.Value = Convert.ToByte(dr["TipoTrasladoInv"]);
                    mvto.Index = index;
                    result.Add(mvto);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDeta_GetByParameter");
                throw exception;
            }
        }

        public DataTable DAL_glMovimientoDeta_GetByParameterExcel(DTO_glMovimientoDeta filter, bool isPre, bool onlyInventario = false)
        {
            try
            {

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                SqlDataAdapter sda = new SqlDataAdapter();
                DataTable table = new DataTable();

                # region crud
                string tabla = isPre ? " glMovimientoDetaPRE " : " glMovimientoDeta ";
                string query = string.Empty;
                bool filterInd = false;

               

                if (onlyInventario)
                {
                    query = " Select Cast(RTrim(ctrl.PrefijoID)+'-'+Convert(Varchar, ctrl.DocumentoNro)  as Varchar(100)) as Prefijo_Documento," +
                            " mvto.BodegaID,ctrl.FechaDoc as Fecha,mvto.MvtoTipoInvID,Case When mvto.EntradaSalida = 1 Then 'E' Else 'S' End EntradaSalida," +
                            " mvto.inReferenciaID,RTRIM(mvto.DescripTExt) as DescripTExt,mvto.CantidadUNI, " +
                            " mvto.ValorUNI,mvto.Valor1LOC,mvto.Valor1EXT,refer.RefProveedor, marca.Descriptivo as MarcaDesc,mvto.ProyectoID," +
                            " mvto.CentroCostoID,mvto.SerialID,ctrl.PrefijoID,ctrl.Estado, ctrl.DocumentoNro,ctrl.DocumentoID,  " +
                            " docuFact.FacturaTipoID,docuInv.TipoTraslado as TipoTrasladoInv " +
                            "  from " + tabla + " mvto with(nolock) " +
                            " Inner join glDocumentoControl as ctrl  with(nolock) on ctrl.NumeroDoc = mvto.NumeroDoc " +
                            " left join  glDocumento     doM with(nolock) on ctrl.Documentoid = doM.DocumentoID" +
                            " Left join faFacturaDocu as docuFact  with(nolock) on docuFact.NumeroDoc = ctrl.NumeroDoc " +
                            " Left join inMovimientoDocu as docuInv  with(nolock) on docuInv.NumeroDoc = ctrl.NumeroDoc " +
                            " Left join inReferencia as refer  with(nolock) on refer.inReferenciaID = mvto.inReferenciaID " +
                            " Left join inMarca as marca  with(nolock) on marca.MarcaInvID = refer.MarcaInvID " +
                            " Where ctrl.EmpresaID = @EmpresaID and ctrl.Estado in(3,4) and doM.ModuloID = 'in' and ctrl.DocumentoID in(51,52,53,54,55,56,57,58,59) ";
                }
                else
                {
                    query = " Select Cast(RTrim(ctrl.PrefijoID)+'-'+Convert(Varchar, ctrl.DocumentoNro)  as Varchar(100)) as Prefijo_Documento," +
                            " mvto.BodegaID,ctrl.FechaDoc as Fecha,mvto.MvtoTipoInvID,Case When mvto.EntradaSalida = 1 Then 'E' Else 'S' End EntradaSalida," +
                            " mvto.inReferenciaID,RTRIM(mvto.DescripTExt) as DescripTExt,mvto.CantidadUNI, " +
                            " mvto.ValorUNI,mvto.Valor1LOC,mvto.Valor1EXT,refer.RefProveedor, marca.Descriptivo as MarcaDesc,mvto.ProyectoID," +
                            " mvto.CentroCostoID,mvto.SerialID,ctrl.PrefijoID,ctrl.Estado, ctrl.DocumentoNro,ctrl.DocumentoID,  " +
                            " docuFact.FacturaTipoID,docuInv.TipoTraslado as TipoTrasladoInv " +
                            "  from " + tabla + " mvto with(nolock) " +
                            " Inner join glDocumentoControl as ctrl  with(nolock) on ctrl.NumeroDoc = mvto.NumeroDoc " +
                            " Left join faFacturaDocu as docuFact  with(nolock) on docuFact.NumeroDoc = ctrl.NumeroDoc " +
                            " Left join inMovimientoDocu as docuInv  with(nolock) on docuInv.NumeroDoc = ctrl.NumeroDoc " +
                            " Left join inReferencia as refer  with(nolock) on refer.inReferenciaID = mvto.inReferenciaID " +
                            " Left join inMarca as marca  with(nolock) on marca.MarcaInvID = refer.MarcaInvID " +
                            " Where ctrl.EmpresaID = @EmpresaID and ctrl.Estado not in(0,4)  and ctrl.DocumentoID in(51,52,53,54,55,56,57,58,59) ";
                }

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if (!string.IsNullOrEmpty(filter.NumeroDoc.Value.ToString()))
                {
                    query += "and mvto.NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = filter.NumeroDoc.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.PrefijoID.Value.ToString()))
                {
                    query += "and ctrl.PrefijoID = @PrefijoID ";
                    mySqlCommand.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                    mySqlCommand.Parameters["@PrefijoID"].Value = filter.PrefijoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DocumentoNro.Value.ToString()))
                {
                    query += "and ctrl.DocumentoNro = @DocumentoNro ";
                    mySqlCommand.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocumentoNro"].Value = filter.DocumentoNro.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DocumentoID.Value.ToString()))
                {
                    query += "and ctrl.DocumentoID = @DocumentoID ";
                    mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocumentoID"].Value = filter.DocumentoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.BodegaID.Value))
                {
                    query += "and mvto.BodegaID = @BodegaID ";
                    mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                    mySqlCommand.Parameters["@BodegaID"].Value = filter.BodegaID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.inReferenciaID.Value))
                {
                    query += "and mvto.inReferenciaID = @inReferenciaID ";
                    mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_ReferenciaID.MaxLength);
                    mySqlCommand.Parameters["@inReferenciaID"].Value = filter.inReferenciaID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.RefProveedor.Value))
                {
                    query += "and refer.RefProveedor = @RefProveedor ";
                    mySqlCommand.Parameters.Add("@RefProveedor", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                    mySqlCommand.Parameters["@RefProveedor"].Value = filter.RefProveedor.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.MarcaInvID.Value))
                {
                    query += "and refer.MarcaInvID = @MarcaInvID ";
                    mySqlCommand.Parameters.Add("@MarcaInvID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                    mySqlCommand.Parameters["@MarcaInvID"].Value = filter.MarcaInvID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.TipoTrasladoInv.Value.ToString()))
                {
                    query += "and docuInv.TipoTraslado = @TipoTraslado ";
                    mySqlCommand.Parameters.Add("@TipoTraslado", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@TipoTraslado"].Value = filter.TipoTrasladoInv.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ActivoID.Value.ToString()))
                {
                    query += "and mvto.ActivoID = @ActivoID ";
                    mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                    mySqlCommand.Parameters["@ActivoID"].Value = filter.ActivoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.EstadoInv.Value.ToString()))
                {
                    query += "and mvto.EstadoInv = @EstadoInv ";
                    mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@EstadoInv"].Value = filter.EstadoInv.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Parametro1.Value.ToString()))
                {
                    query += "and mvto.Parametro1 = @Parametro1 ";
                    mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                    mySqlCommand.Parameters["@Parametro1"].Value = filter.Parametro1.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Parametro2.Value.ToString()))
                {
                    query += "and mvto.Parametro2 = @Parametro2 ";
                    mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                    mySqlCommand.Parameters["@Parametro2"].Value = filter.Parametro2.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.EntradaSalida.Value.ToString()))
                {
                    query += "and mvto.EntradaSalida = @EntradaSalida ";
                    mySqlCommand.Parameters.Add("@EntradaSalida", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                    mySqlCommand.Parameters["@EntradaSalida"].Value = filter.EntradaSalida.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DocSoporte.Value.ToString()))
                {
                    query += "and mvto.DocSoporte = @DocSoporte ";
                    mySqlCommand.Parameters.Add("@DocSoporte", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocSoporte"].Value = filter.DocSoporte.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.MvtoTipoInvID.Value.ToString()))
                {
                    query += "and mvto.MvtoTipoInvID = @MvtoTipoInvID ";
                    mySqlCommand.Parameters.Add("@MvtoTipoInvID", SqlDbType.Char, 10);
                    mySqlCommand.Parameters["@MvtoTipoInvID"].Value = filter.MvtoTipoInvID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ProyectoID.Value.ToString()))
                {
                    query += "and mvto.ProyectoID = @ProyectoID ";
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@ProyectoID"].Value = filter.ProyectoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.CentroCostoID.Value.ToString()))
                {
                    query += "and mvto.CentroCostoID = @CentroCostoID ";
                    mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommand.Parameters["@CentroCostoID"].Value = filter.CentroCostoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DatoAdd1.Value.ToString()))
                {
                    query += "and mvto.DatoAdd1 = @DatoAdd1 ";
                    mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                    mySqlCommand.Parameters["@DatoAdd1"].Value = filter.DatoAdd1.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DatoAdd2.Value.ToString()))
                {
                    query += "and mvto.DatoAdd2 = @DatoAdd2 ";
                    mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                    mySqlCommand.Parameters["@DatoAdd2"].Value = filter.DatoAdd2.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DatoAdd3.Value.ToString()))
                {
                    query += "and mvto.DatoAdd3 = @DatoAdd3 ";
                    mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                    mySqlCommand.Parameters["@DatoAdd3"].Value = filter.DatoAdd3.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DatoAdd4.Value.ToString()))
                {
                    query += "and mvto.DatoAdd4 = @DatoAdd4 ";
                    mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                    mySqlCommand.Parameters["@DatoAdd4"].Value = filter.DatoAdd4.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DatoAdd5.Value.ToString()))
                {
                    query += "and mvto.DatoAdd5 = @DatoAdd5 ";
                    mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                    mySqlCommand.Parameters["@DatoAdd5"].Value = filter.DatoAdd5.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.FechaIni.Value.ToString()))
                {
                    query += "and cast(ctrl.FechaDoc as date) >= @FechaIni ";
                    mySqlCommand.Parameters.Add("@FechaIni", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@FechaIni"].Value = filter.FechaIni.Value.Value.Date;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.FechaFin.Value.ToString()))
                {
                    query += "and cast(ctrl.FechaDoc as date) <= @FechaFin ";
                    mySqlCommand.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@FechaFin"].Value = filter.FechaFin.Value.Value.Date;
                    filterInd = true;
                }
                query += " order by mvto.NumeroDoc desc";
                #endregion
                mySqlCommand.CommandText = query;
                sda.SelectCommand = mySqlCommand;
                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }               

                if (!string.IsNullOrEmpty(mySqlCommand.CommandText))
                    sda.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDeta_GetByParameter");
                throw exception;
            }
        }
        #endregion

        #endregion

        #region glMovimientoDetaPRE

        /// <summary>
        /// Consulta un movimientoDetaPRE segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns>lista de movimientos</returns>
        public List<DTO_glMovimientoDeta> DAL_glMovimientoDetaPRE_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = " Select det.*,serv.ServicioID,con.ConceptoCargoID from glMovimientoDetaPRE det with(nolock) " +
                                            " left join faServicios serv on serv.ServicioID = det.ServicioID and serv.EmpresaGrupoID = det.eg_faServicios "+ 
                                            " left join faConceptos con on con.ConceptoIngresoID = serv.ConceptoIngresoID and con.EmpresaGrupoID = serv.eg_faConceptos " +
                                            " where det.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                List<DTO_glMovimientoDeta> footer = new List<DTO_glMovimientoDeta>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_glMovimientoDeta detail = new DTO_glMovimientoDeta(dr);
                    detail.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                    detail.Index = index;
                    footer.Add(detail);
                    index++;
                }
                dr.Close();
                return footer;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDetaPRE_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona en tabla glMovimientoDetaPRE
        /// </summary>
        /// <param name="footer">Movimientos</param>
        public void DAL_glMovimientoDetaPRE_Add(List<DTO_glMovimientoDeta> footer)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO glMovimientoDetaPRE " +
                                           "    (EmpresaID    " +
                                           "    ,NumeroDoc    " +
                                           "    ,BodegaID " +
                                           "    ,EntradaSalida " +
                                           "    ,Kit " +
                                           "    ,inReferenciaID " +
                                           "    ,EstadoInv " +
                                           "    ,Parametro1 " +
                                           "    ,Parametro2 " +
                                           "    ,IdentificadorTr " +
                                           "    ,CodigoBSID " +
                                           "    ,ServicioID " +
                                           "    ,SerialID " +
                                           "    ,ActivoID " +
                                           "    ,MvtoTipoInvID " +
                                           "    ,MvtoTipoActID " +
                                           "    ,DocSoporte " +
                                           "    ,DocSoporteTER " +
                                           "    ,AsesorID " +
                                           "    ,ProyectoID " +
                                           "    ,CentroCostoID " +
                                           "    ,LineaPresupuestoID " + 
                                           "    ,TerceroID " +
                                           "    ,DatoAdd1 " +
                                           "    ,DatoAdd2 " +
                                           "    ,DatoAdd3 " +
                                           "    ,DatoAdd4 " +
                                           "    ,DatoAdd5 " +
                                           "    ,DescripTExt " +
                                           "    ,EmpaqueInvID " +
                                           "    ,CantidadEMP " +
                                           "    ,CantidadDoc " +
                                           "    ,CantidadUNI " +
                                           "    ,ValorUNI " +
                                           "    ,Valor1LOC " +
                                           "    ,Valor2LOC " +
                                           "    ,Valor3LOC " +
                                           "    ,Valor4LOC " +
                                           "    ,Valor5LOC " +
                                           "    ,Valor6LOC " +
                                           "    ,Valor7LOC " +
                                           "    ,Valor8LOC " +
                                           "    ,Valor9LOC " +
                                           "    ,Valor10LOC " +
                                           "    ,Valor1EXT " +
                                           "    ,Valor2EXT " +
                                           "    ,Valor3EXT " +
                                           "    ,Valor4EXT " +
                                           "    ,Valor5EXT " +
                                           "    ,Valor6EXT " +
                                           "    ,Valor7EXT " +
                                           "    ,Valor8EXT " +
                                           "    ,Valor9EXT " +
                                           "    ,Valor10EXT " +
                                           "    ,CantidadDEV" +
                                           "    ,ConsecutivoPrestamo" +
                                           "    ,ConsecutivoOrdCompra" +
                                           "    ,NroItem" +
                                           "    ,ImprimeInd" +
                                           "    ,eg_inBodega " +
                                           "    ,eg_inReferencia " +
                                           "    ,eg_inRefParametro1 " +
                                           "    ,eg_inRefParametro2 " +
                                           "    ,eg_prBienServicio " +
                                           "    ,eg_faServicios " +
                                           "    ,eg_inMovimientoTipo " +
                                           "    ,eg_acMovimientoTipo " +
                                           "    ,eg_faAsesor " +
                                           "    ,eg_coProyecto " +
                                           "    ,eg_coCentroCosto " +
                                           "    ,eg_coTercero " +
                                           "    ,eg_inEmpaque " +
                                           "    ,eg_plLineaPresupuesto) " + 
                                           "    VALUES          " +
                                           "    (@EmpresaID     " +
                                           "    ,@NumeroDoc     " +
                                           "    ,@BodegaID " +
                                           "    ,@EntradaSalida " +
                                           "    ,@Kit " +
                                           "    ,@inReferenciaID " +
                                           "    ,@EstadoInv " +
                                           "    ,@Parametro1 " +
                                           "    ,@Parametro2 " +
                                           "    ,@IdentificadorTr " +
                                           "    ,@CodigoBSID " +
                                           "    ,@ServicioID " +
                                           "    ,@SerialID " +
                                           "    ,@ActivoID " +
                                           "    ,@MvtoTipoInvID " +
                                           "    ,@MvtoTipoActID " +
                                           "    ,@DocSoporte " +
                                           "    ,@DocSoporteTER " +
                                           "    ,@AsesorID " +
                                           "    ,@ProyectoID " +
                                           "    ,@CentroCostoID " +
                                           "    ,@LineaPresupuestoID " + 
                                           "    ,@TerceroID " +
                                           "    ,@DatoAdd1 " +
                                           "    ,@DatoAdd2 " +
                                           "    ,@DatoAdd3 " +
                                           "    ,@DatoAdd4 " +
                                           "    ,@DatoAdd5 " +
                                           "    ,@DescripTExt " +
                                           "    ,@EmpaqueInvID " +
                                           "    ,@CantidadEMP " +
                                           "    ,@CantidadDoc " +
                                           "    ,@CantidadUNI " +
                                           "    ,@ValorUNI " +
                                           "    ,@Valor1LOC " +
                                           "    ,@Valor2LOC " +
                                           "    ,@Valor3LOC " +
                                           "    ,@Valor4LOC " +
                                           "    ,@Valor5LOC " +
                                           "    ,@Valor6LOC " +
                                           "    ,@Valor7LOC " +
                                           "    ,@Valor8LOC " +
                                           "    ,@Valor9LOC " +
                                           "    ,@Valor10LOC " +
                                           "    ,@Valor1EXT " +
                                           "    ,@Valor2EXT " +
                                           "    ,@Valor3EXT " +
                                           "    ,@Valor4EXT " +
                                           "    ,@Valor5EXT " +
                                           "    ,@Valor6EXT " +
                                           "    ,@Valor7EXT " +
                                           "    ,@Valor8EXT " +
                                           "    ,@Valor9EXT " +
                                           "    ,@Valor10EXT " +
                                           "    ,@CantidadDEV" +
                                           "    ,@ConsecutivoPrestamo" +
                                           "    ,@ConsecutivoOrdCompra" +
                                           "    ,@NroItem" +
                                           "    ,@ImprimeInd" +
                                           "    ,@eg_inBodega " +
                                           "    ,@eg_inReferencia " +
                                           "    ,@eg_inRefParametro1 " +
                                           "    ,@eg_inRefParametro2 " +
                                           "    ,@eg_prBienServicio " +
                                           "    ,@eg_faServicios " +
                                           "    ,@eg_inMovimientoTipo " +
                                           "    ,@eg_acMovimientoTipo " +
                                           "    ,@eg_faAsesor " +
                                           "    ,@eg_coProyecto " +
                                           "    ,@eg_coCentroCosto " +
                                           "    ,@eg_coTercero " +
                                           "    ,@eg_inEmpaque" +
                                           "    ,@eg_plLineaPresupuesto )";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@EntradaSalida", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Kit", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@IdentificadorTr", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@ServicioID", SqlDbType.Char, UDT_ServicioID.MaxLength);
                mySqlCommand.Parameters.Add("@SerialID", SqlDbType.Char, UDT_SerialID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MvtoTipoInvID", SqlDbType.Char, UDT_MvtoTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@MvtoTipoActID", SqlDbType.Char, UDT_MvtoTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocSoporte", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocSoporteTER", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DescripTExt", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@EmpaqueInvID", SqlDbType.Char, UDT_EmpaqueInvID.MaxLength);
                mySqlCommand.Parameters.Add("@CantidadEMP", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadDoc", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadUNI", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorUNI", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor1LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor2LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor3LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor4LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor5LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor6LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor7LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor8LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor9LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor10LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor1EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor2EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor3EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor4EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor5EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor6EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor7EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor8EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor9EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor10EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadDEV", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ConsecutivoPrestamo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ConsecutivoOrdCompra", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NroItem", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ImprimeInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@eg_inBodega", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inRefParametro1", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inRefParametro2", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_faServicios", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inMovimientoTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_acMovimientoTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_faAsesor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inEmpaque", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                
                #endregion
                foreach (DTO_glMovimientoDeta det in footer)
                {
                    #region Asignacion de valores
                    mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommand.Parameters["@NumeroDoc"].Value = det.NumeroDoc.Value;
                    mySqlCommand.Parameters["@BodegaID"].Value = det.BodegaID.Value;
                    mySqlCommand.Parameters["@EntradaSalida"].Value = det.EntradaSalida.Value;
                    mySqlCommand.Parameters["@Kit"].Value = det.Kit.Value;
                    mySqlCommand.Parameters["@inReferenciaID"].Value = det.inReferenciaID.Value;
                    mySqlCommand.Parameters["@EstadoInv"].Value = det.EstadoInv.Value;
                    mySqlCommand.Parameters["@Parametro1"].Value = det.Parametro1.Value;
                    mySqlCommand.Parameters["@Parametro2"].Value = det.Parametro2.Value;
                    mySqlCommand.Parameters["@IdentificadorTr"].Value = det.IdentificadorTr.Value;
                    mySqlCommand.Parameters["@CodigoBSID"].Value = det.CodigoBSID.Value;
                    mySqlCommand.Parameters["@ServicioID"].Value = det.ServicioID.Value;
                    mySqlCommand.Parameters["@SerialID"].Value = det.SerialID.Value;
                    mySqlCommand.Parameters["@ActivoID"].Value = det.ActivoID.Value ?? 0;
                    mySqlCommand.Parameters["@MvtoTipoInvID"].Value = det.MvtoTipoInvID.Value;
                    mySqlCommand.Parameters["@MvtoTipoActID"].Value = det.MvtoTipoActID.Value;
                    mySqlCommand.Parameters["@DocSoporte"].Value = det.DocSoporte.Value;
                    mySqlCommand.Parameters["@DocSoporteTER"].Value = det.DocSoporteTER.Value;
                    mySqlCommand.Parameters["@AsesorID"].Value = det.AsesorID.Value;
                    mySqlCommand.Parameters["@ProyectoID"].Value = det.ProyectoID.Value;
                    mySqlCommand.Parameters["@CentroCostoID"].Value = det.CentroCostoID.Value;
                    mySqlCommand.Parameters["@LineaPresupuestoID"].Value = det.LineaPresupuestoID.Value;
                    mySqlCommand.Parameters["@TerceroID"].Value = det.TerceroID.Value;
                    mySqlCommand.Parameters["@DatoAdd1"].Value = det.DatoAdd1.Value;
                    mySqlCommand.Parameters["@DatoAdd2"].Value = det.DatoAdd2.Value;
                    mySqlCommand.Parameters["@DatoAdd3"].Value = det.DatoAdd3.Value;
                    mySqlCommand.Parameters["@DatoAdd4"].Value = det.DatoAdd4.Value;
                    mySqlCommand.Parameters["@DatoAdd5"].Value = det.DatoAdd5.Value;
                    mySqlCommand.Parameters["@DescripTExt"].Value = det.DescripTExt.Value;
                    mySqlCommand.Parameters["@EmpaqueInvID"].Value = det.EmpaqueInvID.Value;
                    mySqlCommand.Parameters["@CantidadEMP"].Value = det.CantidadEMP.Value;
                    mySqlCommand.Parameters["@CantidadDoc"].Value = det.CantidadDoc.Value ?? 0;
                    mySqlCommand.Parameters["@CantidadUNI"].Value = det.CantidadUNI.Value ?? 0;
                    mySqlCommand.Parameters["@ValorUNI"].Value = det.ValorUNI.Value ?? 0;
                    mySqlCommand.Parameters["@Valor1LOC"].Value = det.Valor1LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor2LOC"].Value = det.Valor2LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor3LOC"].Value = det.Valor3LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor4LOC"].Value = det.Valor4LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor5LOC"].Value = det.Valor5LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor6LOC"].Value = det.Valor6LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor7LOC"].Value = det.Valor7LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor8LOC"].Value = det.Valor8LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor9LOC"].Value = det.Valor9LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor10LOC"].Value = det.Valor10LOC.Value ?? 0;
                    mySqlCommand.Parameters["@Valor1EXT"].Value = det.Valor1EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor2EXT"].Value = det.Valor2EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor3EXT"].Value = det.Valor3EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor4EXT"].Value = det.Valor4EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor5EXT"].Value = det.Valor5EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor6EXT"].Value = det.Valor6EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor7EXT"].Value = det.Valor7EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor8EXT"].Value = det.Valor8EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor9EXT"].Value = det.Valor9EXT.Value ?? 0;
                    mySqlCommand.Parameters["@Valor10EXT"].Value = det.Valor10EXT.Value ?? 0;
                    mySqlCommand.Parameters["@CantidadDEV"].Value = det.CantidadDEV.Value ?? 0;
                    mySqlCommand.Parameters["@ConsecutivoPrestamo"].Value = det.ConsecutivoPrestamo.Value;
                    mySqlCommand.Parameters["@ConsecutivoOrdCompra"].Value = det.ConsecutivoOrdCompra.Value;
                    mySqlCommand.Parameters["@NroItem"].Value = det.NroItem.Value ?? 0;
                    mySqlCommand.Parameters["@ImprimeInd"].Value = det.ImprimeInd.Value ?? true;      
                    mySqlCommand.Parameters["@eg_inBodega"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inBodega, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_inRefParametro1"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inRefParametro1, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_inRefParametro2"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inRefParametro2, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_faServicios"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faServicios, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_inMovimientoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inMovimientoTipo, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_acMovimientoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acMovimientoTipo, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_faAsesor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faAsesor, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_inEmpaque"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inEmpaque, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
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
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDetaPRE_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Edita un registro de recibidos
        /// </summary>
        /// <param name="docCtrl">Documento que se va a editar</param>
        public void DAL_glMovimientoDetaPRE_Update(DTO_glMovimientoDeta deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "UPDATE glMovimientoDetaPRE SET " +
                                           "    NumeroDoc = @NumeroDoc " +
                                           "    ,BodegaID = @BodegaID " +
                                           "    ,EntradaSalida = @EntradaSalida " +
                                           "    ,Kit = @Kit " +
                                           "    ,inReferenciaID = @inReferenciaID " +
                                           "    ,EstadoInv = @EstadoInv " +
                                           "    ,Parametro1  = @Parametro1 " +
                                           "    ,Parametro2  = @Parametro2 " +
                                           "    ,IdentificadorTr = @IdentificadorTr " +
                                           "    ,CodigoBSID = @CodigoBSID " +
                                           "    ,ServicioID  = @ServicioID " +
                                           "    ,SerialID  = @SerialID " +
                                           "    ,ActivoID = @ActivoID " +
                                           "    ,MvtoTipoInvID = @MvtoTipoInvID " +
                                           "    ,MvtoTipoActID  = @MvtoTipoActID " +
                                           "    ,DocSoporte = @DocSoporte " +
                                           "    ,DocSoporteTER = @DocSoporteTER " +
                                           "    ,AsesorID  = @AsesorID " +
                                           "    ,ProyectoID  = @ProyectoID " +
                                           "    ,CentroCostoID  = @CentroCostoID " +
                                           "    ,LineaPresupuestoID  = @LineaPresupuestoID " +
                                           "    ,TerceroID  = @TerceroID " +
                                           "    ,DatoAdd1  = @DatoAdd1 " +
                                           "    ,DatoAdd2  = @DatoAdd2 " +
                                           "    ,DatoAdd3  = @DatoAdd3 " +
                                           "    ,DatoAdd4  = @DatoAdd4 " +
                                           "    ,DatoAdd5  = @DatoAdd5 " +
                                           "    ,DescripTExt  = @DescripTExt " +
                                           "    ,EmpaqueInvID  = @EmpaqueInvID " +
                                           "    ,CantidadEMP = @CantidadEMP " +
                                           "    ,CantidadDoc = @CantidadDoc " +
                                           "    ,CantidadUNI  = @CantidadUNI " +
                                           "    ,ValorUNI  = @ValorUNI " +
                                           "    ,Valor1LOC  = @Valor1LOC " +
                                           "    ,Valor2LOC = @Valor2LOC " +
                                           "    ,Valor3LOC  = @Valor3LOC " +
                                           "    ,Valor4LOC  = @Valor4LOC " +
                                           "    ,Valor5LOC = @Valor5LOC " +
                                           "    ,Valor6LOC = @Valor6LOC " +
                                           "    ,Valor7LOC = @Valor7LOC " +
                                           "    ,Valor8LOC  = @Valor8LOC " +
                                           "    ,Valor9LOC = @Valor9LOC " +
                                           "    ,Valor10LOC  = @Valor10LOC " +
                                           "    ,Valor1EXT  = @Valor1EXT " +
                                           "    ,Valor2EXT  = @Valor2EXT " +
                                           "    ,Valor3EXT  = @Valor3EXT " +
                                           "    ,Valor4EXT  = @Valor4EXT " +
                                           "    ,Valor5EXT  = @Valor5EXT " +
                                           "    ,Valor6EXT  = @Valor6EXT " +
                                           "    ,Valor7EXT  = @Valor7EXT " +
                                           "    ,Valor8EXT  = @Valor8EXT " +
                                           "    ,Valor9EXT  = @Valor9EXT " +
                                           "    ,Valor10EXT  = @Valor10EXT " +
                                           "    ,CantidadDEV  = @CantidadDEV" +
                                           "    ,ConsecutivoPrestamo  = @ConsecutivoPrestamo" +
                                           "    ,ConsecutivoOrdCompra  = @ConsecutivoOrdCompra" +
                                           "    ,NroItem  = @NroItem" +
                                           "    ,ImprimeInd  = @ImprimeInd" +
                                            "WHERE Consecutivo = @Consecutivo ";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@EntradaSalida", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Kit", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@IdentificadorTr", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@ServicioID", SqlDbType.Char, UDT_ServicioID.MaxLength);
                mySqlCommand.Parameters.Add("@SerialID", SqlDbType.Char, UDT_SerialID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MvtoTipoInvID", SqlDbType.Char, UDT_MvtoTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@MvtoTipoActID", SqlDbType.Char, UDT_MvtoTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocSoporte", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocSoporteTER", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.VarChar, 20);
                mySqlCommand.Parameters.Add("@DescripTExt", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@EmpaqueInvID", SqlDbType.Char, UDT_EmpaqueInvID.MaxLength);
                mySqlCommand.Parameters.Add("@CantidadEMP", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadDoc", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadUNI", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorUNI", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor1LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor2LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor3LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor4LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor5LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor6LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor7LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor8LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor9LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor10LOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor1EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor2EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor3EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor4EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor5EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor6EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor7EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor8EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor9EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor10EXT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadDEV", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ConsecutivoPrestamo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ConsecutivoOrdCompra", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NroItem", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ImprimeInd", SqlDbType.Bit);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = deta.NumeroDoc.Value;
                mySqlCommand.Parameters["@Consecutivo"].Value = deta.Consecutivo.Value;
                mySqlCommand.Parameters["@BodegaID"].Value = deta.BodegaID.Value;
                mySqlCommand.Parameters["@EntradaSalida"].Value = deta.EntradaSalida.Value;
                mySqlCommand.Parameters["@Kit"].Value = deta.Kit.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = deta.inReferenciaID.Value;
                mySqlCommand.Parameters["@EstadoInv"].Value = deta.EstadoInv.Value;
                mySqlCommand.Parameters["@Parametro1"].Value = deta.Parametro1.Value;
                mySqlCommand.Parameters["@Parametro2"].Value = deta.Parametro2.Value;
                mySqlCommand.Parameters["@IdentificadorTr"].Value = deta.IdentificadorTr.Value;
                mySqlCommand.Parameters["@CodigoBSID"].Value = deta.CodigoBSID.Value;
                mySqlCommand.Parameters["@ServicioID"].Value = deta.ServicioID.Value;
                mySqlCommand.Parameters["@SerialID"].Value = deta.SerialID.Value;
                mySqlCommand.Parameters["@ActivoID"].Value = deta.ActivoID.Value ?? 0;
                mySqlCommand.Parameters["@MvtoTipoInvID"].Value = deta.MvtoTipoInvID.Value;
                mySqlCommand.Parameters["@MvtoTipoActID"].Value = deta.MvtoTipoActID.Value;
                mySqlCommand.Parameters["@DocSoporte"].Value = deta.DocSoporte.Value;
                mySqlCommand.Parameters["@DocSoporteTER"].Value = deta.DocSoporteTER.Value;
                mySqlCommand.Parameters["@AsesorID"].Value = deta.AsesorID.Value;
                mySqlCommand.Parameters["@ProyectoID"].Value = deta.ProyectoID.Value;
                mySqlCommand.Parameters["@CentroCostoID"].Value = deta.CentroCostoID.Value;
                mySqlCommand.Parameters["@LineaPresupuestoID"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = deta.TerceroID.Value;
                mySqlCommand.Parameters["@DatoAdd1"].Value = deta.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd2"].Value = deta.DatoAdd2.Value;
                mySqlCommand.Parameters["@DatoAdd3"].Value = deta.DatoAdd3.Value;
                mySqlCommand.Parameters["@DatoAdd4"].Value = deta.DatoAdd4.Value;
                mySqlCommand.Parameters["@DatoAdd5"].Value = deta.DatoAdd5.Value;
                mySqlCommand.Parameters["@DescripTExt"].Value = deta.DescripTExt.Value;
                mySqlCommand.Parameters["@EmpaqueInvID"].Value = deta.EmpaqueInvID.Value;
                mySqlCommand.Parameters["@CantidadEMP"].Value = deta.CantidadEMP.Value;
                mySqlCommand.Parameters["@CantidadDoc"].Value = deta.CantidadDoc.Value ?? 0;
                mySqlCommand.Parameters["@CantidadUNI"].Value = deta.CantidadUNI.Value ?? 0;
                mySqlCommand.Parameters["@ValorUNI"].Value = deta.ValorUNI.Value ?? 0;
                mySqlCommand.Parameters["@Valor1LOC"].Value = deta.Valor1LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor2LOC"].Value = deta.Valor2LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor3LOC"].Value = deta.Valor3LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor4LOC"].Value = deta.Valor4LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor5LOC"].Value = deta.Valor5LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor6LOC"].Value = deta.Valor6LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor7LOC"].Value = deta.Valor7LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor8LOC"].Value = deta.Valor8LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor9LOC"].Value = deta.Valor9LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor10LOC"].Value = deta.Valor10LOC.Value ?? 0;
                mySqlCommand.Parameters["@Valor1EXT"].Value = deta.Valor1EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor2EXT"].Value = deta.Valor2EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor3EXT"].Value = deta.Valor3EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor4EXT"].Value = deta.Valor4EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor5EXT"].Value = deta.Valor5EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor6EXT"].Value = deta.Valor6EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor7EXT"].Value = deta.Valor7EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor8EXT"].Value = deta.Valor8EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor9EXT"].Value = deta.Valor9EXT.Value ?? 0;
                mySqlCommand.Parameters["@Valor10EXT"].Value = deta.Valor10EXT.Value ?? 0;
                mySqlCommand.Parameters["@CantidadDEV"].Value = deta.CantidadDEV.Value ?? 0;
                mySqlCommand.Parameters["@ConsecutivoPrestamo"].Value = deta.ConsecutivoPrestamo.Value;
                mySqlCommand.Parameters["@ConsecutivoOrdCompra"].Value = deta.ConsecutivoOrdCompra.Value;
                mySqlCommand.Parameters["@NroItem"].Value = deta.NroItem.Value ?? 0;
                mySqlCommand.Parameters["@ImprimeInd"].Value = deta.ImprimeInd.Value ?? true;    
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDetaPRE_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina registros de la tabla de glMovimientoDeta
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public void DAL_glMovimientoDetaPRE_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText = "DELETE FROM glMovimientoDetaPRE where EmpresaID = @EmpresaID " +
                " and NumeroDoc = @NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDetaPRE_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta un movimientoDetaPRE relacionado con proyectos con saldos de Inventario
        /// </summary>
        /// <param name="periodo">Periodo de saldos de inventarios</param>
        /// <param name="bodega1">Bodega a consultar</param>
        /// <param name="proyectoID">Proyecto a consultar</param>
        /// <returns>lista de movimientos</returns>
        public List<DTO_glMovimientoDeta> DAL_glMovimientoDetaPRE_GetSaldosInvByProyecto(DateTime periodo, string bodega1, string bodega2, string proyectoID, string servicioInvent,string mvtoVentas, bool isPre)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string tabla = isPre ? " glMovimientoDetaPRE " : " glMovimientoDeta ";
                string where = string.Empty;
                string findVentas = string.Empty;
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char,UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                if(!string.IsNullOrEmpty(bodega1))
                {
                    where += " and (saldos.BodegaID = @BodegaEmpresa or saldos.BodegaID = @BodegaCliente) ";
                    mySqlCommand.Parameters.Add("@BodegaEmpresa", SqlDbType.Char, UDT_BodegaID.MaxLength);
                    mySqlCommand.Parameters["@BodegaEmpresa"].Value = bodega1;
                    mySqlCommand.Parameters.Add("@BodegaCliente", SqlDbType.Char, UDT_BodegaID.MaxLength);
                    mySqlCommand.Parameters["@BodegaCliente"].Value = bodega2;
                }
                if (!string.IsNullOrEmpty(proyectoID))
                {
                    where += "  and m.ProyectoID = @ProyectoID ";
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@ProyectoID"].Value = proyectoID;
                }
                if (!string.IsNullOrEmpty(mvtoVentas))//resta los movimientos de facturas de venta ya realizados
                {
                    findVentas += " -(Select Isnull(sum(mExist.CantidadUNI),0) from " + tabla + " mExist where mExist.EntradaSalida = 2 and  mExist.inReferenciaID = refer.inReferenciaID and MvtoTipoInvID = @MvtoVentas and ProyectoID = @ProyectoID) ";
                    mySqlCommand.Parameters.Add("@MvtoVentas", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@MvtoVentas"].Value = mvtoVentas;
                }

                mySqlCommand.CommandText = " Select distinct m.ProyectoID, m.*,tar.TareaID,tar.Descriptivo as DescriptivoTarea, refer.inReferenciaID as Referencia,saldos.BodegaID as Bodega ,refer.Descriptivo as ReferenciaDesc, refer.UnidadInvID,refer.MarcaInvID,refer.RefProveedor, " +
                                            " (tar.Cantidad * det.FactorID) " + findVentas + " as CantidadTot," + 
                                            "  saldos.CantInicial+saldos.CantEntrada-saldos.CantRetiro as Saldo  " +
                                            " from " + tabla + " m " +
                                            "   inner join pyProyectoTarea tar on tar.Consecutivo = m.DocSoporte and m.DatoAdd4 = tar.NumeroDoc " +
                                            "   inner join pyProyectoDeta det on det.ConsecTarea = tar.Consecutivo and m.DatoAdd4 = tar.NumeroDoc " +
                                            "   inner join pyRecurso rec on rec.RecursoID = det.RecursoID and rec.EmpresaGrupoID = det.eg_pyRecurso " +
                                            "   inner join inReferencia refer on refer.inReferenciaID = rec.inReferenciaID and refer.EmpresaGrupoID = rec.eg_inReferencia " +
                                            "   inner join inSaldosExistencias saldos on saldos.inReferenciaID = refer.inReferenciaID and saldos.Periodo = @Periodo  " +
                                            " where m.EmpresaID = @EmpresaID and m.inReferenciaID is null and m.BodegaID is null  " +
                                            " and (saldos.CantInicial+saldos.CantEntrada-saldos.CantRetiro) > 0 " + where +
                                            " order by m.ProyectoID,tar.TareaID";


                List<DTO_glMovimientoDeta> footer = new List<DTO_glMovimientoDeta>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_glMovimientoDeta detail = new DTO_glMovimientoDeta(dr);
                    detail.inReferenciaID.Value = dr["Referencia"].ToString();
                    detail.BodegaID.Value = dr["Bodega"].ToString();
                    detail.DescripTExt.Value = dr["ReferenciaDesc"].ToString();                  
                    detail.CantidadRecurso.Value = Convert.ToDecimal(dr["CantidadTot"]);
                    if (detail.CantidadRecurso.Value < 0)
                        detail.CantidadRecurso.Value = 0;
                    detail.CantidadDispon.Value = Convert.ToDecimal(dr["Saldo"]);
                    if (footer.FindAll(x => x.BodegaID.Value == detail.BodegaID.Value && x.inReferenciaID.Value == detail.inReferenciaID.Value).Sum(x => x.CantidadUNI.Value) == 0)
                        detail.CantidadUNI.Value = detail.CantidadRecurso.Value > detail.CantidadDispon.Value ? detail.CantidadDispon.Value : detail.CantidadRecurso.Value;
                    else
                        detail.CantidadUNI.Value = detail.CantidadDispon.Value - footer.FindAll(x => x.BodegaID.Value == detail.BodegaID.Value && x.inReferenciaID.Value == detail.inReferenciaID.Value).Sum(x => x.CantidadUNI.Value);
                    detail.CantidadEMP.Value = detail.CantidadUNI.Value;
                    detail.TareaID.Value = dr["TareaID"].ToString();
                    detail.DescriptivoTarea.Value = dr["DescriptivoTarea"].ToString();
                    detail.ServicioID.Value = servicioInvent;
                    detail.MarcaInvID.Value = dr["MarcaInvID"].ToString();
                    detail.RefProveedor.Value = dr["RefProveedor"].ToString();
                    detail.DatoAdd5.Value = "INV";
                    detail.Index = index;
                    footer.Add(detail);
                    index++;
                }
                dr.Close();
                return footer;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glMovimientoDetaPRE_Get");
                throw exception;
            }
        }


        #endregion
    }
}
