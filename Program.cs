using System.Globalization;
using CsvHelper;
using SpotifyAPI.Web;

Console.WriteLine("Start");

var config = SpotifyClientConfig.CreateDefault();
var request = new ClientCredentialsRequest("x", "x");
var response = await new OAuthClient(config).RequestToken(request);
var spotify = new SpotifyClient(config.WithToken(response.AccessToken));

var xmasMegaPlId = "0BBjPYvDcKOUF1P5GRB8W7";
var xmas2021 = "2f2n3QUliEghhxDpI64XXP";
var plid = "2f2n3QUliEghhxDpI64XXP";

var pldata = await spotify.Playlists.GetItems(plid);
//var mypls = await spotify.Playlists.CurrentUsers();

var items = await spotify.Paginate(pldata).ToListAsync();
var tracks = items.Select(i => i.Track).OfType<FullTrack>();
var maxArtists = tracks.Max(x => x.Artists.Count);

using (var writer = new StreamWriter("c:\\junk\\xmas2021.csv"))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteField("Song");
    csv.WriteField("Album");
    csv.WriteField("Artist1");
    csv.WriteField("Artist2");
    csv.WriteField("Artist3");
    csv.WriteField("Artist4");
    csv.WriteField("Artist5");
    csv.WriteField("Artist6");
    csv.WriteField("Artist7");
    csv.WriteField("Artist8");
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



Console.WriteLine("End");