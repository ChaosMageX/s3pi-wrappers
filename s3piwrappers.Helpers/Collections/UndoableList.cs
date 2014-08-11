using System;
using System.Collections;
using System.Collections.Generic;
using s3piwrappers.Helpers.Undo;

namespace s3piwrappers.Helpers.Collections
{
    public class UndoableList<T> : IList<T>, IList, IHasGenericInsert, 
        IHasReverse, IHasSwap, IHasUndoManager
    {
        [Flags]
        public enum Actions : byte
        {
            None = 0x00,
            Add = 0x01,
            Insert = 0x02,
            AddNew = 0x04,
            InsertNew = 0x08,
            Remove = 0x10,
            Clear = 0x20,
            All = 0x3f
        }

        private static readonly T[] sEmpty = new T[0];
        private static readonly EqualityComparer<T> sEC 
            = EqualityComparer<T>.Default;

        private UndoManager mUndoManager;
        private Actions mUndoActions;
        private T[] mItems;
        private int mSize;
        private int mVersion;
        private object mSyncRoot;

        public UndoableList()
        {
            this.mItems = sEmpty;
        }

        public UndoManager UndoManager
        {
            get { return this.mUndoManager; }
        }

        public bool GetUndoOn(Actions action)
        {
            return (this.mUndoActions & action) != Actions.None;
        }

        private void EnsureCapacity()
        {
            if (this.mSize == this.mItems.Length)
            {
                if (this.mSize == 0)
                {
                    this.mItems = new T[4];
                }
                else
                {
                    T[] items = new T[2 * this.mSize];
                    Array.Copy(this.mItems, 0, items, 0, this.mSize);
                    this.mItems = items;
                }
            }
        }

        private void Add(T item, bool viaCmd)
        {
            if (viaCmd)
            {
            }
            else
            {
                this.EnsureCapacity();
                this.mItems[this.mSize++] = item;
                this.mVersion++;
            }
        }

        private void Insert(int index, T item, bool viaCmd)
        {
            if (index < 0 || index > this.mSize)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (viaCmd)
            {
            }
            else
            {
                this.EnsureCapacity();
                if (index < this.mSize)
                {
                    Array.Copy(this.mItems, index, this.mItems, index + 1, 
                               this.mSize - index);
                }
                this.mItems[index] = item;
                this.mSize++;
                this.mVersion++;
            }
        }

        private void RemoveAt(int index, bool viaCmd)
        {
            if (index < 0 || index >= this.mSize)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (viaCmd)
            {
            }
            else
            {
                this.mSize--;
                if (index < this.mSize)
                {
                    Array.Copy(this.mItems, index + 1, this.mItems, index, 
                               this.mSize - index);
                }
                this.mItems[this.mSize] = default(T);
                this.mVersion++;
            }
        }

        private void Clear(bool viaCmd)
        {
            if (this.mSize > 0)
            {
                if (viaCmd)
                {
                }
                else
                {
                    Array.Clear(this.mItems, 0, this.mSize);
                    this.mSize = 0;
                    this.mVersion++;
                }
            }
        }

        public void Swap(int index1, int index2)
        {
            if (index1 < 0 || index1 >= this.mSize)
            {
                throw new ArgumentOutOfRangeException("index1");
            }
            if (index2 != index1)
            {
                if (index2 < 0 || index2 >= this.mSize)
                {
                    throw new ArgumentOutOfRangeException("index2");
                }
                T item = this.mItems[index1];
                this.mItems[index1] = this.mItems[index2];
                this.mItems[index2] = item;
                this.mVersion++;
            }
        }

        public void Reverse()
        {
            Array.Reverse(this.mItems, 0, this.mSize);
            this.mVersion++;
        }

        public void Reverse(int index, int count)
        {
            if (this.mSize - index < count)
            {
                throw new ArgumentException("Invalid Offset Length");
            }
            Array.Reverse(this.mItems, index, count);
            this.mVersion++;
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf<T>(this.mItems, item, 0, this.mSize);
        }

        public int IndexOf(T item, int index)
        {
            if (index > this.mSize)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            return Array.IndexOf<T>(this.mItems, item, index, 
                                    this.mSize - index);
        }

        public int IndexOf(T item, int index, int count)
        {
            if (index > this.mSize)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (count < 0 || this.mSize - count < index)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return Array.IndexOf<T>(this.mItems, item, index, count);
        }

        public void Insert(int index, T item)
        {
            this.Insert(index, item, this.GetUndoOn(Actions.Insert));
        }

        public void RemoveAt(int index)
        {
            this.RemoveAt(index, this.GetUndoOn(Actions.Remove));
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= this.mSize)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                return this.mItems[index];
            }
            set
            {
                if (index < 0 || index >= this.mSize)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                if (!sEC.Equals(value, this.mItems[index]))
                {
                    this.mItems[index] = value;
                    this.mVersion++;
                }
            }
        }

        public void Add(T item)
        {
            this.Add(item, this.GetUndoOn(Actions.Add));
        }

        public void Clear()
        {
            this.Clear(this.GetUndoOn(Actions.Clear));
        }

        public bool Contains(T item)
        {
            int i;
            if (item == null)
            {
                for (i = 0; i < this.mSize; i++)
                {
                    if (this.mItems[i] == null)
                    {
                        return true;
                    }
                }
                return false;
            }
            for (i = 0; i < this.mSize; i++)
            {
                if (sEC.Equals(this.mItems[i], item))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array)
        {
            Array.Copy(this.mItems, 0, array, 0, this.mSize);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(this.mItems, 0, array, arrayIndex, this.mSize);
        }

        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            if (this.mSize - index < count)
            {
                throw new ArgumentException("Invalid offset length");
            }
            Array.Copy(this.mItems, index, array, arrayIndex, count);
        }

        public int Count
        {
            get { return this.mSize; }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            int index = Array.IndexOf<T>(this.mItems, item, 0, this.mSize);
            if (index >= 0)
            {
                this.RemoveAt(index, this.GetUndoOn(Actions.Remove));
                return true;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Add()
        {
            throw new NotImplementedException();
        }

        public bool Insert(int index)
        {
            throw new NotImplementedException();
        }

        private static bool isCompatibleObject(object value)
        {
            return value is T || (value == null && default(T) == null);
        }

        int IList.Add(object value)
        {
            if (value == null && default(T) != null)
            {
                throw new ArgumentNullException("value");
            }
            this.Add((T)value, this.GetUndoOn(Actions.Add));
            return this.mSize - 1;
        }

        bool IList.Contains(object value)
        {
            return isCompatibleObject(value) && this.Contains((T)value);
        }

        int IList.IndexOf(object value)
        {
            if (isCompatibleObject(value))
            {
                return this.IndexOf((T)value);
            }
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            if (value == null && default(T) != null)
            {
                throw new ArgumentNullException("value");
            }
            this.Insert(index, (T)value, this.GetUndoOn(Actions.Insert));
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        void IList.Remove(object value)
        {
            if (isCompatibleObject(value))
            {
                this.Remove((T)value);
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (T)value;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array != null && array.Rank != 1)
            {
                throw new ArgumentException(
                    "Multidimensional arrays are not supported", "array");
            }
            Array.Copy(this.mItems, 0, array, index, this.mSize);
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (this.mSyncRoot == null)
                {
                    System.Threading.Interlocked.CompareExchange(
                        ref this.mSyncRoot, new object(), null);
                }
                return this.mSyncRoot;
            }
        }
    }
}
