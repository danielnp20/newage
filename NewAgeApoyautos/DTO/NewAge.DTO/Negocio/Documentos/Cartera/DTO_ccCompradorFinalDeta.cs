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
    /// Models DTO_ccCompradorFinalDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCompradorFinalDeta
    {
        #region DTO_ccCompradorFinalDeta

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCompradorFinalDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.NumDocCredito.Value = Convert.ToInt32(dr["NumDocCredito"]);
                this.Observacion.Value = dr["Observacion"].ToString();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCompradorFinalDeta()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumDocCredito = new UDT_Consecutivo();
            this.Observacion = new UDT_DescripTBase();

            //Campos Adicionales
            this.Aprobado = new UDT_SiNo();
            this.ClienteID = new UDT_ClienteID();
            this.CompradorFinal = new UDT_CompradorCarteraID();
            this.LibranzaID = new UDT_LibranzaID();
            this.Nombre = new UDT_Descriptivo();
            this.VlrLibranza = new UDT_Valor();
            this.TerceroID = new UDT_TerceroID();
        }

        #endregion

        #region Propiedades
        
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set;}
        
        [DataMember]
        public UDT_Consecutivo NumDocCredito { get; set;}

        [DataMember]
        public UDT_DescripTBase Observacion { get; set;}

        //Campos Adicionales
        [DataMember]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorFinal { get; set; }

        [DataMember]
        public UDT_LibranzaID LibranzaID { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        #endregion
    }
}
