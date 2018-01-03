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
    /// Models DTO_prConvenio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prConvenio
    {
        #region DTO_prSaldosDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prConvenio(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.MonedaID.Value = dr["MonedaID"].ToString();
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
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
        public DTO_prConvenio()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Seleccionar = new UDT_SiNo();
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.MonedaID = new UDT_MonedaID();
            this.Valor = new UDT_Valor();
            this.Consecutivo = new UDT_Valor();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_SiNo Seleccionar { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor Consecutivo { get; set; }

        #endregion
    }
}

