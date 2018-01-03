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
    /// Class comprobante manual:
    /// Models DTO_ComprobanteFooter
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ComprobanteFooter
    {
        #region DTO_ComprobanteFooter

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ComprobanteFooter(IDataReader dr, bool isTC = false)
        {
            InitCols();
            try
            {
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
                this.IdentificadorTR.Value = Convert.ToInt64(dr["IdentificadorTR"]);
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.TasaCambio.Value = isTC ? Convert.ToDecimal(dr["TasaCambio"]) : Convert.ToDecimal(dr["TasaCambioBase"]);
                this.vlrBaseML.Value = Convert.ToDecimal(dr["vlrBaseML"]);
                this.vlrBaseME.Value = Convert.ToDecimal(dr["vlrBaseME"]);
                this.vlrMdaLoc.Value = Convert.ToDecimal(dr["vlrMdaLoc"]);
                this.vlrMdaExt.Value = Convert.ToDecimal(dr["vlrMdaExt"]);
                this.CuentaAlternaID.Value = dr["CuentaAlternaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["vlrMdaOtr"].ToString()))
                    this.vlrMdaOtr.Value = Convert.ToDecimal(dr["vlrMdaOtr"]);
                this.DatoAdd1.Value = dr["DatoAdd1"].ToString();
                this.DatoAdd2.Value = dr["DatoAdd2"].ToString();
                this.DatoAdd3.Value = dr["DatoAdd3"].ToString();
                this.DatoAdd4.Value = dr["DatoAdd4"].ToString();
                this.DatoAdd5.Value = dr["DatoAdd5"].ToString();
                this.DatoAdd6.Value = dr["DatoAdd6"].ToString();
                this.DatoAdd7.Value = dr["DatoAdd7"].ToString();
                this.DatoAdd8.Value = dr["DatoAdd8"].ToString();
                this.DatoAdd9.Value = dr["DatoAdd9"].ToString();
                this.DatoAdd10.Value = dr["DatoAdd10"].ToString();
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);

                this.PrefDoc = dr["PrefijoCOM"].ToString().Trim() + " - " + dr["DocumentoCOM"].ToString().Trim();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ComprobanteFooter()
        {
            this.InitCols();
        }

        /// <summary>
        /// Constructor a partir del padre
        /// </summary>
        /// <param name="parent">padre</param>
        public DTO_ComprobanteFooter(DTO_ComprobanteFooter parent)
        {
            this.CuentaID = parent.CuentaID;
            this.CuentaAlternaID = parent.CuentaAlternaID;
            this.TerceroID = parent.TerceroID;
            this.ProyectoID = parent.ProyectoID;
            this.CentroCostoID = parent.CentroCostoID;
            this.LineaPresupuestoID = parent.LineaPresupuestoID;
            this.ConceptoCargoID = parent.ConceptoCargoID;
            this.LugarGeograficoID = parent.LugarGeograficoID;
            this.PrefijoCOM = parent.PrefijoCOM;
            this.DocumentoCOM = parent.DocumentoCOM;
            this.ActivoCOM = parent.ActivoCOM;
            this.ConceptoSaldoID = parent.ConceptoSaldoID;
            this.IdentificadorTR = parent.IdentificadorTR;
            this.Descriptivo = parent.Descriptivo;
            this.TasaCambio = parent.TasaCambio;
            this.vlrBaseML = parent.vlrBaseML;
            this.vlrBaseME = parent.vlrBaseME;
            this.vlrMdaLoc = parent.vlrMdaLoc;
            this.vlrMdaExt = parent.vlrMdaExt;
            this.vlrMdaOtr = parent.vlrMdaOtr;
            this.DatoAdd1 = parent.DatoAdd1;
            this.DatoAdd2 = parent.DatoAdd2;
            this.DatoAdd3 = parent.DatoAdd3;
            this.DatoAdd4 = parent.DatoAdd4;
            this.DatoAdd4 = parent.DatoAdd5;
            this.DatoAdd4 = parent.DatoAdd6;
            this.DatoAdd4 = parent.DatoAdd7;
            this.DatoAdd4 = parent.DatoAdd8;
            this.DatoAdd4 = parent.DatoAdd9;
            this.DatoAdd9 = parent.DatoAdd10;
            this.Consecutivo = parent.Consecutivo;
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CuentaID = new UDT_CuentaID();
            this.CuentaAlternaID = new UDT_CuentaID();
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
            this.IdentificadorTR = new UDT_IdentificadorTR();
            this.Descriptivo = new UDT_DescripTBase();
            this.TasaCambio = new UDT_TasaID();
            this.vlrBaseML = new UDT_TasaID();
            this.vlrBaseME = new UDT_Valor();
            this.vlrMdaLoc = new UDT_Valor();
            this.vlrMdaExt = new UDT_Valor();
            this.vlrMdaOtr = new UDT_Valor();
            this.DatoAdd1 = new UDTSQL_char(50);
            this.DatoAdd2 = new UDTSQL_char(50);
            this.DatoAdd3 = new UDTSQL_char(50);
            this.DatoAdd4 = new UDTSQL_char(50);
            this.DatoAdd5 = new UDTSQL_char(50);
            this.DatoAdd6 = new UDTSQL_char(50);
            this.DatoAdd7 = new UDTSQL_char(50);
            this.DatoAdd8 = new UDTSQL_char(50);
            this.DatoAdd9 = new UDTSQL_char(50);
            this.DatoAdd10 = new UDTSQL_char(50);
            this.Consecutivo = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public int Index { get; set; }

        [DataMember]
        [NotImportable]
        public string PrefDoc { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CuentaID CuentaAlternaID { get; set; }

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
        public UDT_LugarGeograficoID LugarGeograficoID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_PrefijoID PrefijoCOM { get; set; }

        [DataMember]
        [Filtrable]
        public UDTSQL_char DocumentoCOM { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_PlaquetaID ActivoCOM { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_ConceptoSaldoID ConceptoSaldoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_IdentificadorTR IdentificadorTR { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_TasaID TasaCambio { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_TasaID vlrBaseML { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_Valor vlrBaseME { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_Valor vlrMdaLoc { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_Valor vlrMdaExt { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_Valor vlrMdaOtr { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDTSQL_char DatoAdd1 { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDTSQL_char DatoAdd2 { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDTSQL_char DatoAdd3 { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDTSQL_char DatoAdd4 { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDTSQL_char DatoAdd5 { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDTSQL_char DatoAdd6 { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDTSQL_char DatoAdd7 { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDTSQL_char DatoAdd8 { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDTSQL_char DatoAdd9 { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDTSQL_char DatoAdd10 { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo Consecutivo { get; set; }

        #endregion
    }
}
