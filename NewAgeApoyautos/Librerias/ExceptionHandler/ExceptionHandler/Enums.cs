using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NewAge.Librerias.Project;

namespace NewAge.Librerias.ExceptionHandler
{
    public class Enums
    {
        /// <summary>
        /// Enumeration for Exception categories
        /// </summary>
        public enum ExceptionType
        {
            [StringValue("DIR")]
            DirectoryNotFoundException = 1,
            [StringValue("FIL")]
            FileNotFoundException = 2,                                 
            [StringValue("UAE")]
            UnauthorizedAccessException = 3,
            [StringValue("ETY")]
            EntityException = 4,
            [StringValue("SQL")]
            SqlException = 5,           
            [StringValue("ARG")]
            ArgumentException = 6,
            [StringValue("AGN")]
            ArgumentNullException = 7,
            [StringValue("AOR")]
            ArgumentOutOfRangeException = 8,
            [StringValue("ART")]
            ArithmeticException = 9,
            [StringValue("ATM")]
            ArrayTypeMismatchException = 10,
            [StringValue("BIF")]
            BadImageFormatException = 11,
            [StringValue("COE")]
            CoreException = 12,            
            [StringValue("DBZ")]
            DivideByZeroException = 13,
            [StringValue("FOR")]
            FormatException = 14,
            [StringValue("IOR")]
            IndexOutOfRangeException = 15,
            [StringValue("ICE")]
            InvalidCastExpression = 16,
            [StringValue("IOP")]
            InvalidOperationException = 17,
            [StringValue("MSM")]
            MissingMemberException = 18,
            [StringValue("NFN")]
            NotFiniteNumberException = 19,
            [StringValue("NSP")]
            NotSupportedException = 20,
            [StringValue("NRF")]
            NullReferenceException = 21,
            [StringValue("OOM")]
            OutOfMemoryException = 22,
            [StringValue("SOF")]
            StackOverflowException = 23,
            [StringValue("SYS")]
            SystemException = 24,
            [StringValue("DAT")]
            MentorDataException = 25,
            [StringValue("PAR")]
            MentorDataParametersException = 26,
            [StringValue("UKW")]
            UnKnownException = 27
        }

        /// <summary>
        /// Enumeration for Exception categories
        /// </summary>
        public enum MentorDataExceptionType
        {
            Control,
            Reflection,
        }
    }
}
