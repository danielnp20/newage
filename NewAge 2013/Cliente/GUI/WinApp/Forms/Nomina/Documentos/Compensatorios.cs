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
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.Data;
using DevExpress.XtraVerticalGrid.Events;
using NewAge.DTO.Resultados;
using System.Threading;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class Compensatorios : DocumentNominaForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private List<DTO_noCompensatorios> _compensatorios;

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.RefreshDocument();
        }

        private delegate void ActualizarCompensatorio();
        private ActualizarCompensatorio actualizarCompensatorioDelegate;

        private void ActualizarCompensatorioMethod()
        {
            _bc.AdministrationModel.Nomina_UpdCompensatorio(_compensatorios);
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
           
        }

        /// <summary>
        /// Campos para habilitar o deshabilitar los controles
        /// </summary>
        /// <param name="estado">estado true o false</param>
        private void FieldsEnabled(bool estado)
        {
           
        }


        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
            this.gcDocument.DataSource = null;
            this.dtFecha.Enabled = false;
            FormProvider.Master.itemPrint.Enabled = false;
            FormProvider.Master.itemSave.Enabled = false;
            this.LoadData(true);
        }



        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            if (firstTime)
            {
               
            }

        }

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        protected override int GetMasterDocumentID(string colName)
        {
            return 0;
        }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// <param name="oper">Indica si se debe ejecutar una segunda operacion</param>
        /// </summary>
        protected override void RowIndexChanged(int fila, bool oper)
        {

        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            InitializeComponent();
            this.documentID = AppDocuments.Compensatorios;

            base.SetInitParameters();

            //Inicia las variables del formulario
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            //Asigna las propiedades al documento
            this.frmModule = ModulesPrefix.no;
            this.LoadData(true);

            this.InitControls();
            this.AddGridCols();
            this.AfterInitialize();
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Compensatorios

                GridColumn Periodo = new GridColumn();
                Periodo.FieldName = this.unboundPrefix + "Periodo";
                Periodo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Periodo");
                Periodo.UnboundType = UnboundColumnType.String;
                Periodo.VisibleIndex = 0;
                Periodo.Width = 100;
                Periodo.Visible = true;
                Periodo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Periodo);

                GridColumn ContratoNOID = new GridColumn();
                ContratoNOID.FieldName = this.unboundPrefix + "ContratoNOID";
                ContratoNOID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ContratoNOID");
                ContratoNOID.UnboundType = UnboundColumnType.String;
                ContratoNOID.VisibleIndex = 0;
                ContratoNOID.Width = 100;
                ContratoNOID.Visible = true;
                ContratoNOID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ContratoNOID);

                GridColumn Dia1 = new GridColumn();
                Dia1.FieldName = this.unboundPrefix + "Dia1";
                Dia1.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia1");
                Dia1.UnboundType = UnboundColumnType.String;
                Dia1.VisibleIndex = 0;
                Dia1.Width = 100;
                Dia1.Visible = true;
                Dia1.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia1);

                GridColumn Dia2 = new GridColumn();
                Dia2.FieldName = this.unboundPrefix + "Dia2";
                Dia2.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia2");
                Dia2.UnboundType = UnboundColumnType.String;
                Dia2.VisibleIndex = 0;
                Dia2.Width = 20;
                Dia2.Visible = true;
                Dia2.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia2);

                GridColumn Dia3 = new GridColumn();
                Dia3.FieldName = this.unboundPrefix + "Dia3";
                Dia3.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia3");
                Dia3.UnboundType = UnboundColumnType.String;
                Dia3.VisibleIndex = 0;
                Dia3.Width = 20;
                Dia3.Visible = true;
                Dia3.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia3);

                GridColumn Dia4 = new GridColumn();
                Dia4.FieldName = this.unboundPrefix + "Dia4";
                Dia4.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia4");
                Dia4.UnboundType = UnboundColumnType.String;
                Dia4.VisibleIndex = 0;
                Dia4.Width = 20;
                Dia4.Visible = true;
                Dia4.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia4);

                GridColumn Dia5 = new GridColumn();
                Dia5.FieldName = this.unboundPrefix + "Dia5";
                Dia5.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia5");
                Dia5.UnboundType = UnboundColumnType.String;
                Dia5.VisibleIndex = 0;
                Dia5.Width = 20;
                Dia5.Visible = true;
                Dia5.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia5);

                GridColumn Dia6 = new GridColumn();
                Dia6.FieldName = this.unboundPrefix + "Dia6";
                Dia6.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia6");
                Dia6.UnboundType = UnboundColumnType.String;
                Dia6.VisibleIndex = 0;
                Dia6.Width = 20;
                Dia6.Visible = true;
                Dia6.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia6);

                GridColumn Dia7 = new GridColumn();
                Dia7.FieldName = this.unboundPrefix + "Dia7";
                Dia7.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia7");
                Dia7.UnboundType = UnboundColumnType.String;
                Dia7.VisibleIndex = 0;
                Dia7.Width = 20;
                Dia7.Visible = true;
                Dia7.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia7);

                GridColumn Dia8 = new GridColumn();
                Dia8.FieldName = this.unboundPrefix + "Dia8";
                Dia8.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia8");
                Dia8.UnboundType = UnboundColumnType.String;
                Dia8.VisibleIndex = 0;
                Dia8.Width = 20;
                Dia8.Visible = true;
                Dia8.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia8);

                GridColumn Dia9 = new GridColumn();
                Dia9.FieldName = this.unboundPrefix + "Dia9";
                Dia9.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia9");
                Dia9.UnboundType = UnboundColumnType.String;
                Dia9.VisibleIndex = 0;
                Dia9.Width = 20;
                Dia9.Visible = true;
                Dia9.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia9);

                GridColumn Dia10 = new GridColumn();
                Dia10.FieldName = this.unboundPrefix + "Dia10";
                Dia10.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia10");
                Dia10.UnboundType = UnboundColumnType.String;
                Dia10.VisibleIndex = 0;
                Dia10.Width = 20;
                Dia10.Visible = true;
                Dia10.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia10);

                GridColumn Dia11 = new GridColumn();
                Dia11.FieldName = this.unboundPrefix + "Dia11";
                Dia11.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia11");
                Dia11.UnboundType = UnboundColumnType.String;
                Dia11.VisibleIndex = 0;
                Dia11.Width = 20;
                Dia11.Visible = true;
                Dia11.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia11);

                GridColumn Dia12 = new GridColumn();
                Dia12.FieldName = this.unboundPrefix + "Dia12";
                Dia12.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia12");
                Dia12.UnboundType = UnboundColumnType.String;
                Dia12.VisibleIndex = 0;
                Dia12.Width = 20;
                Dia12.Visible = true;
                Dia12.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia12);

                GridColumn Dia13 = new GridColumn();
                Dia13.FieldName = this.unboundPrefix + "Dia13";
                Dia13.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia13");
                Dia13.UnboundType = UnboundColumnType.String;
                Dia13.VisibleIndex = 0;
                Dia13.Width = 20;
                Dia13.Visible = true;
                Dia13.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia13);

                GridColumn Dia14 = new GridColumn();
                Dia14.FieldName = this.unboundPrefix + "Dia14";
                Dia14.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia14");
                Dia14.UnboundType = UnboundColumnType.String;
                Dia14.VisibleIndex = 0;
                Dia14.Width = 20;
                Dia14.Visible = true;
                Dia14.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia14);

                GridColumn Dia15 = new GridColumn();
                Dia15.FieldName = this.unboundPrefix + "Dia15";
                Dia15.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia15");
                Dia15.UnboundType = UnboundColumnType.String;
                Dia15.VisibleIndex = 0;
                Dia15.Width = 20;
                Dia15.Visible = true;
                Dia15.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia15);

                GridColumn Dia16 = new GridColumn();
                Dia16.FieldName = this.unboundPrefix + "Dia16";
                Dia16.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia16");
                Dia16.UnboundType = UnboundColumnType.String;
                Dia16.VisibleIndex = 0;
                Dia16.Width = 20;
                Dia16.Visible = true;
                Dia16.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia16);

                GridColumn Dia17 = new GridColumn();
                Dia17.FieldName = this.unboundPrefix + "Dia17";
                Dia17.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia17");
                Dia17.UnboundType = UnboundColumnType.String;
                Dia17.VisibleIndex = 0;
                Dia17.Width = 20;
                Dia17.Visible = true;
                Dia17.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia17);

                GridColumn Dia18 = new GridColumn();
                Dia18.FieldName = this.unboundPrefix + "Dia18";
                Dia18.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia18");
                Dia18.UnboundType = UnboundColumnType.String;
                Dia18.VisibleIndex = 0;
                Dia18.Width = 20;
                Dia18.Visible = true;
                Dia18.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia18);

                GridColumn Dia19 = new GridColumn();
                Dia19.FieldName = this.unboundPrefix + "Dia19";
                Dia19.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia19");
                Dia19.UnboundType = UnboundColumnType.String;
                Dia19.VisibleIndex = 0;
                Dia19.Width = 20;
                Dia19.Visible = true;
                Dia19.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia19);

                GridColumn Dia20 = new GridColumn();
                Dia20.FieldName = this.unboundPrefix + "Dia20";
                Dia20.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia20");
                Dia20.UnboundType = UnboundColumnType.String;
                Dia20.VisibleIndex = 0;
                Dia20.Width = 20;
                Dia20.Visible = true;
                Dia20.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia20);

                GridColumn Dia21 = new GridColumn();
                Dia21.FieldName = this.unboundPrefix + "Dia21";
                Dia21.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia21");
                Dia21.UnboundType = UnboundColumnType.String;
                Dia21.VisibleIndex = 0;
                Dia21.Width = 20;
                Dia21.Visible = true;
                Dia21.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia21);

                GridColumn Dia22 = new GridColumn();
                Dia22.FieldName = this.unboundPrefix + "Dia22";
                Dia22.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia22");
                Dia22.UnboundType = UnboundColumnType.String;
                Dia22.VisibleIndex = 0;
                Dia22.Width = 20;
                Dia22.Visible = true;
                Dia22.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia22);

                GridColumn Dia23 = new GridColumn();
                Dia23.FieldName = this.unboundPrefix + "Dia23";
                Dia23.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia23");
                Dia23.UnboundType = UnboundColumnType.String;
                Dia23.VisibleIndex = 0;
                Dia23.Width = 20;
                Dia23.Visible = true;
                Dia23.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia23);

                GridColumn Dia24 = new GridColumn();
                Dia24.FieldName = this.unboundPrefix + "Dia24";
                Dia24.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia24");
                Dia24.UnboundType = UnboundColumnType.String;
                Dia24.VisibleIndex = 0;
                Dia24.Width = 20;
                Dia24.Visible = true;
                Dia24.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia24);

                GridColumn Dia25 = new GridColumn();
                Dia25.FieldName = this.unboundPrefix + "Dia25";
                Dia25.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia5");
                Dia25.UnboundType = UnboundColumnType.String;
                Dia25.VisibleIndex = 0;
                Dia25.Width = 20;
                Dia25.Visible = true;
                Dia25.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia25);

                GridColumn Dia26 = new GridColumn();
                Dia26.FieldName = this.unboundPrefix + "Dia26";
                Dia26.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia26");
                Dia26.UnboundType = UnboundColumnType.String;
                Dia26.VisibleIndex = 0;
                Dia26.Width = 20;
                Dia26.Visible = true;
                Dia26.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia26);

                GridColumn Dia27 = new GridColumn();
                Dia27.FieldName = this.unboundPrefix + "Dia27";
                Dia27.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia27");
                Dia27.UnboundType = UnboundColumnType.String;
                Dia27.VisibleIndex = 0;
                Dia27.Width =20;
                Dia27.Visible = true;
                Dia27.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia27);

                GridColumn Dia28 = new GridColumn();
                Dia28.FieldName = this.unboundPrefix + "Dia28";
                Dia28.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia28");
                Dia28.UnboundType = UnboundColumnType.String;
                Dia28.VisibleIndex = 0;
                Dia28.Width = 20;
                Dia28.Visible = true;
                Dia28.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia28);

                GridColumn Dia29 = new GridColumn();
                Dia29.FieldName = this.unboundPrefix + "Dia29";
                Dia29.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia29");
                Dia29.UnboundType = UnboundColumnType.String;
                Dia29.VisibleIndex = 0;
                Dia29.Width = 20;
                Dia29.Visible = true;
                Dia29.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia29);

                GridColumn Dia30 = new GridColumn();
                Dia30.FieldName = this.unboundPrefix + "Dia1";
                Dia30.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia30");
                Dia30.UnboundType = UnboundColumnType.String;
                Dia30.VisibleIndex = 0;
                Dia30.Width = 20;
                Dia30.Visible = true;
                Dia30.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia1);

                GridColumn Dia31 = new GridColumn();
                Dia31.FieldName = this.unboundPrefix + "Dia31";
                Dia31.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dia31");
                Dia31.UnboundType = UnboundColumnType.String;
                Dia31.VisibleIndex = 0;
                Dia31.Width = 20;
                Dia31.Visible = true;
                Dia31.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dia31);

                GridColumn DiasTrabajoMes = new GridColumn();
                DiasTrabajoMes.FieldName = this.unboundPrefix + "DiasTrabajoMes";
                DiasTrabajoMes.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DiasTrabajoMes");
                DiasTrabajoMes.UnboundType = UnboundColumnType.String;
                DiasTrabajoMes.VisibleIndex = 0;
                DiasTrabajoMes.Width = 60;
                DiasTrabajoMes.Visible = true;
                DiasTrabajoMes.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DiasTrabajoMes);

                GridColumn DiasDescansoMes = new GridColumn();
                DiasDescansoMes.FieldName = this.unboundPrefix + "DiasDescansoMes";
                DiasDescansoMes.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DiasDescansoMes");
                DiasDescansoMes.UnboundType = UnboundColumnType.String;
                DiasDescansoMes.VisibleIndex = 0;
                DiasDescansoMes.Width = 60;
                DiasDescansoMes.Visible = true;
                DiasDescansoMes.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DiasDescansoMes);

                GridColumn DiasSaldoAnt = new GridColumn();
                DiasSaldoAnt.FieldName = this.unboundPrefix + "DiasSaldoAnt";
                DiasSaldoAnt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DiasSaldoAnt");
                DiasSaldoAnt.UnboundType = UnboundColumnType.String;
                DiasSaldoAnt.VisibleIndex = 0;
                DiasSaldoAnt.Width = 20;
                DiasSaldoAnt.Visible = true;
                DiasSaldoAnt.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DiasSaldoAnt);

                GridColumn DiasMes = new GridColumn();
                DiasMes.FieldName = this.unboundPrefix + "DiasMes";
                DiasMes.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DiasMes");
                DiasMes.UnboundType = UnboundColumnType.String;
                DiasMes.VisibleIndex = 0;
                DiasMes.Width = 60;
                DiasMes.Visible = true;
                DiasMes.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DiasMes);


                GridColumn DiasPagados = new GridColumn();
                DiasPagados.FieldName = this.unboundPrefix + "DiasPagados";
                DiasPagados.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DiasPagados");
                DiasPagados.UnboundType = UnboundColumnType.String;
                DiasPagados.VisibleIndex = 0;
                DiasPagados.Width = 60;
                DiasPagados.Visible = true;
                DiasPagados.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DiasPagados);

                GridColumn DiasAjustados = new GridColumn();
                DiasAjustados.FieldName = this.unboundPrefix + "DiasAjustados";
                DiasAjustados.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DiasAjustados");
                DiasAjustados.UnboundType = UnboundColumnType.String;
                DiasAjustados.VisibleIndex = 0;
                DiasAjustados.Width = 60;
                DiasAjustados.Visible = true;
                DiasAjustados.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DiasAjustados);

                GridColumn DIasNuevoSaldo = new GridColumn();
                DIasNuevoSaldo.FieldName = this.unboundPrefix + "DIasNuevoSaldo";
                DIasNuevoSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DIasNuevoSaldo");
                DIasNuevoSaldo.UnboundType = UnboundColumnType.String;
                DIasNuevoSaldo.VisibleIndex = 0;
                DIasNuevoSaldo.Width = 60;
                DIasNuevoSaldo.Visible = true;
                DIasNuevoSaldo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DIasNuevoSaldo);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Compensatorios.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        protected override void AfterInitialize()
        {
            _compensatorios = _bc.AdministrationModel.Nomina_GetCompensatorios();
            gcDocument.DataSource = _compensatorios;
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);

            FormProvider.Master.tbBreak.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;
            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemImport.Visible = true;
            FormProvider.Master.itemRevert.Visible = false;

            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSave.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;
                FormProvider.Master.itemImport.Enabled = true;
            }
        }

        #endregion

        #region Eventos Header

       
        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Maneja campos en las grillas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "SiNo" && e.Value == null)
                {
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                }
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
                        {
                            e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                        }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
                            {
                                e.Value = fi.GetValue(dto);
                            }
                            else
                            {
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                            }
                        }
                    }
                }
            }
            if (e.IsSetData)
            {
                if (fieldName == "SiNo")
                {
                    bool value = Convert.ToBoolean(e.Value);
                    if (value)
                        this.select.Add(e.ListSourceRowIndex);
                    else
                        this.select.Remove(e.ListSourceRowIndex);
                }
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (e.Value == null)
                        e.Value = string.Empty;
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
                        {
                            e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            UDT udtProp = (UDT)pi.GetValue(dto, null);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
                            {
                                //e.Value = pi.GetValue(dto, null);
                            }
                            else
                            {
                                UDT udtProp = (UDT)fi.GetValue(dto);
                                udtProp.SetValueFromString(e.Value.ToString());
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Maneja campos de controles en la grilla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "SiNo")
            {
                e.RepositoryItem = this.editChkBox;
            }
        }


        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para recargar los controles del formulario
        /// </summary>
        public override void TBNew()
        {
            this.RefreshDocument();
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBSave()
        {
            Thread process = new Thread(this.SaveThread);
            process.Start();
        }

        /// <summary>
        /// Boton para importir datos
        /// </summary>
        public override void TBImport()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                this.Invoke(this.actualizarCompensatorioDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Compensatorios", "SaveThread"));
            }
        }

        #endregion
    }
}
