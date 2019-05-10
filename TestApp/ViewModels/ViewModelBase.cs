using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {   
        protected bool SetValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(value, property))
                return false;

            property = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
