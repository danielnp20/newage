
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.ADO;
using NewAge.DTO.Reportes;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.Threading;
using NewAge.Librerias.ExceptionHandler;
using SentenceTransformer;
using System.Data;
using System.IO;

namespace NewAge.Negocio
{
    public class ModuloDecisorRiesgo : ModuloBase
    {
        #region Variables

        #region DALs

        private DAL_Decisor _dal_Decisor= null;
        private DAL_drSolicitudDatosPersonales _dal_SolicitaDatosPersonales = null;
        private DAL_drSolicitudDatosVehiculo _dal_SolicitaDatosVehiculo = null;
        private DAL_drSolicitudDatosOtros _dal_SolicitaDatosOtros = null;
        private DAL_ccSolicitudDataCreditoScore _dal_SolicitaDatacredScore = null;
        private DAL_ccSolicitudDataCreditoDatos _dal_SolicitaDatacredDatos = null;
        private DAL_ccSolicitudDataCreditoQuanto _dal_SolicitaDatacredQuanto = null;
        private DAL_drSolicitudDatosChequeados _dal_DatosChequeados = null;




        private DAL_drActividadChequeoLista _dal_ChequeoLista = null;

        private DAL_ccSolicitudDataCreditoUbica _dal_SolicitaDatacredUbica = null;      
        private DAL_ccSolicitudDataCreditoDatos _dal_ccDatacreditoDatos = null;
        private DAL_ccSolicitudDataCreditoScore _dal_ccDatacreditoScore = null;
        private DAL_ccSolicitudDataCreditoQuanto _dal_ccDatacreditoQuanto = null;
        #endregion

        #region Modulos

        private ModuloGlobal _moduloGlobal = null;
        private ModuloCartera _moduloCartera = null;
        private ModuloContabilidad _moduloContabilidad = null;
        private ModuloCuentasXPagar _moduloCxP = null;
        private DAL_ccSolicitudDocu _dalsolDocu = null;

        #endregion

        #endregion 

        /// <summary>
        /// Constructor Modulo Cartera
        /// </summary>
        /// <param name="conn"></param>
        public ModuloDecisorRiesgo(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        #region Operaciones

        /// <summary>
        /// Consulta las operaciones pendientes por gestion
        /// </summary>
        public List<DTO_OperacionesPendientes> OperacionesPendientes()
        {
            this._dal_Decisor = (DAL_Decisor)base.GetInstance(typeof(DAL_Decisor), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            List<DTO_OperacionesPendientes> Pendientes = new List<DTO_OperacionesPendientes>();
            DTO_seUsuario seUsuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId);
            string usuarioID = seUsuario.ID.Value;

            Pendientes = this._dal_Decisor.DAL_Decisor_GetPendientes(usuarioID);
            return Pendientes;
        }
        /// <summary>
        /// Consulta las Obligaciones
        /// </summary>
        public List<DTO_QueryObligaciones> Obligaciones(int NumeroDoc)
        {
            this._dal_Decisor = (DAL_Decisor)base.GetInstance(typeof(DAL_Decisor), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_QueryObligaciones> Pendientes = new List<DTO_QueryObligaciones>();
            Pendientes = this._dal_Decisor.DAL_Decisor_Obligaciones(NumeroDoc);
            return Pendientes;
        }


        /// <summary>
        /// Actualiza Garantias
        /// </summary>
        public DTO_TxResult drSolicitudGarantias_Update(List<DTO_QueryGarantiaControl> Garantias)
        {
            base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                this._dal_Decisor = (DAL_Decisor)base.GetInstance(typeof(DAL_Decisor), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                bool Valida;
                foreach (DTO_QueryGarantiaControl Garantia in Garantias)
                {
                    if (Garantia.ConsGarantia.Value != 0 && Garantia.ConsGarantia.Value != null)
                    {

                        Valida = this._dal_Decisor.DAL_drSolicitudGarantias_Update(Garantia);
                        if (Valida)
                            result.Result = ResultValue.OK;
                        else
                            result.Result = ResultValue.NOK;
                    }
                }
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null )
                    base._mySqlConnectionTx.Rollback();
            }

            return result;

        }

        /// <summary>
        /// Actualiza Obligaciones
        /// </summary>

        public DTO_TxResult drSolicitudObligaciones_Update(List<DTO_QueryObligaciones> Obligaciones)
        {

             base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                this._dal_Decisor = (DAL_Decisor)base.GetInstance(typeof(DAL_Decisor), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                bool Valida;
                foreach (DTO_QueryObligaciones Obligacion in Obligaciones)
                {
                    if (Obligacion.Oblig.Value != 0 && Obligacion.Oblig.Value !=null)
                    {
                        Valida = this._dal_Decisor.DAL_drSolicitudObligaciones_Update(Obligacion);
                        if (Valida)
                            result.Result = ResultValue.OK;
                        else
                            result.Result = ResultValue.NOK;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null )
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Agrega las nuevas solicitudes del canal preferencial
        /// </summary>
        public DTO_TxResult SolicitudCanalPreferencial_Add(bool isAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!isAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            this._dal_Decisor = (DAL_Decisor)base.GetInstance(typeof(DAL_Decisor), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dalsolDocu = (DAL_ccSolicitudDocu)base.GetInstance(typeof(DAL_ccSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_SolicitaDatosVehiculo = (DAL_drSolicitudDatosVehiculo)base.GetInstance(typeof(DAL_drSolicitudDatosVehiculo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloCartera = (ModuloCartera)this.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            try
            {
                string lineaCredVeh = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_LineaCredVehiculos);
                string tipoCreditoVeh = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_TipoCredVehiculos);
                string zonaDef = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Zona);
                string pagaduriaDef = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_PagaduriaXDefecto);
                string centroPagoDef = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CentroPagoPorDefecto);
                DateTime periodo = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo));


                //COnsecutivo Web Control
                string EmpNro = this.Empresa.NumeroControl.Value;
                string _modId = ((int)ModulesPrefix.cc).ToString();
                string keyControl = EmpNro + _modId + AppControl.cc_UltimoConsecCanalPreferencial;


                DTO_glControl datoCtrl = this._moduloGlobal.GetControlByID(Convert.ToInt32(keyControl));
                int consecutivoWebUltimo = string.IsNullOrWhiteSpace(datoCtrl.Data.Value) ? 0 : Convert.ToInt32(datoCtrl.Data.Value);
                int consecutivoWebNuevo = 0;
                List<DTO_DigitaSolicitudDecisor> nuevos = this._dal_Decisor.DAL_Decisor_GetMYSQL(consecutivoWebUltimo);
                foreach (DTO_DigitaSolicitudDecisor sol in nuevos)
                {
                    DTO_ccCliente cli = (DTO_ccCliente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, sol.SolicituDocu.ClienteRadica.Value, true, false);
                    if (cli != null)
                        sol.SolicituDocu.ClienteID.Value = sol.SolicituDocu.ClienteRadica.Value;

                    sol.SolicituDocu.LineaCreditoID.Value = lineaCredVeh;
                    sol.SolicituDocu.TipoCreditoID.Value = tipoCreditoVeh;
                    sol.SolicituDocu.Libranza.Value = 0;
                    sol.SolicituDocu.CentroPagoID.Value = centroPagoDef;
                    sol.SolicituDocu.PagaduriaID.Value = pagaduriaDef;
                    sol.SolicituDocu.ZonaID.Value = zonaDef;
                    sol.SolicituDocu.CooperativaID.Value = string.Empty;
                    sol.SolicituDocu.VersionNro.Value = 1;

                    DTO_ccAsesor asesor = (DTO_ccAsesor)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccAsesor, sol.SolicituDocu.AsesorID.Value, true, false);
                    if (asesor != null)
                        sol.SolicituDocu.ConcesionarioID.Value = asesor.ConcesionarioID.Value;
                    else
                        sol.SolicituDocu.ConcesionarioID.Value = null;

                    DTO_ccConcesionario conces = (DTO_ccConcesionario)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccConcesionario, sol.SolicituDocu.ConcesionarioID.Value, true, false);
                    if (conces != null)
                        sol.SolicituDocu.Ciudad.Value = conces.Ciudad.Value;// sol.SolicituDocu.Ciudad.Value.Equals("1") ? "11001" : "05001";

                    DTO_SolicitudLibranza radicacion = new DTO_SolicitudLibranza();
                    radicacion.DocCtrl.PeriodoDoc.Value = periodo;
                    radicacion.DocCtrl.PeriodoUltMov.Value = periodo;
                    radicacion.DocCtrl.Observacion.Value = string.Empty;
                    radicacion.DocCtrl.DocumentoID.Value = AppDocuments.SolicitudLibranza;
                    radicacion.DocCtrl.NumeroDoc.Value = 0;
                    radicacion.DocCtrl.FechaDoc.Value = DateTime.Now.Month == periodo.Month && DateTime.Now.Year == periodo.Year ? DateTime.Now : new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                    radicacion.DocCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
                    radicacion.DocCtrl.Descripcion.Value = "Solicitud Crédito ";
                    radicacion.DocCtrl.Fecha.Value = DateTime.Now;
                    radicacion.DocCtrl.LugarGeograficoID.Value = sol.SolicituDocu.Ciudad.Value;
                    radicacion.DocCtrl.AreaFuncionalID.Value = this._moduloGlobal.seUsuario_GetUserByReplicaID(UserId).AreaFuncionalID.Value;
                    radicacion.DocCtrl.PrefijoID.Value = this._moduloGlobal.GetPrefijoByDocumento(AppDocuments.SolicitudLibranza);
                    radicacion.DocCtrl.MonedaID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                    radicacion.DocCtrl.TasaCambioDOCU.Value = 0;
                    radicacion.DocCtrl.TasaCambioCONT.Value = 0;
                    radicacion.DocCtrl.Valor.Value = sol.SolicituDocu.VlrPreSolicitado.Value;
                    radicacion.DocCtrl.Iva.Value = 0;
                    radicacion.DocCtrl.Estado.Value = (byte)EstadoDocControl.ParaAprobacion;
                    radicacion.DocCtrl.seUsuarioID.Value = UserId;
                    radicacion.Header = sol.SolicituDocu;

                    var exist = this._dalsolDocu.DAL_ccSolicitudDocu_GetByConsecWeb(sol.SolicituDocu.ConsecutivoWEB.Value.Value);
                    if (exist == null)
                        result = this._moduloCartera.SolicitudLibranza_Add(AppDocuments.SolicitudLibranza, radicacion, true);
                    if (result.Result == ResultValue.NOK)
                        break;
                    //#region Guarda datos Vehiculo            
                    //if (sol.DatosVehiculo.Modelo.Value.HasValue && radicacion.DocCtrl.NumeroDoc.Value.HasValue && radicacion.DocCtrl.NumeroDoc.Value != 0)
                    //{
                    //    #region Agrega nuevo
                    //    sol.DatosVehiculo.NumeroDoc.Value = radicacion.DocCtrl.NumeroDoc.Value;
                    //    sol.DatosVehiculo.Version.Value = 1;
                    //    sol.DatosVehiculo.Consecutivo.Value = this._dal_SolicitaDatosVehiculo.DAL_drSolicitudDatosVehiculo_Add(sol.DatosVehiculo);
                    //    #endregion
                    //}
                    //else
                    //{
                    //    result = result;
                    //}
                    //#endregion
                    consecutivoWebNuevo = sol.SolicituDocu.ConsecutivoWEB.Value.Value;
                }
                #region Actualiza el consecutivo web
                datoCtrl.Data.Value = consecutivoWebNuevo.ToString();
                this._moduloGlobal.glControl_Update(datoCtrl);

                #endregion
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "SolicitudCanalPreferencial_Add");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!isAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !isAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }
        #endregion

        #region Digitacion Solicitud

        /// <summary>
        /// Trae la informacion de una digitacion de solicitud de credito segun a libranza
        /// </summary>
        /// <param name="libranzaID">id del credito</param>
        /// <param name="actFlujoID">Identificador de la actividad de flujo</param>
        /// <returns>Retorna la informacion de una solicitud de credito</returns>
        public DTO_DigitaSolicitudDecisor DigitacionSolicitud_GetBySolicitud(int libranzaID)
        {
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloCartera = (ModuloCartera)base.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_SolicitaDatosPersonales = (DAL_drSolicitudDatosPersonales)base.GetInstance(typeof(DAL_drSolicitudDatosPersonales), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_SolicitaDatosVehiculo = (DAL_drSolicitudDatosVehiculo)base.GetInstance(typeof(DAL_drSolicitudDatosVehiculo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_SolicitaDatosOtros = (DAL_drSolicitudDatosOtros)base.GetInstance(typeof(DAL_drSolicitudDatosOtros), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_SolicitaDatacredUbica = (DAL_ccSolicitudDataCreditoUbica)base.GetInstance(typeof(DAL_ccSolicitudDataCreditoUbica), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_SolicitaDatacredDatos = (DAL_ccSolicitudDataCreditoDatos)base.GetInstance(typeof(DAL_ccSolicitudDataCreditoDatos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_SolicitaDatacredScore = (DAL_ccSolicitudDataCreditoScore)base.GetInstance(typeof(DAL_ccSolicitudDataCreditoScore), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_SolicitaDatacredQuanto = (DAL_ccSolicitudDataCreditoQuanto)base.GetInstance(typeof(DAL_ccSolicitudDataCreditoQuanto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_DatosChequeados = (DAL_drSolicitudDatosChequeados)base.GetInstance(typeof(DAL_drSolicitudDatosChequeados), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_DigitaSolicitudDecisor result = new DTO_DigitaSolicitudDecisor();

                //Trae la solicitud
                DTO_SolicitudLibranza solicitud = this._moduloCartera.SolicitudLibranza_GetByLibranza(libranzaID, "");
                result.DocCtrl = solicitud.DocCtrl;
                result.SolicituDocu = solicitud.Header;

                if (solicitud != null)
                {
                    result.DatosPersonales = this._dal_SolicitaDatosPersonales.DAL_drSolicitudDatosPersonales_GetByNumeroDoc(result.DocCtrl.NumeroDoc.Value.Value, Convert.ToInt16(solicitud.Header.VersionNro.Value));
                    result.DatosVehiculo = this._dal_SolicitaDatosVehiculo.DAL_drSolicitudDatosVehiculo_GetByNumeroDoc(result.DocCtrl.NumeroDoc.Value.Value, Convert.ToInt16(solicitud.Header.VersionNro.Value));
                    result.OtrosDatos = this._dal_SolicitaDatosOtros.DAL_drSolicitudDatosOtros_GetByNumeroDoc(result.DocCtrl.NumeroDoc.Value.Value, Convert.ToInt16(solicitud.Header.VersionNro.Value));
                    result.DataCreditoUbica = this._dal_SolicitaDatacredUbica.DAL_ccSolicitudDataCreditoUbica_GetByNUmeroDoc(result.DocCtrl.NumeroDoc.Value.Value, Convert.ToInt16(solicitud.Header.VersionNro.Value));
                    result.DataCreditoDatos = this._dal_SolicitaDatacredDatos.DAL_ccSolicitudDataCreditoDatos_GetByNUmeroDoc(result.DocCtrl.NumeroDoc.Value.Value, Convert.ToInt16(solicitud.Header.VersionNro.Value));
                    result.DataCreditoScore = this._dal_SolicitaDatacredScore.DAL_ccSolicitudDataCreditoScore_GetByNUmeroDoc(result.DocCtrl.NumeroDoc.Value.Value, Convert.ToInt16(solicitud.Header.VersionNro.Value));
                    result.DataCreditoQuanto = this._dal_SolicitaDatacredQuanto.DAL_ccSolicitudDataCreditoQuanto_GetByNUmeroDoc(result.DocCtrl.NumeroDoc.Value.Value, Convert.ToInt16(solicitud.Header.VersionNro.Value));
                    result.DatosChequeados = this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_GetByNumDoc(result.DocCtrl.NumeroDoc.Value.Value, Convert.ToInt16(solicitud.Header.VersionNro.Value));
                    //result.DataCreditootr = this._dal_SolicitaDatacredUbica.DAL_ccSolicitudDataCreditoUbica_GetByNUmeroDoc(result.DocCtrl.NumeroDoc.Value.Value, Convert.ToInt16(solicitud.Header.VersionNro.Value));
                }

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "SolicitudLibranza_GetByLibranza");
                throw ex;
            }
        }

        /// <summary>
        /// Agrega informacion a las tablas de DigitacionSolicitud
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="data">Datos que se debe agregar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult DigitacionSolicitud_Add(int documentoID, string actFlujoId, DTO_DigitaSolicitudDecisor data, List<DTO_glDocumentoChequeoLista> chequeos, bool isNewVersion, bool isAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!isAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            this._dalsolDocu = (DAL_ccSolicitudDocu)base.GetInstance(typeof(DAL_ccSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_SolicitaDatosPersonales = (DAL_drSolicitudDatosPersonales)base.GetInstance(typeof(DAL_drSolicitudDatosPersonales), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_SolicitaDatosVehiculo = (DAL_drSolicitudDatosVehiculo)base.GetInstance(typeof(DAL_drSolicitudDatosVehiculo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_SolicitaDatosOtros = (DAL_drSolicitudDatosOtros)base.GetInstance(typeof(DAL_drSolicitudDatosOtros), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_DatosChequeados = (DAL_drSolicitudDatosChequeados)base.GetInstance(typeof(DAL_drSolicitudDatosChequeados), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);




            try
            {            
                if (!isNewVersion)
                {
                    bool exist = false;
                    #region Guarda o actualiza Datos personales
                    foreach (DTO_drSolicitudDatosPersonales d in data.DatosPersonales)
                    {
                        //Valida si  existe
                        exist = this._dal_SolicitaDatosPersonales.DAL_drSolicitudDatosPersonales_Exist(d.Consecutivo.Value);
                        if (exist)
                        {
                            #region Actualiza 
                            d.NumeroDoc.Value = data.DocCtrl.NumeroDoc.Value;
                            d.UsuarioDigita.Value = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId).ID.Value;
                            #region Valida si activa la solicitud de datacredito

                            #endregion
                            this._dal_SolicitaDatosPersonales.DAL_drSolicitudDatosPersonales_Update(d);
                            #endregion
                        }
                        else
                        {
                            #region Agrega nuevo
                            d.NumeroDoc.Value = data.DocCtrl.NumeroDoc.Value;
                            d.Version.Value = d.Version.Value?? 1;
                            d.UsuarioDigita.Value = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId).ID.Value;
                            d.Consecutivo.Value = this._dal_SolicitaDatosPersonales.DAL_drSolicitudDatosPersonales_Add(d);
                            #endregion
                        }
                        #region Valida si es codeudor para la solicitud
                        DTO_coTercero terExist = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, d.TerceroID.Value, true, false);
                        if (d.TipoPersona.Value == 2)// && terExist != null)
                            data.SolicituDocu.Codeudor1.Value = d.TerceroID.Value;
                        else if (d.TipoPersona.Value == 3)// && terExist != null)
                            data.SolicituDocu.Codeudor2.Value = d.TerceroID.Value;
                        else if (d.TipoPersona.Value == 4)// && terExist != null)
                            data.SolicituDocu.Codeudor3.Value = d.TerceroID.Value;
                        else if (d.TipoPersona.Value == 5)// && terExist != null)
                            data.SolicituDocu.Codeudor4.Value = d.TerceroID.Value;
                        else if (d.TipoPersona.Value == 6)// && terExist != null)
                            data.SolicituDocu.Codeudor5.Value = d.TerceroID.Value;
                        #endregion
                    }
                    #endregion
                    #region Guarda datos Vehiculo
                    //Valida si existe
                    exist = this._dal_SolicitaDatosVehiculo.DAL_drSolicitudDatosVehiculo_Exist(data.DatosVehiculo.Consecutivo.Value);
                    if (exist)
                    {
                        #region Actualiza 
                        data.DatosVehiculo.NumeroDoc.Value = data.DocCtrl.NumeroDoc.Value;
                        this._dal_SolicitaDatosVehiculo.DAL_drSolicitudDatosVehiculo_Update(data.DatosVehiculo);
                        #endregion
                    }
                    else
                    {
                        if (data.DatosVehiculo.Modelo.Value.HasValue)
                        {
                            #region Agrega nuevo
                            data.DatosVehiculo.NumeroDoc.Value = data.DocCtrl.NumeroDoc.Value;
                            data.DatosVehiculo.Version.Value = data.DatosVehiculo.Version.Value ?? 1;
                            data.DatosVehiculo.Consecutivo.Value = this._dal_SolicitaDatosVehiculo.DAL_drSolicitudDatosVehiculo_Add(data.DatosVehiculo);
                            #endregion
                        }
                    }     
                    #endregion                    
                    #region Guarda Otros datos 
                    //Valida si existe
                    exist = this._dal_SolicitaDatosOtros.DAL_drSolicitudDatosOtros_Exist(data.OtrosDatos.Consecutivo.Value);
                    if (exist)
                    {
                        #region Actualiza
                        data.OtrosDatos.NumeroDoc.Value = data.DocCtrl.NumeroDoc.Value;
                        this._dal_SolicitaDatosOtros.DAL_drSolicitudDatosOtros_Update(data.OtrosDatos);
                        #endregion
                    }
                    else
                    {
                        if (data.OtrosDatos.NumeroDoc.Value.HasValue)
                        {
                            #region Agrega nuevo
                            data.OtrosDatos.NumeroDoc.Value = data.DocCtrl.NumeroDoc.Value;
                            data.OtrosDatos.Version.Value = data.OtrosDatos.Version.Value?? 1;
                            data.OtrosDatos.Consecutivo.Value = this._dal_SolicitaDatosOtros.DAL_drSolicitudDatosOtros_Add(data.OtrosDatos);

                            #endregion
                        }
                    }
                    #endregion                    
                    #region Actualiza glDocumentoChequeoLista

                    foreach (DTO_glDocumentoChequeoLista d in chequeos)
                    {
                        if (d.IncluidoDeudor.Value.Value)
                        {
                            DTO_drSolicitudDatosChequeados actChequeoBase = new DTO_drSolicitudDatosChequeados();
                            actChequeoBase.NumeroDoc.Value = data.SolicituDocu.NumeroDoc.Value;
                            actChequeoBase.Version.Value = data.SolicituDocu.VersionNro.Value;
                            actChequeoBase.TipoPersona.Value = 1;
                            actChequeoBase.NroRegistro.Value = d.Consecutivo.Value;

                            exist = this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Exist(data.SolicituDocu.NumeroDoc.Value.Value, data.SolicituDocu.VersionNro.Value.Value, 1, d.Consecutivo.Value.Value);
                            if (exist)
                            {
                                if (actFlujoId.TrimEnd() == d.ActividadFlujoID.Value.TrimEnd())
                                {
                                    actChequeoBase.ChequeadoInd.Value = d.IncluidoInd.Value;
                                    this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Update(actChequeoBase);
                                }
                                else
                                {
                                    actChequeoBase.VerficadoInd.Value = d.IncluidoInd.Value;
                                    this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_UpdateVerifica(actChequeoBase);
                                }
                            }
                            else
                            {
                                actChequeoBase.ChequeadoInd.Value = d.IncluidoInd.Value;
                                actChequeoBase.Consecutivo.Value = this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Add(actChequeoBase);
                            }
                        }
                        if (d.IncluidoConyuge.Value.Value)
                        {
                            DTO_drSolicitudDatosChequeados actChequeoBase = new DTO_drSolicitudDatosChequeados();
                            actChequeoBase.NumeroDoc.Value = data.SolicituDocu.NumeroDoc.Value;
                            actChequeoBase.Version.Value = data.SolicituDocu.VersionNro.Value;
                            actChequeoBase.TipoPersona.Value = 2;
                            actChequeoBase.NroRegistro.Value = d.Consecutivo.Value;

                            exist = this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Exist(data.SolicituDocu.NumeroDoc.Value.Value, data.SolicituDocu.VersionNro.Value.Value, 2, d.Consecutivo.Value.Value);
                            if (exist)
                            {
                                if (actFlujoId.TrimEnd() == d.ActividadFlujoID.Value.TrimEnd())
                                {
                                    actChequeoBase.ChequeadoInd.Value = d.IncluidoConyugeInd.Value;
                                    this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Update(actChequeoBase);
                                }
                                else
                                {
                                    actChequeoBase.VerficadoInd.Value = d.IncluidoConyugeInd.Value;
                                    this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_UpdateVerifica(actChequeoBase);
                                }
                            }
                            else
                            {
                                actChequeoBase.ChequeadoInd.Value = d.IncluidoConyugeInd.Value;
                                actChequeoBase.Consecutivo.Value = this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Add(actChequeoBase);
                            }
                        }
                        if (d.IncluidoCodeudor1.Value.Value)
                        {
                            DTO_drSolicitudDatosChequeados actChequeoBase = new DTO_drSolicitudDatosChequeados();
                            actChequeoBase.NumeroDoc.Value = data.SolicituDocu.NumeroDoc.Value;
                            actChequeoBase.Version.Value = data.SolicituDocu.VersionNro.Value;
                            actChequeoBase.TipoPersona.Value = 3;
                            actChequeoBase.NroRegistro.Value = d.Consecutivo.Value;

                            exist = this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Exist(data.SolicituDocu.NumeroDoc.Value.Value, data.SolicituDocu.VersionNro.Value.Value, 3, d.Consecutivo.Value.Value);
                            if (exist)
                            {
                                if (actFlujoId.TrimEnd() == d.ActividadFlujoID.Value.TrimEnd())
                                {
                                    actChequeoBase.ChequeadoInd.Value = d.IncluidoCodeudor1Ind.Value;
                                    this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Update(actChequeoBase);
                                }
                                else
                                {
                                    actChequeoBase.VerficadoInd.Value = d.IncluidoCodeudor1Ind.Value;
                                    this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_UpdateVerifica(actChequeoBase);
                                }
                                this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Update(actChequeoBase);
                            }
                            else
                            {
                                actChequeoBase.ChequeadoInd.Value = d.IncluidoCodeudor1Ind.Value;
                                actChequeoBase.Consecutivo.Value = this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Add(actChequeoBase);
                            }
                        }
                        if (d.IncluidoCodeudor2.Value.Value)
                        {
                            DTO_drSolicitudDatosChequeados actChequeoBase = new DTO_drSolicitudDatosChequeados();
                            actChequeoBase.NumeroDoc.Value = data.SolicituDocu.NumeroDoc.Value;
                            actChequeoBase.Version.Value = data.SolicituDocu.VersionNro.Value;
                            actChequeoBase.TipoPersona.Value = 4;

                            actChequeoBase.NroRegistro.Value = d.Consecutivo.Value;

                             exist = this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Exist(data.SolicituDocu.NumeroDoc.Value.Value, data.SolicituDocu.VersionNro.Value.Value, 4, d.Consecutivo.Value.Value);
                            if (exist)
                            {
                                if (actFlujoId.TrimEnd() == d.ActividadFlujoID.Value.TrimEnd())
                                {
                                    actChequeoBase.ChequeadoInd.Value = d.IncluidoCodeudor2Ind.Value;
                                    this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Update(actChequeoBase);
                                }
                                else
                                {
                                    actChequeoBase.VerficadoInd.Value = d.IncluidoCodeudor2Ind.Value;
                                    this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_UpdateVerifica(actChequeoBase);
                                }
                            }
                            else
                            {
                                actChequeoBase.ChequeadoInd.Value = d.IncluidoCodeudor2Ind.Value;
                                actChequeoBase.Consecutivo.Value = this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Add(actChequeoBase);
                            }
                        }
                        if (d.IncluidoCodeudor3.Value.Value)
                        {
                            DTO_drSolicitudDatosChequeados actChequeoBase = new DTO_drSolicitudDatosChequeados();
                            actChequeoBase.NumeroDoc.Value = data.SolicituDocu.NumeroDoc.Value;
                            actChequeoBase.Version.Value = data.SolicituDocu.VersionNro.Value;
                            actChequeoBase.TipoPersona.Value = 5;

                            actChequeoBase.NroRegistro.Value = d.Consecutivo.Value;

                            exist = this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Exist(data.SolicituDocu.NumeroDoc.Value.Value, data.SolicituDocu.VersionNro.Value.Value, 5, d.Consecutivo.Value.Value);
                            if (exist)
                            {
                                if (actFlujoId.TrimEnd() == d.ActividadFlujoID.Value.TrimEnd())
                                {
                                    actChequeoBase.ChequeadoInd.Value = d.IncluidoCodeudor3Ind.Value;
                                    this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Update(actChequeoBase);
                                }
                                else
                                {
                                    actChequeoBase.VerficadoInd.Value = d.IncluidoCodeudor3Ind.Value;
                                    this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_UpdateVerifica(actChequeoBase);
                                }
                            }
                            else
                            {
                                actChequeoBase.ChequeadoInd.Value = d.IncluidoCodeudor3Ind.Value;
                                actChequeoBase.Consecutivo.Value = this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Add(actChequeoBase);
                            }
                        }
                    }
                    #endregion 

                }
                else
                {
                    bool exist = false;
                    this._dal_ccDatacreditoDatos = (DAL_ccSolicitudDataCreditoDatos)base.GetInstance(typeof(DAL_ccSolicitudDataCreditoDatos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_ccDatacreditoQuanto = (DAL_ccSolicitudDataCreditoQuanto)base.GetInstance(typeof(DAL_ccSolicitudDataCreditoQuanto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_ccDatacreditoScore = (DAL_ccSolicitudDataCreditoScore)base.GetInstance(typeof(DAL_ccSolicitudDataCreditoScore), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_SolicitaDatacredUbica = (DAL_ccSolicitudDataCreditoUbica)base.GetInstance(typeof(DAL_ccSolicitudDataCreditoUbica), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    List<string> cedulaSolicitaDatacredito = new List<string>();
                    data.SolicituDocu.VersionNro.Value = data.SolicituDocu.VersionNro.Value + 1;
                    #region Guarda Datos personales
                    foreach (DTO_drSolicitudDatosPersonales d in data.DatosPersonales)
                    {                       
                        #region Agrega nuevo
                        d.NumeroDoc.Value = data.DocCtrl.NumeroDoc.Value;
                        d.Version.Value = d.Version.Value + 1;
                        d.UsuarioDigita.Value = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId).ID.Value;
                        #region Valida si activa la solicitud de datacredito
                        bool solicitaDatacredito = false;
                        if (d.TipoPersona.Value == 1 && data.SolicituDocu.SolicitarDatacreditoDeudor) solicitaDatacredito = true;
                        else if (d.TipoPersona.Value == 2 && data.SolicituDocu.SolicitarDatacreditoCony) solicitaDatacredito = true;
                        else if (d.TipoPersona.Value == 3 && data.SolicituDocu.SolicitarDatacreditoCod1) solicitaDatacredito = true;
                        else if (d.TipoPersona.Value == 4 && data.SolicituDocu.SolicitarDatacreditoCod2) solicitaDatacredito = true;
                        else if (d.TipoPersona.Value == 5 && data.SolicituDocu.SolicitarDatacreditoCod3) solicitaDatacredito = true;
                        if (solicitaDatacredito)
                        {
                            d.DataCreditoRecibeInd.Value = null;
                            d.DataCreditoRecibeFecha.Value = null;
                            d.DataCreditoRecibeUsuario.Value = null;
                            cedulaSolicitaDatacredito.Add(d.TerceroID.Value);
                        } 
                        #endregion
                        d.Consecutivo.Value = this._dal_SolicitaDatosPersonales.DAL_drSolicitudDatosPersonales_Add(d);
                        #endregion
                        #region Valida si es codeudor para la solicitud
                        DTO_coTercero terExist = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, d.TerceroID.Value, true, false);
                        if (d.TipoPersona.Value == 2)// && terExist != null)
                            data.SolicituDocu.Codeudor1.Value = d.TerceroID.Value;
                        else if (d.TipoPersona.Value == 3)// && terExist != null)
                            data.SolicituDocu.Codeudor2.Value = d.TerceroID.Value;
                        else if (d.TipoPersona.Value == 4)// && terExist != null)
                            data.SolicituDocu.Codeudor3.Value = d.TerceroID.Value;
                        else if (d.TipoPersona.Value == 5)// && terExist != null)
                            data.SolicituDocu.Codeudor4.Value = d.TerceroID.Value;
                        else if (d.TipoPersona.Value == 6)// && terExist != null)
                            data.SolicituDocu.Codeudor5.Value = d.TerceroID.Value;
                        data.SolicituDocu.DesestimientoInd.Value = false;
                        data.SolicituDocu.NegociosGestionarInd.Value = false;
                        data.SolicituDocu.ActividadFlujoNegociosGestionarID.Value = null;
                        #endregion
                    }
                    #endregion
                    #region Guarda datos Vehiculo
                    //Valida si existe                  
                    if (data.DatosVehiculo.Modelo.Value.HasValue)
                    {
                        #region Agrega nuevo
                        data.DatosVehiculo.NumeroDoc.Value = data.DocCtrl.NumeroDoc.Value;
                        data.DatosVehiculo.Version.Value = data.DatosVehiculo.Version.Value + 1;
                        data.DatosVehiculo.Consecutivo.Value = this._dal_SolicitaDatosVehiculo.DAL_drSolicitudDatosVehiculo_Add(data.DatosVehiculo);
                        #endregion
                    }
                    #endregion
                    #region Guarda otros datos 
                        //Valida si existe                  
                        #region Agrega nuevo
                        data.OtrosDatos.NumeroDoc.Value = data.DocCtrl.NumeroDoc.Value;
                        data.OtrosDatos.Version.Value = data.OtrosDatos.Version.Value + 1;
                        data.OtrosDatos.Consecutivo.Value = this._dal_SolicitaDatosOtros.DAL_drSolicitudDatosOtros_Add(data.OtrosDatos);
                    #endregion
                    #endregion
                    #region Guarda Datacredito
                    List<DTO_ccSolicitudDataCreditoDatos> datos = this._dal_ccDatacreditoDatos.DAL_ccSolicitudDataCreditoDatos_GetByLastVersion(data.SolicituDocu.NumeroDoc.Value.Value);
                    List<DTO_ccSolicitudDataCreditoScore> score = this._dal_ccDatacreditoScore.DAL_ccSolicitudDataCreditoScore_GetByLastVersion(data.SolicituDocu.NumeroDoc.Value.Value);
                    List<DTO_ccSolicitudDataCreditoUbica> ubica = this._dal_SolicitaDatacredUbica.DAL_ccSolicitudDataCreditoUbica_GetByLastVersion(data.SolicituDocu.NumeroDoc.Value.Value);
                    List<DTO_ccSolicitudDataCreditoQuanto> quanto = this._dal_ccDatacreditoQuanto.DAL_ccSolicitudDataCreditoQuanto_GetByLastVersion(data.SolicituDocu.NumeroDoc.Value.Value);
                    #region Guarda o actualiza ccSolicitudDataCreditoDatos
                    foreach (DTO_ccSolicitudDataCreditoDatos d in datos)
                    {
                        #region Agrega nuevo
                        d.Version.Value = data.SolicituDocu.VersionNro.Value;
                        if(!cedulaSolicitaDatacredito.Contains(Convert.ToInt32(d.NumeroId.Value).ToString()))
                            d.Consecutivo.Value = this._dal_ccDatacreditoDatos.DAL_ccSolicitudDataCreditoDatos_Add(d);
                        #endregion
                    }
                    #endregion
                    #region Guarda o actualiza ccSolicitudDataCreditoScore
                    foreach (DTO_ccSolicitudDataCreditoScore d in score)
                    {                       
                        #region Agrega nuevo
                        d.Version.Value = data.SolicituDocu.VersionNro.Value;
                        if (!cedulaSolicitaDatacredito.Contains(Convert.ToInt32(d.NumeroId.Value).ToString()))
                            d.Consecutivo.Value = this._dal_ccDatacreditoScore.DAL_ccSolicitudDataCreditoScore_Add(d);
                        #endregion
                    }
                    #endregion
                    #region Guarda o actualiza ccSolicitudDataCreditoUbica
                    foreach (DTO_ccSolicitudDataCreditoUbica d in ubica)
                    {                      
                        #region Agrega nuevo
                        d.Version.Value = data.SolicituDocu.VersionNro.Value;
                        if (!cedulaSolicitaDatacredito.Contains(Convert.ToInt32(d.NumeroId.Value).ToString()))
                            d.Consecutivo.Value = this._dal_SolicitaDatacredUbica.DAL_ccSolicitudDataCreditoUbica_Add(d);
                        #endregion
                    }
                    #endregion
                    #region Guarda o actualiza ccSolicitudDataCreditoQuanto
                    foreach (DTO_ccSolicitudDataCreditoQuanto d in quanto)
                    {                       
                        #region Agrega nuevo
                        d.Version.Value = data.SolicituDocu.VersionNro.Value;
                        if (!cedulaSolicitaDatacredito.Contains(Convert.ToInt32(d.NumeroId.Value).ToString()))
                            d.Consecutivo.Value = this._dal_ccDatacreditoQuanto.DAL_ccSolicitudDataCreditoQuanto_Add(d);
                        #endregion
                    }
                    #endregion
                    #endregion

                    #region Actualiza glDocumentoChequeoLista

                    foreach (DTO_drSolicitudDatosChequeados d in data.DatosChequeados)
                    {
                        d.NumeroDoc.Value = data.SolicituDocu.NumeroDoc.Value;
                        d.Version.Value = data.SolicituDocu.VersionNro.Value;
                        //d.TipoPersona.Value = 1;
                        d.Consecutivo.Value=this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Add(d);
                            
                    }
                    #endregion 
                    
                    #region Asignar nuevo flujo

                    string ActFlujoDestimiento = this.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_Desestimiento);
                    string ActFlujoNegGestionar = this.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_NegGestionar);


                    if (string.IsNullOrWhiteSpace(data.SolicituDocu.ActividadFlujoNegociosGestionarID.Value) && data.SolicituDocu.DesestimientoInd.Value == false && data.SolicituDocu.NegociosGestionarInd.Value == false)
                    {
                        result = this.AsignarFlujo(documentoID, data.DocCtrl.NumeroDoc.Value.Value, actFlujoId, false, string.Empty);
                        data.SolicituDocu.DesestimientoInd.Value = false;
                        data.SolicituDocu.NegociosGestionarInd.Value = false;
                        data.SolicituDocu.ActividadFlujoNegociosGestionarID.Value = null;
                    }
                    else

                        if (data.SolicituDocu.NegociosGestionarInd.Value == true)
                        {
                            result = this.AsignarFlujo(documentoID, data.DocCtrl.NumeroDoc.Value.Value, actFlujoId, false, string.Empty, ActFlujoNegGestionar);
                            data.SolicituDocu.ActividadFlujoNegociosGestionarID.Value = actFlujoId;
                        }
                        else if (data.SolicituDocu.DesestimientoInd.Value == true)
                        {
                            result = this.AsignarFlujo(documentoID, data.DocCtrl.NumeroDoc.Value.Value, actFlujoId, false, string.Empty, ActFlujoDestimiento);
                            data.SolicituDocu.ActividadFlujoNegociosGestionarID.Value = null;
                        }
                        // else if (data.SolicituDocu.NegociosGestionarInd.Value == false)
                        else if (!string.IsNullOrWhiteSpace(data.SolicituDocu.ActividadFlujoNegociosGestionarID.Value))
                        {
                            result = this.AsignarFlujo(documentoID, data.DocCtrl.NumeroDoc.Value.Value, actFlujoId, false, string.Empty, data.SolicituDocu.ActividadFlujoNegociosGestionarID.Value);
                            data.SolicituDocu.ActividadFlujoNegociosGestionarID.Value = null;
                        }
                    #endregion
                }
                #region Actualiza Solicitud
                this._dalsolDocu.DAL_ccSolicitudDocu_Update(data.SolicituDocu);
                #endregion

                if (result.Result == ResultValue.OK)
                    result.ResultMessage = string.Empty;

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DigitacionCredito_Add");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!isAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !isAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Trae informacion de registros para solicitud de datacredito
        /// </summary>
        /// <returns>Lista de sol </returns>
        public DataTable drSolicitudDatosPersonales_GetForDatacredito()
        {
            this._dal_SolicitaDatosPersonales = (DAL_drSolicitudDatosPersonales)base.GetInstance(typeof(DAL_drSolicitudDatosPersonales), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            try
            {

                //Ruta documentos especiales
                string pathBasic = this.GetControlValue(AppControl.RutaDocumentosEspeciales);
                //Nombre del archivo
                string fileName = "\\SolicitudDatacredito.txt";
                string path = pathBasic + fileName;

                //Obtiene la info de datacredito
                DataTable results = this._dal_SolicitaDatosPersonales.DAL_drSolicitudDatosPersonales_GetForDatacredito();
                #region Crea el archivo plano con los resultados en la ubicacion obtenida
                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (DataRow row in results.Rows)
                    {
                        if (!string.IsNullOrEmpty(row["TerceroID"].ToString()))
                            sw.WriteLine(row["TerceroID"].ToString().TrimEnd());
                    }
                }
                #endregion

                return results;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "drSolicitudDatosPersonales_GetForDatacredito");
                throw ex;
            }

        }

        #endregion

        #region drActividadChequeoLista
        /// <summary>
        /// Trae la lista de chequeos de un flujo
        /// </summary>
        /// <param name="actividadFlujo">Flujo</param>
        /// <returns>Retorna una lista con la maestra de lista de chequo</returns>
        public List<DTO_drActividadChequeoLista> drActividadChequeoLista_GetByActividad(string actividadFlujo)
        {
            try
            {
                this._dal_ChequeoLista = (DAL_drActividadChequeoLista)base.GetInstance(typeof(DAL_drActividadChequeoLista), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_ChequeoLista.DAL_drActividadChequeoLista_GetByActividad(actividadFlujo);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "drActividadChequeoLista_GetByActividad");
                return null;
            }

        }

        #endregion
        #region drSolicitudDatosChequeados
        /// <summary>
        /// Trae la lista de chequeos de un flujo
        /// </summary>
        /// <param name="actividadFlujo">Flujo</param>
        /// <returns>Retorna una lista con la maestra de lista de chequo</returns>
        public List<DTO_drSolicitudDatosChequeados> drSolicitudDatosChequeados_GetByActividadNumDoc(string actividadFlujo, int numdoc, int version)
        {
            try
            {
                this._dal_DatosChequeados = (DAL_drSolicitudDatosChequeados)base.GetInstance(typeof(DAL_drSolicitudDatosChequeados), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_GetByActividadNumDoc(actividadFlujo, numdoc, version);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "drActividadChequeoLista_GetByActividad");
                return null;
            }

        }
        /// <summary>
        /// Guarda la lista de chequeos de un flujo
        /// </summary>
        /// <param name="actividadFlujo">Flujo</param>
        /// <returns>Retorna una lista con la maestra de lista de chequo</returns>
        public DTO_TxResult drSolicitudDatosChequeados_add(List<DTO_glDocumentoChequeoLista> chequeo, int numdoc, int version)
        {
            try
            {
                this._dal_DatosChequeados = (DAL_drSolicitudDatosChequeados)base.GetInstance(typeof(DAL_drSolicitudDatosChequeados), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_drSolicitudDatosChequeados DatosChequeado = new DTO_drSolicitudDatosChequeados();
                this._dal_DatosChequeados.DAL_drSolicitudDatosChequeados_Add(DatosChequeado);
                DTO_TxResult result = new DTO_TxResult()
                {
                    Result = ResultValue.OK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "drActividadChequeoLista_GetByActividad");
                DTO_TxResult result = new DTO_TxResult()
                {
                    Result = ResultValue.OK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                return result;
            }

        }


        #endregion
    }
}//NameSpace

