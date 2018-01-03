using System;
using System.Data.SqlTypes;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;
using System.Collections;

namespace NewAge.Librerias.Project
{

    /// <summary>
    /// Tipos de formato para exportación
    /// </summary>
    public enum ExportType
    {
        Csv,
        Txt
    }

    public class CsvExport<T> where T : class
    {
        #region Variables

        private Type HierarchyType { get; set; }

        /// <summary>
        /// Tipo de exportación
        /// </summary>
        private ExportType ExpType { get; set; }

        /// <summary>
        /// Lista de objetos a exportar
        /// </summary>
        private List<T> Objects { get; set; }

        /// <summary>
        /// Caracter que separa las columnas
        /// </summary>
        public string Separator { get; set; }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor con lista de campos
        /// </summary>
        /// <param name="objects"></param>
        public CsvExport(List<T> objects)
        {
            this.Objects = objects;
        }

        /// <summary>
        /// Constructor con lista de campos
        /// </summary>
        /// <param name="objects"></param>
        public CsvExport(List<T> objects, Type hType)
        {
            this.HierarchyType = hType;
            this.Objects = objects;
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// Exporta con los nombres de las columnas
        /// Esta hecho para funcionar con DTOs
        /// </summary>
        /// <param name="includeHeaderLine"></param>
        /// <returns></returns>
        private string Export(bool includeHeaderLine, string headerRsx, List<string> excludeColumns = null)
        {
            if (this.ExpType == ExportType.Csv || string.IsNullOrWhiteSpace(this.Separator))
                this.Separator = ",";

            StringBuilder sb = new StringBuilder();

            //Get properties using reflection.
            IList<PropertyInfo> propertyInfos = typeof(T).GetProperties();

            if (includeHeaderLine)
            {
                //add header line.
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if (excludeColumns == null || !excludeColumns.Contains(propertyInfo.Name))
                        sb.Append(propertyInfo.Name).Append(this.Separator);
                }
                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            if (!string.IsNullOrWhiteSpace(headerRsx))
            {
                sb.Append(headerRsx);
                sb.AppendLine();
            }

            //add value for each property.
            foreach (T obj in Objects)
            {
                foreach (PropertyInfo pi in propertyInfos)
                {
                    if (excludeColumns == null || !excludeColumns.Contains(pi.Name))
                    {
                        try
                        {
                            string valClean = string.Empty;

                            if (pi.PropertyType.IsEnum)
                            {
                                object o = pi.GetValue(obj, null);
                                valClean = ((int)o).ToString();
                            }
                            else if (!typeof(IEnumerable).IsAssignableFrom(pi.PropertyType) && pi.PropertyType.GetProperty("Value") != null)
                            {
                                //Propiedades con propiedad interna Value
                                object auxObj = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(obj, null), null);
                                valClean = auxObj != null ? this.SetFriendlyValue(auxObj, auxObj.GetType()) : string.Empty;
                            }
                            else
                            {
                                valClean = this.SetFriendlyValue(pi.GetValue(obj, null), pi.PropertyType);
                            }

                            sb.Append(valClean).Append(this.Separator);
                        }
                        catch(Exception ex)
                        { 
                        }
                    }
                }
                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Exporta con los nombres de las columnas
        /// </summary>
        /// <param name="headerLine">primera fila</param>
        /// <returns></returns>
        private string ExportMaster(string headerLine, List<string> importableCols)
        {
            this.Separator = ",";
            IList<PropertyInfo> propertyInfos = this.HierarchyType.GetProperties();

            StringBuilder sb = new StringBuilder();
            sb.Append(headerLine);
            sb.AppendLine();

            //add value for each property.
            foreach (T obj in this.Objects)
            {
                foreach (string col in importableCols)
                {
                    try
                    {
                        //propertyInfo.Name
                        PropertyInfo pi = this.HierarchyType.GetProperty(col);
                        object auxObj = pi.GetValue(obj, null);
                        string valClean = this.SetFriendlyValue(pi.GetValue(obj, null), pi.PropertyType);

                        //string val = //this.MakeValueCsvFriendly_Master(pi.GetValue(obj, null));

                        sb.Append(valClean).Append(this.Separator);
                    }
                    catch (Exception)
                    {

                    }
                } 


                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Trae el valor para un campo
        /// </summary>
        /// <param name="value">Valor</param>
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

                        newValue = val.ToString("0", CultureInfo.InvariantCulture);
                        if (decimalPlaces > 0)
                        {
                            string decimalStr = decimalPlaces.ToString();
                            newValue += "." + decimalStr.Substring(2, Convert.ToInt32(decimalPlaces));
                        }
                    }

                    //Double 
                    if (fieldType == typeof(double) || fieldType == typeof(Nullable<double>))
                    {
                        double val = Convert.ToDouble(value);
                        double decimalPlaces = val - Math.Truncate(val);

                        newValue = val.ToString("0", CultureInfo.InvariantCulture);
                        if (decimalPlaces > 0)
                        {
                            string decimalStr = decimalPlaces.ToString();
                            newValue += "." + decimalStr.Substring(2, Convert.ToInt32(decimalPlaces));
                        }
                    }
                }

                #endregion

                if (newValue.Contains("\""))
                    newValue = '"' + newValue.Replace("\"", "\"\"") + '"';


                return newValue;
            }
            catch(Exception)
            {
                return string.Empty;
            }
        }

        #endregion

        #region Funciones públicas

        /// <summary>
        /// Exportar un archivo
        /// </summary>
        /// <param name="path">Ruta y nombre del archivo</param>
        public void ExportToFile(string path, ExportType type, bool includeHeaderLine, string headerRsx, List<string> excludeColumns = null)
        {
            this.ExpType = type;

            string str = this.Export(includeHeaderLine, headerRsx, excludeColumns);
            File.WriteAllText(path, str, Encoding.UTF8);
        }

        /// <summary>
        /// Exportar un archivo
        /// </summary>
        /// <param name="path">Ruta y nombre del archivo</param>
        /// <param name="headerline">Primera fila</param>
        /// <param name="importableCols">Columnas importables</param>
        public void ExportToFile_Master(string path, string headerline, List<string> importableCols)
        {
            this.ExpType = ExportType.Csv;

            string str = this.ExportMaster(headerline, importableCols);
            File.WriteAllText(path, str, Encoding.UTF8);
        }

        /// <summary>
        /// Exporta como arreglo de bytes
        /// </summary>
        /// <returns></returns>
        public byte[] ExportToBytes()
        {
            string str = this.Export(false, string.Empty);
            return Encoding.UTF8.GetBytes(str);
        }

        #endregion
    }

}
