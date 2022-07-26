using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using MITT_HIMAWAN_SUTANTO.Models;
using System.Configuration;
using MITT_HIMAWAN_SUTANTO.Repository;
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
using System.Globalization;

namespace MITT_HIMAWAN_SUTANTO.Controllers
{
    public class UserController : Controller
    {
        string Endpoint_RegisterProfile = ConfigurationManager.AppSettings["Endpoint_RegisterProfile"].ToString();

        string ConnSQL = "ConSql";
        // GET: User
        public ActionResult UserProfile()
        {

            PagedList_User<User> model = new PagedList_User<User>();

            try
            {
                if (Session["UserID"] == null || Session["UserID"].ToString() == "")
                {
                    return RedirectToAction("Index", "Login");
                }

                DataSet ds = Get_Menu(ConnSQL);
                Session["menu"] = ds.Tables[0];
                Session["controller"] = "UserController";

                ViewBag.menu = Session["menu"];

                model.UserName = Session["UserID"].ToString();
                model.Name = Session["Name"].ToString();
                model.Address = Session["Address"].ToString();
                model.Email = Session["Email"].ToString();
                model.DOB = Session["DOB"].ToString();

                model.Password = Session["Password"].ToString();
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }
        [HttpPost]
        public ActionResult UserProfile(PagedList_User<User> model, string Submit)
        {
            try
            {
                if (Session["UserID"] == null || Session["UserID"].ToString() == "")
                {
                    return RedirectToAction("Index", "Login");
                }

                DataSet ds = Get_Menu(ConnSQL);
                Session["menu"] = ds.Tables[0];
                Session["controller"] = "UserController";

                ViewBag.menu = Session["menu"];

                if(Submit=="Add")
                {
                    string serviceUrl = Endpoint_RegisterProfile;
                    object input = new
                    {
                        Usernames = model.UserName.Trim(),
                        Names = model.Name.Trim(),
                        Passwords = model.Password.Trim(),
                        Addresss = model.Address.Trim(),
                        DOBs = ConverttoFormatDate(model.DOB.Trim()),
                        Emails = model.Email,
                        Menu = "UserUpdate"
                    };


                    dynamic result = null;
                    string inputJson = (new JavaScriptSerializer()).Serialize(input);
                    HttpClient client = new HttpClient();
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(serviceUrl + "/UpdateUser", inputContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer serializer_VA = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var dict = serializer_VA.Deserialize<Dictionary<string, dynamic>>(result);
                    dynamic Body_VA = (dict["UpdateUserResult"]);
                    dynamic MSG = (Body_VA[0]["MSG"]);

                    TempData["messageRequest"] = "<script>alert('" + MSG + "');</script>";

                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            { 
            }
            return View(model);
        }
            
        public ActionResult UserSkills()
        {
            if (Session["UserID"] == null || Session["UserID"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }

            DataSet ds = Get_Menu(ConnSQL);
            Session["menu"] = ds.Tables[0];
            Session["controller"] = "UserController";

            ViewBag.menu = Session["menu"];

            return View();
        }
        private static string ConverttoFormatDate(string Date)
        {
            string formatDate = Date.Replace("12:00:00 AM","").Trim();
            String sDate = Date;
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            String dy = datevalue.Day.ToString();
            String mn = datevalue.Month.ToString();
            if(mn.Length == 1)
            {
                mn = "0" + mn;
            }
            String yy = datevalue.Year.ToString();

            formatDate = yy + "-" + mn + "-" + dy;  

            return formatDate;
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

        public DataSet Get_SubMenu(string ParentID, string ConSQL)

        {

            SqlCommand com = new SqlCommand("exec [sp_Get_SubMenu] '" + Session["UserID"] + "',@ParentID", Common.GetConnection(ConnSQL));

            com.Parameters.AddWithValue("@ParentID", ParentID);

            SqlDataAdapter da = new SqlDataAdapter(com);

            DataSet ds = new DataSet();

            da.Fill(ds);

            return ds;

        }

        public void get_Submenu(string catid)

        {

            DataSet ds = Get_SubMenu(catid, (ConnSQL));

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