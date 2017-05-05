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
    public class LinkTelefoniasController : Controller
    {
        private BancoContexto db = new BancoContexto();

        // GET: LinkTelefonias
        public ActionResult Index()
        {
            var linkTelefonias = db.LinkTelefonias.Include(l => l.Unidade).Include(o => o.Operadora);
            return View(linkTelefonias.ToList());
        }

        // GET: LinkTelefonias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LinkTelefonia linkTelefonia = db.LinkTelefonias.Find(id);
            if (linkTelefonia == null)
            {
                return HttpNotFound();
            }
            return View(linkTelefonia);
        }

        // GET: LinkTelefonias/Create
        public ActionResult Create()
        {
            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade");
            ViewBag.IDOperadora = new SelectList(db.Operadoras, "IDOperadora", "NomeOperadora");
            return View();
        }

        // POST: LinkTelefonias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDLinkTelefonia,CodigoCliente,DDD,TroncoChave,NumerosPortadosInicio,NumerosPortadosFim,Contato,IDUnidade,IDOperadora")] LinkTelefonia linkTelefonia)
        {
            if (ModelState.IsValid)
            {
                db.LinkTelefonias.Add(linkTelefonia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade", linkTelefonia.IDUnidade);
            ViewBag.IDOperadora = new SelectList(db.Operadoras, "IDOperadora", "NomeOperadora");
            return View(linkTelefonia);
        }

        // GET: LinkTelefonias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LinkTelefonia linkTelefonia = db.LinkTelefonias.Find(id);
            if (linkTelefonia == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade", linkTelefonia.IDUnidade);
            ViewBag.IDOperadora = new SelectList(db.Operadoras, "IDOperadora", "NomeOperadora");
            return View(linkTelefonia);
        }

        // POST: LinkTelefonias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDLinkTelefonia,CodigoCliente,DDD,TroncoChave,NumerosPortadosInicio,NumerosPortadosFim,IDUnidade,IDOperadora")] LinkTelefonia linkTelefonia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(linkTelefonia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade", linkTelefonia.IDUnidade);
            ViewBag.IDOperadora = new SelectList(db.Operadoras, "IDOperadora", "NomeOperadora");
            return View(linkTelefonia);
        }

        // GET: LinkTelefonias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LinkTelefonia linkTelefonia = db.LinkTelefonias.Find(id);
            if (linkTelefonia == null)
            {
                return HttpNotFound();
            }
            return View(linkTelefonia);
        }

        // POST: LinkTelefonias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LinkTelefonia linkTelefonia = db.LinkTelefonias.Find(id);
            db.LinkTelefonias.Remove(linkTelefonia);
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
