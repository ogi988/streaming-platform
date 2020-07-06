using MusicStreaming.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using MusicStreaming.ViewModels;

namespace MusicStreaming.Controllers
{
    public class ArtistsController : Controller
    {
        public ApplicationDbContext _context;

        public ArtistsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Artists
        [AllowAnonymous]
        public ActionResult Index(string sortOrder,string currentFilter, string searchString, int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            var artist = _context.Artists.ToList();
            if (User.IsInRole(RoleName.Admin))
                return View(artist);
            ViewBag.CurrentSort = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                var sorted  = artist.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
                return View("UserIndex", sorted.ToPagedList(pageNumber, pageSize));
            }
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name" : "";
            if (sortOrder == "name")
            {
                var sorted = artist.OrderByDescending(a => a.Name);
                return View("UserIndex", sorted.ToPagedList(pageNumber, pageSize));
            }
            var sort = artist.OrderBy(a => a.Name);

            return View("UserIndex", sort.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Details(int id)
        {
            var artist = _context.Artists.SingleOrDefault(x => x.ArtistId == id);
            var songs = _context.Songs.Where(x => x.Artists.Any(y => y.ArtistId == id));
            var releases = _context.Releases.Where(x => x.ArtistID == id).ToList();
            var re = _context.Releases.Where(x => x.Artist == artist);
            if (artist == null)
            {
                return HttpNotFound();
            }
            var viewModel = new ArtistDetailsViewModel
            {
                Artist = artist,
                Songs = songs,
                Releases = releases
            };
            return View(viewModel);
        }
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult New()
        {
            var artist = new Artist();
            artist.ImgUrl = @"~/Uploads/ArtistImages/default.png";
            return View("ArtistFormApi", artist);

        }
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult Edit(int id)
        {
            var artist = _context.Artists.SingleOrDefault(x => x.ArtistId == id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View("ArtistForm", artist);

        }
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult Delete(int id)
        {
            var artist = _context.Artists.SingleOrDefault(x => x.ArtistId == id);
            _context.Artists.Remove(artist);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = RoleName.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(HttpPostedFileBase file, Artist artist)
        {
            if (ModelState.IsValid)
            {
                if (artist.ArtistId == 0)
                {

                    if (file != null)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var directoryToSave = Server.MapPath(Url.Content("~/Uploads/ArtistImages"));

                        var pathToSave = Path.Combine(directoryToSave, fileName);
                        file.SaveAs(pathToSave);
                        artist.ImgUrl = "~/Uploads/ArtistImages/" + fileName;
                    }
                    _context.Artists.Add(artist);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    var currentArtist = _context.Artists.SingleOrDefault(a => a.ArtistId == artist.ArtistId);
                    if (currentArtist == null)
                        return HttpNotFound();
                    if (file != null)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var directoryToSave = Server.MapPath(Url.Content("~/Uploads/ArtistImages"));

                        var pathToSave = Path.Combine(directoryToSave, fileName);
                        file.SaveAs(pathToSave);
                        currentArtist.ImgUrl = "~/Uploads/ArtistImages/" + fileName;
                    }
                    currentArtist.Name = artist.Name;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            return View("ArtistForm", artist);



        }
    }
}