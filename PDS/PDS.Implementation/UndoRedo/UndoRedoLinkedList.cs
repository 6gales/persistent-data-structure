using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using PDS.Collections;
using PDS.Implementation.Collections;
using PDS.UndoRedo;

namespace PDS.Implementation.UndoRedo
{
    public class UndoRedoLinkedList<T> : IUndoRedoLinkedList<T>
    {
        private readonly IPersistentLinkedList<T> _persistentLinkedList;
        private readonly IPersistentStack<IPersistentLinkedList<T>> _undoStack;
        private readonly IPersistentStack<IPersistentLinkedList<T>> _redoStack;

        public UndoRedoLinkedList()
        {
            _persistentLinkedList = PersistentLinkedList<T>.Empty;
            _undoStack = PersistentStack<IPersistentLinkedList<T>>.Empty;
            _redoStack = PersistentStack<IPersistentLinkedList<T>>.Empty;
        }
        
        private UndoRedoLinkedList(IPersistentLinkedList<T> persistentLinkedList, IPersistentStack<IPersistentLinkedList<T>> undoStack,
            IPersistentStack<IPersistentLinkedList<T>> redoStack)
        {
            _persistentLinkedList = persistentLinkedList;
            _undoStack = undoStack;
            _redoStack = redoStack;
        }

        public IEnumerator<T> GetEnumerator() => _persistentLinkedList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _persistentLinkedList.Count;
        public T First => _persistentLinkedList.First;
        public T Last => _persistentLinkedList.Last;
        IPersistentLinkedList<T> IPersistentLinkedList<T>.AddFirst(T item)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddFirst(item), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoLinkedList<T> IUndoRedoLinkedList<T>.AddLast(T item)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddLast(item), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoLinkedList<T> IUndoRedoLinkedList<T>.RemoveFirst()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveFirst(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoLinkedList<T> IUndoRedoLinkedList<T>.RemoveLast()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveLast(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoLinkedList<T> IUndoRedoLinkedList<T>.Add(T item)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Add(item), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoLinkedList<T> IUndoRedoLinkedList<T>.AddRange(IEnumerable<T> items)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddRange(items), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoLinkedList<T> IUndoRedoLinkedList<T>.AddRange(IReadOnlyCollection<T> items)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddRange(items), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IPersistentLinkedList<T> Clear()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Clear(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoLinkedList<T> IUndoRedoLinkedList<T>.Insert(int index, T item)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Insert(index, item), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoLinkedList<T> IUndoRedoLinkedList<T>.Remove(T value, IEqualityComparer<T>? equalityComparer)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Remove(value, equalityComparer), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoLinkedList<T> IUndoRedoLinkedList<T>.RemoveAll(Predicate<T> match)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveAll(match), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoLinkedList<T> IUndoRedoLinkedList<T>.RemoveAt(int index)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveAt(index), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoLinkedList<T> IUndoRedoLinkedList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveRange(items, equalityComparer), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoLinkedList<T> IUndoRedoLinkedList<T>.RemoveRange(int index, int count)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveRange(index, count), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoLinkedList<T> IUndoRedoLinkedList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Replace(oldValue, newValue, equalityComparer), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoLinkedList<T> IUndoRedoLinkedList<T>.SetItem(int index, T value)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.SetItem(index, value), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoLinkedList<T> IUndoRedoLinkedList<T>.AddFirst(T item)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddFirst(item), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.AddLast(T item)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddLast(item), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.RemoveFirst()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveFirst(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.RemoveLast()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveLast(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public bool Contains(T item) => _persistentLinkedList.Contains(item);

        public T Get(int index) => _persistentLinkedList.Get(index);
        IPersistentLinkedList<T> IPersistentLinkedList<T>.Add(T item)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Add(item), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoDataStructure<T, IUndoRedoStack<T>> 
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoStack<T>>>.AddRange(IEnumerable<T> items)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddRange(items), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoDataStructure<T, IUndoRedoStack<T>> 
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoStack<T>>>.AddRange(IReadOnlyCollection<T> items)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddRange(items), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoDataStructure<T, IUndoRedoStack<T>> 
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoStack<T>>>.Clear()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Clear(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        bool IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoStack<T>>>.IsEmpty => _persistentLinkedList.IsEmpty;

        IUndoRedoDataStructure<T, IUndoRedoStack<T>> 
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoStack<T>>>.Add(T value)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Add(value), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.AddRange(IEnumerable<T> items)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddRange(items), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.AddRange(IReadOnlyCollection<T> items)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddRange(items), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) =>
            _persistentLinkedList.IndexOf(item, index, count, equalityComparer);

        IPersistentLinkedList<T> IPersistentLinkedList<T>.Insert(int index, T item)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Insert(index, item), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.Remove(T value, IEqualityComparer<T>? equalityComparer)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Remove(value, equalityComparer), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.RemoveAll(Predicate<T> match)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveAll(match), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.RemoveAt(int index)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveAt(index), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveRange(items, equalityComparer), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.RemoveRange(int index, int count)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveRange(index, count), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Replace(oldValue, newValue, equalityComparer), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.SetItem(int index, T value)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.SetItem(index, value), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentDataStructure<T, IPersistentLinkedList<T>>.Add(T value)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Add(value), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.AddRange(IEnumerable<T> items)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddRange(items), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.AddRange(IReadOnlyCollection<T> items)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddRange(items), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentStack<T> IPersistentStack<T>.Clear()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Clear(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        bool IPersistentStack<T>.IsEmpty => _persistentLinkedList.IsEmpty;

        IPersistentStack<T> IPersistentStack<T>.Push(T value)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddLast(value), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.Clear()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Clear(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        T IImmutableStack<T>.Peek() => _persistentLinkedList.Last;

        IPersistentStack<T> IPersistentStack<T>.Pop()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveLast(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IImmutableStack<T> IImmutableStack<T>.Pop()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveLast(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IImmutableStack<T> IImmutableStack<T>.Push(T value)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddLast(value), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        bool IImmutableStack<T>.IsEmpty => _persistentLinkedList.IsEmpty;

        bool IPersistentDataStructure<T, IPersistentStack<T>>.IsEmpty => _persistentLinkedList.IsEmpty;

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.Add(T value)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Add(value), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentDataStructure<T, IPersistentLinkedList<T>>.AddRange(IEnumerable<T> items)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddRange(items), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentDataStructure<T, IPersistentLinkedList<T>>.AddRange(IReadOnlyCollection<T> items)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddFirst(items), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentDataStructure<T, IPersistentLinkedList<T>>.Clear()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Clear(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IImmutableQueue<T> Dequeue()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveFirst(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IImmutableQueue<T> Enqueue(T value)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddLast(value), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IImmutableStack<T> IImmutableStack<T>.Clear()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Clear(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        T IImmutableQueue<T>.Peek() => _persistentLinkedList.First;

        bool IImmutableQueue<T>.IsEmpty => _persistentLinkedList.IsEmpty;

        IImmutableQueue<T> IImmutableQueue<T>.Clear()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Clear(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        bool IPersistentDataStructure<T, IPersistentLinkedList<T>>.IsEmpty => _persistentLinkedList.IsEmpty;

        public IUndoRedoDataStructure<T, IUndoRedoStack<T>> Redo()
        {
            if (_redoStack.IsEmpty)
            {
                throw new InvalidOperationException("Redo stack is empty");
            }

            var lastVersion = _redoStack.Peek();
            var r = _redoStack.Pop();
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(lastVersion, u, r);
        }

        public bool TryRedo(out IUndoRedoDataStructure<T, IUndoRedoStack<T>> collection)
        {
            if (_redoStack.IsEmpty)
            {
                collection = this;
                return false;
            }

            collection = Redo();
            return true;
        }

        public bool CanRedo => !_redoStack.IsEmpty;
        public IUndoRedoDataStructure<T, IUndoRedoStack<T>> Undo()
        {
            if (_undoStack.IsEmpty)
            {
                throw new InvalidOperationException("Undo stack is empty");
            }

            var lastVersion = _undoStack.Peek();
            var u = _undoStack.Pop();
            var r = _redoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(lastVersion, u, r);
        }

        public bool TryUndo(out IUndoRedoDataStructure<T, IUndoRedoStack<T>> collection)
        {
            if (_undoStack.IsEmpty)
            {
                collection = this;
                return false;
            }

            collection = Undo();
            return true;
        }

        public bool CanUndo => !_undoStack.IsEmpty;
    }
}