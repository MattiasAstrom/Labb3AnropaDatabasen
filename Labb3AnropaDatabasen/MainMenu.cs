using Labb3AnropaDatabasen.Data;
using Labb3AnropaDatabasen.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Labb3AnropaDatabasen
{
    internal class MainMenu
    {
        StudentService? _studentService;
        EmployeeService? _employeeService;

        private void Init()
        {
            _studentService = new();
            _employeeService = new();
        }

        public void Start()
        {
            Init();

            using (SchoolAssignmentDBContext context = new())
            {
                bool isRunning = true;

                while (isRunning)
                {
                    Console.Clear();
                    Console.WriteLine("Pick an Option:");
                    Console.WriteLine("1. Hämta personal");
                    Console.WriteLine("2. Hämta alla elever");
                    Console.WriteLine("3. Hämta alla elever i en viss klass");
                    Console.WriteLine("4. Hämta betyg från senaste månaden");
                    Console.WriteLine("5. Hämta kurser med snittbetyg");
                    Console.WriteLine("6. Lägga till nya elever");
                    Console.WriteLine("7. Lägga till ny personal");

                    Console.WriteLine("8. Hur många lärare jobbar på de olika avdelningarna?");
                    Console.WriteLine("9. Visa information om alla elever");
                    Console.WriteLine("10. Visa en lsita på alla aktive kurser");

                    int choice = InputValidationHelpers.GetValidMenuChoice(1,9);

                    switch (choice)
                    {
                        case 1:
                            _employeeService?.GetEmployeesMenu(context);
                            break;
                        case 2:
                            _studentService?.GetStudentsMenu(context);
                            break;
                        case 3:
                            _studentService?.GetStudentsByClass(context);
                            break;
                        case 4:
                            _employeeService?.GetGradesLastMonth(context);
                            break;
                        case 5:
                            _employeeService?.GetCoursesWithGrades(context);
                            break;
                        case 6:
                            _studentService?.AddStudent(context);
                            break;
                        case 7:
                            _employeeService?.AddEmployee(context);
                            break;
                    }
                }
            }
        }
    }
}
