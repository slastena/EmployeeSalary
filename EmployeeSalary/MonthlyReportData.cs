using System.Text;

namespace EmployeeSalary
{
    public class MonthlyReportData
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }

        /// <summary>
        /// The number of the month from 1 to 12
        /// </summary>
        public short Month { get; set; }

        public decimal Salary { get; set; }

        public override bool Equals(object obj)
        {
            var other = (MonthlyReportData)obj;
            return EmployeeId.Equals(other.EmployeeId) && Year.Equals(other.Year) && Month.Equals(other.Month) && Salary.Equals(other.Salary);
        }

        public override int GetHashCode()
        {
            return EmployeeId.GetHashCode();
        }

        public override string ToString()
        {
            var properties = GetType().GetProperties();
            var sb = new StringBuilder();
            foreach (var property in properties)
            {
                sb.Append($"{property.Name}: {property.GetValue(this)} ");
            }
            return sb.ToString();
        }
    }
}