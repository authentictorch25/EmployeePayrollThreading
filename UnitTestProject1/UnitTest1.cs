using EmployeePayrollMultithread;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using EmployeePayrollMultithread;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        EmployeePayrollOperations empOperation = new EmployeePayrollOperations();
        [TestMethod]
        public void GivenEmployeeList_AddEmployeeListToDatabase()
        {
            //Arrange
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            employeeList.Add(new EmployeeModel(employeeName: "Kohli", startDate: new System.DateTime(2019, 08, 01), phoneNumber: 4567876543, address: "Bangalore", department: "HR", gender: "M", basicPay: 50000, deductions: 2000, taxablePay: 48000, tax: 1000, netPay: 47000));
            employeeList.Add(new EmployeeModel(employeeName: "Dhoni", startDate: new System.DateTime(2020, 01, 01), phoneNumber: 4563784678, address: "Chennai", department: "HR", gender: "M", basicPay: 50000, deductions: 2000, taxablePay: 48000, tax: 1000, netPay: 47000));
            employeeList.Add(new EmployeeModel(employeeName: "Rohit", startDate: new System.DateTime(2020, 02, 01), phoneNumber: 7865678765, address: "Mumbai", department: "Sales", gender: "M", basicPay: 60000, deductions: 2000, taxablePay: 58000, tax: 1000, netPay: 57000));
            employeeList.Add(new EmployeeModel(employeeName: "Mandhana", startDate: new System.DateTime(2019, 02, 01), phoneNumber: 3456765434, address: "Delhi", department: "Marketing", gender: "F", basicPay: 60000, deductions: 2000, taxablePay: 58000, tax: 1000, netPay: 57000));
            employeeList.Add(new EmployeeModel(employeeName: "Chahal", startDate: new System.DateTime(2020, 04, 12), phoneNumber: 4567898754, address: "Bangalore", department: "Sales", gender: "M", basicPay: 60000, deductions: 2000, taxablePay: 58000, tax: 1000, netPay: 57000));
            bool expected = true;
            Stopwatch s = new Stopwatch();

            //Act
            s.Start();
            bool actual = empOperation.AddEmployeeListToDataBase(employeeList);
            s.Stop();
            Console.WriteLine("Elapsed time: " + s.ElapsedMilliseconds);

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
