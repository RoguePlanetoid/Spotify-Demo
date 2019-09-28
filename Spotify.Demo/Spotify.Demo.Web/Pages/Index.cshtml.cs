using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Spotify.Demo.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public dynamic NewReleases { get; set; }

        public dynamic FollowedArtists { get; set; }

        public async Task OnGetAsync()
        {
	        var token = "<token>";
	        using (var client = new HttpClient())
	        {
		        client.DefaultRequestHeaders.Authorization =
		        new AuthenticationHeaderValue("Bearer", token);
		        var response = await client.GetAsync(new Uri(
			        "https://api.spotify.com/v1/browse/new-releases?country=GB"
		        ));
                NewReleases = await response.Content.ReadAsAsync<dynamic>();
            }
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(new Uri(
                    "https://api.spotify.com/v1/me/following?type=artist"
                ));
                FollowedArtists = await response.Content.ReadAsAsync<dynamic>();
            }
        }
    }
}
