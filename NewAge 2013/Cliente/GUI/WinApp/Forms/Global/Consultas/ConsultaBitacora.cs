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
using SentenceTransformer;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.DTO.Reportes;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ConsultaBitacora : FormWithToolbar, IFiltrable
    {

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private DTO_glConsulta _consulta;
        private Dictionary<short, string> _acciones = new Dictionary<short, string>();
        protected FormTypes frmType = FormTypes.Bitacora;
        protected int documentID=1008;
        protected ModulesPrefix frmModule=ModulesPrefix.gl;
        protected string frmName;

        #endregion

        public ConsultaBitacora()
        {
            InitializeComponent();
            frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
            FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this.frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            
            _bc.Pagging_Init(this.pgGrid, 10);
            _bc.Pagging_SetEvent(this.pgGrid, this.pagging_Click);
            this.pgGrid.PageNumber = 1;

            string resLogin = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + "_Login");
            this._acciones.Add(0, resLogin);
            Array a = Enum.GetValues(typeof(FormsActions));

            foreach (FormsActions fa in a)
            {
                string res = _bc.GetResource(LanguageTypes.Forms, AppMasters.seGrupoDocumento + "_" + fa.ToString());
                this._acciones.Add(Convert.ToInt16(Math.Pow(2, (Double)fa)), res);
            }
            this.TBFilter();

            foreach (GridColumn column in this.masterView.Columns)
            {
                string field = column.FieldName;
                string fieldCap = this.documentID + "_" + field;
                column.Caption = _bc.GetResource(LanguageTypes.Forms, fieldCap);
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Llena el campo acción a una lista de bitacoras
        /// </summary>
        /// <param name="lista"></param>
        private void FillAccion(ref List<DTO_aplBitacora> lista)
        {
            foreach (DTO_aplBitacora bit in lista)
            {
                string data = null;
                if (this._acciones.TryGetValue(bit.AccionID.Value.Value, out data))
                    bit.Accion.Value = data;
            }
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Asigna una consulta desde MasterQuery para hacer el filtrado de datos
        /// </summary>
        /// <param name="consulta"></param>
        /// <param name="fields"></param>
        public void SetConsulta(DTO_glConsulta consulta, List<ConsultasFields> fields)
        {
            try
            {
                if (consulta != null)
                {
                    int visibleindex = 0;
                    this._consulta = consulta;
                    List<DTO_aplBitacora> data = _bc.AdministrationModel.aplBitacoraGetFilteredPaged(this.pgGrid.PageSize, this.pgGrid.PageNumber, consulta).ToList();
                    this.FillAccion(ref data);
                    this.gcBitacora.DataSource = data;
                    long count = _bc.AdministrationModel.aplBitacoraCountFiltered(consulta);
                    this.pgGrid.UpdatePageNumber(count, (this.pgGrid.PageNumber == 1), (this.pgGrid.PageNumber == 1), false);
                    //consulta.Selecciones.Reverse();
                    List<DTO_glConsultaSeleccion> temp = consulta.Selecciones;
                    foreach (GridColumn col in this.masterView.Columns)
                        col.Visible = false;

                    Dictionary<string, string> descFields = new Dictionary<string, string>();
                    descFields.Add("seUsuarioID", "Usuario");
                    descFields.Add("EmpresaID", "Empresa");
                    descFields.Add("DocumentoID", "Documento");
                    descFields.Add("AccionID", "Accion");
                    foreach (DTO_glConsultaSeleccion sel in temp)
                    {
                        string campo = sel.CampoFisico;
                        if (descFields.ContainsKey(campo))//Primero Agrega el campo de descripción y luego el de ID
                        {
                            GridColumn columnaDesc = this.masterView.Columns.ColumnByFieldName(descFields[campo]);
                            if (columnaDesc != null)
                            {
                                columnaDesc.VisibleIndex = visibleindex;
                                columnaDesc.Visible = true;
                            }
                        }
                        GridColumn columna = this.masterView.Columns.ColumnByFieldName(campo);
                        if (columna != null)
                        {
                            columna.VisibleIndex = visibleindex;
                            columna.Visible = true;
                        }
                        visibleindex++;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBitacora.cs", "SetConsulta"));
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this.frmType, this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBitacora.cs", "Form_Enter"));
            }

        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBitacora.cs", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBitacora.cs", "Form_FormClosing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this.frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBitacora.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Cuando la informacion de la grilla se expande
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterView_MasterRowExpanded(object sender, CustomMasterRowEventArgs e)
        {
            GridView gv = (this.masterView.GetDetailView(6, 0) as GridView);
            if (gv != null)
            {
                foreach (GridColumn column in gv.Columns)
                {
                    string field = column.FieldName;
                    string fieldCap = this.documentID + "_" + field;
                    column.Caption = _bc.GetResource(LanguageTypes.Forms, fieldCap);
                }
            }
        }

        /// <summary>
        /// Evento que se ejecuta al darle click al paginador
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void pagging_Click(object sender, System.EventArgs e)
        {
            this.SetConsulta(this._consulta, null);
            //this.pgGrid.UpdatePageNumber(this.pgGrid.Count, (this.pgGrid.PageNumber == 1), false, false);
        } 
        
        #endregion

        #region Barra de Herramientas

        /// <summary>
        /// Boton para filtrar la lista de resultados
        /// </summary>
        public override void TBFilter() 
        {
            MasterQuery mq = new MasterQuery(this, documentID, 1, true, typeof(DTO_aplBitacora), new List<string> { "Actualizaciones", "Usuario", "Empresa", "Documento", "Accion" });
            mq.SetFK("DocumentoID", AppMasters.glDocumento, _bc.CreateFKConfig(AppMasters.glDocumento));
            mq.SetFK("EmpresaID", AppMasters.glEmpresa, _bc.CreateFKConfig(AppMasters.glEmpresa));
            mq.SetFK("seUsuarioID", AppMasters.seUsuario, _bc.CreateFKConfig(AppMasters.seUsuario));
            Dictionary<string, string> filterAcciones = new Dictionary<string, string>();
            foreach (KeyValuePair<short, string> kvp in this._acciones)
            {
                filterAcciones.Add(kvp.Key.ToString(), kvp.Value);
            }
            mq.SetValueDictionary("AccionID", filterAcciones);
            mq.ShowDialog();
        } 
       
        /// <summary>
        /// Boton para generar reportes
        /// </summary>
        public override void TBPrint()
        {
           List<DTO_aplBitacora> data = _bc.AdministrationModel.aplBitacoraGetFiltered(this._consulta).ToList();
           this.FillAccion(ref data);
           
           BitacoraReportBuilder b = new BitacoraReportBuilder(this.documentID,this._acciones, data);
        }

	    #endregion

    }
}
