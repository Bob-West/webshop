using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop.Models;
using static Webshop.Models.Shop;

namespace Webshop.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        public ActionResult Index()
        {
            if(Session["User"] == null)
                return View();
            else
                return View("~/Views/Summary/Index.cshtml");
        }
        public ActionResult checkout_user_login()
        {
            if (Request.Form["login_email"].Length == 0)
            {
                // FEHLERMELDUNG
                Debug.Print("email leer");
            }
            else if (Request.Form["login_password"].Length == 0)
            {
                // FEHLERMELDUNG
                Debug.Print("pwd leer");
            }

            if (Shop.loginUser(Request.Form["login_email"], Request.Form["login_password"]))
            {
                User loggedInUser = Shop.getUserData(Request.Form["login_email"], Request.Form["login_password"]);
                Session["User"] = loggedInUser;
                return View("~/Views/Summary/Index.cshtml");
            }
            else
            {
                Debug.Print("login fehlgeschlagen" + Request.Form["login_email"] + " " + Request.Form["login_password"]);
                return View("Index");
            }
        }
        public ActionResult checkout_user_register()
        {

            User newUser = new User();
            // if (float.TryParse(Request.Form["inputProductAmount"], out vAmount) == false)
            if (Request.Form["salutation"].ToString() == "")
            {
                // FEHLERMELDUNG
            }
            else if (Request.Form["register_firstname"].Length == 0)
            {
                //FEHLERMELDUNG
            }
            else if (Request.Form["register_email"].Length == 0)
            {
                //FEHLERMELDUNG
            }
            else if (Request.Form["register_lastname"].Length == 0)
            {
                //FEHLERMELDUNG
            }
            else if (Request.Form["register_password"].Length == 0)
            {
                // FEHLERMELDUNG
            }
            else if (Request.Form["register_telephone"].Length == 0)
            {
                // FEHLERMELDUNG
            }
            else if (Request.Form["register_bill_street"].Length == 0)
            {
                // FEHLERMELDUNG
            }
            else if (Request.Form["register_bill_zipcode"].Length == 0)
            {
                // FEHLERMELDUNG
            }
            else if (Request.Form["register_bill_country"].Length == 0)
            {
                // FEHLERMELDUNG
            }
            else if (Request.Form["register_bill_city"].Length == 0)
            {
                // FEHLERMELDUNG
            }
            else
            {
                newUser.salutation = Request.Form["salutation"];
                newUser.title = Request.Form["register_title"];
                newUser.firstname = Request.Form["register_firstname"];
                newUser.lastname = Request.Form["register_lastname"];
                newUser.email = Request.Form["register_email"];
                newUser.passwd = Request.Form["register_password"];
                newUser.phone = Request.Form["register_telephone"];
                newUser.bill_street = Request.Form["register_bill_street"];
                newUser.bill_zipcode = Request.Form["register_bill_zipcode"];
                newUser.bill_country = Request.Form["register_bill_country"];
                newUser.bill_city = Request.Form["register_bill_city"];

                if (Request.Form["register_delivery_address"] != null && Request.Form["register_delivery_address"] == "on")
                {
                    //Response.Write("This checkbox is selected");
                    newUser.delivery_street = Request.Form["register_delivery_street"];
                    newUser.delivery_zipcode = Request.Form["register_delivery_zipcode"];
                    newUser.delivery_city = Request.Form["register_delivery_city"];
                    newUser.delivery_country = Request.Form["register_delivery_country"];
                }
                else
                {
                    //Response.Write("This checkbox is not selected");
                    newUser.delivery_street = Request.Form["register_bill_street"];
                    newUser.delivery_zipcode = Request.Form["register_bill_zipcode"];
                    newUser.delivery_city = Request.Form["register_bill_city"];
                    newUser.delivery_country = Request.Form["register_bill_country"];
                }
            }

            User registeredUser = Shop.registerUser(newUser);
            if (registeredUser != null)
            {
                Session["User"] = newUser;
                return View("~/Views/Summary/Index.cshtml");
            }
            else
            {
                Debug.Print("kommt nicht rein");
                return View("Index");
               // return HttpNotFound();
            }

        }
    }
}