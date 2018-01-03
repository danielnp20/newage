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
    /// Models DTO_ccSolicitudDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSolicitudDetallePago
    {
        #region DTO_ccSolicitudDetallePago

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudDetallePago(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.Documento.Value = dr["Documento"].ToString();
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccSolicitudDetallePago()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.TerceroID = new UDT_TerceroID();
            this.Documento = new UDT_DocTerceroID();
            this.Valor = new UDT_Valor();

            //Campo Adicional
            this.Nombre = new UDT_DescripTBase();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DocTerceroID Documento { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        //Campo Adicional
        [DataMember]
        public UDT_DescripTBase Nombre { get; set; }

        #endregion
    }
}
