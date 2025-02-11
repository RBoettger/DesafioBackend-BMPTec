using DesafioBackend.Model;

namespace DesafioBackend.Services
{
    public interface IPlaylistService
    {
        Task<List<PlaylistModel>> BuscarPlaylists();
        Task<List<PlaylistModel>> BuscarArtistaOuMusica(string artistOrSong);
        Task<List<string>> FiltrarArtistasPorGenero(string genre);
        Task<List<PlaylistModel>> FiltrarMusicasDeUmArtista(string name);
        Task<List<string>> ExibirGeneros();
        Task<List<HistoricoPesquisa>> Historico(string artistOrSong);
    }
}
