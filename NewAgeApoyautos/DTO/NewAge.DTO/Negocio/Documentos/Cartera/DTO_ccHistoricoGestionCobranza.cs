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
    /// Models DTO_ccCompradorFinalDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccHistoricoGestionCobranza
    {
        #region ccCobranzaTareas

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccHistoricoGestionCobranza(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.CobranzaGestionID.Value = Convert.ToString(dr["CobranzaGestionID"]);
                this.CodConfirmacion.Value = Convert.ToString(dr["CodConfirmacion"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaConfirmacion"].ToString()))
                    this.FechaConfirmacion.Value = Convert.ToDateTime(dr["FechaConfirmacion"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuota"].ToString()))
                    this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaControl"].ToString()))
                    this.FechaControl.Value = Convert.ToDateTime(dr["FechaControl"]);
                this.Dato1.Value = Convert.ToString(dr["Dato1"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccHistoricoGestionCobranza()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            //Propios
            this.NumeroDoc = new UDT_Consecutivo();
            this.Fecha = new UDTSQL_smalldatetime();
            this.CobranzaGestionID = new UDT_CodigoGrl10();

            //Adicionales
            this.GestionDesc = new UDT_Descriptivo();
            this.ClienteID = new UDT_ClienteID();
            this.Destino = new UDTSQL_char(20);
            this.Nombre = new UDT_Descriptivo();
            this.Libranza = new UDT_Consecutivo();

            //Propios
            this.CodConfirmacion = new UDTSQL_char(30);
            this.FechaConfirmacion = new UDTSQL_smalldatetime();
            this.Consecutivo = new UDT_Consecutivo();
            this.VlrCuota = new UDT_Valor();
            this.FechaControl = new UDTSQL_smalldatetime();
            this.Dato1 = new UDTSQL_char(30);
            this.Dato2 = new UDTSQL_char(30);
            this.Dato3 = new UDTSQL_char(30);

            //Adicionales
             this.CartaInd = new  UDT_SiNo();
             this.CorreoInd = new UDT_SiNo();
             this.MensajeTextoInd = new UDT_SiNo();
             this.MensajeVozInd = new UDT_SiNo();
             this.ReporteInd = new UDT_SiNo();
             this.LlamadaInd = new UDT_SiNo();
             this.PlantillaCarta = new UDTSQL_varcharMAX();
             this.PlantillaEMail = new UDTSQL_varcharMAX();
             this.Referencia = new UDTSQL_char(100);
             this.Direccion = new UDT_DescripTBase();
             this.Ciudad = new UDT_Descriptivo();
             this.Telefono = new UDTSQL_char(15);
             this.Mensaje = new UDT_DescripUnFormat();
             this.Detalle = new List<DTO_ccHistoricoGestionCobranza>();
        }

        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 CobranzaGestionID { get; set; }

        //Adicionales
        [DataMember]
        public UDT_Descriptivo GestionDesc { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDTSQL_char Destino { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDT_Consecutivo Libranza { get; set; }

        [DataMember]
        public UDT_SiNo CartaInd { get; set; }

        [DataMember]
        public UDT_SiNo CorreoInd { get; set; }

        [DataMember]
        public UDT_SiNo MensajeTextoInd { get; set; }

        [DataMember]
        public UDT_SiNo MensajeVozInd { get; set; }

        [DataMember]
        public UDT_SiNo ReporteInd { get; set; }

        [DataMember]
        public UDT_SiNo LlamadaInd { get; set; }

        //Propios
        [DataMember]
        public UDTSQL_char CodConfirmacion { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaConfirmacion { get; set; }   

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaControl { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char Dato1 { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char Dato2 { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char Dato3 { get; set; }

        //Adicionales        
    
        [DataMember]
        [NotImportable]
        public UDTSQL_varcharMAX PlantillaCarta { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_varcharMAX PlantillaEMail { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char Referencia { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTBase Direccion { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo Ciudad { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char Telefono { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripUnFormat Mensaje { get; set; }

        [DataMember]
        [NotImportable]
        public List<DTO_ccHistoricoGestionCobranza> Detalle { get; set; }

        #endregion
    }
}
