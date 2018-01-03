using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using System.Configuration;
using System.Reflection;
using SentenceTransformer;

namespace NewAge.ADO
{
    public class DAL_coAuxiliarAjustaDeta : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_coAuxiliarAjustaDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Revisa si hay un ajuste previo
        /// </summary>
        /// <param name="periodoId">Perdiodo de consulta</param>
        /// <param name="compID">Identificador del comprobante</param>
        /// <param name="compNro">Numero del comprobante</param>
        public int DAL_coAuxiliarAjustaDeta_Count(DateTime periodoId, string compID, int compNro)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ComprobanteNro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ProcesadoInd", SqlDbType.Bit);

                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodoId;
                mySqlCommandSel.Parameters["@ComprobanteID"].Value = compID;
                mySqlCommandSel.Parameters["@ComprobanteNro"].Value = compNro;
                mySqlCommandSel.Parameters["@ProcesadoInd"].Value = false;

                mySqlCommandSel.CommandText =
                    "Select count(*) from coAuxiliarAjustaDeta with(nolock) " +
                    "where PeriodoID = @PeriodoID and ComprobanteID = @ComprobanteID and ComprobanteNro = @ComprobanteNro and ProcesadoInd = @ProcesadoInd";
                int count = Convert.ToInt32(mySqlCommandSel.ExecuteScalar());
                return count;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coAuxiliarAjustaDeta_TablaOrigen");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega la informacion de un comprobante que se este ajustando
        /// </summary>
        /// <param name="comp">Comprobante de ajuste</param>
        public void DAL_coAuxiliarAjustaDeta_Add(DTO_Comprobante comp)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;


                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Tipo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@seUsuarioID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ProcesadoInd", SqlDbType.Bit);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = comp.Header.NumeroDoc.Value;                
                mySqlCommandSel.Parameters["@Tipo"].Value = 1;
                mySqlCommandSel.Parameters["@seUsuarioID"].Value = this.UserId;
                mySqlCommandSel.Parameters["@ProcesadoInd"].Value = 0;

                #region Comprobante original

                mySqlCommandSel.CommandText =
                    "INSERT INTO coAuxiliarAjustaDeta( " +
                    "		NumeroDoc,Tipo,PeriodoID,ComprobanteID,ComprobanteNro,Fecha,CuentaID,TerceroID, " +
                    "		ProyectoID,CentroCostoID,LineaPresupuestoID,ConceptoCargoID,LugarGeograficoID,Descriptivo, " +
                    "		PrefijoCOM,DocumentoCOM,ActivoCOM,IdentificadorTR,MdaOrigen,TasaCambioBase,vlrBaseML, " +
                    "		vlrBaseME,vlrMdaLoc,vlrMdaExt,vlrMdaOtr,seUsuarioID,ProcesadoInd " +
                    ") " +
                    "	SELECT " +
                    "		NumeroDoc,@Tipo,PeriodoID,ComprobanteID,ComprobanteNro,Fecha,CuentaID,TerceroID, " +
                    "		ProyectoID,CentroCostoID,LineaPresupuestoID,ConceptoCargoID,LugarGeograficoID,Descriptivo, " +
                    "		PrefijoCOM,DocumentoCOM,ActivoCOM,IdentificadorTR,MdaOrigen,TasaCambioBase,vlrBaseML, " +
                    "		vlrBaseME,vlrMdaLoc,vlrMdaExt,vlrMdaOtr,@seUsuarioID,@ProcesadoInd " +
                    "	from coAuxiliar with(nolock) " +
                    "	where NumeroDoc = @NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();

                #endregion
                #region Nuevo comprobante

                #region CommandText
                mySqlCommandSel.CommandText =
                    "INSERT INTO coAuxiliarAjustaDeta( " +
                    "	NumeroDoc,Tipo,PeriodoID,ComprobanteID,ComprobanteNro,Fecha,CuentaID,TerceroID, " +
                    "   ProyectoID,CentroCostoID,LineaPresupuestoID,ConceptoCargoID,LugarGeograficoID,Descriptivo, " +
                    "   PrefijoCOM,DocumentoCOM,ActivoCOM,IdentificadorTR,MdaOrigen,TasaCambioBase,vlrBaseML, " +
                    "   vlrBaseME,vlrMdaLoc,vlrMdaExt,vlrMdaOtr,seUsuarioID " +
                    ")VALUES( " +
                    "   @NumeroDoc,@Tipo,@PeriodoID,@ComprobanteID,@ComprobanteNro,@Fecha,@CuentaID,@TerceroID, " +
                    "   @ProyectoID,@CentroCostoID,@LineaPresupuestoID,@ConceptoCargoID,@LugarGeograficoID,@Descriptivo, " +
                    "   @PrefijoCOM,@DocumentoCOM,@ActivoCOM,@IdentificadorTR,@MdaOrigen,@TasaCambioBase,@vlrBaseML, " +
                    "   @vlrBaseME,@vlrMdaLoc,@vlrMdaExt,@vlrMdaOtr,@seUsuarioID" + 
                    ")";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ComprobanteNro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoCargoID", SqlDbType.Char, UDT_ConceptoCargoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LugarGeograficoID", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Descriptivo", SqlDbType.Char, UDT_Descriptivo.MaxLength);
                mySqlCommandSel.Parameters.Add("@PrefijoCOM", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoCOM", SqlDbType.VarChar, 20);
                mySqlCommandSel.Parameters.Add("@ActivoCOM", SqlDbType.Char, UDT_PlaquetaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);
                mySqlCommandSel.Parameters.Add("@MdaOrigen", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TasaCambioBase", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@vlrBaseML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@vlrBaseME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@vlrMdaLoc", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@vlrMdaExt", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@vlrMdaOtr", SqlDbType.Decimal);
                #endregion

                mySqlCommandSel.Parameters["@PeriodoID"].Value = comp.Header.PeriodoID.Value;
                mySqlCommandSel.Parameters["@ComprobanteID"].Value = comp.Header.ComprobanteID.Value;
                mySqlCommandSel.Parameters["@ComprobanteNro"].Value = comp.Header.ComprobanteNro.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = DateTime.Now;
                mySqlCommandSel.Parameters["@Tipo"].Value = 2;
                mySqlCommandSel.Parameters["@MdaOrigen"].Value = comp.Header.MdaOrigen.Value;
                mySqlCommandSel.Parameters["@TasaCambioBase"].Value = comp.Header.TasaCambioBase.Value;

                foreach(DTO_ComprobanteFooter det in comp.Footer)
                {
                    #region Asigna los valores por detalle
                    mySqlCommandSel.Parameters["@CuentaID"].Value = det.CuentaID.Value;
                    mySqlCommandSel.Parameters["@TerceroID"].Value = det.TerceroID.Value;
                    mySqlCommandSel.Parameters["@ProyectoID"].Value = det.ProyectoID.Value;
                    mySqlCommandSel.Parameters["@CentroCostoID"].Value = det.CentroCostoID.Value;
                    mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = det.LineaPresupuestoID.Value;
                    mySqlCommandSel.Parameters["@ConceptoCargoID"].Value = det.ConceptoCargoID.Value;
                    mySqlCommandSel.Parameters["@LugarGeograficoID"].Value = det.LugarGeograficoID.Value;
                    mySqlCommandSel.Parameters["@Descriptivo"].Value = det.Descriptivo.Value;
                    mySqlCommandSel.Parameters["@PrefijoCOM"].Value = det.PrefijoCOM.Value;
                    mySqlCommandSel.Parameters["@DocumentoCOM"].Value = det.DocumentoCOM.Value;
                    mySqlCommandSel.Parameters["@ActivoCOM"].Value = det.ActivoCOM.Value;
                    mySqlCommandSel.Parameters["@IdentificadorTR"].Value = det.IdentificadorTR.Value;
                    mySqlCommandSel.Parameters["@vlrBaseML"].Value = det.vlrBaseML.Value;
                    mySqlCommandSel.Parameters["@vlrBaseME"].Value = det.vlrBaseME.Value;
                    mySqlCommandSel.Parameters["@vlrMdaLoc"].Value = det.vlrMdaLoc.Value;
                    mySqlCommandSel.Parameters["@vlrMdaExt"].Value = det.vlrMdaExt.Value;
                    mySqlCommandSel.Parameters["@vlrMdaOtr"].Value = det.vlrMdaOtr.Value;
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
                #endregion
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coAuxiliarAjustaDeta_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina la información de un ajuste de comprobante
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_coAuxiliarAjustaDeta_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ProcesadoInd", SqlDbType.Bit);
                
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@ProcesadoInd"].Value = false;

                mySqlCommandSel.CommandText = "Delete from coAuxiliarAjustaDeta where NumeroDoc = @NumeroDoc and ProcesadoInd = @ProcesadoInd";
                mySqlCommandSel.ExecuteNonQuery();

                mySqlCommandSel.CommandText = "Delete from coAuxiliarPre where NumeroDoc = @NumeroDoc";
                mySqlCommandSel.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coAuxiliarAjustaDeta_Delete");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Trae un listado de comprobantes pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_ComprobanteAprobacion> DAL_coAuxiliarAjustaDeta_GetPendientes(string actividadFlujoID, string usuarioID)
        {
            try
            {
                List<DTO_ComprobanteAprobacion> result = new List<DTO_ComprobanteAprobacion>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select distinct aj.NumeroDoc, PeriodoID, aj.ComprobanteID, ComprobanteNro " +
                    "from coAuxiliarAjustaDeta aj with(nolock) " +
                    "	inner join glDocumentoControl ctrl with(nolock) on aj.NumeroDoc = ctrl.NumeroDoc " +
                    "	inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "		and act.CerradoInd=@CerradoInd and act.ActividadFlujoID=@ActividadFlujoID " +
                    "	inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +  
                    "	inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID " +
                    "	inner join glActividadPermiso perm with(nolock) on Perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    "       and perm.UsuarioID = @UsuarioID and Perm.AreaFuncionalID = Ctrl.AreaFuncionalID " +
                    "where ProcesadoInd = @ProcesadoInd and Tipo = @Tipo ";

                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@ProcesadoInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@Tipo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);

                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;
                mySqlCommand.Parameters["@ProcesadoInd"].Value = false;
                mySqlCommand.Parameters["@Tipo"].Value = 2;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuarioID;

                SqlDataReader dr;

                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ComprobanteAprobacion dto = new DTO_ComprobanteAprobacion();
                    dto.Aprobado.Value = false;
                    dto.Rechazado.Value = false;
                    dto.Observacion.Value = string.Empty;

                    dto.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                    dto.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                    dto.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                    dto.ComprobanteNro.Value = Convert.ToInt32(dr["ComprobanteNro"]);
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coAuxiliarAjustaDeta_GetPendientes");
                throw exception;
            }
        }

        /// <summary>
        /// Procesa el ajuste de un ajuste de comprobante
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_coAuxiliarAjustaDeta_Procesar(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ProcesadoInd", SqlDbType.Bit);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@ProcesadoInd"].Value = true;

                mySqlCommandSel.CommandText = "Update coAuxiliarAjustaDeta set ProcesadoInd = @ProcesadoInd where NumeroDoc = @NumeroDoc ";
                mySqlCommandSel.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coAuxiliarAjustaDeta_Procesar");
                throw exception;
            }
        }
        #endregion
    }
}
