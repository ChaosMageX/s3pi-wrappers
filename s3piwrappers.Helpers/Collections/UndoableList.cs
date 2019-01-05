using System;
using System.Collections;
using System.Collections.Generic;
using s3piwrappers.Helpers.Undo;

namespace s3piwrappers.Helpers.Collections
{
    public class UndoableList<T> : IList<T>, IList, IHasGenericInsert, 
        IHasReverse, IHasSwap, IHasUndoManager
    {
        /*[Flags]
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
        }/* */

        private static readonly T[] sEmpty = new T[0];
        private static readonly EqualityComparer<T> sEC 
            = EqualityComparer<T>.Default;

        private UndoManager mUndoManager;
        //private Actions mUndoActions;
        private T[] mItems;
        private int mSize;
        private int mVersion;
        private object mSyncRoot;

        public UndoableList(UndoManager manager)
        {
            this.mItems = sEmpty;
            this.mUndoManager = manager;
        }

        public UndoManager UndoManager
        {
            get { return this.mUndoManager; }
        }

        /*public bool GetUndoOn(Actions action)
        {
            return (this.mUndoActions & action) != Actions.None;
        }/* */

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

        #region Addition and Insertion Functions

        private class AddInsertCommand : Command
        {
            private UndoableList<T> mList;
            private int mIndex;
            private T mItem;

            public AddInsertCommand(UndoableList<T> list, int index, T item)
            {
                this.mList = list;
                this.mIndex = index;
                this.mItem = item;
                if (index == -1)
                {
                    this.mLabel = "Add item to list";
                }
                else
                {
                    this.mLabel = string.Concat("Insert item in list at index ", index.ToString());
                }
            }

            public override bool Execute()
            {
                if (this.mIndex == -1)
                {
                    this.mList.mItems[this.mList.mSize++] = this.mItem;
                }
                else
                {
                    if (this.mIndex < this.mList.mSize)
                    {
                        Array.Copy(this.mList.mItems, this.mIndex, 
                                   this.mList.mItems, this.mIndex + 1,
                                   this.mList.mSize - this.mIndex);
                    }
                    this.mList.mItems[this.mIndex] = this.mItem;
                    this.mList.mSize++;
                }
                this.mList.mVersion++;
                return true;
            }

            public override void Undo()
            {
                this.mList.mSize--;
                if (this.mIndex != -1 && this.mIndex < this.mList.mSize)
                {
                    Array.Copy(this.mList.mItems, this.mIndex + 1, 
                               this.mList.mItems, this.mIndex,
                               this.mList.mSize - this.mIndex);
                }
                this.mList.mItems[this.mList.mSize] = default(T);
                this.mList.mVersion--;
            }
        }

        private void Add(T item, bool viaCmd)
        {
            this.EnsureCapacity();
            if (viaCmd)
            {
                this.mUndoManager.Submit(new AddInsertCommand(this, -1, item));
            }
            else
            {
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
            this.EnsureCapacity();
            if (viaCmd)
            {
                this.mUndoManager.Submit(new AddInsertCommand(this, index, item));
            }
            else
            {
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

        #endregion

        #region Removal Functions

        private class RemoveAtCommand : Command
        {
            private UndoableList<T> mList;
            private int mIndex;
            private T mItem;

            public RemoveAtCommand(UndoableList<T> list, int index)
            {
                this.mList = list;
                this.mIndex = index;
                this.mItem = this.mList.mItems[index];
            }

            public override bool Execute()
            {
                this.mList.mSize--;
                if (this.mIndex != -1 && this.mIndex < this.mList.mSize)
                {
                    Array.Copy(this.mList.mItems, this.mIndex + 1,
                               this.mList.mItems, this.mIndex,
                               this.mList.mSize - this.mIndex);
                }
                this.mList.mItems[this.mList.mSize] = default(T);
                this.mList.mVersion++;
                return true;
            }

            public override void Undo()
            {
                if (this.mIndex == -1)
                {
                    this.mList.mItems[this.mList.mSize++] = this.mItem;
                }
                else
                {
                    if (this.mIndex < this.mList.mSize)
                    {
                        Array.Copy(this.mList.mItems, this.mIndex,
                                   this.mList.mItems, this.mIndex + 1,
                                   this.mList.mSize - this.mIndex);
                    }
                    this.mList.mItems[this.mIndex] = this.mItem;
                    this.mList.mSize++;
                }
                this.mList.mVersion--;
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
                this.mUndoManager.Submit(new RemoveAtCommand(this, index));
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

        #endregion

        #region Clear Functions

        private class ClearCommand : Command
        {
            private UndoableList<T> mList;
            private T[] mBackup;

            public ClearCommand(UndoableList<T> list)
            {
                this.mList = list;
                this.mBackup = new T[this.mList.mSize];
                Array.Copy(this.mList.mItems, this.mBackup, this.mList.mSize);
                this.mLabel = "Clear list";
            }

            public override bool Execute()
            {
                Array.Clear(this.mList.mItems, 0, this.mList.mSize);
                this.mList.mSize = 0;
                this.mList.mVersion++;
                return true;
            }

            public override void Undo()
            {
                Array.Copy(this.mBackup, this.mList.mItems, this.mBackup.Length);
                this.mList.mSize = this.mBackup.Length;
                this.mList.mVersion--;
            }
        }

        private void Clear(bool viaCmd)
        {
            if (this.mSize > 0)
            {
                if (viaCmd)
                {
                    this.mUndoManager.Submit(new ClearCommand(this));
                }
                else
                {
                    Array.Clear(this.mItems, 0, this.mSize);
                    this.mSize = 0;
                    this.mVersion++;
                }
            }
        }

        #endregion

        #region Swap Functions

        private class SwapCommand : Command
        {
            private UndoableList<T> mList;
            private int mIndex1;
            private int mIndex2;

            public SwapCommand(UndoableList<T> list, int index1, int index2)
            {
                this.mList = list;
                this.mIndex1 = index1;
                this.mIndex2 = index2;
                this.mLabel = string.Concat("Swap list items at indices ", index1, " and ", index2);
            }

            public override bool Execute()
            {
                T item = this.mList.mItems[this.mIndex1];
                this.mList.mItems[this.mIndex1] = this.mList.mItems[this.mIndex2];
                this.mList.mItems[this.mIndex2] = item;
                this.mList.mVersion++;
                return true;
            }

            public override void Undo()
            {
                T item = this.mList.mItems[this.mIndex1];
                this.mList.mItems[this.mIndex1] = this.mList.mItems[this.mIndex2];
                this.mList.mItems[this.mIndex2] = item;
                this.mList.mVersion--;
            }
        }

        public void Swap(int index1, int index2, bool viaCmd)
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
                if (viaCmd)
                {
                    this.mUndoManager.Submit(new SwapCommand(this, index1, index2));
                }
                else
                {
                    T item = this.mItems[index1];
                    this.mItems[index1] = this.mItems[index2];
                    this.mItems[index2] = item;
                    this.mVersion++;
                }
            }
        }

        public void Swap(int index1, int index2)
        {
            this.Swap(index1, index2, false);
        }

        #endregion

        #region Reverse Functions

        private class ReverseCommand : Command
        {
            private UndoableList<T> mList;
            private int mIndex;
            private int mCount;

            public ReverseCommand(UndoableList<T> list, int index, int count)
            {
                this.mList = list;
                this.mIndex = index;
                this.mCount = count;
                this.mLabel = "Reverse list";
            }

            public override bool Execute()
            {
                throw new NotImplementedException();
            }

            public override void Undo()
            {
                throw new NotImplementedException();
            }
        }

        public void Reverse(int index, int count, bool viaCmd)
        {
            if (this.mSize - index < count)
            {
                throw new ArgumentException("Invalid Offset Length");
            }
            Array.Reverse(this.mItems, index, count);
            this.mVersion++;
        }

        public void Reverse()
        {
            this.Reverse(0, this.mSize, false);
        }

        public void Reverse(int index, int count)
        {
            this.Reverse(index, count, false);
        }

        #endregion

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
            this.Insert(index, item, false);
        }

        public void RemoveAt(int index)
        {
            this.RemoveAt(index, false);
        }

        private class SetAtIndexCommand : Command
        {
            private UndoableList<T> mList;
            private int mIndex;
            private T mBackup;
            private T mItem;

            public SetAtIndexCommand(UndoableList<T> list, int index, T item)
            {
                this.mList = list;
                this.mIndex = index;
                this.mBackup = list.mItems[index];
                this.mItem = item;
                this.mLabel = string.Concat("Set list item at index ", index.ToString());
            }

            public override bool Execute()
            {
                this.mList.mItems[this.mIndex] = this.mItem;
                this.mList.mVersion++;
                return true;
            }

            public override void Undo()
            {
                this.mList.mItems[this.mIndex] = this.mBackup;
                this.mList.mVersion--;
            }
        }

        private void CreateCommandSetAtIndex(int index, object value)
        {
            this.mUndoManager.Submit(new SetAtIndexCommand(this, index, (T)value));
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
            this.Add(item, false);
        }

        public void Clear()
        {
            this.Clear(false);
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
                this.RemoveAt(index, false);
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

        public virtual bool Add()
        {
            this.Add(default(T), false);
            return true;
        }

        public virtual bool Insert(int index)
        {
            this.Insert(index, default(T), false);
            return true;
        }

        private static bool isCompatibleObject(object value)
        {
            return value is T || (value == null && default(T) == null);
        }

        #region IList Functions

        int IList.Add(object value)
        {
            if (value == null && default(T) != null)
            {
                throw new ArgumentNullException("value");
            }
            this.Add((T)value, false);
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
            this.Insert(index, (T)value, false);
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

        #endregion

        #region ICollection Functions

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

        #endregion
    }
}
