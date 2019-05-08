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
    public class SubjectViewModel : Tab
    {
        public string Notice { get; set; }

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
            set { SetValue(ref _SelectedStandard, value); StandardSelectionChanged(value); }
        }

        private ObservableCollection<object> _AllSubjects;
        public ObservableCollection<object> AllSubjects
        {
            get { return _AllSubjects; }
            set { SetValue(ref _AllSubjects, value); }
        }

        private System.Windows.Visibility _StdColumnVisibility;
        public System.Windows.Visibility StdColumnVisibility
        {
            get { return _StdColumnVisibility; }
            set { SetValue(ref _StdColumnVisibility, value); }
        }

        private object _SelectedGridItem;
        public object SelectedGridItem
        {
            get { return _SelectedGridItem; }
            set
            {
                if (value != null)
                {
                    dynamic tmp = value;
                    EditBox = tmp.Name;
                }
                SetValue(ref _SelectedGridItem, value);
            }
        }


        private string _EditBox;
        public string EditBox
        {
            get { return _EditBox; }
            set { SetValue(ref _EditBox, value); }
        }

        private string _ButtonText;
        public string ButtonText
        {
            get { return _ButtonText; }
            set { SetValue(ref _ButtonText, value); }
        }


        private System.Windows.Visibility _AddSectionVisibility;
        public System.Windows.Visibility AddSectionVisibility
        {
            get { return _AddSectionVisibility; }
            set { SetValue(ref _AddSectionVisibility, value); }
        }

        public RelayCommand SaveChangesCommand { get; set; }
        public RelayCommand DeleteSubject { get; set; }

        public SubjectViewModel(string StdId="0")
        {
            this.TabTitle = "Subjects";
            this.ButtonText = "Save";
            this.UserControl = new UserControls.Subject() { DataContext = this };

            using (var context = new SchoolObjContext())
            {
                AllStandards = context.Standards.ToList();
                AllStandards.Insert(0, new EntityDatabase.DomainClasses.Standard() { StandardId = 0, StandardName = "All" });
            }

            this.StandardSelectionChanged(new EntityDatabase.DomainClasses.Standard());
            StdColumnVisibility = System.Windows.Visibility.Visible;
            SelectedStandard = AllStandards.Find(x => x.StandardId == int.Parse(StdId));

            SaveChangesCommand = new RelayCommand(x => SaveChanges());
        }
        public SubjectViewModel(int stdId)
        {
            this.TabTitle = "Add Subjects";
            this.ButtonText = "Add";
            this.Notice = "*Warning : Don't switch or close this tab otherwise you can't re-open this tab and add subjects.*";
            this.UserControl = new UserControls.Subject() { DataContext = this };

            using (var context = new SchoolObjContext())
            {
                AllStandards = context.Standards.Where(s=>s.StandardId==stdId).ToList();
                //AllStandards.Insert(0, new EntityDatabase.DomainClasses.Standard() { StandardId = 0, StandardName = "All" });
                AddSectionVisibility = context.Subjects.Where(s => s.StandardId == stdId).Count() > 0 ? Visibility.Hidden : Visibility.Visible; 
            }

            this.StandardSelectionChanged(new EntityDatabase.DomainClasses.Standard());
            StdColumnVisibility = System.Windows.Visibility.Visible;
            SelectedStandard = AllStandards[0];

            SaveChangesCommand = new RelayCommand(x => AddSubject());
            DeleteSubject = new RelayCommand(x => RemoveSubject());
        }

        public override void RefreshView()
        {
            this.UserControl.DataContext = new SubjectViewModel(this.SelectedStandard.StandardId.ToString());
        }

        private void RemoveSubject()
        {
            if (SelectedGridItem == null)
                return;


            dynamic tmp = SelectedGridItem;
            int id = tmp.Id;

            SchoolObjContext Context = new SchoolObjContext();
            var obj = Context.Subjects.Where(s => s.Id == id).FirstOrDefault();
            Context.Subjects.Remove(obj);
            if (Context.SaveChanges() > 0)
            {
                MainViewModel.RefreshView("Subjects");
                MessageBox.Show("Subject Deleted Successfully", "Success !", MessageBoxButton.OK, MessageBoxImage.Information);
                AllSubjects.Remove(SelectedGridItem);
                EditBox = "";
            }
        }

        private void AddSubject()
        {
            if (EditBox == "")
                return;
            SchoolObjContext Context = new SchoolObjContext();
            var subject = new Subject() { Name = EditBox, StandardId = SelectedStandard.StandardId, Standard = null };
            Context.Subjects.Add(subject);
            if (Context.SaveChanges() > 0)
            {
                MainViewModel.RefreshView("Subjects");
                MessageBox.Show("Subject Added Successfully", "Success !", MessageBoxButton.OK, MessageBoxImage.Information);
                AllSubjects.Add(subject);
                EditBox = "";
            }
        }

        private void SaveChanges()
        {
            if (SelectedGridItem == null)
                return;
            dynamic tmp = SelectedGridItem;
            int id = tmp.Id;
            SchoolObjContext Context = new SchoolObjContext();
            Subject obj = Context.Subjects.Where(s => s.Id == id).FirstOrDefault();
            if (obj != null)
            {
                obj.Name = EditBox;
                if (Context.SaveChanges() > 0)
                {
                    MainViewModel.RefreshView(this.TabTitle);
                    MessageBox.Show("Subject Edited Successfully", "Success !", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                int i = AllSubjects.IndexOf(SelectedGridItem);
                AllSubjects[i] = obj as object;
                EditBox = "";
            }
        }

        private void StandardSelectionChanged(EntityDatabase.DomainClasses.Standard std)
        {
            using (SchoolObjContext Context = new SchoolObjContext())
            {
                if (AllSubjects != null)
                    AllSubjects = null;
                if (std.StandardId == 0)
                {
                    AllSubjects = new ObservableCollection<object>(
                        (from Subject in Context.Subjects
                         join Standard in Context.Standards
                         on Subject.StandardId equals Standard.StandardId
                         select new
                         {
                             Id = Subject.Id,
                             Name = Subject.Name,
                             Standard = Standard.StandardName
                         }).ToList()
                        );
                    StdColumnVisibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    AllSubjects = new ObservableCollection<object>(Context.Subjects.Where(s => s.StandardId == std.StandardId).ToList());
                    StdColumnVisibility = System.Windows.Visibility.Hidden;
                }
            }
        }

    }
}
