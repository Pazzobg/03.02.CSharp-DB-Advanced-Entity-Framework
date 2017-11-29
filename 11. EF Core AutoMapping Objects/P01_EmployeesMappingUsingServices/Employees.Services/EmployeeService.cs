namespace Employees.Services
{
    using System;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Employees.Data;
    using Employees.DtoModels;
    using Employees.Models;
    using Microsoft.EntityFrameworkCore;

    public class EmployeeService
    {
        private readonly EmployeesContext context;

        public EmployeeService(EmployeesContext context)
        {
            this.context = context;
        }

        public EmployeeDto ById(int employeeId)
        {
            var employee = context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid EmployeeId!");
            }

            var employeeDto = Mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        // ====== Pr. 1 Employees Mapping ======

        public PersonalInfoDto FullPersonalInfoById(int employeeId)
        {
            var empl = context.Employees.Find(employeeId);

            if (empl == null)
            {
                throw new ArgumentException("Invalid EmployeeId!");
            }

            var personalInfoDto = Mapper.Map<PersonalInfoDto>(empl);

            return personalInfoDto;
        }

        public void AddEmployee(EmployeeDto dto)
        {
            var employee = Mapper.Map<Employee>(dto);

            context.Employees.Add(employee);
            context.SaveChanges();
        }

        public string SetBirthday(int employeeId, DateTime birthdayDate)
        {
            var employee = context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid EmployeeId!");
            }

            employee.Birthday = birthdayDate;

            context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        public string SetAddress(int employeeId, string address)
        {
            var employee = context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid EmployeeId!");
            }

            employee.Address = address;

            context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        // ====== Pr. 2 Manager Mapping ======

        public string SetManager(int employeeId, int managerId)
        {
            var employee = context.Employees.Find(employeeId);
            var manager = context.Employees.Find(managerId);

            if (employee == null || manager == null)
            {
                throw new ArgumentException("Invalid EmployeeId or ManagerId!");
            }

            employee.ManagerId = managerId;
            context.SaveChanges();

            string employeeManagerNames = $"{employee.FirstName} {employee.LastName} {manager.FirstName} {manager.LastName}";
            return employeeManagerNames;
        }

        public ManagerDto ManagerInfo(int employeeId)
        {
            var manager = context.Employees
                .Include(m => m.ManagedEmployees)
                .FirstOrDefault(m => m.Id == employeeId);

            if (manager == null)
            {
                throw new ArgumentException("Invalid ManagerId!");
            }

            var manDto = Mapper.Map<ManagerDto>(manager);
            return manDto;
        }

        // ====== Pr. 3 Projection ======

        public EmployeeByAgeDto[] ListEmployeesOlder(int age)
        {
            var employeesList = context.Employees
                .Where(e => e.Age > age)
                .Include(e => e.Manager)
                .ProjectTo<EmployeeByAgeDto>()
                .ToArray();

            return employeesList;
        }
    }
}
