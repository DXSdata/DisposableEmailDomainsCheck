using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace DisposableEmailDomainsCheck
{
    public static class DisposableEmailDomains
    {
        static List<String> list = new List<string>();
        public static String IndexUrl = "https://github.com/ivolo/disposable-email-domains/raw/master/index.json";
        public static String TempFile = Path.GetTempPath() + "DisposableEmailDomains.index.json";
        private static IndexState _state = IndexState.NoContent;

        static DisposableEmailDomains()
        {
            GetData();
        }

        /// <summary>
        /// Check if given email or hostname address is listed in the Disposable Email Domains Index
        /// </summary>
        /// <param name="EmailOrHostname"></param>
        /// <returns></returns>
        public static bool Contains(String EmailOrHostname)
        {
            if (!IsReady)
                throw new AccessViolationException("Not ready: " + State.ToString());

            String hostname = EmailOrHostname.Trim().ToLower();

            if (hostname.Contains('@'))
                hostname = hostname.Split("@".ToCharArray(), 2, StringSplitOptions.None)[1];

            return list.Any(o => hostname.EndsWith(o));
        }
        

        /// <summary>
        /// Prepare index
        /// </summary>
        /// <returns></returns>
        private static async Task GetData()
        {
            _state = IndexState.Downloading;

            String content = await DownloadFile(IndexUrl);

            //try to get previously downloaded file 
            if (string.IsNullOrWhiteSpace(content))
            {
                if (File.Exists(TempFile))
                {
                    content = File.ReadAllText(TempFile);
                    _state = IndexState.OkUsingCache;
                }
            }
            else
                File.WriteAllText(TempFile, content);

            ParseJson(content);

            if (!IsReady)
                _state = IndexState.NoContent;

            if (IsReady && _state != IndexState.OkUsingCache)
                _state = IndexState.OK;
        }


        /// <summary>
        /// New index download
        /// </summary>
        /// <returns></returns>
        public static async Task RefreshIndex()
        {
            await GetData();
        }


        /// <summary>
        /// Download file from web
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static async Task<string> DownloadFile(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    using (var result = await client.GetAsync(url))
                    {
                        if (result.IsSuccessStatusCode)
                        {
                            return await result.Content.ReadAsStringAsync();
                        }

                    }
                }
            }
            catch { }

            return null;
        }


        /// <summary>
        /// Parse raw json data into string list
        /// </summary>
        /// <param name="RawJson"></param>
        private static void ParseJson(String RawJson)
        {
            list = JsonConvert.DeserializeObject<List<String>>(RawJson);
        }


        /// <summary>
        /// Checks if index has content
        /// </summary>
        public static bool IsReady
        {
            get
            {
                return (list.Count() > 0);
            }
        }

        /// <summary>
        /// Current index loading state
        /// </summary>
        public static IndexState State
        {
            get
            {
                return _state;
            }
        }
        
        public enum IndexState
        {
            Downloading,
            OK,
            OkUsingCache,
            NoContent
        }
    }
}
