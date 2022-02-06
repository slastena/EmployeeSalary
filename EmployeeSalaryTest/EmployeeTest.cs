using EmployeeSalary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace EmployeeSalaryTest
{
    [TestClass]
    public class EmployeeTest : TestBase
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
            WriteDescription(GetType());
            if (TestContext.TestName.Contains("EmployeeFullNameNonEmpty"))
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

        protected static IEnumerable<object[]> ValidHourRates =>
            new[] {
                new object[] { 1.1m, 1.1m },
                new object[] { 13.5m, 13.5m }
            };

        protected static IEnumerable<object[]> InvalidHourRates =>
           new[] {
                new object[] { decimal.MinusOne},
                new object[] { decimal.MaxValue },
                new object[] { decimal.MinValue },
                new object[] { decimal.One },
                new object[] { decimal.Zero },
           };

        [TestMethod]
        [Description("Check an employee must have a non-empty full name and a valid hour rate")]
        [Owner("Oz")]
        [DynamicData(nameof(ValidHourRates))]
        public void EmployeeFullNameNonEmptyValidHourRate(decimal hourRate, decimal expectedHourRate)
        {
            Employee employee = new Employee(_GoodEmployeeFullName, hourRate);

            TestContext.WriteLine($"Checking an employee can be created with non-empty full name {_GoodEmployeeFullName}");

            Assert.IsNotNull(employee.FullName);
            Assert.AreEqual(expectedHourRate, employee.HourlySalary);
        }

        [TestMethod]
        [Description("Check throw ArgumentException for an employee null full name and a valid hour rate using ExpectedException")]
        [Owner("Oz")]
        [DynamicData(nameof(ValidHourRates))]
        [ExpectedException(typeof(ArgumentException))]
        public void EmployeeFullNameNullValidRate(decimal hourRate, decimal expectedHourRate)
        {
            TestContext.WriteLine($"Checking an employee cannot be created with a null full name");
            new Employee(null, hourRate);
        }

        [TestMethod]
        [Description("Check throw ArgumentException for invalid hour rates for an employee using ExpectedException")]
        [Owner("Oz")]
        [ExpectedException(typeof(ArgumentException))]
        [DynamicData(nameof(InvalidHourRates))]
        public void EmployeeInvalidHourRate_UsingAttribute(decimal invalidHourRate)
        {
            TestContext.WriteLine($"Checking an exception is thrown for invalid hour rate {invalidHourRate}");
            new Employee(_GoodEmployeeFullName, invalidHourRate);
        }
    }
}