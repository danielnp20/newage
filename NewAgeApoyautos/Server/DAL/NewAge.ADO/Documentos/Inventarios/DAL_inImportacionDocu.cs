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
    /// DAL_inImportacionDocu
    /// </summary>
    public class DAL_inImportacionDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_inImportacionDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Adiciona en tabla inImportacionDocu
        /// </summary>
        /// <param name="importacionDocu">items a agregar a inImportacionDocu</param>
        /// <returns>Numero Doc</returns>
        public void DAL_inImportacionDocu_Add(DTO_inImportacionDocu importacionDocu)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = " INSERT INTO inImportacionDocu " +
                                            "(NumeroDoc " +
                                            ",EmpresaID " +
                                            ",AgenteAduanaID " +
                                            ",TipoTransporte " +
                                            ",TasaImport " +
                                            ",DeclaracionImp " +
                                            ",DeclaracionValor " +
                                            ",DocImportadora " +
                                            ",DocTransporte " +
                                            ",DocMvtoZonaFranca " +
                                            ",DatoAdd1 " +
                                            ",DatoAdd2 " +
                                            ",DatoAdd3 " +
                                            ",DatoAdd4 " +
                                            ",DatoAdd5 " +
                                            ",Observacion " +
                                            ",ModalidadImp " +
                                            ",eg_prProveedor) " +
                                            "VALUES" +
                                            "(@NumeroDoc " +
                                            ",@EmpresaID " +
                                            ",@AgenteAduanaID " +
                                            ",@TipoTransporte " +
                                            ",@TasaImport " +
                                            ",@DeclaracionImp " +
                                            ",@DeclaracionValor " +
                                            ",@DocImportadora " +
                                            ",@DocTransporte " +
                                            ",@DocMvtoZonaFranca " +
                                            ",@DatoAdd1 " +
                                            ",@DatoAdd2 " +
                                            ",@DatoAdd3 " +
                                            ",@DatoAdd4 " +
                                            ",@DatoAdd5 " +
                                            ",@Observacion " +
                                            ",@ModalidadImp " +
                                            ",@eg_prProveedor) ";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@AgenteAduanaID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@TipoTransporte", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@TasaImport", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DeclaracionImp", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DeclaracionValor", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DocImportadora", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DocTransporte", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DocMvtoZonaFranca", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Observacion", UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@ModalidadImp", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@eg_prProveedor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = importacionDocu.NumeroDoc.Value;
                mySqlCommand.Parameters["@EmpresaID"].Value = importacionDocu.EmpresaID.Value;
                mySqlCommand.Parameters["@AgenteAduanaID"].Value = importacionDocu.AgenteAduanaID.Value;
                mySqlCommand.Parameters["@TipoTransporte"].Value = importacionDocu.TipoTransporte.Value;
                mySqlCommand.Parameters["@TasaImport"].Value = importacionDocu.TasaImport.Value;
                mySqlCommand.Parameters["@DeclaracionImp"].Value = importacionDocu.DeclaracionImp.Value;
                mySqlCommand.Parameters["@DeclaracionValor"].Value = importacionDocu.DeclaracionValor.Value;
                mySqlCommand.Parameters["@DocImportadora"].Value = importacionDocu.DocImportadora.Value;
                mySqlCommand.Parameters["@DocTransporte"].Value = importacionDocu.DocTransporte.Value;
                mySqlCommand.Parameters["@DocMvtoZonaFranca"].Value = importacionDocu.DocMvtoZonaFranca.Value;
                mySqlCommand.Parameters["@DatoAdd1"].Value = importacionDocu.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd2"].Value = importacionDocu.DatoAdd2.Value;
                mySqlCommand.Parameters["@DatoAdd3"].Value = importacionDocu.DatoAdd3.Value;
                mySqlCommand.Parameters["@DatoAdd4"].Value = importacionDocu.DatoAdd4.Value;
                mySqlCommand.Parameters["@DatoAdd5"].Value = importacionDocu.DatoAdd5.Value;
                mySqlCommand.Parameters["@Observacion"].Value = importacionDocu.Observacion.Value;
                mySqlCommand.Parameters["@ModalidadImp"].Value = importacionDocu.ModalidadImp.Value;
                mySqlCommand.Parameters["@eg_prProveedor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prProveedor, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inImportacionDocu_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="fisicoInventario"></param>
        /// <returns>una liquidacion Docu</returns>
        public DTO_inImportacionDocu DAL_inImportacionDocu_GetByNumeroDoc(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from inImportacionDocu with(nolock) where inImportacionDocu.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                DTO_inImportacionDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_inImportacionDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inImportacionDocu_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta una importacion Header segun un filtro de parametros
        /// </summary>
        /// <param name="header">Filtro de parametros</param>
        /// <returns>Dto de importacion Header</returns>
        public List<DTO_inImportacionDocu> DAL_inImportacionDocu_GetByParameter(DTO_inImportacionDocu header)
        {
            try
            {
                List<DTO_inImportacionDocu> result = new List<DTO_inImportacionDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filtroInd = false;

                query = "select * from inImportacionDocu with(nolock) " +
                                           "where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                if (!string.IsNullOrEmpty(header.NumeroDoc.Value.ToString()))
                {
                    query += "and NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = header.NumeroDoc.Value;
                    filtroInd = true;
                }
                if (!string.IsNullOrEmpty(header.AgenteAduanaID.Value.ToString()))
                {
                    query += "and AgenteAduanaID = @AgenteAduanaID ";
                    mySqlCommand.Parameters.Add("@AgenteAduanaID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                    mySqlCommand.Parameters["@AgenteAduanaID"].Value = header.AgenteAduanaID.Value;
                    filtroInd = true;
                }
                if (!string.IsNullOrEmpty(header.TipoTransporte.Value.ToString()))
                {
                    query += "and TipoTransporte = @TipoTransporte ";
                    mySqlCommand.Parameters.Add("@TipoTransporte", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@TipoTransporte"].Value = header.TipoTransporte.Value;
                    filtroInd = true;
                }
                if (!string.IsNullOrEmpty(header.DocTransporte.Value))
                {
                    query += "and DocTransporte = @DocTransporte ";
                    mySqlCommand.Parameters.Add("@DocTransporte", SqlDbType.Char);
                    mySqlCommand.Parameters["@DocTransporte"].Value = header.DocTransporte.Value;
                    filtroInd = true;
                }
                if (!string.IsNullOrEmpty(header.DocImportadora.Value))
                {
                    query += "and DocImportadora = @DocImportadora ";
                    mySqlCommand.Parameters.Add("@DocImportadora", SqlDbType.Char);
                    mySqlCommand.Parameters["@DocImportadora"].Value = header.DocImportadora.Value;
                    filtroInd = true;
                }
                if (!string.IsNullOrEmpty(header.DocMvtoZonaFranca.Value.ToString()))
                {
                    query += "and DocMvtoZonaFranca = @DocMvtoZonaFranca ";
                    mySqlCommand.Parameters.Add("@DocMvtoZonaFranca", SqlDbType.Char);
                    mySqlCommand.Parameters["@DocMvtoZonaFranca"].Value = header.DocMvtoZonaFranca.Value;
                    filtroInd = true;
                }
                if (!string.IsNullOrEmpty(header.DeclaracionImp.Value.ToString()))
                {
                    query += "and DeclaracionImp = @DeclaracionImp ";
                    mySqlCommand.Parameters.Add("@DeclaracionImp", SqlDbType.Char);
                    mySqlCommand.Parameters["@DeclaracionImp"].Value = header.DeclaracionImp.Value;
                    filtroInd = true;
                }
                if (!string.IsNullOrEmpty(header.DeclaracionValor.Value.ToString()))
                {
                    query += "and DeclaracionValor = @DeclaracionValor ";
                    mySqlCommand.Parameters.Add("@DeclaracionValor", SqlDbType.Char);
                    mySqlCommand.Parameters["@DeclaracionValor"].Value = header.DeclaracionValor.Value;
                    filtroInd = true;
                }
                if (!string.IsNullOrEmpty(header.ModalidadImp.Value.ToString()))
                {
                    query += "and ModalidadImp = @ModalidadImp ";
                    mySqlCommand.Parameters.Add("@ModalidadImp", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@ModalidadImp"].Value = header.ModalidadImp.Value;
                    filtroInd = true;
                }

                mySqlCommand.CommandText = query;

                if (!filtroInd)
                    return result;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_inImportacionDocu fisico = new DTO_inImportacionDocu(dr);
                    result.Add(fisico);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inImportacionDocu_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Actualizar inImportacionDocu
        /// </summary>
        /// <param name="mvtoHeader">importacion</param>
        public void DAL_inImportacionDocu_Upd(DTO_inImportacionDocu importacionDocu)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla inImportacionDocu
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE inImportacionDocu " +
                                           "    SET EmpresaID  = @EmpresaID  " +
                                           "    ,AgenteAduanaID  = @AgenteAduanaID  " +
                                           "    ,TipoTransporte  = @TipoTransporte  " +
                                           "    ,TasaImport  = @TasaImport " +
                                           "    ,DeclaracionImp  = @DeclaracionImp " +
                                           "    ,DeclaracionValor  = @DeclaracionValor " +
                                           "    ,DocImportadora  = @DocImportadora " +
                                           "    ,DocTransporte  = @DocTransporte " +
                                           "    ,DocMvtoZonaFranca  = @DocMvtoZonaFranca " +        
                                           "    ,DatoAdd1  = @DatoAdd1 " +
                                           "    ,DatoAdd2  = @DatoAdd2 " +
                                           "    ,DatoAdd3  = @DatoAdd3 " +
                                           "    ,DatoAdd4  = @DatoAdd4 " +
                                           "    ,DatoAdd5  = @DatoAdd5 " +
                                           "    ,Observacion  = @Observacion " +
                                           "    ,ModalidadImp  = @ModalidadImp " +
                                           "    WHERE NumeroDoc = @NumeroDoc";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@AgenteAduanaID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@TipoTransporte", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@TasaImport", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DeclaracionImp", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DeclaracionValor", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DocImportadora", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DocTransporte", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DocMvtoZonaFranca", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Observacion", UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@ModalidadImp", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@eg_prProveedor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = importacionDocu.NumeroDoc.Value;
                mySqlCommand.Parameters["@EmpresaID"].Value = importacionDocu.EmpresaID.Value;
                mySqlCommand.Parameters["@AgenteAduanaID"].Value = importacionDocu.AgenteAduanaID.Value;
                mySqlCommand.Parameters["@TipoTransporte"].Value = importacionDocu.TipoTransporte.Value;
                mySqlCommand.Parameters["@TasaImport"].Value = importacionDocu.TasaImport.Value;
                mySqlCommand.Parameters["@DeclaracionImp"].Value = importacionDocu.DeclaracionImp.Value;
                mySqlCommand.Parameters["@DeclaracionValor"].Value = importacionDocu.DeclaracionValor.Value;
                mySqlCommand.Parameters["@DocImportadora"].Value = importacionDocu.DocImportadora.Value;
                mySqlCommand.Parameters["@DocTransporte"].Value = importacionDocu.DocTransporte.Value;
                mySqlCommand.Parameters["@DocMvtoZonaFranca"].Value = importacionDocu.DocMvtoZonaFranca.Value;
                mySqlCommand.Parameters["@DatoAdd1"].Value = importacionDocu.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd2"].Value = importacionDocu.DatoAdd2.Value;
                mySqlCommand.Parameters["@DatoAdd3"].Value = importacionDocu.DatoAdd3.Value;
                mySqlCommand.Parameters["@DatoAdd4"].Value = importacionDocu.DatoAdd4.Value;
                mySqlCommand.Parameters["@DatoAdd5"].Value = importacionDocu.DatoAdd5.Value;
                mySqlCommand.Parameters["@Observacion"].Value = importacionDocu.Observacion.Value;
                mySqlCommand.Parameters["@ModalidadImp"].Value = importacionDocu.ModalidadImp.Value;
                mySqlCommand.Parameters["@eg_prProveedor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prProveedor, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inImportacionDocu_Upd");
                throw exception;
            }

        }

        #endregion
    }
}
