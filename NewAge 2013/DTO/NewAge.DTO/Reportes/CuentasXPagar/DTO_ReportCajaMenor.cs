using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio.Reportes;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    // <summary>
    // Clase del documento Caja Menor
    // </summary>
    public class DTO_ReportCajaMenor : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        //public DTO_ReportCajaMenor(IDataReader dr)
        //{
        //    Responsable = dr["Responsable"].ToString();
        //    FechaIni = Convert.ToDateTime(dr["FechaIni"]);
        //    FechaFin = Convert.ToDateTime(dr["FechaFin"]);
        //    CajaMenorNro = dr["CajaMenorNro"].ToString();
        //    FechaCont = Convert.ToDateTime(dr["FechaCont"]);
        //    ValorCajaMenor = Convert.ToDecimal(dr["ValorCajaMenor"]);
        //    UsuarioElab = dr["UsuarioElab"].ToString();
        //    UsuarioSol = dr["UsuarioSol"].ToString();
        //    UsuarioRev = dr["UsuarioRev"].ToString();
        //    UsuarioSV = dr["UsuarioSV"].ToString();
        //    UsuarioApr = dr["UsuarioApr"].ToString();
        //    UsuarioCont = dr["UsuarioCont"].ToString();
        //}

        public DTO_ReportCajaMenor()
        {
            this.CajaMenorDetail = new List<DTO_CajaMenorDetail>();
        }

        #region Propiedades

        /// <summary>
        /// Numero del documento
        /// </summary>
        [DataMember]
        public string Factura { get; set; }


        /// <summary>
        /// Nombre de la persona responsable
        /// </summary>
        [DataMember]
        public string Responsable { get; set; }

        /// <summary>
        /// Fecha Inicial
        /// </summary>
        [DataMember]
        public DateTime FechaIni { get; set; }

        /// <summary>
        /// Fecha Final
        /// </summary>
        [DataMember]
        public DateTime FechaFin { get; set; }

        /// <summary>
        /// Numero de la Caja Menor
        /// </summary>
        [DataMember]
        public string CajaMenorNro { get; set; }

        /// <summary>
        /// Numero registros
        /// </summary>
        [DataMember]
        public int RegNro { get; set; }

        /// <summary>
        /// Fecha solicitud reembolso
        /// </summary>
        [DataMember]
        public DateTime FechaCont { get; set; }

        /// <summary>
        /// Valor de Caja Menor
        /// </summary>
        [DataMember]
        public decimal ValorCajaMenor { get; set; }

        /// <summary>
        /// Valor de Soportes 
        /// </summary>
        [DataMember]
        public decimal ValorSoportes { get; set; }

        /// <summary>
        /// Valor Disponible
        /// </summary>
        [DataMember]
        public decimal ValorDisponible { get; set; }

        /// <summary>
        /// Elaborado por
        /// </summary>
        [DataMember]
        public string UsuarioElab { get; set; }

        /// <summary>
        /// Solicitado por
        /// </summary>
        [DataMember]
        public string UsuarioSol { get; set; }

        /// <summary>
        /// Revisado por
        /// </summary>
        [DataMember]
        public string UsuarioRev { get; set; }

        /// <summary>
        /// Supervisado por
        /// </summary>
        [DataMember]
        public string UsuarioSV { get; set; }

        /// <summary>
        /// Aprobado por
        /// </summary>
        [DataMember]
        public string UsuarioApr { get; set; }

        /// <summary>
        /// Contabilisado por
        /// </summary>
        [DataMember]
        public string UsuarioCont { get; set; }

        /// <summary>
        /// Indicador del Estado del documento (aprobado - false)
        /// </summary>
        [DataMember]
        public bool EstadoInd { get; set; }

        /// <summary>
        /// Detalle del Recibo
        /// </summary>
        [DataMember]
        public List<DTO_CajaMenorDetail> CajaMenorDetail { get; set; }

        /// <summary>
        /// Footer del Recibo
        /// </summary>
        //[DataMember]
        //public List<DTO_ReciboFooter> ReciboFooter { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    
    /// <summary>
    /// Clase del Detalle de Caja Menor
    /// </summary>
    public class DTO_CajaMenorDetail
    {

        #region Propiedades

        /// <summary>
        /// Cargo Especial ID
        /// </summary>
        [DataMember]
        public string CargoEspID { get; set; }

        /// <summary>
        /// Descriptivo de Cargo Especial
        /// </summary>
        [DataMember]
        public string CargoEspDesc { get; set; }
        
        /// <summary>
        /// Fecha de la factura
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Factura Numero
        /// </summary>
        [DataMember]
        public string Documento { get; set; }

        /// <summary>
        /// Nit/CC
        /// </summary>
        [DataMember]
        public string TerceroID { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [DataMember]
        public string TerceroDesc { get; set; }

        /// <summary>
        /// Descriptivo de CentroCosto
        /// </summary>
        [DataMember]
        public string CentroCostoDesc { get; set; }

        /// <summary>
        /// Descriptivo de Proyecto
        /// </summary>
        [DataMember]
        public string ProyectoDesc { get; set; }

        /// <summary>
        /// Descriptive de la factura
        /// </summary>
        [DataMember]
        public string FacturaDesc { get; set; }

        /// <summary>
        /// Valor Bruto
        /// </summary>
        [DataMember]
        public decimal ValorBruto { get; set; }

        /// <summary>
        /// Valor Iva
        /// </summary>
        [DataMember]
        public decimal ValorIva { get; set; }

        /// <summary>
        /// Valor ReteFuente
        /// </summary>
        [DataMember]
        public decimal ValorRteF { get; set; }

        /// <summary>
        /// Valor RteIVA
        /// </summary>
        [DataMember]
        public decimal ValorRteIVA { get; set; }

        /// <summary>
        /// Valor RteICA
        /// </summary>
        [DataMember]
        public decimal ValorRteICA { get; set; }
        #endregion

        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        //public DTO_CajaMenorDetail(IDataReader dr)
        //{
        //    Fecha = Convert.ToDateTime(dr["Fecha"]);
        //    Documento = dr["Documento"].ToString().Trim();
        //    TerceroID = dr["TerceroID"].ToString().Trim();
        //    TerceroDesc = dr["TerceroDesc"].ToString().Trim();
        //    CentroCostoDesc = dr["CentroCostoDesc"].ToString().Trim();
        //    ProyectoDesc = dr["ProyectoDesc"].ToString().Trim();
        //    FacturaDesc = dr["FacturaDesc"].ToString().Trim();
        //    ValorBruto = Convert.ToDecimal(dr["ValorBruto"]);
        //    ValorIva = Convert.ToDecimal(dr["ValorIva"]);
        //    ValorRteF = Convert.ToDecimal(dr["ValorRteF"]);
        //    ValorRteIVA = Convert.ToDecimal(dr["ValorRteIVA"]);
        //    ValorRteICA = Convert.ToDecimal(dr["ValorRteICA"]);       
        //}

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_CajaMenorDetail()
        {
        }
    }

}
