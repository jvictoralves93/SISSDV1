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
using System.DirectoryServices.ActiveDirectory;
using System.Security.AccessControl;
using System.IO;

namespace SISSDV1.Controllers
{
    public class ComputadoresController : Controller
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
            Computador computador = new Computador();
            var list = new List<Computador>();
            return View(list.OrderBy(u => u.NomeComputador));
        }

        public ActionResult Procurar(string computadores)
        {
            Computador computador = new Computador();
            var list = new List<Computador>();

            if (computadores != "")
            {
                list = ProcurarComputadores(computadores);
            }
            else
            {
                list = BuscarTodos();
            }
            return PartialView("Resultado", list.OrderBy(u => u.NomeComputador).Take(100));
        }

        //Buscar Todos os Usuários
        [HttpPost]
        private List<Computador> BuscarTodos()
        {
            //Dados Conexão AD
            Context.ServerIp = "10.0.210.8";
            //Lista de Usuários
            List<Computador> restultado = new List<Computador>();

            //Abre conexão com o AD
            OpenAdConnection();
            // Cria o objeto search
            DirectorySearcher search = new DirectorySearcher(ldapConnection);

            // Adiciona Filtro
            search.Filter = "(objectClass=computer)";
            search.SearchScope = SearchScope.Subtree;
            SearchResultCollection resultCol = search.FindAll();

            // Checa se achou algo
            if (resultCol != null)
            {
                foreach (SearchResult r in resultCol)
                {
                    Computador computador = new Computador();
                    computador.NomeComputador = r.Properties["name"][0].ToString();
                    computador.Hostname = r.Properties["name"][0].ToString();
                    try
                    {
                        computador.SistemaOperacional = r.Properties["OperatingSystem"][0].ToString();
                    }
                    catch
                    {

                        computador.SistemaOperacional = "Sem Sistema Operacional";
                    }          
                    restultado.Add(computador);
                }
            }
            return restultado;
        }

        //Procurar Usuários
        [HttpPost]
        public List<Computador> ProcurarComputadores(string computadores)
        {
            //Dados Conexão AD
            Context.ServerIp = "10.0.210.8";
            //Lista de Usuários
            List<Computador> restultado = new List<Computador>();

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
                search.Filter = "(&(objectClass=computer)(|(cn=*"+computadores+"*)(operatingSystem=*"+computadores+"*)))";
                search.SearchScope = SearchScope.Subtree;
                SearchResultCollection resultCol = search.FindAll();

                // Checa se achou algo
                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        Computador computador = new Computador();
                        computador.NomeComputador = r.Properties["cn"][0].ToString();
                        try
                        {
                            computador.SistemaOperacional = r.Properties["OperatingSystem"][0].ToString();
                        }
                        catch
                        {

                            computador.SistemaOperacional = "Sem Sistema Operacional";
                        }
                        restultado.Add(computador);
                    }
                }
                return restultado;
            }
            catch
            {
                return restultado;
            }
        }

        //Procurar Um Computador
        [HttpPost]
        public DirectoryEntry ProcurarUmComputador(string hostname)
        {
            try
            {
                // Open connection with AD domain
                OpenAdConnection();

                // Create the object "search"
                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                // Add filter
                search.Filter = "(cn="+hostname+")";
                search.SearchScope = SearchScope.Subtree;

                SearchResult searchResult = search.FindOne();
                DirectoryEntry userEntry = searchResult.GetDirectoryEntry();
                CloseAdConnection();
                return userEntry;
            }
            catch (Exception ex)
            {
                CloseAdConnection();
                throw new Exception(ex.Message);
            }
        }

        public ActionResult Novo()
        {
            var cidades = new List<Models.OrganizationalUnit>();
            ViewBag.Cidade = new SelectList(cidades, "Cidade", "Cidade");
            var unidades = new List<OrganizationalUnit>();
            ViewBag.Unidade = new SelectList(unidades, "Unidade", "Unidade");
            var departamentos = new List<OrganizationalUnit>();
            ViewBag.Departamento = new SelectList(departamentos, "Departamento", "Departamento");
            return View();
        }

        //Novo Computador
        [HttpPost]
        public ActionResult CriarComputador(string hostname, string cidade, string unidade, string departamento)
        {
            string usuariologado = Request.Cookies["Username"].Value;
            string senhalogado = Request.Cookies["Senha"].Value;

            //DirectoryEntry tecnicoAdd = new DirectoryEntry("LDAP://"+Context.ServerIp+ProcurarTecnico(tecnico).OU.Ou);            

            try
            {
                //OU em que o host será criado
                DirectoryEntry connectionOu = new DirectoryEntry("LDAP://" +
                    Context.ServerIp + "/OU=Computers,OU=" + departamento + ",OU=Departamentos,OU=" + unidade + ",OU=Unidades"
                    + ",OU=" + cidade + ",OU=Cidades,DC=coc,DC=com,DC=br", usuariologado, senhalogado);

                //Cria o Objeto do Usuário
                DirectoryEntry computador = connectionOu.Children.Add(string.Format("CN={0}", hostname.ToUpper()), "computer");


                //Adiciona os atributos ao computador
                computador.Properties["cn"].Value = hostname.ToUpper();
                computador.Properties["name"].Value = hostname;
                computador.Properties["dNSHostName"].Value = hostname+"coc.com.br";
                computador.Properties["userAccountControl"].Value = 4128;
                computador.Properties["sAMAccountName"].Value = hostname+"$";
                
                //Salva alterações no AD
                computador.CommitChanges();

                //Fecha conexão
                connectionOu.Close();
                computador.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Resetar(string hostname)
        {
            return PartialView(new Computador { Hostname = hostname });
        }

        public ActionResult ResetHost(string hostname)
        {
            string usuariologado = Request.Cookies["Username"].Value;
            string senhalogado = Request.Cookies["Senha"].Value;

            DirectoryEntry host = ProcurarUmComputador(hostname);
            DirectoryEntry connection = new DirectoryEntry(host.Parent.Path, usuariologado, senhalogado);
            try
            {
                connection.Children.Remove(host);
                host.CommitChanges();
                try
                {
                    DirectoryEntry computador = connection.Children.Add(string.Format("CN={0}", hostname.ToUpper()), "computer");

                    //Adiciona os atributos ao computador
                    computador.Properties["cn"].Value = hostname.ToUpper();
                    computador.Properties["name"].Value = hostname;
                    computador.Properties["dNSHostName"].Value = hostname + "coc.com.br";
                    computador.Properties["userAccountControl"].Value = 4128;
                    computador.Properties["sAMAccountName"].Value = hostname + "$";

                    //Salva alterações no AD
                    computador.CommitChanges();
                    computador.Close();


                    host.CommitChanges();                    
                }
                catch (Exception)
                {

                    throw;
                }
            }
            catch (Exception e)
            {
                host.Close();
                connection.Close();
                throw new Exception(e.Message);
            }
            host.Close();
            connection.Close();
            return Json(JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Deletar(string hostname)
        {
            return PartialView(new Computador { Hostname = hostname });
        }

        public ActionResult DeleteHost(string hostname)
        {
            string usuariologado = Request.Cookies["Username"].Value;
            string senhalogado = Request.Cookies["Senha"].Value;

            DirectoryEntry host = ProcurarUmComputador(hostname);
            DirectoryEntry connection = new DirectoryEntry(host.Parent.Path, usuariologado, senhalogado);
            try
            {
                connection.Children.Remove(host);
                host.CommitChanges();
            }
            catch (Exception e)
            {
                host.Close();
                connection.Close();
                throw new Exception(e.Message);
            }
            host.Close();
            connection.Close();
            return Json(JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Sucesso()
        {
            return PartialView();
        }        

        //Retorna o Json Para Preencher o combobox
        public ActionResult VerificaHostname(string hostname)
        {
            var user = new List<Computador>();
            user = TesteHostname(hostname);
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        //Procurar Usuários
        [HttpPost]
        public List<Computador> TesteHostname(string hostname)
        {
            //Lista de Usuários
            List<Computador> resultado = new List<Computador>();

            //Função
            try
            {
                DirectoryEntry entry = new DirectoryEntry(server, username, password);
                //Abre conexão com o AD
                OpenAdConnection();
                // Cria o objeto search
                DirectorySearcher search = new DirectorySearcher(entry);

                // Adiciona Filtro
                search.Filter = "(cn=" + hostname + ")";
                search.SearchScope = SearchScope.Subtree;
                SearchResultCollection resultCol = search.FindAll();

                // Checa se achou algo
                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        Computador computador = new Computador();
                        computador.NomeComputador = r.Properties["cn"][0].ToString();

                        resultado.Add(computador);
                    }
                }
                return resultado;
            }
            catch
            {
                return resultado;
            }
        }

        //Retorna o Json Para Preencher o Gerente
        //public ActionResult BuscaTecnico(string tecnico)
        //{
        //    var buscado = new User();
        //    buscado = ProcurarTecnico(tecnico);
        //    return Json(buscado, JsonRequestBehavior.AllowGet);
        //}

        //Procurar Tecnico
        //[HttpPost]
        //public User ProcurarTecnico(string tecnico)
        //{
        //    //Lista de Usuários
        //    User resultado = new User();

        //    //Função
        //    try
        //    {
        //        //Abre conexão com o AD
        //        OpenAdConnection();
        //        // Cria o objeto search
        //        DirectorySearcher search = new DirectorySearcher(ldapConnection);

        //        // Adiciona Filtro
        //        search.Filter = "(|(displayName=*" + tecnico + "*)(userPrincipalName=*" + tecnico + "*))";
        //        search.SearchScope = SearchScope.Subtree;
        //        SearchResultCollection resultCol = search.FindAll();

        //        // Checa se achou algo
        //        if (resultCol != null)
        //        {
        //            foreach (SearchResult r in resultCol)
        //            {
        //                resultado.NomeExibicao = r.Properties["cn"][0].ToString();
        //                resultado.OU.Ou = r.Properties["distinguishedName"][0].ToString();
        //            }
        //        }
        //        return resultado;
        //    }
        //    catch
        //    {
        //        return resultado;
        //    }
        //}

        //public ActionResult Editar()
        //{
        //    var cidades = new List<Models.OrganizationalUnit>();
        //    ViewBag.Cidade = new SelectList(cidades, "Cidade", "Cidade");
        //    var unidades = new List<OrganizationalUnit>();
        //    ViewBag.Unidade = new SelectList(unidades, "Unidade", "Unidade");
        //    var departamentos = new List<OrganizationalUnit>();
        //    ViewBag.Departamento = new SelectList(departamentos, "Departamento", "Departamento");
        //    return PartialView();
        //}
    }
}