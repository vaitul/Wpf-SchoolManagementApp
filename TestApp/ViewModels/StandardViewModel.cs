using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestApp.Command;
using TestApp.EntityDatabase.SchoolEntityContext;
using TestApp.UserControls;

namespace TestApp.ViewModels
{
    public class StandardViewModel : Tab
    {
        private EntityDatabase.DomainClasses.Standard _CurrentSelectedItem;
        public EntityDatabase.DomainClasses.Standard CurrentSelectedItem
        {
            get { return _CurrentSelectedItem; }
            set { _CurrentSelectedItem = value; }
        }


        private string _NameToBeAdded;
        public string NameToBeAdded
        {
            get { return _NameToBeAdded; }
            set { SetValue(ref _NameToBeAdded, value); }
        }

        private ObservableCollection<EntityDatabase.DomainClasses.Standard> _StandardList;
        public ObservableCollection<EntityDatabase.DomainClasses.Standard> StandardList
        {
            get { return _StandardList; }
            set { _StandardList = value; }
        }

        public RelayCommand SaveChangesCommand { get; set; }
        public RelayCommand AddStandardCommand { get; set; }
        public RelayCommand RemoveRecordCommand { get; set; }
        public RelayCommand ShowSubjectsCommand { get; set; }

        public StandardViewModel()
        {
            TabTitle = "Standards";

            SchoolObjContext context = new SchoolObjContext();
            StandardList = new ObservableCollection<EntityDatabase.DomainClasses.Standard>(context.Standards.ToList());

            //var Result = (
            //    from Standard in context.Standards
            //    select new
            //    {
            //        StandardId = Standard.StandardId,
            //        StandardName = Standard.StandardName,
            //        TotalSubjects = context.Subjects.Where(s => s.StandardId == Standard.StandardId).Count()
            //    }).ToList();
            //StandardList = new ObservableCollection<object>(Result);

            SaveChangesCommand = new RelayCommand((x) =>
            {
                context.SaveChanges();
                MainViewModel.RefreshView("Subjects");
                MainViewModel.RefreshView("Add Students");
                MainViewModel.RefreshView("Show Students");
            });
            AddStandardCommand = new RelayCommand((x) => AddNewStandard());
            RemoveRecordCommand = new RelayCommand((x) => RemoveRecord());
            ShowSubjectsCommand = new RelayCommand((x)=>MainViewModel.Tabs.Add(new SubjectViewModel(CurrentSelectedItem.StandardId.ToString())));

            this.UserControl = new UserControls.Standard() { DataContext = this };
        }

        private void RemoveRecord()
        {
            if (MessageBox.Show("Are sure to delete '" + CurrentSelectedItem.StandardName + "' and it's all students ? ", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            SchoolObjContext context = new SchoolObjContext();
            TestApp.EntityDatabase.DomainClasses.Standard std = context.Standards.Where(s => s.StandardId == CurrentSelectedItem.StandardId).FirstOrDefault();
            if (std == null)
                return;
            context.Standards.Remove(std);
            context.SaveChanges();

            //MainViewModel.CloseTabs("Subjects");
            MainViewModel.RefreshView("Subjects");
            MainViewModel.RefreshView("Add Students");
            MainViewModel.RefreshView("Show Students");

            StandardList.Remove(CurrentSelectedItem);
        }

        private void AddNewStandard()
        {
            SchoolObjContext context = new SchoolObjContext();
            if (NameToBeAdded != null && NameToBeAdded != " " && NameToBeAdded != "")
            {
                EntityDatabase.DomainClasses.Standard std = new EntityDatabase.DomainClasses.Standard() { StandardName = NameToBeAdded };
                context.Standards.Add(std);
                context.SaveChanges();
                StandardList.Add(context.Standards.AsEnumerable().Last());
                var StdId = context.Standards.Where(s => s.StandardName == NameToBeAdded).FirstOrDefault().StandardId;

                NameToBeAdded = "";

                MainViewModel.RefreshView("Subjects");
                MainViewModel.RefreshView("Show Students");

                MainViewModel.Tabs.Add(new SubjectViewModel(StdId));
            }
        }

    }
}
