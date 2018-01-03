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
    /// Models DTO_cpTarjetaCredito
    /// </summary>  
    [DataContract]
    [Serializable]
    public class DTO_cpTarjetaCredito : DTO_MasterBasic
    {
        #region DTO_cpTarjetaCredito
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_cpTarjetaCredito(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                    this.ResponsableDesc.Value = dr["ResponsableDesc"].ToString();
                }

                this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrEmpty(dr["TipoMoneda"].ToString()))
                    this.TipoMoneda.Value = Convert.ToByte(dr["TipoMoneda"]);
                if (!string.IsNullOrEmpty(dr["UsoTarjeta"].ToString()))
                    this.UsoTarjeta.Value = Convert.ToByte(dr["UsoTarjeta"]);
                this.Responsable.Value = dr["Responsable"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_cpTarjetaCredito() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroID = new UDT_BasicID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.TipoMoneda = new UDTSQL_tinyint();
            this.UsoTarjeta = new UDTSQL_tinyint();
            this.Responsable = new UDT_BasicID();
            this.ResponsableDesc = new UDT_Descriptivo();
        }

        public DTO_cpTarjetaCredito(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_cpTarjetaCredito(DTO_aplMaestraPropiedades masterProperties) 
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoMoneda { get; set; }

        [DataMember]
        public UDTSQL_tinyint UsoTarjeta { get; set; }

        [DataMember]
        public UDT_BasicID Responsable { get; set; }

        [DataMember]
        public UDT_Descriptivo ResponsableDesc { get; set; }


    }
}
