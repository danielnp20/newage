using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.Project
{
    /// <summary>
    /// Clase que devuelve un resultado para pegar varias filas
    /// </summary>
    public class PasteOpDTO
    {
        /// <summary>
        /// Identifica si la operación fue exitosa
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mensaje devuelto de la operación
        /// </summary>
        public string MsgResult { get; set; }
    }

    /// <summary>
    /// Contiene los formatos establecidos para importar y exportar cualquier tabla
    /// </summary>
    public static class TableFormat
    {
        /// <summary>
        /// Constante con la identificación de un carácter "Tab"
        /// </summary>
        private const string tabChar = "\t";

        /// <summary>
        /// Formato para las maestras simples
        /// Se cargan dinamicamente de acuerdo a la funcion "FillMasterSimple"
        /// </summary>
        public static string MasterSimple = string.Empty;

        /// <summary>
        /// Llena el formato d las maestras simples de acuerdo a una lista de columnas
        /// </summary>
        /// <param name="cols">Lista de columnas para exportar / importar</param>
        /// <returns>Retorna el formato requerido</returns>
        public static string FillMasterSimple(List<string> cols)
        {
            string ret = string.Empty;
            for (int i = 0 ; i < cols.Count ; ++i)
            {
                if (cols[i] != "Movimiento")
                {
                    ret += cols[i];
                    if (i != cols.Count - 1)
                    {
                        ret += tabChar;
                    }
                }
            }
            ret += Environment.NewLine;
            return ret;
        }

    }
}
