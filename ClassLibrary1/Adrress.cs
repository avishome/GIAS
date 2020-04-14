using System.ComponentModel;

namespace Entities
{
    public class Adrress: INotifyPropertyChanged
    {

        public Adrress(string str)
        {
            this.display_name = str;
        }
        public Adrress()
        {
        }

        public string ToString() {
            return display_name;
        }
        public string toString { get { return ToString(); } }
        public string house_number { get; set; }
        public string road { get; set; }
        public string neighbourhood { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string state { get; set; }
        public string postcode { get; set; }
        public string country { get; set; }
        public string country_code { get; set; }
        public string display_name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}