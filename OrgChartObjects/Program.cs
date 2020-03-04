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
        // Companies are now people
        public Employee corporation = new Employee(0,"Draft Kings");

        public void Add(int id, string name, int managerId)
        {                        
            Employee existingEmployee = corporation.Find(id);
            if (existingEmployee is null)
            {
                Employee newEmployee = new Employee(id, name);

                Employee manager = corporation.Find(managerId);
                if (manager is null)
                {
                    // If manager does not exist then add with no manager.
                    this.corporation.Subordinates.Add(newEmployee);
                }
                else
                {
                    newEmployee.Manager = manager;
                    manager.Subordinates.Add(newEmployee);
                }
            }
            else
            {
                return;
            }            
        }

        public void Print()
        {
            foreach (Employee employee in this.corporation.Subordinates)
            {
                employee.Print(indentSpaces:2);
            }
        }

        public void Remove(int employeeId)
        {                        
            Employee existingEmployee = corporation.Find(employeeId);
            if (existingEmployee is null)
            {
                // if employeeId does not exist then ignore.
                return;
            }
            else
            {                                
                if (existingEmployee.Manager is null)
                {
                    // If the employee had no manager, then the subordinates will have no manager as well.
                    foreach (Employee report in existingEmployee.Subordinates)
                    {
                        // If manager does not exist then add with no manager.
                        this.corporation.Subordinates.Add(report);
                    }
                }
                else
                {                    
                    foreach (Employee report in existingEmployee.Subordinates)
                    {
                        // all subordinates should now report to employees manager.
                        existingEmployee.Manager.Subordinates.Add(report);
                    }                        
                }

                corporation.RemoveSubordinate(employeeId);
            }
        }

        public void Move(int employeeId, int newManagerId)
        {
            // if employee does not exist then ignore.
            Employee existingEmployee = corporation.Find(employeeId);
            if (!(existingEmployee is null))            
            {
                Employee existingManager = corporation.Find(newManagerId);

                // if the new managerId does not exist then ignore.
                if (!(existingManager is null))
                {
                    existingEmployee.Subordinates.ForEach(s => existingManager.Subordinates.Add(s));
                }                
            }                
        }

        public int Count(int employeeId)
        {
            Employee foundEmployee = this.corporation.Find(employeeId);

            if (foundEmployee is null)
            {
                return 0;
            }
            else
            {
                return foundEmployee.CountReports(0);
            }            
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public Employee Manager { get; set; }

        public List<Employee> Subordinates { get; set; }

        public Employee(int employeeId, string name)
        {
            this.Id = employeeId;
            this.Name = name;
            this.Subordinates = new List<Employee>();
        }

        public void RemoveSubordinate(int employeeId)
        {
            // Look at subordinates as well.
            foreach (Employee report in this.Subordinates)
            {
                if (report.Id == employeeId)
                {
                    this.Subordinates.Remove(report);
                    return;
                }
                else
                {
                    report.RemoveSubordinate(employeeId);
                }
            }

            return;
        }

        public int CountReports(int currentCount)
        {            
            foreach (Employee report in this.Subordinates)
            {
                currentCount += report.CountReports(currentCount);
            }

            currentCount += this.Subordinates.Count();

            return currentCount;
        }
        public override string ToString()
        {
            return this.Name + " [" + this.Id.ToString() + "]";
        }

        public void Print(int indentSpaces)
        {
            Console.WriteLine(new String(' ', indentSpaces) + this.ToString());
            indentSpaces++;

            this.Subordinates.ForEach(e => e.Print(indentSpaces));
        }

        public Employee Find(int employeeId)
        {
            if (this.Id.Equals(employeeId))
            {
                return this;
            }
            else
            {
                Employee foundEmployee;

                // Look at subordinates as well.
                foreach (Employee report in this.Subordinates)
                {
                    foundEmployee = report.Find(employeeId);

                    if (foundEmployee != null)
                    {
                        return foundEmployee;
                    }
                }             
            }

            return null;
        }
    }
}
