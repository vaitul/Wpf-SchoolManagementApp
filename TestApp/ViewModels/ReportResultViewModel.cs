using SchoolManagementSys.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestApp.Command;
using TestApp.EntityDatabase.SchoolEntityContext;
using TestApp.ViewModels;

namespace TestApp.ViewModels
{
    public class ReportResultViewModel : Tab
    {
        private List<EntityDatabase.DomainClasses.Standard> _AllStandards;
        public List<EntityDatabase.DomainClasses.Standard> AllStandards
        {
            get { return _AllStandards; }
            set { SetValue(ref _AllStandards, value); }
        }

        private EntityDatabase.DomainClasses.Standard _SelectedStandard;
        public EntityDatabase.DomainClasses.Standard SelectedStandard
        {
            get { return _SelectedStandard; }
            set { SetValue(ref _SelectedStandard, value); Search(""); }
        }

        private ObservableCollection<object> _AllStudents;
        public ObservableCollection<object> AllStudents
        {
            get { return _AllStudents; }
            set { SetValue(ref _AllStudents, value); }
        }

        private dynamic _CurrentItem;
        public dynamic CurrentItem
        {
            get { return _CurrentItem; }
            set { SetValue(ref _CurrentItem, value); }
        }

        public RelayCommand ShowResultCommand { get; set; }

        public ReportResultViewModel()
        {
            this.TabTitle = "Result Report";
            this.UserControl = new ResultReport() { DataContext = this };
            using (var context = new SchoolObjContext())
            {
                AllStandards = context.Standards.ToList();
                AllStandards.Insert(0, new EntityDatabase.DomainClasses.Standard() { StandardId = 0, StandardName = "All" });
                SelectedStandard = AllStandards[0];

            }
            ShowResultCommand = new RelayCommand(x => MainViewModel.Tabs.Add(new ViewModels.ResultViewModel((int)CurrentItem.Student.StudentId, (int)CurrentItem.StandardId)));

        }

        public void Search(string text)
        {
            SchoolObjContext Context = new SchoolObjContext();
            if (SelectedStandard.StandardName == "All")
            {
                AllStudents = new ObservableCollection<object>(

                    (from AllMarks in Context.AllMarks
                     join Student in Context.Students
                     on AllMarks.StudentId equals Student.StudentId
                     join Standard in Context.Standards
                     on AllMarks.StandardId equals Standard.StandardId
                     where Student.FirstName.StartsWith(text) || Student.MiddleName.StartsWith(text) || Student.LastName.StartsWith(text)
                     select new
                     {
                         Student,
                         StandardName = Standard.StandardName,
                         StandardId = Standard.StandardId
                     }

                    ).Distinct().ToList()
                    );



            }
            else
            {

                AllStudents = new ObservableCollection<object>(

                        (from AllMarks in Context.AllMarks
                         join Student in Context.Students
                         on AllMarks.StudentId equals Student.StudentId
                         join Standard in Context.Standards
                         on AllMarks.StandardId equals Standard.StandardId
                         where (Standard.StandardName == SelectedStandard.StandardName) && (Student.FirstName.StartsWith(text) || Student.MiddleName.StartsWith(text) || Student.LastName.StartsWith(text))
                         select new
                         {
                             Student,
                             StandardName = Standard.StandardName
                         }

                        ).Distinct().ToList());
            }
        }

        public void Search_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var txt = sender as TextBox;
            Search(txt.Text);
        }
    }
}
