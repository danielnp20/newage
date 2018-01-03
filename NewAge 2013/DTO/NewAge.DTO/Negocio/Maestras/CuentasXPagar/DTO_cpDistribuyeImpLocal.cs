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
    /// Models DTO_cpDistribuyeImpLocal
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_cpDistribuyeImpLocal : DTO_MasterComplex
    {
        #region DTO_cpDistribuyeImpLocal

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_cpDistribuyeImpLocal(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.LugarGeograficoOriDesc.Value = dr["LugarGeograficoOriDesc"].ToString();
                    this.LugarGeograficoDesc.Value = dr["LugarGeograficoDesc"].ToString();
                }

                this.LugarGeograficoORI.Value = dr["LugarGeograficoORI"].ToString();
                this.LugarGeograficoID.Value = dr["LugarGeograficoID"].ToString();
                this.Porcentaje.Value = Convert.ToDecimal(dr["Porcentaje"]);
            }
            catch(Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_cpDistribuyeImpLocal() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.LugarGeograficoORI = new UDT_BasicID();
            this.LugarGeograficoOriDesc = new UDT_Descriptivo();
            this.LugarGeograficoID = new UDT_BasicID();
            this.LugarGeograficoDesc = new UDT_Descriptivo();
            this.Porcentaje = new UDT_PorcentajeID();      
        }

        public DTO_cpDistribuyeImpLocal(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_cpDistribuyeImpLocal(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID LugarGeograficoORI { get; set; }

        [DataMember]
        public UDT_Descriptivo LugarGeograficoOriDesc { get; set; }

        [DataMember]
        public UDT_BasicID LugarGeograficoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LugarGeograficoDesc { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje { get; set; }

    }

}
