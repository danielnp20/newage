using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_noLiquidacionesBases 
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noLiquidacionesBases(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"].ToString());
                this.ConceptoNOID.Value = dr["ConceptoNOID"].ToString();
                this.ConceptoBase.Value = dr["ConceptoBase"].ToString();
                if(!string.IsNullOrEmpty("Dias"))
                    this.Dias.Value = Convert.ToInt32(dr["Dias"].ToString());
                this.Valor.Value = Convert.ToDecimal(dr["Valor"].ToString());
                this.ValorAcumulado.Value = Convert.ToDecimal(dr["ValorAcumulado"].ToString());
                this.ValorBase.Value = Convert.ToDecimal(dr["ValorBase"].ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
      
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noLiquidacionesBases()
        {
            this.InitCols();
        }

          /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.ConceptoNOID = new UDT_ConceptoNOID();
            this.ConceptoBase = new UDT_ConceptoNOID();
            this.Dias = new UDTSQL_int();
            this.Valor = new UDT_Valor();
            this.ValorAcumulado = new UDT_Valor();
            this.ValorBase = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_ConceptoNOID ConceptoNOID { get; set; }

        [DataMember]
        public UDT_ConceptoNOID ConceptoBase { get; set; }

        [DataMember]
        public UDTSQL_int Dias { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor ValorAcumulado{ get; set; }

        [DataMember]
        public UDT_Valor ValorBase{ get; set; }
              
        #endregion

    }
}
