using MusicStreaming.Models;
using MusicStreaming.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MusicStreaming.Controllers.Api
{
    [Authorize(Roles = RoleName.Admin)]
    public class ArtistsController : ApiController
    {
        private ApplicationDbContext _context;
        
        public ArtistsController()
        {
            _context = new ApplicationDbContext();
        }
        //GET /api/artists
        public IEnumerable<Artist> GetArtists()
        {
            return _context.Artists.ToList();
        }

        //[HttpPost]
        //public IHttpActionResult PostNewArtist([FromBody]ArtistApiViewModel artist)
        //{
        //    //if (!ModelState.IsValid)
        //    //    throw new HttpResponseException(HttpStatusCode.BadRequest);

        //    //if (file != null)
        //    //{
        //    //    var fileName = Path.GetFileName(file.FileName);
        //    //    var ctx = HttpContext.Current;
        //    //    var directoryToSave = ctx.Server.MapPath(Url.Content("~/Uploads/ArtistImages"));

        //    //    var pathToSave = Path.Combine(directoryToSave, fileName);
        //    //    file.SaveAs(pathToSave);
        //    //    artist.ImgUrl = "~/Uploads/ArtistImages/" + fileName;
        //    //}
        //    //_context.Artists.Add(artist);
        //    //_context.SaveChanges();

        //    return Ok();
        //}
        [HttpPost]
        public async Task<HttpResponseMessage> PostNewArtist()
        {

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/Uploads/ArtistImages");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                var name = "";
                string fileName = "";

                foreach (var key in provider.FormData.AllKeys)
                {
                    if(key == "Name")
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                            name = val;
                        Trace.WriteLine(string.Format("{0}: {1}", key, val));
                    }
                }
                foreach (MultipartFileData fileData in provider.FileData)
                {

                    fileName = fileData.Headers.ContentDisposition.FileName;
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }
                    File.Move(fileData.LocalFileName, Path.Combine(root, fileName));

                }
                var artist = new Artist
                {
                    Name = name,
                    ImgUrl = "~/Uploads/ArtistImages/" + fileName
                };
                _context.Artists.Add(artist);
                _context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
        [HttpDelete]
        public IHttpActionResult DeleteArtist(int id)
        {
            var currentArtist = _context.Artists.SingleOrDefault(a => a.ArtistId == id);

            if (currentArtist == null)
                return NotFound();

            _context.Artists.Remove(currentArtist);
            _context.SaveChanges();

            return Ok();
        }
    }
}
