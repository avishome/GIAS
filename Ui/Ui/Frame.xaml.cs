using BLogic;
using LogClass;
using Prism.Commands;
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
using System.Windows.Shapes;
using Ui.viewModel;
using Ui.views;

namespace Ui
{
    /// <summary>
    /// Interaction logic for Frame.xaml
    /// </summary>
    public partial class Frame : Window
    {
        FrameViewModel M;
        public Frame()
        {
            InitializeComponent();
            M = new FrameViewModel();
            DataContext = M;       
        }
 

        private void Chip_Click(object sender, RoutedEventArgs e)
        {
            M.command(ViewTrigger.AnalizeData);
        }

        private void xyz_Click(object sender, RoutedEventArgs e)
        {
            M.command(ViewTrigger.NewReport);
        }

        private void Chip_Click_1(object sender, RoutedEventArgs e)
        {
            M.command(ViewTrigger.back);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            M.command(ViewTrigger.back);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            M.command(ViewTrigger.back);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            M.command(ViewTrigger.NewReport);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            M.command(ViewTrigger.AnalizeData);
        }
    }
    }
