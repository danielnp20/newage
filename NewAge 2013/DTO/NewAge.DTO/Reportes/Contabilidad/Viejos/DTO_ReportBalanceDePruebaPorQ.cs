using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio.Reportes;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte Balance De Prueba por trimestre
    /// </summary>
    public class DTO_ReportBalanceDePruebaPorQ : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
       public DTO_ReportBalanceDePruebaPorQ(IDataReader dr)
        {
            YearID = new DateTime(Convert.ToInt32(dr["Año"]), 1, 1); ;
            CuentaID = dr["CuentaID"].ToString();
            CuentaDesc = dr["CuentaDesc"].ToString();
            MaxLengthInd = Convert.ToInt16(dr["MaxLengthInd"]);
            InicialML = Convert.ToDecimal(dr["InicialML"]);
            InicialME = Convert.ToDecimal(dr["InicialME"]);
            TotalML_Year = Convert.ToDecimal(dr["Y_ML"]);
            TotalME_Year = Convert.ToDecimal(dr["Y_ME"]);
           
            Q1 = new DTO_Q1(dr);
            Q2 = new DTO_Q2(dr);
            Q3 = new DTO_Q3(dr);
            Q4 = new DTO_Q4(dr);
        }      

        #region Propiedades

       /// <summary>
       /// Año del reporte
       /// </summary>
        [DataMember]
        public DateTime YearID { get; set; }

        /// <summary>
        /// Cuenat ID
        /// </summary>
        [DataMember]
        public string CuentaID { get; set; }

        /// <summary>
        /// Descripcion de la Cuenta
        /// </summary>
        [DataMember]
        public string CuentaDesc { get; set; }

        /// <summary>
        /// Indicador que longitud de la Cuenta es igual a Mascara de la Cuanta
        /// </summary>
        [DataMember]
        public int MaxLengthInd { get; set; }

        /// <summary>
        /// Saldo Inicial (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal InicialML { get; set; }

        /// <summary>
        /// Saldo Inicial (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal InicialME { get; set; }

        /// <summary>
        /// Total Movimiento en el año (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal TotalML_Year { get; set; }

        /// <summary>
        /// Total Movimiento en el año (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal TotalME_Year { get; set; }

        /// <summary>
        /// Primero Trimestre
        /// </summary>
        [DataMember]
        public DTO_Q1 Q1 { get; set; }

        /// <summary>
        /// Segundo Trimestre
        /// </summary>
        [DataMember]
        public DTO_Q2 Q2 { get; set; }

        /// <summary>
        /// Tercero Trimestre
        /// </summary>
        [DataMember]
        public DTO_Q3 Q3 { get; set; }

        /// <summary>
        /// Quatro Trimestre
        /// </summary>
        [DataMember]
        public DTO_Q4 Q4 { get; set; }
        #endregion
    }

        [DataContract]
        [Serializable]

        /// <summary>
        /// Clase del Primero Trimestre
        /// </summary>
       public class DTO_Q1
       {
           /// <summary>
           /// Constructor con DataReader
           /// </summary>
              public DTO_Q1(IDataReader dr)
            {
                MovML_01_Q1 = Convert.ToDecimal(dr["MovML_01"]);
                MovME_01_Q1 = Convert.ToDecimal(dr["MovME_01"]);
                MovML_02_Q1 = Convert.ToDecimal(dr["MovML_02"]);
                MovME_02_Q1 = Convert.ToDecimal(dr["MovME_02"]);
                MovML_03_Q1 = Convert.ToDecimal(dr["MovML_03"]);
                MovME_03_Q1 = Convert.ToDecimal(dr["MovME_03"]);
                TotalML_Q1 = Convert.ToDecimal(dr["Q1_ML"]);
                TotalME_Q1 = Convert.ToDecimal(dr["Q1_ME"]);
            }

            #region Propiedades
            /// <summary>
            /// Movimiento en Enero (Moneda Local)
            /// </summary>
            [DataMember]
            public decimal MovML_01_Q1 { get; set; }

            /// <summary>
            /// Movimiento en Enero (Moneda Extranjera)
            /// </summary>
            [DataMember]
            public decimal MovME_01_Q1 { get; set; }

            /// <summary>
            /// Movimiento en Febrero (Moneda Local)
            /// </summary>
            [DataMember]
            public decimal MovML_02_Q1 { get; set; }

            /// <summary>
            /// Movimiento en Febrero (Moneda Extranjera)
            /// </summary>
            [DataMember]
            public decimal MovME_02_Q1 { get; set; }

            /// <summary>
            /// Movimiento en Marzo (Moneda Local)
            /// </summary>
            [DataMember]
            public decimal MovML_03_Q1 { get; set; }

            /// <summary>
            /// Movimiento en Marzo (Moneda Extranjera)
            /// </summary>
            [DataMember]
            public decimal MovME_03_Q1 { get; set; }

            /// <summary>
            /// Total Movimiento en el primero trimestre (Moneda Local)
            /// </summary>
           [DataMember]
            public decimal TotalML_Q1 { get; set; }

           /// <summary>
           /// Total Movimiento en el primero trimestre  (Moneda Extranjera)
           /// </summary>
           [DataMember]
            public decimal TotalME_Q1 { get; set; }
            #endregion
       }

        [DataContract]
        [Serializable]

        /// <summary>
        /// Clase del Segundo Trimestre
        /// </summary>
    public class DTO_Q2
       {
           /// <summary>
           /// Constructor con DataReader
           /// </summary>
           public DTO_Q2(IDataReader dr)
           {
               MovML_01_Q2 = Convert.ToDecimal(dr["MovML_04"]);
               MovME_01_Q2 = Convert.ToDecimal(dr["MovME_04"]);
               MovML_02_Q2 = Convert.ToDecimal(dr["MovML_05"]);
               MovME_02_Q2 = Convert.ToDecimal(dr["MovME_05"]);
               MovML_03_Q2 = Convert.ToDecimal(dr["MovML_06"]);
               MovME_03_Q2 = Convert.ToDecimal(dr["MovME_06"]);
               TotalML_Q2 = Convert.ToDecimal(dr["Q2_ML"]);
               TotalME_Q2 = Convert.ToDecimal(dr["Q2_ME"]);
           }

           #region Propiedades
           /// <summary>
           /// Movimiento en Abril (Moneda Local)
           /// </summary>
           [DataMember]
           public decimal MovML_01_Q2 { get; set; }

           /// <summary>
           /// Movimiento en Abril (Moneda Extranjera)
           /// </summary>
           [DataMember]
           public decimal MovME_01_Q2 { get; set; }

           /// <summary>
           /// Movimiento en Mayo (Moneda Local)
           /// </summary>
           [DataMember]
           public decimal MovML_02_Q2 { get; set; }

           /// <summary>
           /// Movimiento en Mayo (Moneda Extranjera)
           /// </summary>
           [DataMember]
           public decimal MovME_02_Q2 { get; set; }

           /// <summary>
           /// Movimiento en Junio (Moneda Local)
           /// </summary>
           [DataMember]
           public decimal MovML_03_Q2 { get; set; }

           /// <summary>
           /// Movimiento en Junio (Moneda Extranjera)
           /// </summary>
           [DataMember]
           public decimal MovME_03_Q2 { get; set; }

           /// <summary>
           /// Total Movimiento en el segundo trimestre (Moneda Local)
           /// </summary>
           [DataMember]
           public decimal TotalML_Q2 { get; set; }

           /// <summary>
           /// Total Movimiento en el segundo trimestre (Moneda Extranjera)
           /// </summary>
           [DataMember]
           public decimal TotalME_Q2 { get; set; }
           #endregion
       }

        [DataContract]
        [Serializable]

        /// <summary>
        /// Clase del Tercero Trimestre
        /// </summary>
       public class DTO_Q3
       {
           /// <summary>
           /// Constructor con DataReader
           /// </summary>
           public DTO_Q3(IDataReader dr)
           {
               MovML_01_Q3 = Convert.ToDecimal(dr["MovML_07"]);
               MovME_01_Q3 = Convert.ToDecimal(dr["MovME_07"]);
               MovML_02_Q3 = Convert.ToDecimal(dr["MovML_08"]);
               MovME_02_Q3 = Convert.ToDecimal(dr["MovME_08"]);
               MovML_03_Q3 = Convert.ToDecimal(dr["MovML_09"]);
               MovME_03_Q3 = Convert.ToDecimal(dr["MovME_09"]);
               TotalML_Q3 = Convert.ToDecimal(dr["Q3_ML"]);
               TotalME_Q3 = Convert.ToDecimal(dr["Q3_ME"]);
           }

           #region Propiedades
           /// <summary>
           /// Movimiento en Julio (Moneda Local)
           /// </summary>
            [DataMember]
           public decimal MovML_01_Q3 { get; set; }
            /// <summary>
            /// Movimiento en Julio (Moneda Extranjera)
            /// </summary>
           [DataMember]
           public decimal MovME_01_Q3 { get; set; }

           /// <summary>
           /// Movimiento en Agosto (Moneda Local)
           /// </summary>
           [DataMember]
           public decimal MovML_02_Q3 { get; set; }

           /// <summary>
           /// Movimiento en Agosto (Moneda Extranjera)
           /// </summary>
           [DataMember]
           public decimal MovME_02_Q3 { get; set; }

           /// <summary>
           /// Movimiento en Septiembre (Moneda Local)
           /// </summary>
           [DataMember]
           public decimal MovML_03_Q3 { get; set; }

           /// <summary>
           /// Movimiento en Septiembre (Moneda Extranjera)
           /// </summary>
           [DataMember]
           public decimal MovME_03_Q3 { get; set; }

           /// <summary>
           /// Total Movimiento en el tercero trimestre (Moneda Local)
           /// </summary>
           [DataMember]
           public decimal TotalML_Q3 { get; set; }

           /// <summary>
           /// Total Movimiento en el tercero trimestre (Moneda Extranjera)
           /// </summary>
           [DataMember]
           public decimal TotalME_Q3 { get; set; }
           #endregion
       }

        [DataContract]
        [Serializable]

        /// <summary>
        /// Clase del Quatro Trimestre
        /// </summary>
       public class DTO_Q4
       {
           /// <summary>
           /// Constructor con DataReader
           /// </summary>
           public DTO_Q4(IDataReader dr)
           {
               MovML_01_Q4 = Convert.ToDecimal(dr["MovML_10"]);
               MovME_01_Q4 = Convert.ToDecimal(dr["MovME_10"]);
               MovML_02_Q4 = Convert.ToDecimal(dr["MovML_11"]);
               MovME_02_Q4 = Convert.ToDecimal(dr["MovME_11"]);
               MovML_03_Q4 = Convert.ToDecimal(dr["MovML_12"]);
               MovME_03_Q4 = Convert.ToDecimal(dr["MovME_12"]);
               TotalML_Q4 = Convert.ToDecimal(dr["Q4_ML"]);
               TotalME_Q4 = Convert.ToDecimal(dr["Q4_ME"]);
           }

           #region Propiedades

           /// <summary>
           /// Movimiento en Octubre (Moneda Local)
           /// </summary>
            [DataMember]
           public decimal MovML_01_Q4 { get; set; }

            /// <summary>
            /// Movimiento en Octubre (Moneda Extranjera)
            /// </summary>
           [DataMember]
           public decimal MovME_01_Q4 { get; set; }

           /// <summary>
           /// Movimiento en Noviebmre (Moneda Local)
           /// </summary>
           [DataMember]
           public decimal MovML_02_Q4 { get; set; }

           /// <summary>
           /// Movimiento en Noviebmre (Moneda Extranjera)
           /// </summary>
           [DataMember]
           public decimal MovME_02_Q4 { get; set; }

           /// <summary>
           /// Movimiento en Diciembre (Moneda Local)
           /// </summary>
           [DataMember]
           public decimal MovML_03_Q4 { get; set; }

           /// <summary>
           /// Movimiento en Diciembre (Moneda Extranjera)
           /// </summary>
           [DataMember]
           public decimal MovME_03_Q4 { get; set; }

           /// <summary>
           /// Total Movimiento en el quatro trimestre (Moneda Local)
           /// </summary>
           [DataMember]
           public decimal TotalML_Q4 { get; set; }
            
           /// <summary>
           /// Total Movimiento en el quatro trimestre (Moneda Extranjera)
           /// </summary>
           [DataMember]
           public decimal TotalME_Q4 { get; set; }
           #endregion
       }

}
