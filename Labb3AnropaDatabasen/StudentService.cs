using Labb3AnropaDatabasen.Data;
using Labb3AnropaDatabasen.Models;

namespace Labb3AnropaDatabasen
{
    internal class StudentService
    {
        public void GetStudentsMenu(SchoolAssignmentDBContext context)
        {
            Console.Clear();
            Console.WriteLine("Sort students by (1) First Name, (2) Last Name: ");
            string sortBy = InputValidationHelpers.GetValidSortingOption();

            Console.WriteLine("Sort order (1) Ascending, (2) Descending: ");
            string sortOrder = InputValidationHelpers.GetValidSortingOption();

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

            // Start the SSN uniqueness check
            while (context.Students.Any(s => s.Ssn == ssn))
            {
                Console.WriteLine("The SSN already exists. Please enter a different SSN.");
                Console.WriteLine("Press 'Esc' to cancel and go back to the menu.");

                // Allow the user to cancel by pressing 'Esc'
                var keyPress = Console.ReadKey(intercept: true); // intercept key press to avoid printing to the console

                // Check if the user pressed 'Esc' to exit
                if (keyPress.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\nOperation canceled. Returning to the menu...");
                    return; // Exit the method and go back to the menu
                }

                // Otherwise, prompt the user to enter a new SSN
                ssn = InputValidationHelpers.GetNonEmptyString("Enter SSN (Social Security Number): ");
            }

            // Get or create the Title
            string titleName = InputValidationHelpers.GetNonEmptyString("Enter Student Title (e.g., Student, Graduate): ");
            var title = context.Titles.FirstOrDefault(t => t.TitleName == titleName) ?? new Title { TitleName = titleName };
            context.Titles.Add(title);

            // Create the student and set the properties
            var student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email,
                Address = address,
                Ssn = ssn,
                Title = title.TitleId,
                StudentId = context.Students.Max(s => s.StudentId) + 1
            };

            // Add the new student to the database
            context.Students.Add(student);
            context.SaveChanges();

            Console.WriteLine("New student added!");
            Console.ReadLine();
        }


    }
}
