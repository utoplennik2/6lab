using System;


public class Teacher
{
    public string Name { get; private set; }                      //поле + читання

    public Teacher(string name)                                   //параметризований конструктор
    {
        Name = name;
    }

    public void SubscribeToCourse(Course course)                  //підписка викладача на курс щоб отримувати сповіщення  
    {
        course.OnStudentRegistered += OnStudentRegisteredHandler; //реєстрація студентів
        course.OnCourseCompleted += OnCourseCompletedHandler;     //завершення курсу
    }
     
    private void OnStudentRegisteredHandler(Student student, Course course)  //метод про кількість студентів на курсі (при реєстрації)
    {
        Console.WriteLine("викладач отримує сповіщення: на курсі «{0}» зареєстровано {1} студентів", course.Name, course.GetRegisteredCount());
    }

    private void OnCourseCompletedHandler(Course course)  //метод про заврешення курсу  
    {
        Console.WriteLine("викладач отримує сповіщення: курс «{0}» завершено!", course.Name);
    }
}
