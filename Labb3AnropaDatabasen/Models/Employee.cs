using System;
using System.Collections.Generic;

namespace Labb3AnropaDatabasen.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public int? Title { get; set; }

    public DateOnly? DateOfHire { get; set; }

    public int? Salary { get; set; }

    public int? PrimaryDepartment { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual Department? PrimaryDepartmentNavigation { get; set; }

    public virtual Title? TitleNavigation { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
