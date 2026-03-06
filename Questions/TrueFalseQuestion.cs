using System;

namespace ExamSystem
{
    public class TrueFalseQuestion : Question
    {
        public TrueFalseQuestion(string header, string body, int marks, Answer correctAnswer)
            : base(header, body, marks)
        {
            // Only 2 possible answers: True / False
            Answers.Add(new Answer(1, "True"));
            Answers.Add(new Answer(2, "False"));

            CorrectAnswer = correctAnswer ?? throw new ArgumentNullException(nameof(correctAnswer));
        }

        public override void Display()
        {
            Console.WriteLine($"\n{Header}: {Body}  [{Marks} mark(s)]");
            for (int i = 0; i < Answers.Count; i++)
                Console.WriteLine($"  {Answers[i]}");
        }

        public override bool CheckAnswer(Answer studentAnswer)
        {
            if (studentAnswer == null) return false;
            return studentAnswer.Equals(CorrectAnswer);
        }

        public override string ToString() =>
            $"[True/False] {base.ToString()}";
    }
}
