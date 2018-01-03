using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.ADO
{
    public class DAL_drSolicitudDatosPersonales : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_drSolicitudDatosPersonales(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_drSolicitudDatosPersonales que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_drSolicitudDatosPersonales</returns>
        public List<DTO_drSolicitudDatosPersonales> DAL_drSolicitudDatosPersonales_GetAll()
        {
            try
            {
                List<DTO_drSolicitudDatosPersonales> result = new List<DTO_drSolicitudDatosPersonales>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT * FROM drSolicitudDatosPersonales with(nolock)";
                                       
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_drSolicitudDatosPersonales Datos;
                    Datos = new DTO_drSolicitudDatosPersonales(dr);                
                    result.Add(Datos);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosPersonales_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_drSolicitudDatosPersonales que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_drSolicitudDatosPersonales</returns>
        public List<DTO_drSolicitudDatosPersonales> DAL_drSolicitudDatosPersonales_GetByNumeroDoc(int numeroDoc, int version)
        {
            try
            {
                List<DTO_drSolicitudDatosPersonales> result = new List<DTO_drSolicitudDatosPersonales>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("Version", SqlDbType.TinyInt);
                mySqlCommand.Parameters["NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["Version"].Value = version;

                mySqlCommand.CommandText = "SELECT * FROM drSolicitudDatosPersonales with(nolock) where NumeroDoc = @NumeroDoc and Version = @Version ";
                //mySqlCommand.CommandText = "SELECT * FROM drSolicitudDatosPersonales with(nolock)";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_drSolicitudDatosPersonales datos = new DTO_drSolicitudDatosPersonales(dr);
                    result.Add(datos);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosPersonales_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla drSolicitudDatosPersonales
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public int DAL_drSolicitudDatosPersonales_Add(DTO_drSolicitudDatosPersonales Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO drSolicitudDatosPersonales " +
                    "( " +
                    "	NumeroDoc,Version,TipoPersona,TerceroID,TerceroDocTipoID,FechaExpDoc,CiudadExpDoc,FechaNacimiento,"+
                    "   ApellidoPri,ApellidoSdo,NombrePri,NombreSdo,EstadoCivil,ActEconomica1,ActEconomica2,"+
                    "   IngresosREG,IngresosSOP,NroInmuebles,AntCompra,AntUltimoMOV,HipotecasNro,RestriccionesNro,FolioMatricula,"+
                    "   FechaMatricula,UsuarioDigita,Fecha,Correo,CiudadResidencia,DataCreditoRecibeInd,DataCreditoRecibeFecha,DataCreditoRecibeUsuario,"+
                    "   PF_FincaRaiz,PF_FincaRaizDato,PF_EstadoActual,PF_MorasActuales,PF_MorasUltAno,PF_RepNegativos,PF_Estabilidad,PF_Ubicabilidad," +
                    "   PF_Probabilidad,PF_PorMaxFincaRaiz,PF_PorMaxEstadoActual,PF_PorMaxMorasActuales,PF_PorMaxMorasUltAno,PF_PorMaxRepNegativos,"+
                    "   PF_PorMaxEstabilidad,PF_PorMaxUbicabilidad,PF_PorMaxProbabilidad,PF_PorMaxFinancia,PF_IngresoEstimado,PF_RecAguayLuz,"+
                    "   PF_ConfirmaCel,PF_ConfirmaMail,PF_VigenciaFMI,PF_VigenciaConsData,PF_NumHipotecas,PF_NumBienes,PF_Restricciones,PF_AntCompra,"+
                    "   PF_UltAnotacion,PF_IngresosMin1,PF_IngresosMin2,PF_IngresosMin3,PF_IngresosMin4,PF_IngresosMin5,PF_IngresosMin6,PF_IngresosMin7,ListaClintonInd," +
                    "   PF_PorObligaciones,PF_PorUtilizaTDC,PF_PeorCalificacion,PF_Consultas6Meses,PF_MorasAct30,PF_MorasAct60,PF_MorasAct90,PF_MorasAct120,PF_MorasUlt30,"+
                    "   PF_MorasUlt60,PF_MorasUlt90,PF_MorasUlt120,PF_ObligacionCOB,PF_ObligacionDUD,PF_ObligacionCAS,PF_ObligacionEMB,PF_ObligacionREC,PF_ObligacionCAN,"+
                    "   PF_DireccDesde,PF_EntidadesNum,PF_CelularDesde,PF_CorreoDesde,PF_DireccionNum,PF_TelefonoNum,PF_CelularNum,PF_CorreoNum,PF_FactorAcierta,PF_AciertaResultado,"+
                    "   PF_PorObligacionesDato,PF_PorUtilizaTDCDato,PF_PeorCalificacionDato,PF_Consultas6MesesDato,PF_MorasAct30Dato,PF_MorasAct60Dato,PF_MorasAct90Dato,PF_MorasAct120Dato,"+
                    "   PF_MorasUlt30Dato,PF_MorasUlt60Dato,PF_MorasUlt90Dato,PF_MorasUlt120Dato,PF_ObligacionCOBDato,PF_ObligacionDUDDato,PF_ObligacionCASDato,PF_ObligacionEMBDato,"+
                    "   PF_ObligacionRECDato,PF_ObligacionCANDato,PF_DireccDesdeDato,PF_EntidadesNumDato,PF_CelularDesdeDato,PF_CorreoDesdeDato,PF_DireccDesdeMeses,PF_EntidadesNumMeses,"+
                    "   PF_CelularDesdeMeses,PF_CorreoDesdeMeses,PF_DireccionNumDato,PF_TelefonoNumDato,PF_CelularNumDato,PF_CorreoNumDato,PF_DireccionNumCant,PF_TelefonoNumCant," +
                    "   PF_CelularNumCant,PF_CorreoNumCant,"+
                    "   PF_CapacidadPago,PF_PorMaxFinDeuCon,PF_CapPagAdDeu,PF_CapPagAdCon,"+
                    "   PF_EstCtasVIV,PF_CtasTotVIV,PF_EstCtasBAN,PF_CtasTotBAN,PF_EstCtasFIN,PF_CtasTotFIN,PF_EstCtasCOP,PF_CtasTotCOP,PF_EstCtasTDC,PF_CtasTotTDC,PF_EstCtasREA,"+
                    "   PF_CtasTotREA,PF_EstCtasCEL,PF_CtasTotCEL,PF_CtasTotIngEst,PF_QuiantiMIN,PF_QuiantiMAX,PF_QuantoIngrEst,PF_IngrEstxQuanto,PF_FactIngresosREG,PF_IngrCapacPAG,"+
                    "   PF_PorIngrAporta,PF_ReqSopIngrInd,PF_PorIngrPagoCtas,PF_IngrDispPagoCtas,PF_CuotasACT,PF_IngrDispApoyos,FuenteIngresos1,FuenteIngresos2," +
                    "   IndTerceroID,IndApellidoPri,IndApellidoSdo,IndNombrePri,IndNombreSdo,IndTerceroDocTipoID,IndFechaExpDoc,IndFechaNacimiento,IndEstadoCivil,"+
                    "   IndActEconomica1,IndFuenteIngresos1,IndFuenteIngresos2,IndIngresosREG,IndIngresosSOP,IndCorreo,IndCiudadResidencia,IndNroInmuebles,"+
                    "   IndAntCompra,IndAntUltimoMOV,IndHipotecasNro,IndRestriccionesNro,IndFolioMatricula,IndFechaMatricula,GarantePrenda1Ind,GarantePrenda2Ind,GaranteHipoteca1Ind,GaranteHipoteca2Ind" +
                    ") " +
                    "VALUES " +
                    "( " +
                    "	@NumeroDoc,@Version,@TipoPersona,@TerceroID,@TerceroDocTipoID,@FechaExpDoc,@CiudadExpDoc,@FechaNacimiento," +
                    "   @ApellidoPri,@ApellidoSdo,@NombrePri,@NombreSdo,@EstadoCivil,@ActEconomica1,@ActEconomica2," +
                    "   @IngresosREG,@IngresosSOP,@NroInmuebles,@AntCompra,@AntUltimoMOV,@HipotecasNro,@RestriccionesNro,@FolioMatricula," +
                    "   @FechaMatricula,@UsuarioDigita,@Fecha,@Correo,@CiudadResidencia,@DataCreditoRecibeInd,@DataCreditoRecibeFecha,@DataCreditoRecibeUsuario," +
                    "   @PF_FincaRaiz,@PF_FincaRaizDato,@PF_EstadoActual,@PF_MorasActuales,@PF_MorasUltAno,@PF_RepNegativos,@PF_Estabilidad,@PF_Ubicabilidad," +
                    "   @PF_Probabilidad,@PF_PorMaxFincaRaiz,@PF_PorMaxEstadoActual,@PF_PorMaxMorasActuales,@PF_PorMaxMorasUltAno,@PF_PorMaxRepNegativos," +
                    "   @PF_PorMaxEstabilidad,@PF_PorMaxUbicabilidad,@PF_PorMaxProbabilidad,@PF_PorMaxFinancia,@PF_IngresoEstimado,@PF_RecAguayLuz," +
                    "   @PF_ConfirmaCel,@PF_ConfirmaMail,@PF_VigenciaFMI,@PF_VigenciaConsData,@PF_NumHipotecas,@PF_NumBienes,@PF_Restricciones,@PF_AntCompra," +
                    "   @PF_UltAnotacion,@PF_IngresosMin1,@PF_IngresosMin2,@PF_IngresosMin3,@PF_IngresosMin4,@PF_IngresosMin5,@PF_IngresosMin6,@PF_IngresosMin7,@ListaClintonInd," +
                    "   @PF_PorObligaciones,@PF_PorUtilizaTDC,@PF_PeorCalificacion,@PF_Consultas6Meses,@PF_MorasAct30,@PF_MorasAct60,@PF_MorasAct90,@PF_MorasAct120,@PF_MorasUlt30,"+
                    "   @PF_MorasUlt60,@PF_MorasUlt90,@PF_MorasUlt120,@PF_ObligacionCOB,@PF_ObligacionDUD,@PF_ObligacionCAS,@PF_ObligacionEMB,@PF_ObligacionREC,@PF_ObligacionCAN,"+
                    "   @PF_DireccDesde,@PF_EntidadesNum,@PF_CelularDesde,@PF_CorreoDesde,@PF_DireccionNum,@PF_TelefonoNum,@PF_CelularNum,@PF_CorreoNum,@PF_FactorAcierta,@PF_AciertaResultado,"+
                    "   @PF_PorObligacionesDato,@PF_PorUtilizaTDCDato,@PF_PeorCalificacionDato,@PF_Consultas6MesesDato,@PF_MorasAct30Dato,@PF_MorasAct60Dato,@PF_MorasAct90Dato,@PF_MorasAct120Dato," +
                    "   @PF_MorasUlt30Dato,@PF_MorasUlt60Dato,@PF_MorasUlt90Dato,@PF_MorasUlt120Dato,@PF_ObligacionCOBDato,@PF_ObligacionDUDDato,@PF_ObligacionCASDato,@PF_ObligacionEMBDato," +
                    "   @PF_ObligacionRECDato,@PF_ObligacionCANDato,@PF_DireccDesdeDato,@PF_EntidadesNumDato,@PF_CelularDesdeDato,@PF_CorreoDesdeDato,@PF_DireccDesdeMeses,@PF_EntidadesNumMeses," +
                    "   @PF_CelularDesdeMeses,@PF_CorreoDesdeMeses,@PF_DireccionNumDato,@PF_TelefonoNumDato,@PF_CelularNumDato,@PF_CorreoNumDato,@PF_DireccionNumCant,@PF_TelefonoNumCant," +
                    "   @PF_CelularNumCant,@PF_CorreoNumCant," +
                    "   @PF_CapacidadPago,@PF_PorMaxFinDeuCon,@PF_CapPagAdDeu,@PF_CapPagAdCon," +
                    "   @PF_EstCtasVIV,@PF_CtasTotVIV,@PF_EstCtasBAN,@PF_CtasTotBAN,@PF_EstCtasFIN,@PF_CtasTotFIN,@PF_EstCtasCOP,@PF_CtasTotCOP,@PF_EstCtasTDC,@PF_CtasTotTDC,@PF_EstCtasREA," +
                    "   @PF_CtasTotREA,@PF_EstCtasCEL,@PF_CtasTotCEL,@PF_CtasTotIngEst,@PF_QuiantiMIN,@PF_QuiantiMAX,@PF_QuantoIngrEst,@PF_IngrEstxQuanto,@PF_FactIngresosREG,@PF_IngrCapacPAG," +
                    "   @PF_PorIngrAporta,@PF_ReqSopIngrInd,@PF_PorIngrPagoCtas,@PF_IngrDispPagoCtas,@PF_CuotasACT,@PF_IngrDispApoyos,@FuenteIngresos1,@FuenteIngresos2," +
                    "   @IndTerceroID,@IndApellidoPri,@IndApellidoSdo,@IndNombrePri,@IndNombreSdo,@IndTerceroDocTipoID,@IndFechaExpDoc,@IndFechaNacimiento,@IndEstadoCivil,"+
                    "   @IndActEconomica1,@IndFuenteIngresos1,@IndFuenteIngresos2,@IndIngresosREG,@IndIngresosSOP,@IndCorreo,@IndCiudadResidencia,@IndNroInmuebles,@IndAntCompra,"+
                    "   @IndAntUltimoMOV,@IndHipotecasNro,@IndRestriccionesNro,@IndFolioMatricula,@IndFechaMatricula,@GarantePrenda1Ind,@GarantePrenda2Ind,@GaranteHipoteca1Ind,@GaranteHipoteca2Ind" +
                    ") SET @Consecutivo = SCOPE_IDENTITY()";
                #endregion
                #region Creacion de comandos                
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);              
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoPersona", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char,UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TerceroDocTipoID", SqlDbType.Char, UDT_TerceroTipoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaExpDoc", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CiudadExpDoc", SqlDbType.Char,UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaNacimiento", SqlDbType.SmallDateTime);                
                mySqlCommandSel.Parameters.Add("@ApellidoPri", SqlDbType.Char,UDT_DescripTBase.MaxLength);                
                mySqlCommandSel.Parameters.Add("@ApellidoSdo", SqlDbType.Char,UDT_DescripTBase.MaxLength);                                
                mySqlCommandSel.Parameters.Add("@NombrePri", SqlDbType.Char,UDT_DescripTBase.MaxLength);                                               
                mySqlCommandSel.Parameters.Add("@NombreSdo", SqlDbType.Char,UDT_DescripTBase.MaxLength);                                               
                mySqlCommandSel.Parameters.Add("@EstadoCivil", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ActEconomica1", SqlDbType.Char,UDT_ActEconomicaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ActEconomica2", SqlDbType.Char,UDT_ActEconomicaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IngresosREG", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@IngresosSOP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NroInmuebles", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AntCompra", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AntUltimoMOV", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@HipotecasNro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RestriccionesNro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FolioMatricula", SqlDbType.Char,10);
                mySqlCommandSel.Parameters.Add("@FechaMatricula", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@UsuarioDigita", SqlDbType.Char,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Correo", SqlDbType.Char, 60);
                mySqlCommandSel.Parameters.Add("@CiudadResidencia", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DataCreditoRecibeInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@DataCreditoRecibeFecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@DataCreditoRecibeUsuario", SqlDbType.Char,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PF_FincaRaiz", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_FincaRaizDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstadoActual", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasActuales", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUltAno", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_RepNegativos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_Estabilidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_Ubicabilidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_Probabilidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxFincaRaiz", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxEstadoActual", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxMorasActuales", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxMorasUltAno", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxRepNegativos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxEstabilidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxUbicabilidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxProbabilidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxFinancia", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngresoEstimado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_RecAguayLuz", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_ConfirmaCel", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_ConfirmaMail", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_VigenciaFMI", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PF_VigenciaConsData", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PF_NumHipotecas", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_NumBienes", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_Restricciones", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_AntCompra", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_UltAnotacion", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_IngresosMin1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngresosMin2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngresosMin3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngresosMin4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngresosMin5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngresosMin6", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngresosMin7", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ListaClintonInd", SqlDbType.Bit);
                
                mySqlCommandSel.Parameters.Add("@PF_PorObligaciones", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorUtilizaTDC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PeorCalificacion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_Consultas6Meses", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct30", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct60", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct90", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct120", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt30", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt60", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt90", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt120", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionCOB", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionDUD", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionCAS", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionEMB", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionREC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionCAN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_DireccDesde", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EntidadesNum", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CelularDesde", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CorreoDesde", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_DireccionNum", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_TelefonoNum", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CelularNum", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CorreoNum", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_FactorAcierta", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_AciertaResultado", SqlDbType.Decimal);

                /////
                mySqlCommandSel.Parameters.Add("@PF_PorObligacionesDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorUtilizaTDCDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PeorCalificacionDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_Consultas6MesesDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct30Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct60Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct90Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct120Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt30Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt60Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt90Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt120Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionCOBDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionDUDDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionCASDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionEMBDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionRECDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionCANDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_DireccDesdeDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EntidadesNumDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CelularDesdeDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CorreoDesdeDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_DireccDesdeMeses", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PF_EntidadesNumMeses", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PF_CelularDesdeMeses", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PF_CorreoDesdeMeses", SqlDbType.Int);

                mySqlCommandSel.Parameters.Add("@PF_DireccionNumDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_TelefonoNumDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CelularNumDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CorreoNumDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_DireccionNumCant", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_TelefonoNumCant", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_CelularNumCant", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_CorreoNumCant", SqlDbType.Bit);


                
                mySqlCommandSel.Parameters.Add("@PF_CapacidadPago", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxFinDeuCon", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CapPagAdDeu", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CapPagAdCon", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstCtasVIV", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotVIV", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstCtasBAN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotBAN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstCtasFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstCtasCOP", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotCOP", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstCtasTDC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotTDC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstCtasREA", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotREA", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstCtasCEL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotCEL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotIngEst", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_QuiantiMIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_QuiantiMAX", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_QuantoIngrEst", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngrEstxQuanto", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_FactIngresosREG", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngrCapacPAG", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorIngrAporta", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ReqSopIngrInd", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorIngrPagoCtas", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngrDispPagoCtas", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CuotasACT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngrDispApoyos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FuenteIngresos1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FuenteIngresos2", SqlDbType.TinyInt);

                mySqlCommandSel.Parameters.Add("@IndTerceroID", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndApellidoPri", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndApellidoSdo", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndNombrePri", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndNombreSdo", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndTerceroDocTipoID", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndFechaExpDoc", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndFechaNacimiento", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndEstadoCivil", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndActEconomica1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndFuenteIngresos1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndFuenteIngresos2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndIngresosREG", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndIngresosSOP", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndCorreo", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndCiudadResidencia", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndNroInmuebles", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndAntCompra", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndAntUltimoMOV", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndHipotecasNro", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndRestriccionesNro", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndFolioMatricula", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndFechaMatricula", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@GarantePrenda1Ind", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@GarantePrenda2Ind", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@GaranteHipoteca1Ind", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@GaranteHipoteca2Ind", SqlDbType.Bit);
                #endregion
                #region Asigna los valores                
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                mySqlCommandSel.Parameters["@TipoPersona"].Value = Datos.TipoPersona.Value;       
                mySqlCommandSel.Parameters["@TerceroID"].Value = Datos.TerceroID.Value;
                mySqlCommandSel.Parameters["@TerceroDocTipoID"].Value = Datos.TerceroDocTipoID.Value;
                mySqlCommandSel.Parameters["@FechaExpDoc"].Value = Datos.FechaExpDoc.Value;
                mySqlCommandSel.Parameters["@CiudadExpDoc"].Value = Datos.CiudadExpDoc.Value;        
                mySqlCommandSel.Parameters["@FechaNacimiento"].Value = Datos.FechaNacimiento.Value;                             
                mySqlCommandSel.Parameters["@FechaNacimiento"].Value = Datos.FechaNacimiento.Value;                                
                mySqlCommandSel.Parameters["@ApellidoPri"].Value = Datos.ApellidoPri.Value;
                mySqlCommandSel.Parameters["@ApellidoSdo"].Value = Datos.ApellidoSdo.Value;
                mySqlCommandSel.Parameters["@NombrePri"].Value = Datos.NombrePri.Value;
                mySqlCommandSel.Parameters["@NombreSdo"].Value = Datos.NombreSdo.Value;
                mySqlCommandSel.Parameters["@EstadoCivil"].Value = Datos.EstadoCivil.Value;
                mySqlCommandSel.Parameters["@ActEconomica1"].Value = Datos.ActEconomica1.Value;
                mySqlCommandSel.Parameters["@ActEconomica2"].Value = Datos.ActEconomica2.Value;
                mySqlCommandSel.Parameters["@IngresosREG"].Value = Datos.IngresosREG.Value;
                mySqlCommandSel.Parameters["@IngresosSOP"].Value = Datos.IngresosSOP.Value;
                mySqlCommandSel.Parameters["@NroInmuebles"].Value = Datos.NroInmuebles.Value;
                mySqlCommandSel.Parameters["@AntCompra"].Value = Datos.AntCompra.Value;
                mySqlCommandSel.Parameters["@AntUltimoMOV"].Value = Datos.AntUltimoMOV.Value;
                mySqlCommandSel.Parameters["@HipotecasNro"].Value = Datos.HipotecasNro.Value;
                mySqlCommandSel.Parameters["@RestriccionesNro"].Value = Datos.RestriccionesNro.Value;
                mySqlCommandSel.Parameters["@FolioMatricula"].Value = Datos.FolioMatricula.Value;
                mySqlCommandSel.Parameters["@FechaMatricula"].Value = Datos.FechaMatricula.Value;                 
                mySqlCommandSel.Parameters["@UsuarioDigita"].Value = Datos.UsuarioDigita.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = DateTime.Now.Date;
                mySqlCommandSel.Parameters["@Correo"].Value = Datos.Correo.Value;
                mySqlCommandSel.Parameters["@CiudadResidencia"].Value = Datos.CiudadResidencia.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                mySqlCommandSel.Parameters["@DataCreditoRecibeInd"].Value = Datos.DataCreditoRecibeInd.Value;
                mySqlCommandSel.Parameters["@DataCreditoRecibeFecha"].Value = Datos.DataCreditoRecibeFecha.Value;
                mySqlCommandSel.Parameters["@DataCreditoRecibeUsuario"].Value = Datos.DataCreditoRecibeUsuario.Value;
                mySqlCommandSel.Parameters["@PF_FincaRaiz"].Value = Datos.PF_FincaRaiz.Value;
                mySqlCommandSel.Parameters["@PF_FincaRaizDato"].Value = Datos.PF_FincaRaizDato.Value;
                mySqlCommandSel.Parameters["@PF_EstadoActual"].Value = Datos.PF_EstadoActual.Value;
                mySqlCommandSel.Parameters["@PF_MorasActuales"].Value = Datos.PF_MorasActuales.Value;
                mySqlCommandSel.Parameters["@PF_MorasUltAno"].Value = Datos.PF_MorasUltAno.Value;
                mySqlCommandSel.Parameters["@PF_RepNegativos"].Value = Datos.PF_RepNegativos.Value;
                mySqlCommandSel.Parameters["@PF_Estabilidad"].Value = Datos.PF_Estabilidad.Value;
                mySqlCommandSel.Parameters["@PF_Ubicabilidad"].Value = Datos.PF_Ubicabilidad.Value;
                mySqlCommandSel.Parameters["@PF_Probabilidad"].Value = Datos.PF_Probabilidad.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxFincaRaiz"].Value = Datos.PF_PorMaxFincaRaiz.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxEstadoActual"].Value = Datos.PF_PorMaxEstadoActual.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxMorasActuales"].Value = Datos.PF_PorMaxMorasActuales.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxMorasUltAno"].Value = Datos.PF_PorMaxMorasUltAno.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxRepNegativos"].Value = Datos.PF_PorMaxRepNegativos.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxEstabilidad"].Value = Datos.PF_PorMaxEstabilidad.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxUbicabilidad"].Value = Datos.PF_PorMaxUbicabilidad.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxProbabilidad"].Value = Datos.PF_PorMaxProbabilidad.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxFinancia"].Value = Datos.PF_PorMaxFinancia.Value;
                mySqlCommandSel.Parameters["@PF_IngresoEstimado"].Value = Datos.PF_IngresoEstimado.Value;
                mySqlCommandSel.Parameters["@PF_RecAguayLuz"].Value = Datos.PF_RecAguayLuz.Value;
                mySqlCommandSel.Parameters["@PF_ConfirmaCel"].Value = Datos.PF_ConfirmaCel.Value;
                mySqlCommandSel.Parameters["@PF_ConfirmaMail"].Value = Datos.PF_ConfirmaMail.Value;
                mySqlCommandSel.Parameters["@PF_VigenciaFMI"].Value = Datos.PF_VigenciaFMI.Value;
                mySqlCommandSel.Parameters["@PF_VigenciaConsData"].Value = Datos.PF_VigenciaConsData.Value;
                mySqlCommandSel.Parameters["@PF_NumHipotecas"].Value = Datos.PF_NumHipotecas.Value;
                mySqlCommandSel.Parameters["@PF_NumBienes"].Value = Datos.PF_NumBienes.Value;
                mySqlCommandSel.Parameters["@PF_Restricciones"].Value = Datos.PF_Restricciones.Value;
                mySqlCommandSel.Parameters["@PF_AntCompra"].Value = Datos.PF_AntCompra.Value;
                mySqlCommandSel.Parameters["@PF_UltAnotacion"].Value = Datos.PF_UltAnotacion.Value;
                mySqlCommandSel.Parameters["@PF_IngresosMin1"].Value = Datos.PF_IngresosMin1.Value;
                mySqlCommandSel.Parameters["@PF_IngresosMin2"].Value = Datos.PF_IngresosMin2.Value;
                mySqlCommandSel.Parameters["@PF_IngresosMin3"].Value = Datos.PF_IngresosMin3.Value;
                mySqlCommandSel.Parameters["@PF_IngresosMin4"].Value = Datos.PF_IngresosMin4.Value;
                mySqlCommandSel.Parameters["@PF_IngresosMin5"].Value = Datos.PF_IngresosMin5.Value;
                mySqlCommandSel.Parameters["@PF_IngresosMin6"].Value = Datos.PF_IngresosMin6.Value;
                mySqlCommandSel.Parameters["@PF_IngresosMin7"].Value = Datos.PF_IngresosMin7.Value;
                mySqlCommandSel.Parameters["@ListaClintonInd"].Value = Datos.ListaClintonInd.Value;

                mySqlCommandSel.Parameters["@PF_PorObligaciones"].Value = Datos.PF_PorObligaciones.Value;
                mySqlCommandSel.Parameters["@PF_PorUtilizaTDC"].Value = Datos.PF_PorUtilizaTDC.Value;
                mySqlCommandSel.Parameters["@PF_PeorCalificacion"].Value = Datos.PF_PeorCalificacion.Value;
                mySqlCommandSel.Parameters["@PF_Consultas6Meses"].Value = Datos.PF_Consultas6Meses.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct30"].Value = Datos.PF_MorasAct30.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct60"].Value = Datos.PF_MorasAct60.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct90"].Value = Datos.PF_MorasAct90.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct120"].Value = Datos.PF_MorasAct120.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt30"].Value = Datos.PF_MorasUlt30.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt60"].Value = Datos.PF_MorasUlt60.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt90"].Value = Datos.PF_MorasUlt90.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt120"].Value = Datos.PF_MorasUlt120.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionCOB"].Value = Datos.PF_ObligacionCOB.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionDUD"].Value = Datos.PF_ObligacionDUD.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionCAS"].Value = Datos.PF_ObligacionCAS.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionEMB"].Value = Datos.PF_ObligacionEMB.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionREC"].Value = Datos.PF_ObligacionREC.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionCAN"].Value = Datos.PF_ObligacionCAN.Value;
                mySqlCommandSel.Parameters["@PF_DireccDesde"].Value = Datos.PF_DireccDesde.Value;
                mySqlCommandSel.Parameters["@PF_EntidadesNum"].Value = Datos.PF_EntidadesNum.Value;
                mySqlCommandSel.Parameters["@PF_CelularDesde"].Value = Datos.PF_CelularDesde.Value;
                mySqlCommandSel.Parameters["@PF_CorreoDesde"].Value = Datos.PF_CorreoDesde.Value;
                mySqlCommandSel.Parameters["@PF_DireccionNum"].Value = Datos.PF_DireccionNum.Value;
                mySqlCommandSel.Parameters["@PF_TelefonoNum"].Value = Datos.PF_TelefonoNum.Value;
                mySqlCommandSel.Parameters["@PF_CelularNum"].Value = Datos.PF_CelularNum.Value;
                mySqlCommandSel.Parameters["@PF_CorreoNum"].Value = Datos.PF_CorreoNum.Value;
                mySqlCommandSel.Parameters["@PF_FactorAcierta"].Value = Datos.PF_FactorAcierta.Value;
                mySqlCommandSel.Parameters["@PF_AciertaResultado"].Value = Datos.PF_AciertaResultado.Value;
                /////
                mySqlCommandSel.Parameters["@PF_PorObligacionesDato"].Value = Datos.PF_PorObligacionesDato.Value;
                mySqlCommandSel.Parameters["@PF_PorUtilizaTDCDato"].Value = Datos.PF_PorUtilizaTDCDato.Value;
                mySqlCommandSel.Parameters["@PF_PeorCalificacionDato"].Value = Datos.PF_PeorCalificacionDato.Value;
                mySqlCommandSel.Parameters["@PF_Consultas6MesesDato"].Value = Datos.PF_Consultas6MesesDato.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct30Dato"].Value = Datos.PF_MorasAct30Dato.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct60Dato"].Value = Datos.PF_MorasAct60Dato.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct90Dato"].Value = Datos.PF_MorasAct90Dato.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct120Dato"].Value = Datos.PF_MorasAct120Dato.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt30Dato"].Value = Datos.PF_MorasUlt30Dato.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt60Dato"].Value = Datos.PF_MorasUlt60Dato.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt90Dato"].Value = Datos.PF_MorasUlt90Dato.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt120Dato"].Value = Datos.PF_MorasUlt120Dato.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionCOBDato"].Value = Datos.PF_ObligacionCOBDato.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionDUDDato"].Value = Datos.PF_ObligacionDUDDato.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionCASDato"].Value = Datos.PF_ObligacionCASDato.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionEMBDato"].Value = Datos.PF_ObligacionEMBDato.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionRECDato"].Value = Datos.PF_ObligacionRECDato.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionCANDato"].Value = Datos.PF_ObligacionCANDato.Value;
                mySqlCommandSel.Parameters["@PF_DireccDesdeDato"].Value = Datos.PF_DireccDesdeDato.Value;
                mySqlCommandSel.Parameters["@PF_EntidadesNumDato"].Value = Datos.PF_EntidadesNumDato.Value;
                mySqlCommandSel.Parameters["@PF_CelularDesdeDato"].Value = Datos.PF_CelularDesdeDato.Value;
                mySqlCommandSel.Parameters["@PF_CorreoDesdeDato"].Value = Datos.PF_CorreoDesdeDato.Value;
                mySqlCommandSel.Parameters["@PF_DireccDesdeMeses"].Value = Datos.PF_DireccDesdeMeses.Value;
                mySqlCommandSel.Parameters["@PF_EntidadesNumMeses"].Value = Datos.PF_EntidadesNumMeses.Value;
                mySqlCommandSel.Parameters["@PF_CelularDesdeMeses"].Value = Datos.PF_CelularDesdeMeses.Value;
                mySqlCommandSel.Parameters["@PF_CorreoDesdeMeses"].Value = Datos.PF_CorreoDesdeMeses.Value;
                mySqlCommandSel.Parameters["@PF_DireccionNumDato"].Value = Datos.PF_DireccionNumDato.Value;
                mySqlCommandSel.Parameters["@PF_TelefonoNumDato"].Value = Datos.PF_TelefonoNumDato.Value;
                mySqlCommandSel.Parameters["@PF_CelularNumDato"].Value = Datos.PF_CelularNumDato.Value;
                mySqlCommandSel.Parameters["@PF_CorreoNumDato"].Value = Datos.PF_CorreoNumDato.Value;
                mySqlCommandSel.Parameters["@PF_DireccionNumCant"].Value = Datos.PF_DireccionNumCant.Value;
                mySqlCommandSel.Parameters["@PF_TelefonoNumCant"].Value = Datos.PF_TelefonoNumCant.Value;
                mySqlCommandSel.Parameters["@PF_CelularNumCant"].Value = Datos.PF_CelularNumCant.Value;
                mySqlCommandSel.Parameters["@PF_CorreoNumCant"].Value = Datos.PF_CorreoNumCant.Value;


                mySqlCommandSel.Parameters["@PF_CapacidadPago"].Value = Datos.PF_CapacidadPago.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxFinDeuCon"].Value = Datos.PF_PorMaxFinDeuCon.Value;
                mySqlCommandSel.Parameters["@PF_CapPagAdDeu"].Value = Datos.PF_CapPagAdDeu.Value;
                mySqlCommandSel.Parameters["@PF_CapPagAdCon"].Value = Datos.PF_CapPagAdCon.Value;
                mySqlCommandSel.Parameters["@PF_EstCtasVIV"].Value = Datos.PF_EstCtasVIV.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotVIV"].Value = Datos.PF_CtasTotVIV.Value;
                mySqlCommandSel.Parameters["@PF_EstCtasBAN"].Value = Datos.PF_EstCtasBAN.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotBAN"].Value = Datos.PF_CtasTotBAN.Value;
                mySqlCommandSel.Parameters["@PF_EstCtasFIN"].Value = Datos.PF_EstCtasFIN.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotFIN"].Value = Datos.PF_CtasTotFIN.Value;
                mySqlCommandSel.Parameters["@PF_EstCtasCOP"].Value = Datos.PF_EstCtasCOP.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotCOP"].Value = Datos.PF_CtasTotCOP.Value;
                mySqlCommandSel.Parameters["@PF_EstCtasTDC"].Value = Datos.PF_EstCtasTDC.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotTDC"].Value = Datos.PF_CtasTotTDC.Value;
                mySqlCommandSel.Parameters["@PF_EstCtasREA"].Value = Datos.PF_EstCtasREA.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotREA"].Value = Datos.PF_CtasTotREA.Value;
                mySqlCommandSel.Parameters["@PF_EstCtasCEL"].Value = Datos.PF_EstCtasCEL.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotCEL"].Value = Datos.PF_CtasTotCEL.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotIngEst"].Value = Datos.PF_CtasTotIngEst.Value;
                mySqlCommandSel.Parameters["@PF_QuiantiMIN"].Value = Datos.PF_QuiantiMIN.Value;
                mySqlCommandSel.Parameters["@PF_QuiantiMAX"].Value = Datos.PF_QuiantiMAX.Value;
                mySqlCommandSel.Parameters["@PF_QuantoIngrEst"].Value = Datos.PF_QuantoIngrEst.Value;
                mySqlCommandSel.Parameters["@PF_IngrEstxQuanto"].Value = Datos.PF_IngrEstxQuanto.Value;
                mySqlCommandSel.Parameters["@PF_FactIngresosREG"].Value = Datos.PF_FactIngresosREG.Value;
                mySqlCommandSel.Parameters["@PF_IngrCapacPAG"].Value = Datos.PF_IngrCapacPAG.Value;
                mySqlCommandSel.Parameters["@PF_PorIngrAporta"].Value = Datos.PF_PorIngrAporta.Value;
                mySqlCommandSel.Parameters["@PF_ReqSopIngrInd"].Value = Datos.PF_ReqSopIngrInd.Value;
                mySqlCommandSel.Parameters["@PF_PorIngrPagoCtas"].Value = Datos.PF_PorIngrPagoCtas.Value;
                mySqlCommandSel.Parameters["@PF_IngrDispPagoCtas"].Value = Datos.PF_IngrDispPagoCtas.Value;
                mySqlCommandSel.Parameters["@PF_CuotasACT"].Value = Datos.PF_CuotasACT.Value;
                mySqlCommandSel.Parameters["@PF_IngrDispApoyos"].Value = Datos.PF_IngrDispApoyos.Value;

                mySqlCommandSel.Parameters["@FuenteIngresos1"].Value = Datos.FuenteIngresos1.Value;
                mySqlCommandSel.Parameters["@FuenteIngresos2"].Value = Datos.FuenteIngresos2.Value;

                mySqlCommandSel.Parameters["@IndTerceroID"].Value = Datos.IndTerceroID.Value;
                mySqlCommandSel.Parameters["@IndApellidoPri"].Value = Datos.IndApellidoPri.Value;
                mySqlCommandSel.Parameters["@IndApellidoSdo"].Value = Datos.IndApellidoSdo.Value;
                mySqlCommandSel.Parameters["@IndNombrePri"].Value = Datos.IndNombrePri.Value;
                mySqlCommandSel.Parameters["@IndNombreSdo"].Value = Datos.IndNombreSdo.Value;
                mySqlCommandSel.Parameters["@IndTerceroDocTipoID"].Value = Datos.IndTerceroDocTipoID.Value;
                mySqlCommandSel.Parameters["@IndFechaExpDoc"].Value = Datos.IndFechaExpDoc.Value;
                mySqlCommandSel.Parameters["@IndFechaNacimiento"].Value = Datos.IndFechaNacimiento.Value;
                mySqlCommandSel.Parameters["@IndEstadoCivil"].Value = Datos.IndEstadoCivil.Value;
                mySqlCommandSel.Parameters["@IndActEconomica1"].Value = Datos.IndActEconomica1.Value;
                mySqlCommandSel.Parameters["@IndFuenteIngresos1"].Value = Datos.IndFuenteIngresos1.Value;
                mySqlCommandSel.Parameters["@IndFuenteIngresos2"].Value = Datos.IndFuenteIngresos2.Value;
                mySqlCommandSel.Parameters["@IndIngresosREG"].Value = Datos.IndIngresosREG.Value;
                mySqlCommandSel.Parameters["@IndIngresosSOP"].Value = Datos.IndIngresosSOP.Value;
                mySqlCommandSel.Parameters["@IndCorreo"].Value = Datos.IndCorreo.Value;
                mySqlCommandSel.Parameters["@IndCiudadResidencia"].Value = Datos.IndCiudadResidencia.Value;
                mySqlCommandSel.Parameters["@IndNroInmuebles"].Value = Datos.IndNroInmuebles.Value;
                mySqlCommandSel.Parameters["@IndAntCompra"].Value = Datos.IndAntCompra.Value;
                mySqlCommandSel.Parameters["@IndAntUltimoMOV"].Value = Datos.IndAntUltimoMOV.Value;
                mySqlCommandSel.Parameters["@IndHipotecasNro"].Value = Datos.IndHipotecasNro.Value;
                mySqlCommandSel.Parameters["@IndRestriccionesNro"].Value = Datos.IndRestriccionesNro.Value;
                mySqlCommandSel.Parameters["@IndFolioMatricula"].Value = Datos.IndFolioMatricula.Value;
                mySqlCommandSel.Parameters["@IndFechaMatricula"].Value = Datos.IndFechaMatricula.Value;
                mySqlCommandSel.Parameters["@GarantePrenda1Ind"].Value = Datos.GarantePrenda1Ind.Value;
                mySqlCommandSel.Parameters["@GarantePrenda2Ind"].Value = Datos.GarantePrenda2Ind.Value;
                mySqlCommandSel.Parameters["@GaranteHipoteca1Ind"].Value = Datos.GaranteHipoteca1Ind.Value;
                mySqlCommandSel.Parameters["@GaranteHipoteca2Ind"].Value = Datos.GaranteHipoteca2Ind.Value;
                
                //mySqlCommandSel.Parameters["@eg_glZona"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glZona, this.Empresa, egCtrl);

                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                mySqlCommandSel.ExecuteNonQuery();
                int consec = Convert.ToInt32(mySqlCommandSel.Parameters["@Consecutivo"].Value);
                return consec;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosPersonales_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla drSolicitudDatosPersonales
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_drSolicitudDatosPersonales_Update(DTO_drSolicitudDatosPersonales Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =                 
                                           "UPDATE drSolicitudDatosPersonales SET" +
                                               " NumeroDoc = @NumeroDoc " +
                                               " ,Version = @Version " +
                                               " ,TipoPersona = @TipoPersona" +
                                               " ,TerceroID = @TerceroID" +
                                               " ,TerceroDocTipoID= @TerceroDocTipoID" +
                                               " ,FechaExpDoc = @FechaExpDoc" +
                                               " ,CiudadExpDoc = @CiudadExpDoc" +
                                               " ,FechaNacimiento = @FechaNacimiento" +                                               
                                               " ,ApellidoPri= @ApellidoPri" +
                                               " ,ApellidoSdo= @ApellidoSdo" +                                              
                                               " ,NombrePri = @NombrePri" +
                                               " ,NombreSdo = @NombreSdo" +
                                               " ,EstadoCivil = @EstadoCivil" +
                                               " ,ActEconomica1 = @ActEconomica1" +
                                               " ,ActEconomica2 = @ActEconomica2" +
                                               " ,IngresosREG = @IngresosREG" +
                                               " ,IngresosSOP= @IngresosSOP" +
                                               " ,NroInmuebles= @NroInmuebles" +
                                               " ,AntCompra= @AntCompra" +
                                               " ,AntUltimoMOV= @AntUltimoMOV" +
                                               " ,HipotecasNro= @HipotecasNro" +
                                               " ,RestriccionesNro = @RestriccionesNro" +
                                               " ,FolioMatricula= @FolioMatricula" +
                                               " ,FechaMatricula= @FechaMatricula" +
                                               " ,UsuarioDigita= @UsuarioDigita" +
                                               " ,Fecha= @Fecha" +
                                               ",Correo=@Correo" +
                                               ",CiudadResidencia=@CiudadResidencia" +
                                               ",DataCreditoRecibeInd=@DataCreditoRecibeInd" +
                                               ",DataCreditoRecibeFecha=@DataCreditoRecibeFecha" +
                                               ",DataCreditoRecibeUsuario=@DataCreditoRecibeUsuario" +
                                               ",PF_FincaRaiz=@PF_FincaRaiz" +
                                               ",PF_FincaRaizDato=@PF_FincaRaizDato" +
                                               ",PF_EstadoActual=@PF_EstadoActual" +
                                               ",PF_MorasActuales=@PF_MorasActuales" +
                                               ",PF_MorasUltAno=@PF_MorasUltAno" +
                                               ",PF_RepNegativos=@PF_RepNegativos" +
                                               ",PF_Estabilidad=@PF_Estabilidad" +
                                               ",PF_Ubicabilidad=@PF_Ubicabilidad" +
                                               ",PF_Probabilidad=@PF_Probabilidad" +
                                               ",PF_PorMaxFincaRaiz=@PF_PorMaxFincaRaiz" +
                                               ",PF_PorMaxEstadoActual=@PF_PorMaxEstadoActual" +
                                               ",PF_PorMaxMorasActuales=@PF_PorMaxMorasActuales" +
                                               ",PF_PorMaxMorasUltAno=@PF_PorMaxMorasUltAno" +
                                               ",PF_PorMaxRepNegativos=@PF_PorMaxRepNegativos" +
                                               ",PF_PorMaxEstabilidad=@PF_PorMaxEstabilidad" +
                                               ",PF_PorMaxUbicabilidad=@PF_PorMaxUbicabilidad" +
                                               ",PF_PorMaxProbabilidad=@PF_PorMaxProbabilidad" +
                                               ",PF_PorMaxFinancia=@PF_PorMaxFinancia" +
                                               ",PF_IngresoEstimado=@PF_IngresoEstimado" +
                                               ",PF_RecAguayLuz=@PF_RecAguayLuz" +
                                               ",PF_ConfirmaCel=@PF_ConfirmaCel" +
                                               ",PF_ConfirmaMail=@PF_ConfirmaMail" +
                                               ",PF_VigenciaFMI=@PF_VigenciaFMI" +
                                               ",PF_VigenciaConsData=@PF_VigenciaConsData" +
                                               ",PF_NumHipotecas=@PF_NumHipotecas" +
                                               ",PF_NumBienes=@PF_NumBienes" +
                                               ",PF_Restricciones=@PF_Restricciones" +
                                               ",PF_AntCompra=@PF_AntCompra" +
                                               ",PF_UltAnotacion=@PF_UltAnotacion" +
                                               ",PF_IngresosMin1=@PF_IngresosMin1" +
                                               ",PF_IngresosMin2=@PF_IngresosMin2" +
                                               ",PF_IngresosMin3=@PF_IngresosMin3" +
                                               ",PF_IngresosMin4=@PF_IngresosMin4" +
                                               ",PF_IngresosMin5=@PF_IngresosMin5" +
                                               ",PF_IngresosMin6=@PF_IngresosMin6" +
                                               ",PF_IngresosMin7=@PF_IngresosMin7" +
                                               ",ListaClintonInd=@ListaClintonInd"+
                                               ",PF_PorObligaciones=@PF_PorObligaciones" +
                                                ",PF_PorUtilizaTDC=@PF_PorUtilizaTDC" +
                                                ",PF_PeorCalificacion=@PF_PeorCalificacion" +
                                                ",PF_Consultas6Meses=@PF_Consultas6Meses" +
                                                ",PF_MorasAct30=@PF_MorasAct30" +
                                                ",PF_MorasAct60=@PF_MorasAct60" +
                                                ",PF_MorasAct90=@PF_MorasAct90" +
                                                ",PF_MorasAct120=@PF_MorasAct120" +
                                                ",PF_MorasUlt30=@PF_MorasUlt30" +
                                                ",PF_MorasUlt60=@PF_MorasUlt60" +
                                                ",PF_MorasUlt90=@PF_MorasUlt90" +
                                                ",PF_MorasUlt120=@PF_MorasUlt120" +
                                                ",PF_ObligacionCOB=@PF_ObligacionCOB" +
                                                ",PF_ObligacionDUD=@PF_ObligacionDUD" +
                                                ",PF_ObligacionCAS=@PF_ObligacionCAS" +
                                                ",PF_ObligacionEMB=@PF_ObligacionEMB" +
                                                ",PF_ObligacionREC=@PF_ObligacionREC" +
                                                ",PF_ObligacionCAN=@PF_ObligacionCAN" +
                                                ",PF_DireccDesde=@PF_DireccDesde" +
                                                ",PF_EntidadesNum=@PF_EntidadesNum" +
                                                ",PF_CelularDesde=@PF_CelularDesde" +
                                                ",PF_CorreoDesde=@PF_CorreoDesde" +
                                                ",PF_DireccionNum=@PF_DireccionNum" +
                                                ",PF_TelefonoNum=@PF_TelefonoNum" +
                                                ",PF_CelularNum=@PF_CelularNum" +
                                                ",PF_CorreoNum=@PF_CorreoNum" +
                                                ",PF_FactorAcierta=@PF_FactorAcierta" +
                                                ",PF_AciertaResultado=@PF_AciertaResultado" +
                                                ",PF_PorObligacionesDato=@PF_PorObligacionesDato" +
                                                ",PF_PorUtilizaTDCDato=@PF_PorUtilizaTDCDato" +
                                                ",PF_PeorCalificacionDato=@PF_PeorCalificacionDato" +
                                                ",PF_Consultas6MesesDato=@PF_Consultas6MesesDato" +
                                                ",PF_MorasAct30Dato=@PF_MorasAct30Dato" +
                                                ",PF_MorasAct60Dato=@PF_MorasAct60Dato" +
                                                ",PF_MorasAct90Dato=@PF_MorasAct90Dato" +
                                                ",PF_MorasAct120Dato=@PF_MorasAct120Dato" +
                                                ",PF_MorasUlt30Dato=@PF_MorasUlt30Dato" +
                                                ",PF_MorasUlt60Dato=@PF_MorasUlt60Dato" +
                                                ",PF_MorasUlt90Dato=@PF_MorasUlt90Dato" +
                                                ",PF_MorasUlt120Dato=@PF_MorasUlt120Dato" +
                                                ",PF_ObligacionCOBDato=@PF_ObligacionCOBDato" +
                                                ",PF_ObligacionDUDDato=@PF_ObligacionDUDDato" +
                                                ",PF_ObligacionCASDato=@PF_ObligacionCASDato" +
                                                ",PF_ObligacionEMBDato=@PF_ObligacionEMBDato" +
                                                ",PF_ObligacionRECDato=@PF_ObligacionRECDato" +
                                                ",PF_ObligacionCANDato=@PF_ObligacionCANDato" +
                                                ",PF_DireccDesdeDato=@PF_DireccDesdeDato" +
                                                ",PF_EntidadesNumDato=@PF_EntidadesNumDato" +
                                                ",PF_CelularDesdeDato=@PF_CelularDesdeDato" +
                                                ",PF_CorreoDesdeDato=@PF_CorreoDesdeDato" +
                                                ",PF_DireccDesdeMeses=@PF_DireccDesdeMeses" +
                                                ",PF_EntidadesNumMeses=@PF_EntidadesNumMeses" +
                                                ",PF_CelularDesdeMeses=@PF_CelularDesdeMeses" +
                                                ",PF_CorreoDesdeMeses=@PF_CorreoDesdeMeses" +
                                                ",PF_DireccionNumDato=@PF_DireccionNumDato" +
                                                ",PF_TelefonoNumDato=@PF_TelefonoNumDato" +
                                                ",PF_CelularNumDato=@PF_CelularNumDato" +
                                                ",PF_CorreoNumDato=@PF_CorreoNumDato" +
                                                ",PF_DireccionNumCant=@PF_DireccionNumCant" +
                                                ",PF_TelefonoNumCant=@PF_TelefonoNumCant" +
                                                ",PF_CelularNumCant=@PF_CelularNumCant" +
                                                ",PF_CorreoNumCant=@PF_CorreoNumCant" +
                                                ",PF_CapacidadPago=@PF_CapacidadPago" +
                                                ",PF_PorMaxFinDeuCon=@PF_PorMaxFinDeuCon" +
                                                ",PF_CapPagAdDeu=@PF_CapPagAdDeu" +
                                                ",PF_CapPagAdCon=@PF_CapPagAdCon" +
                                                ",PF_EstCtasVIV=@PF_EstCtasVIV" +
                                                ",PF_CtasTotVIV=@PF_CtasTotVIV" +
                                                ",PF_EstCtasBAN=@PF_EstCtasBAN" +
                                                ",PF_CtasTotBAN=@PF_CtasTotBAN" +
                                                ",PF_EstCtasFIN=@PF_EstCtasFIN" +
                                                ",PF_CtasTotFIN=@PF_CtasTotFIN" +
                                                ",PF_EstCtasCOP=@PF_EstCtasCOP" +
                                                ",PF_CtasTotCOP=@PF_CtasTotCOP" +
                                                ",PF_EstCtasTDC=@PF_EstCtasTDC" +
                                                ",PF_CtasTotTDC=@PF_CtasTotTDC" +
                                                ",PF_EstCtasREA=@PF_EstCtasREA" +
                                                ",PF_CtasTotREA=@PF_CtasTotREA" +
                                                ",PF_EstCtasCEL=@PF_EstCtasCEL" +
                                                ",PF_CtasTotCEL=@PF_CtasTotCEL" +
                                                ",PF_CtasTotIngEst=@PF_CtasTotIngEst" +
                                                ",PF_QuiantiMIN=@PF_QuiantiMIN" +
                                                ",PF_QuiantiMAX=@PF_QuiantiMAX" +
                                                ",PF_QuantoIngrEst=@PF_QuantoIngrEst" +
                                                ",PF_IngrEstxQuanto=@PF_IngrEstxQuanto" +
                                                ",PF_FactIngresosREG=@PF_FactIngresosREG" +
                                                ",PF_IngrCapacPAG=@PF_IngrCapacPAG" +
                                                ",PF_PorIngrAporta=@PF_PorIngrAporta" +
                                                ",PF_ReqSopIngrInd=@PF_ReqSopIngrInd" +
                                                ",PF_PorIngrPagoCtas=@PF_PorIngrPagoCtas" +
                                                ",PF_IngrDispPagoCtas=@PF_IngrDispPagoCtas" +
                                                ",PF_CuotasACT=@PF_CuotasACT" +
                                                ",PF_IngrDispApoyos=@PF_IngrDispApoyos" +
                                                ",FuenteIngresos1=@FuenteIngresos1" +
                                                ",FuenteIngresos2=@FuenteIngresos2" +
                                                ",IndTerceroID=@IndTerceroID" +
                                                ",IndApellidoPri=@IndApellidoPri" +
                                                ",IndApellidoSdo=@IndApellidoSdo" +
                                                ",IndNombrePri=@IndNombrePri" +
                                                ",IndNombreSdo=@IndNombreSdo" +
                                                ",IndTerceroDocTipoID=@IndTerceroDocTipoID" +
                                                ",IndFechaExpDoc=@IndFechaExpDoc" +
                                                ",IndFechaNacimiento=@IndFechaNacimiento" +
                                                ",IndEstadoCivil=@IndEstadoCivil" +
                                                ",IndActEconomica1=@IndActEconomica1" +
                                                ",IndFuenteIngresos1=@IndFuenteIngresos1" +
                                                ",IndFuenteIngresos2=@IndFuenteIngresos2" +
                                                ",IndIngresosREG=@IndIngresosREG" +
                                                ",IndIngresosSOP=@IndIngresosSOP" +
                                                ",IndCorreo=@IndCorreo" +
                                                ",IndCiudadResidencia=@IndCiudadResidencia" +
                                                ",IndNroInmuebles=@IndNroInmuebles" +
                                                ",IndAntCompra=@IndAntCompra" +
                                                ",IndAntUltimoMOV=@IndAntUltimoMOV" +
                                                ",IndHipotecasNro=@IndHipotecasNro" +
                                                ",IndRestriccionesNro=@IndRestriccionesNro" +
                                                ",IndFolioMatricula=@IndFolioMatricula" +
                                                ",IndFechaMatricula=@IndFechaMatricula" +
                                                ",GarantePrenda1Ind=@GarantePrenda1Ind," +
                                                "GarantePrenda2Ind=@GarantePrenda2Ind" +
                                                ",GaranteHipoteca1Ind=@GaranteHipoteca1Ind" +
                                                ",GaranteHipoteca2Ind=@GaranteHipoteca2Ind" +
                                                " WHERE  Consecutivo = @Consecutivo";
                #endregion

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoPersona", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TerceroDocTipoID", SqlDbType.Char, UDT_TerceroTipoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaExpDoc", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CiudadExpDoc", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaNacimiento", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ApellidoPri", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ApellidoSdo", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@NombrePri", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@NombreSdo", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@EstadoCivil", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ActEconomica1", SqlDbType.Char, UDT_ActEconomicaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ActEconomica2", SqlDbType.Char, UDT_ActEconomicaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IngresosREG", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@IngresosSOP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NroInmuebles", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AntCompra", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AntUltimoMOV", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@HipotecasNro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RestriccionesNro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FolioMatricula", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters.Add("@FechaMatricula", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@UsuarioDigita", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Correo", SqlDbType.Char, 60);
                mySqlCommandSel.Parameters.Add("@CiudadResidencia", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DataCreditoRecibeInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@DataCreditoRecibeFecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@DataCreditoRecibeUsuario", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PF_FincaRaiz", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_FincaRaizDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstadoActual", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasActuales", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUltAno", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_RepNegativos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_Estabilidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_Ubicabilidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_Probabilidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxFincaRaiz", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxEstadoActual", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxMorasActuales", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxMorasUltAno", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxRepNegativos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxEstabilidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxUbicabilidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxProbabilidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxFinancia", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngresoEstimado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_RecAguayLuz", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_ConfirmaCel", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_ConfirmaMail", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_VigenciaFMI", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PF_VigenciaConsData", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PF_NumHipotecas", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_NumBienes", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_Restricciones", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_AntCompra", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_UltAnotacion", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_IngresosMin1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngresosMin2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngresosMin3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngresosMin4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngresosMin5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngresosMin6", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngresosMin7", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ListaClintonInd", SqlDbType.Bit);

                mySqlCommandSel.Parameters.Add("@PF_PorObligaciones", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorUtilizaTDC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PeorCalificacion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_Consultas6Meses", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct30", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct60", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct90", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct120", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt30", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt60", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt90", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt120", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionCOB", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionDUD", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionCAS", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionEMB", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionREC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionCAN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_DireccDesde", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EntidadesNum", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CelularDesde", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CorreoDesde", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_DireccionNum", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_TelefonoNum", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CelularNum", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CorreoNum", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_FactorAcierta", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_AciertaResultado", SqlDbType.Decimal);

                /////
                mySqlCommandSel.Parameters.Add("@PF_PorObligacionesDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorUtilizaTDCDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PeorCalificacionDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_Consultas6MesesDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct30Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct60Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct90Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasAct120Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt30Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt60Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt90Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_MorasUlt120Dato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionCOBDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionDUDDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionCASDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionEMBDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionRECDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ObligacionCANDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_DireccDesdeDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EntidadesNumDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CelularDesdeDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CorreoDesdeDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_DireccDesdeMeses", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PF_EntidadesNumMeses", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PF_CelularDesdeMeses", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PF_CorreoDesdeMeses", SqlDbType.Int);

                mySqlCommandSel.Parameters.Add("@PF_DireccionNumDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_TelefonoNumDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CelularNumDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CorreoNumDato", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_DireccionNumCant", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_TelefonoNumCant", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_CelularNumCant", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_CorreoNumCant", SqlDbType.Bit);



                mySqlCommandSel.Parameters.Add("@PF_CapacidadPago", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxFinDeuCon", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CapPagAdDeu", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CapPagAdCon", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstCtasVIV", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotVIV", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstCtasBAN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotBAN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstCtasFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstCtasCOP", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotCOP", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstCtasTDC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotTDC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstCtasREA", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotREA", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_EstCtasCEL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotCEL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtasTotIngEst", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_QuiantiMIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_QuiantiMAX", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_QuantoIngrEst", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngrEstxQuanto", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_FactIngresosREG", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngrCapacPAG", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorIngrAporta", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ReqSopIngrInd", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorIngrPagoCtas", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngrDispPagoCtas", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CuotasACT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngrDispApoyos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FuenteIngresos1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FuenteIngresos2", SqlDbType.TinyInt);

                mySqlCommandSel.Parameters.Add("@IndTerceroID", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndApellidoPri", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndApellidoSdo", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndNombrePri", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndNombreSdo", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndTerceroDocTipoID", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndFechaExpDoc", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndFechaNacimiento", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndEstadoCivil", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndActEconomica1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndFuenteIngresos1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndFuenteIngresos2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndIngresosREG", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndIngresosSOP", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndCorreo", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndCiudadResidencia", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndNroInmuebles", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndAntCompra", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndAntUltimoMOV", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndHipotecasNro", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndRestriccionesNro", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndFolioMatricula", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IndFechaMatricula", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@GarantePrenda1Ind", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@GarantePrenda2Ind", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@GaranteHipoteca1Ind", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@GaranteHipoteca2Ind", SqlDbType.Bit);

                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                mySqlCommandSel.Parameters["@TipoPersona"].Value = Datos.TipoPersona.Value;
                mySqlCommandSel.Parameters["@TerceroID"].Value = Datos.TerceroID.Value;
                mySqlCommandSel.Parameters["@TerceroDocTipoID"].Value = Datos.TerceroDocTipoID.Value;
                mySqlCommandSel.Parameters["@FechaExpDoc"].Value = Datos.FechaExpDoc.Value;
                mySqlCommandSel.Parameters["@CiudadExpDoc"].Value = Datos.CiudadExpDoc.Value;
                mySqlCommandSel.Parameters["@FechaNacimiento"].Value = Datos.FechaNacimiento.Value;
                mySqlCommandSel.Parameters["@FechaNacimiento"].Value = Datos.FechaNacimiento.Value;
                mySqlCommandSel.Parameters["@ApellidoPri"].Value = Datos.ApellidoPri.Value;
                mySqlCommandSel.Parameters["@ApellidoSdo"].Value = Datos.ApellidoSdo.Value;
                mySqlCommandSel.Parameters["@NombrePri"].Value = Datos.NombrePri.Value;
                mySqlCommandSel.Parameters["@NombreSdo"].Value = Datos.NombreSdo.Value;
                mySqlCommandSel.Parameters["@EstadoCivil"].Value = Datos.EstadoCivil.Value;
                mySqlCommandSel.Parameters["@ActEconomica1"].Value = Datos.ActEconomica1.Value;
                mySqlCommandSel.Parameters["@ActEconomica2"].Value = Datos.ActEconomica2.Value;
                mySqlCommandSel.Parameters["@IngresosREG"].Value = Datos.IngresosREG.Value;
                mySqlCommandSel.Parameters["@IngresosSOP"].Value = Datos.IngresosSOP.Value;
                mySqlCommandSel.Parameters["@NroInmuebles"].Value = Datos.NroInmuebles.Value;
                mySqlCommandSel.Parameters["@AntCompra"].Value = Datos.AntCompra.Value;
                mySqlCommandSel.Parameters["@AntUltimoMOV"].Value = Datos.AntUltimoMOV.Value;
                mySqlCommandSel.Parameters["@HipotecasNro"].Value = Datos.HipotecasNro.Value;
                mySqlCommandSel.Parameters["@RestriccionesNro"].Value = Datos.RestriccionesNro.Value;
                mySqlCommandSel.Parameters["@FolioMatricula"].Value = Datos.FolioMatricula.Value;
                mySqlCommandSel.Parameters["@FechaMatricula"].Value = Datos.FechaMatricula.Value;
                mySqlCommandSel.Parameters["@UsuarioDigita"].Value = Datos.UsuarioDigita.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = DateTime.Now.Date;
                mySqlCommandSel.Parameters["@Correo"].Value = Datos.Correo.Value;
                mySqlCommandSel.Parameters["@CiudadResidencia"].Value = Datos.CiudadResidencia.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = Datos.Consecutivo.Value;
                mySqlCommandSel.Parameters["@DataCreditoRecibeInd"].Value = Datos.DataCreditoRecibeInd.Value;
                mySqlCommandSel.Parameters["@DataCreditoRecibeFecha"].Value = Datos.DataCreditoRecibeFecha.Value;
                mySqlCommandSel.Parameters["@DataCreditoRecibeUsuario"].Value = Datos.DataCreditoRecibeUsuario.Value;
                mySqlCommandSel.Parameters["@PF_FincaRaiz"].Value = Datos.PF_FincaRaiz.Value;
                mySqlCommandSel.Parameters["@PF_FincaRaizDato"].Value = Datos.PF_FincaRaizDato.Value;
                mySqlCommandSel.Parameters["@PF_EstadoActual"].Value = Datos.PF_EstadoActual.Value;
                mySqlCommandSel.Parameters["@PF_MorasActuales"].Value = Datos.PF_MorasActuales.Value;
                mySqlCommandSel.Parameters["@PF_MorasUltAno"].Value = Datos.PF_MorasUltAno.Value;
                mySqlCommandSel.Parameters["@PF_RepNegativos"].Value = Datos.PF_RepNegativos.Value;
                mySqlCommandSel.Parameters["@PF_Estabilidad"].Value = Datos.PF_Estabilidad.Value;
                mySqlCommandSel.Parameters["@PF_Ubicabilidad"].Value = Datos.PF_Ubicabilidad.Value;
                mySqlCommandSel.Parameters["@PF_Probabilidad"].Value = Datos.PF_Probabilidad.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxFincaRaiz"].Value = Datos.PF_PorMaxFincaRaiz.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxEstadoActual"].Value = Datos.PF_PorMaxEstadoActual.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxMorasActuales"].Value = Datos.PF_PorMaxMorasActuales.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxMorasUltAno"].Value = Datos.PF_PorMaxMorasUltAno.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxRepNegativos"].Value = Datos.PF_PorMaxRepNegativos.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxEstabilidad"].Value = Datos.PF_PorMaxEstabilidad.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxUbicabilidad"].Value = Datos.PF_PorMaxUbicabilidad.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxProbabilidad"].Value = Datos.PF_PorMaxProbabilidad.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxFinancia"].Value = Datos.PF_PorMaxFinancia.Value;
                mySqlCommandSel.Parameters["@PF_IngresoEstimado"].Value = Datos.PF_IngresoEstimado.Value;
                mySqlCommandSel.Parameters["@PF_RecAguayLuz"].Value = Datos.PF_RecAguayLuz.Value;
                mySqlCommandSel.Parameters["@PF_ConfirmaCel"].Value = Datos.PF_ConfirmaCel.Value;
                mySqlCommandSel.Parameters["@PF_ConfirmaMail"].Value = Datos.PF_ConfirmaMail.Value;
                mySqlCommandSel.Parameters["@PF_VigenciaFMI"].Value = Datos.PF_VigenciaFMI.Value;
                mySqlCommandSel.Parameters["@PF_VigenciaConsData"].Value = Datos.PF_VigenciaConsData.Value;
                mySqlCommandSel.Parameters["@PF_NumHipotecas"].Value = Datos.PF_NumHipotecas.Value;
                mySqlCommandSel.Parameters["@PF_NumBienes"].Value = Datos.PF_NumBienes.Value;
                mySqlCommandSel.Parameters["@PF_Restricciones"].Value = Datos.PF_Restricciones.Value;
                mySqlCommandSel.Parameters["@PF_AntCompra"].Value = Datos.PF_AntCompra.Value;
                mySqlCommandSel.Parameters["@PF_UltAnotacion"].Value = Datos.PF_UltAnotacion.Value;
                mySqlCommandSel.Parameters["@PF_IngresosMin1"].Value = Datos.PF_IngresosMin1.Value;
                mySqlCommandSel.Parameters["@PF_IngresosMin2"].Value = Datos.PF_IngresosMin2.Value;
                mySqlCommandSel.Parameters["@PF_IngresosMin3"].Value = Datos.PF_IngresosMin3.Value;
                mySqlCommandSel.Parameters["@PF_IngresosMin4"].Value = Datos.PF_IngresosMin4.Value;
                mySqlCommandSel.Parameters["@PF_IngresosMin5"].Value = Datos.PF_IngresosMin5.Value;
                mySqlCommandSel.Parameters["@PF_IngresosMin6"].Value = Datos.PF_IngresosMin6.Value;
                mySqlCommandSel.Parameters["@PF_IngresosMin7"].Value = Datos.PF_IngresosMin7.Value;
                mySqlCommandSel.Parameters["@ListaClintonInd"].Value = Datos.ListaClintonInd.Value;

                mySqlCommandSel.Parameters["@PF_PorObligaciones"].Value = Datos.PF_PorObligaciones.Value;
                mySqlCommandSel.Parameters["@PF_PorUtilizaTDC"].Value = Datos.PF_PorUtilizaTDC.Value;
                mySqlCommandSel.Parameters["@PF_PeorCalificacion"].Value = Datos.PF_PeorCalificacion.Value;
                mySqlCommandSel.Parameters["@PF_Consultas6Meses"].Value = Datos.PF_Consultas6Meses.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct30"].Value = Datos.PF_MorasAct30.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct60"].Value = Datos.PF_MorasAct60.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct90"].Value = Datos.PF_MorasAct90.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct120"].Value = Datos.PF_MorasAct120.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt30"].Value = Datos.PF_MorasUlt30.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt60"].Value = Datos.PF_MorasUlt60.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt90"].Value = Datos.PF_MorasUlt90.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt120"].Value = Datos.PF_MorasUlt120.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionCOB"].Value = Datos.PF_ObligacionCOB.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionDUD"].Value = Datos.PF_ObligacionDUD.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionCAS"].Value = Datos.PF_ObligacionCAS.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionEMB"].Value = Datos.PF_ObligacionEMB.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionREC"].Value = Datos.PF_ObligacionREC.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionCAN"].Value = Datos.PF_ObligacionCAN.Value;
                mySqlCommandSel.Parameters["@PF_DireccDesde"].Value = Datos.PF_DireccDesde.Value;
                mySqlCommandSel.Parameters["@PF_EntidadesNum"].Value = Datos.PF_EntidadesNum.Value;
                mySqlCommandSel.Parameters["@PF_CelularDesde"].Value = Datos.PF_CelularDesde.Value;
                mySqlCommandSel.Parameters["@PF_CorreoDesde"].Value = Datos.PF_CorreoDesde.Value;
                mySqlCommandSel.Parameters["@PF_DireccionNum"].Value = Datos.PF_DireccionNum.Value;
                mySqlCommandSel.Parameters["@PF_TelefonoNum"].Value = Datos.PF_TelefonoNum.Value;
                mySqlCommandSel.Parameters["@PF_CelularNum"].Value = Datos.PF_CelularNum.Value;
                mySqlCommandSel.Parameters["@PF_CorreoNum"].Value = Datos.PF_CorreoNum.Value;
                mySqlCommandSel.Parameters["@PF_FactorAcierta"].Value = Datos.PF_FactorAcierta.Value;
                mySqlCommandSel.Parameters["@PF_AciertaResultado"].Value = Datos.PF_AciertaResultado.Value;
                /////
                mySqlCommandSel.Parameters["@PF_PorObligacionesDato"].Value = Datos.PF_PorObligacionesDato.Value;
                mySqlCommandSel.Parameters["@PF_PorUtilizaTDCDato"].Value = Datos.PF_PorUtilizaTDCDato.Value;
                mySqlCommandSel.Parameters["@PF_PeorCalificacionDato"].Value = Datos.PF_PeorCalificacionDato.Value;
                mySqlCommandSel.Parameters["@PF_Consultas6MesesDato"].Value = Datos.PF_Consultas6MesesDato.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct30Dato"].Value = Datos.PF_MorasAct30Dato.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct60Dato"].Value = Datos.PF_MorasAct60Dato.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct90Dato"].Value = Datos.PF_MorasAct90Dato.Value;
                mySqlCommandSel.Parameters["@PF_MorasAct120Dato"].Value = Datos.PF_MorasAct120Dato.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt30Dato"].Value = Datos.PF_MorasUlt30Dato.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt60Dato"].Value = Datos.PF_MorasUlt60Dato.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt90Dato"].Value = Datos.PF_MorasUlt90Dato.Value;
                mySqlCommandSel.Parameters["@PF_MorasUlt120Dato"].Value = Datos.PF_MorasUlt120Dato.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionCOBDato"].Value = Datos.PF_ObligacionCOBDato.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionDUDDato"].Value = Datos.PF_ObligacionDUDDato.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionCASDato"].Value = Datos.PF_ObligacionCASDato.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionEMBDato"].Value = Datos.PF_ObligacionEMBDato.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionRECDato"].Value = Datos.PF_ObligacionRECDato.Value;
                mySqlCommandSel.Parameters["@PF_ObligacionCANDato"].Value = Datos.PF_ObligacionCANDato.Value;
                mySqlCommandSel.Parameters["@PF_DireccDesdeDato"].Value = Datos.PF_DireccDesdeDato.Value;
                mySqlCommandSel.Parameters["@PF_EntidadesNumDato"].Value = Datos.PF_EntidadesNumDato.Value;
                mySqlCommandSel.Parameters["@PF_CelularDesdeDato"].Value = Datos.PF_CelularDesdeDato.Value;
                mySqlCommandSel.Parameters["@PF_CorreoDesdeDato"].Value = Datos.PF_CorreoDesdeDato.Value;
                mySqlCommandSel.Parameters["@PF_DireccDesdeMeses"].Value = Datos.PF_DireccDesdeMeses.Value;
                mySqlCommandSel.Parameters["@PF_EntidadesNumMeses"].Value = Datos.PF_EntidadesNumMeses.Value;
                mySqlCommandSel.Parameters["@PF_CelularDesdeMeses"].Value = Datos.PF_CelularDesdeMeses.Value;
                mySqlCommandSel.Parameters["@PF_CorreoDesdeMeses"].Value = Datos.PF_CorreoDesdeMeses.Value;
                mySqlCommandSel.Parameters["@PF_DireccionNumDato"].Value = Datos.PF_DireccionNumDato.Value;
                mySqlCommandSel.Parameters["@PF_TelefonoNumDato"].Value = Datos.PF_TelefonoNumDato.Value;
                mySqlCommandSel.Parameters["@PF_CelularNumDato"].Value = Datos.PF_CelularNumDato.Value;
                mySqlCommandSel.Parameters["@PF_CorreoNumDato"].Value = Datos.PF_CorreoNumDato.Value;
                mySqlCommandSel.Parameters["@PF_DireccionNumCant"].Value = Datos.PF_DireccionNumCant.Value;
                mySqlCommandSel.Parameters["@PF_TelefonoNumCant"].Value = Datos.PF_TelefonoNumCant.Value;
                mySqlCommandSel.Parameters["@PF_CelularNumCant"].Value = Datos.PF_CelularNumCant.Value;
                mySqlCommandSel.Parameters["@PF_CorreoNumCant"].Value = Datos.PF_CorreoNumCant.Value;


                mySqlCommandSel.Parameters["@PF_CapacidadPago"].Value = Datos.PF_CapacidadPago.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxFinDeuCon"].Value = Datos.PF_PorMaxFinDeuCon.Value;
                mySqlCommandSel.Parameters["@PF_CapPagAdDeu"].Value = Datos.PF_CapPagAdDeu.Value;
                mySqlCommandSel.Parameters["@PF_CapPagAdCon"].Value = Datos.PF_CapPagAdCon.Value;
                mySqlCommandSel.Parameters["@PF_EstCtasVIV"].Value = Datos.PF_EstCtasVIV.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotVIV"].Value = Datos.PF_CtasTotVIV.Value;
                mySqlCommandSel.Parameters["@PF_EstCtasBAN"].Value = Datos.PF_EstCtasBAN.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotBAN"].Value = Datos.PF_CtasTotBAN.Value;
                mySqlCommandSel.Parameters["@PF_EstCtasFIN"].Value = Datos.PF_EstCtasFIN.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotFIN"].Value = Datos.PF_CtasTotFIN.Value;
                mySqlCommandSel.Parameters["@PF_EstCtasCOP"].Value = Datos.PF_EstCtasCOP.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotCOP"].Value = Datos.PF_CtasTotCOP.Value;
                mySqlCommandSel.Parameters["@PF_EstCtasTDC"].Value = Datos.PF_EstCtasTDC.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotTDC"].Value = Datos.PF_CtasTotTDC.Value;
                mySqlCommandSel.Parameters["@PF_EstCtasREA"].Value = Datos.PF_EstCtasREA.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotREA"].Value = Datos.PF_CtasTotREA.Value;
                mySqlCommandSel.Parameters["@PF_EstCtasCEL"].Value = Datos.PF_EstCtasCEL.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotCEL"].Value = Datos.PF_CtasTotCEL.Value;
                mySqlCommandSel.Parameters["@PF_CtasTotIngEst"].Value = Datos.PF_CtasTotIngEst.Value;
                mySqlCommandSel.Parameters["@PF_QuiantiMIN"].Value = Datos.PF_QuiantiMIN.Value;
                mySqlCommandSel.Parameters["@PF_QuiantiMAX"].Value = Datos.PF_QuiantiMAX.Value;
                mySqlCommandSel.Parameters["@PF_QuantoIngrEst"].Value = Datos.PF_QuantoIngrEst.Value;
                mySqlCommandSel.Parameters["@PF_IngrEstxQuanto"].Value = Datos.PF_IngrEstxQuanto.Value;
                mySqlCommandSel.Parameters["@PF_FactIngresosREG"].Value = Datos.PF_FactIngresosREG.Value;
                mySqlCommandSel.Parameters["@PF_IngrCapacPAG"].Value = Datos.PF_IngrCapacPAG.Value;
                mySqlCommandSel.Parameters["@PF_PorIngrAporta"].Value = Datos.PF_PorIngrAporta.Value;
                mySqlCommandSel.Parameters["@PF_ReqSopIngrInd"].Value = Datos.PF_ReqSopIngrInd.Value;
                mySqlCommandSel.Parameters["@PF_PorIngrPagoCtas"].Value = Datos.PF_PorIngrPagoCtas.Value;
                mySqlCommandSel.Parameters["@PF_IngrDispPagoCtas"].Value = Datos.PF_IngrDispPagoCtas.Value;
                mySqlCommandSel.Parameters["@PF_CuotasACT"].Value = Datos.PF_CuotasACT.Value;
                mySqlCommandSel.Parameters["@PF_IngrDispApoyos"].Value = Datos.PF_IngrDispApoyos.Value;

                mySqlCommandSel.Parameters["@FuenteIngresos1"].Value = Datos.FuenteIngresos1.Value;
                mySqlCommandSel.Parameters["@FuenteIngresos2"].Value = Datos.FuenteIngresos2.Value;

                mySqlCommandSel.Parameters["@IndTerceroID"].Value = Datos.IndTerceroID.Value;
                mySqlCommandSel.Parameters["@IndApellidoPri"].Value = Datos.IndApellidoPri.Value;
                mySqlCommandSel.Parameters["@IndApellidoSdo"].Value = Datos.IndApellidoSdo.Value;
                mySqlCommandSel.Parameters["@IndNombrePri"].Value = Datos.IndNombrePri.Value;
                mySqlCommandSel.Parameters["@IndNombreSdo"].Value = Datos.IndNombreSdo.Value;
                mySqlCommandSel.Parameters["@IndTerceroDocTipoID"].Value = Datos.IndTerceroDocTipoID.Value;
                mySqlCommandSel.Parameters["@IndFechaExpDoc"].Value = Datos.IndFechaExpDoc.Value;
                mySqlCommandSel.Parameters["@IndFechaNacimiento"].Value = Datos.IndFechaNacimiento.Value;
                mySqlCommandSel.Parameters["@IndEstadoCivil"].Value = Datos.IndEstadoCivil.Value;
                mySqlCommandSel.Parameters["@IndActEconomica1"].Value = Datos.IndActEconomica1.Value;
                mySqlCommandSel.Parameters["@IndFuenteIngresos1"].Value = Datos.IndFuenteIngresos1.Value;
                mySqlCommandSel.Parameters["@IndFuenteIngresos2"].Value = Datos.IndFuenteIngresos2.Value;
                mySqlCommandSel.Parameters["@IndIngresosREG"].Value = Datos.IndIngresosREG.Value;
                mySqlCommandSel.Parameters["@IndIngresosSOP"].Value = Datos.IndIngresosSOP.Value;
                mySqlCommandSel.Parameters["@IndCorreo"].Value = Datos.IndCorreo.Value;
                mySqlCommandSel.Parameters["@IndCiudadResidencia"].Value = Datos.IndCiudadResidencia.Value;
                mySqlCommandSel.Parameters["@IndNroInmuebles"].Value = Datos.IndNroInmuebles.Value;
                mySqlCommandSel.Parameters["@IndAntCompra"].Value = Datos.IndAntCompra.Value;
                mySqlCommandSel.Parameters["@IndAntUltimoMOV"].Value = Datos.IndAntUltimoMOV.Value;
                mySqlCommandSel.Parameters["@IndHipotecasNro"].Value = Datos.IndHipotecasNro.Value;
                mySqlCommandSel.Parameters["@IndRestriccionesNro"].Value = Datos.IndRestriccionesNro.Value;
                mySqlCommandSel.Parameters["@IndFolioMatricula"].Value = Datos.IndFolioMatricula.Value;
                mySqlCommandSel.Parameters["@IndFechaMatricula"].Value = Datos.IndFechaMatricula.Value;
                mySqlCommandSel.Parameters["@GarantePrenda1Ind"].Value = Datos.GarantePrenda1Ind.Value;
                mySqlCommandSel.Parameters["@GarantePrenda2Ind"].Value = Datos.GarantePrenda2Ind.Value;
                mySqlCommandSel.Parameters["@GaranteHipoteca1Ind"].Value = Datos.GaranteHipoteca1Ind.Value;
                mySqlCommandSel.Parameters["@GaranteHipoteca2Ind"].Value = Datos.GaranteHipoteca2Ind.Value;

                //mySqlCommandSel.Parameters["@eg_glZona"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glZona, this.Empresa, egCtrl);

                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosPersonales_Update");
                throw exception;
            }
        }

        public void DAL_drSolicitudDatosPersonales_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandText = "DELETE FROM drSolicitudDatosPersonales WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosPersonales_Delete");
                throw exception;
            }
        }

        #endregion

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter">Doc Control filtro</param>
        /// <returns>Lista </returns>
        public List<DTO_drSolicitudDatosPersonales> DAL_drSolicitudDatosPersonales_GetByParameter(DTO_drSolicitudDatosPersonales filter)
        {
            try
            {
                List<DTO_drSolicitudDatosPersonales> result = new List<DTO_drSolicitudDatosPersonales>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = " Select sol.* " +
                        " from drSolicitudDatosPersonales sol with(nolock) " +
                        " inner join glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = sol.NumeroDoc " +
                        " inner join ccSolicitudDocu docu with(nolock) on docu.NumeroDoc = sol.NumeroDoc " +
                        " left join coTercero terc with(nolock) on terc.TerceroID = sol.TerceroID and terc.EmpresaGrupoID = sol.eg_coTercero " +
                        " where ctrl.EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if (!string.IsNullOrEmpty(filter.NumeroDoc.Value.ToString()))
                {
                    query += "and sol.NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = filter.NumeroDoc.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.TerceroID.Value.ToString()))
                {
                    query += "and sol.TerceroID = @TerceroID ";
                    mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommand.Parameters["@TerceroID"].Value = filter.TerceroID.Value;
                    filterInd = true;
                }

                query += " order by doc.NumeroDoc desc";
                mySqlCommand.CommandText = query;

                if (!filterInd)
                    return result;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_drSolicitudDatosPersonales ctrl = new DTO_drSolicitudDatosPersonales(dr);
                    //ctrl.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                    result.Add(ctrl);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosPersonales_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene un datatable con la info de datacredito
        /// </summary>
        /// <returns>Datatable</returns>
        public DataTable DAL_drSolicitudDatosPersonales_GetForDatacredito()
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();
                DataTable table = new DataTable();

                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                string query = " Select sol.NumeroDoc, sol.TerceroID " +
                        " from drSolicitudDatosPersonales sol with(nolock) " +
                        " inner join glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = sol.NumeroDoc " +
                        " inner join ccSolicitudDocu docu with(nolock) on docu.NumeroDoc = sol.NumeroDoc  and sol.Version = docu.VersionNro " +
                        " left join coTercero terc with(nolock) on terc.TerceroID = sol.TerceroID " +
                        " where ctrl.EmpresaID = @EmpresaID and (sol.DataCreditoRecibeInd = 0 or sol.DataCreditoRecibeInd  is null) " +
                        " order by sol.NumeroDoc desc";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommandSel.CommandText = query;
                sda.SelectCommand = mySqlCommandSel;

                if (!string.IsNullOrEmpty(mySqlCommandSel.CommandText))
                    sda.Fill(table);

                //foreach (DataRow item in table.Rows)
                //    item["TerceroID"] = item.ItemArray[0].ToString().Length > 0 ? item.ItemArray[0].ToString().TrimEnd() : string.Empty;

                return table;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosPersonales_GetForDatacredito");
                return null;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="consec"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public bool DAL_drSolicitudDatosPersonales_Exist(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from drSolicitudDatosPersonales with(nolock) where Consecutivo = @Consecutivo";

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consec;


                if (mySqlCommand.Parameters["@Consecutivo"].Value == null || ((mySqlCommand.Parameters["@Consecutivo"].Value is string) &&
                    string.IsNullOrWhiteSpace(mySqlCommand.Parameters["@Consecutivo"].Value.ToString())))
                    mySqlCommand.Parameters["@Consecutivo"].Value = DBNull.Value;


                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosPersonales_Exist");
                throw exception;
            }
        }

        /// <summary>
        ///  Actualiza el campo Observacion de la tabla drSolicitudDatosPersonales
        /// </summary>
        /// <param name="consecutivo">consec del registro</param>
        /// <param name="recibeInd">si recibe o no</param>
        /// <param name="recibeFecha">fecha de recibido</param>
        /// <param name="recibeUsuario">usuario que recibe</param>
        /// <returns></returns>
        public bool DAL_drSolicitudDatosPersonales_UpdateDatacredito(int consecutivo,bool? recibeInd, DateTime? recibeFecha,string recibeUsuario)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "UPDATE drSolicitudDatosPersonales SET " +
                                               " DataCreditoRecibeInd=@DataCreditoRecibeInd," +
                                               " DataCreditoRecibeFecha=@DataCreditoRecibeFecha," +
                                               " DataCreditoRecibeUsuario=@DataCreditoRecibeUsuario " +
                                                " WHERE Consecutivo = @Consecutivo";
                #endregion

                #region Creacion de comandos              
                mySqlCommandSel.Parameters.Add("@DataCreditoRecibeInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@DataCreditoRecibeFecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@DataCreditoRecibeUsuario", SqlDbType.Char, UDT_UsuarioID.MaxLength);                
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asigna los valores             
                mySqlCommandSel.Parameters["@DataCreditoRecibeInd"].Value = recibeInd;
                mySqlCommandSel.Parameters["@DataCreditoRecibeFecha"].Value = recibeFecha;
                mySqlCommandSel.Parameters["@DataCreditoRecibeUsuario"].Value = recibeUsuario;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = consecutivo;
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosPersonales_UpdateDatacredito");
                throw exception;
            }
        }

    }
}
