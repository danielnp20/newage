using NewAge.Cliente.GUI.PortalCartera.Infrastructure.Controller;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewAge.Cliente.GUI.PortalCartera.Controllers
{
    /// <summary>
    /// Class HomeController:
    /// Provides the search functionalities
    /// </summary>
    [AjaxAuthorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// List of credits
        /// </summary>
        /// <returns>Returns the list of credits</returns>
        [HttpPost]
        public JsonResult GetCredits(DateTime date)
        {
            var clientId = this.Persistance.UserId;
            var creditos = this.CarteraProxy.GetCreditoByClienteAndFecha(clientId, date, true, true);
            List<DTO_CreditoOnline> infoSaldos = new List<DTO_CreditoOnline>();
            foreach(var credito in creditos)
            {
                var infoCredito = this.CarteraProxy.GetSaldoCredito(credito.NumeroDoc.Value.Value, date, true, true, true, false);
                var ppMora = infoCredito.PlanPagos.Where(c => c.FechaCuota.Value.Value < date).ToList();
                if (ppMora.Count > 0)
                {
                    var saldo = ppMora.Sum(y => y.VlrSaldo.Value);
                    infoSaldos.Add(new DTO_CreditoOnline()
                    {
                        Libranza = credito.Libranza.Value.Value,
                        FechaPago = ppMora.FirstOrDefault().FechaCuota.Value.Value,
                        Valor = Convert.ToInt64(saldo)
                        //Tipo = "Propio"
                    });
                }
            }

            var model = new
            {
                saldos = infoSaldos,
                total = infoSaldos.Count
            };

            return new JsonNetResult() { Data = model };
        }

	}
}