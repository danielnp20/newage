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
    public class DTO_VistaQ_ccIncorpora
    {
        #region DTO_ccCreditoLiquida

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        public DTO_VistaQ_ccIncorpora(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaDoc"].ToString()))
                    this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["Mes"].ToString()))
                    this.Mes.Value = Convert.ToByte(dr["Mes"]);
                this.Libranza.Value = Convert.ToString(dr["Libranza"]);
                this.ClienteID.Value = Convert.ToString(dr["ClienteID"]);
                this.PrimApellido.Value = Convert.ToString(dr["PrimApellido"]);
                this.SegApellido.Value = Convert.ToString(dr["SegApellido"]);
                this.PriNombre.Value = Convert.ToString(dr["PriNombre"]);
                this.SegNombre.Value = Convert.ToString(dr["SegNombre"]);
                this.PagadID.Value = Convert.ToString(dr["PagadID"]);
                this.PagadNombre.Value = Convert.ToString(dr["PagadNombre"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrIncorpora"].ToString()))
                    this.VlrIncorpora.Value = Convert.ToByte(dr["VlrIncorpora"]);
                if (!string.IsNullOrWhiteSpace(dr["PlazoIncorpora"].ToString()))
                    this.PlazoIncorpora.Value = Convert.ToByte(dr["PlazoIncorpora"]);
                this.TipoNovedad.Value = Convert.ToString(dr["TipoNovedad"]);
                this.NovedadCodigo.Value = Convert.ToString(dr["NovedadCodigo"]);
                this.SiniestroEstado.Value = Convert.ToString(dr["SiniestroEstado"]);
                this.CobranzaEstado.Value = Convert.ToString(dr["CobranzaEstado"]);
                this.CobranzaGestion.Value = Convert.ToString(dr["CobranzaGestion"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaIncorpora"].ToString()))
                    this.FechaIncorpora.Value = Convert.ToDateTime(dr["FechaIncorpora"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaLiquida"].ToString()))
                    this.FechaLiquida.Value = Convert.ToDateTime(dr["FechaLiquida"]);
                this.LineaCredito.Value = Convert.ToString(dr["LineaCredito"]);
                this.ProfesionNombre.Value = Convert.ToString(dr["ProfesionNombre"]);
                this.ProfesionCodigo.Value = Convert.ToString(dr["ProfesionCodigo"]);
                this.CodAfiliacion.Value = Convert.ToString(dr["CodAfiliacion"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrRecogesaldo"].ToString()))
                    this.VlrRecogesaldo.Value = Convert.ToByte(dr["VlrRecogesaldo"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        #region Inicializar Columnas

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_VistaQ_ccIncorpora()
        {
            this.InitCols();
        } 

        #endregion

        #endregion

        #region Inicializa las columnas

        /// <summary>
        /// Inicializa Columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.Mes = new UDTSQL_int();
            this.Libranza = new UDT_DocTerceroID();
            this.ClienteID = new UDT_ClienteID();
            this.PrimApellido = new UDT_DescripTBase();
            this.SegApellido = new UDT_DescripTBase();
            this.PriNombre = new UDT_DescripTBase();
            this.SegNombre = new UDT_DescripTBase();
            this.PagadID = new UDT_PagaduriaID();
            this.PagadNombre = new UDT_DescripTBase();
            this.VlrIncorpora = new UDT_Valor();
            this.PlazoIncorpora = new UDTSQL_tinyint();
            this.TipoNovedad = new UDTSQL_char(25);
            this.NovedadCodigo = new UDTSQL_char(5);
            this.SiniestroEstado = new UDT_CodigoGrl5();
            this.CobranzaEstado = new UDT_CodigoGrl10();
            this.CobranzaGestion = new UDT_CodigoGrl10();
            this.FechaIncorpora = new UDTSQL_smalldatetime();
            this.FechaLiquida = new UDTSQL_smalldatetime();              
            this.LineaCredito = new UDT_LineaCreditoID();
            this.ProfesionNombre = new UDT_Descriptivo();
            this.ProfesionCodigo = new UDT_ProfesionID();
            this.CodAfiliacion = new UDTSQL_char(20);
            this.VlrRecogesaldo = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDTSQL_int Mes { get; set; }

        [DataMember]
        public UDT_DocTerceroID Libranza { get; set; }

        [DataMember]
        public UDTSQL_tinyint NumeroINC { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_DescripTBase PrimApellido { get; set; }

        [DataMember]
        public UDT_DescripTBase SegApellido { get; set; }

        [DataMember]
        public UDT_DescripTBase PriNombre { get; set; }

        [DataMember]
        public UDT_DescripTBase SegNombre { get; set; }              
        
        [DataMember]
        public UDT_PagaduriaID PagadID { get; set; }

        [DataMember]
        public UDT_DescripTBase PagadNombre { get; set; }

        [DataMember]
        public UDT_Valor VlrIncorpora { get; set; }

        [DataMember]
        public UDTSQL_tinyint PlazoIncorpora { get; set; }

        [DataMember]
        public UDTSQL_char TipoNovedad { get; set; }

        [DataMember]
        public UDTSQL_char NovedadCodigo { get; set; } 

        [DataMember]
        public UDT_CodigoGrl5 SiniestroEstado { get; set; } 

        [DataMember]
        public UDT_CodigoGrl10 CobranzaEstado { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 CobranzaGestion { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaIncorpora { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLiquida { get; set; } 

        [DataMember]
        public UDT_LineaCreditoID LineaCredito { get; set; }

        [DataMember]
        public UDT_ProfesionID ProfesionCodigo { get; set; }

        [DataMember]
        public UDT_Descriptivo ProfesionNombre { get; set; }

        [DataMember]
        public UDTSQL_char CodAfiliacion { get; set; } //20

        [DataMember]
        public UDT_Valor VlrRecogesaldo { get; set; } 
        #endregion

    }
}
