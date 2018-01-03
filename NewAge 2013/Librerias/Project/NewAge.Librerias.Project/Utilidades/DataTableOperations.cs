using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;
using System.IO;
using System.Data.SqlTypes;
using System.Globalization;
using System.Windows.Forms;

namespace NewAge.Librerias.Project
{
    public class DataTableOperations
    {
        #region Ennumeraciones

        /// <summary>
        /// Ennumeración con los tipos de archivos
        /// </summary>
        public enum FileType
        {
            txt = 1,
            xls = 2,
            xls_txt = 3
        }

        #region Estructuras

        /// <summary>
        /// Caracter que separa los campos
        /// </summary>
        private enum SeparadorCampo
        {
            NoTiene = 0,
            Coma = 1,
            PuntoComa = 2
        }

        /// <summary>
        /// Tipo de campo del primer detalle (se usa en la recepción)
        /// </summary>
        private enum PrimerRegTipoCampo
        {
            Texto = 1,
            Fecha = 2,
            Numero = 3,
            Valor = 4
        }

        #region Campos tipo número

        /// <summary>
        /// Longitud de los números
        /// </summary>
        private enum Numero_Longitud
        {
            Fija = 1,
            Variable = 2
        }

        /// <summary>
        /// que encierra los números
        /// </summary>
        private enum Numero_Encierra
        {
            NoTiene = 0,
            Comilla = 1,
            ComillaDoble = 2
        }

        /// <summary>
        /// Hacia donde se justifica un número
        /// </summary>
        private enum Numero_Justifica
        {
            Derecha = 1,
            Izquierda = 2
        }

        /// <summary>
        /// Separador de miles
        /// </summary>
        private enum Numero_SeparaMiles
        {
            NoTiene = 0,
            Coma = 1,
            Puntos = 2
        }

        #endregion

        #region Campos tipo valor

        /// <summary>
        /// Longitud de los valores
        /// </summary>
        private enum Valor_Longitud
        {
            Fija = 1,
            Variable = 2
        }

        /// <summary>
        /// que encierra los valores
        /// </summary>
        private enum Valor_Encierra
        {
            NoTiene = 0,
            Comilla = 1,
            ComillaDoble = 2
        }

        /// <summary>
        /// Hacia donde se justifica un valor
        /// </summary>
        private enum Valor_Justifica
        {
            Derecha = 1,
            Izquierda = 2
        }

        /// <summary>
        /// Separador de miles
        /// </summary>
        private enum Valor_SeparaMiles
        {
            NoTiene = 0,
            Coma = 1,
            Puntos = 2
        }

        /// <summary>
        /// Separador de decimales
        /// </summary>
        private enum Valor_SeparaDecimales
        {
            NoTiene = 0,
            Coma = 1,
            Puntos = 2
        }

        #endregion

        #region Campos tipo texto

        /// <summary>
        /// Longitud de los textos
        /// </summary>
        private enum Texto_Longitud
        {
            Fija = 1,
            Variable = 2
        }

        /// <summary>
        /// que encierra los textos
        /// </summary>
        private enum Texto_Encierra
        {
            NoTiene = 0,
            Comilla = 1,
            ComillaDoble = 2
        }

        /// <summary>
        /// Hacia donde se justifica un texto
        /// </summary>
        private enum Texto_Justifica
        {
            Derecha = 1,
            Izquierda = 2
        }

        #endregion

        #region Campos tipo Fecha

        /// <summary>
        /// Que encierra las fechas
        /// </summary>
        private enum Fecha_Encierra
        {
            NoTiene = 0,
            Comilla = 1,
            ComillaDoble = 2
        }

        /// <summary>
        /// Formato de las fechas
        /// </summary>
        private enum Fecha_Tipo
        {
            AñoMesDia = 1,
            AñoDiaMes = 2,
            MesDiaAño = 3,
            DiaMesAño = 4
        }

        /// <summary>
        /// Separador de las fechas
        /// </summary>
        private enum Fecha_Separador
        {
            NoTiene = 0,
            Slash = 1,
            Guion = 2
        }

        /// <summary>
        /// Formato del año
        /// </summary>
        private enum Fecha_Año
        {
            AAAA = 1,
            AA = 2
        }

        /// <summary>
        /// Formato del mes
        /// </summary>
        private enum Fecha_Mes
        {
            MM = 1,
            Español = 2,
            Esp3Letras = 3,
            Ingles = 4,
            Ing3Letras = 5
        }

        #endregion

        #endregion

        #region Campos

        /// <summary>
        /// Tipos de registro
        /// </summary>
        private enum TipoRegistro
        {
            NA = 0,
            Inicial = 1,
            Detalle = 2,
            SubDetalle = 3,
            Final = 4,
            Titulo = 5
        }

        /// <summary>
        /// Tipo de campo
        /// </summary>
        private enum TipoCampo
        {
            Texto = 1,
            Fecha = 2,
            Numero = 3,
            Valor = 4
        }

        #endregion

        #endregion

        #region Variables

        private string errorStr = "ERROR";

        //Carga de datos
        private DataTable tableTitles;
        private DataTable tableDetails;
        private DataTable tableSubDetails;
        private DataTable tableFinal;

        //Tupla: Titulo, TipoCampo, longitud, eliminaCerosIzq, noUtilizado
        private Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>> cols_Inicial = new Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>>();
        private Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>> cols_Detalle = new Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>>();
        private Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>> cols_SubDetalle = new Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>>();
        private Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>> cols_Final = new Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>>();

        //Información de estructuras
        private int primerRegDetalleLinea;
        private PrimerRegTipoCampo primerRegTipoCampo;
        private Numero_Longitud numero_Longitud;
        private Numero_Encierra numero_Encierra;
        private Numero_Justifica numero_Justifica;
        private Numero_SeparaMiles numero_SeparaMiles;
        private Valor_Longitud valor_Longitud;
        private Valor_Encierra valor_Encierra;
        private Valor_Justifica valor_Justifica;
        private Valor_SeparaMiles valor_SeparaMiles;
        private Valor_SeparaDecimales valor_SeparaDecimales;
        private Texto_Longitud texto_Longitud;
        private Texto_Encierra texto_Encierra;
        private Texto_Justifica texto_Justifica;
        private Fecha_Encierra fecha_Encierra;
        private Fecha_Tipo fecha_Tipo;
        private Fecha_Separador fecha_Separador;
        private Fecha_Año fecha_Año;
        private Fecha_Mes fecha_Mes;

        //Campos
        private string separadorCampo;
        private string nombreArchivo;
        private bool texto_MinusculasInd;
        private bool numero_CerosInd;
        private bool valor_CerosInd;
        private byte valor_Decimales;
        private string valor_CaracterEspecial;

        #endregion

        #region Propiedades

        /// <summary>
        /// Titles table
        /// </summary>
        public DataTable Table_Titles
        {
            get { return this.tableTitles; }
        }

        /// <summary>
        /// Details table
        /// </summary>
        public DataTable Table_Details
        {
            get { return this.tableDetails; }
        }

        /// <summary>
        /// SubDetails table
        /// </summary>
        public DataTable Table_SubDetails
        {
            get { return this.tableSubDetails; }
        }

        /// <summary>
        /// Final table
        /// </summary>
        public DataTable Table_Final
        {
            get { return this.tableFinal; }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DataTableOperations()
        {
            this.nombreArchivo = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Constructor con la estructura del documento
        /// </summary>
        /// <param name="structure">Estructura para importar y exportar</param>
        /// <param name="dType">Tipo de documento de retorno</param>
        public DataTableOperations(object structure, List<object> fields)
        {
            try
            {
                #region Variables

                //Propiedades
                PropertyInfo pi = null;

                //Campos
                Type fType = null;
                string tipoCampoStr = string.Empty;
                TipoRegistro tipoRegistro;

                //Variables de operación
                List<object> fields_init = new List<object>();
                List<object> fields_details = new List<object>();
                List<object> fields_subDetails = new List<object>();
                List<object> fields_final = new List<object>();

                //Tablas de resultados
                this.tableTitles = new DataTable("tableTitles");
                this.tableDetails = new DataTable("tableDetails");
                this.tableSubDetails = new DataTable("tableSubDetails");
                this.tableFinal = new DataTable("tableFinal");

                //Tupla: Titulo, TipoCampo, longitud, eliminaCerosIzq, noUtilizado
                this.cols_Inicial = new Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>>();
                this.cols_Detalle = new Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>>();
                this.cols_SubDetalle = new Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>>();
                this.cols_Final = new Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>>();

                #endregion

                //Carga la estructura
                this.LoadDataStructure(structure);

                // Carga las listas de campos de acuerdo al tipo
                foreach (object campoObj in fields)
                {
                    fType = campoObj.GetType();

                    //TipoRegistro
                    pi = fType.GetProperty("TipoRegistro");
                    tipoCampoStr = pi.GetValue(campoObj, null).ToString();
                    tipoRegistro = (TipoRegistro)Enum.Parse(typeof(TipoRegistro), tipoCampoStr);

                    switch (tipoRegistro)
                    {
                        case TipoRegistro.Inicial:
                            fields_init.Add(campoObj);
                            break;
                        case TipoRegistro.Detalle:
                            fields_details.Add(campoObj);
                            break;
                        case TipoRegistro.SubDetalle:
                            fields_subDetails.Add(campoObj);
                            break;
                        case TipoRegistro.Final:
                            fields_final.Add(campoObj);
                            break;
                    }
                }

                //Agrega el indicardor de validez a todas las tablas
                this.tableTitles.Columns.Add(new DataColumn("ValidRow"));
                this.tableDetails.Columns.Add(new DataColumn("ValidRow"));
                this.tableSubDetails.Columns.Add(new DataColumn("ValidRow"));
                this.tableFinal.Columns.Add(new DataColumn("ValidRow"));

                // Carga las lista de registros en orden
                this.LoadFieldsByType(TipoRegistro.Inicial, fields_init);
                this.LoadFieldsByType(TipoRegistro.Detalle, fields_details);
                this.LoadFieldsByType(TipoRegistro.SubDetalle, fields_subDetails);
                this.LoadFieldsByType(TipoRegistro.Final, fields_final);
            }
            catch(Exception)
            {
                throw;
            }
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// Carga la estructura de la tabla
        /// <param name="structure">Objeto que contine toda la información para la estructura de los datos. Llena las enumeaciones y las variables</param>
        /// </summary>
        private void LoadDataStructure(object structure)
        {
            try
            {
                PropertyInfo pi = null;
                Type objType = structure.GetType();
                string valTxt = string.Empty;
                int valInt;
                byte valByte;
                bool valBool;

                #region Ennumeraciones

                //PrimerRegTipoCampo 
                pi = objType.GetProperty("PrimerRegDetalleTipoCpo");
                valTxt = pi.GetValue(structure, null).ToString();
                if(!string.IsNullOrWhiteSpace(valTxt))
                    this.primerRegTipoCampo = (PrimerRegTipoCampo)Enum.Parse(typeof(PrimerRegTipoCampo), valTxt);

                //Numero_Longitud 
                pi = objType.GetProperty("CpoNumeroLogitud");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.numero_Longitud = (Numero_Longitud)Enum.Parse(typeof(Numero_Longitud), valTxt);

                //Numero_Encierra 
                pi = objType.GetProperty("CpoNumeroEncierra");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.numero_Encierra = (Numero_Encierra)Enum.Parse(typeof(Numero_Encierra), valTxt);

                //Numero_Justifica 
                pi = objType.GetProperty("CpoNumeroJustifica");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.numero_Justifica = (Numero_Justifica)Enum.Parse(typeof(Numero_Justifica), valTxt);

                //Numero_SeparaMiles 
                pi = objType.GetProperty("CpoNumeroMilSepara");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.numero_SeparaMiles = (Numero_SeparaMiles)Enum.Parse(typeof(Numero_SeparaMiles), valTxt);

                //Valor_Longitud 
                pi = objType.GetProperty("CpoValorLogitud");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.valor_Longitud = (Valor_Longitud)Enum.Parse(typeof(Valor_Longitud), valTxt);

                //Valor_Encierra 
                pi = objType.GetProperty("CpoValorEncierra");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.valor_Encierra = (Valor_Encierra)Enum.Parse(typeof(Valor_Encierra), valTxt);

                //Valor_Justifica 
                pi = objType.GetProperty("CpoValorJustifica");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.valor_Justifica = (Valor_Justifica)Enum.Parse(typeof(Valor_Justifica), valTxt);

                //Valor_SeparaMiles 
                pi = objType.GetProperty("CpoValorMilSepara");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.valor_SeparaMiles = (Valor_SeparaMiles)Enum.Parse(typeof(Valor_SeparaMiles), valTxt);

                //Valor_SeparaDecimales 
                pi = objType.GetProperty("CpoValorDecimalSepara");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.valor_SeparaDecimales = (Valor_SeparaDecimales)Enum.Parse(typeof(Valor_SeparaDecimales), valTxt);

                //Texto_Longitud 
                pi = objType.GetProperty("CpoTextoLogitud");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.texto_Longitud = (Texto_Longitud)Enum.Parse(typeof(Texto_Longitud), valTxt);

                //Texto_Encierra 
                pi = objType.GetProperty("CpoTextoEncierra");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.texto_Encierra = (Texto_Encierra)Enum.Parse(typeof(Texto_Encierra), valTxt);

                //Texto_Justifica 
                pi = objType.GetProperty("CpoTextoJustifica");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.texto_Justifica = (Texto_Justifica)Enum.Parse(typeof(Texto_Justifica), valTxt);

                //Fecha_Encierra 
                pi = objType.GetProperty("CpoFechaEncierra");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.fecha_Encierra = (Fecha_Encierra)Enum.Parse(typeof(Fecha_Encierra), valTxt);

                //Fecha_Tipo 
                pi = objType.GetProperty("CpoFechaTipo");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.fecha_Tipo = (Fecha_Tipo)Enum.Parse(typeof(Fecha_Tipo), valTxt);

                //Fecha_Separador 
                pi = objType.GetProperty("CpoFechaSeparador");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.fecha_Separador = (Fecha_Separador)Enum.Parse(typeof(Fecha_Separador), valTxt);

                //Fecha_Año 
                pi = objType.GetProperty("CpoFechaAno");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.fecha_Año = (Fecha_Año)Enum.Parse(typeof(Fecha_Año), valTxt);

                //Fecha_Mes 
                pi = objType.GetProperty("CpoFechaMes");
                valTxt = pi.GetValue(structure, null).ToString();
                if (!string.IsNullOrWhiteSpace(valTxt))
                    this.fecha_Mes = (Fecha_Mes)Enum.Parse(typeof(Fecha_Mes), valTxt);

                #endregion

                #region Variables

                //SeparadorCampo 
                pi = objType.GetProperty("SeparadorCampo");
                valTxt = pi.GetValue(structure, null).ToString();
                SeparadorCampo separador = (SeparadorCampo)Enum.Parse(typeof(SeparadorCampo), valTxt);

                //if (this.fType == FileType.xls)
                //{
                //    this.separadorCampo = "\t";
                //}
                //else
                //{
                    if (separador == SeparadorCampo.Coma)
                        this.separadorCampo = ",";
                    else if (separador == SeparadorCampo.PuntoComa)
                        this.separadorCampo = ";";
                    else
                        this.separadorCampo = string.Empty;
                //}

                //NombreArchivo
                pi = objType.GetProperty("NombreArchivo");
                this.nombreArchivo = pi.GetValue(structure, null).ToString();

                //Primer registro detalle linea (Numero de linea del primer registro de detalle)
                pi = objType.GetProperty("PrimerRegDetalleLinea");
                valTxt = pi.GetValue(structure, null).ToString();
                valInt = !string.IsNullOrWhiteSpace(valTxt) ? Convert.ToInt32(valTxt) : 0;
                this.primerRegDetalleLinea = valInt == 0 || valInt == 1 ? 1 : valInt;

                //Texto
                pi = objType.GetProperty("CpoTextoMinusculasInd");
                valTxt = pi.GetValue(structure, null).ToString();
                valBool = !string.IsNullOrWhiteSpace(valTxt) ? Convert.ToBoolean(valTxt) : false;
                this.texto_MinusculasInd = valBool;

                //Numero
                pi = objType.GetProperty("CpoNumeroCerosInd");
                valTxt = pi.GetValue(structure, null).ToString();
                valBool = !string.IsNullOrWhiteSpace(valTxt) ? Convert.ToBoolean(valTxt) : false;
                this.numero_CerosInd = valBool;

                //Valor
                pi = objType.GetProperty("CpoValorCerosInd");
                valTxt = pi.GetValue(structure, null).ToString();
                valBool = !string.IsNullOrWhiteSpace(valTxt) ? Convert.ToBoolean(valTxt) : false;
                this.valor_CerosInd = valBool;

                pi = objType.GetProperty("CpoValorDecimales");
                valTxt = pi.GetValue(structure, null).ToString();
                valByte = !string.IsNullOrWhiteSpace(valTxt) ? Convert.ToByte(valTxt) : (byte)0;
                this.valor_Decimales = valByte;

                pi = objType.GetProperty("CpoValorCaracterEspecial");
                this.valor_CaracterEspecial = pi.GetValue(structure, null).ToString();

                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Carga la información de las columnas según el tipo en su respectiva lista y agrega las columasn a la tabla
        /// Nota: Llamar esta funcón en orden
        /// </summary>
        /// <param name="rType">Tipo de registro (Inicial, Detalle, SubDetalle, Final)</param>
        /// <param name="fields">Lista de campos</param>
        private void LoadFieldsByType(TipoRegistro rType, List<object> fields)
        {
            #region Variables

            //Geréricas
            PropertyInfo pi = null;
            Type fType = null;

            //Campos
            string titulo;
            TipoCampo tipoCampo;
            string tipoCampoStr = string.Empty;
            string campoID;
            int orden;
            int longitud;
            bool eliminaCerosIzq;
            bool noUtilizado;

            //Info registro
            string val = string.Empty;

            //Tupla: Titulo, TipoCampo, longitud, eliminaCerosIzq, noUtilizado
            Tuple<string, TipoCampo, int, bool, bool> infoCampo;
            Dictionary<string, int> detallesTemp_Orden = new Dictionary<string, int>();
            Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>> detallesTemp_TipoCampo = new Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>>();

            #endregion

            #region Trae toda la información de los campos
            foreach (object campoObj in fields)
            {
                fType = campoObj.GetType();

                #region Carga las propiedades

                //Titulo
                pi = fType.GetProperty("Titulo");
                titulo = pi.GetValue(campoObj, null).ToString().Trim();

                //TipoCampo
                pi = fType.GetProperty("TipoCpo");
                tipoCampoStr = pi.GetValue(campoObj, null).ToString().Trim();
                tipoCampo = (TipoCampo)Enum.Parse(typeof(TipoCampo), tipoCampoStr);

                //Código campo
                pi = fType.GetProperty("CodigoCpo");
                campoID = pi.GetValue(campoObj, null).ToString().Trim();

                //Longitud del campo
                pi = fType.GetProperty("LongitudCpo");
                longitud = Convert.ToInt32(pi.GetValue(campoObj, null).ToString().Trim());

                //Elimina Ceros a la izquierda
                pi = fType.GetProperty("EliminaCeroIzqInd");
                eliminaCerosIzq = Convert.ToBoolean(pi.GetValue(campoObj, null).ToString().Trim());

                //Campo no utilizado
                pi = fType.GetProperty("NoUtilizadoInd");
                noUtilizado = Convert.ToBoolean(pi.GetValue(campoObj, null).ToString().Trim());

                #endregion
                #region Orden de los campos

                //Orden Campo
                pi = fType.GetProperty("DetalleNumero");
                orden = Convert.ToInt32(pi.GetValue(campoObj, null).ToString());

                infoCampo = new Tuple<string, TipoCampo, int, bool, bool>(titulo, tipoCampo, longitud, eliminaCerosIzq, noUtilizado);
                detallesTemp_Orden[campoID] = orden;
                detallesTemp_TipoCampo[campoID] = infoCampo;

                #endregion
            }
            #endregion

            #region Ordena la info

            var items = from pair in detallesTemp_Orden orderby pair.Value ascending select pair;
            foreach (KeyValuePair<string, int> pair in items)
            {
                //Tupla (infoCampo): Titulo, TipoCampo, longitud, eliminaCerosIzq, noUtilizado
                infoCampo = detallesTemp_TipoCampo[pair.Key];
                switch (rType)
                {
                    case TipoRegistro.Inicial:
                        this.tableTitles.Columns.Add(new DataColumn(pair.Key));
                        this.cols_Inicial.Add(pair.Key, infoCampo);
                        break;
                    case TipoRegistro.Detalle:
                        this.tableDetails.Columns.Add(new DataColumn(pair.Key));
                        this.cols_Detalle.Add(pair.Key, infoCampo);
                        break;
                    case TipoRegistro.SubDetalle:
                        this.tableSubDetails.Columns.Add(new DataColumn(pair.Key));
                        this.cols_SubDetalle.Add(pair.Key, infoCampo);
                        break;
                    case TipoRegistro.Final:
                        this.tableFinal.Columns.Add(new DataColumn(pair.Key));
                        this.cols_Final.Add(pair.Key, infoCampo);
                        break;
                }
            }

            #endregion
        }

        /// <summary>
        /// Pone formato a un campo de acuerdo con la infromación de la estructura
        /// </summary>
        /// <param name="value">Valor del campo</param>
        /// <param name="tipo">Tipo del campo</param>
        /// <param name="longitud">Longitud del campo</param>
        /// <param name="eliminaCerosIzq">Indica si se eliminan los ceros a la izquierda</param>
        /// <param name="noUtilizado">Indica si el campo se debe usar o no</param>
        /// <returns>Retorna el valor corregido</returns>
        private string FormatValue(object value, TipoCampo tipo, int longitud, bool eliminaCerosIzq, bool noUtilizado)
        {
            try
            {
                string output;

                //Carga el valor inicial del campo
                if (value == null)
                    output = "";
                else if (value is Nullable && ((INullable)value).IsNull)
                    output = "";
                else
                    output = value.ToString();


                //Revisa si el campo se dbe tener en cuenta
                if (noUtilizado)
                    return output;

                switch (tipo)
                {
                    case TipoCampo.Texto:
                        #region Texto

                        if (this.texto_MinusculasInd)
                            output = output.ToLower();

                        #region Longitud fija
                        if (this.texto_Longitud == Texto_Longitud.Fija)
                        {
                            if (output.Length > longitud)
                                output = output.Substring(0, longitud);
                            else
                            {
                                if (this.texto_Justifica == Texto_Justifica.Derecha)
                                    output = output.PadLeft(longitud, ' ');
                                else
                                    output = output.PadRight(longitud, ' ');
                            }
                        }
                        #endregion

                        #region Encierra

                        if (this.texto_Encierra == Texto_Encierra.Comilla)
                            output = "'" + output + "'";
                        else if (this.texto_Encierra == Texto_Encierra.ComillaDoble)
                            output = "\"" + output + "\"";

                        #endregion

                        #endregion

                        break;
                    case TipoCampo.Numero:
                        #region Numero

                        //int nVal = !string.IsNullOrWhiteSpace(output) ? Convert.ToInt32(output) : 0;
                        string formatNumber = string.Empty;
                        string cerosNum = "0";

                        #region Ceros
                        if (this.numero_CerosInd)
                        {
                            cerosNum = string.Empty;
                            for (int i = 0; i < longitud; ++i)
                                cerosNum += "0";

                            formatNumber = cerosNum;
                        }
                        #endregion

                        #region Longitud Fija

                        if (this.numero_Longitud == Numero_Longitud.Fija && output.Length > longitud)
                            output = errorStr;

                        #endregion

                        if (output != errorStr)
                        {
                            #region Rellena los espacio en longitud fija

                            if (this.numero_Longitud == Numero_Longitud.Fija && output.Length < longitud)
                            {
                                if (this.numero_Justifica == Numero_Justifica.Derecha)
                                {
                                    if (this.numero_CerosInd)
                                    {
                                        output = output.PadLeft(longitud, '0');
                                    }
                                    else
                                    {
                                        output = output.PadLeft(longitud, ' ');
                                    }
                                }
                                else
                                    output = output.PadRight(longitud, ' ');
                            }

                            #endregion

                            #region Encierra

                            if (this.numero_Encierra == Numero_Encierra.Comilla)
                                output = "'" + output + "'";
                            else if (this.numero_Encierra == Numero_Encierra.ComillaDoble)
                                output = "\"" + output + "\"";

                            #endregion
                        }
                        #endregion

                        break;
                    case TipoCampo.Valor:
                        #region Valor

                        decimal vVal = !string.IsNullOrWhiteSpace(output) ? Convert.ToDecimal(output) : 0;
                        int valInt = Convert.ToInt32(Math.Truncate(vVal));
                        string formatValue = string.Empty;
                        string cerosVal = "0";
                        string decimalesVal = string.Empty;

                        #region Ceros

                        if (this.valor_CerosInd)
                        {
                            cerosVal = string.Empty;
                            for (int i = 0; i < longitud; ++i)
                                cerosVal += "0";

                            formatValue = cerosVal;
                        }

                        #endregion

                        #region Decimales

                        if (this.valor_Decimales > 0)
                        {

                            decimalesVal = ".";
                            for (int i = 0; i < valor_Decimales; ++i)
                                decimalesVal += "0";

                            vVal = valInt + Math.Round(vVal - Convert.ToDecimal(valInt), this.valor_Decimales);
                        }

                        #endregion

                        #region Longitud Fija

                        if (this.valor_Longitud == Valor_Longitud.Fija && vVal.ToString().Length > longitud)
                            output = errorStr;

                        #endregion

                        if (output != errorStr)
                        {
                            #region Separador de miles y decimales

                            formatValue = cerosVal + ",0" + decimalesVal;
                            if (this.valor_SeparaMiles == Valor_SeparaMiles.Puntos)
                            {
                                CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
                                output = vVal.ToString(formatValue, elGR);
                            }
                            else if (this.valor_SeparaMiles == Valor_SeparaMiles.Coma)
                                output = vVal.ToString(formatValue, CultureInfo.InvariantCulture);
                            else
                            {
                                formatValue = cerosVal + decimalesVal;

                                if (this.valor_SeparaDecimales == Valor_SeparaDecimales.Puntos)
                                    output = vVal.ToString(formatValue, CultureInfo.InvariantCulture);
                                else
                                {
                                    CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
                                    output = vVal.ToString(formatValue, elGR);
                                }
                            }

                            #endregion

                            #region Rellena los espacion en longitud fija

                            if (this.valor_Longitud == Valor_Longitud.Fija)
                            {
                                if (output.Length < longitud)
                                {
                                    if (this.valor_Justifica == Valor_Justifica.Derecha)
                                        output = output.PadLeft(longitud, ' ');
                                    else
                                        output = output.PadRight(longitud, ' ');
                                }
                            }

                            #endregion

                            #region Caracter Especial

                            if (!string.IsNullOrWhiteSpace(this.valor_CaracterEspecial))
                            {
                                output = this.valor_CaracterEspecial + output;
                            }

                            #endregion

                            #region Encierra

                            if (this.valor_Encierra == Valor_Encierra.Comilla)
                                output = "'" + output + "'";
                            else if (valor_Encierra == Valor_Encierra.ComillaDoble)
                                output = "\"" + output + "\"";

                            #endregion
                        }

                        #endregion

                        break;
                    case TipoCampo.Fecha:
                        #region Fecha

                        if (!string.IsNullOrWhiteSpace(output))
                        {
                            DateTime fVal = Convert.ToDateTime(output);

                            string dateFormat = string.Empty;
                            string separador = string.Empty;
                            string dayFormat = "dd";
                            string monthFormat = string.Empty;
                            string yearFormat = string.Empty;

                            CultureInfo cu = CultureInfo.InvariantCulture;

                            #region Separador

                            if (this.fecha_Separador == Fecha_Separador.Guion)
                                separador = "-";
                            else if (this.fecha_Separador == Fecha_Separador.Slash)
                                separador = "/";

                            #endregion

                            #region Mes

                            monthFormat = "MM";

                            if (this.fecha_Mes == Fecha_Mes.Español)
                            {
                                cu = CultureInfo.CreateSpecificCulture("es-CO");
                                monthFormat = "MMMM";
                            }
                            else if (this.fecha_Mes == Fecha_Mes.Esp3Letras)
                            {
                                cu = CultureInfo.CreateSpecificCulture("es-CO");
                                monthFormat = "MMM";
                            }
                            else if (this.fecha_Mes == Fecha_Mes.Ingles)
                            {
                                monthFormat = "MMMM";
                            }
                            else if (this.fecha_Mes == Fecha_Mes.Ing3Letras)
                            {
                                monthFormat = "MMM";
                            }

                            #endregion

                            #region Año

                            if (this.fecha_Año == Fecha_Año.AA)
                                yearFormat = "yy";
                            else
                                yearFormat = "yyyy";

                            #endregion

                            #region Tipo

                            if (this.fecha_Tipo == Fecha_Tipo.AñoDiaMes)
                                dateFormat = yearFormat + separador + dayFormat + separador + monthFormat;
                            else if (this.fecha_Tipo == Fecha_Tipo.AñoMesDia)
                                dateFormat = yearFormat + separador + monthFormat + separador + dayFormat;
                            else if (this.fecha_Tipo == Fecha_Tipo.DiaMesAño)
                                dateFormat = dayFormat + separador + monthFormat + separador + yearFormat;
                            else if (this.fecha_Tipo == Fecha_Tipo.MesDiaAño)
                                dateFormat = monthFormat + separador + dayFormat + separador + yearFormat;

                            #endregion

                            output = fVal.ToString(dateFormat, cu);
                        }

                        #region Encierra

                        if (this.fecha_Encierra == Fecha_Encierra.Comilla)
                            output = "'" + output + "'";
                        else if (fecha_Encierra == Fecha_Encierra.ComillaDoble)
                            output = "\"" + output + "\"";

                        #endregion

                        #endregion

                        break;
                }

                //General
                if (output.Contains(",") || output.Contains("\""))
                    output = '"' + output.Replace("\"", "\"\"") + '"';

                return output;
            }
            catch (Exception ex)
            {
                this.errorStr = "Error en formato de columnas: " + ex.Message;
                return this.errorStr;
            }
        }

        /// <summary>
        /// Quita formato a un campo de acuerdo con la infromación de la estructura
        /// </summary>
        /// <param name="value">Valor del campo</param>
        /// <param name="tipo">Tipo del campo</param>
        /// <param name="longitud">Longitud del campo</param>
        /// <param name="eliminaCerosIzq">Indica si se eliminan los ceros a la izquierda</param>
        /// <param name="noUtilizado">Indica si el campo se debe usar o no</param>
        /// <returns>Retorna el valor corregido</returns>
        private string UnFormatValue(object value, TipoCampo tipo, int longitud, bool eliminaCerosIzq, bool noUtilizado)
        {
            try
            {
                //Revisa si el campo se dbe tener en cuenta
                if (noUtilizado)
                    return string.Empty;

                string output;

                #region Carga la información inicial

                int firstOccurenceEncierra = 0;
                int lastOccurenceEncierra = 0;
                
                if (value == null)
                    output = "";
                else if (value is Nullable && ((INullable)value).IsNull)
                    output = "";
                else
                    output = value.ToString();

                //Elimina los ceros de la izquierda
                if (eliminaCerosIzq)
                    output = output.TrimStart('0');

                if (string.IsNullOrWhiteSpace(output))
                    return output;

                #endregion

                switch (tipo)
                {
                    case TipoCampo.Texto:
                        #region Texto

                        #region Encierra

                        if (this.texto_Encierra == Texto_Encierra.Comilla)
                        {
                            firstOccurenceEncierra = output.IndexOf("'");
                            output = output.Remove(firstOccurenceEncierra, 1);

                            lastOccurenceEncierra = output.LastIndexOf("'");
                            output = output.Remove(lastOccurenceEncierra);
                        }
                        else if (this.texto_Encierra == Texto_Encierra.ComillaDoble)
                        {
                            firstOccurenceEncierra = output.IndexOf("\"");
                            output = output.Remove(firstOccurenceEncierra, 1);

                            lastOccurenceEncierra = output.LastIndexOf("\"");
                            output = output.Remove(lastOccurenceEncierra);
                        }

                        #endregion

                        #region Longitud fija

                        if (this.texto_Longitud == Texto_Longitud.Fija)
                        {
                            if (output.Length > longitud)
                            {
                                if (this.texto_Justifica == Texto_Justifica.Izquierda)
                                    output = output.Substring(0, longitud);
                                else
                                    output = output.Substring(output.Length - longitud -1, longitud);
                            }
                        }
                        else
                        {
                            output = output.Trim();
                        }

                        #endregion

                        #endregion

                        break;
                    case TipoCampo.Numero:
                        #region Numero

                        #region Encierra

                        if (this.numero_Encierra == Numero_Encierra.Comilla)
                        {
                            firstOccurenceEncierra = output.IndexOf("'");
                            output = output.Remove(firstOccurenceEncierra, 1);

                            lastOccurenceEncierra = output.LastIndexOf("'");
                            output = output.Remove(lastOccurenceEncierra);
                        }
                        else if (this.numero_Encierra == Numero_Encierra.ComillaDoble)
                        {
                            firstOccurenceEncierra = output.IndexOf("\"");
                            output = output.Remove(firstOccurenceEncierra, 1);

                            lastOccurenceEncierra = output.LastIndexOf("\"");
                            output = output.Remove(lastOccurenceEncierra);
                        }

                        #endregion

                        #region Longitud Fija

                        if (this.numero_Longitud == Numero_Longitud.Fija)
                        {
                            if (output.Length > longitud)
                            {
                                if (this.numero_Justifica == Numero_Justifica.Izquierda)
                                    output = output.Substring(0, longitud);
                                else
                                    output = output.Substring(output.Length - longitud - 1, longitud);
                            }
                        }
                        else
                        {
                            output = output.Trim();
                        }

                        #endregion

                        int n = Convert.ToInt32(output, CultureInfo.InvariantCulture);
                        output = n.ToString("0", CultureInfo.InvariantCulture);

                        #endregion

                        break;
                    case TipoCampo.Valor:
                        #region Valor

                        #region Encierra

                        if (this.valor_Encierra == Valor_Encierra.Comilla)
                        {
                            firstOccurenceEncierra = output.IndexOf("'");
                            output = output.Remove(firstOccurenceEncierra, 1);

                            lastOccurenceEncierra = output.LastIndexOf("'");
                            output = output.Remove(lastOccurenceEncierra);
                        }
                        else if (this.valor_Encierra == Valor_Encierra.ComillaDoble)
                        {
                            firstOccurenceEncierra = output.IndexOf("\"");
                            output = output.Remove(firstOccurenceEncierra, 1);

                            lastOccurenceEncierra = output.LastIndexOf("\"");
                            output = output.Remove(lastOccurenceEncierra);
                        }

                        #endregion

                        #region Caracter Especial

                        if (!string.IsNullOrWhiteSpace(this.valor_CaracterEspecial))
                            output = output.Remove(0, this.valor_CaracterEspecial.Length);

                        #endregion

                        #region Longitud Fija

                        if (this.valor_Longitud == Valor_Longitud.Fija)
                        {
                            if (output.Length > longitud)
                            {
                                if (this.valor_Justifica == Valor_Justifica.Izquierda)
                                    output = output.Substring(0, longitud);
                                else
                                    output = output.Substring(output.Length - longitud - 1, longitud);
                            }
                        }
                        else
                        {
                            output = output.Trim();
                        }

                        #endregion

                        #region Separador de miles y decimales

                        //Miles
                        if (this.valor_SeparaMiles == Valor_SeparaMiles.Puntos)
                        {
                            output = output.Replace(".", string.Empty);
                        }
                        else if (this.valor_SeparaMiles == Valor_SeparaMiles.Coma)
                        {
                            output = output.Replace(",", string.Empty);
                        }

                        //Decimales
                        if (this.valor_SeparaDecimales == Valor_SeparaDecimales.Coma)
                            output = output.Replace(",", ".");

                        #endregion

                        decimal val = Convert.ToDecimal(output, CultureInfo.InvariantCulture);
                        decimal decimalPlaces = val - Math.Truncate(val);

                        output = val.ToString("0", CultureInfo.InvariantCulture);
                        if (this.valor_Decimales > 0)
                        {
                            string decimalStr = decimalPlaces.ToString();
                            output += "." + decimalStr.Substring(2, this.valor_Decimales);
                        }

                        #endregion

                        break;
                    case TipoCampo.Fecha:
                        #region Fecha

                        string dia = string.Empty;
                        string mes = string.Empty;
                        string año = string.Empty;

                        #region Encierra

                        if (this.fecha_Encierra == Fecha_Encierra.Comilla)
                        {
                            firstOccurenceEncierra = output.IndexOf("'");
                            output = output.Remove(firstOccurenceEncierra, 1);

                            lastOccurenceEncierra = output.LastIndexOf("'");
                            output = output.Remove(lastOccurenceEncierra);
                        }
                        else if (this.fecha_Encierra == Fecha_Encierra.ComillaDoble)
                        {
                            firstOccurenceEncierra = output.IndexOf("\"");
                            output = output.Remove(firstOccurenceEncierra, 1);

                            lastOccurenceEncierra = output.LastIndexOf("\"");
                            output = output.Remove(lastOccurenceEncierra);
                        }

                        #endregion

                        #region Separador

                        if (this.fecha_Separador == Fecha_Separador.Guion)
                            output = output.Replace("-", string.Empty);
                        else if (this.fecha_Separador == Fecha_Separador.Slash)
                            output = output.Replace("/", string.Empty);

                        #endregion

                        #region Tipo

                        if (this.fecha_Tipo == Fecha_Tipo.AñoDiaMes)
                        {
                            #region Año

                            if (this.fecha_Año == Fecha_Año.AA)
                            {
                                int añoTemp = Convert.ToInt32(output.Substring(0, 2));
                                año = añoTemp <= 70 ? "20" + añoTemp.ToString() : "19" + añoTemp.ToString();

                                output = output.Remove(0, 2);
                            }
                            else
                            {
                                año = output.Substring(0, 4);
                                output = output.Remove(0, 4);                                
                            }

                            #endregion
                            #region Dia

                            dia = output.Substring(0, 2);
                            output = output.Remove(0, 2);

                            #endregion
                            #region Mes
                            mes = this.GetMonth(output);
                            #endregion
                        }
                        else if (this.fecha_Tipo == Fecha_Tipo.AñoMesDia)
                        {
                            #region Año

                            if (this.fecha_Año == Fecha_Año.AA)
                            {
                                int añoTemp = Convert.ToInt32(output.Substring(0, 2));
                                año = añoTemp <= 70 ? "20" + añoTemp.ToString() : "19" + añoTemp.ToString();

                                output = output.Remove(0, 2);
                            }
                            else
                            {
                                año = output.Substring(0, 4);
                                output = output.Remove(0, 4);
                            }

                            #endregion
                            #region Dia

                            dia = output.Substring(output.Length - 2);
                            output = output.Remove(output.Length - 2);

                            #endregion
                            #region Mes
                            mes = this.GetMonth(output);
                            #endregion
                        }
                        else if (this.fecha_Tipo == Fecha_Tipo.DiaMesAño)
                        {
                            #region Dia

                            dia = output.Substring(0, 2);
                            output = output.Remove(0, 2);

                            #endregion
                            #region Año

                            if (this.fecha_Año == Fecha_Año.AA)
                            {
                                int añoTemp = Convert.ToInt32(output.Substring(output.Length - 2));
                                año = añoTemp <= 70 ? "20" + añoTemp.ToString() : "19" + añoTemp.ToString();

                                output = output.Remove(output.Length - 2);
                            }
                            else
                            {
                                año = output.Substring(output.Length - 4);
                                output = output.Remove(output.Length - 4);
                            }

                            #endregion
                            #region Mes
                            mes = this.GetMonth(output);
                            #endregion
                        }
                        else if (this.fecha_Tipo == Fecha_Tipo.MesDiaAño)
                        {
                            #region Año

                            if (this.fecha_Año == Fecha_Año.AA)
                            {
                                int añoTemp = Convert.ToInt32(output.Substring(output.Length - 2));
                                año = añoTemp <= 70 ? "20" + añoTemp.ToString() : "19" + añoTemp.ToString();

                                output = output.Remove(output.Length - 2);
                            }
                            else
                            {
                                año = output.Substring(output.Length - 4);
                                output = output.Remove(output.Length - 4);
                            }

                            #endregion
                            #region Dia

                            dia = output.Substring(output.Length - 2);
                            output = output.Remove(output.Length - 2);

                            #endregion
                            #region Mes
                            mes = this.GetMonth(output);
                            #endregion
                        }

                        #endregion

                        DateTime dt = new DateTime(Convert.ToInt32(año), Convert.ToInt32(mes), Convert.ToInt32(dia));
                        output = dt.ToString(FormatString.Date);

                        #endregion

                        break;
                }

                //Elimina los ceros de la izquierda
                if (eliminaCerosIzq)
                    output = output.TrimStart('0');

                //General
                if (output.Contains(",") || output.Contains("\""))
                    output = '"' + output.Replace("\"", "\"\"") + '"';

                return output;
            }
            catch (Exception ex)
            {
                return errorStr;
            }
        }

        /// <summary>
        /// Trae un mes en número
        /// </summary>
        /// <param name="val">Valor para saber el mes</param>
        /// <returns>Retorna el mes deseado</returns>
        private string GetMonth(string val)
        {
            string mes = val;

            if (this.fecha_Mes == Fecha_Mes.MM)
                return val;

            if (this.fecha_Mes == Fecha_Mes.Español || this.fecha_Mes == Fecha_Mes.Esp3Letras)
            {
                #region Mes dado en español
                switch (val.Substring(0, 3).ToLower())
                {
                    case "ene":
                        mes = "01";
                        break;
                    case "feb":
                        mes = "02";
                        break;
                    case "mar":
                        mes = "03";
                        break;
                    case "abr":
                        mes = "04";
                        break;
                    case "may":
                        mes = "05";
                        break;
                    case "jun":
                        mes = "06";
                        break;
                    case "jul":
                        mes = "07";
                        break;
                    case "ago":
                        mes = "08";
                        break;
                    case "sep":
                        mes = "09";
                        break;
                    case "oct":
                        mes = "10";
                        break;
                    case "nov":
                        mes = "11";
                        break;
                    case "dic":
                        mes = "12";
                        break;
                }
                #endregion
            }
            else
            {
                #region Mes dado en ingles
                switch (val.Substring(0, 3).ToLower())
                {
                    case "jan":
                        mes = "01";
                        break;
                    case "feb":
                        mes = "02";
                        break;
                    case "mar":
                        mes = "03";
                        break;
                    case "apr":
                        mes = "04";
                        break;
                    case "may":
                        mes = "05";
                        break;
                    case "jun":
                        mes = "06";
                        break;
                    case "jul":
                        mes = "07";
                        break;
                    case "aug":
                        mes = "08";
                        break;
                    case "sep":
                        mes = "09";
                        break;
                    case "oct":
                        mes = "10";
                        break;
                    case "nov":
                        mes = "11";
                        break;
                    case "dec":
                        mes = "12";
                        break;
                }
                #endregion
            }

            return mes;
        }

        /// <summary>
        /// Limpia el valor de un campo
        /// </summary>
        /// <param name="value">Valor del campo</param>
        /// <param name="fieldType">Tipo del campo</param>
        /// <returns>Retorna el valor corregido</returns>
        private string SetFriendlyValue(object value, Type fieldType)
        {
            try
            {
                if (value == null)
                    return "";
                if (value is Nullable && ((INullable)value).IsNull)
                    return "";

                string newValue = value.ToString();

                #region Formatos especiales (Corregido)

                if (!string.IsNullOrWhiteSpace(newValue))
                {
                    //Bool
                    if (fieldType == typeof(bool) || fieldType == typeof(Nullable<bool>))
                    {
                        bool val = Convert.ToBoolean(value);
                        newValue = val ? "X" : string.Empty;
                    }

                    //Fecha
                    if (fieldType == typeof(DateTime) || fieldType == typeof(Nullable<DateTime>))
                    {
                        DateTime val = Convert.ToDateTime(value);
                        newValue = val.ToString(FormatString.Date);
                    }

                    //Decimal
                    if (fieldType == typeof(decimal) || fieldType == typeof(Nullable<decimal>))
                    {
                        decimal val = Convert.ToDecimal(value);
                        decimal decimalPlaces = val - Math.Truncate(val);

                        if (decimalPlaces == 0)
                            newValue = val.ToString("0", CultureInfo.InvariantCulture);
                        else
                            newValue = val.ToString("n4", CultureInfo.InvariantCulture);
                    }

                    //Double 
                    if (fieldType == typeof(double) || fieldType == typeof(Nullable<double>))
                    {
                        double val = Convert.ToDouble(value);
                        double decimalPlaces = val - Math.Truncate(val);

                        if (decimalPlaces == 0)
                            newValue = val.ToString("0", CultureInfo.InvariantCulture);
                        else
                            newValue = val.ToString("n4", CultureInfo.InvariantCulture);
                    }
                }

                #endregion

                if (newValue.Contains("\""))
                    newValue = '"' + newValue.Replace("\"", "\"\"") + '"';


                return newValue;
            }
            catch (Exception ex)
            {
                return errorStr;
            }
        }

        /// <summary>
        /// Exporta una fuente de datos 
        /// </summary>
        /// <param name="t">Fuente de datos a exportar</param>
        /// <returns>Retorna una cadena de texto con el formato de exportación</returns>
        private string Export_FromBasicTable(DataTable t, IEnumerable<string> fields, bool includeHeader)
        {
            try
            {
                this.tableDetails = t;
                if (this.tableDetails.Rows.Count == 0 || fields.Count() == 0)
                    return string.Empty;

                #region Variables

                object obj = null;
                string val = string.Empty;
                StringBuilder sb = new StringBuilder();

                #endregion

                #region Carga los titulos
                if (includeHeader)
                {
                    string line = string.Empty;
                    foreach (string campo in fields)
                        line += campo + this.separadorCampo;
                    sb.Append(line);
                }
                #endregion

                #region Carga los detalles

                for (int i = 0; i < this.tableDetails.Rows.Count; ++i)
                {
                    string line = string.Empty;
                    DataRow fila = this.tableDetails.Rows[i];
                    foreach (string campo in fields)
                    {
                        obj = this.tableDetails.Rows[0][campo];
                        if (obj != null)
                            line += obj.ToString() + this.separadorCampo;
                    }
                    sb.Append(line);

                    if (i != this.tableDetails.Rows.Count - 2)
                        sb.Remove(sb.Length - 2, 1).AppendLine();
                    else
                        sb.Remove(sb.Length - 2, 1);
                }

                #endregion

                return sb.ToString();
            }
            catch (Exception)
            {
                return errorStr;
            }
        }

        /// <summary>
        /// Exporta una fuente de datos 
        /// </summary>
        /// '<param name="t">Fuente de datos a exportar</param>
        /// <returns>Retorna una cadena de texto con el formato de exportación</returns>
        private string Export_FromStructuredTable(DataTable t, IEnumerable<object> fields, bool includeHeader)
        {
            try
            {
                this.tableDetails = t;
                if (this.tableDetails.Rows.Count == 0 || fields.Count() == 0)
                    return string.Empty;

                #region Variables

                PropertyInfo pi = null;

                //Campos
                Type fType;
                string tipoCampoStr = string.Empty;
                TipoRegistro tipoRegistro;
                string titulo;
                TipoCampo tipoCampo;
                string campoID;
                int orden;
                int longitud;
                bool eliminaCerosIzq;
                bool noUtilizado;
                Tuple<string, TipoCampo, int, bool, bool> infoDetalle;

                //Info registro
                object obj = null;
                string val = string.Empty;
                Dictionary<string, int> detallesTemp_Orden = new Dictionary<string, int>();
                Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>> detallesTemp_TipoCampo = new Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>>();
                
                //Tupla: Titulo, TipoCampo, longitud, eliminaCerosIzq, noUtilizado
                this.cols_Detalle = new Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>>();
                
                StringBuilder sb_Inicial = new StringBuilder();
                StringBuilder sb_Detalle = new StringBuilder();
                StringBuilder sb_Final = new StringBuilder();
                StringBuilder sb_Result = new StringBuilder();

                #endregion

                #region Carga los titulos
                if (includeHeader)
                {
                    string line = string.Empty;
                    foreach (object campoObj in fields)
                    {
                        fType = campoObj.GetType(); 
                        pi = fType.GetProperty("Titulo");
                        
                        string campo = pi.GetValue(campoObj, null).ToString();
                        sb_Result.Append(campo).Append(this.separadorCampo);
                        //line += campo + this.separadorCampo;
                    }

                    //sb_Result.Append(line);
                }
                #endregion

                #region Carga la información inicial y final
                foreach (object campoObj in fields)
                {
                    fType = campoObj.GetType();

                    //Titulo
                    pi = fType.GetProperty("Titulo");
                    titulo = pi.GetValue(campoObj, null).ToString().Trim();

                    //TipoRegistro
                    pi = fType.GetProperty("TipoRegistro");
                    tipoCampoStr = pi.GetValue(campoObj, null).ToString();
                    tipoRegistro = (TipoRegistro)Enum.Parse(typeof(TipoRegistro), tipoCampoStr);

                    //TipoCampo
                    pi = fType.GetProperty("TipoCpo");
                    tipoCampoStr = pi.GetValue(campoObj, null).ToString();
                    tipoCampo = (TipoCampo)Enum.Parse(typeof(TipoCampo), tipoCampoStr);

                    //Código campo
                    pi = fType.GetProperty("CodigoCpo");
                    campoID = pi.GetValue(campoObj, null).ToString().Trim();

                    //Longitud del campo
                    pi = fType.GetProperty("LongitudCpo");
                    longitud = Convert.ToInt32(pi.GetValue(campoObj, null).ToString());

                    //Elimina Ceros a la izquierda
                    pi = fType.GetProperty("EliminaCeroIzqInd");
                    eliminaCerosIzq = Convert.ToBoolean(pi.GetValue(campoObj, null).ToString().Trim());

                    //Campo no utilizado
                    pi = fType.GetProperty("NoUtilizadoInd");
                    noUtilizado = Convert.ToBoolean(pi.GetValue(campoObj, null).ToString().Trim());

                    if (tipoRegistro == TipoRegistro.Inicial)
                    {
                        #region Campos Iniciales

                        if (this.tableDetails.Columns[campoID] != null)
                        {
                            obj = this.tableDetails.Rows[0][campoID];
                            if (obj != null)
                            {
                                val = this.FormatValue(obj, tipoCampo, longitud, eliminaCerosIzq, noUtilizado);

                                sb_Inicial.Append(val);
                                sb_Inicial.Append(this.separadorCampo);
                            }
                        }

                        #endregion
                    }
                    else if (tipoRegistro == TipoRegistro.Final)
                    {
                        #region Campos Finales

                        if (this.tableDetails.Columns[campoID] != null)
                        {
                            obj = this.tableDetails.Rows[0][campoID];
                            if (obj != null)
                            {
                                val = this.FormatValue(obj, tipoCampo, longitud, eliminaCerosIzq, noUtilizado);

                                sb_Final.Append(val);
                                sb_Final.Append(this.separadorCampo);
                            }
                        }
                        #endregion
                    }
                    else if (tipoRegistro == TipoRegistro.Detalle)
                    {
                        #region Orden del detalle

                        if (this.tableDetails.Columns[campoID] != null)
                        {
                            //Orden Campo
                            pi = fType.GetProperty("DetalleNumero");
                            orden = Convert.ToInt32(pi.GetValue(campoObj, null).ToString());

                            infoDetalle = new Tuple<string, TipoCampo, int, bool, bool>(titulo, tipoCampo, longitud, eliminaCerosIzq, noUtilizado);
                            detallesTemp_Orden[campoID] = orden;
                            detallesTemp_TipoCampo[campoID] = infoDetalle;
                        }
                        #endregion
                    }
                }

                //Ordena los detalles
                var items = from pair in detallesTemp_Orden orderby pair.Value ascending select pair;
                foreach (KeyValuePair<string, int> pair in items)
                {
                    //Tupla (infoDetalle): Titulo, TipoCampo, longitud, eliminaCerosIzq, noUtilizado
                    infoDetalle = detallesTemp_TipoCampo[pair.Key];
                    this.cols_Detalle.Add(pair.Key, infoDetalle);
                }

                #endregion

                #region Carga los detalles

                for (int i = 0; i < this.tableDetails.Rows.Count; ++i)
                {
                    string line = string.Empty;
                    DataRow fila = this.tableDetails.Rows[i];
                    foreach (string campo in this.cols_Detalle.Keys)
                    {
                        obj = this.tableDetails.Rows[i][campo];
                        if (obj != null)
                        {
                            //Tupla (infoDetalle): Titulo, TipoCampo, longitud, eliminaCerosIzq, noUtilizado
                            infoDetalle = detallesTemp_TipoCampo[campo];
                            val = this.FormatValue(obj, infoDetalle.Item2, infoDetalle.Item3, infoDetalle.Item4, infoDetalle.Item5);

                            if (val == errorStr)
                            {
                                line = val + this.separadorCampo;
                                break;
                            }
                            else
                                line += val + this.separadorCampo;
                        }
                    }

                    sb_Detalle.Append(line);

                    if (i != this.tableDetails.Rows.Count - 1)
                        sb_Detalle.AppendLine();
                    //else
                    //    sb_Detalle.Remove(sb_Detalle.Length - 2, 1);
                }

                #endregion

                sb_Result.Append(sb_Inicial);
                if (sb_Result.Length > 0)
                    sb_Result.Remove(sb_Result.Length - 2, 1).AppendLine();

                sb_Result.Append(sb_Detalle);
                if (sb_Result.Length > 0)
                    sb_Result.AppendLine();

                sb_Result.Append(sb_Final);
                if (sb_Result.Length > 0)
                    sb_Result.Remove(sb_Result.Length - 2, 1);

                return sb_Result.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Funciones públicas

        /// <summary>
        /// Carga un Datatable desde cualquier fuente de datos
        /// </summary>
        /// <param name="objType">Tipo de objeto base</param>
        /// <param name="list">Lista de registros</param>
        /// <returns>Retorna el datatable</returns>
        public DataTable Convert_GenericListToDataTable(Type objType, IEnumerable<object> list)
        {
            try
            {
                DataTable table = new DataTable();
                PropertyInfo[] props = objType.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);

                foreach (PropertyInfo propertyInfo in props)
                    table.Columns.Add(propertyInfo.Name);

                string[] values = new string[props.Length];
                foreach (object obj in list)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        PropertyInfo pi = objType.GetProperty(props[i].Name, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (pi != null)
                        {
                            #region Propiedades

                            if (pi.PropertyType.IsEnum)
                            {
                                object o = pi.GetValue(obj, null);
                                values[i] = ((int)o).ToString();
                            }
                            else if (!typeof(IEnumerable).IsAssignableFrom(pi.PropertyType) && pi.PropertyType.GetProperty("Value") != null)
                            {
                                //Propiedades con propiedad interna Value
                                object auxObj = pi.GetValue(obj, null) != null? pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(obj, null), null) : null;
                                if (auxObj != null)
                                    values[i] = this.SetFriendlyValue(auxObj, auxObj.GetType());
                                else
                                    values[i] = this.SetFriendlyValue(pi.GetValue(obj, null), pi.PropertyType);
                            }
                            else
                            {
                                values[i] = this.SetFriendlyValue(pi.GetValue(obj, null), pi.PropertyType);
                            }

                            #endregion
                        }
                        else
                        {
                            #region Campos
                            FieldInfo fi = obj.GetType().GetField(props[i].Name, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (fi != null)
                            {
                                if (fi.FieldType.IsEnum)
                                {
                                    object o = fi.GetValue(obj);
                                    values[i] = ((int)o).ToString();
                                }
                                else if (!typeof(IEnumerable).IsAssignableFrom(fi.FieldType) && fi.FieldType.GetProperty("Value") != null)
                                {
                                    //Propiedades con propiedad interna Value
                                    object auxObj = fi.FieldType.GetProperty("Value").GetValue(pi.GetValue(obj, null), null);
                                    values[i] = this.SetFriendlyValue(auxObj, auxObj.GetType());
                                }
                                else
                                {
                                    values[i] = this.SetFriendlyValue(fi.GetValue(obj), pi.PropertyType);
                                }
                            }
                            #endregion
                        }

                    }
                    table.Rows.Add(values);
                }

                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Loads the data and structure from a file
        /// </summary>
        public void Import_FromTextFile()
        {
            try 
            {
                if (this.cols_Detalle.Count == 0)
                    return;

                OpenFileDialog fDialog = new OpenFileDialog();
                fDialog.Filter = "Texto|*.txt";// "Word|*.xml|Excel|*.uml|Img1|*.uml|Img2|*.uml";

                if (fDialog.ShowDialog() == DialogResult.OK)
                {
                    this.tableDetails.Rows.Clear();

                    string path = Path.GetDirectoryName(fDialog.FileName);
                    string filename = Path.GetFileNameWithoutExtension(fDialog.FileName);
                    string ext = Path.GetExtension(fDialog.FileName);

                    string filePath = path + "\\" + filename + ext;
                    int lineNumber = 0;

                    // Read the file and display it line by line.
                    string lineData;
                    System.IO.StreamReader file = new System.IO.StreamReader(filePath);
                    while ((lineData = file.ReadLine()) != null)
                    {
                        lineNumber++;

                        //Solo toma en cuenta las lineas del archivo que corresponden al detalle
                        if (lineNumber >= this.primerRegDetalleLinea)
                        {
                            int colPos = 0;
                            int colDataIndex = 0;
                            bool filaValida = true;
                            DataRow fila = this.tableDetails.NewRow();
                            List<string> dataFields = new List<string>();
                            if (!string.IsNullOrWhiteSpace(this.separadorCampo))
                            {
                                dataFields = lineData.Split(new string[] { this.separadorCampo }, StringSplitOptions.None).ToList();
                            }

                            //Tupla: Titulo, TipoCampo, longitud, eliminaCerosIzq, noUtilizado
                            foreach (KeyValuePair<string, Tuple<string, TipoCampo, int, bool, bool>> pair in this.cols_Detalle)
                            {
                                string colVal = string.Empty;

                                //Obtiene el valor de la columna, dependiendo si tiene formato o no
                                if (!string.IsNullOrWhiteSpace(this.separadorCampo))
                                {
                                    colVal = dataFields[colPos];
                                    colPos++;
                                }
                                else
                                {
                                    colVal = lineData.Substring(colDataIndex, pair.Value.Item3);
                                    colDataIndex += pair.Value.Item3;
                                }

                                //Quita el formato del campo
                                string formatedVal = this.UnFormatValue(colVal, pair.Value.Item2, pair.Value.Item3, pair.Value.Item4, pair.Value.Item5);

                                //Revisa si el título no corresponde
                                if (!string.IsNullOrWhiteSpace(pair.Value.Item1) && pair.Value.Item1.Trim() != formatedVal.Trim())
                                {
                                    filaValida = false;
                                    break;
                                }

                                fila[pair.Key] = formatedVal;
                            }

                            if (filaValida)
                                fila["ValidRow"] = "1";
                            else
                                fila["ValidRow"] = "0";

                            this.tableDetails.Rows.Add(fila);
                        }
                    }
                    file.Close();
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Exportar un archivo
        /// </summary>
        /// <param name="stringColumns">Indica si las columnas son tipo string. De lo contrario son objetos con propiedad Value</param>
        /// <param name="data">Datateble con los datos</param>
        /// <param name="fields">Lista de campos que se desean exportar</param>
        /// <param name="includeHeader">Include table columns names</param>
        /// <param name="path">Ruta para salvar el archivo</param>
        /// <param name="fType">Tipo de archivo que desea exportar (1: txt / 2: xls / 3: txt y xls)</param>
        public void Export_DataToTxt(bool stringColumns, DataTable data, IEnumerable<object> fields, bool includeHeader, string path)
        {
            try
            {
                #region Carga los datos

                string dataStr = string.Empty;
                if(stringColumns)
                {
                    List<string> fieldsStr = fields.Cast<string>().ToList();
                    dataStr = this.Export_FromBasicTable(data, fieldsStr, includeHeader);
                }
                else
                    dataStr = this.Export_FromStructuredTable(data, fields, includeHeader);

                #endregion

                #region Guarda el archivo

                if (string.IsNullOrWhiteSpace(path))
                {
                    using (SaveFileDialog saveDialog = new SaveFileDialog())
                    {
                        saveDialog.Filter = "Txt|*.txt";
                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            string exportFilePath = saveDialog.FileName;
                            string fileExtenstion = new FileInfo(exportFilePath).Extension;

                            path = saveDialog.FileName;
                            File.WriteAllText(saveDialog.FileName, dataStr, Encoding.UTF8);
                        }
                    }
                }
                else
                {
                    path += this.nombreArchivo;
                    File.WriteAllText(path, dataStr, Encoding.UTF8);
                }
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exporta una fuente de datos 
        /// </summary>
        /// <param name="t">Fuente de datos a exportar</param>
        /// <returns>Retorna una cadena de texto con el formato de exportación</returns>
        public DataTable Export_DataToXls(DataTable t, IEnumerable<object> fields)
        {
            try
            {
                this.tableDetails = t;
                DataTable newTable = new DataTable();

                if (this.tableDetails.Rows.Count == 0 || fields.Count() == 0)
                    return newTable;

                #region Variables

                PropertyInfo pi = null;

                //Campos
                Type fType;
                string tipoCampoStr = string.Empty;
                TipoRegistro tipoRegistro;
                string titulo;
                TipoCampo tipoCampo;
                string campoID;
                int orden;
                int longitud;
                bool eliminaCerosIzq;
                bool noUtilizado;
                Tuple<string, TipoCampo, int, bool, bool> infoDetalle;

                //Info registro
                object obj = null;
                string val = string.Empty;
                Dictionary<string, int> detallesTemp_Orden = new Dictionary<string, int>();
                Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>> detallesTemp_TipoCampo = new Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>>();

                //Tupla: Titulo, TipoCampo, longitud, eliminaCerosIzq, noUtilizado
                this.cols_Detalle = new Dictionary<string, Tuple<string, TipoCampo, int, bool, bool>>();

                #endregion

                #region Carga el formato de los campos

                Dictionary<int, string> orderedColumns = new Dictionary<int, string>();
                foreach (object campoObj in fields)
                {
                    fType = campoObj.GetType();

                    //Titulo
                    pi = fType.GetProperty("Titulo");
                    titulo = pi.GetValue(campoObj, null).ToString().Trim();

                    //TipoRegistro
                    pi = fType.GetProperty("TipoRegistro");
                    tipoCampoStr = pi.GetValue(campoObj, null).ToString();
                    tipoRegistro = (TipoRegistro)Enum.Parse(typeof(TipoRegistro), tipoCampoStr);

                    //TipoCampo
                    pi = fType.GetProperty("TipoCpo");
                    tipoCampoStr = pi.GetValue(campoObj, null).ToString();
                    tipoCampo = (TipoCampo)Enum.Parse(typeof(TipoCampo), tipoCampoStr);

                    //Código campo
                    pi = fType.GetProperty("CodigoCpo");
                    campoID = pi.GetValue(campoObj, null).ToString().Trim();

                    //Longitud del campo
                    pi = fType.GetProperty("LongitudCpo");
                    longitud = Convert.ToInt32(pi.GetValue(campoObj, null).ToString());

                    //Elimina Ceros a la izquierda
                    pi = fType.GetProperty("EliminaCeroIzqInd");
                    eliminaCerosIzq = Convert.ToBoolean(pi.GetValue(campoObj, null).ToString().Trim());

                    //Campo no utilizado
                    pi = fType.GetProperty("NoUtilizadoInd");
                    noUtilizado = Convert.ToBoolean(pi.GetValue(campoObj, null).ToString().Trim());

                    if (tipoRegistro == TipoRegistro.Detalle)
                    {
                        #region Orden del detalle

                        if (this.tableDetails.Columns[campoID] != null)
                        {
                            //Orden Campo
                            pi = fType.GetProperty("DetalleNumero");
                            orden = Convert.ToInt32(pi.GetValue(campoObj, null).ToString());

                            infoDetalle = new Tuple<string, TipoCampo, int, bool, bool>(titulo, tipoCampo, longitud, eliminaCerosIzq, noUtilizado);
                            detallesTemp_Orden[campoID] = orden;
                            detallesTemp_TipoCampo[campoID] = infoDetalle;

                            //Agrega el campo a la nueva tabla
                            orderedColumns.Add(orden, campoID);
                        }
                        #endregion
                    }
                }

                //Ordena los detalles
                var items = from pair in detallesTemp_Orden orderby pair.Value ascending select pair;
                foreach (KeyValuePair<string, int> pair in items)
                {
                    //Tupla (infoDetalle): Titulo, TipoCampo, longitud, eliminaCerosIzq, noUtilizado
                    infoDetalle = detallesTemp_TipoCampo[pair.Key];
                    this.cols_Detalle.Add(pair.Key, infoDetalle);

                    string columnName = orderedColumns[pair.Value];
                    newTable.Columns.Add(new DataColumn(columnName));
                }

                #endregion

                #region Carga los detalles

                for (int i = 0; i < this.tableDetails.Rows.Count; ++i)
                {
                    DataRow fila = newTable.NewRow();
                    foreach (string campo in this.cols_Detalle.Keys)
                    {
                        obj = this.tableDetails.Rows[i][campo];
                        if (obj != null)
                        {
                            //Tupla (infoDetalle): Titulo, TipoCampo, longitud, eliminaCerosIzq, noUtilizado
                            infoDetalle = detallesTemp_TipoCampo[campo];
                            val = this.FormatValue(obj, infoDetalle.Item2, infoDetalle.Item3, infoDetalle.Item4, infoDetalle.Item5);

                            fila[campo] = val;
                        }
                    }

                    newTable.Rows.Add(fila);
                }

                #endregion

                return newTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
