using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KeatonBlog.Models;
using Microsoft.AspNet.Identity;

namespace KeatonBlog.Controllers
{
    [RequireHttps]
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Author).Include(c => c.BlogPost);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.BlogPostId = new SelectList(db.Posts, "Id", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BlogPostId,AuthorId,Body")] Comment comment, int blogPostID)
        {
            if (ModelState.IsValid)
            {
                comment.BlogPostId = blogPostID;
                comment.Created = DateTimeOffset.Now;
                comment.AuthorId = User.Identity.GetUserId();
                db.Comments.Add(comment);
                db.SaveChanges();
                var slug = db.Posts.FirstOrDefault(p => p.Id == blogPostID).Slug;
                return RedirectToAction("Details", "BlogPosts", new { Slug = slug });
            }

            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comment.AuthorId);
            ViewBag.BlogPostId = new SelectList(db.Posts, "Id", "Title", comment.BlogPostId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id, string slug)
        {
            ViewBag.ReturnSlug = slug;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comment.AuthorId);
            ViewBag.BlogPostId = new SelectList(db.Posts, "Id", "Title", comment.BlogPostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Body,BlogPostId,AuthorID,UpdateReason")] Comment comment, string slug)
        {
            if (ModelState.IsValid)
            {
             
                ViewBag.ReturnSlug = slug;
                comment.Updated = DateTimeOffset.Now;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                            }
            return RedirectToAction("Details", "BlogPosts", new { Slug = slug });
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id, string slug)
        {
            ViewBag.ReturnSlug = slug;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string slug)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", "BlogPosts", new { Slug = slug });
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
