using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// DTO Tabla DTO_glMovimientoDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glMovimientoDeta
    {
        #region Constructor

        public DTO_glMovimientoDeta(IDataReader dr, bool activo)
        {
            InitCols();
            try
            {
                this.MvtoTipoActID.Value = dr["MvtoTipoActID"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.DescripTExt.Value = dr["DescripTExt"].ToString();
                this.SerialID.Value = dr["SerialID"].ToString();
                this.Prefijo_Documento.Value = dr["Prefijo_Documento"].ToString();
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.PlaquetaID.Value = dr["PlaquetaID"].ToString();
                this.Valor1LOC.Value = Convert.ToDecimal(dr["Valor1LOC"]);
                this.Valor2LOC.Value = Convert.ToDecimal(dr["Valor2LOC"]);
                this.Valor1EXT.Value = Convert.ToDecimal(dr["Valor1EXT"]);
                this.Valor2EXT.Value = Convert.ToDecimal(dr["Valor2EXT"]);
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.DatoAdd2.Value = dr["DatoAdd2"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["IdentificadorTr"].ToString()))
                    this.IdentificadorTr.Value = Convert.ToInt32(dr["IdentificadorTr"]);
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
        public DTO_glMovimientoDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.BodegaID.Value = dr["BodegaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["EntradaSalida"].ToString()))
                    this.EntradaSalida.Value = Convert.ToByte(dr["EntradaSalida"]);
                this.Kit.Value = dr["inReferenciaID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["EstadoInv"].ToString()))
                    this.EstadoInv.Value = Convert.ToByte(dr["EstadoInv"]);
                this.Parametro1.Value = dr["Parametro1"].ToString();
                this.Parametro2.Value = dr["Parametro2"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["IdentificadorTr"].ToString()))
                    this.IdentificadorTr.Value =  Convert.ToInt32(dr["IdentificadorTr"]);
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.ServicioID.Value = dr["ServicioID"].ToString();
                this.SerialID.Value = dr["SerialID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ActivoID"].ToString()))
                    this.ActivoID.Value = Convert.ToInt32(dr["ActivoID"]);
                this.MvtoTipoInvID.Value = dr["MvtoTipoInvID"].ToString();
                this.MvtoTipoActID.Value = dr["MvtoTipoActID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DocSoporte"].ToString()))
                    this.DocSoporte.Value = Convert.ToInt32(dr["DocSoporte"]);
                this.DocSoporteTER.Value = dr["DocSoporteTER"].ToString();
                this.AsesorID.Value = dr["AsesorID"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.DescripTExt.Value = dr["DescripTExt"].ToString();
                this.DatoAdd1.Value = dr["DatoAdd1"].ToString();
                this.DatoAdd2.Value = dr["DatoAdd2"].ToString();
                this.DatoAdd3.Value = dr["DatoAdd3"].ToString();
                this.DatoAdd4.Value = dr["DatoAdd4"].ToString();
                this.DatoAdd5.Value = dr["DatoAdd5"].ToString();
                this.EmpaqueInvID.Value = dr["EmpaqueInvID"].ToString();
                if(!string.IsNullOrWhiteSpace(dr["CantidadEMP"].ToString()))
                    this.CantidadEMP.Value = Convert.ToDecimal(dr["CantidadEMP"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadDoc"].ToString()))
                    this.CantidadDoc.Value = Convert.ToDecimal(dr["CantidadDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadUNI"].ToString()))
                    this.CantidadUNI.Value = Convert.ToDecimal(dr["CantidadUNI"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorUNI"].ToString()))
                    this.ValorUNI.Value = Convert.ToDecimal(dr["ValorUNI"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor1LOC"].ToString()))
                    this.Valor1LOC.Value = Convert.ToDecimal(dr["Valor1LOC"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor2LOC"].ToString()))
                    this.Valor2LOC.Value = Convert.ToDecimal(dr["Valor2LOC"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor3LOC"].ToString()))
                    this.Valor3LOC.Value = Convert.ToDecimal(dr["Valor3LOC"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor4LOC"].ToString()))
                    this.Valor4LOC.Value = Convert.ToDecimal(dr["Valor4LOC"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor5LOC"].ToString()))
                    this.Valor5LOC.Value = Convert.ToDecimal(dr["Valor5LOC"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor6LOC"].ToString()))
                    this.Valor6LOC.Value = Convert.ToDecimal(dr["Valor6LOC"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor7LOC"].ToString()))
                    this.Valor7LOC.Value = Convert.ToDecimal(dr["Valor7LOC"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor8LOC"].ToString()))
                    this.Valor8LOC.Value = Convert.ToDecimal(dr["Valor8LOC"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor9LOC"].ToString()))
                    this.Valor9LOC.Value = Convert.ToDecimal(dr["Valor9LOC"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor10LOC"].ToString()))
                    this.Valor10LOC.Value = Convert.ToDecimal(dr["Valor10LOC"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor1EXT"].ToString()))
                    this.Valor1EXT.Value = Convert.ToDecimal(dr["Valor1EXT"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor2EXT"].ToString()))
                    this.Valor2EXT.Value = Convert.ToDecimal(dr["Valor2EXT"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor3EXT"].ToString()))
                    this.Valor3EXT.Value = Convert.ToDecimal(dr["Valor3EXT"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor4EXT"].ToString()))
                    this.Valor4EXT.Value = Convert.ToDecimal(dr["Valor4EXT"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor5EXT"].ToString()))
                    this.Valor5EXT.Value = Convert.ToDecimal(dr["Valor5EXT"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor6EXT"].ToString()))
                    this.Valor6EXT.Value = Convert.ToDecimal(dr["Valor6EXT"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor7EXT"].ToString()))
                    this.Valor7EXT.Value = Convert.ToDecimal(dr["Valor7EXT"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor8EXT"].ToString()))
                    this.Valor8EXT.Value = Convert.ToDecimal(dr["Valor8EXT"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor9EXT"].ToString()))
                    this.Valor9EXT.Value = Convert.ToDecimal(dr["Valor9EXT"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor10EXT"].ToString()))
                    this.Valor10EXT.Value = Convert.ToDecimal(dr["Valor10EXT"]);
                if (!string.IsNullOrWhiteSpace(dr["Consecutivo"].ToString()))
                    this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                if (!string.IsNullOrWhiteSpace(dr["LineaPresupuestoID"].ToString()))
                    this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CantidadDEV"].ToString()))
                    this.CantidadDEV.Value = Convert.ToInt32(dr["CantidadDEV"]);
                if (!string.IsNullOrWhiteSpace(dr["ConsecutivoPrestamo"].ToString()))
                    this.ConsecutivoPrestamo.Value = Convert.ToInt32(dr["ConsecutivoPrestamo"]);
                if (!string.IsNullOrWhiteSpace(dr["ConsecutivoOrdCompra"].ToString()))
                    this.ConsecutivoOrdCompra.Value = Convert.ToInt32(dr["ConsecutivoOrdCompra"]);
                if (!string.IsNullOrWhiteSpace(dr["NroItem"].ToString()))
                    this.NroItem.Value = Convert.ToInt32(dr["NroItem"]);
                if (!string.IsNullOrWhiteSpace(dr["ImprimeInd"].ToString()))
                    this.ImprimeInd.Value = Convert.ToBoolean(dr["ImprimeInd"]);

                

            }
            catch (Exception e)
            { 
                throw; 
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glMovimientoDeta()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        protected virtual void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.BodegaID = new UDT_BodegaID();
            this.EntradaSalida = new UDTSQL_tinyint();
            this.Kit = new UDT_inReferenciaID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.EstadoInv = new UDTSQL_tinyint();
            this.Parametro1 = new UDT_ParametroInvID();
            this.Parametro2 = new UDT_ParametroInvID();
            this.IdentificadorTr = new UDT_Consecutivo();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.ServicioID = new UDT_ServicioID();
            this.SerialID = new UDT_SerialID();
            this.ActivoID = new UDT_ActivoID();
            this.MvtoTipoInvID = new UDT_MvtoTipoID();
            this.MvtoTipoActID = new UDT_MvtoTipoID();
            this.DocSoporte = new UDT_Consecutivo();
            this.DocSoporteTER = new UDTSQL_varchar(20);
            this.AsesorID = new UDT_AsesorID();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.TerceroID = new UDT_TerceroID();
            this.DescripTExt = new UDT_DescripTExt();
            this.DatoAdd1 = new UDTSQL_char(20);
            this.DatoAdd2 = new UDTSQL_char(20);
            this.DatoAdd3 = new UDTSQL_char(20);
            this.DatoAdd4 = new UDTSQL_char(20);
            this.DatoAdd5 = new UDTSQL_char(20);
            this.EmpaqueInvID = new UDT_EmpaqueInvID();
            this.CantidadEMP = new UDT_Cantidad();
            this.CantidadDoc = new UDT_Cantidad();
            this.CantidadUNI = new UDT_Cantidad();
            this.ValorUNI = new UDT_Valor();
            this.Valor1LOC = new UDT_Valor();
            this.Valor2LOC = new UDT_Valor();
            this.Valor3LOC = new UDT_Valor();
            this.Valor4LOC = new UDT_Valor();
            this.Valor5LOC = new UDT_Valor();
            this.Valor6LOC = new UDT_Valor();
            this.Valor7LOC = new UDT_Valor();
            this.Valor8LOC = new UDT_Valor();
            this.Valor9LOC = new UDT_Valor();
            this.Valor10LOC = new UDT_Valor();
            this.Valor1EXT = new UDT_Valor();
            this.Valor2EXT = new UDT_Valor();
            this.Valor3EXT = new UDT_Valor();
            this.Valor4EXT = new UDT_Valor();
            this.Valor5EXT = new UDT_Valor();
            this.Valor6EXT = new UDT_Valor();
            this.Valor7EXT = new UDT_Valor();
            this.Valor8EXT = new UDT_Valor();
            this.Valor9EXT = new UDT_Valor();
            this.Valor10EXT = new UDT_Valor();
            this.InReferenciaCodID = new UDT_inReferenciaID();
            this.Consecutivo = new UDT_Consecutivo();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.CantidadDEV = new UDT_Cantidad();
            this.ConsecutivoPrestamo = new UDT_Consecutivo();
            this.ConsecutivoOrdCompra = new UDT_Consecutivo();
            this.NroItem = new UDTSQL_int();
            this.ImprimeInd = new UDT_SiNo();
            //Campos adicionales
            this.PrefijoID = new UDT_PrefijoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.Fecha = new UDTSQL_smalldatetime();
            this.PlaquetaID = new UDT_PlaquetaID();
            this.Descriptivo = new UDT_DescripTBase();
            this.Prefijo_Documento = new UDTSQL_char(20);
            this.EntradaSalidaLetras = new UDTSQL_char(1);
            this.ValorActualUniLOC = new UDT_Valor();
            this.ValorActualUniEXT = new UDT_Valor();
            this.NuevoValorUniLOC = new UDT_Valor();
            this.NuevoValorUniEXT = new UDT_Valor();
            this.ValorAjusteUniLOC = new UDT_Valor();
            this.ValorAjusteUniEXT = new UDT_Valor();
            this.MarcaInvID = new UDT_CodigoGrl();
            this.MarcaDesc = new UDT_Descriptivo();
            this.RefProveedor = new UDT_CodigoGrl20();
            this.FechaIni = new UDTSQL_smalldatetime();
            this.FechaFin = new UDTSQL_smalldatetime();
            this.DocumentoID = new UDT_DocumentoID();
            this.CantidadDispon = new UDT_Cantidad();
            this.CantidadRecurso = new UDT_Cantidad();
            this.TareaID = new UDT_CodigoGrl();
            this.DescriptivoTarea = new UDT_DescripTExt();
            this.EstadoDocCtrl = new UDTSQL_tinyint();
            this.FacturaTipoID = new UDT_FacturaTipoID();
        }

        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public int Index { get; set; }

        [DataMember]
        //[NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        //[NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_BodegaID BodegaID { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDTSQL_tinyint EntradaSalida { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_inReferenciaID Kit { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDTSQL_tinyint EstadoInv { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ParametroInvID Parametro1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ParametroInvID Parametro2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo IdentificadorTr { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_ServicioID ServicioID { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_SerialID SerialID { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_ActivoID ActivoID { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_MvtoTipoID MvtoTipoInvID { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_MvtoTipoID MvtoTipoActID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo DocSoporte { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_varchar DocSoporteTER { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_AsesorID AsesorID { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt DescripTExt { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd3 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd4 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd5 { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_EmpaqueInvID EmpaqueInvID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Cantidad CantidadEMP { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Cantidad CantidadDoc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Cantidad CantidadUNI { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorUNI { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor1LOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor2LOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor3LOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor4LOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor5LOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor6LOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor7LOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor8LOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor9LOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor10LOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor1EXT { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor2EXT { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor3EXT { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor4EXT { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor5EXT { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor6EXT { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor7EXT { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor8EXT { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor9EXT { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor10EXT { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_inReferenciaID InReferenciaCodID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_LineaPresupuestoID LineaPresupuestoID{ get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Cantidad CantidadDEV { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo ConsecutivoPrestamo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo ConsecutivoOrdCompra { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_int NroItem { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SiNo ImprimeInd { get; set; }


        #region Campos adicionales 

        [DataMember]
        [AllowNull]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PlaquetaID PlaquetaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char Prefijo_Documento { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char EntradaSalidaLetras { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorActualUniLOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorActualUniEXT { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor NuevoValorUniLOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor NuevoValorUniEXT { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorAjusteUniLOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorAjusteUniEXT { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_CodigoGrl MarcaInvID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Descriptivo MarcaDesc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_CodigoGrl20 RefProveedor { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime FechaIni { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime FechaFin { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Cantidad CantidadDispon { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Cantidad CantidadRecurso { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_CodigoGrl TareaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt DescriptivoTarea { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint EstadoDocCtrl { get; set; }
        
        [DataMember]
        [AllowNull]
        public UDT_FacturaTipoID FacturaTipoID { get; set; }
        
        #endregion

        #endregion
    }
}
