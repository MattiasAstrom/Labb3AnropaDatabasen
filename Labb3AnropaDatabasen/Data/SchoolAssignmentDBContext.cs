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

    public virtual DbSet<Employee> Employees { get; set; }

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
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__37E005FB1854F90F");

            entity.Property(e => e.CourseId)
                .ValueGeneratedNever()
                .HasColumnName("Course_ID");
            entity.Property(e => e.CourseDescription).IsUnicode(false);
            entity.Property(e => e.CourseName)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.CourseTeacherNavigation).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CourseTeacher)
                .HasConstraintName("FK__Courses__CourseT__2D27B809");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__781134814B09F9D9");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("Employee_ID");
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

            entity.HasOne(d => d.TitleNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.Title)
                .HasConstraintName("FK__Employees__Title__2A4B4B5E");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.GradeId).HasName("PK__Grades__D4437153980455F6");

            entity.Property(e => e.GradeId)
                .ValueGeneratedNever()
                .HasColumnName("Grade_ID");
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
                .HasConstraintName("FK__Grades__Course_I__30F848ED");

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Grades__Student___300424B4");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Grades)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__Grades__Teacher___31EC6D26");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__A2F4E9ACDF06D938");

            entity.HasIndex(e => e.Ssn, "UQ__Students__CA1E8E3C7050C5C1").IsUnique();

            entity.Property(e => e.StudentId)
                .ValueGeneratedNever()
                .HasColumnName("Student_ID");
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
            entity.HasKey(e => e.TitleId).HasName("PK__Titles__01D44740C7895D1E");

            entity.Property(e => e.TitleId)
                .ValueGeneratedNever()
                .HasColumnName("Title_ID");
            entity.Property(e => e.TitleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
