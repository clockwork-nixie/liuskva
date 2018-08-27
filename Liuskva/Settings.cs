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


        private class ImportedSettings
        {
            public string OedAppId { get; set; }            
        }


        public Settings([NotNull] IConfiguration configuration)
        {
            try
            {
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
                var settingsFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configuration.PathToSettingsFile);
                var settingsText = ((File.Exists(settingsFileName)? File.ReadAllText(settingsFileName): null) ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(settingsText))
                {
                    throw new ApplicationException($"Settings file is missing or empty: {settingsFileName}");
                }

                var settings = JsonConvert.DeserializeObject<ImportedSettings>(settingsText);

                OedAppId = keys[0];
                OedApiKey = keys[1];
                OedApiUrl = settings.OedAppId;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Unable to read settings: {exception.Message}", exception);
            }
        }


        public string OedApiKey { get; private set; }
        public string OedApiUrl { get; private set; }
        public string OedAppId { get; private set; }
    }
}