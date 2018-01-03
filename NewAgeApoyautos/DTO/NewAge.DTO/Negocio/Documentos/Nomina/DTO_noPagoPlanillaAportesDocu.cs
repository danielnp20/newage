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
    public class DTO_noPagoPlanillaAportesDocu
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noPagoPlanillaAportesDocu(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"].ToString());
                this.NumeroDocCXP.Value = Convert.ToInt32(dr["NumeroDocCXP"].ToString());
                this.Valor.Value = Convert.ToDecimal(dr["Valor"].ToString());
                this.Iva.Value = Convert.ToDecimal(dr["Iva"].ToString());                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noPagoPlanillaAportesDocu()
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
            this.NumeroDocCXP = new UDT_Consecutivo();
            this.Valor = new UDT_Valor();
            this.Iva = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        public UDT_Consecutivo NumeroDocCXP { get; set; }
        
        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor Iva { get; set; }

        #endregion 
    }
}
