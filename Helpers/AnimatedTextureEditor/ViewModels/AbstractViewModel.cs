using System;
using System.ComponentModel;

namespace s3piwrappers.AnimatedTextureEditor.ViewModels
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
