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
    /// Models glTareaAreaFuncional
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCobranzaGestion : DTO_MasterBasic
    {
        #region DTO_ccCobranzaGestion
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCobranzaGestion(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.EtapaIncumplimientoDesc.Value = dr["EtapaIncumplimientoDesc"].ToString();
                    this.LLamadaDesc.Value = dr["LLamadaDesc"].ToString();
                    this.GestionPreviaDesc.Value = dr["GestionPreviaDesc"].ToString();
                }

                this.EtapaIncumplimiento.Value = dr["EtapaIncumplimiento"].ToString();
                if (!string.IsNullOrEmpty(dr["Dias"].ToString()))
                    this.Dias.Value = Convert.ToInt32(dr["Dias"]);
                if (!string.IsNullOrEmpty(dr["CorreoInd"].ToString()))
                    this.CorreoInd.Value = Convert.ToBoolean(dr["CorreoInd"]);
                if (!string.IsNullOrEmpty(dr["CartaInd"].ToString()))
                    this.CartaInd.Value = Convert.ToBoolean(dr["CartaInd"]);
                if (!string.IsNullOrEmpty(dr["MensajeVozInd"].ToString()))
                    this.MensajeVozInd.Value = Convert.ToBoolean(dr["MensajeVozInd"]);
                if (!string.IsNullOrEmpty(dr["MensajeTextoInd"].ToString()))
                    this.MensajeTextoInd.Value = Convert.ToBoolean(dr["MensajeTextoInd"]);
                if (!string.IsNullOrEmpty(dr["LlamadaInd"].ToString()))
                    this.LlamadaInd.Value = Convert.ToBoolean(dr["LlamadaInd"]);
                if (!string.IsNullOrEmpty(dr["AbonoInd"].ToString()))
                    this.AbonoInd.Value = Convert.ToBoolean(dr["AbonoInd"]);
                if (!string.IsNullOrEmpty(dr["ConyugueInd"].ToString()))
                    this.ConyugueInd.Value = Convert.ToBoolean(dr["ConyugueInd"]);
                if (!string.IsNullOrEmpty(dr["CoDeudorInd"].ToString()))
                    this.CoDeudorInd.Value = Convert.ToBoolean(dr["CoDeudorInd"]);
                if (!string.IsNullOrEmpty(dr["Factor"].ToString()))
                    this.Factor.Value = Convert.ToDecimal(dr["Factor"]);
                this.Mensaje.Value = dr["Mensaje"].ToString();
                this.PlantillaCarta.Value = dr["PlantillaCarta"].ToString();
                this.PlantillaEMail.Value = dr["PlantillaEMail"].ToString();
                this.LLamadaID.Value = dr["LLamadaID"].ToString();
                if (!string.IsNullOrEmpty(dr["ReporteInd"].ToString()))
                    this.ReporteInd.Value = Convert.ToBoolean(dr["ReporteInd"]);
                this.Referencia.Value = dr["Referencia"].ToString();
                if (!string.IsNullOrEmpty(dr["RestriccionCartera"].ToString()))
                    this.RestriccionCartera.Value = Convert.ToByte(dr["RestriccionCartera"]);
                this.GestionPreviaID.Value = dr["GestionPreviaID"].ToString();
                if (!string.IsNullOrEmpty(dr["ControlTipo"].ToString()))
                    this.ControlTipo.Value = Convert.ToByte(dr["ControlTipo"]);
                if (!string.IsNullOrEmpty(dr["GestionDemanda"].ToString()))
                    this.GestionDemanda.Value = Convert.ToByte(dr["GestionDemanda"]);
                if (!string.IsNullOrEmpty(dr["CampID"].ToString()))
                    this.CampID.Value = Convert.ToInt32(dr["CampID"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCobranzaGestion()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.EtapaIncumplimiento = new UDT_BasicID();
            this.EtapaIncumplimientoDesc = new UDT_Descriptivo();
            this.Dias = new UDTSQL_int();
            this.CorreoInd = new UDT_SiNo();
            this.CartaInd = new UDT_SiNo();
            this.MensajeVozInd = new UDT_SiNo();
            this.MensajeTextoInd = new UDT_SiNo();
            this.LlamadaInd = new UDT_SiNo();
            this.AbonoInd = new UDT_SiNo();
            this.ConyugueInd = new UDT_SiNo();
            this.CoDeudorInd = new UDT_SiNo();
            this.Factor = new UDT_FactorID();
            this.Mensaje = new UDT_DescripUnFormat();
            this.PlantillaCarta = new UDT_DescripUnFormat();
            this.PlantillaEMail = new UDT_DescripUnFormat();
            this.LLamadaID = new UDT_BasicID();
            this.LLamadaDesc = new UDT_Descriptivo();
            this.ReporteInd = new UDT_SiNo();
            this.Referencia = new UDTSQL_char(300);
            this.RestriccionCartera = new UDTSQL_tinyint();
            this.GestionPreviaID = new UDT_BasicID();
            this.GestionPreviaDesc = new UDT_Descriptivo();
            this.ControlTipo = new UDTSQL_tinyint();
            this.GestionDemanda = new UDTSQL_tinyint();
            this.CampID = new UDTSQL_int();
        }

        public DTO_ccCobranzaGestion(DTO_MasterBasic comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_ccCobranzaGestion(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID EtapaIncumplimiento { get; set; }

        [DataMember]
        public UDT_Descriptivo EtapaIncumplimientoDesc { get; set; }

        [DataMember]
        public UDTSQL_int Dias { get; set; }
       
        [DataMember]
        public UDT_SiNo CorreoInd { get; set; }

        [DataMember]
        public UDT_SiNo CartaInd { get; set; }

        [DataMember]
        public UDT_SiNo MensajeVozInd { get; set; }

        [DataMember]
        public UDT_SiNo MensajeTextoInd { get; set; }

        [DataMember]
        public UDT_SiNo LlamadaInd { get; set; }

        [DataMember]
        public UDT_SiNo AbonoInd { get; set; }

        [DataMember]
        public UDT_SiNo ConyugueInd { get; set; }

        [DataMember]
        public UDT_SiNo CoDeudorInd { get; set; }

        [DataMember]
        public UDT_FactorID Factor { get; set; }

        [DataMember]
        public UDT_DescripUnFormat Mensaje { get; set; }

        [DataMember]
        public UDT_DescripUnFormat PlantillaCarta { get; set; }

        [DataMember]
        public UDT_DescripUnFormat PlantillaEMail { get; set; }

        [DataMember]
        public UDT_BasicID LLamadaID { get; set; }

        [DataMember]
        public UDT_Descriptivo LLamadaDesc { get; set; }

        [DataMember]
        public UDT_SiNo ReporteInd { get; set; }

        [DataMember]
        public UDTSQL_char Referencia { get; set; }

        [DataMember]
        public UDTSQL_tinyint RestriccionCartera { get; set; }

        [DataMember]
        public UDT_BasicID GestionPreviaID { get; set; }

        [DataMember]
        public UDT_Descriptivo GestionPreviaDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint ControlTipo { get; set; }

        [DataMember]
        public UDTSQL_tinyint GestionDemanda { get; set; }

        [DataMember]
        public UDTSQL_int CampID { get; set; }

    }
}
