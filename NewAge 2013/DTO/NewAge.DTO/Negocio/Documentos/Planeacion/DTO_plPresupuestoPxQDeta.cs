using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_plPresupuestoPxQDeta
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plPresupuestoPxQDeta()
        {
            this.InitCols();
        }

        public DTO_plPresupuestoPxQDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.RecursoDesc.Value = dr["RecursoDesc"].ToString(); /*Descripcion*/

                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrEmpty(dr["Ano"].ToString()))
                    this.Ano.Value = Convert.ToInt32(dr["Ano"]);
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.RecursoID.Value = Convert.ToString(dr["RecursoID"]);
                this.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                if (!string.IsNullOrEmpty(dr["CantidadPRELoc"].ToString()))
                    this.CantidadPRELoc.Value = Convert.ToDecimal(dr["CantidadPRELoc"]);
                if (!string.IsNullOrEmpty(dr["CantidadPREExt"].ToString()))
                    this.CantidadPREExt.Value = Convert.ToDecimal(dr["CantidadPREExt"]);
                if (!string.IsNullOrEmpty(dr["ValorUniOCLoc"].ToString()))
                    this.ValorUniOCLoc.Value = Convert.ToDecimal(dr["ValorUniOCLoc"]);
                if (!string.IsNullOrEmpty(dr["ValorUniOCExt"].ToString()))
                    this.ValorUniOCExt.Value = Convert.ToDecimal(dr["ValorUniOCExt"]);
                if (!string.IsNullOrEmpty(dr["PorcentajeVar"].ToString()))
                    this.PorcentajeVar.Value = Convert.ToDecimal(dr["PorcentajeVar"]);
                if (!string.IsNullOrEmpty(dr["NumeroDocOC"].ToString()))
                    this.NumeroDocOC.Value = Convert.ToInt32(dr["NumeroDocOC"]);
                this.ValorUniLoc.Value = Convert.ToDecimal(dr["ValorUniLoc"]);
                this.ValorUniExt .Value = Convert.ToDecimal(dr["ValorUniExt"]);
                if (!string.IsNullOrEmpty(dr["CantidadSOL"].ToString()))
                    this.CantidadSOL .Value = Convert.ToDecimal(dr["CantidadSOL"]);
                if (!string.IsNullOrEmpty(dr["CantidadPEN"].ToString()))
                    this.CantidadPEN .Value = Convert.ToDecimal(dr["CantidadPEN"]);
                this.DescripTExt.Value = dr["DescripTExt"].ToString();
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);

                //Columnas extras
                try { this.ContratoID.Value = dr["ContratoID"].ToString(); }    catch (Exception) { ; }
                try { this.ActividadID.Value = dr["ActividadID"].ToString(); }   catch (Exception) { ; }
                try { this.LocFisicaID.Value = dr["LocFisicaID"].ToString(); }   catch (Exception) { ; }               
                this.PresupuestoLoc.Value = 0;
                this.PresupuestoExt.Value = 0;
                this.NuevaCantidadPRELoc.Value = 0;
                this.NuevaCantidadPREExt.Value = 0;
                this.NuevoPresupuestoLoc.Value = 0;
                this.NuevoPresupuestoExt.Value = 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_plPresupuestoPxQDeta(bool NewDeta)
        {
            InitCols();
            try
            {              
                this.ValorUniOCLoc.Value = 0;
                this.ValorUniOCExt.Value = 0;
                this.PorcentajeVar.Value = 0;
                this.ValorUniLoc.Value = 0;
                this.ValorUniExt.Value = 0;
                this.CantidadPRELoc.Value = 0;
                this.CantidadPREExt.Value = 0;
                this.NuevaCantidadPRELoc.Value = 0;
                this.NuevaCantidadPREExt.Value = 0;
                this.CantidadSOL.Value = 0;
                this.CantidadPEN.Value = 0;
                this.PresupuestoLoc.Value = 0;
                this.PresupuestoExt.Value = 0;
                this.NuevoPresupuestoLoc.Value = 0;
                this.NuevoPresupuestoExt.Value = 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.Ano = new UDTSQL_int();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.UnidadInvID = new UDT_UnidadInvID();          
            this.ValorUniOCLoc = new UDT_Valor();
            this.ValorUniOCExt = new UDT_Valor();
            this.PorcentajeVar = new UDT_PorcentajeID();
            this.NumeroDocOC = new UDT_Consecutivo();
            this.ValorUniLoc = new UDT_Valor();
            this.ValorUniExt = new UDT_Valor();
            this.CantidadPRELoc = new UDT_Cantidad();
            this.CantidadPREExt = new UDT_Cantidad();           
            this.CantidadSOL = new UDT_Cantidad();
            this.CantidadPEN = new UDT_Cantidad();
            this.DescripTExt = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();

            #region Variables Adicionales
            this.RecursoID = new UDT_CodigoGrl();
            this.RecursoDesc = new UDT_Descriptivo();
            this.AreaFisicaID = new UDT_AreaFisicaID();
            this.AreaFisicaDesc = new UDT_Descriptivo();
            this.CodigoBSDesc = new UDT_Descriptivo();
            this.LineaPresDesc = new UDT_Descriptivo();
            this.ActividadID = new UDT_ActividadID();
            this.LocFisicaID = new UDT_LocFisicaID();
            this.ContratoID = new UDT_ContratoID();
            this.PrefijoIDOC = new UDT_PrefijoID();
            this.NroOC = new UDT_Consecutivo();
            this.NuevaCantidadPRELoc = new UDT_Cantidad();
            this.NuevaCantidadPREExt = new UDT_Cantidad();
            this.PresupuestoLoc = new UDT_Valor();
            this.PresupuestoExt = new UDT_Valor();
            this.NuevoPresupuestoLoc = new UDT_Valor();
            this.NuevoPresupuestoExt = new UDT_Valor();
            this.Detalle = new List<DTO_plPresupuestoPxQDeta>(); 
            #endregion
        }

        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_int Ano { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_RecursoID Recurso { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor ValorUniOCLoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor ValorUniOCExt { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_PorcentajeID PorcentajeVar { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDocOC { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor ValorUniLoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor ValorUniExt { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad CantidadPRELoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad CantidadPREExt { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad CantidadSOL { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad CantidadPEN { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTExt DescripTExt  { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_Consecutivo Consecutivo { get; set; }
  
        #region  Variables adicionales

        [DataMember]
        [NotImportable]
        public UDT_CodigoGrl RecursoID { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_Descriptivo RecursoDesc { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_AreaFisicaID AreaFisicaID { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_Descriptivo AreaFisicaDesc { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_Descriptivo CodigoBSDesc { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_Descriptivo LineaPresDesc { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_ActividadID ActividadID { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_LocFisicaID LocFisicaID { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_ContratoID ContratoID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor PresupuestoLoc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor PresupuestoExt { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad NuevaCantidadPRELoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad NuevaCantidadPREExt { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor NuevoPresupuestoLoc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor NuevoPresupuestoExt { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_PrefijoID PrefijoIDOC { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_Consecutivo NroOC { get; set; }

        [DataMember]
        [NotImportable]
        public List<DTO_plPresupuestoPxQDeta> Detalle { get; set; } 
        #endregion

        #endregion
    }
        
}
