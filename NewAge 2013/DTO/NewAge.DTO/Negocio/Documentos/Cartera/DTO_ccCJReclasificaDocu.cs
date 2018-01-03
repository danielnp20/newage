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
    /// Models DTO_ccCJReclasificaDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCJReclasificaDocu
    {
        #region DTO_ccCJReclasificaDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCJReclasificaDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Empresa.Value = Convert.ToString(dr["Empresa"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.NumDocCredito.Value = Convert.ToInt32(dr["NumDocCredito"]);
                this.FechaCorte.Value = Convert.ToDateTime(dr["FechaCorte"]);
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
        public DTO_ccCJReclasificaDocu()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Empresa = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumDocCredito = new UDT_Consecutivo();
            this.FechaCorte = new UDTSQL_smalldatetime();
            this.Valor = new UDT_Valor();
            this.Iva = new UDT_Valor();

        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID Empresa { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCorte { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor Iva { get; set; }

        #endregion
    }
}
