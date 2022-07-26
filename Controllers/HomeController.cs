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


namespace MITT_HIMAWAN_SUTANTO.Controllers
{
    public class HomeController : Controller
    {

        string ConnSQL = "ConSql";
        public ActionResult Index()
        {
            if (Session["UserID"] == null || Session["UserID"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }

            DataSet ds = Get_Menu(ConnSQL);
            Session["menu"] = ds.Tables[0];
            Session["controller"] = "HomeController";

            ViewBag.menu = Session["menu"];
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

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

        public DataSet Get_SubMenu(string ParentID,string ConSQL)

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

    }
}