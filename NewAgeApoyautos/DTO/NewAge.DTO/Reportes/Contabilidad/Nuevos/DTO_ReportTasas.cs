using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte Saldos
    /// </summary>
    public class DTO_ReportTasas
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportTasas()
        {
            InitCols();
        }

        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportTasas(IDataReader dr, bool isDiaria)
        {
            InitCols();
            try
            {
                if (!isDiaria)
                {
                    #region Reporte Tasas Cierre

                    if (!string.IsNullOrEmpty(dr["CuentaID"].ToString()))
                        this.CuentaID.Value = dr["CuentaID"].ToString();
                    if (!string.IsNullOrEmpty(dr["CuentaDesc"].ToString()))
                        this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                    if (!string.IsNullOrEmpty(dr["FinalML"].ToString()))
                        this.FinalML.Value = Convert.ToDecimal(dr["FinalML"]);
                    if (!string.IsNullOrEmpty(dr["FinalME"].ToString()))
                        this.FinalME.Value = Convert.ToDecimal(dr["FinalME"]);
                    if (!string.IsNullOrEmpty(dr["TasaCierre"].ToString()))
                        this.TasaCierre.Value = Convert.ToDecimal(dr["TasaCierre"]);

                    #endregion
                }
                else
                {
                    #region Reporte Tasas Diarias

                    if (!string.IsNullOrEmpty(dr["Dias"].ToString()))
                        this.Dias.Value = Convert.ToInt16(dr["Dias"]);
                    if (!string.IsNullOrEmpty(dr["Ene"].ToString()))
                        this.Ene.Value = Convert.ToInt16(dr["Ene"]);
                    if (!string.IsNullOrEmpty(dr["Febr"].ToString()))
                        this.Febr.Value = Convert.ToInt16(dr["Febr"]);
                    if (!string.IsNullOrEmpty(dr["Mar"].ToString()))
                        this.Mar.Value = Convert.ToInt16(dr["Mar"]);
                    if (!string.IsNullOrEmpty(dr["Abril"].ToString()))
                        this.Abril.Value = Convert.ToInt16(dr["Abril"]);
                    if (!string.IsNullOrEmpty(dr["May"].ToString()))
                        this.May.Value = Convert.ToInt16(dr["May"]);
                    if (!string.IsNullOrEmpty(dr["Jun"].ToString()))
                        this.Jun.Value = Convert.ToInt16(dr["Jun"]);
                    if (!string.IsNullOrEmpty(dr["Jul"].ToString()))
                        this.Jul.Value = Convert.ToInt16(dr["Jul"]);
                    if (!string.IsNullOrEmpty(dr["Ago"].ToString()))
                        this.Ago.Value = Convert.ToInt16(dr["Ago"]);
                    if (!string.IsNullOrEmpty(dr["Sep"].ToString()))
                        this.Sep.Value = Convert.ToInt16(dr["Sep"]);
                    if (!string.IsNullOrEmpty(dr["Oct"].ToString()))
                        this.Oct.Value = Convert.ToInt16(dr["Oct"]);
                    if (!string.IsNullOrEmpty(dr["Nov"].ToString()))
                        this.Nov.Value = Convert.ToInt16(dr["Nov"]);
                    if (!string.IsNullOrEmpty(dr["Dic"].ToString()))
                        this.Dic.Value = Convert.ToInt16(dr["Dic"]);

                    #endregion
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            #region Reprote Tasas Cierre

            this.CuentaID = new UDT_CuentaID();
            this.CuentaDesc = new UDT_DescripTExt();
            this.FinalML = new UDT_Valor();
            this.FinalME = new UDT_Valor();
            this.TasaCierre = new UDT_Valor();

            #endregion
            #region Reporte Tasas Diarias

            this.Dias = new UDTSQL_int();
            this.Ene = new UDT_Valor();
            this.Febr = new UDT_Valor();
            this.Mar = new UDT_Valor();
            this.Abril = new UDT_Valor();
            this.May = new UDT_Valor();
            this.Jun = new UDT_Valor();
            this.Jul = new UDT_Valor();
            this.Ago = new UDT_Valor();
            this.Sep = new UDT_Valor();
            this.Oct = new UDT_Valor();
            this.Nov = new UDT_Valor();
            this.Dic = new UDT_Valor();

            #endregion
        }

        #region Propiedades

        #region Reporte Tasas Cierre

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_DescripTExt CuentaDesc { get; set; }

        [DataMember]
        public UDT_Valor FinalML { get; set; }

        [DataMember]
        public UDT_Valor FinalME { get; set; }

        [DataMember]
        public UDT_Valor TasaCierre { get; set; }

        #endregion
        #region Reporte Tasas Diarias

        [DataMember]
        public UDTSQL_int Dias { get; set; }

        [DataMember]
        public UDT_Valor Ene { get; set; }

        [DataMember]
        public UDT_Valor Febr { get; set; }

        [DataMember]
        public UDT_Valor Mar { get; set; }

        [DataMember]
        public UDT_Valor Abril { get; set; }

        [DataMember]
        public UDT_Valor May { get; set; }

        [DataMember]
        public UDT_Valor Jun { get; set; }

        [DataMember]
        public UDT_Valor Jul { get; set; }

        [DataMember]
        public UDT_Valor Ago { get; set; }

        [DataMember]
        public UDT_Valor Sep { get; set; }

        [DataMember]
        public UDT_Valor Oct { get; set; }

        [DataMember]
        public UDT_Valor Nov { get; set; }

        [DataMember]
        public UDT_Valor Dic { get; set; }

        #endregion

        #endregion

    }
}
