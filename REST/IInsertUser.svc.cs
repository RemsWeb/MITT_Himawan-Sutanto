using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MITT_HIMAWAN_SUTANTO.DataAccess;

namespace MITT_HIMAWAN_SUTANTO.REST
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "IInsertUser" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select IInsertUser.svc or IInsertUser.svc.cs at the Solution Explorer and start debugging.
    public class IInsertUser : IIInsertUser
    {
        InsertBase oInsert = new InsertBase();

        public List<InsertMessage> InsertUser(string Usernames, string Names, string Passwords, string Addresss, string DOBs, string Emails)
        {

            List<InsertMessage> GetDetail_Data = new List<InsertMessage>();
            InsertMessage data = new InsertMessage();

            DataTable dtInsert = new DataTable();
            dtInsert = oInsert.f_sp_INSERT_USER_PROFILE(Usernames, Names, Passwords, Addresss, DOBs, Emails);

            if (dtInsert.Rows.Count > 0)
            {
                GetDetail_Data.Clear();

                foreach (DataRow item in dtInsert.Rows)
                {
                    data.MSG = item["MSG"].ToString();
                    GetDetail_Data.Add(data);
                }
            }
            else
            {
                data.MSG = "";

            }


            return GetDetail_Data;
        }
    }
}
