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
    /// Models DTO_prContratoPlanPago
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prContratoPlanPago
    {
        #region DTO_prContratoPlanPago

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prContratoPlanPago(IDataReader dr)
        {
            InitCols();
            try
            {               
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);

                if (!string.IsNullOrWhiteSpace(dr["ValorAdicional"].ToString()))
                    this.ValorAdicional.Value = Convert.ToDecimal(dr["ValorAdicional"]);
                
                this.Observacion.Value = dr["Observacion"].ToString();
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
        public DTO_prContratoPlanPago()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {           
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.Fecha = new UDTSQL_datetime();
            this.Valor = new UDT_Valor();
            this.ValorAdicional = new UDT_Valor();
            this.Observacion = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor ValorAdicional { get; set; }

        [DataMember]
        public UDT_DescripTExt  Observacion { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        #endregion
    }
}

