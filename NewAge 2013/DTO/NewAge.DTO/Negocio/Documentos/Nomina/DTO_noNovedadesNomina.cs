using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_noNovedadesNomina
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noNovedadesNomina(IDataReader dr)
        {
            this.InitCols();
            try
            {  
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
                this.ConceptoNOID.Value = dr["ConceptoNOID"].ToString();
                this.ConceptoNODesc.Value = dr["Descriptivo"].ToString();
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.FijaInd.Value = Convert.ToBoolean(dr["FijaInd"]);
                this.PeriodoPago.Value = Convert.ToInt32(dr["PeriodoPago"]);
                this.OrigenNovedad.Value = Convert.ToInt32(dr["OrigenNovedad"]);
                this.ActivaInd.Value = Convert.ToBoolean(dr["ActivaInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }          
        } 

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noNovedadesNomina()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.EmpleadoID = new UDT_EmpleadoID();
            this.ConceptoNOID = new UDT_ConceptoNOID();
            this.ConceptoNODesc = new UDT_DescripTBase();
            this.Valor = new UDT_Valor();
            this.FijaInd = new UDT_SiNo();
            this.PeriodoPago = new UDT_Consecutivo();
            this.OrigenNovedad = new UDT_Consecutivo();
            this.ActivaInd = new UDT_SiNo();
          
        }
        #endregion
        
        [DataMember]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        public UDT_ConceptoNOID ConceptoNOID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTBase ConceptoNODesc { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_SiNo FijaInd { get; set; }

        [DataMember]
        public UDT_Consecutivo PeriodoPago { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo OrigenNovedad { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo ActivaInd { get; set; }
    
    }
}
