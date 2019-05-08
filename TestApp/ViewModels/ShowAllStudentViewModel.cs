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

            RemoveRecordCommand = new RelayCommand(x => RemoveRecord());
            EditStudentCommand = new RelayCommand(x => EditStudent(x));
            ShowResultCommand = new RelayCommand(x=>MainViewModel.Tabs.Add(new ViewModels.ResultViewModel((int)CurrentItem.Student.StudentId)));

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

            UserControl = new UserControls.ShowAllStudent() { DataContext = this };
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
            Student SelectedStudent = CurrentItem.Student;
            Student obj = context.Students.Where(s => s.StudentId == SelectedStudent.StudentId).FirstOrDefault();
            if (obj == null)
                return;
            context.Students.Remove(obj);
            context.SaveChanges();

            AllStudents.Remove(CurrentItem);
        }
    }
}
