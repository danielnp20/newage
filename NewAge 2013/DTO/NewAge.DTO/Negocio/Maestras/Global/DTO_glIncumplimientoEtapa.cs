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
    /// Models DTO_glIncumplimientoEtapa
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glIncumplimientoEtapa : DTO_MasterBasic
    {
        #region glIncumplimientoEtapa
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glIncumplimientoEtapa(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.DocumentoDesc.Value = dr["DocumentoDesc"].ToString();
                    this.AbogadoDesc.Value = dr["AbogadoDesc"].ToString();
                }

                this.DocumentoID.Value = dr["DocumentoID"].ToString();
                this.NivelRiesgo.Value = Convert.ToByte(dr["NivelRiesgo"]);

                if (!string.IsNullOrWhiteSpace(dr["EstadoDeuda"].ToString()))
                    this.EstadoDeuda.Value = Convert.ToByte(dr["EstadoDeuda"]);
                this.Abogado.Value = dr["Abogado"].ToString();

                if (!string.IsNullOrWhiteSpace(dr["DiasInicio"].ToString()))
                    this.DiasInicio.Value = Convert.ToInt32(dr["DiasInicio"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glIncumplimientoEtapa()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.DocumentoID = new UDT_BasicID();
            this.DocumentoDesc = new UDT_Descriptivo();
            this.NivelRiesgo = new UDTSQL_tinyint();
            this.EstadoDeuda = new UDTSQL_tinyint();
            this.Abogado = new UDT_BasicID();
            this.AbogadoDesc = new UDT_Descriptivo();
            this.DiasInicio = new UDTSQL_int();
            //Adicional
            this.DiasInicioInferior = new UDTSQL_int();
        }

        public DTO_glIncumplimientoEtapa(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glIncumplimientoEtapa(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }

        #endregion

        [DataMember]
        public UDT_BasicID DocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumentoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint NivelRiesgo { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoDeuda { get; set; }

        [DataMember]
        public UDT_BasicID Abogado { get; set; }

        [DataMember]
        public UDT_Descriptivo AbogadoDesc { get; set; }

        [DataMember]
        public UDTSQL_int DiasInicio { get; set; }

        //Adicional
        [DataMember]
        public UDTSQL_int DiasInicioInferior { get; set; }
    }
}
