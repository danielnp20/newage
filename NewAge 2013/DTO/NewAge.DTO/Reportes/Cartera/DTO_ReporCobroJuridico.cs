using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_ReporCobroJuridico
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReporCobroJuridico(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.TipoMvto.Value = Convert.ToByte(dr["TipoMvto"]);
                this.FechaMvto.Value = Convert.ToDateTime(dr["FechaMvto"]);
                if (!string.IsNullOrEmpty(dr["VlrCapital"].ToString()))
                    this.VlrCapital.Value = Convert.ToDecimal(dr["VlrCapital"]);
                if (!string.IsNullOrEmpty(dr["FechaInicial"].ToString()))
                    this.FechaInicial.Value = Convert.ToDateTime(dr["FechaInicial"]);
                if (!string.IsNullOrEmpty(dr["FechaFinal"].ToString()))
                    this.FechaFinal.Value = Convert.ToDateTime(dr["FechaFinal"]);
                if (!string.IsNullOrEmpty(dr["DiasMora"].ToString()))
                    this.DiasMora.Value = Convert.ToInt32(dr["DiasMora"]);
                if (!string.IsNullOrEmpty(dr["Tasa"].ToString()))
                    this.Tasa.Value = Convert.ToDecimal(dr["Tasa"]);
                if (!string.IsNullOrEmpty(dr["InteresMora"].ToString()))
                 this.InteresMora.Value = Convert.ToDecimal(dr["InteresMora"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReporCobroJuridico()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.FechaMvto = new UDTSQL_smalldatetime();
            this.TipoMvto = new UDTSQL_tinyint();
            this.VlrCapital = new UDT_Valor();
            this.FechaInicial = new UDTSQL_smalldatetime();
            this.FechaFinal = new UDTSQL_smalldatetime();
            this.DiasMora = new UDTSQL_int();
            this.Tasa = new UDT_PorcentajeID();
            this.InteresMora = new UDT_Valor();

            this.Detalle = new List<DTO_ReporCobroJuridico>();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint TipoMvto { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaMvto { get; set; }

        [DataMember]
        public UDT_Valor VlrCapital { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicial { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFinal { get; set; }

        [DataMember]
        public UDTSQL_int DiasMora { get; set; }

        [DataMember]
        public UDT_PorcentajeID Tasa { get; set; }

        [DataMember]
        public UDT_Valor InteresMora { get; set; }

        //Extra
        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public int index { get; set; }

        [DataMember]
        public List<DTO_ReporCobroJuridico> Detalle { get; set; }

        #endregion
    }
}
