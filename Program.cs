using System.Globalization;
using CsvHelper;
using SpotifyAPI.Web;
class Program
{
    public static SpotifyClient _spotify;

    static async Task Main(string[] args)
    {
        Console.WriteLine("Start");

        var config = SpotifyClientConfig.CreateDefault();
        var request = new ClientCredentialsRequest("x", "x");
        var response = await new OAuthClient(config).RequestToken(request);
        _spotify = new SpotifyClient(config.WithToken(response.AccessToken));

        await FetchPlAsync("0BBjPYvDcKOUF1P5GRB8W7", "c:\\junk\\xmas.csv");
        await FetchPlAsync("2f2n3QUliEghhxDpI64XXP", "c:\\junk\\xmas2021.csv");

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