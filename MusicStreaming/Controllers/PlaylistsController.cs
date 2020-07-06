using Microsoft.AspNet.Identity;
using MusicStreaming.Models;
using MusicStreaming.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStreaming.Controllers
{
    public class PlaylistsController : Controller
    {
        public ApplicationDbContext _context;

        public PlaylistsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Playlists
        public ActionResult Index()
        {
            var allPlaylists = _context.Playlists.ToList();
            var allUsers = _context.Users.ToList();
            var currentUserId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.Id == currentUserId);
            var myPlaylists = _context.Playlists.ToList();
            var my = myPlaylists.Where(p => p.CreatedBy == user);
            var viewModel = new PlaylistViewModel
            {
                MyPlaylists = my,
                AllPlaylists = allPlaylists
            };
            
            return View(viewModel);
        }
        public ActionResult Details(int id, string currentFilter, string searchString)
        {
            var release = _context.Releases.ToList();
            var songs = _context.Songs.Where(x => x.Playlists.Any(y => y.PlaylistId == id));
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
        public ActionResult New()
        {
            var playlist = new Playlist();
            var songs = _context.Songs.Select(a => new
            {
                SongId = a.SongId,
                Name = a.Name
            }).ToList();
            var viewModel = new PlaylistFormViewModel
            {
                Playlist = playlist,
                Songs = new MultiSelectList(songs, "SongId", "Name")
            };
            return View("PlaylistForm", viewModel);
        }
        public ActionResult Edit(int id)
        {
            var playlist = _context.Playlists.SingleOrDefault(p => p.PlaylistId == id);
            if (playlist == null)
                return HttpNotFound();
            var songs = _context.Songs.Select(a => new
            {
                SongId = a.SongId,
                Name = a.Name
            }).ToList();

            var defaultSelected = _context.Songs.Where(x => x.Playlists.Any(y => y.PlaylistId == id));
            int[] songsId = new int[defaultSelected.Count()];
            int i = 0;
            foreach (var a in defaultSelected)
            {
                songsId[i] = a.SongId;
                i++;
            }
            var viewModel = new PlaylistFormViewModel
            {
                Playlist = playlist,
                Songs = new MultiSelectList(songs, "SongId", "Name"),
                SongId = songsId
            };

            return View("PlaylistForm", viewModel);
        }
        public ActionResult Save(Playlist playlist, int[] SongId)
        {
            if (ModelState.IsValid)
            {
                if (playlist.PlaylistId == 0)
                {
                    var currentUserId = User.Identity.GetUserId();
                    var user = _context.Users.SingleOrDefault(u => u.Id == currentUserId);
                    playlist.CreatedBy = user;
                    Random rnd = new Random();
                    int rndImg = rnd.Next(0, SongId.Length);
                    int songId = SongId[rndImg];
                    var randomSong = _context.Songs.SingleOrDefault(r => r.SongId == songId);
                    var x = randomSong.Releases.First().ImageUrl;
                    _context.Entry(randomSong).State = EntityState.Detached;
                    playlist.ImageUrl = x;
                    _context.Playlists.Add(playlist);
                    _context.SaveChanges();
                    InsertManyToManySong(playlist, SongId);


                    return RedirectToAction("Index");
                }
                else
                {
                    var currentPlaylist = _context.Playlists.SingleOrDefault(p => p.PlaylistId == playlist.PlaylistId);
                    if (currentPlaylist == null)
                        return HttpNotFound();
                    currentPlaylist.Name = playlist.Name;
                    RemoveManyToManySong(currentPlaylist);
                    InsertManyToManySong(currentPlaylist, SongId);
                    return RedirectToAction("Index");

                }
            }
            return RedirectToAction("New");

        }
        public ActionResult Delete(int id)
        {
            var playlist = _context.Playlists.SingleOrDefault(p => p.PlaylistId == id);
            if (playlist == null)
                return HttpNotFound();
            RemoveManyToManySong(playlist);
            RemoveManyToManyUser(playlist);
            _context.Playlists.Remove(playlist);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        public void InsertManyToManySong(Playlist playlist, int[] SongId)
        {
            for (int i = 0; i < SongId.Length; i++)
            {
                int id = SongId[i];
                var s = _context.Songs.SingleOrDefault(x => x.SongId == id);
                _context.Songs.Add(s);
                _context.Songs.Attach(s);
                playlist.Songs.Add(s);
            }
            _context.SaveChanges();
        }
        public void RemoveManyToManySong(Playlist playlist)
        {
            List<int> songIds = new List<int>();
            foreach (var s in playlist.Songs)
            {
                songIds.Add(s.SongId);
            }
            foreach (var s in songIds)
            {
                var song = _context.Songs.SingleOrDefault(x => x.SongId == s);
                playlist.Songs.Remove(song);
            }
            _context.SaveChanges();
        }
        public void RemoveManyToManyUser(Playlist playlist)
        {
            var currentUserId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.Id == currentUserId);
            playlist.Users.Remove(user);
            _context.SaveChanges();
        }
        public void InserManyToManyUser(Playlist playlist)
        {
            var currentUserId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.Id == currentUserId);

            _context.Users.Add(user);
            _context.Users.Attach(user);
            playlist.Users.Add(user);
            _context.SaveChanges();
        }
    }
}