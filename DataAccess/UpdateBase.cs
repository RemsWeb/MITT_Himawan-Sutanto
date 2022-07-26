using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace MITT_HIMAWAN_SUTANTO.DataAccess
{
    public class UpdateBase
    {
        private string Conn = ConfigurationManager.ConnectionStrings["ConSql"].ToString();
        private DataAccessBase DataAccess = new DataAccessBase();

        private string const_sp_UPDATE_DATA_USER_PROFILE = "[sp_UPDATE_USER_PROFILE] ";
        public DataTable UPDATE_DATA_USER_PROFILE(string Username, string Name, string Password,
                                              string Address, string DOB, string Email)
        {

            System.Data.DataTable dt = new System.Data.DataTable();

            SqlParameter[] oParam = new SqlParameter[6];

            oParam[0] = new SqlParameter("@Username", SqlDbType.VarChar);
            oParam[0].Value = Convert.ToString(Username);

            oParam[1] = new SqlParameter("@Name", SqlDbType.VarChar);
            oParam[1].Value = Convert.ToString(Name);

            oParam[2] = new SqlParameter("@Password", SqlDbType.VarChar);
            oParam[2].Value = Convert.ToString(Password);

            oParam[3] = new SqlParameter("@Address", SqlDbType.VarChar);
            oParam[3].Value = Convert.ToString(Address);

            oParam[4] = new SqlParameter("@DOB", SqlDbType.VarChar);
            oParam[4].Value = Convert.ToString(DOB);

            oParam[5] = new SqlParameter("@Email", SqlDbType.VarChar);
            oParam[5].Value = Convert.ToString(Email);


            string _connectionString = Conn;

            dt = DataAccess.FillDataset(_connectionString, const_sp_UPDATE_DATA_USER_PROFILE, CommandType.StoredProcedure, oParam);

            return dt;

            // return DataAccess.ExecuteNonQuery(Conn, const_sp_UPDATE_DATA_USER_PROFILE, CommandType.StoredProcedure, oParam);
        }
    }
}