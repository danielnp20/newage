using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NewAge.DTO.UDT
{
    /// <summary>
    /// Clase para manejo de expresiones regulares permitidas
    /// </summary>
    public static class UDT_RegExRules
    {
        public static Regex OnlyChars = new Regex("");
        public static Regex BooleanInt = new Regex("[0-1]");
        public static Regex GreaterThanZero = new Regex("");
    }
}
