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
    public partial class RevertirCartera : DocumentReversiones
    {       
        public RevertirCartera()
            : base()
        {
            //InitializeComponent();
        }

        public RevertirCartera(string mod)
            : base(mod)
        {
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicializa los parametros de la pantalla
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.RevertirCartera;
            this.frmModule = ModulesPrefix.cc;
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa los controles de la aplicacion
        /// </summary>
        protected override void InitControls()
        {
            base.InitControls();

            this.docs = new List<DTO_glDocumento>();
            this.docCtrls = new List<DTO_glDocumentoControl>();

            DTO_glDocumento doc = new DTO_glDocumento();
            doc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.LiquidacionCredito.ToString(), true);
            this.docs.Add(doc);
            doc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.RecaudosManuales.ToString(), true);
            this.docs.Add(doc);
            doc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.PagosTotales.ToString(), true);
            this.docs.Add(doc);
            doc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.RecaudosMasivos.ToString(), true);
            this.docs.Add(doc);
            doc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.Desistimiento.ToString(), true);
            this.docs.Add(doc);
            doc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.VentaCartera.ToString(), true);
            this.docs.Add(doc);
            doc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.CobroJuridico.ToString(), true);
            this.docs.Add(doc);
            doc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.AcuerdoPago.ToString(), true);
            this.docs.Add(doc);
            doc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.AcuerdoPagoIncumplido.ToString(), true);
            this.docs.Add(doc);
            doc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.Incorporacion.ToString(), true);
            this.docs.Add(doc);
            doc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.ReincorporacionCartera.ToString(), true);
            this.docs.Add(doc);
            doc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.DesIncorporacion.ToString(), true);
            this.docs.Add(doc);
            doc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.ReintegroClientes.ToString(), true);
            this.docs.Add(doc);
            doc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.RenovacionPoliza.ToString(), true);
            this.docs.Add(doc);

            this.lkpDocumentos.Properties.DataSource = this.docs;
            this.tlSeparatorPanel.RowStyles[0].Height = 25;
        }             

        #endregion    

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                DTO_glDocumentoControl ctrl = null;
                List<DTO_glDocumentoControl> temps = this.docCtrls.Where(x => x.Marca.Value == true).ToList();

                if (temps.Count > 1)
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.MultiAnular));
                else if (temps.Count == 1)
                {
                    string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Anular);
                    string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Anular_Register);

                    //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ctrl = temps.First();
                        int docID = ctrl.DocumentoID.Value.Value;
                        int numDoc = ctrl.NumeroDoc.Value.Value;

                        FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                        FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                        ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                        FormProvider.Master.ProgressBarThread = new Thread(pth);
                        FormProvider.Master.ProgressBarThread.Start(this.documentID);

                        DTO_TxResult result = new DTO_TxResult();

                        switch(docID)
                        {
                            case AppDocuments.LiquidacionCredito:
                                result  = this._bc.AdministrationModel.LiquidacionCredito_Revertir(this.documentID, numDoc);
                                break;
                            case AppDocuments.RecaudosManuales:
                                result = this._bc.AdministrationModel.CarteraPagos_Revertir(this.documentID, numDoc);
                                break;
                            case AppDocuments.PagosTotales:
                                result = this._bc.AdministrationModel.CarteraPagos_Revertir(this.documentID, numDoc);
                                break;
                            case AppDocuments.RecaudosMasivos:
                                result = this._bc.AdministrationModel.CarteraPagos_Revertir(this.documentID, numDoc);
                                break;
                            case AppDocuments.Desistimiento:
                                result = this._bc.AdministrationModel.Credito_Desistimiento_Revertir(this.documentID, numDoc);
                                break;
                            case AppDocuments.VentaCartera:
                                result = this._bc.AdministrationModel.VentaCartera_Revertir(this.documentID, numDoc);
                                break;
                            case AppDocuments.CobroJuridico:
                                result = this._bc.AdministrationModel.EnvioCJ_Revertir(this.documentID, numDoc);
                                break;
                            case AppDocuments.AcuerdoPago:
                                result = this._bc.AdministrationModel.EnvioCJ_Revertir(this.documentID, numDoc);
                                break;
                            case AppDocuments.AcuerdoPagoIncumplido:
                                result = this._bc.AdministrationModel.EnvioCJ_Revertir(this.documentID, numDoc);
                                break;
                            case AppDocuments.Incorporacion:
                                result = this._bc.AdministrationModel.IncorporacionCredito_Revertir(this.documentID, numDoc);
                                break;
                            case AppDocuments.ReincorporacionCartera:
                                result = this._bc.AdministrationModel.Reincorporacion_Revertir(this.documentID, numDoc);
                                break;
                            case AppDocuments.DesIncorporacion:
                                result = this._bc.AdministrationModel.DesIncorporacion_Revertir(this.documentID, numDoc);
                                break;
                            case AppDocuments.RenovacionPoliza:
                                result = this._bc.AdministrationModel.RenovacionPoliza_Revertir(this.documentID, numDoc);
                                break;                                
                            default:
                                //Reintegros
                                result = this._bc.AdministrationModel.glDocumentoControl_Revertir(this.documentID, numDoc);
                                break;
                        }
                        FormProvider.Master.StopProgressBarThread(this.documentID);

                        MessageForm frm = new MessageForm(result);
                        frm.ShowDialog();

                        if (result.Result == ResultValue.OK)
                            this.Invoke(this.refreshGridDelegate);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RevertirCartera.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion                           
    }
}