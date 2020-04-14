using Entities;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ReportsInMap.xaml
    /// </summary>
    public partial class ReportsInMap : UserControl
    {
        public ReportsInMap()
        {
            InitializeComponent();
            
            Pushpin nit = new Pushpin();


        }
        public List<Report> DataContext2 { 
            get { return _DataContext2; } 
            set {
                _DataContext2 = value;
                if(value is List<Report> && value.Count>0)
                    myMap.Center = new Location(double.Parse(value[0].p1), double.Parse(value[0].p2));
                foreach (var x in value)
                {
                    myMap.Children.Add(new Pushpin() { Location = new Location(double.Parse(x.p1), double.Parse(x.p2)) });
                }
            } 
        }
        private List<Report> _DataContext2 = new List<Report>();

    }
}
