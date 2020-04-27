﻿using LogClass;
using Microsoft.Win32;
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

namespace Ui
{
    /// <summary>
    /// Interaction logic for ReportForm.xaml
    /// </summary>
    public partial class ReportForm : UserControl
    {
        public ReportFormViewModel s;
        
        public ReportForm(LogEvent l, Machine machine, BLogic.DataManager d)
        {
            s = new ReportFormViewModel(l,machine,d);
            DataContext = s;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) {
                s.file = openFileDialog.FileName;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
