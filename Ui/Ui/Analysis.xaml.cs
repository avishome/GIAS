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
using LiveCharts; 
using LiveCharts.Wpf;

namespace Ui
{
    /// <summary>
    /// Interaction logic for Analysis.xaml
    /// </summary>
    public partial class Analysis : UserControl
    {
        public Analysis()
        {
            InitializeComponent();
            PointLabel = chartPoint =>
                 string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
             DataContext = this;

        }
        public Func<ChartPoint, string> PointLabel { get; set; } 
 
 
         private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
         { 
             var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView; 
              
             //clear selected slice. 
             foreach (PieSeries series in chart.Series) 
                 series.PushOut = 0; 
 
 
             var selectedSeries = (PieSeries)chartpoint.SeriesView; 
             selectedSeries.PushOut = 8; 
         }

}
}
