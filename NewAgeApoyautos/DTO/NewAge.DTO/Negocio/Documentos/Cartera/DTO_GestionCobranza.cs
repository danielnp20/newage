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
    /// Models DTO_GestionCobranza
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_GestionCobranza
    {
        #region DTO_GestionCobranza

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_GestionCobranza(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocControlIncumplido"].ToString()))
                    this.NumDocControlIncumplido.Value = Convert.ToInt32(dr["NumDocControlIncumplido"]);
                if (!string.IsNullOrWhiteSpace(dr["ConsEstadoIncumplido"].ToString()))
                    this.ConsEstadoIncumplido.Value = Convert.ToInt32(dr["ConsEstadoIncumplido"]);
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.ClienteDesc.Value = dr["ClienteDesc"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.Libranza.Value = dr["Libranza"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Plazo"].ToString()))
                    this.Plazo.Value = Convert.ToInt16(dr["Plazo"]);
                if (!string.IsNullOrWhiteSpace(dr["CtasVencidas"].ToString()))
                    this.CtasVencidas.Value = Convert.ToDecimal(dr["CtasVencidas"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaMora"].ToString()))
                    this.FechaMora.Value = Convert.ToDateTime(dr["FechaMora"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasMora"].ToString()))
                    this.DiasMora.Value = Convert.ToInt32(dr["DiasMora"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoVencido"].ToString()))
                    this.SaldoVencido.Value = Convert.ToDecimal(dr["SaldoVencido"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrIntMora"].ToString()))
                    this.VlrIntMora.Value = Convert.ToDecimal(dr["VlrIntMora"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPrejuridico"].ToString()))
                    this.VlrPrejuridico.Value = Convert.ToDecimal(dr["VlrPrejuridico"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrOtros"].ToString()))
                    this.VlrOtros.Value = Convert.ToDecimal(dr["VlrOtros"]);
                this.TotalLibranza.Value = Convert.ToDecimal(dr["TotalLibranza"]);
                if (!string.IsNullOrWhiteSpace(dr["Numero1"].ToString()))
                    this.Numero1.Value = Convert.ToInt32(dr["Numero1"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaCompromiso"].ToString()))
                    this.FechaCompromiso.Value = Convert.ToDateTime(dr["FechaCompromiso"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor1"].ToString()))
                    this.Valor1.Value = Convert.ToDecimal(dr["Valor1"]);
                this.ObservacionCompromiso.Value = dr["ObservacionCompromiso"].ToString();
                this.Observaciones.Value = dr["Observaciones"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NumSolicitud"].ToString()))
                    this.NumSolicitud.Value = Convert.ToInt32(dr["NumSolicitud"]);
                this.ObservacionHistoria.Value = dr["ObservacionHistoria"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CumplidoInd"].ToString()))
                    this.CumplidoInd.Value = Convert.ToBoolean(dr["CumplidoInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_GestionCobranza()
        {
            this.InitCols();
            this.CumplidoInd.Value = false;
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.Consecutivo = new UDT_Consecutivo();
            this.NumDocControlIncumplido = new UDT_Consecutivo();
            this.ConsEstadoIncumplido = new UDT_Consecutivo();
            this.ClienteID = new UDT_ClienteID();
            this.ClienteDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_TerceroID();
            this.Libranza = new UDT_DocTerceroID();
            this.Plazo = new UDTSQL_smallint();
            this.CtasVencidas = new UDT_Cantidad();
            this.FechaMora = new UDTSQL_smalldatetime();
            this.DiasMora = new UDTSQL_int();
            this.SaldoVencido = new UDT_Valor();
            this.VlrIntMora = new UDT_Valor();
            this.VlrPrejuridico = new UDT_Valor();
            this.VlrOtros = new UDT_Valor();
            this.TotalLibranza = new UDT_Valor();
            this.Numero1 = new UDTSQL_int();
            this.FechaCompromiso = new UDTSQL_smalldatetime();
            this.Valor1 = new UDT_Valor();
            this.ObservacionCompromiso = new UDT_DescripUnFormat();
            this.Observaciones = new UDT_DescripTExt();
            this.NumSolicitud = new UDT_Consecutivo();
            this.CumplidoInd = new UDT_SiNo();
            this.ObservacionHistoria = new UDT_DescripUnFormat();
            this.Detalle = new List<DTO_GestionCobranza>();
            this.CodeudorDet = new List<DTO_CodeudorDet>();
            this.ActividadEstado = new DTO_InfoTarea();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; } 

        [DataMember]
        public UDT_Consecutivo NumDocControlIncumplido { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsEstadoIncumplido { get; set; }       

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo ClienteDesc { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DocTerceroID Libranza { get; set; }

        [DataMember]
        public UDTSQL_smallint Plazo { get; set; }

        [DataMember]
        public UDT_Cantidad CtasVencidas { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaMora { get; set; }

        [DataMember]
        public UDTSQL_int DiasMora { get; set; }

        [DataMember]
        public UDT_Valor SaldoVencido { get; set; }

        [DataMember]
        public UDT_Valor VlrIntMora { get; set; }

        [DataMember]
        public UDT_Valor VlrPrejuridico { get; set; }

        [DataMember]
        public UDT_Valor VlrOtros { get; set; }

        [DataMember]
        public UDT_Valor TotalLibranza { get; set; }

        [DataMember]
        public UDTSQL_int Numero1 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCompromiso { get; set; }

        [DataMember]
        public UDT_Valor Valor1 { get; set; }

        [DataMember]
        public UDT_DescripUnFormat ObservacionCompromiso { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDT_Consecutivo NumSolicitud { get; set; }

        [DataMember]
        public List<DTO_GestionCobranza> Detalle { get; set; }

        [DataMember]
        public List<DTO_CodeudorDet> CodeudorDet { get; set; }

        [DataMember]
        public UDT_SiNo CumplidoInd { get; set; }

        [DataMember]
        public UDT_DescripUnFormat ObservacionHistoria { get; set; }

        [DataMember]
        public DTO_InfoTarea ActividadEstado { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    public class DTO_CodeudorDet
    {
        #region DTO_CodeudorDet

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_CodeudorDet(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.ClienteDesc.Value = dr["ClienteDesc"].ToString();
                //this.NumSolicitud.Value = Convert.ToInt32(dr["NumSolicitud"]);
                this.ProfesionDes.Value = dr["ProfesionDes"].ToString();
                this.Telefono.Value = dr["Telefono"].ToString();
                this.Celular.Value = dr["Celular"].ToString();
                this.Correo.Value = dr["Correo"].ToString();
                this.ResidenciaDir.Value = dr["ResidenciaDir"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_CodeudorDet()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.ClienteID = new  UDT_ClienteID();
            this.ClienteDesc = new UDT_Descriptivo();
            this.NumSolicitud = new UDT_Consecutivo();
            this.NumDocCredito = new UDT_Consecutivo();
            this.ProfesionDes = new UDT_Descriptivo();
            this.Telefono = new UDTSQL_char(50);
            this.Celular = new UDTSQL_char(50);
            this.Correo = new UDTSQL_char(100);
            this.ResidenciaDir = new UDTSQL_char(100);
            this.Codeudor1 = new UDT_ClienteID();
            this.Codeudor2 = new UDT_ClienteID();
            this.Codeudor3 = new UDT_ClienteID();
            this.Codeudor4 = new UDT_ClienteID();
            this.Codeudor5 = new UDT_ClienteID();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo ClienteDesc { get; set; }

        [DataMember]
        public UDT_Consecutivo NumSolicitud { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        public UDT_Descriptivo ProfesionDes { get; set; }

        [DataMember]
        public UDTSQL_char Telefono { get; set; }

        [DataMember]
        public UDTSQL_char Celular { get; set; }

        [DataMember]
        public UDTSQL_char Correo { get; set; }

        [DataMember]
        public UDTSQL_char ResidenciaDir { get; set; }

        [DataMember]
        public UDT_ClienteID Codeudor1 { get; set; }

        [DataMember]
        public UDT_ClienteID Codeudor2 { get; set; }

        [DataMember]
        public UDT_ClienteID Codeudor3 { get; set; }

        [DataMember]
        public UDT_ClienteID Codeudor4 { get; set; }

        [DataMember]
        public UDT_ClienteID Codeudor5 { get; set; }

        #endregion
    }
}
