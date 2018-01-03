using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// 
    /// Models DTO_pyProyectoTareaCliente
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyProyectoTareaCliente
    {
        #region DTO_pyProyectoTareaCliente
        public DTO_pyProyectoTareaCliente(IDataReader dr)
        {
            InitCols();
            try
            {    
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.TareaEntregable.Value = Convert.ToString(dr["TareaEntregable"]);
                this.Descripcion.Value = Convert.ToString(dr["Descripcion"]);
                if (!string.IsNullOrEmpty(dr["FechaInicio"].ToString()))
                    this.FechaInicio.Value = Convert.ToDateTime(dr["FechaInicio"]);
                if (!string.IsNullOrEmpty(dr["FechaFinal"].ToString()))
                    this.FechaFinal.Value = Convert.ToDateTime(dr["FechaFinal"]);
                if (!string.IsNullOrEmpty(dr["FacturaInd"].ToString()))
                    this.FacturaInd.Value = Convert.ToBoolean(dr["FacturaInd"]);
                this.ServicioID.Value = Convert.ToString(dr["ServicioID"]);
                this.MonedaFactura.Value = Convert.ToString(dr["MonedaFactura"]);
                if (!string.IsNullOrEmpty(dr["ValorFactura"].ToString()))
                    this.ValorFactura.Value = Convert.ToDecimal(dr["ValorFactura"]);
                this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                if (!string.IsNullOrEmpty(dr["Cantidad"].ToString()))
                    this.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
                this.ValorAEntregar.Value = 0;
                this.VlrTotalTareas.Value = 0;
                this.SelectInd.Value = false;
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
        public DTO_pyProyectoTareaCliente()
        {
            InitCols();
            this.SelectInd.Value = false;
            this.ValorAEntregar.Value = 0;
            this.VlrTotalTareas.Value = 0;
        }

        public void InitCols() 
        {
            this.NumeroDoc = new UDT_Consecutivo();          
            this.TareaEntregable= new UDT_CodigoGrl20();
            this.Descripcion = new UDT_DescripTExt();
            this.FechaInicio = new UDTSQL_smalldatetime();
            this.FechaFinal = new UDTSQL_smalldatetime();
            this.FacturaInd = new UDT_SiNo();
            this.ServicioID = new UDT_ServicioID();
            this.ServicioDesc = new UDT_Descriptivo();
            this.MonedaFactura = new UDT_MonedaID();   
            this.ValorFactura = new  UDT_Valor();  
            this.Observaciones = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();
            this.Cantidad = new UDT_Cantidad();
            this.CantEntregada = new UDT_Cantidad();
            this.Detalle = new List<DTO_pyProyectoPlanEntrega>();
            this.DetalleActas = new List<DTO_pyActaEntregaDeta>();
            this.DetalleTareas = new List<DTO_pyProyectoTarea>();
            //Adicionales
            this.SelectInd = new UDT_SiNo();
            this.PorAEntregar = new UDT_PorcentajeID();
            this.ValorAEntregar = new UDT_Valor();
            this.VlrTotalTareas = new UDT_Valor();
            this.ValorEntregado = new UDT_Valor();
            this.NumeroDocActa = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 TareaEntregable { get; set; }

        [DataMember]
        public UDT_DescripTExt Descripcion { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicio { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFinal { get; set; }

        [DataMember]
        public UDT_SiNo FacturaInd { get; set; }

        [DataMember]
        public UDT_ServicioID ServicioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ServicioDesc { get; set; }

        [DataMember]
        public UDT_MonedaID  MonedaFactura { get; set; }

        [DataMember]
        public UDT_Valor ValorFactura { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }
        
        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }

        [DataMember]
        public UDT_Cantidad CantEntregada { get; set; }

        [DataMember]
        public List<DTO_pyProyectoPlanEntrega> Detalle { get; set; }

        [DataMember]
        public List<DTO_pyActaEntregaDeta> DetalleActas { get; set; }

        [DataMember]
        public List<DTO_pyProyectoTarea> DetalleTareas { get; set; }

        //Adicionales

        [DataMember]
        public UDT_SiNo SelectInd { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorAEntregar { get; set; }

        [DataMember]
        public UDT_Valor ValorAEntregar { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocActa { get; set; }

        [DataMember]
        public UDT_Valor VlrTotalTareas { get; set; }

        [DataMember]
        public UDT_Valor ValorEntregado { get; set; }


        #endregion        

    }
}
