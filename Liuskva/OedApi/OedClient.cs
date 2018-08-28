using System;
using System.Net;
using JetBrains.Annotations;
using Liuskva.Utilities;

namespace Liuskva.OedApi
{
    public class OedClient : IOedClient
    {
        [NotNull] private readonly WebClient _webClient = new WebClient();
        [NotNull] private readonly object _baseUrl;

        public OedClient([NotNull] ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            _baseUrl = settings.OedApiUrl;
            
            _webClient.Headers.Add(HttpRequestHeader.Accept, "application/json");
            _webClient.Headers.Add("app_id", $"{settings.OedAppId}");
            _webClient.Headers.Add("app_key", $"{settings.OedApiKey}");
        }
        

        public string FetchDesignation(string word)
        {
            string result;

            try
            {
                result = _webClient.DownloadString($"{_baseUrl}/inflections/en/{Uri.EscapeUriString(word)}");
            }
            catch (WebException exception) when (
                (exception.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                result = null;
            }
            return result;
        }


        public string FetchEntries(string word)
        {
            string result;

            try
            {
                result = _webClient.DownloadString($"{_baseUrl}/entries/en/{Uri.EscapeUriString(word)}");
            }
            catch (WebException exception) when (
                (exception.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                result = null;
            }
            return result;
        }
    }
}