using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del Certificados
    /// </summary>
    public class DTO_Certificates
    {
        /// <summary>
        /// Contructor por defecto
        /// </summary>
        public DTO_Certificates()
        { }

        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_Certificates(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrEmpty(dr["TerceroDesc"].ToString()))
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["CuentaID"].ToString()))
                    this.CuentaID.Value = dr["CuentaID"].ToString();
                if (!string.IsNullOrEmpty(dr["CuentaDesc"].ToString()))
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["BaseML"].ToString()))
                    this.BaseML.Value = dr["BaseML"].ToString();
                if (!string.IsNullOrEmpty(dr["ValorML"].ToString()))
                    this.ValorML.Value = dr["ValorML"].ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroID = new UDT_TerceroID();
            this.TerceroDesc = new UDT_DescripTExt();
            this.CuentaID = new UDT_CuentaID();
            this.CuentaDesc = new UDT_TerceroID();
            this.BaseML = new UDT_DescripTExt();
            this.ValorML = new UDT_CuentaID();
        }

        #region Propiedades

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DescripTExt TerceroDesc { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_TerceroID CuentaDesc { get; set; }

        [DataMember]
        public UDT_DescripTExt BaseML { get; set; }

        [DataMember]
        public UDT_CuentaID ValorML { get; set; }

        #endregion
    }
}

