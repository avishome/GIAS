using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Adrress
    {
        [Key]
        public String Id { get; set; }
        public Adrress(string str)
        {
            this.display_name = str;
            Id = generateID();
        }
        public Adrress()
        {
            Id = generateID();
            display_name = "";
        }
        public string generateID()
        {
            return Guid.NewGuid().ToString("N");
        }
        public override string ToString() {
            return display_name;
        }
        public string display_name { get; set; }

    }
}