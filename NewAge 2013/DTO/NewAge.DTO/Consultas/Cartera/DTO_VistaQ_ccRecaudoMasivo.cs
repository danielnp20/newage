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
    public class VistaQ_ccRecaudoMasivo
    {
        #region DTO_ccCreditoLiquida

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        public VistaQ_ccRecaudoMasivo(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Libranza.Value = Convert.ToString(dr["Libranza"]);
                this.Prefijo.Value = Convert.ToInt32(dr["Prefijo"]);
                this.PagaduriaID.Value = Convert.ToString(dr["PagaduriaID"]);
                this.Nombre_Pagaduria.Value = Convert.ToString(dr["Nombre_Pagaduria"]);
                this.CooperativaID.Value = Convert.ToString(dr["CooperativaID"]);
                this.Nombre_Coop.Value = Convert.ToString(dr["Nombre_Coop"]);
                this.TerceroID.Value = Convert.ToString(dr["TerceroID"]);
                this.Nombre_Cliente.Value = Convert.ToString(dr["Nombre_Cliente"]);
                this.VlrLibranza.Value = Convert.ToInt32(dr["VlrLibranza"]);
                this.VlrCuota.Value = Convert.ToInt32(dr["VlrCuota"]);
                this.FechaLiquida.Value = Convert.ToDateTime(dr["FechaLiquida"]);
                this.CobranzaEstadoID.Value = Convert.ToString(dr["CobranzaEstadoID"]);
                this.NombreCobranzaEstado.Value = Convert.ToString(dr["NombreCobranzaEstado"]);
                this.CobranzaGestionID.Value = Convert.ToString(dr["CobranzaGestionID"]);
                this.NombreCobranzaGestion.Value = Convert.ToString(dr["NombreCobranzaGestion"]);
                this.Saldo_Libranza.Value = Convert.ToInt32(dr["Saldo_Libranza"]);
                this.FechaNomina.Value = Convert.ToDateTime(dr["FechaNomina"]);
                this.EstadoCruce.Value = Convert.ToByte(dr["EstadoCruce"]);
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
        public VistaQ_ccRecaudoMasivo()
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
            this.Libranza = new UDT_DocTerceroID();
            this.Prefijo = new UDT_Consecutivo();
            this.PagaduriaID = new UDT_PagaduriaID();
            this.Nombre_Pagaduria = new UDT_DescripTBase();
            this.CooperativaID = new UDT_CodigoGrl5();
            this.Nombre_Coop = new UDT_DescripTBase();
            this.TerceroID = new UDT_TerceroID();
            this.Nombre_Cliente = new UDT_DescripTBase();
            this.VlrLibranza = new UDT_Valor();
            this.VlrCuota = new UDT_Valor();
            this.FechaLiquida = new UDTSQL_smalldatetime();
            this.CobranzaEstadoID = new UDT_CodigoGrl10();
            this.NombreCobranzaEstado = new UDT_DescripTBase();
            this.CobranzaGestionID = new UDT_CodigoGrl10();
            this.NombreCobranzaGestion = new UDT_DescripTBase();
            this.Saldo_Libranza = new UDT_Valor();
            this.FechaNomina = new UDTSQL_smalldatetime();
            this.EstadoCruce = new UDTSQL_tinyint();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_DocTerceroID Libranza { get; set; }

        [DataMember]
        public UDT_Consecutivo Prefijo { get; set; }

        [DataMember]
        public UDT_PagaduriaID PagaduriaID { get; set; }

        [DataMember]
        public UDT_DescripTBase Nombre_Pagaduria { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 CooperativaID { get; set; }

        [DataMember]
        public UDT_DescripTBase Nombre_Coop { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DescripTBase Nombre_Cliente { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLiquida { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 CobranzaEstadoID { get; set; }

        [DataMember]
        public UDT_DescripTBase NombreCobranzaEstado { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 CobranzaGestionID { get; set; }

        [DataMember]
        public UDT_DescripTBase NombreCobranzaGestion { get; set; }

        [DataMember]
        public UDT_Valor Saldo_Libranza { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaNomina { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoCruce { get; set; }        

        #endregion

    }
}
