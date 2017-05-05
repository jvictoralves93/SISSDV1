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
    public class OperadorasController : Controller
    {
        private BancoContexto db = new BancoContexto();

        // GET: Operadoras
        public ActionResult Index()
        {
            return View(db.Operadoras.ToList().OrderBy(op => op.NomeOperadora));
        }

        // GET: Operadoras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operadora operadora = db.Operadoras.Find(id);
            if (operadora == null)
            {
                return HttpNotFound();
            }
            return View(operadora);
        }

        // GET: Operadoras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Operadoras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDOperadora,NomeOperadora,Contato")] Operadora operadora)
        {
            if (ModelState.IsValid)
            {
                db.Operadoras.Add(operadora);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(operadora);
        }

        // GET: Operadoras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operadora operadora = db.Operadoras.Find(id);
            if (operadora == null)
            {
                return HttpNotFound();
            }
            return View(operadora);
        }

        // POST: Operadoras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDOperadora,NomeOperadora,Contato")] Operadora operadora)
        {
            if (ModelState.IsValid)
            {
                db.Entry(operadora).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(operadora);
        }

        // GET: Operadoras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operadora operadora = db.Operadoras.Find(id);
            if (operadora == null)
            {
                return HttpNotFound();
            }
            return View(operadora);
        }

        // POST: Operadoras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Operadora operadora = db.Operadoras.Find(id);
            db.Operadoras.Remove(operadora);
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
