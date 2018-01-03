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
    public class DTO_ReportLibranzas
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReportLibranzas(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrEmpty(dr["Libranza"].ToString()))
                    this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                if (!string.IsNullOrEmpty(dr["ClienteID"].ToString()))
                    this.ClienteID.Value = Convert.ToString(dr["ClienteID"]);
                if (!string.IsNullOrEmpty(dr["NombreCliente"].ToString()))
                    this.Nombre.Value = Convert.ToString(dr["NombreCliente"]);
                if (!string.IsNullOrEmpty(dr["NombreAsesor"].ToString()))
                    this.Asesor.Value = Convert.ToString(dr["NombreAsesor"]);
                if (!string.IsNullOrEmpty(dr["Plazo"].ToString()))
                    this.Plazo.Value = Convert.ToInt32(dr["Plazo"]);
                if (!string.IsNullOrEmpty(dr["PagaduriaID"].ToString()))
                    this.PagaduriaID.Value = Convert.ToString(dr["PagaduriaID"]);
                if (!string.IsNullOrEmpty(dr["VlrCuota"].ToString()))
                    this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                if (!string.IsNullOrEmpty(dr["VlrSolicitado"].ToString()))
                    this.VlrSolicitado.Value = Convert.ToDecimal(dr["VlrSolicitado"]);
                if (!string.IsNullOrEmpty(dr["VlrPrestamo"].ToString()))
                    this.VlrCredito.Value = Convert.ToDecimal(dr["VlrPrestamo"]);
                if (!string.IsNullOrEmpty(dr["VlrLibranza"].ToString()))
                    this.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                if (!string.IsNullOrEmpty(dr["VlrGiro"].ToString()))
                    this.VlrGiro.Value = Convert.ToDecimal(dr["VlrGiro"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportLibranzas()
        {
            this.InitCols();
        }

        public void LlenarDetalle(List<DTO_ReportLibranzasDetalle> Detalle)
        {
            int cont = 1;
            foreach (DTO_ReportLibranzasDetalle i in Detalle)
            {
                if (cont == 1)
                    this.VlrCapital.Value = i.TotalValor.Value;
                if (cont == 2)
                    this.VlrInteres.Value = i.TotalValor.Value;
                if (cont == 3)
                    this.VlrSeguro.Value = i.TotalValor.Value;
                if (cont == 4)
                    this.VlrAportes.Value = i.TotalValor.Value;
                if (cont == 5)
                    this.VlrAfiliacion.Value = i.TotalValor.Value;
                if (cont == 6)
                    this.VlrEstudioCredito.Value = i.TotalValor.Value;
                if (cont == 7)
                    this.VlrEstudioJuridico.Value = i.TotalValor.Value;
                if (cont == 8)
                    this.VlrComision.Value = i.TotalValor.Value;
                if (cont == 9)
                    this.VlrFNG.Value = i.TotalValor.Value;
                if (cont == 10)
                    this.VlrInteresAnti.Value = i.TotalValor.Value;
                if (cont == 11)
                    this.VlrAportesAnti.Value = i.TotalValor.Value;
                cont++;
            }
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDTSQL_int();
            this.Libranza = new UDTSQL_int();
            this.ClienteID = new UDT_ClienteID();
            this.Nombre = new UDT_Descriptivo();
            this.Asesor = new UDT_Descriptivo();
            this.Plazo = new UDTSQL_int();
            this.PagaduriaID = new UDT_PagaduriaID();
            this.VlrCuota = new UDT_Valor();
            this.VlrSolicitado = new UDT_Valor();
            this.VlrCredito = new UDT_Valor();
            this.VlrLibranza = new UDT_Valor();
            this.VlrGiro = new UDT_Valor();
            this.VlrCapital = new UDT_Valor();
            this.VlrInteres = new UDT_Valor();
            this.VlrSeguro = new UDT_Valor();
            this.VlrAportes = new UDT_Valor();
            this.VlrAfiliacion = new UDT_Valor();
            this.VlrEstudioCredito = new UDT_Valor();
            this.VlrEstudioJuridico = new UDT_Valor();
            this.VlrComision = new UDT_Valor();
            this.VlrFNG = new UDT_Valor();
            this.VlrInteresAnti = new UDT_Valor();
            this.VlrAportesAnti = new UDT_Valor();
            
            //this.VlrSeguroAnti = new UDT_Valor();            
            //this.VlrSaldosFavor = new UDT_Valor();            
            //this.VlrMora = new UDT_Valor();            
            //this.VlrMultaPrepago = new UDT_Valor();                                    
            //this.VlrUsura = new UDT_Valor();
            //this.NombreVlrUsura = new UDTSQL_char(15);                                                          
         }
        #endregion

        #region Propiedades

        [DataMember]
        public UDTSQL_int NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_int Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDT_Descriptivo Asesor { get; set; }

        [DataMember]
        public UDTSQL_int Plazo { get; set; }

        [DataMember]
        public UDT_PagaduriaID PagaduriaID { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrSolicitado { get; set; }

        [DataMember]
        public UDT_Valor VlrCredito { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }
        
        [DataMember]
        public UDT_Valor VlrGiro { get; set; }

        [DataMember]
        public UDT_Valor VlrCapital { get; set; }

        [DataMember]
        public UDT_Valor VlrInteres { get; set; }

        [DataMember]
        public UDT_Valor VlrSeguro { get; set; }

        [DataMember]
        public UDT_Valor VlrAportes { get; set; }

        [DataMember]
        public UDT_Valor VlrAfiliacion { get; set; }

        [DataMember]
        public UDT_Valor VlrEstudioCredito { get; set; }

        [DataMember]
        public UDT_Valor VlrEstudioJuridico { get; set; }

        [DataMember]
        public UDT_Valor VlrComision { get; set; }

        [DataMember]
        public UDT_Valor VlrFNG { get; set; }

        [DataMember]
        public UDT_Valor VlrInteresAnti { get; set; }

        [DataMember]
        public UDT_Valor VlrAportesAnti { get; set; }
        

        #endregion
    }
}
