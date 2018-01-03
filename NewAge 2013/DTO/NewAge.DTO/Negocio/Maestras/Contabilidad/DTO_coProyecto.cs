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
    /// Models DTO_coProyecto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coProyecto : DTO_MasterHierarchyBasic
    {
        #region DTO_coProyecto
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coProyecto(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.PryCapitalTrabajoDesc.Value = dr["PryCapitalTrabajoDesc"].ToString();
                    this.OperacionDesc.Value = dr["OperacionDesc"].ToString();
                    this.LocFisicaDesc.Value = dr["LocFisicaDesc"].ToString();
                    this.ContratoDesc.Value = dr["ContratoDesc"].ToString();
                    this.ProyecPresDesc.Value = dr["ProyecPresDesc"].ToString();
                    this.AreaPresupuestalDesc.Value = dr["AreaPresupuestalDesc"].ToString();
                    this.ActividadDesc.Value = dr["ActividadDesc"].ToString();
                    this.UsuarioRespDesc.Value = dr["UsuarioRespDesc"].ToString();
                    this.UsuarioResponsable.Value = dr["UsuarioID1"].ToString();
                }
                else
                {
                    if (!string.IsNullOrEmpty(dr["UsuarioResponsable"].ToString()))
                        this.UsuarioResponsable.Value = dr["UsuarioResponsable"].ToString();
                }

                this.PryCapitalTrabajo.Value = dr["PryCapitalTrabajo"].ToString();
                this.OperacionID.Value = dr["OperacionID"].ToString();
                this.LocFisicaID.Value = dr["LocFisicaID"].ToString();
                this.ContratoID.Value = dr["ContratoID"].ToString();
                this.ProyectoPresupID.Value = dr["ProyectoPresupID"].ToString();
                this.AreaPresupuestalID.Value = dr["AreaPresupuestalID"].ToString();
                this.TipoComercial.Value = Convert.ToByte(dr["TipoComercial"]);
                this.PresupuestalInd.Value = Convert.ToBoolean(dr["PresupuestalInd"]);
                this.SoloRiesgoInd.Value = Convert.ToBoolean(dr["SoloRiesgoInd"]);
                this.InactivaSolicitudesInd.Value = Convert.ToBoolean(dr["InactivaSolicitudesInd"]);
                this.ActividadID.Value = dr["ActividadID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ProyectoTipo"].ToString()))
                    this.ProyectoTipo.Value = Convert.ToByte(dr["ProyectoTipo"]);
                if (!string.IsNullOrWhiteSpace(dr["ActivoID"].ToString()))
                    this.ActivoID.Value = Convert.ToInt32(dr["ActivoID"]);
                this.FApertura.Value = Convert.ToDateTime(dr["FApertura"]);
                if (!string.IsNullOrWhiteSpace(dr["FCierre"].ToString()))
                    this.FCierre.Value = Convert.ToDateTime(dr["FCierre"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasEstimados"].ToString()))
                    this.DiasEstimados.Value = Convert.ToInt32(dr["DiasEstimados"]);
                if (!string.IsNullOrWhiteSpace(dr["ResponsableCliente"].ToString()))
                    this.ResponsableCliente.Value = Convert.ToString(dr["ResponsableCliente"]);
                if (!string.IsNullOrWhiteSpace(dr["CorreoRespCliente"].ToString()))
                    this.CorreoRespCliente.Value = Convert.ToString(dr["CorreoRespCliente"]);
                if (!string.IsNullOrWhiteSpace(dr["ConsActaEntrega"].ToString()))
                    this.ConsActaEntrega.Value = Convert.ToInt32(dr["ConsActaEntrega"]);
                if (!string.IsNullOrWhiteSpace(dr["ConsActaTrabajo"].ToString()))
                    this.ConsActaTrabajo.Value = Convert.ToInt32(dr["ConsActaTrabajo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coProyecto()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PryCapitalTrabajo = new UDT_BasicID();
            this.PryCapitalTrabajoDesc = new UDT_Descriptivo();
            this.OperacionID = new UDT_BasicID();
            this.OperacionDesc = new UDT_Descriptivo();
            this.ProyectoPresupID = new UDT_BasicID();
            this.ProyecPresDesc = new UDT_Descriptivo();
            this.AreaPresupuestalID = new UDT_BasicID();
            this.AreaPresupuestalDesc = new UDT_Descriptivo();
            this.TipoComercial = new UDTSQL_tinyint();
            this.PresupuestalInd = new UDT_SiNo();
            this.SoloRiesgoInd = new UDT_SiNo();
            this.InactivaSolicitudesInd = new UDT_SiNo();
            this.ActividadID = new UDT_BasicID();
            this.ActividadDesc = new UDT_Descriptivo();
            this.ActivoID = new UDT_ActivoID();
            this.ProyectoTipo = new UDTSQL_tinyint();
            this.FApertura = new UDTSQL_smalldatetime();
            this.FCierre = new UDTSQL_smalldatetime();
            this.DiasEstimados = new UDTSQL_int();
            this.UsuarioResponsable = new UDT_BasicID();
            this.UsuarioRespDesc = new UDT_Descriptivo();
            this.ResponsableCliente = new UDTSQL_char(60);
            this.CorreoRespCliente = new UDTSQL_char(60);
            this.LocFisicaID = new UDT_BasicID();
            this.LocFisicaDesc = new UDT_Descriptivo();
            this.ContratoID = new UDT_BasicID();
            this.ContratoDesc = new UDT_Descriptivo();
            this.ConsActaEntrega = new UDTSQL_int();
            this.ConsActaTrabajo = new UDTSQL_int();
        }

        public DTO_coProyecto(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coProyecto(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_BasicID PryCapitalTrabajo { get; set; }

        [DataMember]
        public UDT_Descriptivo PryCapitalTrabajoDesc { get; set; }

        [DataMember]
        public UDT_BasicID OperacionID { get; set; }

        [DataMember]
        public UDT_Descriptivo OperacionDesc { get; set; }

        [DataMember]
        public UDT_BasicID LocFisicaID { get; set; }

        [DataMember]
        public UDT_Descriptivo LocFisicaDesc { get; set; }

        [DataMember]
        public UDT_BasicID ContratoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ContratoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ProyectoPresupID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyecPresDesc { get; set; }

        [DataMember]
        public UDT_BasicID AreaPresupuestalID { get; set; }

        [DataMember]
        public UDT_Descriptivo AreaPresupuestalDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoComercial { get; set; }

        [DataMember]
        public UDT_SiNo PresupuestalInd { get; set; }

        [DataMember]
        public UDT_SiNo SoloRiesgoInd { get; set; }

        [DataMember]
        public UDT_SiNo InactivaSolicitudesInd { get; set; }

        [DataMember]
        public UDT_BasicID ActividadID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint ProyectoTipo { get; set; }

        [DataMember]
        public UDT_ActivoID ActivoID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FApertura { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FCierre { get; set; }

        [DataMember]
        public UDTSQL_int DiasEstimados { get; set; }

        [DataMember]
        public UDT_BasicID UsuarioResponsable { get; set; }

        [DataMember]
        public UDT_Descriptivo UsuarioRespDesc { get; set; }

        [DataMember]
        public UDTSQL_char ResponsableCliente { get; set; }

        [DataMember]
        public UDTSQL_char CorreoRespCliente { get; set; }

        [DataMember]
        public UDTSQL_int ConsActaEntrega { get; set; }

        [DataMember]
        public UDTSQL_int ConsActaTrabajo { get; set; }
        #endregion
    }
}
