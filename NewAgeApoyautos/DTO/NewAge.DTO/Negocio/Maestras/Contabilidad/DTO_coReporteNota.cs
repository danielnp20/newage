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
    /// Models DTO_coReporteNota
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coReporteNota : DTO_MasterBasic 
    {
        #region DTO_coReporte
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coReporteNota(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if(!isReplica)
                {
                    this.NiifDesc.Value = dr["NiifDesc"].ToString();
                }
                this.NotaRevelacionID.Value = dr["NotaRevelacionID"].ToString();
                this.NiifID.Value = dr["NiifID"].ToString();
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coReporteNota() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NotaRevelacionID = new UDT_BasicID();
            this.NiifID = new UDT_BasicID();
            this.NiifDesc = new UDT_Descriptivo();
        }

        public DTO_coReporteNota(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coReporteNota(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID NotaRevelacionID { get; set; }

        [DataMember]
        public UDT_BasicID NiifID { get; set; }

        [DataMember]
        public UDT_Descriptivo NiifDesc { get; set; }
    }
}
