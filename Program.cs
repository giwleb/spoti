using SpotifyAPI.Web;

Console.WriteLine("Start");

var config = SpotifyClientConfig.CreateDefault();
var request = new ClientCredentialsRequest("x", "x");
var response = await new OAuthClient(config).RequestToken(request);
var spotify = new SpotifyClient(config.WithToken(response.AccessToken));

var xmasMegaPlId = "0BBjPYvDcKOUF1P5GRB8W7";

var pldata = await spotify.Playlists.GetItems(xmasMegaPlId);

var items = await spotify.Paginate(pldata).ToListAsync();
var tracks = items.Select(i => i.Track).OfType<FullTrack>();
var maxArtists = tracks.Max(x => x.Artists.Count);

foreach(var track in tracks)
{  
    string row = $"{track.Name},{track.Album.Name}";
    var artistsCount = track.Artists.Count;

    for (int i = 1; i <= track.Artists.Count; i++)
    {
        row += $",{track.Artists[i-1].Name}";
    }
    for (int i = track.Artists.Count + 1; i <= maxArtists; i++)
    {
        row += ",";
    }

    Console.WriteLine(row);    
}  



Console.WriteLine("End");