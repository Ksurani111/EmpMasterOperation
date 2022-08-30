namespace TestApp.Library
{
    public class SalaryCalculation
    {
        public virtual int getSalary(int Salary)
        {
            return Salary;
        }
    }
    public class BasicSalary : SalaryCalculation
    {
        public override int getSalary(int Salary)
        {
            return base.getSalary(Salary) * 40 / 100;
        }
    }
    public class PFSalary : SalaryCalculation
    {
        public override int getSalary(int Salary)
        {
            return (base.getSalary(Salary) * 40 / 100) * 12 / 100;
        }
    }
}
