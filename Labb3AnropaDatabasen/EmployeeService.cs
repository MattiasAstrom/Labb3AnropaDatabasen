using Labb3AnropaDatabasen.Data;
using Labb3AnropaDatabasen.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb3AnropaDatabasen
{
    internal class EmployeeService
    {
        // Get all employees or employees with a specific title
        public void GetEmployeesMenu(SchoolAssignmentDBContext context)
        {
            int choice = 0;
            bool validChoice = false;

            Console.Clear();
            Console.WriteLine("Available Titles:");

            var titles = context.Employees.Select(e => e.TitleNavigation.TitleName).Distinct().ToList();

            for (int i = 0; i < titles.Count; i++)
                Console.WriteLine($"{i + 1}. {titles[i]}");

            Console.WriteLine($"{titles.Count + 1}. All");

            while (!validChoice)
            {
                Console.WriteLine("Pick one between 1 to " + (titles.Count + 1) + ":");
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

        //Check what grades have been added with the dates that are within the last month.
        public void GetGradesLastMonth(SchoolAssignmentDBContext context)
        {
            Console.Clear();
            var currentDate = DateTime.Now;
            var lastMonthStart = currentDate.AddMonths(-1).Date;
            var lastMonthEnd = currentDate.Date;

            var allGrades = context.Grades
                .Where(g => g.DateOfGrading.HasValue)
                .ToList();

            var gradesQuery = allGrades
                 .Where(g => (g.DateOfGrading.Value.Year == currentDate.Year) && (g.DateOfGrading.Value.Month >= lastMonthStart.Month) && (g.DateOfGrading.Value.Month <= lastMonthEnd.Month))
                 .ToList();

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

        //Goes through every course and prints AVG grade, highest grade and lowest grade.
        public void GetCoursesWithGrades(SchoolAssignmentDBContext context)
        {
            Console.Clear();
            Console.WriteLine("Loading...");
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

            //used to easier get letter grade from numeric grade, don't have loop through the original dictionary.
            Dictionary<double, string> reverseGradeMapping = gradeMapping.ToDictionary(x => x.Value, x => x.Key);
            var courses = context.Courses.Include(c => c.Grades).ToList();
            Console.Clear();

            foreach (var course in courses)
            {
                var numericGrades = course.Grades
                    .Where(g => !string.IsNullOrWhiteSpace(g.Grade1) && gradeMapping.ContainsKey(g.Grade1.Trim()))  // Trim and check validity
                    .Select(g => gradeMapping[g.Grade1.Trim()])
                    .ToList();

                if (numericGrades.Any())
                {
                    var avgGrade = numericGrades.Average();
                    var highestGrade = numericGrades.Max();
                    var lowestGrade = numericGrades.Min();

                    // Convert numeric average back to letter grade using the reversed grade dictionary
                    string avgLetterGrade = GetLetterGrade(avgGrade, reverseGradeMapping);

                    Console.WriteLine($"Course: {course.CourseName}, Average Grade: {avgLetterGrade}, Highest Grade: {reverseGradeMapping[highestGrade]}, Lowest Grade: {reverseGradeMapping[lowestGrade]}");
                }
                else
                    Console.WriteLine($"Course: {course.CourseName}, No valid grades.");
            }

            Console.ReadLine();
        }

        // Get avg letter grade based on the closest numeric score.
        private string GetLetterGrade(double avgGrade, Dictionary<double, string> reverseGradeMapping)
        {
            var closestGrade = reverseGradeMapping.Keys.OrderBy(x => Math.Abs(x - avgGrade)).First();
            return reverseGradeMapping[closestGrade];
        }

        public void AddEmployee(SchoolAssignmentDBContext context)
        {
            Console.Clear();

            // Display all current titles excluding "Active Student" and "Graduated Student"
            var existingTitles = context.Titles
                .Where(t => t.TitleName != "Active Student" && t.TitleName != "Graduated Student")
                .Select(t => t.TitleName)
                .ToList();

            Console.WriteLine("Enter new employee details!");

            //Get all the new information about the employee
            string firstName = InputValidationHelpers.GetNonEmptyString("Enter First Name: ");
            string lastName = InputValidationHelpers.GetNonEmptyString("Enter Last Name: ");
            string phoneNumber = InputValidationHelpers.GetNonEmptyString("Enter Phone Number: ");
            string email = InputValidationHelpers.GetNonEmptyString("Enter Email: ");
            string titleName = "";

            Console.WriteLine("Existing Titles:");
            for (int i = 0; i < existingTitles.Count; i++)
                Console.WriteLine($"{i + 1}. {existingTitles[i]}");

            int selectedTitleIndex = InputValidationHelpers.GetValidClassChoice(existingTitles.Count); // Use GetValidClassChoice for selecting a title
            string selectedTitleName = existingTitles[selectedTitleIndex - 1];
            var title = context.Titles.FirstOrDefault(t => t.TitleName == selectedTitleName);

            // create a new employee object
            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email,
                Title = title.TitleId
            };

            // Add the new employee to the database
            context.Employees.Add(employee);
            context.SaveChanges();

            Console.WriteLine("New employee added!");
            Console.ReadLine();
        }

        public void GetEmployeesByDepartment(SchoolAssignmentDBContext context)
        {
            // Get departments with their associated employees
            var departments = context.Departments.Include(d => d.Employees).ToList();

            // Loop through each department and print the department name and the number of employees
            foreach (var department in departments)
            {
                // Print the department name and the count of employees in that department
                Console.WriteLine($"Department: {department.DepartmentName}, Employees: {department.Employees.Count}");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}