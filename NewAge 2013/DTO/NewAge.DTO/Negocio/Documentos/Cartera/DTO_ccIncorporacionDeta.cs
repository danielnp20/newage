using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class comprobante para aprobacion:
    /// Models DTO_ccIncorporacionDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccIncorporacionDeta
    {
        #region DTO_ccIncorporacionDeta

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        public DTO_ccIncorporacionDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocSolicitud"].ToString())) 
                    this.NumDocSolicitud.Value = Convert.ToInt32(dr["NumDocSolicitud"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocCredito"].ToString())) 
                    this.NumDocCredito.Value = Convert.ToInt32(dr["NumDocCredito"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroINC"].ToString()))
                    this.NumeroINC.Value = Convert.ToByte(dr["NumeroINC"]);//                
                this.PagaduriaID.Value = dr["PagaduriaID"].ToString();
                this.FechaNovedad.Value = Convert.ToDateTime(dr["FechaNovedad"]);
                this.FechaCuota1.Value = Convert.ToDateTime(dr["FechaCuota1"]);
                this.ValorCuota.Value = Convert.ToDecimal(dr["ValorCuota"]);
                if (!string.IsNullOrWhiteSpace(dr["PlazoINC"].ToString()))
                    this.PlazoINC.Value = Convert.ToInt16(dr["PlazoINC"]);//                
                this.NumDocNomina.Value = Convert.ToInt32(dr["NumDocNomina"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorNomina"].ToString())) 
                    this.ValorNomina.Value = Convert.ToDecimal(dr["ValorNomina"]);
                if (!string.IsNullOrWhiteSpace(dr["IncPreviaInd"].ToString()))
                    this.IncPreviaInd.Value = Convert.ToBoolean(dr["IncPreviaInd"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoNovedad"].ToString()))
                    this.TipoNovedad.Value = Convert.ToByte(dr["TipoNovedad"]);
                if (!string.IsNullOrWhiteSpace(dr["IndInconsistencia"].ToString())) 
                    this.IndInconsistencia.Value = Convert.ToBoolean(dr["IndInconsistencia"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoInconsistencia"].ToString())) 
                    this.TipoInconsistencia.Value = Convert.ToByte(dr["TipoInconsistencia"]);
                this.InconsistenciaIncID.Value = dr["InconsistenciaIncID"].ToString();
                this.NovedadIncorporaID.Value = dr["NovedadIncorporaID"].ToString();
                this.Observacion.Value = dr["Observacion"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["OrigenDato"].ToString()))
                    this.OrigenDato.Value = Convert.ToByte(dr["OrigenDato"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaTransmite"].ToString()))
                    this.FechaTransmite.Value = Convert.ToDateTime(dr["FechaTransmite"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccIncorporacionDeta()
        {
            this.InitCols();
            this.ValorAnticipadoInvalid.Value = false;
            this.CanceladoInd.Value = false;
            this.EstadoCuentaInd.Value = false;
            this.EstadoClienteInvalid.Value = false;
            this.NominaExist.Value = false;
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumDocSolicitud = new UDT_Consecutivo();
            this.NumDocCredito = new UDT_Consecutivo();
            this.NumeroINC = new UDTSQL_tinyint();
            this.PagaduriaID = new UDT_PagaduriaID();
            this.FechaNovedad = new UDTSQL_smalldatetime();
            this.FechaCuota1 = new UDTSQL_smalldatetime();
            this.ValorCuota = new UDT_Valor();
            this.PlazoINC = new UDTSQL_smallint();
            this.NumDocNomina = new UDT_Consecutivo();
            this.ValorNomina = new UDT_Valor();
            this.IncPreviaInd = new UDT_SiNo();
            this.TipoNovedad = new UDTSQL_tinyint();
            this.IndInconsistencia = new UDT_SiNo();
            this.TipoInconsistencia = new UDTSQL_tinyint();
            this.InconsistenciaIncID = new UDT_InconsistenciaIncID();
            this.NovedadIncorporaID = new UDTSQL_char(5);
            this.Observacion = new UDT_DescripTExt();
            this.OrigenDato = new UDTSQL_tinyint();
            this.FechaTransmite = new UDTSQL_smalldatetime();
            //Campos Adicionales
            this.MensajeError = new UDT_DescriptivoUnFormat();
            this.FechaNomina = new UDTSQL_smalldatetime();
            this.ClienteID = new UDT_ClienteID();
            this.CodEmpleado = new UDTSQL_char(10);
            this.Nombre = new UDTSQL_char(2000);
            this.Libranza = new UDT_LibranzaID();
            this.PosicionImportacion = new UDTSQL_int();
            this.ValorAnticipadoInvalid = new UDT_SiNo();
            this.CanceladoInd = new UDT_SiNo();
            this.EstadoCuentaInd = new UDT_SiNo();
            this.EstadoClienteInvalid = new UDT_SiNo();
            this.NominaExist = new UDT_SiNo();
            this.Codeudor = new UDT_TerceroID();
        }
        
        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumDocSolicitud { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint NumeroINC { get; set; }//

        [DataMember]
        [NotImportable]
        public UDT_PagaduriaID PagaduriaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaNovedad { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaCuota1 { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smallint PlazoINC { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumDocNomina { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor ValorNomina { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo IncPreviaInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint TipoNovedad { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo IndInconsistencia { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint TipoInconsistencia { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_InconsistenciaIncID InconsistenciaIncID { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char NovedadIncorporaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint OrigenDato { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaTransmite { get; set; }

        #endregion

        #region Campos Adicionales

        [DataMember]
        public UDTSQL_smalldatetime FechaNomina { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDTSQL_char CodEmpleado { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char Nombre { get; set; }

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescriptivoUnFormat MensajeError { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_int PosicionImportacion { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo ValorAnticipadoInvalid { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo CanceladoInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo EstadoCuentaInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo EstadoClienteInvalid { get; set; }

        [DataMember]
        [NotImportable]
        public int indexCuota { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo NominaExist { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_TerceroID Codeudor { get; set; }

        [DataMember]
        public UDT_Valor ValorCuota { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        #endregion

    }
}
