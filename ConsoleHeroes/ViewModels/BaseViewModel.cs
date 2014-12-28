using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHeroes.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string prop)
        {
            if (this.PropertyChanged != null)
            {
                var propertyChangedArg = new PropertyChangedEventArgs(prop);
                this.PropertyChanged(this, propertyChangedArg);
            }
        }
    }
}