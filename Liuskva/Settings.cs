using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Newtonsoft.Json;
using NLog;
using Liuskva.Utilities;
using System.Globalization;

namespace Liuskva
{
    public class Settings : ISettings    
    {
        [NotNull] private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();


        public Settings() { }


        public Settings Initialise([NotNull] IConfiguration configuration)
        {
            try
            {                
                var settingsFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configuration.PathToSettingsFile);
                var settingsText = ((File.Exists(settingsFileName)? File.ReadAllText(settingsFileName): null) ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(settingsText))
                {
                    throw new ApplicationException($"Settings file is missing or empty: {settingsFileName}");
                }
                var settings = JsonConvert.DeserializeObject<Settings>(settingsText);

                foreach (var property in typeof (Settings).GetProperties().Where(p => p != null && p.CanRead && p.CanWrite))
                {
                    property.GetSetMethod().Invoke(this, new[] { property.GetGetMethod().Invoke(settings, null) });
                } 

                var keyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configuration.PathToSecretDecryptionKeyFile);
                var keyText = File.Exists(keyFileName)? File.ReadAllLines(keyFileName)?.Where(x => !x.StartsWith("#"))?.FirstOrDefault(): null;

                if (string.IsNullOrWhiteSpace(keyText))
                {
                    throw new ApplicationException($"Secret key-file is missing or empty: {keyFileName}");
                }
                var keys = keyText.Split(':');

                if (keys.Length != 2 || keys.Any(string.IsNullOrWhiteSpace))
                {
                    throw new ApplicationException($"Secret key-file does not contain two non-empty keys.");
                }
                OedAppId = keys[0];
                OedApiKey = keys[1];
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Unable to read settings: {exception.Message}", exception);
            }
            return this;
        }


        public string OedApiKey { get; set; }
        public string OedApiUrl { get; set; }
        public string OedAppId { get; set; }
    }
}