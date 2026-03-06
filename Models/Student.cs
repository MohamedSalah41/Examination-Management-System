using System;

namespace ExamSystem
{
    public class Student
    {
        public string Name { get; private set; }
        public int Id { get; private set; }

        public Student(string name, int id)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Student name cannot be empty.");
            Name = name;
            Id   = id;
        }

        public void OnExamStarted(object sender, ExamEventArgs e)
        {
            Console.WriteLine($"  [Notification] Student '{Name}' notified: Exam for '{e.Subject.Name}' is starting!");
        }

        public override string ToString() => $"Student({Id}) {Name}";
    }
}
