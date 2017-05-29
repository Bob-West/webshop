using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Webshop.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult user_register()
        {
            float vAmount = 0;
            int vProductID = 0;

            if (Request.Form["inputProductAmount"].Length == 0 || float.TryParse(Request.Form["inputProductAmount"], out vAmount) == false)
            {
                //FEHLERMELDUNG
            }

            if (Shop.registerUser(vProductID, vAmount))
            {
                return View("Cart", Session["Cart"] as Shop.Cart);
            }
            else
            {
                Debug.Print("kommt nicht rein");
                return HttpNotFound();
            }

        }
    }
}