using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeLibrary;

namespace Lab4NatalieYu
{
    // PROG1224 - Lab4
    // Natalie Hyojung Yu
    // Date: August 7, 2022

    public class ProcessEmployees<T> where T : Employee
    {
        //encapsulated fields for calculating information
        private static int totalCount = 0;
        private static decimal totalPay = 0m;
        private static decimal totalBonus = 0m;
        private static decimal totalDeduction = 0m;

        //read-only properties
        public static string TotalCount => totalCount.ToString();
        public static string TotalPay => totalPay.ToString("C2");
        public static string TotalBonus => totalBonus.ToString("C2");
        public static string TotalDeduction => totalDeduction.ToString("C2");

        //instance fields
        private DateTime period;
        public DateTime Period
        {
            get 
            {
                return (period.Year == 1600) ? DateTime.Now : period;
            }
            set => period = value;
        }
        private List<T> employees;

        //constructor
        public ProcessEmployees(DateTime period, List<T> employees)
        {
            this.period = period;
            this.employees = employees;
        }

        public List<string> ProcessPayRoll()
        {
            List<string> output = new List<string>();
            foreach (Employee emp in employees)
            {
                decimal net = 0M;
                decimal bonus = 0M;
                decimal deduction = 0M;

                net = emp.Calculate();
                bonus = emp.Bonus();
                deduction = emp.IncomeTax() + emp.Insurance() + emp.Pension() + emp.UnionDues();

                totalCount++;
                totalPay += net;
                totalBonus += bonus;
                totalDeduction += deduction;

                output.Add($"{emp.Sin}   {emp.FirstName} {emp.LastName}   Net: ${net}   Bonus: ${bonus}   Deduction: ${deduction}\n") ;
            }
            return output;
        }
    }
}
