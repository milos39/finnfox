using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using finnfox;
using finnfox.Extensions;
using finnfox.Models;
using Newtonsoft.Json;

namespace finnfox.Controllers
{
    public class RacunovodstvenaPromenasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        //GET: RacunovodstvenaPromenas/pieChart/godina
        [HttpGet]
        public async Task<ActionResult> pieChart (int godina)
        {
            var userId = User.Identity.GetUserId();

            PieChartViewModel viewModel = new PieChartViewModel();

            if( godina > 0)
            {
                var ukupniPrihodi = db.RacunovodstvenaPromenas.Where(m => m.ApplicationUserId == userId && m.DatumPromene.Year == godina && m.TipRacunovodstvenePromene.PozitivnostTipa == true).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();
                var ukupniRashodi  = db.RacunovodstvenaPromenas.Where(m => m.ApplicationUserId == userId && m.DatumPromene.Year == godina && m.TipRacunovodstvenePromene.PozitivnostTipa == false).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();

                var Usteda = ukupniPrihodi - ukupniRashodi;


                var kategorije = db.TipRacunovodstvenePromenes.Where(m=>m.PozitivnostTipa == false).ToList();
                double vrednostRacunaKategorija = 0;

                if(ukupniPrihodi == 0)
                {
                    foreach (var kategorija in kategorije)
                    {

                        
                            vrednostRacunaKategorija = db.RacunovodstvenaPromenas.Where(m => m.TipPromeneId == kategorija.TipPromeneId && m.DatumPromene.Year == godina && m.ApplicationUserId == userId).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();

                        if (vrednostRacunaKategorija != 0)
                        {
                            viewModel.nasloviSaProcentima.Add(kategorija.NazivTipa);
                            viewModel.kolicineNovcaPoTipu.Add(vrednostRacunaKategorija);
                        }

                        return Json(viewModel, JsonRequestBehavior.AllowGet);


                    }
                }

                
                if(Usteda > 0)
                {
                    double procenat = (Usteda / ukupniPrihodi) * 100;
                    viewModel.nasloviSaProcentima.Add("ušteda - " + Math.Round(procenat, 2) + "%");
                    viewModel.kolicineNovcaPoTipu.Add(Usteda);
                }
                else
                {
                    double procenat = ( (Usteda * -1) / ukupniPrihodi) * 100;
                    viewModel.nasloviSaProcentima.Add("dugovanje - " + Math.Round(procenat, 2) + "%");
                    viewModel.kolicineNovcaPoTipu.Add(Usteda);

                }


                foreach (var kategorija in kategorije)
                {
                    
                    vrednostRacunaKategorija = db.RacunovodstvenaPromenas.Where(m => m.TipPromeneId == kategorija.TipPromeneId&& m.DatumPromene.Year == godina && m.ApplicationUserId == userId ).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();


                    if(vrednostRacunaKategorija != 0)
                    {
                        double procenat = (vrednostRacunaKategorija / ukupniPrihodi) * 100;


                        viewModel.nasloviSaProcentima.Add(kategorija.NazivTipa + " - " + Math.Round(procenat, 2) + "%");
                        viewModel.kolicineNovcaPoTipu.Add(vrednostRacunaKategorija);
                    }
                   


                }

            }
            else
            {

                



                var ukupniPrihodi = db.RacunovodstvenaPromenas.Where(m => m.ApplicationUserId == userId && m.TipRacunovodstvenePromene.PozitivnostTipa == true).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();
                var ukupniRashodi = db.RacunovodstvenaPromenas.Where(m => m.ApplicationUserId == userId && m.TipRacunovodstvenePromene.PozitivnostTipa == false).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();

                var Usteda = ukupniPrihodi - ukupniRashodi;

                var kategorije = db.TipRacunovodstvenePromenes.Where(m => m.PozitivnostTipa == false).ToList();


                double vrednostRacunaKategorija = 0;



                if (Usteda > 0)
                {
                    double procenat = (Usteda / ukupniPrihodi) * 100;
                    viewModel.nasloviSaProcentima.Add("ušteda - " + Math.Round(procenat, 2) + "%");
                    viewModel.kolicineNovcaPoTipu.Add(Usteda);
                }
                else
                {
                    double procenat = ((Usteda * -1) / ukupniPrihodi) * 100;
                    viewModel.nasloviSaProcentima.Add("dugovanje - " + Math.Round(procenat, 2) + "%");
                    viewModel.kolicineNovcaPoTipu.Add(Usteda);

                }

                foreach (var kategorija in kategorije)
                {

                   
                    vrednostRacunaKategorija = db.RacunovodstvenaPromenas.Where(m => m.TipPromeneId == kategorija.TipPromeneId && m.ApplicationUserId == userId).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();

                    if (vrednostRacunaKategorija != 0)
                    {
                        double procenat = (vrednostRacunaKategorija / ukupniPrihodi) * 100;


                        viewModel.nasloviSaProcentima.Add(kategorija.NazivTipa + " - " + Math.Round(procenat, 2) + "%");
                        viewModel.kolicineNovcaPoTipu.Add(vrednostRacunaKategorija);
                    }


                }


            }




            return  Json(viewModel, JsonRequestBehavior.AllowGet);

        }


        //GET: RacunovodstvenaPromenas/promenePoGodini/godina
        [HttpGet]
        public ActionResult promenePoGodini( int godina)
        {
            ListRacunovodstvenaPromenaViewModel viewModel = new ListRacunovodstvenaPromenaViewModel();
            var userId = User.Identity.GetUserId();

            if (godina > 0)
            {
                var models = db.RacunovodstvenaPromenas.Where(m => m.DatumPromene.Year == godina && m.ApplicationUserId == userId  ).ToList();

                foreach (var model in models)
                {
                    viewModel.racunovodstvenePromene.Add(new RacunovodstvenaPromenaDTO()
                    {
                        Id = model.PromenaId,
                        DatumPromene = model.DatumPromene.Date.ToString("dd/MM/yy"),
                        KolicinaNovca = model.KolicinaNovca,
                        NazivPromene = model.NazivPromene,
                        TipRacunovodstvenePromene = model.TipRacunovodstvenePromene.NazivTipa,
                        TipPromeneId = model.TipPromeneId,
                        Valuta = model.Valuta
                    });
                }
                var pozitivno = db.RacunovodstvenaPromenas.Where(m => m.TipRacunovodstvenePromene.PozitivnostTipa == true && m.DatumPromene.Year == godina && m.ApplicationUserId == userId).Select(m=>m.KolicinaNovca).DefaultIfEmpty(0).Sum();
                var negativno = db.RacunovodstvenaPromenas.Where(m => m.TipRacunovodstvenePromene.PozitivnostTipa == false && m.DatumPromene.Year == godina && m.ApplicationUserId == userId).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();




                viewModel.balans = pozitivno - negativno;
                viewModel.godine = db.RacunovodstvenaPromenas.Select(m => m.DatumPromene.Year).Distinct().ToList();
            }
            else
            {
                var models =  db.RacunovodstvenaPromenas.Where(m=>m.ApplicationUserId == userId).ToList();

                  foreach (var model in models)
                {
                    viewModel.racunovodstvenePromene.Add(new RacunovodstvenaPromenaDTO()
                    {
                        Id = model.PromenaId,
                        DatumPromene = model.DatumPromene.Date.ToString("dd/MM/yy"),
                        KolicinaNovca = model.KolicinaNovca,
                        NazivPromene = model.NazivPromene,
                        TipRacunovodstvenePromene = model.TipRacunovodstvenePromene.NazivTipa,
                        TipPromeneId = model.TipPromeneId,
                        Valuta = model.Valuta
                    });
                }


                viewModel.godine = db.RacunovodstvenaPromenas.Select(m => m.DatumPromene.Year).Distinct().ToList();

            }
           

            return  Json(viewModel ,  JsonRequestBehavior.AllowGet );
        }
       // GET: RacunovodstvenaPromenas
        public ActionResult Index()
        {
            
          
            return View();
        }

        // GET: RacunovodstvenaPromenas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RacunovodstvenaPromena racunovodstvenaPromena = db.RacunovodstvenaPromenas.Find(id);
            if (racunovodstvenaPromena == null)
            {
                return HttpNotFound();
            }
            return View(racunovodstvenaPromena);
        }

        // GET: RacunovodstvenaPromenas/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.TipPromeneId = new SelectList( db.RacunovodstvenaPromenas.Where(m => m.ApplicationUserId == userId).Select(m => m.TipRacunovodstvenePromene).Distinct().ToList()
, "TipPromeneId", "NazivTipa");
            return View();
        }

        // POST: RacunovodstvenaPromenas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NazivPromene,DatumPromene,TipPromeneId,ApplicationUserId,KolicinaNovca")] RacunovodstvenaPromena racunovodstvenaPromena)
        {

            racunovodstvenaPromena.PromenaId = null;
            racunovodstvenaPromena.ApplicationUserId = User.Identity.GetUserId();
            ModelState.Remove("ApplicationUserId");
            ModelState.Remove("Valuta");
            ModelState.Remove("PromenaId");

            try
            {

                if (ModelState.IsValid)
                {
                    db.RacunovodstvenaPromenas.Add(racunovodstvenaPromena);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (Exception e)
            {
                ViewBag.TipPromeneId = new SelectList(db.TipRacunovodstvenePromenes, "TipPromeneId", "NazivTipa", racunovodstvenaPromena.TipPromeneId);
                //return View(racunovodstvenaPromena);
                //mozemo da ubacimo error stranicu ili nesto tako
                throw;
            }

            ViewBag.TipPromeneId = new SelectList(db.TipRacunovodstvenePromenes, "TipPromeneId", "NazivTipa", racunovodstvenaPromena.TipPromeneId);
            return View(racunovodstvenaPromena);


        }

        // GET: RacunovodstvenaPromenas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RacunovodstvenaPromena racunovodstvenaPromena = db.RacunovodstvenaPromenas.Find(id);
            if (racunovodstvenaPromena == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipPromeneId = new SelectList(db.TipRacunovodstvenePromenes, "TipPromeneId", "NazivTipa", racunovodstvenaPromena.TipPromeneId);
            return View(racunovodstvenaPromena);
        }

        // POST: RacunovodstvenaPromenas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PromenaId,NazivPromene,DatumPromene,TipPromeneId,ApplicationUserId,KolicinaNovca")] RacunovodstvenaPromena racunovodstvenaPromena)
        {
            racunovodstvenaPromena.ApplicationUserId = User.Identity.GetUserId();
            ModelState.Remove("ApplicationUserId");
            if (ModelState.IsValid)
            {
                db.Entry(racunovodstvenaPromena).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipPromeneId = new SelectList(db.TipRacunovodstvenePromenes, "TipPromeneId", "NazivTipa", racunovodstvenaPromena.TipPromeneId);
            return View(racunovodstvenaPromena);
        }

        // GET: RacunovodstvenaPromenas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RacunovodstvenaPromena racunovodstvenaPromena = db.RacunovodstvenaPromenas.Find(id);
            if (racunovodstvenaPromena == null)
            {
                return HttpNotFound();
            }
            return View(racunovodstvenaPromena);
        }

        // POST: RacunovodstvenaPromenas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RacunovodstvenaPromena racunovodstvenaPromena = db.RacunovodstvenaPromenas.Find(id);
            db.RacunovodstvenaPromenas.Remove(racunovodstvenaPromena);
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
