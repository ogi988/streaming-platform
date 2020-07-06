using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicStreaming.Models
{
    public class Release
    {
        public Release()
        {
            this.Songs = new HashSet<Song>();
        }
        public int ReleaseId { get; set; }
        [Required][StringLength(30)]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public byte Type { get; set; }
        [DisplayName("Artist")]
        public int ArtistID { get; set; }
        public Artist Artist { get; set; }
        [Required]
        [DisplayName("Release Type")]
        public int ReleaseTypeId { get; set; }
        public ReleaseType ReleaseType { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}