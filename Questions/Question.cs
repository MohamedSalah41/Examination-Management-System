using System;

namespace ExamSystem
{
    public abstract class Question
    {
        public string Header { get; protected set; }
        public string Body { get; protected set; }
        public int Marks { get; protected set; }
        public AnswerList Answers { get; protected set; }
        public Answer CorrectAnswer { get; protected set; }

        protected Question(string header, string body, int marks)
        {
            if (string.IsNullOrWhiteSpace(header)) throw new ArgumentException("Header cannot be empty.");
            if (string.IsNullOrWhiteSpace(body))   throw new ArgumentException("Body cannot be empty.");
            if (marks <= 0)                         throw new ArgumentException("Marks must be greater than 0.");

            Header        = header;
            Body          = body;
            Marks         = marks;
            Answers       = new AnswerList();
        }

        public abstract void Display();
        public abstract bool CheckAnswer(Answer studentAnswer);

        public override string ToString() =>
            $"[{Header}] {Body} ({Marks} marks)";

        public override bool Equals(object obj)
        {
            if (obj is Question other)
                return this.Header == other.Header && this.Body == other.Body;
            return false;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Header, Body);
    }
}
