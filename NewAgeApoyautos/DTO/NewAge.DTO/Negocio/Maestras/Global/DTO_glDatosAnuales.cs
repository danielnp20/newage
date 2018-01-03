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
    /// Models DTO_glDatosAnuales
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glDatosAnuales : DTO_MasterBasic
    {
        #region DTO_glDatosAnuales
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glDatosAnuales(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {     
                this.Valor1.Value = Convert.ToDecimal(dr["Valor1"]);
                this.Valor2.Value = Convert.ToDecimal(dr["Valor2"]);
                this.Valor3.Value = Convert.ToDecimal(dr["Valor3"]);
                this.Valor4.Value = Convert.ToDecimal(dr["Valor4"]);
                this.Valor5.Value = Convert.ToDecimal(dr["Valor5"]);
                this.Valor6.Value = Convert.ToDecimal(dr["Valor6"]);
                this.Valor7.Value = Convert.ToDecimal(dr["Valor7"]);
                this.Valor8.Value = Convert.ToDecimal(dr["Valor8"]);
                this.Valor9.Value = Convert.ToDecimal(dr["Valor9"]);
                this.Valor10.Value = Convert.ToDecimal(dr["Valor10"]);
                this.Valor11.Value = Convert.ToDecimal(dr["Valor11"]);
                this.Valor12.Value = Convert.ToDecimal(dr["Valor12"]);
                this.Valor13.Value = Convert.ToDecimal(dr["Valor13"]);
                this.Valor14.Value = Convert.ToDecimal(dr["Valor14"]);
                this.Valor15.Value = Convert.ToDecimal(dr["Valor15"]);
                this.Valor16.Value = Convert.ToDecimal(dr["Valor16"]);
                this.Valor17.Value = Convert.ToDecimal(dr["Valor17"]);
                this.Valor18.Value = Convert.ToDecimal(dr["Valor18"]);
                this.Valor19.Value = Convert.ToDecimal(dr["Valor19"]);
                this.Valor20.Value = Convert.ToDecimal(dr["Valor20"]);
                this.Tasa1.Value = dr["Tasa1"].ToString();
                this.Tasa2.Value = dr["Tasa2"].ToString();
                this.Tasa3.Value = dr["Tasa3"].ToString();
                this.Tasa4.Value = dr["Tasa4"].ToString();
                this.Tasa5.Value = dr["Tasa5"].ToString();
                this.Tasa6.Value = dr["Tasa6"].ToString();
                this.Tasa7.Value = dr["Tasa7"].ToString();
                this.Tasa8.Value = dr["Tasa8"].ToString();
                this.Tasa9.Value = dr["Tasa9"].ToString();
                this.Tasa10.Value = dr["Tasa10"].ToString();
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glDatosAnuales()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
    
            this.Valor1 = new UDT_Valor();
            this.Valor2 = new UDT_Valor();
            this.Valor3 = new UDT_Valor();
            this.Valor4 = new UDT_Valor();
            this.Valor5 = new UDT_Valor();
            this.Valor6 = new UDT_Valor();
            this.Valor7 = new UDT_Valor();
            this.Valor8 = new UDT_Valor();
            this.Valor9 = new UDT_Valor();
            this.Valor10 = new UDT_Valor();
            this.Valor11 = new UDT_Valor();
            this.Valor12 = new UDT_Valor();
            this.Valor13 = new UDT_Valor();
            this.Valor14 = new UDT_Valor();
            this.Valor15 = new UDT_Valor();
            this.Valor16 = new UDT_Valor();
            this.Valor17 = new UDT_Valor();
            this.Valor18 = new UDT_Valor();
            this.Valor19 = new UDT_Valor();
            this.Valor20 = new UDT_Valor();
            this.Tasa1 = new UDT_BasicID();
            this.Tasa2 = new UDT_BasicID();
            this.Tasa3 = new UDT_BasicID();
            this.Tasa4 = new UDT_BasicID();
            this.Tasa5 = new UDT_BasicID();
            this.Tasa6= new UDT_BasicID();
            this.Tasa7 = new UDT_BasicID();
            this.Tasa8 = new UDT_BasicID();
            this.Tasa9 = new UDT_BasicID();
            this.Tasa10 = new UDT_BasicID();
        }

        public DTO_glDatosAnuales(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glDatosAnuales(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_Valor Valor1 { get; set; }

        [DataMember]
        public UDT_Valor Valor2 { get; set; }

        [DataMember]
        public UDT_Valor Valor3 { get; set; }

        [DataMember]
        public UDT_Valor Valor4 { get; set; }

        [DataMember]
        public UDT_Valor Valor5 { get; set; }

        [DataMember]
        public UDT_Valor Valor6 { get; set; }

        [DataMember]
        public UDT_Valor Valor7 { get; set; }

        [DataMember]
        public UDT_Valor Valor8 { get; set; }

        [DataMember]
        public UDT_Valor Valor9 { get; set; }

        [DataMember]
        public UDT_Valor Valor10 { get; set; }

        [DataMember]
        public UDT_Valor Valor11 { get; set; }

        [DataMember]
        public UDT_Valor Valor12 { get; set; }

        [DataMember]
        public UDT_Valor Valor13 { get; set; }

        [DataMember]
        public UDT_Valor Valor14 { get; set; }

        [DataMember]
        public UDT_Valor Valor15 { get; set; }

        [DataMember]
        public UDT_Valor Valor16 { get; set; }

        [DataMember]
        public UDT_Valor Valor17 { get; set; }

        [DataMember]
        public UDT_Valor Valor18 { get; set; }

        [DataMember]
        public UDT_Valor Valor19 { get; set; }

        [DataMember]
        public UDT_Valor Valor20 { get; set; }

        [DataMember]
        public UDT_BasicID Tasa1 { get; set; }

        [DataMember]
        public UDT_BasicID Tasa2 { get; set; }

        [DataMember]
        public UDT_BasicID Tasa3 { get; set; }

        [DataMember]
        public UDT_BasicID Tasa4 { get; set; }

        [DataMember]
        public UDT_BasicID Tasa5 { get; set; }

        [DataMember]
        public UDT_BasicID Tasa6 { get; set; }

        [DataMember]
        public UDT_BasicID Tasa7 { get; set; }

        [DataMember]
        public UDT_BasicID Tasa8 { get; set; }

        [DataMember]
        public UDT_BasicID Tasa9 { get; set; }

        [DataMember]
        public UDT_BasicID Tasa10 { get; set; }

    }

}

