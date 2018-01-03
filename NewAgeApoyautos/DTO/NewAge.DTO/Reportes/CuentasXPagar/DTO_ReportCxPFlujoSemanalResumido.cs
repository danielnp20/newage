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
    public class DTO_ReportCxPFlujoSemanalResumido
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReportCxPFlujoSemanalResumido(IDataReader dr)
        {
            this.InitCols();
            try
            {

                if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrEmpty(dr["Descriptivo"].ToString()))
                    this.Descriptivo.Value = (dr["Descriptivo"].ToString());
                if (!string.IsNullOrEmpty(dr["Semana1"].ToString()))
                    this.Semana1.Value = Convert.ToDecimal(dr["Semana1"]);
                if (!string.IsNullOrEmpty(dr["Semana2"].ToString()))
                    this.Semana2.Value = Convert.ToDecimal(dr["Semana2"]);
                if (!string.IsNullOrEmpty(dr["Semana3"].ToString()))
                    this.Semana3.Value = Convert.ToDecimal(dr["Semana3"]);
                if (!string.IsNullOrEmpty(dr["Semana4"].ToString()))
                    this.Semana4.Value = Convert.ToDecimal(dr["Semana4"]);
                if (!string.IsNullOrEmpty(dr["Semana5"].ToString()))
                    this.Semana5.Value = Convert.ToDecimal(dr["Semana5"]);
                if (!string.IsNullOrEmpty(dr["Semana"].ToString()))
                    this.Semana.Value = Convert.ToInt32(dr["Semana"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportCxPFlujoSemanalResumido()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroID = new UDT_TerceroID();
            this.Descriptivo = new UDT_Descriptivo();
            this.Semana = new UDTSQL_int();
            this.Semana1 = new UDT_Valor();
            this.Semana2 = new UDT_Valor();
            this.Semana3 = new UDT_Valor();
            this.Semana4 = new UDT_Valor();
            this.Semana5 = new UDT_Valor();
            this.VtoFecha = new UDTSQL_datetime();
            this.FechaConrte = new UDTSQL_datetime();
            this.ValorTotal = new UDT_Valor();
        }
        #endregion

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor Semana1 { get; set; }

        [DataMember]
        public UDT_Valor Semana2 { get; set; }

        [DataMember]
        public UDT_Valor Semana3 { get; set; }

        [DataMember]
        public UDT_Valor Semana4 { get; set; }

        [DataMember]
        public UDT_Valor Semana5 { get; set; }

        [DataMember]
        public UDTSQL_datetime VtoFecha { get; set; }

        [DataMember]
        public UDTSQL_int Semana { get; set; }

        //Campos para el manejo de los dias de la semana

        [DataMember]
        public string Dias1 { get; set; }

        [DataMember]
        public string Dias2 { get; set; }

        [DataMember]
        public string Dias3 { get; set; }

        [DataMember]
        public string Dias4 { get; set; }

        [DataMember]
        public string Dias5 { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaConrte { get; set; }

        //Valor
        [DataMember]
        public UDT_Valor ValorTotal { get; set; }
    }
}
