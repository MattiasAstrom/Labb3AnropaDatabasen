using System;
using System.Collections.Generic;
using Labb3AnropaDatabasen.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb3AnropaDatabasen.Data;

public partial class SchoolAssignmentDBContext : DbContext
{
    public SchoolAssignmentDBContext()
    {
    }

    public SchoolAssignmentDBContext(DbContextOptions<SchoolAssignmentDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Title> Titles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SchoolAssignmentDB;Integrated Security=True;Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__37E005FB906570ED");

            entity.Property(e => e.CourseId).HasColumnName("Course_ID");
            entity.Property(e => e.CourseDescription).IsUnicode(false);
            entity.Property(e => e.CourseName)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.CourseTeacherNavigation).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CourseTeacher)
                .HasConstraintName("FK__Courses__CourseT__33D4B598");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__151675D15EF782FD");

            entity.Property(e => e.DepartmentId).HasColumnName("Department_ID");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__781134814544508B");

            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.PrimaryDepartmentNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PrimaryDepartment)
                .HasConstraintName("FK__Employees__Prima__2D27B809");

            entity.HasOne(d => d.TitleNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.Title)
                .HasConstraintName("FK__Employees__Title__2C3393D0");

            entity.HasMany(d => d.Departments).WithMany(p => p.EmployeesNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeDepartment",
                    r => r.HasOne<Department>().WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__EmployeeD__Depar__30F848ED"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__EmployeeD__Emplo__300424B4"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "DepartmentId").HasName("PK__Employee__794053DCCCF55940");
                        j.ToTable("EmployeeDepartments");
                        j.IndexerProperty<int>("EmployeeId").HasColumnName("Employee_ID");
                        j.IndexerProperty<int>("DepartmentId").HasColumnName("Department_ID");
                    });
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__Enrollme__4365BD6A7CAAB65B");

            entity.Property(e => e.EnrollmentId).HasColumnName("Enrollment_ID");
            entity.Property(e => e.FkCourseId).HasColumnName("FK_Course_ID");
            entity.Property(e => e.FkStudentId).HasColumnName("FK_Student_ID");
            entity.Property(e => e.GradeId).HasColumnName("Grade_ID");

            entity.HasOne(d => d.FkCourse).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.FkCourseId)
                .HasConstraintName("FK__Enrollmen__FK_Co__5441852A");

            entity.HasOne(d => d.FkStudent).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.FkStudentId)
                .HasConstraintName("FK__Enrollmen__FK_St__5535A963");

            entity.HasOne(d => d.Grade).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.GradeId)
                .HasConstraintName("FK__Enrollmen__Grade__5629CD9C");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.GradeId).HasName("PK__Grades__D44371535DA0C590");

            entity.Property(e => e.GradeId).HasColumnName("Grade_ID");
            entity.Property(e => e.CourseId).HasColumnName("Course_ID");
            entity.Property(e => e.Grade1)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Grade");
            entity.Property(e => e.StudentId).HasColumnName("Student_ID");
            entity.Property(e => e.TeacherId).HasColumnName("Teacher_ID");

            entity.HasOne(d => d.Course).WithMany(p => p.Grades)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Grades__Course_I__37A5467C");

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Grades__Student___36B12243");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Grades)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__Grades__Teacher___38996AB5");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__A2F4E9AC0EFBB798");

            entity.HasIndex(e => e.Ssn, "UQ__Students__CA1E8E3C841FE4A4").IsUnique();

            entity.Property(e => e.StudentId).HasColumnName("Student_ID");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Ssn)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SSN");

            entity.HasOne(d => d.TitleNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Title)
                .HasConstraintName("FK__Students__Title__276EDEB3");
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.HasKey(e => e.TitleId).HasName("PK__Titles__01D44740082F4708");

            entity.Property(e => e.TitleId).HasColumnName("Title_ID");
            entity.Property(e => e.TitleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
