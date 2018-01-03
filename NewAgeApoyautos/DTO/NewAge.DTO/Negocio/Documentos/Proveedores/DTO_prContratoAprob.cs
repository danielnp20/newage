﻿using System;
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
    /// <summary>
    /// Class Models DTO_prContratoAprob
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prContratoAprob
    {
        #region DTO_prContratoAprob

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prContratoAprob(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.PrefijoID.Value = dr["PrefijoID"].ToString();
                this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                this.PrefDoc = this.PrefijoID.Value + " - " + this.DocumentoNro.Value.Value.ToString();
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.ProveedorNombre.Value = dr["ProveedorNombre"].ToString();
                this.MonedaOrden.Value = dr["MonedaOrden"].ToString();
                this.MonedaPago.Value = dr["MonedaPago"].ToString();
                this.ValorTotalML.Value = Convert.ToDecimal(dr["ValorTotalML"]);
                this.UsuarioID.Value = dr["UsuarioID"].ToString();
                this.Justificacion.Value = dr["Justificacion"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prContratoAprob()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Aprobado = new UDT_SiNo();
            this.Rechazado = new UDT_SiNo();
            this.Observacion = new UDT_DescripTExt();
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.Fecha = new UDTSQL_datetime();
            this.DocumentoID = new UDT_DocumentoID();
            this.PrefijoID = new UDT_PrefijoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.ProveedorID = new UDT_ProveedorID();
            this.ProveedorNombre = new UDT_DescripTBase();
            this.MonedaOrden = new UDT_MonedaID();
            this.MonedaPago = new UDT_MonedaID();
            this.ValorTotalML = new UDT_Valor();
            this.UsuarioID = new UDT_UsuarioID();
            this.Justificacion = new UDT_DescripTExt();
            this.FileUrl = "";
            this.ContratoAprobDet = new List<DTO_prContratoAprobDet>();
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
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        public string PrefDoc { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        public UDT_DescripTBase ProveedorNombre { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaOrden { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaPago { get; set; }

        [DataMember]
        public UDT_Valor ValorTotalML { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioID { get; set; }

        [DataMember]
        public UDT_DescripTExt Justificacion { get; set; }

        [DataMember]
        public string FileUrl { get; set; }

        [DataMember]
        public List<DTO_prContratoAprobDet> ContratoAprobDet { get; set; }
        #endregion
    }

    /// <summary>
    /// Class Models DTO_prContratoAprob
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prContratoAprobDet
    {
        public DTO_prContratoAprobDet(IDataReader dr)
        {
            InitDetCols();
            try
            {
                this.ConsecutivoDetaID.Value = Convert.ToInt32(dr["ConsecutivoDetaID"]);
                this.ContratoDocuID.Value = Convert.ToInt32(dr["ContratoDocuID"]);
                this.ContratoDetaID.Value = Convert.ToInt32(dr["ContratoDetaID"]);
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();                 
                this.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CantidadCont"].ToString()))
                    this.CantidadCont.Value = Convert.ToInt32(dr["CantidadCont"]);
                this.PrefijoSolID.Value = dr["PrefijoSolID"].ToString();
                this.SolicitudDocuID.Value = Convert.ToInt32(dr["SolicitudDocuID"]);
                this.SolicitudDetaID.Value = Convert.ToInt32(dr["SolicitudDetaID"]);
                this.PrefDocSol = this.PrefijoSolID.Value + " - " + this.SolicitudDocuID.Value.Value.ToString();
                this.ValorUni.Value = Convert.ToDecimal(dr["ValorUni"]);
                this.ValorTotML.Value = Convert.ToDecimal(dr["ValorTotML"]);
                this.IvaTotML.Value = Convert.ToDecimal(dr["IvaTotML"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_prContratoAprobDet()
        {
            InitDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.ConsecutivoDetaID = new UDT_Consecutivo();
            this.ContratoDocuID = new UDT_Consecutivo();
            this.ContratoDetaID = new UDT_Consecutivo();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.Descriptivo = new UDT_DescripTExt();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.CantidadCont = new UDT_Cantidad();
            this.PrefijoSolID = new UDT_PrefijoID();
            this.SolicitudDocuID = new UDT_Consecutivo();
            this.SolicitudDetaID = new UDT_Consecutivo();
            this.ValorUni = new UDT_Valor();
            this.ValorTotML = new UDT_Valor();
            this.IvaTotML = new UDT_Valor();
            this.SolicitudCargos = new List<DTO_prSolicitudCargos>();
        }

        #region Propiedades
        
        [DataMember]
        public UDT_Consecutivo ConsecutivoDetaID { get; set; }

        [DataMember]
        public UDT_Consecutivo ContratoDocuID { get; set; }

        [DataMember]
        public UDT_Consecutivo ContratoDetaID { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        public UDT_DescripTExt Descriptivo { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }
               
        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadCont { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoSolID { get; set; }

        [DataMember]
        public UDT_Consecutivo SolicitudDocuID { get; set; }

        [DataMember]
        public UDT_Consecutivo SolicitudDetaID { get; set; }

        [DataMember]
        public string PrefDocSol { get; set; }

        [DataMember]
        public UDT_Valor ValorUni { get; set; }

        [DataMember]
        public UDT_Valor ValorTotML { get; set; }

        [DataMember]
        public UDT_Valor IvaTotML { get; set; }

        [DataMember]
        public List<DTO_prSolicitudCargos> SolicitudCargos { get; set; }
        #endregion
    }
}