using MusicStreaming.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStreaming.ViewModels
{
    public class PlaylistFormViewModel
    {
        public Playlist Playlist { get; set; }
        [DisplayName("Songs")]
        public int[] SongId { get; set; }
        public MultiSelectList Songs { get; set; }
    }
}