using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStreaming.Models
{
    public class ReleaseType
    {
        public int ReleaseTypeId { get; set; }
        public string Name { get; set; }
        public ICollection<Release> Release { get; set; }
    }
}