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
    /// Models DTO_coTercero
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coTercero : DTO_MasterBasic
    {
        #region DTO_coTercero
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coTercero(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.LugarGeoDesc.Value = dr["LugarGeoDesc"].ToString();
                    this.RegimenDesc.Value = dr["RegimenDesc"].ToString();
                    this.TerDocTipoDesc.Value = dr["TerDocTipoDesc"].ToString();
                    this.ActEconDesc.Value = dr["ActEconDesc"].ToString();
                    this.Banco1Desc.Value = dr["Banco1Desc"].ToString();
                    this.Banco2Desc.Value = dr["Banco2Desc"].ToString();

                    this.UsuarioRespDesc.Value = dr["UsuarioRespDesc"].ToString();
                    this.UsuarioResponsable.Value = dr["UsuarioID1"].ToString();
                    this.PaisDesc.Value = dr["PaisDesc"].ToString();
                }
                else
                    this.UsuarioResponsable.Value = dr["UsuarioResponsable"].ToString();

                this.DigitoVerif.Value = dr["DigitoVerif"].ToString();
                this.ApellidoPri.Value = dr["ApellidoPri"].ToString();
                this.ApellidoSdo.Value = dr["ApellidoSdo"].ToString();
                this.NombrePri.Value = dr["NombrePri"].ToString();
                this.NombreSdo.Value = dr["NombreSdo"].ToString();
                this.LugarGeograficoID.Value = dr["LugarGeograficoID"].ToString();
                this.ReferenciaID.Value = dr["ReferenciaID"].ToString();
                this.ActEconomicaID.Value = dr["ActEconomicaID"].ToString();
                this.TerceroDocTipoID.Value = dr["TerceroDocTipoID"].ToString();
                this.BancoID_1.Value = dr["BancoID_1"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CuentaTipo_1"].ToString()))
                    this.CuentaTipo_1.Value = Convert.ToByte(dr["CuentaTipo_1"]);
                this.BcoCtaNro_1.Value = dr["BcoCtaNro_1"].ToString();
                this.BancoID_2.Value = dr["BancoID_2"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CuentaTipo_2"].ToString()))
                    this.CuentaTipo_2.Value = Convert.ToByte(dr["CuentaTipo_2"]);
                this.BcoCtaNro_2.Value = dr["BcoCtaNro_2"].ToString();
                this.Tel1.Value = dr["Tel1"].ToString();
                this.Tel2.Value = dr["Tel2"].ToString();
                this.Direccion.Value = dr["Direccion"].ToString();
                this.CECorporativo.Value = dr["CECorporativo"].ToString();
                this.RepLegalNombre.Value = dr["RepLegalNombre"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["RepLegalCel"].ToString()))
                    this.RepLegalCel.Value = dr["RepLegalCel"].ToString();
                this.RepLegalCE.Value = dr["RepLegalCE"].ToString();
                this.AutoRetIVAInd.Value = Convert.ToBoolean(dr["AutoRetIVAInd"]);
                this.AutoRetRentaInd.Value = Convert.ToBoolean(dr["AutoRetRentaInd"]);
                this.DeclaraIVAInd.Value = Convert.ToBoolean(dr["DeclaraIVAInd"]);
                this.DeclaraRentaInd.Value = Convert.ToBoolean(dr["DeclaraRentaInd"]);
                this.RadicaDirectoInd.Value = Convert.ToBoolean(dr["RadicaDirectoInd"]);
                this.IndependienteEMPInd.Value = Convert.ToBoolean(dr["IndependienteEMPInd"]);
                this.ExcluyeCREEInd.Value = Convert.ToBoolean(dr["ExcluyeCREEInd"]);
                if (!string.IsNullOrWhiteSpace(dr["PersonaJuridica"].ToString()))
                    this.PersonaJuridica.Value = Convert.ToBoolean(dr["PersonaJuridica"]);
                if (!string.IsNullOrWhiteSpace(dr["BancoExtra"].ToString()))
                    this.BancoExtra.Value = dr["BancoExtra"].ToString();
                this.CuentaExtra.Value = dr["CuentaExtra"].ToString();
                this.ABANro.Value = dr["ABANro"].ToString();
                this.SwiftNro.Value = dr["SwiftNro"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DireccionBcoExtra"].ToString()))
                    this.DireccionBcoExtra.Value = dr["DireccionBcoExtra"].ToString();
                this.Pais.Value = dr["Pais"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DescuentoDias1"].ToString()))
                    this.DescuentoDias1.Value = Convert.ToByte(dr["DescuentoDias1"]);
                if (!string.IsNullOrWhiteSpace(dr["DescuentoPorc1"].ToString()))
                    this.DescuentoPorc1.Value = Convert.ToDecimal(dr["DescuentoPorc1"]);
                if (!string.IsNullOrWhiteSpace(dr["DescuentoDias2"].ToString()))
                    this.DescuentoDias2.Value = Convert.ToByte(dr["DescuentoDias2"]);
                if (!string.IsNullOrWhiteSpace(dr["DescuentoCondicion"].ToString()))
                    this.DescuentoCondicion.Value = Convert.ToByte(dr["DescuentoCondicion"]);
                if (!string.IsNullOrWhiteSpace(dr["Beneficiario"].ToString()))
                    this.Beneficiario.Value = dr["Beneficiario"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["RepLegalCed"].ToString()))
                    this.RepLegalCed.Value = dr["RepLegalCed"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["RetIvaResolucion"].ToString()))
                    this.RetIvaResolucion.Value = dr["RetIvaResolucion"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["RetIvaFecha"].ToString()))
                    this.RetIvaFecha.Value = Convert.ToDateTime(dr["RetIvaFecha"]);
                if (!string.IsNullOrWhiteSpace(dr["RetRtaResolucion"].ToString()))
                    this.RetRtaResolucion.Value = dr["RetRtaResolucion"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["RetRtaFecha"].ToString()))
                    this.RetRtaFecha.Value = Convert.ToDateTime(dr["RetRtaFecha"]);
                this.EmpleadoInd.Value = Convert.ToBoolean(dr["EmpleadoInd"]);
                this.ProveedorInd.Value = Convert.ToBoolean(dr["ProveedorInd"]);
                this.ClienteInd.Value = Convert.ToBoolean(dr["ClienteInd"]);
                this.SocioInd.Value = Convert.ToBoolean(dr["SocioInd"]);
                if (!string.IsNullOrWhiteSpace(dr["ContrasenaRep"].ToString()))
                    this.ContrasenaRep.Value = Convert.ToByte(dr["ContrasenaRep"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaUltimaDig"].ToString()))
                    this.FechaUltimaDig.Value = Convert.ToDateTime(dr["FechaUltimaDig"]);
                this.Telefono1.Value = dr["Telefono1"].ToString();
                this.Telefono2.Value = dr["Telefono2"].ToString();
                this.Extension1.Value = dr["Extension1"].ToString();
                this.Extension2.Value = dr["Extension2"].ToString();
                this.Celular1.Value = dr["Celular1"].ToString();
                this.Celular2.Value = dr["Celular2"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coTercero()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.DigitoVerif = new UDTSQL_char(1);
            this.ApellidoPri = new UDT_DescripTBase();
            this.ApellidoSdo = new UDT_DescripTBase();
            this.NombrePri = new UDT_DescripTBase();
            this.NombreSdo = new UDT_DescripTBase();
            this.LugarGeograficoID = new UDT_BasicID();
            this.LugarGeoDesc = new UDT_Descriptivo();
            this.ReferenciaID = new UDT_BasicID();
            this.RegimenDesc = new UDT_Descriptivo();
            this.ActEconomicaID = new UDT_BasicID();
            this.ActEconDesc = new UDT_Descriptivo();
            this.TerceroDocTipoID = new UDT_BasicID();
            this.TerDocTipoDesc = new UDT_Descriptivo();
            this.BancoID_1 = new UDT_BasicID();
            this.Banco1Desc = new UDT_Descriptivo();
            this.CuentaTipo_1 = new UDTSQL_tinyint();
            this.BcoCtaNro_1 = new UDTSQL_varchar(15);
            this.BancoID_2 = new UDT_BasicID();
            this.Banco2Desc = new UDT_Descriptivo();
            this.CuentaTipo_2 = new UDTSQL_tinyint();
            this.BcoCtaNro_2 = new UDTSQL_varchar(15);
            this.Tel1 = new UDTSQL_char(50);
            this.Tel2 = new UDTSQL_char(50);
            this.Direccion = new UDT_DescripTBase();
            this.CECorporativo = new UDTSQL_char(100);
            this.RepLegalNombre = new UDT_DescripTBase();
            this.RepLegalCel = new UDTSQL_char(20);
            this.RepLegalCE = new UDTSQL_char(100);
            this.AutoRetIVAInd = new UDT_SiNo();
            this.AutoRetRentaInd = new UDT_SiNo();
            this.DeclaraIVAInd = new UDT_SiNo();
            this.DeclaraRentaInd = new UDT_SiNo();
            this.RadicaDirectoInd = new UDT_SiNo();
            this.IndependienteEMPInd = new UDT_SiNo();
            this.ExcluyeCREEInd = new UDT_SiNo();
            this.PersonaJuridica = new UDT_SiNo();
            this.UsuarioResponsable = new UDT_BasicID();
            this.UsuarioRespDesc = new UDT_Descriptivo();
            this.BancoExtra = new UDTSQL_char(60);
            this.CuentaExtra = new UDTSQL_char(25);
            this.ABANro = new UDTSQL_char(25);
            this.SwiftNro = new UDTSQL_char(25);
            this.DireccionBcoExtra = new UDT_DescripTExt();
            this.Pais = new UDT_PaisID();
            this.PaisDesc = new UDT_Descriptivo();
            this.DescuentoDias1 = new UDTSQL_tinyint();
            this.DescuentoPorc1 = new UDT_PorcentajeID();
            this.DescuentoDias2 = new UDTSQL_tinyint();
            this.DescuentoPorc2 = new UDT_PorcentajeID();
            this.DescuentoCondicion = new UDTSQL_tinyint();
            this.Beneficiario = new UDT_DescripTBase();
            this.RepLegalCed = new UDTSQL_char(15);
            this.RetIvaResolucion = new UDTSQL_char(20);
            this.RetIvaFecha = new UDTSQL_smalldatetime();
            this.RetRtaResolucion = new UDTSQL_char(20);
            this.RetRtaFecha = new UDTSQL_smalldatetime();
            this.EmpleadoInd = new UDT_SiNo();
            this.ProveedorInd = new UDT_SiNo();
            this.ClienteInd = new UDT_SiNo();
            this.SocioInd = new UDT_SiNo();
            this.ContrasenaRep = new UDTSQL_tinyint();
            this.FechaUltimaDig = new UDTSQL_datetime();
            this.Telefono1 = new UDTSQL_char(15);
            this.Telefono2 = new UDTSQL_char(15);
            this.Extension1 = new UDTSQL_char(15);
            this.Extension2 = new UDTSQL_char(15);
            this.Celular1 = new UDTSQL_char(15);
            this.Celular2 = new UDTSQL_char(15);
        }

        public DTO_coTercero(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coTercero(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_char DigitoVerif { get; set; }

        [DataMember]
        public UDT_DescripTBase ApellidoPri { get; set; }

        [DataMember]
        public UDT_DescripTBase ApellidoSdo { get; set; }

        [DataMember]
        public UDT_DescripTBase NombrePri { get; set; }

        [DataMember]
        public UDT_DescripTBase NombreSdo { get; set; }

        [DataMember]
        public UDT_BasicID LugarGeograficoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LugarGeoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ReferenciaID { get; set; }

        [DataMember]
        public UDT_Descriptivo RegimenDesc { get; set; }

        [DataMember]
        public UDT_BasicID ActEconomicaID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActEconDesc { get; set; }

        [DataMember]
        public UDT_BasicID TerceroDocTipoID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerDocTipoDesc { get; set; }

        [DataMember]
        public UDT_BasicID BancoID_1 { get; set; }

        [DataMember]
        public UDT_Descriptivo Banco1Desc { get; set; }

        [DataMember]
        public UDTSQL_tinyint CuentaTipo_1 { get; set; }

        [DataMember]
        public UDTSQL_varchar BcoCtaNro_1 { get; set; }

        [DataMember]
        public UDT_BasicID BancoID_2 { get; set; }

        [DataMember]
        public UDT_Descriptivo Banco2Desc { get; set; }

        [DataMember]
        public UDTSQL_tinyint CuentaTipo_2 { get; set; }

        [DataMember]
        public UDTSQL_varchar BcoCtaNro_2 { get; set; }

        [DataMember]
        public UDTSQL_char Tel1 { get; set; }

        [DataMember]
        public UDTSQL_char Tel2 { get; set; }

        [DataMember]
        public UDT_DescripTBase Direccion { get; set; }

        [DataMember]
        public UDTSQL_char CECorporativo { get; set; }

        [DataMember]
        public UDT_DescripTBase RepLegalNombre { get; set; }

        [DataMember]
        public UDTSQL_char RepLegalCel { get; set; }

        [DataMember]
        public UDTSQL_char RepLegalCE { get; set; }

        [DataMember]
        public UDT_SiNo AutoRetIVAInd { get; set; }

        [DataMember]
        public UDT_SiNo AutoRetRentaInd { get; set; }

        [DataMember]
        public UDT_SiNo DeclaraIVAInd { get; set; }

        [DataMember]
        public UDT_SiNo DeclaraRentaInd { get; set; }

        [DataMember]
        public UDT_SiNo RadicaDirectoInd { get; set; }

        [DataMember]
        public UDT_SiNo IndependienteEMPInd { get; set; }

        [DataMember]
        public UDT_SiNo ExcluyeCREEInd { get; set; }

        [DataMember]
        public UDT_SiNo PersonaJuridica { get; set; }

        [DataMember]
        public UDT_BasicID UsuarioResponsable { get; set; }

        [DataMember]
        public UDT_Descriptivo UsuarioRespDesc { get; set; }

        [DataMember]
        public UDTSQL_char BancoExtra { get; set; }

        [DataMember]
        public UDTSQL_char CuentaExtra { get; set; }

        [DataMember]
        public UDTSQL_char ABANro { get; set; }

        [DataMember]
        public UDTSQL_char SwiftNro { get; set; }

        [DataMember]
        public UDT_DescripTExt DireccionBcoExtra { get; set; }

        [DataMember]
        public UDT_PaisID Pais { get; set; }

        [DataMember]
        public UDT_Descriptivo PaisDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint DescuentoDias1 { get; set; }

        [DataMember]
        public UDT_PorcentajeID DescuentoPorc1 { get; set; }

        [DataMember]
        public UDTSQL_tinyint DescuentoDias2 { get; set; }

        [DataMember]
        public UDT_PorcentajeID DescuentoPorc2 { get; set; }

        [DataMember]
        public UDTSQL_tinyint DescuentoCondicion { get; set; }

        [DataMember]
        public UDT_DescripTBase Beneficiario { get; set; }

        [DataMember]
        public UDTSQL_char RepLegalCed { get; set; }

        [DataMember]
        public UDTSQL_char RetIvaResolucion { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime RetIvaFecha { get; set; }

        [DataMember]
        public UDTSQL_char RetRtaResolucion { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime RetRtaFecha { get; set; }

        [DataMember]
        public UDT_SiNo EmpleadoInd { get; set; }
        
        [DataMember]
        public UDT_SiNo ProveedorInd { get; set; }
        
        [DataMember]
        public UDT_SiNo ClienteInd { get; set; }
        
        [DataMember]
        public UDT_SiNo SocioInd { get; set; }

        [DataMember]
        public byte[] Contrasena { get; set; }

        [DataMember]
        public UDTSQL_tinyint ContrasenaRep { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaUltimaDig { get; set; }

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
    }
}
