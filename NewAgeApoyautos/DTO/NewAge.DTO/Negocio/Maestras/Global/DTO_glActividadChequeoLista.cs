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
    /// Models DTO_glActividadChequeoLista  
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glActividadChequeoLista : DTO_MasterComplex
    {
        #region DTO_glActividadChequeoLista
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glActividadChequeoLista(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ActividadDesc.Value = dr["ActividadDesc"].ToString();
                    this.ActividadPreviaDesc.Value = dr["ActividadPreviaDesc"].ToString();
                }
                   
                this.ActividadFlujoID.Value = dr["ActividadFlujoID"].ToString();
                this.ActividadPreviaID.Value = dr["ActividadPreviaID"].ToString();
                this.ObligatorioInd.Value = Convert.ToBoolean(dr["ObligatorioInd"]);
                this.Observacion.Value = dr["Observacion"].ToString();
                this.Descripcion.Value = dr["Descripcion"].ToString();

            }
            catch (Exception e)
            {
                
                throw e;
            }
        }
        
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glActividadChequeoLista() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ActividadFlujoID = new UDT_BasicID();
            this.ActividadDesc = new UDT_Descriptivo();
            this.ObligatorioInd = new UDT_SiNo();
            this.Observacion = new UDT_DescripTExt();
            this.Descripcion = new UDTSQL_char(50);
            this.ActividadPreviaID = new UDT_BasicID();
            this.ActividadPreviaDesc = new UDT_Descriptivo();
        }

        public DTO_glActividadChequeoLista(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glActividadChequeoLista(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID ActividadFlujoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadDesc { get; set; }

        [DataMember]
        public UDT_SiNo ObligatorioInd{ get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDTSQL_char Descripcion { get; set; }

        [DataMember]
        public UDT_BasicID ActividadPreviaID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadPreviaDesc { get; set; }
    
    }

}
