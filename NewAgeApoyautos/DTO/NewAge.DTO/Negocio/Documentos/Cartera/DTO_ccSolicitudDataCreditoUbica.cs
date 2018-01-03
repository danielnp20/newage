using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Models DTO_ccSolicitudDataCreditoUbica
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSolicitudDataCreditoUbica
    {
        #region ccSolicitudDataCreditoUbica

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudDataCreditoUbica(IDataReader dr)
        {
            InitCols();
            try
            {
                 this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                 this.Version.Value = Convert.ToByte(dr["Version"]);
                 this.TipoId.Value= Convert.ToString(dr["TipoId"]);
                 this.NumeroId.Value= Convert.ToString(dr["NumeroId"]);
                 this.Nombre.Value= Convert.ToString(dr["Nombre"]);
                 this.FechaExp.Value = Convert.ToDateTime(dr["FechaExp"]);
                 this.CiudadExp.Value= Convert.ToString(dr["CiudadExp"]);
                 this.DeptoExp.Value= Convert.ToString(dr["DeptoExp"]);
                 this.Genero.Value= Convert.ToString(dr["Genero"]);
                 this.RangoEdad.Value= Convert.ToString(dr["RangoEdad"]);
                 this.CiudadTel1.Value= Convert.ToString(dr["CiudadTel1"]);
                 this.DeptoTel1.Value= Convert.ToString(dr["DeptoTel1"]);
                 this.CodCiudadTel1.Value= Convert.ToString(dr["CodCiudadTel1"]);
                 this.CodDeptoTel1.Value= Convert.ToString(dr["CodDeptoTel1"]);
                 this.NumeroTel1.Value= Convert.ToString(dr["NumeroTel1"]);
                 this.TipoTel1.Value= Convert.ToString(dr["TipoTel1"]);
                 this.RepDesdeTel1.Value = Convert.ToDateTime(dr["RepDesdeTel1"]);
                 this.FechaActTel1.Value = Convert.ToDateTime(dr["FechaActTel1"]);
                 this.NumEntidadTel1.Value = Convert.ToByte(dr["NumEntidadTel1"]);
                 this.CiudadTel2.Value= Convert.ToString(dr["CiudadTel2"]);
                 this.DeptoTel2.Value= Convert.ToString(dr["DeptoTel2"]);
                 this.CodCiudadTel2.Value= Convert.ToString(dr["CodCiudadTel2"]);
                 this.CodDeptoTel2.Value= Convert.ToString(dr["CodDeptoTel2"]);
                 this.NumeroTel2.Value= Convert.ToString(dr["NumeroTel2"]);
                 this.TipoTel2.Value= Convert.ToString(dr["TipoTel2"]);
                 this.RepDesdeTel2.Value = Convert.ToDateTime(dr["RepDesdeTel2"]);
                 this.FechaActTel2.Value = Convert.ToDateTime(dr["FechaActTel2"]);
                 this.NumEntidadTel2.Value = Convert.ToByte(dr["NumEntidadTel2"]);
                 this.CiudadTel3.Value= Convert.ToString(dr["CiudadTel3"]);
                 this.DeptoTel3.Value= Convert.ToString(dr["DeptoTel3"]);
                 this.CodCiudadTel3.Value= Convert.ToString(dr["CodCiudadTel3"]);
                 this.CodDeptoTel3.Value= Convert.ToString(dr["CodDeptoTel3"]);
                 this.NumeroTel3.Value= Convert.ToString(dr["NumeroTel3"]);
                 this.TipoTel3.Value= Convert.ToString(dr["TipoTel3"]);
                 this.RepDesdeTel3.Value = Convert.ToDateTime(dr["RepDesdeTel3"]);
                 this.FechaActTel3.Value = Convert.ToDateTime(dr["FechaActTel3"]);
                 this.NumEntidadTel3.Value = Convert.ToByte(dr["NumEntidadTel3"]);
                 this.CiudadDir1.Value= Convert.ToString(dr["CiudadDir1"]);
                 this.DeptoDir1.Value= Convert.ToString(dr["DeptoDir1"]);
                 this.CodCiudadDir1.Value= Convert.ToString(dr["CodCiudadDir1"]);
                 this.CodDeptoDir1.Value= Convert.ToString(dr["CodDeptoDir1"]);
                 this.DireccionDir1.Value= Convert.ToString(dr["DireccionDir1"]);
                 this.TipoDir1.Value= Convert.ToString(dr["TipoDir1"]);
                 this.EstratoDir1.Value= Convert.ToString(dr["EstratoDir1"]);
                 this.RepDesdeDir1.Value = Convert.ToDateTime(dr["RepDesdeDir1"]);
                 this.FechaActDir1.Value = Convert.ToDateTime(dr["FechaActDir1"]);
                 this.NumEntidadDir1.Value = Convert.ToByte(dr["NumEntidadDir1"]);
                 this.CiudadDir2.Value= Convert.ToString(dr["CiudadDir2"]);
                 this.DeptoDir2.Value= Convert.ToString(dr["DeptoDir2"]);
                 this.CodCiudadDir2.Value= Convert.ToString(dr["CodCiudadDir2"]);
                 this.CodDeptoDir2.Value= Convert.ToString(dr["CodDeptoDir2"]);
                 this.DireccionDir2.Value= Convert.ToString(dr["DireccionDir2"]);
                 this.TipoDir2.Value= Convert.ToString(dr["TipoDir2"]);
                 this.EstratoDir2.Value= Convert.ToString(dr["EstratoDir2"]);
                if (!string.IsNullOrWhiteSpace(dr["RepDesdeDir2"].ToString()))
                    this.RepDesdeDir2.Value = Convert.ToDateTime(dr["RepDesdeDir2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaActDir2"].ToString()))
                    this.FechaActDir2.Value = Convert.ToDateTime(dr["FechaActDir2"]);
                if (!string.IsNullOrWhiteSpace(dr["NumEntidadDir2"].ToString()))
                    this.NumEntidadDir2.Value = Convert.ToByte(dr["NumEntidadDir2"]);
                 this.CiudadDir3.Value= Convert.ToString(dr["CiudadDir3"]);
                 this.DeptoDir3.Value= Convert.ToString(dr["DeptoDir3"]);
                 this.CodCiudadDir3.Value= Convert.ToString(dr["CodCiudadDir3"]);
                 this.CodDeptoDir3.Value= Convert.ToString(dr["CodDeptoDir3"]);
                 this.DireccionDir3.Value= Convert.ToString(dr["DireccionDir3"]);
                 this.TipoDir3.Value= Convert.ToString(dr["TipoDir3"]);
                 this.EstratoDir3.Value= Convert.ToString(dr["EstratoDir3"]);
                if (!string.IsNullOrWhiteSpace(dr["RepDesdeDir3"].ToString()))
                    this.RepDesdeDir3.Value = Convert.ToDateTime(dr["RepDesdeDir3"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaActDir3"].ToString()))
                    this.FechaActDir3.Value = Convert.ToDateTime(dr["FechaActDir3"]);
                if (!string.IsNullOrWhiteSpace(dr["NumEntidadDir3"].ToString()))
                    this.NumEntidadDir3.Value = Convert.ToByte(dr["NumEntidadDir3"]);
                 this.Email1.Value= Convert.ToString(dr["Email1"]);
                 this.RepDesdeMail1.Value = Convert.ToDateTime(dr["RepDesdeMail1"]);
                 this.FechaActMail1.Value = Convert.ToDateTime(dr["FechaActMail1"]);
                 this.NumEntidadMail1.Value = Convert.ToByte(dr["NumEntidadMail1"]);
                 this.Email2.Value= Convert.ToString(dr["Email2"]);
                if (!string.IsNullOrWhiteSpace(dr["RepDesdeMail2"].ToString()))
                    this.RepDesdeMail2.Value = Convert.ToDateTime(dr["RepDesdeMail2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaActMail2"].ToString()))
                    this.FechaActMail2.Value = Convert.ToDateTime(dr["FechaActMail2"]);
                if (!string.IsNullOrWhiteSpace(dr["NumEntidadMail2"].ToString()))
                    this.NumEntidadMail2.Value = Convert.ToByte(dr["NumEntidadMail2"]);
                 this.Celular1.Value= Convert.ToString(dr["Celular1"]);
                 this.RepDesdeCel1.Value = Convert.ToDateTime(dr["RepDesdeCel1"]);
                 this.FechaActCel1.Value = Convert.ToDateTime(dr["FechaActCel1"]);
                 this.NumEntidadCel1.Value = Convert.ToByte(dr["NumEntidadCel1"]);
                 this.Celular2.Value= Convert.ToString(dr["Celular2"]);
                if (!string.IsNullOrWhiteSpace(dr["RepDesdeCel2"].ToString()))
                    this.RepDesdeCel2.Value = Convert.ToDateTime(dr["RepDesdeCel2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaActCel2"].ToString()))
                    this.FechaActCel2.Value = Convert.ToDateTime(dr["FechaActCel2"]);
                if (!string.IsNullOrWhiteSpace(dr["NumEntidadCel2"].ToString()))
                    this.NumEntidadCel2.Value = Convert.ToByte(dr["NumEntidadCel2"]);
                 this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);

                 if (!string.IsNullOrWhiteSpace(dr["Direccion1IND"].ToString())) 
                    this.Direccion1IND.Value = Convert.ToBoolean(dr["Direccion1IND"]);
                 if (!string.IsNullOrWhiteSpace(dr["Direccion2IND"].ToString())) 
                     this.Direccion2IND.Value = Convert.ToBoolean(dr["Direccion2IND"]);
                 if (!string.IsNullOrWhiteSpace(dr["Direccion3IND"].ToString()))
                     this.Direccion3IND.Value = Convert.ToBoolean(dr["Direccion3IND"]);
                 if (!string.IsNullOrWhiteSpace(dr["DireccionOtraIND"].ToString()))
                     this.DireccionOtraIND.Value = Convert.ToBoolean(dr["DireccionOtraIND"]);
                 if (!string.IsNullOrWhiteSpace(dr["DireccionOtra"].ToString()))
                     this.DireccionOtra.Value = Convert.ToString(dr["DireccionOtra"]);
                 if (!string.IsNullOrWhiteSpace(dr["TipoDirOtra"].ToString()))
                     this.TipoDirOtra.Value = Convert.ToString(dr["TipoDirOtra"]);
                 if (!string.IsNullOrWhiteSpace(dr["TipoTelOtro"].ToString()))
                     this.TipoTelOtro.Value = Convert.ToString(dr["TipoTelOtro"]); 
                if (!string.IsNullOrWhiteSpace(dr["Telefono1IND"].ToString()))
                     this.Telefono1IND.Value = Convert.ToBoolean(dr["Telefono1IND"]);
                 if (!string.IsNullOrWhiteSpace(dr["Telefono2IND"].ToString()))
                     this.Telefono2IND.Value = Convert.ToBoolean(dr["Telefono2IND"]);
                 if (!string.IsNullOrWhiteSpace(dr["Telefono3IND"].ToString()))
                     this.Telefono3IND.Value = Convert.ToBoolean(dr["Telefono3IND"]);
                 if (!string.IsNullOrWhiteSpace(dr["TelefonoOtroIND"].ToString()))
                     this.TelefonoOtroIND.Value = Convert.ToBoolean(dr["TelefonoOtroIND"]);
                 if (!string.IsNullOrWhiteSpace(dr["TelefonoOtro"].ToString()))
                     this.TelefonoOtro.Value = Convert.ToString(dr["TelefonoOtro"]);
                 if (!string.IsNullOrWhiteSpace(dr["Celular1IND"].ToString()))
                     this.Celular1IND.Value = Convert.ToBoolean(dr["Celular1IND"]);
                 if (!string.IsNullOrWhiteSpace(dr["Celular2IND"].ToString()))
                     this.Celular2IND.Value = Convert.ToBoolean(dr["Celular2IND"]);
                 if (!string.IsNullOrWhiteSpace(dr["CelularOtraIND"].ToString()))
                     this.CelularOtraIND.Value = Convert.ToBoolean(dr["CelularOtraIND"]);
                 if (!string.IsNullOrWhiteSpace(dr["CelularOtro"].ToString()))
                     this.CelularOtro.Value = Convert.ToString(dr["CelularOtro"]);
                 if (!string.IsNullOrWhiteSpace(dr["EMailOtroIND"].ToString()))
                     this.EMailOtroIND.Value = Convert.ToBoolean(dr["EMailOtroIND"]);
                 if (!string.IsNullOrWhiteSpace(dr["EMailOtro"].ToString()))
                     this.EMailOtro.Value = Convert.ToString(dr["EMailOtro"]);
                 if (!string.IsNullOrWhiteSpace(dr["EMail1IND"].ToString()))
                     this.EMail1IND.Value = Convert.ToBoolean(dr["EMail1IND"]);
                 if (!string.IsNullOrWhiteSpace(dr["EMail2IND"].ToString()))
                     this.EMail2IND.Value = Convert.ToBoolean(dr["EMail2IND"]);



            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccSolicitudDataCreditoUbica()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.Version = new UDTSQL_int();
            this.TipoId=new UDTSQL_varchar(1);
            this.NumeroId=new UDTSQL_varchar(11);
            this.Nombre=new UDTSQL_varchar(50);
            this.FechaExp=new UDTSQL_smalldatetime ();
            this.CiudadExp=new UDTSQL_varchar (30);
            this.DeptoExp=new UDTSQL_varchar (30);
            this.Genero=new UDTSQL_varchar (1);
            this.RangoEdad=new UDTSQL_varchar (5);
            this.CiudadTel1=new UDTSQL_varchar (30);
            this.DeptoTel1=new UDTSQL_varchar (20);
            this.CodCiudadTel1=new UDTSQL_varchar (10);
            this.CodDeptoTel1=new UDTSQL_varchar (2);
            this.NumeroTel1=new UDTSQL_varchar (20);
            this.TipoTel1=new UDTSQL_varchar (20);
            this.RepDesdeTel1 = new UDTSQL_smalldatetime();
            this.FechaActTel1=new UDTSQL_smalldatetime ();
            this.NumEntidadTel1=new UDTSQL_tinyint ();
            this.CiudadTel2=new UDTSQL_varchar (30);
            this.DeptoTel2=new UDTSQL_varchar (20);
            this.CodCiudadTel2=new UDTSQL_varchar (10);
            this.CodDeptoTel2=new UDTSQL_varchar (2);
            this.NumeroTel2=new UDTSQL_varchar (20);
            this.TipoTel2=new UDTSQL_varchar (20);
            this.RepDesdeTel2 = new UDTSQL_smalldatetime();
            this.FechaActTel2=new UDTSQL_smalldatetime ();
            this.NumEntidadTel2=new UDTSQL_tinyint ();
            this.CiudadTel3=new UDTSQL_varchar (30);
            this.DeptoTel3=new UDTSQL_varchar (20);
            this.CodCiudadTel3=new UDTSQL_varchar (10);
            this.CodDeptoTel3=new UDTSQL_varchar (2);
            this.NumeroTel3=new UDTSQL_varchar (20);
            this.TipoTel3=new UDTSQL_varchar (20);
            this.RepDesdeTel3 = new UDTSQL_smalldatetime();
            this.FechaActTel3=new UDTSQL_smalldatetime ();
            this.NumEntidadTel3=new UDTSQL_tinyint ();
            this.CiudadDir1=new UDTSQL_varchar (30);
            this.DeptoDir1=new UDTSQL_varchar (20);
            this.CodCiudadDir1=new UDTSQL_varchar (10);
            this.CodDeptoDir1=new UDTSQL_varchar (2);
            this.DireccionDir1=new UDTSQL_varchar (100);
            this.TipoDir1=new UDTSQL_varchar (20);
            this.EstratoDir1=new UDTSQL_varchar (5);
            this.RepDesdeDir1 = new UDTSQL_smalldatetime();
            this.FechaActDir1=new UDTSQL_smalldatetime ();
            this.NumEntidadDir1=new UDTSQL_tinyint ();
            this.CiudadDir2=new UDTSQL_varchar (30);
            this.DeptoDir2=new UDTSQL_varchar (20);
            this.CodCiudadDir2=new UDTSQL_varchar (10);
            this.CodDeptoDir2=new UDTSQL_varchar (2);
            this.DireccionDir2=new UDTSQL_varchar (100);
            this.TipoDir2=new UDTSQL_varchar (20);
            this.EstratoDir2=new UDTSQL_varchar (5);
            this.RepDesdeDir2 = new UDTSQL_smalldatetime();
            this.FechaActDir2=new UDTSQL_smalldatetime ();
            this.NumEntidadDir2=new UDTSQL_tinyint ();
            this.CiudadDir3=new UDTSQL_varchar (30);
            this.DeptoDir3=new UDTSQL_varchar (20);
            this.CodCiudadDir3=new UDTSQL_varchar (10);
            this.CodDeptoDir3=new UDTSQL_varchar (2);
            this.DireccionDir3=new UDTSQL_varchar (100);
            this.TipoDir3=new UDTSQL_varchar (20);
            this.EstratoDir3=new UDTSQL_varchar (5);
            this.RepDesdeDir3 = new UDTSQL_smalldatetime();
            this.FechaActDir3=new UDTSQL_smalldatetime ();
            this.NumEntidadDir3=new UDTSQL_tinyint ();
            this.Email1=new UDTSQL_varchar (100);
            this.RepDesdeMail1 = new UDTSQL_smalldatetime();
            this.FechaActMail1=new UDTSQL_smalldatetime ();
            this.NumEntidadMail1=new UDTSQL_tinyint ();
            this.Email2=new UDTSQL_varchar (100);
            this.RepDesdeMail2 = new UDTSQL_smalldatetime();
            this.FechaActMail2=new UDTSQL_smalldatetime ();
            this.NumEntidadMail2=new UDTSQL_tinyint ();
            this.Celular1=new UDTSQL_varchar (15);
            this.RepDesdeCel1 = new UDTSQL_smalldatetime();
            this.FechaActCel1=new UDTSQL_smalldatetime ();
            this.NumEntidadCel1=new UDTSQL_tinyint ();
            this.Celular2=new UDTSQL_varchar (15);
            this.RepDesdeCel2 = new UDTSQL_smalldatetime();
            this.FechaActCel2=new UDTSQL_smalldatetime ();
            this.NumEntidadCel2=new UDTSQL_tinyint ();

            this.Consecutivo=new UDT_Consecutivo();

            this.Direccion1IND=new UDT_SiNo();
            this.Direccion2IND=new UDT_SiNo();
            this.Direccion3IND=new UDT_SiNo();
            this.DireccionOtraIND=new UDT_SiNo();
            this.DireccionOtra=new UDTSQL_varchar(100);
            this.TipoDirOtra = new UDTSQL_varchar(20);
            this.TipoTelOtro = new UDTSQL_varchar(20);
            this.Telefono1IND=new UDT_SiNo();
            this.Telefono2IND=new UDT_SiNo();
            this.Telefono3IND=new UDT_SiNo();
            this.TelefonoOtroIND=new UDT_SiNo();
            this.TelefonoOtro = new UDTSQL_varchar(20);
            this.Celular1IND=new UDT_SiNo();
            this.Celular2IND=new UDT_SiNo();
            this.CelularOtraIND=new UDT_SiNo();
            this.CelularOtro = new UDTSQL_varchar(15);
            this.EMailOtroIND=new UDT_SiNo();
            this.EMailOtro = new UDTSQL_varchar(100);
            this.EMail1IND = new UDT_SiNo();
            this.EMail2IND = new UDT_SiNo();
          }

        #endregion

        #region Propiedades
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        [DataMember]
        public UDTSQL_int Version{ get; set; }

        [DataMember]
        public UDTSQL_varchar TipoId{ get; set; }


        [DataMember]
        public UDTSQL_varchar NumeroId{ get; set;}

        [DataMember]
        public UDTSQL_varchar Nombre { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaExp { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CiudadExp { get; set; }
        
        [DataMember]
        public UDTSQL_varchar DeptoExp { get; set; }
        
        [DataMember]
        public UDTSQL_varchar Genero { get; set; }
        
        [DataMember]
        public UDTSQL_varchar RangoEdad { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CiudadTel1 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar DeptoTel1 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CodCiudadTel1 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CodDeptoTel1 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar NumeroTel1 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar TipoTel1 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime RepDesdeTel1 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaActTel1 { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint NumEntidadTel1 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CiudadTel2 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar DeptoTel2 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CodCiudadTel2 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CodDeptoTel2 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar NumeroTel2 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar TipoTel2 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime RepDesdeTel2 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaActTel2 { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint NumEntidadTel2 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CiudadTel3 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar DeptoTel3 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CodCiudadTel3 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CodDeptoTel3 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar NumeroTel3 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar TipoTel3 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime RepDesdeTel3 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaActTel3 { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint NumEntidadTel3 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CiudadDir1 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar DeptoDir1 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CodCiudadDir1 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CodDeptoDir1 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar DireccionDir1 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar TipoDir1 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar EstratoDir1 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime RepDesdeDir1 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaActDir1 { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint NumEntidadDir1 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CiudadDir2 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar DeptoDir2 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CodCiudadDir2 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CodDeptoDir2 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar DireccionDir2 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar TipoDir2 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar EstratoDir2 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime RepDesdeDir2 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaActDir2 { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint NumEntidadDir2 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CiudadDir3 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar DeptoDir3 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CodCiudadDir3 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CodDeptoDir3 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar DireccionDir3 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar TipoDir3 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar EstratoDir3 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime RepDesdeDir3 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaActDir3 { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint NumEntidadDir3 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar Email1 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime RepDesdeMail1 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaActMail1 { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint NumEntidadMail1 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar Email2 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime RepDesdeMail2 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaActMail2 { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint NumEntidadMail2 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar Celular1 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime RepDesdeCel1 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaActCel1 { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint NumEntidadCel1 { get; set; }
        
        [DataMember]
        public UDTSQL_varchar Celular2 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime RepDesdeCel2 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaActCel2 { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint NumEntidadCel2 { get; set; }               
        
        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_SiNo Direccion1IND { get; set; }
        	
        [DataMember]
        public UDT_SiNo Direccion2IND { get; set; }
	
        [DataMember]
        public UDT_SiNo Direccion3IND { get; set; }
	
        [DataMember]
        public UDT_SiNo DireccionOtraIND { get; set; }
	
        [DataMember]
        public UDTSQL_varchar DireccionOtra { get; set; }

        [DataMember]
        public UDTSQL_varchar TipoDirOtra { get; set; }

        [DataMember]
        public UDTSQL_varchar TipoTelOtro { get; set; }

        
        [DataMember]
        public UDT_SiNo Telefono1IND { get; set; }
	
        [DataMember]
        public UDT_SiNo Telefono2IND { get; set; }
	
        [DataMember]
        public UDT_SiNo Telefono3IND { get; set; }
	
        [DataMember]
        public UDT_SiNo TelefonoOtroIND { get; set; }
	
        [DataMember]
        public UDTSQL_varchar TelefonoOtro { get; set; }
	
        [DataMember]
        public UDT_SiNo Celular1IND { get; set; }
	
        [DataMember]
        public UDT_SiNo Celular2IND { get; set; }
	
        [DataMember]
        public UDT_SiNo CelularOtraIND { get; set; }

        [DataMember]
        public UDTSQL_varchar CelularOtro { get; set; }

        [DataMember]
        public UDT_SiNo EMailOtroIND { get; set; }
	
        [DataMember]
        public UDTSQL_varchar EMailOtro { get; set; }

        [DataMember]
        public UDT_SiNo EMail1IND { get; set; }
        
        [DataMember]
        public UDT_SiNo EMail2IND { get; set; }

        
        #endregion
    }
}
