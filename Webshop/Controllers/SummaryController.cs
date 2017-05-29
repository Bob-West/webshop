using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop.Models;
using static Webshop.Models.Shop;
using static Webshop.Models.Shop.User;

namespace Webshop.Controllers
{
    public class SummaryController : Controller
    {
        // GET: Summary
        public ActionResult Index()
        {
            return View();
        }
     
    }
}