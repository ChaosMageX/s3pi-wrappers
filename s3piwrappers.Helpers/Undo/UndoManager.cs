using System;

namespace s3piwrappers.Helpers.Undo
{
    /// <summary>
    /// Specialized stack used for managing an Undo/Redo system and
    /// executing <see cref="Command"/> instances submitted by user actions.
    /// </summary>
    public class UndoManager
    {
        /// <summary>
        /// A singleton instance of <see cref="UndoManager"/> for most general uses,
        /// since typically only one Undo/Redo system is present in an application.
        /// </summary>
        public static readonly UndoManager Instance = new UndoManager();

        private static readonly Command[] sEmptyStack = new Command[0];

        /// <summary>
        /// Event triggered when a new <see cref="Command"/> instance is
        /// pushed onto the internal stack after being successfully executed.
        /// </summary>
        public event UndoEventHandler CommandAddedToHistory;
        /// <summary>
        /// Event triggered when a newly submitted <see cref="Command"/> instance
        /// has its <see cref="Command.Execute"/> function invoked by the 
        /// <see cref="UndoManager.Submit(Command)"/> function before being pushed 
        /// onto the internal stack or disposed if the execution wasn't successful.
        /// </summary>
        public event UndoEventHandler CommandExecuted;
        public event UndoEventHandler CommandUndone;
        public event UndoEventHandler CommandRedone;
        public event Action<UndoManager> UndoStackCleared;

        private Command[] mStack;
        private int mStackSize;
        private int mStackSizeLimit;
        private int mLastCmdIndex;
        private Command mCheckpoint;

        /// <summary>
        /// Create a new <see cref="UndoManager"/> instance 
        /// with an empty internal <see cref="Command"/> stack.
        /// </summary>
        public UndoManager()
        {
            this.mStack = sEmptyStack;
            this.mStackSize = 0;
            this.mStackSizeLimit = -1;
            this.mLastCmdIndex = -1;
            this.mCheckpoint = null;
        }

        /// <summary>
        /// Create an new <see cref="UndoManager"/> instance
        /// with an internal <see cref="Command"/> stack
        /// with the given <paramref name="stackCapacity"/>.
        /// </summary>
        /// <param name="stackCapacity">The initial storage capacity 
        /// of the internal <see cref="Command"/> stack.</param>
        public UndoManager(int stackCapacity)
        {
            if (stackCapacity < 0)
            {
                throw new ArgumentOutOfRangeException("stackCapacity");
            }
            else if (stackCapacity == 0)
            {
                this.mStack = sEmptyStack;
            }
            else
            {
                this.mStack = new Command[stackCapacity];
            }
            this.mStackSize = 0;
            this.mStackSizeLimit = -1;
            this.mLastCmdIndex = -1;
            this.mCheckpoint = null;
        }

        public int StackCapacity
        {
            get { return this.mStack.Length; }
            set
            {
                if (value != this.mStack.Length)
                {
                    if (value < this.mStackSize)
                    {
                        throw new ArgumentOutOfRangeException("value");
                    }
                    if (value > 0)
                    {
                        Command[] stack = new Command[value];
                        if (this.mStackSize > 0)
                        {
                            Array.Copy(this.mStack, 0,
                                stack, 0, this.mStackSize);
                        }
                        this.mStack = stack;
                    }
                    else
                    {
                        this.mStack = sEmptyStack;
                    }
                }
            }
        }

        public int StackSize
        {
            get { return this.mStackSize; }
        }

        public int StackSizeLimit
        {
            get { return this.mStackSizeLimit; }
            set { this.mStackSizeLimit = value; }
        }

        public void RemoveStackSizeLimit()
        {
            this.mStackSizeLimit = -1;
        }

        public bool ApplyStackSizeLimit()
        {
            if (this.mStackSizeLimit < 0 || this.mStackSize == 0 ||
                this.mStackSizeLimit >= this.mStackSize)
            {
                return false;
            }
            Command cmd;
            int i, count = 0;
            for (i = 0; i < this.mStackSize; i++)
            {
                cmd = this.mStack[i];
                if (!cmd.GroupWithPreviousCommand)
                {
                    count++;
                }
            }
            int rCount = count - this.mStackSizeLimit + 1;
            if (rCount > 0)
            {
                count = 0;
                int last = 0;
                for (i = 0; i < this.mStackSize; i++)
                {
                    cmd = this.mStack[i];
                    if (!cmd.GroupWithPreviousCommand)
                    {
                        count++;
                    }
                    if (count < rCount)
                    {
                        last++;
                        cmd.Dispose();
                    }
                }
                if (last > this.mStackSize)
                {
                    last = this.mStackSize;
                }
                if (last > 0)
                {
                    this.mStackSize -= last;
                    if (this.mStackSize > 0)
                    {
                        Array.Copy(this.mStack, last,
                            this.mStack, 0, this.mStackSize);
                    }
                    Array.Clear(this.mStack, this.mStackSize, last);
                    return true;
                }
            }
            return false;
        }

        public Command LastExecutedCommand
        {
            get
            {
                if (this.mLastCmdIndex >= 0)
                {
                    return this.mStack[this.mLastCmdIndex];
                }
                return null;
            }
        }

        public void SetCheckpoint()
        {
            if (this.mLastCmdIndex >= 0)
            {
                this.mCheckpoint = this.mStack[this.mLastCmdIndex];
            }
            else
            {
                this.mCheckpoint = null;
            }
        }

        public void ClearCheckpoint()
        {
            this.mCheckpoint = null;
        }

        public bool ChangedSinceCheckpoint
        {
            get
            {
                if (this.mLastCmdIndex < 0)
                {
                    return this.mCheckpoint != null;
                }
                return this.mCheckpoint != this.mStack[this.mLastCmdIndex];
            }
        }

        public bool Submit(Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (!command.Execute())
            {
                command.Dispose();
                return false;
            }
            this.CommandExecuted?.Invoke(this, command);
            Command cmd;
            if (this.mLastCmdIndex < this.mStackSize - 1)
            {
                int index = this.mLastCmdIndex + 1;
                for (int i = index; i < this.mStackSize; i++)
                {
                    this.mStack[i].Dispose();
                }
                Array.Clear(this.mStack, index, this.mStackSize - index);
                this.mStackSize = index;
            }
            if (this.mLastCmdIndex >= 0)
            {
                cmd = this.mStack[this.mLastCmdIndex];
                if (cmd.IsExtendable(command))
                {
                    cmd.Extend(command);
                    command.Dispose();
                    return true;
                }
            }
            if (this.mStackSizeLimit >= 0 && 
                !command.GroupWithPreviousCommand)
            {
                int j, count = 0;
                for (j = 0; j < this.mStackSize; j++)
                {
                    cmd = this.mStack[j];
                    if (!cmd.GroupWithPreviousCommand)
                    {
                        count++;
                    }
                }
                int rCount = count - this.mStackSizeLimit + 1;
                if (rCount > 0)
                {
                    count = 0;
                    int last = 0;
                    for (j = 0; j < this.mStackSize; j++)
                    {
                        cmd = this.mStack[j];
                        if (!cmd.GroupWithPreviousCommand)
                        {
                            count++;
                        }
                        if (count <= rCount)
                        {
                            last++;
                            cmd.Dispose();
                        }
                    }
                    if (last > this.mStackSize)
                    {
                        last = this.mStackSize;
                    }
                    if (last > 0)
                    {
                        this.mStackSize -= last;
                        if (this.mStackSize > 0)
                        {
                            Array.Copy(this.mStack, last, 
                                this.mStack, 0, this.mStackSize);
                        }
                        Array.Clear(this.mStack, this.mStackSize, last);
                    }
                }
            }
            this.mLastCmdIndex = this.mStackSize;
            if (this.mStackSize == this.mStack.Length)
            {
                if (this.mStackSize == 0)
                {
                    this.mStack = new Command[4];
                }
                else
                {
                    Command[] stack = new Command[2 * this.mStackSize];
                    Array.Copy(this.mStack, 0, stack, 0, this.mStackSize);
                    this.mStack = stack;
                }
            }
            this.mStack[this.mStackSize++] = command;
            this.CommandAddedToHistory?.Invoke(this, command);
            return true;
        }

        public void ClearUndoStack()
        {
            this.mLastCmdIndex = -1;
            for (int i = 0; i < this.mStackSize; i++)
            {
                this.mStack[i].Dispose();
            }
            Array.Clear(this.mStack, 0, this.mStackSize);
            this.mStackSize = 0;
            if (this.UndoStackCleared != null)
            {
                this.UndoStackCleared(this);
            }
        }

        public bool CanUndo
        {
            get { return this.mLastCmdIndex >= 0; }
        }

        public string UndoLabel
        {
            get
            {
                if (this.mLastCmdIndex >= 0)
                {
                    return this.mStack[this.mLastCmdIndex].Label;
                }
                return "";
            }
        }

        public void Undo()
        {
            if (this.mLastCmdIndex >= 0)
            {
                Command cmd;
                bool groupWithPrevCmd = true;
                while (this.mLastCmdIndex >= 0 && groupWithPrevCmd)
                {
                    cmd = this.mStack[this.mLastCmdIndex];
                    groupWithPrevCmd = cmd.GroupWithPreviousCommand;
                    cmd.Undo();
                    this.mLastCmdIndex--;
                    this.CommandUndone?.Invoke(this, cmd);
                }
            }
        }

        public bool CanRedo
        {
            get 
            { 
                return this.mStackSize > 0 && 
                    this.mLastCmdIndex < this.mStackSize - 1; 
            }
        }

        public string RedoLabel
        {
            get
            {
                if (this.mStackSize > 0 &&
                    this.mLastCmdIndex < this.mStackSize - 1)
                {
                    return this.mStack[this.mLastCmdIndex + 1].Label;
                }
                return "";
            }
        }

        public void Redo()
        {
            if (this.mStackSize > 0 && 
                this.mLastCmdIndex < this.mStackSize - 1)
            {
                Command cmd;
                int i = this.mLastCmdIndex + 1;
                do
                {
                    cmd = this.mStack[i];
                    cmd.Redo();
                    this.mLastCmdIndex++;
                    this.CommandRedone?.Invoke(this, cmd);
                    i = this.mLastCmdIndex + 1;
                }
                while (i < this.mStackSize && 
                    this.mStack[i].GroupWithPreviousCommand);

                /*bool groupWithPrevCmd = true;
                while (i < this.mStackSize && groupWithPrevCmd)
                {
                    cmd = this.mStack[i];
                    cmd.Redo();
                    this.mLastCmdIndex++;
                    if (this.CommandRedone != null)
                    {
                        this.CommandRedone(this, cmd);
                    }
                    i = this.mLastCmdIndex + 1;
                    if (i < this.mStackSize)
                    {
                        cmd = this.mStack[i];
                        groupWithPrevCmd = cmd.GroupWithPreviousCommand;
                    }
                }/* */
            }
        }
    }
}
