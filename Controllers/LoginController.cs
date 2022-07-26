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


namespace MITT_HIMAWAN_SUTANTO.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            Session.Remove("Rolez");
            Session.Remove("UserID");
            Session.Remove("Company");
            Session.Remove("Kode_Cabang");
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

    }
}