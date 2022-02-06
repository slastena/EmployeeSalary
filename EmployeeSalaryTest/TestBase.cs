using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace EmployeeSalaryTest
{
    public class TestBase
    {
        public TestContext TestContext { get; set; }
        protected string _GoodCompanyName;
        protected string _GoodEmployeeFullName;

        protected string _EmptyCompanyName;

        protected void SetGoodCompanyName()
        {
            _GoodCompanyName = TestContext.Properties["GOOD_COMPANY_NAME"].ToString();
        }

         protected void SetGoodEmployeeFullName()
        {
            _GoodEmployeeFullName = TestContext.Properties["GOOD_EMPLOYEE_FULLNAME"].ToString();
        }

        protected void SetEmptyCompanyName()
        {
            _EmptyCompanyName = TestContext.Properties["EMPTY_COMPANY_NAME"].ToString();
        }

        protected void WriteDescription(Type typ)
        {
            string testName = TestContext.TestName;

            MemberInfo method = typ.GetMethod(testName);
            if (method != null)
            {
                Attribute attr = method.GetCustomAttribute(
                   typeof(DescriptionAttribute));
                if (attr != null)
                {
                    DescriptionAttribute dattr = (DescriptionAttribute)attr;
                    TestContext.WriteLine("Test Description: " + dattr.Description);
                }
            }
        }
    }
}