using BLogic;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ui.viewModel
{
    public class analyzisViewModel : INotifyPropertyChanged
    {
        private int _t,_f;
        public int t { get { calc(); return _t;  } }
        public int f { get { calc(); return _f; } }
        private int _dis;
        public int dis { get { return _dis; } set { _dis = value; OnPropertyRaised("pai"); } }
        public SeriesCollection pai { get
            {
                var p = new SeriesCollection();
                p.Add(new PieSeries() { Title = "in", Values = new ChartValues<int>() { t } });
                p.Add(new PieSeries() { Title = "out", Values = new ChartValues<int>() { f } });
                return p;
            } }
        private DataManager dataManager;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public analyzisViewModel(DataManager dataManager) {
            this.dataManager = dataManager;
            int t, f;
            int dis = 400;
        }
        private void calc() {
            (_t, _f) = dataManager.Analize(dis/1000.0);
        }

    }
}
