using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Models DTO_ConveniosResumen
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ConveniosResumen
    {
        #region DTO_ConveniosResumen

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ConveniosResumen(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Seleccionar.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.MonedaIDConvenio.Value = dr["MonedaIDConvenio"].ToString();
                this.PrefDoc.Value = dr["PrefDoc"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ConveniosResumen()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Seleccionar = new UDT_SiNo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.ProveedorID = new UDT_ProveedorID();
            this.Descriptivo = new UDT_DescripTExt();
            this.MonedaIDConvenio = new UDT_MonedaID();
            this.Valor = new UDT_Valor();
            this.IVA = new UDT_Valor();
            this.PrefDoc = new UDT_DescripTBase();
            this.Detalle = new List<DTO_ConveniosResumenDet>();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public UDT_SiNo Seleccionar { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        public UDT_DescripTExt Descriptivo { get; set; }

        [DataMember]
        public UDT_DescripTBase PrefDoc { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaIDConvenio { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor IVA { get; set; }

        [DataMember]
        public List<DTO_ConveniosResumenDet> Detalle { get; set; }

        #endregion
    }

    //Detalle 
    public class DTO_ConveniosResumenDet
    {
        public DTO_ConveniosResumenDet(IDataReader dr)
        {
            InitDetCols();
            try
            {
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.DescripDetalle.Value = dr["DescripDetalle"].ToString();
                this.SerialID.Value = dr["SerialID"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaConsumo"].ToString()))
                    this.FechaConsumo.Value = Convert.ToDateTime(dr["FechaConsumo"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadConvenio"].ToString()))
                    this.CantidadConvenio.Value = Convert.ToDecimal(dr["CantidadConvenio"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorUni"].ToString()))
                    this.ValorUni.Value = Convert.ToDecimal(dr["ValorUni"]);
                if (!string.IsNullOrWhiteSpace(dr["IVAUni"].ToString()))
                    this.IVAUni.Value = Convert.ToDecimal(dr["IVAUni"]);
                if (!string.IsNullOrWhiteSpace(dr["ConsecutivoDetaID"].ToString()))
                    this.ConsecutivoDetaID.Value = Convert.ToInt32(dr["ConsecutivoDetaID"]);
                if (!string.IsNullOrWhiteSpace(dr["ConsumoDocuID"].ToString()))
                    this.ConsumoDocuID.Value = Convert.ToInt32(dr["ConsumoDocuID"]);
                if (!string.IsNullOrWhiteSpace(dr["ConsumoDetaID"].ToString()))
                    this.ConsumoDetaID.Value = Convert.ToInt32(dr["ConsumoDetaID"]);
                if (!string.IsNullOrWhiteSpace(dr["DocumentoNro"].ToString()))
                    this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                if (!string.IsNullOrWhiteSpace(dr["SolicitudDespachoDocuID"].ToString()))
                    this.SolicitudDespachoDocuID.Value = Convert.ToInt32(dr["SolicitudDespachoDocuID"]);
                if (!string.IsNullOrWhiteSpace(dr["SolicitudDespachoDetaID"].ToString()))
                    this.SolicitudDespachoDetaID.Value = Convert.ToInt32(dr["SolicitudDespachoDetaID"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_ConveniosResumenDet()
        {
            InitDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.DescripDetalle = new UDT_Descriptivo();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.FechaConsumo = new UDTSQL_datetime();
            this.CantidadConvenio = new UDT_Cantidad();
            this.ValorUni = new UDT_Valor();
            this.ValorTotal = new UDT_Valor();
            this.IVAUni = new UDT_Valor();
            this.IVATotal = new UDT_Valor();
            this.SerialID = new UDT_SerialID();

            this.ConsecutivoDetaID = new UDT_Consecutivo();
            this.ConsumoDocuID = new UDT_Consecutivo();
            this.ConsumoDetaID = new UDT_Consecutivo();
            this.SolicitudDespachoDocuID = new UDT_Consecutivo();
            this.SolicitudDespachoDetaID = new UDT_Consecutivo();
            this.DocumentoNro = new UDT_Consecutivo();

            this.Documento1ID = new UDT_Consecutivo();
            this.Documento2ID = new UDT_Consecutivo();
            this.Documento3ID = new UDT_Consecutivo();
            this.Documento4ID = new UDT_Consecutivo();
            this.Documento5ID = new UDT_Consecutivo();
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
            this.DatoAdd1 = new UDTSQL_char(20);
            this.DatoAdd2 = new UDTSQL_char(20);
            this.DatoAdd3 = new UDTSQL_char(20);
            this.DatoAdd4 = new UDTSQL_char(20);
            this.DatoAdd5 = new UDTSQL_char(20);
        }

        #region Propiedades

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Descriptivo DescripDetalle { get; set; }

        [DataMember]
        public UDT_SerialID SerialID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaConsumo { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadConvenio { get; set; } //CantidadDoc1 BD, cantidad del Consumo o SolicDespacho

        [DataMember]
        public UDT_Valor ValorUni { get; set; }

        [DataMember]
        public UDT_Valor ValorTotal { get; set; }

        [DataMember]
        public UDT_Valor IVAUni { get; set; }

        [DataMember]
        public UDT_Valor IVATotal { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsecutivoDetaID { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsumoDocuID { get; set; } //Documento1ID BD, Nro Doc del Consumo

        [DataMember]
        public UDT_Consecutivo ConsumoDetaID { get; set; } //Detalle1ID BD, ConsecutivoDeta del Consumo

        [DataMember]
        public UDT_Consecutivo SolicitudDespachoDocuID { get; set; } //Documento1ID BD, Nro Doc de la solicitud Despacho

        [DataMember]
        public UDT_Consecutivo SolicitudDespachoDetaID { get; set; } //Detalle1ID BD, ConsecutivoDeta de la solicitud Despacho

        [DataMember]
        public UDT_Consecutivo DocumentoNro { get; set; }//DocumentoNro BD, Consecutivo de Doc del Consumo o SolicDespacho

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
        public UDTSQL_char DatoAdd1 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd2 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd3 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd4 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd5 { get; set; }

        #endregion
    }
}
