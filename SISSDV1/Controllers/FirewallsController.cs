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
    public class FirewallsController : Controller
    {
        private BancoContexto db = new BancoContexto();

        // GET: Firewalls
        public ActionResult Index()
        {
            var firewalls = db.Firewalls.Include(f => f.Unidade);
            return View(firewalls.ToList());
        }

        // GET: Firewalls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Firewall firewall = db.Firewalls.Find(id);
            if (firewall == null)
            {
                return HttpNotFound();
            }
            return View(firewall);
        }

        // GET: Firewalls/Create
        public ActionResult Create()
        {
            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade");
            return View();
        }

        // POST: Firewalls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDFirewall,Modelo,Licenca,AcessoInterno,AcessoExterno,IDUnidade")] Firewall firewall)
        {
            if (ModelState.IsValid)
            {
                db.Firewalls.Add(firewall);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade", firewall.IDUnidade);
            return View(firewall);
        }

        // GET: Firewalls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Firewall firewall = db.Firewalls.Find(id);
            if (firewall == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade", firewall.IDUnidade);
            return View(firewall);
        }

        // POST: Firewalls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDFirewall,Modelo,Licenca,AcessoInterno,AcessoExterno,IDUnidade")] Firewall firewall)
        {
            if (ModelState.IsValid)
            {
                db.Entry(firewall).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade", firewall.IDUnidade);
            return View(firewall);
        }

        // GET: Firewalls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Firewall firewall = db.Firewalls.Find(id);
            if (firewall == null)
            {
                return HttpNotFound();
            }
            return View(firewall);
        }

        // POST: Firewalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Firewall firewall = db.Firewalls.Find(id);
            db.Firewalls.Remove(firewall);
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
