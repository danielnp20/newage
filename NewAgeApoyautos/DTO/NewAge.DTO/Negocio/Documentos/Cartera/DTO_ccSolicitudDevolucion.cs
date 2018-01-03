using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// 
    /// Models DTO_ccSolicitudDevolucion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSolicitudDevolucion
    {
        #region DTO_ccSolicitudDevolucion

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudDevolucion(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.NumeroDEV.Value = Convert.ToByte(dr["NumeroDEV"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaDEV"].ToString()))
                    this.FechaDEV.Value = Convert.ToDateTime(dr["FechaDEV"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaRAD"].ToString()))
                    this.FechaRAD.Value = Convert.ToDateTime(dr["FechaRAD"]);
                this.seUsuarioID.Value = Convert.ToInt32(dr["seUsuarioID"]);
                this.ActividadFlujoID.Value = dr["ActividadFlujoID"].ToString();
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudDevolucion()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumeroDEV = new UDTSQL_tinyint();
            this.FechaDEV = new UDTSQL_smalldatetime();
            this.FechaRAD = new UDTSQL_smalldatetime();
            this.ActividadFlujoID = new UDT_ActividadFlujoID();
            this.seUsuarioID = new  UDT_seUsuarioID();
            this.Consecutivo = new UDT_Consecutivo();
            this.Detalle = new List<DTO_ccSolicitudDevolucionDeta>();
            this.UsuarioID = new UDT_UsuarioID();
            this.UsuarioDesc = new UDT_Descriptivo();
            this.ActividadFlujoDesc = new UDT_Descriptivo();
        }

        #endregion

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint NumeroDEV { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDEV { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaRAD { get; set; }

        [DataMember]
        public UDT_seUsuarioID seUsuarioID { get; set; }

        [DataMember]
        public UDT_ActividadFlujoID ActividadFlujoID { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public List<DTO_ccSolicitudDevolucionDeta> Detalle { get; set; }

        //Adicionales

        [DataMember]
        public UDT_UsuarioID UsuarioID { get; set; }

        [DataMember]
        public UDT_Descriptivo UsuarioDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadFlujoDesc { get; set; }




    }
}
