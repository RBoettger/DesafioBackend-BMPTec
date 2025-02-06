using System.Text.Json;
using DesafioBackend.Model;

namespace DesafioBackend.Services
{
    public class PlaylistService : IPlaylistService
    {
        private const string endpoint = "https://guilhermeonrails.github.io/api-csharp-songs/songs.json";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _options;

        public PlaylistService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<PlaylistModel>> BuscarPlaylists()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                using var response = await client.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao acessar API: {response.StatusCode}");
                    return new List<PlaylistModel>();
                }

                var apiResponse = await response.Content.ReadAsStreamAsync();
                var playlistResponse = JsonSerializer.Deserialize<List<PlaylistModel>>(apiResponse, _options);

                return playlistResponse.OrderBy(p => p.Artist).ToList() ??
                                    new List<PlaylistModel>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro na requisição HTTP: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro ao desserializar JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }

            return new List<PlaylistModel>();
        }

        public async Task<List<string>> ExibirGeneros()
        {
            try
            {
                var playlists = await BuscarPlaylists();
                return playlists.Select(p => p.Genre)
                                .Where(g => !string.IsNullOrEmpty(g))
                                .Distinct()
                                .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exibir gêneros: {ex.Message}");
                return new List<string>();
            }
        }

        public async Task<List<string>> FiltrarArtistasPorGenero()
        {
            try
            {
                var playlists = await BuscarPlaylists();
                return playlists.OrderBy(p => p.Genre)
                                .Where(p => !string.IsNullOrEmpty(p.Genre) && !string.IsNullOrEmpty(p.Artist))
                                .Select(p => $"{p.Genre} - {p.Artist}")
                                .Distinct()
                                .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao filtrar artistas por gênero: {ex.Message}");
                return new List<string>();
            }
        }

        public async Task<List<PlaylistModel>> FiltrarMusicasDeUmArtista(string nome)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                {
                    Console.WriteLine("Nome do artista não pode ser vazio ou nulo.");
                    return new List<PlaylistModel>();
                }

                var playlists = await BuscarPlaylists();
                return playlists.Where(p => !string.IsNullOrEmpty(p.Artist) &&
                                            p.Artist.Contains(nome, StringComparison.OrdinalIgnoreCase))
                                .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao filtrar músicas de um artista: {ex.Message}");
                return new List<PlaylistModel>();
            }
        }
    }
}
