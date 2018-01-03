using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.GlobalConfig;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_seMaquina
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_seMaquina : DTO_MasterBasic
    {
        #region DTO_seMaquina
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_seMaquina(IDataReader dr, DTO_aplMaestraPropiedades mp)
            : base(dr, mp)
        {
            InitCols();

            this.Tipo.Value = Convert.ToByte(dr["Tipo"]);
            this.webConfig.Value = (dr["webConfig"]).ToString();
            this.SQLInstancia.Value = (dr["SQLInstancia"]).ToString();
            this.AplTrayectoria.Value = (dr["AplTrayectoria"]).ToString();
            this.CopiaTrayectoria.Value = (dr["CopiaTrayectoria"].ToString());
            
          
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_seMaquina()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Tipo = new UDTSQL_tinyint();
            this.webConfig = new UDTSQL_varchar(2024);
            this.SQLInstancia = new UDT_DescripTBase();
            this.AplTrayectoria = new UDT_DescripTBase();
            this.CopiaTrayectoria = new UDT_DescripTBase();
        }

        public DTO_seMaquina(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_seMaquina(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_tinyint Tipo { get; set; }

        [DataMember]
        public UDTSQL_varchar webConfig { get; set; }

        [DataMember]
        public UDT_DescripTBase SQLInstancia { get; set; }

        [DataMember]
        public UDT_DescripTBase AplTrayectoria { get; set; }

        [DataMember]
        public UDT_DescripTBase CopiaTrayectoria { get; set; }

    }

}