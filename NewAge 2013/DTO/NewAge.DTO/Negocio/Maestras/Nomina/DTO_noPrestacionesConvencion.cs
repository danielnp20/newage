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
    /// Models DTO_noPrestacionesConvencion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noPrestacionesConvencion : DTO_MasterComplex
    {
        #region DTO_noPrestacionesConvencion
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noPrestacionesConvencion(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.ConvencionNODesc.Value = dr["ConvencionNODesc"].ToString();
                    this.ComponenteNominaDesc.Value = Convert.ToString(dr["ComponenteNominaDesc"]);
                }
                this.ConvencionNOID.Value = dr["ConvencionNOID"].ToString();
                this.ComponenteNominaID.Value = Convert.ToString(dr["ComponenteNominaID"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasReconocidos"].ToString()))
                this.DiasReconocidos.Value = Convert.ToInt32(dr["DiasReconocidos"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noPrestacionesConvencion()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ConvencionNOID = new UDT_BasicID();
            this.ConvencionNODesc = new UDT_Descriptivo();
            this.ComponenteNominaID = new UDT_BasicID();
            this.ComponenteNominaDesc = new UDT_Descriptivo();
            this.DiasReconocidos = new UDTSQL_int();
        }

        public DTO_noPrestacionesConvencion(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noPrestacionesConvencion(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ConvencionNOID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConvencionNODesc { get; set; }
        
        [DataMember]
        public UDT_BasicID ComponenteNominaID { get; set; }
        
        [DataMember]
        public UDT_Descriptivo ComponenteNominaDesc { get; set; }

        [DataMember]
        public UDTSQL_int DiasReconocidos { get; set; }
    }
}
