using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.DTO.Recursos;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Comunicacion;
using NewAge.DTO.GlobalConfig;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL Legalizacion Cabezote
    /// </summary>
    public class DAL_cpLegalizaHeader : DAL_Base
    {
       /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_cpLegalizaHeader(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId) : base(c, tx, empresa, userId) { }
        
        #region Funciones Públicas

        /// <summary>
        /// Consulta una legalizacion segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_cpLegalizaHeader DAL_LegalizaHeader_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from cpLegalizaDocu with(nolock) where cpLegalizaDocu.NumeroDoc = @NumeroDoc ";
                
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                DTO_cpLegalizaHeader result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_cpLegalizaHeader(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla cpLegalizaDocu 
        /// </summary>
        /// <param name="leg">Legalizacion</param>
        /// <returns></returns>
        public void DAL_LegalizaHeader_Add(DTO_cpLegalizaHeader leg)
        {
            try
            {
            string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx);
            //    string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
   
                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO cpLegalizaDocu " +
                                           "    (EmpresaID    " +
                                           "    ,NumeroDoc    " +
                                           "    ,CajaMenorID  " +
                                           "    ,FechaIni     " +
                                           "    ,FechaFin     " +
                                           "    ,FechaCont    " +
                                           "    ,ValorFondo   " +
                                           "    ,IdentificadorAnt1   " +
                                           "    ,ValorAnticipo1      " +
                                           "    ,IdentificadorAnt2   " +
                                           "    ,ValorAnticipo2      " +
                                           "    ,IdentificadorAnt3   " +
                                           "    ,ValorAnticipo3      " +
                                           "    ,IdentificadorAnt4   " +
                                           "    ,ValorAnticipo4      " +
                                           "    ,Valor    " +
                                           "    ,IVA      " +
                                           "    ,Estado   " +
                                           "    ,eg_cpCajaMenor)     " +
                                           "    VALUES" +
                                           "    (@EmpresaID     " +
                                           "    ,@NumeroDoc     " +
                                           "    ,@CajaMenorID   " +
                                           "    ,@FechaIni      " +
                                           "    ,@FechaFin      " +
                                           "    ,@FechaCont     " +
                                           "    ,@ValorFondo    " +
                                           "    ,@IdentificadorAnt1     " +
                                           "    ,@ValorAnticipo1        " +
                                           "    ,@IdentificadorAnt2     " +
                                           "    ,@ValorAnticipo2        " +
                                           "    ,@IdentificadorAnt3     " +
                                           "    ,@ValorAnticipo3        " +
                                           "    ,@IdentificadorAnt4     " +
                                           "    ,@ValorAnticipo4        " +
                                           "    ,@Valor         " +
                                           "    ,@IVA           " +
                                           "    ,@Estado        " +
                                           "    ,@eg_cpCajaMenor) ";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CajaMenorID", SqlDbType.Char, UDT_CajaMenorID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaIni", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaCont", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ValorFondo", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt1", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt2", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt3", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo3", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt4", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo4", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@eg_cpCajaMenor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = leg.NumeroDoc.Value;
                mySqlCommand.Parameters["@CajaMenorID"].Value = string.IsNullOrEmpty(leg.CajaMenorID.Value) ? DBNull.Value.ToString(): leg.CajaMenorID.Value;
                mySqlCommand.Parameters["@FechaIni"].Value = leg.FechaIni.Value;
                mySqlCommand.Parameters["@FechaFin"].Value = leg.FechaFin.Value;
                mySqlCommand.Parameters["@FechaCont"].Value = leg.FechaCont.Value;
                mySqlCommand.Parameters["@ValorFondo"].Value = !leg.ValorFondo.Value.HasValue ? null : leg.ValorFondo.Value;
                mySqlCommand.Parameters["@IdentificadorAnt1"].Value = !leg.IdentificadorAnt1.Value.HasValue ? null : leg.IdentificadorAnt1.Value;
                mySqlCommand.Parameters["@ValorAnticipo1"].Value = leg.ValorAnticipo1.Value;
                mySqlCommand.Parameters["@IdentificadorAnt2"].Value = !leg.IdentificadorAnt2.Value.HasValue ? null : leg.IdentificadorAnt2.Value;
                mySqlCommand.Parameters["@ValorAnticipo2"].Value = leg.ValorAnticipo2.Value;
                mySqlCommand.Parameters["@IdentificadorAnt3"].Value = !leg.IdentificadorAnt3.Value.HasValue ? null : leg.IdentificadorAnt3.Value;
                mySqlCommand.Parameters["@ValorAnticipo3"].Value = leg.ValorAnticipo3.Value;
                mySqlCommand.Parameters["@IdentificadorAnt4"].Value = !leg.IdentificadorAnt4.Value.HasValue ? null : leg.IdentificadorAnt4.Value;
                mySqlCommand.Parameters["@ValorAnticipo4"].Value = leg.ValorAnticipo4.Value;
                mySqlCommand.Parameters["@Valor"].Value = leg.Valor.Value;
                mySqlCommand.Parameters["@IVA"].Value = !leg.IVA.Value.HasValue ? null : leg.IVA.Value;
                mySqlCommand.Parameters["@Estado"].Value = leg.Estado.Value;
                mySqlCommand.Parameters["@eg_cpCajaMenor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cpCajaMenor, this.Empresa, egCtrl);

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
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_LegalizaHeader_Add", false);
                throw exception;
            }          
        
        }
          

        /// <summary>
        /// Actualizar la legalizacion en tabla cpLegalizaDocu y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">legalizacion</param>
        public void DAL_LegalizaHeader_Upd(DTO_cpLegalizaHeader leg)
        {
            
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla cpLegalizaDocu
                #region CommandText 
                mySqlCommand.CommandText = "    UPDATE cpLegalizaDocu " +
                                           "    SET EmpresaID  = @EmpresaID  " +
                                           "    ,CajaMenorID = @CajaMenorID " +
                                           "    ,FechaIni  = @FechaIni   " +
                                           "    ,FechaFin  = @FechaFin   " +
                                           "    ,FechaCont = @FechaCont   " +
                                           "    ,ValorFondo = @ValorFondo  " +
                                           "    ,IdentificadorAnt1 = @IdentificadorAnt1  " +
                                           "    ,ValorAnticipo1 = @ValorAnticipo1     " +
                                           "    ,IdentificadorAnt2 = @IdentificadorAnt2  " +
                                           "    ,ValorAnticipo2 = @ValorAnticipo2     " +
                                           "    ,IdentificadorAnt3 = @IdentificadorAnt3  " +
                                           "    ,ValorAnticipo3 = @ValorAnticipo3     " +
                                           "    ,IdentificadorAnt4 = @IdentificadorAnt4  " +
                                           "    ,ValorAnticipo4 = @ValorAnticipo4     " +
                                           "    ,Valor = @Valor   " +
                                           "    ,IVA = @IVA     " +
                                           "    ,Estado = @Estado  " +
                                           "    WHERE NumeroDoc = @NumeroDoc";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CajaMenorID", SqlDbType.Char, UDT_CajaMenorID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaIni", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaCont", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ValorFondo", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt1", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo1", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt2", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo2", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt3", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo3", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IdentificadorAnt4", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ValorAnticipo4", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@eg_cpCajaMenor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = leg.NumeroDoc.Value;
                mySqlCommand.Parameters["@CajaMenorID"].Value = string.IsNullOrEmpty(leg.CajaMenorID.Value) ? DBNull.Value.ToString() : leg.CajaMenorID.Value;
                mySqlCommand.Parameters["@FechaIni"].Value = leg.FechaIni.Value;
                mySqlCommand.Parameters["@FechaFin"].Value = leg.FechaFin.Value;
                mySqlCommand.Parameters["@FechaCont"].Value = leg.FechaCont.Value;
                mySqlCommand.Parameters["@ValorFondo"].Value = !leg.ValorFondo.Value.HasValue ? null : leg.ValorFondo.Value;
                mySqlCommand.Parameters["@IdentificadorAnt1"].Value = !leg.IdentificadorAnt1.Value.HasValue ? null : leg.IdentificadorAnt1.Value;
                mySqlCommand.Parameters["@ValorAnticipo1"].Value = leg.ValorAnticipo1.Value;
                mySqlCommand.Parameters["@IdentificadorAnt2"].Value = !leg.IdentificadorAnt2.Value.HasValue ? null : leg.IdentificadorAnt2.Value;
                mySqlCommand.Parameters["@ValorAnticipo2"].Value = leg.ValorAnticipo2.Value;
                mySqlCommand.Parameters["@IdentificadorAnt3"].Value = !leg.IdentificadorAnt3.Value.HasValue ? null : leg.IdentificadorAnt3.Value;
                mySqlCommand.Parameters["@ValorAnticipo3"].Value = leg.ValorAnticipo3.Value;
                mySqlCommand.Parameters["@IdentificadorAnt4"].Value = !leg.IdentificadorAnt4.Value.HasValue ? null : leg.IdentificadorAnt4.Value;
                mySqlCommand.Parameters["@ValorAnticipo4"].Value = leg.ValorAnticipo4.Value;
                mySqlCommand.Parameters["@Valor"].Value = leg.Valor.Value;
                mySqlCommand.Parameters["@IVA"].Value = !leg.IVA.Value.HasValue ? null : leg.IVA.Value;
                mySqlCommand.Parameters["@Estado"].Value = leg.Estado.Value;
                mySqlCommand.Parameters["@eg_cpCajaMenor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cpCajaMenor, this.Empresa, egCtrl);

                #endregion
                mySqlCommand.ExecuteNonQuery();   
          
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_cpLegalizaHeader_Upd", false);
                throw exception;
            }           

        }              
        
        /// <summary>
        /// Trae un listado de causaciones pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_CausacionAprobacion> DAL_LegalizaHeader_GetPendientesByModulo(int documentID, ModulesPrefix mod)
        {
            try
            {
                List<DTO_CausacionAprobacion> result = new List<DTO_CausacionAprobacion>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select distinct ctrl.NumeroDoc, PeriodoDoc as PeriodoID, ComprobanteID, ComprobanteIDNro as ComprobanteNro, " +
                    "  ctrl.DocumentoID, DocumentoNro, ter.TerceroID, ter.Descriptivo as DescriptivoTercero, MonedaID, valor, usr.UsuarioID " + 
                    "from glDocumentoControl ctrl with(nolock) " +
                    "   inner join cpCuentaXPagar cxp with(nolock) on ctrl.NumeroDoc = cxp.NumeroDoc" +
                    "	inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                    "	inner join coTercero ter with(nolock) on ctrl.TerceroID = ter.TerceroID " +
                    "   inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID " +
                    "   inner join glTareaPermiso Perm with(nolock) on Perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    "   	and Perm.AreaFuncionalID = Ctrl.AreaFuncionalID " +
                    "where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado and ctrl.TareaID = @TareaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@TareaID", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.ParaAprobacion;
                mySqlCommand.Parameters["@TareaID"].Value = documentID;

                SqlDataReader dr;

                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_CausacionAprobacion dto = new DTO_CausacionAprobacion(dr);
                    dto.Aprobado.Value = false;
                    dto.Rechazado.Value = false;
                    dto.Observacion.Value = string.Empty;
                    result.Add(dto);
                }
                dr.Close();

                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@ComprobanteNro", SqlDbType.Int);

                foreach (DTO_CausacionAprobacion dto in result)
                {
                    mySqlCommand.Parameters["@PeriodoID"].Value = dto.PeriodoID.Value.Value;
                    mySqlCommand.Parameters["@ComprobanteID"].Value = dto.ComprobanteID.Value;
                    mySqlCommand.Parameters["@ComprobanteNro"].Value = dto.ComprobanteNro.Value.Value;

                    mySqlCommand.CommandText =
                        "select TOP 1 Descriptivo " +
                        "from coAuxiliarPre with(nolock) " +
                        "where  " +
                        "	EmpresaID = @EmpresaID AND " +
                        "	PeriodoID = @PeriodoID AND " +
                        "	ComprobanteID = @ComprobanteID AND " +
                        "	ComprobanteNro = @ComprobanteNro "; 

                    object des = mySqlCommand.ExecuteScalar();
                    dto.Descriptivo.Value = des == null ? string.Empty : des.ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);//
                //LoggerModel.WriteLogMessage(exception, true);
                throw exception;
            }
        }

        #endregion

    }
}
