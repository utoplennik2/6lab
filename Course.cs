using System;
using System.Collections.Generic;

public class Course
{
    public string Name { get; private set; }                        //поля + читанння 
    public int Capacity { get; private set; }

    public List<Student> RegisteredStudents { get; private set; }  //список студентів 
    public event Action<Student, Course> OnStudentRegistered;      //подія реєстрації стуента на курсі
    public event Action<Course> OnCourseFull;                      //подія заповненого курсу
    public event Action<Course> OnCourseCompleted;                 //подія завершення курсу

    public Course(string name, int capacity)                       //параметризований констурктор 
    {
        Name = name;
        Capacity = capacity;
        RegisteredStudents = new List<Student>();
    }

    public void RegisterStudent(Student student)                   //метод для реєстрації студентів
    { 

        if (student.IsVIP)                                          //перевірка чи студент віп
        {
            var nonVipStudent = RegisteredStudents.Find(s => !s.IsVIP);
            if (nonVipStudent != null)                                   //заміна не віп студента
            {
                RegisteredStudents.Remove(nonVipStudent);
                Console.WriteLine("на жаль, {0} був витіснений через реєстрацію VIP-студента {1}", nonVipStudent.Name, student.Name);
                RegisteredStudents.Add(student);
                OnStudentRegistered?.Invoke(student, this);
            }
            else                                                        //якщо всі місця зайняті віп студентами
            {
                Console.WriteLine("на жаль, {0} не може бути зареєстрований на курс «{1}», оскільки курс повністю заповнений VIP-студентами", student.Name, Name);
                OnCourseFull?.Invoke(this);
            }
        }
        else 
        {
            RegisteredStudents.Add(student);
            OnStudentRegistered?.Invoke(student, this);
        }
    }

    public int GetRegisteredCount()                                         //метод що показує кількість студентів на курсі
    {
        return RegisteredStudents.Count;
    }
    public void ListStudents()                                              //метод для виведення списку зареєстрованих студентів
    {
        Console.WriteLine("студенти на курсі «{0}»:", Name);
        if (RegisteredStudents.Count == 0)
        {
            Console.WriteLine("зареєстрованих студентів немає");
        }
        else
        {
            foreach (var student in RegisteredStudents)
            {
                Console.WriteLine("- {0} (VIP: {1})", student.Name, student.IsVIP);
            }
        }
    }
  
    public void CompleteCourse()                                                     //метод для завершення курсів
    {
                                                                                     //студенти отримали повідомлення.
        OnCourseCompleted?.Invoke(this);

        foreach (var student in RegisteredStudents)  //ВИДАЛЯЄМО СТУДЕНІв
        {
            student.UnsubscribeFromCourse(this);     //відписуємо кожного студента від курсу
        }

        RegisteredStudents.Clear();                  //очищення списку студентів
         
        OnStudentRegistered = null;
        OnCourseFull = null;

        Console.WriteLine("Курс «{0}» завершено", Name);
    }

}

