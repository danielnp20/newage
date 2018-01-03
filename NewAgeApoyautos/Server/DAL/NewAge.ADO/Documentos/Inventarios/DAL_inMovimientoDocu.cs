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
    /// DAL_FacturaDocu
    /// </summary>
    public class DAL_inMovimientoDocu : DAL_Base
    {
       /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_inMovimientoDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Consulta un movimiento Header segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns>Dto de Movimiento Header</returns>
        public DTO_inMovimientoDocu DAL_inMovimientoDocu_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from inMovimientoDocu with(nolock) where inMovimientoDocu.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                DTO_inMovimientoDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_inMovimientoDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inMovimientoDocu_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta un movimiento Header segun un filtro de parametros
        /// </summary>
        /// <param name="header">Filtro de parametros</param>
        /// <returns>Dto de Movimiento Header</returns>
        public List<DTO_inMovimientoDocu> DAL_inMovimientoDocu_GetByParameter(DTO_inMovimientoDocu header)
        {
            try
            {
                List<DTO_inMovimientoDocu> result = new List<DTO_inMovimientoDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filtroActivoInd = false;

                query = "select * from inMovimientoDocu with(nolock) " +
                                           "where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                if (!string.IsNullOrEmpty(header.NumeroDoc.Value.ToString()))
                {
                    query += "and NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = header.NumeroDoc.Value;
                    filtroActivoInd = true;
                }
                if (!string.IsNullOrEmpty(header.MvtoTipoInvID.Value.ToString()))
                {
                    query += "and MvtoTipoInvID = @MvtoTipoInvID ";
                    mySqlCommand.Parameters.Add("@MvtoTipoInvID", SqlDbType.Char, UDT_MvtoTipoID.MaxLength);
                    mySqlCommand.Parameters["@MvtoTipoInvID"].Value = header.MvtoTipoInvID.Value;
                    filtroActivoInd = true;
                }
                if (!string.IsNullOrEmpty(header.BodegaOrigenID.Value))
                {
                    query += "and BodegaOrigenID = @BodegaOrigenID ";
                    mySqlCommand.Parameters.Add("@BodegaOrigenID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                    mySqlCommand.Parameters["@BodegaOrigenID"].Value = header.BodegaOrigenID.Value;
                    filtroActivoInd = true;
                }
                if (!string.IsNullOrEmpty(header.BodegaDestinoID.Value))
                {
                    query += "and BodegaDestinoID = @BodegaDestinoID ";
                    mySqlCommand.Parameters.Add("@BodegaDestinoID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                    mySqlCommand.Parameters["@BodegaDestinoID"].Value = header.BodegaDestinoID.Value;
                    filtroActivoInd = true;
                }
                if (!string.IsNullOrEmpty(header.ClienteID.Value.ToString()))
                {
                    query += "and ClienteID = @ClienteID ";
                    mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                    mySqlCommand.Parameters["@ClienteID"].Value = header.ClienteID.Value;
                    filtroActivoInd = true;
                }
                if (!string.IsNullOrEmpty(header.AgenteAduanaID.Value.ToString()))
                {
                    query += "and AgenteAduanaID = @AgenteAduanaID ";
                    mySqlCommand.Parameters.Add("@AgenteAduanaID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                    mySqlCommand.Parameters["@AgenteAduanaID"].Value = header.AgenteAduanaID.Value;
                    filtroActivoInd = true;
                }
                if (!string.IsNullOrEmpty(header.ModalidadImp.Value.ToString()))
                {
                    query += "and ModalidadImp = @ModalidadImp ";
                    mySqlCommand.Parameters.Add("@ModalidadImp", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@ModalidadImp"].Value = header.ModalidadImp.Value;
                    filtroActivoInd = true;
                }
                if (!string.IsNullOrEmpty(header.DocumentoREL.Value.ToString()))
                {
                    query += "and DocumentoREL = @DocumentoREL ";
                    mySqlCommand.Parameters.Add("@DocumentoREL", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocumentoREL"].Value = header.DocumentoREL.Value;
                    filtroActivoInd = true;
                }
                if (!string.IsNullOrEmpty(header.TipoTransporte.Value.ToString()))
                {
                    query += "and TipoTransporte = @TipoTransporte ";
                    mySqlCommand.Parameters.Add("@TipoTransporte", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@TipoTransporte"].Value = header.TipoTransporte.Value;
                    filtroActivoInd = true;
                }
                if (!string.IsNullOrEmpty(header.VtoFecha.Value.ToString()))
                {
                    query += "and VtoFecha = @VtoFecha ";
                    mySqlCommand.Parameters.Add("@VtoFecha", SqlDbType.DateTime);
                    mySqlCommand.Parameters["@VtoFecha"].Value = header.VtoFecha.Value;
                    filtroActivoInd = true;
                }
                if (!string.IsNullOrEmpty(header.DatoAdd1.Value.ToString()))
                {
                    query += "and DatoAdd1 = @DatoAdd1 ";
                    mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char);
                    mySqlCommand.Parameters["@DatoAdd1"].Value = header.DatoAdd1.Value;
                    filtroActivoInd = true;
                }
                if (!string.IsNullOrEmpty(header.DatoAdd2.Value.ToString()))
                {
                    query += "and DatoAdd2 = @DatoAdd2 ";
                    mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char);
                    mySqlCommand.Parameters["@DatoAdd2"].Value = header.DatoAdd2.Value;
                    filtroActivoInd = true;
                }

                if (!filtroActivoInd)
                    return null;

                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_inMovimientoDocu fisico = new DTO_inMovimientoDocu(dr);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inMovimientoDocu_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona en tabla inMovimientoDocu 
        /// </summary>
        /// <param name="mvtoHeader">movimiento</param>
        public void DAL_inMovimientoDocu_Add(DTO_inMovimientoDocu mvtoHeader)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO inMovimientoDocu " +
                                           " (EmpresaID " +
                                           ",NumeroDoc " +
                                           ",BodegaOrigenID " +
                                           ",BodegaDestinoID " +
                                           ",AsesorID " +
                                           ",MvtoTipoInvID " +
                                           ",DocumentoREL " +
                                           ",ClienteID " +                                           
                                           ",VtoFecha " +
                                           ",Observacion " +
                                           ",AgenteAduanaID " +
                                           ",ModalidadImp " +
                                           ",NotaEnvioREL " +
                                           ",TipoTransporte " +
                                           ",DatoAdd1 " +
                                           ",DatoAdd2 " +
                                           ",DatoAdd3 " +
                                           ",DatoAdd4 " +
                                           ",DatoAdd5 " +
                                           ",DatoAdd6 " +
                                           ",DatoAdd7 " +
                                           ",DatoAdd8 " +
                                           ",DatoAdd9 " +
                                           ",DatoAdd10 " +
                                           ",eg_inBodega " +
                                           ",eg_faAsesor " +
                                           ",eg_inMovimientoTipo " +
                                           ",eg_faCliente " +
                                           ",eg_prProveedor) "+
                                           "VALUES" +
                                           " (@EmpresaID " +
                                           ",@NumeroDoc " +
                                           ",@BodegaOrigenID " +
                                           ",@BodegaDestinoID " +
                                           ",@AsesorID " +
                                           ",@MvtoTipoInvID " +
                                           ",@DocumentoREL " +
                                           ",@ClienteID " +
                                           ",@VtoFecha " +
                                           ",@Observacion " +
                                           ",@AgenteAduanaID " +
                                           ",@ModalidadImp " +
                                           ",@NotaEnvioREL " +
                                           ",@TipoTransporte " +
                                           ",@DatoAdd1 " +
                                           ",@DatoAdd2 " +
                                           ",@DatoAdd3 " +
                                           ",@DatoAdd4 " +
                                           ",@DatoAdd5 " +
                                           ",@DatoAdd6 " +
                                           ",@DatoAdd7 " +
                                           ",@DatoAdd8 " +
                                           ",@DatoAdd9 " +
                                           ",@DatoAdd10 " +
                                           ",@eg_inBodega " +
                                           ",@eg_faAsesor " +
                                           ",@eg_inMovimientoTipo " +
                                           ",@eg_faCliente " +
                                           ",@eg_prProveedor) ";


                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BodegaOrigenID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                mySqlCommand.Parameters.Add("@BodegaDestinoID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                mySqlCommand.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommand.Parameters.Add("@MvtoTipoInvID", SqlDbType.Char, UDT_MvtoTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoREL", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@VtoFecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.Char,UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@AgenteAduanaID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@ModalidadImp", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@NotaEnvioREL", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TipoTransporte", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd6", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd7", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd8", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd9", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd10", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@eg_inBodega", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_faAsesor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inMovimientoTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_faCliente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_prProveedor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = mvtoHeader.NumeroDoc.Value;
                mySqlCommand.Parameters["@BodegaOrigenID"].Value = mvtoHeader.BodegaOrigenID.Value;
                mySqlCommand.Parameters["@BodegaDestinoID"].Value = mvtoHeader.BodegaDestinoID.Value;
                mySqlCommand.Parameters["@AsesorID"].Value = mvtoHeader.AsesorID.Value;
                mySqlCommand.Parameters["@MvtoTipoInvID"].Value = mvtoHeader.MvtoTipoInvID.Value;
                mySqlCommand.Parameters["@DocumentoREL"].Value = mvtoHeader.DocumentoREL.Value;
                mySqlCommand.Parameters["@ClienteID"].Value = mvtoHeader.ClienteID.Value;
                mySqlCommand.Parameters["@VtoFecha"].Value = mvtoHeader.VtoFecha.Value;
                mySqlCommand.Parameters["@Observacion"].Value = mvtoHeader.Observacion.Value;
                mySqlCommand.Parameters["@AgenteAduanaID"].Value = mvtoHeader.AgenteAduanaID.Value;
                mySqlCommand.Parameters["@ModalidadImp"].Value = mvtoHeader.ModalidadImp.Value;
                mySqlCommand.Parameters["@NotaEnvioREL"].Value = mvtoHeader.NotaEnvioREL.Value;
                mySqlCommand.Parameters["@TipoTransporte"].Value = mvtoHeader.TipoTransporte.Value;
                mySqlCommand.Parameters["@DatoAdd1"].Value = mvtoHeader.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd2"].Value = mvtoHeader.DatoAdd2.Value;
                mySqlCommand.Parameters["@DatoAdd3"].Value = mvtoHeader.DatoAdd3.Value;
                mySqlCommand.Parameters["@DatoAdd4"].Value = mvtoHeader.DatoAdd4.Value;
                mySqlCommand.Parameters["@DatoAdd5"].Value = mvtoHeader.DatoAdd5.Value;
                mySqlCommand.Parameters["@DatoAdd6"].Value = mvtoHeader.DatoAdd6.Value;
                mySqlCommand.Parameters["@DatoAdd7"].Value = mvtoHeader.DatoAdd7.Value;
                mySqlCommand.Parameters["@DatoAdd8"].Value = mvtoHeader.DatoAdd8.Value;
                mySqlCommand.Parameters["@DatoAdd9"].Value = mvtoHeader.DatoAdd9.Value;
                mySqlCommand.Parameters["@DatoAdd10"].Value = mvtoHeader.DatoAdd10.Value;
                mySqlCommand.Parameters["@eg_inBodega"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inBodega, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_faAsesor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faAsesor, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inMovimientoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inMovimientoTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_faCliente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faCliente, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inMovimientoDocu_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Actualizar inMovimientoDocu
        /// </summary>
        /// <param name="mvtoHeader">movimiento</param>
        public void DAL_inMovimientoDocu_Upd(DTO_inMovimientoDocu mvtoHeader)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla inMovimientoDocu
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE inMovimientoDocu " +
                                           "    SET EmpresaID  = @EmpresaID  " +
                                           "    ,BodegaOrigenID  = @BodegaOrigenID  " +
                                           "    ,BodegaDestinoID  = @BodegaDestinoID  " +
                                           "    ,AsesorID  = @AsesorID " +
                                           "    ,MvtoTipoInvID  = @MvtoTipoInvID " +
                                           "    ,DocumentoREL  = @DocumentoREL " +
                                           "    ,ClienteID  = @ClienteID " +
                                           "    ,VtoFecha  = @VtoFecha " +
                                           "    ,Observacion  = @Observacion " +
                                           "    ,AgenteAduanaID  = @AgenteAduanaID  " +
                                           "    ,ModalidadImp  = @ModalidadImp " +
                                           "    ,NotaEnvioREL  = @NotaEnvioREL " +
                                           "    ,TipoTransporte  = @TipoTransporte " +
                                           "    ,DatoAdd1  = @DatoAdd1 " +
                                           "    ,DatoAdd2  = @DatoAdd2 " +
                                           "    ,DatoAdd3  = @DatoAdd3 " +
                                           "    ,DatoAdd4  = @DatoAdd4 " +
                                           "    ,DatoAdd5  = @DatoAdd5 " +
                                           "    ,DatoAdd6  = @DatoAdd6 "  +
                                           "    ,DatoAdd7  = @DatoAdd7 "  +
                                           "    ,DatoAdd8  = @DatoAdd8 "  +
                                           "    ,DatoAdd9  = @DatoAdd9 " +
                                           "    ,DatoAdd10 = @DatoAdd10 " +
                                           "    WHERE NumeroDoc = @NumeroDoc";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BodegaOrigenID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                mySqlCommand.Parameters.Add("@BodegaDestinoID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                mySqlCommand.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommand.Parameters.Add("@MvtoTipoInvID", SqlDbType.Char, UDT_MvtoTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoREL", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@VtoFecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@AgenteAduanaID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@ModalidadImp", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@NotaEnvioREL", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TipoTransporte", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd6", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd7", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd8", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd9", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DatoAdd10", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@eg_inBodega", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_faAsesor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inMovimientoTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_faCliente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_prProveedor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = mvtoHeader.NumeroDoc.Value;
                mySqlCommand.Parameters["@BodegaOrigenID"].Value = mvtoHeader.BodegaOrigenID.Value;
                mySqlCommand.Parameters["@BodegaDestinoID"].Value = mvtoHeader.BodegaDestinoID.Value;
                mySqlCommand.Parameters["@AsesorID"].Value = mvtoHeader.AsesorID.Value;
                mySqlCommand.Parameters["@MvtoTipoInvID"].Value = mvtoHeader.MvtoTipoInvID.Value;
                mySqlCommand.Parameters["@DocumentoREL"].Value = mvtoHeader.DocumentoREL.Value;
                mySqlCommand.Parameters["@ClienteID"].Value = mvtoHeader.ClienteID.Value;
                mySqlCommand.Parameters["@VtoFecha"].Value = mvtoHeader.VtoFecha.Value;
                mySqlCommand.Parameters["@Observacion"].Value = mvtoHeader.Observacion.Value;
                mySqlCommand.Parameters["@AgenteAduanaID"].Value = mvtoHeader.AgenteAduanaID.Value;
                mySqlCommand.Parameters["@ModalidadImp"].Value = mvtoHeader.ModalidadImp.Value;
                mySqlCommand.Parameters["@NotaEnvioREL"].Value = mvtoHeader.NotaEnvioREL.Value;
                mySqlCommand.Parameters["@TipoTransporte"].Value = mvtoHeader.TipoTransporte.Value;
                mySqlCommand.Parameters["@DatoAdd1"].Value = mvtoHeader.DatoAdd1.Value;
                mySqlCommand.Parameters["@DatoAdd2"].Value = mvtoHeader.DatoAdd2.Value;
                mySqlCommand.Parameters["@DatoAdd3"].Value = mvtoHeader.DatoAdd3.Value;
                mySqlCommand.Parameters["@DatoAdd4"].Value = mvtoHeader.DatoAdd4.Value;
                mySqlCommand.Parameters["@DatoAdd5"].Value = mvtoHeader.DatoAdd5.Value;
                mySqlCommand.Parameters["@DatoAdd6"].Value = mvtoHeader.DatoAdd6.Value;
                mySqlCommand.Parameters["@DatoAdd7"].Value = mvtoHeader.DatoAdd7.Value;
                mySqlCommand.Parameters["@DatoAdd8"].Value = mvtoHeader.DatoAdd8.Value;
                mySqlCommand.Parameters["@DatoAdd9"].Value = mvtoHeader.DatoAdd9.Value;
                mySqlCommand.Parameters["@DatoAdd10"].Value = mvtoHeader.DatoAdd10.Value;
                mySqlCommand.Parameters["@eg_inBodega"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inBodega, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_faAsesor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faAsesor, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inMovimientoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inMovimientoTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_faCliente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faCliente, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inMovimientoDocu_Upd");
                throw exception;
            }

        }

        /// <summary>
        /// Retorna una lista de NotasEnvio 
        /// </summary>
        /// <returns>Retorna una lista de facturas</returns>
        public List<DTO_NotasEnvioResumen> DAL_NotasEnvio_GetResumen()
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Estado"].Value = EstadoDocControl.Aprobado;

                mySqlCommand.CommandText =
                "select * from inMovimientoDocu mov with(nolock) " +
                    "inner join glDocumentoControl ctrl on (mov.NumeroDoc = ctrl.NumeroDoc) " +
                "where mov.EmpresaID = @EmpresaID and mov.NotaEnvioREL is not null and mov.DocumentoREL is null and ctrl.Estado = @Estado";

                List<DTO_NotasEnvioResumen> result = new List<DTO_NotasEnvioResumen>();
                SqlDataReader dr;

                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_NotasEnvioResumen nota = new DTO_NotasEnvioResumen(dr);
                    result.Add(nota);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_NotasEnvio_GetResumen");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de documentos pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <param name="actividadFlujoID">actividad actual</param>
        /// <returns>Lista de Inventario pendientes a aprobar</returns>
        public List<DTO_inDeterioroAprob> DAL_inMovimientoDocu_GetPendientesByModulo(ModulesPrefix mod, string actividadFlujoID, string usuarioID)
        {
            try
            {
                List<DTO_inDeterioroAprob> result = new List<DTO_inDeterioroAprob>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select distinct ctrl.NumeroDoc, ctrl.PeriodoDoc as PeriodoID, ctrl.DocumentoID,doc.Descriptivo as DocumentoDesc, ctrl.PrefijoID, ctrl.DocumentoNro,  " +
                    "det.inReferenciaID,det.DescripTExt,det.EstadoInv, det.CantidadUNI, det.Valor1LOC, det.Valor1EXT " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "   inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "	    and act.CerradoInd=@CerradoInd and act.ActividadFlujoID=@ActividadFlujoID " +
                    "   inner join inMovimientoDocu header with(nolock) on ctrl.NumeroDoc = header.NumeroDoc" +
                    "	inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                    "   inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID " +
                    "   inner join glMovimientoDetaPRE det with(nolock) on ctrl.EmpresaID = det.EmpresaID and ctrl.NumeroDoc = det.NumeroDoc " +
                    "	inner join glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    "       and perm.UsuarioID = @UsuarioID and perm.AreaFuncionalID = ctrl.AreaFuncionalID  " +
                    "where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado and perm.ActividadFlujoID = @ActividadFlujoID ";

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
                    int numDoc = Convert.ToInt32(dr["NumeroDoc"]);
                    bool nuevo = true;
                    DTO_inDeterioroAprob dtoAsign = new DTO_inDeterioroAprob(dr);
                    List<DTO_inDeterioroAprob> list = result.Where(x => ((DTO_inDeterioroAprob)x).NumeroDoc.Value.Value.Equals(numDoc)).ToList();
                    if (list.Count > 0)
                    {
                        dtoAsign = (DTO_inDeterioroAprob)list.First();
                        nuevo = false;
                    }
                    else
                    {
                        dtoAsign = new DTO_inDeterioroAprob(dr);
                        dtoAsign.Aprobado.Value = false;
                        dtoAsign.Rechazado.Value = false;
                        dtoAsign.Observacion.Value = string.Empty;
                    }

                    DTO_inDeterioroAprobDet solDet = new DTO_inDeterioroAprobDet(dr);
                    dtoAsign.Detalle.Add(solDet);

                    if (nuevo)
                    {
                        if (dtoAsign.Detalle.Count > 0)
                            result.Add(dtoAsign);
                    }
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inMovimientoDocu_GetPendientesByModulo");
                throw exception;
            }
        }

        #endregion
    }
}
