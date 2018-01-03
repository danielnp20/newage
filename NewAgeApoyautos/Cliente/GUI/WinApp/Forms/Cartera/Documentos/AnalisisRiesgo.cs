﻿using System;
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
using System.Threading;
using NewAge.DTO.Resultados;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class AnalisisRiesgo : SolicitudCreditoChequeo
    {
        #region Variables Formulario
        //Obtiene la instancia del controlador
        private BaseController _bc = BaseController.GetInstance();
        #endregion

        public AnalisisRiesgo()
            : base()
        {
            //InitializeComponent();
        }

        public AnalisisRiesgo(string mod)
            : base(mod)
        {
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.AnalisisRiesgo;
            this.frmModule = ModulesPrefix.cc;

            base.SetInitParameters();
            this.tableLayoutPanel1.RowStyles[1].Height = 300;
            this.tableLayoutPanel1.RowStyles[2].Height = 0; 
            this.tableLayoutPanel1.RowStyles[3].Height = 250;

            //Deshabilita los botones +- de la grilla
            this.gcDocuments.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcDocuments.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;
            this.gvDocuments.OptionsView.ShowAutoFilterRow = true;
            
            this.gcAnexos.Visible = false;
        }

        /// <summary>
        /// Carga la informacion de la grilla de Tareas
        /// </summary>
        protected override void LoadTareas()
        {
            try
            {

                DTO_SolicitudAprobacionCartera doc = (DTO_SolicitudAprobacionCartera)this.currentDoc;
                int numeroDoc = doc.NumeroDoc.Value.Value;

                List<DTO_ccTareaChequeoLista> temp =
                    this.tareasAll.Where(x => x.NumeroDoc.Value.Value == numeroDoc).ToList();

                if (temp.Count > 0)
                    this.tareas = temp;
                else
                {
                    this.tareas = new List<DTO_ccTareaChequeoLista>();

                    List<DTO_MasterBasic> chequeos = _bc.AdministrationModel.ccChequeoLista_GetByDocumento(this.documentID);
                    foreach (DTO_MasterBasic basic in chequeos)
                    {
                        DTO_ccTareaChequeoLista chequeo = new DTO_ccTareaChequeoLista();
                        chequeo.NumeroDoc.Value = numeroDoc;
                        chequeo.TareaID.Value = basic.ID.Value;
                        chequeo.Descriptivo.Value = basic.Descriptivo.Value;
                        chequeo.IncluidoInd.Value = false;

                        this.tareas.Add(chequeo);
                    }

                    this.tareasAll.AddRange(this.tareas);
                }

                this.gcTareas.DataSource = this.tareas;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AnalisisRiesgo.cs", "LoadTareas"));
            }
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            //Carga la actividades a revertir
            List<DTO_glActividadFlujo> actPadres = _bc.AdministrationModel.glActividadFlujo_GetParents(this.actividadFlujoID);
            foreach (DTO_glActividadFlujo act in actPadres)
            {
                this.actividadesCombo.Add(act.Descriptivo.Value);
                this.actFlujoForReversion.Add(act.Descriptivo.Value, act.ID.Value);
            }

            //Agrega el texto de anulado
            this.actividadesCombo.Add(this.msgAnulado);
            this.actFlujoForReversion.Add(this.msgAnulado, string.Empty);

            this.editCmb.Items.AddRange(this.actividadesCombo);

        }

        protected override void AddDocumentCols()
        {
            base.AddDocumentCols();
            try
            {
                //Acierta
                GridColumn Acierta = new GridColumn();
                Acierta.FieldName = this.unboundPrefix + "Acierta";
                Acierta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Acierta");
                Acierta.UnboundType = UnboundColumnType.String;
                Acierta.VisibleIndex = 9;
                Acierta.Width = 50;
                Acierta.Visible = true;
                Acierta.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                Acierta.OptionsColumn.AllowEdit = true;
                this.gvDocuments.Columns.Add(Acierta);

                //AciertaCifin
                GridColumn AciertaCifin = new GridColumn();
                AciertaCifin.FieldName = this.unboundPrefix + "AciertaCifin";
                AciertaCifin.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_AciertaCifin");
                AciertaCifin.UnboundType = UnboundColumnType.String;
                AciertaCifin.VisibleIndex = 10;
                AciertaCifin.Width = 50;
                AciertaCifin.Visible = true;
                AciertaCifin.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                AciertaCifin.OptionsColumn.AllowEdit = true;
                this.gvDocuments.Columns.Add(AciertaCifin);
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "AddDocumentCols"));
            }
        }

        #endregion

    }
}
