using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_plCierreLegalizacion
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plCierreLegalizacion()
        {
            this.InitCols();
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plCierreLegalizacion(bool setValues)
        {
            this.InitCols();
            this.CtoOrigenLocML.Value = 0;
            this.CtoOrigenLocME.Value = 0;
            this.CtoOrigenExtME.Value = 0;
            this.CtoOrigenExtML.Value = 0;
            this.CtoInicialLocML.Value = 0;
            this.CtoInicialLocME.Value = 0;
            this.CtoInicialExtME.Value = 0;
            this.CtoInicialExtML.Value = 0;
            this.PtoInicialLocML.Value = 0;
            this.PtoInicialLocME.Value = 0;
            this.PtoInicialExtME.Value = 0;
            this.PtoInicialExtML.Value = 0;
            this.PtoMesLocML.Value =0;
            this.PtoMesLocME.Value = 0;
            this.PtoMesExtME.Value =0;
            this.PtoMesExtML.Value = 0;
            this.PtoTotalLocML.Value = 0;
            this.PtoTotalLocME.Value = 0;
            this.PtoTotalExtME.Value =0;
            this.PtoTotalExtML.Value = 0;
            this.CompActLocML.Value = 0;
            this.CompActLocME.Value =0;
            this.CompActExtME.Value = 0;
            this.CompActExtML.Value = 0;
            this.RecibidoLocML.Value =0;
            this.RecibidoLocME.Value = 0;
            this.RecibidoExtML.Value = 0;
            this.RecibidoExtME.Value = 0;
            this.CompPendLoc.Value = 0;
            this.CompPendExt.Value = 0;
        }

        /// <summary>
        /// Constructor de lectura
        /// </summary>
        /// <param name="dr"></param>
        public DTO_plCierreLegalizacion(IDataReader dr)
        {
            InitCols();
            try
            {
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.ProyectoID.Value = Convert.ToString(dr["ProyectoID"]);
                this.CentroCostoID.Value = Convert.ToString(dr["CentroCostoID"]);
                this.LineaPresupuestoID.Value = Convert.ToString(dr["LineaPresupuestoID"]);
                this.CtoOrigenLocML.Value = Convert.ToDecimal(dr["CtoOrigenLocML"]);
                this.CtoOrigenLocME.Value = Convert.ToDecimal(dr["CtoOrigenLocME"]);
                this.CtoOrigenExtME.Value = Convert.ToDecimal(dr["CtoOrigenExtME"]);
                this.CtoOrigenExtML.Value = Convert.ToDecimal(dr["CtoOrigenExtML"]);
                this.CtoInicialLocML.Value = Convert.ToDecimal(dr["CtoInicialLocML"]);
                this.CtoInicialLocME.Value = Convert.ToDecimal(dr["CtoInicialLocME"]);
                this.CtoInicialExtME.Value = Convert.ToDecimal(dr["CtoInicialExtME"]);
                this.CtoInicialExtML.Value = Convert.ToDecimal(dr["CtoInicialExtML"]);
                this.PtoInicialLocML.Value = Convert.ToDecimal(dr["PtoInicialLocML"]);
                this.PtoInicialLocME.Value = Convert.ToDecimal(dr["PtoInicialLocME"]);
                this.PtoInicialExtME.Value = Convert.ToDecimal(dr["PtoInicialExtME"]);
                this.PtoInicialExtML.Value = Convert.ToDecimal(dr["PtoInicialExtML"]);
                this.PtoMesLocML.Value = Convert.ToDecimal(dr["PtoMesLocML"]);
                this.PtoMesLocME.Value = Convert.ToDecimal(dr["PtoMesLocME"]);
                this.PtoMesExtME.Value = Convert.ToDecimal(dr["PtoMesExtME"]);
                this.PtoMesExtML.Value = Convert.ToDecimal(dr["PtoMesExtML"]);
                this.PtoTotalLocML.Value = Convert.ToDecimal(dr["PtoTotalLocML"]);
                this.PtoTotalLocME.Value = Convert.ToDecimal(dr["PtoTotalLocME"]);
                this.PtoTotalExtME.Value = Convert.ToDecimal(dr["PtoTotalExtME"]);
                this.PtoTotalExtML.Value = Convert.ToDecimal(dr["PtoTotalExtML"]);
                this.CompActLocML.Value = Convert.ToDecimal(dr["CompActLocML"]);
                this.CompActLocME.Value = Convert.ToDecimal(dr["CompActLocME"]);
                this.CompActExtME.Value = Convert.ToDecimal(dr["CompActExtME"]);
                this.CompActExtML.Value = Convert.ToDecimal(dr["CompActExtML"]);
                this.RecibidoLocML.Value = Convert.ToDecimal(dr["RecibidoLocML"]);
                this.RecibidoLocME.Value = Convert.ToDecimal(dr["RecibidoLocME"]);
                this.RecibidoExtML.Value = Convert.ToDecimal(dr["RecibidoExtML"]);
                this.RecibidoExtME.Value = Convert.ToDecimal(dr["RecibidoExtME"]);
                this.CompPendLoc.Value = Convert.ToDecimal(dr["CompPendLoc"]);
                this.CompPendExt.Value = Convert.ToDecimal(dr["CompPendExt"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.PeriodoID= new UDT_PeriodoID();
            this.ProyectoID= new UDT_ProyectoID();
            this.CentroCostoID= new UDT_CentroCostoID();
            this.LineaPresupuestoID= new UDT_LineaPresupuestoID();
            this.CtoOrigenLocML= new UDT_Valor();
            this.CtoOrigenLocME = new UDT_Valor();
            this.CtoOrigenExtME = new UDT_Valor();
            this.CtoOrigenExtML = new UDT_Valor();
            this.CtoInicialLocML = new UDT_Valor();
            this.CtoInicialLocME = new UDT_Valor();
            this.CtoInicialExtME = new UDT_Valor();
            this.CtoInicialExtML = new UDT_Valor();
            this.PtoInicialLocML = new UDT_Valor();
            this.PtoInicialLocME = new UDT_Valor();
            this.PtoInicialExtME = new UDT_Valor();
            this.PtoInicialExtML = new UDT_Valor();
            this.PtoMesLocML = new UDT_Valor();
            this.PtoMesLocME = new UDT_Valor();
            this.PtoMesExtME = new UDT_Valor();
            this.PtoMesExtML = new UDT_Valor();
            this.PtoTotalLocML = new UDT_Valor();
            this.PtoTotalLocME = new UDT_Valor();
            this.PtoTotalExtME = new UDT_Valor();
            this.PtoTotalExtML = new UDT_Valor();
            this.CompActLocML = new UDT_Valor();
            this.CompActLocME = new UDT_Valor();
            this.CompActExtME = new UDT_Valor();
            this.CompActExtML = new UDT_Valor();
            this.RecibidoLocML = new UDT_Valor();
            this.RecibidoLocME = new UDT_Valor();
            this.RecibidoExtML = new UDT_Valor();
            this.RecibidoExtME = new UDT_Valor();
            this.CompPendLoc = new UDT_Valor();
            this.CompPendExt = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }
        
        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Valor CtoOrigenLocML { get; set; }

        [DataMember]
        public UDT_Valor CtoOrigenLocME { get; set; }

        [DataMember]
        public UDT_Valor CtoOrigenExtME { get; set; }

        [DataMember]
        public UDT_Valor CtoOrigenExtML { get; set; }
        
        [DataMember]
        public UDT_Valor CtoInicialLocML { get; set; }
        
        [DataMember]
        public UDT_Valor CtoInicialLocME { get; set; }
        
        [DataMember]
        public UDT_Valor CtoInicialExtME { get; set; }
        
        [DataMember]
        public UDT_Valor CtoInicialExtML { get; set; }
        
        [DataMember]
        public UDT_Valor PtoInicialLocML { get; set; }
        
        [DataMember]
        public UDT_Valor PtoInicialLocME { get; set; }
        
        [DataMember]
        public UDT_Valor PtoInicialExtME { get; set; }
        
        [DataMember]
        public UDT_Valor PtoInicialExtML { get; set; }
        
        [DataMember]
        public UDT_Valor PtoMesLocML { get; set; }
        
        [DataMember]
        public UDT_Valor PtoMesLocME { get; set; }
        
        [DataMember]
        public UDT_Valor PtoMesExtME { get; set; }
        
        [DataMember]
        public UDT_Valor PtoMesExtML { get; set; }
        
        [DataMember]
        public UDT_Valor PtoTotalLocML { get; set; }
        
        [DataMember]
        public UDT_Valor PtoTotalLocME { get; set; }
        
        [DataMember]
        public UDT_Valor PtoTotalExtME { get; set; }
        
        [DataMember]
        public UDT_Valor PtoTotalExtML { get; set; }
        
        [DataMember]
        public UDT_Valor CompActLocML { get; set; }
        
        [DataMember]
        public UDT_Valor CompActLocME { get; set; }
        
        [DataMember]
        public UDT_Valor CompActExtME { get; set; }
        
        [DataMember]
        public UDT_Valor CompActExtML { get; set; }
        
        [DataMember]
        public UDT_Valor RecibidoLocML { get; set; }
        
        [DataMember]
        public UDT_Valor RecibidoLocME { get; set; }
        
        [DataMember]
        public UDT_Valor RecibidoExtML { get; set; }
        
        [DataMember]
        public UDT_Valor RecibidoExtME { get; set; }

        [DataMember]
        public UDT_Valor CompPendLoc { get; set; }

        [DataMember]
        public UDT_Valor CompPendExt { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }
        
        #endregion
    }
}
