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
    /// Models DTO_prConvenioSolicitudDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prConvenioSolicitudDocu
    {
        #region DTO_prConvenioSolicitudDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prConvenioSolicitudDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);               
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.NumeroDocContrato.Value = Convert.ToInt32(dr["NumeroDocContrato"]);
                this.Moneda.Value = dr["Moneda"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["IVA"].ToString()))
                    this.IVA.Value = Convert.ToDecimal(dr["IVA"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prConvenioSolicitudDocu()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.ProveedorID = new UDT_ProveedorID();
            this.NumeroDocContrato = new UDT_Consecutivo();
            this.Moneda = new UDT_MonedaID();
            this.Valor = new UDT_Valor();
            this.IVA = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_ProveedorID ProveedorID { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_MonedaID Moneda { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocContrato { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor IVA { get; set; }

        #endregion
    }
}
