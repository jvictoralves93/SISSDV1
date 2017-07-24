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
    public class UsersController : Controller
    {
        private BancoRM db = new BancoRM();

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

        // GET: Index
        public ActionResult Index()
        {
            Models.User user = new Models.User();
            var list = new List<Models.User>();
            return View(list.OrderBy(u => u.NomeExibicao));
        }      
        
        public ActionResult Procurar(string usuarios)
        {            
            Models.User user = new Models.User();
            var list = new List<Models.User>();

            if (usuarios != "")
            {
                list = ProcurarUsuarios(usuarios);
            }
            else
            {
                list = BuscarTodos();
            }
            return PartialView("Resultado", list.OrderBy(u => u.NomeExibicao).Take(10));
        }

        //Buscar Todos os Usuários
        [HttpPost]
        public List<User> BuscarTodos()
        {
            //Dados Conexão AD
            Context.ServerIp = "10.0.210.8";
            //Lista de Usuários
            List<User> resultado = new List<User>();

            //Abre conexão com o AD
            OpenAdConnection();
            // Cria o objeto search
            DirectorySearcher search = new DirectorySearcher(ldapConnection);

            // Adiciona Filtro
            search.Filter = "(sAMAccountType=805306368)";
            search.SearchScope = SearchScope.Subtree;            
            search.Asynchronous = true;
            SearchResultCollection resultCol = search.FindAll();

            // Checa se achou algo
            if (resultCol != null)
            {
                foreach (SearchResult r in resultCol)
                {
                        User user = new User();

                        try
                        {
                            user.Chapa = r.Properties["description"][0].ToString();
                        }
                        catch
                        {
                            user.Chapa = "Sem chapa";
                        }
                        try
                        {
                            user.Email = r.Properties["userPrincipalName"][0].ToString();
                        }
                        catch
                        {
                            user.Email = "Sem Email";
                        }
                        try
                        {
                            user.NomeExibicao = r.Properties["cn"][0].ToString();
                        }
                        catch
                        {
                            user.NomeExibicao = "Sem Nome";
                        }

                        try
                        {
                            user.Cargo = r.Properties["title"][0].ToString();
                        }
                        catch
                        {
                            user.Cargo = "Sem Cargo";
                        }
                        try
                        {
                            int status = (int)r.Properties["userAccountControl"][0];
                            if (status == 0x0200 || status == 0x0220)
                            {
                                user.Status = true;
                                user.StatusTexto = "Ativado";
                            }
                            else
                            {
                                user.Status = false;
                                user.StatusTexto = "Desativado";
                            }
                        }
                        catch
                        {
                            user.StatusTexto = "Desativado";
                        }
                        resultado.Add(user);
                }
            }            
            //CloseAdConnection();
            return resultado;
        }

        //Procurar Usuários
        [HttpPost]
        public List<User> ProcurarUsuarios(string usuarios)
        {
            //Dados Conexão AD
            Context.ServerIp = "10.0.210.8";
            //Lista de Usuários
            List<User> resultado = new List<User>();

            //Função
            try
            {
                //Abre conexão com o AD
                OpenAdConnection();
                // Cria o objeto search
                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                // Adiciona Filtro
                search.PageSize = 10;
                search.SizeLimit = 10;
                search.Asynchronous = true;
                search.Filter = "(sAMAccountType=805306368)";
                search.Filter = "(&(sAMAccountType=805306368)(|(displayName=*" + usuarios+"*)"
                    +"(userPrincipalName=*"+ usuarios+ "*)(title=*" + usuarios + "*)))";
                search.SearchScope = SearchScope.Subtree;
                search.Asynchronous = true;
                SearchResultCollection resultCol = search.FindAll();

                // Checa se achou algo
                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        User user = new User();
                        try
                        {
                            user.Chapa = r.Properties["description"][0].ToString();
                        }
                        catch
                        {
                            user.Chapa = "Sem Chapa";
                        }
                        try
                        {
                            user.Email = r.Properties["userPrincipalName"][0].ToString();
                        }
                        catch
                        {
                            user.Email = "Sem Email";
                        }
                        try
                        {
                            user.NomeExibicao = r.Properties["cn"][0].ToString();
                        }
                        catch
                        {
                            user.NomeExibicao = "Sem Nome";
                        }
                        try
                        {
                            user.Cargo = r.Properties["title"][0].ToString();
                        }
                        catch
                        {

                            user.Cargo = "Sem Cargo";
                        }
                        int status = (int)r.Properties["userAccountControl"][0];
                        if (status == 0x0200 || status == 0x0220)
                        {
                            user.Status = true;
                            user.StatusTexto = "Ativado";
                        }
                        else
                        {
                            user.Status = false;
                            user.StatusTexto = "Desativado";
                        }

                        resultado.Add(user);
                    }
                }
                return resultado;
            }
            catch
            {                
                return resultado;
            }
        }

        //Get Novo Usuário
        public ActionResult NovoUsuario()
        {
            //Combobox Cidade, Unidade, Departamento
            var cidades = new List<OrganizationalUnit>();
            ViewBag.Cidade = new SelectList(cidades, "Cidade", "Cidade");
            var unidades = new List<OrganizationalUnit>();
            ViewBag.Unidade = new SelectList(unidades, "Unidade", "Unidade");
            var departamentos = new List<OrganizationalUnit>();
            ViewBag.Departamento = new SelectList(departamentos, "Departamento", "Departamento");

            //Combobox Horario de Logon
            //var diassemana = new List<string> { "Domingo" ,"Segunda","Terça", "Quarta", "Quinta", "Sexta", "Sábado" };
            //ViewBag.DiaInicio = new SelectList(diassemana, "Segunda");
            //ViewBag.DiaFim = new SelectList(diassemana, "Sexta");
            //var horas = new List<string> { "01:00", "02:00", "03:00", "04:00", "05:00",
            //    "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00",
            //    "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00",
            //    "20:00", "21:00", "22:00", "23:00", "00:00" };
            //ViewBag.HorarioInicio = new SelectList(horas, "08:00");
            //ViewBag.HorarioFim = new SelectList(horas, "18:00");
            return View();
        }

        //Novo Usuário
        [HttpPost]
        public ActionResult CriarUsuario(string nome, string sobrenome, string username, string cargo, string cpf, string telefone,
            string chapa, string senha, string cidade, string unidade, string departamento, string subdepartamento)
        {
            //Dados Conexão AD
            Context.ServerIp = "10.0.210.8";

            string usuariologado = Request.Cookies["Username"].Value;
            string senhalogado = Request.Cookies["Senha"].Value;

            //Dados Padrões
            string NomeExibicao = nome + " " + sobrenome;
            User usermod = BuscaUserMod(cidade, unidade, departamento);
            List<GrupoAD> grupos = BuscaGrupos(usermod.Username);
            try
            {
                //string para adicionar o usuário aos grupos
                string usercaminho = "CN="+NomeExibicao+",OU=Users,OU=" + departamento + ",OU=Departamentos,OU=" + unidade + ",OU=Unidades"
                    + ",OU=" + cidade + ",OU=Cidades,DC=coc,DC=com,DC=br";

                //OU em que o usuário será criado
                DirectoryEntry connectionOu = new DirectoryEntry("LDAP://" +
                    Context.ServerIp + "/OU=Users,OU=" + departamento + ",OU=Departamentos,OU=" + unidade + ",OU=Unidades"
                    + ",OU=" + cidade + ",OU=Cidades,DC=coc,DC=com,DC=br", usuariologado, senhalogado);             

                //Cria o Objeto do Usuário
                DirectoryEntry user = connectionOu.Children.Add(string.Format("CN={0}", NomeExibicao), "user");

                //Caminho
                user.Properties["distinguishedName"].Value = usercaminho;

                //Adiciona os atributos ao usuário
                //Aba Geral                
                user.Properties["givenName"].Value = nome;
                user.Properties["sn"].Value = sobrenome;
                user.Properties["displayName"].Value = NomeExibicao;         
                user.Properties["description"].Value = chapa;
                user.Properties["name"].Value = NomeExibicao;                
                user.Properties["physicalDeliveryOfficeName"].Value = usermod.Escritorio;
                user.Properties["telephoneNumber"].Value = usermod.Telefone;
                user.Properties["mail"].Value = username + usermod.Email;                
                user.Properties["wWWHomePage"].Value = usermod.PaginaWeb;

                //Aba Endereço                
                user.Properties["streetAddress"].Value = usermod.Endereco;
                user.Properties["l"].Value = usermod.Cidade;
                user.Properties["st"].Value = usermod.Estado;
                user.Properties["postalCode"].Value = usermod.CEP;
                user.Properties["c"].Value = usermod.Pais;

                //Aba Conta
                user.Properties["userPrincipalName"].Value = string.Format("{0}@sebsa.com.br", username);
                user.Properties["sAMAccountName"].Value = username;


                //Horario de Logon
                //user.Properties["logonHours"].Value = "";


                //Aba Telefones
                user.Properties["ipPhone"].Value = telefone;
                user.Properties["info"].Value = cpf;

                //Aba Organização
                user.Properties["title"].Value = cargo;
                user.Properties["department"].Value = subdepartamento;
                user.Properties["company"].Value = unidade;
                user.Properties["manager"].Value = usermod.Gerente;

                //Aba Editor de Atributos                
                if (usermod.Email == "@sebsa.com.br")
                {
                    user.Properties["proxyAddresses"].Add("SMTP:" + username + "@sebsa.com.br");
                    user.CommitChanges();
                }
                else
                {
                    user.Properties["proxyAddresses"].Add("smtp:" + username + "@sebsa.com.br");
                    user.Properties["proxyAddresses"].Add("SMTP:" + username + usermod.Email);
                    user.CommitChanges();
                }              
                
                //Seta a senha, depois salva alterações no AD
                user.Invoke("SetPassword", senha);
                user.Properties["pwdLastSet"].Value = 0;
                user.CommitChanges();                

                //Habilita o usuário
                HabilitarUsuario(username);

                //Grupos
                AdicionarGrupos(grupos, user);

                //Fecha conexão
                connectionOu.Close();
                user.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        private void AdicionarGrupos(List<GrupoAD> grupos, DirectoryEntry user)
        {
            string usuariologado = Request.Cookies["Username"].Value;
            string senhalogado = Request.Cookies["Senha"].Value;
            foreach (GrupoAD grupo in grupos)
            {                
                DirectoryEntry group = new DirectoryEntry("LDAP://"+Context.ServerIp+"/"+grupo.GrupoNome, usuariologado, senhalogado);
                group.Properties["member"].Add(user.Properties["distinguishedName"].Value);
                group.CommitChanges();
                group.Close();      
            }            
        }

        private User BuscaUserMod(string cidade, string unidade, string departamento)
        {
            //Dados Conexão AD
            Context.ServerIp = "10.0.210.8";
            //Lista de Usuários
            User resultado = new User();
            List<GrupoAD> grupos = new List<GrupoAD>();

            //Função
            try
            {
                //Abre conexão com o AD
                DirectoryEntry entry = OpenAdConnection("LDAP://" +
                    Context.ServerIp + "/OU=" + departamento + ",OU=Departamentos,OU=" + unidade + ",OU=Unidades"
                    + ",OU=" + cidade + ",OU=Cidades,DC=coc,DC=com,DC=br");

                // Cria o objeto search
                DirectorySearcher search = new DirectorySearcher(entry);

                // Adiciona Filtro
                search.Filter = "(&(sAMAccountType=805306368)(displayName=*USR MOD*))";
                search.SearchScope = SearchScope.Subtree;
                search.Asynchronous = true;
                SearchResultCollection resultCol = search.FindAll();

                // Checa se achou algo
                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {                        
                        resultado.Pais = r.Properties["c"][0].ToString();
                        resultado.Estado = r.Properties["st"][0].ToString();
                        resultado.Cidade = r.Properties["l"][0].ToString();
                        resultado.Escritorio = r.Properties["physicalDeliveryOfficeName"][0].ToString();
                        resultado.PaginaWeb = r.Properties["wWWHomePage"][0].ToString();
                        resultado.CEP = r.Properties["postalCode"][0].ToString();
                        resultado.Telefone = r.Properties["telephoneNumber"][0].ToString();

                        int tamanho = r.Properties["mail"][0].ToString().IndexOf("@");

                        resultado.Email = r.Properties["mail"][0].ToString().Substring(tamanho);


                        resultado.Endereco = r.Properties["streetAddress"][0].ToString();
                        resultado.Gerente = r.Properties["manager"][0].ToString();
                        resultado.Username = r.Properties["samAccountName"][0].ToString();
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
                search.Filter = "(samAccountName="+username+")";
                search.SearchScope = SearchScope.Subtree;
                search.Asynchronous = true;
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

        //Retorna o Json Para Preencher o combobox
        public ActionResult Cidades()
        {
            var cidades = new List<OrganizationalUnit>();
            cidades = BuscarCidade();
            return Json(cidades, JsonRequestBehavior.AllowGet);
        }

        private List<OrganizationalUnit> BuscarCidade()
        {
            List<OrganizationalUnit> ouList = new List<OrganizationalUnit>();

            try
            {
                //Dados Conexão com AD
                Context.ServerIp = "10.0.210.8";
                string username = "sissd.aplicacao@coc.com.br";
                string password = "Pass@2017";
                DirectoryEntry entry = new DirectoryEntry("LDAP://" +
                    Context.ServerIp + "/OU=Cidades,DC=coc,DC=com,DC=br", username, password);

                DirectorySearcher search = new DirectorySearcher(entry);

                //Filtro de Pesquisa                
                search.Filter = "(objectClass=organizationalUnit)";
                search.SearchScope = SearchScope.OneLevel;
                search.Asynchronous = true;
                SearchResultCollection resultCol = search.FindAll();

                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        OrganizationalUnit ou = new OrganizationalUnit();
                        ou.Cidade = r.Properties["name"][0].ToString();
                        ouList.Add(ou);
                    }
                }
                entry.Close();
                return ouList;
            }
            catch
            {
                return ouList;
            }
        }
        public ActionResult Unidades(string cidade)
        {
            var unidades = new List<OrganizationalUnit>();
            unidades = BuscarUnidade(cidade);
            return Json(unidades, JsonRequestBehavior.AllowGet);
        }

        private List<OrganizationalUnit> BuscarUnidade(string cidade)
        {            
            List<OrganizationalUnit> ouList = new List<OrganizationalUnit>();

            //Dados Conexão com AD
            Context.ServerIp = "10.0.210.8";
            string username = "sissd.aplicacao@coc.com.br";
            string password = "Pass@2017";
            DirectoryEntry entry = new DirectoryEntry("LDAP://" +
                Context.ServerIp + "/OU=Unidades,OU=" + cidade + ",OU=Cidades,DC=coc,DC=com,DC=br", username, password);

            try
            {
                

                DirectorySearcher search = new DirectorySearcher(entry);

                //Filtro de Pesquisa                
                search.Filter = "(objectClass=organizationalUnit)";
                search.SearchScope = SearchScope.OneLevel;
                search.Asynchronous = true;
                SearchResultCollection resultCol = search.FindAll();

                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        OrganizationalUnit ou = new OrganizationalUnit();
                        ou.Unidade = r.Properties["name"][0].ToString();
                        ouList.Add(ou);
                    }
                }
                entry.Close();
                return ouList;
            }
            catch
            {
                entry.Close();
                return ouList;
            }
        }

        //Retorna o Json Para Preencher o combobox
        public ActionResult Departamento(string cidade, string unidade)
        {
            var departamentos = new List<OrganizationalUnit>();
            departamentos = BuscarDepartamento(cidade, unidade);
            return Json(departamentos, JsonRequestBehavior.AllowGet);
        }

        private List<OrganizationalUnit> BuscarDepartamento(string cidade, string unidade)
        {
            List<OrganizationalUnit> ouList = new List<OrganizationalUnit>();

            try
            {
                //Dados Conexão com AD
                Context.ServerIp = "10.0.210.8";
                string username = "sissd.aplicacao@coc.com.br";
                string password = "Pass@2017";
                DirectoryEntry entry = new DirectoryEntry("LDAP://" +
                    Context.ServerIp + "/OU=Departamentos,OU="+unidade+",OU=Unidades,OU="+cidade+",OU=Cidades,DC=coc,DC=com,DC=br", username, password);

                DirectorySearcher search = new DirectorySearcher(entry);

                //Filtro de Pesquisa                
                search.Filter = "(objectClass=organizationalUnit)";
                search.SearchScope = SearchScope.OneLevel;
                search.Asynchronous = true;
                SearchResultCollection resultCol = search.FindAll();

                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        OrganizationalUnit ou = new OrganizationalUnit();
                        ou.Departamento = r.Properties["name"][0].ToString();
                        ouList.Add(ou);
                    }
                }
                entry.Close();
                return ouList;
            }
            catch
            {
                return ouList;
            }
        }

        //Retorna o Json Para Preencher o Gerente
        public ActionResult BuscaGerente(string Gerente)
        {
            var buscado = new User();
            buscado = ProcurarGerente(Gerente);
            return Json(buscado, JsonRequestBehavior.AllowGet);
        }

        //Procurar Gerente
        [HttpPost]
        public User ProcurarGerente(string Gerente)
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
                search.Filter = "(|(displayName=*" + Gerente + "*)(userPrincipalName=*" + Gerente + "*)(distinguishedName="+Gerente+"))";
                search.SearchScope = SearchScope.Subtree;
                search.Asynchronous = true;
                SearchResultCollection resultCol = search.FindAll();

                // Checa se achou algo
                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        resultado.NomeExibicao = r.Properties["cn"][0].ToString();
                        resultado.Gerente = r.Properties["manager"][0].ToString();
                    }
                }
                return resultado;
            }
            catch
            {
                return resultado;
            }
        }
        
        //Retorna o Json Para Preencher o combobox
        public ActionResult VerificaUser(string usuario)
        {
            var user = new List<User>();
            user = TesteUsuario(usuario);
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        //Procurar Usuários
        [HttpPost]
        public List<User> TesteUsuario(string usuario)
        {
            //Dados Conexão AD
            Context.ServerIp = "10.0.210.8";
            string server = "LDAP://" + Context.ServerIp;
            string username = "sissd.aplicacao";
            string password = "Pass@2017";
            //Lista de Usuários
            List<User> resultado = new List<User>();

            //Função
            try
            {
                DirectoryEntry entry = new DirectoryEntry(server, username, password);
                //Abre conexão com o AD
                OpenAdConnection();
                // Cria o objeto search
                DirectorySearcher search = new DirectorySearcher(entry);

                // Adiciona Filtro
                search.Filter = "(samAccountName=" + usuario + ")";
                search.SearchScope = SearchScope.Subtree;
                search.Asynchronous = true;
                SearchResultCollection resultCol = search.FindAll();

                // Checa se achou algo
                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        User user = new User();
                        user.Email = r.Properties["distinguishedName"][0].ToString();

                        resultado.Add(user);
                    }
                }
                entry.Close();
                return resultado;
            }
            catch
            {
                return resultado;
            }
        }


        //Retorna o Json Para Preencher o Usuario
        public ActionResult DetalhesUser(string email)
        {
            var buscado = new User();
            buscado = Detalhes(email);
            return Json(buscado, JsonRequestBehavior.AllowGet);
        }

        //Procurar Usuários
        [HttpPost]
        public User Detalhes(string email)
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
                search.Filter = "(userPrincipalName=*" + email + "*)";
                search.SearchScope = SearchScope.Subtree;
                search.Asynchronous = true;
                SearchResultCollection resultCol = search.FindAll();

                // Checa se achou algo
                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        resultado.Nome = r.Properties["givenName"][0].ToString();
                        resultado.Sobrenome = r.Properties["sn"][0].ToString();
                        resultado.NomeExibicao = r.Properties["displayName"][0].ToString();
                        resultado.Email = r.Properties["mail"][0].ToString();
                        resultado.Cargo = r.Properties["title"][0].ToString();
                        resultado.Username = r.Properties["sAMAccountName"][0].ToString();
                        try
                        {
                            resultado.CPF = r.Properties["info"][0].ToString();
                        }
                        catch
                        {
                            resultado.CPF = "Sem CPF";
                        }
                        try
                        {
                            resultado.Chapa = r.Properties["description"][0].ToString();
                        }
                        catch
                        {
                            resultado.Chapa = "Sem chapa";
                        }
                        resultado.SubDepartamento = r.Properties["department"][0].ToString();
                        resultado.Pais = r.Properties["c"][0].ToString();
                        resultado.Estado = r.Properties["st"][0].ToString();
                        resultado.Cidade = r.Properties["l"][0].ToString();
                        resultado.Escritorio = r.Properties["physicalDeliveryOfficeName"][0].ToString();
                        resultado.PaginaWeb = r.Properties["wWWHomePage"][0].ToString();
                        resultado.CEP = r.Properties["postalCode"][0].ToString();
                        resultado.Telefone = r.Properties["telephoneNumber"][0].ToString();
                        resultado.Email = r.Properties["mail"][0].ToString();
                        resultado.Endereco = r.Properties["streetAddress"][0].ToString();
                        resultado.Gerente = r.Properties["manager"][0].ToString();
                        resultado.Username = r.Properties["samAccountName"][0].ToString();
                        resultado.Unidade = r.Properties["company"][0].ToString();

                        int status = (int)r.Properties["userAccountControl"][0];
                        if (status == 0x0200 || status == 0x0220)
                        {
                            resultado.Status = true;
                            resultado.StatusTexto = "Ativado";
                        }
                        else
                        {
                            resultado.Status = false;
                            resultado.StatusTexto = "Desativado";
                        }

                        string gerentesearch = resultado.Gerente;
                        User gerente = ProcurarGerente(gerentesearch);
                        resultado.Gerente = gerente.NomeExibicao;
                    }
                }
                return resultado;
            }
            catch
            {
                return resultado;
            }
        }

        public ActionResult ResetarSenha(string email)
        {
            return View(new User { Email = email });
        }

        //Resetar Senha
        public ActionResult ResetSenha(string username, string password)
        {
            try
            {
                // Open connection with AD domain
                OpenAdConnection();

                // Create the object "search"
                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                // Add filter
                search.Filter = "(userPrincipalName=*" + username+"*)";
                search.SearchScope = SearchScope.Subtree;

                SearchResult searchResult = search.FindOne();
                DirectoryEntry user = searchResult.GetDirectoryEntry();

                user.Invoke("SetPassword", password);
                user.Properties["pwdLastSet"].Value = 0;
                user.CommitChanges();


                CloseAdConnection();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
                 

        public ActionResult EditarUsuario(string email)
        {            
            var buscado = new User();
            buscado = Detalhes(email);

            return View(buscado);
        }

        public ActionResult EditUser(string username, string nome, string sobrenome, string chapa, string cargo, string subdepartamento,
            string cpf, string telefone)
        {
            string usuariologado = Request.Cookies["Username"].Value;
            string senhalogado = Request.Cookies["Senha"].Value;

            //Dados Padrões
            string NomeExibicao = nome + " " + sobrenome;
            try
            {
                // Open connection with AD domain
                OpenAdConnection();

                // Create the object "search"
                DirectorySearcher search = new DirectorySearcher(new DirectoryEntry(server, usuariologado, senhalogado));

                // Add filter
                search.Filter = "(userPrincipalName=*" + username + "*)";
                search.SearchScope = SearchScope.Subtree;
                search.Asynchronous = true;

                SearchResult searchResult = search.FindOne();

                DirectoryEntry user = new DirectoryEntry(searchResult.Path, usuariologado, senhalogado);

                //Adiciona os atributos ao usuário
                //Aba Geral                
                user.Properties["givenName"].Value = nome;
                user.Properties["sn"].Value = sobrenome;
                user.Properties["displayName"].Value = NomeExibicao;
                user.Properties["description"].Value = chapa;
                //user.Properties["name"].Value = NomeExibicao;                                              
                
                //Aba Telefones
                user.Properties["ipPhone"].Value = telefone;
                user.Properties["info"].Value = cpf;

                //Aba Organização
                user.Properties["title"].Value = cargo;
                user.Properties["department"].Value = subdepartamento;

                //Commit Nas alterações
                user.CommitChanges();
               
                user.Rename("CN=" + NomeExibicao);

                //Fecha conexão
                user.Close();
                user.Dispose();
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetalhesUsuario(string email)
        {
            var buscado = new User();
            buscado = Detalhes(email);

            return View(buscado);
        }

        public ActionResult Verificar(string chapa)
        {
            return View(new User { Chapa = chapa });
        }

        public ActionResult ComparaUserADcomUserRM(string chapa)
        {
            string usuariologado = Request.Cookies["Username"].Value;
            string senhalogado = Request.Cookies["Senha"].Value;

            int contador = 0;

            try
            {
                Funcionarios funcionario = new Funcionarios();
                funcionario = db.Funcionarios.Where(i => i.CHAPA.Contains(chapa)).ToList().First();

                // Open connection with AD domain
                OpenAdConnection();

                // Create the object "search"
                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                // Add filter
                search.Filter = "(description=*" + chapa + "*)";
                search.SearchScope = SearchScope.Subtree;
                search.Asynchronous = true;

                SearchResult searchResult = search.FindOne();

                if (searchResult != null)
                {
                    DirectoryEntry user = new DirectoryEntry(searchResult.Path, usuariologado, senhalogado);

                    string displayname = removerAcentos(user.Properties["displayName"].Value.ToString().ToUpper());

                    if (string.Equals(displayname, removerAcentos(funcionario.NOME.ToUpper())))
                    {
                        ViewBag.Nome = "Nome está certo";
                        contador++;
                    }
                    else
                    {
                        ViewBag.Nome = "Nome está Errado";
                    }
                    if (string.Equals(user.Properties["title"].Value.ToString().ToUpper(), funcionario.CARGO.ToUpper()))
                    {
                        ViewBag.Cargo = "Cargo está certo";
                        contador++;
                    }
                    else
                    {
                        ViewBag.Cargo = "Cargo está Errado";
                    }
                    if (string.Equals(user.Properties["department"].Value.ToString().ToUpper(), funcionario.SECAO.ToUpper()))
                    {
                        ViewBag.Departamento = "Departamento está certo";
                        contador++;
                    }
                    else
                    {
                        ViewBag.Departamento = "Departamento está Errado";
                    }
                    if (string.Equals(user.Properties["info"].Value.ToString().ToUpper(), funcionario.CPF.ToUpper()))
                    {
                        ViewBag.CPF = "CPF está certo";
                        contador++;
                    }
                    else
                    {
                        ViewBag.CPF = "CPF está Errado";
                    }
                    if (string.Equals(user.Properties["physicalDeliveryOfficeName"].Value.ToString().Substring(6).ToUpper(), funcionario.FILIAL.Substring(4).ToUpper()))
                    {
                        ViewBag.Unidade = "Unidade está certo";
                        contador++;
                    }
                    else
                    {
                        ViewBag.Unidade = "Unidade está Errado";
                    }
                }
                if (contador == 5)
                {
                    ViewBag.Result = "Certo";
                }
                else
                {
                    ViewBag.Result = "Errado";
                }

            }
            catch
            {
                ViewBag.Result = "FuncDesligado";
            }                                          
                    
            return View("ResultadoComparacao", new User { Chapa = chapa });
        }

        //Corrige os Dados do usuário no AD
        public ActionResult Corrigir(string chapa)
        {
            string usuariologado = Request.Cookies["Username"].Value;
            string senhalogado = Request.Cookies["Senha"].Value;
            

            Funcionarios funcionario = new Funcionarios();
            funcionario = db.Funcionarios.Where(i => i.CHAPA.Contains(chapa)).ToList().First();

            // Open connection with AD domain
            OpenAdConnection();

            // Create the object "search"
            DirectorySearcher search = new DirectorySearcher(ldapConnection);

            // Add filter
            search.Filter = "(description=*" + chapa + "*)";
            search.SearchScope = SearchScope.Subtree;
            search.Asynchronous = true;

            SearchResult searchResult = search.FindOne();

            if (searchResult != null)
            {
                DirectoryEntry user = new DirectoryEntry(searchResult.Path, usuariologado, senhalogado);

                string displayname = removerAcentos(user.Properties["displayName"].Value.ToString().ToUpper());

                if (string.Equals(displayname, removerAcentos(funcionario.NOME.ToUpper())))
                {
                    ViewBag.Nome = "Nada a Fazer";
                }
                else
                {
                    user.Properties["displayName"].Value = funcionario.NOME;
                    user.Rename("CN=" + funcionario.NOME);
                }
                if (string.Equals(user.Properties["title"].Value.ToString().ToUpper(), funcionario.CARGO.ToUpper()))
                {
                    ViewBag.Cargo = "Nada a Fazer";
                }
                else
                {
                    user.Properties["title"].Value = funcionario.CARGO;
                }
                if (string.Equals(user.Properties["department"].Value.ToString().ToUpper(), funcionario.SECAO.ToUpper()))
                {
                    ViewBag.Departamento = "Nada a Fazer";
                }
                else
                {
                    user.Properties["department"].Value = funcionario.SECAO;
                    //DirectoryEntry departamentonovo = new DirectoryEntry("LDAP://" + Context.ServerIp + "/OU=Users,OU=" + funcionario.SECAO + ",OU=Departamentos,OU=" 
                    //    + removerAcentos(user.Properties["physicalDeliveryOfficeName"].Value.ToString().Substring(6)) +
                    //    ",OU=Unidades" + ",OU=" + removerAcentos(user.Properties["l"].Value.ToString()) + ",OU=Cidades,DC=coc,DC=com,DC=br", usuariologado, senhalogado);
                    //user.MoveTo(departamentonovo);
                }
                if (string.Equals(user.Properties["info"].Value.ToString().ToUpper(), funcionario.CPF.ToUpper()))
                {
                    ViewBag.CPF = "Nada a Fazer";
                }
                else
                {
                    user.Properties["info"].Value = funcionario.CPF;
                }
                if (string.Equals(user.Properties["physicalDeliveryOfficeName"].Value.ToString().Substring(6).ToUpper(), funcionario.FILIAL.Substring(4).ToUpper()))
                {
                    ViewBag.Unidade = "Nada a Fazer";
                }
                else
                {
                    user.Properties["physicalDeliveryOfficeName"].Value = "SEB - " + funcionario.FILIAL.Substring(4);                    
                }
                user.CommitChanges();
            }

            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResultadoComparacao()
        {

            return View();
        }

        public ActionResult Sucesso()
        {
            return View();
        }

        public ActionResult Erro()
        {
            return View();
        }

        //Remover Acentuação
        public static string removerAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }

        //------------------------------- Fim Funções Controller ------------------------------------------//




        //------------------------------- Funções Repository ------------------------------------------//
        //Ativar Usuário
        public void HabilitarUsuario(string usuario)
        {
            try
            {
                // Open connection with AD domain
                OpenAdConnection();

                // Create the object "search"
                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                // Add filter
                search.Filter = string.Format("(anr={0})", usuario);
                search.SearchScope = SearchScope.Subtree;
                search.Asynchronous = true;

                SearchResult searchResult = search.FindOne();
                DirectoryEntry userEntry = searchResult.GetDirectoryEntry();

                int old_UAC = (int)userEntry.Properties["userAccountControl"][0];

                // AD user account disable flag
                int ADS_UF_ACCOUNTDISABLE = 2;

                // To enable an ad user account, we need to clear the disable bit/flag:
                userEntry.Properties["userAccountControl"][0] = (old_UAC & ~ADS_UF_ACCOUNTDISABLE);
                userEntry.CommitChanges();

                userEntry.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //Desativar Usuário
        public ActionResult DesativarUserDesligado(string chapa)
        {
            string usuariologado = Request.Cookies["Username"].Value;
            string senhalogado = Request.Cookies["Senha"].Value;
            try
            {
                // Open connection with AD domain
                OpenAdConnection();

                // Create the object "search"
                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                // Add filter
                search.Filter = "(description=*" + chapa + "*)";
                search.SearchScope = SearchScope.Subtree;
                search.Asynchronous = true;

                SearchResult searchResult = search.FindOne();
                DirectoryEntry userEntry = new DirectoryEntry(searchResult.Path, usuariologado, senhalogado);

                userEntry.Properties["userAccountControl"].Value = 0x202;
                userEntry.CommitChanges();

                userEntry.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Json(JsonRequestBehavior.AllowGet);
        }

        //Habilitar Usuário
        public ActionResult HabilitarUser(string chapa)
        {
            string usuariologado = Request.Cookies["Username"].Value;
            string senhalogado = Request.Cookies["Senha"].Value;
            try
            {
                // Open connection with AD domain
                OpenAdConnection();

                // Create the object "search"
                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                // Add filter
                search.Filter = "(description=*" + chapa + "*)";
                search.SearchScope = SearchScope.Subtree;
                search.Asynchronous = true;

                SearchResult searchResult = search.FindOne();
                DirectoryEntry userEntry = new DirectoryEntry(searchResult.Path, usuariologado, senhalogado);

                int old_UAC = (int)userEntry.Properties["userAccountControl"][0];

                // AD user account disable flag
                int ADS_UF_ACCOUNTDISABLE = 2;

                // To enable an ad user account, we need to clear the disable bit/flag:
                userEntry.Properties["userAccountControl"][0] = (old_UAC & ~ADS_UF_ACCOUNTDISABLE);
                userEntry.CommitChanges();

                userEntry.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}