using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte Comrprobante Control
    /// </summary>
    public class DTO_ReportOrdenCompraDoc
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportOrdenCompraDoc(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Proveedor.Value = Convert.ToString(dr["Proveedor"]);
                this.TerceroID.Value = Convert.ToString(dr["TerceroID"]);
                this.DireccionProv.Value = Convert.ToString(dr["DireccionProv"]);
                this.TelefonoProv.Value = Convert.ToString(dr["TelefonoProv"]);
                this.ContactoComercial.Value = Convert.ToString(dr["ContactoComercial"]);
                this.TelContacto.Value = Convert.ToString(dr["TelContacto"]);
                this.MailContacto.Value = Convert.ToString(dr["MailContacto"]);
                this.LugarEntrega.Value = Convert.ToString(dr["LugarEntrega"]);
                this.LugarEntregaDesc.Value = Convert.ToString(dr["LugarEntregaDesc"]);
                this.EncargadoRecibe.Value = Convert.ToString(dr["EncargadoRecibe"]);
                this.DireccionEntrega.Value = Convert.ToString(dr["DireccionEntrega"]);
                this.TelefonoEntrega.Value = Convert.ToString(dr["TelefonoEntrega"]);
                if (!string.IsNullOrEmpty(dr["FechaEntrega"].ToString()))
                    this.FechaEntrega.Value = Convert.ToDateTime(dr["FechaEntrega"]);
                this.CiudadPais.Value = Convert.ToString(dr["CiudadPais"]);                  
                    this.Estado.Value = Convert.ToString(dr["Estado"]);
                if (!string.IsNullOrEmpty(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.MonedaNeg.Value = Convert.ToString(dr["MonedaNeg"]);
                this.MonedaPago.Value = Convert.ToString(dr["MonedaPago"]);
               // this.Item.Value = Convert.ToString(dr["Item"]);
                this.BienServicio.Value = Convert.ToString(dr["BienServicio"]);
                if (!string.IsNullOrEmpty(dr["Cantidad"].ToString()))
                    this.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
                this.Unidad.Value = Convert.ToString(dr["Unidad"]);
                this.Referencia.Value = Convert.ToString(dr["Referencia"]);
                this.MarcaInvID.Value = Convert.ToString(dr["MarcaInvID"]);
                this.RefProveedor.Value = Convert.ToString(dr["RefProveedor"]);
                this.Descripcion.Value = Convert.ToString(dr["Descripcion"]);
                if (!string.IsNullOrEmpty(dr["ValorUnitario"].ToString()))
                    this.ValorUnitario.Value = Convert.ToDecimal(dr["ValorUnitario"]);
                if (!string.IsNullOrEmpty(dr["ValorTotal"].ToString()))
                    this.ValorTotal.Value = Convert.ToDecimal(dr["ValorTotal"]);
                if (!string.IsNullOrEmpty(dr["ValorAIU"].ToString()))
                    this.ValorAIU.Value = Convert.ToDecimal(dr["ValorAIU"]);
                if (!string.IsNullOrEmpty(dr["ValorIVA"].ToString()))
                    this.ValorIVA.Value = Convert.ToDecimal(dr["ValorIVA"]);
                this.FormaPago.Value = Convert.ToString(dr["FormaPago"]);
                this.Clausulas.Value = Convert.ToString(dr["Clausulas"]);
                this.Instrucciones.Value = Convert.ToString(dr["Instrucciones"]);
                this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
                this.Elaboro.Value = Convert.ToString(dr["Elaboro"]);
                this.ProyectoID.Value = Convert.ToString(dr["ProyectoID"]);
                this.ProyectoDesc.Value = Convert.ToString(dr["ProyectoDesc"]);
                if (!string.IsNullOrEmpty(dr["FechaConsec"].ToString()))
                    this.FechaConsec.Value = Convert.ToDateTime(dr["FechaConsec"]);
                this.PrefDoc.Value = Convert.ToString(dr["PrefDoc"]);
                if (!string.IsNullOrEmpty(dr["DtoProntoPago"].ToString()))
                    this.DtoProntoPago.Value = Convert.ToDecimal(dr["DtoProntoPago"]);
                if (!string.IsNullOrEmpty(dr["CantidadEmpaque"].ToString()))
                    this.CantidadEmpaque.Value = Convert.ToInt32(dr["CantidadEmpaque"]);
                this.EmpaqueInvID.Value = Convert.ToString(dr["EmpaqueInvID"]);
             
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportOrdenCompraDoc()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Proveedor = new UDT_Descriptivo();
            this.TerceroID = new UDT_TerceroID();
            this.DireccionProv = new UDT_DescripTBase();
            this.TelefonoProv = new UDTSQL_char(20);
            this.ContactoComercial = new UDTSQL_char(50);
            this.TelContacto = new UDTSQL_char(20);
            this.MailContacto = new UDT_DescripTBase();
            this.LugarEntrega = new UDT_LocFisicaID();
            this.LugarEntregaDesc = new UDT_Descriptivo();
            this.EncargadoRecibe = new UDT_DescripTBase();
            this.DireccionEntrega = new UDT_DescripTExt();
            this.TelefonoEntrega = new UDT_DescripTBase();
            this.FechaEntrega = new UDTSQL_smalldatetime();
            this.CiudadPais = new UDTSQL_char(203);          
            this.Estado = new UDTSQL_char(15);
            this.Fecha = new UDTSQL_datetime();
            this.MonedaNeg = new UDT_MonedaID();
            this.MonedaPago = new UDT_MonedaID();
            this.Item = new UDT_DescripTBase();
            this.BienServicio = new UDT_CodigoBSID();
            this.Cantidad = new UDT_Cantidad();
            this.Unidad = new UDT_UnidadInvID();
            this.Referencia = new UDT_inReferenciaID();
            this.MarcaInvID = new UDT_CodigoGrl();
            this.RefProveedor = new UDT_CodigoGrl20();
            this.Descripcion = new UDT_DescripTExt();
            this.ValorUnitario = new UDT_Valor();
            this.ValorTotal = new UDT_Valor();
            this.ValorIVA = new UDT_Valor();
            this.ValorAIU = new UDT_Valor();
            this.FormaPago = new UDT_DescripTExt();
            this.Clausulas = new UDT_DescripTExt();
            this.Instrucciones = new UDT_DescripTExt();
            this.Observaciones = new UDT_DescripTExt();
            this.Elaboro = new UDT_Descriptivo();
            this.Aprobo = new UDT_Descriptivo();
            this.ProyectoID = new  UDT_ProyectoID();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.ConsProyMvto = new UDT_Consecutivo();
            this.NumDocProyMvto = new UDT_Consecutivo();
            this.PrefDoc = new UDT_DescripTBase();
            this.FechaConsec = new UDTSQL_smalldatetime();
            this.DtoProntoPago = new UDT_PorcentajeID();
            this.EmpaqueInvID = new UDT_EmpaqueInvID();
            this.CantidadEmpaque = new UDT_Cantidad();
        }

        #region Propiedades

        [DataMember]
        public UDT_Descriptivo Proveedor { get; set; }

        [DataMember]
        public UDT_DescripTBase DireccionProv { get; set; }

        [DataMember]
        public UDTSQL_char TelefonoProv { get; set; }

        [DataMember]
        public UDTSQL_char ContactoComercial { get; set; }

        [DataMember]
        public UDTSQL_char TelContacto { get; set; }

        [DataMember]
        public UDT_DescripTBase MailContacto { get; set; }

        [DataMember]
        public UDT_LocFisicaID LugarEntrega { get; set; }

        [DataMember]
        public UDT_Descriptivo LugarEntregaDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase EncargadoRecibe { get; set; }

        [DataMember]
        public UDT_DescripTExt DireccionEntrega { get; set; }

        [DataMember]
        public UDT_DescripTBase TelefonoEntrega { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaEntrega { get; set; }

        [DataMember]
        public UDTSQL_char CiudadPais { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }
        
        [DataMember]
        public UDTSQL_char Estado { get; set; }
        
        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }        
       
        [DataMember]
        public UDT_MonedaID MonedaNeg { get; set; }
        
        [DataMember]
        public UDT_MonedaID MonedaPago { get; set; }
        
        [DataMember]
        public UDT_DescripTBase Item { get; set; }
        
        [DataMember]
        public UDT_CodigoBSID BienServicio { get; set; }
        
        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }
        
        [DataMember]
        public UDT_UnidadInvID Unidad { get; set; }
        
        [DataMember]
        public UDT_inReferenciaID Referencia { get; set; }

        [DataMember]
        public UDT_CodigoGrl MarcaInvID { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 RefProveedor { get; set; }
        
        [DataMember]
        public UDT_DescripTExt Descripcion { get; set; }
        
        [DataMember]
        public UDT_Valor ValorUnitario { get; set; }
        
        [DataMember]
        public UDT_Valor ValorTotal { get; set; }

        [DataMember]
        public UDT_Valor ValorIVA { get; set; }
        
        [DataMember]
        public UDT_Valor ValorAIU { get; set; }
        
        [DataMember]
        public UDT_DescripTExt FormaPago { get; set; }
        
        [DataMember]
        public UDT_DescripTExt Clausulas { get; set; }
        
        [DataMember]
        public UDT_DescripTExt Instrucciones { get; set; }
        
        [DataMember]
        public UDT_Descriptivo Elaboro { get; set; }

        [DataMember]
        public UDT_Descriptivo Aprobo { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsProyMvto { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocProyMvto { get; set; }

        [DataMember]
        public UDT_DescripTBase PrefDoc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaConsec { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDT_PorcentajeID DtoProntoPago { get; set; }

        [DataMember]
        public UDT_EmpaqueInvID EmpaqueInvID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadEmpaque { get; set; }   
        
        #endregion

    }

}
