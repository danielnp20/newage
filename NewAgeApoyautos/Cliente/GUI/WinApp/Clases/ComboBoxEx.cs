using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    /// <summary>
    /// Clase para manejar los items de un combo box con text y value
    /// </summary>
    public class ComboBoxItem
    {
        private string _value;
        private string _text;

        public string Value { get { return _value; } }
        public string Text { get { return _text; } }

        public ComboBoxItem(string cValue, string cText)
        {
            this._value = cValue;
            this._text = cText;
        }
    }

    /// <summary>  
    /// Represents an ComboBox with additional properties for setting the   
    /// size of the AutoComplete Drop-Down window.  
    /// </summary>  
    public class ComboBoxEx : ComboBox
    {
        #region Variables

        private int _acDropDownHeight = 106;
        private int _acDropDownWidth = 170;

        #endregion

        #region Propiedades

        //<EditorBrowsable(EditorBrowsableState.Always), _  
        [Browsable(true), Description("The width, in pixels, of the auto complete drop down box"), DefaultValue(170)]
        public int AutoCompleteDropDownWidth
        {
            get { return _acDropDownWidth; }

            set { _acDropDownWidth = value; }
        }

        //<EditorBrowsable(EditorBrowsableState.Always), _  
        [Browsable(true), Description("The height, in pixels, of the auto complete drop down box"), DefaultValue(106)]
        public int AutoCompleteDropDownHeight
        {
            get { return _acDropDownHeight; }

            set { _acDropDownHeight = value; }
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Obtiene un item del combobox segun el valor
        /// </summary>
        /// <param name="v">Valor</param>
        /// <returns>Retorna un item</returns>
        public ComboBoxItem GetItem(string v)
        {
            foreach (ComboBoxItem item in this.Items)
            {
                if (item.Value == v)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// remueve un item del combobox segun el valor
        /// </summary>
        /// <param name="v">Valor</param>
        /// <returns>Retorna un item</returns>
        public void RemoveItem(string v)
        {
            try
            {
                foreach (ComboBoxItem item in this.Items)
                {
                    if (item.Value == v)
                    {
                        this.Items.Remove(item);
                        return;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        #endregion

        #region Funciones Protected

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ACWindow.RegisterOwner(this);
        }

        #endregion

        #region Nested type: ACWindow

        /// <summary>  
        /// Provides an encapsulation of an Auto complete drop down window   
        /// handle and window proc.  
        /// </summary>  
        private class ACWindow : NativeWindow
        {
            private static readonly Dictionary<IntPtr, ACWindow> ACWindows;

            #region "Win API Declarations"

            private const UInt32 WM_WINDOWPOSCHANGED = 0x47;
            private const UInt32 WM_NCDESTROY = 0x82;

            private const UInt32 SWP_NOSIZE = 0x1;
            private const UInt32 SWP_NOMOVE = 0x2;
            private const UInt32 SWP_NOZORDER = 0x4;
            private const UInt32 SWP_NOREDRAW = 0x8;
            private const UInt32 SWP_NOACTIVATE = 0x10;

            private const UInt32 GA_ROOT = 2;
            private static readonly List<ComboBoxEx> owners;

            [DllImport("user32.dll")]
            private static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

            [DllImport("user32.dll")]
            private static extern IntPtr GetAncestor(IntPtr hWnd, UInt32 gaFlags);

            [DllImport("kernel32.dll")]
            private static extern int GetCurrentThreadId();

            [DllImport("user32.dll")]
            private static extern void GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

            [DllImport("user32.dll")]
            private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

            [DllImport("user32.dll")]
            private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

            #region Nested type: EnumThreadDelegate

            private delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

            #endregion

            #region Nested type: RECT

            [StructLayout(LayoutKind.Sequential)]
            private struct RECT
            {
                public readonly int Left;

                public readonly int Top;

                public readonly int Right;

                public readonly int Bottom;


                public Point Location
                {
                    get { return new Point(Left, Top); }
                }
            }

            #endregion

            #endregion

            private ComboBoxEx _owner;

            static ACWindow()
            {
                ACWindows = new Dictionary<IntPtr, ACWindow>();
                owners = new List<ComboBoxEx>();
            }

            /// <summary>  
            /// Creates a new ACWindow instance from a specific window handle.  
            /// </summary>  
            private ACWindow(IntPtr handle)
            {
                AssignHandle(handle);
            }

            /// <summary>  
            /// Registers a ComboBoxEx for adjusting the Complete Dropdown window size.  
            /// </summary>  
            public static void RegisterOwner(ComboBoxEx owner)
            {
                if ((owners.Contains(owner)))
                {
                    return;
                }

                owners.Add(owner);
                EnumThreadWindows(GetCurrentThreadId(), EnumThreadWindowCallback, IntPtr.Zero);
            }

            /// <summary>  
            /// This callback will receive the handle for each window that is  
            /// associated with the current thread. Here we match the drop down window name   
            /// to the drop down window name and assign the top window to the collection  
            /// of auto complete windows.  
            /// </summary>  
            private static bool EnumThreadWindowCallback(IntPtr hWnd, IntPtr lParam)
            {
                if ((GetClassName(hWnd) == "Auto-Suggest Dropdown"))
                {
                    IntPtr handle = GetAncestor(hWnd, GA_ROOT);

                    if ((!ACWindows.ContainsKey(handle)))
                    {
                        ACWindows.Add(handle, new ACWindow(handle));
                    }
                }
                return true;
            }

            /// <summary>  
            /// Gets the class name for a specific window handle.  
            /// </summary>  
            private static string GetClassName(IntPtr hRef)
            {
                var lpClassName = new StringBuilder(256);
                GetClassName(hRef, lpClassName, 256);

                return lpClassName.ToString();
            }

            /// <summary>  
            /// Overrides the NativeWindow's WndProc to handle when the window  
            /// attributes changes.  
            /// </summary>  
            protected override void WndProc(ref Message m)
            {
                if ((m.Msg == WM_WINDOWPOSCHANGED))
                {
                    // If the owner has not been set we need to find the ComboBoxEx that  
                    // is associated with this dropdown window. We do it by checking if  
                    // the upper-left location of the drop-down window is within the   
                    // ComboxEx client rectangle.   
                    if ((_owner == null))
                    {
                        Rectangle ownerRect = default(Rectangle);
                        var acRect = new RECT();

                        foreach (ComboBoxEx cbo in owners)
                        {
                            GetWindowRect(Handle, ref acRect);
                            ownerRect = cbo.RectangleToScreen(cbo.ClientRectangle);

                            if ((ownerRect.Contains(acRect.Location)))
                            {
                                _owner = cbo;
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                        owners.Remove(_owner);
                    }

                    if (((_owner != null)))
                    {
                        SetWindowPos(Handle, IntPtr.Zero, -5, 0, _owner.AutoCompleteDropDownWidth,
                                     _owner.AutoCompleteDropDownHeight, SWP_NOMOVE | SWP_NOZORDER | SWP_NOACTIVATE);
                    }
                }

                if ((m.Msg == WM_NCDESTROY))
                {
                    ACWindows.Remove(Handle);
                }

                base.WndProc(ref m);
            }
        }

        #endregion
    }
}
