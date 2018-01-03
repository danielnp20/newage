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
    public class DTO_ccArchivoPlanoXPagaduria
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccArchivoPlanoXPagaduria(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["codigo"].ToString()))
                    this.codigo.Value = dr["codigo"].ToString();
                if (!string.IsNullOrEmpty(dr["CC"].ToString()))
                    this.CC.Value = dr["CC"].ToString();
                if (!string.IsNullOrEmpty(dr["VlrCuota"].ToString()))
                    this.VlrCuota.Value = dr["VlrCuota"].ToString();
                if (!string.IsNullOrEmpty(dr["NroCuotas"].ToString()))
                    this.NroCuotas.Value = dr["NroCuotas"].ToString();
                if (!string.IsNullOrEmpty(dr["Libranza"].ToString()))
                    this.Libranza.Value = dr["Libranza"].ToString();
                if (!string.IsNullOrEmpty(dr["TipoNove"].ToString()))
                    this.TipoNove.Value = dr["TipoNove"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccArchivoPlanoXPagaduria()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.codigo = new UDTSQL_char(10);
            this.CC = new UDTSQL_char(20);
            this.VlrCuota = new UDTSQL_char(20);
            this.NroCuotas = new UDTSQL_char(2);
            this.Libranza = new UDTSQL_char(20);
            this.TipoNove = new UDTSQL_char(1);
        }
        #endregion

        #region Region

        [DataMember]
        public UDTSQL_char codigo { get; set; }

        [DataMember]
        public UDTSQL_char CC { get; set; }

        [DataMember]
        public UDTSQL_char VlrCuota { get; set; }

        [DataMember]
        public UDTSQL_char NroCuotas { get; set; }

        [DataMember]
        public UDTSQL_char Libranza { get; set; }

        [DataMember]
        public UDTSQL_char TipoNove { get; set; }
        #endregion
    }
}
