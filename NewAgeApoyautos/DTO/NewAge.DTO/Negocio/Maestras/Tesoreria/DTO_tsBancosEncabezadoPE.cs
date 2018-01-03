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
    /// Models DTO_tsBancosEncabezadoPE
    /// </summary>  
    [DataContract]
    [Serializable]
    public class DTO_tsBancosEncabezadoPE : DTO_MasterComplex
    {
        #region DTO_tsBancosEncabezadoPE
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_tsBancosEncabezadoPE(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.BancoCuentaDesc.Value = dr["BancoCuentaDesc"].ToString();
                }

                this.BancoCuentaID.Value = dr["BancoCuentaID"].ToString();
                this.Posicion.Value = Convert.ToByte(dr["Posicion"]);
                this.Decripcion.Value = dr["Decripcion"].ToString();
                this.Longitud.Value = Convert.ToByte(dr["Longitud"]);
                this.TipoDato.Value = Convert.ToByte(dr["TipoDato"]);
                this.Dato.Value = dr["Dato"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_tsBancosEncabezadoPE()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.BancoCuentaID = new UDT_BasicID();
            this.BancoCuentaDesc = new UDT_Descriptivo();
            this.Posicion = new UDTSQL_tinyint();
            this.Decripcion = new UDT_DescripTBase();
            this.Longitud = new UDTSQL_tinyint();
            this.TipoDato = new UDTSQL_tinyint();
            this.Dato = new UDTSQL_char(20);
        }

        public DTO_tsBancosEncabezadoPE(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_tsBancosEncabezadoPE(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID BancoCuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo BancoCuentaDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint Posicion { get; set; }

        [DataMember]
        public UDT_DescripTBase Decripcion { get; set; }

        [DataMember]
        public UDTSQL_tinyint Longitud { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoDato { get; set; }

        [DataMember]
        public UDTSQL_char Dato { get; set; }

    }
}
