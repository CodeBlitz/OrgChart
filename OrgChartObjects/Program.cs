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
