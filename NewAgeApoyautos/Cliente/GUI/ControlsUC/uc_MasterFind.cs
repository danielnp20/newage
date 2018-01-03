using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.ControlsUC
{
    /// <summary>
    /// Control de usuario para buscar un registro de una maestra
    /// </summary>
    public partial class uc_MasterFind : UserControl
    {
        #region Variables

        //Variables para abrir el formulario modal
        private string _countMethod = "MasterSimple_Count";
        private string _dataMethod = "MasterSimple_GetPaged";
        private string _dataRowMethod = "MasterSimple_GetByID";
        private string _initHierarchyMethod = "InitFindControl";
        private string _resultHierCode = "ResultCode";
        private string _colDesc = "Descriptivo";

        private bool _findDesc = true;
        #endregion

        #region propiedades

        /// <summary>
        /// Tabla con la info de la maestra
        /// </summary>
        public DTO_glTabla Table;

        /// <summary>
        /// Tipo de modal que se debe abrir para hacer la busqueda
        /// </summary>
        public Type ModalType;

        /// <summary>
        /// Nombre de la tabla
        /// </summary>
        public string TableName;

        /// <summary>
        /// Indica si es una tabla jerarquica
        /// </summary>
        public bool IsHierarchical;

        /// <summary>
        /// Identificador del documento
        /// </summary>
        public int DocId;

        /// <summary>
        /// Llave primaria de la tabla
        /// </summary>
        public string ColId;

        /// <summary>
        /// Grupo de empresas al que pertenece la maestra
        /// </summary>
        public string GrupoEmpresa;

        /// <summary>
        /// Identifica si se muestra o el texto de la maestra
        /// </summary>
        public bool HasLabel;

        /// <summary>
        /// Identifica si se muestra o no la descripcion
        /// </summary>
        public bool HasDesc;

        /// <summary>
        /// Indica si el texto ingresado tiene un valor existente
        /// </summary>
        public bool ValidID;

        /// <summary>
        /// Indica si es una jerarquica y solo acepta hojas
        /// </summary>
        public bool OnlyRoots;

        /// <summary>
        /// Nombre de la maestra con la traduccion
        /// </summary>
        public string CodeRsx
        {
            get { return this.lblMaster.Text; }
        }

        /// <summary>
        /// Obtiene el codigo seleccionado
        /// </summary>
        public string Value
        {
            get { return this.txtCode.Text.Trim(); }
            set { this.txtCode.Text = value.Trim(); }
        }

        /// <summary>
        /// Obtiene el recurso para el codigode la maestra
        /// </summary>
        public string LabelRsx
        {
            get { return this.lblMaster.Text.Trim(); }
        }

        public List<DTO_glConsultaFiltro> Filtros
        {
            get;
            set;
        }

        #endregion

        #region Declaración Handlers

        /// <summary>
        /// Obtiene la descripcion de una maestra
        /// </summary>
        /// <param name="onlyRoots">Indica si solo acepta raices para una jerarquica</param>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="colIDName">Npmbre de la PK</param>
        /// <param name="colIDVal">Valor de la PK</param>
        /// <returns>Retorna la descripcion del elemento dado el codigo</returns>
        public delegate Tuple<string, string> GetByIDHandler(bool onlyRoots, int documentID, string colIDName, string colIDVal, List<DTO_glConsultaFiltro> filtros);
        GetByIDHandler getMasterDescriptionByID;
        public event GetByIDHandler GetMasterDescriptionByID
        {
            add { this.getMasterDescriptionByID += value; }
            remove { this.getMasterDescriptionByID -= value; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public uc_MasterFind()
        {
            InitializeComponent();
        }

        #region Funciones Publicas

        /// <summary>
        /// Inicializa el control
        /// <param name="maxLength">Longitud maxima del campo</param>
        /// <param name="lblRsx">Recurso de la maestra (ID)</param>
        /// <param name="onlyRoots">Indica para las jerarquiecas si solo acepta hojas</param>
        /// <param name="mainControl">Indica si es un control principal, para ponerle un color diferente</param>
        /// </summary>
        public void InitControl(int maxLength, string lblRsx, bool onlyRoots, bool mainControl)
        {
            this.ValidID = false;
            this.OnlyRoots = onlyRoots;
            this.txtCode.MaxLength = maxLength;
            this.txtDesc.Visible = this.HasDesc;
            this.lblMaster.Visible = this.HasLabel;

            this.lblMaster.Text = lblRsx;
            if (mainControl)
            {
                this.txtCode.BackColor = Color.LightBlue;
                this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            }
            else
            {
                this.txtCode.BackColor = Color.White;
                this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            }
        }

        /// <summary>
        /// Deshabilita o habilita el control
        /// </summary>
        /// <param name="enable">Indicador para habilitar o deshabilitar el control</param>
        public void EnableControl(bool enable)
        {
            this.txtCode.ReadOnly = !enable;
            this.txtDesc.ReadOnly = !enable;
            this.btnFind.Enabled = enable;
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Boton para buscar datos del primer nivel
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                object[] currentParams;

                currentParams = new Object[9];
                currentParams[0] = this.txtCode;
                currentParams[1] = this.DocId.ToString();
                currentParams[2] = this._countMethod;
                currentParams[3] = this._dataMethod;
                currentParams[4] = null;
                currentParams[5] = this.ColId;
                currentParams[6] = this._colDesc;
                currentParams[7] = this.IsHierarchical;
                currentParams[8] = this.Filtros;

                Form f = (Form)Activator.CreateInstance(this.ModalType, currentParams);
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        /// <summary>
        /// Boton para validar caracteres ingresados
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Char.IsLetterOrDigit(e.KeyChar) || Char.IsControl(e.KeyChar))// || Char.IsWhiteSpace(e.KeyChar))
                {
                    this._findDesc = true;
                    e.Handled = false;
                }
                else
                    e.Handled = true;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Cambio de codigo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtCode.Text = this.txtCode.Text.Trim();
                if (this.txtCode.Text != string.Empty && this._findDesc)
                {
                    if (this.getMasterDescriptionByID != null)
                    {
                        Tuple<string, string> vals = this.getMasterDescriptionByID(this.OnlyRoots, this.DocId, this.ColId, this.txtCode.Text, this.Filtros);

                        this.txtDesc.Text = vals.Item1;
                        if (this.txtCode.Text != vals.Item2 && !string.IsNullOrWhiteSpace(vals.Item2))
                        {
                            //this._findDesc = false;
                            this.txtCode.Text = vals.Item2;
                        }

                        if (!string.IsNullOrEmpty(vals.Item1))
                            this.ValidID = true;
                        else
                            this.ValidID = false;
                    }
                }
                else
                {
                    this.txtDesc.Text = string.Empty;
                    this.ValidID = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

    }
}
