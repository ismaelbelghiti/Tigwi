
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class PasswordAuthModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool CheckPassword(string pass) {
                return Password == pass;
        }

        public PasswordAuthModel() { }
    }
}