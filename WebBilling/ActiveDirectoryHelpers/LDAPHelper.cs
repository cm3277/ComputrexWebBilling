using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.DirectoryServices.AccountManagement;
using System.Web.Mvc;
using CSGOSideLoungeMVCTest4.DAL;
using WebBilling.Models;

namespace WebBilling.ActiveDirectoryHelpers
{
    public static class LDAPHelper
    {
        public static IHtmlString If(this IHtmlString value, bool evaluation)
        {
            return evaluation ? value : MvcHtmlString.Empty;
        }

        public static bool UserIsMemberOfGroup(string group)
        {
            try
            {
                string[] groups = { group };
                return UserIsMemberOfGroups(HttpContext.Current.User.Identity.Name, groups);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool UserIsMemberOfGroups(string username, string[] groups)
        {
            /* Return true immediately if the authorization is not
            locked down to any particular AD group */
            if (groups == null || groups.Length == 0)
            {
                return true;
            }
            CompXDbContext db = new CompXDbContext();
            TechAccount tech = db.TechAccounts.Find(HttpContext.Current.User.Identity.Name.ToLower());
            if (tech == null)
                return false;
            foreach (var group in groups)
            {
                if (tech.permissionGroup == group)
                    return true;
            }

            return false;
            //OLD FROM ACTIVE DIRECTORY
            // Verify that the user is in the given AD group (if any)x
            
           /* string connString = ConfigurationManager.ConnectionStrings["ADConnectionString"].ConnectionString;
            int startIndex = connString.IndexOf("//") + 2;
            string domain = connString.Substring(startIndex, connString.IndexOf(".", startIndex) - startIndex);
            var context = new PrincipalContext(ContextType.Domain, domain);
            var userPrincipal = UserPrincipal.FindByIdentity(context,
                                                     IdentityType.SamAccountName,
                                                     username);
                
                foreach (var group in groups)
                {
                    if (userPrincipal.IsMemberOf(context, IdentityType.Name, group))
                    {
                        return true;
                    }
                }

            return false;*/
        }
    }
}