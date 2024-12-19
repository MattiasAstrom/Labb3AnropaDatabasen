using System;
using System.Collections.Generic;

namespace Labb3AnropaDatabasen.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Employee> EmployeesNavigation { get; set; } = new List<Employee>();
}
