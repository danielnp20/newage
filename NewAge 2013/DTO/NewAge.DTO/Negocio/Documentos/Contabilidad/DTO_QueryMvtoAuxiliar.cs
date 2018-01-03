using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;
using System.Reflection;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class:
    /// Models DTO_QueryMvtoAuxiliar
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryMvtoAuxiliar
    {
        #region DTO_QueryMvtoAuxiliar

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryMvtoAuxiliar(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteNro"].ToString()))
                    this.ComprobanteNro.Value = Convert.ToInt32(dr["ComprobanteNro"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.DocumentoTipo.Value = Convert.ToByte(dr["DocumentoTipo"]);
                if (!string.IsNullOrWhiteSpace(dr["DocumentoNro"].ToString()))
                    this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                this.DocumentoTercero.Value = dr["DocumentoTercero"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                this.LugarGeograficoID.Value = dr["LugarGeograficoID"].ToString();
                this.PrefijoCOM.Value = dr["PrefijoCOM"].ToString();
                this.DocumentoCOM.Value = dr["DocumentoCOM"].ToString();
                this.ActivoCOM.Value = dr["ActivoCOM"].ToString();
                this.ConceptoSaldoID.Value = dr["ConceptoSaldoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["IdentificadorTR"].ToString()))
                    this.IdentificadorTR.Value = Convert.ToInt64(dr["IdentificadorTR"]);
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.vlrMdaLoc.Value = Convert.ToDecimal(dr["vlrMdaLoc"]);
                if (!string.IsNullOrWhiteSpace(dr["vlrMdaExt"].ToString()))
                    this.vlrMdaExt.Value = Convert.ToDecimal(dr["vlrMdaExt"]);
                if (!string.IsNullOrWhiteSpace(dr["vlrMdaOtr"].ToString()))
                    this.vlrMdaOtr.Value = Convert.ToDecimal(dr["vlrMdaOtr"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                this.DocumentoDes.Value = dr["DocumentoDes"].ToString();
                this.CuentaDes.Value = dr["CuentaDes"].ToString();             
                this.TerceroDes.Value = dr["TerceroDes"].ToString();
                this.ProyectoDes.Value = dr["ProyectoDes"].ToString();
                this.CentroCtoDes.Value = dr["CentroCtoDes"].ToString();
                this.LineaPresDes.Value = dr["LineaPresDes"].ToString();
                this.ConceptoDes.Value = dr["ConceptoDes"].ToString();

                if (!string.IsNullOrWhiteSpace(dr["ComprobanteNro"].ToString()))
                    this.Comprobante = dr["ComprobanteID"].ToString().Trim() + " - " + dr["ComprobanteNro"].ToString().Trim();               
                this.PrefDoc = dr["DocumentoCOM"].ToString().Trim();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryMvtoAuxiliar()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.PeriodoID = new UDT_PeriodoID();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteNro = new UDT_Consecutivo();
            this.Fecha = new UDTSQL_smalldatetime();
            this.NumeroDoc = new UDT_Consecutivo();
            this.DocumentoID = new UDT_Consecutivo();
            this.DocumentoTipo = new UDTSQL_tinyint();
            this.DocumentoNro = new UDT_Consecutivo();
            this.DocumentoTercero = new UDTSQL_char(20);
            this.CuentaID = new UDT_CuentaID();
            this.TerceroID = new UDT_TerceroID();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.ConceptoCargoID = new UDT_ConceptoCargoID();
            this.LugarGeograficoID = new UDT_LugarGeograficoID();
            this.PrefijoCOM = new UDT_PrefijoID();
            this.DocumentoCOM = new UDTSQL_char(20);
            this.ActivoCOM = new UDT_PlaquetaID();
            this.ConceptoSaldoID = new UDT_ConceptoSaldoID();
            this.BalanceTipoID = new UDT_BalanceTipoID();
            this.IdentificadorTR = new UDT_IdentificadorTR();
            this.Descriptivo = new UDT_DescripTBase();
            this.vlrMdaLoc = new UDT_Valor();
            this.vlrMdaExt = new UDT_Valor();
            this.vlrMdaOtr = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
            //descripciones
            this.DocumentoDes = new UDT_Descriptivo();
            this.CuentaDes = new UDT_Descriptivo();
            this.TerceroDes = new UDT_Descriptivo();
            this.CentroCtoDes = new UDT_Descriptivo();
            this.ProyectoDes = new UDT_Descriptivo();
            this.LineaPresDes = new UDT_Descriptivo();
            this.ConceptoDes = new UDT_Descriptivo();
            this.ValorVenta = new UDT_Valor();
            this.ValorCompra = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [Filtrable]
        [NotImportable]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        [Filtrable]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        [Filtrable]
        [NotImportable]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        [Filtrable]
        [NotImportable]
        public UDT_Consecutivo ComprobanteNro { get; set; }       

        [DataMember]
        [Filtrable]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_Consecutivo DocumentoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint DocumentoTipo { get; set; }

        [DataMember]
        [Filtrable]
        [NotImportable]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        [Filtrable]
        [NotImportable]
        public UDTSQL_char DocumentoTercero { get; set; }

        [DataMember]
        [Filtrable]
        public string PrefDoc { get; set; }

        [DataMember]
        [Filtrable]
        public string Comprobante { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_CuentaID CuentaID { get; set; }
        
        [DataMember]
        [Filtrable]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_ConceptoCargoID ConceptoCargoID { get; set; }

        [DataMember]
        [Filtrable]
        [NotImportable]
        public UDT_LugarGeograficoID LugarGeograficoID { get; set; }

        [DataMember]
        [Filtrable]
        [NotImportable]
        public UDT_PrefijoID PrefijoCOM { get; set; }

        [DataMember]
        [Filtrable]
        [NotImportable]
        public UDTSQL_char DocumentoCOM { get; set; }

        [DataMember]
        [Filtrable]
        [NotImportable]
        public UDT_PlaquetaID ActivoCOM { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_ConceptoSaldoID ConceptoSaldoID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_BalanceTipoID BalanceTipoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_IdentificadorTR IdentificadorTR { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_Valor vlrMdaLoc { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_Valor vlrMdaExt { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        [NotImportable]
        public UDT_Valor vlrMdaOtr { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo Consecutivo { get; set; }

        //Descripciones
        [DataMember]
        [NotImportable]
        public UDT_Descriptivo  DocumentoDes { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo CuentaDes { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo TerceroDes { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo CentroCtoDes { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo ProyectoDes { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo LineaPresDes { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo ConceptoDes { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_Valor ValorVenta { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_Valor ValorCompra { get; set; }

        [DataMember]
        [NotImportable]
        public string ViewDoc { get; set; }

        #endregion
    }
}
