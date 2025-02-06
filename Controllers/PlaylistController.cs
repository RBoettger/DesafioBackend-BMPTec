using DesafioBackend.Model;
using DesafioBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace DesafioBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private IPlaylistService _service;

        public PlaylistController(IPlaylistService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult<List<PlaylistModel>>> Get()
        {
            var playlists = await _service.BuscarPlaylists();
            return Ok(playlists);
        }

        [HttpGet("Exibir genero")]
        public async Task<ActionResult<List<PlaylistModel>>> GetGenre()
        {
            var genre = await _service.ExibirGeneros();
            return Ok(genre);
        }

        [HttpGet("Filtrar por genero")]
        public async Task<ActionResult<List<string>>> GetFilterForGenre()
        {
            var filter = await _service.FiltrarArtistasPorGenero();
            return Ok(filter);
        }

        [HttpGet("Filtrar musicas por artista")]
        public async Task<ActionResult<List<PlaylistModel>>> GetSongForArtist(string artist)
        {
            var songs = await _service.FiltrarMusicasDeUmArtista(artist);
            return Ok(songs);
        }

    }
}
