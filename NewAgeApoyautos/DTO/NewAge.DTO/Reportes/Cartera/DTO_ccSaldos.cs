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
    public class DTO_ccSaldos
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSaldos(IDataReader dr, bool isSaldoFavor)
        {
            this.InitCols();
            try
            {
                //Campos Generales
                if (!string.IsNullOrEmpty(dr["Libranza"].ToString()))
                    this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                if (!string.IsNullOrEmpty(dr["ClienteID"].ToString()))
                    this.ClienteID.Value = dr["ClienteID"].ToString();
                if (!string.IsNullOrEmpty(dr["Descriptivo"].ToString()))
                    this.Descriptivo.Value = dr["Descriptivo"].ToString();

                //Campos para el reporte de Saldos
                if (!isSaldoFavor)
                {
                    if (!string.IsNullOrEmpty(dr["PagaduriaID"].ToString()))
                        this.PagaduriaID.Value = dr["PagaduriaID"].ToString();
                    if (!string.IsNullOrEmpty(dr["CompradorCarteraID"].ToString()))
                        this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                    if (!string.IsNullOrEmpty(dr["VlrTotal"].ToString()))
                        this.VlrTotal.Value = Convert.ToDecimal(dr["VlrTotal"]);
                    if (!string.IsNullOrEmpty(dr["Capital"].ToString()))
                        this.Capital.Value = Convert.ToDecimal(dr["Capital"]);
                    if (!string.IsNullOrEmpty(dr["Interes"].ToString()))
                        this.Interes.Value = Convert.ToDecimal(dr["Interes"]);
                    if (!string.IsNullOrEmpty(dr["Seguro"].ToString()))
                        this.Seguro.Value = Convert.ToDecimal(dr["Seguro"]);
                    if (!string.IsNullOrEmpty(dr["Otros"].ToString()))
                        this.Otros.Value = Convert.ToDecimal(dr["Otros"]);
                    if (!string.IsNullOrEmpty(dr["Oferta"].ToString()))
                        this.Oferta.Value = dr["Oferta"].ToString();
                }
                    //Campos para el reporte de saldos a favor
                else
                {
                    if (!string.IsNullOrEmpty(dr["SaldoAFavor"].ToString()))
                        this.SaldoAFavor.Value = Convert.ToDecimal(dr["SaldoAFavor"]);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccSaldos()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Libranza = new UDT_LibranzaID();
            this.IdentificadorTR = new UDT_IdentificadorTR();
            this.ClienteID = new UDT_ClienteID();
            this.Descriptivo = new UDT_Descriptivo();
            this.PagaduriaID = new UDT_PagaduriaID();
            this.CompradorCarteraID = new UDTSQL_char(5);
            this.VlrTotal = new UDT_Valor();
            this.Capital = new UDT_Valor();
            this.Interes = new UDT_Valor();
            this.Seguro = new UDT_Valor();
            this.Otros = new UDT_Valor();
            this.Oferta = new UDTSQL_varchar(50);
            this.SaldoAFavor = new UDT_Valor();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_IdentificadorTR IdentificadorTR { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_PagaduriaID PagaduriaID { get; set; }

        [DataMember]
        public UDTSQL_char CompradorCarteraID { get; set; }

        [DataMember]
        public UDT_Valor VlrTotal { get; set; }

        [DataMember]
        public UDT_Valor Capital { get; set; }

        [DataMember]
        public UDT_Valor Interes { get; set; }

        [DataMember]
        public UDT_Valor Seguro { get; set; }

        [DataMember]
        public UDT_Valor Otros { get; set; }

        [DataMember]
        public UDTSQL_varchar Oferta { get; set; }

        [DataMember]
        public UDT_Valor SaldoAFavor { get; set; }

        #endregion
    }
}
