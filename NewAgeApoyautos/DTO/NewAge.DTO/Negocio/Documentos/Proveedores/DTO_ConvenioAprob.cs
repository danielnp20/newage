using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Models DTO_AnticipoAprobacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ConvenioAprob 
    {
        #region DTO_ConvenioAprob

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ConvenioAprob(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                try { this.ProveedorID.Value = dr["ProveedorID"].ToString(); }  catch (Exception) { };
                try { this.ProveedorNombre.Value = dr["ProveedorNombre"].ToString(); }  catch (Exception) { };
                try { this.Moneda.Value = dr["Moneda"].ToString(); }   catch (Exception) { };
                 try { this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]); }   catch (Exception) { };
                try { if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                      this.ValorDoc.Value = Convert.ToDecimal(dr["Valor"]);  }    catch (Exception) { };
                try { this.ProyectoID.Value = dr["ProyectoID"].ToString();}    catch (Exception) { };
                try { this.CentroCostoID.Value = dr["CentroCostoID"].ToString(); }    catch (Exception) { };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ConvenioAprob()
        {
            InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Aprobado = new UDT_SiNo();
            this.Rechazado = new UDT_SiNo();
            this.Observacion = new UDT_DescripTExt();
            this.NumeroDoc = new UDT_Consecutivo();
            this.DocumentoNro = new UDT_Consecutivo();
            this.Descriptivo = new UDT_DescripTBase();
            this.ProveedorID = new UDT_ProveedorID();
            this.ProveedorNombre = new UDT_DescripTBase();
            this.Moneda = new UDT_MonedaID();
            this.Fecha = new UDTSQL_smalldatetime();
            this.ValorDoc = new UDT_Valor();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.listConvenioSolicitudDet = new List<DTO_prSolicitudDespachoAprobDet>();
            this.listConvenioConsumoDet = new List<DTO_prConvenioConsumoDirectoAprobDet>();
            this.FileUrl = "";
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        public UDT_SiNo Rechazado { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        public UDT_DescripTBase ProveedorNombre { get; set; }

        [DataMember]
        public UDT_MonedaID Moneda { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_Valor ValorDoc { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public List<DTO_prSolicitudDespachoAprobDet> listConvenioSolicitudDet { get; set; }

        [DataMember]
        public List<DTO_prConvenioConsumoDirectoAprobDet> listConvenioConsumoDet { get; set; }

        [DataMember]
        public string FileUrl { get; set; }
        #endregion
    }

    //Detalle de las solicitudes de Despacho en la aprobacion
    [DataContract]
    [Serializable]
    public class DTO_prSolicitudDespachoAprobDet
    {
        public DTO_prSolicitudDespachoAprobDet(IDataReader dr)
        {
            InitDetCols();
            try
            {
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.DescriptivoCodBS.Value = dr["DescriptivoCodBS"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.DescriptivoRef.Value = dr["DescriptivoRef"].ToString();
                if (!string.IsNullOrEmpty(dr["CantidadDoc1"].ToString()))
                    this.CantidadDoc1.Value = Convert.ToDecimal(dr["CantidadDoc1"]);
                if(!string.IsNullOrEmpty(dr["IVAUni"].ToString()))
                    this.IVAUni.Value = Convert.ToDecimal(dr["IVAUni"]);
                if (!string.IsNullOrEmpty(dr["ValorUni"].ToString()))
                    this.ValorUni.Value = Convert.ToDecimal(dr["ValorUni"]);
                if (!string.IsNullOrEmpty(dr["ValorTotML"].ToString()))
                    this.ValorTotML.Value = Convert.ToDecimal(dr["ValorTotML"]);
                if (!string.IsNullOrEmpty(dr["ValorTotME"].ToString()))
                    this.ValorTotME.Value = Convert.ToDecimal(dr["ValorTotME"]);
                if (!string.IsNullOrEmpty(dr["IvaTotML"].ToString()))
                    this.IvaTotML.Value = Convert.ToDecimal(dr["IvaTotML"]);
                if (!string.IsNullOrEmpty(dr["IvaTotME"].ToString()))
                    this.IvaTotME.Value = Convert.ToDecimal(dr["IvaTotME"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_prSolicitudDespachoAprobDet()
        {
            InitDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.CodigoBSID = new UDT_CodigoBSID();
            this.DescriptivoCodBS = new UDT_Descriptivo();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.DescriptivoRef = new UDT_Descriptivo();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.CantidadDoc1 = new UDT_Cantidad();
            this.IVAUni = new UDT_Valor();
            this.ValorUni = new UDT_Valor();
            this.ValorTotML = new UDT_Valor();
            this.ValorTotME = new UDT_Valor();
            this.IvaTotML = new UDT_Valor();
            this.IvaTotME = new UDT_Valor();
        }

        #region Propiedades
        
        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Descriptivo DescriptivoCodBS { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Descriptivo DescriptivoRef { get; set; }
               
        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDoc1 { get; set; }

        [DataMember]
        public UDT_Valor IVAUni { get; set; }

        [DataMember]
        public UDT_Valor ValorUni { get; set; }

        [DataMember]
        public UDT_Valor ValorTotML { get; set; }

        [DataMember]
        public UDT_Valor ValorTotME { get; set; }

        [DataMember]
        public UDT_Valor IvaTotML { get; set; }

        [DataMember]
        public UDT_Valor IvaTotME { get; set; }

        #endregion
    }

    //Detalle de los Consumos de Proyecto de Convenio en la aprobacion
    [DataContract]
    [Serializable]
    public class DTO_prConvenioConsumoDirectoAprobDet
    {
        public DTO_prConvenioConsumoDirectoAprobDet(IDataReader dr)
        {
            InitDetCols();
            try
            {
                this.FechaPlanilla.Value = Convert.ToDateTime(dr["FechaPlanilla"]);
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.DescriptivoProv.Value = dr["DescriptivoProv"].ToString();
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.DescriptivoCodBS.Value = dr["DescriptivoCodBS"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["inReferenciaID"].ToString()))
                    this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DescriptivoRef"].ToString()))
                    this.DescriptivoRef.Value = dr["DescriptivoRef"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["SerialID"].ToString()))
                    this.SerialID.Value = dr["SerialID"].ToString();
                this.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
                this.NumeroDocOC.Value = Convert.ToInt32(dr["NumeroDocOC"]);
                this.PrefDocOC.Value = dr["PrefDocOC"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_prConvenioConsumoDirectoAprobDet()
        {
            InitDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.FechaPlanilla = new UDTSQL_datetime();
            this.ProveedorID = new UDT_ProveedorID();
            this.DescriptivoProv = new UDT_Descriptivo();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.DescriptivoCodBS = new UDT_Descriptivo();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.DescriptivoRef = new UDT_Descriptivo();
            this.SerialID = new  UDT_SerialID();
            this.Cantidad = new UDT_Cantidad();
            this.ValorDet = new UDT_Valor();
            this.ValorUniDet = new UDT_Valor();
            this.NumeroDocOC = new UDT_Consecutivo();
            this.PrefDocOC = new UDT_DescripTBase();
        }

        #region Propiedades

        [DataMember]
        public UDTSQL_datetime FechaPlanilla { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        public UDT_Descriptivo DescriptivoProv { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Descriptivo DescriptivoCodBS { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Descriptivo DescriptivoRef { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SerialID SerialID { get; set; }

        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }

        [DataMember]
        public UDT_Valor ValorUniDet { get; set; }

        [DataMember]
        public UDT_Valor ValorDet { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo NumeroDocOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase PrefDocOC { get; set; }

        #endregion
    }
}
