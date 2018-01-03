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
    public class DTO_plPresupuestoPxQ
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plPresupuestoPxQ()
        {
            this.InitCols();
        }

        public DTO_plPresupuestoPxQ(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                if (!string.IsNullOrEmpty(dr["Ano"].ToString()))
                    this.Ano.Value = Convert.ToInt32(dr["Ano"]);
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                if (!string.IsNullOrEmpty(dr["CantidadPRELoc"].ToString()))
                    this.CantidadPRELoc.Value = Convert.ToDecimal(dr["CantidadPRELoc"]);
                if (!string.IsNullOrEmpty(dr["CantidadPREExt"].ToString()))
                    this.CantidadPREExt.Value = Convert.ToDecimal(dr["CantidadPREExt"]);
                this.ValorUniLoc.Value = Convert.ToDecimal(dr["ValorUniLoc"]);
                this.ValorUniExt .Value = Convert.ToDecimal(dr["ValorUniExt"]);
                if (!string.IsNullOrEmpty(dr["CantidadSOL"].ToString()))
                    this.CantidadSOL .Value = Convert.ToDecimal(dr["CantidadSOL"]);
                if (!string.IsNullOrEmpty(dr["CantidadPEN"].ToString()))
                    this.CantidadPEN .Value = Convert.ToDecimal(dr["CantidadPEN"]);
                if (!string.IsNullOrEmpty(dr["RecursoID"].ToString()))
                    this.RecursoID.Value = Convert.ToString(dr["RecursoID"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_plPresupuestoPxQ(bool NewDeta)
        {
            InitCols();
            try
            {
                this.CantidadPRELoc.Value = 0;
                this.CantidadPREExt.Value = 0;
                this.ValorUniLoc.Value = 0;
                this.ValorUniExt.Value = 0;
                this.CantidadSOL.Value = 0;
                this.CantidadPEN.Value = 0;
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
            this.EmpresaID = new UDT_EmpresaID();
            this.Ano = new UDTSQL_int();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.CantidadPRELoc = new UDT_Cantidad();
            this.CantidadPREExt = new UDT_Cantidad();
            this.ValorUniLoc = new UDT_Valor();
            this.ValorUniExt = new UDT_Valor();
            this.CantidadSOL = new UDT_Cantidad();
            this.CantidadPEN = new UDT_Cantidad();
            this.RecursoID = new UDT_CodigoGrl();
            this.Consecutivo = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

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
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad CantidadPRELoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad CantidadPREExt { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor ValorUniLoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor ValorUniExt { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad CantidadSOL { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad CantidadPEN { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CodigoGrl RecursoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo Consecutivo { get; set; }
        
        #endregion
    }
}
