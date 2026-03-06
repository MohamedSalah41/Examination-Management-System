using System;

namespace ExamSystem
{
    public class Repository<T> where T : ICloneable, IComparable<T>
    {
        private T[] _items;
        private int _count;

        public Repository()
        {
            _items = new T[10];
            _count = 0;
        }

        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (_count == _items.Length) Resize();
            _items[_count++] = item;
        }

        public void Remove(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_items[i].CompareTo(item) == 0)
                {
                    for (int j = i; j < _count - 1; j++)
                        _items[j] = _items[j + 1];
                    _items[--_count] = default;
                    return;
                }
            }
        }

        public void Sort()
        {
            for (int i = 0; i < _count - 1; i++)
                for (int j = 0; j < _count - i - 1; j++)
                    if (_items[j].CompareTo(_items[j + 1]) > 0)
                    {
                        T temp     = _items[j];
                        _items[j]  = _items[j + 1];
                        _items[j + 1] = temp;
                    }
        }

        public T[] GetAll()
        {
            T[] result = new T[_count];
            for (int i = 0; i < _count; i++)
                result[i] = _items[i];
            return result;
        }

        private void Resize()
        {
            T[] bigger = new T[_items.Length * 2];
            for (int i = 0; i < _count; i++)
                bigger[i] = _items[i];
            _items = bigger;
        }
    }
}
