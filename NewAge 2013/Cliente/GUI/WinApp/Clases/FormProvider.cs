using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraVerticalGrid;
using NewAge.Cliente.GUI.WinApp.Forms;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraTab;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Controls;


namespace NewAge.Cliente.GUI.WinApp.Clases
{
    /// <summary>
    /// Clase usada para tener una sola instancia activa de un formulario
    /// </summary>
    internal static class FormProvider
    {
        #region Propiedades

        /// <summary>
        /// Diccionario de formularios
        /// </summary>
        internal static Dictionary<Type, Form> lookup = new Dictionary<Type, Form>();

        /// <summary>
        /// Diccionarios con el estado de la barra de herramientas
        /// </summary>
        internal static Dictionary<int, bool[]> tb_Status = new Dictionary<int, bool[]>();
 
        /// <summary>
        /// Formulario Activo
        /// </summary>
        internal static Form Active
        {
            get;
            set;
        }

        /// <summary>
        /// Formulario Maestro
        /// </summary>
        internal static MDI Master
        {
            get;
            set;
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// Remueve un formulario de la colección
        /// </summary>
        /// <param name="sender">Quien inicia el evento</param>
        /// <param name="e">Evento que se ejecuta cuando cierra el formulario</param>
        private static void Remover(Object sender, FormClosedEventArgs e)
        {
            lookup.Remove(sender.GetType());
        }

        /// <summary>
        /// Funcion recursiva que cambia los recursos de los controles y sus hijos
        /// </summary>
        /// <param name="ctrls">Lista de controles</param>
        /// <param name="bc">Base controller</param>
        private static void LoadResources(Control.ControlCollection ctrls, BaseController bc)
        {
            try
            {
                //Barra de herramientas
                if (ctrls.Owner is ToolStrip)
                {
                    var items = ((ToolStrip)ctrls.Owner).Items;
                    foreach (ToolStripItem item in items)
                    {
                        item.Text = bc.GetResource(LanguageTypes.ToolBar, item.Text);
                    }
                }

                if
                (
                    ctrls.Owner is Label || ctrls.Owner is LinkLabel || ctrls.Owner is Button || ctrls.Owner is GroupBox || ctrls.Owner is CheckBox || ctrls.Owner is RadioButton ||
                    ctrls.Owner is CheckEdit || ctrls.Owner is LabelControl || ctrls.Owner is DockPanel || ctrls.Owner is XtraTabPage || ctrls.Owner is GroupControl || ctrls.Owner is CheckButton || ctrls.Owner is RadioGroup || ctrls.Owner is SimpleButton
                )
                {
                    ctrls.Owner.Text = bc.GetResource(LanguageTypes.Forms, ctrls.Owner.Text);
                }

                //Controles del formulario
                if (ctrls.Count > 0)
                {

                    foreach (Control c in ctrls)
                    {
                        if
                        (
                            c is Label || c is LinkLabel || c is Button || c is GroupBox || c is CheckBox || c is RadioButton ||
                            c is CheckEdit || c is LabelControl || c is DockPanel || c is XtraTabPage || c is GroupControl || c is CheckButton || c is SimpleButton
                        )
                        {
                            c.Text = bc.GetResource(LanguageTypes.Forms, c.Text);
                        }
                        if (c is SimpleButton)
                        {
                            SimpleButton ctrl = (SimpleButton)c;
                            ctrl.ToolTip = bc.GetResource(LanguageTypes.Forms, ctrl.ToolTip);
                        }
                        if (c is RadioGroup)
                        {
                            RadioGroup rg = (RadioGroup)c;
                            foreach (RadioGroupItem item in rg.Properties.Items)
                            {
                                item.Description = bc.GetResource(LanguageTypes.Forms, item.Description);
                            }
                        }
                        if (c.Controls.Count > 0)
                            LoadResources(c.Controls, bc);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Funciones Internas

        internal static bool HasForms()
        {
            return lookup.Count() > 0 ? true : false;
        }

        /// <summary>
        /// Obtiene la instancia de un formulario
        /// </summary>
        /// <param name="owner">Formulario</param>
        /// <param name="m">Formulario maestro</param>
        /// <param name="t">Tipo</param>
        /// <returns>Retorna el formulario requerido</returns>
        internal static Form GetInstance(Type t)
        {
            try
            {
                // Devuelve el formulario si ya esta creado
                if (lookup.ContainsKey(t))
                {
                    lookup[t].Activate();
                    Active = lookup[t];
                    Master.tabForms.SelectedTabPage = Master.GetTabPage(lookup[t].Name);
                    Master.tvModules.SelectedNode = Master.tvModules.Nodes[Master.Module.ToString()].Nodes[lookup[t].Name];

                    return lookup[t];
                }

                // Crea una nueva instancia
                Form f = (Form)Activator.CreateInstance(t);
                lookup.Add(t, f);
                f.MdiParent = Master;
                f.WindowState = FormWindowState.Maximized;
                f.Show();
                return f;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene la instancia de un formulario
        /// </summary>
        /// <param name="owner">Formulario</param>
        /// <param name="m">Formulario maestro</param>
        /// <param name="t">Tipo</param>
        /// <returns>Retorna el formulario requerido</returns>
        internal static Form GetInstance(Type t, ModulesPrefix mod)
        {
            try
            {
                // Devuelve el formulario si ya esta creado
                if (lookup.ContainsKey(t))
                {
                    lookup[t].Activate();
                    Active = lookup[t];
                    Master.tabForms.SelectedTabPage = Master.GetTabPage(lookup[t].Name);
                    Master.tvModules.SelectedNode = Master.tvModules.Nodes[Master.Module.ToString()].Nodes[lookup[t].Name];

                    return lookup[t];
                }

                // Crea una nueva instancia
                Form f = (Form)Activator.CreateInstance(t, new object[] { mod.ToString() });
                lookup.Add(t, f);
                f.MdiParent = Master;
                f.WindowState = FormWindowState.Maximized;
                f.Show();
                return f;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene la instancia de un formulario
        /// </summary>
        /// <param name="owner">Formulario</param>
        /// <param name="m">Formulario maestro</param>
        /// <param name="t">Tipo</param>
        /// <returns>Retorna el formulario requerido</returns>
        internal static Form GetInstance(Type t, object[] arr)
        {
            try
            {
                // Devuelve el formulario si ya esta creado
                if (lookup.ContainsKey(t))
                {
                    lookup[t].Activate();
                    Active = lookup[t];
                    Master.tabForms.SelectedTabPage = Master.GetTabPage(lookup[t].Name);
                    Master.tvModules.SelectedNode = Master.tvModules.Nodes[Master.Module.ToString()].Nodes[lookup[t].Name];

                    return lookup[t];
                }

                // Crea una nueva instancia
                Form f = (Form)Activator.CreateInstance(t, arr);
                lookup.Add(t, f);
                f.MdiParent = Master;
                f.WindowState = FormWindowState.Maximized;
                f.Show();
                return f;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retorna el estado de la barra de herramientas de un formulario
        /// </summary>
        /// <param name="docID">Identificadoer del documento</param>
        /// <returns>Retorna las seguridades del documento</returns>
        internal static bool[] GetTB(int docID)
        {
            if (tb_Status.ContainsKey(docID))
                return tb_Status[docID];
            else
                return null;
        }

        /// <summary>
        /// Asigna los permisos de la TB segun el documento
        /// </summary>
        /// <param name="docID">Identificadoer del documento</param>
        /// <param name="tb">Permisos de la barra de herramientas</param>
        internal static void SetTB(int docID, bool[] tb)
        {
            tb_Status[docID] = tb;
        }

        /// <summary>
        /// Identifica si un formulario se encuentra activo
        /// </summary>
        /// <param name="t">Tipo</param>
        /// <returns>Retorna el indicador de existencia</returns>
        internal static Form GetForm(Type t)
        {
            return lookup.ContainsKey(t) ? lookup[t] : null;
        }

        /// <summary>
        /// Identifica si un formulario se encuentra activo
        /// </summary>
        /// <param name="t">Tipo</param>
        /// <returns>Retorna el indicador de existencia</returns>
        internal static bool Exists(Type t)
        {
            return lookup.ContainsKey(t) ? true : false;
        }

        /// <summary>
        /// Remueve un formulario de la colección
        /// </summary>
        internal static void Close(Type t)
        {
            // Devuelve el formulario si ya esta creado
            if (lookup.ContainsKey(t))
            {
                lookup[t].Close();
                if (lookup.Count <= 0)
                {
                    Master.FormType = FormTypes.Other;
                }
            };
        }

        /// <summary>
        /// Remueve todos los formularios abiertos
        /// </summary>
        internal static void CloseAll()
        {
            try
            {
                int count = lookup.Keys.Count;
                for (int i = 0; i < count; ++i)
                {
                    Type t = lookup.Keys.ElementAt(0);
                    lookup[t].Close();
                }

                Master.FormType = FormTypes.Other;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Remueve el formulario actual
        /// </summary>
        /// <param name="sender">Quien inicia el evento</param>
        /// <param name="e">Evento que se ejecuta cuando cierra el formulario</param>
        internal static void CloseCurrent()
        {
            if (lookup.ContainsKey(Active.GetType()))
            {
                lookup[Active.GetType()].Close();
                if (lookup.Count <= 0)
                {
                    Master.FormType = FormTypes.Other;
                }
            };
        }

        /// <summary>
        /// Remueve un formulario de la colección
        /// </summary>
        /// <param name="docID">Documento que se debe eliminar</param>
        /// <param name="t">Instancia de formulario que se debe eliminar</param>
        internal static void Remove(int docID, Type t)
        {
            // Limpia los diccionarios de formularios y TB
            if (lookup.ContainsKey(t))
                lookup.Remove(t);

            if (tb_Status.ContainsKey(docID))
                tb_Status.Remove(docID);
        }

        /// <summary>
        /// Carga los recursos de una forma
        /// </summary>
        /// <param name="frm">Forma que se necesita cargar</param>
        /// <param name="title">Titlu de la forma</param>
        internal static void LoadResources(Form frm, int documentID)
        {
            try
            {
                BaseController bc = BaseController.GetInstance();
                frm.Text = bc.GetResource(LanguageTypes.Forms, documentID.ToString());
                frm.Name = frm.GetType().Name;

                foreach (Control c in frm.Controls)
                {
                    LoadResources(c.Controls, bc);
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        /// <summary>
        /// Devuelve un tipo de formulario segun el nombre
        /// </summary>
        /// <param name="str">Nombre del formulario</param>
        /// <returns>Retirna el tipo de un formulario especifico</returns>
        internal static Type GetFormType(string form)
        {
            Type tipo = Type.GetType("NewAge.Cliente.GUI.WinApp.Forms." + form);
            return tipo;
        }

        /// <summary>
        /// Devuelve un tipo de formulario segun el nombre
        /// </summary>
        /// <param name="str">Nombre del formulario</param>
        /// <returns>Retirna el tipo de un formulario especifico</returns>
        internal static Type GetFormularioType(string form)
        {
            Type tipo = Type.GetType("NewAge.Cliente.GUI.WinApp.Formularios." + form);
            return tipo;
        }

        #endregion     

    }
}
