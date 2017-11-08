namespace P02_DatabaseFirst
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using P02_DatabaseFirst.Data;
    using P02_DatabaseFirst.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            /* Pr. 3
             * Your first task is to extract all employees and print their 
             * first, last and middle name, their job title and salary, 
             * rounded to 2 symbols after the decimal separator, all of those separated with a space. 
             * Order them by employee id. */

            // EmployeesFullInformation();

            /*Pr. 4
             * Your task is to extract all employees with salary over 50000. 
             * Return only the first names of those employees, ordered alphabetically.*/

            // EmployeeswithSalaryOver50000();

            /*Pr. 5
             * Extract all employees from the Research and Development department. 
             * Order them by salary (in ascending order), then by first name (in descending order). 
             * Print only their first name, last name, department name and salary */

            // ResearchandDevelopment();

            /*Pr. 6
             * Create a new address with text "Vitoshka 15" and TownId 4. 
             * Set that address to the employee with last name "Nakov".
             * Then order by descending all the employees by their Address’ Id, take 10 rows and from them, 
             * take the AddressText. Print the results each on a new line */

            // AddingaNewAddressandUpdatingEmployee();

            /*Pr. 7
             * Find the first 30 employees who have projects started in the period 2001 - 2003 (inclusive). 
             * Print each employee's first name, last name, manager’s first name and last name. 
             * Then print all of their projects in the format "--<ProjectName> - <StartDate> - <EndDate>", 
             * each on a new row. If a project has no end date, print "not finished" instead. */

            // EmployeesAndProjects();

            /*Pr. 8
             * Find all addresses, ordered by the number of employees who live there (descending), 
             * then by town name (ascending), and finally by address text (ascending). 
             * Take only the first 10 addresses. For each address print it in the format 
             * "<AddressText>, <TownName> - <EmployeeCount> employees": */

            // AddressByTown();

            /*Pr. 9
             * Get the employee with id 147. Print only his/her first name, last name, job title and 
             * projects (print only their names). The projects should be ordered by name (ascending). */

            // Employee147();

            /*Pr. 10
             * Find all departments with more than 5 employees. Order them by employee count (ascending), 
             * then by department name (alphabetically). 
             * For each department, print the department name and the manager’s first and last name 
             * on the first row. Then print the first name, the last name and the job title of every employee 
             * on a new row. Then, print 10 dashes before the next department ("----------"). 
             * Order the employees by first name (ascending), then by last name (ascending). */

            // DepartmentsWithMoreThan5Employees();

            /*Pr. 11
             * Write a program that prints information about the last 10 started projects. 
             * Sort them by name lexicographically and print their name, 
             * description and start date, each on a new row. */

            // FindLatest10Projects();

            /*Pr. 12
             * Write a program that increase salaries of all employees that are in the Engineering, 
             * Tool Design, Marketing or Information Services department by 12%. 
             * Then print first name, last name and salary (2 symbols after the decimal separator) 
             * for those employees whose salary was increased. 
             * Order them by first name (ascending), then by last name (ascending). */

            // IncreaseSalaries();

            /*Pr. 13
             * Write a program that finds all employees whose first name starts with "Sa". 
             * Print their first, last name, their job title and salary in the format given in the example below. 
             * Order them by first name, then by last name (ascending).
             * *Note: You have to solve previous task in order to display proper results. */

            // FindEmployeesByFirstNameStartinWithSa();

            /*Pr. 14 
             * Let's delete the project with id 2. Then, take 10 projects and print their names on the console, 
             * each on a new line. Remember to restore your database after this task. */

            // DeleteProjectById();

            /*Pr. 15
             * Write a program that deletes a town by its name, given as an input. 
             * Also, delete all addresses that are in those towns. Print on the console the number of addresses 
             * that were deleted. There will be employees living at those addresses, which will be a problem 
             * when trying to delete the addresses. So, start by setting the AddressID of each employee 
             * for the given address to null. After all of them are set to null, you may safely remove 
             * all the addresses from the context.Addresses and finally remove the given town. */

            // RemoveTowns();
        }

        private static void RemoveTowns()
        {
            using (var db = new SoftUniContext())
            {
                string townName = Console.ReadLine();
                var townToRemove = db.Towns.SingleOrDefault(t => t.Name == townName);
                var addressesInTown = db.Addresses
                    .Include(a => a.Town)
                    .Include(a => a.Employees)
                    .Where(a => a.Town.Name == townName)
                    .ToList();

                int addressesCount = addressesInTown.Count();

                foreach (var addr in addressesInTown)
                {
                    var employeesOnThisAddress = addr.Employees
                        .ToList();

                    foreach (var empl in employeesOnThisAddress)
                    {
                        empl.AddressId = null;
                    }

                    db.Addresses.Remove(addr);
                }

                db.Towns.Remove(townToRemove);
                db.SaveChanges();

                string addressPluralization = addressesCount == 1 ? "address" : "addresses";
                string toBePluralization = addressesCount == 1 ? "was" : "were";

                Console.WriteLine($"{addressesCount} {addressPluralization} in {townName} {toBePluralization} deleted");
            }
        }

        private static void DeleteProjectById()
        {
            using (var db = new SoftUniContext())
            {
                const int ProjectToRemoveId = 2;
                var project = db.Projects.SingleOrDefault(p => p.ProjectId == ProjectToRemoveId);

                var projEmployees = db.EmployeesProjects
                    .Where(ep => ep.ProjectId == ProjectToRemoveId)
                    .ToList();

                db.EmployeesProjects.RemoveRange(projEmployees);
                db.Projects.Remove(project);

                db.SaveChanges();

                var projects = db.Projects
                    .Take(10)
                    .ToList();

                foreach (var p in projects)
                {
                    Console.WriteLine(p.Name);
                }
            }
        }

        private static void FindEmployeesByFirstNameStartinWithSa()
        {
            using (var db = new SoftUniContext())
            {
                var employeesSa = db.Employees
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle,
                        e.Salary
                    })
                    .Where(e => e.FirstName.StartsWith("Sa"))
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToList();

                foreach (var e in employeesSa)
                {
                    Console.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
                }
            }
        }

        private static void IncreaseSalaries()
        {
            using (var db = new SoftUniContext())
            {
                var promotedLuckyBastards = db.Employees
                    .Include(b => b.Department)
                    .Where(b => b.Department.Name == "Engineering" ||
                                b.Department.Name == "Tool Design" ||
                                b.Department.Name == "Marketing" ||
                                b.Department.Name == "Information Services")
                    .ToList();

                for (int i = 0; i < promotedLuckyBastards.Count; i++)
                {
                    var currentSonOfAGun = promotedLuckyBastards[i];
                    currentSonOfAGun.Salary *= 1.12m;
                }

                db.SaveChanges();

                foreach (var b in promotedLuckyBastards.OrderBy(b => b.FirstName).ThenBy(b => b.LastName))
                {
                    Console.WriteLine($"{b.FirstName} {b.LastName} (${b.Salary:f2})");
                }
            }
        }

        private static void FindLatest10Projects()
        {
            using (var db = new SoftUniContext())
            {
                string datetimeFormat = "M/d/yyyy h:mm:ss tt";

                var projects = db.Projects
                    .OrderByDescending(p => p.StartDate)
                    .Take(10)
                    .OrderBy(p => p.Name)
                    .ToList();

                foreach (var p in projects)
                {
                    var date = p.StartDate.ToString(datetimeFormat, CultureInfo.InvariantCulture);

                    Console.WriteLine(p.Name + Environment.NewLine + p.Description + Environment.NewLine + date);
                }
            }
        }

        private static void DepartmentsWithMoreThan5Employees()
        {
            using (var db = new SoftUniContext())
            {
                var departments = db.Departments
                    .Include(d => d.Employees)
                    .Where(d => d.Employees.Count > 5)
                    .OrderBy(d => d.Employees.Count)
                    .ThenBy(d => d.Name)
                    .ToList();

                foreach (var dept in departments)
                {
                    Console.WriteLine($"{dept.Name} - {dept.Manager.FirstName} {dept.Manager.LastName}");

                    foreach (var e in dept.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
                    {
                        Console.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                    }

                    Console.WriteLine("----------");
                }
            }
        }

        private static void Employee147()
        {
            using (var db = new SoftUniContext())
            {
                var employee = db.Employees
                    .Include(e => e.EmployeesProjects)
                    .ThenInclude(ep => ep.Project)
                    .Select(e => new
                    {
                        e.EmployeeId,
                        e.FirstName,
                        e.LastName,
                        e.JobTitle,
                        e.EmployeesProjects
                    })
                    .FirstOrDefault(e => e.EmployeeId == 147);

                Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                foreach (var proj in employee.EmployeesProjects.Select(ep => ep.Project).OrderBy(p => p.Name))
                {
                    Console.WriteLine(proj.Name);
                }
            }
        }

        private static void AddressByTown()
        {
            using (var db = new SoftUniContext())
            {
                var addresses = db.Addresses
                    .Include(a => a.Employees)
                    .Include(e => e.Town)
                    .OrderByDescending(a => a.Employees.Count)
                    .ThenBy(a => a.Town.Name)
                    .ThenBy(a => a.AddressText)
                    .Take(10)
                    .ToList();

                foreach (var a in addresses)
                {
                    Console.WriteLine($"{a.AddressText}, {a.Town.Name} - {a.Employees.Count} employees");
                }
            }
        }

        private static void EmployeesAndProjects()
        {
            using (var db = new SoftUniContext())
            {
                string datetimeFormat = "M/d/yyyy h:mm:ss tt";
                DateTime projectTimeframeStart = new DateTime(2001, 01, 01);
                DateTime projectTimeframeEnd = new DateTime(2003, 12, 31);

                var employees = db.Employees
                    .Include(e => e.Manager)
                    .Include(e => e.EmployeesProjects)
                    .ThenInclude(ep => ep.Project)
                    .Where(e => e.EmployeesProjects
                                .Any(ep => ep.Project.StartDate >= projectTimeframeStart &&
                                ep.Project.StartDate <= projectTimeframeEnd))
                    .Take(30)
                    .ToList();

                foreach (var empl in employees)
                {
                    Console.WriteLine($"{empl.FirstName} {empl.LastName} - Manager: {empl.Manager.FirstName} {empl.Manager.LastName}");

                    foreach (var project in empl.EmployeesProjects.Select(ep => ep.Project))
                    {
                        string name = project.Name;
                        string startDate = project.StartDate.ToString(datetimeFormat, CultureInfo.InvariantCulture);
                        string endDate = string.Empty;

                        if (project.EndDate != null)
                        {
                            endDate = project.EndDate.Value.ToString(datetimeFormat, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            endDate = "not finished";
                        }

                        Console.WriteLine($"--{name} - {startDate} - {endDate}");
                    }
                }
            }
        }

        private static void AddingaNewAddressandUpdatingEmployee()
        {
            using (var db = new SoftUniContext())
            {
                var addressToAdd = new Address()
                {
                    AddressText = "Vitoshka 15",
                    TownId = 4
                };

                var nakov = db.Employees.SingleOrDefault(e => e.LastName == "Nakov");

                db.Addresses.Add(addressToAdd);
                nakov.Address = addressToAdd;

                db.SaveChanges();

                var employees = db.Employees
                    .Include(e => e.Address)
                    .OrderByDescending(e => e.AddressId)
                    .Take(10)
                    .ToList();

                foreach (var empl in employees)
                {
                    Console.WriteLine(empl.Address.AddressText);
                }
            }
        }

        private static void ResearchandDevelopment()
        {
            using (var db = new SoftUniContext())
            {
                var employees = db.Employees
                    .Where(e => e.Department.Name == "Research and Development")
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.Department,
                        e.Salary
                    })
                    .OrderBy(e => e.Salary)
                    .ThenByDescending(e => e.FirstName)
                    .ToList();

                foreach (var e in employees)
                {
                    Console.WriteLine($"{e.FirstName} {e.LastName} from Research and Development - ${e.Salary:f2}");
                }
            }
        }

        private static void EmployeeswithSalaryOver50000()
        {
            using (var db = new SoftUniContext())
            {
                var employees = db.Employees
                    .Select(e => new
                    {
                        e.FirstName,
                        e.Salary
                    })
                    .Where(e => e.Salary > 50000)
                    .OrderBy(e => e.FirstName)
                    .ToList();

                foreach (var employee in employees)
                {
                    Console.WriteLine(employee.FirstName);
                }
            }
        }

        private static void EmployeesFullInformation()
        {
            using (var db = new SoftUniContext())
            {
                var employees = db.Employees
                    .Select(e => new
                    {
                        e.EmployeeId,
                        e.FirstName,
                        e.LastName,
                        e.MiddleName,
                        e.JobTitle,
                        e.Salary
                    })
                    .OrderBy(e => e.EmployeeId)
                    .ToList();

                foreach (var employee in employees)
                {
                    Console.WriteLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
                }
            }
        }
    }
}
