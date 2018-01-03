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
    public class DTO_acCostos
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_acCostos(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["BalanceTipoID"].ToString()))
                    this.BalanceTipoID.Value = dr["BalanceTipoID"].ToString();
                if (!string.IsNullOrEmpty(dr["PeriodoID"].ToString()))
                    this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                if (!string.IsNullOrEmpty(dr["PlaquetaID"].ToString()))
                    this.PlaquetaID.Value = dr["PlaquetaID"].ToString();
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = dr["Observacion"].ToString();
                if (!string.IsNullOrEmpty(dr["SerialID"].ToString()))
                    this.SerialID.Value = dr["SerialID"].ToString();
                if (!string.IsNullOrEmpty(dr["LocFisicaID"].ToString()))
                    this.LocFisicaID.Value = dr["LocFisicaID"].ToString();
                if (!string.IsNullOrEmpty(dr["ActivoClaseID"].ToString()))
                    this.ActivoClaseID.Value = dr["ActivoClaseID"].ToString();
                if (!string.IsNullOrEmpty(dr["NombreClase"].ToString()))
                    this.NombreClase.Value = dr["NombreClase"].ToString();
                if (!string.IsNullOrEmpty(dr["SaldoML"].ToString()))
                    this.SaldoML.Value = Convert.ToDecimal(dr["SaldoML"]);
                if (!string.IsNullOrEmpty(dr["SaldoME"].ToString()))
                    this.SaldoME.Value = Convert.ToDecimal(dr["SaldoME"]);
                if (!string.IsNullOrEmpty(dr["Costo"].ToString()))
                    this.Costo.Value = Convert.ToDecimal(dr["Costo"]);
                if (!string.IsNullOrEmpty(dr["Depresiacion"].ToString()))
                    this.Depresiacion.Value = Convert.ToDecimal(dr["Depresiacion"]);
                if (!string.IsNullOrEmpty(dr["Deterioro"].ToString()))
                    this.Deterioro.Value = Convert.ToDecimal(dr["Deterioro"]);
                if (!string.IsNullOrEmpty(dr["Revalorizacion"].ToString()))
                    this.Revalorizacion.Value = Convert.ToDecimal(dr["Revalorizacion"]);
                if (!string.IsNullOrEmpty(dr["Desmantelamiento"].ToString()))
                    this.Desmantelamiento.Value = Convert.ToDecimal(dr["Desmantelamiento"]);

                //if(bool)
                //{
                //    if (!string.IsNullOrEmpty(dr["SaldoME"].ToString()))
                //    this.SaldoME.Value = Convert.ToDecimal(dr["SaldoME"]);
                //}

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acCostos()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.BalanceTipoID = new UDT_BalanceTipoID();
            this.PeriodoID = new UDTSQL_datetime();
            this.PlaquetaID = new UDT_PlaquetaID();
            this.Observacion = new UDT_DescripTBase();
            this.SerialID = new UDT_SerialID();
            this.LocFisicaID = new UDT_LocFisicaID();
            this.ActivoClaseID = new UDT_ActivoClaseID();
            this.NombreClase = new UDT_Descriptivo();
            this.SaldoML = new UDT_Valor();
            this.SaldoME = new UDT_Valor();
            this.Costo = new UDT_Valor();
            this.Depresiacion = new UDT_Valor();
            this.Deterioro = new UDT_Valor();
            this.Revalorizacion = new UDT_Valor();
            this.Desmantelamiento = new UDT_Valor();

            //Otros
            this.Libro = new UDT_Descriptivo();
        }
        #endregion

        #region Propiedades

        //Fijas
        [DataMember]
        public UDT_BalanceTipoID BalanceTipoID { get; set; }

        [DataMember]
        public UDTSQL_datetime PeriodoID { get; set; }

        [DataMember]
        public UDT_PlaquetaID PlaquetaID { get; set; }

        [DataMember]
        public UDT_DescripTBase Observacion { get; set; }

        [DataMember]
        public UDT_SerialID SerialID { get; set; }

        [DataMember]
        public UDT_LocFisicaID LocFisicaID { get; set; }

        [DataMember]
        public UDT_ActivoClaseID ActivoClaseID { get; set; }

        [DataMember]
        public UDT_Descriptivo NombreClase { get; set; }

        [DataMember]
        public UDT_Valor SaldoML { get; set; }

        [DataMember]
        public UDT_Valor SaldoME { get; set; }

        [DataMember]
        public UDT_Valor Costo { get; set; }

        [DataMember]
        public UDT_Valor Depresiacion { get; set; }

        [DataMember]
        public UDT_Valor Deterioro { get; set; }

        [DataMember]
        public UDT_Valor Revalorizacion { get; set; }

        [DataMember]
        public UDT_Valor Desmantelamiento { get; set; }

        //Otras
        [DataMember]
        public UDT_Descriptivo Libro { get; set; }

        #endregion
    }
}
