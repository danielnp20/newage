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
    /// Models DTO_pyServicio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyServicio
    {
        #region DTO_pyServicio

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyServicio(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ClaseServicioID.Value = Convert.ToString(dr["ClaseServicioID"]);
                this.LineaFlujoID.Value = Convert.ToString(dr["LineaFlujoID"]);
                this.ActividadEtapaID.Value = Convert.ToString(dr["ActividadEtapaID"]);
                this.TareaID.Value = Convert.ToString(dr["TareaID"]);
                this.TrabajoID.Value = Convert.ToString(dr["TrabajoID"]);
                this.SemanaPrograma.Value = Convert.ToInt32(dr["SemanaPrograma"]);
                this.SemanaProgramaFin.Value = Convert.ToInt32(dr["SemanaProgramaFin"]);
                this.UsuarioResponsable.Value = Convert.ToInt32(dr["UsuarioResponsable"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["CentroCostoID"].ToString()))
                    this.CentroCostoID.Value = Convert.ToString(dr["CentroCostoID"]);                
                if (!string.IsNullOrWhiteSpace(dr["FechaIniciaPRO"].ToString()))
                    this.FechaIniciaPRO.Value = Convert.ToDateTime(dr["FechaIniciaPRO"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["FechaTerminaPRO"].ToString()))
                    this.FechaTerminaPRO.Value = Convert.ToDateTime(dr["FechaTerminaPRO"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["FechaInicia"].ToString()))
                    this.FechaInicia.Value = Convert.ToDateTime(dr["FechaInicia"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["FechaTermina"].ToString()))
                    this.FechaTermina.Value = Convert.ToDateTime(dr["FechaTermina"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["FechaCierre"].ToString()))
                    this.FechaCierre.Value = Convert.ToDateTime(dr["FechaCierre"].ToString());
                this.Estado.Value =  Convert.ToByte(dr["Estado"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["NumDocTarea"].ToString()))
                    this.NumDocTarea.Value = Convert.ToInt32(dr["NumDocTarea"].ToString());           
                if (!string.IsNullOrWhiteSpace(dr["NumDocEntrega"].ToString()))
                    this.NumDocEntrega.Value = Convert.ToInt32(dr["NumDocEntrega"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["NumDocFactura"].ToString()))
                    this.NumDocFactura.Value = Convert.ToInt32(dr["NumDocFactura"].ToString());           
                this.Observaciones.Value = Convert.ToString(dr["Observaciones"].ToString());
                this.CantidadTR.Value = Convert.ToDecimal(dr["CantidadTR"].ToString());

                if (!string.IsNullOrWhiteSpace(dr["ActividadEtapaIDDesc"].ToString()))
                    this.ActividadEtapaIDDesc.Value = dr["ActividadEtapaIDDesc"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TareaIDDesc"].ToString()))
                    this.TareaIDDesc.Value = dr["TareaIDDesc"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TrabajoIDDesc"].ToString()))
                    this.TrabajoIDDesc.Value = dr["TrabajoIDDesc"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["LineaFlujoIDDesc"].ToString()))
                    this.LineaFlujoIDDesc.Value = dr["LineaFlujoIDDesc"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CentroCostoIDDesc"].ToString()))
                    this.CentroCostoIDDesc.Value = dr["CentroCostoIDDesc"].ToString();
               
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
        public DTO_pyServicio()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.ClaseServicioID = new UDT_CodigoGrl();
            this.LineaFlujoID = new UDT_CodigoGrl();
            this.ActividadEtapaID = new UDT_CodigoGrl();
            this.TareaID = new UDT_CodigoGrl();
            this.TrabajoID = new UDT_CodigoGrl(); 
            this.SemanaPrograma = new UDTSQL_int();
            this.SemanaProgramaFin = new UDTSQL_int();
            this.UsuarioResponsable = new UDT_seUsuarioID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.FechaIniciaPRO = new UDTSQL_smalldatetime();
            this.FechaTerminaPRO = new UDTSQL_smalldatetime();
            this.FechaInicia = new UDTSQL_smalldatetime();
            this.FechaTermina = new UDTSQL_smalldatetime();
            this.FechaCierre = new UDTSQL_smalldatetime();
            this.Estado = new UDTSQL_tinyint();
            this.NumDocEntrega = new UDT_Consecutivo();
            this.NumDocFactura = new UDT_Consecutivo();
            this.NumDocTarea = new UDT_Consecutivo();;
            this.Observaciones = new UDT_DescripTBase();
            this.CantidadTR = new UDT_Cantidad();
            this.ActividadEtapaIDDesc = new UDT_DescripTBase();
            this.TareaIDDesc = new UDT_DescripTBase();
            this.TrabajoIDDesc = new UDT_DescripTBase();
            this.LineaFlujoIDDesc = new UDT_DescripTBase();
            this.CentroCostoIDDesc = new UDT_DescripTBase();
            this.IndUpdate = false;
            
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_CodigoGrl ClaseServicioID { get; set; }

        [DataMember]
        public UDT_CodigoGrl LineaFlujoID { get; set; }

        [DataMember]
        public UDT_CodigoGrl ActividadEtapaID { get; set; }

        [DataMember]
        public UDT_CodigoGrl TrabajoID { get; set; }

        [DataMember]
        public UDTSQL_int SemanaPrograma { get; set; }

        [DataMember]
        public UDTSQL_int SemanaProgramaFin { get; set; }   
        
        [DataMember]
        public UDT_seUsuarioID UsuarioResponsable { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaIniciaPRO { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaTerminaPRO { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicia { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaTermina { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCierre { get; set; }

        [DataMember]
        public UDTSQL_tinyint Estado { get; set; }

        [DataMember]
        public UDT_DescripTBase Observaciones { get; set; }      

        [DataMember]
        public UDT_Consecutivo NumDocTarea { get; set; }
        
        [DataMember]
        public UDT_Consecutivo NumDocEntrega { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocFactura { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_CodigoGrl TareaID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadTR { get; set; }

        #endregion

        #region Descriptivos

        [DataMember]
        public UDT_DescripTBase LineaFlujoIDDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase ActividadEtapaIDDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase TareaIDDesc { get; set; }
        
        [DataMember]
        public UDT_DescripTBase TrabajoIDDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase CentroCostoIDDesc { get; set; }       
        
        #endregion 

        #region Adicionales

        [DataMember]
        public bool IndUpdate { get; set; }

        [DataMember]
        public List<DTO_pyProyectoDeta> Detalle { get; set; }

        #endregion

    }
}
