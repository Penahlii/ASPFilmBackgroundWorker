using Newtonsoft.Json.Linq;

namespace FilmBackgroundWorker.Services;

public class OmdbApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "5492d64a"; 

    public OmdbApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<JObject> SearchFilmsAsync(string letter, int page = 1)
    {
        var response = await _httpClient.GetStringAsync($"http://www.omdbapi.com/?s={letter}&page={page}&apikey={_apiKey}");
        return JObject.Parse(response);
    }
}
