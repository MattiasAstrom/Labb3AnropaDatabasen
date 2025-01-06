using System;
using System.Collections.Generic;

namespace Labb3AnropaDatabasen.Models;

public partial class Enrollment
{
    public int EnrollmentId { get; set; }

    public int? FkCourseId { get; set; }

    public int? FkStudentId { get; set; }

    public int? GradeId { get; set; }

    public virtual Course? FkCourse { get; set; }

    public virtual Student? FkStudent { get; set; }

    public virtual Grade? Grade { get; set; }
}
