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
    /// Models DTO_ccComisionDescAsesor
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccComisionDescAsesor : DTO_MasterComplex
    {
        #region DTO_ccComisionDescAsesor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccComisionDescAsesor(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica) this.AsesorDesc.Value   = dr["AsesorDesc"].ToString();
                if (!isReplica) this.ComisionDesc.Value = dr["ComisionDesc"].ToString();

                this.AsesorID.Value                     = dr["AsesorID"].ToString();
                this.ComisionDescID.Value               = dr["ComisionDescID"].ToString();
                this.Descriptivo.Value                  = dr["Descriptivo"].ToString();
                this.Valor.Value                        = Convert.ToDecimal(dr["Valor"].ToString());

            }
            catch (Exception e)
            {
                
                throw e;
            }
        }
        
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccComisionDescAsesor() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.AsesorID         = new UDT_BasicID();
            this.AsesorDesc       = new UDT_Descriptivo();
            this.ComisionDescID   = new UDT_BasicID();
            this.ComisionDesc     = new UDT_Descriptivo();
            this.Descriptivo      = new UDT_DescripTBase();
            this.Valor            = new UDT_Valor();
            
        }

        public DTO_ccComisionDescAsesor(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccComisionDescAsesor(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID AsesorID { get; set; }

        [DataMember]
        public UDT_Descriptivo AsesorDesc { get; set; }

        [DataMember]
        public UDT_BasicID ComisionDescID { get; set; }

        [DataMember]
        public UDT_Descriptivo ComisionDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

    }

}
