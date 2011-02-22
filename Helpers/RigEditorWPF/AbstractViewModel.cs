using System;
using System.ComponentModel;

namespace s3piwrappers.RigEditor
{
    public abstract class AbstractViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
        }
        public virtual void Dispose(){}
    }
}
