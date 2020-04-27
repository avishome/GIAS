using BLogic;
using LogClass;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ui
{
    public class ReportFormViewModel
    {
        /*pubilc void postr()
        {
            HttpClient c = new HttpClient();
            if (this.file != "") {
                System.IO.FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0 , data.Length);
                fs.Close();
            }
            var values = new Dictionary<string, object>
            {
                
            };
            var content = new FormUrlEncodedContent(values);
            var response = await c.PostAsync("", content);
            var responseString = await response.Content.ReadAsStringAsync();
        }*/
        public OnlineStreem internet;
        LogEvent log;
        dynamic Selectplace;
        int _indexSelect;
        public int indexSelect { get { return _indexSelect; } set { _indexSelect = value; selectPlace(value); } }
        public ObservableCollection<String> Suggests { get; set; }
        private string _val;
        internal string file;
        public DateTime date { get; set; }
        public string time { get; set; }
        public ICommand send { get; set; }
        Machine machine;
        DataManager d;
        public string val { 
            get { return _val; }
            set { if (_val != value) { _val = value; this.UpdateSuggests(val); } }
        }

        public ReportFormViewModel(LogEvent l, Machine machine, DataManager d) {
            log = l;
            _val = "";
            this.d = d;
            date = DateTime.Now;
            time = DateTime.Now.ToString("HH:mm:ss");
            Suggests = new ObservableCollection<string>() { };
            this.machine = machine;
            send = new DelegateCommand(
                     () => insert(),
                     () => { log.LogMessege("check", true);  return true; }
                     );
            
        }

        private void insert()
        {
            DateTime e;
            try
            {
                 e = DateTime.ParseExact(time, "HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch { return; }
            if ( date == null) return;
            var d = new DateTime(date.Year, date.Month, date.Day,e.Hour,e.Minute,e.Second);
            log.LogMessege(d.ToString(),true);
            
            this.Selectplace = internet.getLocData(0);

            if (Selectplace != null)
            {
                var report = new Entities.Report(Guid.NewGuid().ToString("N"), d, new string[2] { Selectplace["lat"], Selectplace["lon"] },"");
                this.d.addReport(report);
            }
        }
        public void logfunc(string str) { log.LogMessege(str,true); }
        private async void UpdateSuggests(string q)
        {
            string query = string.Format(GlobFuncs.getConfig("serviceAutoComplit"), GlobFuncs.getConfig("token"), q);
            internet = new OnlineStreem(query, log);
            await internet.PostCallAPI();
            internet.getAddress();
            Suggests.Clear();
            foreach(var l in internet.getAddress())
                Suggests.Add(l);
            this.machine.Fire(ViewTrigger.SelectPlaceOnMap);
        }
        public void selectPlace(int place) {
            this.Selectplace = internet.getLocData(place);
            if (!(this.Selectplace is null))
            {
                val = this.Selectplace["display_name"];
                log.LogMessege(val, false);
            }
        }

    }
}
