using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using System.Data;

namespace NewAge.DTO.Negocio
{

    /// <summary>
    /// Class Pago Facturas:
    /// Models DTO_DetalleFactura
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_RecibosDeCaja
    {
        #region DTO_DetalleFactura

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr"></param>
        public DTO_RecibosDeCaja(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.Documento.Value = (dr["Documento"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.Nit.Value = (dr["Nit"]).ToString();
                this.Descriptivo.Value = (dr["Descriptivo"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.CajaID.Value = (dr["CajaID"]).ToString();
                this.CajaDesc.Value = (dr["CajaDesc"]).ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_RecibosDeCaja()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Nit = new UDTSQL_char(50);
            this.Documento = new UDT_DescripTBase();
            this.Fecha = new UDTSQL_smalldatetime();
            this.Descriptivo = new UDT_Descriptivo();
            this.Valor = new UDT_Valor();
            this.CajaDesc = new UDT_Descriptivo();
            this.CajaID = new UDTSQL_char(5);

            this.FechaIni = new UDTSQL_datetime();
            this.FechaFin = new UDTSQL_datetime();
        }

        #endregion

        #region Propiedades

        //Grilla
        [DataMember]
        public UDT_DescripTBase Documento { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDTSQL_char Nit { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDTSQL_char CajaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CajaDesc { get; set; }

        // CamposExtras

        [DataMember]
        public UDTSQL_datetime FechaIni { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaFin { get; set; }
        
        [DataMember]
        public string FiltroNit { get; set; }

        [DataMember]
        public string FiltroCaja { get; set; } 

        #endregion
    }
}
