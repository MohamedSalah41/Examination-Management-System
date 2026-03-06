using System;
using System.Collections.Generic;

namespace ExamSystem
{
    public abstract class Exam : ICloneable, IComparable<Exam>
    {
        public int Time { get; protected set; }                 // in minutes
        public int NumberOfQuestions { get; protected set; }
        public Question[] Questions { get; protected set; }
        public Dictionary<Question, Answer> QuestionAnswerDictionary { get; private set; }
        public Subject Subject { get; protected set; }

        private ExamMode _mode;
        public ExamMode Mode
        {
            get => _mode;
            protected set
            {
                _mode = value;
                if (_mode == ExamMode.Starting)
                    OnExamStarted(new ExamEventArgs(Subject, this));
            }
        }

        // Event
        public event ExamStartedHandler ExamStarted;

        protected Exam(int time, int numberOfQuestions, Subject subject)
        {
            if (time <= 0)             throw new ArgumentException("Time must be > 0.");
            if (numberOfQuestions <= 0) throw new ArgumentException("NumberOfQuestions must be > 0.");
            if (subject == null)        throw new ArgumentNullException(nameof(subject));

            Time                      = time;
            NumberOfQuestions         = numberOfQuestions;
            Subject                   = subject;
            Questions                 = new Question[numberOfQuestions];
            QuestionAnswerDictionary  = new Dictionary<Question, Answer>();
            _mode                     = ExamMode.Queued;
        }

        public void AddQuestion(int index, Question question)
        {
            if (index < 0 || index >= NumberOfQuestions)
                throw new IndexOutOfRangeException("Question index out of range.");
            Questions[index] = question ?? throw new ArgumentNullException(nameof(question));
        }

        public abstract void ShowExam();

        public virtual void Start()
        {
            Console.WriteLine($"\n=== Exam Starting ({GetType().Name}) | Subject: {Subject.Name} | Time: {Time} min ===");
            Mode = ExamMode.Starting;   
        }

        public virtual void Finish()
        {
            Mode = ExamMode.Finished;
            Console.WriteLine("\n=== Exam Finished ===");
        }

        public void CorrectExam()
        {
            int score = 0;
            int total = 0;

            foreach (var question in Questions)
            {
                if (question == null) continue;

                if (QuestionAnswerDictionary.TryGetValue(question, out Answer studentAnswer))
                {
                    bool correct = question.CheckAnswer(studentAnswer);
                    if (correct) score += question.Marks;
                }
                total += question.Marks;
            }

            Console.WriteLine($"\n--- Result: {score} / {total} ---");
        }

        protected void OnExamStarted(ExamEventArgs e)
        {
            ExamStarted?.Invoke(this, e);
            Subject.NotifyStudents(this);
        }

        public int CompareTo(Exam other)
        {
            if (other == null) return 1;
            int cmp = this.Time.CompareTo(other.Time);
            if (cmp != 0) return cmp;
            return this.NumberOfQuestions.CompareTo(other.NumberOfQuestions);
        }

        public object Clone()
        {
            Exam cloned = (Exam)this.MemberwiseClone();

            cloned.Questions = new Question[this.Questions.Length];
            for (int i = 0; i < this.Questions.Length; i++)
                cloned.Questions[i] = this.Questions[i];

            
            cloned.QuestionAnswerDictionary = new Dictionary<Question, Answer>();
            foreach (var pair in this.QuestionAnswerDictionary)
                cloned.QuestionAnswerDictionary[pair.Key] = pair.Value;

            return cloned;
        }

        public override bool Equals(object obj)
        {
            if (obj is Exam other)
                return this.Time == other.Time &&
                       this.NumberOfQuestions == other.NumberOfQuestions &&
                       this.Subject.Name == other.Subject.Name;
            return false;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Time, NumberOfQuestions, Subject.Name);

        public override string ToString() =>
            $"{GetType().Name} | Subject: {Subject.Name} | Time: {Time}min | Questions: {NumberOfQuestions} | Mode: {Mode}";
    }
}
