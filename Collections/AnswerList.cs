using System;

namespace ExamSystem
{
    public class AnswerList
    {
        private Answer[] _answers;
        private int _count;

        public int Count => _count;

        public AnswerList()
        {
            _answers = new Answer[10];
            _count = 0;
        }

        public void Add(Answer answer)
        {
            if (answer == null) throw new ArgumentNullException(nameof(answer));

            if (_count == _answers.Length)
                Resize();

            _answers[_count++] = answer;
        }

        public Answer GetById(int id)
        {
            for (int i = 0; i < _count; i++)
                if (_answers[i].Id == id)
                    return _answers[i];
            return null;
        }

        // Indexer is hereeee
        public Answer this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();
                return _answers[index];
            }
        }

        private void Resize()
        {
            Answer[] bigger = new Answer[_answers.Length * 2];
            for (int i = 0; i < _count; i++)
                bigger[i] = _answers[i];
            _answers = bigger;
        }
    }
}
