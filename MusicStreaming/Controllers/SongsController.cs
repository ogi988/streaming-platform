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
    public class SongsController : Controller
    {
        public ApplicationDbContext _context;

        public SongsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Songs
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            var artists = _context.Artists.ToList();
            var genres = _context.Genres.ToList();
            var songs = _context.Songs.ToList();
            if (User.IsInRole(RoleName.Admin))
                return View(songs);
            ViewBag.CurrentFilter = searchString;
            if (searchString == null)
            {
                searchString = currentFilter;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                var sorted = songs.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
                return View("UserIndex", sorted.ToPagedList(pageNumber, pageSize));
            }

            return View("UserIndex", songs.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = RoleName.Admin)]
        public ActionResult New()
        {
            var genres = _context.Genres.ToList();
            var artists = _context.Artists.Select(a => new
            {
                ArtistId = a.ArtistId,
                Name = a.Name
            }).ToList();
            var viewModel = new SongViewModel
            {
                Song = new Song(),
                Artists = new MultiSelectList(artists, "ArtistID", "Name"),
                Genres = genres
            };
            return View("SongForm",viewModel);
        }
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult Edit(int id)
        {
            var song = _context.Songs.SingleOrDefault(s => s.SongId == id);
            if (song == null)
                return HttpNotFound();
            var genres = _context.Genres.ToList();
            var artists = _context.Artists.Select(a => new
            {
                ArtistId = a.ArtistId,
                Name = a.Name
            }).ToList();
            var defaultSelected = _context.Artists.Where(x => x.Songs.Any(y => y.SongId == id));

            int[] artistsId = new int[defaultSelected.Count()];
            int i = 0;
            foreach (var a in defaultSelected)
            {
                artistsId[i] = a.ArtistId;
                i++;
            }
            var viewModel = new SongViewModel
            {
                Song = song,
                Artists = new MultiSelectList(artists, "ArtistID", "Name"),
                Genres = genres,
                ArtistId = artistsId

            };
            return View("SongForm", viewModel);
        }
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult Delete(int id)
        {
            var song = _context.Songs.SingleOrDefault(s => s.SongId == id);
            if (song == null)
                return HttpNotFound();
            RemoveManyToMany(song);
            _context.Songs.Remove(song);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = RoleName.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(HttpPostedFileBase file, Song song, int[] ArtistId)
        {

            if (song.SongId == 0)
            {
                if (ModelState.IsValid)
                {

                    if (file != null)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var directoryToSave = Server.MapPath(Url.Content("~/Uploads/Songs"));

                        var pathToSave = Path.Combine(directoryToSave, fileName);
                        file.SaveAs(pathToSave);
                        song.SongUrl = "~/Uploads/Songs/" + fileName;
                    }
                    _context.Songs.Add(song);
                    InserManyToMany(song, ArtistId);
                    return RedirectToAction("Index");
                }
                var genres = _context.Genres.ToList();
                var artists = _context.Artists.Select(a => new
                {
                    ArtistId = a.ArtistId,
                    Name = a.Name
                }).ToList();
                var viewModel = new SongViewModel
                {
                    Song = song,
                    Artists = new MultiSelectList(artists, "ArtistID", "Name"),
                    Genres = genres
                };
                return View("SongForm", viewModel);

            }
            else
            {

                var currentSong = _context.Songs.SingleOrDefault(s => s.SongId == song.SongId);
                if (currentSong == null)
                    return HttpNotFound();
                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var directoryToSave = Server.MapPath(Url.Content("~/Uploads/Songs"));

                        var pathToSave = Path.Combine(directoryToSave, fileName);
                        file.SaveAs(pathToSave);
                        currentSong.SongUrl = "~/Uploads/Songs/" + fileName;
                    }
                    currentSong.Name = song.Name;
                    currentSong.GenreId = song.GenreId;

                    RemoveManyToMany(currentSong);
                    InserManyToMany(currentSong, ArtistId);
                    return RedirectToAction("Index");
                }
                var genres = _context.Genres.ToList();
                var artists = _context.Artists.Select(a => new
                {
                    ArtistId = a.ArtistId,
                    Name = a.Name
                }).ToList();
                
                var viewModel = new SongViewModel
                {
                    Song = song,
                    Artists = new MultiSelectList(artists, "ArtistID", "Name"),
                    Genres = genres,
                    ArtistId = ArtistId

                };
                return View("SongForm", viewModel);

            }

        }
        public void InserManyToMany(Song song, int[] ArtistId)
        {

            for (int i = 0; i < ArtistId.Length; i++)
            {
                int id = ArtistId[i];
                var a = _context.Artists.SingleOrDefault(x => x.ArtistId == id);
                _context.Artists.Add(a);
                _context.Artists.Attach(a);
                song.Artists.Add(a);
            }
            _context.SaveChanges();
        }
        public void RemoveManyToMany(Song song)
        {
            List<int> artistIds = new List<int>();
            foreach (var a in song.Artists)
            {
                artistIds.Add(a.ArtistId);
            }
            foreach (var a in artistIds)
            {
                var artist = _context.Artists.SingleOrDefault(x => x.ArtistId == a);
                song.Artists.Remove(artist);
            }
            _context.SaveChanges();
        }
    }
}