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
    /// Models DTO_glNivelesAprobacionDoc
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glNivelesAprobacionDoc : DTO_MasterComplex
    {
        #region DTO_glNivelesAprobacionDoc
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glNivelesAprobacionDoc(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.DocumentoDesc.Value = dr["DocumentoDesc"].ToString();
                    this.GrupoApruebaDocDesc.Value = dr["GrupoApruebaDocDesc"].ToString();
                    this.SeccionFuncionalDesc.Value = dr["SeccionFuncionalDesc"].ToString();
                    this.ActividadFlujoDesc.Value = Convert.ToString(dr["ActividadFlujoDesc"]);
                }
                this.DocumentoID.Value=dr["DocumentoID"].ToString();
                this.GrupoApruebaDocID.Value=dr["GrupoApruebaDocID"].ToString();
                this.TipoEspecial.Value=Convert.ToByte(dr["TipoEspecial"]);
                this.Valor.Value=Convert.ToDecimal(dr["Valor"]);
                this.Orden.Value = Convert.ToByte(dr["Orden"]);
                this.ActividadFlujoID.Value = Convert.ToString(dr["ActividadFlujoID"]);
                this.NivelAprobacion.Value=Convert.ToByte(dr["NivelAprobacion"]);
                this.SeccionFuncionalID.Value = dr["SeccionFuncionalID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glNivelesAprobacionDoc()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
           this.DocumentoID =new UDT_BasicID();
           this.DocumentoDesc = new UDT_Descriptivo();
           this.GrupoApruebaDocID=new UDT_BasicID();
           this.GrupoApruebaDocDesc = new UDT_Descriptivo();
           this.TipoEspecial=new UDTSQL_tinyint();
           this.Valor=new UDT_Valor();
           this.Orden=new UDTSQL_smallint();
           this.ActividadFlujoID = new UDT_BasicID();
           this.ActividadFlujoDesc = new UDT_Descriptivo();
           this.NivelAprobacion=new UDTSQL_tinyint();
           this.SeccionFuncionalID = new UDT_BasicID();
           this.SeccionFuncionalDesc = new UDT_Descriptivo();
        }

        public DTO_glNivelesAprobacionDoc(DTO_MasterComplex comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_glNivelesAprobacionDoc(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID DocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumentoDesc { get; set; }

        [DataMember]
        public UDT_BasicID GrupoApruebaDocID { get; set; }

        [DataMember]
        public UDT_Descriptivo GrupoApruebaDocDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoEspecial { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDTSQL_smallint Orden { get; set; }

        [DataMember]
        public UDT_BasicID ActividadFlujoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadFlujoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint NivelAprobacion { get; set; }

        [DataMember]
        public UDT_BasicID SeccionFuncionalID { get; set; }

        [DataMember]
        public UDT_Descriptivo SeccionFuncionalDesc { get; set; }
    }
}
