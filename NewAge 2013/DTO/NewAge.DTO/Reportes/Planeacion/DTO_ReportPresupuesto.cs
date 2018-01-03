using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte Auxiliar
    /// </summary>
    public class DTO_ReportPresupuesto
    {
        /// <summary>
        /// Constructor con DataReader
        /// <param name="islibros">Verifica si lo que se va a imprimir son solo los libros</param>
        /// </summary>
        public DTO_ReportPresupuesto(IDataReader dr, bool isAcumulado)
        {
            InitCols();
            try
            {
                #region Propiedades Genericas
                if (!string.IsNullOrEmpty(dr["Viegencia"].ToString()))
                    this.Viegencia.Value = Convert.ToInt16(dr["Viegencia"]);
                if (!string.IsNullOrEmpty(dr["FApertura"].ToString()))
                    this.FApertura.Value = Convert.ToDateTime(dr["FApertura"]);
                if (!string.IsNullOrEmpty(dr["CentroCostoID"].ToString()))
                    this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                if (!string.IsNullOrEmpty(dr["CostoDesc"].ToString()))
                    this.CostoDesc.Value = dr["CostoDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["LineaPresupuestoID"].ToString()))
                    this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                if (!string.IsNullOrEmpty(dr["LineaDesc"].ToString()))
                    this.LineaDesc.Value = dr["LineaDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["Proyecto"].ToString()))
                    this.Proyecto.Value = dr["Proyecto"].ToString();
                if (!string.IsNullOrEmpty(dr["Actividad"].ToString()))
                    this.Actividad.Value = dr["Actividad"].ToString();
                if (!string.IsNullOrEmpty(dr["ProyectoID"].ToString()))
                    this.ProyectoID.Value = dr["ProyectoID"].ToString();

                #endregion

                if (isAcumulado)
                {
                    #region Valores para el acumulado
                    if (!string.IsNullOrEmpty(dr["VlrLocal"].ToString()))
                        this.VlrLocal.Value = Convert.ToDecimal(dr["VlrLocal"]);
                    if (!string.IsNullOrEmpty(dr["VlrExtEquiv"].ToString()))
                        this.VlrExtEquiv.Value = Convert.ToDecimal(dr["VlrExtEquiv"]);
                    if (!string.IsNullOrEmpty(dr["VlrExt"].ToString()))
                        this.VlrExt.Value = Convert.ToDecimal(dr["VlrExt"]);
                    if (!string.IsNullOrEmpty(dr["VlrLocEquiv"].ToString()))
                        this.VlrLocEquiv.Value = Convert.ToDecimal(dr["VlrLocEquiv"]);
                    if (!string.IsNullOrEmpty(dr["VlrTotalLocal"].ToString()))
                        this.VlrTotalLocal.Value = Convert.ToDecimal(dr["VlrTotalLocal"]);
                    if (!string.IsNullOrEmpty(dr["VlrTotalExt"].ToString()))
                        this.VlrTotalExt.Value = Convert.ToDecimal(dr["VlrTotalExt"]);

                    #endregion
                }
                else
                {
                    #region  Valores para Moneda Local Sin Consolidar

                    if (!string.IsNullOrEmpty(dr["SaldoIniLoc"].ToString()))
                        this.SaldoIniLoc.Value = Convert.ToDecimal(dr["SaldoIniLoc"]);
                    if (!string.IsNullOrEmpty(dr["SaldoEneLoc"].ToString()))
                        this.SaldoEneLoc.Value = Convert.ToDecimal(dr["SaldoEneLoc"]);
                    if (!string.IsNullOrEmpty(dr["SaldoFebLoc"].ToString()))
                        this.SaldoFebLoc.Value = Convert.ToDecimal(dr["SaldoFebLoc"]);
                    if (!string.IsNullOrEmpty(dr["SaldoMarLoc"].ToString()))
                        this.SaldoMarLoc.Value = Convert.ToDecimal(dr["SaldoMarLoc"]);
                    if (!string.IsNullOrEmpty(dr["SaldoAbrLoc"].ToString()))
                        this.SaldoAbrLoc.Value = Convert.ToDecimal(dr["SaldoAbrLoc"]);
                    if (!string.IsNullOrEmpty(dr["SaldoMayLoc"].ToString()))
                        this.SaldoMayLoc.Value = Convert.ToDecimal(dr["SaldoMayLoc"]);
                    if (!string.IsNullOrEmpty(dr["SaldoJunLoc"].ToString()))
                        this.SaldoJunLoc.Value = Convert.ToDecimal(dr["SaldoJunLoc"]);
                    if (!string.IsNullOrEmpty(dr["SaldoJulLoc"].ToString()))
                        this.SaldoJulLoc.Value = Convert.ToDecimal(dr["SaldoJulLoc"]);
                    if (!string.IsNullOrEmpty(dr["SaldoAgoLoc"].ToString()))
                        this.SaldoAgoLoc.Value = Convert.ToDecimal(dr["SaldoAgoLoc"]);
                    if (!string.IsNullOrEmpty(dr["SaldoSepLoc"].ToString()))
                        this.SaldoSepLoc.Value = Convert.ToDecimal(dr["SaldoSepLoc"]);
                    if (!string.IsNullOrEmpty(dr["SaldoOctLoc"].ToString()))
                        this.SaldoOctLoc.Value = Convert.ToDecimal(dr["SaldoOctLoc"]);
                    if (!string.IsNullOrEmpty(dr["SaldoNovLoc"].ToString()))
                        this.SaldoNovLoc.Value = Convert.ToDecimal(dr["SaldoNovLoc"]);
                    if (!string.IsNullOrEmpty(dr["SaldoDicLoc"].ToString()))
                        this.SaldoDicLoc.Value = Convert.ToDecimal(dr["SaldoDicLoc"]);
                    if (!string.IsNullOrEmpty(dr["TotalSaldoLoc"].ToString()))
                        this.TotalSaldoLoc.Value = Convert.ToDecimal(dr["TotalSaldoLoc"]);


                    #endregion
                    #region Valores Para Moneda Extranjera Sin Consolidar

                    if (!string.IsNullOrEmpty(dr["SaldoIniExt"].ToString()))
                        this.SaldoIniExt.Value = Convert.ToDecimal(dr["SaldoIniExt"]);
                    if (!string.IsNullOrEmpty(dr["SaldoEneExt"].ToString()))
                        this.SaldoEneExt.Value = Convert.ToDecimal(dr["SaldoEneExt"]);
                    if (!string.IsNullOrEmpty(dr["SaldoFebExt"].ToString()))
                        this.SaldoFebExt.Value = Convert.ToDecimal(dr["SaldoFebExt"]);
                    if (!string.IsNullOrEmpty(dr["SaldoMarExt"].ToString()))
                        this.SaldoMarExt.Value = Convert.ToDecimal(dr["SaldoMarExt"]);
                    if (!string.IsNullOrEmpty(dr["SaldoAbrExt"].ToString()))
                        this.SaldoAbrExt.Value = Convert.ToDecimal(dr["SaldoAbrExt"]);
                    if (!string.IsNullOrEmpty(dr["SaldoMayExt"].ToString()))
                        this.SaldoMayExt.Value = Convert.ToDecimal(dr["SaldoMayExt"]);
                    if (!string.IsNullOrEmpty(dr["SaldoJunExt"].ToString()))
                        this.SaldoJunExt.Value = Convert.ToDecimal(dr["SaldoJunExt"]);
                    if (!string.IsNullOrEmpty(dr["SaldoJulExt"].ToString()))
                        this.SaldoJulExt.Value = Convert.ToDecimal(dr["SaldoJulExt"]);
                    if (!string.IsNullOrEmpty(dr["SaldoAgoExt"].ToString()))
                        this.SaldoAgoExt.Value = Convert.ToDecimal(dr["SaldoAgoExt"]);
                    if (!string.IsNullOrEmpty(dr["SaldoSepExt"].ToString()))
                        this.SaldoSepExt.Value = Convert.ToDecimal(dr["SaldoSepExt"]);
                    if (!string.IsNullOrEmpty(dr["SaldoOctExt"].ToString()))
                        this.SaldoOctExt.Value = Convert.ToDecimal(dr["SaldoOctExt"]);
                    if (!string.IsNullOrEmpty(dr["SaldoNovExt"].ToString()))
                        this.SaldoNovExt.Value = Convert.ToDecimal(dr["SaldoNovExt"]);
                    if (!string.IsNullOrEmpty(dr["SaldoDicExt"].ToString()))
                        this.SaldoDicExt.Value = Convert.ToDecimal(dr["SaldoDicExt"]);
                    if (!string.IsNullOrEmpty(dr["TotalSaldoExt"].ToString()))
                        this.TotalSaldoExt.Value = Convert.ToDecimal(dr["TotalSaldoExt"]);

                    #endregion
                    #region  Valores para Moneda Local Consolidados

                    if (!string.IsNullOrEmpty(dr["SaldoIniLocConSoli"].ToString()))
                        this.SaldoIniLocConSoli.Value = Convert.ToDecimal(dr["SaldoIniLocConSoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoEneLocConsoli"].ToString()))
                        this.SaldoEneLocConsoli.Value = Convert.ToDecimal(dr["SaldoEneLocConsoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoFebLocConsoli"].ToString()))
                        this.SaldoFebLocConsoli.Value = Convert.ToDecimal(dr["SaldoFebLocConsoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoMarLocConsoli"].ToString()))
                        this.SaldoMarLocConsoli.Value = Convert.ToDecimal(dr["SaldoMarLocConsoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoAbrLocConsoli"].ToString()))
                        this.SaldoAbrLocConsoli.Value = Convert.ToDecimal(dr["SaldoAbrLocConsoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoMayLocConsoli"].ToString()))
                        this.SaldoMayLocConsoli.Value = Convert.ToDecimal(dr["SaldoMayLocConsoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoJunLocConsoli"].ToString()))
                        this.SaldoJunLocConsoli.Value = Convert.ToDecimal(dr["SaldoJunLocConsoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoJulLocConsoli"].ToString()))
                        this.SaldoJulLocConsoli.Value = Convert.ToDecimal(dr["SaldoJulLocConsoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoAgoLocConsoli"].ToString()))
                        this.SaldoAgoLocConsoli.Value = Convert.ToDecimal(dr["SaldoAgoLocConsoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoSepLocConsoli"].ToString()))
                        this.SaldoSepLocConsoli.Value = Convert.ToDecimal(dr["SaldoSepLocConsoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoOctLocConsoli"].ToString()))
                        this.SaldoOctLocConsoli.Value = Convert.ToDecimal(dr["SaldoOctLocConsoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoNovLocConsoli"].ToString()))
                        this.SaldoNovLocConsoli.Value = Convert.ToDecimal(dr["SaldoNovLocConsoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoDicLocConsoli"].ToString()))
                        this.SaldoDicLocConsoli.Value = Convert.ToDecimal(dr["SaldoDicLocConsoli"]);
                    if (!string.IsNullOrEmpty(dr["TotalSaldoConsoliLoc"].ToString()))
                        this.TotalSaldoConsoliLoc.Value = Convert.ToDecimal(dr["TotalSaldoConsoliLoc"]);

                    #endregion
                    #region Valores Para Moneda Extranjera Consolidados

                    if (!string.IsNullOrEmpty(dr["SaldoIniExtConSoli"].ToString()))
                        this.SaldoIniExtConSoli.Value = Convert.ToDecimal(dr["SaldoIniExtConSoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoEneExtConSoli"].ToString()))
                        this.SaldoEneExtConSoli.Value = Convert.ToDecimal(dr["SaldoEneExtConSoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoFebExtConSoli"].ToString()))
                        this.SaldoFebExtConSoli.Value = Convert.ToDecimal(dr["SaldoFebExtConSoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoMarExtConSoli"].ToString()))
                        this.SaldoMarExtConSoli.Value = Convert.ToDecimal(dr["SaldoMarExtConSoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoAbrExtConSoli"].ToString()))
                        this.SaldoAbrExtConSoli.Value = Convert.ToDecimal(dr["SaldoAbrExtConSoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoMayExtConSoli"].ToString()))
                        this.SaldoMayExtConSoli.Value = Convert.ToDecimal(dr["SaldoMayExtConSoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoJunExtConSoli"].ToString()))
                        this.SaldoJunExtConSoli.Value = Convert.ToDecimal(dr["SaldoJunExtConSoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoJulExtConSoli"].ToString()))
                        this.SaldoJulExtConSoli.Value = Convert.ToDecimal(dr["SaldoJulExtConSoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoAgoExtConSoli"].ToString()))
                        this.SaldoAgoExtConSoli.Value = Convert.ToDecimal(dr["SaldoAgoExtConSoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoSepExtConSoli"].ToString()))
                        this.SaldoSepExtConSoli.Value = Convert.ToDecimal(dr["SaldoSepExtConSoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoOctExtConSoli"].ToString()))
                        this.SaldoOctExtConSoli.Value = Convert.ToDecimal(dr["SaldoOctExtConSoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoNovExtConSoli"].ToString()))
                        this.SaldoNovExtConSoli.Value = Convert.ToDecimal(dr["SaldoNovExtConSoli"]);
                    if (!string.IsNullOrEmpty(dr["SaldoDicExtConSoli"].ToString()))
                        this.SaldoDicExtConSoli.Value = Convert.ToDecimal(dr["SaldoDicExtConSoli"]);
                    if (!string.IsNullOrEmpty(dr["TotalSaldoConsoliExt"].ToString()))
                        this.TotalSaldoConsoliExt.Value = Convert.ToDecimal(dr["TotalSaldoConsoliExt"]);

                    #endregion
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportPresupuesto()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            #region Propiedades Genericas

            this.ProyectoID = new UDT_ProyectoID();
            this.ProyectoDesc = new UDT_DescripTBase();
            this.Viegencia = new UDTSQL_int();
            this.FApertura = new UDTSQL_smalldatetime();
            this.ActividadID = new UDT_ActividadID();
            this.ActivDesc = new UDT_DescripTBase();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.CostoDesc = new UDT_DescripTBase();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.LineaDesc = new UDT_DescripTBase();
            this.Proyecto = new UDTSQL_char(100);
            this.Actividad = new UDTSQL_char(100);

            #endregion
            #region  Valores para Moneda Local Sin Consolidar

            this.SaldoIniLoc = new UDT_Valor();
            this.SaldoEneLoc = new UDT_Valor();
            this.SaldoFebLoc = new UDT_Valor();
            this.SaldoMarLoc = new UDT_Valor();
            this.SaldoAbrLoc = new UDT_Valor();
            this.SaldoMayLoc = new UDT_Valor();
            this.SaldoJunLoc = new UDT_Valor();
            this.SaldoJulLoc = new UDT_Valor();
            this.SaldoAgoLoc = new UDT_Valor();
            this.SaldoSepLoc = new UDT_Valor();
            this.SaldoOctLoc = new UDT_Valor();
            this.SaldoNovLoc = new UDT_Valor();
            this.SaldoDicLoc = new UDT_Valor();
            this.TotalSaldoLoc = new UDT_Valor();

            #endregion
            #region Valores Para Moneda Extranjera Sin Consolidar

            this.SaldoIniExt = new UDT_Valor();
            this.SaldoEneExt = new UDT_Valor();
            this.SaldoFebExt = new UDT_Valor();
            this.SaldoMarExt = new UDT_Valor();
            this.SaldoAbrExt = new UDT_Valor();
            this.SaldoMayExt = new UDT_Valor();
            this.SaldoJunExt = new UDT_Valor();
            this.SaldoJulExt = new UDT_Valor();
            this.SaldoAgoExt = new UDT_Valor();
            this.SaldoSepExt = new UDT_Valor();
            this.SaldoOctExt = new UDT_Valor();
            this.SaldoNovExt = new UDT_Valor();
            this.SaldoDicExt = new UDT_Valor();
            this.TotalSaldoExt = new UDT_Valor();

            #endregion
            #region  Valores para Moneda Local Consolidados

            this.SaldoIniLocConSoli = new UDT_Valor();
            this.SaldoEneLocConsoli = new UDT_Valor();
            this.SaldoFebLocConsoli = new UDT_Valor();
            this.SaldoMarLocConsoli = new UDT_Valor();
            this.SaldoAbrLocConsoli = new UDT_Valor();
            this.SaldoMayLocConsoli = new UDT_Valor();
            this.SaldoJunLocConsoli = new UDT_Valor();
            this.SaldoJulLocConsoli = new UDT_Valor();
            this.SaldoAgoLocConsoli = new UDT_Valor();
            this.SaldoSepLocConsoli = new UDT_Valor();
            this.SaldoOctLocConsoli = new UDT_Valor();
            this.SaldoNovLocConsoli = new UDT_Valor();
            this.SaldoDicLocConsoli = new UDT_Valor();
            this.TotalSaldoConsoliLoc = new UDT_Valor();

            #endregion
            #region Valores Para Moneda Extranjera Consolidados

            this.SaldoIniExtConSoli = new UDT_Valor();
            this.SaldoEneExtConSoli = new UDT_Valor();
            this.SaldoFebExtConSoli = new UDT_Valor();
            this.SaldoMarExtConSoli = new UDT_Valor();
            this.SaldoAbrExtConSoli = new UDT_Valor();
            this.SaldoMayExtConSoli = new UDT_Valor();
            this.SaldoJunExtConSoli = new UDT_Valor();
            this.SaldoJulExtConSoli = new UDT_Valor();
            this.SaldoAgoExtConSoli = new UDT_Valor();
            this.SaldoSepExtConSoli = new UDT_Valor();
            this.SaldoOctExtConSoli = new UDT_Valor();
            this.SaldoNovExtConSoli = new UDT_Valor();
            this.SaldoDicExtConSoli = new UDT_Valor();
            this.TotalSaldoConsoliExt = new UDT_Valor();

            #endregion
            #region Valores para el acumulado

            this.VlrLocal = new UDT_Valor();
            this.VlrExtEquiv = new UDT_Valor();
            this.VlrExt = new UDT_Valor();
            this.VlrLocEquiv = new UDT_Valor();
            this.VlrTotalLocal = new UDT_Valor();
            this.VlrTotalExt = new UDT_Valor();

            #endregion
        }

        #region Propiedades

        #region Propiedades Genericas

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_DescripTBase ProyectoDesc { get; set; }

        [DataMember]
        public UDTSQL_int Viegencia { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FApertura { get; set; }

        [DataMember]
        public UDT_ActividadID ActividadID { get; set; }

        [DataMember]
        public UDT_DescripTBase ActivDesc { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_DescripTBase CostoDesc { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_DescripTBase LineaDesc { get; set; }

        [DataMember]
        public UDTSQL_char Proyecto { get; set; }

        [DataMember]
        public UDTSQL_char Actividad { get; set; }


        #endregion
        #region  Valores para Moneda Local Sin Consolidar

        [DataMember]
        public UDT_Valor SaldoIniLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoEneLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoFebLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoMarLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoAbrLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoMayLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoJunLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoJulLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoAgoLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoSepLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoOctLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoNovLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoDicLoc { get; set; }

        [DataMember]
        public UDT_Valor TotalSaldoLoc { get; set; }

        #endregion
        #region Valores Para Moneda Extranjera Sin Consolidar

        [DataMember]
        public UDT_Valor SaldoIniExt { get; set; }

        [DataMember]
        public UDT_Valor SaldoEneExt { get; set; }

        [DataMember]
        public UDT_Valor SaldoFebExt { get; set; }

        [DataMember]
        public UDT_Valor SaldoMarExt { get; set; }

        [DataMember]
        public UDT_Valor SaldoAbrExt { get; set; }

        [DataMember]
        public UDT_Valor SaldoMayExt { get; set; }

        [DataMember]
        public UDT_Valor SaldoJunExt { get; set; }

        [DataMember]
        public UDT_Valor SaldoJulExt { get; set; }

        [DataMember]
        public UDT_Valor SaldoAgoExt { get; set; }

        [DataMember]
        public UDT_Valor SaldoSepExt { get; set; }

        [DataMember]
        public UDT_Valor SaldoOctExt { get; set; }

        [DataMember]
        public UDT_Valor SaldoNovExt { get; set; }

        [DataMember]
        public UDT_Valor SaldoDicExt { get; set; }

        [DataMember]
        public UDT_Valor TotalSaldoExt { get; set; }

        #endregion
        #region  Valores para Moneda Local Consolidados

        [DataMember]
        public UDT_Valor SaldoIniLocConSoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoEneLocConsoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoFebLocConsoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoMarLocConsoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoAbrLocConsoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoMayLocConsoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoJunLocConsoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoJulLocConsoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoAgoLocConsoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoSepLocConsoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoOctLocConsoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoNovLocConsoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoDicLocConsoli { get; set; }

        [DataMember]
        public UDT_Valor TotalSaldoConsoliLoc { get; set; }

        #endregion
        #region Valores Para Moneda Extranjera Consolidados

        [DataMember]
        public UDT_Valor SaldoIniExtConSoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoEneExtConSoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoFebExtConSoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoMarExtConSoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoAbrExtConSoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoMayExtConSoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoJunExtConSoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoJulExtConSoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoAgoExtConSoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoSepExtConSoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoOctExtConSoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoNovExtConSoli { get; set; }

        [DataMember]
        public UDT_Valor SaldoDicExtConSoli { get; set; }

        [DataMember]
        public UDT_Valor TotalSaldoConsoliExt { get; set; }

        #endregion
        #region Valores para el acumulado

        [DataMember]
        public UDT_Valor VlrLocal { get; set; }

        [DataMember]
        public UDT_Valor VlrExtEquiv { get; set; }

        [DataMember]
        public UDT_Valor VlrExt { get; set; }

        [DataMember]
        public UDT_Valor VlrLocEquiv { get; set; }

        [DataMember]
        public UDT_Valor VlrTotalLocal { get; set; }

        [DataMember]
        public UDT_Valor VlrTotalExt { get; set; }

        #endregion

        #endregion

    }
}
