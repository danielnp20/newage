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
    /// Models DTO_coCargoCosto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coCargoCosto : DTO_MasterComplex
    {
        #region DTO_coCargoCosto
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coCargoCosto(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.OperacionDesc.Value = dr["OperacionDesc"].ToString();
                    this.ConceptoCargoDesc.Value = dr["ConceptoCargoDesc"].ToString();
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                    this.CuentaIFRSDesc.Value = dr["CuentaIFRSDesc"].ToString();
                    this.LineaPresupuestoDesc.Value = dr["LineaPresupuestoDesc"].ToString();
                }

                this.OperacionID.Value = dr["OperacionID"].ToString();
                this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                if (!string.IsNullOrEmpty(dr["CuentaIFRS"].ToString()))
                    this.CuentaIFRS.Value = dr["CuentaIFRS"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
            }
            catch (Exception e)
            {
              throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coCargoCosto() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.OperacionID = new UDT_BasicID();
            this.OperacionDesc = new UDT_Descriptivo();
            this.ConceptoCargoID = new UDT_BasicID();
            this.ConceptoCargoDesc = new UDT_Descriptivo();
            this.CuentaID = new UDT_BasicID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.CuentaIFRS = new UDT_BasicID();
            this.CuentaIFRSDesc = new UDT_Descriptivo();
            this.LineaPresupuestoID = new UDT_BasicID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
        }

        public DTO_coCargoCosto(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coCargoCosto(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID OperacionID { get; set; }

        [DataMember]
        public UDT_Descriptivo OperacionDesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCargoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_BasicID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; }


        [DataMember]
        public UDT_BasicID CuentaIFRS { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaIFRSDesc { get; set; }
    }

}
