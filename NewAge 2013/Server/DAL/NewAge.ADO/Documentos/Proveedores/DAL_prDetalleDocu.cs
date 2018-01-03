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
    /// DAL de DAL_prDetalleDocu
    /// </summary>
    public class DAL_prDetalleDocu : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_prDetalleDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Trae un activo control por segun la llave primaria
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        public DTO_prDetalleDocu DAL_prDetalleDocu_GetByID(int ConsecutivoDetaID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select det.*,ref.RefProveedor,ref.MarcaInvID,IsNull(empaque.Cantidad,1) as CantidadxEmpaque from prDetalleDocu det with(nolock) " +
                                              " left join inReferencia ref with(nolock) On ref.inReferenciaID = det.inReferenciaID and ref.EmpresaGrupoID = det.eg_inReferencia  " +
                                              " left join inEmpaque empaque with(nolock) On empaque.EmpaqueInvID = det.EmpaqueInvID and empaque.EmpresaGrupoID = det.eg_inEmpaque  " +
                                              "where det.ConsecutivoDetaID = @ConsecutivoDetaID";

                mySqlCommand.Parameters.Add("@ConsecutivoDetaID", SqlDbType.Int);
                mySqlCommand.Parameters["@ConsecutivoDetaID"].Value = ConsecutivoDetaID;

                DTO_prDetalleDocu res = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    res = new DTO_prDetalleDocu(dr);
                    res.MarcaInvID.Value = dr["MarcaInvID"].ToString();
                    res.RefProveedor.Value = dr["RefProveedor"].ToString();
                    res.CantidadxEmpaque.Value = Convert.ToInt32(dr["CantidadxEmpaque"]);
                    if (res.CantidadxEmpaque.Value == 0)
                        res.CantidadxEmpaque.Value = 1;
                }
                dr.Close();
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_prDetalleDocu", "DAL_prDetalleDocu_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega un registro al control de documentos
        /// </summary>
        public int DAL_prDetalleDocu_Add(DTO_prDetalleDocu rec)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "INSERT INTO [prDetalleDocu]" +
                        "(EmpresaID" +
                        ",NumeroDoc" +
                        ",CodigoBSID" +
                        ",Descriptivo" +
                        ",inReferenciaID" +
                        ",LineaPresupuestoID" +
                        ",EstadoInv" +
                        ",Parametro1" +
                        ",Parametro2" +
                        ",ActivoID" +
                        ",SerialID" +
                        ",OrigenMonetario" +
                        ",MonedaID" +
                        ",UnidadInvID" +
                        ",EmpaqueInvID" +
                        ",CantidadSol" +
                        ",CantidadEMP" +
                        ",ValorUni" +
                        ",IVAUni" +
                        ",ValorTotML" +
                        ",IvaTotML" +
                        ",ValorTotME" +
                        ",IvaTotME" +
                        ",SolicitudDocuID" +
                        ",LegalizaDocuID" +
                        ",OrdCompraDocuID" +
                        ",ContratoDocuID" +
                        ",InventarioDocuID" +
                        ",RecibidoDocuID" +
                        ",ActivoDocuID" +
                        ",FacturaDocuID" +
                        ",Documento1ID" +
                        ",Documento2ID" +
                        ",Documento3ID" +
                        ",Documento4ID" +
                        ",Documento5ID" +
                        ",SolicitudDetaID" +
                        ",LegalizaDetaID" +
                        ",OrdCompraDetaID" +
                        ",ContratoDetaID" +
                        ",InventarioDetaID" +
                        ",RecibidoDetaID" +
                        ",Detalle1ID" +
                        ",Detalle2ID" +
                        ",Detalle3ID" +
                        ",Detalle4ID" +
                        ",Detalle5ID" +
                        ",CantidadDoc1" +
                        ",CantidadDoc2" +
                        ",CantidadDoc3" +
                        ",CantidadDoc4" +
                        ",CantidadDoc5" +
                        ",VlrLocal01" +
                        ",VlrExtra01" +
                        ",VlrLocal02" +
                        ",VlrExtra02" +
                        ",VlrLocal03" +
                        ",VlrExtra03" +
                        ",VlrLocal04" +
                        ",VlrExtra04" +
                        ",VlrLocal05" +
                        ",VlrExtra05" +
                        ",VlrLocal06" +
                        ",VlrExtra06" +
                        ",VlrLocal07" +
                        ",VlrExtra07" +
                        ",VlrLocal08" +
                        ",VlrExtra08" +
                        ",VlrLocal09" +
                        ",VlrExtra09" +
                        ",VlrLocal10" +
                        ",VlrExtra10" +
                        ",eg_prBienServicio" +
                        ",eg_inReferencia" +
                        ",eg_inRefParametro1" +
                        ",eg_inRefParametro2" +
                        ",eg_inUnidad" +
                        ",eg_inEmpaque" +
                        ",eg_plLineaPresupuesto" +
                        ",DatoAdd1" +
                        ",DatoAdd2" +
                        ",DatoAdd3" +
                        ",DatoAdd4" +
                        ",DatoAdd5" +
                        ",CantidadOC" +
                        ",CantidadRec" +
                        ",CantidadCont " +
                        ",PorcentajeIVA " +
                        ",ValorBaseAIU " +
                        ",ValorAIU " +
                        ",VlrIVAAIU " +
                        ",CodigoAdminAIU " +
                        ",ValorAdminAIU " +
                        ",IVAAdminAIU " +
                        ",PorIVAAdminAIU " +
                        ",CodigoImprevAIU " +
                        ",ValorImprevAIU " +
                        ",IVAImprevAIU " +
                        ",PorIVAImprevAIU " +
                        ",CodigoUtilidadAIU " +
                        ",ValorUtilidadAIU " +
                        ",IVAUtilidadAIU " +
                        ",PorIVAUtilidadAIU " +
                        ",DiasEntrega " +
                        ",TipoAprobSobreejecucion " +
                        ",UsuarioRevSobreejec " +
                        ",FechaRevSobreejec " +
                        ",UsuarioAprSobreejec " +
                        ",FechaAprSobreejec " +
                        ",CantidadINV " +
                    ")VALUES(" +
                        "@EmpresaID" +
                        ",@NumeroDoc" +
                        ",@CodigoBSID" +
                        ",@Descriptivo" +
                        ",@inReferenciaID" +
                        ",@LineaPresupuestoID" +
                        ",@EstadoInv" +
                        ",@Parametro1" +
                        ",@Parametro2" +
                        ",@ActivoID" +
                        ",@SerialID" +
                        ",@OrigenMonetario" +
                        ",@MonedaID" +
                        ",@UnidadInvID" +
                        ",@EmpaqueInvID" +
                        ",@CantidadSol" +
                        ",@CantidadEMP" +
                        ",@ValorUni" +
                        ",@IVAUni" +
                        ",@ValorTotML" +
                        ",@IvaTotML" +
                        ",@ValorTotME" +
                        ",@IvaTotME" +
                        ",@SolicitudDocuID" +
                        ",@LegalizaDocuID" +
                        ",@OrdCompraDocuID" +
                        ",@ContratoDocuID" +
                        ",@InventarioDocuID" +
                        ",@RecibidoDocuID" +
                        ",@ActivoDocuID" +
                        ",@FacturaDocuID" +
                        ",@Documento1ID" +
                        ",@Documento2ID" +
                        ",@Documento3ID" +
                        ",@Documento4ID" +
                        ",@Documento5ID" +
                        ",@SolicitudDetaID" +
                        ",@LegalizaDetaID" +
                        ",@OrdCompraDetaID" +
                        ",@ContratoDetaID" +
                        ",@InventarioDetaID" +
                        ",@RecibidoDetaID" +
                        ",@Detalle1ID" +
                        ",@Detalle2ID" +
                        ",@Detalle3ID" +
                        ",@Detalle4ID" +
                        ",@Detalle5ID" +
                        ",@CantidadDoc1" +
                        ",@CantidadDoc2" +
                        ",@CantidadDoc3" +
                        ",@CantidadDoc4" +
                        ",@CantidadDoc5" +
                        ",@VlrLocal01" +
                        ",@VlrExtra01" +
                        ",@VlrLocal02" +
                        ",@VlrExtra02" +
                        ",@VlrLocal03" +
                        ",@VlrExtra03" +
                        ",@VlrLocal04" +
                        ",@VlrExtra04" +
                        ",@VlrLocal05" +
                        ",@VlrExtra05" +
                        ",@VlrLocal06" +
                        ",@VlrExtra06" +
                        ",@VlrLocal07" +
                        ",@VlrExtra07" +
                        ",@VlrLocal08" +
                        ",@VlrExtra08" +
                        ",@VlrLocal09" +
                        ",@VlrExtra09" +
                        ",@VlrLocal10" +
                        ",@VlrExtra10" +
                        ",@eg_prBienServicio" +
                        ",@eg_inReferencia" +
                        ",@eg_inRefParametro1" +
                        ",@eg_inRefParametro2" +              
                        ",@eg_inUnidad" +
                        ",@eg_inEmpaque" +
                        ",@eg_plLineaPresupuesto" +
                        ",@DatoAdd1" +
                        ",@DatoAdd2" +
                        ",@DatoAdd3" +
                        ",@DatoAdd4" +
                        ",@DatoAdd5" +
                        ",@CantidadOC" +
                        ",@CantidadRec" +
                        ",@CantidadCont " +
                        ",@PorcentajeIVA " +
                        ",@ValorBaseAIU " +
                        ",@ValorAIU " +
                        ",@VlrIVAAIU " +
                        ",@CodigoAdminAIU " +
                        ",@ValorAdminAIU " +
                        ",@IVAAdminAIU " +
                        ",@PorIVAAdminAIU " +
                        ",@CodigoImprevAIU " +
                        ",@ValorImprevAIU " +
                        ",@IVAImprevAIU " +
                        ",@PorIVAImprevAIU " +
                        ",@CodigoUtilidadAIU " +
                        ",@ValorUtilidadAIU " +
                        ",@IVAUtilidadAIU " +
                        ",@PorIVAUtilidadAIU " +
                        ",@DiasEntrega" +
                        ",@TipoAprobSobreejecucion " +
                        ",@UsuarioRevSobreejec " +
                        ",@FechaRevSobreejec " +
                        ",@UsuarioAprSobreejec " +
                        ",@FechaAprSobreejec " +
                        ",@CantidadINV " +
                        ") SET @ConsecutivoDetaID = SCOPE_IDENTITY()";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@Descriptivo", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@SerialID", SqlDbType.Char, UDT_SerialID.MaxLength);
                mySqlCommand.Parameters.Add("@OrigenMonetario", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@MonedaID", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@UnidadInvID", SqlDbType.Char, UDT_UnidadInvID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpaqueInvID", SqlDbType.Char, UDT_EmpaqueInvID.MaxLength);
                mySqlCommand.Parameters.Add("@CantidadSol", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadEMP", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorUni", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVAUni", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorTotML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IvaTotML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorTotME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IvaTotME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@SolicitudDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@LegalizaDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@OrdCompraDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ContratoDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@InventarioDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ActivoDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FacturaDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@RecibidoDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Documento1ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Documento2ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Documento3ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Documento4ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Documento5ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@SolicitudDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@LegalizaDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@OrdCompraDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ContratoDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@InventarioDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@RecibidoDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Detalle1ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Detalle2ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Detalle3ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Detalle4ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Detalle5ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CantidadDoc1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadDoc2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadDoc3", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadDoc4", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadDoc5", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal01", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra01", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal02", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra02", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal03", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra03", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal04", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra04", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal05", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra05", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal06", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra06", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal07", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra07", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal08", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra08", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal09", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra09", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal10", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra10", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inRefParametro1", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inRefParametro2", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inUnidad", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inEmpaque", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@ConsecutivoDetaID", SqlDbType.Int, 1);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@CantidadOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadRec", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadCont", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorcentajeIVA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorBaseAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrIVAAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CodigoAdminAIU", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@ValorAdminAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVAAdminAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorIVAAdminAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CodigoImprevAIU", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@ValorImprevAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVAImprevAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorIVAImprevAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CodigoUtilidadAIU", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@ValorUtilidadAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVAUtilidadAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorIVAUtilidadAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DiasEntrega", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@TipoAprobSobreejecucion", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@UsuarioRevSobreejec", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaRevSobreejec", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@UsuarioAprSobreejec", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaAprSobreejec", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@CantidadINV", SqlDbType.Int);
                mySqlCommand.Parameters["@ConsecutivoDetaID"].Direction = ParameterDirection.Output;
                #endregion
                #region Asignacion de Valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = rec.NumeroDoc.Value;
                mySqlCommand.Parameters["@CodigoBSID"].Value = rec.CodigoBSID.Value;
                mySqlCommand.Parameters["@Descriptivo"].Value = rec.Descriptivo.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = rec.inReferenciaID.Value;
                mySqlCommand.Parameters["@LineaPresupuestoID"].Value = rec.LineaPresupuestoID.Value;
                mySqlCommand.Parameters["@EstadoInv"].Value = rec.EstadoInv.Value;
                mySqlCommand.Parameters["@Parametro1"].Value = rec.Parametro1.Value;
                mySqlCommand.Parameters["@Parametro2"].Value = rec.Parametro2.Value;
                mySqlCommand.Parameters["@ActivoID"].Value = rec.ActivoID.Value;
                mySqlCommand.Parameters["@SerialID"].Value = rec.SerialID.Value;
                mySqlCommand.Parameters["@OrigenMonetario"].Value = rec.OrigenMonetario.Value;
                mySqlCommand.Parameters["@MonedaID"].Value = rec.MonedaID.Value;
                mySqlCommand.Parameters["@UnidadInvID"].Value = rec.UnidadInvID.Value;
                mySqlCommand.Parameters["@EmpaqueInvID"].Value = rec.EmpaqueInvID.Value;
                mySqlCommand.Parameters["@CantidadSol"].Value = rec.CantidadSol.Value;
                mySqlCommand.Parameters["@CantidadEMP"].Value = rec.CantidadEMP.Value;
                mySqlCommand.Parameters["@ValorUni"].Value = rec.ValorUni.Value;
                mySqlCommand.Parameters["@IVAUni"].Value = rec.IVAUni.Value;
                mySqlCommand.Parameters["@ValorTotML"].Value = rec.ValorTotML.Value;
                mySqlCommand.Parameters["@IvaTotML"].Value = rec.IvaTotML.Value;
                mySqlCommand.Parameters["@ValorTotME"].Value = rec.ValorTotME.Value;
                mySqlCommand.Parameters["@IvaTotME"].Value = rec.IvaTotME.Value;
                mySqlCommand.Parameters["@SolicitudDocuID"].Value = rec.SolicitudDocuID.Value;
                mySqlCommand.Parameters["@LegalizaDocuID"].Value = rec.LegalizaDocuID.Value;
                mySqlCommand.Parameters["@OrdCompraDocuID"].Value = rec.OrdCompraDocuID.Value;
                mySqlCommand.Parameters["@ContratoDocuID"].Value = rec.ContratoDocuID.Value;
                mySqlCommand.Parameters["@InventarioDocuID"].Value = rec.InventarioDocuID.Value;
                mySqlCommand.Parameters["@RecibidoDocuID"].Value = rec.RecibidoDocuID.Value;
                mySqlCommand.Parameters["@ActivoDocuID"].Value = rec.ActivoDocuID.Value;
                mySqlCommand.Parameters["@FacturaDocuID"].Value = rec.FacturaDocuID.Value;
                mySqlCommand.Parameters["@Documento1ID"].Value = rec.Documento1ID.Value;
                mySqlCommand.Parameters["@Documento2ID"].Value = rec.Documento2ID.Value;
                mySqlCommand.Parameters["@Documento3ID"].Value = rec.Documento3ID.Value;
                mySqlCommand.Parameters["@Documento4ID"].Value = rec.Documento4ID.Value;
                mySqlCommand.Parameters["@Documento5ID"].Value = rec.Documento5ID.Value;
                mySqlCommand.Parameters["@SolicitudDetaID"].Value = rec.SolicitudDetaID.Value;
                mySqlCommand.Parameters["@LegalizaDetaID"].Value = rec.LegalizaDetaID.Value;
                mySqlCommand.Parameters["@OrdCompraDetaID"].Value = rec.OrdCompraDetaID.Value;
                mySqlCommand.Parameters["@ContratoDetaID"].Value = rec.ContratoDetaID.Value;
                mySqlCommand.Parameters["@InventarioDetaID"].Value = rec.InventarioDetaID.Value;
                mySqlCommand.Parameters["@RecibidoDetaID"].Value = rec.RecibidoDetaID.Value;
                mySqlCommand.Parameters["@Detalle1ID"].Value = rec.Detalle1ID.Value;
                mySqlCommand.Parameters["@Detalle2ID"].Value = rec.Detalle2ID.Value;
                mySqlCommand.Parameters["@Detalle3ID"].Value = rec.Detalle3ID.Value;
                mySqlCommand.Parameters["@Detalle4ID"].Value = rec.Detalle4ID.Value;
                mySqlCommand.Parameters["@Detalle5ID"].Value = rec.Detalle5ID.Value;
                mySqlCommand.Parameters["@CantidadDoc1"].Value = rec.CantidadDoc1.Value;
                mySqlCommand.Parameters["@CantidadDoc2"].Value = rec.CantidadDoc2.Value;
                mySqlCommand.Parameters["@CantidadDoc3"].Value = rec.CantidadDoc3.Value;
                mySqlCommand.Parameters["@CantidadDoc4"].Value = rec.CantidadDoc4.Value;
                mySqlCommand.Parameters["@CantidadDoc5"].Value = rec.CantidadDoc5.Value;
                mySqlCommand.Parameters["@VlrLocal01"].Value = rec.VlrLocal01.Value;
                mySqlCommand.Parameters["@VlrExtra01"].Value = rec.VlrExtra01.Value;
                mySqlCommand.Parameters["@VlrLocal02"].Value = rec.VlrLocal02.Value;
                mySqlCommand.Parameters["@VlrExtra02"].Value = rec.VlrExtra02.Value;
                mySqlCommand.Parameters["@VlrLocal03"].Value = rec.VlrLocal03.Value;
                mySqlCommand.Parameters["@VlrExtra03"].Value = rec.VlrExtra03.Value;
                mySqlCommand.Parameters["@VlrLocal04"].Value = rec.VlrLocal04.Value;
                mySqlCommand.Parameters["@VlrExtra04"].Value = rec.VlrExtra04.Value;
                mySqlCommand.Parameters["@VlrLocal05"].Value = rec.VlrLocal05.Value;
                mySqlCommand.Parameters["@VlrExtra05"].Value = rec.VlrExtra05.Value;
                mySqlCommand.Parameters["@VlrLocal06"].Value = rec.VlrLocal06.Value;
                mySqlCommand.Parameters["@VlrExtra06"].Value = rec.VlrExtra06.Value;
                mySqlCommand.Parameters["@VlrLocal07"].Value = rec.VlrLocal07.Value;
                mySqlCommand.Parameters["@VlrExtra07"].Value = rec.VlrExtra07.Value;
                mySqlCommand.Parameters["@VlrLocal08"].Value = rec.VlrLocal08.Value;
                mySqlCommand.Parameters["@VlrExtra08"].Value = rec.VlrExtra08.Value;
                mySqlCommand.Parameters["@VlrLocal09"].Value = rec.VlrLocal09.Value;
                mySqlCommand.Parameters["@VlrExtra09"].Value = rec.VlrExtra09.Value;
                mySqlCommand.Parameters["@VlrLocal10"].Value = rec.VlrLocal10.Value;
                mySqlCommand.Parameters["@VlrExtra10"].Value = rec.VlrExtra10.Value;
                mySqlCommand.Parameters["@DatoAdd1"].Value = rec.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd2"].Value = rec.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd3"].Value = rec.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd4"].Value = rec.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd5"].Value = rec.DatoAdd1.Value;
                mySqlCommand.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inRefParametro1"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inRefParametro1, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inRefParametro2"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inRefParametro2, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inUnidad"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inUnidad, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inEmpaque"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inEmpaque, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@CantidadOC"].Value = rec.CantidadOC.Value;
                mySqlCommand.Parameters["@CantidadRec"].Value = rec.CantidadRec.Value;
                mySqlCommand.Parameters["@CantidadCont"].Value = rec.CantidadCont.Value;
                mySqlCommand.Parameters["@PorcentajeIVA"].Value = rec.PorcentajeIVA.Value;
                mySqlCommand.Parameters["@ValorBaseAIU"].Value = rec.ValorBaseAIU.Value;
                mySqlCommand.Parameters["@ValorAIU"].Value = rec.ValorAIU.Value;
                mySqlCommand.Parameters["@VlrIVAAIU"].Value = rec.VlrIVAAIU.Value;
                mySqlCommand.Parameters["@CodigoAdminAIU"].Value = rec.CodigoAdminAIU.Value;
                mySqlCommand.Parameters["@ValorAdminAIU"].Value = rec.ValorAdminAIU.Value;
                mySqlCommand.Parameters["@IVAAdminAIU"].Value = rec.IVAAdminAIU.Value;
                mySqlCommand.Parameters["@PorIVAAdminAIU"].Value = rec.PorIVAAdminAIU.Value;
                mySqlCommand.Parameters["@CodigoImprevAIU"].Value = rec.CodigoImprevAIU.Value;
                mySqlCommand.Parameters["@ValorImprevAIU"].Value = rec.ValorImprevAIU.Value;
                mySqlCommand.Parameters["@IVAImprevAIU"].Value = rec.IVAImprevAIU.Value;
                mySqlCommand.Parameters["@PorIVAImprevAIU"].Value = rec.PorIVAImprevAIU.Value;
                mySqlCommand.Parameters["@CodigoUtilidadAIU"].Value = rec.CodigoUtilidadAIU.Value;
                mySqlCommand.Parameters["@ValorUtilidadAIU"].Value = rec.ValorUtilidadAIU.Value;
                mySqlCommand.Parameters["@IVAUtilidadAIU"].Value = rec.IVAUtilidadAIU.Value;
                mySqlCommand.Parameters["@PorIVAUtilidadAIU"].Value = rec.PorIVAUtilidadAIU.Value;
                mySqlCommand.Parameters["@DiasEntrega"].Value = rec.DiasEntrega.Value;
                mySqlCommand.Parameters["@TipoAprobSobreejecucion"].Value = rec.TipoAprobSobreejecucion.Value;
                mySqlCommand.Parameters["@UsuarioRevSobreejec"].Value = rec.UsuarioRevSobreejec.Value;
                mySqlCommand.Parameters["@FechaRevSobreejec"].Value = rec.FechaRevSobreejec.Value;
                mySqlCommand.Parameters["@UsuarioAprSobreejec"].Value = rec.UsuarioAprSobreejec.Value;
                mySqlCommand.Parameters["@FechaAprSobreejec"].Value = rec.FechaAprSobreejec.Value;
                mySqlCommand.Parameters["@CantidadINV"].Value = rec.CantidadINV.Value;
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
                int recId = Convert.ToInt32(mySqlCommand.Parameters["@ConsecutivoDetaID"].Value);

                return recId;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_prDetalleDocu", "DAL_prDetalleDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Edita un registro de recibidos
        /// </summary>
        /// <param name="docCtrl">Documento que se va a editar</param>
        public void DAL_prDetalleDocu_Update(DTO_prDetalleDocu rec)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "UPDATE prDetalleDocu SET" +
                    "   NumeroDoc = @NumeroDoc, " +
                    "   CodigoBSID = @CodigoBSID," +
                    "   Descriptivo = @Descriptivo," +
                    "   inReferenciaID = @inReferenciaID, " +
                    "   LineaPresupuestoID = @LineaPresupuestoID, " +
                    "   EstadoInv = @EstadoInv," +
                    "   Parametro1 = @Parametro1," +
                    "   Parametro2 = @Parametro2," +
                    "   ActivoID = @ActivoID, " +
                    "   SerialID = @SerialID, " +
                    "   OrigenMonetario = @OrigenMonetario," +
                    "   MonedaID = @MonedaID, " +
                    "   UnidadInvID = @UnidadInvID, " +
                    "   EmpaqueInvID = @EmpaqueInvID, " +
                    "   CantidadSol = @CantidadSol, " +
                    "   CantidadEMP = @CantidadEMP, " +
                    "   ValorUni = @ValorUni, " +
                    "   IVAUni = @IVAUni, " +
                    "   ValorTotML = @ValorTotML, " +
                    "   IvaTotML = @IvaTotML, " +
                    "   ValorTotME = @ValorTotME, " +
                    "   IvaTotME = @IvaTotME, " +
                    "   SolicitudDocuID = @SolicitudDocuID, " +
                    "   LegalizaDocuID = @LegalizaDocuID, " +
                    "   OrdCompraDocuID = @OrdCompraDocuID, " +
                    "   ContratoDocuID = @ContratoDocuID, " +
                    "   InventarioDocuID = @InventarioDocuID, " +
                    "   RecibidoDocuID = @RecibidoDocuID, " +
                    "   ActivoDocuID = @ActivoDocuID, " +
                    "   FacturaDocuID = @FacturaDocuID, " +
                    "   Documento1ID = @Documento1ID, " +
                    "   Documento2ID = @Documento2ID, " +
                    "   Documento3ID = @Documento3ID, " +
                    "   Documento4ID = @Documento4ID, " +
                    "   Documento5ID = @Documento5ID, " +
                    "   SolicitudDetaID = @SolicitudDetaID, " +
                    "   LegalizaDetaID = @LegalizaDetaID, " +
                    "   OrdCompraDetaID = @OrdCompraDetaID, " +
                    "   ContratoDetaID = @ContratoDetaID, " +
                    "   InventarioDetaID = @InventarioDetaID, " +
                    "   RecibidoDetaID = @RecibidoDetaID, " +
                    "   Detalle1ID = @Detalle1ID, " +
                    "   Detalle2ID = @Detalle2ID, " +
                    "   Detalle3ID = @Detalle3ID, " +
                    "   Detalle4ID = @Detalle4ID, " +
                    "   Detalle5ID = @Detalle5ID, " +
                    "   CantidadDoc1 = @CantidadDoc1," +
                    "   CantidadDoc2 = @CantidadDoc2," +
                    "   CantidadDoc3= @CantidadDoc3," +
                    "   CantidadDoc4 = @CantidadDoc4," +
                    "   CantidadDoc5 = @CantidadDoc5," +
                    "   VlrLocal01 = @VlrLocal01, " +
                    "   VlrExtra01 = @VlrExtra01, " +
                    "   VlrLocal02 = @VlrLocal02, " +
                    "   VlrExtra02 = @VlrExtra02, " +
                    "   VlrLocal03 = @VlrLocal03, " +
                    "   VlrExtra03 = @VlrExtra03, " +
                    "   VlrLocal04 = @VlrLocal04, " +
                    "   VlrExtra04 = @VlrExtra04, " +
                    "   VlrLocal05 = @VlrLocal05, " +
                    "   VlrExtra05 = @VlrExtra05, " +
                    "   VlrLocal06 = @VlrLocal06, " +
                    "   VlrExtra06 = @VlrExtra06, " +
                    "   VlrLocal07 = @VlrLocal07, " +
                    "   VlrExtra07 = @VlrExtra07, " +
                    "   VlrLocal08 = @VlrLocal08, " +
                    "   VlrExtra08 = @VlrExtra08, " +
                    "   VlrLocal09 = @VlrLocal09, " +
                    "   VlrExtra09 = @VlrExtra09, " +
                    "   VlrLocal10 = @VlrLocal10, " +
                    "   VlrExtra10 = @VlrExtra10, " +
                    "   DatoAdd1 = @DatoAdd1, " +
                    "   DatoAdd2 = @DatoAdd2, " +
                    "   DatoAdd3 = @DatoAdd3, " +
                    "   DatoAdd4 = @DatoAdd4, " +
                    "   DatoAdd5 = @DatoAdd5, " +
                    "   CantidadOC = @CantidadOC, " +
                    "   CantidadRec = @CantidadRec, " +
                    "   CantidadCont = @CantidadCont, " +
                    "   PorcentajeIVA = @PorcentajeIVA, " +
                    "   ValorBaseAIU = @ValorBaseAIU, " +
                    "   ValorAIU = @ValorAIU, " +
                    "   VlrIVAAIU = @VlrIVAAIU, " +
                    "   CodigoAdminAIU = @CodigoAdminAIU, " +
                    "   ValorAdminAIU = @ValorAdminAIU, " +
                    "   IVAAdminAIU = @IVAAdminAIU, " +
                    "   PorIVAAdminAIU = @PorIVAAdminAIU, " +
                    "   CodigoImprevAIU = @CodigoImprevAIU, " +
                    "   ValorImprevAIU = @ValorImprevAIU, " +
                    "   IVAImprevAIU = @IVAImprevAIU, " +
                    "   PorIVAImprevAIU = @PorIVAImprevAIU, " +
                    "   CodigoUtilidadAIU = @CodigoUtilidadAIU, " +
                    "   ValorUtilidadAIU = @ValorUtilidadAIU, " +
                    "   IVAUtilidadAIU = @IVAUtilidadAIU, " +
                    "   PorIVAUtilidadAIU = @PorIVAUtilidadAIU, " +
                    "   DiasEntrega = @DiasEntrega, " +
                    "   TipoAprobSobreejecucion = @TipoAprobSobreejecucion, " +
                    "   UsuarioRevSobreejec = @UsuarioRevSobreejec, " +
                    "   FechaRevSobreejec = @FechaRevSobreejec, " +
                    "   UsuarioAprSobreejec = @UsuarioAprSobreejec, " +
                    "   FechaAprSobreejec = @FechaAprSobreejec, " +
                    "   CantidadINV = @CantidadINV " +
                    "WHERE ConsecutivoDetaID = @ConsecutivoDetaID ";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ConsecutivoDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@Descriptivo", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@SerialID", SqlDbType.Char, UDT_SerialID.MaxLength);
                mySqlCommand.Parameters.Add("@OrigenMonetario", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@MonedaID", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@UnidadInvID", SqlDbType.Char, UDT_UnidadInvID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpaqueInvID", SqlDbType.Char, UDT_EmpaqueInvID.MaxLength);
                mySqlCommand.Parameters.Add("@CantidadSol", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadEMP", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorUni", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVAUni", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorTotML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IvaTotML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorTotME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IvaTotME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@SolicitudDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@LegalizaDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@OrdCompraDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ContratoDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@InventarioDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ActivoDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FacturaDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@RecibidoDocuID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Documento1ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Documento2ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Documento3ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Documento4ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Documento5ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@SolicitudDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@LegalizaDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@OrdCompraDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ContratoDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@InventarioDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@RecibidoDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Detalle1ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Detalle2ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Detalle3ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Detalle4ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Detalle5ID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CantidadDoc1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadDoc2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadDoc3", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadDoc4", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadDoc5", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal01", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra01", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal02", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra02", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal03", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra03", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal04", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra04", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal05", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra05", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal06", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra06", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal07", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra07", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal08", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra08", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal09", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra09", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrLocal10", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrExtra10", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@CantidadOC", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadRec", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadCont", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorcentajeIVA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorBaseAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrIVAAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CodigoAdminAIU", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@ValorAdminAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVAAdminAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorIVAAdminAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CodigoImprevAIU", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@ValorImprevAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVAImprevAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorIVAImprevAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CodigoUtilidadAIU", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@ValorUtilidadAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVAUtilidadAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorIVAUtilidadAIU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DiasEntrega", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@TipoAprobSobreejecucion", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@UsuarioRevSobreejec", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaRevSobreejec", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@UsuarioAprSobreejec", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaAprSobreejec", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@CantidadINV", SqlDbType.Int);
                #endregion
                #region Asignacion de Valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = rec.NumeroDoc.Value;
                mySqlCommand.Parameters["@ConsecutivoDetaID"].Value = rec.ConsecutivoDetaID.Value;
                mySqlCommand.Parameters["@CodigoBSID"].Value = rec.CodigoBSID.Value;
                mySqlCommand.Parameters["@Descriptivo"].Value = rec.Descriptivo.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = rec.inReferenciaID.Value;
                mySqlCommand.Parameters["@LineaPresupuestoID"].Value = rec.LineaPresupuestoID.Value;
                mySqlCommand.Parameters["@EstadoInv"].Value = rec.EstadoInv.Value;
                mySqlCommand.Parameters["@Parametro1"].Value = rec.Parametro1.Value;
                mySqlCommand.Parameters["@Parametro2"].Value = rec.Parametro2.Value;
                mySqlCommand.Parameters["@ActivoID"].Value = rec.ActivoID.Value;
                mySqlCommand.Parameters["@SerialID"].Value = rec.SerialID.Value;
                mySqlCommand.Parameters["@OrigenMonetario"].Value = rec.OrigenMonetario.Value;
                mySqlCommand.Parameters["@MonedaID"].Value = rec.MonedaID.Value;
                mySqlCommand.Parameters["@UnidadInvID"].Value = rec.UnidadInvID.Value;
                mySqlCommand.Parameters["@EmpaqueInvID"].Value = rec.EmpaqueInvID.Value;
                mySqlCommand.Parameters["@CantidadSol"].Value = rec.CantidadSol.Value;
                mySqlCommand.Parameters["@CantidadEMP"].Value = rec.CantidadEMP.Value;
                mySqlCommand.Parameters["@ValorUni"].Value = rec.ValorUni.Value;
                mySqlCommand.Parameters["@IVAUni"].Value = rec.IVAUni.Value;
                mySqlCommand.Parameters["@ValorTotML"].Value = rec.ValorTotML.Value;
                mySqlCommand.Parameters["@IvaTotML"].Value = rec.IvaTotML.Value;
                mySqlCommand.Parameters["@ValorTotME"].Value = rec.ValorTotME.Value;
                mySqlCommand.Parameters["@IvaTotME"].Value = rec.IvaTotME.Value;
                mySqlCommand.Parameters["@SolicitudDocuID"].Value = rec.SolicitudDocuID.Value;
                mySqlCommand.Parameters["@LegalizaDocuID"].Value = rec.LegalizaDocuID.Value;
                mySqlCommand.Parameters["@OrdCompraDocuID"].Value = rec.OrdCompraDocuID.Value;
                mySqlCommand.Parameters["@ContratoDocuID"].Value = rec.ContratoDocuID.Value;
                mySqlCommand.Parameters["@InventarioDocuID"].Value = rec.InventarioDocuID.Value;
                mySqlCommand.Parameters["@RecibidoDocuID"].Value = rec.RecibidoDocuID.Value;
                mySqlCommand.Parameters["@ActivoDocuID"].Value = rec.ActivoDocuID.Value;
                mySqlCommand.Parameters["@FacturaDocuID"].Value = rec.FacturaDocuID.Value;
                mySqlCommand.Parameters["@Documento1ID"].Value = rec.Documento1ID.Value;
                mySqlCommand.Parameters["@Documento2ID"].Value = rec.Documento2ID.Value;
                mySqlCommand.Parameters["@Documento3ID"].Value = rec.Documento3ID.Value;
                mySqlCommand.Parameters["@Documento4ID"].Value = rec.Documento4ID.Value;
                mySqlCommand.Parameters["@Documento5ID"].Value = rec.Documento5ID.Value;
                mySqlCommand.Parameters["@SolicitudDetaID"].Value = rec.SolicitudDetaID.Value;
                mySqlCommand.Parameters["@LegalizaDetaID"].Value = rec.LegalizaDetaID.Value;
                mySqlCommand.Parameters["@OrdCompraDetaID"].Value = rec.OrdCompraDetaID.Value;
                mySqlCommand.Parameters["@ContratoDetaID"].Value = rec.ContratoDetaID.Value;
                mySqlCommand.Parameters["@InventarioDetaID"].Value = rec.InventarioDetaID.Value;
                mySqlCommand.Parameters["@RecibidoDetaID"].Value = rec.RecibidoDetaID.Value;
                mySqlCommand.Parameters["@Detalle1ID"].Value = rec.Detalle1ID.Value;
                mySqlCommand.Parameters["@Detalle2ID"].Value = rec.Detalle2ID.Value;
                mySqlCommand.Parameters["@Detalle3ID"].Value = rec.Detalle3ID.Value;
                mySqlCommand.Parameters["@Detalle4ID"].Value = rec.Detalle4ID.Value;
                mySqlCommand.Parameters["@Detalle5ID"].Value = rec.Detalle5ID.Value;
                mySqlCommand.Parameters["@CantidadDoc1"].Value = rec.CantidadDoc1.Value;
                mySqlCommand.Parameters["@CantidadDoc2"].Value = rec.CantidadDoc2.Value;
                mySqlCommand.Parameters["@CantidadDoc3"].Value = rec.CantidadDoc3.Value;
                mySqlCommand.Parameters["@CantidadDoc4"].Value = rec.CantidadDoc4.Value;
                mySqlCommand.Parameters["@CantidadDoc5"].Value = rec.CantidadDoc5.Value;
                mySqlCommand.Parameters["@VlrLocal01"].Value = rec.VlrLocal01.Value;
                mySqlCommand.Parameters["@VlrExtra01"].Value = rec.VlrExtra01.Value;
                mySqlCommand.Parameters["@VlrLocal02"].Value = rec.VlrLocal02.Value;
                mySqlCommand.Parameters["@VlrExtra02"].Value = rec.VlrExtra02.Value;
                mySqlCommand.Parameters["@VlrLocal03"].Value = rec.VlrLocal03.Value;
                mySqlCommand.Parameters["@VlrExtra03"].Value = rec.VlrExtra03.Value;
                mySqlCommand.Parameters["@VlrLocal04"].Value = rec.VlrLocal04.Value;
                mySqlCommand.Parameters["@VlrExtra04"].Value = rec.VlrExtra04.Value;
                mySqlCommand.Parameters["@VlrLocal05"].Value = rec.VlrLocal05.Value;
                mySqlCommand.Parameters["@VlrExtra05"].Value = rec.VlrExtra05.Value;
                mySqlCommand.Parameters["@VlrLocal06"].Value = rec.VlrLocal06.Value;
                mySqlCommand.Parameters["@VlrExtra06"].Value = rec.VlrExtra06.Value;
                mySqlCommand.Parameters["@VlrLocal07"].Value = rec.VlrLocal07.Value;
                mySqlCommand.Parameters["@VlrExtra07"].Value = rec.VlrExtra07.Value;
                mySqlCommand.Parameters["@VlrLocal08"].Value = rec.VlrLocal08.Value;
                mySqlCommand.Parameters["@VlrExtra08"].Value = rec.VlrExtra08.Value;
                mySqlCommand.Parameters["@VlrLocal09"].Value = rec.VlrLocal09.Value;
                mySqlCommand.Parameters["@VlrExtra09"].Value = rec.VlrExtra09.Value;
                mySqlCommand.Parameters["@VlrLocal10"].Value = rec.VlrLocal10.Value;
                mySqlCommand.Parameters["@VlrExtra10"].Value = rec.VlrExtra10.Value;
                mySqlCommand.Parameters["@DatoAdd1"].Value = rec.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd2"].Value = rec.DatoAdd2.Value;
                mySqlCommand.Parameters["@DatoAdd3"].Value = rec.DatoAdd3.Value;
                mySqlCommand.Parameters["@DatoAdd4"].Value = rec.DatoAdd4.Value;
                mySqlCommand.Parameters["@DatoAdd5"].Value = rec.DatoAdd5.Value;
                mySqlCommand.Parameters["@CantidadOC"].Value = rec.CantidadOC.Value;
                mySqlCommand.Parameters["@CantidadRec"].Value = rec.CantidadRec.Value;
                mySqlCommand.Parameters["@CantidadCont"].Value = rec.CantidadCont.Value;
                mySqlCommand.Parameters["@PorcentajeIVA"].Value = rec.PorcentajeIVA.Value;
                mySqlCommand.Parameters["@ValorBaseAIU"].Value = rec.ValorBaseAIU.Value;
                mySqlCommand.Parameters["@ValorAIU"].Value = rec.ValorAIU.Value;
                mySqlCommand.Parameters["@VlrIVAAIU"].Value = rec.VlrIVAAIU.Value;
                mySqlCommand.Parameters["@CodigoAdminAIU"].Value = rec.CodigoAdminAIU.Value;
                mySqlCommand.Parameters["@ValorAdminAIU"].Value = rec.ValorAdminAIU.Value;
                mySqlCommand.Parameters["@IVAAdminAIU"].Value = rec.IVAAdminAIU.Value;
                mySqlCommand.Parameters["@PorIVAAdminAIU"].Value = rec.PorIVAAdminAIU.Value;
                mySqlCommand.Parameters["@CodigoImprevAIU"].Value = rec.CodigoImprevAIU.Value;
                mySqlCommand.Parameters["@ValorImprevAIU"].Value = rec.ValorImprevAIU.Value;
                mySqlCommand.Parameters["@IVAImprevAIU"].Value = rec.IVAImprevAIU.Value;
                mySqlCommand.Parameters["@PorIVAImprevAIU"].Value = rec.PorIVAImprevAIU.Value;
                mySqlCommand.Parameters["@CodigoUtilidadAIU"].Value = rec.CodigoUtilidadAIU.Value;
                mySqlCommand.Parameters["@ValorUtilidadAIU"].Value = rec.ValorUtilidadAIU.Value;
                mySqlCommand.Parameters["@IVAUtilidadAIU"].Value = rec.IVAUtilidadAIU.Value;
                mySqlCommand.Parameters["@PorIVAUtilidadAIU"].Value = rec.PorIVAUtilidadAIU.Value;
                mySqlCommand.Parameters["@DiasEntrega"].Value = rec.DiasEntrega.Value;
                mySqlCommand.Parameters["@TipoAprobSobreejecucion"].Value = rec.TipoAprobSobreejecucion.Value;
                mySqlCommand.Parameters["@UsuarioRevSobreejec"].Value = rec.UsuarioRevSobreejec.Value;
                mySqlCommand.Parameters["@FechaRevSobreejec"].Value = rec.FechaRevSobreejec.Value;
                mySqlCommand.Parameters["@UsuarioAprSobreejec"].Value = rec.UsuarioAprSobreejec.Value;
                mySqlCommand.Parameters["@FechaAprSobreejec"].Value = rec.FechaAprSobreejec.Value;
                mySqlCommand.Parameters["@CantidadINV"].Value = rec.CantidadINV.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DTO_prDetalleDocu_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la lista de prDetalleDocu segun el numero del documento
        /// </summary>
        /// <param name="NumeroDoc">Numero del documento</param>
        /// <param name="isFactura">Valida si se filtra por identificador de FacturaDocuID</param>
        /// <returns>Lista de detalle</returns>
        public List<DTO_prDetalleDocu> DAL_prDetalleDocu_GetByNumeroDoc(int NumeroDoc, bool isFactura)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                if (isFactura)
                {
                    mySqlCommand.CommandText = " Select det.*, ref.RefProveedor,ref.MarcaInvID,IsNull(empaque.Cantidad,1) as CantidadxEmpaque from prDetalleDocu det with(nolock)  " +
                                               " left join inReferencia ref with(nolock) On ref.inReferenciaID = det.inReferenciaID and ref.EmpresaGrupoID = det.eg_inReferencia     " +
                                               " left join inEmpaque empaque with(nolock) On empaque.EmpaqueInvID = det.EmpaqueInvID and empaque.EmpresaGrupoID = det.eg_inEmpaque  " +
                                               " where det.FacturaDocuID = @FacturaDocuID";
                                    
                    mySqlCommand.Parameters.Add("@FacturaDocuID", SqlDbType.Int);
                    mySqlCommand.Parameters["@FacturaDocuID"].Value = NumeroDoc;

                }
                else
                {
                    mySqlCommand.CommandText = "select det.*,ref.RefProveedor,ref.MarcaInvID,IsNull(empaque.Cantidad,1) as CantidadxEmpaque from prDetalleDocu det with(nolock) " +
                                                " left join inReferencia ref with(nolock) On ref.inReferenciaID = det.inReferenciaID and ref.EmpresaGrupoID = det.eg_inReferencia  " +
                                                " left join inEmpaque empaque with(nolock) On empaque.EmpaqueInvID = det.EmpaqueInvID and empaque.EmpresaGrupoID = det.eg_inEmpaque  " +
                                                "where det.NumeroDoc = @NumeroDoc";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                }

                List<DTO_prDetalleDocu> footer = new List<DTO_prDetalleDocu>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_prDetalleDocu detail = new DTO_prDetalleDocu(dr);
                    detail.MarcaInvID.Value = dr["MarcaInvID"].ToString();
                    detail.RefProveedor.Value = dr["RefProveedor"].ToString();
                    detail.CantidadxEmpaque.Value = Convert.ToInt32(dr["CantidadxEmpaque"]);
                    detail.Index = index;
                    footer.Add(detail);
                    index++;
                }
                dr.Close();
                return footer;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_prDetalleDocu", "DAL_prDetalleDocu_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de Recibidos aprobados pendientes para generar el detalle de cargos
        /// </summary>
        /// <returns>Listado de recibidos aprobados</returns>
        public List<DTO_prRecibidoAprob> DAL_prDetalleDocu_GetPendientesCargosRecib()
        {
            try
            {
                List<DTO_prRecibidoAprob> result = new List<DTO_prRecibidoAprob>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Common parameters
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = ModulesPrefix.pr;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@DocumentoID"].Value = AppDocuments.Recibido ;

                #endregion

                mySqlCommand.CommandText =
                "select distinct '' as PrefijoID, ctrl.EmpresaID,ctrl.FechaDoc Fecha,det.ConsecutivoDetaID,doc.DocumentoID, ctrl.NumeroDoc,ctrl.DocumentoNro,usr.UsuarioID,rec.ProveedorID, '' as ProveedorNombre," +
                    "'' as MonedaID,0 as TasaCambioDOCU,'' as ObservacionDesc,det.RecibidoDocuID,det.RecibidoDetaID,det.OrdCompraDocuID,det.OrdCompraDetaID," +
                    "det.SolicitudDetaID,det.SolicitudDocuID,det.CantidadRec, det.CodigoBSID,det.ValorUni,det.IVAUni,'' as Descriptivo,'' as inReferenciaID,'' as SerialID,'' as UnidadInvID " +
                    "det.ValorTotML CostoML,det.IvaTotML CostoIvaML, det.ValorTotME CostoME, det.IvaTotME CostoIvaME "+
                 "from glDocumentoControl ctrl with(nolock) " +
                    "inner join prDetalleDocu   det with(nolock) on ctrl.NumeroDoc = det.NumeroDoc " +
                    "inner join glDocumento     doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID 	" +
                    "inner join seUsuario       usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID   " +
                    "inner join prRecibidoDocu  rec with(nolock) on det.NumeroDoc = rec.NumeroDoc " +
                "where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado and ctrl.DocumentoID = @DocumentoID " +
                    "and not exists(select null from prDetalleCargos detCargos where ctrl.NumeroDoc  = detCargos.NumeroDoc) " +
                "group By ctrl.EmpresaID,ctrl.FechaDoc,det.ConsecutivoDetaID,doc.DocumentoID,ctrl.NumeroDoc,ctrl.DocumentoNro, usr.UsuarioID,  rec.ProveedorID," +
                     "det.RecibidoDocuID,det.RecibidoDetaID,det.OrdCompraDocuID,det.OrdCompraDetaID," +
                     "det.SolicitudDetaID,det.SolicitudDocuID,det.CantidadRec ,det.CodigoBSID,"+
                     "det.ValorTotML,det.IvaTotML, det.ValorTotME, det.IvaTotME,det.ValorUni,det.IVAUni ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_prRecibidoAprob dto = new DTO_prRecibidoAprob(dr);
                    DTO_prRecibidoAprobDet dtoDet = new DTO_prRecibidoAprobDet(dr);
                    dto.Detalle.Add(dtoDet);
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prDetalleDocu_GetPendientesCargosRecib");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public List<DTO_prDetalleDocu> DAL_prDetalleDocu_GetByParameter(DTO_prDetalleDocu filter)
        {
            try
            {
                List<DTO_prDetalleDocu> result = new List<DTO_prDetalleDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = "select * from prDetalleDocu with(nolock) " +
                                       "where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                if (!string.IsNullOrEmpty(filter.NumeroDoc.Value.ToString()))
                {
                    query += "and NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = filter.NumeroDoc.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.CodigoBSID.Value))
                {
                    query += "and CodigoBSID = @CodigoBSID ";
                    mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                    mySqlCommand.Parameters["@CodigoBSID"].Value = filter.CodigoBSID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.inReferenciaID.Value))
                {
                    query += "and inReferenciaID = @inReferenciaID ";
                    mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_ReferenciaID.MaxLength);
                    mySqlCommand.Parameters["@inReferenciaID"].Value = filter.inReferenciaID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.EstadoInv.Value.ToString()))
                {
                    query += "and EstadoInv = @EstadoInv ";
                    mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@EstadoInv"].Value = filter.EstadoInv.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Parametro1.Value.ToString()))
                {
                    query += "and Parametro1 = @Parametro1 ";
                    mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                    mySqlCommand.Parameters["@Parametro1"].Value = filter.Parametro1.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Parametro2.Value.ToString()))
                {
                    query += "and Parametro2 = @Parametro2 ";
                    mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                    mySqlCommand.Parameters["@Parametro2"].Value = filter.Parametro2.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ActivoID.Value.ToString()))
                {
                    query += "and ActivoID = @ActivoID ";
                    mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                    mySqlCommand.Parameters["@ActivoID"].Value = filter.ActivoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.SerialID.Value.ToString()))
                {
                    query += "and SerialID = @SerialID ";
                    mySqlCommand.Parameters.Add("@SerialID", SqlDbType.Char, UDT_SerialID.MaxLength);
                    mySqlCommand.Parameters["@SerialID"].Value = filter.SerialID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.EmpaqueInvID.Value.ToString()))
                {
                    query += "and EmpaqueInvID = @EmpaqueInvID ";
                    mySqlCommand.Parameters.Add("@EmpaqueInvID", SqlDbType.Char, UDT_EmpaqueInvID.MaxLength);
                    mySqlCommand.Parameters["@EmpaqueInvID"].Value = filter.EmpaqueInvID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.UnidadInvID.Value.ToString()))
                {
                    query += "and UnidadInvID = @UnidadInvID ";
                    mySqlCommand.Parameters.Add("@UnidadInvID", SqlDbType.Char, UDT_UnidadInvID.MaxLength);
                    mySqlCommand.Parameters["@UnidadInvID"].Value = filter.UnidadInvID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.SolicitudDocuID.Value.ToString()))
                {
                    query += "and SolicitudDocuID = @SolicitudDocuID ";
                    mySqlCommand.Parameters.Add("@SolicitudDocuID", SqlDbType.Int);
                    mySqlCommand.Parameters["@SolicitudDocuID"].Value = filter.SolicitudDocuID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.LegalizaDocuID.Value.ToString()))
                {
                    query += "and LegalizaDocuID = @LegalizaDocuID ";
                    mySqlCommand.Parameters.Add("@LegalizaDocuID", SqlDbType.Int);
                    mySqlCommand.Parameters["@LegalizaDocuID"].Value = filter.LegalizaDocuID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.OrdCompraDocuID.Value.ToString()))
                {
                    query += "and OrdCompraDocuID = @OrdCompraDocuID ";
                    mySqlCommand.Parameters.Add("@OrdCompraDocuID", SqlDbType.Int);
                    mySqlCommand.Parameters["@OrdCompraDocuID"].Value = filter.OrdCompraDocuID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ContratoDocuID.Value.ToString()))
                {
                    query += "and ContratoDocuID = @ContratoDocuID ";
                    mySqlCommand.Parameters.Add("@ContratoDocuID", SqlDbType.Int);
                    mySqlCommand.Parameters["@ContratoDocuID"].Value = filter.ContratoDocuID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.InventarioDocuID.Value.ToString()))
                {
                    query += "and InventarioDocuID = @InventarioDocuID ";
                    mySqlCommand.Parameters.Add("@InventarioDocuID", SqlDbType.Int);
                    mySqlCommand.Parameters["@InventarioDocuID"].Value = filter.InventarioDocuID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.RecibidoDocuID.Value.ToString()))
                {
                    query += "and RecibidoDocuID = @RecibidoDocuID ";
                    mySqlCommand.Parameters.Add("@RecibidoDocuID", SqlDbType.Int);
                    mySqlCommand.Parameters["@RecibidoDocuID"].Value = filter.RecibidoDocuID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ActivoDocuID.Value.ToString()))
                {
                    query += "and ActivoDocuID = @ActivoDocuID ";
                    mySqlCommand.Parameters.Add("@ActivoDocuID", SqlDbType.Int);
                    mySqlCommand.Parameters["@ActivoDocuID"].Value = filter.ActivoDocuID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.FacturaDocuID.Value.ToString()))
                {
                    query += "and FacturaDocuID = @FacturaDocuID ";
                    mySqlCommand.Parameters.Add("@FacturaDocuID", SqlDbType.Int);
                    mySqlCommand.Parameters["@FacturaDocuID"].Value = filter.FacturaDocuID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Documento1ID.Value.ToString())) //Consumo Proyecto
                {
                    query += "and Documento1ID = @Documento1ID ";
                    mySqlCommand.Parameters.Add("@Documento1ID", SqlDbType.Int);
                    mySqlCommand.Parameters["@Documento1ID"].Value = filter.Documento1ID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Documento2ID.Value.ToString())) //Solicitud Servicios(Proyectos)
                {
                    query += "and Documento2ID = @Documento2ID ";
                    mySqlCommand.Parameters.Add("@Documento2ID", SqlDbType.Int);
                    mySqlCommand.Parameters["@Documento2ID"].Value = filter.Documento2ID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Documento3ID.Value.ToString())) 
                {
                    query += "and Documento3ID = @Documento3ID ";
                    mySqlCommand.Parameters.Add("@Documento3ID", SqlDbType.Int);
                    mySqlCommand.Parameters["@Documento3ID"].Value = filter.Documento3ID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Documento4ID.Value.ToString())) 
                {
                    query += "and Documento4ID = @Documento4ID ";
                    mySqlCommand.Parameters.Add("@Documento4ID", SqlDbType.Int);
                    mySqlCommand.Parameters["@Documento4ID"].Value = filter.Documento4ID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Documento5ID.Value.ToString())) 
                {
                    query += "and Documento5ID = @Documento5ID ";
                    mySqlCommand.Parameters.Add("@Documento5ID", SqlDbType.Int);
                    mySqlCommand.Parameters["@Documento5ID"].Value = filter.Documento5ID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.SolicitudDetaID.Value.ToString()))
                {
                    query += "and SolicitudDetaID = @SolicitudDetaID ";
                    mySqlCommand.Parameters.Add("@SolicitudDetaID", SqlDbType.Int);
                    mySqlCommand.Parameters["@SolicitudDetaID"].Value = filter.SolicitudDetaID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.LegalizaDetaID.Value.ToString()))
                {
                    query += "and LegalizaDetaID = @LegalizaDetaID ";
                    mySqlCommand.Parameters.Add("@LegalizaDetaID", SqlDbType.Int);
                    mySqlCommand.Parameters["@LegalizaDetaID"].Value = filter.LegalizaDetaID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.OrdCompraDetaID.Value.ToString()))
                {
                    query += "and OrdCompraDetaID = @OrdCompraDetaID ";
                    mySqlCommand.Parameters.Add("@OrdCompraDetaID", SqlDbType.Int);
                    mySqlCommand.Parameters["@OrdCompraDetaID"].Value = filter.OrdCompraDetaID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ContratoDetaID.Value.ToString()))
                {
                    query += "and ContratoDetaID = @ContratoDetaID ";
                    mySqlCommand.Parameters.Add("@ContratoDetaID", SqlDbType.Int);
                    mySqlCommand.Parameters["@ContratoDetaID"].Value = filter.ContratoDetaID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.InventarioDetaID.Value.ToString()))
                {
                    query += "and InventarioDetaID = @InventarioDetaID ";
                    mySqlCommand.Parameters.Add("@InventarioDetaID", SqlDbType.Int);
                    mySqlCommand.Parameters["@InventarioDetaID"].Value = filter.InventarioDetaID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.RecibidoDetaID.Value.ToString()))
                {
                    query += "and RecibidoDetaID = @RecibidoDetaID ";
                    mySqlCommand.Parameters.Add("@RecibidoDetaID", SqlDbType.Int);
                    mySqlCommand.Parameters["@RecibidoDetaID"].Value = filter.RecibidoDetaID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Detalle1ID.Value.ToString()))
                {
                    query += "and Detalle1ID = @Detalle1ID ";
                    mySqlCommand.Parameters.Add("@Detalle1ID", SqlDbType.Int);
                    mySqlCommand.Parameters["@Detalle1ID"].Value = filter.Detalle1ID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Detalle2ID.Value.ToString()))
                {
                    query += "and Detalle2ID = @Detalle2ID ";
                    mySqlCommand.Parameters.Add("@Detalle2ID", SqlDbType.Int);
                    mySqlCommand.Parameters["@Detalle2ID"].Value = filter.Detalle2ID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Detalle3ID.Value.ToString()))
                {
                    query += "and Detalle3ID = @Detalle3ID ";
                    mySqlCommand.Parameters.Add("@Detalle3ID", SqlDbType.Int);
                    mySqlCommand.Parameters["@Detalle3ID"].Value = filter.Detalle3ID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Detalle4ID.Value.ToString()))
                {
                    query += "and Detalle4ID = @Detalle4ID ";
                    mySqlCommand.Parameters.Add("@Detalle4ID", SqlDbType.Int);
                    mySqlCommand.Parameters["@Detalle4ID"].Value = filter.Detalle4ID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Detalle5ID.Value.ToString()))
                {
                    query += "and Detalle5ID = @Detalle5ID ";
                    mySqlCommand.Parameters.Add("@Detalle5ID", SqlDbType.Int);
                    mySqlCommand.Parameters["@Detalle5ID"].Value = filter.Detalle5ID.Value.Value;
                    filterInd = true;
                }
                mySqlCommand.CommandText = query;

                if (!filterInd)
                    return result;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_prDetalleDocu ctrl = new DTO_prDetalleDocu(dr);
                    result.Add(ctrl);
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

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="document">Documento a filtrar</param>
        /// <param name="numeroDoc">identificador del documento a filtrar</param>
        /// <param name="consecutivoDeta">identificador del detalle si se requiere</param>
        /// <returns></returns>
        public List<DTO_prDetalleDocu> DAL_prDetalleDocu_GetByDocument(int document, int numeroDoc, int consecutivoDeta)
        {
            try
            {
                List<DTO_prDetalleDocu> result = new List<DTO_prDetalleDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Common parameters
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);             
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;             
                #endregion

                string query = string.Empty;

                query = "select distinct det.ConsecutivoDetaID, det.NumeroDoc, det.CodigoBSID,det.inReferenciaID,det.Descriptivo,det.ActivoID,det.SerialID,det.LineaPresupuestoID, " +
                               "det.SolicitudDocuID,det.SolicitudDetaID,det.OrdCompraDocuID,det.OrdCompraDetaID,det.ContratoDocuID,det.RecibidoDocuID,det.RecibidoDetaID,det.FacturaDocuID, " +
                               "det.CantidadSol,det.CantidadOC,det.CantidadCont,det.CantidadRec,det.ValorUni,det.ValorTotML,det.ValorTotME,det.IVAUni,det.IvaTotML,det.IvaTotME " +
                        "from prDetalleDocu det with(nolock) " +
                        "inner join glDocumentoControl doc on det.NumeroDoc = doc.NumeroDoc "; 

                //Consulta Solicitud que relacionen Orden Compra
                if (document == AppDocuments.Solicitud)
                {
                    mySqlCommand.Parameters.Add("@SolicitudDocuID", SqlDbType.Int);
                    mySqlCommand.Parameters["@SolicitudDocuID"].Value = numeroDoc;
                    query += " where det.SolicitudDocuID = @SolicitudDocuID and det.SolicitudDetaID <> det.ConsecutivoDetaID and det.RecibidoDocuID is null";
                    if (consecutivoDeta != 0)
                    {
                        mySqlCommand.Parameters.Add("@SolicitudDetaID", SqlDbType.Int);
                        mySqlCommand.Parameters["@SolicitudDetaID"].Value = consecutivoDeta;
                        query += " and det.SolicitudDetaID = @SolicitudDetaID";
                    }
                }
                //Consulta Orden de Compra que relacionen Recibidos
                else if (document == AppDocuments.OrdenCompra)
                {
                    mySqlCommand.Parameters.Add("@OrdCompraDocuID", SqlDbType.Int);
                    mySqlCommand.Parameters["@OrdCompraDocuID"].Value = numeroDoc;
                    query += " where det.OrdCompraDocuID = @OrdCompraDocuID  and det.OrdCompraDocuID <> det.ConsecutivoDetaID and det.RecibidoDocuID is not null ";
                    if (consecutivoDeta != 0)
                    {
                        mySqlCommand.Parameters.Add("@OrdCompraDetaID", SqlDbType.Int);
                        mySqlCommand.Parameters["@OrdCompraDetaID"].Value = consecutivoDeta;
                        query += " and det.OrdCompraDetaID = @OrdCompraDetaID";
                    }
                }
                //Consulta Recibidos que relacionen Facturas
                else if (document == AppDocuments.Recibido)
                {
                    mySqlCommand.Parameters.Add("@RecibidoDocuID", SqlDbType.Int);
                    mySqlCommand.Parameters["@RecibidoDocuID"].Value = numeroDoc;
                    query += " where det.RecibidoDocuID = @RecibidoDocuID  and det.RecibidoDocuID is not null and det.FacturaDocuID is not null ";
                    if (consecutivoDeta != 0)
                    {
                        mySqlCommand.Parameters.Add("@RecibidoDetaID", SqlDbType.Int);
                        mySqlCommand.Parameters["@RecibidoDetaID"].Value = consecutivoDeta;
                        query += "and det.RecibidoDetaID = @RecibidoDetaID";
                    }
                }
                mySqlCommand.CommandText = query;

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_prDetalleDocu ctrl = new DTO_prDetalleDocu(dr,true);
                    result.Add(ctrl);
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

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="document">Documento a filtrar</param>
        /// <param name="numeroDoc">identificador del documento a filtrar</param>
        /// <param name="consecutivoDeta">identificador del detalle si se requiere</param>
        /// <returns></returns>
        public List<DTO_prDetalleDocu> DAL_prDetalleDocu_GetSolicitudByProyecto(int documentoIDFilter, int? numeroDoc, string proyectoID)
        {
            try
            {
                List<DTO_prDetalleDocu> result = new List<DTO_prDetalleDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                string query = string.Empty;
                string where = string.Empty;
                #region Common parameters
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters["@DocumentoID"].Value = documentoIDFilter;
                if (!string.IsNullOrEmpty(proyectoID))
                {
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@ProyectoID"].Value = proyectoID;
                    where += " and  carg.ProyectoID = @ProyectoID ";
                }
                if (numeroDoc != null)
                {
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                    where += " and ctrl.NumeroDoc = @NumeroDoc ";
                }
                #endregion
                
                query = "select distinct det.*,carg.ProyectoID,carg.LineaPresupuestoID from prdetalledocu det " +
                        "    left join prSolicitudCargos carg on carg.ConsecutivoDetaID = det.SolicitudDetaID " +
                        "    inner join glDocumentoControl ctrl on ctrl.NumeroDoc = det.NumeroDoc and ctrl.DocumentoID = @DocumentoID " +
                        "Where ctrl.EmpresaID = @EmpresaID " + where;

                mySqlCommand.CommandText = query;

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_prDetalleDocu ctrl = new DTO_prDetalleDocu(dr);
                    result.Add(ctrl);
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

        /// <summary>
        /// Obtiene los items para cerrar de un documento
        /// </summary>
        /// <param name="documentFilter">Documento  a cerrar</param>
        /// <param name="prefijoID">Prefijo del doc</param>
        /// <param name="docNro">nro  del Doc</param>
        /// <param name="proveedorID">Proveedor</param>
        /// <param name="referenciaID">Referencia</param>
        /// <param name="codigoBS">Codigo BS</param>
        /// <returns>Lista de detalle</returns>
        public List<DTO_prDetalleDocu> DAL_prDetalleDocu_GetPendienteForCierre(int documentFilter, string prefijoID, int? docNro, string proveedorID, string referenciaID, string codigoBS)
        {
            try
            {
                List<DTO_prDetalleDocu> result = new List<DTO_prDetalleDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Filtros
                string filter = string.Empty;
                if (!string.IsNullOrEmpty(prefijoID))
                {
                    filter += " and ctrl.PrefijoID = @PrefijoID ";
                    mySqlCommand.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                    mySqlCommand.Parameters["@PrefijoID"].Value = prefijoID;
                }
                if (docNro != null)
                {
                    filter += " and ctrl.DocumentoNro = @DocumentoNro ";
                    mySqlCommand.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocumentoNro"].Value = docNro;
                }
                if (!string.IsNullOrEmpty(proveedorID))
                {
                    filter += " and docu.ProveedorID = @ProveedorID ";
                    mySqlCommand.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                    mySqlCommand.Parameters["@ProveedorID"].Value = proveedorID;
                }
                  
                if (!string.IsNullOrEmpty(referenciaID))
                {
                    filter += " and det.inReferenciaID = @inReferenciaID ";
                    mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                    mySqlCommand.Parameters["@inReferenciaID"].Value = referenciaID;
                }

                if (!string.IsNullOrEmpty(codigoBS))
                {
                    filter += " and det.CodigoBSID = @CodigoBSID ";
                    mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                    mySqlCommand.Parameters["@CodigoBSID"].Value = codigoBS;
                }        
                #endregion

                #region Crea parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@DocumentoID1", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoID2", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoID3", SqlDbType.Int); 
                #endregion

                #region Asigna valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado; 
                #endregion

                if (documentFilter == (byte)AppDocuments.Solicitud)
                {
                    mySqlCommand.Parameters.Add("@DocumentoID4", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocumentoID1"].Value = AppDocuments.Solicitud;
                    mySqlCommand.Parameters["@DocumentoID2"].Value = AppDocuments.OrdenCompra;
                    mySqlCommand.Parameters["@DocumentoID3"].Value = AppDocuments.CierreDetalleSolicitud;
                    mySqlCommand.Parameters["@DocumentoID4"].Value = AppDocuments.TransaccionManual;

                    mySqlCommand.CommandText =
                     " Select ctrl.NumeroDoc,ctrl.PeriodoDoc,ctrl.FechaDoc,ctrl.PrefijoID,ctrl.DocumentoNro, " +
                     " solCargo.ProyectoID,solCargo.CentroCostoID,refer.MarcaInvID,refer.RefProveedor,IsNull(mat.Margen,0) as Margen,IsNull(empaque.Cantidad,1) as CantidadxEmpaque,temp.CantidadSum CantidadPend,det.* " +
                     " from    (  Select det.SolicitudDocuID,det.SolicitudDetaID,SUM(det.CantidadSol) CantidadSum " +
                     "            from prDetalleDocu det with(nolock)  " +
                     "                inner join glDocumentoControl ctrlSol with(nolock) on det.NumeroDoc = ctrlSol.NumeroDoc " +
                     "                inner join glDocumento doc with(nolock) on ctrlSol.DocumentoID = doc.DocumentoID  " +
                     "            where ctrlSol.EmpresaID = @EmpresaID and (doc.ModuloID = 'pr' or doc.ModuloID = 'in')  " +
                     "                and ( (ctrlSol.DocumentoID = @DocumentoID1 and ctrlSol.Estado = @Estado)  or ctrlSol.DocumentoID = @DocumentoID2 or ctrlSol.DocumentoID = @DocumentoID3 or ctrlSol.DocumentoID = @DocumentoID4) " +
                     "            group by det.SolicitudDocuID,det.SolicitudDetaID ) temp " +
                     " inner join glDocumentoControl ctrl with(nolock) on temp.SolicitudDocuID = ctrl.NumeroDoc " +
                     " inner join prDetalleDocu det with(nolock) on temp.SolicitudDetaID = det.ConsecutivoDetaID " +
                     " inner join prSolicitudDocu docu with(nolock) on temp.SolicitudDocuID = docu.NumeroDoc " +
                     " inner join prSolicitudCargos solCargo with(nolock) on det.ConsecutivoDetaID = solCargo.ConsecutivoDetaID " +
                     " left join inReferencia refer with(nolock) on refer.inReferenciaID = IsNull(det.inReferenciaID,det.CodigoBSID) and refer.EmpresaGrupoID = IsNull(det.eg_inReferencia,det.eg_prBienServicio)  " +
                     " left join inEmpaque empaque with(nolock) On empaque.EmpaqueInvID = det.EmpaqueInvID and empaque.EmpresaGrupoID = det.eg_inEmpaque  " +
                     " left join inMaterial mat with(nolock) on mat.MaterialInvID = refer.MaterialInvID and mat.EmpresaGrupoID = refer.eg_inMaterial " +
                     " where temp.CantidadSum!=0 and temp.CantidadSum > 0  " + filter; // and docu.Destino = 0 and (det.OrigenMonetario = 1 or det.OrigenMonetario is null)
                }
                else if (documentFilter == (byte)AppDocuments.OrdenCompra)
                {
                    mySqlCommand.Parameters["@DocumentoID1"].Value = AppDocuments.OrdenCompra;
                    mySqlCommand.Parameters["@DocumentoID2"].Value = AppDocuments.Recibido;
                    mySqlCommand.Parameters["@DocumentoID3"].Value = AppDocuments.CierreDetalleOrdenComp;

                    mySqlCommand.CommandText =
                     " Select ctrl.NumeroDoc,ctrl.PeriodoDoc,ctrl.FechaDoc,ctrl.PrefijoID,ctrl.DocumentoNro,docu.ProveedorID, " +
                     " solCargo.ProyectoID,solCargo.CentroCostoID,refer.MarcaInvID,refer.RefProveedor,IsNull(mat.Margen,0) as Margen,IsNull(empaque.Cantidad,1) as CantidadxEmpaque,temp.CantidadSum CantidadPend,det.* " +
                     " from    (  Select det.OrdCompraDocuID,det.OrdCompraDetaID,SUM(det.CantidadOC) CantidadSum " +
                     "            from prDetalleDocu det with(nolock)  " +
                     "                inner join glDocumentoControl ctrlOC with(nolock) on det.NumeroDoc = ctrlOC.NumeroDoc " +
                     "                inner join glDocumento doc with(nolock) on ctrlOC.DocumentoID = doc.DocumentoID  " +
                     "            where ctrlOC.EmpresaID = @EmpresaID and doc.ModuloID = 'pr'  " +
                     "                and ( (ctrlOC.DocumentoID = @DocumentoID1 and ctrlOC.Estado = @Estado)  or ctrlOC.DocumentoID = @DocumentoID2 or ctrlOC.DocumentoID = @DocumentoID3) " +
                     "            group by det.OrdCompraDocuID,det.OrdCompraDetaID ) temp " +
                     " inner join glDocumentoControl ctrl with(nolock) on temp.OrdCompraDocuID = ctrl.NumeroDoc " +
                     " inner join prDetalleDocu det with(nolock) on temp.OrdCompraDetaID = det.ConsecutivoDetaID " +
                     " inner join prOrdenCompraDocu docu with(nolock) on temp.OrdCompraDocuID = docu.NumeroDoc " +
                     " inner join prSolicitudCargos solCargo with(nolock) on det.SolicitudDetaID = solCargo.ConsecutivoDetaID " +
                     " left join inReferencia refer with(nolock) on refer.inReferenciaID = IsNull(det.inReferenciaID,det.CodigoBSID) and refer.EmpresaGrupoID = IsNull(det.eg_inReferencia,det.eg_prBienServicio)  " +
                     " left join inEmpaque empaque with(nolock) On empaque.EmpaqueInvID = det.EmpaqueInvID and empaque.EmpresaGrupoID = det.eg_inEmpaque  " +
                     " left join inMaterial mat with(nolock) on mat.MaterialInvID = refer.MaterialInvID and mat.EmpresaGrupoID = refer.eg_inMaterial " +
                     " where temp.CantidadSum!=0 and temp.CantidadSum > 0  " + filter; // and docu.Destino = 0 and (det.OrigenMonetario = 1 or det.OrigenMonetario is null)
                }
                else if (documentFilter == (byte)AppDocuments.Recibido)
                {
                    mySqlCommand.Parameters["@DocumentoID1"].Value = AppDocuments.Recibido;
                    mySqlCommand.Parameters["@DocumentoID2"].Value = AppDocuments.CausarFacturas;
                    mySqlCommand.Parameters["@DocumentoID3"].Value = AppDocuments.CierreDetalleRecibidos;

                    mySqlCommand.CommandText =
                     " Select ctrl.NumeroDoc,ctrl.PeriodoDoc,ctrl.FechaDoc,ctrl.PrefijoID,ctrl.DocumentoNro,docu.ProveedorID, " +
                     " solCargo.ProyectoID,solCargo.CentroCostoID,refer.MarcaInvID,refer.RefProveedor,IsNull(mat.Margen,0) as Margen,IsNull(empaque.Cantidad,1) as CantidadxEmpaque,temp.CantidadSum CantidadPend,det.* " +
                     " from    (  Select det.RecibidoDocuID,det.RecibidoDetaID,SUM(det.CantidadRec) CantidadSum " +
                     "            from prDetalleDocu det with(nolock)  " +
                     "                inner join glDocumentoControl ctrlOC with(nolock) on det.NumeroDoc = ctrlOC.NumeroDoc " +
                     "                inner join glDocumento doc with(nolock) on ctrlOC.DocumentoID = doc.DocumentoID  " +
                     "            where ctrlOC.EmpresaID = @EmpresaID and doc.ModuloID = 'pr'  " +
                     "                and ( (ctrlOC.DocumentoID = @DocumentoID1 and ctrlOC.Estado = @Estado)  or ctrlOC.DocumentoID = @DocumentoID2  or ctrlOC.DocumentoID = @DocumentoID3) " +
                     "            group by det.RecibidoDocuID,det.RecibidoDetaID ) temp " +
                     " inner join glDocumentoControl ctrl with(nolock) on temp.RecibidoDocuID = ctrl.NumeroDoc " +
                     " inner join prDetalleDocu det with(nolock) on temp.RecibidoDetaID = det.ConsecutivoDetaID  and FacturaDocuID is null" +
                     " inner join prRecibidoDocu docu with(nolock) on temp.RecibidoDocuID = docu.NumeroDoc " +
                     " inner join prSolicitudCargos solCargo with(nolock) on det.SolicitudDetaID = solCargo.ConsecutivoDetaID " +
                     " left join inReferencia refer with(nolock) on refer.inReferenciaID = IsNull(det.inReferenciaID,det.CodigoBSID) and refer.EmpresaGrupoID = IsNull(det.eg_inReferencia,det.eg_prBienServicio) " +
                     " left join inEmpaque empaque with(nolock) On empaque.EmpaqueInvID = det.EmpaqueInvID and empaque.EmpresaGrupoID = det.eg_inEmpaque  " +
                     " left join inMaterial mat with(nolock) on mat.MaterialInvID = refer.MaterialInvID and mat.EmpresaGrupoID = refer.eg_inMaterial " +
                     " where temp.CantidadSum!=0 and temp.CantidadSum > 0  " + filter; // and docu.Destino = 0 and (det.OrigenMonetario = 1 or det.OrigenMonetario is null)
                }


                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_prDetalleDocu dtoRes = new DTO_prDetalleDocu(dr);
                    if (!string.IsNullOrWhiteSpace(dr["PeriodoDoc"].ToString()))
                        dtoRes.PeriodoDoc.Value = Convert.ToDateTime(dr["PeriodoDoc"]);
                    if (!string.IsNullOrWhiteSpace(dr["FechaDoc"].ToString()))
                        dtoRes.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                    dtoRes.PrefDoc = dr["PrefijoID"].ToString().TrimEnd() + "-" +  dr["DocumentoNro"].ToString();
                    dtoRes.ProyectoID.Value = dr["ProyectoID"].ToString();
                    dtoRes.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                    dtoRes.RefProveedor.Value = dr["RefProveedor"].ToString();
                    dtoRes.MarcaInvID.Value = dr["MarcaInvID"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["CantidadPend"].ToString()))
                        dtoRes.CantidadPend.Value = Convert.ToDecimal(dr["CantidadPend"]); //Cantidad Pendiente   
                    if (documentFilter == (byte)AppDocuments.OrdenCompra || documentFilter == (byte)AppDocuments.Recibido)
                        dtoRes.ProveedorID.Value = dr["ProveedorID"].ToString();
                    dtoRes.CantidadxEmpaque.Value = Convert.ToInt32(dr["CantidadxEmpaque"]);
                    if (dtoRes.CantidadPend.Value > Convert.ToDecimal(dr["Margen"]))
                        result.Add(dtoRes);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prDetalle_GetPendienteByParameter");
                throw exception;
            }
        }
    

        /// <summary>
        /// Elimina registro specificado de la tabla prDetalleDocu
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public void DAL_prDetalleDocu_Delete(int consDetaID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConsecutivoDetaID", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ConsecutivoDetaID"].Value = consDetaID;

                mySqlCommandSel.CommandText = "DELETE FROM prDetalleDocu where EmpresaID = @EmpresaID " +
                " and ConsecutivoDetaID = @ConsecutivoDetaID";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prDetalleDocu_GetByNumeroDoc");
                throw exception;
            }
        }


        #endregion

    }
}
