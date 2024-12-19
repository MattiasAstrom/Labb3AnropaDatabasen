using System;
using System.Collections.Generic;

namespace Labb3AnropaDatabasen.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public string? CourseDescription { get; set; }

    public int? CourseTeacher { get; set; }

    public int? Credits { get; set; }

    public int? ArchivedBoolean { get; set; }

    public virtual Employee? CourseTeacherNavigation { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
