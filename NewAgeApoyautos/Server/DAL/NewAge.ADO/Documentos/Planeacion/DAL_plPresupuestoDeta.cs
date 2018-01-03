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
    public class DAL_plPresupuestoDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_plPresupuestoDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega un registro de cierre 
        /// </summary>
        /// <param name="deta">Cierre nuevo</param>
        /// <returns></returns>
        public void DAL_plPresupuestoDeta_AddItem(DTO_plPresupuestoDeta deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "INSERT INTO plPresupuestoDeta " +
                    "( " +
                        "NumeroDoc,ProyectoID,CentroCostoID,LineaPresupuestoID,Ano," +
                        "ValorLoc00,ValorLoc01,ValorLoc02,ValorLoc03,ValorLoc04,ValorLoc05,ValorLoc06,ValorLoc07,ValorLoc08,ValorLoc09,ValorLoc10,ValorLoc11,ValorLoc12," +
                        "EquivExt00,EquivExt01,EquivExt02,EquivExt03,EquivExt04,EquivExt05,EquivExt06,EquivExt07,EquivExt08,EquivExt09,EquivExt10,EquivExt11,EquivExt12," +
                        "ValorExt00,ValorExt01,ValorExt02,ValorExt03,ValorExt04,ValorExt05,ValorExt06,ValorExt07,ValorExt08,ValorExt09,ValorExt10,ValorExt11,ValorExt12," +
                        "EquivLoc00,EquivLoc01,EquivLoc02,EquivLoc03,EquivLoc04,EquivLoc05,EquivLoc06,EquivLoc07,EquivLoc08,EquivLoc09,EquivLoc10,EquivLoc11,EquivLoc12," +
                        "Porcentaje01,Porcentaje02,Porcentaje03,Porcentaje04,Porcentaje05,Porcentaje06,Porcentaje07,Porcentaje08,Porcentaje09,Porcentaje10,Porcentaje11,Porcentaje12," +
                        "DescripTExt,eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto" + 
                    ") " +
                    "VALUES " +
                    "( " +
                        "@NumeroDoc,@ProyectoID,@CentroCostoID,@LineaPresupuestoID,@Ano," +
                        "@ValorLoc00,@ValorLoc01,@ValorLoc02,@ValorLoc03,@ValorLoc04,@ValorLoc05,@ValorLoc06,@ValorLoc07,@ValorLoc08,@ValorLoc09,@ValorLoc10,@ValorLoc11,@ValorLoc12," +
                        "@EquivExt00,@EquivExt01,@EquivExt02,@EquivExt03,@EquivExt04,@EquivExt05,@EquivExt06,@EquivExt07,@EquivExt08,@EquivExt09,@EquivExt10,@EquivExt11,@EquivExt12," +
                        "@ValorExt00,@ValorExt01,@ValorExt02,@ValorExt03,@ValorExt04,@ValorExt05,@ValorExt06,@ValorExt07,@ValorExt08,@ValorExt09,@ValorExt10,@ValorExt11,@ValorExt12," +
                        "@EquivLoc00,@EquivLoc01,@EquivLoc02,@EquivLoc03,@EquivLoc04,@EquivLoc05,@EquivLoc06,@EquivLoc07,@EquivLoc08,@EquivLoc09,@EquivLoc10,@EquivLoc11,@EquivLoc12," +
                        "@Porcentaje01,@Porcentaje02,@Porcentaje03,@Porcentaje04,@Porcentaje05,@Porcentaje06,@Porcentaje07,@Porcentaje08,@Porcentaje09,@Porcentaje10,@Porcentaje11,@Porcentaje12," +                        
                        "@DescripTExt,@eg_coProyecto,@eg_coCentroCosto,@eg_plLineaPresupuesto" +
                    ") ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Ano", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ValorLoc00", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc01", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc02", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc03", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc04", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc05", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc06", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc07", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc08", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc09", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt00", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt01", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt02", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt03", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt04", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt05", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt06", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt07", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt08", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt09", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt00", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt01", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt02", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt03", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt04", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt05", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt06", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt07", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt08", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt09", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc00", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc01", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc02", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc03", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc04", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc05", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc06", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc07", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc08", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc09", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Porcentaje01", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Porcentaje02", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Porcentaje03", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Porcentaje04", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Porcentaje05", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Porcentaje06", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Porcentaje07", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Porcentaje08", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Porcentaje09", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Porcentaje10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Porcentaje11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Porcentaje12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DescripTExt", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = deta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = deta.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = deta.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@Ano"].Value = deta.Ano.Value; 
                mySqlCommandSel.Parameters["@ValorLoc00"].Value = deta.ValorLoc00.Value;
                mySqlCommandSel.Parameters["@ValorLoc01"].Value = deta.ValorLoc01.Value;
                mySqlCommandSel.Parameters["@ValorLoc02"].Value = deta.ValorLoc02.Value;
                mySqlCommandSel.Parameters["@ValorLoc03"].Value = deta.ValorLoc03.Value;
                mySqlCommandSel.Parameters["@ValorLoc04"].Value = deta.ValorLoc04.Value;
                mySqlCommandSel.Parameters["@ValorLoc05"].Value = deta.ValorLoc05.Value;
                mySqlCommandSel.Parameters["@ValorLoc06"].Value = deta.ValorLoc06.Value;
                mySqlCommandSel.Parameters["@ValorLoc07"].Value = deta.ValorLoc07.Value;
                mySqlCommandSel.Parameters["@ValorLoc08"].Value = deta.ValorLoc08.Value;
                mySqlCommandSel.Parameters["@ValorLoc09"].Value = deta.ValorLoc09.Value;
                mySqlCommandSel.Parameters["@ValorLoc10"].Value = deta.ValorLoc10.Value;
                mySqlCommandSel.Parameters["@ValorLoc11"].Value = deta.ValorLoc11.Value;
                mySqlCommandSel.Parameters["@ValorLoc12"].Value = deta.ValorLoc12.Value;
                mySqlCommandSel.Parameters["@EquivExt00"].Value = deta.EquivExt00.Value;
                mySqlCommandSel.Parameters["@EquivExt01"].Value = deta.EquivExt01.Value;
                mySqlCommandSel.Parameters["@EquivExt02"].Value = deta.EquivExt02.Value;
                mySqlCommandSel.Parameters["@EquivExt03"].Value = deta.EquivExt03.Value;
                mySqlCommandSel.Parameters["@EquivExt04"].Value = deta.EquivExt04.Value;
                mySqlCommandSel.Parameters["@EquivExt05"].Value = deta.EquivExt05.Value;
                mySqlCommandSel.Parameters["@EquivExt06"].Value = deta.EquivExt06.Value;
                mySqlCommandSel.Parameters["@EquivExt07"].Value = deta.EquivExt07.Value;
                mySqlCommandSel.Parameters["@EquivExt08"].Value = deta.EquivExt08.Value;
                mySqlCommandSel.Parameters["@EquivExt09"].Value = deta.EquivExt09.Value;
                mySqlCommandSel.Parameters["@EquivExt10"].Value = deta.EquivExt10.Value;
                mySqlCommandSel.Parameters["@EquivExt11"].Value = deta.EquivExt11.Value;
                mySqlCommandSel.Parameters["@EquivExt12"].Value = deta.EquivExt12.Value;
                mySqlCommandSel.Parameters["@ValorExt00"].Value = deta.ValorExt00.Value;
                mySqlCommandSel.Parameters["@ValorExt01"].Value = deta.ValorExt01.Value;
                mySqlCommandSel.Parameters["@ValorExt02"].Value = deta.ValorExt02.Value;
                mySqlCommandSel.Parameters["@ValorExt03"].Value = deta.ValorExt03.Value;
                mySqlCommandSel.Parameters["@ValorExt04"].Value = deta.ValorExt04.Value;
                mySqlCommandSel.Parameters["@ValorExt05"].Value = deta.ValorExt05.Value;
                mySqlCommandSel.Parameters["@ValorExt06"].Value = deta.ValorExt06.Value;
                mySqlCommandSel.Parameters["@ValorExt07"].Value = deta.ValorExt07.Value;
                mySqlCommandSel.Parameters["@ValorExt08"].Value = deta.ValorExt08.Value;
                mySqlCommandSel.Parameters["@ValorExt09"].Value = deta.ValorExt09.Value;
                mySqlCommandSel.Parameters["@ValorExt10"].Value = deta.ValorExt10.Value;
                mySqlCommandSel.Parameters["@ValorExt11"].Value = deta.ValorExt11.Value;
                mySqlCommandSel.Parameters["@ValorExt12"].Value = deta.ValorExt12.Value;
                mySqlCommandSel.Parameters["@EquivLoc00"].Value = deta.EquivLoc00.Value;
                mySqlCommandSel.Parameters["@EquivLoc01"].Value = deta.EquivLoc01.Value;
                mySqlCommandSel.Parameters["@EquivLoc02"].Value = deta.EquivLoc02.Value;
                mySqlCommandSel.Parameters["@EquivLoc03"].Value = deta.EquivLoc03.Value;
                mySqlCommandSel.Parameters["@EquivLoc04"].Value = deta.EquivLoc04.Value;
                mySqlCommandSel.Parameters["@EquivLoc05"].Value = deta.EquivLoc05.Value;
                mySqlCommandSel.Parameters["@EquivLoc06"].Value = deta.EquivLoc06.Value;
                mySqlCommandSel.Parameters["@EquivLoc07"].Value = deta.EquivLoc07.Value;
                mySqlCommandSel.Parameters["@EquivLoc08"].Value = deta.EquivLoc08.Value;
                mySqlCommandSel.Parameters["@EquivLoc09"].Value = deta.EquivLoc09.Value;
                mySqlCommandSel.Parameters["@EquivLoc10"].Value = deta.EquivLoc10.Value;
                mySqlCommandSel.Parameters["@EquivLoc11"].Value = deta.EquivLoc11.Value;
                mySqlCommandSel.Parameters["@EquivLoc12"].Value = deta.EquivLoc12.Value;
                mySqlCommandSel.Parameters["@Porcentaje01"].Value = deta.Porcentaje01.Value;
                mySqlCommandSel.Parameters["@Porcentaje02"].Value = deta.Porcentaje02.Value;
                mySqlCommandSel.Parameters["@Porcentaje03"].Value = deta.Porcentaje03.Value;
                mySqlCommandSel.Parameters["@Porcentaje04"].Value = deta.Porcentaje04.Value;
                mySqlCommandSel.Parameters["@Porcentaje05"].Value = deta.Porcentaje05.Value;
                mySqlCommandSel.Parameters["@Porcentaje06"].Value = deta.Porcentaje06.Value;
                mySqlCommandSel.Parameters["@Porcentaje07"].Value = deta.Porcentaje07.Value;
                mySqlCommandSel.Parameters["@Porcentaje08"].Value = deta.Porcentaje08.Value;
                mySqlCommandSel.Parameters["@Porcentaje09"].Value = deta.Porcentaje09.Value;
                mySqlCommandSel.Parameters["@Porcentaje10"].Value = deta.Porcentaje10.Value;
                mySqlCommandSel.Parameters["@Porcentaje11"].Value = deta.Porcentaje11.Value;
                mySqlCommandSel.Parameters["@Porcentaje12"].Value = deta.Porcentaje12.Value;
                mySqlCommandSel.Parameters["@DescripTExt"].Value = deta.DescripTExt.Value;
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoDeta_AddItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="cierre">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_plPresupuestoDeta_Delete(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommandSel.CommandText = "Delete from plPresupuestoDeta WHERE NumeroDoc= @NumeroDoc";
                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoDeta_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae el documento asociado a un presupuesto
        /// </summary>
        /// <returns></returns>
        public List<DTO_plPresupuestoDeta> DAL_plPresupuestoDeta_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_plPresupuestoDeta> results = new List<DTO_plPresupuestoDeta>();

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                mySqlCommand.CommandText = "SELECT deta.*, proy.ContratoID, proy.ActividadID,proy.LocFisicaID from plPresupuestoDeta deta with(nolock)  " +
                                           "inner join coProyecto proy on proy.ProyectoID = deta.ProyectoID and deta.eg_coProyecto = proy.EmpresaGrupoID  " +
                                           "WHERE NumeroDoc= @NumeroDoc";
                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_plPresupuestoDeta deta = new DTO_plPresupuestoDeta(dr);
                    results.Add(deta);
                }

                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoDeta_Get");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Trae la informacion de los presupuestos de un usuario por proyecto y periodo
        /// </summary>
        /// <returns></returns>
        public DTO_Presupuesto DAL_plPresupuestoDeta_GetNuevo(int documentoID, string proyectoID, DateTime PeriodoDoc, bool orderByAsc)
        {
            try
            {
                DTO_Presupuesto presupuesto = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Revisa glDocumentoControl

                DTO_glDocumentoControl ctrl = null;

                #region Creacion de comandos
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@seUsuarioID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.SmallDateTime);
                #endregion
                #region Asigna los valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentoID;
                mySqlCommand.Parameters["@seUsuarioID"].Value = this.UserId;
                mySqlCommand.Parameters["@ProyectoID"].Value = proyectoID;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = PeriodoDoc;
                #endregion
                #region CommandText
                mySqlCommand.CommandText =
                    "select * from glDocumentoControl ctrl with(nolock) " +
                    "where ctrl.EmpresaID = @EmpresaID and ctrl.DocumentoID = @DocumentoID " +
                    "  and ctrl.ProyectoID = @ProyectoID and ctrl.PeriodoDoc = @PeriodoDoc" +
                    "  and ctrl.seUsuarioID = @seUsuarioID order By NumeroDoc " + (orderByAsc? "asc": "desc");
                #endregion

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                    ctrl = new DTO_glDocumentoControl(dr);

                dr.Close();

                #endregion

                if (ctrl != null)
                {
                    #region Carga la info del presupuesto

                    List<DTO_plPresupuestoDeta> detalles = this.DAL_plPresupuestoDeta_Get(ctrl.NumeroDoc.Value.Value);

                    #region Presupuesto Consolidado(plPresupuestoTotal)   
                    if (detalles.Count > 0)
                    {
                        mySqlCommand.Parameters.Clear();
                        mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                        mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                        mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                        mySqlCommand.Parameters.Add("@Ano", SqlDbType.Int);
                    }
                    foreach (DTO_plPresupuestoDeta item in detalles)
                    {
                        mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommand.Parameters["@ProyectoID"].Value = item.ProyectoID.Value;
                        mySqlCommand.Parameters["@CentroCostoID"].Value = item.CentroCostoID.Value;
                        mySqlCommand.Parameters["@LineaPresupuestoID"].Value = item.LineaPresupuestoID.Value;
                        mySqlCommand.Parameters["@Ano"].Value = PeriodoDoc.Year;

                        mySqlCommand.CommandText = "select SUM(ValorLoc00 +ValorLoc01+ValorLoc02+ValorLoc03+ValorLoc04+ValorLoc05+ValorLoc06+ValorLoc07+ValorLoc08+ValorLoc09+ValorLoc10+ValorLoc11+ValorLoc12) as SaldoAnteriorLoc," +
                                                   "       SUM(ValorExt00 +ValorExt01+ValorExt02+ValorExt03+ValorExt04+ValorExt05+ValorExt06+ValorExt07+ValorExt08+ValorExt09+ValorExt10+ValorExt11+ValorExt12) as SaldoAnteriorExt from plPresupuestoTotal with(nolock)  " +
                                                   "where EmpresaID=@EmpresaID and ProyectoID=@ProyectoID and Ano=@Ano and CentroCostoID=@CentroCostoID and LineaPresupuestoID=@LineaPresupuestoID";
                        dr = mySqlCommand.ExecuteReader();
                        while (dr.Read())
                        {
                            item.VlrSaldoAntLoc.Value = dr["SaldoAnteriorLoc"] != DBNull.Value ? Math.Round(Convert.ToDecimal(dr["SaldoAnteriorLoc"])) : 0;
                            item.VlrSaldoAntExtr.Value = dr["SaldoAnteriorExt"] != DBNull.Value ? Math.Round(Convert.ToDecimal(dr["SaldoAnteriorExt"])) : 0;
                            item.VlrNuevoSaldoLoc.Value = item.VlrSaldoAntLoc.Value + item.VlrMvtoLocal.Value;
                            item.VlrNuevoSaldoExtr.Value = item.VlrSaldoAntExtr.Value + item.VlrMvtoExtr.Value;
                        }
                        dr.Close();
                    }              
                    #endregion      

                    presupuesto = new DTO_Presupuesto(ctrl, detalles);
                    #endregion
                }

                return presupuesto;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoDeta_GetNuevo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos para aprobar 
        /// </summary>
        /// <returns></returns>
        public List<DTO_PresupuestoAprob> DAL_plPresupuestoDeta_GetNuevosForAprob(int documentoID, string actividadFlujo, string usuarioID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                List<DTO_PresupuestoAprob> result = new List<DTO_PresupuestoAprob>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "select ctrl.NumeroDoc, ctrl.PrefijoID,ctrl.PeriodoDoc, ctrl.DocumentoNro, ctrl.ProyectoID,  deta.CentroCostoID, deta.LineaPresupuestoID, deta.DescripTExt as ObservacionDeta, " +
                    "   proy.Descriptivo as ProyectoDesc, cc.Descriptivo as CentroCostoDesc, lp.Descriptivo as LineaPresDesc, " +
                    "	SUM(ROUND(ValorLoc01+ValorLoc02+ValorLoc03+ValorLoc04+ValorLoc05+ValorLoc06+ValorLoc07+ValorLoc08+ValorLoc09+ValorLoc10+ValorLoc11+ValorLoc12,0)) AS ValorML, " +
                    "	SUM(ROUND(ValorExt01+ValorExt02+ValorExt03+ValorExt04+ValorExt05+ValorExt06+ValorExt07+ValorExt08+ValorExt09+ValorExt10+ValorExt11+ValorExt12,0)) AS ValorME " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "   inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "	        and act.CerradoInd=@CerradoInd and act.ActividadFlujoID=@ActividadFlujoID " +
                    "	inner join plPresupuestoDeta deta with(nolock) on ctrl.NumeroDoc = deta.NumeroDoc " +
                    "	inner join coProyecto proy with(nolock) on ctrl.ProyectoID = proy.ProyectoID  and proy.EmpresaGrupoID = @eg_coProyecto " +
                    "	inner join coCentroCosto cc with(nolock) on deta.CentroCostoID = cc.CentroCostoID and cc.EmpresaGrupoID = @eg_coCentroCosto " +
                    "	inner join plLineaPresupuesto lp with(nolock) on deta.LineaPresupuestoID = lp.LineaPresupuestoID  and lp.EmpresaGrupoID = @eg_plLineaPresupuesto " +
                    "   inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID  " +
                    "   inner join glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID and perm.AreaFuncionalID = ctrl.AreaFuncionalID " +
                    "           and perm.UsuarioID = @UsuarioID " +
                    "where ctrl.EmpresaID = @EmpresaID and DocumentoID = @DocumentoID and Estado = @Estado and perm.ActividadFlujoID = @ActividadFlujoID " +
                    "group by ctrl.NumeroDoc, ctrl.PrefijoID,ctrl.PeriodoDoc, ctrl.DocumentoNro,ctrl.ProyectoID,deta.CentroCostoID, deta.LineaPresupuestoID,deta.DescripTExt,proy.Descriptivo,cc.Descriptivo,lp.Descriptivo ";
                #endregion
                #region Creacion de comandos
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                if (documentoID == AppDocuments.GenerarPresupuestoAprob || documentoID == AppDocuments.ConsolidacionAprob || documentoID == AppDocuments.AreaPresupAprob)
                    mySqlCommand.Parameters["@DocumentoID"].Value = AppDocuments.Presupuesto;
                else if (documentoID == AppDocuments.AdicionPresupuestoAprob)
                    mySqlCommand.Parameters["@DocumentoID"].Value = AppDocuments.AdicionPresupuesto;
                else if (documentoID == AppDocuments.ReclasificPresupuestoAprob)
                    mySqlCommand.Parameters["@DocumentoID"].Value = AppDocuments.ReclasifPresupuesto;
                else
                    mySqlCommand.Parameters["@DocumentoID"].Value = AppDocuments.TrasladoPresupuesto;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuarioID;
                mySqlCommand.Parameters["@Estado"].Value = (Int16)EstadoDocControl.ParaAprobacion;
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actividadFlujo;
                mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                #endregion

                SqlDataReader dr;

                int index = 0;
                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    int numDoc = Convert.ToInt32(dr["NumeroDoc"]);
                    bool nuevo = true;
                    DTO_PresupuestoAprob dto = new DTO_PresupuestoAprob(dr);
                    List<DTO_PresupuestoAprob> list = result.Where(x => ((DTO_PresupuestoAprob)x).NumeroDoc.Value.Value.Equals(numDoc)).ToList();
                    if (list.Count > 0)
                    {
                        dto = list.First();
                        nuevo = false;
                    }
                    else
                        dto = new DTO_PresupuestoAprob(dr);

                    DTO_PresupuestoAprobDetalle dtoDet = new DTO_PresupuestoAprobDetalle(dr);
                    dto.Detalle.Add(dtoDet);

                    if (nuevo)
                    {
                        dto.Index = index;
                        dto.Aprobado.Value = false;
                        dto.Rechazado.Value = false;
                        dto.TotalML.Value = 0;
                        dto.TotalME.Value = 0;
                        result.Add(dto);
                    }
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoDeta_GetNuevosForAprob");
                throw exception;
            }
        }

        #endregion
    }
}
