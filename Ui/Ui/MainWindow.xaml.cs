using Entities;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Frame f = new Frame();
            f.Show();
            ReportsList = new List<Report>();
            ReportsList.Add(new Report() { Id = "Complete this WPF tutorial", dateTime = new DateTime(2018, 2, 2), loc = new Adrress(""), p1="37.806029", p2="-122.407007" });
            ReportsList.Add(new Report() { Id = "fsdsdgsd", dateTime = new DateTime(2018, 12, 12), loc = new Adrress(""), p1 = "33.806029", p2 = "-122.407007" });
            ReportsList.Add(new Report() { Id = "Complete this ", dateTime = new DateTime(2028, 1, 2), loc = new Adrress(""), p1 = "37.806029", p2 = "-125.407007" });
            
            InitializeComponent();
            
            ReportT.DataContext = ReportsList;
            ReportL.DataContext = ReportsList;
            ReportM.DataContext2 = ReportsList;
            
        }

        public List<Report> ReportsList { get; set; }
    }
}
