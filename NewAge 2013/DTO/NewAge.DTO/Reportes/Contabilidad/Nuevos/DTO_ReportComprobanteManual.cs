using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_ReportComprobanteManual
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr">Lectura de Data</param>
        /// <param name="isDocContable">Verifica si es Documento Contable (TRUE = Carga info Doc Contable, FALSE = Carga info Causacion Fact.)</param>
        /// <param name="?"></param>
        public DTO_ReportComprobanteManual(IDataReader dr)
        {
            this.InitCols();
            try
            {
                //Valores Genericos
                #region Valores Genericos

                if (!string.IsNullOrEmpty(dr["CuentaID"].ToString()))
                    this.CuentaID.Value = dr["CuentaID"].ToString();
                if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrEmpty(dr["ProyectoID"].ToString()))
                    this.ProyectoID.Value = dr["ProyectoID"].ToString();
                if (!string.IsNullOrEmpty(dr["ConceptoCargoID"].ToString()))
                    this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                if (!string.IsNullOrEmpty(dr["LineaPresupuestoID"].ToString()))
                    this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                if (!string.IsNullOrEmpty(dr["Descripcion"].ToString()))
                    this.Descripcion.Value = dr["Descripcion"].ToString();
                if (!string.IsNullOrEmpty(dr["fechaFac"].ToString()))
                    this.fechaFac.Value = Convert.ToDateTime(dr["fechaFac"]);
                if (!string.IsNullOrEmpty(dr["ComprobanteID"].ToString()))
                    this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                if (!string.IsNullOrEmpty(dr["ComprobanteNro"].ToString()))
                    this.ComprobanteNro.Value = Convert.ToInt32(dr["ComprobanteNro"]);
                if (!string.IsNullOrEmpty(dr["Base"].ToString()))
                    this.Base.Value = Convert.ToDecimal(dr["Base"]);
                if (!string.IsNullOrEmpty(dr["Debito"].ToString()))
                    this.Debito.Value = Convert.ToDecimal(dr["Debito"]);
                if (!string.IsNullOrEmpty(dr["Credito"].ToString()))
                    this.Credito.Value = Convert.ToDecimal(dr["Credito"]);
               
                #endregion

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportComprobanteManual()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            #region Columnas Genericas

            this.CuentaID = new UDT_CuentaID();
            this.TerceroID = new UDT_TerceroID();
            this.ProyectoID = new UDT_ProyectoID();
            this.ConceptoCargoID = new UDT_ConceptoCargoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.Descripcion = new UDT_DescripTBase();
            this.fechaFac = new UDTSQL_smalldatetime();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteNro = new UDT_Consecutivo();
            this.Base = new UDT_Valor();
            this.Debito = new UDT_Valor();
            this.Credito = new UDT_Valor();


            #endregion
        }
        #endregion

        #region Propiedades

        #region Propiedades Genericas

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_ConceptoCargoID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_DescripTBase Descripcion { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime fechaFac { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Consecutivo ComprobanteNro { get; set; }

        [DataMember]
        public UDT_Valor Base { get; set; }

        [DataMember]
        public UDT_Valor Debito { get; set; }

        [DataMember]
        public UDT_Valor Credito { get; set; }

        #endregion

        #endregion
    }
}
