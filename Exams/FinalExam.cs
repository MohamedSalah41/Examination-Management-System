using System;

namespace ExamSystem
{
    public class FinalExam : Exam
    {
        public FinalExam(int time, int numberOfQuestions, Subject subject)
            : base(time, numberOfQuestions, subject) { }

        public override void ShowExam()
        {
            Console.WriteLine("\n====== FINAL EXAM ======");
            Console.WriteLine(ToString());

            foreach (var question in Questions)
            {
                if (question == null) continue;

                question.Display();

                Console.Write("  Your answer (enter ID): ");
                string input = Console.ReadLine();

                Answer studentAnswer;

                if (question is ChooseAllQuestion)
                {
                    studentAnswer = new Answer(0, input ?? "");
                }
                else
                {
                    int.TryParse(input, out int id);
                    studentAnswer = question.Answers.GetById(id);
                    if (studentAnswer == null)
                    {
                        Console.WriteLine("  Invalid answer.");
                        studentAnswer = new Answer(0, "invalid");
                    }
                }

                QuestionAnswerDictionary[question] = studentAnswer;
            }
        }

        public override void Finish()
        {
            base.Finish();

            Console.WriteLine("\n--- Your Submitted Answers ---");
            foreach (var question in Questions)
            {
                if (question == null) continue;
                Console.WriteLine(question.ToString());

                if (QuestionAnswerDictionary.TryGetValue(question, out Answer studentAnswer))
                    Console.WriteLine($"  Your answer: {studentAnswer}");
                else
                    Console.WriteLine("  No answer submitted.");
            }

            Console.WriteLine("\n(Results will be announced later.)");
        }
    }
}
