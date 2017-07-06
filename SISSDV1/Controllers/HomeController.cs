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
    public class HomeController : Controller
    {
        private BancoContexto db = new BancoContexto();
        private BancoRM db2 = new BancoRM();

        //--------------------------------------- Funções AD ------------------------------------------//
        private DirectoryEntry ldapConnection;
        public string server = "LDAP://" + Context.ServerIp;
        private string username = "sissd.aplicacao";
        private string password = "Pass@2017";

        void OpenAdConnection()
        {
            ldapConnection = new DirectoryEntry(server, username, password);
        }

        DirectoryEntry OpenAdConnection(string server)
        {
            DirectoryEntry connection = new DirectoryEntry(server, username, password);
            return connection;
        }

        void CloseAdConnection()
        {
            ldapConnection.Close();
        }

        public void ChangeServerIp()
        {
            if (Context.ServerIp == "10.0.210.8")
                Context.ServerIp = "10.0.210.8";
            else
                Context.ServerIp = "10.0.210.9";
        }

        void CloseAdConnection(DirectoryEntry connection)
        {
            connection.Close();
        }

        //--------------------------------- Fim Funções AD -------------------------------------//

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Escala()
        {
            List<EscalaSabado> escala = new List<EscalaSabado>();
            escala = db.Escalas.Where(i => i.start.Month >= DateTime.Now.Month).Take(4).ToList();

            return PartialView(escala);
        }                

        public ActionResult Aniversariantes()
        {
            List<Funcionarios> aniversariantes = new List<Funcionarios>();
            aniversariantes = db2.Funcionarios.Where(i => i.DTNASCIMENTO.Month.ToString() == DateTime.Now.Month.ToString() && i.DTNASCIMENTO.Day >= DateTime.Now.Day && i.FILIAL == "SEB CSC").OrderBy(i => i.DTNASCIMENTO.Day).ToList();

            return PartialView(aniversariantes);
        }

        public ActionResult ResumoFuncionarios()
        {
            List<Funcionarios> funcionarios = db2.Funcionarios.ToList();


            ViewBag.qtd = funcionarios.Count();
            ViewBag.ferias = funcionarios.Where(i => i.SITUACAO == "Férias").Count();
            ViewBag.licenca = funcionarios.Where(i => i.SITUACAO == "Licença Mater.").Count();

            return View();
        }

        public ActionResult ResumoUnidades()
        {
            ViewBag.unidades = db.Unidades.ToList().Count();
            return View();
        }

        public ActionResult SiteCode()
        {
            List<Unidade> sitecode = db.Unidades.ToList();
            return View(sitecode);
        }

        public ActionResult ResumoAD()
        {
            return View();
        }

        public ActionResult ResumoLinks()
        {
            List<Operadora> operadoras = db.Operadoras.ToList();
            List<Link> links = db.Links.ToList();
            List<LinkTelefonia> linkstelefonia = db.LinkTelefonias.ToList();

            ViewBag.operadoras = operadoras.Count();
            ViewBag.links = links.Count();
            ViewBag.linkstel = linkstelefonia.Count();
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string senha)
        {
            Repository repository = new Repository();
            //Dados para conexão no AD
            Context.ServerIp = "10.0.210.8";
            string gruposissd = "CN=RAO_CSC_TI_SisSD,OU=Front Line,OU=Infraestrutura,OU=TI,OU=Departamentos,OU=CSC,OU=Unidades,OU=Ribeirao Preto,OU=Cidades,DC=coc,DC=com,DC=br";
            //Testa Função login com o usuário e senha passados no formulário
            if (LoginUser(username, senha))
            {
                //Buscar Grupos
                User usuario = new User();              
                List<GrupoAD> grupoAcesso = BuscaGrupos(username);
                foreach (GrupoAD grupo in grupoAcesso)
                {
                    if (grupo.GrupoNome == gruposissd)
                    {
                        usuario.Admin = true;
                        Response.Cookies.Add(new HttpCookie("Acesso", "Permitido"));
                        Response.Cookies["Acesso"].Expires = DateTime.Now.AddDays(1);
                    }
                }
                //Salvando variáveis com os dados do usuário Logado           
                User userEntry = SearchOneUser(username);           
                usuario.NomeExibicao = userEntry.NomeExibicao;
                usuario.Nome = userEntry.Nome;
                usuario.Sobrenome = userEntry.Sobrenome;
                usuario.Email = userEntry.Email;
                usuario.Cargo = userEntry.Cargo;
                usuario.Username = username;
                usuario.Senha = senha;

                //Salvando Dados em Cookies
                Response.Cookies.Add(new HttpCookie("NomeCompleto", usuario.NomeExibicao));
                Response.Cookies["NomeCompleto"].Expires = DateTime.Now.AddDays(1);

                Response.SetCookie(new HttpCookie("Nome", usuario.Nome)); //Response.Cookies.Add(new HttpCookie("Nome", usuario.Nome));
                Response.Cookies["Nome"].Expires = DateTime.Now.AddDays(1);

                Response.Cookies.Add(new HttpCookie("Sobrenome", usuario.Sobrenome));
                Response.Cookies["Sobrenome"].Expires = DateTime.Now.AddDays(1);

                Response.Cookies.Add(new HttpCookie("Email", usuario.Email));
                Response.Cookies["Email"].Expires = DateTime.Now.AddDays(1);

                Response.Cookies.Add(new HttpCookie("Cargo", usuario.Cargo));
                Response.Cookies["Cargo"].Expires = DateTime.Now.AddDays(1);

                Response.Cookies.Add(new HttpCookie("Username", usuario.Username));
                Response.Cookies["Username"].Expires = DateTime.Now.AddDays(1);

                Response.Cookies.Add(new HttpCookie("Senha", usuario.Senha));
                Response.Cookies["Senha"].Expires = DateTime.Now.AddDays(1);
                                
                return Index();
            }
            else
            {
                ModelState.AddModelError("", "Usuário ou senha Incorretos");
                return View("Login");
            }
        }

        //Login View
        public ActionResult Login()
        {
            return View();
        }        

        //Ajuda
        public  ActionResult Ajuda()
        {
            return View();
        }

        //Sobre
        public ActionResult Sobre()
        {
            ViewBag.Message = "Página de Informações.";

            return View();
        }

        //Contato
        public ActionResult Contato()
        {
            ViewBag.Message = "Página de Contato";
            return View();
        }

        public ActionResult Pesquisar(string pesquisa)
        {
            return View(
                db.Unidades.Where(uni => uni.NomeUnidade.Contains(pesquisa) || uni.Cidade.Contains(pesquisa) 
                || uni.RazaoSocial.Contains(pesquisa)).ToList());
        }
        

        public bool LoginUser(string username, string password)
        {
            string container = @"OU=Infraestrutura,OU=TI,OU=Departamentos,OU=CSC,OU=Unidades,OU=Ribeirao Preto,OU=Cidades,DC=coc,DC=com,DC=br";

            try
            {
                using (PrincipalContext connection = new PrincipalContext(ContextType.Domain, Context.ServerIp, container, this.username, this.password))
                {
                    if (connection.ValidateCredentials(username, password))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }                        
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public User SearchOneUser(string username)
        {
            //Dados Conexão AD
            Context.ServerIp = "10.0.210.8";
            //Lista de Usuários
            User resultado = new User();

            //Função
            try
            {
                //Abre conexão com o AD
                OpenAdConnection();
                // Cria o objeto search
                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                // Adiciona Filtro
                search.Filter = "(sAMAccountName=*" + username + "*)";
                search.SearchScope = SearchScope.Subtree;
                SearchResultCollection resultCol = search.FindAll();

                // Checa se achou algo
                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        resultado.NomeExibicao = r.Properties["displayName"][0].ToString();
                        resultado.Nome = r.Properties["givenName"][0].ToString();
                        resultado.Sobrenome = r.Properties["sn"][0].ToString();
                        resultado.Email = r.Properties["mail"][0].ToString();
                        resultado.Cargo = r.Properties["title"][0].ToString();
                        resultado.Username = r.Properties["sAMAccountName"][0].ToString();
                    }
                }
                return resultado;
            }
            catch
            {
                return resultado;
            }
        }

        //Busca Os Grupos do Usuário Modelo
        public List<GrupoAD> BuscaGrupos(string username)
        {
            //Lista de grupos
            List<GrupoAD> grupos = new List<GrupoAD>();

            //Função
            try
            {
                //Abre conexão com o AD
                OpenAdConnection();
                // Cria o objeto search
                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                // Adiciona Filtro
                search.Filter = "(samAccountName=" + username + ")";
                search.SearchScope = SearchScope.Subtree;
                SearchResultCollection resultCol = search.FindAll();

                // Checa se achou algo
                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        foreach (object grp in r.Properties["memberOf"])
                        {
                            GrupoAD grupo = new GrupoAD();
                            grupo.GrupoNome = grp.ToString();

                            grupos.Add(grupo);
                        }
                    }
                }
                return grupos;
            }
            catch
            {
                return grupos;
            }
        }

        public ActionResult Sair(string usuario)
        {
            //Mata os cookies
            Response.Cookies["NomeCompleto"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Nome"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Sobrenome"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Email"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Cargo"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Username"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Acesso"].Expires = DateTime.Now.AddDays(-1);
            //Retorna a View para logar novamente
            return View("Login");
        }        
    }
}