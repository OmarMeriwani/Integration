using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Dapper;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;
using System.ServiceModel;
using Shortcodes.MXSRRef;
using System.IO;
using System.Xml.Linq;

namespace Shortcodes.Controllers
{
    public class TivoliClass
    {

        public static string SendTicket(string CreatedBy, string ReportedBy,string Shortcode, string description, string operation, string scid, string username, string password)
        {
            try 
            {
                //2400	Change Rate	CHANGE RATE
                //2399	Remove Short Code	REMOVE SC
                //2398	NewService_IVR_SMS	IVR_SMS
                //2396	NewService_SMS	SMS
                //2397	NewService_IVR	IVR
                int classid = 0;
                string summary = "";
                if (operation == "CHANGE RATE")
                {
                    classid = 2400;
                    summary = "Change Shortcode Rate #" + Shortcode;
                }
                else if (operation == "REMOVE")
                {
                    classid = 2399;
                    summary = "Remove shortcode #" + Shortcode;
                }
                else if (operation == "IVR_SMS")
                {
                    classid = 2398;
                    summary = "Create IVR SMS Shortcode #" + Shortcode;

                }
                else if (operation == "SMS")
                {
                    classid = 2396;
                    summary = "Create SMS Shortcode #" + Shortcode;
                }
                else if (operation == "IVR")
                {
                    classid = 2397;
                    summary = "Create IVR Shortcode #" + Shortcode;
                }

                description = description + "\nService link: https://enterprise.korektel.com/Shortcodes/Service/Details/" + scid.ToString();
                MXSRRef.MXSR j = new MXSRRef.MXSR();
                j.Credentials = new NetworkCredential(username, password);
                ByPassCert.BypassCertificateError();
                j.Url = "APPLICATION_URL/meaweb/wsdl/MXSR?wsdl";
                List<MXSRRef.MXSR_SRType> t = new List<MXSRRef.MXSR_SRType>();
                MXSRRef.MXSR_SRType k = new MXSRRef.MXSR_SRType();
                k.ACTLABCOST = new MXSRRef.MXDoubleType() { Value = 0 };
                k.ACTLABHRS = new MXSRRef.MXDoubleType() { Value = 0 };
                k.OWNER = new MXSRRef.MXStringType() { Value = CreatedBy.ToUpper() };
                k.CHANGEDATE = new MXSRRef.MXDateTimeType() { Value = DateTime.Now };
                k.CREATEDBY = new MXSRRef.MXStringType() { Value = CreatedBy.ToUpper() };
                k.CREATIONDATE = new MXSRRef.MXDateTimeType() { Value = DateTime.Now };
                k.REPORTEDBY = new MXSRRef.MXStringType() { Value = ReportedBy.ToUpper() };
                k.CCVOUCHERSN3 = new MXSRRef.MXStringType() { Value = scid };
                k.SITEID = new MXSRRef.MXStringType() { Value = "site_it" };
                k.DESCRIPTION = new MXSRRef.MXStringType() { Value = summary };
                k.DESCRIPTION_LONGDESCRIPTION = new MXSRRef.MXStringType() { Value = description };
                k.ASSIGNEDOWNERGROUP = new MXSRRef.MXStringType() { Value = "MKT" };
                k.OWNERGROUP = new MXSRRef.MXStringType() { Value = "MKT" };
                k.EXTERNALSYSTEM = new MXSRRef.MXDomainType() { Value = "SHORTCODE" };
                t.Add(k);
                DateTime d = DateTime.Now;
                string reflang = "";
                string translang = "";
                string no = "";
                string maximoVersiom = "";
                bool uiui = true;
                MXSRRef.SRKeyType[] kh = j.CreateMXSR(t.ToArray(), ref d, ref uiui, ref reflang, ref   translang, ref  no, ref  maximoVersiom);
                string nooo = kh[0].TICKETID.Value;
                string nooo = "";
                return nooo;
            }
            catch (System.Net.WebException exx)
            { throw exx; }
            catch (Exception ex)
            {
                string message = ex.Message;
                string km = ex.InnerException.Message;
                throw ex;
            }
            
        }
        public static string SendTicket(string Shortcode, string description, string operation, string scid, string username, string password)
        {
            //2400	Change Rate	CHANGE RATE
            //2399	Remove Short Code	REMOVE SC
            //2398	NewService_IVR_SMS	IVR_SMS
            //2396	NewService_SMS	SMS
            //2397	NewService_IVR	IVR
            int classid = 0;
            string summary = "";
            if (operation == "CHANGE RATE")
            {
                classid = 2400;
                summary = "Change Shortcode Rate #" + Shortcode;
            }
            else if (operation == "REMOVE")
            {
                classid = 2399;
                summary = "Remove shortcode #" + Shortcode;
            }
            else if (operation == "IVR_SMS")
            {
                classid = 2398;
                summary = "Create IVR SMS Shortcode #" + Shortcode;

            }
            else if (operation == "SMS")
            {
                classid = 2396;
                summary = "Create SMS Shortcode #" + Shortcode;
            }
            else if (operation == "IVR")
            {
                classid = 2397;
                summary = "Create IVR Shortcode #" + Shortcode;
            }

            description = description + "\nService link: /Service/Details/" + scid.ToString();
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 50;
                System.Net.WebClient wc = new System.Net.WebClient();
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(new Uri(@"SERVICE_URL/services/MXSR"));
                wbRequest.Credentials = new NetworkCredential("USER", "PASSWORD");
                string dateString = String.Format("{0:s}", DateTime.Now);
                wbRequest.Timeout = 50000;
                byte[] bytes;
                string requestXml = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:max=""http://www.ibm.com/maximo""> 
   <soapenv:Header/> 
   <soapenv:Body> 
      <max:CreateMXSR> 
         <max:MXSRSet> 
            <!--Zero or more repetitions:--> 
            <max:SR> 
               <!--Optional:--> 
              <max:ASSIGNEDOWNERGROUP changed=""?"">MKT</max:ASSIGNEDOWNERGROUP>
              <max:OWNERGROUP changed=""?"">MKT</max:OWNERGROUP>
              <max:EXTERNALSYSTEM changed=""?"">SHORTCODE</max:EXTERNALSYSTEM>
              <max:SITEID changed=""?"">site_it</max:SITEID>
              <max:DESCRIPTION changed=""?"">" + summary + @"</max:DESCRIPTION>
                <max:CCVOUCHERSN3 changed=""?"">"+ scid +@"</max:CCVOUCHERSN3>
               <max:CREATEDBY changed=""?"">" + username + @"</max:CREATEDBY>
               <max:DESCRIPTION_LONGDESCRIPTION changed=""?"">" + description + @"</max:DESCRIPTION_LONGDESCRIPTION>
               <max:OWNER changed=""?"">" + username + @"</max:OWNER>
               <max:REPORTEDBY changed=""?"">" + username +@"</max:REPORTEDBY>
              <max:CREATIONDATE changed=""?"">" + dateString + @"</max:CREATIONDATE>
              <max:CHANGEDATE changed=""?"">" + dateString + @"</max:CHANGEDATE>
              <max:CLASSSTRUCTUREID changed=""?"">" + classid + @"</max:CLASSSTRUCTUREID>
               <!--Optional:--> 
            </max:SR> 
         </max:MXSRSet> 
      </max:CreateMXSR> 
   </soapenv:Body> 
</soapenv:Envelope>";
                bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
                wbRequest.ContentType = "text/xml; encoding='utf-8'";
                wbRequest.ContentLength = bytes.Length;
                wbRequest.Method = "POST";
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback (   delegate { return true; });

                Stream requestStream = wbRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();

                string strResponse = "";
                try
                {
                    using (WebResponse webResponse = wbRequest.GetResponse())
                    {
                        var streamReader = new StreamReader(webResponse.GetResponseStream());
                        strResponse = streamReader.ReadToEnd();
                        string TicketID = GetIDFromXml(strResponse);
                        return TicketID;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {

                throw ex ;
            }
            finally
            {

            }
        }


     
}