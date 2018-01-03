using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.ADO;
using System.Data.SqlClient;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using System.Reflection;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;
using NewAge.DTO.Reportes;
using System.Collections;
using System.Diagnostics;
using SentenceTransformer;
using System.IO;
using System.Data;

namespace NewAge.Negocio
{
    public class ModuloGlobal : ModuloBase
    {
        #region Variables

        #region Dals

        private DAL_ccAnexosLista _dal_ccAnexosLista = null;
        private DAL_ccCliente _dal_ccCliente = null;
        private DAL_ccChequeoLista _dal_ccChequeoLista = null;
        private DAL_ccCarteraComponente _dal_ccCarteraComponente = null;
        private DAL_ccClasificacionxRiesgo _dal_ccClasificacionxRiesgo = null;
        private DAL_coCargoCosto _dal_coCargoCosto = null;
        private DAL_coComprobantePrefijo _dal_coComprobantePrefijo = null;
        private DAL_coDocumento _dal_coDocumento = null;
        private DAL_coImpuesto _dal_coImpuesto = null;
        private DAL_coImpuestoLocal _dal_coImpuestoLocal = null;
        private DAL_ConsultasGenerales _dal_ConsultasGenerales = null;
        private DAL_glActividadEstado _dal_glActividadEstado = null;
        private DAL_glActividadFlujo _dal_glActividadFlujo = null;
        private DAL_glActividadPermiso _dal_glActividadPermiso = null;
        private DAL_glControl _dal_glControl = null;
        private DAL_glConsulta _dal_glConsulta = null;
        private DAL_glConsultaFiltro _dal_glConsultaFiltro = null;
        private DAL_glConsultaSeleccion _dal_glConsultaSeleccion = null;
        private DAL_glDocAnexoControl _dal_glDocAnexoControl = null;
        private DAL_glDocumentoAprueba _dal_glDocumentoAprueba = null;
        private DAL_glDocumentoControl _dal_glDocumentoControl = null;
        private DAL_glEmpresa _dal_glEmpresa = null;
        private DAL_glEmpresaGrupo _dal_glEmpresaGrupo = null;
        private DAL_glGarantiaControl _dal_garantiaControl;
        private DAL_glGestionDocumentalBitacora _dal_glGestionDocumentalBitacora = null;
        private DAL_glIncumpleCambioEstado _dal_IncumpleCambioEst;
        private DAL_glLlamadasControl _dal_glLlamadasControl = null;
        private DAL_glMovimientoDeta _dal_glMovimientoDeta = null;
        private DAL_glTabla _dal_glTabla = null;
        private DAL_glTasaDeCambio _dal_glTasaCambio = null;
        private DAL_MasterSimple _dal_MasterSimple = null;
        private DAL_MasterComplex _dal_MasterComplex = null;
        private DAL_ReportesGlobal _dal_reportesGlobal;
        private DAL_seDelegacionHistoria _dal_seDelegacionHistoria = null;
        private DAL_seGrupoDocumento _dal_seGrupoDocumento = null;
        private DAL_seMaquina _dal_seMaquina = null;
        private DAL_seUsuario _dal_seUsuario = null;
        private DAL_inMovimientoDocu _dal_inMovimientoDocu = null;
        private DAL_coTercero _dal_coTercero = null;
        private DAL_ccSolicitudDocu _dal_ccSolicitudDocu = null;
        private DAL_glActividadChequeoLista _dal_glActividadChequeo = null;
        private DAL_glDocumentoChequeoLista _dal_glDocumentoChequeoLista = null;

        #endregion

        #region Modulos

        private ModuloAplicacion _moduloAplicacion = null;
        private ModuloCartera _moduloCartera = null;
        private ModuloContabilidad _moduloContabilidad = null;
        private ModuloCuentasXPagar _moduloCuentasXPagar = null;
        private ModuloProveedores _moduloProveedores = null;
        private ModuloTesoreria _moduloTesoreria = null;
        private ModuloPlaneacion _moduloPlaneacion = null;
        private ModuloInventarios _moduloInventarios = null;

        #endregion

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Trae el numero de control para una empresa
        /// </summary>
        /// <returns>Retorna el numero de control para asignarlo a una empresa</returns>
        private int GetCompanyControlNum()
        {
            try
            {
                this._dal_glControl = (DAL_glControl)base.GetInstance(typeof(DAL_glControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glControl ctrl = this._dal_glControl.DAL_glControl_GetById(AppControl.ControlEmpresa);
                int ver = Convert.ToInt32(ctrl.Data.Value);
                int newVer = ver + 1;

                ctrl.Data.Value = newVer.ToString();
                this._dal_glControl.DAL_glControl_Update(ctrl);

                return newVer;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GetCompanyControlNum");
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// Constructor Fachada Maestras
        /// </summary>
        /// <param name="conn"></param>
        public ModuloGlobal(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        #region Maestras

        #region ccCarteraComponente

        /// <summary>
        /// Trae los componentes de  cartera dependiendo de la linea de credito
        /// </summary>
        /// <param name="lineaCreditoID">Linea de credito</param>
        /// <returns></returns>
        public List<DTO_ccCarteraComponente> ccCarteraComponente_GetByLineaCredito(string lineaCreditoID)
        {
            try
            {
                this._dal_ccCarteraComponente = (DAL_ccCarteraComponente)base.GetInstance(typeof(DAL_ccCarteraComponente), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_ccCarteraComponente.DAL_ccCarteraComponente_GetByLineaCredito(lineaCreditoID);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ccCarteraComponente_GetByLineaCredito");
                return null;
            }

        }

        #endregion

        #region ccClasificacionxRiesgo

        /// <summary>
        /// Obtiene un cliente seg;un el codigo
        /// </summary>
        /// <param name="codigo">Codigo del empleado</param>
        /// <returns>Retorna el cliente</returns>
        internal DTO_ccClasificacionxRiesgo ccClasificacionxRiesgo_GetByID(string claseCredito, int diasVencidos)
        {
            try
            {
                this._dal_ccClasificacionxRiesgo = (DAL_ccClasificacionxRiesgo)base.GetInstance(typeof(DAL_ccClasificacionxRiesgo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccClasificacionxRiesgo.DocumentID = AppMasters.ccClasificacionxRiesgo;
                return this._dal_ccClasificacionxRiesgo.DAL_ccClasificacionxRiesgo_GetByID(claseCredito, diasVencidos);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ccClasificacionxRiesgo_GetByID");
                return null;
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
        public DTO_TxResult ccCliente_Add(int documentoID, byte[] bItems, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            List<DTO_glDocumentoControl> ctrls = new List<DTO_glDocumentoControl>();
            try
            {
                //Variables
                string periodoStr = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
                DateTime periodo = Convert.ToDateTime(periodoStr);
                string mdaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string prefijoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string proyectoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string ctoCostoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string lugarGeoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);

                List<DTO_MasterBasic> items = CompressedSerializer.Decompress<List<DTO_MasterBasic>>(bItems);

                if (items == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "ERR_AGN_0007";
                }
                else
                {
                    this._dal_MasterSimple = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_coTercero = (DAL_coTercero)base.GetInstance(typeof(DAL_coTercero), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    for (int i = 0; i < items.Count; i++)
                    {
                        DTO_TxResultDetail txD = new DTO_TxResultDetail();

                        DTO_ccCliente cli = (DTO_ccCliente)items[i];

                        #region Crea el glDocumentoControl

                        DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                        ctrl.EmpresaID.Value = this.Empresa.ID.Value;
                        ctrl.DocumentoID.Value = AppDocuments.ControlIncumplimiento;
                        ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                        ctrl.Fecha.Value = DateTime.Now;
                        ctrl.PeriodoDoc.Value = periodo;
                        ctrl.PeriodoUltMov.Value = periodo;
                        ctrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                        ctrl.PrefijoID.Value = prefijoXDef;
                        ctrl.MonedaID.Value = mdaLocal;
                        ctrl.TasaCambioCONT.Value = 0;
                        ctrl.TasaCambioDOCU.Value = 0;
                        ctrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                        ctrl.TerceroID.Value = cli.TerceroID.Value;
                        ctrl.ProyectoID.Value = proyectoXDef;
                        ctrl.CentroCostoID.Value = ctoCostoXDef;
                        ctrl.LugarGeograficoID.Value = lugarGeoXDef;
                        ctrl.seUsuarioID.Value = this.UserId;

                        // Inserta el documento a la base de datos
                        txD = this.glDocumentoControl_Add(documentoID, ctrl, true);
                        if (txD.Message != ResultValue.OK.ToString())
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = "NOK";
                            result.Details.Add(txD);
                        }

                        ctrl.NumeroDoc.Value = Convert.ToInt32(txD.Key);
                        ctrls.Add(ctrl);
                        #endregion
                        #region Agrega el cliente

                        if (txD == null || txD.DetailsFields.Count == 0)
                        {
                            cli.DocumCobranza.Value = ctrl.NumeroDoc.Value;
                            this._dal_MasterSimple.DocumentID = AppMasters.ccCliente;
                            
                            txD = this._dal_MasterSimple.DAL_MasterSimple_AddItem(cli);
                            txD.line = i + 1;
                            if (txD != null && txD.DetailsFields.Count > 0)
                            {
                                result.Result = ResultValue.NOK;
                                result.Details.Add(txD);
                            }
                        }

                        #endregion
                        #region Actualiza contraseña Tercero
                        bool updOK =  this._dal_coTercero.DAL_coTercero_ResetPassword(cli.TerceroID.Value, cli.TerceroID.Value, true);
                        if (!updOK)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_UpdatePwd;
                            return result;
                        }
                        #endregion
                    }

                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ccCliente_Add");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                    
                        foreach(DTO_glDocumentoControl ctrl in ctrls)
                        {
                            ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentoID, ctrl.PrefijoID.Value);
                            this.ActualizaConsecutivos(ctrl, true, false, false);
                        }
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Actualiza un cliente
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult ccCliente_Update(DTO_ccCliente cliente, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                this._dal_MasterSimple = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Actualiza el tercero

                this._dal_MasterSimple.DocumentID = AppMasters.coTercero;

                UDT_BasicID udtId = new UDT_BasicID(){ Value = cliente.TerceroID.Value };
                DTO_coTercero tercero = (DTO_coTercero)this._dal_MasterSimple.DAL_MasterSimple_GetByID(udtId, false);
                tercero.ApellidoPri.Value = cliente.ApellidoPri.Value;
                tercero.ApellidoSdo.Value = cliente.ApellidoSdo.Value;
                tercero.NombrePri.Value = cliente.NombrePri.Value;
                tercero.NombreSdo.Value =  cliente.NombreSdo.Value;
                tercero.Descriptivo.Value = cliente.Descriptivo.Value;
                tercero.LugarGeograficoID.Value = cliente.ResidenciaCiudad.Value;
                tercero.Direccion.Value = cliente.ResidenciaDir.Value;
                tercero.Tel1.Value = cliente.Telefono.Value;
                tercero.Tel2.Value = cliente.Celular.Value;
                tercero.Telefono1.Value = cliente.Telefono1.Value;
                tercero.Telefono2.Value = cliente.Telefono2.Value;
                tercero.Extension1.Value = cliente.Extension1.Value;
                tercero.Extension2.Value = cliente.Extension2.Value;
                tercero.Celular1.Value =  cliente.Celular1.Value;
                tercero.Celular2.Value = cliente.Celular2.Value;
                tercero.CECorporativo.Value = cliente.Correo.Value;
                tercero.BancoID_1.Value = cliente.BancoID_1.Value;
                tercero.CuentaTipo_1.Value = cliente.CuentaTipo_1.Value;
                tercero.BcoCtaNro_1.Value = cliente.BcoCtaNro_1.Value;
                result = this._dal_MasterSimple.DAL_MasterSimple_Update(tercero, true);
                if (result.Result == ResultValue.NOK)
                    return result;

                #endregion
                #region Actualiza el cliente
                this._dal_MasterSimple.DocumentID = AppMasters.ccCliente;
                result = this._dal_MasterSimple.DAL_MasterSimple_Update(cliente, true);

                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ccCliente_Update");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                    }
                }
                else if (base._mySqlConnectionTx != null && base._mySqlConnectionTx.Connection != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Obtiene un cliente según el codigo
        /// </summary>
        /// <param name="codigo">Codigo del empleado</param>
        /// <returns>Retorna el cliente</returns>
        internal DTO_ccCliente ccCliente_GetClienteByCodigoEmpleado(string codigo)
        {
            try
            {
                this._dal_ccCliente = (DAL_ccCliente)base.GetInstance(typeof(DAL_ccCliente), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_ccCliente.DAL_ccCliente_GetClienteByCodigoEmpleado(codigo);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ccCliente_GetClienteByCodigoEmpleado");
                return null;
            }

        }

        /// <summary>
        ///  Adiciona una lista de clientes
        /// </summary>
        /// <param name="documentoID">Identifica el documento</param>
        /// <param name="data">data a guardar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResultDetail ccCliente_AddFromSource(int documentoID, object data, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            this._dal_MasterComplex = (DAL_MasterComplex)this.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_MasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            DTO_TxResultDetail result = new DTO_TxResultDetail();
            result.Message = ResultValue.OK.ToString();

            try
            {
                if (documentoID == AppDocuments.RegistroSolicitud || documentoID == AppDocuments.DigitacionCreditoFinanciera || documentoID == AppDocuments.VerificacionPreliminar)
                {
                    this._dal_ccSolicitudDocu = (DAL_ccSolicitudDocu)this.GetInstance(typeof(DAL_ccSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    DTO_DigitaSolicitudDecisor datos = (DTO_DigitaSolicitudDecisor)data;
                    DTO_ccPagaduria pag = (DTO_ccPagaduria)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, datos.SolicituDocu.PagaduriaID.Value, true, false);

                    #region Actualiza o crea el Tercero y Cliente
                    foreach (DTO_drSolicitudDatosPersonales persona in datos.DatosPersonales)
                    {
                        DTO_coTercero coTercero = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, persona.TerceroID.Value, true, false);
                        if (coTercero == null)
                        {
                            #region Crea el coTercero
                            coTercero = new DTO_coTercero();
                            coTercero.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                            coTercero.ID.Value = persona.TerceroID.Value;
                            coTercero.Descriptivo.Value = persona.ApellidoPri.Value + " " + persona.ApellidoSdo.Value + " " + persona.NombrePri.Value + " " + persona.NombreSdo.Value;
                            coTercero.ApellidoPri.Value = persona.ApellidoPri.Value;
                            coTercero.ApellidoSdo.Value = persona.ApellidoSdo.Value;
                            coTercero.NombrePri.Value = persona.NombrePri.Value;
                            coTercero.NombreSdo.Value = persona.NombreSdo.Value;
                            //coTercero.Direccion.Value = persona.DirResidencia.Value;
                            //coTercero.Tel1.Value = persona.TelResidencia.Value;
                            //coTercero.Telefono1.Value = persona.TelResidencia.Value;
                            //coTercero.Tel2.Value = persona.TelTrabajo.Value;
                            //coTercero.Telefono2.Value = persona.TelTrabajo.Value;
                            //coTercero.Celular1.Value = persona.Celular1.Value;
                            //coTercero.Celular2.Value = persona.Celular2.Value;
                            //coTercero.CECorporativo.Value = persona.CorreoElectronico.Value;
                            coTercero.LugarGeograficoID.Value = persona.CiudadResidencia.Value;
                            coTercero.ReferenciaID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_RegimenFiscalXDefecto);
                            //coTercero.ActEconomicaID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ActEconomicaXDef);
                            DTO_glLugarGeografico lugar = (DTO_glLugarGeografico)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, persona.CiudadResidencia.Value, true, false);
                            if (lugar == null)
                            {
                                result.DetailsFields = new List<DTO_TxResultDetailFields>();
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = "CiudadResidencia";
                                rdF.Message = "Ciudad no es válida, verifique";
                                result.Message = ResultValue.NOK.ToString();
                                result.DetailsFields.Add(rdF);
                                return result;
                            }
                            coTercero.Pais.Value = lugar.PaisID.Value;
                            coTercero.TerceroDocTipoID.Value = persona.TerceroDocTipoID.Value;
                            coTercero.AutoRetRentaInd.Value = false;
                            coTercero.AutoRetIVAInd.Value = false;
                            coTercero.DeclaraIVAInd.Value = false;
                            coTercero.DeclaraRentaInd.Value = false;
                            coTercero.ExcluyeCREEInd.Value = false;
                            coTercero.IndependienteEMPInd.Value = false;
                            coTercero.RadicaDirectoInd.Value = false;
                            coTercero.EmpleadoInd.Value = false;
                            coTercero.ProveedorInd.Value = false;
                            coTercero.SocioInd.Value = false;
                            coTercero.ClienteInd.Value = true;
                            if (persona.TipoPersona.Value == 1)
                            {
                                coTercero.ClienteInd.Value = true;
                                coTercero.BancoID_1.Value = datos.SolicituDocu.BancoID_1.Value;
                                coTercero.CuentaTipo_1.Value = datos.SolicituDocu.CuentaTipo_1.Value;
                                coTercero.BcoCtaNro_1.Value = datos.SolicituDocu.BcoCtaNro_1.Value;
                            }
                            else
                                coTercero.ClienteInd.Value = false;
                            coTercero.ActivoInd.Value = true;
                            coTercero.CtrlVersion.Value = 1;
                            coTercero.ActivoInd.Value = true;
                            this._dal_MasterSimple.DocumentID = AppMasters.coTercero;
                            result = this._dal_MasterSimple.DAL_MasterSimple_AddItem(coTercero);
                            if (result.Message == ResultValue.NOK.ToString())
                                return result;
                            if (persona.TipoPersona.Value == 2)
                                datos.SolicituDocu.Codeudor1.Value = persona.TerceroID.Value;
                            else if (persona.TipoPersona.Value == 3)
                                datos.SolicituDocu.Codeudor2.Value = persona.TerceroID.Value;
                            else if (persona.TipoPersona.Value == 4)
                                datos.SolicituDocu.Codeudor3.Value = persona.TerceroID.Value;
                            else if (persona.TipoPersona.Value == 5)
                                datos.SolicituDocu.Codeudor4.Value = persona.TerceroID.Value;
                            else if (persona.TipoPersona.Value == 6)
                                datos.SolicituDocu.Codeudor5.Value = persona.TerceroID.Value;
                            #endregion
                            #region Crea el ccCliente
                            if (persona.TipoPersona.Value == 1) // Si es deudor
                            {
                                DTO_ccCliente cli = new DTO_ccCliente();
                                cli.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                                cli.ID.Value = persona.TerceroID.Value;
                                cli.TerceroID.Value = persona.TerceroID.Value;
                                cli.Descriptivo.Value = persona.ApellidoPri.Value + " " + persona.ApellidoSdo.Value + " " + persona.NombrePri.Value + " " + persona.NombreSdo.Value;
                                cli.ApellidoPri.Value = persona.ApellidoPri.Value;
                                cli.ApellidoSdo.Value = persona.ApellidoSdo.Value;
                                cli.NombrePri.Value = persona.NombrePri.Value;
                                cli.NombreSdo.Value = persona.NombreSdo.Value;
                                cli.FechaExpDoc.Value = persona.FechaExpDoc.Value;
                                cli.FechaNacimiento.Value = persona.FechaNacimiento.Value;
                                //cli.Sexo.Value = persona.Sexo.Value;
                                //cli.EstadoCivil.Value = persona.EstadoCivil.Value.HasValue ? persona.EstadoCivil.Value : 0;
                                //cli.ResidenciaDir.Value = persona.DirResidencia.Value;
                                //cli.ResidenciaTipo.Value = persona.TipoVivienda.Value;   //validar consistencia de opciones
                                //cli.Barrio.Value = persona.BarrioResidencia.Value;
                                //cli.Telefono.Value = persona.TelResidencia.Value;
                                //cli.Telefono1.Value = persona.TelResidencia.Value;
                                //cli.Telefono2.Value = persona.TelTrabajo.Value;
                                //cli.TelefonoTrabajo.Value = persona.TelTrabajo.Value;
                                //cli.Celular.Value = persona.Celular1.Value;
                                //cli.Celular1.Value = persona.Celular1.Value;
                                //cli.Celular2.Value = persona.Celular2.Value;
                                //cli.ZonaID.Value = pag != null ? pag.ZonaID.Value : string.Empty;
                                //cli.LaboralDireccion.Value = !string.IsNullOrEmpty(persona.DirTrabajo.Value) ? persona.DirTrabajo.Value : "No Aplica";
                                //cli.Correo.Value = persona.CorreoElectronico.Value;
                                //cli.Cargo.Value = !string.IsNullOrEmpty(persona.Cargo.Value) ? persona.Cargo.Value : "No Aplica";
                                //cli.LaboralEntidad.Value = !string.IsNullOrEmpty(persona.LugarTrabajo.Value) ? persona.LugarTrabajo.Value : "No Aplica";
                                //cli.Antiguedad.Value = persona.AntTrabajo.Value;
                                cli.ClienteTipo.Value = 1;
                                cli.AsesorID.Value = datos.SolicituDocu.AsesorID.Value;
                                //cli.VlrDevengado.Value = persona.VlrIngresosMes.Value;
                                //cli.VlrDeducido.Value = persona.VlrEgresosMes.Value;
                                //cli.VlrActivos.Value = persona.VlrActivos.Value;
                                //cli.VlrPasivos.Value = persona.VlrPasivos.Value;
                                cli.VlrMesada.Value = 0;
                                cli.VlrConsultado.Value = 0;
                                cli.VlrOpera.Value = 0;
                                cli.ActivoInd.Value = true;
                                cli.CtrlVersion.Value = 1;
                                this._dal_MasterSimple.DocumentID = AppMasters.ccCliente;
                                result = this._dal_MasterSimple.DAL_MasterSimple_AddItem(cli);
                                if (result.Message == ResultValue.NOK.ToString())
                                    return result;
                            }
                            #endregion                            
                        }
                        else
                        {
                            #region Actualiza el coTercero
                            coTercero.Descriptivo.Value = persona.ApellidoPri.Value + " " + persona.ApellidoSdo.Value + " " + persona.NombrePri.Value + " " + persona.NombreSdo.Value;
                            coTercero.ApellidoPri.Value = persona.ApellidoPri.Value;
                            coTercero.ApellidoSdo.Value = persona.ApellidoSdo.Value;
                            coTercero.NombrePri.Value = persona.NombrePri.Value;
                            coTercero.NombreSdo.Value = persona.NombreSdo.Value;
                            //coTercero.Direccion.Value = persona.DirResidencia.Value;
                            //coTercero.Tel1.Value = persona.TelResidencia.Value;
                            //coTercero.Telefono1.Value = persona.TelResidencia.Value;
                            //coTercero.Tel2.Value = persona.TelTrabajo.Value;
                            //coTercero.Telefono2.Value = persona.TelTrabajo.Value;
                            //coTercero.Celular1.Value = persona.Celular1.Value;
                            //coTercero.Celular2.Value = persona.Celular2.Value;
                            //coTercero.CECorporativo.Value = persona.CorreoElectronico.Value;
                            coTercero.LugarGeograficoID.Value = persona.CiudadResidencia.Value;
                            coTercero.ReferenciaID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_RegimenFiscalXDefecto);
                            DTO_glLugarGeografico lugar = (DTO_glLugarGeografico)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, persona.CiudadResidencia.Value, true, false);
                            if (lugar == null)
                            {
                                result.DetailsFields = new List<DTO_TxResultDetailFields>();
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = "CiudadResidencia";
                                rdF.Message = "Ciudad no es válida, verifique";
                                result.Message = ResultValue.NOK.ToString();
                                result.DetailsFields.Add(rdF);
                                return result;
                            }
                            coTercero.Pais.Value = lugar.PaisID.Value;
                            coTercero.TerceroDocTipoID.Value = persona.TerceroDocTipoID.Value;
                            if (persona.TipoPersona.Value == 1)
                            {
                                coTercero.ClienteInd.Value = true;
                                coTercero.BancoID_1.Value = datos.SolicituDocu.BancoID_1.Value;
                                coTercero.CuentaTipo_1.Value = datos.SolicituDocu.CuentaTipo_1.Value;
                                coTercero.BcoCtaNro_1.Value = datos.SolicituDocu.BcoCtaNro_1.Value;
                            }
                            else
                                coTercero.ClienteInd.Value = false;
                            coTercero.ActivoInd.Value = true;
                            coTercero.CtrlVersion.Value = coTercero.CtrlVersion.Value++;
                            this._dal_MasterSimple.DocumentID = AppMasters.coTercero;
                            DTO_TxResult detailResult = this._dal_MasterSimple.DAL_MasterSimple_Update(coTercero, true);
                            if (detailResult.ResultMessage == ResultValue.NOK.ToString())
                                return result;
                            if (persona.TipoPersona.Value == 2)
                                datos.SolicituDocu.Codeudor1.Value = persona.TerceroID.Value;
                            else if (persona.TipoPersona.Value == 3)
                                datos.SolicituDocu.Codeudor2.Value = persona.TerceroID.Value;
                            else if (persona.TipoPersona.Value == 4)
                                datos.SolicituDocu.Codeudor3.Value = persona.TerceroID.Value;
                            else if (persona.TipoPersona.Value == 5)
                                datos.SolicituDocu.Codeudor4.Value = persona.TerceroID.Value;
                            else if (persona.TipoPersona.Value == 6)
                                datos.SolicituDocu.Codeudor5.Value = persona.TerceroID.Value;
                            #endregion
                            #region Actualiza el cliente
                            if (persona.TipoPersona.Value == 1) // Si es deudor
                            {
                                DTO_ccCliente cli = (DTO_ccCliente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, datos.SolicituDocu.ClienteRadica.Value, true, false);
                                if (cli != null)
                                {
                                    #region Actualiza Cliente
                                    cli.Descriptivo.Value = persona.ApellidoPri.Value + " " + persona.ApellidoSdo.Value + " " + persona.NombrePri.Value + " " + persona.NombreSdo.Value;
                                    cli.ApellidoPri.Value = persona.ApellidoPri.Value;
                                    cli.ApellidoSdo.Value = persona.ApellidoSdo.Value;
                                    cli.NombrePri.Value = persona.NombrePri.Value;
                                    cli.NombreSdo.Value = persona.NombreSdo.Value;
                                    cli.TerceroID.Value = persona.TerceroID.Value;
                                    cli.FechaExpDoc.Value = persona.FechaExpDoc.Value;
                                    cli.FechaNacimiento.Value = persona.FechaNacimiento.Value;
                                    cli.NacimientoCiudad.Value = persona.CiudadResidencia.Value;
                                    //cli.Sexo.Value = persona.Sexo.Value;
                                    //cli.EstadoCivil.Value = persona.EstadoCivil.Value;
                                    //cli.ResidenciaDir.Value = persona.DirResidencia.Value;
                                    //cli.ResidenciaTipo.Value = persona.TipoVivienda.Value;
                                    //cli.Barrio.Value = persona.BarrioResidencia.Value;
                                    //cli.Telefono.Value = persona.TelResidencia.Value;
                                    //cli.Telefono1.Value = persona.TelResidencia.Value;
                                    //cli.Telefono2.Value = persona.TelTrabajo.Value;
                                    //cli.TelefonoTrabajo.Value = persona.TelTrabajo.Value;
                                    //cli.Celular.Value = persona.Celular1.Value;
                                    //cli.Celular1.Value = persona.Celular1.Value;
                                    //cli.Celular2.Value = persona.Celular2.Value;
                                    //cli.LaboralDireccion.Value = !string.IsNullOrEmpty(persona.DirTrabajo.Value) ? persona.DirTrabajo.Value : "No Aplica";
                                    //cli.LaboralEntidad.Value = !string.IsNullOrEmpty(persona.LugarTrabajo.Value) ? persona.LugarTrabajo.Value : "No Aplica";
                                    //cli.Correo.Value = persona.CorreoElectronico.Value;
                                    //cli.Cargo.Value = !string.IsNullOrEmpty(persona.Cargo.Value) ? persona.Cargo.Value : "No Aplica";
                                    //cli.LaboralEntidad.Value = persona.LugarTrabajo.Value;
                                    //cli.Antiguedad.Value = persona.AntTrabajo.Value;
                                    cli.ClienteTipo.Value = 1;
                                    cli.AsesorID.Value = datos.SolicituDocu.AsesorID.Value;
                                    //cli.VlrDevengado.Value = persona.VlrIngresosMes.Value;
                                    //cli.VlrDeducido.Value = persona.VlrEgresosMes.Value;
                                    //cli.VlrActivos.Value = persona.VlrActivos.Value;
                                    //cli.VlrPasivos.Value = persona.VlrPasivos.Value;
                                    cli.VlrMesada.Value = 0;
                                    cli.VlrConsultado.Value = 0;
                                    cli.VlrOpera.Value = 0;
                                    cli.ActivoInd.Value = true;
                                    cli.CtrlVersion.Value = cli.CtrlVersion.Value++;
                                    this._dal_MasterSimple.DocumentID = AppMasters.ccCliente;
                                    detailResult = this._dal_MasterSimple.DAL_MasterSimple_Update(cli, true);
                                    if (detailResult.ResultMessage == ResultValue.NOK.ToString())
                                        return result;
                                    #endregion
                                }
                                else
                                {
                                    #region Nuevo ccCliente
                                    cli = new DTO_ccCliente();
                                    cli.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                                    cli.ID.Value = persona.TerceroID.Value;
                                    cli.Descriptivo.Value = coTercero.Descriptivo.Value;
                                    cli.ApellidoPri.Value = datos.SolicituDocu.ApellidoPri.Value;
                                    cli.ApellidoSdo.Value = datos.SolicituDocu.ApellidoSdo.Value;
                                    cli.NombrePri.Value = datos.SolicituDocu.NombrePri.Value;
                                    cli.NombreSdo.Value = datos.SolicituDocu.NombreSdo.Value;
                                    cli.TerceroID.Value = datos.SolicituDocu.ClienteRadica.Value;
                                    cli.FechaExpDoc.Value = persona.FechaExpDoc.Value;
                                    cli.FechaNacimiento.Value = persona.FechaNacimiento.Value;
                                    //cli.Sexo.Value = persona.Sexo.Value;
                                    cli.EstadoCivil.Value = persona.EstadoCivil.Value.HasValue ? persona.EstadoCivil.Value : 0;
                                    //cli.ResidenciaDir.Value = persona.DirResidencia.Value;
                                    //cli.ResidenciaTipo.Value = persona.TipoVivienda.Value;   //validar consistencia de opciones
                                    //cli.Barrio.Value = persona.BarrioResidencia.Value;
                                    //cli.Telefono.Value = persona.TelResidencia.Value;
                                    //cli.Telefono1.Value = persona.TelResidencia.Value;
                                    //cli.Telefono2.Value = persona.TelTrabajo.Value;
                                    //cli.TelefonoTrabajo.Value = persona.TelTrabajo.Value;
                                    //cli.Celular.Value = persona.Celular1.Value;
                                    //cli.Celular1.Value = persona.Celular1.Value;
                                    //cli.Celular2.Value = persona.Celular2.Value;
                                    //cli.ZonaID.Value = pag != null ? pag.ZonaID.Value : string.Empty;
                                    //cli.LaboralDireccion.Value = !string.IsNullOrEmpty(persona.DirTrabajo.Value) ? persona.DirTrabajo.Value : "No Aplica";
                                    //cli.Correo.Value = persona.CorreoElectronico.Value;
                                    //cli.Cargo.Value = !string.IsNullOrEmpty(persona.Cargo.Value) ? persona.Cargo.Value : "No Aplica";
                                    //cli.LaboralEntidad.Value = !string.IsNullOrEmpty(persona.LugarTrabajo.Value) ? persona.LugarTrabajo.Value : "No Aplica"; ;
                                    //cli.Antiguedad.Value = persona.AntTrabajo.Value;
                                    cli.ClienteTipo.Value = 1;
                                    cli.AsesorID.Value = datos.SolicituDocu.AsesorID.Value;
                                    //cli.VlrDevengado.Value = persona.VlrIngresosMes.Value;
                                    //cli.VlrDeducido.Value = persona.VlrEgresosMes.Value;
                                    //cli.VlrActivos.Value = persona.VlrActivos.Value;
                                    //cli.VlrPasivos.Value = persona.VlrPasivos.Value;
                                    cli.VlrMesada.Value = 0;
                                    cli.VlrConsultado.Value = 0;
                                    cli.VlrOpera.Value = 0;
                                    cli.ActivoInd.Value = true;
                                    cli.CtrlVersion.Value = 1;
                                    this._dal_MasterSimple.DocumentID = AppMasters.ccCliente;
                                    result = this._dal_MasterSimple.DAL_MasterSimple_AddItem(cli);
                                    if (result.Message == ResultValue.NOK.ToString())
                                        return result;
                                    #endregion
                                }
                            }

                            #endregion
                        }
                        #region Crea glTerceroReferencia
                        //if (!string.IsNullOrEmpty(persona.NombreReferencia1.Value))
                        //{
                        //    #region Referencia 1
                        //    DTO_glTerceroReferencia refCliente = new DTO_glTerceroReferencia();
                        //    refCliente.EmpresaGrupoID.Value = this.Empresa.EmpresaGrupoID_.Value;
                        //    refCliente.TerceroID.Value = persona.TerceroID.Value;
                        //    refCliente.TipoReferencia.Value = persona.TipoReferencia1.Value;
                        //    refCliente.Nombre.Value = persona.NombreReferencia1.Value;
                        //    refCliente.Relacion.Value = persona.RelReferencia1.Value;
                        //    refCliente.Direccion.Value = persona.DirReferencia1.Value;
                        //    refCliente.Barrio.Value = persona.BarrioReferencia1.Value;
                        //    refCliente.Telefono.Value = persona.TelefonoReferencia1.Value;
                        //    //refCliente.Ciudad.Value = persona..Value;
                        //    //refCliente.Correo.Value = persona.NombreReferencia1.Value;
                        //    refCliente.ActivoInd.Value = true;
                        //    refCliente.CtrlVersion.Value = 1;
                        //    refCliente.NuevoRegistro.Value = true;
                        //    this._dal_MasterComplex.DocumentID = AppMasters.glTerceroReferencia;

                        //    Dictionary<string, string> pks = new Dictionary<string, string>();
                        //    pks.Add("TerceroID", coTercero.ID.Value);
                        //    pks.Add("TipoReferencia", persona.TipoReferencia1.Value.ToString());
                        //    pks.Add("Nombre", persona.NombreReferencia1.Value);
                        //    //pks["TerceroID"] = coTercero.ID.Value;
                        //    //pks["TipoReferencia"] = persona.TipoReferencia2.Value.ToString();
                        //    //pks["Nombre"] = persona.NombreReferencia2.Value;
                        //    DTO_glTerceroReferencia terRefe = (DTO_glTerceroReferencia)this._dal_MasterComplex.DAL_MasterComplex_GetByID(pks, true);

                        //    if (terRefe == null)
                        //    {
                        //        DTO_TxResultDetail resultDetails = this._dal_MasterComplex.DAL_MasterComplex_AddItem(refCliente);
                        //    }
                        //    else
                        //        this._dal_MasterComplex.DAL_MasterComplex_Update(refCliente, true);

                        //    if (result.Message == ResultValue.NOK.ToString())
                        //        return result;
                        //    #endregion
                        //}
                        //if (!string.IsNullOrEmpty(persona.NombreReferencia2.Value))
                        //{
                        //    #region Referencia 2
                        //    DTO_glTerceroReferencia refCliente = new DTO_glTerceroReferencia();
                        //    refCliente.EmpresaGrupoID.Value = this.Empresa.EmpresaGrupoID_.Value;
                        //    refCliente.TerceroID.Value = persona.TerceroID.Value;
                        //    refCliente.TipoReferencia.Value = persona.TipoReferencia2.Value;
                        //    refCliente.Nombre.Value = persona.NombreReferencia2.Value;
                        //    refCliente.Relacion.Value = persona.RelReferencia2.Value;
                        //    refCliente.Direccion.Value = persona.DirReferencia2.Value;
                        //    refCliente.Barrio.Value = persona.BarrioReferencia2.Value;
                        //    refCliente.Telefono.Value = persona.TelefonoReferencia2.Value;
                        //    refCliente.ActivoInd.Value = true;
                        //    refCliente.CtrlVersion.Value = 1;
                        //    refCliente.NuevoRegistro.Value = true;
                        //    this._dal_MasterComplex.DocumentID = AppMasters.glTerceroReferencia;
                        //    Dictionary<string, string> pks = new Dictionary<string, string>();
                        //    pks["TerceroID"] = coTercero.ID.Value;
                        //    pks["TipoReferencia"] = persona.TipoReferencia2.Value.ToString();
                        //    pks["Nombre"] = persona.NombreReferencia2.Value;
                        //    DTO_glTerceroReferencia terRefe = (DTO_glTerceroReferencia)this._dal_MasterComplex.DAL_MasterComplex_GetByID(pks, true);

                        //    if (terRefe == null)
                        //    {
                        //        DTO_TxResultDetail resultDetails = this._dal_MasterComplex.DAL_MasterComplex_AddItem(refCliente);
                        //    }
                        //    else
                        //        this._dal_MasterComplex.DAL_MasterComplex_Update(refCliente, true);

                        //    if (result.Message == ResultValue.NOK.ToString())
                        //        return result;
                        //    #endregion
                        //}
                        #endregion    
                    }
                    #region Actualiza la solicitud
                    datos.SolicituDocu.ClienteID.Value = datos.SolicituDocu.ClienteRadica.Value;
                    this._dal_ccSolicitudDocu.DAL_ccSolicitudDocu_Update(datos.SolicituDocu);
                    #endregion
                    #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ccCliente_Add");
                return result;
            }
            finally
            {
                if (result.Message == ResultValue.OK.ToString())
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
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
        public DTO_TxResult ccFasecolda_Migracion(byte[] fasecoldasItems, byte[] modelosItems, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            this._dal_MasterComplex = (DAL_MasterComplex)this.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_MasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, AppProcess.GestionFasecolda);
            batchProgress[tupProgress] = 1;

            List<DTO_ccFasecolda> fasecoldas = CompressedSerializer.Decompress<List<DTO_ccFasecolda>>(fasecoldasItems);
            List<DTO_ccFasecoldaModelo> modelos = CompressedSerializer.Decompress<List<DTO_ccFasecoldaModelo>>(modelosItems);

            int count = fasecoldas.Count + modelos.Count;
            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / (count != 0 ? count : 1);

            try
            {
   
                #region Crea Fasecolda
                foreach (DTO_ccFasecolda fase in fasecoldas)
                {
                    DTO_ccFasecolda fasecolda = (DTO_ccFasecolda)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccFasecolda, fase.ID.Value, true, false);
                    if (fasecolda == null)
                    {
                        #region Crea registro     
                        fase.CtrlVersion.Value = 1;
                        fase.ActivoInd.Value = true;
                        this._dal_MasterSimple.DocumentID = AppMasters.ccFasecolda;
                        DTO_TxResultDetail resultDet = this._dal_MasterSimple.DAL_MasterSimple_AddItem(fase);
                        if (resultDet.Message == ResultValue.NOK.ToString())
                        {
                            result.Details.Add(resultDet);
                            return result;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Actualiza registro
                        fase.ReplicaID.Value = fasecolda.ReplicaID.Value;
                        fase.EmpresaGrupoID.Value = fasecolda.EmpresaGrupoID.Value;
                        fase.ActivoInd.Value = true;
                        fase.CtrlVersion.Value = fasecolda.CtrlVersion.Value++;
                        this._dal_MasterSimple.DocumentID = AppMasters.ccFasecolda;
                        result = this._dal_MasterSimple.DAL_MasterSimple_Update(fase, true);
                        if (result.Result == ResultValue.NOK)
                        {
                            return result;
                        } 
                        #endregion
                    }
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                }
                #endregion
                this._dal_MasterComplex.DocumentID = AppMasters.ccFasecoldaModelo;
                this._dal_MasterComplex.DAL_MasterComplex_DeleteAll(true);

                #region Crea FasecoldaModelo
                foreach (DTO_ccFasecoldaModelo fase in modelos)
                {                    
                    #region Crea registro     
                    fase.CtrlVersion.Value = 1;
                    fase.ActivoInd.Value = true;
                    DTO_TxResultDetail resultDet = this._dal_MasterComplex.DAL_MasterComplex_AddItem(fase);
                    if (resultDet.Message == ResultValue.NOK.ToString())
                    {
                        result.Details.Add(resultDet);
                        return result;
                    }
                    #endregion
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                }
                #endregion

                batchProgress[tupProgress] = 100;
                result.Details = new List<DTO_TxResultDetail>();
                return result;
            }
            catch (Exception ex)
            {
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ccCliente_Add");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
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
                this._dal_ccChequeoLista = (DAL_ccChequeoLista)base.GetInstance(typeof(DAL_ccChequeoLista), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_ccChequeoLista.DAL_ccChequeoLista_GetByDocumento(documentoID);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ccPagaduriaAnexos_GetByPagaduria");
                return null;
            }

        }

        #endregion

        #region ccPagaduriaAnexos

        /// <summary>
        /// Trae los documentos anexos dependiendo de la pagaduria
        /// </summary>
        /// <param name="pagaduriaID">Id de la pagaduria</param>
        /// <returns></returns>
        public List<DTO_MasterBasic> ccPagaduriaAnexos_GetByPagaduria(string pagaduriaID)
        {
            try
            {
                this._dal_ccAnexosLista = (DAL_ccAnexosLista)base.GetInstance(typeof(DAL_ccAnexosLista), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_ccAnexosLista.DAL_ccAnexosLista_GetByPagaduria(pagaduriaID);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ccPagaduriaAnexos_GetByPagaduria");
                return null;
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
        public string coCargoCosto_GetCuentaIDByCargoOper(string ConceptoCargoID, string operID, string lineaPresID)
        {
            try
            {
                this._dal_coCargoCosto = (DAL_coCargoCosto)base.GetInstance(typeof(DAL_coCargoCosto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_coCargoCosto.DAL_coCargoCosto_GetCuentaIDByCargoOper(ConceptoCargoID, operID, lineaPresID);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "coCargoCosto_GetCuentaIDByCargoOper");
                return null;
            }
        }

        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="ConceptoCargoID">Identificador del concepto de cargo</param>
        /// <param name="operacionID">Identificador de la operacion</param>
        /// <param name="lineaPresID">Identificador de la linea presupuestal</param>
        /// <returns>Retorna una cuenta o null si no existe</returns>
        public string coCargoCosto_GetCuentaIDByCargoOper(string ConceptoCargoID, string proyID, string ctoCostoID, string lineaPresID)
        {
            try
            {
                string operID = string.Empty;
                if (!string.IsNullOrWhiteSpace(proyID))
                {
                    DTO_coProyecto proy = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, proyID, true, false);
                    operID = proy.OperacionID.Value;
                }
                else
                {
                    DTO_coCentroCosto ctoCosto = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, ctoCostoID, true, false);
                    operID = ctoCosto.OperacionID.Value;
                }

                //De ser necesario trae la linea presupuestal por defecto
                DTO_plLineaPresupuesto lp = (DTO_plLineaPresupuesto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, lineaPresID, true, false);
                if (lp != null && lp.TablaControlInd.Value.Value)
                    lineaPresID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);

                this._dal_coCargoCosto = (DAL_coCargoCosto)base.GetInstance(typeof(DAL_coCargoCosto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_coCargoCosto.DAL_coCargoCosto_GetCuentaIDByCargoOper(ConceptoCargoID, operID, lineaPresID);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "coCargoCosto_GetCuentaIDByCargoOper");
                return null;
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
                bool updVal = valor >= 0 ? false : true;
                valor = Math.Abs(valor);

                DTO_CuentaValor result = null;

                //De ser necesario trae la linea presupuestal por defecto
                DTO_plLineaPresupuesto lp = (DTO_plLineaPresupuesto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, lineaPresID, true, false);
                if (lp != null && lp.TablaControlInd.Value.Value)
                    lineaPresID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);

                this._dal_coCargoCosto = (DAL_coCargoCosto)base.GetInstance(typeof(DAL_coCargoCosto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                object obj = this._dal_coCargoCosto.DAL_coCargoCosto_GetCuentaByCargoOper(conceptoCargoID, operacionID, lineaPresID, valor);

                if (obj != null)
                {
                    result = (DTO_CuentaValor)obj;
                    result.Base.Value = 0;

                    #region Llena la info de la cuenta
                    DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, result.CuentaID.Value, true, false);
                    NaturalezaCuenta nat = (NaturalezaCuenta)Enum.Parse(typeof(NaturalezaCuenta), cta.Naturaleza.Value.Value.ToString());

                    if (nat == NaturalezaCuenta.Credito)
                        valor *= -1;

                    result.Valor.Value = valor;
                    #endregion

                    //Actualiza los valores
                    if (updVal)
                        result.Valor.Value = result.Valor.Value * -1;
                }

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "coCargoCosto_GetCuentaByCargoOper");
                return null;
            }
        }

        #endregion

        #region coComprobantePrefijo

        /// <summary>
        /// Trae el identificador de un comprobante segun el documento y el prefijo
        /// </summary>
        /// <param name="documentoIDExt">coDocumentoID del documento de la PK</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="compAnulacion">Indica si debe traer el comprobante de anulacion</param>
        /// <returns>Retorna el identificador de un comprobante o null si no existe</returns>
        public string coComprobantePrefijo_GetComprobanteByDocPref(int coDocumentoID, string prefijoID, bool compAnulacion)
        {
            this._dal_coComprobantePrefijo = (DAL_coComprobantePrefijo)base.GetInstance(typeof(DAL_coComprobantePrefijo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_coComprobantePrefijo.DAL_coComprobantePrefijo_GetComprobanteByDocPref(coDocumentoID, prefijoID, compAnulacion);
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
            this._dal_coDocumento = (DAL_coDocumento)base.GetInstance(typeof(DAL_coDocumento), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_coDocumento.DAL_coDocumento_PrefijoExists(prefijoID);
        }

        #endregion

        #region coImpuesto

        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="valor">Valor sobre el cual se esta trabajando</param>
        /// <param name="conceptoCargoID">Identificador del concepto de cargo</param>
        /// <param name="regFisTerceroID">Identificador del regimen fiscal del tercero</param>
        /// <param name="lugarGeoID">Identificador del lugar geografico</param>
        /// <param name="porcentaje">Porcentaje sobre el cual se va a calcular la base</param>
        /// <returns>Retorna una lista de tuplas <Cuenta,TipoImpuesto> </returns>
        internal List<DTO_SerializedObject> coImpuesto_GetCuentasByPK(ModulesPrefix mod, DTO_coTercero tercero, string conceptoCargoID, string lugarGeoID, decimal valor, string conceptoCargo2)
        {
            List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();

            try
            {
                this._dal_coImpuesto = (DAL_coImpuesto)base.GetInstance(typeof(DAL_coImpuesto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables
                string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);

                //Asigna el lugar geografico por defecto
                lugarGeoID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);

                //Trae el regimen fiscal de la empresa
                DTO_coTercero terceroEmp = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, terceroPorDefecto, true, false);
                string regFisEmpID = terceroEmp.ReferenciaID.Value;
                string regFisTerceroID = tercero.ReferenciaID.Value;

                //Trae los datos de glControl
                string tipoImpReteICA = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA);
                string tipoImpReteIVA = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteIVA);
                string tipoImpReteFte = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente);
                string tipoImpIVA = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA);
                string lugGeoImp = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);

                //Periodo
                DateTime p = Convert.ToDateTime(this.GetControlValueByCompany(mod, AppControl.co_Periodo));
                DateTime periodo = new DateTime(p.Year, 1, 1);

                //Generales
                long count;
                DTO_glConsulta query;
                List<DTO_glConsultaFiltro> filtros;
                Dictionary<string, string> impPks;
                DTO_coImpuesto impDTO;

                Dictionary<string, DTO_coPlanCuenta> cacheCtas = new Dictionary<string, DTO_coPlanCuenta>();
                DTO_coPlanCuenta ctaDTO;
                #endregion
                #region Trea la info de las retenciones (retefuente - UVT)

                DAL_MasterComplex dalComplex = new DAL_MasterComplex(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                dalComplex.DocumentID = AppMasters.noReteFuenteBasica;

                List<DTO_MasterComplex> ienum = (dalComplex.DAL_MasterComplex_GetPaged(10, 1, null, true)).ToList();
                List<DTO_noReteFuenteBasica> listReteFtes = ienum.Cast<DTO_noReteFuenteBasica>().ToList();

                #endregion
                #region Revisa si distribuye ICA
                bool distribuye = false;
                if (!string.IsNullOrWhiteSpace(lugarGeoID))
                {
                    DTO_glLugarGeografico lg = (DTO_glLugarGeografico)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLugarGeografico, lugarGeoID, true, false);
                    if (lg.DistribuyeInd.Value.Value)
                        distribuye = true;
                }
                #endregion
                #region Agrega los impuestos extranjeros
                List<string> ctasIds = this._dal_coImpuesto.DAL_coImpuesto_GetCuentasByPK(tercero, conceptoCargoID, lugarGeoID, regFisEmpID, tipoImpReteIVA, tipoImpReteFte);
                if (!string.IsNullOrEmpty(conceptoCargo2))
                {
                    Dictionary<string, string> keyscoImpuesto = new Dictionary<string, string>();
                    keyscoImpuesto.Add("RegimenFiscalEmpresaID", regFisEmpID);
                    keyscoImpuesto.Add("RegimenFiscalTerceroID", regFisTerceroID);
                    keyscoImpuesto.Add("LugarGeograficoID", lugarGeoID);
                    keyscoImpuesto.Add("ConceptoCargoID", conceptoCargo2);
                    keyscoImpuesto.Add("ImpuestoTipoID", tipoImpIVA);
                    DTO_coImpuesto iva2 = (DTO_coImpuesto)this.GetMasterComplexDTO(AppMasters.coImpuesto, keyscoImpuesto, true);
                    if (iva2 != null)
                        ctasIds.Add(iva2.CuentaID.Value);
                }

                foreach (string ctaId in ctasIds)
                {
                    if (cacheCtas.ContainsKey(ctaId))
                        ctaDTO = cacheCtas[ctaId];
                    else
                    {
                        ctaDTO = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, ctaId, true, false);
                        cacheCtas.Add(ctaId, ctaDTO);
                    }

                    if (!distribuye || ctaDTO.ImpuestoTipoID.Value != tipoImpReteICA)
                    {
                        DTO_SerializedObject ctaVal = this.Impuesto_LlenaInfoCuenta(ctaDTO, valor, lugarGeoID, periodo, tercero.RadicaDirectoInd.Value.Value, tipoImpReteFte, listReteFtes);
                        if (ctaVal.GetType() == typeof(DTO_TxResult))
                        {
                            results = new List<DTO_SerializedObject>();
                            results.Add(ctaVal);
                            return results;
                        }

                        results.Add((DTO_CuentaValor)ctaVal);
                    }
                }
                #endregion
                #region Distribucion de ICA
                if (distribuye)
                {
                    DAL_MasterComplex dalDistribuye = (DAL_MasterComplex)this.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    dalDistribuye.DocumentID = AppMasters.cpDistribuyeImpLocal;

                    #region Carga el filtro del lugar geografico
                    filtros = new List<DTO_glConsultaFiltro>();
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "LugarGeograficoORI",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = lugarGeoID,
                        OperadorSentencia = "AND"
                    });
                    query = new DTO_glConsulta();
                    query.DocumentoID = AppMasters.cpDistribuyeImpLocal;
                    query.Filtros = filtros;
                    #endregion
                    count = dalDistribuye.DAL_MasterComplex_Count(query, true);
                    if (count > 0)
                    {
                        #region Carga los lugares de distribucion
                        var lugares = dalDistribuye.DAL_MasterComplex_GetPaged(count, 1, query, true);
                        decimal total = 0;
                        foreach (var lg in lugares)
                        {
                            DTO_cpDistribuyeImpLocal distr = (DTO_cpDistribuyeImpLocal)lg;
                            total += distr.Porcentaje.Value.Value;

                            impPks = new Dictionary<string, string>();
                            impPks.Add("RegimenFiscalEmpresaID", regFisEmpID);
                            impPks.Add("RegimenFiscalTerceroID", regFisTerceroID);
                            impPks.Add("ImpuestoTipoID", tipoImpReteICA);
                            impPks.Add("ConceptoCargoID", conceptoCargoID);
                            impPks.Add("LugarGeograficoID", distr.LugarGeograficoID.Value);

                            impDTO = (DTO_coImpuesto)this.GetMasterComplexDTO(AppMasters.coImpuesto, impPks, true);
                            if (impDTO != null)
                            {
                                if (cacheCtas.ContainsKey(impDTO.CuentaID.Value))
                                    ctaDTO = cacheCtas[impDTO.CuentaID.Value];
                                else
                                {
                                    ctaDTO = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, impDTO.CuentaID.Value, true, false);
                                    cacheCtas.Add(impDTO.CuentaID.Value, ctaDTO);
                                }

                                decimal valorTemp = Math.Round(valor * distr.Porcentaje.Value.Value / 100, 2);
                                DTO_SerializedObject ctaVal = this.Impuesto_LlenaInfoCuenta(ctaDTO, valorTemp, distr.LugarGeograficoID.Value, periodo, tercero.RadicaDirectoInd.Value.Value, tipoImpReteFte, listReteFtes);
                                if (ctaVal.GetType() == typeof(DTO_TxResult))
                                {
                                    results = new List<DTO_SerializedObject>();
                                    results.Add(ctaVal);
                                    return results;
                                }

                                results.Add((DTO_CuentaValor)ctaVal);
                            }
                        }

                        if (total != 100)
                        {
                            DTO_TxResult result = new DTO_TxResult();
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Co_DistPorcLugGeo + "&&" + lugarGeoID;

                            results = new List<DTO_SerializedObject>();
                            results.Add(result);
                            return results;
                        }

                        #endregion
                    }
                }
                #endregion

                return results;
            }
            catch (Exception ex)
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "coImpuesto_GetCuentasByPK");

                results = new List<DTO_SerializedObject>();
                results.Add(result);
                return results;
            }
        }

        #endregion

        #region coImpuestoLocal

        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="valor">Valor sobre el cual se esta trabajando</param>
        /// <param name="actEconomicaID">Identificador de la actividad economica</param>
        /// <param name="regFisTerceroID">Identificador del regimen fiscal del tercero</param>
        /// <param name="lugarGeoID">Identificador del lugar geografico</param>
        /// <param name="porcentaje">Porcentaje sobre el cual se va a calcular la base</param>
        /// <returns>Retorna una lista de tuplas <Cuenta,TipoImpuesto> </returns>
        internal List<DTO_SerializedObject> coImpuestoLocal_GetCuentasByPK(ModulesPrefix mod, DTO_coTercero tercero, string actEconomicaID, string lugarGeoID, decimal valor)
        {
            List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();

            try
            {
                string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                this._dal_coImpuestoLocal = (DAL_coImpuestoLocal)base.GetInstance(typeof(DAL_coImpuestoLocal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables

                //Trae el regimen fiscal de la empresa
                DTO_coTercero terceroEmp = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, terceroPorDefecto, true, false);
                string regFisEmpID = terceroEmp.ReferenciaID.Value;
                string regFisTerceroID = tercero.ReferenciaID.Value;

                //Trae los datos de glControl
                string tipoImpReteICA = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA);
                string tipoImpReteIVA = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteIVA);
                string tipoImpReteFte = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente);
                string lugGeoImp = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);

                //Periodo
                DateTime p = Convert.ToDateTime(this.GetControlValueByCompany(mod, AppControl.co_Periodo));
                DateTime periodo = new DateTime(p.Year, 1, 1);

                //Generales
                long count;
                DTO_glConsulta query;
                List<DTO_glConsultaFiltro> filtros;
                Dictionary<string, string> impPks;
                DTO_coImpuestoLocal impDTO;

                Dictionary<string, DTO_coPlanCuenta> cacheCtas = new Dictionary<string, DTO_coPlanCuenta>();
                DTO_coPlanCuenta ctaDTO;
                #endregion
                #region Trea la info de las retenciones (retefuente - UVT)

                DAL_MasterComplex dalComplex = new DAL_MasterComplex(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                dalComplex.DocumentID = AppMasters.noReteFuenteBasica;

                List<DTO_MasterComplex> ienum = (dalComplex.DAL_MasterComplex_GetPaged(10, 1, null, true)).ToList();
                List<DTO_noReteFuenteBasica> listReteFtes = ienum.Cast<DTO_noReteFuenteBasica>().ToList();

                #endregion
                #region Revisa si distribuye ICA
                bool distribuye = false;
                if (!string.IsNullOrWhiteSpace(lugarGeoID))
                {
                    DTO_glLugarGeografico lg = (DTO_glLugarGeografico)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLugarGeografico, lugarGeoID, true, false);
                    if (lg.DistribuyeInd.Value.Value)
                        distribuye = true;
                }
                #endregion
                #region Agrega los impuestos extranjeros
                List<string> ctasIds = this._dal_coImpuestoLocal.DAL_coImpuestoLocal_GetCuentasByPK(tercero, actEconomicaID, lugarGeoID, regFisEmpID, tipoImpReteIVA, tipoImpReteFte);
                foreach (string ctaId in ctasIds)
                {
                    if (cacheCtas.ContainsKey(ctaId))
                        ctaDTO = cacheCtas[ctaId];
                    else
                    {
                        ctaDTO = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, ctaId, true, false);
                        cacheCtas.Add(ctaId, ctaDTO);
                    }

                    if (!distribuye || ctaDTO.ImpuestoTipoID.Value != tipoImpReteICA)
                    {
                        DTO_SerializedObject ctaVal = this.Impuesto_LlenaInfoCuenta(ctaDTO, valor, lugarGeoID, periodo, tercero.RadicaDirectoInd.Value.Value, tipoImpReteFte, listReteFtes);
                        if (ctaVal.GetType() == typeof(DTO_TxResult))
                        {
                            results = new List<DTO_SerializedObject>();
                            results.Add(ctaVal);
                            return results;
                        }

                        results.Add((DTO_CuentaValor)ctaVal);
                    }
                }
                #endregion
                #region Distribucion de ICA
                if (distribuye)
                {
                    DAL_MasterComplex dalDistribuye = (DAL_MasterComplex)this.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    dalDistribuye.DocumentID = AppMasters.cpDistribuyeImpLocal;

                    #region Carga el filtro del lugar geografico
                    filtros = new List<DTO_glConsultaFiltro>();
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "LugarGeograficoORI",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = lugarGeoID,
                        OperadorSentencia = "AND"
                    });
                    query = new DTO_glConsulta();
                    query.DocumentoID = AppMasters.cpDistribuyeImpLocal;
                    query.Filtros = filtros;
                    #endregion
                    count = dalDistribuye.DAL_MasterComplex_Count(query, true);
                    if (count > 0)
                    {
                        #region Carga los lugares de distribucion
                        var lugares = dalDistribuye.DAL_MasterComplex_GetPaged(count, 1, query, true);
                        decimal total = 0;
                        foreach (var lg in lugares)
                        {
                            DTO_cpDistribuyeImpLocal distr = (DTO_cpDistribuyeImpLocal)lg;
                            total += distr.Porcentaje.Value.Value;

                            impPks = new Dictionary<string, string>();
                            impPks.Add("RegimenFiscalEmpresaID", regFisEmpID);
                            impPks.Add("RegimenFiscalTerceroID", regFisTerceroID);
                            impPks.Add("ImpuestoTipoID", tipoImpReteICA);
                            impPks.Add("ActEconomicaID", actEconomicaID);
                            impPks.Add("LugarGeograficoID", distr.LugarGeograficoID.Value);

                            impDTO = (DTO_coImpuestoLocal)this.GetMasterComplexDTO(AppMasters.coImpuestoLocal, impPks, true);
                            if (impDTO != null)
                            {
                                if (cacheCtas.ContainsKey(impDTO.CuentaID.Value))
                                    ctaDTO = cacheCtas[impDTO.CuentaID.Value];
                                else
                                {
                                    ctaDTO = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, impDTO.CuentaID.Value, true, false);
                                    cacheCtas.Add(impDTO.CuentaID.Value, ctaDTO);
                                }

                                decimal valorTemp = Math.Round(valor * distr.Porcentaje.Value.Value / 100, 2);
                                DTO_SerializedObject ctaVal = this.Impuesto_LlenaInfoCuenta(ctaDTO, valorTemp, distr.LugarGeograficoID.Value, periodo, tercero.RadicaDirectoInd.Value.Value, tipoImpReteFte, listReteFtes);
                                if (ctaVal.GetType() == typeof(DTO_TxResult))
                                {
                                    results = new List<DTO_SerializedObject>();
                                    results.Add(ctaVal);
                                    return results;
                                }

                                results.Add((DTO_CuentaValor)ctaVal);
                            }
                        }

                        if (total != 100)
                        {
                            DTO_TxResult result = new DTO_TxResult();
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Co_DistPorcLugGeo + "&&" + lugarGeoID;

                            results = new List<DTO_SerializedObject>();
                            results.Add(result);
                            return results;
                        }

                        #endregion
                    }
                }
                #endregion

                return results;
            }
            catch (Exception ex)
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "coImpuestoLocal_GetCuentasByPK");

                results = new List<DTO_SerializedObject>();
                results.Add(result);
                return results;
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
                List<decimal> results = new List<decimal>();

                DAL_coPlanCuenta dalCta = new DAL_coPlanCuenta(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string impTipoIVA = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA);
                results = dalCta.DAL_coPlanCuenta_TarifasImpuestos(impTipoIVA);

                return results;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "coPlanCuenta_TarifasImpuestos");
                return null;
            }
        }

        /// <summary>
        /// Trae la cuenta de la cuenta alterna
        /// </summary>
        /// <param name="id">Identificador de la maestra</param>
        /// <param name="active">indica si el registro debe estar activo</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Retorna un registro de la cuenta alterna</returns>
        public DTO_coPlanCuenta coPlanCuenta_GetCuentaAlterna(int documentID, UDT_BasicID id, bool active, List<DTO_glConsultaFiltro> filtros)
        {
            try
            {
                DTO_coPlanCuenta ctaAlt = null;

                string empCorpID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_EmpresaCorporativa);
                if (!string.IsNullOrWhiteSpace(empCorpID))
                {
                    DTO_glEmpresa emp = (DTO_glEmpresa)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glEmpresa, empCorpID, true, false);
                    if (emp != null)
                    {
                        DAL_MasterHierarchy hierarchyDAL = new DAL_MasterHierarchy(this._mySqlConnection, this._mySqlConnectionTx, emp, this.UserId, this.loggerConnectionStr);
                        hierarchyDAL.DocumentID = documentID;

                        DTO_MasterBasic basicDTO = hierarchyDAL.DAL_MasterSimple_GetByID(id, true, filtros);
                        if (basicDTO != null)
                        {
                            DTO_MasterHierarchyBasic hierarchyDTO = hierarchyDAL.CompleteHierarchy(basicDTO);
                            ctaAlt = (DTO_coPlanCuenta)hierarchyDTO;
                        }
                    }
                }
                return ctaAlt;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "coPlanCuenta_GetCuentaAlterna");
                return null;
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
                long count = 0;

                string empCorpID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_EmpresaCorporativa);
                if (!string.IsNullOrWhiteSpace(empCorpID))
                {
                    DTO_glEmpresa emp = (DTO_glEmpresa)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glEmpresa, empCorpID, true, false);
                    if (emp != null)
                    {
                        DAL_MasterHierarchy hierarchyeDAL = new DAL_MasterHierarchy(this._mySqlConnection, this._mySqlConnectionTx, emp, this.UserId, this.loggerConnectionStr);
                        hierarchyeDAL.DocumentID = documentID;

                        count = hierarchyeDAL.DAL_MasterHierarchy_CountChildren(parentId, idFilter, descFilter, active, filtros);
                    }
                }

                return count;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "coPlanCuenta_Count");
                return 0;
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
        public IEnumerable<DTO_MasterHierarchyBasic> coPlanCuenta_GetPagedChildren(int documentID, int pageSize, int pageNum, OrderDirection orderDirection, UDT_BasicID parentId,
            string idFilter, string descFilter, bool? active, List<DTO_glConsultaFiltro> filtros)
        {
            try
            {
                IEnumerable<DTO_MasterHierarchyBasic> list = new List<DTO_MasterHierarchyBasic>();

                string empCorpID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_EmpresaCorporativa);
                if (!string.IsNullOrWhiteSpace(empCorpID))
                {
                    DTO_glEmpresa emp = (DTO_glEmpresa)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glEmpresa, empCorpID, true, false);
                    if (emp != null)
                    {
                        DAL_MasterHierarchy hierarchyDAL = new DAL_MasterHierarchy(this._mySqlConnection, this._mySqlConnectionTx, emp, this.UserId, this.loggerConnectionStr);
                        hierarchyDAL.DocumentID = documentID;

                        IEnumerable<DTO_MasterBasic> simpleList = hierarchyDAL.DAL_MasterHierarchy_GetChindrenPaged(pageSize, pageNum, orderDirection, parentId, idFilter, descFilter, active, filtros);
                        list = hierarchyDAL.CompleteHierarchyList(simpleList);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "coPlanCuenta_GetPaged");
                return null;
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
                DAL_coPlanillaConsolidacion dalPlanilla = new DAL_coPlanillaConsolidacion(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string compID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteConsolidacion);
                string periodoStr = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
                DateTime periodo = DateTime.Parse(periodoStr);

                List<DTO_ComprobanteConsolidacion> results = dalPlanilla.DAL_coPlanillaConsolidacion_GetEmpresas(compID, periodo);

                return results;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "coPlanillaConsolidacion_GetEmpresas");
                return null;
            }
        }

        #endregion

        #region coTercero

        /// <summary>
        /// Trae un usuario
        /// </summary>
        /// <param name="userId">Identificador de usuario</param>
        /// <param name="documentoID">documentoID de usuario</param>
        /// <param name="password">Contraseña de usuario</param>
        /// <returns>Returna un usuario</returns>
        public UserResult coTercero_ValidateUserCredentials(string cedula, string password)
        {
            try
            {
                DTO_MasterBasic terceroBasic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, cedula, true, false);

                if (terceroBasic == null || terceroBasic.ID == null || terceroBasic.IdName == null)
                {
                    return UserResult.NotExists;
                }
                else
                {
                    this._dal_glControl = (DAL_glControl)base.GetInstance(typeof(DAL_glControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    DTO_glControl ctrlDTO = this._dal_glControl.DAL_glControl_GetById(AppControl.RepeticionesContrasenaBloqueo);
                    byte repContrasena = Convert.ToByte(ctrlDTO.Data.Value);

                    DTO_coTercero user = (DTO_coTercero)terceroBasic;
                    this._dal_coTercero = (DAL_coTercero)base.GetInstance(typeof(DAL_coTercero), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    return _dal_coTercero.DAL_coTercero_ValidateUserCredentials(user, password, repContrasena);
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UserValidate, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "coTercero_ValidateUserCredentials");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="pwd">Contraseña nueva</param>
        /// <returns>Retorna verdadero si la operación se pudo realizar, de lo contrario falso</returns>
        public bool coTercero_UpdatePassword(string userID, string pwd, bool insideAnotherTx)
        {
            bool result = true;
            try
            {
                if (!insideAnotherTx)
                    base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

                var today = DateTime.Now;
                this._dal_coTercero = (DAL_coTercero)base.GetInstance(typeof(DAL_coTercero), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int rows = _dal_coTercero.DAL_coTercero_UpdatePassword(userID.ToString(), pwd);

                if (rows > 0)
                {
                    this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    int bId = this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, AppMasters.coTercero, Convert.ToInt16(Math.Pow(2, Convert.ToInt32(FormsActions.Edit))), System.DateTime.Now, Convert.ToInt32(userID), "ReplicaID", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);

                    DAL_aplBitacoraAct btAct = (DAL_aplBitacoraAct)base.GetInstance(typeof(DAL_aplBitacoraAct), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    btAct.DAL_aplBitacoraAct_Add(bId, AppMasters.coTercero, "ContrasenaFecCambio", today.ToString());
                }
                else
                    result = false;

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdatePwd, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "coTercero_UpdatePassword");
                return false;
            }
            finally
            {
                if (result)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="pwd">Contraseña nueva</param>
        /// <returns>Retorna verdadero si la operación se pudo realizar, de lo contrario falso</returns>
        public bool coTercero_ResetPassword(string userID, string pwd)
        {
            this._dal_coTercero = (DAL_coTercero)base.GetInstance(typeof(DAL_coTercero), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_coTercero.DAL_coTercero_ResetPassword(userID, pwd, false);
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
            this._dal_glActividadFlujo = (DAL_glActividadFlujo)base.GetInstance(typeof(DAL_glActividadFlujo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glActividadFlujo.DAL_glActividadFlujo_GetTareaID(documentoID);
        }

        /// <summary>
        /// Obtiene la lista de padres de una actividad de flujo
        /// </summary>
        /// <param name="actFlujoID">Actividad hija</param>
        /// <returns>Retorna la lista de tareas</returns>
        public List<DTO_glActividadFlujo> glActividadFlujo_GetParents(string actFlujoID)
        {
            this._dal_glActividadFlujo = (DAL_glActividadFlujo)base.GetInstance(typeof(DAL_glActividadFlujo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glActividadFlujo.DAL_glActividadFlujo_GetParents(actFlujoID);
        }

        #endregion

        #region glActividadPermiso

        /// <summary>
        /// Obtiene la lista de usuarios a los que les tiene que enviar una alarma
        /// </summary>
        /// <param name="actFlujoID">Identificador de la tarea</param>
        /// <param name="afID">Identificador del area funcional</param>
        /// <returns>Retorna la lista de usuarios</returns>
        internal List<string> glActividadPermiso_GetAlarmaUsuarioByTareaAndAF(string actFlujoID, string afID)
        {
            this._dal_glActividadPermiso = (DAL_glActividadPermiso)base.GetInstance(typeof(DAL_glActividadPermiso), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glActividadPermiso.DAL_ActividadPermiso_GetAlarmaUsuarioByActividadAndAF(actFlujoID, afID);
        }

        /// <summary>
        /// Consulta un listado de tareas de glActividadPermiso para usuarios
        /// </summary>
        /// <returns>Listado de TareaID(int)</returns>
        public List<string> glActividadPermiso_GetActividadesByUser()
        {
            DTO_seUsuario user = this.seUsuario_GetUserByReplicaID(this.UserId);
            DAL_OperacionesDocumentos _dal_OperacionesDocumentos = new DAL_OperacionesDocumentos(base._mySqlConnection, base._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_OperacionesDocumentos.glActividadPermiso_GetActividadByUser(user.ID.Value);
        }

        #endregion

        #region glEmpresa

        /// <summary>
        /// Trae la imagen del logo de la empresa
        /// </summary>
        /// <returns>arreglo de bytes con la imagen del logo</returns>
        public byte[] glEmpresaLogo()
        {
            this._dal_glEmpresa = (DAL_glEmpresa)base.GetInstance(typeof(DAL_glEmpresa), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glEmpresa.DAL_glEmpresaLogo();
        }

        /// <summary>
        ///  Adiciona una lista de empresas
        /// </summary>
        /// <param name="documentoID">Identifica el documento</param>
        /// <param name="bItems">Lista de empresas</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="accion">Identifica la acción </param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult glEmpresa_Add(int documentoID, byte[] bItems, int accion, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DAL_MasterSimple empresaDAL = new DAL_MasterSimple(base._mySqlConnection, base._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            empresaDAL.DocumentID = documentoID;

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                List<DTO_MasterBasic> items = CompressedSerializer.Decompress<List<DTO_MasterBasic>>(bItems);

                if (items == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "ERR_AGN_0007";
                }
                else
                {
                    this._dal_glControl = (DAL_glControl)base.GetInstance(typeof(DAL_glControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_glEmpresaGrupo = (DAL_glEmpresaGrupo)base.GetInstance(typeof(DAL_glEmpresaGrupo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    string grupoCopiaID = this.GetControlValue(AppControl.GrupoEmpresaCopia);
                    string empCopiaID = this.GetControlValue(AppControl.EmpresaCopia);

                    DTO_glEmpresa empCopia = (DTO_glEmpresa)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glEmpresa, empCopiaID, true, false);

                    for (int i = 0; i < items.Count; i++)
                    {
                        DTO_TxResultDetail txD = new DTO_TxResultDetail();

                        DTO_glEmpresa emp = (DTO_glEmpresa)items[i];
                        DAL_MasterSimple dal_ge = new DAL_MasterSimple(base._mySqlConnection, base._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        dal_ge.DocumentID = AppMasters.glEmpresaGrupo;

                        UDT_BasicID udt = new UDT_BasicID() { Value = emp.ID.Value };
                        DTO_MasterBasic basic = dal_ge.DAL_MasterSimple_GetByID(udt, false);

                        //if (basic != null)
                        //{
                        //    result.Result = ResultValue.NOK;
                        //    txD.line = i + 1;
                        //    txD.Key = emp.ID.ToString();
                        //    txD.Message = DictionaryMessages.Err_AddEG;
                        //    result.Details.Add(txD);

                        //    break;
                        //}

                        #region Agrega la maestra con el numero de control
                        try
                        {
                            int numCtrl = this.GetCompanyControlNum();
                            emp.NumeroControl.Value = numCtrl.ToString();
                            emp.EmpresaGrupoID_.Value = emp.ID.Value;
                            txD = empresaDAL.DAL_MasterSimple_AddItem(emp);
                            txD.line = i + 1;
                            txD.Key = emp.ID.ToString();

                            result.Details.Add(txD);

                            if (txD != null && txD.DetailsFields.Count > 0)
                                result.Result = ResultValue.NOK;

                        }
                        catch (Exception ex)
                        {
                            result.Result = ResultValue.NOK;
                            txD = new DTO_TxResultDetail();
                            txD.line = i + 1;
                            txD.Key = emp.ID.ToString();
                            txD.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_glEmpresa_Add");
                            result.Details.Add(txD);
                        }
                        #endregion
                        if (result.Result == ResultValue.OK)
                        {
                            #region Agrega los datos a la tabla de control
                            try
                            {
                                this._dal_glControl.AddCompanyValues(emp.ID.Value, emp.NumeroControl.Value, empCopia.NumeroControl.Value);
                            }
                            catch (Exception)
                            {
                                result.Result = ResultValue.NOK;
                                txD.line = i + 1;
                                txD.Key = emp.ID.ToString();
                                txD.Message = DictionaryMessages.Err_AddControl;
                                result.Details.Add(txD);
                            }

                            #endregion
                            #region Agrega el grupo de empresas

                            DTO_glEmpresaGrupo eg = new DTO_glEmpresaGrupo()
                            {
                                ID = new DTO.UDT.UDT_BasicID() { Value = emp.ID.Value, MaxLength = 10 },
                                Descriptivo = new DTO.UDT.UDT_Descriptivo() { Value = emp.Descriptivo.Value },
                            };

                            try
                            {
                                bool res = this._dal_glEmpresaGrupo.DAL_glEmpresaGrupo_Add(eg, grupoCopiaID, true);
                                if (!res)
                                {
                                    result.Result = ResultValue.NOK;
                                    txD.line = i + 1;
                                    txD.Key = emp.ID.ToString();
                                    txD.Message = DictionaryMessages.Err_AddEG;
                                    result.Details.Add(txD);
                                }
                            }
                            catch (Exception ex)
                            {
                                result.Result = ResultValue.NOK;
                                txD.line = i + 1;
                                txD.Key = emp.ID.ToString();
                                txD.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_glEmpresaGrupo_Add");
                                result.Details.Add(txD);
                            }

                            #endregion
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_glEmpresa_Add");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        ///  Elimina una empresa
        /// </summary>
        /// <param name="documentoID">Identifica el documento</param>
        /// <param name="empresaDel">Empresa que de desea eliminar</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult glEmpresa_Delete(int documentoID, DTO_glEmpresa empresaDel, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            DAL_MasterSimple empresaDAL = new DAL_MasterSimple(base._mySqlConnection, base._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            empresaDAL.DocumentID = documentoID;

            DAL_aplBitacora bita = (DAL_aplBitacora)base.GetInstance(typeof(NewAge.ADO.DAL_aplBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            try
            {
                bool canDelete = bita.DAL_aplBitacora_DeleteCompany(this.Empresa.ID.Value);
                if (!canDelete)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_DeleteEmp;
                    result.Details = new List<DTO_TxResultDetail>();
                    return result;
                }

                //Elimina la empresa
                result = empresaDAL.DAL_MasterSimple_Delete(empresaDel.ID, false);
                if (result.Result == ResultValue.NOK)
                    return result;

                //Elimina los datos de la tabla de control
                this._dal_glControl = (DAL_glControl)base.GetInstance(typeof(DAL_glControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glControl.DeleteCompanyValues(empresaDel.NumeroControl.Value);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_glEmpresa_Delete");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
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
        public bool glEmpresaGrupo_Add(byte[] bItems, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            bool result = true;
            try
            {
                List<DTO_MasterBasic> items = CompressedSerializer.Decompress<List<DTO_MasterBasic>>(bItems);

                this._dal_glEmpresaGrupo = (DAL_glEmpresaGrupo)base.GetInstance(typeof(DAL_glEmpresaGrupo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string grupoCopiaID = this.GetControlValue(AppControl.GrupoEmpresaCopia);

                if (items == null)
                    result = false;
                else
                {
                    DTO_glEmpresaGrupo eg;
                    for (int i = 0; i < items.Count; i++)
                    {
                        eg = (DTO_glEmpresaGrupo)items[i];
                        result = this._dal_glEmpresaGrupo.DAL_glEmpresaGrupo_Add(eg, grupoCopiaID, true);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result = false;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glEmpresaGrupo_Add");
                return result;
            }
            finally
            {
                if (result)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Elimina un grupo de empresas
        /// </summary>
        /// <param name="grupo">Grupo de embresas</param>
        /// <param name="egCopia">Grupo de empresas del cual se saca la copia</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transacción</param>
        /// <returns></returns>
        public bool glEmpresaGrupo_Delete(string egID, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            bool result = true;
            try
            {
                this._dal_glEmpresaGrupo = (DAL_glEmpresaGrupo)base.GetInstance(typeof(DAL_glEmpresaGrupo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                result = this._dal_glEmpresaGrupo.DAL_glEmpresaGrupo_Delete(egID, insideAnotherTx);
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glEmpresaGrupo_Delete");
                return result;
            }
            finally
            {
                if (result)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region glTabla

        /// <summary>
        /// Retorna todas las gl tablas de un grupo de empresas
        /// </summary>
        /// <param name="empGrupoIDs">diccionario con los empresa grupo para cada tipo de EmpresaGrupo</param>
        /// <returns>Lista de glTabla</returns>
        public IEnumerable<DTO_glTabla> glTabla_GetAllByEmpresaGrupo(Dictionary<int, string> empGrupoIDs, bool jerarquicaInd)
        {
            this._dal_glTabla = (DAL_glTabla)base.GetInstance(typeof(DAL_glTabla), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glTabla.DAL_glTabla_GetAllByEmpresaGrupo(empGrupoIDs, jerarquicaInd);
        }

        /// <summary>
        /// Retorna la info de una tabla segun el nombre y el grupo de empresas
        /// </summary>
        /// <param name="tablaNombre">Tabla Nombre</param>
        /// <param name="empGrupo">Grupo de empresas</param>
        /// <returns>Retorna la informacion de una tabla</returns>
        public DTO_glTabla glTabla_GetByTablaNombre(string tablaNombre, string empGrupo)
        {
            this._dal_glTabla = (DAL_glTabla)base.GetInstance(typeof(DAL_glTabla), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glTabla.DAL_glTabla_GetByTablaNombre(tablaNombre, empGrupo);
        }

        /// <summary>
        /// Indica si una tabla tiene datos para un grupo empresa
        /// </summary>
        /// <param name="tablaNombre">Nombre de la tabla</param>
        /// <param name="empGrupo">Empresa Grupo</param>
        /// <returns>True si tiene datos, False si no tiene datos</returns>
        public bool glTabla_HasData(string tablaNombre, string empGrupo)
        {
            try
            {
                DTO_glTabla tabla = glTabla_GetByTablaNombre(tablaNombre, empGrupo);
                DTO_aplMaestraPropiedades props = StaticMethods.GetParameters(base._mySqlConnection, base._mySqlConnectionTx, tabla.DocumentoID.Value.Value, this.loggerConnectionStr);
                if (props.ColumnaID.Contains(','))
                {
                    //Compleja
                    DAL_MasterComplex dalMaster = (DAL_MasterComplex)base.GetInstance(typeof(NewAge.ADO.DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    dalMaster.DocumentID = tabla.DocumentoID.Value.Value;
                    if (dalMaster.DAL_MasterSimple_Count(null, null, null) > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    //Simple o Jerarquica
                    DAL_MasterSimple dalMaster = (DAL_MasterSimple)base.GetInstance(typeof(NewAge.ADO.DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    dalMaster.DocumentID = tabla.DocumentoID.Value.Value;
                    if (dalMaster.DAL_MasterSimple_Count(null, null, null) > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glTabla_HasData");
                return false;
            }
        }

        #endregion

        #region glTasaCambio

        /// <summary>
        /// Obtiene el la tasa de cambio
        /// </summary>
        /// <param name="monedaID">Identificador de la moneda</param>
        /// <param name="fecha">Fecha</param>
        /// <returns>Retorna la tasa de cambio</returns>
        public decimal TasaDeCambio_Get(string monedaID, DateTime fecha)
        {
            this._dal_glTasaCambio = (DAL_glTasaDeCambio)base.GetInstance(typeof(DAL_glTasaDeCambio), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glTasaCambio.DAL_TasaDeCambio_Get(monedaID, fecha);
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
                this._dal_glActividadChequeo = (DAL_glActividadChequeoLista)base.GetInstance(typeof(DAL_glActividadChequeoLista), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_glActividadChequeo.DAL_glActividadChequeoLista_GetByActividad(actividadFlujo);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glActividadChequeoLista_GetByActividad");
                return null;
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
            this._dal_seDelegacionHistoria = (DAL_seDelegacionHistoria)base.GetInstance(typeof(DAL_seDelegacionHistoria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_seDelegacionHistoria.DAL_seDelegacionHistoria_Get(userId);
        }

        /// <summary>
        /// Agrega una delegacion
        /// </summary>
        /// <param name="del">Delegacion</param>
        public bool seDelegacionHistoria_Add(int documentID, DTO_seDelegacionHistoria del, bool insideAnotherTx)
        {
            bool result = true; ;
            try
            {
                if (!insideAnotherTx)
                    base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

                //Crea el registro
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_seDelegacionHistoria = (DAL_seDelegacionHistoria)base.GetInstance(typeof(DAL_seDelegacionHistoria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                bool exists = this._dal_seDelegacionHistoria.DAL_seDelegacionHistoria_Exists(del.UsuarioID.Value, del.FechaInicialAsig.Value.Value);

                if (exists)
                {
                    this._dal_seDelegacionHistoria.DAL_seDelegacionHistoria_Update(del.UsuarioID.Value, del.FechaInicialAsig.Value.Value,
                        del.FechaFinalAsig.Value.Value, del.UsuarioRemplazo.Value);

                    //Guarda el registro en la bitacora
                    int bId = this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, Convert.ToInt16(Math.Pow(2, Convert.ToInt32(FormsActions.Edit))), System.DateTime.Now, this.UserId, del.UsuarioID.Value, del.FechaInicialAsig.Value.Value.ToShortDateString(), string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);
                }
                else
                {
                    this._dal_seDelegacionHistoria.DAL_seDelegacionHistoria_Add(del);

                    //Guarda el registro en la bitacora
                    int bId = this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, Convert.ToInt16(Math.Pow(2, Convert.ToInt32(FormsActions.Add))), System.DateTime.Now, this.UserId, del.UsuarioID.Value, del.FechaInicialAsig.Value.Value.ToShortDateString(), string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);
                }

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "seDelegacionHistoria_Add");
                return false;
            }
            finally
            {
                if (result)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Actualiza el estado de un delegado
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="enabled">Nuevo estado</param>
        public bool seDelegacionHistoria_UpdateStatus(int documentID, string userID, DateTime fechaIni, bool enabled, bool insideAnotherTx)
        {
            bool result = true;
            try
            {
                if (!insideAnotherTx)
                    base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

                //Crea el registro
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_seDelegacionHistoria = (DAL_seDelegacionHistoria)base.GetInstance(typeof(DAL_seDelegacionHistoria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                this._dal_seDelegacionHistoria.DAL_seDelegacionHistoria_UpdateStatus(userID, fechaIni, enabled);

                //Guarda el registro en la bitacora
                int bId = this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, Convert.ToInt16(Math.Pow(2, Convert.ToInt32(FormsActions.Edit))), System.DateTime.Now, this.UserId, userID, fechaIni.ToShortDateString(), string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "seDelegacionHistoria_UpdateStatus");
                return false;
            }
            finally
            {
                if (result)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Asigna delegaciones
        /// </summary>
        public void seDelegacionHistoria_Activar()
        {
            bool result = true;

            try
            {
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

                Dictionary<string, DTO_glEmpresa> cacheEmpresas = new Dictionary<string, DTO_glEmpresa>();
                DTO_glEmpresa empresa = null;

                this._dal_seDelegacionHistoria = (DAL_seDelegacionHistoria)base.GetInstance(typeof(DAL_seDelegacionHistoria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_seDelegacionHistoria> results = this._dal_seDelegacionHistoria.DAL_seDelegacionHistoria_GetNewDelegaciones();
                foreach (DTO_seDelegacionHistoria del in results)
                {
                    #region Trae la info de la empresa
                    if (cacheEmpresas.ContainsKey(del.EmpresaID.Value))
                        empresa = cacheEmpresas[del.EmpresaID.Value];
                    else
                    {
                        DAL_MasterSimple simpleDAL = new DAL_MasterSimple(base._mySqlConnection, base._mySqlConnectionTx, null, 0, this.loggerConnectionStr);
                        simpleDAL.DocumentID = AppMasters.glEmpresa;

                        UDT_BasicID udt = new UDT_BasicID();
                        udt.Value = del.EmpresaID.Value;
                        empresa = (DTO_glEmpresa)simpleDAL.DAL_MasterSimple_GetByID(udt, true);

                        cacheEmpresas.Add(del.EmpresaID.Value, empresa);
                    }
                    #endregion
                    this._dal_seDelegacionHistoria.DAL_seDelegacionHistoria_Activar(del, empresa);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "seDelegacionHistoria_Activar");
            }
            finally
            {
                if (result)
                    base._mySqlConnectionTx.Commit();
                else if (base._mySqlConnectionTx != null)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Asigna delegaciones
        /// </summary>
        public void seDelegacionHistoria_Desactivar()
        {
            bool result = true;

            try
            {
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

                Dictionary<string, DTO_glEmpresa> cacheEmpresas = new Dictionary<string, DTO_glEmpresa>();
                DTO_glEmpresa empresa = null;

                this._dal_seDelegacionHistoria = (DAL_seDelegacionHistoria)base.GetInstance(typeof(DAL_seDelegacionHistoria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_seDelegacionHistoria> results = this._dal_seDelegacionHistoria.DAL_seDelegacionHistoria_GetOldDelegaciones();
                foreach (DTO_seDelegacionHistoria del in results)
                {
                    #region Trae la info de la empresa
                    if (cacheEmpresas.ContainsKey(del.EmpresaID.Value))
                        empresa = cacheEmpresas[del.EmpresaID.Value];
                    else
                    {
                        DAL_MasterSimple simpleDAL = new DAL_MasterSimple(base._mySqlConnection, base._mySqlConnectionTx, null, 0, this.loggerConnectionStr);
                        simpleDAL.DocumentID = AppMasters.glEmpresa;

                        UDT_BasicID udt = new UDT_BasicID();
                        udt.Value = del.EmpresaID.Value;
                        empresa = (DTO_glEmpresa)simpleDAL.DAL_MasterSimple_GetByID(udt, true);

                        cacheEmpresas.Add(del.EmpresaID.Value, empresa);
                    }
                    #endregion
                    this._dal_seDelegacionHistoria.DAL_seDelegacionHistoria_Desactivar(del, empresa);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "seDelegacionHistoria_Desactivar");
            }
            finally
            {
                if (result)
                    base._mySqlConnectionTx.Commit();
                else if (base._mySqlConnectionTx != null)
                    base._mySqlConnectionTx.Rollback();
            }
        }
        #endregion

        #region seGrupoDocumento

        #region Funciones Privadas

        /// <summary>
        /// Verifica si existe una seguridad
        /// </summary>
        /// <param name="grupo">Identificador del grupo de seguridades</param>
        /// <param name="documento">Identificador del documento</param>
        /// <returns>Retorna un Dto de seguridades</returns>
        private DTO_seGrupoDocumento GetSecurity(string grupo, int documento)
        {
            this._dal_seGrupoDocumento = (DAL_seGrupoDocumento)base.GetInstance(typeof(DAL_seGrupoDocumento), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_seGrupoDocumento.GetSecurity(grupo, documento);
        }

        /// <summary>
        /// Adiciona una seguridad
        /// </summary>
        /// <param name="dto">Seguridad</param>
        /// <returns>Retorna una respuesta TxResult</returns>
        private DTO_TxResultDetail seGrupoDocumento_Add(DTO_seGrupoDocumento dto)
        {
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            try
            {
                this._dal_seGrupoDocumento = (DAL_seGrupoDocumento)base.GetInstance(typeof(DAL_seGrupoDocumento), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_seGrupoDocumento.DAL_seGrupoDocumento_Add(dto);
                rd.Message = "OK";

                return rd;
            }
            catch (Exception ex)
            {
                rd.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "seGrupoDocumento_Add"); ;
                return rd;
            }
        }

        /// <summary>
        /// Adiciona una seguridad
        /// </summary>
        /// <param name="dto">Seguridad</param>
        /// <returns>Retorna una respuesta TxResult</returns>
        private DTO_TxResultDetail seGrupoDocumento_Update(DTO_seGrupoDocumento dto)
        {
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            try
            {
                this._dal_seGrupoDocumento = (DAL_seGrupoDocumento)base.GetInstance(typeof(DAL_seGrupoDocumento), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_seGrupoDocumento.DAL_seGrupoDocumento_Update(dto);
                return rd;
            }
            catch (Exception ex)
            {
                rd.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "seGrupoDocumento_Update");
                return rd;
            }
        }

        /// <summary>
        /// Agrega actualizaciones a la bitacora
        /// </summary>
        /// <param name="item">registro a guardar</param>
        /// <param name="isUpd">Verifica si es actualizacion o insercion</param>
        /// <param name="seUsuario">Identificador del usuario</param>
        private int AddBitacoraValue(DTO_seGrupoDocumento item, bool isUpd, int seUsuario)
        {
            try
            {
                int accion = isUpd ? (int)FormsActions.Edit : (int)FormsActions.Add;
                string[] ret = new string[6];

                ret[0] = item.seGrupoID.Value;
                ret[1] = item.DocumentoID.Value.Value.ToString();
                ret[2] = string.Empty;
                ret[3] = string.Empty;
                ret[4] = string.Empty;
                ret[5] = string.Empty;

                DAL_aplBitacora bt = (DAL_aplBitacora)base.GetInstance(typeof(NewAge.ADO.DAL_aplBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int bId = bt.DAL_aplBitacora_Add(this.Empresa.ID.Value, AppMasters.seGrupoDocumento, Convert.ToInt16(Math.Pow(2, accion)), DateTime.Now, seUsuario, ret, 0, 0, 0);

                return bId;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "AddBitacoraValue");
                throw ex;
            }
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Trae la lista de seguridades de documentos de un usuario dada la empresa
        /// </summary>
        /// <param name="userId">Codigo de seguridad del usuario</param>
        /// <param name="userEmpDef">Empresa</param>
        /// <param name="isGroupActive">Si el grupo de seguridad esta activo</param>
        /// <returns>Retorna las seguridades de un usuario en una empresa</returns>
        public IEnumerable<DTO_seGrupoDocumento> seGrupoDocumento_GetByUsuarioID(string userEmpDef, int userId, bool isGroupActive)
        {
            this._dal_seGrupoDocumento = (DAL_seGrupoDocumento)base.GetInstance(typeof(DAL_seGrupoDocumento), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_seGrupoDocumento.DAL_seGrupoDocumento_GetByUsuarioID(userEmpDef, userId, isGroupActive);
        }

        /// <summary>
        /// Obtiene las seguridades del sistema para un grupo dado el modulo y el tipo de documento
        /// </summary>
        /// <param name="grupo">Grupo de seguridades</param>
        /// <param name="tipo">Tipo de documento</param>
        /// <returns>Retorna las seguridades de un grupo</returns>
        public IEnumerable<DTO_seGrupoDocumento> seGrupoDocumento_GetByType(string grupo, string tipo)
        {
            this._dal_seGrupoDocumento = (DAL_seGrupoDocumento)base.GetInstance(typeof(DAL_seGrupoDocumento), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_seGrupoDocumento.DAL_seGrupoDocumento_GetByType(grupo, tipo);
        }

        /// <summary>
        /// Actualiza una lista de seguridades
        /// </summary>
        /// <param name="bItems">Lista de seguridades comprimidas</param>
        /// <param name="seUsuario">Identificador del usuario</param>
        /// <returns>Retorna la lista de seguridades comprimidas</returns>
        public DTO_TxResult seGrupoDocumento_UpdateSecurity(byte[] bItems, int seUsuario, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            //Se usa para ver si se esta insertando o editando
            List<bool> updates = new List<bool>();

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                List<DTO_seGrupoDocumento> items = CompressedSerializer.Decompress<List<DTO_seGrupoDocumento>>(bItems);
                if (items.Equals(null))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "ERR_AGN_0007";
                }
                else
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        try
                        {
                            DTO_seGrupoDocumento old = this.GetSecurity(items[i].seGrupoID.Value, items[i].DocumentoID.Value.Value);
                            bool alreadyExists = old != null ? true : false;

                            DTO_TxResultDetail txD = new DTO_TxResultDetail();
                            txD.line = i + 1;
                            txD.Key = "seGrupoID [" + items[i].seGrupoID.Value + "] - glDocumentoID[" + items[i].DocumentoID.Value.Value.ToString() + "]";

                            //Agrega o actualiza el registro
                            if (alreadyExists)
                            {
                                items[i].CtrlVersion = old.CtrlVersion;
                                txD = this.seGrupoDocumento_Update(items[i]);
                                int bId = this.AddBitacoraValue(items[i], alreadyExists, seUsuario);

                                //Actualiza los campos de bitacora act
                                DAL_aplBitacoraAct btAct = (DAL_aplBitacoraAct)base.GetInstance(typeof(NewAge.ADO.DAL_aplBitacoraAct), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                                btAct.DAL_aplBitacoraAct_Add(bId, AppMasters.seGrupoDocumento, items[i].AccionesPerm.Value.Value.ToString(), old.AccionesPerm.Value.Value.ToString());
                            }
                            else
                            {
                                txD = this.seGrupoDocumento_Add(items[i]);
                                int bId = this.AddBitacoraValue(items[i], alreadyExists, seUsuario);
                            }

                            result.Details.Add(txD);
                            if (txD != null && !string.IsNullOrWhiteSpace(txD.Message) && txD.DetailsFields.Count > 0)
                                result.Result = ResultValue.NOK;
                        }
                        catch (Exception ex)
                        {
                            result.Result = ResultValue.NOK;
                            DTO_TxResultDetail txD = new DTO_TxResultDetail();
                            txD.line = i + 1;
                            txD.Key = "seGrupoID [" + items[i].seGrupoID.Value + "] - glDocumentoID[" + items[i].DocumentoID.Value.Value.ToString() + "]";
                            txD.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, seUsuario.ToString(), "DAL_seGrupoDocumento_UpdateSecurity");
                            result.Details.Add(txD);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_seGrupoDocumento_UpdateSecurity");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #endregion

        #region seMaquina

        /// <summary>
        /// valida que la maquina pueda ingresar al sistema
        /// </summary>
        /// <param name="macs">MACs posibles</param>
        /// <returns>Retorna verdadero si la maquina tiene permiso</returns>
        public bool seMaquina_ValidatePC(List<string> macs)
        {
            this._dal_seMaquina = (DAL_seMaquina)base.GetInstance(typeof(DAL_seMaquina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_seMaquina.DAL_seMaquina_ValidatePC(macs);
        }

        #endregion

        #region seLAN

        /// <summary>
        /// Trae la configuración de una LAN
        /// </summary>
        /// <param name="lan">Nombre de la LAN</param>
        /// <returns>Retorna la configuracion de una LAN</returns>
        public DTO_seLAN seLAN_GetLanByID(string lan, int documentID)
        {
            DAL_MasterSimple simpleDAL = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            UDT_BasicID udt = new UDT_BasicID() { Value = lan, IsInt = false };
            simpleDAL.DocumentID = documentID;
            DTO_seLAN dtoLAN = (DTO_seLAN)simpleDAL.DAL_MasterSimple_GetByID(udt, true);
            return dtoLAN;
        }

        /// <summary>
        /// Trae todas las configuraciones de LAN
        /// </summary>
        /// <returns>Retorna la lista de LANs y sus configuraciones</returns>
        public List<DTO_seLAN> seLAN_GetLanAll(int documentID)
        {
            DAL_MasterSimple simpleDAL = (DAL_MasterSimple)base.GetInstance(typeof(NewAge.ADO.DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            simpleDAL.DocumentID = documentID;
            long rowCount = simpleDAL.DAL_MasterSimple_Count(null, null, true);
            List<DTO_seLAN> dtoLAN = new List<DTO_seLAN>();
            List<DTO_MasterBasic> listLAN = simpleDAL.DAL_MasterSimple_GetPaged(rowCount, 1, null, null, true).ToList();
            foreach (var list in listLAN)
            {
                DTO_seLAN lan = (DTO_seLAN)list;
                dtoLAN.Add(lan);
            }
            // List<DTO_seLAN> dtoLAN = simpleDAL.DAL_MasterSimple_GetPaged(rowCount, 1, null, null, true).ToList();
            return dtoLAN;
        }

        #endregion

        #region seUsuario

        #region Funciones Privadas

        /// <summary>
        /// Adiciona un usuario
        /// </summary>
        /// <param name="dto">MasterBasic</param>
        /// <returns>Retorna el resultado TxResult de la operacion</returns>
        private DTO_TxResultDetail seUsuario_Add(DTO_seUsuario dto)
        {
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            try
            {
                this._dal_seUsuario = (DAL_seUsuario)base.GetInstance(typeof(DAL_seUsuario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string egControl = this.GetControlValue(AppControl.GrupoEmpresaGeneral);
                bool validDTO = true;

                //Mensajes de error
                string msgExistingItem = DictionaryMessages.PkInUse;
                string msgEmptyField = DictionaryMessages.EmptyField;
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                #region Validacion de nulls
                if (string.IsNullOrEmpty(dto.ID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "UsuarioID";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (string.IsNullOrEmpty(dto.Descriptivo.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "Descriptivo";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (!dto.ActivoInd.Value.HasValue)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "ActivoInd";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                //Campos extras
                if (string.IsNullOrEmpty(dto.IdiomaID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "IdiomaID";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (string.IsNullOrEmpty(dto.EmpresaIDPref.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "EmpresaIDPref";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (string.IsNullOrEmpty(dto.EmpresaID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "EmpresaID";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (string.IsNullOrEmpty(dto.AreaFuncionalID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "AreaFuncionalID";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (string.IsNullOrEmpty(dto.CorreoElectronico.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "CorreoElectronico";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                #endregion
                #region Validacion Pk inexistente
                DTO_seUsuario user = this.seUsuario_GetUserbyID(dto.ID.Value);

                if (user != null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "UsuarioID";
                    rdF.Message = msgExistingItem;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                #endregion
                #region validaciones de FKS
                DTO_MasterBasic basic;
                #region IdiomaID
                DAL_aplIdioma idiomaDAL = (DAL_aplIdioma)base.GetInstance(typeof(DAL_aplIdioma), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                bool hasLang = idiomaDAL.DAL_aplIdioma_Exists(dto.IdiomaID.Value);
                if (!hasLang)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "IdiomaID";
                    rdF.Message = msg_FkNotFound + "&&" + dto.IdiomaID.Value;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                #endregion
                #region EmpresaIdPref
                basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glEmpresa, dto.EmpresaIDPref.Value, true, false);
                if (basic == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "EmpresaIdPref";
                    rdF.Message = msg_FkNotFound + "&&" + dto.EmpresaIDPref.Value;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                #endregion
                #region EmpresaID
                basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glEmpresa, dto.EmpresaID.Value, true, false);
                if (basic == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "EmpresaId";
                    rdF.Message = msg_FkNotFound + "&&" + dto.EmpresaID.Value;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                #endregion
                #region AreaFuncionalID
                basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, dto.AreaFuncionalID.Value, true, false);
                if (basic == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "AreaFuncionalID";
                    rdF.Message = msg_FkNotFound + "&&" + dto.AreaFuncionalID.Value;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                #endregion
                #endregion
                #region Validacion de restricciones
                List<DTO_TxResultDetailFields> invRules = DTO_Validations.CheckRules(this.loggerConnectionStr, base._mySqlConnection, base._mySqlConnectionTx, dto, this.Empresa, string.Empty, this.UserId, true);
                if (invRules.Count > 0)
                {
                    foreach (DTO_TxResultDetailFields resDetail in invRules)
                        rd.DetailsFields.Add(resDetail);

                    validDTO = false;
                }
                #endregion
                #region Ejecutar consulta
                if (validDTO)
                    this._dal_seUsuario.DAL_seUsuario_Add(dto, egControl);
                #endregion

                return rd;
            }
            catch (Exception ex)
            {
                rd.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "seUsuario_Add");
                return rd;
            }
        }

        /// <summary>
        ///Actualiza un usuario
        /// </summary>
        /// <param name="dto">MasterBasic</param> 
        /// <returns>Retorna el resultado TxResult de la operacion</returns>
        private DTO_TxResultDetail seUsuario_Update(DTO_seUsuario dto)
        {
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            try
            {
                this._dal_seUsuario = (DAL_seUsuario)base.GetInstance(typeof(DAL_seUsuario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                bool validDTO = true;

                //Mensajes de error
                string msgEmptyField = DictionaryMessages.EmptyField;
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                #region Validacion de nulls
                if (string.IsNullOrEmpty(dto.ID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "UsuarioID";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (string.IsNullOrEmpty(dto.Descriptivo.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "Descriptivo";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (!dto.ActivoInd.Value.HasValue)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "ActivoInd";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                //Campos extras
                if (string.IsNullOrEmpty(dto.IdiomaID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "IdiomaID";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (string.IsNullOrEmpty(dto.EmpresaIDPref.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "EmpresaIDPref";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (string.IsNullOrEmpty(dto.EmpresaID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "EmpresaID";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (string.IsNullOrEmpty(dto.AreaFuncionalID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "AreaFuncionalID";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                if (string.IsNullOrEmpty(dto.CorreoElectronico.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "CorreoElectronico";
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);

                    validDTO = false;
                }
                #endregion
                #region validaciones de FKS
                try
                {
                    #region IdiomaID
                    DTO_MasterBasic basic;
                    DAL_aplIdioma idiomaDAL = (DAL_aplIdioma)base.GetInstance(typeof(DAL_aplIdioma), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    bool hasLang = idiomaDAL.DAL_aplIdioma_Exists(dto.IdiomaID.Value);
                    if (!hasLang)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "IdiomaID";
                        rdF.Message = msg_FkNotFound + "&&" + dto.IdiomaID.Value;
                        rd.DetailsFields.Add(rdF);

                        validDTO = false;
                    }
                    #endregion
                    #region EmpresaIdPref
                    basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glEmpresa, dto.EmpresaIDPref.Value, true, false);
                    if (basic == null)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "EmpresaIdPref";
                        rdF.Message = msg_FkNotFound + "&&" + dto.EmpresaIDPref.Value;
                        rd.DetailsFields.Add(rdF);

                        validDTO = false;
                    }
                    #endregion
                    #region EmpresaID
                    basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glEmpresa, dto.EmpresaID.Value, true, false);
                    if (basic == null)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "EmpresaID";
                        rdF.Message = msg_FkNotFound + "&&" + dto.EmpresaID.Value;
                        rd.DetailsFields.Add(rdF);

                        validDTO = false;
                    }
                    #endregion
                    #region AreaFuncionalID
                    basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, dto.AreaFuncionalID.Value, true, false);
                    if (basic == null)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "AreaFuncionalID";
                        rdF.Message = msg_FkNotFound + "&&" + dto.AreaFuncionalID.Value;
                        rd.DetailsFields.Add(rdF);

                        validDTO = false;
                    }
                    #endregion
                }
                catch (Exception eFK)
                {
                    throw eFK;
                }
                #endregion
                #region Validacion de restricciones
                List<DTO_TxResultDetailFields> invRules = DTO_Validations.CheckRules(this.loggerConnectionStr, base._mySqlConnection, base._mySqlConnectionTx, dto, this.Empresa, string.Empty, this.UserId, false);
                if (invRules.Count > 0)
                {
                    foreach (DTO_TxResultDetailFields resDetail in invRules)
                    {
                        rd.DetailsFields.Add(resDetail);
                    }

                    validDTO = false;
                }
                #endregion
                #region Ejecutar actualizacion
                if (validDTO)
                    this._dal_seUsuario.DAL_seUsuario_Update(dto);
                #endregion

                return rd;
            }
            catch (Exception ex)
            {
                rd.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "seUsuario_Update");
                return rd;
            }

        }

        /// <summary>
        /// Retorna una lista de DTO_TxResultDetailFields 
        /// </summary>
        /// <param name="oldUsr">Usuario antes de consultado</param>
        /// <param name="Usr">Usuario dado</param>
        /// <returns>Lista de DTO_TxResultDetailFields</returns>
        private List<DTO_TxResultDetailFields> seUsuario_GetDiferentFields(DTO_seUsuario oldUsr, DTO_seUsuario Usr)
        {
            List<DTO_TxResultDetailFields> response = new List<DTO_TxResultDetailFields>();
            if (oldUsr.ID.Value != Usr.ID.Value)
            {
                DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                field.Field = "UsuarioID";
                field.OldValue = oldUsr.ID.Value;
                field.NewValue = Usr.ID.Value;
                response.Add(field);
            }
            if (oldUsr.Descriptivo.Value != Usr.Descriptivo.Value)
            {
                DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                field.Field = "Descriptivo";
                field.OldValue = oldUsr.Descriptivo.Value.ToString();
                field.NewValue = Usr.Descriptivo.Value.ToString();
                response.Add(field);
            }
            if (oldUsr.IdiomaID.Value != Usr.IdiomaID.Value)
            {
                DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                field.Field = "IdiomaID";
                field.OldValue = oldUsr.IdiomaID.Value.ToString();
                field.NewValue = Usr.IdiomaID.Value.ToString();
                response.Add(field);
            }
            if (oldUsr.EmpresaIDPref.Value != Usr.EmpresaIDPref.Value)
            {
                DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                field.Field = "EmpresaIDPref";
                field.OldValue = oldUsr.EmpresaIDPref.Value.ToString();
                field.NewValue = Usr.EmpresaIDPref.Value.ToString();
                response.Add(field);
            }
            if (oldUsr.EmpresaID.Value != Usr.EmpresaID.Value)
            {
                DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                field.Field = "EmpresaID";
                field.OldValue = oldUsr.EmpresaID.Value.ToString();
                field.NewValue = Usr.EmpresaID.Value.ToString();
                response.Add(field);
            }
            if (oldUsr.AreaFuncionalID.Value != Usr.AreaFuncionalID.Value)
            {
                DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                field.Field = "AreaFuncionalID";
                field.OldValue = oldUsr.AreaFuncionalID.Value.ToString();
                field.NewValue = Usr.AreaFuncionalID.Value.ToString();
                response.Add(field);
            }
            if (oldUsr.SeccionFuncionalID.Value != Usr.SeccionFuncionalID.Value)
            {
                DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                field.Field = "SeccionFuncionalID";
                field.OldValue = oldUsr.SeccionFuncionalID.Value.ToString();
                field.NewValue = Usr.SeccionFuncionalID.Value.ToString();
                response.Add(field);
            }
            if (oldUsr.CorreoElectronico.Value != Usr.CorreoElectronico.Value)
            {
                DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                field.Field = "CorreoElectronico";
                field.OldValue = oldUsr.CorreoElectronico.Value.ToString();
                field.NewValue = Usr.CorreoElectronico.Value.ToString();
                response.Add(field);
            }
            if (oldUsr.ActivoInd.Value != Usr.ActivoInd.Value)
            {
                DTO_TxResultDetailFields field = new DTO_TxResultDetailFields();
                field.Field = "ActivoInd";
                field.OldValue = oldUsr.ActivoInd.Value.ToString();
                field.NewValue = Usr.ActivoInd.Value.ToString();
                response.Add(field);
            }

            return response;
        }

        #endregion

        #region Funciones Publicas

        #region Funciones seUsuario

        /// <summary>
        /// Trae un usuario
        /// </summary>
        /// <param name="userId">Identificador de usuario</param>
        /// <param name="documentoID">documentoID de usuario</param>
        /// <param name="password">Contraseña de usuario</param>
        /// <returns>Returna un usuario</returns>
        public UserResult seUsuario_ValidateUserCredentials(int documentoID, string userId, string password)
        {
            try
            {
                DTO_MasterBasic basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, documentoID, userId, true, false);

                this._dal_glControl = (DAL_glControl)base.GetInstance(typeof(DAL_glControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glControl ctrlDTO = this._dal_glControl.DAL_glControl_GetById(AppControl.RepeticionesContrasenaBloqueo);

                if (basic == null || basic.ID == null || basic.IdName == null)
                    return UserResult.NotExists;
                else
                {
                    DTO_seUsuario user = (DTO_seUsuario)basic;

                    this._dal_seUsuario = (DAL_seUsuario)base.GetInstance(typeof(DAL_seUsuario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    return _dal_seUsuario.DAL_seUsuario_ValidateUserCredentials(userId, password, user, ctrlDTO);
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UserValidate, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "seUsuario_ValidateUserCredentials");
                throw exception;
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
        public bool seUsuario_UpdatePassword(int userID, string pwd, string oldPwd, string oldPwdDate, bool insideAnotherTx)
        {
            bool result = true;
            try
            {
                if (!insideAnotherTx)
                    base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

                this._dal_seUsuario = (DAL_seUsuario)base.GetInstance(typeof(DAL_seUsuario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int rows = _dal_seUsuario.DAL_seUsuario_UpdatePassword(userID, pwd, oldPwd, oldPwdDate);

                if (rows > 0)
                {
                    this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    int bId = this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, AppMasters.seUsuario, Convert.ToInt16(Math.Pow(2, Convert.ToInt32(FormsActions.Edit))), System.DateTime.Now, userID, "ReplicaID", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);

                    DAL_aplBitacoraAct btAct = (DAL_aplBitacoraAct)base.GetInstance(typeof(DAL_aplBitacoraAct), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    btAct.DAL_aplBitacoraAct_Add(bId, AppMasters.seUsuario, "Contrasena", oldPwd);
                    btAct.DAL_aplBitacoraAct_Add(bId, AppMasters.seUsuario, "ContrasenaFecCambio", oldPwdDate);
                }
                else
                    result = false;

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdatePwd, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "seUsuario_UpdatePassword");
                return false;
            }
            finally
            {
                if (result)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
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
            this._dal_seUsuario = (DAL_seUsuario)base.GetInstance(typeof(DAL_seUsuario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_seUsuario.DAL_seUsuario_ResetPassword(userID, pwd, false);
        }

        /// <summary>
        /// Devuelve las empresas a las que tiene permiso un usuario
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <returns>Retorna una lista de empresas</returns>
        public IEnumerable<DTO_glEmpresa> seUsuario_GetUserCompanies(string userID)
        {
            this._dal_seUsuario = (DAL_seUsuario)base.GetInstance(typeof(DAL_seUsuario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_seUsuario.DAL_seUsuario_GetUserCompanies(userID);
        }

        /// <summary>
        /// Trae un usuario segun el nombre (NO LA REPLICA)
        /// </summary>
        /// <param name="documentoID">Identifica el documento</param>
        /// <param name="userId">Identificador de usuario</param>
        /// <returns>Retorna un usuario</returns>
        public DTO_seUsuario seUsuario_GetUserbyID(string userId)
        {
            this._dal_seUsuario = (DAL_seUsuario)base.GetInstance(typeof(DAL_seUsuario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_seUsuario.DAL_seUsuario_GetUserbyID(userId);
        }

        /// <summary>
        /// Trae un usuario de acuerdo con el id de la replica (pk)
        /// </summary>
        /// <param name="replicaID">Identificador del usuario (ReplicaID)</param>
        /// <returns>Retorna el Usuario</returns>
        public DTO_seUsuario seUsuario_GetUserByReplicaID(int replicaID)
        {
            this._dal_seUsuario = (DAL_seUsuario)base.GetInstance(typeof(DAL_seUsuario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_seUsuario.DAL_seUsuario_GetUserByReplicaID(replicaID);
        }

        #endregion

        #region Funciones master simple

        /// <summary>
        /// Trae la lista de usuarios
        /// </summary>
        /// <param name="documentoID">Identifica el documento</param>
        /// <param name="active">Indicador de activo</param>
        /// <returns>Devuelve la lista de usuarios </returns>
        public IEnumerable<DTO_MasterBasic> seUsuario_GetAll(int documentoID, bool? active)
        {
            try
            {
                DAL_MasterSimple simpleDAL = (DAL_MasterSimple)base.GetInstance(typeof(NewAge.ADO.DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                simpleDAL.DocumentID = documentoID;

                long count = simpleDAL.DAL_MasterSimple_Count(null, null, active);
                IEnumerable<DTO_MasterBasic> list = simpleDAL.DAL_MasterSimple_GetPaged(count, 1, null, null, active);
                return list;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seUsuario_GetAll");
                throw exception;
            }
        }

        /// <summary>
        ///  Adiciona una lista de empresas
        /// </summary>
        /// <param name="documentoID">Identifica el documento</param>
        /// <param name="bItems">Lista de empresas</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="accion">Identifica la acción </param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult seUsuario_Add(int documentID, byte[] bItems, int seUsuario, int accion, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                List<DTO_MasterBasic> items = CompressedSerializer.Decompress<List<DTO_MasterBasic>>(bItems);

                if (items == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "ERR_AGN_0007";
                }
                else
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        DTO_seUsuario usr = (DTO_seUsuario)items[i];
                        try
                        {
                            DTO_TxResultDetail txD = this.seUsuario_Add(usr);
                            txD.line = i + 1;
                            txD.Key = usr.ID.ToString();

                            result.Details.Add(txD);

                            if (txD != null && txD.DetailsFields.Count > 0)
                                result.Result = ResultValue.NOK;
                        }
                        catch (Exception ex)
                        {
                            result.Result = ResultValue.NOK;
                            DTO_TxResultDetail txD = new DTO_TxResultDetail();
                            txD.line = i + 1;
                            txD.Key = usr.ID.ToString();
                            txD.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, seUsuario.ToString(), "DAL_seUsuario_Add");
                            result.Details.Add(txD);
                        }
                    }

                    if (result.Result.Equals(ResultValue.OK))
                    {
                        bool opRes = true;
                        for (int i = 0; i < items.Count; i++)
                        {
                            DTO_seUsuario usr = (DTO_seUsuario)items[i];

                            //Agrega la información de la empresa a la bitacora
                            this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, Convert.ToInt16(Math.Pow(2, accion)), DateTime.Now, seUsuario, usr.ID.Value, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);
                        }

                        if (opRes)
                            result.Result = ResultValue.OK;
                    }
                    else
                        result.Result = ResultValue.NOK;
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "seUsuario_Add");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Actualiza un usuario
        /// </summary>
        /// <param name="documentoID">Identifica el documento</param>
        /// <param name="usr">registro donde se realiza la acción</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <returns>Resultado TxResult</returns>
        public DTO_TxResult seUsuario_Update(int documentoID, DTO_seUsuario usr, int seUsuario, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            //Objeto respuesta
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail txD = new DTO_TxResultDetail();
            List<DTO_TxResultDetailFields> txdfs = new List<DTO_TxResultDetailFields>();

            bool validUpdate = true;
            try
            {
                result.Result = ResultValue.OK;

                //Traer el usuario existente
                DTO_seUsuario oldUsr = this.seUsuario_GetUserbyID(usr.ID.Value);

                //Consultar por el id para determinar si existe
                if (oldUsr == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.InvalidCode;
                    return result;
                }

                //Determinar los campos que cambian
                txdfs = this.seUsuario_GetDiferentFields(oldUsr, usr);

                //Verificar las versiones de result
                if (oldUsr.CtrlVersion.Value < usr.CtrlVersion.Value)
                {
                    //Error catatrofico de datos inconsistencia de datos
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Data; //msg3
                    return result;
                }
                else if (oldUsr.CtrlVersion.Value > usr.CtrlVersion.Value)
                {
                    //Baje la ultima versión
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_UpdateGrid; //msg4
                    return result;
                }

                //Iguales debe seguir
                //Incrementar la versión
                oldUsr.CtrlVersion.Value = Convert.ToInt16(oldUsr.CtrlVersion.Value + 1);

                try
                {
                    if (oldUsr.ID != usr.ID)
                        oldUsr.ID = usr.ID;
                    if (oldUsr.Descriptivo != usr.Descriptivo)
                        oldUsr.Descriptivo = usr.Descriptivo;
                    if (oldUsr.ActivoInd != usr.ActivoInd)
                        oldUsr.ActivoInd = usr.ActivoInd;

                    txD = this.seUsuario_Update(usr);
                    txD.line = 1;
                    txD.Key = usr.ID.ToString();
                    result.Details.Add(txD);

                    if (txD.DetailsFields.Count > 0)
                        validUpdate = false;
                    else
                        txD.DetailsFields = txdfs;
                }
                catch (Exception ex)
                {

                    result.ResultMessage = "Error ";
                    result.Result = ResultValue.NOK;

                    result.Details = new List<DTO_TxResultDetail>();

                    txD.line = 1;
                    txD.Key = usr.ID.ToString();
                    txD.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, seUsuario.ToString(), "DAL_seUsuario_Update");
                    result.Details.Add(txD);
                }

                if (result.Result.Equals(ResultValue.OK) && result.Details.Count > 0 && validUpdate)
                {
                    DAL_aplBitacoraAct btAct = (DAL_aplBitacoraAct)base.GetInstance(typeof(NewAge.ADO.DAL_aplBitacoraAct), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    foreach (DTO_TxResultDetail lgB in result.Details)
                    {
                        this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        int bId = this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentoID, Convert.ToInt16(Math.Pow(2, Convert.ToInt32(FormsActions.Edit))), System.DateTime.Now, seUsuario, lgB.Key, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);

                        if (lgB.DetailsFields != null && lgB.DetailsFields.Count > 0)
                            foreach (DTO_TxResultDetailFields field in lgB.DetailsFields)
                                btAct.DAL_aplBitacoraAct_Add(bId, documentoID, field.Field, field.OldValue);
                    }

                    result.Result = ResultValue.OK;
                    result.Details = null;
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "seUsuario_Update");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK && validUpdate)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else
                {
                    if (!validUpdate)
                        result.Result = ResultValue.NOK;
                    if (base._mySqlConnectionTx != null && !insideAnotherTx)
                        base._mySqlConnectionTx.Rollback();
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region glDocumentoAprueba

        /// <summary>
        /// Dal para Consulta de registros en la tabla glDocumentoAprueba
        /// </summary>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <returns>listado de registros de la tabla glDocumentoAprueba por numero de documento</returns>
        public DTO_TxResult glDocumentoAprueba_Add(DTO_glDocumentoAprueba docAprueba)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                this._dal_glDocumentoAprueba = new DAL_glDocumentoAprueba(base._mySqlConnection, base._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glDocumentoAprueba.DAL_glDocumentoAprueba_Add(docAprueba);
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glDocumentoAprueba_Add");
                return result;
            }
            return result;
        }

        /// <summary>
        ///  Guarda  en glDocumentoAprueba obteniendo los usuarios con niveles de aprobacion
        /// </summary>
        /// <param name="documentID">Documento actual</param>
        /// <param name="numeroDoc">identificador del documento a consultar</param>        
        /// <param name="valor">Indica el valor tope por aprobar(si es 0 no lo tiene en cuenta)</param>
        /// <param name="updateDocAprueba">Indica si actualiza o guarda la info</param>
        /// <returns>DTO_glDocumentoAprueba</returns>
        internal DTO_TxResult glDocumentoAprueba_AddByNivelApr(int documentID, int numeroDoc, decimal valor)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            DTO_glDocumentoAprueba docAprueba = new DTO_glDocumentoAprueba();
            bool tipoEspecialInd = false;
            int c = 1;

            this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_MasterComplex = (DAL_MasterComplex)this.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            #region Valida datos de aprobacion del Documento
            DTO_glDocumento documento = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentID.ToString(), true, true);
            tipoEspecialInd = documento.AprobacionEspecialInd.Value.Value;
            valor = documento.AprobacionValorInd.Value.Value ? valor : 0;
            #endregion

            try
            {
                #region Obtiene variables iniciales
                DTO_seUsuario userCurrent = (DTO_seUsuario)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.seUsuario, this.seUsuario_GetUserByReplicaID(this.UserId).ID.Value, true, false);
                DTO_glAreaFuncional areaFuncional = (DTO_glAreaFuncional)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, userCurrent.AreaFuncionalID.Value, true, false);
                DTO_glSeccionFuncional seccionFunc = userCurrent.SeccionFuncionalID.Value != null ? (DTO_glSeccionFuncional)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glSeccionFuncional, userCurrent.SeccionFuncionalID.Value, true, false) : null;
                DTO_glAreaFuncional areaFuncAlterna = areaFuncional.AreaAprobacionAlternaID.Value != null ? (DTO_glAreaFuncional)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, areaFuncional.AreaAprobacionAlternaID.Value, true, false) : null;

                #endregion
                #region Obtiene los niveles de aprobacion del Documento
                DTO_glConsulta consulta = new DTO_glConsulta();
                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "DocumentoID",
                    ValorFiltro = documentID.ToString(),
                    OperadorFiltro = OperadorFiltro.Igual,
                    OperadorSentencia = "AND"
                });
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "GrupoApruebaDocID",
                    ValorFiltro = areaFuncional.GrupoApruebaDocID.Value,
                    OperadorFiltro = OperadorFiltro.Igual,
                    OperadorSentencia = "AND"
                });
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "TipoEspecial",
                    ValorFiltro = tipoEspecialInd ? "1" : "0",
                    OperadorFiltro = OperadorFiltro.Igual,
                    OperadorSentencia = "AND"
                });
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "Valor",
                    ValorFiltro = valor.ToString().Replace(",", "."),
                    OperadorFiltro = valor != 0 ? OperadorFiltro.MayorIgual : OperadorFiltro.Igual,
                });
                consulta.Filtros = filtros;
                this._dal_MasterComplex.DocumentID = AppMasters.glNivelesAprobacionDoc;
                long count = this._dal_MasterComplex.DAL_MasterComplex_Count(consulta, true);
                List<DTO_MasterComplex> listTmp = _dal_MasterComplex.DAL_MasterComplex_GetPaged(count, 1, consulta, true).ToList();
                List<DTO_glNivelesAprobacionDoc> listNivelApprove = listTmp.Cast<DTO_glNivelesAprobacionDoc>().ToList();

                if (listNivelApprove != null && listNivelApprove.Count > 0)
                {
                    if (valor != 0)
                    {
                        listNivelApprove = listNivelApprove.OrderBy(x => x.Valor.Value).ToList();
                        listNivelApprove = listNivelApprove.FindAll(x => x.Valor.Value == listNivelApprove.First().Valor.Value).ToList();
                    }
                    else
                        listNivelApprove = listNivelApprove.OrderBy(x => x.Orden.Value).ToList();
                }
                #endregion

                foreach (DTO_glNivelesAprobacionDoc niv in listNivelApprove)
                {
                    #region Asigna usuarios segun los niveles de aprobacion obtenidos
                    int? usuarioAsignado = null;
                    if (niv.NivelAprobacion.Value == (byte)NivelAprobacionUser.SeccionDirecta)
                    {
                        DTO_glSeccionFuncional seccionDirecta = (DTO_glSeccionFuncional)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glSeccionFuncional, niv.SeccionFuncionalID.Value, true, false);
                        usuarioAsignado = this.seUsuario_GetUserbyID(seccionDirecta.DirectorSeccion.Value).ReplicaID.Value;
                    }
                    else if (niv.NivelAprobacion.Value == (byte)NivelAprobacionUser.DirectorArea)
                        usuarioAsignado = this.seUsuario_GetUserbyID(areaFuncional.DirectorArea.Value).ReplicaID.Value;
                    else if (niv.NivelAprobacion.Value == (byte)NivelAprobacionUser.SubDirectorArea)
                        usuarioAsignado = this.seUsuario_GetUserbyID(areaFuncional.SubDirectorArea.Value).ReplicaID.Value;
                    else if (niv.NivelAprobacion.Value == (byte)NivelAprobacionUser.DirectorSección)
                    {
                        if (seccionFunc != null)
                            usuarioAsignado = this.seUsuario_GetUserbyID(seccionFunc.DirectorSeccion.Value).ReplicaID.Value;
                        else
                        {
                            docAprueba = null;
                            break;
                        }
                    }
                    else if (niv.NivelAprobacion.Value == (byte)NivelAprobacionUser.DirectorAreaAlterna)
                    {
                        if (areaFuncAlterna != null)
                            usuarioAsignado = this.seUsuario_GetUserbyID(areaFuncAlterna.DirectorArea.Value).ReplicaID.Value;
                        else
                        {
                            docAprueba = null;
                            break;
                        }
                    }

                    if (c == 1) docAprueba.UsuarioAprueba1.Value = usuarioAsignado;
                    else if (c == 2) docAprueba.UsuarioAprueba2.Value = usuarioAsignado;
                    else if (c == 3) docAprueba.UsuarioAprueba3.Value = usuarioAsignado;
                    else if (c == 4) docAprueba.UsuarioAprueba4.Value = usuarioAsignado;
                    else if (c == 5) docAprueba.UsuarioAprueba5.Value = usuarioAsignado;
                    else if (c == 6) docAprueba.UsuarioAprueba6.Value = usuarioAsignado;
                    else if (c == 7) docAprueba.UsuarioAprueba7.Value = usuarioAsignado;
                    else if (c == 8) docAprueba.UsuarioAprueba8.Value = usuarioAsignado;
                    else if (c == 9) docAprueba.UsuarioAprueba9.Value = usuarioAsignado;
                    else if (c == 10) docAprueba.UsuarioAprueba10.Value = usuarioAsignado;
                    c++;
                    #endregion
                }

                #region Valida consistencia de datos
                if (listNivelApprove.Count == 0 && valor != 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Pr_NivelApproveNotExistByValue;
                    return result;
                }

                if (docAprueba != null && listNivelApprove.Count > 0)
                {
                    docAprueba.NumeroDoc.Value = numeroDoc;
                    docAprueba.UsuarioAprueba.Value = docAprueba.UsuarioAprueba1.Value;
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "Error en parametrizacion de usuarios";
                    if(docAprueba == null)
                        result.ResultMessage += "-NO hay glDocumentoAprueba con el Usuario " + this.UserId.ToString() + "User SeccionFuncional: " + (seccionFunc != null ? seccionFunc.DirectorSeccion.Value : "Vacio. " + " Favor enviar error al administrador");
                    if (listNivelApprove.Count == 0)
                        result.ResultMessage += "-NO hay niveles de aprobacion en la transaccion con el Usuario " + this.UserId.ToString() + "User SeccionFuncional: " +(seccionFunc != null ? seccionFunc.DirectorSeccion.Value : "Vacio." + " Favor enviar error al administrador"); 
                    return result;
                }
                #endregion
                #region Guarda o actualiza el documento de Aprobacion

                DTO_glDocumentoAprueba docExist = this.glDocumentoAprueba_Get(numeroDoc);
                if (docExist == null)
                {
                    result = this.glDocumentoAprueba_Add(docAprueba);
                    if (result.Result == ResultValue.NOK)
                    {
                        result.ResultMessage = DictionaryMessages.Err_Pr_NivelApproveNotExist;
                        return result;
                    }
                }
                else
                {
                    result = this.glDocumentoAprueba_Update(docAprueba);
                    if (result.Result == ResultValue.NOK)
                    {
                        result.ResultMessage = DictionaryMessages.Err_Pr_NivelApproveNotExist;
                        return result;
                    }
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ModuloGlobal_glDocumentoAprueba_AddByNivelApr");
                result.Result = ResultValue.NOK;
                return result;
            }
        }

        /// <summary>
        /// Dal para Consulta de registros en la tabla glDocumentoAprueba
        /// </summary>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <returns>listado de registros de la tabla glDocumentoAprueba por numero de documento</returns>
        public DTO_glDocumentoAprueba glDocumentoAprueba_Get(int numeroDoc)
        {
            DTO_glDocumentoAprueba docAprueba = new DTO_glDocumentoAprueba();
            this._dal_glDocumentoAprueba = new DAL_glDocumentoAprueba(base._mySqlConnection, base._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            docAprueba = this._dal_glDocumentoAprueba.DAL_glDocumentoAprueba_Get(numeroDoc);

            return docAprueba;
        }

        /// <summary>
        /// Actualizar de registros en la tabla glDocumentoAprueba
        /// </summary>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <returns>listado de registros de la tabla glDocumentoAprueba por numero de documento</returns>
        public DTO_TxResult glDocumentoAprueba_Update(DTO_glDocumentoAprueba docAprueba)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                this._dal_glDocumentoAprueba = new DAL_glDocumentoAprueba(base._mySqlConnection, base._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glDocumentoAprueba.DAL_glDocumentoAprueba_Upd(docAprueba);
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glDocumentoAprueba_Update");
                return result;
            }
            return result;
        }

        /// <summary>
        ///  Actualiza la tabla con los usuarios y fechas de aprobacion
        /// </summary>
        /// <param name="numeroDoc">identificador del documento a actualizar</param>
        /// <returns>DTO_glDocumentoAprueba</returns>
        internal DTO_glDocumentoAprueba glDocumentoAprueba_UpdateUserApprover(int numeroDoc)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            DTO_glDocumentoAprueba docAprueba = this.glDocumentoAprueba_Get(numeroDoc);
            if (docAprueba.UsuarioAprueba.Value == docAprueba.UsuarioAprueba1.Value && docAprueba.FechaAprueba1.Value == null)
            {
                docAprueba.FechaAprueba1.Value = DateTime.Now.Date;
                docAprueba.UsuarioAprueba.Value = docAprueba.UsuarioAprueba2.Value;
            }
            else if (docAprueba.UsuarioAprueba.Value == docAprueba.UsuarioAprueba2.Value && docAprueba.FechaAprueba2.Value == null)
            {
                docAprueba.FechaAprueba2.Value = DateTime.Now.Date;
                docAprueba.UsuarioAprueba.Value = docAprueba.UsuarioAprueba3.Value;
            }
            else if (docAprueba.UsuarioAprueba.Value == docAprueba.UsuarioAprueba3.Value && docAprueba.FechaAprueba3.Value == null)
            {
                docAprueba.FechaAprueba3.Value = DateTime.Now.Date;
                docAprueba.UsuarioAprueba.Value = docAprueba.UsuarioAprueba4.Value;
            }
            else if (docAprueba.UsuarioAprueba.Value == docAprueba.UsuarioAprueba4.Value && docAprueba.FechaAprueba4.Value == null)
            {
                docAprueba.FechaAprueba4.Value = DateTime.Now.Date;
                docAprueba.UsuarioAprueba.Value = docAprueba.UsuarioAprueba5.Value;
            }
            else if (docAprueba.UsuarioAprueba.Value == docAprueba.UsuarioAprueba5.Value && docAprueba.FechaAprueba5.Value == null)
            {
                docAprueba.FechaAprueba5.Value = DateTime.Now.Date;
                docAprueba.UsuarioAprueba.Value = docAprueba.UsuarioAprueba6.Value;
            }
            else if (docAprueba.UsuarioAprueba.Value == docAprueba.UsuarioAprueba6.Value && docAprueba.FechaAprueba6.Value == null)
            {
                docAprueba.FechaAprueba6.Value = DateTime.Now.Date;
                docAprueba.UsuarioAprueba.Value = docAprueba.UsuarioAprueba7.Value;
            }
            else if (docAprueba.UsuarioAprueba.Value == docAprueba.UsuarioAprueba7.Value && docAprueba.FechaAprueba7.Value == null)
            {
                docAprueba.FechaAprueba7.Value = DateTime.Now.Date;
                docAprueba.UsuarioAprueba.Value = docAprueba.UsuarioAprueba8.Value;
            }
            else if (docAprueba.UsuarioAprueba.Value == docAprueba.UsuarioAprueba8.Value && docAprueba.FechaAprueba8.Value == null)
            {
                docAprueba.FechaAprueba8.Value = DateTime.Now.Date;
                docAprueba.UsuarioAprueba.Value = docAprueba.UsuarioAprueba9.Value;
            }
            else if (docAprueba.UsuarioAprueba.Value == docAprueba.UsuarioAprueba9.Value && docAprueba.FechaAprueba9.Value == null)
            {
                docAprueba.FechaAprueba9.Value = DateTime.Now.Date;
                docAprueba.UsuarioAprueba.Value = docAprueba.UsuarioAprueba10.Value;
            }
            else if (docAprueba.UsuarioAprueba.Value == docAprueba.UsuarioAprueba10.Value && docAprueba.FechaAprueba10.Value == null)
            {
                docAprueba.FechaAprueba10.Value = DateTime.Now.Date;
                docAprueba.UsuarioAprueba.Value = null;
            }
            result = this.glDocumentoAprueba_Update(docAprueba);
            if (result.Result == ResultValue.NOK)
                docAprueba = null;
            return docAprueba;
        }

        #endregion

        #region glActividadControl

        /// <summary>
        /// Dal para Consulta de registros en la tabla glTareasControl
        /// </summary>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <returns>listado de registros de la tabla glTareasControl por numero de documento</returns>
        public IEnumerable<DTO_glActividadControl> glActividadControl_Get(int numeroDoc)
        {
            DAL_glActividadControl _dal_glActividadControl = new DAL_glActividadControl(base._mySqlConnection, base._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glActividadControl.DAL_glActividadControl_Get(numeroDoc);
        }

        #endregion

        #region glControl

        /// <summary>
        ///  Trae una fila de la tabla de control de acuerdo a un id
        /// </summary>
        /// <param name="controlId">ID de control</param>
        /// <returns>dto control encontrado</returns>
        public DTO_glControl GetControlByID(int controlId)
        {
            try
            {
                this._dal_glControl = (DAL_glControl)base.GetInstance(typeof(DAL_glControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glControl ctrl = _dal_glControl.DAL_glControl_GetById(controlId);
                return ctrl;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "GetControlByID");
                throw exception;
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
                this._dal_glControl = (DAL_glControl)base.GetInstance(typeof(DAL_glControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_glControl.glControl_GetByNumeroEmpresa(isBasic, numEmpresa);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "glControl_GetByNumeroEmpresa");
                throw exception;
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
                this._dal_glControl = (DAL_glControl)base.GetInstance(typeof(DAL_glControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glControl.DAL_glControl_UpdateModuleData(data);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "glControl_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza glControl
        /// </summary>
        /// <param name="control">control</param> 
        /// <returns>Retorna una respuesta TxResult</returns>
        public DTO_TxResult glControl_Update(DTO_glControl control)
        {
            //Objeto respuesta
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                this._dal_glControl = (DAL_glControl)base.GetInstance(typeof(DAL_glControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glControl.DAL_glControl_Update(control);

                result.Result = ResultValue.OK;
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_glControl_Update");
                return result;
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
            this._dal_glDocAnexoControl = (DAL_glDocAnexoControl)base.GetInstance(typeof(DAL_glDocAnexoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocAnexoControl.DAL_glDocAnexoControl_GetAnexosByNumeroDoc(numeroDoc);
        }

        /// <summary>
        /// Retorna un anexo de un documento
        /// </summary>
        /// <param name="">replica</param>
        /// <returns>Retorna un anexo</returns>
        public DTO_glDocAnexoControl glDocAnexoControl_GetAnexosByReplica(int replica)
        {
            this._dal_glDocAnexoControl = (DAL_glDocAnexoControl)base.GetInstance(typeof(DAL_glDocAnexoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocAnexoControl.DAL_glDocAnexoControl_GetAnexosByReplica(replica);
        }

        /// <summary>
        /// Actualiza los anexos de un documento
        /// </summary>
        /// <param name="mod">Modulo que guarda los anexos</param>
        /// <param name="list">lista de anexos</param>
        /// <returns></returns>
        public DTO_TxResult glDocAnexoControl_Update(ModulesPrefix mod, List<DTO_glDocAnexoControl> list)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                int i = 0;
                this._dal_glDocAnexoControl = (DAL_glDocAnexoControl)base.GetInstance(typeof(DAL_glDocAnexoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string path = this.GetControlValue(AppControl.RutaFisicaArchivos) + this.GetControlValue(AppControl.RutaAnexos);
                string anexosPath = string.Format(path, mod.ToString());
                string nameFormat = this.GetControlValue(AppControl.NombreAnexoDocumento);

                foreach (DTO_glDocAnexoControl anexo in list)
                {
                    try
                    {
                        i++;
                        if (anexo.Actualizar.Value.Value)
                        {
                            if (anexo.Eliminar.Value.Value)
                            {
                                this._dal_glDocAnexoControl.DAL_glDocAnexoControl_Delete(anexo.ArchivoNombre.Value);

                                if (File.Exists(anexosPath + anexo.ArchivoNombre.Value))
                                    File.Delete(anexosPath + anexo.ArchivoNombre.Value);
                            }
                            else if (anexo.Nuevo.Value.Value)
                            {
                                #region Crea el archivo

                                string fileName = string.Format(nameFormat, anexo.NumeroDoc.Value.Value.ToString(),
                                    anexo.ConsReplica.Value.Value.ToString() + anexo.Extension);

                                string filePath = anexosPath + fileName;

                                if (!Directory.Exists(anexosPath))
                                    Directory.CreateDirectory(anexosPath);

                                File.WriteAllBytes(filePath, anexo.Archivo);

                                #endregion
                                #region Agrega el registro

                                anexo.ArchivoNombre.Value = fileName;
                                this._dal_glDocAnexoControl.DAL_glDocAnexoControl_Add(anexo);
                                #endregion
                            }
                            else if (!string.IsNullOrWhiteSpace(anexo.ArchivoNombre.Value) && anexo.Archivo != null)
                            {
                                string fileName = string.Format(nameFormat, anexo.NumeroDoc.Value.Value.ToString(),
                                    anexo.ConsReplica.Value.Value.ToString() + anexo.Extension);

                                string filePath = anexosPath + fileName;

                                File.WriteAllBytes(filePath, anexo.Archivo);
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        result.Result = ResultValue.NOK;

                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                        rd.line = i;
                        rd.Message = Mentor_Exception.LogException_Local(this.loggerConnectionStr, e1, this.UserId.ToString(), "ModuloGlobal_glDocAnexoControl_Update");

                        result.Details.Add(rd);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ModuloGlobal_glDocAnexoControl_Update");

                return result;
            }
        }

        #endregion

        #region glDocumentoControl

        #region Funciones Privadas

        /// <summary>
        /// Actualiza los consecutivos del documento control
        /// </summary>
        /// <param name="ctrl">DocumentoControl</param>
        /// <param name="updDocNro">Indica si se debe actyualizar el numero de documento</param>
        /// <param name="updCompNro">Indica si se debe actualizar el numero de comprobante</param>
        /// <param name="isPre">Indica si debe actualizar la tabla de aux o auxPre (para docs internos)</param>
        internal void ActualizaConsecutivos(DTO_glDocumentoControl ctrl, bool updDocNro, bool updCompNro, bool isPre)
        {
            try
            {
                DocumentoTipo tipo = (DocumentoTipo)Enum.Parse(typeof(DocumentoTipo), ctrl.DocumentoTipo.Value.Value.ToString());
                int? docNro = updDocNro ? (int?)ctrl.DocumentoNro.Value.Value : null;
                int? compNro = updCompNro ? (int?)ctrl.ComprobanteIDNro.Value.Value : null;
                string compID = updCompNro ? ctrl.ComprobanteID.Value : string.Empty;

                this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glDocumentoControl.DAL_glDocumentoControl_UpdateConsecutivos(ctrl.NumeroDoc.Value.Value, tipo, docNro, compNro, compID, isPre, ctrl.DocumentoID.Value);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ModuloGlobal_ActualizaDocumentoNro");
            }
        }

        /// <summary>
        /// Obtiene el numero de documentos en un periodo segun el estado
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="mod">Modulo de consulta</param>
        /// <param name="estados">Posibles estados</param>
        /// <returns>Retorna ael numero de documentos</returns>
        internal int glDocumentoControl_CountDocumentsByEstado(DateTime periodo, ModulesPrefix mod, List<EstadoDocControl> estados,string excluyeDocs)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glDocumentoControl.DAL_glDocumentoControl_CountDocumentsByEstado(periodo, mod, estados, excluyeDocs);
        }

        /// <summary>
        /// Valida si se puede realizar el proceso de cierre anual
        /// </summary>
        /// <param name="year">Año de validacion</param>
        /// <returns>Retirna verdadero si ya se realizo el proceso de ajuste en cambio</returns>
        internal bool glDocumentoControl_ValidaAjusteEnCambio(DateTime periodo)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glDocumentoControl.DAL_glDocumentoControl_ValidaAjusteEnCambio(periodo);
        }

        /// <summary>
        /// Revisa cuantos documentos existen en un estado y un periodo
        /// </summary>
        /// <param name="estado">Estado</param>
        /// <param name="periodo">Periodo</param>
        /// <returns>Retorna la cantidad de documentos en un estado</returns>
        internal int glDocumentoControl_CountByEstadoPeriodo(EstadoDocControl estado, DateTime periodo, ModulesPrefix mod)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glDocumentoControl.DAL_glDocumentoControl_CountByEstadoModulo(estado, periodo, mod);
        }

        /// <summary>
        /// Calcula la cantidad de "hijos" con una estado especifico
        /// </summary>
        /// <param name="numeroDoc">Numero de documento padre</param>
        /// <param name="estado">Estado del documento del hijo que se desa filtrar</param>
        public int glDocumentoControl_GetApproveChilds(int numeroDoc, int estado)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glDocumentoControl.DAL_glDocumentoControl_GetApproveChilds(numeroDoc, estado);
        }

        /// <summary>
        /// Obtiene la liquiadcion documento aociado al tipo de documento y periodo para un empleado
        /// </summary>
        /// <param name="documentID">documentoID</param>
        /// <param name="empleadoID">empleadoID</param>
        /// <param name="periodo">periodo</param>
        /// <returns>Documento</returns>
        internal DTO_glDocumentoControl glDocumentoControl_GetDocEmpleado(int documentID, string terceroID, DateTime periodo)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocumentoControl.DAL_glDocumentoControl_GetByDocEmpleado(documentID, terceroID, periodo);
        }

        /// <summary>
        /// Obtiene la liquiadcion documento aociado al tipo de documento y periodo para un empleado
        /// </summary>
        /// <param name="documentID">identificador del documento</param>
        /// <param name="periodo">periodo</param>
        /// <param name="contrato">numero de contrato del empleado</param>
        /// <returns>documento asociado al empleado</returns>
        internal DTO_glDocumentoControl glDocumentoControl_GetDocEmpleado(int documentID, DateTime periodo, int contrato)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocumentoControl.DAL_glDocumentoControl_GetByDocEmpleado(documentID, periodo, contrato);
        }

        /// <summary>
        /// Trae un documento relacionado a un proceso de billing
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="compID">Comprobante</param>
        /// <param name="monedaID">Moneda</param>
        /// <returns>Retorna el documento control </returns>
        internal DTO_glDocumentoControl glDocumentoControl_GetByBilling(DateTime periodo, string compID, string monedaID)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocumentoControl.DAL_glDocumentoControl_GetByBilling(periodo, compID, monedaID);
        }

        /// <summary>
        /// Trae un documento relacionado a un comprobante
        /// </summary>
        internal DTO_glDocumentoControl glDocumentoControl_GetByCierreAnual(int year)
        {
            string perido14 = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndicadorMes14);
            bool p14 = false;
            if (perido14.Equals("1") || perido14.Equals(true.ToString()))
                p14 = true;

            DateTime periodo = new DateTime(year, 12, p14 ? 3 : 2);

            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocumentoControl.DAL_glDocumentoControl_GetByCierreAnual(periodo);
        }

        /// <summary>
        /// Obtiene el documento relacionado con una libranza
        /// </summary>
        /// <param name="libranza">Libranza</param>
        /// <param name="actFlujoID">Identificador de la actividad de flujo</param>
        /// <param name="cerradoInd">Indica si trae la actividad con estado cerrado o abierto</param>
        /// <returns>Documento</returns>
        internal DTO_glDocumentoControl glDocumentoControl_GetByCxP(int NumDocPadre)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocumentoControl.DAL_glDocumentoControl_GetByCxP(NumDocPadre);
        }

        /// <summary>
        /// Trae un documento de la migarcion de nomina
        /// </summary>
        /// <param name="documentID">Documento del cual esta buscando el control</param>
        /// <param name="periodo">Periodo de búsqueda</param>
        /// <param name="centroPagoID">Identificador del centro de pago</param>
        /// <returns>Retorna el glDocumentoControl de la ML y laME</returns>
        internal List<DTO_glDocumentoControl> glDocumentoControl_GetByMigracionNomina(int documentID, DateTime periodo, string centroPagoID)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocumentoControl.DAL_glDocumentoControl_GetByMigracionNomina(documentID, periodo, centroPagoID);
        }

        /// <summary>
        /// Trae la lista de documentos para ajuste de saldos en un periodo
        /// </summary>
        /// <param name="documentID">Documento del cual esta buscando el control</param>
        /// <param name="periodo">Periodo de búsqueda</param>
        /// <returns>Retorna el glDocumentoControl de la ML y laME</returns>
        internal List<DTO_glDocumentoControl> glDocumentoControl_GetByPeriodoDocumento(int documentID, DateTime periodo)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocumentoControl.DAL_glDocumentoControl_GetByPeriodoDocumento(documentID, periodo);
        }

        /// <summary>
        /// Obtiene la liquiadcion documento aociado al tipo de documento y periodo para un empleado
        /// </summary>
        /// <param name="documentID">documentoID</param>
        /// <param name="empleadoID">empleadoID</param>
        /// <param name="periodo">periodo</param>
        /// <returns>Documento</returns>
        internal List<DTO_glDocumentoControl> glDocumentoControl_GetDocEmpleado(int documentID, string terceroID)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocumentoControl.DAL_glDocumentoControl_GetDocEmpleado(documentID, terceroID);
        }

        /// <summary>
        /// Actualiza el periodo
        /// </summary>
        /// <param name="numeroDoc">periodo</param>
        /// <returns></returns>
        internal void glDocumentoControl_UpdatePeriodo(int numeroDoc, DateTime periodo)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_glDocumentoControl.DAL_glDocumentoControl_UpdatePeriodo(numeroDoc, periodo);
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Agrega un registro a glDocumentoControl y guarda en las bitacoras
        /// </summary>
        /// <param name="documentID">Identificador del documento que inicia el documento</param>
        /// <param name="docCtrl">Documento que se va a insertar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResultDetail glDocumentoControl_Add(int documentID, DTO_glDocumentoControl docCtrl, bool insideAnotherTx,bool saltaActFlujo=false)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.Message = ResultValue.OK.ToString();
            bool validDto = true;

            try
            {
                string msg_FkNotFound = DictionaryMessages.FkNotFound;
                #region Validar FKs

                #region glAreaFuncional
                DAL_MasterSimple dalMasterSimple = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                dalMasterSimple.DocumentID = AppMasters.glAreaFuncional;
                DTO_MasterBasic dtoMasterBasicAreaFuncional = dalMasterSimple.DAL_MasterSimple_GetByID(new UDT_BasicID() { Value = docCtrl.AreaFuncionalID.Value }, true);
                if (!string.IsNullOrWhiteSpace(docCtrl.AreaFuncionalID.Value) && dtoMasterBasicAreaFuncional == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "AreaFuncionalID";
                    rdF.Message = msg_FkNotFound + "&&" + docCtrl.AreaFuncionalID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }
                #endregion
                #region glPrefijo
                dalMasterSimple = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                dalMasterSimple.DocumentID = AppMasters.glPrefijo;
                DTO_MasterBasic dtoMasterBasicPrefijo = dalMasterSimple.DAL_MasterSimple_GetByID(new UDT_BasicID() { Value = docCtrl.PrefijoID.Value }, true);
                if (!string.IsNullOrWhiteSpace(docCtrl.PrefijoID.Value) && dtoMasterBasicPrefijo == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "PrefijoID";
                    rdF.Message = msg_FkNotFound + "&&" + docCtrl.PrefijoID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }

                #endregion
                #region glMoneda
                dalMasterSimple.DocumentID = AppMasters.glMoneda;
                DTO_MasterBasic dtoMasterBasicGlMoneda = dalMasterSimple.DAL_MasterSimple_GetByID(new UDT_BasicID() { Value = docCtrl.MonedaID.Value }, true);
                if (!string.IsNullOrWhiteSpace(docCtrl.MonedaID.Value) && dtoMasterBasicGlMoneda == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "MonedaID";
                    rdF.Message = msg_FkNotFound + "&&" + docCtrl.MonedaID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }
                #endregion
                #region coComprobante
                dalMasterSimple.DocumentID = AppMasters.coComprobante;
                DTO_MasterBasic dtoMasterBasicComprobante = dalMasterSimple.DAL_MasterSimple_GetByID(new UDT_BasicID() { Value = docCtrl.ComprobanteID.Value }, true);
                if (!string.IsNullOrWhiteSpace(docCtrl.ComprobanteID.Value) && dtoMasterBasicComprobante == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "ComprobanteID";
                    rdF.Message = msg_FkNotFound + "&&" + docCtrl.ComprobanteID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }
                #endregion
                #region coTercero
                dalMasterSimple.DocumentID = AppMasters.coTercero;
                DTO_MasterBasic dtoMasterBasicTercero = dalMasterSimple.DAL_MasterSimple_GetByID(new UDT_BasicID() { Value = docCtrl.TerceroID.Value }, true);
                if (!string.IsNullOrWhiteSpace(docCtrl.TerceroID.Value) && dtoMasterBasicTercero == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "TerceroID";
                    rdF.Message = msg_FkNotFound + "&&" + docCtrl.TerceroID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }
                #endregion
                #region coPlanCuenta
                dalMasterSimple.DocumentID = AppMasters.coPlanCuenta;
                DTO_MasterBasic dtoMasterBasicCoplanCta = dalMasterSimple.DAL_MasterSimple_GetByID(new UDT_BasicID() { Value = docCtrl.CuentaID.Value }, true);
                if (!string.IsNullOrWhiteSpace(docCtrl.CuentaID.Value) && dtoMasterBasicCoplanCta == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "CuentaID";
                    rdF.Message = msg_FkNotFound + "&&" + docCtrl.CuentaID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }
                #endregion
                #region coProyecto
                dalMasterSimple.DocumentID = AppMasters.coProyecto;
                DTO_MasterBasic dtoMasterBasicProyecto = dalMasterSimple.DAL_MasterSimple_GetByID(new UDT_BasicID() { Value = docCtrl.ProyectoID.Value }, true);
                if (!string.IsNullOrWhiteSpace(docCtrl.ProyectoID.Value) && dtoMasterBasicProyecto == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "ProyectoID";
                    rdF.Message = msg_FkNotFound + "&&" + docCtrl.ProyectoID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }
                #endregion
                #region coCentroCosto
                dalMasterSimple.DocumentID = AppMasters.coCentroCosto;
                DTO_MasterBasic dtoMasterBasicCentroCsto = dalMasterSimple.DAL_MasterSimple_GetByID(new UDT_BasicID() { Value = docCtrl.CentroCostoID.Value }, true);
                if (!string.IsNullOrWhiteSpace(docCtrl.CentroCostoID.Value) && dtoMasterBasicCentroCsto == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "CentroCostoID";
                    rdF.Message = msg_FkNotFound + "&&" + docCtrl.CentroCostoID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }
                #endregion
                #region plLineaPresupuesto
                dalMasterSimple.DocumentID = AppMasters.plLineaPresupuesto;
                DTO_MasterBasic dtoMasterBasicLineaPre = dalMasterSimple.DAL_MasterSimple_GetByID(new UDT_BasicID() { Value = docCtrl.LineaPresupuestoID.Value }, true);
                if (!string.IsNullOrWhiteSpace(docCtrl.LineaPresupuestoID.Value) && dtoMasterBasicLineaPre == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "LineaPresupuestoID";
                    rdF.Message = msg_FkNotFound + "&&" + docCtrl.LineaPresupuestoID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }
                #endregion
                #region glLugarGeografico
                dalMasterSimple.DocumentID = AppMasters.glLugarGeografico;
                DTO_MasterBasic dtoMasterBasicLGeo = dalMasterSimple.DAL_MasterSimple_GetByID(new UDT_BasicID() { Value = docCtrl.LugarGeograficoID.Value }, true);
                if (!string.IsNullOrWhiteSpace(docCtrl.LugarGeograficoID.Value) && dtoMasterBasicLGeo == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "LugarGeograficoID";
                    rdF.Message = msg_FkNotFound + "&&" + docCtrl.LineaPresupuestoID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }
                #endregion

                if (!validDto)
                {
                    rd.Message = "NOK";
                    return rd;
                }

                #endregion

                this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int numDoc = this._dal_glDocumentoControl.DAL_glDocumentoControl_Add(docCtrl);
                if (numDoc != 0)
                {
                    rd.Key = numDoc.ToString();
                    #region Guarda en la bitacora
                    this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, docCtrl.DocumentoID.Value.Value, (int)FormsActions.Add, DateTime.Now,
                        this.UserId, this.Empresa.ID.Value, docCtrl.DocumentoID.Value.Value.ToString(), numDoc.ToString(), string.Empty,
                        string.Empty, string.Empty, 0, 0, 0);
                    #endregion
                    #region Revisa que un documento solo tenga una alarma y asigna el flujo
                    if (!saltaActFlujo)
                    {
                        List<string> actividades = this.glActividadFlujo_GetActividadesByDocumentID(documentID);
                        if (actividades.Count == 1)
                        {
                            DTO_TxResult result = this.AsignarFlujo(documentID, numDoc, actividades[0], false, docCtrl.Observacion.Value);
                            if (result.Result == ResultValue.NOK)
                                rd.Message = result.ResultMessage;
                        }
                        else if (actividades.Count > 1)
                        {
                            rd.Message = DictionaryMessages.Err_Gl_DocMultActivities + "&&" + docCtrl.DocumentoID.Value.Value.ToString();
                            return rd;
                        }
                    }
                    #endregion
                }

                return rd;
            }
            catch (Exception ex)
            {
                //Log error
                rd.Message = "NOK";
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ModuloGlobal_glDocumentoControl_Add");
                return rd;
            }
            finally
            {
                if (rd.Message == ResultValue.OK.ToString())
                {
                    if (!insideAnotherTx)
                        this._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    this._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Edita un registro al control de documentos
        /// </summary>
        /// <param name="docCtrl">Documento que se va a editar</param>
        /// <param name="updBitacora">Indica si se debe actualizar la bitacora</param>
        /// <returns></returns>
        public DTO_TxResultDetail glDocumentoControl_Update(DTO_glDocumentoControl docCtrl, bool updBitacora, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.Message = "OK";

            try
            {
                this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glDocumentoControl.DAL_glDocumentoControl_Update(docCtrl);

                if (updBitacora)
                {
                    #region Guarda en la bitacora
                    this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, docCtrl.DocumentoID.Value.Value, (int)FormsActions.Add, DateTime.Now,
                        this.UserId, this.Empresa.ID.Value, docCtrl.DocumentoID.Value.Value.ToString(), docCtrl.NumeroDoc.Value.ToString(), string.Empty,
                        string.Empty, string.Empty, 0, 0, 0);
                    #endregion
                    #region Guarda en la bitacora de documentos (gltareasControl)

                    DTO_glActividadControl actCtrl = new DTO_glActividadControl();
                    actCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                    actCtrl.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                    actCtrl.DocumentoID.Value = docCtrl.DocumentoID.Value;
                    actCtrl.ActividadFlujoID.Value = docCtrl.DocumentoID.Value.ToString();
                    actCtrl.UsuarioID.Value = this.UserId;
                    //actCtrl.CompAnula.Value =  UDT_ComprobanteID();
                    //actCtrl.CompNroAnula.Value =  UDT_Consecutivo();
                    actCtrl.Fecha.Value = DateTime.Now;
                    actCtrl.Observacion.Value = docCtrl.Observacion.Value;
                    actCtrl.AlarmaInd.Value = false;

                    this.AgregarActividadControl(actCtrl);

                    #endregion
                }

                return rd;
            }
            catch (Exception ex)
            {
                //Log error
                rd.Message = "NOK";
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ModuloGlobal_glDocumentoControl_Update");
                return rd;
            }
            finally
            {
                if (rd.Message == ResultValue.OK.ToString())
                {
                    if (!insideAnotherTx)
                        this._mySqlConnectionTx.Commit();
                }
                else
                {
                    rd.Message = ResultValue.NOK.ToString();
                    if (base._mySqlConnectionTx != null && !insideAnotherTx)
                        this._mySqlConnectionTx.Rollback();
                }
            }
        }

        /// <summary>
        /// Anula un documento (Cuando aun no esta aprobado)
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la operacion</param>
        /// <param name="numerosDoc">Pk del documento a anular</param>
        /// <returns>Retorna el resultado</returns>
        public DTO_TxResult glDocumentoControl_Anular(int documentID, List<int> numerosDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            decimal porcTotal = 0;
            decimal porcParte = 100 / numerosDoc.Count != 0? numerosDoc.Count : 1;

            try
            {
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                foreach (var num in numerosDoc)
                {
                    DTO_glDocumentoControl docCtrl = this.glDocumentoControl_GetByID(num);
                    if (docCtrl != null)
                    {
                        EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), docCtrl.Estado.Value.Value.ToString());
                        if (estado != EstadoDocControl.Aprobado)
                        {
                            DTO_glDocumento doc = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, docCtrl.DocumentoID.Value.Value.ToString(), true, true);

                            #region Revisa si tiene comprobantePre y contabiliza en 0$ si existe
                            if (!string.IsNullOrEmpty(docCtrl.ComprobanteID.Value))
                            {
                                DTO_Comprobante auxiliarPre = this._moduloContabilidad.Comprobante_GetAll(num, true, docCtrl.PeriodoDoc.Value.Value, docCtrl.ComprobanteID.Value, null); 
                                if (auxiliarPre != null && auxiliarPre.Footer.Count > 0)
                                {
                                    //Borra el auxiliar premilinar
                                    this._moduloContabilidad.BorrarAuxiliar_Pre(docCtrl.PeriodoDoc.Value.Value, docCtrl.ComprobanteID.Value, docCtrl.ComprobanteIDNro.Value.Value);
                                }
                                //Obtiene la cuenta de la contrapartida
                                DTO_ComprobanteFooter compAnul = auxiliarPre.Footer.Find(x => x.CuentaID.Value == docCtrl.CuentaID.Value);
                                if (compAnul != null)
                                {
                                    //Contabiliza el comprobante anulado en $0
                                    compAnul.vlrBaseML.Value = 0;
                                    compAnul.vlrBaseME.Value = 0;
                                    compAnul.vlrMdaLoc.Value = 0;
                                    compAnul.vlrMdaExt.Value = 0;
                                    compAnul.vlrMdaOtr.Value = 0;
                                    compAnul.Descriptivo.Value = string.Empty;
                                    compAnul.DatoAdd4.Value = string.Empty;
                                    auxiliarPre.Footer.RemoveAll(x=>x.CuentaID.Value != docCtrl.CuentaID.Value);
                                    this._moduloContabilidad.AgregarAuxiliar(auxiliarPre);
                                }
                            }  
                            #endregion  
                            #region Actualiza glDocumentoControl
                            //Cambia el estado
                            ModulesPrefix mod = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), doc.ModuloID.Value.ToLower());
                            this.glDocumentoControl_ChangeDocumentStatus(documentID, num, EstadoDocControl.Anulado, EstadoDocControl.Anulado.ToString(), true);
                            porcTotal += porcParte;
                            batchProgress[tupProgress] = (int)porcTotal;

                            #endregion
                            #region Elimina el flujo de tareas
                            this.DeshabilitarAlarma(docCtrl.NumeroDoc.Value.Value, string.Empty);
                            #endregion
                            #region Trae todos los hijos asociados al documento
                            this._dal_glDocumentoControl = (DAL_glDocumentoControl)this.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            List<int> childs = this._dal_glDocumentoControl.DAL_glDocumentoControl_GetChilds(num);
                            if (childs.Count > 0)
                            {
                                porcTotal = 0;
                                porcParte = 100 / childs.Count;
                                foreach (int child in childs)
                                {
                                    DTO_glDocumentoControl docCtrlChild = this.glDocumentoControl_GetByID(child);
                                    if (docCtrlChild != null)
                                    {
                                        //Actualiza el documento de anulacion
                                        docCtrlChild.DocumentoAnula.Value = documentID;
                                        this.glDocumentoControl_Update(docCtrlChild, false, true);
                                        //Cambia el estado
                                        this.glDocumentoControl_ChangeDocumentStatus(documentID, child, EstadoDocControl.Anulado, EstadoDocControl.Anulado.ToString(), true);

                                        #region Guarda en la bitacora de documentos (gltareasControl)
                                        DTO_glActividadControl actCtrl = new DTO_glActividadControl();
                                        actCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                                        actCtrl.NumeroDoc.Value = num;
                                        actCtrl.DocumentoID.Value = docCtrl.DocumentoID.Value;
                                        actCtrl.ActividadFlujoID.Value = docCtrl.DocumentoID.Value.Value.ToString();
                                        actCtrl.UsuarioID.Value = this.UserId;
                                        actCtrl.Fecha.Value = DateTime.Now;
                                        actCtrl.Observacion.Value = EstadoDocControl.Anulado.ToString();
                                        actCtrl.AlarmaInd.Value = false;

                                        this.AgregarActividadControl(actCtrl);
                                        #endregion
                                        #region Elimina el flujo de tareas
                                        this.DeshabilitarAlarma(child, string.Empty);
                                        #endregion
                                    }
                                    else
                                    {
                                        result.Result = ResultValue.NOK;
                                        result.ResultMessage = DictionaryMessages.Err_Gl_UpdChilds;
                                        return result;
                                    }

                                    porcTotal += porcParte;
                                    batchProgress[tupProgress] = (int)porcTotal;
                                }
                            }
                            #endregion
                            
                            List<string> actAnul = this.glActividadFlujo_GetActividadesByDocumentID(documentID);
                            if (actAnul.Count > 0)
                                result = this.AsignarFlujo(documentID, num, actAnul[0], false, "Anulacion Documento");

                            porcTotal += porcParte;
                            batchProgress[tupProgress] = (int)porcTotal;
                        }
                        else
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_UpdateDocument;
                            return result;
                        }
                    }
                    else
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_GettingDocument;
                        return result;
                    } 
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glDocumentoControl_Anular");

                return result;
            }
            finally
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = 100;

                if (result.Result == ResultValue.OK)
                {
                    result.ResultMessage = "OK";
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Anula un documento
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la operacion</param>
        /// <param name="numeroDoc">Pk del documento a anular</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado</returns>
        public DTO_TxResult glDocumentoControl_Revertir(int documentID, int numeroDoc, int? consecutivoPos, ref List<DTO_glDocumentoControl> ctrls,
            ref List<DTO_coComprobante> coComps, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            #region Inicia las variables globales

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            if (!consecutivoPos.HasValue)
            {
                consecutivoPos = 0;
                ctrls = new List<DTO_glDocumentoControl>();
                coComps = new List<DTO_coComprobante>();
            }

            #endregion
            try
            {
                this._dal_glDocumentoControl = (DAL_glDocumentoControl)this.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_glDocumentoControl ctrlAnula = null;
                DTO_coComprobante coCompAnula = null;
                DTO_glDocumentoControl ctrlOld = this.glDocumentoControl_GetByID(numeroDoc);

                #region Validaciones

                //Valida que el documento exista
                if (ctrlOld == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_GettingDocument;
                    return result;
                }

                //Valida que no tenga padres asociados
                if (consecutivoPos == 0)
                {
                    bool hasParent = this._dal_glDocumentoControl.DAL_glDocumentoControl_HasParent(numeroDoc);
                    if (hasParent)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Gl_HasParents;
                        return result;
                    }
                }

                //Valida el estado
                EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), ctrlOld.Estado.Value.Value.ToString());
                DTO_glDocumento doc = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, ctrlOld.DocumentoID.Value.Value.ToString(), true, true);
                if (estado != EstadoDocControl.Aprobado)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = consecutivoPos == 0 ? DictionaryMessages.Err_Gl_DocNoApr :
                        DictionaryMessages.Err_Gl_DocChildNoApr + "&&(" + doc.ID.Value + ")" + doc.Descriptivo.Value;
                    return result;
                }

                //Verifica que tenga documento de reversion
                if (string.IsNullOrWhiteSpace(doc.DocAnula.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Gl_NoDocAnula + "&&(" + doc.ID.Value + ")" + doc.Descriptivo.Value;
                    return result;
                }

                #endregion
                #region Trae y revierte todos los hijos asociados al documento

                this._dal_glDocumentoControl = (DAL_glDocumentoControl)this.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<int> childs = this._dal_glDocumentoControl.DAL_glDocumentoControl_GetChilds(numeroDoc);
                foreach (int child in childs)
                {
                    consecutivoPos += 1;
                    DTO_glDocumentoControl ctrlChild = this.glDocumentoControl_GetByID(child);

                    switch (ctrlChild.DocumentoID.Value.Value)
                    {
                        #region Cartera
                        case AppDocuments.LiquidacionCredito:
                            this._moduloCartera = (ModuloCartera)this.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloCartera.LiquidacionCredito_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.RecaudosManuales:
                            this._moduloCartera = (ModuloCartera)this.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloCartera.CarteraPagos_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.PagosTotales:
                            this._moduloCartera = (ModuloCartera)this.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloCartera.CarteraPagos_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.RecaudosMasivos:
                            this._moduloCartera = (ModuloCartera)this.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloCartera.CarteraPagos_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.VentaCartera:
                            this._moduloCartera = (ModuloCartera)this.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloCartera.VentaCartera_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.CobroJuridico:
                            this._moduloCartera = (ModuloCartera)this.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloCartera.EnvioCJ_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.AcuerdoPago:
                            this._moduloCartera = (ModuloCartera)this.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloCartera.EnvioCJ_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.AcuerdoPagoIncumplido:
                            this._moduloCartera = (ModuloCartera)this.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloCartera.EnvioCJ_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.Desistimiento:
                            this._moduloCartera = (ModuloCartera)this.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloCartera.Credito_Desistimiento_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.Incorporacion:
                            this._moduloCartera = (ModuloCartera)this.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloCartera.IncorporacionCredito_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.DesIncorporacion:
                            this._moduloCartera = (ModuloCartera)this.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloCartera.DesIncorporacion_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.ReincorporacionCartera:
                            this._moduloCartera = (ModuloCartera)this.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloCartera.Reincorporacion_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;

                        #endregion
                        #region CxP
                        case AppDocuments.CausarFacturas:
                            this._moduloCuentasXPagar = (ModuloCuentasXPagar)this.GetInstance(typeof(ModuloCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloCuentasXPagar.CuentasXPagar_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        #endregion
                        #region Proveedores
                        case AppDocuments.Solicitud:
                            this._moduloProveedores = (ModuloProveedores)this.GetInstance(typeof(ModuloProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloProveedores.Proveedores_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.SolicitudDirecta:
                            this._moduloProveedores = (ModuloProveedores)this.GetInstance(typeof(ModuloProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloProveedores.Proveedores_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.OrdenCompra:
                            this._moduloProveedores = (ModuloProveedores)this.GetInstance(typeof(ModuloProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloProveedores.Proveedores_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.Recibido:
                            this._moduloProveedores = (ModuloProveedores)this.GetInstance(typeof(ModuloProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloProveedores.Proveedores_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.ConsumoProyecto:
                            this._moduloProveedores = (ModuloProveedores)this.GetInstance(typeof(ModuloProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloProveedores.Proveedores_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.SolicitudDespachoConvenio:
                            this._moduloProveedores = (ModuloProveedores)this.GetInstance(typeof(ModuloProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloProveedores.Proveedores_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        #endregion
                        #region Inventarios
                        case AppDocuments.TransaccionManual:
                            this._moduloInventarios = (ModuloInventarios)this.GetInstance(typeof(ModuloInventarios), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloInventarios.Inventarios_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.TransaccionAutomatica:
                            this._moduloInventarios = (ModuloInventarios)this.GetInstance(typeof(ModuloInventarios), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloInventarios.Inventarios_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        #endregion
                        #region Planeacion
                        case AppDocuments.Presupuesto:
                            this._moduloPlaneacion = (ModuloPlaneacion)this.GetInstance(typeof(ModuloPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloPlaneacion.Planeacion_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.AdicionPresupuesto:
                            this._moduloPlaneacion = (ModuloPlaneacion)this.GetInstance(typeof(ModuloPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloPlaneacion.Planeacion_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.ReclasifPresupuesto:
                            this._moduloPlaneacion = (ModuloPlaneacion)this.GetInstance(typeof(ModuloPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloPlaneacion.Planeacion_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        case AppDocuments.TrasladoPresupuesto:
                            this._moduloPlaneacion = (ModuloPlaneacion)this.GetInstance(typeof(ModuloPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloPlaneacion.Planeacion_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                        #endregion
                        default:
                            result = this.glDocumentoControl_Revertir(documentID, child, consecutivoPos, ref ctrls, ref coComps, true);
                            break;
                    }

                    if (result.Result == ResultValue.NOK)
                        return result;
                }

                #endregion
                #region Crea el documento de anulacion
                ModulesPrefix mod = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), doc.ModuloID.Value.ToLower());
                string periodoStr = this.GetControlValueByCompany(mod, AppControl.co_Periodo);
                DateTime periodo = Convert.ToDateTime(periodoStr);
                DateTime fechaDoc = DateTime.Now;
                if (fechaDoc.Year != periodo.Year || fechaDoc.Month != periodo.Month)
                    fechaDoc = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));

                ctrlAnula = ObjectCopier.Clone(ctrlOld);
                ctrlAnula.NumeroDoc.Value = 0;
                ctrlAnula.DocumentoTipo.Value = (int)DocumentoTipo.DocInterno;
                ctrlAnula.DocumentoNro.Value = 0;
                ctrlAnula.DocumentoID.Value = Convert.ToInt32(doc.DocAnula.Value);
                ctrlAnula.PeriodoDoc.Value = periodo;
                ctrlAnula.FechaDoc.Value = fechaDoc;
                ctrlAnula.PeriodoAnula.Value = ctrlOld.PeriodoDoc.Value;
                ctrlAnula.DocumentoAnula.Value = ctrlOld.NumeroDoc.Value;
                ctrlAnula.ComprobanteID.Value = string.Empty;
                ctrlAnula.ComprobanteIDNro.Value = 0;
                ctrlAnula.Descripcion.Value = "REVERSION DE DOCUMENTO: " + ctrlOld.NumeroDoc.Value.Value.ToString(); ;

                this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int numDoc = this._dal_glDocumentoControl.DAL_glDocumentoControl_Add(ctrlAnula);
                ctrlAnula.NumeroDoc.Value = numDoc;

                #endregion
                #region Revierte el/los comprobante (si existe)

                if (!string.IsNullOrWhiteSpace(ctrlOld.ComprobanteID.Value))
                {
                    List<Tuple<string, int>> comprobantes = this._moduloContabilidad.Comprobante_GetComprobantesByNumeroDoc(ctrlOld.NumeroDoc.Value.Value);
                    foreach (Tuple<string, int> tupla in comprobantes)
                    {
                        DTO_Comprobante compOld = this._moduloContabilidad.Comprobante_Get(true, false, ctrlOld.PeriodoDoc.Value.Value,
                            tupla.Item1, tupla.Item2, null, null);

                        result = this._moduloContabilidad.Comprobante_Revertir(ctrlAnula, compOld, mod, ref coCompAnula);
                        if (result.Result == ResultValue.NOK)
                            return result;
                    }

                    ctrlAnula.ComprobanteID.Value = coCompAnula.ID.Value;
                    this.glDocumentoControl_Update(ctrlAnula, false, true);
                }

                #endregion
                #region Revierte el documento existente

                //Actualiza el documento de anulacion
                ctrlOld.DocumentoAnula.Value = ctrlAnula.NumeroDoc.Value;
                ctrlOld.PeriodoAnula.Value = ctrlAnula.PeriodoDoc.Value;
                this.glDocumentoControl_Update(ctrlOld, false, true);

                //Cambia el estado
                this.glDocumentoControl_ChangeDocumentStatus(documentID, numeroDoc, EstadoDocControl.Revertido, EstadoDocControl.Revertido.ToString(), true);

                #endregion
                #region Actualiza el control de actividades

                // Guarda en la bitacora de documentos (glActividadControl)
                DTO_glActividadControl actCtrl = new DTO_glActividadControl();
                actCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                actCtrl.NumeroDoc.Value = ctrlOld.NumeroDoc.Value.Value;
                actCtrl.DocumentoID.Value = documentID;
                actCtrl.ActividadFlujoID.Value = ctrlOld.DocumentoID.Value.Value.ToString();
                actCtrl.UsuarioID.Value = this.UserId;
                actCtrl.Fecha.Value = DateTime.Now;
                actCtrl.Observacion.Value = EstadoDocControl.Radicado.ToString();
                actCtrl.AlarmaInd.Value = false;

                if (!string.IsNullOrWhiteSpace(ctrlOld.ComprobanteID.Value))
                {
                    actCtrl.CompAnula.Value = ctrlOld.ComprobanteID.Value;
                    actCtrl.CompNroAnula.Value = ctrlOld.ComprobanteIDNro.Value;
                }

                this.AgregarActividadControl(actCtrl);

                #endregion
                #region Elimina el flujo de tareas
                this.DeshabilitarAlarma(ctrlOld.NumeroDoc.Value.Value, string.Empty);
                #endregion

                ctrls.Add(ctrlAnula);
                coComps.Add(coCompAnula);
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glDocumentoControl_Revertir");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        #region Commit y consecutivos

                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        for (int i = 0; i < ctrls.Count; ++i)
                        {
                            DTO_glDocumentoControl ctrlAnula = ctrls[i];
                            DTO_coComprobante coCompAnula = coComps[i];

                            //Obtiene el consecutivo del comprobante (cuando existe)
                            ctrlAnula.DocumentoNro.Value = this.GenerarDocumentoNro(ctrlAnula.DocumentoID.Value.Value, ctrlAnula.PrefijoID.Value);
                            if (coCompAnula != null)
                                ctrlAnula.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coCompAnula, ctrlAnula.PrefijoID.Value, ctrlAnula.PeriodoDoc.Value.Value, ctrlAnula.DocumentoNro.Value.Value);

                            this.ActualizaConsecutivos(ctrlAnula, true, coCompAnula != null, false);
                            if (coCompAnula != null)
                                this._moduloContabilidad.ActualizaComprobanteNro(ctrlAnula.NumeroDoc.Value.Value, ctrlAnula.ComprobanteIDNro.Value.Value, false);
                        }

                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
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
        public int glDocumentoControl_ChangeDocumentStatus(int documentoID, int numeroDoc, EstadoDocControl estado, string obs, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            bool isValid = true;
            try
            {
                DTO_glDocumentoControl docCtrl = this.glDocumentoControl_GetByID(numeroDoc);
                if (docCtrl != null)
                {
                    #region Cambia estado del glDocumentoControl

                    this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_glDocumentoControl.DAL_glDocumentoControl_ChangeDocumentStatus(numeroDoc, estado, obs);

                    #endregion
                    #region Guarda en la bitacora

                    this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    int bId = this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Edit, DateTime.Now,
                        this.UserId, this.Empresa.ID.Value, docCtrl.NumeroDoc.Value.Value.ToString(), docCtrl.DocumentoID.Value.Value.ToString(), string.Empty,
                        string.Empty, string.Empty, 0, 0, 0);

                    #endregion

                    return bId;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                isValid = false;
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloGlobal_glDocumentoControl_ChangeDocumentStatus");
                throw exception;
            }
            finally
            {
                if (isValid)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else
                {
                    if (base._mySqlConnectionTx != null && !insideAnotherTx)
                        base._mySqlConnectionTx.Rollback();
                }
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
        public DTO_glDocumentoControl glDocumentoControl_GetByID(int numeroDoc)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocumentoControl.DAL_glDocumentoControl_GetByID(numeroDoc);
        }

        /// <summary>
        /// Trae un registro de glDocumentoControl
        /// </summary>
        /// <param name="documentID">Identificadior del documento</param>
        /// <param name="idPrefijo">Identificador del prefijo</param>
        /// <param name="documentoNro">Nunero de documento</param>
        /// <returns></returns>
        public DTO_glDocumentoControl glDocumentoControl_GetInternalDoc(int documentID, string idPrefijo, int documentoNro)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocumentoControl.DAL_glDocumentoControl_GetInternalDoc(documentID, idPrefijo, documentoNro);
        }

        /// <summary>
        /// Trae un registro de glDocuemntoControl
        /// </summary>
        /// <param name="cuentaID">Identificadior de la cuenta</param>
        /// <param name="idTercero">Identificador del tercero</param>
        /// <param name="DocumentoTercero">Documento del tercero</param>
        /// <returns>Retorna el documento</returns>
        public DTO_glDocumentoControl glDocumentoControl_GetExternalDoc(int documentID, string idTercero, string DocumentoTercero)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocumentoControl.DAL_glDocumentoControl_GetExternalDoc(documentID, idTercero, DocumentoTercero);
        }

        /// <summary>
        /// Trae un registro de glDocumentoControl
        /// </summary>
        /// <param name="cuentaID">Identificadior de la cuenta</param>
        /// <param name="idPrefijo">Identificador del prefijo</param>
        /// <param name="documentoNro">Nunero de documento</param>
        /// <returns></returns>
        public DTO_glDocumentoControl glDocumentoControl_GetInternalDocByCta(string cuentaID, string idPrefijo, int documentoNro)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocumentoControl.DAL_glDocumentoControl_GetInternalDocByCta(cuentaID, idPrefijo, documentoNro);
        }

        /// <summary>
        /// Trae un registro de glDocuemntoControl
        /// </summary>
        /// <param name="cuentaID">Identificadior de la cuenta</param>
        /// <param name="idTercero">Identificador del tercero</param>
        /// <param name="DocumentoTercero">Documento del tercero</param>
        /// <returns>Retorna el documento</returns>
        public DTO_glDocumentoControl glDocumentoControl_GetExternalDocByCta(string cuentaID, string idTercero, string DocumentoTercero)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocumentoControl.DAL_glDocumentoControl_GetExternalDocByCta(cuentaID, idTercero, DocumentoTercero);
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
        public DTO_glDocumentoControl glDocumentoControl_GetByComprobante(int documentoID, DateTime periodo, string comprobanteID, int compNro)
        {
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocumentoControl.DAL_glDocumentoControl_GetByComprobante(documentoID, periodo, comprobanteID, compNro);
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
            this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glDocumentoControl.DAL_glDocumentoControl_GetByLibranzaSolicitud(libranza, actFlujoID, cerradoInd);
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
                string periodoStr = this.GetControlValueByCompany(mod, AppControl.co_Periodo);
                DateTime periodo = Convert.ToDateTime(periodoStr);
                this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<DTO_glDocumentoControl> result = this._dal_glDocumentoControl.DAL_glDocumentoControl_GetByModulo(periodo, mod, contabilizado);
                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glDocumentoControl_GetForPosteo");
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
                this._dal_glDocumentoControl = (DAL_glDocumentoControl)base.GetInstance(typeof(DAL_glDocumentoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_glDocumentoControl> result = this._dal_glDocumentoControl.DAL_glDocumentoControl_GetByParameter(ctrl);
                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glDocumentoControl_GetByParameter");
                throw ex;
            }
        }

        #endregion

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
        ///  <param name="LLamadaInd">Si asigna valores de llamadas</param>
        /// <param name="estadoTareaInd">Indica si esta cerrada o no</param>
        /// <returns></returns>
        public List<DTO_InfoTarea> glActividadEstado_GetPendientesByParameter(int? numeroDoc, int? documentoID, string actFlujoID, DateTime? fechaIni,
            DateTime? fechaFin, string terceroID, string prefijoID, int? docNro, EstadoTareaIncumplimiento tipo, bool llamadaInd, bool? vencidas)
        {
            try
            {
                this._dal_glActividadEstado = (DAL_glActividadEstado)base.GetInstance(typeof(DAL_glActividadEstado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_InfoTarea> result = this._dal_glActividadEstado.DAL_glActividadEstado_GetPendientesByParameter(numeroDoc, documentoID, actFlujoID, fechaIni, 
                    fechaFin, terceroID, prefijoID, docNro, tipo);

                if (llamadaInd)
                    result = result.FindAll(x => !string.IsNullOrEmpty(x.LlamadaID.Value));

                foreach (DTO_InfoTarea tarea in result)
                {
                    //Obtiene los Dias de diferencia para incumplimiento
                    int difDias = tarea.FechaFin.Value != null ? DateTime.Today.Day - tarea.FechaFin.Value.Value.Day : DateTime.Today.Day - tarea.FechaInicio.Value.Value.Day;
                    tarea.Incumplimiento.Value = difDias > 0 ? difDias : 0;
                    if (tarea.UnidadTiempo.Value == (byte)UnidadTiempo.Minuto || tarea.UnidadTiempo.Value == (byte)UnidadTiempo.Hora)
                        tarea.Incumplimiento.Value = tarea.Incumplimiento.Value * 24; // Convierte a Horas

                    //Valida el tipo de documento para asignar info
                    if (tarea.DocumentoTipo.Value == (byte)DocumentoTipo.DocInterno)
                        tarea.PrefDoc.Value = tarea.PrefijoID.Value + "-" + tarea.DocumentoNro.Value.ToString();
                    else if (tarea.DocumentoTipo.Value == (byte)DocumentoTipo.DocExterno)
                        tarea.PrefDoc.Value = tarea.DocumentoTercero.Value;

                    //Valida el documento para asignar nuevo Tercero
                    if (tarea.DocumentoID.Value == AppDocuments.DatosTerceros)
                    {
                        DTO_coTercero terceroEstado = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, tarea.TerceroIDActEstado.Value, true, false);
                        tarea.TerceroID.Value = terceroEstado != null ? terceroEstado.ID.Value : tarea.TerceroID.Value;
                        tarea.TerceroDesc.Value = terceroEstado != null ? terceroEstado.Descriptivo.Value : tarea.TerceroDesc.Value;
                        tarea.PrefDoc.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto) + "-1";
                    }
                    if (llamadaInd)
                    {
                        DTO_MasterBasic llamada = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLLamadaProposito, tarea.LlamadaID.Value, true, false);
                        tarea.LlamadaDesc.Value = llamada != null ? llamada.Descriptivo.Value : string.Empty;
                    }
                }
                
                //revisar este tema 1: Vencidas 
                if (vencidas.HasValue)
                {
                    if (vencidas.Value) // cuando 1
                        result = result.FindAll(x => x.Incumplimiento.Value > 0);
                    else
                        result = result.FindAll(x => x.Incumplimiento.Value == 0);
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "glActividadEstado_GetPendientesByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega registros a actividadEstado
        /// </summary>
        /// <param name="documentID">Documento actual</param>
        /// <param name="notas">lista de notas</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult glActividadEstado_AddNotas(int documentID, List<DTO_InfoTarea> notas, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 3;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            List<DTO_glActividadEstado> activEstado = new List<DTO_glActividadEstado>();

            try
            {
                this._dal_glActividadEstado = (DAL_glActividadEstado)this.GetInstance(typeof(DAL_glActividadEstado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string af = this.GetAreaFuncionalByUser();
                string prefijoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string actividadFlujoxDef = this.GetControlValue(AppControl.ActividadFlujoRecordatorios);

                foreach (DTO_InfoTarea item in notas)
                {
                    if (item.NumeroDoc.Value == null)
                    {
                        #region Carga y Guarda glDocumentoControl
                        DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                        ctrl.DocumentoNro.Value = 0;
                        ctrl.DocumentoID.Value = documentID;
                        ctrl.NumeroDoc.Value = 0;
                        ctrl.DocumentoTipo.Value = (int)DocumentoTipo.DocInterno;
                        ctrl.Fecha.Value = DateTime.Now;
                        ctrl.FechaDoc.Value = DateTime.Now;
                        ctrl.PeriodoDoc.Value = DateTime.Now.Date;
                        ctrl.PeriodoUltMov.Value = ctrl.PeriodoDoc.Value;
                        ctrl.AreaFuncionalID.Value = af;
                        ctrl.PrefijoID.Value = prefijoDef;
                        ctrl.Valor.Value = 0;
                        ctrl.Iva.Value = 0;
                        ctrl.TasaCambioCONT.Value = 0;
                        ctrl.TasaCambioDOCU.Value = 0;
                        ctrl.Descripcion.Value = "Notas Actividad Estado";
                        ctrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                        ctrl.seUsuarioID.Value = this.UserId;

                        DTO_TxResultDetail resultGLDC = this.glDocumentoControl_Add(documentID, ctrl, true);
                        if (resultGLDC.Message != ResultValue.OK.ToString())
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = "NOK";
                            result.Details.Add(resultGLDC);
                            return result;
                        }
                        ctrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                        #endregion
                        #region Asigna y Guarda en glActividadEstado
                        DTO_glActividadEstado actEstado = new DTO_glActividadEstado();
                        actEstado.ActividadFlujoID.Value = actividadFlujoxDef;
                        actEstado.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                        actEstado.AlarmaInd.Value = false;
                        actEstado.CerradoInd.Value = item.CerradoInd.Value;
                        actEstado.FechaInicio.Value = item.FechaInicio.Value;
                        actEstado.seUsuarioID.Value = this.UserId;
                        actEstado.Observaciones.Value = item.Observaciones.Value;
                        this._dal_glActividadEstado.DAL_glActividadEstado_Add(actEstado);
                        #endregion
                    }
                    else
                    {
                        #region Asigna y Actualiza en glActividadEstado
                        DTO_glActividadEstado actEstado = new DTO_glActividadEstado();
                        actEstado.ActividadFlujoID.Value = actividadFlujoxDef;
                        actEstado.NumeroDoc.Value = item.NumeroDoc.Value;
                        actEstado.CerradoInd.Value = item.CerradoInd.Value;
                        actEstado.FechaInicio.Value = item.FechaInicio.Value;
                        actEstado.Observaciones.Value = item.Observaciones.Value;
                        this._dal_glActividadEstado.DAL_glActividadEstado_Upd(actEstado);
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                //numeroDoc = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glActividadEstado_AddNotas");
                return result;
            }
            finally
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = 100;

                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        base._mySqlConnectionTx = null;
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Agrega o actualiza registros a actividadEstado
        /// </summary>
        /// <param name="documentID">Documento actual</param>
        /// <param name="notas">lista de notas</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult glActividadEstado_AddOrUpd(int documentID,DTO_InfoTarea actividad,bool update, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 3;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            List<DTO_glActividadEstado> activEstado = new List<DTO_glActividadEstado>();

            try
            {
                this._dal_glActividadEstado = (DAL_glActividadEstado)this.GetInstance(typeof(DAL_glActividadEstado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                if (!update)
                {                        
                    #region Guarda en glActividadEstado
                    DTO_glActividadEstado actEstado = new DTO_glActividadEstado();
                    actEstado.ActividadFlujoID.Value = actividad.ActividadFlujoID.Value;
                    actEstado.NumeroDoc.Value = actividad.NumeroDoc.Value;
                    actEstado.AlarmaInd.Value = actividad.FechaAlarma1.Value.HasValue ? true : false;
                    actEstado.CerradoInd.Value = actividad.CerradoInd.Value;
                    actEstado.FechaInicio.Value = actividad.FechaInicio.Value;
                    actEstado.FechaAlarma1.Value = actividad.FechaAlarma1.Value;
                    actEstado.UsuarioAlarma1.Value = actividad.UsuarioAlarma1.Value.HasValue? this.seUsuario_GetUserByReplicaID(actividad.UsuarioAlarma1.Value.Value).ID.Value : string.Empty;
                    actEstado.FechaCerrado.Value = actividad.FechaCerrado.Value;
                    actEstado.FechaFin.Value = actividad.FechaFin.Value;
                    actEstado.TerceroID.Value = actividad.TerceroID.Value;
                    actEstado.seUsuarioID.Value = this.UserId;
                    actEstado.Observaciones.Value = actividad.Observaciones.Value;
                    this._dal_glActividadEstado.DAL_glActividadEstado_Add(actEstado);
                    #endregion
                }
                else
                {
                    #region Actualiza en glActividadEstado
                    DTO_glActividadEstado actEstado = new DTO_glActividadEstado();
                    actEstado.ActividadFlujoID.Value = actividad.ActividadFlujoID.Value;
                    actEstado.NumeroDoc.Value = actividad.NumeroDoc.Value;
                    actEstado.AlarmaInd.Value = actividad.FechaAlarma1.Value.HasValue ? true : false;
                    actEstado.CerradoInd.Value = actividad.CerradoInd.Value;
                    actEstado.FechaInicio.Value = actividad.FechaInicio.Value;
                    actEstado.FechaAlarma1.Value = actividad.FechaAlarma1.Value;
                    actEstado.UsuarioAlarma1.Value = actividad.UsuarioAlarma1.Value.HasValue ? this.seUsuario_GetUserByReplicaID(actividad.UsuarioAlarma1.Value.Value).ID.Value : string.Empty;
                    actEstado.FechaCerrado.Value = actividad.FechaCerrado.Value;
                    actEstado.FechaFin.Value = actividad.FechaFin.Value;
                    actEstado.TerceroID.Value = actividad.TerceroID.Value;
                    actEstado.seUsuarioID.Value = this.UserId;
                    actEstado.Observaciones.Value = actividad.Observaciones.Value;
                    this._dal_glActividadEstado.DAL_glActividadEstado_Upd(actEstado);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                //numeroDoc = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glActividadEstado_AddOrUpd");
                return result;
            }
            finally
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = 100;

                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        base._mySqlConnectionTx = null;
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// DEvuelve el flujo de un documento a un estado o flujo anterior
        /// </summary>
        /// <param name="documentActividad">Documento de la transaccion</param>
        /// <param name="actFlujoNueva">Actividad destino de devolucion</param>
        /// <param name="numeroDoc">id del documento</param>
        /// <param name="observacion">Observacion del proceso</param>
        public DTO_TxResult DevolverFlujoDocumento(int documentActividad, string actFlujoNueva, int? numeroDoc, string observacion, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();


            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                #region Actualiza el flujo

                List<string> actividades = this.glActividadFlujo_GetActividadesByDocumentID(documentActividad);
                string actActual = actividades[0].Trim();
                result = this.ActualizarReversionFlujo(documentActividad, numeroDoc.Value, actActual, actFlujoNueva, observacion);

                #endregion
                //#region Actualiza la solicitud
                //if (!string.IsNullOrEmpty(observacion) && numeroDoc != null)
                //{
                //    DTO_ccSolicitudDocu docu = this._dal_ccSolicitudDocu.DAL_ccSolicitudDocu_GetByNumeroDoc(numeroDoc.Value);
                //    docu.Observacion.Value = docu.ObservacionRechazo.Value;
                //    this._dal_ccSolicitudDocu.DAL_ccSolicitudDocu_Update(docu);
                //}

                //#endregion
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "SolicitudCredito_Rechazar");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        this._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    this._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region glGarantiaControl

        /// <summary>
        /// Agrega un registro al control de garantias
        /// </summary>
        public DTO_TxResult glGarantiaControl_Add(int documentID, List<DTO_glGarantiaControl> garantias, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 3;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_garantiaControl = (DAL_glGarantiaControl)this.GetInstance(typeof(DAL_glGarantiaControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                foreach (var garantia in garantias)
                    this._dal_garantiaControl.DAL_glGarantiaControl_AddOrUpdate(garantia);

            }
            catch (Exception ex)
            {
                 result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Convenio_Add");
                return result;
            }
            finally
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = 100;

                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        base._mySqlConnectionTx = null;
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
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
            this._dal_garantiaControl = (DAL_glGarantiaControl)base.GetInstance(typeof(DAL_glGarantiaControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_glGarantiaControl> result = new List<DTO_glGarantiaControl>();
            result = this._dal_garantiaControl.DAL_glGarantiaControl_GetByParameter(filter);

            foreach (DTO_glGarantiaControl garantia in result)
            {
                //Asigna datos del doc si existe
                if (!string.IsNullOrEmpty(garantia.NumeroDoc.Value.ToString()))
                {
                    DTO_glDocumentoControl doc = this.glDocumentoControl_GetByID(garantia.NumeroDoc.Value.Value);
                    if (doc.DocumentoTipo.Value == (byte)DocumentoTipo.DocInterno)
                    {
                        garantia.PrefijoID.Value = doc.PrefijoID.Value;
                        garantia.DocumentoNro.Value = doc.DocumentoNro.Value;
                        garantia.PrefDoc.Value = doc.PrefijoID.Value + "-" + doc.DocumentoNro.Value.ToString();
                    }
                    else if (doc.DocumentoTipo.Value == (byte)DocumentoTipo.DocExterno)
                    {
                        garantia.DocumentoTercero.Value = doc.DocumentoTercero.Value;
                        garantia.PrefDoc.Value = doc.DocumentoTercero.Value;
                    }
                }
                DTO_glGarantia garan = (DTO_glGarantia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glGarantia, garantia.GarantiaID.Value, true, false);
                garantia.GarantiaDesc.Value = garan != null ? garan.Descriptivo.Value : string.Empty;
                
            }

            //Filtra por documento
            if (!string.IsNullOrEmpty(prefijoID) && docNro != null)
                result = result.FindAll(x => x.PrefijoID.Value == prefijoID && x.DocumentoNro.Value == docNro).ToList();
            //Garantias Vencidas
            if (estado == 1)
                result = result.FindAll(x => x.FechaVTO.Value <= DateTime.Today).ToList();
            else if (estado == 2)
                result = result.FindAll(x => x.FechaVTO.Value >= DateTime.Today).ToList();

            return result;
        }
        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"> Filtro</param>
        /// <param name="prefijoID"> Prefijo filtro</param>
        /// <param name="docNro"> Doc nro filtro</param>
        /// <param name="estado"> Estado de la garantia</param>
        /// <returns>Lista </returns>
        public List<DTO_QueryGarantiaControl> glGarantiaControl_Decisor(int numerodoc)
        {
            this._dal_garantiaControl = (DAL_glGarantiaControl)base.GetInstance(typeof(DAL_glGarantiaControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_QueryGarantiaControl> result = new List<DTO_QueryGarantiaControl>();
            result = this._dal_garantiaControl.DAL_glGarantiaControl_Decisor(numerodoc);

            return result;
        }

        #endregion

        #region glIncumpleCambioEstado

       /// <summary>
       /// Agrega info a glIncumpleCambioEstado
       /// </summary>
       /// <param name="documentID">documento Actual</param>
       /// <param name="gestionList">gestionList</param>
       /// <returns></returns>
        public DTO_TxResult glIncumpleCambioEstado_Update(int documentID, List<DTO_GestionCobranza> gestionList, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 3;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            List<DTO_glIncumpleCambioEstado> incumplimientos = new List<DTO_glIncumpleCambioEstado>();
            try
            {
                this._dal_IncumpleCambioEst = (DAL_glIncumpleCambioEstado)this.GetInstance(typeof(DAL_glIncumpleCambioEstado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Actualiza los registros
                foreach (var item in gestionList)
                {
                    DTO_glIncumpleCambioEstado incump = this._dal_IncumpleCambioEst.DAL_glIncumpleCambioEstado_GetByConsecutivo(item.ConsEstadoIncumplido.Value.Value);
                    incump.FechaCompromiso.Value = item.FechaCompromiso.Value;
                    incump.Observaciones.Value = incump.Observaciones.Value + "///" +
                                                 DateTime.Now.Date.ToString() +"-"+
                                                 this.seUsuario_GetUserByReplicaID(this.UserId).ID.Value +"-"+ 
                                                 item.FechaCompromiso.Value.ToString() +"-"+
                                                 item.Valor1.Value.ToString() +"-"+
                                                 item.ObservacionCompromiso.Value;
                    incump.Numero1.Value = item.Numero1.Value;
                    incump.Valor1.Value = item.Valor1.Value;
                    this._dal_IncumpleCambioEst.DAL_glIncumpleCambioEstado_UpdateItem(incump);
                }
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glIncumpleCambioEstado_Add");
                return result;
            }
            finally
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = 100;

                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        base._mySqlConnectionTx = null;
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Agrega info a glIncumpleCambioEstado
        /// </summary>
        /// <param name="documentID">documento Actual</param>
        /// <param name="gestionList">gestionList</param>
        /// <returns></returns>
        public void glIncumpleCambioEstado_CierraEstado(int consecutivo, DateTime fechaFin)
        {
            this._dal_IncumpleCambioEst = (DAL_glIncumpleCambioEstado)this.GetInstance(typeof(DAL_glIncumpleCambioEstado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_IncumpleCambioEst.DAL_glIncumpleCambioEstado_CierraEstado(consecutivo, fechaFin);
        }

        #endregion

        #region glDocumentoGestionBitacora

        /// <summary>
        /// Agrega un registro al a la gestion documental
        /// </summary>
        public int glGestionDocumentalBitacora_Add(DTO_glGestionDocumentalBitacora doc)
        {
            this._dal_glGestionDocumentalBitacora = (DAL_glGestionDocumentalBitacora)base.GetInstance(typeof(DAL_glGestionDocumentalBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glGestionDocumentalBitacora.DAL_glGestionDocumentalBitacora_Add(doc);
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
            this._dal_glLlamadasControl = (DAL_glLlamadasControl)base.GetInstance(typeof(DAL_glLlamadasControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_glLlamadasControl.DAL_glLlamadasControl_GetByNumeroDoc(numeroDoc);
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
        public DTO_TxResult glLlamadasControl_Add(int documentoID, string actividadFlujoID, List<DTO_glLlamadasControl> llamadasCtrl, List<DTO_glTerceroReferencia> terceroRefs, bool sendToAprob, bool isAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!isAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            this._dal_glLlamadasControl = (DAL_glLlamadasControl)base.GetInstance(typeof(DAL_glLlamadasControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_MasterComplex = (DAL_MasterComplex)base.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_MasterComplex.DocumentID = AppMasters.glTerceroReferencia;
            try
            {
                if (llamadasCtrl.Count != 0)
                {
                    #region Actualiza los registros de glTerceroReferencia
                    foreach (DTO_glTerceroReferencia refTercero in terceroRefs)
                    {
                        if (refTercero.NuevoRegistro.Value.Value)
                        {
                            refTercero.ReplicaID.Value = null;
                            DTO_TxResultDetail resultDetails = this._dal_MasterComplex.DAL_MasterComplex_AddItem(refTercero);
                            if (resultDetails.Message == "NOK")
                            {
                                result.Details = new List<DTO_TxResultDetail>();
                                result.Details.Add(resultDetails);
                                result.Result = ResultValue.NOK;
                                return result;
                            }
                            else
                            {
                                Dictionary<string, string> dic = new Dictionary<string, string>();
                                dic.Add("EmpresaGrupoID", refTercero.EmpresaGrupoID.Value);
                                dic.Add("TerceroID", refTercero.TerceroID.Value);
                                dic.Add("TipoReferencia", refTercero.TipoReferencia.Value.ToString());
                                dic.Add("Nombre", refTercero.Nombre.Value);

                                DTO_glTerceroReferencia newRefTercero = (DTO_glTerceroReferencia)this.GetMasterComplexDTO(AppMasters.glTerceroReferencia, dic, true);
                                llamadasCtrl.Where(x => x.NombreReferencia.Value == refTercero.Nombre.Value).ToList().ForEach(y => y.NumReferencia.Value = newRefTercero.ReplicaID.Value);
                                result.ResultMessage = string.Empty;
                            }
                        }
                        else
                        {
                            result = this._dal_MasterComplex.DAL_MasterComplex_Update(refTercero, true);
                            if (result.Result == ResultValue.NOK)
                                return result;
                        }

                    }
                    #endregion
                    #region Guarda en la bitacora

                    this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    int bId = this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Edit, DateTime.Now,
                        this.UserId, this.Empresa.ID.Value, llamadasCtrl[0].NumeroDoc.Value.Value.ToString(), documentoID.ToString(), string.Empty,
                        string.Empty, string.Empty, 0, 0, 0);

                    #endregion
                    #region Agrega a las llamadas
                    //Borra la informacion ya existente
                    this._dal_glLlamadasControl.DAL_glLlamadasControl_Delete(llamadasCtrl.FirstOrDefault().NumeroDoc.Value.Value);
                    foreach (DTO_glLlamadasControl item in llamadasCtrl)
                    {
                        //Agrega la llamada a la tabla glLlamadasControl
                        this._dal_glLlamadasControl.DAL_glLlamadasControl_Add(item);
                    }
                    #endregion
                    #region Activa las alarmas
                    if (sendToAprob)
                    {
                        result = this.AsignarFlujo(documentoID, llamadasCtrl[0].NumeroDoc.Value.Value, actividadFlujoID, false, string.Empty);
                        if (result.Result == ResultValue.OK)
                            result.ResultMessage = string.Empty;
                    }
                    #endregion
                    return result;
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glLlamadasControl_Add");
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

        #endregion

        #region glMovimientoDeta

        #region Funciones Internas

        /// <summary>
        /// Trae la lista de glMovimientoDeta segun el numero de la factura asociada
        /// </summary>
        /// <param name="NumeroDoc">Numero de la factura</param>
        /// <returns></returns>
        internal List<DTO_glMovimientoDeta> glMovimientoDeta_GetByNumeroDoc(int NumeroDoc, bool trasladoNotaEnvio = false)
        {
            this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glMovimientoDeta.DAL_glMovimientoDeta_GetByNumeroDoc(NumeroDoc, trasladoNotaEnvio);
        }

        /// <summary>
        /// Trae la lista de glMovimientoDeta segun el numero de la factura asociada
        /// </summary>
        /// <param name="NumeroDoc">Numero de la factura</param>
        /// <returns></returns>
        internal List<DTO_glMovimientoDeta> glMovimientoDeta_GetByConsecutivo(int Consecutivo)
        {
            this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glMovimientoDeta.DAL_glMovimientoDeta_GetByConsecutivo(Consecutivo);
        }

        /// <summary>
        /// Adiciona registro en la tabla glMovimientoDeta 
        /// </summary>
        /// <param name="fact">Factura</param>
        /// <returns></returns>
        internal DTO_TxResult glMovimientoDeta_Add(List<DTO_glMovimientoDeta> det, bool validBodegaTransac = false, bool insideAnotherTx = false)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            bool isValid = true;
            try
            {
                this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glMovimientoDeta.DAL_glMovimientoDeta_Add(det);

                #region Valida la bodega y actualiza cada registro creado
                if (validBodegaTransac && det.Count > 0)
                {
                    List<DTO_glMovimientoDeta> detalleUpdate = this._dal_glMovimientoDeta.DAL_glMovimientoDeta_GetByNumeroDoc(det[0].NumeroDoc.Value.Value);
                    foreach (DTO_glMovimientoDeta mov in detalleUpdate)
                    {
                        if (!string.IsNullOrEmpty(mov.BodegaID.Value))
                        {
                            DTO_inBodega bod = (DTO_inBodega)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, mov.BodegaID.Value, true, false);
                            DTO_inCosteoGrupo costeo = (DTO_inCosteoGrupo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, bod.CosteoGrupoInvID.Value, true, false);
                            mov.IdentificadorTr.Value = costeo.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mov.Consecutivo.Value : 0;//Si la bodega es transaccional asigna el consecutivo 
                            this._dal_glMovimientoDeta.DAL_glMovimientoDeta_Update(mov);
                        }
                    }
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                isValid = false;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glMovimientoDeta_Add");
                return result;
            }
            finally
            {
                if (isValid)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Elimina registros de la tabla de glMovimientoDeta
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        internal DTO_TxResult glMovimientoDeta_Delete(int NumeroDoc)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glMovimientoDeta.DAL_glMovimientoDeta_Delete(NumeroDoc);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glMovimientoDeta_Delete");
                return result;
            }
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Obtiene la cantidad de registros de la tabla glMovimientoDeta
        /// </summary>
        /// <param name="consulta">Filtros</param>
        /// <returns>Retorna la cantidad de registros de la consulta</returns>
        public long glMovimientoDeta_Count(DTO_glConsulta consulta)
        {
            List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
            if (consulta != null && consulta.Filtros != null)
                filtros = consulta.Filtros;

            string where = Transformer.WhereSql(filtros, typeof(DTO_glMovimientoDeta));
            if (!string.IsNullOrWhiteSpace(where))
                where = "WHERE " + where;

            this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glMovimientoDeta.DAL_glMovimientoDeta_Count(consulta, where);
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
            int ini = (pageNum - 1) * pageSize + 1;
            int fin = pageNum * pageSize;

            List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

            if (consulta != null && consulta.Filtros != null)
                filtros = consulta.Filtros;

            string where = Transformer.WhereSql(filtros, typeof(DTO_glMovimientoDeta));
            if (!string.IsNullOrWhiteSpace(where))
                where = "WHERE " + where;

            this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_inMovimientoDocu = (DAL_inMovimientoDocu)base.GetInstance(typeof(DAL_inMovimientoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            List<DTO_glMovimientoDeta> result =  this._dal_glMovimientoDeta.DAL_glMovimientoDeta_GetPaged(pageSize, pageNum, ini, fin, consulta, where);
                 
            return result;               
              
        }

        /// <summary>
        /// Trae la lista de glMovimientoDeta con campos especificos para  los activos
        /// </summary>
        /// <param name="NumeroDoc">Numero de la factura</param>
        /// <returns></returns>
        public List<DTO_glMovimientoDeta> glMovimientoDeta_Get_ActivoFind(int NumeroDoc)
        {
            this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glMovimientoDeta.DAL_glMovimientoDeta_GetBy_ActivoFind(NumeroDoc);
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Mvto detalle</returns>
        public List<DTO_glMovimientoDeta> glMovimientoDeta_GetByParameter(DTO_glMovimientoDeta filter, bool isPre)
        {
            this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_inMovimientoDocu = (DAL_inMovimientoDocu)base.GetInstance(typeof(DAL_inMovimientoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            List<DTO_glMovimientoDeta> result = this._dal_glMovimientoDeta.DAL_glMovimientoDeta_GetByParameter(filter,isPre);
            try
            {               
                foreach (var item in result)
                {                      
                    item.Prefijo_Documento.Value = item.PrefijoID.Value + "-" + item.DocumentoNro.Value.ToString();
                    item.EntradaSalidaLetras.Value = item.EntradaSalida.Value.HasValue? item.EntradaSalida.Value.Value == 1 ? "E" : "S" : null;
                } 
                //Filtra por fechas
                if (filter.FechaIni.Value.HasValue)
                    result = result.FindAll(x => x.Fecha.Value >= filter.FechaIni.Value).ToList();
                if (filter.FechaFin.Value.HasValue)
                    result = result.FindAll(x => x.Fecha.Value <= filter.FechaFin.Value).ToList();
            }
            catch (Exception ex)
            {
                ;
            }
            return result;
        }

        /// <summary>
        /// Edita un registro de glMovimientoDeta
        /// </summary>
        /// <param name="listDeta">Lista de items que se va a editar</param>
        public DTO_TxResult glMovimientoDeta_Update(List<DTO_glMovimientoDeta> listDeta, bool insideAnotherTx = false)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            bool isValid = true;
            try
            {
                this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                foreach (DTO_glMovimientoDeta det in listDeta)
                    this._dal_glMovimientoDeta.DAL_glMovimientoDeta_Update(det);

                result.Result = ResultValue.OK;
                return result;
            }
            catch (Exception ex)
            {
                isValid = false;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glMovimientoDeta_Update");
                return result;
            }
            finally
            {
                if (isValid)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #endregion

        #region glMovimientoDetaPRE

        #region Funciones Publicas

        /// <summary>
        /// Trae la lista de glMovimientoDeta segun el numero de la factura asociada
        /// </summary>
        /// <param name="NumeroDoc">Numero de la factura</param>
        /// <returns></returns>
        public List<DTO_glMovimientoDeta> glMovimientoDetaPRE_GetNumeroDoc(int NumeroDoc)
        {
            this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glMovimientoDeta.DAL_glMovimientoDetaPRE_Get(NumeroDoc);
        }

        /// <summary>
        /// Adiciona registro en la tabla glMovimientoDeta 
        /// </summary>
        /// <param name="fact">Factura</param>
        /// <returns></returns>
        public DTO_TxResult glMovimientoDetaPRE_Add(List<DTO_glMovimientoDeta> det, bool insideAnotherTx = false)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            bool isValid = true;
            try
            {
                this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glMovimientoDeta.DAL_glMovimientoDetaPRE_Add(det);

                return result;
            }
            catch (Exception ex)
            {
                isValid = false;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glMovimientoDeta_Add");
                return result;
            }
            finally
            {
                if (isValid)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Elimina registros de la tabla de glMovimientoDeta
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public DTO_TxResult glMovimientoDetaPRE_Delete(int NumeroDoc, bool insideAnotherTx = false)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            bool isValid = true;
            try
            {
                this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glMovimientoDeta.DAL_glMovimientoDetaPRE_Delete(NumeroDoc);

                return result;
            }
            catch (Exception ex)
            {
                isValid = false;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "glMovimientoDeta_Delete");
                return result;
            }
            finally
            {
                if (isValid)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Consulta un movimientoDetaPRE relacionado con proyectos con saldos de Inventario
        /// </summary>
        /// <param name="periodo">Periodo de saldos de inventarios</param>
        /// <param name="bodega">Bodega a consultar</param>
        /// <param name="proyectoID">Proyecto a consultar</param>
        /// <returns>lista de movimientos</returns>
        public List<DTO_glMovimientoDeta> glMovimientoDetaPRE_GetSaldosInvByProyecto(DateTime periodo, string proyectoID, bool isPre)
        {
            this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_MasterSimple = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            string servicioInvent = this.GetControlValueByCompany(ModulesPrefix.fa,AppControl.fa_ServicioInventarios);
            string mvtoInvVentas = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovVentasLoc);


            string bodega1 = string.Empty, bodega2 = string.Empty;
            DTO_glConsulta consulta = new DTO_glConsulta();
            List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
            DTO_glConsultaFiltro filtro = new DTO_glConsultaFiltro()
            {
                CampoFisico = "ProyectoID",
                ValorFiltro = proyectoID,
                OperadorFiltro = OperadorFiltro.Igual
            };
            filtros.Add(filtro);
            consulta.Filtros = filtros;

            //Trae la lista actividades del flujo
            this._dal_MasterSimple.DocumentID = AppMasters.inBodega;

            long count = this._dal_MasterSimple.DAL_MasterSimple_Count(consulta,null, true);
            List<DTO_inBodega> list = this._dal_MasterSimple.DAL_MasterSimple_GetPaged(count, 1, consulta,null, true).Cast<DTO_inBodega>().ToList();

            if (list.Count > 0)
                bodega1 = list[0].ID.Value;
            if(list.Count > 1)
                bodega2 = list[1].ID.Value;

            return this._dal_glMovimientoDeta.DAL_glMovimientoDetaPRE_GetSaldosInvByProyecto(periodo, bodega1, bodega2, proyectoID, servicioInvent, mvtoInvVentas, isPre);
        }

        #endregion

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
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_glDocumentoChequeoLista = (DAL_glDocumentoChequeoLista)this.GetInstance(typeof(DAL_glDocumentoChequeoLista), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return _dal_glDocumentoChequeoLista.DAL_glDocumentoChequeoLista_GetNumeroDoc(numeroDoc);

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "glDocumentoChequeoLista_GetByNumeroDoc");
                throw exception;
            }
        }
        #endregion

        #region Consultas

        #region Funciones privadas

        /// <summary>
        /// Metodo que arma el where de la consulta de las consultas
        /// </summary>
        /// <param name="filtro">Filtros de la consulta</param>
        /// <param name="onlyId">Bandera que indica si es solo el ID</param>
        /// <param name="parameters">parametros de la consulta</param>
        /// <returns>String correspondiente al where</returns>
        private string glConsulta_WhereBuilder(DTO_glConsulta filtro, bool onlyId, List<SqlParameter> parameters)
        {
            this._dal_glConsulta = (DAL_glConsulta)base.GetInstance(typeof(DAL_glConsulta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glConsulta.DAL_glConsulta_WhereBuilder(filtro, onlyId, parameters);
        }

        /// <summary>
        /// Metodo que arma el where de la consulta de las consultas
        /// </summary>
        /// <param name="filtro">Filtros de la consulta</param>
        /// <param name="onlyId">Bandera que indica si es solo el ID</param>
        /// <param name="parameters">parametros de la consulta</param>
        /// <returns>String correspondiente al where</returns>
        private string glConsulta_UpdateSetBuilder(DTO_glConsulta filtro, List<SqlParameter> parameters)
        {
            this._dal_glConsulta = (DAL_glConsulta)base.GetInstance(typeof(DAL_glConsulta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glConsulta.DAL_glConsulta_UpdateSetBuilder(filtro, parameters);
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
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (userID.HasValue)
                filtro.seUsuarioID = userID.Value;

            string WhereBuilder = this.glConsulta_WhereBuilder(filtro, false, parameters);

            this._dal_glConsulta = (DAL_glConsulta)base.GetInstance(typeof(DAL_glConsulta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glConsulta.DAL_glConsulta_GetAll(filtro, WhereBuilder, parameters);
        }
        
        /// <summary>
        /// Obtiene una consulta especifica
        /// </summary>
        /// <param name="filtro">Informacion para fitrar la consulta</param>
        /// <returns>consulta</returns>
        public DTO_glConsulta glConsulta_Get(DTO_glConsulta filtro)
        {
            var parameters = new List<SqlParameter>();
            string WhereBuilder = this.glConsulta_WhereBuilder(filtro, true, parameters);

            this._dal_glConsulta = (DAL_glConsulta)base.GetInstance(typeof(DAL_glConsulta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_glConsulta result = this._dal_glConsulta.DAL_glConsulta_Get(filtro, WhereBuilder, parameters);
            return result;
        }

        /// <summary>
        /// Adicionar una nueva consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta adicionada</returns>
        public DTO_glConsulta glConsulta_Add(DTO_glConsulta filtro)
        {
            this._dal_glConsulta = (DAL_glConsulta)base.GetInstance(typeof(DAL_glConsulta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_glConsulta result = this._dal_glConsulta.DAL_glConsulta_Add(filtro);
            return this.glConsulta_Get(result);
        }

        /// <summary>
        /// Actualizar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a actualizar</param>
        /// <returns>consulta actualizada</returns>
        public DTO_glConsulta glConsulta_Update(DTO_glConsulta filtro)
        {
            var parameters = new List<SqlParameter>();
            string UpdateSetBuilder = this.glConsulta_UpdateSetBuilder(filtro, parameters);

            this._dal_glConsulta = (DAL_glConsulta)base.GetInstance(typeof(DAL_glConsulta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_glConsulta result = this._dal_glConsulta.DAL_glConsulta_Update(filtro, UpdateSetBuilder, parameters);
            return glConsulta_Get(result);
        }

        /// <summary>
        /// Eliminar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta eliminada</returns>
        public void glConsulta_Delete(DTO_glConsulta filtro, bool insideAnotherTx)
        {
            bool isOk = false;
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                DAL_glConsultaFiltro cf = new DAL_glConsultaFiltro(base._mySqlConnection, base._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                cf.DAL_glConsultaFiltro_DeleteByQueryID(filtro.glConsultaID);
                DAL_glConsultaSeleccion cs = new DAL_glConsultaSeleccion(base._mySqlConnection, base._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                cs.DAL_glConsultaSeleccion_DeleteByQueryID(filtro.glConsultaID);

                this._dal_glConsulta = (DAL_glConsulta)base.GetInstance(typeof(DAL_glConsulta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glConsulta.DAL_glConsulta_Delete(filtro);

                isOk = true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glConsulta_Delete");
                throw exception;
            }
            finally
            {
                if (isOk)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region glConsultaFiltro

        /// <summary>
        /// Trae todos los glConsulta
        /// </summary>
        /// <param name="filtro">Informacion del filtro de la consulta</param>
        /// <returns>Devuelve los glConsulta a partir de un filtro aplicado</returns>
        public IEnumerable<DTO_glConsultaFiltro> glConsultaFiltro_GetAll(DTO_glConsultaFiltro filtro)
        {
            var parameters = new List<SqlParameter>();
            string WhereBuilder = this.glConsultaFiltro_WhereBuilder(filtro, false, parameters);

            this._dal_glConsultaFiltro = (DAL_glConsultaFiltro)base.GetInstance(typeof(DAL_glConsultaFiltro), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glConsultaFiltro.DAL_glConsultaFiltro_GetAll(filtro, WhereBuilder, parameters);
        }

        /// <summary>
        /// Obtiene una consulta especifica
        /// </summary>
        /// <param name="filtro">Informacion para fitrar la consulta</param>
        /// <returns>consulta</returns>
        public DTO_glConsultaFiltro glConsultaFiltro_Get(DTO_glConsultaFiltro filtro)
        {
            var parameters = new List<SqlParameter>();
            string WhereBuilder = this.glConsultaFiltro_WhereBuilder(filtro, true, parameters);

            this._dal_glConsultaFiltro = (DAL_glConsultaFiltro)base.GetInstance(typeof(DAL_glConsultaFiltro), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glConsultaFiltro.DAL_glConsultaFiltro_Get(filtro, WhereBuilder, parameters);
        }

        /// <summary>
        /// Adicionar una nueva consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta adicionada</returns>
        public DTO_glConsultaFiltro glConsultaFiltro_Add(DTO_glConsultaFiltro filtro)
        {
            this._dal_glConsultaFiltro = (DAL_glConsultaFiltro)base.GetInstance(typeof(DAL_glConsultaFiltro), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_glConsultaFiltro result = this._dal_glConsultaFiltro.DAL_glConsultaFiltro_Add(filtro);
            return this.glConsultaFiltro_Get(result);
        }

        /// <summary>
        /// Actualizar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a actualizar</param>
        /// <returns>consulta actualizada</returns>
        public DTO_glConsultaFiltro glConsultaFiltro_Update(DTO_glConsultaFiltro filtro)
        {
            var parameters = new List<SqlParameter>();
            string UpdateSetBuilder = this.glConsultaFiltro_UpdateSetBuilder(filtro, parameters);

            this._dal_glConsultaFiltro = (DAL_glConsultaFiltro)base.GetInstance(typeof(DAL_glConsultaFiltro), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_glConsultaFiltro result = this._dal_glConsultaFiltro.DAL_glConsultaFiltro_Update(filtro, UpdateSetBuilder, parameters);
            return glConsultaFiltro_Get(result);
        }

        /// <summary>
        /// Eliminar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta eliminada</returns>
        public void glConsultaFiltro_Delete(DTO_glConsultaFiltro filtro)
        {
            var parameters = new List<SqlParameter>();
            string WhereBuilder = this.glConsultaFiltro_WhereBuilder(filtro, false, parameters);

            this._dal_glConsultaFiltro = (DAL_glConsultaFiltro)base.GetInstance(typeof(DAL_glConsultaFiltro), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_glConsultaFiltro.DAL_glConsultaFiltro_Delete(filtro, WhereBuilder, parameters);

        }

        /// <summary>
        /// Eliminar una  consulta
        /// </summary>
        /// <param name="queryID">identificador de la consulta</param>
        /// <returns>consulta eliminada</returns>
        public void glConsultaFiltro_DeleteByQueryID(int queryID)
        {
            this._dal_glConsultaFiltro = (DAL_glConsultaFiltro)base.GetInstance(typeof(DAL_glConsultaFiltro), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_glConsultaFiltro.DAL_glConsultaFiltro_DeleteByQueryID(queryID);
        }

        /// <summary>
        /// Metodo que arma el where de la consulta de las consultas
        /// </summary>
        /// <param name="filtro">Filtros de la consulta</param>
        /// <param name="onlyId">Bandera que indica si es solo el ID</param>
        /// <param name="parameters">parametros de la consulta</param>
        /// <returns>String correspondiente al where</returns>
        private string glConsultaFiltro_WhereBuilder(DTO_glConsultaFiltro filtro, bool onlyId, List<SqlParameter> parameters)
        {
            this._dal_glConsultaFiltro = (DAL_glConsultaFiltro)base.GetInstance(typeof(DAL_glConsultaFiltro), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glConsultaFiltro.DAL_glConsultaFiltro_WhereBuilder(filtro, onlyId, parameters);
        }

        /// <summary>
        /// Metodo que arma el where de la consulta de las consultas
        /// </summary>
        /// <param name="filtro">Filtros de la consulta</param>
        /// <param name="parameters">parametros de la consulta</param>
        /// <returns>String correspondiente al where</returns>
        private string glConsultaFiltro_UpdateSetBuilder(DTO_glConsultaFiltro filtro, List<SqlParameter> parameters)
        {
            this._dal_glConsultaFiltro = (DAL_glConsultaFiltro)base.GetInstance(typeof(DAL_glConsultaFiltro), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glConsultaFiltro.DAL_glConsultaFiltro_UpdateSetBuilder(filtro, parameters);
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
            var parameters = new List<SqlParameter>();
            string WhereBuilder = this.glConsultaSeleccion_WhereBuilder(filtro, false, parameters);

            this._dal_glConsultaSeleccion = (DAL_glConsultaSeleccion)base.GetInstance(typeof(DAL_glConsultaSeleccion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glConsultaSeleccion.DAL_glConsultaSeleccion_GetAll(filtro, WhereBuilder, parameters);
        }

        /// <summary>
        /// Obtiene una consulta especifica
        /// </summary>
        /// <param name="filtro">Informacion para fitrar la consulta</param>
        /// <returns>consulta</returns>
        public DTO_glConsultaSeleccion glConsultaSeleccion_Get(DTO_glConsultaSeleccion filtro)
        {
            var parameters = new List<SqlParameter>();
            string WhereBuilder = this.glConsultaSeleccion_WhereBuilder(filtro, true, parameters);

            this._dal_glConsultaSeleccion = (DAL_glConsultaSeleccion)base.GetInstance(typeof(DAL_glConsultaSeleccion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glConsultaSeleccion.DAL_glConsultaSeleccion_Get(filtro, WhereBuilder, parameters);
        }

        /// <summary>
        /// Adicionar una nueva consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta adicionada</returns>
        public DTO_glConsultaSeleccion glConsultaSeleccion_Add(DTO_glConsultaSeleccion filtro)
        {
            this._dal_glConsultaSeleccion = (DAL_glConsultaSeleccion)base.GetInstance(typeof(DAL_glConsultaSeleccion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_glConsultaSeleccion result = this._dal_glConsultaSeleccion.DAL_glConsultaSeleccion_Add(filtro);
            return this.glConsultaSeleccion_Get(result);
        }

        /// <summary>
        /// Actualizar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a actualizar</param>
        /// <returns>consulta actualizada</returns>
        public DTO_glConsultaSeleccion glConsultaSeleccion_Update(DTO_glConsultaSeleccion filtro)
        {
            var parameters = new List<SqlParameter>();
            string UpdateSetBuilder = this.glConsultaSeleccion_UpdateSetBuilder(filtro, parameters);

            this._dal_glConsultaSeleccion = (DAL_glConsultaSeleccion)base.GetInstance(typeof(DAL_glConsultaSeleccion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_glConsultaSeleccion result = this._dal_glConsultaSeleccion.DAL_glConsultaSeleccion_Update(filtro, UpdateSetBuilder, parameters);
            return glConsultaSeleccion_Get(result);
        }

        /// <summary>
        /// Eliminar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta eliminada</returns>
        public void glConsultaSeleccion_Delete(DTO_glConsultaSeleccion filtro)
        {
            var parameters = new List<SqlParameter>();
            string WhereBuilder = this.glConsultaSeleccion_WhereBuilder(filtro, false, parameters);

            this._dal_glConsultaSeleccion = (DAL_glConsultaSeleccion)base.GetInstance(typeof(DAL_glConsultaSeleccion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_glConsultaSeleccion.DAL_glConsultaSeleccion_Delete(filtro, WhereBuilder, parameters);

        }

        /// <summary>
        /// Eliminar una  consulta
        /// </summary>
        /// <param name="queryID">identificador de la consulta</param>
        /// <returns>consulta eliminada</returns>
        public void glConsultaSeleccion_DeleteByQueryID(int queryID)
        {
            this._dal_glConsultaSeleccion = (DAL_glConsultaSeleccion)base.GetInstance(typeof(DAL_glConsultaSeleccion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_glConsultaSeleccion.DAL_glConsultaSeleccion_DeleteByQueryID(queryID);
        }

        /// <summary>
        /// Metodo que arma el where de la consulta de las consultas
        /// </summary>
        /// <param name="filtro">Filtros de la consulta</param>
        /// <param name="onlyId">Bandera que indica si es solo el ID</param>
        /// <param name="parameters">parametros de la consulta</param>
        /// <returns>String correspondiente al where</returns>
        private string glConsultaSeleccion_WhereBuilder(DTO_glConsultaSeleccion filtro, bool onlyId, List<SqlParameter> parameters)
        {
            this._dal_glConsultaSeleccion = (DAL_glConsultaSeleccion)base.GetInstance(typeof(DAL_glConsultaSeleccion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glConsultaSeleccion.DAL_glConsultaSeleccion_WhereBuilder(filtro, onlyId, parameters);
        }

        /// <summary>
        /// Metodo que arma el where de la consulta de las consultas
        /// </summary>
        /// <param name="filtro">Filtros de la consulta</param>
        /// <param name="parameters">parametros de la consulta</param>
        /// <returns>String correspondiente al where</returns>
        private string glConsultaSeleccion_UpdateSetBuilder(DTO_glConsultaSeleccion filtro, List<SqlParameter> parameters)
        {
            this._dal_glConsultaSeleccion = (DAL_glConsultaSeleccion)base.GetInstance(typeof(DAL_glConsultaSeleccion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glConsultaSeleccion.DAL_glConsultaSeleccion_UpdateSetBuilder(filtro, parameters);
        }

        #endregion

        #region Consultas Generales

        /// <summary>
        /// Consultas generales
        /// </summary>
        /// <param name="vista">Nombre de la vista</param>
        /// <param name="dtoType">Tipo de DTO</param>
        /// <param name="consulta">Consulta con filtros</param>
        /// <returns></returns>
        public DataTable ConsultasGenerales(string vista, Type dtoType, DTO_glConsulta consulta)
        {
            try
            {
                DataTable result;
                this._dal_ConsultasGenerales = (DAL_ConsultasGenerales)this.GetInstance(typeof(DAL_ConsultasGenerales), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                result = this._dal_ConsultasGenerales.DAL_ConsultasGenerales_Consultar(vista, dtoType, consulta);

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Reports_cc_CreditoXLS");
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
            Dictionary<string, string> options = null;
            this._moduloAplicacion = (ModuloAplicacion)base.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            DTO_aplMaestraCampo campo = this._moduloAplicacion.aplMaestraCampo_GetColumn(docId, columnName);
            if (!string.IsNullOrEmpty(campo.RegExpression))
            {
                options = new Dictionary<string, string>();
                DTO_seUsuario usuario = this.seUsuario_GetUserByReplicaID(this.UserId);
                List<string> ids = campo.RegExpression.Split('|').ToList();
                foreach (string id in ids)
                {
                    options.Add(
                                    id,
                                    this._moduloAplicacion.aplIdiomaTraduccion_GetById(usuario.IdiomaID.Value, llave + (ids.IndexOf(id) + 1).ToString()).Dato.Value
                                );

                }

            }
            return options;
        }

        #endregion

        #region Reportes

        /// <summary>
        /// Funcion que se encaga de traer los datos de los Documentos Pendietes
        /// </summary>
        /// <param name="Periodo">Periodo a consultar los documentos Pendientes</param>
        /// <param name="modulo">Filtrar un modulo especifico</param>
        /// <returns>Listado DTO</returns>
        public List<DTO_GlobalTotal> ReportesGlobal_DocumentosPendiente(DateTime Periodo,byte tipoReport, string modulo, string documentoID)
        {
            List<DTO_GlobalTotal> docPendientes = new List<DTO_GlobalTotal>();
            try
            {
                List<DTO_GlobalTotal> result = new List<DTO_GlobalTotal>();
                DTO_GlobalTotal docPen = new DTO_GlobalTotal();
                docPen.DetallesDocPendientes = new List<DTO_ReportDocumentoPendientes>();
                this._dal_reportesGlobal = (DAL_ReportesGlobal)this.GetInstance(typeof(DAL_ReportesGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                docPen.DetallesDocPendientes = this._dal_reportesGlobal.DAL_ReportesGlobal_DocumentosPendiente(Periodo,tipoReport, modulo,documentoID);
                List<string> distinct = (from c in docPen.DetallesDocPendientes select c.ModuloID.Value).Distinct().ToList();

                foreach (var module in distinct)
                {
                    DTO_GlobalTotal docPendient = new DTO_GlobalTotal();
                    docPendient.DetallesDocPendientes = new List<DTO_ReportDocumentoPendientes>();

                    docPendient.DetallesDocPendientes = docPen.DetallesDocPendientes.Where(x => x.ModuloID.Value == module).ToList();
                    result.Add(docPendient);
                }

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesGlobal_DocumentosPendiente");
                return docPendientes;
            }
        }

        /// <summary>
        /// Genera un reporte
        /// </summary>
        /// <param name="documentID">documento con el cual se salva el archivo</param>
        /// <param name="numeroDoc">Numero del documento con el cual se salva el archivo>
        public void GenerarReportOld(int documentID, int numeroDoc)
        {
          
        }

        #endregion

    }
}
