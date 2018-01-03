using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_CuentaValor
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_CuentaValor : DTO_SerializedObject
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_CuentaValor(string cuenta, decimal valor, decimal baseVlr, string lugarGeoID, string tipoImpuesto)
        {
            InitCols();
            this.CuentaID.Value = cuenta;
            this.Valor.Value = valor;
            this.Base.Value = baseVlr;
            this.LugarGeograficoID = lugarGeoID;
            this.TipoImpuesto = tipoImpuesto;
            
            this.IVADescontable = false;
            this.TarifaCosto = 0;
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CuentaID = new UDT_CuentaID();
            this.Valor = new UDT_Valor();
            this.Base = new UDT_Valor();
        }
	    #endregion
 
        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor Base { get; set; }

        [DataMember]
        public string LugarGeograficoID { get; set; }

        [DataMember]
        public string TipoImpuesto { get; set; }

        [DataMember]
        public bool IVADescontable { get; set; }

        [DataMember]
        public decimal TarifaCosto { get; set; }

    }
}
