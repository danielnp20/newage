using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using System.Reflection;
using System.Data;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Clases
{

    /// <summary>
    /// Clase para importar y exportar documentos
    /// </summary>
    internal class ImpExpData
    {
        #region Variables y propiedades

        private BaseController _bc = BaseController.GetInstance();
        internal DTO_glDocMigracionEstructura Estructura { get; set; }
        internal List<DTO_glDocMigracionCampo> Campos { get; set; }

        #endregion

        internal ImpExpData(string codDoc)
        {
            try
            {
                #region Estructura

                this.Estructura = (DTO_glDocMigracionEstructura)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocMigracionEstructura, false, codDoc, false);

                #endregion
                #region Campos

                if (this.Estructura != null)
                {
                    Dictionary<string, string> pks = new Dictionary<string, string>();
                    pks.Add("CodigoDoc", codDoc);

                    DTO_glConsulta query = new DTO_glConsulta();
                    List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "CodigoDoc",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = codDoc,
                        OperadorSentencia = OperadorSentencia.And
                    });
                    query.Filtros.AddRange(filtros);

                    long count = _bc.AdministrationModel.MasterComplex_Count(AppMasters.glDocMigracionCampo, query, true);
                    IEnumerable<DTO_MasterComplex> all = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.glDocMigracionCampo, count, 1, query, true);
                    this.Campos = all.Cast<DTO_glDocMigracionCampo>().ToList();
                }
                else
                    this.Campos = new List<DTO_glDocMigracionCampo>();
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ImpExpData_WinApp.cs", "ImpExpDocumentos"));
            }
        }

        #region Funciones públicas

        /// <summary>
        /// Exporta la información de un documento
        /// </summary>
        internal void ExportData(DataTable table)
        {
            try
            {
                if (this.Estructura == null)
                    return;

                //List<DTO_glDocMigracionCampo> camposIniciales = this.campos.Where(c => c.TipoRegistro.Value.Value == (byte)TipoRegistro.Inicial).ToList();
                //List<DTO_glDocMigracionCampo> camposDetalles = this.campos.Where(c => c.TipoRegistro.Value.Value == (byte)TipoRegistro.Detalle).OrderBy(c => c.DetalleNumero).ToList();
                //List<DTO_glDocMigracionCampo> camposFinales = this.campos.Where(c => c.TipoRegistro.Value.Value == (byte)TipoRegistro.Final).ToList();

                //foreach (DataRow Fila in table.Rows)
                //{
                //    foreach

                //}
            }
            catch
            {

            }
        }

        #endregion
    }
}
