using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using SentenceTransformer;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para obtener la cuenta de la tabla de cargos 
    /// </summary>
    public partial class DigitacionCuentas : Form
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        private int _documentID;
        private bool _multiMoneda;
        private int _tipoMoneda;
        private DTO_glConceptoSaldo conceptoSaldo = null;
        private DTO_ComprobanteFooter _data;
        public DTO_ComprobanteFooter ReturnData;

        private string conceptoCargoId = string.Empty;
        private string proyectoId = string.Empty;
        private string centroCostoId = string.Empty;
        private string lineaPreId = string.Empty;
        private string operacionId = string.Empty;
        private string lugarGeoId = string.Empty;
        private string cuentaId = string.Empty;

        private string defProyecto = string.Empty;
        private string defCentroCosto = string.Empty;
        private string defLineaPresupuesto = string.Empty;
        private string defLugarGeo = string.Empty;

        private decimal _impuesto = 0;
        #endregion

        #region Propiedades
        private DTO_coPlanCuenta _cuenta = null;
        private DTO_coPlanCuenta Cuenta
        {
            get
            {
                return _cuenta;
            }
            set
            {
                this._cuenta = value;

                if (value == null)
                {
                    this.tlpValores.Enabled = false;

                    this.cuentaId = string.Empty;
                    this.conceptoSaldo = null;
                    this.masterCuenta.Value = string.Empty;
                }
                else
                {
                    this.cuentaId = this._cuenta.ID.Value;
                    this.masterCuenta.Value = this.cuentaId;
                    this.conceptoSaldo = (DTO_glConceptoSaldo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, this._cuenta.ConceptoSaldoID.Value, true);

                    #region Asigna los parametros segun la cuenta

                    //Asigna el centro de costo
                    if (this._cuenta.CentroCostoInd.Value.Value)
                    {
                        this.masterCentroCosto.EnableControl(true);
                    }
                    else
                    {
                        this.masterCentroCosto.EnableControl(false);
                        this.masterCentroCosto.Value = this.defCentroCosto;
                    }

                    //Asigna el proyecto
                    if (this._cuenta.ProyectoInd.Value.Value)
                    {
                        this.masterProyecto.EnableControl(true);
                    }
                    else
                    {
                        this.masterProyecto.EnableControl(false);
                        this.masterProyecto.Value = this.defProyecto;
                    }

                    //Asigna el Lugar geografico
                    if (this._cuenta.LugarGeograficoInd.Value.Value)
                    {
                        this.masterLugarGeo.EnableControl(true);
                    }
                    else
                    {
                        this.masterLugarGeo.EnableControl(false);
                        this.masterLugarGeo.Value = this.defLugarGeo;
                    }

                    //Asigna el Linea Presupuesto
                    if (this._cuenta.LineaPresupuestalInd.Value.Value)
                    {
                        this.masterLineaPre.EnableControl(true);
                    }
                    else
                    {
                        this.masterLineaPre.EnableControl(false);
                        this.masterLineaPre.Value = this.defLineaPresupuesto;
                    }
                    #endregion
                    #region Habilita los campos de los valores segun parametros de la cuenta
                    this.tlpValores.Enabled = true;

                    this.tbBaseML.Enabled = true;
                    this.tbValorML.Enabled = true;
                    this.tbBaseME.Enabled = true;
                    this.tbValorME.Enabled = true;

                    if (string.IsNullOrEmpty(this._cuenta.ImpuestoTipoID.Value))
                    {
                        this._impuesto = 0;
                        this.tbBaseML.Enabled = false;
                        this.tbBaseME.Enabled = false;
                    }
                    else
                    {
                        this._impuesto = this._cuenta.ImpuestoPorc != null && this._cuenta.ImpuestoPorc.Value.HasValue ? this._cuenta.ImpuestoPorc.Value.Value : 0;
                        this.tbValorML.Enabled = false;
                        this.tbValorME.Enabled = false;
                    }

                    if (!this._multiMoneda && this._tipoMoneda == (int)TipoMoneda.Local)
                    {
                        this.tbBaseME.Enabled = false;
                        this.tbValorME.Enabled = false;
                    }

                    if (!this._multiMoneda && this._tipoMoneda == (int)TipoMoneda.Foreign)
                    {
                        this.tbBaseML.Enabled = false;
                        this.tbValorML.Enabled = false;
                    }
                    #endregion
                }
            }
        }

        private DTO_coConceptoCargo _conceptoCargo = null;
        private DTO_coConceptoCargo ConceptoCargo
        {
            get
            {
                return _conceptoCargo;
            }
            set
            {
                this._conceptoCargo = value;

                if (value != null)
                {
                    if (!string.IsNullOrEmpty(this.ConceptoCargo.CuentaID.Value))
                    {
                        if (string.IsNullOrEmpty(this.cuentaId))
                            this.cuentaId = this.ConceptoCargo.CuentaID.Value;

                        this.Cuenta = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, this.cuentaId, true);
                    }
                    else
                    {
                        this.masterProyecto.EnableControl(true);
                        this.masterCentroCosto.EnableControl(true);

                        if (!string.IsNullOrEmpty(this.masterCentroCosto.Value))
                            this.CentroCosto = (DTO_coCentroCosto)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, false, this.masterCentroCosto.Value, true);

                        if (!string.IsNullOrEmpty(this.masterProyecto.Value))
                            this.Proyecto = (DTO_coProyecto)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, false, this.masterProyecto.Value, true);
                    }
                }
                else
                {
                    this.conceptoCargoId = string.Empty;
                    this.masterConceptoCargo.Value = string.Empty;
                    this.Cuenta = null;
                }
            }
        }

        private DTO_coProyecto _proyecto = null;
        private DTO_coProyecto Proyecto
        {
            get
            {
                return _proyecto;
            }
            set
            {
                _proyecto = value;
                if (value != null)
                {
                    this.operacionId = string.Empty;
                    //this.masterCentroCosto.EnableControl(true);

                    if (!string.IsNullOrEmpty(this.Proyecto.OperacionID.Value))
                        this.Operacion = (DTO_coOperacion)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coOperacion, false, this.Proyecto.OperacionID.Value, true);
                }
                else
                {
                    this.proyectoId = string.Empty;
                    this.masterProyecto.Value = string.Empty;

                    if (this.ConceptoCargo != null && string.IsNullOrEmpty(this.ConceptoCargo.CuentaID.Value) 
                        && this.CentroCosto != null && !string.IsNullOrEmpty(this.CentroCosto.OperacionID.Value))
                        this.Operacion = (DTO_coOperacion)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coOperacion, false, this.CentroCosto.OperacionID.Value, true);
                    else
                        this.Operacion = null;
                }
            }
        }

        private DTO_coCentroCosto _centroCosto = null;
        private DTO_coCentroCosto CentroCosto
        {
            get
            {
                return _centroCosto;
            }
            set
            {
                _centroCosto = value;
                if (value != null)
                {
                    if (string.IsNullOrEmpty(this.operacionId.Trim()) && !string.IsNullOrEmpty(this.CentroCosto.OperacionID.Value))
                        this.Operacion = (DTO_coOperacion)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coOperacion, false, this.CentroCosto.OperacionID.Value, true);
                }
                else
                {
                    this.centroCostoId = string.Empty;
                    this.masterCentroCosto.Value = string.Empty;

                    if (this.ConceptoCargo != null && !string.IsNullOrEmpty(this.ConceptoCargo.CuentaID.Value) 
                        && this.Proyecto != null && !string.IsNullOrEmpty(this.Proyecto.OperacionID.Value))
                        this.Operacion = (DTO_coOperacion)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coOperacion, false, this.Proyecto.OperacionID.Value, true);
                    else
                        this.Operacion = null;
                }
            }
        }

        private DTO_coOperacion _operacion = null;
        private DTO_coOperacion Operacion
        {
            get
            {
                return _operacion;
            }
            set
            {
                _operacion = value;
                if (value != null)
                {
                    this.operacionId = this._operacion.ID.Value;
                    this.masterLineaPre.EnableControl(true);

                    if (!string.IsNullOrEmpty(this.masterLineaPre.Value))
                        this.LineaPre = (DTO_plLineaPresupuesto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, this.masterLineaPre.Value, true);
                }
                else
                {
                    this.operacionId = string.Empty;
                    if (this.ConceptoCargo != null && string.IsNullOrEmpty(this.ConceptoCargo.CuentaID.Value))
                        this.Cuenta = null;
                }
            }
        }

        private DTO_plLineaPresupuesto _lineaPre = null;
        private DTO_plLineaPresupuesto LineaPre
        {
            get
            {
                return _lineaPre;
            }
            set
            {
                _lineaPre = value;
                string lineaPreTemp = string.Empty;
                if (value != null)
                {
                    lineaPreTemp = this.LineaPre.TablaControlInd.Value.Value ? this.defLineaPresupuesto : this.LineaPre.ID.Value;

                    if (this.ConceptoCargo!=null && string.IsNullOrEmpty(this.ConceptoCargo.CuentaID.Value))
                    {
                        if (!string.IsNullOrEmpty(this.conceptoCargoId) && !string.IsNullOrEmpty(this.operacionId))
                        {
                            long count = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.coCargoCosto, null, null);
                            List<DTO_coCargoCosto> complexCargo = this._bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.coCargoCosto, count, 1, null, null).Cast<DTO_coCargoCosto>().ToList();

                            List<string> cuentas = (from data in complexCargo
                                                    where data.ConceptoCargoID.Value == this.conceptoCargoId && data.OperacionID.Value == this.operacionId && data.LineaPresupuestoID.Value == lineaPreTemp
                                                    select data.CuentaID.Value).ToList();
                            if (cuentas.Count < 1)
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CtaCargoCostoNoExiste));
                                this.Cuenta = null;
                            }
                            else if (cuentas.Count > 1)
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CtaCargoCostoNoUnica));
                                this.Cuenta = null;
                            }
                            else
                            {
                                this.cuentaId = cuentas[0];
                                this.Cuenta = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, this.cuentaId, true);
                            }
                        }
                    }
                }
                else
                {
                    this.lineaPreId = string.Empty;
                    this.masterLineaPre.Value = string.Empty;

                    if (this.ConceptoCargo != null && string.IsNullOrEmpty(this.ConceptoCargo.CuentaID.Value))
                        this.Cuenta = null;
                }
            }
        }
        #endregion

        public DigitacionCuentas(DTO_ComprobanteFooter data, bool multiMoneda, int tipoMoneda)
        {
            InitializeComponent();

            this._documentID = AppForms.DigitacionCuentas; 
            this._data = data;
            this._multiMoneda = multiMoneda;
            this._tipoMoneda = tipoMoneda;

            //Carga los valores por defecto
            this.defProyecto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            this.defCentroCosto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            this.defLineaPresupuesto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
            this.defLugarGeo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);

            #region Iniciate masterControls
            this._bc.InitMasterUC(this.masterConceptoCargo, AppMasters.coConceptoCargo, true, true, true, false);
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
            this._bc.InitMasterUC(this.masterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            this._bc.InitMasterUC(this.masterLineaPre, AppMasters.plLineaPresupuesto, true, true, true, false);
            this._bc.InitMasterUC(this.masterLugarGeo, AppMasters.glLugarGeografico, true, true, true, false);
            this._bc.InitMasterUC(this.masterCuenta, AppMasters.coPlanCuenta, true, true, true, false);
            #endregion
            #region Enable Controls
            this.masterConceptoCargo.EnableControl(true);
            this.masterProyecto.EnableControl(false);
            this.masterCentroCosto.EnableControl(false);
            this.masterLineaPre.EnableControl(false);
            this.masterLugarGeo.EnableControl(false);
            this.masterCuenta.EnableControl(false);
            this.masterConceptoCargo.Focus();

            if (!this._multiMoneda && this._tipoMoneda == (int)TipoMoneda.Local)
            {
                this.tlpValores.RowStyles[2].Height = 0;
                this.tlpValores.RowStyles[3].Height = 0;
                this.tlpValores.Height = 40;
            }
            else if (!this._multiMoneda && this._tipoMoneda == (int)TipoMoneda.Foreign)
            {
                this.tlpValores.RowStyles[0].Height = 0;
                this.tlpValores.RowStyles[1].Height = 0;
                this.tlpValores.Height = 40;
            }
            else
            {
                this.tlpValores.RowStyles[0].Height = 20;
                this.tlpValores.RowStyles[1].Height = 20;
                this.tlpValores.RowStyles[2].Height = 20;
                this.tlpValores.RowStyles[3].Height = 20;
                this.tlpValores.Height = 80;
            }

            this.tlpValores.Enabled = false;
            #endregion

            FormProvider.LoadResources(this, this._documentID);
            this.LoadData();
        }

        #region Funciones privadas

        /// <summary>
        /// Cargar Datos por defecto
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.conceptoCargoId = this._data.ConceptoCargoID.Value;
                this.proyectoId = this._data.ProyectoID.Value;
                this.centroCostoId = this._data.CentroCostoID.Value;
                this.lineaPreId = this._data.LineaPresupuestoID.Value;
                this.lugarGeoId = this._data.LugarGeograficoID.Value;
                this.cuentaId = string.IsNullOrEmpty(this._data.CuentaID.Value) ? string.Empty : this._data.CuentaID.Value;

                this.masterConceptoCargo.Value = this._data.ConceptoCargoID.Value;
                this.masterProyecto.Value = this._data.ProyectoID.Value;
                this.masterCentroCosto.Value = this._data.CentroCostoID.Value;
                this.masterLineaPre.Value = this._data.LineaPresupuestoID.Value;
                this.masterLugarGeo.Value = this._data.LugarGeograficoID.Value;
                this.masterCuenta.Value = this.cuentaId;

                this.tbBaseML.EditValue = 0;
                this.tbValorML.EditValue = 0;
                this.tbBaseME.EditValue = 0;
                this.tbValorME.EditValue = 0;

                this.ConceptoCargo = (DTO_coConceptoCargo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coConceptoCargo, false, this.conceptoCargoId, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCuentas.cs", "LoadData"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que se ejecuta al salir del control de concepto cargo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterConceptoCargo_Leave(object sender, EventArgs e)
        {
            try
            {
                //this._terceroFocus = false;
                ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;

                if (master.ValidID)
                {
                    if (string.IsNullOrWhiteSpace(this.conceptoCargoId) || master.Value != this.conceptoCargoId)
                    {
                        this.cuentaId = string.Empty;
                        this.masterCuenta.Value = this.cuentaId;
                        this.conceptoCargoId = master.Value;
                        this.ConceptoCargo = (DTO_coConceptoCargo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coConceptoCargo, false, this.conceptoCargoId, true);
                    }
                }
                else
                {
                    //this.conceptoCargoId = string.Empty;
                    this.ConceptoCargo = null;
                    this.Proyecto = null;
                    this.CentroCosto = null;
                    this.LineaPre = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCuentas.cs", "masterConceptoCargo_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control de proyecto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterProyecto_Leave(object sender, EventArgs e)
        {
            try
            {
                //this._terceroFocus = false;
                ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;

                if (master.ValidID)
                {
                    if (string.IsNullOrWhiteSpace(this.proyectoId) || master.Value != this.proyectoId)
                    {
                        this.proyectoId = master.Value;
                        this.Proyecto = (DTO_coProyecto)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, false, this.proyectoId, true);
                    }
                }
                else
                {
                    //this.proyectoId = string.Empty;
                    this.Proyecto = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCuentas.cs", "masterProyecto_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control de CentroCosto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterCentroCosto_Leave(object sender, EventArgs e)
        {
            try
            {
                //this._terceroFocus = false;
                ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;

                if (master.ValidID)
                {
                    if (string.IsNullOrWhiteSpace(this.centroCostoId) || master.Value != this.centroCostoId)
                    {
                        this.centroCostoId = master.Value;
                        this.CentroCosto = (DTO_coCentroCosto)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, false, this.centroCostoId, true);
                    }
                }
                else
                    this.CentroCosto = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCuentas.cs", "masterCentroCosto_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control de LineaPre
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterLineaPre_Leave(object sender, EventArgs e)
        {
            try
            {
                //this._terceroFocus = false;
                ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;

                if (master.ValidID)
                {
                    if (string.IsNullOrWhiteSpace(this.lineaPreId) || master.Value != this.lineaPreId)
                    {
                        this.lineaPreId = master.Value;
                        this.LineaPre = (DTO_plLineaPresupuesto)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, this.lineaPreId, true);
                    }
                }
                else
                {
                    //this.lineaPreId = string.Empty;
                    this.LineaPre = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCuentas.cs", "masterLineaPre_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control de LugarGeo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterLugarGeo_Leave(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCuentas.cs", "masterLugarGeo_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo BaseML 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void tbBaseML_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbBaseML.Text))
                this.tbBaseML.EditValue = 0;

            decimal imp = Convert.ToDecimal(this.tbBaseML.EditValue, CultureInfo.InvariantCulture) * this._impuesto / 100;

            if (this.Cuenta.Naturaleza.Value.Value == (short)NaturalezaCuenta.Credito)
                imp *= -1;

            this.tbValorML.EditValue = imp;
            //validField = this.CalcularValorExtra();
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo BaseME 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void tbBaseME_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbBaseME.Text))
                this.tbBaseME.EditValue = 0;

            decimal imp = Convert.ToDecimal(this.tbBaseME.EditValue, CultureInfo.InvariantCulture) * this._impuesto / 100;

            if (this.Cuenta.Naturaleza.Value.Value == (short)NaturalezaCuenta.Credito)
                imp *= -1;

            this.tbValorME.EditValue = imp;
        }

        /// <summary>
        /// Evento que se ejecuta al hacer click al boton
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (this.masterCuenta.ValidID)
            {
                this.ReturnData = new DTO_ComprobanteFooter();
                this.ReturnData.CentroCostoID.Value = this.masterCentroCosto.Value;
                this.ReturnData.ConceptoCargoID.Value = this.masterConceptoCargo.Value;
                this.ReturnData.ConceptoSaldoID.Value = this.Cuenta.ConceptoSaldoID.Value;
                this.ReturnData.CuentaID.Value = this.masterCuenta.Value;
                this.ReturnData.LineaPresupuestoID.Value = this.masterLineaPre.Value;
                this.ReturnData.LugarGeograficoID.Value = this.masterLugarGeo.Value;
                this.ReturnData.ProyectoID.Value = this.masterProyecto.Value;

                this.ReturnData.vlrBaseML.Value = Convert.ToDecimal(this.tbBaseML.EditValue, CultureInfo.InvariantCulture);
                this.ReturnData.vlrBaseME.Value = Convert.ToDecimal(this.tbBaseME.EditValue, CultureInfo.InvariantCulture);
                this.ReturnData.vlrMdaLoc.Value = Convert.ToDecimal(this.tbValorML.EditValue, CultureInfo.InvariantCulture);
                this.ReturnData.vlrMdaExt.Value = Convert.ToDecimal(this.tbValorME.EditValue, CultureInfo.InvariantCulture);
            }
            else
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterCuenta.CodeRsx);
                MessageBox.Show(msg);
                this.ReturnData = null;
            }

            this.Close();
        }
       
        #endregion
    }
}
