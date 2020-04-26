using BLogic;
using LogClass;
using Prism.Commands;
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
using System.Windows.Shapes;
using Ui.views;

namespace Ui
{
    /// <summary>
    /// Interaction logic for Frame.xaml
    /// </summary>
    public partial class Frame : Window
    {
        public Machine Machine;
        public ICommand NewReport { get; set; }
        public ICommand Analitics { get; set; }
        public ICommand Back { get; set; }
        public ICommand Cluster { get; set; }

        public ViewTrigger options = ViewTrigger.Combina; // set your default value here
        public Entities.Cluster corentCluster;
        LogEvent loge;
        ReportForm Rf;
        public wellcome wellcome;
        public Frame()
        {
            wellcome = new wellcome();
            wellcome.Show();
            Log log = new Log(new Callback(MainStatus));
            
            Machine = new Machine();
            DataManager D = new DataManager(log);
            stateConfigure(Machine, D);

            InitializeComponent();
            DataContext = this;


            loge = log.LogMessege("log working:)", true);
            eventConnect();
            D.InputFromUrl(GlobFuncs.getConfig("dataUrl"));
            D.viewData();
            
        }
        private void eventConnect() {
            NewReport = StateMachineCommandEx.CreateCommand(Machine, ViewTrigger.NewReport);
            Analitics = StateMachineCommandEx.CreateCommand(Machine, ViewTrigger.AnalizeData);
            Back = StateMachineCommandEx.CreateCommand(Machine, ViewTrigger.back);
            Cluster = StateMachineCommandEx.CreateCommand(Machine, ViewTrigger.ClusterCombina);
            /*switch1.Checked += new RoutedEventHandler((Object x, RoutedEventArgs y) => Machine.Fire(ViewTrigger.Map));
            switch2.Checked += new RoutedEventHandler((Object x, RoutedEventArgs y) => Machine.Fire(ViewTrigger.Tab));
            switch3.Checked += new RoutedEventHandler((Object x, RoutedEventArgs y) => Machine.Fire(ViewTrigger.List));
            switch4.Checked += new RoutedEventHandler((Object x, RoutedEventArgs y) => Machine.Fire(ViewTrigger.back));
            */
        }
        private void stateConfigure(Machine Machine, DataManager D) {
            var w = Machine.SetTriggerParameters<Entities.Cluster>(ViewTrigger.ShowReports);
            Machine.Configure(ViewState.Start).OnExit(() => { wellcome.Close(); D.labelDataAsync(GlobFuncs.getConfig("serviceMap"), GlobFuncs.getConfig("token")); });
            Machine.Configure(ViewState.ClusterList).OnEntry(() => { var t = new ListOfCluster(); t.DataContext = D.List; LeftBox.Content = t;  });
            Machine.Configure(ViewState.ClusterMap).OnEntry(() => { var t = new ReportsInMap(); t.DataContext2 = D.Point; LeftBox.Content = t;  });
            Machine.Configure(ViewState.ClusterTabs).OnEntry(() => { var t = new ClusterTab(Machine, w); t.DataContext = D.List; LeftBox.Content = t; });
            Machine.Configure(ViewState.ClusterCombina).OnEntry(() => { var t = new ReportsInMap(); t.DataContext2 = D.Point; var t2 = new ListOfCluster(); t2.DataContext = D.Point; LeftBox.Content = t; RightBox.Content = t2; });
            Machine.Configure(ViewState.AnalizeData).OnEntry(() => { var t = new Analysis(D);  LeftBox.Content = t; var t2 = new linarChart(D); RightBox.Content = t2; });
            Machine.Configure(ViewState.NewReport).OnEntry(() => { AutoComplitTreament(Machine); });
            Machine.Configure(ViewState.ReportList).OnEntryFrom<Entities.Cluster>(w, (C) => { listOfReports t = new listOfReports(); corentCluster = C; t.DataContext = C; RightBox.Content = t; });
            Machine.Configure(ViewState.ReportMap).OnEntry(() => { ReportsInMap t = new ReportsInMap(); t.DataContext2 = corentCluster.ToList(); RightBox.Content = t; });
            Machine.Configure(ViewState.ReportTabs).OnEntry(() => { ReportTab t = new ReportTab(); t.DataContext = corentCluster; RightBox.Content = t; });
            Machine.Configure(ViewState.ReportList).OnEntry(() => { listOfReports t = new listOfReports(); t.DataContext = corentCluster; RightBox.Content = t; });
            Machine.Configure(ViewState.AllReports).OnEntry(() => { listOfReports t = new listOfReports(); getAllReports(D); t.DataContext = corentCluster; RightBox.Content = t;
                                                    ReportsInMap t2 = new ReportsInMap(); t2.DataContext2 = corentCluster.ToList(); LeftBox.Content = t2;
            });

            Machine.OnTransitioned(OnTransitionAction);
        }

        private void getAllReports(DataManager d)
        {
            corentCluster = d.GetListReports();
        }

        private void AutoComplitTreament(Machine Machine)
        {
            if (Rf is null) Rf = new ReportForm(loge, Machine);
            else
            {
                var t = new ReportsInMap();
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
                RightBox.Content = t;
            }
            LeftBox.Content = Rf;
        }

        private void OnTransitionAction(Machine.Transition transition)
        {
            console.Text = String.Format("Transition from {0} to {1}, trigger = {2}.", transition.Source, transition.Destination, transition.Trigger);
        }

        public void MainStatus(LogEvent subLog)
        {
            if(subLog.getMessege().EndsWith("| Done")) Machine.Fire(ViewTrigger.DoneCollect);
            fullLog.Text+="\n" + subLog.getMessege();
            console.Text = subLog.getMessege();
            subLog.Subscribe(new Callback(MainStatus));
        }
        public ViewTrigger Options
        {
            get { return options;  }
            set { options = value; Machine.Fire(value); }
        }

        private void Chip_Click(object sender, RoutedEventArgs e)
        {
            Machine.Fire(ViewTrigger.AnalizeData);
        }

        private void xyz_Click(object sender, RoutedEventArgs e)
        {
            Machine.Fire(ViewTrigger.NewReport);
        }

        private void Chip_Click_1(object sender, RoutedEventArgs e)
        {
            Machine.Fire(ViewTrigger.back);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Machine.Fire(ViewTrigger.ClusterCombina);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Machine.Fire(ViewTrigger.back);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Machine.Fire(ViewTrigger.NewReport);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Machine.Fire(ViewTrigger.AnalizeData);
        }
    }
    }
