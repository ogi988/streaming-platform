using MusicStreaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStreaming.ViewModels
{
    public class ReleasesViewModel
    {
        public IEnumerable<Artist> Artists { get; set; }
        public IEnumerable<ReleaseType> ReleaseTypes { get; set; }
        public IEnumerable<Release> Releases { get; set; }
        
    }
}