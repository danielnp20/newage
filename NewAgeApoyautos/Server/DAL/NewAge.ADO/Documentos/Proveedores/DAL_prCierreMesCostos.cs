using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;
using System.Linq;

namespace NewAge.ADO
{
    public class DAL_prCierreMesCostos : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prCierreMesCostos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega un registro de cierre 
        /// </summary>
        /// <param name="deta">Cierre nuevo</param>
        /// <returns></returns>
        private void DAL_prCierreMesCostos_AddItem(DTO_prCierreMesCostos deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "INSERT INTO prCierreMesCostos (EmpresaID,PeriodoID,CodigoBSID,inReferenciaID,NumeroDoc,VlrLocal,VlrExtra,eg_prBienServicio,eg_inReferencia)" +
                    "VALUES  (@EmpresaID,@PeriodoID,@CodigoBSID,@inReferenciaID,@NumeroDoc,@VlrLocal,@VlrExtra,@eg_prBienServicio,@eg_inReferencia)";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrLocal", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrExtra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = deta.PeriodoID.Value;
                mySqlCommandSel.Parameters["@CodigoBSID"].Value = deta.CodigoBSID.Value;
                mySqlCommandSel.Parameters["@inReferenciaID"].Value = deta.inReferenciaID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = deta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@VlrLocal"].Value = deta.VlrLocal.Value;
                mySqlCommandSel.Parameters["@VlrExtra"].Value = deta.VlrExtra.Value;
                mySqlCommandSel.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
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
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prCierreMesCostos_AddItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="total">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        private void DAL_prCierreMesCostos_UpdateItem(DTO_prCierreMesCostos deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "UPDATE prCierreMesCostos " +
                    "   SET " +
                    "       NumeroDoc = @NumeroDoc " +
                    "       ,VlrLocal = @VlrLocal " +
                    "       ,VlrExtra = @VlrExtra " +
                    "WHERE EmpresaID= @EmpresaID AND PeriodoID= @PeriodoID AND CodigoBSID= @CodigoBSID " +
                    "   AND inReferenciaID= @inReferenciaID and eg_prBienServicio=@eg_prBienServicio,eg_inReferencia=@eg_inReferencia";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrLocal", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrExtra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = deta.PeriodoID.Value;
                mySqlCommandSel.Parameters["@CodigoBSID"].Value = deta.CodigoBSID.Value;
                mySqlCommandSel.Parameters["@inReferenciaID"].Value = deta.inReferenciaID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = deta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@VlrLocal"].Value = deta.VlrLocal.Value;
                mySqlCommandSel.Parameters["@VlrExtra"].Value = deta.VlrExtra.Value;
                mySqlCommandSel.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
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
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prCierreMesCostos_UpdateItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de prCierreMesCostos
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_prCierreMesCostos_Add(DTO_prCierreMesCostos deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                   "SELECT COUNT (*) from prCierreMesCostos with(nolock) " +
                   "WHERE EmpresaID= @EmpresaID AND PeriodoID= @PeriodoID AND CodigoBSID= @CodigoBSID " +
                    "   AND inReferenciaID= @inReferenciaID and eg_CodigoBS=@eg_CodigoBS,eg_Referencia=@eg_Referencia";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_CodigoBS", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_Referencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = deta.PeriodoID.Value;
                mySqlCommandSel.Parameters["@CodigoBSID"].Value = deta.CodigoBSID.Value;
                mySqlCommandSel.Parameters["@inReferenciaID"].Value = deta.inReferenciaID.Value;
                mySqlCommandSel.Parameters["@eg_CodigoBS"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_Referencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                #endregion

                //Verifica si agrega o actualiza el registro
                int count = Convert.ToInt32(mySqlCommandSel.ExecuteScalar());
                if (count == 0)
                    this.DAL_prCierreMesCostos_AddItem(deta);
                else
                    this.DAL_prCierreMesCostos_UpdateItem(deta);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prCierreMesCostos_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Trae el documento asociado a un cierre
        /// </summary>
        /// <returns></returns>
        public List<DTO_prCierreMesCostos> DAL_prCierreMesCostos_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_prCierreMesCostos> results = new List<DTO_prCierreMesCostos>();

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                mySqlCommand.CommandText = "SELECT deta.*, proy.ContratoID, proy.ActividadID,proy.LocFisicaID from prCierreMesCostos deta with(nolock)  " +
                                           "inner join coProyecto proy on proy.ProyectoID = deta.ProyectoID and deta.eg_coProyecto = proy.EmpresaGrupoID  " +
                                           "WHERE NumeroDoc= @NumeroDoc";
                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_prCierreMesCostos deta = new DTO_prCierreMesCostos(dr);
                    results.Add(deta);
                }

                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prCierreMesCostos_Get");
                throw exception;
            }
        }

        #endregion

        #region Otras
        /// <summary>
        /// Trae la informacion de los cierres proveedores
        /// </summary>
        /// <returns>Lista de cierres</returns>
        public List<DTO_prCierreMesCostos> DAL_prCierreMesCostos_GetByParameter(DateTime periodo, string codigoBSID, string referenciaID, int? numeroDocOC)
        {
            List<DTO_prCierreMesCostos> results = new List<DTO_prCierreMesCostos>();

            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = "select * from prCierreMesCostos doc with(nolock) " +
                        "where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                query += "and PeriodoID = @PeriodoID ";
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;

                if (!string.IsNullOrEmpty(codigoBSID))
                {
                    query += "and CodigoBSID = @CodigoBSID ";
                    mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                    mySqlCommand.Parameters["@CodigoBSID"].Value = codigoBSID;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(referenciaID))
                {
                    query += "and inReferenciaID = @inReferenciaID ";
                    mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                    mySqlCommand.Parameters["@inReferenciaID"].Value = referenciaID;
                    filterInd = true;
                }

                if (numeroDocOC != null)
                {
                    query += "and NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDocOC;
                    filterInd = true;
                }
                mySqlCommand.CommandText = query;

                if (!filterInd)
                    return results;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_prCierreMesCostos total = new DTO_prCierreMesCostos(dr);
                    results.Add(total);
                    index++;
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prCierreMesCostos_GetByParameter");
                throw exception;
            }
        }
        #endregion

    }
}
