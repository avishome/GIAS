using Entities;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ui.viewModel
{

    class reportTabViewModel
    {
        public Machine machine { get; set; }
        public Stateless.StateMachine<ViewState, ViewTrigger>.TriggerWithParameters<string> w1;
        public Cluster corentCluster { get; set; }
        public ICommand openImg { get; set; }
        public reportTabViewModel(Stateless.StateMachine<ViewState, ViewTrigger>.TriggerWithParameters<string> w1, Machine machine, Cluster corentCluster) {
            this.corentCluster = corentCluster;
            this.w1 = w1;
            this.machine = machine;
            openImg = new DelegateCommand<string>(
                     (string r) => { machine.Fire(w1, r); }
             );
        }
    }
}
