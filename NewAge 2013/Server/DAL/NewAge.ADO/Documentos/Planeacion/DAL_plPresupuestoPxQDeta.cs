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
    public class DAL_plPresupuestoPxQDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_plPresupuestoPxQDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega informacion a la tabla plPresupuestoPxQ
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private void DAL_plPresupuestoPxQDeta_AddItem(DTO_plPresupuestoPxQDeta pxq)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "INSERT INTO plPresupuestoPxQDeta " +
                    "( " +
                         "NumeroDoc,Ano,ProyectoID,CentroCostoID,LineaPresupuestoID,CodigoBSID, inReferenciaID,UnidadInvID," +
                         "RecursoID,CantidadPRELoc,CantidadPREExt,ValorUniOCLoc,ValorUniOCExt,PorcentajeVar,NumeroDocOC,ValorUniLoc,ValorUniExt,CantidadSOL,CantidadPEN,DescripTExt," +
                         "eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto,eg_prBienServicio,eg_inReferencia,eg_inUnidad,eg_pyRecurso" +
                    ") " +
                    "VALUES " +
                    "( " +
                         "@NumeroDoc,@Ano,@ProyectoID,@CentroCostoID,@LineaPresupuestoID,@CodigoBSID, @inReferenciaID,@UnidadInvID," +
                         "@RecursoID,@CantidadPRELoc,@CantidadPREExt,@ValorUniOCLoc,@ValorUniOCExt,@PorcentajeVar,@NumeroDocOC,@ValorUniLoc,@ValorUniExt,@CantidadSOL,@CantidadPEN,@DescripTExt," +
                         "@eg_coProyecto,@eg_coCentroCosto,@eg_plLineaPresupuesto,@eg_prBienServicio,@eg_inReferencia,@eg_inUnidad,@eg_pyRecurso" +
                    ") ";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Ano", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommandSel.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@UnidadInvID", SqlDbType.Char, UDT_UnidadInvID.MaxLength);
                //Valores
                mySqlCommandSel.Parameters.Add("@RecursoID", SqlDbType.Char, UDT_RecursoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CantidadPRELoc", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadPREExt", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorUniOCLoc", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorUniOCExt", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorcentajeVar", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@NumeroDocOC", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ValorUniLoc", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorUniExt", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadPEN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DescripTExt", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                //Eg
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_inUnidad", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyRecurso", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = pxq.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Ano"].Value = pxq.Ano.Value.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = pxq.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = pxq.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = pxq.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CodigoBSID"].Value = pxq.CodigoBSID.Value;
                mySqlCommandSel.Parameters["@inReferenciaID"].Value = pxq.inReferenciaID.Value;
                mySqlCommandSel.Parameters["@UnidadInvID"].Value = pxq.UnidadInvID.Value;
                //Valores
                mySqlCommandSel.Parameters["@RecursoID"].Value = pxq.RecursoID.Value;/////////
                mySqlCommandSel.Parameters["@CantidadPRELoc"].Value = pxq.CantidadPRELoc.Value;
                mySqlCommandSel.Parameters["@CantidadPREExt"].Value = pxq.CantidadPREExt.Value;
                mySqlCommandSel.Parameters["@ValorUniOCLoc"].Value = pxq.ValorUniOCLoc.Value;
                mySqlCommandSel.Parameters["@ValorUniOCExt"].Value = pxq.ValorUniOCExt.Value;
                mySqlCommandSel.Parameters["@PorcentajeVar"].Value = pxq.PorcentajeVar.Value;
                mySqlCommandSel.Parameters["@NumeroDocOC"].Value = pxq.NumeroDocOC.Value;
                mySqlCommandSel.Parameters["@ValorUniLoc"].Value = pxq.ValorUniLoc.Value;
                mySqlCommandSel.Parameters["@ValorUniExt"].Value = pxq.ValorUniExt.Value;
                mySqlCommandSel.Parameters["@CantidadSOL"].Value = pxq.CantidadSOL.Value;
                mySqlCommandSel.Parameters["@CantidadPEN"].Value = pxq.CantidadPEN.Value;
                mySqlCommandSel.Parameters["@DescripTExt"].Value = pxq.DescripTExt.Value;
                //Eg
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_inUnidad"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inUnidad, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_pyRecurso"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyRecurso, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoPxQDeta_AddItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="pxq">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        private void DAL_plPresupuestoPxQDeta_UpdateItem(DTO_plPresupuestoPxQDeta pxq)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "UPDATE plPresupuestoPxQDeta " +
                    "SET " +
                    "   CantidadPRELoc = CantidadPRELoc + @CantidadPRELoc, " +
                    "   CantidadPREExt = CantidadPREExt + @CantidadPREExt, " +
                    "   ValorUniOCLoc = ValorUniOCLoc + @ValorUniOCLoc, " +
                    "   ValorUniOCExt = ValorUniOCExt + @ValorUniOCExt,  " +
                    "   ValorUniLoc = ValorUniLoc + @ValorUniLoc, " +
                    "   ValorUniExt = ValorUniExt + @ValorUniExt,  " +
                    "   PorcentajeVar = @PorcentajeVar,  " +
                    "   NumeroDocOC = @NumeroDocOC,  " +
                    "   DescripTExt = @DescripTExt,  " +
                    "   CantidadSOL = CantidadSOL + @CantidadSOL,  " +
                    "   CantidadPEN = CantidadPEN + @CantidadPEN  " +
                    "WHERE NumeroDoc = @NumeroDoc AND Ano= @Ano AND ProyectoID= @ProyectoID " +
                    "   AND CentroCostoID= @CentroCostoID AND LineaPresupuestoID= @LineaPresupuestoID" +
                    "   AND CodigoBSID= @CodigoBSID AND inReferenciaID= @inReferenciaID" +
                    "   AND eg_coProyecto=@eg_coProyecto AND eg_coCentroCosto=@eg_coCentroCosto AND eg_plLineaPresupuesto=@eg_plLineaPresupuesto " +
                    "   AND eg_inReferencia = @eg_inReferencia AND eg_prBienServicio=@eg_prBienServicio ";

                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Ano", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommandSel.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                //Valores
                mySqlCommandSel.Parameters.Add("@UnidadInvID", SqlDbType.Char, UDT_UnidadInvID.MaxLength);                
                mySqlCommandSel.Parameters.Add("@CantidadPRELoc", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadPREExt", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorUniOCLoc", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorUniOCExt", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorcentajeVar", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@NumeroDocOC", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ValorUniLoc", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorUniExt", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadPEN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DescripTExt", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                // EG
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = pxq.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Ano"].Value = pxq.Ano.Value.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = pxq.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = pxq.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = pxq.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CodigoBSID"].Value = pxq.CodigoBSID.Value;
                mySqlCommandSel.Parameters["@inReferenciaID"].Value = pxq.inReferenciaID.Value;
                //Valores
                mySqlCommandSel.Parameters["@UnidadInvID"].Value = pxq.UnidadInvID.Value;                
                mySqlCommandSel.Parameters["@CantidadPRELoc"].Value = pxq.CantidadPRELoc.Value;
                mySqlCommandSel.Parameters["@CantidadPREExt"].Value = pxq.CantidadPREExt.Value;
                mySqlCommandSel.Parameters["@ValorUniOCLoc"].Value = pxq.ValorUniOCLoc.Value;
                mySqlCommandSel.Parameters["@ValorUniOCExt"].Value = pxq.ValorUniOCExt.Value;
                mySqlCommandSel.Parameters["@PorcentajeVar"].Value = pxq.PorcentajeVar.Value;
                mySqlCommandSel.Parameters["@NumeroDocOC"].Value = pxq.NumeroDocOC.Value;
                mySqlCommandSel.Parameters["@ValorUniLoc"].Value = pxq.ValorUniLoc.Value;
                mySqlCommandSel.Parameters["@ValorUniExt"].Value = pxq.ValorUniExt.Value;
                mySqlCommandSel.Parameters["@CantidadSOL"].Value = pxq.CantidadSOL.Value;
                mySqlCommandSel.Parameters["@CantidadPEN"].Value = pxq.CantidadPEN.Value;
                mySqlCommandSel.Parameters["@DescripTExt"].Value = pxq.DescripTExt.Value;
                // EG
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoPxQDeta_UpdateItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de plPresupuestoPxQ
        /// </summary>
        /// <param name="pxq">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_plPresupuestoPxQDeta_Add(DTO_plPresupuestoPxQDeta pxq)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT COUNT (*) from plPresupuestoPxQDeta with(nolock) " +
                    "WHERE Ano= @Ano AND ProyectoID= @ProyectoID " +
                    "   AND CentroCostoID= @CentroCostoID AND LineaPresupuestoID= @LineaPresupuestoID" +
                    "   AND CodigoBSID= @CodigoBSID AND inReferenciaID= @inReferenciaID" +
                    "   AND eg_coProyecto=@eg_coProyecto AND eg_coCentroCosto=@eg_coCentroCosto AND eg_plLineaPresupuesto=@eg_plLineaPresupuesto " +
                    "   AND eg_inReferencia = @eg_inReferencia AND eg_prBienServicio=@eg_prBienServicio ";
                     
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@Ano", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommandSel.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                // EG
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@Ano"].Value = pxq.Ano.Value.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = pxq.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = pxq.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = pxq.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CodigoBSID"].Value = pxq.CodigoBSID.Value;
                mySqlCommandSel.Parameters["@inReferenciaID"].Value = pxq.inReferenciaID.Value;
                //EG
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                #endregion

                //Verifica si agrega o actualiza el registro
                int count = Convert.ToInt32(mySqlCommandSel.ExecuteScalar());
                if (count == 0)
                    this.DAL_plPresupuestoPxQDeta_AddItem(pxq);
                else
                {  
                    this.DAL_plPresupuestoPxQDeta_UpdateItem(pxq);
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Err_AddData");
                throw exception;
            }
        }

        /// <summary>
        /// Trae el documento asociado a un presupuesto
        /// </summary>
        /// <returns></returns>
        public List<DTO_plPresupuestoPxQDeta> DAL_plPresupuestoPxQDeta_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_plPresupuestoPxQDeta> results = new List<DTO_plPresupuestoPxQDeta>();

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                mySqlCommand.CommandText = "SELECT deta.*, proy.ContratoID, proy.ActividadID,proy.LocFisicaID from plPresupuestoPxQDeta deta with(nolock)  " +
                                           "inner join coProyecto proy on proy.ProyectoID = deta.ProyectoID and deta.eg_coProyecto = proy.EmpresaGrupoID  " +
                                           "WHERE NumeroDoc= @NumeroDoc";
                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_plPresupuestoPxQDeta deta = new DTO_plPresupuestoPxQDeta(dr);
                    results.Add(deta);
                }

                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoPxQDeta_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de  presupuesto
        /// </summary>
        /// <param name="cierre">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_plPresupuestoPxQDeta_Delete(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommandSel.CommandText = "Delete from plPresupuestoPxQDeta WHERE NumeroDoc= @NumeroDoc";
                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoPxQDeta_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de  presupuesto
        /// </summary>
        /// <param name="cierre">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_plPresupuestoPxQDeta_DeleteByCant(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommandSel.CommandText = "Delete from plPresupuestoPxQDeta WHERE NumeroDoc= @NumeroDoc and CantidadPRELoc = 0 and CantidadPREExt = 0";
                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoPxQDeta_Delete");
                throw exception;
            }
        }

        #endregion

        #region Otras
        /// <summary>
        /// Trae la informacion de los presupuestos consolidados con un filtro
        /// </summary>
        /// <returns>Lista de presupuestos</returns>
        public List<DTO_plPresupuestoPxQDeta> DAL_plPresupuestoPxQDeta_GetByParameter(DTO_plPresupuestoPxQDeta filter)
        {
            List<DTO_plPresupuestoPxQDeta> results = new List<DTO_plPresupuestoPxQDeta>();

            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = "select * from plPresupuestoPxQDeta doc with(nolock) " +
                        "where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if (!string.IsNullOrEmpty(filter.Ano.Value.ToString()))
                {
                    query += "and Ano = @Ano ";
                    mySqlCommand.Parameters.Add("@Ano", SqlDbType.Int);
                    mySqlCommand.Parameters["@Ano"].Value = filter.Ano.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ProyectoID.Value.ToString()))
                {
                    query += "and ProyectoID = @ProyectoID ";
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@ProyectoID"].Value = filter.ProyectoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.LineaPresupuestoID.Value.ToString()))
                {
                    query += "and LineaPresupuestoID = @LineaPresupuestoID ";
                    mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                    mySqlCommand.Parameters["@LineaPresupuestoID"].Value = filter.LineaPresupuestoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.CentroCostoID.Value.ToString()))
                {
                    query += "and CentroCostoID = @CentroCostoID ";
                    mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommand.Parameters["@CentroCostoID"].Value = filter.CentroCostoID.Value;
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
                    DTO_plPresupuestoPxQDeta total = new DTO_plPresupuestoPxQDeta(dr);
                    results.Add(total);
                    index++;
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoPxQDeta_GetByParameter");
                throw exception;
            }
        }
        #endregion

    }
}
