using System;

namespace ExamSystem
{
    
    public class ChooseAllQuestion : Question
    {
        public AnswerList CorrectAnswers { get; private set; }

        public ChooseAllQuestion(string header, string body, int marks, AnswerList answers, AnswerList correctAnswers)
            : base(header, body, marks)
        {
            if (answers == null || answers.Count < 2)
                throw new ArgumentException("ChooseAll question needs at least 2 answers.");
            if (correctAnswers == null || correctAnswers.Count < 1)
                throw new ArgumentException("ChooseAll question needs at least 1 correct answer.");

            Answers        = answers;
            CorrectAnswers = correctAnswers;

            CorrectAnswer  = correctAnswers[0];
        }

        public override void Display()
        {
            Console.WriteLine($"\n{Header}: {Body}  [{Marks} mark(s)]  (Choose all that apply)");
            for (int i = 0; i < Answers.Count; i++)
                Console.WriteLine($"  {Answers[i]}");
        }

        public override bool CheckAnswer(Answer studentAnswer)
        {
            if (studentAnswer == null) return false;

            string[] parts = studentAnswer.Text.Split(',');
            int[] studentIds = new int[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                if (!int.TryParse(parts[i].Trim(), out studentIds[i]))
                    return false;
            }

            if (studentIds.Length != CorrectAnswers.Count)
                return false;

            for (int i = 0; i < CorrectAnswers.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < studentIds.Length; j++)
                {
                    if (studentIds[j] == CorrectAnswers[i].Id)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) return false;
            }
            return true;
        }

        public override string ToString() =>
            $"[ChooseAll] {base.ToString()}";
    }
}
