using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using TestApp.ViewModels;

namespace TestApp.UserControls
{
    /// <summary>
    /// Interaction logic for Student.xaml
    /// </summary>
    public partial class Student : UserControl
    {
        public Student()
        {
            InitializeComponent();
        }

        private void SelectFileButton_Clicked(object sender, RoutedEventArgs e)
        {
            var ViewModel = this.DataContext as StudentViewModel;

            var Dialog = new OpenFileDialog()
            {
                Filter = "Images (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg | Pdf (.pdf)|*.pdf",
                FilterIndex = 0
            };
            if (Dialog.ShowDialog() != true) { return; }

            ViewModel.DocFileType = Dialog.SafeFileName;

            ViewModel.DocFile = File.ReadAllBytes(Dialog.FileName);
        }
    }
}
