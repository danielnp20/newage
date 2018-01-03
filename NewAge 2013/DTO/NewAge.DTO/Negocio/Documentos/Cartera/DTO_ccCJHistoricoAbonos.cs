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
    /// Models DTO_ccCJHistoricoAbonos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCJHistoricoAbonos
    {
        #region DTO_ccCJHistoricoAbonos

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCJHistoricoAbonos(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ClaseDeuda.Value = Convert.ToByte(dr["ClaseDeuda"]);                
                this.ComponenteCarteraID.Value = dr["ComponenteCarteraID"].ToString();
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.TipoAbono.Value = Convert.ToByte(dr["TipoAbono"]);
                if (!string.IsNullOrWhiteSpace(dr["DocProceso"].ToString()))
                   this.DocProceso.Value = Convert.ToInt32(dr["DocProceso"]);  
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
        public DTO_ccCJHistoricoAbonos()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.ClaseDeuda = new UDTSQL_tinyint();
            this.ComponenteCarteraID = new UDT_ComponenteCarteraID();
            this.Valor = new UDT_Valor();
            this.TipoAbono = new UDTSQL_tinyint();
            this.DocProceso = new UDT_Consecutivo();
            this.Consecutivo = new UDT_Consecutivo();           
        }

        #endregion

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_tinyint ClaseDeuda { get; set; }

        [DataMember]
        public UDT_ComponenteCarteraID ComponenteCarteraID { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoAbono { get; set; }

        [DataMember]
        public UDT_Consecutivo DocProceso { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }


    }
}
