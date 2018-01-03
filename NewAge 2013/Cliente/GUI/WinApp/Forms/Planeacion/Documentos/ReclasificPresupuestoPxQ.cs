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
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.Cliente.GUI.WinApp.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using SentenceTransformer;
using NewAge.DTO.Resultados;
using NewAge.DTO.Attributes;
using DevExpress.XtraEditors.Controls;
using System.Globalization;
using System.IO;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ReclasificPresupuestoPxQ : DocumentPresupuestoPxQ
    {
        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public ReclasificPresupuestoPxQ()
        {
            try
            {
                //InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificPresupuestoPxQ.cs", "ReclasificPresupuestoPxQ"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            try
            {
                this.documentID = AppDocuments.ReclasifPresupuestoPxQ;
                this._frmModule = ModulesPrefix.pl;

                base.SetInitParameters();
                this.format = _bc.GetImportExportFormat(typeof(DTO_plPresupuestoDeta), this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificPresupuestoPxQ.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Carga la info del formulario
        /// </summary>
        protected override void LoadData()
        {
            try
            {
                #region Valida el Tipo de Proyecto
                if (Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Capex || Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Inversion)
                {
                    if (this.masterProyecto.ValidID)
                    {
                        #region Valida el Proyecto/Campo/Contrato
                        DTO_coProyecto proy = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, this.masterProyecto.Value, true);
                        if (string.IsNullOrEmpty(proy.LocFisicaID.Value))
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeIncompleteFK);
                            MessageBox.Show(string.Format(msg, this.masterProyecto.CodeRsx, "Loc. Física"));
                            this.validHeader = false;
                            return;
                        }
                        DTO_glLocFisica locFisica = (DTO_glLocFisica)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLocFisica, false, proy.LocFisicaID.Value, true);
                        if (locFisica != null && string.IsNullOrEmpty(locFisica.AreaFisica.Value))
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeIncompleteFK);
                            MessageBox.Show(string.Format(msg, "Loc. Física del Proyecto", this.masterCampo.CodeRsx));
                            this.validHeader = false;
                            return;
                        }
                        DTO_glAreaFisica campo = (DTO_glAreaFisica)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFisica, false, locFisica.AreaFisica.Value, true);
                        if (campo != null && string.IsNullOrEmpty(campo.ContratoID.Value))
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeIncompleteFK);
                            MessageBox.Show(string.Format(msg, this.masterCampo.CodeRsx, "Contrato"));
                            this.validHeader = false;
                            return;
                        }
                        if (string.IsNullOrEmpty(proy.ActividadID.Value))
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeIncompleteFK);
                            MessageBox.Show(string.Format(msg, this.masterProyecto, this.masterActividad.CodeRsx));
                            this.validHeader = false;
                            return;
                        }

                        this.locFisicaID = proy.LocFisicaID.Value;
                        this.areaFisicaID = locFisica.AreaFisica.Value;
                        this.masterCampo.Value = this.areaFisicaID;
                        this.masterContrato.Value = campo.ContratoID.Value;
                        this.actividadID = proy.ActividadID.Value;
                        this.masterActividad.Value = this.actividadID;
                        #endregion
                    }
                    else
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeInvalid);
                        MessageBox.Show(string.Format(msg, this.masterProyecto.LabelRsx, this.masterProyecto.Value));
                        FormProvider.Master.itemSave.Enabled = false;
                        this.validHeader = false;
                        return;
                    }
                }
                else
                {
                    if (this.masterContrato.ValidID)
                    {
                        #region Valida el Campo/Actividad
                        if (!string.IsNullOrEmpty(this.masterCampo.Value) && !this.masterCampo.ValidID)
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeInvalid);
                            MessageBox.Show(string.Format(msg, this.masterCampo.LabelRsx, this.masterCampo.Value));
                            this.validHeader = false;
                            return;
                        }
                        if (!string.IsNullOrEmpty(this.masterActividad.Value) && !this.masterActividad.ValidID)
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeInvalid);
                            MessageBox.Show(string.Format(msg, this.masterActividad.LabelRsx, this.masterActividad.Value));
                            this.validHeader = false;
                            return;
                        }
                        this.areaFisicaID = this.masterCampo.Value;
                        this.actividadID = this.masterActividad.Value;
                        #endregion
                    }
                    else
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeInvalid);
                        MessageBox.Show(string.Format(msg, this.masterContrato.LabelRsx, this.masterContrato.Value));
                        FormProvider.Master.itemSave.Enabled = false;
                        this.validHeader = false;
                        return;
                    }
                }
                #endregion

                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);

                //Carga Presupuesto PxQ
                this.presupuesto = this._bc.AdministrationModel.PresupuestoPxQ_GetPresupuestoPxQConsolidado(AppDocuments.Presupuesto, this.masterProyecto.Value, this.periodo,
                                        Convert.ToByte(this.cmbTipoProyecto.EditValue), this.masterContrato.Value, this.masterActividad.Value, this.masterCampo.Value, (byte)EstadoDocControl.Aprobado);

                if (this.presupuesto == null)
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_PresupuestoNotExist));
                    FormProvider.Master.itemSave.Enabled = false;
                    this.validHeader = false;
                    return;
                }
                else
                {
                    if (this.presupuesto.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EstateInvalid));
                        FormProvider.Master.itemSave.Enabled = false;
                        this.validHeader = false;
                        return;
                    }
                    else
                        FormProvider.Master.itemSendtoAppr.Enabled = true;
                    #region Carga el cabezote
                    this.proyectoID = this.masterProyecto.Value;
                    this.dtPeriod.DateTime = this.periodo;
                    this.txtTasaCambio.EditValue = this.presupuesto.DocCtrl.TasaCambioDOCU.Value.Value;
                    this.numeroDocPresup = this.presupuesto.DocCtrl.NumeroDoc.Value.Value;
                    #endregion
                    #region Carga Detalle(Verifica si ya existe reclasif)
                    DTO_Presupuesto reclasifExist = this._bc.AdministrationModel.Presupuesto_GetNuevo(AppDocuments.ReclasifPresupuesto, this.masterProyecto.Value, this.periodo);
                    if (reclasifExist == null || reclasifExist.DocCtrl.Estado.Value == (byte)EstadoDocControl.Aprobado || reclasifExist.DocCtrl.Estado.Value == (byte)EstadoDocControl.Anulado || reclasifExist.DocCtrl.Estado.Value == (byte)EstadoDocControl.Revertido)
                    {
                        List<DTO_plPresupuestoPxQDeta> listDistinct = new List<DTO_plPresupuestoPxQDeta>();
                        List<string> distinctAreaFisica = (from c in this.presupuesto.DetallesPxQ select c.AreaFisicaID.Value).Distinct().ToList();
                        #region Distinct x AreaFisica
                        foreach (string area in distinctAreaFisica)
                        {
                            DTO_plPresupuestoPxQDeta deta = new DTO_plPresupuestoPxQDeta(true);
                            deta.AreaFisicaID.Value = area;
                            deta.AreaFisicaDesc.Value = this.presupuesto.DetallesPxQ.Find(x => x.AreaFisicaID.Value == area).AreaFisicaDesc.Value;
                            deta.PresupuestoLoc.Value = this.presupuesto.DetallesPxQ.FindAll(x => x.AreaFisicaID.Value == area).Sum(x => x.ValorUniLoc.Value * x.CantidadPRELoc.Value);
                            deta.PresupuestoExt.Value = this.presupuesto.DetallesPxQ.FindAll(x => x.AreaFisicaID.Value == area).Sum(x => x.ValorUniExt.Value * x.CantidadPREExt.Value);
                            deta.Detalle.AddRange(this.presupuesto.DetallesPxQ.Where(x => x.AreaFisicaID.Value == area));
                            deta.Detalle = deta.Detalle.OrderBy(x => x.RecursoID.Value).ToList();
                            listDistinct.Add(deta);
                        }
                        #endregion
                        this.detListFinal = listDistinct.OrderBy(x => x.AreaFisicaID.Value).ToList();
                        this.proyectoID = this.presupuesto.DocCtrl.ProyectoID.Value;
                    }
                    #endregion

                    this.dtPeriod.Enabled = false;
                    this.masterProyecto.EnableControl(false);
                }

                this.validHeader = true;
                this.isValid_Det = true;
                this.LoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "LoadData"));
            }
        }
    }
}
