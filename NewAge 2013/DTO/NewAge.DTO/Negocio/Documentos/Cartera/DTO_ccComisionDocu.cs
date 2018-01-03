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
    /// Models DTO_ccComisionDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccComisionDocu
    {
        #region DTO_ccComisionDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccComisionDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.FechaInicial.Value = Convert.ToDateTime(dr["FechaInicial"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFinal"].ToString()))
                    this.FechaFinal.Value = Convert.ToDateTime(dr["FechaFinal"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["Iva"].ToString()))
                    this.Iva.Value = Convert.ToDecimal(dr["Iva"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccComisionDocu()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.FechaInicial = new UDTSQL_smalldatetime();
            this.FechaFinal = new UDTSQL_smalldatetime();
            this.Valor = new UDT_Valor();
            this.Iva = new UDT_Valor();

        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicial { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFinal { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor Iva { get; set; }

        #endregion
    }
}
