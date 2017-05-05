﻿using SISSDV1.Models;
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
        //------------------------------- Funções AD ------------------------------------------//

        //Variáveis de conexão
        private DirectoryEntry ldapConnection;
        public string server = "LDAP://" + Context.ServerIp;
        private string username = "sissd.aplicacao";
        private string password = "Pass@2017";

        //Abrir conexão
        void OpenAdConnection()
        {
            ldapConnection = new DirectoryEntry(server, username, password);
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
                Context.ServerIp = "";
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
            return PartialView("Resultado", list.OrderBy(u => u.NomeExibicao));
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
            search.PageSize = 6000;
            search.SearchScope = SearchScope.Subtree;
            SearchResultCollection resultCol = search.FindAll();

            // Checa se achou algo
            if (resultCol != null)
            {
                foreach (SearchResult r in resultCol)
                {
                    if (r.Properties["userPrincipalName"][0] != null)
                    {
                        User user = new User();


                        try
                        {
                            user.objectSid = r.Properties["objectSid"][0].ToString();
                        }
                        catch
                        {
                            user.objectSid = "fuuuuuuuu";
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
                            if (status == 0x0200)
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
            }
            CloseAdConnection();
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
                search.PageSize = 50;
                search.SizeLimit = 50;
                search.Filter = "(sAMAccountType=805306368)";
                search.Filter = "(&(sAMAccountType=805306368)(|(displayName=*" + usuarios+"*)"
                    +"(userPrincipalName=*"+ usuarios+ "*)(title=*" + usuarios + "*)))";
                search.SearchScope = SearchScope.Subtree;
                SearchResultCollection resultCol = search.FindAll();

                // Checa se achou algo
                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        User user = new User();
                        user.Email = r.Properties["userPrincipalName"][0].ToString();
                        user.NomeExibicao = r.Properties["cn"][0].ToString();
                        try
                        {
                            user.Cargo = r.Properties["title"][0].ToString();
                        }
                        catch
                        {

                            user.Cargo = "Sem Cargo";
                        }
                        int status = (int)r.Properties["userAccountControl"][0];
                        if (status == 0x0200)
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
            var diassemana = new List<string> { "Domingo" ,"Segunda","Terça", "Quarta", "Quinta", "Sexta", "Sábado" };
            ViewBag.DiaInicio = new SelectList(diassemana, "Segunda");
            ViewBag.DiaFim = new SelectList(diassemana, "Sexta");
            var horas = new List<string> { "01:00", "02:00", "03:00", "04:00", "05:00",
                "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00",
                "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00",
                "20:00", "21:00", "22:00", "23:00", "00:00" };
            ViewBag.HorarioInicio = new SelectList(horas, "08:00");
            ViewBag.HorarioFim = new SelectList(horas, "18:00");
            return View();
        }

        //Novo Usuário
        [HttpPost]
        public ActionResult CriarUsuario(string nome, string sobrenome, string diainicio, string diafim, string horarioinicio, string horariofim, string username, string cargo, string cpf, string telefone,
            int chapa, string senha, string cidade, string unidade, string departamento, string subdepartamento)
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
            foreach (GrupoAD grupo in grupos)
            {                
                DirectoryEntry group = new DirectoryEntry("LDAP://"+Context.ServerIp+"/"+grupo.GrupoNome, username, password);
                group.Properties["member"].Add(user.Properties["distinguishedName"].Value);
                group.CommitChanges();
                CloseAdConnection(group);             
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
                        resultado.Email = r.Properties["mail"][0].ToString();
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
                CloseAdConnection(entry);
                return ouList;
            }
            catch
            {
                CloseAdConnection();
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

            try
            {
                //Dados Conexão com AD
                Context.ServerIp = "10.0.210.8";
                string username = "sissd.aplicacao@coc.com.br";
                string password = "Pass@2017";
                DirectoryEntry entry = new DirectoryEntry("LDAP://" +
                    Context.ServerIp + "/OU=Unidades,OU=" + cidade + ",OU=Cidades,DC=coc,DC=com,DC=br", username, password);

                DirectorySearcher search = new DirectorySearcher(entry);

                //Filtro de Pesquisa                
                search.Filter = "(objectClass=organizationalUnit)";
                search.SearchScope = SearchScope.OneLevel;
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
                CloseAdConnection(entry);
                return ouList;
            }
            catch
            {
                CloseAdConnection();
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
                CloseAdConnection();
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
                CloseAdConnection();
                return resultado;
            }
            catch
            {
                CloseAdConnection();
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
                CloseAdConnection();
                return resultado;
            }
        }


        //Retorna o Json Para Preencher o Gerente
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
                SearchResultCollection resultCol = search.FindAll();

                // Checa se achou algo
                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        resultado.NomeExibicao = r.Properties["displayName"][0].ToString();
                        resultado.Email = r.Properties["mail"][0].ToString();
                        resultado.Cargo = r.Properties["title"][0].ToString();
                        resultado.Username = r.Properties["sAMAccountName"][0].ToString();
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
                        string gerentesearch = resultado.Gerente;
                        User gerente = ProcurarGerente(gerentesearch);
                        resultado.Gerente = gerente.NomeExibicao;
                    }
                }
                return resultado;
            }
            catch
            {
                CloseAdConnection();
                return resultado;
            }
        }

        public ActionResult ResetarSenha()
        {
            return View();
        }

        public ActionResult ResetarSenha(string username)
        {
            return View(new User { Username = username });
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
                search.Filter = "(mail=*"+username+"*)";
                search.SearchScope = SearchScope.Subtree;

                SearchResult searchResult = search.FindOne();
                DirectoryEntry user = searchResult.GetDirectoryEntry();

                user.Invoke("SetPassword", password);
                user.CommitChanges();


                CloseAdConnection();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult Editar(User user)
        {
            var cidades = new List<OrganizationalUnit>();
            ViewBag.Cidade = new SelectList(cidades, "Cidade", "Cidade");
            var unidades = new List<OrganizationalUnit>();
            ViewBag.Unidade = new SelectList(unidades, "Unidade", "Unidade");
            var departamentos = new List<OrganizationalUnit>();
            ViewBag.Departamento = new SelectList(departamentos, "Departamento", "Departamento");

            //Combobox Horario de Logon
            var diassemana = new List<string> { "Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado" };
            ViewBag.DiaInicio = new SelectList(diassemana, "Segunda");
            ViewBag.DiaFim = new SelectList(diassemana, "Sexta");
            var horas = new List<string> { "01:00", "02:00", "03:00", "04:00", "05:00",
                "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00",
                "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00",
                "20:00", "21:00", "22:00", "23:00", "00:00" };
            ViewBag.HorarioInicio = new SelectList(horas, "08:00");
            ViewBag.HorarioFim = new SelectList(horas, "18:00");
            return View(user);
        }

        public ActionResult ComparaUserADcomUserRM()
        {
            return View();
        }

        public ActionResult Deletar()
        {
            return View();
        }

        public ActionResult DetalhesUsuario(string email)
        {
            var buscado = new User();
            buscado = Detalhes(email);

            return View(buscado);
        }

        public ActionResult Verificar()
        {
            return View();
        }

        public ActionResult Sucesso()
        {
            return PartialView();
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

                SearchResult searchResult = search.FindOne();
                DirectoryEntry userEntry = searchResult.GetDirectoryEntry();

                int old_UAC = (int)userEntry.Properties["userAccountControl"][0];

                // AD user account disable flag
                int ADS_UF_ACCOUNTDISABLE = 2;

                // To enable an ad user account, we need to clear the disable bit/flag:
                userEntry.Properties["userAccountControl"][0] = (old_UAC & ~ADS_UF_ACCOUNTDISABLE);
                userEntry.CommitChanges();

                CloseAdConnection();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }     
    }
}