using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Threading;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_prSolicitudCargos
    /// </summary>
    public class DAL_prSolicitudCargos : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prSolicitudCargos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region DAL_prSolicitudCargos

        #region Funciones publicas

        /// <summary>
        /// Consulta un prSolicitudCargos segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns>lista de Cargos</returns>
        public List<DTO_prSolicitudCargos> DAL_prSolicitudCargos_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prSolicitudCargos with(nolock) where prSolicitudCargos.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                List<DTO_prSolicitudCargos> footer = new List<DTO_prSolicitudCargos>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_prSolicitudCargos detail = new DTO_prSolicitudCargos(dr);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSolicitudCargos_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta un prSolicitudCargos segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns>lista de Cargos</returns>
        public List<DTO_prSolicitudCargos> DAL_prSolicitudCargos_GetByID(int ConsecutivoDetaID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prSolicitudCargos with(nolock) where prSolicitudCargos.ConsecutivoDetaID = @ConsecutivoDetaID ";

                mySqlCommand.Parameters.Add("@ConsecutivoDetaID", SqlDbType.Int);
                mySqlCommand.Parameters["@ConsecutivoDetaID"].Value = ConsecutivoDetaID;

                List<DTO_prSolicitudCargos> footer = new List<DTO_prSolicitudCargos>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_prSolicitudCargos detail = new DTO_prSolicitudCargos(dr);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSolicitudCargos_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona en tabla prSolicitudCargos
        /// </summary>
        /// <param name="footer">Cargos</param>
        public void DAL_prSolicitudCargos_Add(List<DTO_prSolicitudCargos> footer)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prSolicitudCargos " +
                                           "    (EmpresaID    " +
                                           "    ,NumeroDoc    " +
                                           "    ,ConsecutivoDetaID " +
                                           "    ,ProyectoID " +
                                           "    ,CentroCostoID " +
                                           "    ,LineaPresupuestoID " +
                                           "    ,PorcentajeID " +
                                           "    ,eg_coProyecto " +
                                           "    ,eg_coCentroCosto " +
                                           "    ,eg_plLineaPresupuesto) " +
                                           "    VALUES" +
                                           "    (@EmpresaID " +
                                           "    ,@NumeroDoc " +
                                           "    ,@ConsecutivoDetaID " +
                                           "    ,@ProyectoID " +
                                           "    ,@CentroCostoID " +
                                           "    ,@LineaPresupuestoID " +
                                           "    ,@PorcentajeID " +
                                           "    ,@eg_coProyecto " +
                                           "    ,@eg_coCentroCosto " +
                                           "    ,@eg_plLineaPresupuesto) ";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ConsecutivoDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommand.Parameters.Add("@PorcentajeID", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                foreach (DTO_prSolicitudCargos det in footer)
                {
                    #region Asignacion de valores
                    mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommand.Parameters["@NumeroDoc"].Value = det.NumeroDoc.Value;
                    mySqlCommand.Parameters["@ConsecutivoDetaID"].Value = det.ConsecutivoDetaID.Value;
                    mySqlCommand.Parameters["@ProyectoID"].Value = det.ProyectoID.Value;
                    mySqlCommand.Parameters["@CentroCostoID"].Value = det.CentroCostoID.Value;
                    mySqlCommand.Parameters["@LineaPresupuestoID"].Value = det.LineaPresupuestoID.Value;
                    mySqlCommand.Parameters["@PorcentajeID"].Value = det.PorcentajeID.Value;    
                    mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSolicitudCargos_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina registros de la tabla de prSolicitudCargos
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public void DAL_prSolicitudCargos_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText = "DELETE FROM prSolicitudCargos where EmpresaID = @EmpresaID " +
                " and NumeroDoc = @NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSolicitudCargos_Delete");
                throw exception;
            }
        }

        #endregion 

        #endregion
    }
}
