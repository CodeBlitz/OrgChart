using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgChart
{
    class Program
    {
        static void Main(string[] args)
        {
            var orgChart = new OrgChart();

            orgChart.Add(10, "Sharilyn Gruber", -1);
            orgChart.Add(7, "Denice Mattice", 10);
            orgChart.Add(3, "Lawana Futrell", -1);
            orgChart.Add(34, "Lissette Gorney", 7);
            orgChart.Add(5, "Lan Puls", 3);
            //orgChart.Move(3, 10);
            //orgChart.Remove(7);
            orgChart.Print();
            Console.WriteLine(orgChart.Count(10));

            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }

    public class OrgChart
    {
        private List<Employee> employees = new List<Employee>();

        public void Add(int id, string name, int managerId)
        {
            if (!this.employees.Exists(employee => employee.id == id))
            {
                var addedEmployee = new Employee() { id = id, name = name };

                if (this.employees.Exists(manager => manager.id == managerId))
                {
                    addedEmployee.managerId = managerId;
                }
                
                this.employees.Add(addedEmployee);
            }
        }

        public void Print()
        {
            var topEmployees = this.employees.FindAll(e => e.managerId.HasValue == false);

            foreach (Employee employee in topEmployees)
            {
                Console.WriteLine(employee.ToString());
                PrintDirectReports(managerId: employee.id, indentCount: 1);
            }
        }

        public void PrintDirectReports(int managerId, int indentCount)
        {
            indentCount++;

            var directReports = this.employees.FindAll(employee => employee.managerId == managerId);

            foreach (var employee in directReports)
            {
                Console.WriteLine(new String(' ', indentCount) + employee.ToString());
                PrintDirectReports(managerId: employee.id, indentCount: indentCount);
            }            
        }

        public void Remove(int employeeId)
        {
            if (employees.Exists(employee => employee.id == employeeId))
            {
                var subordinateEmployees = employees.FindAll(employee => employee.managerId == employeeId);
                
                var removingEmployee = employees.Find(employee => employee.id == employeeId);

                if (removingEmployee.managerId.HasValue)
                {
                    subordinateEmployees.ForEach(subordinate => subordinate.managerId = removingEmployee.managerId);
                }
                else
                {
                    subordinateEmployees.ForEach(subordinate => subordinate.managerId = null);
                }
            }
        }

        public void Move(int employeeId, int newManagerId)
        {
            if (employees.Exists(e => e.id == employeeId) && employees.Exists(m => m.managerId == newManagerId))
            {
                var foundEmployee = employees.First(e => e.id == employeeId);
                {
                    foundEmployee.managerId = newManagerId;
                }                
            }
        }

        public int Count(int employeeId)
        {
            int totalCount = 0;

            if (this.employees.Exists(employee => employee.id == employeeId))
            {
                var reports = this.employees.FindAll(r => r.managerId == employeeId);

                totalCount += reports.Count();

                foreach (var report in reports)
                {
                    totalCount += Count(report.id);
                }
            }

            return totalCount;
        }
    }

    public struct Employee
    {
        public string name;
        public int id;
        public Nullable<int> managerId;

        public override string ToString()
        {
            return $"{this.name} [{this.id}]";
        }
    }
}
