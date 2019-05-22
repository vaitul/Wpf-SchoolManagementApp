﻿using System;
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
using TestApp.ViewModels;

namespace SchoolManagementSys.UserControls
{
    /// <summary>
    /// Interaction logic for ResultReport.xaml
    /// </summary>
    public partial class ResultReport
    {
        public ResultReport()
        {
            InitializeComponent();
        }

        private void Searchbox_KeyUp(object sender, KeyEventArgs e)
        {
            (DataContext as ReportResultViewModel).Search_KeyUp(sender,e);
        }
    }
}
