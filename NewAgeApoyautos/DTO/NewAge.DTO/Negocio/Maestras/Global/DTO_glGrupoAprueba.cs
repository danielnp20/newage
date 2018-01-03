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
    /// Models DTO_glGrupoAprueba
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glGrupoAprueba : DTO_MasterBasic
    {
        #region DTO_glGrupoAprueba
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glGrupoAprueba(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                    this.DocumentoDesc.Value = dr["DocumentoDesc"].ToString();
                this.DocumentoID.Value = dr["DocumentoID"].ToString();
                this.ValorInd.Value = Convert.ToBoolean(dr["ValorInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glGrupoAprueba()
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
            this.ValorInd = new UDT_SiNo();
        }

        public DTO_glGrupoAprueba(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glGrupoAprueba(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID DocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumentoDesc { get; set; }

        [DataMember]
        public UDT_SiNo ValorInd { get; set; }
    }
}
