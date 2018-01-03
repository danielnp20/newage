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
    public class DAL_cpTarjetaDocu : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_cpTarjetaDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Obtiene la informacion de cpTarjetaDocu por numero de documento
        /// </summary>
        /// <param name="numeroDoc">consecutivo numDoc</param>
        /// <returns></returns>
        public DTO_cpTarjetaDocu DAL_cpTarjetaDocu_GetByEstado(int numeroDoc, EstadoDocControl estado)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                string query = string.Empty;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                if (estado != null)
                {
                    mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@Estado"].Value = (Int16)estado;
                    query = "and ctrl.Estado = @Estado";
                }
                mySqlCommand.CommandText =
                    "select ant.* from cpTarjetaDocu ant with(nolock) " +
                    "	inner join glDocumentoControl ctrl with(nolock) on ant.NumeroDoc = ctrl.NumeroDoc " + query +
                    "   where ctrl.NumeroDoc = @NumeroDoc";

                DTO_cpTarjetaDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                    result = new DTO_cpTarjetaDocu(dr);

                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpTarjetaDocu_GetByEstado");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega tarjeta pago
        /// </summary>
        /// <param name="tarjetaDocu">informacion de tarjeta pago</param>
        /// <returns></returns>
        public void DAL_cpTarjetaDocu_Add(DTO_cpTarjetaDocu tarjetaDocu)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                               "   INSERT INTO [cpTarjetaDocu]    " +
                               "    ([EmpresaID]    " +
                               "    ,[NumeroDoc]    " +                             
                               "    ,[TarjetaCreditoID]    " +
                               "    ,[PeriodoPago]    " +
                               "    ,[Valor]    " +
                               "    ,[IVA]    " +
                               "    ,[NumeroDocCXP])    " +
                               "    VALUES    " +
                               "    (  @EmpresaID  " +
                               "    ,  @NumeroDoc  " +
                               "    ,  @TarjetaCreditoID  " +
                               "    ,  @PeriodoPago  " +
                               "    ,  @Valor  " +
                               "    ,  @IVA  " +
                               "    ,  @NumeroDocCXP)  ";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TarjetaCreditoID", SqlDbType.Char, UDT_TarjetaCreditoID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoPago", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@NumeroDocCXP", SqlDbType.Int);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = tarjetaDocu.NumeroDoc.Value;
                mySqlCommand.Parameters["@TarjetaCreditoID"].Value = tarjetaDocu.TarjetaCreditoID.Value;
                mySqlCommand.Parameters["@PeriodoPago"].Value = tarjetaDocu.PeriodoPago.Value;
                mySqlCommand.Parameters["@Valor"].Value = tarjetaDocu.Valor.Value;
                mySqlCommand.Parameters["@IVA"].Value = tarjetaDocu.IVA.Value;
                mySqlCommand.Parameters["@NumeroDocCXP"].Value = tarjetaDocu.NumeroDocCXP.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpTarjetaDocu_Add");
                throw exception;                
            }
        }

        /// <summary>
        /// Actualiza cpTarjetaDocu
        /// </summary>
        /// <param name="_anticipo">informacion de la cpTarjetaDocu</param>
        /// <returns></returns>
        public void DAL_cpTarjetaDocu_Upd(DTO_cpTarjetaDocu tarjetaDocu)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    UPDATE [cpTarjetaDocu] " +
                                              "    SET [EmpresaID] = @EmpresaID " +
                                              "    ,[NumeroDoc] = @NumeroDoc " +
                                              "    ,[TarjetaCreditoID] = @TarjetaCreditoID " +
                                              "    ,[PeriodoPago] = @PeriodoPago    " +
                                              "    ,[Valor] =  @Valor   " +
                                              "    ,[IVA] =  @IVA   " +                                             
                                              "    ,[NumeroDocCXP] = @NumeroDocCXP  " +
                                              "    WHERE NumeroDoc = @NumeroDoc ";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TarjetaCreditoID", SqlDbType.Char, UDT_TarjetaCreditoID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoPago", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@NumeroDocCXP", SqlDbType.Int);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = tarjetaDocu.NumeroDoc.Value;
                mySqlCommand.Parameters["@TarjetaCreditoID"].Value = tarjetaDocu.TarjetaCreditoID.Value;
                mySqlCommand.Parameters["@PeriodoPago"].Value = tarjetaDocu.PeriodoPago.Value;
                mySqlCommand.Parameters["@Valor"].Value = tarjetaDocu.Valor.Value;
                mySqlCommand.Parameters["@IVA"].Value = tarjetaDocu.IVA.Value;
                mySqlCommand.Parameters["@NumeroDocCXP"].Value = tarjetaDocu.NumeroDocCXP.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpTarjetaDocu_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna el valor total para una lista de cpTarjetaDocu 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tm">Tipo de moneda sobre el cual estan viendo las Tarjetas Docu</param>
        /// <param name="tc">Tasa de cambio en el dia</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna el valor total de las TarjetaDocu</returns>
        public decimal DAL_cpTarjetaDocu_GetResumenVal(DateTime periodo, TipoMoneda tm, decimal tc, string terceroID)
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpTarjetaDocu_GetResumenVal");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna una lista de cpTarjetaDocu 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoMoneda">Tipo de moneda de los cuales hay que traer las cpTarjetaDocu</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna una lista de cpTarjetaDocu</returns>
        public List<DTO_AnticiposResumen> DAL_cpTarjetaDocu_GetResumen(DateTime periodo, TipoMoneda tipoMoneda, string terceroID)
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
               
                mySqlCommand.CommandText =
                    "select * from " +
                    "( " +
                    "	select doc.DocumentoID, doc.FechaDoc Fecha, doc.PrefijoID, doc.DocumentoNro, doc.MonedaID, cta.OrigenMonetario, " +
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
                    "		inner join coCuentaSaldo saldo with(nolock) on doc.NumeroDoc = saldo.IdentificadorTR " +
                    "           and doc.CuentaID = saldo.CuentaID " +
                    "		inner join coPlanCuenta cta with(nolock) on saldo.CuentaID = cta.CuentaID " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpTarjetaDocu_GetResumen");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de cpTarjetaDocu pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_AnticipoAprobacion> DAL_cpTarjetaDocu_GetPendientesByModulo(ModulesPrefix mod, string actividadFlujoID, string usuarioID)
        {
            try
            {
                List<DTO_AnticipoAprobacion> result = new List<DTO_AnticipoAprobacion>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select distinct ctrl.*, ctrl.ComprobanteIDNro as ComprobanteNro, ctrl.PeriodoDoc as PeriodoID, " +
                    "	usr.UsuarioID as UsuarioID, tar.Valor as Valor, tar.TarjetaCreditoID, ter.Descriptivo DescriptivoTercero " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "   inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "	    and act.CerradoInd=@CerradoInd and act.ActividadFlujoID=@ActividadFlujoID " +
                    "	inner join cpTarjetaDocu tar with(nolock) on tar.NumeroDoc = ctrl.NumeroDoc " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpTarjetaDocu_GetPendientesByModulo");
                throw exception;
            }
        }

    }
      
}
