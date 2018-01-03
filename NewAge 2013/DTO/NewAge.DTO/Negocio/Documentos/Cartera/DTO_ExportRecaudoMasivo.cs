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
    /// Models DTO_ExportRecaudoMasivo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ExportRecaudoMasivo
    {
        #region DTO_ExportRecaudoMasivo

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        public DTO_ExportRecaudoMasivo(IDataReader dr)
        {
            InitCols();
            try
            {
          
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ExportRecaudoMasivo()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Libranza = new UDT_LibranzaID();          
            this.ClienteID = new UDT_ClienteID();           
            this.Nombre = new UDTSQL_char(2000);
            this.TipoMvto = new UDT_DocumentoID();
            this.Documento = new UDT_DescripTBase();
            this.ValorCuota = new UDT_Valor();
            this.FechaNomina = new UDTSQL_smalldatetime();
            this.FechaDoc = new UDTSQL_smalldatetime();
        }
        
        #endregion

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDTSQL_char Nombre { get; set; }

        [DataMember]
        public UDT_DocumentoID TipoMvto { get; set; }

        [DataMember]
        public UDT_DescripTBase Documento { get; set; }

        [DataMember]
        public UDT_Valor ValorCuota { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaNomina { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

    }
}
