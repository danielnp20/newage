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
    public class DTO_ccArchivoIncorporaciones
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccArchivoIncorporaciones(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if(!string.IsNullOrWhiteSpace(dr["TipoNovedad"].ToString()))
                    this.TipoNovedad.Value = Convert.ToByte(dr["TipoNovedad"]);
                this.LibranzaID.Value = Convert.ToInt32(dr["LibranzaID"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAfilia"].ToString()))
                    this.NumeroINC.Value = 0;
                else
                    this.NumeroINC.Value = Convert.ToInt32(dr["NumeroINC"]);
                if(this.TipoNovedad.Value == 1)
                {
                    this.Libranza.Value = this.LibranzaID.Value;
                }
                else
                {
                    this.Libranza.Value = Convert.ToInt32(this.LibranzaID.Value.ToString() + this.NumeroINC.Value.ToString());
                }
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.Nombre.Value = dr["Nombre"].ToString();
                this.CodEmpleado.Value = dr["CodEmpleado"].ToString();
                this.ProfesionID.Value = dr["ProfesionID"].ToString();
                this.CentroPagoID.Value = dr["CentroPagoID"].ToString();
                this.CodNemonico.Value = dr["CodNemonico"].ToString();
                this.VlrIncorpora.Value = Convert.ToDecimal(dr["VlrIncorpora"]);
                this.VlrSaldo.Value = Convert.ToDecimal(dr["VlrSaldo"]);
                if (!string.IsNullOrWhiteSpace(dr["Plazo"].ToString()))
                    this.Plazo.Value = Convert.ToInt16(dr["Plazo"]);
                this.FechaCuota1.Value = Convert.ToDateTime(dr["FechaCuota1"]);
                this.FechaVto.Value = Convert.ToDateTime(dr["FechaVto"]);
                this.CodPolicia.Value = dr["CodPolicia"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaAfilia"].ToString()))
                    this.FechaAfilia.Value = Convert.ToDateTime(dr["FechaAfilia"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaDesafilia"].ToString()))
                    this.FechaDesafilia.Value = Convert.ToDateTime(dr["FechaDesafilia"]);
                this.IncorporaIDE.Value = dr["IncorporaIDE"].ToString();
                this.DesincorporaIDE.Value = dr["DesincorporaIDE"].ToString();
                this.AfiliaIDE.Value = dr["AfiliaIDE"].ToString();
                this.DesafiliaIDE.Value = dr["DesafiliaIDE"].ToString();
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccArchivoIncorporaciones()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Consecutivo = new UDT_Consecutivo();
            this.TipoNovedad = new UDTSQL_tinyint();
            this.LibranzaID = new UDT_LibranzaID();
            this.NumeroINC = new UDTSQL_int();
            this.Libranza = new UDTSQL_int();
            this.TerceroCoop = new UDT_TerceroID();
            this.ClienteID = new UDT_ClienteID();
            this.Nombre = new UDT_Descriptivo();
            this.CodEmpleado = new UDTSQL_char(12);
            this.ProfesionID = new UDT_BasicID();
            this.CentroPagoID = new UDT_BasicID();
            this.PagaduriaID = new UDT_BasicID();
            this.CodNemonico = new UDTSQL_char(20);
            this.VlrIncorpora = new UDT_Valor();
            this.VlrSaldo = new UDT_Valor();
            this.Plazo = new UDTSQL_smallint();
            this.FechaCuota1 = new UDTSQL_datetime();
            this.FechaVto = new UDTSQL_datetime();
            this.CodPolicia = new UDTSQL_varchar(10);
            this.FechaAfilia = new UDTSQL_datetime();
            this.FechaDesafilia = new UDTSQL_datetime();
            this.IncorporaIDE = new UDT_CodigoGrl5();
            this.DesincorporaIDE = new UDT_CodigoGrl5();
            this.AfiliaIDE = new UDT_CodigoGrl5();
            this.DesafiliaIDE = new UDT_CodigoGrl5();
            this.CodigoNovedad = new UDT_CodigoGrl5();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoNovedad { get; set; }

        [DataMember]
        public UDT_LibranzaID LibranzaID { get; set; }

        [DataMember]
        public UDTSQL_int NumeroINC { get; set; }

        [DataMember]
        public UDTSQL_int Libranza { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroCoop { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDTSQL_char CodEmpleado { get; set; }

        [DataMember]
        public UDT_BasicID ProfesionID { get; set; }

        [DataMember]
        public UDT_BasicID CentroPagoID { get; set; }

        [DataMember]
        public UDT_BasicID PagaduriaID { get; set; }

        [DataMember]
        public UDTSQL_char CodNemonico { get; set; }

        [DataMember]
        public UDT_Valor VlrIncorpora { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldo { get; set; }

        [DataMember]
        public UDTSQL_smallint Plazo { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaCuota1 { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaVto { get; set; }

        [DataMember]
        public UDTSQL_varchar CodPolicia { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaAfilia { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaDesafilia { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 CodigoNovedad { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 IncorporaIDE { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 DesincorporaIDE { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 AfiliaIDE { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 DesafiliaIDE { get; set; }

        #endregion
    }
}
