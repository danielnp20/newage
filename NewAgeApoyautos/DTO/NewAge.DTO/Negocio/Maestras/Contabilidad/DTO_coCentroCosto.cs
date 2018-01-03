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
    /// Models DTO_coCentroCosto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coCentroCosto : DTO_MasterHierarchyBasic
    {
        #region DTO_coCentroCosto
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coCentroCosto(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.OperacionDesc.Value = dr["OperacionDesc"].ToString();
                    this.CtoCapitalTrabajoDesc.Value = dr["CtoCapitalTrabajoDesc"].ToString();
                    this.UnidGenEfectDesc.Value = dr["UnidGenEfectDesc"].ToString();
                }

                this.OperacionID.Value = dr["OperacionID"].ToString();
                this.CtoCapitalTrabajo.Value = dr["CtoCapitalTrabajo"].ToString();
                if (!string.IsNullOrEmpty(dr["UnidGenEfectID"].ToString()))
                    this.UnidGenEfectID.Value = dr["UnidGenEfectID"].ToString();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coCentroCosto() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.OperacionID = new UDT_BasicID();
            this.OperacionDesc = new UDT_Descriptivo();
            this.CtoCapitalTrabajo = new UDT_BasicID();
            this.CtoCapitalTrabajoDesc = new UDT_Descriptivo();
            this.UnidGenEfectID = new UDT_BasicID();
            this.UnidGenEfectDesc = new UDT_Descriptivo();
        }

        public DTO_coCentroCosto(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coCentroCosto(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID OperacionID { get; set; }

        [DataMember]
        public UDT_Descriptivo OperacionDesc { get; set; }

        [DataMember]
        public UDT_BasicID CtoCapitalTrabajo  { get; set; }

        [DataMember]
        public UDT_Descriptivo CtoCapitalTrabajoDesc { get; set; }

        [DataMember]
        public UDT_BasicID UnidGenEfectID { get; set; }

        [DataMember]
        public UDT_Descriptivo UnidGenEfectDesc { get; set; }

    }

}
