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
    /// Models DTO_pyTareaCliente
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyTareaCliente : DTO_MasterComplex
    {
        #region pyTrabajoRecurso
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyTareaCliente(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ClaseServicioDesc.Value = Convert.ToString(dr["ClaseServicioDesc"]);
                    this.TareaClienteDesc.Value = Convert.ToString(dr["TareaClienteDesc"]);
                    this.ServicioDesc.Value = Convert.ToString(dr["ServicioDesc"]);
                    this.MonedaFacturacionDesc.Value = Convert.ToString(dr["MonedaFacturacionDesc"]);
                }

                if (!string.IsNullOrEmpty(dr["ClaseServicioID"].ToString()))
                    this.ClaseServicioID.Value = Convert.ToString(dr["ClaseServicioID"]);
                if (!string.IsNullOrEmpty(dr["TareaCliente"].ToString()))
                    this.TareaCliente.Value = Convert.ToString(dr["TareaCliente"]);
                if (!string.IsNullOrEmpty(dr["ServicioID"].ToString()))
                    this.ServicioID.Value = Convert.ToString(dr["ServicioID"]);
                if (!string.IsNullOrEmpty(dr["MonedaFacturacion"].ToString()))
                    this.MonedaFacturacion.Value = Convert.ToString(dr["MonedaFacturacion"]);
                if (!string.IsNullOrEmpty(dr["Observaciones"].ToString()))
                    this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
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
        public DTO_pyTareaCliente(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ClaseServicioID.Value = Convert.ToString(dr["ClaseServicioID"]);
                this.ClaseServicioDesc.Value = Convert.ToString(dr["ClaseServicioDesc"]);
                this.TareaCliente.Value = Convert.ToString(dr["TareaCliente"]);
                this.TareaClienteDesc.Value = Convert.ToString(dr["TareaClienteDesc"]);
                this.ServicioID.Value = Convert.ToString(dr["ServicioID"]);
                this.ServicioDesc.Value = Convert.ToString(dr["ServicioDesc"]);
                this.MonedaFacturacion.Value = Convert.ToString(dr["MonedaFacturacion"]);
                this.MonedaFacturacionDesc.Value = Convert.ToString(dr["MonedaFacturacionDesc"]);
                this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_pyTareaCliente() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ClaseServicioID = new UDT_BasicID();
            this.ClaseServicioDesc = new UDT_Descriptivo();
            this.TareaCliente = new UDT_BasicID();
            this.TareaClienteDesc = new UDT_Descriptivo();
            this.ServicioID = new UDT_BasicID();
            this.ServicioDesc = new UDT_Descriptivo();
            this.MonedaFacturacion = new UDT_BasicID();
            this.MonedaFacturacionDesc = new UDT_Descriptivo();
            this.Observaciones = new UDT_DescripTExt();
        }

        public DTO_pyTareaCliente(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyTareaCliente(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID ClaseServicioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ClaseServicioDesc { get; set; }

        [DataMember]
        public UDT_BasicID TareaCliente { get; set; }

        [DataMember]
        public UDT_Descriptivo TareaClienteDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID ServicioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ServicioDesc { get; set; }

        [DataMember]
        public UDT_BasicID MonedaFacturacion { get; set; }

        [DataMember]
        public UDT_Descriptivo MonedaFacturacionDesc { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }        
    }

}
