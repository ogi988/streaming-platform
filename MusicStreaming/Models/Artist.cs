using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStreaming.Models
{
    public class Artist
    {
        public Artist()
        {
            this.Songs = new HashSet<Song>();
        }
        public int ArtistId { get; set; }
        [Required][StringLength(30)]
        public string Name { get; set; }
        [DisplayName("Upload Image")]
        public string ImgUrl { get; set; }


        public ICollection<Song> Songs { get; set; }
        public ICollection<Release> Release { get; set; }
    }
}