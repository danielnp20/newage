using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_plPresupuestoSoporte
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plPresupuestoSoporte()
        {
            this.InitCols();
            this.Valor1.Value = 0;
            this.Valor2.Value = 0;
            this.Valor3.Value = 0;
            this.Valor4.Value = 0;
            this.Valor5.Value = 0;
            this.Valor6.Value = 0;
            this.Valor7.Value = 0;
            this.Valor8.Value = 0;
            this.Valor9.Value = 0;
            this.Valor10.Value = 0;
        }

        /// <summary>
        /// Constructor de lectura
        /// </summary>
        /// <param name="dr"></param>
        public DTO_plPresupuestoSoporte(IDataReader dr)
        {
            InitCols();

            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]); 
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();

                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteNro"].ToString()))
                    this.ComprobanteNro.Value = Convert.ToInt32(dr["ComprobanteNro"]);
                this.Prefijo.Value = dr["Prefijo"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NumDocumento"].ToString()))
                    this.NumDocumento.Value = Convert.ToInt32(dr["NumDocumento"]);
                if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);

                this.DatoAdd1.Value = dr["DatoAdd1"].ToString();
                this.DatoAdd2.Value = dr["DatoAdd2"].ToString();
                this.DatoAdd3.Value = dr["DatoAdd3"].ToString();
                this.DatoAdd4.Value = dr["DatoAdd4"].ToString();
                this.DatoAdd5.Value = dr["DatoAdd5"].ToString();
                this.DatoAdd6.Value = dr["DatoAdd6"].ToString();
                this.DatoAdd7.Value = dr["DatoAdd7"].ToString();
                this.DatoAdd8.Value = dr["DatoAdd8"].ToString();
                this.DatoAdd9.Value = dr["DatoAdd9"].ToString();
                this.DatoAdd10.Value = dr["DatoAdd10"].ToString();

                if (!string.IsNullOrWhiteSpace(dr["Valor1"].ToString()))
                    this.Valor1.Value = Convert.ToDecimal(dr["Valor1"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor2"].ToString()))
                    this.Valor2.Value = Convert.ToDecimal(dr["Valor2"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor3"].ToString()))
                    this.Valor3.Value = Convert.ToDecimal(dr["Valor3"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor4"].ToString()))
                    this.Valor4.Value = Convert.ToDecimal(dr["Valor4"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor5"].ToString()))
                    this.Valor5.Value = Convert.ToDecimal(dr["Valor5"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor6"].ToString()))
                    this.Valor6.Value = Convert.ToDecimal(dr["Valor6"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor7"].ToString()))
                    this.Valor7.Value = Convert.ToDecimal(dr["Valor7"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor8"].ToString()))
                    this.Valor8.Value = Convert.ToDecimal(dr["Valor8"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor9"].ToString()))
                    this.Valor9.Value = Convert.ToDecimal(dr["Valor9"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor10"].ToString()))
                    this.Valor10.Value = Convert.ToDecimal(dr["Valor10"]);

                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.PeriodoID = new UDT_PeriodoID();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.CuentaID = new UDT_CuentaID();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteNro = new UDT_Consecutivo();
            this.Prefijo = new UDT_PrefijoID();
            this.NumDocumento = new UDT_Consecutivo();
            this.Fecha = new UDTSQL_smalldatetime();
            this.DatoAdd1 = new UDTSQL_char(30);
            this.DatoAdd2 = new UDTSQL_char(30);
            this.DatoAdd3 = new UDTSQL_char(30);
            this.DatoAdd4 = new UDTSQL_char(30);
            this.DatoAdd5 = new UDTSQL_char(30);
            this.DatoAdd6 = new UDTSQL_char(30);
            this.DatoAdd7 = new UDTSQL_char(30);
            this.DatoAdd8 = new UDTSQL_char(30);
            this.DatoAdd9 = new UDTSQL_char(30);
            this.DatoAdd10 = new UDT_DescripTBase();
            this.Valor1 = new UDT_Valor();
            this.Valor2 = new UDT_Valor();
            this.Valor3 = new UDT_Valor();
            this.Valor4 = new UDT_Valor();
            this.Valor5 = new UDT_Valor();
            this.Valor6 = new UDT_Valor();
            this.Valor7 = new UDT_Valor();
            this.Valor8 = new UDT_Valor();
            this.Valor9 = new UDT_Valor();
            this.Valor10 = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Consecutivo ComprobanteNro { get; set; }

        [DataMember]
        public UDT_PrefijoID Prefijo { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocumento { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd1 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd2 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd3 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd4 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd5 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd6 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd7 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd8 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd9 { get; set; }

        [DataMember]
        public UDT_DescripTBase DatoAdd10 { get; set; }

        [DataMember]
        public UDT_Valor Valor1 { get; set; }

        [DataMember]
        public UDT_Valor Valor2 { get; set; }

        [DataMember]
        public UDT_Valor Valor3 { get; set; }

        [DataMember]
        public UDT_Valor Valor4 { get; set; }

        [DataMember]
        public UDT_Valor Valor5 { get; set; }

        [DataMember]
        public UDT_Valor Valor6 { get; set; }

        [DataMember]
        public UDT_Valor Valor7 { get; set; }

        [DataMember]
        public UDT_Valor Valor8 { get; set; }

        [DataMember]
        public UDT_Valor Valor9 { get; set; }

        [DataMember]
        public UDT_Valor Valor10 { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        #endregion
    }
    
}
