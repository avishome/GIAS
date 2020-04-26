using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ui.viewModel
{
    public class linarChartViewModel
    {
        public linarChartViewModel(BLogic.DataManager d) {
            this.d = d;
            YFormatter = value => { return value.ToString(); };
        }
        public BLogic.DataManager d { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public LiveCharts.ChartValues<double> data
        {
            get
            {
                Entities.Cluster reports = d.GetListReports();
                DateTime min = reports.Min((x) => x.dateTime);
                DateTime max = reports.Max((x) => x.dateTime);
                int lenghtOfGrafh = (max.Year - min.Year) * 12 + (max.Month - min.Month) - (max.Day < min.Day ? 1 : 0);
                Labels = new string[lenghtOfGrafh + 1];
                double[] res = new double[lenghtOfGrafh + 1];
                for (int i = 0; i < lenghtOfGrafh + 1; i++)
                {
                    Labels[i] = min.AddMonths(i).ToString("MMM yy");
                }
                foreach (var r in d.GetListReports())
                {
                    int temp = (r.dateTime.Year - min.Year) * 12 + (r.dateTime.Month - min.Month) - (r.dateTime.Day < min.Day ? 1 : 0);
                    if (temp > -1 && temp < lenghtOfGrafh + 1)
                        res[temp]++;
                }

                return new LiveCharts.ChartValues<double>(res);
            }
        }
        public LiveCharts.SeriesCollection ser
        {
            get
            {
                return new LiveCharts.SeriesCollection() { new LineSeries() { Title = "falls", Values = data } };
            }
        }
    }
}
