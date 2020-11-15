using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayrollMultithread
{
    public class EmployeeModel
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public double PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public string Gender { get; set; }
        public double BasicPay { get; set; }
        public double Deductions { get; set; }
        public double TaxablePay { get; set; }
        public double Tax { get; set; }
        public double NetPay { get; set; }
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeModel"/> class.
        /// </summary>
        /// <param name="employeeName">Name of the employee.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="address">The address.</param>
        /// <param name="department">The department.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="basicPay">The basic pay.</param>
        /// <param name="deductions">The deductions.</param>
        /// <param name="taxablePay">The taxable pay.</param>
        /// <param name="tax">The tax.</param>
        /// <param name="netPay">The net pay.</param>
        public EmployeeModel(string employeeName, DateTime startDate, double phoneNumber, string address, string department, string gender, double basicPay, double deductions, double taxablePay, double tax, double netPay)
        {
            this.EmployeeName = employeeName;
            this.StartDate = startDate;
            this.PhoneNumber = phoneNumber;
            this.Address = address;
            this.Department = department;
            this.Gender = gender;
            this.BasicPay = basicPay;
            this.Deductions = deductions;
            this.TaxablePay = taxablePay;
            this.Tax = tax;
            this.NetPay = netPay;
        }
    }
}
