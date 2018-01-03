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
    public class DTO_noCompensatorios
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noCompensatorios(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.Periodo.Value = Convert.ToDateTime(dr["Periodo"]);
                this.ContratoNOID.Value = Convert.ToByte(dr["ContratoNOID"]);
                this.Dia1.Value = dr["Dia1"].ToString();
                this.Dia2.Value = dr["Dia2"].ToString();
                this.Dia3.Value = dr["Dia3"].ToString();
                this.Dia4.Value = dr["Dia4"].ToString();
                this.Dia5.Value = dr["Dia5"].ToString();
                this.Dia6.Value = dr["Dia6"].ToString();
                this.Dia7.Value = dr["Dia7"].ToString();
                this.Dia8.Value = dr["Dia8"].ToString();
                this.Dia9.Value = dr["Dia9"].ToString();
                this.Dia10.Value = dr["Dia10"].ToString();
                this.Dia11.Value = dr["Dia11"].ToString();
                this.Dia12.Value = dr["Dia12"].ToString();
                this.Dia13.Value = dr["Dia13"].ToString();
                this.Dia14.Value = dr["Dia14"].ToString();
                this.Dia15.Value = dr["Dia15"].ToString();
                this.Dia16.Value = dr["Dia16"].ToString();
                this.Dia17.Value = dr["Dia17"].ToString();
                this.Dia18.Value = dr["Dia18"].ToString();
                this.Dia19.Value = dr["Dia19"].ToString();
                this.Dia20.Value = dr["Dia20"].ToString();
                this.Dia21.Value = dr["Dia21"].ToString();
                this.Dia22.Value = dr["Dia22"].ToString();
                this.Dia23.Value = dr["Dia23"].ToString();
                this.Dia24.Value = dr["Dia24"].ToString();
                this.Dia25.Value = dr["Dia25"].ToString();
                this.Dia26.Value = dr["Dia26"].ToString();
                this.Dia27.Value = dr["Dia27"].ToString();
                this.Dia28.Value = dr["Dia28"].ToString();
                this.Dia29.Value = dr["Dia29"].ToString();
                this.Dia30.Value = dr["Dia30"].ToString();
                this.Dia31.Value = dr["Dia31"].ToString();

                if (!string.IsNullOrEmpty(dr["DiasTrabajoMes"].ToString()))
                    this.DiasTrabajoMes.Value = Convert.ToByte(dr["DiasTrabajoMes"].ToString());
                if (!string.IsNullOrEmpty(dr["DiasDescansoMes"].ToString()))
                    this.DiasDescansoMes.Value = Convert.ToByte(dr["DiasDescansoMes"].ToString());
                if (!string.IsNullOrEmpty(dr["DiasSaldoAnt"].ToString()))
                    this.DiasSaldoAnt.Value = Convert.ToByte(dr["DiasSaldoAnt"].ToString());
                if (!string.IsNullOrEmpty(dr["DiasMes"].ToString()))
                    this.DiasMes.Value = Convert.ToByte(dr["DiasMes"].ToString());
                if (!string.IsNullOrEmpty(dr["DiasPagados"].ToString()))
                    this.DiasPagados.Value = Convert.ToByte(dr["DiasPagados"].ToString());
                if (!string.IsNullOrEmpty(dr["DiasAjustados"].ToString()))
                    this.DiasAjustados.Value = Convert.ToByte(dr["DiasAjustados"].ToString());
                if (!string.IsNullOrEmpty(dr["DIasNuevoSaldo"].ToString()))
                    this.DIasNuevoSaldo.Value = Convert.ToByte(dr["DIasNuevoSaldo"].ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
        } 

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noCompensatorios()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.Periodo = new UDT_PeriodoID();
            this.ContratoNOID = new UDT_ContratoNOID();
            this.EmpleadoID = new UDT_EmpleadoID();
            this.Dia1 = new UDTSQL_char(1);
            this.Dia2 = new UDTSQL_char(1);
            this.Dia3 = new UDTSQL_char(1);
            this.Dia4 = new UDTSQL_char(1);
            this.Dia5 = new UDTSQL_char(1);
            this.Dia6 = new UDTSQL_char(1);
            this.Dia7 = new UDTSQL_char(1);
            this.Dia8 = new UDTSQL_char(1);
            this.Dia9 = new UDTSQL_char(1);
            this.Dia10 = new UDTSQL_char(1);
            this.Dia11 = new UDTSQL_char(1);
            this.Dia12 = new UDTSQL_char(1);
            this.Dia13 = new UDTSQL_char(1);
            this.Dia14 = new UDTSQL_char(1);
            this.Dia15 = new UDTSQL_char(1);
            this.Dia16 = new UDTSQL_char(1);
            this.Dia17 = new UDTSQL_char(1);
            this.Dia18 = new UDTSQL_char(1);
            this.Dia19 = new UDTSQL_char(1);
            this.Dia20 = new UDTSQL_char(1);
            this.Dia21 = new UDTSQL_char(1);
            this.Dia22 = new UDTSQL_char(1);
            this.Dia23 = new UDTSQL_char(1);
            this.Dia24 = new UDTSQL_char(1);
            this.Dia25 = new UDTSQL_char(1);
            this.Dia26 = new UDTSQL_char(1);
            this.Dia27 = new UDTSQL_char(1);
            this.Dia28 = new UDTSQL_char(1);
            this.Dia29 = new UDTSQL_char(1);
            this.Dia30 = new UDTSQL_char(1);
            this.Dia31 = new UDTSQL_char(1);
            this.DiasTrabajoMes = new UDTSQL_tinyint();
            this.DiasDescansoMes = new UDTSQL_tinyint();
            this.DiasSaldoAnt = new UDTSQL_tinyint();
            this.DiasMes = new UDTSQL_tinyint();
            this.DiasPagados = new UDTSQL_tinyint();
            this.DiasAjustados = new UDTSQL_tinyint();
            this.DIasNuevoSaldo = new UDTSQL_tinyint();
      
        }
        #endregion
        
        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_PeriodoID Periodo { get; set; }

        [DataMember]
        public UDT_ContratoNOID ContratoNOID { get; set; }

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        public UDTSQL_char Dia1 { get; set; }

        [DataMember]
        public UDTSQL_char Dia2 { get; set; }

        [DataMember]
        public UDTSQL_char Dia3 { get; set; }

        [DataMember]
        public UDTSQL_char Dia4 { get; set; }

        [DataMember]
        public UDTSQL_char Dia5 { get; set; }

        [DataMember]
        public UDTSQL_char Dia6 { get; set; }

        [DataMember]
        public UDTSQL_char Dia7 { get; set; }

        [DataMember]
        public UDTSQL_char Dia8 { get; set; }

        [DataMember]
        public UDTSQL_char Dia9 { get; set; }

        [DataMember]
        public UDTSQL_char Dia10 { get; set; }

        [DataMember]
        public UDTSQL_char Dia11 { get; set; }

        [DataMember]
        public UDTSQL_char Dia12 { get; set; }

        [DataMember]
        public UDTSQL_char Dia13 { get; set; }

        [DataMember]
        public UDTSQL_char Dia14 { get; set; }

        [DataMember]
        public UDTSQL_char Dia15 { get; set; }

        [DataMember]
        public UDTSQL_char Dia16 { get; set; }

        [DataMember]
        public UDTSQL_char Dia17 { get; set; }

        [DataMember]
        public UDTSQL_char Dia18 { get; set; }

        [DataMember]
        public UDTSQL_char Dia19 { get; set; }

        [DataMember]
        public UDTSQL_char Dia20 { get; set; }

        [DataMember]
        public UDTSQL_char Dia21 { get; set; }

        [DataMember]
        public UDTSQL_char Dia22 { get; set; }

        [DataMember]
        public UDTSQL_char Dia23 { get; set; }

        [DataMember]
        public UDTSQL_char Dia24 { get; set; }

        [DataMember]
        public UDTSQL_char Dia25 { get; set; }

        [DataMember]
        public UDTSQL_char Dia26 { get; set; }

        [DataMember]
        public UDTSQL_char Dia27 { get; set; }

        [DataMember]
        public UDTSQL_char Dia28 { get; set; }

        [DataMember]
        public UDTSQL_char Dia29 { get; set; }

        [DataMember]
        public UDTSQL_char Dia30 { get; set; }

        [DataMember]
        public UDTSQL_char Dia31 { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasTrabajoMes { get; set; }
                
        [DataMember]
        public UDTSQL_tinyint DiasDescansoMes { get; set; }
                
        [DataMember]
        public UDTSQL_tinyint DiasSaldoAnt { get; set; }
                
        [DataMember]
        public UDTSQL_tinyint DiasMes { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasPagados { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasAjustados { get; set; }

        [DataMember]
        public UDTSQL_tinyint DIasNuevoSaldo { get; set; }
                  
    }
}
