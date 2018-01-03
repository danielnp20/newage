using System;
using System.Collections.Generic;
using System.Text;
using Excel;
using System.Windows.Forms;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    /// <summary>
    /// crea un documento de excel
    /// </summary>
    public class ExcelGenerator
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private Excel.Application _app = null;
        private Excel.Workbook _workBook = null;
        private Excel.Worksheet _workSheet = null;
        private Excel.Range _workSheet_range = null;
        // Verifica si puede seguir con la siguiente lines
        private bool _nextRow = true;
        private bool _throwEx = false;

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Libera un objeto
        /// </summary>
        /// <param name="obj">objeto</param>
        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// Cuenta el numero de ceros que hay al iniciar el string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string FormatNumber(string data)
        {
            string res = "0";
            char[] arr = data.ToCharArray();
            for (int i = 0; i < arr.Length - 1; ++i)
            {
                res += "0";
            }

            return res;
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ExcelGenerator()
        {
            this.CreateEmptyDoc();
        }

        /// <summary>
        /// Crea el documento
        /// </summary>
        public void CreateEmptyDoc()
        {
            try
            {
                _app = new Excel.Application();
                if (_app == null)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidStartExcel));
                    return;
                }

                _app.Visible = true;

                _workBook = _app.Workbooks.Add(1);
                _workSheet = (Excel.Worksheet)_workBook.Sheets[1];
                if (_workSheet == null)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCreateWorkSheet));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CreateExcelDoc.cs", "CreateDoc"));
            }
        }

        /// <summary>
        /// Agrega Data
        /// </summary>
        /// <param name="row">Fila</param>
        /// <param name="col">Columna</param>
        /// <param name="data">Dato</param>
        /// <param name="cell">Celda</param>
        public void AddData(int row, int col, string data)
        {
            try
            {
                int result = 0;

                if (!data.StartsWith("0") || !Int32.TryParse(data, out result))
                {
                    _workSheet.Cells[row, col] = data;
                }
                else
                {
                    //normal
                    var cell = (Range)_workSheet.Cells[row, col];
                    string format = this.FormatNumber(data);
                    cell.NumberFormat = format;
                    cell.Value = data;

                    //normal
                    //var cell3 = (Range)worksheet.Cells[row + 3, col];
                    //cell3.Value = "'" + data;
                }

                this._nextRow = true;
                this._throwEx = false;
            }
            catch (Exception e)
            {
                if (e is System.Runtime.InteropServices.COMException)
                {
                    if (e.Message.Contains("0x800AC472"))
                    {
                        TopMessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ExportingNotHandle));
                        this._nextRow = true;
                        this._throwEx = true;
                    }
                }
                else
                {
                    this._nextRow = false;
                    this._throwEx = true;
                    throw e;
                }
            }
            finally
            {
                if (this._nextRow && this._throwEx)
                {
                    this.AddData(row, col, data);
                }
            }
        }

        /// <summary>
        /// Cloase the current book
        /// </summary>
        public void CloseBook()
        {
            this._workBook.Close(true);
            this._app.Quit();

            this.ReleaseObject(this._workSheet);
            this.ReleaseObject(this._workBook);
            this.ReleaseObject(_app);
        }

        #endregion
    }
}
