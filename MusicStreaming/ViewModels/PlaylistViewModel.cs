using MusicStreaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStreaming.ViewModels
{
    public class PlaylistViewModel
    {
        public IEnumerable<Playlist> MyPlaylists { get; set; }
        public IEnumerable<Playlist> AllPlaylists { get; set; }
    }
}