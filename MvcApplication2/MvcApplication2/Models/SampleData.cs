using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MvcApplication2.Models
{
    public class SampleData : DropCreateDatabaseIfModelChanges<AuthEntities>
    {
        protected override void Seed(AuthEntities context)
        {
            base.Seed(context);
            var users = new List<PasswordAuthModel>
            {
                new PasswordAuthModel { UserId = 1, UserName = "azerty", Password = "azerty" },
                new PasswordAuthModel  { UserId = 2, UserName = "qwerty", Password = "qwerty" }
            };
            users.ForEach(a => context.PasswordAuth.Add(a));
        }
    }
}