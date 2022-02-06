using EmployeeSalary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace EmployeeSalaryTest
{
    [TestClass]
    public class CompanyTest : TestBase
    {
        #region Class Initialize and Cleanup Methods

        [ClassInitialize()]
        public static void ClassInitialize(TestContext tc)
        {
            tc.WriteLine("ClassInitialize method");
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
        }

        #endregion Class Initialize and Cleanup Methods

        #region Test Initialize Method

        [TestInitialize()]
        public void TestInitialize()
        {
            TestContext.WriteLine("TestInitialize");
            WriteDescription(this.GetType());
            if (TestContext.TestName.Contains("Company") && !TestContext.TestName.Contains("CompanyNameNullOrEmpty"))
            {
                SetGoodCompanyName();
            }
            if (TestContext.TestName.Contains("Employee"))
            {
                SetGoodEmployeeFullName();
            }
        }

        #endregion Test Initialize Method

        #region Test Cleanup Method

        [TestCleanup()]
        public void TestCleanup()
        {
            TestContext.WriteLine("TestCleanup");
        }

        #endregion Test Cleanup Method

        protected static IEnumerable<object[]> InvalidHiringDateValues =>
            new[] {
                new object[] { DateTime.MinValue },
                new object[] { DateTime.MinValue }
            };

        [TestMethod]
        [Description("Check the company can be created with a non-empty company name")]
        [Owner("Oz")]
        public void CompanyNameNonEmpty()
        {
            Company company = new Company(_GoodCompanyName);

            TestContext.WriteLine($"Checking company can be created with non-empty company name {_GoodCompanyName}");

            Assert.IsNotNull(company.Name);
        }

        [TestMethod]
        [Description("Check for thrown ArgumentException when a company cannot be created without a company name using ExpectedException")]
        [Owner("Oz")]
        [DataRow(null, DisplayName = "First test(null)")]
        [DataRow("", DisplayName = "First test(\"\")")]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("Exception")]
        public void CompanyNameNullOrEmpty(string companyName)
        {
            TestContext.WriteLine("Checking company cannot be created with a null as a company name");

            new Company(companyName);
        }

        [TestMethod]
        [Owner("Oz")]
        public void CompanyHasNoEmployeesByDefault()
        {
            Company company = new Company(_GoodCompanyName);
            Assert.AreEqual(company.Employees.Length, 0);
        }

        [TestMethod]
        [Owner("Oz")]
        public void AddUniqueEmployeeToCompany()
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.1m);
            company.AddEmployee(employee, DateTime.Now.Date);
            Assert.AreEqual(company.Employees.Length, 1);
        }

        [TestMethod]
        [Owner("Oz")]
        [DynamicData(nameof(InvalidHiringDateValues))]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("Exception")]
        public void AddUniqueEmployeeToCompanyInvalidHiringDate(DateTime invalidHiringDate)
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.1m);
            company.AddEmployee(employee, invalidHiringDate);
        }

        [TestMethod]
        [Owner("Oz")]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("Exception")]
        public void AddNonUniqueEmployeeToCompany()
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.1m);
            company.AddEmployee(employee, DateTime.Now.Date);
            company.AddEmployee(employee, DateTime.Now.Date);
        }

        [TestMethod]
        [Owner("Oz")]
        public void AddTwoDifferentEmployeesToCompany()
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee1 = new Employee(_GoodEmployeeFullName, 1.2m);
            Employee employee2 = new Employee("Michael Enlin", 1.1m);
            company.AddEmployee(employee1, DateTime.Now.Date);
            company.AddEmployee(employee2, DateTime.Now.Date);
            Assert.AreEqual(2, company.Employees.Length);
        }

        [TestMethod]
        [Owner("Oz")]
        public void RemoveExistingEmployeeFromCompany()
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.1m);
            company.AddEmployee(employee, DateTime.Now.Date);
            company.RemoveEmployee(employee.Id, DateTime.Now.Date);
            Assert.AreEqual(0, company.Employees.Length);
        }

        [TestMethod]
        [Owner("Oz")]
        [DynamicData(nameof(InvalidHiringDateValues))]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("Exception")]
        public void RemoveExistingEmployeeFromCompanyInvalidHiringEndDate(DateTime invalidHiringEndDate)
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.1m);
            company.AddEmployee(employee, DateTime.Now.Date);
            company.RemoveEmployee(employee.Id, invalidHiringEndDate);
        }

        [TestMethod]
        [Owner("Oz")]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("Exception")]
        public void RemoveExistingEmployeeFromCompanyHiringEndDateBeforeHireStartDate()
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.1m);
            var hireStartDate = DateTime.Now;
            company.AddEmployee(employee, hireStartDate);

            company.RemoveEmployee(employee.Id, hireStartDate.AddDays(-1));
        }

        [TestMethod]
        [Owner("Oz")]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("Exception")]
        public void RemoveNonExistingEmployeeFromCompany()
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.1m);
            company.RemoveEmployee(employee.Id, DateTime.Now.Date);
        }

        [TestMethod]
        [Owner("Oz")]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("Exception")]
        public void AddHoursToNonExistingCompanyEmployee()
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.1m);
            company.AddHours(employee.Id, DateTime.Now.Date, 1, 1);
        }

        [TestMethod]
        [Owner("Oz")]
        [DataRow(24, 60, DisplayName = "First test(25,60)")]
        [DataRow(-1, -1, DisplayName = "Second test(-1,-1)")]
        [DataRow(0, -1, DisplayName = "Third test(0,-1)")]
        [DataRow(-1, 0, DisplayName = "Fourth test(-1,0)")]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("Exception")]
        public void AddInvalidHoursMinutesToExisitingCompanyEmployee(int hours, int minutes)
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.1m);
            company.AddEmployee(employee, DateTime.Now.Date);

            company.AddHours(employee.Id, DateTime.Now.Date, hours, minutes);
        }

        [TestMethod]
        [Owner("Oz")]
        [DataRow(0, 1, DisplayName = "First test(0,1)")]
        [DataRow(1, 0, DisplayName = "Second test(1,0)")]
        [DataRow(1, 1, DisplayName = "Third test(1,1)")]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("Exception")]
        public void AddMoreWorkingTimeThanRemainedOfTheDayEndToExistingCompanyEmployeeEndOfDay(int hours, int minutes)
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.1m);
            var today = DateTime.Now;
            var workStartDateAndTime = new DateTime(today.Year, today.Month, 1, 23, 59, 59);
            company.AddEmployee(employee, today);

            company.AddHours(employee.Id, workStartDateAndTime, hours, minutes);
        }

        [TestMethod]
        [Owner("Oz")]
        [DataRow(23, 59, DisplayName = "First test(23,59)")]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("Exception")]
        public void AddMoreWorkingTimeThanRemainedOfTheDayEndToExistingCompanyEmployeeStartOfDay(int hours, int minutes)
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.1m);
            var today = DateTime.Now;
            var workStartDateAndTime = new DateTime(today.Year, today.Month, 1, 0, 1, 0);
            company.AddEmployee(employee, today);

            company.AddHours(employee.Id, workStartDateAndTime, hours, minutes);
        }

        [TestMethod]
        [Owner("Oz")]
        [DataRow(1, 1, DisplayName = "First test(1,1)")]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("Exception")]
        public void AddWorkingTimeBeforeHiringDateToExistingCompanyEmployee(int hours, int minutes)
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.1m);
            var hiringDate = DateTime.Now;
            var workingDate = hiringDate.AddDays(-1);
            var workStartDateAndTime = new DateTime(workingDate.Year, workingDate.Month, workingDate.Day, 23, 0, 0);
            company.AddEmployee(employee, hiringDate);

            company.AddHours(employee.Id, workStartDateAndTime, hours, minutes);
        }

        [TestMethod]
        [Owner("Oz")]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("Exception")]
        public void GetFullSalaryReportForCompanyEmployeesInvalidReportPeriod()
        {
            Company company = new Company(_GoodCompanyName);

            var periodStart = DateTime.MinValue;
            var periodEnd = DateTime.MaxValue;

            company.GetMonthlyReport(periodEnd, periodStart);
        }

         [TestMethod]
        [Owner("Oz")]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("Exception")]
        public void GetFullSalaryReportForCompanyEmployeesReportPeriodInFuture()
        {
            Company company = new Company(_GoodCompanyName);

            var periodStart = DateTime.Now.AddDays(1);
            var periodEnd = periodStart.AddMonths(1);

            company.GetMonthlyReport(periodStart, periodEnd);
        }

        [TestMethod]
        [Owner("Oz")]
        public void GetFullSalaryReportForCompanyWithNoEmployees()
        {
            Company company = new Company(_GoodCompanyName);
            var periodStart = DateTime.Now.AddYears(-1);
            var periodEnd = DateTime.Now;

            var result = company.GetMonthlyReport(periodStart, periodEnd);
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        [Owner("Oz")]
        [ExpectedException(typeof(ArgumentException))]
        [TestCategory("Exception")]
        public void GetFullSalaryReportForCompanyZeroPeriod()
        {
            Company company = new Company(_GoodCompanyName);
            var periodStart = DateTime.Now.AddYears(-1);
            var periodEnd = periodStart;

            company.GetMonthlyReport(periodStart, periodEnd);
        }

        [TestMethod]
        [Owner("Oz")]
        public void GetFullSalaryReportForCompanyEmployee()
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.2m);
            var hiringDate = new DateTime(2021, 1, 1);
            company.AddEmployee(employee, hiringDate);

            var periodStart = new DateTime(hiringDate.Year, hiringDate.Month, 1, 0, 1, 0);
            var periodEnd = DateTime.Now;

            var expectedResult = new MonthlyReportData[]
            {
                new MonthlyReportData(){
                EmployeeId = 1,
                Year = periodStart.Year,
                Month = (short)periodStart.Month,
                Salary = employee.HourlySalary * 1 + employee.HourlySalary / 60 * 1
            },
            new MonthlyReportData(){
                EmployeeId = 1,
                Year = periodStart.Year,
                Month = (short)periodStart.AddMonths(1).Month,
                Salary = employee.HourlySalary * 2 + employee.HourlySalary / 60 * 2
            },
            new MonthlyReportData(){
                EmployeeId = 1,
                Year = periodStart.Year,
                Month = (short)periodStart.AddMonths(2).Month,
                Salary = employee.HourlySalary * 3 + employee.HourlySalary / 60 * 3
            },
            new MonthlyReportData(){
                EmployeeId = 1,
                Year = periodStart.Year,
                Month = (short)periodStart.AddMonths(3).Month,
                Salary = employee.HourlySalary * 4 + employee.HourlySalary / 60 * 4
            }
        };

            company.AddHours(employee.Id, periodStart, 1, 1);
            company.AddHours(employee.Id, periodStart.AddMonths(1), 2, 2);
            company.AddHours(employee.Id, periodStart.AddMonths(2), 3, 3);
            company.AddHours(employee.Id, periodStart.AddMonths(3), 4, 4);

            var result = company.GetMonthlyReport(periodStart, periodEnd);

            Assert.AreEqual(result.Length, 4);
            CollectionAssert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [Owner("Oz")]
        public void GetFullSalaryReportForCompanyEmployeeSalaryDoubledOnWeekend()
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.2m);
            var hiringDate = new DateTime(2021, 1, 1);
            company.AddEmployee(employee, hiringDate);

            var periodStart = new DateTime(hiringDate.Year, hiringDate.Month, 2, 0, 1, 0);
            var periodEnd = DateTime.Now;

            var expectedResult = new MonthlyReportData[]
            {
                new MonthlyReportData(){
                EmployeeId = 1,
                Year = periodStart.Year,
                Month = (short)periodStart.Month,
                Salary = (employee.HourlySalary * 1 + employee.HourlySalary / 60 * 1) * 2 + (employee.HourlySalary * 2 + employee.HourlySalary / 60 * 2) * 2
                }
            };

            company.AddHours(employee.Id, periodStart, 1, 1); //Saturday
            company.AddHours(employee.Id, periodStart.AddDays(1), 2, 2); //Sunday

            var result = company.GetMonthlyReport(periodStart, periodEnd);

            Assert.AreEqual(result.Length, 1);
            CollectionAssert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [Owner("Oz")]
        public void GetFullSalaryReportForCompanyEmployeeAfterEmployeeHasBeenRemovedHireEndEqualsPeriodEnd()
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.2m);
            var hiringDate = new DateTime(2021, 1, 1);
            company.AddEmployee(employee, hiringDate);

            var periodStart = new DateTime(hiringDate.Year, hiringDate.Month, 2, 0, 1, 0);
            var periodEnd = DateTime.Now;
            var expectedResult = new MonthlyReportData[]
            {
                new MonthlyReportData(){
                EmployeeId = 1,
                Year = periodStart.Year,
                Month = (short)periodStart.Month,
                Salary = (employee.HourlySalary * 1 + employee.HourlySalary / 60 * 1) * 2 + (employee.HourlySalary * 2 + employee.HourlySalary / 60 * 2) * 2
                }
            };

            company.AddHours(employee.Id, periodStart, 1, 1); //Saturday
            company.AddHours(employee.Id, periodStart.AddDays(1), 2, 2); //Sunday

            company.RemoveEmployee(employee.Id, periodEnd);

            var result = company.GetMonthlyReport(periodStart, periodEnd);

            Assert.AreEqual(1, result.Length);
            CollectionAssert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [Owner("Oz")]
        public void GetFullSalaryReportForCompanyEmployeeAfterEmployeeHasBeenRemovedHireEndEqualsPeriodStart()
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.2m);
            var hiringDate = new DateTime(2021, 1, 1);
            company.AddEmployee(employee, hiringDate);

            var periodStart = new DateTime(hiringDate.Year, hiringDate.Month, 2, 0, 1, 0);
            var periodEnd = DateTime.Now;
           
            company.AddHours(employee.Id, periodStart, 1, 1); //Saturday
            company.AddHours(employee.Id, periodStart.AddDays(1), 2, 2); //Sunday

            company.RemoveEmployee(employee.Id, periodStart);

            var result = company.GetMonthlyReport(periodStart, periodEnd);

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        [Owner("Oz")]
        public void GetFullSalaryReportForCompanyEmployeeAfterEmployeeHasBeenRemovedHireEndBeforePeriodStart()
        {
            Company company = new Company(_GoodCompanyName);
            Employee employee = new Employee(_GoodEmployeeFullName, 1.2m);
            var hiringDate = new DateTime(2021, 1, 1);
            company.AddEmployee(employee, hiringDate);

            var periodStart = new DateTime(hiringDate.Year, hiringDate.Month, 2, 0, 1, 0);
            var periodEnd = DateTime.Now;

            company.AddHours(employee.Id, periodStart, 1, 1); //Saturday
            company.AddHours(employee.Id, periodStart.AddDays(1), 2, 2); //Sunday

            company.RemoveEmployee(employee.Id, hiringDate);

            var result = company.GetMonthlyReport(periodStart, periodEnd);
            Assert.AreEqual(0, result.Length);
        }
    }
}