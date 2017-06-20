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
            Shop.Order.save_Order();
            sendMail();
            HttpContext.Session["Cart"] = null;
            return View("danke");
        }

        public ActionResult sendMail()
        {
            //Shop.Order order;
            if (Session["Cart"] == null || Session["User"] == null || Session["Order"] == null) // Change != null
            {
                // Fehlermeldung
                return View("index");
            }
            else
            {
                //Change recipient email!!!!
                String recipientEmail = "azv82041@oepia.com";
                clsEMail.SendEmail("consumer", new Dictionary<clsEMail.RecipientType, String>() { { clsEMail.RecipientType.To, recipientEmail } }, "Bestellbestätigung", null);
                clsEMail.SendEmail("shop", new Dictionary<clsEMail.RecipientType, String>() { { clsEMail.RecipientType.To, recipientEmail } }, "Bestellung erhalten", null);
                return View("danke");
            }

        }

    }
}