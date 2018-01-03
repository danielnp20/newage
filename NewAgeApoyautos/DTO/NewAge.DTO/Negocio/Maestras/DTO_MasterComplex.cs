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
    /// Class DTO_MasterComplex:
    /// Models the generic complex master DTO
    /// </summary>
    [DataContract]
    [Serializable]
    [KnownType(typeof(DTO_acContabiliza))]
    [KnownType(typeof(DTO_acDotacion))]
    [KnownType(typeof(DTO_acProduccionxPozoDUP))]
    [KnownType(typeof(DTO_acReservasxPozoDUP))]
    [KnownType(typeof(DTO_ccActividadesCobranza))]
    [KnownType(typeof(DTO_ccComponenteCuenta))]
    [KnownType(typeof(DTO_ccCompradorAnexos))]
    [KnownType(typeof(DTO_ccChequeoLista))]
    [KnownType(typeof(DTO_ccComponenteEdad))]
    [KnownType(typeof(DTO_ccCompradorPagaduria))]
    [KnownType(typeof(DTO_ccCompradorPortafilio))]
    [KnownType(typeof(DTO_ccCompradorMonto))]
    [KnownType(typeof(DTO_ccChequeoLista))]
    [KnownType(typeof(DTO_ccClasificacionxRiesgo))]
    [KnownType(typeof(DTO_ccEstadoCuentaCesionarioTabla))]
    [KnownType(typeof(DTO_ccFasecoldaModelo))]
    [KnownType(typeof(DTO_ccLineaComponente))]
    [KnownType(typeof(DTO_ccLineaComponenteMonto))]
    [KnownType(typeof(DTO_ccLineaComponentePlazo))]
    [KnownType(typeof(DTO_ccPagaduriaAnexos))]
    [KnownType(typeof(DTO_ccPoliticaIndicesCobranza))]
    [KnownType(typeof(DTO_ccTipoNomina))]
    [KnownType(typeof(DTO_ccValorAutorizado))]
    [KnownType(typeof(DTO_coImpuesto))]
    [KnownType(typeof(DTO_coImpuestoLocal))]
    [KnownType(typeof(DTO_coCargoCosto))]
    [KnownType(typeof(DTO_coBalanceReclasifica))]
    [KnownType(typeof(DTO_coComprBalanceTipo))]
    [KnownType(typeof(DTO_coDocumentoPermiso))]
    [KnownType(typeof(DTO_coValIVA))]
    [KnownType(typeof(DTO_coImpuestoFiltro))]
    [KnownType(typeof(DTO_coImpDeclaracionCuenta))]
    [KnownType(typeof(DTO_coImpDeclaracionRenglon))]
    [KnownType(typeof(DTO_coImpDeclaracionCalendario))]
    [KnownType(typeof(DTO_coActividadTercero))]
    [KnownType(typeof(DTO_coIvaRetencion))]
    [KnownType(typeof(DTO_coControl))]
    [KnownType(typeof(DTO_coComprobantePrefijo))]
    [KnownType(typeof(DTO_coTasaCierre))]
    [KnownType(typeof(DTO_coReporteLinea))]
    [KnownType(typeof(DTO_coReporteFiltro))]
    [KnownType(typeof(DTO_cpCargoAnticipoTipo))]
    [KnownType(typeof(DTO_cpDistribuyeImpLocal))]
    [KnownType(typeof(DTO_faPrecioServicio))]
    [KnownType(typeof(DTO_glActividadAreaFuncional))]
    [KnownType(typeof(DTO_glAreaFuncionalDocumentoPrefijo))]
    [KnownType(typeof(DTO_glDatosMensuales))]
    [KnownType(typeof(DTO_glDiasFestivos))]
    [KnownType(typeof(DTO_glDocMigracionCampo))]
    [KnownType(typeof(DTO_glDocumentoAnexo))]
    [KnownType(typeof(DTO_glEmpresaModulo))]
    [KnownType(typeof(DTO_glTasaCambio))]
    [KnownType(typeof(DTO_glTerceroReferencia))]
    [KnownType(typeof(DTO_glUsuarioxGrupo))]
    [KnownType(typeof(DTO_glActividadPermiso))]
    [KnownType(typeof(DTO_glNivelesAprobacionDoc))]
    [KnownType(typeof(DTO_glProcedimientoFlujo))]
    [KnownType(typeof(DTO_glActividadListaChequeo))]
    [KnownType(typeof(DTO_glLLamadaPregunta))]
    [KnownType(typeof(DTO_inBodegaRefe))]
    [KnownType(typeof(DTO_inBodegaUbicacion))]
    [KnownType(typeof(DTO_inConversionUnidad))]
    [KnownType(typeof(DTO_inPartesComponentes))]
    [KnownType(typeof(DTO_inRefEquivalentes))]
    [KnownType(typeof(DTO_inRefMarca))]
    [KnownType(typeof(DTO_inTipoParametro1))]
    [KnownType(typeof(DTO_inTipoParametro2))]
    [KnownType(typeof(DTO_inContabiliza))]
    [KnownType(typeof(DTO_noPrestacionesConvencion))]
    [KnownType(typeof(DTO_noAportesPorcentaje))]
    [KnownType(typeof(DTO_noBonificaciones))]
    [KnownType(typeof(DTO_noDistribuyeNomina))]
    [KnownType(typeof(DTO_noNovedadesxDia))]
    [KnownType(typeof(DTO_noEmpleadoFamilia))]
    [KnownType(typeof(DTO_noReteFuenteMinima))]
    [KnownType(typeof(DTO_noReteFuenteBasica))]
    [KnownType(typeof(DTO_ocConceptoCuenta))]
    [KnownType(typeof(DTO_ocContratoCampo))]
    [KnownType(typeof(DTO_ocGrupoCtaSocio))]
    [KnownType(typeof(DTO_ocParticionTabla))]
    [KnownType(typeof(DTO_ocParticionTablaFija))]
    [KnownType(typeof(DTO_ocProyectoTRM))]
    [KnownType(typeof(DTO_ocRangosOverhead))]
    [KnownType(typeof(DTO_ocTipoCosteoSocio))]
    [KnownType(typeof(DTO_plActividadLineaPresupuestal))]
    [KnownType(typeof(DTO_plDistribucionCampo))]
    [KnownType(typeof(DTO_plGrupoPresupuestalLinea))]
    [KnownType(typeof(DTO_plGrupoPresupuestalUsuario))]
    [KnownType(typeof(DTO_plTasasPresupuesto))]
    [KnownType(typeof(DTO_plRazonFinanciera))]
    [KnownType(typeof(DTO_pyRecursoCostoBase))]
    [KnownType(typeof(DTO_pyTarea))]
    [KnownType(typeof(DTO_pyTareaXLineaFlujo))]
    [KnownType(typeof(DTO_pyTrabajo))]
    [KnownType(typeof(DTO_pyTareaRecurso))]
    [KnownType(typeof(DTO_pyTareaClase))]
    [KnownType(typeof(DTO_rhCargosxAreaFuncional))]
    [KnownType(typeof(DTO_rhCompetenciasxCargo))]
    [KnownType(typeof(DTO_rhEstudioxCargo))]
    [KnownType(typeof(DTO_rhExperienciaxCargo))]
    [KnownType(typeof(DTO_rhFuncionxCargo))]
    [KnownType(typeof(DTO_rhPruebasxCargo))]
    [KnownType(typeof(DTO_seUsuarioGrupo))]
    [KnownType(typeof(DTO_seUsuarioPrefijo))]
    [KnownType(typeof(DTO_seGrupoDocumento))]
    [KnownType(typeof(DTO_tsBancosEncabezadoPE))]
    [KnownType(typeof(DTO_tsBancosDatosPE))]
    [KnownType(typeof(DTO_tsBancoCierrePE))]
    [KnownType(typeof(DTO_tsConceptoExtracto))]
    [KnownType(typeof(DTO_pyCapituloCliente))]
    [KnownType(typeof(DTO_prProveedorPrecio))]
    public class DTO_MasterComplex
    {
        #region Constructor

        /// <summary>
        /// Builds a DTO from a datareader
        /// </summary>
        /// <param name="?"></param>
        public DTO_MasterComplex(IDataReader dr, DTO_aplMaestraPropiedades props)
        {
            this.InitCols(props);

            //La lista de resultados ya trae por defecto la empresa
            this.ActivoInd.Value = Convert.ToBoolean(dr["ActivoInd"]);
            this.CtrlVersion.Value = Convert.ToInt16(dr["CtrlVersion"]);
            this.ReplicaID.Value = Convert.ToInt32(dr["ReplicaID"]);

            if(props.GrupoEmpresaInd)
                this.EmpresaGrupoID.Value = Convert.ToString(dr["EmpresaGrupoID"]);
        }

        /// <summary>
        /// Constructor por defecto con la lista de propiedades
        /// </summary>
        public DTO_MasterComplex(DTO_aplMaestraPropiedades props) 
        {
            this.InitCols(props);
        }  

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_MasterComplex() {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PKValues = new Dictionary<string, string>();
            this.ActivoInd = new UDT_SiNo();
            this.CtrlVersion = new UDT_CtrlVersion();
            this.ReplicaID = new UDT_ReplicaID();
            this.EmpresaGrupoID = new UDT_EmpresaGrupoID();
        }

        /// <summary>
        /// Constructor usado por reflection para llenar los dtos simpoles
        /// </summary>
        /// <param name="basic"></param>
        public DTO_MasterComplex(DTO_MasterComplex comp)
        {
            this.PKValues = comp.PKValues;
            this.ActivoInd = comp.ActivoInd;
            this.CtrlVersion = comp.CtrlVersion;
            this.ReplicaID = comp.ReplicaID;
            this.EmpresaGrupoID = comp.EmpresaGrupoID;
        }  

        /// <summary>
        /// Inicializa las columnas con un listado de propiedades
        /// </summary>
        /// <param name="props"></param>
        protected virtual void InitCols(DTO_aplMaestraPropiedades props)
        {
            this.ActivoInd = new UDT_SiNo();
            this.CtrlVersion = new UDT_CtrlVersion();
            this.ReplicaID = new UDT_ReplicaID();
            this.EmpresaGrupoID = new UDT_EmpresaGrupoID();

            this.PKValues = new Dictionary<string, string>();
            foreach (DTO_aplMaestraCampo field in props.Campos)
            {
                if (field.PKInd)
                {
                    this.PKValues.Add(field.NombreColumna, string.Empty);
                }
            }
        }

        #endregion 

        /// <summary>
        /// Gets or sets the list of PKs (col, valor)
        /// </summary>
        [DataMember]
        public Dictionary<string, string> PKValues { get; set; }

        /// <summary>
        /// Gets or sets the EmpresaGrupoID
        /// </summary>
        [DataMember]
        public UDT_EmpresaGrupoID EmpresaGrupoID { get; set; }

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
