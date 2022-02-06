using System;

namespace EmployeeSalary
{
    public interface ICompany
    {
        /// <summary>
        /// Name of company
        /// </summary>
        string Name { get; }

        /// <summary>
        /// List of employees that are working for the company
        /// </summary>
        Employee[] Employees { get; }

        /// <summary>
        /// Adds new employee from the given date. Employee Id must be unique
        /// </summary>
        /// <param name="employee">Employee to add</param>
        /// <param name="contractStartDate">Employee work start date and time. Can be any date in the past and future</param>
        void AddEmployee(Employee employee, DateTime contractStartDate);

        /// <summary>
        /// Remove employee from the company at the given date
        /// </summary>
        /// <param name="employeeId">Id of the employee</param>
        /// <param name="contractEndDate">Employee work end date and time. Can be any date in the past and future</param>
        void RemoveEmployee(int employeeId, DateTime contractEndDate);

        /// <summary>
        /// Report worked time at given day and time. If employee
        /// report 1 hour and 30 minutes at 13:00 than it means that
        /// employee was working from 13:00 to 14:30
        /// </summary>
        /// <param name="employeeId">Id of the employee</param>
        /// <param name="dateAndTime">Date when work was started.</param>
        /// <param name="hours">Full hours</param>
        /// <param name="minutes">And minutes</param>
        void AddHours(int employeeId, DateTime dateAndTime, int hours, int minutes);

        /// <summary>
        /// Get a full report for each employee where data available
        /// for each month. Salary is duplicated for work done on
        /// the weekends (Saturday, Sunday)
        /// </summary>
        /// <param name="periodStartDate">Report start date</param>
        /// <param name="periodEndDate">Report end date</param>
        MonthlyReportData[] GetMonthlyReport(DateTime periodStartDate, DateTime periodEndDate);
    }
}