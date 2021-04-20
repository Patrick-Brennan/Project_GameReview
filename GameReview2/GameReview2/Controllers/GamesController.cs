using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using GameReview2.Helpers;
using GameReview2.Models;
using GameReview2.ViewModels;
using PagedList;

namespace GameReview2.Controllers
{
    public class GamesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /* public ActionResult Calculate()
         {
             return View("Index");
         }

         [HttpPost]
         public ActionResult Calculate(Game game)
         {
             using GameReview2.Models.CriticReview
             CriticReviewId = new criticReviewId
             int criticReviewCount = 0;
             criticReviewCount = 
             return View("Index", game);
         }*/

        /*
        public ActionResult Index()
        {
            ViewBag.Counter = db.CriticReviews.Count();
            //IEnumerable<Game> gameCount = cr.GetGames();
            var gameCount = cr.GetGames();
            //var gameCount = db.Games.Where(y => GameId.Contains(y.GameId));
            var gamesModel = new List<Game>();
            foreach (var game in gameCount)
            {
                var criticReviewCount = db.CriticReviews.Where(x => x.GameId == game.GameId).Count();
                gamesModel.Add(new Game
                {
                    Title = game.Title,
                    CriticReviewCount = criticReviewCount
                });
            }
            return View(gamesModel);
        }
        */

        // GET: Games
        public ActionResult Index(string sortDir, string searchString, string currentFilter,
            int? page, string sortOrder = "")
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.currentFilter = searchString;
            ViewBag.sortDir = sortDir;
            ViewBag.sortOrder = sortOrder;

            var games = db.Games.Include(g => g.GameInfo).AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                games = games.Where(std => std.Title.Contains(searchString));
            }

            switch (sortOrder.ToLower())
            {
                case "title":
                    if (sortDir.ToLower() == "desc")
                        games = games.OrderByDescending(s => s.Title);
                    else
                        games = games.OrderBy(s => s.Title);
                    break;
                case "criticScoreAvg":
                    if (sortDir.ToLower() == "desc")
                        games = games.OrderByDescending(s => s.CriticScoreAvg);
                    else
                        games = games.OrderBy(s => s.CriticScoreAvg);
                    break;
                case "userScoreAvg":
                    if (sortDir.ToLower() == "desc")
                        games = games.OrderByDescending(s => s.UserScoreAvg);
                    else
                        games = games.OrderBy(s => s.UserScoreAvg);
                    break;

                default:
                    games = games.OrderBy(s => s.Title);
                    break;
            }

            int pageSize = 6;
            int pageNumber = page ?? 1;

            var data = games.ToPagedList(pageNumber, pageSize);

            //add a condition to check if you have an ajax call
            if (Request.IsAjaxRequest())
            {
                return PartialView("_GameList", data);
            }

            return View(data);
        }

        // GET: Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Include(p => p.GameInfo).Where(p => p.GameId == id).FirstOrDefault();
            if (game == null)
            {
                return HttpNotFound();
            }
            Mapper.CreateMap<Game, GameViewModel>().ForMember(x => x.Photo, opt => opt.Ignore());

            var gameVM = Mapper.Map<GameViewModel>(game);
            gameVM.PhotoDB = game.Photo;
            return View(gameVM);
        }

        // GET: Games/Create
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Create()
        {
            ViewBag.GameInfoId = new SelectList(db.GameInfoes, "GameInfoId", "Rating", "Genre");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Create(GameViewModel gameVM)
        {
            if (ModelState.IsValid)
            {
                Game game = new Game();
                game.Title = gameVM.Title;
                game.Summary = gameVM.Summary;
                if (gameVM.Photo != null)
                    game.Photo = ImageConverter.ByteArrayFromPostedFile(gameVM.Photo);
                GameInfo gameinfo = new GameInfo();
                gameinfo.Genre = gameVM.GameInfo.Genre;
                gameinfo.Rating = gameVM.GameInfo.Rating;
                game.GameInfo = gameinfo;
                db.Games.Add(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GameInfoId = new SelectList(db.GameInfoes, "GameInfoId", "Rating", "Genre", gameVM.GameInfoId);
            return View(gameVM);
        }

        // GET: Games/Edit/5
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Include(p => p.GameInfo).Where(p => p.GameId == id).FirstOrDefault();
            if (game == null)
            {
                return HttpNotFound();
            }
            Mapper.CreateMap<Game, GameViewModel>().ForMember(x => x.Photo, opt => opt.Ignore());

            var gameVM = Mapper.Map<GameViewModel>(game);
            gameVM.PhotoDB = game.Photo;
            ViewBag.GameInfoId = new SelectList(db.GameInfoes, "GameInfoId", "Rating", "Genre", game.GameInfoId);

            return View(gameVM);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Edit(GameViewModel gameVM)
        {
            if (ModelState.IsValid)
            {
                Game game = db.Games.Find(gameVM.GameId);
                game.Title = gameVM.Title;
                game.Summary = gameVM.Summary;
                if (gameVM != null && gameVM.Photo != null)
                    game.Photo = ImageConverter.ByteArrayFromPostedFile(gameVM.Photo);
                game.GameInfo = gameVM.GameInfo;
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GameInfoId = new SelectList(db.GameInfoes, "GameInfoId", "Rating", "Genre", gameVM.GameInfoId);

            return View(gameVM);
        }

        // GET: Games/Delete/5
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Include(p => p.GameInfo).Where(p => p.GameId == id).FirstOrDefault();
            if (game == null)
            {
                return HttpNotFound();
            }
            Mapper.CreateMap<Game, GameViewModel>().ForMember(x => x.Photo, opt => opt.Ignore());

            var gameVM = Mapper.Map<GameViewModel>(game);
            gameVM.PhotoDB = game.Photo;
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}