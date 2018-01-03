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
    public class DTO_glLLamadaCodigo : DTO_MasterBasic
    {
        #region DTO_glLLamadaCodigo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glLLamadaCodigo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["NoEfectivaInd"].ToString()))
                    this.NoEfectivaInd.Value = Convert.ToBoolean(dr["NoEfectivaInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glLLamadaCodigo()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.NoEfectivaInd = new UDT_SiNo();
        }

        public DTO_glLLamadaCodigo(DTO_MasterBasic comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_glLLamadaCodigo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_SiNo NoEfectivaInd { get; set; }
    }
}
