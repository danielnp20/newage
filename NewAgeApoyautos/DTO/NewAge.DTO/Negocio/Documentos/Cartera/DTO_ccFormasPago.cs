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
    /// Class comprobante para aprobacion:
    /// Models DTO_ccFormasPago.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccFormasPago
    {
        #region DTO_ccFormasPago
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccFormasPago()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.FormaPagoID = new UDTSQL_char(5);
            this.Descripcion = new UDT_DescripTBase();
            this.VlrPago = new UDT_Valor();
            this.VlrPago.Value = 0;
            this.Documento = new UDTSQL_int();
        }

        #region Propiedades

        [DataMember]
        public UDTSQL_char FormaPagoID { get; set; }

        [DataMember]
        public UDT_DescripTBase Descripcion { get; set; }

        [DataMember]
        public UDTSQL_int Documento { get; set; }

        [DataMember]
        public UDT_Valor VlrPago { get; set; }
        #endregion

        #endregion

    }
}
