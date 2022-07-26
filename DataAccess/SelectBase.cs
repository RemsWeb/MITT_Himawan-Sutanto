using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace MITT_HIMAWAN_SUTANTO.DataAccess
{
    public class SelectBase
    {
        private string Conn = ConfigurationManager.ConnectionStrings["ConSql"].ToString();
        private DataAccessBase DataAccess = new DataAccessBase();

        private string const_sp_GET_DETAIL_UserList = "sp_GET_DETAIL_UserList";
        private string const_sp_Check_Login = "sp_Check_Login";
        
        public DataTable f_GET_DETAIL_UserList(string Usernames)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            SqlParameter[] oParam = new SqlParameter[1];

            oParam[0] = new SqlParameter("@Usernames", SqlDbType.VarChar);
            oParam[0].Value = Convert.ToString(Usernames);
            

            string _connectionString = Conn;
            dt = DataAccess.FillDataset(_connectionString, const_sp_GET_DETAIL_UserList, CommandType.StoredProcedure, oParam);

            return dt;
        }

        public DataTable f_CHECK_LOGIN(string Usernames, string Password)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            SqlParameter[] oParam = new SqlParameter[2];

            oParam[0] = new SqlParameter("@Username", SqlDbType.VarChar);
            oParam[0].Value = Convert.ToString(Usernames);

            oParam[1] = new SqlParameter("@Password", SqlDbType.VarChar);
            oParam[1].Value = Convert.ToString(Password);

            string _connectionString = Conn;
            dt = DataAccess.FillDataset(_connectionString, const_sp_Check_Login, CommandType.StoredProcedure, oParam);

            return dt;
        }

    }
}