using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Entities;
namespace BLogic
{
    internal class OnlineStreem
    {
        private string url;
        LogEvent log;
        public JArray data;
        public dynamic data1;
        public async Task<JArray> PostCallAPI()
        {
            object jsonObject = new object();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                    log.LogMessege("send request", false);
                    var response = await client.PostAsync(url, content);
                    if (response != null)
                    {
                        log.LogMessege("response return", false);
                        var jsonString = await response.Content.ReadAsStringAsync();
                        try
                        {
                            data = JArray.Parse(jsonString);
                        }
                        catch (Exception e) {
                            try
                            {
                                dynamic SteamDetails = JsonConvert.DeserializeObject<dynamic>(jsonString);
                                data1 = SteamDetails;
                            }
                            catch {
                                log.LogMessege("the Url dont response as json" + jsonString, false);
                                throw e;
                            }
                            
                            
                        }
                        return data;
                    }
                    log.LogMessege("respose null", false);
                }
            }
            catch (Exception e)
            {
                log.LogMessege(e.Message, false);
                while (e != null)
                {
                    log.LogMessege(e.Message, false);
                    e = e.InnerException;
                }
                log.LogMessege("End Of Error", false);
            }
            return null;
        }

        internal string getLoc()
        {
            return data1["display_name"].ToString();
        }

        public OnlineStreem(string v, LogEvent log)
        {
            this.url = v;
            this.log = log;

        }

        
        internal Cluster toReportList()
        {
            Cluster all = new Cluster();

            foreach (JObject content in data)
            {
                try {
                    Report item = new Report(
                        (string)content["_id"]["$oid"],
                        new DateTime(
                            (int)content["element_5_3"], 
                            (int)content["element_5_1"], 
                            (int)content["element_5_2"],
                            (int)content["element_1_1"], 
                            (int)content["element_1_2"], 
                            (int)content["element_1_3"]),
                        ((string)content["element_3"]).Split('$'),
                        (string)content["element_2"]
                        );
                    all.push(item);  
                } catch(Exception e) {
                    log.LogMessege("one entery fail: "+ e.ToString(), false);
                }
            }
            log.LogMessege("generate "+all.size()+" reports", false);
            return all;
        }

    }
}