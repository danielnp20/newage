using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.Project 
{    
    /// <summary>
    /// Clase con el listado de variables de formatos string
    /// </summary>
    public static class FormatString
    {
        //Formato de nombres resumidos
        public static string DocumentShort = "_shortDesc";
        //Formatos Base de datos
        public static string DB_Date_MMsDDsYYYY = "MM/dd/yyyy";
        public static string DB_Date_YYYY_MM_DD = "yyyy-MM-dd";
        public static string En_Date = "MM/dd/yyyy";
        //Fechas
        public static string Time = "T";
        public static string Date = "dd/MM/yyyy";
        public static string Period = "yyyy/MM";
        public static string ControlDate = "yyyy/MM/dd";

    }
}
