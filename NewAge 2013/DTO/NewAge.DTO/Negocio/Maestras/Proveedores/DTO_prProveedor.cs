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
    /// Models DTO_prProveedor
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prProveedor : DTO_MasterBasic
    {
        #region DTO_prProveedor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prProveedor(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                    if (!string.IsNullOrEmpty(dr["UsuarioDesc"].ToString()))
                        this.UsuarioDesc.Value = dr["UsuarioDesc"].ToString();
                }

                
                this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrEmpty(dr["ServicioDirectoInd"].ToString()))
                    this.ServicioDirectoInd.Value = Convert.ToBoolean(dr["ServicioDirectoInd"]);
                if (!string.IsNullOrEmpty(dr["Contacto"].ToString()))
                    this.Contacto.Value = dr["Contacto"].ToString();
                if (!string.IsNullOrEmpty(dr["Cargo"].ToString()))
                    this.Cargo.Value = dr["Cargo"].ToString();
                if (!string.IsNullOrEmpty(dr["TelContacto"].ToString()))
                    this.TelContacto.Value = dr["TelContacto"].ToString();
                if (!string.IsNullOrEmpty(dr["MailContacto"].ToString()))
                    this.MailContacto.Value = dr["MailContacto"].ToString();
                if (!string.IsNullOrEmpty(dr["ContactoComercial"].ToString()))
                    this.ContactoComercial.Value = dr["ContactoComercial"].ToString();
                if (!string.IsNullOrEmpty(dr["TelContactoComercial"].ToString()))
                    this.TelContactoComercial.Value = dr["TelContactoComercial"].ToString();
                if (!string.IsNullOrEmpty(dr["MailContactoComercial"].ToString()))
                    this.MailContactoComercial.Value = dr["MailContactoComercial"].ToString();
                if (!string.IsNullOrEmpty(dr["ContactoTesoreria"].ToString()))
                    this.ContactoTesoreria.Value = dr["ContactoTesoreria"].ToString();
                if (!string.IsNullOrEmpty(dr["TelContactoTesoreria"].ToString()))
                    this.TelContactoTesoreria.Value = dr["TelContactoTesoreria"].ToString();
                if (!string.IsNullOrEmpty(dr["MailContactoTesoreria"].ToString()))
                    this.MailContactoTesoreria.Value = dr["MailContactoTesoreria"].ToString();
                if (!string.IsNullOrEmpty(dr["RegistroMercantil"].ToString()))
                    this.RegistroMercantil.Value = dr["RegistroMercantil"].ToString();
                if (!string.IsNullOrEmpty(dr["RegistroFecha"].ToString()))
                    this.RegistroFecha.Value = Convert.ToDateTime(dr["RegistroFecha"]);
                if (!string.IsNullOrEmpty(dr["VigenciaFecha"].ToString()))
                    this.VigenciaFecha.Value = Convert.ToDateTime(dr["VigenciaFecha"]);
                if (!string.IsNullOrEmpty(dr["CupoLocal"].ToString()))
                    this.CupoLocal.Value = Convert.ToDecimal(dr["CupoLocal"]);
                if (!string.IsNullOrEmpty(dr["CupoExtra"].ToString()))
                    this.CupoExtra.Value = Convert.ToDecimal(dr["CupoExtra"]);
                if (!string.IsNullOrEmpty(dr["CondicionEspecialPago"].ToString()))
                    this.CondicionEspecialPago.Value = dr["CondicionEspecialPago"].ToString();

                if (!string.IsNullOrEmpty(dr["DtoPtoPagoDias"].ToString()))
                    this.DtoPtoPagoDias.Value = Convert.ToByte(dr["DtoPtoPagoDias"]);
                if (!string.IsNullOrEmpty(dr["PtoPagoPorcentaje"].ToString()))
                    this.PtoPagoPorcentaje.Value = Convert.ToDecimal(dr["PtoPagoPorcentaje"]);
                if (!string.IsNullOrEmpty(dr["PorAnticipo"].ToString()))
                    this.PorAnticipo.Value = Convert.ToDecimal(dr["PorAnticipo"]);
                if (!string.IsNullOrEmpty(dr["PagoContraEntregaInd"].ToString()))
                    this.PagoContraEntregaInd.Value = Convert.ToBoolean(dr["PagoContraEntregaInd"]);

                if (!string.IsNullOrEmpty(dr["CalidadNorma"].ToString()))
                    this.CalidadNorma.Value = dr["CalidadNorma"].ToString();
                if (!string.IsNullOrEmpty(dr["CalidadEntidad"].ToString()))
                    this.CalidadEntidad.Value = dr["CalidadEntidad"].ToString();
                if (!string.IsNullOrEmpty(dr["CalidadVto"].ToString()))
                    this.CalidadVto.Value = Convert.ToDateTime(dr["CalidadVto"]);
                if (!string.IsNullOrEmpty(dr["MedioAmbNormal"].ToString()))
                    this.MedioAmbNormal.Value = dr["MedioAmbNormal"].ToString();
                if (!string.IsNullOrEmpty(dr["MedioAmbEntidad"].ToString()))
                    this.MedioAmbEntidad.Value = dr["MedioAmbEntidad"].ToString();
                if (!string.IsNullOrEmpty(dr["MedioAmbVto"].ToString()))
                    this.MedioAmbVto.Value = Convert.ToDateTime(dr["MedioAmbVto"]);
                if (!string.IsNullOrEmpty(dr["SeguridadNormal"].ToString()))
                    this.SeguridadNormal.Value = dr["SeguridadNormal"].ToString();
                if (!string.IsNullOrEmpty(dr["SeguridadEntidad"].ToString()))
                    this.SeguridadEntidad.Value = dr["SeguridadEntidad"].ToString();
                if (!string.IsNullOrEmpty(dr["SeguridadVto"].ToString()))
                    this.SeguridadVto.Value = Convert.ToDateTime(dr["SeguridadVto"]);
                if (!string.IsNullOrEmpty(dr["NivelRiesgo"].ToString()))
                    this.NivelRiesgo.Value = Convert.ToByte(dr["NivelRiesgo"]);
                if (!string.IsNullOrEmpty(dr["RegistroRUC"].ToString()))
                    this.RegistroRUC.Value = dr["RegistroRUC"].ToString();
                if (!string.IsNullOrEmpty(dr["Puntaje"].ToString()))
                    this.Puntaje.Value = Convert.ToByte(dr["Puntaje"]);
                if (!string.IsNullOrEmpty(dr["CalidadPoliticaInd"].ToString()))
                    this.CalidadPoliticaInd.Value = Convert.ToBoolean(dr["CalidadPoliticaInd"]);
                if (!string.IsNullOrEmpty(dr["MedioAmbPoliticaInd"].ToString()))
                    this.MedioAmbPoliticaInd.Value = Convert.ToBoolean(dr["MedioAmbPoliticaInd"]);
                if (!string.IsNullOrEmpty(dr["SeguridadPoliticaInd"].ToString()))
                    this.SeguridadPoliticaInd.Value = Convert.ToBoolean(dr["SeguridadPoliticaInd"]);
                if (!string.IsNullOrEmpty(dr["AlcoholPoliticaInd"].ToString()))
                    this.AlcoholPoliticaInd.Value = Convert.ToBoolean(dr["AlcoholPoliticaInd"]);
                if (!string.IsNullOrEmpty(dr["DrogasArmasPoliticaInd"].ToString()))
                    this.DrogasArmasPoliticaInd.Value = Convert.ToBoolean(dr["DrogasArmasPoliticaInd"]);
                if (!string.IsNullOrEmpty(dr["Antiguedad"].ToString()))
                    this.Antiguedad.Value = Convert.ToByte(dr["Antiguedad"]);
                if (!string.IsNullOrEmpty(dr["ContratosCant"].ToString()))
                    this.ContratosCant.Value = Convert.ToByte(dr["ContratosCant"]);
                if (!string.IsNullOrEmpty(dr["ProfesionalesTecnicos"].ToString()))
                    this.ProfesionalesTecnicos.Value = Convert.ToByte(dr["ProfesionalesTecnicos"]);
                if (!string.IsNullOrEmpty(dr["ProfesionalesAdmin"].ToString()))
                    this.ProfesionalesAdmin.Value = Convert.ToByte(dr["ProfesionalesAdmin"]);
                if (!string.IsNullOrEmpty(dr["TecnicosOper"].ToString()))
                    this.TecnicosOper.Value = Convert.ToByte(dr["TecnicosOper"]);
                if (!string.IsNullOrEmpty(dr["UsuarioDcum"].ToString()))
                    this.UsuarioDcum.Value = dr["UsuarioDcum"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaDocum"].ToString()))
                    this.FechaDocum.Value = Convert.ToDateTime(dr["FechaDocum"]);
                if (!string.IsNullOrEmpty(dr["TipoProveedor"].ToString()))
                    this.TipoProveedor.Value = Convert.ToByte(dr["TipoProveedor"]);

                if (!string.IsNullOrEmpty(dr["Direccion"].ToString()))
                    this.Direccion.Value = dr["Direccion"].ToString();

                if (!string.IsNullOrEmpty(dr["RepresentanteLegal"].ToString()))
                    this.RepresentanteLegal.Value = dr["RepresentanteLegal"].ToString();
                if (!string.IsNullOrEmpty(dr["ObjetoSocial"].ToString()))
                    this.ObjetoSocial.Value = (dr["ObjetoSocial"].ToString());

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prProveedor()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroID = new UDT_BasicID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.ServicioDirectoInd = new UDT_SiNo();
            this.Contacto = new UDTSQL_char(50);
            this.Cargo = new UDTSQL_char(25);
            this.TelContacto = new UDTSQL_char(25);
            this.MailContacto = new UDT_DescripTBase();
            this.ContactoComercial = new UDTSQL_char(50);
            this.TelContactoComercial = new UDTSQL_char(25);
            this.ContactoTesoreria = new UDTSQL_char(50);
            this.MailContactoComercial = new UDTSQL_char(50);
            this.TelContactoTesoreria = new UDTSQL_char(25);
            this.MailContactoTesoreria = new UDTSQL_char(50);
            this.RegistroMercantil = new UDTSQL_char(25);
            this.RegistroFecha = new UDTSQL_smalldatetime();
            this.VigenciaFecha = new UDTSQL_smalldatetime();
            this.CupoLocal = new UDT_Valor();
            this.CupoExtra = new UDT_Valor();
            this.CondicionEspecialPago = new UDT_DescripTBase();
            this.DtoPtoPagoDias = new UDTSQL_tinyint();
            this.PtoPagoPorcentaje = new UDT_PorcentajeID();
            this.PorAnticipo = new UDT_PorcentajeID();
            this.PagoContraEntregaInd = new UDT_SiNo();
            this.CalidadNorma = new UDTSQL_char(25);
            this.CalidadEntidad = new UDTSQL_char(40);
            this.CalidadVto = new UDTSQL_smalldatetime();
            this.MedioAmbNormal = new UDTSQL_char(25);
            this.MedioAmbEntidad = new UDTSQL_char(40);
            this.MedioAmbVto = new UDTSQL_smalldatetime();
            this.SeguridadNormal = new UDTSQL_char(25);
            this.SeguridadEntidad = new UDTSQL_char(40);
            this.SeguridadVto = new UDTSQL_smalldatetime();
            this.NivelRiesgo = new UDTSQL_tinyint();
            this.RegistroRUC = new UDTSQL_char(25);
            this.Puntaje = new UDTSQL_tinyint();
            this.CalidadPoliticaInd = new UDT_SiNo();
            this.MedioAmbPoliticaInd = new UDT_SiNo();
            this.SeguridadPoliticaInd = new UDT_SiNo();
            this.AlcoholPoliticaInd = new UDT_SiNo();
            this.DrogasArmasPoliticaInd = new UDT_SiNo();
            this.Antiguedad = new UDTSQL_tinyint();
            this.ContratosCant = new UDTSQL_tinyint();
            this.ProfesionalesTecnicos = new UDTSQL_tinyint();
            this.ProfesionalesAdmin = new UDTSQL_tinyint();
            this.TecnicosOper = new UDTSQL_tinyint();
            this.UsuarioDcum = new UDT_BasicID();
            this.UsuarioDesc = new UDT_Descriptivo();
            this.FechaDocum = new UDTSQL_smalldatetime();
            this.TipoProveedor = new UDTSQL_tinyint();
            this.Direccion = new UDT_DescripTBase();
            this.RepresentanteLegal = new UDTSQL_char(50);
            this.ObjetoSocial = new UDTSQL_char(50);

            //Extra
            this.Ciudad = new UDT_LugarGeograficoID();
            this.Web = new UDT_DescripTBase();
            this.Tipo_Identificacion = new UDTSQL_tinyint();
        }

        public DTO_prProveedor(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_prProveedor(DTO_aplMaestraPropiedades masterProperties)
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
        public UDT_SiNo ServicioDirectoInd { get; set; }

        [DataMember]
        public UDTSQL_char Contacto { get; set; }

        [DataMember]
        public UDTSQL_char Cargo { get; set; }

        [DataMember]
        public UDTSQL_char TelContacto { get; set; }

        [DataMember]
        public UDT_DescripTBase MailContacto { get; set; }

        [DataMember]
        public UDTSQL_char ContactoComercial { get; set; }

        [DataMember]
        public UDTSQL_char TelContactoComercial { get; set; }

        [DataMember]
        public UDTSQL_char MailContactoComercial { get; set; }

        [DataMember]
        public UDTSQL_char ContactoTesoreria { get; set; }

        [DataMember]
        public UDTSQL_char TelContactoTesoreria { get; set; }

        [DataMember]
        public UDTSQL_char MailContactoTesoreria { get; set; }

        [DataMember]
        public UDTSQL_char RegistroMercantil { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime RegistroFecha { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime VigenciaFecha { get; set; }

        [DataMember]
        public UDT_Valor CupoLocal { get; set; }

        [DataMember]
        public UDT_Valor CupoExtra { get; set; }

        [DataMember]
        public UDT_DescripTBase CondicionEspecialPago { get; set; }

        [DataMember]
        public UDTSQL_tinyint DtoPtoPagoDias { get; set; }

        [DataMember]
        public UDT_PorcentajeID PtoPagoPorcentaje { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorAnticipo { get; set; }

        [DataMember]
        public UDT_SiNo PagoContraEntregaInd { get; set; }

        [DataMember]
        public UDTSQL_char CalidadNorma { get; set; }

        [DataMember]
        public UDTSQL_char CalidadEntidad { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime CalidadVto { get; set; }

        [DataMember]
        public UDTSQL_char MedioAmbNormal { get; set; }

        [DataMember]
        public UDTSQL_char MedioAmbEntidad { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime MedioAmbVto { get; set; }

        [DataMember]
        public UDTSQL_char SeguridadNormal { get; set; }

        [DataMember]
        public UDTSQL_char SeguridadEntidad { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime SeguridadVto { get; set; }

        [DataMember]
        public UDTSQL_tinyint NivelRiesgo { get; set; }

        [DataMember]
        public UDTSQL_char RegistroRUC { get; set; }

        [DataMember]
        public UDTSQL_tinyint Puntaje { get; set; }

        [DataMember]
        public UDT_SiNo CalidadPoliticaInd { get; set; }

        [DataMember]
        public UDT_SiNo MedioAmbPoliticaInd { get; set; }

        [DataMember]
        public UDT_SiNo SeguridadPoliticaInd { get; set; }

        [DataMember]
        public UDT_SiNo AlcoholPoliticaInd { get; set; }

        [DataMember]
        public UDT_SiNo DrogasArmasPoliticaInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint Antiguedad { get; set; }

        [DataMember]
        public UDTSQL_tinyint ContratosCant { get; set; }

        [DataMember]
        public UDTSQL_tinyint ProfesionalesTecnicos { get; set; }

        [DataMember]
        public UDTSQL_tinyint ProfesionalesAdmin { get; set; }

        [DataMember]
        public UDTSQL_tinyint TecnicosOper { get; set; }

        [DataMember]
        public UDT_BasicID UsuarioDcum { get; set; }

        [DataMember]
        public UDT_Descriptivo UsuarioDesc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDocum { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoProveedor { get; set; }

        [DataMember]
        public UDT_DescripTBase Direccion { get; set; }

        [DataMember]
        public UDTSQL_char RepresentanteLegal { get; set; }

        [DataMember]
        public UDTSQL_char ObjetoSocial { get; set; }

        //Extra

        [DataMember]
        public UDT_LugarGeograficoID Ciudad { get; set; }

        [DataMember]
        public UDT_DescripTBase Web { get; set; }

        [DataMember]
        public UDTSQL_tinyint Tipo_Identificacion { get; set; } 
        #endregion
    }
}
