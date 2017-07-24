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
    public class ServidoresController : Controller
    {
        private BancoContexto db = new BancoContexto();

        // GET: Servidores
        public ActionResult Index()
        {
            var servidors = db.Servidors.Include(s => s.Unidade);
            return View(servidors.ToList());
        }

        // GET: Servidores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servidor servidor = db.Servidors.Find(id);
            if (servidor == null)
            {
                return HttpNotFound();
            }
            return View(servidor);
        }

        // GET: Servidores/Create
        public ActionResult Create()
        {
            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade");
            return View();
        }

        // POST: Servidores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDServidor,Hostname,IP,Descricao,IDUnidade")] Servidor servidor)
        {
            if (ModelState.IsValid)
            {
                db.Servidors.Add(servidor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade", servidor.IDUnidade);
            return View(servidor);
        }

        // GET: Servidores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servidor servidor = db.Servidors.Find(id);
            if (servidor == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade", servidor.IDUnidade);
            return View(servidor);
        }

        // POST: Servidores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDServidor,Hostname,IP,Descricao,IDUnidade")] Servidor servidor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servidor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade", servidor.IDUnidade);
            return View(servidor);
        }

        // GET: Servidores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servidor servidor = db.Servidors.Find(id);
            if (servidor == null)
            {
                return HttpNotFound();
            }
            return View(servidor);
        }

        // POST: Servidores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Servidor servidor = db.Servidors.Find(id);
            db.Servidors.Remove(servidor);
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
