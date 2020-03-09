using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LaP
{
    class ContextInitializer : DropCreateDatabaseAlways<LaPContext>
    {
        protected override void Seed(LaPContext db)
        {
            Account account = new Account { Name = "Вконтакте", Login = "q_dd@list", Password = "278314qdd" };

            db.Accounts.Add(account);
            db.SaveChanges();
        }
    }
}
