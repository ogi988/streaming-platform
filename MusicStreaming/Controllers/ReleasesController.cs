using MusicStreaming.Models;
using MusicStreaming.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStreaming.Controllers
{
    public class ReleasesController : Controller
    {
        public ApplicationDbContext _context;

        public ReleasesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            var artists = _context.Artists.ToList();
            var releaseTypes = _context.ReleaseTypes.ToList();
            var releases = _context.Releases.ToList();

            foreach (var r in releases)
            {
                if (r.ImageUrl == null)
                {
                    r.ImageUrl = @"~/Uploads/ReleaseImages/default.png";
                }
            }
            if (User.IsInRole(RoleName.Admin))
                return View(releases);

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
                var sorted = releases.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
                return View("UserIndex", sorted.ToPagedList(pageNumber, pageSize));
            }
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name" : "";
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date" : "";
            if (sortOrder == "name")
            {
                var sorted = releases.OrderByDescending(a => a.Name);
                return View("UserIndex", sorted.ToPagedList(pageNumber, pageSize));
            }
            else if(sortOrder == "date")
            {
                var sorted = releases.OrderByDescending(a => a.CreatedAt);
                return View("UserIndex", sorted.ToPagedList(pageNumber, pageSize));
            }
            var sort = releases.OrderBy(a => a.Name);
            return View("UserIndex", sort.ToPagedList(pageNumber, pageSize));

        }
        public ActionResult Details(int id, string currentFilter, string searchString)
        {
            var release = _context.Releases.ToList();
            var songs = _context.Songs.Where(x => x.Releases.Any(y => y.ReleaseId == id));
            var artists = _context.Artists.ToList();
            var genres = _context.Genres.ToList();
            ViewBag.CurrentFilter = searchString;
            if (searchString == null)
            {
                searchString = currentFilter;
            }
            
            if (!String.IsNullOrEmpty(searchString))
            {
                var sorted = songs.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
                return View(sorted);
            }

            return View(songs);
        }
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult New()
        {
            var artists = _context.Artists.ToList();
            var releaseTypes = _context.ReleaseTypes.ToList();
            var songs = _context.Songs.Select(a => new
            {
                SongId = a.SongId,
                Name = a.Name
            }).ToList();
            var release = new Release();
            release.ImageUrl = @"~/Uploads/ReleaseImages/default.png";
            var viewModel = new ReleaseFormViewModel
            {
                Release = release,
                Artists = artists,
                ReleaseTypes = releaseTypes,
                Songs = new MultiSelectList(songs, "SongId", "Name"),

            };
            return View("ReleaseForm", viewModel);
        }
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult Edit(int id)
        {
            var release = _context.Releases.SingleOrDefault(r => r.ReleaseId == id);
            if (release == null)
                return HttpNotFound();
            var artists = _context.Artists.ToList();
            var releaseTypes = _context.ReleaseTypes.ToList();
            var songs = _context.Songs.Select(a => new
            {
                SongId = a.SongId,
                Name = a.Name
            }).ToList();

            var defaultSelected = _context.Songs.Where(x => x.Releases.Any(y => y.ReleaseId == id));
            int[] songsId = new int[defaultSelected.Count()];
            int i = 0;
            foreach (var a in defaultSelected)
            {
                songsId[i] = a.SongId;
                i++;
            }
            if(release.ImageUrl == null)
            {
                release.ImageUrl = @"~/Uploads/ReleaseImages/default.png";
            }
            var viewModel = new ReleaseFormViewModel
            {
                Release = release,
                ReleaseTypes = releaseTypes,
                Artists = artists,
                Songs = new MultiSelectList(songs, "SongId", "Name"),
                SongId = songsId
            };

            return View("ReleaseForm", viewModel);
        }
        [Authorize(Roles = RoleName.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Release release, HttpPostedFileBase file, int[] SongId)
        {
            if (release.ReleaseId == 0)
            {
                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var directoryToSave = Server.MapPath(Url.Content("~/Uploads/ReleaseImages"));

                        var pathToSave = Path.Combine(directoryToSave, fileName);
                        file.SaveAs(pathToSave);
                        release.ImageUrl = "~/Uploads/ReleaseImages/" + fileName;
                    }
                    _context.Releases.Add(release);
                    InsertManyToMany(release, SongId);
                    return RedirectToAction("Index");
                }
                var artists = _context.Artists.ToList();
                var releaseTypes = _context.ReleaseTypes.ToList();
                var songs = _context.Songs.Select(a => new
                {
                    SongId = a.SongId,
                    Name = a.Name
                }).ToList();
                if(release.ImageUrl == null)
                {
                    release.ImageUrl = @"~/Uploads/ReleaseImages/default.png";
                }
                var viewModel = new ReleaseFormViewModel
                {
                    Release = release,
                    Artists = artists,
                    ReleaseTypes = releaseTypes,
                    Songs = new MultiSelectList(songs, "SongId", "Name"),

                };
                return View("ReleaseForm", viewModel);
            }
            else
            {
                var currentRelease = _context.Releases.SingleOrDefault(r => r.ReleaseId == release.ReleaseId);
                if (currentRelease == null)
                    return HttpNotFound();
                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var directoryToSave = Server.MapPath(Url.Content("~/Uploads/ReleaseImages"));

                        var pathToSave = Path.Combine(directoryToSave, fileName);
                        file.SaveAs(pathToSave);
                        currentRelease.ImageUrl = "~/Uploads/ReleaseImages/" + fileName;
                    }
                    currentRelease.Name = release.Name;
                    currentRelease.ReleaseTypeId = release.ReleaseTypeId;
                    currentRelease.ArtistID = release.ArtistID;
                    RemoveManyToMany(currentRelease);
                    InsertManyToMany(currentRelease, SongId);
                    return RedirectToAction("Index");
                }
                var artists = _context.Artists.ToList();
                var releaseTypes = _context.ReleaseTypes.ToList();
                var songs = _context.Songs.Select(a => new
                {
                    SongId = a.SongId,
                    Name = a.Name
                }).ToList();
                if (release.ImageUrl == null)
                {
                    currentRelease.ImageUrl = @"~/Uploads/ReleaseImages/default.png";
                }
                var viewModel = new ReleaseFormViewModel
                {
                    Release = currentRelease,
                    ReleaseTypes = releaseTypes,
                    Artists = artists,
                    Songs = new MultiSelectList(songs, "SongId", "Name"),
                    SongId = SongId
                };
                return View("ReleaseForm", viewModel);
            }

        }
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult Delete(int id)
        {
            var release = _context.Releases.SingleOrDefault(r => r.ReleaseId == id);
            if (release == null)
                return HttpNotFound();
            RemoveManyToMany(release);
            _context.Releases.Remove(release);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public void RemoveManyToMany(Release release)
        {
            List<int> songIds = new List<int>();
            foreach (var s in release.Songs)
            {
                songIds.Add(s.SongId);
            }
            foreach (var s in songIds)
            {
                var song = _context.Songs.SingleOrDefault(x => x.SongId == s);
                release.Songs.Remove(song);
            }
            _context.SaveChanges();
        }
        public void InsertManyToMany(Release release, int[] SongId)
        {
            for (int i = 0; i < SongId.Length; i++)
            {
                int id = SongId[i];
                var s = _context.Songs.SingleOrDefault(x => x.SongId == id);
                _context.Songs.Add(s);
                _context.Songs.Attach(s);
                release.Songs.Add(s);
            }
            _context.SaveChanges();
        }
        
    }
}