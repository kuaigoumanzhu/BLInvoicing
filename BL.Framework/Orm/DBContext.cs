using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BL.Framework.Orm
{
    public class DBContext
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["BLDBContext"].ConnectionString;

        public SqlConnection OpneConnection()
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        public SqlConnection OpenConnection(bool mars=false)
        {
            var cs = connectionString;
            if (mars)
            {
                var scsb = new SqlConnectionStringBuilder(cs) { 
                    MultipleActiveResultSets=true
                };
                cs = scsb.ConnectionString;
            }
            var conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}
