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
    /// Models DTO_ccFasecoldaModelo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccFasecoldaModelo : DTO_MasterComplex
    {
        #region DTO_ccFasecoldaModelo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccFasecoldaModelo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.FasecoldaDesc.Value = Convert.ToString(dr["FasecoldaDesc"].ToString());
                }

                this.FasecoldaID.Value = Convert.ToString(dr["FasecoldaID"].ToString());
                this.Modelo.Value = Convert.ToString(dr["Modelo"].ToString());
                this.Valor.Value = Convert.ToDecimal(dr["Valor"].ToString());
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }
        
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccFasecoldaModelo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.FasecoldaID = new UDT_BasicID();
            this.FasecoldaDesc = new UDT_Descriptivo();
            this.Modelo = new UDT_CodigoGrl5();
            this.Valor = new UDT_Valor();
        }

        public DTO_ccFasecoldaModelo(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccFasecoldaModelo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID FasecoldaID { get; set; }

        [DataMember]
        public UDT_Descriptivo FasecoldaDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 Modelo { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

    }

}
