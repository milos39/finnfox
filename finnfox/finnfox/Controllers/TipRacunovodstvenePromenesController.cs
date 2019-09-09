using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using finnfox;
using finnfox.Models;

namespace finnfox.Controllers
{
    public class TipRacunovodstvenePromenesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TipRacunovodstvenePromenes
        public ActionResult Index()
        {
            return View(db.TipRacunovodstvenePromenes.ToList());
        }

        // GET: TipRacunovodstvenePromenes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipRacunovodstvenePromene tipRacunovodstvenePromene = db.TipRacunovodstvenePromenes.Find(id);
            if (tipRacunovodstvenePromene == null)
            {
                return HttpNotFound();
            }
            return View(tipRacunovodstvenePromene);
        }

        // GET: TipRacunovodstvenePromenes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipRacunovodstvenePromenes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TipPromeneId,NazivTipa,PozitivnostTipa")] TipRacunovodstvenePromene tipRacunovodstvenePromene)
        {
            if (ModelState.IsValid)
            {
                db.TipRacunovodstvenePromenes.Add(tipRacunovodstvenePromene);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipRacunovodstvenePromene);
        }

        // GET: TipRacunovodstvenePromenes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipRacunovodstvenePromene tipRacunovodstvenePromene = db.TipRacunovodstvenePromenes.Find(id);
            if (tipRacunovodstvenePromene == null)
            {
                return HttpNotFound();
            }
            return View(tipRacunovodstvenePromene);
        }

        // POST: TipRacunovodstvenePromenes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TipPromeneId,NazivTipa,PozitivnostTipa")] TipRacunovodstvenePromene tipRacunovodstvenePromene)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipRacunovodstvenePromene).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipRacunovodstvenePromene);
        }

        // GET: TipRacunovodstvenePromenes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipRacunovodstvenePromene tipRacunovodstvenePromene = db.TipRacunovodstvenePromenes.Find(id);
            if (tipRacunovodstvenePromene == null)
            {
                return HttpNotFound();
            }
            return View(tipRacunovodstvenePromene);
        }

        // POST: TipRacunovodstvenePromenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipRacunovodstvenePromene tipRacunovodstvenePromene = db.TipRacunovodstvenePromenes.Find(id);
            db.TipRacunovodstvenePromenes.Remove(tipRacunovodstvenePromene);
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
