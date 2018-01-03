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
    public class DTO_VistaQ_ccCesiones
    {
        #region DTO_ccCreditoLiquida

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        public DTO_VistaQ_ccCesiones(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                //this.Tipoidentificacion.Value = Convert.ToByte(dr["Tipoidentificacion"]);
                //this.Numeroidentificacion.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.ciudadCedula.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.FechaExpedicion.Value = Convert.ToDateTime(dr["Tipoidentificacion"]);
                //this.PrimerApellido.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.SegundoApellido.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Nombre.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.ciudadNacimiento.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.FechaNacimiento.Value = Convert.ToDateTime(dr["Tipoidentificacion"]);
                //this.DireccionResidencia.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.CiudadResidencia.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Telefono.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Barrio.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Celular.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Sexo.Value = Convert.ToByte(dr["Tipoidentificacion"]);
                //this.Estrato.Value = Convert.ToByte(dr["Tipoidentificacion"]);
                //this.DireccionLaboral.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.ciudadLaboral.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.TelefonoLaboral.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Empresa.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.email.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.TipoCliente.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Ocupacion.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Profesion.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.NombreProfesion.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Cargo.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Antiguedad.Value = Convert.ToByte(dr["Tipoidentificacion"]);
                //this.EstadoCivil.Value = Convert.ToByte(dr["Tipoidentificacion"]);
                //this.Devengado.Value = Convert.ToDecimal(dr["Tipoidentificacion"]);
                //this.descuento.Value = Convert.ToDecimal(dr["Tipoidentificacion"]);
                //this.Disponible.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Capacidad.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.PerCargo.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.dVinculac.Value = Convert.ToDateTime(dr["Tipoidentificacion"]);
                //this.NomFamiliar1.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.ParFamiliar1.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.dirFamiliar1.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.barreffam1.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.ciureffam1.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.telreffam1.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.NomFamiliar2.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.ParFamiliar2.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.dirFamiliar2.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.barreffam2.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.ciureffam2.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.telreffam2.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.NomrefPers1.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.ParRefPers1.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.DirParPers1.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.BarrRefPers1.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.CiuRefPers1.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.TelRefPers1.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.NomrefPers2.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.ParRefPers2.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.DirParPers2.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.BarrRefPers2.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.CiuRefPers2.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.TelRefPers2.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Libranza.Value = Convert.ToByte(dr["Tipoidentificacion"]);
                //this.FechaLiq.Value = Convert.ToDateTime(dr["Tipoidentificacion"]);
                //this.Cooperativa.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.idPagaduria.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.NomPagaduria.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.NitPagaduria.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Cuotas.Value = Convert.ToByte(dr["Tipoidentificacion"]);
                //this.VlrCredito.Value = Convert.ToDecimal(dr["Tipoidentificacion"]);
                //this.VlrLibranza.Value = Convert.ToDecimal(dr["Tipoidentificacion"]);
                //this.VrGiro.Value = Convert.ToDecimal(dr["Tipoidentificacion"]);
                //this.VrCuota.Value = Convert.ToDecimal(dr["Tipoidentificacion"]);
                //this.PagoCesion.Value = Convert.ToDecimal(dr["Tipoidentificacion"]);
                //this.UtiCesion.Value = Convert.ToDecimal(dr["Tipoidentificacion"]);
                //this.VlrInteres.Value = Convert.ToDecimal(dr["Tipoidentificacion"]);
                //this.Acierta.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.FechaAcierta.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.AfiMes.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Afitot.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Afipor.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.OdoMes.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.OdoTot.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.OdoPor.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.tecMes.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.TecTot.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.TecPor.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.PagMes.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.PagTot.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.PagPor.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.avaMes.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.AvaTot.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.AvaPor.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.EstMes.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.EstTot.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.EstPor.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.ApoMes.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.ApoTot.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.ApoPor.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.GarMes.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.GarTot.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.GarPor.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.AsiMes.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.AsiTot.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.AsisDear.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.DrLegal.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.AsiPor.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Edad.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Estado.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Incorporado.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.Recaudo.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.IdAsesor.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.NombreAsesor.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.NitAseor.Value = Convert.ToString(dr["Tipoidentificacion"]);
                //this.CodEstrato.Value = Convert.ToString(dr["Tipoidentificacion"]);
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
        public DTO_VistaQ_ccCesiones()
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
            this.Oferta = new UDT_DocTerceroID();
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.Mes = new UDTSQL_int();
            this.CompradorCod = new UDT_CompradorCarteraID();
            this.NombreComprador = new UDT_Descriptivo();
            this.TipoDocu = new UDTSQL_char(2);
            this.Cedula = new UDT_TerceroID();
            this.CiudadCed = new UDT_LugarGeograficoID();
            this.FecExped = new UDTSQL_smalldatetime();
            this.PrimApellido = new UDT_DescripTBase();
            this.SegApellido = new UDT_DescripTBase();
            this.CiudadNac = new UDT_LugarGeograficoID();
            this.FechaNac = new UDTSQL_smalldatetime();
            this.CiudadRes = new UDT_LugarGeograficoID();
            this.TelefonoRes = new UDTSQL_char(30);
            this.Celular = new UDTSQL_char(20);
            this.Sexo = new UDTSQL_char(1);
            this.Estrato = new UDTSQL_tinyint();
            this.CiudadLab = new UDT_LugarGeograficoID();
            this.TelefonoLab = new UDTSQL_char(20);
            this.Empresa = new UDTSQL_char(20);
            this.Antiguedad = new UDTSQL_tinyint();
            this.EstadoCivil = new UDTSQL_char(1);
            this.Devengado = new UDT_Valor();
            this.Descuentos = new UDT_Valor();
            this.Disponible = new UDT_Valor();
            this.FecVinculado = new UDTSQL_smalldatetime();
            this.Libranza = new UDT_DocTerceroID();
            this.FechaLiquida = new UDTSQL_smalldatetime();
            this.CooperativaID = new UDT_CodigoGrl5();
            this.PagadID = new UDT_PagaduriaID();
            this.PagadNombre = new UDT_DescripTBase();
            this.PagadNit = new UDT_TerceroID();
            this.Cuotas = new  UDTSQL_smallint();
            this.VlrCredito = new UDT_Valor();
            this.VlrLibranza = new UDT_Valor();
            this.VlrGiro = new UDT_Valor();
            this.VlrCuota = new UDT_Valor();
            this.PagoCesion = new  UDT_Valor();
            this.UtilCesion = new UDT_Valor();
            this.VlrInteres = new UDT_Valor();
            this.Acierta = new UDTSQL_char(20);
            this.FechaAcierta = new UDTSQL_smalldatetime();
            this.Edad = new UDTSQL_tinyint();
            this.Estado = new UDTSQL_char(15);
            this.MesIncorpora = new UDTSQL_tinyint();
            this.MesNomina = new UDTSQL_tinyint();
            this.LineaCredito = new UDT_LineaCreditoID();
            this.CompradorCartera = new UDT_CompradorCarteraID();
            this.CodAsesor = new UDT_AsesorID();
            this.NombreAsesor = new UDT_DescripTBase();
            this.NitAsesor = new UDT_TerceroID();
            this.CtasVenta = new UDTSQL_int();
            this.VlrVenta = new UDT_Valor();
            this.TipoCredito = new UDT_CodigoGrl5();
            this.VlrRecogesaldo = new UDT_Valor();
            this.VlrCompraCar = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_DocTerceroID Oferta { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDTSQL_int Mes { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorCod { get; set; }

        [DataMember]
        public UDT_Descriptivo NombreComprador { get; set; }

        [DataMember]
        public UDTSQL_char TipoDocu { get; set; } //2

        [DataMember]
        public UDT_TerceroID Cedula { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID CiudadCed { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FecExped { get; set; }

        [DataMember]
        public UDT_DescripTBase PrimApellido { get; set; }

        [DataMember]
        public UDT_DescripTBase SegApellido { get; set; }

        [DataMember]
        public UDT_DescripTBase PriNombre { get; set; }

        [DataMember]
        public UDT_DescripTBase SegNombre { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID CiudadNac { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaNac { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID CiudadRes { get; set; }

        [DataMember]
        public UDTSQL_char TelefonoRes { get; set; }

        [DataMember]
        public UDTSQL_char Celular { get; set; }

        [DataMember]
        public UDTSQL_char Sexo { get; set; } 

        [DataMember]
        public UDTSQL_tinyint Estrato { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID CiudadLab { get; set; }

        [DataMember]
        public UDTSQL_char TelefonoLab { get; set; }

        [DataMember]
        public UDTSQL_char Empresa { get; set; }

        [DataMember]
        public UDTSQL_tinyint Antiguedad { get; set; }

        [DataMember]
        public UDTSQL_char EstadoCivil { get; set; } 
        
        [DataMember]
        public UDT_Valor Devengado { get; set; }

        [DataMember]
        public UDT_Valor Descuentos { get; set; }

        [DataMember]
        public UDT_Valor Disponible { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FecVinculado { get; set; }

        [DataMember]
        public UDT_DocTerceroID Libranza { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLiquida { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 CooperativaID { get; set; }

        [DataMember]
        public UDT_PagaduriaID PagadID { get; set; }

        [DataMember]
        public UDT_DescripTBase PagadNombre { get; set; }

        [DataMember]
        public UDT_TerceroID PagadNit { get; set; }

        [DataMember]
        public UDTSQL_smallint Cuotas { get; set; }

        [DataMember]
        public UDT_Valor VlrCredito { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDT_Valor VlrGiro { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor PagoCesion { get; set; }

        [DataMember]
        public UDT_Valor UtilCesion { get; set; }

        [DataMember]
        public UDT_Valor VlrInteres { get; set; }

        [DataMember]
        public UDTSQL_char Acierta { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaAcierta { get; set; }

        [DataMember]
        public UDTSQL_tinyint Edad { get; set; }

        [DataMember]
        public UDTSQL_char Estado { get; set; } 

        [DataMember]
        public UDTSQL_tinyint MesIncorpora { get; set; }

        [DataMember]
        public UDTSQL_tinyint MesNomina { get; set; }

        [DataMember]
        public UDT_LineaCreditoID LineaCredito { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorCartera { get; set; }

        [DataMember]
        public UDT_AsesorID CodAsesor { get; set; }

        [DataMember]
        public UDT_DescripTBase NombreAsesor { get; set; }
        
        [DataMember]
        public UDT_TerceroID NitAsesor { get; set; }

        [DataMember]
        public UDTSQL_int CtasVenta { get; set; }

        [DataMember]
        public UDT_Valor VlrVenta { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 TipoCredito { get; set; }

        [DataMember]
        public UDT_Valor VlrRecogesaldo { get; set; }

        [DataMember]
        public UDT_Valor VlrCompraCar { get; set; }

        #endregion

    }
}
