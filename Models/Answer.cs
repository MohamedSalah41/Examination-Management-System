using System;

namespace ExamSystem
{
    public class Answer : IComparable<Answer>
    {
        public int Id { get; private set; }
        public string Text { get; private set; }

        public Answer(int id, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Answer text cannot be empty.");
            Id = id;
            Text = text;
        }

        public int CompareTo(Answer other)
        {
            if (other == null) return 1;
            return this.Id.CompareTo(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (obj is Answer other)
                return this.Id == other.Id;
            return false;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => $"[{Id}] {Text}";
    }
}
