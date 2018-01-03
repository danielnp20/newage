using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL Legalizacion Detalle
    /// </summary>
    public class DAL_Legalizacion : DAL_Base
    {
       /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_Legalizacion(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region LegalizaHeader

        /// <summary>
        /// Consulta una legalizacion segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_cpLegalizaDocu DAL_LegalizaHeader_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from cpLegalizaDocu with(nolock) where cpLegalizaDocu.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                DTO_cpLegalizaDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_cpLegalizaDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_LegalizaHeader_Get");
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla cpLegalizaDocu 
        /// </summary>
        /// <param name="leg">Legalizacion</param>
        /// <returns></returns>
        public void DAL_LegalizaHeader_Add(DTO_cpLegalizaDocu leg)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO cpLegalizaDocu " +
                                           "    (EmpresaID    " +
                                           "    ,NumeroDoc    " +
                                           "    ,CajaMenorID  " +
                                           "    ,TarjetaCreditoID  " +
                                           "    ,FechaIni     " +
                                           "    ,FechaFin     " +
                                           "    ,FechaCont    " +
                                           "    ,ValorFondo   " +
                                           "    ,IdentificadorAnt1   " +
                                           "    ,ValorAnticipo1      " +
                                           "    ,IdentificadorAnt2   " +
                                           "    ,ValorAnticipo2      " +
                                           "    ,IdentificadorAnt3   " +
                                           "    ,ValorAnticipo3      " +
                                           "    ,IdentificadorAnt4   " +
                                           "    ,ValorAnticipo4      " +
                                           "    ,IdentificadorAnt5   " +
                                           "    ,ValorAnticipo5      " +
                                           "    ,Valor    " +
                                           "    ,IVA      " +
                                           "    ,Estado   " +
                                           "    ,eg_cpCajaMenor   " +
                                           "    ,eg_tsTarjetaCredito)     " +
                                           "     VALUES" +
                                           "    (@EmpresaID     " +
                                           "    ,@NumeroDoc     " +
                                           "    ,@CajaMenorID   " +
                                           "    ,@TarjetaCreditoID   " +
                                           "    ,@FechaIni      " +
                                           "    ,@FechaFin      " +
                                           "    ,@FechaCont     " +
                                           "    ,@ValorFondo    " +
                                           "    ,@IdentificadorAnt1     " +
                                           "    ,@ValorAnticipo1        " +
                                           "    ,@IdentificadorAnt2     " +
                                           "    ,@ValorAnticipo2        " +
                                           "    ,@IdentificadorAnt3     " +
                                           "    ,@ValorAnticipo3        " +
                                           "    ,@IdentificadorAnt4     " +
                                           "    ,@ValorAnticipo4        " +
                                           "    ,@IdentificadorAnt5     " +
                                           "    ,@ValorAnticipo5        " +
                                           "    ,@Valor         " +
                                           "    ,@IVA           " +
                                           "    ,@Estado        " +
                                           "    ,@eg_cpCajaMenor " +
                                           "    ,@eg_tsTarjetaCredito) ";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CajaMenorID", SqlDbType.Char, UDT_CajaMenorID.MaxLength);
                mySqlCommand.Parameters.Add("@TarjetaCreditoID", SqlDbType.Char, UDT_TarjetaCreditoID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaIni", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaCont", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ValorFondo", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt1", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt2", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt3", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo3", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt4", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo4", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt5", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo5", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@eg_cpCajaMenor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_tsTarjetaCredito", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = leg.NumeroDoc.Value;
                mySqlCommand.Parameters["@CajaMenorID"].Value = string.IsNullOrEmpty(leg.CajaMenorID.Value) ? DBNull.Value.ToString() : leg.CajaMenorID.Value;
                mySqlCommand.Parameters["@TarjetaCreditoID"].Value = string.IsNullOrEmpty(leg.TarjetaCreditoID.Value) ? DBNull.Value.ToString() : leg.TarjetaCreditoID.Value;
                mySqlCommand.Parameters["@FechaIni"].Value = leg.FechaIni.Value;
                mySqlCommand.Parameters["@FechaFin"].Value = leg.FechaFin.Value;
                mySqlCommand.Parameters["@FechaCont"].Value = leg.FechaCont.Value;
                mySqlCommand.Parameters["@ValorFondo"].Value = !leg.ValorFondo.Value.HasValue ? null : leg.ValorFondo.Value;
                mySqlCommand.Parameters["@IdentificadorAnt1"].Value = !leg.IdentificadorAnt1.Value.HasValue ? null : leg.IdentificadorAnt1.Value;
                mySqlCommand.Parameters["@ValorAnticipo1"].Value = leg.ValorAnticipo1.Value;
                mySqlCommand.Parameters["@IdentificadorAnt2"].Value = !leg.IdentificadorAnt2.Value.HasValue ? null : leg.IdentificadorAnt2.Value;
                mySqlCommand.Parameters["@ValorAnticipo2"].Value = leg.ValorAnticipo2.Value;
                mySqlCommand.Parameters["@IdentificadorAnt3"].Value = !leg.IdentificadorAnt3.Value.HasValue ? null : leg.IdentificadorAnt3.Value;
                mySqlCommand.Parameters["@ValorAnticipo3"].Value = leg.ValorAnticipo3.Value;
                mySqlCommand.Parameters["@IdentificadorAnt4"].Value = !leg.IdentificadorAnt4.Value.HasValue ? null : leg.IdentificadorAnt4.Value;
                mySqlCommand.Parameters["@ValorAnticipo4"].Value = leg.ValorAnticipo4.Value;
                mySqlCommand.Parameters["@IdentificadorAnt5"].Value = !leg.IdentificadorAnt5.Value.HasValue ? null : leg.IdentificadorAnt5.Value;
                mySqlCommand.Parameters["@ValorAnticipo5"].Value = leg.ValorAnticipo5.Value;
                mySqlCommand.Parameters["@Valor"].Value = leg.Valor.Value;
                mySqlCommand.Parameters["@IVA"].Value = !leg.IVA.Value.HasValue ? null : leg.IVA.Value;
                mySqlCommand.Parameters["@Estado"].Value = leg.Estado.Value;
                mySqlCommand.Parameters["@eg_cpCajaMenor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cpCajaMenor, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_tsTarjetaCredito"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cpTarjetaCredito, this.Empresa, egCtrl);

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
                int numDoc = Convert.ToInt32(mySqlCommand.Parameters["@NumeroDoc"].Value);

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_LegalizaHeader_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Actualizar la legalizacion en tabla cpLegalizaDocu y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">legalizacion</param>
        public void DAL_LegalizaHeader_Upd(DTO_cpLegalizaDocu leg)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla cpLegalizaDocu
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE cpLegalizaDocu " +
                                           "    SET EmpresaID  = @EmpresaID  " +
                                           "    ,CajaMenorID = @CajaMenorID " +
                                           "    ,TarjetaCreditoID = @TarjetaCreditoID " +
                                           "    ,FechaIni  = @FechaIni   " +
                                           "    ,FechaFin  = @FechaFin   " +
                                           "    ,FechaCont = @FechaCont   " +
                                           "    ,ValorFondo = @ValorFondo  " +
                                           "    ,IdentificadorAnt1 = @IdentificadorAnt1  " +
                                           "    ,ValorAnticipo1 = @ValorAnticipo1     " +
                                           "    ,IdentificadorAnt2 = @IdentificadorAnt2  " +
                                           "    ,ValorAnticipo2 = @ValorAnticipo2     " +
                                           "    ,IdentificadorAnt3 = @IdentificadorAnt3  " +
                                           "    ,ValorAnticipo3 = @ValorAnticipo3     " +
                                           "    ,IdentificadorAnt4 = @IdentificadorAnt4  " +
                                           "    ,ValorAnticipo4 = @ValorAnticipo4     " +
                                           "    ,IdentificadorAnt5 = @IdentificadorAnt5  " +
                                           "    ,ValorAnticipo5 = @ValorAnticipo5     " +
                                           "    ,Valor = @Valor   " +
                                           "    ,IVA = @IVA     " +
                                           "    ,Estado = @Estado  " +
                                           "    ,NumeroDocCXP = @NumeroDocCXP  " +
                                           "    WHERE NumeroDoc = @NumeroDoc ";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CajaMenorID", SqlDbType.Char, UDT_CajaMenorID.MaxLength);
                mySqlCommand.Parameters.Add("@TarjetaCreditoID", SqlDbType.Char, UDT_TarjetaCreditoID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaIni", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaCont", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ValorFondo", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt1", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt2", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt3", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo3", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt4", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo4", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt5", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo5", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@NumeroDocCXP", SqlDbType.Int);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = leg.NumeroDoc.Value;
                mySqlCommand.Parameters["@CajaMenorID"].Value = string.IsNullOrEmpty(leg.CajaMenorID.Value) ? null : leg.CajaMenorID.Value;
                mySqlCommand.Parameters["@TarjetaCreditoID"].Value = string.IsNullOrEmpty(leg.TarjetaCreditoID.Value) ? null : leg.TarjetaCreditoID.Value;
                mySqlCommand.Parameters["@FechaIni"].Value = leg.FechaIni.Value;
                mySqlCommand.Parameters["@FechaFin"].Value = leg.FechaFin.Value;
                mySqlCommand.Parameters["@FechaCont"].Value = leg.FechaCont.Value;
                mySqlCommand.Parameters["@ValorFondo"].Value = !leg.ValorFondo.Value.HasValue ? null : leg.ValorFondo.Value;
                mySqlCommand.Parameters["@IdentificadorAnt1"].Value = !leg.IdentificadorAnt1.Value.HasValue ? null : leg.IdentificadorAnt1.Value;
                mySqlCommand.Parameters["@ValorAnticipo1"].Value = leg.ValorAnticipo1.Value;
                mySqlCommand.Parameters["@IdentificadorAnt2"].Value = !leg.IdentificadorAnt2.Value.HasValue ? null : leg.IdentificadorAnt2.Value;
                mySqlCommand.Parameters["@ValorAnticipo2"].Value = leg.ValorAnticipo2.Value;
                mySqlCommand.Parameters["@IdentificadorAnt3"].Value = !leg.IdentificadorAnt3.Value.HasValue ? null : leg.IdentificadorAnt3.Value;
                mySqlCommand.Parameters["@ValorAnticipo3"].Value = leg.ValorAnticipo3.Value;
                mySqlCommand.Parameters["@IdentificadorAnt4"].Value = !leg.IdentificadorAnt4.Value.HasValue ? null : leg.IdentificadorAnt4.Value;
                mySqlCommand.Parameters["@ValorAnticipo4"].Value = leg.ValorAnticipo4.Value;
                mySqlCommand.Parameters["@IdentificadorAnt5"].Value = !leg.IdentificadorAnt5.Value.HasValue ? null : leg.IdentificadorAnt5.Value;
                mySqlCommand.Parameters["@ValorAnticipo5"].Value = leg.ValorAnticipo5.Value;
                mySqlCommand.Parameters["@Valor"].Value = leg.Valor.Value;
                mySqlCommand.Parameters["@IVA"].Value = !leg.IVA.Value.HasValue ? null : leg.IVA.Value;
                mySqlCommand.Parameters["@Estado"].Value = leg.Estado.Value;
                mySqlCommand.Parameters["@NumeroDocCXP"].Value = leg.NumeroDocCXP.Value;

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
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpLegalizaHeader_Upd");
                throw exception;
            }

        }

        /// <summary>
        /// Le cambia el estado a un documentoControl
        /// </summary>
        /// <param name="numeroDoc">Numero de documento - PK (NumeroDoc) de glDocumentoControl</param>
        /// <param name="estado">Nuevo estado</param>
        /// <param name="alarmEnabled">Indica si se debe activar o no la alarma (Null: para no tocar las alarmas)</param>
        /// <param name="alarmTareaID">Indica una tarea particular para asignar (Null: asigna la que venga de glDocumento)</param>
        /// <param name="compAnulaID">Identificador de comprobante de anulacion</param>
        /// <param name="compNroAnula">Numero de comprobante de anulacion</param>
        /// <param name="userId">Identificador del usuario que esta ejecutando la operacion</param>
        /// <returns>Retorna el identificador de la bitacora con que se guardo la info</returns>
        public void DAL_cpLegalizaDocu_ChangeDocumentStatus(int numeroDoc, EstadoInterCajaMenor estado, DTO_cpLegalizaDocu headerAprob)
        {
            try
            {
                #region Actualiza, el estado y los usuarios que realizaran los niveles interm de aprobacion

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string set = "Estado=@Estado ";

                #region Actualiza la informacion los usuarios y fechas
                   set += ", UsuarioSolicita = @UsuarioSolicita, FechaSolicita = @FechaSolicita, UsuarioRevisa = @UsuarioRevisa, " +
                           "FechaRevisa = @FechaRevisa, UsuarioSupervisa = @UsuarioSupervisa, FechaSupervisa = @FechaSupervisa," +
                           "UsuarioAprueba = @UsuarioAprueba, FechaAprueba = @FechaAprueba, UsuarioContabiliza = @UsuarioContabiliza," +
                           "FechaContabiliza = @FechaContabiliza";

                    mySqlCommand.Parameters.Add("@FechaSolicita", SqlDbType.DateTime);
                    mySqlCommand.Parameters.Add("@FechaRevisa", SqlDbType.DateTime);
                    mySqlCommand.Parameters.Add("@FechaSupervisa", SqlDbType.DateTime);
                    mySqlCommand.Parameters.Add("@FechaAprueba", SqlDbType.DateTime);
                    mySqlCommand.Parameters.Add("@FechaContabiliza", SqlDbType.DateTime);


                    mySqlCommand.Parameters.Add("@UsuarioSolicita", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                    mySqlCommand.Parameters.Add("@UsuarioRevisa", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                    mySqlCommand.Parameters.Add("@UsuarioSupervisa", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                    mySqlCommand.Parameters.Add("@UsuarioAprueba", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                    mySqlCommand.Parameters.Add("@UsuarioContabiliza", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);

                    #region Asignacion de valores
                    mySqlCommand.Parameters["@UsuarioSolicita"].Value = headerAprob.UsuarioSolicita.Value;
                    mySqlCommand.Parameters["@UsuarioRevisa"].Value = headerAprob.UsuarioRevisa.Value;
                    mySqlCommand.Parameters["@UsuarioSupervisa"].Value = headerAprob.UsuarioSupervisa.Value;
                    mySqlCommand.Parameters["@UsuarioAprueba"].Value = headerAprob.UsuarioAprueba.Value;
                    mySqlCommand.Parameters["@UsuarioContabiliza"].Value = headerAprob.UsuarioContabiliza.Value;

                    // FechaSolicita
                    if (headerAprob.FechaSolicita.Value != null)
                        mySqlCommand.Parameters["@FechaSolicita"].Value = headerAprob.FechaSolicita.Value;
                    else
                        mySqlCommand.Parameters["@FechaSolicita"].Value = DBNull.Value;
                    // FechaRevisa
                    if (headerAprob.FechaRevisa.Value != null)
                        mySqlCommand.Parameters["@FechaRevisa"].Value = headerAprob.FechaRevisa.Value;
                    else
                        mySqlCommand.Parameters["@FechaRevisa"].Value = DBNull.Value;
                    // FechaSupervisa
                    if (headerAprob.FechaSupervisa.Value != null)
                        mySqlCommand.Parameters["@FechaSupervisa"].Value = headerAprob.FechaSupervisa.Value;
                    else
                        mySqlCommand.Parameters["@FechaSupervisa"].Value = DBNull.Value;
                    // FechaAprueba
                    if (headerAprob.FechaAprueba.Value != null)
                        mySqlCommand.Parameters["@FechaAprueba"].Value = headerAprob.FechaAprueba.Value;
                    else
                        mySqlCommand.Parameters["@FechaAprueba"].Value = DBNull.Value;
                    // FechaContabiliza
                    if (headerAprob.FechaContabiliza.Value != null)
                        mySqlCommand.Parameters["@FechaContabiliza"].Value = headerAprob.FechaContabiliza.Value;
                    else
                        mySqlCommand.Parameters["@FechaContabiliza"].Value = DBNull.Value;

                    #endregion
                   
                #endregion
                #region Ejecuta el query
                mySqlCommand.CommandText =
                    "UPDATE cpLegalizaDocu SET " + set + " WHERE NumeroDoc=@NumeroDoc";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@Estado"].Value = Convert.ToByte(estado);

                mySqlCommand.ExecuteNonQuery();
                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpLegalizaDocu_ChangeDocumentStatus");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de causaciones pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_LegalizacionAprobacion> DAL_Legalizacion_GetPendientesByModulo(ModulesPrefix mod, string actividadFlujoID, string usuarioID)
        {
            try
            {
                List<DTO_LegalizacionAprobacion> result = new List<DTO_LegalizacionAprobacion>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select distinct ctrl.NumeroDoc, PeriodoDoc as PeriodoID, ComprobanteID, ctrl.ComprobanteIDNro as ComprobanteNro, ctrl.DocumentoNro, usr.UsuarioID, " +
                    "  ctrl.DocumentoID, ctrl.TerceroID, MonedaID,leg.NumeroDocCxP, leg.CajaMenorID, leg.TarjetaCreditoID, leg.Valor,SUM(leg.ValorAnticipo1 + leg.ValorAnticipo2 + leg.ValorAnticipo3 + leg.ValorAnticipo4 + leg.ValorAnticipo5) as ValorTarjeta " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "   inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "	    and act.CerradoInd=@CerradoInd and act.ActividadFlujoID=@ActividadFlujoID " +
                    "   inner join cpLegalizaDocu leg with(nolock) on ctrl.NumeroDoc = leg.NumeroDoc" +
                    "	inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                      "	inner join coTercero ter with(nolock) on ctrl.TerceroID = ter.TerceroID " +
                    "   inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID " +
                    "   inner join glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    "       and perm.UsuarioID = @UsuarioID and Perm.AreaFuncionalID = ctrl.AreaFuncionalID " +
                    "where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado and perm.ActividadFlujoID = @ActividadFlujoID " +
                    "Group By ctrl.NumeroDoc,PeriodoDoc, ComprobanteID, ctrl.ComprobanteIDNro, ctrl.DocumentoNro, usr.UsuarioID,  " +
                    "ctrl.DocumentoID, ctrl.TerceroID, MonedaID,leg.NumeroDocCxP, leg.CajaMenorID, leg.TarjetaCreditoID, leg.Valor";
                
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
                    DTO_LegalizacionAprobacion dto = new DTO_LegalizacionAprobacion(dr);
                    if (!string.IsNullOrWhiteSpace(dr["NumeroDocCxP"].ToString()))
                        dto.NumeroDocCxP.Value = Convert.ToInt32(dr["NumeroDocCxP"]); 
                    dto.Aprobado.Value = false;
                    dto.Rechazado.Value = false;
                    dto.Observacion.Value = string.Empty;
                    result.Add(dto);
                }
                dr.Close();

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);

                foreach (DTO_LegalizacionAprobacion dto in result)
                {
                    mySqlCommand.Parameters["@NumeroDoc"].Value = dto.NumeroDoc.Value.Value;
                    mySqlCommand.Parameters["@DocumentoID"].Value = dto.DocumentoID.Value;

                    mySqlCommand.CommandText =
                        "select Observacion, Fecha " +
                        "from glDocumentoControl with(nolock) " +
                        "where  " +
                        "	EmpresaID = @EmpresaID AND " +
                        "	NumeroDoc = @NumeroDoc AND " + 
                        "   DocumentoID = @DocumentoID";

                    dr = mySqlCommand.ExecuteReader();
                    if (dr.Read())
                    {
                        dto.Descriptivo.Value = dr["Observacion"] == null ? string.Empty : dr["Observacion"].ToString();
                        dto.Fecha.Value =  Convert.ToDateTime(dr["Fecha"]);
                    }
                    dr.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Legalizacion_GetPendientesByModulo");
                throw exception;
            }
        }

        #endregion

        #region LegalizaFooter

        /// <summary>
        /// Consulta una legalizacion segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public List<DTO_cpLegalizaFooter>DAL_LegalizaFooter_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "Select det.*,proy.Descriptivo as ProyectoDesc, cto.Descriptivo as CentroCostoDesc from cpLegalizaDeta det with(nolock) " +
                                            " inner join coProyecto proy with(nolock) on proy.ProyectoID = det.ProyectoID and proy.EmpresaGrupoID = det.eg_coProyecto " +
                                            " inner join coCentroCosto cto with(nolock) on cto.CentroCostoID = det.CentroCostoID and cto.EmpresaGrupoID = det.eg_coCentroCosto " +
                                            " where det.NumeroDoc = @NumeroDoc order by det.Item  ";
                
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                List<DTO_cpLegalizaFooter> footer = new List<DTO_cpLegalizaFooter>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_cpLegalizaFooter detail = new DTO_cpLegalizaFooter(dr);
                    detail.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
                    detail.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                    detail.Index = index;
                    footer.Add(detail);
                    index++;
                }
                dr.Close();
                return footer;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_LegalizaFooter_Get");
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla cpLegalizaDeta
        /// </summary>
        /// <param name="footer">Legalizacion</param>
        public void DAL_LegalizaFooter_Add(List<DTO_cpLegalizaFooter> footer)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO cpLegalizaDeta " +
                                           "    (EmpresaID    " +
                                           "    ,NumeroDoc    " +
                                           "    ,Item         " +
                                           "    ,MonedaID     " +
                                           "    ,CargoEspecialID  " +
                                           "    ,Factura      " +
                                           "    ,FactEquivalente  " +
                                           "    ,Fecha        " +
                                           "    ,TasaCambioDOCU   " +
                                           "    ,TasaCambioCONT   " +
                                           "    ,TerceroID    " +
                                           "    ,Nombre    " +
                                           "    ,NuevoTerceroInd  " +
                                           "    ,Descriptivo  " +
                                           "    ,ProyectoID   " +
                                           "    ,CentroCostoID" +
                                           "    ,LugarGeograficoID " +
                                           "    ,ValorBruto   " +
                                           "    ,ValorNeto    " +
                                           "    ,PorIVA1      " +
                                           "    ,BaseIVA1     " +
                                           "    ,ValorIVA1    " +
                                           "    ,PorIVA2      " +
                                           "    ,BaseIVA2     " +
                                           "    ,ValorIVA2    " +
                                           "    ,RteIVA1AsumidoInd " +
                                           "    ,RteIVA2AsumidoInd " +
                                           "    ,PorRteIVA1    " +
                                           "    ,PorRteIVA2    " +
                                           "    ,BaseRteIVA1   " +
                                           "    ,BaseRteIVA2  " +
                                           "    ,ValorRteIVA1  " +
                                           "    ,ValorRteIVA2  " +
                                           "    ,RteFteAsumidoInd " +
                                           "    ,PorRteFuente " +
                                           "    ,BaseRteFuente " +
                                           "    ,ValorRteFuente   " +
                                           "    ,RteICAAsumidoInd" +
                                           "    ,PorRteICA    " +
                                           "    ,BaseRteICA   " +
                                           "    ,ValorRteICA  " +
                                           "    ,PorImpConsumo  " +
                                           "    ,BaseImpConsumo  " +
                                           "    ,ValorImpConsumo  " +
                                           "    ,eg_cpCargoEspecial" +
                                           "    ,eg_coTercero " +
                                           "    ,eg_coProyecto" +
                                           "    ,eg_coCentroCosto  " +
                                           "    ,eg_glLugarGeografico) " +
                                           "    VALUES          " +
                                           "    (@EmpresaID     " +
                                           "    ,@NumeroDoc     " +
                                           "    ,@Item          " +
                                           "    ,@MonedaID      " +
                                           "    ,@CargoEspecialID  " +
                                           "    ,@Factura       " +
                                           "    ,@FactEquivalente  " +
                                           "    ,@Fecha         " +
                                           "    ,@TasaCambioDOCU " +
                                           "    ,@TasaCambioCONT " +
                                           "    ,@TerceroID      " +
                                           "    ,@Nombre" +
                                           "    ,@NuevoTerceroInd" +
                                           "    ,@Descriptivo    " +
                                           "    ,@ProyectoID     " +
                                           "    ,@CentroCostoID  " +
                                           "    ,@LugarGeograficoID  " +
                                           "    ,@ValorBruto     " +
                                           "    ,@ValorNeto      " +
                                           "    ,@PorIVA1        " +
                                           "    ,@BaseIVA1       " +
                                           "    ,@ValorIVA1      " +
                                           "    ,@PorIVA2        " +
                                           "    ,@BaseIVA2       " +
                                           "    ,@ValorIVA2      " +
                                           "    ,@RteIVA1AsumidoInd " +
                                           "    ,@RteIVA2AsumidoInd " +
                                           "    ,@PorRteIVA1      " +
                                           "    ,@PorRteIVA2      " +
                                           "    ,@BaseRteIVA1     " +
                                           "    ,@BaseRteIVA2     " +
                                           "    ,@ValorRteIVA1    " +
                                           "    ,@ValorRteIVA2    " +
                                           "    ,@RteFteAsumidoInd " +
                                           "    ,@PorRteFuente    " +
                                           "    ,@BaseRteFuente   " +
                                           "    ,@ValorRteFuente  " +
                                           "    ,@RteICAAsumidoInd " +
                                           "    ,@PorRteICA       " +
                                           "    ,@BaseRteICA      " +
                                           "    ,@ValorRteICA     " +
                                           "    ,@PorImpConsumo  " +
                                           "    ,@BaseImpConsumo  " +
                                           "    ,@ValorImpConsumo  " +
                                           "    ,@eg_cpCargoEspecial " +
                                           "    ,@eg_coTercero   " +
                                           "    ,@eg_coProyecto  " +
                                           "    ,@eg_coCentroCosto   " +
                                           "    ,@eg_glLugarGeografico) ";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Item", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MonedaID", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@CargoEspecialID", SqlDbType.Char, UDT_CargoEspecialID.MaxLength);
                mySqlCommand.Parameters.Add("@Factura", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@FactEquivalente", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@TasaCambioDOCU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@TasaCambioCONT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@Nombre", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommand.Parameters.Add("@NuevoTerceroInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Descriptivo", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@LugarGeograficoID", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommand.Parameters.Add("@ValorBruto", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorNeto", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorIVA1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@BaseIVA1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorIVA1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorIVA2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@BaseIVA2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorIVA2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@RteIVA1AsumidoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@RteIVA2AsumidoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@PorRteIVA1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorRteIVA2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@BaseRteIVA1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@BaseRteIVA2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorRteIVA1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorRteIVA2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@RteFteAsumidoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@PorRteFuente", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@BaseRteFuente", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorRteFuente", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@RteICAAsumidoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@PorRteICA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@BaseRteICA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorRteICA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorImpConsumo", SqlDbType.Decimal);               
                mySqlCommand.Parameters.Add("@BaseImpConsumo", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorImpConsumo", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_cpCargoEspecial", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                foreach (DTO_cpLegalizaFooter det in footer)
                {
                    #region Asignacion de valores
                    mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommand.Parameters["@NumeroDoc"].Value = det.NumeroDoc.Value;
                    mySqlCommand.Parameters["@Item"].Value = det.Item.Value;
                    mySqlCommand.Parameters["@MonedaID"].Value = det.MonedaID.Value;
                    mySqlCommand.Parameters["@CargoEspecialID"].Value = det.CargoEspecialID.Value;
                    mySqlCommand.Parameters["@Factura"].Value = det.Factura.Value;
                    mySqlCommand.Parameters["@FactEquivalente"].Value = string.IsNullOrEmpty(det.FactEquivalente.Value) ? DBNull.Value.ToString() : det.FactEquivalente.Value; ;
                    mySqlCommand.Parameters["@Fecha"].Value = det.Fecha.Value;
                    mySqlCommand.Parameters["@TasaCambioDOCU"].Value = det.TasaCambioDOCU.Value;
                    mySqlCommand.Parameters["@TasaCambioCONT"].Value = det.TasaCambioCONT.Value;
                    mySqlCommand.Parameters["@TerceroID"].Value = det.TerceroID.Value;
                    mySqlCommand.Parameters["@Nombre"].Value = det.Nombre.Value;
                    mySqlCommand.Parameters["@NuevoTerceroInd"].Value = det.NuevoTerceroInd.Value;
                    mySqlCommand.Parameters["@Descriptivo"].Value = det.Descriptivo.Value;
                    mySqlCommand.Parameters["@ProyectoID"].Value = det.ProyectoID.Value;
                    mySqlCommand.Parameters["@CentroCostoID"].Value = det.CentroCostoID.Value;
                    mySqlCommand.Parameters["@LugarGeograficoID"].Value = det.LugarGeograficoID.Value;
                    mySqlCommand.Parameters["@ValorBruto"].Value = det.ValorBruto.Value;
                    mySqlCommand.Parameters["@ValorNeto"].Value = det.ValorNeto.Value;
                    mySqlCommand.Parameters["@PorIVA1"].Value = !det.PorIVA1.Value.HasValue ? 0 : det.PorIVA1.Value;
                    mySqlCommand.Parameters["@BaseIVA1"].Value = !det.BaseIVA1.Value.HasValue ? null : det.BaseIVA1.Value;
                    mySqlCommand.Parameters["@ValorIVA1"].Value = !det.ValorIVA1.Value.HasValue ? null : det.ValorIVA1.Value;
                    mySqlCommand.Parameters["@PorIVA2"].Value = !det.PorIVA2.Value.HasValue ? 0 : det.PorIVA2.Value;
                    mySqlCommand.Parameters["@BaseIVA2"].Value = !det.BaseIVA2.Value.HasValue ? null : det.BaseIVA2.Value;
                    mySqlCommand.Parameters["@ValorIVA2"].Value = !det.ValorIVA2.Value.HasValue ? null : det.ValorIVA2.Value;
                    mySqlCommand.Parameters["@RteIVA1AsumidoInd"].Value = det.RteIVA1AsumidoInd.Value;
                    mySqlCommand.Parameters["@RteIVA2AsumidoInd"].Value = det.RteIVA2AsumidoInd.Value;
                    mySqlCommand.Parameters["@PorRteIVA1"].Value = !det.PorRteIVA1.Value.HasValue ? 0 : det.PorRteIVA1.Value;
                    mySqlCommand.Parameters["@PorRteIVA2"].Value = !det.PorRteIVA2.Value.HasValue ? 0 : det.PorRteIVA2.Value;
                    mySqlCommand.Parameters["@BaseRteIVA1"].Value = !det.BaseRteIVA1.Value.HasValue ? null : det.BaseRteIVA1.Value;
                    mySqlCommand.Parameters["@BaseRteIVA2"].Value = !det.BaseRteIVA2.Value.HasValue ? null : det.BaseRteIVA2.Value;
                    mySqlCommand.Parameters["@ValorRteIVA1"].Value = !det.ValorRteIVA1.Value.HasValue ? null : det.ValorRteIVA1.Value;
                    mySqlCommand.Parameters["@ValorRteIVA2"].Value = !det.ValorRteIVA2.Value.HasValue ? null : det.ValorRteIVA2.Value;
                    mySqlCommand.Parameters["@RteFteAsumidoInd"].Value = det.RteFteAsumidoInd.Value;
                    mySqlCommand.Parameters["@PorRteFuente"].Value = !det.PorRteFuente.Value.HasValue ? 0 : det.PorRteFuente.Value;
                    mySqlCommand.Parameters["@BaseRteFuente"].Value = !det.BaseRteFuente.Value.HasValue ? null : det.BaseRteFuente.Value;
                    mySqlCommand.Parameters["@ValorRteFuente"].Value = !det.ValorRteFuente.Value.HasValue ? null : det.ValorRteFuente.Value;
                    mySqlCommand.Parameters["@RteICAAsumidoInd"].Value = det.RteICAAsumidoInd.Value;
                    mySqlCommand.Parameters["@PorRteICA"].Value = !det.PorRteICA.Value.HasValue ? 0 : det.PorRteICA.Value;
                    mySqlCommand.Parameters["@BaseRteICA"].Value = !det.BaseRteICA.Value.HasValue ? null : det.BaseRteICA.Value;
                    mySqlCommand.Parameters["@ValorRteICA"].Value = !det.ValorRteICA.Value.HasValue ? 0 : det.ValorRteICA.Value;
                    mySqlCommand.Parameters["@PorImpConsumo"].Value = !det.PorImpConsumo.Value.HasValue ? 0 : det.PorImpConsumo.Value;
                    mySqlCommand.Parameters["@BaseImpConsumo"].Value = !det.BaseImpConsumo.Value.HasValue ? 0 : det.BaseImpConsumo.Value;
                    mySqlCommand.Parameters["@ValorImpConsumo"].Value = !det.ValorImpConsumo.Value.HasValue ? 0 : det.ValorImpConsumo.Value;
                    mySqlCommand.Parameters["@eg_cpCargoEspecial"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cpCargoEspecial, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);
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
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_LegalizaFooter_Add");
                throw exception;
            }                  
        }

        /// <summary>
        /// Actualizar la tabla cpLegalizaDeta 
        /// </summary>
        /// <param name="leg">legalizacion</param>
        public void DAL_LegalizaFooter_UpdFactEquiv(int numeroDoc, string factEquival)
        {
            
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla cpLegalizaDeta
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE cpLegalizaDeta " +
                                           "    SET FactEquivalente = @FactEquivalente     " +
                                           "    WHERE NumeroDoc = @NumeroDoc";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FactEquivalente", SqlDbType.Char, 20);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@FactEquivalente"].Value = string.IsNullOrEmpty(factEquival) ? DBNull.Value.ToString() : factEquival;

                #endregion
                mySqlCommand.ExecuteNonQuery();   
          
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_LegalizaFooter_UpdFactEquiv");
                throw exception;
            }           

        }

        /// <summary>
        /// Agrega registros a la tabla de cpLegalizaDeta
        /// </summary>
        /// <param name="compNro">NumeroDoc</param>
        public void DAL_LegalizaFooter_Delete(int numeroDoc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText = "DELETE FROM cpLegalizaDeta where EmpresaID = @EmpresaID " +
                " and NumeroDoc = @NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Legalizacion_BorrarLegalizaFooter");
                throw exception;
            }
        }

        #endregion

    }
}
