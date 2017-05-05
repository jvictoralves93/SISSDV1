using SISSDV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Data.SqlClient;
using System.Data.Sql;

namespace SISSDV1.Controllers
{
    public class EscalaController : Controller
    {
        private BancoContexto db = new BancoContexto();

        // GET: Escala
        public ActionResult Index()
        {
            return View(db.Escalas);
        }

        [HttpPost]
        public ActionResult CriarEvento(string title, DateTime start)
        {
            EscalaSabado novaescala = new EscalaSabado();
            novaescala.title = title;
            novaescala.description = null;
            novaescala.start = start;
            novaescala.end = start;
            novaescala.allday = false;

            db.Entry(novaescala).State = EntityState.Added;
            db.SaveChanges();
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarEventos()
        {
            List<EscalaSabado> eventos = new List<EscalaSabado>();

            foreach (var item in db.Escalas.ToList())
            {

                EscalaSabado evento = new EscalaSabado
                {
                    id = item.id,
                    title = item.title,
                    start = item.start,
                    end = item.end,
                    allday = item.allday
                };
                eventos.Add(evento);
            }
            var lista = eventos.ToArray();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AlterarEvento(string title, DateTime start)
        {
            EscalaSabado novaescala = new EscalaSabado();
            novaescala.title = title;
            novaescala.description = null;
            novaescala.start = start;
            novaescala.end = start;
            novaescala.allday = false;

            db.Entry(novaescala).State = EntityState.Modified;
            db.SaveChanges();
            return Json(JsonRequestBehavior.AllowGet);
        }

    }
}