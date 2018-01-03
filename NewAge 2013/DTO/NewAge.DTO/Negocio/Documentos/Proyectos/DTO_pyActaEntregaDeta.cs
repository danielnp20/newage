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
    /// Models DTO_pyActaEntregaDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyActaEntregaDeta
    {
        #region DTO_pyActaEntregaDeta
        public DTO_pyActaEntregaDeta(IDataReader dr)
        {
            InitCols();
            try
            {    
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ConsTareaCliente.Value = Convert.ToInt32(dr["ConsTareaCliente"]);
                this.ConsTareaEntrega.Value = Convert.ToInt32(dr["ConsTareaEntrega"]);
                this.NumDocProyecto.Value = Convert.ToInt32(dr["NumDocProyecto"]);
                if (!string.IsNullOrEmpty(dr["FechaEntrega"].ToString()))
                    this.FechaEntrega.Value = Convert.ToDateTime(dr["FechaEntrega"]);
                if (!string.IsNullOrEmpty(dr["EntregaFinalInd"].ToString()))
                    this.EntregaFinalInd.Value = Convert.ToBoolean(dr["EntregaFinalInd"]);
                if (!string.IsNullOrEmpty(dr["PorEntregado"].ToString()))
                    this.PorEntregado.Value = Convert.ToDecimal(dr["PorEntregado"]);
                if (!string.IsNullOrEmpty(dr["FacturaInd"].ToString()))
                    this.FacturaInd.Value = Convert.ToBoolean(dr["FacturaInd"]);
                if (!string.IsNullOrEmpty(dr["ValorFactura"].ToString()))
                    this.ValorFactura.Value = Convert.ToDecimal(dr["ValorFactura"]);
                this.RespCliente.Value = Convert.ToString(dr["RespCliente"]);
                this.UsuarioID.Value = Convert.ToString(dr["UsuarioID"]);
                if (!string.IsNullOrEmpty(dr["NumDocFactura"].ToString()))
                    this.NumDocFactura.Value = Convert.ToInt32(dr["NumDocFactura"]);
                this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);    
                //Adicionales
                this.TareaEntregable.Value = Convert.ToString(dr["TareaEntregable"]);
                this.Descripcion.Value = Convert.ToString(dr["Descripcion"]);
                if (!string.IsNullOrEmpty(dr["Cantidad"].ToString()))
                    this.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
                else
                    this.Cantidad.Value = 0;
                this.ValorAEntregar.Value = 0;
                this.CantEntregada.Value = 0;
                this.CantPendiente.Value = 0;
                this.CantAEntregar.Value = 0;
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
        public DTO_pyActaEntregaDeta()
        {
            InitCols();
            this.ValorFactura.Value = 0;
            this.ValorAEntregar.Value = 0;
            this.PorEntregado.Value = 0;
            this.CantProgramada.Value = 0;
            this.CantEntregada.Value = 0;
            this.CantPendiente.Value = 0;
            this.CantAEntregar.Value = 0;
        }

        public void InitCols() 
        {
            this.NumeroDoc = new UDT_Consecutivo();          
            this.ConsTareaCliente= new UDT_Consecutivo();
            this.ConsTareaEntrega = new UDT_Consecutivo();
            this.NumDocProyecto = new UDT_Consecutivo();
            this.FechaEntrega = new UDTSQL_smalldatetime();
            this.EntregaFinalInd = new UDT_SiNo();
            this.PorEntregado = new UDT_PorcentajeID();
            this.FacturaInd = new UDT_SiNo();   
            this.ValorFactura = new  UDT_Valor();
            this.RespCliente = new UDTSQL_char(60);
            this.UsuarioID = new UDT_UsuarioID();
            this.NumDocFactura = new UDT_Consecutivo();
            this.Observaciones = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();
            this.Cantidad = new UDT_Cantidad();
            this.Detalle = new List<DTO_pyProyectoPlanEntrega>();
            //Adicionales
            this.TareaEntregable = new UDT_CodigoGrl20();
            this.Descripcion = new UDT_DescripTBase();
            this.PorEntregaProg = new UDT_PorcentajeID();
            this.PorPendiente = new UDT_PorcentajeID();
            this.PorAEntregar = new UDT_PorcentajeID();
            this.CantProgramada = new UDT_Cantidad();
            this.CantEntregada = new UDT_Cantidad();
            this.CantPendiente = new UDT_Cantidad();
            this.CantAEntregar = new UDT_Cantidad();
            this.ValorAEntregar = new UDT_Valor();
            this.DocumentoNro = new UDTSQL_int();
            this.Estado = new UDTSQL_smallint();
            this.ConsActaProy = new UDTSQL_int();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsTareaCliente { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsTareaEntrega { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocProyecto { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaEntrega { get; set; }

        [DataMember]
        public UDT_SiNo EntregaFinalInd { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorEntregado { get; set; } 

        [DataMember]
        public UDT_SiNo FacturaInd { get; set; } 

        [DataMember]
        public UDT_Valor ValorFactura { get; set; }

        [DataMember]
        public UDTSQL_char RespCliente { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocFactura { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }

        [DataMember]
        public List<DTO_pyProyectoPlanEntrega> Detalle { get; set; }

        //Adicionales
        [DataMember]
        public UDT_CodigoGrl20 TareaEntregable { get; set; }

        [DataMember]
        public UDT_DescripTBase Descripcion { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorEntregaProg { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorPendiente { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorAEntregar { get; set; }

        [DataMember]
        public UDT_Cantidad CantProgramada { get; set; }

        [DataMember]
        public UDT_Cantidad CantEntregada { get; set; }

        [DataMember]
        public UDT_Cantidad CantPendiente { get; set; }

        [DataMember]
        public UDT_Cantidad CantAEntregar { get; set; } 

        [DataMember]
        public UDT_Valor ValorAEntregar { get; set; }

        [DataMember]
        public UDTSQL_int DocumentoNro { get; set; }

        [DataMember]
        public UDTSQL_smallint Estado { get; set; }

        [DataMember]
        public UDTSQL_int ConsActaProy { get; set; }


        #endregion        

    }
}
