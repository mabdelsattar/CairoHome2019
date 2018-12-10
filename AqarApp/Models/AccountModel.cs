using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AqarApp.Models
{
    public class AccountModel
    {
        AqarDBEntities db;

        private List<Account> listAccounts = new List<Account>();
        public AccountModel() {
            db = new AqarDBEntities();
            var users = db.UserInfoes.ToList();
            listAccounts = users.Select(c => new Account {
                Username = c.Username,
                Password = c.Password,
                Roles = new string[] { c.RoleId.ToString() }
            }).ToList();

            /*listAccounts.Add(new Account()
            {
                Username= "ahmed.mohammed@gmail.com",
                Password= "Pass#123",
                Roles = new string[] { "superadmin","admin","employee" }
            });

            listAccounts.Add(new Account()
            {
                Username = "acc2",
                Password = "12345",
                Roles = new string[] {  "admin", "employee" }
            });

            listAccounts.Add(new Account()
            {
                Username = "acc3",
                Password = "12345",
                Roles = new string[] { "employee" }
            });*/
        }

        public Account find(string username) 
        {
            return listAccounts.Where(a => a.Username.ToLower().Equals(username.ToLower())).FirstOrDefault();
        }
        public Account login(string username,string password)
        {
            return listAccounts.Where(a => a.Username.ToLower().Equals(username.ToLower()) && 
            a.Password.Equals(password)).FirstOrDefault();
        }




    }
}