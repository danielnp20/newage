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
    /// Class comprobante para aprobacion:
    /// Models DTO_FacturaEquivalente
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_FacturaEquivalente : DTO_ComprobanteAprobacion
    {
        #region DTO_FacturaEquivalente

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_FacturaEquivalente(IDataReader dr) : base(dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = Convert.ToString(dr["TerceroID"]);
                if (!string.IsNullOrEmpty(dr["DocumentoCOM"].ToString()))
                    this.DocumentoCOM.Value = Convert.ToString(dr["DocumentoCOM"]);
                if (!string.IsNullOrEmpty(dr["FacturaEQ"].ToString()))
                    this.FacturaEQ.Value = Convert.ToInt32(dr["FacturaEQ"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_FacturaEquivalente()
            : base()
        {
            InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.TerceroID = new UDT_TerceroID();
            this.DocumentoCOM = new UDTSQL_char(20);
            this.FacturaEQ = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDTSQL_char DocumentoCOM { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Consecutivo FacturaEQ { get; set; }

        #endregion
    }
}
