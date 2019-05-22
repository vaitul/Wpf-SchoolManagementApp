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
using System.Windows.Shapes;
using TestApp.EntityDatabase.DomainClasses;
using TestApp.EntityDatabase.SchoolEntityContext;

namespace TestApp.Dialogs
{
    /// <summary>
    /// Interaction logic for ImageDialog.xaml
    /// </summary>
    public partial class ImageDialog : Window
    {
        public ImageDialog(Student Student)
        {
            InitializeComponent();
            this.Title = "Birth Certificate";
            if (Student != null)
            {
                this.Title = "Birth Certificate of " + Student.FirstName + " " + Student.MiddleName;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                //bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                //bitmapImage.CacheOption = BitmapCacheOption.Default;
                bitmapImage.StreamSource = new MemoryStream(Student.Doc);
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                Image.Source = bitmapImage;

            }
        }
    }
}
