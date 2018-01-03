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
    /// Models DTO_glSeccionFuncional.cs
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glSeccionFuncional : DTO_MasterBasic
    {
        #region DTO_glSeccionFuncional
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glSeccionFuncional(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.AreaFuncionalDesc.Value = dr["AreaFuncionalDesc"].ToString();
                    this.UsuarioDesc.Value = dr["UsuarioDesc"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["UsuarioID1"].ToString()))
                        this.DirectorSeccion.Value = dr["UsuarioID1"].ToString();
                }
                else
                    this.DirectorSeccion.Value = dr["DirectorSeccion"].ToString();

                this.AreaFuncionalID.Value = dr["AreaFuncionalID"].ToString();
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glSeccionFuncional() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.AreaFuncionalID = new UDT_BasicID();
            this.AreaFuncionalDesc = new UDT_Descriptivo();
            this.DirectorSeccion = new UDT_BasicID();
            this.UsuarioDesc = new UDT_Descriptivo();
         }

        public DTO_glSeccionFuncional(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glSeccionFuncional(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
     
        [DataMember]
        public UDT_BasicID AreaFuncionalID { get; set; }

        [DataMember]
        public UDT_Descriptivo AreaFuncionalDesc { get; set; }

        [DataMember]
        public UDT_BasicID DirectorSeccion { get; set; }

        [DataMember]
        public UDT_Descriptivo UsuarioDesc { get; set; }
        
    }

}
