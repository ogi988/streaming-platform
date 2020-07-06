using MusicStreaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStreaming.ViewModels
{
    public class ArtistDetailsViewModel
    {
        public Artist Artist { get; set; }
        public IEnumerable<Song> Songs { get; set; }
        public IEnumerable<Release> Releases { get; set; }
    }
}