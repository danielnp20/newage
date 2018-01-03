using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    public class DAL_cpAnticipos : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_cpAnticipos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Obtiene la informacion de cpAnticipos por numero de documento
        /// </summary>
        /// <param name="numeroDoc">consecutivo numDoc</param>
        /// <returns></returns>
        public DTO_cpAnticipo DAL_cpAnticipos_GetByEstado(int numeroDoc, EstadoDocControl estado)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@Estado"].Value = (Int16)estado;

                mySqlCommand.CommandText =
                    "select ant.* from cpAnticipo ant with(nolock) " +
                    "	inner join glDocumentoControl ctrl with(nolock) on ant.NumeroDoc = ctrl.NumeroDoc " +
                    "		and ctrl.Estado = @Estado " +
                    "where ctrl.NumeroDoc = @NumeroDoc";

                DTO_cpAnticipo result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                    result = new DTO_cpAnticipo(dr);

                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpAnticipo_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega anticipo
        /// </summary>
        /// <param name="_anticipo">informacion del anticipo</param>
        /// <returns></returns>
        public void DAL_cpAnticipos_Add(DTO_cpAnticipo _anticipo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                               "   INSERT INTO [cpAnticipo]    " +
                               "    ([EmpresaID]    " +
                               "    ,[NumeroDoc]    " +
                               "    ,[RadicaFecha]    " +
                               "    ,[AnticipoTipoID]    " +
                               "    ,[Plazo]    " +
                               "    ,[ConceptoCxPID]    " +
                               "    ,[NumeroDocCXP]    " +
                               "    ,[FechaSalida]    " +
                               "    ,[FechaRetorno]    " +
                               "    ,[TipoViaje]    " +
                               "    ,[DiasAlojamiento]    " +
                               "    ,[Valor]    " +
                               "    ,[ValorAlojamiento]    " +
                               "    ,[DiasAlimentacion]    " +
                               "    ,[ValorAlimentacion]    " +
                               "    ,[DiasTransporte]    " +
                               "    ,[ValorTransporte]    " +
                               "    ,[DiasOtrosGastos]    " +
                               "    ,[ValorOtrosGastos]    " +
                               "    ,[ValorTiquetes]    " +
                               "    ,[eg_cpAnticipoTipo]    " +
                               "    ,[eg_cpConceptoCXP])    " +
                               "    VALUES    " +
                               "    (  @EmpresaID  " +
                               "    ,  @NumeroDoc  " +
                               "    ,  @RadicaFecha  " +
                               "    ,  @AnticipoTipoID  " +
                               "    ,  @Plazo  " +
                               "    ,  @ConceptoCxPID  " +
                               "    ,  @NumeroDocCXP  " +
                               "    ,  @FechaSalida  " +
                               "    ,  @FechaRetorno  " +
                               "    ,  @TipoViaje  " +
                               "    ,  @DiasAlojamiento  " +
                               "    ,  @Valor  " +
                               "    ,  @ValorAlojamiento  " +
                               "    ,  @DiasAlimentacion  " +
                               "    ,  @ValorAlimentacion  " +
                               "    ,  @DiasTransporte  " +
                               "    ,  @ValorTransporte  " +
                               "    ,  @DiasOtrosGastos  " +
                               "    ,  @ValorOtrosGastos  " +
                               "    ,  @ValorTiquetes  " +
                               "    ,  @eg_cpAnticipoTipo  " +
                               "    ,  @eg_cpConceptoCXP)  ";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@RadicaFecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@AnticipoTipoID", SqlDbType.Char, UDT_AnticipoTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@Plazo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ConceptoCxPID", SqlDbType.Char, UDT_ConceptoCxPID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDocCXP", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaSalida", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaRetorno", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@TipoViaje", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@DiasAlojamiento", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorAlojamiento", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DiasAlimentacion", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ValorAlimentacion", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DiasTransporte", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ValorTransporte", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DiasOtrosGastos", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ValorOtrosGastos", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorTiquetes", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_cpAnticipoTipo", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_cpConceptoCXP", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = _anticipo.NumeroDoc.Value;
                mySqlCommand.Parameters["@RadicaFecha"].Value = _anticipo.RadicaFecha.Value;
                mySqlCommand.Parameters["@AnticipoTipoID"].Value = _anticipo.AnticipoTipoID.Value;
                mySqlCommand.Parameters["@Plazo"].Value = _anticipo.Plazo.Value;
                mySqlCommand.Parameters["@ConceptoCxPID"].Value = _anticipo.ConceptoCxPID.Value;
                mySqlCommand.Parameters["@NumeroDocCXP"].Value = _anticipo.NumeroDocCXP.Value;
                mySqlCommand.Parameters["@FechaSalida"].Value = _anticipo.FechaSalida.Value;
                mySqlCommand.Parameters["@FechaRetorno"].Value = _anticipo.FechaRetorno.Value;
                mySqlCommand.Parameters["@TipoViaje"].Value = _anticipo.TipoViaje.Value;
                mySqlCommand.Parameters["@DiasAlojamiento"].Value = _anticipo.DiasAlojamiento.Value;
                mySqlCommand.Parameters["@Valor"].Value = _anticipo.Valor.Value;
                mySqlCommand.Parameters["@ValorAlojamiento"].Value = _anticipo.ValorAlojamiento.Value;
                mySqlCommand.Parameters["@DiasAlimentacion"].Value = _anticipo.DiasAlojamiento.Value;
                mySqlCommand.Parameters["@ValorAlimentacion"].Value = _anticipo.ValorAlimentacion.Value;
                mySqlCommand.Parameters["@DiasTransporte"].Value = _anticipo.DiasTransporte.Value;
                mySqlCommand.Parameters["@ValorTransporte"].Value = _anticipo.ValorTransporte.Value;
                mySqlCommand.Parameters["@DiasOtrosGastos"].Value = _anticipo.DiasOtrosGastos.Value;
                mySqlCommand.Parameters["@ValorOtrosGastos"].Value = _anticipo.ValorOtrosGastos.Value;
                mySqlCommand.Parameters["@ValorTiquetes"].Value = _anticipo.ValorTiquetes.Value;
                mySqlCommand.Parameters["@eg_cpAnticipoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cpAnticipoTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_cpConceptoCXP"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cpConceptoCXP, this.Empresa, egCtrl);
                #endregion

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpAnticipos_Add");
                throw exception;                
            }
        }

        /// <summary>
        /// Actualiza Anticipo
        /// </summary>
        /// <param name="_anticipo">informacion del anticipo</param>
        /// <returns></returns>
        public void DAL_cpAnticipos_Upd(DTO_cpAnticipo _anticipo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    UPDATE [cpAnticipo] " +
                                              "    SET [EmpresaID] = @EmpresaID " +
                                              "    ,[NumeroDoc] = @NumeroDoc " +
                                              "    ,[RadicaFecha] = @RadicaFecha " +
                                              "    ,[AnticipoTipoID] = @AnticipoTipoID " +
                                              "    ,[Plazo] = @Plazo " +
                                              "    ,[ConceptoCxPID] = @ConceptoCxPID    " +
                                              "    ,[NumeroDocCXP] = @NumeroDocCXP  " +
                                              "    ,[FechaSalida] = @FechaSalida    " +
                                              "    ,[FechaRetorno] =  @FechaRetorno  " +
                                              "    ,[TipoViaje] = @TipoViaje    " +
                                              "    ,[DiasAlojamiento] =  @DiasAlojamiento   " +
                                              "    ,[Valor] =  @Valor   " +
                                              "    ,[ValorAlojamiento] =  @ValorAlojamiento " +
                                              "    ,[DiasAlimentacion] =  @DiasAlimentacion " +
                                              "    ,[ValorAlimentacion] = @ValorAlimentacion " +
                                              "    ,[DiasTransporte] =  @DiasTransporte   " +
                                              "    ,[ValorTransporte] =  @ValorTransporte " +
                                              "    ,[DiasOtrosGastos] =  @DiasOtrosGastos   " +
                                              "    ,[ValorOtrosGastos] = @ValorOtrosGastos  " +
                                              "    ,[ValorTiquetes] =  @ValorTiquetes  " +
                                              "    ,[eg_cpAnticipoTipo] = @eg_cpAnticipoTipo " +
                                              "    ,[eg_cpConceptoCXP] = @eg_cpConceptoCXP " +
                                              "    WHERE NumeroDoc = @NumeroDoc ";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@RadicaFecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@AnticipoTipoID", SqlDbType.Char, UDT_AnticipoTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@Plazo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ConceptoCxPID", SqlDbType.Char, UDT_ConceptoCxPID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDocCXP", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaSalida", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaRetorno", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@TipoViaje", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@DiasAlojamiento", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorAlojamiento", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DiasAlimentacion", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ValorAlimentacion", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DiasTransporte", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ValorTransporte", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DiasOtrosGastos", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ValorOtrosGastos", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorTiquetes", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_cpAnticipoTipo", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_cpConceptoCXP", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = _anticipo.NumeroDoc.Value;
                mySqlCommand.Parameters["@RadicaFecha"].Value = _anticipo.RadicaFecha.Value;
                mySqlCommand.Parameters["@AnticipoTipoID"].Value = _anticipo.AnticipoTipoID.Value;
                mySqlCommand.Parameters["@Plazo"].Value = _anticipo.Plazo.Value;
                mySqlCommand.Parameters["@ConceptoCxPID"].Value = _anticipo.ConceptoCxPID.Value;
                mySqlCommand.Parameters["@NumeroDocCXP"].Value = _anticipo.NumeroDocCXP.Value;
                mySqlCommand.Parameters["@FechaSalida"].Value = _anticipo.FechaSalida.Value;
                mySqlCommand.Parameters["@FechaRetorno"].Value = _anticipo.FechaRetorno.Value;
                mySqlCommand.Parameters["@TipoViaje"].Value = _anticipo.TipoViaje.Value;
                mySqlCommand.Parameters["@DiasAlojamiento"].Value = _anticipo.DiasAlojamiento.Value;
                mySqlCommand.Parameters["@Valor"].Value = _anticipo.Valor.Value;
                mySqlCommand.Parameters["@ValorAlojamiento"].Value = _anticipo.ValorAlojamiento.Value;
                mySqlCommand.Parameters["@DiasAlimentacion"].Value = _anticipo.DiasAlojamiento.Value;
                mySqlCommand.Parameters["@ValorAlimentacion"].Value = _anticipo.ValorAlimentacion.Value;
                mySqlCommand.Parameters["@DiasTransporte"].Value = _anticipo.DiasTransporte.Value;
                mySqlCommand.Parameters["@ValorTransporte"].Value = _anticipo.ValorTransporte.Value;
                mySqlCommand.Parameters["@DiasOtrosGastos"].Value = _anticipo.DiasOtrosGastos.Value;
                mySqlCommand.Parameters["@ValorOtrosGastos"].Value = _anticipo.ValorOtrosGastos.Value;
                mySqlCommand.Parameters["@ValorTiquetes"].Value = _anticipo.ValorTiquetes.Value;
                mySqlCommand.Parameters["@eg_cpAnticipoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cpAnticipoTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_cpConceptoCXP"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cpConceptoCXP, this.Empresa, egCtrl);
                #endregion

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpAnticipos_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna el valor total para una lista de anticipos 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tm">Tipo de moneda sobre el cual estan viendo los anticipos</param>
        /// <param name="tc">Tasa de cambio en el dia</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna el valor total de los anticipos</returns>
        public decimal DAL_cpAnticipos_GetResumenVal(DateTime periodo, TipoMoneda tm, decimal tc, string terceroID)
        {
            try
            {
                byte tmDoc = (byte)tm;
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = AppDocuments.Anticipos;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;
                mySqlCommand.Parameters["@TerceroID"].Value = terceroID;
               
                string col = tm == TipoMoneda.Local ? "ML" : "ME";

                mySqlCommand.CommandText =
                    "select * from " +
                    "( " +
                    "	select cta.OrigenMonetario," +
                    "		( " +
                    "			saldo.DbOrigenLocML + saldo.DbOrigenExtML + saldo.CrOrigenLocML + saldo.CrOrigenExtML + " +
                    "			saldo.DbSaldoIniLocML + saldo.DbSaldoIniExtML + saldo.CrSaldoIniLocML + saldo.CrSaldoIniExtML " +
                    "		) as ML, " +
                    "		( " +
                    "			saldo.DbOrigenLocME + saldo.DbOrigenExtME + saldo.CrOrigenLocME + saldo.CrOrigenExtME + " +
                    "			saldo.DbSaldoIniLocME + saldo.DbSaldoIniExtME + saldo.CrSaldoIniLocME + saldo.CrSaldoIniExtME	 " +
                    "		) as ME " +
                    "	from glDocumentoControl doc with(nolock) " +
                    "		inner join coCuentaSaldo saldo with(nolock) on doc.NumeroDoc = saldo.IdentificadorTR " +
                    "           and doc.CuentaID = saldo.CuentaID " +
                    "		inner join coPlanCuenta cta with(nolock) on saldo.CuentaID = cta.CuentaID " +
                    "	where doc.EmpresaID = @EmpresaID and doc.DocumentoID = @DocumentoID and saldo.PeriodoID = @PeriodoDoc" +
                    "       and saldo.TerceroID = @TerceroID " +
                    ") as res " +
                    "where ML <> 0 and ME <> 0  ";

                decimal res = 0;
                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    byte tmAnt = Convert.ToByte(dr["OrigenMonetario"]);
                    decimal ml = Convert.ToDecimal(dr["ML"]);
                    decimal me = Convert.ToDecimal(dr["ME"]);

                    if (tm == TipoMoneda.Local)
                    {
                        if (tmAnt != tmDoc)
                            ml = me * tc;
                        res += Math.Round(ml, 2);
                    }
                    else
                    {
                        if (tmAnt != tmDoc)
                            me =  ml / tc;
                        res += Math.Round(me, 2);
                    }
                }
                dr.Close();

                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpAnticipos_GetResumenVal");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna una lista de anticipos 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoMoneda">Tipo de moneda de los cuales hay que traer los anticipos</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <param name="anticipoTarjeta">Indica si es anticipo de tarjeta de credito</param>
        /// <returns>Retorna una lista de anticipos</returns>
        public List<DTO_AnticiposResumen> DAL_cpAnticipos_GetResumen(DateTime periodo, TipoMoneda tipoMoneda, string terceroID, bool anticipoTarjeta)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = AppDocuments.Anticipos;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;
                mySqlCommand.Parameters["@TerceroID"].Value = terceroID;
               
                string tipoAnticipo = string.Empty;
                if(anticipoTarjeta)
                    tipoAnticipo = " and anticipoTipo.AnticipoTipoID = 2";

                mySqlCommand.CommandText =
                    "select * from " +
                    "( " +
                    "	select distinct doc.DocumentoID, doc.FechaDoc Fecha, doc.PrefijoID, doc.DocumentoNro, doc.MonedaID, cta.OrigenMonetario, " +
                    "		saldo.CuentaID, saldo.TerceroID, doc.DocumentoTercero, saldo.ProyectoID, saldo.CentroCostoID, " +
                    "		saldo.LineaPresupuestoID, saldo.ConceptoSaldoID, saldo.IdentificadorTR, saldo.ConceptoCargoID, " +
                    "		( " +
                    "			saldo.DbOrigenLocML + saldo.DbOrigenExtML + saldo.CrOrigenLocML + saldo.CrOrigenExtML + " +
                    "			saldo.DbSaldoIniLocML + saldo.DbSaldoIniExtML + saldo.CrSaldoIniLocML + saldo.CrSaldoIniExtML " +
                    "		) as ML, " +
                    "		( " +
                    "			saldo.DbOrigenLocME + saldo.DbOrigenExtME + saldo.CrOrigenLocME + saldo.CrOrigenExtME + " +
                    "			saldo.DbSaldoIniLocME + saldo.DbSaldoIniExtME + saldo.CrSaldoIniLocME + saldo.CrSaldoIniExtME	 " +
                    "		) as ME " +
                    "	from glDocumentoControl doc with(nolock) " +
                    "		inner join coCuentaSaldo saldo with(nolock) on doc.NumeroDoc = saldo.IdentificadorTR  and doc.CuentaID = saldo.CuentaID " +
                    "		inner join coPlanCuenta cta with(nolock) on saldo.CuentaID = cta.CuentaID " +
                    "       inner join cpAnticipo anticipo with(nolock) on doc.NumeroDoc = anticipo.NumeroDoc " +
                    "       inner join cpAnticipoTipo anticipoTipo with(nolock) on anticipoTipo.AnticipoTipoID = anticipo.AnticipoTipoID" + tipoAnticipo +
                    "	where doc.EmpresaID = @EmpresaID and doc.DocumentoID = @DocumentoID and saldo.PeriodoID = @PeriodoDoc " +
                    "       and saldo.TerceroID = @TerceroID " +
                    ") as res ";

                if (tipoMoneda == TipoMoneda.Local)
                    mySqlCommand.CommandText += "where ML <> 0 ";
                else if (tipoMoneda == TipoMoneda.Foreign)
                    mySqlCommand.CommandText += "where ME <> 0 ";
                else
                    mySqlCommand.CommandText += "where ML <> 0 and ME <> 0 ";

                mySqlCommand.CommandText += "order by IdentificadorTR ";

                List<DTO_AnticiposResumen> result = new List<DTO_AnticiposResumen>();
                SqlDataReader dr;

                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_AnticiposResumen ant = new DTO_AnticiposResumen(dr);
                    result.Add(ant);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpAnticipos_GetResumen");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de anticipos pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_AnticipoAprobacion> DAL_cpAnticipos_GetPendientesByModulo(ModulesPrefix mod, string actividadFlujoID, string usuarioID)
        {
            try
            {
                List<DTO_AnticipoAprobacion> result = new List<DTO_AnticipoAprobacion>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select distinct ctrl.*, ctrl.ComprobanteIDNro as ComprobanteNro, ctrl.PeriodoDoc as PeriodoID, " +
                    "	usr.UsuarioID as UsuarioID, ant.Valor as Valor,ant.AnticipoTipoID, ter.Descriptivo DescriptivoTercero " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "   inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "	    and act.CerradoInd=@CerradoInd and act.ActividadFlujoID=@ActividadFlujoID " +
                    "	inner join cpAnticipo ant with(nolock) on ant.NumeroDoc = ctrl.NumeroDoc " +
                    "	inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                    "	inner join coTercero ter with(nolock) on ctrl.TerceroID = ter.TerceroID " +
                    "	inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID " +
                    "	inner join glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    "       and perm.UsuarioID = @UsuarioID and Perm.AreaFuncionalID = Ctrl.AreaFuncionalID " +
                    "where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado and perm.ActividadFlujoID = @ActividadFlujoID";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.ParaAprobacion;
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuarioID;

                SqlDataReader dr;

                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_AnticipoAprobacion dto = new DTO_AnticipoAprobacion(dr);
                    dto.Descripcion.Value = dr["Observacion"].ToString();
                    dto.AnticipoTipoID.Value = dr["AnticipoTipoID"].ToString();
                    dto.Aprobado.Value = false;
                    dto.Rechazado.Value = false;
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpAnticipos_GetPendientesByModulo");
                throw exception;
            }
        }

    }
      
}
