using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogClass;
namespace BLogic
{
    public class GlobFuncs
    {
        public static string getConfig(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString.ToString();
        }
        public static void MainStatus(LogEvent subLog)
        {
            Console.Write(subLog.getMessege() + "\n");
            subLog.Subscribe(new Callback(RemoteJsonStuatus));
        }
        public static void RemoteJsonStuatus(LogEvent x)
        {
            Console.Write("    " + x.getMessege() + "\n");
        }
    }
}
