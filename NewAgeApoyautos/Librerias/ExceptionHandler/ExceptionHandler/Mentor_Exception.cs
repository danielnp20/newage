using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Runtime;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.IO;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.Logger.Facade;
using NewAge.DTO.Logger;

namespace NewAge.Librerias.ExceptionHandler
{
    public class Mentor_Exception : Exception
    {
        #region Funciones privadas

        /// <summary>
        /// Returns a path or directory or key 
        /// </summary>
        /// <param name="dnfe"></param>
        /// <returns></returns>
        private static string GetString(Exception dnfe, int index)
        {
            System.Text.RegularExpressions.Regex pathMatcher = new System.Text.RegularExpressions.Regex(@"[^']+");
            if (pathMatcher.Matches(dnfe.Message).Count > 1)
            {
                if (pathMatcher.Matches(dnfe.Message)[index - 1] != null)
                {
                    return pathMatcher.Matches(dnfe.Message)[index - 1].Value;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns a path or directory or key 
        /// </summary>
        /// <param name="dnfe"></param>
        /// <returns></returns>
        private static string GetValue(Exception dnfe)
        {
            string response = "";
            string msg = dnfe.Message;
            if (msg.Contains('(') && msg.Contains(')'))
            {
                int ini = msg.IndexOf('(');
                int fin = msg.IndexOf(')');
                response = msg.Substring(ini + 1, fin - ini - 1).Trim();
            }
            return response;

        }

        /// <summary>
        /// Trae el mensaje del error
        /// </summary>
        /// <returns></returns>
        private static string GetErrorMessage(Exception ex)
        {
            string usrMessage = string.Empty;
            if (ex.Message.StartsWith("ERR_"))
            {
                return ex.Message;
            }
            else
            {
                usrMessage = "ERR_";
                #region DirectoryNotFoundException  Directory not found.  
                if (ex is DirectoryNotFoundException)
                {
                    var path = GetString(ex, 1);
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.DirectoryNotFoundException) + "_" + Convert.ToInt32(Enums.ExceptionType.DirectoryNotFoundException).ToString().PadLeft(4, '0');
                    if (path != string.Empty)
                    {
                        usrMessage = usrMessage + "&&" + path;
                    }
                }
                #endregion
                #region FileNotFoundException  File not found.  
                else if (ex is FileNotFoundException)
                {
                    var path = GetString(ex, 1);
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.FileNotFoundException) + "_" + Convert.ToInt32(Enums.ExceptionType.FileNotFoundException).ToString().PadLeft(4, '0');
                    if (path != string.Empty)
                    {
                        usrMessage = usrMessage + "&&" + path;
                    }
                }
                #endregion
                #region UnauthorizedAccessException Does nothave access
                else if (ex is UnauthorizedAccessException)
                {
                    var path = GetString(ex, 1);
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.UnauthorizedAccessException) + "_" + Convert.ToInt32(Enums.ExceptionType.UnauthorizedAccessException).ToString().PadLeft(4, '0');
                    if (path != string.Empty)
                    {
                        usrMessage = usrMessage + "&&" + path;
                    }
                }
                #endregion
                #region EntityException 
                else if (ex is EntityException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.EntityException) + "_" + Convert.ToInt32(Enums.ExceptionType.EntityException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region ArgumentException  An argument to a method was invalid.  
                else if (ex is ArgumentException)
                {
                    var path = GetString(ex,1);
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.ArgumentException) + "_" + Convert.ToInt32(Enums.ExceptionType.ArgumentException).ToString().PadLeft(4, '0');
                    if (path != string.Empty)
                    {
                        usrMessage = usrMessage + "&&" + path;
                    }
                }
                #endregion
                #region ArgumentNullException  A null argument was passed to a method that doesn't accept it. 
                else if (ex is ArgumentNullException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.ArgumentNullException) + "_" + Convert.ToInt32(Enums.ExceptionType.ArgumentNullException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region ArgumentOutOfRangeException  Argument value is out of range.  
                else if (ex is ArgumentOutOfRangeException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.ArgumentOutOfRangeException) + "_" + Convert.ToInt32(Enums.ExceptionType.ArgumentOutOfRangeException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region ArithmeticException  Arithmetic over - or underflow has occurred.  
                else if (ex is ArithmeticException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.ArithmeticException) + "_" + Convert.ToInt32(Enums.ExceptionType.ArithmeticException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region ArrayTypeMismatchException  Attempt to store the wrong type of object in an array.  
                else if (ex is ArrayTypeMismatchException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.ArrayTypeMismatchException) + "_" + Convert.ToInt32(Enums.ExceptionType.ArrayTypeMismatchException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region BadImageFormatException  Image is in the wrong format. 
                else if (ex is BadImageFormatException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.BadImageFormatException) + "_" + Convert.ToInt32(Enums.ExceptionType.BadImageFormatException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region DivideByZeroException  An attempt was made to divide by zero.  
                else if (ex is DivideByZeroException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.DivideByZeroException) + "_" + Convert.ToInt32(Enums.ExceptionType.DivideByZeroException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region FormatException  The format of an argument is wrong.  
                else if (ex is FormatException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.FormatException) + "_" + Convert.ToInt32(Enums.ExceptionType.FormatException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region IndexOutOfRangeException  An array index is out of bounds.  
                else if (ex is IndexOutOfRangeException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.IndexOutOfRangeException) + "_" + Convert.ToInt32(Enums.ExceptionType.IndexOutOfRangeException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region InvalidOperationException  A method was called at an invalid time.
                else if (ex is InvalidOperationException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.InvalidOperationException) + "_" + Convert.ToInt32(Enums.ExceptionType.InvalidOperationException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region MissingMemberException  An invalid version of a DLL was accessed.
                else if (ex is MissingMemberException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.MissingMemberException) + "_" + Convert.ToInt32(Enums.ExceptionType.MissingMemberException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region NotFiniteNumberException  A number is not valid. 
                else if (ex is NotFiniteNumberException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.NotFiniteNumberException) + "_" + Convert.ToInt32(Enums.ExceptionType.NotFiniteNumberException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region NotSupportedException  Indicates sthat a method is not implemented by a class.  
                else if (ex is NotSupportedException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.NotSupportedException) + "_" + Convert.ToInt32(Enums.ExceptionType.NotSupportedException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region NullReferenceException  Attempt to use an unassigned reference.  
                else if (ex is NullReferenceException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.NullReferenceException) + "_" + Convert.ToInt32(Enums.ExceptionType.NullReferenceException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region OutOfMemoryException  Not enough memory to continue execution. 
                else if (ex is OutOfMemoryException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.OutOfMemoryException) + "_" + Convert.ToInt32(Enums.ExceptionType.OutOfMemoryException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region StackOverflowException  A stack has overflown. 
                else if (ex is StackOverflowException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.StackOverflowException) + "_" + Convert.ToInt32(Enums.ExceptionType.StackOverflowException).ToString().PadLeft(4, '0');
                }
                #endregion
                #region MENTORDataException  
                else if (ex is MentorDataException)
                {
                    if (((MentorDataException)ex).ErrorCode == ExceptionHandler.Enums.MentorDataExceptionType.Control)
                    {
                        usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.MentorDataException) + "_" + (10 + Convert.ToInt32(Enums.ExceptionType.MentorDataException).ToString()); 
                        if (!((MentorDataException)ex).Equals(string.Empty))
                        {
                            usrMessage = usrMessage + "&&" + ((MentorDataException)ex).Detail;                            
                        }
                    }
                    else if (((MentorDataException)ex).ErrorCode == ExceptionHandler.Enums.MentorDataExceptionType.Reflection)
                    {
                        usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.MentorDataException) + "_" + (11 + Convert.ToInt32(Enums.ExceptionType.MentorDataException).ToString());
                        if (!((MentorDataException)ex).Equals(string.Empty))
                        {
                            usrMessage = usrMessage + "&&" + ((MentorDataException)ex).Origen + "&&" +((MentorDataException)ex).Detail;
                        }
                    }
                    else
                    {
                        usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.MentorDataException) + "_" + Convert.ToInt32(Enums.ExceptionType.MentorDataException).ToString().PadLeft(4, '0');
                        if (!((MentorDataException)ex).Equals(string.Empty))
                        {
                            usrMessage = usrMessage + "&&" + ((MentorDataException)ex).Detail;
                        }
                    }
                }
                #endregion
                #region MENTORData parameters Exception  
                else if (ex is MentorDataParametersException)
                {
                    if (!((MentorDataParametersException)ex).Type.Equals(ExceptionHandler.MentorDataParametersException.ExType.Lenght))
                    {
                        usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.MentorDataParametersException) + "_" + (10 + Convert.ToInt32(Enums.ExceptionType.MentorDataParametersException).ToString());
                        if (!((MentorDataParametersException)ex).Equals(string.Empty))
                        {
                            usrMessage = usrMessage + "&&" + ((MentorDataParametersException)ex).Key + "&&" + ((MentorDataParametersException)ex).MaxLenght;
                        }
                    }
                    else if (!((MentorDataParametersException)ex).Type.Equals(ExceptionHandler.MentorDataParametersException.ExType.Table))
                    {
                        usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.MentorDataParametersException) + "_" + (20 + Convert.ToInt32(Enums.ExceptionType.MentorDataParametersException).ToString());
                        if (!((MentorDataParametersException)ex).Equals(string.Empty))
                        {
                            usrMessage = usrMessage + "&&" + ((MentorDataParametersException)ex).Key;
                        }
                    }
                }
                #endregion
                #region SqlException  Failure to access a type member;such as a method or field. 
                else if (ex is SqlException)
                {
                    #region Primary Key Violation
                    if (((SqlException)ex).Number.Equals(2627))
                    {
                        usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.SqlException) + "_" + (100 + Convert.ToInt32(Enums.ExceptionType.SqlException).ToString());
                        //usrMessage = usrMessage + "&&";// +path;
                    }
                    #endregion
                    #region  ForeignKey Violation (contraint)
                    else if (((SqlException)ex).Number.Equals(547))
                    {
                        usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.SqlException) + "_" + (200 + Convert.ToInt32(Enums.ExceptionType.SqlException).ToString());
                        //usrMessage = usrMessage + "&&";// +path;
                    }
                    #endregion
                    #region  Unique Index/Constriant Violation
                    else if (((SqlException)ex).Number.Equals(2601))
                    {
                        usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.SqlException) + "_" + (300 + Convert.ToInt32(Enums.ExceptionType.SqlException).ToString());
                        //usrMessage = usrMessage + "&&";// +path;
                    }
                    #endregion
                    #region Cannot insert null values
                    else if (((SqlException)ex).Number.Equals(515))
                    {
                        string path = GetString(ex, 2) + " " + GetValue(ex);
                        usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.SqlException) + "_" + (400 + Convert.ToInt32(Enums.ExceptionType.SqlException).ToString());
                        usrMessage = usrMessage + "&&" + path;
                    }
                    #endregion
                    #region Cannot convert values
                    else if (((SqlException)ex).Number.Equals(245))
                    {
                        usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.SqlException) + "_" + (500 + Convert.ToInt32(Enums.ExceptionType.SqlException).ToString());
                        //usrMessage = usrMessage + "&&";// +path;
                    }
                    #endregion
                    else
                    {
                        usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.SqlException) + "_" + Convert.ToInt32(Enums.ExceptionType.SqlException).ToString().PadLeft(4, '0');
                    }
                }
                #endregion
                #region SystemException  A failed run-time check;used as a base class for other.
                else if (ex is SystemException)
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.SystemException) + "_" + Convert.ToInt32(Enums.ExceptionType.SystemException).ToString().PadLeft(4, '0');
                }
                #endregion
                else
                {
                    usrMessage = usrMessage + StringEnum.GetStringValue(Enums.ExceptionType.UnKnownException) + "_" + Convert.ToInt32(Enums.ExceptionType.UnKnownException).ToString().PadLeft(4, '0');
                }
            }

            return usrMessage;
        }

        #endregion

        #region Funciones públicas

        /// <summary>
        /// Writes a message to the log
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="ex">Exception</param>
        /// <param name="errorLocation">Error location</param>
        /// <param name="sendMail">True to send a email, false to not send the email</param>
        public static string LogException_Local(string strConn, Exception e, string user, string location)
        {
            var ex = e.GetBaseException();
            string usrMessage = GetErrorMessage(ex);

            LoggerFacade.LogError(strConn, user, ex, location);

            return usrMessage;
        }

        /// <summary>
        /// Writes a message to the log
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="ex">Exception</param>
        /// <param name="errorLocation">Error location</param>
        /// <param name="sendMail">True to send a email, false to not send the email</param>
        public static string LogException_Service(Exception e, string user, string location, bool SendToDBAndEmail)
        {
            var ex = e.GetBaseException();
            string usrMessage = GetErrorMessage(ex);

            LoggerFacade.LogServiceError(user, ex, location);

            return usrMessage;
        }

        #endregion
    }
}

