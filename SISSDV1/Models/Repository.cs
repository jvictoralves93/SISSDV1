using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using ActiveDs;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SISSDV1.Models
{
    public class Repository
    {
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
            if (Context.ServerIp == "10.0.210.9")
                Context.ServerIp = "";
            else
                Context.ServerIp = "10.0.210.9";
        }

        void CloseAdConnection(DirectoryEntry connection)
        {
            connection.Close();
        }

        public bool Login(string username, string password)
        {        
            string container = @"OU=Infraestrutura,OU=TI,OU=Departamentos,OU=CSC,OU=Unidades,OU=Ribeirao Preto,OU=Cidades,DC=coc,DC=com,DC=br";

            try
            {
                using (PrincipalContext connection = new PrincipalContext(ContextType.Domain, Context.ServerIp, container, this.username, this.password))
                {
                    if (connection.ValidateCredentials(username, password))
                    {
                        UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(connection, username);
                        if (userPrincipal != null)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Logout()
        {
            User user = new User();
            user = null;
        }

        static void SetDontExpirePassword(DirectoryEntry User)
        {
            try
            {

                int val;
                const int ADS_UF_DONT_EXPIRE_PASSWD = 0x10000;
                val = (int)User.Properties["userAccountControl"].Value;
                User.Properties["userAccountControl"].Value = val |
                    ADS_UF_DONT_EXPIRE_PASSWD;
                User.CommitChanges();
            }
            catch { }
        }

        static void SetEnableUserAccount(DirectoryEntry User)
        {
            try
            {
                int val = (int)User.Properties["userAccountConrol"].Value;
                User.Properties["userAccountControl"].Value = val & ~(int)ActiveDs.ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE;
                User.CommitChanges();
            }
            catch { }
        }

        public void VerifyIsOk(DirectoryEntry user)
        {
            //group, username with domain, password never expires
            AddMemberToCfsGroup(user);

            string usuario = user.Properties["userPrincipalName"][0].ToString();
            string usuarioAcountName = user.Properties["samAccountname"][0].ToString();

            if (usuario != string.Format("{0}@sebsa.com.br", usuarioAcountName))
            {
                user.Properties["userPrincipalName"].Value = string.Format("{0}@sebsa.com.br", user.Properties["samAccountname"].Value);
                user.CommitChanges();
            }

            SetDontExpirePassword(user);

            user.Close();
        }

        public List<OrganizationalUnit> GetOuList()
        {
            List<OrganizationalUnit> ouList = new List<OrganizationalUnit>();

            try
            {
                OpenAdConnection();
                DirectorySearcher search = new DirectorySearcher(ldapConnection);
                search.Filter = "(&(ou=*)(objectClass=organizationalUnit))";
                search.PropertiesToLoad.Add("ou");
                search.PropertiesToLoad.Add("name");
                search.PropertiesToLoad.Add("description");
                SearchResultCollection resultCol = search.FindAll();

                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        if (r.Properties.Contains("ou") && r.Properties.Contains("name") && r.Properties.Contains("description"))
                        {
                            OrganizationalUnit ou = new OrganizationalUnit();
                            ou.Ou = r.Properties["ou"][0].ToString();
                            ou.Name = r.Properties["name"][0].ToString();
                            ou.Description = r.Properties["description"][0].ToString();
                            ouList.Add(ou);
                        }
                    }
                }
                return ouList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public DirectoryEntry SearchOneUser(string username)
        {
            try
            {
                // Open connection with AD domain
                OpenAdConnection();

                // Create the object "search"
                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                // Add filter
                search.Filter = string.Format("(anr={0})", username);
                search.SearchScope = SearchScope.Subtree;

                SearchResult searchResult = search.FindOne();
                DirectoryEntry userEntry = searchResult.GetDirectoryEntry();
                return userEntry;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddLogedMonitor(string username)
        {
            try
            {
                DirectoryEntry userEntry = SearchOneUser(username);

                User user = new User();

                user.Username = userEntry.Properties["samAccountName"][0].ToString();

                try
                {
                    user.NomeExibicao = userEntry.Properties["givenName"][0].ToString() + " " + userEntry.Properties["sn"][0].ToString();
                }
                catch
                {
                    try
                    {
                        user.NomeExibicao = userEntry.Properties["description"][0].ToString();
                    }
                    catch
                    {
                        try
                        {
                            user.NomeExibicao = userEntry.Properties["givenName"][0].ToString();
                        }
                        catch
                        {
                            try
                            {
                                user.NomeExibicao = userEntry.Properties["sn"][0].ToString();
                            }
                            catch
                            {
                                user.NomeExibicao = string.Empty;
                            }
                        }
                    }
                }

                Context.UserLoged = user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<User> SearchUser(string usuarios)
        {
            List<User> searchResult = new List<User>();
            try
            {
                // Open connection with AD domain
                OpenAdConnection();

                // Create the object "search"
                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                // Add filter
                search.Filter = string.Format("(anr={0})", usuarios);
                search.SearchScope = SearchScope.Subtree;
                SearchResultCollection resultCol = search.FindAll();

                // Check the results in resulCol and add they at searchResult
                if (resultCol != null)
                {
                    foreach (SearchResult r in resultCol)
                    {
                        User user = new User();
                        user.Username = r.Properties["samaccountname"][0].ToString();
                        try
                        {
                            user.NomeExibicao = r.Properties["givenName"][0].ToString() + " " + r.Properties["sn"][0].ToString();
                        }
                        catch
                        {
                            try
                            {
                                user.NomeExibicao = r.Properties["description"][0].ToString();
                            }
                            catch
                            {
                                try
                                {
                                    user.NomeExibicao = r.Properties["givenName"][0].ToString();
                                }
                                catch
                                {
                                    try
                                    {
                                        user.NomeExibicao = r.Properties["sn"][0].ToString();
                                    }
                                    catch
                                    {
                                        user.NomeExibicao = string.Empty;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            searchResult.Add(user);
                        }
                    }
                }
                CloseAdConnection();
                return searchResult;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void EnableADUserUsingUserAccountControl(string username)
        {
            try
            {
                // Open connection with AD domain
                OpenAdConnection();

                // Create the object "search"
                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                // Add filter
                search.Filter = string.Format("(anr={0})", username);
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

        public void ResetUserAdPassword(string username, string password)
        {
            try
            {
                // Open connection with AD domain
                OpenAdConnection();

                // Create the object "search"
                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                // Add filter
                search.Filter = string.Format("(anr={0})", username);
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
        }

        public void AddMemberToCfsGroup(DirectoryEntry user)
        {
            try
            {
                DirectoryEntry connection = OpenAdConnection("LDAP://" + Context.ServerIp + "/OU=All Groups,DC=rpo-acad,DC=unaerpnet,DC=br");
                DirectoryEntry group = connection.Children.Find("CN=CFS_RPO_ACAD-Alunos");

                group.Properties["member"].Add(user.Properties["distinguishedName"].Value);

                //TODO check, why isn't working
                group.CommitChanges();

                CloseAdConnection(connection);
            }
            catch { }
        }

        public void CreateUserAd(string code, string name, string lastName, string course, string password)
        {
            try
            {
                DirectoryEntry connectionOu = OpenAdConnection("LDAP://" + Context.ServerIp + "/OU=Students,OU=All Users,DC=rpo-acad,DC=unaerpnet,DC=br");
                DirectoryEntry ou = connectionOu.Children.Find(string.Format("OU={0}", course));

                string NomeExibicao = name + " " + lastName;

                DirectoryEntry user = ou.Children.Add(string.Format("CN={0}", code), "user");

                //Add attributes to the nem user
                user.Properties["samAccountName"].Value = code;
                user.Properties["sn"].Value = lastName;
                user.Properties["displayName"].Value = NomeExibicao;
                user.Properties["givenName"].Value = name;
                user.Properties["description"].Value = NomeExibicao;
                user.Properties["userPrincipalName"].Value = String.Format("{0}@rpo-acad.unaerpnet.br", code);
                user.Properties["name"].Value = NomeExibicao;

                //Save in AD the new user
                user.CommitChanges();

                //Set the password to the new user and save it in AD
                user.Invoke("SetPassword", password);
                user.CommitChanges();


                //TODO check both method that are getting problem

                //Set password to never expire
                SetDontExpirePassword(user);

                user.CommitChanges();

                CloseAdConnection(connectionOu);

                //Enable the new user account
                EnableADUserUsingUserAccountControl(code);

                AddMemberToCfsGroup(user);
                user.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public void RemoveUserAd(string code)
        {
            DirectoryEntry user = SearchOneUser(code);
            DirectoryEntry connection = OpenAdConnection(user.Parent.Path);
            try
            {
                connection.Children.Remove(user);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            CloseAdConnection(connection);
        }

    }
}
