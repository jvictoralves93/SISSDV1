using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SISSDV1.Models;

namespace SISSDV1.Controllers
{
    public class UnidadesController : Controller
    {
        private BancoContexto db = new BancoContexto();

        // GET: Unidades
        public ActionResult Index()
        {
            return View(db.Unidades.ToList().OrderBy(uni => uni.Cidade));
        }
        
        // GET: Unidades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unidade unidade = db.Unidades.Include("Links").Include("LinkTelefonias")
               .Include("Tecnicos").Include("Firewalls")
               .Single(p => p.IDUnidade == id);
            return View(unidade);
        }

        // GET: Unidades/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Unidades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDUnidade,NomeUnidade,Cidade,TelefoneUnidade,SiteCode,Endereco,RazaoSocial,CNPJ")] Unidade unidade)
        {
            if (ModelState.IsValid)
            {
                db.Unidades.Add(unidade);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unidade);
        }

        // GET: Unidades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unidade unidade = db.Unidades.Find(id);
            if (unidade == null)
            {
                return HttpNotFound();
            }
            return View(unidade);
        }

        // POST: Unidades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDUnidade,NomeUnidade,Cidade,TelefoneUnidade,SiteCode,Endereco,RazaoSocial,CNPJ")] Unidade unidade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unidade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unidade);
        }

        // GET: Unidades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unidade unidade = db.Unidades.Find(id);
            if (unidade == null)
            {
                return HttpNotFound();
            }
            return View(unidade);
        }

        // POST: Unidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Unidade unidade = db.Unidades.Find(id);
            db.Unidades.Remove(unidade);
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

        public ActionResult Procurar(string usuarios)
        {
            var list = new List<Unidade>();
            
            list = db.Unidades.ToList();

            return PartialView("Resultado", list.OrderBy(u => u.Cidade));
        }

    }
}
