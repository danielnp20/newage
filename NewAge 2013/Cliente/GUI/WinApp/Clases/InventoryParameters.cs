using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    public static class InventoryParameters
    {
        //Para uso general de los formularios
        private static BaseController _bc = BaseController.GetInstance();
        private static List<DTO_glConsultaFiltro> filtrosParam1 = new List<DTO_glConsultaFiltro>();
        private static List<DTO_glConsultaFiltro> filtrosParam2 = new List<DTO_glConsultaFiltro>();

        /// <summary>
        /// Obtiene los parametros de la referencias de acuerdo al tipo de ref(inRefTipo)
        /// </summary>
        /// <param name="refTipoID">Tipo Referencia</param>
        /// <param name="isParametro1">Consulta parametro 1</param>
        /// <returns></returns>
        public static List<DTO_glConsultaFiltro> GetQueryParameters(string refTipoID, bool isParametro1)
        {
            if (isParametro1)
            {
                //Trae los Tipos de parametro1 que existen
                List<string> listParameter1 = new List<string>();
                long countP1 = _bc.AdministrationModel.MasterComplex_Count(AppMasters.inTipoParameter1, null, null);
                var tipoParameter1 = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.inTipoParameter1, countP1, 1, null, null).ToList();
              
                #region Trae filtro de parametro 1
                if (tipoParameter1 != null)
                {
                   
                    foreach (var p1 in tipoParameter1)
                    {
                        //Si el Tipo de la lista anterior coincide con el tipo de la referencia seleccionada lo agrega
                        DTO_inTipoParametro1 dtoTipo = (DTO_inTipoParametro1)p1;
                        if (dtoTipo.TipoInvID.Value == refTipoID)
                            listParameter1.Add(dtoTipo.Parametro1ID.Value);
                    }
                    int indexLast = listParameter1.Count - 1;
                    filtrosParam1.Clear();
                    //Filtra el control de maestra con los datos obtenidos
                    foreach (string p1 in listParameter1)
                    {
                        if (p1 != listParameter1[indexLast])
                            filtrosParam1.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "Parametro1ID",
                                OperadorFiltro = OperadorFiltro.Igual,
                                ValorFiltro = p1,
                                OperadorSentencia = "OR"
                            });
                        else
                            filtrosParam1.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "Parametro1ID",
                                OperadorFiltro = OperadorFiltro.Igual,
                                ValorFiltro = p1,
                            });
                    }
                }
                return filtrosParam1; 
                    #endregion
            }
            else
            {               
                //Trae los Tipos de parametro2 que existen
                List<string> listParameter2 = new List<string>();
                long countP2 = _bc.AdministrationModel.MasterComplex_Count(AppMasters.inTipoParameter2, null, null);
                var tipoParameter2 = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.inTipoParameter2, countP2, 1, null, null).ToList();
            
                #region Trae filtro de parametro 2
                if (tipoParameter2 != null)
                {
                   
                    foreach (var p2 in tipoParameter2)
                    {
                        //Si el Tipo de la lista anterior coincide con el tipo de la referencia seleccionada lo agrega
                        DTO_inTipoParametro2 dtoTipo = (DTO_inTipoParametro2)p2;
                        if (dtoTipo.TipoInvID.Value == refTipoID)
                            listParameter2.Add(dtoTipo.Parametro2ID.Value);
                    }
                    int indexLast = listParameter2.Count - 1;
                    filtrosParam2.Clear();
                    //Filtra el control de maestra con los datos obtenidos
                    foreach (string p2 in listParameter2)
                    {
                        if (p2 != listParameter2[indexLast])
                            filtrosParam2.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "Parametro2ID",
                                OperadorFiltro = OperadorFiltro.Igual,
                                ValorFiltro = p2,
                                OperadorSentencia = "OR"
                            });
                        else
                            filtrosParam2.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "Parametro2ID",
                                OperadorFiltro = OperadorFiltro.Igual,
                                ValorFiltro = p2,
                            });
                    }
                }
                return filtrosParam2; 
                #endregion
            }
        }
    }
}
