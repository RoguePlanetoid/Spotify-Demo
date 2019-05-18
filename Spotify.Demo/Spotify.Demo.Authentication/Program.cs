using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Demo.Authentication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var token = string.Empty;
            using (var auth = new HttpClient())
            {
                auth.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(
                $"{Settings.ClientId}:{Settings.ClientSecret}")));
                var response = await auth.PostAsync(new Uri("https://accounts.spotify.com/api/token"),
                new FormUrlEncodedContent(new Dictionary<string, string>()
                { { "grant_type", "client_credentials" } }));
                token = Regex.Match(await response.Content.ReadAsStringAsync(),
                ".*\"access_token\":\"(.*?)\".*", RegexOptions.IgnoreCase).Groups[1].Value;
            }
            Console.WriteLine(token);
            Console.ReadLine();
        }
    }
}
