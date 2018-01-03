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
    /// Models DTO_ConsumoProyectoResumen
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ConsumoProyectoResumen
    {
        #region DTO_prOrdenCompraResumen

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ConsumoProyectoResumen(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Seleccionar.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.MonedaIDConsumo.Value = dr["MonedaIDConsumo"].ToString();
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ConsumoProyectoResumen()
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
            this.MonedaIDConsumo = new UDT_MonedaID();
            this.Valor = new UDT_Valor();
            this.IVA = new UDT_Valor();
            this.Detalle = new List<DTO_ConsumoProyectoResumenDet>();     
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
        public UDT_MonedaID MonedaIDConsumo { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor IVA { get; set; }

        [DataMember]
        public List<DTO_ConsumoProyectoResumenDet> Detalle { get; set; }

        #endregion
    }

    //Detalle Del Consumo
    public class DTO_ConsumoProyectoResumenDet
    {
        public DTO_ConsumoProyectoResumenDet(IDataReader dr)
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
                if (!string.IsNullOrWhiteSpace(dr["CantidadConsumo"].ToString()))
                    this.CantidadConsumo.Value = Convert.ToDecimal(dr["CantidadConsumo"]);
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
                if (!string.IsNullOrWhiteSpace(dr["DocumentoNroConsumo"].ToString()))
                    this.DocumentoNroConsumo.Value = Convert.ToInt32(dr["DocumentoNroConsumo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_ConsumoProyectoResumenDet()
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
            this.CantidadConsumo = new UDT_Cantidad();
            this.ValorUni = new UDT_Valor();
            this.ValorTotal = new UDT_Valor();
            this.IVAUni = new UDT_Valor();
            this.IVATotal = new UDT_Valor();
            this.SerialID = new UDT_SerialID();          
          
            this.ConsecutivoDetaID = new UDT_Consecutivo();
            this.ConsumoDocuID = new UDT_Consecutivo();
            this.ConsumoDetaID = new UDT_Consecutivo();
            this.DocumentoNroConsumo = new UDT_Consecutivo(); 
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
        public UDT_Cantidad CantidadConsumo { get; set; } //CantidadDoc1 BD, cantidad del Consumo

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
        public UDT_Consecutivo DocumentoNroConsumo { get; set; }

        #endregion
    }
}
