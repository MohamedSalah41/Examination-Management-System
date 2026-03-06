using System;
using System.Collections.Generic;
using System.IO;

namespace ExamSystem
{
    public class QuestionList : List<Question>
    {
        private readonly string _logFile;

        public QuestionList(string logFile)
        {
            if (string.IsNullOrWhiteSpace(logFile))
                throw new ArgumentException("Log file name cannot be empty.");
            _logFile = logFile;
        }

        // Override Add to also log the question to file
        public new void Add(Question question)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));

            base.Add(question);

            // Log to file in append mode
            using (StreamWriter writer = new StreamWriter(_logFile, append: true))
            {
                writer.WriteLine("--------------------------------------------------");
                writer.WriteLine($"Logged at: {DateTime.Now}");
                writer.WriteLine(question.ToString());
                writer.WriteLine($"Marks: {question.Marks}");
                writer.WriteLine();
            }
        }
    }
}
