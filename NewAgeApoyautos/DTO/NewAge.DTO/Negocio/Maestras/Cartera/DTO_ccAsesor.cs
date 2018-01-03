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
    /// Models DTO_caAsesor
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccAsesor : DTO_MasterBasic
    {
        #region ccAsesor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccAsesor(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                    this.ConcesionarioDesc.Value = dr["ConcesionarioDesc"].ToString();
                }
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.FechaIngreso.Value = Convert.ToDateTime(dr["FechaIngreso"]);
                this.PorcComision.Value = Convert.ToDecimal(dr["PorcComision"]);
                this.ConcesionarioID.Value = dr["ConcesionarioID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_ccAsesor()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_BasicID();
            this.FechaIngreso = new UDTSQL_smalldatetime();
            this.PorcComision = new UDT_PorcentajeID();
            this.ConcesionarioID = new UDT_CodigoGrl10();
            this.ConcesionarioDesc = new UDT_Descriptivo();
        }

        public DTO_ccAsesor(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccAsesor(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaIngreso { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorcComision { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 ConcesionarioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConcesionarioDesc { get; set; }

    }

}
