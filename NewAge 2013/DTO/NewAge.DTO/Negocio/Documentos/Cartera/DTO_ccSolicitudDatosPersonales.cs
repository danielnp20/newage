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
    /// Models DTO_ccSolicitudDatosPersonales 
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSolicitudDatosPersonales 
    {
        #region ccSolicitudDatosPersonales 

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudDatosPersonales (IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Version.Value = Convert.ToInt32(dr["Version"]);
                this.TipoPersona.Value = Convert.ToByte(dr["TipoPersona"]);
                this.TerceroID.Value=Convert.ToString(dr["TerceroID"]);
                this.TerceroDocTipoID.Value=Convert.ToString(dr["TerceroDocTipoID"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaExpDoc"].ToString()))
                    this.FechaExpDoc.Value = Convert.ToDateTime(dr["FechaExpDoc"]);

                if (!string.IsNullOrWhiteSpace(dr["CiudadExpDoc"].ToString()))
                    this.CiudadExpDoc.Value = Convert.ToString(dr["CiudadExpDoc"]);

                if (!string.IsNullOrWhiteSpace(dr["FechaNacimiento"].ToString()))
                    this.FechaNacimiento.Value = Convert.ToDateTime(dr["FechaNacimiento"]);
                this.Descriptivo.Value = Convert.ToString(dr["Descriptivo"]);
                if (!string.IsNullOrWhiteSpace(dr["ApellidoPri"].ToString()))
                    this.ApellidoPri.Value = Convert.ToString(dr["ApellidoPri"]);
                if (!string.IsNullOrWhiteSpace(dr["ApellidoSdo"].ToString()))
                    this.ApellidoSdo.Value = Convert.ToString(dr["ApellidoSdo"]);
                if (!string.IsNullOrWhiteSpace(dr["NombrePri"].ToString()))
                    this.NombrePri.Value = Convert.ToString(dr["NombrePri"]);
                if (!string.IsNullOrWhiteSpace(dr["NombreSdo"].ToString()))
                    this.NombreSdo.Value = Convert.ToString(dr["NombreSdo"]);                             
                this.Celular1.Value= Convert.ToString(dr["Celular1"]);
                this.Celular2.Value= Convert.ToString(dr["Celular2"]);
                this.CorreoElectronico.Value= Convert.ToString(dr["CorreoElectronico"]);
                this.DirResidencia.Value= Convert.ToString(dr["DirResidencia"]);    
                this.BarrioResidencia.Value= Convert.ToString(dr["BarrioResidencia"]);       
                this.CiudadResidencia.Value= Convert.ToString(dr["CiudadResidencia"]);
                if (!string.IsNullOrWhiteSpace(dr["AntResidencia"].ToString()))
                    this.AntResidencia.Value= Convert.ToByte(dr["AntResidencia"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoVivienda"].ToString()))
                    this.TipoVivienda.Value= Convert.ToByte(dr["TipoVivienda"]);                    
                this.TelResidencia.Value= Convert.ToString(dr["TelResidencia"]);                        
                this.LugarTrabajo.Value= Convert.ToString(dr["LugarTrabajo"]);                
                this.DirTrabajo.Value= Convert.ToString(dr["DirTrabajo"]);
                this.BarrioTrabajo.Value= Convert.ToString(dr["BarrioTrabajo"]);                        
                this.CiudadTrabajo.Value= Convert.ToString(dr["CiudadTrabajo"]);                            
                this.TelTrabajo.Value= Convert.ToString(dr["TelTrabajo"]);                                
                this.Cargo.Value= Convert.ToString(dr["Cargo"]);
                if (!string.IsNullOrWhiteSpace(dr["AntTrabajo"].ToString()))
                    this.AntTrabajo.Value= Convert.ToByte(dr["AntTrabajo"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoContrato"].ToString()))
                    this.TipoContrato.Value= Convert.ToByte(dr["TipoContrato"]);                                           
                this.EPS.Value= Convert.ToString(dr["EPS"]);
                if (!string.IsNullOrWhiteSpace(dr["Personascargo"].ToString()))
                    this.Personascargo.Value= Convert.ToByte(dr["Personascargo"]);
                this.Conyugue.Value= Convert.ToString(dr["Conyugue"]);
                this.NombreConyugue.Value= Convert.ToString(dr["NombreConyugue"]);
                this.ActConyugue.Value= Convert.ToString(dr["ActConyugue"]);
                if (!string.IsNullOrWhiteSpace(dr["AntConyugue"].ToString()))
                    this.AntConyugue.Value= Convert.ToByte(dr["AntConyugue"]);
                this.EmpresaConyugue.Value = Convert.ToString(dr["EmpresaConyugue"]);                               
                this.DirResConyugue.Value= Convert.ToString(dr["DirResConyugue"]);
                this.TelefonoConyugue.Value= Convert.ToString(dr["TelefonoConyugue"]);
                this.CelularConyugue.Value= Convert.ToString(dr["CelularConyugue"]);
                this.NombreReferencia1.Value= Convert.ToString(dr["NombreReferencia1"]);
                if (!string.IsNullOrWhiteSpace(dr["RelReferencia1"].ToString()))
                     this.RelReferencia1.Value= Convert.ToString(dr["RelReferencia1"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoReferencia1"].ToString()))
                     this.TipoReferencia1.Value= Convert.ToByte(dr["TipoReferencia1"]);
                this.DirReferencia1.Value= Convert.ToString(dr["DirReferencia1"]);
                this.BarrioReferencia1.Value= Convert.ToString(dr["BarrioReferencia1"]);
                this.TelefonoReferencia1.Value= Convert.ToString(dr["TelefonoReferencia1"]);
                this.CelularReferencia1.Value= Convert.ToString(dr["CelularReferencia1"]);
                this.NombreReferencia2.Value= Convert.ToString(dr["NombreReferencia2"]);
                if (!string.IsNullOrWhiteSpace(dr["RelReferencia2"].ToString()))
                     this.RelReferencia2.Value= Convert.ToString(dr["RelReferencia2"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoReferencia2"].ToString()))
                    this.TipoReferencia2.Value= Convert.ToByte(dr["TipoReferencia2"]);
                this.DirReferencia2.Value= Convert.ToString(dr["DirReferencia2"]);
                this.BarrioReferencia2.Value= Convert.ToString(dr["BarrioReferencia2"]);
                this.TelefonoReferencia2.Value= Convert.ToString(dr["TelefonoReferencia2"]);
                this.CelularReferencia2.Value = Convert.ToString(dr["CelularReferencia2"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrActivos"].ToString()))
                    this.VlrActivos.Value = Convert.ToInt64(dr["VlrActivos"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPasivos"].ToString()))
                    this.VlrPasivos.Value = Convert.ToInt64(dr["VlrPasivos"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPatrimonio"].ToString()))
                    this.VlrPatrimonio.Value = Convert.ToInt64(dr["VlrPatrimonio"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrEgresosMes"].ToString()))
                    this.VlrEgresosMes.Value = Convert.ToInt64(dr["VlrEgresosMes"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrIngresosMes"].ToString()))
                    this.VlrIngresosMes.Value = Convert.ToInt64(dr["VlrIngresosMes"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrIngresosNoOpe"].ToString()))
                    this.VlrIngresosNoOpe.Value = Convert.ToInt64(dr["VlrIngresosNoOpe"]);
                this.DescrOtrosIng.Value = Convert.ToString(dr["DescrOtrosIng"]);
                this.DescrOtrosBinenes.Value = Convert.ToString(dr["DescrOtrosBinenes"]);
                this.EntCredito1.Value = Convert.ToString(dr["EntCredito1"]);
                if (!string.IsNullOrWhiteSpace(dr["Plazo1"].ToString()))
                    this.Plazo1.Value = Convert.ToByte(dr["Plazo1"]);
                if (!string.IsNullOrWhiteSpace(dr["Saldo1"].ToString()))
                    this.Saldo1.Value = Convert.ToInt64(dr["Saldo1"]);
                this.EntCredito2.Value = Convert.ToString(dr["EntCredito2"]);
                if (!string.IsNullOrWhiteSpace(dr["Plazo2"].ToString()))
                    this.Plazo2.Value = Convert.ToByte(dr["Plazo2"]);
                if (!string.IsNullOrWhiteSpace(dr["Saldo2"].ToString()))
                    this.Saldo2.Value = Convert.ToInt64(dr["Saldo2"]);
                if (!string.IsNullOrWhiteSpace(dr["SolicitudInd1"].ToString()))
                    this.SolicitudInd1.Value = Convert.ToBoolean(dr["SolicitudInd1"]);
                this.DeclFondos.Value = Convert.ToString(dr["DeclFondos"]);
                this.BR_Direccion.Value = Convert.ToString(dr["BR_Direccion"]);
                if (!string.IsNullOrWhiteSpace(dr["BR_Valor"].ToString()))
                    this.BR_Valor.Value = Convert.ToInt64(dr["BR_Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["BR_AfectacionFamInd"].ToString()))
                    this.BR_AfectacionFamInd.Value = Convert.ToBoolean(dr["BR_AfectacionFamInd"]);
                if (!string.IsNullOrWhiteSpace(dr["BR_HipotecaInd"].ToString()))
                    this.BR_HipotecaInd.Value = Convert.ToBoolean(dr["BR_HipotecaInd"]);
                if (!string.IsNullOrWhiteSpace(dr["BR_HipotecaNombre"].ToString()))
                    this.BR_HipotecaNombre.Value = Convert.ToString(dr["BR_HipotecaNombre"]);
                if (!string.IsNullOrWhiteSpace(dr["VE_Marca"].ToString()))
                    this.VE_Marca.Value = Convert.ToString(dr["VE_Marca"]);
                this.VE_Clase.Value = Convert.ToString(dr["VE_Clase"]);
                if (!string.IsNullOrWhiteSpace(dr["VE_Modelo"].ToString()))
                    this.VE_Modelo.Value = Convert.ToInt32(dr["VE_Modelo"]);
                this.VE_Placa.Value = Convert.ToString(dr["VE_Placa"]);
                if (!string.IsNullOrWhiteSpace(dr["VE_PignoradoInd"].ToString()))
                    this.VE_PignoradoInd.Value = Convert.ToBoolean(dr["VE_PignoradoInd"]);
                this.VE_PignoradoNombre.Value = Convert.ToString(dr["VE_PignoradoNombre"]);
                if (!string.IsNullOrWhiteSpace(dr["VE_Valor"].ToString()))
                    this.VE_Valor.Value = Convert.ToInt32(dr["VE_Valor"]);
                this.UsuarioDigita.Value = Convert.ToString(dr["UsuarioDigita"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaDigita"].ToString()))
                    this.FechaDigita.Value = Convert.ToDateTime(dr["FechaDigita"]);
                this.UsuarioVerifica.Value = Convert.ToString(dr["UsuarioVerifica"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaVerifica"].ToString()))
                    this.FechaVerifica.Value = Convert.ToDateTime(dr["FechaVerifica"]);
                this.UsuarioConfirma.Value = Convert.ToString(dr["UsuarioConfirma"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaConfirma"].ToString()))
                    this.FechaConfirma.Value = Convert.ToDateTime(dr["FechaConfirma"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);

                if (!string.IsNullOrWhiteSpace(dr["Sexo"].ToString()))
                    this.Sexo.Value = Convert.ToByte(dr["Sexo"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoCivil"].ToString()))
                    this.EstadoCivil.Value = Convert.ToByte(dr["EstadoCivil"]);

                if (!string.IsNullOrWhiteSpace(dr["DataCreditoDireccion"].ToString()))
                    this.DataCreditoDireccion.Value = Convert.ToByte(dr["DataCreditoDireccion"]);

                if (!string.IsNullOrWhiteSpace(dr["DataCreditoTelefono"].ToString()))
                    this.DataCreditoTelefono.Value = Convert.ToByte(dr["DataCreditoTelefono"]);

                if (!string.IsNullOrWhiteSpace(dr["DataCreditoCelular"].ToString()))
                    this.DataCreditoCelular.Value = Convert.ToByte(dr["DataCreditoCelular"]);

                if (!string.IsNullOrWhiteSpace(dr["DataCreditoCorreo"].ToString()))
                    this.DataCreditoCorreo.Value = Convert.ToByte(dr["DataCreditoCorreo"]);   

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccSolicitudDatosPersonales()
        {
            this.InitCols();
            this.VlrActivos.Value = 0;
            this.VlrPasivos.Value = 0;
            this.VlrPatrimonio.Value = 0;
            this.VlrEgresosMes.Value = 0;
            this.VlrIngresosMes.Value = 0;
            this.VlrIngresosNoOpe.Value = 0;
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.Version = new UDTSQL_int();
            this.TipoPersona = new UDTSQL_tinyint();
            this.TerceroID = new UDT_TerceroID();
            this.TerceroDocTipoID = new UDT_TerceroTipoID();
            this.FechaExpDoc= new UDTSQL_smalldatetime();
            this.CiudadExpDoc= new UDT_LugarGeograficoID();
            this.FechaNacimiento= new UDTSQL_smalldatetime();
            this.Descriptivo= new UDT_DescriptivoUnFormat();
            this.ApellidoPri= new UDT_DescriptivoUnFormat();
            this.ApellidoSdo= new UDT_DescriptivoUnFormat();
            this.NombrePri= new UDT_DescriptivoUnFormat();
            this.NombreSdo= new UDT_DescriptivoUnFormat();

            this.Celular1 = new UDTSQL_char(15);
            this.Celular2 = new UDTSQL_char(15);
            this.CorreoElectronico = new UDTSQL_char(60);
            this.DirResidencia= new UDT_DescriptivoUnFormat();
            this.BarrioResidencia=new UDTSQL_char(50);
            this.CiudadResidencia=new UDT_LugarGeograficoID();
            this.AntResidencia=new UDTSQL_tinyint();
            this.TipoVivienda=new UDTSQL_tinyint();
            this.TelResidencia = new UDTSQL_char(15);
            this.LugarTrabajo = new UDTSQL_char(60);
            this.DirTrabajo = new UDT_DescriptivoUnFormat();
            this.BarrioTrabajo = new UDTSQL_char(50);
            this.CiudadTrabajo = new UDT_LugarGeograficoID();
            this.TelTrabajo = new UDTSQL_char(20);
            this.Cargo = new UDTSQL_char(40);
            this.AntTrabajo = new UDTSQL_tinyint();
            this.TipoContrato = new UDTSQL_tinyint();
            this.EPS = new UDTSQL_char(20);
            this.Personascargo = new UDTSQL_tinyint();
            this.Conyugue = new UDTSQL_char(15);
            this.NombreConyugue = new UDTSQL_char(100);
            this.ActConyugue = new UDTSQL_char(40);
            this.AntConyugue = new UDTSQL_tinyint();
            this.EmpresaConyugue = new UDTSQL_varchar(50);
            this.DirResConyugue = new UDT_DescriptivoUnFormat();
            this.TelefonoConyugue = new UDTSQL_char(20);
            this.CelularConyugue = new UDTSQL_char(20);
            this.NombreReferencia1 = new UDTSQL_char(100);
            this.RelReferencia1 = new UDTSQL_char(50);
            this.TipoReferencia1 = new UDTSQL_tinyint();
            this.DirReferencia1=new UDT_DescriptivoUnFormat();
            this.BarrioReferencia1 = new UDTSQL_char(50);
            this.TelefonoReferencia1 = new UDTSQL_char(20);
            this.CelularReferencia1 = new UDTSQL_char(20);

            this.NombreReferencia2 = new UDTSQL_char(100);
            this.RelReferencia2 = new UDTSQL_char(50);
            this.TipoReferencia2 = new UDTSQL_tinyint();
            this.DirReferencia2 = new UDT_DescriptivoUnFormat();
            this.BarrioReferencia2 = new UDTSQL_char(50);
            this.TelefonoReferencia2 = new UDTSQL_char(20);
            this.CelularReferencia2 = new UDTSQL_char(20);
            this.VlrActivos=new UDT_Valor();
            this.VlrPasivos=new UDT_Valor();
            this.VlrPatrimonio=new UDT_Valor();
            this.VlrEgresosMes=new UDT_Valor();
            this.VlrIngresosMes=new UDT_Valor();
            this.VlrIngresosNoOpe=new UDT_Valor();
            this.DescrOtrosIng=new UDT_DescriptivoUnFormat();
            this.DescrOtrosBinenes=new UDT_DescriptivoUnFormat();
            this.EntCredito1=new UDTSQL_varchar(50);
            this.Plazo1=new UDTSQL_tinyint();
            this.Saldo1=new UDT_Valor();
            this.EntCredito2=new UDTSQL_varchar(50);
            this.Plazo2=new UDTSQL_tinyint();
            this.Saldo2=new UDT_Valor();
            this.SolicitudInd1=new UDT_SiNo();
            this.DeclFondos=new UDT_DescriptivoUnFormat();
            this.BR_Direccion=new UDTSQL_varchar(60);
            this.BR_Valor=new UDT_Valor();
            this.BR_AfectacionFamInd=new UDT_SiNo();
            this.BR_HipotecaInd = new UDT_SiNo();
            this.BR_HipotecaNombre=new UDTSQL_varchar(60);
            this.VE_Marca=new UDTSQL_varchar(30);
            this.VE_Clase=new UDTSQL_varchar(30);
            this.VE_Modelo=new UDTSQL_int();
            this.VE_Placa=new UDTSQL_char(6);
            this.VE_PignoradoInd=new UDT_SiNo();
            this.VE_PignoradoNombre=new UDTSQL_varchar(60);
            this.VE_Valor=new UDT_Valor();
            this.UsuarioDigita=new UDT_UsuarioID();
            this.FechaDigita=new UDTSQL_smalldatetime();
            this.UsuarioVerifica=new UDT_UsuarioID();
            this.FechaVerifica=new UDTSQL_smalldatetime();
            this.UsuarioConfirma=new UDT_UsuarioID();
            this.FechaConfirma=new UDTSQL_smalldatetime();
            this.Consecutivo=new UDT_Consecutivo();
            this.Sexo= new UDTSQL_tinyint();
            this.EstadoCivil = new UDTSQL_tinyint();
            this.DataCreditoCelular = new UDTSQL_tinyint();
            this.DataCreditoCorreo = new UDTSQL_tinyint();
            this.DataCreditoDireccion = new UDTSQL_tinyint();
            this.DataCreditoTelefono = new UDTSQL_tinyint();
          }

        #endregion

        #region Propiedades
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        public UDTSQL_int Version { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint TipoPersona { get; set; }
        
        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_TerceroTipoID TerceroDocTipoID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaExpDoc { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID CiudadExpDoc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaNacimiento { get; set; }

        [DataMember]
        public UDT_DescriptivoUnFormat Descriptivo { get; set; }

        [DataMember]
        public UDT_DescriptivoUnFormat ApellidoPri{ get; set; }

        [DataMember]
        public UDT_DescriptivoUnFormat ApellidoSdo{ get; set; }
      
        [DataMember]
        public UDT_DescriptivoUnFormat NombrePri{ get; set; }
        
        [DataMember]
        public UDT_DescriptivoUnFormat NombreSdo{ get; set; }

        [DataMember]
        public UDTSQL_char Celular1 { get; set; }

        [DataMember]
        public UDTSQL_char Celular2 { get; set; }       

        [DataMember]
        public UDTSQL_char CorreoElectronico { get; set; }       

        [DataMember]
        public UDT_DescriptivoUnFormat DirResidencia{ get; set; }       
        
        [DataMember]
        public UDTSQL_char BarrioResidencia{ get; set; }       
        
        [DataMember]
        public UDT_LugarGeograficoID CiudadResidencia{ get; set; }       
        
        [DataMember]
        public UDTSQL_tinyint AntResidencia{ get; set; }       
	
        [DataMember]
        public UDTSQL_tinyint TipoVivienda{ get; set; }       
	        
	    [DataMember]
        public UDTSQL_char TelResidencia{ get; set; }       
	
        [DataMember]
        public UDTSQL_char LugarTrabajo{ get; set; }       
	
	    [DataMember]
        public UDT_DescriptivoUnFormat DirTrabajo{ get; set; }       
	
	    [DataMember]
        public UDTSQL_char BarrioTrabajo{ get; set; }       
	
	    [DataMember]
        public UDT_LugarGeograficoID CiudadTrabajo{ get; set; }       
	
        [DataMember]
        public UDTSQL_char TelTrabajo{ get; set; }       

        [DataMember]
        public UDTSQL_char Cargo{ get; set; }       
	
	    [DataMember]
        public UDTSQL_tinyint AntTrabajo{ get; set; }       
	
		[DataMember]
        public UDTSQL_tinyint TipoContrato	{ get; set; }       

        [DataMember]
        public UDTSQL_char EPS	{ get; set; }       

        [DataMember]
        public UDTSQL_tinyint Personascargo{ get; set; }       

        [DataMember]
        public UDTSQL_char Conyugue{ get; set; }       

        [DataMember]
        public UDTSQL_char NombreConyugue{ get; set; }       

	    [DataMember]
        public UDTSQL_char ActConyugue{ get; set; }       

        [DataMember]
        public UDTSQL_tinyint AntConyugue{ get; set; }

        [DataMember]
        public UDTSQL_varchar EmpresaConyugue { get; set; }       

	
        [DataMember]
        public UDT_DescriptivoUnFormat DirResConyugue{ get; set; }  
	    [DataMember]
        public UDTSQL_char TelefonoConyugue{ get; set; }       
        [DataMember]
        public UDTSQL_char CelularConyugue{ get; set; }         
        [DataMember]
        public UDTSQL_char NombreReferencia1{ get; set; }       
        [DataMember]
        public UDTSQL_char RelReferencia1{ get; set; }
        [DataMember]
        public UDTSQL_tinyint TipoReferencia1{ get; set; }       
        [DataMember]
        public UDT_DescriptivoUnFormat DirReferencia1{ get; set; }
        [DataMember]
        public UDTSQL_char BarrioReferencia1{ get; set; }
        [DataMember]
        public UDTSQL_char TelefonoReferencia1{ get; set; }
        [DataMember]
        public UDTSQL_char CelularReferencia1{ get; set; }
        [DataMember]
        public UDTSQL_char NombreReferencia2{ get; set; }
        [DataMember]
        public UDTSQL_char RelReferencia2{ get; set; }
        [DataMember]
        public UDTSQL_tinyint TipoReferencia2{ get; set; }       
        [DataMember]
        public UDT_DescriptivoUnFormat DirReferencia2{ get; set; }  

        [DataMember]
        public UDTSQL_char BarrioReferencia2{ get; set; }
        [DataMember]
        public UDTSQL_char TelefonoReferencia2{ get; set; }
        [DataMember]
        public UDTSQL_char CelularReferencia2{ get; set; }
	    [DataMember]
        public UDT_Valor VlrActivos{ get; set; }
        [DataMember]
        public UDT_Valor VlrPasivos{ get; set; }

        [DataMember]
        public UDT_Valor VlrPatrimonio{ get; set; }
        [DataMember]
        public UDT_Valor VlrEgresosMes{ get; set; }
        [DataMember]
        public UDT_Valor VlrIngresosMes{ get; set; }

        [DataMember]
        public UDT_Valor VlrIngresosNoOpe{ get; set; }

        [DataMember]
        public UDT_DescriptivoUnFormat DescrOtrosIng{ get; set; }  

        [DataMember]
        public UDT_DescriptivoUnFormat DescrOtrosBinenes{ get; set; }  

	    [DataMember]
        public UDTSQL_varchar EntCredito1{ get; set; }  
	
	
        [DataMember]
        public UDTSQL_tinyint Plazo1{ get; set; }  

   	
        [DataMember]
        public UDT_Valor Saldo1{ get; set; }  
	
        [DataMember]
        public UDTSQL_varchar EntCredito2{ get; set; }  
	
        [DataMember]
        public UDTSQL_tinyint Plazo2{ get; set; }  
	
        [DataMember]
        public UDT_Valor Saldo2{ get; set; }   
	
        [DataMember]
        public UDT_SiNo SolicitudInd1{ get; set; }  

        [DataMember]
        public UDT_DescriptivoUnFormat DeclFondos	{ get; set; }  
	
        [DataMember]
        public UDTSQL_varchar BR_Direccion{ get; set; }  
	
        [DataMember]
        public UDT_Valor  BR_Valor{ get; set; }  
	
        [DataMember]
        public UDT_SiNo BR_AfectacionFamInd{ get; set; }  

        [DataMember]
        public UDT_SiNo BR_HipotecaInd	{ get; set; }  
	
        [DataMember]
        public UDTSQL_varchar BR_HipotecaNombre{ get; set; }  
	
        [DataMember]
        public UDTSQL_varchar VE_Marca{ get; set; }  
	
        [DataMember]
        public UDTSQL_varchar VE_Clase{ get; set; }  
	
        [DataMember]
        public UDTSQL_int VE_Modelo{ get; set; }  
	
        [DataMember]
        public UDTSQL_char VE_Placa{ get; set; }  
	
        [DataMember]
        public UDT_SiNo VE_PignoradoInd{ get; set; }  
	
        [DataMember]
        public UDTSQL_varchar VE_PignoradoNombre{ get; set; }  
	
        [DataMember]
        public UDT_Valor VE_Valor{ get; set; }  

        [DataMember]
        public UDT_UsuarioID UsuarioDigita	{ get; set; }  
	
        [DataMember]
        public UDTSQL_smalldatetime FechaDigita{ get; set; }  

        [DataMember]
        public UDT_UsuarioID UsuarioVerifica	{ get; set; }  

        [DataMember]
        public UDTSQL_smalldatetime FechaVerifica	{ get; set; }  

        [DataMember]
        public UDT_UsuarioID UsuarioConfirma	{ get; set; }  
	
        [DataMember]
        public UDTSQL_smalldatetime FechaConfirma{ get; set; }  
	
        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDTSQL_tinyint Sexo { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoCivil { get; set; }

        [DataMember]
        public UDTSQL_tinyint DataCreditoDireccion { get; set; }
        [DataMember]
        public UDTSQL_tinyint DataCreditoTelefono { get; set; }
        [DataMember]
        public UDTSQL_tinyint DataCreditoCelular { get; set; }
        [DataMember]
        public UDTSQL_tinyint DataCreditoCorreo { get; set; }

        #endregion
    }
}
