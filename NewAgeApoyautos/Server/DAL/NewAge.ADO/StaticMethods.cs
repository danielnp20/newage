using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;

namespace NewAge.ADO
{
    public static class StaticMethods
    {
        #region Variables 

        /// <summary>
        /// Lista de parametros por tabla
        /// </summary>
        private static Dictionary<string, SortedDictionary<int, DTO_aplMaestraPropiedades>> masterProps = new Dictionary<string, SortedDictionary<int, DTO_aplMaestraPropiedades>>();

        #endregion

        #region Funciones privadas

        /// <summary>
        /// Carga la informacion de los parametros para todas las maestras
        /// </summary>
        private static void LoadDictionary(SqlConnection c, SqlTransaction tx, string loggerConn)
        {
            DAL_aplMaestraPropiedad dal = new DAL_aplMaestraPropiedad(c, tx, null, 0, loggerConn);
            var masters = dal.DAL_aplMaestraPropiedades_GetAll();

            SortedDictionary<int, DTO_aplMaestraPropiedades> cMasters = new SortedDictionary<int, DTO_aplMaestraPropiedades>();
            foreach (DTO_aplMaestraPropiedades p in masters)
            {
                try
                {
                    cMasters[p.DocumentoID] = p;
                }
                catch (Exception ex)
                {
                    ;
                }
            }

            masterProps[c.Database] = cMasters;
        }

        #endregion

        #region Funciones Públicas

        /// <summary>
        /// Obtiene la lista de parametros de un formulario
        /// </summary>
        /// <param name="key">Documento ID</param>
        /// <returns>Retorna las propiedades de una tabla maestra</returns>
        public static DTO_aplMaestraPropiedades GetParameters(SqlConnection c, SqlTransaction tx, int key, string loggerConn)
        {
            string dbName = c.Database;
            if (masterProps.ContainsKey(dbName))
            {
                if (masterProps[dbName].Count == 0)
                    LoadDictionary(c, tx, loggerConn);
            }
            else
            {
                LoadDictionary(c, tx, loggerConn);
            }

            DTO_aplMaestraPropiedades props;
            SortedDictionary<int, DTO_aplMaestraPropiedades> cMasters = masterProps[dbName];            

            cMasters.TryGetValue(key, out props);
            if (props == null || props.DocumentoID == 0)
            {
                throw new MentorDataParametersException(key.ToString(), MentorDataParametersException.ExType.Table, null);
            }

            return props;
        }

        /// <summary>
        /// Carga el grupo de empresas general de la tabla de control
        /// </summary>
        /// <returns>Retorn el grupo de empresas consultado</returns>
        public static string GetGrupoEmpresasControl(SqlConnection c, SqlTransaction tx, string loggerConn)
        {
            DAL_glControl dal = new DAL_glControl(c, tx, null, 0, loggerConn);
            string ge = dal.DAL_glControl_GetById(AppControl.GrupoEmpresaGeneral).Data.Value;

            return ge;
        }

        #endregion

    }
}
