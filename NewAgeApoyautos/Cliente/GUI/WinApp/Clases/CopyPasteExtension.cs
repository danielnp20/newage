using Microsoft.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    /// <summary>
    /// Clase q provee utilidades de importar y exportar
    /// </summary>
    public static class CopyPasteExtension
    {
        #region variables
        //Errores
        private static BaseController _bc = BaseController.GetInstance();
        private static string Err_ColumnsFormat = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_ColumnsFormat);
        private static string Err_ColumnDuplicated = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_ColumnDuplicated);
        private static string Err_ColumnInvalid = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_ColumnInvalid);
        private static string Err_ColumnLeft = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_ColumnLeft);
        private static string Err_Copy = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Copy);
        private static string Err_Paste = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Paste);
        //Mensajes
        private static string Msg_SuccessCopy = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessCopy);
        private static string Msg_SuccessPaste = _bc.GetResource(LanguageTypes.Messages ,DictionaryMessages.SuccessPaste);
        //Número de registros a insertar
        private static int numLineas;
        //Valor de retorno
        private static string retMsg = string.Empty;
        // Caracter para separar campos
        public const string tabChar = "\t";
        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Valida las columnas de un texto sugún el formato
        /// </summary>
        /// <param name="format">Formato para pegar</param>
        /// <param name="text">Fila de las columnas</param>
        private static bool ValidColumns(string format, string cols)
        {
            try
            {
                bool ret = true;
                format = format.Replace(Environment.NewLine, string.Empty);

                //Arreglo con las columnas del formato
                string[] formatColumns = format.Split(new string[] { tabChar }, StringSplitOptions.RemoveEmptyEntries);
                //Arreglo con las columnas dadas
                string[] textColumns = cols.Split(new string[] { tabChar }, StringSplitOptions.RemoveEmptyEntries);
                //Diccionario cpon diferencias entre las columnas dadas y las requeridas
                Dictionary<string, bool> dict = new Dictionary<string, bool>();
                //Diccionario cpon diferencias entre las columnas dadas y las requeridas
                Dictionary<string, string> dictErr = new Dictionary<string, string>();

                //Agrega las columnas reales al diccionario
                foreach (string s in formatColumns)
                {
                    dict.Add(s.ToLower(), false);
                }

                //Valida las columnas del contenido del portapapeles
                foreach (string s in textColumns)
                {
                    FindObject(dict, dictErr, s.ToLower());
                }

                //Verifica que no hayan errores con las columnas
                retMsg = string.Empty;
                if (dictErr.Count > 0)
                {
                    ret = false;
                    foreach (var s in dictErr)
                    {
                        retMsg += s.Value + Environment.NewLine;
                    }
                }

                if (dict.ContainsValue(false))
                {
                    ret = false;
                    foreach (var s in dict)
                    {
                        if (!s.Value)
                            retMsg += string.Format(Err_ColumnLeft, s.Key) + Environment.NewLine;
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para verificar la existencia de una columna en el texto original
        /// </summary>
        /// <param name="dic">Diccionario con las columnas del formato</param>
        /// <param name="dicErr">Diccionario con la lista de errores</param>
        /// <param name="obj">Llave que se va a buscar</param>
        private static void FindObject(Dictionary<string, bool> dic, Dictionary<string, string> dicErr, string obj)
        {
            try
            {
                if (dic.ContainsKey(obj))
                {
                    if (!dic[obj])
                    {
                        dic[obj] = true;
                    }
                    else
                    {
                        dicErr.Add(obj, string.Format(Err_ColumnDuplicated, obj));
                    }
                }
                else
                {
                    dicErr.Add(obj, string.Format(Err_ColumnInvalid, obj));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Copia el texto especificado en clipboard
        /// </summary>
        /// <param name="text"></param>
        public static string CopyToClipBoard(string text)
        {
            try
            {
                //Clipboard..Clear();
                Clipboard.SetText(text);

                return Msg_SuccessCopy;
            }
            catch (Exception ex)
            {
                return Err_Copy + ex.Message;
            }
        }

        /// <summary>
        /// Pega un texto del clip board para ingresar nuevos datos
        /// </summary>
        /// <param name="format">Formato para pegar</param>
        public static PasteOpDTO PasteFromClipBoard(string format)
        {
            try
            {
                var text = Clipboard.GetText();
                var lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                if (!ValidColumns(format, lines[0]))
                {
                    return new PasteOpDTO()
                    {
                        Success = false,
                        MsgResult = Err_ColumnsFormat + Environment.NewLine + retMsg
                    };
                }

                return new PasteOpDTO()
                {
                    Success = true,
                    MsgResult = text
                };                
            }
            catch (Exception ex)
            {
                return new PasteOpDTO()
                {
                    Success = false,
                    MsgResult = Err_Paste + ex.Message
                };    
            }
        }

        #endregion

    }
}
