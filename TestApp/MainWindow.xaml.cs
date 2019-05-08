using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestApp.EntityDatabase.DomainClasses;
using TestApp.EntityDatabase.SchoolEntityContext;
using TestApp.ViewModels;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModels.MainViewModel ViewModel;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel(this);
            DataContext = ViewModel;


            //using (var Context = new SchoolObjContext())
            //{
            //    Student Student = (from s in Context.Students
            //                       where s.FirstName.StartsWith("Vaitul") || s.MiddleName.StartsWith("Vaitul") || s.LastName.StartsWith("Vaitul")
            //                       select s).FirstOrDefault();
            //    if (Student != null)
            //    {

            //        dynamic obj = (from Subjects in Context.Subjects

            //                       join Marksheet in Context.AllMarks
            //                       on Subjects.Id equals Marksheet.SubjectId

            //                       where Marksheet.StudentId == Student.StudentId && Marksheet.StandardId == Student.StandardId
            //                       select new
            //                       {
            //                           Subject = Subjects.Name,
            //                           Mark = Marksheet.Mark,
            //                           SubjectResult = Marksheet.Mark >= 35 ? true : false
            //                       }).ToList();
            //    }
            //}
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


    }
}
