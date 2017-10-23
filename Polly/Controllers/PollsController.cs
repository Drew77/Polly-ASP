using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Polly.Models;
using Microsoft.AspNet.Identity;
using System.Web.Script.Serialization;

namespace Polly.Controllers
{
    [RequireHttps]
    public class PollsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Polls
        public ActionResult Index()
        {


            return View(db.Polls.ToList());
        }

        // GET: Polls/Details/5
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Poll poll = db.Polls.Where(p => p.Id == id).Include("PollAnswers")
                          .FirstOrDefault();
            if (poll == null)
            {
                return HttpNotFound();
            }

            var userID = Request.AnonymousID;
            var tags = db.Tags.Where(t => t.PollId == id && t.User == userID).FirstOrDefault();
            if (tags != null)
            {
                ViewBag.HaveVoted = true;
            }
            ViewBag.Owner = User.Identity.GetUserName() == poll.Creator ? true : false;


                return View(poll);
        }

        // POST: Polls/Details/5
        [ValidateAntiForgeryToken]
        public ActionResult Vote(int id, int Answer)
        {
            var userID = Request.AnonymousID;
            var tags = db.Tags.Where(t => t.PollId == id && t.User == userID).FirstOrDefault();
            if (tags == null)
            {
                Tag tag = new Tag();
                tag.User = userID;
                tag.PollId = id;
                Answer vote = db.Answers.Find(Answer);
                vote.Votes++;
                db.Tags.Add(tag);
                db.SaveChanges();
            }
            ViewBag.HaveVoted = true;

           
   
            var Options = db.Answers.Where(p => p.PollId == id).ToList();
            return PartialView("_AnswerPartial", Options);

        }
        [ValidateAntiForgeryToken]
        public ActionResult AddAnswer(int id, string Answer)
        {
            var userID = Request.AnonymousID;
            var tags = db.Tags.Where(t => t.PollId == id && t.User == userID).FirstOrDefault();
            if (tags == null)
            {
                Tag tag = new Tag();
                tag.User = userID;
                tag.PollId = id;
                var newAnswer = new Answer();
                newAnswer.Option = Answer;
                newAnswer.PollId = id;
                newAnswer.Votes = 1;
                db.Answers.Add(newAnswer);
                db.Tags.Add(tag);
                db.SaveChanges();
            }
            ViewBag.HaveVoted = true;



            var Options = db.Answers.Where(p => p.PollId == id).ToList();
            return PartialView("_AnswerPartial", Options);
        }


        public ActionResult Data(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Poll poll = db.Polls.Where(p => p.Id == id).Include("PollAnswers")
                          .FirstOrDefault();
            if (poll == null)
            {
                return HttpNotFound();
            }

            var data = poll.PollAnswers.ToList();

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(data);
            return Json(json, JsonRequestBehavior.AllowGet);
        }



        // GET: Polls/Create
        [Authorize]
        public ActionResult Create()
        {

            var ViewModel = new CreateVM();
            ViewModel.Answers = new string[10];

  

            return View(ViewModel);
        }

        // POST: Polls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(CreateVM ViewModel)
        {
            if (ModelState.IsValid)
            {
                var poll = new Poll();
                poll.Create(ViewModel);
                poll.Creator = User.Identity.GetUserName();
                db.Polls.Add(poll);
                db.SaveChanges();
        
                return RedirectToAction("Details", new { id = poll.Id });
            }

            return View(ViewModel);
        }




        // GET: Polls/Delete/5
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                return HttpNotFound();
            }
            return View(poll);
        }

        // POST: Polls/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Poll poll = db.Polls.Find(id);
            db.Polls.Remove(poll);
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
