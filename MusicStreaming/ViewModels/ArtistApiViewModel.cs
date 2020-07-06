using MusicStreaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStreaming.ViewModels
{
    public class ArtistApiViewModel
    {
        public Artist artist { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}