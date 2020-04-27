using Entities;
using Prism.Commands;
using System.Windows.Controls;
using System.Windows.Input;


namespace Ui
{
    /// <summary>
    /// Interaction logic for ReportTab.xaml
    /// </summary>
    public partial class ReportTab : UserControl
    {
        public Machine machine { get; set; }
        public Stateless.StateMachine<ViewState, ViewTrigger>.TriggerWithParameters<string> w1;
        public Cluster corentCluster { get; set; }
        public ReportTab(Stateless.StateMachine<ViewState, ViewTrigger>.TriggerWithParameters<string> w1, Machine machine, Cluster corentCluster)
        {
            this.corentCluster = corentCluster;
            this.w1 = w1;
            this.machine = machine;
            InitializeComponent();
            openImg = new DelegateCommand<string>(
                     (string r) => { machine.Fire(w1, r);}
             );
            DataContext = this;
        }

        public ICommand openImg { get; set;}
    }
}
