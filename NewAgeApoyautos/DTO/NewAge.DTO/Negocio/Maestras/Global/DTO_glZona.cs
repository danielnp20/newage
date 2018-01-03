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
    /// Models DTO_glZona
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glZona : DTO_MasterBasic
    {
        #region glDocMigracionEstructura
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glZona(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.AreaFisicaDesc.Value = dr["AreaFisicaDesc"].ToString();
                }

                this.GerenteZona.Value = Convert.ToString(dr["GerenteZona"]);
                if (!string.IsNullOrEmpty(dr["PorcComision"].ToString()))
                    this.PorcComision.Value = Convert.ToDecimal(dr["PorcComision"]);
                this.AreaFisica.Value = Convert.ToString(dr["AreaFisica"]);                
            }
            catch (Exception e)
            {
               throw e;
            }         
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glZona()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.GerenteZona = new UDT_TerceroID();
            this.PorcComision = new UDT_PorcentajeID();
            this.AreaFisica = new UDT_BasicID();
            this.AreaFisicaDesc = new UDT_Descriptivo();
        }

        public DTO_glZona(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glZona(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
                               
        [DataMember]
        public UDT_TerceroID GerenteZona { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorcComision { get; set; }

        [DataMember]
        public UDT_BasicID AreaFisica { get; set; }

        [DataMember]
        public UDT_Descriptivo AreaFisicaDesc { get; set; }
    }

}
