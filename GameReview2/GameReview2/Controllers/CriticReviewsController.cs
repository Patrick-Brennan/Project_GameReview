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
    public class CriticReviewsController : Controller
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

            var games = db.CriticReviews.Include(g => g.Game).AsQueryable();
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
                        games = games.OrderByDescending(s => s.CriticFullName);
                    else
                        games = games.OrderBy(s => s.CriticFullName);
                    break;
                case "score":
                    if (sortDir.ToLower() == "desc")
                        games = games.OrderByDescending(s => s.CriticScore);
                    else
                        games = games.OrderBy(s => s.CriticScore);
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
                return PartialView("_CriticReviewList", data);
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
            CriticReview criticReview = db.CriticReviews.Include(p => p.Game).Where(p => p.CriticReviewId == id).FirstOrDefault();
            if (criticReview == null)
            {
                return HttpNotFound();
            }
            return View(criticReview);
        }

        // GET: CriticReviews/Create
        [Authorize(Roles = RoleName.CanDo)]
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
        [Authorize(Roles = RoleName.CanDo)]
        public ActionResult Create(CriticReviewViewModel criticReviewVM)
        {
            if (ModelState.IsValid)
            {
                CriticReview criticReview = new CriticReview();
                criticReview.GameId = criticReviewVM.GameId;
                criticReview.CriticFullName = UserHelper.GetUserName(db.Users, User.Identity);
                criticReview.CriticCreatedOn = DateTime.Now;
                criticReview.CriticUpdatedOn = DateTime.Now;
                criticReview.CriticScore = criticReviewVM.CriticScore;
                criticReview.CriticRev = criticReviewVM.CriticRev;
                db.CriticReviews.Add(criticReview);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Games, "Id", "Title", criticReviewVM.GameId);
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
            CriticReview criticReview = db.CriticReviews.Find(id);
            if (criticReview == null)
            {
                return HttpNotFound();
            }
            return View(criticReview);
        }

        // POST: CriticReviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Edit(CriticReviewViewModel criticReviewVM)
        {

            if (ModelState.IsValid)
            {
                CriticReview criticReview = db.CriticReviews.Find(criticReviewVM.CriticReviewId);
                criticReview.CriticFullName = criticReviewVM.CriticFullName;
                criticReview.CriticUpdatedOn = DateTime.Now;
                criticReview.CriticScore = criticReviewVM.CriticScore;
                criticReview.CriticRev = criticReviewVM.CriticRev;
                db.Entry(criticReview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Games, "Id", "Title", criticReviewVM.GameId);

            return View(criticReviewVM);
        }

        // GET: CriticReviews/Delete/5
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CriticReview criticReview = db.CriticReviews.Find(id);
            if (criticReview == null)
            {
                return HttpNotFound();
            }
            return View(criticReview);
        }

        // POST: CriticReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = RoleName.CanManage)]
        public ActionResult DeleteConfirmed(int id)
        {
            CriticReview criticReview = db.CriticReviews.Find(id);
            db.CriticReviews.Remove(criticReview);
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
