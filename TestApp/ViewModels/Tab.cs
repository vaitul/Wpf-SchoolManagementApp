using System;
using System.Windows.Controls;
using TestApp.Command;

namespace TestApp.ViewModels
{
    public class Tab : ViewModelBase
    {
        public string TabTitle { get; set; }
        public UserControl UserControl { get; set; }
        //public bool IsSelected { get; set; }
        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected == false && TabTitle == "Add Subjects")
                {
                    CloseTabCommand.Execute(null);
                    //MainViewModel.CloseTabs("Add Subjects");
                }
                _IsSelected = value;
            }
        }

        public RelayCommand CloseTabCommand { get; set; }
        public event EventHandler CloseRequested;

        public Tab()
        {
            CloseTabCommand = new RelayCommand((x) => CloseTab());
            IsSelected = true;
        }


        private void CloseTab()
        {
            if (CloseRequested != null)
                CloseRequested(this, EventArgs.Empty);
        }
        public virtual void RefreshView() { }
    }
}