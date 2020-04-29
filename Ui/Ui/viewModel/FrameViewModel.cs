using BLogic;
using LogClass;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ui.views;

namespace Ui.viewModel
{
    class FrameViewModel :  INotifyPropertyChanged
    {
        object _leftTab;
        public object leftTab { get { return _leftTab; } set { _leftTab = value; leftTitle = value.GetType().ToString(); OnPropertyRaised("leftTab"); } }
        object _rightTab;
        public object rightTab { get { return _rightTab; } set { _rightTab = value; rightTitle = value.GetType().ToString(); OnPropertyRaised("rightTab"); } }
        string _TitleConsole;
        public string TitleConsole { get { return _TitleConsole; } set { _TitleConsole = value; OnPropertyRaised("TitleConsole"); } }
        string _logConsole;
        public string logConsole { get { return _logConsole; } set { _logConsole = value; OnPropertyRaised("logConsole"); } }
        string _rightTitle;
        public string rightTitle { get { return _rightTitle; } set { _rightTitle = value; OnPropertyRaised("rightTitle"); } }
        string _leftTitle;
        public string leftTitle { get { return _leftTitle; } set { _leftTitle = value; OnPropertyRaised("leftTitle"); } }

        public Machine Machine;
        public ICommand NewReport { get; set; }
        public ICommand Analitics { get; set; }
        public ICommand Back { get; set; }
        public ICommand Cluster { get; set; }
        public ICommand Refresh { get; set; }
        public ICommand AllReport { get; set; }
        public ICommand Save { get; set; }


        public ViewTrigger options = ViewTrigger.Combina; // set your default value here

        public ViewTrigger Options
        {
            get { return options; }
            set { options = value; Machine.Fire(value); }
        }

        public Entities.Cluster corentCluster;
        LogEvent loge;
        ReportForm Rf;
        public wellcome wellcome;
        public FrameViewModel() {
            wellcome = new wellcome();
            wellcome.Show();
            Log log = new Log(new Callback(MainStatus));

            Machine = new Machine();
            DataManager D = new DataManager(log);
            stateConfigure(Machine, D);

            loge = log.LogMessege("log working:)", true);
            eventConnect(D);
            D.InputFromUrl(GlobFuncs.getConfig("dataUrl"));
            D.viewData();
        }
        private void stateConfigure(Machine Machine, DataManager D)
        {
            var w = Machine.SetTriggerParameters<Entities.Cluster>(ViewTrigger.ShowReports);
            var w1 = Machine.SetTriggerParameters<string>(ViewTrigger.ImageView);

            Machine.Configure(ViewState.Start).OnExit(() => { wellcome.Close(); D.labelDataAsync(GlobFuncs.getConfig("serviceMap"), GlobFuncs.getConfig("token")); });
            Machine.Configure(ViewState.ClusterList).OnEntry(() => { var t = new ListOfCluster(); t.DataContext = D.List; leftTab = t; });
            Machine.Configure(ViewState.ClusterMap).OnEntry(() => { var t = new ReportsInMap(); t.DataContext2 = D.Point; leftTab = t; });
            Machine.Configure(ViewState.ClusterTabs).OnEntry(() => { var t = new ClusterTab(Machine, w); t.DataContext = D.List; leftTab = t; });
            Machine.Configure(ViewState.ClusterCombina).OnEntry(() => { var t = new ReportsInMap(); t.DataContext2 = D.Point; var t2 = new ListOfCluster(); t2.DataContext = D.List; leftTab = t; rightTab = t2; });
            Machine.Configure(ViewState.AnalizeData).OnEntry(() => { var t = new Analysis(D); leftTab = t; var t2 = new linarChart(D); rightTab = t2; });
            Machine.Configure(ViewState.NewReport).OnEntry(() => { AutoComplitTreament(Machine, D); });
            //Machine.Configure(ViewState.ReportList).OnEntryFrom<Entities.Cluster>(w, (C) => { listOfReports t = new listOfReports(); corentCluster = C; t.DataContext = C; rightTab = t; });
            Machine.Configure(ViewState.ReportTabs).OnEntryFrom<Entities.Cluster>(w, (C) => { ReportTab t = new ReportTab(w1, Machine, corentCluster); rightTab = t; corentCluster = C; t.DataContext = C; });
            Machine.Configure(ViewState.ReportTabs).OnEntryFrom<string>(w1, (C) => { ImageView t = new ImageView(); t.DataContext = C; t.Show(); });
            Machine.Configure(ViewState.ReportMap).OnEntry(() => { ReportsInMap t = new ReportsInMap(); t.DataContext2 = corentCluster.ToList(); rightTab = t; });
            Machine.Configure(ViewState.ReportTabs).OnEntry(() => { ReportTab t = new ReportTab(w1, Machine, corentCluster); rightTab = t; });
            Machine.Configure(ViewState.ReportList).OnEntry(() => { listOfReports t = new listOfReports(); t.DataContext = corentCluster; rightTab = t; });
            Machine.Configure(ViewState.AllReports).OnEntry(() =>
            {
                listOfReports t = new listOfReports(); getAllReports(D); t.DataContext = corentCluster; rightTab = t;
                ReportsInMap t2 = new ReportsInMap(); t2.DataContext2 = corentCluster.ToList(); leftTab = t2;
            });
        }

        private void AutoComplitTreament(Machine Machine, DataManager d)
        {
            var t = new ReportsInMap();
            if (Rf is null) Rf = new ReportForm(loge, Machine, d);
            else
            {
                List<Entities.Report> q = new List<Entities.Report>();
                if (!(Rf.s.internet.data is null))
                    foreach (var i in Rf.s.internet.data)
                    {
                        q.Add(new Entities.Report()
                        {
                            p1 = ((dynamic)i)["lat"],
                            p2 = ((dynamic)i)["lon"]
                        });
                    };
                t.DataContext2 = q;  
            }
            rightTab = t;
            leftTab = Rf;
        }
        private void eventConnect(DataManager d)
        {
            NewReport = StateMachineCommandEx.CreateCommand(Machine, ViewTrigger.NewReport);
            Analitics = StateMachineCommandEx.CreateCommand(Machine, ViewTrigger.AnalizeData);
            Back = StateMachineCommandEx.CreateCommand(Machine, ViewTrigger.back);
            Cluster = StateMachineCommandEx.CreateCommand(Machine, ViewTrigger.ClusterCombina);
            AllReport = StateMachineCommandEx.CreateCommand(Machine, ViewTrigger.AllReports);
            Refresh = new DelegateCommand<string>(
                     (string r) => { d.refreshLocalFromDB(); }
             );
            Save = new DelegateCommand<string>(
                     (string r) => { d.Save(); }
             );

        }
        private void OnTransitionAction(Machine.Transition transition)
        {
            TitleConsole = String.Format("Transition from {0} to {1}, trigger = {2}.", transition.Source, transition.Destination, transition.Trigger);
        }
        
        internal void command(ViewTrigger trigger)
        {
            Machine.Fire(trigger);
        }

        public void MainStatus(LogEvent subLog)
        {
            if (subLog.getMessege().EndsWith("| Done")) Machine.Fire(ViewTrigger.DoneCollect);
            logConsole += "\n" + subLog.getMessege();
            TitleConsole = subLog.getMessege();
            subLog.Subscribe(new Callback(MainStatus));
        }


        private void getAllReports(DataManager d)
        {
            corentCluster = d.GetListReports();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
