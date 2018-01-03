using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_inBodega
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inBodega : DTO_MasterBasic
    {
        #region DTO_inBodega

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inBodega(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.inBodegaContabDesc.Value = dr["inBodegaContabDesc"].ToString();
                    this.AreaFuncionalDesc.Value = dr["AreaFuncionalDesc"].ToString();
                    this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                    this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
                    this.BodegaTipoDesc.Value = dr["BodegaTipoDesc"].ToString();
                    this.CosteoGrupoInvDesc.Value = dr["CosteoGrupoInvDesc"].ToString();
                    this.PrefijoDesc.Value = dr["PrefijoDesc"].ToString();
                    this.ResponsableDesc.Value = dr["ResponsableDesc"].ToString();
                    this.LocFisicaDesc.Value = dr["LocFisicaDesc"].ToString();
                    this.LineaPresupuestoDesc.Value = dr["LineaPresupuestoDesc"].ToString();
                }
                this.inBodegaContabID.Value = dr["inBodegaContabID"].ToString();
                this.AreaFuncionalID.Value = dr["AreaFuncionalID"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.BodegaTipoID.Value = dr["BodegaTipoID"].ToString();
                this.CosteoGrupoInvID.Value = dr["CosteoGrupoInvID"].ToString();
                this.inBodegaContabID.Value = dr["inBodegaContabID"].ToString();
                this.PrefijoID.Value = dr["PrefijoID"].ToString();
                this.Responsable.Value = dr["Responsable"].ToString();
                if (!string.IsNullOrEmpty(dr["InvFisicoInd"].ToString()))
                    this.InvFisicoInd.Value = Convert.ToBoolean(dr["InvFisicoInd"]);
                if (!string.IsNullOrEmpty(dr["NumeroDocINI"].ToString()))
                    this.NumeroDocINI.Value = Convert.ToInt32(dr["NumeroDocINI"]);
                if (!string.IsNullOrEmpty(dr["NumeroDocFIN"].ToString()))
                    this.NumeroDocFIN.Value = Convert.ToInt32(dr["NumeroDocFIN"]);
                if (!string.IsNullOrEmpty(dr["OrdenSalidaInd"].ToString()))
                    this.OrdenSalidaInd.Value = Convert.ToBoolean(dr["OrdenSalidaInd"]);
                if (!string.IsNullOrEmpty(dr["ControlaProyectoInd"].ToString()))
                    this.ControlaProyectoInd.Value = Convert.ToBoolean(dr["ControlaProyectoInd"]);
                this.LocFisicaID.Value = dr["LocFisicaID"].ToString();
                if (!string.IsNullOrEmpty(dr["LineaPresupuestoID"].ToString()))
                    this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.ConsultaCostoInd.Value = false;
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inBodega()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.BodegaTipoID = new UDT_BasicID();
            this.BodegaTipoDesc = new UDT_Descriptivo();
            this.CosteoGrupoInvID = new UDT_BasicID();
            this.CosteoGrupoInvDesc = new UDT_Descriptivo();
            this.inBodegaContabID = new UDT_BasicID();
            this.inBodegaContabDesc = new UDT_Descriptivo();
            this.AreaFuncionalID = new UDT_BasicID();
            this.AreaFuncionalDesc = new UDT_Descriptivo();
            this.PrefijoID = new UDT_BasicID();
            this.PrefijoDesc = new UDT_Descriptivo();
            this.Responsable = new UDT_BasicID();
            this.ResponsableDesc = new UDT_Descriptivo();
            this.ProyectoID = new UDT_BasicID();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.CentroCostoID = new UDT_BasicID();
            this.CentroCostoDesc = new UDT_Descriptivo();
            this.InvFisicoInd = new UDT_SiNo();
            this.NumeroDocINI = new UDT_Consecutivo();
            this.NumeroDocFIN = new UDT_Consecutivo();
            this.OrdenSalidaInd = new UDT_SiNo();
            this.ControlaProyectoInd = new UDT_SiNo();
            this.LocFisicaID = new UDT_BasicID();
            this.LocFisicaDesc = new UDT_Descriptivo();
            this.LineaPresupuestoID = new UDT_BasicID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
            //Campos adicionales
            this.ConsultaCostoInd = new UDT_SiNo();
            this.DetalleReferencias = new List<DTO_inReferencia>();
            this.ConsultaCostoInd.Value = false;
        }

        public DTO_inBodega(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();           
        }

        public DTO_inBodega(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID BodegaTipoID { get; set; }

        [DataMember]
        public UDT_Descriptivo BodegaTipoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CosteoGrupoInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo CosteoGrupoInvDesc { get; set; }

        [DataMember]
        public UDT_BasicID inBodegaContabID { get; set; }

        [DataMember]
        public UDT_Descriptivo inBodegaContabDesc { get; set; }

        [DataMember]
        public UDT_BasicID AreaFuncionalID { get; set; }

        [DataMember]
        public UDT_Descriptivo AreaFuncionalDesc { get; set; }

        [DataMember]
        public UDT_BasicID PrefijoID { get; set; }

        [DataMember]
        public UDT_Descriptivo PrefijoDesc { get; set; }

        [DataMember]
        public UDT_BasicID Responsable { get; set; }

        [DataMember]
        public UDT_Descriptivo ResponsableDesc { get; set; }

        [DataMember]
        public UDT_BasicID ProyectoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CentroCostoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; }

        [DataMember]
        public UDT_SiNo InvFisicoInd { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocINI { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocFIN { get; set; }

        [DataMember]
        public UDT_SiNo OrdenSalidaInd { get; set; }

        [DataMember]
        public UDT_SiNo ControlaProyectoInd { get; set; }

        [DataMember]
        public UDT_BasicID LocFisicaID { get; set; }

        [DataMember]
        public UDT_Descriptivo LocFisicaDesc { get; set; }

        [DataMember]
        public UDT_BasicID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; }

        //Campos adicionales
        [DataMember]
        public UDT_SiNo ConsultaCostoInd { get; set; }

        [DataMember]
        public List<DTO_inReferencia> DetalleReferencias { get; set; }

    }

}
