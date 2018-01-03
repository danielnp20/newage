using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;
using System.ComponentModel;
using System.Reflection;

namespace NewAge.ADO
{
    public class DAL_ccNominaExclusiones : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccNominaExclusiones(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega informacion a la tabla ccNominaExclusiones
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccNominaExclusiones_Add(DTO_ccNominaExclusiones exclusiones)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "INSERT INTO ccNominaExclusiones   " +
                                               "    ( PagaduriaID  " +
                                               "    ,FechaNomina  " +
                                               "    ,ValorNomina  " +
                                               "    ,Renglon  " +
                                               "    ,Cedula  " +
                                               "    ,Nombre  " +
                                               "    ,Libranza  " +
                                               "    ,Valor  " +
                                               "    ,Observacion  " +
                                               "    ,MensajeError  " +
                                               "    ,eg_ccPagaduria  )  " +
                                               "VALUES    " +
                                                "   (@PagaduriaID  " +
                                               "    ,@FechaNomina  " +
                                               "    ,@ValorNomina  " +
                                               "    ,@Renglon  " +
                                               "    ,@Cedula  " +
                                               "    ,@Nombre  " +
                                               "    ,@Libranza  " +
                                               "    ,@Valor  " +
                                               "    ,@Observacion  " +
                                               "    ,@MensajeError  " +
                                               "    ,@eg_ccPagaduria  )  ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaNomina", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ValorNomina", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Renglon", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Cedula", SqlDbType.VarChar,20);
                mySqlCommandSel.Parameters.Add("@Nombre", SqlDbType.VarChar, 100);
                mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.VarChar, 20); 
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@MensajeError", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccPagaduria", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@PagaduriaID"].Value = exclusiones.PagaduriaID.Value;
                mySqlCommandSel.Parameters["@FechaNomina"].Value = exclusiones.FechaNomina.Value;
                mySqlCommandSel.Parameters["@ValorNomina"].Value = exclusiones.ValorNomina.Value;
                mySqlCommandSel.Parameters["@Renglon"].Value = exclusiones.Renglon.Value;
                mySqlCommandSel.Parameters["@Cedula"].Value = exclusiones.Cedula.Value;
                mySqlCommandSel.Parameters["@Nombre"].Value = exclusiones.Nombre.Value;
                mySqlCommandSel.Parameters["@Libranza"].Value = exclusiones.Libranza.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = exclusiones.Valor.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = exclusiones.Observacion.Value;
                mySqlCommandSel.Parameters["@MensajeError"].Value = exclusiones.MensajeError.Value;
                mySqlCommandSel.Parameters["@eg_ccPagaduria"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccPagaduria, this.Empresa, egCtrl);
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
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaExclusiones_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza los campos de la tabla ccNominaExclusiones
        /// </summary>
        public void DAL_ccNominaExclusiones_Update(DTO_ccNominaExclusiones exclusiones)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaTransmite", SqlDbType.DateTime);

                //mySqlCommandSel.Parameters["@Consecutivo"].Value = consecutivo;
                //mySqlCommandSel.Parameters["@FechaTransmite"].Value = fechaTransmite;

                mySqlCommandSel.CommandText = "UPDATE ccNominaExclusiones SET FechaTransmite = @FechaTransmite WHERE Consecutivo = @Consecutivo";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaExclusiones_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto</returns>
        public bool DAL_ccNominaExclusiones_Exist(string pagaduriaID, DateTime fechaNom)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from ccNominaExclusiones with(nolock) where PagaduriaID = @PagaduriaID and FechaNomina = @FechaNomina ";

                mySqlCommand.Parameters.Add("@PagaduriaID", SqlDbType.Char,UDT_PagaduriaID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaNomina", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters["@PagaduriaID"].Value = pagaduriaID;
                mySqlCommand.Parameters["@FechaNomina"].Value = fechaNom;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaExclusiones_Exist");
                throw exception;
            }
        }

        #endregion

    }

}
