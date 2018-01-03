using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// 
    /// Models DTO_CapituloPlaneacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_CapituloPlaneacion
    {
        #region DTO_CapituloPlaneacion
        public DTO_CapituloPlaneacion(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ClaseServicioID.Value = Convert.ToString(dr["ClaseServicioID"]);
                this.CapituloGrupoID.Value = Convert.ToString(dr["CapituloGrupoID"]);
                this.CapituloGrupoDesc.Value = Convert.ToString(dr["CapituloDesc"]);
                this.SubCapituloID.Value = Convert.ToString(dr["SubCapituloID"]);
                this.SubCapituloDesc.Value = Convert.ToString(dr["SubCapituloDesc"]);                 
            }
            catch (Exception e)
            {                
                throw e;
            }
        }
       
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_CapituloPlaneacion()
        {
            InitCols();
        }

        public void InitCols() 
        {
            this.ClaseServicioID = new UDT_CodigoGrl();
            this.CapituloGrupoID = new UDT_CodigoGrl();
            this.CapituloGrupoDesc = new UDT_Descriptivo();
            this.SubCapituloID = new UDT_CodigoGrl();
            this.SubCapituloDesc = new UDT_Descriptivo();
            this.Detalle = new List<DTO_pyPreProyectoTarea>();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_CodigoGrl ClaseServicioID { get; set; }

        [DataMember]
        public UDT_CodigoGrl CapituloGrupoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CapituloGrupoDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl SubCapituloID { get; set; }

        [DataMember]
        public UDT_Descriptivo SubCapituloDesc { get; set; }

        [DataMember]
        public List<DTO_pyPreProyectoTarea> Detalle { get; set; }

        #endregion
    }
}
