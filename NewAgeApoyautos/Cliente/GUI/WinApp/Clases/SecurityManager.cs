using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    /// <summary>
    /// Clase que maneja las seguridades de los usuarios
    /// </summary>
    internal static class SecurityManager
    {

        #region Funciones Internal

        /// <summary>
        /// Codigo de seguridad de la forma
        /// </summary>
        internal static long SecurityCode{get; set;}

        /// <summary>
        /// Lista de permisos de la forma
        /// </summary>
        internal static BitArray Rights { get; set; }

        /// <summary>
        /// Devuelve la seguridad de un formulario
        /// </summary>
        /// <param name="oldV">Valor actual</param>
        /// <param name="newV">Nuevo valor</param>
        /// <returns>Retorna el nivel de seguridad de un formulario</returns>
        internal static long SetFormSecurity(long oldV, long newV)
        {
            try
            {
                int retValue = 0;
                int arrayLength = 32;
                BitArray oldRights = new BitArray(BitConverter.GetBytes(oldV));
                BitArray newRights = new BitArray(BitConverter.GetBytes(newV));
                BitArray finalRights = new BitArray(arrayLength);

                //Inicializa el arreglo de bits
                for (int i = 0; i < arrayLength; ++i)
                {
                    finalRights[i] = (oldRights[i] == true || newRights[i] == true) ? true : false;
                }

                var result = new int[1];
                finalRights.CopyTo(result, 0);
                retValue = result[0];

                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Devuelve se una forma tiene permisos sobre una accion
        /// </summary>
        /// <param name="act">Accion</param>
        /// <returns>Retorna verdadero si el usuario tiene permisos sobre una accion, de lo contrario retorna falso</returns>
        internal static bool HasAccess(int formId, FormsActions act)
        {
            try
            {
                int action = (int)act;
                string outP = string.Empty;

                BaseController bc = BaseController.GetInstance();
                bc.AdministrationModel.FormsSecurity.TryGetValue(formId.ToString(), out outP);

                if (String.IsNullOrEmpty(outP))
                {
                    return false;
                }
                else
                {
                    SecurityCode = Convert.ToInt64(outP);
                    Rights = new BitArray(BitConverter.GetBytes(SecurityCode));
                    return Rights[action];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Trae el arreglo de las seguridades 
        /// </summary>
        /// <param name="value">Entero que representa las seguridades</param>
        /// <returns></returns>
        internal static BitArray GetDocumentSecurity(long value)
        {
            try
            {
                Rights = new BitArray(BitConverter.GetBytes(value));
                return Rights;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Asigna el codigo de las seguridades en formato de numero (Para guardar en la BD)
        /// </summary>
        /// <param name="arr">Arreglo</param>
        /// <returns>Retorna el codigo de seguridad</returns>
        internal static long GetCodeSecurity(BitArray arr)
        {
            long res = 0;
            for (int i = 0; i < arr.Length; ++i)
            {
                if (arr[i])
                {
                    long t = (long)Math.Pow(2, i);
                    res += t;
                }
            }

            return res;
        }

        #endregion

    }
}
