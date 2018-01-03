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
    /// Models DTO_CobroJuridicoAuxiliar.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_CobroJuridicoAuxiliar
    {
        #region DTO_CobroJuridicoAuxiliar

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_CobroJuridicoAuxiliar(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                this.TerceroID.Value = Convert.ToString(dr["TerceroID"]);
                this.ClienteID.Value = Convert.ToString(dr["ClienteID"]);
                this.ClienteDesc.Value = Convert.ToString(dr["ClienteDesc"]);
                this.EstadoDeuda.Value = Convert.ToByte(dr["EstadoDeuda"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                this.FechaCont.Value = Convert.ToDateTime(dr["FechaCont"]);
                this.Componente.Value = Convert.ToString(dr["Componente"]);
                this.ConceptoSaldoID.Value = Convert.ToString(dr["ConceptoSaldoID"]);
                this.ComponenteDesc.Value = Convert.ToString(dr["ComponenteDesc"]);
                this.Comprobante.Value = Convert.ToString(dr["Comprobante"]);
                this.ComprobanteID.Value = Convert.ToString(dr["ComprobanteID"]);
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteIDNro"].ToString()))
                    this.ComprobanteIDNro.Value = Convert.ToInt32(dr["ComprobanteIDNro"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuota"].ToString()))
                    this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                if (!string.IsNullOrWhiteSpace(dr["ConsAux"].ToString()))
                    this.ConsAux.Value = Convert.ToInt32(dr["ConsAux"]);
                this.Seleccionar.Value = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_CobroJuridicoAuxiliar()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Seleccionar = new UDT_SiNo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.Libranza = new UDT_LibranzaID();
            this.TerceroID = new UDT_TerceroID();
            this.ClienteID = new UDT_ClienteID();
            this.EstadoDeuda = new UDTSQL_tinyint();
            this.ClienteDesc = new UDT_Descriptivo();
            this.DocumentoID = new UDT_DocumentoID();
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.FechaCont = new UDTSQL_smalldatetime();
            this.Componente = new UDT_ComponenteCarteraID();
            this.ComponenteDesc = new UDT_Descriptivo();
            this.ConceptoSaldoID = new UDT_ConceptoSaldoID();
            this.Comprobante = new UDT_DescripTBase();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteIDNro = new UDT_Consecutivo();
            this.VlrCuota = new UDT_Valor();             
            this.VlrCapital = new UDT_Valor();
            this.VlrPoliza = new UDT_Valor();
            this.VlrIntCapital = new UDT_Valor();
            this.VlrIntPoliza = new UDT_Valor();
            this.VlrGastos = new UDT_Valor();
            this.ConsAux = new UDT_Consecutivo();
            this.Detalle = new List<DTO_CobroJuridicoAuxiliar>();
                  
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_SiNo Seleccionar { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo ClienteDesc { get; set; }

       [DataMember]
        public UDTSQL_tinyint EstadoDeuda { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCont { get; set; }

        [DataMember]
        public UDT_ComponenteCarteraID Componente { get; set; }

        [DataMember]
        public UDT_Descriptivo ComponenteDesc { get; set; }

        [DataMember]
        public UDT_ConceptoSaldoID ConceptoSaldoID { get; set; }

        [DataMember]
        public UDT_DescripTBase Comprobante { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Consecutivo ComprobanteIDNro { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrCapital { get; set; }

        [DataMember]
        public UDT_Valor VlrPoliza { get; set; }

        [DataMember]
        public UDT_Valor VlrIntCapital { get; set; }

        [DataMember]
        public UDT_Valor VlrIntPoliza { get; set; }

        [DataMember]
        public UDT_Valor VlrGastos { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsAux { get; set; }

        [DataMember]
        public List<DTO_CobroJuridicoAuxiliar> Detalle { get; set; }

        #endregion
    }
                  
}
