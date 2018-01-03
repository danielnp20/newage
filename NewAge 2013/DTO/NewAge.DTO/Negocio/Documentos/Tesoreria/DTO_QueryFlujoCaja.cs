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
    #region Documentos
    /// <summary>
    /// Class Models DTO_QueryFlujoCaja
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryFlujoCaja
    {
        #region DTO_QueryFlujoCaja

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryFlujoCaja(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["PerA"].ToString()))
                    this.PerA.Value = Convert.ToDecimal(dr["PerA"]);
                if (!string.IsNullOrEmpty(dr["Per0"].ToString()))
                    this.Per0.Value = Convert.ToDecimal(dr["Per0"]);
                if (!string.IsNullOrEmpty(dr["Per1"].ToString()))
                    this.Per1.Value = Convert.ToDecimal(dr["Per1"]);
                if (!string.IsNullOrEmpty(dr["Per2"].ToString()))
                    this.Per2.Value = Convert.ToDecimal(dr["Per2"]);
                if (!string.IsNullOrEmpty(dr["Per3"].ToString()))
                    this.Per3.Value = Convert.ToDecimal(dr["Per3"]);
                if (!string.IsNullOrEmpty(dr["Per4"].ToString()))
                    this.Per4.Value = Convert.ToDecimal(dr["Per4"]);
                if (!string.IsNullOrEmpty(dr["Per5"].ToString()))
                    this.Per5.Value = Convert.ToDecimal(dr["Per5"]);
                if (!string.IsNullOrEmpty(dr["Per6"].ToString()))
                    this.Per6.Value = Convert.ToDecimal(dr["Per6"]);
                if (!string.IsNullOrEmpty(dr["PerM"].ToString()))
                    this.PerM.Value = Convert.ToDecimal(dr["PerM"]);
                this.Documento.Value = dr["Documento"].ToString();
                

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryFlujoCaja()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Documento = new UDTSQL_char(50);
            this.PerA = new UDTSQL_decimal();
            this.Per0 = new UDTSQL_decimal();
            this.Per1 = new UDTSQL_decimal();
            this.Per2 = new UDTSQL_decimal();
            this.Per3 = new UDTSQL_decimal();
            this.Per4 = new UDTSQL_decimal();
            this.Per5 = new UDTSQL_decimal();
            this.Per6 = new UDTSQL_decimal();
            this.PerM = new UDTSQL_decimal();

        }
        #endregion

        #region Propiedades    

        [DataMember]
        public UDTSQL_char Documento { get; set; }

        [DataMember]
        public UDTSQL_decimal PerA { get; set; }

        [DataMember]
        public UDTSQL_decimal Per0 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per1 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per2 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per3 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per4 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per5 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per6 { get; set; }

        [DataMember]
        public UDTSQL_decimal PerM { get; set; }

        [DataMember]
        public List<DTO_QueryFlujoCajaDetalle> Detalle { get; set; }

        #endregion
    }
    #endregion

    #region Detalle
    /// <summary>
    /// Class Models DTO_QueryFlujoCajaDetalle
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryFlujoCajaDetalle
    {
        public DTO_QueryFlujoCajaDetalle(IDataReader dr)
        {
            InitDetCols();
            try
            {
                this.Documento.Value = dr["Documento"].ToString();

                if (!string.IsNullOrEmpty(dr["Tercero"].ToString()))
                    this.Tercero.Value = Convert.ToString(dr["Tercero"]);
                if (!string.IsNullOrEmpty(dr["Nombre"].ToString()))
                    this.Nombre.Value = Convert.ToString(dr["Nombre"]);
                if (!string.IsNullOrEmpty(dr["Factura"].ToString()))
                    this.Factura.Value = Convert.ToString(dr["Factura"]);
                if (!string.IsNullOrEmpty(dr["FechaVto"].ToString()))
                    this.FechaVto.Value = Convert.ToDateTime(dr["FechaVto"]);
                if (!string.IsNullOrEmpty(dr["Semana"].ToString()))
                    this.Semana.Value = Convert.ToInt32(dr["Semana"]);
                if (!string.IsNullOrEmpty(dr["SemanaAct"].ToString()))
                    this.SemanaAct.Value = Convert.ToInt32(dr["SemanaAct"]);
                if (!string.IsNullOrEmpty(dr["SemanaDif"].ToString()))
                    this.SemanaDif.Value = Convert.ToInt32(dr["SemanaDif"]);
                if (!string.IsNullOrEmpty(dr["SaldoML"].ToString()))
                    this.SaldoML.Value = Convert.ToDecimal(dr["SaldoML"]);
                if (!string.IsNullOrEmpty(dr["SaldoME"].ToString()))
                    this.SaldoME.Value = Convert.ToDecimal(dr["SaldoME"]);

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public DTO_QueryFlujoCajaDetalle()
        {
            InitDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.Documento = new UDTSQL_char (50);
            this.Tercero=new UDT_TerceroID();
            this.Nombre=new UDT_DescripTExt();
            this.Factura=new UDTSQL_char(20);
            this.FechaVto=new UDTSQL_smalldatetime();
            this.Semana=new UDTSQL_int();
            this.SemanaAct=new UDTSQL_int();
            this.SemanaDif=new UDTSQL_int();
            this.SaldoML = new UDTSQL_decimal();
            this.SaldoME = new UDTSQL_decimal();
            this.FileUrl = "";
        }

        #region Propiedades


        [DataMember]
        public UDTSQL_char Documento { get; set; }

        [DataMember]
        public UDT_TerceroID Tercero { get; set; }

        [DataMember]
        public UDT_DescripTExt Nombre { get; set; }

        [DataMember]
        public UDTSQL_char Factura { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaVto { get; set; }

        [DataMember]
        public UDTSQL_decimal SaldoML { get; set; }

        [DataMember]
        public UDTSQL_decimal SaldoME { get; set; }

        [DataMember]
        public UDTSQL_int Semana { get; set; }

        [DataMember]
        public UDTSQL_int SemanaAct { get; set; }

        [DataMember]
        public UDTSQL_int SemanaDif { get; set; }

        [DataMember]
        public string FileUrl { get; set; }
        #endregion
    }
    #endregion

}
