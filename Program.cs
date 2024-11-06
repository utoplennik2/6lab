using System;
using System.Collections.Generic;

public class Program
{
    private static List<Course> courses = new List<Course>();

    public static void Main(string[] args)
    {
        InitializeCourses();
        Teacher teacher = new Teacher(" ");

        foreach (var course in courses)                         //підписка вчитеоя на курс
        {
            teacher.SubscribeToCourse(course);
        }

        while (true)                                            //менб
        {
            Console.WriteLine("будь ласка, оберіть опцію:");
            Console.WriteLine("1 - додати студента");
            Console.WriteLine("2 - переглянути студентів на курсі");
            Console.WriteLine("3 - завершити курс");
            Console.WriteLine("4 - вихід з програми");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    AddStudent();
                    break;
                case "2":
                    ListStudentsInCourse();
                    break;
                case "3":
                    CompleteSelectedCourse();
                    break;
                case "4":
                    Console.WriteLine("програму завершено!");
                    return;
                default:
                    Console.WriteLine("будь ласка, введіть від 1-4");
                    break;
            }

            if (courses.Count == 0)
            {
                Console.WriteLine("усі курси завершено! програму завершено");
                break;
            }
        }
    }

    private static void CompleteSelectedCourse()                                   //завершення курсу
    {
        while (true)
        {
            Console.WriteLine("виберіть курс для завершення (або натисніть 0, щоб повернутися до головного меню):");

            for (int i = 0; i < courses.Count; i++)                               //виведення списку  курсів
            {
                Console.WriteLine($"{i + 1} - {courses[i].Name}");
            }

            string courseChoice = Console.ReadLine();

            if (courseChoice == "0") return;

            int courseIndex;
            if (int.TryParse(courseChoice, out courseIndex) && courseIndex >= 1 && courseIndex <= courses.Count) //перевірка на допустимість значень
            {
                Course selectedCourse = courses[courseIndex - 1];

                selectedCourse.CompleteCourse();                  //завершення курсу та видалення зі списку
                courses.Remove(selectedCourse);

                if (courses.Count == 0)                           //перевірка чи залишилися курси ще
                {
                    Console.WriteLine("усі курси завершено! програму завершено ");
                    Environment.Exit(0);  //завершення арограми
                }

                Console.WriteLine("чи бажаєте продовжити роботу з іншими курсами? (так/ні):");
                string answer = Console.ReadLine()?.ToLower();
                if (answer == "ні")
                {
                    Console.WriteLine("програму завершено");
                    Environment.Exit(0);  //завершення арограми
                }
                break;
            }
            else
            {
                Console.WriteLine("неправильний вибір курсу");
            }
        }
    }

    private static void InitializeCourses()                 //кулькісьт місць на курсах
    {
        courses.Add(new Course("програмування", 3));
        courses.Add(new Course("дизайн", 3));
    }

    private static void AddStudent()                                    // метод для додавання студентів на курс
    {
        while (true)
        {
            Console.WriteLine("додайте студента у форматі: ім'я VIP (true/false) або натисніть 0, щоб повернутися до головного меню):");
            string input = Console.ReadLine();

            if (input == "0") return;                                   // повернення до головного меню, якщо натиснуто 0

            try
            {
                var parts = input.Split(' ');

                if (parts.Length < 2)                                   // перевірка, чи введено ім'я та статус VIP
                {
                    throw new Exception("спробуйте: Ім'я VIP (true/false)");
                }

                string name = parts[0];
                bool isVIP = parts[1].ToLower() == "true";              // перетворення статусу VIP в булеве значення

                Console.WriteLine("оберіть курс (1 - програмування, 2 - дизайн або натисніть 0, щоб повернутися до головного меню):");
                string courseChoice = Console.ReadLine();               // вибір курсу

                if (courseChoice == "0") return;

                Course selectedCourse = null;

                if (courseChoice == "1")
                {
                    selectedCourse = courses[0];
                    if (selectedCourse.RegisteredStudents.Count < selectedCourse.Capacity || isVIP)  // перевірка, чи є місце на курсі
                    {
                        Student student = new Student(name, isVIP);     // створення нового студента

                        int initialCount = selectedCourse.GetRegisteredCount();  // збереження початкової кількості студентів
                        selectedCourse.RegisterStudent(student);         // реєстрація студента
                        student.SubscribeToCourse(selectedCourse);       // підписка на повідомлення про завершення курсу

                        int newCount = selectedCourse.GetRegisteredCount();
                        if (newCount > initialCount)                    // перевірка, чи студент успішно доданий
                        {
                            Console.WriteLine("вітаю! {0} було додано до курсу", name);
                        }
                    }
                    else
                    {
                        Console.WriteLine("немає місця");               // повідомлення, якщо курс заповнений
                    }
                }
                else if (courseChoice == "2")
                {
                    selectedCourse = courses[1];
                    if (selectedCourse.RegisteredStudents.Count < selectedCourse.Capacity)
                    {
                        Student student = new Student(name, isVIP);     // створення нового студента

                        int initialCount = selectedCourse.GetRegisteredCount();  // збереження початкової кількості студентів
                        selectedCourse.RegisterStudent(student);         // реєстрація студента
                        student.SubscribeToCourse(selectedCourse);       // підписка на повідомлення про завершення курсу

                        int newCount = selectedCourse.GetRegisteredCount();
                        if (newCount > initialCount)                     // перевірка, чи студент успішно доданий
                        {
                            Console.WriteLine("вітаю! {0} було додано до курсу", name);
                        }
                    }
                    else
                    {
                        Console.WriteLine("немає місця");               // повідомлення, якщо курс заповнений
                    }
                }
                else
                {
                    throw new Exception("некоректне введення даних");   // помилка, якщо введено невірний вибір курсу
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("помилка: {0}", ex.Message);         // виведення повідомлення про помилку
            }
        }
    }

    public static void ListCourses()                               // метод для виведення списку курсів
    {
        if (courses.Count == 0)
        {
            Console.WriteLine("Немає доступних курсів.");
            return;
        }

        Console.WriteLine("Доступні курси:");
        for (int i = 0; i < courses.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {courses[i].Name}");   // Виведення індексу та назви курсу
        }
    }


    private static void ListStudentsInCourse()                              //перегляд студентів на обраному курсі
    {
        ListCourses();
        while (true)
        {
            Console.WriteLine("оберіть курс для перегляду студентів (0, щоб повернутися до головного меню):");
            string courseChoice = Console.ReadLine();

            if (courseChoice == "0") return;

            if (courseChoice == "1" && courses.Count > 0 && courses[0] != null)
            {
                if (courses[0].GetRegisteredCount() == 0)
                {
                    Console.WriteLine("Студенти цього курсу були видалені!");
                }
                else
                {
                    courses[0].ListStudents();
                    Console.WriteLine($"{courses[0].GetRegisteredCount()}\n {courses[0].Name}");
                }
            }
            else if (courseChoice == "2" && courses.Count > 1 && courses[1] != null)
            {
                if (courses[1].GetRegisteredCount() == 0)
                {
                    Console.WriteLine("Студенти цього курсу були видалені!");
                }
                else
                {
                    courses[1].ListStudents();
                    Console.WriteLine($"{courses[1].GetRegisteredCount()}\n {courses[1].Name}");
                    
                }
            }
            else
            {
                Console.WriteLine("Невірний вибір курсу або курс вже був видалений.");
            }
        }
    }

}
