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
    /// Models DTO_coImpDeclaracionDetaRenglon
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coImpDeclaracionDetaRenglon
    {
        #region DTO_Declaracion

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coImpDeclaracionDetaRenglon(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Renglon.Value = dr["Renglon"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.ValorAjustado.Value = Convert.ToDecimal(dr["ValorAjustado"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coImpDeclaracionDetaRenglon()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.Renglon = new UDT_Renglon();
            this.Descriptivo = new UDT_DescripTBase();
            this.Valor = new UDT_Valor();
            this.ValorAjustado = new UDT_Valor();
            this.SignoSuma = new UDTSQL_tinyint();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Renglon Renglon { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor ValorAjustado { get; set; }

        [DataMember]
        public UDTSQL_tinyint SignoSuma { get; set; }

        #endregion

    }
}
