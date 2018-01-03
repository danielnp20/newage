using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using System.Threading;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    public class DAL_Mayorizacion : DAL_Base
    {
        #region Variables y Propiedades

        /// <summary>
        /// Diccionario de <campo,tabla> sobre las llaves que se utilizan para la mayorización
        /// </summary>
        private Dictionary<int, DTO_glTabla> tablasPivotes = new Dictionary<int, DTO_glTabla>();
        private Dictionary<int, string> columnasPivotes = new Dictionary<int, string>();
        private DateTime Periodo
        {
            get;
            set;
        }
        private string Balance
        {
            get;
            set;
        }
        private int DocumentID
        {
            get;
            set;
        }

        /// <summary>
        /// Referencia al diccionario de progresos
        /// </summary>
        private Dictionary<Tuple<int, int>, int> BatchProgress
        {
            get;
            set;
        }

        /// <summary>
        /// Progreso de la mayorizacion
        /// </summary>
        private Tuple<int, int> tupProgress;
        private int _progress;
        public int Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                this._progress = value;
                this.BatchProgress[this.tupProgress] = (value * 100) / ProgresoTotal;
            }
        }

        private int ProgresoTotal
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_Mayorizacion(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn)
            : base(c, tx, empresa, userId, loggerConn)
        {
        }

        #region Funciones Publicas

        /// <summary>
        /// Mayoriza un balance para un periodo determinado dentro de una transacción
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="balance">Balance</param>
        /// <param name="userId">Usuario</param>
        /// <param name="batchProgress">Referencia al diccionario para reportar avance</param>
        public DTO_TxResult Proceso_Mayorizar(int documentID, DateTime periodo, string balance, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx, string CuentaAlternativaID, bool esCorporativa, string CuentaID)
        {
            if (!insideAnotherTx)
                base.MySqlConnectionTx = base.MySqlConnection.BeginTransaction();

            this.tupProgress = new Tuple<int, int>(this.UserId, documentID);
            DTO_TxResult res = new DTO_TxResult();
            bool valid = false;
            try
            {
                this.Periodo = periodo;
                this.Balance = balance;
                this.DocumentID = documentID;
                this.BatchProgress = batchProgress;

                #region Valida que el mes de contabilidad no este cerrado

                #endregion

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(this.MySqlConnection, this.MySqlConnectionTx, this.loggerConnectionStr);
                DAL_glTabla dal_Tabla = new DAL_glTabla(base.MySqlConnection, base.MySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Configurar diccionarios

                this.tablasPivotes = new Dictionary<int, DTO_glTabla>();
                this.columnasPivotes = new Dictionary<int, string>();

                int indice = 0;
                DTO_aplMaestraPropiedades props = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, AppMasters.coPlanCuenta, this.loggerConnectionStr);
                this.tablasPivotes.Add(indice, dal_Tabla.DAL_glTabla_GetByTablaNombre(props.NombreTabla, base.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl)));
                this.columnasPivotes.Add(indice, "CuentaID");

                #endregion

                this.ProgresoTotal = this.tablasPivotes[indice].LevelsUsed();
                this.MayorizarSaldos(CuentaAlternativaID, esCorporativa, CuentaID);
                valid = true;
            }
            catch (Exception e)
            {
                res.Result = ResultValue.NOK;
                res.ResultMessage = DictionaryMessages.Err_UpdateData;
            }
            finally
            {
                if (valid)
                {
                    res.Result = ResultValue.OK;
                    if (!insideAnotherTx)
                        base.MySqlConnectionTx.Commit();
                }
                else
                {
                    if (!insideAnotherTx)
                        base.MySqlConnectionTx.Commit();
                }
            }
            return res;
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Mayoriza los saldos para un perios y un tipo de balance
        /// </summary>
        private void MayorizarSaldos(string CuentaAlternativaID, bool isCorporativa, string CuentaID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                string query = "";
                if (!isCorporativa)
                {
                    query =
                        "SELECT DISTINCT EmpresaID,PeriodoID,BalanceTipoID,CuentaID,ProyectoID,CentroCostoID,LineaPresupuestoID," +
                        "	SUM(vlBaseML) vlBaseML, SUM(vlBaseME) vlBaseME, SUM(DbOrigenLocML) DbOrigenLocML,SUM(DbOrigenExtML) DbOrigenExtML, " +
                        "	SUM(DbOrigenLocME) DbOrigenLocME,SUM(DbOrigenExtME) DbOrigenExtME,SUM(CrOrigenLocML) CrOrigenLocML, " +
                        "	SUM(CrOrigenExtML) CrOrigenExtML,SUM(CrOrigenLocME) CrOrigenLocME,SUM(CrOrigenExtME) CrOrigenExtME, " +
                        "	SUM(DbSaldoIniLocML) DbSaldoIniLocML,SUM(DbSaldoIniExtML) DbSaldoIniExtML,SUM(DbSaldoIniLocME) DbSaldoIniLocME, " +
                        "	SUM(DbSaldoIniExtME) DbSaldoIniExtME,SUM(CrSaldoIniLocML) CrSaldoIniLocML,SUM(CrSaldoIniExtML) CrSaldoIniExtML, " +
                        "	SUM(CrSaldoIniLocME) CrSaldoIniLocME,SUM(CrSaldoIniExtME) CrSaldoIniExtME, " +
                        "	eg_coBalanceTipo,eg_coPlanCuenta,eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto,@cuentaAlternaID as CuentaAlternaID " +
                        "FROM coCuentaSaldo " +
                        "WHERE EmpresaID=@EmpresaID AND PeriodoID=@PeriodoID AND BalanceTipoID=@BalanceTipoID " +
                        "GROUP BY EmpresaID,PeriodoID,BalanceTipoID,CuentaID,ProyectoID,CentroCostoID,LineaPresupuestoID," +
                        "	eg_coBalanceTipo,eg_coPlanCuenta,eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto ";
                }
                else
                {
                    query =
                        "SELECT DISTINCT EmpresaID,PeriodoID,BalanceTipoID,@cuentaID as CuentaID,ProyectoID,CentroCostoID,LineaPresupuestoID," +
                        "	SUM(vlBaseML) vlBaseML, SUM(vlBaseME) vlBaseME, SUM(DbOrigenLocML) DbOrigenLocML,SUM(DbOrigenExtML) DbOrigenExtML, " +
                        "	SUM(DbOrigenLocME) DbOrigenLocME,SUM(DbOrigenExtME) DbOrigenExtME,SUM(CrOrigenLocML) CrOrigenLocML, " +
                        "	SUM(CrOrigenExtML) CrOrigenExtML,SUM(CrOrigenLocME) CrOrigenLocME,SUM(CrOrigenExtME) CrOrigenExtME, " +
                        "	SUM(DbSaldoIniLocML) DbSaldoIniLocML,SUM(DbSaldoIniExtML) DbSaldoIniExtML,SUM(DbSaldoIniLocME) DbSaldoIniLocME, " +
                        "	SUM(DbSaldoIniExtME) DbSaldoIniExtME,SUM(CrSaldoIniLocML) CrSaldoIniLocML,SUM(CrSaldoIniExtML) CrSaldoIniExtML, " +
                        "	SUM(CrSaldoIniLocME) CrSaldoIniLocME,SUM(CrSaldoIniExtME) CrSaldoIniExtME, " +
                        "	eg_coBalanceTipo,eg_coPlanCuenta,eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto,CuentaAlternaID " +
                        "FROM coCuentaSaldo " +
                        "WHERE EmpresaID=@EmpresaID AND PeriodoID=@PeriodoID AND BalanceTipoID=@BalanceTipoID " +
                        "GROUP BY EmpresaID,PeriodoID,BalanceTipoID,CuentaID,ProyectoID,CentroCostoID,LineaPresupuestoID," +
                        "	eg_coBalanceTipo,eg_coPlanCuenta,eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto ";
                }
                List<string[]> llaves = new List<string[]>();

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.VarChar, UDT_BalanceTipoID.MaxLength);


                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = this.Periodo;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = this.Balance;
                if (!isCorporativa)
                {
                    mySqlCommand.Parameters.Add("@cuentaAlternaID", SqlDbType.VarChar);
                    mySqlCommand.Parameters["@cuentaAlternaID"].Value = CuentaAlternativaID;
                }
                else
                {
                    mySqlCommand.Parameters.Add("@cuentaID", SqlDbType.VarChar);
                    mySqlCommand.Parameters["@cuentaID"].Value = CuentaID;
                }
                mySqlCommand.CommandText = query;

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    string[] llavesFila = new string[columnasPivotes.Count];
                    foreach (KeyValuePair<int, string> kvp in columnasPivotes)
                        llavesFila[kvp.Key] = dr[kvp.Value].ToString().Trim();

                    llaves.Add(llavesFila);
                }
                dr.Close();

                if (llaves.Count == 0)
                    return;

                string delete = "delete from coBalance WHERE EmpresaID=@EmpresaID AND PeriodoID=@PeriodoID AND BalanceTipoID=@BalanceTipoID ";
                mySqlCommand.CommandText = delete;
                mySqlCommand.ExecuteNonQuery();

                string insert = "insert into coBalance " + query + "";
                mySqlCommand.CommandText = insert;
                mySqlCommand.ExecuteNonQuery();
                if(!isCorporativa)
                    this.MayorizarRollUp(0, llaves, true, isCorporativa,CuentaAlternativaID);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "MayorizarSaldos");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion recursiva que mayoriza las jerarquias luego de haber mayorizado las hojas
        /// </summary>
        /// <param name="indiceLlave">Inicialmente es 0, la fnción automaticamente lo incrementa para hacer sus propios llamados</param>
        /// <param name="llaves">Llaves que se van a mayorizar</param>
        /// <param name="first">Indica si es el primer llamado. Al arrancar debe ser true</param>
        private void MayorizarRollUp(int indiceLlave, List<string[]> llaves, bool first = false, bool isCorporativa = false, string CuentaAlternativaID="")
        {
            this.Progress++;
            SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
            mySqlCommand.Transaction = base.MySqlConnectionTx;

            mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
            mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
            mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.VarChar, UDT_BalanceTipoID.MaxLength);

            mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
            mySqlCommand.Parameters["@PeriodoID"].Value = this.Periodo;
            mySqlCommand.Parameters["@BalanceTipoID"].Value = this.Balance;

            if (!isCorporativa)
            {
                mySqlCommand.Parameters.Add("@cuentaAlternaID", SqlDbType.VarChar);
                mySqlCommand.Parameters["@cuentaAlternaID"].Value = CuentaAlternativaID;
            }
            List<string[]> llavesPadres = new List<string[]>();
            foreach (string[] arr in llaves)
            {
                string andLlaves = string.Empty;
                string selectLlaves = string.Empty;
                string pivotCode = arr[indiceLlave];
                int childLength = ChildLength(indiceLlave, pivotCode);
                if (!first)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (i == indiceLlave)
                        {
                            andLlaves += " AND " + columnasPivotes[i] + " LIKE '" + arr[i] + "%' AND LEN(" + columnasPivotes[i] + ")=" + childLength;
                            selectLlaves += ",'" + pivotCode + "' " + columnasPivotes[i];
                        }
                        else
                        {
                            andLlaves += " AND " + columnasPivotes[i] + " = '" + arr[i] + "'";
                            selectLlaves += "," + columnasPivotes[i];
                        }
                    }
                    string query="";
                    if (!isCorporativa)
                    {
                        query =
                            "SELECT DISTINCT EmpresaID,PeriodoID,BalanceTipoID" + selectLlaves + "," +
                                "ProyectoID,CentroCostoID,LineaPresupuestoID,SUM(vlBaseML) vlBaseML,SUM(vlBaseME) vlBaseME,SUM(DbOrigenLocML) DbOrigenLocML," +
                                "SUM(DbOrigenExtML) DbOrigenExtML,SUM(DbOrigenLocME) DbOrigenLocME,SUM(DbOrigenExtME) DbOrigenExtME,SUM(CrOrigenLocML) CrOrigenLocML," +
                                "SUM(CrOrigenExtML) CrOrigenExtML,SUM(CrOrigenLocME) CrOrigenLocME,SUM(CrOrigenExtME) CrOrigenExtME," +
                                "SUM(DbSaldoIniLocML) DbSaldoIniLocML,SUM(DbSaldoIniExtML) DbSaldoIniExtML,SUM(DbSaldoIniLocME) DbSaldoIniLocME," +
                                "SUM(DbSaldoIniExtME) DbSaldoIniExtME,SUM(CrSaldoIniLocML) CrSaldoIniLocML,SUM(CrSaldoIniExtML) CrSaldoIniExtML," +
                                "SUM(CrSaldoIniLocME) CrSaldoIniLocME,SUM(CrSaldoIniExtME) CrSaldoIniExtME," +
                                "eg_coBalanceTipo,eg_coPlanCuenta,eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto,@cuentaAlternaID as CuentaAlternaID " +
                            "FROM " +
                            "(" +
                            "   SELECT EmpresaID,PeriodoID,BalanceTipoID" + selectLlaves + "," +
                            "       ProyectoID,CentroCostoID,LineaPresupuestoID,vlBaseML,vlBaseME,DbOrigenLocML,DbOrigenExtML,DbOrigenLocME,DbOrigenExtME, CrOrigenLocML," +
                            "       CrOrigenExtML,CrOrigenLocME,CrOrigenExtME,DbSaldoIniLocML,DbSaldoIniExtML,DbSaldoIniLocME,DbSaldoIniExtME,CrSaldoIniLocML," +
                            "       CrSaldoIniExtML,CrSaldoIniLocME,CrSaldoIniExtME,eg_coBalanceTipo,eg_coPlanCuenta,eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto,CuentaAlternaID " +
                            "   FROM coBalance with(nolock) WHERE 1=1 " + andLlaves +
                            ") coBalance " +
                            "WHERE EmpresaID=@EmpresaID AND PeriodoID=@PeriodoID AND BalanceTipoID=@BalanceTipoID " +
                            "GROUP BY EmpresaID,PeriodoID,BalanceTipoID,CuentaID,ProyectoID,CentroCostoID,LineaPresupuestoID,eg_coBalanceTipo,eg_coPlanCuenta,eg_coProyecto," +
                            "   eg_coCentroCosto,eg_plLineaPresupuesto,CuentaAlternaID";
                    }
                    else
                    {
                        query =
                            "SELECT DISTINCT EmpresaID,PeriodoID,BalanceTipoID" + selectLlaves + "," +
                                "ProyectoID,CentroCostoID,LineaPresupuestoID,SUM(vlBaseML) vlBaseML,SUM(vlBaseME) vlBaseME,SUM(DbOrigenLocML) DbOrigenLocML," +
                                "SUM(DbOrigenExtML) DbOrigenExtML,SUM(DbOrigenLocME) DbOrigenLocME,SUM(DbOrigenExtME) DbOrigenExtME,SUM(CrOrigenLocML) CrOrigenLocML," +
                                "SUM(CrOrigenExtML) CrOrigenExtML,SUM(CrOrigenLocME) CrOrigenLocME,SUM(CrOrigenExtME) CrOrigenExtME," +
                                "SUM(DbSaldoIniLocML) DbSaldoIniLocML,SUM(DbSaldoIniExtML) DbSaldoIniExtML,SUM(DbSaldoIniLocME) DbSaldoIniLocME," +
                                "SUM(DbSaldoIniExtME) DbSaldoIniExtME,SUM(CrSaldoIniLocML) CrSaldoIniLocML,SUM(CrSaldoIniExtML) CrSaldoIniExtML," +
                                "SUM(CrSaldoIniLocME) CrSaldoIniLocME,SUM(CrSaldoIniExtME) CrSaldoIniExtME," +
                                "eg_coBalanceTipo,eg_coPlanCuenta,eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto,CuentaAlternaID " +
                            "FROM " +
                            "(" +
                            "   SELECT EmpresaID,PeriodoID,BalanceTipoID" + selectLlaves + "," +
                            "       ProyectoID,CentroCostoID,LineaPresupuestoID,vlBaseML,vlBaseME,DbOrigenLocML,DbOrigenExtML,DbOrigenLocME,DbOrigenExtME, CrOrigenLocML," +
                            "       CrOrigenExtML,CrOrigenLocME,CrOrigenExtME,DbSaldoIniLocML,DbSaldoIniExtML,DbSaldoIniLocME,DbSaldoIniExtME,CrSaldoIniLocML," +
                            "       CrSaldoIniExtML,CrSaldoIniLocME,CrSaldoIniExtME,eg_coBalanceTipo,eg_coPlanCuenta,eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto,CuentaAlternaID " +
                            "   FROM coBalance with(nolock) WHERE 1=1 " + andLlaves +
                            ") coBalance " +
                            "WHERE EmpresaID=@EmpresaID AND PeriodoID=@PeriodoID AND BalanceTipoID=@BalanceTipoID " +
                            "GROUP BY EmpresaID,PeriodoID,BalanceTipoID,CuentaID,ProyectoID,CentroCostoID,LineaPresupuestoID,eg_coBalanceTipo,eg_coPlanCuenta,eg_coProyecto," +
                            "   eg_coCentroCosto,eg_plLineaPresupuesto,CuentaAlternaID";
                    }
                    string insert =
                        "insert into coBalance (" +
                        "   EmpresaID,PeriodoID,BalanceTipoID,CuentaID,ProyectoID,CentroCostoID,LineaPresupuestoID,vlBaseML,vlBaseME,DbOrigenLocML,DbOrigenExtML," +
                        "   DbOrigenLocME,DbOrigenExtME, CrOrigenLocML,CrOrigenExtML,CrOrigenLocME,CrOrigenExtME,DbSaldoIniLocML,DbSaldoIniExtML,DbSaldoIniLocME," +
                        "   DbSaldoIniExtME,CrSaldoIniLocML,CrSaldoIniExtML,CrSaldoIniLocME,CrSaldoIniExtME," +
                        "   eg_coBalanceTipo,eg_coPlanCuenta,eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto,CuentaAlternaID) " + query + "";

                    mySqlCommand.CommandText = insert;
                    try
                    {
                        mySqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                        Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "MayorizarRollUp");
                        throw exception;
                    }
                }

                if (!isRoot(indiceLlave, pivotCode))
                {
                    //Subir en la jerarquia
                    string[] nuevoArr = (string[])arr.Clone();
                    nuevoArr[indiceLlave] = ParentCode(indiceLlave, arr[indiceLlave]);
                    if (!llavesPadres.Contains(nuevoArr, new StringArrayComparer()))
                    {
                        llavesPadres.Add(nuevoArr);
                    }
                }
            }
            if (llavesPadres.Count > 0)
            {
                //Mayorizar padres
                this.MayorizarRollUp(indiceLlave, llavesPadres,false,isCorporativa,CuentaAlternativaID);
            }

            for (int nuevoIndice = indiceLlave + 1; nuevoIndice < columnasPivotes.Count(); nuevoIndice++)
            {
                List<string[]> nuevasLlaves = new List<string[]>();
                foreach (string[] arr in llaves)
                {
                    string[] nuevoArr = (string[])arr.Clone();
                    nuevoArr[nuevoIndice] = ParentCode(nuevoIndice, arr[nuevoIndice]);
                    if (!nuevasLlaves.Contains(nuevoArr, new StringArrayComparer()))
                    {
                        nuevasLlaves.Add(nuevoArr);
                    }
                }
                this.MayorizarRollUp(nuevoIndice, nuevasLlaves);
            }
        }

        /// <summary>
        /// Indica si es la raiz de la jerarquia
        /// </summary>
        /// <param name="indice"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool isRoot(int indice, string code)
        {
            return (this.tablasPivotes[indice].CodeLength(1) == code.Length);
        }

        /// <summary>
        /// Codigo del padre
        /// </summary>
        /// <param name="indice"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private string ParentCode(int indice, string code)
        {
            DTO_glTabla table = this.tablasPivotes[indice];
            int level = table.LengthToLevel(code.Length);
            level--;
            int lengthParent = table.CodeLength(level);
            return code.Substring(0, lengthParent);
        }

        /// <summary>
        /// Longiotud de el hijo dado el codigo del padre
        /// </summary>
        /// <param name="indice"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private int ChildLength(int indice, string code)
        {
            DTO_glTabla table = this.tablasPivotes[indice];
            int level = table.LengthToLevel(code.Length);
            level++;
            int lengthChild = table.CodeLength(level);
            return lengthChild;
        }

        #endregion
    }

    /// <summary>
    /// Clase para comparar arreglos de string
    /// </summary>
    public class StringArrayComparer : IEqualityComparer<string[]>
    {
        public bool Equals(string[] b1, string[] b2)
        {
            return Enumerable.SequenceEqual(b1, b2);
        }
        public int GetHashCode(string[] bx)
        {
            return bx.GetHashCode();
        }
    }

}
