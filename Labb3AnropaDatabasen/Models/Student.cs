using System;
using System.Collections.Generic;

namespace Labb3AnropaDatabasen.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Ssn { get; set; }

    public string? PhoneNumber { get; set; }

    public int? Title { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual Title? TitleNavigation { get; set; }
}
