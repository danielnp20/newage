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
    /// Models DTO_prBienServicio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prBienServicio : DTO_MasterHierarchyBasic
    {
        #region DTO_prBienServicio
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prBienServicio(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ClaseBSDesc.Value = dr["ClaseBSDesc"].ToString();
                    this.UnidadInvDesc.Value = dr["UnidadInvDesc"].ToString();
                    this.CBS_UtiliContrConstrDesc.Value = dr["CBS_UtiliContrConstrDesc"].ToString();
                    this.CBS_ImprevContrConstrDesc.Value = dr["CBS_ImprevContrConstrDesc"].ToString();
                    this.CBS_AdminContrConstrDesc.Value = dr["CBS_AdminContrConstrDesc"].ToString();
                    this.CBS_AdminGtosReemDesc.Value = dr["CBS_AdminGtosReemDesc"].ToString();
                    this.MonCompraDesc.Value = dr["MonCompraDesc"].ToString();
                }                

                this.ClaseBSID.Value = dr["ClaseBSID"].ToString();
                this.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                this.CBS_AdminGtosReem.Value = dr["CBS_AdminGtosReem"].ToString();
                this.CBS_AdminContrConstr.Value = dr["CBS_AdminContrConstr"].ToString();
                this.CBS_ImprevContrConstr.Value = dr["CBS_ImprevContrConstr"].ToString();
                this.CBS_UtiliContrConstr.Value = dr["CBS_UtiliContrConstr"].ToString();               
                if (!string.IsNullOrWhiteSpace(dr["TipoControl"].ToString()))
                    this.TipoControl.Value = Convert.ToByte(dr["TipoControl"]);
                if (!string.IsNullOrWhiteSpace(dr["DocCompra"].ToString()))
                    this.DocCompra.Value = Convert.ToInt32(dr["DocCompra"]);
                this.MonCompra.Value = dr["MonCompra"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["VlrCompra"].ToString()))
                    this.VlrCompra.Value = Convert.ToDecimal(dr["VlrCompra"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prBienServicio() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ClaseBSID = new UDT_BasicID();
            this.ClaseBSDesc = new UDT_Descriptivo();
            this.UnidadInvID = new UDT_BasicID();
            this.UnidadInvDesc = new UDT_Descriptivo();
            this.CBS_AdminGtosReem = new UDT_BasicID();
            this.CBS_AdminGtosReemDesc = new UDT_Descriptivo();
            this.CBS_AdminContrConstr = new UDT_BasicID();
            this.CBS_AdminContrConstrDesc = new UDT_Descriptivo();
            this.CBS_ImprevContrConstr = new UDT_BasicID();
            this.CBS_ImprevContrConstrDesc = new UDT_Descriptivo();
            this.CBS_UtiliContrConstr = new UDT_BasicID();
            this.CBS_UtiliContrConstrDesc = new UDT_Descriptivo();
            this.TipoControl = new UDTSQL_tinyint();
            this.DocCompra = new UDT_Consecutivo();
            this.MonCompra = new UDT_BasicID();
            this.MonCompraDesc = new UDT_Descriptivo();
            this.VlrCompra = new UDT_Valor();
        }

        public DTO_prBienServicio(DTO_MasterHierarchyBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_prBienServicio(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  

        #endregion 

        [DataMember]
        public UDT_BasicID ClaseBSID { get; set; }

        [DataMember]
        public UDT_Descriptivo ClaseBSDesc { get; set; }

        [DataMember]
        public UDT_BasicID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo UnidadInvDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoControl { get; set; }

        [DataMember]
        public UDT_BasicID CBS_AdminGtosReem { get; set; }

        [DataMember]
        public UDT_Descriptivo CBS_AdminGtosReemDesc { get; set; }

        [DataMember]
        public UDT_BasicID CBS_AdminContrConstr { get; set; }

        [DataMember]
        public UDT_Descriptivo CBS_AdminContrConstrDesc { get; set; }

        [DataMember]
        public UDT_BasicID CBS_UtiliContrConstr { get; set; }

        [DataMember]
        public UDT_Descriptivo CBS_UtiliContrConstrDesc { get; set; }

        [DataMember]
        public UDT_BasicID CBS_ImprevContrConstr { get; set; }

        [DataMember]
        public UDT_Descriptivo CBS_ImprevContrConstrDesc { get; set; }

        [DataMember]
        public UDT_Consecutivo DocCompra { get; set; }

        [DataMember]
        public UDT_BasicID MonCompra { get; set; }

        [DataMember]
        public UDT_Descriptivo MonCompraDesc { get; set; }

        [DataMember]
        public UDT_Valor VlrCompra { get; set; }

    }
}
