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
    public class TecnicosController : Controller
    {
        private BancoContexto db = new BancoContexto();

        // GET: Tecnicos
        public ActionResult Index()
        {
            var tecnicoes = db.Tecnicoes.Include(t => t.Unidade);
            return View(tecnicoes.ToList());
        }

        // GET: Tecnicos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tecnico tecnico = db.Tecnicoes.Find(id);
            if (tecnico == null)
            {
                return HttpNotFound();
            }
            return View(tecnico);
        }

        // GET: Tecnicos/Create
        public ActionResult Create()
        {
            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade");
            return View();
        }

        // POST: Tecnicos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDTecnico,NomeTecnico,Celular,Ramal,IDUnidade")] Tecnico tecnico)
        {
            if (ModelState.IsValid)
            {
                db.Tecnicoes.Add(tecnico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade", tecnico.IDUnidade);
            return View(tecnico);
        }

        // GET: Tecnicos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tecnico tecnico = db.Tecnicoes.Find(id);
            if (tecnico == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade", tecnico.IDUnidade);
            return View(tecnico);
        }

        // POST: Tecnicos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDTecnico,NomeTecnico,Celular,Ramal,IDUnidade")] Tecnico tecnico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tecnico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade", tecnico.IDUnidade);
            return View(tecnico);
        }

        // GET: Tecnicos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tecnico tecnico = db.Tecnicoes.Find(id);
            if (tecnico == null)
            {
                return HttpNotFound();
            }
            return View(tecnico);
        }

        // POST: Tecnicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tecnico tecnico = db.Tecnicoes.Find(id);
            db.Tecnicoes.Remove(tecnico);
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
