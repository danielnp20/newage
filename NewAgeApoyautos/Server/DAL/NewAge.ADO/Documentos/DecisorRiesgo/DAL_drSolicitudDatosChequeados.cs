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
    public class DAL_drSolicitudDatosChequeados : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_drSolicitudDatosChequeados(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_drSolicitudDatosChequeados que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_drSolicitudDatosChequeados</returns>
        public List<DTO_drSolicitudDatosChequeados> DAL_drSolicitudDatosChequeados_GetAll()
        {
            try
            {
                List<DTO_drSolicitudDatosChequeados> result = new List<DTO_drSolicitudDatosChequeados>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT * FROM drSolicitudDatosChequeados with(nolock) ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_drSolicitudDatosChequeados Datos;
                    Datos = new DTO_drSolicitudDatosChequeados(dr);
                    result.Add(Datos);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosChequeados_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_drSolicitudDatosChequeados que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_drSolicitudDatosChequeados</returns>
        public List<DTO_drSolicitudDatosChequeados> DAL_drSolicitudDatosChequeados_GetByActividadNumDoc(string actividadFlujoID,int numeroDoc, int version)
        {
            try
            {
                List<DTO_drSolicitudDatosChequeados> result = new List<DTO_drSolicitudDatosChequeados>();


                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Version", SqlDbType.Int);


                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char,UDT_CodigoGrl10.MaxLength);

                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@Version"].Value = version;

                
                mySqlCommand.CommandText =
                            "SELECT * FROM drSolicitudDatosChequeados Dato with(nolock) " +
                             "  inner join drActividadChequeoLista Lista on dato.NroRegistro=Lista.ReplicaID"+
                             "  where lista.ActividadFlujoID =@ActividadFlujoID and NumeroDoc = @NumeroDoc and Version = @Version";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {

                    DTO_drSolicitudDatosChequeados anexo = new DTO_drSolicitudDatosChequeados(dr);
                    result.Add(anexo);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosChequeados_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_drSolicitudDatosChequeados que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_drSolicitudDatosChequeados</returns>
        public List<DTO_drSolicitudDatosChequeados> DAL_drSolicitudDatosChequeados_GetByNumDoc( int numeroDoc, int version)
        {
            try
            {
                List<DTO_drSolicitudDatosChequeados> result = new List<DTO_drSolicitudDatosChequeados>();


                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Version", SqlDbType.Int);


                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@Version"].Value = version;


                mySqlCommand.CommandText =
                            "SELECT * FROM drSolicitudDatosChequeados Dato with(nolock) " +
                             "  where NumeroDoc = @NumeroDoc and Version = @Version";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {

                    DTO_drSolicitudDatosChequeados anexo = new DTO_drSolicitudDatosChequeados(dr);
                    result.Add(anexo);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosChequeados_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla drSolicitudDatosChequeados
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public int DAL_drSolicitudDatosChequeados_Add(DTO_drSolicitudDatosChequeados Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO drSolicitudDatosChequeados " +
                    "( " +
                     "  NumeroDoc,Version,TipoPersona,NroRegistro,ChequeadoInd,VerficadoInd" +
                     ") " +
                    "VALUES " +
                    "( " +
                    "  @NumeroDoc,@Version,@TipoPersona,@NroRegistro,@ChequeadoInd,@VerficadoInd" +
                    ") SET @Consecutivo = SCOPE_IDENTITY()";
                #endregion
                #region Creacion de comandos                
                mySqlCommandSel.Parameters.Add("@NumeroDoc",SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version",SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoPersona", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@NroRegistro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ChequeadoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@VerficadoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Consecutivo",SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                mySqlCommandSel.Parameters["@TipoPersona"].Value = Datos.TipoPersona.Value;
                mySqlCommandSel.Parameters["@ChequeadoInd"].Value = Datos.ChequeadoInd.Value;
                mySqlCommandSel.Parameters["@VerficadoInd"].Value = Datos.VerficadoInd.Value;
                mySqlCommandSel.Parameters["@NroRegistro"].Value = Datos.NroRegistro.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;

                //Eg
//                mySqlCommandSel.Parameters["@eg_ccAseguradora"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAseguradora, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosChequeados_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla drSolicitudDatosChequeados
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_drSolicitudDatosChequeados_Update(DTO_drSolicitudDatosChequeados Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                              //"UPDATE drSolicitudDatosChequeados SET " +
                                              //                  ",NumeroDoc=@NumeroDoc," +
                                              //                  ",Version=@Version," +
                                              //                  ",TipoPersona=@TipoPersona," +
                                              //                  ",NroRegistro=@NroRegistro," +
                                              //                  ",ChequeadoInd=@ChequeadoInd," +
                                              //                  " WHERE  Consecutivo = @Consecutivo";
                                              "UPDATE drSolicitudDatosChequeados SET " +
                                                                "ChequeadoInd=@ChequeadoInd" +
                                                                "   where NumeroDoc=NumeroDoc" +
                                                                "   and Version=@Version" +
                                                                "   and	TipoPersona=@TipoPersona" +
                                                                "   and	NroRegistro=@NroRegistro";                
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoPersona", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@NroRegistro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ChequeadoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@VerficadoInd", SqlDbType.Bit);                
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                mySqlCommandSel.Parameters["@TipoPersona"].Value = Datos.TipoPersona.Value;
                mySqlCommandSel.Parameters["@ChequeadoInd"].Value = Datos.ChequeadoInd.Value;
                mySqlCommandSel.Parameters["@VerficadoInd"].Value = Datos.VerficadoInd.Value;                
                mySqlCommandSel.Parameters["@NroRegistro"].Value = Datos.NroRegistro.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = Datos.Consecutivo.Value;

                //Eg
                //                mySqlCommandSel.Parameters["@eg_ccAseguradora"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAseguradora, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosChequeados_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla drSolicitudDatosChequeados
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_drSolicitudDatosChequeados_UpdateVerifica(DTO_drSolicitudDatosChequeados Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                    //"UPDATE drSolicitudDatosChequeados SET " +
                    //                  ",NumeroDoc=@NumeroDoc," +
                    //                  ",Version=@Version," +
                    //                  ",TipoPersona=@TipoPersona," +
                    //                  ",NroRegistro=@NroRegistro," +
                    //                  ",ChequeadoInd=@ChequeadoInd," +
                    //                  " WHERE  Consecutivo = @Consecutivo";
                                              "UPDATE drSolicitudDatosChequeados SET " +
                                                                "   VerficadoInd=@VerficadoInd" +
                                                                "   where NumeroDoc=NumeroDoc" +
                                                                "   and Version=@Version" +
                                                                "   and	TipoPersona=@TipoPersona" +
                                                                "   and	NroRegistro=@NroRegistro";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoPersona", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@NroRegistro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ChequeadoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@VerficadoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                mySqlCommandSel.Parameters["@TipoPersona"].Value = Datos.TipoPersona.Value;
                mySqlCommandSel.Parameters["@ChequeadoInd"].Value = Datos.ChequeadoInd.Value;
                mySqlCommandSel.Parameters["@VerficadoInd"].Value = Datos.VerficadoInd.Value;
                mySqlCommandSel.Parameters["@NroRegistro"].Value = Datos.NroRegistro.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = Datos.Consecutivo.Value;

                //Eg
                //                mySqlCommandSel.Parameters["@eg_ccAseguradora"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAseguradora, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosChequeados_Update");
                throw exception;
            }
        }


        public void DAL_drSolicitudDatosChequeados_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandText = "DELETE FROM drSolicitudDatosChequeados WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosChequeados_Delete");
                throw exception;
            }
        }

        #endregion

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="consec"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public bool DAL_drSolicitudDatosChequeados_Exist(int? numeroDoc , int? version, int tipopersona, int? NroRegistro)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = 
                        "select count(*) from drSolicitudDatosChequeados with(nolock)"+
                            "   where NumeroDoc=NumeroDoc"+
                            "   and Version=@Version"+
                            "   and	TipoPersona=@TipoPersona"+
                            "   and	NroRegistro=@NroRegistro";
                                				
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Version", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TipoPersona", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NroRegistro", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@Version"].Value = version;
                mySqlCommand.Parameters["@TipoPersona"].Value = tipopersona;
                mySqlCommand.Parameters["@NroRegistro"].Value = NroRegistro;


                if (mySqlCommand.Parameters["@NumeroDoc"].Value == null || ((mySqlCommand.Parameters["@NumeroDoc"].Value is string) &&
                    string.IsNullOrWhiteSpace(mySqlCommand.Parameters["@NumeroDoc"].Value.ToString())))
                    mySqlCommand.Parameters["@NumeroDoc"].Value = DBNull.Value;


                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosChequeados_Exist");
                throw exception;
            }
        }

    }
}
