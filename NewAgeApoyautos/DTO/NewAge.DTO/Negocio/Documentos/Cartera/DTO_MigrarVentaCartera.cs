using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// DTO Tabla DTO_MigrarFacturaVenta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_MigrarVentaCartera
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_MigrarVentaCartera()
        {
            InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            #region Venta Cartera
            this.Libranza = new UDT_LibranzaID();
            this.ClienteID = new UDT_ClienteID();
            this.Nombre = new UDT_DescripTBase();
            this.CompradorCartera = new UDT_CompradorCarteraID();
            this.CuotasVendidas = new UDTSQL_int();
            this.TasaVenta = new UDT_Valor();
            this.VlrCuota = new UDT_Valor();
            this.VlrLibranza = new UDT_Valor();
            this.VlrVenta = new UDT_Valor();
            this.FechaCuota1 = new UDTSQL_smalldatetime(); 
            #endregion
            #region coTecero
            this.Apellido1 = new UDT_DescripTBase();
            this.Apellido2 = new UDT_DescripTBase();
            this.Nombre1 = new UDT_DescripTBase();
            this.Nombre2 = new UDT_DescripTBase();
            this.Ciudad = new UDT_BasicID();
            this.RegFiscal = new UDT_BasicID();
            this.ActEconomicaID = new UDT_BasicID();
            this.TipoDocumento = new UDT_BasicID();
            this.Telefono = new UDTSQL_char(20);
            this.Direccion = new UDT_DescripTBase();
            this.CorreoElectronico = new UDTSQL_char(60);
            this.AutoRetenedorIVAInd = new UDT_SiNo();
            this.AutoRetenedorInd = new UDT_SiNo();
            this.DeclaraIVAInd = new UDT_SiNo();
            this.DeclaraRentaInd = new UDT_SiNo();
            this.RadicaDirectoInd = new UDT_SiNo();
            this.IndependienteEMPInd = new UDT_SiNo();
            this.ExcluyeCREEInd = new UDT_SiNo(); 
            #endregion
            #region ccCliente
            this.FechaExpDoc = new UDTSQL_smalldatetime();
            this.FechaNacimiento = new UDTSQL_smalldatetime();
            this.NacimientoCiudad = new UDT_LugarGeograficoID();
            this.Sexo = new UDTSQL_tinyint();
            this.EstadoCivil = new UDTSQL_tinyint();
            this.ResidenciaDir = new UDTSQL_char(50);
            this.ResidenciaTipo = new UDTSQL_tinyint();
            this.ZonaID = new UDT_ZonaID();
            this.LaboralDireccion = new UDTSQL_char(50);
            this.Cargo = new UDTSQL_char(25);
            this.ProfesionID = new UDT_ProfesionID();
            this.LaboralEntidad = new UDTSQL_char(50); 
            this.Antiguedad = new UDTSQL_tinyint(); 
            this.ClienteTipo = new UDTSQL_tinyint();
            this.Estrato = new UDTSQL_tinyint();
            this.EscolaridadNivel = new UDTSQL_tinyint();
            this.JornadaLaboral = new UDTSQL_tinyint();
            this.Ocupacion = new UDTSQL_tinyint();
            this.AsesorID = new UDT_AsesorID();
            this.VlrDevengado = new UDT_Valor();
            this.VlrDeducido = new UDT_Valor();
            this.VlrActivos = new UDT_Valor();
            this.VlrPasivos = new UDT_Valor();
            this.VlrMesada = new UDT_Valor();
            this.VlrConsultado = new UDT_Valor();
            this.VlrOpera = new UDT_Valor();
            this.FechaIngreso = new UDTSQL_smalldatetime();
            #endregion
        }

        #endregion

        #region Propiedades

        #region Propiedades Venta Cartera
        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_DescripTBase Nombre { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorCartera { get; set; }

        [DataMember]
        public UDTSQL_int CuotasVendidas { get; set; }

        [DataMember]
        public UDT_Valor TasaVenta { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDT_Valor VlrVenta { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCuota1 { get; set; }
        #endregion

        #region Propiedades coTercero

        [DataMember]
        public UDT_DescripTBase Apellido1 { get; set; }

        [DataMember]
        public UDT_DescripTBase Apellido2 { get; set; }

        [DataMember]
        public UDT_DescripTBase Nombre1 { get; set; }

        [DataMember]
        public UDT_DescripTBase Nombre2 { get; set; }

        [DataMember]
        public UDT_BasicID Ciudad { get; set; }

        [DataMember]
        public UDT_BasicID RegFiscal { get; set; }

        [DataMember]
        public UDT_BasicID ActEconomicaID { get; set; }

        [DataMember]
        public UDT_BasicID TipoDocumento { get; set; }

        [DataMember]
        public UDT_DescripTBase Direccion { get; set; }

        [DataMember]
        public UDTSQL_char Telefono { get; set; }

        [DataMember]
        public UDTSQL_char CorreoElectronico { get; set; }

        [DataMember]
        public UDT_SiNo AutoRetenedorInd { get; set; }

        [DataMember]
        public UDT_SiNo AutoRetenedorIVAInd { get; set; }

        [DataMember]
        public UDT_SiNo DeclaraIVAInd { get; set; }

        [DataMember]
        public UDT_SiNo DeclaraRentaInd { get; set; }

        [DataMember]
        public UDT_SiNo ExcluyeCREEInd { get; set; }

        [DataMember]
        public UDT_SiNo IndependienteEMPInd { get; set; }

        [DataMember]
        public UDT_SiNo RadicaDirectoInd { get; set; }
        #endregion

        #region Propiedades ccCliente

        [DataMember]
        public UDTSQL_smalldatetime FechaExpDoc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaNacimiento { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID NacimientoCiudad { get; set; }

        [DataMember]
        public UDTSQL_tinyint Sexo { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoCivil { get; set; }

        [DataMember]
        public UDTSQL_char ResidenciaDir { get; set; }

        [DataMember]
        public UDTSQL_tinyint ResidenciaTipo { get; set; }

        [DataMember]
        public UDT_ZonaID ZonaID { get; set; }

        [DataMember]
        public UDTSQL_char LaboralDireccion { get; set; }

        [DataMember]
        public UDTSQL_char Cargo { get; set; }

        [DataMember]
        public UDT_ProfesionID ProfesionID { get; set; }

        [DataMember]
        public UDTSQL_char LaboralEntidad { get; set; }

        [DataMember]
        public UDTSQL_tinyint Antiguedad { get; set; }

        [DataMember]
        public UDTSQL_tinyint ClienteTipo { get; set; }

        [DataMember]
        public UDTSQL_tinyint Estrato { get; set; }

        [DataMember]
        public UDTSQL_tinyint EscolaridadNivel { get; set; }

        [DataMember]
        public UDTSQL_tinyint JornadaLaboral { get; set; }

        [DataMember]
        public UDTSQL_tinyint Ocupacion { get; set; }

        [DataMember]
        public UDT_AsesorID AsesorID { get; set; }

        [DataMember]
        public UDT_Valor VlrDevengado { get; set; }

        [DataMember]
        public UDT_Valor VlrDeducido { get; set; }

        [DataMember]
        public UDT_Valor VlrActivos { get; set; }

        [DataMember]
        public UDT_Valor VlrPasivos { get; set; }

        [DataMember]
        public UDT_Valor VlrMesada { get; set; }

        [DataMember]
        public UDT_Valor VlrConsultado { get; set; }

        [DataMember]
        public UDT_Valor VlrOpera { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaIngreso { get; set; } 
        #endregion

        #endregion

    }
}
