using BLogic;
using LogClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
        Machine machine;
        public string val { 
            get { return _val; }
            set { if (_val != value) { _val = value; this.UpdateSuggests(val); } }
        }

        public ReportFormViewModel(LogEvent l, Machine machine) {
            log = l;
            _val = "";
            Suggests = new ObservableCollection<string>() { };
            this.machine = machine;
        }

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
