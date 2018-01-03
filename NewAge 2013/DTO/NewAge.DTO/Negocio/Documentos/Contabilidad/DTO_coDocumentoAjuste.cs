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
    /// DTO Tabla Documento Ajuste
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coDocumentoAjuste
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coDocumentoAjuste(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.IdentificadorTR.Value = Convert.ToInt64(dr["IdentificadorTR"]);                
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coDocumentoAjuste()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.IdentificadorTR = new UDT_IdentificadorTR();
            this.Valor = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_IdentificadorTR IdentificadorTR { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        #endregion
    }
}
