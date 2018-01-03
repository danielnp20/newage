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
    public class DAL_plCierreCompromisos : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_plCierreCompromisos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega un registro de cierre 
        /// </summary>
        /// <param name="deta">Cierre nuevo</param>
        /// <returns></returns>
        public void DAL_plCierreCompromisos_AddItem(DTO_plCierreCompromisos deta)
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
                       " EmpresaID "+
                       " ,PeriodoID "+
                       " ,NumeroDoc "+
                       " ,ProyectoID "+
                       " ,CentroCostoID "+
                       " ,LineaPresupuestoID "+
                       " ,CompActLocML "+
                       " ,CompActLocME "+
                       " ,CompActExtME "+
                       " ,CompActExtML "+
                       " ,CompSgtIniLocML "+
                       " ,CompSgtIniLocME "+
                       " ,CompSgtIniExtME "+
                       " ,CompSgtIniExtML "+
                       " ,eg_coProyecto "+
                       " ,eg_coCentroCosto "+
                       " ,eg_plLineaPresupuesto " +
                    ") " +
                    "VALUES " +
                    "( " +
                       " @EmpresaID " +
                       " ,@PeriodoID " +
                       " ,@NumeroDoc " +
                       " ,@ProyectoID " +
                       " ,@CentroCostoID " +
                       " ,@LineaPresupuestoID " +
                       " ,@CompActLocML " +
                       " ,@CompActLocME " +
                       " ,@CompActExtME " +
                       " ,@CompActExtML " +
                       " ,@CompSgtIniLocML " +
                       " ,@CompSgtIniLocME " +
                       " ,@CompSgtIniExtME " +
                       " ,@CompSgtIniExtML " +
                       " ,@eg_coProyecto " +
                       " ,@eg_coCentroCosto " +
                       " ,@eg_plLineaPresupuesto " +
                    ") ";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompActLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompActLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompActExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompActExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompSgtIniLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompSgtIniLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompSgtIniExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompSgtIniExtML", SqlDbType.Decimal);
                //Eg
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = deta.PeriodoID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = deta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = deta.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = deta.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@CompActLocML"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CompActLocME"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CompActExtME"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CompActExtML"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CompSgtIniLocML"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CompSgtIniLocME"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CompSgtIniExtME"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CompSgtIniExtML"].Value = deta.LineaPresupuestoID.Value;
                
                //Eg
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                #endregion

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plCierreCompromisos_AddItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="cierre">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_plCierreCompromisos_Delete(int NumeroDoc)
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plCierreCompromisos_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae el documento asociado a un presupuesto
        /// </summary>
        /// <returns></returns>
        public List<DTO_plCierreCompromisos> DAL_plCierreCompromisos_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_plCierreCompromisos> results = new List<DTO_plCierreCompromisos>();

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                mySqlCommand.CommandText = "select * from plPresupuestoDeta with(nolock) WHERE NumeroDoc= @NumeroDoc";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_plCierreCompromisos deta = new DTO_plCierreCompromisos(dr);
                    results.Add(deta);
                }

                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plCierreCompromisos_Get");
                throw exception;
            }
        }

        #endregion
    }
}
