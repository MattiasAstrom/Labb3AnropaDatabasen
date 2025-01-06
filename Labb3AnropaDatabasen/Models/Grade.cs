using System;
using System.Collections.Generic;

namespace Labb3AnropaDatabasen.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public string? Grade1 { get; set; }

    public DateOnly? DateOfGrading { get; set; }

    public int? StudentId { get; set; }

    public int? CourseId { get; set; }

    public int? TeacherId { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Student? Student { get; set; }

    public virtual Employee? Teacher { get; set; }
}
