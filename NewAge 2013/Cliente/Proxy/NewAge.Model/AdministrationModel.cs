﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.DTO.Negocio.Documentos.Activos;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;
using NewAge.Server.GlobalService;
using NewAge.Server.AppService;
using NewAge.Server.MasterService;
using NewAge.Server.ActivosService;
using NewAge.Server.CarteraService;
using NewAge.Server.ContabilidadService;
using NewAge.Server.CFTService;
using NewAge.Server.NominaService;
using NewAge.Server.ProveedoresService;
using NewAge.Server.OpConjuntasService;
using NewAge.Server.InventariosService;
using NewAge.Server.ReportesService;
using SentenceTransformer;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;

namespace NewAge.Cliente.Proxy.Model
{
    /// <summary>
    /// Modela las operaciones y validaciones de la lógica de negocio para la información del sitio
    /// </summary>
    public partial class AdministrationModel : BaseModel
    {
        #region Variables y Propiedades (Servicios)

        private bool isLocal;

        #region Servicios
        #region Reportes
        /// <summary>
        /// Servicio de Inventarios 
        /// InventariosService
        /// </summary>
        private ChannelFactory<IReportesService> reportesChannelFactory = new ChannelFactory<IReportesService>("WSHttpBinding_IReportesService");
        private IReportesService reportesLocalService = null;
        private IReportesService _reportesServiceWCF;
        internal IReportesService ReportesServiceWCF
        {
            get
            {
                return this._reportesServiceWCF;
            }
        }
        internal IReportesService ReportesService
        {
            get
            {
                if (this.BasicConfig != null && this.BasicConfig.RemotoInd.Value == false)
                {
                    if (reportesLocalService == null)
                        reportesLocalService = new ReportesService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);

                    return reportesLocalService;
                }
                else
                    return (this.ReportesServiceWCF as IReportesService);
            }
        }
        #endregion
        #region App
        /// <summary>
        /// Servicio de Aplicacion 
        /// AdministrationAppService
        /// </summary>
        private ChannelFactory<IAppService> appChannelFactory = new ChannelFactory<IAppService>("WSHttpBinding_IAppService");
        private IAppService _temporales = null;
        private IAppService appLocalService = null;
        private IAppService _appServiceWCF;
        internal IAppService AppServiceWCF
        {
            get
            {
                return this._appServiceWCF;
            }
        }
        internal IAppService AppService
        {
            get
            {
                if (this.BasicConfig != null && this.BasicConfig.RemotoInd.Value == false)
                {
                    if (appLocalService == null)
                        appLocalService = new AppService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);

                    return appLocalService;
                }
                else
                    return (this.AppServiceWCF as IAppService);
            }
        }
        #endregion
        #region Master
        /// <summary>
        /// Servicio de Maestras 
        /// AdministrationMasterService
        /// </summary>
        private ChannelFactory<IMasterService> masterChannelFactory = new ChannelFactory<IMasterService>("WSHttpBinding_IMasterService");
        private IMasterService masterLocalService = null;
        private IMasterService _masterServiceWCF;
        internal IMasterService MasterServiceWCF
        {
            get
            {
                return this._masterServiceWCF;
            }
        }
        internal IMasterService MasterService
        {
            get
            {
                if (this.BasicConfig != null && this.BasicConfig.RemotoInd.Value == false)
                {
                    if (masterLocalService == null)
                        masterLocalService = new MasterService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);

                    return masterLocalService;
                }
                else
                    return (this.MasterServiceWCF as IMasterService);
            }

        }
        #endregion
        #region Global
        /// <summary>
        /// Servicio de Adminstración 
        /// AdministrationService
        /// </summary>
        private ChannelFactory<IGlobalService> globalChannelFactory = new ChannelFactory<IGlobalService>("WSHttpBinding_IGlobalService");
        private IGlobalService globalLocalService = null;
        private IGlobalService _globalServiceWCF;
        internal IGlobalService GlobalServiceWCF
        {
            get
            {
                return this._globalServiceWCF;
            }
        }
        internal IGlobalService GlobalService
        {
            get
            {
                if (this.BasicConfig != null && this.BasicConfig.RemotoInd.Value == false)
                {
                    if (globalLocalService == null)
                        globalLocalService = new GlobalService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);

                    return globalLocalService;
                }
                else
                    return (this.GlobalServiceWCF as IGlobalService);
            }
        }
        #endregion
        #region Activos
        /// <summary>
        /// Servicio de Activos Fijos 
        /// ActivosService
        /// </summary>
        private ChannelFactory<IActivosService> activosChannelFactory = new ChannelFactory<IActivosService>("WSHttpBinding_IActivosService");
        private IActivosService activosLocalService = null;
        private IActivosService _activosServiceWCF;
        internal IActivosService ActivosServiceWCF
        {
            get
            {
                return this._activosServiceWCF;
            }
        }
        internal IActivosService ActivosService
        {
            get
            {
                if (this.BasicConfig != null && this.BasicConfig.RemotoInd.Value == false)
                {
                    if (activosLocalService == null)
                        activosLocalService = new ActivosService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);

                    return activosLocalService;
                }
                else
                    return (this.ActivosServiceWCF as IActivosService);
            }
        }
        #endregion
        #region Cartera
        /// <summary>
        /// Servicio de Cartera 
        /// CarteraService
        /// </summary>
        private ChannelFactory<ICarteraService> carteraChannelFactory = new ChannelFactory<ICarteraService>("WSHttpBinding_ICarteraService");
        private ICarteraService carteraLocalService = null;
        private ICarteraService _carteraServiceWCF;
        internal ICarteraService CarteraServiceWCF
        {
            get
            {
                return this._carteraServiceWCF;
            }
        }
        internal ICarteraService CarteraService
        {
            get
            {
                if (this.BasicConfig != null && this.BasicConfig.RemotoInd.Value == false)
                {
                    if (carteraLocalService == null)
                        carteraLocalService = new CarteraService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);

                    return carteraLocalService;
                }
                else
                    return (this.CarteraServiceWCF as ICarteraService);
            }
        }
        #endregion
        #region Contabilidad
        /// <summary>
        /// Servicio de Contabilidad 
        /// ContabilidadService
        /// </summary>
        private ChannelFactory<IContabilidadService> contabilidadChannelFactory = new ChannelFactory<IContabilidadService>("WSHttpBinding_IContabilidadService");
        private IContabilidadService contabilidadLocalService = null;
        private IContabilidadService _contabilidadServiceWCF;
        internal IContabilidadService ContabilidadServiceWCF
        {
            get
            {
                return this._contabilidadServiceWCF;
            }
        }
        internal IContabilidadService ContabilidadService
        {
            get
            {
                if (this.BasicConfig != null && this.BasicConfig.RemotoInd.Value == false)
                {
                    if (contabilidadLocalService == null)
                        contabilidadLocalService = new ContabilidadService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);

                    return contabilidadLocalService;
                }
                else
                    return (this.ContabilidadServiceWCF as IContabilidadService);
            }
        }
        #endregion
        #region CxP
        /// <summary>
        /// Servicio de Contabilidad 
        /// ContabilidadService
        /// </summary>
        private ChannelFactory<ICFTService> cftChannelFactory = new ChannelFactory<ICFTService>("WSHttpBinding_ICFTService");
        private ICFTService cftLocalService = null;
        private ICFTService _cftServiceWCF;
        internal ICFTService CftServiceWCF
        {
            get
            {
                return this._cftServiceWCF;
            }
        }
        internal ICFTService CftService
        {
            get
            {
                if (this.BasicConfig != null && this.BasicConfig.RemotoInd.Value == false)
                {
                    if (contabilidadLocalService == null)
                        cftLocalService = new CFTService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);

                    return cftLocalService;
                }
                else
                    return (CftServiceWCF as ICFTService);
            }
        }
        #endregion
        #region Inventarios
        /// <summary>
        /// Servicio de Inventarios 
        /// InventariosService
        /// </summary>
        private ChannelFactory<IInventariosService> inventariosChannelFactory = new ChannelFactory<IInventariosService>("WSHttpBinding_IInventariosService");
        private IInventariosService inventariosLocalService = null;
        private IInventariosService _inventariosServiceWCF;
        internal IInventariosService InventariosServiceWCF
        {
            get
            {
                return this._inventariosServiceWCF;
            }
        }
        internal IInventariosService InventariosService
        {
            get
            {
                if (this.BasicConfig != null && this.BasicConfig.RemotoInd.Value == false)
                {
                    if (inventariosLocalService == null)
                        inventariosLocalService = new InventariosService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);

                    return inventariosLocalService;
                }
                else
                    return (this.InventariosServiceWCF as IInventariosService);
            }
        }
        #endregion
        #region Nomina
        /// <summary>
        /// Servicio de Nomina
        /// NominaService
        /// </summary>
        private ChannelFactory<INominaService> nominaChannelFactory = new ChannelFactory<INominaService>("WSHttpBinding_INominaService");
        private INominaService nominaLocalService = null;
        private INominaService _nominaServiceWCF;
        internal INominaService NominaServiceWCF
        {
            get
            {
                return this._nominaServiceWCF;
            }
        }
        internal INominaService NominaService
        {
            get
            {
                if (this.BasicConfig != null && this.BasicConfig.RemotoInd.Value == false)
                {
                    if (nominaLocalService == null)
                        nominaLocalService = new NominaService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);

                    return nominaLocalService;
                }
                else
                    return (this.NominaServiceWCF as INominaService);
            }
        }
        #endregion
        #region OpConjuntas
        /// <summary>
        /// Servicio de OpConjuntas 
        /// OpConjuntasService
        /// </summary>
        private ChannelFactory<IOpConjuntasService> opConjuntasChannelFactory = new ChannelFactory<IOpConjuntasService>("WSHttpBinding_IOpConjuntasService");
        private IOpConjuntasService opConjuntasLocalService = null;
        private IOpConjuntasService _opConjuntasServiceWCF;
        internal IOpConjuntasService OpConjuntasServiceWCF
        {
            get
            {
                return this._opConjuntasServiceWCF;
            }
        }
        internal IOpConjuntasService OpConjuntasService
        {
            get
            {
                if (this.BasicConfig != null && this.BasicConfig.RemotoInd.Value == false)
                {
                    if (opConjuntasLocalService == null)
                        opConjuntasLocalService = new OpConjuntasService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);

                    return opConjuntasLocalService;
                }
                else
                    return (this.OpConjuntasServiceWCF as IOpConjuntasService);
            }
        }
        #endregion
        #region Proveedores
        /// <summary>
        /// Servicio de Proveedores
        /// ProveedoresService
        /// </summary>
        private ChannelFactory<IProveedoresService> proveedoresChannelFactory = new ChannelFactory<IProveedoresService>("WSHttpBinding_IProveedoresService");
        private IProveedoresService proveedoresLocalService = null;
        private IProveedoresService _proveedoresServiceWCF;
        internal IProveedoresService ProveedoresServiceWCF
        {
            get
            {
                return this._proveedoresServiceWCF;
            }
        }
        internal IProveedoresService ProveedoresService
        {
            get
            {
                if (this.BasicConfig != null && this.BasicConfig.RemotoInd.Value == false)
                {
                    if (proveedoresLocalService == null)
                        proveedoresLocalService = new ProveedoresService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);

                    return proveedoresLocalService;
                }
                else
                    return (this.ProveedoresServiceWCF as IProveedoresService);
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Configurarion de la LAN que indica si es o no remoto
        /// </summary>
        private DTO_seLAN _basicConfig = null;
        public DTO_seLAN BasicConfig
        {
            get
            {
                return this._basicConfig;
            }
            set
            {
                this._basicConfig = value;
            }
        }

        /// <summary>
        /// Manejo de las veriables de persistencia 
        /// </summary>
        private IAdministrationModelPersistance _persistance;
        internal IAdministrationModelPersistance Persistance
        {
            get
            {
                return this._persistance;
            }
        }

        /// <summary>
        /// Identificador unico para el canal activo del usuario
        /// </summary>
        private Guid _userChannel;
        #endregion

        /// <summary>
        /// Constructor por defecto        
        /// </summary>
        public AdministrationModel(bool ipLocal, string connStr)
            : base()
        {
            this.isLocal = ipLocal;
            if (isLocal)
            {
                this.BasicConfig = new DTO_seLAN();
                this.BasicConfig.CadenaConn.Value = connStr;
                this.BasicConfig.CadenaConnLogger.Value = connStr;
                this.BasicConfig.RemotoInd.Value = false;
            }

            if (this.EnvironmentType == EnvironmentType.Web)
            {
                this._persistance = new WebAdministrationModelPersistance();
            }
            else if (this.EnvironmentType == EnvironmentType.Windows)
            {
                this._persistance = new WindowsAdministrationModelPersistance();
            }
            else
            {
                throw new ApplicationException("Ambiente no definido.");
            }
        }

        #region Funciones propias del AM

        /// <summary>
        /// Inicia el Administration Model
        /// </summary>
        /// <param name="basic">indica si son los servicios basicos o los de operacion</param>
        public void Start(bool basic)
        {
            try
            {
                if (basic)
                {
                    if (this._userChannel == Guid.Empty)
                        this._userChannel = Guid.NewGuid();

                    #region Servicios Basicos
                    if (this.BasicConfig.RemotoInd.Value.Value)
                    {
                        #region Servicios remotos

                        this._appServiceWCF = this.appChannelFactory.CreateChannel();
                        this.AppServiceWCF.CrearCanal(this._userChannel);
                        this.appLocalService = this._appServiceWCF;

                        this._masterServiceWCF = this.masterChannelFactory.CreateChannel();
                        this.MasterServiceWCF.CrearCanal(this._userChannel);
                        this.masterLocalService = this._masterServiceWCF;

                        this._globalServiceWCF = this.globalChannelFactory.CreateChannel();
                        this.GlobalServiceWCF.CrearCanal(this._userChannel);
                        this.globalLocalService = this._globalServiceWCF;

                        #endregion
                    }
                    else
                    {
                        #region Inicia los servicios por defecto
                        this.appLocalService = new AppService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);
                        this.masterLocalService = new MasterService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);
                        this.globalLocalService = new GlobalService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);

                        this.appLocalService.CrearCanal(this._userChannel);
                        this.masterLocalService.CrearCanal(this._userChannel);
                        this.globalLocalService.CrearCanal(this._userChannel);
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region servicios por modulo
                    if (this.BasicConfig.RemotoInd.Value.Value)
                    {
                        #region Inicia los por WCF

                        //Reportes
                        this._reportesServiceWCF = this.reportesChannelFactory.CreateChannel();
                        this.ReportesServiceWCF.CrearCanal(this._userChannel);
                        this.reportesLocalService = this._reportesServiceWCF;

                        //if (this.Modules.ContainsKey(ModulesPrefix.ac.ToString()))
                        //{
                        this._activosServiceWCF = this.activosChannelFactory.CreateChannel();
                        this.ActivosServiceWCF.CrearCanal(this._userChannel);
                        this.activosLocalService = this._activosServiceWCF;
                        //}
                        //if (this.Modules.ContainsKey(ModulesPrefix.cc.ToString()))
                        //{
                        this._carteraServiceWCF = this.carteraChannelFactory.CreateChannel();
                        this.CarteraServiceWCF.CrearCanal(this._userChannel);
                        this.carteraLocalService = this._carteraServiceWCF;//}
                        //if (this.Modules.ContainsKey(ModulesPrefix.co.ToString()))
                        //{
                        this._contabilidadServiceWCF = this.contabilidadChannelFactory.CreateChannel();
                        this.ContabilidadServiceWCF.CrearCanal(this._userChannel);
                        this.contabilidadLocalService = this._contabilidadServiceWCF;
                        //}
                        //if (this.Modules.ContainsKey(ModulesPrefix.cp.ToString()))
                        //{
                        this._cftServiceWCF = this.cftChannelFactory.CreateChannel();
                        this.CftServiceWCF.CrearCanal(this._userChannel);
                        this.cftLocalService = this._cftServiceWCF;
                        //}
                        //if (this.Modules.ContainsKey(ModulesPrefix.@in.ToString()))
                        //{
                        this._inventariosServiceWCF = this.inventariosChannelFactory.CreateChannel();
                        this.InventariosServiceWCF.CrearCanal(this._userChannel);
                        this.inventariosLocalService = this._inventariosServiceWCF;
                        //}
                        //if (this.Modules.ContainsKey(ModulesPrefix.no.ToString()))
                        //{
                        this._nominaServiceWCF = this.nominaChannelFactory.CreateChannel();
                        this.NominaServiceWCF.CrearCanal(this._userChannel);
                        this.nominaLocalService = this._nominaServiceWCF;
                        //}
                        //if (this.Modules.ContainsKey(ModulesPrefix.oc.ToString()))
                        //{
                        this._opConjuntasServiceWCF = this.opConjuntasChannelFactory.CreateChannel();
                        this.OpConjuntasServiceWCF.CrearCanal(this._userChannel);
                        this.opConjuntasLocalService = this._opConjuntasServiceWCF;
                        //}
                        //if (this.Modules.ContainsKey(ModulesPrefix.pr.ToString()))
                        //{
                        this._proveedoresServiceWCF = this.proveedoresChannelFactory.CreateChannel();
                        this.ProveedoresServiceWCF.CrearCanal(this._userChannel);
                        this.proveedoresLocalService = this._proveedoresServiceWCF;
                        //}

                        #endregion
                    }
                    else
                    {
                        #region Inicial los servicios locales por defecto

                        this.reportesLocalService = new ReportesService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);

                        //if (this.Modules.ContainsKey(ModulesPrefix.ac.ToString()))
                        this.activosLocalService = new ActivosService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);
                        //if (this.Modules.ContainsKey(ModulesPrefix.cc.ToString()))
                        this.carteraLocalService = new CarteraService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);
                        //if (this.Modules.ContainsKey(ModulesPrefix.co.ToString()))
                        this.contabilidadLocalService = new ContabilidadService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);
                        //if (this.Modules.ContainsKey(ModulesPrefix.cp.ToString()))
                        this.cftLocalService = new CFTService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);
                        //if (this.Modules.ContainsKey(ModulesPrefix.@in.ToString()))
                        this.inventariosLocalService = new InventariosService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);
                        //if (this.Modules.ContainsKey(ModulesPrefix.no.ToString()))
                        this.nominaLocalService = new NominaService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);
                        //if (this.Modules.ContainsKey(ModulesPrefix.oc.ToString()))
                        this.opConjuntasLocalService = new OpConjuntasService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);
                        //if (this.Modules.ContainsKey(ModulesPrefix.pr.ToString()))
                        this.proveedoresLocalService = new ProveedoresService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);

                        this.reportesLocalService.CrearCanal(this._userChannel);
                        //if (this.Modules.ContainsKey(ModulesPrefix.ac.ToString()))
                        this.activosLocalService.CrearCanal(this._userChannel);
                        //if (this.Modules.ContainsKey(ModulesPrefix.cc.ToString()))
                        this.carteraLocalService.CrearCanal(this._userChannel);
                        //if (this.Modules.ContainsKey(ModulesPrefix.co.ToString()))
                        this.contabilidadLocalService.CrearCanal(this._userChannel);
                        //if (this.Modules.ContainsKey(ModulesPrefix.cp.ToString()))
                        this.cftLocalService.CrearCanal(this._userChannel);
                        //if (this.Modules.ContainsKey(ModulesPrefix.@in.ToString()))
                        this.inventariosLocalService.CrearCanal(this._userChannel);
                        //if (this.Modules.ContainsKey(ModulesPrefix.no.ToString()))
                        this.nominaLocalService.CrearCanal(this._userChannel);
                        //if (this.Modules.ContainsKey(ModulesPrefix.oc.ToString()))
                        this.opConjuntasLocalService.CrearCanal(this._userChannel);
                        //if (this.Modules.ContainsKey(ModulesPrefix.pr.ToString()))
                        this.proveedoresLocalService.CrearCanal(this._userChannel);

                        #endregion
                    }

                    if (!string.IsNullOrWhiteSpace(BasicConfig.CadenaConn.Value))
                        this._temporales = new AppService(this.BasicConfig.CadenaConn.Value, this.BasicConfig.CadenaConnLogger.Value);
                    #endregion
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inicializa la empresa y el usuario
        /// </summary>
        public void IniciarEmpresaUsuario(bool basic)
        {
            if (basic)
            {
                #region Inicia las empresas de los servicios basicos

                if (this.BasicConfig != null && this.BasicConfig.RemotoInd.Value.Value)
                {
                    this.AppServiceWCF.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    this.GlobalServiceWCF.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                }
                else
                {
                    this.appLocalService.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    this.globalLocalService.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                }

                #endregion
            }
            else
            {
                #region Inicia los servicios de las empresas segun el modulo

                if (this.BasicConfig != null && this.BasicConfig.RemotoInd.Value.Value)
                {
                    #region Remoto

                    //Basicos
                    this.ReportesServiceWCF.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    this.AppServiceWCF.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    this.MasterServiceWCF.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    this.GlobalServiceWCF.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);

                    //Por modulo
                    //  if (this.Modules.ContainsKey(ModulesPrefix.ac.ToString()))
                    this.ActivosServiceWCF.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    // if (this.Modules.ContainsKey(ModulesPrefix.cc.ToString()))
                    this.CarteraServiceWCF.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    // if (this.Modules.ContainsKey(ModulesPrefix.co.ToString()))
                    this.ContabilidadServiceWCF.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    // if (this.Modules.ContainsKey(ModulesPrefix.cp.ToString()))
                    this.CftServiceWCF.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    // if (this.Modules.ContainsKey(ModulesPrefix.@in.ToString()))
                    this.InventariosServiceWCF.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    // if (this.Modules.ContainsKey(ModulesPrefix.no.ToString()))
                    this.NominaServiceWCF.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    //  if (this.Modules.ContainsKey(ModulesPrefix.oc.ToString()))
                    this.OpConjuntasServiceWCF.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    // if (this.Modules.ContainsKey(ModulesPrefix.pr.ToString()))
                    this.ProveedoresServiceWCF.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);


                    this._temporales.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    #endregion
                }
                else
                {
                    #region Local

                    //Basicos
                    this.reportesLocalService.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    this.appLocalService.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    this.masterLocalService.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    this.globalLocalService.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);

                    //Por modulo
                    //if (this.Modules.ContainsKey(ModulesPrefix.ac.ToString()))
                    this.activosLocalService.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    //if (this.Modules.ContainsKey(ModulesPrefix.cc.ToString()))
                    this.carteraLocalService.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    //if (this.Modules.ContainsKey(ModulesPrefix.co.ToString()))
                    this.contabilidadLocalService.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    //if (this.Modules.ContainsKey(ModulesPrefix.cp.ToString()))
                    this.cftLocalService.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    //if (this.Modules.ContainsKey(ModulesPrefix.@in.ToString()))
                    this.inventariosLocalService.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    // if (this.Modules.ContainsKey(ModulesPrefix.no.ToString()))
                    this.nominaLocalService.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    // if (this.Modules.ContainsKey(ModulesPrefix.oc.ToString()))
                    this.opConjuntasLocalService.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);
                    // if (this.Modules.ContainsKey(ModulesPrefix.pr.ToString()))
                    this.proveedoresLocalService.IniciarEmpresaUsuario(this._userChannel, this.Empresa, this.User.ReplicaID.Value.Value);

                    #endregion
                }

                #endregion
            }
        }

        #endregion

        #region Operaciones de autenticación

        /// <summary>
        /// Saca el usuario actual
        /// </summary>
        public void LogOut()
        {
            this.Persistance.LogOut();
        }

        /// <summary>
        /// Autentica un usuario
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="rememberMe">True to remember the user, false otherwise</param>
        public void LogOn(string userName, bool rememberMe)
        {
            this.Persistance.LogOn(userName, rememberMe);
        }

        #endregion

        #region Propiedades públicas (Persistencia)

        /// <summary>
        /// Código del usuario
        /// </summary>
        public DTO_seUsuario User
        {
            get
            {
                return this.Persistance.User;
            }
            set
            {
                this.Persistance.User = value;
            }
        }

        /// <summary>
        /// Lista de datos de la tabla de control
        /// </summary>
        public List<DTO_glControl> ControlList
        {
            get
            {
                return this.Persistance.ControlList;
            }
            set
            {
                this.Persistance.ControlList = value;
            }
        }

        /// <summary>
        /// Empresa en la que se esta trabajando
        /// </summary>
        public DTO_glEmpresa Empresa
        {
            get
            {
                return this.Persistance.Empresa;
            }
            set
            {
                this.Persistance.Empresa = value;
            }
        }

        /// <summary>
        /// Pais en el que se esta trabajando
        /// </summary>
        public DTO_glPais Pais
        {
            get
            {
                return this.Persistance.Pais;
            }
            set
            {
                this.Persistance.Pais = value;
            }
        }

        /// <summary>
        /// Grupo de empresas por defecto
        /// </summary>
        public string EmpresaGrupoGeneral
        {
            get
            {
                return this.Persistance.EmpresaGrupoGeneral;
            }
            set
            {
                this.Persistance.EmpresaGrupoGeneral = value;
            }
        }

        /// <summary>
        /// Seguridades del usuario
        /// </summary>
        public Dictionary<string, string> FormsSecurity
        {
            get
            {
                return this.Persistance.FormsSecurity;
            }
            set
            {
                this.Persistance.FormsSecurity = value;
            }
        }

        /// <summary>
        /// Diccionario con lista de configuraciones
        /// </summary>
        public Dictionary<string, string> Config
        {
            get
            {
                return this.Persistance.Config;
            }
            set
            {
                this.Persistance.Config = value;
            }
        }

        /// <summary>
        /// Diccionario con lista de modulos
        /// </summary>
        public Dictionary<string, string> Modules
        {
            get
            {
                return this.Persistance.Modules;
            }
            set
            {
                this.Persistance.Modules = value;
            }
        }

        /// <summary>
        /// Diccionario con lista de idiomas
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Languages
        {
            get
            {
                return this.Persistance.Languages;
            }
            set
            {
                this.Persistance.Languages = value;
            }
        }

        /// <summary>
        /// Diccionario con lista de menus por modulo
        /// </summary>
        public Dictionary<string, MainMenu> Menus
        {
            get
            {
                return this.Persistance.Menus;
            }
            set
            {
                this.Persistance.Menus = value;
            }
        }

        /// <summary>
        /// Diccionario con lista de tablas
        /// </summary>
        public Dictionary<Tuple<int, string>, DTO_glTabla> Tables
        {
            get
            {
                return this.Persistance.Tables;
            }
            set
            {
                this.Persistance.Tables = value;
            }
        }

        /// <summary>
        /// Diccionario con lista de tablas
        /// </summary>
        public Dictionary<int, DTO_aplMaestraPropiedades> MasterProperties
        {
            get
            {
                return this.Persistance.MasterProperties;
            }
            set
            {
                this.Persistance.MasterProperties = value;
            }
        }

        /// <summary>
        /// Objeto que contiene los ultimos datos copiados
        /// </summary>
        public object DataCopied
        {
            get
            {
                return this.Persistance.DataCopied;
            }
            set
            {
                this.Persistance.DataCopied = value;
            }
        }

        /// <summary>
        /// Indica si la empresa sobre la que se trabaja maneja multimoneda
        /// </summary>
        public bool MultiMoneda
        {
            get
            {
                return this.Persistance.MultiMoneda;
            }
            set
            {
                this.Persistance.MultiMoneda = value;
            }
        }

        #endregion

        #region Creación y cierre de las operaciones del servicio

        /// <summary>
        /// Obtiene el servicio listo para usar
        /// </summary>
        private void CreateService(Type serviceType)
        {
            try
            {
                if (this.BasicConfig == null || this.BasicConfig.RemotoInd.Value.Value)
                {
                    #region Se llama 1 vez por cada servicio creado y es remoto

                    //Reportes
                    if (serviceType == typeof(IReportesService) && (this._reportesServiceWCF == null || this._userChannel == Guid.Empty))
                    {
                        this._reportesServiceWCF = this.reportesChannelFactory.CreateChannel();
                        this.ReportesServiceWCF.CrearCanal(this._userChannel);
                    }
                    //Global
                    if (serviceType == typeof(IGlobalService) && (this._globalServiceWCF == null || this._userChannel == Guid.Empty))
                    {
                        this._globalServiceWCF = this.globalChannelFactory.CreateChannel();
                        this.GlobalServiceWCF.CrearCanal(this._userChannel);
                    }

                    //App
                    if (serviceType == typeof(IAppService) && (this._appServiceWCF == null || this._userChannel == Guid.Empty))
                    {
                        this._appServiceWCF = this.appChannelFactory.CreateChannel();
                        this.AppServiceWCF.CrearCanal(this._userChannel);
                    }

                    //Master
                    if (serviceType == typeof(IMasterService) && (this._masterServiceWCF == null || this._userChannel == Guid.Empty))
                    {
                        this._masterServiceWCF = this.masterChannelFactory.CreateChannel();
                        this.MasterServiceWCF.CrearCanal(this._userChannel);
                    }

                    //Activos
                    if (serviceType == typeof(IActivosService) && (this._activosServiceWCF == null || this._userChannel == Guid.Empty))
                    {
                        this._activosServiceWCF = this.activosChannelFactory.CreateChannel();
                        this.ActivosServiceWCF.CrearCanal(this._userChannel);
                    }

                    //Cartera
                    if (serviceType == typeof(ICarteraService) && (this._carteraServiceWCF == null || this._userChannel == Guid.Empty))
                    {
                        this._carteraServiceWCF = this.carteraChannelFactory.CreateChannel();
                        this.CarteraServiceWCF.CrearCanal(this._userChannel);
                    }

                    //Contabilidad
                    if (serviceType == typeof(IContabilidadService) && (this._contabilidadServiceWCF == null || this._userChannel == Guid.Empty))
                    {
                        this._contabilidadServiceWCF = this.contabilidadChannelFactory.CreateChannel();
                        this.ContabilidadServiceWCF.CrearCanal(this._userChannel);
                    }

                    //CxP
                    if (serviceType == typeof(ICFTService) && (this._cftServiceWCF == null || this._userChannel == Guid.Empty))
                    {
                        this._cftServiceWCF = this.cftChannelFactory.CreateChannel();
                        this.CftServiceWCF.CrearCanal(this._userChannel);
                    }

                    //Inventarios
                    if (serviceType == typeof(IInventariosService) && (this._inventariosServiceWCF == null || this._userChannel == Guid.Empty))
                    {
                        this._inventariosServiceWCF = this.inventariosChannelFactory.CreateChannel();
                        this.InventariosServiceWCF.CrearCanal(this._userChannel);
                    }

                    //Nomina
                    if (serviceType == typeof(INominaService) && (this._nominaServiceWCF == null || this._userChannel == Guid.Empty))
                    {
                        this._nominaServiceWCF = this.nominaChannelFactory.CreateChannel();
                        this.NominaServiceWCF.CrearCanal(this._userChannel);
                    }

                    //Op Conjuntas
                    if (serviceType == typeof(IOpConjuntasService) && (this._opConjuntasServiceWCF == null || this._userChannel == Guid.Empty))
                    {
                        this._opConjuntasServiceWCF = this.opConjuntasChannelFactory.CreateChannel();
                        this.OpConjuntasServiceWCF.CrearCanal(this._userChannel);
                    }

                    //Proveedores
                    if (serviceType == typeof(IProveedoresService) && (this._proveedoresServiceWCF == null || this._userChannel == Guid.Empty))
                    {
                        this._proveedoresServiceWCF = this.proveedoresChannelFactory.CreateChannel();
                        this.ProveedoresServiceWCF.CrearCanal(this._userChannel);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Err: " + ex.Message);
            }
        }

        /// <summary>
        /// Cierra el canal de los servicios
        /// </summary>
        public void CloseChannels()
        {
            //Reportes
            try
            {
                if (this.ReportesService != null)
                    this.ReportesService.CerrarCanal(this._userChannel);
            }
            catch (Exception ex) { }

            //Global
            try
            {
                if (this.GlobalService != null)
                    this.GlobalService.CerrarCanal(this._userChannel);
            }
            catch (Exception ex) { }

            //Aplicacion
            try
            {
                if (this.AppService != null)
                    this.AppService.CerrarCanal(this._userChannel);
            }
            catch (Exception ex) { }

            //Maestras
            try
            {
                if (this.MasterService != null)
                    this.MasterService.CerrarCanal(this._userChannel);
            }
            catch (Exception ex) { }

            //Activos
            try
            {
                if (this.ActivosService != null)
                    this.ActivosService.CerrarCanal(this._userChannel);
            }
            catch (Exception ex) { }

            //Cartera
            try
            {
                if (this.CarteraService != null)
                    this.CarteraService.CerrarCanal(this._userChannel);
            }
            catch (Exception ex) { }

            //Contabilidad
            try
            {

                if (this.ContabilidadService != null)
                    this.ContabilidadService.CerrarCanal(this._userChannel);
            }
            catch (Exception ex) { }

            //CxP
            try
            {
                if (this.CftService != null)
                    this.CftService.CerrarCanal(this._userChannel);
            }
            catch (Exception ex) { }

            //Inventarios
            try
            {
                if (this.InventariosService != null)
                    this.InventariosService.CerrarCanal(this._userChannel);
            }
            catch (Exception ex) { }

            //Nomina
            try
            {
                if (this.NominaService != null)
                    this.NominaService.CerrarCanal(this._userChannel);
            }
            catch (Exception ex) { }

            //Proveedores
            try
            {
                if (this.ProveedoresService != null)
                    this.ProveedoresService.CerrarCanal(this._userChannel);
            }
            catch (Exception ex) { }

            //Operaciones conjuntas
            try
            {
                if (this.OpConjuntasService != null)
                    this.OpConjuntasService.CerrarCanal(this._userChannel);
            }
            catch (Exception ex) { }

        }

        /// <summary>
        /// Aborta el servicio
        /// </summary>
        private void AbortService(Type serviceType, bool forceWcf = false)
        {
            if (this.BasicConfig == null || this.BasicConfig.RemotoInd.Value.Value || forceWcf)
            {
                if (serviceType == typeof(IReportesService))
                    this._reportesServiceWCF.ADO_CloseDBConnection(-1);
                if (serviceType == typeof(IAppService))
                    this._appServiceWCF.ADO_CloseDBConnection(-1);
                if (serviceType == typeof(IMasterService))
                    this._masterServiceWCF.ADO_CloseDBConnection(-1);
                if (serviceType == typeof(IGlobalService))
                    this._globalServiceWCF.ADO_CloseDBConnection(-1);
                if (serviceType == typeof(IActivosService))
                    this._activosServiceWCF.ADO_CloseDBConnection(-1);
                if (serviceType == typeof(ICarteraService))
                    this._carteraServiceWCF.ADO_CloseDBConnection(-1);
                if (serviceType == typeof(IContabilidadService))
                    this._contabilidadServiceWCF.ADO_CloseDBConnection(-1);
                if (serviceType == typeof(ICFTService))
                    this._cftServiceWCF.ADO_CloseDBConnection(-1);
                if (serviceType == typeof(INominaService))
                    this._nominaServiceWCF.ADO_CloseDBConnection(-1);
                if (serviceType == typeof(IInventariosService))
                    this._inventariosServiceWCF.ADO_CloseDBConnection(-1);
                if (serviceType == typeof(IProveedoresService))
                    this._proveedoresServiceWCF.ADO_CloseDBConnection(-1);
                if (serviceType == typeof(IOpConjuntasService))
                    this._opConjuntasServiceWCF.ADO_CloseDBConnection(-1);
            }
        }

        #endregion

        #region Servicios WCF

        #region  AppService

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción de un usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ConsultarProgresoApp(int documentID)
        {
            try
            {
                this.CreateService(typeof(IAppService));
                var result = this.AppService.ConsultarProgreso(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }

        #endregion

        #region Alarmas

        /// <summary>
        /// Dice si un usuario tiene alarmas pendientes
        /// </summary>
        /// <returns>Devuelve verdadero si el usuario tiene alarmas</returns>
        public bool Alarmas_HasAlarms(string userName)
        {
            try
            {
                this.CreateService(typeof(IAppService));
                var result = this.AppService.Alarmas_HasAlarms(this._userChannel, userName);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae el listado de tareas pendientes para envio de correos
        /// </summary>
        /// <returns>Retorna el listado de tareas pendientes</returns>
        public IEnumerable<DTO_Alarma> Alarmas_GetAll(string userName = null)
        {
            try
            {
                this.CreateService(typeof(IAppService));
                var result = this.AppService.Alarmas_GetAll(this._userChannel, userName);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }

        #endregion

        #region aplBitacora

        /// <summary>
        /// Consulta la bitácora dado un filtro
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns></returns>
        public IEnumerable<DTO_aplBitacora> aplBitacoraGetFiltered(DTO_glConsulta consulta)
        {
            try
            {
                this.CreateService(typeof(IAppService));
                var res = this.AppService.aplBitacoraGetFiltered(this._userChannel, consulta);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }

        /// <summary>
        /// Consulta la bitácora dado un filtro y devuelve la cantidad de resultados
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns></returns>
        public long aplBitacoraCountFiltered(DTO_glConsulta consulta)
        {
            try
            {
                this.CreateService(typeof(IAppService));
                var res = this.AppService.aplBitacoraCountFiltered(this._userChannel, consulta);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));

                throw ex;
            }
        }

        /// <summary>
        /// Consulta la bitácora dado un filtro por páginas
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns></returns>
        public IEnumerable<DTO_aplBitacora> aplBitacoraGetFilteredPaged(int pageSize, int pageNum, DTO_glConsulta consulta)
        {
            try
            {
                this.CreateService(typeof(IAppService));
                var res = this.AppService.aplBitacoraGetFilteredPaged(this._userChannel, pageSize, pageNum, consulta);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }

        #endregion

        #region aplIdioma

        /// <summary>
        /// Trae todos los Idiomas
        /// </summary>
        /// <returns>Devuelve los Idiomas</returns>
        public IEnumerable<DTO_aplIdioma> aplIdioma_GetAll()
        {
            try
            {
                this.CreateService(typeof(IAppService));
                var result = this.AppService.aplIdioma_GetAll(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }

        #endregion

        #region aplIdiomaTraduccion

        /// <summary>
        /// Trae la versión de un idioma
        /// </summary>
        /// <returns>Devuelve la versión de un Idioma</returns>
        public IEnumerable<DTO_aplIdiomaTraduccion> aplIdiomaTraduccion_GetResources(string idiomaId)
        {
            try
            {
                this.CreateService(typeof(IAppService));
                var result = this.AppService.aplIdiomaTraduccion_GetResources(this._userChannel, idiomaId);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }

        #endregion

        #region aplMaestraPropiedad

        /// <summary>
        /// Trae toda la informacion de las maestras
        /// </summary>
        /// <returns>Devuelve la lista de maestras</returns>
        public IEnumerable<DTO_aplMaestraPropiedades> aplMaestraPropiedad_GetAll()
        {
            try
            {
                this.CreateService(typeof(IAppService));
                var result = this.AppService.aplMaestraPropiedad_GetAll(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }

        #endregion

        #region aplModulo

        /// <summary>
        /// Trae todos los Modulos visibles
        /// </summary>
        /// <returns>Devuelve los Modulos visbles</returns>
        public IEnumerable<DTO_aplModulo> aplModulo_GetByVisible(short visible, bool onlyOperative)
        {
            try
            {
                this.CreateService(typeof(IAppService));
                var result = this.AppService.aplModulo_GetByVisible(this._userChannel, visible, onlyOperative);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }

        #endregion

        #region AplReporte

        /// <summary>
        /// Obtiene un resporte predefinido para una empresa
        /// </summary>
        /// <param name="documentoID">Identificador del reporte</param>
        /// <returns>Retorna el reporte del documento</returns>
        public byte[] aplReporte_GetByID(int documentoID)
        {
            try
            {
                this.CreateService(typeof(IAppService));
                var result = this.AppService.aplReporte_GetByID(this._userChannel, documentoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }

        /// <summary>
        /// Ingresa o actualiza un reporte predefinido para un usuario
        /// </summary>
        /// <param name="documentoID">Identificador del reporte</param>
        /// <returns>Retorna el reporte del documento</returns>
        public void aplReporte_Update(DTO_aplReporte report)
        {
            try
            {
                this.CreateService(typeof(IAppService));
                this.AppService.aplReporte_Update(this._userChannel, report);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }


        #endregion

        #region Operaciones Documentos

        /// <summary>
        /// Verifica si se ha realizado algun proceso de cierre de un modulo para un oerdioo determinado
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="mod">Modulo sobre el cual se esta trabajando</param>
        /// <returns>Verdadero si el periodo ha tenido cierres</returns>
        public bool PeriodoHasCierre(DateTime periodo, ModulesPrefix mod)
        {
            try
            {
                this.CreateService(typeof(IAppService));
                var result = this.AppService.PeriodoHasCierre(this._userChannel, periodo, mod);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }

        /// <summary>
        /// Verifica si un periodo existe y el mes esta abierto
        /// </summary>
        /// <param name="empresaID">Identificador de la empresa</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="mod">Modulo sobre el cual se esta trabajando</param>
        /// <returns>Verdadero si el periodo se puede usar</returns>
        public EstadoPeriodo CheckPeriod(DateTime periodo, ModulesPrefix mod)
        {
            try
            {
                this.CreateService(typeof(IAppService));
                var result = this.AppService.CheckPeriod(this._userChannel, periodo, mod);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el prefijo de un documento dado
        /// </summary>
        /// <param name="areaFuncionalID">Codigo del area funcional</param>
        /// <param name="documentoID">Codigo del documento</param>
        /// <returns>Retorna el codigo del prefijo</returns>
        public string PrefijoDocumento_Get(string areaFuncionalID, int documentoID)
        {
            try
            {
                this.CreateService(typeof(IAppService));
                var result = this.AppService.PrefijoDocumento_Get(this._userChannel, areaFuncionalID, documentoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae el consecutivo para un numero de documento
        /// Si no existe crea uno y lo inicia en 1
        /// </summary>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="onlyGet">Indica si solo puede traer la info o tambien crear un nuevo numero</param>
        /// <returns>Retorna el consecutivo</returns>
        public int DocumentoNro_Get(int documentID, string prefijoID)
        {
            try
            {
                this.CreateService(typeof(IAppService));
                var result = this.AppService.DocumentoNro_Get(this._userChannel, documentID, prefijoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }

        #endregion

        #region Reportes

        /// <summary>
        /// Trae la data para un reporte
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="consulta"></param>
        /// <returns></returns>
        public List<DTO_BasicReport> GetReportData(int reportId, DTO_glConsulta consulta, List<ConsultasFields> fields = null)
        {
            try
            {
                this.CreateService(typeof(IAppService));
                if (fields == null)
                    fields = new List<ConsultasFields>();
                byte[] bItems = CompressedSerializer.Compress<List<ConsultasFields>>(fields);
                var result = this.AppService.GetReportData(this._userChannel, reportId, consulta, bItems);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IAppService));
                throw ex;
            }
        }

        #endregion

        #region Temporales

        /// <summary>
        /// Revisa si existe un temporal segun el origen y el usuario
        /// </summary>
        /// <param name="origen">origen relacionado</param>
        /// <param name="usuario">usuario relacionado</param>
        /// <returns>objeto temporal</returns>
        public bool aplTemporales_HasTemp(string origen, DTO_seUsuario usuario)
        {
            if (_temporales != null)
            {
                var result = this._temporales.aplTemporales_HasTemp(this._userChannel, origen, usuario);
                return result;
            }
            return false;
        }

        /// <summary>
        /// Trae el temporal de un origen determinado y luego lo borra de los temporales
        /// </summary>
        /// <param name="origen">origen relacionado</param>
        /// <param name="usuario">usuario relacionado</param>
        /// <returns>objeto temporal</returns>
        public object aplTemporales_GetByOrigen(string origen, DTO_seUsuario usuario)
        {
            object objectoTemporal = null;
            try
            {
                if (_temporales != null)
                {
                    byte[] arr = this._temporales.aplTemporales_GetByOrigen(this._userChannel, origen, usuario);
                    MemoryStream memStream = new MemoryStream();
                    BinaryFormatter binForm = new BinaryFormatter();
                    memStream.Write(arr, 0, arr.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    objectoTemporal = (Object)binForm.Deserialize(memStream);
                }
                return objectoTemporal;
            }
            catch (Exception ex)
            {
                return objectoTemporal;
            }
        }

        /// <summary>
        /// Guarda un objeto en temporales. También borra un objeto que anteriormente estuviese bajo el mismo origen para ese usuario
        /// </summary>
        /// <param name="origen">origen relacionado</param>
        /// <param name="usuario">usuario</param>
        /// <param name="objeto">objeto a guardar</param>
        public void aplTemporales_Save(string origen, DTO_seUsuario usuario, object objeto)
        {
            if (_temporales != null)
                this._temporales.aplTemporales_Save(this._userChannel, origen, usuario, objeto);
        }

        /// <summary>
        /// Elimina los temporales de un usuario
        /// </summary>
        /// <param name="origen">Origen de los datos</param>
        /// <param name="usuario">Usuario que esta buscando temporales</param>
        public void aplTemporales_Clean(string origen, DTO_seUsuario usuario)
        {
            if (_temporales != null)
                this._temporales.aplTemporales_Clean(this._userChannel, origen, usuario);
        }

        #endregion

        #endregion

        #region MasterService

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción de un usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ConsultarProgresoMaster(int documentID)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var result = this.MasterService.ConsultarProgreso(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        #endregion

        #region Maestras

        #region MasterComplex - Llaves multiples

        /// <summary>
        /// Retorna una maestra básica a partir del identificador
        /// </summary>
        /// <param name="pks">Identificador de la maestra</param>
        /// <param name="EmpresaGrupoID">Identificador por el cual se filtra</param>
        /// <returns>Devuelve la maestra basica</returns>
        public DTO_MasterComplex MasterComplex_GetByID(int documentID, Dictionary<string, string> pks, bool active)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.masterLocalService.MasterComplex_GetByID(this._userChannel, documentID, pks, active);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Cantidad de registros de una maestra
        /// </summary>
        /// <param name="consulta">Filtro</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Retorna el numero de registros de la consulta</returns>
        public long MasterComplex_Count(int documentID, DTO_glConsulta consulta, bool? active)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.masterLocalService.MasterComplex_Count(this._userChannel, documentID, consulta, active);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae los registros de una maestra compleja
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de página</param>
        /// <param name="consulta">Filtro</param>
        /// <returns>Devuelve los registros de una maestra compleja</returns>
        public IEnumerable<DTO_MasterComplex> MasterComplex_GetPaged(int documentID, long pageSize, int pageNum, DTO_glConsulta consulta, bool? active)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.masterLocalService.MasterComplex_GetPaged(this._userChannel, documentID, pageSize, pageNum, consulta, active);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Adiciona una lista a la maestra
        /// </summary>
        /// <param name="bItems">Lista de registros</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="progress">Progreso de insercion de los datos</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult MasterComplex_Add(int documentID, byte[] bItems, int accion)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.MasterService.MasterComplex_Add(this._userChannel, documentID, bItems, accion);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza una maestra
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult MasterComplex_Update(int documentID, DTO_MasterComplex item)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.MasterService.MasterComplex_Update(this._userChannel, documentID, item);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Borra una maestra a partir de su id
        /// </summary>
        /// <param name="pks">Llaves primarias de la maestra</param>
        /// <returns>Devuelve el resultado de la operacion</returns>  
        public DTO_TxResult MasterComplex_Delete(int documentID, Dictionary<string, string> pks)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.MasterService.MasterComplex_Delete(this._userChannel, documentID, pks);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Exporta los registros de una maestra
        /// </summary>
        /// <param name="colsRsx">Nombres de las columnas con los recursos</param>
        /// <param name="consulta">Filtro</param>
        /// <param name="filtrosExtra">Filtro especial</param>
        /// <returns>Retorna el nombre del archivo</returns>
        public string MasterComplex_Export(int documentID, string colsRsx, DTO_glConsulta consulta)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.MasterService.MasterComplex_Export(this._userChannel, documentID, colsRsx, consulta);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae el numero de la fila de una lista de Pks
        /// </summary>
        /// <param name="pks">Idenfiticador de la maestra</param>
        /// <param name="consulta">Filtro</param>
        /// <param name="active">Indicador de activo</param>
        /// <returns>Devuelve el número de fila ordenando por id</returns>
        public long MasterComplex_Rownumber(int documentID, Dictionary<string, string> pks, DTO_glConsulta consulta, bool? active)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.masterLocalService.MasterComplex_Rownumber(this._userChannel, documentID, pks, consulta, active);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        #endregion

        #region MasterHierarchy

        #region Funciones que llaman a MasterSimple

        /// <summary>
        /// Trae los registros de una maestra 
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de página</param>
        /// <param name="consulta">Filtros</param>
        /// <param name="FiltrosExtra">Filtros de codigo y descripción</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Devuelve los registros de una maestra</returns>
        public IEnumerable<DTO_MasterHierarchyBasic> MasterHierarchy_GetPaged(int documentID, long pageSize, int pageNum, DTO_glConsulta consulta, List<DTO_glConsultaFiltro> FiltrosExtra, bool? active)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.masterLocalService.MasterHierarchy_GetPaged(this._userChannel, documentID, pageSize, pageNum, consulta, FiltrosExtra, active);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        #endregion

        #region Funciones Maestra jerarquica

        /// <summary>
        /// Retorna una maestra básica a partir del identificador
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="id">Identificador de la maestra</param>
        /// <param name="active">indica si el registro debe estar activo</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Devuelve la maestra jerarquica</returns>
        public DTO_MasterHierarchyBasic MasterHierarchy_GetByID(int documentID, UDT_BasicID id, bool active, List<DTO_glConsultaFiltro> filtros = null)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.masterLocalService.MasterHierarchy_GetByID(this._userChannel, documentID, id, active, filtros);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la cantidad de hijos de un padre determinado
        /// </summary>
        /// <param name="parentId">id del padre</param>
        /// <param name="idFilter">filtro de id para los hijos</param>
        /// <param name="descFilter">filtro de descripcion</param>
        /// <param name="active">filtro de activo</param>
        /// <returns>Retorna la cantidad de hijos</returns>
        public long MasterHierarchy_CountChildren(int documentID, UDT_BasicID parentId, string idFilter, string descFilter, bool? active, List<DTO_glConsultaFiltro> filtros)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.masterLocalService.MasterHierarchy_CountChildren(this._userChannel, documentID, parentId, idFilter, descFilter, active, filtros);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna una lista de hijos de una maestra de jerarquica
        /// </summary>
        /// <param name="pageSize">Número de registros por página</param>
        /// <param name="pageNum">Número de página</param>
        /// <param name="orderDirection">Ordenamiento</param>
        /// <param name="parentId">Identificador del padre</param>
        /// <param name="idFilter">Filtro para la columna del ID</param>
        /// <param name="descFilter">Filtro para la columna de la descripción</param>
        /// <param name="active">Indicador si se pueden traer solo datos activos</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Retorna la lista de resultados</returns>
        public IEnumerable<DTO_MasterBasic> MasterHierarchy_GetChindrenPaged(int documentID, int pageSize, int pageNum, OrderDirection orderDirection, UDT_BasicID parentId, string idFilter, string descFilter, bool? active, List<DTO_glConsultaFiltro> filtros = null)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.masterLocalService.MasterHierarchy_GetChindrenPaged(this._userChannel, documentID, pageSize, pageNum, orderDirection, parentId, idFilter, descFilter, active, filtros);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Adiciona una lista de dtos
        /// </summary>
        /// <param name="bItems">Lista de datos</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="progress">Información con el progreso de la transacción</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult MasterHierarchy_Add(int documentID, byte[] bItems, int accion)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.MasterService.MasterHierarchy_Add(this._userChannel, documentID, bItems, accion);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza un registro de una maestra jerarquica
        /// </summary>
        /// <param name="item">Registro para actualizar</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult MasterHierarchy_Update(int documentID, DTO_MasterHierarchyBasic item)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.MasterService.MasterHierarchy_Update(this._userChannel, documentID, item);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Borra un registro
        /// </summary>
        /// <param name="id">Identificador del registro</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult MasterHierarchy_Delete(int documentID, UDT_BasicID id)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.MasterService.MasterHierarchy_Delete(this._userChannel, documentID, id);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Valida si los padres de un id existen
        /// </summary>
        /// <param name="documentID">Identificador de la maestra</param>
        /// <param name="id">id del hijo</param>
        /// <returns>True si existen los padres</returns>
        public bool MasterHierarchy_CheckParents(int documentID, UDT_BasicID id)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.masterLocalService.MasterHierarchy_CheckParents(this._userChannel, documentID, id);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region MasterSimple

        /// <summary>
        /// <summary>
        /// Retorna una maestra básica a partir del identificador
        /// </summary>
        /// <param name="id">Identificador de la maestra</param>
        /// <param name="active">indica si el registro debe estar activo</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Devuelve la maestra basica</returns>
        public DTO_MasterBasic MasterSimple_GetByID(int documentID, UDT_BasicID id, bool active, List<DTO_glConsultaFiltro> filtros = null)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var result = this.masterLocalService.MasterSimple_GetByID(this._userChannel, documentID, id, active, filtros);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna una maestra básica a partir de su descripcion
        /// En caso de encontrar mas de un resultado devolvera el primero
        /// </summary>
        /// <param name="desc">Descriptivo de la maestra</param>
        /// <returns>Devuelve la maestra basica</returns>
        public DTO_MasterBasic MasterSimple_GetByDesc(int documentID, string desc)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var result = this.masterLocalService.MasterSimple_GetByDesc(this._userChannel, documentID, desc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Cuenta la cantidad de resultados dado un filtro
        /// </summary>
        /// <param name="consulta">Filtro</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns></returns>
        public long MasterSimple_Count(int documentID, DTO_glConsulta consulta, List<DTO_glConsultaFiltro> FiltrosExtra, bool? active)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.masterLocalService.MasterSimple_Count(this._userChannel, documentID, consulta, FiltrosExtra, active);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae los registros de una maestra 
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de página</param>
        /// <param name="consulta">Filtros</param>
        /// <param name="FiltrosExtra">Filtros de codigo y descripción</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Devuelve los registros de una maestra</returns>
        public IEnumerable<DTO_MasterBasic> MasterSimple_GetPaged(int documentID, long pageSize, int pageNum, DTO_glConsulta consulta, List<DTO_glConsultaFiltro> FiltrosExtra, bool? active)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var result = this.masterLocalService.MasterSimple_GetPaged(this._userChannel, documentID, pageSize, pageNum, consulta, FiltrosExtra, active);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        ///  Adiciona una lista a la maestra básica
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <param name="bItems">Lista de registros</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="progress">Progreso de la operacion "usuario,progreso"</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult MasterSimple_Add(int documentID, byte[] bItems, int accion)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var result = this.MasterService.MasterSimple_Add(this._userChannel, documentID, bItems, accion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza una maestra básica
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult MasterSimple_Update(int documentID, DTO_MasterBasic item)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var result = this.MasterService.MasterSimple_Update(this._userChannel, documentID, item);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Borra una maestra básica a partir de su id
        /// </summary>
        /// <param name="id">Identificador de la maestra</param>
        /// <returns>Resultado</returns>  
        public DTO_TxResult MasterSimple_Delete(int documentID, UDT_BasicID id)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.MasterService.MasterSimple_Delete(this._userChannel, documentID, id);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Exporta los registros de una maestra
        /// </summary>
        /// <param name="colsRsx">Nombres de las columnas con los recursos</param>
        /// <param name="consulta">Filtro</param>
        /// <param name="filtrosExtra">Filtro especial</param>
        /// <returns>Retorna el nombre del archivo</returns>
        public string MasterSimple_Export(int documentID, string colsRsx, DTO_glConsulta consulta, List<DTO_glConsultaFiltro> filtrosExtra)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.MasterService.MasterSimple_Export(this._userChannel, documentID, colsRsx, consulta, filtrosExtra);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae el numero de la fila de un ID
        /// </summary>
        /// <param name="id">Idenfiticador de la maestra</param>
        /// <param name="consulta">Filtro</param>
        /// <param name="active">Indicador de activo</param>
        /// <returns>Devuelve el número de fila ordenando por id</returns>
        public long MasterSimple_Rownumber(int documentID, UDT_BasicID id, DTO_glConsulta consulta, bool? active)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.masterLocalService.MasterSimple_Rownumber(this._userChannel, documentID, id, consulta, active);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        #endregion

        #region Masters Common

        /// <summary>
        /// Metodo de TODAS las maestras para traer un campo de imagen
        /// </summary>
        /// <param name="replicaId">Replica Id de la fila a buscar</param>
        /// <param name="fieldName">Nombre del campo que contiene la imagen</param>
        /// <returns>arreglo de bytes con la imagen del logo</returns>
        public byte[] Master_GetImage(int docId, int replicaId, string fieldName)
        {
            try
            {
                this.CreateService(typeof(IMasterService));
                var res = this.masterLocalService.Master_GetImage(this._userChannel, docId, replicaId, fieldName);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IMasterService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #endregion

        #region ActivosService

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción de un usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ConsultarProgresoActivos(int documentID)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.ConsultarProgreso(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        #endregion

        #region Procesos

        /// <summary>
        /// Genera la depreciacion de los activos
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <param name="periodo">Periodo de cierre</param>
        public DTO_TxResult Proceso_GenerarDepreciacionActivos(int documentID, DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.Proceso_GenerarDepreciacionActivos(this._userChannel, documentID, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera un Reproceso de la Depreciación por Unidades de Producción
        /// </summary>
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <param name="fechaIni">fechaIni</param>
        /// <param name="fechaFinal">fechaFinal</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_TxResult> Proceso_ReProcesoDepreciacion(int documentID, DateTime fechaIni, DateTime fechaFinal)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.Proceso_ReProcesoDepreciacion(this._userChannel, documentID, fechaIni, fechaFinal);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        #endregion

        #region acActivoControl

        /// <summary>
        /// Trae los activos fijos para la Empresa actual
        /// </summary>
        /// <returns></returns>
        public List<DTO_acActivoControl> acActivoControl_Get()
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_Get(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene los activos segun los filtros dados
        /// </summary>
        /// <param name="serialID">identificador de serial</param>
        /// <param name="PlaquetaID">identificador de plaqueta</param>
        /// <param name="locFisicaID">identificador de localizacion fisica</param>
        /// <param name="referenciaID">identifiador de referencia</param>
        /// <param name="isContenedor">indica si es contendor</param>
        /// <returns>listado de Activos</returns>
        public List<DTO_acActivoControl> acActivoControl_GetFilters(string serialID,
                                                                        string PlaquetaID,
                                                                        string locFisicaID,
                                                                        string referenciaID,
                                                                        string centroCosto,
                                                                        string proyecto,
                                                                        string clase,
                                                                        string tipo,
                                                                        string grupo,
                                                                        string responsable,
                                                                        bool isContenedor,
                                                                        int pageSize,
                                                                        int pageNum)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_GetFilters(this._userChannel,
                    serialID,
                    PlaquetaID,
                    locFisicaID,
                    referenciaID,
                    centroCosto,
                    proyecto,
                    clase,
                    tipo,
                    grupo,
                    responsable,
                    isContenedor, pageSize, pageNum);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }

        }


        /// <summary>
        /// Obtiene el numero de activos segun los filtros dados
        /// </summary>
        /// <param name="serialID">identificador de serial</param>
        /// <param name="PlaquetaID">identificador de plaqueta</param>
        /// <param name="locFisicaID">identificador de localizacion fisica</param>
        /// <param name="referenciaID">identifiador de referencia</param>
        /// <param name="centroCosto">identificador centro de costo</param>
        /// <param name="proyecto">identificador de proyecto</param>
        /// <param name="clase">identificador de clase</param>
        /// <param name="tipo">identificador de tipo</param>
        /// <param name="grupo">identificador de grupo</param>
        /// <param name="responsable">resposable</param>
        /// <param name="isContenedor">indica si es contendor</param>
        /// <returns>listado de Activos</returns>
        public int acActivoControl_GetFiltersCount(string serialID,
                                                        string PlaquetaID,
                                                        string locFisicaID,
                                                        string referenciaID,
                                                        string centroCosto,
                                                        string proyecto,
                                                        string clase,
                                                        string tipo,
                                                        string grupo,
                                                        string responsable,
                                                        bool isContenedor
                                                       )
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_GetFiltersCount(this._userChannel,
                    serialID,
                    PlaquetaID,
                    locFisicaID,
                    referenciaID,
                    centroCosto,
                    proyecto,
                    clase,
                    tipo,
                    grupo,
                    responsable,
                    isContenedor);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un activo control por segun la llave primaria
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        public DTO_acActivoControl acActivoControl_GetByID(int activoID)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_GetByID(this._userChannel, activoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un activo control
        /// </summary>
        /// <param name="plaqueta">Plaqueta</param>
        /// <returns>Retorna un activo</returns>
        public DTO_acActivoControl acActivoControl_GetByPlaqueta(string plaquetaID)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_GetByPlaqueta(this._userChannel, plaquetaID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega un registro al control de documentos
        /// </summary>
        public DTO_TxResultDetail acActivoControl_Add(int documentID, DTO_acActivoControl acCtrl, string documento6ID, bool updateRecibido)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_Add(this._userChannel, documentID, acCtrl, documento6ID, false);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega varios registros al control de activos
        /// </summary>
        public List<DTO_TxResult> acActivoControl_AddList(int documentoID, string actividadFlujoID, string prefijo, List<DTO_acActivoControl> docCtrls, string tipoMvto, decimal tasaCambio)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_AddList(this._userChannel, documentoID, actividadFlujoID, prefijo, tipoMvto, docCtrls, tasaCambio);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega varios registros al control de activos
        /// </summary>
        public List<DTO_TxResult> acActivoControl_AddListActivos(int documentoID, int activoID, string actividadFlujoID, string prefijo, List<DTO_acActivoControl> docCtrls, string tipoMvto, decimal tasaCambio)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_AddListActivos(this._userChannel, documentoID, activoID, actividadFlujoID, prefijo, tipoMvto, docCtrls, tasaCambio);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza varios registros en el acActivoControl
        /// </summary>       
        public List<DTO_TxResult> acActivoControl_Update(List<DTO_acActivoControl> acCtrl, string tipoMvto, int activoID)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_Update(this._userChannel, acCtrl, tipoMvto, activoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// tare uno o  varios registros del acActivoControl de acuerdo al parametro.
        /// </summary>       
        public List<DTO_acActivoControl> acActivoControl_GetBy_Parameter(DTO_acActivoControl acCtrl)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_GetBy_Parameter(this._userChannel, acCtrl);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// tare uno o  varios registros del acActivoControl de acuerdo al parametro y tipo de movimiento.
        /// </summary>       
        public List<DTO_acActivoControl> acActivoControl_GetByParameterForTranfer(DTO_acActivoControl acCtrl)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_GetByParameterForTranfer(this._userChannel, acCtrl);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza la plaqueta.
        /// </summary>
        /// <param name="acActivoControl">Dto_acActivoControl</param>
        /// <param name="tipoMvto">Tipo de movimiento</param>
        /// <param name="activoID">ID del Activo</param>
        /// <param name="prefijo">Prefijo q viene desde la pantalla</param>
        /// <returns>objeto de resultados</returns>
        public DTO_TxResult acActivoControl_PlaquetaUpdate(List<DTO_acActivoControl> acActivoControl, string tipoMvto, int activoID, string prefijo)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_UpdatePlaqueta(this._userChannel, acActivoControl, tipoMvto, activoID, prefijo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        #endregion

        #region Alta de Activos

        /// <summary>
        /// Obtiene la lista de activos recibidos por numero de Factura
        /// </summary>
        /// <param name="numeroDoc">Numero de factura</param>
        /// <returns>Lista de activos</returns>
        public List<DTO_acActivoControl> AltaActivos_GetActivosByNumDoc(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.AltaActivos_GetActivosByNumDoc(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        #endregion

        #region Traslado de Activos

        /// <summary>
        /// Actualiza varios registros en el acActivoControl de auerdo al tipo de movimiento
        /// </summary>       
        public List<DTO_TxResult> acActivoControl_TrasladoActivos(int documentoID, List<DTO_acActivoControl> acCtrl, int activoID, string prefijoID, string tipoMvto, DateTime fechaDocu)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_TrasladoActivos(this._userChannel, documentoID, acCtrl, activoID, prefijoID, tipoMvto, fechaDocu);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        #endregion

        #region SaldosActivos


        /// <summary>
        /// Obtiene una lista de saldos de un activo
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="identificadorTR">ActivoID</param>
        /// <param name="periodo">Periodo en el q se encuetra</param>
        /// <returns>Lista de saldos de activos</returns>
        public List<DTO_acActivoSaldo> acCargarSaldos(int identificadorTR, DateTime periodo, string clase)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_CargarSaldos(this._userChannel, identificadorTR, periodo, clase);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Carga una lista de  dto_ActivoControl con los saldos por meses de acuerdo al año del periodo
        /// </summary>
        /// <param name="periodo">Periodo de busqueda</param>
        /// <param name="identificadorTR">ActivoID</param>
        /// <returns>Lista de saldos de un activo</returns>
        /// <param name="mLocal">bool si es true es consulta por moneda local de lo contrario es consulta por moneda extranjera</param>
        public List<DTO_acActivoSaldo> acCargarSaldos_Meses(string año, int identificadorTR, string activoClaseID, bool mLocal)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_CargarSaldos_Meses(this._userChannel, año, identificadorTR, activoClaseID, mLocal);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae los movimientos por componente de cada activo 
        /// </summary>
        /// <param name="identificadorTR">ActivoID</param>
        /// <param name="periodo">Periodo actual</param>
        /// <param name="clase">Clase de activo</param>
        /// <returns>Lista de DTO_activoSaldo</returns>
        public List<DTO_acActivoSaldo> acCargarMvtos(int identificadorTR, DateTime periodo, string clase)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_CargarMvtos(this._userChannel, identificadorTR, periodo, clase);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que trae el comprobante de acuerdo al numeroDoc y el idTR
        /// </summary>
        /// <param name="numeroDoc">Numero del documento</param>
        /// <param name="identTR">IdentificadorTR</param>
        /// <returns>Retorna una lista de comprobanteFooter</returns>
        public List<DTO_ComprobanteFooter> acActivoControl_GetByIdentificadorTR(int numeroDoc, int identTR)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_GetByIdentificadorTR(this._userChannel, numeroDoc, identTR);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion para actualiza saldos correspondiente a activos.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="documentoID">int id del documento</param>
        /// <param name="prefijoID">string prefijo del modlo</param>
        /// <param name="tipoMvto">tipo de mov q viene de la pantalla e identifica la transaccion</param>
        /// <param name="dto_activoSaldo">lista de saldos a actualizar</param>
        /// <param name="acActivoCtrl">lista de activos a los cuales se les actualiza los saldos</param>
        /// <returns>lista de resultados</returns>
        public List<DTO_TxResult> acActivoControl_UpdateSaldos(int documentoID, string actividadFlujoID, string prefijoID, string tipoMvto, List<DTO_acActivoSaldo> dto_activoSaldo, List<DTO_acActivoControl> acActivoCtrl)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_UpdateSaldos(this._userChannel, documentoID, actividadFlujoID, prefijoID, tipoMvto, dto_activoSaldo, acActivoCtrl);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae los compponentes de acContabiliza de acuerdo a la clase del activo
        /// </summary>
        /// <param name="clase">Activo clase </param>
        /// <returns>Lista de activosaldo</returns>
        public List<DTO_acActivoSaldo> acActivoControl_GetComponentesPorClaseActivoID(string clase)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_GetComponentesPorClaseActivoID(this._userChannel, clase);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que trae una lista de saldos por componente
        /// </summary>
        /// <param name="periodo">preiodo del modulo</param>
        /// <param name="componentes">Lista con info del componetne</param>
        /// <param name="concSaldo">Compnente</param>
        /// <param name="identificadorTR">Id del activo</param>
        /// <param name="activoClaseID">Clase del activoID</param>
        /// <returns>Lista de activosaldo</returns>
        public List<DTO_acActivoSaldo> acActivoControl_GetSaldoXComponente(DateTime periodo, List<DTO_acActivoSaldo> componentes, int identificadorTR, string activoClaseID)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_GetSaldoXComponente(this._userChannel, periodo, componentes, identificadorTR, activoClaseID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        #endregion

        #region Retiro Activos

        /// <summary>
        /// Listado de Componentes y sus saldos por activo
        /// </summary>
        /// <param name="activoID">identificador del activo</param>
        /// <returns></returns>
        public List<DTO_acRetiroActivoComponente> RetiroActivos_GetComponenentes(int activoID, string tipoBalance)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.RetiroActivos_GetComponenentes(this._userChannel, activoID, tipoBalance);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Listado de Componentes y sus saldos por activo
        /// </summary>
        /// <param name="activoID">identificador del activo</param>
        /// <returns></returns>
        public List<DTO_acRetiroActivoComponente> acActivoFijos_GetComponenentes(int activoID)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoFijos_GetComponenentes(this._userChannel, activoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }


        /// <summary>
        /// Da de baja un activo
        /// </summary>
        /// <param name="documentoID">Identificador Documento</param>
        /// <param name="actividadFlujoID">Identificador Actividad</param>
        /// <param name="prefijoID">Prefijo</param>
        /// <param name="acActivoControlList">Lista activos</param>
        /// <param name="tipoMvto">Tipo Movimiento</param>
        /// <param name="batchProgress">Indicador de Progreso</param>
        /// <param name="insideAnotherTx">Indica si viene de una Transacción</param>
        /// <returns></returns>
        public List<DTO_TxResult> acActivoControl_RetiroActivos(int documentoID, string actividadFlujoID, string prefijoID, List<DTO_acActivoControl> acActivoControlList, string tipoMvto)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_RetiroActivos(this._userChannel, documentoID, actividadFlujoID, prefijoID, acActivoControlList, tipoMvto);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        #endregion



        #region Deterioro Activos

        /// <summary>
        /// Trae un saldo
        /// </summary>
        /// <param name="tipoBalance">Tipo de balance</param>
        /// <param name="concSaldo">Concepto de saldo</param>
        /// <param name="identificadorTR">Consecutivo del socumento por el cual se va a buscar el saldo</param>
        /// <returns>Retorna un listado de saldo</returns>
        public List<DTO_acActivoSaldo> acActivoControl_GetSaldoCompraActivo(string concSaldo, int identificadorTR, string tipoBalance)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_GetSaldoCompraActivo(this._userChannel, concSaldo, identificadorTR, tipoBalance);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Crear el comprobante de Deterioro o Revalorización de un listado de Activos
        /// </summary>
        /// <param name="documentoID">documento generador</param>
        /// <param name="actividadFlujoID">actividad Flujo</param>
        /// <param name="balanceID">identificador del balance</param>
        /// <param name="prefijoID">prefijo documento</param>
        /// <param name="acActivoControlList">listado de activos</param>
        /// <param name="batchProgress">barra de progreso</param>
        /// <param name="insideAnotherTx">determina si procede de una transacción</param>
        /// <returns>lista de resultados</returns>
        public List<DTO_TxResult> acActivoControl_Deterioro(int documentoID, string actividadFlujoID, string balanceID, string prefijoID, bool deterioroInd, string tipoMov, DTO_coDocumentoRevelacion revelacion, List<DTO_acActivoControl> acActivoControlList)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_Deterioro(this._userChannel, documentoID, actividadFlujoID, balanceID, prefijoID, deterioroInd, tipoMov, revelacion, acActivoControlList);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }

        }

        #endregion

        #region Contenedores

        /// <summary>
        /// Obtiene los activos hijos 
        /// </summary>
        /// <param name="activoID">activoID padre</param>
        /// <returns>listado de activos control</returns>
        public List<DTO_acActivoControl> acActivoControl_GetChildrenActivos(int activoID)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_GetChildrenActivos(this._userChannel, activoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }


        #endregion

        #region acActivoGarantia

        /// <summary>
        /// Trae lista activo garantia para plantilla de importacion
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        public List<DTO_acActivoGarantia> acActivoGarantia_GetForImport()
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoGarantia_GetForImport(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega una lista de activo garantia
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        public DTO_TxResult acActivoGarantia_Add(int documentoID,List<DTO_acActivoGarantia> acGar)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoGarantia_Add(this._userChannel,documentoID,acGar);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        #endregion

        #region Movimientos Activos

        /// <summary>
        /// Movimientos Activos 
        /// </summary>
        /// <param name="documentoID">documentoID</param>
        /// <param name="activoID">activoID</param>
        /// <param name="actividadFlujoID">actividadFlujoID</param>
        /// <param name="prefijoID">prefijoID</param>
        /// <param name="acActivoControlList">Listado de Activos</param>
        /// <param name="tipoMvto">Tipo Movimiento</param>
        /// <param name="proyectoID">proyectoID</param>
        /// <param name="centroCostoID">centroCostoID</param>
        /// <param name="locFisicaID">locFisicaID</param>
        /// <param name="responsable">responsable</param>
        /// <param name="batchProgress">indicador de progreso</param>
        /// <param name="insideAnotherTx">indica si viene de una transaccion</param>
        /// <returns>lista de resultados</returns>
        public List<DTO_TxResult> acActivoControl_MovActivos(int documentoID, string actividadFlujoID, string prefijoID, List<DTO_acActivoControl> acActivoControlList, string tipoMvto,
                                                             string proyectoID, string centroCostoID, string locFisicaID, string responsable)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.acActivoControl_MovActivos(this._userChannel, documentoID, actividadFlujoID, prefijoID, acActivoControlList, tipoMvto, proyectoID, centroCostoID, locFisicaID, responsable);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Funcion que obtiene la información del activo de acuerdo al filtro
        /// </summary>
        /// <param name="plaqueta">Filtro de PLaquetaID</param>
        /// <param name="serial">Filtro de SerialID</param>
        /// <param name="referencia">Filtro de Referencia</param>
        /// <param name="clase">Filtro de ActivoClaseID</param>
        /// <param name="tipo">Filtro de ActivoTipoID</param>
        /// <param name="grupo">Filtro de ActivoGrupoID</param>
        /// <param name="locFisica">Filtro de LocfisicaIF</param>
        /// <param name="contenedor">Bool que dice si trae o no los activos contenidos</param>
        /// <param name="responsable">Filtro de Responsable - TerceroID</param>
        /// <returns>Lista de detalles para la consulta</returns>
        public List<DTO_acQueryActivoControl> ActivoGetByParameter(string plaqueta, string serial, string referencia, string clase, string tipo, string grupo, string locFisica, bool contenedor, string responsable)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.ActivoGetByParameter(this._userChannel, plaqueta, serial, referencia, clase, tipo, grupo, locFisica, contenedor, responsable);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcionque obrtinee los saldos correspondientes al activo
        /// </summary>
        /// <param name="identificadorTr">Activo ID del activo</param>
        /// <param name="balanceTipoID">Tipo de Libro (Funcional o IFRS)</param>
        /// <param name="año">Año que se va a consultar</param>
        /// <param name="mes">Mes que se quiere consultar</param>
        /// <returns>Lista de saldos por activo</returns>
        public List<DTO_acActivoQuerySaldos> ActivoControl_GetSaldosByMesYLibro(int identificadorTr, string balanceTipoID, DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(IActivosService));
                var result = this.ActivosService.ActivoControl_GetSaldosByMesYLibro(this._userChannel, identificadorTr, balanceTipoID, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IActivosService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region CarteraService

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción de un usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ConsultarProgresoCartera(int documentID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ConsultarProgreso(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Cartera Corporativa

        #region Cierres

        /// <summary>
        /// Realiza el proceso de cierre diario
        /// </summary>
        public DTO_TxResult Cartera_CerrarDia(DateTime fechaCierre)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Cartera_CerrarDia(this._userChannel, fechaCierre);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Carga la información para hacer un cierre diario
        /// </summary>
        /// <param name="fecha">Fecha de cierre</param>
        /// <param name="balanceFunc">Balance funcional</param>
        /// <returns></returns>
        public List<DTO_ccCierreDia> ccCierreDia_GetAll(DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ccCierreDia_GetAll(this._userChannel, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Carga la información para hacer un cierre diario
        /// </summary>
        /// <param name="fecha">Fecha de cierre</param>
        /// <param name="balanceFunc">Balance funcional</param>
        /// <returns></returns>
        public List<DTO_ccCierreMes> ccCierreMes_GetAll(Int16 año)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ccCierreMes_GetAll(this._userChannel, año);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Carga todos los cierres mes con uno o varios filtros
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns>Lista de CierresCartera</returns>
        public List<DTO_ccCierreMesCartera> ccCierreMesCartera_GetByParameter(DTO_ccCierreMesCartera filter)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ccCierreMesCartera_GetByParameter(this._userChannel, filter);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Realiza el proceso de cierre mensual
        /// </summary>
        public DTO_TxResult Proceso_CierreMesCartera(int documentID, DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Proceso_CierreMesCartera(this._userChannel, documentID, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Carga la información para realizar listado de Centrales de Riesgo
        /// </summary>
        /// <param name="periodo">Periodo a consultar</param>
        /// <returns>Listado de Cierres</returns>
        public List<DTO_CentralRiesgoMes> ccCierreMesCartera_GetCierreCentralRiesgoMes(DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ccCierreMesCartera_GetCierreCentralRiesgoMes(this._userChannel, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Migraciones

        /// <summary>
        /// Valida que la información basica de la migracion nomina
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="centroPagoID">Identificador del centro de pago</param>
        /// <param name="numDoc">Identificador del documento control</param>
        /// <param name="pagaduriaID">Identificador de la pagaduria</param>
        /// <param name="docTercero">Documento Tercero</param>
        /// <param name="isMensual">Indica si la consulta es quincenal o mensual</param>
        /// <param name="fechaIni">Fecha inicial de la migracion</param>
        /// <param name="fechaFin">Fecha final de la migracion</param>
        /// <param name="data">Información a migrar</param>
        /// <returns></returns>
        public DTO_TxResult RecaudosMasivos_Validar(int documentID, DateTime periodo, string pagaduriaID, DateTime fechaAplica,
            ref List<DTO_ccIncorporacionDeta> data)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RecaudosMasivos_Validar(this._userChannel, documentID, periodo, pagaduriaID, fechaAplica, ref data);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Procesa la migracion de nomina
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="numDoc">Identificador unico del documento</param>
        /// <param name="centroPagoID">Identificador del centro de pago</param>
        /// <param name="pagaduriaID">Identificador de la pagaduria</param>
        /// <param name="docTercero">Identificador del documento tercero</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="isMensual">Indica si la consulta es quincenal o mensual</param>
        /// <param name="fecha">Fecha de la migracion (15 o ultimo dia del mes)</param>
        /// <param name="data">Información a migrar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult RecaudosMasivos_Procesar(int documentID, string centroPagoID, string pagaduriaID, DateTime periodo, DateTime fecha,
            List<DTO_ccIncorporacionDeta> data)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RecaudosMasivos_Procesar(this._userChannel, documentID, centroPagoID, pagaduriaID, periodo, fecha, data);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Paga los registros de la migracion de nomina
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="actFlujoID">Identificador de la actividad de flujo</param>
        /// <param name="data">Datos a migrar</param>
        /// <returns>Retorna la lista de resultados</returns>
        public DTO_TxResult RecaudosMasivos_Pagar(int documentID, string actFlujoID, DateTime periodo, DateTime fecha, DateTime fechaAplica,
            decimal valorPagaduria, string centroPagoID, DTO_tsBancosCuenta banco, List<DTO_ccIncorporacionDeta> data, List<DTO_ccIncorporacionDeta> exclusiones)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RecaudosMasivos_Pagar(this._userChannel, documentID, actFlujoID, periodo, fecha, fechaAplica,
                    valorPagaduria, centroPagoID, banco, data,exclusiones);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Proceso de migracion de cartera
        /// </summary>
        /// <param name="documentID">Identificador del documento que inicia el proceso</param>
        /// <param name="periodo">Periodo de ingreso de datos</param>
        /// <param name="compComodinID">Identificador del componente comodin</param>
        /// <param name="data">Lista de creditos a migrar</param>
        /// <returns>Retorna al resultado de la operacion</returns>
        public List<DTO_TxResult> MigracionCartera(int documentID, DateTime periodo, string compComodinID, List<DTO_MigracionCartera> data)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.MigracionCartera(this._userChannel, documentID, periodo, compComodinID, data);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Proceso de migracion de cartera
        /// </summary>
        /// <param name="documentID">Identificador del documento que inicia el proceso</param>
        /// <param name="data">Lista de creditos a migrar</param>
        /// <returns>Retorna al resultado de la operacion</returns>
        public List<DTO_TxResult> MigracionEstadoCartera(int documentID, List<DTO_MigracionEstadoCartera> data)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.MigracionEstadoCartera(this._userChannel, documentID, data);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Proceso de migracion de cartera
        /// </summary>
        /// <param name="documentID">Identificador del documento que inicia el proceso</param>
        /// <param name="periodo">Periodo de ingreso de datos</param>
        /// <param name="pagaduria">pagaduria</param>
        /// <param name="data">Lista de creditos a migrar</param>
        /// <returns>Retorna al resultado de la operacion</returns>
        public List<DTO_TxResult> MigracionVerificacion(int documentID, DateTime periodo, string pagaduria, List<DTO_MigracionVerificacion> data)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.MigracionVerificacion(this._userChannel, documentID,periodo,pagaduria, data);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }
        

        #endregion

        #region Consultas Generales

        #region Información de los Creditos

        /// <summary>
        /// Funcion que trae la informacion de un credito y su plan de pagos
        /// </summary>
        /// <param name="libranza">Identificador del credito</param>
        /// <returns>Retorna el DTO_Credito con la informacion del credito y su plan de pagos</returns>
        public DTO_Credito GetCredito_All(int libranza)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetCredito_All(this._userChannel, libranza);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la info de un credito por su numero doc
        /// </summary>
        /// <param name="numDoc">Num Doc del credito a buscar</param>
        /// <returns><Retorna la info de un credito/returns>
        public DTO_ccCreditoDocu GetCreditoByNumeroDoc(int numDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetCreditoByNumeroDoc(this._userChannel, numDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la info de un credito por su libranza
        /// </summary>
        /// <param name="isSolicitud">Indicador si se esta buscando una solisitud o un credito</param>
        /// <param name="libranzaID">Identificador de la libranza</param>
        /// <returns><Retorna la info de un credito/returns>
        public DTO_ccCreditoDocu GetCreditoByLibranza(int libranzaID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetCreditoByLibranza(this._userChannel, libranzaID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Muestra el credito de acuerdo a la libranza 
        /// </summary>
        /// <param name="libranza">Numero de libranza a consultar</param>
        /// <param name="numeroDoc">Numero doc de la libranza</param>
        /// <param name="fechaCorte">Fecha en se que realizo la libranza</param>
        /// <param name="isCooperativa"></param>
        /// <returns></returns>
        public DTO_ccCreditoDocu GetCreditoByLibranzaAndFechaCorte(int libranza, int numeroDoc, DateTime fechaCorte)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                DTO_ccCreditoDocu result = this.CarteraService.GetCreditosByLibranzaAndFechaCorte(this._userChannel, libranza, numeroDoc, fechaCorte);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la info de un credito segun el cliente
        /// </summary>
        /// <param name="cliente">Identificador del cliente</param>
        /// <returns><Retorna la info de un credito/returns>
        public List<DTO_ccCreditoDocu> GetCreditoByCliente(string cliente)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetCreditoByCliente(this._userChannel, cliente);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la info de un credito segun el cliente
        /// </summary>
        /// <param name="cliente">Identificador del cliente</param>
        /// <returns><Retorna la info de un credito/returns>
        public List<DTO_ccCreditoDocu> GetCreditoByClienteAndFecha(string cliente, DateTime fechaCorte,bool onlyWithSaldo, bool useFechaCorte)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetCreditoByClienteAndFecha(this._userChannel, cliente, fechaCorte,onlyWithSaldo,useFechaCorte);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la info de un credito segun el comprador de cartera
        /// </summary>
        /// <param name="compradorCartera">Identificador del comprador de cartera</param>
        /// <returns><Retorna la info de un credito/returns>
        public List<DTO_ccCreditoDocu> GetCreditoByCompradorCartera(string compradorCartera)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetCreditoByCompradorCartera(this._userChannel, compradorCartera);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu segun al cliente
        /// </summary>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <param name="isFijado">Indica si el credito esta fijado</param>        
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoDocu> GetCreditosPendientesByCliente(string clienteID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetCreditosPendientesByCliente(this._userChannel, clienteID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae todos los creditos donde la cedula sea codeudor
        /// </summary>
        /// <param name="codeudor">Identificador del codeudor</param>       
        /// <returns>retorna una lista de DTO_ccCreditoDocu</returns>
        public List<DTO_ccCreditoDocu> GetCreditosByCodeudor(string codeudor)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetCreditosByCodeudor(this._userChannel, codeudor);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu segun al cliente
        /// </summary>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <param name="isFijado">Indica si el credito esta fijado</param>        
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoDocu> GetCreditosPendientesByClienteAndEstado(string clienteID, List<byte> estados)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetCreditosPendientesByClienteAndEstado(this._userChannel, clienteID, estados);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu segun al cliente y el proposito
        /// </summary>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <param name="proposito">Proposito en el que esta el credito</param>
        /// <param name="isFijado">Indicador EC_FijadoInd</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoDocu> GetCreditosPendientesByProposito(string clienteID, int proposito)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetCreditosPendientesByProposito(this._userChannel, clienteID, proposito);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae el plan de pagos de un crédito
        /// </summary>
        /// <param name="numeroDoc">Identificador único del crédito</param>
        /// <returns></returns>
        public List<DTO_ccCreditoPlanPagos> GetPlanPagos(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetPlanPagos(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene los correos de los clientes
        /// </summary>
        /// <param name="clienteID">cliente filtrado</param>
        /// <param name="cleinteInd">solo clientes</param>
        /// <param name="conyugeInd">solo conyuges</param>
        /// <param name="codeudorInd">solo codeudores</param>
        /// <returns>Correos</returns>
        public List<DTO_CorreoCliente> GetCorreosCliente(string clienteID, bool clienteInd, bool conyugeInd, bool codeudorInd)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetCorreosCliente(this._userChannel, clienteID, clienteInd, conyugeInd, codeudorInd);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }
        #endregion

        #region Estado de pagos y saldos

        /// <summary>
        /// Funcion que retorna la informacion del recuado manual
        /// </summary>
        /// <param name="numDoc">Numero Doc</param>
        /// <param name="fechaCorte">Fecha de corte</param>
        /// <returns>Retorna DTO con el plan de pagos y sus componentes</returns>
        public DTO_InfoCredito GetInfoCredito(int numDoc, DateTime fechaCorte)
        {
            {
                try
                {
                    this.CreateService(typeof(ICarteraService));
                    var result = this.CarteraService.GetInfoCredito(this._userChannel, numDoc, fechaCorte);
                    return result;
                }
                catch (Exception ex)
                {
                    this.AbortService(typeof(ICarteraService));
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Funcion que retorna la informacion del recuado manual
        /// </summary>
        /// <param name="numDoc">Numero Doc</param>
        /// <param name="fechaCorte">Fecha de corte</param>
        /// <returns>Retorna DTO con el plan de pagos y sus componentes</returns>
        public DTO_InfoCredito GetSaldoCredito(int numDoc, DateTime fechaCorte, bool asignaMora, bool asignaUsura, bool asignaPJ, bool useWhere = true)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetSaldoCredito(this._userChannel, numDoc, fechaCorte, asignaMora, asignaUsura, asignaPJ,useWhere);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna la informacion de los pagos del credito
        /// </summary>
        /// <param name="numDoc">Numero Doc</param>
        /// <param name="fechaCorte">Fecha de corte</param>
        /// <returns>Retorna DTO con el plan de pagos y sus componentes</returns>
        public DTO_InfoCredito GetPagosCredito(int numDoc, DateTime fechaCorte)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetPagosCredito(this._userChannel, numDoc, fechaCorte);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que trae la informacion de los pagos de un credito
        /// </summary>
        /// <param name="numDoc">Numero de referencia del credito</param>
        /// <returns></returns>
        public DTO_InfoPagos GetInfoPagos(int numDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetInfoPagos(this._userChannel, numDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que trae los saldos de las cuentas asociadas a un credito
        /// </summary>
        /// <param name="numDoc">Numero de referencia del credito</param>
        /// <param name="tercero">Tercero al cual pertenece el credito</param>
        /// <returns></returns>
        public List<DTO_ccSaldosComponentes> GetSaldoCuentasForCredito(int numDoc, string tercero)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetSaldoCuentasForCredito(this._userChannel, numDoc, tercero);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un registro ccCreditoPagos por consecutivo
        /// </summary>
        /// <returns>retorna un ccCreditoPagos</returns>
        public DTO_ccCreditoPagos ccCreditoPagos_GetByCons(int consecutivo)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ccCreditoPagos_GetByCons(this._userChannel, consecutivo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Ventas de cartera

        /// <summary>
        /// Trae la info de la venta de un credito por su libranza
        /// </summary>
        /// <param name="numDocLibranza">Identificador de la libranza</param>
        /// <returns><Retorna la info de un credito/returns>
        public DTO_VentaCartera GetInfoVentaByLibranza(int numDocLibranza, int libranza)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetInfoVentaByLibranza(this._userChannel, numDocLibranza, libranza);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un registro de DTO_ccVentaDeta con base a libranza
        /// </summary>
        /// <returns>retorna un registro de DTO_ccVentaDeta</returns>
        public DTO_ccVentaDeta ccVentaDeta_GetByNumDocLibranza(int numDocCredito)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ccVentaDeta_GetByNumDocLibranza(this._userChannel, numDocCredito);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Gestion Cobranza

        /// <summary>
        /// Trae la info del cierre del ultima dia de cartera
        /// </summary>
        /// <param name="cliente">Cliente a consultar</param>
        /// <param name="numDoc Credito">orden de la lista</param>
        /// <returns>Dia de Cierre</returns>
        public DTO_ccCierreDiaCartera ccCierreDiaCartera_GetUltimoDiaCierre(string cliente, int? numDocCredito)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ccCierreDiaCartera_GetUltimoDiaCierre(this._userChannel, cliente, numDocCredito);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la info de un credito segun el cliente
        /// </summary>
        /// <param name="cliente">Cliente a consultar</param>
        /// <param name="orden">orden de la lista</param>
        /// <returns>Lista de creditos por cliente</returns>
        public List<DTO_GestionCobranza> ccCierreDiaCartera_GetGestionCobranza(string cliente, byte orden)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ccCierreDiaCartera_GetGestionCobranza(this._userChannel, cliente, orden);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }
        
        /// <summary>
        /// Trae la info de las actividades de Gestion Cobranza
        /// </summary>
        /// <param name="fechaINi">Fecha inicial a consultar</param>
        /// <param name="fechaFin">Fecha inicial a consultar</param>
        /// <param name="ActividadFlujoID">actividad a consultar</param>
        /// <param name="cliente">client a consultar</param>
        /// <returns>Lista de actividades por cobranza</returns>
        public List<DTO_QueryGestionCobranza> GestionCobranza_GetActividades(DateTime fechaIni, DateTime fechaFin, string etapaFilter, string cliente)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GestionCobranza_GetActividades(this._userChannel,fechaIni,fechaFin,etapaFilter,cliente);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega datos de actividad y actualiza historia del credito
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="cobranzas">Lista de los cobranzas</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult GestionCobranza_Add(int documentoID, List<DTO_GestionCobranza> cobranzas)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GestionCobranza_Add(this._userChannel, documentoID,cobranzas);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene datatable para Excel
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaCorte">Fecha Corte</param>
        /// <param name="gestionCobrID">Gestion CobranzaID</param>
        /// <param name="estado">Estado de la cobranza</param>
        /// <param name="tipoGestion">Tipo gestion</param>
        /// <returns>Datatable</returns>
        public DataTable HistoricoGestionCobranza_GetExcel(int documentoID, DateTime fechaCorte, string gestionCobrID, string clienteID, string libranza, byte? estado, byte? tipoGestion)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.HistoricoGestionCobranza_GetExcel(this._userChannel, documentoID, fechaCorte,gestionCobrID,clienteID,libranza,estado,tipoGestion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene lista para gestion
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaCorte">Fecha Corte</param>
        /// <param name="gestionCobrID">Gestion CobranzaID</param>
        /// <param name="estado">Estado de la cobranza</param>
        /// <param name="tipoGestion">Tipo gestion</param>
        /// <returns>Lista</returns>
        public List<DTO_ccHistoricoGestionCobranza> HistoricoGestionCobranza_GetGestion(int documentoID, DateTime fechaCorte, string gestionCobrID, byte? estado, byte? tipoGestion)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.HistoricoGestionCobranza_GetGestion(this._userChannel, documentoID, fechaCorte, gestionCobrID, estado, tipoGestion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza la tabla de gestiones historico
        /// </summary>
        /// <param name="data">datos a actualizar</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult HistoricoGestionCobranza_Update(int documentoID, List<DTO_ccHistoricoGestionCobranza> data)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.HistoricoGestionCobranza_Update(this._userChannel, documentoID, data);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }
        #endregion

        #endregion

        #region Operaciones Generales

        #region Pantallas Aprobacion

        /// <summary>
        /// Retorna el Listado de las solicitudes de credito
        /// </summary>
        /// <returns></returns> 
        public List<DTO_SolicitudAprobacionCartera> SolicitudLibranza_GetForVerificacion(int DocumentoID, string actividadFlujoID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SolicitudLibranza_GetForVerificacion(this._userChannel, DocumentoID, actividadFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Verifica en la lista que documentos estan para aprobar y rechazar, Actualiza la tarea si es aprobado.
        /// </summary>
        /// <param name="documentID">Documento que ejecula la transaccion</param>
        /// <param name="solicitudes">Listado de documentos</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> SolicitudLibranza_AprobarRechazar(int documentID, string actFlujoID, List<DTO_SolicitudAprobacionCartera> solicitudes, List<DTO_ccSolicitudAnexo> anexos, List<DTO_glDocumentoChequeoLista> tareas)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SolicitudLibranza_AprobarRechazar(this._userChannel, documentID, actFlujoID, solicitudes, anexos,   tareas);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Verifica en la lista que documentos estan para aprobar y rechazar, Actualiza la tarea si es aprobado.
        /// </summary>
        /// <param name="documentID">Documento que ejecula la transaccion</param>
        /// <param name="creditos">Listado de documentos</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Credito_AprobarRechazar(int documentID, string actFlujoID, List<DTO_ccCreditoDocu> creditos)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.PantallaCredito_AprobarRechazar(this._userChannel, documentID, actFlujoID, creditos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// Realiza el proceso de liquidacion de cartera
        /// </summary>
        /// <param name="edad">Edad del cliente. No se usa en el simulador</param>
        /// <param name="lineaCredID">Identificador de la linea de credito</param>
        /// <param name="pagaduriaID">Identificador de la pagaduria</param>
        /// <param name="plazo">Plazo de pago</param>
        /// <param name="valorPrestamo">Valor del prestamo</param>
        /// <param name="fechaLiquida">Fecha de liquidacion</param>
        /// <param name="traerCuotas">Indica si se debe incluir el plan de pagos</param>
        /// <returns>Retorna un objeto TxResult si se presenta un error, de lo contrario devuelve un objeto de tipo DTO_PlanDePagos</returns>
        public DTO_SerializedObject GenerarLiquidacionCartera(string lineaCredID, string pagaduriaID, int valorSolicitado, int vlrGiro, int plazo,
            int edad, DateTime fechaLiquida, decimal? interes, DateTime fechaCuota1)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GenerarLiquidacionCartera(this._userChannel, lineaCredID, pagaduriaID, valorSolicitado, vlrGiro, plazo, edad, fechaLiquida, interes, fechaCuota1);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que actualiza la fecha del plan de pagos y la informacion del credito
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="credito">DTO con la informacion del credito y su plan de pagos</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        public List<DTO_SerializedObject> CambiaFechaPlanPagos(int documentID, DTO_Credito credito)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.CambiaFechaPlanPagos(this._userChannel, documentID, credito);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Creacion Creditos

        #region Solicitud Libranza

        /// <summary>
        /// Trae la info de las solicitudes de un cliente
        /// </summary>
        /// <param name="cliente">Identificador del cliente</param>
        /// <returns><Retorna la info de un credito/returns>
        public List<DTO_ccSolicitudDocu> GetSolicitudesByCliente(string cliente)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetSolicitudesByCliente(this._userChannel, cliente);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna el Listado de las solicitudes de viabilidad
        /// </summary>
        /// <param name="documentoID">Id del Documento</param>
        /// <param name="actFlujoID">Actividad de flujo del documento</param>
        /// <returns>Retorna un listado de las solicitudes que se encuentrar para aprobar o rechazar</returns> 
        public List<DTO_ccSolicitudDocu> GetSolicitudesByActividad(string actFlujoID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetSolicitudesByActividad(this._userChannel, actFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }


        /// <summary>
        /// Trae la informacion de una solicitud de credito segun a libranza
        /// </summary>
        /// <param name="libranzaID">Libranza</param>
        /// <param name="actFlujoID">Identificador de la actividad de flujo</param>
        /// <returns>Retorna la informacion de una solicitud de credito</returns>
        public DTO_SolicitudLibranza SolicitudLibranza_GetByLibranza(int libranzaID, string actFlujoID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SolicitudLibranza_GetByLibranza(this._userChannel, libranzaID, actFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae el NumeroDoc de la tarea
        /// </summary>
        /// <param name="NumeroDoc"></param>
        /// <returns>Trae el NumeroDoc de la tarea</returns>
        public string EstadoSolicitud_GetByNumeroDoc(string _numeroLibranza)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                string result = this.CarteraService.EstadoSolicitud_GetByNumeroDoc(this._userChannel, _numeroLibranza);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualizar el campo de seUsuario del glDocumentoControl
        /// </summary>
        /// <param name="ctrSolicitud">Actualizar el campo de seUsuario del glDocumentoControl</param>
        public void Solicitud_UpdateUSer(DTO_glDocumentoControl ctrSolicitud)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                this.CarteraService.Solicitud_UpdateUSer(this._userChannel, ctrSolicitud);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="solicitud">Solicitud que se debe agregar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult SolicitudLibranza_Add(int documentoID, DTO_SolicitudLibranza solicitud)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SolicitudLibranza_Add(this._userChannel, documentoID, solicitud);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna el Listdo de los Documentos Anexos
        /// </summary>
        /// <param name="NumeroDoc">Numero del Documento</param>
        /// <returns>Lista de DTO_ccSolicitudAnexo</returns>
        public List<DTO_ccSolicitudAnexo> SolicitudLibranza_GetAnexosByID(int NumeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SolicitudLibranza_GetAnexosByID(this._userChannel, NumeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna el Listdo de las tareas
        /// </summary>
        /// <param name="NumeroDoc">Numero del Documento</param>
        /// <returns>Lista de DTO_ccSolicitudAnexo</returns>
        public List<DTO_ccTareaChequeoLista> SolicitudLibranza_GetTareasByNumeroDoc(int NumeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SolicitudLibranza_GetTareasByNumeroDoc(this._userChannel, NumeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae todos los registros de las solicitudes con su estado actual
        /// </summary>
        /// <returns>retorna una lista de DTO_SolicitudGestion</returns>
        public List<DTO_SolicitudGestion> SolicitudLibranza_GetGestionSolicitud()
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SolicitudLibranza_GetGestionSolicitud(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }
        #endregion

        #region Registro Solicitud

        /// <summary>
        /// Trae la informacion de un registro de solicitud de credito segun a libranza
        /// </summary>
        /// <param name="libranzaID">id del credito</param>
        /// <param name="actFlujoID">Identificador de la actividad de flujo</param>
        /// <returns>Retorna la informacion de una solicitud de credito</returns>
        public DTO_DigitaSolicitudCredito RegistroSolicitud_GetBySolicitud(int libranzaID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RegistroSolicitud_GetBySolicitud(this._userChannel, libranzaID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega informacion a las tablas del registro de solicitud
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="data">Datos que se debe agregar</param>
        ///  <param name="isNewVersion">Indica si es nueva version</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult RegistroSolicitud_Add(int documentoID, string actFlujoId, DTO_DigitaSolicitudCredito data, bool isNewVersion)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RegistroSolicitud_Add(this._userChannel,documentoID,actFlujoId,data,isNewVersion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la informacion de un registro de solicitud con cedula
        /// </summary>
        /// <param name="deudor">Deudor del credito</param>
        /// <param name="cedula">cedula a obtener</param>
        /// <param name="tipoPersona">Tipo de persona a obtener</param>
        /// <param name="versionNro">Version del registro</param>
        /// <returns>Datos del cliente</returns>
        public DTO_ccSolicitudDatosPersonales RegistroSolicitud_GetByCedula(string deudor, string cedula, byte tipoPersona, int versionNro)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RegistroSolicitud_GetByCedula(this._userChannel, deudor,cedula, tipoPersona, versionNro);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Aprobacion Solicitud Libranza

        /// <summary>
        /// Retorna el Listado de las solicitudes de credito
        /// </summary>
        /// <param name="documentoID">Id del Documento</param>
        /// <param name="actFlujoID">Actividad de flujo del documento</param>
        /// <returns>Retorna un listado de las solicitudes que se encuentrar para aprobar o rechazar</returns> 
        public List<DTO_SolicitudAprobacionCartera> SolicitudDocu_GetForAprobacion(int documentoID, string actFlujoID, int _libranza)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SolicitudDocu_GetForAprobacion(this._userChannel, documentoID, actFlujoID, _libranza);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Verifica en la lista que documentos estan para aprobar y rechazar, Actualiza la tarea si es aprobado.
        /// </summary>
        /// <param name="documentID">Documento que ejecula la transaccion</param>
        /// <param name="solicitudes">Listado de documentos</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> AprobacionSolicitud_AprobarRechazar(int documentID, string actFlujoID, List<DTO_SolicitudAprobacionCartera> solicitudes)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.AprobacionSolicitud_AprobarRechazar(this._userChannel, documentID, actFlujoID, solicitudes);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Digitacion Credito

        /// <summary>
        /// Trae la informacion de una solicitud de credito segun a libranza
        /// </summary>
        /// <param name="libranzaID">Libranza</param>
        /// <param name="actFlujoID">Identificador de la actividad de flujo</param>
        /// <returns>Retorna la informacion de una solicitud de credito</returns>
        public DTO_DigitacionCredito DigitacionCredito_GetByLibranza(int libranzaID, string actFlujoID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.DigitacionCredito_GetByLibranza(this._userChannel, libranzaID, actFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="digCredito">Solicitud que se debe agregar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult DigitacionCredito_Add(int documentoID, string actFlujoID, DTO_DigitacionCredito digCredito, List<DTO_Cuota> cuotasExtras)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.DigitacionCredito_Add(this._userChannel, documentoID, actFlujoID, digCredito, cuotasExtras);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Añade los un registro a ccCreditoComponentes
        /// </summary>
        /// <param name="mvto">Movimiento de la caretra</param>
        public List<DTO_ccCreditoComponentes> ccCreditoComponentes_GetByNumDocCred(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ccCreditoComponentes_GetByNumDocCred(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Referenciacion Cartera

        #endregion

        #region Solicitud Anticipos

        /// <summary>
        /// Funcion que retorna el listado de las compra de cartera para solicitud de anticipos
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="libranzaID">Numero de la libranza</param>
        /// <returns>Retorna las compra de cartera para realizar la solicitud de anticipo</returns>     
        public List<DTO_ccSolicitudCompraCartera> SolicitudAnticipo_GetByLibranza(int documentID, int libranzaID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SolicitudAnticipo_GetByLibranza(this._userChannel, documentID, libranzaID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Envia la solicitud de nuevos anticipos
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccion</param>
        /// <param name="compras">Listado de carteras externas a comprar</param>
        /// <returns>Retorna la lista de resultados</returns>
        public List<DTO_SerializedObject> SolicitudAnticipos_SolicitarAnticipos(int documentID, List<DTO_ccSolicitudCompraCartera> compras)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SolicitudAnticipos_SolicitarAnticipos(this._userChannel, documentID, compras);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera los paz y salvos de la compra de cartea
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccion</param>
        /// <param name="compras">Listado de carteras externas a comprar</param>
        /// <returns>Retorna la lista de resultados</returns>
        public List<DTO_SerializedObject> SolicitudAnticipos_GenerarPazYSalvo(int documentID, List<DTO_ccSolicitudCompraCartera> compras)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SolicitudAnticipos_GenerarPazYSalvo(this._userChannel, documentID, compras);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera los paz y salvos de la compra de cartea
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccion</param>
        /// <param name="compras">Listado de carteras externas a comprar</param>
        /// <returns>Retorna la lista de resultados</returns>
        public List<DTO_SerializedObject> SolicitudAnticipos_RevertirAnticipos(int documentID, List<DTO_ccSolicitudCompraCartera> compras)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SolicitudAnticipos_RevertirAnticipos(this._userChannel, documentID, compras);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Devolución Solicitud

        /// <summary>
        /// Trae la informacion de una solicitud de credito para devolición
        /// </summary>
        /// <param name="libranzaID">Libranza</param>
        /// <returns>Retorna la informacion de una solicitud de credito</returns>
        public DTO_ccSolicitudDocu DevolucionSolicitud_GetByLibranza(int libranzaID, ref List<DTO_ccSolicitudDevolucionDeta> devoluciones)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.DevolucionSolicitud_GetByLibranza(this._userChannel, libranzaID, ref devoluciones);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="solicitud">Solicitud que se debe agregar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult DevolucionSolicitud_Add(int documentoID, DTO_ccSolicitudDevolucion devolucion)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.DevolucionSolicitud_Add(this._userChannel, documentoID, devolucion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Consulta informacion de la tabla ccSolicitudDevolucion
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="numDocSol">Solicitud que se debe consultar</param>
        /// <returns>Retorna lista</returns>
        public List<DTO_ccSolicitudDevolucion> DevolucionSolicitud_GetByNumeroDoc(int documentoID, int numDocSol)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.DevolucionSolicitud_GetByNumeroDoc(this._userChannel, documentoID, numDocSol);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la informacion de una solicitud de credito para devolición
        /// </summary>
        /// <param name="libranzaID">Libranza</param>
        /// <returns>Retorna la informacion de una solicitud de credito</returns>
        public string Report_cc_DevolucionSolicitud(string _credito, int _numDoc, int _numDev)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_cc_DevolucionSolicitud(this._userChannel, _credito, _numDoc, _numDev);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Liquidación Crédito

        /// <summary>
        /// Retorna el Listado de todos los creditos aprobados
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="detalleBeneficiarios">Indicador para establecer si debe buscar los beneficiarios del credito</param>
        /// <param name="allEmpresas">Indica si trae la infomacion de todas las empresas</param>
        /// <returns></returns>     
        public List<DTO_ccCreditoDocu> LiquidacionCredito_GetAll(string actFlujoID, bool detalleBeneficiarios, bool allEmpresas)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.LiquidacionCredito_GetAll(this._userChannel, actFlujoID, detalleBeneficiarios, allEmpresas);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Verifica en la lista que documentos estan para aprobar, Actualiza la tarea si es aprobado.
        /// </summary>
        /// <param name="documentID">Documento que ejecula la transaccion</param>
        /// <param name="solicitudes">Listado de documentos</param>
        /// <returns>Retorna el resultado de la operacion</returns>Guid channel, int documentID, string actFlujoID, List<DTO_ccCredito> ccCreditos 
        public List<DTO_SerializedObject> LiquidacionCredito_AprobarRechazar(int documentID, string actFlujoID, List<DTO_SolicitudAprobacionCartera> solicitudes)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.LiquidacionCredito_AprobarRechazar(this._userChannel, documentID, actFlujoID, solicitudes);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Revierte un crédito
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="numeroDoc"></param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult LiquidacionCredito_Revertir(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.LiquidacionCredito_Revertir(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Restructura sin Cambio de credito
        /// </summary>
        /// <documentID>Identificador del socumento</documentID>
        /// <ctrlCredito>Documento control relacionado con el crédito</ctrlCredito>
        /// <digCredito>Datos nuevos del credito</creditoPoliza>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult RestructuracionSinCambio(int documentID, string actFlujoID, DTO_ccCreditoDocu credito, DTO_DigitacionCredito digCredito)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RestructuracionSinCambio(this._userChannel, documentID, actFlujoID, credito, digCredito);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Aprobación Giro

        /// <summary>
        /// Funcion que aprueba o rechaza un giro
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="actFlujoID">Actividad Flujo ID</param>
        /// <param name="ccCreditos">Lista con los giros para aprobar o rechazar</param>
        /// <returns>Retorna una lista de objetos</returns>
        public List<DTO_TxResult> AprobarGiro_Credito_AprobarRechazar(int documentID, string actFlujoID, List<DTO_ccCreditoDocu> ccCreditos, bool pagoMaviso)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.AprobarGiro_Credito_AprobarRechazar(this._userChannel, documentID, actFlujoID, ccCreditos, pagoMaviso);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que aprueba o rechaza un giro
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="actFlujoID">Actividad Flujo ID</param>
        /// <param name="ccCreditos">Lista con los giros para aprobar o rechazar</param>
        /// <returns>Retorna una lista de objetos</returns>
        public List<DTO_TxResult> AprobarGiro_CreditoRechazo(int documentID, string actFlujoID, List<DTO_ccCreditoDocu> creditos)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.AprobarGiro_CreditoRechazo(this._userChannel, documentID, actFlujoID, creditos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna el Listado de todos los creditos rechazados
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <returns></returns>     
        public List<DTO_ccCreditoDocu> AprobarGiroRechazo_GetAll(string actFlujoID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.AprobarGiroRechazo_GetAll(this._userChannel, actFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Estado Cuenta

        /// <summary>
        /// Agrega un estado de cuenta
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="actFlujoId">Actividad de flujo</param>
        /// <param name="infoCredito">Informacion Credito</param>
        /// <param name="estadoCuenta">Informacion del estado de cuenta</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult EstadoCuenta_Add(int documentoID, string actFlujoId, DTO_InfoCredito infoCredito, DTO_EstadoCuenta estadoCuenta)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.EstadoCuenta_Add(this._userChannel, documentoID, actFlujoId, infoCredito, estadoCuenta);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna los componentes del estado de cuenta
        /// </summary>
        /// <param name="numDoc">Numero Doc</param>
        /// <param name="isPagoParcial">Indicador para verificar si hay pagos parciales (Pagos Totales)</param>
        /// <returns>Retorna el listado de los componentes del estado de cuenta segun el numDoc</returns>
        public List<DTO_ccEstadoCuentaComponentes> EstadoCuenta_GetComponentesByNumeroDoc(int numDoc, bool isPagoTotal)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.EstadoCuenta_GetComponentesByNumeroDoc(this._userChannel, numDoc, isPagoTotal);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna los componentes del estado de cuenta
        /// </summary>
        /// <param name="numDoc">Numero Doc</param>
        /// <returns>Retorna el listado de los componentes del estado de cuenta segun el numDoc</returns>
        public DTO_ccEstadoCuentaHistoria EstadoCuenta_GetHistoria(int numDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.EstadoCuenta_GetHistoria(this._userChannel, numDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna los componentes del estado de cuenta para revocatoria
        /// </summary>
        /// <param name="aseguradora">Aseguradora</param>
        /// <returns>Retorna el listado de creditos para revocar </returns>
        public List<DTO_ccCreditoDocu> EstadoCuenta_GetForRevocatoria(string aseguradora)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.EstadoCuenta_GetForRevocatoria(this._userChannel, aseguradora);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna la información de un estado de cuenta
        /// </summary>
        /// <param name="numDoc">Numero Doc</param>
        /// <returns>Retorna la información asociada a un estado de cuenta</returns>
        public DTO_EstadoCuenta EstadoCuenta_GetAll(int numDocEC)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.EstadoCuenta_GetAll(this._userChannel, numDocEC);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega un documento de revocacion
        /// </summary>
        /// <param name="documentoID">ID</param>
        /// <param name="actFlujoId">flujo</param>
        /// <param name="aseguradoraID">ASeguradora</param>
        /// <param name="ctrl">doc control</param>
        /// <param name="creditosRevocar">lista de creditos a revocar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult PolizaRevocatoria_Add(int documentoID,string aseguradoraID, DTO_glDocumentoControl ctrl, List<DTO_ccCreditoDocu> creditosRevocar, bool update)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.PolizaRevocatoria_Add(this._userChannel, documentoID,aseguradoraID,ctrl,creditosRevocar,update);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Pagos Creditos

        /// <summary>
        /// Funcion que agrega un racaudo manual a la tabla ccCreditoPagos
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="actFlujoId">Actividad de flujo</param>
        /// <param name="reciboCaja">Recibo de caja</param>
        /// <param name="credito">Informacion basica del credito</param>
        /// <param name="planPagos">Lista del plan de pagos</param>
        /// <param name="componentes">Lista de los componentes de cada cuota</param> List<DTO_ccCreditoComponentes> componentes
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult PagosCreditos_Parcial(TipoRecaudo tipoRecaudo, int documentID, string actFlujoID, DateTime fechaDoc, DateTime fechaPago,
            DTO_tsReciboCajaDocu reciboCaja, DTO_ccCreditoDocu credito, List<DTO_ccCreditoPlanPagos> planPagos,List<DTO_ccSaldosComponentes> componentesPago, List<DTO_ccSaldosComponentes> componentes)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.PagosCreditos_Parcial(this._userChannel, tipoRecaudo, documentID, actFlujoID, fechaDoc, fechaPago, reciboCaja, credito, planPagos,componentesPago, componentes);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que realiza el pago parcial o total de un credito para pago total
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccione</param>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="reciboCaja">Recibo de caja</param>
        /// <param name="credito">Informacion basica del credito</param>
        /// <param name="componentesPago">Lista de los componentes de cada cuota</param> List<DTO_ccCreditoComponentes> componentes
        /// <param name="isPagoParcial">Indica si se esta realizando un pago parcial</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult PagosCreditos_Total(int documentID, string actFlujoID, DateTime fechaDoc, DateTime fechaPago, DTO_tsReciboCajaDocu reciboCaja,
            DTO_ccCreditoDocu credito, List<DTO_ccEstadoCuentaComponentes> ec_componentes, List<DTO_ccSaldosComponentes> componentesPago, bool isPagoParcial)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.PagosCreditos_Total(this._userChannel, documentID, actFlujoID, fechaDoc, fechaPago, reciboCaja, credito,
                    ec_componentes, componentesPago, isPagoParcial);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Revierte un pago de cartera
        /// </summary>
        /// <param name="documentID">Documento que genera la reversión</param>
        /// <param name="numeroDoc">Numero doc del pago a revertir</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult CarteraPagos_Revertir(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.CarteraPagos_Revertir(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Otras Operaciones Créditos

        /// <summary>
        /// Funcion que realiza el rechazo de un crédito
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccion</param>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="credito">Informacion basica del credito</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Credito_Rechazo(int documentID, string actFlujoID, DTO_ccCreditoDocu credito, DateTime fecha, string bancoID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Credito_Rechazo(this._userChannel, documentID, actFlujoID, credito, fecha, bancoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que realiza el desistimiento de un crédito
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccione</param>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="reciboCaja">Recibo de caja</param>
        /// <param name="credito">Informacion basica del credito</param>
        /// <param name="componentesPago">Lista de los componentes de cada cuota</param> List<DTO_ccCreditoComponentes> componentes
        /// <param name="isPagoParcial">Indica si se esta realizando un pago parcial</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Credito_Desistimiento(int documentID, string actFlujoID, DTO_ccCreditoDocu credito, DateTime fecha)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Credito_Desistimiento(this._userChannel, documentID, actFlujoID, credito, fecha);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que realiza el desistimiento de un crédito
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccione</param>
        /// <param name="credito">Informacion basica del credito</param>
        /// <param name="centroPagoID">Identificador del nuevo centro de pago</param>
        /// <param name="ciudadID">Identificador de la ciudad</param>
        /// <param name="pagaduriaID">Identificador de la pagaduría</param>
        /// <param name="zonaID">Identificador de la zona</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Credito_CambioDatos(int documentID, DTO_ccCreditoDocu credito, string centroPagoID, string pagaduriaID, string zonaID, string ciudadID,
            string cooperativaID, string novedad, string estadoCobranza, string gestionCobranza, string estadoSinisestro, string obs)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Credito_CambioDatos(this._userChannel, documentID, credito, centroPagoID, pagaduriaID, zonaID, ciudadID, cooperativaID,
                    novedad, estadoCobranza, gestionCobranza, estadoSinisestro, obs);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }


        /// <summary>
        /// Funcion que realiza el cambio de datos de un cliente
        /// </summary>
        /// <param name="Cambio">Tipo Cambio</param>
        /// <param name="cliente">Informacion basica del Cliente</param>
        /// <param name="tercero">Informacion basica del Tercero</param>
        public DTO_TxResult Cliente_CambioDatos(int cambio,DTO_ccCliente cliente, DTO_coTercero tercero)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Cliente_CambioDatos(this._userChannel, cambio,cliente, tercero);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Revierte el desistimiento de un crédito
        /// </summary>
        /// <param name="documentID">Documento que genera la reversión</param>
        /// <param name="numeroDoc">Numero doc del pago a revertir</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Credito_Desistimiento_Revertir(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Credito_Desistimiento_Revertir(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae el saldo de un crédito
        /// </summary>
        /// <returns>retorna una el saldo de un crédito</returns>
        public decimal Credito_GetSaldoFlujos(int numeroDoc, out int flujosPagados)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Credito_GetSaldoFlujos(this._userChannel, numeroDoc, out flujosPagados);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Incorporaciones

        #region Incorporacion

        /// <summary>
        /// Rertorna los pagaduria inválidas en una fechade incorporación
        /// </summary>
        /// <returns></returns>
        public List<string> IncorporacionCredito_GetInvalidPagadurias(DateTime fechaIncorpora)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.IncorporacionCredito_GetInvalidPagadurias(this._userChannel, fechaIncorpora);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que consulta las solicitudes que estan en estado de incorporacion previa activado
        /// </summary>
        /// <param name="centroPago">Centro de pago que se usa para consultar</param>
        /// <param name="isVerificacion">Indica si la operacion es la de verificacion o no</param>
        /// <param name="tipoVerificacion">Indica el tipo de verificacion</param>
        /// <returns>Retorna una lista con las solicitudes de credito para incorporacion segun la pagaduria</returns>
        public List<DTO_ccSolicitudDocu> IncorporacionSolicitud_GetByCentroPago(string centroPago, DateTime fechaIncorpora, string actFlujoID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.IncorporacionSolicitud_GetByCentroPago(this._userChannel, centroPago, fechaIncorpora, actFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que consulta las solicitudes que estan en estado de incorporacion previa activado
        /// </summary>
        /// <param name="centroPago">Centro de pago que se usa para consultar</param>
        /// <param name="isVerificacion">Indica si la operacion es la de verificacion o no</param>
        /// <param name="tipoVerificacion">Indica el tipo de verificacion</param>
        /// <returns>Retorna una lista con las solicitudes de credito para incorporacion segun la pagaduria</returns>
        public List<DTO_ccSolicitudDocu> IncorporacionSolicitudVerificacion_GetByCentroPago(string centroPago, string actFlujoID, int tipoVerificacion)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.IncorporacionSolicitudVerificacion_GetByCentroPago(this._userChannel, centroPago, actFlujoID, tipoVerificacion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que consulta los creditos para incorporacion segun una pagaduria
        /// </summary>
        /// <param name="centroPago">Centro de pago que se usa para consultar</param>
        /// <param name="isVerificacion">Indica si la operacion es la de verificacion o no</param>
        /// <returns>Retorna una lista con los creditos para incorporacion segun la pagaduria</returns>
        public List<DTO_ccCreditoDocu> IncorporacionCredito_GetByCentroPago(string centroPago, DateTime fechaIncorpora, string actFlujoID, bool getPendientes = false)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.IncorporacionCredito_GetByCentroPago(this._userChannel, centroPago, fechaIncorpora, actFlujoID,getPendientes);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que consulta los creditos para incorporacion segun una pagaduria
        /// </summary>
        /// <param name="centroPago">Centro de pago que se usa para consultar</param>
        /// <param name="isVerificacion">Indica si la operacion es la de verificacion o no</param>
        /// <returns>Retorna una lista con los creditos para incorporacion segun la pagaduria</returns>
        public List<DTO_ccCreditoDocu> IncorporacionCreditoVerificacion_GetByCentroPago(string centroPago, string actFlujoID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.IncorporacionCreditoVerificacion_GetByCentroPago(this._userChannel, centroPago, actFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna las incorporaciones por credito
        /// </summary>
        /// <returns></returns>
        public List<DTO_ccIncorporacionDeta> IncorporacionCredito_GetByNumDocCred(int numDocredito)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.IncorporacionCredito_GetByNumDocCred(this._userChannel, numDocredito);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }


        /// <summary>
        /// Realiza la incorporacion de los creditos o las solicitudes
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="documentID">Documento que ejecuta la transaccion</param>
        /// <param name="CentroPago">Id del centro de pago</param>
        /// <param name="fechaIncorpora">Fecha en la que se incorporan los creditos</param>
        /// <param name="vlrIncorporacion"> Valor total de la incorporacion</param>
        /// <param name="creditos">Lista de creditos que se estan incorporando</param>
        /// <param name="solicitudes">Lista de solicitudes que se estan incorporando</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public Tuple<int, List<DTO_SerializedObject>, string> IncorporacionCredito_Aprobar(int documentID, string actFlujoID, string centroPago, DateTime fechaIncorpora, Decimal vlrIncorporacion, List<DTO_ccCreditoDocu> creditos, List<DTO_ccSolicitudDocu> solicitudes)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.IncorporacionCredito_Aprobar(this._userChannel, documentID, actFlujoID, centroPago, fechaIncorpora, vlrIncorporacion, creditos, solicitudes);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Revierte una incorporacion
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="numeroDoc"></param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult IncorporacionCredito_Revertir(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.IncorporacionCredito_Revertir(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Guarda las incorporaciones 
        /// </summary>
        /// <param name="documentID">documento actual</param>
        /// <param name="periodoID">periodo actual</param>
        /// <param name="fecha">fecha actual</param>
        /// <param name="migraciones">lista de miegraciones a incorporar</param>
        /// <returns>resultado</returns>
        public DTO_TxResult Incorporacion_Guardar(int documentID, DateTime periodoID, DateTime fecha, List<DTO_MigrarIncorporacionDeta> migraciones)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Incorporacion_Guardar(this._userChannel, documentID, periodoID, fecha, migraciones);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }
        #endregion

        #region Verificación Incorporación

        /// <summary>
        /// Realiza la incorporacion de los creditos o las solicitudes
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="documentID">Documento que ejecuta la transaccion</param>
        /// <param name="CentroPago">Id del centro de pago</param>
        /// <param name="fechaVerificacion">Fecha en la que se verifica la incorporacion</param>
        /// <param name="vlrVerificacion"> Valor total de la verificacion</param>
        /// <param name="creditos">Lista de creditos que se estan incorporando</param>
        /// <param name="solicitudes">Lista de solicitudes que se estan incorporando</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> VerificacionIncorporacion_AprobarRechazar(int documentID, string actFlujoID, string centroPago, DateTime fechaVerificacion, Decimal vlrVerificacion, List<DTO_ccCreditoDocu> creditos, List<DTO_ccSolicitudDocu> solicitudes)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.VerificacionIncorporacion_AprobarRechazar(this._userChannel, documentID, actFlujoID, centroPago, fechaVerificacion, vlrVerificacion, creditos, solicitudes);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region DesIncorporacion

        /// <summary>
        /// Funcion que consulta los creditos para desincorporar que pertenecen a un centro de pago
        /// </summary>
        /// <param name="centroPago">Centro de pago que se usa para consultar</param>
        /// <returns>Retorna una lista con los creditos para incorporacion segun la pagaduria</returns>
        public List<DTO_ccCreditoDocu> DesIncorporacionCredito_Get()
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.DesIncorporacionCredito_Get(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Verifica en la lista que documentos estan para aprobar y rechazar, Actualiza la tarea si es aprobado.
        /// </summary>
        /// <param name="documentID">Documento que ejecula la transaccion</param>
        /// <param name="creditos">Listado de documentos</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> DesIncorporacion_AprobarRechazar(int documentID, string actFlujoID, List<DTO_ccCreditoDocu> creditos, DateTime fechaNovedad)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.DesIncorporacion_AprobarRechazar(this._userChannel, documentID, actFlujoID, creditos, fechaNovedad);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Revierte una desincorporacion
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="numeroDoc"></param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult DesIncorporacion_Revertir(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.DesIncorporacion_Revertir(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Reincorporacion

        /// <summary>
        /// Trae la información para hacer reincorporaciones
        /// </summary>
        /// <returns></returns>
        public List<DTO_ccReincorporacionDeta> Reincorporacion_GetForReincorporacion(DateTime periodo, string centroPagoID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Reincorporacion_GetForReincorporacion(this._userChannel, periodo, centroPagoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega reincorporaciones
        /// </summary>
        /// <returns></returns>
        public DTO_TxResult Reincorporacion_Aprobar(int documentID, string actFlujoID, DateTime periodo, DateTime fecha, List<DTO_ccReincorporacionDeta> data)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Reincorporacion_Aprobar(this._userChannel, documentID, actFlujoID, periodo, fecha, data);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Revierte una reincorporacion
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="numeroDoc"></param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Reincorporacion_Revertir(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Reincorporacion_Revertir(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Archivos Incorporacion

        /// <summary>
        /// Carga los creditos que estn incorporados
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="pagaduria"></param>
        /// <returns></returns>
        public List<DTO_ccArchivoIncorporaciones> GetArchivosIncorporacion(DateTime periodo, string pagaduria)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetArchivosIncorporacion(this._userChannel, periodo, pagaduria);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza los campos de la tabla ccIncorporacionDeta
        /// </summary>
        public void UpdateIncorporacionFechaTransmite(DateTime fechaTransmite, List<int> consecutivos)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                this.CarteraService.UpdateIncorporacionFechaTransmite(this._userChannel, fechaTransmite, consecutivos);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }


        #endregion

        #endregion

        #region Venta Cartera

        #region Endoso Libranzas

        /// <summary>
        /// Verifica en la lista que documentos estan para aprobar y rechazar, Actualiza la tarea si es aprobado.
        /// </summary>
        /// <param name="documentID">Documento que ejecula la transaccion</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="creditosVendidos">Lista de creditos para aprobar/rechazar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> EndosoLibranzasVendidas_AprobarRechazar(int documentID, string actFlujoID, List<DTO_ccVentaDeta> creditosVendidos)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.EndosoLibranzasVendidas_AprobarRechazar(this._userChannel, documentID, actFlujoID, creditosVendidos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Preventa Cartera

        /// <summary>
        /// Trae todos las ofertas de preventa y venta
        /// </summary>
        /// <param name="compradorCarteraID">Comprador a consultar</param>
        /// <param name="isVenta">Indicador para establecer si trae las ofertas de preventas ode ventas</param>
        /// <returns>retorna una lista de ccCreditoDocu</returns>
        public List<string> PreventaCartera_GetOfertas(string compradorCarteraID, bool isVenta)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.PreventaCartera_GetOfertas(this._userChannel, compradorCarteraID, isVenta);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que obtiene los creditos que fueron preseleccionados para la venta
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="compradorCarteraID">Comprador de cartera</param>
        /// <param name="oferta">Oferta de la compra</param>
        /// <param name="factorCesion">Factor que se aplica a la venta</param>
        /// <returns></returns> 
        public DTO_VentaCartera PreventaCartera_GetCreditos(string actFlujoID, string compradorCarteraID, string oferta, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.PreventaCartera_GetCreditos(this._userChannel, actFlujoID, compradorCarteraID, oferta, fechaIni, fechaFin);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que genera un nuevo documento de venta de cartera
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="ventaDocu">DTO que contiene la informacion de la venta docu</param>
        /// <param name="creditos">Lista de creditos pre vendidos</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        public Tuple<int, List<DTO_SerializedObject>> PreventaCartera_Add(int documentID, string actFlujoID, bool sendToApprove, DateTime fechaVenta, DTO_VentaCartera ventaCartera)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.PreventaCartera_Add(this._userChannel, documentID, actFlujoID, sendToApprove, fechaVenta, ventaCartera);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Venta Cartera

        /// <summary>
        /// Trae todos los registros de DTO_ccNominaDeta
        /// </summary>
        /// <returns>retorna una lista de DTO_ccNominaDeta</returns>
        public DTO_ccVentaDocu ccVentaDocu_GetByID(int NumeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ccVentaDocu_GetByID(this._userChannel, NumeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que trae los creditos para la venta de cartera
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="compradorCarteraID">Comprador de la venta de cartera</param>
        /// <param name="oferta">Oferta de la compra de cartera</param>
        /// <returns></returns>
        public DTO_VentaCartera VentaCartera_GetForVenta(string actFlujoID, string compradorCarteraID, string oferta)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.VentaCartera_GetForVenta(this._userChannel, actFlujoID, compradorCarteraID, oferta);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que trae los creditos para la venta de cartera
        /// </summary>
        /// <param name="compradorCarteraID">Comprador de la venta de cartera</param>
        /// <param name="oferta">oferta del credito</param>
        /// <param name="mesINI">Mes Inicial de la consulta</param>
        /// <param name="mesFIN">Mes Final de la consulta</param>
        /// <param name="Tipo">Tipo de Consulta</param>
        /// <returns>Lista de Ventas</returns>        
        public List<DTO_QueryVentaCartera> VentaCartera_GetForCompradorCart(string compradorCarteraID, string oferta, DateTime mesINI, DateTime mesFIN, TipoVentaCartera tipo)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.VentaCartera_GetForCompradorCart(this._userChannel, compradorCarteraID, oferta, mesINI, mesFIN, tipo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que trae los creditos vendidos
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <returns></returns>
        public DTO_VentaCartera VentaCartera_GetByActividadFlujo(string actFlujoID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.VentaCartera_GetByActividadFlujo(this._userChannel, actFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que genera un nuevo documento de recompra de cartera con sus detalles
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="ventaCartera">Dto que contiene el documento y el detalle de la venta de cartera</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        public Tuple<int, List<DTO_SerializedObject>, string> VentaCartera_Add(int documentID, string actFlujoID, DTO_VentaCartera ventaCartera)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.VentaCartera_Add(this._userChannel, documentID, actFlujoID, ventaCartera);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Revierte una venta de cartera
        /// </summary>
        /// <param name="documentID">Documento que genera la reversión</param>
        /// <param name="numeroDoc">Numero doc del pago a revertir</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult VentaCartera_Revertir(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.VentaCartera_Revertir(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Fondeo Cartera

        /// <summary>
        /// Funcion que guarda los creditos comprados por el fondeador
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="compraDocu">DTO del documento a generar</param>
        /// <param name="migracionVenta">Lista con los detalles a guardad</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        public List<DTO_SerializedObject> FondeoCartera_Add(int documentID, string actFlujoID, DTO_ccCompraDocu compraDocu, List<DTO_MigrarVentaCartera> migracionVenta)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.FondeoCartera_Add(this._userChannel, documentID, actFlujoID, compraDocu, migracionVenta);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Pago Flujos

        /// <summary>
        /// Funcion que trae los creditos para el pago de flujos
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="fechaPeriodo">Fecha actual del periodo</param>
        /// <returns></returns>
        public DTO_PagoFlujos PagoFlujos_GetForPago(string actFlujoID, DateTime fechaPeriodo, string oferta, int? libranza, string comprador)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.PagoFlujos_GetForPago(this._userChannel, actFlujoID, fechaPeriodo, oferta, libranza, comprador);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que genera un nuevo documento de pago de flujos con sus detalles
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="recompraCartera">Dto que contiene el documento y el detalle del pago de flujo</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        public List<DTO_SerializedObject> PagoFlujos_Add(int documentID, string actFlujoID, string compradorID, DateTime fechaPago, DTO_PagoFlujos pagoFlujos)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.PagoFlujos_Add(this._userChannel, documentID, actFlujoID, compradorID, fechaPago, pagoFlujos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Reasigna Comprador Cartera

        /// <summary>
        /// Funcion que trae los creditos para reasignar el comprador
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="compradorCarteraID">Comprador de la venta de cartera</param>
        /// <returns></returns>
        public DTO_ReasignaCompradorFinal ReasignaCompradorCartera_Get(string actFlujoID, string compradorCarteraID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ReasignaCompradorCartera_Get(this._userChannel, actFlujoID, compradorCarteraID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que genera un nuevo documento de reasignacion de compradores de cartera con sus detalles
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="reasignaCompFinal">Dto que contiene el documento y el detalle del pago de flujo</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        public List<DTO_SerializedObject> ReasignaCompradorCartera_Add(int documentID, string actFlujoID, DTO_ReasignaCompradorFinal reasignaCompFinal)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ReasignaCompradorCartera_Add(this._userChannel, documentID, actFlujoID, reasignaCompFinal);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Recompra Cartera

        /// <summary>
        /// Funcion que retorna la lista de recompras asignadas a ese comprado
        /// </summary>
        /// <param name="compradorCarteraID">Comprador de cartera</param>
        /// <returns>Lista con las recompras del comprador de cartera</returns>
        public List<DTO_ccRecompraDeta> RecompraCartera_GetByCompradorCartera(string compradorCarteraID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RecompraCartera_GetByCompradorCartera(this._userChannel, compradorCarteraID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que trae la lista de creditos disponibles para la compra o sustitucion
        /// </summary>
        /// <param name="actFlujoID">Actividad de Flujo</param>
        /// <param name="compradorCarteraID">Id del comprador de la cartera</param>
        /// <returns></returns>
        public DTO_RecompraCartera RecompraCartera_GetForCompraAndSustitucion(string compradorCarteraID, List<int> libranzasFilter, ref string msgError)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RecompraCartera_GetForCompraAndSustitucion(this._userChannel, compradorCarteraID,libranzasFilter,ref msgError);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que genera un nuevo documento de recompra de cartera con sus detalles
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="recompraCartera">Dto que contiene el documento y el detalle de la recompra de cartera</param>
        /// <returns></returns>
        public List<DTO_SerializedObject> RecompraCartera_Add(int documentID, string actFlujoID, bool isIndividual, bool isMaduracionAnt, DTO_RecompraCartera recompraCartera)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RecompraCartera_Add(this._userChannel, documentID, actFlujoID, isIndividual, isMaduracionAnt, recompraCartera);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Nota Credito

        /// <summary>
        /// Funcion que genera una nota credito de un credito
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="resumenNC">Lista de componentes</param>
        /// <param name="infoCartera">lista de cuotas</param>
        /// <param name="ctrl">documento nota credito</param>
        /// <param name="resintegroSaldo">cuenta para reintegro saldo</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        public DTO_SerializedObject NotaCredito_Add(int documentID, List<DTO_NotaCreditoResumen> resumenNC, DTO_InfoCredito infoCartera, DTO_glDocumentoControl ctrl, string reintegroSaldo)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.NotaCredito_Add(this._userChannel, documentID, resumenNC, infoCartera,ctrl, reintegroSaldo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Cobro Jurídico y Acuerdos de Pago

        /// <summary>
        /// Trae la info de un historico
        /// </summary>
        /// <param name="numDocCredito">Numero doc del crédito</param>
        /// <returns>retorna los 2 ultimos registros del histórico de CJ</returns>
        public Tuple<DTO_ccCJHistorico, DTO_ccCJHistorico> ccCJHistorico_GetForAbono(int numDocCredito, bool withFilter)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ccCJHistorico_GetForAbono(this._userChannel, numDocCredito, withFilter);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu segun al cliente, y que tenga un estado de cuenta en cobro juridico
        /// </summary>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <param name="proposito">Indicador del estado de cuenta en cobro juridico</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoDocu> GetCobroJuridicoByCliente(string clienteID, DateTime fecha)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetCobroJuridicoByCliente(this._userChannel, clienteID, fecha);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que genera un nuevo documento de cobro juridico
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="cobroJuridicos">Lista de los creditos a realizar cobro juridico</param>
        /// <param name="NUevoEstadoCart">El nuevo estado en que queda el cliente</param>
        /// <returns></returns>
        public List<DTO_SerializedObject> EnvioCobroJuridico(int documentID, string actFlujoID, string clienteID, DTO_ccCreditoDocu cobroJuridico,
            List<DTO_ccSaldosComponentes> saldosComponentes, DateTime fechaDoc, DateTime fechaMvto, TipoEstadoCartera nuevoEstadoCart)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.EnvioCobroJuridico(this._userChannel, documentID, actFlujoID, clienteID, cobroJuridico, saldosComponentes,
                    fechaDoc, fechaMvto, nuevoEstadoCart);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae todos los registros del coAuxiliar que tengan cobro juridico y no existan en ccCJHistorico
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_CobroJuridicoAuxiliar> GetCobroJuridicoFromAuxiliar()
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetCobroJuridicoFromAuxiliar(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que agrega info a ccCJHistorico
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="cobroJuridicos">resumen de los cobrosJuridicos a guardar</param>
        /// <returns></returns>
        public List<DTO_TxResult> ccCJHistorico_Add(int documentID, List<DTO_CobroJuridicoAuxiliar> cobroJuridicos)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ccCJHistorico_Add(this._userChannel, documentID, cobroJuridicos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Revierte un envío a CJ
        /// </summary>
        /// <param name="documentID">Documento que genera la reversión</param>
        /// <param name="numeroDoc">Numero doc del pago a revertir</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult EnvioCJ_Revertir(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.EnvioCJ_Revertir(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que genera un nuevo documento de sentencia judicial
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="clienteID">Cliente de la sentencia</param>
        /// <param name="creditoSentencia">credito</param>
        /// <param name="credComponentes">componentes</param>
        /// <param name="fechaDoc">fecha del documento</param>
        /// <returns></returns>
        public List<DTO_SerializedObject> SentenciaJudicial_Add(int documentID, string actFlujoID, DTO_ccCreditoDocu creditoSentencia,TipoEstadoCartera nuevoEst,DateTime fechaDoc, DateTime fechaSentencia)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SentenciaJudicial_Add(this._userChannel, documentID, actFlujoID,creditoSentencia, nuevoEst,fechaDoc,fechaSentencia);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }
        #endregion

        #region Reintegros

        #region Pagos Especiales

        /// <summary>
        /// Funcion que trae los creditos para el reintegro a los clientes
        /// </summary>
        /// <param name="componenteCarteraID">Componente que se esta consultando</param>
        /// <returns>Retorna una lista con los creditos que poseen saldo en el componente especificado</returns>
        public DTO_SerializedObject PagosEspeciales_GetByComponente(string actFlujoID, string componenteCarteraID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.PagosEspeciales_GetByComponente(this._userChannel, actFlujoID, componenteCarteraID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que trae los creditos para la aprobacion del reintegro a los clientes
        /// </summary>
        /// <param name="componenteCarteraID">Componente que se esta consultando</param>
        /// <param name="actFlujoID">Actividad de flujo del documento</param>
        /// <returns>Retorna una lista con los creditos que poseen saldo en el componente especificado</returns>
        public DTO_SerializedObject PagosEspeciales_GetAprobByComponente(string actFlujoID, string componenteCarteraID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.PagosEspeciales_GetAprobByComponente(this._userChannel, actFlujoID, componenteCarteraID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que genera un nuevo documento de reintegro a clientes con su detalle
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="reintegrosClientes">Lista de los reintegros a realizar</param>
        /// <returns></returns>
        public List<DTO_SerializedObject> PagosEspeciales_Aprobar(int documentID, string actFlujoID, string headerRsx, List<DTO_ccReintegroClienteDeta> reintegrosClientes)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.PagosEspeciales_Aprobar(this._userChannel, documentID, actFlujoID, headerRsx, reintegrosClientes);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Reintegros

        /// <summary>
        /// Funcion que trae los terceros para el reintegro a los clientes
        /// </summary>
        /// <param name="cuentaID">Cuenta que se esta consultando</param>
        /// <returns>Retorna una lista con los terceros que poseen saldo en la especificada</returns>
        public List<DTO_ccReintegroClienteDeta> ReintegroClientes_GetByCuenta(string cuentaID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ReintegroClientes_GetByCuenta(this._userChannel, cuentaID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que genera un nuevo documento de reintegro a clientes con su detalle
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="fechaReintegro">Fecha en la que se realiza el reintegro</param>
        /// <param name="reintegrosClientes">Lista de los reintegros a realizar</param>
        /// <param name="vlrTotalReintegro">Valor total del reintegro</param>
        /// <returns></returns>
        public List<DTO_SerializedObject> ReintegroClientes_Add(int documentID, string actFlujoID, List<DTO_ccReintegroClienteDeta> reintegrosClientes, DateTime fechaReintegro,
            decimal vlrTotalReintegro, bool isGiroAsociado, string reintegroSaldo)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ReintegroClientes_Add(this._userChannel, documentID, actFlujoID, reintegrosClientes, fechaReintegro, vlrTotalReintegro, 
                    isGiroAsociado, reintegroSaldo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que trae los creditos para la aprobacion del reintegro a los clientes
        /// </summary>
        /// <param name="componenteCarteraID">Componente que se esta consultando</param>
        /// <param name="actFlujoID">Actividad de flujo del documento</param>
        /// <returns>Retorna una lista con los creditos que poseen saldo en el componente especificado</returns>
        public List<DTO_ccReintegroClienteDeta> ReintegroClientes_GetAprobByCuenta(string actFlujoID, string cuentaID)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ReintegroClientes_GetAprobByCuenta(this._userChannel, actFlujoID, cuentaID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que genera un nuevo documento de reintegro a clientes con su detalle
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="reintegrosClientes">Lista de los reintegros a realizar</param>
        /// <returns></returns>
        public List<DTO_SerializedObject> ReintegroClientes_AprobarGiro(int documentID, string actFlujoID, string headerRsx,
            List<DTO_ccReintegroClienteDeta> reintegrosClientes)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ReintegroClientes_AprobarGiro(this._userChannel, documentID, actFlujoID, headerRsx, reintegrosClientes);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que genera un nuevo documento de reintegro a clientes con su detalle
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="reintegrosClientes">Lista de los reintegros a realizar</param>
        /// <returns></returns>
        public List<DTO_SerializedObject> ReintegroClientes_AprobarAjuste(int documentID, string actFlujoID, string headerRsx,
            List<DTO_ccReintegroClienteDeta> reintegrosClientes)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.ReintegroClientes_AprobarAjuste(this._userChannel, documentID, actFlujoID, headerRsx, reintegrosClientes);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Sustituciones

        /// <summary>
        /// Valida que la información basica de la sustitución de créditos
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="data">Información a migrar</param>
        /// <returns></returns>
        public DTO_TxResult SustitucionCreditos_Validar(int documentID, DateTime fecha, ref List<DTO_ccSustitucionCreditos> data)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SustitucionCreditos_Validar(this._userChannel, documentID, fecha, ref data);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Procesa la sustitución de créditos
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="data">Información a migrar</param>
        /// <returns></returns>
        public DTO_TxResult SustitucionCreditos_Procesar(int documentID, DateTime fecha, List<DTO_ccSustitucionCreditos> data)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SustitucionCreditos_Procesar(this._userChannel, documentID, fecha, data);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Liquidacion Comisiones

        /// <summary>
        /// Funcion que trae la lista con las liquidaciones de los asesores
        /// </summary>
        /// <returns>Retorna una lista con los asesores para liquidar las comisiones </returns>
        public List<DTO_ccComisionDeta> LiquidacionComisionesCartera_GetForLiquidacion(DateTime fecha)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.LiquidacionComisionesCartera_GetForLiquidacion(this._userChannel,fecha);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que trae la lista con las liquidaciones de los asesores
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo del documento</param>
        /// <returns>Retorna una lista con las liquidaciones de comision para aprobacion </returns>
        public List<DTO_ccComisionDeta> LiquidacionComisionesCartera_GetForAprobacion(string actFlujoID,DateTime Periodo)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.LiquidacionComisionesCartera_GetForAprobacion(this._userChannel, actFlujoID,Periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que genera un nuevo documento de venta de cartera
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="liquidaComisiones">DTO con la informacion de las liquidaciones a procesar</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        public List<DTO_SerializedObject> LiquidacionComisionesCartera_Add(int documentID, string actFlujoID, DTO_LiquidacionComisiones liquidaComisiones)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.LiquidacionComisionesCartera_Add(this._userChannel, documentID, actFlujoID, liquidaComisiones);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Realiza la aprobacion de la liquidacion de las comisiones
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="documentID">Documento que ejecuta la transaccion</param>
        /// <param name="comisionesDeta">Lista de las comisiones</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> LiquidacionComisionesCartera_AprobarRechazar(int documentID, string actFlujoID, List<DTO_ccComisionDeta> comisionesDeta)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.LiquidacionComisionesCartera_AprobarRechazar(this._userChannel, documentID, actFlujoID, comisionesDeta);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Cartera Financiera

        #region Cierres

        /// <summary>
        /// Realiza el proceso de cierre mensual
        /// </summary>
        public DTO_TxResult Proceso_CierreMesCarteraFin(int documentID, DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Proceso_CierreMesCarteraFin(this._userChannel, documentID, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Realiza el proceso de amortizacion mensual
        /// </summary>
        public DTO_TxResult Proceso_AmortizacionMensual(int documentID, DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Proceso_AmortizacionMensual(this._userChannel, documentID, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Realiza el proceso de Cartera en Administracion
        /// </summary>
        public DTO_TxResult Proceso_CarteraAdministracion(int documentID, DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Proceso_CarteraAdministracion(this._userChannel, documentID, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Polizas

        /// <summary>
        /// Obtiene las polizas
        /// </summary>
        /// <param name="poliza">filtro</param>
        /// <returns><Retorna la info filtrada/returns>
        public void PolizaEstado_Upd(DTO_ccPolizaEstado poliza)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                this.CarteraService.PolizaEstado_Upd(this._userChannel, poliza);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }
        
        /// <summary>
        /// Elimina la poliza pedida
        /// </summary>
        /// <param name="terceroID">tercero de la poliza</param>
        /// <param name="poliza">Nro de la poliza</param>
        /// <returns></returns>
        public void PolizaEstado_Delete(string terceroID, string poliza)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                this.CarteraService.PolizaEstado_Delete(this._userChannel, terceroID,poliza);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene las polizas
        /// </summary>
        /// <param name="poliza">filtro</param>
        /// <returns><Retorna la info filtrada/returns>
        public List<DTO_ccPolizaEstado> PolizaEstado_GetByParameter(DTO_ccPolizaEstado poliza)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.PolizaEstado_GetByParameter(this._userChannel, poliza);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Renueva una póliza
        /// </summary>
        /// <documentID>Identificador del socumento</documentID>
        /// <ctrlCredito>Documento control relacionado con el crédito</ctrlCredito>
        /// <creditoPoliza>Credito con las actualizaciones de la poliza</creditoPoliza>
        /// <cuota1Pol>Primera cuota del crédito asignado a la poliza</cuota1Pol>
        /// <cuotasPoliza>Plan de pagos por cada cuota para la poliza</cuotasPoliza>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult RenovacionPoliza(int documentID, int? numDocSolicitud, DTO_ccCreditoDocu credito, DTO_PlanDePagos planPagos, DTO_ccPolizaEstado poliza)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RenovacionPoliza(this._userChannel, documentID, numDocSolicitud, credito, planPagos, poliza);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Registra o actualiza las polizas de cartera
        /// </summary>
        /// <param name="documentID">Documento actual</param>
        /// <param name="polizaEstado">Encabezado</param>
        /// <param name="detallePoliza">Detalle</param>
        /// <param name="insideAnotherTx">Indica es otra transaccion</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult RegistroPoliza(int documentID, byte tipoMvto, DTO_ccPolizaEstado polizaEstado)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RegistroPoliza(this._userChannel, documentID, tipoMvto, polizaEstado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene las polizas de cartera para pagos
        /// </summary>
        /// <returns>Lista</returns>
        public List<DTO_ccPolizaEstado> Poliza_GetForPagos()
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Poliza_GetForPagos(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Realiza los pagos de poliza con autorizacion de Giro(CxP)
        /// </summary>
        /// <param name="documentID">doc actual</param>
        /// <param name="pagos">lista de pagos</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult PagoPolizasCartera(int documentID, DateTime fechaDoc, List<DTO_ccPolizaEstado> pagos, string aseguradora)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.PagoPolizasCartera(this._userChannel, documentID,fechaDoc, pagos, aseguradora);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la ultima poliza de un crédito en cobro jurídico
        /// </summary>
        /// <param name="numDocCredito">Identificador único del crédito</param>
        /// <returns>Información de la póliza</returns>
        public DTO_ccPolizaEstado PolizaEstado_GetLastPoliza(int numDocCredito, int libranza)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.PolizaEstado_GetLastPoliza(this._userChannel, numDocCredito, libranza);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la info de un credito segun el cliente
        /// </summary>
        /// <param name="cliente">Identificador del cliente</param>
        /// <returns><Retorna la info de un credito/returns>
        public List<DTO_ccCreditoDocu> GetCreditosForRenovacionPoliza(string cliente)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GetCreditosForRenovacionPoliza(this._userChannel, cliente);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Renueva una póliza de cobro Juridico
        /// </summary>
        /// <documentID>Identificador del socumento</documentID>
        /// <poliza>Poliza a renovar</cuotasPoliza>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult CobroJuridicoRenovacionPoliza_Add(int documentID, DTO_ccPolizaEstado poliza)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.CobroJuridicoRenovacionPoliza_Add(this._userChannel,documentID, poliza);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Revierte la renovacion de poliza
        /// </summary>
        /// <param name="documentID">Documento que genera la reversión</param>
        /// <param name="numeroDoc">Numero doc del pago a revertir</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult RenovacionPoliza_Revertir(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RenovacionPoliza_Revertir(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Creación Créditos

        /// <summary>
        /// Realiza el proceso de liquidacion de cartera financiera
        /// </summary>
        /// <param name="lineaCredID">Identificador de la linea de credito</param>
        /// <param name="valorCredito">Valor solicitado para el credito</param>
        /// <param name="valorPoliza">Valor solicitado para la poliza</param>
        /// <param name="valorFuturo">Valor Futuro de la solicitud solo para cooperativas</param>
        /// <param name="vlrGiro">Valor a girar</param>
        /// <param name="plazoCredito">Plazo del credito solicitado</param>
        /// <param name="plazoPoliza">PLazo de la poliza</param>
        /// <param name="edad"> Edad de la persona que solicita el credito</param>
        /// <param name="fechaLiquida">Fecha en la cual se solicita el credito</param>
        /// <param name="fechaCuota1">Fecha de la primera cuota</param>
        /// <param name="interesCredito">Tasa de interes que se aplica para el credito</param>
        /// <param name="interesPoliza">Tasa de interes que se aplica a la poliza</param>
        /// <param name="cta1Poliza">Cuota en la que debe empezar la poliza</param>
        /// <param name="liquidaAll">Indica si debe liquidar Credito y Poliza, o solo Poliza</param>
        /// <returns>Retorna un objeto TxResult si se presenta un error, de lo contrario devuelve un objeto de tipo DTO_PlanDePagos</returns>
        public DTO_SerializedObject GenerarPlanPagosFinanciera(string lineaCredID, int valorCredito, int valorPoliza, int valorFuturo, int vlrGiro, int plazoCredito, int plazoPoliza,
            int edad, DateTime fechaLiquida, DateTime fechaCuota1, decimal interesCredito, decimal interesPoliza, int cta1Poliza, int vlrCuotaPol, bool liquidaAll,
            List<DTO_Cuota> cuotasExtras, Dictionary<string, decimal> compsNuevoValor, int numDocCredito, string tipoCredito, bool excluyeCompInvisibleInd)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.GenerarPlanPagosFinanciera(this._userChannel, lineaCredID, valorCredito, valorPoliza, valorFuturo, vlrGiro, plazoCredito, plazoPoliza, edad,
                        fechaLiquida, fechaCuota1, interesCredito, interesPoliza, cta1Poliza, vlrCuotaPol, liquidaAll, cuotasExtras, compsNuevoValor, numDocCredito, tipoCredito, excluyeCompInvisibleInd);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza la informacion de la vaibilidad
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccione</param>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="solicitudes">Solicitudes que se estan modificando</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> SolicitudFin_AprobarRechazar(int documentID, string actFlujoID, List<DTO_ccSolicitudDocu> solicitudes, List<DTO_ccSolicitudAnexo> anexos)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.SolicitudFin_AprobarRechazar(this._userChannel, documentID, actFlujoID, solicitudes, anexos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Pagos

        /// <summary>
        /// Valida que la información basica de la migracion nomina
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="fechaNomina">Fecha de los recaudos</param>
        /// <param name="data">Información a migrar</param>
        /// <returns></returns>
        public DTO_TxResult RecaudosMasivosFin_Validar(int documentID, DateTime fechaNomina, string pagaduriaID, ref List<DTO_ccIncorporacionDeta> data)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RecaudosMasivosFin_Validar(this._userChannel, documentID, fechaNomina, pagaduriaID, ref data);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Procesa la migracion de nomina
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="centroPagoID">Identificador del centro de pago</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="data">Información a migrar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult RecaudosMasivosFin_Procesar(int documentID, DateTime periodo, List<DTO_ccIncorporacionDeta> data, string pagaduria)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RecaudosMasivosFin_Procesar(this._userChannel, documentID, periodo, data,pagaduria);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la relación de pagos de un recauso masivo
        /// </summary>
        /// <param name="data">Datos a migrar</param>
        /// <returns>Retorna la lista de resultados</returns>
        public DataTable RecaudosMasivos_GetRelacionPagos(int documentID, List<DTO_ccIncorporacionDeta> data)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.RecaudosMasivos_GetRelacionPagos(this._userChannel, documentID, data);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        #endregion

        #region Cobro Juridico
        /// <summary>
        /// Actualiza la info del cobro juridico
        /// </summary>
        /// <param name="fechaCorte">Fecha de corte</param>
        /// <param name="libranza">Identificador de la libranza</param>
        public void ccCJHistorico_RecalcularInteresCJ(DateTime fechaCorte, int libranza)
        {
            try
            {
               this.CreateService(typeof(ICarteraService));
               this.CarteraService.ccCJHistorico_RecalcularInteresCJ(this._userChannel, fechaCorte, libranza);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }
        #endregion

        #endregion

        #endregion

        #region ContabilidadService

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción de un usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ConsultarProgresoCont(int documentID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.ConsultarProgreso(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        #endregion

        #region Cont - General y Procesos

        /// <summary>
        /// Genera los comprobantes y saldos para el ajuste en cambio
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <param name="AreaFuncionalID">Area funcional desde ka cual se ejecuta el proceso (la del usuario)</param>
        /// <param name="ndML">Numero de documento de glDocumentoControl ML</param>
        /// <param name="ndME">Numero de documento de glDocumentoControl ME</param>
        /// <returns></returns>
        public Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>> Proceso_AjusteEnCambio(int documentID, string actividadFlujoID, string areaFuncionalID,
            DateTime periodo, string libroID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Proceso_AjusteEnCambio(this._userChannel, documentID, actividadFlujoID, areaFuncionalID, periodo, libroID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Procesa el ajuste en cambio para un periodo seleccionado
        /// </summary>
        /// <param name="comps">Comprobantes para aprobar</param>
        /// <param name="periodo">Periodo del ajuste</param>
        /// <returns>Retorna el resultado de las operaciones</returns>
        public List<DTO_TxResult> Proceso_ProcesarBalancePreliminar(int documentID, string actividadFlujoID, List<DTO_ComprobanteAprobacion> comps,
            DateTime periodo, string libroID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Proceso_ProcesarBalancePreliminar(this._userChannel, documentID, actividadFlujoID, comps, periodo, libroID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza el valor de la cuenta alterna en las tablas de coBalance coCuentaSaldo y coAuxiliar
        /// </summary>
        public DTO_TxResult Proceso_CuentaAlterna(int documentID, string actividadFlujoID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Proceso_CuentaAlterna(this._userChannel, documentID, actividadFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Proceso de prorrateo IVA
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Proceso_ProrrateoIVA(int documentID, string actividadFlujoID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Proceso_ProrrateoIVA(this._userChannel, documentID, actividadFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Proceso para consolidar balances entre empresas
        /// </summary>
        /// <param name="documentID">Identificador del documento que genera el proceso</param>
        /// <param name="list">Lista de empresas a consolidar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_TxResult> Proceso_ConsolidacionBalances(int documentID, string actividadFlujoID, List<DTO_ComprobanteConsolidacion> list)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Proceso_ConsolidacionBalances(this._userChannel, documentID, actividadFlujoID, list);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Reclasifica un libro fiscal 
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la transaccion</param>
        /// <param name="libroID">Identificador del libro fiscal</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public DTO_TxResult Proceso_ReclasificacionLibros(int documentID, DateTime periodoID, string libroID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Proceso_ReclasificacionLibros(this._userChannel, documentID, periodoID, libroID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        #endregion

        #region Ajuste Comprobantes

        /// <summary>
        /// Obtiene un auxiliar a partir de las llaves
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public DTO_Comprobante AjusteComprobante_Get(DateTime periodo, string comprobanteID, int compNro)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.AjusteComprobante_Get(this._userChannel, periodo, comprobanteID, compNro);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Ajusta un comprobante existente
        /// </summary>
        /// <param name="documentID">documento Id</param>
        /// <param name="comp">Comprobante a ajustar</param>
        /// <param name="insideAnotherTx">determina si viene de una transaccion</param>
        public DTO_TxResult AjusteComprobante_Generar(int documentID, string actividadFlujoID, DTO_Comprobante comp)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.AjusteComprobante_Generar(this._userChannel, documentID, actividadFlujoID, comp);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Elimina un ajuste de comprobante
        /// </summary>
        /// <param name="numeroDoc"></param>
        /// <returns></returns>
        public DTO_TxResult AjusteComprobante_Eliminar(int documentID, string actividadFlujoID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.AjusteComprobante_Eliminar(this._userChannel, documentID, actividadFlujoID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de los ajustes pendientes de aprobar
        /// </summary>
        /// <returns>Retorna una lista de comprobantes de ajuste</returns>
        public List<DTO_ComprobanteAprobacion> AjusteComprobante_GetPendientes(string actividadFlujoID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.AjusteComprobante_GetPendientes(this._userChannel, actividadFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Recibe una lista de probobantes paar aprobar o rechazar
        /// </summary>
        /// <param name="comps">Comprobantes que se deben aprobar o rechazar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> AjusteComprobante_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_ComprobanteAprobacion> comps)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.AjusteComprobante_AprobarRechazar(this._userChannel, documentID, actividadFlujoID, comps);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        #endregion

        #region Comprobante

        #region AuxiliarPre

        /// <summary>
        /// Indica si hay un auxiliarPre
        /// </summary>
        /// <param name="empresaID">Codigo de la empresa</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public bool ComprobantePre_Exists(int documentID, DateTime periodo, string comprobanteID, int compNro)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.ComprobantePre_Exists(this._userChannel, documentID, periodo, comprobanteID, compNro);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Indica si hay un comprobante en auxiliarPre
        /// </summary>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <returns>Retorna un entero con la cantidad de comprobantes que hay en aux con un comprobanteID</returns>
        public bool ComprobanteExistsInAuxPre(string comprobanteID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                bool result = this.ContabilidadService.ComprobanteExistsInAuxPre(this._userChannel, comprobanteID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega un comprobante
        /// </summary>
        /// <param name="comprobante">Comprobante con los datos</param>
        /// <param name="areaFuncionalID">Identificador del area funcional</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <param name="numeroDoc">Pk de glDocumentoControl (Null: El comprobante es el encargado de generar el registro)</param>
        /// <param name="numComp">Numero del comprobante generado (en caso de ser nuevo)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult ComprobantePre_Add(int documentoID, ModulesPrefix mod, DTO_Comprobante comprobante, string areaFuncionalID, string prefijoID, int? numeroDoc, DTO_coDocumentoRevelacion revelacion)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.ComprobantePre_Add(this._userChannel, documentoID, mod, comprobante, areaFuncionalID, prefijoID, numeroDoc, revelacion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Elimina un auxiliar (pre) y crea el registro vacio
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        public void ComprobantePre_Delete(int documentID, string actividadFlujoID, DateTime periodo, string comprobanteID, int compNro)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                this.ContabilidadService.ComprobantePre_Delete(this._userChannel, documentID, actividadFlujoID, periodo, comprobanteID, compNro);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Envia para aprobacion un comprobante
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        public DTO_SerializedObject ComprobantePre_SendToAprob(int documentID, string actividadFlujoID, ModulesPrefix currentMod, DateTime periodo, string comprobanteID, int compNro, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.ComprobantePre_SendToAprob(this._userChannel, documentID, actividadFlujoID, currentMod, periodo, comprobanteID, compNro, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de comprobantes pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_ComprobanteAprobacion> ComprobantePre_GetPendientesByModulo(ModulesPrefix mod, string actividadFlujoID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.ComprobantePre_GetPendientesByModulo(this._userChannel, mod, actividadFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        #endregion

        #region Auxiliar

        /// <summary>
        /// Obtiene un auxiliar con correspondiente IdentificadorTR y periodo anterior o igual a correspondiente Periodo
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="identTR">IdentificadorTR</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_BitacoraSaldo> Comprobante_GetByIdentificadorTR(DateTime periodo, long identTR)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Comprobante_GetByIdentificadorTR(this._userChannel, periodo, identTR);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el numero de registros de un comprobante
        /// </summary>
        /// <param name="allData">Dice si trae todos los datos incluyendo la contrapartida o solo los creados por el usuario</param>
        /// <param name="isPre">Indica si debe traer los datos de coAuxiliarPre (si es falso los trae de coAuxiliar)</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public int Comprobante_Count(bool allData, bool isPre, DateTime periodo, string comprobanteID, int compNro, DTO_glConsulta consulta = null)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Comprobante_Count(this._userChannel, allData, isPre, periodo, comprobanteID, compNro, consulta);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene un auxiliar a partir de las llaves (si tiene el numero de documento busca las CxP)
        /// </summary>
        /// <param name="numDoc">Numero de documento de busqueda</param>
        /// <param name="isPre">Indica si debe traer los datos de coAuxiliarPre (si es falso los trae de coAuxiliar)</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public DTO_Comprobante Comprobante_GetAll(int numDoc, bool isPre, DateTime periodo, string comprobanteID, int? compNro)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Comprobante_GetAll(this._userChannel, numDoc, isPre, periodo, comprobanteID, compNro);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene un auxiliar a partir de las llaves
        /// </summary>
        /// <param name="allData">Dice si trae todos los datos incluyendo la contrapartida o solo los creados por el usuario</param>
        /// <param name="isPre">Indica si debe traer los datos de coAuxiliarPre (si es falso los trae de coAuxiliar)</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public DTO_Comprobante Comprobante_Get(bool allData, bool isPre, DateTime periodo, string comprobanteID, int compNro, int? pageSize, int? pageNum, DTO_glConsulta consulta = null)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Comprobante_Get(this._userChannel, allData, isPre, periodo, comprobanteID, compNro, pageSize, pageNum, consulta);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// OBtiene los documentos de movimiento de cartera
        /// </summary>
        /// <param name="clienteMov">Filtro del cliente</param>
        /// <param name="libranza">Filtro del libranza</param>
        /// <param name="fechaInt">Filtro del fechaIni</param>
        /// <param name="fechaFin">Filtro del fechaFin</param>
        /// <param name="pagaduria">Filtro de la pagaduria</param>
        /// <param name="tipoMovimiento">Filtro del tipoMov</param>
        /// <returns>Lista de documentos</returns> 
        public List<DTO_QueryCarteraMvto> Cartera_GetMvto(string clienteMov, string NroCredito, DateTime fechaInt, DateTime fechaFin, int tipoMovimiento, int tipoAnulado)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.Cartera_GetMvto(this._userChannel, clienteMov, NroCredito, fechaInt, fechaFin, tipoMovimiento, tipoAnulado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que trae los movimientos de cartera de una libranza
        /// </summary>
        /// <param name="libranza">libranza</param>
        /// <returns>Lista</returns>
        public List<DTO_QueryCarteraMvto> CarteraMvto_QueryByLibranza(int libranza)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.CarteraService.CarteraMvto_QueryByLibranza(this._userChannel,libranza);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        /// Recibe una lista de probobantes paar aprobar o rechazar
        /// </summary>
        /// <param name="comps">Comprobantes que se deben aprobar o rechazar</param>
        /// <param name="userId">Usuario que realiza la transaccion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Comprobante_AprobarRechazar(int documentID, string actividadFlujoID, ModulesPrefix currentMod, List<DTO_ComprobanteAprobacion> comps, bool updDocCtrl, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Comprobante_AprobarRechazar(this._userChannel, documentID, actividadFlujoID, currentMod, comps, updDocCtrl, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera una lista de comprobantes
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la operacion (los que se van a guardar en glDocumentoControl)</param>
        /// <param name="periodo">Periodo de migración</param>
        /// <param name="comps">Lista de comprobantes</param>
        /// <param name="areaFuncionalID">Area funcional del usuario</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="borraInfoPeriodo">Inidca si se debe borrar la información del periodo</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_TxResult> Comprobante_Migracion(int documentID, DateTime periodo, List<DTO_Comprobante> comps, string areaFuncionalID,
            string prefijoID, bool borraInfoPeriodo)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Comprobante_Migracion(this._userChannel, documentID, periodo, comps, areaFuncionalID, prefijoID, borraInfoPeriodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Realiza la reclasificacion de saldos
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="numeroDoc">Numero de documento (PK)</param>
        /// <param name="proyectoID">Identificador del nuevo proyecto</param>
        /// <param name="ctoCostoID">Identificador del centro de costo</param>
        /// <param name="lgID">Identificador del lugar geografico</param>
        /// <param name="obs">Observacion del documento</param>
        /// <returns>Retorna el resultado del proceso</returns>
        public DTO_TxResult Comprobante_ReclasificacionSaldos(int documentID, string actividadFlujoID, int numeroDoc, string proyectoID, string ctoCostoID, string lgID, string obs)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Comprobante_ReclasificacionSaldos(this._userChannel, documentID, actividadFlujoID, numeroDoc, proyectoID, ctoCostoID, lgID, obs);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae el valor de las cuentas de costo
        /// </summary>
        /// <param name="numeroDoc">numero de documento</param>
        /// <returns></returns>
        public decimal Comprobante_GetValorByCuentaCosto(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Comprobante_GetValorByCuentaCosto(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae las transferencias bancarias por tercero
        /// </summary>
        /// <param name="terceroID">tercero a validar</param>
        /// <param name="docTercero">numero de la factura</param>
        /// <returns>lista de comp</returns>
        public DTO_Comprobante Comprobante_GetTransfBancariaByTercero(string terceroID, string docTercero)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Comprobante_GetTransfBancariaByTercero(this._userChannel, terceroID, docTercero);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="periodoInicial">periodo Inicial</param>
        /// <param name="periodoFinal">periodo final</param>
        /// <param name="filter">filtro</param>
        /// <returns>Lista de auxiliares</returns>
        public List<DTO_QueryMvtoAuxiliar> Comprobante_GetAuxByParameter(DateTime? periodoInicial, DateTime? periodoFinal, DTO_QueryMvtoAuxiliar filter)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Comprobante_GetAuxByParameter(this._userChannel, periodoInicial, periodoFinal, filter);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="periodoInicial">periodo</param>
        /// <param name="lugarxDef">lugarGeo</param>
        /// <returns>Lista de auxiliares</returns>
        public List<DTO_PagoImpuesto> Comprobante_GetAuxForImpuesto(DateTime periodoFilter)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Comprobante_GetAuxForImpuesto(this._userChannel, periodoFilter);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }
        #endregion

        #endregion

        #region Cierres

        /// <summary>
        /// Obtiene ultimo mes cerrado
        /// </summary>
        /// <param name="mod">Modulo de consulta</param>
        /// <returns>Retorna periodo o null si no existe alguno</returns>
        public DateTime? GetUltimoMesCerrado(ModulesPrefix mod)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var res = this.ContabilidadService.GetUltimoMesCerrado(this._userChannel, mod);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Indica si el periodo enviado es el ultimo mes cerrado
        /// </summary>
        /// <param name="mod">Modulo de consulta</param>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>Retorna True si el aperiodo se puede abrir de lo contrario false</returns>
        public bool UltimoMesCerrado(ModulesPrefix mod, DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var res = this.ContabilidadService.UltimoMesCerrado(this._userChannel, mod, periodo);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Abre un nuevo mes
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="periodo">Periodo para abrir</param>
        /// <param name="modulo">Modulo que se desea abrir</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Proceso_AbrirMes(int documentID, DateTime periodo, ModulesPrefix modulo)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var res = this.ContabilidadService.Proceso_AbrirMes(this._userChannel, documentID, periodo, modulo);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Hace el cierre mensual
        /// </summary>
        /// <param name="empresa">Empresa</param>
        /// <param name="periodo">Periodo a cerrar</param>
        /// <param name="modulo">Módulo a cerrar</param>
        /// <param name="userId">Id del usuario</param>
        /// <returns></returns>
        public DTO_TxResult Proceso_CierrePeriodo(int documentID, DateTime periodo, ModulesPrefix modulo)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var res = this.ContabilidadService.Proceso_CierrePeriodo(this._userChannel, documentID, periodo, modulo);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Hace el cierre anual
        /// </summary>
        /// <param name="empresa">Empresa</param>
        /// <param name="year">Año a cerrar</param>
        /// <param name="userId">Usuario que hace el cierre</param>
        /// <returns></returns>
        public Tuple<DTO_TxResult, DTO_ComprobanteAprobacion> Proceso_CierreAnual(int documentID, string actividadFlujoID, string areaFuncionalID, int year, string libroID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var res = this.ContabilidadService.Proceso_CierreAnual(this._userChannel, documentID, actividadFlujoID, areaFuncionalID, year, libroID);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Carga la informacion del cierre mensual
        /// </summary>
        /// <param name="año">Año en que se hizo el cierre</param>
        /// <returns></returns>
        public List<DTO_coCierreMes> coCierreMes_GetAll(Int16 año)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.ContabilidadService.coCierreMes_GetAll(this._userChannel, año);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }

        /// <summary>
        ///  Carga la información para hacer un cierre Mensual
        /// </summary>
        /// <param name="filter">Filtro</param>
        /// <returns></returns>
        public List<DTO_coCierreMes> coCierreMes_GetByParameter(DTO_coCierreMes filter, RompimientoSaldos? romp1 = null, RompimientoSaldos? romp2 = null)
        {
            try
            {
                this.CreateService(typeof(ICarteraService));
                var result = this.ContabilidadService.coCierreMes_GetByParameter(this._userChannel, filter, romp1, romp2);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICarteraService));
                throw ex;
            }
        }
        #endregion

        #region CruceCuentas(Ajuste Saldos)

        /// <summary>
        /// Creal el documento Ajuste y asociar en glDocumentoControl
        /// </summary>
        /// <param name="ctrl">referencia documento</param>
        /// <param name="ajuste">documento Ajuste</param>
        /// <param name="comp">Comprobante</param>
        /// <returns></returns>
        public DTO_TxResult CruceCuentas_Ajustar(int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_coDocumentoAjuste ajuste,
            DTO_Comprobante comp)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.CruceCuentas_Ajustar(this._userChannel, documentID, actividadFlujoID, ctrl, ajuste, comp);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        #endregion

        #region Distribucion Comprobante

        /// <summary>
        /// Obtiene la lista de registros de la distribucion
        /// </summary>
        /// <returns></returns>
        public List<DTO_coCompDistribuyeTabla> ComprobanteDistribucion_GetDistribucion()
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.ComprobanteDistribucion_GetDistribucion(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene la lista de registros de las exclusiones
        /// </summary>
        /// <returns></returns>
        public List<DTO_coCompDistribuyeExcluye> ComprobanteDistribucion_GetExclusiones()
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.ComprobanteDistribucion_GetExclusiones(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza la informacion para la distribucion de comprobantes
        /// </summary>
        /// <param name="documentoId">Identificador del documento</param>
        /// <param name="tablas">Registros de distribucion</param>
        /// <param name="excluyen">Registros de exclucion</param>
        /// <returns></returns>
        public DTO_TxResult ComprobanteDistribucion_Update(int documentID, List<DTO_coCompDistribuyeTabla> tablas, List<DTO_coCompDistribuyeExcluye> excluyen)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.ComprobanteDistribucion_Update(this._userChannel, documentID, tablas, excluyen);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene la lista de registros de la distribucion
        /// </summary>
        /// <returns></returns>
        public List<DTO_coCompDistribuyeTabla> ComprobanteDistribucion_GetForProcess()
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.ComprobanteDistribucion_GetForProcess(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera los preliminares y revierto los comprobantes
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="origenes">Lista de comprobantes que se deben distribuir</param>
        /// <param name="periodoIni">Periodo Inicial</param>
        /// <param name="periodoFin">Periodo Final</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>> ComprobanteDistribucion_GenerarPreliminar(int documentID, string actividadFlujoID, List<DTO_coCompDistribuyeTabla> origenes,
            DateTime periodoIni, DateTime periodoFin, string libroID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var res = this.ContabilidadService.ComprobanteDistribucion_GenerarPreliminar(this._userChannel, documentID, actividadFlujoID, origenes, periodoIni, periodoFin, libroID);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        #endregion

        #region Impuestos

        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="mod">Modulo que realiza la consulta</param>
        /// <param name="valor">Valor sobre el cual se esta trabajando</param>
        /// <param name="tercero">Tercero que esta ejecutando la consulta</param>
        /// <param name="conceptoCargoID">Identificador del concepto de cargo</param>
        /// <param name="lugarGeoID">Identificador del lugar geografico</param>
        /// <param name="operacionID">Identificador de la operacion</param>
        /// <param name="lineaPresID">Identificador de la linea presupuestal</param>
        /// <returns>Retorna una lista de cuentas</returns>
        public List<DTO_SerializedObject> LiquidarImpuestos(ModulesPrefix mod, DTO_coTercero tercero, string cuentaCosto, string conceptoCargoID, string operacionID, string lugarGeoID, string lineaPresID, decimal valor)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.LiquidarImpuestos(this._userChannel, mod, tercero, cuentaCosto, conceptoCargoID, operacionID, lugarGeoID, lineaPresID, valor);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la lista de declaracion de impuestos para un periodo
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>Retorna la lista de declaraciones</returns>
        public List<DTO_DeclaracionImpuesto> DeclaracionesImpuestos_Get(DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.DeclaracionesImpuestos_Get(this._userChannel, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la lista de renglones de una declaracion
        /// </summary>
        /// <param name="numeroDoc">Numero de documento (si ya fue procesado previamente)</param>
        /// <param name="impuestoID">Identificador de la declaracion</param>
        /// <param name="mesDeclaracion">Mes de declaracion</param>
        /// <param name="añoDeclaracion">Año de declaracion</param>
        /// <returns>Retorna la lista de renglones</returns>
        public List<DTO_coImpDeclaracionDetaRenglon> DeclaracionesRenglones_Get(int numeroDoc, string impuestoID, short mesDeclaracion, short añoDeclaracion)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.DeclaracionesRenglones_Get(this._userChannel, numeroDoc, impuestoID, mesDeclaracion, añoDeclaracion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Procesa una declaracion
        /// </summary>
        /// <param name="documentID">Identificador del documnto que genera el proceso</param>
        /// <param name="impuestoID">Identificador de la declaracion</param>
        /// <param name="mesDeclaracion">Mes de declaracion</param>
        /// <param name="añoDeclaracion">Año de declaracion</param>
        /// <param name="numeroDoc">Numero de documento (si ya fue procesado previamente)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult ProcesarDeclaracion(int documentID, string impuestoID, short periodoCalendario, short mesDeclaracion, short añoDeclaracion, int? numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.ProcesarDeclaracion(this._userChannel, documentID, impuestoID, periodoCalendario, mesDeclaracion, añoDeclaracion, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae los detalles de un renglon
        /// </summary>
        /// <param name="impuestoID">Identificador de la declaracion</param>
        /// <param name="renglon">Renglon</param>
        /// <param name="mesDeclaracion">Mes de la declaracion</param>
        /// <param name="añoDeclaracion">Año de la declaracion</param>
        /// <returns>Retorna la lista de cuentas del detalle</returns>
        public List<DTO_DetalleRenglon> DetallesRenglon_Get(string impuestoID, string renglon, short mesDeclaracion, short añoDeclaracion)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.DetallesRenglon_Get(this._userChannel, impuestoID, renglon, mesDeclaracion, añoDeclaracion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        #endregion

        #region Mayorizacion

        /// <summary>
        /// Realiza la mayorización de balances de acuerdo a saldos
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="balance">Tipo de balance</param>
        /// <param name="userId">Usuario que realiza la mayorizacion</param>
        /// <param name="empresa">Empresa</param>
        public DTO_TxResult Proceso_Mayorizar(int documentID, DateTime periodo, string tipoBalance)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var res = this.ContabilidadService.Proceso_Mayorizar(this._userChannel, documentID, periodo, tipoBalance);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        #endregion

        #region Reclasificaciones Fiscales

        /// <summary>
        /// Obtiene la lista de registros de la reclasificacion
        /// </summary>
        /// <returns></returns>
        public List<DTO_coReclasificaBalance> ReclasificacionFiscal_GetDistribucion()
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.ReclasificacionFiscal_GetDistribucion(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene la lista de registros de las exclusiones
        /// </summary>
        /// <returns></returns>
        public List<DTO_coReclasificaBalExcluye> ReclasificacionFiscal_GetExclusiones()
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.ReclasificacionFiscal_GetExclusiones(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza la informacion para la reclasificacion de comprobantes
        /// </summary>
        /// <param name="documentoId">Identificador del documento</param>
        /// <param name="tablas">Registros de reclasificacion</param>
        /// <param name="excluyen">Registros de exclucion</param>
        /// <returns></returns>
        public DTO_TxResult ReclasificacionFiscal_Update(int documentID, List<DTO_coReclasificaBalance> tablas, List<DTO_coReclasificaBalExcluye> excluyen)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.ReclasificacionFiscal_Update(this._userChannel, documentID, tablas, excluyen);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene la lista de registros de la reclasificacion
        /// </summary>
        /// <returns></returns>
        public List<DTO_coReclasificaBalance> ReclasificacionFiscal_GetForProcess(string tipoBalanceID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.ReclasificacionFiscal_GetForProcess(this._userChannel, tipoBalanceID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Procesa la reclasificacion
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult ReclasificacionFiscal_Procesar(int documentID, string actividadFlujoID, string tipoBalanceID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var res = this.ContabilidadService.ReclasificacionFiscal_Procesar(this._userChannel, documentID, actividadFlujoID, tipoBalanceID);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        #endregion

        #region Saldos

        /// <summary>
        /// Trae un saldo
        /// </summary>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <param name="tipoBalance">Tipo de balance</param>
        /// <param name="concSaldo">Concepto de saldo</param>
        /// <param name="identificadorTR">Consecutivo del socumento por el cual se va a buscar el saldo</param>
        /// <returns>Retorna un saldo</returns>
        public DTO_coCuentaSaldo Saldo_GetByDocumento(string cuentaID, string concSaldo, long identificadorTR, string balanceTipo)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Saldo_GetByDocumento(this._userChannel, cuentaID, concSaldo, identificadorTR, balanceTipo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la informacion de los saldos en ML o ME
        /// </summary>
        /// <param name="isML">Indica si se deben consultar en ML</param>
        /// <param name="PeriodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta</param>
        /// <returns>Retorna el valor del saldo</returns>
        public decimal Saldo_GetByDocumentoCuenta(bool isML, DateTime PeriodoID, long identificadorTR, string cuentaID, string libroID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.Saldo_GetByDocumentoCuenta(this._userChannel, isML, PeriodoID, identificadorTR, cuentaID, libroID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Verifica si ya hay uno o más conceptos de saldo en coCuentaSaldo
        /// </summary>
        /// <param name="ConceptoSaldoIDNew">Id de concepto saldo nuevo</param>
        /// <param name="conceptoSaldoIDOld">Id de concepto saldo anterior</param>
        /// <param name="cuentaID">Id de la cuenta anterior</param>
        /// <returns>true si existe</returns>
        public bool Saldo_ExistsByConcSaldo(string ConceptoSaldoIDNew, string conceptoSaldoIDOld, string cuentaID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                return this.ContabilidadService.Saldo_ExistsByCtaConcSaldo(this._userChannel, ConceptoSaldoIDNew, conceptoSaldoIDOld, cuentaID);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }

        }

        /// <summary>
        /// Trae la informacion de los saldos en ML o ME
        /// </summary>
        /// <param name="isML">Indica si se deben consultar en ML</param>
        /// <param name="PeriodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta</param>
        /// <returns>Retorna el valor del saldo</returns>
        public decimal Saldo_GetByPeriodoCuenta(bool isML, DateTime PeriodoID, string cuentaID, string libroID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                return this.ContabilidadService.Saldo_GetByPeriodoCuenta(this._userChannel, isML, PeriodoID, cuentaID, libroID);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }

        }

        /// <summary>
        /// Revisa si un documento ha tenido movimientos de saldos despues de su creación
        /// </summary>
        /// <param name="idTR">Identificador del documento</param>
        /// <param name="periodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta del documento</param>
        /// <param name="libroID">Libro de consulta</param>
        /// <returns>Retorna true si ha tenido nuevos movimientos, de lo contrario false</returns>
        public bool Saldo_HasMovimiento(int idTR, DateTime periodoID, DTO_coPlanCuenta cta, string libroID)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                return this.ContabilidadService.Saldo_HasMovimiento(this._userChannel, idTR, periodoID, cta, libroID);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }

        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns>Lista de saldos </returns>
        public List<DTO_coCuentaSaldo> Saldos_GetByParameter(DTO_coCuentaSaldo filter)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                return this.ContabilidadService.Saldos_GetByParameter(this._userChannel, filter);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }

        }

        #endregion

        #region Revelaciones

        /// <summary>
        /// Documento Revelacion
        /// </summary>
        /// <param name="revelacion">objeto Revelacion</param>
        /// <returns></returns>
        public DTO_TxResult DocumentoRevelacion_Add(DTO_coDocumentoRevelacion revelacion)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.DocumentoRevelacion_Add(this._userChannel, revelacion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene un documento revelación por numero de documento
        /// </summary>
        ///<param name="numeroDoc">número de documento</param>
        ///<returns>Revelación</returns>
        public DTO_coDocumentoRevelacion DocumentoRevelacion_Get(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IContabilidadService));
                var result = this.ContabilidadService.DocumentoRevelacion_Get(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IContabilidadService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region CxP - Tesoreria - Facturacion

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción de un usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ConsultarProgresoCFT(int documentID)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.ConsultarProgreso(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        #endregion

        #region CxP

        #region cpAnticipos

        public DTO_cpAnticipo cpAnticipos_GetByEstado(int numeroDoc, EstadoDocControl estado)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.cpAnticipos_GetByEstado(this._userChannel, numeroDoc, estado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Radica ó actualizan un Anticipo en el sistema
        /// </summary>
        /// <param name="empresa">id empresa</param>
        /// <param name="documentID">id documento</param>
        /// <param name="_dtoCtrl">documento control asociado</param>
        /// <param name="_anticipo">documento anticipo</param>
        /// <param name="userID">usuario sistema</param>
        /// <param name="update">true si es actualizar, false si es adicionar</param>
        /// <returns></returns>
        public DTO_SerializedObject cpAnticipos_Guardar(int documentID, DTO_glDocumentoControl _dtoCtrl, DTO_cpAnticipo _anticipo, bool update)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.cpAnticipos_Guardar(this._userChannel, documentID, _dtoCtrl, _anticipo, update);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna el valor total para una lista de anticipos 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tm">Tipo de moneda sobre el cual estan viendo los anticipos</param>
        /// <param name="tc">Tasa de cambio en el dia</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna el valor total de los anticipos</returns>
        public decimal cpAnticipos_GetResumenVal(DateTime periodo, TipoMoneda tm, decimal tc, string terceroID)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.cpAnticipos_GetResumenVal(this._userChannel, periodo, tm, tc, terceroID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna una lista de anticipos 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoMoneda">Tipo de moneda de los cuales hay que traer los anticipos</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <param name="anticipoTarjeta">Indica si es anticipo de tarjeta de credito</param>
        /// <returns>Retorna una lista de anticipos</returns>
        public List<DTO_AnticiposResumen> cpAnticipos_GetResumen(DateTime periodo, TipoMoneda tipoMoneda, string terceroID, bool anticipoTarjeta = false)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.cpAnticipos_GetResumen(this._userChannel, periodo, tipoMoneda, terceroID, anticipoTarjeta);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de causaciones pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_AnticipoAprobacion> cpAnticipos_GetPendientesByModulo(ModulesPrefix mod, string actividadFlujoID)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.cpAnticipos_GetPendientesByModulo(this._userChannel, mod, actividadFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Proceso que aprueba o rechaza un listado de anticipos
        /// </summary>
        /// <param name="comps">listado de anticipos</param>
        /// <param name="userId">usuario responsable</param>
        /// <returns></returns>
        public List<DTO_SerializedObject> cpAnticipos_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_AnticipoAprobacion> comps, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.cpAnticipos_AprobarRechazar(this._userChannel, documentID, actividadFlujoID, comps, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        #endregion

        #region Cuentas X Pagar

        /// <summary>
        /// Agrega una lista de CxP
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="actividadFlujoID">Actividad de flujo</param>
        /// <param name="concSaldoID">Nuevo concepto de saldo para las cuentas</param>
        /// <param name="ctrlList">Lista de documentos</param>
        /// <param name="cxpList">Lista de cuentas por pagar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult CuentasXPagar_Migracion(int documentID, string actividadFlujoID, string concSaldoID, List<DTO_glDocumentoControl> ctrls, List<DTO_cpCuentaXPagar> cxpList)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.CuentasXPagar_Migracion(this._userChannel, documentID, actividadFlujoID, concSaldoID, ctrls, cxpList);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene una cuenta x pagar segun el numero de documento asociado
        /// </summary>
        /// <param name="numeroDoc">numero de documento</param>
        /// <returns></returns>
        public DTO_cpCuentaXPagar CuentasXPagar_Get(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.CuentasXPagar_Get(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Radicar Factura en tabla cpCuentaXPagar y asociar en glDocumentoControl
        /// </summary>
        /// <param name="dtoCtrl">referencia documento</param>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        public object CuentasXPagar_Radicar(int documentID, DTO_glDocumentoControl dtoCtrl, DTO_cpCuentaXPagar cxp, bool mainWindow, bool update, out int numDoc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.CuentasXPagar_Radicar(this._userChannel, documentID, dtoCtrl, cxp, mainWindow, update, out numDoc);
                return result;
            }
            catch (CommunicationException ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Guardar en tabla cpCuentaXPagar
        /// </summary>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        public DTO_TxResult CuentasXPagar_Add(int documentID, DTO_cpCuentaXPagar cxp)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.CuentasXPagar_Add(this._userChannel, documentID, cxp);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Cambiar el Estado del documento y actualiza la informacion de la factura.
        /// </summary>
        /// <param name="dtoCtrl">referencia documento</param>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        public DTO_TxResult CuentasXPagar_Devolver(int documentID, DTO_glDocumentoControl dtoCtrl, DTO_cpCuentaXPagar cxp, bool mainWindow)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.CuentasXPagar_Devolver(this._userChannel, documentID, dtoCtrl, cxp, mainWindow);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Carga la informacion completa de una facturA
        /// </summary>
        /// <param name="terceroID">Identificador del tercero</param>
        /// <param name="documentoNro">Numero del documento(tercero)</param>
        /// <param name="documentoTercero">Documento del tercero</param>
        /// <returns>Retorna la factura</returns>
        public DTO_CuentaXPagar CuentasXPagar_GetForCausacion(int documentID, string terceroID, string documentoTercero, bool chekEstado = true)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.CuentasXPagar_GetForCausacion(this._userChannel, documentID, terceroID, documentoTercero, chekEstado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Causa una cuenta por pagar radicada
        /// </summary>
        /// <param name="cxp">Cuenta por pagar</param>
        /// <param name="comp">Comprobante que se debe agregar</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns></returns>
        public DTO_TxResult CuentasXPagar_Causar(int documentID, DTO_glDocumentoControl ctrl, DTO_cpCuentaXPagar cxp, DTO_Comprobante comp)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.CuentasXPagar_Causar(this._userChannel, documentID, ctrl, cxp, comp);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Causa una cuenta por pagar radicada
        /// </summary>
        /// <param name="listCxP">Lista de Cuenta por pagar</param>
        /// <returns></returns>
        public DTO_TxResult CuentasXPagar_CausarMasivo(int documentID, List<DTO_CuentaXPagar> listCxP)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.CuentasXPagar_CausarMasivo(this._userChannel, documentID, listCxP);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de causaciones pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_CausacionAprobacion> CuentasXPagar_GetPendientesByModulo(ModulesPrefix mod, string actividadFlujoID, bool checkUser = true)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.CuentasxPagar_GetPendientesByModulo(this._userChannel, mod, actividadFlujoID, checkUser);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));

                throw ex;
            }
        }

        /// <summary>
        /// Aprueba o rechazas causacion de facturas
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="comps">lista comprobantes</param>
        /// <param name="updDocCtrl">actualiza el documento control</param>
        /// <param name="createDoc">genera archivo fisico</param>
        /// <param name="batchProgress">progreso proceso</param>
        /// <returns>listado de DT</returns>
        public List<DTO_SerializedObject> CuentasXPagar_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_ComprobanteAprobacion> comps, bool updDocCtrl, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.CuentasXPagar_AprobarRechazar(this._userChannel, documentID, actividadFlujoID, comps, updDocCtrl, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));

                throw ex;
            }
        }

        /// <summary>
        /// Revierte una cuenta por pagar
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="numeroDoc"></param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult CuentasXPagar_Revertir(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.CuentasXPagar_Revertir(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));

                throw ex;
            }
        }

        /// <summary>
        /// Generar los consecutivos de las Facturas Equivalentes
        /// </summary>
        /// <param name="periodo">periodo</param>
        public DTO_TxResult CuentasXPagar_ConsecutivoFactEquivalente(DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.CuentasXPagar_ConsecutivoFactEquivalente(this._userChannel, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Reclasificacion SaldosCxP
        /// </summary>
        /// <param name="facturas">Pagos a programar o desprogramar</param>
        /// <param name="cuenta">cuenta contrapartida</param>
        /// <param name="fecha">fecha Doc</param>
        /// <param name="tc">tasa de cambio</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public DTO_TxResult ReclasificacionSaldosCxP(int documentID, string actividadFlujoID, List<DTO_ProgramacionPagos> facturas, string cuenta,
            DateTime fecha, decimal tc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.ReclasificacionSaldosCxP(this._userChannel, documentID, actividadFlujoID, facturas, cuenta, fecha,tc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }


        #region Consultas

        /// <summary>
        /// Función que carga una lista de facrutas
        /// </summary>
        /// <param name="año">Año para filtrar</param>
        /// <param name="terceroId">Filtro del tercero</param>
        /// <param name="factNro">Numero de factura</param>
        /// <returns>Lista de Facturas</returns>
        public List<DTO_QueryFacturas> ConsultarFacturas(DateTime periodo, string terceroId, string conceptoCxP, string factNro, int tipoConsul, int? tipoFact)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.ConsultarFacturas(this._userChannel, periodo, terceroId, conceptoCxP,factNro, tipoConsul, tipoFact);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Legalización

        /// <summary>
        /// Guardar en tabla cpLegalizaDeta
        /// </summary>
        /// <param name="documentID">Documento que envia a aprobacion</param>
        /// <param name="leg">legalización</param>
        /// <returns></returns>
        public DTO_SerializedObject Legalizacion_Add(int documentID, DTO_Legalizacion leg, bool ComprobantePRE)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.Legalizacion_Add(this._userChannel, documentID, leg, ComprobantePRE);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene una legalizacion segun el numero de documento asociado
        /// </summary>
        /// <param name="numeroDoc">numero de documento</param>
        /// <returns>DTO_cpLegalizaHeader</returns>
        public DTO_Legalizacion Legalizacion_Get(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.Legalizacion_Get(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Guardar en tabla cpLegalizaDeta
        /// </summary>
        /// <param name="documentID">Documento que envia a aprobacion</param>
        /// <param name="leg">legalización</param>
        /// <returns></returns>
        public DTO_TxResult Legalizacion_Update(int documentID, List<DTO_cpLegalizaFooter> leg, DTO_cpLegalizaDocu header)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.Legalizacion_Update(this._userChannel, documentID, leg, header);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Envia para aprobacion una legalizacion
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="numeroDoc">numeroDoc de la legalizacion</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject Legalizacion_SendToAprob(int documentID, int numeroDoc, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.Legalizacion_SendToAprob(this._userChannel, documentID, numeroDoc, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de causaciones pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_LegalizacionAprobacion> Legalizacion_GetPendientesByModulo(ModulesPrefix mod, string actividadFlujoID)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.Legalizacion_GetPendientesByModulo(this._userChannel, mod, actividadFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Recibe una lista de cajas menores para aprobar o rechazar
        /// </summary>
        /// <param name="leg">Cajas menores que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Legalizacion_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_LegalizacionAprobacion> leg, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.Legalizacion_AprobarRechazar(this._userChannel, documentID, actividadFlujoID, leg, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Guardar el Auxiliar Pre
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="leg">Legalizacion a agregar con la informacion necesaria</param>
        /// <returns>resultado</returns>
        public DTO_TxResult Legalizacion_ComprobantePreAdd(int documentID, DTO_Legalizacion leg)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.Legalizacion_ComprobantePreAdd(this._userChannel, documentID, leg);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }
        #endregion

        #region Tarjeta Credito

        /// <summary>
        /// Obtiene un objeto DTO_cpTarjetaDocu por numero de documento
        /// </summary>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <returns></returns>
        public DTO_cpTarjetaDocu cpTarjetaDocu_GetByEstado(int numeroDoc, EstadoDocControl estado, out List<DTO_cpTarjetaPagos> lisTarjetaPago)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.cpTarjetaDocu_GetByEstado(this._userChannel, numeroDoc, estado, out lisTarjetaPago);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Guarda o actualiza documento de Tarjeta Credito
        /// </summary>
        /// <param name="_dtoCtrl">documento asociado</param>
        /// <param name="_tarjetaDocu">Tarjeta Docu</param>
        /// <param name="userID">usuario</param>
        /// <param name="update">bandera actualizacion</param>
        /// <returns></returns>
        public DTO_SerializedObject cpTarjetaDocu_Guardar(int documentID, DTO_glDocumentoControl _dtoCtrl, DTO_cpTarjetaDocu _tarjetaDocu, List<DTO_cpTarjetaPagos> _listTarjetaPago, bool update)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.cpTarjetaDocu_Guardar(this._userChannel, documentID, _dtoCtrl, _tarjetaDocu, _listTarjetaPago, update);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna el valor total para una lista de Tarjetas Docu 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tm">Tipo de moneda sobre el cual estan viendo las tarjetas</param>
        /// <param name="tc">Tasa de cambio en el dia</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna el valor total de las tarjetas</returns>
        public decimal cpTarjetaDocu_GetResumenVal(DateTime periodo, TipoMoneda tm, decimal tc, string terceroID)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.cpTarjetaDocu_GetResumenVal(this._userChannel, periodo, tm, tc, terceroID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna una lista de tarjetas Docu 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoMoneda">Tipo de moneda de los cuales hay que traer las tarjetas</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna una lista de Tarjetas Docu</returns>
        public List<DTO_AnticiposResumen> cpTarjetaDocu_GetResumen(DateTime periodo, TipoMoneda tipoMoneda, string terceroID)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.cpTarjetaDocu_GetResumen(this._userChannel, periodo, tipoMoneda, terceroID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de Tarjetas pago pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_AnticipoAprobacion> cpTarjetaDocu_GetPendientesByModulo(ModulesPrefix modulo, string actividadFlujoID)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.cpTarjetaDocu_GetPendientesByModulo(this._userChannel, modulo, actividadFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Recibe una lista de Tarjetas Docu para aprobar o rechazar
        /// </summary>
        /// <param name="tarjetaPago">Tarjetas Docu que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> cpTarjetaDocu_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_AnticipoAprobacion> comps, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.cpTarjetaDocu_AprobarRechazar(this._userChannel, documentID, actividadFlujoID, comps, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Facturacion

        #region FacturaVenta

        /// <summary>
        /// Consulta una tabla faFacturaDocu segun el numero de documento
        /// </summary>
        /// <param name="NumeroDoc">Numero de la factura</param>
        /// <returns></returns>
        public DTO_faFacturaDocu faFacturaDocu_Get(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.faFacturaDocu_Get(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Carga la informacion completa de la factura
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="facturaNro">Numero de la factura</param>
        /// <returns>Retorna datos de FacturaVenta</returns>
        public DTO_faFacturacion FacturaVenta_Load(int documentID, string prefijoID, int facturaNro)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.FacturaVenta_Load(this._userChannel, documentID, prefijoID, facturaNro);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Guardar nueva FacturaVenta y asociar en glDocumentoControl
        /// </summary>
        /// <param name="ctrl">glDocumentoControl</param>
        /// <param name="header">FacturaDocu</param>
        /// <param name="footer">FacturaFooter</param>
        /// <returns>NumeroDoc</returns>
        public DTO_SerializedObject FacturaVenta_Guardar(int documentID, DTO_glDocumentoControl ctrl, DTO_faFacturaDocu header, List<DTO_faFacturacionFooter> footer, bool update, out int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.FacturaVenta_Guardar(this._userChannel, documentID, ctrl, header, footer, update, out numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Aprueba o rechazas factura
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="factList">lista facturas</param>
        /// <param name="updateDocCtrl">actualiza el documento control</param>
        /// <param name="createDoc">genera archivo fisico</param>
        public List<DTO_SerializedObject> FacturaVenta_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_faFacturacionAprobacion> factList)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.FacturaVenta_AprobarRechazar(this._userChannel, documentID, actividadFlujoID, factList);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de facturas pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_faFacturacionAprobacion> FacturaVenta_GetPendientesByModulo(ModulesPrefix mod, string actividadFlujoID)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.FacturaVenta_GetPendientesByModulo(this._userChannel, mod, actividadFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna una lista de facturas 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoMoneda">Tipo de moneda de los cuales hay que traer las facturas</param>
        /// <param name="terceroID">Tercero del Cliente</param>
        /// <returns>Retorna una lista de facturas</returns>
        public List<DTO_faFacturacionResumen> Facturacion_GetResumen(DateTime periodo, TipoMoneda tipoMoneda, string terceroID)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.Facturacion_GetResumen(this._userChannel, periodo, tipoMoneda, terceroID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna una lista de facturas 
        /// </summary>
        /// <param name="clienteID">Periodo de consulta</param>
        /// <param name="NotaEnvioEmptyInd">filtrar por nota de envio</param>
        /// <returns>Retorna una lista de facturas</returns>
        public List<DTO_faFacturaDocu> FacturaVenta_GetByCliente(int documentID, string clienteID, bool NotaEnvioEmptyInd = true)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.FacturaVenta_GetByCliente(this._userChannel, documentID, clienteID, NotaEnvioEmptyInd);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza la tabla faFacturaDocu 
        /// </summary>
        /// <param name="fact">DTO_faFacturaDocu</param>
        /// <param name="OnlyFacturaFija">Indica si solo guarda el indicador de facturaFija</param>
        /// <returns></returns>
        public DTO_TxResult FacturaDocu_Upd(int documentoID, List<DTO_faFacturaDocu> fact, bool OnlyFacturaFija)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.FacturaDocu_Upd(this._userChannel, documentoID, fact, OnlyFacturaFija);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega una lista de facturas
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="actividadFlujoID">Actividad de flujo</param>
        /// <param name="migrarList">Datos para migrar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult FacturaVenta_MigracionGeneral(int documentID, string actividadFlujoID, List<DTO_MigrarFacturaVenta> migrarList, ref List<int> numDocs)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.FacturaVenta_MigracionGeneral(this._userChannel, documentID, actividadFlujoID, migrarList, ref numDocs);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Anula una factura de venta
        /// </summary>
        /// <param name="numDocFacts">nums de las facturas a anular</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult FacturaVenta_Anular(int documentID, List<int> numDocFacts)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.FacturaVenta_Anular(this._userChannel, documentID, numDocFacts);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Revierte una factura venta
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="numeroDoc"></param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Facturacion_Revertir(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.Facturacion_Revertir(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }
        #endregion

        #region CxC

        /// <summary>
        /// Agrega una lista de CxC
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="actividadFlujoID">Actividad de flujo</param>
        /// <param name="concSaldoID">Nuevo concepto de saldo para las cuentas</param>
        /// <param name="ctrlList">Lista de documentos</param>
        /// <param name="cxcFactList">Lista de cuentas por cobrar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult CuentasXCobrar_Migracion(int documentID, string actividadFlujoID, string concSaldoID, List<DTO_glDocumentoControl> ctrlList, List<DTO_faFacturaDocu> cxcFactList)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.CuentasXCobrar_Migracion(this._userChannel, documentID, actividadFlujoID, concSaldoID, ctrlList, cxcFactList);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        #endregion

        #region Consultas

        public List<DTO_QueryHeadFactura> ConsultarFacturasXNota(DateTime año, string terceroId, int tipoConsulta, string Asesor, string Zona, string Proyecto, int TipoFact, string NumFact, string Prefijo, bool facturaFijaInd)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.ConsultarFacturasXNota(this._userChannel, año, terceroId, tipoConsulta, Asesor, Zona, Proyecto, TipoFact, NumFact, Prefijo, facturaFijaInd);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Tesoreria

        /// <summary>
        ///  Obtiene el detalle de los proyectos
        /// </summary>
        /// <param name="fechaCorte">Fecha corte</param>
        /// <param name="loadTareas">Carga las tareas del proyecto</param>
        /// <param name="loadDetalleTarea">carga el detalle por tarea</param>
        /// <param name="loadDetalle">carga el detalle por proyecto</param>
        /// <returns>resultado</returns>
        public List<DTO_QueryFlujoFondos> tsFlujoFondos(DateTime fechaCorte)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.tsFlujoFondos(this._userChannel, fechaCorte);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        ///  Obtiene el detalle de los proyectos
        /// </summary>
        /// <param name="fechaCorte">Fecha corte</param>
        /// <param name="loadTareas">Carga las tareas del proyecto</param>
        /// <param name="loadDetalleTarea">carga el detalle por tarea</param>
        /// <param name="loadDetalle">carga el detalle por proyecto</param>
        /// <returns>resultado</returns>
        public List<DTO_QueryFlujoFondosTareas> tsFlujoFondos_Tareas(DateTime fechaCorte, string proyecto,bool? recaudosInd)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.tsFlujoFondos_Tareas(this._userChannel, fechaCorte, proyecto, recaudosInd);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }
        

        /// <summary>
        ///  Obtiene el detalle de tesoreria
        /// </summary>
        /// <param name="fechaCorte">Fecha corte</param>
        /// <returns>resultado</returns>
        public List<DTO_QueryFlujoCaja> tsFlujoCaja(DateTime fechaCorte)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.tsFlujoCaja(this._userChannel, fechaCorte);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        ///  Obtiene el detalle del Flujo de Caja
        /// </summary>
        /// <param name="Documento">Documento</param>
        /// <returns>resultado</returns>
        public List<DTO_QueryFlujoCajaDetalle> tsFlujoCajaDetalle(string Documento)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.tsFlujoCajaDetalle(this._userChannel, Documento);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }


        /// <summary>
        ///  Obtiene la semana del detalle del Flujo de Caja
        /// </summary>
        /// <param name="Documento">Documento</param>
        /// <returns>resultado</returns>
        public string Global_DiaSemana(Int16 Semana)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.Global_DiaSemana(this._userChannel, Semana);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        ///  Obtiene el mes del detalle del Flujo de Caja
        /// </summary>
        /// <param name="Documento">Documento</param>
        /// <returns>resultado</returns>
        public string Global_Mes(Int16 Mes)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.Global_Mes(this._userChannel, Mes);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }


        #region PagosElectronicos

        /// <summary>
        /// Consulta las facturas para transmitir al banco
        /// </summary>
        /// <returns>Lista de facturas para transmitir al banco</returns>
        public List<DTO_PagosElectronicos> PagosElectronicos_GetPagosElectronicosSinTransmitir()
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.PagosElectronicos_GetPagosElectronicosSinTransmitir(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        /// <summary>
        /// Guarda el estado actual de los pagos electronicos
        /// </summary>
        /// <param name="pagosElectronicos">Listado de los pagos actuales</param>
        /// <returns>Resultado de la operación</returns>
        public DTO_TxResult PagosElectronicos_Guardar(int documentID, List<DTO_PagosElectronicos> pagosElectronicos)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.PagosElectronicos_Guardar(this._userChannel, documentID, pagosElectronicos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        /// <summary>
        /// Transmite los pagos electronicos
        /// </summary>
        /// <param name="pagosElectronicos">Listado de los pagos actuales</param>
        /// <returns>Resultado de la operación</returns>
        public DTO_TxResult PagosElectronicos_Transmitir(int documentID, List<DTO_PagosElectronicos> pagosElectronicos)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.PagosElectronicos_Transmitir(this._userChannel, documentID, pagosElectronicos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        /// <summary>
        /// Consulta los pagos transmitidos al banco, buscando por tercero y fecha de transmicion
        /// </summary>
        /// <param name="terceroID">Tercero al que se le realizó el pago</param>
        /// <param name="fechaTransmicion">Fecha en la que se realizó la transmición</param>
        /// <returns>Lista de facturas para transmitir al banco</returns>
        public List<DTO_PagosElectronicos> PagosElectronicos_GetPagosElectronicosTransmitidos(string terceroID, DateTime fechaTransmicion)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.PagosElectronicos_GetPagosElectronicosTransmitidos(this._userChannel, terceroID, fechaTransmicion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        /// <summary>
        /// Revierte la transmicion de los pagos electronicos
        /// </summary>
        /// <param name="pagosElectronicos">Listado de los pagos a revertir</param>
        /// <returns>Resultado de la operación</returns>
        public DTO_TxResult PagosElectronicos_RevertirTransmicion(int documentID, List<DTO_PagosElectronicos> pagosElectronicos)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.PagosElectronicos_RevertirTransmicion(this._userChannel, documentID, pagosElectronicos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        #endregion

        #region Pagos

        /// <summary>
        /// Obtiene la lista de pagos para su programación
        /// </summary>
        /// <returns>Lista de pagos para su programación</returns>
        public List<DTO_ProgramacionPagos> ProgramacionPagos_GetProgramacionPagos()
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.ProgramacionPagos_GetProgramacionPagos(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        /// <summary>
        /// Programa pagos
        /// </summary>
        /// <param name="programacionesPagos">Pagos a programar o desprogramar</param>
        /// <param name="pagoAprobacionInd">Indica si la empresa aprueba o deja pendiente de aprobación</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public DTO_TxResult ProgramacionPagos_ProgramarPagos(int documentID, string actividadFlujoID, List<DTO_ProgramacionPagos> programacionesPagos, bool pagoAprobacionInd)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.ProgramacionPagos_ProgramarPagos(this._userChannel, documentID, actividadFlujoID, programacionesPagos, pagoAprobacionInd);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        /// <summary>
        /// Consulta las facturas programadas previamente para pagar
        /// </summary>
        /// <returns>Lista de facturas por tercero para pagar</returns>
        public List<DTO_SerializedObject> PagoFacturas_GetPagoFacturas()
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.PagoFacturas_GetPagoFacturas(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        /// <summary>
        /// Registra el pago de una factura en un documento
        /// </summary>
        /// <param name="pagoFacturas">Pagos a registrar</param>
        /// <param name="areaFuncionalID">Código del área funcional</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public List<DTO_TxResult> PagoFacturas_RegistrarPagoFacturas(int documentID, string actividadFlujoID, List<DTO_PagoFacturas> pagoFacturas, DateTime fechaPago,
            string areaFuncionalID)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.PagoFacturas_RegistrarPagoFacturas(this._userChannel, documentID, actividadFlujoID, pagoFacturas, fechaPago, areaFuncionalID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        /// <summary>
        /// Transferencias bancarias
        /// </summary>
        /// <param name="programacionesPagos">Pagos a programar o desprogramar</param>
        /// <param name="pagoAprobacionInd">Indica si la empresa aprueba o deja pendiente de aprobación</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public List<DTO_TxResult> TransferenciasBancarias_Transferencias(int documentID, string actividadFlujoID, List<DTO_ProgramacionPagos> programacionesPagos,
            DateTime fecha, decimal tc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.TransferenciasBancarias_Transferencias(this._userChannel, documentID, actividadFlujoID, programacionesPagos, fecha, tc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        /// <summary>
        /// Revierte una transferencia bancaria
        /// </summary>
        /// <param name="documentID">documento</param>
        /// <param name="compTransf">comprobante a revertir</param>
        /// <returns></returns>
        public DTO_TxResult TransferenciasBancarias_Revertir(int documentID, DTO_Comprobante compTransf)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.TransferenciasBancarias_Revertir(this._userChannel, documentID, compTransf);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        /// <summary>
        /// Carga la informacion para la anulacion de un cheque
        /// </summary>
        /// <param name="documentID">Documento que realiza la transaccion</param>
        /// <param name="fechaPago">fecha de la operacion</param>
        /// <param name="pagoFactura">Pago factura</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        public DTO_TxResult AnularCheques(int documentID, DateTime fechaPago, DTO_PagoFacturas pagoFactura)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.AnularCheques(this._userChannel, documentID, fechaPago,pagoFactura);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        /// <summary>
        /// Obtiene el docu de los pagos
        /// </summary>
        /// /// <returns>DTO de pagos</returns>
        public DTO_tsBancosDocu tsBancosDocu_Get(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.tsBancosDocu_Get(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        #endregion

        #region Recibos de Caja

        /// <summary>
        /// Consulta una tabla tsReciboCajaDocu segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Recibo</param>
        /// <returns></returns>
        public DTO_tsReciboCajaDocu ReciboCaja_Get(int NumeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.ReciboCaja_Get(this._userChannel, NumeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Carga la informacion completa del Recibo de Caja
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="cajaID">PrefijoID (corresponde a cajaID)</param>
        /// <param name="reciboNro">Numero de Documento interno</param>
        /// <returns>Retorna el Recibo de Caja</returns>
        public DTO_ReciboCaja ReciboCaja_GetForLoad(int documentID, string cajaID, int reciboNro)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.ReciboCaja_GetForLoad(this._userChannel, documentID, cajaID, reciboNro);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Guardar nuevo Recibo de Caja y asociar en glDocumentoControl
        /// </summary>
        /// <param name="ctrl">referencia documento</param>
        /// <param name="recibo">Recibo de Caja</param>
        /// <param name="comp">Comprobante</param>
        /// <returns></returns>
        public DTO_TxResult ReciboCaja_Guardar(int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_tsReciboCajaDocu recibo, DTO_Comprobante comp,
            out int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.ReciboCaja_Guardar(this._userChannel, documentID, actividadFlujoID, ctrl, recibo, comp, out numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        /// <summary>
        /// Migra una lista de recibos de caja
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la operacion (los que se van a guardar en glDocumentoControl)</param>
        /// <param name="periodo">Periodo de migración</param>
        /// <param name="recibos">Lista de recibos</param>
        /// <param name="areaFuncionalID">Area funcional del usuario</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_TxResult> ReciboCaja_Migracion(int documentID, List<DTO_ReciboCaja> recibos)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.ReciboCaja_Migracion(this._userChannel, documentID, recibos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }
        }

        #endregion

        #region Traslado de Fondos

        /// <summary>
        /// Genera el documento de traslado de fondos
        /// </summary>
        /// <param name="ctrl">Documento a guardar</param>
        /// <param name="tblAux">Tabla auxiliar con datos adicionales</param>
        /// <param name="generaOrdenPago">Valor que indica si el documento genra orden de pago o no</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public DTO_TxResult TrasladoFondos_TrasladarFondos(int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_tsBancosDocu tblAux, bool generaOrdenPago)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.TrasladoFondos_TrasladarFondos(this._userChannel, documentID, actividadFlujoID, ctrl, tblAux, generaOrdenPago);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        #endregion

        #region Consignaciones

        /// <summary>
        /// Genera el documento de traslado de fondos
        /// </summary>
        /// <param name="ctrl">Documento a guardar</param>
        /// <param name="tblAux">Tabla auxiliar con datos adicionales</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public DTO_TxResult Consignaciones_Consignar(int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_tsBancosDocu tblAux)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.Consignaciones_Consignar(this._userChannel, documentID, actividadFlujoID, ctrl, tblAux);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        #endregion

        #region Notas Bancarias

        /// <summary>
        /// Crea una nota bancaria
        /// </summary>
        /// <param name="dtoCtrl">referencia documento</param>
        /// <returns></returns>
        public DTO_TxResult NotasBancarias_Radicar(int documentID, DTO_glDocumentoControl dtoCtrl, DTO_Comprobante comp, DTO_coDocumentoRevelacion revelacion)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                var result = this.CftService.NotasBancarias_Radicar(this._userChannel, documentID, dtoCtrl, comp, revelacion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        #endregion

        #region Consultas

        /// <summary>
        /// Funcion que carga la información de cada cheque con sus respectivos movimientos
        /// </summary>
        /// <param name="bancoID">Filtro del bancoID</param>
        /// <param name="nit">TerceroDi</param>
        /// <param name="fechaIni">Fecha inicial de la consulta</param>
        /// <param name="fechaFin">Fecha final de la consulta</param>
        /// <param name="numCheque">Numero del cheque</param>
        /// <returns>Lista de cheques con sus respectivos detalles</returns>
        public List<DTO_ChequesGirados> GetCheques(string bancoID, string nit, DateTime fechaIni, DateTime fechaFin, string numCheque)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                string url = string.Empty;
                var result = this.CftService.GetCheques(this._userChannel, bancoID, nit, fechaIni, fechaFin, numCheque, out url, null);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        /// <summary>
        /// Funcion que carga la información de los recibos de caja
        /// </summary>
        /// <param name="CajaID">Filtro de la caja</param>
        /// <param name="tercero">TerceroID</param>
        /// <param name="fechaIni">Fecha inicial de la consulta</param>
        /// <param name="fechaFin">Fecha final de la consulta</param>
        /// <param name="numReciboCaja">Numero del recibo</param>
        /// <returns>Lista de recibos</returns>
        public List<DTO_QueryReciboCaja> ReciboCaja_GetByParameter(string CajaID, string tercero, DateTime fechaIni, DateTime fechaFin, string numReciboCaja)
        {
            try
            {
                this.CreateService(typeof(ICFTService));
                string url = string.Empty;
                var result = this.CftService.ReciboCaja_GetByParameter(this._userChannel, CajaID, tercero, fechaIni, fechaFin, numReciboCaja);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(ICFTService));
                throw ex;
            }

        }

        #endregion

        #region Reportes
        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="bancoID">Banco ID</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="orden">Orden para mostrar en el reporte</param>
        /// <param name="nombreBen">Nombredel beneficiario para filtrar</param>
        /// <returns>Nombre del reporte</returns>
        public string Reporte_TsChequesGiradosDetalle(string bancoID, string terceroID, DateTime fechaIni, DateTime fechaFin, string orden, bool? nombreBen)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Ts_ChequesGiradosDetalle(this._userChannel, bancoID, terceroID, fechaIni, fechaFin, orden, nombreBen);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="bancoID">Banco ID</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="tiporeporte">tiporeporte</param>
        /// <returns>Nombre del reporte</returns>
        public string Reporte_TsEjecutado(DateTime fechaIni, string tipoReporte)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Ts_Ejecutado(this._userChannel, fechaIni,tipoReporte);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        #endregion

        #endregion

        #endregion

        #region GlobalService

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción de un usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ConsultarProgresoAdmin(int documentID)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.ConsultarProgreso(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region Maestras

        #region ccCarteraComponente

        /// <summary>
        /// Trae los componentes de  cartera dependiendo de la linea de credito
        /// </summary>
        /// <param name="pagaduriaID">Linea de credito</param>
        /// <returns></returns>
        public List<DTO_ccCarteraComponente> ccCarteraComponente_GetByLineaCredito(string lineaCreditoID)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.ccCarteraComponente_GetByLineaCredito(this._userChannel, lineaCreditoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }

        }

        #endregion

        #region ccAnexosLista

        /// <summary>
        /// Trae los documentos anexos dependiendo de la pagaduria
        /// </summary>
        /// <param name="pagaduriaID">Id de la pagaduria</param>
        /// <returns></returns>
        public List<DTO_MasterBasic> ccAnexosLista_GetByPagaduria(string pagaduriaID)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.ccAnexosLista_GetByPagaduria(this._userChannel, pagaduriaID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }

        }

        #endregion

        #region ccCliente

        /// <summary>
        ///  Adiciona una lista de clientes
        /// </summary>
        /// <param name="documentoID">Identifica el documento</param>
        /// <param name="bItems">Lista de empresas</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult ccCliente_Add(int documentoID, byte[] bItems)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.ccCliente_Add(this._userChannel, documentoID, bItems);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }

        }

        /// <summary>
        /// Actualiza un cliente
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult ccCliente_Update(DTO_ccCliente cliente)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.ccCliente_Update(this._userChannel, cliente);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        ///  Adiciona una lista de clientes
        /// </summary>
        /// <param name="documentoID">Identifica el documento</param>
        /// <param name="data">data a guardar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResultDetail ccCliente_AddFromSource(int documentoID, object data)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.ccCliente_AddFromSource(this._userChannel, documentoID, data);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }
        #endregion

        #region ccFasecolda
        /// <summary>
        ///  Adiciona una lista de fasecoldas y modelos
        /// </summary>
        /// <param name="fasecoldas">fasecoldas</param>
        /// <param name="modelos">modelos</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult ccFasecolda_Migracion(byte[] fasecoldas, byte[] modelos)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.ccFasecolda_Migracion(this._userChannel, fasecoldas, modelos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }
        #endregion

        #region ccChequeoLista

        /// <summary>
        /// Trae la lista de tareas asociados a un documento
        /// </summary>
        /// <param name="documentoID">Id de la pagaduria</param>
        /// <returns>Retorna una lista con la maestra de lista de chequo</returns>
        public List<DTO_MasterBasic> ccChequeoLista_GetByDocumento(int documentoID)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.ccChequeoLista_GetByDocumento(this._userChannel, documentoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }

        }

        #endregion

        #region glActividadChequeoLista
        /// <summary>
        /// Trae la lista de chequeos de un flujo
        /// </summary>
        /// <param name="actividadFlujo">Flujo</param>
        /// <returns>Retorna una lista con la maestra de lista de chequo</returns>
        public List<DTO_MasterBasic> glActividadChequeoLista_GetByActividad(string actividadFlujo)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glActividadChequeoLista_GetByActividad(this._userChannel, actividadFlujo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }

        }

        #endregion

        #region coCargoCosto

        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="ConceptoCargoID">Identificador del concepto de cargo</param>
        /// <param name="operacionID">Identificador de la operacion</param>
        /// <param name="lineaPresID">Identificador de la linea presupuestal</param>
        /// <returns>Retorna una cuenta o null si no existe</returns>
        public string coCargoCosto_GetCuentaIDByCargoProy(string ConceptoCargoID, string proyID, string ctoCostoID, string lineaPresID)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.coCargoCosto_GetCuentaIDByCargoOper(this._userChannel, ConceptoCargoID, proyID, ctoCostoID, lineaPresID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="valor">Valor sobre el cual se esta trabajando</param>
        /// <param name="conceptoCargoID">Identificador del concepto de cargo</param>
        /// <param name="operacionID">Identificador de la operacion</param>
        /// <param name="lineaPresID">Identificador de la linea presupuestal</param>
        /// <returns>Retorna una cuenta o string.Empty si no existe</returns>
        public DTO_CuentaValor coCargoCosto_GetCuentaByCargoOper(string conceptoCargoID, string operacionID, string lineaPresID, decimal valor)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.coCargoCosto_GetCuentaByCargoOper(this._userChannel, conceptoCargoID, operacionID, lineaPresID, valor);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region coComprobantePrefijo

        /// <summary>
        /// Trae el identificador de un comprobante segun el documento y el prefijo
        /// </summary>
        /// <param name="coDocumentoID">Identificador del documento de la PK</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="compAnulacion">Indica si debe traer el comprobante de anulacion</param>
        /// <returns>Retorna el identificador de un comprobante o null si no existe</returns>
        public string coComprobantePrefijo_GetComprobanteByDocPref(int documentID, int coDocumentoID, string prefijoID, bool compAnulacion)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.coComprobantePrefijo_GetComprobanteByDocPref(this._userChannel, documentID, coDocumentoID, prefijoID, compAnulacion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region coDocumento

        /// <summary>
        /// Dice si un prefijo ya esta asignado en la tabla coDocumento
        /// </summary>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <returns>Retorna Verdadero si el prefijo ya existe, de lo contrario retorna falso</returns>
        public bool coDocumento_PrefijoExists(string prefijoID)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.coDocumento_PrefijoExists(this._userChannel, prefijoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region coPlanCuenta

        /// <summary>
        /// Trae una lista de tarifas para una cuenta
        /// </summary>
        /// <param name="impTipoID">Tipo de impuesto</param>
        public List<decimal> coPlanCuenta_TarifasImpuestos()
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.coPlanCuenta_TarifasImpuestos(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la cuenta de la cuenta alterna
        /// </summary>
        /// <param name="id">Identificador de la maestra</param>
        /// <param name="active">indica si el registro debe estar activo</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Retorna un registro de la cuenta alterna</returns>
        public DTO_coPlanCuenta coPlanCuenta_GetCuentaAlterna(int documentID, UDT_BasicID id, bool active, List<DTO_glConsultaFiltro> filtros = null)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.coPlanCuenta_GetCuentaAlterna(this._userChannel, documentID, id, active, filtros);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Cuenta la cantidad de resultados dado un filtro
        /// </summary>
        /// <param name="consulta">Filtros</param>
        /// <param name="FiltrosExtra">Filtros de codigo y descripción</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Retorna el numero de registros encontrados</returns>
        public long coPlanCuenta_CountChildren(int documentID, UDT_BasicID parentId, string idFilter, string descFilter, bool? active, List<DTO_glConsultaFiltro> filtros)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.coPlanCuenta_CountChildren(this._userChannel, documentID, parentId, idFilter, descFilter, active, filtros);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae los registros de un plan de cuentas corporativo 
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de página</param>
        /// <param name="consulta">Filtros</param>
        /// <param name="FiltrosExtra">Filtros de codigo y descripción</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Devuelve los registros de un plan de cuentas corporativo</returns>
        public IEnumerable<DTO_MasterBasic> coPlanCuenta_GetPagedChildren(int documentID, int pageSize, int pageNum, OrderDirection orderDirection, UDT_BasicID parentId,
            string idFilter, string descFilter, bool? active, List<DTO_glConsultaFiltro> filtros)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.coPlanCuenta_GetPagedChildren(this._userChannel, documentID, pageSize, pageNum, orderDirection, parentId, idFilter,
                    descFilter, active, filtros);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region coPlanillaConsolidacion

        /// <summary>
        /// Trae la lista de empresas a consilidar
        /// </summary>
        /// <returns>Retorna la lista de empresas a consilidar</returns>
        public List<DTO_ComprobanteConsolidacion> coPlanillaConsolidacion_GetEmpresas()
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.coPlanillaConsolidacion_GetEmpresas(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region glActividadFlujo

        /// <summary>
        /// Obtiene la lista de tareas de un documento
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <returns>Retorna la lista de tareas</returns>
        public List<string> glActividadFlujo_GetActividadesByDocumentID(int documentoID)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glActividadFlujo_GetActividadesByDocumentID(this._userChannel, documentoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene la lista de padres de una actividad de flujo
        /// </summary>
        /// <param name="actFlujoID">Actividad hija</param>
        /// <returns>Retorna la lista de tareas</returns>
        public List<DTO_glActividadFlujo> glActividadFlujo_GetParents(string actFlujoID)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glActividadFlujo_GetParents(this._userChannel, actFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region glActividadPermiso

        /// <summary>
        /// Consulta un listado de tareas de glTareaPermiso para usuarios
        /// </summary>
        /// <returns>Listado de TareaID(int)</returns>
        public List<string> glActividadPermiso_GetActividadesByUser()
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glActividadPermiso_GetActividadesByUser(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region glEmpresa

        /// <summary>
        /// Trae la imagen del logo de la empresa
        /// </summary>
        /// <returns>arreglo de bytes con la imagen del logo</returns>
        public byte[] glEmpresaLogo()
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glEmpresaLogo(this._userChannel, this.Empresa);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        ///  Adiciona una lista de empresas
        /// </summary>
        /// <param name="bItems">Lista de empresas</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="accion">Identifica la acción </param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult glEmpresa_Add(byte[] bItems, int accion)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glEmpresa_Add(this._userChannel, AppMasters.glEmpresa, bItems, accion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        ///  Elimina una empresa
        /// </summary>
        /// <param name="empresaDel">Empresa que de desea eliminar</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult glEmpresa_Delete(DTO_glEmpresa empresaDel)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glEmpresa_Delete(this._userChannel, AppMasters.glEmpresa, empresaDel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region glEmpresaGrupo

        /// <summary>
        /// Agrega un nuevo grupo de empresas
        /// </summary>
        /// <param name="grupo">Grupo de embresas</param>
        /// <param name="egCopia">Grupo de empresas del cual se saca la copia</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transacción</param>
        /// <returns></returns>
        public bool glEmpresaGrupo_Add(byte[] bItems)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glEmpresaGrupo_Add(this._userChannel, bItems);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Elimina un grupo de empresas
        /// </summary>
        /// <param name="grupo">Grupo de embresas</param>
        /// <param name="egCopia">Grupo de empresas del cual se saca la copia</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transacción</param>
        /// <returns></returns>
        public bool glEmpresaGrupo_Delete(string egID)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glEmpresaGrupo_Delete(this._userChannel, egID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region glTabla

        /// <summary>
        /// Retorna todas las gl tablas de un grupo de empresas
        /// </summary>
        /// <param name="empGrupo">Nombre del grupo de empresas</param>
        /// <returns>Lista de glTabla</returns>
        public IEnumerable<DTO_glTabla> glTabla_GetAllByEmpresaGrupo(Dictionary<int, string> empGrupo, bool jerarquicaInd = false)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.glTabla_GetAllByEmpresaGrupo(this._userChannel, empGrupo, jerarquicaInd);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Indica si una tabla tiene datos para un grupo empresa
        /// </summary>
        /// <param name="tablaNombre">Nombre de la tabla</param>
        /// <returns>True si tiene datos, False si no tiene datos</returns>
        public bool glTabla_HasData(string tablaNombre, string empresaGrupo)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.glTabla_HasData(this._userChannel, tablaNombre, empresaGrupo);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna la info de una tabla segun el nombre y el grupo de empresas
        /// </summary>
        /// <param name="tablaNombre">Tabla Nombre</param>
        /// <param name="empGrupo">Grupo de empresas</param>
        /// <returns>Retorna la informacion de una tabla</returns>
        public DTO_glTabla glTabla_GetByTablaNombre(string tablaNombre, string empGrupo)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.glTabla_GetByTablaNombre(this._userChannel, tablaNombre, empGrupo);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region glTasaDeCambio

        /// <summary>
        /// Obtiene el la tasa de cambio
        /// </summary>
        /// <param name="monedaID">Identificador de la moneda</param>
        /// <param name="fecha">Fecha</param>
        /// <returns>Retorna la tasa de canbio</returns>
        public decimal TasaDeCambio_Get(string monedaID, DateTime fecha)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.TasaDeCambio_Get(this._userChannel, monedaID, fecha);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region seDelegacionTarea

        /// <summary>
        /// Trae la lista de seguridades de documentos de un usuario dada la empresa
        /// </summary>
        /// <param name="userId">Codigo de seguridad del usuario</param>
        /// <param name="userEmpDef">Empresa</param>
        /// <param name="isGroupActive">Si el grupo de seguridad esta activo</param>
        /// <returns>Retorna las seguridades de un usuario en una empresa</returns>
        public List<DTO_seDelegacionHistoria> seDelegacionHistoria_Get(string userId)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seDelegacionHistoria_Get(this._userChannel, userId);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega una delegacion
        /// </summary>
        /// <param name="del">Delegacion</param>
        public bool seDelegacionHistoria_Add(int documentID, DTO_seDelegacionHistoria del)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seDelegacionHistoria_Add(this._userChannel, documentID, del);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza el estado de un delegado
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="enabled">Nuevo estado</param>
        public bool seDelegacionHistoria_UpdateStatus(int documentID, string userID, DateTime fechaIni, bool enabled)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seDelegacionHistoria_UpdateStatus(this._userChannel, documentID, userID, fechaIni, enabled);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region seGrupoDocumento

        /// <summary>
        /// Trae la lista de seguridades de documentos de un usuario dada la empresa
        /// </summary>
        /// <param name="userId">Codigo de seguridad del usuario</param>
        /// <param name="userEmpDef">Empresa</param>
        /// <returns>Retorna las seguridades de un usuario en una empresa</returns>
        public IEnumerable<DTO_seGrupoDocumento> seGrupoDocumento_GetByUsuarioId(int userId, string userEmpDef)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seGrupoDocumento_GetByUsuarioId(this._userChannel, userEmpDef, userId, true);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene las seguridades del sistema para un grupo dado el modulo y el tipo de documento
        /// </summary>
        /// <param name="grupo">Grupo de seguridades</param>
        /// <param name="tipo">Tipo de documento</param>
        /// <returns>Retorna las seguridades de un grupo</returns>
        public IEnumerable<DTO_seGrupoDocumento> seGrupoDocumento_GetByType(string grupo, string tipo)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seGrupoDocumento_GetByType(this._userChannel, grupo, tipo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));

                throw ex;
            }
        }

        /// <summary>
        /// Actualiza una lista de seguridades
        /// </summary>
        /// <param name="bItems">Lista de seguridades comprimidas</param>
        /// <param name="seUsuario">Identificador del usuario</param>
        /// <returns>Retorna la lista de seguridades comprimidas</returns>
        public DTO_TxResult seGrupoDocumento_UpdateSecurity(byte[] bItems, int seUsuario)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seGrupoDocumento_UpdateSecurity(this._userChannel, bItems, seUsuario);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));

                throw ex;
            }
        }

        #endregion

        #region seMaquina

        /// <summary>
        /// valida que la maquina pueda ingresar al sistema
        /// </summary>
        /// <param name="pcMAC">MACs posibles</param>
        /// <returns>Retorna verdadero si la maquina tiene permiso</returns>
        /// <returns>Devuelve si el usuarios es valido Usuarios</returns>
        public bool seMaquina_ValidatePC(List<string> macs)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seMaquina_ValidatePC(this._userChannel, macs);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));

                throw ex;
            }
        }

        #endregion

        #region seLAN

        /// <summary>
        /// Trae la configuración de una LAN
        /// </summary>
        /// <param name="lan">Nombre de la LAN</param>
        /// <returns>Retorna la configuracion de una LAN</returns>
        public DTO_seLAN seLAN_GetLanByID(string lan)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.seLAN_GetLanByID(this._userChannel, lan, AppMasters.seLAN);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae todas las configuraciones de LAN
        /// </summary>
        /// <returns>Retorna la lista de LANs y sus configuraciones</returns>
        public List<DTO_seLAN> seLAN_GetLanAll()
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.seLAN_GetLanAll(this._userChannel, AppMasters.seLAN);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region seUsuario

        /// <summary>
        /// Trae un usuario valido
        /// </summary>
        /// <param name="userId">Identificador de usuario</param>
        /// <param name="password">Contraseña de usuario</param>
        /// <returns>Returna un usuario valido</returns>
        public UserResult seUsuario_ValidateUserCredentials(string userId, string password)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seUsuario_ValidateUserCredentials(this._userChannel, userId, password);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un usuario
        /// </summary>
        /// <param name="userId">Identificador de usuario</param>
        /// <returns>Retorna un usuario</returns>
        public DTO_seUsuario seUsuario_GetUserbyID(string userId)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seUsuario_GetUserbyID(this._userChannel, userId);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un usuario de acuerdo con el id de la replica (pk)
        /// </summary>
        /// <param name="userID">Identificador del usuario (ReplicaID)</param>
        /// <returns>Retorna el Usuario</returns>
        public DTO_seUsuario seUsuario_GetUserByReplicaID(int replicaID)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seUsuario_GetUserByReplicaID(this._userChannel, replicaID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la lista de usuarios
        /// </summary>
        /// <param name="active">Indicador de activo</param>
        /// <returns>Devuelve la lista de usuarios </returns>
        public IEnumerable<DTO_MasterBasic> seUsuario_GetAll(bool? active)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seUsuario_GetAll(this._userChannel, AppMasters.seUsuario, active);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="pwd">Contraseña nueva</param>
        /// <param name="oldPwd">Contraseña vieja</param>
        /// <param name="oldPwdDate">Fecha en que fue modificada por ultima vez la contraseña</param>
        /// <returns>Retorna verdadero si la operación se pudo realizar, de lo contrario falso</returns>
        public bool seUsuario_UpdatePassword(int userID, string pwd, string oldPwd, string oldPwdDate)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seUsuario_UpdatePassword(this._userChannel, userID, pwd, oldPwd, oldPwdDate);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="pwd">Contraseña nueva</param>
        /// <returns>Retorna verdadero si la operación se pudo realizar, de lo contrario falso</returns>
        public bool seUsuario_ResetPassword(int userID, string pwd)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seUsuario_ResetPassword(this._userChannel, userID, pwd);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Devuelve las empresas a las que tiene permiso un usuario
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <returns>Retorna una lista de empresas</returns>
        public IEnumerable<DTO_glEmpresa> seUsuario_GetUserCompanies(string userID)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seUsuario_GetUserCompanies(this._userChannel, userID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        ///  Adiciona una lista de empresas
        /// </summary>
        /// <param name="bItems">Lista de empresas</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="accion">Identifica la acción </param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult seUsuario_Add(byte[] bItems, int seUsuario, int accion)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seUsuario_Add(this._userChannel, AppMasters.seUsuario, bItems, seUsuario, accion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza un usuario
        /// </summary>
        /// <param name="usr">registro donde se realiza la acción</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <returns>Resultado TxResult</returns>
        public DTO_TxResult seUsuario_Update(DTO_seUsuario usr, int seUsuario)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.seUsuario_Update(this._userChannel, AppMasters.seUsuario, usr, seUsuario);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region glActividadControl

        /// <summary>
        /// Trae los registro de la tabla glTareasControl
        /// </summary>
        /// <param name="numeroDoc">consecutivo de documento</param>
        /// <returns></returns>
        public IEnumerable<DTO_glActividadControl> glActividadControl_Get(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glActividadControl_Get(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region glActividadEstado

        /// <summary>
        /// Obtiene lista de actividades con llamada pendientes
        /// </summary>
        /// <param name="documentoID">documento filtro</param>
        /// <param name="actFlujoID">actividad o tarea filtro</param>
        /// <param name="fechaIni">fecha inicial de consulta</param>
        /// <param name="fechaFin">fecha final de consulta</param>
        /// <param name="terceroID">tercero filtro</param>
        /// <param name="prefijoID">prefijo filtro</param>
        /// <param name="docNro">nro de documento filtro</param>
        ///  <param name="tipo">Filtra vencidas, no vencidas o todas</param>
        ///   <param name="LLamadaInd">Si asigna valores de llamadas</param>
        /// <param name="estadoTareaInd">Indica si esta cerrada(Pendiente) o no</param>
        /// <returns></returns>
        public List<DTO_InfoTarea> glActividadEstado_GetPendientesByParameter(int? numeroDoc, int? documentoID, string actFlujoID, DateTime? fechaIni,
            DateTime? fechaFin, string terceroID, string prefijoID, int? docNro, EstadoTareaIncumplimiento tipo, bool llamadaInd, bool? vencidas)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glActividadEstado_GetPendientesByParameter(this._userChannel, numeroDoc, documentoID, actFlujoID, fechaIni, fechaFin,
                    terceroID, prefijoID, docNro, tipo, llamadaInd, vencidas);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega registros a actividadEstado
        /// </summary>
        /// <param name="documentID">Documento actual</param>
        /// <param name="notas">lista de notas</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult glActividadEstado_AddNotas(int documentID, List<DTO_InfoTarea> notas)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glActividadEstado_AddNotas(this._userChannel, documentID, notas);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene la actividad actual del documento solicitado y filtra las que solo estan abiertas si requiere
        /// </summary>
        /// <param name="NumeroDoc">id del documento</param>
        /// <param name="onlyAbiertas">valida solo las actividades abiertas</param>
        /// <returns>ActividadFlujo</returns>
        public string glActividadEstado_GetActFlujoByNumeroDoc(int numeroDoc, bool onlyAbiertas = false)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glActividadEstado_GetActFlujoByNumeroDoc(this._userChannel, numeroDoc, onlyAbiertas);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }
        #endregion

        #region glGarantiaControl

        /// <summary>
        /// Agrega un registro al control de garantias
        /// </summary>
        /// <param name="documentID">documento actual</param>
        /// <param name="garantias">data a guardar o actualizar</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult glGarantiaControl_Add(int documentID, List<DTO_glGarantiaControl> garantias, List<int> itemsDelete)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glGarantiaControl_Add(this._userChannel, documentID, garantias,itemsDelete);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"> Filtro</param>
        /// <param name="prefijoID"> Prefijo filtro</param>
        /// <param name="docNro"> Doc nro filtro</param>
        /// <param name="estado"> Estado de la garantia</param>
        /// <returns>Lista </returns>
        public List<DTO_glGarantiaControl> glGarantiaControl_GetByParameter(DTO_glGarantiaControl filter, string prefijoID, int? docNro, byte? estado)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glGarantiaControl_GetByParameter(this._userChannel, filter, prefijoID, docNro, estado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }
        #endregion

        #region glIncumpleCambioEstado

        /// <summary>
        /// Agrega info a glIncumpleCambioEstado
        /// </summary>
        /// <param name="documentID">documento Actual</param>
        /// <param name="gestionList">gestionList</param>
        /// <returns></returns>
        public DTO_TxResult glIncumpleCambioEstado_Update(int documentID, List<DTO_GestionCobranza> gestionList)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glIncumpleCambioEstado_Update(this._userChannel, documentID, gestionList);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region glControl

        /// <summary>
        /// Actualiza glControl
        /// </summary>
        /// <param name="control">control</param> 
        /// <returns>Retorna una respuesta TxResult</returns>
        public DTO_TxResult glControl_Update(DTO_glControl control)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glControl_Update(this._userChannel, control);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae todos los glControl de una empresa
        /// </summary>
        /// <param name="isBasic">Indica si solo trae la informacion basica</param>
        /// <param name="numEmpresa">Numero de control de una empresa</param>
        /// <returns>enumeracion de glControl</returns>
        public IEnumerable<DTO_glControl> glControl_GetByNumeroEmpresa(bool isBasic, string numEmpresa)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glControl_GetByNumeroEmpresa(this._userChannel, isBasic, numEmpresa);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza una lista de registros 
        /// </summary>
        /// <param name="data">Diccionario con la lista de datos "Llave,Valor"</param>
        public void glControl_UpdateModuleData(Dictionary<string, string> data)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                this.GlobalService.glControl_UpdateModuleData(this._userChannel, data);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region glDocAnexoControl

        /// <summary>
        /// Retorna la lista de anexos de un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento control</param>
        /// <returns>Retorna la lista de anexos</returns>
        public List<DTO_glDocAnexoControl> glDocAnexoControl_GetAnexosByNumeroDoc(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glDocAnexoControl_GetAnexosByNumeroDoc(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna un anexo de un documento
        /// </summary>
        /// <param name="">replica</param>
        /// <returns>Retorna un anexo</returns>
        public DTO_glDocAnexoControl glDocAnexoControl_GetAnexosByReplica(int replica)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glDocAnexoControl_GetAnexosByReplica(this._userChannel, replica);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza los anexos de un documento
        /// </summary>
        /// <param name="mod">Modulo que guarda los anexos</param>
        /// <param name="list">lista de anexos</param>
        /// <returns></returns>
        public DTO_TxResult glDocAnexoControl_Update(ModulesPrefix mod, List<DTO_glDocAnexoControl> list)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glDocAnexoControl_Update(this._userChannel, mod, list);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }


        #endregion

        #region glDocumentoControl

        /// <summary>
        /// Anula un documento
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la operacion</param>
        /// <param name="numeroDoc">Pk del documento a anular</param>
        /// <returns>Retorna el resultado</returns>
        public DTO_TxResult glDocumentoControl_Anular(int documentID, List<int> numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glDocumentoControl_Anular(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Revierte un documento
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la operacion</param>
        /// <param name="numeroDoc">Pk del documento a anular</param>
        /// <returns>Retorna el resultado</returns>
        [OperationContract]
        public DTO_TxResult glDocumentoControl_Revertir(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glDocumentoControl_Revertir(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Le cambia el estado a un documentoControl y guarda en la bitacora
        /// </summary>
        /// <param name="documentoID">Documento que esta ejecutando la transaccion (TAREA ACTUAL) y del cual se busca la siguiente tarea</param>
        /// <param name="numeroDoc">Numero de documento - PK (NumeroDoc) de glDocumentoControl</param>
        /// <param name="estado">Nuevo estado</param>
        /// <param name="obs">Observaciones</param>
        /// <returns>Retorna el identificador de la bitacora con que se guardo la info</returns>
        public int glDocumentoControl_ChangeDocumentStatus(int documentoID, int numeroDoc, EstadoDocControl estado, string obs)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glDocumentoControl_ChangeDocumentStatus(this._userChannel, documentoID, numeroDoc, estado, obs);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }


        /// <summary>
        /// Trae un documento por el identificador unico
        /// </summary>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <returns></returns>
        public DTO_glDocumentoControl glDocumentoControl_GetByID(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glDocumentoControl_GetByID(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un registro de glDocumentoControl
        /// </summary>
        /// <param name="idPrefijo">Identificador del prefijo</param>
        /// <param name="documentoNro">Nunero de documento</param>
        /// <returns></returns>
        public DTO_glDocumentoControl glDocumentoControl_GetInternalDoc(int documentId, string idPrefijo, int numeroDocInterno)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glDocumentoControl_GetInternalDoc(this._userChannel, documentId, idPrefijo, numeroDocInterno);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un registro de glDocuemntoControl
        /// </summary>
        /// <param name="idTercero">Identificador del tercero</param>
        /// <param name="DocumentoTercero">Documento del tercero</param>
        /// <returns>Retorna el documento</returns>
        public DTO_glDocumentoControl glDocumentoControl_GetExternalDoc(int documentId, string idTercero, string numeroDocTercero)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glDocumentoControl_GetExternalDoc(this._userChannel, documentId, idTercero, numeroDocTercero);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un registro de glDocumentoControl
        /// </summary>
        /// <param name="idPrefijo">Identificador del prefijo</param>
        /// <param name="documentoNro">Nunero de documento</param>
        /// <returns></returns>
        public DTO_glDocumentoControl glDocumentoControl_GetInternalDocByCta(string cuentaID, string idPrefijo, int numeroDocInterno)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glDocumentoControl_GetInternalDocByCta(this._userChannel, cuentaID, idPrefijo, numeroDocInterno);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un registro de glDocuemntoControl
        /// </summary>
        /// <param name="idTercero">Identificador del tercero</param>
        /// <param name="DocumentoTercero">Documento del tercero</param>
        /// <returns>Retorna el documento</returns>
        public DTO_glDocumentoControl glDocumentoControl_GetExternalDocByCta(string cuentaID, string idTercero, string numeroDocTercero)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glDocumentoControl_GetExternalDocByCta(this._userChannel, cuentaID, idTercero, numeroDocTercero);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un documento relacionado a un comprobante
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Identificador del comprobante</param>
        /// <param name="compNro">Numeor de comprobante</param>
        /// <param name="estado">Estado del comprobante</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Retorna </returns>
        public DTO_glDocumentoControl glDocumentoControl_GetByComprobante(int documentId, DateTime periodo, string comprobanteID, int compNro)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glDocumentoControl_GetByComprobante(this._userChannel, documentId, periodo, comprobanteID, compNro);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el documento relacionado con una libranza
        /// </summary>
        /// <param name="libranza">Libranza</param>
        /// <param name="actFlujoID">Identificador de la actividad de flujo</param>
        /// <param name="cerradoInd">Indica si trae la actividad con estado cerrado o abierto</param>
        /// <returns>Documento</returns>
        public DTO_glDocumentoControl glDocumentoControl_GetByLibranzaSolicitud(int libranza, string actFlujoID, bool cerradoInd)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glDocumentoControl_GetByLibranzaSolicitud(this._userChannel, libranza, actFlujoID, cerradoInd);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la info de los documento s para generarles el posteo de comprobantes
        /// </summary>
        /// <param name="mod">Modulo del cual se van a traer el listado de documentos</param>
        /// <param name="contabilizado">Indica si trae los documentos que ya fueron procesados</param>
        /// <returns>Retorna el listado de documentos</returns>
        public List<DTO_glDocumentoControl> glDocumentoControl_GetForPosteo(ModulesPrefix mod, bool contabilizado)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glDocumentoControl_GetForPosteo(this._userChannel, mod, contabilizado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="ctrl">Doc Control filtro</param>
        /// <returns>Lista de Doc Control </returns>
        public List<DTO_glDocumentoControl> glDocumentoControl_GetByParameter(DTO_glDocumentoControl ctrl)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glDocumentoControl_GetByParameter(this._userChannel, ctrl);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region glLlamadasControl

        /// <summary>
        /// Trae la lista de glLlamadasControl
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        /// <returns>Retorna un listado de glLlamadasControl </returns>
        public List<DTO_glLlamadasControl> glLlamadasControl_GetByID(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glLlamadasControl_GetByID(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega un registro en glLlamadasControl
        /// </summary>
        /// <param name="documentoID">Documento ID</param>
        /// <param name="actividadFlujoID">Actividad de flujo</param>
        /// <param name="llamadasCtrl">Lista de las llamdas realizadas</param>
        /// <param name="terceroRefs">Lista de las referencias</param>
        /// <param name="sendToAprob">Indicador para establecer si se guarda el registro y se asigna la actividad de flujo o no</param>
        /// <returns>Retorna el resultado de la consulta</returns>
        public DTO_TxResult glLlamadasControl_Add(int documentoID, string actividadFlujoID, List<DTO_glLlamadasControl> llamadasCtrl, List<DTO_glTerceroReferencia> terceroRefs, bool sendToAprob)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.GlobalService.glLlamadasControl_Add(this._userChannel, documentoID, actividadFlujoID, llamadasCtrl, terceroRefs, sendToAprob);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region glMovimientoDeta

        /// <summary>
        /// Obtiene la cantidad de registros de la tabla glMovimientoDeta
        /// </summary>
        /// <param name="consulta">Filtros</param>
        /// <returns>Retorna la cantidad de registros de la consulta</returns>
        public long glMovimientoDeta_Count(DTO_glConsulta consulta)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.globalLocalService.glMovimientoDeta_Count(this._userChannel, consulta);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene una lista de registros de la tabla glMovimientoDeta
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de la pagina de consulta</param>
        /// <param name="consulta">Filtros</param>
        /// <returns>Retorna los registros de la consulta</returns>
        public List<DTO_glMovimientoDeta> glMovimientoDeta_GetPaged(int pageSize, int pageNum, DTO_glConsulta consulta)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.globalLocalService.glMovimientoDeta_GetPaged(this._userChannel, pageSize, pageNum, consulta);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// tare uno o  varios registros del glMovimientoDeta para los activos
        /// </summary>       
        public List<DTO_glMovimientoDeta> glMovimientoDeta_GetBy_ActivoFind(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.globalLocalService.glMovimientoDeta_GetBy_ActivoFind(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Mvto detalle</returns>
        public List<DTO_glMovimientoDeta> glMovimientoDeta_GetByParameter(DTO_glMovimientoDeta filter, bool isPre, bool onlyInventario = false)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.globalLocalService.glMovimientoDeta_GetByParameter(this._userChannel, filter, isPre, onlyInventario);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        public DataTable glMovimientoDeta_GetByParameterExcel(DTO_glMovimientoDeta filter, bool isPre, bool onlyInventario = false)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.globalLocalService.glMovimientoDeta_GetByParameterExcel(this._userChannel, filter, isPre, onlyInventario);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }
        /// <summary>
        /// Consulta un movimientoDetaPRE relacionado con proyectos con saldos de Inventario
        /// </summary>
        /// <param name="periodo">Periodo de saldos de inventarios</param>
        /// <param name="bodega">Bodega a consultar</param>
        /// <param name="proyectoID">Proyecto a consultar</param>
        /// <returns>lista de movimientos</returns>
        public List<DTO_glMovimientoDeta> glMovimientoDetaPRE_GetSaldosInvByProyecto(DateTime periodo, string proyectoID,bool isPre)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.globalLocalService.glMovimientoDetaPRE_GetSaldosInvByProyecto(this._userChannel, periodo,proyectoID,isPre);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }
        #endregion

        #region glDocumentoChequeoLista
        /// <summary>
        /// Retorna el Listado los Documentos Anexos
        /// </summary>
        /// <returns></returns> 
        public List<DTO_glDocumentoChequeoLista> glDocumentoChequeoLista_GetByNumeroDoc(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.globalLocalService.glDocumentoChequeoLista_GetByNumeroDoc(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }
        #endregion

        #region Consultas

        #region glConsultaFiltro

        /// <summary>
        /// Trae todos los glConsulta
        /// </summary>
        /// <param name="filtro">Informacion del filtro de la consulta</param>
        /// <returns>Devuelve los glConsulta a partir de un filtro aplicado</returns>
        public IEnumerable<DTO_glConsultaFiltro> glConsultaFiltro_GetAll(DTO_glConsultaFiltro filtro)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.glConsultaFiltro_GetAll(this._userChannel, filtro);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene una consulta especifica
        /// </summary>
        /// <param name="filtro">Informacion para fitrar la consulta</param>
        /// <returns>consulta</returns>
        public DTO_glConsultaFiltro glConsultaFiltro_Get(DTO_glConsultaFiltro filtro)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.glConsultaFiltro_Get(this._userChannel, filtro);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Adicionar una nueva consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta adicionada</returns>
        public DTO_glConsultaFiltro glConsultaFiltro_Add(DTO_glConsultaFiltro filtro)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.glConsultaFiltro_Add(this._userChannel, filtro);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Eliminar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta eliminada</returns>
        public void glConsultaFiltro_Delete(DTO_glConsultaFiltro filtro)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                this.GlobalService.glConsultaFiltro_Delete(this._userChannel, filtro);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualizar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a actualizar</param>
        /// <returns>consulta actualizada</returns>
        public DTO_glConsultaFiltro glConsultaFiltro_Update(DTO_glConsultaFiltro filtro)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.glConsultaFiltro_Update(this._userChannel, filtro);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region glConsulta

        /// <summary>
        /// Trae todos los glConsulta
        /// </summary>
        /// <param name="filtro">Informacion del filtro de la consulta</param>
        /// <returns>Devuelve los glConsulta a partir de un filtro aplicado</returns>
        public IEnumerable<DTO_glConsulta> glConsulta_GetAll(DTO_glConsulta filtro, int? userID)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.glConsulta_GetAll(this._userChannel, filtro, userID);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Adicionar una nueva consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta adicionada</returns>
        public DTO_glConsulta glConsulta_Add(DTO_glConsulta filtro)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.glConsulta_Add(this._userChannel, filtro);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Eliminar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta eliminada</returns>
        public void glConsulta_Delete(DTO_glConsulta filtro)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                this.GlobalService.glConsulta_Delete(this._userChannel, filtro);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualizar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a actualizar</param>
        /// <returns>consulta actualizada</returns>
        public DTO_glConsulta glConsulta_Update(DTO_glConsulta filtro)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.glConsulta_Update(this._userChannel, filtro);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region glConsultaSeleccion

        /// <summary>
        /// Trae todos los glConsulta
        /// </summary>
        /// <param name="filtro">Informacion del filtro de la consulta</param>
        /// <returns>Devuelve los glConsulta a partir de un filtro aplicado</returns>
        public IEnumerable<DTO_glConsultaSeleccion> glConsultaSeleccion_GetAll(DTO_glConsultaSeleccion filtro)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.glConsultaSeleccion_GetAll(this._userChannel, filtro);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene una consulta especifica
        /// </summary>
        /// <param name="filtro">Informacion para fitrar la consulta</param>
        /// <returns>consulta</returns>
        public DTO_glConsultaSeleccion glConsultaSeleccion_Get(DTO_glConsultaSeleccion filtro)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.glConsultaSeleccion_Get(this._userChannel, filtro);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Adicionar una nueva consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta adicionada</returns>
        public DTO_glConsultaSeleccion glConsultaSeleccion_Add(DTO_glConsultaSeleccion filtro)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.glConsultaSeleccion_Add(this._userChannel, filtro);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Eliminar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta eliminada</returns>
        public void glConsultaSeleccion_Delete(DTO_glConsultaSeleccion filtro)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                this.GlobalService.glConsultaSeleccion_Delete(this._userChannel, filtro);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualizar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a actualizar</param>
        /// <returns>consulta actualizada</returns>
        public DTO_glConsultaSeleccion glConsultaSeleccion_Update(DTO_glConsultaSeleccion filtro)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.glConsultaSeleccion_Update(this._userChannel, filtro);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region Consultas Generales

        /// <summary>
        /// Consultas generales
        /// </summary>
        /// <param name="vista">Nombre de la vista</param>
        /// <param name="dtoType">Tipo de DTO</param>
        /// <param name="consulta">Consulta con filtros</param>
        /// <param name="fields">Capos a mostrar en los resultados</param>
        /// <returns></returns>
        public DataTable ConsultasGenerales(string vista, Type dtoType, DTO_glConsulta consulta)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var res = this.GlobalService.ConsultasGenerales(this._userChannel, vista, dtoType, consulta);
                return res;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Global

        /// <summary>
        /// Crea un diccionario con la informacion de los combos de las maestras
        /// </summary>
        /// <param name="docId">identificador documemto</param>
        /// <param name="columnName">nombre columna</param>
        /// <param name="llave">llave de recurso</param>
        /// <returns></returns>
        public Dictionary<string, string> GetOptionsControl(int docId, string columnName, string llave)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                var result = this.globalLocalService.GetOptionsControl(this._userChannel, docId, columnName, llave);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera un reporte
        /// </summary>
        /// <param name="documentID">documento con el cual se salva el archivo</param>
        /// <param name="numeroDoc">Numero del documento con el cual se salva el archivo>
        public void GenerarReportOld(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IGlobalService));
                this.globalLocalService.GenerarReportOld(this._userChannel, documentID, numeroDoc);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IGlobalService));
                throw ex;
            }
        }

        #endregion

        #region Reporte

        /// <summary>
        /// Genera reporte de errores encontrados en la importacion
        /// </summary>
        /// <returns>Lista de campos con error</returns>
        #region Reporte TxResult

        public string Rep_TxResult(DTO_TxResult txResult)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Rep_TxResult(this._userChannel, txResult);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Reporte TxResultDetails

        public string Rep_TxResultDetails(List<DTO_TxResult> txResult)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Rep_TxResultDetails(this._userChannel, txResult);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion
        #endregion

        #endregion

        #region NominaService

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción de un usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ConsultarProgresoNomina(int documentID)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.ConsultarProgreso(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Aprobaciones - Pagos

        #region Aprobacion Nomina

        /// <summary>
        /// Aprueba ó Rechaza una solicitud de Pago de Nomina
        /// </summary>
        /// <param name="liquidaciones">listado de documentos</param>
        /// <param name="insideAnotherTx">indica si viene o no de una transacción</param>
        /// <returns>objeto de resultado</returns>
        public DTO_TxResult Nomina_AprobarLiquidacion(List<DTO_noNominaPreliminar> liquidaciones, string actividadFlujoID)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_AprobarLiquidacion(this._userChannel, liquidaciones, actividadFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Aprobacion Otros

        /// <summary>
        /// Trae las liquidaciones hacia Terceros
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="documentID">Identificador de Documento</param>
        /// <param name="Periodo">Periodo</param>
        /// <returns></returns>
        public List<DTO_noLiquidacionOtro> Nomina_GetLiquidacionOtros(List<int> documents, DateTime Periodo)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_GetLiquidacionOtros(this._userChannel, documents, Periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Aprueba una solicitud de Pago a Otros
        /// </summary>
        /// <param name="terceros">listado de terceros</param>
        /// <returns>objeto de resultado</returns>
        public DTO_TxResult Nomina_AprobarPagosTerceros(List<DTO_NominaPlanillaContabilizacion> terceros)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_AprobarPagosTerceros(this._userChannel, terceros);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }


        #endregion

        #region Pagos Nomina

        /// <summary>
        /// Realiza el Pago de la Nomina
        /// </summary>
        /// <param name="liquidaciones">listado de documentos</param>
        /// <param name="insideAnotherTx">indica si viene o no de una transacción</param>
        /// <returns>objeto de resultado</returns>
        public DTO_TxResult Nomina_PagoNomina(int documentoID, DateTime periodo, DateTime fechaDoc, List<DTO_noPagoLiquidaciones> liquidaciones, string actividadFlujoID)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_PagoNomina(this._userChannel, documentoID, periodo, fechaDoc, liquidaciones, actividadFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

		/// <summary>
        /// Procesa el Pago de la Nomina detallando el comprobante de tesoreria por empleado
        /// </summary>
        /// <param name="documentoID">documento de Pago</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="fechaDoc">Fecha Documento</param>
        /// <param name="liquidaciones">liquidaciones</param>
        /// <returns>resultado</returns>
		public List<DTO_TxResult> Nomina_PagoNominaDetallada(int documentoID, DateTime periodo, DateTime fechaDoc, List<DTO_noPagoLiquidaciones> liquidaciones)
		{
			 try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_PagoNominaDetallada(this._userChannel, documentoID, periodo, fechaDoc, liquidaciones);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
		}

        #endregion

        #endregion

        #region Aumento Salario

        /// <summary>
        /// Actualiza el Ajuste de Salario en la tabla empleados y crea la novedad de contrato
        /// </summary>
        /// <param name="lSalarios">listado de ajustes en salarios</param>
        /// <returns>listado de resultados</returns>
        public DTO_TxResult Nomina_UpdSalarioEmpleado(List<DTO_AumentoSalarial> lSalarios)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_UpdSalarioEmpleado(this._userChannel, lSalarios, false);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Cesantias e Intereses de Cesantias

        public void UpdateCesantias(int numeroDoc, decimal valorCesantias, decimal valorIntereses, bool indCesantias)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                this.NominaService.UpdateCesantias(this._userChannel, numeroDoc, valorCesantias, valorIntereses, indCesantias);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Compensatorios

        /// <summary>
        /// Obtiene el historico de compensatorios
        /// </summary>
        /// <returns>listado de compensatorios</returns>
        public List<DTO_noCompensatorios> Nomina_GetCompensatorios()
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_GetCompensatorios(this._userChannel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        // <summary>
        /// Actualiza la informacion del compensatorio
        /// </summary>
        /// <param name="compesatorio">objeto compensatorio</param>
        /// <returns>true si la operacion es exitosa</returns>
        public DTO_TxResult Nomina_UpdCompensatorio(List<DTO_noCompensatorios> compesatorio)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_UpdCompensatorio(this._userChannel, compesatorio);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Compensación Flexible

        /// <summary>
        /// Obtiene el listado de beneficios por Empleado
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="empleadoID">identificador empleado</param>
        /// <returns>listado de beneficios</returns>
        public List<DTO_noBeneficiosxEmpleado> Nomina_GetBeneficioXEmpleado(string empleadoID)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_GetBeneficioXEmpleado(this._userChannel, empleadoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Adiciona un listado de beneficios por empleado
        /// </summary>
        /// <param name="lbeneficios">listado de beneficios</param>
        /// <returns>resultado</returns>
        public DTO_TxResult Nomina_AddBeneficioXEmpleado(List<DTO_noBeneficiosxEmpleado> lbeneficios)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_AddBeneficioXEmpleado(this._userChannel, lbeneficios, false);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// Obtiene el consolidado de las liquidacion de nomina X periodo
        /// </summary>
        /// <param name="periodo">Periodo de Liquidacion</param>
        /// <returns></returns>
        public List<DTO_NominaContabilizacion> noLiquidacionesDocu_GetTotal(DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.noLiquidacionesDocu_GetTotal(this._userChannel, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #region Empleados

        /// <summary>
        /// Generar un listado de empleados de acuerdo al periodo y el estado de la liquidación
        /// </summary>
        /// <param name="documentoID">ID del documento</param>
        /// <param name="periodo">perido</param>
        /// <param name="estadoLiquidacion">estado de la liquidación</param>
        /// <param name="procesadaInd">indica si el documento ya fue procesada</param>
        /// <param name="estadoEmpleado">estado del empleado</param>
        /// <returns>listado de empleados</returns>
        public List<DTO_noEmpleado> Nomina_noEmpleadoGet(int documentoID, DateTime periodo, byte estadoLiquidacion, bool procesadaInd, byte estadoEmpleado)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_noEmpleadoGet(this._userChannel, documentoID, periodo, estadoLiquidacion, procesadaInd, estadoEmpleado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Lista los empleados segun el estado
        /// </summary>
        /// <param name="activoInd">estado</param>
        /// <param name="empleado">empleado</param>
        /// <returns>lista de empleados</returns>
        public List<DTO_noEmpleado> Nomina_SearchEmpleados(bool activoInd, string empleado)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_SearchEmpleados(this._userChannel, activoInd, empleado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }

        }

        /// <summary>
        /// Incorpora un empleado a la empresa
        /// </summary>
        /// <param name="empleado">objeto empleado</param>
        /// <param name="insideAnotherTx">indica si viende de una transacción</param>
        /// <returns>restulado de la transacción</returns>
        public DTO_TxResult Nomina_IncorporacionEmpleado(DTO_noEmpleado empleado)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_IncorporacionEmpleado(this._userChannel, empleado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Reincorpora un empleado a la empresa
        /// </summary>
        /// <param name="empleado">objeto empleado</param>
        /// <param name="insideAnotherTx">indica si viende de una transacción</param>
        /// <returns>restulado de la transacción</returns>
        public DTO_TxResult Nomina_ReinCorporacionEmpleado(DTO_noEmpleado empleado)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_ReinCorporacionEmpleado(this._userChannel, empleado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae el estado de las liquidaciones del empleado del periodo de liquidación en curso
        /// </summary>
        /// <param name="empleadoID">empleadoID</param>
        /// <returns>Estado Liquidaciones</returns>
        public DTO_noEstadoLiquidaciones Nomina_GetEstadoLiquidaciones(string empleadoID)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_GetEstadoLiquidaciones(this._userChannel, empleadoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae el estado de las liquidaciones del empleado del periodo de liquidación en curso
        /// </summary>
        /// <param name="empleadoID">empleadoID</param>
        /// <returns>Estado Liquidaciones</returns>
        public int noEmpleado_CountTerceroID(string tercero)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                return this.NominaService.noEmpleado_CountTerceroID(this._userChannel, tercero);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }
        #endregion

        #region Liquidacion Documento

        /// <summary>
        /// Obtiene la liquidaciones del empleado asociadas al tipo de documento a liquidar
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="documentoID">identificador del documento</param>
        /// <param name="empleado">empleado</param>
        /// <returns>listado de liquidaciones </returns>
        public List<DTO_noLiquidacionesDocu> Nomina_GetLiquidacionesDocu(int documentoID, DateTime periodo, DTO_noEmpleado empleado)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_GetLiquidacionesDocu(this._userChannel, documentoID, periodo, empleado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene los documentos de liquidación aprobados
        /// </summary>
        /// <param name="documentoId">identificador del documento</param>
        /// <param name="periodo">periodo</param>
        /// <returns>listado de liquidaciones aprobadas para generar pago</returns>
        public List<DTO_noPagoLiquidaciones> Nomina_NominaPagosGet(string actividadFlujoID, DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_NominaPagosGet(this._userChannel, actividadFlujoID, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el periodo de vacaciones a liquidar
        /// </summary>
        /// <param name="empleadoId">identificador del empleado</param>
        /// <param name="estado">estado de la liquidacion</param>
        public List<DTO_noLiquidacionVacacionesDeta> Nomina_GetPeriodoVacaciones(string empleadoId, bool estado)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_GetPeriodoVacaciones(this._userChannel, empleadoId, estado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Liquidacion Detalle

        /// <summary>
        /// Obtiene el documento Completo de Liquidación
        /// </summary>
        /// <param name="documentoId">identificador de documento</param>
        /// <param name="periodo">periodo de liquidacion de nomina</param>
        /// <param name="empleado">empleado</param>
        /// <returns>liquidacion completa</returns>
        public DTO_noNominaDefinitiva Nomina_NominaDefinitivaGet(int documentoId, DateTime periodo, DTO_noEmpleado empleado)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_NominaDefinitivaGet(this._userChannel, documentoId, periodo, empleado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        ///  Obtiene el detalle para efectos de Pago de Nomina
        /// </summary>
        /// <param name="periodo">Periodo de Nomina</param>
        /// <param name="empleadoId">Identificador de Empleado</param>
        /// <returns>Listado Detalle</returns>
        public List<DTO_noLiquidacionesDetalle> Nomina_GetDetallePago(int documentoID, DateTime periodo, string empleadoId)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_GetDetallePago(this._userChannel, documentoID, periodo, empleadoId);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Liquidacion Preliminar

        /// <summary>
        /// Obtiene listado de detalle liquidacion preliminar (Prenomina)
        /// </summary>
        /// <param name="contratoNOID">identificador de contrato</param>
        /// <returns>Listado de detalles liquidacion</returns>
        public List<DTO_noLiquidacionPreliminar> Nomina_LiquidacionPreliminarGetAll(int documentoID, DateTime periodo, DTO_noEmpleado empleado)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_LiquidacionPreliminarGetAll(this._userChannel, documentoID, periodo, empleado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el documento Completo de Liquidación
        /// </summary>
        /// <param name="documentoId">identificador de documento</param>
        /// <param name="periodo">periodo de liquidacion de nomina</param>
        /// <param name="empleado">empleado</param>
        /// <returns>liquidacion completa</returns>
        public List<DTO_noNominaPreliminar> Nomina_NominaPreliminarGet(string actividadFlujoID, DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_NominaPreliminarGet(this._userChannel, actividadFlujoID, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Novedades Contrato

        /// <summary>
        /// Lista las novedades de contrato por empleado
        /// </summary>
        /// <param name="empleadoID">identificador empleado</param>
        /// <returns>listado de novedades de contrato</returns>
        public List<DTO_noNovedadesContrato> Nomina_GetNovedadesContrato(string empleadoID)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_GetNovedadesContrato(this._userChannel, empleadoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Adicona una novedad de contrato 
        /// </summary>
        /// <param name="novedad">novedad de contrato</param>
        /// <returns>true si la operacion es exitosa</returns>
        public DTO_TxResult Nomina_AddNovedadesContrato(List<DTO_noNovedadesContrato> novedades)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_AddNovedadesContrato(this._userChannel, novedades);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Elimina una novedad de contrato
        /// </summary>
        /// <param name="novedad">novedad de contrato</param>
        /// <returns>true si la elimina</returns>
        public DTO_TxResult Nomina_noNovedadesContrato_Delete(DTO_noNovedadesContrato novedad)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_noNovedadesContrato_Delete(this._userChannel, novedad);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Novedades Nomina


        /// <summary>
        /// lista las novedades de nomina por empleado
        /// </summary>
        /// <param name="empleadoID">identificador de empleado</param>
        /// <returns>listado de novedades</returns>
        public List<DTO_noNovedadesNomina> Nomina_GetNovedades(string empleadoID)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_GetNovedades(this._userChannel, empleadoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega las novedades de nomina 
        /// </summary>
        /// <param name="novedades">listado de novedades</param>
        /// <returns></returns>
        public DTO_TxResult Nomina_AddNovedadNomina(List<DTO_noNovedadesNomina> novedades)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_AddNovedadNomina(this._userChannel, novedades);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }

        }

        /// <summary>
        /// Elimina una novedad de Nomina
        /// </summary>
        /// <param name="novedad">novedad de nomina</param>
        public void Nomina_DelNovedadesNomina(DTO_noNovedadesNomina novedad)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                this.NominaService.Nomina_DelNovedadesNomina(this._userChannel, novedad);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }


        #endregion

        #region Planillas de Aportes

        /// <summary>
        /// Obtiene la Liquidación de la Planilla de Aportes para el Empleado y periodo Actual
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="periodo">periodo de liquidacion del empleado</param>
        /// <returns>Planilla de Aportes</returns>
        public DTO_noPlanillaAportesDeta Nomina_GetPlanillaAportes(string empleadoID, DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_GetPlanillaAportes(this._userChannel, empleadoID, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene la Liquidación de la Planilla de Aportes para el Empleado y periodo Actual
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="periodo">periodo de liquidacion del empleado</param>
        /// <returns>Planilla de Aportes</returns>
        public List<DTO_noPlanillaAportesDeta> Nomina_GetAllPlanillaAportes(DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_GetAllPlanillaAportes(this._userChannel, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza listado de planillas de aportes modificadas
        /// </summary>
        /// <param name="lplanilla">listado de planillas</param>
        /// <param name="insideAnotherTx">si viene de alguna tx</param>
        /// <returns>resultado</returns>
        public DTO_TxResult Nomina_PlanillaAportesDeta_Upd(List<DTO_noPlanillaAportesDeta> lplanilla)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_PlanillaAportesDeta_Upd(this._userChannel, lplanilla);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Proceso trae valores planilla X tercero
        /// </summary>
        public List<DTO_NominaPlanillaContabilizacion> noPlanillaAportesDeta_GetValoreXTercero(bool isPlanilla)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.noPlanillaAportesDeta_GetValoreXTercero(this._userChannel, isPlanilla);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Planillas diarias trabajo

        /// <summary>
        /// Obtiene las planillas diarias de trabajos asociadas al empleado
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de planillas</returns>
        public List<DTO_noPlanillaDiariaTrabajo> Nomina_GetPlanillaDiaria(string empleadoID, DateTime periodo, Int16 contratoNo)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_GetPlanillaDiaria(this._userChannel, empleadoID, periodo, contratoNo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Adiciona una planilla diaria de trabajo
        /// </summary>
        /// <param name="prestamo">planilla</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        public DTO_TxResult Nomina_AddPlanillaDiaria(List<DTO_noPlanillaDiariaTrabajo> planillas)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_AddPlanillaDiaria(this._userChannel, planillas);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Prestamos

        /// <summary>
        /// Obtiene prestamos asociados al empleados
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de prestamos</returns>
        public List<DTO_noPrestamo> Nomina_GetPrestamos(string empleadoID)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_GetPrestamos(this._userChannel, empleadoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Adiciona un Prestamo
        /// </summary>
        /// <param name="prestamo">objeto prestamo</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        public DTO_TxResult Nomina_AddPrestamo(List<DTO_noPrestamo> prestamos)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_AddPrestamo(this._userChannel, prestamos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Procesos

        /// <summary>
        /// Liquida la Nomina
        /// </summary>
        /// <param name="lEmpleados">listado de empleados</param>
        /// <returns>listado de resultados</returns>
        public List<DTO_TxResult> LiquidarNomina(DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.LiquidarNomina(this._userChannel, periodo, fechaDoc, lEmpleados);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Liquidación de Contrato 
        /// </summary>
        /// <param name="periodo">Periodo de Documento</param>
        /// <param name="lEmpleados">Listado de Empleados</param>
        /// <param name="fechaRetiro">Fecha de Retiro</param>
        ///  <param name="causa">Causa Liquidación</param>
        /// <returns></returns>
        public List<DTO_TxResult> LiquidarContrato(DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados, DateTime fechaRetiro, int causa)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.LiquidarContrato(this._userChannel, periodo, fechaDoc, lEmpleados, fechaRetiro, causa);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }


        /// <summary>
        /// Liquida las Cesantias
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="periodo">periodo actual</param>
        /// <param name="lEmpleados">lista de empleados</param>
        /// <param name="fechaIniLiq">fecha inicial corte prima</param>
        /// <param name="fechaFinLiq">fecha final corte prima</param>
        /// <param name="resolucion">resolución</param>
        /// <param name="batchProgress">barra de progreso</param>
        /// <returns></returns>
        public List<DTO_TxResult> LiquidarCesantias(DateTime periodo, DateTime fechaDoc, DateTime fechaIniLiq, DateTime fechaFinLiq, DateTime fechaPago, string resolucion, TipoLiqCesantias tLiqCesantias, List<DTO_noEmpleado> lEmpleados)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.LiquidarCesantias(this._userChannel, periodo, fechaDoc, fechaIniLiq, fechaFinLiq, fechaPago, resolucion, tLiqCesantias, lEmpleados);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Liquida la Prima 
        /// </summary>
        /// <param name="periodo">periodo actual</param>
        /// <param name="lEmpleados">lista de empleados</param>
        /// <param name="fechaIniLiq">fecha inicial corte prima</param>
        /// <param name="fechaFinLiq">fecha final corte prima</param>
        /// <param name="incNomina">determina si se incluye en la Nomina</param>
        /// <param name="batchProgress">barra de progreso</param>
        /// <returns></returns>
        public List<DTO_TxResult> LiquidarPrima(DateTime periodo, DateTime fechaDoc, DateTime fechaIniLiq, DateTime fechaFinLiq, bool incNomina, List<DTO_noEmpleado> lEmpleados)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.LiquidarPrima(this._userChannel, periodo, fechaDoc, fechaIniLiq, fechaFinLiq, incNomina, lEmpleados);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Proceso para liquidar las vacaciones

        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="fechaIni">fecha inicial vacaciones</param>
        /// <param name="fechaFin">fecha final vacaciones</param>
        /// <param name="diasVacDinero">días vacaciones en dinero</param>
        /// <param name="indIncNomina">indica si incluye en nómina</param>
        /// <param name="indPrima">indica si se incluye la prima</param>
        /// <param name="resolucion">resolucion</param>
        /// <param name="batchProgress">indicador de progreso</param>
        /// <returns></returns>
        public List<DTO_TxResult> LiquidarVacaciones(DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados, DateTime fechaIniLiq, DateTime fechaFinLiq,
                                                     int diasVacTiempo, int diasVacDinero, bool indIncNomina, bool indPrima, string resolucion, DateTime fechaPagoLiq, DateTime fechaIniPagoLiq, DateTime fechaIniPendVacac, DateTime fechaFinPendVacac)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.LiquidarVacaciones(this._userChannel, periodo, fechaDoc, lEmpleados, fechaIniLiq, fechaFinLiq, diasVacTiempo, diasVacDinero, indIncNomina, indPrima, resolucion, fechaPagoLiq, fechaIniPagoLiq, fechaIniPendVacac, fechaFinPendVacac);
                return result;

            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        ///  Liquida las Provisiones
        /// </summary>
        /// <param name="periodo">periodod de liquidación</param>
        /// <param name="lEmpleados">lista de empleados</param>
        /// <param name="batchProgress">progreso</param>
        /// <returns>listad de resultados</returns>
        public List<DTO_TxResult> LiquidarProvisiones(DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.LiquidarProvisiones(this._userChannel, periodo, fechaDoc, lEmpleados);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }


        /// <summary>
        ///  Liquida las Provisiones
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="periodo">periodod de liquidación</param>
        /// <param name="lEmpleados">lista de empleados</param>
        /// <param name="batchProgress">progreso</param>
        /// <returns>listad de resultados</returns>
        public List<DTO_TxResult> LiquidarPlanilla(DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.LiquidarPlanilla(this._userChannel, periodo, fechaDoc, lEmpleados);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }


        /// <summary>
        /// Calculo los días reales de vacaciones del empleado
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="fechaIni">fecha inicial vacaciones</param>
        /// <param name="fechaFin">fecha final vacaciones</param>
        /// <returns>número de días de vacaciones</returns>
        public int CalcularDiasVacaciones(string empleadoID, DateTime fechaIni, DateTime fechaFin, out decimal diasCausados)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.CalcularDiasVacaciones(this._userChannel, empleadoID, fechaIni, fechaFin, out diasCausados);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Contabiliza la Nomina para el Periodo Actual
        /// </summary>
        /// <param name="periodo">Periodo de la Nomina</param>
        /// <returns>Resultado</returns>
        public List<DTO_TxResult> Proceso_ContabilizarNomina(int documentoID, DateTime periodo, DateTime fechaDoc, List<DTO_NominaContabilizacionDetalle> liquidaciones, byte procesarSel)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Proceso_ContabilizarNomina(this._userChannel, documentoID, periodo, fechaDoc, liquidaciones, procesarSel);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Proceso de contabilización de la Planilla de Aportes
        /// </summary>
        /// <param name="documentoID">identificador del documento de Pagos</param>
        /// <param name="periodo">Periodo actual de la Nomina</param>
        /// <param name="liquidaciones">liquidaciones de la Planilla</param>
        /// <returns>listado de resultados</returns>
        public List<DTO_TxResult> Proceso_ContabilizarPlanilla(int documentoID, DateTime periodo, List<DTO_noPlanillaAportesDeta> liquidaciones)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Proceso_ContabilizarPlanilla(this._userChannel, documentoID, periodo, liquidaciones);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Contabiliza las Provisiones para el Periodo Actual
        /// </summary>
        /// <param name="periodo">Periodo de la Nomina</param>
        /// <param name="batchProgress">Barra de Progreso</param>
        /// <returns>Resultado</returns>
        public List<DTO_TxResult> Proceso_ContabilizarProvisiones(int documentoID, DateTime periodo, List<DTO_noProvisionDeta> liquidaciones)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Proceso_ContabilizarProvisiones(this._userChannel, documentoID, periodo, liquidaciones);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Migración Información Historica Nomina
        /// </summary>
        /// <param name="lEmpleados">liquidaciones Empleados</param>
        /// <returns></returns>
        public List<DTO_TxResult> Proceso_MigracionNomina(int documentoID, List<DTO_noMigracionNomina> lEmpleados)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Proceso_MigracionNomina(this._userChannel, documentoID, lEmpleados);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Migración Información Historica Nomina
        /// </summary>
        /// <param name="lProvisiones">Provisiones</param>
        public List<DTO_TxResult> Proceso_MigracionProvisiones(int documentoID, List<DTO_noProvisionDeta> lProvisiones)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Proceso_MigracionProvisiones(this._userChannel, documentoID, lProvisiones);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene los documentos de liquidación aprobados por periodo
        /// </summary>
        /// <param name="documentoId">identificador del documento</param>
        /// <param name="periodo">periodo</param>
        /// <param name="tipoLiquida">Tipo de liquidacion</param>
        /// <returns>listado de liquidaciones aprobadas para generar boletas pago</returns>
        public List<DTO_NominaEnvioBoleta> Proceso_noLiquidacionesDocu_GetNominaLiquida(int documentoID, DateTime periodo, byte tipoLiquida)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Proceso_noLiquidacionesDocu_GetNominaLiquida(this._userChannel, documentoID, periodo, tipoLiquida);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Provisiones

        /// <summary>
        /// Obtiene un listado de la liquidación de Provisiones del empleado en el periodo
        /// </summary>
        /// <param name="periodo">periodo</param>
        /// <param name="contratoNOID">número contrato empleado</param>
        /// <returns>lista de provisiones</returns>
        public List<DTO_noProvisionDeta> Nomina_ProvisionDeta_Get(DateTime periodo, int contratoNOID)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_ProvisionDeta_Get(this._userChannel, periodo, contratoNOID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Traslados

        /// <summary>
        /// Obtiene los traslados
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de traslados</returns>
        public List<DTO_noTraslado> Nomina_GetTraslados(string empleadoID)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_GetTraslados(this._userChannel, empleadoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Adiciona un traslado
        /// </summary>
        /// <param name="prestamo">objeto traslado</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        public DTO_TxResult Nomina_AddTraslado(List<DTO_noTraslado> traslados)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Nomina_AddTraslado(this._userChannel, traslados);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Reportes
        /// <summary>
        /// Funcion q envia a imprimir la Prenomina
        /// </summary>
        /// <param name="numeroDoc">Numero del documento</param>
        /// <param name="lEmpleados">Lista de dto_MasterBasic</param>
        /// <param name="_ldetalle">Lista de detalles de dto_NoLiquidacionPreliminar</param>
        public void noPrint(int numeroDoc, DateTime periodo, List<DTO_MasterBasic> lEmpleados, List<DTO_noLiquidacionPreliminar> _ldetalle)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                this.NominaService.Print(this._userChannel, numeroDoc, periodo, lEmpleados, _ldetalle);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion q envia a imprimir las Vacaciones
        /// </summary>
        /// <param name="numeroDoc">Numero del documento</param>
        /// <param name="empleado">Info del empleado en el dto noEmpleado</param>
        /// <param name="_ldetalle">Lista de detalles de dto_NoLiquidacionPreliminar</param>
        /// void PrintVacaciones(Guid channel, int numeroDoc, DTO_noEmpleado empleado, DTO_noLiquidacionesDocu liquidacion, List<DTO_noLiquidacionPreliminar> _lDetalles, bool isApro)
        public void noPrintVacaciones(int numeroDoc, DTO_noEmpleado empleado, DTO_noLiquidacionesDocu liquidacion, List<DTO_noLiquidacionPreliminar> _ldetalle, bool isAprob)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                this.NominaService.PrintVacaciones(this._userChannel, numeroDoc, empleado, liquidacion, _ldetalle, isAprob);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene un datatable con la info de Tesoreria segun filtros
        /// </summary>
        /// <param name="documentoID">Tipo de Reporte a Generar</param>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="empleadoID">Empleado</param>
        /// <param name="operacionNoID">Operacion Nomina</param>
        /// <param name="conceptoNoID">Concepto Nomina</param>
        /// <param name="areaFuncID">Area Funcional</param>
        /// <param name="fondoID">Fondo Nom</param>
        /// <param name="cajaID">Caja Nomina</param>
        /// <param name="otroFilter">Filtro adicional</param>
        /// <param name="agrup">Agrupar u ordenar</param>
        /// <param name="romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable Reportes_No_NominaToExcel(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string empleadoID, string operacionNoID,
                                                   string conceptoNoID, string areaFuncID, string fondoID, string cajaID, string terceroID, object otroFilter, byte? agrup, byte? romp)
        {

            try
            {
                this.CreateService(typeof(INominaService));
                return this.NominaService.Reportes_No_NominaToExcel(this._userChannel, documentoID, tipoReporte, fechaIni, fechaFin, empleadoID, operacionNoID, conceptoNoID, areaFuncID, fondoID, cajaID, terceroID, otroFilter, agrup, romp);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }


        /// <summary>
        /// Genera un archivo de las planillas de los empleados
        /// </summary>
        /// <param name="rutaArchivo">Ruta de archivo a guardar</param>
        /// <param name="planillas">Planillas a migrar</param>
        /// <returns>nombre del archivo</returns>
        public string Reportes_No_GenerarArchivoPlanilla(string rutaArchivo, List<DTO_noPlanillaAportesDeta> planillas)
        {

            try
            {
                this.CreateService(typeof(INominaService));
                return this.NominaService.Reportes_No_GenerarArchivoPlanilla(this._userChannel, rutaArchivo, planillas);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }
        #endregion

        #region Reversiones

        /// <summary>
        /// Proceso para revertir la liquidación de la Nomina
        /// </summary>
        /// <param name="documentoID">Documento de Nomina</param>
        /// <param name="periodo">Periodo de Liquidacion</param>
        /// <param name="lEmpleados">lista de Empleados</param>        
        public DTO_TxResult Nomina_RevertirLiqNomina(Guid channel, int documentoID, DateTime periodo, List<DTO_noEmpleado> lEmpleados)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.RevertirLiqNomina(this._userChannel, documentoID, periodo, lEmpleados);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region InventariosService

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción de un usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ConsultarProgresoInventarios(int documentID)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.ConsultarProgreso(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        #endregion

        #region Comprobantes

        /// <summary>
        /// Proceso de posteo de comprobantes para el modulo de inventarios
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <returns>Retorna la lista de resultados (uno por cada comprobante)</returns>
        public List<DTO_TxResult> PosteoComprobantesInv(int documentID)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.PosteoComprobantes(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Aprueba una lista de posteo decomprobantes
        /// </summary>
        /// <param name="documentID">Identificador del documento qu eejecuta la transaccion</param>
        /// <param name="mod">Modulo del cual se esta aprobando el posteo</param>
        /// <param name="docs">Lista de documentos a aprobar</param>
        /// <returns>Retorna el resultado dela operacion</returns>
        public List<DTO_TxResult> AprobarPosteoInv(int documentID, string actividadFlujoID, ModulesPrefix currentMod, List<DTO_glDocumentoControl> docs)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.AprobarPosteo(this._userChannel, documentID, actividadFlujoID, currentMod, docs);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        #endregion

        #region Movimientos

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="transaccion">Dto del movimiento completo</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject Transaccion_Add(int documentID, DTO_MvtoInventarios transaccion, bool update, out int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.Transaccion_Add(this._userChannel, documentID, transaccion, update, out numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        ///Obtiene una transaccion manual
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Asociado</param>
        /// <param name="trasladoNotaEnvio">Indica si es traslado de Nota Envio</param>
        /// <returns>DTO_inControlSaldosCostos</returns>
        public DTO_MvtoInventarios Transaccion_Get(int documentID, int numeroDoc, bool trasladoNotaEnvio = false)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.Transaccion_Get(this._userChannel, documentID, numeroDoc, trasladoNotaEnvio);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Envia para aprobacion una transaccion manual
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="numeroDoc">numeroDoc de la transacción</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Transaccion_SendToAprob(int documentID, int numeroDoc, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.Transaccion_SendToAprob(this._userChannel, documentID, numeroDoc, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        ///Obtiene la relacion de saldos y costos para las salidas por referencia
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="saldo">dto para filtrar</param>
        /// <param name="costoTotal">costoTotal</param>
        public decimal Transaccion_SaldoExistByReferencia(int documentID, DTO_inControlSaldosCostos saldo, ref DTO_inCostosExistencias costo)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.Transaccion_SaldoExistByReferencia(this._userChannel, documentID, saldo, ref costo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="ctrlSaldoCosto"></param>
        /// <returns>Dto de Control de saldos y costos</returns>
        public List<DTO_inControlSaldosCostos> inControlSaldosCostos_GetByParameter(int documentID, DTO_inControlSaldosCostos ctrlSaldoCosto)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.inControlSaldosCostos_GetByParameter(this._userChannel, documentID, ctrlSaldoCosto);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Retorna una lista de notas envio 
        /// </summary>        
        /// <returns>Retorna una lista de notas envio</returns>
        public List<DTO_NotasEnvioResumen> NotasEnvio_GetResumen(int documentID)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.NotasEnvio_GetResumen(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Recibe o devuelve entradas de una Nota de Envio
        /// </summary>
        /// <param name="documentID"> documento relacionado</param>
        /// <param name="notaEnvio">resumen de mvtos</param>
        /// <param name="actFlujoID">Actividad actual</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        public DTO_SerializedObject NotaEnvio_RecibirDevolver(int documentID, DTO_MvtoInventarios notaEnvio, string actFlujoID, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.NotaEnvio_RecibirDevolver(this._userChannel, documentID, notaEnvio, actFlujoID, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene una lista de movimientos Docu
        /// </summary>
        /// <param name="documentoID">Documento Asociado</param>
        /// <param name="header">Filtro para consulta</param>
        /// <returns>List DTO_inMovimientoDocu </returns>
        public List<DTO_inMovimientoDocu> inMovimientoDocu_GetByParameter(int documentoID, DTO_inMovimientoDocu header)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.inMovimientoDocu_GetByParameter(this._userChannel, documentoID, header);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza la tabla inMovimientoDocu 
        /// </summary>
        /// <param name="documentoID">Documento asociado</param>
        /// <param name="header">dto a ingresar</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult inMovimientoDocu_Upd(int documentoID, DTO_inMovimientoDocu header)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.inMovimientoDocu_Upd(this._userChannel, documentoID, header);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }
        #endregion

        #region Inventario Fisico
        /// <summary>
        /// Guardar el inventario fisico
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="bodega">Bodega relacionada</param>
        /// <param name="periodo">Periodo del inventario fisico</param>
        /// <param name="numeroDoc">Numero Doc relacionado</param>
        /// <param name="fisicoInventario"></param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject InventarioFisico_Add(int documentID, string bodega, DateTime periodo, out int numeroDoc, List<DTO_inFisicoInventario> fisicoInventario)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.InventarioFisico_Add(this._userChannel, documentID, bodega, periodo, out numeroDoc, fisicoInventario);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        ///Obtiene un inventario fisico
        /// </summary>
        /// <param name="documentID">Documento Asociado</param>
        /// <param name="fisicoInv">Dto filtro</param>
        /// <returns> lista de DTO_inFisicoInventario</returns>
        public List<DTO_inFisicoInventario> InventarioFisico_Get(int documentID, DTO_inFisicoInventario fisicoInv)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.InventarioFisico_Get(this._userChannel, documentID, fisicoInv);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Envia para aprobacion un inventario fisico
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="actividadFlujoID">Actividad actual</param>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject InventarioFisico_SendToAprob(int documentID, string actividadFlujoID, int numeroDoc, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.InventarioFisico_SendToAprob(this._userChannel, documentID, actividadFlujoID, numeroDoc, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de inventario fisico para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un listado</returns>
        public List<DTO_InvFisicoAprobacion> InventarioFisico_GetPendientesByModulo(ModulesPrefix mod, string actividadFlujoID)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.InventarioFisico_GetPendientesByModulo(this._userChannel, mod, actividadFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Aprueba o rechaza Inventario Fisico
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="invFisico">lista de inventarios de bodega</param>
        ///  <param name="actFlujoID">Actividad reciente</param>
        /// <param name="updDocCtrl">actualiza el documento control</param>
        /// <param name="createDoc">genera archivo fisico</param>
        /// <param name="batchProgress">progreso proceso</param>
        /// <returns>listado de DT</returns>
        public List<DTO_SerializedObject> InventarioFisico_AprobarRechazar(int documentID, List<DTO_InvFisicoAprobacion> invFisico, string actFlujoID, bool updDocCtrl, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.InventarioFisico_AprobarRechazar(this._userChannel, documentID, invFisico, actFlujoID, updDocCtrl, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }

        }

        /// <summary>
        /// Elimina los saldos guardados de la bodega actual
        /// </summary>        
        public void InventarioFisico_Delete(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                this.InventariosService.InventarioFisico_Delete(this._userChannel, numeroDoc);
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }
        #endregion

        #region Distribucion Costos

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="transaccion">Dto del movimiento completo</param>
        /// <param name="costosDist">Dto de Distribucion Costos</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject inDistribucionCostos_Add(int documentID, DTO_MvtoInventarios transaccion, List<DTO_inDistribucionCosto> costosDist, bool update, out int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.inDistribucionCostos_Add(this._userChannel, documentID, transaccion, costosDist, update, out numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        ///Obtiene una lista de Distribucion Costo
        /// </summary>
        /// <param name="documentID">Documento Asociado</param>
        /// <param name="numeroDoc">numero Doc inv</param>
        /// <param name="byNroDocFact">Indica si filtra por numero doc de la factura</param>
        /// <returns> lista de DTO_inDistribucionCosto</returns>
        public List<DTO_inDistribucionCosto> inDistribucionCosto_GetByNumeroDoc(int documentID, int numeroDoc, bool byNroDocFact)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.inDistribucionCosto_GetByNumeroDoc(this._userChannel, documentID, numeroDoc, byNroDocFact);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }
        #endregion

        #region Liquidacion de Importacion

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="importacion">Dto del movimiento completo</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject LiquidacionImportacion_Add(int documentID, DTO_LiquidacionImportacion importacion, bool update, out int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.LiquidacionImportacion_Add(this._userChannel, documentID, importacion, update, out numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        #endregion

        #region Deterioro / Revalorizacion

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="deterioro">Dto del movimiento completo</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject Deterioro_Add(int documentID, DTO_MvtoInventarios deterioro, bool update, out int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.Deterioro_Add(this._userChannel, documentID, deterioro, update, out numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Envia para aprobacion un deterioro
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="actividadFlujoID">Actividad actual</param>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject Deterioro_SendToAprob(int documentID, string actividadFlujoID, int numeroDoc, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.Deterioro_SendToAprob(this._userChannel, documentID, actividadFlujoID, numeroDoc, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Aprueba o rechaza un Deterioro
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="deterioro">lista de inventarios de bodega</param>
        ///  <param name="actFlujoID">Actividad reciente</param>
        /// <param name="createDoc">genera archivo fisico</param>
        /// <param name="batchProgress">progreso proceso</param>
        /// <returns>listado de DTO</returns>
        public List<DTO_SerializedObject> Deterioro_AprobarRechazar(int documentID, List<DTO_inDeterioroAprob> deterioro, string actFlujoID, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.Deterioro_AprobarRechazar(this._userChannel, documentID, deterioro, actFlujoID, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }

        }

        /// <summary>
        /// Trae un listado de deterioro fisico para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        ///  <param name="actFlujoID">Actividad Actual</param>
        /// <returns>Retorna un listado</returns>
        public List<DTO_inDeterioroAprob> inMovimientoDocu_GetPendientesByModulo(ModulesPrefix mod, string actFlujoID)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.inMovimientoDocu_GetPendientesByModulo(this._userChannel, mod, actFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Traslado Saldos de inventario a un nuevo periodo
        /// </summary>
        /// <param name="documentID"> documento relacionado</param>
        /// <param name="periodoID">periodo actual del modulo</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        public DTO_TxResult Proceso_TrasladoSaldosInventario(int documentID, DateTime periodoOld)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.Proceso_TrasladoSaldosInventario(this._userChannel, documentID, periodoOld);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        #endregion

        #region  Orden Salida

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="ordenSalida">Dto de la Orden Salida</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject OrdenSalida_Add(int documentID, DTO_OrdenSalida ordenSalida, bool update, out int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.OrdenSalida_Add(this._userChannel, documentID, ordenSalida, update, out numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene una Orden de Salida
        /// </summary>
        /// <param name="bodegaID">Bodega a Filtrar</param>
        /// <returns>Una orden</returns>
        public DTO_OrdenSalida OrdenSalida_GeyByBodega(string bodegaID, int? numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.OrdenSalida_GeyByBodega(this._userChannel, bodegaID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Aprueba la Orden de Salida y cambia el estado
        /// </summary>
        /// <param name="documentID">Documento</param>
        /// <param name="orden">Orden de salida</param>
        /// <returns>Una orden</returns>
        public DTO_SerializedObject OrdenSalida_ApproveOrden(int documentID, DTO_OrdenSalida orden)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.OrdenSalida_ApproveOrden(this._userChannel, documentID, orden);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera un  mvto de salida de Inventarios a partir de la ORden de Salida
        /// </summary>
        /// <param name="documentID">Documento</param>
        /// <param name="orden">Orden de salida</param>
        /// <returns>Una orden</returns>
        public DTO_SerializedObject OrdenSalida_ApproveMvtoInv(int documentID, DTO_OrdenSalida orden)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.OrdenSalida_ApproveMvtoInv(this._userChannel, documentID, orden);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }


        #endregion

        #region Reportes
        /// <summary>
        /// Genera el reporte de acuerdo al tipo
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="_listFisicoInventario">Lista de InvFisico</param>
        /// <param name="tipoReporte">Tipo del reporte</param>
        public string ReporteInvFisico(int documentID, string bodegaID, DateTime periodoID, List<DTO_inFisicoInventario> _listFisicoInventario, InventarioFisicoReportType tipoReporte)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var response = this.ReportesService.ReporteInvFisico(this._userChannel, documentID, bodegaID, periodoID, _listFisicoInventario, tipoReporte);
                return response;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }

        }
        #endregion

        #region Relacion Prestamos
        /// <summary>
        ///Crea un dto de reporte 
        ///</summary>
        /// <param name="proyecto">Documento Asosiado</param>
        ///<param name="inReferenciaID">Lista de items a mostrar</param>
        ///<returns>Resultado</returns>
        public string Report_RelacionPrestamos(string proyecto, string inReferenciaID)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var response = this.ReportesService.Report_RelacionPrestamos(this._userChannel, proyecto, inReferenciaID);
                return response;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }

        }

        #endregion

        #region Documentos Inventario
        /// <summary>
        ///Crea un dto de reporte 
        ///</summary>
        /// <param name="proyecto">Documento Asosiado</param>
        ///<param name="inReferenciaID">Lista de items a mostrar</param>
        ///<returns>Resultado</returns>
        public string Report_Movimientos(string movimientoID, string bodegaID, string proyectoID, string tipoReporte, DateTime fechaIni, byte resumido)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var response = this.ReportesService.Report_Movimientos(this._userChannel,movimientoID,bodegaID, proyectoID,tipoReporte,fechaIni, resumido);
                return response;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }

        }

        #endregion


        #region Consultas
        /// <summary>
        /// Funcion que obriene la informacion del serial
        /// </summary>
        /// <param name="serial">Filtro SerialID</param>
        /// <param name="bodegaID">Filtro de BodegaID</param>
        /// <param name="inReferenciaID">Filtro de Referencia</param>
        /// <param name="inCliente">Filtro de TerceroID</param>
        /// <returns>Lista de detalles de las consultas</returns>
        public List<DTO_inQuerySeriales> inSaldosExistencias_GetBySerial(string serial, string bodegaID, string inReferenciaID, string inCliente)
        {
            try
            {
                this.CreateService(typeof(IInventariosService));
                var result = this.InventariosService.inSaldosExistencias_GetBySerial(this._userChannel, serial, bodegaID, inReferenciaID, inCliente);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IInventariosService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region OpConjuntasService

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción de un usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ConsultarProgresoOpConjuntas(int documentID)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.ConsultarProgreso(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        #endregion

        #region OpConjuntas

        #region Billing

        /// <summary>
        /// Proceso para hacer la particion del Billing
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Billing_Particion(int documentID, string actividadFlujoID)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.Billing_Particion(this._userChannel, documentID, actividadFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Procesa el billing y generas los comprobantes Gross
        /// </summary>
        /// <param name="actividadFlujoID">Identificador de la actividad de flujo</param>
        /// <param name="periodo">Periodo del billing</param>
        /// <returns>Retirna al resultado de la operación</returns>
        public DTO_TxResult ProcesarBilling(int documentID, string actividadFlujoID, DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.ProcesarBilling(this._userChannel, documentID, actividadFlujoID, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        #endregion

        #region CashCall

        /// <summary>
        /// Genera el proceso de cash call
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult CashCall_Procesar(int documentID, string actividadFlujoID, DateTime periodoID)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.ProcesarBilling(this._userChannel, documentID, actividadFlujoID, periodoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        #endregion

        #region Distribucion

        /// <summary>
        /// Distribuye entre los socios de acuerdo a los porcentajes dados
        /// </summary>
        /// <param name="periodo">PeriodoID</param>
        public DTO_TxResult Proceso_ocDetalleLegalizacion_Distribucion(DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.Proceso_ocDetalleLegalizacion_Distribucion(this._userChannel, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }

        }

        #endregion

        #region Consultas

        /// <summary>
        /// Obtiene de acuerdo a un filtro la info de Operaciones de detalle mensual
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="filter">objeto filtro</param>
        /// <param name="tipoInforme">Tipo de Informe</param>
        /// <param name="proType">Tipo de Proyecto</param>
        /// <param name="tipoMda">Tipo de Moneda</param>
        /// <param name="mdaOrigen">Tipo de Moneda Origen</param>
        /// <returns>Lista de informe  mensual</returns>
        public List<DTO_QueryInformeMensualCierre> ocDetalleLegalizacion_GetInfoMensual(int documentID, DTO_QueryInformeMensualCierre filter, byte tipoInforme, ProyectoTipo proType, TipoMoneda_LocExt tipoMda, TipoMoneda mdaOrigen)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.ocDetalleLegalizacion_GetInfoMensual(this._userChannel, documentID, filter, tipoInforme, proType, tipoMda, mdaOrigen);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Planeacion


        /// <summary>
        ///  Obtiene el detalle de los proyectos
        /// </summary>
        /// <param name="fechaCorte">Fecha corte</param>
        /// <param name="loadTareas">Carga las tareas del proyecto</param>
        /// <param name="loadDetalleTarea">carga el detalle por tarea</param>
        /// <param name="loadDetalle">carga el detalle por proyecto</param>
        /// <returns>resultado</returns>
        public List<DTO_QueryEjecucionPresupuestal> plEjecucionPresupuestal(DateTime fechaCorte)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.plEjecucionPresupuestal(this._userChannel, fechaCorte);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        ///  Obtiene el detalle de los proyectos
        /// </summary>
        /// <param name="fechaCorte">Fecha corte</param>
        /// <param name="loadTareas">Carga las tareas del proyecto</param>
        /// <param name="loadDetalleTarea">carga el detalle por tarea</param>
        /// <param name="loadDetalle">carga el detalle por proyecto</param>
        /// <returns>resultado</returns>
        public List<DTO_QueryIndicadores> plIndicadores(DateTime fechaCorte)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.plIndicadores(this._userChannel, fechaCorte);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }


        #region Presupuesto

        /// <summary>
        /// Trae la informacion de los presupuestos consolidados con un filtro
        /// </summary>
        /// <returns>Presupuesto consolidado</returns>
        public DTO_Presupuesto Presupuesto_GetConsolidadoTotal(int documentID, string proyectoID, DateTime periodoID, byte proyectoTipo, string contratoID, string actividadID, string campo)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.Presupuesto_GetConsolidadoTotal(this._userChannel, documentID, proyectoID, periodoID, proyectoTipo, contratoID, actividadID, campo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos de un usuario por proyecto y periodo
        /// </summary>
        /// <returns></returns>
        public DTO_Presupuesto Presupuesto_GetNuevo(int documentoID, string proyectoID, DateTime periodoID, bool orderByAsc = true)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.Presupuesto_GetNuevo(this._userChannel, documentoID, proyectoID, periodoID, orderByAsc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Carga la información de un nuevo presupuesto
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta el proceso</param>
        /// <param name="actividadFlujoID">Actuvidad de flujo relacionada</param>
        /// <param name="periodoPresupuesto">Periodo de presupuesto</param>
        /// <param name="tc">Tasa de cambio</param>
        /// <param name="presupuesto">Presupuesto que se va a geenrar</param>
        /// <param name="isAnotherTx">Revisa si se esta ejecutando en otar transaccion</param>
        /// <returns></returns>
        public DTO_SerializedObject Presupuesto_Nuevo(int documentoID, DateTime periodoPresupuesto, string proyectoID, decimal tc, DTO_Presupuesto presupuesto, bool onlySave)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.Presupuesto_Add(this._userChannel, documentoID, periodoPresupuesto, proyectoID, tc, presupuesto, onlySave);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos para aprobar por proyecto y periodo
        /// </summary>
        /// <returns></returns>
        public List<DTO_PresupuestoAprob> Presupuesto_GetNuevosForAprob(int documentoID, string actividadFlujo)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.Presupuesto_GetNuevosForAprob(this._userChannel, documentoID, actividadFlujo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos para aprobar por proyecto y periodo
        /// </summary>
        /// <param name="areaPresupuestalID">Area presupuestal</param>
        /// <param name="centroCostoID">Centro de costo</param>
        /// <returns></returns>
        public List<DTO_PresupuestoDetalle> Presupuesto_GetNuevosForAprobDetails(string proyectoID, DateTime periodoID, string areaPresupuestalID, string centroCostoID)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.Presupuesto_GetNuevosForAprobDetails(this._userChannel, proyectoID, periodoID, areaPresupuestalID, centroCostoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Aprueba una lista de presupuestos
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la transaccion</param>
        /// <param name="actividadFlujoID">Identificador de la actividad de flujo</param>
        /// <param name="proyectoID">Identificador del proyecto</param>
        /// <param name="periodoID">Periodo del presupuesto</param>
        /// <param name="areasIDs">Lista de areas para aprobar</param>
        /// <returns>Retorna una lista de resultados o  alarmas</returns>
        public List<DTO_SerializedObject> Presupuesto_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_PresupuestoAprob> docs)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.Presupuesto_AprobarRechazar(this._userChannel, documentID, actividadFlujoID, docs);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos consolidados con un filtro
        /// </summary>
        /// <returns>Lista de presupuestos</returns>
        public List<DTO_plPresupuestoTotal> plPresupuestoTotal_GetByParameter(DTO_plPresupuestoTotal filter)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.plPresupuestoTotal_GetByParameter(this._userChannel, filter);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        ///  Trae la informacion de planeacion proveedores
        /// </summary>
        /// <param name="consActLinea"> Consecutivo filtro</param>
        /// <returns>Lista</returns>
        public List<DTO_plPlaneacion_Proveedores> plPlaneacion_Proveedores_GetByConsActLinea(int consActLinea)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.plPlaneacion_Proveedores_GetByConsActLinea(this._userChannel, consActLinea);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Aprueba Un Presupuesto contable
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la transaccion</param>
        /// <param name="numeroDoc">Identificador del doc</param>
        /// <param name="periodoID">Periodo del presupuesto</param>
        /// <returns></returns>
        public DTO_TxResult PresupuestoContable_Aprobar(int documentID, int numeroDoc, DateTime periodoDoc)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.PresupuestoContable_Aprobar(this._userChannel, documentID, numeroDoc, periodoDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }
        #endregion

        #region Presupuesto PxQ

        /// <summary>
        /// Carga la información de un nuevo presupuesto
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta el proceso</param>
        /// <param name="periodoPresupuesto">periodo del presupuesto</param>
        /// <param name="proyectoID">Proyecto</param>
        /// <param name="tc">Tasa de cambio</param>
        /// <param name="presupuesto">Presupuesto que se va a generar</param>
        /// <param name="isAnotherTx">Revisa si se esta ejecutando en otar transaccion</param>
        /// <returns>Resultado</returns>
        public DTO_SerializedObject PresupuestoPxQ_Add(int documentoID, DateTime periodoPresupuesto, string proyectoID, decimal tc, DTO_Presupuesto presupuesto, bool onlySave)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.PresupuestoPxQ_Add(this._userChannel, documentoID, periodoPresupuesto, proyectoID, tc, presupuesto, onlySave);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos PxQ con un filtro
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="proyecto"></param>
        /// <param name="periodo"></param>
        /// <param name="proyectoTipo"></param>
        /// <param name="contratoID"></param>
        /// <param name="actividadID"></param>
        /// <param name="areaFisica"></param>
        /// <param name="validState"></param>
        /// <returns></returns>
        public DTO_Presupuesto PresupuestoPxQ_GetPresupuestoPxQConsolidado(int documentID, string proyecto, DateTime periodo, byte proyectoTipo, string contratoID, string actividadID, string areaFisica, byte estadoDocCtrl)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.PresupuestoPxQ_GetPresupuestoPxQConsolidado(this._userChannel, documentID, proyecto, periodo, proyectoTipo, contratoID, actividadID, areaFisica, estadoDocCtrl);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos PxQ con un filtro
        /// </summary>
        /// <param name="DocumentID">documento Actual</param>
        /// <param name="Proyecto">proyecto</param>
        /// <param name="Periodo">periodo actual</param>
        /// <param name="ProyectoTipo">tipo de Proyecto</param>
        /// <param name="ContratoID">Contrato</param>
        /// <param name="ActividadID">Actividad</param>
        /// <returns>Presupuesto detallado</returns>
        public DTO_Presupuesto PresupuestoPxQ_GetDataPxQ(int documentID, byte tipoProyecto, string proyectoID, DateTime periodo, string contratoID, string actividadID, string areaFisicaID, string lineaPresupID, string recursoID)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.PresupuestoPxQ_GetDataPxQ(this._userChannel, documentID, tipoProyecto, proyectoID, periodo, contratoID, actividadID, areaFisicaID, lineaPresupID, recursoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Envia para aprobacion 
        /// </summary>
        /// <param name="currentMod">Modulo que esta ejecutando la operacion</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="userId">Usuario que ejecuta la transaccion</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject PresupuestoPxQ_SendToAprob(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.PresupuestoPxQ_SendToAprob(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        #endregion

        #region Reversion Documentos

        /// <summary>
        /// Revierte una Documento de Planeacion
        /// </summary>
        /// <param name="documentID">identificador documento</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Planeacion_Reversion(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.Planeacion_Reversion(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        #endregion

        #region Procesos

        /// <summary>
        /// Ejecutar proceso de cierre Legalizacion en Planeacion 
        /// </summary>
        /// <param name="periodo">Periodo de Cierre</param>
        public DTO_TxResult Proceso_plCierreLegalizacion_Cierre(DateTime periodo)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.Proceso_plCierreLegalizacion_Cierre(this._userChannel, periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Obtiene de acuerdo a un filtro la info de presupuesto de cierre mensual
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="filter">objeto filtro</param>
        /// <param name="tipoInforme">Tipo de Informe</param>
        /// <param name="proType">Tipo de Proyecto</param>
        /// <param name="tipoMda">Tipo de Moneda</param>
        /// <param name="mdaOrigen">Tipo de Moneda Origen</param>
        /// <returns>Lista de informe  mensual</returns>
        public List<DTO_QueryInformeMensualCierre> plCierreLegalizacion_GetInfoMensual(int documentID, DTO_QueryInformeMensualCierre filter, byte tipoInforme, ProyectoTipo proType, TipoMoneda_LocExt tipoMda, TipoMoneda mdaOrigen)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.plCierreLegalizacion_GetInfoMensual(this._userChannel, documentID, filter, tipoInforme, proType, tipoMda, mdaOrigen);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene de acuerdo a un filtro la info de Estado de presupuesto por periodo
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="filter">objeto filtro</param>
        /// <param name="proType">Tipo de Proyecto</param>
        /// <param name="tipoMda">Tipo de Moneda</param>
        /// <param name="mdaOrigen">Tipo de Moneda Origen</param>
        /// <returns>Lista de informe  mensual</returns>
        public List<DTO_QueryEstadoEjecucion> plCierreLegalizacion_GetEstadoEjecByPeriodo(int documentID, DTO_QueryEstadoEjecucion filter, ProyectoTipo proType, TipoMoneda_LocExt tipoMda, TipoMoneda mdaOrigen)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.plCierreLegalizacion_GetEstadoEjecByPeriodo(this._userChannel, documentID, filter, proType, tipoMda, mdaOrigen);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene de acuerdo a un filtro la info de plSobreEjecucion
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="estado">Estado de los documentos</param>
        /// <param name="areaAprob">Area de aprobacion</param>
        /// <returns>Lista de informe </returns>
        public List<DTO_plSobreEjecucion> plSobreEjecucion_GetOrdenCompraSobreEjec(int documentID, int estado, string areaAprob)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.plSobreEjecucion_GetOrdenCompraSobreEjec(this._userChannel, documentID, estado, areaAprob);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }
        #endregion

        #endregion

        #region Proyectos

        #region Solicitud Proyecto

        /// <summary>
        /// Obtiene el detalle del documento
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>Detalle Documento</returns>
        public DTO_pyPreProyectoDocu pyPreProyectoDocu_Get(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyPreProyectoDocu_Get(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Proceso que genera la solicitud
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="prefijo">prefijo</param>
        /// <param name="numeroDoc">numero doc</param>
        /// <param name="areaFuncional">area funcional</param>
        /// <param name="claseServicioID">clase servicio</param>
        /// <param name="clienteID">cliente</param>
        /// <param name="proyectoID">proyecto</param>
        /// <returns></returns>
        public DTO_TxResult SolicitudProyecto_Add(int documentoID, ref int numeroDoc, string claseServicioID, string areaFuncional, string prefijo, string proyectoID, string centroCto, string observaciones, DTO_SolicitudTrabajo transaccion)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.SolicitudProyecto_Add(this._userChannel, documentoID, ref numeroDoc, claseServicioID, areaFuncional, prefijo, proyectoID, centroCto, observaciones, transaccion);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Enviar aprobación la solicitud de trabajo
        /// </summary>
        /// <param name="documentID">documentoID</param>
        /// <param name="numeroDoc">numero documento</param>
        public DTO_SerializedObject SolicitudProyecto_AprobarProy(int documentID, DTO_SolicitudTrabajo transaccion, DateTime fechaInicio)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.SolicitudProyecto_AprobarProy(this._userChannel, documentID, transaccion, fechaInicio);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Proceso que guarda un detalle del Cliente
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="numeroDoc">numero doc</param>
        /// <param name="transaccion">info del proyecto</param>
        /// <returns></returns>
        public DTO_TxResult SolicitudProyecto_AddAPUCliente(int documentoID, int numeroDoc, DTO_SolicitudTrabajo transaccion, bool saveProyectoInd)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.SolicitudProyecto_AddAPUCliente(this._userChannel, documentoID, numeroDoc, transaccion, saveProyectoInd);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un registro de glDocumentoControl
        /// </summary>
        /// <param name="documentID">Identificador del Documento</param>
        /// <param name="idPrefijo">Identificador del prefijo</param>
        /// <param name="documentoNro">Numero de documento</param>
        /// <param name="actividadFlujoID">Identificador del Documento</param>
        /// <returns></returns>
        public DTO_glDocumentoControl pyServicioDocu_GetInternalDoc(int documentID, string idPrefijo, int documentoNro, string actividadFlujoID)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyServicioDocu_GetInternalDoc(this._userChannel, documentID, idPrefijo, documentoNro, actividadFlujoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae una lista de servicio Deta
        /// </summary>
        /// <param name="documentID">Identificador del Documento</param>
        /// <param name="TrabajoID">Identificador del prefijo</param>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="loadDetaInd">Indica si carga detalle de un documento</param>
        /// <returns>Lista de componentes</returns>
        public List<DTO_pyPreProyectoDeta> pyPreProyectoDeta_GetByTarea(int documentID, string tareaID, string claseServidioID, int? numeroDoc, bool loadDetaExist, decimal tasaCambio = 0)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyPreProyectoDeta_GetByParameter(this._userChannel, documentID, tareaID, claseServidioID, numeroDoc, loadDetaExist,tasaCambio);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Consulta las tareas existentes con filtro
        /// </summary>
        /// <param name="numeroDoc">Identificador documento</param>
        /// <param name="tarea">tarea a analizar</param>
        /// <returns>Lista de resultados</returns>
        public List<DTO_pyPreProyectoTarea> pyPreProyectoTarea_Get(int? numeroDoc, string tareaID, string claseServicioID)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyPreProyectoTarea_Get(this._userChannel, numeroDoc, tareaID, claseServicioID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene los datos  de la solicitud
        /// </summary>
        /// <param name="documentoID">documento</param>
        /// <param name="prefijoID">prefijo del doc</param>
        /// <param name="docNro">nro consecutivo</param>
        /// <param name="numeroDoc">identificador doc</param>
        /// <param name="claseServicioID">clase servicio</param>
        /// <param name="isPreProyecto">Indica si carga las tablas de solicitud</param>
        /// <param name="loadMvtos">Indica si carga las los mvtos del detalle</param>
        /// <param name="loadActas">Indica si carga las actas de trabajo</param>
        /// <param name="loadTrazabilidad">Indica si carga la consulta de trazabilidad del proyecto</param>
        /// <returns>Objeto con todos los datos</returns>
        public DTO_SolicitudTrabajo SolicitudProyecto_Load(int documentoID, string prefijoID, int? docNro, int? numeroDoc, string claseServicioID,
                                                           string proyectoID, bool isPreProyecto, bool loadMvtos, bool loadActas, bool loadAPUCliente, bool loadTrazabilidad = false)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.SolicitudProyecto_Load(this._userChannel, documentoID, prefijoID, docNro, numeroDoc, claseServicioID, proyectoID, isPreProyecto, loadMvtos, loadActas, loadAPUCliente, loadTrazabilidad);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }
        #endregion

        #region Orden de Servicio


        /// <summary>
        /// Obtiene el detalle del documento
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>Detalle Documento</returns>
        public DTO_pyProyectoDocu pyProyectoDocu_Get(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyProyectoDocu_Get(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        #endregion

        #region Planeacion Compras



        /// <summary>
        /// Aprueba los recursos para  crear una Solicitud de Orden de Compra
        /// </summary>
        /// <param name="documentID">documento actual</param>
        /// <param name="listMvtos">Lista de recursos a aprobar</param>
        /// <returns>Result</returns>
        public DTO_SerializedObject CompraRecursos_ApproveSolicitudOC(int documentID, List<DTO_pyProyectoMvto> listMvtos)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.CompraRecursos_ApproveSolicitudOC(this._userChannel, documentID, listMvtos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene los recursos pendientes para  crear una Solicitud de Orden de Compra
        /// </summary>
        /// <returns>Lista de</returns>
        public List<DTO_pyProyectoMvto> CompraRecursos_GetPendientesForApprove(DateTime fechaTope)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.CompraRecursos_GetPendientesForApprove(this._userChannel, fechaTope);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        #endregion


        #region prComprasModificaFechas
        /// <summary>
        /// Consulta las tareas existentes con filtro
        /// </summary>
        /// <param name="numeroDoc">Identificador documento</param>
        /// <param name="aprobadoDocInd">Indica si trae un documento aprobado o sin aprobar</param>
        /// <returns>Lista de dtos</returns>
        public List<DTO_prComprasModificaFechas> prComprasModificaFechas_GetByNumeroDoc(int numeroDoc,bool aprobadoDocInd)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.prComprasModificaFechas_GetByNumeroDoc(this._userChannel, numeroDoc, aprobadoDocInd);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        public DTO_TxResult prComprasModificaFechas_Add(DTO_prComprasModificaFechas datos)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.prComprasModificaFechas_Add(this._userChannel, datos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;   
            }
        }


        public DTO_TxResult prComprasModificaFechas_Upd(DTO_prComprasModificaFechas datos)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.prComprasModificaFechas_Upd(this._userChannel, datos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        public DTO_prComprasModificaFechas prComprasModificaFechas_Load(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.prComprasModificaFechas_Load(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }
        #endregion


        #region pyProyectoModificaFechas
        /// <summary>
        /// Consulta las tareas existentes con filtro
        /// </summary>
        /// <param name="numeroDoc">Identificador documento</param>
        /// <param name="Tarea">Identificador de tarea</param>
        /// <returns>Lista de dtos</returns>
        public List<DTO_pyProyectoModificaFechas> pyProyectoModificaFechas_GetByNumeroDoc(int numeroDoc,string tarea)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyProyectoModificaFechas_GetByNumeroDoc(this._userChannel, numeroDoc, tarea);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        public DTO_TxResult pyProyectoModificaFechas_Add(DTO_pyProyectoModificaFechas datos)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyProyectoModificaFechas_Add(this._userChannel, datos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }


        public DTO_TxResult pyProyectoModificaFechas_Upd(DTO_pyProyectoModificaFechas datos)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyProyectoModificaFechas_Upd(this._userChannel, datos);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        public DTO_pyProyectoModificaFechas pyProyectoModificaFechas_Load(int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyProyectoModificaFechas_Load(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }
        #endregion

        #region pyProyectoTarea
        /// <summary>
        /// Consulta las tareas existentes con filtro
        /// </summary>
        /// <param name="numeroDoc">Identificador documento</param>
        /// <param name="tarea">tarea a analizar</param>
        /// <param name="claseServicio">clase servicio</param>
        /// <param name="recursoTimeInd">valida si es recurso de tiempo</param>
        /// <returns>Lista de resultados</returns>
        public List<DTO_pyProyectoTarea> pyProyectoTarea_Get(int? numeroDoc, string tareaID, string claseServicioID, bool recursoTimeInd = false)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyProyectoTarea_Get(this._userChannel, numeroDoc, tareaID, claseServicioID, recursoTimeInd);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        #endregion

        #region pyProyectoDeta

        /// <summary>
        /// Trae una lista de proyecto Deta
        /// </summary>
        /// <param name="documentID">Identificador del Documento</param>
        /// <param name="tareaID">Identificador del prefijo</param>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="loadDetaExist">Indica si carga detalle de un documento</param>
        ///  <param name="recursoTimeInd">Indica si carga si carga los recursos de solo tiempo</param>
        /// <returns>Lista de detalle</returns>
        public List<DTO_pyProyectoDeta> pyProyectoDeta_GetByParameter(int documentID, string tareaID, string claseServicioID, int? numeroDoc, bool loadDetaExist, bool recursoTimeInd, decimal tasaCambio = 0)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyProyectoDeta_GetByParameter(this._userChannel, documentID, tareaID, claseServicioID, numeroDoc, loadDetaExist,recursoTimeInd, tasaCambio);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }


        #endregion

        #region pyProyectoMvto
        /// <summary>
        /// Actualiza la tabla de mvtos
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>Detalle Documento</returns>
        public DTO_SerializedObject pyProyectoMvto_Upd(int documentID, List<DTO_pyProyectoMvto> listMvtos, bool SendToSolicitudInd)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyProyectoMvto_Upd(this._userChannel, documentID, listMvtos, SendToSolicitudInd);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el consolidado de documentos relacionados al proyecto
        /// </summary>
        /// <param name="consProyMvto">identificador del mvto de proyecto</param>
        /// <returns>Documentos</returns>
        public List<DTO_glDocumentoControl> pyProyectoMvto_GetDocsAnexo(int? consProyMvto)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyProyectoMvto_GetDocsAnexo(this._userChannel, consProyMvto);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        ///<summary>
        /// Trae los mvtos con el filtro
        /// </summary>
        /// <param name="filter">filtro adecuado</param>
        /// <returns>Detalle mvtos</returns>
        public List<DTO_pyProyectoMvto> pyProyectoMvto_GetParameter(DTO_pyProyectoMvto filter)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyProyectoMvto_GetParameter(this._userChannel, filter);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }


        ///<summary>
        /// Trae un mvto de proyecto por consecutivo
        /// </summary>
        /// <param name="consecMvto">filtro consec</param>
        /// <returns>mvto</returns>
        public DTO_pyProyectoMvto pyProyectoMvto_GetByConsecutivo(int? consecMvto)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyProyectoMvto_GetByConsecutivo(this._userChannel, consecMvto);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }
        #endregion

        #region Acta de Trabajo

        /// <summary>
        /// Aprueba el Acta de Trabajo y genera un recibido de Proveedores
        /// </summary>
        /// <param name="documentID">documento actual</param>
        /// <param name="proyecto">Lista de actas a aprobar</param>
        /// <returns>Result</returns>
        public DTO_SerializedObject ActaTrabajo_ApproveRecibidoBS(int documentID, DTO_SolicitudTrabajo proyecto, DTO_glDocumentoControl ctrlActa)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.ActaTrabajo_ApproveRecibidoBS(this._userChannel, documentID, proyecto,ctrlActa);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega un docum de Acta de Trabajo
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="docCtrl">doc control</param>
        /// <param name="actasList">lista de actas</param>
        /// <returns></returns>
        public DTO_SerializedObject ActaTrabajo_Add(int documentoID, DTO_glDocumentoControl docCtrl, List<DTO_pyActaTrabajoDeta> actasList, DTO_coProyecto proy, bool update)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.ActaTrabajo_Add(this._userChannel, documentoID, docCtrl, actasList, proy,update);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene las actas de trabajo
        /// </summary>
        /// <param name="numeroDoc">identificador doc</param>
        /// <returns>Objeto con todos los datos</returns>
        public List<DTO_pyActaTrabajoDeta> ActasTrabajo_Load(int? numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.ActasTrabajo_Load(this._userChannel, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }
        #endregion

        #region Otras

        /// <summary>
        /// Trae las tareas(pyTarea) con un filtro
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns>Lista</returns>
        public List<DTO_TareasFilter> TareasFilter_Get(DTO_TareasFilter filter)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.TareasFilter_Get(this._userChannel, filter);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        ///  Obtiene el detalle de los proyectos
        /// </summary>
        /// <param name="fechaCorte">FEcha corte</param>
        /// <param name="loadTareas">Carga las tareas y recursos del proyecto</param>
        /// <param name="loadCompras">carga las compras del proyecto</param>
        /// <param name="loadFinanciero">carga las transacciones financieras</param>
        /// <returns>resultado</returns>
        public List<DTO_QueryComiteTecnico> pyProyectoDocu_GetAllProyectos(DateTime fechaCorte, bool loadTareas, bool loadCompras, bool loadFinanciero)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyProyectoDocu_GetAllProyectos(this._userChannel, fechaCorte,loadTareas,loadCompras,loadFinanciero);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        #endregion

        #region Entregas Cliente

        /// <summary>
        ///  Guarda las tareas Cliente para realizar entregas posteriores
        /// </summary>
        /// <param name="documentoID">doc actual</param>
        /// <param name="numDocProy">identificador del proy</param>
        /// <param name="tareasCliente">lista de tareas</param>
        /// <param name="tareas">tareas del proyecto</param>
        /// <param name="tareasAdic">tareas adicionales del proyecto</param>
        /// <returns></returns>
        public DTO_TxResult EntregasCliente_Add(int documentoID, int numDocProy, List<DTO_pyProyectoTareaCliente> tareasCliente, List<DTO_pyProyectoTarea> tareas, List<DTO_pyProyectoTarea> tareasAdic,List<int> entregasDelete)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.EntregasCliente_Add(this._userChannel, documentoID, numDocProy, tareasCliente, tareas, tareasAdic,entregasDelete);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae las tareas Cliente asignadas a un proyecto
        /// </summary>
        /// <param name="numDocProy">identificador del proyecto</param>
        /// <param name="tareaCliente">tarea Cliente</param>
        /// <param name="desc">descripcion de la tarea</param>
        /// <returns></returns>
        public List<DTO_pyProyectoTareaCliente> pyProyectoTareaCliente_GetByNumeroDoc(int numDocProy, string tareaCliente, string desc)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyProyectoTareaCliente_GetByNumeroDoc(this._userChannel, numDocProy, tareaCliente, desc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega un docum de Acta de Trabajo
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="docCtrl">doc control</param>
        /// <param name="listEntregables">lista de actas</param>
        /// <returns>resultado</returns>
        public DTO_SerializedObject ActaEntrega_Add(int documentoID, DTO_glDocumentoControl docCtrl, List<DTO_pyProyectoTareaCliente> listEntregables, DTO_coProyecto proy, bool update)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.ActaEntrega_Add(this._userChannel, documentoID, docCtrl, listEntregables, proy, update);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Aprueba un docum de Acta de Entrega
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="docCtrl">doc control</param>
        /// <param name="proy">proyecto actual</param>
        /// <returns>resultado</returns>
        public DTO_SerializedObject ActaEntrega_Aprobar(int documentoID, DTO_glDocumentoControl docCtrl, DTO_coProyecto proy)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.ActaEntrega_Aprobar(this._userChannel, documentoID, docCtrl, proy);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene las actas de un proyecto con un filtro
        /// </summary>
        /// <param name="filter">filtros</param>
        /// <returns>lista</returns>
        public List<DTO_pyActaEntregaDeta> pyActaEntregaDeta_GetByParameter(DTO_pyActaEntregaDeta filter)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.pyActaEntregaDeta_GetByParameter(this._userChannel, filter);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Acta de entrega
        /// </summary>
        /// <param name="documentID">Doc</param>
        /// <param name="ctrlProy">ctrl</param>
        /// <param name="docuProy">header</param>
        /// <param name="listActaDeta">lista</param>
        /// <returns>resultado</returns>
        public DTO_SerializedObject ActaEntrega_ApprovePreFactura(int documentID, DTO_glDocumentoControl ctrlProy, DTO_pyProyectoDocu docuProy, List<DTO_pyProyectoTareaCliente> listActaDeta, List<DTO_faFacturacionFooter> listDetalleFact)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.ActaEntrega_ApprovePreFactura(this._userChannel, documentID, ctrlProy, docuProy, listActaDeta, listDetalleFact);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

         /// <summary>
        ///  Guarda las tareas Cliente para realizar entregas posteriores
        /// </summary>
        /// <param name="documentoID">doc actual</param>
        /// <param name="numDocProy">identificador del proy</param>
        /// <param name="entregables">lista de entregables</param>
        /// <param name="deleteEntregable">Valida si borra entregables</param>
        /// <param name="deleteProgramacion">Valida si borra programacion</param>
        /// <param name="deleteActas">Valida si borra actas</param>
        /// <param name="anulaPreFacturas">Valida si anula prefacturas</param>
        /// <returns>Resultados</returns>
        public DTO_TxResult EntregasCliente_Delete(int documentoID, int numDocProy, List<DTO_pyProyectoTareaCliente> entregables, bool deleteEntregable, bool deleteProgramacion, bool deleteActas, bool anulaPreFacturas)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.EntregasCliente_Delete(this._userChannel, documentoID, numDocProy,entregables,deleteEntregable,deleteProgramacion,deleteActas,anulaPreFacturas);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        #endregion

        #region Migrar Insumos

        /// <summary>
        /// Agrega o actualiza una lista de insumos o proveedores
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="listInsumos">Lista de insumos</param>
        /// <param name="listProveedores">Lista de proveedores</param>
        /// <param name="listAnalisis">Lista de analisis</param>
        /// <param name="listAPU">Lista de Analisis Precios Unitarios</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult MigracionInsumos(int documentID, List<DTO_MigracionInsumos> listInsumos, List<DTO_MigracionProveedor> listProveedores, List<DTO_MigracionGrupos> listAnalisis, List<DTO_MigracionAPU> lisAPU)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.MigracionInsumos(this._userChannel, documentID, listInsumos, listProveedores, listAnalisis, lisAPU);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega o actualiza una lista de locaciones y entregas
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="listLocaciones">Lista de Locaciones</param>
        /// <param name="listLocTareas">Lista de tareas</param>
        /// <param name="listLocRecursos">Lista de recursos</param>
        /// <param name="listEntregas">Lista de entregas</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult MigracionLocacionEntregas(int documentID, int numDocProy, List<DTO_pyProyectoLocacion> listLocaciones, List<DTO_pyProyectoIngDetalleTarea> listLocTareas, List<DTO_pyProyectoIngDetalleDeta> listLocRecursos, List<DTO_pyProyectoEntregasxMes> listEntregas)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.MigracionLocacionEntregas(this._userChannel, documentID, numDocProy, listLocaciones, listLocTareas, listLocRecursos, listEntregas);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        /// <summary>
        /// Trea la lista de locaciones y entregas de un proyecto
        /// </summary>
        /// <param name="numDocProy">Documnto que inicia la tx</param>
        /// <param name="listLocaciones">Lista de Locaciones</param>
        /// <param name="listLocTareas">Lista de tareas</param>
        /// <param name="listLocRecursos">Lista de recursos</param>
        /// <param name="listEntregas">Lista de entregas</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult LocacionEntregasGetByProyecto(int documentID, int numDocProy, ref List<DTO_pyProyectoLocacion> listLocaciones, ref List<DTO_pyProyectoIngDetalleTarea> listLocTareas, ref List<DTO_pyProyectoIngDetalleDeta> listLocRecursos, ref List<DTO_pyProyectoEntregasxMes> listEntregas)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.LocacionEntregasGetByProyecto(this._userChannel, documentID, numDocProy, ref listLocaciones, ref listLocTareas, ref listLocRecursos, ref listEntregas);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        #endregion

        #region Reversion Documentos

        /// <summary>
        /// Revierte una Documento de Proyectos
        /// </summary>
        /// <param name="documentID">identificador documento</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Proyectos_Revertir(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IOpConjuntasService));
                var result = this.OpConjuntasService.Proyectos_Revertir(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IOpConjuntasService));
                throw ex;
            }
        }

        #endregion
        #endregion

        #endregion

        #region ProveedoresService

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción de un usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ConsultarProgresoProveedores(int documentID)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.ConsultarProgreso(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        #endregion

        #region DetalleDocu
        /// <summary>
        /// Trae un detalle de proveedores 
        /// </summary>
        /// <param name="detalleID">Identificador del detalle</param>
        /// <returns>Retorna el detalle</returns>
        public DTO_prDetalleDocu prDetalleDocu_GetByID( int detalleID)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.prDetalleDocu_GetByID(this._userChannel, detalleID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae la lista de prDetalleDocu segun el numero del documento
        /// </summary>
        /// <param name="NumeroDoc">Numero del documento</param>
        /// <param name="isFactura">Valida si se filtra por identificador de FacturaDocuID</param>
        /// <returns>Lista de detalle</returns>
        public List<DTO_prDetalleDocu> prDetalleDocu_GetByNumeroDoc(int NumeroDoc, bool isFactura)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.prDetalleDocu_GetByNumeroDoc(this._userChannel, NumeroDoc, isFactura);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="document">Documento a filtrar</param>
        /// <param name="numeroDoc">identificador del documento a filtrar</param>
        /// <param name="consecutivoDeta">identificador del detalle si se requiere</param>
        /// <returns></returns>
        public List<DTO_prDetalleDocu> prDetalleDocu_GetByDocument(int document, int numeroDoc, int consecutivoDeta = 0)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.prDetalleDocu_GetByDocument(this._userChannel, document, numeroDoc, consecutivoDeta);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter">Filtro de la tabla</param>
        /// <returns>Lista Dto de Detalle Docu</returns>
        public List<DTO_prDetalleDocu> prDetalleDocu_GetParameter(DTO_prDetalleDocu filter)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.prDetalleDocu_GetParameter(this._userChannel, filter);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene los items para cerrar de un documento
        /// </summary>
        /// <param name="documentFilter">Documento  a cerrar</param>
        /// <param name="prefijoID">Prefijo del doc</param>
        /// <param name="docNro">nro  del Doc</param>
        /// <param name="proveedorID">Proveedor</param>
        /// <param name="referenciaID">Referencia</param>
        /// <param name="codigoBS">Codigo BS</param>
        /// <returns>Lista de detalle</returns>
        public List<DTO_prDetalleDocu> prDetalleDocu_GetPendienteForCierre(int documentFilter, string prefijoID, int? docNro, string proveedorID, string referenciaID, string codigoBS)
        {

            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.prDetalleDocu_GetPendienteForCierre(this._userChannel, documentFilter, prefijoID, docNro, proveedorID, referenciaID, codigoBS);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        #endregion

        #region Cierre Detalle
        /// <summary>
        /// Guardar el documento
        /// </summary>
        /// <param name="documentID">ID del documento</param>
        /// <param name="ctrl">referencia a glDocumentoControl</param>
        /// <param name="footer">la lista de detalle</param>
        /// <returns>si la operacion es exitosa</returns>
        public DTO_SerializedObject CierreDetalle_Guardar(int documentID, DTO_glDocumentoControl ctrl, List<DTO_prDetalleDocu> footer)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.CierreDetalle_Guardar(this._userChannel, documentID, ctrl, footer);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        #endregion

        #region Solicitud

        /// <summary>
        /// Carga la informacion completa de la solicitud
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="solNro">Numero de la solicitud</param>
        /// <returns>Retorna datos de Solicitud</returns>
        public DTO_prSolicitud Solicitud_Load(int documentID, string prefijoID, int solNro)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Solicitud_Load(this._userChannel, documentID, prefijoID, solNro);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Guardar nueva Solicitud y asociar en glDocumentoControl
        /// </summary>
        /// <param name="ctrl">glDocumentoControl</param>
        /// <param name="header">SolicitudDocu</param>
        /// <param name="footer">SolicitudFooter</param>
        /// <returns>NumeroDoc</returns>
        public DTO_SerializedObject Solicitud_Guardar(int documentID, DTO_glDocumentoControl ctrl, DTO_prSolicitudDocu header, DTO_prSolicitudDirectaDocu headerDirecta, List<DTO_prSolicitudFooter> footer, bool update, out int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Solicitud_Guardar(this._userChannel, documentID, ctrl, header, headerDirecta, footer, update, out numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Manda el documento a aprobacion
        /// </summary>
        /// <returns></returns>
        public DTO_SerializedObject Solicitud_SendToAprob(int documentID, int numeroDoc, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Solicitud_SendToAprob(this._userChannel, documentID, numeroDoc, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de solicitudes pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prSolicitudAprobacion> Solicitud_GetPendientesByModulo(ModulesPrefix mod, int documentID, string actividadFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Solicitud_GetPendientesByModulo(this._userChannel, mod, documentID, actividadFlujoID, usuario);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de solicitudes pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prSolicitudAsignacion> Solicitud_GetPendientesForAssign(ModulesPrefix mod, int documentID, string actividadFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Solicitud_GetPendientesForAssign(this._userChannel, mod, documentID, actividadFlujoID, usuario);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }


        /// <summary>
        /// Recibe una lista de solicitudes para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="sol">solicitud que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Solicitud_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_prSolicitudAprobacion> sol, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Solicitud_AprobarRechazar(this._userChannel, documentID, actividadFlujoID, sol, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Recibe una lista de solicitudes para asignar
        /// </summary>
        /// <param name="documentID">documento que relaciona la asignacion</param>
        /// <param name="sol">solicitud que se deben asignar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Solicitud_Asignar(int documentID, string actividadFlujoID, List<DTO_prSolicitudAsignacion> sol)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Solicitud_Asignar(this._userChannel, documentID, actividadFlujoID, sol);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de solicitudes para orden de compra
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prSolicitudResumen> Solicitud_GetResumen(int documentID, DTO_seUsuario usuario, ModulesPrefix mod, TipoMoneda origenMonet)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Solicitud_GetResumen(this._userChannel, documentID, usuario, mod,origenMonet);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de las solicitudes direcctas pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Lista de pendientes</returns>
        public List<DTO_prSolicitudDirectaAprob> SolicitudDirecta_GetPendientesByModulo(int documentoID, string actFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.SolicitudDirecta_GetPendientesByModulo(this._userChannel, documentoID, actFlujoID, usuario);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }
        /// <summary>
        /// Trae la lista de prSolicitudCargos segun el identificador del detalle
        /// </summary>
        /// <param name="NumeroDoc">Numero del documento</param>
        /// <returns></returns>
        public List<DTO_prSolicitudCargos> prSolicitudCargos_GetByConsecutivoDetaID(int documentID, int ConsecutivoDetaID)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.prSolicitudCargos_GetByConsecutivoDetaID(this._userChannel, documentID, ConsecutivoDetaID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        // <summary>
        /// Recibe una lista de solicitudes directas para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="actividadFlujoID">actividad del documento</param>
        /// <param name="sol">solicitud que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> SolicitudDirecta_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_prSolicitudDirectaAprob> sol, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.SolicitudDirecta_AprobarRechazar(this._userChannel, documentID, actividadFlujoID, sol, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }
        #endregion

        #region Orden de compra

        /// <summary>
        /// Carga la informacion completa del Orden de compra
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="solNro">Consecutivo del Orden de Compra</param>
        /// <param name="numeroDoc">numero Doc de la OC</param>
        /// <returns>Retorna datos del Orden de compra</returns>
        public DTO_prOrdenCompra OrdenCompra_Load(int documentID, string prefijoID, int ordenNro, int numeroDoc = 0)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.OrdenCompra_Load(this._userChannel, documentID, prefijoID, ordenNro, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Guardar nuevo Orden de Compra y asociar en glDocumentoControl
        /// </summary>
        /// <param name="orden">Data completa</param>
        /// <returns></returns>
        public DTO_SerializedObject OrdenCompra_Guardar(int documentID, DTO_prOrdenCompra orden, bool update, out int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.OrdenCompra_Guardar(this._userChannel, documentID, orden, update, out numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de ordenes de compra pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>la lista de los ordenes de compra pendientes </returns>
        public List<DTO_prOrdenCompraAprob> OrdenCompra_GetPendientesByModulo(int documentID, string actFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.OrdenCompra_GetPendientesByModulo(this._userChannel, documentID, actFlujoID, usuario);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Recibe una lista de Ordenes de Compra para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="usuario">usuario que aprueba el orden de compra</param>
        /// <param name="ord">ordenes de compra que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> OrdenCompra_AprobarRechazar(int documentID, string actividadFlujoID, DTO_seUsuario usuario, List<DTO_prOrdenCompraAprob> ord, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.OrdenCompra_AprobarRechazar(this._userChannel, documentID, actividadFlujoID, usuario, ord, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de ordenes de compra para recibido
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prOrdenCompraResumen> OrdenCompra_GetResumen(int documentID, DTO_seUsuario usuario, ModulesPrefix mod, List<Tuple<string, string>> filtros)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.OrdenCompra_GetResumen(this._userChannel, documentID, usuario, mod, filtros);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }
        #endregion

        #region prContratoPlanPagos

        /// <summary>
        /// Actualiza la lista de prContratoPlanPago en base de datos
        /// </summary>
        /// <param name="listContPlanPago">la lista de DTO_prContratoPlanPago</param>
        /// <returns></returns>
        public DTO_TxResult prContratoPlanPago_Upd(List<DTO_prContratoPlanPago> listContPlanPago, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.prContratoPlanPago_Upd(this._userChannel, listContPlanPago, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }
        #endregion

        #region Contrato

        /// <summary>
        /// Trae un listado de los contratos pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Lista de pendientes para aprobar</returns>
        public List<DTO_prContratoAprob> Contrato_GetPendientesByModulo(int documentID, string actFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Contrato_GetPendientesByModulo(this._userChannel, documentID, actFlujoID, usuario);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Recibe una lista de contratos para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="contrato">ordenes de compra que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Contrato_AprobarRechazar(int documentID, string actividadFlujoID, DTO_seUsuario usuario, List<DTO_prContratoAprob> contrato, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Contrato_AprobarRechazar(this._userChannel, documentID, actividadFlujoID, usuario, contrato, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        #endregion

        #region Recibido

        /// <summary>
        /// Guardar nuevo Recibido y asociar en glDocumentoControl
        /// </summary>
        /// <param name="ctrl">Referencia documento</param>
        /// <param name="header">RecibidoDocu</param>
        /// <param name="footer">RecibidoFooter</param>
        /// <param name="numDoc">Numero doc del documento guardado</param>
        /// <param name="docTransporte">documento de transporte para inventarios</param>
        /// <param name="manifCarga">maminifiesto de carga para inventarios</param>
        /// <returns>Resultado</returns>
        public DTO_SerializedObject Recibido_Guardar(int documentID, DTO_glDocumentoControl ctrl, DTO_prRecibidoDocu header, List<DTO_prOrdenCompraResumen> footer, out int numeroDoc, string docTransporte, string manifCarga)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Recibido_Guardar(this._userChannel, documentID, ctrl, header, footer, out numeroDoc, docTransporte, manifCarga);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Carga la informacion completa del documento
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="solNro">Numero de Documento interno</param>
        /// <returns>Retorna el Recibido</returns>
        public DTO_prRecibido Recibido_Load(int documentID, string prefijoID, int recNro, int numeroDoc = 0)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Recibido_Load(this._userChannel, documentID, prefijoID, recNro, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de recibidos pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>la lista de los recibidos pendientes </returns>
        public List<DTO_prRecibidoAprob> Recibido_GetPendientesByModulo(int documentID, string actFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Recibido_GetPendientesByModulo(this._userChannel, documentID, actFlujoID, usuario);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de los Recibidos no facturados ya aprobados
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="actFlujoID">Actividad Flujo</param>
        /// <param name="usuario">usuario actual</param>
        /// <param name="NroDocfacturaExist">Numero Doc del documento de la factura si existe</param>
        /// <param name="loadIVA">Indica si carga el porc del iva o no</param>, bool 
        /// <returns>Lista de Recibidos Aprobados</returns>
        public List<DTO_prRecibidoAprob> Recibido_GetRecibidoNoFacturado(int documentID, string actFlujoID, string proveedor, bool loadIVA, int NroDocfacturaExist = 0)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Recibido_GetRecibidoNoFacturado(this._userChannel, documentID, actFlujoID,proveedor, loadIVA, NroDocfacturaExist);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Recibe una lista de Recibido para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="usuario">usuario que aprueba el orden de compra</param>
        /// <param name="ord">ordenes de compra que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Recibido_AprobarRechazar(int documentID, string actividadFlujoID, DTO_seUsuario usuario, List<DTO_prRecibidoAprob> rec, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Recibido_AprobarRechazar(this._userChannel, documentID, actividadFlujoID, usuario, rec, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Envia para aprobacion un recibido
        /// </summary>
        /// <param name="numeroDoc">Codigo del comprobante</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject Recibido_SendToAprob(Guid channel, int documentID, int numeroDoc, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Recibido_SendToAprob(this._userChannel, documentID, numeroDoc, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Radicar Factura o Devuelve en tabla cpCuentaXPagar y asociar en glDocumentoControl
        /// </summary>
        /// <param name="_dtoCtrl">referencia documento</param>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        public DTO_SerializedObject Recibido_RadicarDevolver(int documentID, List<DTO_prRecibidoAprob> recibidosNoFactSelect, DTO_glDocumentoControl _dtoCtrl, DTO_cpCuentaXPagar cta, bool update, out int numeroDoc, bool devolverInd)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Recibido_RadicarDevolver(this._userChannel, documentID, recibidosNoFactSelect, _dtoCtrl, cta, update, out numeroDoc, devolverInd);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el detalle de cargos de los Recibidos aprobados pendientes
        /// </summary>
        /// <returns>lista de resultados</returns>
        public List<DTO_SerializedObject> Recibido_GenerarDetalleCargosRecib(int documentID)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Recibido_GenerarDetalleCargosRecib(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        #endregion

        #region Convenios

        /// <summary>
        /// Guardar el documento
        /// </summary>
        /// <param name="documentID">ID del documento</param>
        /// <param name="data">data a guardar</param>
        /// <param name="numeroDoc">identificador interior del documento</param>
        /// <returns>si la operacion es exitosa</returns>
        public DTO_SerializedObject Convenio_Add(int documentID, DTO_Convenios data, out int numeroDoc, bool update)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Convenio_Add(this._userChannel, documentID, data, out numeroDoc, update);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de los Convenios pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_ConvenioAprob> Convenio_GetPendientesByModulo(int documentID, string actFlujoID, DTO_seUsuario usuario, bool SolicitudInd)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Convenio_GetPendientesByModulo(this._userChannel, documentID, actFlujoID, usuario, SolicitudInd);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Carga la informacion completa del documento
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="NroContrato">Numero de Contrato para obtener la solicitud de convenios</param>
        /// <returns>Retorna el Recibido</returns>
        public DTO_Convenios Convenio_GetByNroContrato(int documentID, string prefijoID, int NroContrato)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Convenio_GetByNroContrato(this._userChannel, documentID, prefijoID, NroContrato);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Carga la informacion completa del documento
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="NroConsecutivo">Numero de Documento consecutivo</param>
        /// <returns>Retorna el Convenio</returns>
        public DTO_Convenios Convenio_Get(int documentID, string prefijoID, int NroConsecutivo, bool ConsumoProyectoInd)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Convenio_Get(this._userChannel, documentID, prefijoID, NroConsecutivo, ConsumoProyectoInd);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Recibe una lista de Convenios para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="ord">ordenes de compra que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Convenio_AprobarRechazar(int documentID, string actividadFlujoID, DTO_seUsuario usuario, List<DTO_ConvenioAprob> convAp, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Convenio_AprobarRechazar(this._userChannel, documentID, actividadFlujoID, usuario, convAp, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        ///   Envia para aprobacion un convenio
        /// </summary>
        /// <param name="documentID">Documento de aprobacion</param>
        /// <param name="actividadFlujoID">actividad actual</param>
        /// <param name="numeroDoc">identificador del documento a aprobar</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject Convenio_SendToAprob(int documentID, string actividadFlujoID, int numeroDoc, bool createDoc)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Convenio_SendToAprob(this._userChannel, documentID, actividadFlujoID, numeroDoc, createDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        #region Consumo Proyecto
        /// <summary>
        /// Trae un listado de los Consumos de Proyecto para recibido
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_ConveniosResumen> ConsumoProyecto_GetResumen(int documentID, DTO_seUsuario usuario, ModulesPrefix mod, DateTime fechaCorte, string proveedorID)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.ConsumoProyecto_GetResumen(this._userChannel, documentID, usuario, mod, fechaCorte, proveedorID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Agrega un recibido de consumo y Recibe los consumos de proyecto en los saldos 
        /// </summary>
        /// <param name="documentID">Documento actual</param>
        /// <param name="recibCons">Lista de items a recibir</param>
        /// <param name="proveedorID">proveedor correspondiente</param>
        /// <param name="fechaDoc">Fecha del documento a generar</param>
        /// <returns>Respuesta</returns>
        public DTO_SerializedObject RecibidoConvenios_Add(int documentID, List<DTO_ConveniosResumen> recibCons, string proveedorID, DateTime fechaDoc)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.RecibidoConvenios_Add(this._userChannel, documentID, recibCons, proveedorID, fechaDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae un listado de Solicitudes de Despacho Convenios para recibido
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Lista de Solicitudes</returns>
        public List<DTO_ConveniosResumen> SolicitudDespachoConvenio_GetResumen(int documentID, DTO_seUsuario usuario, ModulesPrefix mod, string proveedor)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.SolicitudDespachoConvenio_GetResumen(this._userChannel, documentID, usuario, mod, proveedor);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        #endregion
        #endregion

        #region Reversion Documentos

        /// <summary>
        /// Revierte una Documento de PRoveedores
        /// </summary>
        /// <param name="documentID">identificador documento</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Proveedores_Reversion(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Proveedores_Reversion(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Anula Proveedores
        /// </summary>
        /// <param name="numDoc">nums de las Proveedores a anular</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult Proveedoress_Anular(int documentID, List<int> numDocs)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Proveedoress_Anular(this._userChannel, documentID, numDocs);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }
        #endregion

        #region Provision
        /// <summary>
        /// Guarda documento de Provision con un comprobante de los Recibidos Pendientes por facturar
        /// </summary>
        /// <param name="documentID">documento de provision</param>
        /// <returns>Lista de Resultados</returns>
        public DTO_TxResult Provision_RecibidoNotFacturadoAdd(int documentID)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.Provision_RecibidoNotFacturadoAdd(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }
        #endregion

        #region Consultas

        /// <summary>
        /// Trae un listado de las documentos con detalle como consulta
        /// </summary>
        /// <param name="documentID">documento relacionado</param>
        /// <param name="docs">Lista de documentos a consultar</param>
        /// <returns>Detalle de la consulta</returns>
        public List<DTO_ConsultaCompras> ConsultaCompras_Get(int documentID, List<DTO_glDocumentoControl> ctrls)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.ConsultaCompras_Get(this._userChannel, documentID, ctrls);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene una lista de cierres
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <param name="codigoBSID">codigo Bien servicio</param>
        /// <param name="referenciaID">referencia</param>
        /// <param name="numeroDocOC">numero Doc Orden Compra</param>
        /// <returns>lista de cierres</returns>
        public List<DTO_prCierreMesCostos> prCierreMesCostos_GetByParameter(DateTime periodo, string codigoBSID, string referenciaID, int? numeroDocOC)
        {
            try
            {
                this.CreateService(typeof(IProveedoresService));
                var result = this.ProveedoresService.prCierreMesCostos_GetByParameter(this._userChannel, periodo, codigoBSID, referenciaID, numeroDocOC);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IProveedoresService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region ReportesService

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción de un usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ConsultarProgresoReportes(int documentID)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ConsultarProgreso(this._userChannel, documentID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Activos

        /// <summary>
        /// Funcion que carga el DTO de Acitivos Saldos para mostrar el valor por componente
        /// </summary>
        /// <param name="channel">Canal de trasmicion de datos</param>
        /// <param name="libro">Libro que se desea Mostrar</param>
        /// <param name="Periodo">Periodo que se desea mostrar</param>
        /// <param name="plaqueta">Filtra una plaqueta especifica para mostrar</param>
        /// <param name="serial"> Filtra un serial especifico para mostrar</param>
        /// <param name="referencia"> Filtar una referencia especifica para mostrar</param>
        /// <param name="clase">Filtra una clase especifica para mostrar</param>
        /// <param name="tipo">Filtra un tipo especifico a mostrar </param>
        /// <param name="grupos">Filtra un grupo especifico para mostrar</param>
        /// <param name="propietario">Filtra un propietario especifico para mostrar</param>
        ///<param name="formatType">Tipo de Formato a exportar el reporte</param>
        /// <param name="ConceptoSaldo">Lista de los conceptos saldo de Activos Fijos</param>
        /// <returns>DTO_Activos</returns>
        public DTO_TxResult ReportesActivos_Saldos(string libro, DateTime Periodo, string plaqueta, string serial, string referencia, string clase, string tipo,
            string grupos, string propietario, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesActivos_Saldos(this._userChannel, libro, Periodo, plaqueta, serial, referencia, clase, tipo, grupos, propietario, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que carga el DTO de Acitivos Saldos para mostrar el valor por componente
        /// </summary>
        /// <param name="libro">Libro que se desea Mostrar</param>
        /// <param name="Periodo">Periodo que se desea mostrar</param>
        /// <param name="plaqueta">Filtra una plaqueta especifica para mostrar</param>
        /// <param name="serial"> Filtra un serial especifico para mostrar</param>
        /// <param name="referencia"> Filtar una referencia especifica para mostrar</param>
        /// <param name="clase">Filtra una clase especifica para mostrar</param>
        /// <param name="tipo">Filtra un tipo especifico a mostrar </param>
        /// <param name="grupos">Filtra un grupo especifico para mostrar</param>
        /// <param name="propietario">Filtra un propietario especifico para mostrar</param>
        ///<param name="formatType">Tipo de Formato a exportar el reporte</param>
        /// <param name="ConceptoSaldo">Lista de los conceptos saldo de Activos Fijos</param>
        /// <returns>DTO_Activos</returns>
        public DTO_TxResult ReportesActivos_SaldosML(string libro, DateTime Periodo, string plaqueta, string serial, string referencia, string clase, string tipo,
            string grupos, string propietario, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesActivos_SaldosML(this._userChannel, libro, Periodo, plaqueta, serial, referencia, clase, tipo, grupos, propietario, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que carga el DTO de Acitivos Saldos para mostrar el valor por componente
        /// </summary>
        /// <param name="libro">Libro que se desea Mostrar</param>
        /// <param name="Periodo">Periodo que se desea mostrar</param>
        /// <param name="plaqueta">Filtra una plaqueta especifica para mostrar</param>
        /// <param name="serial"> Filtra un serial especifico para mostrar</param>
        /// <param name="referencia"> Filtar una referencia especifica para mostrar</param>
        /// <param name="clase">Filtra una clase especifica para mostrar</param>
        /// <param name="tipo">Filtra un tipo especifico a mostrar </param>
        /// <param name="grupos">Filtra un grupo especifico para mostrar</param>
        /// <param name="propietario">Filtra un propietario especifico para mostrar</param>
        ///<param name="formatType">Tipo de Formato a exportar el reporte</param>
        /// <param name="ConceptoSaldo">Lista de los conceptos saldo de Activos Fijos</param>
        /// <returns>DTO_Activos</returns>
        public DTO_TxResult ReportesActivos_SaldosME(string libro, DateTime Periodo, string plaqueta, string serial, string referencia, string clase, string tipo,
            string grupos, string propietario, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesActivos_SaldosME(this._userChannel, libro, Periodo, plaqueta, serial, referencia, clase, tipo, grupos, propietario, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }


        public string ReportesActivos_ComparacionLibros(int año, int mes, string clase, string tipo, string grupo, string centroCost, string logFis,
            string proyecto, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesActivos_ComparacionLibros(this._userChannel, año, mes, clase, tipo, grupo, centroCost, logFis, proyecto, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        public DTO_TxResult ReportesActivos_EquiposArrendados(DateTime Periodo, int Estado, string Tercero, string Plaqueta, string Serial, string TipoRef, string Rompimiento, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesActivos_EquiposArrendados(this._userChannel, Periodo, Estado, Tercero, Plaqueta, Serial, TipoRef, Rompimiento, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        public DTO_TxResult ReportesActivos_ImportacionesTemporales(DateTime Periodo, string Plaqueta, string Serial, string TipoRef, string Rompimiento, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesActivos_ImpotacionesTemporales(this._userChannel, Periodo, Plaqueta, Serial, TipoRef, Rompimiento, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        #endregion

        #region Cartera

        /// <summary>
        /// Genera el archivos plano para la pagaduria que tiene el centro de pago
        /// </summary>
        /// <param name="pagaduria">Pagaduria que se desea generar</param>
        /// <returns>Archivo plano</returns>
        public DTO_TxResult Report_Cc_ArchivosPlanos(string pagaduria)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_ArchivosPlanos(this._userChannel, pagaduria);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mes">Mes de consulta</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="filter">Filtros</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cc_Aportes(DateTime mes, DateTime fechaIni, DateTime fechaFin, string filter, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_Aportes(this._userChannel, mes, fechaIni, fechaFin, filter, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="orderName">Ordenar por nombre?</param>
        /// <param name="filter">Filtro</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cc_Aseguradora(DateTime fechaIni, DateTime fechaFin, bool orderName, string filter, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_Aseguradora(this._userChannel, fechaIni, fechaFin, orderName, filter, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que devuele el nombre del reporte
        /// </summary>
        /// <param name="periodo">Periodo del reporte</param>
        /// <param name="clienteFiltro">Filtro del Cliente</param>
        /// <returns>Nombre del Reporte</returns>
        public string Report_Cc_AportesCliente(DateTime periodo, string clienteFiltro, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_AportesCliente(this._userChannel, periodo, clienteFiltro, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae el reporte de Aportes a Clientes(Revision Saldo)
        /// </summary>
        /// <param name="Año"></param>
        /// <param name="Mes"></param>
        /// <param name="_tercero"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public string Report_Cc_Aportes_a_Clientes(int Año, int Mes, string _tercero, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_Aportes_a_Clientes(this._userChannel, Año, Mes, _tercero, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que devuele el nombre del reporte
        /// </summary>
        /// <param name="clienteFiltro">Filtro del Cliente</param>
        /// <returns>Nombre del Reporte</returns>
        public string Report_Cc_EstadoDeCuenta(DateTime fechaIni, DateTime fechaFin, string _tercero, string clienteFiltro, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_EstadoDeCuenta(this._userChannel, fechaIni, fechaFin, _tercero, clienteFiltro, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Servicio que obtiene la infomacion del certificado de deuda por la libranza y la fecha de corte
        /// </summary>
        /// <param name="fechaCorte">Fecha capturada en el form</param>
        /// <param name="libranza">Libranza que va a consultar</param>
        /// <returns>certifcado de deuda</returns>
        public DTO_ccCertificadoDeuda Report_Cc_CertificadoDeuda(DateTime fechaCorte, int libranza)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_CertificadoDeuda(this._userChannel, fechaCorte, libranza);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numeroDoc"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public string Report_Cc_Oferta(int numeroDoc, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_Oferta(this._userChannel, numeroDoc, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene datatable para Excel
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="cliente">Cliente</param>
        /// <param name="libranza">Libranza</param>
        /// <param name="zonaID">Zona</param>
        /// <param name="ciudad">Ciudad</param>
        /// <param name="ConcesionarioID">Concecionario</param>
        /// <param name="asesor">Asesor</param>
        /// <param name="lineaCredi">LineaCredito</param>
        /// <param name="compCartera">CompradorCartera</param>
        /// <param name="pagaduria">Pagaduria</param>
        /// <param name="centroPago">CentroPago</param>
        /// <param name="agrup">Agrup</param>
        /// <param name="romp">Romp</param>
        /// <returns></returns>
        public DataTable Report_Cc_CarteraToExcel(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza,
                string zona, string ciudad, string concesionario, string asesor, string lineaCred, string compCart, string pagaduria, string centroPago, byte? agrup, byte? romp, object filter = null)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_CarteraToExcel(this._userChannel, documentoID, tipoReporte, fechaIni, fechaFin, cliente, libranza, zona, ciudad, concesionario, asesor, lineaCred, compCart, pagaduria, centroPago, agrup, romp,filter);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene datatable para Excel
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="cliente">Cliente</param>
        /// <param name="libranza">Libranza</param>
        /// <param name="zonaID">Zona</param>
        /// <param name="ciudad">Ciudad</param>
        /// <param name="ConcesionarioID">Concecionario</param>
        /// <param name="asesor">Asesor</param>
        /// <param name="lineaCredi">LineaCredito</param>
        /// <param name="compCartera">CompradorCartera</param>
        /// <param name="pagaduria">Pagaduria</param>
        /// <param name="centroPago">CentroPago</param>
        /// <param name="agrup">Agrup</param>
        /// <param name="romp">Romp</param>
        /// <returns></returns>
        public string Report_Cc_CarteraByParameter(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza,
                string zona, string ciudad, string concesionario, string asesor, string lineaCred, string compCart, string pagaduria, string centroPago, byte? agrup, byte? romp, object filter, int? numeroDoc = null)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_CarteraByParameter(this._userChannel, documentoID, tipoReporte, fechaIni, fechaFin, cliente, libranza, zona, ciudad, concesionario, asesor, lineaCred, compCart, pagaduria, centroPago, agrup, romp, filter, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Servicio q retorna el nombre del reporte
        /// </summary>
        /// <param name="documentReportID">documento del Reporte</param>
        /// <param name="numDoc">identificador del documento</param>
        /// <param name="isAprobada">Es aprobada</param>
        /// <param name="formatType">tipo de reporte</param>
        /// <returns></returns>
        public string Report_Cc_CarteraByNumeroDoc(int documentID, string _nameProposito, int numDoc, DateTime fechaCorte, DateTime? fechaFNC, byte diasFNC, bool isAprobada, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_CarteraByNumeroDoc(this._userChannel, documentID, _nameProposito, numDoc, fechaCorte, fechaFNC,diasFNC, isAprobada, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte de la relación de pagos de un recauso masivo
        /// </summary>
        /// <param name="data">Datos a migrar</param>
        /// <returns>Retorna el nombre del reporte</returns>
        public string Report_Cc_RecaudosMasivosGetRelacionPagos(int documentID, List<DTO_ccIncorporacionDeta> data)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_RecaudosMasivosGetRelacionPagos(this._userChannel, documentID, data);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte de la relación de pagos de un recauso masivo
        /// </summary>
        /// <param name="data">Datos a migrar</param>
        /// <returns>Retorna el nombre del reporte</returns>
        public string Report_Cc_CobroJuridico(int documentID, byte claseDeuda, byte tipoReporte, string cliente, string obligacion, byte tipoEstado)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_CobroJuridico(this._userChannel, documentID, claseDeuda, tipoReporte, cliente, obligacion,tipoEstado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte de cobro juridico historico
        /// </summary>
        /// <param name="numDocCredito">num Doc del credito</param>
        /// <returns>Retorna el nombre del reporte</returns>
        public string Report_Cc_CobroJuridicoHistoria(int documentID, int numDocCredito, byte claseDeuda)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_CobroJuridicoHistoria(this._userChannel, documentID,numDocCredito, claseDeuda);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene datatable para Excel
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="cliente">Cliente</param>
        /// <param name="libranza">Libranza</param>
        /// <param name="zonaID">Zona</param>
        /// <param name="ciudad">Ciudad</param>
        /// <param name="ConcesionarioID">Concecionario</param>
        /// <param name="asesor">Asesor</param>
        /// <param name="lineaCredi">LineaCredito</param>
        /// <param name="compCartera">CompradorCartera</param>
        /// <param name="pagaduria">Pagaduria</param>
        /// <param name="centroPago">CentroPago</param>
        /// <param name="agrup">Agrup</param>
        /// <param name="romp">Romp</param>
        /// <returns></returns>
        public DataTable Report_Cc_CobroJuridicoToExcel(int documentoID, byte tipoReporte, string cliente, string libranza, byte claseDeuda)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_CobroJuridicoToExcel(this._userChannel, documentoID, tipoReporte, cliente, libranza, claseDeuda);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Reporte Datacrédito
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <returns></returns>
        public DataTable Report_Cc_DataCredito(DateTime periodo,byte tipo)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_DataCredito(this._userChannel, periodo,tipo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Reporte Certificados
        /// </summary>
        /// <param name="documentID">DocumentoID</param>
        /// <param name="Lista de campos">data</param>
        /// <returns>nombre de reporte</returns>
        public string Report_Cc_Certificados(int document, Dictionary<int, string> data)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_Certificados(this._userChannel, document,data);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Reporte Cartas
        /// </summary>
        /// <param name="documentID">DocumentoID</param>
        /// <param name="Lista de campos">data</param>
        /// <returns>nombre de reporte</returns>
        public string Report_Cc_CartaCierreDiario(int document, string tipoReport,List<DTO_ccHistoricoGestionCobranza> dataHistorico)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_CartaCierreDiario(this._userChannel, document, tipoReport, dataHistorico);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Reporte de GEstion de Cobranza Dia o mes
        /// </summary>
        /// <param name="documentID">DocumentoID</param>
        /// <param name="tipoReporte">Tipo de reporte</param>
        /// <param name="fechaCorte">fecha periodo</param>
        /// <returns>nombre de reporte</returns>
        public string Report_Cc_GestionCobranza(int document, string tipoReport, DateTime fechaCorte, string etapa,bool excluyeDemanda)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_GestionCobranza(this._userChannel, document, tipoReport, fechaCorte,etapa,excluyeDemanda);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #region Incorporaciones

        /// <summary>
        /// Genreal el reporte de los creditos incorporados
        /// </summary>
        /// <param name="numeroDoc"></param>
        /// <param name="isLiquidacion"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public string Report_Cc_Incorporacion(int numeroDoc, bool isLiquidacion, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_Incorporacion(this._userChannel, numeroDoc, true, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que se encarga del nombre del reporte
        /// </summary>
        /// <param name="FechaInicial">Filtro de Fecha Inicial desde que fecha se desean ver las Incoporaciones</param>
        /// <param name="FechaFinal">Filtro de Fecha Final hasta que fecha se desean ver las Incoporaciones</param>
        /// <param name="Pagaduria">Pagaduria que se desea filtrar</param>
        /// <param name="formatType">Tipo de Exporataion del reporte</param>
        /// <returns></returns>
        public string ReportesCartera_Cc_PagaduriaIncorporacion(DateTime FechaInicial, DateTime FechaFinal, string Pagaduria, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesCartera_Cc_PagaduriaIncorporacion(this._userChannel, FechaInicial, FechaFinal, Pagaduria, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion q genera el archivo en excel
        /// </summary>
        /// <param name="FechaInicial">Filtro de Fecha Inicial desde que fecha se desean ver las Incoporaciones</param>
        /// <param name="FechaFinal">Filtro de Fecha Final hasta que fecha se desean ver las Incoporaciones</param>
        /// <param name="Pagaduria">Pagaduria que se desea filtrar</param>
        /// <param name="formatType">Tipo de Exporataion del reporte</param>
        /// <returns></returns>
        public string ReportesCartera_Cc_PagaduriaIncorporacionPlantilla(DateTime FechaInicial, DateTime FechaFinal, string Pagaduria)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesCartera_Cc_PagaduriaIncorporacionPlantilla(this._userChannel, FechaInicial, FechaFinal, Pagaduria);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Informe SIGCOOP

        /// <summary>
        /// Funcion que se encarga de los datos de Excel
        /// </summary>
        /// <param name="Periodo">Periodo a filtrar</param>
        /// <param name="Formato">Tipo de Formato que desea Exportar</param>
        /// <returns></returns>
        public DataTable ReportesCartera_Cc_InformeSIGCOOP(DateTime Periodo, string Formato)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesCartera_Cc_InformeSIGCOOP(this._userChannel, Periodo, Formato);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Referenciacion

        /// <summary>
        /// Funcion q se encarga de generar el reporte
        /// </summary>
        /// <param name="libranza">Numero de la libranza q se desea ver</param>
        /// <param name="cliente">Clinte que se va a filtrar</param>
        /// <param name="FechaRef">Fecha en que se referencio el credito</param>
        /// <returns>Nombre del reporte</returns>
        public DTO_TxResult ReportesCartera_Cc_Referenciacion(string libranza, string cliente, DateTime FechaRef, bool _llamadaCtrl, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesCartera_Cc_Referenciacion(this._userChannel, libranza, cliente, FechaRef, _llamadaCtrl, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Libranzas
        /// <summary>
        /// Funcion que dvuelve le nombre del reporte generado
        /// </summary>
        /// <returns>nombre del reporte</returns>
        public DTO_TxResult ReportesCartera_Cc_Libranzas(DateTime Periodo, DateTime PeriodoFin, string Cliente, string Libranza, string Asesor, string Pagaduria, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesCartera_Cc_Libranzas(this._userChannel, Periodo, PeriodoFin, Cliente, Libranza, Asesor, Pagaduria, formatType);
                return result;

            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que dvuelve le nombre del reporte generado
        /// </summary>
        /// <returns>nombre del reporte</returns>
        public DataTable ReportesCartera_Cc_LibranzasExcel(DateTime Periodo, DateTime PeriodoFin, string Cliente, string Libranza, string Asesor, string Pagaduria)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesCartera_PlantillaExcelLibranza(this._userChannel, Periodo, PeriodoFin, Cliente, Libranza, Asesor, Pagaduria);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que carga una lista con información de los creditos segun filtro
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="cliente"></param>
        /// <param name="libranza"></param>
        /// <param name="zonaID"></param>
        /// <param name="ciudad"></param>
        /// <param name="pagaduria"></param>
        /// <param name="centroPagoID"></param>
        /// <param name="asesor"></param>
        /// <param name="lineaCredi"></param>
        /// <param name="compCartera"></param>
        /// <returns>Lsta de Creditos</returns>
        public string Report_Cc_AnalisisPagos(byte tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string concesionario, string asesor, string lineaCredi, string compCartera, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_AnalisisPagos(this._userChannel, tipoReporte, fechaIni, fechaFin, cliente, libranza, zonaID, ciudad, concesionario, asesor, lineaCredi, compCartera, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que carga una lista con información de los creditos segun filtro
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="cliente"></param>
        /// <param name="libranza"></param>
        /// <param name="zonaID"></param>
        /// <param name="ciudad"></param>
        /// <param name="pagaduria"></param>
        /// <param name="centroPagoID"></param>
        /// <param name="asesor"></param>
        /// <param name="lineaCredi"></param>
        /// <param name="compCartera"></param>
        /// <returns>Lsta de Creditos</returns>
        public DataTable Report_Cc_AnalisisPagosExcel(byte tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string concesionario, string asesor, string lineaCredi, string compCartera, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_AnalisisPagosExcel(this._userChannel, tipoReporte, fechaIni, fechaFin, cliente, libranza, zonaID, ciudad, concesionario, asesor, lineaCredi, compCartera, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Liquidacion Credritos

        public string ReportesCartera_Cc_LiquidacionCredito(int libranza, ExportFormatType formatType, int numDoc)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.ReportesService.ReportesCartera_Cc_LiquidacionCredito(this._userChannel, libranza, formatType, numDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        #endregion

        #region Saldos

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="periodo">Fecha inicial del reporte</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cc_Saldos(DateTime periodo, string cliente, string pagaduria, string lineaCredi, string compCartera, string asesor, string tipoCartera, bool isSaldoFavor, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_Saldos(this._userChannel, periodo, cliente, pagaduria, lineaCredi, compCartera, asesor, tipoCartera, isSaldoFavor, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que devuele el nombre del reporte
        /// </summary>
        /// <param name="periodo">Periodo del reporte</param>
        /// <param name="clienteFiltro">Filtro del Cliente</param>
        /// <param name="libranzaFiltro">Filtro Libranza</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cc_SaldosAFavor(DateTime periodo, string cliente, string pagaduria, string lineaCredi, string compCartera, string asesor, string tipoCartera, bool isSaldoFavor, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_SaldosAFavor(this._userChannel, periodo, cliente, pagaduria, lineaCredi, compCartera, asesor, tipoCartera, isSaldoFavor, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="perido"></param>
        /// <param name="cliente"></param>
        /// <param name="pagaduria"></param>
        /// <param name="lineaCredi"></param>
        /// <param name="compCartera"></param>
        /// <param name="asesor"></param>
        /// <param name="plazo"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public string Report_Cc_SaldosMora(DateTime perido, string cliente, string pagaduria, string lineaCredi, string compCartera, string asesor, int plazo, string tipoCartera, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_SaldosMora(this._userChannel, perido, cliente, pagaduria, lineaCredi, compCartera, asesor, plazo, tipoCartera, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// CARTERA EN MORA
        /// </summary>
        /// <param name="perido"></param>
        /// <param name="cliente"></param>
        /// <param name="pagaduria"></param>
        /// <param name="lineaCredi"></param>
        /// <param name="compCartera"></param>
        /// <param name="asesor"></param>
        /// <param name="plazo"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public string Report_Cc_CarteraMora(DateTime periodo, DateTime fechaIni, DateTime fechaFin, string comprador, string oferta, string libranza, bool isResumida, string orden, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_CarteraMora(this._userChannel, periodo, fechaIni, fechaFin, comprador, oferta, libranza, isResumida, orden, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Cartera Saldos
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="cliente"></param>
        /// <param name="libranza"></param>
        /// <param name="zonaID"></param>
        /// <param name="ciudad"></param>
        /// <param name="concesionario"></param>
        /// <param name="asesor"></param>
        /// <param name="lineaCredi"></param>
        /// <param name="compCartera"></param>
        /// <param name="agrupamiento"></param>
        /// <param name="romp"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public object Report_Cc_SaldosNuevo(byte tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string concesionario, string asesor, string lineaCredi, string compCartera, byte agrupamiento, byte romp, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cc_SaldosNuevo(this._userChannel, tipoReporte, fechaIni, fechaFin, cliente, libranza, zonaID, ciudad, concesionario, asesor, lineaCredi, compCartera, agrupamiento, romp, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        #endregion

        #region Solicitudes

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="fechaIncial">Fecha inicial por la cual se va a filtrar</param>
        /// <param name="fechaFinal">Fecha final por la cual se va a filtrar</param>
        /// <param name="cliente">Cliente que se desea filtraar</param>
        /// <param name="libranza">Numero de lalibranza por  la cual se va a filtrar</param>
        /// <param name="asesor">Asesor por el cual se va a filtrar</param>
        /// <param name="estado">Filtro en q estan las solicitudes</param>
        /// <param name="formatType">Forma de exportar el reporte</param>
        /// <returns>URL del reporte</returns>
        public string ReportesCartera_Cc_Solicitudes(DateTime fechaIncial, DateTime fechaFinal, string cliente, string libranza, string asesor, string estado, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                return this.ReportesService.ReportesCartera_Cc_Solicitudes(this._userChannel, fechaIncial, fechaFinal, cliente, libranza, asesor, estado, formatType);

            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Venta Cartera

        /// <summary>
        /// Servicio q carga el nombre del reporte
        /// </summary>
        /// <param name="fechaIni">Mes inicial por el cual se va a filtrar</param>
        /// <param name="fechaFin">Mes final por el cual se va a filtrar</param>
        /// <param name="comprador">Comprador por el cual se desea filtrar</param>
        /// <param name="oferta">Oferta que se desea ver</param>
        /// <param name="libranza">Numero de Libranza por el cual se desea ver</param>
        /// <param name="isResumida">Filtra el reportes (True) para Resumido (False) para Detallado</param>
        /// <param name="formatType">Formato en que se va exportar el reporte</param>
        /// <returns>URL del reporte</returns>
        public string ReportesCartera_VentaCartera(DateTime fechaIni, DateTime fechaFin, string comprador, string oferta, string libranza, bool isResumida, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesCartera_VentaCartera(this._userChannel, fechaIni, fechaFin, comprador, oferta, libranza, isResumida, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Servicio q carga el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="fechaIni">Mes inicial por el cual se va a filtrar</param>
        /// <param name="fechaFin">Mes final por el cual se va a filtrar</param>
        /// <param name="comprador">Comprador por el cual se desea filtrar</param>
        /// <param name="oferta">Oferta que se desea ver</param>
        /// <param name="libranza">Numero de Libranza por el cual se desea ver</param>
        /// <param name="formatType">Formato en que se va exportar el reporte</param>
        /// <returns>URL del reporte</returns>
        public string ReportesCartera_VentaCarteraDetallado(DateTime fechaIni, DateTime fechaFin, string comprador, string oferta, string libranza, bool isResumida, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesCartera_VentaCarteraDetallado(this._userChannel, fechaIni, fechaFin, comprador, oferta, libranza, isResumida, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Crédito
        /// <summary>
        /// Reporte de Credito
        /// </summary>
        /// <param name="fechaIncial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="libranza"></param>
        /// <param name="Credito"></param>
        /// <returns></returns>
        public string Reports_cc_Credito(int mesIni, int mesFin, int año, string libranza, string Credito)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                return this.ReportesService.Reports_cc_Credito(this._userChannel, mesIni, mesFin, año, libranza, Credito);

            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        ///  Reporte en Excel
        /// </summary>
        /// <param name="mesIni"></param>
        /// <param name="mesFin"></param>
        /// <param name="año"></param>
        /// <param name="libranza"></param>
        /// <param name="Credito"></param>
        /// <returns></returns>
        public DataTable Reports_cc_CreditoXLS(string Credito)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reports_cc_CreditoXLS(this._userChannel, Credito);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        #endregion

        #region Prejuridico
        /// <summary>
        /// Reporte Prejuridico (CF)
        /// </summary>
        /// <param name="_tercero"></param>
        /// <param name="_mesIni"></param>
        /// <param name="_mesFin"></param>
        /// <param name="_año"></param>
        /// <returns>Retorna la Informacion del reporte Prejuridico (PDF)</returns>
        public string Report_cc_Prejuridico(string _tercero, int _mesIni, int _mesFin, int _año, string _report)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                return this.ReportesService.Report_cc_Prejuridico(this._userChannel, _tercero, _mesIni, _mesFin, _año, _report);

            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Exporta el reporte en excel del formulario Preventa
        /// </summary>
        /// <param name="_libranzaTercero"></param>
        /// <param name="_cedulaTercero"></param>
        /// <returns></returns>
        public DataTable ExportExcel_cc_GetVistaCesionesByPreventa(List<int> numeroDocs)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ExportExcel_cc_GetVistaCesionesByPreventa(this._userChannel, numeroDocs);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Contabilidad

        #region Documentos

        #region Comprobante Manual

        /// <summary>
        /// Funcion que se encarga de traer el resultado con el nombre del reporte
        /// </summary>
        /// <param name="numeroDoc">Identificador de documentos</param>
        /// <param name="isAprovada">Verifica si es aprobada (True: Trae los Datos de la Tabla coAuxiliar, False: Trae los datos de la tabla coAuxiliarPre) </param>
        /// <param name="moneda">Verifica la moneda que se esta trabajando (True:Local, False: Extranjera) </param>
        /// <param name="formatType">Forma de exportar el Reporte</param>
        /// <returns></returns>
        public string ReportesContabilidad_ComprobanteManual(int numeroDoc, bool isAprovada, bool moneda, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_ComprobanteManual(this._userChannel, numeroDoc, isAprovada, moneda, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Documento Contable

        /// <summary>
        /// Funcion que se encarga del nombre del reporte
        /// </summary>
        /// <param name="numDoc">Identificador del con q se guardan los registro en la BD</param>
        /// <param name="isAprovada">Obtiene la informacion (true = Aprobada, Trae la info de coAuxilar; False = ParaAprobacion, Trae la info de coAuxilarPre </param>
        /// <param name="documento">Tipo de documento a imprimir, coloca el nombre del reporte</param>
        /// <param name="formatType">Formato para exportar el reporte</param>
        /// <returns>Nombre del reporte</returns>
        public string ReportesContabilidad_DocumentoContable(int numDoc, bool isAprovada, int documento, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_DocumentoContable(this._userChannel, numDoc, isAprovada, documento, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Reportes PDF

        #region General
        /// <summary>
        /// Funcion q se encarga de traer el nombre del reporte
        /// </summary>
        /// <param name="documentID">documento del reporte</param>
        /// <param name="tipoRep">Tipo de reporte</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">FechaFinal</param>
        /// <param name="libro">Libro balance</param>
        /// <param name="compID">ComprobanteID</param>
        /// <param name="compNro">Comp nro</param>
        /// <param name="cuenta">Cuenta ID</param>
        /// <param name="tercero">Tercero ID</param>
        /// <param name="proyecto">Proyecto ID</param>
        /// <param name="centroCto">Centro Cto ID</param>
        /// <param name="lineaPres">Linea Pres ID</param>
        /// <param name="otroFilter">Otro Filtro</param>
        /// <param name="orden">Ordenamiento</param>
        /// <param name="romp">Rompimiento</param>
        /// <returns>nombre del reporte</returns>
        public string ReportesContabilidad_GetByParameter(int documentID, byte? tipoRep, DateTime? fechaIni, DateTime? fechaFin, string libro, string compID, int? compNro,
                      string cuenta, string tercero, string proyecto, string centroCto, string lineaPres, object otroFilter, byte? orden, byte? romp)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_GetByParameter(this._userChannel, documentID, tipoRep, fechaIni, fechaFin, libro,
                    compID, compNro, cuenta, tercero, proyecto, centroCto, lineaPres, otroFilter, orden, romp);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Auxiliar

        /// <summary>
        /// Funcion que reptorna el nombre del reporte Auxiliar ML
        /// </summary>
        /// <param name="año">Año que se va a mostrar</param>
        /// <param name="mesInicial">Mes por el cual se va a filtrar</param>
        /// <param name="mesFin">Mes por el cual se va a filtrar</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuentaInicial">Tipo de cuenta que se desea ver</param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <param name="formatType">Tipo de formato que se va a exportar el reporte</param>
        /// <returns>Nombre del Reporte</returns>
        public string ReportesContabilidad_AuxiliarML(DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFin,
           string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_AuxiliarML(this._userChannel, fechaInicial, fechaFinal, libro, cuentaInicial, cuentaFin, tercero,
                    proyecto, centroCosto, lineaPresupuestal, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que reptorna el nombre del reporte Auxiliar ME
        /// </summary>
        /// <param name="año">Año que se va a mostrar</param>
        /// <param name="mesInicial">Mes por el cual se va a filtrar</param>
        /// <param name="mesFin">Mes por el cual se va a filtrar</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuentaInicial">Tipo de cuenta que se desea ver</param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <param name="formatType">Tipo de formato que se va a exportar el reporte</param>
        /// <returns>Nombre del Reporte</returns>
        public string ReportesContabilidad_AuxiliarME(DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFin,
           string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_AuxiliarME(this._userChannel, fechaInicial, fechaFinal, libro, cuentaInicial, cuentaFin, tercero,
                    proyecto, centroCosto, lineaPresupuestal, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que reptorna el nombre del reporte Auxiliar Ambas MOnedas
        /// </summary>
        /// <param name="año">Año que se va a mostrar</param>
        /// <param name="mesInicial">Mes por el cual se va a filtrar</param>
        /// <param name="mesFin">Mes por el cual se va a filtrar</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuenta">Tipo de cuenta que se desea ver</param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <param name="formatType">Tipo de formato que se va a exportar el reporte</param>
        /// <returns>Nombre del Reporte</returns>
        public string ReportesContabiliad_AuxiliarMultiMoneda(DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFin,
           string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabiliad_AuxiliarMultiMoneda(this._userChannel, fechaInicial, fechaFinal, libro, cuentaInicial, cuentaFin, tercero,
                    proyecto, centroCosto, lineaPresupuestal, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que reptorna el nombre del reporte Auxiliar x Tercero ML
        /// </summary>
        /// <param name="año">Año que se va a mostrar</param>
        /// <param name="mesInicial">Mes por el cual se va a filtrar</param>
        /// <param name="mesFin">Mes por el cual se va a filtrar</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuentaInicial">Tipo de cuenta que se desea ver</param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <param name="formatType">Tipo de formato que se va a exportar el reporte</param>
        /// <returns>Nombre del Reporte</returns>
        public string ReportesContabilidad_AuxiliarxTerceroML(DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFinal,
           string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_AuxiliarxTerceroML(this._userChannel, fechaInicial, fechaFinal, libro, cuentaInicial, cuentaFinal, tercero,
                    proyecto, centroCosto, lineaPresupuestal, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que reptorna el nombre del reporte Auxiliar x Tercero ME
        /// </summary>
        /// <param name="año">Año que se va a mostrar</param>
        /// <param name="mesInicial">Mes por el cual se va a filtrar</param>
        /// <param name="mesFin">Mes por el cual se va a filtrar</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuentaInicial">Tipo de cuenta que se desea ver</param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <param name="formatType">Tipo de formato que se va a exportar el reporte</param>
        /// <returns>Nombre del Reporte</returns>
        public string ReportesContabilidad_AuxiliarxTerceroME(DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFinal,
            string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_AuxiliarxTerceroME(this._userChannel, fechaInicial, fechaFinal, libro, cuentaInicial, cuentaFinal, tercero,
                    proyecto, centroCosto, lineaPresupuestal, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que reptorna el nombre del reporte Auxiliar x Tercero Ambas monedas
        /// </summary>
        /// <param name="año">Año que se va a mostrar</param>
        /// <param name="mesInicial">Mes por el cual se va a filtrar</param>
        /// <param name="mesFin">Mes por el cual se va a filtrar</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuentaInicial">Tipo de cuenta que se desea ver</param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <param name="formatType">Tipo de formato que se va a exportar el reporte</param>
        /// <returns>Nombre del Reporte</returns>
        public string ReportesContabiliad_AuxiliarxTerceroMultiMoneda(DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFinal,
         string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabiliad_AuxiliarxTerceroMultiMoneda(this._userChannel, fechaInicial, fechaFinal, libro, cuentaInicial, cuentaFinal,
                    tercero, proyecto, centroCosto, lineaPresupuestal, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que se encarga del nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mes">Mes que se desea ver</param>
        /// <param name="tipoBalance">Libro que se va a consultar</param>
        /// <param name="cuentaIni">Filtro rango cuentas, Cuenta Inicial</param>
        /// <param name="cuentaFin">Filtro rango cuentas, Cuenta Final</param>
        /// <returns>URL con el nombre del reporte</returns>
        public string ReportesContabilidad_PlantillaExcelAuxiliar(DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial,
            string cuentaFin, string tercero, string proyecto, string centroCosto, string lineaPresupuestal)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_PlantillaExcelAuxiliar(this._userChannel, fechaInicial, fechaFinal, libro, cuentaInicial, cuentaFin,
                    tercero, proyecto, centroCosto, lineaPresupuestal);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        #endregion

        #region Balance

        public string ReporteContabilidad_BalanceComparativo(string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_BalanceComparativo(this._userChannel, libroAux, moneda, año, fechaFin, fechaIni, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        public string ReporteContabilidad_BalanceComparativoME(string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_BalanceComparativoME(this._userChannel, libroAux, moneda, año, fechaFin, fechaIni, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        public string ReportesContabilidad_BalanceComparativosSaldosML(string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_BalanceComparativosSaldosML(this._userChannel, libroAux, moneda, año, fechaFin, fechaIni, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        public string ReportesContabilidad_BalanceComparativosSaldosME(string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_BalanceComparativosSaldosME(this._userChannel, libroAux, moneda, año, fechaFin, fechaIni, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        public string ReportesContabilidad_BalanceComparativosSaldosAM(string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_BalanceComparativosSaldosAM(this._userChannel, libroAux, moneda, año, fechaFin, fechaIni, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="mesFin">Mes de consulta</param>
        /// <returns>Nombre del reporte</returns>
        public string ReportesContabilidad_InventariosBalance(int mesIni, int mesFin, string Libro, string cuentaIni, string cuentaFin, int _año, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_InventariosBalance(this._userChannel, mesIni, mesFin, Libro, cuentaIni, cuentaFin, _año, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="mesFin">Mes de consulta</param>
        /// <returns>Nombre del reporte</returns>
        public string ReportesContabilidad_InventariosBalanceSinSaldo(int mesIni,int mesFin, string Libro, string cuentaIni, string cuentaFin, int _año, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_InventariosBalanceSinSaldo(this._userChannel,mesIni, mesFin, Libro, cuentaIni, cuentaFin, _año, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que se encarga del nombre del reporte
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mes">Mes que se desea ver</param>
        /// <param name="tipoBalance">Libro que se va a consultar</param>
        /// <param name="cuentaIni">Filtro rango cuentas, Cuenta Inicial</param>
        /// <param name="cuentaFin">Filtro rango cuentas, Cuenta Final</param>
        /// <returns>URL con el nombre del reporte</returns>
        public string ReportesContabilidad_PlantillaExcelInventarioBalance(int mes, string Libro, string cuentaIni, string cuentaFin, int _año)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_PlantillaExcelInventarioBalance(this._userChannel, mes, Libro, cuentaIni, cuentaFin, _año);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }


        #endregion

        #region Certificados

        /// <summary>
        /// Funcion que se encarga de traer el certificado de Rete Fuente
        /// </summary>
        /// <param name="Periodo">Periodo de Consulta</param>
        /// <param name="Impuesto">Impuesto al que se desea realizar el certificado</param>
        /// <param name="formatType">Tipo de Formto a exportar el Reporte</param>
        /// <returns>Resultado con URL del reporte</returns>
        public DTO_TxResult ReportesContabilidad_CertificadoReteFuente(DateTime Periodo, string Impuesto, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_CertificadoReteFuente(this._userChannel, Periodo, Impuesto, formatType);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Comprobantes

        #region Comprobante

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="mes">Mes de consulta</param>
        /// <returns>Nombre del reporte</returns>
        public DTO_TxResult ReportesContabilidad_Comprobante(int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, bool porHoja, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_Comprobante(this._userChannel, año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal, porHoja, formatType);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="mes">Mes de consulta</param>
        /// <returns>Nombre del reporte</returns>
        public DTO_TxResult ReportesContabilidad_ComprobanteME(int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, bool porHoja, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_ComprobanteME(this._userChannel, año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal, porHoja, formatType);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="mes">Mes de consulta</param>
        /// <returns>Nombre del reporte</returns>
        public DTO_TxResult ReportesContabilidad_ComprobanteMLyME(int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, bool porHoja, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_ComprobanteMLyME(this._userChannel, año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal, porHoja, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Preliminar

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="mes">Mes de consulta</param>
        /// <returns>Nombre del reporte</returns>
        public DTO_TxResult ReportesContabilidad_ComprobantePreliminar(int documentID, int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));

                var result = this.ReportesService.ReportesContabilidad_ComprobantePreliminar(this._userChannel,documentID, año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal, formatType);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="mes">Mes de consulta</param>
        /// <returns>Nombre del reporte</returns>
        public DTO_TxResult ReportesContabilidad_ComprobantePreliminarME(int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));

                var result = this.ReportesService.ReportesContabilidad_ComprobantePreliminarME(this._userChannel, año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal, formatType);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="mes">Mes de consulta</param>
        /// <returns>Nombre del reporte</returns>
        public DTO_TxResult ReportesContabilidad_ComprobantePreliminarMLyME(int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_ComprobantePreliminarMLyME(this._userChannel, año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal, formatType);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Control

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="mes">Mes de consulta</param>
        /// <returns>Nombre del reporte</returns>
        public DTO_TxResult ReportesContabilidad_ComprobanteControl(int año, int mes, string comprobanteID, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_ComprobanteControl(this._userChannel, año, mes, comprobanteID, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Libros

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mes">Mes de consulta</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="filter">Filtros</param>
        /// <returns>Nombre del reporte</returns>
        public string ReportesContabilidad_LibroDiario(int año, int mes, string tipoBalance, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_LibroDiario(this._userChannel, año, mes, tipoBalance, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mes">Mes de consulta</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="filter">Filtros</param>
        /// <returns>Nombre del reporte</returns>
        public string ReportesContabilidad_LibroDiarioComprobante(int año, int mes, string tipoBalance, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_LibroDiarioComprobante(this._userChannel, año, mes, tipoBalance, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mes">Mes de consulta</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="filter">Filtros</param>
        /// <returns>Nombre del reporte</returns>
        public string ReportesContabilidad_LibroMayor(int año, int mes, string tipoBalance,/*, string cuentaIni, string cuentaFin,*/ ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_LibroMayor(this._userChannel, año, mes, tipoBalance,/*, cuentaIni, cuentaFin,*/ formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que se encarga del nombre del reporte
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mes">Mes que se desea ver</param>
        /// <param name="tipoBalance">Libro que se va a consultar</param>
        /// <param name="cuentaIni">Filtro rango cuentas, Cuenta Inicial</param>
        /// <param name="cuentaFin">Filtro rango cuentas, Cuenta Final</param>
        /// <returns>URL con el nombre del reporte</returns>
        public string ReportesContabilidad_PlantillaExcelLibroMayor(int año, int mes, string tipoBalance/*, string cuentaIni, string cuentaFin*/)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_PlantillaExcelLibroMayor(this._userChannel, año, mes, tipoBalance/*, cuentaIni, cuentaFin*/);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        #endregion

        #region Saldo

        #region Filtro Cuenta

        /// <summary>
        /// Funcion que retorna el nombre del reporte (Cuenta-Tercero)
        /// </summary>
        /// <param name="año">Año de consulta</param>
        /// <param name="mes">Mes que se quiere filtar</param>
        /// <param name="libro">Tipo de libro a mostrar</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns></returns>
        public string ReportesContabilidad_SaldosCuentaTercero(int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_SaldosCuentaTercero(this._userChannel, año, mesInicial, mesFin, libro, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
            //public string ReportesContabilidad_SaldosFuncional(int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType)
            //{
            //    try
            //    {
            //        this.CreateService(typeof(IReportesService));
            //        var result = this.ReportesService.ReportesContabilidad_SaldosFuncional(this._userChannel, año,  mesInicial,  mesFin, libro, formatType);
            //        return result;
            //    }
            //    catch (Exception ex)
            //    {
            //        this.AbortService(typeof(IReportesService));
            //        throw ex;
            //    }

            //}

        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte (Cuenta-CentroCosto)
        /// </summary>
        /// <param name="año">Año de consulta</param>
        /// <param name="mes">Mes que se quiere filtar</param>
        /// <param name="libro">Tipo de libro a mostrar</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns></returns>
        public string ReportesContabilidad_SaldosCuentaCentroCosto(int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_SaldosCuentaCentroCosto(this._userChannel, año, mesInicial, mesFin, libro, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }

        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte (Cuenta-Proyecto)
        /// </summary>
        /// <param name="año">Año de consulta</param>
        /// <param name="mes">Mes que se quiere filtar</param>
        /// <param name="libro">Tipo de libro a mostrar</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns></returns>
        public string ReportesContabilidad_SaldosCuentaProyecto(int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_SaldosCuentaProyecto(this._userChannel, año, mesInicial, mesFin, libro, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }

        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte (Cuenta-LinieaPresupuesto)
        /// </summary>
        /// <param name="año">Año de consulta</param>
        /// <param name="mes">Mes que se quiere filtar</param>
        /// <param name="libro">Tipo de libro a mostrar</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns></returns>
        public string ReportesContabilidad_SaldosCuentaLineaPresupuesto(int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_SaldosCuentaLineaPresupuesto(this._userChannel, año, mesInicial, mesFin, libro, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }

        }
        #endregion

        #region Filtro Tercero

        /// <summary>
        /// Funcion que retorna el nombre del reporte (Tercero-Cuenta)
        /// </summary>
        /// <param name="año">Año de consulta</param>
        /// <param name="mes">Mes que se quiere filtar</param>
        /// <param name="libro">Tipo de libro a mostrar</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns></returns>
        public string ReportesContabilidad_SaldosTerceroCuenta(int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_SaldosTerceroCuenta(this._userChannel, año, mesInicial, mesFin, libro, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }

        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte (Tercero-CentroCosto)
        /// </summary>
        /// <param name="año">Año de consulta</param>
        /// <param name="mes">Mes que se quiere filtar</param>
        /// <param name="libro">Tipo de libro a mostrar</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns></returns>
        public string ReportesContabilidad_SaldosTerceroCentroCosto(int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_SaldosTerceroCuenta(this._userChannel, año, mesInicial, mesFin, libro, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }

        }

        #endregion

        #endregion

        #region Tasas

        /// <summary>
        /// Funcion que se encarga de generar el reporte
        /// </summary>
        /// <param name="Periodo">Periodo de consulta</param>
        /// <param name="isDiaria">Tipo de reporte a imprimir (True: Reportes Tasa Cierre, False: Reprote Tasa Diaria)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Resultado con la URL del reporte</returns>
        public DTO_TxResult ReportesContabilidad_Tasas(DateTime Periodo, bool isDiarias, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));

                DTO_TxResult result = new DTO_TxResult();

                if (!isDiarias)
                    result = this.ReportesService.ReportesContabilidad_TasasCierre(this._userChannel, Periodo, isDiarias, formatType);
                else
                    result = this.ReportesService.ReportesContabilidad_TasasDiarias(this._userChannel, Periodo, isDiarias, formatType);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Varios
        /// <summary>
        ///  Permite traer los saldos de un tipo de reporte con lineas y filtros personalizados
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="ReporteID">Periodo Inicial</param>
        /// <param name="PeriodoIni">Periodo Inicial</param>
        /// <param name="PeriodoFin">Periodo Inicial</param>
        /// <returns>Resultado con la URL del reporte</returns>
        public string ReportesContabilidad_ReporteLineaParametrizable(int documentReportID, string reporteID, byte tipoReport, DateTime Periodoini, DateTime PeriodoFin)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_ReporteLineaParametrizable(this._userChannel, documentReportID, reporteID, tipoReport, Periodoini, PeriodoFin);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        ///  Permite crear el reporte de presupuesto contable
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="periodo">Periodo Inicial</param>
        /// <param name="proyecto">Periodo Inicial</param>
        /// <param name="libro">Periodo Inicial</param>
        /// <param name="monedaID">Periodo Inicial</param>
        /// <returns>Resultado con la URL del reporte</returns>
        public string ReportesContabilidad_EjecucionPresupuestal(DateTime periodo, byte rompimiento, string proyecto, string centroCto, string libro, string monedaID)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_EjecucionPresupuestal(this._userChannel, periodo, rompimiento, proyecto, centroCto, libro, monedaID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        #endregion

        #endregion

        #region Reportes XLS

        /// <summary>
        /// Obtiene un datatable con la info de Tesoreria segun filtros
        /// </summary>
        /// <param name="documentoID">Documento relacionado</param>
        /// <param name="tipoReporte">Tipo reporte</param>
        /// <param name="fechaIni">Fecha ini</param>
        /// <param name="fechaFin">Fecha Fin</param>
        /// <param name="terceroID">Tercero</param>
        /// <param name="cuentaID">Cuenta</param>
        /// <param name="centroCtoID">Centro Cto</param>
        /// <param name="proyectoID">Proyecto</param>
        /// <param name="lineaPresupID">Linea Presup</param>
        /// <param name="balanceTipo">Balance Tipo</param>
        /// <param name="comprobID">Comprobante ID</param>
        /// <param name="compNro">Comp nro</param>
        /// <param name="otroFilter">otro filtro</param>
        /// <param name="agrup">Agrupar</param>
        /// <param name="romp">romper u ordenar</param>
        /// <returns>datatable</returns>
        public DataTable Reportes_Co_ContabilidadToExcel(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string terceroID, string cuentaID, string centroCtoID, string proyectoID, string lineaPresupID, string balanceTipo, string comprobID,
                                                         string compNro, object otroFilter, byte? agrup, byte? romp)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reportes_Co_ContabilidadToExcel(this._userChannel, documentoID, tipoReporte, fechaIni,fechaFin, terceroID,cuentaID,centroCtoID,proyectoID, lineaPresupID,balanceTipo,comprobID,compNro,otroFilter,agrup,romp);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }


        #region Balance

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Periodo"></param>
        /// <param name="LongitudCuenta"></param>
        /// <param name="SaldoIncial"></param>
        /// <param name="CuentaInicial"></param>
        /// <param name="CuentaFinal"></param>
        /// <param name="libro"></param>
        /// <param name="tipoReport"></param>
        /// <param name="Moneda"></param>
        /// <returns></returns>
        public DataTable ReportesContabilidad_BalancePruebas(DateTime Periodo, int LongitudCuenta, int SaldoIncial, string CuentaInicial, string CuentaFinal, string libro, string tipoReport, string Moneda)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_BalancePruebas(this._userChannel, Periodo, LongitudCuenta, SaldoIncial, CuentaInicial, CuentaFinal, libro,
                    tipoReport, Moneda);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// <Reporte Balance 2>
        /// </summary>
        /// <param name="Periodo"></param>
        /// <param name="LongitudCuenta"></param>
        /// <param name="SaldoIncial"></param>
        /// <param name="CuentaInicial"></param>
        /// <param name="CuentaFinal"></param>
        /// <param name="libro"></param>
        /// <param name="tipoReport"></param>
        /// <param name="Moneda"></param>
        /// <returns></returns>

        public DataTable ReportesContabilidad_ReporteBalancePruebasXLS(int año, int LongitudCuenta, int SaldoIncial, string CuentaInicial,
                                     string CuentaFinal, string libro, string tipoReport, string Moneda, int MesInicial, int MesFinal, byte? Combo1, byte? Combo2, string proyecto, string centroCto)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_ReporteBalancePruebasXLS(this._userChannel, año, LongitudCuenta, SaldoIncial, CuentaInicial,
                                                CuentaFinal, libro, tipoReport, Moneda, MesInicial, MesFinal, Combo1, Combo2, proyecto, centroCto);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// crea el repoprte de balance
        /// </summary>
        /// <param name="Periodo"></param>
        /// <param name="LongitudCuenta"></param>
        /// <param name="SaldoIncial"></param>
        /// <param name="CuentaInicial"></param>
        /// <param name="CuentaFinal"></param>
        /// <param name="libro"></param>
        /// <param name="tipoReport"></param>
        /// <param name="Moneda"></param>
        /// <returns>nombre del reporte</returns>
        public string ReportesContabilidad_ReportBalancePruebas(int Periodo, int LongitudCuenta, int SaldoIncial, string CuentaInicial, string CuentaFinal, string libro, string tipoReport, string Moneda, int _fechaIni, int _fechaFin, byte? Combo1, byte? Combo2, string proyecto, string centroCto)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var Resultado = this.ReportesService.ReportesContabilidad_ReportBalancePruebas(this._userChannel, Periodo, LongitudCuenta, SaldoIncial, CuentaInicial, CuentaFinal, libro, tipoReport, Moneda, _fechaIni, _fechaFin, Combo1, Combo2, proyecto, centroCto);
                return Resultado;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Comprobante

        /// <summary>
        /// Funcion que se encarga de traer la informaion para Generar el XLS de comprobantes
        /// </summary>
        /// <param name="Periodo">Periodo de consulta</param>
        /// <param name="comprobanteID">Filtra un comprobante especifico</param>
        /// <param name="libro">Libro a mostrar</param>
        /// <param name="comprobanteInicial">Numero de comprobante Inicial</param>
        /// <param name="comprobanteFinal">Numero de comprobante Final</param>
        /// <returns>Tabla con Resultados</returns>
        public DataTable ReportesContabilidad_ComprobanteXLS(DateTime Periodo, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesContabilidad_ComprobanteXLS(this._userChannel, Periodo, comprobanteID, libro, comprobanteInicial, comprobanteFinal);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }



        #endregion

        #endregion

        #endregion

        #region Cuentas X Pagar

        #region Legalizacion:Caja Menor, Legalizacion Gastos, Leg Tarjetas

        /// <summary>
        /// Retorna el nombre del rerporte
        /// </summary>
        /// <param name="channel">Canal de trasmicion de datos</param>
        /// <param name="numeroDoc">nro del doc</param>
        /// <param name="prefijoID">Prefijo Doc</param>
        /// <param name="docNro">Doc consecutivo</param>
        /// <returns>URL del reporte</returns>
        public string Report_Cp_CajaMenor(int? numeroDoc, string prefijoID, int? docNro, bool isPreliminar)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cp_CajaMenor(this._userChannel, numeroDoc,prefijoID,docNro,isPreliminar);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Edades
        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaIni">fecha inicial del reporte</param>
        /// <param name="fechaFin">fecha final del reporte</param>
        /// <param name="tercero">Filtro de terceroID</param>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cp_PorEdadesDetallado(DateTime fechaIni, string terceroID, string cuentaID, bool isDetallada, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cp_PorEdadesDetallado(this._userChannel, fechaIni, terceroID, cuentaID, isDetallada, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaIni">fecha de corte  del reporte</param>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cp_PorEdadesResumido(DateTime fechaIni, string terceroID, string cuentaID, bool isDetallada, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cp_PorEdadesResumido(this._userChannel, fechaIni, terceroID, cuentaID, isDetallada, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Facturas
        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaIni">Fecha del periodo a consultar</param>
        /// <param name="tercero">Filtro del tercero</param>
        /// <returns>Nombre del reporte</returns>
        public string Reporte_Cp_FacturasXPagar(DateTime fechaIni, string Tercero, int Moneda, string Cuenta, bool isMultimoneda, ExportFormatType formatExport)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_FacturasXPagar(this._userChannel, fechaIni, Tercero, Moneda, Cuenta, isMultimoneda, formatExport);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaIni">fecha inicial del reporte</param>
        /// <param name="fechaFin">fecha final del reporte</param>
        /// <param name="tercero">Filtro de terceroID</param>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        public string Reporte_Cp_FacturasPagadas(DateTime fechaIni, DateTime fechaFin, string tercero, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reporte_Cp_FacturasPagadas(this._userChannel, fechaIni, fechaFin, tercero, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que se encarga de traer la lista con el nombre del reportes de Causacion y Factura equivalente
        /// </summary>
        /// <param name="numDoc">Identificador de las facturas a causar</param>
        /// <param name="isAprovada">Obtiene la informacion (true = Aprobada, Trae la info de coAuxilar; False = ParaAprobacion, Trae la info de coAuxilarPre</param>
        /// <param name="facEquivalente">Verifica si el tercero al que se le esta haciendo la Factura equivalente es Independiente (True = Genera Fac.Equivalente)</param>
        /// <param name="formatType">Tipo de formato de exportacion</param>
        /// <returns>Listado URL Con el nombre del reporte</returns>
        public string Reportes_Cp_CausacionFacturas(int numDoc, bool isAprobada, bool isNotaCredito, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reportes_Cp_CausacionFacturas(this._userChannel, numDoc, isAprobada,isNotaCredito, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que se encarga de traer el nombre del reporte
        /// </summary>
        /// <param name="numDoc">Identificador de las facturas a causar</param>
        /// <param name="impuestos">Listado de los impuestos a cobrar (se obtiene de glControl)</param>
        /// <param name="isAprovada">Obtiene la informacion (true = Aprobada, Trae la info de coAuxilar; False = ParaAprobacion, Trae la info de coAuxilarPre</param>
        /// <param name="facEquivalente">Verifica si el tercero al que se le esta haciendola Factura equivalente es Independiente (True = Genera Fac.Equivalente)</param>
        /// <param name="formatType">Tipo de formato de exportacion</param>
        /// <returns>URL Con el nombre del reporte</returns>
        public string Reportes_Cp_FacturaEquivalente(DateTime fecha, string tercero, bool facturaEquivalente, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reportes_Cp_FacturaEquivalente(this._userChannel, fecha, tercero, facturaEquivalente, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que obtiene le nombre del reporte
        /// </summary>
        /// <param name="tipoReport">Tipo de Reporte</param>
        /// <param name="periodoIni">PeriodoIni</param>
        /// <param name="periodoFin">PeriodoFin</param>
        /// <param name="cuentaID">Cuenta</param>
        /// <param name="bancoCuentaID">Banco</param>
        /// <param name="terceroID">Tercero</param>
        /// <param name="orden">Orden</param>
        /// <returns>nombre del reporte</returns>
        public string Reportes_Cp_CxPvsPagos(byte tipoReport, DateTime periodoIni, DateTime periodoFin, string cuentaID, string bancoCuentaID, string terceroID, byte orden)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reportes_Cp_CxPvsPagos(this._userChannel, tipoReport, periodoIni, periodoFin, cuentaID, bancoCuentaID, terceroID, orden);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }


        #endregion

        #region Flujos
        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cp_FlujoSemanalResumido(DateTime fechaCorte, string filtro, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cp_FlujoSemanalResumido(this._userChannel, fechaCorte, filtro, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cp_FlujoSemanalDetallado(DateTime fechaCorte, string filtro, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Cp_FlujoSemanalDetallado(this._userChannel, fechaCorte, filtro, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        public string ReportesCuentasXPagar_FlujoSemanalDetallado(List<DateTime> fechaIni, int Moneda, string Tercero, bool isDetallado, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesCuentasXPagar_FlujoSemanalDetallado(this._userChannel, fechaIni, Moneda, Tercero, isDetallado, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        #endregion

        #region Libro de Compras

        /// <summary>
        /// Funcion que se encarga del Nombre del reporte
        /// </summary>
        /// <param name="fecha">Fecha q se desea ver las compras</param>
        /// <param name="tercero">Tercero especifico q se desea ver</param>
        /// <param name="formatType">Tipo de formato para exportar el Reporte</param>
        /// <returns>URL del reporte</returns>             
        public string Reportes_Cp_LibroCompras(DateTime fecha, string tercero, bool facturaEquivalente, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reportes_Cp_LibroCompras(this._userChannel, fecha, tercero, facturaEquivalente, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Radicaciones
        /// <summary>
        /// Funcion que dvuelve le nombre del reporte generado
        /// </summary>
        /// <returns>nombre del reporte</returns>
        public string Reporte_Cp_Radicaciones(int yearIni, int yearFin, DateTime fechaIni, DateTime fechaFin, string Tercero, string Estado, string Orden, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reporte_Cp_Radicaiones(this._userChannel, yearIni, yearFin, fechaIni, fechaFin, Tercero, Estado, Orden, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Tarjetas
        public string ReportesCuentasXPagar_TarjetasPago(int numDoc, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_CxP_TarjetasPagas(this._userChannel, numDoc, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        public string ReportesCuentasXPagar_LegalizaTarjetas(int numDoc, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesCuentasXPagar_LegalizaTarjetas(this._userChannel, numDoc, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        #endregion

        #region Anticipos

        public string ReportesCuentasXPagar_Anticipos(DateTime Fecha, int Moneda, string Tercero, bool isDetallado, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesCuentasXPagar_Anticipos(this._userChannel, Fecha, Moneda, Tercero, isDetallado, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        public string ReportesCuentasXPagar_DocumentoAnticipo(int numDoc, bool isAprobada, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesCuentasXPagar_DocumentoAnticipo(this._userChannel, numDoc, isAprobada, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        public string ReportesCuentasXPagar_DocumentoAnticipoViaje(int numDoc, bool isAprobada, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesCuentasXPagar_DocumentoAnticipoViaje(this._userChannel, numDoc, isAprobada, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region EXCEL
        /// <summary>
        /// Obtiene un datatable con la info de CxP segun filtros
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="tercero">tercero</param>
        /// <param name="facturaNro">facturaNro</param>
        /// <param name="bancoCuenta">bancoCuenta</param>
        /// <param name="Agrupamiento">Agrupamiento</param>
        /// <param name="Romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable Reportes_Cp_CxPToExcel(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string tercero, string facturaNro,
                                                string cuentaID, string bancoCuentaID, string moneda, object otroFilter, byte? agrup, byte? romp)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reportes_Cp_CxPToExcel(this._userChannel, documentoID, tipoReporte, fechaIni, fechaFin, tercero, facturaNro,
                                                                         cuentaID, bancoCuentaID, moneda, otroFilter, agrup, romp);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene un datatable con la info de CxP segun filtros
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="tercero">tercero</param>
        /// <param name="facturaNro">facturaNro</param>
        /// <param name="bancoCuenta">bancoCuenta</param>
        /// <param name="Agrupamiento">Agrupamiento</param>
        /// <param name="Romp">Rompimiento</param>
        /// <returns>Datatable</returns>

        public DataTable Reportes_CC_CxCToExcel(int documentoID, byte? tipoReporte, DateTime fechaIni, DateTime? fechaFin, string tercero, bool isDetallada)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reportes_CC_CxCToExcel(this._userChannel, documentoID, tipoReporte, fechaIni, fechaFin, tercero, isDetallada);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }



        #endregion

        #endregion

        #region Facturacion

        /// <summary>
        /// Funcion q retorna el nombre del reporte
        /// </summary>
        /// <param name="numDoc">NUmero de Documento por el cual se va a filtrar la fatura</param>
        /// <param name="formatType">Tipo de exportacion del reporte</param>
        /// <returns>URL del reprote</returns>
        public string ReportesFacturacion_FacturaVenta(int documentID, string numDoc, bool isAprobada, ExportFormatType formatType,decimal valorAnticipo, decimal valorRteGarantia, decimal? porcRteGarantia, bool printAIUind = false)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesFacturacion_FacturaVenta(this._userChannel, documentID, numDoc, isAprobada, formatType,valorAnticipo,valorRteGarantia,porcRteGarantia, printAIUind);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Trae el nombre del reporte de facturas masivo
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="prefijo">Prefijo</param>
        /// <param name="docNroIni">nro Inicial</param>
        /// <param name="docNroIni">nro Inicial</param>
        /// <returns>URL del Reporte</returns>
        public string ReportesFacturacion_FacturaVentaMasivo(string prefijo, int docNroIni, int docnroFin)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesFacturacion_FacturaVentaMasivo(this._userChannel, prefijo, docNroIni, docnroFin);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        /// <summary>
        /// Funcion q retorna el nombre del reporte para cuentas por cobrar detallada
        /// </summary>
        /// <param name="fechaCorte">Fecha corte par la consutlta</param>
        /// <param name="tercero">tercero por el cual se va a filtrar</param>
        /// <param name="isDetallada">Revisa si la consulta es detalla o resumida (true) detallada (false) resumida</para
        /// <param name="formatType">Formato en que se exporta el reporte</param></param>
        /// <returns>URL del reporte</returns>
        public string ReportesFacturacion_CxCPorEdadesDetalladas(DateTime fechaCorte, string tercero, bool isDetallada, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesFacturacion_CxCPorEdadesDetalladas(this._userChannel, fechaCorte, tercero, isDetallada, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion q retorna el nombre del reporte para cuentas por cobrar resumida
        /// </summary>
        /// <param name="fechaCorte">Fecha corte par la consutlta</param>
        /// <param name="tercero">tercero por el cual se va a filtrar</param>
        /// <param name="isDetallada">Revisa si la consulta es detalla o resumida (true) detallada (false) resumida</para
        /// <param name="formatType">Formato en que se exporta el reporte</param></param>
        /// <returns>URL del reporte</returns>
        public string ReportesFacturacion_CxCPorEdadesResumida(DateTime fechaCorte, string tercero, bool isDetallada, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesFacturacion_CxCPorEdadesResumida(this._userChannel, fechaCorte, tercero, isDetallada, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        public string ReportesFacturacion_LibroVentas(DateTime periodo, int diaFinal, string cliente, string prefijo, string NroFactura, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesFacturacion_LibroVentas(this._userChannel, periodo, diaFinal, cliente, prefijo, NroFactura, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que obtiene el nombre del reporte
        /// </summary>
        /// <param name="fecha">Fecha a consultar</param>
        /// <param name="Tercero">Filtro por tercero </param>
        /// <param name="Moneda">Filtro de la Moneda de origen </param>
        /// <param name="Cuenta">Filtro de la cuenta</param>
        /// <param name="isMultimoneda">Indica si el reporte es para empresa Multimoneda</param>
        /// <param name="formatType">Tipo de formato para exportar</param>
        /// <returns></returns>
        public string Reporte_Cp_CuentasXCobrar(DateTime fechaIni, string Tercero, int Moneda, string Cuenta, bool isMultimoneda, ExportFormatType formatExport)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_CuentasXCobrar(this._userChannel, fechaIni, Tercero, Moneda, Cuenta, isMultimoneda, formatExport);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Global

        /// <summary>
        /// Funcion que se encarga de traer el nombre del reportes
        /// </summary>
        /// <param name="Periodo">Periodo a consultar los documentos Pendientes</param>
        /// <param name="modulo">Filtrar un modulo especifico</param>
        /// <param name="formatType">Tipo de formato a exportar el Reporte</param>
        /// <returns>URl del reportes</returns>
        public DTO_TxResult ReportesGlobal_DocumentosPendiente(DateTime Periodo,byte tipoReport, string modulo,string documentoID, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesGlobal_DocumentosPendiente(this._userChannel, Periodo,tipoReport, modulo,documentoID, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Inventarios

        #region Documentos

        /// <summary>
        /// Funcion que retorna el nombre del reporte de documentos
        /// </summary>
        /// <param name="mvto">mvto existente</param>
        /// <param name="documentID">Documento</param>
        /// <param name="numDoc">numero doc para consulta</param>
        /// <returns>Nombre del Reporte</returns>
        public string Reports_In_TransaccionMvto(DTO_MvtoInventarios mvto, int documentID, int numDoc, byte tipoMvto)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reports_In_TransaccionMvto(this._userChannel, mvto,documentID,numDoc,tipoMvto);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }


        #endregion


        #region Saldos


        public string ReportesInventarios_Saldos(int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, string Libro,
                string rompimiento, bool _parametro, string tipoReporte,ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesInventarios_Saldos(this._userChannel, año, mesIni, bodega, tipoBodega, referencia, grupo,
                    clase, tipo, serie, material, isSerial, Libro, rompimiento, _parametro, tipoReporte, formatType);

                return result;

         
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        #endregion

        #region Kardex
        /// <summary>
        /// Funcion que retorna el nombre del reporte de Saldos
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>
        /// <returns></returns>
        public string ReportesInventarios_KardexDetallado(int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesInventarios_KardexDetallado(this._userChannel, año, mesIni, bodega, tipoBodega, referencia, grupo,
                    clase, tipo, serie, material, isSerial, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte de Kardex sin parametros
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>
        /// <returns></returns>
        public string ReportesInventarios_KardexSinParametros(int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesInventarios_KardexSinParametros(this._userChannel, año, mesIni, bodega, tipoBodega, referencia, grupo,
                    clase, tipo, serie, material, isSerial, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte de Saldos con Kardex x referencia
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>
        /// <returns></returns>
        public string ReportesInventarios_KardexxReferencia(int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesInventarios_KardexxReferencia(this._userChannel, año, mesIni, bodega, tipoBodega, referencia, grupo,
                    clase, tipo, serie, material, isSerial, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte de Kardex sin parametros x reportes
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>
        /// <returns></returns>
        public string ReportesInventarios_KardexSinParametrosxReferencia(int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesInventarios_KardexSinParametrosxReferencia(this._userChannel, año, mesIni, bodega, tipoBodega, referencia,
                    grupo, clase, tipo, serie, material, isSerial, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        #endregion

        #region Serial

        /// <summary>
        /// Funcion que retorna el nombre del reportes serial
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>
        /// <returns></returns>
        public string ReportesInventarios_SerialxBodega(int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesInventarios_SerialxBodega(this._userChannel, año, mesIni, bodega, tipoBodega, referencia,
                    grupo, clase, tipo, serie, material, isSerial, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reportes serial
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>
        /// <returns></returns>
        public string ReportesInventarios_SerialxReferencia(int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesInventarios_SerialxReferencia(this._userChannel, año, mesIni, bodega, tipoBodega, referencia,
                    grupo, clase, tipo, serie, material, isSerial, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna el Nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes por el cual se va a filtrar</param>
        /// <param name="bodega">Bodega por la cual se desea filtrar</param>
        /// <param name="tipoBodega">Tipo de Bodega por e l cual se desea filtrar</param>
        /// <param name="referencia">Referencia por el cual se desea filtrar</param>
        /// <param name="grupo">Grupo por el cual se desea filtrar</param>
        /// <param name="clase">Clase por el cual se desea filtrar</param>
        /// <param name="tipo">Tipo por el cual se desea filtrar</param>
        /// <param name="serie">Serie por el cual se desea filtrar</param>
        /// <param name="material">Material por el cual se desea filtrar</param>
        /// <param name="isSerial">Parametro para imprimir reporte de Serial</param>
        /// <param name="formatType">Tipo de Formato en que se va a imprimir el reporte</param>
        /// <returns>Nombre del Reporte</returns>
        /// <returns></returns>
        public string ReportesInventarios_SerialxBodegaCosto(int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesInventarios_SerialxBodegaCosto(this._userChannel, año, mesIni, bodega, tipoBodega, referencia,
                    grupo, clase, tipo, serie, material, isSerial, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna el Nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes por el cual se va a filtrar</param>
        /// <param name="bodega">Bodega por la cual se desea filtrar</param>
        /// <param name="tipoBodega">Tipo de Bodega por e l cual se desea filtrar</param>
        /// <param name="referencia">Referencia por el cual se desea filtrar</param>
        /// <param name="grupo">Grupo por el cual se desea filtrar</param>
        /// <param name="clase">Clase por el cual se desea filtrar</param>
        /// <param name="tipo">Tipo por el cual se desea filtrar</param>
        /// <param name="serie">Serie por el cual se desea filtrar</param>
        /// <param name="material">Material por el cual se desea filtrar</param>
        /// <param name="isSerial">Parametro para imprimir reporte de Serial</param>
        /// <param name="formatType">Tipo de Formato en que se va a imprimir el reporte</param>
        /// <returns>Nombre del Reporte</returns>
        /// <returns></returns>
        public string ReportesInventarios_SerialxReferenciaCosto(int año, int mesIni, string bodega, string tipoBodega,
                   string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, string tipoReporte, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesInventarios_SerialxReferenciaCosto(this._userChannel, año, mesIni, bodega, tipoBodega, referencia,
                    grupo, clase, tipo, serie, material, isSerial,tipoReporte ,formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }


        #endregion

        #region EXCEL

        /// <summary>
        /// Obtiene un datatable con la info de Inventarios segun filtros
        /// </summary>
        /// <param name="documentoID">Tipo de Reporte a Generar</param>
        /// <param name="mesIni">Fecha Inicial</param>
        /// <param name="mesFin">Fecha Final</param>
        /// <param name="bodega">bodega</param>
        /// <param name="tipoBodega">tipoBodega</param>
        /// <param name="grupo">Grupo</param>
        /// <param name="clase">tipoBodega</param>
        /// <param name="Tipo">Tipo</param>
        /// <param name="serie">serie</param>
        /// <param name="material">material</param>
        /// <param name="isSerial">isSerial</param>
        /// <param name="otroFilter">otroFilter</param>
        /// <param name="agrup">agrupamiento</param>
        /// <param name="Romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable Reportes_In_InventarioToExcel(int documentID, DateTime? mesIni, DateTime? mesFin, string bodega, string tipoBodega, string referencia, string grupo, string clase, string tipo,
                                                        string serie, string material, bool isSerial, string libro,string proyectoID,string mvtoTipoID, object otroFilter, byte? agrup, byte? romp)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reportes_In_InventarioToExcel(this._userChannel, documentID, mesIni, mesFin, bodega, tipoBodega, referencia, grupo, clase, tipo,serie, material, isSerial, libro,proyectoID,mvtoTipoID, otroFilter, agrup, romp);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        /// <summary>
        /// Obtiene un datatable con la info de Inventarios segun filtros
        /// </summary>
        /// <param name="movimiento">Tipo  de movimiento</param>
        /// <param name="bodega">bodega</param>
        /// <param name="proyecto">proyecto</param>
        /// <param name="TipoReporte">report</param>
        /// <param name="mesIni">Fecha Inicial</param>
        /// <param name="detalle">detalle</param>
        public DataTable Reportes_In_DocumentoToExcel(string movimientoID, string bodegaID, string proyectoID, string tipoReporte, DateTime fechaIni, byte detalle)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reportes_In_DocumentoToExcel(this._userChannel, movimientoID, bodegaID, proyectoID, tipoReporte, fechaIni, detalle);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }


        #endregion

        #endregion

        #region Nomina

        /// <summary>
        /// Genera el reporte 
        /// </summary>
        /// <param name="documentoID">Documnto por el cual se consulta</param>
        /// <param name="periodo">Periodo del modulo</param>
        /// <param name="orden">Orden en el que se muestra el reporte</param>
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="isApro">Es para aprobar?</param>
        /// <returns></returns>
        public string Report_No_DetailLiquidaciones(int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool isOrderByName, bool isPre, ExportFormatType formatType, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_DetailLiquidaciones(this._userChannel, documentoID, periodo, orden, fechaini, fechaFin, isAll, isOrderByName, isPre, formatType, terceroid, operacionnoid, areafuncionalid, conceptonoid);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte 
        /// </summary>
        /// <param name="documentoID">Documnto por el cual se consulta</param>
        /// <param name="periodo">Periodo del modulo</param>
        /// <param name="orden">Orden en el que se muestra el reporte</param>
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="isApro">Es para aprobar?</param>
        /// <returns></returns>
        public string Report_No_XConcepto(int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool isOrerByName, bool isPre, ExportFormatType formatType, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_XConcepto(this._userChannel, documentoID, periodo, fechaini, fechaFin, orden, isAll, isOrerByName, isPre, formatType, terceroid, operacionnoid, areafuncionalid, conceptonoid);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte 
        /// </summary>
        /// <param name="documentoID">Documnto por el cual se consulta</param>
        /// <param name="periodo">Periodo del modulo</param>
        /// <param name="orden">Orden en el que se muestra el reporte</param>
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="isApro">Es para aprobar?</param>
        /// <returns></returns> 
        public string Report_No_Detalle(int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool isOrderByName, bool isPre, ExportFormatType formatType, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_Detalle(this._userChannel, documentoID, periodo, orden, fechaini, fechaFin, isAll, isOrderByName, isPre, formatType, terceroid, operacionnoid, areafuncionalid, conceptonoid);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte 
        /// </summary>
        /// <param name="documentoID">Documnto por el cual se consulta</param>
        /// <param name="periodo">Periodo del modulo</param>
        /// <param name="orden">Orden en el que se muestra el reporte</param>
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="isApro">Es para aprobar?</param>
        /// <returns></returns>
        public string Report_No_TotalXConcepto(int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool orderByName, bool isPre, ExportFormatType formatType, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_TotalXConcepto(this._userChannel, documentoID, periodo, orden, fechaini, fechaFin, isAll, orderByName, isPre, formatType, terceroid, operacionnoid, areafuncionalid, conceptonoid);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte 
        /// </summary>
        /// <param name="documentoID">Documnto por el cual se consulta</param>
        /// <param name="periodo">Periodo del modulo</param>
        /// <param name="orden">Orden en el que se muestra el reporte</param> 
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="isApro">Es para aprobar?</param>
        /// <returns></returns>
        public string Report_No_VacacionesPagadas(DateTime fechaIni, DateTime fechaFin, string empleadoFil, bool orderBy, ExportFormatType formatType, String empleadoid)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_VacacionesPagadas(this._userChannel, fechaIni, fechaFin, empleadoFil, orderBy, formatType, empleadoid);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte
        /// </summary>
        /// <param name="_documentoID"></param>
        /// <param name="_otroPorAlgo"></param>
        /// <param name="formatType"></param>
        /// <returns>Genera el reporte Vaciones Pendientes</returns>
        public string Report_No_VacacionesPendientes(string _empleadoID, int _vacaciones, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_VacacionesPendientes(this._userChannel, _empleadoID, _vacaciones, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Reporte Documento de Liquidación 
        /// </summary>
        /// <param name="_empleadoID"></param>
        /// <param name="_vacaciones"></param>
        /// <param name="formatType"></param>
        /// <returns>Reporte Documento de Liquidación </returns>
        public string Report_No_VacacionesDocumento(string _empleadoID, int _vacaciones, string fechaFiltro)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_VacacionesDocumento(this._userChannel, _empleadoID, _vacaciones, fechaFiltro);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Consulta por empleado
        /// </summary>
        /// <param name="_empleadoID"></param>
        /// <returns></returns>
        public List<DTO_ReportVacacionesDocumento> Report_No_GetVacacionesByEmpleado(string _empleadoID)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Report_No_GetVacacionesByEmpleado(this._userChannel, _empleadoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Consulta de por empleado para traer las fechas y utilizarlas como filtro.
        /// </summary>
        /// <param name="_empleadoID"></param>
        /// <returns></returns>
        public List<DTO_ReportVacacionesDocumento> Report_No_GetLiquidaContratoByEmpleado(string _empleadoID)
        {
            try
            {
                this.CreateService(typeof(INominaService));
                var result = this.NominaService.Report_No_GetLiquidaContratoByEmpleado(this._userChannel, _empleadoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(INominaService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte 
        /// </summary>
        /// <param name="documentoID">Documnto por el cual se consulta</param>
        /// <param name="periodo">Periodo del modulo</param>
        /// <param name="orden">Orden en el que se muestra el reporte</param>
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="isApro">Es para aprobar?</param>
        /// <returns></returns>
        public string Report_No_Prestamo(DateTime fechaIni, DateTime fechaFin, bool orderByName, ExportFormatType formatType, String empleadoid)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_Prestamo(this._userChannel, fechaIni, fechaFin, orderByName, formatType, empleadoid);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte 
        /// </summary>
        /// <param name="documentoID">Documnto por el cual se consulta</param>
        /// <param name="periodo">Periodo del modulo</param>
        /// <param name="orden">Orden en el que se muestra el reporte</param>  
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="isApro">Es para aprobar?</param>
        /// <returns></returns>
        public string Report_No_AportesPension(DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, ExportFormatType formatType, String terceroid, String nofondosaludid, String nocajaid)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_AportesPension(this._userChannel, fechaIni, fechaFin, filtro, orderByName, formatType, terceroid, nofondosaludid, nocajaid);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte 
        /// </summary>
        /// <param name="documentoID">Documnto por el cual se consulta</param>
        /// <param name="periodo">Periodo del modulo</param>
        /// <param name="orden">Orden en el que se muestra el reporte</param>
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="isApro">Es para aprobar?</param>
        /// <returns></returns>
        public string Report_No_AportesSalud(DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, ExportFormatType formatType, String terceroid, String nofondosaludid, String nocajaid)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_AportesSalud(this._userChannel, fechaIni, fechaFin, filtro, orderByName, formatType, terceroid, nofondosaludid, nocajaid);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte  
        /// </summary>
        /// <param name="documentoID">Documnto por el cual se consulta</param>
        /// <param name="periodo">Periodo del modulo</param>
        /// <param name="orden">Orden en el que se muestra el reporte</param>
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="isApro">Es para aprobar?</param>
        /// <returns></returns>
        public string Report_No_AporteVoluntarioPension(DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, ExportFormatType formatType, String terceroid, String nofondosaludid, String nocajaid)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_AporteVoluntarioPension(this._userChannel, fechaIni, fechaFin, filtro, orderByName, formatType, terceroid, nofondosaludid, nocajaid);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte 
        /// </summary>
        /// <param name="documentoID">Documnto por el cual se consulta</param>
        /// <param name="periodo">Periodo del modulo</param>
        /// <param name="orden">Orden en el que se muestra el reporte</param>
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="isApro">Es para aprobar?</param>
        /// <returns></returns>
        public string Report_No_AporteARP(DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, ExportFormatType formatType, String terceroid, String nofondosaludid, String nocajaid)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_AporteARP(_userChannel, fechaIni, fechaFin, filtro, orderByName, formatType, terceroid, nofondosaludid, nocajaid);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Reporte de CAja de compensacion
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="_terceroID"></param>
        /// <returns></returns>
        public string Report_No_AportesCajaCompensacion(DateTime fechaIni, DateTime fechaFin, String _terceroID, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_AportesCajaCompensacion(_userChannel, fechaIni, fechaFin, _terceroID, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Reporte de Gastos de Empresa
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="_terceroID"></param>
        /// <returns></returns>
        public string Report_No_GastosEmpresa(DateTime fechaIni, DateTime fechaFin, String _terceroID, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_GastosEmpresa(_userChannel, fechaIni, fechaFin, _terceroID, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="orderByName">Ordena por nombre?</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_No_BoletaPago(string empleadoID, int _mes, int _año, string _documentoNomina, string _quincena, ExportFormatType formatType, int? numDoc)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_BoletaPago(this._userChannel, empleadoID, _mes, _año, _documentoNomina, _quincena,numDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el nombre del reporte con la info de nomina segun filtros
        /// </summary>
        /// <param name="documentoID">Tipo de Reporte a Generar</param>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="empleadoID">Empleado</param>
        /// <param name="operacionNoID">Operacion Nomina</param>
        /// <param name="conceptoNoID">Concepto Nomina</param>
        /// <param name="areaFuncID">Area Funcional</param>
        /// <param name="fondoID">Fondo Nom</param>
        /// <param name="cajaID">Caja Nomina</param>
        /// <param name="otroFilter">Filtro adicional</param>
        /// <param name="agrup">Agrupar u ordenar</param>
        /// <param name="romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public string Report_No_NominaGetByParameter(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string empleadoID, string operacionNoID,
                                                     string conceptoNoID, string areaFuncID, string fondoID, string cajaID, string terceroID, object otroFilter, byte? agrup, byte? romp)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_No_NominaGetByParameter(this._userChannel, documentoID, tipoReporte, fechaIni, fechaFin, empleadoID, operacionNoID,
                                                                                 conceptoNoID, areaFuncID, fondoID, cajaID, terceroID, otroFilter, agrup, romp);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Operaciones Conjuntas

        /// <summary>
        /// Funcion que se encarga de traer los datos de cierre
        /// </summary>
        /// <param name="Periodo">Periodo que se va a consultar</param>
        /// <returns>Tabla con datos</returns>
        public DataTable ReportesOpereacionesConjuntas_Legalizaciones(DateTime Periodo)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesOperacionesConjuntas_Legalizaciones(this._userChannel, Periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Planeacion


        #region Cierre Legalizacion

        /// <summary>
        /// Funcion que se encarga de traer los datos de la sobre Ejecucion
        /// </summary>
        /// <param name="Periodo">Periodo que se desea verificar</param>
        /// <param name="contrato">Filtra un Contrato</param>
        /// <param name="bloque">Filtra un bloque</param>
        /// <param name="campo">Filtra un campo</param>
        /// <param name="pozo">Filtra un Pozo</param>
        /// <param name="proyecto">Filtra un Proyecto</param>
        /// <param name="actividad">Filtra una Actividad</param>
        /// <param name="lineaPresupuesto">Filtra una Linea Presupuesto</param>
        /// <param name="centroCosto">Filtra un Centro Costo</param>
        /// <param name="recurso">Filtra un recurso</param>        
        /// <returns>Tabla con resultados</returns>
        public DataTable ReportesPlaneacion_CierreLegalizacion(DateTime Periodo, string contrato, string bloque, string campo, string pozo, string proyecto, string actividad,
            string lineaPresupuesto, string centroCosto, string recurso)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesPlaneacion_CierreLegalizacion(this._userChannel, Periodo, contrato, bloque, campo, pozo, proyecto, actividad, lineaPresupuesto, centroCosto,
                    recurso);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Presupuesto

        #region NO BORRAR
        /* //Reportes Sin Consolidar
        /// <summary>
        /// Funcion q se encarga del nombre del reporte
        /// </summary>
        /// <param name="periodo">Perido que se desea ver</param>
        /// <param name="proyecto">Proyecto por el que se va a filtral</param>
        /// <param name="isLocal">Verifica la moneda con que se va imprimir el reporte (True: Imprime el Report en Moneda Loc, False: Imprime el Report en Moneda Ext).</param>
        /// <param name="formatType">Tipo de exportacion del reporte</param>
        /// <returns>URL del reporte</returns>
        public DTO_TxResult ReportesPlaneacion_PresupuestoSinConsolidar(DateTime periodo, string proyecto, bool isAcumulado, bool isLocal, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = new DTO_TxResult();

                if (isLocal)
                    result = this.ReportesService.ReportesPlaneacion_PresupuestoMLSinConsolidar(this._userChannel, periodo, proyecto, isAcumulado, formatType);
                else
                    result = this.ReportesService.ReportesPlaneacion_PresupuestoMESinConsolidar(this._userChannel, periodo, proyecto, isAcumulado, formatType);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        //Reportes Consolidados
        /// <summary>
        /// Funcion que se encarga del nombre del reporte Moneda Local
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="periodo">periodo q se desea ver</param>
        /// <param name="proyecto">Proyecto que se desea ver</param>
        /// <param name="formatType">Tipo de formato para exportar el reporte</param>
        /// <returns>URL Con el reporte</returns>
        public DTO_TxResult ReportesPlaneacion_PresupuestoConsolidados(DateTime periodo, string proyecto, bool isAcumulado, bool isLocal, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = new DTO_TxResult();

                if (isLocal)
                    result = this.ReportesService.ReportesPlaneacion_PresupuestoMLConsolidados(this._userChannel, periodo, proyecto, isAcumulado, formatType);
                else
                    result = this.ReportesService.ReportesPlaneacion_PresupuestoMEConsolidados(this._userChannel, periodo, proyecto, isAcumulado, formatType);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }*/

        #endregion

        //Reporte Acumulado
        /// <summary>
        /// Funcion que se encarga de generar el nombre del reporte
        /// </summary>
        /// <param name="periodo">Perido para la consulta</param>
        /// <param name="proyecto">Proyecto q se desea ver</param>
        /// <param name="isAcumulado">Verifica si es acumulado (True: Ejecula Procedimiento, False: Ejecuta Consulta)</param>
        /// <param name="formatType">Tipo de formato para exportar el reporte</param>
        /// <returns>URL</returns>
        public DataTable ReportesPlaneacion_PresupuestoAcumulado(DateTime periodo, string proyecto, bool isAcumulado, bool tipoMoneda, bool isConsololidado)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesPlaneacion_PresupuestoAcumulado(this._userChannel, periodo, proyecto, isAcumulado, tipoMoneda, isConsololidado);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Ejecucion Presupuestal

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Proyecto por Actividad
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo que se desea presupuestar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto que se desea ver (1 = Capex, 2 = Opex, 3 = Inversion, 4 = Administrativo, 5 = Inventarios, 6 = Capital de Trabajo, 7 = Distribucion) </param>
        /// <param name="TipoReporte">Tipo Reporte a mostrar (ProyAct = Proyecto x Actividad, LinRecur = Lineas x Recurso, RecurAct = Recurso x Actividad, LineCosto = Lineas x Centro Costo)</param>
        /// <param name="ProyectoID">Filtra un Proyecto  ------------------	(Se utiliza en el reporte de ProyAct)</param>
        /// <param name="ActividadID">Filtra una Actividad -----------------	(Se utiliza en el reporte de ProyAct y RecurAct)</param>
        /// <param name="LineaPresupuestalID">Filtra una Linea Presupuestal ---	(Se utiliza en el reporte de  LinRecur y LineCosto)	</param>
        /// <param name="CentroCostoID">Filtra un Centro de Costo --------- (Se utiliza en el reporte de LineCosto)</param>
        /// <param name="RecursoGrupoID">Filtra un Resurso Grupo -----------	(Se utiliza en el reporte de LinRecur y RecurAct)</param>
        /// <param name="isLocal">Moneda en que se va a imprimir (True: Moneda Local, False: Moneda Extranjera)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Listado de DTO con la ejecion presupuestal</returns>
        public DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalProyectoxActividad(DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, bool isLocal, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = new DTO_TxResult();

                if (isLocal)
                    result = this.ReportesService.ReportesPlaneacion_EjecucionPresupuestalProyectoxActividadML(this._userChannel, Periodo, ProyectoTipo, TipoReporte, ProyectoID,
                        ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID, formatType);
                else
                    result = this.ReportesService.ReportesPlaneacion_EjecucionPresupuestalProyectoxActividadME(this._userChannel, Periodo, ProyectoTipo, TipoReporte, ProyectoID,
                        ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID, formatType);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Linea por Centro de Costo Moneda Local
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo que se desea presupuestar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto que se desea ver (1 = Capex, 2 = Opex, 3 = Inversion, 4 = Administrativo, 5 = Inventarios, 6 = Capital de Trabajo, 7 = Distribucion) </param>
        /// <param name="TipoReporte">Tipo Reporte a mostrar (ProyAct = Proyecto x Actividad, LinRecur = Lineas x Recurso, RecurAct = Recurso x Actividad, LineCosto = Lineas x Centro Costo)</param>
        /// <param name="ProyectoID">Filtra un Proyecto  ------------------	(Se utiliza en el reporte de ProyAct)</param>
        /// <param name="ActividadID">Filtra una Actividad -----------------	(Se utiliza en el reporte de ProyAct y RecurAct)</param>
        /// <param name="LineaPresupuestalID">Filtra una Linea Presupuestal ---	(Se utiliza en el reporte de  LinRecur y LineCosto)	</param>
        /// <param name="CentroCostoID">Filtra un Centro de Costo --------- (Se utiliza en el reporte de LineCosto)</param>
        /// <param name="RecursoGrupoID">Filtra un Resurso Grupo -----------	(Se utiliza en el reporte de LinRecur y RecurAct)</param>
        /// <param name="isLocal">Moneda en que se va a imprimir (True: Moneda Local, False: Moneda Extranjera)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Listado de DTO con la ejecion presupuestal</returns>
        public DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalLineaXCentroCto(DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, bool isLocal, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = new DTO_TxResult();

                if (isLocal)
                    result = this.ReportesService.ReportesPlaneacion_EjecucionPresupuestalLineaXCentroCtoML(this._userChannel, Periodo, ProyectoTipo, TipoReporte, ProyectoID,
                        ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID, formatType);
                else
                    result = this.ReportesService.ReportesPlaneacion_EjecucionPresupuestalLineaXCentroCtoME(this._userChannel, Periodo, ProyectoTipo, TipoReporte, ProyectoID,
                        ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID, formatType);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Linea por Recurso Moneda Local
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo que se desea presupuestar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto que se desea ver (1 = Capex, 2 = Opex, 3 = Inversion, 4 = Administrativo, 5 = Inventarios, 6 = Capital de Trabajo, 7 = Distribucion) </param>
        /// <param name="TipoReporte">Tipo Reporte a mostrar (ProyAct = Proyecto x Actividad, LinRecur = Lineas x Recurso, RecurAct = Recurso x Actividad, LineCosto = Lineas x Centro Costo)</param>
        /// <param name="ProyectoID">Filtra un Proyecto  ------------------	(Se utiliza en el reporte de ProyAct)</param>
        /// <param name="ActividadID">Filtra una Actividad -----------------	(Se utiliza en el reporte de ProyAct y RecurAct)</param>
        /// <param name="LineaPresupuestalID">Filtra una Linea Presupuestal ---	(Se utiliza en el reporte de  LinRecur y LineCosto)	</param>
        /// <param name="CentroCostoID">Filtra un Centro de Costo --------- (Se utiliza en el reporte de LineCosto)</param>
        /// <param name="RecursoGrupoID">Filtra un Resurso Grupo -----------	(Se utiliza en el reporte de LinRecur y RecurAct)</param>
        /// <param name="isLocal">Moneda en que se va a imprimir (True: Moneda Local, False: Moneda Extranjera)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Listado de DTO con la ejecion presupuestal</returns>
        public DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalLineaXRecurso(DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, bool isLocal, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = new DTO_TxResult();

                if (isLocal)
                    result = this.ReportesService.ReportesPlaneacion_EjecucionPresupuestalLineaXRecursoML(this._userChannel, Periodo, ProyectoTipo, TipoReporte, ProyectoID,
                        ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID, formatType);
                else
                    result = this.ReportesService.ReportesPlaneacion_EjecucionPresupuestalLineaXRecursoME(this._userChannel, Periodo, ProyectoTipo, TipoReporte, ProyectoID,
                        ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID, formatType);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Recurso por Actividad Moneda Local
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo que se desea presupuestar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto que se desea ver (1 = Capex, 2 = Opex, 3 = Inversion, 4 = Administrativo, 5 = Inventarios, 6 = Capital de Trabajo, 7 = Distribucion) </param>
        /// <param name="TipoReporte">Tipo Reporte a mostrar (ProyAct = Proyecto x Actividad, LinRecur = Lineas x Recurso, RecurAct = Recurso x Actividad, LineCosto = Lineas x Centro Costo)</param>
        /// <param name="ProyectoID">Filtra un Proyecto  ------------------	(Se utiliza en el reporte de ProyAct)</param>
        /// <param name="ActividadID">Filtra una Actividad -----------------	(Se utiliza en el reporte de ProyAct y RecurAct)</param>
        /// <param name="LineaPresupuestalID">Filtra una Linea Presupuestal ---	(Se utiliza en el reporte de  LinRecur y LineCosto)	</param>
        /// <param name="CentroCostoID">Filtra un Centro de Costo --------- (Se utiliza en el reporte de LineCosto)</param>
        /// <param name="RecursoGrupoID">Filtra un Resurso Grupo -----------	(Se utiliza en el reporte de LinRecur y RecurAct)</param>
        /// <param name="isLocal">Moneda en que se va a imprimir (True: Moneda Local, False: Moneda Extranjera)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Listado de DTO con la ejecion presupuestal</returns>
        public DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalRecursoXActividad(DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, bool isLocal, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = new DTO_TxResult();

                if (isLocal)
                    result = this.ReportesService.ReportesPlaneacion_EjecucionPresupuestalRecursoXActividadML(this._userChannel, Periodo, ProyectoTipo, TipoReporte, ProyectoID,
                        ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID, formatType);
                else
                    result = this.ReportesService.ReportesPlaneacion_EjecucionPresupuestalRecursoXActividadME(this._userChannel, Periodo, ProyectoTipo, TipoReporte, ProyectoID,
                        ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID, formatType);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que se encarga de traer los datos para generar el reportes de Ejecucion Presupuestal por Origen
        /// </summary>
        /// <param name="Periodo">Periodo a consultar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto a consultar</param>
        /// <returns>Tabla con la ejecucion presupuestal</returns>
        public DataTable ReportesPlaneacion_EjecucionPresupuestalXOrigen(DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesPlaneacion_EjecucionPresupuestalXOrigen(this._userChannel, Periodo, ProyectoTipo, TipoReporte, ProyectoID, ActividadID,
                    LineaPresupuestalID, CentroCostoID, RecursoGrupoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region SobreEjecucion

        /// <summary>
        /// Funcion que se encarga de traer los datos de la sobre Ejecucion
        /// </summary>
        /// <param name="contrato">Filtra un Contrato</param>
        /// <param name="bloque">Filtra un bloque</param>
        /// <param name="campo">Filtra un campo</param>
        /// <param name="pozo">Filtra un Pozo</param>
        /// <param name="proyecto">Filtra un Proyecto</param>
        /// <param name="actividad">Filtra una Actividad</param>
        /// <param name="lineaPresupuesto">Filtra una Linea Presupuesto</param>
        /// <param name="centroCosto">Filtra un Centro Costo</param>
        /// <param name="recurso">Filtra un recurso</param>
        /// <param name="usuario">Filtra un usuario</param>
        /// <param name="prefijo">Filtra un prefijo </param>
        /// <param name="numeroDoc">Filtra un numero de Documento Especifico</param>
        /// <returns>Tabla con resultados</returns>
        public DataTable ReportesPlaneacion_SobreEjecucion(int year, string contrato, string bloque, string campo, string pozo, string proyecto, string actividad,
            string lineaPresupuesto, string centroCosto, string recurso, string usuario, string prefijo, string numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesPlaneacion_SobreEjecucion(this._userChannel, year, contrato, bloque, campo, pozo, proyecto, actividad, lineaPresupuesto, centroCosto,
                    recurso, usuario, prefijo, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Proveedores

        #region Compromisos VS Facturas

        /// <summary>
        /// Funcion que se encarga de  traer los compromisos contra las facturas
        /// </summary>
        /// <param name="FechaInicial">Fecha de consulta inicial</param>
        /// <param name="FechaFinal">Fecha de consulta final</param>
        /// <param name="proveedor">Filtra un proveedor en especifico</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns>Listado de DTO</returns>
        public DTO_TxResult ReportesProveedores_CompromisosVSFacturas(DateTime FechaInicial, DateTime FechaFinal, string proveedor, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesProveedores_CompromisosVSFacturas(this._userChannel, FechaInicial, FechaFinal, proveedor, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que trae la trazabilidad de los proveedores
        /// </summary>
        /// <param name="FechaInicial">Fecha de consulta inicial</param>
        /// <param name="FechaFinal">Fecha de consulta final</param>
        /// <param name="resumidoInd">valida si es resumido o detallado</param>
        ///  <param name="proveedor">Filtra un proveedor en especifico</param>
        ///  <param name="proyectoID">Filtra un proyecto en especifico</param>
        /// <returns>Listado de DTO</returns>
        public string Report_ProveedoresTrazabilizad(DateTime fechaIni, DateTime? fechaFin, bool resumidoInd, bool pendientesInd, string proveedorID, string proyectoID)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_ProveedoresTrazabilizad(this._userChannel, fechaIni, fechaFin, resumidoInd,pendientesInd,proveedorID, proyectoID);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }


        #endregion

        #region Orden Compras

        /// <summary>
        /// Funcion que se encarda de traer los datos de orden de compras
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>        /// <param name="FechaIni">Fecha incial que se desea ver los datos</param>
        /// <param name="FechaFin">Fecha Final hasta donde quiere verificar los datos</param>
        /// <param name="Proveedor">Filtra un proveedor en especifico</param>
        /// <param name="Estado">Filtra el estado de la orden de compra</param>
        /// <param name="isDetallado">Verifica que tipo de reporte mostrar (True: Muestra Orden Compra Detallada, False: Muestra Orden Compra Resumida)</param>
        /// <param name="formatType">Tipo de formato para exportar el reporte</param>
        /// <returns>Listado de DTO</returns>
        public DTO_TxResult ReportesProveedores_OrdenCompras(DateTime FechaIni, DateTime FechaFin, string Proveedor, Dictionary<int, string> filtros, bool isDetallado, string Moneda, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesProveedores_OrdenCompras(this._userChannel, FechaIni, FechaFin, Proveedor, filtros, isDetallado, Moneda, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que se encarda de traer los datos de orden de compras
        /// </summary>
        /// <param name="FechaIni">Fecha incial que se desea ver los datos</param>
        /// <param name="FechaFin">Fecha Final hasta donde quiere verificar los datos</param>
        /// <param name="Proveedor">Filtra un proveedor en especifico</param>
        /// <param name="Estado">Filtra el estado de la orden de compra</param>
        /// <param name="isDetallado">Verifica que tipo de reporte mostrar (True: Muestra Orden Compra Detallada, False: Muestra Orden Compra Resumida)</param>
        /// <param name="formatType">Tipo de formato para exportar el reporte</param>
        /// <returns>Listado de DTO</returns>
        public DTO_TxResult ReportesProveedores_OrdenComprasDetallada(DateTime FechaIni, DateTime FechaFin, string Proveedor, Dictionary<int, string> filtros, bool isDetallado, string Moneda, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesProveedores_OrdenComprasDetallada(this._userChannel, FechaIni, FechaFin, Proveedor, filtros, isDetallado, Moneda, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }


        #endregion

        #region Solicitudes
        public string ReportesProveedores_Solicitudes(Dictionary<int, string> filtros, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesProveedores_Solicitudes(this._userChannel, filtros, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte de un documento de Solicitud o Recibido
        /// </summary>
        /// <param name="documentoID">Id del Reporte</param>
        /// <param name="numeroDoc">numero del Doc</param>
        /// <param name="isPreliminar">si es para aprobacion</param>
        /// <param name="tipoReporte">Tipo de reporte</param>
        /// <returns></returns>
        public string ReportesProveedores_SolicitudOrRecibidoDoc(int documentoID, int numeroDoc, bool isPreliminar, byte tipoReporte)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesProveedores_SolicitudOrRecibidoDoc(this._userChannel, documentoID, numeroDoc, isPreliminar,tipoReporte);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }
        #endregion

        #region OrdenCompra

        /// <summary>
        /// Funcion q se encarga de traer el nombre del reporte
        /// </summary>
        /// <param name="numDoc">Identificador de las facturas a Pagar</param>
        /// <param name="exportType">Tipo de exportacion del reporte</param>
        /// <returns>Nombre del reporte</returns>
        public string ReportesProveedores_OrdenCompra(int numDoc, byte tipoReporte, bool showReport, bool isPreliminar = false)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesProveedores_OrdenCompra(this._userChannel, numDoc, tipoReporte, showReport, isPreliminar);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Recibidos

        /// <summary>
        /// Funcion que se encarga de traer las ordenes recibidas resumido
        /// </summary>
        /// <param name="Periodo">Periodo que se desea consultar</param>
        /// <param name="proveedor">Filtrar un Proveedor especifico</param>
        /// <param name="isDetallado">Verifica que tipo de reporte mostrar (True: Muestra Orden Compra Detallada, False: Muestra Orden Compra Resumida)</param>
        /// <param name="exportType">Formato de Ezportacion del reporte</param>
        /// <returns></returns>
        public DTO_TxResult ReportesProveedores_Recibidos(DateTime Periodo, string proveedor, bool isDetallado, Dictionary<int, string> filtros,bool isFacturdo, ExportFormatType exportType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));

                DTO_TxResult result = new DTO_TxResult();

                //Carga el servicio del reporte recibido resumido
                if (!isDetallado)
                    result = this.ReportesService.ReportesProveedores_Recibidos(this._userChannel, Periodo, proveedor, isDetallado,filtros, isFacturdo, exportType);

                //Carga el servicio del reporte recibido Detallado
                else
                    result = this.ReportesService.ReportesProveedores_RecibidosDetallado(this._userChannel, Periodo, proveedor, isDetallado, filtros,isFacturdo, exportType);

                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Excel
        /// <summary>
        /// Funcion que se encarga de traer los datos para generar reportes de procedimientos de compra en excel
        /// </summary>
        /// <param name="Periodo">Periodo a consultar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto a consultar</param>
        /// <returns>Tabla con la ejecucion presupuestal</returns>
        public DataTable ReportesProveedores_ProcedimientoComprasXLS(Dictionary<int, string> filtros, DateTime fechaIni, DateTime fechaFinal)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesProveedores_ProcedimientoComprasXLS(this._userChannel, filtros,fechaIni, fechaFinal);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

 
        #endregion 
        #endregion

        #region Proyectos

        /// <summary>
        /// Funcion q se encarga de traer el cumplimiento del proyecto
        /// </summary>
        /// <param name="FechaCorte">Fecha de Corte</param>
        /// <param name="Proyecto">Filtra un Proyecto Especifico</param>
        /// <param name="Estado">Filtra un Estado Especifico</param>
        /// <param name="LineaFlujo">Filtra un LineaFlujo Especifico</param>
        /// <param name="Etapa">Filtra un Etapa Especifico</param>
        /// <returns></returns>
        public DataTable ReportesProyectos_Cumplimiento(DateTime FechaCorte, string Proyecto, string Estado, string LineaFlujo, string Etapa)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesProyectos_Cumplimiento(this._userChannel, FechaCorte, Proyecto, Estado, LineaFlujo, Etapa);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

       /// <summary>
        /// Funcion que se encarga de traer la informacion del Presupuesto que se requiere para cada proyecto
       /// </summary>
       /// <param name="tipoReporte">Tipo de reporte</param>
       /// <param name="proyecto">Proyecto</param>
       /// <param name="centroCto">Centro Cto</param>
       /// <param name="cliente">Cliente</param>
       /// <param name="prefijo">Prefijo</param>
       /// <param name="docNro">nro del doc</param>
       /// <returns>nombre del reporte</returns>
        public string ReportesProyectos_EjecPresupuesto(byte tipoReporte, string proyecto, string centroCto, string cliente, string prefijo, int? docNro)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesProyectos_EjecPresupuesto(this._userChannel, tipoReporte, proyecto,centroCto,cliente, prefijo,docNro);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion para crear el reporte de Planeacion costos
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="agrupam">agrupamiento</param>
        /// <param name="mesIni">Mes Inicial</param>
        /// <param name="mesFin">Mes Fin</param>
        /// <returns>nombre del reporte</returns>
        public string Reportes_py_PlaneacionCostos(DTO_SolicitudTrabajo solicitud, bool useMultiplicadorInd, byte agrupam, DateTime? mesIni, DateTime? mesFin)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reportes_py_PlaneacionCostos(this._userChannel, solicitud, useMultiplicadorInd, agrupam, mesIni, mesFin);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion para crear el reporte de Planeacion costos
        /// </summary>

        /// <param name="periodo">Periodo</param>
        /// <returns>nombre del reporte</returns>
        public string Reportes_py_Indicadores(DateTime?periodo)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reportes_py_Indicadores(this._userChannel,periodo);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion para crear el reporte de Actas TRabajo
        /// </summary>
        /// <param name="solicitud">Datos</param>
        /// <param name="actaNroActual">acta actual</param>
        /// <returns>nombre del reporte</returns>
        public string Reportes_py_ActaTrabajo(DTO_SolicitudTrabajo solicitud, int actaNroActual, DateTime fechaActa)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reportes_py_ActaTrabajo(this._userChannel, solicitud, actaNroActual, fechaActa);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion para crear el reporte de Actas
        /// </summary>
        /// <param name="solicitud">Datos</param>
        /// <param name="actaNroActual">Acta actual</param>
        /// <returns>nombre del reporte</returns>
        public string Reportes_py_ActaEntrega(DTO_SolicitudTrabajo solicitud, int actaNroActual,DateTime fechaActa)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reportes_py_ActaEntrega(this._userChannel, solicitud, actaNroActual,fechaActa);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion para crear el reporte de resumen Documentos de COtizacion
        /// </summary>
        /// <param name="mesIni">MEs inicial</param>
        /// <param name="mesFin">MEs Final</param>
        /// <returns>nombre del reporte</returns>
        public string Reportes_pyResumenDocsCotiz(DateTime mesIni, DateTime mesFin)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reportes_pyResumenDocsCotiz(this._userChannel, mesIni, mesFin);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #region Tesoreria

        /// <summary>
        /// Funcion que devuelve el nombre del reporte
        /// </summary>
        /// <param name="data">Data cargada</param>
        /// <returns></returns>
        public string Report_Ts_ChequesGirados(List<DTO_ChequesGirados> data)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Ts_ChequesGirados(this._userChannel, data);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="bancoID">Banco ID</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="orden">Orden para mostrar en el reporte</param>
        /// <param name="nombreBen">Nombredel beneficiario para filtrar</param>
        /// <returns>Nombre del reporte</returns>
        public string Reporte_Ts_ChequesGiradosRep(string bancoID, string terceroID, DateTime fechaIni, DateTime fechaFin, string orden, bool? nombreBen)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Ts_ChequesGiradosRep(this._userChannel, bancoID, terceroID, fechaIni, fechaFin, orden, nombreBen);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que llama al servicio para generar el reporte
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha fin del reporte</param>
        /// <param name="nit">filtro</param>
        /// <param name="caja">filtro</param>
        /// <returns></returns>
        public string Reporte_Ts_RecibosDeCaja(DateTime fechaIni, DateTime fechaFin, string nit, string caja)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Ts_RecibosDeCaja(this._userChannel, fechaIni, fechaFin, nit, caja);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="documentID">Documento</param>
        /// <param name="numeroDoc">Identificador del Doc</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Ts_ReciboCajaDoc(int documentID, int numeroDoc)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Ts_ReciboCajaDoc(this._userChannel, documentID, numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="formatType">Tipo Formato</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Ts_LibroDeBancos(DateTime fechaIni, DateTime fechaFin, string filtro, ExportFormatType formatType)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Ts_LibroDeBancos(this._userChannel, fechaIni, fechaFin, filtro, formatType);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="formatType">Tipo Formato</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Ts_RelacionPagos(DateTime fechaIni, DateTime fechaFin, string bancoID, string nit, string numCheque, ExportFormatType exportype)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Ts_RelacionPagos(this._userChannel, fechaIni, fechaFin, bancoID, nit, numCheque, exportype);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Genera el reporte 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="formatType">Tipo Formato</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Ts_RelacionPagosXBancos(DateTime fechaIni, DateTime fechaFin, string bancoID, string nit, string numCheque, ExportFormatType exportype)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Report_Ts_RelacionPagosXBancos(this._userChannel, fechaIni, fechaFin, bancoID, nit, numCheque, exportype);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        /// <summary>
        /// Funcion q se encarga de traer el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="numDoc">Identificador de las facturas a Pagar</param>
        /// <param name="exportType">Tipo de exportacion del reporte</param>
        /// <returns>Nombre del reporte</returns>
        public string ReportesTesoreria_PagosFactura(int documentID,int numDoc, ExportFormatType exportType, bool isTransferencia = false)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.ReportesTesoreria_PagosFactura(this._userChannel,documentID, numDoc, exportType, isTransferencia);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #region EXCEL

        /// <summary>
        /// Obtiene un datatable con la info de Tesoreria segun filtros
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="tercero">tercero</param>
        ///  <param name="chequeNro">cheque Nro</param>
        /// <param name="facturaNro">facturaNro</param>
        /// <param name="bancoCuenta">bancoCuenta</param>
        /// <param name="Agrupamiento">Agrupamiento</param>
        /// <param name="Romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable Reportes_Ts_TesoreriaToExcel(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string tercero, string chequeNro,
                                                 string facturaNro, string bancoCuentaID, byte? agrup, byte? romp)
        {
            try
            {
                this.CreateService(typeof(IReportesService));
                var result = this.ReportesService.Reportes_Ts_TesoreriaToExcel(this._userChannel, documentoID, tipoReporte, fechaIni, fechaFin, tercero, chequeNro, facturaNro, bancoCuentaID, agrup, romp);
                return result;
            }
            catch (Exception ex)
            {
                this.AbortService(typeof(IReportesService));
                throw ex;
            }
        }

        #endregion

        #endregion

        #endregion

        #endregion
    }
}
