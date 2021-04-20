using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using GameReview2.Helpers;
using GameReview2.Models;
using GameReview2.ViewModels;
using PagedList;

namespace GameReview2.Controllers
{
    public class UserReviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CriticReviews
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

            var games = db.UserReviews.Include(g => g.Game).AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                games = games.Where(std => std.Game.Title.Contains(searchString));
            }

            switch (sortOrder.ToLower())
            {
                case "title":
                    if (sortDir.ToLower() == "desc")
                        games = games.OrderByDescending(s => s.Game.Title);
                    else
                        games = games.OrderBy(s => s.Game.Title);
                    break;
                case "name":
                    if (sortDir.ToLower() == "desc")
                        games = games.OrderByDescending(s => s.UserFullName);
                    else
                        games = games.OrderBy(s => s.UserFullName);
                    break;
                case "score":
                    if (sortDir.ToLower() == "desc")
                        games = games.OrderByDescending(s => s.UserScore);
                    else
                        games = games.OrderBy(s => s.UserScore);
                    break;

                default:
                    games = games.OrderBy(s => s.Game.Title);
                    break;
            }

            int pageSize = 10;
            int pageNumber = page ?? 1;

            var data = games.ToPagedList(pageNumber, pageSize);

            //add a condition to check if you have an ajax call
            if (Request.IsAjaxRequest())
            {
                return PartialView("_UserReviewList", data);
            }

            return View(data);
        }

        // GET: CriticReviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserReview userReview = db.UserReviews.Include(p => p.Game).Where(p => p.UserReviewId == id).FirstOrDefault();
            if (userReview == null)
            {
                return HttpNotFound();
            }
            return View(userReview);
        }

        // GET: CriticReviews/Create
        public ActionResult Create()
        {
            ViewBag.GameId = new SelectList(db.Games, "GameId", "Title");
            return View();
        }

        // POST: CriticReviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserReviewViewModel userReviewVM)
        {
            if (ModelState.IsValid)
            {
                UserReview userReview = new UserReview();
                userReview.GameId = userReviewVM.GameId;
                userReview.UserFullName = UserHelper.GetUserName(db.Users, User.Identity);
                userReview.UserCreatedOn = DateTime.Now;
                userReview.UserUpdatedOn = DateTime.Now;
                userReview.UserScore = userReviewVM.UserScore;
                userReview.UserRev = userReviewVM.UserRev;
                db.UserReviews.Add(userReview);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Games, "Id", "Title", userReviewVM.GameId);
            return View();
        }

        // GET: CriticReviews/Edit/5
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserReview userReview = db.UserReviews.Find(id);
            if (userReview == null)
            {
                return HttpNotFound();
            }
            return View(userReview);
        }

        // POST: CriticReviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Edit(UserReviewViewModel userReviewVM)
        {

            if (ModelState.IsValid)
            {
                UserReview userReview = db.UserReviews.Find(userReviewVM.UserReviewId);
                userReview.UserUpdatedOn = DateTime.Now;
                userReview.UserScore = userReviewVM.UserScore;
                userReview.UserRev = userReviewVM.UserRev;
                db.Entry(userReview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Games, "Id", "Title", userReviewVM.GameId);

            return View(userReviewVM);
        }

        // GET: CriticReviews/Delete/5
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserReview userReview = db.UserReviews.Find(id);
            if (userReview == null)
            {
                return HttpNotFound();
            }
            return View(userReview);
        }

        // POST: CriticReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult DeleteConfirmed(int id)
        {
            UserReview userReview = db.UserReviews.Find(id);
            db.UserReviews.Remove(userReview);
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
