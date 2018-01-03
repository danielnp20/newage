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
    /// Models DTO_coPlanCuenta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coPlanCuenta : DTO_MasterHierarchyBasic
    {
        #region DTO_coPlanCuenta
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coPlanCuenta(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.CuentaIFRSDesc.Value = dr["CuentaIFRSDesc"].ToString();
                    this.LibroUnicoDesc.Value = dr["LibroUnicoDesc"].ToString();
                    this.ConceptoSaldoDesc.Value = dr["ConceptoSaldoDesc"].ToString();
                    this.CuentaAlternaDesc.Value = dr["CuentaAlternaDesc"].ToString();
                    this.CtaGrupoDesc.Value = dr["CtaGrupoDesc"].ToString();
                    this.ConceptoCargoDesc.Value = dr["ConceptoCargoDesc"].ToString();
                    this.ImpTipoDesc.Value = dr["ImpTipoDesc"].ToString();
                    this.NITCierreDesc.Value = dr["NITCierreDesc"].ToString();
                    this.NITInfExogenaDesc.Value = dr["NITInfExogenaDesc"].ToString();
                    this.LugarGeograficoDesc.Value = dr["LugarGeograficoDesc"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["ocGrupoDesc"].ToString()))
                        this.ocGrupoDesc.Value = dr["ocGrupoDesc"].ToString();
                    this.FuenteUsoDesc.Value = dr["FuenteUsoDesc"].ToString();
                    //this.NotaRevelacionDesc.Value = dr["NotaRevelacionDesc"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["CuentaAnulaDesc"].ToString()))
                        this.CuentaAnulaDesc.Value = dr["CuentaAnulaDesc"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["CuentaCostoDesc"].ToString()))
                        this.CuentaCostoDesc.Value = dr["CuentaCostoDesc"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["LineaPresupuestoDesc"].ToString()))
                        this.LineaPresupuestoDesc.Value = dr["LineaPresupuestoDesc"].ToString();
                }
                if (!string.IsNullOrWhiteSpace(dr["ocGrupoID"].ToString()))
                    this.ocGrupoID.Value = dr["ocGrupoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FuenteUsoID"].ToString()))
                    this.FuenteUsoID.Value = dr["FuenteUsoID"].ToString();
                this.Descriptivo1.Value = dr["Descriptivo1"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CuentaIFRS"].ToString()))
                    this.CuentaIFRS.Value = dr["CuentaIFRS"].ToString();
                this.Tipo.Value = dr["Tipo"].ToString();
                this.Naturaleza.Value = Convert.ToByte(dr["Naturaleza"]);
                this.ConceptoSaldoID.Value = dr["ConceptoSaldoID"].ToString();
                this.CuentaAlternaID.Value = dr["CuentaAlternaID"].ToString();
                this.OrigenMonetario.Value = Convert.ToByte(dr["OrigenMonetario"]);
                this.CuentaGrupoID.Value = dr["CuentaGrupoID"].ToString();
                this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                this.ImpuestoTipoID.Value = dr["ImpuestoTipoID"].ToString();
                this.LugarGeograficoID.Value = dr["LugarGeograficoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ImpuestoPorc"].ToString()))
                    this.ImpuestoPorc.Value = Convert.ToDecimal(dr["ImpuestoPorc"]);
                if (!string.IsNullOrWhiteSpace(dr["MontoMinimo"].ToString()))
                    this.MontoMinimo.Value = Convert.ToInt32(dr["MontoMinimo"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoTercero"].ToString()))
                    this.TipoTercero.Value = Convert.ToByte(dr["TipoTercero"]);
                this.NITCierreAnual.Value = dr["NITCierreAnual"].ToString();
                this.NITInfExogena.Value = dr["NITInfExogena"].ToString();
                this.TerceroInd.Value = Convert.ToBoolean(dr["TerceroInd"]);
                this.TerceroSaldosInd.Value = Convert.ToBoolean(dr["TerceroSaldosInd"]);
                this.CentroCostoInd.Value = Convert.ToBoolean(dr["CentroCostoInd"]);
                this.DocumentoControlInd.Value = Convert.ToBoolean(dr["DocumentoControlInd"]);
                this.ProyectoInd.Value = Convert.ToBoolean(dr["ProyectoInd"]);
                this.LineaPresupuestalInd.Value = Convert.ToBoolean(dr["LineaPresupuestalInd"]);
                this.ConceptoCargoInd.Value = Convert.ToBoolean(dr["ConceptoCargoInd"]);
                this.LugarGeograficoInd.Value = Convert.ToBoolean(dr["LugarGeograficoInd"]);
                this.MascaraCta.Value = Convert.ToByte(dr["MascaraCta"]);
                if (!string.IsNullOrWhiteSpace(dr["CuentaCostoIVA"].ToString()))
                    this.CuentaCostoIVA.Value = Convert.ToString(dr["CuentaCostoIVA"]);
                if (!string.IsNullOrWhiteSpace(dr["LineaPresupuestoID"].ToString()))
                    this.LineaPresupuestoID.Value = Convert.ToString(dr["LineaPresupuestoID"]);
                this.AjCambioTerceroInd.Value = Convert.ToBoolean(dr["AjCambioTerceroInd"]);
                this.AjCambioRealizadoInd.Value = Convert.ToBoolean(dr["AjCambioRealizadoInd"]);
                this.NoDeducibleInd.Value = Convert.ToBoolean(dr["NoDeducibleInd"]);
                if (!string.IsNullOrWhiteSpace(dr["CuentaAnulaID"].ToString()))
                    this.CuentaAnulaID.Value = dr["CuentaAnulaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["LibroUnicoID"].ToString()))
                    this.LibroUnicoID.Value = dr["LibroUnicoID"].ToString();
                    this.DescriptivoIFRS.Value = dr["DescriptivoIFRS"].ToString();
                this.CtaCorrienteInd.Value = Convert.ToBoolean(dr["CtaCorrienteInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coPlanCuenta()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Descriptivo1 = new UDT_DescripTBase();
            this.CuentaIFRS = new UDT_BasicID();
            this.CuentaIFRSDesc = new UDT_Descriptivo();
            this.Tipo = new UDTSQL_char(1);
            this.Naturaleza = new UDTSQL_tinyint();
            this.ConceptoSaldoID = new UDT_BasicID();
            this.ConceptoSaldoDesc = new UDT_Descriptivo();
            this.CuentaAlternaID = new UDT_BasicID();
            this.CuentaAlternaDesc = new UDT_Descriptivo();
            this.OrigenMonetario = new UDTSQL_tinyint();
            this.CuentaGrupoID = new UDT_BasicID();
            this.CtaGrupoDesc = new UDT_Descriptivo();
            this.ConceptoCargoID = new UDT_BasicID();
            this.ConceptoCargoDesc = new UDT_Descriptivo();
            this.ImpuestoTipoID = new UDT_BasicID();
            this.ImpTipoDesc = new UDT_Descriptivo();
            this.LugarGeograficoID = new UDT_BasicID();
            this.LugarGeograficoDesc = new UDT_Descriptivo();
            this.ImpuestoPorc = new UDT_PorcentajeID();
            this.MontoMinimo = new UDTSQL_int();
            this.TipoTercero = new UDTSQL_tinyint();
            this.NITCierreAnual = new UDT_BasicID();
            this.NITCierreDesc = new UDT_Descriptivo();
            this.NITInfExogena = new UDT_BasicID();
            this.NITInfExogenaDesc = new UDT_Descriptivo();
            this.TerceroInd = new UDT_SiNo();
            this.TerceroSaldosInd = new UDT_SiNo();
            this.CentroCostoInd = new UDT_SiNo();
            this.DocumentoControlInd = new UDT_SiNo();
            this.ProyectoInd = new UDT_SiNo();
            this.LineaPresupuestalInd = new UDT_SiNo();
            this.ConceptoCargoInd = new UDT_SiNo();
            this.LugarGeograficoInd = new UDT_SiNo();
            this.MascaraCta = new UDTSQL_tinyint();
            this.CuentaCostoIVA = new UDT_BasicID();
            this.CuentaCostoDesc = new UDT_Descriptivo();
            this.LineaPresupuestoID = new UDT_BasicID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
            this.AjCambioTerceroInd = new UDT_SiNo();
            this.AjCambioRealizadoInd = new UDT_SiNo();
            this.NoDeducibleInd = new UDT_SiNo();
            this.ocGrupoID = new UDT_BasicID();
            this.ocGrupoDesc = new UDT_Descriptivo();
            this.FuenteUsoID = new UDT_BasicID();
            this.FuenteUsoDesc = new UDT_Descriptivo();
            this.NotaRevelacionID = new UDT_BasicID();
            this.NotaRevelacionDesc = new UDT_Descriptivo();
            this.CuentaAnulaID = new UDT_BasicID();
            this.CuentaAnulaDesc = new UDT_Descriptivo();
            this.LibroUnicoID = new UDT_BasicID();
            this.LibroUnicoDesc = new UDT_Descriptivo();
            this.DescriptivoIFRS = new UDT_DescripTBase();
            this.CtaCorrienteInd = new UDT_SiNo();
        }

        public DTO_coPlanCuenta(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coPlanCuenta(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        #region  Propiedades

        [DataMember]
        public UDT_DescripTBase Descriptivo1 { get; set; }

        [DataMember]
        public UDT_BasicID CuentaIFRS { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaIFRSDesc { get; set; }

        [DataMember]
        public UDTSQL_char Tipo { get; set; }

        [DataMember]
        public UDTSQL_tinyint Naturaleza { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoSaldoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoSaldoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaAlternaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaAlternaDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaAnulaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaAnulaDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint OrigenMonetario { get; set; }

        [DataMember]
        public UDT_BasicID CuentaGrupoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CtaGrupoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCargoDesc { get; set; }

        [DataMember]
        public UDT_BasicID LugarGeograficoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LugarGeograficoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ImpuestoTipoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ImpTipoDesc { get; set; }

        [DataMember]
        public UDT_PorcentajeID ImpuestoPorc { get; set; }

        [DataMember]
        public UDTSQL_int MontoMinimo { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoTercero { get; set; }

        [DataMember]
        public UDT_BasicID NITCierreAnual { get; set; }

        [DataMember]
        public UDT_Descriptivo NITCierreDesc { get; set; }

        [DataMember]
        public UDT_BasicID NITInfExogena { get; set; }

        [DataMember]
        public UDT_Descriptivo NITInfExogenaDesc { get; set; }

        [DataMember]
        public UDT_SiNo TerceroInd { get; set; }

        [DataMember]
        public UDT_SiNo TerceroSaldosInd { get; set; }

        [DataMember]
        public UDT_SiNo CentroCostoInd { get; set; }

        [DataMember]
        public UDT_SiNo DocumentoControlInd { get; set; }

        [DataMember]
        public UDT_SiNo ProyectoInd { get; set; }

        [DataMember]
        public UDT_SiNo LineaPresupuestalInd { get; set; }

        [DataMember]
        public UDT_SiNo ConceptoCargoInd { get; set; }

        [DataMember]
        public UDT_SiNo LugarGeograficoInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint MascaraCta { get; set; }

        [DataMember]
        public UDT_BasicID CuentaCostoIVA { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaCostoDesc { get; set; }        

        [DataMember]
        public UDT_BasicID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; }

        [DataMember]
        public UDT_SiNo AjCambioTerceroInd { get; set; }

        [DataMember]
        public UDT_SiNo AjCambioRealizadoInd { get; set; }

        [DataMember]
        public UDT_SiNo NoDeducibleInd { get; set; }

        [DataMember]
        public UDT_BasicID ocGrupoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ocGrupoDesc { get; set; }

        [DataMember]
        public UDT_BasicID FuenteUsoID { get; set; }

        [DataMember]
        public UDT_Descriptivo FuenteUsoDesc { get; set; }

        [DataMember]
        public UDT_BasicID NotaRevelacionID { get; set; }

        [DataMember]
        public UDT_Descriptivo NotaRevelacionDesc { get; set; }

        [DataMember]
        public UDT_BasicID LibroUnicoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LibroUnicoDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase DescriptivoIFRS { get; set; }

        [DataMember]
        public UDT_SiNo CtaCorrienteInd { get; set; }
        #endregion
    }
}
