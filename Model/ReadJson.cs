using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LiftTracker.test
{
    class ReadJson
    {
        private readonly string _sampleJsonFilePath;

        public ReadJson(string sampleJsonFilePath)
        {
            _sampleJsonFilePath = sampleJsonFilePath;
        }

        public List<Lift> UseUserDefinedJsonObj()
        {
            //using StreamReader reader = new(_sampleJsonFilePath);
            var json = File.ReadAllText(_sampleJsonFilePath);
            List<Lift> lifts = JsonSerializer.Deserialize<List<Lift>>(json);
            //var lift = JsonSerializer.Deserialize<Lift>(json);
            return lifts;
        }
    }
}
