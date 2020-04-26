using Prism.Commands;
using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ui
{
    public class Machine : StateMachine<ViewState, ViewTrigger>
    {
        public Machine():base(ViewState.Start) {
            ConfigureMachine();
        }
        private void ConfigureMachine()
        {
            // Idle
            this.Configure(ViewState.Start)
                .OnEntry(() => { Console.Write("start"); })
                .Permit(ViewTrigger.DoneCollect, ViewState.ClusterCombina)
                .Permit(ViewTrigger.NewReport, ViewState.NewReport).
                Permit(ViewTrigger.AnalizeData, ViewState.AnalizeData);

            // Refund money
            this.Configure(ViewState.ClusterView)
                .OnEntry(() => { Console.Write("view cluter"); })
                .Permit(ViewTrigger.back, ViewState.Cluster);

            this.Configure(ViewState.Cluster)
                .OnEntry(() => { Console.Write("cluter"); })
                .Permit(ViewTrigger.Combina, ViewState.ClusterCombina)
                .Permit(ViewTrigger.List, ViewState.ClusterList)
                .Permit(ViewTrigger.Tab, ViewState.ClusterTabs)
                .Permit(ViewTrigger.Map, ViewState.ClusterMap)
                .Permit(ViewTrigger.AnalizeData, ViewState.AnalizeData)
                .Permit(ViewTrigger.NewReport,ViewState.NewReport)
                .Permit(ViewTrigger.ShowReports,ViewState.ReportList)
                .Permit(ViewTrigger.back, ViewState.AllReports);

            this.Configure(ViewState.AnalizeData).OnEntry(()=> { Console.Write("analize"); })
                .SubstateOf(ViewState.Cluster);

            this.Configure(ViewState.ClusterCombina)
                .OnEntry(() => { Console.Write("view cluter combina"); })
                .SubstateOf(ViewState.Cluster);

            this.Configure(ViewState.ClusterList)
                .OnEntry(() => { Console.Write("view cluter list"); })
                .SubstateOf(ViewState.Cluster);

            this.Configure(ViewState.ClusterMap)
                .OnEntry(() => { Console.Write("view cluter map"); })
                .SubstateOf(ViewState.Cluster);

            this.Configure(ViewState.ClusterTabs)
                .OnEntry(() => { Console.Write("view cluter Tab"); })
                .SubstateOf(ViewState.Cluster);


            // CanSelectCoffee
            this.Configure(ViewState.Report)
                .OnEntry(() => { Console.Write("Report"); })
                .Permit(ViewTrigger.Combina, ViewState.ReportCombina)
                .Permit(ViewTrigger.List, ViewState.ReportList)
                .Permit(ViewTrigger.Map, ViewState.ReportMap)
                .Permit(ViewTrigger.Tab, ViewState.ReportTabs)
                .Permit(ViewTrigger.AnalizeData, ViewState.AnalizeData)
                .Permit(ViewTrigger.NewReport, ViewState.NewReport)
                .Permit(ViewTrigger.ShowReports, ViewState.ReportList)
                .Permit(ViewTrigger.back, ViewState.AllReports);

            this.Configure(ViewState.AllReports)
               .OnEntry(() => { Console.Write("all report"); })
               .Permit(ViewTrigger.ClusterCombina, ViewState.ClusterCombina)
               .Permit(ViewTrigger.back, ViewState.ClusterCombina)
               .SubstateOf(ViewState.Report);

            this.Configure(ViewState.ReportView)
                .OnEntry(() => { Console.Write("view report"); })
                .Permit(ViewTrigger.back, ViewState.Report);

            this.Configure(ViewState.NewReport)
                .OnEntry(() => { Console.Write("new report"); })
                .Permit(ViewTrigger.back, ViewState.ClusterCombina)
                .PermitReentry(ViewTrigger.SelectPlaceOnMap)
                .PermitReentry(ViewTrigger.NewReport)
                .SubstateOf(ViewState.Cluster);


            this.Configure(ViewState.ReportCombina)
                .OnEntry(() => { Console.Write("view report combina"); })
                .SubstateOf(ViewState.Report);

            this.Configure(ViewState.ReportList)
                .OnEntry(() => { Console.Write("view report list"); })
                .SubstateOf(ViewState.Report);

            this.Configure(ViewState.ReportMap)
                .OnEntry(() => { Console.Write("view report map"); })
                .SubstateOf(ViewState.Report);

            this.Configure(ViewState.ReportTabs)
                .OnEntry(() => { Console.Write("view report tab"); })
                .SubstateOf(ViewState.Report);
            
            

        }
    }
}
