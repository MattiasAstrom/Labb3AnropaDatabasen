using Labb3AnropaDatabasen.Data;
using Labb3AnropaDatabasen.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb3AnropaDatabasen
{
    internal class EmployeeService
    {
        public void GetEmployeesMenu(SchoolAssignmentDBContext context)
        {
            int choice = 0;
            bool validChoice = false;

            Console.Clear();
            Console.WriteLine("Select a Title:");

            var titles = context.Employees.Select(e => e.TitleNavigation.TitleName).Distinct().ToList();

            for (int i = 0; i < titles.Count; i++)
                Console.WriteLine($"{i + 1}. {titles[i]}");

            Console.WriteLine($"{titles.Count + 1}. All");

            while (!validChoice)
            {
                Console.WriteLine("Enter the number corresponding to the title (1 to " + (titles.Count + 1) + "):");
                string input = Console.ReadLine();

                if (int.TryParse(input, out choice) && choice >= 1 && choice <= titles.Count + 1)
                    validChoice = true;
                else
                    Console.WriteLine("Invalid selection, please try again.");
            }

            string? selectedTitle = null;
            if (choice <= titles.Count)
            {
                selectedTitle = titles[choice - 1];
            }

            var employees = GetEmployees(context, selectedTitle);

            if (employees.Count == 0)
                Console.WriteLine("No employees found.");
            else
            {
                foreach (var employee in employees)
                    Console.WriteLine($"{employee.FirstName} {employee.LastName}, Title: {employee.TitleNavigation?.TitleName}");
            }

            Console.ReadLine();
        }


        public List<Employee> GetEmployees(SchoolAssignmentDBContext context, string title = null)
        {
            var query = context.Employees.Include(e => e.TitleNavigation).AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(e => e.TitleNavigation.TitleName == title);

            return query.ToList();
        }

        public void GetGradesLastMonth(SchoolAssignmentDBContext context)
        {
            Console.Clear();

            // Get the current date
            var currentDate = DateTime.Now;

            // Start of the date range (1 month ago from today)
            var lastMonthStart = currentDate.AddMonths(-1).Date;  // 1 month back from today at midnight (ignoring time)

            // End of the date range (today)
            var lastMonthEnd = currentDate.Date;  // Today's date at midnight (ignoring time)

            // Print the date range for debugging
            Console.WriteLine($"Date Range: {lastMonthStart.ToShortDateString()} to {lastMonthEnd.ToShortDateString()}");

            // Check all grades with non-null DateOfGrading
            var allGrades = context.Grades
                .Where(g => g.DateOfGrading.HasValue)
                .ToList();

            // Print out the found grades and their DateOfGrading for debugging
            Console.WriteLine($"Found {allGrades.Count} grades with non-null DateOfGrading:");
            foreach (var grade in allGrades)
            {
                Console.WriteLine($"GradeId: {grade.GradeId}, DateOfGrading: {grade.DateOfGrading?.ToShortDateString()}");
            }

            // Filter grades within the date range (between November 15th and December 15th)
            var gradesQuery = allGrades
                 .Where(g => g.DateOfGrading.Value.Month >= lastMonthStart.Month && g.DateOfGrading.Value.Month <= lastMonthEnd.Month)
                 .ToList();

            // Print out how many grades matched the date range
            Console.WriteLine($"Found {gradesQuery.Count} grades between {lastMonthStart.ToShortDateString()} and {lastMonthEnd.ToShortDateString()}.");

            // If there are no grades, notify and return
            if (gradesQuery.Any())
            {
                var grades = gradesQuery
                    .Join(context.Students,
                          grade => grade.StudentId,
                          student => student.StudentId,
                          (grade, student) => new { grade, student })  // Inner join with Students
                    .Join(context.Courses,
                          gs => gs.grade.CourseId,
                          course => course.CourseId,
                          (gs, course) => new { gs.grade, gs.student, course })  // Inner join with Courses
                    .Select(x => new
                    {
                        StudentName = $"{x.student.FirstName} {x.student.LastName}",
                        CourseName = x.course.CourseName,
                        Grade = x.grade.Grade1,
                        DateOfGrading = x.grade.DateOfGrading
                    })
                    .ToList();

                // Print the grades with student and course details
                Console.WriteLine("Grades from the last month (between 11/15/2024 and 12/15/2024):\n");
                foreach (var grade in grades)
                {
                    Console.WriteLine($"Student: {grade.StudentName}, " +
                                      $"Course: {grade.CourseName}, " +
                                      $"Grade: {grade.Grade}, " +
                                      $"Date of Grading: {grade.DateOfGrading?.ToShortDateString()}");
                }
            }
            else
            {
                Console.WriteLine($"No grades found between {lastMonthStart.ToShortDateString()} and {lastMonthEnd.ToShortDateString()}.");
            }

            Console.ReadKey();
        }

        public void GetCoursesWithGrades(SchoolAssignmentDBContext context)
        {
            Console.Clear();

            // Map letter grades to numeric values
            Dictionary<string, double> gradeMapping = new Dictionary<string, double>
    {
        { "A+", 8 },
        { "A", 7 },
        { "B+", 6 },
        { "B", 5 },
        { "C+", 4 },
        { "C", 3 },
        { "D+", 2 },
        { "D", 1 },
        { "F", 0 }
    };

            // Reverse the grade mapping for converting numeric back to letter grades
            Dictionary<double, string> reverseGradeMapping = gradeMapping.ToDictionary(x => x.Value, x => x.Key);

            var courses = context.Courses.Include(c => c.Grades).ToList();

            foreach (var course in courses)
            {
                // Convert letter grades to numeric grades
                var numericGrades = course.Grades
                    .Where(g => !string.IsNullOrWhiteSpace(g.Grade1) && gradeMapping.ContainsKey(g.Grade1.Trim()))  // Trim and check validity
                    .Select(g => gradeMapping[g.Grade1.Trim()])
                    .ToList();

                // Debugging: Print the grades that are being processed
                if (numericGrades.Any())
                {
                    var avgGrade = numericGrades.Average();
                    var highestGrade = numericGrades.Max();
                    var lowestGrade = numericGrades.Min();

                    // Convert numeric average back to letter grade
                    string avgLetterGrade = GetLetterGrade(avgGrade, reverseGradeMapping);

                    Console.WriteLine($"Course: {course.CourseName}, Average Grade: {avgLetterGrade}, Highest Grade: {reverseGradeMapping[highestGrade]}, Lowest Grade: {reverseGradeMapping[lowestGrade]}");
                }
                else
                {
                    Console.WriteLine($"Course: {course.CourseName}, No valid grades.");
                }
            }

            Console.ReadLine();
        }

        // Function to map numeric average to the corresponding letter grade
        private string GetLetterGrade(double avgGrade, Dictionary<double, string> reverseGradeMapping)
        {
            // Find the closest numeric grade and map it back to the letter grade
            var closestGrade = reverseGradeMapping.Keys.OrderBy(x => Math.Abs(x - avgGrade)).First();
            return reverseGradeMapping[closestGrade];
        }

        public void AddEmployee(SchoolAssignmentDBContext context)
        {
            Console.Clear();

            // Display all current titles excluding "Student" and "Graduated Student"
            var existingTitles = context.Titles
                .Where(t => t.TitleName != "Active Student" && t.TitleName != "Graduated Student")
                .Select(t => t.TitleName)
                .ToList();

            Console.WriteLine("Existing Titles:");
            foreach (var item in existingTitles)
            {
                Console.WriteLine($"- {item}");
            }

            Console.WriteLine("\nEnter new employee details:");

            string firstName = InputValidationHelpers.GetNonEmptyString("Enter First Name: ");
            string lastName = InputValidationHelpers.GetNonEmptyString("Enter Last Name: ");
            string phoneNumber = InputValidationHelpers.GetNonEmptyString("Enter Phone Number: ");
            string email = InputValidationHelpers.GetNonEmptyString("Enter Email: ");

            // Get or create the Title
            foreach (var item in existingTitles)
                Console.WriteLine($"- {item}");
            
            string titleName = InputValidationHelpers.GetNonEmptyString("Enter Employee Title (e.g., Teacher, Admin): ");
            var title = context.Titles.FirstOrDefault(t => t.TitleName == titleName) ?? new Title { TitleName = titleName };

            // Avoid adding duplicate titles, only add if the title doesn't already exist
            if (!existingTitles.Contains(titleName))
            {
                context.Titles.Add(title);
            }

            // Create the employee and set the properties
            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email,
                Title = title.TitleId,
                EmployeeId = context.Students.Max(s => s.StudentId) + 1
            };

            // Add the new employee to the database
            context.Employees.Add(employee);
            context.SaveChanges();

            Console.WriteLine("New employee added!");
            Console.ReadLine();
        }

    }
}
