﻿using System;
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
    public class LinksController : Controller
    {
        private BancoContexto db = new BancoContexto();

        // GET: Links
        public ActionResult Index()
        {
            var links = db.Links.Include(l => l.Unidade).Include(o => o.Operadora);
            return View(links.ToList());
        }

        // GET: Links/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
        }

        // GET: Links/Create
        public ActionResult Create()
        {
            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade");
            ViewBag.IDOperadora = new SelectList(db.Operadoras, "IDOperadora", "NomeOperadora");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDLink,Designacao,CodigoCliente,Capacidade,Tipo,Assinatura,IDUnidade,IDOperadora")] Link link)
        {
            if (ModelState.IsValid)
            {
                db.Links.Add(link);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade", link.IDUnidade);
            ViewBag.IDOperadora = new SelectList(db.Operadoras, "IDOperadora", "NomeOperadora", link.IDOperadora);
            return View(link);
        }

        // GET: Links/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade", link.IDUnidade);
            ViewBag.IDOperadora = new SelectList(db.Operadoras, "IDOperadora", "NomeOperadora", link.IDOperadora);
            return View(link);
        }

        // POST: Links/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDLink,Designacao,CodigoCliente,Capacidade,Tipo,IDUnidade,IDOperadora")] Link link)
        {
            if (ModelState.IsValid)
            {
                db.Entry(link).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUnidade = new SelectList(db.Unidades, "IDUnidade", "NomeUnidade", link.IDUnidade);
            ViewBag.IDOperadora = new SelectList(db.Operadoras, "IDOperadora", "NomeOperadora", link.IDOperadora);
            return View(link);
        }

        // GET: Links/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
        }

        // POST: Links/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Link link = db.Links.Find(id);
            db.Links.Remove(link);
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
