using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ORGANIZATIONAL Structure
 * This is a program to manage the Organizational Structure in a company.
 * The program should allow us to:
 * 1) Add employees
 * 2) Generate the Org Chart in a specific format.
 * 3) Remove employees
 * 4) Move an employee from on manager to another
 * 5) Count the number of employees that ultimately report up to the given manager.
 * 
 * The input for the program is a text file comprising of a series of commands and parameters that may look like this:
 * 
 * 6 <== Number of linesin the input data to process.
 * add, 10, Sharilyn Gruber, -1  <== Add Sharilyn Gruber as an employee with ID 10, with no manager.
 * add, 7, Denice Mattice, 10  <== Add Sharilyn Gruber as an employee with ID 10, with no manager.
 * add, 3, Lawana Futrell, -1  <== Add Sharilyn Gruber as an employee with ID 10, with no manager.
 * add, 34, Lissette Gorney, 7  <== Add Sharilyn Gruber as an employee with ID 10, with no manager.
 * add, 5, Lan Puls, 3  <== Add Sharilyn Gruber as an employee with ID 10, with no manager.
 * Print  <== Print the Org Chart as specified below.
 * 
 * The output of the program, when fed the above as input, will be
 *
 * Sharlyn Gruber [10]
 *  Denice Mattice [7]
 *   Lissette Gorney [34]
 * Lawana Futrell [3]
 *  Lan Puls [5]
 *
 * Each employee is printed as a Name [employee id] - There is a space between the name and ID.
 * At each reporting level, add 2 spaces precedding the name.
 * 
 * Employees are always displayed in the order they are processed.
 * 
 * Adding and Employee:
 * The command is of the format
 * add, <employee id>, <name>, <manager id>
 * 
 * If an employee with an id has been added, then subsequent additions of employees with the same id are ignored.
 * If an employee's manager has not already been added, then the employee is considered to have no manager.
 * 
 * Moving and Employee:
 * The command is of the format
 * move, <employee id>, <new manager id>
 * 
 * If an employee of that id does not exist, then do nothing.
 * If an employee with the new manager id does not exist, then do nothing.
 * The newly moved employee will be appended to the manager's list of reports, when the org chart is printed.
 * Subsequent adds will continue to be added to the end of the list of reports.
 * 
 * Removing an Employee:
 * The command is of the format
 * remove, <employee id>
 * 
 * If an employee of that id does not exist, then do nothing.
 * After removal, if the employee had any reports, they should now report to the removed employee's manager.
 * These employees will show up in the same order as they did under thier former manager, but after any current reports of the new manager.
 * If the removed employee had no manager, then the reports will have no manager either.
 * 
 * Counting Employees:
 * The command is of the format
 * count, <employee id>
 * This should return the total number of reports, including any indirect reports (all descendant reports), that the specified employee has.
 */

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
