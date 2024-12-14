using System;
using System.Collections.Generic;

namespace Labb3AnropaDatabasen.Models;

public partial class Title
{
    public int TitleId { get; set; }

    public string? TitleName { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
