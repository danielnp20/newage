using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.Linq;
using System.Data.SqlClient;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;

namespace SentenceTransformer
{
    /// <summary>
    /// 
    /// </summary>
    public static class Transformer
    {
        #region Funciones de filtrado
    
        public static IEnumerable Filtrado(Enums.SqlProvider sqlProvider, List<ConsultasFields> fields, List<DTO_glConsultaFiltro> filtros, List<DTO_glConsultaSeleccion> selecciones, IEnumerable Data, Type itemsType)
        {
            if (filtros.Count > 0)
            {
                string sentenceSelect = "";
                string sentenceWhere = "";

                selecciones = selecciones.OrderBy(x => x.Idx).ToList();

                //por ahora se recorre selecciones y se arma el select luego los filtros
                for (int i = 0; i < selecciones.Count; i++)
                {
                    sentenceSelect = sentenceSelect + ", " + selecciones[i].CampoFisico;
                }

                filtros = filtros.OrderBy(x => x.Idx).ToList();

                //por ahora se recorre selecciones y se arma el select luego los filtros
                for (int i = 0; i < filtros.Count; i++)
                {
                    string operadorS = filtros[i].OperadorSentencia;
                    string operadorF = filtros[i].OperadorFiltro;

                    string valor = GetValorByType(fields, filtros[i]);
                    string filtro = " " + filtros[i].CampoFisico + ".Value " + filtros[i].OperadorFiltro + " " + valor + " ";

                    if (operadorF.Equals(OperadorFiltro.Contiene))
                        filtro = " " + filtros[i].CampoFisico + ".Value.Contains(" + valor + ") ";

                    if (operadorF.Equals(OperadorFiltro.Comienza))
                        filtro = " " + filtros[i].CampoFisico + ".Value.StartsWith(" + valor + ") ";

                    if (operadorS == "N/A")
                        operadorS = "";

                    sentenceWhere = sentenceWhere + filtro;

                    if (i < (filtros.Count - 1))
                        sentenceWhere += operadorS;
                }

                //var t = Data.AsQueryable().Where(sentenceSelect).Select("new (" + sentenceSelect + ")");
                //IEnumerable<DTO_simplePruebaJerarquica> aux = Data.Cast<DTO_simplePruebaJerarquica>();
                IEnumerable t = Data.AsQueryable().Where(sentenceWhere);

                return t;
            }
            else
            {
                return Data;
            }
        }

        /// <summary>
        /// Retorna la parte del where de una sentencia SQL
        /// </summary>
        /// <param name="filtros">Filtros a aplicar</param>
        /// <param name="objectType">Type del objeto que contiene la definicion de campos</param>
        /// <returns>string con las condiciones</returns>
        public static string WhereSql(List<DTO_glConsultaFiltro> filtros, Type objectType, List<ConsultasFields> consultaFields=null ,Dictionary<string, string> fieldTables=null)
        {
            if (consultaFields == null)
                consultaFields = new List<ConsultasFields>();
            string sentenceWhere = "";
            for (int i = 0; i < filtros.Count(); i++)
            {
                DTO_glConsultaFiltro fil = filtros[i];
                Type t = FieldType(objectType, fil.CampoFisico);
                if ((t == typeof(DateTime) || t == typeof(DateTime?)))
                {
                    DateTime fecha = Convert.ToDateTime(fil.ValorFiltro);
                    fil.ValorFiltro = fecha.ToString(FormatString.DB_Date_YYYY_MM_DD);
                }
                if (fil.ValorFiltro.Equals(true.ToString()))
                    fil.ValorFiltro = "1";
                if (fil.ValorFiltro.Equals(false.ToString()))
                    fil.ValorFiltro = "0";
                string valor = GetSQLValorByType(objectType, fil, consultaFields);

                var operadorS = fil.OperadorSentencia;
                string operadorF = (fil.OperadorFiltro.Equals(OperadorFiltro.Comienza) || fil.OperadorFiltro.Equals(OperadorFiltro.Contiene) || fil.ValorFiltro.Contains(ComodinesFiltro.UnLugar) || fil.ValorFiltro.Contains(ComodinesFiltro.VariosLugares)) ? "LIKE" : fil.OperadorFiltro;
                string campoFisico = fil.CampoFisico;

                if (fieldTables != null && fieldTables.ContainsKey(campoFisico))
                    campoFisico = fieldTables[campoFisico] + "." + campoFisico;

                string filtro = string.Empty;

                if ((t == typeof(DateTime) || t == typeof(DateTime?)))
                    filtro = " CAST(" + campoFisico + " AS Date) " + operadorF + " " + valor + " ";
                //else if (t == typeof(string))
                //    filtro = " " + campoFisico + " " + operadorF + " '" + valor + "' ";
                else
                    filtro = " " + campoFisico + " " + operadorF + " " + valor + " ";

                if (operadorS == "N/A")
                    operadorS = "";

                sentenceWhere = sentenceWhere + filtro;

                if (i < (filtros.Count - 1))
                    sentenceWhere += operadorS;
            }
            return sentenceWhere;
        }

        /// <summary>
        /// Retorna una lista de filtros en string para aplicar sobre una grilla de DeveExpress
        /// </summary>
        /// <param name="filtros"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static string FiltrosGrilla(List<DTO_glConsultaFiltro> filtros,List<ConsultasFields> fields, Type objectType)
        {
            string res = string.Empty;
            for (int i = 0; i < filtros.Count; i++)
            {
                string valor = GetGridFilterValorByType(fields, filtros[i]);

                string operadorS = filtros[i].OperadorSentencia;
                string operadorF = filtros[i].OperadorFiltro;

                PropertyInfo pi=objectType.GetProperty(filtros[i].CampoFisico);
                string campoFisicoQ=filtros[i].CampoFisico;
                if (pi!=null && Transformer.IsSubclassOfRawGeneric(typeof(UDT),pi.PropertyType)){
                    campoFisicoQ=filtros[i].CampoFisico + ".Value";
                }

                string filtro = " [" + campoFisicoQ + "] " + filtros[i].OperadorFiltro + " " + valor + " ";

                string valorFiltroString = filtros[i].ValorFiltro;

                if (operadorF.Equals(OperadorFiltro.Contiene))
                {
                    filtro = " [" + campoFisicoQ + "] Like '%" + valorFiltroString + "%' ";
                }

                if (operadorF.Equals(OperadorFiltro.Comienza))
                {
                    filtro = " [" + campoFisicoQ + "] Like '" + valorFiltroString + "%' ";
                }

                if (valorFiltroString.Contains(ComodinesFiltro.UnLugar) || valorFiltroString.Contains(ComodinesFiltro.VariosLugares))
                {
                    valorFiltroString = valorFiltroString.Replace(ComodinesFiltro.UnLugar, "_");
                    valorFiltroString = valorFiltroString.Replace(ComodinesFiltro.VariosLugares, "%");

                    filtro = " [" + campoFisicoQ + "] Like '" + valorFiltroString + "' ";
                }

                if (string.IsNullOrWhiteSpace(res))
                {
                    res = filtro;
                }
                else
                {
                    res += " " + operadorS + " " + filtro;
                }
            }
            return res;
        }
     
        #endregion

        /// <summary>
        /// Retirna un valor con las comillas o sin ellas dependiendo del tipo
        /// </summary>
        /// <param name="fiedls">Fileds</param>
        /// <param name="valor">valor</param>
        /// <returns></returns>
        private static string GetValorByType(List<ConsultasFields> fiedls, DTO_glConsultaFiltro filtro)
        {
            string response = filtro.ValorFiltro;
            foreach (ConsultasFields mf in fiedls)
            {
                if (mf.Field.Equals(filtro.CampoFisico))
                {
                    //Type t = Type.GetType("NewAge.DTO.UDT." + mf.Tipo + ", NewAge.DTO");
                    //PropertyInfo pi = t.GetProperty("Value");
                    var type = mf.Tipo;

                    if (type.Equals(typeof(string)) || type.Equals(typeof(DateTime?)) || type.Equals(typeof(DateTime)))
                    {
                        response = "\"" + filtro.ValorFiltro + "\"";
                        return response;
                    }
                }
            }

            return response;
        }

        private static string GetSQLValorByType(Type dtoType, DTO_glConsultaFiltro filtro, List<ConsultasFields> fields)
        {
            string fieldName = filtro.CampoFisico;
            string value = string.Empty;
            Type t = FieldType(dtoType, fieldName);
            if (t == null)
            {
                List<ConsultasFields> filtered = fields.Where(x => x.Field.Equals(fieldName)).ToList();
                if (filtered.Count > 0)
                    t = filtered.First().Tipo;
            }
            if (t != null)
            {
                if (t == typeof(string) || t == typeof(DateTime) || t == typeof(DateTime?))
                {
                    if (filtro.OperadorFiltro.Trim().Equals(OperadorFiltro.Contiene))
                        value = "'%" + filtro.ValorFiltro + "%'";
                    else if (filtro.OperadorFiltro.Trim().Equals(OperadorFiltro.Comienza))
                        value = "'" + filtro.ValorFiltro + "%'";
                    else
                        value = "'" + filtro.ValorFiltro + "'";
                    value = value.Replace(ComodinesFiltro.UnLugar, "_");
                    value = value.Replace(ComodinesFiltro.VariosLugares, "%");
                }
                else
                {
                    value = filtro.ValorFiltro;
                }
            }
            else
            {
                value = filtro.ValorFiltro;
            }
            //value = value.Replace("'", "''"); // Most important one! This line alone can prevent most injection attacks
            //value = value.Replace("--", "");
            //value = value.Replace("[", "[[]");
            return value;
        }

        /// <summary>
        /// Retirna un valor con las comillas o sin ellas dependiendo del tipo
        /// </summary>
        /// <param name="fiedls">Fileds</param>
        /// <param name="valor">valor</param>
        /// <returns></returns>
        private static string GetGridFilterValorByType(List<ConsultasFields> fiedls, DTO_glConsultaFiltro filtro)
        {
            string response = filtro.ValorFiltro;

            foreach (ConsultasFields mf in fiedls)
            {
                if (mf.Field.Equals(filtro.CampoFisico))
                {
                    var type = mf.Tipo;

                    if (type.Equals(typeof(string)) || type.Equals(typeof(DateTime?)) || type.Equals(typeof(DateTime)))
                    {
                        response = "'" + filtro.ValorFiltro + "'";
                        return response;
                    }
                }
            }

            return response;
        }

        /// <summary>
        /// Devuelve el tipo de una propiedad. Si no la encuentre devuelve el tipo de null
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static Type FieldType(Type objectType, string fieldName)
        {
            PropertyInfo pi = objectType.GetProperty(fieldName);
            Type fieldType = null;
            if (pi != null)
            {
                if (IsSubclassOfRawGeneric(typeof(UDT), pi.PropertyType))
                {
                    Type udtType = pi.PropertyType;
                    fieldType = udtType.GetProperty("Value").PropertyType;
                }
                else
                    fieldType = pi.PropertyType;
            }
            else 
            {
                FieldInfo fi = objectType.GetField(fieldName);
                if (fi != null)
                {
                    if (IsSubclassOfRawGeneric(typeof(UDT), fi.FieldType))
                    {
                        Type udtType = fi.FieldType;
                        fieldType = udtType.GetProperty("Value").PropertyType;
                    }
                    else
                    {
                        fieldType = fi.FieldType;
                    }
                }
            }

            return fieldType;
        }

        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }


    /// <summary>
    /// Clase con los tipos de operadores
    /// </summary>
    public class OperadorFiltro
    {
        public static string Igual = "=";
        public static string Menor = "<";
        public static string MenorIgual = "<=";
        public static string Mayor = ">";
        public static string MayorIgual = ">=";
        public static string Diferente = "<>";
        public static string Contiene = "Like";
        public static string Comienza = "Starts";
    }

    /// <summary>
    /// Clase con los tipos de operadores
    /// </summary>
    public class OperadorSentencia
    {
        public static string Or = "OR";
        public static string And = "AND";
    }

    /// <summary>
    /// Clase con los tipos de operadores
    /// </summary>
    public class ComodinesFiltro
    {
        public static string UnLugar = "#";
        public static string VariosLugares = "*";
    }
}
