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
    /// Models DTO_pySolServicio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pySolServicio
    {
        #region DTO_pySolServicio

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pySolServicio(IDataReader dr)
        {
            InitCols();
            try
            {               
                this.ClaseServicioID.Value = Convert.ToString(dr["ClaseServicioID"]);
                this.LineaFlujoID.Value = Convert.ToString(dr["LineaFlujoID"]);
                this.ActividadEtapaID.Value = Convert.ToString(dr["ActividadEtapaID"]);
                this.TareaID.Value = Convert.ToString(dr["TareaID"]);
                this.SemanaPrograma.Value = Convert.ToInt32(dr["SemanaPrograma"]);
                this.SemanaProgramaFin.Value = Convert.ToInt32(dr["SemanaProgramaFin"]);
                this.CentroCostoID.Value = Convert.ToString(dr["CentroCostoID"]);
                this.LineaPresupuestoID.Value = Convert.ToString(dr["LineaPresupuestoID"]);  
                this.TrabajoID.Value = Convert.ToString(dr["TrabajoID"]);
                if (!string.IsNullOrWhiteSpace(dr["Cantidad"].ToString()))
                 this.CantidadTR.Value = Convert.ToInt32(dr["Cantidad"]);
                if (!string.IsNullOrWhiteSpace(dr["ActividadEtapaIDDesc"].ToString()))
                    this.ActividadEtapaIDDesc.Value =dr["ActividadEtapaIDDesc"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ActividadFlujoIDDesc"].ToString()))
                    this.TareaIDDesc.Value = dr["TareaIDDesc"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CentroCostoIDDesc"].ToString()))
                    this.CentroCostoIDDesc.Value = dr["CentroCostoIDDesc"].ToString();              
                if (!string.IsNullOrWhiteSpace(dr["TrabajoIDDesc"].ToString()))
                    this.TrabajoIDDesc.Value = dr["TrabajoIDDesc"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["LineaFlujoIDDesc"].ToString()))
                    this.LineaFlujoIDDesc.Value = dr["LineaFlujoIDDesc"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["LineaPresupuestoIDDesc"].ToString()))
                    this.LineaPresupuestoIDDesc.Value = dr["LineaPresupuestoIDDesc"].ToString();
                this.Aprobado.Value = false;
                this.Rechazado.Value = false;                             
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
        public DTO_pySolServicio()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.ClaseServicioID = new UDT_CodigoGrl();
            this.LineaFlujoID = new UDT_CodigoGrl();
            this.ActividadEtapaID = new UDT_CodigoGrl();
            this.TareaID = new UDT_CodigoGrl();
            this.TrabajoID = new UDT_CodigoGrl();
            this.CantidadTR = new UDT_Cantidad();
            this.SemanaPrograma = new UDTSQL_int();
            this.SemanaProgramaFin = new UDTSQL_int();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaFlujoIDDesc = new UDT_DescripTBase();
            this.ActividadEtapaIDDesc = new UDT_DescripTBase();
            this.TareaIDDesc = new UDT_DescripTBase();           
            this.TrabajoIDDesc = new UDT_DescripTBase();
            this.CentroCostoIDDesc = new UDT_DescripTBase();
            this.Observaciones = new UDT_DescripTBase();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.LineaPresupuestoIDDesc = new UDT_DescripTBase();
            this.CostoLocal = new UDT_Valor();
            this.CostoExtra = new UDT_Valor();
            this.Aprobado = new UDT_SiNo();
            this.Rechazado = new UDT_SiNo();
        }

        #endregion

        [DataMember]
        public UDT_CodigoGrl ClaseServicioID { get; set; }

        [DataMember]
        public UDT_CodigoGrl LineaFlujoID { get; set; }

        [DataMember]
        public UDT_CodigoGrl ActividadEtapaID { get; set; }

        [DataMember]
        public UDT_CodigoGrl TareaID { get; set; }

        [DataMember]
        public UDT_CodigoGrl TrabajoID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadTR { get; set; }

        [DataMember]
        public UDTSQL_int SemanaPrograma { get; set; }

        [DataMember]
        public UDTSQL_int SemanaProgramaFin { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }       
        
        [DataMember]
        public UDT_DescripTBase Observaciones { get; set; }


        #region Descriptivos

        [DataMember]
        public UDT_DescripTBase LineaFlujoIDDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase ActividadEtapaIDDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase TareaIDDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase CentroCostoIDDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase TrabajoIDDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase LineaPresupuestoIDDesc { get; set; }

      
        #endregion 

        #region Agrupaciones
        
        [DataMember]
        public List<DTO_pyPreProyectoDeta> Detalle { get; set; }

        #endregion

        #region Otras
        
        [DataMember]
        public bool IndUpdate { get; set; }

        [DataMember]
        public UDT_Valor CostoLocal { get; set; }

        [DataMember]
        public UDT_Valor CostoExtra { get; set; }

        [DataMember]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        public UDT_SiNo Rechazado { get; set; }

        #endregion

    }
}
