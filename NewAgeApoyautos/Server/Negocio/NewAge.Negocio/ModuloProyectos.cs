using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.ADO;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.Negocio.Documentos.Activos;
using NewAge.ADO.Documentos.Activos_Fijos;
using SentenceTransformer;
using System.Data;

namespace NewAge.Negocio
{
    public class ModuloProyectos : ModuloBase
    {
        #region Variables

        #region Dals

        private DAL_pyProyectoDeta _dal_pyProyectoDeta = null;
        private DAL_pyProyectoDocu _dal_pyProyectoDocu = null;
        private DAL_ReportesProyectos _dal_pyReportesProyecto = null;
        private DAL_pyPreProyectoDeta _dal_pyPreProyectoDeta = null;
        private DAL_pyPreProyectoDocu _dal_pyPreProyectoDocu = null;
        private DAL_MasterSimple _dal_MasterSimple = null;
        private DAL_MasterComplex _dal_MasterComplex = null;
        private DAL_MasterHierarchy _dal_MasterHierarchy= null;
        private DAL_pyPreProyectoTarea _dal_pyPreProyectoTarea = null;
        private DAL_pyProyectoTarea _dal_pyProyectoTarea = null;
        private DAL_pyProyectoMvto _dal_pyProyectoMvto = null;
        private DAL_prDetalleDocu _dal_prDetalleDocu = null;
        private DAL_prSaldosDocu _dal_prSaldosDocu = null;
        private DAL_pyActaTrabajoDeta _dal_pyActaTrabajoDet = null;
        private DAL_pyProyectoTareaCliente _dal_pyProyTareaCliente = null;
        private DAL_pyProyectoPlanEntrega _dal_pyProyPlanEntrega = null;
        private DAL_pyActaEntregaDeta _dal_pyActaEntregaDeta = null;
        private DAL_pyProyectoDetaCLI _dal_pyProyectoDetaCLI = null;
        private DAL_pyPreProyectoDetaCLI _dal_pyPreProyectoDetaCLI = null;
        private DAL_pyProyectoDetaHistoria _dal_pyProyectoDetaHist = null;
        private DAL_pyProyectoTareaHistoria _dal_pyProyectoTareaHist = null;
        #endregion
        #region Modulos
        private ModuloAplicacion _moduloAplicacion = null;
        private ModuloGlobal _moduloGlobal = null;
        private ModuloProveedores _moduloProveedores = null;
        private ModuloFacturacion _moduloFacturacion = null;
        private ModuloContabilidad _moduloContabilidad = null;

        #endregion

        #endregion

        /// <summary>
        /// Constructor Modulo ModuloProyectos
        /// </summary>
        /// <param name="conn"></param>
        public ModuloProyectos(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }
        
        #region Proyectos

        #region Solicitud de Proyecto(PreProyecto)

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
        public DTO_TxResult SolicitudProyecto_Add( int documentoID,  ref int numeroDoc, string claseServicioID, string areaFuncional, string prefijo,
                                                  string proyectoID, string centroCto, string observaciones,DTO_SolicitudTrabajo transaccion)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            DTO_glDocumentoControl _docControl = null;

            base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyPreProyectoDeta = (DAL_pyPreProyectoDeta)this.GetInstance(typeof(DAL_pyPreProyectoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyPreProyectoDocu = (DAL_pyPreProyectoDocu)this.GetInstance(typeof(DAL_pyPreProyectoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyPreProyectoTarea = (DAL_pyPreProyectoTarea)this.GetInstance(typeof(DAL_pyPreProyectoTarea), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                #region Variables iniciales
                _docControl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                DateTime periodo = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_Periodo));

                string lugarGeoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string centroCosXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string prefijoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string proyectoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string terceroXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                string trabajoXDef = this.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TrabajoDefecto);

                if (string.IsNullOrEmpty(trabajoXDef))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.py).ToString() + AppControl.py_TrabajoDefecto + "&&" + string.Empty;
                    return result;
                }
              

                DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                decimal tc = this._moduloGlobal.TasaDeCambio_Get(mdaExt, today);
                int numDoc = 0; 
                #endregion

                if (_docControl == null)
                {                   
                    #region Guarda el Doc Control
                    _docControl = new DTO_glDocumentoControl();
                    _docControl.EmpresaID.Value = this.Empresa.ID.Value;
                    _docControl.DocumentoID.Value = documentoID;
                    _docControl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    _docControl.Fecha.Value = DateTime.Now;
                    _docControl.PeriodoDoc.Value = periodo;
                    _docControl.PeriodoUltMov.Value = periodo;
                    _docControl.FechaDoc.Value = transaccion.FechaDoc.Value;
                    _docControl.AreaFuncionalID.Value = areaFuncional;
                    _docControl.Observacion.Value = observaciones;
                    _docControl.PrefijoID.Value = !string.IsNullOrEmpty(prefijo) ? prefijo : prefijoXDef;
                    _docControl.MonedaID.Value = mdaLoc;
                    _docControl.TasaCambioCONT.Value = tc;
                    _docControl.TasaCambioDOCU.Value = tc;
                    _docControl.Estado.Value = (byte)EstadoDocControl.SinAprobar;                   
                    _docControl.ProyectoID.Value = !string.IsNullOrEmpty(proyectoID) ? proyectoID : proyectoXDef;
                    _docControl.CentroCostoID.Value = centroCto;
                    _docControl.LugarGeograficoID.Value = lugarGeoXDef;
                    _docControl.seUsuarioID.Value = this.UserId;
                    _docControl.NumeroDoc.Value = 0;
                    _docControl.DocumentoNro.Value = 0;
                    _docControl.ComprobanteIDNro.Value = 0;
                    _docControl.Descripcion.Value = "Creacion Preproyecto";
                    _docControl.Valor.Value = transaccion.Header.Valor.Value;
                    _docControl.Iva.Value = transaccion.Header.ValorIVA.Value;
                    _docControl.TerceroID.Value = terceroXDef;
                    if (!string.IsNullOrEmpty(transaccion.Header.ClienteID.Value))
                    {
                        DTO_faCliente cliente = (DTO_faCliente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, transaccion.Header.ClienteID.Value, true, false);
                        _docControl.TerceroID.Value = cliente != null ? cliente.TerceroID.Value : terceroXDef;
                    }
                    DTO_TxResultDetail resultGLDC = new DTO_TxResultDetail();
                    resultGLDC = this._moduloGlobal.glDocumentoControl_Add(AppDocuments.PreProyecto, _docControl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_AddDocument;
                        return result;
                    }

                    numDoc = Convert.ToInt32(resultGLDC.Key);
                    _docControl.NumeroDoc.Value = numDoc;
                    #endregion

                    #region Guarda en pyPreProyectoDocu
                   

                    transaccion.Header.NumeroDoc.Value = numDoc;
                    transaccion.Header.ClaseServicioID.Value = claseServicioID;
                    this._dal_pyPreProyectoDocu.DAL_pyPreProyectoDocu_Add(transaccion.Header);
                    #endregion

                    #region Guarda en pyPreProyectoTarea - pyPreProyectoDeta

                    foreach (DTO_pyPreProyectoTarea tarea in transaccion.Detalle)
                    {                        
                        tarea.NumeroDoc.Value = numDoc;
                        tarea.CentroCostoID.Value = centroCto;
                        tarea.Descriptivo.Value = tarea.Descriptivo.Value.Length > UDT_DescripTExt.MaxLength ? tarea.Descriptivo.Value.Substring(0, UDT_DescripTExt.MaxLength) : tarea.Descriptivo.Value;
                        tarea.TrabajoID.Value =trabajoXDef;
                        tarea.TareaCliente.Value = string.IsNullOrEmpty(tarea.TareaCliente.Value) ? tarea.TareaID.Value : tarea.TareaCliente.Value;
                        int consTarea = this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_Add(tarea);

                        #region Guarda en pyPreProyectoDeta
                        foreach (DTO_pyPreProyectoDeta det in tarea.Detalle)
                        {
                            det.ConsecTarea.Value = consTarea;
                            det.NumeroDoc.Value = numDoc;
                            det.TrabajoID.Value = trabajoXDef;
                            this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Add(det);
                        }
                        #endregion
                    }
                    #endregion                    
                    #region Guarda en pyPreProyectoTarea - pyPreProyectoDeta(Tareas Adicionales)

                    foreach (DTO_pyPreProyectoTarea tareaAdic in transaccion.DetalleTareasAdic)
                    {
                        tareaAdic.NumeroDoc.Value = numDoc;
                        tareaAdic.CentroCostoID.Value = centroCto;
                        tareaAdic.Descriptivo.Value = tareaAdic.Descriptivo.Value.Length > UDT_DescripTExt.MaxLength ? tareaAdic.Descriptivo.Value.Substring(0, UDT_DescripTExt.MaxLength) : tareaAdic.Descriptivo.Value;
                        tareaAdic.TrabajoID.Value =  trabajoXDef ;
                        tareaAdic.TareaCliente.Value = string.IsNullOrEmpty(tareaAdic.TareaCliente.Value) ? tareaAdic.TareaID.Value : tareaAdic.TareaCliente.Value;
                        int consTarea = this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_Add(tareaAdic);

                        #region Guarda en pyPreProyectoDeta
                        foreach (DTO_pyPreProyectoDeta det in tareaAdic.Detalle)
                        {
                            det.ConsecTarea.Value = consTarea;
                            det.NumeroDoc.Value = numDoc;
                            det.TrabajoID.Value = trabajoXDef;
                            this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Add(det);
                        }
                        #endregion
                    }
                    #endregion                    
                }
                else
                {
                    //PreProyecto
                    if (_docControl.DocumentoID.Value == AppDocuments.PreProyecto)
                    {
                        _docControl.Valor.Value = transaccion.Detalle.Sum(x=>x.CostoTotalML.Value);
                        _docControl.Iva.Value = transaccion.Header.ValorIVA.Value;
                        _docControl.ProyectoID.Value = string.IsNullOrEmpty(proyectoID) ? transaccion.DocCtrl.ProyectoID.Value : proyectoID;
                        _docControl.CentroCostoID.Value = string.IsNullOrEmpty(centroCto) ? transaccion.DocCtrl.CentroCostoID.Value : centroCto;
                        _docControl.FechaDoc.Value = transaccion.FechaDoc.Value;
                        if (!string.IsNullOrEmpty(transaccion.Header.ClienteID.Value))
                        {
                            DTO_faCliente cliente = (DTO_faCliente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, transaccion.Header.ClienteID.Value, true, false);
                            _docControl.TerceroID.Value = cliente != null ? cliente.TerceroID.Value : terceroXDef;
                        }
                        this._moduloGlobal.glDocumentoControl_Update(_docControl, true, true);
                        #region Actualiza en pyPreProyectoDocu
                        transaccion.Header.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                        transaccion.Header.ClaseServicioID.Value = claseServicioID;
                        this._dal_pyPreProyectoDocu.DAL_pyPreProyectoDocu_Upd(transaccion.Header);
                        #endregion

                        #region Actualiza Tareas y Recursos

                        #region Elimina las tareas solicitadas
                        foreach (int t in transaccion.Header.TareasDeleted)
                        {
                            this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Delete(t,null);
                            this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_DeleteByConsecutivo(t);
                        } 
                        #endregion
                        #region Elimina los recursos solicitados
                        foreach (int r in transaccion.Header.RecursosDeleted)
                            this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_DeleteByConsecutivo(r);
                        #endregion
                        foreach (DTO_pyPreProyectoTarea tarea in transaccion.Detalle)
                        {
                            //Valida si la tarea existe
                            bool exist = this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_Exist(tarea.Consecutivo.Value);                            
                            if(exist)
                            {
                                #region Actualiza (pyPreProyectoTarea-pyPreProyectoDeta)  
                                tarea.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                tarea.CentroCostoID.Value = centroCto;
                                tarea.TrabajoID.Value = trabajoXDef;
                                this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_Upd(tarea);
                                foreach (DTO_pyPreProyectoDeta det in tarea.Detalle)
                                {
                                    //Valida si el detalle existe
                                    bool existDeta = this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Exist(det.Consecutivo.Value);
                                    if (existDeta)
                                    {
                                        det.TrabajoID.Value = trabajoXDef;
                                        this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Upd(det);
                                    }
                                    else
                                    {
                                        det.ConsecTarea.Value = tarea.Consecutivo.Value;
                                        det.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                        det.TrabajoID.Value = trabajoXDef;
                                        this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Add(det);
                                    }
                                } 
                                #endregion
                            }
                            else
                            {
                                #region Agrega nuevo (pyPreProyectoTarea-pyPreProyectoDeta)
                                tarea.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                tarea.CentroCostoID.Value = centroCto;
                                tarea.TareaCliente.Value = string.IsNullOrEmpty(tarea.TareaCliente.Value) ? tarea.TareaID.Value : tarea.TareaCliente.Value;
                                this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_Add(tarea);
                                foreach (DTO_pyPreProyectoDeta det in tarea.Detalle)
                                {
                                    //Agrega nuevo detalle
                                    det.ConsecTarea.Value = tarea.Consecutivo.Value;
                                    det.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                    det.TrabajoID.Value = string.IsNullOrEmpty(det.TrabajoID.Value) ? trabajoXDef : det.TrabajoID.Value;
                                    this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Add(det);
                                } 
                                #endregion
                            }                           
                        }
                        #endregion
                        #region Actualiza Tareas Adicionales

                        foreach (DTO_pyPreProyectoTarea tareaAdic in transaccion.DetalleTareasAdic)
                        {
                            //Valida si la tarea existe
                            bool exist = this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_Exist(tareaAdic.Consecutivo.Value);
                            if (exist)
                            {
                                #region Actualiza (pyPreProyectoTarea-pyPreProyectoDeta)
                                this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_Upd(tareaAdic);
                                foreach (DTO_pyPreProyectoDeta det in tareaAdic.Detalle)
                                {
                                    //Valida si el detalle existe
                                    bool existDeta = this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Exist(det.Consecutivo.Value);
                                    if (existDeta)
                                    {
                                        det.TrabajoID.Value = trabajoXDef;
                                        this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Upd(det);
                                    }
                                    else
                                    {
                                        det.ConsecTarea.Value = tareaAdic.Consecutivo.Value;
                                        det.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                        det.TrabajoID.Value = trabajoXDef;
                                        this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Add(det);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region Agrega nuevo (pyPreProyectoTarea-pyPreProyectoDeta)
                                tareaAdic.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                tareaAdic.CentroCostoID.Value = centroCto;
                                tareaAdic.TareaCliente.Value = string.IsNullOrEmpty(tareaAdic.TareaCliente.Value) ? tareaAdic.TareaID.Value : tareaAdic.TareaCliente.Value;
                                this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_Add(tareaAdic);
                                foreach (DTO_pyPreProyectoDeta det in tareaAdic.Detalle)
                                {
                                    //Agrega nuevo detalle
                                    det.ConsecTarea.Value = tareaAdic.Consecutivo.Value;
                                    det.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                    det.TrabajoID.Value = string.IsNullOrEmpty(det.TrabajoID.Value) ? trabajoXDef : det.TrabajoID.Value;
                                    this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Add(det);
                                }
                                #endregion
                            }
                        }
                        #endregion
                        
                    }
                    //Proyecto
                    else if (_docControl.DocumentoID.Value == AppDocuments.Proyecto)
                    {
                        this._dal_pyProyectoDocu = (DAL_pyProyectoDocu)this.GetInstance(typeof(DAL_pyProyectoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        this._dal_pyProyectoDeta = (DAL_pyProyectoDeta)this.GetInstance(typeof(DAL_pyProyectoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        this._dal_pyProyectoTarea = (DAL_pyProyectoTarea)this.GetInstance(typeof(DAL_pyProyectoTarea), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                        #region Actualiza el Doc Control
                        _docControl.Valor.Value = transaccion.Detalle.Sum(x => x.CostoTotalML.Value);
                        _docControl.Iva.Value = transaccion.Header.ValorIVA.Value;
                        _docControl.ProyectoID.Value = transaccion.DocCtrl.ProyectoID.Value;
                        _docControl.CentroCostoID.Value = transaccion.DocCtrl.CentroCostoID.Value;
                        if (!string.IsNullOrEmpty(transaccion.HeaderProyecto.ClienteID.Value))
                        {
                            DTO_faCliente cliente = (DTO_faCliente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, transaccion.HeaderProyecto.ClienteID.Value, true, false);
                            _docControl.TerceroID.Value = cliente != null ? cliente.TerceroID.Value : terceroXDef;
                        }
                        this._moduloGlobal.glDocumentoControl_Update(_docControl, true, true);
                        #endregion
                        #region Actualiza en pyProyectoDocu
                        transaccion.Header.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                        this._dal_pyProyectoDocu.DAL_pyProyectoDocu_Upd(transaccion.HeaderProyecto);
                        #endregion
                        #region Actualiza Tareas y Recursos
                        #region Elimina las tareas solicitadas
                        foreach (int t in transaccion.HeaderProyecto.TareasDeleted)
                        {
                            this._dal_pyProyectoDeta.DAL_pyProyectoDeta_Delete(t, null);
                            this._dal_pyProyectoTarea.DAL_pyProyectoTarea_DeleteByConsecutivo(t);
                        }
                        #endregion
                        #region Elimina los recursos solicitados
                        foreach (int r in transaccion.HeaderProyecto.RecursosDeleted)
                            this._dal_pyProyectoDeta.DAL_pyProyectoDeta_DeleteByConsecutivo(r);
                        #endregion
                        foreach (DTO_pyProyectoTarea tarea in transaccion.DetalleProyecto)
                        {
                            //Valida si la tarea existe
                            bool exist = this._dal_pyProyectoTarea.DAL_pyProyectoTarea_Exist(tarea.Consecutivo.Value);                            
                            if(exist)
                            {
                                #region Actualiza (pyProyectoTarea-pyProyectoDeta)
                                this._dal_pyProyectoTarea.DAL_pyProyectoTarea_Upd(tarea);
                                foreach (DTO_pyProyectoDeta det in tarea.Detalle)
                                {
                                    //Valida si el detalle existe
                                    bool existDeta = this._dal_pyProyectoDeta.DAL_pyProyectoDeta_Exist(det.Consecutivo.Value);
                                    if (existDeta)
                                    {
                                        det.TrabajoID.Value = trabajoXDef;
                                        this._dal_pyProyectoDeta.DAL_pyProyectoDeta_Upd(det);
                                    }
                                    else
                                    {
                                        det.ConsecTarea.Value = tarea.Consecutivo.Value;
                                        det.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                        det.TrabajoID.Value = trabajoXDef;
                                        this.pyProyectoDeta_Add(det);
                                    }
                                }
                                    
                                #endregion
                            }
                            else
                            {
                                #region Agrega nuevo (pyProyectoTarea-pyProyectoDeta)
                                tarea.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                tarea.CentroCostoID.Value = centroCto;
                                tarea.TareaCliente.Value = string.IsNullOrEmpty(tarea.TareaCliente.Value) ? tarea.TareaID.Value : tarea.TareaCliente.Value;
                                this._dal_pyProyectoTarea.DAL_pyProyectoTarea_Add(tarea);
                                foreach (DTO_pyProyectoDeta det in tarea.Detalle)
                                {
                                    //Agrega nuevo detalle
                                    det.ConsecTarea.Value = tarea.Consecutivo.Value;
                                    det.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                    det.TrabajoID.Value = string.IsNullOrEmpty(det.TrabajoID.Value) ? trabajoXDef : det.TrabajoID.Value;
                                    this.pyProyectoDeta_Add(det);
                                }
                                #endregion
                            }                            
                        }
                        #endregion
                        #region Actualiza Tareas Adicionales

                        foreach (DTO_pyProyectoTarea tareaAdic in transaccion.DetalleProyectoTareaAdic)
                        {
                            //Valida si la tarea existe
                            bool exist = this._dal_pyProyectoTarea.DAL_pyProyectoTarea_Exist(tareaAdic.Consecutivo.Value);
                            if (exist)
                            {
                                #region Actualiza (pyProyectoTarea-pyProyectoDeta)
                                this._dal_pyProyectoTarea.DAL_pyProyectoTarea_Upd(tareaAdic);
                                foreach (DTO_pyProyectoDeta det in tareaAdic.Detalle)
                                {
                                    //Valida si el detalle existe
                                    bool existDeta = this._dal_pyProyectoDeta.DAL_pyProyectoDeta_Exist(det.Consecutivo.Value);
                                    if (existDeta)
                                    {
                                        det.TrabajoID.Value = trabajoXDef;
                                        this._dal_pyProyectoDeta.DAL_pyProyectoDeta_Upd(det);
                                    }
                                    else
                                    {
                                        det.ConsecTarea.Value = tareaAdic.Consecutivo.Value;
                                        det.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                        det.TrabajoID.Value =  trabajoXDef;
                                        this.pyProyectoDeta_Add(det);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region Agrega nuevo (pyProyectoTarea-pyProyectoDeta)
                                tareaAdic.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                tareaAdic.CentroCostoID.Value = centroCto;
                                tareaAdic.TareaCliente.Value = string.IsNullOrEmpty(tareaAdic.TareaCliente.Value) ? tareaAdic.TareaID.Value : tareaAdic.TareaCliente.Value;
                                this._dal_pyProyectoTarea.DAL_pyProyectoTarea_Add(tareaAdic);
                                foreach (DTO_pyProyectoDeta det in tareaAdic.Detalle)
                                {
                                    //Agrega nuevo detalle
                                    det.ConsecTarea.Value = tareaAdic.Consecutivo.Value;
                                    det.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                    det.TrabajoID.Value = string.IsNullOrEmpty(det.TrabajoID.Value) ? trabajoXDef : det.TrabajoID.Value;
                                    this.pyProyectoDeta_Add(det);
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }                   
                }
                return result;
            }
            catch (Exception ex)
            {
                numeroDoc = 0;
                transaccion.Header.NumeroDoc.Value = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "SolicitudProyecto_Add");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    base._mySqlConnectionTx.Commit();
                    base._mySqlConnectionTx = null;
                    this._moduloGlobal._mySqlConnectionTx = null;

                    if (numeroDoc == 0)
                    {
                        _docControl.DocumentoNro.Value = Convert.ToInt32(this.GenerarDocumentoNro(documentoID, _docControl.PrefijoID.Value));
                        this._moduloGlobal.ActualizaConsecutivos(_docControl, true, false, false);
                    }
                    numeroDoc = _docControl.NumeroDoc.Value.Value;
                    result.ExtraField = _docControl.DocumentoNro.Value.ToString();
                }
                else if (base._mySqlConnectionTx != null)
                {
                   // numeroDoc = 0;
                    base._mySqlConnectionTx.Rollback();
                }
                    
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
        /// <param name="loadActasTrab">Indica si carga las actas de trabajo</param>
        /// <param name="loadTrazabilidad">Indica si carga la consulta de trazabilidad del proyecto</param>
        /// <returns>Objeto con todos los datos</returns>
        public DTO_SolicitudTrabajo SolicitudProyecto_Load(int documentoID, string prefijoID, int? docNro, int? numeroDoc, string claseServicioID,
                                                           string proyectoID, bool isPreProyecto, bool loadMvtos, bool loadActasTrab, bool loadAPUCliente, bool loadTrazabilidad = false)
        {
            try
            {
                #region Variables
                this._dal_pyPreProyectoDeta = (DAL_pyPreProyectoDeta)this.GetInstance(typeof(DAL_pyPreProyectoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyPreProyectoDocu = (DAL_pyPreProyectoDocu)this.GetInstance(typeof(DAL_pyPreProyectoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyPreProyectoDetaCLI = (DAL_pyPreProyectoDetaCLI)this.GetInstance(typeof(DAL_pyPreProyectoDetaCLI), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyectoDetaCLI = (DAL_pyProyectoDetaCLI)this.GetInstance(typeof(DAL_pyProyectoDetaCLI), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_SolicitudTrabajo proyecto = new DTO_SolicitudTrabajo();
                DTO_glDocumentoControl docCtrl = null; 
                #endregion

                if (isPreProyecto)
                {
                    #region Carga info del PreProyecto
                    //Trae glDocumentoControl
                    if (numeroDoc != null)
                        docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc.Value);
                    else if (!string.IsNullOrEmpty(prefijoID) && docNro != null)
                        docCtrl = this._moduloGlobal.glDocumentoControl_GetInternalDoc(AppDocuments.PreProyecto, prefijoID, docNro.Value);
                    else if (!string.IsNullOrEmpty(proyectoID))
                    {
                        DTO_glDocumentoControl filterDoc = new DTO_glDocumentoControl();
                        filterDoc.DocumentoID.Value = AppDocuments.PreProyecto;
                        filterDoc.ProyectoID.Value = proyectoID;
                        var res = this._moduloGlobal.glDocumentoControl_GetByParameter(filterDoc);
                        if (res.Count > 0)
                        {
                            docCtrl = res.First();
                            if (!string.IsNullOrEmpty(prefijoID) && !prefijoID.Equals(docCtrl.PrefijoID.Value))
                                docCtrl = null;
                        }
                           
                    }

                    //Si no existe devuelve null
                    if (docCtrl != null)
                        proyecto.DocCtrl = docCtrl;
                    else
                        return null;

                    //Obtiene los datos de la tabla pyPreProyectoDocu
                    DTO_pyPreProyectoDocu docu = this.pyPreProyectoDocu_Get(docCtrl.NumeroDoc.Value.Value);
                    //Si no existe devuelve null
                    if (docu != null)
                        proyecto.Header = docu;
                    else
                        return null;

                    //Obtiene las tareas del PreProyecto
                    var det = this.pyPreProyectoTarea_Get(docCtrl.NumeroDoc.Value, string.Empty, string.Empty);
                    proyecto.Detalle = det.FindAll(x => !x.CostoAdicionalInd.Value.Value).ToList();
                    proyecto.DetalleTareasAdic = det.FindAll(x => x.CostoAdicionalInd.Value.Value).ToList();

                    //Obtiene el APU del Cliente
                    if (loadAPUCliente)
                    {
                        foreach (DTO_pyPreProyectoTarea tarea in proyecto.Detalle)
                            tarea.DetalleAPUCliente = this._dal_pyPreProyectoDetaCLI.DAL_pyPreProyectoDetaCLI_GetByConsecTarea(tarea.Consecutivo.Value);
                    }
                    #endregion
                }
                else
                {
                    #region Carga Info del Proyecto
                    this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)this.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_pyActaTrabajoDet = (DAL_pyActaTrabajoDeta)this.GetInstance(typeof(DAL_pyActaTrabajoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    //Trae glDocumentoControl
                    if (numeroDoc != null)
                        docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc.Value);
                    else if (!string.IsNullOrEmpty(prefijoID) && docNro != null)
                        docCtrl = this._moduloGlobal.glDocumentoControl_GetInternalDoc(AppDocuments.Proyecto, prefijoID, docNro.Value);
                    else if (!string.IsNullOrEmpty(proyectoID))
                    {
                        DTO_glDocumentoControl filterDoc = new DTO_glDocumentoControl();
                        filterDoc.DocumentoID.Value = AppDocuments.Proyecto;
                        filterDoc.ProyectoID.Value = proyectoID;
                        var res = this._moduloGlobal.glDocumentoControl_GetByParameter(filterDoc);
                        if (res.Count > 0)
                            docCtrl = res.First();
                    }
                    //Si no existe devuelve null
                    if (docCtrl != null)
                        proyecto.DocCtrl = docCtrl;
                    else
                        return null;

                    //Obtiene los datos de la tabla pyPreProyectoDocu
                    DTO_pyProyectoDocu docu = this.pyProyectoDocu_Get(docCtrl.NumeroDoc.Value.Value);
                    //Si no existe devuelve null
                    if (docu != null)
                        proyecto.HeaderProyecto = docu;
                    else
                        return null;

                    //Obtiene tareas de pyProyectoTarea con su detalle
                    var det = this.pyProyectoTarea_Get(docCtrl.NumeroDoc.Value, string.Empty, docu.ClaseServicioID.Value);
                    proyecto.DetalleProyecto = det.FindAll(x => !x.CostoAdicionalInd.Value.Value).ToList();
                    proyecto.DetalleProyectoTareaAdic = det.FindAll(x => x.CostoAdicionalInd.Value.Value).ToList();

                    //Obtiene el APU del Cliente
                    if (loadAPUCliente)
                    {
                        foreach (DTO_pyProyectoTarea tarea in proyecto.DetalleProyecto)
                            tarea.DetalleAPUCliente = this._dal_pyProyectoDetaCLI.DAL_pyProyectoDeta_GetByConsecTarea(tarea.Consecutivo.Value);
                    }

                    if (loadMvtos)
                    {
                        //Carga los movimientos que existen del Presupuesto
                        DTO_pyProyectoMvto filter = new DTO_pyProyectoMvto();
                        filter.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                        proyecto.Movimientos = this.pyProyectoMvto_GetParameter(filter);
                    }
                    if (loadActasTrab)
                    {
                        //Carga las actas de Trabajo realizadas
                        DTO_pyActaTrabajoDeta filterActa = new DTO_pyActaTrabajoDeta();
                        filterActa.NumDocProyecto.Value = docCtrl.NumeroDoc.Value;
                        proyecto.ActaTrabajosDeta = this._dal_pyActaTrabajoDet.DAL_pyActaTrabajoDeta_GetByParameter(filterActa);
                    }
                    if (loadTrazabilidad)
                    {
                        //Carga el estado actual de Cantidades y Valores del Proyecto(Trazabilidad)
                        proyecto.ResumenTrazabilidad = this._dal_pyProyectoMvto.DAL_QueryTrazabilidadProy(docCtrl.NumeroDoc.Value.Value);
                        foreach (DTO_QueryTrazabilidad res in proyecto.ResumenTrazabilidad)
                        {
                            res.CantPresupuestado.Value = res.CantidadTarea.Value;
                            res.CantSolicitado.Value = res.Detalle.Sum(x => x.CantSolicitado.Value);
                            res.CantComprado.Value = res.Detalle.Sum(x => x.CantComprado.Value);
                            res.CantRecibido.Value = res.Detalle.Sum(x => x.CantRecibido.Value);
                            res.CantConsumido.Value = res.Detalle.Sum(x => x.CantConsumido.Value);
                            res.CantFacturado.Value = res.Detalle.Sum(x => x.CantFacturado.Value);
                            res.VlrPresupuestado.Value = res.Detalle.Sum(x => x.VlrPresupuestado.Value);
                            res.VlrSolicitado.Value = res.Detalle.Sum(x => x.VlrSolicitado.Value);
                            res.VlrComprado.Value = res.Detalle.Sum(x => x.VlrComprado.Value);
                            res.VlrRecibido.Value = res.Detalle.Sum(x => x.VlrRecibido.Value);
                            res.VlrConsumido.Value = res.Detalle.Sum(x => x.VlrConsumido.Value);
                            res.VlrFacturado.Value = res.Detalle.Sum(x => x.VlrFacturado.Value);
                        }
                    } 
                    #endregion
                }                

                return proyecto;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "SolicitudTrabajo_Load");
                throw exception;
            }
        }

        /// <summary>
        /// Enviar aprobación la solicitud de trabajo
        /// </summary>
        /// <param name="documentID">documentoID</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="batchProgress">indicador de progreso</param>
        /// <param name="insideAnotherTx">indica si esta en una transacción</param>
        /// <returns></returns>
        public DTO_SerializedObject SolicitudProyecto_AprobarProy(int documentID, DTO_SolicitudTrabajo transaccion, DateTime fechaInicio,Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.line = 1;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            int numDoc = 0;
            DTO_Alarma alarma = null;
            DTO_glDocumentoControl _docControl = null;
            try
            {
                decimal porcTotal = 0;
                decimal porcParte = 100 / 4;

                #region Carga variables iniciales

                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyectoDeta = (DAL_pyProyectoDeta)this.GetInstance(typeof(DAL_pyProyectoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyectoDocu = (DAL_pyProyectoDocu)this.GetInstance(typeof(DAL_pyProyectoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyectoTarea = (DAL_pyProyectoTarea)this.GetInstance(typeof(DAL_pyProyectoTarea), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterHierarchy = (DAL_MasterHierarchy)this.GetInstance(typeof(DAL_MasterHierarchy), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)this.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string centroCosXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string trabajoXDef = this.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TrabajoDefecto);
                string diasOC = this.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasRequeridosOrdCompra);
                string diasOCImport = this.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasRequeridosOrdCompraImportac);
                string codigoBSInventarios = this.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_CodigoBSCompraInv);

                int diasOrdCompraEmp = 0;
                int diasOrdCompraImport = 0;
                if (!string.IsNullOrEmpty(diasOC))
                    diasOrdCompraEmp = Convert.ToInt32(diasOC);            
                #endregion

                //Valida si es el PreProyecto para crear el Proyecto final
                if (transaccion.DocCtrl.DocumentoID.Value == AppDocuments.PreProyecto)
                {
                    EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), transaccion.DocCtrl.Estado.Value.Value.ToString());
                    if (estado == EstadoDocControl.Aprobado)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_DocumentEstateAprob;
                        return result;
                    }
                    #region Valida si el ProyectoID es valido para crear como nuevo
                    DTO_glDocumentoControl filter = new DTO_glDocumentoControl();
                    filter.DocumentoID.Value = AppDocuments.Proyecto;
                    filter.ProyectoID.Value = transaccion.DocCtrl.ProyectoID.Value;
                    var docsByProyecto = this._moduloGlobal.glDocumentoControl_GetByParameter(filter);
                    if (docsByProyecto.Count > 0)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Py_ProyectoAlreadyExist;
                        return result;
                    } 
                    #endregion

                    #region Actualiza Preproyecto
                    this._dal_pyPreProyectoDeta = (DAL_pyPreProyectoDeta)this.GetInstance(typeof(DAL_pyPreProyectoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_pyPreProyectoDocu = (DAL_pyPreProyectoDocu)this.GetInstance(typeof(DAL_pyPreProyectoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_pyPreProyectoTarea = (DAL_pyPreProyectoTarea)this.GetInstance(typeof(DAL_pyPreProyectoTarea), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    #region Actualiza Doc Control
                    transaccion.DocCtrl.Valor.Value = transaccion.Detalle.Sum(x => x.CostoTotalML.Value);
                    transaccion.DocCtrl.Iva.Value = transaccion.Header.ValorIVA.Value;
                    this._moduloGlobal.glDocumentoControl_Update(transaccion.DocCtrl, true, true); 
                    #endregion
                    #region Actualiza en pyPreProyectoDocu
                    transaccion.Header.NumeroDoc.Value = transaccion.DocCtrl.NumeroDoc.Value;
                    this._dal_pyPreProyectoDocu.DAL_pyPreProyectoDocu_Upd(transaccion.Header);
                    #endregion
                    #region Actualiza Tareas y Recursos
                    #region Elimina las tareas solicitadas
                    foreach (int t in transaccion.Header.TareasDeleted)
                    {
                        this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Delete(t, null);
                        this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_DeleteByConsecutivo(t);
                    }
                    #endregion
                    #region Elimina los recursos solicitados
                    foreach (int r in transaccion.Header.RecursosDeleted)
                        this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_DeleteByConsecutivo(r);
                    #endregion
                    foreach (DTO_pyPreProyectoTarea tarea in transaccion.Detalle)
                    {
                        //Valida si la tarea existe
                        bool exist = this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_Exist(tarea.Consecutivo.Value);
                        if (exist)
                        {
                            #region Actualiza (pyPreProyectoTarea-pyPreProyectoDeta)
                            tarea.NumeroDoc.Value = transaccion.DocCtrl.NumeroDoc.Value;
                            tarea.CentroCostoID.Value = centroCosXDef;
                            tarea.TrabajoID.Value = trabajoXDef;
                            this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_Upd(tarea);
                            foreach (DTO_pyPreProyectoDeta det in tarea.Detalle)
                            {
                                //Valida si el detalle existe
                                bool existDeta = this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Exist(det.Consecutivo.Value);
                                if (existDeta)
                                {
                                    det.TrabajoID.Value = trabajoXDef;
                                    this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Upd(det);
                                }
                                else
                                {
                                    det.ConsecTarea.Value = tarea.Consecutivo.Value;
                                    det.NumeroDoc.Value = transaccion.DocCtrl.NumeroDoc.Value;
                                    det.TrabajoID.Value = trabajoXDef;
                                    this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Add(det);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region Agrega nuevo (pyPreProyectoTarea-pyPreProyectoDeta)
                            tarea.NumeroDoc.Value = transaccion.DocCtrl.NumeroDoc.Value;
                            tarea.CentroCostoID.Value = centroCosXDef;
                            tarea.TareaCliente.Value = string.IsNullOrEmpty(tarea.TareaCliente.Value) ? tarea.TareaID.Value : tarea.TareaCliente.Value;
                            this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_Add(tarea);
                            foreach (DTO_pyPreProyectoDeta det in tarea.Detalle)
                            {
                                //Agrega nuevo detalle
                                det.ConsecTarea.Value = tarea.Consecutivo.Value;
                                det.NumeroDoc.Value = transaccion.DocCtrl.NumeroDoc.Value;
                                det.TrabajoID.Value = string.IsNullOrEmpty(det.TrabajoID.Value) ? trabajoXDef : det.TrabajoID.Value;
                                this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Add(det);
                            }
                            #endregion
                        }
                    }
                    #endregion
                    #region Actualiza Tareas Adicionales
                    foreach (DTO_pyPreProyectoTarea tareaAdic in transaccion.DetalleTareasAdic)
                    {
                        //Valida si la tarea existe
                        bool exist = this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_Exist(tareaAdic.Consecutivo.Value);
                        if (exist)
                        {
                            #region Actualiza (pyPreProyectoTarea-pyPreProyectoDeta)
                            this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_Upd(tareaAdic);
                            foreach (DTO_pyPreProyectoDeta det in tareaAdic.Detalle)
                            {
                                //Valida si el detalle existe
                                bool existDeta = this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Exist(det.Consecutivo.Value);
                                if (existDeta)
                                    this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Upd(det);
                                else
                                {
                                    det.ConsecTarea.Value = tareaAdic.Consecutivo.Value;
                                    det.NumeroDoc.Value = transaccion.DocCtrl.NumeroDoc.Value;
                                    det.TrabajoID.Value = string.IsNullOrEmpty(det.TrabajoID.Value) ? trabajoXDef : det.TrabajoID.Value;
                                    this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Add(det);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region Agrega nuevo (pyPreProyectoTarea-pyPreProyectoDeta)
                            tareaAdic.NumeroDoc.Value = transaccion.DocCtrl.NumeroDoc.Value;
                            tareaAdic.CentroCostoID.Value = centroCosXDef;
                            tareaAdic.TareaCliente.Value = string.IsNullOrEmpty(tareaAdic.TareaCliente.Value) ? tareaAdic.TareaID.Value : tareaAdic.TareaCliente.Value;
                            this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_Add(tareaAdic);
                            foreach (DTO_pyPreProyectoDeta det in tareaAdic.Detalle)
                            {
                                //Agrega nuevo detalle
                                det.ConsecTarea.Value = tareaAdic.Consecutivo.Value;
                                det.NumeroDoc.Value = transaccion.DocCtrl.NumeroDoc.Value;
                                det.TrabajoID.Value = string.IsNullOrEmpty(det.TrabajoID.Value) ? trabajoXDef : det.TrabajoID.Value;
                                this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Add(det);
                            }
                            #endregion
                        }
                    }
                    #endregion
                    #endregion

                    #region Guarda Proyecto Nuevo
                    #region Guarda el Doc Control
                    _docControl = new DTO_glDocumentoControl();
                    _docControl.EmpresaID.Value = this.Empresa.ID.Value;
                    _docControl.DocumentoID.Value = AppDocuments.Proyecto;
                    _docControl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    _docControl.Fecha.Value = transaccion.DocCtrl.FechaDoc.Value;
                    _docControl.PeriodoDoc.Value = transaccion.DocCtrl.PeriodoDoc.Value;
                    _docControl.PeriodoUltMov.Value = transaccion.DocCtrl.PeriodoDoc.Value;
                    _docControl.FechaDoc.Value = transaccion.DocCtrl.FechaDoc.Value;
                    _docControl.AreaFuncionalID.Value = transaccion.DocCtrl.AreaFuncionalID.Value;
                    _docControl.Observacion.Value = transaccion.DocCtrl.Observacion.Value;
                    _docControl.PrefijoID.Value = transaccion.DocCtrl.PrefijoID.Value;
                    _docControl.MonedaID.Value = transaccion.DocCtrl.MonedaID.Value;
                    _docControl.TasaCambioCONT.Value = transaccion.DocCtrl.TasaCambioCONT.Value;
                    _docControl.TasaCambioDOCU.Value = transaccion.DocCtrl.TasaCambioDOCU.Value;
                    _docControl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                   
                    _docControl.ProyectoID.Value = transaccion.DocCtrl.ProyectoID.Value;
                    _docControl.CentroCostoID.Value = transaccion.DocCtrl.CentroCostoID.Value;
                    _docControl.LugarGeograficoID.Value = transaccion.DocCtrl.LugarGeograficoID.Value;
                    _docControl.seUsuarioID.Value = transaccion.DocCtrl.seUsuarioID.Value;
                    _docControl.NumeroDoc.Value = 0;
                    _docControl.DocumentoNro.Value = 0;
                    _docControl.ComprobanteIDNro.Value = 0;
                    _docControl.Descripcion.Value = "Creacion Proyecto";
                    _docControl.Valor.Value = transaccion.DocCtrl.Valor.Value;
                    _docControl.Iva.Value = transaccion.DocCtrl.Iva.Value;
                    _docControl.TerceroID.Value = transaccion.DocCtrl.TerceroID.Value;
                    if (!string.IsNullOrEmpty(transaccion.Header.ClienteID.Value))
                    {
                        DTO_faCliente cliente = (DTO_faCliente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, transaccion.Header.ClienteID.Value, true, false);
                        _docControl.TerceroID.Value = cliente != null ? cliente.TerceroID.Value : transaccion.DocCtrl.TerceroID.Value;
                    }
                    DTO_TxResultDetail resultGLDC = new DTO_TxResultDetail();
                    resultGLDC = this._moduloGlobal.glDocumentoControl_Add(AppDocuments.Proyecto, _docControl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_AddDocument;
                        return result;
                    }

                    numDoc = Convert.ToInt32(resultGLDC.Key);
                    _docControl.NumeroDoc.Value = numDoc;

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region Guarda en pyProyectoDocu
                    DTO_pyProyectoDocu proyectoDocu = new DTO_pyProyectoDocu();
                    proyectoDocu.EmpresaID.Value = this.Empresa.ID.Value;
                    proyectoDocu.NumeroDoc.Value = numDoc;
                    proyectoDocu.ClaseServicioID.Value = transaccion.Header.ClaseServicioID.Value;
                    proyectoDocu.ClienteID.Value = transaccion.Header.ClienteID.Value;
                    proyectoDocu.ContratoID.Value = transaccion.Header.ContratoID.Value;
                    proyectoDocu.DescripcionSOL.Value = transaccion.Header.DescripcionSOL.Value;
                    proyectoDocu.DocSolicitud.Value = transaccion.Header.NumeroDoc.Value;
                    proyectoDocu.FechaInicio.Value = fechaInicio;
                    proyectoDocu.RecursosXTrabajoInd.Value = transaccion.Header.RecursosXTrabajoInd.Value;
                    proyectoDocu.ResponsableCLI.Value = transaccion.Header.ResponsableCLI.Value;
                    proyectoDocu.ResponsableCorreo.Value = transaccion.Header.ResponsableCorreo.Value;
                    proyectoDocu.ResponsableEMP.Value = transaccion.Header.ResponsableEMP.Value;
                    proyectoDocu.ResponsableTelefono.Value = transaccion.Header.ResponsableTelefono.Value;
                    proyectoDocu.EmpresaNombre.Value = transaccion.Header.EmpresaNombre.Value;
                    proyectoDocu.APUIncluyeAIUInd.Value = transaccion.Header.APUIncluyeAIUInd.Value;
                    proyectoDocu.TipoRedondeo.Value = transaccion.Header.TipoRedondeo.Value;
                    proyectoDocu.EquipoCantidadInd.Value = transaccion.Header.EquipoCantidadInd.Value;
                    proyectoDocu.PersonalCantidadInd.Value = transaccion.Header.PersonalCantidadInd.Value;
                    proyectoDocu.Jerarquia.Value = transaccion.Header.Jerarquia.Value;
                    proyectoDocu.Licitacion.Value = transaccion.Header.Licitacion.Value;
                    proyectoDocu.PorClienteADM.Value = transaccion.Header.PorClienteADM.Value;
                    proyectoDocu.PorClienteIMP.Value = transaccion.Header.PorClienteIMP.Value;
                    proyectoDocu.PorClienteUTI.Value = transaccion.Header.PorClienteUTI.Value;
                    proyectoDocu.PorEmpresaADM.Value = transaccion.Header.PorEmpresaADM.Value;
                    proyectoDocu.PorEmpresaIMP.Value = transaccion.Header.PorEmpresaIMP.Value;
                    proyectoDocu.PorEmpresaUTI.Value = transaccion.Header.PorEmpresaUTI.Value;
                    proyectoDocu.TipoSolicitud.Value = transaccion.Header.TipoSolicitud.Value;
                    proyectoDocu.PropositoProyecto.Value = transaccion.Header.PropositoProyecto.Value;
                    proyectoDocu.MonedaPresupuesto.Value = transaccion.Header.MonedaPresupuesto.Value;
                    proyectoDocu.TasaCambio.Value = transaccion.Header.TasaCambio.Value;
                    proyectoDocu.PorMultiplicadorPresup.Value = transaccion.Header.PorMultiplicadorPresup.Value;
                    proyectoDocu.MultiplicadorActivoInd.Value = transaccion.Header.MultiplicadorActivoInd.Value;
                    proyectoDocu.PorIVA.Value = transaccion.Header.PorIVA.Value;
                    proyectoDocu.VersionPrevia.Value = 1;
                    proyectoDocu.Version.Value = 1;
                    this._dal_pyProyectoDocu.DAL_pyProyectoDocu_Add(proyectoDocu);
                    #endregion
                    #region Guarda Tareas - Detalle - Movimiento
                    foreach (DTO_pyPreProyectoTarea solTarea in transaccion.Detalle)
                    {
                        #region Guarda Tareas (pyProyectoTarea)
                        DTO_pyProyectoTarea tarea = new DTO_pyProyectoTarea();
                        tarea.NumeroDoc.Value = numDoc;
                        tarea.TareaCliente.Value = solTarea.TareaCliente.Value;
                        tarea.TareaID.Value = solTarea.TareaID.Value;
                        tarea.CapituloTareaID.Value = solTarea.CapituloTareaID.Value;
                        tarea.CapituloDesc.Value = solTarea.CapituloDesc.Value;
                        tarea.CapituloGrupoID.Value = solTarea.CapituloGrupoID.Value;
                        tarea.Cantidad.Value = solTarea.Cantidad.Value;
                        tarea.CentroCostoID.Value = transaccion.DocCtrl.CentroCostoID.Value;
                        tarea.CostoAdicionalInd.Value = solTarea.CostoAdicionalInd.Value;
                        tarea.CostoDiferenciaML.Value = solTarea.CostoDiferenciaML.Value;
                        tarea.CostoExtraCLI.Value = solTarea.CostoExtraCLI.Value;
                        tarea.CostoLocalCLI.Value = solTarea.CostoLocalCLI.Value;
                        tarea.CostoLocalUnitCLI.Value = solTarea.CostoLocalUnitCLI.Value;
                        tarea.CostoTotalME.Value = solTarea.CostoTotalME.Value;
                        tarea.CostoTotalML.Value = solTarea.CostoTotalML.Value;
                        tarea.CostoTotalUnitME.Value = solTarea.CostoTotalUnitME.Value;
                        tarea.CostoTotalUnitML.Value = solTarea.CostoTotalUnitML.Value;
                        tarea.Descriptivo.Value = solTarea.Descriptivo.Value;
                        tarea.DetalleInd.Value = solTarea.DetalleInd.Value;
                        tarea.FechaInicio.Value = solTarea.FechaInicio.Value;
                        tarea.FechaFin.Value = solTarea.FechaFin.Value;
                        tarea.ImprimirTareaInd.Value = solTarea.ImprimirTareaInd.Value;
                        tarea.Nivel.Value = solTarea.Nivel.Value;
                        tarea.Observacion.Value = solTarea.Observacion.Value;
                        tarea.Observaciones.Value = solTarea.Observaciones.Value;
                        tarea.TareaPadre.Value = solTarea.TareaPadre.Value;
                        tarea.UnidadInvID.Value = solTarea.UnidadInvID.Value;
                        tarea.UsuarioID.Value = solTarea.UsuarioID.Value;
                        tarea.PorDescuento.Value = solTarea.PorDescuento.Value;
                        tarea.AjCambioLocal.Value = solTarea.AjCambioLocal.Value;
                        tarea.AjCambioExtra.Value = solTarea.AjCambioExtra.Value;
                        int consTarea = this._dal_pyProyectoTarea.DAL_pyProyectoTarea_Add(tarea);
                        #endregion
                        #region Guarda Detalle(pyProyectoDeta,pyProyectoMvto)
                        foreach (DTO_pyPreProyectoDeta solDet in solTarea.Detalle)
                        {
                            #region Guarda en pyProyectoDeta
                            DTO_pyProyectoDeta det = new DTO_pyProyectoDeta();
                            det.NumeroDoc.Value = numDoc;
                            det.TrabajoID.Value = solDet.TrabajoID.Value;
                            det.ConsecTarea.Value = consTarea;
                            det.Cantidad.Value = solDet.Cantidad.Value;
                            det.CantidadTOT.Value = solDet.CantidadTOT.Value;
                            det.CantSolicitud.Value = solDet.CantSolicitud.Value;
                            det.CostoExtra.Value = solDet.CostoExtra.Value;
                            det.CostoExtraEMP.Value = solDet.CostoExtraEMP.Value;
                            det.CostoExtraPRY.Value = solDet.CostoExtraPRY.Value;
                            det.CostoExtraTOT.Value = solDet.CostoExtraTOT.Value;
                            det.CostoLocal.Value = solDet.CostoLocal.Value;
                            det.CostoLocalEMP.Value = solDet.CostoLocalEMP.Value;
                            det.CostoLocalPRY.Value = solDet.CostoLocalPRY.Value;
                            det.CostoLocalTOT.Value = solDet.CostoLocalTOT.Value;
                            det.Distancia_Turnos.Value = solDet.Distancia_Turnos.Value;
                            det.DocCompra.Value = solDet.DocCompra.Value;
                            det.FactorID.Value = solDet.FactorID.Value;
                            det.FechaFijadaInd.Value = solDet.FechaFijadaInd.Value;
                            det.FechaInicio.Value = solDet.FechaInicio.Value;
                            det.FechaFin.Value = solDet.FechaFin.Value;
                            det.FechaInicioAUT.Value = solDet.FechaInicioAUT.Value;
                            det.Observaciones.Value = solDet.Observaciones.Value;
                            det.Peso_Cantidad.Value = solDet.Peso_Cantidad.Value;
                            det.PorVariacion.Value = solDet.PorVariacion.Value;
                            det.RecursoID.Value = solDet.RecursoID.Value;
                            det.TiempoTotal.Value = solDet.TiempoTotal.Value;
                            det.UnidadInvID.Value = solDet.UnidadInvID.Value;
                            det.TipoRecurso.Value = solDet.TipoRecurso.Value;
                            int consDeta = this.pyProyectoDeta_Add(det);
                            #endregion
                            #region Guarda en pyProyectoMvto
                            result = this.pyProyectoMvto_Add(numDoc, proyectoDocu, tarea, det, consDeta);
                            if (result.Result == ResultValue.NOK)
                                return result;
                            #endregion
                        }
                        #endregion

                        porcTotal += porcParte;
                        batchProgress[tupProgress] = (int)porcTotal;
                    }
                    #endregion
                    #region Guarda Tareas - Detalle (Tareas Adicionales)
                    foreach (DTO_pyPreProyectoTarea solTareaAdic in transaccion.DetalleTareasAdic)
                    {
                        DTO_pyProyectoTarea tarea = new DTO_pyProyectoTarea();
                        tarea.NumeroDoc.Value = numDoc;
                        tarea.TareaCliente.Value = solTareaAdic.TareaCliente.Value;
                        tarea.TareaID.Value = solTareaAdic.TareaID.Value;
                        tarea.CapituloTareaID.Value = solTareaAdic.CapituloTareaID.Value;
                        tarea.CapituloDesc.Value = solTareaAdic.CapituloDesc.Value;
                        tarea.CapituloGrupoID.Value = solTareaAdic.CapituloGrupoID.Value;
                        tarea.Cantidad.Value = solTareaAdic.Cantidad.Value;
                        tarea.CentroCostoID.Value = transaccion.DocCtrl.CentroCostoID.Value;
                        tarea.CostoAdicionalInd.Value = solTareaAdic.CostoAdicionalInd.Value;
                        tarea.CostoDiferenciaML.Value = solTareaAdic.CostoDiferenciaML.Value;
                        tarea.CostoExtraCLI.Value = solTareaAdic.CostoExtraCLI.Value;
                        tarea.CostoLocalCLI.Value = solTareaAdic.CostoLocalCLI.Value;
                        tarea.CostoLocalUnitCLI.Value = solTareaAdic.CostoLocalUnitCLI.Value;
                        tarea.CostoTotalME.Value = solTareaAdic.CostoTotalME.Value;
                        tarea.CostoTotalML.Value = solTareaAdic.CostoTotalML.Value;
                        tarea.CostoTotalUnitME.Value = solTareaAdic.CostoTotalUnitME.Value;
                        tarea.CostoTotalUnitML.Value = solTareaAdic.CostoTotalUnitML.Value;
                        tarea.Descriptivo.Value = solTareaAdic.Descriptivo.Value;
                        tarea.DetalleInd.Value = solTareaAdic.DetalleInd.Value;
                        tarea.FechaInicio.Value = solTareaAdic.FechaInicio.Value;
                        tarea.FechaFin.Value = solTareaAdic.FechaFin.Value;
                        tarea.ImprimirTareaInd.Value = solTareaAdic.ImprimirTareaInd.Value;
                        tarea.Nivel.Value = solTareaAdic.Nivel.Value;
                        tarea.Observacion.Value = solTareaAdic.Observacion.Value;
                        tarea.Observaciones.Value = solTareaAdic.Observaciones.Value;
                        tarea.TareaPadre.Value = solTareaAdic.TareaPadre.Value;
                        tarea.UnidadInvID.Value = solTareaAdic.UnidadInvID.Value;
                        int consTarea = this._dal_pyProyectoTarea.DAL_pyProyectoTarea_Add(tarea);

                        #region Guarda en pyProyectoDeta
                        foreach (DTO_pyPreProyectoDeta solDet in solTareaAdic.Detalle)
                        {
                            DTO_pyProyectoDeta det = new DTO_pyProyectoDeta();
                            det.NumeroDoc.Value = numDoc;
                            det.TrabajoID.Value = solDet.TrabajoID.Value;
                            det.ConsecTarea.Value = consTarea;
                            det.Cantidad.Value = solDet.Cantidad.Value;
                            det.CantidadTOT.Value = solDet.CantidadTOT.Value;
                            det.CantSolicitud.Value = solDet.CantSolicitud.Value;
                            det.CostoExtra.Value = solDet.CostoExtra.Value;
                            det.CostoExtraEMP.Value = solDet.CostoExtraEMP.Value;
                            det.CostoExtraPRY.Value = solDet.CostoExtraPRY.Value;
                            det.CostoExtraTOT.Value = solDet.CostoExtraTOT.Value;
                            det.CostoLocal.Value = solDet.CostoLocal.Value;
                            det.CostoLocalEMP.Value = solDet.CostoLocalEMP.Value;
                            det.CostoLocalPRY.Value = solDet.CostoLocalPRY.Value;
                            det.CostoLocalTOT.Value = solDet.CostoLocalTOT.Value;
                            det.Distancia_Turnos.Value = solDet.Distancia_Turnos.Value;
                            det.DocCompra.Value = solDet.DocCompra.Value;
                            det.FactorID.Value = solDet.FactorID.Value;
                            det.FechaFijadaInd.Value = solDet.FechaFijadaInd.Value;
                            det.FechaInicio.Value = solDet.FechaInicio.Value;
                            det.FechaFin.Value = solDet.FechaFin.Value;
                            det.FechaInicioAUT.Value = solDet.FechaInicioAUT.Value;
                            det.Observaciones.Value = solDet.Observaciones.Value;
                            det.Peso_Cantidad.Value = solDet.Peso_Cantidad.Value;
                            det.PorVariacion.Value = solDet.PorVariacion.Value;
                            det.RecursoID.Value = solDet.RecursoID.Value;
                            det.TiempoTotal.Value = solDet.TiempoTotal.Value;
                            det.UnidadInvID.Value = solDet.UnidadInvID.Value;
                            det.TipoRecurso.Value = solDet.TipoRecurso.Value;
                            this.pyProyectoDeta_Add(det);
                        }
                        #endregion

                        porcTotal += porcParte;
                        batchProgress[tupProgress] = (int)porcTotal;
                    }
                    #endregion 
                    #endregion

                    #region Se Aprueba el doc de Preproyecto
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, transaccion.DocCtrl.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, string.Empty, true);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = 100;

                    #endregion
                    #region Actualiza el coProyecto
                    DTO_coProyecto proy = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, transaccion.DocCtrl.ProyectoID.Value, true, false);
                    proy.FApertura.Value = fechaInicio;
                    proy.CtrlVersion.Value = proy.CtrlVersion.Value++;
                    this._dal_MasterHierarchy.DocumentID = AppMasters.coProyecto;
                    result = this._dal_MasterHierarchy.DAL_MasterHierarchy_Update(proy, true);

                    if (result.Result == ResultValue.NOK)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_AddDocument;
                        return result;
                    }
                    #endregion 
                }
                else //Si es el Proyecto Final lo aprueba
                {
                    this._dal_pyProyectoDetaHist = (DAL_pyProyectoDetaHistoria)this.GetInstance(typeof(DAL_pyProyectoDetaHistoria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_pyProyectoTareaHist = (DAL_pyProyectoTareaHistoria)this.GetInstance(typeof(DAL_pyProyectoTareaHistoria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, transaccion.DocCtrl.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, string.Empty, true);
                    _docControl = transaccion.DocCtrl;
                    numDoc = _docControl.NumeroDoc.Value.Value;
                    #region Actualiza el Doc Control
                    _docControl.Valor.Value = transaccion.Detalle.Sum(x => x.CostoTotalML.Value);
                    _docControl.Iva.Value = transaccion.Header.ValorIVA.Value;
                    this._moduloGlobal.glDocumentoControl_Update(_docControl, true, true);
                    #endregion
                    #region Actualiza en pyProyectoDocu
                    transaccion.HeaderProyecto.VersionPrevia.Value = transaccion.HeaderProyecto.Version.Value;
                    this._dal_pyProyectoDocu.DAL_pyProyectoDocu_Upd(transaccion.HeaderProyecto);
                    #endregion
                    #region Actualiza Tareas y Recursos
                    #region Elimina las tareas solicitadas
                    foreach (int t in transaccion.HeaderProyecto.TareasDeleted)
                    {
                        this._dal_pyProyectoDeta.DAL_pyProyectoDeta_Delete(t, null);
                        this._dal_pyProyectoTarea.DAL_pyProyectoTarea_DeleteByConsecutivo(t);
                    }
                    #endregion
                    #region Elimina los recursos solicitados
                    foreach (int r in transaccion.HeaderProyecto.RecursosDeleted)
                        this._dal_pyProyectoDeta.DAL_pyProyectoDeta_GetByConsecutivo(r);
                    #endregion
                    foreach (DTO_pyProyectoTarea tarea in transaccion.DetalleProyecto)
                    {
                        //Valida si la tarea existe
                        bool exist = this._dal_pyProyectoTarea.DAL_pyProyectoTarea_Exist(tarea.Consecutivo.Value);
                        if (exist)
                        {
                            #region Actualiza (pyProyectoTarea-pyProyectoDeta)
                            this._dal_pyProyectoTarea.DAL_pyProyectoTarea_Upd(tarea);
                            foreach (DTO_pyProyectoDeta det in tarea.Detalle)
                            {
                                //Valida si el detalle existe
                                bool existDeta = this._dal_pyProyectoDeta.DAL_pyProyectoDeta_Exist(det.Consecutivo.Value);
                                if (existDeta)
                                {
                                    det.TrabajoID.Value = trabajoXDef;
                                    this._dal_pyProyectoDeta.DAL_pyProyectoDeta_Upd(det);

                                    #region Gaurda en pyProyectoMvto la nueva cantidad
                                    DTO_pyProyectoMvto filter = new DTO_pyProyectoMvto();
                                    filter.NumeroDoc.Value = transaccion.DocCtrl.NumeroDoc.Value;
                                    filter.ConsecDeta.Value = det.Consecutivo.Value;
                                    List<DTO_pyProyectoMvto> movsByDeta = this.pyProyectoMvto_GetParameter(filter);
                                    if (movsByDeta.Count > 0)
                                    {
                                        decimal cantExist = movsByDeta.Sum(x => x.CantidadTOT.Value.Value);
                                        decimal cantNueva = 0;
                                        DTO_inUnidad unidad = (DTO_inUnidad)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inUnidad, det.UnidadInvID.Value, true, false);
                                        int decimalAprox = unidad != null && unidad.AproximaEnteroInd.Value.Value ? 0 : 2;
                                        if (det.TipoRecurso.Value == (byte)TipoRecurso.Equipo && !transaccion.HeaderProyecto.EquipoCantidadInd.Value.Value)
                                            cantNueva = det.FactorID.Value != 0 ? Math.Round(tarea.Cantidad.Value.Value / det.FactorID.Value.Value, decimalAprox) : 0;
                                        if (det.TipoRecurso.Value == (byte)TipoRecurso.Equipo && transaccion.HeaderProyecto.EquipoCantidadInd.Value.Value)
                                            cantNueva = det.FactorID.Value != 0 ? Math.Round(tarea.Cantidad.Value.Value * det.FactorID.Value.Value, decimalAprox) : 0;
                                        if (det.TipoRecurso.Value == (byte)TipoRecurso.Personal && !transaccion.HeaderProyecto.PersonalCantidadInd.Value.Value)
                                            cantNueva = det.FactorID.Value != 0 ? Math.Round(tarea.Cantidad.Value.Value / det.FactorID.Value.Value, decimalAprox) : 0;
                                        if (det.TipoRecurso.Value == (byte)TipoRecurso.Personal && transaccion.HeaderProyecto.PersonalCantidadInd.Value.Value)
                                            cantNueva = det.FactorID.Value != 0 ? Math.Round(tarea.Cantidad.Value.Value * det.FactorID.Value.Value, decimalAprox) : 0;
                                        else
                                            cantNueva = Math.Round(tarea.Cantidad.Value.Value * det.FactorID.Value.Value, decimalAprox);

                                        //valida si hay mas cantidad requerida  para agregar nuevo movimiento
                                        if (cantExist < cantNueva) 
                                        {
                                            #region Guarda en pyProyectoMvto
                                            DTO_pyProyectoMvto mvto = new DTO_pyProyectoMvto();
                                            mvto.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                            mvto.ConsecDeta.Value = det.Consecutivo.Value;
                                            mvto.Version.Value = transaccion.HeaderProyecto.Version.Value;
                                            mvto.EmpresaID.Value = this.Empresa.ID.Value;
                                            mvto.TipoMvto.Value = 1;// Adicion
                                            mvto.NumProyecto.Value = _docControl.NumeroDoc.Value;
                                            mvto.FactorID.Value = det.FactorID.Value;
                                            mvto.CostoLocal.Value = det.CostoLocal.Value;
                                            mvto.CostoExtra.Value = det.CostoExtra.Value;

                                            mvto.CantidadTOT.Value = cantNueva - cantExist;
                                            mvto.CantidadSOL.Value = 0;
                                            mvto.CantidadNOM.Value = 0;
                                            mvto.CantidadACT.Value = 0;
                                            mvto.CantidadINV.Value = 0;
                                            mvto.CantidadBOD.Value = 0;
                                            mvto.CantidadREC.Value = 0;
                                            mvto.CantidadPROV.Value = 0;
                                            mvto.CostoLocalTOT.Value = det.FactorID.Value != 0 ? (det.CostoLocalTOT.Value / det.FactorID.Value) : 0;
                                            mvto.CostoExtraTOT.Value = det.CostoExtraTOT.Value;
                                            mvto.FechaInicioTarea.Value = tarea.FechaInicio.Value;
                                            DTO_pyRecurso rec = (DTO_pyRecurso)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyRecurso, det.RecursoID.Value, true, false);
                                            #region Calcula Fecha Compra
                                            int diasCompraByTipoRef = 0;
                                            if (rec != null)
                                            {
                                                #region Obtiene Dias Compra por Tipo Ref
                                                DTO_inReferencia refer = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, rec.inReferenciaID.Value, true, false);
                                                if (refer != null)
                                                {
                                                    DTO_inRefTipo tipoRef = (DTO_inRefTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefTipo, refer.TipoInvID.Value, true, false);
                                                    diasCompraByTipoRef = tipoRef.DiasCompra.Value.Value;
                                                }
                                                #endregion
                                                #region Obtiene la Codigo de BS
                                                if (rec.TipoRecurso.Value == (byte)TipoRecurso.Insumo)
                                                    mvto.CodigoBSID.Value = codigoBSInventarios;
                                                else
                                                {
                                                    if (string.IsNullOrEmpty(rec.CodigoBSID.Value))
                                                    {
                                                        result.Result = ResultValue.NOK;
                                                        result.ResultMessage = "Debe Parametrizar el CodigoBS del recurso " + rec.ID.Value;
                                                        return result;
                                                    }
                                                    else
                                                        mvto.CodigoBSID.Value = rec.CodigoBSID.Value;
                                                }

                                                #endregion
                                                #region Obtiene la linea Presupuesto
                                                DTO_MasterBasic dtoLinea = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, tarea.CapituloTareaID.Value, true, false);
                                                if (dtoLinea != null)
                                                    mvto.LineaPresupuestoID.Value = tarea.CapituloTareaID.Value;
                                                else
                                                {
                                                    DTO_prBienServicio bs = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prBienServicio, mvto.CodigoBSID.Value, true, false);
                                                    if (bs != null)
                                                    {
                                                        DTO_glBienServicioClase bsClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bs.ClaseBSID.Value, true, false);
                                                        mvto.LineaPresupuestoID.Value = bsClase.LineaPresupuestoID.Value;
                                                    }
                                                }
                                                #endregion
                                            }

                                            #region Obtiene los dias de importacion si tiene Mda Extanjera
                                            if (!string.IsNullOrEmpty(diasOCImport) && det.CostoExtra.Value != 0 && det.CostoExtra.Value != null)
                                                diasOrdCompraImport = Convert.ToInt32(diasOCImport);
                                            else
                                                diasOrdCompraImport = 0;

                                            #endregion
                                            if (mvto.FechaInicioTarea.Value != null)
                                                mvto.FechaOrdCompra.Value = mvto.FechaInicioTarea.Value.Value.AddDays(-(diasCompraByTipoRef + diasOrdCompraEmp + diasOrdCompraImport));
                                            #endregion
                                            mvto.FechaRecibido.Value = mvto.FechaInicioTarea.Value;
                                            mvto.Observaciones.Value = det.Observaciones.Value;
                                            this._dal_pyProyectoMvto.DAL_pyProyectoMvto_Add(mvto);
                                            #endregion
                                        }
                                        //valida si hay menos cantidad requerida y cantidad de proveedor para agregar nuevo movimiento negativo
                                        else if (cantExist > cantNueva && movsByDeta.Sum(x => x.CantidadPROV.Value) <= cantNueva) 
                                        {
                                            #region Guarda en pyProyectoMvto
                                            DTO_pyProyectoMvto mvto = new DTO_pyProyectoMvto();
                                            mvto.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                            mvto.ConsecDeta.Value = det.Consecutivo.Value;
                                            mvto.Version.Value = transaccion.HeaderProyecto.Version.Value;
                                            mvto.EmpresaID.Value = this.Empresa.ID.Value;
                                            mvto.TipoMvto.Value = 4;// Retiro
                                            mvto.NumProyecto.Value = _docControl.NumeroDoc.Value;
                                            mvto.FactorID.Value = det.FactorID.Value;
                                            mvto.CostoLocal.Value = det.CostoLocal.Value;
                                            mvto.CostoExtra.Value = det.CostoExtra.Value;

                                            mvto.CantidadTOT.Value = cantNueva - cantExist;
                                            mvto.CantidadSOL.Value = 0;
                                            mvto.CantidadNOM.Value = 0;
                                            mvto.CantidadACT.Value = 0;
                                            mvto.CantidadINV.Value = 0;
                                            mvto.CantidadBOD.Value = 0;
                                            mvto.CantidadREC.Value = 0;
                                            mvto.CantidadPROV.Value = 0;
                                            mvto.CostoLocalTOT.Value = det.FactorID.Value != 0 ? (det.CostoLocalTOT.Value / det.FactorID.Value) : 0;
                                            mvto.CostoExtraTOT.Value = det.CostoExtraTOT.Value;
                                            mvto.FechaInicioTarea.Value = tarea.FechaInicio.Value;
                                            DTO_pyRecurso rec = (DTO_pyRecurso)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyRecurso, det.RecursoID.Value, true, false);
                                            #region Calcula Fecha Compra
                                            int diasCompraByTipoRef = 0;
                                            if (rec != null)
                                            {
                                                #region Obtiene Dias Compra por Tipo Ref
                                                DTO_inReferencia refer = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, rec.inReferenciaID.Value, true, false);
                                                if (refer != null)
                                                {
                                                    DTO_inRefTipo tipoRef = (DTO_inRefTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefTipo, refer.TipoInvID.Value, true, false);
                                                    diasCompraByTipoRef = tipoRef.DiasCompra.Value.Value;
                                                }
                                                #endregion
                                                #region Obtiene la Codigo de BS
                                                if (rec.TipoRecurso.Value == (byte)TipoRecurso.Insumo)
                                                    mvto.CodigoBSID.Value = codigoBSInventarios;
                                                else
                                                {
                                                    if (string.IsNullOrEmpty(rec.CodigoBSID.Value))
                                                    {
                                                        result.Result = ResultValue.NOK;
                                                        result.ResultMessage = "Debe Parametrizar el CodigoBS del recurso " + rec.ID.Value;
                                                        return result;
                                                    }
                                                    else
                                                        mvto.CodigoBSID.Value = rec.CodigoBSID.Value;
                                                }

                                                #endregion
                                                #region Obtiene la linea Presupuesto
                                                DTO_MasterBasic dtoLinea = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, tarea.CapituloTareaID.Value, true, false);
                                                if (dtoLinea != null)
                                                    mvto.LineaPresupuestoID.Value = tarea.CapituloTareaID.Value;
                                                else
                                                {
                                                    DTO_prBienServicio bs = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prBienServicio, mvto.CodigoBSID.Value, true, false);
                                                    if (bs != null)
                                                    {
                                                        DTO_glBienServicioClase bsClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bs.ClaseBSID.Value, true, false);
                                                        mvto.LineaPresupuestoID.Value = bsClase.LineaPresupuestoID.Value;
                                                    }
                                                }
                                                #endregion
                                            }

                                            #region Obtiene los dias de importacion si tiene Mda Extanjera
                                            if (!string.IsNullOrEmpty(diasOCImport) && det.CostoExtra.Value != 0 && det.CostoExtra.Value != null)
                                                diasOrdCompraImport = Convert.ToInt32(diasOCImport);
                                            else
                                                diasOrdCompraImport = 0;

                                            #endregion
                                            if (mvto.FechaInicioTarea.Value != null)
                                                mvto.FechaOrdCompra.Value = mvto.FechaInicioTarea.Value.Value.AddDays(-(diasCompraByTipoRef + diasOrdCompraEmp + diasOrdCompraImport));
                                            #endregion
                                            mvto.FechaRecibido.Value = mvto.FechaInicioTarea.Value;
                                            mvto.Observaciones.Value = det.Observaciones.Value;
                                            this._dal_pyProyectoMvto.DAL_pyProyectoMvto_Add(mvto);
                                            #endregion
                                        }
                                        else //Sino actualiza el movimiento actual
                                        {
                                            foreach (DTO_pyProyectoMvto m in movsByDeta)
                                            {
                                                #region Calcula Fecha Compra
                                                DTO_pyRecurso rec = (DTO_pyRecurso)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyRecurso, det.RecursoID.Value, true, false);
                                                int diasCompraByTipoRef = 0;
                                                m.FechaInicioTarea.Value = tarea.FechaInicio.Value;
                                                if (rec != null)
                                                {
                                                    #region Obtiene Dias Compra por Tipo Ref
                                                    DTO_inReferencia refer = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, rec.inReferenciaID.Value, true, false);
                                                    if (refer != null)
                                                    {
                                                        DTO_inRefTipo tipoRef = (DTO_inRefTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefTipo, refer.TipoInvID.Value, true, false);
                                                        diasCompraByTipoRef = tipoRef.DiasCompra.Value.Value;
                                                    }
                                                    #endregion 
                                                    #region Obtiene la Codigo de BS
                                                    if (rec.TipoRecurso.Value == (byte)TipoRecurso.Insumo)
                                                        m.CodigoBSID.Value = codigoBSInventarios;
                                                    else
                                                    {
                                                        if (!string.IsNullOrEmpty(rec.CodigoBSID.Value))
                                                            m.CodigoBSID.Value = rec.CodigoBSID.Value;                                                          
                                                    }
                                                    #endregion
                                                    #region Obtiene la linea Presupuesto
                                                    DTO_MasterBasic dtoLinea = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, tarea.CapituloTareaID.Value, true, false);
                                                    if (dtoLinea != null)
                                                        m.LineaPresupuestoID.Value = tarea.CapituloTareaID.Value;
                                                    else
                                                    {
                                                        DTO_prBienServicio bs = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prBienServicio, m.CodigoBSID.Value, true, false);
                                                        if (bs != null)
                                                        {
                                                            DTO_glBienServicioClase bsClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bs.ClaseBSID.Value, true, false);
                                                            m.LineaPresupuestoID.Value = bsClase.LineaPresupuestoID.Value;
                                                        }
                                                    }

                                                    #endregion
                                                }

                                                #region Obtiene los dias de importacion si tiene Mda Extanjera
                                                if (!string.IsNullOrEmpty(diasOCImport) && det.CostoExtra.Value != 0 && det.CostoExtra.Value != null)
                                                    diasOrdCompraImport = Convert.ToInt32(diasOCImport);
                                                else
                                                    diasOrdCompraImport = 0;

                                                #endregion
                                                if (m.FechaInicioTarea.Value != null)
                                                    m.FechaOrdCompra.Value = m.FechaInicioTarea.Value.Value.AddDays(-(diasCompraByTipoRef + diasOrdCompraEmp + diasOrdCompraImport));
                                                //if (m.CantidadTOT.Value > (m.CantidadPROV.Value + m.CantidadSOL.Value))
                                                //    m.CantidadTOT.Value = cantNueva;
                                                this._dal_pyProyectoMvto.DAL_pyProyectoMvto_Upd(m);
                                                #endregion
                                            }
                                        
                                        }
                                    }
                                    else
                                    {
                                        #region Guarda en pyProyectoMvto
                                        result = this.pyProyectoMvto_Add(numDoc, transaccion.HeaderProyecto, tarea, det, det.Consecutivo.Value.Value);
                                        if (result.Result == ResultValue.NOK)
                                            return result;
                                        #endregion
                                    }
                                    #endregion
                                }                                 
                                else
                                {
                                    det.ConsecTarea.Value = tarea.Consecutivo.Value;
                                    det.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                    det.TrabajoID.Value = trabajoXDef;
                                    int conseDet = this.pyProyectoDeta_Add(det);

                                    #region Guarda en pyProyectoMvto
                                    result = this.pyProyectoMvto_Add(numDoc, transaccion.HeaderProyecto, tarea, det, conseDet);
                                    if (result.Result == ResultValue.NOK)
                                        return result;
                                    #endregion
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region Agrega nuevo (pyProyectoTarea-pyProyectoDeta)
                            tarea.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                            tarea.CentroCostoID.Value = centroCosXDef;
                            tarea.TareaCliente.Value = string.IsNullOrEmpty(tarea.TareaCliente.Value) ? tarea.TareaID.Value : tarea.TareaCliente.Value;
                            this._dal_pyProyectoTarea.DAL_pyProyectoTarea_Add(tarea);
                            foreach (DTO_pyProyectoDeta det in tarea.Detalle)
                            {
                                //Agrega nuevo detalle
                                det.ConsecTarea.Value = tarea.Consecutivo.Value;
                                det.NumeroDoc.Value = _docControl.NumeroDoc.Value;
                                det.TrabajoID.Value = string.IsNullOrEmpty(det.TrabajoID.Value) ? trabajoXDef : det.TrabajoID.Value;
                                int conseDet = this.pyProyectoDeta_Add(det);

                                #region Guarda en pyProyectoMvto
                                result = this.pyProyectoMvto_Add(numDoc, transaccion.HeaderProyecto, tarea, det, conseDet);
                                if (result.Result == ResultValue.NOK)
                                    return result;
                                #endregion
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region Guarda (pyProyectoTareaHistoria-pyProyectoDetaHistoria)
                    foreach (DTO_pyProyectoTarea tarea in transaccion.DetalleProyecto)
                    {
                        //Valida si la version existe
                        bool exist = this._dal_pyProyectoTareaHist.DAL_pyProyectoTareaHistoria_Exist(tarea.NumeroDoc.Value.Value, tarea.TareaID.Value, tarea.TareaCliente.Value,
                                                                                                transaccion.HeaderProyecto.Version.Value.Value);
                        if (!exist)
                        {
                            tarea.Consecutivo.Value = null;
                            tarea.Version.Value = transaccion.HeaderProyecto.Version.Value;
                            this._dal_pyProyectoTareaHist.DAL_pyProyectoTareaHistoria_Add(tarea);
                            foreach (DTO_pyProyectoDeta det in tarea.Detalle)
                            {
                                bool existDeta = this._dal_pyProyectoDetaHist.DAL_pyProyectoDetaHistoria_Exist(det.ConsecTarea.Value.Value, det.RecursoID.Value, transaccion.HeaderProyecto.Version.Value.Value);
                                if (!existDeta)
                                {
                                    det.Consecutivo.Value = null;
                                    det.ConsecTarea.Value = tarea.Consecutivo.Value;
                                    det.Version.Value = transaccion.HeaderProyecto.Version.Value;
                                    this._dal_pyProyectoDetaHist.DAL_pyProyectoDetaHistoria_Add(det);
                                }                                    
                            }
                        }
                    }
                    #endregion 

                    //byte versionPrevia = Convert.ToByte(transaccion.HeaderProyecto.Version.Value.Value - 1);
                    //var tareasPrevia = this._dal_pyProyectoTareaHist.DAL_pyProyectoTareaHistoria_Get(transaccion.DocCtrl.NumeroDoc.Value.Value, versionPrevia);
                }

                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    //Trae la info de la alarma
                    alarma = this.GetFirstMailInfo(numDoc, true);
                    alarma.NumeroDoc = numDoc.ToString();
                    alarma.PrefijoID = transaccion.DocCtrl.PrefijoID.Value.TrimEnd();
                    alarma.Consecutivo = transaccion.DocCtrl.DocumentoNro.Value.ToString();
                    return alarma;
                    #endregion
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "SolicitudProyecto_AprobarProy");
                throw ex;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        if (transaccion.DocCtrl.DocumentoID.Value == AppDocuments.PreProyecto)
                        {
                            _docControl.DocumentoNro.Value = Convert.ToInt32(this.GenerarDocumentoNro(AppDocuments.Proyecto, _docControl.PrefijoID.Value));
                            this._moduloGlobal.ActualizaConsecutivos(_docControl, true, false, false);

                            numDoc = _docControl.NumeroDoc.Value.Value;
                            result.ExtraField = _docControl.DocumentoNro.Value.ToString();
                            alarma.Consecutivo = _docControl.DocumentoNro.Value.ToString(); 
                        }                       
                    }
                }
                else if (base._mySqlConnectionTx != null)
                    base._mySqlConnectionTx.Rollback();
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
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyectoDetaCLI = (DAL_pyProyectoDetaCLI)this.GetInstance(typeof(DAL_pyProyectoDetaCLI), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyPreProyectoDetaCLI = (DAL_pyPreProyectoDetaCLI)this.GetInstance(typeof(DAL_pyPreProyectoDetaCLI), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables iniciales
                string trabajoXDef = this.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TrabajoDefecto);

                if (string.IsNullOrEmpty(trabajoXDef))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.py).ToString() + AppControl.py_TrabajoDefecto + "&&" + string.Empty;
                    return result;
                }
                #endregion

                #region Guarda o actualiza en pyPreProyectoDetaCLI
                foreach (DTO_pyPreProyectoTarea tarea in transaccion.Detalle)
                {
                    #region Borra el detalle requerido en la tabla
                    foreach (int det in tarea.APUDeleted)
                        this._dal_pyProyectoDetaCLI.DAL_pyProyectoDetaCLI_Delete(det); 
                    #endregion
                    foreach (DTO_pyPreProyectoDeta det in tarea.DetalleAPUCliente)
                    {
                        bool existDeta = this._dal_pyProyectoDetaCLI.DAL_pyProyectoDetaCLI_Exist(det.Consecutivo.Value);
                        if (existDeta)
                            this._dal_pyPreProyectoDetaCLI.DAL_pyPreProyectoDetaCLI_Upd(det);
                        else
                        {
                            det.ConsecTarea.Value = tarea.Consecutivo.Value;
                            det.NumeroDoc.Value = numeroDoc;
                            det.TrabajoID.Value = !string.IsNullOrEmpty(det.TrabajoID.Value) ? det.TrabajoID.Value : trabajoXDef;
                            this._dal_pyPreProyectoDetaCLI.DAL_pyPreProyectoDetaCLI_Add(det);
                        }
                    }
                }
                #endregion   
               
                if (saveProyectoInd)
                {
                    bool estadoProyecto = transaccion.DocCtrl.Estado.Value == (byte)EstadoDocControl.Aprobado ? true : false;
                    if (estadoProyecto)
                    {
                        //Trae el proyecto final
                        var trans = this.SolicitudProyecto_Load(AppDocuments.Proyecto, string.Empty, null, null, string.Empty, transaccion.DocCtrl.ProyectoID.Value, false, false, false, false);

                        if (trans != null)
                        {
                            foreach (DTO_pyProyectoTarea tarea in trans.DetalleProyecto)
                            {
                                //Elimina el detalle para reemplazarlo
                                this._dal_pyProyectoDetaCLI.DAL_pyProyectoDetaCLI_DeleteByConsecTarea(tarea.Consecutivo.Value);
                                #region Guarda Detalle (pyProyectoDetaCLI)
                                //Trae la info del PreProyecto para agregarla al Proyecto 
                                var tareaPreProyecto = transaccion.Detalle.Find(x => x.TareaID.Value == tarea.TareaID.Value && x.TareaCliente.Value == tarea.TareaCliente.Value);
                                if (tareaPreProyecto != null)
                                {
                                    foreach (DTO_pyPreProyectoDeta det in tareaPreProyecto.DetalleAPUCliente)
                                    {
                                        #region Guarda en pyProyectoDetaCLI
                                        DTO_pyProyectoDeta detCLI = new DTO_pyProyectoDeta();
                                        detCLI.NumeroDoc.Value = tarea.NumeroDoc.Value;
                                        detCLI.TrabajoID.Value = det.TrabajoID.Value;
                                        detCLI.ConsecTarea.Value = tarea.Consecutivo.Value;
                                        detCLI.Cantidad.Value = det.Cantidad.Value;
                                        detCLI.CantidadTOT.Value = det.CantidadTOT.Value;
                                        detCLI.CantSolicitud.Value = det.CantSolicitud.Value;
                                        detCLI.CostoExtra.Value = det.CostoExtra.Value;
                                        detCLI.CostoExtraEMP.Value = det.CostoExtraEMP.Value;
                                        detCLI.CostoExtraPRY.Value = det.CostoExtraPRY.Value;
                                        detCLI.CostoExtraTOT.Value = det.CostoExtraTOT.Value;
                                        detCLI.CostoLocal.Value = det.CostoLocal.Value.Value;
                                        detCLI.CostoLocalEMP.Value = det.CostoLocalEMP.Value;
                                        detCLI.CostoLocalPRY.Value = det.CostoLocalPRY.Value;
                                        detCLI.CostoLocalTOT.Value = det.CostoLocalTOT.Value;
                                        detCLI.Distancia_Turnos.Value = det.Distancia_Turnos.Value;
                                        detCLI.DocCompra.Value = det.DocCompra.Value;
                                        detCLI.FactorID.Value = det.FactorID.Value;
                                        detCLI.FechaFijadaInd.Value = det.FechaFijadaInd.Value;
                                        detCLI.FechaInicio.Value = det.FechaInicio.Value;
                                        detCLI.FechaFin.Value = det.FechaFin.Value;
                                        detCLI.FechaInicioAUT.Value = det.FechaInicioAUT.Value;
                                        detCLI.Observaciones.Value = det.Observaciones.Value;
                                        detCLI.Peso_Cantidad.Value = det.Peso_Cantidad.Value;
                                        detCLI.PorVariacion.Value = det.PorVariacion.Value;
                                        detCLI.RecursoID.Value = det.RecursoID.Value;
                                        detCLI.TiempoTotal.Value = det.TiempoTotal.Value;
                                        detCLI.UnidadInvID.Value = det.UnidadInvID.Value;
                                        int consDeta = this._dal_pyProyectoDetaCLI.DAL_pyProyectoDetaCLI_Add(detCLI);
                                        #endregion
                                    } 
                                }
                                #endregion
                            } 
                        }                     
                    }
                    else
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "El documento del proyecto aún no se encuentra aprobado";
                        return result;
                    }
                }
                result.ExtraField = transaccion.DocCtrl.DocumentoNro.Value.ToString();
                return result;
            }
            catch (Exception ex)
            {
                numeroDoc = 0;
                transaccion.Header.NumeroDoc.Value = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ModuloProyectos_pyPreProyectoDetaGenerarSolicitud");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    base._mySqlConnectionTx.Commit();                   
                }
                else if (base._mySqlConnectionTx != null)
                {
                    base._mySqlConnectionTx.Rollback();
                }

            }
        }

        #region pyPreProyectoDocu
        /// <summary>
        /// Cambia la actividad del flujo
        /// </summary>
        /// <param name="documentID">Identificador documento</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="actividadFlujoID">actividad flujo</param>
        /// <returns></returns>
        public DTO_TxResult pyPreProyectoDocu_AsignaActividad(int documentID, int numeroDoc, string actividadFlujoID)
        {
            DTO_TxResult result = this.AsignarFlujo(documentID, numeroDoc, actividadFlujoID, false, string.Empty);
            return result;
        }

        /// <summary>
        /// Obtiene el detalle del documento
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>Detalle Documento</returns>
        public DTO_pyPreProyectoDocu pyPreProyectoDocu_Get(int numeroDoc)
        {
            try
            {
                this._dal_pyPreProyectoDocu = (DAL_pyPreProyectoDocu)this.GetInstance(typeof(DAL_pyPreProyectoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                var data = this._dal_pyPreProyectoDocu.DAL_pyPreProyectoDocu_Get(numeroDoc);
                return data;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyPreProyectoDocu_Get");
                throw exception;
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
        public DTO_glDocumentoControl pyPreProyectoDocu_GetInternalDoc(int documentID, string idPrefijo, int documentoNro, string actividadFlujoID)
        {
            try
            {
                this._dal_pyPreProyectoDocu = (DAL_pyPreProyectoDocu)this.GetInstance(typeof(DAL_pyPreProyectoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                var data = this._dal_pyPreProyectoDocu.DAL_pyProyectoDocu_GetInternalDoc(documentID, idPrefijo, documentoNro, actividadFlujoID);
                return data;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyPreProyectoDocu_Get");
                throw exception;
            }
        }

        #endregion

        #region pyPreProyectoTarea
        /// <summary>
        /// Consulta las tareas existentes con filtro
        /// </summary>
        /// <param name="numeroDoc">Identificador documento</param>
        /// <param name="tarea">tarea a analizar</param>
        /// <returns>Lista de resultados</returns>
        public List<DTO_pyPreProyectoTarea> pyPreProyectoTarea_Get(int? numeroDoc, string tareaID, string claseServicioID, bool recursoTimeInd = false)
        {
            try
            {
                this._dal_pyPreProyectoTarea = (DAL_pyPreProyectoTarea)this.GetInstance(typeof(DAL_pyPreProyectoTarea), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_pyPreProyectoTarea> tareas = this._dal_pyPreProyectoTarea.DAL_pyPreProyectoTarea_Get(numeroDoc, tareaID, claseServicioID).ToList();
                if (numeroDoc != null) //Existente
                {
                    DTO_pyClaseProyecto claseProy = (DTO_pyClaseProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, claseServicioID, true, false);
                    List<DTO_pyPreProyectoDeta> recursosDet = this.pyPreProyectoDeta_GetByParameter(0, string.Empty, string.Empty, numeroDoc, true, recursoTimeInd).ToList();
                    int index = 0;
                    foreach (DTO_pyPreProyectoTarea t in tareas)
                    {
                        t.Index = index;
                        t.Detalle = recursosDet.FindAll(x => x.ConsecTarea.Value == t.Consecutivo.Value).ToList();
                        //t.CostoTotalML.Value = t.Detalle.Sum(x => x.CostoLocalTOT.Value);
                        if (t.Cantidad.Value != null && t.Cantidad.Value != 0)
                            t.CostoLocalUnitCLI.Value = t.CostoLocalCLI.Value / t.Cantidad.Value;
                        t.CostoDiferenciaML.Value = t.CostoLocalCLI.Value - t.CostoTotalML.Value;
                        DTO_pyTarea dtoTarea = (DTO_pyTarea)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyTarea, t.TareaID.Value, true, false);
                        if (claseProy != null && dtoTarea != null) // Valida la lineaPresupuesto a usar(Cliente/Interno)
                            t.LineaPresupuestoID.Value = claseProy.TipoPresupuesto.Value == 1 ? t.TareaID.Value : dtoTarea.LineaPresupuestoID.Value;
                        index++;
                    }
                }
                else // Nuevo
                {
                    List<DTO_pyPreProyectoDeta> recursosDet = this.pyPreProyectoDeta_GetByParameter(0, string.Empty, claseServicioID, null, false,false).ToList();
                    int index = 0;
                    foreach (DTO_pyPreProyectoTarea t in tareas)
                    {
                        t.Index = index;
                        t.Detalle = recursosDet.FindAll(x => x.TareaID.Value == t.TareaID.Value);
                        //t.CostoTotalML.Value = t.Detalle.Sum(x => x.CostoLocalTOT.Value);
                        DTO_pyTarea dtoTarea = (DTO_pyTarea)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyTarea, t.TareaID.Value, true, false);
                        if (dtoTarea != null)
                            t.LineaPresupuestoID.Value = dtoTarea.LineaPresupuestoID.Value;
                        index++;
                    }
                }
                return tareas;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyPreProyectoTarea_GetByTrabajo");
                throw exception;
            }
        }

        #endregion

        #region pyPreProyectoDeta
        /// <summary>
        /// Trae una lista de servicio Deta
        /// </summary>
        /// <param name="documentID">Identificador del Documento</param>
        /// <param name="tareaID">Identificador del prefijo</param>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="loadDetaExist">Indica si carga detalle de un documento</param>
        ///  <param name="recursoTimeInd">Indica si carga si carga los recursos de solo tiempo</param>
        /// <returns>Lista de componentes</returns>
        public List<DTO_pyPreProyectoDeta> pyPreProyectoDeta_GetByParameter(int documentID, string tareaID, string claseServicioID, int? numeroDoc, bool loadDetaExist, bool recursoTimeInd, decimal tasaCambio = 0)
        {
            try
            {
                List<DTO_pyPreProyectoDeta> result = new List<DTO_pyPreProyectoDeta>();
                this._dal_pyPreProyectoDeta = (DAL_pyPreProyectoDeta)this.GetInstance(typeof(DAL_pyPreProyectoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                if (!string.IsNullOrEmpty(tareaID)) //Carga con tarea
                {
                    DTO_pyClaseProyecto dtoClase = (DTO_pyClaseProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, claseServicioID, true, false);

                    if(dtoClase != null && dtoClase.TareaAsociadaInd.Value.Value)
                    {
                        Dictionary<string, string> pks = new Dictionary<string, string>();
                        pks.Add("TareaID", tareaID);
                        pks.Add("ClaseServicioID", claseServicioID);
                        DTO_pyTareaClase dtoTarCla = (DTO_pyTareaClase)this.GetMasterComplexDTO(AppMasters.pyTareaClase, pks, true);

                        if (dtoTarCla != null)
                            result = this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_GetByTarea(tareaID, claseServicioID, numeroDoc, loadDetaExist, recursoTimeInd);
                    }
                    else
                        result = this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_GetByTarea(tareaID, string.Empty, numeroDoc, loadDetaExist, recursoTimeInd);
                }
                else // carga Todo
                    result = this._dal_pyPreProyectoDeta.DAL_pyPreProyectoDeta_GetByTarea(tareaID, string.Empty, numeroDoc, loadDetaExist, recursoTimeInd);

                if (!loadDetaExist)
                {
                    foreach (DTO_pyPreProyectoDeta d in result)
                    {
                        d.Cantidad.Value = 1;
                        d.CantSolicitud.Value = 1;
                    }
                }

                foreach (DTO_pyPreProyectoDeta rec in result.FindAll(x => x.CostoLocal.Value == 0 && x.CostoExtra.Value != 0 && x.CostoExtra.Value != null))
                        rec.CostoLocal.Value = tasaCambio * rec.CostoExtra.Value; 
                        

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ComponentesAnalisis_GetByTarea");
                throw exception;
            }
        }

        /// <summary>
        /// Carga la Solicitud de Servicio
        /// </summary>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="claseServicioID">clase servicio</param>
        /// <param name="existe">si existe la solicitud</param>
        /// <returns>detalle solicitud</returns>
        public List<DTO_pySolServicio> pyProyectoDeta_GetSolicitud(int numeroDoc, string claseServicioID, string actividadFlujoID)
        {
            try
            {
                this._dal_pyPreProyectoDeta = (DAL_pyPreProyectoDeta)this.GetInstance(typeof(DAL_pyPreProyectoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //List<DTO_pySolServicioPrograma> data = null;
                List<DTO_pySolServicio> lServicio = new List<DTO_pySolServicio>();
                //DTO_pySolServicio servicio = null;

                //data = this._dal_pySolServicioPrograma.DAL_pySolServicioPrograma_Get(numeroDoc, claseServicioID);

                //foreach (var item in data)
                //{
                //    servicio = new DTO_pySolServicio();
                //    servicio.LineaFlujoID.Value = item.LineaFlujoID.Value;
                //    servicio.LineaFlujoIDDesc.Value = item.LineaFlujoIDDesc.Value;
                //    servicio.ActividadEtapaID.Value = item.ActividadEtapaID.Value;
                //    servicio.ActividadEtapaIDDesc.Value = item.ActividadEtapaIDDesc.Value;
                //    servicio.TareaID.Value = item.TareaID.Value;
                //    servicio.TareaIDDesc.Value = item.TareaIDDesc.Value;
                //    servicio.TrabajoID.Value = item.TrabajoID.Value;
                //    servicio.TrabajoIDDesc.Value = item.TrabajoIDDesc.Value;
                //    servicio.CantidadTR.Value = item.CantidadTR.Value;
                //    servicio.CentroCostoIDDesc.Value = item.CentroCostoIDDesc.Value;
                //    servicio.LineaPresupuestoID.Value = item.LineaPresupuestoID.Value;
                //    servicio.LineaPresupuestoIDDesc.Value = item.LineaPresupuestoIDDesc.Value;
                //    servicio.ClaseServicioID.Value = item.ClaseServicioID.Value;
                //    servicio.Observaciones.Value = item.Observaciones.Value;
                //    servicio.SemanaPrograma.Value = item.SemanaPrograma.Value;
                //    servicio.SemanaProgramaFin.Value = item.SemanaProgramaFin.Value;
                //    lServicio.Add(servicio);
                //}

                //foreach (var item in lServicio)
                //{
                //    item.Detalle = this._DAL_pyPreProyectoDeta.DAL_pyPreProyectoDeta_Get(numeroDoc, claseServicioID,
                //                                                                                item.LineaFlujoID.Value, item.TareaID.Value,
                //                                                                                item.ActividadEtapaID.Value, item.TrabajoID.Value);
                //}  

                return lServicio;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyServicioDeta_GetSolicitud");
                throw exception;
            }
        }
        
        #endregion

        #region pyProyectoDocu
 
        /// <summary>
        /// Obtiene el detalle del documento
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>Detalle Documento</returns>
        public DTO_pyProyectoDocu pyProyectoDocu_Get(int numeroDoc)
        {
            try
            {
                this._dal_pyProyectoDocu = (DAL_pyProyectoDocu)this.GetInstance(typeof(DAL_pyProyectoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                var data = this._dal_pyProyectoDocu.DAL_pyProyectoDocu_Get(numeroDoc);
                return data;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyProyectoDocu_Get");
                throw exception;
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
                this._dal_pyProyectoTarea = (DAL_pyProyectoTarea)this.GetInstance(typeof(DAL_pyProyectoTarea), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_pyProyectoTarea> tareas = this._dal_pyProyectoTarea.DAL_pyProyectoTarea_Get(numeroDoc, tareaID).ToList();
                if (numeroDoc != null)
                {
                    DTO_pyClaseProyecto claseProy = (DTO_pyClaseProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, claseServicioID, true, false);
                    List<DTO_pyProyectoDeta> recursosDet = this.pyProyectoDeta_GetByParameter(0, string.Empty, string.Empty, numeroDoc, true, recursoTimeInd).ToList();
                    int index = 0;
                    foreach (DTO_pyProyectoTarea t in tareas)
                    {
                        t.Index = index;
                        t.Detalle = recursosDet.FindAll(x => x.ConsecTarea.Value == t.Consecutivo.Value).ToList();
                        if (t.Cantidad.Value != null && t.Cantidad.Value != 0)
                            t.CostoLocalUnitCLI.Value = t.CostoLocalCLI.Value / t.Cantidad.Value;
                        t.CostoDiferenciaML.Value = t.CostoLocalCLI.Value - t.CostoTotalML.Value;
                        DTO_pyTarea dtoTarea = (DTO_pyTarea)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyTarea, t.TareaID.Value, true, false);
                        if (claseProy != null && dtoTarea != null) // Valida la lineaPresupuesto a usar(Cliente/Interno)
                            t.LineaPresupuestoID.Value = claseProy.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Construccion ? t.TareaID.Value : dtoTarea.LineaPresupuestoID.Value;
                        else if( dtoTarea != null)
                            t.LineaPresupuestoID.Value = dtoTarea.LineaPresupuestoID.Value;
                        index++;
                    }
                }
                return tareas;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyProyectoTarea_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta las tareas existentes con filtro
        /// </summary>
        /// <param name="numeroDoc">Identificador documento</param>
        /// <param name="tarea">tarea a analizar</param>
        /// <param name="claseServicio">clase servicio</param>
        /// <param name="recursoTimeInd">valida si es recurso de tiempo</param>
        /// <returns>Lista de resultados</returns>
        public DTO_pyProyectoTarea pyProyectoTarea_GetByConsecutivo(int consecutivo)
        {
            try
            {
                this._dal_pyProyectoTarea = (DAL_pyProyectoTarea)this.GetInstance(typeof(DAL_pyProyectoTarea), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyectoDeta = (DAL_pyProyectoDeta)this.GetInstance(typeof(DAL_pyProyectoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_pyProyectoTarea tarea = this._dal_pyProyectoTarea.DAL_pyProyectoTarea_GetByConsecutivo(consecutivo);
                if (tarea != null)
                {
                    List<DTO_pyProyectoDeta> recursosDet = this._dal_pyProyectoDeta.DAL_pyProyectoDeta_GetByTarea(string.Empty, string.Empty, null, true, false, tarea.Consecutivo.Value.Value);
                    tarea.Detalle = recursosDet;
                }
                return tarea;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyProyectoTarea_GetByConsecutivo");
                throw exception;
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
                List<DTO_pyProyectoDeta> result = new List<DTO_pyProyectoDeta>();
                this._dal_pyProyectoDeta = (DAL_pyProyectoDeta)this.GetInstance(typeof(DAL_pyProyectoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                if (numeroDoc != null)
                {
                    result = this._dal_pyProyectoDeta.DAL_pyProyectoDeta_GetByNumeroDoc(numeroDoc);

                    this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)this.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    foreach (DTO_pyProyectoDeta deta in result)
                    {
                        DTO_pyProyectoMvto filter = new DTO_pyProyectoMvto();
                        filter.ConsecDeta.Value = deta.Consecutivo.Value;
                        deta.DetalleMvto = this.pyProyectoMvto_GetParameter(filter);
                    }                    
                }
                else
                {
                    if (!string.IsNullOrEmpty(tareaID))
                    {
                        DTO_pyClaseProyecto dtoClase = (DTO_pyClaseProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, claseServicioID, true, false);

                        if (dtoClase != null && dtoClase.TareaAsociadaInd.Value.Value)
                        {
                            Dictionary<string, string> pks = new Dictionary<string, string>();
                            pks.Add("TareaID", tareaID);
                            pks.Add("ClaseServicioID", claseServicioID);
                            DTO_pyTareaClase dtoTarCla = (DTO_pyTareaClase)this.GetMasterComplexDTO(AppMasters.pyTareaClase, pks, true);

                            if (dtoTarCla != null)
                                result = this._dal_pyProyectoDeta.DAL_pyProyectoDeta_GetByTarea(tareaID, claseServicioID, numeroDoc, loadDetaExist, recursoTimeInd);
                        }
                        else
                            result = this._dal_pyProyectoDeta.DAL_pyProyectoDeta_GetByTarea(tareaID, string.Empty, numeroDoc, loadDetaExist, recursoTimeInd);
                    }
                    else
                        result = this._dal_pyProyectoDeta.DAL_pyProyectoDeta_GetByTarea(tareaID, claseServicioID, numeroDoc, loadDetaExist, recursoTimeInd);

                    if (!loadDetaExist)
                    {
                        foreach (DTO_pyProyectoDeta d in result)
                        {
                            d.Cantidad.Value = 1;
                            d.CantSolicitud.Value = 1;
                        }
                    }

                    foreach (DTO_pyProyectoDeta rec in result.FindAll(x => x.CostoLocal.Value == 0 && x.CostoExtra.Value != 0))
                        rec.CostoLocal.Value = tasaCambio * rec.CostoExtra.Value; 
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyProyectoDeta_GetByParameter");
                throw exception;
            }
        }
        
        ///<summary>
        /// Carga la Solicitud de Servicio
        /// </summary>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="claseServicioID">clase servicio</param>
        /// <param name="existe">si existe la solicitud</param>
        /// <returns>detalle solicitud</returns>
        public int pyProyectoDeta_Add(DTO_pyProyectoDeta deta)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                this._dal_pyProyectoDeta = (DAL_pyProyectoDeta)this.GetInstance(typeof(DAL_pyProyectoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_pyProyectoDeta.DAL_pyProyectoDeta_Add(deta);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyProyectoDeta_Add");
                return 0;
            }
        }

        #endregion

        #endregion

        #region Planeacion Compras

        /// <summary>
        /// Obtiene los recursos pendientes para  crear una Solicitud de Orden de Compra
        /// </summary>
        /// <returns>Lista de</returns>
        public List<DTO_pyProyectoMvto> CompraRecursos_GetPendientesForApprove(DateTime fechaTope)
        {
            try
            {
                this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)this.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return  this._dal_pyProyectoMvto.DAL_pyProyectoMvto_GetPendientesForAprrove(fechaTope);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "CompraRecursos_GetPendientesForApprove");
                throw exception;
            }
        }

        /// <summary>
        /// Aprueba los recursos para  crear una Solicitud de Orden de Compra
        /// </summary>
        /// <param name="documentID">documento actual</param>
        /// <param name="listMvtos">Lista de recursos a aprobar</param>
        /// <returns>Result</returns>
        public DTO_SerializedObject CompraRecursos_ApproveSolicitudOC(int documentID, List<DTO_pyProyectoMvto> listMvtos, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_SerializedObject resultModOProveedor = new DTO_SerializedObject();
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            result.Result = ResultValue.OK;
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.line = 1;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;           

             List<DTO_glDocumentoControl> ctrlSolicitudes = new List<DTO_glDocumentoControl>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            try
            {
                this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)this.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloProveedores = (ModuloProveedores)this.GetInstance(typeof(ModuloProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string periodoProv = this.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_Periodo);
                string mdaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);

                DTO_glDocumentoControl ctrlSol = new DTO_glDocumentoControl();
                DTO_prSolicitudDocu headerSol = new DTO_prSolicitudDocu();
                List<DTO_prSolicitudFooter> footerSol = new List<DTO_prSolicitudFooter>();

                List<int> numDocDistinct = listMvtos.Select(x => x.NumeroDoc.Value.Value).Distinct().ToList();
                foreach (int numDoc in numDocDistinct)
                {   
                    int numeroDoc = 0;
                    List<DTO_pyProyectoMvto> mvtosByNumDoc = listMvtos.FindAll(x => x.NumeroDoc.Value.Value == numDoc).ToList();
                    if (mvtosByNumDoc.Count > 0)
                    {
                        DTO_SolicitudTrabajo proyecto = this.SolicitudProyecto_Load(AppDocuments.Proyecto, string.Empty, null, mvtosByNumDoc.First().NumeroDoc.Value, string.Empty, string.Empty, false, false, false, false);
                        #region Asigna el Doc Control Sol
                        ctrlSol = new DTO_glDocumentoControl();
                        ctrlSol.EmpresaID.Value = this.Empresa.ID.Value;
                        ctrlSol.DocumentoID.Value = AppDocuments.Solicitud;
                        ctrlSol.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                        ctrlSol.Fecha.Value = DateTime.Now;
                        ctrlSol.PeriodoDoc.Value = Convert.ToDateTime(periodoProv);
                        ctrlSol.PeriodoUltMov.Value = Convert.ToDateTime(periodoProv);
                        ctrlSol.FechaDoc.Value = Convert.ToDateTime(periodoProv);
                        ctrlSol.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                        //ctrlSol.Observacion.Value = observaciones;
                        ctrlSol.PrefijoID.Value = proyecto.DocCtrl.PrefijoID.Value;
                        ctrlSol.MonedaID.Value = mdaLocal;
                        ctrlSol.TasaCambioCONT.Value = 0;
                        ctrlSol.TasaCambioDOCU.Value = 0;
                        ctrlSol.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                        ctrlSol.ProyectoID.Value = proyecto.DocCtrl.ProyectoID.Value;
                        ctrlSol.CentroCostoID.Value = proyecto.DocCtrl.CentroCostoID.Value;
                        ctrlSol.LugarGeograficoID.Value = proyecto.DocCtrl.LugarGeograficoID.Value;
                        ctrlSol.seUsuarioID.Value = this.UserId;
                        ctrlSol.NumeroDoc.Value = 0;
                        ctrlSol.DocumentoNro.Value = 0;
                        ctrlSol.ComprobanteIDNro.Value = 0;
                        ctrlSol.Descripcion.Value = "Creacion Solicitud OC de Proveedores";
                        ctrlSol.Observacion.Value = "Solicitud Orden de Compra del Proyecto " + proyecto.DocCtrl.PrefDoc.Value;
                        ctrlSol.Valor.Value = mvtosByNumDoc.Sum(x => x.CostoLocalTOT.Value);
                        ctrlSol.Iva.Value = 0;
                        #endregion
                        #region Asigna el Header(prSolicitudDocu)
                        DTO_coProyecto proy = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, proyecto.DocCtrl.ProyectoID.Value, true, false);
                        headerSol.LugarEntrega.Value = proy.LocFisicaID.Value;
                        headerSol.FechaEntrega.Value = proyecto.HeaderProyecto.FechaInicio.Value;
                        headerSol.AreaAprobacion.Value = this.GetAreaFuncionalByUser();
                        headerSol.Prioridad.Value = 1; // Normal
                        headerSol.UsuarioSolicita.Value = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId).ID.Value;
                        headerSol.Destino.Value = 0; //Orden Compra
                        headerSol.Ano.Value = proyecto.HeaderProyecto.FechaInicio.Value.Value.Year;
                        #endregion
                        #region Asigna el Detalle Sol y Cargos
                        foreach (DTO_pyProyectoMvto mvto in mvtosByNumDoc)
                        {
                            DTO_prSolicitudFooter det = new DTO_prSolicitudFooter();
                            det.DetalleDocu = new DTO_prDetalleDocu();
                            det.DetalleDocu.CodigoBSID.Value = mvto.CodigoBSID.Value;
                            det.DetalleDocu.inReferenciaID.Value = mvto.inReferenciaID.Value;
                            det.DetalleDocu.Descriptivo.Value = mvto.RecursoDesc.Value;
                            det.DetalleDocu.EstadoInv.Value = 1;
                            det.DetalleDocu.MonedaID.Value = mdaLocal;
                            det.DetalleDocu.UnidadInvID.Value = mvto.UnidadInvID.Value;
                            det.DetalleDocu.CantidadSol.Value = mvto.CantidadSOL.Value;
                            det.DetalleDocu.Documento4ID.Value = mvto.NumeroDoc.Value;
                            det.DetalleDocu.Detalle4ID.Value = mvto.Consecutivo.Value;
                            det.DetalleDocu.CantidadDoc4.Value = mvto.CantidadSOL.Value;
                            det.DetalleDocu.DatoAdd4.Value = "(" + mvto.TareaCliente.Value + ")";
                            det.DetalleDocu.LineaPresupuestoID.Value = proyecto.DetalleProyecto.Find(x => x.TareaCliente.Value == mvto.TareaCliente.Value).LineaPresupuestoID.Value;
                            if (!string.IsNullOrEmpty(mvto.inReferenciaID.Value))
                            {
                                DTO_inReferencia refer = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, mvto.inReferenciaID.Value, true, false);
                                DTO_inRefTipo refTipo = (DTO_inRefTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefTipo, refer.TipoInvID.Value, true, false);
                                det.DetalleDocu.DiasEntrega.Value = Convert.ToByte(refTipo.DiasCompra.Value);
                                det.DetalleDocu.EmpaqueInvID.Value = refer.EmpaqueInvID.Value;
                            }
                            else
                                det.DetalleDocu.DiasEntrega.Value = 0;

                            det.DetalleDocu.OrigenMonetario.Value = mvto.CostoExtra.Value != 0 && mvto.CostoExtra.Value != null ? (byte)TipoMoneda_LocExt.Foreign : (byte)TipoMoneda_LocExt.Local;

                            //Asigna Cargos de la Solicitud
                            DTO_prSolicitudCargos cargo = new DTO_prSolicitudCargos();
                            cargo.ProyectoID.Value = proyecto.DocCtrl.ProyectoID.Value;
                            cargo.CentroCostoID.Value = proyecto.DocCtrl.CentroCostoID.Value;
                            cargo.LineaPresupuestoID.Value = det.DetalleDocu.LineaPresupuestoID.Value;
                            cargo.PorcentajeID.Value = 100;
                            det.SolicitudCargos.Add(cargo);
                            footerSol.Add(det);
                        }
                        #endregion

                        resultModOProveedor = this._moduloProveedores.Solicitud_Guardar(AppDocuments.Solicitud, ctrlSol, headerSol, null, footerSol, false, out numeroDoc, batchProgress, true);

                        if (resultModOProveedor.GetType() == typeof(DTO_TxResult))
                        {
                            DTO_TxResult res = (DTO_TxResult)resultModOProveedor;
                            if (res.Result == ResultValue.NOK)
                            {
                                result.ResultMessage = res.ResultMessage;
                                result.Result = ResultValue.NOK;
                                return result;
                            }
                        }
                        else if (numeroDoc != 0)
                        {
                            #region Actualiza pyProyectoMvto con la solicitud de Ord Compra
                            ctrlSol.NumeroDoc.Value = numeroDoc;
                            ctrlSolicitudes.Add(ObjectCopier.Clone(ctrlSol));
                            foreach (DTO_pyProyectoMvto mvtoPadre in mvtosByNumDoc)
                            {
                                foreach (DTO_pyProyectoMvto mvtoHij in mvtoPadre.DetalleTareas)
                                {
                                    mvtoHij.CantidadPROV.Value = mvtoHij.CantidadPROV.Value + mvtoHij.CantidadSOL.Value;
                                    mvtoHij.CantidadSOL.Value = 0;
                                    this._dal_pyProyectoMvto.DAL_pyProyectoMvto_Upd(mvtoHij);
                                }
                            }
                            #endregion
                            #region Aprueba la solicitud de Ord Compra

                            DTO_glDocumentoAprueba docAprueba = this._moduloGlobal.glDocumentoAprueba_UpdateUserApprover(numeroDoc);
                            if (docAprueba == null)
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_UpdateData;
                            }
                            //Si UsuarioAprueba es null realiza el proceso de Aprobacion final
                            else if (docAprueba.UsuarioAprueba.Value == null)
                            {
                                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, numeroDoc, EstadoDocControl.Aprobado, string.Empty, true);

                                #region Genera el registro en prSaldosDocu
                                this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                               // this._dal_prSaldosDocu = (DAL_prSaldosDocu)this.GetInstance(typeof(DAL_prSaldosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                                //foreach (DTO_prSolicitudFooter det in footerSol)
                                //{
                                //    DTO_prSaldosDocu saldo = new DTO_prSaldosDocu();
                                //    saldo.EmpresaID.Value = this.Empresa.ID.Value;
                                //    saldo.NumeroDoc.Value = numeroDoc;
                                //    saldo.ConsecutivoDetaID.Value = det.DetalleDocu.ConsecutivoDetaID.Value;
                                //    saldo.CantidadDocu.Value = det.DetalleDocu.CantidadSol.Value;
                                //    saldo.CantidadMovi.Value = 0;
                                //    this._dal_prSaldosDocu.DAL_prSaldosDocu_Add(saldo);
                                //}
                                #endregion
                                List<string> act = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.SolicitudAprob);
                                if (act.Count > 0)
                                    this.AsignarFlujo(documentID, numeroDoc, act[0], false, string.Empty);
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "pyPreProyectoMvto_Upd");
                throw ex;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloProveedores._mySqlConnectionTx = null;

                        for (int i = 0; i < ctrlSolicitudes.Count; ++i)
                        {
                            DTO_glDocumentoControl ctrl = ctrlSolicitudes[i];
                            ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(ctrl.DocumentoID.Value.Value, ctrl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(ctrl, true, false, false);
                        } 
                    }
                }
                else if (base._mySqlConnectionTx != null)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region pyProyectoMvto

        /// <summary>
        /// Actualiza la tabla de mvtos
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>Detalle Documento</returns>
        public DTO_SerializedObject pyProyectoMvto_Upd(int documentID, List<DTO_pyProyectoMvto> listMvtos, bool sendToSolicitudInd, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            result.Result = ResultValue.OK;
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.line = 1;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            try
            {
                this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)this.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                foreach (DTO_pyProyectoMvto mvto in listMvtos.FindAll(x => x.CantidadTOT.Value != x.CantidadPROV.Value))
                   this._dal_pyProyectoMvto.DAL_pyProyectoMvto_Upd(mvto);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "pyPreProyectoMvto_Upd");
                throw ex;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        base._mySqlConnectionTx = null;
                    }
                }
                else if (base._mySqlConnectionTx != null)
                    base._mySqlConnectionTx.Rollback();
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
                this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)this.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_pyProyectoMvto.DAL_pyProyectoMvto_GetDocsAnexo(consProyMvto);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyProyectoMvto_GetDocsAnexo");
                throw exception;
            }
        }

        /// <summary>
        /// Guarda en la tabla de mvtos
        /// </summary>
        /// <param name="numDocProy">numero Documento</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult pyProyectoMvto_Add(int numDocProy, DTO_pyProyectoDocu proyectoDocu,DTO_pyProyectoTarea tarea, DTO_pyProyectoDeta det, int consDeta)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)this.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string diasOC = this.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasRequeridosOrdCompra);
                string diasOCImport = this.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasRequeridosOrdCompraImportac);
                string codigoBSInventarios = this.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_CodigoBSCompraInv);

                int diasOrdCompraEmp = 0;
                int diasOrdCompraImport = 0;
                if (!string.IsNullOrEmpty(diasOC))
                    diasOrdCompraEmp = Convert.ToInt32(diasOC);

                #region Guarda en pyProyectoMvto
                DTO_pyProyectoMvto mvto = new DTO_pyProyectoMvto();
                mvto.EmpresaID.Value = this.Empresa.ID.Value;
                mvto.NumeroDoc.Value = numDocProy;
                mvto.ConsecDeta.Value = consDeta;
                mvto.Version.Value = proyectoDocu.Version.Value;
                mvto.TipoMvto.Value = 1;// Presupuesto
                mvto.NumProyecto.Value = numDocProy;
                mvto.FactorID.Value = det.FactorID.Value;
                mvto.CostoLocal.Value = det.CostoLocal.Value;
                mvto.CostoExtra.Value = det.CostoExtra.Value;
                DTO_inUnidad unidad = (DTO_inUnidad)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inUnidad, det.UnidadInvID.Value, true, false);
                int decimalAprox = unidad != null && unidad.AproximaEnteroInd.Value.Value ? 0 : 2;
                if (det.TipoRecurso.Value == (byte)TipoRecurso.Equipo && !proyectoDocu.EquipoCantidadInd.Value.Value)
                    mvto.CantidadTOT.Value = det.FactorID.Value != 0 ? Math.Round(tarea.Cantidad.Value.Value / det.FactorID.Value.Value, decimalAprox) : 0;
                if (det.TipoRecurso.Value == (byte)TipoRecurso.Equipo && proyectoDocu.EquipoCantidadInd.Value.Value)
                    mvto.CantidadTOT.Value = det.FactorID.Value != 0 ? Math.Round(tarea.Cantidad.Value.Value * det.FactorID.Value.Value, decimalAprox) : 0;

                if (det.TipoRecurso.Value == (byte)TipoRecurso.Personal && !proyectoDocu.PersonalCantidadInd.Value.Value)
                    mvto.CantidadTOT.Value = det.FactorID.Value != 0 ? Math.Round(tarea.Cantidad.Value.Value / det.FactorID.Value.Value, decimalAprox) : 0;
                if (det.TipoRecurso.Value == (byte)TipoRecurso.Personal && proyectoDocu.PersonalCantidadInd.Value.Value)
                    mvto.CantidadTOT.Value = det.FactorID.Value != 0 ? Math.Round(tarea.Cantidad.Value.Value * det.FactorID.Value.Value, decimalAprox) : 0;
                else
                    mvto.CantidadTOT.Value = Math.Round(tarea.Cantidad.Value.Value * det.FactorID.Value.Value, decimalAprox);
                mvto.CantidadSOL.Value = 0;
                mvto.CantidadNOM.Value = 0;
                mvto.CantidadACT.Value = 0;
                mvto.CantidadINV.Value = 0;
                mvto.CantidadBOD.Value = 0;
                mvto.CantidadREC.Value = 0;
                mvto.CantidadPROV.Value = 0;
                mvto.CostoLocalTOT.Value = det.CostoLocalTOT.Value;
                mvto.CostoExtraTOT.Value = det.CostoExtraTOT.Value;
                mvto.FechaInicioTarea.Value = tarea.FechaInicio.Value;
                DTO_pyRecurso rec = (DTO_pyRecurso)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyRecurso, det.RecursoID.Value, true, false);
                #region Calcula Fecha Compra
                int diasCompraByTipoRef = 0;
                if (rec != null)
                {
                    #region Obtiene Dias Compra por Tipo Ref
                    DTO_inReferencia refer = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, rec.inReferenciaID.Value, true, false);
                    if (refer != null)
                    {
                        DTO_inRefTipo tipoRef = (DTO_inRefTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefTipo, refer.TipoInvID.Value, true, false);
                        diasCompraByTipoRef = tipoRef.DiasCompra.Value.Value;
                    }
                    #endregion
                    #region Obtiene la Codigo de BS
                    if (rec.TipoRecurso.Value == (byte)TipoRecurso.Insumo)//|| rec.TipoRecurso.Value == (byte)TipoRecurso.Equipo)
                        mvto.CodigoBSID.Value = codigoBSInventarios;
                    else
                    {
                        if (string.IsNullOrEmpty(rec.CodigoBSID.Value))
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = "Debe Parametrizar el CodigoBS del recurso " + rec.ID.Value;
                            return result;
                        }
                        else
                            mvto.CodigoBSID.Value = rec.CodigoBSID.Value;
                    }

                    #endregion
                    #region Obtiene la linea Presupuesto
                    DTO_MasterBasic dtoLinea = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, tarea.CapituloTareaID.Value, true, false);
                    if (dtoLinea != null)
                        mvto.LineaPresupuestoID.Value = tarea.CapituloTareaID.Value;
                    else
                    {
                        DTO_prBienServicio bs = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prBienServicio, mvto.CodigoBSID.Value, true, false);
                        if (bs != null)
                        {
                            DTO_glBienServicioClase bsClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bs.ClaseBSID.Value, true, false);
                            mvto.LineaPresupuestoID.Value = bsClase.LineaPresupuestoID.Value;
                        }
                    }

                    #endregion
                }

                #region Obtiene los dias de importacion si tiene Mda Extanjera
                if (!string.IsNullOrEmpty(diasOCImport) && det.CostoExtra.Value != 0 && det.CostoExtra.Value != null)
                    diasOrdCompraImport = Convert.ToInt32(diasOCImport);
                else
                    diasOrdCompraImport = 0;

                #endregion
                if (mvto.FechaInicioTarea.Value != null)
                    mvto.FechaOrdCompra.Value = mvto.FechaInicioTarea.Value.Value.AddDays(-(diasCompraByTipoRef + diasOrdCompraEmp + diasOrdCompraImport));
                #endregion
                mvto.FechaRecibido.Value = mvto.FechaInicioTarea.Value;
                mvto.Observaciones.Value = det.Observaciones.Value;
              
                #endregion
                result.ExtraField = this._dal_pyProyectoMvto.DAL_pyProyectoMvto_Add(mvto).ToString();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyProyectoMvto_Add");
                result.Result = ResultValue.NOK;
                result.ResultMessage = exception.Message;
                return result;
            }
        }

        ///<summary>
        /// Actualiza la tabla de mvtos
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>Detalle Documento</returns>
        internal List<DTO_pyProyectoMvto> pyProyectoMvto_GetParameter(DTO_pyProyectoMvto filter)
        {
            try
            {
                this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)this.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_pyProyectoMvto.DAL_pyProyectoMvto_GetByParameter(filter);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyProyectoMvto_GetDocsAnexo");
                throw exception;
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
        public DTO_SerializedObject ActaTrabajo_ApproveRecibidoBS(int documentID, DTO_SolicitudTrabajo proyecto, DTO_glDocumentoControl ctrlActa, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_SerializedObject resultModOProveedor = new DTO_SerializedObject();
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            result.Result = ResultValue.OK;
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.line = 1;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            DTO_Alarma alarma = null;

            List<DTO_glDocumentoControl> ctrlRecibidos = new List<DTO_glDocumentoControl>();
            proyecto.ActaTrabajosDeta = proyecto.ActaTrabajosDeta.FindAll(x => x.CantidadREC.Value != 0);
            
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            try
            {
                this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)this.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyActaTrabajoDet = (DAL_pyActaTrabajoDeta)this.GetInstance(typeof(DAL_pyActaTrabajoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloProveedores = (ModuloProveedores)this.GetInstance(typeof(ModuloProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string periodoProv = this.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_Periodo);
                string mdaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);

        
                List<string> proveedorDistinct = proyecto.ActaTrabajosDeta.Select(x => x.ProveedorID.Value).Distinct().ToList();
                foreach (string prov in proveedorDistinct)
                {
                    DTO_glDocumentoControl ctrlRec = new DTO_glDocumentoControl();
                    DTO_prRecibidoDocu headerRec = new DTO_prRecibidoDocu();
                    List<DTO_prOrdenCompraResumen> footerRec = new List<DTO_prOrdenCompraResumen>();

                    int numeroDocRec = 0;
                    List<DTO_pyActaTrabajoDeta> actasByProv =  proyecto.ActaTrabajosDeta.FindAll(x => x.ProveedorID.Value == prov).ToList();
                    #region Asigna el Doc Control Recib
                    ctrlRec = new DTO_glDocumentoControl();
                    ctrlRec.EmpresaID.Value = this.Empresa.ID.Value;
                    ctrlRec.DocumentoID.Value = AppDocuments.Recibido;
                    ctrlRec.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    ctrlRec.Fecha.Value = DateTime.Now;
                    ctrlRec.PeriodoDoc.Value = Convert.ToDateTime(periodoProv);
                    ctrlRec.PeriodoUltMov.Value = Convert.ToDateTime(periodoProv);
                    ctrlRec.FechaDoc.Value = ctrlActa.FechaDoc.Value;
                    ctrlRec.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                    ctrlRec.PrefijoID.Value = proyecto.DocCtrl.PrefijoID.Value;
                    ctrlRec.MonedaID.Value = mdaLocal;
                    ctrlRec.TasaCambioCONT.Value = 0;
                    ctrlRec.TasaCambioDOCU.Value = 0;
                    ctrlRec.Estado.Value = (byte)EstadoDocControl.Aprobado;
                    ctrlRec.ProyectoID.Value = proyecto.DocCtrl.ProyectoID.Value;
                    ctrlRec.CentroCostoID.Value = proyecto.DocCtrl.CentroCostoID.Value;
                    ctrlRec.LugarGeograficoID.Value = proyecto.DocCtrl.LugarGeograficoID.Value;
                    ctrlRec.seUsuarioID.Value = this.UserId;
                    ctrlRec.NumeroDoc.Value = 0;
                    ctrlRec.DocumentoNro.Value = 0;
                    ctrlRec.ComprobanteIDNro.Value = 0;
                    ctrlRec.Descripcion.Value = "Creacion Recibido Proveedores(Acta Trabajo)";
                    ctrlRec.Observacion.Value = "Recibido Bienes y Servicios del Proyecto " + proyecto.DocCtrl.PrefDoc.Value+ proyecto.DocCtrl.DocumentoNro.Value.ToString();
                    ctrlRec.Valor.Value = 0;
                    ctrlRec.Iva.Value = 0;
                    DTO_prProveedor proveedor = (DTO_prProveedor)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, prov, true, false);
                    ctrlRec.TerceroID.Value = proveedor != null? proveedor.TerceroID.Value : string.Empty;
                    #endregion
                    #region Asigna el Header(prRecibidoDocu)
                    DTO_coProyecto proy = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, proyecto.DocCtrl.ProyectoID.Value, true, false);
                    headerRec.ProveedorID.Value = prov;
                    headerRec.LugarEntrega.Value = proy.LocFisicaID.Value;
                    headerRec.ConformidadInd.Value = true;
                    #endregion
                    #region Asigna el Detalle Recibido
                    foreach (DTO_pyActaTrabajoDeta acta in actasByProv)
                    {
                        DTO_prOrdenCompraResumen resumen = new DTO_prOrdenCompraResumen();
                        resumen.OrdCompraDetaID.Value = acta.ConsOrdCompraDeta.Value;
                        resumen.CantidadRec.Value = acta.CantidadREC.Value;
                        resumen.MonedaIDOC.Value = mdaLocal;
                        resumen.MonedaPagoOC.Value = mdaLocal;
                        footerRec.Add(resumen);
                    }
                    #endregion

                    resultModOProveedor = this._moduloProveedores.Recibido_Guardar(AppDocuments.Recibido, ctrlRec, headerRec,footerRec, out numeroDocRec,string.Empty,string.Empty, batchProgress, true);

                    if (resultModOProveedor.GetType() == typeof(DTO_TxResult))
                    {
                        DTO_TxResult res = (DTO_TxResult)resultModOProveedor;
                        if (res.Result == ResultValue.NOK)
                        {
                            result.ResultMessage = res.ResultMessage;
                            result.Result = ResultValue.NOK;
                            return result;
                        }
                    }
                    else if (numeroDocRec != 0)
                    {
                        ctrlRec.NumeroDoc.Value = numeroDocRec;
                        ctrlRecibidos.Add(ObjectCopier.Clone(ctrlRec));
                        #region Actualiza las Cantidades en los movimientos del Proyecto(pyProyectoMvto)
                        foreach (DTO_pyActaTrabajoDeta acta in proyecto.ActaTrabajosDeta)
                        {
                            DTO_pyProyectoMvto mvtoProy = this._dal_pyProyectoMvto.DAL_pyProyectoMvto_GetByConsecutivo(acta.ConsProyMvto.Value);
                            mvtoProy.CantidadREC.Value = mvtoProy.CantidadREC.Value + acta.CantidadREC.Value;
                            this._dal_pyProyectoMvto.DAL_pyProyectoMvto_Upd(mvtoProy);
                        }
                        #endregion                                               
                        #region Actualiza el las actas  con el numero del Recibido(pyActaTrabajoDeta)
                        foreach (DTO_pyActaTrabajoDeta acta in proyecto.ActaTrabajosDeta)
                        {
                            DTO_pyActaTrabajoDeta actaProy = this._dal_pyActaTrabajoDet.DAL_pyActaTrabajoDeta_GetByConsecutivo(acta.Consecutivo.Value.Value);
                            actaProy.NumDocRecibido.Value = numeroDocRec;
                            this._dal_pyActaTrabajoDet.DAL_pyActaTabajoDeta_Upd(actaProy);
                        }
                        #endregion  
                        #region Aprueba el Doc de Acta de Trabajo
                        this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, actasByProv.First().NumeroDoc.Value.Value, EstadoDocControl.Aprobado, string.Empty, true);
                        #endregion
                        List<string> act = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.RecibidoAprob);
                        if (act.Count > 0)
                            this.AsignarFlujo(documentID, numeroDocRec, act[0], false, string.Empty);
                    }
                }
                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    //Trae la info de la alarma
                    alarma = this.GetFirstMailInfo(ctrlRecibidos[0].NumeroDoc.Value.Value, true);
                    alarma.NumeroDoc = ctrlRecibidos[0].NumeroDoc.Value.ToString();
                    alarma.PrefijoID = ctrlRecibidos[0].PrefijoID.Value.TrimEnd();
                    alarma.Consecutivo = ctrlRecibidos[0].DocumentoNro.Value.ToString();
                    return alarma;
                    #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "pyPreProyectoMvto_Upd");
                throw ex;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloProveedores._mySqlConnectionTx = null;

                        for (int i = 0; i < ctrlRecibidos.Count; ++i)
                        {
                            DTO_glDocumentoControl ctrl = ctrlRecibidos[i];
                            ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(ctrl.DocumentoID.Value.Value, ctrl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(ctrl, true, false, false);
                        } 
                    }

                }
                else if (base._mySqlConnectionTx != null)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Agrega un docum de Acta de Trabajo
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="docCtrl">doc control</param>
        /// <param name="actasList">lista de actas</param>
        /// <returns></returns>
        public DTO_SerializedObject ActaTrabajo_Add(int documentoID, DTO_glDocumentoControl docCtrl, List<DTO_pyActaTrabajoDeta> actasList, bool update, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            DTO_Alarma alarma = null;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyActaTrabajoDet= (DAL_pyActaTrabajoDeta)this.GetInstance(typeof(DAL_pyActaTrabajoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)this.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int numDoc = 0;

                if (!update)
                {                  
                    docCtrl.NumeroDoc.Value = 0;
                    DTO_TxResultDetail resultGLDC = new DTO_TxResultDetail();
                    resultGLDC = this._moduloGlobal.glDocumentoControl_Add(AppDocuments.ActaTrabajo, docCtrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_AddDocument;
                        return result;
                    }

                    numDoc = Convert.ToInt32(resultGLDC.Key);
                    docCtrl.NumeroDoc.Value = numDoc;              

                    #region Guarda en pyActaTrabajoDeta

                    foreach (DTO_pyActaTrabajoDeta acta in actasList.FindAll(x=>x.CantidadREC.Value > 0))
                    {
                        acta.NumeroDoc.Value = numDoc;
                        this._dal_pyActaTrabajoDet.DAL_pyActaTrabajoDeta_Add(acta);
                    }
                    #endregion                    
                }
                else
                {   
                    #region Actualiza en pyActaTrabajoDeta

                    foreach (DTO_pyActaTrabajoDeta acta in actasList.FindAll(x => x.CantidadREC.Value > 0))
                    {
                       DTO_pyActaTrabajoDeta exist = this._dal_pyActaTrabajoDet.DAL_pyActaTrabajoDeta_GetByConsecutivo(acta.Consecutivo.Value);
                        if(exist != null)
                            this._dal_pyActaTrabajoDet.DAL_pyActaTabajoDeta_Upd(acta);
                        else
                              this._dal_pyActaTrabajoDet.DAL_pyActaTrabajoDeta_Add(acta);
                    }
                    #endregion
                }

                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    //Trae la info de la alarma
                    alarma = this.GetFirstMailInfo(numDoc, true);
                    alarma.NumeroDoc = numDoc.ToString();
                    alarma.PrefijoID = docCtrl.PrefijoID.Value.TrimEnd();
                    alarma.Consecutivo = docCtrl.DocumentoNro.Value.ToString();
                    return alarma;
                    #endregion
                }

                return result;
            }
            catch (Exception ex)
            {
                docCtrl.NumeroDoc.Value = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ActaTrabajo_Add");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        if (docCtrl.DocumentoNro.Value == 0)
                        {
                            docCtrl.DocumentoNro.Value = Convert.ToInt32(this.GenerarDocumentoNro(documentoID, docCtrl.PrefijoID.Value));
                            this._moduloGlobal.ActualizaConsecutivos(docCtrl, true, false, false);
                        }
                        result.ExtraField = docCtrl.DocumentoNro.Value.ToString();
                        alarma.Consecutivo = docCtrl.DocumentoNro.Value.ToString();
                    }
                }
                else if (base._mySqlConnectionTx != null)
                {
                    docCtrl.NumeroDoc.Value = 0;
                    base._mySqlConnectionTx.Rollback();
                }

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
                this._dal_pyActaTrabajoDet = (DAL_pyActaTrabajoDeta)this.GetInstance(typeof(DAL_pyActaTrabajoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_pyActaTrabajoDeta> actas = new List<DTO_pyActaTrabajoDeta>();

                //Carga las actas de Trabajo realizadas
                DTO_pyActaTrabajoDeta filterActa = new DTO_pyActaTrabajoDeta();
                filterActa.NumeroDoc.Value = numeroDoc;
                actas = this._dal_pyActaTrabajoDet.DAL_pyActaTrabajoDeta_GetByParameter(filterActa);

                return actas;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ActaTrabajo_Load");
                throw exception;
            }
        }

        #endregion

        #region Otras 

        /// <summary>
        /// Trae los documentos para aprobación pendientes 
        /// </summary>
        /// <param name="documentoID">documentoID</param>
        /// <param name="actividadFLujoId">actividadFlujoID</param>
        /// <returns></returns>
        public List<DTO_TareasFilter> TareasFilter_Get(DTO_TareasFilter filter)
        {
            try
            {
                List<DTO_TareasFilter> results = new List<DTO_TareasFilter>();
                this._dal_pyPreProyectoTarea = (DAL_pyPreProyectoTarea)this.GetInstance(typeof(DAL_pyPreProyectoTarea), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                var data = this._dal_pyPreProyectoTarea.DAL_TareasFilter_Get(filter);

                List<string> tareas = data.Select(x => x.TareaID.Value).Distinct().ToList();
                foreach (string t in tareas)
                {
                    DTO_TareasFilter res = new DTO_TareasFilter();
                    res.Detalle = data.FindAll(x => x.TareaID.Value == t).ToList();
                    res.TareaID.Value = t;
                    res.TareaDesc.Value = data.Find(x => x.TareaID.Value == t).TareaDesc.Value;
                    res.UnidadInvID.Value = data.Find(x => x.TareaID.Value == t).UnidadInvID.Value;
                    results.Add(res);
                }

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_TareasFilter_Get");
                throw exception;
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
        /// <param name="tareasProy">tareas del proyecto</param>
        /// <param name="tareasAdicProy">tareas adicionales del proyecto</param>
        /// <returns></returns>
        public DTO_TxResult EntregasCliente_Add(int documentoID, int numDocProy, List<DTO_pyProyectoTareaCliente> tareasCliente, List<DTO_pyProyectoTarea> tareasProy, List<DTO_pyProyectoTarea> tareasAdicProy,List<int> entregasDelete)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_pyProyectoTarea = (DAL_pyProyectoTarea)this.GetInstance(typeof(DAL_pyProyectoTarea), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyTareaCliente = (DAL_pyProyectoTareaCliente)this.GetInstance(typeof(DAL_pyProyectoTareaCliente), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyPlanEntrega = (DAL_pyProyectoPlanEntrega)this.GetInstance(typeof(DAL_pyProyectoPlanEntrega), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Agrega Tareas Cliente
                foreach (DTO_pyProyectoTareaCliente tarea in tareasCliente)
                {
                    //Valida si la tarea existe
                    bool exist = this._dal_pyProyTareaCliente.DAL_pyProyectoTareaCliente_Exist(tarea.Consecutivo.Value);
                    if (exist)
                    {
                        #region Actualiza (pyPreProyectoTareaCliente)
                        this._dal_pyProyTareaCliente.DAL_pyProyectoTareaCliente_Upd(tarea);
                        #region Elimina las entregas solicitadas
                        if (entregasDelete != null)
                            foreach (int r in entregasDelete)
                                this._dal_pyProyPlanEntrega.DAL_pyProyectoPlanEntrega_DeleteByConsecutivo(r);
                        #endregion
                        foreach (DTO_pyProyectoPlanEntrega entrega in tarea.Detalle)
                        {
                            //Valida si existe
                            bool existPlan = this._dal_pyProyPlanEntrega.DAL_pyProyectoPlanEntrega_Exist(entrega.Consecutivo.Value);
                            if (existPlan)
                            {
                                #region Actualiza (pyPreProyectoPlanEntrega)
                                this._dal_pyProyPlanEntrega.DAL_pyProyectoPlanEntrega_Upd(entrega);
                                #endregion
                            }
                            else
                            {
                                #region Agrega nuevo (pyPreProyectoPlanEntrega)
                                entrega.ConsecTarea.Value = tarea.Consecutivo.Value;
                                this._dal_pyProyPlanEntrega.DAL_pyProyectoPlanEntrega_Add(entrega);
                                #endregion
                            }

                            //Actualiza las tareas con el consecutivo del plan entrega (ConsecEntrega)
                            foreach (DTO_pyProyectoTarea tar in entrega.DetalleTareas)
                            {
                                #region Actualiza (pyPreProyectoTarea)
                                tar.ConsEntrega.Value = entrega.Consecutivo.Value;
                                this._dal_pyProyectoTarea.DAL_pyProyectoTarea_UpdEntregable(tar);
                                #endregion
                            }
                        }
                        #endregion                       
                    }
                    else
                    {
                        #region Agrega nuevo (pyPreProyectoTareaCliente)
                        this._dal_pyProyTareaCliente.DAL_pyProyectoTareaCliente_DeleteByNumeroDoc(tarea.NumeroDoc.Value.Value,tarea.TareaEntregable.Value); 
                        this._dal_pyProyTareaCliente.DAL_pyProyectoTareaCliente_Add(tarea);                        
                        foreach (DTO_pyProyectoPlanEntrega entrega in tarea.Detalle)
                        {                            
                            #region Agrega nuevo (pyPreProyectoPlanEntrega)
                            entrega.ConsecTarea.Value = tarea.Consecutivo.Value;
                            this._dal_pyProyPlanEntrega.DAL_pyProyectoPlanEntrega_Add(entrega);
                            #endregion
                        }
                        #endregion
                    }
                }                
                #endregion                
                #region Actualiza Tareas Proyecto(TareaEntregable)
                if (tareasProy != null)
                {
                    foreach (DTO_pyProyectoTarea tarea in tareasProy)
                    {
                        #region Actualiza (pyPreProyectoTarea)
                        this._dal_pyProyectoTarea.DAL_pyProyectoTarea_UpdEntregable(tarea);
                        #endregion

                    } 
                }
                #endregion
                #region Actualiza Tareas Proyecto Adic
                if (tareasAdicProy != null)
                {
                    foreach (DTO_pyProyectoTarea tarea in tareasAdicProy)
                    {
                        #region Actualiza (pyPreProyectoTarea)
                        this._dal_pyProyectoTarea.DAL_pyProyectoTarea_UpdEntregable(tarea);
                        #endregion

                    } 
                }
                #endregion
                return result;
            }
            catch (Exception ex)
            {
                numDocProy = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "EntregasCliente_Add");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    base._mySqlConnectionTx.Commit();
                    base._mySqlConnectionTx = null;
                }
                else if (base._mySqlConnectionTx != null)
                {
                    base._mySqlConnectionTx.Rollback();
                }

            }
        }

        /// <summary>
        /// Trae las tareas Cliente asignadas a un proyecto
        /// </summary>
        /// <param name="numDocProy">identificador del proyecto</param>
        /// <param name="tareaCliente">tarea Cliente</param>
        /// <param name="desc">descripcion de la tarea</param>
        /// <returns></returns>
        public List<DTO_pyProyectoTareaCliente> pyProyectoTareaCliente_GetByNumeroDoc(int numDocProy,string tareaCliente,string desc)
        {
            try
            {
                List<DTO_pyProyectoTareaCliente> result = new List<DTO_pyProyectoTareaCliente>();
                this._dal_pyProyTareaCliente = (DAL_pyProyectoTareaCliente)this.GetInstance(typeof(DAL_pyProyectoTareaCliente), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyPlanEntrega = (DAL_pyProyectoPlanEntrega)this.GetInstance(typeof(DAL_pyProyectoPlanEntrega), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyActaEntregaDeta = (DAL_pyActaEntregaDeta)this.GetInstance(typeof(DAL_pyActaEntregaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_pyActaEntregaDeta filter = new DTO_pyActaEntregaDeta();
                filter.NumDocProyecto.Value = numDocProy;
                List<DTO_pyActaEntregaDeta> listActaDetaExist = this.pyActaEntregaDeta_GetByParameter(filter); 
                List<DTO_pyProyectoTarea> tareasProy = this.pyProyectoTarea_Get(numDocProy, string.Empty, string.Empty);              
                result = this._dal_pyProyTareaCliente.DAL_pyProyectoTareaCliente_Get(numDocProy,tareaCliente, desc);

                //Recorre los entregables
                foreach (DTO_pyProyectoTareaCliente entregable in result)
                {
                    int index = 1;
                    entregable.Detalle = this._dal_pyProyPlanEntrega.DAL_pyProyectoPlanEntrega_GetTareaCliente(entregable.Consecutivo.Value.Value,null);

                    //Trae las tareas asignadas al entregable
                    entregable.DetalleTareas = tareasProy.FindAll(x=>x.TareaEntregable.Value == entregable.TareaEntregable.Value);
                    //Recorre la programacion de entregables
                    entregable.Detalle.ForEach(x =>
                    {
                       //Trae las tareas que tengan asignado plan de Entrega
                       x.DetalleTareas = tareasProy.FindAll(y => y.ConsEntrega.Value == x.Consecutivo.Value);
                       x.TareaEntregable.Value = entregable.TareaEntregable.Value;
                       x.Index = index;
                       index++;

                        //Llena las actas de entrega
                        DTO_pyActaEntregaDeta acta = new DTO_pyActaEntregaDeta();
                        acta.ConsTareaCliente.Value = x.ConsecTarea.Value;
                        acta.ConsTareaEntrega.Value = x.Consecutivo.Value;
                        acta.NumDocProyecto.Value = numDocProy;
                        acta.FacturaInd.Value = x.FacturaInd.Value;
                        acta.FechaEntrega.Value = x.FechaEntrega.Value;
                        acta.Observaciones.Value = x.Observaciones.Value;
                        acta.PorEntrega.Value = x.PorEntrega.Value;
                        acta.NumeroDoc.Value = listActaDetaExist.Exists(y => y.ConsTareaEntrega.Value == x.Consecutivo.Value) ? listActaDetaExist.Find(y => y.ConsTareaEntrega.Value == x.Consecutivo.Value).NumeroDoc.Value : null;
                        acta.NumDocFactura.Value = listActaDetaExist.Exists(y => y.ConsTareaEntrega.Value ==  x.Consecutivo.Value) ? listActaDetaExist.Find(y => y.ConsTareaEntrega.Value ==  x.Consecutivo.Value).NumDocFactura.Value : null;
                        acta.PorEntregado.Value = listActaDetaExist.FindAll(y => y.ConsTareaEntrega.Value ==  x.Consecutivo.Value).Sum(z => z.PorEntregado.Value);
                        acta.Consecutivo.Value = listActaDetaExist.Exists(y => y.ConsTareaEntrega.Value ==  x.Consecutivo.Value) ? listActaDetaExist.Find(y => y.ConsTareaEntrega.Value ==  x.Consecutivo.Value).Consecutivo.Value : null;
                        acta.PorPendiente.Value = acta.PorEntrega.Value - acta.PorEntregado.Value;
                        acta.PorAEntregar.Value = 0;
                        acta.Cantidad.Value = x.Cantidad.Value;
                        acta.CantEntregada.Value = listActaDetaExist.FindAll(y => y.ConsTareaEntrega.Value == acta.ConsTareaEntrega.Value).Sum(z => z.Cantidad.Value);
                        acta.CantPendiente.Value = acta.Cantidad.Value - acta.CantEntregada.Value;
                        acta.CantAEntregar.Value = 0;
                        acta.ValorFactura.Value = x.ValorFactura.Value;
                        acta.ValorAEntregar.Value = Math.Round(acta.PorEntrega.Value != 0 ? (acta.PorAEntregar.Value.Value * acta.ValorFactura.Value.Value) / acta.PorEntrega.Value.Value : 0, 2);
                        entregable.DetalleActas.Add(acta);

                    });
                    entregable.ValorFactura.Value = entregable.Detalle.Sum(x => x.ValorFactura.Value);
                    var rows = this._dal_pyActaEntregaDeta.DAL_pyActaEntregaDeta_Get(null, numDocProy, entregable.Consecutivo.Value, null);
                    entregable.ValorEntregado.Value = rows.Sum(x => x.ValorFactura.Value);
                    entregable.CantEntregada.Value = rows.Sum(x => x.Cantidad.Value);
                    DTO_MasterBasic servicio = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple,AppMasters.faServicios,entregable.ServicioID.Value,true,false);
                    entregable.ServicioDesc.Value = servicio != null? servicio.Descriptivo.Value : string.Empty;
                    if(entregable.FechaInicio.Value == null)
                       entregable.FechaInicio.Value = tareasProy.FindAll(x => x.TareaCliente.Value == entregable.TareaEntregable.Value).Min(y => y.FechaInicio.Value);
                    if (entregable.FechaFinal.Value == null)
                       entregable.FechaFinal.Value = tareasProy.FindAll(x => x.TareaCliente.Value == entregable.TareaEntregable.Value).Max(y => y.FechaFin.Value);
                }
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyProyectoTareaCliente_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae las tareas Cliente asignadas a un proyecto
        /// </summary>
        /// <param name="numDocProy">identificador del proyecto</param>
        /// <param name="tareaCliente">tarea Cliente</param>
        /// <param name="desc">descripcion de la tarea</param>
        /// <returns></returns>
        public List<DTO_pyProyectoPlanEntrega> pyProyectoPlanEntrega_GetByTareaCliente(int consectarea, DateTime fechaEntrega)
        {
            try
            {
                List<DTO_pyProyectoPlanEntrega> result = new List<DTO_pyProyectoPlanEntrega>();
                this._dal_pyProyPlanEntrega = (DAL_pyProyectoPlanEntrega)this.GetInstance(typeof(DAL_pyProyectoPlanEntrega), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                result = this._dal_pyProyPlanEntrega.DAL_pyProyectoPlanEntrega_GetTareaCliente(consectarea, fechaEntrega);
                int index = 1;
                result.ForEach(x =>
                {
                    x.Index = index;
                    index++;
                });
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyProyectoPlanEntrega_GetByTareaCliente");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega un docum de Acta de Trabajo
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="docCtrl">doc control</param>
        /// <param name="listEntregables">lista de actas</param>
        /// <returns>resultado</returns>
        public DTO_SerializedObject ActaEntrega_Add(int documentoID, DTO_glDocumentoControl docCtrl, List<DTO_pyProyectoTareaCliente> listEntregables, bool update, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            DTO_Alarma alarma = null;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyActaEntregaDeta = (DAL_pyActaEntregaDeta)this.GetInstance(typeof(DAL_pyActaEntregaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int numDoc = 0;

                if (!update)
                {
                    docCtrl.NumeroDoc.Value = 0;
                    DTO_TxResultDetail resultGLDC = new DTO_TxResultDetail();
                    resultGLDC = this._moduloGlobal.glDocumentoControl_Add(AppDocuments.ActaEntrega, docCtrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_AddDocument;
                        return result;
                    }

                    numDoc = Convert.ToInt32(resultGLDC.Key);
                    docCtrl.NumeroDoc.Value = numDoc;

                    #region Guarda en pyActaTrabajoDeta

                    foreach (DTO_pyProyectoTareaCliente tarea in listEntregables)
                    {
                        foreach (DTO_pyActaEntregaDeta acta in tarea.DetalleActas.FindAll(x=>x.PorAEntregar.Value != 0))
                        {
                            acta.NumeroDoc.Value = numDoc;
                            acta.ValorFactura.Value = acta.ValorAEntregar.Value;
                            this._dal_pyActaEntregaDeta.DAL_pyActaEntregaDeta_Add(acta); 
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Actualiza en pyActaTrabajoDeta
                    numDoc = docCtrl.NumeroDoc.Value.Value;
                    foreach (DTO_pyProyectoTareaCliente tarea in listEntregables)
                    {
                        foreach (DTO_pyActaEntregaDeta acta in tarea.DetalleActas.FindAll(x => x.PorAEntregar.Value != 0))
                        {
                            bool exist = this._dal_pyActaEntregaDeta.DAL_pyActaEntregaDeta_Exist(acta.Consecutivo.Value);
                            if (exist)
                            {
                                acta.ValorFactura.Value = acta.ValorAEntregar.Value;
                                this._dal_pyActaEntregaDeta.DAL_pyActaEntregaDeta_Upd(acta);
                            }
                            else
                            {
                                acta.NumeroDoc.Value = numDoc;
                                acta.ValorFactura.Value = acta.ValorAEntregar.Value;
                                this._dal_pyActaEntregaDeta.DAL_pyActaEntregaDeta_Add(acta);
                            }
                        }
                    }
                    #endregion
                }

                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    //Trae la info de la alarma
                    alarma = this.GetFirstMailInfo(numDoc, true);
                    alarma.NumeroDoc = numDoc.ToString();
                    alarma.PrefijoID = docCtrl.PrefijoID.Value.TrimEnd();
                    alarma.Consecutivo = docCtrl.DocumentoNro.Value.ToString();
                    return alarma;
                    #endregion
                }

                return result;
            }
            catch (Exception ex)
            {
                docCtrl.NumeroDoc.Value = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ActaEntrega_Add");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        if (docCtrl.DocumentoNro.Value == 0)
                        {
                            docCtrl.DocumentoNro.Value = Convert.ToInt32(this.GenerarDocumentoNro(documentoID, docCtrl.PrefijoID.Value));
                            this._moduloGlobal.ActualizaConsecutivos(docCtrl, true, false, false);
                        }
                        result.ExtraField = docCtrl.DocumentoNro.Value.ToString();
                        alarma.Consecutivo = docCtrl.DocumentoNro.Value.ToString();
                    }
                }
                else if (base._mySqlConnectionTx != null)
                {
                    docCtrl.NumeroDoc.Value = 0;
                    base._mySqlConnectionTx.Rollback();
                }

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
                List<DTO_pyActaEntregaDeta> result = new List<DTO_pyActaEntregaDeta>();
                this._dal_pyActaEntregaDeta = (DAL_pyActaEntregaDeta)this.GetInstance(typeof(DAL_pyActaEntregaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                result = this._dal_pyActaEntregaDeta.DAL_pyActaEntregaDeta_GetByParameter(filter);
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyActaEntregaDeta_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Acta de entrega
        /// </summary>
        /// <param name="documentID">Doc</param>
        /// <param name="ctrlProy">ctrl</param>
        /// <param name="docuProy">header</param>
        /// <param name="listEntregables">lista</param>
        /// <returns>resultado</returns>
        public DTO_SerializedObject ActaEntrega_ApprovePreFactura(int documentID,DTO_glDocumentoControl ctrlProy,DTO_pyProyectoDocu docuProy, List<DTO_pyProyectoTareaCliente> listEntregables,List<DTO_faFacturacionFooter> listDetalleFact,Dictionary<Tuple<int, int>, int> batchProgress)
        {
            base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            DTO_Alarma alarma = null;
            DTO_glDocumentoControl docCtrl = new DTO_glDocumentoControl();
            DTO_faFacturaDocu header = new DTO_faFacturaDocu();
            List<DTO_faFacturacionFooter> footerList = new List<DTO_faFacturacionFooter>();

            try
            {
                DTO_TxResult resultFactDocu = new DTO_TxResult();
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterSimple = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloFacturacion = (ModuloFacturacion)base.GetInstance(typeof(ModuloFacturacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables por defecto
                string zona = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_ZonaxDefecto);
                string listaPrecio = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_ListaPreciosxdefecto);
                string lineaPresupuesto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string terceroEmpresa = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                string asesor = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_AsesorPorDefecto);
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string periodoFactString = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_Periodo);
                DateTime periodoFact = !string.IsNullOrEmpty(periodoFactString) ? Convert.ToDateTime(periodoFactString) : DateTime.Today;
                DateTime fechaFact = periodoFact.Month ==  DateTime.Now.Month?  DateTime.Today : new DateTime(periodoFact.Year,periodoFact.Month,DateTime.DaysInMonth(periodoFact.Year,periodoFact.Month));
                #endregion               

                #region Valida la Cuenta del Tipo de factura
                string cta = string.Empty;
                string comprobanteID = string.Empty;
                DTO_pyClaseProyecto claseProy = (DTO_pyClaseProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto,docuProy.ClaseServicioID.Value, true, false);
                DTO_faFacturaTipo facTipo = (DTO_faFacturaTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faFacturaTipo, claseProy.FacturaTipoID.Value, true, false);
                if (facTipo != null)
                {
                    DTO_coDocumento coDocumento = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, facTipo.coDocumentoID.Value, true, false);
                    if (coDocumento != null)
                    {
                        comprobanteID = coDocumento.ComprobanteID.Value;
                        cta = ctrlProy.MonedaID.Value == monedaLocal ? coDocumento.CuentaLOC.Value : coDocumento.CuentaEXT.Value;
                        DTO_coPlanCuenta dtoCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, cta, true, false);
                        if (dtoCuenta != null)
                        {
                            DTO_glConceptoSaldo concSaldoDoc = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, dtoCuenta.ConceptoSaldoID.Value, true, false);
                            if (concSaldoDoc.coSaldoControl.Value.Value != (int)SaldoControl.Doc_Interno)
                            {
                                result.ResultMessage = DictionaryMessages.Err_InvalidCuentaTipoFact;
                                result.Result = ResultValue.NOK;
                                return result;
                            }
                            else if (coDocumento.DocumentoID.Value != AppDocuments.FacturaVenta.ToString())
                            {
                                result.ResultMessage = DictionaryMessages.Err_InvalidDocFact;
                                result.Result = ResultValue.NOK;
                                return result;
                            }
                        }  
                    }
                }
                else
                {
                    result.ResultMessage = "La Tipo de Factura no existe, verifique la clase del Proyecto";
                    result.Result = ResultValue.NOK;
                    return result;
                }

                #endregion

                if (result.Result == ResultValue.OK)
                {
                    int i = 0;
                    int percent = ((i + 1) * 100) / listEntregables.Count;
                    batchProgress[tupProgress] = percent;

                    int numeroDoc = 0;
                    #region Asigna Documento Control
                    //Documento Control
                    docCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                    docCtrl.DocumentoID.Value = AppDocuments.FacturaVenta;
                    docCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    docCtrl.PeriodoDoc.Value = periodoFact;
                    docCtrl.Fecha.Value = DateTime.Now;
                    docCtrl.FechaDoc.Value = fechaFact;
                    docCtrl.PeriodoUltMov.Value = fechaFact;
                    docCtrl.MonedaID.Value = ctrlProy.MonedaID.Value;
                    docCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                    docCtrl.PrefijoID.Value = ctrlProy.PrefijoID.Value;
                    docCtrl.TerceroID.Value = ctrlProy.TerceroID.Value;
                    docCtrl.TasaCambioCONT.Value = 0;
                    docCtrl.TasaCambioDOCU.Value = 0;
                    docCtrl.LugarGeograficoID.Value = ctrlProy.LugarGeograficoID.Value;
                    docCtrl.LineaPresupuestoID.Value = lineaPresupuesto;
                    docCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                    docCtrl.seUsuarioID.Value = this.UserId;
                    docCtrl.Descripcion.Value = "PREFACTURA VENTA DE ACTA ENTREGA ";
                    docCtrl.ProyectoID.Value = ctrlProy.ProyectoID.Value;
                    docCtrl.CentroCostoID.Value = ctrlProy.CentroCostoID.Value;
                    docCtrl.Observacion.Value = "Entrega de items del Proyecto  " + ctrlProy.ProyectoID.Value;                    
                    docCtrl.CuentaID.Value = cta;
                    docCtrl.ComprobanteID.Value = comprobanteID;
                    docCtrl.ComprobanteIDNro.Value = 0;
                    docCtrl.DocumentoNro.Value = 0;
                    docCtrl.ConsSaldo.Value = 0;
                    docCtrl.Valor.Value = listDetalleFact.Sum(x => x.Movimiento.Valor1LOC.Value);
                    docCtrl.Iva.Value = 0;
                    #endregion
                    #region Asigna faFacturaDocu
                    DTO_faCliente cliente = (DTO_faCliente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, docuProy.ClienteID.Value, true, false);
                    header.EmpresaID.Value = this.Empresa.ID.Value;
                    header.NumeroDoc.Value = 0;
                    header.AsesorID.Value = asesor;
                    header.FacturaTipoID.Value = claseProy.FacturaTipoID.Value;
                    header.DocumentoREL.Value = 0;
                    header.FacturaREL.Value = 0;
                    header.MonedaPago.Value = ctrlProy.MonedaID.Value;
                    header.ClienteID.Value = docuProy.ClienteID.Value;
                    header.ListaPrecioID.Value = cliente.ListaPrecioID.Value;
                    header.ZonaID.Value = cliente.ZonaID.Value;
                    header.TasaPago.Value = 1;
                    header.FechaVto.Value = docCtrl.FechaDoc.Value;
                    header.FormaPago.Value = "n/a";
                    header.Valor.Value = listDetalleFact.Sum(x=>x.Movimiento.Valor1LOC.Value);
                    header.Iva.Value = 0;
                    header.Bruto.Value = header.Valor.Value;
                    header.Porcentaje1.Value = 0;
                    header.Porcentaje2.Value = 0;
                    header.PorcPtoPago.Value = 0;
                    header.FechaPtoPago.Value = ctrlProy.FechaDoc.Value;
                    header.ValorPtoPago.Value = 0;
                    header.Retencion1.Value = 0;
                    header.Retencion2.Value = 0;
                    header.Retencion3.Value = 0;
                    header.Retencion4.Value = 0;
                    header.Retencion10.Value = docuProy.VlrAnticipoFactVenta;
                    header.DatoAdd5.Value = docuProy.VlrPorcAnticipo.ToString()+"%";
                    header.FacturaFijaInd.Value = false;
                    header.RteGarantiaIvaInd.Value = false;
                    #endregion                    
                    #region Guarda la factura Nueva
                    DTO_SerializedObject resulFact = this._moduloFacturacion.FacturaVenta_Guardar(AppDocuments.FacturaVenta, docCtrl, header, listDetalleFact, false, out numeroDoc, batchProgress, true);
                    if (resulFact.GetType() == typeof(DTO_TxResult))
                    {
                        DTO_TxResult restmp = (DTO_TxResult)resulFact;
                        result = restmp;
                        return restmp;
                    }
                    else
                    {
                        #region Actualiza los items de Acta Deta
                        this._dal_pyActaEntregaDeta = (DAL_pyActaEntregaDeta)base.GetInstance(typeof(DAL_pyActaEntregaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                        foreach (DTO_pyProyectoTareaCliente entr in listEntregables.FindAll(x => x.SelectInd.Value.Value))
	                    {
                            foreach (DTO_pyActaEntregaDeta acta in entr.DetalleActas)
                            {
                                acta.NumDocFactura.Value = docCtrl.NumeroDoc.Value;
                                this._dal_pyActaEntregaDeta.DAL_pyActaEntregaDeta_Upd(acta);
                            }
	                    }
                          
                        #endregion
                        #region Asigna el usuario con la alarma
                        try
                        {
                            //Trae la info de la alarma
                            alarma = this.GetFirstMailInfo(docCtrl.NumeroDoc.Value.Value, true);
                            alarma.NumeroDoc = numeroDoc.ToString();
                            return alarma;
                        }
                        catch (Exception ex)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_ReportCreate;
                        }
                        #endregion
                    }
                    #endregion
                    i++;
                }

                batchProgress[tupProgress] = 100;
                return result;
            }
            catch (Exception ex)
            {
                batchProgress[tupProgress] = 100;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ActaEntrega_ApprovePreFactura");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    base._mySqlConnectionTx.Commit();

                    base._mySqlConnectionTx = null;
                    this._moduloGlobal._mySqlConnectionTx = null;
                    this._moduloFacturacion._mySqlConnectionTx = null;
                    this._moduloContabilidad._mySqlConnectionTx = null;

                    if (docCtrl.NumeroDoc.Value != 0)
                    {
                        DTO_coComprobante coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, docCtrl.ComprobanteID.Value, true, false);
                        docCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(AppDocuments.FacturaVenta, docCtrl.PrefijoID.Value);
                        docCtrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, docCtrl.PrefijoID.Value, docCtrl.PeriodoDoc.Value.Value, docCtrl.DocumentoNro.Value.Value);

                        this._moduloContabilidad.ActualizaComprobanteNro(docCtrl.NumeroDoc.Value.Value, docCtrl.ComprobanteIDNro.Value.Value, true);
                        this._moduloGlobal.ActualizaConsecutivos(docCtrl, true, true, true);
                        if (alarma != null)
                            alarma.Consecutivo = docCtrl.DocumentoNro.Value.ToString();
                    }
                }
                else if (base._mySqlConnectionTx != null)
                    base._mySqlConnectionTx.Rollback();
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
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyTareaCliente = (DAL_pyProyectoTareaCliente)this.GetInstance(typeof(DAL_pyProyectoTareaCliente), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyPlanEntrega = (DAL_pyProyectoPlanEntrega)this.GetInstance(typeof(DAL_pyProyectoPlanEntrega), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyActaEntregaDeta = (DAL_pyActaEntregaDeta)this.GetInstance(typeof(DAL_pyActaEntregaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<int> facturas = new List<int>();
                foreach (DTO_pyProyectoTareaCliente entreg in entregables)
                {                   
                    if (anulaPreFacturas)
                    {                       
                        foreach (DTO_pyActaEntregaDeta acta in entreg.DetalleActas.FindAll(x=>x.NumDocFactura.Value.HasValue))
                        {
                            if (!facturas.Exists(x => x == acta.NumDocFactura.Value.Value))
                            {
                                DTO_glDocumentoControl ctrlFactura = this._moduloGlobal.glDocumentoControl_GetByID(acta.NumDocFactura.Value.Value);
                                if (ctrlFactura != null && ctrlFactura.Estado.Value != (byte)EstadoDocControl.Aprobado)
                                {
                                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentoID, acta.NumDocFactura.Value.Value, EstadoDocControl.Anulado, "Anulado por Actualizacion de Entregables", true);
                                    this._moduloGlobal.glMovimientoDetaPRE_Delete(acta.NumDocFactura.Value.Value,true);
                                    this._moduloContabilidad.BorrarAuxiliar_Pre(ctrlFactura.PeriodoDoc.Value.Value, ctrlFactura.ComprobanteID.Value, ctrlFactura.ComprobanteIDNro.Value.Value);
                                    facturas.Add(acta.NumDocFactura.Value.Value);
                                }
                            }
                        }
                      
                    }
                    if (deleteActas)
                    {
                        List<int> actas = new List<int>();
                        foreach (DTO_pyActaEntregaDeta acta in entreg.DetalleActas.FindAll(x => x.Consecutivo.Value.HasValue))
                        {
                            //Borra el acta
                            this._dal_pyActaEntregaDeta.DAL_pyActaEntregaDeta_DeleteByConsecutivo(acta.Consecutivo.Value.Value);
                            //Anula el documento de acta Entrega
                            if (acta.NumeroDoc != null && !actas.Exists(x => x == acta.NumeroDoc.Value.Value))
                            {
                                DTO_glDocumentoControl ctrlActa = this._moduloGlobal.glDocumentoControl_GetByID(acta.NumeroDoc.Value.Value);
                                if (ctrlActa != null)
                                {
                                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentoID, acta.NumeroDoc.Value.Value, EstadoDocControl.Anulado, "Anulado por Actualizacion de Entregables", true);
                                    actas.Add(acta.NumeroDoc.Value.Value);
                                }
                            }
                        }
                    }
                    if (deleteProgramacion)
                    {
                        foreach (DTO_pyProyectoPlanEntrega plan in entreg.Detalle)
                            this._dal_pyProyPlanEntrega.DAL_pyProyectoPlanEntrega_DeleteByConsecutivo(plan.Consecutivo.Value.Value);
                    }                  
                }

                if(deleteEntregable)
                    this._dal_pyProyTareaCliente.DAL_pyProyectoTareaCliente_DeleteByNumeroDoc(numDocProy,string.Empty);
                
                return result;
            }
            catch (Exception ex)
            {
                numDocProy = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "EntregasCliente_Add");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    base._mySqlConnectionTx.Commit();
                    base._mySqlConnectionTx = null;
                }
                else if (base._mySqlConnectionTx != null)
                {
                    base._mySqlConnectionTx.Rollback();
                }

            }
        }

        #endregion

        #endregion

        #region Migracion

        /// <summary>
        /// Agrega o actualiza una lista de insumos o proveedores
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="listInsumos">Lista de insumos</param>
        /// <param name="listProveedores">Lista de proveedores</param>
        /// <param name="listGrupos">Lista de analisis</param>
        /// <param name="listAPU">Lista de Analisis Precios Unitarios</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult MigracionInsumos(int documentID, List<DTO_MigracionInsumos> listInsumos, List<DTO_MigracionProveedor> listProveedores, List<DTO_MigracionGrupos> listGrupos, List<DTO_MigracionAPU> listAPU,Dictionary<Tuple<int, int>, int> batchProgress)
        {
            base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                DTO_TxResult resultFactDocu = new DTO_TxResult();
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterSimple = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterComplex = (DAL_MasterComplex)base.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterHierarchy = (DAL_MasterHierarchy)base.GetInstance(typeof(DAL_MasterHierarchy), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                int i =0;
                #region Variables por defecto
                string grupoInvDef = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_GrupoxDef);
                string claseInv = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_ClasexDef);
                string tipoInvDef = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoxDef);
                string serieInv = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_SeriexDef);
                string materialInv = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_MaterialxDef);
                string marcaInvXDef = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_MarcaxDef);
                string posicionInv = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_PosicionxDef);
                string unidadInv = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_UnidadxDef);
                string lugGeoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string terxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                string refxDefecto = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_ReferenciaporDefecto);
                string codigoBSInv = this.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_CodigoBSCompraInv);
                string mdaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);

                #region Valida la existencia de FKs
                //DTO_MasterBasic asesorvalid = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faAsesor, tipoInv, true, false);
                //if (asesorvalid == null)
                //{
                //    result.Result = ResultValue.NOK;
                //    result.ResultMessage = DictionaryMessages.Err_CodeInvalid + "&&" + "AsesorID" + "&&" + tipoInv;
                //    return result;
                //}
                #endregion

                #endregion

                #region Valida si los terceros y proveedores existen para crearlos
                if (listProveedores != null && listProveedores.Count > 0)
                {
                    List<string> listProveedor = listProveedores.Select(x => x.Codigo.Value.ToString()).Distinct().ToList();
                    foreach (DTO_MigracionProveedor prov in listProveedores)
                    {
                        int percent = ((i + 1) * 100) / listProveedores.Count;
                        batchProgress[tupProgress] = percent;
                        DTO_prProveedor dtoProveedor = (DTO_prProveedor)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, prov.Codigo.Value.ToString(), true, false);
                        DTO_TxResultDetail detailResult;
                        if (dtoProveedor == null)
                        {
                            //DTO_prProveedor nuevoProv = proveedores.Find(x => x.TerceroID.Value == prov);
                            #region Crea el Nuevo tercero
                            //dtoTercero = new DTO_coTercero();
                            //dtoTercero.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                            //dtoTercero.ID.Value = prov;
                            //dtoTercero.Descriptivo.Value = nuevoTer.Descriptivo.Value;
                            //dtoTercero.ApellidoPri.Value = nuevoTer.Descriptivo.Value;
                            //dtoTercero.ApellidoSdo.Value = nuevoTer.Descriptivo.Value;
                            //dtoTercero.NombrePri.Value = nuevoTer.Descriptivo.Value;
                            //dtoTercero.NombreSdo.Value = nuevoTer.Descriptivo.Value;
                            ////dtoTercero.Direccion.Value = nuevoTer.Direccion.Value;
                            //dtoTercero.Tel1.Value = nuevoTer.TelContacto.Value;
                            //dtoTercero.CECorporativo.Value = nuevoTer.MailContacto.Value;
                            //dtoTercero.LugarGeograficoID.Value = nuevoTer.Ciudad.Value;
                            //dtoTercero.ReferenciaID.Value = "RCN"; //Cual glControl va?
                            //dtoTercero.ActEconomicaID.Value = "0000";//Cual glControl va?
                            //DTO_glLugarGeografico lugar = (DTO_glLugarGeografico)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, nuevoTer.Ciudad.Value, true, false);
                            //dtoTercero.Pais.Value = lugar.PaisID.Value;
                            //dtoTercero.TerceroDocTipoID.Value = "NIT";//Cual glControl va?
                            //dtoTercero.AutoRetRentaInd.Value = false;//De donde lo traigo?
                            //dtoTercero.AutoRetIVAInd.Value = false;//De donde lo traigo?
                            //dtoTercero.DeclaraIVAInd.Value = false;//De donde lo traigo?
                            //dtoTercero.DeclaraRentaInd.Value = false;//De donde lo traigo?
                            //dtoTercero.ExcluyeCREEInd.Value = false;//De donde lo traigo?
                            //dtoTercero.IndependienteEMPInd.Value = false;//De donde lo traigo?
                            //dtoTercero.ProveedorInd.Value = true;
                            //dtoTercero.RadicaDirectoInd.Value = false;//De donde lo traigo?
                            //dtoTercero.ActivoInd.Value = true;
                            //dtoTercero.CtrlVersion.Value = 1;
                            //this._dal_MasterSimple.DocumentID = AppMasters.coTercero;
                            //detailResult = this._dal_MasterSimple.DAL_MasterSimple_AddItem(dtoTercero);
                            //result.Details = new List<DTO_TxResultDetail>();
                            //if (detailResult.DetailsFields.Count > 0)
                            //result.Details.Add(detailResult);

                            //if (detailResult.Message == ResultValue.NOK.ToString())
                            //{
                            //    result.Result = ResultValue.NOK;
                            //    break;
                            //}
                            #endregion
                            #region Crea el Nuevo Proveedor
                            dtoProveedor = new DTO_prProveedor();
                            dtoProveedor.ID.Value = prov.Codigo.Value.ToString();
                            dtoProveedor.TerceroID.Value = terxDef;
                            dtoProveedor.Descriptivo.Value = prov.Nombre.Value;
                            dtoProveedor.Direccion.Value = prov.Direccion.Value;
                            dtoProveedor.TelContacto.Value = prov.Telefono.Value.Length > 20 ? prov.Telefono.Value.Substring(0, 20) : prov.Telefono.Value;
                            dtoProveedor.MailContacto.Value = prov.Email.Value;
                            dtoProveedor.Ciudad.Value = prov.Ciudad.Value;
                            dtoProveedor.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                            dtoProveedor.ActivoInd.Value = true;
                            dtoProveedor.CtrlVersion.Value = 1;
                            this._dal_MasterSimple.DocumentID = AppMasters.prProveedor;
                            detailResult = this._dal_MasterSimple.DAL_MasterSimple_AddItem(dtoProveedor);
                            result.Details = new List<DTO_TxResultDetail>();
                            if (detailResult.DetailsFields.Count > 0)
                                result.Details.Add(detailResult);
                            if (detailResult.Message == ResultValue.NOK.ToString())
                            {
                                result.Result = ResultValue.NOK;
                                break;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Actualiza Proveedor
                            dtoProveedor.TerceroID.Value = terxDef;
                            dtoProveedor.Descriptivo.Value = prov.Nombre.Value;
                            dtoProveedor.Direccion.Value = prov.Direccion.Value;
                            dtoProveedor.TelContacto.Value = prov.Telefono.Value.Length > 20 ? prov.Telefono.Value.Substring(0, 20) : prov.Telefono.Value;
                            dtoProveedor.MailContacto.Value = prov.Email.Value;
                            dtoProveedor.Ciudad.Value = prov.Ciudad.Value;
                            dtoProveedor.CtrlVersion.Value = dtoProveedor.CtrlVersion.Value++;
                            this._dal_MasterSimple.DocumentID = AppMasters.prProveedor;
                            result = this._dal_MasterSimple.DAL_MasterSimple_Update(dtoProveedor, true);
                            if (result.Result == ResultValue.NOK)
                                break;
                            #endregion
                        }
                        i++;
                    } 
                }
                #endregion

                #region Valida los insumos
                if (listInsumos != null && listInsumos.Count > 0)
                {
                    bool addTareaXRecEquipo = this.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_IndCrearTareasMigraInsumosEquipo).Equals("1") ? true : false;
                    bool addTareaXRecPerson = this.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_IndCrearTareasMigraInsumosPerson).Equals("1") ? true : false;
                    bool addTareaXRecTransporte = this.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_IndCrearTareasMigraInsumosTrans).Equals("1") ? true : false;
                    bool addTareaXRecMaterial = this.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_IndCrearTareasMigraInsumosMaterialEspec).Equals("1") ? true : false;
                    string servicioFactXDef = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_ServicioxDefecto);

                    string undMedida = string.Empty;
                    foreach (DTO_MigracionInsumos insumo in listInsumos)
                    {
                        int percent = ((i + 1) * 100) / listInsumos.Count;
                        batchProgress[tupProgress] = percent;
                        DTO_pyRecurso dtoRecurso = (DTO_pyRecurso)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyRecurso, insumo.Codigo.Value, true, false);
                        DTO_TxResultDetail detailResult;
                        if (dtoRecurso == null)
                        {
                            #region Crea el Empaque 
                            undMedida = insumo.Medida.Value;
                            DTO_inUnidad dtoUnidad = (DTO_inUnidad)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inUnidad, undMedida, true, false);
                            DTO_inEmpaque dtoEmpaque = (DTO_inEmpaque)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, undMedida, true, false);
                            if (dtoEmpaque == null)
                            {
                                #region Agrega Empaque
                                dtoEmpaque = new DTO_inEmpaque();
                                dtoEmpaque.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                                dtoEmpaque.ID.Value = insumo.Medida.Value;
                                if (dtoUnidad == null)
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = "La Unidad de Medida " + insumo.Medida.Value + " del insumo " + insumo.Codigo.Value + " no existe, verifique";
                                    return result;
                                }
                                dtoEmpaque.Descriptivo.Value = dtoUnidad.Descriptivo.Value;
                                dtoEmpaque.UnidadInvID.Value = insumo.Medida.Value;
                                dtoEmpaque.EmpaqueTipo.Value = 0;
                                dtoEmpaque.Margen.Value = 1;
                                dtoEmpaque.Cantidad.Value = 1;
                                dtoEmpaque.UnidadVariableInd.Value = false;
                                dtoEmpaque.ActivoInd.Value = true;
                                dtoEmpaque.CtrlVersion.Value = 1;
                                this._dal_MasterSimple.DocumentID = AppMasters.inEmpaque;
                                detailResult = this._dal_MasterSimple.DAL_MasterSimple_AddItem(dtoEmpaque);
                                detailResult.line = i;
                                result.Details = new List<DTO_TxResultDetail>();
                                if (detailResult.DetailsFields.Count > 0)
                                    result.Details.Add(detailResult);
                                if (detailResult.Message == ResultValue.NOK.ToString())
                                {
                                    result.Result = ResultValue.NOK;
                                    break;
                                } 
                                #endregion
                            }
                            #endregion                            
                            #region Crea la nueva Referencia(inReferencia)
                            DTO_inReferencia dtoReferencia = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, insumo.Codigo.Value, true, false);
                            if (dtoReferencia == null && (insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Insumo || insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Equipo))
                            {
                                dtoReferencia = new DTO_inReferencia();
                                dtoReferencia.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                                dtoReferencia.ID.Value = insumo.Codigo.Value;
                                dtoReferencia.Descriptivo.Value = insumo.Nombre.Value;
                                dtoReferencia.DescrDetallada.Value = insumo.Observacion.Value;
                                dtoReferencia.GrupoInvID.Value = !string.IsNullOrEmpty(insumo.Grupo.Value)?insumo.Grupo.Value:grupoInvDef;
                                dtoReferencia.ClaseInvID.Value = claseInv;
                                dtoReferencia.TipoInvID.Value = !string.IsNullOrEmpty(insumo.TipoInv.Value) ? insumo.TipoInv.Value : tipoInvDef;
                                dtoReferencia.SerieID.Value = serieInv;
                                dtoReferencia.MaterialInvID.Value = materialInv;
                                dtoReferencia.MarcaInvID.Value = !string.IsNullOrEmpty(insumo.MarcaInvID.Value)? insumo.MarcaInvID.Value: marcaInvXDef;
                                dtoReferencia.RefProveedor.Value = insumo.RefProveedor.Value;
                                dtoReferencia.PosicionArancelID.Value = posicionInv;
                                dtoReferencia.EmpaqueInvID.Value = insumo.Medida.Value;
                                dtoReferencia.UnidadInvID.Value = insumo.Medida.Value;
                                dtoReferencia.CostoStandar.Value = insumo.Precio.Value;
                                dtoReferencia.ActivoInd.Value = true;
                                dtoReferencia.CtrlVersion.Value = 1;
                                dtoReferencia.ProveedorID.Value = insumo.Proveedor.Value;
                                this._dal_MasterSimple.DocumentID = AppMasters.inReferencia;
                                detailResult = this._dal_MasterSimple.DAL_MasterSimple_AddItem(dtoReferencia);
                                detailResult.line = i;
                                result.Details = new List<DTO_TxResultDetail>();
                                if (detailResult.DetailsFields.Count > 0)
                                    result.Details.Add(detailResult);
                                if (detailResult.Message == ResultValue.NOK.ToString())
                                {
                                    result.Result = ResultValue.NOK;
                                    break;
                                }
                            }
                            #endregion                            
                            #region Crea el nuevo CodigoBS(prBienServicio)
                            if (insumo.Tipo_Grupo.Value != (byte)TipoRecurso.Insumo)
                            {
                                DTO_prBienServicio dtoBienServicio = new DTO_prBienServicio();
                                dtoBienServicio.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                                dtoBienServicio.ID.Value = insumo.Codigo.Value;
                                dtoBienServicio.Descriptivo.Value = insumo.Nombre.Value;
                                dtoBienServicio.TipoControl.Value = 1;
                                dtoBienServicio.UnidadInvID.Value = insumo.Medida.Value;
                                if (insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Equipo)
                                    dtoBienServicio.ClaseBSID.Value = "02";
                                else if (insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Personal)
                                    dtoBienServicio.ClaseBSID.Value = "03";
                                else if (insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Transporte)
                                    dtoBienServicio.ClaseBSID.Value = "04";
                                dtoBienServicio.ActivoInd.Value = true;
                                dtoBienServicio.CtrlVersion.Value = 1;
                                dtoBienServicio.MovInd.Value = true;
                                this._dal_MasterHierarchy.DocumentID = AppMasters.prBienServicio;
                                detailResult = this._dal_MasterHierarchy.DAL_MasterSimple_AddItem(dtoBienServicio);
                                detailResult.line = i;
                                result.Details = new List<DTO_TxResultDetail>();
                                if (detailResult.DetailsFields.Count > 0)
                                    result.Details.Add(detailResult);
                                if (detailResult.Message == ResultValue.NOK.ToString())
                                {
                                    result.Result = ResultValue.NOK;
                                    break;
                                } 
                            }
                            #endregion                            
                            #region Crea el nuevo Recurso(pyRecurso)
                            dtoRecurso = new DTO_pyRecurso();
                            dtoRecurso.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                            dtoRecurso.ID.Value = insumo.Codigo.Value;
                            dtoRecurso.Descriptivo.Value = insumo.Nombre.Value;
                            dtoRecurso.TipoRecurso.Value = insumo.Tipo_Grupo.Value;
                            dtoRecurso.UnidadInvID.Value = insumo.Medida.Value;
                            dtoRecurso.CostoBaseLocal.Value = insumo.Precio.Value;
                            dtoRecurso.FactorID.Value = 1;
                            dtoRecurso.TiempoRealInd.Value = true;
                            dtoRecurso.TipoCalculo.Value = 1;
                            if(insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Insumo || insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Equipo)
                                dtoRecurso.inReferenciaID.Value = dtoReferencia.ID.Value;
                            if (insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Insumo)
                                dtoRecurso.CodigoBSID.Value = codigoBSInv;
                            else
                                dtoRecurso.CodigoBSID.Value = insumo.Codigo.Value;
                            dtoRecurso.ActivoInd.Value = true;
                            dtoRecurso.CtrlVersion.Value = 1;
                            this._dal_MasterSimple.DocumentID = AppMasters.pyRecurso;
                            detailResult = this._dal_MasterSimple.DAL_MasterSimple_AddItem(dtoRecurso);
                            detailResult.line = i;
                            result.Details = new List<DTO_TxResultDetail>();
                            if (detailResult.DetailsFields.Count > 0)
                                result.Details.Add(detailResult);
                            if (detailResult.Message == ResultValue.NOK.ToString())
                            {
                                result.Result = ResultValue.NOK;
                                break;
                            }
                            #endregion                            
                            #region Crea el nuevo RecursoCosto(pyRecursoCostoBase)
                            Dictionary<string, string> pks = new Dictionary<string, string>();
                            pks.Add("LugarGeograficoID",lugGeoDef);
                            pks.Add("RecursoID", dtoRecurso.ID.Value);
                            DTO_pyRecursoCostoBase dtoRecursoCosto = (DTO_pyRecursoCostoBase)this.GetMasterComplexDTO(AppMasters.pyRecursoCostoBase, pks,true);
                            if (dtoRecursoCosto == null)
                            {
                                dtoRecursoCosto = new DTO_pyRecursoCostoBase();
                                dtoRecursoCosto.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                                dtoRecursoCosto.LugarGeograficoID.Value = lugGeoDef;
                                dtoRecursoCosto.RecursoID.Value = dtoRecurso.ID.Value;
                                dtoRecursoCosto.ProveedorEXT.Value = insumo.Proveedor.Value; //Construdata
                                dtoRecursoCosto.ProveedorEMP.Value = insumo.Proveedor2.Value != null ? insumo.Proveedor2.Value : insumo.Proveedor.Value; //Quamtum
                                dtoRecursoCosto.CostoLocalEXT.Value = insumo.MonedaID.Value.Equals(mdaLocal)? insumo.Precio.Value: 0;// Construdata
                                dtoRecursoCosto.CostoLocalEMP.Value = insumo.MonedaID.Value.Equals(mdaLocal)? (insumo.Precio2.Value != null? insumo.Precio2.Value : insumo.Precio.Value): 0; //Quantum
                                dtoRecursoCosto.CostoExtraEXT.Value = !insumo.MonedaID.Value.Equals(mdaLocal) ? insumo.Precio.Value : 0;// Construdata    
                                dtoRecursoCosto.CostoExtraEMP.Value = !insumo.MonedaID.Value.Equals(mdaLocal) ? (insumo.Precio2.Value != null ? insumo.Precio2.Value : insumo.Precio.Value) : 0; //Quantum;    
                                dtoRecursoCosto.ActivoInd.Value = true;
                                dtoRecursoCosto.CtrlVersion.Value = 1;
                                this._dal_MasterComplex.DocumentID = AppMasters.pyRecursoCostoBase;
                                detailResult = this._dal_MasterComplex.DAL_MasterComplex_AddItem(dtoRecursoCosto);
                                detailResult.line = i;
                                result.Details = new List<DTO_TxResultDetail>();
                                if (detailResult.DetailsFields.Count > 0)                                 
                                    result.Details.Add(detailResult);
                                if (detailResult.Message == ResultValue.NOK.ToString())
                                {
                                    result.Result = ResultValue.NOK;
                                    break;
                                }
                            }
                            #endregion
                            
                        }
                        else
                        {
                            undMedida = string.IsNullOrEmpty(insumo.Medida.Value) ? dtoRecurso.UnidadInvID.Value : insumo.Medida.Value;
                            #region Crea el Empaque si no existe
                            
                            DTO_inUnidad dtoUnidad = (DTO_inUnidad)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inUnidad, undMedida, true, false);
                            DTO_inEmpaque dtoEmpaque = (DTO_inEmpaque)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, undMedida, true, false);
                            if (dtoEmpaque == null)
                            {
                                #region Agrega Empaque
                                dtoEmpaque = new DTO_inEmpaque();
                                dtoEmpaque.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                                dtoEmpaque.ID.Value = undMedida;
                                if (dtoUnidad == null)
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = "La Unidad de Medida " + undMedida + " del insumo " + insumo.Codigo.Value + " no existe, verifique";
                                    return result;
                                }
                                dtoEmpaque.Descriptivo.Value = dtoUnidad.Descriptivo.Value;
                                dtoEmpaque.UnidadInvID.Value = undMedida;
                                dtoEmpaque.EmpaqueTipo.Value = 0;
                                dtoEmpaque.Margen.Value = 1;
                                dtoEmpaque.Cantidad.Value = 1;
                                dtoEmpaque.UnidadVariableInd.Value = false;
                                dtoEmpaque.ActivoInd.Value = true;
                                dtoEmpaque.CtrlVersion.Value = 1;
                                this._dal_MasterSimple.DocumentID = AppMasters.inEmpaque;
                                detailResult = this._dal_MasterSimple.DAL_MasterSimple_AddItem(dtoEmpaque);
                                detailResult.line = i;
                                result.Details = new List<DTO_TxResultDetail>();
                                if (detailResult.DetailsFields.Count > 0)
                                    result.Details.Add(detailResult);
                                if (detailResult.Message == ResultValue.NOK.ToString())
                                {
                                    result.Result = ResultValue.NOK;
                                    break;
                                }
                                #endregion
                            }
                            #endregion                            
                            #region Actualiza o agrega la referencia
                            if (insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Insumo || insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Equipo)
                            {
                                DTO_inReferencia dtoReferencia = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, insumo.Codigo.Value, true, false);
                                if (dtoReferencia != null)
                                {
                                    dtoReferencia.Descriptivo.Value = !string.IsNullOrEmpty(insumo.Nombre.Value) ? insumo.Nombre.Value : dtoReferencia.Descriptivo.Value;
                                    dtoReferencia.DescrDetallada.Value = !string.IsNullOrEmpty(insumo.Observacion.Value) ? insumo.Observacion.Value : dtoReferencia.DescrDetallada.Value; 
                                    dtoReferencia.UnidadInvID.Value = !string.IsNullOrEmpty(insumo.Medida.Value) ? insumo.Medida.Value : dtoReferencia.UnidadInvID.Value;
                                    dtoReferencia.ProveedorID.Value = !string.IsNullOrEmpty(insumo.Proveedor.Value) ? insumo.Proveedor.Value : dtoReferencia.ProveedorID.Value;
                                    dtoReferencia.GrupoInvID.Value = !string.IsNullOrEmpty(insumo.Grupo.Value) ? insumo.Grupo.Value : dtoReferencia.GrupoInvID.Value;
                                    dtoReferencia.TipoInvID.Value = !string.IsNullOrEmpty(insumo.TipoInv.Value) ? insumo.TipoInv.Value : dtoReferencia.TipoInvID.Value;
                                    dtoReferencia.MarcaInvID.Value = !string.IsNullOrEmpty(insumo.MarcaInvID.Value) ? insumo.MarcaInvID.Value : dtoReferencia.MarcaInvID.Value;
                                    dtoReferencia.RefProveedor.Value = !string.IsNullOrEmpty(insumo.RefProveedor.Value) ? insumo.RefProveedor.Value : dtoReferencia.RefProveedor.Value;
                                    dtoReferencia.EmpaqueInvID.Value = !string.IsNullOrEmpty(insumo.Medida.Value) ? insumo.Medida.Value : dtoReferencia.EmpaqueInvID.Value;
                                    dtoReferencia.CtrlVersion.Value = dtoReferencia.CtrlVersion.Value++;
                                    dtoReferencia.CostoStandar.Value = insumo.Precio.Value;
                                    this._dal_MasterSimple.DocumentID = AppMasters.inReferencia;
                                    result = this._dal_MasterSimple.DAL_MasterSimple_Update(dtoReferencia, true);
                                    if (result.Result == ResultValue.NOK)
                                        break; 
                                }
                                else
                                {
                                    dtoReferencia = new DTO_inReferencia();
                                    dtoReferencia.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                                    dtoReferencia.ID.Value = insumo.Codigo.Value;
                                    dtoReferencia.Descriptivo.Value = !string.IsNullOrEmpty(insumo.Nombre.Value) ? insumo.Nombre.Value : dtoRecurso.Descriptivo.Value;
                                    dtoReferencia.DescrDetallada.Value = insumo.Observacion.Value;
                                    dtoReferencia.GrupoInvID.Value = !string.IsNullOrEmpty(insumo.Grupo.Value) ? insumo.Grupo.Value : grupoInvDef; 
                                    dtoReferencia.ClaseInvID.Value = claseInv;
                                    dtoReferencia.TipoInvID.Value = !string.IsNullOrEmpty(insumo.TipoInv.Value) ? insumo.TipoInv.Value : tipoInvDef;
                                    dtoReferencia.SerieID.Value = serieInv;
                                    dtoReferencia.MaterialInvID.Value = materialInv;
                                    dtoReferencia.MarcaInvID.Value = !string.IsNullOrEmpty(insumo.MarcaInvID.Value) ? insumo.MarcaInvID.Value : marcaInvXDef;
                                    dtoReferencia.RefProveedor.Value = insumo.RefProveedor.Value;
                                    dtoReferencia.PosicionArancelID.Value = posicionInv;
                                    dtoReferencia.EmpaqueInvID.Value = undMedida;
                                    dtoReferencia.UnidadInvID.Value = undMedida;
                                    dtoReferencia.CostoStandar.Value = insumo.Precio.Value;
                                    dtoReferencia.ActivoInd.Value = true;
                                    dtoReferencia.CtrlVersion.Value = 1;
                                    dtoReferencia.ProveedorID.Value = insumo.Proveedor.Value;
                                    this._dal_MasterSimple.DocumentID = AppMasters.inReferencia;
                                    detailResult = this._dal_MasterSimple.DAL_MasterSimple_AddItem(dtoReferencia);
                                    detailResult.line = i;
                                    result.Details = new List<DTO_TxResultDetail>();
                                    if (detailResult.DetailsFields.Count > 0)
                                        result.Details.Add(detailResult);
                                    if (detailResult.Message == ResultValue.NOK.ToString())
                                    {
                                        result.Result = ResultValue.NOK;
                                        break;
                                    }
                                }
                            }
                            #endregion                            
                            #region Actualiza o agrega el codigoBS
                            string codigoBSRecurso = !string.IsNullOrEmpty(dtoRecurso.CodigoBSID.Value) ? dtoRecurso.CodigoBSID.Value : insumo.Codigo.Value;
                            if (insumo.Tipo_Grupo.Value != (byte)TipoRecurso.Insumo)
                            {
                                DTO_prBienServicio dtoBienServicio = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy,AppMasters.prBienServicio, codigoBSRecurso, true, false);
                                if (dtoBienServicio != null)
                                {
                                    dtoBienServicio.Descriptivo.Value = !string.IsNullOrEmpty(insumo.Nombre.Value) ? insumo.Nombre.Value : dtoBienServicio.Descriptivo.Value; 
                                    dtoBienServicio.TipoControl.Value = 1;
                                    dtoBienServicio.UnidadInvID.Value = !string.IsNullOrEmpty(insumo.Medida.Value) ? insumo.Medida.Value : dtoBienServicio.UnidadInvID.Value;
                                    if (insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Equipo)
                                        dtoBienServicio.ClaseBSID.Value = string.IsNullOrEmpty(dtoBienServicio.ClaseBSID.Value) ? "02" : dtoBienServicio.ClaseBSID.Value;
                                    else if (insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Personal)
                                        dtoBienServicio.ClaseBSID.Value = string.IsNullOrEmpty(dtoBienServicio.ClaseBSID.Value) ? "03" : dtoBienServicio.ClaseBSID.Value;
                                    else if (insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Transporte)
                                        dtoBienServicio.ClaseBSID.Value = string.IsNullOrEmpty(dtoBienServicio.ClaseBSID.Value) ? "04" : dtoBienServicio.ClaseBSID.Value;
                                    dtoBienServicio.CtrlVersion.Value = dtoBienServicio.CtrlVersion.Value++;
                                    this._dal_MasterHierarchy.DocumentID = AppMasters.prBienServicio;
                                    result = this._dal_MasterHierarchy.DAL_MasterSimple_Update(dtoBienServicio, true);
                                    if (result.Result == ResultValue.NOK)
                                        break;
                                }
                                else
                                {
                                    dtoBienServicio = new DTO_prBienServicio();
                                    dtoBienServicio.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                                    dtoBienServicio.ID.Value = codigoBSRecurso;
                                    dtoBienServicio.Descriptivo.Value = !string.IsNullOrEmpty(insumo.Nombre.Value) ? insumo.Nombre.Value : dtoRecurso.Descriptivo.Value;
                                    dtoBienServicio.TipoControl.Value = 1;
                                    dtoBienServicio.UnidadInvID.Value = undMedida;
                                    if (insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Equipo)
                                        dtoBienServicio.ClaseBSID.Value = "02";
                                    else if (insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Personal)
                                        dtoBienServicio.ClaseBSID.Value = "03";
                                    else if (insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Transporte)
                                        dtoBienServicio.ClaseBSID.Value = "04";
                                    dtoBienServicio.ActivoInd.Value = true;
                                    dtoBienServicio.CtrlVersion.Value = 1;
                                    dtoBienServicio.MovInd.Value = true;
                                    this._dal_MasterHierarchy.DocumentID = AppMasters.prBienServicio;
                                    detailResult = this._dal_MasterHierarchy.DAL_MasterSimple_AddItem(dtoBienServicio);
                                    detailResult.line = i;
                                    result.Details = new List<DTO_TxResultDetail>();
                                    if (detailResult.DetailsFields.Count > 0)
                                        result.Details.Add(detailResult);
                                    if (detailResult.Message == ResultValue.NOK.ToString())
                                    {
                                        result.Result = ResultValue.NOK;
                                        break;
                                    }
                                }
                            }
                            #endregion                            
                            #region Actualiza el recurso
                            dtoRecurso.Descriptivo.Value = !string.IsNullOrEmpty(insumo.Nombre.Value) ? insumo.Nombre.Value : dtoRecurso.Descriptivo.Value;
                            dtoRecurso.TipoRecurso.Value = !string.IsNullOrEmpty(insumo.Tipo_Grupo.Value.ToString()) ? insumo.Tipo_Grupo.Value : dtoRecurso.TipoRecurso.Value;
                            dtoRecurso.UnidadInvID.Value = !string.IsNullOrEmpty(insumo.Medida.Value) ? insumo.Medida.Value : dtoRecurso.UnidadInvID.Value;
                            dtoRecurso.CostoBaseLocal.Value = !string.IsNullOrEmpty(insumo.Precio.Value.ToString()) ? insumo.Precio.Value : dtoRecurso.CostoBaseLocal.Value;
                            if(insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Insumo || insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Equipo)
                                dtoRecurso.inReferenciaID.Value = insumo.Codigo.Value;
                            if (insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Insumo)
                                dtoRecurso.CodigoBSID.Value = codigoBSInv;
                            else
                                dtoRecurso.CodigoBSID.Value =codigoBSRecurso;
                            dtoRecurso.CtrlVersion.Value = dtoRecurso.CtrlVersion.Value++;
                            this._dal_MasterSimple.DocumentID = AppMasters.pyRecurso;
                            result = this._dal_MasterSimple.DAL_MasterSimple_Update(dtoRecurso, true);
                            if (result.Result == ResultValue.NOK)
                                break;
                          
                            #endregion                                                        
                            #region Actualiza el recursoCosto
                            Dictionary<string, string> pks = new Dictionary<string, string>();
                            pks.Add("LugarGeograficoID", lugGeoDef);
                            pks.Add("RecursoID", dtoRecurso.ID.Value);
                            DTO_pyRecursoCostoBase dtoRecursoCosto = (DTO_pyRecursoCostoBase)this.GetMasterComplexDTO(AppMasters.pyRecursoCostoBase, pks, true);
                            if (dtoRecursoCosto != null)
                            {
                                dtoRecursoCosto.CostoLocalEXT.Value = insumo.MonedaID.Value.Equals(mdaLocal)? insumo.Precio.Value : 0;// Construdata
                                dtoRecursoCosto.CostoLocalEMP.Value = insumo.MonedaID.Value.Equals(mdaLocal)? (insumo.Precio2.Value != null ? insumo.Precio2.Value : insumo.Precio.Value) : 0; //EmpresaActual
                                dtoRecursoCosto.CostoExtraEXT.Value = !insumo.MonedaID.Value.Equals(mdaLocal) ? insumo.Precio.Value : 0;// Construdata
                                dtoRecursoCosto.CostoExtraEMP.Value = !insumo.MonedaID.Value.Equals(mdaLocal) ? (insumo.Precio2.Value != null ? insumo.Precio2.Value : insumo.Precio.Value) : 0; //EmpresaActual;    
                                dtoRecursoCosto.ProveedorEXT.Value = !string.IsNullOrEmpty(insumo.Proveedor.Value) ? insumo.Proveedor.Value : dtoRecursoCosto.ProveedorEXT.Value;//Construdata
                                dtoRecursoCosto.ProveedorEMP.Value = insumo.Proveedor2.Value != null ? insumo.Proveedor2.Value : dtoRecursoCosto.ProveedorEMP.Value; //EmpresaActual                               
                                dtoRecursoCosto.CtrlVersion.Value = dtoRecursoCosto.CtrlVersion.Value++;
                                this._dal_MasterComplex.DocumentID = AppMasters.pyRecursoCostoBase;
                                result = this._dal_MasterComplex.DAL_MasterComplex_Update(dtoRecursoCosto, true);
                                if (result.Result == ResultValue.NOK)
                                    break; 
                            }
                            else
                            {
                                dtoRecursoCosto = new DTO_pyRecursoCostoBase();
                                dtoRecursoCosto.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                                dtoRecursoCosto.LugarGeograficoID.Value = lugGeoDef;
                                dtoRecursoCosto.RecursoID.Value = dtoRecurso.ID.Value;
                                dtoRecursoCosto.ProveedorEXT.Value = insumo.Proveedor.Value; //Construdata
                                dtoRecursoCosto.ProveedorEMP.Value = insumo.Proveedor2.Value != null ? insumo.Proveedor2.Value : insumo.Proveedor.Value; //Quamtum
                                dtoRecursoCosto.CostoLocalEXT.Value = insumo.MonedaID.Value.Equals(mdaLocal) ? insumo.Precio.Value : 0;// Construdata
                                dtoRecursoCosto.CostoLocalEMP.Value = insumo.MonedaID.Value.Equals(mdaLocal) ? (insumo.Precio2.Value != null ? insumo.Precio2.Value : insumo.Precio.Value) : 0; //Quantum
                                dtoRecursoCosto.CostoExtraEXT.Value = !insumo.MonedaID.Value.Equals(mdaLocal) ? insumo.Precio.Value : 0;// Construdata    
                                dtoRecursoCosto.CostoExtraEMP.Value = !insumo.MonedaID.Value.Equals(mdaLocal) ? (insumo.Precio2.Value != null ? insumo.Precio2.Value : insumo.Precio.Value) : 0; //Quantum;    
                                dtoRecursoCosto.ActivoInd.Value = true;
                                dtoRecursoCosto.CtrlVersion.Value = 1;
                                this._dal_MasterComplex.DocumentID = AppMasters.pyRecursoCostoBase;
                                detailResult = this._dal_MasterComplex.DAL_MasterComplex_AddItem(dtoRecursoCosto);
                                detailResult.line = i;
                                result.Details = new List<DTO_TxResultDetail>();
                                if (detailResult.DetailsFields.Count > 0)
                                    result.Details.Add(detailResult);
                                if (detailResult.Message == ResultValue.NOK.ToString())
                                {
                                    result.Result = ResultValue.NOK;
                                    break;
                                }
                            }
                            #endregion
                        }

                        #region Valida si guarda Tareas y TareaRecurso  
                        bool saveInd = false;
                        if (addTareaXRecEquipo && insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Equipo)
                            saveInd = true;
                        else if (addTareaXRecPerson && insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Personal)
                            saveInd = true;
                        else if (addTareaXRecTransporte && insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Transporte)
                            saveInd = true;
                        else if (addTareaXRecMaterial && insumo.Tipo_Grupo.Value == (byte)TipoRecurso.Insumo)
                            saveInd = true;
                        if (saveInd)
                        {
                            #region Agrega o Actualiza la Tarea (pyTarea)
                            DTO_pyTarea dtoTarea = (DTO_pyTarea)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyTarea, insumo.Codigo.Value, true, false);
                            if (dtoTarea == null)
                            {
                                dtoTarea = new DTO_pyTarea();
                                dtoTarea.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                                dtoTarea.ID.Value = insumo.Codigo.Value;
                                dtoTarea.Descriptivo.Value = !string.IsNullOrEmpty(insumo.Nombre.Value) ? insumo.Nombre.Value : dtoRecurso.Descriptivo.Value;
                                dtoTarea.CapituloTareaID.Value = !string.IsNullOrEmpty(insumo.Grupo.Value) ? insumo.Grupo.Value : string.Empty;
                                dtoTarea.LineaPresupuestoID.Value = dtoTarea.CapituloTareaID.Value;
                                dtoTarea.UnidadInvID.Value = undMedida;
                                dtoTarea.ServicioID.Value = servicioFactXDef;
                                dtoTarea.TipoTarea.Value = 1;
                                dtoTarea.EntregaIndividualInd.Value = true;
                                dtoTarea.ActivoInd.Value = true;
                                dtoTarea.CtrlVersion.Value = 1;
                                this._dal_MasterSimple.DocumentID = AppMasters.pyTarea;
                                detailResult = this._dal_MasterSimple.DAL_MasterSimple_AddItem(dtoTarea);
                                detailResult.line = i;
                                result.Details = new List<DTO_TxResultDetail>();
                                if (detailResult.DetailsFields.Count > 0)
                                    result.Details.Add(detailResult);
                                if (detailResult.Message == ResultValue.NOK.ToString())
                                {
                                    result.Result = ResultValue.NOK;
                                    break;
                                }
                            }
                            else
                            {
                                dtoTarea.Descriptivo.Value = !string.IsNullOrEmpty(insumo.Nombre.Value) ? insumo.Nombre.Value : dtoTarea.Descriptivo.Value;
                                dtoTarea.CapituloTareaID.Value = !string.IsNullOrEmpty(insumo.Grupo.Value) ? insumo.Grupo.Value : dtoTarea.CapituloTareaID.Value;
                                dtoTarea.LineaPresupuestoID.Value = dtoTarea.CapituloTareaID.Value;
                                dtoTarea.UnidadInvID.Value = !string.IsNullOrEmpty(undMedida) ? undMedida : dtoTarea.UnidadInvID.Value;
                                dtoTarea.ServicioID.Value = servicioFactXDef;
                                dtoTarea.CtrlVersion.Value = dtoTarea.CtrlVersion.Value++;
                                this._dal_MasterSimple.DocumentID = AppMasters.pyTarea;
                                result = this._dal_MasterSimple.DAL_MasterSimple_Update(dtoTarea, true);
                                if (result.Result == ResultValue.NOK)
                                    break;
                            }
                            #endregion
                            #region Agrega o Actualiza la Tarea Recurso(pyTareaRecurso)
                            Dictionary<string, string> pks = new Dictionary<string, string>();
                            pks.Add("TareaID", insumo.Codigo.Value);
                            pks.Add("RecursoID", insumo.Codigo.Value);
                            DTO_pyTareaRecurso dtoTareaRec = (DTO_pyTareaRecurso)this.GetMasterComplexDTO(AppMasters.pyTareaRecurso, pks, true);
                            if (dtoTareaRec == null)
                            {
                                dtoTareaRec = new DTO_pyTareaRecurso();
                                dtoTareaRec.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                                dtoTareaRec.TareaID.Value = insumo.Codigo.Value;
                                dtoTareaRec.RecursoID.Value = insumo.Codigo.Value;
                                dtoTareaRec.CostoBase.Value = 0;
                                dtoTareaRec.FactorID.Value = 1;
                                dtoTareaRec.ActivoInd.Value = true;
                                dtoTareaRec.CtrlVersion.Value = 1;
                                this._dal_MasterComplex.DocumentID = AppMasters.pyTareaRecurso;
                                detailResult = this._dal_MasterComplex.DAL_MasterComplex_AddItem(dtoTareaRec);
                                detailResult.line = i;
                                result.Details = new List<DTO_TxResultDetail>();
                                if (detailResult.DetailsFields.Count > 0)
                                    result.Details.Add(detailResult);
                                if (detailResult.Message == ResultValue.NOK.ToString())
                                {
                                    result.Result = ResultValue.NOK;
                                    break;
                                }
                            }
                            #endregion
                        }
                        #endregion

                        i++; 
                    } 
                }
                #endregion

                #region Valida los Grupos
                if (listGrupos != null && listGrupos.Count > 0)
                {
                    string recursoXDef = this.GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_RecursoxDef);
                    foreach (DTO_MigracionGrupos grupo in listGrupos)
                    {
                        int percent = ((i + 1) * 100) / listGrupos.Count;
                        batchProgress[tupProgress] = percent;
                        DTO_pyTareaCapitulo dtoTareaCap = (DTO_pyTareaCapitulo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyTareaCapitulo, grupo.CapituloTareaID.Value, true, false);
                        DTO_TxResultDetail detailResult;
                        if (dtoTareaCap == null)
                        {                            
                            #region Crea el nuevo Capitulo(pyCapituloTarea)
                            dtoTareaCap = new DTO_pyTareaCapitulo();
                            dtoTareaCap.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                            dtoTareaCap.ID.Value = grupo.CapituloTareaID.Value;
                            dtoTareaCap.Descriptivo.Value = grupo.Descriptivo.Value;
                            dtoTareaCap.CapituloGrupoID.Value = grupo.CapituloGrupoID.Value;
                            dtoTareaCap.ActivoInd.Value = true;
                            dtoTareaCap.CtrlVersion.Value = 1;
                            this._dal_MasterSimple.DocumentID = AppMasters.pyTareaCapitulo;
                            detailResult = this._dal_MasterSimple.DAL_MasterSimple_AddItem(dtoTareaCap);
                            detailResult.line = i;
                            result.Details = new List<DTO_TxResultDetail>();
                            if (detailResult.DetailsFields.Count > 0)
                                result.Details.Add(detailResult);
                            if (detailResult.Message == ResultValue.NOK.ToString())
                            {
                                result.Result = ResultValue.NOK;
                                break;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Actualiza el Capitulo
                            dtoTareaCap.CtrlVersion.Value = dtoTareaCap.CtrlVersion.Value++;
                            dtoTareaCap.CapituloGrupoID.Value = grupo.CapituloGrupoID.Value != null ? grupo.CapituloGrupoID.Value : dtoTareaCap.CapituloGrupoID.Value;
                            this._dal_MasterSimple.DocumentID = AppMasters.pyTareaCapitulo;
                            result = this._dal_MasterSimple.DAL_MasterSimple_Update(dtoTareaCap, true);
                            if (result.Result == ResultValue.NOK)
                                break;
                            #endregion
                        }

                        #region Crea la nueva Linea(plLineaPresupuesto)
                        DTO_plLineaPresupuesto dtoLineaPres = (DTO_plLineaPresupuesto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, grupo.CapituloTareaID.Value, true, false);
                        if (dtoLineaPres == null)
                        {
                            #region Crea la nueva linea
                            dtoLineaPres = new DTO_plLineaPresupuesto();
                            dtoLineaPres.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                            dtoLineaPres.ID.Value = grupo.CapituloTareaID.Value;
                            dtoLineaPres.Descriptivo.Value = grupo.Descriptivo.Value;
                            dtoLineaPres.ControlCosto.Value = 1;
                            dtoLineaPres.ControlEjecucionPxQ.Value = 1;
                            dtoLineaPres.TablaControlInd.Value = false;
                            dtoLineaPres.ControlCantidadPXQInd.Value = false;
                            dtoLineaPres.IngresosInd.Value = false;
                            dtoLineaPres.RecursoID.Value = recursoXDef;
                            dtoLineaPres.ActivoInd.Value = true;
                            dtoLineaPres.CtrlVersion.Value = 1;
                            this._dal_MasterSimple.DocumentID = AppMasters.plLineaPresupuesto;
                            detailResult = this._dal_MasterSimple.DAL_MasterSimple_AddItem(dtoLineaPres);
                            detailResult.line = i;
                            result.Details = new List<DTO_TxResultDetail>();
                            if (detailResult.DetailsFields.Count > 0)
                                result.Details.Add(detailResult);
                            if (detailResult.Message == ResultValue.NOK.ToString())
                            {
                                result.Result = ResultValue.NOK;
                                break;
                            } 
                            #endregion
                        }
                        else
                        {
                            #region Actualiza la linea 
                            dtoLineaPres.CtrlVersion.Value = dtoLineaPres.CtrlVersion.Value++;
                            this._dal_MasterSimple.DocumentID = AppMasters.plLineaPresupuesto;
                            result = this._dal_MasterSimple.DAL_MasterSimple_Update(dtoLineaPres, true);
                            if (result.Result == ResultValue.NOK)
                                break;
                            #endregion
                        }
                        #endregion
                        #region Crea el nuevo inGrupo(inRefGrupo)
                        DTO_inRefGrupo dtoRefGrupo = (DTO_inRefGrupo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefGrupo, grupo.CapituloTareaID.Value, true, false);
                        if (dtoRefGrupo == null)
                        {
                            #region Crea el nuevo Grupo
                            dtoRefGrupo = new DTO_inRefGrupo();
                            dtoRefGrupo.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                            dtoRefGrupo.ID.Value = grupo.CapituloTareaID.Value;
                            dtoRefGrupo.Descriptivo.Value = grupo.Descriptivo.Value;
                            dtoRefGrupo.LineaPresupuestoID.Value = grupo.CapituloTareaID.Value;
                            dtoRefGrupo.ActivoInd.Value = true;
                            dtoRefGrupo.CtrlVersion.Value = 1;
                            this._dal_MasterSimple.DocumentID = AppMasters.inRefGrupo;
                            detailResult = this._dal_MasterSimple.DAL_MasterSimple_AddItem(dtoRefGrupo);
                            detailResult.line = i;
                            result.Details = new List<DTO_TxResultDetail>();
                            if (detailResult.DetailsFields.Count > 0)
                                result.Details.Add(detailResult);
                            if (detailResult.Message == ResultValue.NOK.ToString())
                            {
                                result.Result = ResultValue.NOK;
                                break;
                            } 
                            #endregion
                        }
                        else
                        {
                            #region Actualiza el Grupo
                            dtoRefGrupo.CtrlVersion.Value = dtoRefGrupo.CtrlVersion.Value++;
                            dtoRefGrupo.LineaPresupuestoID.Value = grupo.CapituloTareaID.Value;
                            this._dal_MasterSimple.DocumentID = AppMasters.inRefGrupo;
                            result = this._dal_MasterSimple.DAL_MasterSimple_Update(dtoRefGrupo, true);
                            if (result.Result == ResultValue.NOK)
                                break;
                            #endregion
                        }
                        #endregion 
                        i++;
                    }
                }
                #endregion

                #region Valida los APU
                if (listAPU != null && listAPU.Count > 0)
                {
                    foreach (DTO_MigracionAPU apu in listAPU)
                    {
                        int percent = ((i + 1) * 100) / listAPU.Count;
                        batchProgress[tupProgress] = percent;                      
                        Dictionary<string, string> pks = new Dictionary<string, string>();
                        pks.Add("TareaID", apu.TareaID.Value);
                        pks.Add("RecursoID", apu.RecursoID.Value);
                        DTO_pyTareaRecurso dtoTrabajoRec = (DTO_pyTareaRecurso)this.GetMasterComplexDTO(AppMasters.pyTareaRecurso, pks, true);
                        DTO_TxResultDetail detailResult;
                        if (dtoTrabajoRec == null)
                        {
                            #region Agrega un APU
                            dtoTrabajoRec = new DTO_pyTareaRecurso();
                            dtoTrabajoRec.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                            dtoTrabajoRec.TareaID.Value = apu.TareaID.Value;
                            dtoTrabajoRec.RecursoID.Value = apu.RecursoID.Value;
                            dtoTrabajoRec.FactorID.Value = apu.Factor.Value;
                            dtoTrabajoRec.ActivoInd.Value = true;
                            dtoTrabajoRec.CtrlVersion.Value = 1;
                            this._dal_MasterComplex.DocumentID = AppMasters.pyTareaRecurso;
                            detailResult = this._dal_MasterComplex.DAL_MasterComplex_AddItem(dtoTrabajoRec);
                            detailResult.line = i;
                            result.Details = new List<DTO_TxResultDetail>();
                            if (detailResult.DetailsFields.Count > 0)
                                result.Details.Add(detailResult);
                            if (detailResult.Message == ResultValue.NOK.ToString())
                            {
                                //result.Result = ResultValue.NOK;
                                //break;
                            } 
                            #endregion
                        }
                        else
                        {  
                            #region Actualiza el APU
                            dtoTrabajoRec.CtrlVersion.Value = dtoTrabajoRec.CtrlVersion.Value++;
                            this._dal_MasterSimple.DocumentID = AppMasters.pyTareaRecurso;
                            result = this._dal_MasterComplex.DAL_MasterComplex_Update(dtoTrabajoRec, true);
                            if (result.Result == ResultValue.NOK)
                                break;
                            #endregion
                        }
                        i++;
                    }
                }
                #endregion

                batchProgress[tupProgress] = 100;
                return result;
            }
            catch (Exception ex)
            {
                batchProgress[tupProgress] = 100;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "MigracionInsumos");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    base._mySqlConnectionTx.Commit();
                    base._mySqlConnectionTx = null;
                    this._moduloGlobal._mySqlConnectionTx = null;
                }
                else if (base._mySqlConnectionTx != null && base._mySqlConnectionTx.Connection != null)
                    base._mySqlConnectionTx.Rollback();
            }
        }
        
        #endregion

        #region Reportes

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
                this._dal_pyReportesProyecto = (DAL_ReportesProyectos)this.GetInstance(typeof(DAL_ReportesProyectos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_pyReportesProyecto.DAL_ReportesProyectos_Cumplimiento(FechaCorte, Proyecto, Estado, LineaFlujo, Etapa);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReportesProyectos_Cumplimiento");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que se encarga de traer la informacion del Presupuesto que se requiere para cada proyecto
        /// </summary>
        /// <param name="Periodo">Perido a presupuestar</param>
        /// <param name="Proyecto">Filtra un proyecto especifico a verificar</param>
        /// <returns>Tabla con el presupuesto</returns>
        public DataTable ReportesProyectos_Presupuesto(DateTime Periodo, string Proyecto)
        {
            try
            {
                this._dal_pyReportesProyecto = (DAL_ReportesProyectos)this.GetInstance(typeof(DAL_ReportesProyectos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_pyReportesProyecto.DAL_ReportesProyectos_Presupuesto(Periodo, Proyecto);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReportesProyectos_Presupuesto");
                throw exception;
            }
        }

     
        #endregion    
    }
}