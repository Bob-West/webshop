using System;
using System.Collections.Generic;   
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop.Models;

namespace Webshop.Controllers
{
  public class ShopController : Controller
  {
    // GET: Shop
    public ActionResult Index()
    {
      return View();
    }
    public ActionResult Cart()
    {
        return View("Cart", Session["Cart"]);
    }
        public ActionResult addtocart()
        {
            float vAmount = 0;
            int vProductID = 0;
            //Datenprüfung
            //amount: vorhanden? valide Zahl?

            if (Request.Form["inputProductAmount"].Length == 0)
            {
                //FEHLERMELDUNG
            }
            else
            {
                if (float.TryParse(Request.Form["inputProductAmount"], out vAmount) == false)
                {
                    //FEHLERMELDUNG
                }
            }

            if (Request.Form["inputProductID"].Length == 0)
            {
                //FEHLERMELDUNG
            }
            else
            {
                if (int.TryParse(Request.Form["inputProductID"], out vProductID) == false)
                {
                    //FEHLERMELDUNG
                }
            }

            if(Shop.AddtoCartItem(vProductID, vAmount))
            {
                //return View("Index", Session["Cart"] as Shop.Cart);
                return View("~/Views/Home/Index.cshtml", Session["Cart"] as Shop.Cart);
            }
            else
            {
                Debug.Print("kommt nicht rein");
                return HttpNotFound();
            }
                 
        }

        public ActionResult deletefromcart(int id)
        {
            if (Shop.deleteItemfromCart(id))
            {
                Debug.Print("kommt rein"+ id);
                return View("Cart", Session["Cart"] as Shop.Cart);
            }
            else
            {
                Debug.Print("kommt nicht rein");
                return HttpNotFound();
            }

        }

        public ActionResult updateAmount(int id)
        {
            
            float vAmount = 0;
            
            if (Request.Form["addamount"].Length == 0)
            {
                //FEHLERMELDUNG
            }
            else
            {
                if (float.TryParse(Request.Form["addamount"], out vAmount) == false)
                {
                    //FEHLERMELDUNG
                }
            }


            if (Shop.updateItemAmount(id,vAmount))
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