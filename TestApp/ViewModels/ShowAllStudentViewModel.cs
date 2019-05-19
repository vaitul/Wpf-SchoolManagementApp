using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TestApp.Command;
using TestApp.EntityDatabase.DomainClasses;
using TestApp.EntityDatabase.SchoolEntityContext;
using TestApp.UserControls;

namespace TestApp.ViewModels
{
    public class ShowAllStudentViewModel : Tab
    {
        private dynamic _CurrentItem;
        public dynamic CurrentItem
        {
            get { return _CurrentItem; }
            set { SetValue(ref _CurrentItem, value); }
        }


        private ObservableCollection<object> _AllStudents;
        public ObservableCollection<object> AllStudents
        {
            get { return _AllStudents; }
            set { SetValue(ref _AllStudents, value); }
        }


        public RelayCommand SaveCommand { get; set; }
        public RelayCommand RemoveRecordCommand { get; set; }
        public RelayCommand EditStudentCommand { get; set; }
        public RelayCommand ShowResultCommand { get; set; }

        public ShowAllStudentViewModel()
        {
            TabTitle = "Show Students";
            UserControl = new ShowAllStudent() { DataContext = this };

            SchoolObjContext Context = new SchoolObjContext();
            AllStudents = new ObservableCollection<object>(Context.Students.Join(
                    Context.Standards,
                    s => s.StandardId,
                    std => std.StandardId,
                    (s, std) => new
                    {
                        Student = s,
                        StandardName = std.StandardName
                    }
                ).ToList());

            RemoveRecordCommand = new RelayCommand(x => RemoveRecord());
            EditStudentCommand = new RelayCommand(x => EditStudent(x));
            ShowResultCommand = new RelayCommand(x =>
            {
                var Student = Context.Students.Where(s => s.StudentId == (int)x).FirstOrDefault();
                if (Student == null)
                    return;
                var check = (from sheet in Context.AllMarks
                             where sheet.StudentId == Student.StudentId && sheet.StandardId == Student.StandardId
                             select sheet).FirstOrDefault();
                if (check == null)
                {
                    if (MessageBox.Show("Result not available yet, click 'Yes' to generate", "Result not available", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        MainViewModel.Tabs.Add(new ViewModels.ResultViewModel((int)CurrentItem.Student.StudentId, 0, true));
                        return;
                    }
                }
                else
                    MainViewModel.Tabs.Add(new ViewModels.ResultViewModel((int)CurrentItem.Student.StudentId));
            });



        }

        public override void RefreshView()
        {
            SchoolObjContext Context = new SchoolObjContext();
            AllStudents = new ObservableCollection<object>(Context.Students.Join(
                    Context.Standards,
                    s => s.StandardId,
                    std => std.StandardId,
                    (s, std) => new
                    {
                        Student = s,
                        StandardName = std.StandardName
                    }
                ).ToList());
        }

        private void EditStudent(object Id)
        {
            MainViewModel.Tabs.Add(new StudentViewModel((int)Id));
            this.CloseTabCommand.Execute(null);
        }

        private void RemoveRecord()
        {
            if (MessageBox.Show("Are sure to delete this student ? ", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            SchoolObjContext context = new SchoolObjContext();
            dynamic item = CurrentItem;
            EntityDatabase.DomainClasses.Student SelectedStudent = CurrentItem.Student;
            EntityDatabase.DomainClasses.Student obj = context.Students.Where(s => s.StudentId == SelectedStudent.StudentId).FirstOrDefault();
            if (obj == null)
                return;
            context.Students.Remove(obj);
            context.SaveChanges();

            AllStudents.Remove(CurrentItem);
        }
    }
}
