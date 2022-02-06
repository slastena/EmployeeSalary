using System;

namespace EmployeeSalary
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var company = new Company("Good Company LLC");
            var employee = new Employee("JackJane Doe Jr.", 10.2m);
            Employee employee2 = new Employee("Michael Enlin", 10.1m);

            var hiringDate = new DateTime(2021, 1, 1);
            company.AddEmployee(employee, hiringDate);
            company.AddEmployee(employee2, hiringDate);

            var periodStart = new DateTime(hiringDate.Year, hiringDate.Month, 1, 0, 1, 0);
            var periodEnd = DateTime.Now;

            company.AddHours(employee.Id, periodStart, 1, 1);
            company.AddHours(employee.Id, periodStart.AddMonths(1), 2, 2);
            company.AddHours(employee.Id, periodStart.AddYears(1).AddMonths(2), 3, 3);
            company.AddHours(employee.Id, periodStart.AddMonths(3), 4, 4);

            company.AddHours(employee2.Id, periodStart, 1, 1);
            company.AddHours(employee2.Id, periodStart.AddMonths(1), 2, 2);
            company.AddHours(employee2.Id, periodStart.AddYears(1).AddMonths(2), 3, 3);
            company.AddHours(employee2.Id, periodStart.AddMonths(3), 4, 4);

            company.GetMonthlyReport(periodStart, periodEnd);
        }
    }
}