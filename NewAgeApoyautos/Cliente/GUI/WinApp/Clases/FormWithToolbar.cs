using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    /// <summary>
    /// Define los métodos necesarios para implementar la funcionalidad de la barra de herramientas
    /// </summary>
    public class FormWithToolbar : Form
    {
        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public virtual void TBNew() { }
        
        /// <summary>
        /// Boton para salvar los cambios
        /// </summary>
        public virtual void TBSave() { }

        /// <summary>
        /// Boton para eliminar un o un conjunto de registros
        /// </summary>
        public virtual void TBDelete() { }


        ///////////////////////// Separador Barra /////////////////////////

        /// <summary>
        /// Boton para imprimir la lista de resultados
        /// </summary>
        public virtual void TBPrint() { }

        /// <summary>
        /// Boton para filtrar datos basicos
        /// </summary>
        public virtual void TBSearch() { }
        
        /// <summary>
        /// Boton para filtrar la lista de resultados
        /// </summary>
        public virtual void TBFilter() { }

        /// <summary>
        /// Boton para asignar un filtro de resultados por defecto
        /// </summary>
        public virtual void TBFilterDef() { }

        /// <summary>
        /// Boton para asignar un filtro de resultados por defecto
        /// </summary>
        public virtual void TBEdit() { }

        ///////////////////////// Separador Barra /////////////////////////

        /// <summary>
        /// Boton para generar una plantilla para importar registros
        /// </summary>
        public virtual void TBGenerateTemplate() { }

        /// <summary>
        /// Boton para copiar un conjunto de registros de un documento
        /// </summary>
        public virtual void TBCopy() { }

        /// <summary>
        /// Boton para pegar un conjunto de registros de un documento
        /// </summary>
        public virtual void TBPaste() { }

        /// <summary>
        /// Boton para copiar un o un conjunto de registros
        /// </summary>
        public virtual void TBImport() { }

        /// <summary>
        /// Boton para pegar lo que este en el portapapeles
        /// </summary>
        public virtual void TBExport() { }


        ///////////////////////// Separador Barra /////////////////////////

        /// <summary>
        /// Boton para mostrar actualizar contenido
        /// </summary>
        public virtual void TBUpdate() { }

        /// <summary>
        /// Boton para mostrar resetear un password
        /// </summary>
        public virtual void TBResetPwd() { }

        /// <summary>
        /// Boton para revertir
        /// </summary>
        public virtual void TBRevert() { }

        /// <summary>
        /// Boton para Enviar un documento para aprobacion
        /// </summary>
        public virtual void TBSendtoAppr() { }

        ///////////////////////// Separador Barra /////////////////////////

        /// <summary>
        /// Boton para mostrar alarmas
        /// </summary>
        public virtual void TBAlarm() { }

        /// <summary>
        /// Boton para mostrar ayuda
        /// </summary>
        public virtual void TBHelp() { }

        /// <summary>
        /// Boton para cerrar el formulario
        /// </summary>
        public virtual void TBClose() { }

    }
}
