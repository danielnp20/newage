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
    /// Models DTO_glActividadListaChequeo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glActividadListaChequeo : DTO_MasterComplex
    {
        #region glAreaFuncionalDocumentoPrefijo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glActividadListaChequeo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ActividadFlujoDesc.Value = dr["ActividadFlujoDesc"].ToString();
                }
                this.ActividadFlujoID.Value = dr["ActividadFlujoID"].ToString();
                this.Descripcion.Value = dr["Descripcion"].ToString();
                this.ObligatorioInd.Value = Convert.ToBoolean(dr["ObligatorioInd"]);
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = Convert.ToString(dr["Observacion"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glActividadListaChequeo()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.ActividadFlujoID = new UDT_BasicID();
            this.ActividadFlujoDesc = new UDT_Descriptivo();
            this.Descripcion = new UDTSQL_char(50);
            this.ObligatorioInd = new UDT_SiNo();
            this.Observacion = new UDT_DescripTExt();
        }

        public DTO_glActividadListaChequeo(DTO_MasterComplex comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_glActividadListaChequeo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ActividadFlujoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadFlujoDesc { get; set; }

        [DataMember]
        public UDTSQL_char Descripcion { get; set; }

        [DataMember]
        public UDT_SiNo ObligatorioInd { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }
    }
}
