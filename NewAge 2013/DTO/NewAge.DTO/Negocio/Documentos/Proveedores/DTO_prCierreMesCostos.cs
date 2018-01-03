using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;
using System.Reflection;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class recibidos de bienes y servicios:
    /// Models DTO_prCierreMesCostos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prCierreMesCostos
    {
        #region DTO_prSaldosDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prCierreMesCostos(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.VlrLocal.Value = Convert.ToDecimal(dr["VlrLocal"]);
                this.VlrExtra.Value = Convert.ToDecimal(dr["VlrExtra"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                ;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prCierreMesCostos()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.PeriodoID = new UDT_PeriodoID();           
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.VlrLocal = new UDT_Valor();
            this.VlrExtra = new UDT_Valor();
            this.Consecutivo = new UDT_Valor();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Valor VlrLocal { get; set; }

        [DataMember]
        public UDT_Valor VlrExtra { get; set; }

        [DataMember]
        public UDT_Valor Consecutivo { get; set; }

        #endregion
    }
}

