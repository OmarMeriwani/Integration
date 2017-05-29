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

/// <summary>
/// Summary description for TivoliDB_commands
/// </summary>
public class TivoliDB_commands
{
	public TivoliDB_commands()
	{
		   private OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["Tivolidbdata"].ConnectionString);
        private IDbConnection _db = new OracleConnection(ConfigurationManager.ConnectionStrings["Tivolidbdata"].ConnectionString);

        public List<Models.Ticket> SearchTicket(int? scid)
        {
            try
            {
                //2400	Change Rate	CHANGE RATE
                //2399	Remove Short Code	REMOVE SC
                //2398	NewService_IVR_SMS	IVR_SMS
                //2396	NewService_SMS	SMS
                //2397	NewService_IVR	IVR
                string commadn = @"SELECT TICKETID,ccvouchersn ,DESCRIPTION,STATUS,STATUSDATE,REPORTEDBY,REPORTDATE,CREATEDBY,CREATIONDATE FROM MAXIMO.TICKET where status  <> 'CLOSED'  and classstructureid in ('2396','2397','2398','2399','2400') and ccvouchersn  is not null ";
                if (scid != null)
                {
                    string addition = " and ccvouchersn = '" + scid.ToString() + "'";
                    return this._db.Query<Models.Ticket>(commadn + addition).ToList();
                }
                else
                { return this._db.Query<Models.Ticket>(commadn).ToList(); }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in retrieving data from tivoli database " + ex.Message);
            }
        }
        public List<Models.Ticket> SearchTicket(bool OpenClose, DateTime? from, DateTime? to)
        {
            try
            {
                //2400	Change Rate	CHANGE RATE
                //2399	Remove Short Code	REMOVE SC
                //2398	NewService_IVR_SMS	IVR_SMS
                //2396	NewService_SMS	SMS
                //2397	NewService_IVR	IVR
                string commadn = @"SELECT TICKETID,ccvouchersn ,DESCRIPTION,STATUS,STATUSDATE,REPORTEDBY,REPORTDATE,CREATEDBY,CREATIONDATE FROM MAXIMO.TICKET where status   " + (OpenClose == true ? " <> 'CLOSED' " : " = 'CLOSED'") + @"  and CREATIONDATE >= coalesce(:fromm,CREATIONDATE)   and 
CREATIONDATE <=  coalesce(:too,CREATIONDATE ) and classstructureid in ('2396','2397','2398','2399','2400') and ccvouchersn  is not null order by STATUSDATE desc";
                return this._db.Query<Models.Ticket>(commadn,new {fromm = from , too = to }).ToList(); 
            }
            catch (Exception ex)
            {
                throw new Exception("Error in retrieving data from tivoli database " + ex.Message);
            }
        }
        //public List<Models.Ticket> TicketHistory(int? scid)
        //{
        //    try
        //    {
        //        string commadn = @"SELECT TICKETID,AFFECTEDPHONE SC_ID,DESCRIPTION,STATUS,STATUSDATE,REPORTEDBY,REPORTDATE,CREATEDBY,CREATIONDATE FROM MAXIMO.TICKET where  siteid = 'site_it'";
        //        if (scid != null)
        //        {
        //            string addition = " and ccvouchersn='" + scid.ToString() + "'";
        //            return this._db.Query<Models.Ticket>(commadn + addition).ToList();
        //        }
        //        else
        //        { return this._db.Query<Models.Ticket>(commadn).ToList(); }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error in retrieving data from tivoli database " + ex.Message);
        //    }
        //}
        //select ccvouchersn from ticket where status='CLOSED'  and classstructureid in ('2385','2383','2381','2380','2386') and ccvouchersn  is not null
        public List<Models.Ticket> TicketHistory(int scid)
        {
            try
            {
                string commadn = @"select jj.ticketid, jj.owner, jj.owndate, jj.ownerchangeby, tt.status from TKOWNERHISTORY jj join ticket tt on jj.ticketid= tt.ticketid   where tt.ticketid in (select ticketid from ticket where ccvouchersn=:sc_id )";

                return this._db.Query<Models.Ticket>(commadn, new { sc_id = scid.ToString()}).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in retrieving data from tivoli database " + ex.Message);
            }
        }
        public List<Models.Ticket> TicketStatusHistory(int scid)
        {
            try
            {
                string commadn = @"select ticketid, Status, changeby, changedate,owner from TKSTATUS  where ticketid=(select max(tt.ticketid) from ticket tt where tt.ccvouchersn=:sc_id) order by changedate desc";
                return this._db.Query<Models.Ticket>(commadn, new { sc_id = scid.ToString() }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in retrieving ticket status data from tivoli database " + ex.Message);
            }
        }


        public string FinishedTickets()
        {
            try
            {
                //2400	Change Rate	CHANGE RATE
                //2399	Remove Short Code	REMOVE SC
                //2398	NewService_IVR_SMS	IVR_SMS
                //2396	NewService_SMS	SMS
                //2397	NewService_IVR	IVR
                string commadn = @"select count(*) from ticket  where status  <> 'CLOSED'  and classstructureid in ('2396','2397','2398','2399','2400') and ccvouchersn  is not null";

                return this._db.Query<string>(commadn).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in retrieving data from tivoli database " + ex.Message);
            }
        }

        public string PendingInTechnicalCount()
        {
            try
            {
                string commadn = @"select count(*) from ticket  where status  <> 'CLOSED'  and classstructureid in ('2396','2397','2398','2399','2400') and ccvouchersn  is not null";
                return this._db.Query<string>(commadn).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in retrieving data from tivoli database " + ex.Message);
            }
        }
        public List<int> PendingInTechnical()
        {
            try
            {
                string commadn = @"select ccvouchersn from ticket where status  <> 'CLOSED'  and classstructureid in ('2396','2397','2398','2399','2400') and ccvouchersn  is not null";
                return this._db.Query<int>(commadn).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in retrieving data from tivoli database" + ex.Message);
            }
        }
        public List<int> PendingInTechnicalTicketNumbers()
        {
            try
            {
                string commadn = @"select ticketid from ticket where status  <> 'CLOSED'  and classstructureid in ('2396','2397','2398','2399','2400') and ccvouchersn  is not null";
                return this._db.Query<int>(commadn).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in retrieving data from tivoli database " + ex.Message);
            }
        }
        //
        public List<Models.Ticket> TicketHistoryByTicketID(int ticketid)
        {
            try
            {
                string commadn = @"select ticketid, ownergroup, owner, owndate, ownerchangeby from tkownerhistory where ticketid=:ticketid";
                string addition = " and ccvouchersn = '" + ticketid.ToString() + "'";
                return this._db.Query<Models.Ticket>(commadn + addition).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in retrieving data from tivoli database " + ex.Message);
            }
        }

        public List<Models.Ticket> TicketsProgress (List<int> ticketNumbers)
        {
            List<Models.Ticket> tt = new List<Models.Ticket>();
           // ('2396','2397','2398','2399','2400')
            //2400	Change Rate	CHANGE RATE
            //2399	Remove Short Code	REMOVE SC
            //2398	NewService_IVR_SMS	IVR_SMS
            //2396	NewService_SMS	SMS
            //2397	NewService_IVR	IVR
            string Command = @"select 
round(
(select count (*) from (select  distinct status from TKSTATUS where ticketid=:tkid and status like ('PEND%')))
/
(
select 
(Case  CLASSSTRUCTUREID 
                                       when '2399' then 6
                                       when '2400' then 4
                                       when '2396' then 9
                                       when '2397' then 9
                                       when '2398' then 8 end)
from ticket where ticketid=:tkid),2 )*100 progress, (select 
case lower(status) 
when 'closed' then 'progress-bar-success'
when 'retmkt' then 'progress-bar-danger'
when 'retra' then 'progress-bar-danger'
when 'retra' then 'progress-bar-danger'
else 'progress-bar-danger' end style  from ticket where ticketid=:tkid
) style , (select description  from ticket where ticketid=:tkid)  description ,
(select status  from ticket where ticketid=:tkid) status  from dual";
            if (ticketNumbers.Count != 0 )
            {
                foreach (int i in ticketNumbers)
                {
                    Models.Ticket y = new Models.Ticket();
                    y.DESCRIPTION = "";

                        y = this._db.Query<Models.Ticket>(Command, new { tkid = i }).SingleOrDefault();
                    
                    if (String.IsNullOrEmpty(y.DESCRIPTION) == false) 
                    {
                        tt.Add(y);
                    }
                }
            }
            else
            {
                Exception ex = new Exception("No items requested for the progress bar");
                throw ex;
            }
            if (tt.Count == 0)
            {
                Exception ex = new Exception("No items exist for the progress bar");
                throw ex;
            }
            return tt;
        }
    }
	}
}