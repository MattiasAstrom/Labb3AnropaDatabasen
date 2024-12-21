using Labb3AnropaDatabasen.Data;
using Labb3AnropaDatabasen.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb3AnropaDatabasen
{
    internal class StudentService
    {
        public void GetStudentsMenu(SchoolAssignmentDBContext context)
        {
            Console.Clear();
            Console.WriteLine("Sort students by:");
            Console.WriteLine("1. First Name");
            Console.WriteLine("2. Second Name");
            string sortBy = InputValidationHelpers.GetValidSortingOption(1,2);

            Console.WriteLine("Sort order:");
            Console.WriteLine("1. Ascending");
            Console.WriteLine("2. Descending");
            string sortOrder = InputValidationHelpers.GetValidSortingOption(1,2);

            var students = GetStudents(context, sortBy, sortOrder);
            foreach (var student in students)
            {
                Console.WriteLine($"{student.FirstName} {student.LastName}");
            }
            Console.ReadLine();
        }

        public List<Student> GetStudents(SchoolAssignmentDBContext context, string sortBy, string sortOrder)
        {
            var query = context.Students.AsQueryable();

            if (sortBy == "1")
                query = sortOrder == "1" ? query.OrderBy(s => s.FirstName) : query.OrderByDescending(s => s.FirstName);
            else
                query = sortOrder == "1" ? query.OrderBy(s => s.LastName) : query.OrderByDescending(s => s.LastName);

            return query.ToList();
        }

        public void GetStudentsByClass(SchoolAssignmentDBContext context)
        {
            Console.Clear();
            var titles = context.Courses.ToList();

            Console.WriteLine("Select Class:");
            for (int i = 0; i < titles.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {titles[i].CourseName}");
            }

            int classChoice = InputValidationHelpers.GetValidClassChoice(titles.Count);
            var selectedTitle = titles[classChoice - 1];

            var studentsInClass = GetStudentsByTitle(context, selectedTitle.CourseId);
            if (studentsInClass.Count == 0)
            {
                Console.WriteLine("No students found in this class.");
            }
            else
            {
                foreach (var student in studentsInClass)
                {
                    Console.WriteLine($"{student.FirstName} {student.LastName}");
                }
            }
            Console.ReadLine();
        }


        public List<Student> GetStudentsByTitle(SchoolAssignmentDBContext context, int titleId)
        {
            return context.Students.Where(s => s.Title == titleId).ToList();
        }

        public void AddStudent(SchoolAssignmentDBContext context)
        {
            Console.Clear();

            string firstName = InputValidationHelpers.GetNonEmptyString("Enter Student First Name: ");
            string lastName = InputValidationHelpers.GetNonEmptyString("Enter Student Last Name: ");
            string phoneNumber = InputValidationHelpers.GetNonEmptyString("Enter Phone Number: ");
            string email = InputValidationHelpers.GetNonEmptyString("Enter Email: ");
            string address = InputValidationHelpers.GetNonEmptyString("Enter Address: ");
            string ssn = InputValidationHelpers.GetNonEmptyString("Enter SSN (Social Security Number): ");

            // Check so SSN (social security number) is unique.
            while (context.Students.Any(s => s.Ssn == ssn))
            {
                Console.WriteLine("The SSN already exists. Please enter a different SSN.");
                Console.WriteLine("Press 'Esc' to cancel and go back to the menu.");

                var keyPress = Console.ReadKey(); 

                if (keyPress.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\nOperation canceled. Returning to the menu...");
                    return;
                }

                ssn = InputValidationHelpers.GetNonEmptyString("Enter SSN (Social Security Number): ");
            }

            //NOTE: logic if we wanted to add a custom title to student or a more comprehensive title implementation.
            //string titleName = InputValidationHelpers.GetNonEmptyString("Enter Student Title (e.g., Student, Graduate): ");
            //var title = context.Titles.FirstOrDefault(t => t.TitleName == titleName) ?? new Title { TitleName = titleName };
            //context.Titles.Add(title);

            // Create the student and set the properties
            var student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email,
                Address = address,
                Ssn = ssn,
                Title = context.Titles.FirstOrDefault(t => t.TitleName == "Active Student").TitleId
            };

            // Add the new student to the database
            context.Students.Add(student);
            context.SaveChanges();

            Console.WriteLine("New student added!");
            Console.ReadLine();
        }
    }
}