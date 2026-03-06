using System;

namespace ExamSystem
{
    public class PracticeExam : Exam
    {
        public PracticeExam(int time, int numberOfQuestions, Subject subject)
            : base(time, numberOfQuestions, subject) { }

        public override void ShowExam()
        {
            Console.WriteLine("\n====== PRACTICE EXAM ======");
            Console.WriteLine(ToString());

            foreach (var question in Questions)
            {
                if (question == null) continue;

                question.Display();

                // Get student answer
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

            Console.WriteLine("\n--- Review ---");
            int score = 0, total = 0;

            foreach (var question in Questions)
            {
                if (question == null) continue;
                total += question.Marks;
                Console.WriteLine(question.ToString());

                if (QuestionAnswerDictionary.TryGetValue(question, out Answer studentAnswer))
                {
                    bool correct = question.CheckAnswer(studentAnswer);
                    Console.WriteLine($"  Your answer : {studentAnswer}");

                    if (question is ChooseAllQuestion chooseAll)
                    {
                        Console.Write("  Correct answers: ");
                        for (int i = 0; i < chooseAll.CorrectAnswers.Count; i++)
                            Console.Write(chooseAll.CorrectAnswers[i] + " ");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine($"  Correct answer: {question.CorrectAnswer}");
                    }

                    Console.WriteLine($"  Result: {(correct ? "✓ Correct" : "✗ Wrong")}");
                    if (correct) score += question.Marks;
                }
            }

            Console.WriteLine($"\n=== Final Grade: {score} / {total} ===");
        }
    }
}
