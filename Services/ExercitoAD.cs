using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace WebApp.Services
{
    public class ExercitoAD
    {
        public static ExercitoUserAD GetInfoUserAD(string nim)
        {
            DirectoryEntry entry = new DirectoryEntry("LDAP://exercito.local");
            DirectorySearcher Dsearch = new DirectorySearcher(entry);
            Dsearch.Filter = "(&(objectClass=user)(SAMAccountName=" + nim + "))";
            Dsearch.SearchScope = SearchScope.Subtree;
            SearchResult results = Dsearch.FindOne();

            if(results != null)
            {
                return new ExercitoUserAD
                {
                    email = results.Properties["mail"][0].ToString(),
                    nim = nim
                };
            }
            else
            {
                return null;
            }
        }
    }
}
