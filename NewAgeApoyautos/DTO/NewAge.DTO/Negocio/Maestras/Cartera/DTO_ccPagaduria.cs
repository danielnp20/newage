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
    /// Models DTO_coActividad
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccPagaduria : DTO_MasterBasic
    {
        #region DTO_caPagaduria
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccPagaduria(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                    this.CiudadDesc.Value = dr["Ciudad"].ToString();
                    this.SectorDesc.Value = dr["SectorDesc"].ToString();
                    this.CodTransmisionAfDesc.Value = Convert.ToString(dr["CodTransmisionAfDesc"]);
                    this.CodTransmisionDesafDesc.Value = Convert.ToString(dr["CodTransmisionDesafDesc"]);
                    this.CodTransmisionIncDesc.Value = Convert.ToString(dr["CodTransmisionIncDesc"]);
                    this.CodTransmisionDesincDesc.Value = Convert.ToString(dr["CodTransmisionDesincDesc"]);
                }

                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.Ciudad.Value = dr["Ciudad"].ToString();
                this.PeriodoPago.Value = Convert.ToByte(dr["PeriodoPago"]);
                this.DiaCorte.Value = Convert.ToByte(dr["DiaCorte"]);
                this.DiaTope.Value = Convert.ToByte(dr["DiaTope"]);
                this.RecaudoMes.Value = Convert.ToBoolean(dr["RecaudoMes"]);
                this.CodIdentifica.Value = dr["CodIdentifica"].ToString();
                this.CodIncorpora.Value = dr["CodIncorpora"].ToString();
                if (!string.IsNullOrEmpty(dr["IncorporacionPorc"].ToString()))
                    this.IncorporacionPorc.Value = Convert.ToInt16(dr["IncorporacionPorc"]);
                this.UsuarioIncorpora.Value = dr["UsuarioIncorpora"].ToString();
                this.UsuarioNomina.Value = dr["UsuarioNomina"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaIncorpora"].ToString()))
                    this.FechaIncorpora.Value = Convert.ToDateTime(dr["FechaIncorpora"]);
                if (!string.IsNullOrEmpty(dr["FechaNomina"].ToString()))
                    this.FechaNomina.Value = Convert.ToDateTime(dr["FechaNomina"]);
                this.Contacto.Value = Convert.ToString(dr["Contacto"]);//
                this.Documentos.Value = Convert.ToString(dr["Documentos"]);
                this.Informacion.Value = Convert.ToString(dr["Informacion"]);
                if (!string.IsNullOrEmpty(dr["FormaIncorpora"].ToString()))
                    this.FormaIncorpora.Value = Convert.ToByte(dr["FormaIncorpora"]);
                this.MedioEnvio.Value = Convert.ToString(dr["MedioEnvio"]);
                if (!string.IsNullOrEmpty(dr["FormaEnvio"].ToString()))
                    this.FormaEnvio.Value = Convert.ToByte(dr["FormaEnvio"]);
                if (!string.IsNullOrEmpty(dr["MaxmesesInc"].ToString()))
                    this.MaxmesesInc.Value = Convert.ToByte(dr["MaxmesesInc"]);
                this.Dispositivo.Value = Convert.ToString(dr["Dispositivo"]);
                this.Destino.Value = Convert.ToString(dr["Destino"]);
                this.Observacion.Value = Convert.ToString(dr["Observacion"]);//
                this.CodTransmisionAf.Value = Convert.ToString(dr["CodTransmisionAf"]);
                this.CodTransmisionDesaf.Value = Convert.ToString(dr["CodTransmisionDesaf"]);
                this.CodTransmisionInc.Value = Convert.ToString(dr["CodTransmisionInc"]);
                this.CodTransmisionDesinc.Value = Convert.ToString(dr["CodTransmisionDesinc"]);
                if (!string.IsNullOrEmpty(dr["RegistraInd"].ToString()))
                    this.RegistraInd.Value = Convert.ToBoolean(dr["RegistraInd"]);
                this.IncorporacionTipo.Value = Convert.ToByte(dr["IncorporacionTipo"]);
                this.CodEmpleadoInd.Value = Convert.ToBoolean(dr["CodEmpleadoInd"]);
                this.CentroPagoInd.Value = Convert.ToBoolean(dr["CentroPagoInd"]);
                this.BloqueaVentaInd.Value = Convert.ToBoolean(dr["BloqueaVentaInd"]);
                this.IncorporaIDE.Value = Convert.ToString(dr["IncorporaIDE"]);
                this.DesincorporaIDE.Value = Convert.ToString(dr["DesincorporaIDE"]);
                this.AfiliaIDE.Value = Convert.ToString(dr["AfiliaIDE"]);
                this.DesafiliaIDE.Value = Convert.ToString(dr["DesafiliaIDE"]);
                this.SectorID.Value = Convert.ToString(dr["SectorID"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccPagaduria()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_BasicID();
            this.Ciudad = new UDT_BasicID();
            this.CiudadDesc = new UDT_Descriptivo();
            this.PeriodoPago = new UDTSQL_tinyint();
            this.DiaCorte = new UDTSQL_tinyint();
            this.DiaTope = new UDTSQL_tinyint();
            this.RecaudoMes = new UDT_SiNo();
            this.CodIdentifica = new UDTSQL_char(10);
            this.CodIncorpora = new UDTSQL_char(10);
            this.IncorporacionPorc = new UDTSQL_numeric();
            this.UsuarioIncorpora = new UDT_UsuarioID();
            this.UsuarioNomina = new UDT_UsuarioID();
            this.FechaIncorpora = new UDTSQL_smalldatetime();
            this.FechaNomina = new UDTSQL_smalldatetime();
            this.Contacto = new UDTSQL_char(100);//
            this.Documentos = new UDTSQL_char(100);
            this.Informacion = new UDT_DescripTBase();
            this.FormaIncorpora = new UDTSQL_tinyint();
            this.MedioEnvio = new UDTSQL_char(50);
            this.FormaEnvio = new UDTSQL_tinyint();
            this.MaxmesesInc = new UDTSQL_tinyint();
            this.Dispositivo = new UDTSQL_char(50);
            this.Destino = new UDT_DescripTBase();
            this.Observacion = new UDT_DescripTExt();//
            this.CodTransmisionAf = new UDT_CodigoGrl5();
            this.CodTransmisionAfDesc = new UDT_Descriptivo();
            this.CodTransmisionDesaf = new UDT_CodigoGrl5();
            this.CodTransmisionDesafDesc = new UDT_Descriptivo();
            this.CodTransmisionInc = new UDT_CodigoGrl5();
            this.CodTransmisionIncDesc = new UDT_Descriptivo();
            this.CodTransmisionDesinc = new UDT_CodigoGrl5();
            this.CodTransmisionDesincDesc = new UDT_Descriptivo();
            this.RegistraInd = new UDT_SiNo();
            this.IncorporacionTipo = new UDTSQL_tinyint();
            this.CodEmpleadoInd = new UDT_SiNo();
            this.CentroPagoInd = new UDT_SiNo();
            this.BloqueaVentaInd = new UDT_SiNo();
            this.IncorporaIDE = new UDT_CodigoGrl5();
            this.DesincorporaIDE = new UDT_CodigoGrl5();
            this.AfiliaIDE = new UDT_CodigoGrl5();
            this.DesafiliaIDE = new UDT_CodigoGrl5();
            this.SectorID= new UDT_CodigoGrl10();
            this.SectorDesc=new UDT_Descriptivo();
        }

        public DTO_ccPagaduria(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccPagaduria(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_BasicID Ciudad { get; set; }

        [DataMember]
        public UDT_Descriptivo CiudadDesc { get; set; }


      

        [DataMember]
        public UDTSQL_tinyint PeriodoPago { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiaCorte { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiaTope { get; set; }

        [DataMember]
        public UDT_SiNo RecaudoMes { get; set; }

        [DataMember]
        public UDTSQL_char CodIncorpora { get; set; }

        [DataMember]
        public UDTSQL_char CodIdentifica { get; set; }

        [DataMember]
        public UDTSQL_numeric IncorporacionPorc { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioNomina { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioIncorpora { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaNomina { get; set; }

        [DataMember]
        public UDTSQL_char Contacto { get; set; }   ///

        [DataMember]
        public UDTSQL_char Documentos { get; set; }

        [DataMember]
        public UDT_DescripTBase Informacion { get; set; }

        [DataMember]
        public UDTSQL_tinyint FormaIncorpora { get; set; }

        [DataMember]
        public UDTSQL_char MedioEnvio { get; set; }

        [DataMember]
        public UDTSQL_tinyint FormaEnvio { get; set; }

        [DataMember]
        public UDTSQL_tinyint MaxmesesInc { get; set; }

        [DataMember]
        public UDTSQL_char Dispositivo { get; set; }

        [DataMember]
        public UDT_DescripTBase Destino { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; } ///

        [DataMember]
        public UDT_CodigoGrl5 CodTransmisionAf { get; set; }

        [DataMember]
        public UDT_Descriptivo CodTransmisionAfDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 CodTransmisionDesaf { get; set; }

        [DataMember]
        public UDT_Descriptivo CodTransmisionDesafDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 CodTransmisionInc { get; set; }

        [DataMember]
        public UDT_Descriptivo CodTransmisionIncDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 CodTransmisionDesinc { get; set; }

        [DataMember]
        public UDT_Descriptivo CodTransmisionDesincDesc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaIncorpora { get; set; }

        [DataMember]
        public UDT_SiNo RegistraInd { get; set; }

        [DataMember]
        public UDT_SiNo CodEmpleadoInd { get; set; }

        [DataMember]
        public UDT_SiNo CentroPagoInd { get; set; }

        [DataMember]
        public UDT_SiNo BloqueaVentaInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint IncorporacionTipo { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 IncorporaIDE { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 DesincorporaIDE { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 AfiliaIDE { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 DesafiliaIDE { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 SectorID { get; set; }

        [DataMember]
        public UDT_Descriptivo SectorDesc { get; set; }
    }
}
