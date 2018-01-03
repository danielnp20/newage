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
    /// Models DTO_glUsuarioxGrupo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glDatosMensuales : DTO_MasterComplex
    {
        #region DTO_glDatosMensuales
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glDatosMensuales(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.Periodo.Value = Convert.ToDateTime(dr["Periodo"]);
                this.Tasa1.Value = Convert.ToDecimal(dr["Tasa1"]);
                this.Tasa2.Value = Convert.ToDecimal(dr["Tasa2"]);
                if (!string.IsNullOrEmpty(dr["Tasa3"].ToString()))
                    this.Tasa3.Value = Convert.ToDecimal(dr["Tasa3"]);
                if (!string.IsNullOrEmpty(dr["Tasa4"].ToString()))
                    this.Tasa4.Value = Convert.ToDecimal(dr["Tasa4"]);
                if (!string.IsNullOrEmpty(dr["Tasa5"].ToString()))
                    this.Tasa5.Value = Convert.ToDecimal(dr["Tasa5"]);
                if (!string.IsNullOrEmpty(dr["Tasa6"].ToString()))
                    this.Tasa6.Value = Convert.ToDecimal(dr["Tasa6"]);
                if (!string.IsNullOrEmpty(dr["Tasa7"].ToString()))
                    this.Tasa7.Value = Convert.ToDecimal(dr["Tasa7"]);
                if (!string.IsNullOrEmpty(dr["Tasa8"].ToString()))
                    this.Tasa8.Value = Convert.ToDecimal(dr["Tasa8"]);
                if (!string.IsNullOrEmpty(dr["Tasa9"].ToString()))
                    this.Tasa9.Value = Convert.ToDecimal(dr["Tasa9"]);
                if (!string.IsNullOrEmpty(dr["Tasa10"].ToString()))
                    this.Tasa10.Value = Convert.ToDecimal(dr["Tasa10"]);
                this.Valor1.Value = Convert.ToDecimal(dr["Valor1"]);
                this.Valor2.Value = Convert.ToDecimal(dr["Valor2"]);
                if (!string.IsNullOrEmpty(dr["Valor3"].ToString()))
                    this.Valor3.Value = Convert.ToDecimal(dr["Valor3"]);
                if (!string.IsNullOrEmpty(dr["Valor4"].ToString()))
                    this.Valor4.Value = Convert.ToDecimal(dr["Valor4"]);
                if (!string.IsNullOrEmpty(dr["Valor5"].ToString()))
                    this.Valor5.Value = Convert.ToDecimal(dr["Valor5"]);
                if (!string.IsNullOrEmpty(dr["Valor6"].ToString()))
                    this.Valor6.Value = Convert.ToDecimal(dr["Valor6"]);
                if (!string.IsNullOrEmpty(dr["Valor7"].ToString()))
                    this.Valor7.Value = Convert.ToDecimal(dr["Valor7"]);
                if (!string.IsNullOrEmpty(dr["Valor8"].ToString()))
                    this.Valor8.Value = Convert.ToDecimal(dr["Valor8"]);
                if (!string.IsNullOrEmpty(dr["Valor9"].ToString()))
                    this.Valor9.Value = Convert.ToDecimal(dr["Valor9"]);
                if (!string.IsNullOrEmpty(dr["Valor10"].ToString()))
                    this.Valor10.Value = Convert.ToDecimal(dr["Valor10"]);
                if (!string.IsNullOrEmpty(dr["Fecha1"].ToString()))
                    this.Fecha1.Value = Convert.ToDateTime(dr["Fecha1"]);
                if (!string.IsNullOrEmpty(dr["Fecha2"].ToString()))
                    this.Fecha2.Value = Convert.ToDateTime(dr["Fecha2"]);
                if (!string.IsNullOrEmpty(dr["Fecha3"].ToString()))
                    this.Fecha3.Value = Convert.ToDateTime(dr["Fecha3"]);
                if (!string.IsNullOrEmpty(dr["Fecha4"].ToString()))
                    this.Fecha4.Value = Convert.ToDateTime(dr["Fecha4"]);
                if (!string.IsNullOrEmpty(dr["Fecha5"].ToString()))
                    this.Fecha5.Value = Convert.ToDateTime(dr["Fecha5"]);
                if (!string.IsNullOrEmpty(dr["Fecha6"].ToString()))
                    this.Fecha6.Value = Convert.ToDateTime(dr["Fecha6"]);
                if (!string.IsNullOrEmpty(dr["Fecha7"].ToString()))
                    this.Fecha7.Value = Convert.ToDateTime(dr["Fecha7"]);
                if (!string.IsNullOrEmpty(dr["Fecha8"].ToString()))
                    this.Fecha8.Value = Convert.ToDateTime(dr["Fecha8"]);
                if (!string.IsNullOrEmpty(dr["Fecha9"].ToString()))
                    this.Fecha9.Value = Convert.ToDateTime(dr["Fecha9"]);
                if (!string.IsNullOrEmpty(dr["Fecha10"].ToString()))
                    this.Fecha10.Value = Convert.ToDateTime(dr["Fecha10"]);
            }
            catch (Exception e)
            {
               throw e;
            }         
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glDatosMensuales() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PeriodoID = new UDTSQL_smalldatetime();
            this.Periodo = new UDTSQL_smalldatetime();
            this.Tasa1 = new UDT_TasaID();
            this.Tasa2 = new UDT_TasaID();
            this.Tasa3 = new UDT_TasaID();
            this.Tasa4 = new UDT_TasaID();
            this.Tasa5 = new UDT_TasaID();
            this.Tasa6 = new UDT_TasaID();
            this.Tasa7 = new UDT_TasaID();
            this.Tasa8 = new UDT_TasaID();
            this.Tasa9 = new UDT_TasaID();
            this.Tasa10 = new UDT_TasaID();
            this.Valor1 = new UDT_Valor();
            this.Valor2 = new UDT_Valor();
            this.Valor3 = new UDT_Valor();
            this.Valor4 = new UDT_Valor();
            this.Valor5 = new UDT_Valor();
            this.Valor6 = new UDT_Valor();
            this.Valor7 = new UDT_Valor();
            this.Valor8 = new UDT_Valor();
            this.Valor9 = new UDT_Valor();
            this.Valor10 = new UDT_Valor();
            this.Fecha1 = new UDTSQL_smalldatetime();
            this.Fecha2 = new UDTSQL_smalldatetime();
            this.Fecha3 = new UDTSQL_smalldatetime();
            this.Fecha4 = new UDTSQL_smalldatetime();
            this.Fecha5 = new UDTSQL_smalldatetime();
            this.Fecha6 = new UDTSQL_smalldatetime();
            this.Fecha7 = new UDTSQL_smalldatetime();
            this.Fecha8 = new UDTSQL_smalldatetime();
            this.Fecha9 = new UDTSQL_smalldatetime();
            this.Fecha10 = new UDTSQL_smalldatetime();
        }

        public DTO_glDatosMensuales(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glDatosMensuales(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_smalldatetime PeriodoID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Periodo { get; set; }

        [DataMember]
        public UDT_TasaID Tasa1 { get; set; }

        [DataMember]
        public UDT_TasaID Tasa2 { get; set; }

        [DataMember]
        public UDT_TasaID Tasa3 { get; set; }

        [DataMember]
        public UDT_TasaID Tasa4 { get; set; }

        [DataMember]
        public UDT_TasaID Tasa5 { get; set; }

        [DataMember]
        public UDT_TasaID Tasa6 { get; set; }

        [DataMember]
        public UDT_TasaID Tasa7 { get; set; }

        [DataMember]
        public UDT_TasaID Tasa8 { get; set; }

        [DataMember]
        public UDT_TasaID Tasa9 { get; set; }

        [DataMember]
        public UDT_TasaID Tasa10 { get; set; }

        [DataMember]
        public UDT_Valor Valor1 { get; set; }

        [DataMember]
        public UDT_Valor Valor2 { get; set; }

        [DataMember]
        public UDT_Valor Valor3 { get; set; }

        [DataMember]
        public UDT_Valor Valor4 { get; set; }

        [DataMember]
        public UDT_Valor Valor5 { get; set; }

        [DataMember]
        public UDT_Valor Valor6 { get; set; }

        [DataMember]
        public UDT_Valor Valor7 { get; set; }

        [DataMember]
        public UDT_Valor Valor8 { get; set; }

        [DataMember]
        public UDT_Valor Valor9 { get; set; }

        [DataMember]
        public UDT_Valor Valor10 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha1 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha2 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha3 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha4 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha5 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha6 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha7 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha8 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha9 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha10 { get; set; }
    }

}
