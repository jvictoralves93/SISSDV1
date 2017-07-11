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

namespace SISSDV1.Controllers
{
    public class GruposController : Controller
    {
        //------------------------------- Funções AD ------------------------------------------//

        //Variáveis de conexão
        private DirectoryEntry ldapConnection;
        public string server = "LDAP://" + Context.ServerIp;
        private string username = "sissd.aplicacao";
        private string password = "Pass@2017";

        //Abrir conexão
        void OpenAdConnection()
        {
            string usuariologado = Request.Cookies["Username"].Value;
            string senhalogado = Request.Cookies["Senha"].Value;
            ldapConnection = new DirectoryEntry(server, usuariologado, senhalogado);
        }

        //Abrir conexão
        DirectoryEntry OpenAdConnection(string server)
        {
            DirectoryEntry connection = new DirectoryEntry(server, username, password);
            return connection;
        }

        //Mudar Servidores AD
        public void ChangeServerIp()
        {
            if (Context.ServerIp == "10.0.210.8")
                Context.ServerIp = "10.0.210.8";
            else
                Context.ServerIp = "10.0.210.9";
        }

        //Fechar conexão
        void CloseAdConnection()
        {
            ldapConnection.Close();
        }

        //Fechar conexão
        void CloseAdConnection(DirectoryEntry connection)
        {
            connection.Close();
        }
        //------------------------------- Fim Funções AD ------------------------------------------//

        //------------------------------- Funções Controller ------------------------------------------//
        // GET: Computadores
        public ActionResult Index()
        {
            GrupoAD grupo = new GrupoAD();
            var list = new List<GrupoAD>();
            return View(list.OrderBy(u => u.GrupoNome));
        }

        public ActionResult Procurar(string grupos)
        {
            GrupoAD computador = new GrupoAD();
            var list = new List<GrupoAD>();

            if (grupos != "")
            {
                list = ProcurarGrupos(grupos);
            }
            else
            {
                list = BuscarTodos();
            }
            return PartialView("Resultado", list.OrderBy(u => u.GrupoNome).Take(100));
        }

        //Buscar Todos os Usuários
        [HttpPost]
        private List<GrupoAD> BuscarTodos()
        {
            //Dados Conexão AD
            Context.ServerIp = "10.0.210.8";
            //Lista de Usuários
            List<GrupoAD> restultado = new List<GrupoAD>();

            //Abre conexão com o AD
            OpenAdConnection();
            // Cria o objeto search
            DirectorySearcher search = new DirectorySearcher(ldapConnection);

            // Adiciona Filtro
            search.Filter = "(objectClass=group)";
            search.SearchScope = SearchScope.Subtree;
            SearchResultCollection resultCol = search.FindAll();

            // Checa se achou algo
            if (resultCol != null)
            {
                foreach (SearchResult r in resultCol)
                {
                    GrupoAD grupo = new GrupoAD();
                    grupo.GrupoNome = r.Properties["cn"][0].ToString();                

                    restultado.Add(grupo);
                }
            }
            return restultado;
        }

        //Procurar Usuários
        [HttpPost]
        public List<GrupoAD> ProcurarGrupos(string grupos)
        {
            //Dados Conexão AD
            Context.ServerIp = "10.0.210.8";
            //Lista de Usuários
            List<GrupoAD> restultado = new List<GrupoAD>();

            //Função
            try
            {
                //Abre conexão com o AD
                OpenAdConnection();
                // Cria o objeto search
                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                // Adiciona Filtro
                search.PageSize = 50;
                search.SizeLimit = 50;
                search.Filter = "(&(objectClass=group)(cn=*" + grupos + "*))";
                search.SearchScope = SearchScope.Subtree;
                SearchResultCollection resultCol = search.FindAll();

                // Checa se achou algo
                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        GrupoAD grupo = new GrupoAD();
                        grupo.GrupoNome = r.Properties["cn"][0].ToString();
                        restultado.Add(grupo);
                    }
                }
                return restultado;
            }
            catch
            {
                return restultado;
            }
        }

        //public ActionResult Novo()
        //{
        //    var cidades = new List<Models.OrganizationalUnit>();
        //    ViewBag.Cidade = new SelectList(cidades, "Cidade", "Cidade");
        //    var unidades = new List<OrganizationalUnit>();
        //    ViewBag.Unidade = new SelectList(unidades, "Unidade", "Unidade");
        //    var departamentos = new List<OrganizationalUnit>();
        //    ViewBag.Departamento = new SelectList(departamentos, "Departamento", "Departamento");
        //    return View();
        //}

        public ActionResult Detalhes()
        {
            return PartialView();
        }

        public ActionResult Sucesso()
        {
            return PartialView();
        }
    }
}