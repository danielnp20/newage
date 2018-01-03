using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_plSobreEjecucion
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plSobreEjecucion()
        {
            this.InitCols();
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plSobreEjecucion(bool setValues)
        {
            this.InitCols();
            this.CantidadDOC.Value = 0;
            this.ValorOCLocML.Value = 0;
            this.ValorOCLocME.Value = 0;
            this.ValorOCExtME.Value = 0;
            this.ValorOCExtML.Value = 0;
            this.CantidadSOL.Value = 0;
            this.CtoOrigenLocML.Value = 0;
            this.CtoOrigenLocME.Value = 0;
            this.CtoOrigenExtME.Value = 0;
            this.CtoOrigenExtML.Value = 0;
            this.CantidadPTO.Value = 0;
            this.PtoMesLocML.Value = 0;
            this.PtoMesLocME.Value = 0;
            this.PtoMesExtME.Value = 0;
            this.PtoMesExtML.Value = 0;
            this.PtoTotalLocML.Value = 0;
            this.PtoTotalLocME.Value = 0;
            this.PtoTotalExtME.Value = 0;
            this.PtoTotalExtML.Value = 0;
            this.CompActLocML.Value = 0;
            this.CompActLocME.Value = 0;
            this.CompActExtME.Value = 0;
            this.CompActExtML.Value = 0;
            this.RecibidoLocML.Value = 0;
            this.RecibidoLocME.Value = 0;
            this.RecibidoExtME.Value = 0;
            this.RecibidoExtML.Value = 0;
            this.CtoInicialLocML.Value = 0;
            this.ocProcesoLocML.Value = 0;
            this.ocProcesoExtME.Value = 0;
            this.CtoInicialExtME.Value = 0;
            this.PtoInicialLocML.Value = 0;
            this.PtoInicialExtME.Value = 0;
            this.CompInicialocML.Value = 0;
            this.CompinicialExtME.Value = 0;
            this.RecInicialLocML.Value = 0;
            this.RecInicialExtME.Value = 0;
            this.ocProcesoInicialLocML.Value = 0;
            this.ocProcesoInicialExtME.Value = 0;
        }

        /// <summary>
        /// Constructor de lectura
        /// </summary>
        /// <param name="dr"></param>
        public DTO_plSobreEjecucion(IDataReader dr)
        {
            InitCols();

            try
            {
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.ProyectoID.Value = Convert.ToString(dr["ProyectoID"]);
                this.CentroCostoID.Value = Convert.ToString(dr["CentroCostoID"]);
                this.LineaPresupuestoID.Value = Convert.ToString(dr["LineaPresupuestoID"]);
                this.CodigoBSID.Value = Convert.ToString(dr["CodigoBSID"]);
                this.PrefijoID.Value = Convert.ToString(dr["PrefijoID"]);
                this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                this.TipoDocumento.Value = Convert.ToByte(dr["TipoDocumento"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["ConsecutivoDetaID"].ToString()))
                    this.ConsecutivoDetaID.Value = Convert.ToInt32(dr["ConsecutivoDetaID"]);
                this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["MonedaOrigen"].ToString()))
                    this.MonedaOrigen.Value = Convert.ToByte(dr["MonedaOrigen"]);
                this.AreaAprobacion.Value = Convert.ToString(dr["AreaAprobacion"]);
                this.Estado.Value = Convert.ToByte(dr["Estado"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoAprobacion"].ToString()))
                    this.TipoAprobacion.Value = Convert.ToByte(dr["TipoAprobacion"]);
                this.ProveedorID.Value = Convert.ToString(dr["ProveedorID"]);
                if (!string.IsNullOrWhiteSpace(dr["TasaOC"].ToString()))
                    this.TasaOC.Value = Convert.ToDecimal(dr["TasaOC"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadDOC"].ToString()))
                    this.CantidadDOC.Value = Convert.ToDecimal(dr["CantidadDOC"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorOCLocML"].ToString()))
                    this.ValorOCLocML.Value = Convert.ToDecimal(dr["ValorOCLocML"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorOCLocME"].ToString()))
                    this.ValorOCLocME.Value = Convert.ToDecimal(dr["ValorOCLocME"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorOCExtME"].ToString()))
                    this.ValorOCExtME.Value = Convert.ToDecimal(dr["ValorOCExtME"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorOCExtML"].ToString()))
                    this.ValorOCExtML.Value = Convert.ToDecimal(dr["ValorOCExtML"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadSOL"].ToString()))
                    this.CantidadSOL.Value = Convert.ToDecimal(dr["CantidadSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["CtoOrigenLocML"].ToString()))
                    this.CtoOrigenLocML.Value = Convert.ToDecimal(dr["CtoOrigenLocML"]);
                if (!string.IsNullOrWhiteSpace(dr["CtoOrigenLocME"].ToString()))
                    this.CtoOrigenLocME.Value = Convert.ToDecimal(dr["CtoOrigenLocME"]);
                if (!string.IsNullOrWhiteSpace(dr["CtoOrigenExtME"].ToString()))
                    this.CtoOrigenExtME.Value = Convert.ToDecimal(dr["CtoOrigenExtME"]);
                if (!string.IsNullOrWhiteSpace(dr["CtoOrigenExtML"].ToString()))
                    this.CtoOrigenExtML.Value = Convert.ToDecimal(dr["CtoOrigenExtML"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadPTO"].ToString()))
                    this.CantidadPTO.Value = Convert.ToDecimal(dr["CantidadPTO"]);
                if (!string.IsNullOrWhiteSpace(dr["PtoMesLocML"].ToString()))
                    this.PtoMesLocML.Value = Convert.ToDecimal(dr["PtoMesLocML"]);
                if (!string.IsNullOrWhiteSpace(dr["PtoMesLocME"].ToString()))
                    this.PtoMesLocME.Value = Convert.ToDecimal(dr["PtoMesLocME"]);
                if (!string.IsNullOrWhiteSpace(dr["PtoMesExtME"].ToString()))
                    this.PtoMesExtME.Value = Convert.ToDecimal(dr["PtoMesExtME"]);
                if (!string.IsNullOrWhiteSpace(dr["PtoMesExtML"].ToString()))
                    this.PtoMesExtML.Value = Convert.ToDecimal(dr["PtoMesExtML"]);
                if (!string.IsNullOrWhiteSpace(dr["PtoTotalLocML"].ToString()))
                    this.PtoTotalLocML.Value = Convert.ToDecimal(dr["PtoTotalLocML"]);
                if (!string.IsNullOrWhiteSpace(dr["PtoTotalLocME"].ToString()))
                    this.PtoTotalLocME.Value = Convert.ToDecimal(dr["PtoTotalLocME"]);
                if (!string.IsNullOrWhiteSpace(dr["PtoTotalExtME"].ToString()))
                    this.PtoTotalExtME.Value = Convert.ToDecimal(dr["PtoTotalExtME"]);
                if (!string.IsNullOrWhiteSpace(dr["PtoTotalExtML"].ToString()))
                    this.PtoTotalExtML.Value = Convert.ToDecimal(dr["PtoTotalExtML"]);
                if (!string.IsNullOrWhiteSpace(dr["CompActLocML"].ToString()))
                    this.CompActLocML.Value = Convert.ToDecimal(dr["CompActLocML"]);
                if (!string.IsNullOrWhiteSpace(dr["CompActLocME"].ToString()))
                    this.CompActLocME.Value = Convert.ToDecimal(dr["CompActLocME"]);
                if (!string.IsNullOrWhiteSpace(dr["CompActExtME"].ToString()))
                    this.CompActExtME.Value = Convert.ToDecimal(dr["CompActExtME"]);
                if (!string.IsNullOrWhiteSpace(dr["CompActExtML"].ToString()))
                    this.CompActExtML.Value = Convert.ToDecimal(dr["CompActExtML"]);
                if (!string.IsNullOrWhiteSpace(dr["RecibidoLocML"].ToString()))
                    this.RecibidoLocML.Value = Convert.ToDecimal(dr["RecibidoLocML"]);
                if (!string.IsNullOrWhiteSpace(dr["RecibidoLocME"].ToString()))
                    this.RecibidoLocME.Value = Convert.ToDecimal(dr["RecibidoLocME"]);
                if (!string.IsNullOrWhiteSpace(dr["RecibidoExtME"].ToString()))
                    this.RecibidoExtME.Value = Convert.ToDecimal(dr["RecibidoExtME"]);
                if (!string.IsNullOrWhiteSpace(dr["RecibidoExtML"].ToString()))
                    this.RecibidoExtML.Value = Convert.ToDecimal(dr["RecibidoExtML"]);
                if (!string.IsNullOrWhiteSpace(dr["CtoInicialLocML"].ToString()))
                    this.CtoInicialLocML.Value = Convert.ToDecimal(dr["CtoInicialLocML"]);
                if (!string.IsNullOrWhiteSpace(dr["ocProcesoLocML"].ToString()))
                    this.ocProcesoLocML.Value = Convert.ToDecimal(dr["ocProcesoLocML"]);
                if (!string.IsNullOrWhiteSpace(dr["ocProcesoExtME"].ToString()))
                    this.ocProcesoExtME.Value = Convert.ToDecimal(dr["ocProcesoExtME"]);
                if (!string.IsNullOrWhiteSpace(dr["CtoInicialExtME"].ToString()))
                    this.CtoInicialExtME.Value = Convert.ToDecimal(dr["CtoInicialExtME"]);
                if (!string.IsNullOrWhiteSpace(dr["PtoInicialLocML"].ToString()))
                    this.PtoInicialLocML.Value = Convert.ToDecimal(dr["PtoInicialLocML"]);
                if (!string.IsNullOrWhiteSpace(dr["PtoInicialExtME"].ToString()))
                    this.PtoInicialExtME.Value = Convert.ToDecimal(dr["PtoInicialExtME"]);
                if (!string.IsNullOrWhiteSpace(dr["CompInicialocML"].ToString()))
                    this.CompInicialocML.Value = Convert.ToDecimal(dr["CompInicialocML"]);
                if (!string.IsNullOrWhiteSpace(dr["CompinicialExtME"].ToString()))
                    this.CompinicialExtME.Value = Convert.ToDecimal(dr["CompinicialExtME"]);
                if (!string.IsNullOrWhiteSpace(dr["RecInicialLocML"].ToString()))
                    this.RecInicialLocML.Value = Convert.ToDecimal(dr["RecInicialLocML"]);
                if (!string.IsNullOrWhiteSpace(dr["RecInicialExtME"].ToString()))
                    this.RecInicialExtME.Value = Convert.ToDecimal(dr["RecInicialExtME"]);
                if (!string.IsNullOrWhiteSpace(dr["ocProcesoInicialLocML"].ToString()))
                    this.ocProcesoInicialLocML.Value = Convert.ToDecimal(dr["ocProcesoInicialLocML"]);
                if (!string.IsNullOrWhiteSpace(dr["ocProcesoInicialExtME"].ToString()))
                    this.ocProcesoInicialExtME.Value = Convert.ToDecimal(dr["ocProcesoInicialExtME"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioRevSobreejec"].ToString()))
                    this.UsuarioRevSobreejec.Value = Convert.ToString(dr["UsuarioRevSobreejec"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaRevSobreejec"].ToString()))
                    this.FechaRevSobreejec.Value = Convert.ToDateTime(dr["FechaRevSobreejec"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAprSobreejec"].ToString()))
                    this.UsuarioAprSobreejec.Value = Convert.ToString(dr["UsuarioAprSobreejec"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAprSobreejec"].ToString()))
                    this.FechaAprSobreejec.Value = Convert.ToDateTime(dr["FechaAprSobreejec"]);
                if (!string.IsNullOrWhiteSpace(dr["SolicitaPresInd"].ToString()))
                    this.SolicitaPresInd.Value = Convert.ToBoolean(dr["SolicitaPresInd"]);
                if (!string.IsNullOrWhiteSpace(dr["Observacion"].ToString()))
                    this.Observacion.Value = Convert.ToString(dr["Observacion"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);

                try
                {
                    //CAmpos Adicionales
                    this.Descriptivo.Value = Convert.ToString(dr["Descriptivo"]);
                    this.NombreProv.Value = Convert.ToString(dr["NombreProv"]);
                    this.SolicitadoOrigenLoc.Value = Convert.ToDecimal(dr["SolicitadoOrigenLoc"]);
                    this.SolicitadoOrigenExt.Value = Convert.ToDecimal(dr["SolicitadoOrigenExt"]);
                    this.DisponibleOrigenLoc.Value = Convert.ToDecimal(dr["DisponibleOrigenLoc"]);
                    this.DisponibleOrigenExt.Value = Convert.ToDecimal(dr["DisponibleOrigenExt"]);
                    this.EnProcesoOrigenLoc.Value = Convert.ToDecimal(dr["EnProcesoOrigenLoc"]);
                    this.EnProcesoOrigenExt.Value = Convert.ToDecimal(dr["EnProcesoOrigenExt"]);
                    this.PorAprobarOrigenLoc.Value = Convert.ToDecimal(dr["PorAprobarOrigenLoc"]);
                    this.PorAprobarOrigenExt.Value = Convert.ToDecimal(dr["PorAprobarOrigenExt"]);
                }
                catch (Exception)
                { ; }
            }
            catch (Exception)
            {                
                throw;
            }
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.CodigoBSID = new  UDT_CodigoBSID ();
            this.PrefijoID = new  UDT_PrefijoID ();
            this.DocumentoNro = new UDT_Consecutivo ();
            this.TipoDocumento = new UDTSQL_tinyint ();
            this.NumeroDoc = new UDT_Consecutivo ();
            this.ConsecutivoDetaID = new UDT_Consecutivo ();
            this.FechaDoc = new UDTSQL_smalldatetime ();
            this.MonedaOrigen = new UDTSQL_tinyint ();
            this.AreaAprobacion = new UDT_AreaFuncionalID ();
            this.Estado = new UDTSQL_tinyint ();
            this.TipoAprobacion = new UDTSQL_tinyint ();
            this.ProveedorID = new UDT_ProveedorID ();
            this.TasaOC = new UDT_TasaID ();
            this.CantidadDOC = new  UDT_Cantidad ();
            this.ValorOCLocML = new UDT_Valor();
            this.ValorOCLocME = new UDT_Valor();
            this.ValorOCExtME = new UDT_Valor();
            this.ValorOCExtML = new UDT_Valor();
            this.CantidadSOL = new UDT_Cantidad ();
            this.CtoOrigenLocML = new UDT_Valor();
            this.CtoOrigenLocME = new UDT_Valor();
            this.CtoOrigenExtME = new UDT_Valor();
            this.CtoOrigenExtML = new UDT_Valor();
            this.CantidadPTO = new UDT_Cantidad ();
            this.PtoMesLocML = new UDT_Valor();
            this.PtoMesLocME = new UDT_Valor();
            this.PtoMesExtME = new UDT_Valor();
            this.PtoMesExtML = new UDT_Valor();
            this.PtoTotalLocML = new UDT_Valor();
            this.PtoTotalLocME = new UDT_Valor();
            this.PtoTotalExtME = new UDT_Valor();
            this.PtoTotalExtML = new UDT_Valor();
            this.CompActLocML = new UDT_Valor();
            this.CompActLocME = new UDT_Valor();
            this.CompActExtME = new UDT_Valor();
            this.CompActExtML = new UDT_Valor();
            this.RecibidoLocML = new UDT_Valor();
            this.RecibidoLocME = new UDT_Valor();
            this.RecibidoExtME = new UDT_Valor();
            this.RecibidoExtML = new UDT_Valor();
            this.CtoInicialLocML = new UDT_Valor();
            this.ocProcesoLocML = new UDT_Valor();
            this.ocProcesoExtME = new UDT_Valor();
            this.CtoInicialExtME = new UDT_Valor();
            this.PtoInicialLocML = new UDT_Valor();
            this.PtoInicialExtME = new UDT_Valor();
            this.CompInicialocML = new UDT_Valor();
            this.CompinicialExtME = new UDT_Valor();
            this.RecInicialLocML = new UDT_Valor();
            this.RecInicialExtME = new UDT_Valor();
            this.ocProcesoInicialLocML = new UDT_Valor();
            this.ocProcesoInicialExtME = new UDT_Valor();
            this.UsuarioRevSobreejec = new UDT_UsuarioID();
            this.FechaRevSobreejec = new UDTSQL_smalldatetime();
            this.UsuarioAprSobreejec = new UDT_UsuarioID();
            this.FechaAprSobreejec = new UDTSQL_smalldatetime();
            this.SolicitaPresInd = new UDT_SiNo();
            this.Observacion = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();
            //Campos Adicionales
            this.PrefDoc = new UDT_DescripTBase();
            this.Descriptivo = new UDT_Descriptivo();
            this.NombreProv = new UDT_Descriptivo();
            this.SolicitadoOrigenLoc = new UDT_Valor();
            this.SolicitadoOrigenExt = new UDT_Valor ();
            this.DisponibleOrigenLoc = new UDT_Valor();
            this.DisponibleOrigenExt = new UDT_Valor();
            this.EnProcesoOrigenLoc = new UDT_Valor();
            this.EnProcesoOrigenExt = new UDT_Valor();
            this.PorAprobarOrigenLoc = new UDT_Valor();
            this.PorAprobarOrigenExt = new UDT_Valor();
            this.Solicita = new UDT_Valor();
            this.Disponible = new UDT_Valor();
            this.EnProceso = new UDT_Valor();
            this.PorAprobar = new UDT_Valor();
            this.Detalle = new List<DTO_plSobreEjecucion>();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoDocumento { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsecutivoDetaID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDTSQL_tinyint MonedaOrigen { get; set; }

        [DataMember]
        public UDT_AreaFuncionalID AreaAprobacion { get; set; }

        [DataMember]
        public UDTSQL_tinyint Estado { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoAprobacion { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        public UDT_TasaID TasaOC { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDOC { get; set; }

        [DataMember]
        public UDT_Valor ValorOCLocML { get; set; }

        [DataMember]
        public UDT_Valor ValorOCLocME { get; set; }

        [DataMember]
        public UDT_Valor ValorOCExtME { get; set; }

        [DataMember]
        public UDT_Valor ValorOCExtML { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadSOL { get; set; }

        [DataMember]
        public UDT_Valor CtoOrigenLocML { get; set; }

        [DataMember]
        public UDT_Valor CtoOrigenLocME { get; set; }

        [DataMember]
        public UDT_Valor CtoOrigenExtME { get; set; }

        [DataMember]
        public UDT_Valor CtoOrigenExtML { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadPTO { get; set; }

        [DataMember]
        public UDT_Valor PtoMesLocML { get; set; }

        [DataMember]
        public UDT_Valor PtoMesLocME { get; set; }

        [DataMember]
        public UDT_Valor PtoMesExtME { get; set; }

        [DataMember]
        public UDT_Valor PtoMesExtML { get; set; }

        [DataMember]
        public UDT_Valor PtoTotalLocML { get; set; }

        [DataMember]
        public UDT_Valor PtoTotalLocME { get; set; }

        [DataMember]
        public UDT_Valor PtoTotalExtME { get; set; }

        [DataMember]
        public UDT_Valor PtoTotalExtML { get; set; }

        [DataMember]
        public UDT_Valor CompActLocML { get; set; }

        [DataMember]
        public UDT_Valor CompActLocME { get; set; }

        [DataMember]
        public UDT_Valor CompActExtME { get; set; }

        [DataMember]
        public UDT_Valor CompActExtML { get; set; }

        [DataMember]
        public UDT_Valor RecibidoLocML { get; set; }

        [DataMember]
        public UDT_Valor RecibidoLocME { get; set; }

        [DataMember]
        public UDT_Valor RecibidoExtML { get; set; }

        [DataMember]
        public UDT_Valor RecibidoExtME { get; set; }

        [DataMember]
        public UDT_Valor CtoInicialLocML { get; set; }

        [DataMember]
        public UDT_Valor CtoInicialExtME { get; set; }

        [DataMember]
        public UDT_Valor ocProcesoLocML { get; set; }

        [DataMember]
        public UDT_Valor ocProcesoExtME { get; set; }

        [DataMember]
        public UDT_Valor PtoInicialLocML { get; set; }

        [DataMember]
        public UDT_Valor PtoInicialExtME { get; set; }

        [DataMember]
        public UDT_Valor CompInicialocML { get; set; }

        [DataMember]
        public UDT_Valor CompinicialExtME { get; set; }

        [DataMember]
        public UDT_Valor RecInicialLocML { get; set; }

        [DataMember]
        public UDT_Valor RecInicialExtME { get; set; }

        [DataMember]
        public UDT_Valor ocProcesoInicialLocML { get; set; }

        [DataMember]
        public UDT_Valor ocProcesoInicialExtME { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioRevSobreejec { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaRevSobreejec { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioAprSobreejec { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaAprSobreejec { get; set; }

        [DataMember]
        public UDT_SiNo SolicitaPresInd { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        //Campos Adicionales

        [DataMember]
        public UDT_DescripTBase PrefDoc { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_Descriptivo NombreProv { get; set; }

        [DataMember]
        public UDT_Valor SolicitadoOrigenLoc { get; set; }

        [DataMember]
        public UDT_Valor SolicitadoOrigenExt { get; set; }

        [DataMember]
        public UDT_Valor DisponibleOrigenLoc { get; set; }

        [DataMember]
        public UDT_Valor DisponibleOrigenExt { get; set; }

        [DataMember]
        public UDT_Valor EnProcesoOrigenLoc { get; set; }

        [DataMember]
        public UDT_Valor EnProcesoOrigenExt { get; set; }

        [DataMember]
        public UDT_Valor PorAprobarOrigenLoc { get; set; }

        [DataMember]
        public UDT_Valor PorAprobarOrigenExt { get; set; }

        [DataMember]
        public UDT_Valor Solicita { get; set; }

        [DataMember]
        public UDT_Valor Disponible { get; set; }

        [DataMember]
        public UDT_Valor EnProceso { get; set; }

        [DataMember]
        public UDT_Valor PorAprobar { get; set; }

        [DataMember]
        public List<DTO_plSobreEjecucion> Detalle { get; set; }

        #endregion
    }
    
}
