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

namespace NewAge.Cliente.GUI.WinApp.ControlsUC
{
    /// <summary>
    /// control de usuario para el manejo de jerarquías
    /// </summary>
    public partial class uc_Hierarchy : UserControl
    {
        #region Variables

        //Tabla de glTabla (contiene la info de la jerarquia)
        private DTO_glTabla _table;
        //Diccionario con el id del dto y el cmpo de la jerarquia
        private Dictionary<string, DTO_hierarchy> _hierarchies = new Dictionary<string, DTO_hierarchy>();
        //Formulario modal que se debe abrir
        private Type _formType;
        //Id seleccionado
        private string _idSelected;
        //Variable para indicar si cambio un texto de un hoja a un elemento anterior
        private bool _updateGridCode;

        //Identificador del documento
        protected int docId;
        //Nombre columna id
        protected string colId;
        //Nombre columna descriptivo
        protected string colDesc;

        //Indicador si es para un nuevo registro
        public bool Insertando;
        //Indicador si es para un nuevo registro
        public bool Editando;

        #endregion
        
        #region Propiedades

        /// <summary>
        /// Asigna el titulo del codigo
        /// </summary>
        public string TextCode = "msg_code";

        /// <summary>
        /// Asigna el titulo de la descripción
        /// </summary>
        public string TextDescr = "msg_description";

        public bool Editable
        {
            set
            {
                for (int i = 1; i <= HierarchyLevels(); i++)
                {
                    this.TxtCodeHierarchy(i).ReadOnly = !value;
                }
            }
        }
       
        #endregion

        #region Declaración Handlers

        /// <summary>
        /// Agrega una columna de la jerarquia al control
        /// Crea una instancia del handler
        /// Crea una propiedad para el handler
        /// </summary>
        /// <param name="col">Columna que se va agregar</param>
        /// <param name="index">Indice de la columna</param>
        public delegate void AddColumnHandler(GridColumn col, int index);
        AddColumnHandler addHierarchyCol;
        new public event AddColumnHandler AddHierarchyCol
        {
            add { this.addHierarchyCol += value; }
            remove { this.addHierarchyCol -= value; }
        }

        /// <summary>
        /// Completa la descripcion dado el codigo
        /// Si no existe muestra el cuadro de dialogo para seleccionar uno
        /// Crea una instancia del handler
        /// Crea una propiedad para el handler
        /// </summary>
        /// <param name="txtCode">Control del código</param>
        /// <param name="txtDescription">Control de la descripción</param>
        /// <param name="code">Codigo ingresado</param>
        /// <param name="padre">Código del padre en la jerarquia</param>
        public delegate void FillLevelHandler(TextBox txtCode, TextBox txtDescription, string code, string padre);
        FillLevelHandler fillLevel;
        new public event FillLevelHandler FillLevel
        {
            add { this.fillLevel += value; }
            remove { this.fillLevel -= value; }
        }

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

        /// <summary>
        /// Pone el valor del codigo en el formulario del cual se llamo el componente
        /// </summary>
        /// <param name="code">Nuevo codigo</param>
        public delegate void UpdateEditGridHandler(string code);
        UpdateEditGridHandler updateEditGrid;
        new public event UpdateEditGridHandler UpdateEditGrid
        {
            add { this.updateEditGrid += value; }
            remove { this.updateEditGrid -= value; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public uc_Hierarchy()
        {
            InitializeComponent();
        }

        #region Funciones Privadas

        /// <summary>
        /// Retorna el label correspondiente al nombre del nivel de la jerarquia
        /// </summary>
        /// <param name="level">Nivel del código</param>
        /// <returns>Label</returns>
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

        #region Funciones Publicas

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fType">Formulario</param>
        /// <param name="t">Tabla del sistema</param>
        /// <param name="modalArgs">Argumentos para el formulario modal</param>
        public void InitControl(Type fType, DTO_glTabla t, int docId, string colId, string colDesc)
        {
            this._formType = fType;
            this._table = t;
            this.docId = docId;
            this.colId = colId;
            this.colDesc = colDesc;
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

            this.txtCodeHierarachyLvl1.CharacterCasing = CharacterCasing.Upper;
            this.txtCodeHierarachyLvl2.CharacterCasing = CharacterCasing.Upper;
            this.txtCodeHierarachyLvl3.CharacterCasing = CharacterCasing.Upper;
            this.txtCodeHierarachyLvl4.CharacterCasing = CharacterCasing.Upper;
            this.txtCodeHierarachyLvl5.CharacterCasing = CharacterCasing.Upper;
            this.txtCodeHierarachyLvl6.CharacterCasing = CharacterCasing.Upper;
            this.txtCodeHierarachyLvl7.CharacterCasing = CharacterCasing.Upper;
            this.txtCodeHierarachyLvl8.CharacterCasing = CharacterCasing.Upper;
            this.txtCodeHierarachyLvl9.CharacterCasing = CharacterCasing.Upper;
            this.txtCodeHierarachyLvl10.CharacterCasing = CharacterCasing.Upper;

            this.lblCodeHier.Text = this.TextCode;
            this.lblDescrHier.Text = this.TextDescr;
        }

        /// <summary>
        /// Reinicia la lista de jerarquias
        /// </summary>
        public void ResetHierarchy()
        {
            this.Editando = false;
            _idSelected = null;
            _hierarchies = new Dictionary<string, DTO_hierarchy>();
        }

        /// <summary>
        /// Agrega un nuevo elemento al diccionario de datos
        /// </summary>
        /// <param name="id">Id del diccionario</param>
        /// <param name="h">Jerarquia del elemento</param>
        public void AddData(string id, DTO_hierarchy h)
        {
            _hierarchies.Add(id, h);
        }

        /// <summary>
        /// Asigna la jerarquia a los controles
        /// </summary>
        /// <param name="grlModule">Control con los campos (referencia)</param>
        public int AssignHierarchy()
        {
            int fIndex = 0;
            int[] lengths=this._table.LevelLengths();
            string[] descs = this._table.LevelDescs();
            for (int i=0;i<this._table.LevelsUsed();i++){
                if (lengths[i] != null && lengths[i] != 0)
                {
                    GridColumn jCol = new GridColumn();
                    jCol.FieldName = descs[i];
                    jCol.Caption = descs[i];
                    jCol.UnboundType = UnboundColumnType.Object;
                    jCol.Width = 35;
                    //jCol.UnboundExpression = "Jerarquia.Codigos[" + i+ "]";

                    if (this.addHierarchyCol != null)
                        this.addHierarchyCol(jCol, fIndex);

                    fIndex++;
                    this.LabelHierarchy(i + 1).Text=descs[i];
                }
            }

            return fIndex;
        }

        /// <summary>
        /// Pone los datos del cuadro segun el registro seleccionado
        /// </summary>
        public void FillHierarchySelected(string localizacionIdSelected)
        {
            _updateGridCode = false;
            this.Editando = true;
            _idSelected = localizacionIdSelected;
            DTO_hierarchy hierarchy = new DTO_hierarchy();
            if (this._hierarchies.TryGetValue(localizacionIdSelected, out hierarchy))
            {
                //Obtiene el nivel de la jerarquia seleccionado
                int instanceLevel = hierarchy.NivelInstancia;
                if (instanceLevel != this._table.LevelsUsed())
                {
                    this._updateGridCode = true;
                }

                #region Mostrar/Ocultar controles

                for (int i = 0; i < DTO_glTabla.MaxLevels; i++)
                //for (int i = (DTO_glTabla.MaxLevels-1); i >= 0; i--)
                {
                    int level = i + 1;
                    if (this.LabelHierarchy(level) != null)
                        this.LabelHierarchy(level).Visible = (i < instanceLevel);

                    if (this.TxtCodeHierarchy(level) != null)
                    {
                        this.TxtCodeHierarchy(level).Visible = (i < instanceLevel);

                        if (i == (instanceLevel - 1))
                            this.TxtCodeHierarchy(level).Enabled = true;
                        else
                            this.TxtCodeHierarchy(level).Enabled = false;

                        //si el textbox no es el del nivel de la jerarquia seleccionada
                        if (i <= instanceLevel)
                            this.TxtCodeHierarchy(level).Text = hierarchy.Codigos[i];
                        else
                            this.TxtCodeHierarchy(level).Text = string.Empty;
                    }

                    if (this.TxtDescHierarchy(level) != null)
                    {
                        this.TxtDescHierarchy(level).Visible = (i < instanceLevel);
                        if (i < instanceLevel)
                            this.TxtDescHierarchy(level).Text = hierarchy.Descripciones[i];
                        else
                            this.TxtDescHierarchy(level).Text = string.Empty;
                    }

                    if (this.BtnHierarchy(level) != null)
                        this.BtnHierarchy(level).Visible = false;
                }

                #endregion
            }
        }

        /// <summary>
        /// limpia la seccion de jerarquia para iniciar una insercion
        /// </summary>
        public void CleanHierarchy()
        {
            _updateGridCode = false;
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
            int levels= this.HierarchyLevels();
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
            string buffer="";
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
            var currentParams = new Object[7];
            currentParams[0] = this.TxtCodeHierarchy(level);
            UDT_BasicID idParent=new UDT_BasicID();
            for (int i = 1; i < level; i++)
            {
                idParent.Value += this.TxtCodeHierarchy(i).Text;
            }
            currentParams[1] = idParent;
            currentParams[2] = docId;
            currentParams[3] = docId.ToString();
            currentParams[4] = this.colId;
            currentParams[5] = this.colDesc;
            currentParams[6] = new List<DTO_glConsultaFiltro>();
            
            //InitArgs.CopyTo(currentParams, 2);
            //(this.TxtCodeHierarchy(level), idParent, this.DocumentID, this.FrmProperties.DocumentoID.ToString(), this.FrmProperties.ColumnaID, this._fkConfig.DescField)
            Form f = (Form)Activator.CreateInstance(this._formType, currentParams);
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

            if (level==this._table.LevelsUsed()){
                if (code.Length == this.CodeLength(this._table.LevelsUsed()) || this._updateGridCode)
                {
                    this.updateEditGrid(code);
                }
                else
                {
                    this.updateEditGrid("");
                }
                return;
            }
            #endregion

            if (this.Insertando)
            {
                this.checkLevelNew(level, code, this.TxtCodeHierarchy(level), this.TxtDescHierarchy(level), this.BtnHierarchy(level), this.TxtCodeHierarchy(level + 1), this.TxtDescHierarchy(level + 1), this.BtnHierarchy(level + 1), this.LabelHierarchy(level+1));
                this.TxtCodeHierarchy(level + 1).Focus();
            }
            else
            {
                DTO_hierarchy hierarchy = new DTO_hierarchy();
                if (this._hierarchies.TryGetValue(this._idSelected, out hierarchy))
                {
                    //this.TxtDescHierarchy(level).Text = string.Empty;
                    if (code.Length == this.CodeLength(hierarchy.NivelInstancia))
                    {
                        this.updateEditGrid(code);
                    }
                    else
                    {
                        this.updateEditGrid("");
                    }
                }
            }
        }

        #endregion

        #region Validacion Ingreso de valores

        private void txtCodeHierarachyLvl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetterOrDigit(e.KeyChar) || Char.IsControl(e.KeyChar))// || Char.IsWhiteSpace(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        #endregion

        #endregion

    }
}
