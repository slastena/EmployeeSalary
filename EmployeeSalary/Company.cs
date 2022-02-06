using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeSalary
{
    public class Company : ICompany
    {
        public string Name { get; private set; }

        public Employee[] Employees
        { get { return UniqueEmployees.ToArray(); } }

        private HashSet<Employee> UniqueEmployees { get; set; }

        private List<MonthlyReportData> SalaryReport { get; set; }

        public Company(string companyName)
        {
            Name = ValidateCompanyName(companyName);
            UniqueEmployees = new HashSet<Employee>();
            SalaryReport = new List<MonthlyReportData>();
        }

        private static string ValidateCompanyName(string companyName)
        {
            if (string.IsNullOrEmpty(companyName))
            {
                throw new ArgumentException("A company must have a non-empty name.");
            }
            return companyName;
        }

        public void AddEmployee(Employee employee, DateTime contractStartDate)
        {
            ValidateHiringDate(contractStartDate);
            if (UniqueEmployees.Contains(employee))
            {
                throw new ArgumentException($"An employee with {employee.Id} id already exists in the company {Name}");
            }
            employee.SetEmployeeId(GenerateUniqueEmployeeId());
            employee.SetHiringDate(contractStartDate);
            UniqueEmployees.Add(employee);
        }

        private void ValidateHiringDate(DateTime hiringDate)
        {
            if (hiringDate.Equals(DateTime.MinValue) || hiringDate.Equals(DateTime.MaxValue))
            {
                throw new ArgumentException($"Incorrect date of  {hiringDate}.");
            }
        }

        private int GenerateUniqueEmployeeId()
        {
            if (Employees.Length.Equals(0)) return 1;
            else
            {
                var currentId = Employees.Max((employee) => employee.Id);
                return currentId + 1;
            }
        }

        public void AddHours(int employeeId, DateTime dateAndTime, int hours, int minutes)
        {
            var employee = Employees.FirstOrDefault((employee) => employee.Id.Equals(employeeId));
            if (!UniqueEmployees.Contains(employee))
            {
                throw new ArgumentException($"An employee with {employeeId} id does not exist in the company {Name}");
            }
            ValidateWorkingTimeAgainstHiringDate(employee.HiringDate, dateAndTime);
            ValidateWorkingTime(dateAndTime, hours, minutes);
            if (hours >= 0 && hours < 24 && minutes >= 0 && minutes < 60)
            {
                var workItem = new MonthlyReportData()
                {
                    EmployeeId = employeeId,
                    Year = dateAndTime.Year,
                    Month = (short)dateAndTime.Month,
                    Salary = CalculateSalary(employee.HourlySalary, hours, minutes)
                };
                workItem.Salary = WeekendSalaryAdjustment(dateAndTime, workItem.Salary);
                SalaryReport.Add(workItem);
            }
            else
            {
                throw new ArgumentException($"Reported working hours of {hours} and/or minutes of {minutes} are invalid");
            }
        }

        private static void ValidateWorkingTimeAgainstHiringDate(DateTime hiringDate, DateTime workingDateAndTime)
        {
            if (workingDateAndTime < hiringDate)
            {
                throw new ArgumentException($"An employee cannot start work before the hiring date");
            }
        }

        private static void ValidateWorkingTime(DateTime dateAndTime, int hours, int minutes)
        {
            var today = dateAndTime.DayOfYear;
            var addedUp = dateAndTime.AddMinutes(minutes).AddHours(hours);
            if (addedUp.DayOfYear > today)
            {
                throw new ArgumentException($"Reported working hours of {hours} and/or minutes of {minutes} exceed the remaining time for the current day {dateAndTime}");
            }
        }

        private static decimal CalculateSalary(decimal hourRate, int hours, int minutes)
        {
            var minuteRate = hourRate / 60;
            return hourRate * hours + minuteRate * minutes;
        }

        private static decimal WeekendSalaryAdjustment(DateTime dateAndTime, decimal salary)
        {
            if (dateAndTime.DayOfWeek == DayOfWeek.Sunday || dateAndTime.DayOfWeek == DayOfWeek.Saturday)
            {
                return salary * 2;
            }
            return salary;
        }

        public MonthlyReportData[] GetMonthlyReport(DateTime periodStartDate, DateTime periodEndDate)
        {
            ValidateHiringDate(periodStartDate);
            ValidateHiringDate(periodEndDate);
            if (periodEndDate <= periodStartDate || (periodStartDate > DateTime.Now) || periodEndDate > DateTime.Now)
            {
                throw new ArgumentException($"Incorrect monthly report period selected.");
            }

            var report = SalaryReport.GroupBy(record => new { record.EmployeeId, record.Year, record.Month })
                .OrderBy(x => x.Key.EmployeeId)
                .ThenBy(x => x.Key.Year)
                .ThenBy(y => y.Key.Month)
                .Select(z => new MonthlyReportData()
                {
                    EmployeeId = z.Key.EmployeeId,
                    Year = z.Key.Year,
                    Month = z.Key.Month,
                    Salary = z.Sum(i => i.Salary)
                }).ToList();

            PrintMonthlyReport(report);

            return report.ToArray();
        }

        private void PrintMonthlyReport(List<MonthlyReportData> monthlyReportDatas)
        {
            Console.WriteLine("Full salary report for each company employee per month.\n");
            monthlyReportDatas.ForEach(Console.WriteLine);
        }

        public void RemoveEmployee(int employeeId, DateTime contractEndDate)
        {
            ValidateHiringDate(contractEndDate);
            var employee = Employees.FirstOrDefault((employee) => employee.Id.Equals(employeeId));
            if (UniqueEmployees.Contains(employee))
            {
                if (contractEndDate >= employee.HiringDate)
                {
                    UniqueEmployees.Remove(employee);
                    CleanUpSalaryReportOnEmployeeRemoval(employee, contractEndDate);
                }
                else
                {
                    throw new ArgumentException($"Employee hire end date cannot be before employee hire start date");
                }
            }
            else
            {
                throw new ArgumentException($"An employee with {employeeId} id does not exist in the company {Name}");
            }
        }

        private void CleanUpSalaryReportOnEmployeeRemoval(Employee employee, DateTime contractEnd)
        {
            SalaryReport.RemoveAll(report => report.EmployeeId.Equals(employee.Id) && report.Year.Equals(contractEnd.Year) && report.Month.Equals((short)contractEnd.Month));
        }
    }
}