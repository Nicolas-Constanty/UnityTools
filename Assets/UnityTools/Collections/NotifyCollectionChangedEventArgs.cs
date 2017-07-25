using System;
using System.Collections;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace UnityTools.Collections
{
    public enum NotifyCollectionChangedAction
    {
        Add,
        Move,
        Remove,
        Replace,
        Reset
    }

    public class NotifyCollectionChangedEventArgs : EventArgs
    {
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
        {
            //Reset
            Action = action;
        }

        public NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction action,
            IList changedItems
        )
        {
            //Reset || Add || Remove
            Action = action;
            NewItems = changedItems;
        }


        public NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction action,
            IList newItems,
            IList oldItems
        )

        {
            //Replace
            Action = action;
            NewItems = newItems;
            OldItems = oldItems;
        }

        public NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction action,
            IList newItems,
            IList oldItems,
            int startingIndex
        )
        {
            //Replace
            Action = action;
            NewItems = newItems;
            OldItems = oldItems;
            NewStartingIndex = startingIndex;
        }

        public NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction action,
            IList changedItems,
            int startingIndex
        )
        {
            //Reset || Add || Remove
            Action = action;
            NewItems = changedItems;
            NewStartingIndex = startingIndex;
        }

        public NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction action,
            IList changedItems,
            int index,
            int oldIndex
        )
        {
            //Move
            Action = action;
            NewItems = changedItems;
            NewStartingIndex = index;
            OldStartingIndex = oldIndex;
        }

        public NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction action,
            object changedItem
        )
        {
            //Reset || Add || Remove
            Action = action;
            NewItems = new List<object> { changedItem };
        }

        public NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction action,
            object changedItem,
            int index
        )
        {
            //Reset || Add || Remove
            Action = action;
            NewStartingIndex = index;
            NewItems = new List<object> { changedItem };
        }

        public NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction action,
            object changedItem,
            int index,
            int oldIndex
        )
        {
            //Move
            Action = action;
            NewStartingIndex = index;
            NewItems = new List<object> { changedItem };
            OldStartingIndex = oldIndex;
        }

        public NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction action,
            object newItem,
            object oldItem
        )
        {
            //Replace
            Action = action;
            NewItems = new List<object> { newItem };
            OldItems = new List<object> { oldItem };
        }

        public NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction action,
            object newItem,
            object oldItem,
            int index
        )
        {
            //Replace
            Action = action;
            NewItems = new List<object> { newItem };
            OldItems = new List<object> { oldItem };
            NewStartingIndex = index;
        }

        public NotifyCollectionChangedAction Action { get; private set; }
        public IList NewItems { get; private set; }
        public int NewStartingIndex { get; private set; }
        public IList OldItems { get; private set; }
        public int OldStartingIndex { get; private set; }
    }

    public delegate void NotifyCollectionChangedEventHandler(
        object sender,
        NotifyCollectionChangedEventArgs e
    );
}
