using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using MITT_HIMAWAN_SUTANTO.Models;
using System.Configuration;
using ASMIK_MAGI.Repository;
using System.Net;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using static MITT_HIMAWAN_SUTANTO.Models.MenuViewModels;
using System.Collections;
using System.Threading;
using System.Data.OleDb;
using BotDetect.Web.Mvc;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace MITT_HIMAWAN_SUTANTO.Controllers
{
    public class RegisterController : Controller
    {

        string Endpoint_RegisterProfile = ConfigurationManager.AppSettings["Endpoint_RegisterProfile"].ToString();
        // GET: Register
        public ActionResult Insert()
        {

            PagedList_RegisterModels<RegisterModels> model = new PagedList_RegisterModels<RegisterModels>();

            try
            {

            }
            catch(Exception ex)
            {

            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Insert(PagedList_RegisterModels<RegisterModels> model = null, string Submit = "")
        {
            try
            {
                if(Submit== "Add")
                {
                    string serviceUrl = Endpoint_RegisterProfile;
                    object input = new
                    {
                        Usernames = model.UserName.Trim(),
                        Names = model.Name.Trim(),
                        Passwords = model.Password.Trim(),
                        Addresss = model.Address.Trim(),
                        DOBs = GetDateInYYYYMMDD(model.DOB.Trim()),
                        Emails = model.Email
                    };


                    dynamic result = null;
                    string inputJson = (new JavaScriptSerializer()).Serialize(input);
                    HttpClient client = new HttpClient();
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(serviceUrl + "/InsertUser", inputContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer serializer_VA = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var dict = serializer_VA.Deserialize<Dictionary<string, dynamic>>(result);
                    dynamic Body_VA = (dict["InsertUserResult"]);
                    dynamic MSG = (Body_VA[0]["MSG"]);

                    TempData["messageRequest"] = "<script>alert('" + MSG + "');</script>";

                    return RedirectToAction("Index", "Login");
                    
                }
            }
            catch (Exception ex)
            {

            }

            return View();
        }

        #region Menu
        public DataSet Get_Menu(string ConnSQL)
        {

            SqlCommand com = new SqlCommand("exec [sp_Get_Menu_Parent] '" + Session["UserID"] + "'", Common.GetConnection(ConnSQL));

            SqlDataAdapter da = new SqlDataAdapter(com);

            DataSet ds = new DataSet();

            da.Fill(ds);


            return ds;

        }

        public DataSet Get_SubMenu(string ParentID)

        {

            SqlCommand com = new SqlCommand("exec [sp_Get_SubMenu] '" + Session["UserID"] + "',@ParentID");

            com.Parameters.AddWithValue("@ParentID", ParentID);

            SqlDataAdapter da = new SqlDataAdapter(com);

            DataSet ds = new DataSet();

            da.Fill(ds);

            return ds;

        }

        public void get_Submenu(string catid)

        {

            DataSet ds = Get_SubMenu(catid);

            List<SubMenu> submenulist = new List<SubMenu>();

            foreach (DataRow dr in ds.Tables[0].Rows)

            {

                submenulist.Add(new SubMenu
                {

                    MenuID = dr["MenuID"].ToString(),

                    MenuName = dr["MenuName"].ToString(),

                    ActionName = dr["ActionName"].ToString(),

                    ControllerName = dr["ControllerName"].ToString()

                });

            }

            Session["submenu"] = submenulist;

        }
        #endregion
        public string GetDateInYYYYMMDD(string dt)
        {
            if (dt == "1900-01-01")
            {
                return dt;
            }
            string[] stringSeparators = new string[] { "/" };
            string[] str = dt.Split('/');

            if (str[0].Length < 4)
            {
                string tempdt = string.Empty;
                for (int i = 2; i >= 0; i += -1)
                    tempdt += str[i] + "-";
                tempdt = tempdt.Substring(0, 10);
                return tempdt;
            }
            else
            {
                return dt;
            }
        }

    }
}