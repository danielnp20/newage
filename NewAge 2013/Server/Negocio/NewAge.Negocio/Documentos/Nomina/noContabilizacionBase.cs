using NewAge.ADO;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NewAge.Negocio
{
    public abstract class noContabilizacionBase : ModuloBase
    {
        #region Public Variables

        public List<DTO_TxResult> Results = new List<DTO_TxResult>();
        public string terceroPorDefecto = string.Empty;
        public string docContable = string.Empty;
        public string lineaPres = string.Empty;
        public string cCosto = string.Empty;
        public string proyectoXDef = string.Empty;
        public string lugarGeografico = string.Empty;
        public string conceptoCxP = string.Empty;
        public string prefijDef = string.Empty;
        public string concCargoDef = string.Empty;
        public ModuloGlobal _moduloGlobal = null;
        public ModuloContabilidad _moduloContabilidad = null;
        public DAL_noLiquidacionesDocu _dal_noLiquidacionesDocu = null;
        public DAL_noLiquidacionesDetalle _dal_noLiquidacionesDetalle = null;
        public DAL_noPlanillaAportesDeta _dal_noPlanillaAportesDeta = null;
        public DAL_noProvisionDeta _dal_noProvisionDeta = null;
        private DTO_coComprobante comp;

      
        public string mdaLoc;
        public string mdaExt;
        public decimal tc;

        #endregion

        #region Private Variables

        private DateTime periodo;
        private DateTime fechaDoc;
        private int documentoId;
        private DateTime today;
        private List<DTO_NominaContabilizacionDetalle> liquidaciones;
        private DTO_noEmpleado empleado;

        #endregion

        #region Properties

        public DateTime Periodo
        {
            get { return periodo; }
            set { periodo = value; }
        }

        public DateTime FechaDoc
        {
            get { return fechaDoc; }
            set { fechaDoc = value; }
        }

        public int DocumentoId
        {
            get { return documentoId; }
            set { documentoId = value; }
        }

        public List<DTO_NominaContabilizacionDetalle> Liquidaciones
        {
            get { return liquidaciones; }
            set { liquidaciones = value; }
        }

        public DTO_noEmpleado Empleado
        {
            get { return empleado; }
            set { empleado = value; }
        }

        public DTO_coComprobante Comp
        {
            get { return comp; }
            set { comp = value; }
        }

        #endregion
        
        /// <summary>
        /// Constructor 
        /// </summary>

        public noContabilizacionBase(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) 
        {
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this.today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            this.mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            this.terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
            this.docContable = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_DocumContableLiquidaciones);
            this.lineaPres = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
            this.cCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            this.proyectoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            this.lugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
            this.conceptoCxP = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoCxPagosNomina);
            this.prefijDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
            this.concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
            this.tc = this._moduloGlobal.TasaDeCambio_Get(mdaExt, today);

        }

        #region Base Methods

        /// <summary>
        /// Obtiene el detalle de la liquidaciones de Nomina
        /// </summary>
        /// <returns></returns>
        public DTO_noNominaDefinitiva GetLiquidacionesDetalle(int docId)
        {           
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionesDetalle = (DAL_noLiquidacionesDetalle)this.GetInstance(typeof(DAL_noLiquidacionesDetalle), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            DTO_glDocumentoControl docControl = this._moduloGlobal.glDocumentoControl_GetDocEmpleado(docId, this.Empleado.TerceroID.Value, this.Periodo);
            DTO_noNominaDefinitiva liqCompleta = new DTO_noNominaDefinitiva(docControl);
            if (liqCompleta.DocControl != null)
            {

                liqCompleta.DocLiquidacion = this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_GetByNumeroDoc(liqCompleta.DocControl.NumeroDoc.Value.Value);
                liqCompleta.Detalle = this._dal_noLiquidacionesDetalle.DAL_noLiquidacionesDetalle_Get(liqCompleta.DocControl.NumeroDoc.Value.Value);
            }
            return liqCompleta;
        }

        /// <summary>
        /// Proceso para Generar los consecutivos de los documentos
        /// </summary>
        /// <returns></returns>
        public void GenerarConsecutivos(int numDoc, int documentoId, DTO_coComprobante coComp)
        {
            DTO_glDocumentoControl docControl = null;
            docControl = this._moduloGlobal.glDocumentoControl_GetByID(numDoc);
            docControl.DocumentoNro.Value = Convert.ToInt32(this.GenerarDocumentoNro(documentoId, docControl.PrefijoID.Value));
            int comprobanteNro = this.GenerarComprobanteNro(coComp, docControl.PrefijoID.Value, docControl.PeriodoDoc.Value.Value, docControl.DocumentoNro.Value.Value);
            this._moduloGlobal.ActualizaConsecutivos(docControl, true, true, false);
            this._moduloContabilidad.ActualizaComprobanteNro(docControl.NumeroDoc.Value.Value, comprobanteNro, false, coComp.ID.Value);
         }

        #endregion

        public abstract List<DTO_TxResult> Contabilizar();
    }
}
