using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_coReporteLinea
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coReporteLinea : DTO_MasterComplex
    {
        #region DTO_coReporteLinea
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coReporteLinea(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ReporteDesc.Value = dr["ReporteDesc"].ToString();
                    this.NotaRevelacionDesc.Value = dr["NotaRevelacionDesc"].ToString();
                }

                this.ReporteID.Value = dr["ReporteID"].ToString();
                this.RepLineaID.Value = dr["RepLineaID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.TipoLinea.Value = Convert.ToByte(dr["TipoLinea"]);
                this.LineaInicial.Value = dr["LineaInicial"].ToString();
                this.LineaFinal.Value = dr["LineaFinal"].ToString();
                this.SaldoMvto.Value = Convert.ToByte(dr["SaldoMvto"]);
                this.TipoDetalle.Value = Convert.ToByte(dr["TipoDetalle"]);
                this.NotaRevelacionID.Value = dr["NotaRevelacionID"].ToString();
                this.DescripcionAgrupa.Value = dr["DescripcionAgrupa"].ToString();
                this.VlrTotalMLRepLinea.Value = 0;

            }
            catch(Exception e)
            {
              throw  e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coReporteLinea() : base()
        {
            InitCols();
            this.VlrTotalMLRepLinea.Value = 0;
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.ReporteID = new UDT_BasicID();
            this.ReporteDesc = new UDT_Descriptivo();
            this.RepLineaID = new UDT_RepLineaID();
            this.Descriptivo = new UDT_DescripUnFormat();
            this.TipoLinea = new UDTSQL_tinyint();
            this.LineaInicial = new UDT_RepLineaID();
            this.LineaFinal = new UDT_RepLineaID();
            this.SaldoMvto = new UDTSQL_tinyint();
            this.TipoDetalle = new UDTSQL_tinyint();
            this.NotaRevelacionID = new UDT_BasicID();
            this.NotaRevelacionDesc = new UDT_Descriptivo();
            this.DescripcionAgrupa = new UDT_DescripUnFormat();  
            //Adicionales
            this.SaldosMLRepLinea = new List<DTO_SaldosVista>();
            this.Detalle = new List<DTO_coReporteLinea>();
            this.VlrTotalMLRepLinea = new UDT_Valor();
            this.OrdenAgrupa = new UDT_Consecutivo();
            this.isSubTotal = false;
            
        }

        public DTO_coReporteLinea(DTO_MasterComplex comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_coReporteLinea(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID ReporteID { get; set; }

        [DataMember]
        public UDT_Descriptivo ReporteDesc { get; set; }

        [DataMember]
        public UDT_RepLineaID RepLineaID { get; set; }

        [DataMember]
        public UDT_DescripUnFormat Descriptivo { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoLinea { get; set; }

        [DataMember]
        public UDT_RepLineaID LineaInicial { get; set; }

        [DataMember]
        public UDT_RepLineaID LineaFinal { get; set; }

        [DataMember]
        public UDTSQL_tinyint SaldoMvto { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoDetalle { get; set; }

        [DataMember]
        public UDT_BasicID NotaRevelacionID { get; set; }

        [DataMember]
        public UDT_Descriptivo NotaRevelacionDesc { get; set; }

        [DataMember]
        public UDT_DescripUnFormat DescripcionAgrupa { get; set; }
    
        //Adicionales
        [DataMember]
        public List<DTO_SaldosVista> SaldosMLRepLinea { get; set; }

        [DataMember]
        public List<DTO_coReporteLinea> Detalle { get; set; }

        [DataMember]
        public UDT_Valor VlrTotalMLRepLinea { get; set; }

        [DataMember]
        public bool isSubTotal { get; set; }

        [DataMember]
        public UDT_Consecutivo OrdenAgrupa { get; set; }

        #region Valores
        [DataMember]
        public decimal VlrLin1 { get; set; }

        [DataMember]
        public decimal VlrLin2 { get; set; }

        [DataMember]
        public decimal VlrLin3 { get; set; }

        [DataMember]
        public decimal VlrLin4 { get; set; }

        [DataMember]
        public decimal VlrLin5 { get; set; }

        [DataMember]
        public decimal VlrLin6 { get; set; }

        [DataMember]
        public decimal VlrLin7 { get; set; }

        [DataMember]
        public decimal VlrLin8 { get; set; }

        [DataMember]
        public decimal VlrLin9 { get; set; }

        [DataMember]
        public decimal VlrLin10 { get; set; }

        [DataMember]
        public decimal VlrLin11 { get; set; }

        [DataMember]
        public decimal VlrLin12 { get; set; }

        [DataMember]
        public decimal VlrLin13 { get; set; }

        [DataMember]
        public decimal VlrLin14 { get; set; }

        [DataMember]
        public decimal VlrLin15 { get; set; }

        [DataMember]
        public decimal VlrLin16 { get; set; }

        [DataMember]
        public decimal VlrLin17 { get; set; }

        [DataMember]
        public decimal VlrLin18 { get; set; }

        [DataMember]
        public decimal VlrLin19 { get; set; }

        [DataMember]
        public decimal VlrLin20 { get; set; }

        [DataMember]
        public decimal VlrLin21 { get; set; }

        [DataMember]
        public decimal VlrLin22 { get; set; }

        [DataMember]
        public decimal VlrLin23 { get; set; }

        [DataMember]
        public decimal VlrLin24 { get; set; }

        [DataMember]
        public decimal VlrLin25 { get; set; }

        [DataMember]
        public decimal VlrLin26 { get; set; }

        [DataMember]
        public decimal VlrLin27 { get; set; }

        [DataMember]
        public decimal VlrLin28 { get; set; }

        [DataMember]
        public decimal VlrLin29 { get; set; }

        [DataMember]
        public decimal VlrLin30 { get; set; }

        [DataMember]
        public decimal VlrLin31 { get; set; }

        [DataMember]
        public decimal VlrLin32 { get; set; }

        [DataMember]
        public decimal VlrLin33 { get; set; }

        [DataMember]
        public decimal VlrLin34 { get; set; }

        [DataMember]
        public decimal VlrLin35 { get; set; }

        [DataMember]
        public decimal VlrLin36 { get; set; }

        [DataMember]
        public decimal VlrLin37 { get; set; }

        [DataMember]
        public decimal VlrLin38 { get; set; }

        [DataMember]
        public decimal VlrLin39 { get; set; }

        [DataMember]
        public decimal VlrLin40 { get; set; }

        [DataMember]
        public decimal VlrLin41 { get; set; }

        [DataMember]
        public decimal VlrLin42 { get; set; }

        [DataMember]
        public decimal VlrLin43 { get; set; }

        [DataMember]
        public decimal VlrLin44 { get; set; }

        [DataMember]
        public decimal VlrLin45 { get; set; }

        [DataMember]
        public decimal VlrLin46 { get; set; }

        [DataMember]
        public decimal VlrLin47 { get; set; }

        [DataMember]
        public decimal VlrLin48 { get; set; }

        [DataMember]
        public decimal VlrLin49 { get; set; }

        [DataMember]
        public decimal VlrLin50 { get; set; }

        #endregion
        #region Descripciones

        [DataMember]
        public string DescLin1 { get; set; }

        [DataMember]
        public string DescLin2 { get; set; }

        [DataMember]
        public string DescLin3 { get; set; }

        [DataMember]
        public string DescLin4 { get; set; }

        [DataMember]
        public string DescLin5 { get; set; }

        [DataMember]
        public string DescLin6 { get; set; }

        [DataMember]
        public string DescLin7 { get; set; }

        [DataMember]
        public string DescLin8 { get; set; }

        [DataMember]
        public string DescLin9 { get; set; }

        [DataMember]
        public string DescLin10 { get; set; }

        [DataMember]
        public string DescLin11 { get; set; }

        [DataMember]
        public string DescLin12 { get; set; }

        [DataMember]
        public string DescLin13 { get; set; }

        [DataMember]
        public string DescLin14 { get; set; }

        [DataMember]
        public string DescLin15 { get; set; }

        [DataMember]
        public string DescLin16 { get; set; }

        [DataMember]
        public string DescLin17 { get; set; }

        [DataMember]
        public string DescLin18 { get; set; }

        [DataMember]
        public string DescLin19 { get; set; }

        [DataMember]
        public string DescLin20 { get; set; }

        [DataMember]
        public string DescLin21 { get; set; }

        [DataMember]
        public string DescLin22 { get; set; }

        [DataMember]
        public string DescLin23 { get; set; }

        [DataMember]
        public string DescLin24 { get; set; }

        [DataMember]
        public string DescLin25 { get; set; }

        [DataMember]
        public string DescLin26 { get; set; }

        [DataMember]
        public string DescLin27 { get; set; }

        [DataMember]
        public string DescLin28 { get; set; }

        [DataMember]
        public string DescLin29 { get; set; }

        [DataMember]
        public string DescLin30 { get; set; }

        [DataMember]
        public string DescLin31 { get; set; }

        [DataMember]
        public string DescLin32 { get; set; }

        [DataMember]
        public string DescLin33 { get; set; }

        [DataMember]
        public string DescLin34 { get; set; }

        [DataMember]
        public string DescLin35 { get; set; }

        [DataMember]
        public string DescLin36 { get; set; }

        [DataMember]
        public string DescLin37 { get; set; }

        [DataMember]
        public string DescLin38 { get; set; }

        [DataMember]
        public string DescLin39 { get; set; }

        [DataMember]
        public string DescLin40 { get; set; }

        [DataMember]
        public string DescLin41 { get; set; }

        [DataMember]
        public string DescLin42 { get; set; }

        [DataMember]
        public string DescLin43 { get; set; }

        [DataMember]
        public string DescLin44 { get; set; }

        [DataMember]
        public string DescLin45 { get; set; }

        [DataMember]
        public string DescLin46 { get; set; }

        [DataMember]
        public string DescLin47 { get; set; }

        [DataMember]
        public string DescLin48 { get; set; }

        [DataMember]
        public string DescLin49 { get; set; }

        [DataMember]
        public string DescLin50 { get; set; }

        #endregion

    }
}
