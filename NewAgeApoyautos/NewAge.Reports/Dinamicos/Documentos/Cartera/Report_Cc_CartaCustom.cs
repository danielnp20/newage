using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.Librerias.ExceptionHandler;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;

namespace NewAge.Reports.Dinamicos
{
    public partial class Report_Cc_CartaCustom : XtraReport
    {
        
        #region Variables
         protected ModuloGlobal _moduloGlobal;

        protected SqlConnection _connection;
        protected SqlTransaction _transaction;
        protected DTO_glEmpresa _empresa;
        protected int _userID;
        protected ModuloBase _moduloBase;
        protected ExportFormatType _formatType;

        protected int? numeroDoc;
        protected string loggerConnectionStr;
        protected ReportProvider reportProvider;
       
        #endregion

        #region Propiedades

        /// <summary>
        /// Empresa
        /// </summary>
        internal DTO_glEmpresa Empresa
        {
            get { return this._empresa; }
            set { this._empresa = value; }
        }

        /// <summary>
        /// Nombre del Reporte
        /// </summary>
        public string ReportName
        {
            get;
            set;
        }

        /// <summary>
        /// Ruta del reporte
        /// </summary>
        public string Path
        {
            get;
            set;
        }

        #endregion        

        #region Constructor

        public Report_Cc_CartaCustom()
        {
          //  this.InitializeComponent();
        }

        public Report_Cc_CartaCustom(DTO_glEmpresa emp)
        {
            this.InitializeComponent();
            this._empresa = emp;
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="conn">conexion a base datos</param>
        /// <param name="tx">transaccion</param>
        /// <param name="emp">empresa</param>
        /// <param name="userID">identificador del usuario</param>
        public Report_Cc_CartaCustom(string loggerConn, SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, ExportFormatType formatType, int? numDoc = null)
        {
            InitializeComponent();

            this._connection = conn;
            this._transaction = tx;
            this._empresa = emp;
            this._userID = userID;
            this._formatType = formatType;
            this.loggerConnectionStr = loggerConn;

            this.numeroDoc = numDoc;
            this.SetInitParameters();
        }

        #endregion

        /// <summary>
        /// Inicializa objetos y parametros iniciales
        /// </summary>
        protected virtual void SetInitParameters()
        {
            XtraReport report = new XtraReport();
            this._moduloGlobal = new ModuloGlobal(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this._moduloGlobal.Empresa = this.Empresa;
            this._moduloBase = new ModuloBase(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            DTO_seUsuario usuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this._userID);

            #region Nombre del reporte

            string repName;
            string filesPath = this._moduloGlobal.GetControlValue(AppControl.RutaFisicaArchivos);
            string docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            string ext = this.GetExtention();

            if (this.numeroDoc.HasValue)
            {
                // Reporte de documento
                string fileFormat = this._moduloGlobal.GetControlValue(AppControl.NombreArchivoDocumentos);
                repName = string.Format(fileFormat, this.numeroDoc.ToString());
                docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaDocumentos);
            }
            else
            {
                // Reporte temporal
                repName = Guid.NewGuid().ToString();
                docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            }

            this.ReportName = repName.ToString() + ext;
            this.Path = filesPath + docsPath + ReportName;

            #endregion            
            #region Recusos
          
            this.reportProvider = new ReportProvider(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this.reportProvider.LoadResources(this.AllControls<XRControl>());

            #endregion
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int document, object datos, Dictionary<string, string> adicionales)
        {
            try
            {
                List<DTO_SolicitudLibranza> source = new List<DTO_SolicitudLibranza>();
                DTO_SolicitudLibranza first = new DTO_SolicitudLibranza();

                string filesPath = this._moduloGlobal.GetControlValue(AppControl.RutaFisicaArchivos);
                string docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaPlantillas);
                string pathTemplate = filesPath + docsPath + document + ".rtf";
                try { first.PlantillaCarta = System.IO.File.ReadAllText(pathTemplate); } catch (Exception ex) { first.PlantillaCarta = ex.Message; }
                string fechaActualCarta = DateTime.Now.ToString("dd") + " de " + DateTime.Now.ToString("MMMM") + " de " + DateTime.Now.Year.ToString();

                if (datos.GetType() == typeof(DTO_DigitaSolicitudDecisor))
                {
                    DTO_DigitaSolicitudDecisor solicitud = (DTO_DigitaSolicitudDecisor)datos;
                    DTO_ccConcesionario concesionario = (DTO_ccConcesionario)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccConcesionario, solicitud.SolicituDocu.ConcesionarioID.Value, true, false);
                    string nombreCli = solicitud.SolicituDocu.NombrePri.Value + " " + solicitud.SolicituDocu.NombreSdo.Value + " " + solicitud.SolicituDocu.ApellidoPri.Value + " " + solicitud.SolicituDocu.ApellidoSdo.Value;

                        first.PlantillaCarta = first.PlantillaCarta.Replace("%FECHA%", fechaActualCarta);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%FECHACORTE%", adicionales.ContainsKey("%FECHACORTE%") ? adicionales["%FECHACORTE%"] : fechaActualCarta);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%NOMBRES%", nombreCli);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CEDULA%", Convert.ToInt32(solicitud.SolicituDocu.ClienteRadica.Value).ToString("n0"));
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%VITRINA%", concesionario != null ? concesionario.Descriptivo.Value : solicitud.SolicituDocu.ConcesionarioID.Value);

                    if (solicitud.DatosVehiculo != null)
                    {
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%MARCA%", solicitud.DatosVehiculo.Marca.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%REFERENCIA%", solicitud.DatosVehiculo.Referencia.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%MODELO%", solicitud.DatosVehiculo.Modelo.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%SERVICIO%", solicitud.DatosVehiculo.Servicio.Value == 1 ? "PARTICULAR" : "PÚBLICO");
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%PRECIOVENTA%", solicitud.DatosVehiculo.PrecioVenta.Value.HasValue ? solicitud.DatosVehiculo.PrecioVenta.Value.Value.ToString("n0") : "0");
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%NROPRENDA%", solicitud.DatosVehiculo.NumeroPrenda.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%PLACAS%", solicitud.DatosVehiculo.Placa.Value);
                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%MOTOR%", solicitud.DatosVehiculo.Modelo.Value);
                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%SERIE%", solicitud.DatosVehiculo.Servicio.Value);
                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%TIPO%", solicitud.DatosVehiculo.Tipocaja.Value);
                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%COLOR%", solicitud.DatosVehiculo..Value == 1 ? "Particular" : "Público");

                        if(!string.IsNullOrEmpty(solicitud.DatosVehiculo.FasecoldaCod.Value))
                        {
                            DTO_ccFasecolda fase = (DTO_ccFasecolda)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccFasecolda, solicitud.DatosVehiculo.FasecoldaCod.Value, true, false);
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%LINEA%", fase.Tipo1.Value);
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%CLASE%", fase.Clase.Value);
                        }
                    }
                    if (document == AppReports.drCartaEnvioPrenda)
                    {
                        DTO_glLugarGeografico ciu = (DTO_glLugarGeografico)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLugarGeografico, solicitud.SolicituDocu.Ciudad.Value, true, false);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CIUDAD%", ciu != null ? (ciu.Descriptivo.Value) : solicitud.SolicituDocu.Ciudad.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%FECHA%", fechaActualCarta);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%PREFIJOPRENDA%", solicitud.DatosVehiculo.PrefijoPrenda.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%PRENDA%", solicitud.DatosVehiculo.NumeroPrenda.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%NOMBRES%", nombreCli);
                    }
                    else if (document == AppReports.drRevocacionAprobacion)
                    {
                       
                    }
                    else if (document == AppReports.drTrasladoCuenta)
                    {
                    }
                    else if (document == AppReports.drVencimientoTerminos)
                    {
                    }
                    else if (document == AppReports.drAsegurabilidad)
                    {
                        DTO_glLugarGeografico ciu = (DTO_glLugarGeografico)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLugarGeografico, solicitud.SolicituDocu.Ciudad.Value, true, false);
                        string diaActual = DateTime.Now.ToString("dd") ;
                        string mesActual = DateTime.Now.ToString("MMMM") ;
                        string yearActual = DateTime.Now.Year.ToString();

                        first.PlantillaCarta = first.PlantillaCarta.Replace("%DIA%",diaActual);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%MES%", mesActual);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%ANNO%", yearActual);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CIUDAD%", ciu != null ? (ciu.Descriptivo.Value) : solicitud.SolicituDocu.Ciudad.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%NOMBRE%", nombreCli);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CEDULA%", solicitud.SolicituDocu.ClienteID.Value);
                        
                            //DTO_coTercero ter = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor1.Value, true, false);
                            //first.PlantillaCarta = first.PlantillaCarta.Replace("%DIA%", DateTime.Now.ToString("dd"));
                            //first.PlantillaCarta = first.PlantillaCarta.Replace("%MES%", DateTime.Now.Month.ToString());
                            //first.PlantillaCarta = first.PlantillaCarta.Replace("%ANNO%", DateTime.Now.Year.ToString());
                            //first.PlantillaCarta = first.PlantillaCarta.Replace("%CIUDAD%", ciu != null ? (ciu.Descriptivo.Value) : solicitud.SolicituDocu.Ciudad.Value);
                            //first.PlantillaCarta = first.PlantillaCarta.Replace("%NOMBRE%", ter != null ? (ter.NombrePri.Value + " " + ter.NombreSdo.Value + " " + ter.ApellidoPri.Value + " " + ter.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor1.Value);
                            //first.PlantillaCarta = first.PlantillaCarta.Replace("%CEDULA%", solicitud.SolicituDocu.Codeudor1.Value);
                        
                    }
                    else if (document == AppReports.drCondicionesGenerales)
                    {
                        DTO_coTercero cony = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor1.Value, true, false);
                        DTO_coTercero cod1 = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor2.Value, true, false);
                        DTO_coTercero cod2 = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor3.Value, true, false);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%DEUDOR%", nombreCli);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCDEUDOR%", solicitud.SolicituDocu.ClienteID.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%FECHA%", fechaActualCarta);// Revisar de donde tomo la fecha
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CONYUGE%", cony != null ? (cony.NombrePri.Value + " " + cony.NombreSdo.Value + " " + cony.ApellidoPri.Value + " " + cony.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor1.Value);                        
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCONYUGE%", solicitud.SolicituDocu.Codeudor1.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%FECHA2%", fechaActualCarta);// Revisar de donde tomo la fecha
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%COD1%", cod1 != null ? (cod1.NombrePri.Value + " " + cod1.NombreSdo.Value + " " + cod1.ApellidoPri.Value + " " + cod1.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor2.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCOD1%", solicitud.SolicituDocu.Codeudor2.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%FECHA3%", fechaActualCarta);// Revisar de donde tomo la fecha
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%COD2%", cod2 != null ? (cod2.NombrePri.Value + " " + cod2.NombreSdo.Value + " " + cod2.ApellidoPri.Value + " " + cod2.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor3.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCOD2%", solicitud.SolicituDocu.Codeudor3.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%FECHA4%", fechaActualCarta);// Revisar de donde tomo la fecha
                    }
                    else if (document == AppReports.drPagareCredito)
                    {
                        DTO_coTercero cony = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor1.Value, true, false);
                        DTO_coTercero cod1 = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor2.Value, true, false);
                        DTO_coTercero cod2 = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor3.Value, true, false);

                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CREDITO%", solicitud.SolicituDocu.Libranza.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%DEUDOR%", nombreCli);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCDEUDOR%", solicitud.SolicituDocu.ClienteID.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CONYUGE%", cony != null ? (cony.NombrePri.Value + " " + cony.NombreSdo.Value + " " + cony.ApellidoPri.Value + " " + cony.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor1.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCONYUGE%", solicitud.SolicituDocu.Codeudor1.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%COD1%", cod1 != null ? (cod1.NombrePri.Value + " " + cod1.NombreSdo.Value + " " + cod1.ApellidoPri.Value + " " + cod1.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor2.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCOD1%", solicitud.SolicituDocu.Codeudor2.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%COD2%", cod2 != null ? (cod1.NombrePri.Value + " " + cod2.NombreSdo.Value + " " + cod2.ApellidoPri.Value + " " + cod2.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor3.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCOD2%", solicitud.SolicituDocu.Codeudor3.Value);
                    }
                    else if (document == AppReports.drCartaPagareCredito)
                    {
                        DTO_glLugarGeografico ciu = (DTO_glLugarGeografico)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLugarGeografico, solicitud.SolicituDocu.Ciudad.Value, true, false);
                        DTO_coTercero cony = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor1.Value, true, false);
                        DTO_coTercero cod1 = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor2.Value, true, false);
                        DTO_coTercero cod2 = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor3.Value, true, false);


                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CREDITO%", solicitud.SolicituDocu.Libranza.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CIUDAD%", ciu != null ? (ciu.Descriptivo.Value) : solicitud.SolicituDocu.Ciudad.Value);

                        Decimal dia = Convert.ToDecimal(DateTime.Now.ToString("dd"));
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%DIALETRAS%", CurrencyFormater.GetCurrencyString("ES", "num", dia));
                        
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%DIA%", DateTime.Now.ToString("dd"));
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%MES%", DateTime.Now.Month.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%ANNO%", DateTime.Now.Year.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%DEUDOR%", nombreCli);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCDEUDOR%", solicitud.SolicituDocu.ClienteID.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CONYUGE%", cony != null ? (cony.NombrePri.Value + " " + cony.NombreSdo.Value + " " + cony.ApellidoPri.Value + " " + cony.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor1.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCONYUGE%", solicitud.SolicituDocu.Codeudor1.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%COD1%", cod1 != null ? (cod1.NombrePri.Value + " " + cod1.NombreSdo.Value + " " + cod1.ApellidoPri.Value + " " + cod1.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor2.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCOD1%", solicitud.SolicituDocu.Codeudor2.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%COD2%", cod2 != null ? (cod1.NombrePri.Value + " " + cod2.NombreSdo.Value + " " + cod2.ApellidoPri.Value + " " + cod2.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor3.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCOD2%", solicitud.SolicituDocu.Codeudor3.Value);
                    }
                    else if (document == AppReports.drPagareSeguro)
                    {
                        DTO_coTercero cony = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor1.Value, true, false);
                        DTO_coTercero cod1 = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor2.Value, true, false);
                        DTO_coTercero cod2 = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor3.Value, true, false);

                        first.PlantillaCarta = first.PlantillaCarta.Replace("%SEGUROS%", solicitud.SolicituDocu.Poliza.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%DEUDOR%", nombreCli);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCDEUDOR%", solicitud.SolicituDocu.ClienteID.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CONYUGE%", cony != null ? (cony.NombrePri.Value + " " + cony.NombreSdo.Value + " " + cony.ApellidoPri.Value + " " + cony.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor1.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCONYUGE%", solicitud.SolicituDocu.Codeudor1.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%COD1%", cod1 != null ? (cod1.NombrePri.Value + " " + cod1.NombreSdo.Value + " " + cod1.ApellidoPri.Value + " " + cod1.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor2.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCOD1%", solicitud.SolicituDocu.Codeudor2.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%COD2%", cod2 != null ? (cod1.NombrePri.Value + " " + cod2.NombreSdo.Value + " " + cod2.ApellidoPri.Value + " " + cod2.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor3.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCOD2%", solicitud.SolicituDocu.Codeudor3.Value);
                    }
                    else if (document == AppReports.drCartaPagareSeguro)
                    {
                        DTO_glLugarGeografico ciu = (DTO_glLugarGeografico)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLugarGeografico, solicitud.SolicituDocu.Ciudad.Value, true, false);
                        DTO_coTercero cony = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor1.Value, true, false);
                        DTO_coTercero cod1 = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor2.Value, true, false);
                        DTO_coTercero cod2 = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor3.Value, true, false);

                        first.PlantillaCarta = first.PlantillaCarta.Replace("%SEGUROS%", solicitud.SolicituDocu.Poliza.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CIUDAD%", ciu != null ? (ciu.Descriptivo.Value) : solicitud.SolicituDocu.Ciudad.Value);
                        Decimal dia = Convert.ToDecimal(DateTime.Now.ToString("dd"));
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%DIALETRAS%", CurrencyFormater.GetCurrencyString("ES", "num", dia));

                        first.PlantillaCarta = first.PlantillaCarta.Replace("%DIA%", DateTime.Now.ToString("dd"));
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%MES%", DateTime.Now.Month.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%ANNO%", DateTime.Now.Year.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%DEUDOR%", nombreCli);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCDEUDOR%", solicitud.SolicituDocu.ClienteID.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CONYUGE%", cony != null ? (cony.NombrePri.Value + " " + cony.NombreSdo.Value + " " + cony.ApellidoPri.Value + " " + cony.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor1.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCONYUGE%", solicitud.SolicituDocu.Codeudor1.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%COD1%", cod1 != null ? (cod1.NombrePri.Value + " " + cod1.NombreSdo.Value + " " + cod1.ApellidoPri.Value + " " + cod1.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor2.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCOD1%", solicitud.SolicituDocu.Codeudor2.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%COD2%", cod2 != null ? (cod1.NombrePri.Value + " " + cod2.NombreSdo.Value + " " + cod2.ApellidoPri.Value + " " + cod2.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor3.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCOD2%", solicitud.SolicituDocu.Codeudor3.Value);
                    }
                    else if (document == AppReports.drPrendaDeudor)
                    {
                        DTO_glLugarGeografico ciu = (DTO_glLugarGeografico)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLugarGeografico, solicitud.SolicituDocu.Ciudad.Value, true, false);
                        DTO_ccCliente deu = (DTO_ccCliente)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, solicitud.SolicituDocu.ClienteID.Value, true, false);
                        DTO_coTercero cony = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor1.Value, true, false);
                        DTO_coTercero cod1 = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor2.Value, true, false);
                        DTO_coTercero cod2 = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor3.Value, true, false);

                        if (document == AppReports.drPrendaDeudor)
                        {
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%PRENDA%", solicitud.SolicituDocu.Poliza.Value.ToString());
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%DEUDOR%", nombreCli);
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%CCDEUDOR%", solicitud.SolicituDocu.ClienteID.Value);

                            Decimal Valor = Convert.ToDecimal(solicitud.SolicituDocu.VlrSolicitado.Value.Value);
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRMONTOLETRAS%", CurrencyFormater.GetCurrencyString("ES", "num", Valor));
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRMONTO%", solicitud.SolicituDocu.VlrSolicitado.Value.HasValue ? solicitud.SolicituDocu.VlrSolicitado.Value.Value.ToString("n0") : "0");

                            first.PlantillaCarta = first.PlantillaCarta.Replace("%MARCA%", solicitud.DatosVehiculo.Marca.Value);
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%MOTOR%", solicitud.DatosVehiculo.Motor.Value);
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%CLASE%", solicitud.DatosVehiculo.Clase.Value.ToString());
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%PLACAS%", solicitud.DatosVehiculo.Placa.Value);
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%SERVICIO%", solicitud.DatosVehiculo.Servicio.Value == 1 ? "PARTICULAR" : "PÚBLICO");
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%MODELO%", solicitud.DatosVehiculo.Modelo.Value.ToString());
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%SERIE%", solicitud.DatosVehiculo.Serie.Value.ToString());// REVISAR SERIE O CHASIS
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%COLOR%", solicitud.DatosVehiculo.Color.Value.ToString());
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%TIPO%", solicitud.DatosVehiculo.Tipo.Value.ToString());
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%LINEA%", solicitud.DatosVehiculo.Linea.Value.ToString());
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%DIRECCION%", deu != null ? (deu.ResidenciaDir.Value) : solicitud.SolicituDocu.ClienteID.Value);
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%CIUDAD%", deu != null ? (deu.ResidenciaCiudadDesc.Value) : solicitud.SolicituDocu.ClienteID.Value);

                            Decimal ValorVehiculo = Convert.ToDecimal(solicitud.SolicituDocu.VlrSolicitado.Value.Value);
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRVEHICULOLETRAS%", CurrencyFormater.GetCurrencyString("ES", "num", ValorVehiculo));
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRVEHICULO%", solicitud.DatosVehiculo.PrecioVenta.Value.HasValue ? solicitud.DatosVehiculo.PrecioVenta.Value.Value.ToString("n0") : "0");

                            first.PlantillaCarta = first.PlantillaCarta.Replace("%CIUDADFIRMA%", ciu != null ? (ciu.Descriptivo.Value) : solicitud.SolicituDocu.Ciudad.Value);
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%DIA%", DateTime.Now.ToString("dd"));
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%MES%", DateTime.Now.Month.ToString());
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%ANNO%", DateTime.Now.Year.ToString());
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%DEUDOR%", nombreCli);
                            first.PlantillaCarta = first.PlantillaCarta.Replace("%CCDEUDOR%", solicitud.SolicituDocu.ClienteID.Value);
                        }
                        else
                        { 
                        }
                    }

                    else if (document == AppReports.drPrendaDeudor)
                    {
                        DTO_glLugarGeografico ciu = (DTO_glLugarGeografico)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLugarGeografico, solicitud.SolicituDocu.Ciudad.Value, true, false);
                        DTO_ccCliente deu = (DTO_ccCliente)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, solicitud.SolicituDocu.ClienteID.Value, true, false);
                        DTO_coTercero cony = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor1.Value, true, false);
                        DTO_coTercero cod1 = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor2.Value, true, false);
                        DTO_coTercero cod2 = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor3.Value, true, false);

                        first.PlantillaCarta = first.PlantillaCarta.Replace("%PRENDA%", solicitud.SolicituDocu.Poliza.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%DEUDOR%", nombreCli);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCDEUDOR%", solicitud.SolicituDocu.ClienteID.Value);

                        Decimal Valor = Convert.ToDecimal(solicitud.SolicituDocu.VlrSolicitado.Value.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRMONTOLETRAS%", CurrencyFormater.GetCurrencyString("ES", "num", Valor));
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRMONTO%", solicitud.SolicituDocu.VlrSolicitado.Value.HasValue ? solicitud.SolicituDocu.VlrSolicitado.Value.Value.ToString("n0") : "0");

                        first.PlantillaCarta = first.PlantillaCarta.Replace("%MARCA%", solicitud.DatosVehiculo.Marca.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%MOTOR%", solicitud.DatosVehiculo.Motor.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CLASE%", solicitud.DatosVehiculo.Clase.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%PLACAS%", solicitud.DatosVehiculo.Placa.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%SERVICIO%", solicitud.DatosVehiculo.Servicio.Value == 1 ? "PARTICULAR" : "PÚBLICO");
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%MODELO%", solicitud.DatosVehiculo.Modelo.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%SERIE%", solicitud.DatosVehiculo.Serie.Value.ToString());// REVISAR SERIE O CHASIS
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%COLOR%", solicitud.DatosVehiculo.Color.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%TIPO%", solicitud.DatosVehiculo.Tipo.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%LINEA%", solicitud.DatosVehiculo.Linea.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%DIRECCION%", deu != null ? (deu.ResidenciaDir.Value) : solicitud.SolicituDocu.ClienteID.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CIUDAD%", deu != null ? (deu.ResidenciaCiudadDesc.Value) : solicitud.SolicituDocu.ClienteID.Value);

                        Decimal ValorVehiculo = Convert.ToDecimal(solicitud.SolicituDocu.VlrSolicitado.Value.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRVEHICULOLETRAS%", CurrencyFormater.GetCurrencyString("ES", "num", ValorVehiculo));
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRVEHICULO%", solicitud.DatosVehiculo.PrecioVenta.Value.HasValue ? solicitud.DatosVehiculo.PrecioVenta.Value.Value.ToString("n0") : "0");

                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CIUDADFIRMA%", ciu != null ? (ciu.Descriptivo.Value) : solicitud.SolicituDocu.Ciudad.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%DIA%", DateTime.Now.ToString("dd"));
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%MES%", DateTime.Now.Month.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%ANNO%", DateTime.Now.Year.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%DEUDOR%", nombreCli);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCDEUDOR%", solicitud.SolicituDocu.ClienteID.Value);
                    }
                    
                    else if (document == AppReports.drCertGrupoDeudores)
                    {
                        DTO_glLugarGeografico ciu = (DTO_glLugarGeografico)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLugarGeografico, solicitud.SolicituDocu.Ciudad.Value, true, false);
                        DTO_coTercero cony = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor1.Value, true, false);

                        first.PlantillaCarta = first.PlantillaCarta.Replace("%DEUDOR%", nombreCli);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCDEUDOR%", solicitud.SolicituDocu.ClienteID.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CONYUGE%", cony != null ? (cony.NombrePri.Value + " " + cony.NombreSdo.Value + " " + cony.ApellidoPri.Value + " " + cony.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor1.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CCCONYUGE%", solicitud.SolicituDocu.Codeudor1.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%FECHA%", fechaActualCarta);// Revisar de donde tomo la fecha
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%FECHA2%", fechaActualCarta); // Revisar de donde tomo la fecha drsolciitud
                    }
                    else if (document == AppReports.drAprobacionDirectaSinDoc)
                    {
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%PLAZO%", solicitud.SolicituDocu.Plazo.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRMONTO%", solicitud.SolicituDocu.VlrSolicitado.Value.Value.ToString("n0"));
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%PLAZO%", solicitud.SolicituDocu.Plazo.Value.ToString());
                    }

                    else if (document == AppReports.drAprobacionDirectaConDoc)
                    {
                        DTO_coTercero ter = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor1.Value, true, false);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CONYUGE%", ter != null ? (ter.NombrePri.Value + " " + ter.NombreSdo.Value + " " + ter.ApellidoPri.Value + " " + ter.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor1.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRMONTO%", solicitud.SolicituDocu.VlrSolicitado.Value.Value.ToString("n0"));
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%PLAZO%", solicitud.SolicituDocu.Plazo.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRGARANTIA%", solicitud.DatosVehiculo.PrecioVenta.Value.Value.ToString("n0"));

                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRMONTOLETRAS%", solicitud.DatosVehiculo..Value);
                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRGARANTIA%", solicitud.DatosVehiculo..Value);
                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRCUOTAINI%", solicitud.DatosVehiculo..Value);
                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%PORMAX%", solicitud.DatosVehiculo..Value);
                    }
                    else if (document == AppReports.drNoViable)
                    {
                    }
                    else if (document == AppReports.drPreAprobacion)
                    {
                        DTO_coTercero ter = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, solicitud.SolicituDocu.Codeudor1.Value, true, false);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%CONYUGE%", ter != null ? (ter.NombrePri.Value + " " + ter.NombreSdo.Value + " " + ter.ApellidoPri.Value + " " + ter.ApellidoSdo.Value) : solicitud.SolicituDocu.Codeudor1.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRMONTO%", solicitud.SolicituDocu.VlrSolicitado.Value.Value.ToString("n0"));
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%PLAZO%", solicitud.SolicituDocu.Plazo.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%MODELO%", solicitud.DatosVehiculo.Modelo.Value==1?"Particular":"Publico");
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRGARANTIA%", solicitud.DatosVehiculo.PrecioVenta.Value.Value.ToString("n0"));
                        
                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRMONTOLETRAS%", solicitud...Value);
                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRGARANTIA%", solicitud...Value);
                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRCUOTAINI%", solicitud...Value);
                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%PORMAX%", solicitud...Value);
                    }
                    else if (document == AppReports.drRatificacion)
                    {
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRMONTO%", solicitud.SolicituDocu.VlrSolicitado.Value.Value.ToString("n0"));
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%PLAZO%", solicitud.SolicituDocu.Plazo.Value.ToString());
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRGARANTIA%", solicitud.DatosVehiculo.PrecioVenta.Value.Value.ToString("n0"));
                        int cuotainicial = Convert.ToInt32(solicitud.DatosVehiculo.PrecioVenta.Value) - Convert.ToInt32(solicitud.SolicituDocu.VlrSolicitado.Value);
                        first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRCUOTAINI%", cuotainicial.ToString("n0"));
                        
                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRMONTOLETRAS%", solicitud...Value);
                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRGARANTIA%", solicitud...Value);
                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%VLRCUOTAINI%", solicitud...Value);
                        //first.PlantillaCarta = first.PlantillaCarta.Replace("%PORMAX%", solicitud...Value);
                    }
                    first.Detalle.Add(first);
                }


                source.Add(first);

                this.DataSource = source;
                this.ShowPreview();
                //this.CreateReport();
                return this.ReportName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables básicas para el reporte del usuario
        /// </summary>
        /// <param name="conn">conexion a base datos</param>
        /// <param name="tx">transaccion</param>
        /// <param name="emp">empresa</param>
        /// <param name="userID">identificador del usuario</param>
        public void InitUserReport(string loggerConn, SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, ExportFormatType formatType, int? numDoc = null)
        {
            this._connection = conn;
            this._transaction = tx;
            this._empresa = emp;
            this._userID = userID;
            this._formatType = formatType;
            this.loggerConnectionStr = loggerConn;

            this.numeroDoc = numDoc;
            this.SetUserParameters();
        }

        /// <summary>
        /// Inicializa objetos y parametros iniciales
        /// </summary>
        protected virtual void SetUserParameters()
        {
            this._moduloGlobal = new ModuloGlobal(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this._moduloGlobal.Empresa = this.Empresa;
            this._moduloBase = new ModuloBase(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            DTO_seUsuario usuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this._userID);

            #region Nombre del reporte

            string repName;
            string filesPath = this._moduloGlobal.GetControlValue(AppControl.RutaFisicaArchivos);
            string docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            string ext = this.GetExtention();

            if (this.numeroDoc.HasValue)
            {
                // Reporte de documento
                string fileFormat = this._moduloGlobal.GetControlValue(AppControl.NombreArchivoDocumentos);
                repName = string.Format(fileFormat, this.numeroDoc.ToString());
                docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaDocumentos);
            }
            else
            {
                // Reporte temporal
                repName = Guid.NewGuid().ToString();
                docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            }

            this.ReportName = repName.ToString() + ext;
            this.Path = filesPath + docsPath + ReportName;

            #endregion
            #region Header
            byte[] logo = this._moduloGlobal.glEmpresaLogo();
            try
            {
                object img = Utility.ByteArrayToObject(logo);
                Image logoImage = (Image)img;
            }
            catch { ; }

            #endregion
            #region Recusos

            this.reportProvider = new ReportProvider(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this.reportProvider.LoadResources(this.AllControls<XRControl>());

            #endregion
        }

        /// <summary>
        /// Función que exporta de acuerdo al tipo de formato seleccionado por el usuario
        /// </summary>
        protected void CreateReport()
        {
            try
            {
                switch (this._formatType)
                {
                    case ExportFormatType.pdf:
                        this.ExportToPdf(Path);
                        break;
                    case ExportFormatType.xls:
                        this.ExportToXls(Path);
                        break;
                    case ExportFormatType.xlsx:
                        this.ExportToXlsx(Path);
                        break;
                    case ExportFormatType.html:
                        this.ExportToHtml(Path);
                        break;
                }

                if (this.numeroDoc.HasValue)
                    this.ReportName = this.numeroDoc.ToString();
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this._userID.ToString(), "CreateReport");
                throw ex;
            }
        }

        /// <summary>
        /// Funcion para crear los paramatros de conexion
        /// </summary>
        protected void ConfigureConnection(SqlDataSource source)
        {
            try
            {
                MsSqlConnectionParameters connParameters = new MsSqlConnectionParameters();
                SqlConnectionStringBuilder credentials = new SqlConnectionStringBuilder(this.loggerConnectionStr);
                connParameters.DatabaseName = this._connection.Database;
                connParameters.ServerName = this._connection.DataSource;
                connParameters.AuthorizationType = credentials.IntegratedSecurity ? MsSqlAuthorizationType.Windows : MsSqlAuthorizationType.SqlServer;
                connParameters.UserName = credentials.UserID;
                connParameters.Password = credentials.Password;
                source.ConnectionName = "NewAgeConnection";
                source.ConnectionParameters = connParameters;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this._userID.ToString(), "ConfigureConnection");
                throw ex;
            }
        }


        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion que de acuerdo al ExportType devuelve la extensión
        /// </summary>
        private string GetExtention()
        {
            string extension = string.Empty;
            switch (this._formatType)
            {
                case ExportFormatType.pdf:
                    extension =  ".pdf";
                    break;
                case ExportFormatType.xls:
                    extension = ".xls";
                    break;
                case ExportFormatType.xlsx:
                    extension = ".xlsx";
                    break;
                case ExportFormatType.html:
                    extension = ".html";
                    break;
            }
            return extension;
        }
        #endregion

    }
}
