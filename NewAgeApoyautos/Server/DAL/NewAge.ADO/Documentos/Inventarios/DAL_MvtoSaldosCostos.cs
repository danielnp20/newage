using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.Resultados;

namespace NewAge.ADO
{
    public class DAL_MvtoSaldosCostos : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_MvtoSaldosCostos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        #region Control Saldos-Costos

        /// <summary>
        /// Consulta el saldo de existencias
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns>Dto</returns>
        public DTO_inControlSaldosCostos DAL_inControlSaldosCostos_Get(DTO_inControlSaldosCostos filter)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                string query;
                bool filterInd = false;

                query = "Select ctrlCto.*, bodTipo.BodegaTipo, costeo.CosteoTipo "+
                        "  From inControlSaldosCostos ctrlCto with(nolock) " +
                        "  inner join inBodega bod with(nolock) on bod.BodegaID = ctrlCto.BodegaID " +
                        "  inner join inBodegaTipo bodTipo with(nolock) on bodTipo.BodegaTipoID = bod.BodegaTipoID " +
                        "  inner join inCosteoGrupo costeo with(nolock) on costeo.CosteoGrupoInvID = ctrlCto.CosteoGrupoInvID " +
                        "Where ctrlCto.EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                if (!string.IsNullOrEmpty(filter.Periodo.Value.ToString()))
                {
                    query += "and ctrlCto.Periodo = @Periodo ";
                    mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@Periodo"].Value = filter.Periodo.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.BodegaID.Value))
                {
                    query += "and ctrlCto.BodegaID = @BodegaID ";
                    mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                    mySqlCommand.Parameters["@BodegaID"].Value = filter.BodegaID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.inReferenciaID.Value))
                {
                    query += "and ctrlCto.inReferenciaID = @inReferenciaID ";
                    mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_ReferenciaID.MaxLength);
                    mySqlCommand.Parameters["@inReferenciaID"].Value = filter.inReferenciaID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ActivoID.Value.ToString()))
                {
                    query += "and ctrlCto.ActivoID = @ActivoID ";
                    mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                    mySqlCommand.Parameters["@ActivoID"].Value = filter.ActivoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.EstadoInv.Value.ToString()))
                {
                    query += "and ctrlCto.EstadoInv = @EstadoInv ";
                    mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@EstadoInv"].Value = filter.EstadoInv.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Parametro1.Value.ToString()))
                {
                    query += "and ctrlCto.Parametro1 = @Parametro1 ";
                    mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                    mySqlCommand.Parameters["@Parametro1"].Value = filter.Parametro1.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Parametro2.Value.ToString()))
                {
                    query += "and ctrlCto.Parametro2 = @Parametro2 ";
                    mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                    mySqlCommand.Parameters["@Parametro2"].Value = filter.Parametro2.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.IdentificadorTr.Value.ToString()))
                {
                    query += "and ctrlCto.IdentificadorTr = @IdentificadorTr ";
                    mySqlCommand.Parameters.Add("@IdentificadorTr", SqlDbType.Int);
                    mySqlCommand.Parameters["@IdentificadorTr"].Value = filter.IdentificadorTr.Value;
                    filterInd = true;
                }

                mySqlCommand.CommandText = query;
   
                DTO_inControlSaldosCostos result = null;

                if (!filterInd)
                    return result;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_inControlSaldosCostos(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inControlSaldosCostos_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona en tabla inControlSaldosCostos
        /// </summary>
        /// <param name="saldos">movimiento</param>
        public void DAL_inControlSaldosCostos_Add(DTO_inControlSaldosCostos saldos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = " INSERT INTO inControlSaldosCostos " +
                                           "(EmpresaID " +
                                           ",BodegaID " +
                                           ",inReferenciaID " +
                                           ",ActivoID " +
                                           ",EstadoInv " +
                                           ",Parametro1 " +
                                           ",Parametro2 " +
                                           ",IdentificadorTr " +
                                           ",Periodo " +
                                           ",CosteoGrupoInvID" +
                                           ",RegistroSaldo" +
                                           ",RegistroCosto" +
                                           ",eg_inBodega " +
                                           ",eg_inReferencia " +
                                           ",eg_inRefParametro1" +
                                           ",eg_inRefParametro2 " +
                                           ",eg_inCosteoGrupo) " +
                                           "VALUES" +
                                           "(@EmpresaID " +
                                           ",@BodegaID " +
                                           ",@inReferenciaID " +
                                           ",@ActivoID " +
                                           ",@EstadoInv " +
                                           ",@Parametro1 " +
                                           ",@Parametro2 " +
                                           ",@IdentificadorTr " +
                                           ",@Periodo " +
                                           ",@CosteoGrupoInvID" +
                                           ",@RegistroSaldo" +
                                           ",@RegistroCosto" +
                                           ",@eg_inBodega " +
                                           ",@eg_inReferencia " +
                                           ",@eg_inRefParametro1" +
                                           ",@eg_inRefParametro2" +
                                           ",@eg_inCosteoGrupo) ";


                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@IdentificadorTr", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@CosteoGrupoInvID", SqlDbType.Char, UDT_CosteoGrupoInvID.MaxLength);
                mySqlCommand.Parameters.Add("@RegistroSaldo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@RegistroCosto", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@eg_inBodega", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inRefParametro1", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inRefParametro2", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inCosteoGrupo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@BodegaID"].Value = saldos.BodegaID.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = saldos.inReferenciaID.Value;
                mySqlCommand.Parameters["@ActivoID"].Value = saldos.ActivoID.Value;
                mySqlCommand.Parameters["@EstadoInv"].Value = saldos.EstadoInv.Value;
                mySqlCommand.Parameters["@Parametro1"].Value = saldos.Parametro1.Value;
                mySqlCommand.Parameters["@Parametro2"].Value = saldos.Parametro2.Value;
                mySqlCommand.Parameters["@IdentificadorTr"].Value = saldos.IdentificadorTr.Value;
                mySqlCommand.Parameters["@Periodo"].Value = saldos.Periodo.Value;
                mySqlCommand.Parameters["@CosteoGrupoInvID"].Value = saldos.CosteoGrupoInvID.Value;
                mySqlCommand.Parameters["@RegistroSaldo"].Value = saldos.RegistroSaldo.Value;
                mySqlCommand.Parameters["@RegistroCosto"].Value = saldos.RegistroCosto.Value;
                mySqlCommand.Parameters["@eg_inBodega"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inBodega, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inRefParametro1"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inRefParametro1, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inRefParametro2"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inRefParametro2, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inCosteoGrupo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inCosteoGrupo, this.Empresa, egCtrl);
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
                //int numDoc = Convert.ToInt32(mySqlCommand.Parameters["@Consecutivo"].Value);

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inControlSaldosCostos_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="ctrlSaldoCosto"></param>
        /// <returns>Dto de Control de saldos y costos</returns>
        public List<DTO_inControlSaldosCostos> DAL_inControlSaldosCostos_GetByParameter(DTO_inControlSaldosCostos ctrlSaldoCosto, bool CostosIFRSInd = false)
        {
            try
            {
                List<DTO_inControlSaldosCostos> result = new List<DTO_inControlSaldosCostos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string where;

                where = "Select ctrlCto.*, bodTipo.BodegaTipo, costeo.CosteoTipo " +
                         "  From inControlSaldosCostos ctrlCto with(nolock) " +
                         "  inner join inBodega bod with(nolock) on bod.BodegaID = ctrlCto.BodegaID " +
                         "  inner join inBodegaTipo bodTipo with(nolock) on bodTipo.BodegaTipoID = bod.BodegaTipoID " +
                         "  inner join inCosteoGrupo costeo with(nolock) on costeo.CosteoGrupoInvID = ctrlCto.CosteoGrupoInvID " +
                         "Where ctrlCto.EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                if (!string.IsNullOrEmpty(ctrlSaldoCosto.Periodo.Value.ToString()))
                {
                    where += " and ctrlCto.Periodo = @Periodo ";
                    mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@Periodo"].Value = ctrlSaldoCosto.Periodo.Value;
                }
                if (!string.IsNullOrEmpty(ctrlSaldoCosto.BodegaID.Value))
                {
                    where += "and ctrlCto.BodegaID = @BodegaID ";
                    mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                    mySqlCommand.Parameters["@BodegaID"].Value = ctrlSaldoCosto.BodegaID.Value;
                }
                if (!string.IsNullOrEmpty(ctrlSaldoCosto.inReferenciaID.Value))
                {
                    where += " and ctrlCto.inReferenciaID = @inReferenciaID ";
                    mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_ReferenciaID.MaxLength);
                    mySqlCommand.Parameters["@inReferenciaID"].Value = ctrlSaldoCosto.inReferenciaID.Value;
                }
                if (!string.IsNullOrEmpty(ctrlSaldoCosto.ActivoID.Value.ToString()))
                {
                    where += " and ctrlCto.ActivoID = @ActivoID ";
                    mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                    mySqlCommand.Parameters["@ActivoID"].Value = ctrlSaldoCosto.ActivoID.Value;
                }
                if (!string.IsNullOrEmpty(ctrlSaldoCosto.EstadoInv.Value.ToString()))
                {
                    where += " and ctrlCto.EstadoInv = @EstadoInv ";
                    mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@EstadoInv"].Value = ctrlSaldoCosto.EstadoInv.Value;
                }
                if (!string.IsNullOrEmpty(ctrlSaldoCosto.Parametro1.Value.ToString()))
                {
                    where += " and ctrlCto.Parametro1 = @Parametro1 ";
                    mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                    mySqlCommand.Parameters["@Parametro1"].Value = ctrlSaldoCosto.Parametro1.Value;
                }
                if (!string.IsNullOrEmpty(ctrlSaldoCosto.Parametro2.Value.ToString()))
                {
                    where += " and ctrlCto.Parametro2 = @Parametro2 ";
                    mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                    mySqlCommand.Parameters["@Parametro2"].Value = ctrlSaldoCosto.Parametro2.Value;
                }
                if (!string.IsNullOrEmpty(ctrlSaldoCosto.IdentificadorTr.Value.ToString()))
                {
                    where += " and ctrlCto.IdentificadorTr = @IdentificadorTr ";
                    mySqlCommand.Parameters.Add("@IdentificadorTr", SqlDbType.Int);
                    mySqlCommand.Parameters["@IdentificadorTr"].Value = ctrlSaldoCosto.IdentificadorTr.Value;
                }
                if (!string.IsNullOrEmpty(ctrlSaldoCosto.CosteoGrupoInvID.Value.ToString()))
                {
                    where += "and ctrlCto.CosteoGrupoInvID = @CosteoGrupoInvID ";
                    mySqlCommand.Parameters.Add("@CosteoGrupoInvID", SqlDbType.Char, UDT_CosteoGrupoInvID.MaxLength);
                    mySqlCommand.Parameters["@CosteoGrupoInvID"].Value = ctrlSaldoCosto.CosteoGrupoInvID.Value;
                }
                if (ctrlSaldoCosto.BodegaActivaInd.Value.HasValue)
                {
                    where += " and bod.ActivoInd = @BodegaActivaInd ";
                    mySqlCommand.Parameters.Add("@BodegaActivaInd", SqlDbType.Bit);
                    mySqlCommand.Parameters["@BodegaActivaInd"].Value = ctrlSaldoCosto.BodegaActivaInd.Value;
                }

                mySqlCommand.CommandText = where;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_inControlSaldosCostos ctrl = new DTO_inControlSaldosCostos(dr);
                    result.Add(ctrl);
                    index++;
                }
                dr.Close();

                #region Consulta Totales
                if (result.Count > 0)
                {                   
                    foreach (DTO_inControlSaldosCostos saldoCosto in result)
                    {
                        mySqlCommand.Parameters.Clear();
                        mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                        mySqlCommand.Parameters["@Consecutivo"].Value = saldoCosto.RegistroCosto.Value;
                        mySqlCommand.CommandText = "select * from inCostosExistencias where Consecutivo=@Consecutivo";
                        dr = mySqlCommand.ExecuteReader();
                        if (dr.Read())
                        {
                            DTO_inCostosExistencias costos = new DTO_inCostosExistencias(dr);
                            saldoCosto.CostosExistencia = costos;
                            //saldoCosto.CantidadDisp.Value = costos.CantInicial.Value + costos.CantEntrada.Value - costos.CantRetiro.Value;
                            if (!CostosIFRSInd)
                            {                              
                                saldoCosto.ValorLocalDisp.Value = costos.CtoLocSaldoIni.Value + costos.CtoLocEntrada.Value - costos.CtoLocSalida.Value;
                                saldoCosto.ValorFobLocalDisp.Value = costos.FobLocSaldoIni.Value + costos.FobLocEntrada.Value - costos.FobLocSalida.Value;
                                saldoCosto.ValorExtranjeroDisp.Value = costos.CtoExtSaldoIni.Value + costos.CtoExtEntrada.Value - costos.CtoExtSalida.Value;
                                saldoCosto.ValorFobExtDisp.Value = costos.FobExtSaldoIni.Value + costos.FobExtEntrada.Value - costos.FobExtSalida.Value;
                            }
                            else
                            {
                                saldoCosto.ValorLocalDisp.Value = costos.CtoLocSaldoIniIFRS.Value + costos.CtoLocEntradaIFRS.Value - costos.CtoLocSalidaIFRS.Value;
                                saldoCosto.ValorFobLocalDisp.Value = costos.FobLocSaldoIniIFRS.Value + costos.FobLocEntradaIFRS.Value - costos.FobLocSalidaIFRS.Value;
                                saldoCosto.ValorExtranjeroDisp.Value = costos.CtoExtSaldoIniIFRS.Value + costos.CtoExtEntradaIFRS.Value - costos.CtoExtSalidaIFRS.Value;
                                saldoCosto.ValorFobExtDisp.Value = costos.FobExtSaldoIniIFRS.Value + costos.FobExtEntradaIFRS.Value - costos.FobExtSalidaIFRS.Value; 
                            }
                        }
                        dr.Close();                      

                        //Consulta Saldo disponible
                        mySqlCommand.Parameters["@Consecutivo"].Value = saldoCosto.RegistroSaldo.Value;
                        mySqlCommand.CommandText = "select * from inSaldosExistencias where Consecutivo=@Consecutivo";
                        dr = mySqlCommand.ExecuteReader();
                        if (dr.Read())
                        {
                            saldoCosto.SaldosExistencias = new DTO_inSaldosExistencias(dr);
                            saldoCosto.CantidadDisp.Value = saldoCosto.SaldosExistencias.CantInicial.Value + saldoCosto.SaldosExistencias.CantEntrada.Value - saldoCosto.SaldosExistencias.CantRetiro.Value;    
                        }                                                 
                        dr.Close();

                        //Obtiene el costo unitario
                        where = string.Empty;
                        mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                        mySqlCommand.Parameters["@Periodo"].Value = saldoCosto.Periodo.Value;
                        if (!string.IsNullOrEmpty(saldoCosto.CosteoGrupoInvID.Value))
                        {
                            where += " and CosteoGrupoInvID = @CosteoGrupoInvID ";
                            mySqlCommand.Parameters.Add("@CosteoGrupoInvID", SqlDbType.Char, UDT_CosteoGrupoInvID.MaxLength);
                            mySqlCommand.Parameters["@CosteoGrupoInvID"].Value = saldoCosto.CosteoGrupoInvID.Value;
                        }
                        if (!string.IsNullOrEmpty(saldoCosto.inReferenciaID.Value))
                        {
                            where += " and inReferenciaID = @inReferenciaID ";
                            mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_ReferenciaID.MaxLength);
                            mySqlCommand.Parameters["@inReferenciaID"].Value = saldoCosto.inReferenciaID.Value;
                        }
                        mySqlCommand.CommandText = " Select Round((case when CantInicial+CantEntrada-CantRetiro=0 then 0 else (CtoLocSaldoIni+CtoLocEntrada-CtoLocSalida)/ (CantInicial+CantEntrada-CantRetiro) end),3) as CtoUnitarioLoc, " +
                                                    "       Round((case when CantInicial+CantEntrada-CantRetiro=0 then 0 else (CtoExtSaldoIni+CtoExtEntrada-CtoExtSalida)/ (CantInicial+CantEntrada-CantRetiro) end),3) as CtoUnitarioExt " + 
                                                    " From inCostosExistencias " +
                                                    " where EmpresaID = @EmpresaID and Periodo = @Periodo  " + where;
                        dr = mySqlCommand.ExecuteReader();
                        if (dr.Read())
                        {
                            if (!string.IsNullOrEmpty(dr["CtoUnitarioLoc"].ToString()))
                                saldoCosto.CostosExistencia.CtoUnitarioLoc.Value = Convert.ToDecimal(dr["CtoUnitarioLoc"]);
                            else
                                saldoCosto.CostosExistencia.CtoUnitarioLoc.Value = 0;
                            if (!string.IsNullOrEmpty(dr["CtoUnitarioExt"].ToString()))
                                saldoCosto.CostosExistencia.CtoUnitarioExt.Value = Convert.ToDecimal(dr["CtoUnitarioExt"]);
                            else
                                saldoCosto.CostosExistencia.CtoUnitarioExt.Value = 0;
                        }
                        dr.Close();
                    }
                }
                
                #endregion      
                #region Consulta Mvto(glMovimientoDeta)
                if (result.Count > 0)
                {  
                    foreach (DTO_inControlSaldosCostos saldoCosto in result)
                    {
                        where = string.Empty;
                        mySqlCommand.Parameters.Clear();
                        mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        if (!string.IsNullOrEmpty(saldoCosto.BodegaID.Value))
                        {
                            where += " and BodegaID = @BodegaID ";
                            mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                            mySqlCommand.Parameters["@BodegaID"].Value = saldoCosto.BodegaID.Value;
                        }
                        if (!string.IsNullOrEmpty(saldoCosto.inReferenciaID.Value))
                        {
                            where += " and inReferenciaID = @inReferenciaID ";
                            mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_ReferenciaID.MaxLength);
                            mySqlCommand.Parameters["@inReferenciaID"].Value = saldoCosto.inReferenciaID.Value;
                        }              
                        mySqlCommand.CommandText = "select * from glMovimientoDeta where EmpresaID=@EmpresaID " + where;
                        dr = mySqlCommand.ExecuteReader();
                        while(dr.Read())
                        {
                            DTO_glMovimientoDeta mvto = new DTO_glMovimientoDeta(dr);
                            saldoCosto.DetalleMvto.Add(mvto);
                        }
                        dr.Close();
                    }
                }

                #endregion      
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inControlSaldosCostos_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Verifica si hay conceptos de saldo en uso teniendo en cuenta la CuentaID
        /// </summary>
        /// <param name="conceptoSaldoID">Id de concepto saldo</param>
        /// <param name="cuentaID">Id de la cuenta relacionada</param>
        /// <returns>true si existe</returns>
        public bool DAL_MvtoSaldosCostos_SaldoExists(string conceptoSaldoID, string cuentaID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                string query = string.Empty;

                if (cuentaID != null)
                {
                    mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommand.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                    mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);

                    mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommand.Parameters["@ConceptoSaldoID"].Value = conceptoSaldoID;
                    mySqlCommand.Parameters["@CuentaID"].Value = cuentaID;

                    query = "SELECT count(*) FROM coCuentaSaldo with(nolock) " +
                             "WHERE EmpresaID=@EmpresaID and ConceptoSaldoID=@ConceptoSaldoID AND CuentaID=@CuentaID";
                }
                else
                {
                    mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommand.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);

                    mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommand.Parameters["@ConceptoSaldoID"].Value = conceptoSaldoID;

                    query = "SELECT count(*) FROM coCuentaSaldo with(nolock) " +
                            "WHERE EmpresaID=@EmpresaID and ConceptoSaldoID=@ConceptoSaldoID";
                }
                mySqlCommand.CommandText = query;
                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());

                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MvtoSaldosCostos_SaldoExists");
                throw exception;
            }
        }

        #endregion

        #region Saldos

        /// <summary>
        /// Consulta el saldo de existencias
        /// </summary>
        /// <param name="Consecutivo">Consecutivo asociado</param>
        /// <returns>Dto saldos</returns>
        public DTO_inSaldosExistencias DAL_inSaldosExistencias_Get(int Consecutivo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from inSaldosExistencias with(nolock) where inSaldosExistencias.Consecutivo = @Consecutivo ";

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = Consecutivo;

                DTO_inSaldosExistencias result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_inSaldosExistencias(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inSaldosExistencias_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona en tabla inSaldosExistencias
        /// </summary>
        /// <param name="saldos">saldo</param>
        /// <returns>consecutivo</returns>
        public int DAL_inSaldosExistencias_Add(DTO_inSaldosExistencias saldos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = " INSERT INTO inSaldosExistencias " +
                                           "(EmpresaID " +
                                           ",Periodo " +
                                           ",BodegaID " +
                                           ",inReferenciaID " +
                                           ",ActivoID " +
                                           ",EstadoInv " +
                                           ",Parametro1 " +
                                           ",Parametro2 " +
                                           ",IdentificadorTr " +
                                           ",CantInicial" +
                                           ",CantEntrada" +
                                           ",CantRetiro" +
                                           ",eg_inBodega " +
                                           ",eg_inReferencia " +
                                           ",eg_inRefParametro1" +
                                           ",eg_inRefParametro2) " +
                                           "VALUES" +
                                           "(@EmpresaID " +
                                           ",@Periodo " +
                                           ",@BodegaID " +
                                           ",@inReferenciaID " +
                                           ",@ActivoID " +
                                           ",@EstadoInv " +
                                           ",@Parametro1 " +
                                           ",@Parametro2 " +
                                           ",@IdentificadorTr " +
                                           ",@CantInicial" +
                                           ",@CantEntrada" +
                                           ",@CantRetiro" +
                                           ",@eg_inBodega " +
                                           ",@eg_inReferencia " +
                                           ",@eg_inRefParametro1" +
                                           ",@eg_inRefParametro2) " +
                            " SET @Consecutivo = SCOPE_IDENTITY()";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Parametro1", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@Parametro2", SqlDbType.Char, UDT_ParametroInvID.MaxLength);
                mySqlCommand.Parameters.Add("@IdentificadorTr", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CantInicial", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantEntrada", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantRetiro", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_inBodega", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inRefParametro1", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inRefParametro2", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int, 1);
                mySqlCommand.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Periodo"].Value = saldos.Periodo.Value;
                mySqlCommand.Parameters["@BodegaID"].Value = saldos.BodegaID.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = saldos.inReferenciaID.Value;
                mySqlCommand.Parameters["@ActivoID"].Value = saldos.ActivoID.Value;
                mySqlCommand.Parameters["@EstadoInv"].Value = saldos.EstadoInv.Value;
                mySqlCommand.Parameters["@Parametro1"].Value = saldos.Parametro1.Value;
                mySqlCommand.Parameters["@Parametro2"].Value = saldos.Parametro2.Value;
                mySqlCommand.Parameters["@IdentificadorTr"].Value = saldos.IdentificadorTr.Value;
                mySqlCommand.Parameters["@CantInicial"].Value = saldos.CantInicial.Value;
                mySqlCommand.Parameters["@CantEntrada"].Value = saldos.CantEntrada.Value;
                mySqlCommand.Parameters["@CantRetiro"].Value = saldos.CantRetiro.Value;
                mySqlCommand.Parameters["@eg_inBodega"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inBodega, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inRefParametro1"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inRefParametro1, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inRefParametro2"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inRefParametro2, this.Empresa, egCtrl);
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
                int numDoc = Convert.ToInt32(mySqlCommand.Parameters["@Consecutivo"].Value);
                return numDoc;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inSaldosExistencias_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Actualizar en tabla inSaldosExistencias 
        /// </summary>
        /// <param name="saldos">filtro</param>
        public void DAL_inSaldosExistencias_Upd(DTO_inSaldosExistencias saldos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla inSaldosExistencias
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE inSaldosExistencias " +
                                           "    SET Periodo  = @Periodo  " +
                                           "    ,CantInicial  = @CantInicial " +
                                           "    ,CantEntrada  = @CantEntrada " +
                                           "    ,CantRetiro  = @CantRetiro " +
                                           "    WHERE Consecutivo = @Consecutivo";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@CantInicial", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantEntrada", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantRetiro", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@Periodo"].Value = saldos.Periodo.Value;
                mySqlCommand.Parameters["@CantInicial"].Value = saldos.CantInicial.Value;
                mySqlCommand.Parameters["@CantEntrada"].Value = saldos.CantEntrada.Value;
                mySqlCommand.Parameters["@CantRetiro"].Value = saldos.CantRetiro.Value;
                mySqlCommand.Parameters["@Consecutivo"].Value = saldos.Consecutivo.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inCostosExistencias_Upd");
                throw exception;
            }

        }

        /// <summary>
        /// Funcion que obtiene la informacion del serial por el filtro. 
        /// </summary>
        /// <param name="filtro">(Serial, Bodega, inReferenciaID, TerceroID)</param>
        /// <returns>Lista de saldos.</returns>
        public List<DTO_inQuerySeriales> DAL_inSaldosExistencias_GetBySerial(string serial, string bodegaID, string inReferenciaID, string inCliente)
        {
            try
            {
                List<DTO_inQuerySeriales> result = new List<DTO_inQuerySeriales>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Variables

                string serialFiltro = "";
                string bodegaFiltro = "";
                string refFiltro = "";
                string terceroFiltro = "";

                #endregion

                #region Manejo de Filtros

                if (!string.IsNullOrWhiteSpace(serial))
                    serialFiltro = "AND acCtrl.SerialID = " + "'" + serial + " '";
                if (!string.IsNullOrWhiteSpace(bodegaID))
                    bodegaFiltro = "AND saldos.bodegaID = " + "'" + bodegaID + " '";
                if (!string.IsNullOrWhiteSpace(inReferenciaID))
                    refFiltro = "AND saldos.inReferenciaID = " + "'" + inReferenciaID + " '";
                if (!string.IsNullOrWhiteSpace(inCliente))
                    terceroFiltro = "AND ctrl.TerceroID = " + "'" + inCliente + " '";

                #endregion

                #region Command Text

                mySqlCommand.CommandText =
                    "SELECT acCtrl.SerialID, saldos.BodegaID, saldos.inReferenciaID, ctrl.NumeroDoc, " +
                    " Documento = (CAST(ltrim(rtrim(ctrl.PrefijoID)) AS VARCHAR(20))+'-'+ CAST(ctrl.DocumentoNro AS VARCHAR(20))), " +
                    " ctrl.FechaDoc as Periodo, Ctrl.DocumentoID as Tipo, ctrl.DocumentoTercero as DocSoporte " +
                    " FROM INSALDOSEXISTENCIAS saldos " +
                    " INNER JOIN acActivoControl acCtrl ON acCtrl.ActivoID = saldos.IdentificadorTR " +
                    " INNER JOIN glDocumentoControl ctrl ON ctrl.NumeroDoc = acCtrl.NumeroDocUltMvto " +
                    " WHERE saldos.ActivoID <> 0 " +
                    serialFiltro +
                    bodegaFiltro +
                    refFiltro +
                    terceroFiltro;
                
                SqlDataReader dr = mySqlCommand.ExecuteReader(); 
                #endregion
                while (dr.Read())
                {
                    DTO_inQuerySeriales r = new DTO_inQuerySeriales(dr);
                    result.Add(r);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inSaldosExistencias_GetBySerial");
                throw exception;
            }
        }

        #endregion

        #region Costos

        /// <summary>
        /// Consulta el costo de existencias
        /// </summary>
        /// <param name="Consecutivo">Consecutivo a filtrar</param>
        /// <returns>Dto costos</returns>
        public DTO_inCostosExistencias DAL_inCostosExistencias_Get(int Consecutivo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from inCostosExistencias with(nolock) where inCostosExistencias.Consecutivo = @Consecutivo ";

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = Consecutivo;

                DTO_inCostosExistencias result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_inCostosExistencias(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inCostosExistencias_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="costos"></param>
        /// <returns>Dto de Control de saldos y costos</returns>
        public DTO_inCostosExistencias DAL_inCostosExistencias_GetByParameter(DTO_inCostosExistencias costos)
        {
            try
            {
                DTO_inCostosExistencias result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = "select * from inCostosExistencias with(nolock) " +
                                           "where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                if (!string.IsNullOrEmpty(costos.Periodo.Value.ToString()))
                {
                    query += "and Periodo = @Periodo ";
                    mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@Periodo"].Value = costos.Periodo.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(costos.CosteoGrupoInvID.Value))
                {
                    query += "and CosteoGrupoInvID = @CosteoGrupoInvID ";
                    mySqlCommand.Parameters.Add("@CosteoGrupoInvID", SqlDbType.Char, UDT_CosteoGrupoInvID.MaxLength);
                    mySqlCommand.Parameters["@CosteoGrupoInvID"].Value = costos.CosteoGrupoInvID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(costos.inReferenciaID.Value))
                {
                    query += "and inReferenciaID = @inReferenciaID ";
                    mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_ReferenciaID.MaxLength);
                    mySqlCommand.Parameters["@inReferenciaID"].Value = costos.inReferenciaID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(costos.ActivoID.Value.ToString()))
                {
                    query += "and ActivoID = @ActivoID ";
                    mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                    mySqlCommand.Parameters["@ActivoID"].Value = costos.ActivoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(costos.EstadoInv.Value.ToString()))
                {
                    query += "and EstadoInv = @EstadoInv ";
                    mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@EstadoInv"].Value = costos.EstadoInv.Value;
                    filterInd = true;
                }               
                if (!string.IsNullOrEmpty(costos.IdentificadorTr.Value.ToString()))
                {
                    query += "and IdentificadorTr = @IdentificadorTr ";
                    mySqlCommand.Parameters.Add("@IdentificadorTr", SqlDbType.Int);
                    mySqlCommand.Parameters["@IdentificadorTr"].Value = costos.IdentificadorTr.Value;
                    filterInd = true;
                } 
                mySqlCommand.CommandText = query;

                if (!filterInd)
                    return result;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                if (dr.Read())
                {
                    result = new DTO_inCostosExistencias(dr);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inCostosExistencias_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona en tabla inCostosExistencias
        /// </summary>
        /// <param name="costos">movimiento</param>
        /// <returns>Consecutivo</returns>
        public int DAL_inCostosExistencias_Add(DTO_inCostosExistencias costos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = " INSERT INTO inCostosExistencias " +
                                           "(EmpresaID " +
                                           ",Periodo " +
                                           ",CosteoGrupoInvID " +
                                           ",inReferenciaID " +
                                           ",ActivoID " +
                                           ",EstadoInv " +
                                           ",IdentificadorTr " +
                                           ",CantInicial" +
                                           ",CantEntrada" +
                                           ",CantRetiro" +
                                           ",FobLocSaldoIni" +
                                           ",CtoLocSaldoIni" +
                                           ",FobLocEntrada" +
                                           ",CtoLocEntrada" +
                                           ",FobLocSalida" +
                                           ",CtoLocSalida" +
                                           ",FobExtSaldoIni" +
                                           ",CtoExtSaldoIni" +
                                           ",FobExtEntrada" +
                                           ",CtoExtEntrada" +
                                           ",FobExtSalida" +
                                           ",CtoExtSalida" +
                                           ",AxISaldoIni" +
                                           ",AxIEntrada" +
                                           ",AxISalida" +
                                           ",FobLocSaldoIniIFRS" +
                                           ",CtoLocSaldoIniIFRS" +
                                           ",FobLocEntradaIFRS" +
                                           ",CtoLocEntradaIFRS" +
                                           ",FobLocSalidaIFRS" +
                                           ",CtoLocSalidaIFRS" +
                                           ",FobExtSaldoIniIFRS" +
                                           ",CtoExtSaldoIniIFRS" +
                                           ",FobExtEntradaIFRS" +
                                           ",CtoExtEntradaIFRS" +
                                           ",FobExtSalidaIFRS" +
                                           ",CtoExtSalidaIFRS" +
                                           ",eg_inCosteoGrupo" +
                                           ",eg_inReferencia )" +
                                           "VALUES" +
                                             "(@EmpresaID " +
                                           ",@Periodo " +
                                           ",@CosteoGrupoInvID " +
                                           ",@inReferenciaID " +
                                           ",@ActivoID " +
                                           ",@EstadoInv " +
                                           ",@IdentificadorTr " +
                                           ",@CantInicial" +
                                           ",@CantEntrada" +
                                           ",@CantRetiro" +
                                           ",@FobLocSaldoIni" +
                                           ",@CtoLocSaldoIni" +
                                           ",@FobLocEntrada" +
                                           ",@CtoLocEntrada" +
                                           ",@FobLocSalida" +
                                           ",@CtoLocSalida" +
                                           ",@FobExtSaldoIni" +
                                           ",@CtoExtSaldoIni" +
                                           ",@FobExtEntrada" +
                                           ",@CtoExtEntrada" +
                                           ",@FobExtSalida" +
                                           ",@CtoExtSalida" +
                                           ",@AxISaldoIni" +
                                           ",@AxIEntrada" +
                                           ",@AxISalida" +
                                           ",@FobLocSaldoIniIFRS" +
                                           ",@CtoLocSaldoIniIFRS" +
                                           ",@FobLocEntradaIFRS" +
                                           ",@CtoLocEntradaIFRS" +
                                           ",@FobLocSalidaIFRS" +
                                           ",@CtoLocSalidaIFRS" +
                                           ",@FobExtSaldoIniIFRS" +
                                           ",@CtoExtSaldoIniIFRS" +
                                           ",@FobExtEntradaIFRS" +
                                           ",@CtoExtEntradaIFRS" +
                                           ",@FobExtSalidaIFRS" +
                                           ",@CtoExtSalidaIFRS" +
                                           ",@eg_inCosteoGrupo" +
                                           ",@eg_inReferencia )" +
                            " SET @Consecutivo = SCOPE_IDENTITY()";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@CosteoGrupoInvID", SqlDbType.Char, UDT_CosteoGrupoInvID.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EstadoInv", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@IdentificadorTr", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CantInicial", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantEntrada", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantRetiro", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobLocSaldoIni", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoLocSaldoIni", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobLocEntrada", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoLocEntrada", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobLocSalida", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoLocSalida", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobExtSaldoIni", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoExtSaldoIni", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobExtEntrada", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoExtEntrada", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobExtSalida", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoExtSalida", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@AxISaldoIni", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@AxIEntrada", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@AxISalida", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobLocSaldoIniIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoLocSaldoIniIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobLocEntradaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoLocEntradaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobLocSalidaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoLocSalidaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobExtSaldoIniIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoExtSaldoIniIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobExtEntradaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoExtEntradaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobExtSalidaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoExtSalidaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_inCosteoGrupo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int, 1);
                mySqlCommand.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Periodo"].Value = costos.Periodo.Value;
                mySqlCommand.Parameters["@CosteoGrupoInvID"].Value = costos.CosteoGrupoInvID.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = costos.inReferenciaID.Value;
                mySqlCommand.Parameters["@ActivoID"].Value = costos.ActivoID.Value;
                mySqlCommand.Parameters["@EstadoInv"].Value = costos.EstadoInv.Value;
                mySqlCommand.Parameters["@IdentificadorTr"].Value = costos.IdentificadorTr.Value;
                mySqlCommand.Parameters["@CantInicial"].Value = costos.CantInicial.Value;
                mySqlCommand.Parameters["@CantEntrada"].Value = costos.CantEntrada.Value;
                mySqlCommand.Parameters["@CantRetiro"].Value = costos.CantRetiro.Value;
                mySqlCommand.Parameters["@FobLocSaldoIni"].Value = costos.FobLocSaldoIni.Value;
                mySqlCommand.Parameters["@CtoLocSaldoIni"].Value = costos.CtoLocSaldoIni.Value;
                mySqlCommand.Parameters["@FobLocEntrada"].Value = costos.FobLocEntrada.Value;
                mySqlCommand.Parameters["@CtoLocEntrada"].Value = costos.CtoLocEntrada.Value;
                mySqlCommand.Parameters["@FobLocSalida"].Value = costos.FobLocSalida.Value;
                mySqlCommand.Parameters["@CtoLocSalida"].Value = costos.CtoLocSalida.Value;
                mySqlCommand.Parameters["@FobExtSaldoIni"].Value = costos.FobExtSaldoIni.Value;
                mySqlCommand.Parameters["@CtoExtSaldoIni"].Value = costos.CtoExtSaldoIni.Value;
                mySqlCommand.Parameters["@FobExtEntrada"].Value = costos.FobExtEntrada.Value;
                mySqlCommand.Parameters["@CtoExtEntrada"].Value = costos.CtoExtEntrada.Value;
                mySqlCommand.Parameters["@FobExtSalida"].Value = costos.FobExtSalida.Value;
                mySqlCommand.Parameters["@CtoExtSalida"].Value = costos.CtoExtSalida.Value;
                mySqlCommand.Parameters["@AxISaldoIni"].Value = costos.AxISaldoIni.Value;
                mySqlCommand.Parameters["@AxIEntrada"].Value = costos.AxIEntrada.Value;
                mySqlCommand.Parameters["@AxISalida"].Value = costos.AxISalida.Value;
                mySqlCommand.Parameters["@FobLocSaldoIniIFRS"].Value = costos.FobLocSaldoIniIFRS.Value;
                mySqlCommand.Parameters["@CtoLocSaldoIniIFRS"].Value = costos.CtoLocSaldoIniIFRS.Value;
                mySqlCommand.Parameters["@FobLocEntradaIFRS"].Value = costos.FobLocEntradaIFRS.Value;
                mySqlCommand.Parameters["@CtoLocEntradaIFRS"].Value = costos.CtoLocEntradaIFRS.Value;
                mySqlCommand.Parameters["@FobLocSalidaIFRS"].Value = costos.FobLocSalidaIFRS.Value;
                mySqlCommand.Parameters["@CtoLocSalidaIFRS"].Value = costos.CtoLocSalidaIFRS.Value;
                mySqlCommand.Parameters["@FobExtSaldoIniIFRS"].Value = costos.FobExtSaldoIniIFRS.Value;
                mySqlCommand.Parameters["@CtoExtSaldoIniIFRS"].Value = costos.CtoExtSaldoIniIFRS.Value;
                mySqlCommand.Parameters["@FobExtEntradaIFRS"].Value = costos.FobExtEntradaIFRS.Value;
                mySqlCommand.Parameters["@CtoExtEntradaIFRS"].Value = costos.CtoExtEntradaIFRS.Value;
                mySqlCommand.Parameters["@FobExtSalidaIFRS"].Value = costos.FobExtSalidaIFRS.Value;
                mySqlCommand.Parameters["@CtoExtSalidaIFRS"].Value = costos.CtoExtSalidaIFRS.Value;
                mySqlCommand.Parameters["@eg_inCosteoGrupo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inCosteoGrupo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
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
                int numDoc = Convert.ToInt32(mySqlCommand.Parameters["@Consecutivo"].Value);
                return numDoc;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inCostosExistencias_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Actualizar la tabla de inCostosExitencias
        /// </summary>
        /// <param name="costos">costos</param>
        public void DAL_inCostosExistencias_Upd(DTO_inCostosExistencias costos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla inCostosExistencias
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE inCostosExistencias " +
                                           "    SET Periodo  = @Periodo  " +
                                           "    ,CantInicial  = @CantInicial " +
                                           "    ,CantEntrada  = @CantEntrada " +
                                           "    ,CantRetiro  = @CantRetiro " +
                                           "    ,FobLocSaldoIni  = @FobLocSaldoIni " +
                                           "    ,CtoLocSaldoIni  = @CtoLocSaldoIni " +
                                           "    ,FobLocEntrada  = @FobLocEntrada " +
                                           "    ,CtoLocEntrada  = @CtoLocEntrada " +
                                           "    ,FobLocSalida  = @FobLocSalida " +
                                           "    ,CtoLocSalida  = @CtoLocSalida " +
                                           "    ,FobExtSaldoIni  = @FobExtSaldoIni " +
                                           "    ,CtoExtSaldoIni  = @CtoExtSaldoIni " +
                                           "    ,FobExtEntrada  = @FobExtEntrada " +
                                           "    ,CtoExtEntrada  = @CtoExtEntrada " +
                                           "    ,FobExtSalida  = @FobExtSalida " +
                                           "    ,CtoExtSalida  = @CtoExtSalida " +
                                           "    ,AxISaldoIni  = @AxISaldoIni " +
                                           "    ,AxIEntrada  = @AxIEntrada " +
                                           "    ,AxISalida  = @AxISalida " +
                                           "    ,FobLocSaldoIniIFRS  = @FobLocSaldoIniIFRS " +
                                           "    ,CtoLocSaldoIniIFRS  = @CtoLocSaldoIniIFRS " +
                                           "    ,FobLocEntradaIFRS  = @FobLocEntradaIFRS " +
                                           "    ,CtoLocEntradaIFRS  = @CtoLocEntradaIFRS " +
                                           "    ,FobLocSalidaIFRS  = @FobLocSalidaIFRS " +
                                           "    ,CtoLocSalidaIFRS  = @CtoLocSalidaIFRS " +
                                           "    ,FobExtSaldoIniIFRS  = @FobExtSaldoIniIFRS " +
                                           "    ,CtoExtSaldoIniIFRS  = @CtoExtSaldoIniIFRS " +
                                           "    ,FobExtEntradaIFRS  = @FobExtEntradaIFRS " +
                                           "    ,CtoExtEntradaIFRS  = @CtoExtEntradaIFRS " +
                                           "    ,FobExtSalidaIFRS  = @FobExtSalidaIFRS " +
                                           "    ,CtoExtSalidaIFRS  = @CtoExtSalidaIFRS " +
                                           "    WHERE Consecutivo = @Consecutivo";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@CantInicial", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantEntrada", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantRetiro", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobLocSaldoIni", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoLocSaldoIni", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobLocEntrada", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoLocEntrada", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobLocSalida", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoLocSalida", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobExtSaldoIni", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoExtSaldoIni", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobExtEntrada", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoExtEntrada", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobExtSalida", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoExtSalida", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@AxISaldoIni", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@AxIEntrada", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@AxISalida", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobLocSaldoIniIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoLocSaldoIniIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobLocEntradaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoLocEntradaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobLocSalidaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoLocSalidaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobExtSaldoIniIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoExtSaldoIniIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobExtEntradaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoExtEntradaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@FobExtSalidaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CtoExtSalidaIFRS", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@Periodo"].Value = costos.Periodo.Value;
                mySqlCommand.Parameters["@CantInicial"].Value = costos.CantInicial.Value;
                mySqlCommand.Parameters["@CantEntrada"].Value = costos.CantEntrada.Value;
                mySqlCommand.Parameters["@CantRetiro"].Value = costos.CantRetiro.Value;
                mySqlCommand.Parameters["@FobLocSaldoIni"].Value = costos.FobLocSaldoIni.Value;
                mySqlCommand.Parameters["@CtoLocSaldoIni"].Value = costos.CtoLocSaldoIni.Value;
                mySqlCommand.Parameters["@FobLocEntrada"].Value = costos.FobLocEntrada.Value;
                mySqlCommand.Parameters["@CtoLocEntrada"].Value = costos.CtoLocEntrada.Value;
                mySqlCommand.Parameters["@FobLocSalida"].Value = costos.FobLocSalida.Value;
                mySqlCommand.Parameters["@CtoLocSalida"].Value = costos.CtoLocSalida.Value;
                mySqlCommand.Parameters["@FobExtSaldoIni"].Value = costos.FobExtSaldoIni.Value;
                mySqlCommand.Parameters["@CtoExtSaldoIni"].Value = costos.CtoExtSaldoIni.Value;
                mySqlCommand.Parameters["@FobExtEntrada"].Value = costos.FobExtEntrada.Value;
                mySqlCommand.Parameters["@CtoExtEntrada"].Value = costos.CtoExtEntrada.Value;
                mySqlCommand.Parameters["@FobExtSalida"].Value = costos.FobExtSalida.Value;
                mySqlCommand.Parameters["@CtoExtSalida"].Value = costos.CtoExtSalida.Value;
                mySqlCommand.Parameters["@AxISaldoIni"].Value = costos.AxISaldoIni.Value;
                mySqlCommand.Parameters["@AxIEntrada"].Value = costos.AxIEntrada.Value;
                mySqlCommand.Parameters["@AxISalida"].Value = costos.AxISalida.Value;
                mySqlCommand.Parameters["@FobLocSaldoIniIFRS"].Value = costos.FobLocSaldoIniIFRS.Value;
                mySqlCommand.Parameters["@CtoLocSaldoIniIFRS"].Value = costos.CtoLocSaldoIniIFRS.Value;
                mySqlCommand.Parameters["@FobLocEntradaIFRS"].Value = costos.FobLocEntradaIFRS.Value;
                mySqlCommand.Parameters["@CtoLocEntradaIFRS"].Value = costos.CtoLocEntradaIFRS.Value;
                mySqlCommand.Parameters["@FobLocSalidaIFRS"].Value = costos.FobLocSalidaIFRS.Value;
                mySqlCommand.Parameters["@CtoLocSalidaIFRS"].Value = costos.CtoLocSalidaIFRS.Value;
                mySqlCommand.Parameters["@FobExtSaldoIniIFRS"].Value = costos.FobExtSaldoIniIFRS.Value;
                mySqlCommand.Parameters["@CtoExtSaldoIniIFRS"].Value = costos.CtoExtSaldoIniIFRS.Value;
                mySqlCommand.Parameters["@FobExtEntradaIFRS"].Value = costos.FobExtEntradaIFRS.Value;
                mySqlCommand.Parameters["@CtoExtEntradaIFRS"].Value = costos.CtoExtEntradaIFRS.Value;
                mySqlCommand.Parameters["@FobExtSalidaIFRS"].Value = costos.FobExtSalidaIFRS.Value;
                mySqlCommand.Parameters["@CtoExtSalidaIFRS"].Value = costos.CtoExtSalidaIFRS.Value;
                mySqlCommand.Parameters["@Consecutivo"].Value = costos.Consecutivo.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inCostosExistencias_Upd");
                throw exception;
            }
        }
        #endregion

        #region Cierres

        /// <summary>
        /// Verifica si un periodo ya esta cerrado
        /// </summary>
        /// <param name="mod">Modulo de consulta</param>
        /// <param name="periodo">Periodo de consulta</param>
        public bool DAL_MvtoSaldosCostos_IsPeriodoCerrado(ModulesPrefix mod, DateTime periodo, ref bool exists)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();
                mySqlCommand.Parameters["@Periodo"].Value = periodo;

                mySqlCommand.CommandText = "select * from coCierresControl with(nolock) " +
                                            "where EmpresaID=@EmpresaID and ModuloID=@ModuloID and Periodo=@Periodo";

                bool result;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    exists = true;
                    int abierto = Convert.ToInt32(dr["AbiertoInd"]);
                    if (abierto == 0)
                        result = true;
                    else
                        result = false;
                }
                else
                {
                    exists = false;
                    result = false;
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ComprobantePre_IsPeriodoCerrado");
                throw exception;
            }
        }

        /// <summary>
        /// Carga la lista de saldos para el nuevo periodo
        /// </summary>
        /// <param name="periodo">Periodo de cierre</param>
        /// <param name="modulo">Modulo de cierre</param>
        /// <param name="siguientePeriodo">Nuevo periodo</param>
        public void DAL_MvtoSaldosCostos_GetSaldosCierre(DateTime periodo, string moduloId, DateTime siguientePeriodo, List<DTO_coCuentaSaldo> saldosInsert, List<DTO_coCuentaSaldo> saldosUpdate)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                #region Definicion de parametros
                mySqlCommand.Parameters.Add("EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("NuevoPeriodo", SqlDbType.DateTime);

                mySqlCommand.Parameters["EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["ModuloID"].Value = moduloId;
                mySqlCommand.Parameters["NuevoPeriodo"].Value = siguientePeriodo;
                #endregion
                #region Query
                //La primero columna (saldoNew.PeriodoID - NuevoPeriodo) indica si ese saldo existe en el siguiente periodo
                mySqlCommand.CommandText =
                    "SELECT saldoNew.PeriodoID NuevoPeriodo, saldoOld.* " +
                    "FROM coCuentaSaldo saldoOld " +
                    "JOIN glConceptoSaldo ON saldoOld.ConceptoSaldoID = glConceptoSaldo.ConceptoSaldoID " +
                    "                       AND saldoOld.eg_glConceptoSaldo=glConceptoSaldo.EmpresaGrupoID " +
                    "LEFT JOIN coCuentaSaldo saldoNew ON saldoOld.EmpresaID=saldoNew.EmpresaID " +
                    "                       AND saldoOld.BalanceTipoID=saldoNew.BalanceTipoID " +
                    "                       AND saldoOld.CuentaID=saldoNew.CuentaID " +
                    "                       AND saldoOld.TerceroID=saldoNew.TerceroID " +
                    "                       AND saldoOld.ProyectoID=saldoNew.ProyectoID " +
                    "                       AND saldoOld.CentroCostoID=saldoNew.CentroCostoID " +
                    "                       AND saldoOld.LineaPresupuestoID=saldoNew.LineaPresupuestoID " +
                    "                       AND saldoOld.ConceptoSaldoID=saldoNew.ConceptoSaldoID " +
                    "                       AND saldoOld.IdentificadorTR=saldoNew.IdentificadorTR " +
                    "                       AND saldoOld.ConceptoCargoID=saldoNew.ConceptoCargoID " +
                    "                       AND saldoNew.PeriodoID=@NuevoPeriodo " +
                    "WHERE saldoOld.EmpresaID=@EmpresaID AND saldoOld.PeriodoID=@PeriodoID AND UPPER(glConceptoSaldo.ModuloID)=UPPER(@ModuloID) ";
                #endregion
                #region Carga los datos
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_coCuentaSaldo saldo = new DTO_coCuentaSaldo(dr);
                    if (dr.IsDBNull(0))
                        saldosInsert.Add(saldo);
                    else
                        saldosUpdate.Add(saldo);
                }
                dr.Close();
                #endregion
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_MvtoSaldosCostos_GetSaldosCierre");
                throw ex;
            }
        }

        /// <summary>
        /// Trae informacion  de los mvtos de inventarios para contabilidad
        /// </summary>
        /// <returns>retorna una lista de comprob </returns>
        public Object DAL_MvtoSaldosCostos_GetComprobanteMvto(int numeroDoc)
        {
            try
            {
                DTO_Comprobante comp = new DTO_Comprobante();
                List<DTO_ComprobanteFooter> comprobantes = new List<DTO_ComprobanteFooter>();
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                SqlCommand mySqlCommand = new SqlCommand("Inventarios_ComprobanteMvto", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandType = CommandType.StoredProcedure;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.line = 0;
                int line = 0;
                bool addLine = false;
                while (dr.Read())
                {
                    //Valida si hay errores de salida
                    if (dr.GetName(0) == "Linea")
                    {
                        #region Asigna el error
                        result.Result = ResultValue.NOK;
                        DTO_TxResultDetailFields rdf = new DTO_TxResultDetailFields();
                        line = Convert.ToInt32(dr["Linea"]);

                        if (line != rd.line)
                        {
                            if (addLine)
                                result.Details.Add(rd);

                            addLine = true;
                            rd = new DTO_TxResultDetail();
                            rd.line = line;
                            rd.Message = "NOK";
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                        }
                        rd.Message = dr["Valor"].ToString(); 
                        #endregion
                    }
                    else //Sino crea el comprobante
                    {
                        #region Footer
                        DTO_ComprobanteFooter dto = new DTO_ComprobanteFooter();
                        dto.CuentaID.Value = dr["CuentaID"].ToString();
                        dto.TerceroID.Value = dr["TerceroID"].ToString();
                        dto.ProyectoID.Value = dr["ProyectoID"].ToString();
                        dto.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                        dto.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                        dto.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                        dto.LugarGeograficoID.Value = dr["LugarGeograficoID"].ToString();
                        dto.PrefijoCOM.Value = dr["PrefijoCOM"].ToString();
                        dto.DocumentoCOM.Value = dr["DocumentoCOM"].ToString();
                        dto.ActivoCOM.Value = dr["ActivoCOM"].ToString();
                        dto.ConceptoSaldoID.Value = dr["ConceptoSaldoID"].ToString();
                        dto.IdentificadorTR.Value = Convert.ToInt64(dr["IdentificadorTR"]);
                        dto.Descriptivo.Value = dr["Descriptivo"].ToString();
                        dto.TasaCambio.Value = Convert.ToDecimal(dr["TasaCambioBase"]);
                        dto.vlrBaseML.Value = Convert.ToDecimal(dr["vlrBaseML"]);
                        dto.vlrBaseME.Value = Convert.ToDecimal(dr["vlrBaseME"]);
                        dto.vlrMdaLoc.Value = Convert.ToDecimal(dr["vlrMdaLoc"]);
                        dto.vlrMdaExt.Value = Convert.ToDecimal(dr["vlrMdaExt"]);
                        dto.CuentaAlternaID.Value = dr["CuentaAlternaID"].ToString();
                        if (!string.IsNullOrWhiteSpace(dr["vlrMdaOtr"].ToString()))
                            dto.vlrMdaOtr.Value = Convert.ToDecimal(dr["vlrMdaOtr"]);
                        dto.DatoAdd1.Value = dr["DatoAdd1"].ToString();
                        dto.DatoAdd2.Value = dr["DatoAdd2"].ToString();
                        dto.DatoAdd3.Value = dr["DatoAdd3"].ToString();
                        dto.DatoAdd4.Value = dr["DatoAdd4"].ToString();
                        dto.DatoAdd5.Value = dr["DatoAdd5"].ToString();
                        dto.DatoAdd6.Value = dr["DatoAdd6"].ToString();
                        dto.DatoAdd7.Value = dr["DatoAdd7"].ToString();
                        dto.DatoAdd8.Value = dr["DatoAdd8"].ToString();
                        dto.DatoAdd9.Value = dr["DatoAdd9"].ToString();
                        dto.DatoAdd10.Value = dr["DatoAdd10"].ToString(); 
                        #endregion
                        #region Header
                        comp.Header.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                        comp.Header.ComprobanteNro.Value = 0;
                        comp.Header.EmpresaID.Value = dr["EmpresaID"].ToString();
                        comp.Header.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                        comp.Header.NumeroDoc.Value = numeroDoc;
                        comp.Header.MdaOrigen.Value = Convert.ToByte(dr["MdaOrigen"]);
                        comp.Header.MdaTransacc.Value = dr["MdaTransacc"].ToString();
                        comp.Header.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                        comp.Header.TasaCambioBase.Value = dto.TasaCambio.Value;
                        comp.Header.TasaCambioOtr.Value = Convert.ToDecimal(dr["TasaCambioOtr"]);
                        comprobantes.Add(dto); 
                        #endregion
                    }
                }
                comp.Footer = comprobantes.FindAll(x => x.vlrMdaLoc.Value != 0).ToList();
                dr.Close();

                if (result.Result == ResultValue.NOK)
                {
                    result.Details.Add(rd);
                    result.ResultMessage = DictionaryMessages.Err_UpdateData;
                    return result;
                }

                return comp;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_MvtoSaldosCostos_GetComprobanteMvto");
                throw exception;
            }
        }
        
        #endregion

        #endregion
    }
}


