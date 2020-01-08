using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace BLogic
{

    class Program
    {
        static void Main(string[] args)
        {
            Log log = new Log(new Callback(GlobFuncs.MainStatus));
           
            DataManager D = new DataManager(log);

            D.InputFromAsync(GlobFuncs.getConfig("dataUrl")).Wait();
            D.labelDataAsync(GlobFuncs.getConfig("serviceMap"), GlobFuncs.getConfig("token")).Wait();

            Console.ReadKey();
        }

    }
}
