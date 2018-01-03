using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.Project
{
    /// <summary>
    /// Clase que lleva el progreso de una operación
    /// </summary>
    public static class DictionaryProgress
    {
        #region Variables

        /// <summary>
        /// Contiene el progreso de las transacciones enviadas por los usuarios <(usuario - documentoID), progreso>
        /// </summary>
        public static Dictionary<Tuple<int, int>, int> BatchProgress = new Dictionary<Tuple<int, int>, int>();

        #endregion

        /// <summary>
        /// Prepara el diccionario para registrar progreso de una transaccion <(usuario - documentoID), progreso>
        /// </summary>
        /// <param name="userID">Identificadpr del usuario</param>
        /// <param name="DocumentID">Identificador del proceso (documento)</param>
        public static void IniciarProceso(int userID, int DocumentID)
        {
            Tuple<int, int> t = new Tuple<int, int>(userID, DocumentID);
            if (BatchProgress == null)
            {
                BatchProgress = new Dictionary<Tuple<int, int>, int>();
                BatchProgress.Add(t, 0);
            }

            if (BatchProgress.ContainsKey(t))
                BatchProgress[t] = 0;
            else
                BatchProgress.Add(t, 0);
        }

        /// <summary>
        /// Consulta el progreso de una transacción
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="DocumentID">Identificador del proceso (documento)</param>
        /// <returns>Retorna el porcentaje del progreso</returns>
        public static int ConsultarProgreso(int userID, int DocumentID)
        {
            Tuple<int, int> t = new Tuple<int, int>(userID, DocumentID);
            if (BatchProgress != null && BatchProgress.ContainsKey(t))
                return BatchProgress[t];
            else
                return 0;
        }

    }
}
