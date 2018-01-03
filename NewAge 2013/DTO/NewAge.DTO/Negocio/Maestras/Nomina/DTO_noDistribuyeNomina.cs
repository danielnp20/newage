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
    /// Models DTO_noDistribuyeNomina
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noDistribuyeNomina : DTO_MasterComplex 
    {
        #region DTO_noDistribuyeNomina
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noDistribuyeNomina(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.ProyectoOrigenDesc.Value = dr["ProyectoOrigenDesc"].ToString(); 
                    this.ProyectoDestinoDesc.Value = dr["ProyectoDestinoDesc"].ToString(); 
                }
                this.ProyectoOrigen.Value = dr["ProyectoOrigen"].ToString();
                this.ProyectoDestino.Value = dr["ProyectoDestino"].ToString();
                this.PorcentajeID.Value = Convert.ToDecimal(dr["PorcentajeID"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noDistribuyeNomina() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ProyectoOrigen = new UDT_BasicID();
            this.ProyectoOrigenDesc = new UDT_Descriptivo();
            this.ProyectoDestino = new UDT_BasicID();
            this.ProyectoDestinoDesc = new UDT_Descriptivo();
            this.PorcentajeID = new UDT_PorcentajeID();           
        }

        public DTO_noDistribuyeNomina(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noDistribuyeNomina(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
        
        [DataMember]
        public UDT_BasicID ProyectoOrigen { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoOrigenDesc { get; set; }

        [DataMember]
        public UDT_BasicID ProyectoDestino { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDestinoDesc { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorcentajeID { get; set; }
    }

}

