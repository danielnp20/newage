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
    public class DTO_noNovedadesContrato
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noNovedadesContrato(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
                this.ContratoNONovID.Value = dr["ContratoNONovID"].ToString();
                this.ContratoNONovDesc.Value = dr["Descriptivo"].ToString();
                this.FechaInicial.Value = Convert.ToDateTime(dr["FechaInicial"]);
                this.FechaFinal.Value = Convert.ToDateTime(dr["FechaFinal"]);
                this.Documento.Value = dr["Documento"].ToString();
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.ActivaInd.Value = Convert.ToBoolean(dr["ActivaInd"]);
                this.Observacion.Value = dr["Observacion"].ToString();
                this.ContratoNOID.Value = Convert.ToInt32(dr["ContratoNOID"]);
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
        public DTO_noNovedadesContrato()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.EmpleadoID = new UDT_EmpleadoID();
            this.ContratoNONovID = new UDT_ContratoNONovID();
            this.ContratoNONovDesc = new UDT_DescripTBase();
            this.FechaInicial = new UDTSQL_smalldatetime();
            this.FechaFinal = new UDTSQL_smalldatetime();
            this.Documento = new UDTSQL_char(20);
            this.Valor = new UDT_Valor();
            this.ActivaInd = new UDT_SiNo();
            this.Observacion = new UDT_DescripTBase();
            this.ContratoNOID = new UDT_ContratoNOID();
            this.Consecutivo = new UDT_Consecutivo();
        }
        #endregion

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        public UDT_ContratoNONovID ContratoNONovID { get; set; }

        [DataMember]
        public UDT_DescripTBase ContratoNONovDesc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicial { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFinal { get; set; }

        [DataMember]
        public UDTSQL_char Documento { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_SiNo ActivaInd { get; set; }

        [DataMember]
        public UDT_DescripTBase Observacion { get; set; }

        [DataMember]
        public UDT_ContratoNOID ContratoNOID { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }
    }
}
