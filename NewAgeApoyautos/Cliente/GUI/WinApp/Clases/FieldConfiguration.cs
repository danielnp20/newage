using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using System.Windows.Forms;
using NewAge.DTO.UDT;

namespace NewAge.Cliente.GUI.WinApp.Clases
{

    #region Funciones Publicas

    /// <summary>
    /// Clase con las configuraciones de una columna para la grilla
    /// </summary>
    public class FieldConfiguration
    {
        /// <summary>
        /// Indica el tipo de dato
        /// </summary>
        public Type ValueType = typeof(string);

        /// <summary>
        /// nombre del campo
        /// </summary>
        public string FieldName;

        /// <summary>
        /// Numero de la fila en la grilla de edición
        /// </summary>
        public int RowIndex;

        /// <summary>
        /// indica el orden de las columnas de la grilla
        /// </summary>
        public int ColumnIndex;

        /// <summary>
        /// indica el ancho de las columnas de la grilla
        /// </summary>
        public int ColumnWidth;

        /// <summary>
        /// Indica si es visible en la grilla de edición
        /// </summary>
        public bool EditVisible = true;
        
        /// <summary>
        /// Indica si es visible en la grilla
        /// </summary>
        public bool GridVisible = true;

        /// <summary>
        /// nombre con el que sale el campo
        /// </summary>
        public string Caption;

        /// <summary>
        /// indica si se puede editar
        /// </summary>
        public bool Editable = true;

        /// <summary>
        /// Indica si permite valores nulos
        /// </summary>
        public bool AllowNull = false;

        /// <summary>
        /// Nombre del tab de la grilla de edición en el que se debe mostrar. Vacio si es uno nada más
        /// </summary>
        public string Tab=string.Empty;

        /// <summary>
        /// Nombre del tab de la grilla de edición en el que se debe mostrar. Vacio si es uno nada más
        /// </summary>
        public bool Fixed = false;
    }

    /// <summary>
    /// Clase para un campo de texto
    /// </summary>
    public class TextFieldConfiguration : FieldConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t">Tipo de dato</param>
        /// <param name="maxLenght">Tamaño máximo permitido</param>
        /// <param name="casing">Comportamiento de los caracteres</param>
        /// <param name="type">Tipos de datos permitidos (segun enum)</param>
        /// <param name="editable">Indica si se puede editar la información</param>
        public TextFieldConfiguration(Type t, int maxLenght, CharacterCasing casing, TextFieldType type, bool editable)
        {
            this.ValueType = t;
            this.MaxLength = maxLenght;
            this.Casing = casing;
            this.TextType = type;
            this.Editable = editable;
        }

        /// <summary>
        /// Longitud Maxima del Campo del codigo
        /// </summary>
        public int MaxLength=50;

        /// <summary>
        /// Casing del código
        /// </summary>
        public CharacterCasing Casing = CharacterCasing.Normal;

        /// <summary>
        /// Indica si acepta simbolos en la
        /// </summary>
        public TextFieldType TextType = TextFieldType.Everything;

        /// <summary>
        /// Expresion regular para limitar los tipos de datos
        /// </summary>
        public string Regex=UDT.DefaultRegex;
    }

    /// <summary>
    /// Clase para un campo de checkbox
    /// </summary>
    public class CheckFieldConfiguration : FieldConfiguration
    {
        public CheckFieldConfiguration()
        {
            this.ValueType = typeof(bool);
        }
    }

    public class IntRankConfiguration : FieldConfiguration
    {

        public string[] Options;

        /// <summary>
        /// Expresion regular para limitar los tipos de datos
        /// </summary>
        public string Regex = UDT.DefaultRegex;

        public IntRankConfiguration(string[] arrOpt)
        {
            Options = arrOpt;
        }
    }

    /// <summary>
    /// Clase para un boton para las FKs
    /// </summary>
    public class ButtonEditFKConfiguration : TextFieldConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t">Tipo de dato</param>
        /// <param name="maxLenght">Tamaño máximo permitido</param>
        /// <param name="casing">Comportamiento de los caracteres</param>
        /// <param name="type">Tipos de datos permitidos (segun enum)</param>
        /// <param name="editable">Indica si se puede editar la información</param>
        /// <param name="fkConfig">configuracion de la llave foranea</param>
        public ButtonEditFKConfiguration(Type t, int maxLenght, CharacterCasing casing, TextFieldType type, bool editable, ForeignKeyFieldConfig fkConfig)
            : base(t, maxLenght, casing, type, editable)
        {
            this.FkConfig = fkConfig;
        }
        /// <summary>
        /// Configuracion del llamado a la llave foranea
        /// </summary>
        /// 
        public ForeignKeyFieldConfig FkConfig;

        /// <summary>
        /// Filtros para usar en el FK
        /// </summary>
        public List<DTO_glConsultaFiltro> Filtros = new List<DTO_glConsultaFiltro>();
    }

    /// <summary>
    /// Clase para guardar la configuracion de una llave foranea
    /// </summary>
    public class ForeignKeyFieldConfig
    {
        /// <summary>
        /// Campo que contiene la llave
        /// </summary>
        public string KeyField;

        /// <summary>
        /// Campo que contiene la descripción
        /// </summary>
        public string DescField;

        /// <summary>
        /// Metodo para contar los datos
        /// </summary>
        public string CountMethod;

        /// <summary>
        /// Metodo para traer los datos de la llave foranea
        /// </summary>
        public string DataMethod;

        /// <summary>
        /// Metodo para traer una fila de acuerdo al id
        /// </summary>
        public string DataRowMethod;

        /// <summary>
        /// argumentos extra
        /// </summary>
        public Object[] args;

        /// <summary>
        /// Codigo del formulario a mostrar
        /// </summary>
        public string ModalFormCode;

        /// <summary>
        /// El nombre de la tabla a la que apunta el FK
        /// </summary>
        public string TableName;
    }

    public class RichTextFieldConfiguration : FieldConfiguration
    {
        public bool isHtml = true;
    }

    public class ImageFieldConfiguration : FieldConfiguration
    {

    }

    #endregion

}
