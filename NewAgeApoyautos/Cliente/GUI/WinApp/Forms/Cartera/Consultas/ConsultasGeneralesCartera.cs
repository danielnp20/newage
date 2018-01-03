using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.Threading;
using NewAge.DTO.Resultados;
using SentenceTransformer;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class ConsultasGeneralesCartera : QueryFiltersForm
    {
        #region Constructores

        public ConsultasGeneralesCartera()
            : base()
        {
            //InitializeComponent();
        }

        public ConsultasGeneralesCartera(string mod)
            : base(mod)
        {
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicializa los parametros de la pantalla
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppQueries.ConsultasGeneralesCc;
            this.documenFiltroID = AppDynamicQueries.CreditosLiquida;
            this.frmModule = ModulesPrefix.cc;

            //Queries
            this.docs.Add(AppDynamicQueries.CreditosLiquida, _bc.GetResource(LanguageTypes.Forms, AppDynamicQueries.CreditosLiquida.ToString()));
            this.docs.Add(AppDynamicQueries.Saldos, _bc.GetResource(LanguageTypes.Forms, AppDynamicQueries.Saldos.ToString()));
            this.docs.Add(AppDynamicQueries.Recaudos, _bc.GetResource(LanguageTypes.Forms, AppDynamicQueries.Recaudos.ToString()));
            this.docs.Add(AppDynamicQueries.Cesiones, _bc.GetResource(LanguageTypes.Forms, AppDynamicQueries.Cesiones.ToString()));
            this.docs.Add(AppDynamicQueries.Flujos, _bc.GetResource(LanguageTypes.Forms, AppDynamicQueries.Flujos.ToString()));
            this.docs.Add(AppDynamicQueries.Incorporaciones, _bc.GetResource(LanguageTypes.Forms, AppDynamicQueries.Incorporaciones.ToString()));
            this.docs.Add(AppDynamicQueries.EstadosCuenta, _bc.GetResource(LanguageTypes.Forms, AppDynamicQueries.EstadosCuenta.ToString()));
            this.docs.Add(AppDynamicQueries.ProduccionSolic, _bc.GetResource(LanguageTypes.Forms, AppDynamicQueries.ProduccionSolic.ToString()));

            base.SetInitParameters();
        }


        #endregion  

        #region Eventos Formulario

        /// <summary>
        /// Evento para lista los documetnos asociados por glDocumento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void lkpConsulta_EditValueChanged(object sender, System.EventArgs e)
        {
            string Consulta = string.Empty;
            try
            {
                base.lkpConsulta_EditValueChanged(sender, e);
                this.documenFiltroID = this.lkpConsulta.EditValue != null ? Convert.ToInt32(this.lkpConsulta.EditValue) : 0;

                switch (this.documenFiltroID)
                {
                    #region CreditoLiquida
                    case AppDynamicQueries.CreditosLiquida:
                        this.viewType = typeof(DTO_VistaQ_ccCreditosLiquida);
                        this.viewName = "VistaQ_ccCreditosLiquida";

                        #region Carga los filtros

                        List<string> colsFilter = new List<string>();
                        colsFilter.Add("NumeroDoc");
                        colsFilter.Add("EmpresaID");

                        this.mq = new MasterQuery(this, this.documenFiltroID, this.userId, true, this.viewType, colsFilter, this.documentID);

                        //Fks
                        this.mq.SetFK("Cliente", AppMasters.ccCliente, _bc.CreateFKConfig(AppMasters.ccCliente));
                        this.mq.SetFK("Zona", AppMasters.glZona, _bc.CreateFKConfig(AppMasters.glZona));
                        this.mq.SetFK("Ciudad", AppMasters.glLugarGeografico, _bc.CreateFKConfig(AppMasters.glLugarGeografico));
                        this.mq.SetFK("CentroPago", AppMasters.ccCentroPagoPAG, _bc.CreateFKConfig(AppMasters.ccCentroPagoPAG));
                        this.mq.SetFK("PagaduriaID", AppMasters.ccPagaduria, _bc.CreateFKConfig(AppMasters.ccPagaduria));
                        this.mq.SetFK("ConcesionarioID", AppMasters.ccConcesionario, _bc.CreateFKConfig(AppMasters.ccConcesionario));
                        this.mq.SetFK("CooperativaID", AppMasters.ccCooperativa, _bc.CreateFKConfig(AppMasters.ccCooperativa));
                        this.mq.SetFK("AreaFuncionalID", AppMasters.glAreaFuncional, _bc.CreateFKConfig(AppMasters.glAreaFuncional));
                        this.mq.SetFK("CentroCostoID", AppMasters.coCentroCosto, _bc.CreateFKConfig(AppMasters.coCentroCosto));
                        this.mq.SetFK("LugarGeograficoID", AppMasters.ccCentroPagoPAG, _bc.CreateFKConfig(AppMasters.ccCentroPagoPAG));
                        this.mq.SetFK("CuentaID", AppMasters.coPlanCuenta, _bc.CreateFKConfig(AppMasters.coPlanCuenta));
                        this.mq.SetFK("ComprobanteID", AppMasters.coComprobante, _bc.CreateFKConfig(AppMasters.coComprobante));
                        this.mq.SetFK("LineaCredito", AppMasters.ccLineaCredito, _bc.CreateFKConfig(AppMasters.ccLineaCredito));
                        this.mq.SetFK("TipoCredito", AppMasters.ccTipoCredito, _bc.CreateFKConfig(AppMasters.ccTipoCredito));

                        //Combos
                        //Estado
                        Dictionary<string, string> dicEstate = new Dictionary<string, string>();
                        dicEstate.Add("", this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField));
                        dicEstate.Add("-1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateCerrado));
                        dicEstate.Add("0", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAnulado));
                        dicEstate.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateSinAprobar));
                        dicEstate.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateParaAprobacion));
                        dicEstate.Add("3", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAprobado));
                        dicEstate.Add("4", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateRevertido));
                        dicEstate.Add("5", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateDevuelto));
                        dicEstate.Add("6", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateRadicado));
                        dicEstate.Add("7", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateRevisado));
                        dicEstate.Add("8", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateContabilizado));
                        this.mq.SetValueDictionary("Estado", dicEstate);

                        #endregion
                        break; 
                    #endregion

                    #region Saldos
                    case AppDynamicQueries.Saldos:
                        this.viewType = typeof(DTO_VistaQ_ccSaldosCartera);
                        this.viewName = "VistaQ_SaldosCartera";

                        #region Carga los filtros

                        List<string> colsFilterSaldo = new List<string>();
                        colsFilterSaldo.Add("NumeroDoc");
                        colsFilterSaldo.Add("EmpresaID");

                        this.mq = new MasterQuery(this, this.documenFiltroID, this.userId, true, this.viewType, colsFilterSaldo, this.documentID);

                        this.mq.SetFK("ClienteID", AppMasters.ccCliente, _bc.CreateFKConfig(AppMasters.ccCliente));
                        this.mq.SetFK("CompradorCarteraID", AppMasters.ccCompradorCartera, _bc.CreateFKConfig(AppMasters.ccCompradorCartera));
                        this.mq.SetFK("PagaduriaID", AppMasters.ccPagaduria, _bc.CreateFKConfig(AppMasters.ccPagaduria));
                        this.mq.SetFK("AsesorID", AppMasters.ccAsesor, _bc.CreateFKConfig(AppMasters.ccAsesor));
                        this.mq.SetFK("LineaCreditoID", AppMasters.ccLineaCredito, _bc.CreateFKConfig(AppMasters.ccLineaCredito));

                        #endregion
                        break;   
                    #endregion   
                  
                    #region Recaudos Masivos
                    case AppDynamicQueries.Recaudos:
                        this.viewType = typeof(VistaQ_ccRecaudoMasivo);
                        this.viewName = "VistaQ_ccRecaudoMasivo";

                        #region Cargar Filtros
                        List<string> colsFilterRecaudos = new List<string>();
                        colsFilterRecaudos.Add("NumeroDoc");
                        colsFilterRecaudos.Add("EmpresaID");

                        this.mq = new MasterQuery(this, this.documenFiltroID, this.userId, true, this.viewType, colsFilterRecaudos, this.documentID);

                        this.mq.SetFK("TerceroID", AppMasters.ccCliente, _bc.CreateFKConfig(AppMasters.ccCliente));
                        this.mq.SetFK("PagaduriaID", AppMasters.ccPagaduria, _bc.CreateFKConfig(AppMasters.ccPagaduria));
                        this.mq.SetFK("CooperativaID", AppMasters.ccCooperativa, _bc.CreateFKConfig(AppMasters.ccCooperativa));
                        this.mq.SetFK("CobranzaEstadoID", AppMasters.ccCobranzaEstado, _bc.CreateFKConfig(AppMasters.ccCobranzaEstado));
                        this.mq.SetFK("CobranzaGestionID", AppMasters.ccCobranzaGestion, _bc.CreateFKConfig(AppMasters.ccCobranzaEstado));
                        #endregion

                        break; 
                    #endregion

                    #region Cesiones
                    case AppDynamicQueries.Cesiones:
                        this.viewType = typeof(DTO_VistaQ_ccCesiones);
                        this.viewName = "VistaQ_ccCesiones";

                        #region Cargar Filtros
                        List<string> colsFilterCesiones = new List<string>();
                        colsFilterCesiones.Add("NumeroDoc");
                        colsFilterCesiones.Add("EmpresaID");

                        this.mq = new MasterQuery(this, this.documenFiltroID, this.userId, true, this.viewType, colsFilterCesiones, this.documentID);

                        this.mq.SetFK("TerceroID", AppMasters.ccCliente, _bc.CreateFKConfig(AppMasters.ccCliente));
                        this.mq.SetFK("Ciudad_Nacimiento", AppMasters.glLugarGeografico, _bc.CreateFKConfig(AppMasters.glLugarGeografico));
                        this.mq.SetFK("ResidenciaDir", AppMasters.glLugarGeografico, _bc.CreateFKConfig(AppMasters.glLugarGeografico));
                        this.mq.SetFK("LaboralCiudad", AppMasters.glLugarGeografico, _bc.CreateFKConfig(AppMasters.glLugarGeografico));
                        this.mq.SetFK("Cooperativa", AppMasters.ccCooperativa, _bc.CreateFKConfig(AppMasters.ccCooperativa));
                        this.mq.SetFK("Id_Pagaduria", AppMasters.ccPagaduria, _bc.CreateFKConfig(AppMasters.ccPagaduria));
                        this.mq.SetFK("AsesorID", AppMasters.ccAsesor, _bc.CreateFKConfig(AppMasters.ccAsesor));
                        this.mq.SetFK("IDEstrato", AppMasters.acEstado, _bc.CreateFKConfig(AppMasters.acEstado));

                        #endregion

                        break; 
                    #endregion

                    #region Flujos
                    case AppDynamicQueries.Flujos:
                        this.viewType = typeof(DTO_VistaQ_ccCesiones);
                        this.viewName = "VistaQ_ccFlujos";

                        #region Cargar Filtros
                        List<string> colsFilterFlujo = new List<string>();
                        colsFilterFlujo.Add("NumeroDoc");
                        colsFilterFlujo.Add("EmpresaID");

                        this.mq = new MasterQuery(this, this.documenFiltroID, this.userId, true, this.viewType, colsFilterFlujo, this.documentID);

                        this.mq.SetFK("TerceroID", AppMasters.ccCliente, _bc.CreateFKConfig(AppMasters.ccCliente));
                        this.mq.SetFK("Ciudad_Nacimiento", AppMasters.glLugarGeografico, _bc.CreateFKConfig(AppMasters.glLugarGeografico));
                        this.mq.SetFK("ResidenciaDir", AppMasters.glLugarGeografico, _bc.CreateFKConfig(AppMasters.glLugarGeografico));
                        this.mq.SetFK("LaboralCiudad", AppMasters.glLugarGeografico, _bc.CreateFKConfig(AppMasters.glLugarGeografico));
                        this.mq.SetFK("Cooperativa", AppMasters.ccCooperativa, _bc.CreateFKConfig(AppMasters.ccCooperativa));
                        this.mq.SetFK("Id_Pagaduria", AppMasters.ccPagaduria, _bc.CreateFKConfig(AppMasters.ccPagaduria));
                        this.mq.SetFK("AsesorID", AppMasters.ccAsesor, _bc.CreateFKConfig(AppMasters.ccAsesor));
                        this.mq.SetFK("IDEstrato", AppMasters.acEstado, _bc.CreateFKConfig(AppMasters.acEstado));

                        #endregion

                        break;
                    #endregion

                    #region Incorporaciones
                    case AppDynamicQueries.Incorporaciones:
                        this.viewType = typeof(DTO_VistaQ_ccIncorpora);
                        this.viewName = "VistaQ_ccIncorporacion";

                        #region Cargar Filtros
                        List<string> colsFilterIncorp = new List<string>();
                        colsFilterIncorp.Add("NumeroDoc");
                        colsFilterIncorp.Add("EmpresaID");

                        this.mq = new MasterQuery(this, this.documenFiltroID, this.userId, true, this.viewType, colsFilterIncorp, this.documentID);

                        this.mq.SetFK("ClienteID", AppMasters.ccCliente, _bc.CreateFKConfig(AppMasters.ccCliente));
                        this.mq.SetFK("NovedadCodigo", AppMasters.ccIncorporacionNovedad, _bc.CreateFKConfig(AppMasters.ccIncorporacionNovedad));
                        this.mq.SetFK("SiniestroEstado", AppMasters.ccSiniestroEstado, _bc.CreateFKConfig(AppMasters.ccSiniestroEstado));
                        this.mq.SetFK("CobranzaGestion", AppMasters.ccCobranzaEstado, _bc.CreateFKConfig(AppMasters.ccCobranzaEstado));
                        this.mq.SetFK("LineaCredito", AppMasters.ccLineaCredito, _bc.CreateFKConfig(AppMasters.ccLineaCredito));
                        this.mq.SetFK("ProfesionCodigo", AppMasters.ccProfesion, _bc.CreateFKConfig(AppMasters.ccProfesion));
                        #endregion

                        break;
                    #endregion
                    
                    #region EstadoCuenta
                    case AppDynamicQueries.EstadosCuenta:
                        this.viewType = typeof(DTO_VistaQ_ccCesiones);
                        this.viewName = "VistaQ_ccEstadoCuenta";

                        #region Cargar Filtros
                        List<string> colsFilterEstadoCta = new List<string>();
                        colsFilterEstadoCta.Add("NumeroDoc");
                        colsFilterEstadoCta.Add("EmpresaID");

                        this.mq = new MasterQuery(this, this.documenFiltroID, this.userId, true, this.viewType, colsFilterEstadoCta, this.documentID);

                        this.mq.SetFK("TerceroID", AppMasters.ccCliente, _bc.CreateFKConfig(AppMasters.ccCliente));
                        this.mq.SetFK("Ciudad_Nacimiento", AppMasters.glLugarGeografico, _bc.CreateFKConfig(AppMasters.glLugarGeografico));
                        this.mq.SetFK("ResidenciaDir", AppMasters.glLugarGeografico, _bc.CreateFKConfig(AppMasters.glLugarGeografico));
                        this.mq.SetFK("LaboralCiudad", AppMasters.glLugarGeografico, _bc.CreateFKConfig(AppMasters.glLugarGeografico));
                        this.mq.SetFK("Cooperativa", AppMasters.ccCooperativa, _bc.CreateFKConfig(AppMasters.ccCooperativa));
                        this.mq.SetFK("Id_Pagaduria", AppMasters.ccPagaduria, _bc.CreateFKConfig(AppMasters.ccPagaduria));
                        this.mq.SetFK("AsesorID", AppMasters.ccAsesor, _bc.CreateFKConfig(AppMasters.ccAsesor));
                        this.mq.SetFK("IDEstrato", AppMasters.acEstado, _bc.CreateFKConfig(AppMasters.acEstado));

                        #endregion

                        break;
                    #endregion

                    #region Produccion Solicitudes
                    case AppDynamicQueries.ProduccionSolic:
                        this.viewType = typeof(DTO_VistaQ_ccProduccionSolic);
                        this.viewName = "VistaQ_ccProduccionSolic";

                        #region Cargar Filtros
                        List<string> colsFilterProduccionSol = new List<string>();
                        colsFilterProduccionSol.Add("NumeroDoc");
                        colsFilterProduccionSol.Add("EmpresaID");

                        this.mq = new MasterQuery(this, this.documenFiltroID, this.userId, true, this.viewType, colsFilterProduccionSol, this.documentID);

                        this.mq.SetFK("CooperativaID", AppMasters.ccCooperativa, _bc.CreateFKConfig(AppMasters.ccCooperativa));
                        this.mq.SetFK("CodAsesor", AppMasters.ccAsesor, _bc.CreateFKConfig(AppMasters.ccAsesor));
                        this.mq.SetFK("ProfesionID", AppMasters.ccProfesion, _bc.CreateFKConfig(AppMasters.ccProfesion));
                        this.mq.SetFK("Cedula", AppMasters.ccCliente, _bc.CreateFKConfig(AppMasters.ccCliente));
                        this.mq.SetFK("BancoID", AppMasters.tsBanco, _bc.CreateFKConfig(AppMasters.tsBanco));
                        this.mq.SetFK("LineaCredito", AppMasters.ccLineaCredito, _bc.CreateFKConfig(AppMasters.ccLineaCredito));
                        this.mq.SetFK("Usuario", AppMasters.seUsuario, _bc.CreateFKConfig(AppMasters.seUsuario));
                        this.mq.SetFK("PagadID", AppMasters.ccCentroPagoPAG, _bc.CreateFKConfig(AppMasters.ccCentroPagoPAG));
                        this.mq.SetFK("Zona", AppMasters.glZona, _bc.CreateFKConfig(AppMasters.glZona));
                        this.mq.SetFK("CentroCosto", AppMasters.coCentroCosto, _bc.CreateFKConfig(AppMasters.coCentroCosto));
                        #endregion

                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "ConsultasGeneralesCartera.cs", "lkpConsulta_EditValueChanged"));
            }
        }

        #endregion 
                       
    }
}