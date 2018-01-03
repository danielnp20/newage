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
    public class DTO_ReportCxPPorEdades
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReportCxPPorEdades(IDataReader dr, bool isDetallada)
        {
            this.InitCols();
            try
            {
                //Datos Genericos
                if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrEmpty(dr["Descriptivo"].ToString()))
                    this.Descriptivo.Value = (dr["Descriptivo"].ToString());
                if (!string.IsNullOrEmpty(dr["No_Vencidas"].ToString()))
                    this.No_Vencidas.Value =Convert.ToDecimal(dr["No_Vencidas"]);
                if (!string.IsNullOrEmpty(dr["Treinta"].ToString()))
                    this.Treinta.Value = Convert.ToDecimal(dr["Treinta"]);
                if (!string.IsNullOrEmpty(dr["Sesenta"].ToString()))
                    this.Sesenta.Value = Convert.ToDecimal(dr["Sesenta"]);
                if (!string.IsNullOrEmpty(dr["Noventa"].ToString()))
                    this.Noventa.Value = Convert.ToDecimal(dr["Noventa"]);
                if (!string.IsNullOrEmpty(dr["COchenta"].ToString()))
                    this.COchenta.Value = Convert.ToDecimal(dr["COchenta"]);
                if (!string.IsNullOrEmpty(dr["MasCOchenta"].ToString()))
                    this.MasCOchenta.Value = Convert.ToDecimal(dr["MasCOchenta"]);

                //Datos para reporte detallado
                if (isDetallada)
                {
                    if (!string.IsNullOrEmpty(dr["Factura"].ToString()))
                        this.Factura.Value = Convert.ToString(dr["Factura"]);
                    if (!string.IsNullOrEmpty(dr["FacturaFecha"].ToString()))
                        this.FacturaFecha.Value = Convert.ToDateTime(dr["FacturaFecha"]);
                    if (!string.IsNullOrEmpty(dr["VtoFecha"].ToString()))
                        this.VtoFecha.Value = Convert.ToDateTime(dr["VtoFecha"]);
                    if (!string.IsNullOrEmpty(dr["CuentaId"].ToString()))
                        this.CuentaId.Value = (dr["CuentaId"].ToString());
                }
                else
                {
                    if (!string.IsNullOrEmpty(dr["total"].ToString()))
                        this.Total.Value = Convert.ToDecimal(dr["total"]);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportCxPPorEdades()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CuentaId = new UDT_CuentaID();
            this.FacturaFecha = new UDTSQL_datetime();
            this.VtoFecha = new UDTSQL_datetime();
            this.Factura = new UDTSQL_char(20);
            this.No_Vencidas = new UDT_Valor();
            this.TerceroID = new UDT_TerceroID();
            this.Descriptivo = new UDT_Descriptivo();
            this.Treinta = new UDT_Valor();
            this.Sesenta = new UDT_Valor();
            this.Noventa = new UDT_Valor();
            this.COchenta = new UDT_Valor();
            this.MasCOchenta = new UDT_Valor();
            this.Total = new UDT_Valor();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDTSQL_char Factura { get; set; }

        [DataMember]
        public UDTSQL_datetime FacturaFecha { get; set; }

        [DataMember]
        public UDTSQL_datetime VtoFecha { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaId { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDTSQL_int DifDia { get; set; }

        [DataMember]
        public UDT_Valor No_Vencidas { get; set; }

        [DataMember]
        public UDT_Valor Treinta { get; set; }

        [DataMember]
        public UDT_Valor Sesenta { get; set; }

        [DataMember]
        public UDT_Valor Noventa { get; set; }

        [DataMember]
        public UDT_Valor COchenta { get; set; }

        [DataMember]
        public UDT_Valor MasCOchenta { get; set; }

        //Reporte Resumido
        [DataMember]
        public UDT_Valor Total { get; set; }


        #endregion

    }
}
