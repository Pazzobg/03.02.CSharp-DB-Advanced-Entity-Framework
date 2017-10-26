using System;
using System.Collections.Generic;
using System.Linq;

public class StartUp
{
    public static void Main()
    {
        var company = new Dictionary<string, List<Employee>>();

        int n = int.Parse(Console.ReadLine());

        for (int i = 0; i < n; i++)
        {
            string[] input = Console.ReadLine().Split();

            string name = input[0];
            decimal salary = decimal.Parse(input[1]);
            string position = input[2];
            string department = input[3];
            string email = "n/a";
            int age = -1;

            /* Works for 80/100 in Judge
             * 
             if (input.Length == 5)
            {
                if (input[4].Contains("@"))
                {
                    email = input[4];
                }
                else
                {
                    age = int.Parse(input[4]);
                }
            }
            else if (input.Length == 6)
            {
                if (input[4].Contains("@"))
                {
                    email = input[4];
                    age = int.Parse(input[5]);
                }
                else
                {
                    age = int.Parse(input[4]);
                    email = input[5];
                }
            }
             */

            if (input.Length == 5 || input.Length == 6)
            {
                int ageParsed = -1;
                bool successfullyParsed = int.TryParse(input[4], out ageParsed);

                if (input.Length == 5)
                {
                    if (successfullyParsed)
                    {
                        age = ageParsed;
                    }
                    else
                    {
                        email = input[4];
                    }
                }
                else if (input.Length == 6)
                {
                    if (successfullyParsed)
                    {
                        age = ageParsed;
                        email = input[5];
                    }
                    else
                    {
                        email = input[4];
                        age = int.Parse(input[5]);
                    }
                }
            }

            var currentEmployee = new Employee(name, salary, position, department, email, age);

            if (!company.ContainsKey(department))
            {
                company[department] = new List<Employee>();
            }

            company[department].Add(currentEmployee);
        }

        var orderedCompany = company
            .OrderByDescending(x => x.Value.Average(y => y.Salary))
            .ToDictionary(x => x.Key, x => x.Value);

        var bestPaidDept = orderedCompany.First();
        var bestPaidEmployees = bestPaidDept.Value.ToList();

        Console.WriteLine($"Highest Average Salary: {bestPaidDept.Key}");

        foreach (var employee in bestPaidEmployees.OrderByDescending(x => x.Salary))
        {
            Console.WriteLine(employee.ToString());
        }
    }
}