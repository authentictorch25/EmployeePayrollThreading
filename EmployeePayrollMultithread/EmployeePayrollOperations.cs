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

        NLog nLog = new NLog();

        /// <summary>
        /// Mutex is mutual exclusion used to synchronise the threads
        /// Similar to locks
        /// </summary>
        private static Mutex mutex = new Mutex();

        /// <summary>
        /// Adds the employee list to data base.
        /// </summary>
        /// <param name="employeeList">The employee list.</param>
        public bool AddEmployeeListToDataBase(List<EmployeeModel> employeeList)
        {
            foreach (var employee in employeeList)
            {
                nLog.LogDebug($"Adding the Employee: {employee.EmployeeName} via ThreadID: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine("Employee being added:" + employee.EmployeeName);
                bool flag = AddEmployeeToDatabase(employee);
                Console.WriteLine("Employee added:" + employee.EmployeeName);
                nLog.LogInfo($"Employee {employee.EmployeeName} added in Database via ThreadId: " + Thread.CurrentThread.ManagedThreadId);
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
                nLog.LogDebug($"Adding the Employee: {employeeData.EmployeeName} without synchronization via ThreadID: {Thread.CurrentThread.ManagedThreadId}");
                //For each employeeData present in list new thread is created and all threads run according
                //to the time slot assigned by the thread scheduler
                Thread thread = new Thread(() =>
                {
                    Console.WriteLine("Current thread id: " + Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("Employee Being added" + employeeData.EmployeeName);
                    AddEmployeeToDatabase(employeeData);
                    Console.WriteLine("Employee added:" + employeeData.EmployeeName);
                    nLog.LogInfo($"Employee {employeeData.EmployeeName} added in Database without synchronization via ThreadId: " + Thread.CurrentThread.ManagedThreadId);
                });
                thread.Start();
            });
            Console.WriteLine("Total added employees: " + employeeList.Count);
        }

        /// <summary>
        /// Adds the employee list to data base with synchronization.
        /// </summary>
        /// <param name="employeeList">The employee list.</param>
        public void AddEmployeeListToDataBaseWithSynchronization(List<EmployeeModel> employeeList)
        {
            /// Iterating over all the employee model list and point individual employee detail of the list
            employeeList.ForEach(employeeData =>
            {
                Task thread = new Task(() =>
                {
                    //Lock the set of codes for the current employeeData
                    lock (employeeData)
                    {
                        //mutex.WaitOne();
                        nLog.LogDebug($"Adding the Employee: {employeeData.EmployeeName} using synchronization via ThreadID: {Thread.CurrentThread.ManagedThreadId}");
                        Console.WriteLine("Current thread id: " + Thread.CurrentThread.ManagedThreadId);
                        Console.WriteLine("Employee Being added" + employeeData.EmployeeName);
                        this.AddEmployeeToDatabase(employeeData);
                        Console.WriteLine("Employee added:" + employeeData.EmployeeName);
                        nLog.LogInfo($"Employee {employeeData.EmployeeName} added in Database using synchronization via ThreadId: " + Thread.CurrentThread.ManagedThreadId);
                        //mutex.ReleaseMutex();
                    }
                });
                thread.Start();
                thread.Wait();
            });
        }

        /// <summary>
        /// Adds the employee list to multiple tables with synchronization.
        /// </summary>
        /// <param name="employeeList">The employee list.</param>
        public void AddEmployeeListToMultipleTablesWithSynchronization(List<EmployeeModel> employeeList)
        {
            /// Iterating over all the employee model list and point individual employee detail of the list
            employeeList.ForEach(employeeData =>
            {
                Task thread = new Task(() =>
                {
                    //Lock the set of codes for the current employeeData
                    lock (employeeData)
                    {
                        //mutex.WaitOne();
                        nLog.LogDebug($"Adding the Employee: {employeeData.EmployeeName} using synchronization via ThreadID: {Thread.CurrentThread.ManagedThreadId}");
                        Console.WriteLine("Current thread id: " + Thread.CurrentThread.ManagedThreadId);
                        Console.WriteLine("Employee Being added" + employeeData.EmployeeName);
                        this.AddEmployeeDetailsIntoMultipleTables(employeeData);
                        Console.WriteLine("Employee added:" + employeeData.EmployeeName);
                        nLog.LogInfo($"Employee {employeeData.EmployeeName} added in Database using synchronization via ThreadId: " + Thread.CurrentThread.ManagedThreadId);
                        //mutex.ReleaseMutex();
                    }
                });
                thread.Start();
                thread.Wait();
            });
        }

        /// <summary>
        /// Adds the employee details into multiple tables.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool AddEmployeeDetailsIntoMultipleTables(EmployeeModel employee)
        {
            lock (this)
            {
                DBConnection dbc = new DBConnection();
                connection = dbc.GetConnection();
                try
                {
                    using (connection)
                    {
                        SqlCommand command = new SqlCommand("dbo.spAddEmployeeDetailsIntoMultipleTablesMultiThreading", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@empname", employee.EmployeeName);
                        command.Parameters.AddWithValue("@start_date", employee.StartDate);
                        command.Parameters.AddWithValue("@gender", employee.Gender);
                        command.Parameters.AddWithValue("@phonenumber", employee.PhoneNumber);
                        command.Parameters.AddWithValue("@address", employee.Address);
                        command.Parameters.AddWithValue("@department", employee.Department);
                        command.Parameters.AddWithValue("@Basic_Pay", employee.BasicPay);
                        command.Parameters.AddWithValue("@Deductions", employee.Deductions);
                        command.Parameters.AddWithValue("@Taxable_pay", employee.TaxablePay);
                        command.Parameters.AddWithValue("@tax", employee.Tax);
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
        /// <summary>
        /// Adds the employee to database.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool AddEmployeeToDatabase(EmployeeModel employee)
        {
            lock (this)
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
        /// <summary>
        /// Updates the salary details using threads with synchronize.
        /// </summary>
        /// <param name="employeeList">The employee list.</param>
        public void UpdateSalaryDetailsUsingThreadsWithSync(List<EmployeeModel> employeeList)
        {
            /// Iterating over all the employee model list and point individual employee detail of the list
            employeeList.ForEach(employeeData =>
            {
                Task thread = new Task(() =>
                {
                    //Lock the set of codes for the current employeeData
                    lock (employeeData)
                    {
                        //mutex.WaitOne();
                        nLog.LogDebug($"Updating the Employee: {employeeData.EmployeeName} using synchronization via ThreadID: {Thread.CurrentThread.ManagedThreadId}");
                        Console.WriteLine("Current thread id: " + Thread.CurrentThread.ManagedThreadId);
                        Console.WriteLine("Employee whose salary being updated" + employeeData.EmployeeName);
                        this.UpdateSalaryDetailsIntoMultipleTable(employeeData);
                        Console.WriteLine("Employee whose salary updated:" + employeeData.EmployeeName);
                        nLog.LogInfo($"Employee {employeeData.EmployeeName} updated in Database using synchronization via ThreadId: " + Thread.CurrentThread.ManagedThreadId);
                        //mutex.ReleaseMutex();
                    }
                });
                thread.Start();
                thread.Wait();
            });
        }
        public bool UpdateSalaryDetailsIntoMultipleTable(EmployeeModel employee)
        {
            lock (this)
            {
                DBConnection dbc = new DBConnection();
                connection = dbc.GetConnection();
                try
                {
                    using (connection)
                    {
                        SqlCommand command = new SqlCommand("dbo.spUpdateSalary", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@empname", employee.EmployeeName);
                        command.Parameters.AddWithValue("@Basic_Pay", employee.BasicPay);
                        command.Parameters.AddWithValue("@Deductions", employee.Deductions);
                        command.Parameters.AddWithValue("@Taxable_pay", employee.TaxablePay);
                        command.Parameters.AddWithValue("@tax", employee.Tax);
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
}