using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmployeeSalaryTest
{
    [TestClass]
    public class EmployeeSalaryTestInitialization
    {
        [AssemblyInitialize()]
        public static void AssemblyInitialize(TestContext tc)
        {
            // TODO: Initialize for all tests within an assembly
            tc.WriteLine("AssemblyInitialize");
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            // TODO: Clean up after all
            // tests in an assembly
        }
    }
}