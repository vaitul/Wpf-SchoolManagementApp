using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestApp.Command;
using TestApp.EntityDatabase.DomainClasses;
using TestApp.EntityDatabase.SchoolEntityContext;

namespace TestApp.ViewModels
{
    public class ResultViewModel : Tab
    {
        private Student _Student;
        public Student Student
        {
            get { return _Student; }
            set { _Student = value; }
        }

        private EntityDatabase.DomainClasses.Standard _Standard;
        public EntityDatabase.DomainClasses.Standard Standard
        {
            get { return _Standard; }
            set { _Standard = value; }
        }

        private ObservableCollection<object> _SubjectAndMarks;

        public ObservableCollection<object> SubjectAndMarks
        {
            get { return _SubjectAndMarks; }
            set { SetValue(ref _SubjectAndMarks, value); }
        }


        private int _Total;
        public int Total
        {
            get { return _Total; }
            set { SetValue(ref _Total, value); }
        }

        private string _Result;
        public string Result
        {
            get { return _Result; }
            set { SetValue(ref _Result, value); }
        }

        private double _Percentage;
        public double Percentage
        {
            get { return _Percentage; }
            set { SetValue(ref _Percentage, value); }
        }

        private object _SelectedSubjectItem;
        public object SelectedSubjectItem
        {
            get { return _SelectedSubjectItem; }
            set
            {
                if (value != null)
                {
                    dynamic tmp = value;
                    EditBox = tmp.Mark;
                }
                SetValue(ref _SelectedSubjectItem, value);
            }
        }


        private int _EditBox;
        public int EditBox
        {
            get { return _EditBox; }
            set { SetValue(ref _EditBox, value); }
        }


        public RelayCommand SaveMarksCommand { get; set; }

        public ResultViewModel(int StudentId)
         {

            TabTitle = "Result";
            UserControl = new UserControls.ShowResult() { DataContext = this };

            using (SchoolObjContext Context = new SchoolObjContext())
            {
                this.Student = Context.Students.Where(s => s.StudentId == StudentId).FirstOrDefault();
                if (this.Student != null)
                {
                    this.Standard = Context.Standards.Where(s => s.StandardId == Student.StandardId).FirstOrDefault();
                    this.Calculate();
                }
            }

            SaveMarksCommand = new RelayCommand(x => SaveMarks());

            if (SubjectAndMarks != null && SubjectAndMarks.Count>0)
                SelectedSubjectItem = SubjectAndMarks[0];
        }

        private void SaveMarks()
        {

            if (EditBox > 100 || EditBox < 0)
            {
                MessageBox.Show("Marks must be >=0 and <100", "Invalid Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedSubjectItem == null)
                return;

            dynamic tmp = SelectedSubjectItem;
            int ID = tmp.MarksheetId;

            SchoolObjContext Context = new SchoolObjContext();
            var SubjectMark = Context.AllMarks.Where(s => s.Id == ID).FirstOrDefault();
            SubjectMark.Mark = EditBox;
            Context.SaveChanges();


            if (SelectedSubjectItem != null)
            {
                var index = SubjectAndMarks.IndexOf(SelectedSubjectItem);
                if (SubjectAndMarks.Count - 1 > index)
                {
                    SelectedSubjectItem = SubjectAndMarks[index + 1];
                }
                else
                    SelectedSubjectItem = SubjectAndMarks[0];
            }

            Calculate();

        }

        private void Calculate()
        {
            SchoolObjContext Context = new SchoolObjContext();
            this.SubjectAndMarks = new ObservableCollection<object>((from Subject in Context.Subjects
                                                                     join Marksheet in Context.AllMarks
                                                                     on Subject.Id equals Marksheet.SubjectId
                                                                     where Marksheet.StudentId == Student.StudentId
                                                                     select new
                                                                     {
                                                                         MarksheetId = Marksheet.Id,
                                                                         SubjectName = Subject.Name,
                                                                         Mark = Marksheet.Mark,
                                                                         Result = Marksheet.Mark<35?"Fail":"Pass"
                                                                     }).ToList());

            Result = "PASS";
            Total = 0;
            if (SubjectAndMarks == null)
                return;
            foreach (dynamic item in SubjectAndMarks)
            {
                Total += (int)item.Mark;
                if (Result == "PASS")
                {
                    if (item.Mark < 35)
                        Result = "FAIL";
                }
            }
            if (Total == 0)
                Percentage = 0;
            else
                Percentage = Total / SubjectAndMarks.Count;
        }
    }
}
