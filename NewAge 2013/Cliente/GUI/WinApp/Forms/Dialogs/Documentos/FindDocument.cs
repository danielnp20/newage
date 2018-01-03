using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;

namespace NewAge.Cliente.GUI.WinApp.Forms
{

    /// <summary>
    /// Formulario para buscar documentos
    /// </summary>
    public partial class FindDocument : Form
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl[] _documentos = new DTO_glDocumentoControl[3];
        private int _indiceInterno = 0;
        private int _indiceExterno = 1;
        private int _indiceComprobante = 2;
        private int? _activo = null;   
        private int _documentID;

        #endregion

        public FindDocument()
        {
            InitializeComponent();
            this._documentID = AppForms.ConsultaDocumentos;
            this.ChangeStatusControls(1);

            this._bc.InitMasterUC(this.mfDocument, AppMasters.glDocumento, false, true, true, false);
            this._bc.InitMasterUC(this.mfPrefijo, AppMasters.glPrefijo, true, true, true, false);
            this._bc.InitMasterUC(this.mfTercero, AppMasters.coTercero, true, true, true, false);
            this._bc.InitMasterUC(this.mfComprobante, AppMasters.coComprobante, true, true, true, false);
            FormProvider.LoadResources(this, this._documentID);
            this.periodo.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }      

        #region Funciones Privadas

        /// <summary>
        /// Cambia estado de los controles segun indice
        /// </summary>
        /// <param name="index">Indice a cambiar estado</param>
        private void ChangeStatusControls(int index) 
        {
            switch (index)
            {
                case 1:
                    this.mfPrefijo.EnableControl(true);
                    this.txtNumDocInterno.Enabled = true;
                    this.btnQueryDoc.Enabled = true;
                    this.mfTercero.EnableControl(false);
                    this.mfComprobante.EnableControl(false);
                    this.txtDocExterno.Enabled = false;
                    this.txtNumComprobante.Enabled = false;
                    this.periodo.EnabledControl = false;
                    break;
                case 2:
                    this.mfPrefijo.EnableControl(false);
                    this.txtNumDocInterno.Enabled = false;
                    this.btnQueryDoc.Enabled = false;
                    this.mfTercero.EnableControl(true);
                    this.txtDocExterno.Enabled = true;
                    this.mfComprobante.EnableControl(false);  
                    this.txtNumComprobante.Enabled = false;
                    this.periodo.EnabledControl = false;
                    break;
                case 3:
                    this.mfPrefijo.EnableControl(false);
                    this.txtNumDocInterno.Enabled = false;
                    this.btnQueryDoc.Enabled = false;
                    this.mfTercero.EnableControl(false);
                    this.txtDocExterno.Enabled = false;
                    this.mfComprobante.EnableControl(true);  
                    this.txtNumComprobante.Enabled = true;
                    this.periodo.EnabledControl = true; ;
                    break;
                default:
                    this.mfPrefijo.EnableControl(true);
                    this.txtNumDocInterno.Enabled = true;
                    this.btnQueryDoc.Enabled = true;
                    this.mfTercero.EnableControl(false);
                    this.txtDocExterno.Enabled = false;
                    this.mfComprobante.EnableControl(false);  
                    this.txtNumComprobante.Enabled = false;
                    this.periodo.EnabledControl = false;
                    break;
            }
        }

        /// <summary>
        /// Obtiene un documento interno
        /// </summary>
        /// <returns>un dto de gDocumentoControl</returns>
        private DTO_glDocumentoControl GetDocumentInt()
        {
            try
            {
                string prefijo = mfPrefijo.Value;
                int docId = Convert.ToInt32(mfDocument.Value);
                int numDoc = Convert.ToInt32(txtNumDocInterno.Text);
                DTO_glDocumentoControl doc = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(docId, prefijo, numDoc);
                return doc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FindDocument.cs", "GetDocumentInt"));
                return null;
            }            
        }

        /// <summary>
        /// Obtiene un documento externo
        /// </summary>
        /// <returns>un dto de gDocumentoControl</returns>
        private DTO_glDocumentoControl GetDocumentExt()
        {
            try
            {
                string tercero = mfTercero.Value;
                int docId = Convert.ToInt32(this.mfDocument.Value);
                string docExt = this.txtDocExterno.Text;
                DTO_glDocumentoControl doc = this._bc.AdministrationModel.glDocumentoControl_GetExternalDoc(docId, tercero, docExt);

                return doc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FindDocument.cs", "GetDocumentExt"));
                return null;
            }
        }

        /// <summary>
        ///  Obtiene un documento interno
        /// </summary>
        /// <returns>un dto de gDocumentoControl</returns>
        private DTO_glDocumentoControl GetComprobante()
        {
            try
            {
                string comprobanteId = mfComprobante.Value;
                int docId = Convert.ToInt32(mfDocument.Value);
                int comprobanteNro = Convert.ToInt32(txtNumComprobante.Text);
                DTO_glDocumentoControl doc = this._bc.AdministrationModel.glDocumentoControl_GetByComprobante(docId, periodo.DateTime, comprobanteId, comprobanteNro);
                return doc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FindDocument.cs", "GetComprobante"));
                return null;
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Trae la información del comprobante
        /// </summary>
        private void periodo_ValueChanged()
        {
            try
            {
                string comprobanteId = mfComprobante.Value;
                int docId = Convert.ToInt32(mfDocument.Value);
                int comprobanteNro = Convert.ToInt32(txtNumComprobante.Text);
                DTO_glDocumentoControl doc = this._bc.AdministrationModel.glDocumentoControl_GetByComprobante(docId, periodo.DateTime, comprobanteId, comprobanteNro);
                if (doc != null)
                {
                    this._documentos[_indiceComprobante] = doc;
                    this.btnAbrir.Enabled = true;
                    this.btnAbrir.Focus();
                    _activo = _indiceComprobante;
                }
            }
            catch (Exception)
            {
                ;
            }
        }

        /// <summary>
        /// Activa el numero de documento correspondiente
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNumDoc_Click(object sender, EventArgs e)
        {
            try
            {
                TextBox tx = sender as TextBox;
                if (tx.ReadOnly)
                {
                    tx.ReadOnly = false;
                    tx.Focus();
                }
            }
            catch (Exception)
            {
                ;
            }

        }

        /// <summary>
        /// revisa el estado del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void rbtPrefijo_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeStatusControls(1);
        }

        /// <summary>
        /// revisa el estado del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void rbtTercero_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeStatusControls(2);
        }

        /// <summary>
        /// revisa el estado del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void rtbComprobante_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeStatusControls(3);
        }

        /// <summary>
        /// CLick en el botón
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            DTO_glDocumentoControl doc = null;

            if (this.mfDocument.ValidID)
            {
                int docID = Convert.ToInt32(this.mfDocument.Value);
                bool canAccess = SecurityManager.HasAccess(docID, FormsActions.Get);
                if (!canAccess)
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentNoAccess));
                    return;
                }


                if (rbtPrefijo.Checked)
                {
                    #region Busqueda por Doc interno
                    if (this.mfPrefijo.ValidID)
                    {
                        if (!string.IsNullOrEmpty(this.txtNumDocInterno.Text))
                        {
                            doc = this.GetDocumentInt();
                            if (doc != null)
                            {
                                DTO_Comprobante comprobante = null;
                                //if (doc.ComprobanteID.Value != string.Empty && doc.ComprobanteIDNro.Value != null && doc.ComprobanteIDNro.Value.HasValue && doc.ComprobanteIDNro.Value.Value != 0)
                                //{
                                bool isPre = false;
                                if (doc.Estado.Value.Value == (byte)EstadoDocControl.SinAprobar || doc.Estado.Value.Value == (byte)EstadoDocControl.ParaAprobacion)
                                    isPre = true;

                                comprobante = this._bc.AdministrationModel.Comprobante_GetAll(doc.NumeroDoc.Value.Value, isPre, doc.PeriodoDoc.Value.Value, doc.ComprobanteID.Value,
                                    doc.ComprobanteIDNro.Value);
                                //}

                                if (comprobante != null)
                                    new ShowDocumentForm(doc, comprobante).Show();
                                else
                                    new ShowDocumentForm(doc, null).Show();
                            }
                            else
                            {
                                msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                                MessageBox.Show(msg);
                            }
                        }
                        else
                        {
                            msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                            MessageBox.Show(msg);
                            this.txtNumDocInterno.Focus();
                        }
                    }
                    else
                    {
                        msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                        MessageBox.Show(string.Format(msg, this.mfPrefijo.LabelRsx));
                        this.mfPrefijo.Focus();
                    }
                    #endregion
                }
                else if (rbtTercero.Checked)
                {
                    #region Busqueda por Doc Externo
                    if (this.mfTercero.ValidID)
                    {
                        if (!string.IsNullOrEmpty(this.txtDocExterno.Text))
                        {
                            doc = this.GetDocumentExt();
                            if (doc != null)
                            {
                                DTO_Comprobante comprobante = null;
                                //if (doc.ComprobanteID.Value != string.Empty && doc.ComprobanteIDNro.Value != null && doc.ComprobanteIDNro.Value.HasValue && doc.ComprobanteIDNro.Value.Value != 0)
                                //{
                                bool isPre = false;
                                if (doc.Estado.Value.Value == (byte)EstadoDocControl.SinAprobar || doc.Estado.Value.Value == (byte)EstadoDocControl.ParaAprobacion ||
                                     doc.Estado.Value.Value == (byte)EstadoDocControl.Radicado)
                                    isPre = true;

                                comprobante = this._bc.AdministrationModel.Comprobante_GetAll(doc.NumeroDoc.Value.Value, isPre, doc.PeriodoDoc.Value.Value, doc.ComprobanteID.Value,
                                    doc.ComprobanteIDNro.Value);
                                //}

                                if (comprobante != null)
                                    new ShowDocumentForm(doc, comprobante).Show();
                                else
                                    new ShowDocumentForm(doc, null).Show();
                            }
                            else
                            {
                                msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                                MessageBox.Show(msg);
                            }
                        }
                        else
                        {
                            msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                            MessageBox.Show(msg);
                            this.txtDocExterno.Focus();
                        }
                    }
                    else
                    {
                        msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                        MessageBox.Show(string.Format(msg, this.mfTercero.LabelRsx));
                        this.mfTercero.Focus();
                    }
                    #endregion
                }
                else if (rtbComprobante.Checked)
                {
                    #region Busqueda por Comprobante
                    if (this.mfComprobante.ValidID)
                    {
                        if (!string.IsNullOrEmpty(this.txtNumComprobante.Text))
                        {
                            doc = this.GetComprobante();
                            if (doc != null)
                            {
                                bool isPre = false;
                                if (doc.Estado.Value.Value == (byte)EstadoDocControl.SinAprobar || doc.Estado.Value.Value == (byte)EstadoDocControl.ParaAprobacion)
                                    isPre = true;

                                DTO_Comprobante comprobante = this._bc.AdministrationModel.Comprobante_GetAll(doc.NumeroDoc.Value.Value, isPre, doc.PeriodoDoc.Value.Value,
                                    doc.ComprobanteID.Value, doc.ComprobanteIDNro.Value);

                                if (comprobante != null)
                                    new ShowDocumentForm(doc, comprobante).Show();
                                else
                                {
                                    msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                                    MessageBox.Show(msg);
                                }
                            }
                            else
                            {
                                msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                                MessageBox.Show(msg);
                            }
                        }
                        else
                        {
                            msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                            MessageBox.Show(msg);
                            this.txtNumComprobante.Focus();
                        }
                    }
                    else
                    {
                        msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                        MessageBox.Show(string.Format(msg, this.mfComprobante.LabelRsx));
                        this.mfComprobante.Focus();
                    }
                    #endregion
                }
            }
            else
            {
                msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                MessageBox.Show(string.Format(msg, this.mfDocument.LabelRsx));
            }
        }


        #endregion

        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> docs = new List<int>();
                if (this.mfDocument.ValidID)
                    docs.Add(Convert.ToInt32(this.mfDocument.Value));
                ModalQueryDocument getDocControl = new ModalQueryDocument(docs);
                getDocControl.ShowDialog();
                if (getDocControl.DocumentoControl != null)
                {
                    this.txtNumDocInterno.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                    this.mfPrefijo.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FindDocument.cs.cs", "FindDocument.cs-btnQueryDoc_Click"));
            }
        }
    }
}
