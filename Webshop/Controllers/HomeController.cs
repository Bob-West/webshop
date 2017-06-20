using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop.Models;
using System.Collections;

namespace Webshop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
          return View();
        }

        public ActionResult VielenDank()
        {
            return View("danke");
        }

        public ActionResult sendMail()
        {
            //Shop.Order order;
            if (Session["Cart"] == null || Session["User"] == null || Session["Order"] != null) // Change != null
            {
                // Fehlermeldung
                return View();
            }
            else
            {
                clsEMail.SendEmail("consumer", new Dictionary<clsEMail.RecipientType, String>() { { clsEMail.RecipientType.To, "wqi33563@oepia.com" } }, "Test Mail", null);
            }

            return View("danke");
        }

    }
}