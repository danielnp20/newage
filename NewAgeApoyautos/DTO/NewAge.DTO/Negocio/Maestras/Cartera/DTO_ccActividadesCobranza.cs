using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_ccActividadesCobranza
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccActividadesCobranza : DTO_MasterComplex
    {
        #region ccActividadesCobranza
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccActividadesCobranza(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ClienteDesc.Value = Convert.ToString(dr["ClienteDesc"].ToString());
                    this.ActividadFlujoDesc.Value = Convert.ToString(dr["ActividadFlujoDesc"].ToString());
                    this.LLamadaDesc.Value = dr["LLamadaDesc"].ToString();
                }
                this.ClienteID.Value = Convert.ToString(dr["ClienteID"].ToString());
                this.ActividadFlujoID.Value = Convert.ToString(dr["ActividadFlujoID"].ToString());
                if (!string.IsNullOrEmpty(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"].ToString());
                this.TipoEvento.Value = Convert.ToByte(dr["TipoEvento"].ToString());
                this.EstadoDeuda.Value = Convert.ToByte(dr["EstadoDeuda"].ToString());
                this.LLamadaID.Value = Convert.ToString(dr["LLamadaID"].ToString());
                this.TipoReferencia.Value = Convert.ToByte(dr["TipoReferencia"].ToString());
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = Convert.ToString(dr["Observacion"].ToString());
                if (!string.IsNullOrEmpty(dr["TextoCorreo"].ToString()))
                    this.TextoCorreo.Value = Convert.ToString(dr["TextoCorreo"].ToString());
                if (!string.IsNullOrEmpty(dr["PlantillaCarta"].ToString()))
                    this.PlantillaCarta.Value = Convert.ToString(dr["PlantillaCarta"].ToString());
                if (!string.IsNullOrEmpty(dr["ConsCierreDia"].ToString()))
                    this.ConsCierreDia.Value = Convert.ToInt32(dr["ConsCierreDia"].ToString());
                
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }
        
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccActividadesCobranza() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ActividadFlujoID = new UDT_BasicID();
            this.ActividadFlujoDesc = new UDT_Descriptivo();
            this.ClienteID = new UDT_BasicID();
            this.ClienteDesc = new UDT_Descriptivo();
            this.Fecha = new UDTSQL_smalldatetime();
            this.TipoEvento = new UDTSQL_tinyint();
            this.EstadoDeuda = new UDTSQL_tinyint();
            this.LLamadaID = new UDT_BasicID();
            this.LLamadaDesc = new UDT_Descriptivo();
            this.TipoReferencia = new UDTSQL_tinyint();
            this.Observacion = new UDT_DescripTExt();
            this.TextoCorreo = new UDT_DescripTExt();
            this.PlantillaCarta = new UDT_DescripTBase();
            this.ConsCierreDia = new UDT_Consecutivo();
        }

        public DTO_ccActividadesCobranza(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccActividadesCobranza(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo ClienteDesc { get; set; }

        [DataMember]
        public UDT_BasicID ActividadFlujoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadFlujoDesc { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint TipoEvento { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint EstadoDeuda { get; set; }
        
        [DataMember]
        public UDT_BasicID LLamadaID { get; set; }
        
        [DataMember]
        public UDT_Descriptivo LLamadaDesc { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint TipoReferencia { get; set; }
        
        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }
        
        [DataMember]
        public UDT_DescripTExt TextoCorreo { get; set; }
        
        [DataMember]
        public UDT_DescripTBase PlantillaCarta { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsCierreDia { get; set; }

    }

}
