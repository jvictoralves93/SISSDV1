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
            novaescala.description = title;
            novaescala.start = start;
            novaescala.end = start;
            novaescala.allday = true;

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
                    end = item.start,
                    allday = item.allday
                };
                eventos.Add(evento);
            }
            var lista = eventos.ToArray();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AlterarEvento(int id, string title, DateTime start)
        {
            EscalaSabado escala = new EscalaSabado();
            escala.id = id;
            escala.title = title;
            escala.start = start;
            escala.end = start;
            escala.allday = true;
            escala.description = title;

            db.Entry(escala).State = EntityState.Modified;
            db.SaveChanges();
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult ModalExcluir(int id)
        {
            return View(new EscalaSabado { id = id });
        }
        [HttpPost]
        public ActionResult ExcluirEvento(int id)
        {
            EscalaSabado escala = db.Escalas.Find(id);
            if (escala.title == "Feriado")
            {
                List<EscalaSabado> escalas = db.Escalas.Where(i => i.start >= escala.start).ToList();
                foreach (EscalaSabado escalasab in escalas)
                {
                    escalasab.id = escalasab.id;
                    escalasab.title = escalasab.title;
                    escalasab.start = escalasab.start.AddDays(-7);
                    escalasab.end = escalasab.end.AddDays(-7);
                    escalasab.allday = true;
                    escalasab.description = escalasab.title;

                    db.Entry(escalasab).State = EntityState.Modified;
                    db.SaveChanges();
                }
                db.Escalas.Remove(escala);
                db.SaveChanges();
            }
            else
            {
                db.Escalas.Remove(escala);
                db.SaveChanges();
            }            
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult Sucesso()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AdiantarEventos(DateTime start)
        {
            List<EscalaSabado> escalas = db.Escalas.Where(i => i.start >= start).ToList();
            foreach (EscalaSabado escala in escalas)
            {
                escala.id = escala.id;
                escala.title = escala.title;
                escala.start = escala.start.AddDays(7);
                escala.end = escala.end.AddDays(7);
                escala.allday = true;
                escala.description = escala.title;
                
                db.Entry(escala).State = EntityState.Modified;
                db.SaveChanges();
            }

            EscalaSabado novaescala = new EscalaSabado();
            novaescala.title = "Feriado";
            novaescala.description = "Feriado";
            novaescala.start = start;
            novaescala.end = start;
            novaescala.allday = true;

            db.Entry(novaescala).State = EntityState.Added;
            db.SaveChanges();


            return Json(JsonRequestBehavior.AllowGet);
        }

    }
}