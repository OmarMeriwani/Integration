using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel;
using TivoliWO.ServiceReference1;
using System.Xml.Serialization;
using System.Net;
using System.IO;

namespace TivoliWO
{
    public class TivoliWorkOrder
    {
        public void SendWorkOrder()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 50;
            System.Net.WebClient wc = new System.Net.WebClient();
            HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(new Uri(@"LINK/meaweb/wsdl/MXWO?wsdl"));
            //HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(new Uri(@"LINK/meaweb/wsdl/MXWO?wsdl"));               
            wbRequest.Credentials = new NetworkCredential("USER", "PASSWORD");
            string dateString = String.Format("{0:s}", DateTime.Now);//DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-T" + DateTime.Now.ToUniversalTime().ToString();
            wbRequest.Timeout = 50000;
            byte[] bytes;
            string requestXml = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:max=""http://www.ibm.com/maximo"">
  <soapenv:Header/> 
   <soapenv:Body> 
      <max:CreateMXWO creationDateTime=""2017-07-26T16:29:39+03:00""> 
         <max:MXWOSet> 
         <max:WORKORDER>
              <max:WONUM >TESTTTTTTTTTT</max:WONUM>
               <max:REPORTEDBY >Omar.Sirwan</max:REPORTEDBY>
              <max:REPORTDATE >2017-07-26T16:26:30+03:00</max:REPORTDATE>
              <max:CLASSSTRUCTUREID >2435</max:CLASSSTRUCTUREID>
              <max:LOCATION>BAGHDAD</max:LOCATION>
               <max:ORGID>KOREK</max:ORGID>
			<max:WOCLASS maxvalue=""WORKORDER"">WORKORDER</max:WOCLASS>
			 <max:WOGROUP>1076</max:WOGROUP>
              </max:WORKORDER> 
         </max:MXWOSet> 
      </max:CreateMXWO> 
   </soapenv:Body> 
</soapenv:Envelope>
";
            bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
            wbRequest.ContentType = "text/xml; encoding='utf-8'";
            wbRequest.ContentLength = bytes.Length;
            wbRequest.Method = "POST";
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });

            System.IO.Stream requestStream = wbRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            string strResponse = "";
            try
            {
                using (WebResponse webResponse = wbRequest.GetResponse())
                {
                    var streamReader = new StreamReader(webResponse.GetResponseStream());
                    strResponse = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}