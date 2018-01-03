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
    public class DTO_ccAseguradoraReport
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccAseguradoraReport(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["ClienteId"].ToString()))
                    this.ClienteId.Value = dr["ClienteId"].ToString();
                if (!string.IsNullOrEmpty(dr["Descriptivo"].ToString()))
                    this.Descriptivo.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaNacimiento"].ToString()))
                    this.FechaNaciemiento = Convert.ToDateTime(dr["FechaNacimiento"]);
                if (!string.IsNullOrEmpty(dr["SaldoMLoc"].ToString()))
                    this.SaldoMdaLocal.Value = Convert.ToDecimal(dr["SaldoMLoc"]);
                if (!string.IsNullOrEmpty(dr["Libranza"].ToString()))
                    this.Libranza.Value = dr["Libranza"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaLiquida"].ToString()))
                    this.FechaLiquida = Convert.ToDateTime(dr["FechaLiquida"]);
                if (!string.IsNullOrEmpty(dr["PagaduriaDesc"].ToString()))
                    this.PagaduriaDesc.Value = dr["PagaduriaDesc"].ToString();
                //if (!string.IsNullOrEmpty(dr["Edad"].ToString()))
                //    this.Edad = Convert.ToInt32(dr["Edad"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccAseguradoraReport()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ClienteId = new UDT_ClienteID();
            this.Descriptivo = new UDT_Descriptivo();
            this.FechaNaciemiento = new DateTime();
            this.SaldoMdaLocal = new UDT_Valor();
            this.Libranza = new UDTSQL_char(20);
            this.FechaLiquida = new DateTime();
            this.PagaduriaDesc = new UDT_Descriptivo();
            this.FechaIni = new DateTime();
            this.FechaFin = new DateTime();
        }
        #endregion

        [DataMember]
        public UDT_ClienteID ClienteId { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public DateTime FechaNaciemiento { get; set; }

        [DataMember]
        public UDT_Valor SaldoMdaLocal { get; set; }

        [DataMember]
        public UDTSQL_char Libranza { get; set; }

        [DataMember]
        public DateTime FechaLiquida { get; set; }

        [DataMember]
        public UDT_Descriptivo PagaduriaDesc { get; set; }

        //Campos Extra
        [DataMember]
        public DateTime FechaIni { get; set; }

        [DataMember]
        public DateTime FechaFin { get; set; }
        
        [DataMember]
        public int Edad { get; set; }
    }
}
