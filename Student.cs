using System;

public class Student
{
    public string Name { get; private set; }                      //поля + властивості
    public bool IsVIP { get; private set; }

    public Student(string name, bool isVIP)                       //конструктор
    {
        Name = name;
        IsVIP = isVIP;
    }

    public void SubscribeToCourse(Course course)                //метод що підписує студентів на завершення курсу
    {
        course.OnCourseCompleted += OnCourseCompletedHandler;  
    }
    
    public void UnsubscribeFromCourse(Course course)             //відписка від події завершення курсу
    {
        course.OnCourseCompleted -= OnCourseCompletedHandler;  
    }

    private void OnCourseCompletedHandler(Course course)         //метод що інформує студентів про завершення курсу
    {
        Console.WriteLine("студент {0} отримує повідомлення: курс «{1}» завершено!", Name, course.Name);
    }
}
