using System;

namespace ExamSystem
{
    // Delegate
    public delegate void ExamStartedHandler(object sender, ExamEventArgs e);

    public class ExamEventArgs : EventArgs
    {
        public Subject Subject { get; }
        public Exam Exam { get; }

        public ExamEventArgs(Subject subject, Exam exam)
        {
            Subject = subject;
            Exam    = exam;
        }
    }
}
