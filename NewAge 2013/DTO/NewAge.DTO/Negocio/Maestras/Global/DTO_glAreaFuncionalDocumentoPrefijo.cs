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
    /// Models DTO_glAreaFuncionalDocumentoPrefijo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glAreaFuncionalDocumentoPrefijo : DTO_MasterComplex
    {
        #region glAreaFuncionalDocumentoPrefijo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glAreaFuncionalDocumentoPrefijo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.DocumentoDesc.Value = dr["DocumentoDesc"].ToString();
                    this.PrefijoDesc.Value = dr["PrefijoDesc"].ToString();
                    this.AreaFuncionalDesc.Value = dr["AreaFuncionalDesc"].ToString();
                }
              
                this.AreaFuncionalID.Value = dr["AreaFuncionalID"].ToString();
                this.DocumentoID.Value = dr["DocumentoID"].ToString();
                this.PrefijoID.Value = dr["PrefijoID"].ToString();               
            }
            catch(Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glAreaFuncionalDocumentoPrefijo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.AreaFuncionalID = new UDT_BasicID();
            this.AreaFuncionalDesc = new UDT_Descriptivo();
            this.DocumentoID = new UDT_BasicID(true);
            this.DocumentoDesc = new UDT_Descriptivo();
            this.PrefijoID = new UDT_BasicID();
            this.PrefijoDesc = new UDT_Descriptivo();
           
        }

        public DTO_glAreaFuncionalDocumentoPrefijo(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_glAreaFuncionalDocumentoPrefijo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID AreaFuncionalID { get; set; }

        [DataMember]
        public UDT_Descriptivo AreaFuncionalDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID DocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumentoDesc { get; set; }

        [DataMember]
        public UDT_BasicID PrefijoID { get; set; }

        [DataMember]
        public UDT_Descriptivo PrefijoDesc { get; set; }

       
        
    }
}
