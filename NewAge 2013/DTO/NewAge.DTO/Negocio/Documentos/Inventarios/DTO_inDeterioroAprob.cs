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
    /// Class Models DTO_inDeterioroAprob
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inDeterioroAprob
    {
        #region DTO_inDeterioroAprob

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inDeterioroAprob(IDataReader dr)
        {
            InitCols();
            try
            {               
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.PrefijoID.Value = dr["PrefijoID"].ToString();
                this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                this.PrefDoc = this.PrefijoID.Value + " - " + this.DocumentoNro.Value.Value.ToString();
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                 this.DocumentoDesc.Value = dr["DocumentoDesc"].ToString();
                this.EstadoInv.Value = Convert.ToByte(dr["EstadoInv"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inDeterioroAprob()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Aprobado = new UDT_SiNo();
            this.Rechazado = new UDT_SiNo();
            this.Observacion = new UDT_DescripTExt();
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.PeriodoID = new UDTSQL_datetime();
            this.DocumentoID = new UDT_DocumentoID();
            this.PrefijoID = new UDT_PrefijoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.DocumentoDesc = new UDT_Descriptivo();
            this.EstadoInv = new UDTSQL_tinyint();           
            this.ValorTotalML = new UDT_Valor();
            this.ValorTotalME = new UDT_Valor();
            this.FileUrl = "";
            this.Detalle = new List<DTO_inDeterioroAprobDet>();
        }
        #endregion

        #region Propiedades
        [DataMember]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        public UDT_SiNo Rechazado { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDTSQL_datetime PeriodoID { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        public string PrefDoc { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumentoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoInv { get; set; }

        [DataMember]
        public UDT_Valor ValorTotalML { get; set; }

        [DataMember]
        public UDT_Valor ValorTotalME { get; set; }
     
        [DataMember]
        public string FileUrl { get; set; }

        [DataMember]
        public List<DTO_inDeterioroAprobDet> Detalle { get; set; }
        #endregion
    }

    /// <summary>
    /// Class Models DTO_inDeterioroAprobDet
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inDeterioroAprobDet
    {
        public DTO_inDeterioroAprobDet(IDataReader dr)
        {
            InitDetCols();
            try
            {
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.DescripTExt.Value = dr["DescripTExt"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CantidadUNI"].ToString()))
                    this.CantidadUNI.Value = Convert.ToInt32(dr["CantidadUNI"]);
                this.Valor1LOC.Value = Convert.ToDecimal(dr["Valor1LOC"]);
                this.Valor1EXT.Value = Convert.ToDecimal(dr["Valor1EXT"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_inDeterioroAprobDet()
        {
            InitDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.inReferenciaID = new UDT_inReferenciaID();
            this.DescripTExt = new UDT_DescripTExt();
            this.CantidadUNI = new UDT_Cantidad();
            this.Valor1LOC = new UDT_Valor();
            this.Valor1EXT = new UDT_Valor();
        }

        #region Propiedades

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_DescripTExt DescripTExt { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadUNI { get; set; }

        [DataMember]
        public UDT_Valor Valor1LOC { get; set; }

        [DataMember]
        public UDT_Valor Valor1EXT { get; set; }
     
        #endregion
    }
}
