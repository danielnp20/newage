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
    /// Models DTO_ccCompradorFinalDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCompradorFinalDocu
    {
        #region DTO_ccCompradorFinalDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCompradorFinalDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                this.CompradorFinal.Value = dr["CompradorFinal"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCompradorFinalDocu()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.CompradorFinal = new UDT_CompradorCarteraID();

            //Campo Adicional
            this.FechaReasigna = new UDTSQL_smalldatetime();
            this.TerceroID = new UDT_TerceroID();
        }

        #endregion

        #region Propiedades
        
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set;}
        
        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set;}

        [DataMember]
        public UDT_CompradorCarteraID CompradorFinal { get; set; }

        //Campo Adicional

        [DataMember]
        public UDTSQL_smalldatetime FechaReasigna { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        #endregion
    }
}
