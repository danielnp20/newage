using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;
using System.Reflection;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class recibidos de bienes y servicios:
    /// Models DTO_prDetalleDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prDetalleDocu
    {
        #region prDetalleDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prDetalleDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ConsecutivoDetaID.Value = Convert.ToInt32(dr["ConsecutivoDetaID"]);
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["inReferenciaID"].ToString()))
                    this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["LineaPresupuestoID"].ToString()))
                    this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString(); 
                if (!string.IsNullOrWhiteSpace(dr["EstadoInv"].ToString()))
                    this.EstadoInv.Value = Convert.ToByte(dr["EstadoInv"]); 
                if (!string.IsNullOrWhiteSpace(dr["Parametro1"].ToString()))
                    this.Parametro1.Value = dr["Parametro1"].ToString(); 
                if (!string.IsNullOrWhiteSpace(dr["Parametro2"].ToString()))
                    this.Parametro2.Value = dr["Parametro2"].ToString(); 
                if (!string.IsNullOrWhiteSpace(dr["ActivoID"].ToString()))
                    this.ActivoID.Value = Convert.ToInt32(dr["ActivoID"]); 
                if (!string.IsNullOrWhiteSpace(dr["SerialID"].ToString()))
                    this.SerialID.Value = dr["SerialID"].ToString(); 
                if (!string.IsNullOrWhiteSpace(dr["OrigenMonetario"].ToString()))
                    this.OrigenMonetario.Value = Convert.ToByte(dr["OrigenMonetario"]); 
                if (!string.IsNullOrWhiteSpace(dr["MonedaID"].ToString()))
                    this.MonedaID.Value = dr["MonedaID"].ToString(); 
                this.UnidadInvID.Value = dr["UnidadInvID"].ToString(); 
                if (!string.IsNullOrWhiteSpace(dr["EmpaqueInvID"].ToString()))
                    this.EmpaqueInvID.Value = dr["EmpaqueInvID"].ToString(); 
                if (!string.IsNullOrWhiteSpace(dr["CantidadSol"].ToString()))
                    this.CantidadSol.Value = Convert.ToDecimal(dr["CantidadSol"]); 
                if (!string.IsNullOrWhiteSpace(dr["CantidadEMP"].ToString()))
                    this.CantidadEMP.Value = Convert.ToDecimal(dr["CantidadEMP"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorUni"].ToString()))
                    this.ValorUni.Value = Convert.ToDecimal(dr["ValorUni"]); 
                if (!string.IsNullOrWhiteSpace(dr["IVAUni"].ToString()))
                    this.IVAUni.Value = Convert.ToDecimal(dr["IVAUni"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorTotML"].ToString()))
                    this.ValorTotML.Value = Convert.ToDecimal(dr["ValorTotML"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorTotME"].ToString()))
                    this.ValorTotME.Value = Convert.ToDecimal(dr["ValorTotME"]); 
                if (!string.IsNullOrWhiteSpace(dr["IvaTotML"].ToString()))
                    this.IvaTotML.Value = Convert.ToDecimal(dr["IvaTotML"]); 
                if (!string.IsNullOrWhiteSpace(dr["IvaTotME"].ToString()))
                    this.IvaTotME.Value = Convert.ToDecimal(dr["IvaTotME"]); 
                if (!string.IsNullOrWhiteSpace(dr["SolicitudDocuID"].ToString()))
                    this.SolicitudDocuID.Value = Convert.ToInt32(dr["SolicitudDocuID"]); 
                if (!string.IsNullOrWhiteSpace(dr["LegalizaDocuID"].ToString()))
                    this.LegalizaDocuID.Value = Convert.ToInt32(dr["LegalizaDocuID"]); 
                if (!string.IsNullOrWhiteSpace(dr["OrdCompraDocuID"].ToString()))
                    this.OrdCompraDocuID.Value = Convert.ToInt32(dr["OrdCompraDocuID"]); 
                if (!string.IsNullOrWhiteSpace(dr["ContratoDocuID"].ToString()))
                    this.ContratoDocuID.Value = Convert.ToInt32(dr["ContratoDocuID"]); 
                if (!string.IsNullOrWhiteSpace(dr["InventarioDocuID"].ToString()))
                    this.InventarioDocuID.Value = Convert.ToInt32(dr["InventarioDocuID"]); 
                if (!string.IsNullOrWhiteSpace(dr["RecibidoDocuID"].ToString()))
                    this.RecibidoDocuID.Value = Convert.ToInt32(dr["RecibidoDocuID"]); 
                if (!string.IsNullOrWhiteSpace(dr["ActivoDocuID"].ToString()))
                    this.ActivoDocuID.Value = Convert.ToInt32(dr["ActivoDocuID"]); 
                if (!string.IsNullOrWhiteSpace(dr["FacturaDocuID"].ToString()))
                    this.FacturaDocuID.Value = Convert.ToInt32(dr["FacturaDocuID"]); 
                if (!string.IsNullOrWhiteSpace(dr["Documento1ID"].ToString()))
                    this.Documento1ID.Value = Convert.ToInt32(dr["Documento1ID"]); 
                if (!string.IsNullOrWhiteSpace(dr["Documento2ID"].ToString()))
                    this.Documento2ID.Value = Convert.ToInt32(dr["Documento2ID"]); 
                if (!string.IsNullOrWhiteSpace(dr["Documento3ID"].ToString()))
                    this.Documento3ID.Value = Convert.ToInt32(dr["Documento3ID"]); 
                if (!string.IsNullOrWhiteSpace(dr["Documento4ID"].ToString()))
                    this.Documento4ID.Value = Convert.ToInt32(dr["Documento4ID"]); 
                if (!string.IsNullOrWhiteSpace(dr["Documento5ID"].ToString()))
                    this.Documento5ID.Value = Convert.ToInt32(dr["Documento5ID"]); 
                if (!string.IsNullOrWhiteSpace(dr["SolicitudDetaID"].ToString()))
                    this.SolicitudDetaID.Value = Convert.ToInt32(dr["SolicitudDetaID"]); 
                if (!string.IsNullOrWhiteSpace(dr["LegalizaDetaID"].ToString()))
                    this.LegalizaDetaID.Value = Convert.ToInt32(dr["LegalizaDetaID"]); 
                if (!string.IsNullOrWhiteSpace(dr["OrdCompraDetaID"].ToString()))
                    this.OrdCompraDetaID.Value = Convert.ToInt32(dr["OrdCompraDetaID"]); 
                if (!string.IsNullOrWhiteSpace(dr["ContratoDetaID"].ToString()))
                    this.ContratoDetaID.Value = Convert.ToInt32(dr["ContratoDetaID"]); 
                if (!string.IsNullOrWhiteSpace(dr["InventarioDetaID"].ToString()))
                    this.InventarioDetaID.Value = Convert.ToInt32(dr["InventarioDetaID"]); 
                if (!string.IsNullOrWhiteSpace(dr["RecibidoDetaID"].ToString()))
                    this.RecibidoDetaID.Value = Convert.ToInt32(dr["RecibidoDetaID"]); 
                if (!string.IsNullOrWhiteSpace(dr["Detalle1ID"].ToString()))
                    this.Detalle1ID.Value = Convert.ToInt32(dr["Detalle1ID"]); 
                if (!string.IsNullOrWhiteSpace(dr["Detalle2ID"].ToString()))
                    this.Detalle2ID.Value = Convert.ToInt32(dr["Detalle2ID"]); 
                if (!string.IsNullOrWhiteSpace(dr["Detalle3ID"].ToString()))
                    this.Detalle3ID.Value = Convert.ToInt32(dr["Detalle3ID"]); 
                if (!string.IsNullOrWhiteSpace(dr["Detalle4ID"].ToString()))
                    this.Detalle4ID.Value = Convert.ToInt32(dr["Detalle4ID"]); 
                if (!string.IsNullOrWhiteSpace(dr["Detalle5ID"].ToString()))
                    this.Detalle5ID.Value = Convert.ToInt32(dr["Detalle5ID"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadDoc1"].ToString()))
                    this.CantidadDoc1.Value = Convert.ToDecimal(dr["CantidadDoc1"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadDoc2"].ToString()))
                    this.CantidadDoc2.Value = Convert.ToDecimal(dr["CantidadDoc2"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadDoc3"].ToString()))
                    this.CantidadDoc3.Value = Convert.ToDecimal(dr["CantidadDoc3"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadDoc4"].ToString()))
                    this.CantidadDoc4.Value = Convert.ToDecimal(dr["CantidadDoc4"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadDoc5"].ToString()))
                    this.CantidadDoc5.Value = Convert.ToDecimal(dr["CantidadDoc5"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrLocal01"].ToString()))
                //this.ProvisionInd.Value = Convert.ToBoolean(dr["ProvisionInd"]);
                    this.VlrLocal01.Value = Convert.ToDecimal(dr["VlrLocal01"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrExtra01"].ToString()))
                    this.VlrExtra01.Value = Convert.ToDecimal(dr["VlrExtra01"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrLocal02"].ToString()))
                    this.VlrLocal02.Value = Convert.ToDecimal(dr["VlrLocal02"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrExtra02"].ToString()))
                    this.VlrExtra02.Value = Convert.ToDecimal(dr["VlrExtra02"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrLocal03"].ToString()))
                    this.VlrLocal03.Value = Convert.ToDecimal(dr["VlrLocal03"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrExtra03"].ToString()))
                    this.VlrExtra03.Value = Convert.ToDecimal(dr["VlrExtra03"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrLocal04"].ToString()))
                    this.VlrLocal04.Value = Convert.ToDecimal(dr["VlrLocal04"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrExtra04"].ToString()))
                    this.VlrExtra04.Value = Convert.ToDecimal(dr["VlrExtra04"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrLocal05"].ToString()))
                    this.VlrLocal05.Value = Convert.ToDecimal(dr["VlrLocal05"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrExtra05"].ToString()))
                    this.VlrExtra05.Value = Convert.ToDecimal(dr["VlrExtra05"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrLocal06"].ToString()))
                    this.VlrLocal06.Value = Convert.ToDecimal(dr["VlrLocal06"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrExtra06"].ToString()))
                    this.VlrExtra06.Value = Convert.ToDecimal(dr["VlrExtra06"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrLocal07"].ToString()))
                    this.VlrLocal07.Value = Convert.ToDecimal(dr["VlrLocal07"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrExtra07"].ToString()))
                    this.VlrExtra07.Value = Convert.ToDecimal(dr["VlrExtra07"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrLocal08"].ToString()))
                    this.VlrLocal08.Value = Convert.ToDecimal(dr["VlrLocal08"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrExtra08"].ToString()))
                    this.VlrExtra08.Value = Convert.ToDecimal(dr["VlrExtra08"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrLocal09"].ToString()))
                    this.VlrLocal09.Value = Convert.ToDecimal(dr["VlrLocal09"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrExtra09"].ToString()))
                    this.VlrExtra09.Value = Convert.ToDecimal(dr["VlrExtra09"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrLocal10"].ToString()))
                    this.VlrLocal10.Value = Convert.ToDecimal(dr["VlrLocal10"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrExtra10"].ToString()))
                    this.VlrExtra10.Value = Convert.ToDecimal(dr["VlrExtra10"]); 
                this.DatoAdd1.Value = dr["DatoAdd1"].ToString(); 
                this.DatoAdd2.Value = dr["DatoAdd2"].ToString(); 
                this.DatoAdd3.Value = dr["DatoAdd3"].ToString(); 
                this.DatoAdd4.Value = dr["DatoAdd4"].ToString(); 
                this.DatoAdd5.Value = dr["DatoAdd5"].ToString(); 
                if (!string.IsNullOrWhiteSpace(dr["CantidadCont"].ToString()))
                    this.CantidadCont.Value = Convert.ToDecimal(dr["CantidadCont"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadRec"].ToString()))
                    this.CantidadRec.Value = Convert.ToDecimal(dr["CantidadRec"]); 
                if (!string.IsNullOrWhiteSpace(dr["CantidadOC"].ToString()))
                    this.CantidadOC.Value = Convert.ToDecimal(dr["CantidadOC"]); 
                if (!string.IsNullOrWhiteSpace(dr["PorcentajeIVA"].ToString()))
                    this.PorcentajeIVA.Value = Convert.ToDecimal(dr["PorcentajeIVA"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorBaseAIU"].ToString()))
                    this.ValorBaseAIU.Value = Convert.ToDecimal(dr["ValorBaseAIU"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorAIU"].ToString()))
                    this.ValorAIU.Value = Convert.ToDecimal(dr["ValorAIU"]); 
                if (!string.IsNullOrWhiteSpace(dr["VlrIVAAIU"].ToString()))
                    this.VlrIVAAIU.Value = Convert.ToDecimal(dr["VlrIVAAIU"]); 
                if (!string.IsNullOrWhiteSpace(dr["CodigoAdminAIU"].ToString()))
                    this.CodigoAdminAIU.Value = dr["CodigoAdminAIU"].ToString(); 
                if (!string.IsNullOrWhiteSpace(dr["ValorAdminAIU"].ToString()))
                    this.ValorAdminAIU.Value = Convert.ToDecimal(dr["ValorAdminAIU"]); 
                if (!string.IsNullOrWhiteSpace(dr["IVAAdminAIU"].ToString()))
                    this.IVAAdminAIU.Value = Convert.ToDecimal(dr["IVAAdminAIU"]); 
                if (!string.IsNullOrWhiteSpace(dr["PorIVAAdminAIU"].ToString()))
                    this.PorIVAAdminAIU.Value = Convert.ToDecimal(dr["PorIVAAdminAIU"]); 
                if (!string.IsNullOrWhiteSpace(dr["CodigoImprevAIU"].ToString()))
                    this.CodigoImprevAIU.Value = dr["CodigoImprevAIU"].ToString(); 
                if (!string.IsNullOrWhiteSpace(dr["ValorImprevAIU"].ToString()))
                    this.ValorImprevAIU.Value = Convert.ToDecimal(dr["ValorImprevAIU"]); 
                if (!string.IsNullOrWhiteSpace(dr["IVAImprevAIU"].ToString()))
                    this.IVAImprevAIU.Value = Convert.ToDecimal(dr["IVAImprevAIU"]); 
                if (!string.IsNullOrWhiteSpace(dr["PorIVAImprevAIU"].ToString()))
                    this.PorIVAImprevAIU.Value = Convert.ToDecimal(dr["PorIVAImprevAIU"]); 
                if (!string.IsNullOrWhiteSpace(dr["CodigoUtilidadAIU"].ToString()))
                    this.CodigoUtilidadAIU.Value = dr["CodigoUtilidadAIU"].ToString(); 
                if (!string.IsNullOrWhiteSpace(dr["ValorUtilidadAIU"].ToString()))
                    this.ValorUtilidadAIU.Value = Convert.ToDecimal(dr["ValorUtilidadAIU"]); 
                if (!string.IsNullOrWhiteSpace(dr["IVAUtilidadAIU"].ToString()))
                    this.IVAUtilidadAIU.Value = Convert.ToDecimal(dr["IVAUtilidadAIU"]); 
                if (!string.IsNullOrWhiteSpace(dr["PorIVAUtilidadAIU"].ToString())) 
                    this.PorIVAUtilidadAIU.Value = Convert.ToDecimal(dr["PorIVAUtilidadAIU"]); 
                if (!string.IsNullOrWhiteSpace(dr["DiasEntrega"].ToString()))
                    this.DiasEntrega.Value = Convert.ToByte(dr["DiasEntrega"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoAprobSobreejecucion"].ToString()))
                    this.TipoAprobSobreejecucion.Value = Convert.ToByte(dr["TipoAprobSobreejecucion"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioRevSobreejec"].ToString()))
                    this.UsuarioRevSobreejec.Value = Convert.ToInt32(dr["UsuarioRevSobreejec"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaRevSobreejec"].ToString()))
                    this.FechaRevSobreejec.Value = Convert.ToDateTime(dr["FechaRevSobreejec"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAprSobreejec"].ToString()))
                    this.UsuarioAprSobreejec.Value = Convert.ToInt32(dr["UsuarioAprSobreejec"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAprSobreejec"].ToString()))
                    this.FechaAprSobreejec.Value = Convert.ToDateTime(dr["FechaAprSobreejec"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadINV"].ToString()))
                    this.CantidadINV.Value = Convert.ToDecimal(dr["CantidadINV"]); 
                this.CantidadCierre.Value = 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prDetalleDocu(IDataReader dr, bool consultaDoc)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ConsecutivoDetaID.Value = Convert.ToInt32(dr["ConsecutivoDetaID"]);
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["inReferenciaID"].ToString()))
                    this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["LineaPresupuestoID"].ToString()))
                    this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ActivoID"].ToString()))
                    this.ActivoID.Value = Convert.ToInt32(dr["ActivoID"]);
                if (!string.IsNullOrWhiteSpace(dr["SerialID"].ToString()))
                    this.SerialID.Value = dr["SerialID"].ToString();    
                if (!string.IsNullOrWhiteSpace(dr["CantidadSol"].ToString()))
                    this.CantidadSol.Value = Convert.ToDecimal(dr["CantidadSol"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorUni"].ToString()))
                    this.ValorUni.Value = Convert.ToDecimal(dr["ValorUni"]);
                if (!string.IsNullOrWhiteSpace(dr["IVAUni"].ToString()))
                    this.IVAUni.Value = Convert.ToDecimal(dr["IVAUni"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorTotML"].ToString()))
                    this.ValorTotML.Value = Convert.ToDecimal(dr["ValorTotML"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorTotME"].ToString()))
                    this.ValorTotME.Value = Convert.ToDecimal(dr["ValorTotME"]);
                if (!string.IsNullOrWhiteSpace(dr["IvaTotML"].ToString()))
                    this.IvaTotML.Value = Convert.ToDecimal(dr["IvaTotML"]);
                if (!string.IsNullOrWhiteSpace(dr["IvaTotME"].ToString()))
                    this.IvaTotME.Value = Convert.ToDecimal(dr["IvaTotME"]);
                if (!string.IsNullOrWhiteSpace(dr["SolicitudDocuID"].ToString()))
                    this.SolicitudDocuID.Value = Convert.ToInt32(dr["SolicitudDocuID"]);   
                if (!string.IsNullOrWhiteSpace(dr["OrdCompraDocuID"].ToString()))
                    this.OrdCompraDocuID.Value = Convert.ToInt32(dr["OrdCompraDocuID"]);
                if (!string.IsNullOrWhiteSpace(dr["ContratoDocuID"].ToString()))
                    this.ContratoDocuID.Value = Convert.ToInt32(dr["ContratoDocuID"]);   
                if (!string.IsNullOrWhiteSpace(dr["RecibidoDocuID"].ToString()))
                    this.RecibidoDocuID.Value = Convert.ToInt32(dr["RecibidoDocuID"]);
                if (!string.IsNullOrWhiteSpace(dr["FacturaDocuID"].ToString()))
                    this.FacturaDocuID.Value = Convert.ToInt32(dr["FacturaDocuID"]);
                if (!string.IsNullOrWhiteSpace(dr["SolicitudDetaID"].ToString()))
                    this.SolicitudDetaID.Value = Convert.ToInt32(dr["SolicitudDetaID"]); 
                if (!string.IsNullOrWhiteSpace(dr["OrdCompraDetaID"].ToString()))
                    this.OrdCompraDetaID.Value = Convert.ToInt32(dr["OrdCompraDetaID"]);    
                if (!string.IsNullOrWhiteSpace(dr["RecibidoDetaID"].ToString()))
                    this.RecibidoDetaID.Value = Convert.ToInt32(dr["RecibidoDetaID"]);   
                if (!string.IsNullOrWhiteSpace(dr["CantidadCont"].ToString()))
                    this.CantidadCont.Value = Convert.ToDecimal(dr["CantidadCont"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadRec"].ToString()))
                    this.CantidadRec.Value = Convert.ToDecimal(dr["CantidadRec"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadOC"].ToString()))
                    this.CantidadOC.Value = Convert.ToDecimal(dr["CantidadOC"]);     
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prDetalleDocu()
        {
            this.InitCols();
            this.CantidadCierre.Value = 0;
            this.CantidadPend.Value = 0;
            this.CantidadxEmpaque.Value = 0;
            this.CantEmpaque.Value = 0;
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.ConsecutivoDetaID = new UDT_Consecutivo();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.Descriptivo = new UDT_DescripTExt();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.EstadoInv = new UDTSQL_tinyint();
            this.Parametro1 = new UDT_ParametroInvID();
            this.Parametro2 = new UDT_ParametroInvID();
            this.ActivoID = new UDT_ActivoID();
            this.SerialID = new UDT_SerialID();
            this.OrigenMonetario = new UDTSQL_tinyint();
            this.MonedaID = new UDT_MonedaID();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.EmpaqueInvID = new UDT_EmpaqueInvID();
            this.CantidadSol = new UDT_Cantidad();
            this.CantidadEMP = new UDT_Cantidad();
            this.ValorUni = new UDT_Valor();
            this.IVAUni = new UDT_Valor();
            this.ValorTotML = new UDT_Valor();
            this.ValorTotME = new UDT_Valor();
            this.IvaTotML = new UDT_Valor();
            this.IvaTotME = new UDT_Valor();
            this.SolicitudDocuID = new UDT_Consecutivo();
            this.LegalizaDocuID = new UDT_Consecutivo();
            this.OrdCompraDocuID = new UDT_Consecutivo();
            this.ContratoDocuID = new UDT_Consecutivo();
            this.InventarioDocuID = new UDT_Consecutivo();
            this.RecibidoDocuID = new UDT_Consecutivo();
            this.ActivoDocuID = new UDT_Consecutivo();
            this.FacturaDocuID = new UDT_Consecutivo();
            this.Documento1ID = new UDT_Consecutivo();
            this.Documento2ID = new UDT_Consecutivo();
            this.Documento3ID = new UDT_Consecutivo();
            this.Documento4ID = new UDT_Consecutivo();
            this.Documento5ID = new UDT_Consecutivo();
            this.SolicitudDetaID = new UDT_Consecutivo();
            this.LegalizaDetaID = new UDT_Consecutivo();
            this.OrdCompraDetaID = new UDT_Consecutivo();
            this.ContratoDetaID = new UDT_Consecutivo();
            this.InventarioDetaID = new UDT_Consecutivo();
            this.RecibidoDetaID = new UDT_Consecutivo();
            this.Detalle1ID = new UDT_Consecutivo();
            this.Detalle2ID = new UDT_Consecutivo();
            this.Detalle3ID = new UDT_Consecutivo();
            this.Detalle4ID = new UDT_Consecutivo();
            this.Detalle5ID = new UDT_Consecutivo();
            this.CantidadDoc1 = new UDT_Cantidad();
            this.CantidadDoc2 = new UDT_Cantidad();
            this.CantidadDoc3 = new UDT_Cantidad();
            this.CantidadDoc4 = new UDT_Cantidad();
            this.CantidadDoc5 = new UDT_Cantidad();
            this.VlrLocal01 = new UDT_Valor();
            this.VlrExtra01 = new UDT_Valor();
            this.VlrLocal02 = new UDT_Valor();
            this.VlrExtra02 = new UDT_Valor();
            this.VlrLocal03 = new UDT_Valor();
            this.VlrExtra03 = new UDT_Valor();
            this.VlrLocal04 = new UDT_Valor();
            this.VlrExtra04 = new UDT_Valor();
            this.VlrLocal05 = new UDT_Valor();
            this.VlrExtra05 = new UDT_Valor();
            this.VlrLocal06 = new UDT_Valor();
            this.VlrExtra06 = new UDT_Valor();
            this.VlrLocal07 = new UDT_Valor();
            this.VlrExtra07 = new UDT_Valor();
            this.VlrLocal08 = new UDT_Valor();
            this.VlrExtra08 = new UDT_Valor();
            this.VlrLocal09 = new UDT_Valor();
            this.VlrExtra09 = new UDT_Valor();
            this.VlrLocal10 = new UDT_Valor();
            this.VlrExtra10 = new UDT_Valor();
            this.DatoAdd1 = new UDTSQL_char(20);
            this.DatoAdd2 = new UDTSQL_char(20);
            this.DatoAdd3 = new UDTSQL_char(20);
            this.DatoAdd4 = new UDTSQL_char(20);
            this.DatoAdd5 = new UDTSQL_char(20);
            this.CantidadCont = new UDT_Cantidad();
            this.CantidadRec = new UDT_Cantidad();
            this.CantidadOC = new UDT_Cantidad();
            this.PorcentajeIVA = new UDT_PorcentajeID();
            this.ValorBaseAIU = new UDT_Valor();
            this.ValorAIU = new UDT_Valor();
            this.VlrIVAAIU = new UDT_Valor();
            this.CodigoAdminAIU = new UDT_CodigoBSID();
            this.ValorAdminAIU = new UDT_Valor();
            this.IVAAdminAIU = new UDT_Valor();
            this.PorIVAAdminAIU = new UDT_PorcentajeID();
            this.CodigoImprevAIU = new UDT_CodigoBSID();
            this.ValorImprevAIU = new UDT_Valor();
            this.IVAImprevAIU = new UDT_Valor();
            this.PorIVAImprevAIU = new UDT_PorcentajeID();
            this.CodigoUtilidadAIU = new UDT_CodigoBSID();
            this.ValorUtilidadAIU = new UDT_Valor();
            this.IVAUtilidadAIU = new UDT_Valor();
            this.PorIVAUtilidadAIU = new UDT_PorcentajeID();
            this.DiasEntrega = new UDTSQL_tinyint();
            this.TipoAprobSobreejecucion = new UDTSQL_tinyint();
            this.UsuarioRevSobreejec = new UDT_seUsuarioID();
            this.FechaRevSobreejec = new UDTSQL_smalldatetime();
            this.UsuarioAprSobreejec = new UDT_seUsuarioID();
            this.FechaAprSobreejec = new UDTSQL_smalldatetime();
            this.CantidadINV = new UDT_Cantidad();
            //Adicionales
            this.PeriodoDoc = new UDTSQL_smalldatetime();
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.ProveedorID = new UDT_ProveedorID();
            this.MarcaInvID = new UDT_CodigoGrl();
            this.RefProveedor = new UDT_CodigoGrl20();
            this.CantidadPend = new UDT_Cantidad();
            this.CantidadCierre = new UDT_Cantidad();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.ProveedorDesc = new UDT_Descriptivo();
            this.TareaID = new UDT_CodigoGrl();
            this.CantidadTarea = new UDT_Cantidad();
            this.UnidadEmpaque = new UDT_UnidadInvID();
            this.CantidadxEmpaque = new UDT_Cantidad();
            this.CantEmpaque = new UDT_Cantidad();           
        }

        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public int Index { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsecutivoDetaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }
        
        [DataMember]
        [Filtrable]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        public UDT_DescripTExt Descriptivo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint EstadoInv { get; set; }

        [DataMember]
        public UDT_ParametroInvID Parametro1 { get; set; }

        [DataMember]
        public UDT_ParametroInvID Parametro2 { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_ActivoID ActivoID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SerialID SerialID { get; set; }

        [DataMember]
        public UDTSQL_tinyint OrigenMonetario { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        public UDT_EmpaqueInvID EmpaqueInvID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadSol { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadEMP { get; set; }

        [DataMember]
        public UDT_Valor ValorUni { get; set; }

        [DataMember]
        public UDT_Valor IVAUni { get; set; }

        [DataMember]
        public UDT_Valor ValorTotML { get; set; }

        [DataMember]
        public UDT_Valor IvaTotML { get; set; }

        [DataMember]
        public UDT_Valor ValorTotME { get; set; }

        [DataMember]
        public UDT_Valor IvaTotME { get; set; }
        
        [DataMember]
        public UDT_Consecutivo SolicitudDocuID { get; set; }

        [DataMember]
        public UDT_Consecutivo LegalizaDocuID { get; set; }

        [DataMember]
        public UDT_Consecutivo OrdCompraDocuID { get; set; }

        [DataMember]
        public UDT_Consecutivo ContratoDocuID { get; set; }
        
        [DataMember]
        public UDT_Consecutivo InventarioDocuID { get; set; }

        [DataMember]
        public UDT_Consecutivo RecibidoDocuID { get; set; }

        [DataMember]
        public UDT_Consecutivo ActivoDocuID { get; set; }

        [DataMember]
        public UDT_Consecutivo FacturaDocuID { get; set; }

        [DataMember]
        public UDT_Consecutivo Documento1ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Documento2ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Documento3ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Documento4ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Documento5ID { get; set; }

        [DataMember]
        public UDT_Consecutivo SolicitudDetaID { get; set; }

        [DataMember]
        public UDT_Consecutivo LegalizaDetaID { get; set; }

        [DataMember]
        public UDT_Consecutivo OrdCompraDetaID { get; set; }

        [DataMember]
        public UDT_Consecutivo ContratoDetaID { get; set; }

        [DataMember]
        public UDT_Consecutivo InventarioDetaID { get; set; }

        [DataMember]
        public UDT_Consecutivo RecibidoDetaID { get; set; }

        [DataMember]
        public UDT_Consecutivo Detalle1ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Detalle2ID { get; set; }
        
        [DataMember]
        public UDT_Consecutivo Detalle3ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Detalle4ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Detalle5ID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDoc1 { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDoc2 { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDoc3 { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDoc4 { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDoc5 { get; set; }

        [DataMember]
        public UDT_Valor VlrLocal01 { get; set; }

        [DataMember]
        public UDT_Valor VlrExtra01 { get; set; }

        [DataMember]
        public UDT_Valor VlrLocal02 { get; set; }

        [DataMember]
        public UDT_Valor VlrExtra02 { get; set; }

        [DataMember]
        public UDT_Valor VlrLocal03 { get; set; }

        [DataMember]
        public UDT_Valor VlrExtra03 { get; set; }

        [DataMember]
        public UDT_Valor VlrLocal04 { get; set; }

        [DataMember]
        public UDT_Valor VlrExtra04 { get; set; }

        [DataMember]
        public UDT_Valor VlrLocal05 { get; set; }

        [DataMember]
        public UDT_Valor VlrExtra05 { get; set; }

        [DataMember]
        public UDT_Valor VlrLocal06 { get; set; }

        [DataMember]
        public UDT_Valor VlrExtra06 { get; set; }

        [DataMember]
        public UDT_Valor VlrLocal07 { get; set; }

        [DataMember]
        public UDT_Valor VlrExtra07 { get; set; }

        [DataMember]
        public UDT_Valor VlrLocal08 { get; set; }

        [DataMember]
        public UDT_Valor VlrExtra08 { get; set; }

        [DataMember]
        public UDT_Valor VlrLocal09 { get; set; }

        [DataMember]
        public UDT_Valor VlrExtra09 { get; set; }

        [DataMember]
        public UDT_Valor VlrLocal10 { get; set; }

        [DataMember]
        public UDT_Valor VlrExtra10 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd1 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd2 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd3 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd4 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd5 { get; set; }
        
        [DataMember]
        public UDT_Cantidad CantidadOC { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadRec { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadCont { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorcentajeIVA { get; set; }

        [DataMember]
        public UDT_Valor ValorBaseAIU { get; set; }

        [DataMember]
        public UDT_Valor ValorAIU { get; set; }

        [DataMember]
        public UDT_Valor VlrIVAAIU { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoAdminAIU { get; set; }

        [DataMember]
        public UDT_Valor ValorAdminAIU { get; set; }

        [DataMember]
        public UDT_Valor IVAAdminAIU { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorIVAAdminAIU { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoImprevAIU { get; set; }

        [DataMember]
        public UDT_Valor ValorImprevAIU { get; set; }

        [DataMember]
        public UDT_Valor IVAImprevAIU { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorIVAImprevAIU { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoUtilidadAIU { get; set; }

        [DataMember]
        public UDT_Valor ValorUtilidadAIU { get; set; }

        [DataMember]
        public UDT_Valor IVAUtilidadAIU { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorIVAUtilidadAIU { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasEntrega { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoAprobSobreejecucion{ get; set; }

        [DataMember]
        public UDT_seUsuarioID UsuarioRevSobreejec { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaRevSobreejec { get; set; }

        [DataMember]
        public UDT_seUsuarioID UsuarioAprSobreejec { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaAprSobreejec { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadINV { get; set; }

        //Adicionales

        [DataMember]
        public UDTSQL_smalldatetime PeriodoDoc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public string PrefDoc { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadPend { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadCierre { get; set; }

        [DataMember]
        public UDT_CodigoGrl MarcaInvID { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 RefProveedor { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo ProveedorDesc { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadxEmpaque { get; set; }

        [DataMember]
        public UDT_Cantidad CantEmpaque { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadEmpaque { get; set; }

        //Campos Proyectos
        [DataMember]
        public UDT_CodigoGrl TareaID { get; set; }

        [DataMember]
        public int? ConsecTarea { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadTarea { get; set; }

        #endregion
    }
}
