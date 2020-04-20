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
    /// Interaction logic for ClusterTab.xaml
    /// </summary>
    public partial class ClusterTab : UserControl
    {
        public delegate void ClusterSelected(Entities.Cluster c);
        public event ClusterSelected close;
        public Entities.Cluster corentCluster { get; set; }
        public Stateless.StateMachine<ViewState, ViewTrigger>.TriggerWithParameters<Entities.Cluster> w;
        public Machine Machine { get; }

        public ClusterTab(Entities.Cluster corentCluster, Machine machine, Stateless.StateMachine<ViewState, ViewTrigger>.TriggerWithParameters<Entities.Cluster> w)
        {
            this.w = w;
            this.corentCluster = corentCluster;
            Machine = machine;
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            corentCluster = (Entities.Cluster)(sender as Button).DataContext;
            Machine.Fire(w, (Entities.Cluster)(sender as Button).DataContext);
        }
    }
}
