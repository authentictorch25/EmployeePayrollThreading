using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeePayrollMultithread
{
   public class EmployeePayrollOperations
        { 
         public static SqlConnection connection { get; set; }

        /// <summary>
        /// Adds the employee list to data base.
        /// </summary>
        /// <param name="employeeList">The employee list.</param>
        /// <returns></returns>
        public bool AddEmployeeListToDataBase(List<EmployeeModel> employeeList)
        {
            foreach (var employee in employeeList)
            {
                Console.WriteLine("Employee being added:" + employee.EmployeeName);
                bool flag = AddEmployeeToDatabase(employee);
                Console.WriteLine("Employee added:" + employee.EmployeeName);
                if (flag == false)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Adds the employee list to database using thread.
        /// </summary>
        /// <param name="employeeList">The employee list.</param>
        public void AddEmployeeListToDataBaseWithThread(List<EmployeeModel> employeeList)
        {
            employeeList.ForEach(employeeData =>
            {
                //For each employeeData present in list new thread is created and all threads run according
                //to the time slot assigned by the thread scheduler
                Task thread = new Task(() =>
                {
                    Console.WriteLine("Current thread id: " + Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("Employee Being added" + employeeData.EmployeeName);
                    this.AddEmployeeToDatabase(employeeData);
                    Console.WriteLine("Employee added:" + employeeData.EmployeeName);
                });
                thread.Start();
            });
        }

        /// <summary>
        /// Adds the employee to database.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool AddEmployeeToDatabase(EmployeeModel employee)
        {
            DBConnection dbc = new DBConnection();
            connection = dbc.GetConnection();
            try
            {
                using (connection)
                {
                    SqlCommand command = new SqlCommand("dbo.SpAddEmployeeDetails", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@name", employee.EmployeeName);
                    command.Parameters.AddWithValue("@start", employee.StartDate);
                    command.Parameters.AddWithValue("@gender", employee.Gender);
                    command.Parameters.AddWithValue("@phone_number", employee.PhoneNumber);
                    command.Parameters.AddWithValue("@address", employee.Address);
                    command.Parameters.AddWithValue("@department", employee.Department);
                    command.Parameters.AddWithValue("@Basic_Pay", employee.BasicPay);
                    command.Parameters.AddWithValue("@Deductions", employee.Deductions);
                    command.Parameters.AddWithValue("@Taxable_pay", employee.TaxablePay);
                    command.Parameters.AddWithValue("@Income_tax", employee.Tax);
                    command.Parameters.AddWithValue("@Net_pay", employee.NetPay);
                    connection.Open();
                    var result = command.ExecuteNonQuery();
                    connection.Close();
                    if (result != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State.Equals("Open"))
                    connection.Close();
            }
        }
    }
}