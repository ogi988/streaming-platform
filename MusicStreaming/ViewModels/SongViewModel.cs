using MusicStreaming.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStreaming.ViewModels
{
    public class SongViewModel
    {
        public Song Song { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        [DisplayName("Artists")]
        [Required]
        public int[] ArtistId { get; set; }
        public MultiSelectList Artists { get; set; }
    }
}