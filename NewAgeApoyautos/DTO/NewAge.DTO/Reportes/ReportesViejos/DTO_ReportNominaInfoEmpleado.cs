using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Reportes;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]
    public class DTO_ReportNominaInfoEmpleado 
    {
        public DTO_ReportNominaInfoEmpleado(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["FechaIngreso"].ToString()))
                    this.FechaIngreso = Convert.ToDateTime(dr["FechaIngreso"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaRetiro"].ToString()))
                    this.FechaRetiro = Convert.ToDateTime(dr["FechaRetiro"]);
                if (!string.IsNullOrWhiteSpace(dr["LugarGeograficoID"].ToString()))
                { this.Localidad = dr["LugarGeograficoID"].ToString(); }
                if (!string.IsNullOrWhiteSpace(dr["EmpleadoID"].ToString()))
                { this.EmpleadoID = dr["EmpleadoID"].ToString(); }
                if (!string.IsNullOrWhiteSpace(dr["OperacionNOID"].ToString()))
                    this.Operacion = dr["OperacionNOID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["EmpleadoDesc"].ToString()))
                { this.EmpleadoDesc = dr["EmpleadoDesc"].ToString(); }
                if (!string.IsNullOrWhiteSpace(dr["CargoEmpID"].ToString()))
                { this.Cargo = dr["CargoEmpDesc"].ToString(); }
                if (!string.IsNullOrWhiteSpace(dr["BrigadaID"].ToString()))
                    this.Brigada = dr["BrigadaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CentroCostoID"].ToString()))
                    this.CentroCosto = dr["CentroCostoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["Dias1"].ToString()))
                    this.DiasDescanso = Convert.ToInt32(dr["Dias1"]);
                if (!string.IsNullOrWhiteSpace(dr["SueldoML"].ToString()))
                    this.Salario = Convert.ToDecimal(dr["SueldoML"]);
                if (!string.IsNullOrWhiteSpace(dr["ProyectoID"].ToString()))
                    this.Proyecto = (dr["ProyectoID"].ToString());
            }
            catch (Exception e)
            { ; }
        }

        private void InitCols()
        {

            this.FechaIngreso = new DateTime();
            this.NumeroDoc = new int();
            this.DiasDescanso = new int();
            this.Salario = new decimal();
        }
        /// <summary>
        /// Constructor x Defecto
        /// </summary>
        public DTO_ReportNominaInfoEmpleado()
        {
            this.InitCols();
        }

        #region Propiedades
        [DataMember]
        public int NumeroDoc { get; set; }

        [DataMember]
        public string EmpleadoID { get; set; }

        [DataMember]
        public string EmpleadoDesc { get; set; }

        [DataMember]
        public DateTime FechaIngreso { get; set; }

        [DataMember]
        public DateTime? FechaRetiro { get; set; }

        [DataMember]
        public string Localidad { get; set; }

        [DataMember]
        public string Operacion { get; set; }

        [DataMember]
        public string Cargo { get; set; }

        [DataMember]
        public string Brigada { get; set; }

        [DataMember]
        public string CentroCosto { get; set; }

        [DataMember]
        public decimal Salario { get; set; }

        [DataMember]
        public string Proyecto { get; set; }

        [DataMember]
        public int DiasDescanso { get; set; }

        [DataMember]
        public string LocaclidadDesc { get; set; }

        [DataMember]
        public string CentroCostDesc { get; set; }

        [DataMember]
        public string ProyectoDesc { get; set; }

        [DataMember]
        public DateTime FechaInicial { get; set; }

        [DataMember]
        public DateTime FechaFinal { get; set; }

        [DataMember]
        public decimal TotalPagar { get; set; }
       
        [DataMember]
        public List<DTO_noReportDetalleEmpleadoConcepto> Detalles { get; set; }
        #endregion
    }
}

