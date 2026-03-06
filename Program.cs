using System;

namespace ExamSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("    EXAMINATION MANAGEMENT SYSTEM");
            Console.WriteLine("========================================\n");

            Subject subject = new Subject("C# Programming");

            Student s1 = new Student("Ali Hassan",   1);
            Student s2 = new Student("Sara Mohamed", 2);
            Student s3 = new Student("Omar Tarek",   3);

            subject.Enroll(s1);
            subject.Enroll(s2);
            subject.Enroll(s3);

            Console.WriteLine(subject);


            Answer trueAnswer  = new Answer(1, "True");
            Answer falseAnswer = new Answer(2, "False");
            TrueFalseQuestion q1 = new TrueFalseQuestion(
                "Q1",
                "C# is an object-oriented language.",
                2,
                trueAnswer
            );

            AnswerList chooseOneAnswers = new AnswerList();
            chooseOneAnswers.Add(new Answer(1, "int"));
            chooseOneAnswers.Add(new Answer(2, "string"));
            chooseOneAnswers.Add(new Answer(3, "bool"));
            chooseOneAnswers.Add(new Answer(4, "float"));
            ChooseOneQuestion q2 = new ChooseOneQuestion(
                "Q2",
                "Which type stores whole numbers in C#?",
                3,
                chooseOneAnswers,
                chooseOneAnswers.GetById(1)   
            );

            AnswerList chooseAllAnswers = new AnswerList();
            chooseAllAnswers.Add(new Answer(1, "Abstract"));
            chooseAllAnswers.Add(new Answer(2, "Private"));
            chooseAllAnswers.Add(new Answer(3, "Override"));
            chooseAllAnswers.Add(new Answer(4, "Goto"));

            AnswerList correctOnes = new AnswerList();
            correctOnes.Add(new Answer(1, "Abstract"));
            correctOnes.Add(new Answer(3, "Override"));

            ChooseAllQuestion q3 = new ChooseAllQuestion(
                "Q3",
                "Which of these are valid OOP keywords in C#? (select all that apply)",
                5,
                chooseAllAnswers,
                correctOnes
            );

            PracticeExam practiceExam = new PracticeExam(30, 3, subject);
            practiceExam.AddQuestion(0, q1);
            practiceExam.AddQuestion(1, q2);
            practiceExam.AddQuestion(2, q3);

            FinalExam finalExam = new FinalExam(60, 3, subject);
            finalExam.AddQuestion(0, q1);
            finalExam.AddQuestion(1, q2);
            finalExam.AddQuestion(2, q3);

            QuestionList practiceLog = new QuestionList("practice_questions.log");
            practiceLog.Add(q1);
            practiceLog.Add(q2);
            practiceLog.Add(q3);
            Console.WriteLine("\nQuestions logged to 'practice_questions.log'");

            Repository<Exam> examRepo = new Repository<Exam>();
            examRepo.Add(practiceExam);
            examRepo.Add(finalExam);
            examRepo.Sort();
            Console.WriteLine("\nExam repository sorted by time:");
            foreach (var e in examRepo.GetAll())
                Console.WriteLine("  " + e);

            // ── 7. Ask user to select exam type ────────────────────
            Console.WriteLine("\n\nSelect Exam Type:");
            Console.WriteLine("  1 - Practice Exam");
            Console.WriteLine("  2 - Final Exam");
            Console.Write("Enter choice: ");
            string choice = Console.ReadLine();

            Exam selectedExam = choice == "2" ? (Exam)finalExam : practiceExam;

            // ── 8. Run the selected exam ───────────────────────────
            selectedExam.Start();       // Mode -> Starting -> fires event -> notifies students
            selectedExam.ShowExam();    // Ask questions, collect answers
            selectedExam.Finish();      // Mode -> Finished, show results

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
