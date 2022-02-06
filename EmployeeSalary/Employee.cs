using System;
using System.Linq;

namespace EmployeeSalary
{
    public class Employee
    {
        /// <summary>
        /// Unique ID of the employee
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Employee full name
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Hourly salary of worked full hour. Use proportion for
        /// time smaller than 1 hour.
        /// </summary>
        public decimal HourlySalary { get; private set; }

        public DateTime HiringDate { get; private set; }

        public Employee(string fullName, decimal hourRate)
        {
            FullName = ValidateFullName(fullName);
            HourlySalary = ValidateHourRate(hourRate);
        }

        private static string ValidateFullName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException("An employee must have a non-empty full name");
            }
            return fullName;
        }

        private static decimal ValidateHourRate(decimal hourRate)
        {
            decimal[] invalidHourRates = { decimal.MaxValue, decimal.MinValue, decimal.Zero, decimal.One, decimal.MinusOne };
            if (invalidHourRates.Contains(hourRate))
            {
                throw new ArgumentException("An hour rate must be in the valid range of 1..X");
            }
            return hourRate;
        }

        internal void SetEmployeeId(int id)
        {
            Id = id;
        }

        internal void SetHiringDate(DateTime contractStartDate)
        {
            HiringDate = contractStartDate;
        }

        public override bool Equals(object obj)
        {
            var other = (Employee)obj;
            return Id.Equals(other.Id) && FullName.Equals(other.FullName, StringComparison.CurrentCultureIgnoreCase) && HiringDate.Equals(other.HiringDate);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}