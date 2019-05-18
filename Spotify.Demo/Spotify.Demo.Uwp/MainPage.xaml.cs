using Spotify.NetStandard.Client;
using Spotify.NetStandard.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Spotify.Demo.Uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private readonly ISpotifyClient client = SpotifyClientFactory.CreateSpotifyClient(
        Settings.ClientId, Settings.ClientSecret);
        private readonly Uri redirect_url = new Uri("http://example.com/callback");
        private const string state = "hainton";

    public enum Option
    {
	    NewReleases,
	    FeaturedPlaylists,
	    FollowedArtists,
	    SavedAlbums
    }

        private void Display_Loaded(object sender, RoutedEventArgs e) =>
            WebView.Navigate(client.AuthUser(redirect_url, state,
            new NetStandard.Requests.Scope { FollowRead = true, LibraryRead = true }));

        private async void WebView_NavigationCompleted(WebView sender,
            WebViewNavigationCompletedEventArgs e)
        {
            if (e.Uri.Host == redirect_url.Host)
            {
                WebView.Visibility = Visibility.Collapsed;
                if (await client.AuthUserAsync(e.Uri, redirect_url, state) != null)
                    Options.ItemsSource = Enum.GetValues(typeof(Option)).Cast<Option>();
            }
        }

        private async void Options_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var option = (Option)Options.SelectedItem;
            var cursor = new NetStandard.Requests.Cursor() { Limit = 10 };
            var page = new NetStandard.Requests.Page() { Limit = 10 };
            switch (option)
            {
                case Option.NewReleases:
                    Display.ItemsSource = (await client.LookupNewReleasesAsync(page: page)).Albums.Items;
                    break;
                case Option.FollowedArtists:
                    Display.ItemsSource = (await client.AuthLookupFollowedArtistsAsync(cursor: cursor)).Items;
                    break;
                case Option.FeaturedPlaylists:
	                Display.ItemsSource = (await client.LookupFeaturedPlaylistsAsync(page: page)).Playlists.Items;
	                break;
                case Option.SavedAlbums:
	                Display.ItemsSource = (await client.AuthLookupUserSavedAlbumsAsync(cursor: cursor)).Items.Select(x=> x.Album);
	                break;
            }
        }
    }
}
