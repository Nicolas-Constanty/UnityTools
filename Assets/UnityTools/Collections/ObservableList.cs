using System;
using System.Collections;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace UnityTools.Collections
{
    [Serializable]
    public class ObservableList<T> : IList<T>, INotifyCollectionChanged
    {
        private readonly List<T> m_List = new List<T>();
        public IEnumerator<T> GetEnumerator()
        {
            return m_List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            m_List.Add(item);
            if (CollectionChanged != null)
                CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void Clear()
        {
            m_List.Clear();
            if (CollectionChanged != null)
                CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(T item)
        {
            return m_List.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_List.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            bool remove = m_List.Remove(item);
            if (CollectionChanged != null)
                CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return remove;
        }

        public int Count
        {
            get { return m_List.Count; }
        }

        public bool IsReadOnly
        {
            //https://msdn.microsoft.com/fr-fr/library/bb346454(v=vs.110).aspx
            get { return false; }
        }
        public int IndexOf(T item)
        {
            return m_List.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            m_List.Insert(index, item);
            if (CollectionChanged != null)
                CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));

        }

        public void RemoveAt(int index)
        {
            T elem = m_List[index];
            m_List.RemoveAt(index);
            if (CollectionChanged != null)
                CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, elem, index));
        }

        public T this[int index]
        {
            get { return m_List[index]; }
            set { m_List[index] = value; }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }

    public interface IQueue<T>
    {
        int Count { get; }

        void Clear();
        bool Contains(T item);
        void CopyTo(T[] array, int idx);
        T Dequeue();
        void Enqueue(T item);
        T Peek();
        T[] ToArray();
        void TrimExcess();
    }

    public interface IStack<T>
    {
        int Count { get; }

        void Clear();
        bool Contains(T t);
        void CopyTo(T[] dest, int idx);
        T Peek();
        T Pop();
        void Push(T t);
        T[] ToArray();
        void TrimExcess();
    }

    public class ObservableStack<T> : IStack<T>, INotifyCollectionChanged
    {
        private readonly Stack<T> m_Stack = new Stack<T>();

        public int Count { get { return m_Stack.Count; } }
        public void Clear()
        {
            m_Stack.Clear();
            if (CollectionChanged != null)
                CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(T t)
        {
            return m_Stack.Contains(t);
        }

        public void CopyTo(T[] dest, int idx)
        {
            m_Stack.CopyTo(dest, idx);
        }

        public T Peek()
        {
            return m_Stack.Peek();
        }

        public T Pop()
        {
            T item = m_Stack.Pop();
            if (CollectionChanged != null)
                CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return item;
        }

        public void Push(T t)
        {
            m_Stack.Push(t);
            if (CollectionChanged != null)
                CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, t));
        }

        public T[] ToArray()
        {
            return m_Stack.ToArray();
        }

        public void TrimExcess()
        {
            m_Stack.TrimExcess();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }

    public class ObservableQueue<T> : IQueue<T>, INotifyCollectionChanged
    {
        private readonly Queue<T> m_Queue = new Queue<T>();
        public int Count { get { return m_Queue.Count;} }

        public void Clear()
        {
            m_Queue.Clear();
            if (CollectionChanged != null)
                CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(T item)
        {
            return m_Queue.Contains(item);
        }

        public void CopyTo(T[] array, int idx)
        {
            m_Queue.CopyTo(array, idx);
        }

        public T Dequeue()
        {
            T item = m_Queue.Dequeue();
            if (CollectionChanged != null)
                CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return item;
        }

        public void Enqueue(T item)
        {
            m_Queue.Enqueue(item);
            if (CollectionChanged != null)
                CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public T Peek()
        {
            return m_Queue.Peek();
        }

        public T[] ToArray()
        {
            return m_Queue.ToArray();
        }

        public void TrimExcess()
        {
            m_Queue.TrimExcess();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}


