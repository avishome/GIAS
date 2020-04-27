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
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Microsoft.Win32;
using System.ComponentModel;

namespace Ui
{
    public class ReportFormViewModel : INotifyPropertyChanged
    {
        private void OnPropertyRaised(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        
        public OnlineStreem internet;
        LogEvent log;
        dynamic Selectplace;
        int _indexSelect;
        public int indexSelect { get { return _indexSelect; } set { _indexSelect = value; selectPlace(value); } }
        public ObservableCollection<String> Suggests { get; set; }
        private string _val;
        internal string file;
        GeoLocation location;
        public bool hasFile { get { return location != null; }  set { } }
        public DateTime date { get; set; }
        public string time { get; set; }
        public ICommand send { get; set; }
        public ICommand addpic { get; set; }
        Machine machine;
        DataManager d;

        public event PropertyChangedEventHandler PropertyChanged;

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
            addpic = new DelegateCommand(
                     () => takePic(),
                     () => { return true; }
                     );
        }
        public void takePic() {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                file = openFileDialog.FileName;
                try
                {
                    var gps = ImageMetadataReader.ReadMetadata(file)
                                 .OfType<GpsDirectory>()
                                 .FirstOrDefault();
                    location = gps.GetGeoLocation();
                    OnPropertyRaised("hasFile");
                    log.LogMessege(String.Format("Image at {0},{1}", location.Latitude, location.Longitude), true);
                }
                catch (Exception e) { log.LogMessege(e.Message, true); return; }

            }
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
            if (location != null)
            {
                var report = new Entities.Report(Guid.NewGuid().ToString("N"), d, new string[2] { location.Latitude.ToString(), location.Longitude.ToString() }, "");
                this.d.addReport(report);
            }
            else if (Selectplace != null)
            {
                var report = new Entities.Report(Guid.NewGuid().ToString("N"), d, new string[2] { Selectplace["lat"], Selectplace["lon"] }, "");
                this.d.addReport(report);
            }
            else
                log.LogMessege("no place...", true);
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
