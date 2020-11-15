using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EmployeePayrollMultithread
{
    public class DBConnection
    {
        public SqlConnection GetConnection()
        {
            //For windows authentication
            //public static string ConnectionString = "Data Source=ASEEMANAND\SQLEXPRESS;Initial Catalog=payroll_service;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //For sql authentication
            string connectionString = @"Server=LAPTOP-V5IRNHKS\SQLEXPRESS; Initial Catalog =payroll_services; User ID = akash; Password=akash2507";
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
