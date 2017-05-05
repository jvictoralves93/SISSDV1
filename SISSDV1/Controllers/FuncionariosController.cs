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
    public class FuncionariosController : Controller
    {
        private BancoRM db = new BancoRM();

        // GET: Funcionarios
        public ActionResult Index()
        {
            return View(db.Funcionarios.ToList().OrderBy(i => i.NOME).Take(0));
        }

        public ActionResult Pesquisar()
        {
            var list = new List<Models.Funcionarios>();
            list = db.Funcionarios.ToList();
            return PartialView("Resultado", list);
        }

        // GET: Funcionarios/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Funcionarios Funcionarios = db.Funcionarios.Find(id);
            if (Funcionarios == null)
            {
                return HttpNotFound();
            }
            return View(Funcionarios);
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
