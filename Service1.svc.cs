using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MITT_HIMAWAN_SUTANTO.DataAccess;


namespace MITT_HIMAWAN_SUTANTO
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        InsertBase oInsert = new InsertBase();
        SelectBase oSelect = new SelectBase();
        UpdateBase oUpdate = new UpdateBase();

        public List<InsertMessage> InsertUser(string Usernames, string Names, string Passwords, string Addresss, string DOBs, string Emails, string Menu)
        {
            List<InsertMessage> GetDetail_Data = new List<InsertMessage>();

            try
            {

                InsertMessage data = new InsertMessage();

                DataTable dtInsert = new DataTable();
                dtInsert = oInsert.f_sp_INSERT_USER_PROFILE(Usernames, Names, Passwords, Addresss, DOBs, Emails,Menu);

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

            }
            catch(Exception ex)
            {

            }
            return GetDetail_Data;
        }

        public List<UpdateMEssage> UpdateUser(string Usernames, string Names, string Passwords, string Addresss, string DOBs, string Emails,string Menu)
        {
            List<UpdateMEssage> GetDetail_Data = new List<UpdateMEssage>();

            try
            {

                UpdateMEssage data = new UpdateMEssage();

                DataTable dtUpdate = new DataTable();
                dtUpdate = oInsert.f_sp_INSERT_USER_PROFILE(Usernames, Names, Passwords, Addresss, DOBs, Emails,Menu);

                if (dtUpdate.Rows.Count > 0)
                {
                    GetDetail_Data.Clear();

                    foreach (DataRow item in dtUpdate.Rows)
                    {
                        data.MSG = item["MSG"].ToString();
                        GetDetail_Data.Add(data);
                    }
                }
                else
                {
                    data.MSG = "";

                }

            }
            catch (Exception ex)
            {

            }
            return GetDetail_Data;
        }

        public List<GetDetail> CheckLogin(string Usernames, string Passwords)
        {
            List<GetDetail> GetDetail_Data = new List<GetDetail>();

            try
            {

                GetDetail data = new GetDetail();

                DataTable dtSelect = new DataTable();
                dtSelect = oSelect.f_CHECK_LOGIN(Usernames, Passwords);

                if (dtSelect.Rows.Count > 0)
                {
                    GetDetail_Data.Clear();

                    foreach (DataRow item in dtSelect.Rows)
                    {
                        data.MSG = item["MSG"].ToString();
                        data.UserName = item["Username"].ToString();
                        data.Name = item["Name"].ToString();
                        data.DOB = item["DOB"].ToString();
                        data.Address = item["Address"].ToString();
                        data.Email = item["Email"].ToString();
                        data.Password = item["Password"].ToString();
                        GetDetail_Data.Add(data);
                    }
                }
                else
                {
                    data.MSG = "";
                    data.UserName = "";
                    data.Name = "";
                    data.Password = "";
                    data.DOB = "";
                    data.Address = "";
                    data.Email = "";
                }

            }
            catch (Exception ex)
            {

            }
            return GetDetail_Data;
        }
    }
}
