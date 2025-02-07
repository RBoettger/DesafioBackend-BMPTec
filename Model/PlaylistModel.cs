using System.ComponentModel.DataAnnotations;

namespace DesafioBackend.Model
{
    public class PlaylistModel
    {
        public int Id { get; set; }
        public string? Artist {  get; set; }
        public string? Song { get; set; }
        public string? Genre { get; set; }
    }
}
