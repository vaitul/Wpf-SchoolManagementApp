using TestApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TestApp.Command;

namespace TestApp.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public static ObservableCollection<Tab> Tabs { get; set; }

        public RelayCommand NewStudentTabCommand { get; set; }
        public RelayCommand NewStandardTabCommand { get; set; }
        public RelayCommand ShowAllStudentTabCommand { get; set; }
        public RelayCommand NewSubjectsTabCommand { get; set; }
        public RelayCommand NewResultReportCommand { get; set; }

        public static void CloseTabs(string TabTitle)
        {
            List<int> indexes = new List<int>();
            foreach (var tab in MainViewModel.Tabs)
                if (tab.TabTitle == TabTitle)
                    indexes.Add(MainViewModel.Tabs.IndexOf(tab));
            for (int i = indexes.Count - 1; i >= 0; i--)
                MainViewModel.Tabs[indexes[i]].CloseTabCommand.Execute(null);
        }

        public static void RefreshView(string TabTitle)
        {
            List<int> indexes = new List<int>();
            foreach (var tab in MainViewModel.Tabs)
                if (tab.TabTitle == TabTitle)
                    indexes.Add(MainViewModel.Tabs.IndexOf(tab));
            for (int i = indexes.Count - 1; i >= 0; i--)
                MainViewModel.Tabs[indexes[i]].RefreshView();
        }

        private Visibility _HomeImageVisibilty;
        public Visibility HomeImageVisibilty
        {
            get { return _HomeImageVisibilty; }
            set { SetValue(ref _HomeImageVisibilty, value); }
        }


        public MainViewModel(Window window)
        {
            Tabs = new ObservableCollection<Tab>();

            NewStandardTabCommand = new RelayCommand((x) => NewStandardTab());
            NewStudentTabCommand = new RelayCommand((x) => NewStudentTab());
            ShowAllStudentTabCommand = new RelayCommand((x) => ShowAllStudent());
            NewSubjectsTabCommand = new RelayCommand((x) => NewSubjectsTab());
            NewResultReportCommand = new RelayCommand((x) => NewResultReportTab());

            Tabs.CollectionChanged += Tabs_Collection_Changed;

            HomeImageVisibilty = Visibility.Visible;


            ReportResultViewModel tmp = new ReportResultViewModel();
            tmp = null;

            NewResultReportTab();


        }

        private void NewResultReportTab()
        {
            Tabs.Add(new ReportResultViewModel());
        }

        private void NewSubjectsTab()
        {
            Tabs.Add(new SubjectViewModel());
        }

        private void NewStudentTab()
        {
            Tabs.Add(new StudentViewModel());
        }


        private void ShowAllStudent()
        {
            Tabs.Add(new ShowAllStudentViewModel());
        }

        private void NewStandardTab()
        {
            Tabs.Add(new StandardViewModel());
        }



        private void Tabs_Collection_Changed(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Tab tab;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    tab = (Tab)e.NewItems[0];
                    tab.CloseRequested += OnTabCloseRequested;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    tab = (Tab)e.OldItems[0];
                    tab.CloseRequested -= OnTabCloseRequested;
                    break;
            }
            HomeImageVisibility();
        }

        private void OnTabCloseRequested(object sender, EventArgs e)
        {
            Tabs.Remove(sender as Tab);
        }

        public void HomeImageVisibility()
        {
            if (Tabs.Count <= 0)
                HomeImageVisibilty = Visibility.Visible;
            else
                HomeImageVisibilty = Visibility.Hidden;
        }
    }
}
