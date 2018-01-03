using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// 
    /// Models DTO_pyRecursoCosto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyRecursoCosto
    {
        #region DTO_pyRecursoCosto

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyRecursoCosto(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Periodo.Value = Convert.ToDateTime(dr["Periodo"]);
                this.CodigoBSID.Value = Convert.ToString(dr["CodigoBSID"]);
                this.inReferenciaID.Value = Convert.ToString(dr["inReferenciaID"]);
                this.TrabajoID.Value = Convert.ToString(dr["TrabajoID"]);
                if (!string.IsNullOrWhiteSpace(dr["CostoLocal"].ToString()))
                    this.CostoLocal.Value = Convert.ToDecimal(dr["CostoLocal"]);
                if (!string.IsNullOrWhiteSpace(dr["CostoExtra"].ToString()))
                    this.CostoExtra.Value = Convert.ToDecimal(dr["CostoExtra"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyRecursoCosto()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.Periodo = new UDT_PeriodoID();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.TrabajoID = new UDT_CodigoGrl();
            this.CostoLocal = new UDT_Valor();
            this.CostoExtra = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
        }

        #endregion
        
        [DataMember]
        public UDT_PeriodoID Periodo { get; set; }
        
        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }
        
        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }
        
        [DataMember]
        public UDT_CodigoGrl TrabajoID { get; set; }
        
        [DataMember]
        public UDT_Valor CostoLocal { get; set; }
        
        [DataMember]
        public UDT_Valor CostoExtra { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }
    }
}
