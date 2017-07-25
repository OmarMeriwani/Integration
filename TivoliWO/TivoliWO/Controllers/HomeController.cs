using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel;

namespace TivoliWO.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
            ServiceReference1.MXWO_WORKORDERType a = new ServiceReference1.MXWO_WORKORDERType();
           
            ServiceReference1.CreateMXWORequest m = new ServiceReference1.CreateMXWORequest();
            m.MXWOSet[0] = a;
            ServiceReference1.QueryMXWORequest mxq = new ServiceReference1.QueryMXWORequest();
            

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}