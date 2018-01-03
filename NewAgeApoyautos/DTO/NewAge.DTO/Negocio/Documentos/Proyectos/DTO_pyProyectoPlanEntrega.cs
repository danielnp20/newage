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
    /// Models DTO_pyProyectoPlanEntrega
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyProyectoPlanEntrega
    {
        #region DTO_pyProyectoPlanEntrega
        public DTO_pyProyectoPlanEntrega(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ConsecTarea.Value = Convert.ToInt32(dr["ConsecTarea"]);
                if (!string.IsNullOrEmpty(dr["FechaEntrega"].ToString()))
                    this.FechaEntrega.Value = Convert.ToDateTime(dr["FechaEntrega"]);
                this.TipoEntrega.Value = Convert.ToByte(dr["TipoEntrega"]);
                if (!string.IsNullOrEmpty(dr["PorEntrega"].ToString()))
                    this.PorEntrega.Value = Convert.ToDecimal(dr["PorEntrega"]);
                if (!string.IsNullOrEmpty(dr["FacturaInd"].ToString()))
                    this.FacturaInd.Value = Convert.ToBoolean(dr["FacturaInd"]);
                if (!string.IsNullOrEmpty(dr["ValorFactura"].ToString()))
                    this.ValorFactura.Value = Convert.ToDecimal(dr["ValorFactura"]);
                this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
                if (!string.IsNullOrEmpty(dr["FechaRecaudoFactura"].ToString()))
                    this.FechaRecaudoFactura.Value = Convert.ToDateTime(dr["FechaRecaudoFactura"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                if (!string.IsNullOrEmpty(dr["Cantidad"].ToString()))
                    this.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
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
        public DTO_pyProyectoPlanEntrega()
        {
            InitCols();
            this.PorEntrega.Value = 0;
            this.ValorFactura.Value = 0;
            this.PorEntregado.Value = 0;
            this.PorPendiente.Value = 0;
            this.ValorAEntregar.Value = 0;
        }

        public void InitCols() 
        {
            this.ConsecTarea = new UDT_Consecutivo();   
            this.FechaEntrega = new UDTSQL_smalldatetime();
            this.TipoEntrega = new UDTSQL_tinyint();
            this.PorEntrega = new UDT_PorcentajeID();
            this.FacturaInd = new UDT_SiNo();
            this.ValorFactura = new  UDT_Valor();  
            this.Observaciones = new UDT_DescripTExt();
            this.FechaRecaudoFactura = new UDTSQL_smalldatetime();
            this.Cantidad = new UDT_Cantidad();
            this.Consecutivo = new UDT_Consecutivo();
            //Adicionales
            this.TareaEntregable = new UDT_CodigoGrl20();
            this.PorEntregado = new UDT_PorcentajeID();
            this.PorPendiente = new UDT_PorcentajeID();
            this.PorAEntregar = new UDT_PorcentajeID();
            this.ValorAEntregar = new UDT_Valor();
            this.DetalleTareas = new List<DTO_pyProyectoTarea>();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo ConsecTarea { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaEntrega { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoEntrega { get; set; } 

        [DataMember]
        public UDT_PorcentajeID  PorEntrega { get; set; }

        [DataMember]
        public UDT_SiNo FacturaInd { get; set; } 

        [DataMember]
        public UDT_Valor ValorFactura { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaRecaudoFactura { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }

        //Adicionales
        [DataMember]
        public UDT_CodigoGrl20 TareaEntregable { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorEntregado { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorPendiente { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorAEntregar { get; set; }

        [DataMember]
        public UDT_Valor ValorAEntregar { get; set; }

        [DataMember]
        public List<DTO_pyProyectoTarea> DetalleTareas { get; set; }

        [DataMember]
        public int Index { get; set; }

        #endregion        

    }
}
