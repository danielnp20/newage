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
    public class DTO_ReportCxPFlujoSemanalDetallado
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReportCxPFlujoSemanalDetallado(IDataReader dr, bool isDetallado)
        {
            this.InitCols();
            try
            {
                if (isDetallado)
                {
                    if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                        this.TerceroID.Value = dr["TerceroID"].ToString();
                    if (!string.IsNullOrEmpty(dr["NombreTerc"].ToString()))
                        this.NombreTerc.Value = (dr["NombreTerc"].ToString());
                    if (!string.IsNullOrEmpty(dr["ValorNeto"].ToString()))
                        this.ValorNeto.Value = Convert.ToDecimal(dr["ValorNeto"]);
                    if (!string.IsNullOrEmpty(dr["MdaOrigen"].ToString()))
                        this.MdaOrigen.Value = (dr["MdaOrigen"].ToString());
                    if (!string.IsNullOrEmpty(dr["Factura"].ToString()))
                        this.Factura.Value = (dr["Factura"].ToString());
                    if (!string.IsNullOrEmpty(dr["Descripcion"].ToString()))
                        this.Descripcion.Value = (dr["Descripcion"].ToString());
                    if (!string.IsNullOrEmpty(dr["Semana1"].ToString()))
                        this.Semana1.Value = Convert.ToDecimal(dr["Semana1"]);
                    if (!string.IsNullOrEmpty(dr["Semana2"].ToString()))
                        this.Semana2.Value = Convert.ToDecimal(dr["Semana2"]);
                    if (!string.IsNullOrEmpty(dr["Semana3"].ToString()))
                        this.Semana3.Value = Convert.ToDecimal(dr["Semana3"]);
                    if (!string.IsNullOrEmpty(dr["Semana4"].ToString()))
                        this.Semana4.Value = Convert.ToDecimal(dr["Semana4"]);
                    if (!string.IsNullOrEmpty(dr["Semana5"].ToString()))
                        this.Semana5.Value = Convert.ToDecimal(dr["Semana5"]);
                    if (!string.IsNullOrEmpty(dr["Semana6"].ToString()))
                        this.Semana6.Value = Convert.ToDecimal(dr["Semana6"]);
                }
                if (!isDetallado)
                {
                    if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                        this.TerceroID.Value = dr["TerceroID"].ToString();
                    if (!string.IsNullOrEmpty(dr["NombreTerc"].ToString()))
                        this.NombreTerc.Value = (dr["NombreTerc"].ToString());
                    if (!string.IsNullOrEmpty(dr["Semana1"].ToString()))
                        this.Semana1.Value = Convert.ToDecimal(dr["Semana1"]);
                    if (!string.IsNullOrEmpty(dr["Semana2"].ToString()))
                        this.Semana2.Value = Convert.ToDecimal(dr["Semana2"]);
                    if (!string.IsNullOrEmpty(dr["Semana3"].ToString()))
                        this.Semana3.Value = Convert.ToDecimal(dr["Semana3"]);
                    if (!string.IsNullOrEmpty(dr["Semana4"].ToString()))
                        this.Semana4.Value = Convert.ToDecimal(dr["Semana4"]);
                    if (!string.IsNullOrEmpty(dr["Semana5"].ToString()))
                        this.Semana5.Value = Convert.ToDecimal(dr["Semana5"]);
                    if (!string.IsNullOrEmpty(dr["Semana6"].ToString()))
                        this.Semana6.Value = Convert.ToDecimal(dr["Semana6"]);
                    this.TotalMensual.Value = Semana1.Value + Semana2.Value + Semana3.Value + Semana4.Value + Semana5.Value + Semana6.Value;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportCxPFlujoSemanalDetallado()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroID = new UDT_TerceroID();
            this.NombreTerc = new UDT_Descriptivo();
            this.ValorNeto = new UDT_Valor();
            this.MdaOrigen = new UDTSQL_char(5);
            this.Factura = new UDTSQL_char(20);
            this.Descripcion = new UDT_Descriptivo();
            this.Semana1 = new UDT_Valor();
            this.Semana2 = new UDT_Valor();
            this.Semana3 = new UDT_Valor();
            this.Semana4 = new UDT_Valor();
            this.Semana5 = new UDT_Valor();
            this.Semana6 = new UDT_Valor();
            this.TotalMensual = new UDT_Valor();
        }
        #endregion

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo NombreTerc { get; set; }

        [DataMember]
        public UDT_Valor ValorNeto { get; set; }

        [DataMember]
        public UDTSQL_char MdaOrigen { get; set; }

        [DataMember]
        public UDTSQL_char Factura { get; set; }

        [DataMember]
        public UDT_Descriptivo Descripcion { get; set; }

        [DataMember]
        public UDT_Valor Semana1 { get; set; }

        [DataMember]
        public UDT_Valor Semana2 { get; set; }

        [DataMember]
        public UDT_Valor Semana3 { get; set; }

        [DataMember]
        public UDT_Valor Semana4 { get; set; }

        [DataMember]
        public UDT_Valor Semana5 { get; set; }

        [DataMember]
        public UDT_Valor Semana6 { get; set; }

        //Datos Adicionales

        [DataMember]
        public UDT_Valor TotalMensual { get; set; }
    }
}
