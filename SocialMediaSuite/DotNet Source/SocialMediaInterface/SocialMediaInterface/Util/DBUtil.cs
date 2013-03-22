using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace SocialMediaInterface.Util
{
    class DBUtil
    {
        string conStr = string.Empty;

        SqlConnection sqlCon;

        /// <summary>
        /// constructor
        /// </summary>
        public DBUtil()
        {
            this.conStr = ConfigurationManager.ConnectionStrings["local"].ToString();
            this.sqlCon = new SqlConnection(this.conStr);
        }

        /// <summary>
        /// constructor with connection string
        /// </summary>
        /// <param name="connectionStr"></param>
        public DBUtil(string connectionStr)
        {
            this.conStr = connectionStr;
            this.sqlCon = new SqlConnection(this.conStr);
        }

        /// <summary>
        /// Insert method
        /// </summary>
        /// <param name="inserSQL"></param>
        /// <returns></returns>
        public int InsertData(string inserSQL)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            this.sqlCon.Open();
            adapter.InsertCommand = new SqlCommand(inserSQL, this.sqlCon);
            int row = adapter.InsertCommand.ExecuteNonQuery();
            this.sqlCon.Close();
            return row;
        }

        /// <summary>
        /// Query execution method
        /// </summary>
        /// <param name="query"></param>
        public void ExecuteQuery(string query)
        {
            SqlCommand cmd = new SqlCommand(query,this.sqlCon);
            this.sqlCon.Open();
            cmd.ExecuteNonQuery();
            this.sqlCon.Close();
        }
    }
}
