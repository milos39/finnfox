using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
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



        
        public ActionResult balansKorisnika()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var prihodi = db.RacunovodstvenaPromenas.Where(m => m.ApplicationUserId == userId && m.TipRacunovodstvenePromene.PozitivnostTipa == true).Select(m=>m.KolicinaNovca).DefaultIfEmpty(0).Sum();
                var rashodi = db.RacunovodstvenaPromenas.Where(m => m.ApplicationUserId == userId && m.TipRacunovodstvenePromene.PozitivnostTipa == false).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();

                var balans = prihodi - rashodi;
                return Json(balans, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;

            }




        }


        private void procenatUstede(  double Usteda, ref PieChartViewModel viewModel, double ukupniPrihodi )
        {

            if (ukupniPrihodi == 0)
                 return;
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
        }





        [HttpGet]
        public ActionResult meseciZaGodinu(int godina)
        {
            var userId = User.Identity.GetUserId();
            var meseci = db.RacunovodstvenaPromenas.Where(m => m.DatumPromene.Year == godina && m.ApplicationUserId == userId).Select(m => m.DatumPromene.Month).Distinct().OrderBy(m=>m).ToList();
            return Json(meseci, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult godineSviKorisnici ()
        {
            return Json(db.RacunovodstvenaPromenas.Select(m => m.DatumPromene.Year).Distinct().OrderBy(m => m).ToList(), JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult globalniProcentiUstede()
        {

            List<int> godine = db.RacunovodstvenaPromenas.Select(m => m.DatumPromene.Year).Distinct().ToList();
            List<int> meseci = db.RacunovodstvenaPromenas.Select(m => m.DatumPromene.Month).Distinct().OrderBy(m=>m).ToList();
            List<List<double>> listaVrednosti = new List<List<double>>();
            List<double> vrednosti;

            RacunovodstvenePromeneTipMesecViewModel viewModel = new RacunovodstvenePromeneTipMesecViewModel();
            foreach (var godina in godine)
            {
                var prihodiPoMesecima = db.RacunovodstvenaPromenas.Where(m => m.TipRacunovodstvenePromene.PozitivnostTipa == true && m.DatumPromene.Year == godina).GroupBy(x => x.DatumPromene.Month, (key, value) => new { mesec = key, vrenost = value.Sum(m => m.KolicinaNovca) });
                var rashodiPoMesecima = db.RacunovodstvenaPromenas.Where(m => m.TipRacunovodstvenePromene.PozitivnostTipa == false && m.DatumPromene.Year == godina).GroupBy(x => x.DatumPromene.Month, (key, value) => new { mesec = key, vrenost = value.Sum(m => m.KolicinaNovca) });
                vrednosti = new List<double>();

                for (int i = 0; i < meseci.Count; i++)
                {
                    var prihodZaMesec = 0.0;
                    var rashodZaMesec = 0.0;
                  
                    var trenutniMesec = meseci[i];
                
                    prihodZaMesec = prihodiPoMesecima.Where(m => m.mesec == trenutniMesec).Select(m => m.vrenost).DefaultIfEmpty(0).SingleOrDefault();
                    rashodZaMesec = rashodiPoMesecima.Where(m => m.mesec == trenutniMesec).Select(m => m.vrenost).DefaultIfEmpty(0).SingleOrDefault();
                   
                    var usteda = prihodZaMesec - rashodZaMesec;
                    var procenatUstede = -100.00;

                    if (prihodZaMesec != 0)
                      procenatUstede = (usteda / prihodZaMesec) * 100;

                      vrednosti.Add(Math.Round(procenatUstede, 2));
                }
                listaVrednosti.Add(vrednosti);
            }

            viewModel.kategorije = godine.Select(m => m.ToString() ).ToList();
            viewModel.meseci = meseci;
            viewModel.vrednostiPoKategoriji = listaVrednosti;

            return Json(viewModel, JsonRequestBehavior.AllowGet);
            
        }



        public ActionResult globalnePromeneMesecTip(int godina)
        {

           
            List<int> nizVrednosti = new List<int>();
            var sviRashodi = db.RacunovodstvenaPromenas.Where(m => m.TipRacunovodstvenePromene.PozitivnostTipa == false && m.DatumPromene.Year == godina).Select(m => m.TipRacunovodstvenePromene).Distinct().ToList();
            List<int> meseci = new List<int>();
            List<List<double>> vrednostiPoKategoriji = new List<List<double>>();
            List<double> vrednosti = new List<double>();
            List<string> kategorije = new List<string>();

            RacunovodstvenePromeneTipMesecViewModel viewModel = new RacunovodstvenePromeneTipMesecViewModel();
            var querryResult = db.RacunovodstvenaPromenas.Where(m => m.TipRacunovodstvenePromene.PozitivnostTipa == false && m.DatumPromene.Year == godina).GroupBy(x => new {  x.DatumPromene.Month , x.TipRacunovodstvenePromene.NazivTipa }, (key, values) => new { mesec = key, vrednost = values.Sum(m => m.KolicinaNovca )}).ToList();
                
            foreach (var objekat in querryResult)
            {
                if (!meseci.Contains(objekat.mesec.Month))
                    meseci.Add(objekat.mesec.Month);

              
            }


            foreach (var rashod in  sviRashodi)
            {

                vrednosti = new List<double>();

             //   bool flag = false;
               
                foreach (var objekat in querryResult)
                {

                    if(rashod.NazivTipa == objekat.mesec.NazivTipa)
                    {
                        for (int i = 0; i < meseci.Count; i++)
                        {

                             if( meseci[i] == objekat.mesec.Month )
                            {
                                if (vrednosti.Count < meseci.Count)
                                    vrednosti.Add(objekat.vrednost);
                                else
                                    vrednosti[i] = objekat.vrednost;


                            }
                             else
                            {
                                if (vrednosti.Count < meseci.Count )
                                    vrednosti.Add(0);
                            }

                        }
                    }
                      
                }

                vrednostiPoKategoriji.Add(vrednosti);                
                kategorije.Add(rashod.NazivTipa);

            }


            viewModel.kategorije = kategorije;
            viewModel.meseci = meseci;
            viewModel.vrednostiPoKategoriji = vrednostiPoKategoriji;

            return Json(viewModel, JsonRequestBehavior.AllowGet);

        }


        //GET: RacunovodstvenaPromenas/promenePoMesecu?godina=val&trenutniMesec=val
        public ActionResult promenePoMesecu(int godina,int mesec)
        {
            ListRacunovodstvenaPromenaMesecViewModel viewModel = new ListRacunovodstvenaPromenaMesecViewModel();
            var userId = User.Identity.GetUserId();

            if (godina > 0)
            {
                var models = db.RacunovodstvenaPromenas.Where(m => m.DatumPromene.Year == godina && m.DatumPromene.Month == mesec && m.ApplicationUserId == userId).ToList();

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
                var pozitivno = db.RacunovodstvenaPromenas.Where(m => m.TipRacunovodstvenePromene.PozitivnostTipa == true && m.DatumPromene.Year == godina && m.DatumPromene.Month == mesec && m.ApplicationUserId == userId).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();
                var negativno = db.RacunovodstvenaPromenas.Where(m => m.TipRacunovodstvenePromene.PozitivnostTipa == false && m.DatumPromene.Year == godina && m.DatumPromene.Month == mesec && m.ApplicationUserId == userId).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();




                viewModel.balans = pozitivno - negativno;
                viewModel.godine = db.RacunovodstvenaPromenas.Where(m => m.ApplicationUserId == userId).Select(m => m.DatumPromene.Year).Distinct().OrderBy(m=>m).ToList();
                viewModel.meseciZaDatuGodinu = db.RacunovodstvenaPromenas.Where(m => m.ApplicationUserId == userId && m.DatumPromene.Year == godina).Select(m => m.DatumPromene.Month).Distinct().OrderBy(m=>m).ToList();
            }
           


            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        //GET: RacunovodstvenaPromenas/godinaMesecChart?godina=val&trenutniMesec=val
        [HttpGet]
        public ActionResult godinaMesecChart (int godina,int mesec)
        {

            var userId = User.Identity.GetUserId();
            PieChartViewModel viewModel = new PieChartViewModel();

            var ukupniPrihodi = db.RacunovodstvenaPromenas.Where(m => m.ApplicationUserId == userId && m.DatumPromene.Year == godina && m.DatumPromene.Month == mesec && m.TipRacunovodstvenePromene.PozitivnostTipa == true).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();
            var ukupniRashodi = db.RacunovodstvenaPromenas.Where(m => m.ApplicationUserId == userId && m.DatumPromene.Year == godina && m.DatumPromene.Month == mesec && m.TipRacunovodstvenePromene.PozitivnostTipa == false).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();

            var Usteda = ukupniPrihodi - ukupniRashodi;

            var kategorije = db.TipRacunovodstvenePromenes.Where(m => m.PozitivnostTipa == false).ToList();


            procenatUstede(Usteda,ref viewModel, ukupniPrihodi);

            

            foreach (var kategorija in kategorije)
            {
                var vrednostRacunaKategorija = db.RacunovodstvenaPromenas.Where(m => m.TipPromeneId == kategorija.TipPromeneId && m.DatumPromene.Year == godina && m.DatumPromene.Month == mesec && m.ApplicationUserId == userId).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();
                double procenat = 0;
                string procenatString = "";
                if (ukupniPrihodi != 0)
                {
                    procenat = (vrednostRacunaKategorija / ukupniPrihodi) * 100;
                    procenatString =  " " +Math.Round(procenat, 2) + "%";

                }

                if (vrednostRacunaKategorija != 0)
                {
                    viewModel.nasloviSaProcentima.Add(kategorija.NazivTipa + procenatString);
                    viewModel.kolicineNovcaPoTipu.Add(vrednostRacunaKategorija );

                }

            }


            return Json(viewModel, JsonRequestBehavior.AllowGet);
      

        }




        //GET: RacunovodstvenaPromenas/godinaChart/godina
        [HttpGet]
        public ActionResult godinaChart (int godina)
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

                procenatUstede(Usteda, ref viewModel, ukupniPrihodi);

                foreach (var kategorija in kategorije)
                {


                    vrednostRacunaKategorija = db.RacunovodstvenaPromenas.Where(m => m.TipPromeneId == kategorija.TipPromeneId && m.DatumPromene.Year == godina && m.ApplicationUserId == userId).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();


                    double procenat = 0;
                    string procenatString = "";
                    if (ukupniPrihodi != 0)
                    {
                        procenat = (vrednostRacunaKategorija / ukupniPrihodi) * 100;
                        procenatString = " - " + Math.Round(procenat, 2) + "%";
                    }

                    if (vrednostRacunaKategorija != 0)
                    {
                        viewModel.nasloviSaProcentima.Add(kategorija.NazivTipa + procenatString);
                        viewModel.kolicineNovcaPoTipu.Add(vrednostRacunaKategorija);
                    }



                }
                return Json(viewModel, JsonRequestBehavior.AllowGet);

            }
            else
            {

                



                var ukupniPrihodi = db.RacunovodstvenaPromenas.Where(m => m.ApplicationUserId == userId && m.TipRacunovodstvenePromene.PozitivnostTipa == true).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();
                var ukupniRashodi = db.RacunovodstvenaPromenas.Where(m => m.ApplicationUserId == userId && m.TipRacunovodstvenePromene.PozitivnostTipa == false).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();

                var Usteda = ukupniPrihodi - ukupniRashodi;

                var kategorije = db.TipRacunovodstvenePromenes.Where(m => m.PozitivnostTipa == false).ToList();


                double vrednostRacunaKategorija = 0;



                procenatUstede(Usteda,ref viewModel, ukupniPrihodi);


                foreach (var kategorija in kategorije)
                {

                   
                    vrednostRacunaKategorija = db.RacunovodstvenaPromenas.Where(m => m.TipPromeneId == kategorija.TipPromeneId && m.ApplicationUserId == userId).Select(m => m.KolicinaNovca).DefaultIfEmpty(0).Sum();

                    if (vrednostRacunaKategorija != 0)
                    {

                        double procenat = 0;
                        string procenatString = "";
                        if (ukupniPrihodi != 0)
                        {
                            procenat = (vrednostRacunaKategorija / ukupniPrihodi) * 100;
                            procenatString = " - " + Math.Round(procenat, 2) + "%";
                        }


                        viewModel.nasloviSaProcentima.Add(kategorija.NazivTipa + procenatString);
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
                viewModel.godine = db.RacunovodstvenaPromenas.Where(m => m.ApplicationUserId == userId).Select( m => m.DatumPromene.Year).Distinct().OrderBy(m=>m).ToList();
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

                  


                viewModel.godine = db.RacunovodstvenaPromenas.Where(m=>m.ApplicationUserId == userId).Select(m => m.DatumPromene.Year).Distinct().OrderBy(m => m).ToList();

            }
           

            return  Json(viewModel ,  JsonRequestBehavior.AllowGet );
        }
       // GET: RacunovodstvenaPromenas
        public ActionResult Index()
        {
            
          
            return View();
        }

        //GET: RacunovodstvenaPromenas/globalnaAnalitikas
        [HttpGet]
        public ActionResult globalnaAnalitika()
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
            ViewBag.TipPromeneId = new SelectList(db.TipRacunovodstvenePromenes, "TipPromeneId", "NazivTipa");
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
            try
            {
                RacunovodstvenaPromena racunovodstvenaPromena = db.RacunovodstvenaPromenas.Find(id);
                db.RacunovodstvenaPromenas.Remove(racunovodstvenaPromena);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
           
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
