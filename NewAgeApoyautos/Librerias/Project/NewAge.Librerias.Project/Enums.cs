using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.Project
{ 
    /// <summary>
    /// Enumeración con la lista de tipos de idiomas
    /// </summary>
    public enum LanguageTypes
    {
        Error = 1,
        Forms = 2,
        Menu = 3,
        Messages = 4,
        Modules = 5,
        Tables = 6,
        ToolBar = 7,
        Mail = 8
    }

    /// <summary>
    /// Enumerados que definen los tipos de
    /// intervalos de tiempo posibles.
    /// </summary>
    public enum DateInterval
    {
        Day,
        DayOfYear,
        Hour,
        Minute,
        Month,
        Quarter,
        Second,
        Weekday,
        WeekOfYear,
        Year
    }

}
