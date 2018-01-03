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
    /// Models glTareaAreaFuncional
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glLLamadaPregunta : DTO_MasterComplex
    {
        #region DTO_glLLamadaPregunta
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glLLamadaPregunta(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                    this.LLamadaDesc.Value = dr["LLamadaDesc"].ToString();

                this.LLamadaID.Value = dr["LLamadaID"].ToString();
                this.TipoReferencia.Value = Convert.ToByte(dr["TipoReferencia"]);
                this.CodPregunta.Value = dr["CodPregunta"].ToString();
                this.Pregunta.Value = dr["Pregunta"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glLLamadaPregunta()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.LLamadaID = new UDT_BasicID();
            this.LLamadaDesc = new UDT_Descriptivo();
            this.TipoReferencia = new UDTSQL_tinyint();
            this.CodPregunta = new UDTSQL_char(10);
            this.Pregunta = new UDT_DescripTBase();
        }

        public DTO_glLLamadaPregunta(DTO_MasterComplex comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_glLLamadaPregunta(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID LLamadaID { get; set; }

        [DataMember]
        public UDT_Descriptivo LLamadaDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoReferencia { get; set; }

        [DataMember]
        public UDTSQL_char CodPregunta { get; set; }

        [DataMember]
        public UDT_DescripTBase Pregunta { get; set; }
    }
}
