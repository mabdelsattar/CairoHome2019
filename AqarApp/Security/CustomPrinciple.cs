using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

using AqarApp.Models;

namespace AqarApp.Security
{
    public class CustomPrinciple : IPrincipal
    {
        private Account accout;
        public CustomPrinciple(Account accout)
        {
            this.accout = accout;
            this.Identity = new GenericIdentity(accout.Username);
        }

        public IIdentity Identity
        {
            get;
            set;
        }

        public bool IsInRole(string role)
        {
            var rols = role.Split(new char[] { ',' });
            return rols.Any(r => this.accout.Roles.Contains(role));
        }
    }
}