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
    /// Models DTO_ccReincorporacionDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccReincorporacionDeta
    {
        #region DTO_ccReincorporacionDeta

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        public DTO_ccReincorporacionDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.NumDocCredito.Value = Convert.ToInt32(dr["NumDocCredito"]);
                this.PeriodoNomina.Value = Convert.ToDateTime(dr["PeriodoNomina"]).Date;
                this.CentroPagoID.Value = dr["CentroPagoID"].ToString();
                this.CentroPagoModificaID.Value = dr["CentroPagoModificaID"].ToString();
                this.CambioCentroPagoIND.Value = Convert.ToBoolean(dr["CambioCentroPagoIND"]);
                this.NumeroINC.Value = Convert.ToByte(dr["NumeroINC"]);
                this.NumeroINCIni.Value = Convert.ToByte(dr["NumeroINC"]);
                if (!string.IsNullOrWhiteSpace(dr["PlazoINC"].ToString()))
                    this.PlazoINC.Value = Convert.ToByte(dr["PlazoINC"]);
                this.ValorCuota.Value = Convert.ToDecimal(dr["ValorCuota"]);
                this.FechaCuota1.Value = Convert.ToDateTime(dr["FechaCuota1"]);
                this.EstadoCruce.Value = Convert.ToByte(dr["EstadoCruce"]);
                this.Observacion.Value = Convert.ToString(dr["Observacion"]);
                this.NovedadIncorporaID.Value = Convert.ToString(dr["NovedadIncorporaID"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoNovedad"].ToString()))
                    this.TipoNovedad.Value = Convert.ToByte(dr["TipoNovedad"]);
                if (!string.IsNullOrWhiteSpace(dr["ConsIncorpora"].ToString()))
                    this.ConsIncorpora.Value = Convert.ToInt32(dr["ConsIncorpora"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        public DTO_ccReincorporacionDeta(IDataReader dr, bool getForReincorporacion)
        {
            InitCols();
            try
            {
                this.NumDocCredito.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.NumeroINC.Value = Convert.ToByte(dr["NumeroINC"]);
                this.NumeroINCIni.Value = Convert.ToByte(dr["NumeroINC"]);
                this.PeriodoNomina.Value = Convert.ToDateTime(dr["FechaNomina"]).Date;
                this.EstadoCruce.Value = Convert.ToByte(dr["EstadoCruce"]);
                this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.Nombre.Value = dr["Nombre"].ToString();
                this.ProfesionID.Value = dr["ProfesionID"].ToString();
                this.CentroPagoID.Value = dr["CentroPagoID"].ToString();
                this.CentroPagoDesc.Value = dr["CentroPagoDesc"].ToString();
                this.EmpleadoCodigo.Value = dr["EmpleadoCodigo"].ToString();
                this.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                this.VlrSaldo.Value = Convert.ToDecimal(dr["VlrSaldo"]);
                this.ValorCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                this.VlrCuotaCredito.Value = Convert.ToDecimal(dr["VlrCuota"]);
                this.FechaLiquida.Value = Convert.ToDateTime(dr["FechaLiquida"]);
                this.CobranzaEstadoID.Value = dr["CobranzaEstadoID"].ToString();
                this.CobranzaGestionID.Value = dr["CobranzaGestionID"].ToString();
                this.SiniestroEstadoID.Value = dr["SiniestroEstadoID"].ToString();
                this.NovedadIncorporaID.Value = dr["NovedadIncorporaID"].ToString();
                this.PlazoCredito.Value = Convert.ToByte(dr["Plazo"]);
                if (this.VlrCuotaCredito.Value != 0)
                    this.PlazoINC.Value = Convert.ToInt16(Math.Ceiling(this.VlrSaldo.Value.Value / this.VlrCuotaCredito.Value.Value));
                else
                    this.PlazoINC.Value = 0;
                this.FechaCuota1.Value = Convert.ToDateTime(dr["FechaCuota1"]);

                if (!string.IsNullOrWhiteSpace(dr["FechaDoc"].ToString()))
                    this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                this.TipoMvto.Value = dr["TipoMvto"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaNomina"].ToString()))
                    this.FechaNomina.Value = Convert.ToDateTime(dr["FechaNomina"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccReincorporacionDeta()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumDocCredito = new UDT_Consecutivo();
            this.PeriodoNomina = new UDT_PeriodoID();            
            this.CentroPagoID = new UDT_CentroPagoID();
            this.CentroPagoDesc = new UDT_Descriptivo();
            this.CentroPagoModificaID = new UDT_CentroPagoID();
            this.CentroPagoModificaDesc = new UDT_Descriptivo();
            this.CambioCentroPagoIND = new UDT_SiNo();
            this.NumeroINC = new UDTSQL_tinyint();
            this.PlazoINC = new UDTSQL_smallint();
            this.ValorCuota = new UDT_Valor();
            this.FechaCuota1 = new UDTSQL_smalldatetime();
            this.EstadoCruce = new UDTSQL_tinyint();
            this.Observacion = new UDT_DescripTExt();
            this.NovedadIncorporaID = new UDTSQL_char(5);
            this.TipoNovedad = new UDTSQL_tinyint();
            this.ConsIncorpora = new UDT_Consecutivo();
            this.CambioCentroPagoIND = new UDT_SiNo();
            this.Consecutivo = new UDT_Consecutivo();
            
            //Otras
            this.Aprobado = new UDT_SiNo();
            this.Extra = new UDT_SiNo();
            this.NumeroINCIni = new UDTSQL_tinyint();
            this.PlazoCredito = new UDTSQL_smallint();
            this.Libranza = new UDT_LibranzaID();
            this.ClienteID = new UDT_ClienteID();
            this.Nombre = new UDT_Descriptivo();
            this.ProfesionID = new UDT_ProfesionID();
            this.EmpleadoCodigo = new UDTSQL_char(12);
            this.VlrLibranza = new UDT_Valor();
            this.VlrSaldo = new UDT_Valor();
            this.VlrCuotaCredito = new UDT_Valor();
            this.FechaLiquida = new UDTSQL_datetime();
            this.CobranzaEstadoID = new UDT_CodigoGrl10();
            this.CobranzaGestionID = new UDT_CodigoGrl10();
            this.SiniestroEstadoID = new UDT_CodigoGrl5();
            this.FechaDoc = new UDTSQL_datetime();
            this.TipoMvto = new UDT_CodigoGrl20();
            this.FechaNomina = new UDTSQL_datetime();
            this.Valor = new UDT_Valor();
        }
        
        #endregion

        #region Propiedades

        //Extra pero se pone aca para la importación
        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_PeriodoID PeriodoNomina { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CentroPagoID CentroPagoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo CentroPagoDesc { get; set; }

        [DataMember]
        public UDT_CentroPagoID CentroPagoModificaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo CentroPagoModificaDesc { get; set; }

        [DataMember]
        public UDT_SiNo CambioCentroPagoIND { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint NumeroINC { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smallint PlazoINC { get; set; }

        [DataMember]
        public UDT_Valor ValorCuota { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaCuota1 { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint EstadoCruce { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDTSQL_char NovedadIncorporaID { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoNovedad { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo ConsIncorpora { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo Consecutivo { get; set; }

        #endregion

        #region Campos adicionales

        [DataMember]
        [NotImportable]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo Extra { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint NumeroINCIni { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smallint PlazoCredito { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_ProfesionID ProfesionID { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char EmpleadoCodigo { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrSaldo { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrCuotaCredito { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_datetime FechaLiquida { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CodigoGrl10 CobranzaEstadoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CodigoGrl10 CobranzaGestionID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CodigoGrl5 SiniestroEstadoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_datetime FechaDoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CodigoGrl20 TipoMvto { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_datetime FechaNomina { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor Valor { get; set; }

        #endregion

    }
}
