using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class DTO_MasterBasic:
    /// Models del generic basic master DTO
    /// </summary>
    [DataContract]
    [Serializable]

    [KnownType(typeof(DTO_acClase))]
    [KnownType(typeof(DTO_acGrupo))]
    [KnownType(typeof(DTO_acComponenteActivo))]
    [KnownType(typeof(DTO_acMovimientoTipo))]
    [KnownType(typeof(DTO_ccAsesor))]
    [KnownType(typeof(DTO_ccAseguradora))]
    [KnownType(typeof(DTO_ccCarteraComponente))]
    [KnownType(typeof(DTO_ccCentroPagoPAG))]
    [KnownType(typeof(DTO_ccChequeoLista))]
    [KnownType(typeof(DTO_ccCliente))]
    [KnownType(typeof(DTO_ccComisionDescAsesor))]
    [KnownType(typeof(DTO_ccComisionDescuento))]
    [KnownType(typeof(DTO_ccCompradorCartera))]
    [KnownType(typeof(DTO_ccCompradorGrupo))]
    [KnownType(typeof(DTO_ccConcesionario))]
    [KnownType(typeof(DTO_ccEstadoCartera))]
    [KnownType(typeof(DTO_ccFasecolda))]
    [KnownType(typeof(DTO_ccFinanciera))]
    [KnownType(typeof(DTO_ccLineaCredito))]
    [KnownType(typeof(DTO_ccNominaINC))]
    [KnownType(typeof(DTO_ccPagaduria))]
    [KnownType(typeof(DTO_ccTipoCredito))]
    [KnownType(typeof(DTO_ccValorAmparado))]
    [KnownType(typeof(DTO_coCentroCosto))]
    [KnownType(typeof(DTO_coComprobante))]
    [KnownType(typeof(DTO_coCuentaGrupo))]
    [KnownType(typeof(DTO_coImpuestoTipo))]
    [KnownType(typeof(DTO_coPlanCuenta))]
    [KnownType(typeof(DTO_ccProfesion))]
    [KnownType(typeof(DTO_coProyecto))]
    [KnownType(typeof(DTO_coOperacion))]
    [KnownType(typeof(DTO_coActEconomica))]
    [KnownType(typeof(DTO_coTercero))]
    [KnownType(typeof(DTO_coBalanceTipo))]
    [KnownType(typeof(DTO_coFuenteUso))]
    [KnownType(typeof(DTO_coImpuestoDeclaracion))]
    [KnownType(typeof(DTO_coImpuestoFormato))]
    [KnownType(typeof(DTO_coOperacion))]
    [KnownType(typeof(DTO_coTerceroDocTipo))]
    [KnownType(typeof(DTO_coPlanCuenta))]
    [KnownType(typeof(DTO_coProyecto))]
    [KnownType(typeof(DTO_coConceptoCargo))]
    [KnownType(typeof(DTO_coDocumento))]
    [KnownType(typeof(DTO_coImpuestoConcepto))]
    [KnownType(typeof(DTO_coLineaNegocio))]
    [KnownType(typeof(DTO_coTasaCierre))]
    [KnownType(typeof(DTO_coRegimenFiscal))]
    [KnownType(typeof(DTO_coReporteFiltro))]
    [KnownType(typeof(DTO_coReporteNota))]
    [KnownType(typeof(DTO_coUnidadGenEfectivo))]
    [KnownType(typeof(DTO_cpCargoEspecial))]
    [KnownType(typeof(DTO_cpConceptoCXP))]
    [KnownType(typeof(DTO_cpCajaMenor))]
    [KnownType(typeof(DTO_cpAnticipoTipo))]
    [KnownType(typeof(DTO_cpTarjetaCredito))]
    [KnownType(typeof(DTO_faCliente))]
    [KnownType(typeof(DTO_faConceptos))]
    [KnownType(typeof(DTO_faServicios))]
    [KnownType(typeof(DTO_faAsesor))]
    [KnownType(typeof(DTO_faFacturaTipo))]
    [KnownType(typeof(DTO_glActividadFlujo))]
    [KnownType(typeof(DTO_glAreaFisica))]
    [KnownType(typeof(DTO_glAreaFuncional))]
    [KnownType(typeof(DTO_glDocumento))]
    [KnownType(typeof(DTO_glEmpresa))]
    [KnownType(typeof(DTO_glEmpresaGrupo))]
    [KnownType(typeof(DTO_glIncumplimientoEtapa))]
    [KnownType(typeof(DTO_glLocFisica))]
    [KnownType(typeof(DTO_glLugarGeografico))]
    [KnownType(typeof(DTO_glBienServicioClase))]
    [KnownType(typeof(DTO_glConceptoSaldo))]
    [KnownType(typeof(DTO_glDatosAnuales))]
    [KnownType(typeof(DTO_glDocMigracionEstructura))]
    [KnownType(typeof(DTO_glDocumentoTipo))]
    [KnownType(typeof(DTO_glGarantia))]
    [KnownType(typeof(DTO_glGrupoAprueba))]
    [KnownType(typeof(DTO_glHorarioTrabajo))]
    [KnownType(typeof(DTO_glSeccionFuncional))]
    [KnownType(typeof(DTO_glPais))]
    [KnownType(typeof(DTO_glProcedimiento))]
    [KnownType(typeof(DTO_glTabla))]
    [KnownType(typeof(DTO_inBodega))]
    [KnownType(typeof(DTO_inBodegaTipo))]
    [KnownType(typeof(DTO_inEmpaque))]
    [KnownType(typeof(DTO_inMovimientoTipo))]
    [KnownType(typeof(DTO_inRefClase))]
    [KnownType(typeof(DTO_inReferencia))]
    [KnownType(typeof(DTO_inRefGrupo))]
    [KnownType(typeof(DTO_inRefTipo))]
    [KnownType(typeof(DTO_inImportacionModalidad))]
    [KnownType(typeof(DTO_noEmpleado))]
    [KnownType(typeof(DTO_noFondo))]
    [KnownType(typeof(DTO_noCaja))]
    [KnownType(typeof(DTO_noConceptoPlaTra))]
    [KnownType(typeof(DTO_noContratoNov))]
    [KnownType(typeof(DTO_noConvencion))]
    [KnownType(typeof(DTO_noComponenteNomina))]
    [KnownType(typeof(DTO_noRiesgo))]
    [KnownType(typeof(DTO_noTurnoCompensatorio))]
    [KnownType(typeof(DTO_noRol))]
    [KnownType(typeof(DTO_noConceptoNOM))]
    [KnownType(typeof(DTO_noCompFlexible))]
    [KnownType(typeof(DTO_inPosicionArancel))]
    [KnownType(typeof(DTO_inUnidad))]
    [KnownType(typeof(DTO_inCosteoGrupo))]
    [KnownType(typeof(DTO_inReferenciaCod))]
    [KnownType(typeof(DTO_ocCuentaGrupo))]
    [KnownType(typeof(DTO_ocOtrosConceptos))]
    [KnownType(typeof(DTO_ocSocio))]
    [KnownType(typeof(DTO_ocTipoCosto))]
    [KnownType(typeof(DTO_plLineaPresupuesto))]
    [KnownType(typeof(DTO_plGrupoPresupuestal))]
    [KnownType(typeof(DTO_plRecurso))]
    [KnownType(typeof(DTO_plLineaPresupuesto))]
    [KnownType(typeof(DTO_prBienServicio))]
    [KnownType(typeof(DTO_prProveedor))]
    [KnownType(typeof(DTO_pyClaseProyecto))]
    [KnownType(typeof(DTO_pyContrato))]
    [KnownType(typeof(DTO_pyEtapa))]
    [KnownType(typeof(DTO_pyLineaFlujo))]
    [KnownType(typeof(DTO_pyListaPrecio))]
    [KnownType(typeof(DTO_pyRecurso))]
    [KnownType(typeof(DTO_pyTrabajo))]    
    [KnownType(typeof(DTO_rhCargos))]
    [KnownType(typeof(DTO_rhCompetencia))]
    [KnownType(typeof(DTO_rhNivelSalarial))]
    [KnownType(typeof(DTO_seUsuario))]
    [KnownType(typeof(DTO_seLAN))]
    [KnownType(typeof(DTO_tsBancosCuenta))]
    [KnownType(typeof(DTO_tsCaja))]
    [KnownType(typeof(DTO_tsIngresoConcepto))]
    [KnownType(typeof(DTO_tsNotaBancaria))]
    [KnownType(typeof(DTO_tsFlujoFondo))]
    [KnownType(typeof(DTO_tsBanco))]
    [KnownType(typeof(DTO_tsFormaPago))]
    [KnownType(typeof(DTO_faListaPrecio))]
    [KnownType(typeof(DTO_ccCobranzaEstado))]
    [KnownType(typeof(DTO_MasterHierarchyBasic))]
    public class DTO_MasterBasic
    {
        #region Constructor

        /// <summary>
        /// Builds a DTO from a datareader
        /// </summary>
        /// <param name="?"></param>
        public DTO_MasterBasic(IDataReader dr, DTO_aplMaestraPropiedades props, bool isReplica = false)
        {
            this.IdName = props.ColumnaID;
            this.InitCols(props);

            if (props.GrupoEmpresaInd)
                this.EmpresaGrupoID.Value = Convert.ToString(dr["EmpresaGrupoID"]);

            //La lista de resultados ya trae por defecto la empresa
            this.ID.Value = Convert.ToString(dr[this.IdName]);
            this.Descriptivo.Value = Convert.ToString(dr["Descriptivo"]);
            this.ActivoInd.Value = Convert.ToBoolean(dr["ActivoInd"]);
            this.CtrlVersion.Value = Convert.ToInt16(dr["CtrlVersion"]);
            this.ReplicaID.Value = Convert.ToInt32(dr["ReplicaID"]);
        }

        /// <summary>
        /// Constructor por defecto con la lista de propiedades
        /// </summary>
        public DTO_MasterBasic(DTO_aplMaestraPropiedades props) 
        {
            this.IdName = props.ColumnaID;
            this.InitCols(props);
        }  

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_MasterBasic() {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ID = new UDT_BasicID();
            this.ID.MaxLength = 50;
            this.Descriptivo = new UDT_Descriptivo();
            this.ActivoInd = new UDT_SiNo();
            this.CtrlVersion = new UDT_CtrlVersion();
            this.ReplicaID = new UDT_ReplicaID();
            this.EmpresaGrupoID = new UDT_EmpresaGrupoID();
        }

        /// <summary>
        /// Constructor usado por reflection para llenar los dtos simpoles
        /// </summary>
        /// <param name="basic"></param>
        public DTO_MasterBasic(DTO_MasterBasic basic)
        {
            this.IdName = basic.IdName;
            this.ID = basic.ID;
            this.Descriptivo = basic.Descriptivo;
            this.ActivoInd = basic.ActivoInd;
            this.CtrlVersion = basic.CtrlVersion;
            this.ReplicaID = basic.ReplicaID;
            this.EmpresaGrupoID = basic.EmpresaGrupoID;
        }  

        protected virtual void InitCols(DTO_aplMaestraPropiedades props)
        {
            this.ID = new UDT_BasicID();
            this.ID.MaxLength = props.IDLongitudMax;
            this.Descriptivo = new UDT_Descriptivo();
            this.ActivoInd = new UDT_SiNo();
            this.CtrlVersion = new UDT_CtrlVersion();
            this.ReplicaID = new UDT_ReplicaID();

            if (props.GrupoEmpresaInd)
                this.EmpresaGrupoID = new UDT_EmpresaGrupoID();

        }

        #endregion 

        /// <summary>
        /// Gets or sets the name for the pk field
        /// </summary>
        [DataMember]
        public string IdName { get; set; }

        /// <summary>
        /// Gets or sets the pk field
        /// </summary>
        [DataMember]
        public UDT_BasicID ID { get; set; }

        /// <summary>
        /// Gets or sets the EmpresaID
        /// </summary>
        [DataMember]
        public UDT_EmpresaGrupoID EmpresaGrupoID { get; set; }

        /// <summary>
        /// Gets or sets the Descriptivo
        /// </summary>
        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        /// <summary>
        /// Gets or sets the ActivoInd
        /// </summary>
        [DataMember]
        public UDT_SiNo ActivoInd { get; set; }

        /// <summary>
        /// Gets or sets the CtrlVersion
        /// </summary>
        [DataMember]
        public UDT_CtrlVersion CtrlVersion { get; set; }

        /// <summary>
        /// Gets or sets the ReplicaID
        /// </summary>
        [DataMember]
        public UDT_ReplicaID ReplicaID { get; set; }
    }
}
