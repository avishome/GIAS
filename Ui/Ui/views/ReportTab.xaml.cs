using Entities;
using Prism.Commands;
using System.Windows.Controls;
using System.Windows.Input;
using Ui.viewModel;

namespace Ui
{
    /// <summary>
    /// Interaction logic for ReportTab.xaml
    /// </summary>
    public partial class ReportTab : UserControl
    {
        public ReportTab(Stateless.StateMachine<ViewState, ViewTrigger>.TriggerWithParameters<string> w1, Machine machine, Cluster corentCluster)
        {

            DataContext = new reportTabViewModel(w1,machine, corentCluster);
            InitializeComponent();
        }

    }
}
