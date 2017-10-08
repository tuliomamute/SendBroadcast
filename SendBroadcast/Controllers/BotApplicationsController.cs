using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SendBroadcast.Models;
using System.Text;

namespace SendBroadcast.Controllers
{
    public class BotApplicationsController : Controller
    {
        private SendBroadcastContext db = new SendBroadcastContext();

        private string GenerateAuthorizationToken(string botname, string accesstoken)

        {
            string OriginalKey = Encoding.UTF8.GetString(Convert.FromBase64String(accesstoken));

            string UserAndKey = $"{botname}:{OriginalKey}";

            return $"Key {Convert.ToBase64String(Encoding.UTF8.GetBytes(UserAndKey))}";

        }

        // GET: BotApplications
        public ActionResult Index()
        {
            return View(db.BotApplications.ToList());
        }

        // GET: BotApplications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            BotApplication botApplication = db.BotApplications.Find(id);
            if (botApplication == null)
                return HttpNotFound();

            return View(botApplication);
        }

        // GET: BotApplications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BotApplications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BotId,BotName,BotIdentifier,BotAccessToken")] BotApplication botApplication)
        {
            botApplication.BotAuthorizationTokenApi = GenerateAuthorizationToken(botApplication.BotIdentifier, botApplication.BotAccessToken);

            if (ModelState.IsValid)
            {
                db.BotApplications.Add(botApplication);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(botApplication);
        }

        // GET: BotApplications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            BotApplication botApplication = db.BotApplications.Find(id);
            if (botApplication == null)
                return HttpNotFound();

            return View(botApplication);
        }

        // POST: BotApplications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BotId,BotName,BotIdentifier,BotAccessToken")] BotApplication botApplication)
        {
            botApplication.BotAuthorizationTokenApi = GenerateAuthorizationToken(botApplication.BotIdentifier, botApplication.BotAccessToken);

            if (ModelState.IsValid)
            {
                db.Entry(botApplication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(botApplication);
        }

        // GET: BotApplications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            BotApplication botApplication = db.BotApplications.Find(id);
            if (botApplication == null)
                return HttpNotFound();

            return View(botApplication);
        }

        // POST: BotApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BotApplication botApplication = db.BotApplications.Find(id);
            db.BotApplications.Remove(botApplication);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
