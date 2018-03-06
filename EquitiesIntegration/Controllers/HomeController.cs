using EquitiesIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EquitiesIntegration.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private EquitiesIntegrationContext db = new EquitiesIntegrationContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            ViewBag.CitizenshipId = new SelectList(db.Citizenships, "CitizenshipId", "Name");
            return View();
        }

        public ActionResult Search()
        {
            ViewBag.CitizenshipId = new SelectList(db.Citizenships, "CitizenshipId", "Name");
            return View();
        }

        public ActionResult Results()
        {
            var list = db.Customers.ToList();
            return View(list);
        }

    }
}
