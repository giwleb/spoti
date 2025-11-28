using System.Globalization;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using SpotifyAPI.Web;
class Program
{
    public static SpotifyClient _spotify;

    static async Task Main(string[] args)
    {
        Console.WriteLine("Start");

        var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
        var configuration = builder.Build();

        var clientId = configuration["Spotify:ClientId"] ?? string.Empty;
        var clientSecret = configuration["Spotify:ClientSecret"] ?? string.Empty;

        var spotifyConfig = SpotifyClientConfig.CreateDefault();
        var request = new ClientCredentialsRequest(clientId, clientSecret);
        var response = await new OAuthClient(spotifyConfig).RequestToken(request);
        _spotify = new SpotifyClient(spotifyConfig.WithToken(response.AccessToken));

        await FetchPlAsync("0BBjPYvDcKOUF1P5GRB8W7", "/mnt/c/junk/xmas.csv");
        await FetchPlAsync("5hFUdgYsZrd1eAsOufSbEF", "/mnt/c/junk/xmas2024.csv");

        Console.WriteLine("End");
    }

    static async Task FetchPlAsync(string plid, string filepath)
    {
        var pldata = await _spotify.Playlists.GetItems(plid);        
        var items = await _spotify.Paginate(pldata).ToListAsync(); // this pulls the entire list, since I need it for the max
        var tracks = items.Select(i => i.Track).OfType<FullTrack>();
        var maxArtists = tracks.Max(x => x.Artists.Count);

        using (var writer = new StreamWriter(filepath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteField("Song");
            csv.WriteField("Album");
            for (int i = 1; i <= maxArtists; i++)
            {
                csv.WriteField($"Artist{i}");
            }
            csv.NextRecord();
            foreach(var track in tracks)
            {  
                csv.WriteField(track.Name);   
                csv.WriteField(track.Album.Name);  
            
                var artistsCount = track.Artists.Count;

                for (int i = 1; i <= track.Artists.Count; i++)
                {
                    csv.WriteField(track.Artists[i-1].Name);
                }
                for (int i = track.Artists.Count + 1; i <= maxArtists; i++)
                {
                    csv.WriteField(string.Empty);
                }

                csv.NextRecord();
            }  
        }
    }
}