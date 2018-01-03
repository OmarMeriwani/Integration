 public bool IsInstalled(string MSISDN)
    {
        DataTable dttResParams = new DataTable();
        AIR_CMD ClassAIR_CMD = new AIR_CMD();
        ClassAIR_CMD.GetAccountDetails(MSISDN, ref dttResParams);
        string str_activationStatusFlag = "";
        foreach (DataRow dtrIMSIRows in dttResParams.Rows)
        {
            if (dtrIMSIRows["name"].ToString().ToLower() == "activationstatusflag")
            {
                str_activationStatusFlag = dtrIMSIRows["value"].ToString();
                if (str_activationStatusFlag == "0")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }
        return false;
    }