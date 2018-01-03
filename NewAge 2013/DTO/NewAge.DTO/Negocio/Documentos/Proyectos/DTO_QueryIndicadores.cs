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
    /// Class Models DTO_QueryIndicadores
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryIndicadores
    {
        #region DTO_QueryIndicadores

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryIndicadores(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ReporteID.Value = dr["ReporteID"].ToString();
                this.DescReporte.Value = dr["DescReporte"].ToString();
                this.Formula.Value = dr["Formula"].ToString();
                this.Grupo.Value = dr["Grupo"].ToString();
                this.PresentaTipo.Value = dr["PresentaTipo"].ToString();

                if (!string.IsNullOrWhiteSpace(dr["Indicador"].ToString()))
                    this.Indicador.Value = Convert.ToDecimal(dr["Indicador"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["Factor"].ToString()))
                    this.Factor.Value = Convert.ToDecimal(dr["Factor"]);
                
                
                this.cUnidad.Value = dr["cUnidad"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryIndicadores()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            
            this.ReporteID = new UDTSQL_char(15);
            this.DescReporte = new UDTSQL_char(60);
            this.Grupo = new UDTSQL_char(15);
            this.PresentaTipo = new UDTSQL_char(1);
            this.Formula = new UDTSQL_char(100);
            this.Indicador =new UDTSQL_decimal ();
            this.Factor = new UDTSQL_decimal();
            this.Valor = new UDTSQL_decimal();
            this.cUnidad = new UDTSQL_char(20);
            this.Dato = new UDTSQL_char(50);
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDTSQL_char ReporteID { get; set; }

        [DataMember]
        public UDTSQL_char DescReporte { get; set; }

        [DataMember]
        public UDTSQL_char Grupo { get; set; }

        [DataMember]
        public UDTSQL_char PresentaTipo { get; set; }

        [DataMember]
        public UDTSQL_char Formula { get; set; }

        [DataMember]
        public UDTSQL_decimal Indicador { get; set; }

        [DataMember]
        public UDTSQL_decimal Valor { get; set; }

        [DataMember]
        public UDTSQL_decimal Factor { get; set; }


        [DataMember]
        public UDTSQL_char cUnidad { get; set; }

        [DataMember]
        public UDTSQL_char Dato { get; set; }

        #endregion
    }
    #endregion



}
