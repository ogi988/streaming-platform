using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicStreaming.Models
{
    
    public class Song
    {
        public Song()
        {
            this.Playlists = new HashSet<Playlist>();
            this.Releases = new HashSet<Release>();
            this.Artists = new HashSet<Artist>();
        }
        public int SongId { get; set; }
        [Required][StringLength(30)]
        public string Name { get; set; }
        public string SongUrl { get; set; }
        [Required]
        [DisplayName("Genre")]
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; }
        public virtual ICollection<Release> Releases { get; set; }
        public virtual ICollection<Artist> Artists { get; set; }
    }
}