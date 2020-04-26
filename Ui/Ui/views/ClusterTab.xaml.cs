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

        public Stateless.StateMachine<ViewState, ViewTrigger>.TriggerWithParameters<Entities.Cluster> w;
        public Machine Machine { get; }

        public ClusterTab(Machine machine, Stateless.StateMachine<ViewState, ViewTrigger>.TriggerWithParameters<Entities.Cluster> w)
        {
            this.w = w;
            Machine = machine;
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Machine.Fire(w, (Entities.Cluster)(sender as Button).DataContext);
        }
    }
}
