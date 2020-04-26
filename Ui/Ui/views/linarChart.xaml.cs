using LiveCharts.Wpf;
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
using Ui.viewModel;

namespace Ui.views
{
    /// <summary>
    /// Interaction logic for linarChart.xaml
    /// </summary>
    public partial class linarChart : UserControl
    {
        public linarChartViewModel Vm { get; set; }

        public linarChart(BLogic.DataManager d)
        {
            Vm = new linarChartViewModel(d);
            InitializeComponent();
            DataContext = Vm;
           
        }
    }
}
