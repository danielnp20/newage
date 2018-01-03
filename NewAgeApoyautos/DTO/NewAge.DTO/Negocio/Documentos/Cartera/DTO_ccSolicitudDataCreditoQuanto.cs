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
    /// Models DTO_ccSolicitudDataCreditoQuanto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSolicitudDataCreditoQuanto
    {
        #region ccSolicitudDataCreditoQuanto

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudDataCreditoQuanto(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Version.Value = Convert.ToInt32(dr["Version"]);
                this.TipoId.Value = Convert.ToString(dr["TipoId"]);
                this.NumeroId.Value = Convert.ToString(dr["NumeroId"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                this.VlrMinimo.Value = Convert.ToInt32(dr["VlrMinimo"]);
                this.VlrMedio.Value = Convert.ToInt32(dr["VlrMedio"]);
                this.VlrMaximo.Value = Convert.ToInt32(dr["VlrMaximo"]);
                this.VlrMinimoSMLV.Value = Convert.ToInt32(dr["VlrMinimoSMLV"]);
                this.VlrMedioSMLV.Value = Convert.ToInt32(dr["VlrMedioSMLV"]);
                this.VlrMaximoSMLV.Value = Convert.ToInt32(dr["VlrMaximoSMLV"]);
                this.Exclusiones.Value = Convert.ToInt32(dr["Exclusiones"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccSolicitudDataCreditoQuanto()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.Version = new UDTSQL_int();
            this.TipoId=new UDTSQL_varchar(1);
            this.NumeroId=new UDTSQL_varchar(11);
            this.VlrMinimo = new UDTSQL_int();
            this.VlrMedio= new UDTSQL_int();
            this.VlrMaximo = new UDTSQL_int();
            this.VlrMinimoSMLV = new UDTSQL_int();
            this.VlrMedioSMLV = new UDTSQL_int();
            this.VlrMaximoSMLV = new UDTSQL_int();
            this.Exclusiones = new UDTSQL_int();
            this.Consecutivo=new UDT_Consecutivo();
          }

        #endregion

        #region Propiedades
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        [DataMember]
        public UDTSQL_int Version{ get; set; }

        [DataMember]
        public UDTSQL_varchar TipoId{ get; set; }

        [DataMember]
        public UDTSQL_varchar NumeroId{ get; set;}         
        
        [DataMember]
        public UDTSQL_int VlrMinimo{ get; set; }

        [DataMember]
        public UDTSQL_int VlrMedio{ get; set; }

        [DataMember]
        public UDTSQL_int VlrMaximo { get; set; }

        [DataMember]
        public UDTSQL_int VlrMinimoSMLV { get; set; }

        [DataMember]
        public UDTSQL_int VlrMedioSMLV { get; set; }

        [DataMember]
        public UDTSQL_int VlrMaximoSMLV { get; set; }
        
        [DataMember]
        public UDTSQL_int Exclusiones { get; set; } 
        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }
        #endregion
    }
}
