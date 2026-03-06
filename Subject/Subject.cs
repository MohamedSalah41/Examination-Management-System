using System;

namespace ExamSystem
{
    public class Subject
    {
        public string Name { get; private set; }

        private Student[] _enrolledStudents;
        private int _count;

        public Student[] EnrolledStudents => _enrolledStudents;
        public int StudentCount => _count;

        public Subject(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Subject name cannot be empty.");
            Name              = name;
            _enrolledStudents = new Student[20];
            _count            = 0;
        }

        public void Enroll(Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));
            if (_count == _enrolledStudents.Length) throw new InvalidOperationException("Max students reached.");
            _enrolledStudents[_count++] = student;
        }

        public void NotifyStudents(Exam exam)
        {
            Console.WriteLine($"\n--- Notifying {_count} student(s) enrolled in '{Name}' ---");
            for (int i = 0; i < _count; i++)
                _enrolledStudents[i].OnExamStarted(this, new ExamEventArgs(this, exam));
        }

        public override string ToString() => $"Subject: {Name} ({_count} students)";
    }
}
