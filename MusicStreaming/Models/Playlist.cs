using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MusicStreaming.Models
{
    public class Playlist
    {
        public Playlist()
        {
            this.Songs = new HashSet<Song>();
            this.Users = new HashSet<ApplicationUser>();
        }

        public int PlaylistId { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }

}