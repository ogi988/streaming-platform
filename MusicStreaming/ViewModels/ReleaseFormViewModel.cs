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
    public class ReleaseFormViewModel
    {
        public IEnumerable<Artist> Artists { get; set; }
        public IEnumerable<ReleaseType> ReleaseTypes { get; set; }
        public Release Release { get; set; }
        [DisplayName("Songs")]
        [Required]
        public int[] SongId { get; set; }
        public MultiSelectList Songs { get; set; }
    }
}