using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_ReportLibranzasDetalle
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos, Lista de Componentes Ordenados
        /// Posiciones en la lista:
        /// 0. Capital
        /// 1. Interes
        /// 2. Seguro
        /// 3. Saldos a favor
        /// 4. Mora
        /// 5. MultiPrepago
        /// 6. Aportes
        /// 7. Pagaduria
        /// 8. Usura
        /// 9. CONFED
        /// 10. Interes Anticipado
        /// 11. Aportes Anticipado
        /// 12. Seguro Anticipado
        /// 13. Afiliaciones
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReportLibranzasDetalle(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.Nombre.Value = Convert.ToString(dr["Nombre"]);
                this.TotalValor.Value = Convert.ToDecimal(dr["TotalValor"]);
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportLibranzasDetalle()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Nombre = new UDTSQL_char(15);
            this.TotalValor = new UDT_Valor();

            //this.VlrCapital = new UDT_Valor();
            //this.NombreVlrCapital = new UDTSQL_char(15);
            //this.VlrInteres = new UDT_Valor();
            //this.NombreVlrInteres = new UDTSQL_char(15);
            //this.VlrSeguro = new UDT_Valor();
            //this.NombreVlrSeguro = new UDTSQL_char(15);
            //this.VlrSaldosFavor = new UDT_Valor();
            //this.NombreVlrSaldosFavor = new UDTSQL_char(15);
            //this.VlrMora = new UDT_Valor();
            //this.NombreVlrMora = new UDTSQL_char(15);
            //this.VlrMultaPrepago = new UDT_Valor();
            //this.NombreVlrMultaPrepago = new UDTSQL_char(15);
            //this.VlrAportes = new UDT_Valor();
            //this.NombreVlrAportes = new UDTSQL_char(15);
            //this.VlrPagaduria = new UDT_Valor();
            //this.NombreVlrPagaduria = new UDTSQL_char(15);
            //this.VlrUsura = new UDT_Valor();
            //this.NombreVlrUsura = new UDTSQL_char(15);
            //this.VlrCONFED = new UDT_Valor();
            //this.NombreVlrCONFED = new UDTSQL_char(15);
            //this.VlrInteresAnti = new UDT_Valor();
            //this.NombreVlrInteresAnti = new UDTSQL_char(15);
            //this.VlrAportesAnti = new UDT_Valor();
            //this.NombreVlrAportesAnti = new UDTSQL_char(15);
            //this.VlrSeguroAnti = new UDT_Valor();
            //this.NombreVlrSeguroAnti = new UDTSQL_char(15);
            //this.VlrAfiliacion = new UDT_Valor();
            //this.NombreVlrAfiliacion = new UDTSQL_char(15);
        }
        #endregion
        
        [DataMember]
        public UDTSQL_char Nombre { get; set; }

        [DataMember]
        public UDT_Valor TotalValor { get; set; }



        //[DataMember]
        //public UDT_Valor VlrCapital { get; set; }
        
        //[DataMember]
        //[NotImportable]
        //public UDTSQL_char NombreVlrCapital { get; set; }
        
        //[DataMember]
        //public UDT_Valor VlrInteres { get; set; }
        
        //[DataMember]
        //[NotImportable]
        //public UDTSQL_char NombreVlrInteres { get; set; }
        
        //[DataMember]
        //public UDT_Valor VlrSeguro { get; set; }
        
        //[DataMember]
        //[NotImportable]
        //public UDTSQL_char NombreVlrSeguro { get; set; }
        
        //[DataMember]
        //public UDT_Valor VlrSaldosFavor { get; set; }
        
        //[DataMember]
        //[NotImportable]
        //public UDTSQL_char NombreVlrSaldosFavor { get; set; }
        
        //[DataMember]
        //public UDT_Valor VlrMora { get; set; }
        
        //[DataMember]
        //[NotImportable]
        //public UDTSQL_char NombreVlrMora { get; set; }
        
        //[DataMember]
        //public UDT_Valor VlrMultaPrepago { get; set; }
        
        //[DataMember]
        //[NotImportable]
        //public UDTSQL_char NombreVlrMultaPrepago { get; set; }
        
        //[DataMember]
        //public UDT_Valor VlrAportes { get; set; }
        
        //[DataMember]
        //[NotImportable]
        //public UDTSQL_char NombreVlrAportes { get; set; }
        
        //[DataMember]
        //public UDT_Valor VlrPagaduria { get; set; }
        
        //[DataMember]
        //[NotImportable]
        //public UDTSQL_char NombreVlrPagaduria { get; set; }
        
        //[DataMember]
        //public UDT_Valor VlrUsura { get; set; }
        
        //[DataMember]
        //[NotImportable]
        //public UDTSQL_char NombreVlrUsura { get; set; }
        
        //[DataMember]
        //public UDT_Valor VlrCONFED { get; set; }
        
        //[DataMember]
        //[NotImportable]
        //public UDTSQL_char NombreVlrCONFED { get; set; }
        
        //[DataMember]
        //public UDT_Valor VlrInteresAnti { get; set; }
        
        //[DataMember]
        //[NotImportable]
        //public UDTSQL_char NombreVlrInteresAnti { get; set; }
        
        //[DataMember]
        //public UDT_Valor VlrAportesAnti { get; set; }
        
        //[DataMember]
        //[NotImportable]
        //public UDTSQL_char NombreVlrAportesAnti { get; set; }
        
        //[DataMember]
        //public UDT_Valor VlrSeguroAnti { get; set; }
        
        //[DataMember]
        //[NotImportable]
        //public UDTSQL_char NombreVlrSeguroAnti { get; set; }
        
        //[DataMember]
        //public UDT_Valor VlrAfiliacion { get; set; }
        
        //[DataMember]
        //[NotImportable]
        //public UDTSQL_char NombreVlrAfiliacion { get; set; }
    }
}
