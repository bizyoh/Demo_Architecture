using Application.Interfaces;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ServerService : IServerService
    {
        public void ChangeServer()
        {
            var appSettingsPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "appsettings.json");
            var json = File.ReadAllText(appSettingsPath);

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(new ExpandoObjectConverter());
            jsonSettings.Converters.Add(new StringEnumConverter());

            dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(json, jsonSettings);

            config.DebugEnabled = true;
            if (config.DatabaseProvider == "SQLServer")
            {
                config.DatabaseProvider = "Postgres";
            }
            else
            {
                config.DatabaseProvider = "SQLServer";
            }
            var newJson = JsonConvert.SerializeObject(config, Formatting.Indented, jsonSettings);

            File.WriteAllText(appSettingsPath, newJson);
        }
    }
}
