using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_ReportFacturaVenta
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReportFacturaVenta(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.NITCliente.Value = dr["NITCliente"].ToString();
                this.ClienteDesc.Value = dr["ClienteDesc"].ToString();
                this.DirCliente.Value = dr["DirCliente"].ToString();
                this.DirComercialCli.Value = dr["DirComercialCli"].ToString();
                this.TelCliente.Value = dr["TelCliente"].ToString();
                this.CorreoCliente.Value = dr["CorreoCliente"].ToString();
                this.CiudadCliente.Value = dr["CiudadCliente"].ToString();
                this.DocumentoNro.Value = dr["DocumentoNro"].ToString();
                this.PrefijoID.Value = dr["PrefijoID"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaDoc"].ToString()))
                    this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                if (!string.IsNullOrEmpty(dr["FechaVto"].ToString()))
                    this.FechaVto.Value = Convert.ToDateTime(dr["FechaVto"]);
                this.ServicioID.Value = dr["ServicioID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.Observacion.Value = dr["Observacion"].ToString();
                this.Responsable.Value = dr["Responsable"].ToString();
                this.Producto.Value = dr["Producto"].ToString();
                if (!string.IsNullOrEmpty(dr["ValorUNI"].ToString()))
                    this.ValorUNI.Value = Convert.ToDecimal(dr["ValorUNI"]);
                if (!string.IsNullOrEmpty(dr["CantidadUNI"].ToString()))
                    this.CantidadUNI.Value = Convert.ToDecimal(dr["CantidadUNI"]);
                if (!string.IsNullOrEmpty(dr["VlrBruto"].ToString()))
                    this.VlrBruto.Value = Convert.ToDecimal(dr["VlrBruto"]);
                if (!string.IsNullOrEmpty(dr["Iva"].ToString()))
                    this.Iva.Value = Convert.ToDecimal(dr["Iva"]);             
                if (!string.IsNullOrEmpty(dr["VlrTotal"].ToString()))
                    this.VlrTotal.Value = Convert.ToDecimal(dr["VlrTotal"]);
                if (!string.IsNullOrEmpty(dr["NroItem"].ToString()))
                    this.NroItem.Value = Convert.ToInt32(dr["NroItem"]);
                if (!string.IsNullOrEmpty(dr["ImprimeInd"].ToString()))
                    this.ImprimeInd.Value = Convert.ToBoolean(dr["ImprimeInd"]);
                if (!string.IsNullOrEmpty(dr["Retencion1"].ToString()))
                    this.Retencion1.Value = Convert.ToDecimal(dr["Retencion1"]);
                if (!string.IsNullOrEmpty(dr["Retencion2"].ToString()))
                    this.Retencion2.Value = Convert.ToDecimal(dr["Retencion2"]);
                if (!string.IsNullOrEmpty(dr["Retencion3"].ToString()))
                    this.Retencion3.Value = Convert.ToDecimal(dr["Retencion3"]);
                if (!string.IsNullOrEmpty(dr["Retencion4"].ToString()))
                    this.Retencion4.Value = Convert.ToDecimal(dr["Retencion4"]);
                if (!string.IsNullOrEmpty(dr["Retencion5"].ToString()))
                    this.Retencion5.Value = Convert.ToDecimal(dr["Retencion5"]);
                if (!string.IsNullOrEmpty(dr["Retencion6"].ToString()))
                    this.Retencion6.Value = Convert.ToDecimal(dr["Retencion6"]);
                if (!string.IsNullOrEmpty(dr["Retencion7"].ToString()))
                    this.Retencion7.Value = Convert.ToDecimal(dr["Retencion7"]);
                if (!string.IsNullOrEmpty(dr["Retencion8"].ToString()))
                    this.Retencion8.Value = Convert.ToDecimal(dr["Retencion8"]);
                if (!string.IsNullOrEmpty(dr["Retencion9"].ToString()))
                    this.Retencion9.Value = Convert.ToDecimal(dr["Retencion9"]);
                if (!string.IsNullOrEmpty(dr["Retencion10"].ToString()))
                    this.Retencion10.Value = Convert.ToDecimal(dr["Retencion10"]);
                if (!string.IsNullOrWhiteSpace(dr["DatoAdd1"].ToString()))
                    this.DatoAdd1.Value = Convert.ToString(dr["DatoAdd1"]);
                if (!string.IsNullOrWhiteSpace(dr["DatoAdd2"].ToString()))
                    this.DatoAdd2.Value = Convert.ToString(dr["DatoAdd2"]);
                if (!string.IsNullOrWhiteSpace(dr["DatoAdd3"].ToString()))
                    this.DatoAdd3.Value = Convert.ToString(dr["DatoAdd3"]);
                if (!string.IsNullOrWhiteSpace(dr["DatoAdd4"].ToString()))
                    this.DatoAdd4.Value = Convert.ToString(dr["DatoAdd4"]);
                if (!string.IsNullOrWhiteSpace(dr["DatoAdd5"].ToString()))
                    this.DatoAdd5.Value = Convert.ToString(dr["DatoAdd5"]);
                if (!string.IsNullOrEmpty(dr["PorcAnticipo"].ToString()))
                    this.PorcAnticipo.Value = Convert.ToDecimal(dr["PorcAnticipo"]);
                if (!string.IsNullOrEmpty(dr["PorcRteGarantia"].ToString()))
                    this.PorcRteGarantia.Value = Convert.ToDecimal(dr["PorcRteGarantia"]);
                if (!string.IsNullOrEmpty(dr["Administracion"].ToString()))
                    this.Administracion.Value = Convert.ToDecimal(dr["Administracion"]);
                else
                    this.Administracion.Value = 0;
                if (!string.IsNullOrEmpty(dr["Imprevistos"].ToString()))
                    this.Imprevistos.Value = Convert.ToDecimal(dr["Imprevistos"]);
                else
                    this.Imprevistos.Value = 0;
                if (!string.IsNullOrEmpty(dr["Utilidad"].ToString()))
                    this.Utilidad.Value = Convert.ToDecimal(dr["Utilidad"]);
                else
                    this.Utilidad.Value = 0;
                this.TotalAIU.Value = this.Administracion.Value + this.Imprevistos.Value + this.Utilidad.Value;
                this.VlrTotal.Value = this.VlrTotal.Value + this.TotalAIU.Value;
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                this.FormaPago.Value = dr["FormaPago"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportFacturaVenta()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ClienteID = new UDT_ClienteID();
            this.NITCliente = new UDT_Descriptivo();
            this.ClienteDesc = new UDT_Descriptivo();
            this.DirCliente = new UDT_DescripTBase();
            this.DirComercialCli = new UDT_DescripTBase();
            this.TelCliente = new UDTSQL_char(20);
            this.CorreoCliente = new UDTSQL_char(60);
            this.CiudadCliente = new UDT_Descriptivo();
            this.DocumentoNro = new UDTSQL_char(6);
            this.PrefijoID = new UDT_PrefijoID();
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.Observacion = new UDT_DescripTExt();
            this.Responsable = new UDTSQL_char(30);
            this.FechaVto = new UDTSQL_smalldatetime();
            this.ServicioID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.Producto = new UDT_DescripTExt();
            this.ValorUNI = new UDT_Valor();
            this.CantidadUNI = new UDT_Cantidad();
            this.VlrBruto = new UDT_Valor();
            this.Iva = new UDT_Valor();
            this.VlrTotal = new UDT_Valor();
            this.ImprimeInd = new UDT_SiNo();
            this.NroItem = new UDTSQL_int();
            this.NitEmpresa = new UDT_Descriptivo();
            this.TelefonoEmpresa = new UDTSQL_char(20);
            this.DireccionEmpresa = new UDT_DescripTBase();
            this.CiudadEmpresa = new UDT_DescripTBase();
            this.VlrAnticipo = new UDT_Valor();
            this.PorcAnticipo = new UDT_Valor();
            this.VlrRteGarantia = new UDT_Valor();
            this.PorcRteGarantia = new UDT_Valor();
            this.Retencion1 = new UDT_Valor();
            this.Retencion2 = new UDT_Valor();
            this.Retencion3 = new UDT_Valor();
            this.Retencion4 = new UDT_Valor();
            this.Retencion5 = new UDT_Valor();
            this.Retencion6 = new UDT_Valor();
            this.Retencion7 = new UDT_Valor();
            this.Retencion8 = new UDT_Valor();
            this.Retencion9 = new UDT_Valor();
            this.Retencion10 = new UDT_Valor();
            this.Administracion = new UDT_Valor();
            this.Imprevistos = new UDT_Valor();
            this.Utilidad = new UDT_Valor();
            this.TotalAIU = new UDT_Valor();
            this.DatoAdd1 = new UDTSQL_char(20);
            this.DatoAdd2 = new UDTSQL_char(20);
            this.DatoAdd3 = new UDTSQL_char(20);
            this.DatoAdd4 = new UDTSQL_char(20);
            this.DatoAdd5 = new UDTSQL_char(20);
            this.ProyectoID = new UDT_ProyectoID();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.FormaPago = new UDT_DescripTBase();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo NITCliente { get; set; }

        [DataMember]
        public UDT_Descriptivo ClienteDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase DirCliente { get; set; }

        [DataMember]
        public UDT_DescripTBase DirComercialCli { get; set; }

        [DataMember]
        public UDTSQL_char TelCliente { get; set; }

        [DataMember]
        public UDTSQL_char CorreoCliente { get; set; }

        [DataMember]
        public UDT_Descriptivo CiudadCliente { get; set; }

        [DataMember]
        public UDTSQL_char DocumentoNro { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDTSQL_char Responsable { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaVto { get; set; }

        [DataMember]
        public UDT_CodigoBSID ServicioID { get; set; }

        [DataMember]
        public UDT_inReferenciaID  inReferenciaID { get; set; }

        [DataMember]
        public UDT_DescripTExt Producto { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase FormaPago { get; set; }

        [DataMember]
        public UDT_Valor ValorUNI { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadUNI { get; set; }

        [DataMember]
        public UDT_Valor VlrBruto { get; set; }

        [DataMember]
        public UDT_Valor Iva { get; set; }

        [DataMember]
        public UDT_Valor VlrTotal { get; set; }

        [DataMember]
        public UDT_Descriptivo NitEmpresa { get; set; }

        [DataMember]
        public UDTSQL_char TelefonoEmpresa { get; set; }

        [DataMember]
        public UDT_DescripTBase DireccionEmpresa { get; set; }

        [DataMember]
        public UDT_DescripTBase CiudadEmpresa { get; set; }

        [DataMember]
        public UDT_SiNo ImprimeInd { get; set; }

        [DataMember]
        public UDTSQL_int NroItem { get; set; }

        [DataMember]
        public UDT_Valor VlrAnticipo { get; set; }

        [DataMember]
        public UDT_Valor PorcAnticipo { get; set; }

        [DataMember]
        public UDT_Valor VlrRteGarantia { get; set; }

        [DataMember]
        public UDT_Valor PorcRteGarantia { get; set; }

        [DataMember]
        public UDT_Valor Retencion1 { get; set; }

        [DataMember]
        public UDT_Valor Retencion2 { get; set; }

        [DataMember]
        public UDT_Valor Retencion3 { get; set; }

        [DataMember]
        public UDT_Valor Retencion4 { get; set; }

        [DataMember]
        public UDT_Valor Retencion5 { get; set; }

        [DataMember]
        public UDT_Valor Retencion6 { get; set; }

        [DataMember]
        public UDT_Valor Retencion7 { get; set; }

        [DataMember]
        public UDT_Valor Retencion8 { get; set; }

        [DataMember]
        public UDT_Valor Retencion9 { get; set; }

        [DataMember]
        public UDT_Valor Retencion10 { get; set; }

        [DataMember]
        public UDT_Valor Administracion { get; set; }

        [DataMember]
        public UDT_Valor Imprevistos { get; set; }

        [DataMember]
        public UDT_Valor Utilidad { get; set; }

        [DataMember]
        public UDT_Valor TotalAIU { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd1 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd2 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd3 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd4 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd5 { get; set; }

        #endregion

    }
}
