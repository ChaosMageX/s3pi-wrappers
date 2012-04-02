using System;

namespace s3piwrappers.EffectCloner.Swarm
{
    public class EffectNameChangedEventArgs : EventArgs
    {
        private string mOldName;
        private string mNewName;
        public EffectNameChangedEventArgs(string oldName, string newName)
        {
            this.mOldName = oldName;
            this.mNewName = newName;
        }
        public string OldName { get { return this.mOldName; } }
        public string NewName { get { return this.mNewName; } }
    }
}
