using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_ccCliente
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCliente : DTO_MasterBasic
    {
        #region DTO_ccCliente
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCliente(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.CiudadExpDocDesc.Value = dr["CiudadExpDocDesc"].ToString();
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                    this.NacimientoCiudadDesc.Value = dr["NacimientoCiudadDesc"].ToString();
                    this.ResidenciaCiudadDesc.Value = dr["ResidenciaCiudadDesc"].ToString();
                    this.ZonaDesc.Value = dr["ZonaDesc"].ToString();
                    this.Banco1Desc.Value = dr["Banco1Desc"].ToString();//
                    this.AsesorDesc.Value = dr["AsesorDesc"].ToString();
                    this.LaboralCiudadDesc.Value = dr["LaboralCiudadDesc"].ToString();
                    this.ProfesionDesc.Value = dr["ProfesionDesc"].ToString();
                    this.AbogadoDesc.Value = dr["AbogadoDesc"].ToString();
                    this.CobranzaEstadoDesc.Value = dr["CobranzaEstadoDesc"].ToString();
                    this.CobranzaGestionDesc.Value = dr["CobranzaGestionDesc"].ToString();
                    this.PagaduriaDesc.Value = dr["PagaduriaDesc"].ToString();

                }

                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.FechaExpDoc.Value = Convert.ToDateTime(dr["FechaExpDoc"]);
                this.CiudadExpDoc.Value = dr["CiudadExpDoc"].ToString();
                this.FechaNacimiento.Value = Convert.ToDateTime(dr["FechaNacimiento"]);
                this.NacimientoCiudad.Value = dr["NacimientoCiudad"].ToString();
                this.ResidenciaCiudad.Value = dr["ResidenciaCiudad"].ToString();
                this.Sexo.Value = Convert.ToByte(dr["Sexo"]);
                this.EstadoCivil.Value = Convert.ToByte(dr["EstadoCivil"]);
                this.EstadoOtro.Value = dr["EstadoOtro"].ToString();
                this.ResidenciaDir.Value = dr["ResidenciaDir"].ToString();
                this.ResidenciaTipo.Value = Convert.ToByte(dr["ResidenciaTipo"]);
                this.Barrio.Value = dr["Barrio"].ToString();
                this.Telefono.Value = dr["Telefono"].ToString();
                this.Celular.Value = dr["Celular"].ToString();
                this.ZonaID.Value = dr["ZonaID"].ToString();
                this.ProfesionID.Value = dr["ProfesionID"].ToString();
                this.LaboralDireccion.Value = dr["LaboralDireccion"].ToString();
                this.LaboralCiudad.Value = dr["LaboralCiudad"].ToString();
                this.TelefonoTrabajo.Value = dr["TelefonoTrabajo"].ToString();
                this.Correo.Value = dr["Correo"].ToString();
                this.Cargo.Value = dr["Cargo"].ToString();
                this.EmpleadoCodigo.Value = dr["EmpleadoCodigo"].ToString();
                this.LaboralEntidad.Value = dr["LaboralEntidad"].ToString();
                this.Antiguedad.Value = Convert.ToByte(dr["Antiguedad"]);
                this.BancoID_1.Value = dr["BancoID_1"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CuentaTipo_1"].ToString()))
                    this.CuentaTipo_1.Value = Convert.ToByte(dr["CuentaTipo_1"]);
                if (!string.IsNullOrWhiteSpace(dr["BcoCtaNro_1"].ToString()))
                    this.BcoCtaNro_1.Value = dr["BcoCtaNro_1"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ClienteTipo"].ToString()))
                    this.ClienteTipo.Value = Convert.ToByte(dr["ClienteTipo"]);
                if (!string.IsNullOrWhiteSpace(dr["PensionadoInd"].ToString()))
                    this.PensionadoInd.Value = Convert.ToBoolean(dr["PensionadoInd"]);
                if (!string.IsNullOrWhiteSpace(dr["Estrato"].ToString()))
                    this.Estrato.Value = Convert.ToByte(dr["Estrato"]);
                if (!string.IsNullOrWhiteSpace(dr["EscolaridadNivel"].ToString()))
                    this.EscolaridadNivel.Value = Convert.ToByte(dr["EscolaridadNivel"]);
                if (!string.IsNullOrWhiteSpace(dr["MujerInd"].ToString()))
                    this.MujerInd.Value = Convert.ToBoolean(dr["MujerInd"]);
                if (!string.IsNullOrEmpty(dr["JornadaLaboral"].ToString()))
                    this.JornadaLaboral.Value = Convert.ToByte(dr["JornadaLaboral"]);
                if (!string.IsNullOrEmpty(dr["EmpleadoInd"].ToString()))
                    this.EmpleadoInd.Value = Convert.ToBoolean(dr["EmpleadoInd"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoContrato"].ToString()))
                    this.TipoContrato.Value = Convert.ToString(dr["TipoContrato"]);
                if (!string.IsNullOrWhiteSpace(dr["Ocupacion"].ToString()))
                    this.Ocupacion.Value = Convert.ToByte(dr["Ocupacion"]);
                this.AsesorID.Value = dr["AsesorID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["VlrDeducido"].ToString()))
                    this.VlrDeducido.Value = Convert.ToDecimal(dr["VlrDeducido"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrDevengado"].ToString()))
                    this.VlrDevengado.Value = Convert.ToDecimal(dr["VlrDevengado"]);
                if (!string.IsNullOrEmpty(dr["VlrOtrosIng"].ToString()))
                    this.VlrOtrosIng.Value = Convert.ToDecimal(dr["VlrOtrosIng"]);
                this.DescrOtrosIng.Value = dr["DescrOtrosIng"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["VlrActivos"].ToString()))
                    this.VlrActivos.Value = Convert.ToDecimal(dr["VlrActivos"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPasivos"].ToString()))
                    this.VlrPasivos.Value = Convert.ToDecimal(dr["VlrPasivos"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrMesada"].ToString()))
                    this.VlrMesada.Value = Convert.ToDecimal(dr["VlrMesada"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrConsultado"].ToString()))
                    this.VlrConsultado.Value = Convert.ToDecimal(dr["VlrConsultado"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrOpera"].ToString()))
                    this.VlrOpera.Value = Convert.ToDecimal(dr["VlrOpera"]);
                if (!string.IsNullOrEmpty(dr["UltimaAsambleaInd"].ToString()))
                    this.UltimaAsambleaInd.Value = Convert.ToBoolean(dr["UltimaAsambleaInd"]);
                if (!string.IsNullOrEmpty(dr["BloqueoPorReliquidacion"].ToString()))
                    this.BloqueoPorReliquidacion.Value = Convert.ToBoolean(dr["BloqueoPorReliquidacion"]);
                if (!string.IsNullOrEmpty(dr["FechaIngreso"].ToString()))
                    this.FechaIngreso.Value = Convert.ToDateTime(dr["FechaIngreso"]);
                if (!string.IsNullOrEmpty(dr["FechaIngresoPAG"].ToString()))
                    this.FechaIngresoPAG.Value = Convert.ToDateTime(dr["FechaIngresoPAG"]);
                if (!string.IsNullOrEmpty(dr["FechaRetiro"].ToString()))
                    this.FechaRetiro.Value = Convert.ToDateTime(dr["FechaRetiro"]);
                if (!string.IsNullOrEmpty(dr["EstadoCartera"].ToString()))
                    this.EstadoCartera.Value = Convert.ToByte(dr["EstadoCartera"]);
                if (!string.IsNullOrEmpty(dr["FechaINIEstado"].ToString()))
                    this.FechaINIEstado.Value = Convert.ToDateTime(dr["FechaINIEstado"]);
                this.AbogadoID.Value = Convert.ToString(dr["AbogadoID"]);
                this.CobranzaEstadoID.Value = Convert.ToString(dr["CobranzaEstadoID"]);
                this.CobranzaGestionID.Value = Convert.ToString(dr["CobranzaGestionID"]);
                if (!string.IsNullOrEmpty(dr["DocumCobranza"].ToString()))
                    this.DocumCobranza.Value = Convert.ToInt32(dr["DocumCobranza"]);
                if (!string.IsNullOrEmpty(dr["ConsIncumplimiento"].ToString()))
                    this.ConsIncumplimiento.Value = Convert.ToInt32(dr["ConsIncumplimiento"]);
                if (!string.IsNullOrEmpty(dr["NumDocVencido"].ToString()))
                    this.NumDocVencido.Value = Convert.ToInt32(dr["NumDocVencido"]);
                if (!string.IsNullOrEmpty(dr["CedEsposa"].ToString()))
                    this.CedEsposa.Value = Convert.ToString(dr["CedEsposa"]);
                if (!string.IsNullOrEmpty(dr["NomEsposa"].ToString()))
                    this.NomEsposa.Value = Convert.ToString(dr["NomEsposa"]);
                if (!string.IsNullOrEmpty(dr["FechEsposa"].ToString()))
                    this.FechEsposa.Value = Convert.ToDateTime(dr["FechEsposa"]);
                this.Acierta.Value = Convert.ToString(dr["Acierta"]);
                this.AciertaCifin.Value = Convert.ToString(dr["AciertaCifin"]);
                this.Telefono1.Value = dr["Telefono1"].ToString();
                this.Telefono2.Value = dr["Telefono2"].ToString();
                this.Extension1.Value = dr["Extension1"].ToString();
                this.Extension2.Value = dr["Extension2"].ToString();
                this.Celular1.Value = dr["Celular1"].ToString();
                this.Celular2.Value = dr["Celular2"].ToString();
                this.ApellidoPri.Value = dr["ApellidoPri"].ToString();
                this.ApellidoSdo.Value = dr["ApellidoSdo"].ToString();
                this.NombrePri.Value = dr["NombrePri"].ToString();
                this.NombreSdo.Value = dr["NombreSdo"].ToString();
                this.PagaduriaID.Value = dr["PagaduriaID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCliente() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroID = new UDT_BasicID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.FechaExpDoc = new UDTSQL_smalldatetime();
            this.CiudadExpDoc = new UDT_BasicID();
            this.CiudadExpDocDesc = new UDT_Descriptivo();
            this.FechaNacimiento = new UDTSQL_smalldatetime();
            this.NacimientoCiudad = new UDT_BasicID();
            this.NacimientoCiudadDesc = new UDT_DescripTBase();
            this.Sexo = new UDTSQL_tinyint();
            this.EstadoCivil = new UDTSQL_tinyint();
            this.EstadoOtro = new UDTSQL_char(20);
            this.ResidenciaDir = new UDT_DescripTBase();
            this.ResidenciaTipo = new UDTSQL_tinyint();
            this.ResidenciaCiudad = new UDT_BasicID();
            this.ResidenciaCiudadDesc = new UDT_DescripTBase();
            this.Barrio = new UDTSQL_char(25);
            this.Telefono = new UDTSQL_char(50);
            this.Celular = new UDTSQL_char(50);
            this.ZonaID = new UDT_BasicID();
            this.ZonaDesc = new UDT_Descriptivo();
            this.LaboralDireccion = new UDT_DescripTBase();
            this.LaboralCiudad = new UDT_BasicID();
            this.LaboralCiudadDesc = new UDT_Descriptivo();
            this.TelefonoTrabajo = new UDTSQL_char(50);
            this.Correo = new UDTSQL_char(100);
            this.Cargo = new UDTSQL_char(50);
            this.EmpleadoCodigo = new UDTSQL_char(20);
            this.ProfesionID = new UDT_BasicID();
            this.ProfesionDesc = new UDT_Descriptivo();
            this.LaboralEntidad = new UDTSQL_char(60);
            this.Antiguedad = new UDTSQL_tinyint();

            this.BancoID_1 = new UDT_BasicID();
            this.Banco1Desc = new UDT_Descriptivo();
            this.CuentaTipo_1 = new UDTSQL_tinyint();
            this.BcoCtaNro_1 = new UDTSQL_varchar(15);
            this.ClienteTipo = new UDTSQL_tinyint();
            this.PensionadoInd = new UDT_SiNo();
            this.Estrato = new UDTSQL_tinyint();
            this.EscolaridadNivel = new UDTSQL_tinyint();
            this.MujerInd = new UDT_SiNo();
            this.SectorEconomico = new UDTSQL_char(20);
            this.JornadaLaboral = new UDTSQL_tinyint();
            this.EmpleadoInd = new UDT_SiNo();
            this.TipoContrato = new UDTSQL_char(20);
            this.Ocupacion = new UDTSQL_tinyint();
            this.AsesorID = new UDT_BasicID();
            this.AsesorDesc = new UDT_Descriptivo();
            this.VlrDevengado = new UDT_Valor();
            this.VlrDeducido = new UDT_Valor();
            this.VlrOtrosIng = new UDT_Valor();
            this.DescrOtrosIng = new UDTSQL_char(30);
            this.VlrActivos = new UDT_Valor();
            this.VlrPasivos = new UDT_Valor();
            this.VlrMesada = new UDT_Valor();
            this.VlrConsultado = new UDT_Valor();
            this.VlrOpera = new UDT_Valor();
            this.FechaIngresoPAG = new UDTSQL_smalldatetime();
            this.UltimaAsambleaInd = new UDT_SiNo();
            this.BloqueoPorReliquidacion = new UDT_SiNo();
            this.FechaIngreso = new UDTSQL_smalldatetime();
            this.FechaRetiro = new UDTSQL_smalldatetime();
            this.EstadoCartera = new UDTSQL_tinyint();
            this.FechaINIEstado = new UDTSQL_smalldatetime();
            this.AbogadoID = new UDT_BasicID();
            this.AbogadoDesc = new UDT_Descriptivo();
            this.CobranzaEstadoID = new UDT_BasicID();
            this.CobranzaEstadoDesc = new UDT_Descriptivo();
            this.CobranzaGestionID = new UDT_BasicID();
            this.CobranzaGestionDesc = new UDT_Descriptivo();
            this.DocumCobranza = new UDT_Consecutivo();
            this.ConsIncumplimiento = new UDT_Consecutivo();
            this.NumDocVencido = new UDT_Consecutivo();
            this.NumDocCJ = new UDT_Consecutivo();
            this.CedEsposa = new UDTSQL_char(15);
            this.NomEsposa = new UDTSQL_char(100);
            this.FechEsposa = new UDTSQL_smalldatetime();
            this.Acierta = new UDTSQL_char(10);
            this.AciertaCifin = new UDTSQL_char(10);
            this.Telefono1 = new UDTSQL_char(15);
            this.Telefono2 = new UDTSQL_char(15);
            this.Extension1 = new UDTSQL_char(15);
            this.Extension2 = new UDTSQL_char(15);
            this.Celular1 = new UDTSQL_char(15);
            this.Celular2 = new UDTSQL_char(15);
            this.ApellidoPri = new UDT_DescripTBase();
            this.ApellidoSdo = new UDT_DescripTBase();
            this.NombrePri = new UDT_DescripTBase();
            this.NombreSdo = new UDT_DescripTBase();
            this.PagaduriaID = new UDT_BasicID();
            this.PagaduriaDesc = new UDT_Descriptivo();
        }

        public DTO_ccCliente(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccCliente(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaExpDoc { get; set; }

        [DataMember]
        public UDT_BasicID CiudadExpDoc { get; set; }

        [DataMember]
        public UDT_Descriptivo CiudadExpDocDesc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaNacimiento { get; set; }

        [DataMember]
        public UDT_BasicID NacimientoCiudad { get; set; }

        [DataMember]
        public UDT_DescripTBase NacimientoCiudadDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint Sexo { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoCivil { get; set; }

        [DataMember]
        public UDTSQL_char EstadoOtro { get; set; }

        [DataMember]
        public UDT_DescripTBase ResidenciaDir { get; set; }

        [DataMember]
        public UDTSQL_tinyint ResidenciaTipo { get; set; }

        [DataMember]
        public UDT_BasicID ResidenciaCiudad { get; set; }

        [DataMember]
        public UDT_DescripTBase ResidenciaCiudadDesc { get; set; }

        [DataMember]
        public UDTSQL_char Barrio { get; set; }

        [DataMember]
        public UDTSQL_char Telefono { get; set; }

        [DataMember]
        public UDTSQL_char Celular { get; set; }

        [DataMember]
        public UDT_BasicID ZonaID { get; set; }

        [DataMember]
        public UDT_Descriptivo ZonaDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase LaboralDireccion { get; set; }

        [DataMember]
        public UDT_BasicID LaboralCiudad { get; set; }

        [DataMember]
        public UDT_Descriptivo LaboralCiudadDesc { get; set; }

        [DataMember]
        public UDTSQL_char TelefonoTrabajo { get; set; }

        [DataMember]
        public UDTSQL_char Correo { get; set; }

        [DataMember]
        public UDTSQL_char Cargo { get; set; }

        [DataMember]
        public UDTSQL_char EmpleadoCodigo { get; set; }

        [DataMember]
        public UDT_BasicID ProfesionID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProfesionDesc { get; set; }

        [DataMember]
        public UDTSQL_char LaboralEntidad { get; set; }

        [DataMember]
        public UDTSQL_tinyint Antiguedad { get; set; }

        [DataMember]
        public UDT_BasicID BancoID_1 { get; set; }

        [DataMember]
        public UDT_Descriptivo Banco1Desc { get; set; }

        [DataMember]
        public UDTSQL_tinyint CuentaTipo_1 { get; set; }

        [DataMember]
        public UDTSQL_varchar BcoCtaNro_1 { get; set; }

        [DataMember]
        public UDTSQL_tinyint ClienteTipo { get; set; }

        [DataMember]
        public UDT_SiNo PensionadoInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint Estrato { get; set; }

        [DataMember]
        public UDTSQL_tinyint EscolaridadNivel { get; set; }

        [DataMember]
        public UDT_SiNo MujerInd { get; set; }

        [DataMember]
        public UDTSQL_char SectorEconomico { get; set; }

        [DataMember]
        public UDTSQL_tinyint JornadaLaboral { get; set; }

        [DataMember]
        public UDT_SiNo EmpleadoInd { get; set; }

        [DataMember]
        public UDTSQL_char TipoContrato { get; set; }

        [DataMember]
        public UDTSQL_tinyint Ocupacion { get; set; }

        [DataMember]
        public UDT_BasicID AsesorID { get; set; }

        [DataMember]
        public UDT_Descriptivo AsesorDesc { get; set; }

        [DataMember]
        public UDT_Valor VlrDevengado { get; set; }

        [DataMember]
        public UDT_Valor VlrDeducido { get; set; }

        [DataMember]
        public UDT_Valor VlrOtrosIng { get; set; }

        [DataMember]
        public UDTSQL_char DescrOtrosIng { get; set; }

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
        public UDT_SiNo UltimaAsambleaInd { get; set; }

        [DataMember]
        public UDT_SiNo BloqueoPorReliquidacion { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaIngreso { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaIngresoPAG { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaRetiro { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoCartera { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaINIEstado { get; set; }

        //[DataMember]
        //public UDT_Valor PuntosTOT { get; set; }

        [DataMember]
        public UDT_BasicID AbogadoID { get; set; }

        [DataMember]
        public UDT_Descriptivo AbogadoDesc { get; set; }
        //
        [DataMember]
        public UDT_BasicID CobranzaEstadoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CobranzaEstadoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CobranzaGestionID { get; set; }

        [DataMember]
        public UDT_Descriptivo CobranzaGestionDesc { get; set; }
        //
        [DataMember]
        public UDT_Consecutivo DocumCobranza { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsIncumplimiento { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocVencido { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocCJ { get; set; }

        [DataMember]
        public UDTSQL_char CedEsposa { get; set; }

        [DataMember]
        public UDTSQL_char NomEsposa { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechEsposa { get; set; }

        [DataMember]
        public UDTSQL_char Acierta { get; set; }

        [DataMember]
        public UDTSQL_char AciertaCifin { get; set; }

        [DataMember]
        public UDTSQL_char Telefono1 { get; set; }

        [DataMember]
        public UDTSQL_char Extension1 { get; set; }

        [DataMember]
        public UDTSQL_char Telefono2 { get; set; }

        [DataMember]
        public UDTSQL_char Extension2 { get; set; }

        [DataMember]
        public UDTSQL_char Celular1 { get; set; }

        [DataMember]
        public UDTSQL_char Celular2 { get; set; }

        [DataMember]
        public UDT_DescripTBase ApellidoPri { get; set; }

        [DataMember]
        public UDT_DescripTBase ApellidoSdo { get; set; }

        [DataMember]
        public UDT_DescripTBase NombrePri { get; set; }

        [DataMember]
        public UDT_DescripTBase NombreSdo { get; set; }

        [DataMember]
        public UDT_BasicID PagaduriaID { get; set; }

        [DataMember]
        public UDT_Descriptivo PagaduriaDesc { get; set; }


        #endregion
    }

}
