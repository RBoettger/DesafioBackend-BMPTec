using System.Text.Json;
using DesafioBackend.Db;
using DesafioBackend.Model;
using DesafioBackend.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackend.Services
{
    public class PlaylistService : IPlaylistService
    {
        private const string endpoint = "https://guilhermeonrails.github.io/api-csharp-songs/songs.json";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _options;
        private readonly AppDbContext _context;

        public PlaylistService(IHttpClientFactory httpClientFactory, AppDbContext context)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<HistoricoPesquisa>> Historico(string artistOrSong)
        {
            return await _context.HistoricoPesquisas
                .Where(h => h.TermoPesquisa.Contains(artistOrSong))
                .OrderByDescending(h => h.DataConsulta)
                .ToListAsync();
        }

        public async Task<List<PlaylistModel>> BuscarArtistaOuMusica(string artistOrSong)
        {
            var playlist = await BuscarPlaylists();

            var resultado = playlist
                .Where(p =>
                    (!string.IsNullOrEmpty(p.Artist) && p.Artist.Contains(artistOrSong, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(p.Song) && p.Song.Contains(artistOrSong, StringComparison.OrdinalIgnoreCase))
                )
                .OrderBy(p => p.Artist)
                .ToList();
            TipoDeBusca tipoDeBusca;
            if (resultado.Any(p => p.Artist.Contains(artistOrSong, StringComparison.OrdinalIgnoreCase)))
            {
                tipoDeBusca = TipoDeBusca.Artista;
            }
            else
            {
                tipoDeBusca = TipoDeBusca.Musica;
            }
            var historico = new HistoricoPesquisa
            {
                Tipo = tipoDeBusca,
                TermoPesquisa = artistOrSong,
                ResultadoJson = JsonSerializer.Serialize(resultado),
                DataConsulta = DateTime.Now
            };
            _context.HistoricoPesquisas.Add(historico);
                await _context.SaveChangesAsync();

                return resultado;
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
                return playlistResponse.ToList() ??
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

        public async Task<List<string>> FiltrarArtistasPorGenero(string genre)
        {
            try
            {
                var playlists = await BuscarPlaylists();
                return playlists.Where(p => !string.IsNullOrEmpty(p.Genre) && p.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(p.Artist))
                                .OrderBy(p => p.Artist)
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
