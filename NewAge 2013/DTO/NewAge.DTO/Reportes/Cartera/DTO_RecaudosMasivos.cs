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
    public class DTO_RecaudosMasivos
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_RecaudosMasivos(IDataReader dr)
        {
            this.InitCols();
            try
            {
                //Campos Generales
                if (!string.IsNullOrEmpty(dr["Credito"].ToString()))
                    this.Credito.Value = Convert.ToInt32(dr["Credito"]);
                if (!string.IsNullOrEmpty(dr["ClienteID"].ToString()))
                    this.ClienteID.Value = dr["ClienteID"].ToString();
                if (!string.IsNullOrEmpty(dr["Nombre"].ToString()))
                    this.Nombre.Value = dr["Nombre"].ToString();
                if (!string.IsNullOrEmpty(dr["Capital"].ToString()))
                    this.Capital.Value = Convert.ToDecimal(dr["Capital"]);
                if (!string.IsNullOrEmpty(dr["Interes"].ToString()))
                    this.Interes.Value = Convert.ToDecimal(dr["Interes"]);
                if (!string.IsNullOrEmpty(dr["InteresMora"].ToString()))
                    this.InteresMora.Value = Convert.ToDecimal(dr["InteresMora"]);
                if (!string.IsNullOrEmpty(dr["Seguro"].ToString()))
                    this.Seguro.Value = Convert.ToDecimal(dr["Seguro"]);
                if (!string.IsNullOrEmpty(dr["InteresSeguro"].ToString()))
                    this.InteresSeguro.Value = Convert.ToDecimal(dr["InteresSeguro"]);
                if (!string.IsNullOrEmpty(dr["SaldoAFavor"].ToString()))
                    this.SaldoAFavor.Value = Convert.ToDecimal(dr["SaldoAFavor"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_RecaudosMasivos()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Credito = new UDT_LibranzaID();
            this.ClienteID = new UDT_ClienteID();
            this.Nombre = new UDT_Descriptivo();
            this.Cuota = new UDTSQL_int();
            this.NumDoc = new UDT_Consecutivo();
            this.Total = new UDT_Valor();
            this.Capital = new UDT_Valor();
            this.Interes = new UDT_Valor();
            this.InteresMora = new UDT_Valor();
            this.Seguro = new UDT_Valor();
            this.InteresSeguro = new UDT_Valor();
            this.SaldoAFavor = new UDT_Valor();
            this.Honorarios = new UDT_Valor();
            this.GastosLegal = new UDT_Valor();
            this.RecaudosDet = new List<DTO_RecaudosMasivos>();
        }
        #endregion

        #region Propiedades
        //1038_NumDoc	
        //1038_Credito	
        //Cliente	
        //1038_Total	
        //1038_CAPITAL(001)	
        //1038_INTERESES(002)	
        //1038_INTERES  MORA(005)	
        //1038_SALDO A FAVOR(100)	
        //1038_SEGURO(003)	
        //1038_INTERES SEGURO(004)

        [DataMember]
        public UDT_Consecutivo NumDoc { get; set; }

        [DataMember]
        public UDT_LibranzaID Credito { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDTSQL_int Cuota { get; set; }

        [DataMember]
        public UDT_Valor Total { get; set; }

        [DataMember]
        public UDT_Valor Capital { get; set; }

        [DataMember]
        public UDT_Valor Interes { get; set; }

        [DataMember]
        public UDT_Valor InteresMora { get; set; }

        [DataMember]
        public UDT_Valor Seguro { get; set; }

        [DataMember]
        public UDT_Valor InteresSeguro { get; set; }

        [DataMember]
        public UDT_Valor SaldoAFavor { get; set; }

        [DataMember]
        public UDT_Valor Honorarios { get; set; }

        [DataMember]
        public UDT_Valor GastosLegal { get; set; }

        [DataMember]
        public List<DTO_RecaudosMasivos> RecaudosDet { get; set; }

        #endregion
    }
}
