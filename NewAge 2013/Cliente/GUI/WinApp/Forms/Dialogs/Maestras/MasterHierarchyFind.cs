using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using System.Reflection;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Cliente.GUI.WinApp.Forms;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// control de usuario para el manejo de jerarquías
    /// </summary>
    public partial class MasterHierarchyFind : Form
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        //Tabla de glTabla (contiene la info de la jerarquia)
        private DTO_glTabla _table;
        //Diccionario con el id del dto y el cmpo de la jerarquia
        private Dictionary<string, DTO_hierarchy> _hierarchies = new Dictionary<string, DTO_hierarchy>();
        private ForeignKeyFieldConfig _fkConfig;
        private string _textButtonSelect = string.Empty;
        //Nivel maximo de niveles
        protected int? maxLevel = null;
        /// <summary>
        /// Variable para guardar el codigo seleccionado
        /// </summary>
        protected string resultCode = string.Empty;
        /// <summary>
        /// Asigna el titulo del codigo
        /// </summary>
        public string TextCode = string.Empty;
        /// <summary>
        /// Asigna el titulo de la descripción
        /// </summary>
        public string TextDescr = string.Empty;
        public Object[] InitArgs = new Object[] { };
        public List<DTO_glConsultaFiltro> Filtros;

        #endregion

        #region Propiedades

        public DTO_aplMaestraPropiedades FrmProperties
        {
            get;
            set;
        }

        /// <summary>
        /// Propiedad para consultar el valor seleccionado
        /// </summary>
        public string ResultCode
        {
            get
            {
                return resultCode;
            }
        }
       
        #endregion       

        #region Declaración Handlers
        
        /// <summary>
        /// Revisa un codigo en un nivel determinado
        /// Dependiendo si existe o no y en que nivel está
        /// habilita lso componentes para el siguiente nivel
        /// Crea una instancia del handler
        /// Crea una propiedad para el handler
        /// </summary>
        /// <param name="nivel">nivel en el q se encuentra</param>
        /// <param name="codigo">codigo a revisar</param>
        /// <param name="code">textbox dnde se escribio el codigo</param>
        /// <param name="desc">textobox de la descripcion del codigo</param>
        /// <param name="codeButton">boton para buscar el codigo actual</param>
        /// <param name="nextCode">textbox del codigo del siguiente nivel</param>
        /// <param name="nextDesc">textobox de la descripcipn del siguiente nivel</param>
        /// <param name="nextButton">boton del siguiente nivel</param>
        /// <param name="nextLbl">Label del siguiente nivel</param>
        public delegate void CheckLevelNewHandler(int nivel, string codigo, TextBox code, TextBox desc, Button codeButton, TextBox nextCode, TextBox nextDesc, Button nextButton, Label nextLbl);
        CheckLevelNewHandler checkLevelNew;
        new public event CheckLevelNewHandler CheckLevelNew
        {
            add { this.checkLevelNew += value; }
            remove { this.checkLevelNew -= value; }
        }

        public void ProcessCodeInput(int level, string code)
        {
            TextBox txtcode=this.TxtCodeHierarchy(level);
            TextBox desc=this.TxtDescHierarchy(level);
            Button codeButton=this.BtnHierarchy(level);
            TextBox nextCode=this.TxtCodeHierarchy(level+1);
            TextBox nextDesc = this.TxtDescHierarchy(level+1);
            Button nextButton = this.BtnHierarchy(level+1);
            Label nextLbl = this.LabelHierarchy(level+1);
            //this.SetCode(string.Empty);

            if (code.Length < this.CodeLength(level))
                return;

            UDT_BasicID codigoTemp = new UDT_BasicID();
            codigoTemp.MaxLength = this.FrmProperties.IDLongitudMax;
            codigoTemp.Value = code;

            int levels = this.HierarchyLevels();
            DTO_MasterHierarchyBasic dto = null;
            try
            {
                dto = _bc.AdministrationModel.MasterHierarchy_GetByID(this.DocumentID, codigoTemp, true);
            }
            catch (Exception)
            {
                return;
            }

            if (dto != null)
            {
                string dtoDescVal = dto.Descriptivo.Value;

                desc.Text = dtoDescVal;
                txtcode.Enabled = false;
                codeButton.Enabled = false;
                if (level < levels)
                {
                    if (nextCode != null)
                    {
                        nextCode.Visible = true;
                        nextCode.Enabled = true;
                    }
                    if (nextDesc != null)
                        nextDesc.Visible = true;
                    if (nextButton != null)
                        nextButton.Visible = true;
                    if (nextLbl != null)
                        nextLbl.Visible = true;
                }
                //this.LevelNew = level + 1;
                this.SetCode(code);
                return;
            }
            else
            {
                this.SetCode(string.Empty);
                //this.LevelNew = level;
                return;
            }
        }

        /// <summary>
        /// Pone el valor del codigo en el formulario del cual se llamo el componente
        /// </summary>
        /// <param name="code">Nuevo codigo</param>
        public delegate void UpdateEditGridHandler(string code);
        UpdateEditGridHandler updateEditGrid;
        private string EmpresaGrupoID;
        private int DocumentID;
        new public event UpdateEditGridHandler UpdateEditGrid
        {
            add { this.updateEditGrid += value; }
            remove { this.updateEditGrid -= value; }
        }

        /// <summary>
        /// Pone el valor del codigo en el formulario del cual se llamo el componente
        /// </summary>
        /// <param name="code">Nuevo codigo</param>
        public virtual void SetCode(string code)
        {
            //this.updateEditGrid(code);
            this.resultCode = code;
            int levelCode = this._table.LengthToLevel(code.Length);
            if (levelCode == this.MaxShownLevel() || (this.maxLevel == 0 && levelCode > 0))
            {
                this.btnReturn.Text = this._textButtonSelect + " " + code;
                this.btnReturn.Enabled = true;
                this.btnReturn.Focus();
            }
            else
            {
                this.btnReturn.Text = this._textButtonSelect;
                this.btnReturn.Enabled = false;
                this.TxtCodeHierarchy(levelCode + 1).Focus();
            }
        }

        #endregion

        public MasterHierarchyFind()
        {
            InitializeComponent();
            this.TextCode = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Code);
            this.TextDescr = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Description);
            this._textButtonSelect = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.HierarFindSelect);
            this.btnCancel.Text = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.HierarFindCancel);
        }       

        #region Funciones Privadas

        /// <summary>
        /// Retorna el label correspondiente al nombre del nivel de la jerarquia
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private Label LabelHierarchy(int level)
        {
            FieldInfo fi = this.GetType().GetField("lblHierarachyLvl" + level, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi != null)
            {
                return (Label)fi.GetValue(this);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retorna el text box correspondiente al codigo del nivel de la jerarquia
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private TextBox TxtCodeHierarchy(int level)
        {
            FieldInfo fi = this.GetType().GetField("txtCodeHierarachyLvl" + level, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi != null)
            {
                return (TextBox)fi.GetValue(this);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retorna el text box correspondiente a la descripcion del codigo
        /// del nivel de la jerarquia
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private TextBox TxtDescHierarchy(int level)
        {
            FieldInfo fi = this.GetType().GetField("txtBtnCodeHierarachyLvl" + level, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi != null)
            {
                return (TextBox)fi.GetValue(this);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retorna el boton correspondiente a la busqueda del codigo
        /// del nivel de la jerarquia
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private Button BtnHierarchy(int level)
        {
            FieldInfo fi = this.GetType().GetField("btnEditJerar" + level, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi != null)
            {
                return (Button)fi.GetValue(this);
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Funciones Protected

        /// <summary>
        /// Muestra los niveles mostrados en la tabla
        /// </summary>
        /// <returns>cantidad de niveles usados</returns>
        protected int MaxShownLevel()
        {
            if (this.maxLevel != null && this.maxLevel != 0)
            {
                return Math.Min(this.maxLevel.Value, this._table.LevelsUsed());
            }
            else
                return this._table.LevelsUsed();

        }

        #endregion

        #region Funciones publicas

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="t">Tabla del sistema</param>
        /// <param name="config">Argumentos para el formulario modal</param>
        /// <param name="selLevel">Opcional. Si no se proporciona solo permite seleccionar hojas(ultimo nivel)
        /// Si es 0, permite de cualquier nivel, Si es mayor de 0 solo permite seleccionar elementos de cierto nivel
        /// de la jerarqui</param>
        public void InitControl(DTO_glTabla t, ForeignKeyFieldConfig config, int? selLevel = null)
        {
            this._table = t;
            this.maxLevel = selLevel;
            //Ponerle limite a los componentes de la jerarquia
            this.txtCodeHierarachyLvl1.MaxLength = Convert.ToInt32(_table.lonNivel1.Value);
            this.txtCodeHierarachyLvl2.MaxLength = Convert.ToInt32(_table.lonNivel2.Value);
            this.txtCodeHierarachyLvl3.MaxLength = Convert.ToInt32(_table.lonNivel3.Value);
            this.txtCodeHierarachyLvl4.MaxLength = Convert.ToInt32(_table.lonNivel4.Value);
            this.txtCodeHierarachyLvl5.MaxLength = Convert.ToInt32(_table.lonNivel5.Value);
            this.txtCodeHierarachyLvl6.MaxLength = Convert.ToInt32(_table.lonNivel6.Value);
            this.txtCodeHierarachyLvl7.MaxLength = Convert.ToInt32(_table.lonNivel7.Value);
            this.txtCodeHierarachyLvl8.MaxLength = Convert.ToInt32(_table.lonNivel8.Value);
            this.txtCodeHierarachyLvl9.MaxLength = Convert.ToInt32(_table.lonNivel9.Value);
            this.txtCodeHierarachyLvl10.MaxLength = Convert.ToInt32(_table.lonNivel10.Value);

            this.lblCodeHier.Text = this.TextCode;
            this.lblDescrHier.Text = this.TextDescr;
            this.DocumentID = this._table.DocumentoID.Value.Value;
            this.FrmProperties = _bc.AdministrationModel.MasterProperties[this.DocumentID];
            this.EmpresaGrupoID = _bc.GetMaestraEmpresaGrupoByDocumentID(this.DocumentID);
            this._fkConfig = config;
            this.btnReturn.Text = this._textButtonSelect;
            this.LabelHierarchy(1).Text = this._table.LevelDescs()[0];
            for (int i = 2; i <= this.MaxShownLevel(); i++)
            {
                if (this.TxtCodeHierarchy(i) != null)
                {
                    this.TxtCodeHierarchy(i).Text = string.Empty;
                    this.TxtCodeHierarchy(i).Enabled = false;
                }
                if (this.BtnHierarchy(i) != null)
                {
                    this.BtnHierarchy(i).Visible = false;
                    this.BtnHierarchy(i).Enabled = true;
                }
                if (this.LabelHierarchy(i) != null)
                    this.LabelHierarchy(i).Text = this._table.LevelDescs()[i - 1];
            }
            for (int i = this.MaxShownLevel() + 1; i <= DTO_glTabla.MaxLevels; i++)
            {
                int level = i;
                if (this.LabelHierarchy(level) != null)
                    this.LabelHierarchy(level).Visible = false;
                if (this.TxtCodeHierarchy(level) != null)
                {
                    this.TxtCodeHierarchy(level).Visible = false;
                    this.TxtCodeHierarchy(level).Text = string.Empty;
                    this.TxtCodeHierarchy(level).Enabled = true;
                }
                if (this.TxtDescHierarchy(level) != null)
                {
                    this.TxtDescHierarchy(level).Visible = false;
                    this.TxtDescHierarchy(level).Text = string.Empty;
                }
                if (this.BtnHierarchy(level) != null)
                {
                    this.BtnHierarchy(level).Visible = false;
                    this.BtnHierarchy(level).Enabled = true;
                }
            }
            this.btnReturn.Enabled = false;
        }

        /// <summary>
        /// Inicializa el control basado en paramtros 
        /// </summary>
        /// <param name="t">Tabla del sistema</param>
        /// <param name="keyField">Pk de la tabla</param>
        /// <param name="descField">Campo de descripcion</param>
        /// <param name="countMethod">metodo de saber el numero de registros</param>
        /// <param name="dataMethod">Metodo para traer todo el listado de datos</param>
        /// <param name="dataRowMethod">Metodo para raer un registro por el id</param>
        /// <param name="args">Argumentos extras</param>
        /// <param name="modalFormCode">Codigo del formulario modal</param>
        /// <param name="tableName">Tabla del sistema</param>
        public void InitFindControl(DTO_glTabla t, string keyField, string descField, string countMethod, string dataMethod, string dataRowMethod, string modalFormCode, string tableName, List<DTO_glConsultaFiltro> filtros)
        {
            this.Filtros = filtros;

            ForeignKeyFieldConfig config = new ForeignKeyFieldConfig()
            {
                KeyField = keyField,
                DescField = descField,
                CountMethod = countMethod,
                DataMethod = dataMethod,
                DataRowMethod = dataMethod,
                ModalFormCode = modalFormCode,
                TableName = tableName
            };

            this.InitControl(t, config);
        }

        /// <summary>
        /// Reinicia la lista de jerarquias
        /// </summary>
        public void ResetHierarchy()
        {
            //this._editando = false;
            //_idSelected = null;
            //_hierarchies = new Dictionary<string, DTO_hierarchy>();
        }

        /// <summary>
        /// Agrega un nuevo elemento al diccionario de datos
        /// </summary>
        /// <param name="id">Id del diccionario</param>
        /// <param name="h">Jerarquia del elemento</param>
        public void AddData(string id, DTO_hierarchy h)
        {
            //_hierarchies.Add(id, h);
        }

        /// <summary>
        /// limpia la seccion de jerarquia para iniciar una insercion
        /// </summary>
        public void CleanHierarchy()
        {
            int level;

            level = 1;
            if (this.LabelHierarchy(level) != null)
                this.LabelHierarchy(level).Visible = true;
            if (this.TxtCodeHierarchy(level) != null)
            {
                this.TxtCodeHierarchy(level).Visible = true;
                this.TxtCodeHierarchy(level).Enabled = true;
                this.TxtCodeHierarchy(level).Text = string.Empty;
            }
            if (this.TxtDescHierarchy(level) != null)
            {
                this.TxtDescHierarchy(level).Visible = true;
                this.TxtDescHierarchy(level).Text = string.Empty;
            }
            if (this.BtnHierarchy(level) != null)
            {
                this.BtnHierarchy(level).Visible = true;
                this.BtnHierarchy(level).Enabled = true;
            }

            for (int i = 1; i < DTO_glTabla.MaxLevels; i++)
            {
                level = i + 1;
                if (this.LabelHierarchy(level) != null)
                    this.LabelHierarchy(level).Visible = false;
                if (this.TxtCodeHierarchy(level) != null)
                {
                    this.TxtCodeHierarchy(level).Visible = false;
                    this.TxtCodeHierarchy(level).Text = string.Empty;
                    this.TxtCodeHierarchy(level).Enabled = true;
                }
                if (this.TxtDescHierarchy(level) != null)
                {
                    this.TxtDescHierarchy(level).Visible = false;
                    this.TxtDescHierarchy(level).Text = string.Empty;
                }
                if (this.BtnHierarchy(level) != null)
                {
                    this.BtnHierarchy(level).Visible = false;
                    this.BtnHierarchy(level).Enabled = true;
                }
            }

        }

        /// <summary>
        /// Devuelve la cantidad de niveles
        /// </summary>
        /// <returns>Devuelve la cantidad de niveles</returns>
        public int HierarchyLevels()
        {
            return this._table.LevelsUsed();
        }

        /// <summary>
        /// Devuelve la longitud
        /// </summary>
        /// <param name="level">Nivel</param>
        /// <returns>Retorna la longitud del campo</returns>
        public int CodeLength(int level)
        {
            return this._table.CodeLength(level);
        }

        /// <summary>
        /// Devuelve el numero del nivel al que corresponde un codigo
        /// Si el codigo no corresponde devuelve 0
        /// </summary>
        /// <param name="code">Codigo a evaluar</param>
        /// <returns>del 1 al 5 si corresponde con algun nivel de la jerarquia, 0 si no</returns>
        public int CodeLevel(string code)
        {
            int codeLength = code.Length;
            int levels = this.HierarchyLevels();
            for (int i = 1; i <= levels; i++)
            {
                if (codeLength == this.CodeLength(i))
                    return i;
            }
            //Si pasa aca es que no cuedra con la jerarquia
            return 0;
        }

        /// <summary>
        /// Completa el codigo de la jerarquia
        /// </summary>
        /// <returns>Retorna el codigo correspondiente</returns>
        public string CompleteCode()
        {
            string buffer = "";
            for (int i = 0; i < this._table.LevelsUsed(); i++)
            {
                buffer += this.TxtCodeHierarchy(i + 1).Text;
            }
            return buffer;
        }

        #endregion

        #region Eventos

        #region Click de los botones de búsqueda

        /// <summary>
        /// Boton para buscar datos del primer nivel
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnEditJerar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            int level=Convert.ToInt32(b.Name.Substring("btnEditJerar".Length));
            UDT_BasicID idParent=new UDT_BasicID();
            for (int i = 1; i < level;i++ )
                idParent.Value += this.TxtCodeHierarchy(i).Text;
            Form f = new ModalMasterHierarchy(this.TxtCodeHierarchy(level), idParent, this.DocumentID, this.FrmProperties.DocumentoID.ToString(), this.FrmProperties.ColumnaID, this._fkConfig.DescField, this.Filtros);
            f.ShowDialog();
        }
      
        #endregion

        #region Cambios en el texto ingresado (tab)

        /// <summary>
        /// Cambio el codigo del nivel de la jerarquia
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtCodeHierarachyLvl_TextChanged(object sender, EventArgs e)
        {
            TextBox txtBox=(TextBox)sender;
            int level = Convert.ToInt32(txtBox.Name.Substring("txtCodeHierarachyLvl".Length));
            
            string padre=string.Empty;
            
            for (int i=1;i<level;i++)
                padre+= this.TxtCodeHierarchy(i).Text;
            
            string code = padre + this.TxtCodeHierarchy(level).Text;

            #region Para el ultimo nivel

            //if (level==this._table.LevelsUsed()){
            //    if (code.Length == this.CodeLength(5))
            //    {
            //        this.SetCode(code);
            //    }
            //    else
            //    {
            //        this.SetCode("");
            //    }
            //    return;
            //}
            #endregion

            this.ProcessCodeInput(level, code);
        }

        /// <summary>
        /// Identifica la tecla presionada para ejecutar una accion
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtCodeHierarachyLvl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        #endregion

        #region Validacion Ingreso de valores

        /// <summary>
        /// Al presionar cualquier tecla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtCodeHierarachyLvl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetterOrDigit(e.KeyChar) || Char.IsControl(e.KeyChar))// || Char.IsWhiteSpace(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        #endregion

        /// <summary>
        /// Se ejecuta cuando se debe cerrar el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MasterHierarchyFind_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Inicializa la busqueda
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.InitControl(this._table, this._fkConfig, this.maxLevel);

            this.TxtCodeHierarchy(1).Text = string.Empty;
            this.TxtCodeHierarchy(1).Enabled = true;
            this.txtBtnCodeHierarachyLvl1.Text = string.Empty;
            this.BtnHierarchy(1).Enabled = true;
            this.txtCodeHierarachyLvl1.Focus();
            for (int i = 2; i <= this.MaxShownLevel(); i++)
            {
                if (this.TxtCodeHierarchy(i) != null)
                {
                    this.TxtCodeHierarchy(i).Text = string.Empty;
                    this.TxtCodeHierarchy(i).Enabled = false;
                }
                if (this.BtnHierarchy(i) != null)
                {
                    this.BtnHierarchy(i).Visible = false;
                    this.BtnHierarchy(i).Enabled = true;
                }
                if (this.LabelHierarchy(i) != null)
                    this.LabelHierarchy(i).Text = this._table.LevelDescs()[i - 1];

                FieldInfo fi = this.GetType().GetField("txtBtnCodeHierarachyLvl" + i, BindingFlags.Instance | BindingFlags.NonPublic);
                if (fi != null)
                {
                    ((TextBox)fi.GetValue(this)).Text = string.Empty; ;
                }

            }

        }

        /// <summary>
        /// Cierra la forma
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Evento que se ejecuta al presionar una tecla
        /// </summary>
        /// <param name="msg">Mensaje del evento</param>
        /// <param name="keyData">tecla presionada</param>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion       

    }
}


