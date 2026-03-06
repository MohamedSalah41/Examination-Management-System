using System;

namespace ExamSystem
{
    public class ChooseOneQuestion : Question
    {
        public ChooseOneQuestion(string header, string body, int marks, AnswerList answers, Answer correctAnswer)
            : base(header, body, marks)
        {
            if (answers == null || answers.Count < 2)
                throw new ArgumentException("ChooseOne question needs at least 2 answers.");

            Answers       = answers;
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
            $"[ChooseOne] {base.ToString()}";
    }
}
