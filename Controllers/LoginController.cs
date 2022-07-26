using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
//using MITT_HIMAWAN_SUTANTO.Repository;
using MITT_HIMAWAN_SUTANTO.Models;
using System.Web.Security;
using System.Data.SqlClient;
using Microsoft.Owin.Security;
using System.Text;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace MITT_HIMAWAN_SUTANTO.Controllers
{
    public class LoginController : Controller
    {
        string Endpoint_RegisterProfile = ConfigurationManager.AppSettings["Endpoint_RegisterProfile"].ToString();

        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            Session.Remove("UserID");
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            HttpContext.Session.Clear();
            HttpContext.Session.Abandon();
            //AuthenticationManager.SignOut();
            LoginModels model = new LoginModels();
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            string ConnSQL = string.Empty;

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(LoginModels model, string Submit)
        {
            try
            {
                if(Submit== "Change")
                {
                    Session["UserID"] = "Administrator";
                    return RedirectToAction("Insert", "Register");
                }
                else
                {
                    string serviceUrl = Endpoint_RegisterProfile;
                    object input = new
                    {
                        Usernames = model.username.Trim(),
                        Passwords = model.password.Trim()

                    };


                    dynamic result = null;
                    string inputJson = (new JavaScriptSerializer()).Serialize(input);
                    HttpClient client = new HttpClient();
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(serviceUrl + "/CheckLogin", inputContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer serializer_VA = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var dict = serializer_VA.Deserialize<Dictionary<string, dynamic>>(result);
                    dynamic Body_VA = (dict["CheckLoginResult"]);
                    dynamic MSG = "";
                    dynamic Usernames = "";
                    dynamic Name = "";
                    dynamic Address = "";
                    dynamic DOB = "";
                    dynamic Email = "";
                    dynamic Password = "";


                    if (Body_VA.Count !=0)
                    {
                        MSG = (Body_VA[0]["MSG"]); 
                    }
                    
                    if(MSG== "Data Valid")
                    {
                        Usernames = (Body_VA[0]["UserName"]);
                        Name = (Body_VA[0]["Name"]);
                        Address = (Body_VA[0]["Address"]);
                        DOB = Convert.ToDateTime((Body_VA[0]["DOB"]));
                        Email = (Body_VA[0]["Email"]);
                        Password =  (Body_VA[0]["Password"]);

                        Session["UserID"] = Usernames;
                        Session["Name"] = Name;
                        Session["DOB"] = DOB;
                        Session["Address"] = Address;
                        Session["Email"] = Email;
                        Session["Password"] = Password;

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        if(MSG=="")
                        {
                            MSG = "Data Tidak Ada";
                        }

                        TempData["messageRequest"] = "<script>alert('" + MSG + "');</script>";

                        return RedirectToAction("Index", "Login");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message.ToString() + "');</script>");
            }
            return View(model);
        }
      

        private readonly Random _random = new Random();
        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        public void AntiFixationInit()
        {
            var value = "";
            value = Guid.NewGuid().ToString();
            //value = RandomString(24);
            HttpCookie mycookie = HttpContext.Request.Cookies["ASPFIXATION"];

            mycookie = new HttpCookie("ASPFIXATION");
            //mycookie.Value = value;

            mycookie.Value = value;

            Response.Cookies.Add(mycookie);
            //Response.Cookies["ASPFIXATION"] = value;
            Session["ASPFIXATION"] = value;
        }

        public void AntiFixationVerify(object LoginPage)
        {
            var cookie_value = "";
            var session_value = "";
            cookie_value = Request.Cookies["ASPFIXATION"].ToString();
            session_value = Session["ASPFIXATION"].ToString();
            if (cookie_value != session_value)
                Response.Redirect("Index");
        }
        public ActionResult LogOff()
        {

            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            HttpContext.Session.Clear();
            HttpContext.Session.Abandon();
            //AuthenticationManager.SignOut();
            Session.RemoveAll();
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", string.Empty));
                Response.Cookies["ASP.NET_SessionId"].HttpOnly = true;
                Response.Cookies.Remove("ASP.NET_SessionId");

                //

                Response.Cookies["ASPFIXATION"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["ASPFIXATION"].Value = string.Empty;
                Response.Cookies.Add(new HttpCookie("ASPFIXATION", string.Empty));
                Response.Cookies["ASPFIXATION"].HttpOnly = true;
                Response.Cookies.Add(new HttpCookie("ASPFIXATION", ""));
                Response.Cookies.Remove("ASPFIXATION");
                HttpContext.Request.Cookies.Clear();

            }


            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Login");
        }

    }
}