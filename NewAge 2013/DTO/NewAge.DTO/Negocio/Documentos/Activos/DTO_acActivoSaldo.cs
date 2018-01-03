using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio.Documentos.Activos
{
    [Serializable]
    [DataContract]
    public class DTO_acActivoSaldo
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acActivoSaldo()
        {
            InitCols();     
        }

        /// <summary>
        /// Construye el dto a partir de una consula hecha en la bd
        /// </summary>
        /// <param name="dr">DataReader</param>
        public DTO_acActivoSaldo(IDataReader dr)
        {
            InitCols();
            try
            {
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.acComponenteID.Value = dr["acComponenteID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.IdentificadorTR.Value = Convert.ToInt32(dr["IdentificadorTR"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoMLoc"].ToString()))
                    this.SaldoMLoc.Value = Convert.ToInt32(dr["SaldoMLoc"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoMExt"].ToString()))
                    this.SaldoMExt.Value = Convert.ToInt32(dr["SaldoMExt"]);
                if (!string.IsNullOrWhiteSpace(dr["MvtMLoc"].ToString()))
                    this.MvtMLoc.Value = Convert.ToInt32(dr["MvtMLoc"]);
                if (!string.IsNullOrWhiteSpace(dr["MvtMExt"].ToString()))
                    this.MvtMExt.Value = Convert.ToInt32(dr["MvtMExt"]);
                this.Naturaleza.Value =  Convert.ToByte(dr["Naturaleza"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_acActivoSaldo(IDataReader dr, bool activoSaldo)
        {
            InitCols();
            try
            {
                this.CuentaID.Value = dr["CuentaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Enero"].ToString()))
                    this.Enero.Value = Convert.ToInt32(dr["Enero"]);
                if (!string.IsNullOrWhiteSpace(dr["Febrero"].ToString()))
                    this.Febrero.Value = Convert.ToInt32(dr["Febrero"]);
                if (!string.IsNullOrWhiteSpace(dr["Marzo"].ToString()))
                    this.Marzo.Value = Convert.ToInt32(dr["Marzo"]);
                if (!string.IsNullOrWhiteSpace(dr["Abril"].ToString()))
                    this.Abril.Value = Convert.ToInt32(dr["Abril"]);
                if (!string.IsNullOrWhiteSpace(dr["Mayo"].ToString()))
                    this.Mayo.Value = Convert.ToInt32(dr["Mayo"]);
                if (!string.IsNullOrWhiteSpace(dr["Junio"].ToString()))
                    this.Junio.Value = Convert.ToInt32(dr["Junio"]);
                if (!string.IsNullOrWhiteSpace(dr["Julio"].ToString()))
                    this.Julio.Value = Convert.ToInt32(dr["Julio"]);
                if (!string.IsNullOrWhiteSpace(dr["Agosto"].ToString()))
                    this.Agosto.Value = Convert.ToInt32(dr["Agosto"]);
                if (!string.IsNullOrWhiteSpace(dr["Septiembre"].ToString()))
                    this.Septiembre.Value = Convert.ToInt32(dr["Septiembre"]);
                if (!string.IsNullOrWhiteSpace(dr["Octubre"].ToString()))
                    this.Octubre.Value = Convert.ToInt32(dr["Octubre"]);
                if (!string.IsNullOrWhiteSpace(dr["Noviembre"].ToString()))
                    this.Noviembre.Value = Convert.ToInt32(dr["Noviembre"]);
                if (!string.IsNullOrWhiteSpace(dr["Diciembre"].ToString()))
                    this.Diciembre.Value = Convert.ToInt32(dr["Diciembre"]);
            }
            catch (Exception e)
            { 
                throw e;
            }
        }

        private void InitCols()
        {
            this.CuentaID = new UDT_CuentaID();
            this.IdentificadorTR = new UDT_Consecutivo();
            this.acComponenteID = new UDT_acComponenteID();
            this.Descriptivo = new UDT_DescripTBase();
            this.SaldoMExt = new UDT_Valor();
            this.SaldoMLoc = new UDT_Valor();
            //
            this.Enero = new UDT_Valor();
            this.Febrero = new UDT_Valor();
            this.Marzo = new UDT_Valor();
            this.Abril = new UDT_Valor();
            this.Mayo = new UDT_Valor();
            this.Junio = new UDT_Valor();
            this.Julio = new UDT_Valor();
            this.Agosto = new UDT_Valor();
            this.Septiembre = new UDT_Valor();
            this.Octubre = new UDT_Valor();
            this.Noviembre = new UDT_Valor();
            this.Diciembre = new UDT_Valor();
            this.MvtMLoc = new UDT_Valor();
            this.MvtMExt = new UDT_Valor();
            this.Naturaleza = new UDTSQL_tinyint();
            this.dtoCoCuentaSaldo = new DTO_coCuentaSaldo();
            this.SaldoLoc = new UDT_Valor();
            this.SaldoExt = new UDT_Valor();
        }

        #region Propiedades

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_Consecutivo IdentificadorTR { get; set; }

        [DataMember]
        public UDT_acComponenteID acComponenteID { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }
        
        //Saldos

        [DataMember]
        public UDT_Valor SaldoMLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoMExt { get; set; }

        [DataMember]
        public UDT_Valor MvtMLoc { get; set; }

        [DataMember]
        public UDT_Valor MvtMExt { get; set; }

        [DataMember]
        public UDTSQL_tinyint Naturaleza { get; set; }

        //Meses
        [DataMember]
        public UDT_Valor Enero { get; set; }

        [DataMember]
        public UDT_Valor Febrero { get; set; }

        [DataMember]
        public UDT_Valor Marzo { get; set; }

        [DataMember]
        public UDT_Valor Abril { get; set; }

        [DataMember]
        public UDT_Valor Mayo { get; set; }

        [DataMember]
        public UDT_Valor Junio { get; set; }

        [DataMember]
        public UDT_Valor Julio { get; set; }

        [DataMember]
        public UDT_Valor Agosto { get; set; }

        [DataMember]
        public UDT_Valor Septiembre { get; set; }

        [DataMember]
        public UDT_Valor Octubre { get; set; }

        [DataMember]
        public UDT_Valor Noviembre { get; set; }

        [DataMember]
        public UDT_Valor Diciembre { get; set; }

        [DataMember]
        public DTO_coCuentaSaldo dtoCoCuentaSaldo { get; set; }

        // Campos extras

        [DataMember]
        public UDT_Valor SaldoLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoExt { get; set; }

        #endregion
    }
}
