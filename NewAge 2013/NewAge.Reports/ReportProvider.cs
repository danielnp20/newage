using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NewAge.Reports
{
    public class ReportProvider
    {
        #region Variables

        private ModuloGlobal modGlobal;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tx"></param>
        /// <param name="emp"></param>
        /// <param name="userID"></param>
        /// <param name="loggerConn"></param>
        public ReportProvider(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn)
        {
            modGlobal = new ModuloGlobal(conn, tx, emp, userID, loggerConn);
        }

        /// <summary>
        /// Funcion recursiva que cambia los recursos de los controles y sus hijos
        /// </summary>
        /// <param name="ctrls">Lista de controles</param>
        /// <param name="bc">Base controller</param>
        internal void LoadResources(IEnumerable<XRControl> ctrls)
        {                        
            try
            {
                //Controles del formulario
                if (ctrls.Count() > 0)
                {
                    foreach (XRControl c in ctrls)
                    {
                        if
                        (
                            c is XRLabel || c is XRTableCell
                        )
                        {
                            c.Text = this.modGlobal.GetResource(LanguageTypes.Forms, c.Text);
                        }                       
                        if (c.Controls.Count > 0)
                            this.LoadResources(c.AllControls<XRControl>());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       
    }
}
