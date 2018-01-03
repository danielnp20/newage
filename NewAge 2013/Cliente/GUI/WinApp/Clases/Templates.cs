using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraPrinting.Native;
using Word =  Microsoft.Office.Interop.Word;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    public class Templates
    {
        #region Variables
        private BaseController _bc = BaseController.GetInstance();
        private object missing = Type.Missing;

        private Word.Application app = new Word.Application();
        private Word.Document doc = new Word.Document();
        #endregion

        /// <summary>
        /// Genera la plantilla con los parametros establecidos
        /// </summary>
        /// <param name="pathTemplate">Nombre del documento que tiene la plantilla</param>
        /// <param name="parameters">Parametros para reemplazar</param>
        internal void GenerarPlantilla(string nameDoc,string pathTemplate, List<string> parameters,ref bool saveOK)
        {
            try
            {
                string url = _bc.UrlDocumentFile(TipoArchivo.Plantillas, null, null, pathTemplate);

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Word Documents (*.doc)|*.doc";
                saveDialog.FileName = nameDoc;
                saveDialog.RestoreDirectory = true;
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    app = new Word.Application();
                    doc = app.Documents.Open(url);
                    this.ReplaceWordDoc(doc, parameters);
               
                    string docName = saveDialog.FileName;
                    if (docName.Length > 0)
                    {
                        object oDocName = (object)docName;
                        doc.SaveAs(oDocName);
                        saveOK = true;
                    }
                }
                try
                { 
                    doc.Close();
                    app.Quit();
                }
                catch (Exception) { ;}    
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Templates.cs", "GenerarPlantilla"));
            }
        }

        /// <summary>
        /// Genera la plantilla con los parametros establecidos
        /// </summary>
        /// <param name="pathTemplate">Nombre del documento que tiene la plantilla</param>
        /// <param name="parameters">Parametros para reemplazar</param>
        internal void GenerarPlantillaPDF(string nameDoc, string pathTemplate, List<string> parameters, ref bool saveOK)
        {
            try
            {
                string url = _bc.UrlDocumentFile(TipoArchivo.Plantillas, null, null, pathTemplate);

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PDF (*.pdf)|*.pdf";
                saveDialog.FileName = nameDoc;
                //saveDialog.RestoreDirectory = true;
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    app = new Word.Application();
                    doc = app.Documents.Open(url);
                    this.ReplaceWordDoc(doc, parameters);    

                    string docName = saveDialog.FileName;
                    if (docName.Length > 0)
                    {
                        object oDocName = (object)docName;
                        doc.SaveAs(oDocName);
                        Process.Start(saveDialog.FileName);
                        saveOK = true;
                    }
                }
                try
                {
                    doc.Close();
                    app.Quit();
                }
                catch (Exception) { ;}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Templates.cs", "GenerarPlantilla"));
            }
        }

        /// <summary>
        /// Cambia los parametros
        /// </summary>
        /// <param name="doc">Documento</param>
        /// <param name="parameters">Lista de parametros para reemplazar</param>
        private void ReplaceWordDoc(Word.Document doc, List<string> parameters)
        {
            for (int i = 0; i < parameters.Count; ++i)
            {
                foreach (Range tmpRange in ((Document)doc).StoryRanges)
                {
                    // Set the text to find and replace
                    tmpRange.Find.Text = "{" + i.ToString() +"}";
                    tmpRange.Find.Replacement.Text = parameters[i];

                    // Set the Find.Wrap property to continue (so it doesn't prompt the user or stop when it hits the end of the section)
                    tmpRange.Find.Wrap = WdFindWrap.wdFindContinue;

                    // Declare an object to pass as a parameter that sets the Replace parameter to the "wdReplaceAll" enum
                    object replaceAll = WdReplace.wdReplaceAll;

                    // Execute the Find and Replace -- notice that the 1th parameter is the "replaceAll" enum object
                    tmpRange.Find.Execute(ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref replaceAll,
                        ref missing, ref missing, ref missing, ref missing);
                }
            }
        }
    }
}
