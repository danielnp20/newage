using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Reportes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Models DTO_DigitaSolicitudDecisor
    [Serializable]
    [DataContract]
    public class DTO_DigitaSolicitudDecisor
    /// </summary>
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_DigitaSolicitudDecisor()
        {
            this.SolicituDocu = new DTO_ccSolicitudDocu();            
            this.DocCtrl = new DTO_glDocumentoControl();
            this.DatosPersonales = new List<DTO_drSolicitudDatosPersonales>();
            this.DatosVehiculo = new DTO_drSolicitudDatosVehiculo();
            this.OtrosDatos = new DTO_drSolicitudDatosOtros();
            this.DataCreditoUbica = new List<DTO_ccSolicitudDataCreditoUbica>();
            this.ActividadesChequeo = new List<DTO_glDocumentoChequeoLista>();
            this.DataCreditoDatos = new List<DTO_ccSolicitudDataCreditoDatos>();
            this.DataCreditoScore = new List<DTO_ccSolicitudDataCreditoScore>();
            this.DataCreditoQuanto = new List<DTO_ccSolicitudDataCreditoQuanto>();
            this.DatosChequeados = new List<DTO_drSolicitudDatosChequeados>();
            this.DocCtrlPrenda1 = new DTO_glDocumentoControl();
            this.DocCtrlHipoteca1 = new DTO_glDocumentoControl();
        }

        #region Propiedades

        [DataMember]
        public DTO_ccSolicitudDocu SolicituDocu
        {
            get;
            set;
        }

        [DataMember]
        public DTO_drSolicitudDatosOtros  OtrosDatos
        {
            get;
            set;
        }

        [DataMember]
        public DTO_glDocumentoControl DocCtrl
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_drSolicitudDatosPersonales> DatosPersonales
        {
            get;
            set;
        }

        [DataMember]
        public DTO_drSolicitudDatosVehiculo DatosVehiculo
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_ccSolicitudDataCreditoUbica> DataCreditoUbica
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_glDocumentoChequeoLista> ActividadesChequeo
        {
            get;
            set;
        }
        [DataMember]
        public List<DTO_ccSolicitudDataCreditoDatos> DataCreditoDatos
        {
            get;
            set;
        }
        [DataMember]
        public List<DTO_ccSolicitudDataCreditoScore> DataCreditoScore
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_ccSolicitudDataCreditoQuanto> DataCreditoQuanto
        {
            get;
            set;
        }
        [DataMember]
        public List<DTO_drSolicitudDatosChequeados> DatosChequeados
        {
            get;
            set;
        }

        [DataMember]
        public DTO_glDocumentoControl DocCtrlPrenda1
        {
            get;
            set;
        }

        [DataMember]
        public DTO_glDocumentoControl DocCtrlHipoteca1
        {
            get;
            set;
        }



   
        #endregion

    }
}