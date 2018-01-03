using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// DTO Tabla Recibo Caja Documento
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_tsReciboCajaDocu
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_tsReciboCajaDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.CajaID.Value = Convert.ToString(dr["CajaID"]);
                this.BancoCuentaID.Value = Convert.ToString(dr["BancoCuentaID"]);
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.IVA.Value = Convert.ToDecimal(dr["IVA"]);
                this.ClienteID.Value = Convert.ToString(dr["ClienteID"]);
                this.TerceroID.Value = Convert.ToString(dr["TerceroID"]);
                
                if (!string.IsNullOrWhiteSpace(dr["NumeroDocNomina"].ToString()))
                    this.NumeroDocNomina.Value = Convert.ToInt32(dr["NumeroDocNomina"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaConsignacion"].ToString()))
                    this.FechaConsignacion.Value = Convert.ToDateTime(dr["FechaConsignacion"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAplica"].ToString()))
                    this.FechaAplica.Value = Convert.ToDateTime(dr["FechaAplica"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_tsReciboCajaDocu()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.CajaID = new UDTSQL_char(5);
            this.BancoCuentaID = new UDTSQL_char(5);
            this.Valor = new UDT_Valor();
            this.IVA = new UDT_Valor();
            this.ClienteID = new UDTSQL_char(20);
            this.TerceroID = new UDT_TerceroID();
            this.NumeroDocNomina = new UDT_Consecutivo();
            this.FechaConsignacion = new UDTSQL_smalldatetime();
            this.FechaAplica = new UDTSQL_smalldatetime();

            //Extras
            this.Item = new UDT_Consecutivo();
            this.TasaCambio = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        //Solo para migración de recibos de caja
        [DataMember]
        public UDT_Consecutivo Item { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_char CajaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char BancoCuentaID { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor IVA { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDTSQL_char ClienteID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDocNomina { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaConsignacion { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaAplica { get; set; }

        //Campos extras
        
        [DataMember]
        [NotImportable]
        public DTO_ccCJHistorico ccCJHistoricoPago { get; set; }  //Se usa para el abono a cobro jurídico en financieras

        [DataMember]
        [NotImportable]
        public DTO_ccCJHistorico ccCJHistoricoInteres { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor TasaCambio { get; set; }

        #endregion

    }
}
