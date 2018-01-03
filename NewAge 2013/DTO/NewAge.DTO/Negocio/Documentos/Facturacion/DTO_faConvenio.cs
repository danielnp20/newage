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
    /// DTO Tabla DTO_faConvenio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_faConvenio
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_faConvenio(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["EmpresaID"].ToString()))
                    this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["ServicioID"].ToString()))
                    this.ServicioID.Value = Convert.ToString(dr["ServicioID"]);
                if (!string.IsNullOrWhiteSpace(dr["inReferenciaID"].ToString()))
                    this.inReferenciaID.Value = Convert.ToString(dr["inReferenciaID"]);
                if (!string.IsNullOrWhiteSpace(dr["Moneda"].ToString()))
                    this.Moneda.Value = Convert.ToString(dr["Moneda"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["Consecutivo"].ToString()))
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
        public DTO_faConvenio()
        {
            InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.ServicioID = new UDT_ServicioID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.Moneda = new UDT_MonedaID();
            this.Valor = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
           
        }

        #endregion

        #region Propiedades

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }
        
        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_ServicioID ServicioID { get; set; }
        
        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_inReferenciaID inReferenciaID { get; set; }
        
        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_MonedaID Moneda { get; set; }
        
        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Valor Valor { get; set; }
        
        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Consecutivo Consecutivo { get; set; }

        #endregion
    }
}
