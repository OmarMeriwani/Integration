using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel;
using TivoliWO.ServiceReference1;

namespace TivoliWO.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            MXWO_WORKORDERType a = new MXWO_WORKORDERType();
            a.DESCRIPTION = new MXStringType { Value = "TEST" };
            a.LOCATION = new MXStringType { Value = "BABEL" };
            a.CLASSSTRUCTUREID = new MXStringType { Value = "06" };
            a.ASSIGNEDOWNERGROUP = new MXStringType() { Value = "MKT" };
            a.OWNERGROUP = new MXStringType() { Value = "MKT" };
            
            MXWO_WORKORDERType[] m = new MXWO_WORKORDERType[1];
            m[0] = a;
            ServiceReference1.MXWOPortTypeClient MXWOPORTCLIENT = new MXWOPortTypeClient();
            
            CreateMXWORequest CMXWReq = new CreateMXWORequest();
            CMXWReq.MXWOSet = m;
            CMXWReq.creationDateTime = DateTime.Now ;
            
            MXWOPORTCLIENT.CreateMXWOAsync(CMXWReq);
            //DateTime cd = DateTime.Today;
            //string ss = "";
            //MXWOPORTCLIENT.CreateMXWO(m,ref cd ,ref  ss,ref ss,ref ss,ref ss );

            return View();
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