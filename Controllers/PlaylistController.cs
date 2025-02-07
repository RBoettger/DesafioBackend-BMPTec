using DesafioBackend.Model;
using DesafioBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace DesafioBackend.Controllers
{
    [Route("api/playlists")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _service;

        public PlaylistController(IPlaylistService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<PlaylistModel>>> Buscar()
        {
            var playlists = await _service.BuscarPlaylists();
            return Ok(playlists);
        }

        [HttpGet("buscar/{termo}")]
        public async Task<ActionResult<List<PlaylistModel>>> BuscarArtistaOuMusica(string termo)
        {
            var resultado = await _service.BuscarArtistaOuMusica(termo);
            return Ok(resultado);
        }

        [HttpGet("generos")]
        public async Task<ActionResult<List<string>>> ExibirGeneros()
        {
            var generos = await _service.ExibirGeneros();
            return Ok(generos);
        }

        [HttpGet("artistas/{genero}")]
        public async Task<ActionResult<List<string>>> FiltrarArtistasPorGenero(string genero)
        {
            var artistas = await _service.FiltrarArtistasPorGenero(genero);
            return Ok(artistas);
        }

        [HttpGet("musicas/{artista}")]
        public async Task<ActionResult<List<PlaylistModel>>> FiltrarMusicasDeUmArtista(string artista)
        {
            var musicas = await _service.FiltrarMusicasDeUmArtista(artista);
            return Ok(musicas);
        }

        [HttpGet("historico")]
        public async Task<ActionResult<List<HistoricoPesquisa>>> BuscarHistorico()
        {
            var historico = await _service.Historico("");
            return Ok(historico);
        }
    }
}
