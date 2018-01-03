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
    /// Class Models DTO_QueryComiteTecnico
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryComiteTecnico
    {
        #region DTO_QueryComiteTecnico

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryComiteTecnico(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ResponsableDesc.Value = dr["ResponsableDesc"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.ClienteDesc.Value = dr["ClienteDesc"].ToString();
                this.PrefDoc  = dr["PrefDoc"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaInicio"].ToString()))
                    this.FechaInicio.Value = Convert.ToDateTime(dr["FechaInicio"]);
                if (!string.IsNullOrEmpty(dr["FechaFin"].ToString()))
                    this.FechaFin.Value = Convert.ToDateTime(dr["FechaFin"]);
                if (!string.IsNullOrEmpty(dr["Version"].ToString()))
                    this.Version.Value = Convert.ToInt32(dr["Version"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryComiteTecnico()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.ResponsableDesc = new UDT_Descriptivo();
            this.ProyectoID = new UDT_ProyectoID();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.ClienteID = new UDT_ClienteID();
            this.ClienteDesc = new UDT_Descriptivo();
            this.FechaInicio = new UDTSQL_smalldatetime();
            this.FechaFin = new UDTSQL_smalldatetime();
            this.Version = new UDTSQL_int();
            this.FileUrl = "";
            this.Detalle = new List<DTO_QueryComiteTecnicoTareas>();
            this.OrdenCompraPendientes = new List<DTO_QueryComiteCompras>();
            this.OrdenCompraEnProceso = new List<DTO_QueryComiteCompras>();
            this.RecibidoPendientes = new List<DTO_QueryComiteCompras>();
            this.FacturaPendientes = new List<DTO_QueryComiteCompras>();
        }
        #endregion

        #region Propiedades    

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Descriptivo ResponsableDesc { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public string PrefDoc { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo ClienteDesc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicio { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFin { get; set; }

        [DataMember]
        public UDTSQL_int Version { get; set; }

        [DataMember]
        public int DiasAtrasoProy { get; set; }

        [DataMember]
        public int DiasAtrasoCompras { get; set; }

        [DataMember]
        public int DiasAtrasoEntrega { get; set; }

        [DataMember]
        public int DiasAtrasoOC { get; set; }

        [DataMember]
        public int DiasAtrasoOCEnProceso { get; set; }

        [DataMember]
        public int DiasAtrasoRec { get; set; }

        [DataMember]
        public int DiasAtrasoFact { get; set; }

        [DataMember]
        public List<DTO_QueryComiteTecnicoTareas> Detalle { get; set; }

        [DataMember]
        public List<DTO_QueryComiteCompras> OrdenCompraPendientes { get; set; }

        [DataMember]
        public List<DTO_QueryComiteCompras> OrdenCompraEnProceso { get; set; }

        [DataMember]
        public List<DTO_QueryComiteCompras> RecibidoPendientes { get; set; }

        [DataMember]
        public List<DTO_QueryComiteCompras> FacturaPendientes { get; set; }

        [DataMember]
        public List<DTO_pyProyectoMvto> Movimientos { get; set; }

        [DataMember]
        public List<DTO_pyActaTrabajoDeta> ActaTrabajosDeta { get; set; }

        [DataMember]
        public List<DTO_QueryTrazabilidad> ResumenTrazabilidad { get; set; }

        [DataMember]
        public string FileUrl { get; set; }
        #endregion
    }
    #endregion

    #region Tareas
    /// <summary>
    /// Class Models DTO_QueryComiteTecnicoTareas
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryComiteTecnicoTareas
    {
        public DTO_QueryComiteTecnicoTareas(IDataReader dr)
        {
            InitDetCols();
            try
            {
                this.TareaID.Value = Convert.ToString(dr["TareaID"]);
                this.TareaCliente.Value = Convert.ToString(dr["TareaCliente"]);
                this.Descriptivo.Value = Convert.ToString(dr["Descriptivo"]);
                if (!string.IsNullOrEmpty(dr["FechaInicio"].ToString()))
                    this.FechaInicio.Value = Convert.ToDateTime(dr["FechaInicio"]);
                if (!string.IsNullOrEmpty(dr["FechaFin"].ToString()))
                    this.FechaFin.Value = Convert.ToDateTime(dr["FechaFin"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_QueryComiteTecnicoTareas()
        {
            InitDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.TareaID = new UDT_CodigoGrl();
            this.TareaCliente = new UDT_CodigoGrl20();
            this.Descriptivo = new UDT_DescripUnFormat();
            this.FechaInicio = new UDTSQL_smalldatetime();
            this.FechaSolicitud = new UDTSQL_smalldatetime();
            this.FechaTrabajo = new UDTSQL_smalldatetime();
            this.FechaFin = new UDTSQL_smalldatetime();
            this.Detalle = new  List<DTO_pyProyectoMvto>();
        }

        #region Propiedades

        [DataMember]
        public UDT_CodigoGrl20 TareaCliente { get; set; }

        [DataMember]
        public UDT_CodigoGrl TareaID { get; set; }        

        [DataMember]
        public UDT_DescripUnFormat Descriptivo { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicio { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaSolicitud { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaTrabajo { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFin { get; set; }

        [DataMember]
        public int DiasAtrasoProy { get; set; }

        [DataMember]
        public int DiasAtrasoCompras { get; set; }

        [DataMember]
        public int DiasAtrasoEntrega { get; set; }

        [DataMember]
        public List<DTO_pyProyectoMvto> Detalle { get; set; }
        #endregion
    }
    #endregion

    #region Compras
    /// <summary>
    /// Class Models DTO_QueryComiteCompras
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryComiteCompras
    {
        #region DTO_QueryComiteCompras

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryComiteCompras(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["NumDocCompra"].ToString()))
                    this.NumDocCompra.Value = Convert.ToInt32(dr["NumDocCompra"]);
                if (!string.IsNullOrEmpty(dr["DocIDCompra"].ToString()))
                    this.DocIDCompra.Value = Convert.ToInt32(dr["DocIDCompra"]);
                this.PrefDocCompra = dr["PrefDocCompra"].ToString();
                if (!string.IsNullOrEmpty(dr["Estado"].ToString()))
                    this.Estado.Value = Convert.ToByte(dr["Estado"]);
                if (!string.IsNullOrEmpty(dr["FechaCreacion"].ToString()))
                    this.FechaCreacion.Value = Convert.ToDateTime(dr["FechaCreacion"]);
                if (!string.IsNullOrEmpty(dr["FechaAjustada"].ToString()))
                    this.FechaAjustada.Value = Convert.ToDateTime(dr["FechaAjustada"]);
                this.UsuarioResp.Value = dr["UsuarioResp"].ToString();
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.ProveedorDesc.Value = dr["ProveedorDesc"].ToString();
                this.BodegaDesc.Value = dr["BodegaDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaEntrega"].ToString()))
                    this.FechaEntrega.Value = Convert.ToDateTime(dr["FechaEntrega"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryComiteCompras()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumDocCompra = new UDT_Consecutivo();
            this.DocIDCompra = new UDT_Consecutivo();
            this.Estado = new UDTSQL_int();
            this.FechaCreacion = new UDTSQL_smalldatetime();
            this.UsuarioResp = new UDT_Descriptivo();
            this.ProveedorID = new UDT_ProveedorID();
            this.ProveedorDesc = new UDT_Descriptivo();
            this.BodegaDesc = new UDT_Descriptivo();   
                  
            this.FechaEntrega = new UDTSQL_smalldatetime();
            this.FechaAjustada = new UDTSQL_smalldatetime();     
            this.FileUrl = "";
            this.Detalle = new List<DTO_prDetalleDocu>();
        }
        #endregion

        #region Propiedades    

        [DataMember]
        public UDT_Consecutivo NumDocCompra { get; set; }

        [DataMember]
        public string PrefDocCompra { get; set; }

        [DataMember]
        public UDT_Consecutivo DocIDCompra { get; set; }

        [DataMember]
        public UDTSQL_int Estado { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCreacion { get; set; }

        [DataMember]
        public UDT_Descriptivo UsuarioResp { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProveedorDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo BodegaDesc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaEntrega { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaAjustada { get; set; }

        [DataMember]
        public int DiasAtrasoOC { get; set; }

        [DataMember]
        public int DiasAtrasoOCEnProceso { get; set; }

        [DataMember]
        public int DiasAtrasoRec { get; set; }

        [DataMember]
        public int DiasAtrasoFact { get; set; }

        [DataMember]
        public List<DTO_prDetalleDocu> Detalle { get; set; }

        [DataMember]
        public string FileUrl { get; set; }
        #endregion
    }
    #endregion

    #region Recursos
    /// <summary>
    /// Class Models DTO_QueryComiteTecnicoRecursos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryComiteTecnicoRecursos
    {
        public DTO_QueryComiteTecnicoRecursos(IDataReader dr)
        {
            InitDetCols();
            try
            {
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_QueryComiteTecnicoRecursos()
        {
            InitDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.CodigoBSID = new UDT_CodigoBSID();
            this.Descriptivo = new UDT_DescripTExt();

            this.SolicitudCargos = new List<DTO_prSolicitudCargos>();
        }

        #region Propiedades

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        public UDT_DescripTExt Descriptivo { get; set; }

        [DataMember]
        public List<DTO_prSolicitudCargos> SolicitudCargos { get; set; }
        #endregion
    } 
    #endregion
}
