using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TestApp.Command;
using TestApp.EntityDatabase.DomainClasses;
using TestApp.EntityDatabase.SchoolEntityContext;

namespace TestApp.ViewModels
{
    class StudentViewModel : Tab
    {
        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set { SetValue(ref _FirstName, value); }
        }

        private string _MiddleName;
        public string MiddleName
        {
            get { return _MiddleName; }
            set { SetValue(ref _MiddleName, value); }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set { SetValue(ref _LastName, value); }
        }

        private int _Age;
        public int Age
        {
            get { return _Age; }
            set { SetValue(ref _Age, value); }
        }

        private string _City;
        public string City
        {
            get { return _City; }
            set { SetValue(ref _City, value); }
        }


        private EntityDatabase.DomainClasses.Standard _SelectedStandard;
        public EntityDatabase.DomainClasses.Standard SelectedStandard
        {
            get { return _SelectedStandard; }
            set { SetValue(ref _SelectedStandard, value); }
        }

        private int _StdIndex;
        public int StdIndex
        {
            get { return _StdIndex; }
            set { _StdIndex = value; }
        }

        private string _DocFileType;

        public string DocFileType
        {
            get { return _DocFileType; }
            set
            {
                if (value == null)
                    return;

                var ext = value.Split('.').LastOrDefault();
                if (ext.Equals("PDF", StringComparison.InvariantCultureIgnoreCase))
                    _DocFileType = "PDF";
                else if (ext.Equals("PHOTO", StringComparison.InvariantCultureIgnoreCase))
                    _DocFileType = "PHOTO";
                else
                    _DocFileType = "";
                OnPropertyChanged("DocFileType");
            }
        }

        private Byte[] _DocFile;
        public Byte[] DocFile
        {
            get { return _DocFile; }
            set
            {
                SetValue(ref _DocFile,value);
                if (DocFileType.Equals("photo",StringComparison.InvariantCultureIgnoreCase))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    //bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    //bitmapImage.CacheOption = BitmapCacheOption.Default;
                    bitmapImage.StreamSource = new MemoryStream(value);
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    DocImageSource = bitmapImage;
                }
            }
        }

        private ImageSource _DocImageSource;
        public ImageSource DocImageSource
        {
            get { return _DocImageSource; }
            set { SetValue(ref _DocImageSource , value); }
        }


        private string _ButtonName;
        public string ButtonName
        {
            get { return _ButtonName; }
            set { SetValue(ref _ButtonName, value); }
        }

        //public bool CanShowResult { get; set; }


        //for Standard Selection Combo Box
        public List<EntityDatabase.DomainClasses.Standard> AllStandards { get; set; }

        //Commands
        public RelayCommand SubmitStudentCommand { get; set; }
        public RelayCommand ShowResultCommand { get; set; }

        //Call when Editing
        public StudentViewModel(int Id)
        {

            TabTitle = "Edit Students";
            ButtonName = "Save Changes";

            SchoolObjContext context = new SchoolObjContext();

            if (Id > 0)
            {
                using (var Context = new SchoolObjContext())
                {
                    Student obj = Context.Students.Where(s => s.StudentId == Id).FirstOrDefault();
                    AllStandards = context.Standards.Where(s => s.StandardId == obj.StandardId).ToList();

                    if (obj != null)
                    {
                        this.FirstName = obj.FirstName;
                        this.MiddleName = obj.MiddleName;
                        this.LastName = obj.LastName;
                        this.Age = obj.Age;
                        this.City = obj.City;
                        this.DocFileType = obj.DocType;
                        this.DocFile = obj.Doc;
                        SelectedStandard = Context.Standards.Where(s => s.StandardId == obj.StandardId).FirstOrDefault();

                        for (int i = 0; i < AllStandards.Count; i++)
                        {
                            if (AllStandards[i].StandardId == SelectedStandard.StandardId)
                            {
                                StdIndex = i;
                                break;
                            }
                        }
                    }
                }
            }

            //Update edited student
            SubmitStudentCommand = new RelayCommand(x =>
            {
                if (!CheckCanSubmit())
                {
                    MessageBox.Show("Fill all the field properly", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    //CanShowResult = false;
                    return;
                }
                //else
                //    CanShowResult = true;

                EntityDatabase.DomainClasses.Student stud = context.Students.Where(s => s.StudentId == Id).FirstOrDefault();

                if (stud != null)
                {
                    stud.FirstName = this.FirstName;
                    stud.MiddleName = this.MiddleName;
                    stud.LastName = this.LastName;
                    stud.Age = this.Age;
                    stud.City = this.City;
                    stud.StandardId = this.SelectedStandard.StandardId;
                    stud.Doc = this.DocFile;
                    stud.DocType = this.DocFileType;

                    if (context.SaveChanges() > 0)
                    {
                        MainViewModel.RefreshView("Show Students");
                        MainViewModel.RefreshView("Result Report");

                        MessageBox.Show("Student Edited Successfully", "Edited !", MessageBoxButton.OK, MessageBoxImage.Information);
                        //CanShowResult = true;
                    }
                    //else
                    //{
                    //    MessageBox.Show("Student not Edited", "Failed !", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    CanShowResult = false;
                    //}


                    OnPropertyChanged("SelectedStandard");
                    this.CloseTabCommand.Execute(null);
                    MainViewModel.Tabs.Add(new ShowAllStudentViewModel());

                }
            });
            //ShowResultCommand = new RelayCommand(x =>
            //{
            //    this.SubmitStudentCommand.Execute(null);
            //    if (CanShowResult)
            //        MainViewModel.Tabs.Add(new ViewModels.ResultViewModel(Id));
            //});

            UserControl = new UserControls.Student { DataContext = this };
        }

        //Call when new adding
        public StudentViewModel()
        {
            TabTitle = "Add Students";

            ButtonName = "Add";

            SchoolObjContext context = new SchoolObjContext();
            AllStandards = context.Standards.ToList();

            SubmitStudentCommand = new RelayCommand(x =>
            {
                if (!CheckCanSubmit())
                {
                    MessageBox.Show("Fill all the field properly", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    //CanShowResult = false;
                    return;
                }

                SchoolObjContext ctx = new SchoolObjContext();

                //EntityDatabase.DomainClasses.Standard stan = ctx.Standards.Where(s => s.StandardId == this.SelectedStandard.StandardId).FirstOrDefault();

                ctx.Students.Add(new Student()
                {
                    FirstName = this.FirstName,
                    MiddleName = this.MiddleName,
                    LastName = this.LastName,
                    Age = this.Age,
                    City = this.City,
                    StandardId = this.SelectedStandard.StandardId,
                    Doc = this.DocFile,
                    DocType = this.DocFileType
                });

                if (ctx.SaveChanges() > 0)
                {
                    MessageBox.Show("Student Registered Successfully", "Registered !", MessageBoxButton.OK, MessageBoxImage.Information);

                    //CanShowResult = true;

                    MainViewModel.RefreshView("Show Students");
                }
                else
                {
                    //CanShowResult = false;
                    MessageBox.Show("Student Registration Failed", "Registration Failed !", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                this.FirstName = MiddleName = LastName = "";
                this.Age = 0;
                this.City = "";
                this.SelectedStandard = ctx.Standards.FirstOrDefault();
                this.DocFile = null;
                OnPropertyChanged("SelectedStandard");
            });
            //ShowResultCommand = new RelayCommand(x =>
            //{
            //    SubmitStudentCommand.Execute(null);
            //    if (CanShowResult)
            //        MainViewModel.Tabs.Add(new ResultViewModel(context.Students.OrderByDescending(s => s.StudentId).FirstOrDefault<Student>().StudentId));
            //});


            UserControl = new UserControls.Student { DataContext = this };
        }

        public override void RefreshView()
        {
            SchoolObjContext context = new SchoolObjContext();
            AllStandards = context.Standards.ToList();
            StdIndex = _StdIndex;
            OnPropertyChanged("AllStandards");
        }

        public bool CheckCanSubmit()
        {
            if (this.FirstName == null || this.FirstName == " " || this.FirstName == "")
                return false;
            if (this.MiddleName == null || this.MiddleName == " " || this.MiddleName == "")
                return false;
            if (this.LastName == null || this.LastName == " " || this.LastName == "")
                return false;
            if (this.City == null || this.City == " " || this.City == "")
                return false;
            if (this.Age < 5)
                return false;
            if (this.DocFile == null)
                return false;
            if (this.SelectedStandard == null)
                return false;
            return true;
        }
    }
}