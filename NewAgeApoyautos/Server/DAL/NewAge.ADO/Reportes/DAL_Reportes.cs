using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;
using SentenceTransformer;
using System.Text.RegularExpressions;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    public class DAL_Reportes : DAL_Base
    {
        public Dictionary<int, ReportMetadata> ReportsConfig = new Dictionary<int, ReportMetadata>();

        public DAL_Reportes(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn)
            : base(c, tx, empresa, userId, loggerConn)
        {
            ConfigureReports();
        }

        private void ConfigureReports()
        {
            #region Bitacora Report
            ReportsConfig.Add(1, new ReportMetadata(typeof(DTO_repBitacora), "SELECT * FROM aplBitacora WHERE {0} ORDER BY FECHA DESC", DefaultFill));
            #endregion

            #region Balance Reports
            #region Balance de Prueba Report

            #region Cuenta Funcional
            ReportsConfig.Add(AppReports.coBalanceDePrueba, new ReportMetadata(typeof(DTO_ReportBalanceDePrueba),
                "declare @MesIni int declare @MesFin int declare @CuentaLength int declare @SaldoInicialInd int declare @EmpresaID char(10) declare @cuentaIni  char(15) declare @cuentaFin char(15) " +
            " set {1} set {2} set {3} set {6} set {7} set {8} set {10} " +
            "select * from( SELECT b.CuentaID CuentaID,c.Descriptivo CuentaDesc " +
                ",case when (c.MascaraCta<=@CuentaLength) then (case when LEN(b.CuentaID)=c.MascaraCta then 1 else 0 end) else (case when LEN(b.CuentaID)=@CuentaLength then 1 else 0 end) end MaxLengthInd " +
                ",b.CentroCostoID,ccosto.Descriptivo CentroCostoDesc,b.ProyectoID,proyecto.Descriptivo ProyectoDesc,b.LineaPresupuestoID,lp.Descriptivo LineaPresupuestoDesc,c.Naturaleza " +
                ",case when (c.Naturaleza=1) then 1 else (-1) end Signo " +
                ",(b.DbOrigenLocML+b.DbOrigenExtML) DebitoML " +//",case when (c.Naturaleza=1) then (b.DbOrigenLocML+b.DbOrigenExtML) else (b.DbOrigenLocML+b.DbOrigenExtML)*(-1)end DebitoML " + //
                ",(b.CrOrigenLocML+b.CrOrigenExtML)*(-1) CreditoML " +//",case when (c.Naturaleza=1) then (b.CrOrigenLocML+b.CrOrigenExtML)*(-1) else (b.CrOrigenLocML+b.CrOrigenExtML)end CreditoML " +  //
                ",(b.DbOrigenLocME+b.DbOrigenExtME) DebitoME " +// ",case when (c.Naturaleza=1) then (b.DbOrigenLocME+b.DbOrigenExtME) else (b.DbOrigenLocME+b.DbOrigenExtME)*(-1)end DebitoME " +//
                ",(b.CrOrigenLocME+b.CrOrigenExtME)*(-1)CreditoME " +//",case when (c.Naturaleza=1) then (b.CrOrigenLocME+b.CrOrigenExtME)*(-1) else (b.CrOrigenLocME+b.CrOrigenExtME)end CreditoME " + //  
                ",case when (@MesIni < 12 and MONTH(b.PeriodoID)=@MesIni)or(@MesIni>=12 and MONTH(b.PeriodoID)=12 and DAY(b.PeriodoID)=@MesIni-11) " +
                    "then case when (c.Naturaleza=1) " +
                        "then (b.DbSaldoIniLocML+b.DbSaldoIniExtML+b.CrSaldoIniLocML+b.CrSaldoIniExtML)*@SaldoInicialInd " +
                        "else (-1)*(b.DbSaldoIniLocML+b.DbSaldoIniExtML+b.CrSaldoIniLocML+b.CrSaldoIniExtML)*@SaldoInicialInd end " +
                    "else 0 end InicialML " +
                ",case when (@MesFin < 12 and MONTH(b.PeriodoID)=@MesFin)or(@MesFin>=12 and MONTH(b.PeriodoID)=12 and DAY(b.PeriodoID)=@MesFin-11) " +
                "then (case when (c.Naturaleza=1) then  (b.DbSaldoIniLocML+b.DbSaldoIniExtML+b.CrSaldoIniLocML+b.CrSaldoIniExtML+ b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)*@SaldoInicialInd " +
                "   else (-1)*(b.DbSaldoIniLocML+b.DbSaldoIniExtML+b.CrSaldoIniLocML+b.CrSaldoIniExtML+b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)*@SaldoInicialInd end) " +
                "    else 0 end FinalML " +
                ",case when (@MesIni < 12 and MONTH(b.PeriodoID)=@MesIni)or(@MesIni>=12 and MONTH(b.PeriodoID)=12 and DAY(b.PeriodoID)=@MesIni-11) " +
                    "then case when (c.Naturaleza=1) " +
                        "then (b.DbSaldoIniLocME+b.DbSaldoIniExtME+b.CrSaldoIniLocME+b.CrSaldoIniExtME)*@SaldoInicialInd " +
                        "else (-1)*(b.DbSaldoIniLocME+b.DbSaldoIniExtME+b.CrSaldoIniLocME+b.CrSaldoIniExtME)*@SaldoInicialInd end " +
                    "else 0 end InicialME " +
                ",case when (@MesFin < 12 and MONTH(b.PeriodoID)=@MesFin)or(@MesFin>=12 and MONTH(b.PeriodoID)=12 and DAY(b.PeriodoID)=@MesFin-11) " +
                "then (case when (c.Naturaleza=1) then  (b.DbSaldoIniLocME+b.DbSaldoIniExtME+b.CrSaldoIniLocME+b.CrSaldoIniExtME+ b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)*@SaldoInicialInd " +
                "   else (-1)*(b.DbSaldoIniLocME+b.DbSaldoIniExtME+b.CrSaldoIniLocME+b.CrSaldoIniExtME+b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)*@SaldoInicialInd end) " +
                "    else 0 end FinalME " +
                ",((b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)) MovimientoML " +
                ",((b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)) MovimientoME " +
            "FROM coBalance b inner join coPlanCuenta c with(nolock) on(b.CuentaID=c.CuentaID and b.eg_coPlanCuenta=c.EmpresaGrupoID) " +
            "inner join coProyecto proyecto with(nolock) on(b.ProyectoID=proyecto.ProyectoID and b.eg_coProyecto=proyecto.EmpresaGrupoID) " +
            "inner join coCentroCosto ccosto with(nolock) on(b.CentroCostoID=ccosto.CentroCostoID and b.eg_coCentroCosto=ccosto.EmpresaGrupoID ) " +
            "inner join plLineaPresupuesto lp with(nolock) on(b.LineaPresupuestoID=lp.LineaPresupuestoID and b.eg_plLineaPresupuesto=lp.EmpresaGrupoID) " +
            "WHERE b.EmpresaID = @EmpresaID and (LEN(b.CuentaID)<=(case when(c.MascaraCta<=@CuentaLength)then c.MascaraCta else @CuentaLength end)) " +
               "and Month(PeriodoID) between (case when @MesIni>12 then 12 else @MesIni end) and (case when @MesFin>12 then 12 else @MesFin end)" +
               "and day(PeriodoID) between (case when @MesIni>12 then @MesIni-11 else 1 end) and (case when @MesFin>12 then @MesFin-11 else 1 end)" +
                "and {4} and ({5}) and ({9}) "+
                " and  b.CuentaID BETWEEN @cuentaIni and @cuentaFin  " +
                ")temp where InicialML!=0 or DebitoML!=0 or CreditoML!=0 or FinalML!=0 or InicialME!=0 or DebitoME!=0 or CreditoME!=0 or FinalME!=0 order by CuentaID", DefaultFill));
            #endregion

            #endregion

            #region Balance de Prueba por meses Report

            #region Cuenta Funcional
            ReportsConfig.Add(AppReports.coBalanceDePruebaPorMeses, new ReportMetadata(typeof(DTO_ReportBalanceDePruebaPorMeses),
            "create function dbo.calcValorML(@p int,@m int,@n int,@v_1 numeric(20,4),@v_2 numeric(20,4),@v_3 numeric(20,4),@v_4 numeric(20,4)) returns numeric(20,4) " +
            "begin return case when (@p=@m)then case when (@n=1) then ((@v_1+@v_2+@v_3+@v_4)/1000) else ((@v_1+@v_2+@v_3+@v_4)/1000)*(-1)end else 0 end end \r\ngo\r\n " +
            "declare @CuentaLength int set {2}  " +
            "select * ,(MovML_01+MovML_02+MovML_03+MovML_04+MovML_05+MovML_06+MovML_07+MovML_08+MovML_09+MovML_10+MovML_11+MovML_12) Y_ML " +
                ",(MovME_01+MovME_02+MovME_03+MovME_04+MovME_05+MovME_06+MovME_07+MovME_08+MovME_09+MovME_10+MovME_11+MovME_12) Y_ME " +
            "from(SELECT b.CuentaID CuentaID,c.Descriptivo CuentaDesc,c.Naturaleza,YEAR(b.PeriodoID) Año " +
                ",case when (c.MascaraCta<=@CuentaLength) then (case when LEN(b.CuentaID)=c.MascaraCta then 1 else 0 end) else (case when LEN(b.CuentaID)=@CuentaLength then 1 else 0 end) end MaxLengthInd " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),1,c.Naturaleza,b.DbSaldoIniLocML,b.DbSaldoIniExtML,b.CrSaldoIniLocML,b.CrSaldoIniExtML)) as InicialML " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),1,c.Naturaleza,b.DbSaldoIniLocME,b.DbSaldoIniExtME,b.CrSaldoIniLocME,b.CrSaldoIniExtME)) as InicialME " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),1,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_01 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),1,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_01 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),2,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_02 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),2,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_02 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),3,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_03 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),3,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_03 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),4,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_04 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),4,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_04 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),5,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_05 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),5,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_05 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),6,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_06 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),6,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_06 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),7,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_07 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),7,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_07 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),8,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_08 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),8,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_08 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),9,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_09 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),9,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_09 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),10,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_10 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),10,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_10 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),11,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_11 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),11,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_11 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),12,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_12 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),12,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_12 " +
            "FROM coBalance b inner join coPlanCuenta c with(nolock) on(b.CuentaID=c.CuentaID and b.eg_coPlanCuenta=c.EmpresaGrupoID) " +
            "WHERE (LEN(b.CuentaID)<=(case when(c.MascaraCta<=@CuentaLength)then c.MascaraCta else @CuentaLength end)) and {4} and ({5}) and ({7}) " +
            "group by YEAR(b.PeriodoID),b.CuentaID,c.Descriptivo,c.MovInd,c.MascaraCta,c.Naturaleza)temp1 order by Año,CuentaID " +
            "if OBJECT_ID(N'dbo.calcValorML', N'FN') IS NOT NULL  DROP FUNCTION dbo.calcValorML ", DefaultFill));


            //ReportsConfig.Add(AppReports.BalanceDePruebaPorMeses, new ReportMetadata(typeof(DTO_ReportBalanceDePruebaPorMeses), "declare @CuentaLength int set {2} " +
            //    "select *,(MovML_01+MovML_02+MovML_03+MovML_04+MovML_05+MovML_06+MovML_07+MovML_08+MovML_09+MovML_10+MovML_11+MovML_12) Y_ML " + 
            //    ",(MovME_01+MovME_02+MovME_03+MovME_04+MovME_05+MovME_06+MovME_07+MovME_08+MovME_09+MovME_10+MovME_11+MovME_12) Y_ME " + 
            //"from(SELECT b.CuentaID CuentaID,c.Descriptivo CuentaDesc,YEAR(b.PeriodoID) Año " +
            //    ",case when (c.MascaraCta<=@CuentaLength) then (case when LEN(b.CuentaID)=c.MascaraCta then 1 else 0 end) else (case when LEN(b.CuentaID)=@CuentaLength then 1 else 0 end) end MaxLengthInd " +
            //    ",sum(case when (MONTH(b.PeriodoID)=1)then ((b.DbSaldoIniLocML+b.DbSaldoIniExtML+b.CrSaldoIniLocML+b.CrSaldoIniExtML)/1000)else 0 end) InicialML " + 
            //    ",sum(case when (MONTH(b.PeriodoID)=1)then ((b.DbSaldoIniLocME+b.DbSaldoIniExtME+b.CrSaldoIniLocME+b.CrSaldoIniExtME)/1000)else 0 end) InicialME " + 
            //    ",sum(case when (MONTH(b.PeriodoID)=1)then ((b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000)else 0 end) MovML_01 " + 
            //    ",sum(case when (MONTH(b.PeriodoID)=1)then ((b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000)else 0 end) MovME_01 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=2)then ((b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000) else 0 end) MovML_02 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=2)then ((b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000) else 0 end) MovME_02 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=3)then ((b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000) else 0 end) MovML_03 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=3)then ((b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000) else 0 end) MovME_03 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=4)then ((b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000) else 0 end) MovML_04 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=4)then ((b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000) else 0 end) MovME_04 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=5)then ((b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000) else 0 end) MovML_05 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=5)then ((b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000) else 0 end) MovME_05 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=6)then ((b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000) else 0 end) MovML_06 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=6)then ((b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000) else 0 end) MovME_06 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=7)then ((b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000) else 0 end) MovML_07 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=7)then ((b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000) else 0 end) MovME_07 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=8)then ((b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000) else 0 end) MovML_08 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=8)then ((b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000) else 0 end) MovME_08 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=9)then ((b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000) else 0 end) MovML_09 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=9)then ((b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000) else 0 end) MovME_09 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=10)then ((b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000) else 0 end) MovML_10 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=10)then ((b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000) else 0 end) MovME_10 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=11)then ((b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000) else 0 end) MovML_11 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=11)then ((b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000) else 0 end) MovME_11 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=12)then ((b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000) else 0 end) MovML_12 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=12)then ((b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000) else 0 end) MovME_12 " +
            //"FROM coBalance b inner join coPlanCuenta c with(nolock) on(b.CuentaID=c.CuentaID and b.eg_coPlanCuenta=c.EmpresaGrupoID) " +
            //"WHERE (LEN(b.CuentaID)<=(case when(c.MascaraCta<=@CuentaLength)then c.MascaraCta else @CuentaLength end)) and {4} and ({5}) and ({6}) " +
            //"GROUP BY YEAR(b.PeriodoID),b.CuentaID,c.Descriptivo,c.MovInd,c.MascaraCta)temp1 order by Año,CuentaID", DefaultFill));
            #endregion

            #endregion

            #region Balance de Prueba por trimestres Report

            #region Cuenta Funcional
            ReportsConfig.Add(AppReports.coBalanceDePruebaPorQ, new ReportMetadata(typeof(DTO_ReportBalanceDePruebaPorQ),
            "create function dbo.calcValorML(@p int,@m int,@n int,@v_1 numeric(20,4),@v_2 numeric(20,4),@v_3 numeric(20,4),@v_4 numeric(20,4)) returns numeric(20,4) " +
            "begin return case when (@p=@m)then case when (@n=1) then ((@v_1+@v_2+@v_3+@v_4)/1000) else ((@v_1+@v_2+@v_3+@v_4)/1000)*(-1)end else 0 end end \r\ngo\r\n " +
            "declare @CuentaLength int set {2} " +
            "select *,(Q1_ML+Q2_ML+Q3_ML+Q4_ML) Y_ML,(Q1_ME+Q2_ME+Q3_ME+Q4_ME) Y_ME " +
            "from(select * ,(MovML_01+MovML_02+MovML_03) Q1_ML,(MovME_01+MovME_02+MovME_03) Q1_ME " +
                ",(MovML_04+MovML_05+MovML_06) Q2_ML,(MovME_04+MovME_05+MovME_06) Q2_ME " +
                ",(MovML_07+MovML_08+MovML_09) Q3_ML,(MovME_07+MovME_08+MovME_09) Q3_ME " +
                ",(MovML_10+MovML_11+MovML_12) Q4_ML,(MovME_10+MovME_11+MovME_12) Q4_ME " +
            "from(SELECT b.CuentaID CuentaID,c.Descriptivo CuentaDesc,YEAR(b.PeriodoID) Año " +
                ",case when (c.MascaraCta<=@CuentaLength) then (case when LEN(b.CuentaID)=c.MascaraCta then 1 else 0 end) else (case when LEN(b.CuentaID)=@CuentaLength then 1 else 0 end) end MaxLengthInd " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),1,c.Naturaleza,b.DbSaldoIniLocML,b.DbSaldoIniExtML,b.CrSaldoIniLocML,b.CrSaldoIniExtML)) as InicialML " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),1,c.Naturaleza,b.DbSaldoIniLocME,b.DbSaldoIniExtME,b.CrSaldoIniLocME,b.CrSaldoIniExtME)) as InicialME " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),1,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_01 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),1,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_01 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),2,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_02 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),2,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_02 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),3,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_03 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),3,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_03 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),4,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_04 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),4,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_04 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),5,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_05 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),5,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_05 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),6,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_06 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),6,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_06 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),7,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_07 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),7,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_07 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),8,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_08 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),8,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_08 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),9,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_09 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),9,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_09 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),10,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_10 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),10,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_10 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),11,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_11 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),11,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_11 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),12,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML)) as MovML_12 " +
                ",sum(dbo.calcValorML(MONTH(b.PeriodoID),12,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME)) as MovME_12 " +
            "from coBalance b inner join coPlanCuenta c with(nolock) on(b.CuentaID=c.CuentaID and b.eg_coPlanCuenta=c.EmpresaGrupoID) " +
            "where (LEN(b.CuentaID)<=(case when(c.MascaraCta<=@CuentaLength)then c.MascaraCta else @CuentaLength end)) and {4} and ({5}) and ({7}) " +
            "group by YEAR(b.PeriodoID),b.CuentaID,c.Descriptivo,c.MovInd,c.MascaraCta)temp1)temp2 " +
            "if OBJECT_ID(N'dbo.calcValorML', N'FN') IS NOT NULL DROP FUNCTION dbo.calcValorML ", DefaultFill));

            //ReportsConfig.Add(AppReports.BalanceDePruebaPorQ, new ReportMetadata(typeof(DTO_ReportBalanceDePruebaPorQ), "declare @CuentaLength int set {2} " +
            //    "select *,(Q1_ML+Q2_ML+Q3_ML+Q4_ML) Y_ML,(Q1_ME+Q2_ME+Q3_ME+Q4_ME) Y_ME " +
            //"from(select *,(MovML_01+MovML_02+MovML_03) Q1_ML,(MovME_01+MovME_02+MovME_03) Q1_ME,(MovML_04+MovML_05+MovML_06) Q2_ML,(MovME_04+MovME_05+MovME_06) Q2_ME " +
            //    ",(MovML_07+MovML_08+MovML_09) Q3_ML,(MovME_07+MovME_08+MovME_09) Q3_ME,(MovML_10+MovML_11+MovML_12) Q4_ML,(MovME_10+MovME_11+MovME_12) Q4_ME " + 
            //"from(SELECT b.CuentaID CuentaID,c.Descriptivo CuentaDesc,YEAR(b.PeriodoID) Año " +
            //    ",case when (c.MascaraCta<=@CuentaLength) then (case when LEN(b.CuentaID)=c.MascaraCta then 1 else 0 end) else (case when LEN(b.CuentaID)=@CuentaLength then 1 else 0 end) end MaxLengthInd " +
            //    ",sum(case when (MONTH(b.PeriodoID)=1) then (b.DbSaldoIniLocML+b.DbSaldoIniExtML+b.CrSaldoIniLocML+b.CrSaldoIniExtML)/1000 else 0 end) InicialML " +
            //    ",sum(case when (MONTH(b.PeriodoID)=1) then (b.DbSaldoIniLocME+b.DbSaldoIniExtME+b.CrSaldoIniLocME+b.CrSaldoIniExtME)/1000 else 0 end) InicialME " +
            //    ",sum(case when (MONTH(b.PeriodoID)=1) then (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000 else 0 end) MovML_01 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=1) then (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000 else 0 end) MovME_01 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=2) then (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000 else 0 end) MovML_02 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=2) then (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000 else 0 end) MovME_02 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=3) then (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000 else 0 end) MovML_03 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=3) then (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000 else 0 end) MovME_03 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=4) then (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000 else 0 end) MovML_04 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=4) then (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000 else 0 end) MovME_04 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=5) then (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000 else 0 end) MovML_05 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=5) then (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000 else 0 end) MovME_05 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=6) then (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000 else 0 end) MovML_06 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=6) then (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000 else 0 end) MovME_06 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=7) then (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000 else 0 end) MovML_07 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=7) then (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000 else 0 end) MovME_07 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=8) then (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000 else 0 end) MovML_08 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=8) then (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000 else 0 end) MovME_08 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=9) then (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000 else 0 end) MovML_09 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=9) then (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000 else 0 end) MovME_09 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=10) then (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000 else 0 end) MovML_10 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=10) then (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000 else 0 end) MovME_10 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=11) then (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000 else 0 end) MovML_11 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=11) then (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000 else 0 end) MovME_11 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=12) then (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)/1000 else 0 end) MovML_12 " +
            //    ",sum(case when (MONTH(b.PeriodoID)=12) then (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)/1000 else 0 end) MovME_12 " +
            //"FROM coBalance b inner join coPlanCuenta c with(nolock) on(b.CuentaID=c.CuentaID and b.eg_coPlanCuenta=c.EmpresaGrupoID) " +
            //"WHERE (LEN(b.CuentaID)<=(case when(c.MascaraCta<=@CuentaLength)then c.MascaraCta else @CuentaLength end)) and {4} and ({5}) and ({6}) " +
            //"GROUP BY YEAR(b.PeriodoID),b.CuentaID,c.Descriptivo,c.MovInd,c.MascaraCta)temp1)temp2 order by Año,CuentaID", DefaultFill)); 
            #endregion

            #endregion

            #region Balance de Prueba Comparativo

            #region Cuenta Funcional
            ReportsConfig.Add(AppReports.coBalanceDePruebaComparativo, new ReportMetadata(typeof(DTO_ReportBalanceDePruebaComparativo),
            "create function dbo.calcValorML(@p int,@m int,@n int,@v_1 numeric(20,4),@v_2 numeric(20,4),@v_3 numeric(20,4),@v_4 numeric(20,4)) " +
            "returns numeric(20,4) begin return case when (@p=@m)then case when (@n=1) then (@v_1+@v_2+@v_3+@v_4) else (@v_1+@v_2+@v_3+@v_4)*(-1)end else 0 end end \r\ngo\r\n " +
            "declare @Year int declare @SaldoInicialInd int declare @CuentaLength int set {0} set {2} set {3} " +
            "select temp1.CuentaID,temp1.CuentaDesc,temp1.MaxLengthInd " +
                ",SUM(temp1.MovimientoML_curr) MovimientoML_curr,SUM(temp1.MovimientoML_prev) MovimientoML_prev " +
                ",SUM(temp1.MovimientoME_curr) MovimientoME_curr,SUM(temp1.MovimientoME_prev) MovimientoME_prev " +
                ",SUM(temp1.InicialML_curr)+SUM(temp1.MovimientoML_curr) FinalML_curr,SUM(temp1.InicialML_prev)+SUM(temp1.MovimientoML_prev) FinalML_prev " +
                ",SUM(temp1.InicialME_curr)+SUM(temp1.MovimientoME_curr) FinalME_curr,SUM(temp1.InicialME_prev)+SUM(temp1.MovimientoME_prev) FinalME_prev " +
            "from(select b.EmpresaID,b.CuentaID CuentaID,c.Descriptivo CuentaDesc " +
                ",case when (c.MascaraCta<=@CuentaLength) then (case when LEN(b.CuentaID)=c.MascaraCta then 1 else 0 end) else (case when LEN(b.CuentaID)=@CuentaLength then 1 else 0 end) end MaxLengthInd 	" +
                ",dbo.calcValorML(year(b.PeriodoID),@Year,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML) MovimientoML_curr " +
                ",dbo.calcValorML(year(b.PeriodoID),@Year-1,c.Naturaleza,b.DbOrigenLocML,b.DbOrigenExtML,b.CrOrigenLocML,b.CrOrigenExtML) MovimientoML_prev " +
                ",dbo.calcValorML(year(b.PeriodoID),@Year,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME) MovimientoME_curr " +
                ",dbo.calcValorML(year(b.PeriodoID),@Year-1,c.Naturaleza,b.DbOrigenLocME,b.DbOrigenExtME,b.CrOrigenLocME,b.CrOrigenExtME) MovimientoME_prev " +
                ",case when (year(b.PeriodoID)=@Year) then dbo.calcValorML(month(b.PeriodoID),1,c.Naturaleza,b.DbSaldoIniLocML,b.DbSaldoIniExtML,b.CrSaldoIniLocML,b.CrSaldoIniExtML)*@SaldoInicialInd else 0 end InicialML_curr " +
                ",case when (year(b.PeriodoID)=@Year-1) then dbo.calcValorML(month(b.PeriodoID),1,c.Naturaleza,b.DbSaldoIniLocML,b.DbSaldoIniExtML,b.CrSaldoIniLocML,b.CrSaldoIniExtML)*@SaldoInicialInd else 0 end InicialML_prev " +
                ",case when (year(b.PeriodoID)=@Year) then dbo.calcValorML(month(b.PeriodoID),1,c.Naturaleza,b.DbSaldoIniLocME,b.DbSaldoIniExtME,b.CrSaldoIniLocME,b.CrSaldoIniExtME)*@SaldoInicialInd else 0 end InicialME_curr " +
                ",case when (year(b.PeriodoID)=@Year-1) then dbo.calcValorML(month(b.PeriodoID),1,c.Naturaleza,b.DbSaldoIniLocME,b.DbSaldoIniExtME,b.CrSaldoIniLocME,b.CrSaldoIniExtME)*@SaldoInicialInd else 0 end InicialME_prev " +
            "FROM coBalance b inner join coPlanCuenta c with(nolock) on(b.CuentaID=c.CuentaID and b.eg_coPlanCuenta=c.EmpresaGrupoID) " +
            "WHERE (LEN(b.CuentaID)<=(case when(c.MascaraCta<=@CuentaLength)then c.MascaraCta else @CuentaLength end)) " +
                "and (year(b.PeriodoID)=@Year-1 or year(b.PeriodoID)=@Year) and {4} and ({5}) and ({7}) )temp1 " +
            "group by temp1.CuentaID,temp1.CuentaDesc,temp1.MaxLengthInd order by temp1.CuentaID " +
            "if OBJECT_ID(N'dbo.calcValorML', N'FN') IS NOT NULL DROP FUNCTION dbo.calcValorML ", DefaultFill));

            //ReportsConfig.Add(AppReports.BalanceDePruebaComparativo, new ReportMetadata(typeof(DTO_ReportBalanceDePruebaComparativo), "declare @Year int declare @SaldoInicialInd int declare @CuentaLength int set {0} set {2} set {3} " +
            //"select temp1.CuentaID,temp1.CuentaDesc,temp1.MaxLengthInd " +
            //    ",SUM(temp1.MovimientoML_curr) MovimientoML_curr,SUM(temp1.MovimientoML_prev) MovimientoML_prev " +
            //    ",SUM(temp1.MovimientoME_curr) MovimientoME_curr,SUM(temp1.MovimientoME_prev) MovimientoME_prev " +
            //    ",SUM(temp1.InicialML_curr)+SUM(temp1.MovimientoML_curr) FinalML_curr,SUM(temp1.InicialML_prev)+SUM(temp1.MovimientoML_prev) FinalML_prev " +
            //    ",SUM(temp1.InicialME_curr)+SUM(temp1.MovimientoME_curr) FinalME_curr,SUM(temp1.InicialME_prev)+SUM(temp1.MovimientoME_prev) FinalME_prev " +
            //"from(select b.EmpresaID,b.CuentaID CuentaID,c.Descriptivo CuentaDesc " +
            //",case when (c.MascaraCta<=@CuentaLength) then (case when LEN(b.CuentaID)=c.MascaraCta then 1 else 0 end) else (case when LEN(b.CuentaID)=@CuentaLength then 1 else 0 end) end MaxLengthInd " +
            //    ",case when (year(b.PeriodoID)=@Year) then (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML) else 0 end MovimientoML_curr " +
            //    ",case when (year(b.PeriodoID)=@Year-1) then (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML) else 0 end MovimientoML_prev " +
            //    ",case when (year(b.PeriodoID)=@Year) then (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME) else 0 end MovimientoME_curr " +
            //    ",case when (year(b.PeriodoID)=@Year-1) then (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME) else 0 end MovimientoME_prev " +
            //    ",case when (year(b.PeriodoID)=@Year and MONTH(b.PeriodoID)=1) then(b.DbSaldoIniLocML+b.DbSaldoIniExtML+b.CrSaldoIniLocML+b.CrSaldoIniExtML)*@SaldoInicialInd else 0 end InicialML_curr " +
            //    ",case when (year(b.PeriodoID)=@Year-1 and MONTH(b.PeriodoID)=1) then(b.DbSaldoIniLocML+b.DbSaldoIniExtML+b.CrSaldoIniLocML+b.CrSaldoIniExtML)*@SaldoInicialInd else 0 end InicialML_prev " +
            //    ",case when (year(b.PeriodoID)=@Year and MONTH(b.PeriodoID)=1) then(b.DbSaldoIniLocME+b.DbSaldoIniExtME+b.CrSaldoIniLocME+b.CrSaldoIniExtME)*@SaldoInicialInd else 0 end InicialME_curr " +
            //    ",case when (year(b.PeriodoID)=@Year-1 and MONTH(b.PeriodoID)=1) then(b.DbSaldoIniLocME+b.DbSaldoIniExtME+b.CrSaldoIniLocME+b.CrSaldoIniExtME)*@SaldoInicialInd else 0 end InicialME_prev " +
            //"FROM coBalance b inner join coPlanCuenta c with(nolock) on(b.CuentaID=c.CuentaID and b.eg_coPlanCuenta=c.EmpresaGrupoID) " +
            //"WHERE (LEN(b.CuentaID)<=(case when(c.MascaraCta<=@CuentaLength)then c.MascaraCta else @CuentaLength end)) " +
            //    "and (year(b.PeriodoID)=@Year-1 or year(b.PeriodoID)=@Year) and {4} and ({5}) and ({6}) " +
            //")temp1 group by temp1.CuentaID,temp1.CuentaDesc,temp1.MaxLengthInd order by temp1.CuentaID", DefaultFill));
            #endregion

            #endregion

            #region Balance General

            #region Cuenta Funcional
            ReportsConfig.Add(AppReports.coBalanceGeneral, new ReportMetadata(typeof(DTO_ReportBalanceGeneral),
            "declare @Level2 int declare @Level3 int declare @Level4 int declare @Level5 int declare @Level6 int " +
            "set {0} set {1} set {2} set {3} set {4} " +
            "select temp1.CuentaID,temp1.CuentaDesc,temp1.MaxLengthInd " +
                ",case when (LEN(temp1.CuentaID) = @Level2) then SUM(temp1.InicialML)+SUM(temp1.MovimientoML) else null end FinalML_L2 " +
                ",case when (LEN(temp1.CuentaID) = @Level2) then SUM(temp1.InicialME)+SUM(temp1.MovimientoME) else null end FinalME_L2 " +
                ",case when (LEN(temp1.CuentaID) = @Level3) then SUM(temp1.InicialML)+SUM(temp1.MovimientoML) else null end FinalML_L3 " +
                ",case when (LEN(temp1.CuentaID) = @Level3) then SUM(temp1.InicialME)+SUM(temp1.MovimientoME) else null end FinalME_L3 " +
                ",case when (LEN(temp1.CuentaID) = @Level4) then SUM(temp1.InicialML)+SUM(temp1.MovimientoML) else null end FinalML_L4 " +
                ",case when (LEN(temp1.CuentaID) = @Level4) then SUM(temp1.InicialME)+SUM(temp1.MovimientoME) else null end FinalME_L4 " +
                ",case when (LEN(temp1.CuentaID) = @Level5) then SUM(temp1.InicialML)+SUM(temp1.MovimientoML) else null end FinalML_L5 " +
                ",case when (LEN(temp1.CuentaID) = @Level5) then SUM(temp1.InicialME)+SUM(temp1.MovimientoME) else null end FinalME_L5 " +
                ",case when (LEN(temp1.CuentaID) = @Level6) then SUM(temp1.InicialML)+SUM(temp1.MovimientoML) else null end FinalML_L6 " +
                ",case when (LEN(temp1.CuentaID) = @Level6) then SUM(temp1.InicialME)+SUM(temp1.MovimientoME) else null end FinalME_L6 " +
            "from(select b.EmpresaID,b.CuentaID CuentaID,c.Descriptivo CuentaDesc " +
                ",case when LEN(b.CuentaID)=c.MascaraCta or LEN(b.CuentaID)=@Level6 then 1 else 0 end MaxLengthInd " +
                ",b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML MovimientoML " +
                ",b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME MovimientoME " +
                ",b.DbSaldoIniLocML+b.DbSaldoIniExtML+b.CrSaldoIniLocML+b.CrSaldoIniExtML InicialML " +
                ",b.DbSaldoIniLocME+b.DbSaldoIniExtME+b.CrSaldoIniLocME+b.CrSaldoIniExtME InicialME " +
            "FROM coBalance b inner join coPlanCuenta c with(nolock) on(b.CuentaID=c.CuentaID and b.eg_coPlanCuenta=c.EmpresaGrupoID) " +
            "WHERE (LEN(b.CuentaID)<=c.MascaraCta and LEN(b.CuentaID)<=@Level6) and {5} and ({6}) and ({7}))temp1 " +
            "group by temp1.CuentaID,temp1.CuentaDesc,temp1.MaxLengthInd order by temp1.CuentaID", DefaultFill));
            #endregion

            #endregion
            #endregion

            #region Saldos Report

            #region Saldos
            ReportsConfig.Add(AppReports.coSaldos, new ReportMetadata(typeof(DTO_ReportSaldos1),

            //"IF EXISTS ( SELECT * FROM sysobjects WHERE id = object_id(N'dbo.calcValorML') AND xtype IN (N'FN', N'IF', N'TF')) DROP FUNCTION dbo.calcValorML  \r\ngo\r\n " +
            "create function dbo.calcValorML(@n int,@v_1 numeric(20,4),@v_2 numeric(20,4),@v_3 numeric(20,4),@v_4 numeric(20,4)) " +
            "returns numeric(20,4) begin return case when (@n=1) then (@v_1+@v_2+@v_3+@v_4) else (@v_1+@v_2+@v_3+@v_4)*(-1)end end \r\ngo\r\n " +
            "declare @MesIni int  declare @EmpresaID char(10) declare @cuentaIni  char(15) declare @cuentaFin char(15) set {0} set {3}  set {4} set {5} " +
            " select * from(select temp1.EmpresaID,  temp1.PeriodoID,temp1.CuentaID,temp1.Descriptivo CuentaDesc,temp1.TerceroID,tercero.Descriptivo TerceroDesc " +
                ",temp1.ProyectoID,proyecto.Descriptivo ProyectoDesc,temp1.CentroCostoID,ccosto.Descriptivo CentroCostoDesc " +
                ",temp1.LineaPresupuestoID,lp.Descriptivo LineaPresupuestoDesc,temp1.BalanceTipoID ,csaldo.coSaldoControl SaldoControl " +
                ",temp1.DocumentoNumero,temp1.DocumentoPrefijo,temp1.DocumentoTercero,InicialML,InicialME,case when (temp1.Naturaleza=1) then 1 else (-1) end Signo " +
                ",(temp1.DbOrigenLocML+temp1.DbOrigenExtML) DebitoML,((temp1.CrOrigenLocML+temp1.CrOrigenExtML)*(-1)) CreditoML " +
                ",(temp1.DbOrigenLocME+temp1.DbOrigenExtME) DebitoME,((temp1.CrOrigenLocME+temp1.CrOrigenExtME)*(-1)) CreditoME " +
                ",(dbo.calcValorML(temp1.Naturaleza,temp1.DbOrigenLocML,temp1.DbOrigenExtML,temp1.CrOrigenLocML,temp1.CrOrigenExtML) + InicialML) FinalML " +
                ",(dbo.calcValorML(temp1.Naturaleza,temp1.DbOrigenLocME,temp1.DbOrigenExtME,temp1.CrOrigenLocME,temp1.CrOrigenExtME) + InicialME) FinalME " +
            " FROM(select cs.*,docCtrl.DocumentoNro DocumentoNumero,docCtrl.PrefijoID DocumentoPrefijo,docCtrl.DocumentoTercero,cuenta.Descriptivo,cuenta.Naturaleza " +
                ",case when (@MesIni < 12 and MONTH(cs.PeriodoID)=@MesIni)or(@MesIni>=12 and MONTH(cs.PeriodoID)=12 and DAY(cs.PeriodoID)=@MesIni-11) " +
                    "then dbo.calcValorML(cuenta.Naturaleza,cs.DbSaldoIniLocML,cs.DbSaldoIniExtML,cs.CrSaldoIniLocML,cs.CrSaldoIniExtML) else 0 end InicialML " +
                ",case when (@MesIni < 12 and MONTH(cs.PeriodoID)=@MesIni)or(@MesIni>=12 and MONTH(cs.PeriodoID)=12 and DAY(cs.PeriodoID)=@MesIni-11) " +
                    "then dbo.calcValorML(cuenta.Naturaleza,cs.DbSaldoIniLocME,cs.DbSaldoIniExtME,cs.CrSaldoIniLocME,cs.CrSaldoIniExtME) else 0 end InicialME  " +
            " from coCuentaSaldo cs left join glDocumentoControl docCtrl with(nolock) on(cs.IdentificadorTR=docCtrl.NumeroDoc and cs.EmpresaID=docCtrl.EmpresaID) " +
                "inner join coPlanCuenta cuenta with(nolock) on(cs.CuentaID=cuenta.CuentaID and cs.eg_coPlanCuenta=cuenta.EmpresaGrupoID) )temp1 " +
                "inner join coTercero tercero with(nolock) on(temp1.TerceroID=tercero.TerceroID and temp1.eg_coTercero=tercero.EmpresaGrupoID) " +
                "inner join coProyecto proyecto with(nolock) on(temp1.ProyectoID=proyecto.ProyectoID and temp1.eg_coProyecto=proyecto.EmpresaGrupoID) " +
                "inner join coCentroCosto ccosto with(nolock) on(temp1.CentroCostoID=ccosto.CentroCostoID and temp1.eg_coCentroCosto=ccosto.EmpresaGrupoID) " +
                "inner join plLineaPresupuesto lp with(nolock) on(temp1.LineaPresupuestoID=lp.LineaPresupuestoID and temp1.eg_plLineaPresupuesto=lp.EmpresaGrupoID) " +
                "inner join glConceptoSaldo csaldo with(nolock) on(temp1.ConceptoSaldoID=csaldo.ConceptoSaldoID and temp1.eg_glConceptoSaldo=csaldo.EmpresaGrupoID) " +
            "where temp1.EmpresaID = @EmpresaID and temp1.CuentaID BETWEEN   @cuentaIni  and @cuentaFin " + 
            " and Month(temp1.PeriodoID)>=(case when @MesIni>12 then 12 else @MesIni end) and day(PeriodoID)=(case when @MesIni>12 then @MesIni-11 else 1 end) and {1} and ({2}) and ({6}) " +
            ")temp where InicialML!=0 or DebitoML!=0 or CreditoML!=0 or FinalML!=0 or InicialME!=0 or DebitoME!=0 or CreditoME!=0 or FinalME!=0 " +
            "if OBJECT_ID(N'dbo.calcValorML', N'FN') IS NOT NULL DROP FUNCTION dbo.calcValorML ", DefaultFill));

            #endregion

            #endregion

            #region Formularios
            #region Formularios soporte
            ReportsConfig.Add(AppReports.coFormulariosDetail, new ReportMetadata(typeof(DTO_FormulariosDetail), "select " +
                "impC.ImpuestoDeclID Declaracion,impC.Renglon,impR.Descriptivo RenglonDesc,impC.CuentaID,cuenta.Descriptivo CuentaDesc " +
                ",aux.TerceroID,terc.Descriptivo TerceroDesc,csaldo.coSaldoControl SaldoControl,ctrl.PrefijoID DocumentoPrefijo " +
                ",ctrl.DocumentoNro,ctrl.DocumentoTercero,aux.vlrBaseML*(-1) BaseML,aux.vlrMdaLoc*(-1) ValorML,aux.ComprobanteID,aux.ComprobanteNro " +
            "from coImpDeclaracionCuenta impC " +
                "inner join coImpDeclaracionRenglon impR with(nolock)on (impC.ImpuestoDeclID = impR.ImpuestoDeclID and impC.Renglon=impR.Renglon and impC.EmpresaGrupoID=impR.EmpresaGrupoID) " +
                "inner join coAuxiliar aux with(nolock)on (impC.CuentaID=aux.CuentaID) " +
                "inner join glDocumentoControl ctrl with(nolock)on (aux.NumeroDoc=ctrl.NumeroDoc and aux.EmpresaID = ctrl.EmpresaID) " +
                "inner join coPlanCuenta cuenta with(nolock)on (impC.CuentaID=cuenta.CuentaID and impC.eg_coPlanCuenta=cuenta.EmpresaGrupoID) " +
                "inner join glConceptoSaldo csaldo with(nolock)on (aux.ConceptoSaldoID=csaldo.ConceptoSaldoID and aux.eg_glConceptoSaldo=csaldo.EmpresaGrupoID) " +
                "inner join coTercero terc with(nolock)on (aux.TerceroID=terc.TerceroID and aux.eg_coTercero=terc.EmpresaGrupoID) " +
                "where aux.TerceroID!=cuenta.NITCierreAnual and {0} and {1}", DefaultFill));

            #region Old query
            /* Old query
            ReportsConfig.Add(AppReports.FormulariosDetail, new ReportMetadata(typeof(DTO_FormulariosDetail), "select temp2.* " +
                ",cuenta.Descriptivo CuentaDesc,csaldo.coSaldoControl SaldoControl,tercero.Descriptivo TerceroDesc " +
            "from(select temp1.*,docCtrl.NumeroDoc DocumentoNumero,docCtrl.PrefijoID DocumentoPrefijo,docCtrl.DocumentoTercero " +
            "from( SELECT imp.*,aux.TerceroID,aux.eg_coTercero,aux.PeriodoID,aux.IdentificadorTR " +
                ",aux.EmpresaID,aux.ConceptoSaldoID,aux.eg_glConceptoSaldo,aux.vlrMdaLoc ValorML,aux.vlrBaseML BaseML,aux.ComprobanteID,aux.ComprobanteNro " +
            "FROM coImpDeclaracionCuenta imp inner join coAuxiliar aux with(nolock) on(imp.CuentaID=aux.CuentaID and imp.eg_coPlanCuenta=aux.eg_coPlanCuenta) " +
            "where {0} and {1} " +
            ")temp1 left join glDocumentoControl docCtrl with(nolock) on(temp1.IdentificadorTR=docCtrl.NumeroDoc and temp1.EmpresaID=docCtrl.EmpresaID) " +
            ")temp2 inner join coPlanCuenta cuenta with(nolock) on(temp2.CuentaID=cuenta.CuentaID and temp2.eg_coPlanCuenta=cuenta.EmpresaGrupoID) " +
            "inner join glConceptoSaldo csaldo with(nolock) on(temp2.ConceptoSaldoID=csaldo.ConceptoSaldoID and temp2.eg_glConceptoSaldo=csaldo.EmpresaGrupoID) " +
            "inner join coTercero tercero with(nolock) on(temp2.TerceroID=tercero.TerceroID and temp2.eg_coTercero=tercero.EmpresaGrupoID) " +
            "where cuenta.ImpuestoTipoID is not null order by Renglon", DefaultFill));             
            */
            #endregion
            #endregion

            #region Formularios por cuenta
            ReportsConfig.Add(AppReports.coFormulariosCuenta, new ReportMetadata(typeof(DTO_FormulariosCuenta), "select " +
                "impC.ImpuestoDeclID Declaracion,impC.Renglon,impR.Descriptivo RenglonDesc,impC.CuentaID,cuenta.Descriptivo CuentaDesc,aux.vlrMdaLoc*(-1) ValorML " +
            "from coImpDeclaracionCuenta impC " +
                "inner join coImpDeclaracionRenglon impR with(nolock)on (impC.ImpuestoDeclID = impR.ImpuestoDeclID and impC.Renglon=impR.Renglon and impC.EmpresaGrupoID=impR.EmpresaGrupoID) " +
                "inner join coAuxiliar aux with(nolock)on (impC.CuentaID=aux.CuentaID) " +
                "inner join coPlanCuenta cuenta with(nolock)on (impC.CuentaID=cuenta.CuentaID and impC.eg_coPlanCuenta=cuenta.EmpresaGrupoID) " +
                "where {0} and {1}", DefaultFill));

            #region Old query
            /* 
            ReportsConfig.Add(AppReports.FormulariosCuenta, new ReportMetadata(typeof(DTO_FormulariosCuenta), "select temp1.Declaracion " +
                ",temp1.Renglon,temp1.CuentaID,cuenta.Descriptivo CuentaDesc,temp1.Descriptivo,SUM(temp1.ValorML) ValorML " +
            "from(SELECT imp.*,aux.vlrMdaLoc ValorML " +
            "FROM coImpDeclaracionCuenta imp inner join coAuxiliar aux with(nolock) on(imp.CuentaID=aux.CuentaID and imp.eg_coPlanCuenta=aux.eg_coPlanCuenta) " +
            "where SaldoInd=0 and {0} and {1} " +
            ")temp1 inner join coPlanCuenta cuenta with(nolock) on(temp1.CuentaID=cuenta.CuentaID and temp1.eg_coPlanCuenta=cuenta.EmpresaGrupoID) " +
            "group by temp1.Declaracion,temp1.Renglon,temp1.CuentaID,cuenta.Descriptivo,temp1.Descriptivo order by Declaracion", DefaultFill));

            ReportsConfig.Add(AppReports.FormulariosCuenta_balance, new ReportMetadata(typeof(DTO_FormulariosCuenta), "select temp1.Declaracion " +
                ",temp1.Renglon,temp1.CuentaID,cuenta.Descriptivo CuentaDesc,temp1.Descriptivo,temp1.ValorML " +
            "from(select imp.Declaracion,imp.Renglon,imp.CuentaID,bal.eg_coPlanCuenta,imp.Descriptivo " +
                ",sum(bal.DbOrigenLocML+bal.DbOrigenExtML+bal.CrOrigenLocML+bal.CrOrigenExtML) ValorML " +
            "FROM coImpDeclaracionCuenta imp inner join coBalance bal with(nolock) on(imp.CuentaID=bal.CuentaID and imp.eg_coPlanCuenta=bal.eg_coPlanCuenta) " +
            "where SaldoInd=1 and {0} and {1} " +
            "group by imp.Declaracion,imp.Renglon,imp.CuentaID,bal.eg_coPlanCuenta,imp.Descriptivo " +
            ")temp1 inner join coPlanCuenta cuenta with(nolock) on(temp1.CuentaID=cuenta.CuentaID and temp1.eg_coPlanCuenta=cuenta.EmpresaGrupoID) " +
            "order by Declaracion", DefaultFill));
            */
            #endregion
            #endregion
            #endregion

            #region Relacion Documentos Report
            ReportsConfig.Add(AppReports.coRelacionDocumentos, new ReportMetadata(typeof(DTO_ReportRelacionDocumentos), "select temp.Fecha " +
                ",d.DocExternoInd DocSaldoControl,temp.DocumentoID ,temp.PrefijoID DocumentoPrefijo,prefijo.Descriptivo DocumentoPrefijoDesc " +
                ",temp.DocumentoNro DocumentoNumero ,temp.DocumentoTercero,temp.Observacion DocumentoDesc " +
                ",temp.Estado DocumentoEstado,temp.TerceroID,tercero.Descriptivo TerceroDesc,temp.MonedaID MonedaCodigo " +
            "from glDocumentoControl temp inner join glDocumento d with(nolock) on(temp.DocumentoID=d.DocumentoID) " +
            "left join coTercero tercero with(nolock) on(temp.TerceroID=tercero.TerceroID and temp.eg_coTercero=tercero.EmpresaGrupoID) " +
            "left join glPrefijo prefijo with(nolock) on(temp.PrefijoID=prefijo.PrefijoID and temp.eg_glPrefijo=prefijo.EmpresaGrupoID) " +
            "where {0} and ({1})", DefaultFill));
            #endregion

            #region Saldos Documentos Report
            ReportsConfig.Add(AppReports.coSaldosDocumentos, new ReportMetadata(typeof(DTO_ReportSaldosDocumentos), "select * from(SELECT temp1.Fecha " +
                ",temp1.DocumentoID,csaldo.coSaldoControl DocSaldoControl,temp1.DocumentoPrefijo,temp1.DocumentoNumero,temp1.DocumentoTercero,temp1.DocumentoDesc " +
                ",case when cuenta.MovInd=1 then substring(temp1.CuentaID,1,cuenta.MascaraCta)else temp1.CuentaID end CuentaID,cuenta.Descriptivo CuentaDesc " +
                ",temp1.TerceroID,tercero.Descriptivo TerceroDesc " +
                ",((temp1.DbSaldoIniLocML+temp1.DbSaldoIniExtML+temp1.CrSaldoIniLocML+temp1.CrSaldoIniExtML)+temp1.DbOrigenLocML+temp1.DbOrigenExtML+temp1.CrOrigenLocML+temp1.CrOrigenExtML) FinalML " +
                ",((temp1.DbSaldoIniLocME+temp1.DbSaldoIniExtME+temp1.CrSaldoIniLocME+temp1.CrSaldoIniExtME)+temp1.DbOrigenLocME+temp1.DbOrigenExtME+temp1.CrOrigenLocME+temp1.CrOrigenExtME) FinalME " +
            "from(select cs.*,docCtrl.DocumentoNro DocumentoNumero,docCtrl.PrefijoID DocumentoPrefijo,docCtrl.DocumentoTercero,docCtrl.DocumentoID,docCtrl.Observacion DocumentoDesc,docCtrl.Fecha " +
            "from coCuentaSaldo cs inner join glDocumentoControl docCtrl with(nolock) on(cs.IdentificadorTR=docCtrl.NumeroDoc and cs.EmpresaID=docCtrl.EmpresaID) " +
            ")temp1 left join coPlanCuenta cuenta with(nolock) on(temp1.CuentaID=cuenta.CuentaID and temp1.eg_coPlanCuenta=cuenta.EmpresaGrupoID) " +
            "left join coTercero tercero with(nolock) on(temp1.TerceroID=tercero.TerceroID and temp1.eg_coTercero=tercero.EmpresaGrupoID) " +
            "left join glConceptoSaldo csaldo with(nolock) on(temp1.ConceptoSaldoID=csaldo.ConceptoSaldoID and temp1.eg_glConceptoSaldo=csaldo.EmpresaGrupoID) " +
            "where (cuenta.MovInd=1 or LEN(temp1.CuentaID)<cuenta.MascaraCta))temp where {0} and ({1})", DefaultFill));
            #endregion

            #region Cuentas por pagar por edades
            
            #region Cuentas por Pagar Movimientos por Periodo
            ReportsConfig.Add(AppReports.cpMovimientosPeriodo, new ReportMetadata(typeof(DTO_ReportCxPxMovimientosxPeriodo), " SELECT " +
           " saldo.CuentaID,  IsNull(coTer.ApellidoPri, '') + ', ' + IsNull(coTer.NombrePri, '') as TerceroDesc, ctrl.DocumentoTercero, ctrl.TerceroID as TerceroID, aux.NumeroDoc, aux.Descriptivo, aux.Fecha, aux.ComprobanteNro, aux.vlrMdaLoc, aux.vlrMdaExt, 	 " +
           " sum(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) AS SaldoMdaLoc, " +
           " sum(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME + DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME + CrSaldoIniExtME) AS SaldoMdaExt " +
            " FROM coCuentaSaldo saldo WITH(NOLOCK) " +
               " INNER JOIN glConceptoSaldo as conSaldo with(nolock) on conSaldo.ConceptoSaldoID = saldo.ConceptoSaldoID " +
                   " AND conSaldo.coSaldoControl = 3 " +
           " INNER JOIN coAuxiliar as aux with(nolock) on aux.IdentificadorTR = saldo.IdentificadorTR " +
           " INNER JOIN glDocumentoControl as ctrl with(nolock) on ctrl.NumeroDoc = saldo.IdentificadorTR " +
           " INNER JOIN coTercero as coTer with(nolock) on coTer.TerceroID = saldo.TerceroID	" +
            " WHERE saldo.IdentificadorTR != 0 " +
               " AND {0} " + //Fecha
               " AND saldo.BalanceTipoID = 'BDP' " + //Asugnar Parametro
               " AND (	   saldo.DbOrigenLocML + saldo.DbSaldoIniLocML != 0 " +
                   "	OR saldo.DbOrigenExtML + saldo.DbSaldoIniExtML != 0 " +
                   "	OR saldo.DbOrigenLocME + saldo.DbSaldoIniLocME != 0 " +
                   "	OR saldo.DbOrigenExtME + saldo.DbSaldoIniExtME != 0 " +
                   "	OR saldo.CrOrigenLocML + saldo.CrSaldoIniLocML != 0 " +
                   "	OR saldo.CrOrigenExtML + saldo.CrSaldoIniExtML != 0 " +
                   "	OR saldo.CrOrigenLocME + saldo.CrSaldoIniLocME != 0 " +
                   "	OR saldo.CrOrigenExtME + saldo.CrSaldoIniExtME != 0 " +
                   ")" +
                   " AND {2} and ({3}) " +
                   " AND ctrl.DocumentoID = 21	" +
           " GROUP BY saldo.CuentaID, coTer.NombrePri, ctrl.DocumentoTercero,aux.Descriptivo, aux.Fecha, aux.ComprobanteNro, aux.vlrMdaLoc, aux.vlrMdaExt, aux.NumeroDoc, ctrl.TerceroID ,ctrl.DocumentoID, coTer.NombrePri, coTer.ApellidoPri " +
           " ORDER BY aux.Fecha asc ", DefaultFill));
            #endregion
            #endregion

            #region Facturas por Pagar
            ReportsConfig.Add(AppReports.cpFacturasPorPagar, new ReportMetadata(typeof(DTO_ReportFacturasPorPagar),
            "create function dbo.calcValor(@n int,@v_1 numeric(20,4),@v_2 numeric(20,4)) returns numeric(20,4)begin " +
            "return case when(@n=1) then ((@v_1+@v_2)) else ((@v_1+@v_2))*(-1) end end \r\ngo\r\n " +
            "declare @OrigenMon tinyint set {2} " +
            "select*from(SELECT docCtrl.TerceroID,terc.Descriptivo TerceroDesc,docCtrl.DocumentoTercero Documento,docCtrl.CuentaID,cuenta.Descriptivo CuentaDesc " +
                ",cXp.FacturaFecha FechaDoc,cXp.VtoFecha,docCtrl.ComprobanteID,docCtrl.ComprobanteIDNro,docCtrl.MonedaID,docCtrl.TasaCambioCONT TasaCambio " +
                ",case when(cuenta.Naturaleza=2) then -1 else 1 end Signo " +
                ",cXp.Valor VrBrutoML,cXp.ValorLocal VrNetoML,cXp.ValorExtra VrNetoME " +
                ",sum(dbo.calcValor(cuenta.Naturaleza,(cs.DbSaldoIniLocML+cs.DbSaldoIniExtML+cs.CrSaldoIniLocML+cs.CrSaldoIniExtML),(cs.DbOrigenLocML+cs.DbOrigenExtML+cs.CrOrigenLocML+cs.CrOrigenExtML)))SaldoTotalML " +
                ",sum(dbo.calcValor(cuenta.Naturaleza,(cs.DbSaldoIniLocME+cs.DbSaldoIniExtME+cs.CrSaldoIniLocME+cs.CrSaldoIniExtME),(cs.DbOrigenLocME+cs.DbOrigenExtME+cs.CrOrigenLocME+cs.CrOrigenExtME)))SaldoTotalME " +
                ",sum((cs.DbSaldoIniLocML+cs.DbSaldoIniExtML+cs.CrSaldoIniLocML+cs.CrSaldoIniExtML)+(cs.DbOrigenLocML+cs.DbOrigenExtML+cs.CrOrigenLocML+cs.CrOrigenExtML))SaldoTotalML_sinSigno " +
                ",sum((cs.DbSaldoIniLocME+cs.DbSaldoIniExtME+cs.CrSaldoIniLocME+cs.CrSaldoIniExtME)+(cs.DbOrigenLocME+cs.DbOrigenExtME+cs.CrOrigenLocME+cs.CrOrigenExtME))SaldoTotalME_sinSigno " +
            "FROM glDocumentoControl docCtrl inner join cpCuentaXPagar cXp with(nolock) on(docCtrl.NumeroDoc=cXp.NumeroDoc and docCtrl.EmpresaID=cXp.EmpresaID) " +
                "inner join coCuentaSaldo cs with(nolock) on(docCtrl.NumeroDoc=cs.IdentificadorTR and docCtrl.EmpresaID=cs.EmpresaID) " +
                "left join coPlanCuenta cuenta with(nolock) on(docCtrl.CuentaID=cuenta.CuentaID and docCtrl.eg_coPlanCuenta=cuenta.EmpresaGrupoID) " +
                "left join coTercero terc with(nolock) on(docCtrl.TerceroID=terc.TerceroID and docCtrl.eg_coTercero=terc.EmpresaGrupoID) " +
            "where cuenta.OrigenMonetario = @OrigenMon and {0} and {1} and ({3}) " +
            "group by docCtrl.TerceroID,terc.Descriptivo,docCtrl.DocumentoTercero,docCtrl.CuentaID,cuenta.Descriptivo,cXp.FacturaFecha,cXp.VtoFecha " +
                ",docCtrl.ComprobanteID,docCtrl.ComprobanteIDNro,cXp.Valor,docCtrl.TasaCambioCONT,docCtrl.MonedaID,cXp.ValorLocal,cXp.ValorExtra,cuenta.Naturaleza " +
            ") temp1 where @OrigenMon=1 and SaldoTotalML!=0 or @OrigenMon=2 and SaldoTotalME!=0 order by TerceroID,Documento " +
            "if OBJECT_ID(N'dbo.calcValor', N'FN') IS NOT NULL  DROP FUNCTION dbo.calcValor", DefaultFill));

            #region Old query
            //ReportsConfig.Add(AppReports.FacturasPorPagar, new ReportMetadata(typeof(DTO_ReportFacturasPorPagar), "declare @OrigenMon tinyint set {2} " +
            //"select*from(select docCtrl.TerceroID,terc.Descriptivo TerceroDesc,docCtrl.DocumentoTercero Documento,docCtrl.CuentaID,cuenta.Descriptivo CuentaDesc " +
            //",cXp.FacturaFecha FechaDoc,cXp.VtoFecha,docCtrl.ComprobanteID,docCtrl.ComprobanteIDNro,cXp.Valor SaldoInicial,docCtrl.MonedaID,docCtrl.TasaCambioCONT TasaCambio " +
            //",sum(case when (cuenta.Naturaleza=2)then((cs.DbSaldoIniLocML+cs.DbSaldoIniExtML+cs.CrSaldoIniLocML+cs.CrSaldoIniExtML)-cs.DbOrigenLocML-cs.DbOrigenExtML-cs.CrOrigenLocML-cs.CrOrigenExtML) " +
            //    "else((cs.DbSaldoIniLocML+cs.DbSaldoIniExtML+cs.CrSaldoIniLocML+cs.CrSaldoIniExtML)+cs.DbOrigenLocML+cs.DbOrigenExtML+cs.CrOrigenLocML+cs.CrOrigenExtML) end) SaldoActualML " +
            //",sum(case when (cuenta.Naturaleza=2)then((cs.DbSaldoIniLocME+cs.DbSaldoIniExtME+cs.CrSaldoIniLocME+cs.CrSaldoIniExtME)-cs.DbOrigenLocME-cs.DbOrigenExtME-cs.CrOrigenLocME-cs.CrOrigenExtME) " +
            //    "else((cs.DbSaldoIniLocME+cs.DbSaldoIniExtME+cs.CrSaldoIniLocME+cs.CrSaldoIniExtME)+cs.DbOrigenLocME+cs.DbOrigenExtME+cs.CrOrigenLocME+cs.CrOrigenExtME) end) SaldoActualME " +
            //"FROM glDocumentoControl docCtrl inner join cpCuentaXPagar cXp with(nolock) on(docCtrl.NumeroDoc=cXp.NumeroDoc and docCtrl.EmpresaID=cXp.EmpresaID) " +
            //    "inner join coCuentaSaldo cs with(nolock) on(docCtrl.NumeroDoc=cs.IdentificadorTR and docCtrl.EmpresaID=cs.EmpresaID) " +
            //    "left join coPlanCuenta cuenta with(nolock) on(docCtrl.CuentaID=cuenta.CuentaID and docCtrl.eg_coPlanCuenta=cuenta.EmpresaGrupoID) " +
            //    "left join coTercero terc with(nolock) on(docCtrl.TerceroID=terc.TerceroID and docCtrl.eg_coTercero=terc.EmpresaGrupoID) " +
            //"where cuenta.OrigenMonetario = @OrigenMon and {0} and {1} and ({3})  " +
            //"group by docCtrl.TerceroID,terc.Descriptivo,docCtrl.DocumentoTercero,docCtrl.CuentaID,cuenta.Descriptivo,cXp.FacturaFecha,cXp.VtoFecha " +
            //",docCtrl.ComprobanteID,docCtrl.ComprobanteIDNro,cXp.Valor,docCtrl.TasaCambioCONT,docCtrl.MonedaID  " +
            //") temp1 where @OrigenMon=1 and SaldoActualML!=0 or @OrigenMon=2 and SaldoActualME!=0 order by TerceroID,Documento", DefaultFill));
            #endregion
            #endregion

            #region Anticipos Pendientes
            ReportsConfig.Add(AppReports.cpAnticiposPendientes, new ReportMetadata(typeof(DTO_ReportAnticipo),
            "SELECT docCtrl.TerceroID,terc.Descriptivo TerceroDesc,docCtrl.DocumentoTercero,docCtrl.Observacion,a.RadicaFecha Fecha " +
                ",a.AnticipoTipoID,at.Descriptivo AnticipoTipoDesc,a.Valor Valor,docCtrl.MonedaID,docCtrl.TasaCambioCONT TasaCambio " +
            "FROM glDocumentoControl docCtrl  " +
                "inner join cpAnticipo a with(nolock) on(docCtrl.NumeroDoc=a.NumeroDoc and docCtrl.EmpresaID=a.EmpresaID) " +
                "inner join cpAnticipoTipo at with(nolock) on(a.AnticipoTipoID=at.AnticipoTipoID and a.eg_cpAnticipoTipo=at.EmpresaGrupoID) " +
                "left join coTercero terc with(nolock) on(docCtrl.TerceroID=terc.TerceroID and docCtrl.eg_coTercero=terc.EmpresaGrupoID)  " +
            "where {0} and {1} and ({3}) ", DefaultFill));
            #endregion

            #region Certificates
            
            #region Certificates Detailed
            ReportsConfig.Add(AppReports.coCertificatesDetail, new ReportMetadata(typeof(DTO_CertificatesDetailedReport), "select " +
                "aux.TerceroID,terc.Descriptivo TerceroDesc,impC.Renglon,impR.Descriptivo RenglonDesc " +
                ",impC.CuentaID,cuenta.Descriptivo CuentaDesc,csaldo.coSaldoControl SaldoControl,ctrl.PrefijoID DocumentoPrefijo " +
                ",ctrl.DocumentoNro,ctrl.DocumentoTercero,aux.vlrBaseML BaseML,aux.vlrMdaLoc ValorML,aux.ComprobanteID,aux.ComprobanteNro " +
            "from coImpDeclaracionCuenta impC " +
                "inner join coImpDeclaracionRenglon impR with(nolock)on (impC.ImpuestoDeclID=impR.ImpuestoDeclID and impC.Renglon=impR.Renglon and impC.EmpresaGrupoID=impR.EmpresaGrupoID) " +
                "inner join coAuxiliar aux with(nolock)on (impC.CuentaID=aux.CuentaID) " +
                "inner join glDocumentoControl ctrl with(nolock)on (aux.NumeroDoc=ctrl.NumeroDoc and aux.EmpresaID = ctrl.EmpresaID) " +
                "inner join coPlanCuenta cuenta with(nolock)on (impC.CuentaID=cuenta.CuentaID and impC.eg_coPlanCuenta=cuenta.EmpresaGrupoID) " +
                "inner join glConceptoSaldo csaldo with(nolock)on (aux.ConceptoSaldoID=csaldo.ConceptoSaldoID and aux.eg_glConceptoSaldo=csaldo.EmpresaGrupoID) " +
                "inner join coTercero terc with(nolock)on (aux.TerceroID=terc.TerceroID and aux.eg_coTercero=terc.EmpresaGrupoID) " +
            "where {0} and {1} and ({2}) order by aux.TerceroID", DefaultFill));

            #region Old query
            /*
            ReportsConfig.Add(AppReports.CertificatesDetail, new ReportMetadata(typeof(DTO_CertificatesDetailedReport), "select temp1.TerceroID,TerceroDesc " +
                ",CuentaID,CuentaDesc,ImpuestoTipoID,TerceroID_cs,t.Descriptivo TerceroDesc_cs,temp1.PeriodoID,csaldo.coSaldoControl SaldoControl " +
                ",DocumentoNumero,DocumentoPrefijo,DocumentoTercero,sum(BaseML) BaseML,sum(ValorML) ValorML,ComprobanteID,ComprobanteNro " +
            "from(select docCtrl.TerceroID,t.Descriptivo TerceroDesc,cs.CuentaID,c.Descriptivo CuentaDesc,c.ImpuestoTipoID " +
                ",cs.TerceroID TerceroID_cs,cs.eg_coTercero,cs.PeriodoID,cs.ConceptoSaldoID,cs.eg_glConceptoSaldo,docCtrl.NumeroDoc DocumentoNumero " +
                ",docCtrl.PrefijoID DocumentoPrefijo,docCtrl.DocumentoTercero,cs.vlBaseML BaseML " +
                ",cs.CrOrigenExtML+cs.CrOrigenLocML+cs.DbOrigenExtML+cs.DbOrigenLocML ValorML,docCtrl.ComprobanteID,docCtrl.ComprobanteIDNro ComprobanteNro " +
            "from coCuentaSaldo cs inner join glDocumentoControl docCtrl with(nolock) on (cs.IdentificadorTR = docCtrl.NumeroDoc and cs.EmpresaID = docCtrl.EmpresaID) " +
            "inner join coPlanCuenta c with(nolock) on (cs.CuentaID = c.CuentaID and cs.eg_coPlanCuenta = c.EmpresaGrupoID) " +
            "inner join coTercero t with(nolock) on (docCtrl.TerceroID = t.TerceroID and docCtrl.eg_coTercero = t.EmpresaGrupoID) " +
            "where  {0} and ({1}) )temp1 " +
            "inner join glConceptoSaldo csaldo with(nolock) on(temp1.ConceptoSaldoID=csaldo.ConceptoSaldoID and temp1.eg_glConceptoSaldo=csaldo.EmpresaGrupoID) " +
            "inner join coTercero t with(nolock) on(temp1.TerceroID_cs = t.TerceroID and temp1.eg_coTercero = t.EmpresaGrupoID) " +
            "group by temp1.TerceroID,TerceroDesc,CuentaID,CuentaDesc,ImpuestoTipoID,TerceroID_cs,t.Descriptivo,temp1.PeriodoID " +
                ",csaldo.coSaldoControl,DocumentoNumero,DocumentoPrefijo,DocumentoTercero,ComprobanteID,ComprobanteNro " +
            "order by temp1.TerceroID,CuentaID,t.Descriptivo", DefaultFill));
            */
            #endregion
            #endregion
            #endregion

            #region Recibo de Caja
            ReportsConfig.Add(AppReports.coReciboCaja, new ReportMetadata(typeof(DTO_ReportReciboCaja), " select docCtrl.DocumentoID " +
                 ",docCtrl.Fecha,caja.CajaID,caja.Descriptivo CajaDesc,docCtrl.DocumentoNro ReciboNro,docCtrl.ComprobanteID,docCtrl.ComprobanteIDNro ComprobanteNro " +
                 ",docCtrl.TerceroID,tercero.Descriptivo TerceroDesc,docCtrl.Observacion DocumentoDesc,docCtrl.MonedaID MonedaID,recibo.Valor Valor, " +
                 ",aux.CuentaID,cuenta.Descriptivo CuentaDesc,aux.TerceroID TerceroID_cuenta,aux.vlrMdaLoc ValorML_cuenta " +
            "from glDocumentoControl docCtrl inner join tsReciboCajaDocu recibo with(nolock) on(docCtrl.NumeroDoc=recibo.NumeroDoc) " +
            "inner join coAuxiliar aux with(nolock) on(docCtrl.PeriodoDoc = aux.PeriodoID and docCtrl.EmpresaID = aux.EmpresaID and docCtrl.ComprobanteID = aux.ComprobanteID and docCtrl.ComprobanteIDNro = aux.ComprobanteNro) " +
            "left join tsCaja caja with(nolock) on(recibo.CajaID=caja.CajaID and recibo.eg_tsCaja=caja.EmpresaGrupoID) " +
            "left join coTercero tercero with(nolock) on(docCtrl.TerceroID=tercero.TerceroID and docCtrl.eg_coTercero=tercero.EmpresaGrupoID) " +
            "left join coPlanCuenta cuenta with(nolock) on(aux.CuentaID=cuenta.CuentaID and aux.eg_coPlanCuenta=cuenta.EmpresaGrupoID) " +
            "where docCtrl.DocumentoID = 32 and {0} and ({1})", ReciboCajaFill));
            #endregion

            #region Programacion de Pagos
            ReportsConfig.Add(AppReports.tsProgramacionPagos, new ReportMetadata(typeof(DTO_ReportProgramacionPagos), "select cxp.BancoCuentaID " +
                ",bc.Descriptivo BancoCuentaDesc,ctrl.TerceroID,terc.Descriptivo TerceroDesc,ctrl.DocumentoTercero Factura " +
                ",ctrl.Observacion Concepto,cxp.FacturaFecha,ctrl.MonedaID,cxp.ValorPago " +
            "from glDocumentoControl ctrl inner join cpCuentaXPagar cxp with(nolock) on(ctrl.NumeroDoc=cxp.NumeroDoc) " +
            "left join tsBancosCuenta bc with(nolock) on(cxp.BancoCuentaID=bc.BancoCuentaID) " +
            "left join coTercero terc with(nolock) on(ctrl.TerceroID=terc.TerceroID and ctrl.eg_coTercero=terc.EmpresaGrupoID) " +
            "where cxp.PagoAprobacionInd=1", DefaultFill));
            #endregion

            #region PagoFacturas
            //ReportsConfig.Add(AppReports.PagoFacturas, new ReportMetadata(typeof(DTO_ReportPagoFacturas), "select " +
            //"NumeroDoc from tsBancosDocu with(nolock) where {0}", DefaultFill));
            #endregion

            #region Detalle de Nómina
            //            ReportsConfig.Add(AppReports.Certificates, new ReportMetadata(typeof(DTO_Certificates), "SELECT " +
            //                "    noEmpleado.EmpleadoID, noEmpleado.Descriptivo, glDocumentoControl.DocumentoID, noLiquidacionesDocu.* "+
            //"FROM glDocumentoControl "+
            //"INNER JOIN noLiquidacionesDocu ON  glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc "+
            //"INNER JOIN noEmpleado ON noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID "+
            //"WHERE glDocumentoControl.EmpresaID = @EmpresaID "+
            //"AND glDocumentoControl.DocumentoID = @DocumentoID "+ 
            //"AND glDocumentoControl.PeriodoDoc = @Periodo ",ReportsConfig )
            #endregion
        }

        public List<DTO_BasicReport> GetData(int reportId, DTO_glConsulta consulta, List<ConsultasFields> fields)
        {
            try
            {
                List<DTO_BasicReport> res = new List<DTO_BasicReport>();
                ReportMetadata metadata = null;
                ReportsConfig.TryGetValue(reportId, out metadata);
                if (metadata == null)
                {
                    return null;
                }
                string queryTotal = string.Empty + metadata.Query;
                if (consulta != null && consulta.Filtros != null)
                {
                    string[] where = CreateSQLFilters(reportId, consulta.Filtros, metadata, fields);
                    if (queryTotal.Contains("{") && queryTotal.Contains("}"))
                    {
                            queryTotal = string.Format(queryTotal, where);
                    }
                    else
                    {
                        queryTotal = "SELECT * FROM (" + queryTotal + ") TEMP WHERE " + where[0];
                    }
                }
                else
                {
                    int count = queryTotal.Count(x => x == '{');
                    string[] where = new string[count];
                    for (int i = 0; i < count; i++)
                    {
                        where[i] = "1=1";
                    }
                    if (count > 0)
                    {
                        queryTotal = string.Format(queryTotal, where);
                    }
                }
                if (queryTotal.Contains("\r\ngo\r\n"))
                {
                    Regex regex = new Regex(@"\r{0,1}\ngo\r{0,1}\n");
                    string[] commands = regex.Split(queryTotal);
                    if (commands[0] != string.Empty)
                    {
                        using (SqlCommand command = new SqlCommand(commands[0], base.MySqlConnection))
                        {
                            command.ExecuteNonQuery();
                            command.Dispose();
                        }
                    }
                    if (commands[1] != string.Empty)
                    {
                        queryTotal = commands[1];
                    }
                }
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                
                mySqlCommand.CommandText = queryTotal;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                res = metadata.FillMethod(dr, metadata.DtoType);
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccAseguradoraReport");
                throw exception;
            }
        }

        /// <summary>
        /// Convierte un grupo de filtros en un grupo de strings (WHERE) de sql
        /// </summary>
        /// <param name="reportCode">codigo del reporte</param>
        /// <param name="filters">filtros</param>
        /// <param name="metadata">metadata del reporte</param>
        /// <returns></returns>
        public string[] CreateSQLFilters(int reportCode, List<DTO_glConsultaFiltro> filters, ReportMetadata metadata, List<ConsultasFields> consultaFields)
        {
            List<string> sqlFilters = new List<string>();
            Dictionary<int, List<DTO_glConsultaFiltro>> organizedFilters = new Dictionary<int, List<DTO_glConsultaFiltro>>();
            Dictionary<int, Dictionary<string, string>> fieldsTables = new Dictionary<int, Dictionary<string, string>>();
            switch (reportCode)
            {
                #region LibroDiario, InventariosBalance
                case AppReports.coLibroDiario:
                case AppReports.coInventariosBalance:
                    List<DTO_glConsultaFiltro> periodoFilter_MesIni = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> periodoFilter_MesFin = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> periodoFilters = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> tbFilters = new List<DTO_glConsultaFiltro>();
                    foreach (DTO_glConsultaFiltro filter in filters)
                    {
                        switch (filter.CampoFisico)
                        {
                            case "@MesIni":
                                periodoFilter_MesIni.Add(filter);
                                break;
                            case "@MesFin":
                                periodoFilter_MesFin.Add(filter);
                                break;
                            case "Month(PeriodoID)":
                            case "Year(PeriodoID)":
                                periodoFilters.Add(filter);
                                break;
                            case "BalanceTipoID":
                                tbFilters.Add(filter);
                                break;
                        };
                    };
                    organizedFilters.Add(0, periodoFilter_MesIni);
                    organizedFilters.Add(1, periodoFilters);
                    organizedFilters.Add(2, tbFilters);
                    break;
                #endregion

                #region LibroMayor
                case AppReports.coLibroMayor:
                    periodoFilters = new List<DTO_glConsultaFiltro>();
                    tbFilters = new List<DTO_glConsultaFiltro>();
                    foreach (DTO_glConsultaFiltro filter in filters)
                    {
                        switch (filter.CampoFisico)
                        {
                            case "Month(PeriodoID)":
                            case "Year(PeriodoID)":
                            case "LEN(base.CuentaID)":
                                periodoFilters.Add(filter);
                                break;
                            case "BalanceTipoID":
                                tbFilters.Add(filter);
                                break;
                        };
                    };
                    organizedFilters.Add(0, periodoFilters);
                    organizedFilters.Add(1, tbFilters);
                    Dictionary<string, string> tables = new Dictionary<string, string>();
                    tables.Add("CuentaID", "base");
                    fieldsTables.Add(0, tables);
                    break;
                #endregion

                #region ReporteComprobantes
                case AppReports.coReporteComprobantes:
                //case AppReports.ComprobanteControl:
                    periodoFilters = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> userFilters = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> tableType = new List<DTO_glConsultaFiltro>();

                    foreach (DTO_glConsultaFiltro filter in filters)
                    {
                        switch (filter.CampoFisico)
                        {
                            case "Month(PeriodoID)":
                            case "Year(PeriodoID)":
                                periodoFilters.Add(filter);
                                break;
                            case "coAuxiliar":
                            case "coAuxiliarPre":
                                tableType.Add(filter);
                                break;
                            default:
                                userFilters.Add(filter);
                                break;
                        };
                    };
                    organizedFilters.Add(0, tableType);
                    organizedFilters.Add(1, periodoFilters);

                    if (userFilters.Count != 0)
                    {
                        organizedFilters.Add(2, userFilters);
                        tables = new Dictionary<string, string>();
                        foreach (DTO_glConsultaFiltro filter in userFilters)
                        {
                            if (!filter.CampoFisico.Contains("base") && !tables.ContainsKey(filter.CampoFisico))
                            {
                                tables.Add(filter.CampoFisico, "base");
                            };
                        };
                        fieldsTables.Add(2, tables);
                    }
                    else
                    {
                        DTO_glConsultaFiltro fakeFilter = new DTO_glConsultaFiltro();
                        fakeFilter.CampoFisico = "1";
                        fakeFilter.OperadorFiltro = "=";
                        fakeFilter.OperadorSentencia = "AND";
                        fakeFilter.ValorFiltro = "1";
                        userFilters.Add(fakeFilter);
                        organizedFilters.Add(2, userFilters);
                    };
                    break;
                #endregion

                #region BalanceDePrueba, BalanceDePruebaPorMeses, BalanceDePruebaPorQ, BalanceDePruebaComparativo, TasasDeCierre
                case AppReports.coBalanceDePrueba:
                case AppReports.coBalanceDePruebaPorMeses:
                case AppReports.coBalanceDePruebaPorQ:
                case AppReports.coBalanceDePruebaComparativo:
                case AppReports.coTasasDeCierre:
                    List<DTO_glConsultaFiltro> periodoFilter_Year = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> cuentaLengthFilter = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> saldoInicialInd = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> empresaID = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> cuentaIni = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> cuentaFin = new List<DTO_glConsultaFiltro>();
                    periodoFilter_MesIni = new List<DTO_glConsultaFiltro>();
                    periodoFilter_MesFin = new List<DTO_glConsultaFiltro>();
                    periodoFilters = new List<DTO_glConsultaFiltro>();
                    tbFilters = new List<DTO_glConsultaFiltro>();
                    userFilters = new List<DTO_glConsultaFiltro>();

                    foreach (DTO_glConsultaFiltro filter in filters)
                    {
                        switch (filter.CampoFisico)
                        {
                            case "@Year":
                                periodoFilter_Year.Add(filter);
                                break;
                            case "@MesIni":
                                periodoFilter_MesIni.Add(filter);
                                break;
                            case "@MesFin":
                                periodoFilter_MesFin.Add(filter);
                                break;
                            case "@CuentaLength":
                                cuentaLengthFilter.Add(filter);
                                break;
                            case "@SaldoInicialInd":
                                saldoInicialInd.Add(filter);
                                break;
                            case "Month(PeriodoID)":
                            case "Year(PeriodoID)":
                                periodoFilters.Add(filter);
                                break;
                            case "b.BalanceTipoID":
                                tbFilters.Add(filter);
                                break;
                            case "@EmpresaID":
                                empresaID.Add(filter);
                                break;
                            case "@cuentaIni":
                                cuentaIni.Add(filter);
                                break;
                            case"@cuentaFin":
                                cuentaFin.Add(filter);
                                break;
                            default:
                                userFilters.Add(filter);
                                break;
                        };
                    };
                    organizedFilters.Add(0, periodoFilter_Year);
                    organizedFilters.Add(1, periodoFilter_MesIni);
                    organizedFilters.Add(2, cuentaLengthFilter);
                    organizedFilters.Add(3, saldoInicialInd);
                    organizedFilters.Add(4, periodoFilters);
                    organizedFilters.Add(5, tbFilters);
                    organizedFilters.Add(6, empresaID);
                    organizedFilters.Add(7, cuentaIni);
                    organizedFilters.Add(8, cuentaFin);                   

                    if (userFilters.Count != 0)
                    {
                        organizedFilters.Add(9, userFilters);
                        tables = new Dictionary<string, string>();
                        foreach (DTO_glConsultaFiltro filter in userFilters)
                        {
                            if (!filter.CampoFisico.Contains("b") && !tables.ContainsKey(filter.CampoFisico))
                            {
                                tables.Add(filter.CampoFisico, "b");
                            };
                        };
                        fieldsTables.Add(9, tables);
                    }
                    else
                    {
                        DTO_glConsultaFiltro fakeFilter = new DTO_glConsultaFiltro();
                        fakeFilter.CampoFisico = "1";
                        fakeFilter.OperadorFiltro = "=";
                        fakeFilter.OperadorSentencia = "AND";
                        fakeFilter.ValorFiltro = "1";
                        userFilters.Add(fakeFilter);
                        organizedFilters.Add(9, userFilters);
                    };
                    organizedFilters.Add(10, periodoFilter_MesFin);
                    break;
                #endregion

                #region BalanceGeneral
                case AppReports.coBalanceGeneral:
                    List<DTO_glConsultaFiltro> level2 = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> level3 = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> level4 = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> level5 = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> level6 = new List<DTO_glConsultaFiltro>();
                    periodoFilters = new List<DTO_glConsultaFiltro>();
                    tbFilters = new List<DTO_glConsultaFiltro>();
                    userFilters = new List<DTO_glConsultaFiltro>();

                    foreach (DTO_glConsultaFiltro filter in filters)
                    {
                        switch (filter.CampoFisico)
                        {
                            case "@Level2":
                                level2.Add(filter);
                                break;
                            case "@Level3":
                                level3.Add(filter);
                                break;
                            case "@Level4":
                                level4.Add(filter);
                                break;
                            case "@Level5":
                                level5.Add(filter);
                                break;
                            case "@Level6":
                                level6.Add(filter);
                                break;
                            case "Month(PeriodoID)":
                            case "Year(PeriodoID)":
                                periodoFilters.Add(filter);
                                break;
                            case "b.BalanceTipoID":
                                tbFilters.Add(filter);
                                break;
                            default:
                                userFilters.Add(filter);
                                break;
                        };
                    };
                    organizedFilters.Add(0, level2);
                    organizedFilters.Add(1, level3);
                    organizedFilters.Add(2, level4);
                    organizedFilters.Add(3, level5);
                    organizedFilters.Add(4, level6);
                    organizedFilters.Add(5, periodoFilters);
                    organizedFilters.Add(6, tbFilters);

                    if (userFilters.Count != 0)
                    {
                        organizedFilters.Add(7, userFilters);
                        tables = new Dictionary<string, string>();
                        foreach (DTO_glConsultaFiltro filter in userFilters)
                        {
                            if (!filter.CampoFisico.Contains("b") && !tables.ContainsKey(filter.CampoFisico))
                            {
                                tables.Add(filter.CampoFisico, "b");
                            };
                        };
                        fieldsTables.Add(7, tables);
                    }
                    else
                    {
                        DTO_glConsultaFiltro fakeFilter = new DTO_glConsultaFiltro();
                        fakeFilter.CampoFisico = "1";
                        fakeFilter.OperadorFiltro = "=";
                        fakeFilter.OperadorSentencia = "AND";
                        fakeFilter.ValorFiltro = "1";
                        userFilters.Add(fakeFilter);
                        organizedFilters.Add(7, userFilters);
                    };
                    break;
                #endregion

                #region Saldos
                case AppReports.coSaldos:
                periodoFilter_MesIni = new List<DTO_glConsultaFiltro>();
                periodoFilter_MesFin = new List<DTO_glConsultaFiltro>();
                    periodoFilters = new List<DTO_glConsultaFiltro>();
                    tbFilters = new List<DTO_glConsultaFiltro>();
                    userFilters = new List<DTO_glConsultaFiltro>();
                    empresaID = new List<DTO_glConsultaFiltro>();
                    cuentaIni = new List<DTO_glConsultaFiltro>();
                    cuentaFin = new List<DTO_glConsultaFiltro>();

                    foreach (DTO_glConsultaFiltro filter in filters)
                    {
                        switch (filter.CampoFisico)
                        {
                            case "@MesIni":
                                periodoFilter_MesIni.Add(filter);
                                break;
                            case "@MesFin":
                                periodoFilter_MesFin.Add(filter);
                                break;
                            case "Month(temp1.PeriodoID)":
                            case "Year(temp1.PeriodoID)":
                                periodoFilters.Add(filter);
                                break;
                            case "temp1.BalanceTipoID":
                                tbFilters.Add(filter);
                                break;
                            case "@EmpresaID":
                                empresaID.Add(filter);
                                break;
                            case "@cuentaIni":
                                cuentaIni.Add(filter);
                                break;
                            case "@cuentaFin":
                                cuentaFin.Add(filter);
                                break;
                            default:
                                userFilters.Add(filter);
                                break;
                        };
                    };
                    organizedFilters.Add(0, periodoFilter_MesIni);
                    organizedFilters.Add(1, periodoFilters);
                    organizedFilters.Add(2, tbFilters);
                    organizedFilters.Add(3, empresaID);
                    organizedFilters.Add(4, cuentaIni);
                    organizedFilters.Add(5, cuentaFin);

                    if (userFilters.Count != 0)
                    {
                        organizedFilters.Add(6, userFilters);
                        tables = new Dictionary<string, string>();
                        foreach (DTO_glConsultaFiltro filter in userFilters)
                        {
                            if (!filter.CampoFisico.Contains("temp1") && !tables.ContainsKey(filter.CampoFisico))
                            {
                                tables.Add(filter.CampoFisico, "temp1");
                            };
                        };
                        fieldsTables.Add(6, tables);
                    }
                    else
                    {
                        DTO_glConsultaFiltro fakeFilter = new DTO_glConsultaFiltro();
                        fakeFilter.CampoFisico = "1";
                        fakeFilter.OperadorFiltro = "=";
                        fakeFilter.OperadorSentencia = "AND";
                        fakeFilter.ValorFiltro = "1";
                        userFilters.Add(fakeFilter);
                        organizedFilters.Add(6, userFilters);
                    };
                    break;
                #endregion

                #region Formularios, Formularios_balance, FormulariosDetail, FormulariosCuenta, FormulariosCuenta_balance
                //case AppReports.Formularios:
                //case AppReports.Formularios_balance:
                case AppReports.coFormulariosDetail:
                case AppReports.coFormulariosCuenta:
                //case AppReports.FormulariosCuenta_balance:
                case AppReports.coCertificates:
                case AppReports.coCertificatesDetail:

                    periodoFilters = new List<DTO_glConsultaFiltro>();
                    tbFilters = new List<DTO_glConsultaFiltro>();
                    userFilters = new List<DTO_glConsultaFiltro>();

                    foreach (DTO_glConsultaFiltro filter in filters)
                    {
                        switch (filter.CampoFisico)
                        {
                            case "Month(aux.Fecha)":
                            case "Year(aux.Fecha)":
                                periodoFilters.Add(filter);
                                break;
                            case "impC.ImpuestoDeclID":
                                tbFilters.Add(filter);
                                break;
                            default:
                                userFilters.Add(filter);
                                break;
                        };
                    };
                    organizedFilters.Add(0, periodoFilters);
                    organizedFilters.Add(1, tbFilters);

                    if (userFilters.Count != 0)
                    {
                        organizedFilters.Add(2, userFilters);
                        tables = new Dictionary<string, string>();
                        foreach (DTO_glConsultaFiltro filter in userFilters)
                        {
                            if (!filter.CampoFisico.Contains("aux") && !tables.ContainsKey(filter.CampoFisico))
                            {
                                tables.Add(filter.CampoFisico, "aux");
                            };
                        };
                        fieldsTables.Add(2, tables);
                    }
                    else
                    {
                        DTO_glConsultaFiltro fakeFilter = new DTO_glConsultaFiltro();
                        fakeFilter.CampoFisico = "1";
                        fakeFilter.OperadorFiltro = "=";
                        fakeFilter.OperadorSentencia = "AND";
                        fakeFilter.ValorFiltro = "1";
                        userFilters.Add(fakeFilter);
                        organizedFilters.Add(2, userFilters);
                    };
                    break;
                #endregion

                #region SaldosDocumentos, Relacion Documentos
                case AppReports.coSaldosDocumentos:
                case AppReports.coRelacionDocumentos:
                    periodoFilters = new List<DTO_glConsultaFiltro>();
                    userFilters = new List<DTO_glConsultaFiltro>();

                    foreach (DTO_glConsultaFiltro filter in filters)
                    {
                        switch (filter.CampoFisico)
                        {
                            case "Month(temp.Fecha)":
                            case "Year(temp.Fecha)":
                            case "temp.DocumentoID":
                                periodoFilters.Add(filter);
                                break;
                            //case "temp.DocumentoID":
                            //    else userFilters.Add(filter);
                            //    break;
                            default:
                                userFilters.Add(filter);
                                break;
                        };
                    };
                    organizedFilters.Add(0, periodoFilters);

                    if (userFilters.Count != 0)
                    {
                        organizedFilters.Add(1, userFilters);
                        tables = new Dictionary<string, string>();
                        foreach (DTO_glConsultaFiltro filter in userFilters)
                        {
                            if (!filter.CampoFisico.Contains("temp") && !tables.ContainsKey(filter.CampoFisico))
                            {
                                tables.Add(filter.CampoFisico, "temp");
                            };
                        };
                        fieldsTables.Add(1, tables);
                    }
                    else
                    {
                        DTO_glConsultaFiltro fakeFilter = new DTO_glConsultaFiltro();
                        fakeFilter.CampoFisico = "1";
                        fakeFilter.OperadorFiltro = "=";
                        fakeFilter.OperadorSentencia = "AND";
                        fakeFilter.ValorFiltro = "1";
                        userFilters.Add(fakeFilter);
                        organizedFilters.Add(1, userFilters);
                    };
                    break;
                #endregion

                #region CxPMovimientos por Periodo
                case AppReports.cpMovimientosPeriodo:

                    List<DTO_glConsultaFiltro> periodoFiltersSaldo = new List<DTO_glConsultaFiltro>();
                    tbFilters = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> periodoFiltersAux = new List<DTO_glConsultaFiltro>();
                    userFilters = new List<DTO_glConsultaFiltro>();
                    {
                        foreach (DTO_glConsultaFiltro filter in filters)
                        {
                            switch (filter.CampoFisico)
                            {
                                case "year(saldo.PeriodoID)":
                                case "Month(saldo.PeriodoID)":
                                    periodoFiltersSaldo.Add(filter);
                                    break;
                                case "saldo.BalanceTipoID":
                                    tbFilters.Add(filter);
                                    break;
                                case "year(aux.PeriodoID)":
                                case "Month(aux.PeriodoID)":
                                    periodoFiltersAux.Add(filter);
                                    break;
                                default:
                                    userFilters.Add(filter);
                                    break;
                            }
                        }
                    }
                    organizedFilters.Add(0, periodoFiltersSaldo);
                    organizedFilters.Add(1, tbFilters);
                    organizedFilters.Add(2, periodoFiltersAux);

                    if (userFilters.Count != 0)
                    {
                        organizedFilters.Add(3, userFilters);
                        tables = new Dictionary<string, string>();
                        foreach (DTO_glConsultaFiltro filter in userFilters)
                        {
                            if (filter.CampoFisico.Contains("TerceroID") && !filter.CampoFisico.Contains("saldo") && !tables.ContainsKey(filter.CampoFisico))
                            {
                                tables.Add(filter.CampoFisico, "saldo");
                            };
                        };
                        fieldsTables.Add(3, tables);
                    }
                    else
                    {
                        DTO_glConsultaFiltro fakeFilter = new DTO_glConsultaFiltro();
                        fakeFilter.CampoFisico = "1";
                        fakeFilter.OperadorFiltro = "=";
                        fakeFilter.OperadorSentencia = "AND";
                        fakeFilter.ValorFiltro = "1";
                        userFilters.Add(fakeFilter);
                        organizedFilters.Add(3, userFilters);
                    }
                    break;
                #endregion

                #region ReciboCaja
                case AppReports.coReciboCaja:
                    periodoFilters = new List<DTO_glConsultaFiltro>();
                    userFilters = new List<DTO_glConsultaFiltro>();

                    foreach (DTO_glConsultaFiltro filter in filters)
                    {
                        switch (filter.CampoFisico)
                        {
                            case "Month(docCtrl.Fecha)":
                            case "Year(docCtrl.Fecha)":
                                periodoFilters.Add(filter);
                                break;
                            case "docCtrl.DocumentoNro":
                            case "recibo.CajaID":
                                periodoFilters.Add(filter);
                                break;
                            default:
                                userFilters.Add(filter);
                                break;
                        };
                    };
                    organizedFilters.Add(0, periodoFilters);

                    if (userFilters.Count != 0)
                    {
                        organizedFilters.Add(1, userFilters);
                        tables = new Dictionary<string, string>();
                        foreach (DTO_glConsultaFiltro filter in userFilters)
                        {
                            if (filter.CampoFisico.Contains("TerceroID") && !filter.CampoFisico.Contains("docCtrl") && !tables.ContainsKey(filter.CampoFisico))
                            {
                                tables.Add(filter.CampoFisico, "docCtrl");
                            };

                        };
                        fieldsTables.Add(1, tables);
                    }
                    else
                    {
                        DTO_glConsultaFiltro fakeFilter = new DTO_glConsultaFiltro();
                        fakeFilter.CampoFisico = "1";
                        fakeFilter.OperadorFiltro = "=";
                        fakeFilter.OperadorSentencia = "AND";
                        fakeFilter.ValorFiltro = "1";
                        userFilters.Add(fakeFilter);
                        organizedFilters.Add(1, userFilters);
                    };
                    break;
                #endregion

                #region Tasas De Cierre Diarias
                case AppReports.coTasasDiarias:
                    periodoFilters = new List<DTO_glConsultaFiltro>();

                    foreach (DTO_glConsultaFiltro filter in filters)
                    {
                        switch (filter.CampoFisico)
                        {
                            case "Year(Fecha)":
                            case "EmpresaGrupoID":
                                periodoFilters.Add(filter);
                                break;
                        };
                    };
                    organizedFilters.Add(0, periodoFilters);
                    break;
                #endregion

                #region Facturas por Pagar
                case AppReports.cpFacturasPorPagar:
                case AppReports.cpAnticiposPendientes:
                    periodoFilters = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> documentoFilters = new List<DTO_glConsultaFiltro>();
                    List<DTO_glConsultaFiltro> origenMonetarioFilters = new List<DTO_glConsultaFiltro>();
                    userFilters = new List<DTO_glConsultaFiltro>();

                    foreach (DTO_glConsultaFiltro filter in filters)
                    {
                        switch (filter.CampoFisico)
                        {
                            case "Month(FacturaFecha)":
                            case "Year(FacturaFecha)":
                            case "Month(a.RadicaFecha)":
                            case "Year(a.RadicaFecha)":
                                periodoFilters.Add(filter);
                                break;
                            case "docCtrl.DocumentoID":
                                documentoFilters.Add(filter);
                                break;
                            case "@OrigenMon":
                                origenMonetarioFilters.Add(filter);
                                break;
                            default:
                                userFilters.Add(filter);
                                break;
                        };
                    };
                    organizedFilters.Add(0, periodoFilters);
                    organizedFilters.Add(1, documentoFilters);
                    organizedFilters.Add(2, origenMonetarioFilters);

                    if (userFilters.Count != 0)
                    {
                        organizedFilters.Add(3, userFilters);
                        tables = new Dictionary<string, string>();
                        foreach (DTO_glConsultaFiltro filter in userFilters)
                        {
                            if (filter.CampoFisico.Contains("TerceroID") && !filter.CampoFisico.Contains("docCtrl") && !tables.ContainsKey(filter.CampoFisico))
                            {
                                tables.Add(filter.CampoFisico, "docCtrl");
                            };
                        };
                        fieldsTables.Add(3, tables);
                    }
                    else
                    {
                        DTO_glConsultaFiltro fakeFilter = new DTO_glConsultaFiltro();
                        fakeFilter.CampoFisico = "1";
                        fakeFilter.OperadorFiltro = "=";
                        fakeFilter.OperadorSentencia = "AND";
                        fakeFilter.ValorFiltro = "1";
                        userFilters.Add(fakeFilter);
                        organizedFilters.Add(3, userFilters);
                    };
                    break;
                #endregion

                default:
                    organizedFilters.Add(0, filters);
                    break;
            }

            foreach (KeyValuePair<int, List<DTO_glConsultaFiltro>> kvp in organizedFilters)
            {
                Dictionary<string, string> tables = null;
                fieldsTables.TryGetValue(kvp.Key, out tables);
                string where = SentenceTransformer.Transformer.WhereSql(kvp.Value, metadata.DtoType, consultaFields, tables);
                sqlFilters.Add(where);
            }
            return sqlFilters.ToArray();
        }

        /// <summary>
        /// Crea un dto a partir de el data reader y las porpiedades de la maestra
        /// </summary>
        /// <param name="dr">datareader</param>
        /// <param name="props">maestra</param>
        /// <returns></returns>
        protected DTO_BasicReport CreateDto(IDataReader dr, Type dtoType)
        {
            try
            {
                return (DTO_BasicReport)Activator.CreateInstance(dtoType, new object[] { dr });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Metodos para llenar Resultados
        /// <summary>
        /// Metodo para llenar fila a fila con constructor
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public List<DTO_BasicReport> DefaultFill(SqlDataReader dr, Type t)
        {
            List<DTO_BasicReport> res = new List<DTO_BasicReport>();
            while (dr.Read())
            {

                DTO_BasicReport row = CreateDto(dr, t);
                if (row != null)
                    res.Add(row);
            }
            dr.Close();
            return res;
        }

        /// <summary>
        /// Metodo para llenar una lista de DTO_ReportReciboCaja
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public List<DTO_BasicReport> ReciboCajaFill(SqlDataReader dr, Type t)
        {
            List<DTO_BasicReport> res = new List<DTO_BasicReport>();
            while (dr.Read())
            {
                string caja = Convert.ToString(dr["CajaID"]);
                string recibo = Convert.ToString(dr["ReciboNro"]);
                bool nuevo = true;
                DTO_ReportReciboCaja reportObject = new DTO_ReportReciboCaja(dr);
                List<DTO_BasicReport> f = res.Where(x => ((DTO_ReportReciboCaja)x).CajaID.Trim().Equals(caja.Trim()) && ((DTO_ReportReciboCaja)x).ReciboNro.Trim().Equals(recibo.Trim())).ToList();
                if (f.Count > 0)
                {
                    reportObject = (DTO_ReportReciboCaja)f.First();
                    nuevo = false;
                }
                else
                    reportObject.ReciboDetail = new List<DTO_ReciboDetail>();

                DTO_ReciboDetail reciboDetail = new DTO_ReciboDetail(dr);
                reportObject.ReciboDetail.Add(reciboDetail);

                if (nuevo)
                    res.Add(reportObject);
            }
            dr.Close();
            return res;
        }

        public List<DTO_BasicReport> NominaDetalle(SqlDataReader dr, Type t)
        {
            List<DTO_BasicReport> res = new List<DTO_BasicReport>();
            while (dr.Read())
            {
                string caja = Convert.ToString(dr["CajaID"]);
                string recibo = Convert.ToString(dr["ReciboNro"]);
                bool nuevo = true;
                DTO_ReportReciboCaja reportObject = new DTO_ReportReciboCaja(dr);
                List<DTO_BasicReport> f = res.Where(x => ((DTO_ReportReciboCaja)x).CajaID.Trim().Equals(caja.Trim()) && ((DTO_ReportReciboCaja)x).ReciboNro.Trim().Equals(recibo.Trim())).ToList();
                if (f.Count > 0)
                {
                    reportObject = (DTO_ReportReciboCaja)f.First();
                    nuevo = false;
                }
                else
                    reportObject.ReciboDetail = new List<DTO_ReciboDetail>();

                DTO_ReciboDetail reciboDetail = new DTO_ReciboDetail(dr);
                reportObject.ReciboDetail.Add(reciboDetail);

                if (nuevo)
                    res.Add(reportObject);
            }
            dr.Close();
            return res;
        }

        #endregion
    }

    /// <summary>
    /// Clase que maneja la metadata de un reporte
    /// </summary>
    public class ReportMetadata
    {
        public ReportMetadata(Type dtoType, string query, Func<SqlDataReader, Type, List<DTO_BasicReport>> fillMethod)
        {
            this.DtoType = dtoType;
            this.Query = query;
            this.FillMethod = fillMethod;
        }

        public Type DtoType;

        public string Query = string.Empty;

        public Func<SqlDataReader, Type, List<DTO_BasicReport>> FillMethod;

    }
}
