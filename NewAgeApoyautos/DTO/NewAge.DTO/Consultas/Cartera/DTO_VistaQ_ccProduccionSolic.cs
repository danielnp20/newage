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
    public class DTO_VistaQ_ccProduccionSolic
    {
        #region DTO_ccCreditoLiquida

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        public DTO_VistaQ_ccProduccionSolic(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.CooperativaID.Value = Convert.ToString(dr["CooperativaID"]);
                this.CodAsesor.Value = Convert.ToString(dr["CodAsesor"]);
                this.NombreAsesor.Value = Convert.ToString(dr["NombreAsesor"]);
                this.ProfesionID.Value = Convert.ToString(dr["ProfesionID"]);
                this.Libranza.Value = Convert.ToString(dr["Libranza"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaSol"].ToString()))
                    this.FechaSol.Value = Convert.ToDateTime(dr["FechaSol"]);
                this.Cedula.Value = Convert.ToString(dr["Cedula"]);
                this.NomCliente.Value = Convert.ToString(dr["NomCliente"]);
                this.DireccionRes.Value = Convert.ToString(dr["DireccionRes"]);
                this.TelefonoRes.Value = Convert.ToString(dr["TelefonoRes"]);
                this.CiudadRes.Value = Convert.ToString(dr["CiudadRes"]);
                this.DepartamentoRes.Value = Convert.ToString(dr["DepartamentoRes"]);
                this.Celular.Value = Convert.ToString(dr["Celular"]);
                this.Correo.Value = Convert.ToString(dr["Correo"]);
                if (!string.IsNullOrWhiteSpace(dr["PensionadoInd"].ToString()))
                    this.PensionadoInd.Value = Convert.ToBoolean(dr["PensionadoInd"]);
                this.EmpleadoCodigo.Value = Convert.ToString(dr["EmpleadoCodigo"]);
                this.BancoID.Value = Convert.ToString(dr["BancoID"]);
                this.NomBanco.Value = Convert.ToString(dr["NomBanco"]);
                this.NroCtaBanco.Value = Convert.ToString(dr["NroCtaBanco"]);
                this.LineaCredito.Value = Convert.ToString(dr["LineaCredito"]);
                this.NomLinea.Value = Convert.ToString(dr["NomLinea"]);
                this.TipoCredito.Value = Convert.ToString(dr["TipoCredito"]);
                this.Estado.Value = Convert.ToString(dr["Estado"]);
                this.Usuario.Value = Convert.ToString(dr["Usuario"]);
                if (!string.IsNullOrWhiteSpace(dr["Plazo"].ToString()))
                    this.Plazo.Value = Convert.ToInt16(dr["Plazo"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuota"].ToString()))
                    this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSolicitado"].ToString()))
                    this.VlrSolicitado.Value = Convert.ToDecimal(dr["VlrSolicitado"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrLibranza"].ToString()))
                    this.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrAdicional"].ToString()))
                    this.VlrAdicional.Value = Convert.ToDecimal(dr["VlrAdicional"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCredito"].ToString()))
                    this.VlrCredito.Value = Convert.ToDecimal(dr["VlrCredito"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCompra"].ToString()))
                    this.VlrCompra.Value = Convert.ToDecimal(dr["VlrCompra"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrDescuento"].ToString()))
                    this.VlrDescuento.Value = Convert.ToDecimal(dr["VlrDescuento"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrLibranza"].ToString()))
                    this.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrGiro"].ToString()))
                    this.VlrGiro.Value = Convert.ToDecimal(dr["VlrGiro"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrDevengado"].ToString()))
                    this.VlrDevengado.Value = Convert.ToDecimal(dr["VlrDevengado"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrDeducido"].ToString()))
                    this.VlrDeducido.Value = Convert.ToDecimal(dr["VlrDeducido"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrConsultado"].ToString()))
                    this.VlrConsultado.Value = Convert.ToDecimal(dr["VlrConsultado"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrMesada"].ToString()))
                    this.VlrMesada.Value = Convert.ToDecimal(dr["VlrMesada"]);
                this.Codeudor1.Value = Convert.ToString(dr["Codeudor1"]);
                this.Codeudor2.Value = Convert.ToString(dr["Codeudor2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaCorte"].ToString()))
                    this.FechaCorte.Value = Convert.ToDateTime(dr["FechaCorte"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasPendientes"].ToString()))
                    this.DiasPendientes.Value = Convert.ToInt32(dr["DiasPendientes"]);
                this.PagadID.Value = Convert.ToString(dr["PagadID"]);
                this.PagadNombre.Value = Convert.ToString(dr["PagadNombre"]);
                this.Zona.Value = Convert.ToString(dr["Zona"]);
                this.NomZona.Value = Convert.ToString(dr["NomZona"]);
                this.CentroCosto.Value = Convert.ToString(dr["CentroCosto"]);
                this.NomCentroCosto.Value = Convert.ToString(dr["NomCentroCosto"]);   
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        #region Inicializar Columnas

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_VistaQ_ccProduccionSolic()
        {
            this.InitCols();
        } 

        #endregion

        #endregion

        #region Inicializa las columnas

        /// <summary>
        /// Inicializa Columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.CooperativaID = new UDT_CodigoGrl5();
            this.CodAsesor = new UDT_AsesorID();
            this.NombreAsesor = new UDT_DescripTBase();
            this.ProfesionID = new UDT_ProfesionID();
            this.Libranza = new UDT_DocTerceroID();
            this.FechaSol = new UDTSQL_smalldatetime();
            this.Cedula = new UDT_TerceroID();
            this.NomCliente = new UDT_Descriptivo();
            this.DireccionRes = new UDT_DescripTBase();
            this.TelefonoRes = new UDTSQL_char(30);
            this.CiudadRes = new UDT_LugarGeograficoID();
            this.DepartamentoRes = new UDT_LugarGeograficoID();
            this.Celular = new UDTSQL_char(20);
            this.Correo = new UDTSQL_char(60);
            this.PensionadoInd = new UDT_SiNo();
            this.EmpleadoCodigo = new UDTSQL_char(20);
            this.BancoID = new UDT_BancoID();
            this.NomBanco = new UDT_Descriptivo();
            this.NroCtaBanco = new UDTSQL_char(15);
            this.LineaCredito = new UDT_LineaCreditoID();
            this.NomLinea = new UDT_Descriptivo();
            this.TipoCredito = new UDT_CodigoGrl5();
            this.Estado = new UDTSQL_char(2);
            this.Usuario = new UDT_UsuarioID();
            this.Plazo = new UDTSQL_smallint();
            this.VlrCuota = new UDT_Valor();
            this.VlrSolicitado = new UDT_Valor();
            this.VlrLibranza = new UDT_Valor();
            this.VlrAdicional = new UDT_Valor();
            this.VlrCredito = new UDT_Valor();
            this.VlrCompra = new UDT_Valor();
            this.VlrDescuento = new UDT_Valor();
            this.VlrLibranza = new UDT_Valor();
            this.VlrGiro = new UDT_Valor();
            this.VlrDevengado = new UDT_Valor();
            this.VlrDeducido = new UDT_Valor();
            this.VlrConsultado = new UDT_Valor();
            this.VlrMesada = new UDT_Valor();
            this.Codeudor1 = new UDT_ClienteID();
            this.Codeudor2 = new UDT_ClienteID();
            this.FechaCorte = new UDTSQL_smalldatetime();
            this.DiasPendientes = new UDTSQL_int();
            this.PagadID = new UDT_PagaduriaID();
            this.PagadNombre = new UDT_Descriptivo();
            this.Zona = new UDT_ZonaID();
            this.NomZona = new UDT_Descriptivo();
            this.CentroCosto = new UDT_CentroCostoID();
            this.NomCentroCosto = new UDT_Descriptivo();         
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 CooperativaID { get; set; }

        [DataMember]
        public UDT_AsesorID CodAsesor { get; set; }

        [DataMember]
        public UDT_DescripTBase NombreAsesor { get; set; }

        [DataMember]
        public UDT_ProfesionID ProfesionID { get; set; }

        [DataMember]
        public UDT_DocTerceroID Libranza { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaSol { get; set; }

        [DataMember]
        public UDT_TerceroID Cedula { get; set; }

        [DataMember]
        public UDT_Descriptivo NomCliente { get; set; }

        [DataMember]
        public UDT_DescripTBase DireccionRes { get; set; }

        [DataMember]
        public UDTSQL_char TelefonoRes { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID CiudadRes { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID DepartamentoRes { get; set; }

        [DataMember]
        public UDTSQL_char Celular { get; set; }

        [DataMember]
        public UDTSQL_char Correo { get; set; }

        [DataMember]
        public UDT_SiNo PensionadoInd { get; set; }

        [DataMember]
        public UDTSQL_char EmpleadoCodigo { get; set; }

        [DataMember]
        public UDT_BancoID BancoID { get; set; }

        [DataMember]
        public UDT_Descriptivo NomBanco { get; set; }

        [DataMember]
        public UDTSQL_char NroCtaBanco { get; set; }

        [DataMember]
        public UDT_LineaCreditoID LineaCredito { get; set; }

        [DataMember]
        public UDT_Descriptivo NomLinea { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 TipoCredito { get; set; }

        [DataMember]
        public UDTSQL_char Estado { get; set; }

        [DataMember]
        public UDT_UsuarioID Usuario { get; set; }

        [DataMember]
        public UDTSQL_smallint Plazo { get; set; }
        
        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrSolicitado { get; set; }

        [DataMember]
        public UDT_Valor VlrAdicional { get; set; }

        [DataMember]
        public UDT_Valor VlrCredito { get; set; }

        [DataMember]
        public UDT_Valor VlrCompra { get; set; }

        [DataMember]
        public UDT_Valor VlrDescuento { get; set; }

         [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

         [DataMember]
        public UDT_Valor VlrGiro { get; set; }

         [DataMember]
        public UDT_Valor VlrDevengado { get; set; }

        [DataMember]
        public UDT_Valor VlrDeducido { get; set; }

        [DataMember]
        public UDT_Valor VlrConsultado { get; set; }

        [DataMember]
        public UDT_Valor VlrMesada { get; set; }

        [DataMember]
        public UDT_ClienteID Codeudor1 { get; set; }

        [DataMember]
        public UDT_ClienteID Codeudor2 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCorte { get; set; }

        [DataMember]
        public UDTSQL_int DiasPendientes { get; set; }

        [DataMember]
        public UDT_PagaduriaID PagadID { get; set; }

        [DataMember]
        public UDT_Descriptivo PagadNombre { get; set; }

        [DataMember]
        public UDT_ZonaID Zona { get; set; }

        [DataMember]
        public UDT_Descriptivo NomZona { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCosto { get; set; }

        [DataMember]
        public UDT_Descriptivo NomCentroCosto { get; set; }

        #endregion

    }
}
