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
    public class DTO_glEmpresaModulo : DTO_MasterComplex
    {
        #region DTO_glEmpresaModulo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glEmpresaModulo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.EmpresaDesc.Value = dr["EmpresaDesc"].ToString();
                    this.ModuloDesc.Value = dr["ModuloDesc"].ToString();
                }

                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.ModuloID.Value = dr["ModuloID"].ToString();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glEmpresaModulo()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.EmpresaID = new UDT_BasicID();
            this.EmpresaDesc = new UDT_Descriptivo();
            this.ModuloID = new UDT_BasicID();
            this.ModuloDesc = new UDT_Descriptivo();
        }

        public DTO_glEmpresaModulo(DTO_MasterComplex comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_glEmpresaModulo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID EmpresaID { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpresaDesc { get; set; }

        [DataMember]
        public UDT_BasicID ModuloID { get; set; }

        [DataMember]
        public UDT_Descriptivo ModuloDesc { get; set; }
    }
}
