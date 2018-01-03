using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_ccComponenteEdad
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccComponenteEdad : DTO_MasterComplex
    {
        #region DTO_ccClienteReferencia
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccComponenteEdad(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ComponenteCarteraDesc.Value = dr["ComponenteCarteraDesc"].ToString();
                }

                this.ComponenteCarteraID.Value = dr["ComponenteCarteraID"].ToString();
                this.Edad.Value = Convert.ToInt32(dr["Edad"]);
                if (!string.IsNullOrEmpty(dr["Valor"].ToString()))
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrEmpty(dr["PorcentajeID"].ToString()))
                this.PorcentajeID.Value = Convert.ToDecimal(dr["PorcentajeID"]);
                if (!string.IsNullOrEmpty(dr["FactorCesion"].ToString()))
                this.FactorCesion.Value = Convert.ToDecimal(dr["FactorCesion"]);
                
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccComponenteEdad() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ComponenteCarteraID = new UDT_BasicID();
            this.ComponenteCarteraDesc = new UDT_Descriptivo();
            this.Edad = new UDTSQL_int();
            this.Valor = new UDT_Valor();
            this.PorcentajeID = new UDT_PorcentajeID();
            this.FactorCesion = new UDT_PorcentajeID();
        }

        public DTO_ccComponenteEdad(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccComponenteEdad(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID ComponenteCarteraID { get; set; }

        [DataMember]
        public UDT_Descriptivo ComponenteCarteraDesc { get; set; }

        [DataMember]
        public UDTSQL_int Edad { get; set; }

         [DataMember]
        public UDT_Valor Valor { get; set; }
        
         [DataMember]
        public UDT_PorcentajeID PorcentajeID { get; set; }

        [DataMember]
         public UDT_PorcentajeID FactorCesion { get; set; }


    }

}
