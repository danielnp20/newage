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
    public class DAL_ccSolicitudDataCreditoDatos : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccSolicitudDataCreditoDatos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccSolicitudDataCreditoDatos que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_ccSolicitudDataCreditoDatos</returns>
        public List<DTO_ccSolicitudDataCreditoDatos> DAL_ccSolicitudDataCreditoDatos_GetAll()
        {
            try
            {
                List<DTO_ccSolicitudDataCreditoDatos> result = new List<DTO_ccSolicitudDataCreditoDatos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT Datos.* FROM ccSolicitudDataCreditoDatos Datos with(nolock)";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudDataCreditoDatos Datos;
                    Datos = new DTO_ccSolicitudDataCreditoDatos(dr);
                    result.Add(Datos);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoDatos_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public List<DTO_ccSolicitudDataCreditoDatos> DAL_ccSolicitudDataCreditoDatos_GetByNUmeroDoc(int numeroDoc, Int16? versionNro)
        {
            try
            {
                List<DTO_ccSolicitudDataCreditoDatos> result = new List<DTO_ccSolicitudDataCreditoDatos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters.Add("@Version", SqlDbType.SmallInt);
                mySqlCommand.Parameters["@Version"].Value = versionNro;

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                if (versionNro.HasValue)
                    mySqlCommand.CommandText = "select * from ccSolicitudDataCreditoDatos  with(nolock) " +
                                         " Where NumeroDoc =  @NumeroDoc and Version =  @Version  ";
                else
                    mySqlCommand.CommandText = "select * from ccSolicitudDataCreditoDatos  with(nolock) " +
                                        " Where NumeroDoc =  @NumeroDoc ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                    result.Add(new DTO_ccSolicitudDataCreditoDatos(dr));
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoDatos_GetByNUmeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="numeroDoc">identificador de la actividad</param>
        /// <returns>Dto de Detalle Docu</returns>
        public List<DTO_ccSolicitudDataCreditoDatos> DAL_ccSolicitudDataCreditoDatos_GetByLastVersion(int numeroDoc)
        {
            try
            {
                List<DTO_ccSolicitudDataCreditoDatos> result = new List<DTO_ccSolicitudDataCreditoDatos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommand.CommandText = "select Version from ccSolicitudDataCreditoDatos  with(nolock) " +
                                      " Where NumeroDoc =  @NumeroDoc  order by Version desc ";

                var version = mySqlCommand.ExecuteScalar();

                mySqlCommand.Parameters.Add("@Version", SqlDbType.SmallInt);
                mySqlCommand.Parameters["@Version"].Value = version != null? Convert.ToInt16(version) : version;

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommand.CommandText = "Select * from ccSolicitudDataCreditoDatos datos with(nolock) " +
                                            //"  inner join drSolicitudDatosPersonales sol on sol.NumeroDoc = datos.NumeroDoc and sol.Version = datos.Version and sol.TerceroID = cast(datos.NumeroId as integer) "+
                                            " Where datos.NumeroDoc = @NumeroDoc and datos.Version = @Version ";// and sol.DataCreditoRecibeInd is not null  ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                    result.Add(new DTO_ccSolicitudDataCreditoDatos(dr));
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoDatos_GetByLastVersion");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDataCreditoDatos
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public int DAL_ccSolicitudDataCreditoDatos_Add(DTO_ccSolicitudDataCreditoDatos Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO ccSolicitudDataCreditoDatos " +
                    "( " +
                    "   NumeroDoc,Version,TipoId,NumeroId,EstadoDoc,Nombre,RangoEdad,Genero,CiudadExp,FechaAct,ActEconomica,RUT,TipoContrato," +
                    "   FechaContrato,NumeObligACT,NumObligaBAN,VlrInicialBAN,VlrSaldoBAN,VlrCuotasBAN,VlrMoraBAN,NumObligaVIV,VlrInicialVIV," +
                    "   VlrSaldoVIV,VlrCuotasVIV,VlrMoraVIV,NumObligaFIN,VlrInicialFIN,VlrSaldoFIN,VlrCuotasFIN,VlrMoraFIN,NumeroTDC,VlrCuposTDC," +
                    "   VlrUtilizadoTDC,PorUtilizaTDC,VlrCuotasTDC,VlrMoraTDC,Rango0TDC,Rango1TDC,Rango2TDC,Rango3TDC,Rango4TDC,Rango5TDC,Rango6TDC," +
                    "   FchAntiguaTDC,NumObligaREA,VlrInicialREA,VlrSaldoREA,VlrCuotasREA,VlrMoraREA,NumObligaCEL,VlrCuotasCEL,VlrMoraCEL,NumObligaCOP," +
                    "   VlrInicialCOP,VlrSaldoCOP,VlrCuotasCOP,VlrMoraCOP,NumObligaCOD,VlrSaldoCOD,VlrCuotasCOD,VlrMoraCOD,EstadoActDia,EstadoAct30,EstadoAct60," +
                    "   EstadoAct90,EstadoAct120,EstadoActCas,EstadoActDud,EstadoActCob,EstadoHis30,EstadoHis60,EstadoHis90,EstadoHis120,EstadoHisCan," +
                    "   EstadoHisRec,AlturaMorTDC,AlturaMorBAN,AlturaMorCOP,AlturaMorHIP,PeorEndeudT1,PeorEndeudT2,CtasAhorrosAct,CtasCorrienteAct,CtasEmbargadas," +
                    "   CtasMalManejo,CtasSaldadas,UltConsultas,EstadoConsulta" +
                    ") " +
                    "VALUES " +
                    "( " +
                    "   @NumeroDoc,@Version,@TipoId,@NumeroId,@EstadoDoc,@Nombre,@RangoEdad,@Genero,@CiudadExp,@FechaAct,@ActEconomica,@RUT,@TipoContrato," +
                    "   @FechaContrato,@NumeObligACT,@NumObligaBAN,@VlrInicialBAN,@VlrSaldoBAN,@VlrCuotasBAN,@VlrMoraBAN,@NumObligaVIV,@VlrInicialVIV,@VlrSaldoVIV," +
                    "   @VlrCuotasVIV,@VlrMoraVIV,@NumObligaFIN,@VlrInicialFIN,@VlrSaldoFIN,@VlrCuotasFIN,@VlrMoraFIN,@NumeroTDC,@VlrCuposTDC,@VlrUtilizadoTDC," +
                    "   @PorUtilizaTDC,@VlrCuotasTDC,@VlrMoraTDC,@Rango0TDC,@Rango1TDC,@Rango2TDC,@Rango3TDC,@Rango4TDC,@Rango5TDC,@Rango6TDC,@FchAntiguaTDC," +
                    "   @NumObligaREA,@VlrInicialREA,@VlrSaldoREA,@VlrCuotasREA,@VlrMoraREA,@NumObligaCEL,@VlrCuotasCEL,@VlrMoraCEL,@NumObligaCOP,@VlrInicialCOP," +
                    "   @VlrSaldoCOP,@VlrCuotasCOP,@VlrMoraCOP,@NumObligaCOD,@VlrSaldoCOD,@VlrCuotasCOD,@VlrMoraCOD,@EstadoActDia,@EstadoAct30,@EstadoAct60," +
                    "   @EstadoAct90,@EstadoAct120,@EstadoActCas,@EstadoActDud,@EstadoActCob,@EstadoHis30,@EstadoHis60,@EstadoHis90,@EstadoHis120,@EstadoHisCan," +
                    "   @EstadoHisRec,@AlturaMorTDC,@AlturaMorBAN,@AlturaMorCOP,@AlturaMorHIP,@PeorEndeudT1,@PeorEndeudT2,@CtasAhorrosAct,@CtasCorrienteAct," +
                    "   @CtasEmbargadas,@CtasMalManejo,@CtasSaldadas,@UltConsultas,@EstadoConsulta" +
                ") SET @Consecutivo = SCOPE_IDENTITY() ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoId", SqlDbType.Char,1);
                mySqlCommandSel.Parameters.Add("@NumeroId", SqlDbType.Char,11);
                mySqlCommandSel.Parameters.Add("@EstadoDoc", SqlDbType.Char, 11);
                mySqlCommandSel.Parameters.Add("@Nombre", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@RangoEdad", SqlDbType.Char, 5);
                mySqlCommandSel.Parameters.Add("@Genero", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@CiudadExp", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@FechaAct", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ActEconomica", SqlDbType.Char, 2);
                mySqlCommandSel.Parameters.Add("@RUT", SqlDbType.Char,2);
                mySqlCommandSel.Parameters.Add("@TipoContrato", SqlDbType.Char,1);
                mySqlCommandSel.Parameters.Add("@FechaContrato", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumeObligACT", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@NumObligaBAN", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrInicialBAN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoBAN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotasBAN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraBAN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumObligaVIV", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrInicialVIV", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoVIV", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotasVIV", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraVIV", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumObligaFIN", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrInicialFIN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoFIN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotasFIN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraFIN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroTDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrCuposTDC", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrUtilizadoTDC", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PorUtilizaTDC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCuotasTDC", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraTDC", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Rango0TDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Rango1TDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Rango2TDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Rango3TDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Rango4TDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Rango5TDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Rango6TDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FchAntiguaTDC", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumObligaREA", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrInicialREA", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoREA", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotasREA", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraREA", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumObligaCEL", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrCuotasCEL", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraCEL", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumObligaCOP", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrInicialCOP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCOP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotasCOP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraCOP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumObligaCOD", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCOD", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotasCOD", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraCOD", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EstadoActDia", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoAct30", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoAct60", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoAct90", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoAct120", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoActCas", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoActDud", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoActCob", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoHis30", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoHis60", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoHis90", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoHis120", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoHisCan", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoHisRec", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AlturaMorTDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AlturaMorBAN", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AlturaMorCOP", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AlturaMorHIP", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PeorEndeudT1", SqlDbType.Char,1);
                mySqlCommandSel.Parameters.Add("@PeorEndeudT2", SqlDbType.Char,1);
                mySqlCommandSel.Parameters.Add("@CtasAhorrosAct", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CtasCorrienteAct", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CtasEmbargadas", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CtasMalManejo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CtasSaldadas", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@UltConsultas", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoConsulta", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);


                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                mySqlCommandSel.Parameters["@TipoId"].Value = Datos.TipoId.Value;
                mySqlCommandSel.Parameters["@NumeroId"].Value = Datos.NumeroId.Value;
                mySqlCommandSel.Parameters["@EstadoDoc"].Value = Datos.EstadoDoc.Value;
                mySqlCommandSel.Parameters["@Nombre"].Value = Datos.Nombre.Value;
                mySqlCommandSel.Parameters["@RangoEdad"].Value = Datos.RangoEdad.Value;
                mySqlCommandSel.Parameters["@Genero"].Value = Datos.Genero.Value;
                mySqlCommandSel.Parameters["@CiudadExp"].Value = Datos.CiudadExp.Value;
                mySqlCommandSel.Parameters["@FechaAct"].Value = Datos.FechaAct.Value;
                mySqlCommandSel.Parameters["@ActEconomica"].Value = Datos.ActEconomica.Value;
                mySqlCommandSel.Parameters["@RUT"].Value = Datos.RUT.Value;
                mySqlCommandSel.Parameters["@TipoContrato"].Value = Datos.TipoContrato.Value;
                mySqlCommandSel.Parameters["@FechaContrato"].Value = Datos.FechaContrato.Value;
                mySqlCommandSel.Parameters["@NumeObligACT"].Value = Datos.NumeObligACT.Value;
                mySqlCommandSel.Parameters["@NumObligaBAN"].Value = Datos.NumObligaBAN.Value;
                mySqlCommandSel.Parameters["@VlrInicialBAN"].Value = Datos.VlrInicialBAN.Value;
                mySqlCommandSel.Parameters["@VlrSaldoBAN"].Value = Datos.VlrSaldoBAN.Value;
                mySqlCommandSel.Parameters["@VlrCuotasBAN"].Value = Datos.VlrCuotasBAN.Value;
                mySqlCommandSel.Parameters["@VlrMoraBAN"].Value = Datos.VlrMoraBAN.Value;
                mySqlCommandSel.Parameters["@NumObligaVIV"].Value = Datos.NumObligaVIV.Value;
                mySqlCommandSel.Parameters["@VlrInicialVIV"].Value = Datos.VlrInicialVIV.Value;
                mySqlCommandSel.Parameters["@VlrSaldoVIV"].Value = Datos.VlrSaldoVIV.Value;
                mySqlCommandSel.Parameters["@VlrCuotasVIV"].Value = Datos.VlrCuotasVIV.Value;
                mySqlCommandSel.Parameters["@VlrMoraVIV"].Value = Datos.VlrMoraVIV.Value;
                mySqlCommandSel.Parameters["@NumObligaFIN"].Value = Datos.NumObligaFIN.Value;
                mySqlCommandSel.Parameters["@VlrInicialFIN"].Value = Datos.VlrInicialFIN.Value;
                mySqlCommandSel.Parameters["@VlrSaldoFIN"].Value = Datos.VlrSaldoFIN.Value;
                mySqlCommandSel.Parameters["@VlrCuotasFIN"].Value = Datos.VlrCuotasFIN.Value;
                mySqlCommandSel.Parameters["@VlrMoraFIN"].Value = Datos.VlrMoraFIN.Value;
                mySqlCommandSel.Parameters["@NumeroTDC"].Value = Datos.NumeroTDC.Value;
                mySqlCommandSel.Parameters["@VlrCuposTDC"].Value = Datos.VlrCuposTDC.Value;
                mySqlCommandSel.Parameters["@VlrUtilizadoTDC"].Value = Datos.VlrUtilizadoTDC.Value;
                mySqlCommandSel.Parameters["@PorUtilizaTDC"].Value = Datos.PorUtilizaTDC.Value;
                mySqlCommandSel.Parameters["@VlrCuotasTDC"].Value = Datos.VlrCuotasTDC.Value;
                mySqlCommandSel.Parameters["@VlrMoraTDC"].Value = Datos.VlrMoraTDC.Value;
                mySqlCommandSel.Parameters["@Rango0TDC"].Value = Datos.Rango0TDC.Value;
                mySqlCommandSel.Parameters["@Rango1TDC"].Value = Datos.Rango1TDC.Value;
                mySqlCommandSel.Parameters["@Rango2TDC"].Value = Datos.Rango2TDC.Value;
                mySqlCommandSel.Parameters["@Rango3TDC"].Value = Datos.Rango3TDC.Value;
                mySqlCommandSel.Parameters["@Rango4TDC"].Value = Datos.Rango4TDC.Value;
                mySqlCommandSel.Parameters["@Rango5TDC"].Value = Datos.Rango5TDC.Value;
                mySqlCommandSel.Parameters["@Rango6TDC"].Value = Datos.Rango6TDC.Value;
                mySqlCommandSel.Parameters["@FchAntiguaTDC"].Value = Datos.FchAntiguaTDC.Value;
                mySqlCommandSel.Parameters["@NumObligaREA"].Value = Datos.NumObligaREA.Value;
                mySqlCommandSel.Parameters["@VlrInicialREA"].Value = Datos.VlrInicialREA.Value;
                mySqlCommandSel.Parameters["@VlrSaldoREA"].Value = Datos.VlrSaldoREA.Value;
                mySqlCommandSel.Parameters["@VlrCuotasREA"].Value = Datos.VlrCuotasREA.Value;
                mySqlCommandSel.Parameters["@VlrMoraREA"].Value = Datos.VlrMoraREA.Value;
                mySqlCommandSel.Parameters["@NumObligaCEL"].Value = Datos.NumObligaCEL.Value;
                mySqlCommandSel.Parameters["@VlrCuotasCEL"].Value = Datos.VlrCuotasCEL.Value;
                mySqlCommandSel.Parameters["@VlrMoraCEL"].Value = Datos.VlrMoraCEL.Value;
                mySqlCommandSel.Parameters["@NumObligaCOP"].Value = Datos.NumObligaCOP.Value;
                mySqlCommandSel.Parameters["@VlrInicialCOP"].Value = Datos.VlrInicialCOP.Value;
                mySqlCommandSel.Parameters["@VlrSaldoCOP"].Value = Datos.VlrSaldoCOP.Value;
                mySqlCommandSel.Parameters["@VlrCuotasCOP"].Value = Datos.VlrCuotasCOP.Value;
                mySqlCommandSel.Parameters["@VlrMoraCOP"].Value = Datos.VlrMoraCOP.Value;
                mySqlCommandSel.Parameters["@NumObligaCOD"].Value = Datos.NumObligaCOD.Value;
                mySqlCommandSel.Parameters["@VlrSaldoCOD"].Value = Datos.VlrSaldoCOD.Value;
                mySqlCommandSel.Parameters["@VlrCuotasCOD"].Value = Datos.VlrCuotasCOD.Value;
                mySqlCommandSel.Parameters["@VlrMoraCOD"].Value = Datos.VlrMoraCOD.Value;
                mySqlCommandSel.Parameters["@EstadoActDia"].Value = Datos.EstadoActDia.Value;
                mySqlCommandSel.Parameters["@EstadoAct30"].Value = Datos.EstadoAct30.Value;
                mySqlCommandSel.Parameters["@EstadoAct60"].Value = Datos.EstadoAct60.Value;
                mySqlCommandSel.Parameters["@EstadoAct90"].Value = Datos.EstadoAct90.Value;
                mySqlCommandSel.Parameters["@EstadoAct120"].Value = Datos.EstadoAct120.Value;
                mySqlCommandSel.Parameters["@EstadoActCas"].Value = Datos.EstadoActCas.Value;
                mySqlCommandSel.Parameters["@EstadoActDud"].Value = Datos.EstadoActDud.Value;
                mySqlCommandSel.Parameters["@EstadoActCob"].Value = Datos.EstadoActCob.Value;
                mySqlCommandSel.Parameters["@EstadoHis30"].Value = Datos.EstadoHis30.Value;
                mySqlCommandSel.Parameters["@EstadoHis60"].Value = Datos.EstadoHis60.Value;
                mySqlCommandSel.Parameters["@EstadoHis90"].Value = Datos.EstadoHis90.Value;
                mySqlCommandSel.Parameters["@EstadoHis120"].Value = Datos.EstadoHis120.Value;
                mySqlCommandSel.Parameters["@EstadoHisCan"].Value = Datos.EstadoHisCan.Value;
                mySqlCommandSel.Parameters["@EstadoHisRec"].Value = Datos.EstadoHisRec.Value;
                mySqlCommandSel.Parameters["@AlturaMorTDC"].Value = Datos.AlturaMorTDC.Value;
                mySqlCommandSel.Parameters["@AlturaMorBAN"].Value = Datos.AlturaMorBAN.Value;
                mySqlCommandSel.Parameters["@AlturaMorCOP"].Value = Datos.AlturaMorCOP.Value;
                mySqlCommandSel.Parameters["@AlturaMorHIP"].Value = Datos.AlturaMorHIP.Value;
                mySqlCommandSel.Parameters["@PeorEndeudT1"].Value = Datos.PeorEndeudT1.Value;
                mySqlCommandSel.Parameters["@PeorEndeudT2"].Value = Datos.PeorEndeudT2.Value;
                mySqlCommandSel.Parameters["@CtasAhorrosAct"].Value = Datos.CtasAhorrosAct.Value;
                mySqlCommandSel.Parameters["@CtasCorrienteAct"].Value = Datos.CtasCorrienteAct.Value;
                mySqlCommandSel.Parameters["@CtasEmbargadas"].Value = Datos.CtasEmbargadas.Value;
                mySqlCommandSel.Parameters["@CtasMalManejo"].Value = Datos.CtasMalManejo.Value;
                mySqlCommandSel.Parameters["@CtasSaldadas"].Value = Datos.CtasSaldadas.Value;
                mySqlCommandSel.Parameters["@UltConsultas"].Value = Datos.UltConsultas.Value;
                mySqlCommandSel.Parameters["@EstadoConsulta"].Value = Datos.EstadoConsulta.Value;

                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                //Eg
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoDatos_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDataCreditoDatos
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccSolicitudDataCreditoDatos_Update(DTO_ccSolicitudDataCreditoDatos Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                           "UPDATE ccSolicitudDataCreditoDatos SET  " +
                                                " Version=@Version" +
                                                " ,TipoId=@TipoId" +
                                                " ,NumeroId=@NumeroId" +
                                                " ,EstadoDoc=@EstadoDoc" +
                                                " ,Nombre=@Nombre" +
                                                " ,RangoEdad=@RangoEdad" +
                                                " ,Genero=@Genero" +
                                                " ,CiudadExp=@CiudadExp" +
                                                " ,FechaAct=@FechaAct" +
                                                " ,ActEconomica=@ActEconomica" +
                                                " ,RUT=@RUT" +
                                                " ,TipoContrato=@TipoContrato" +
                                                " ,FechaContrato=@FechaContrato" +
                                                " ,NumeObligACT=@NumeObligACT" +
                                                " ,NumObligaBAN=@NumObligaBAN" +
                                                " ,VlrInicialBAN=@VlrInicialBAN" +
                                                " ,VlrSaldoBAN=@VlrSaldoBAN" +
                                                " ,VlrCuotasBAN=@VlrCuotasBAN" +
                                                " ,VlrMoraBAN=@VlrMoraBAN" +
                                                " ,NumObligaVIV=@NumObligaVIV" +
                                                " ,VlrInicialVIV=@VlrInicialVIV" +
                                                " ,VlrSaldoVIV=@VlrSaldoVIV" +
                                                " ,VlrCuotasVIV=@VlrCuotasVIV" +
                                                " ,VlrMoraVIV=@VlrMoraVIV" +
                                                " ,NumObligaFIN=@NumObligaFIN" +
                                                " ,VlrInicialFIN=@VlrInicialFIN" +
                                                " ,VlrSaldoFIN=@VlrSaldoFIN" +
                                                " ,VlrCuotasFIN=@VlrCuotasFIN" +
                                                " ,VlrMoraFIN=@VlrMoraFIN" +
                                                " ,NumeroTDC=@NumeroTDC" +
                                                " ,VlrCuposTDC=@VlrCuposTDC" +
                                                " ,VlrUtilizadoTDC=@VlrUtilizadoTDC" +
                                                " ,PorUtilizaTDC=@PorUtilizaTDC" +
                                                " ,VlrCuotasTDC=@VlrCuotasTDC" +
                                                " ,VlrMoraTDC=@VlrMoraTDC" +
                                                " ,Rango0TDC=@Rango0TDC" +
                                                " ,Rango1TDC=@Rango1TDC" +
                                                " ,Rango2TDC=@Rango2TDC" +
                                                " ,Rango3TDC=@Rango3TDC" +
                                                " ,Rango4TDC=@Rango4TDC" +
                                                " ,Rango5TDC=@Rango5TDC" +
                                                " ,Rango6TDC=@Rango6TDC" +
                                                " ,FchAntiguaTDC=@FchAntiguaTDC" +
                                                " ,NumObligaREA=@NumObligaREA" +
                                                " ,VlrInicialREA=@VlrInicialREA" +
                                                " ,VlrSaldoREA=@VlrSaldoREA" +
                                                " ,VlrCuotasREA=@VlrCuotasREA" +
                                                " ,VlrMoraREA=@VlrMoraREA" +
                                                " ,NumObligaCEL=@NumObligaCEL" +
                                                " ,VlrCuotasCEL=@VlrCuotasCEL" +
                                                " ,VlrMoraCEL=@VlrMoraCEL" +
                                                " ,NumObligaCOP=@NumObligaCOP" +
                                                " ,VlrInicialCOP=@VlrInicialCOP" +
                                                " ,VlrSaldoCOP=@VlrSaldoCOP" +
                                                " ,VlrCuotasCOP=@VlrCuotasCOP" +
                                                " ,VlrMoraCOP=@VlrMoraCOP" +
                                                " ,NumObligaCOD=@NumObligaCOD" +
                                                " ,VlrSaldoCOD=@VlrSaldoCOD" +
                                                " ,VlrCuotasCOD=@VlrCuotasCOD" +
                                                " ,VlrMoraCOD=@VlrMoraCOD" +
                                                " ,EstadoActDia=@EstadoActDia" +
                                                " ,EstadoAct30=@EstadoAct30" +
                                                " ,EstadoAct60=@EstadoAct60" +
                                                " ,EstadoAct90=@EstadoAct90" +
                                                " ,EstadoAct120=@EstadoAct120" +
                                                " ,EstadoActCas=@EstadoActCas" +
                                                " ,EstadoActDud=@EstadoActDud" +
                                                " ,EstadoActCob=@EstadoActCob" +
                                                " ,EstadoHis30=@EstadoHis30" +
                                                " ,EstadoHis60=@EstadoHis60" +
                                                " ,EstadoHis90=@EstadoHis90" +
                                                " ,EstadoHis120=@EstadoHis120" +
                                                " ,EstadoHisCan=@EstadoHisCan" +
                                                " ,EstadoHisRec=@EstadoHisRec" +
                                                " ,AlturaMorTDC=@AlturaMorTDC" +
                                                " ,AlturaMorBAN=@AlturaMorBAN" +
                                                " ,AlturaMorCOP=@AlturaMorCOP" +
                                                " ,AlturaMorHIP=@AlturaMorHIP" +
                                                " ,PeorEndeudT1=@PeorEndeudT1" +
                                                " ,PeorEndeudT2=@PeorEndeudT2" +
                                                " ,CtasAhorrosAct=@CtasAhorrosAct" +
                                                " ,CtasCorrienteAct=@CtasCorrienteAct" +
                                                " ,CtasEmbargadas=@CtasEmbargadas" +
                                                " ,CtasMalManejo=@CtasMalManejo" +
                                                " ,CtasSaldadas=@CtasSaldadas" +
                                                " ,UltConsultas=@UltConsultas" +
                                                " ,EstadoConsulta=@EstadoConsulta" +
                                                " WHERE Consecutivo = @Consecutivo";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoId", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@NumeroId", SqlDbType.Char, 11);
                mySqlCommandSel.Parameters.Add("@EstadoDoc", SqlDbType.Char, 11);
                mySqlCommandSel.Parameters.Add("@Nombre", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@RangoEdad", SqlDbType.Char, 5);
                mySqlCommandSel.Parameters.Add("@Genero", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@CiudadExp", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@FechaAct", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ActEconomica", SqlDbType.Char, 2);
                mySqlCommandSel.Parameters.Add("@RUT", SqlDbType.Char, 2);
                mySqlCommandSel.Parameters.Add("@TipoContrato", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@FechaContrato", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumeObligACT", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@NumObligaBAN", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrInicialBAN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoBAN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotasBAN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraBAN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumObligaVIV", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrInicialVIV", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoVIV", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotasVIV", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraVIV", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumObligaFIN", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrInicialFIN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoFIN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotasFIN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraFIN", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroTDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrCuposTDC", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrUtilizadoTDC", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PorUtilizaTDC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCuotasTDC", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraTDC", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Rango0TDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Rango1TDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Rango2TDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Rango3TDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Rango4TDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Rango5TDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Rango6TDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FchAntiguaTDC", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumObligaREA", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrInicialREA", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoREA", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotasREA", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraREA", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumObligaCEL", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrCuotasCEL", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraCEL", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumObligaCOP", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrInicialCOP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCOP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotasCOP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraCOP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumObligaCOD", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCOD", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotasCOD", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraCOD", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EstadoActDia", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoAct30", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoAct60", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoAct90", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoAct120", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoActCas", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoActDud", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoActCob", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoHis30", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoHis60", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoHis90", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoHis120", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoHisCan", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoHisRec", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AlturaMorTDC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AlturaMorBAN", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AlturaMorCOP", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AlturaMorHIP", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PeorEndeudT1", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@PeorEndeudT2", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@CtasAhorrosAct", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CtasCorrienteAct", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CtasEmbargadas", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CtasMalManejo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CtasSaldadas", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@UltConsultas", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoConsulta", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);


                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                mySqlCommandSel.Parameters["@TipoId"].Value = Datos.TipoId.Value;
                mySqlCommandSel.Parameters["@NumeroId"].Value = Datos.NumeroId.Value;
                mySqlCommandSel.Parameters["@EstadoDoc"].Value = Datos.EstadoDoc.Value;
                mySqlCommandSel.Parameters["@Nombre"].Value = Datos.Nombre.Value;
                mySqlCommandSel.Parameters["@RangoEdad"].Value = Datos.RangoEdad.Value;
                mySqlCommandSel.Parameters["@Genero"].Value = Datos.Genero.Value;
                mySqlCommandSel.Parameters["@CiudadExp"].Value = Datos.CiudadExp.Value;
                mySqlCommandSel.Parameters["@FechaAct"].Value = Datos.FechaAct.Value;
                mySqlCommandSel.Parameters["@ActEconomica"].Value = Datos.ActEconomica.Value;
                mySqlCommandSel.Parameters["@RUT"].Value = Datos.RUT.Value;
                mySqlCommandSel.Parameters["@TipoContrato"].Value = Datos.TipoContrato.Value;
                mySqlCommandSel.Parameters["@FechaContrato"].Value = Datos.FechaContrato.Value;
                mySqlCommandSel.Parameters["@NumeObligACT"].Value = Datos.NumeObligACT.Value;
                mySqlCommandSel.Parameters["@NumObligaBAN"].Value = Datos.NumObligaBAN.Value;
                mySqlCommandSel.Parameters["@VlrInicialBAN"].Value = Datos.VlrInicialBAN.Value;
                mySqlCommandSel.Parameters["@VlrSaldoBAN"].Value = Datos.VlrSaldoBAN.Value;
                mySqlCommandSel.Parameters["@VlrCuotasBAN"].Value = Datos.VlrCuotasBAN.Value;
                mySqlCommandSel.Parameters["@VlrMoraBAN"].Value = Datos.VlrMoraBAN.Value;
                mySqlCommandSel.Parameters["@NumObligaVIV"].Value = Datos.NumObligaVIV.Value;
                mySqlCommandSel.Parameters["@VlrInicialVIV"].Value = Datos.VlrInicialVIV.Value;
                mySqlCommandSel.Parameters["@VlrSaldoVIV"].Value = Datos.VlrSaldoVIV.Value;
                mySqlCommandSel.Parameters["@VlrCuotasVIV"].Value = Datos.VlrCuotasVIV.Value;
                mySqlCommandSel.Parameters["@VlrMoraVIV"].Value = Datos.VlrMoraVIV.Value;
                mySqlCommandSel.Parameters["@NumObligaFIN"].Value = Datos.NumObligaFIN.Value;
                mySqlCommandSel.Parameters["@VlrInicialFIN"].Value = Datos.VlrInicialFIN.Value;
                mySqlCommandSel.Parameters["@VlrSaldoFIN"].Value = Datos.VlrSaldoFIN.Value;
                mySqlCommandSel.Parameters["@VlrCuotasFIN"].Value = Datos.VlrCuotasFIN.Value;
                mySqlCommandSel.Parameters["@VlrMoraFIN"].Value = Datos.VlrMoraFIN.Value;
                mySqlCommandSel.Parameters["@NumeroTDC"].Value = Datos.NumeroTDC.Value;
                mySqlCommandSel.Parameters["@VlrCuposTDC"].Value = Datos.VlrCuposTDC.Value;
                mySqlCommandSel.Parameters["@VlrUtilizadoTDC"].Value = Datos.VlrUtilizadoTDC.Value;
                mySqlCommandSel.Parameters["@PorUtilizaTDC"].Value = Datos.PorUtilizaTDC.Value;
                mySqlCommandSel.Parameters["@VlrCuotasTDC"].Value = Datos.VlrCuotasTDC.Value;
                mySqlCommandSel.Parameters["@VlrMoraTDC"].Value = Datos.VlrMoraTDC.Value;
                mySqlCommandSel.Parameters["@Rango0TDC"].Value = Datos.Rango0TDC.Value;
                mySqlCommandSel.Parameters["@Rango1TDC"].Value = Datos.Rango1TDC.Value;
                mySqlCommandSel.Parameters["@Rango2TDC"].Value = Datos.Rango2TDC.Value;
                mySqlCommandSel.Parameters["@Rango3TDC"].Value = Datos.Rango3TDC.Value;
                mySqlCommandSel.Parameters["@Rango4TDC"].Value = Datos.Rango4TDC.Value;
                mySqlCommandSel.Parameters["@Rango5TDC"].Value = Datos.Rango5TDC.Value;
                mySqlCommandSel.Parameters["@Rango6TDC"].Value = Datos.Rango6TDC.Value;
                mySqlCommandSel.Parameters["@FchAntiguaTDC"].Value = Datos.FchAntiguaTDC.Value;
                mySqlCommandSel.Parameters["@NumObligaREA"].Value = Datos.NumObligaREA.Value;
                mySqlCommandSel.Parameters["@VlrInicialREA"].Value = Datos.VlrInicialREA.Value;
                mySqlCommandSel.Parameters["@VlrSaldoREA"].Value = Datos.VlrSaldoREA.Value;
                mySqlCommandSel.Parameters["@VlrCuotasREA"].Value = Datos.VlrCuotasREA.Value;
                mySqlCommandSel.Parameters["@VlrMoraREA"].Value = Datos.VlrMoraREA.Value;
                mySqlCommandSel.Parameters["@NumObligaCEL"].Value = Datos.NumObligaCEL.Value;
                mySqlCommandSel.Parameters["@VlrCuotasCEL"].Value = Datos.VlrCuotasCEL.Value;
                mySqlCommandSel.Parameters["@VlrMoraCEL"].Value = Datos.VlrMoraCEL.Value;
                mySqlCommandSel.Parameters["@NumObligaCOP"].Value = Datos.NumObligaCOP.Value;
                mySqlCommandSel.Parameters["@VlrInicialCOP"].Value = Datos.VlrInicialCOP.Value;
                mySqlCommandSel.Parameters["@VlrSaldoCOP"].Value = Datos.VlrSaldoCOP.Value;
                mySqlCommandSel.Parameters["@VlrCuotasCOP"].Value = Datos.VlrCuotasCOP.Value;
                mySqlCommandSel.Parameters["@VlrMoraCOP"].Value = Datos.VlrMoraCOP.Value;
                mySqlCommandSel.Parameters["@NumObligaCOD"].Value = Datos.NumObligaCOD.Value;
                mySqlCommandSel.Parameters["@VlrSaldoCOD"].Value = Datos.VlrSaldoCOD.Value;
                mySqlCommandSel.Parameters["@VlrCuotasCOD"].Value = Datos.VlrCuotasCOD.Value;
                mySqlCommandSel.Parameters["@VlrMoraCOD"].Value = Datos.VlrMoraCOD.Value;
                mySqlCommandSel.Parameters["@EstadoActDia"].Value = Datos.EstadoActDia.Value;
                mySqlCommandSel.Parameters["@EstadoAct30"].Value = Datos.EstadoAct30.Value;
                mySqlCommandSel.Parameters["@EstadoAct60"].Value = Datos.EstadoAct60.Value;
                mySqlCommandSel.Parameters["@EstadoAct90"].Value = Datos.EstadoAct90.Value;
                mySqlCommandSel.Parameters["@EstadoAct120"].Value = Datos.EstadoAct120.Value;
                mySqlCommandSel.Parameters["@EstadoActCas"].Value = Datos.EstadoActCas.Value;
                mySqlCommandSel.Parameters["@EstadoActDud"].Value = Datos.EstadoActDud.Value;
                mySqlCommandSel.Parameters["@EstadoActCob"].Value = Datos.EstadoActCob.Value;
                mySqlCommandSel.Parameters["@EstadoHis30"].Value = Datos.EstadoHis30.Value;
                mySqlCommandSel.Parameters["@EstadoHis60"].Value = Datos.EstadoHis60.Value;
                mySqlCommandSel.Parameters["@EstadoHis90"].Value = Datos.EstadoHis90.Value;
                mySqlCommandSel.Parameters["@EstadoHis120"].Value = Datos.EstadoHis120.Value;
                mySqlCommandSel.Parameters["@EstadoHisCan"].Value = Datos.EstadoHisCan.Value;
                mySqlCommandSel.Parameters["@EstadoHisRec"].Value = Datos.EstadoHisRec.Value;
                mySqlCommandSel.Parameters["@AlturaMorTDC"].Value = Datos.AlturaMorTDC.Value;
                mySqlCommandSel.Parameters["@AlturaMorBAN"].Value = Datos.AlturaMorBAN.Value;
                mySqlCommandSel.Parameters["@AlturaMorCOP"].Value = Datos.AlturaMorCOP.Value;
                mySqlCommandSel.Parameters["@AlturaMorHIP"].Value = Datos.AlturaMorHIP.Value;
                mySqlCommandSel.Parameters["@PeorEndeudT1"].Value = Datos.PeorEndeudT1.Value;
                mySqlCommandSel.Parameters["@PeorEndeudT2"].Value = Datos.PeorEndeudT2.Value;
                mySqlCommandSel.Parameters["@CtasAhorrosAct"].Value = Datos.CtasAhorrosAct.Value;
                mySqlCommandSel.Parameters["@CtasCorrienteAct"].Value = Datos.CtasCorrienteAct.Value;
                mySqlCommandSel.Parameters["@CtasEmbargadas"].Value = Datos.CtasEmbargadas.Value;
                mySqlCommandSel.Parameters["@CtasMalManejo"].Value = Datos.CtasMalManejo.Value;
                mySqlCommandSel.Parameters["@CtasSaldadas"].Value = Datos.CtasSaldadas.Value;
                mySqlCommandSel.Parameters["@UltConsultas"].Value = Datos.UltConsultas.Value;
                mySqlCommandSel.Parameters["@EstadoConsulta"].Value = Datos.EstadoConsulta.Value;

                //mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = Datos.Consecutivo.Value;
                //Eg
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoDatos_Update");
                throw exception;
            }
        }

        public void DAL_ccSolicitudDataCreditoDatos_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandText = "DELETE FROM ccSolicitudDataCreditoDatos WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoDatos_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto</returns>
        public bool DAL_ccSolicitudDataCreditoDatos_Exist(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from ccSolicitudDataCreditoDatos with(nolock) where Consecutivo = @Consecutivo";

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consec;

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoDatos_Exist");
                throw exception;
            }
        }
        #endregion

       
    }
}
