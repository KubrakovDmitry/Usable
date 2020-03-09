using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENBmodelLibrary
{
    class ContextInitializer : DropCreateDatabaseAlways<ContactContext>
    {
        protected override void Seed(ContactContext db)
        {
            Contact contact = new Contact { Name = "Я", MainTelephone = "89268902386" };
            db.Contacts.Add(contact);
            db.SaveChanges();
            Telephone telephone = new Telephone { Name = contact.Name, telephone = contact.MainTelephone, Contact = contact };
            Telephone telephone2 = new Telephone { Name = contact.Name, telephone = "89163561167", Contact = contact };
            db.Telephones.AddRange(new List<Telephone> { telephone, telephone2 });
            db.Telephones.Add(telephone);
            db.SaveChanges();
            Email email = new Email { Name = contact.Name, email = "kubrakoff.dmitry@yandex.ru", Contact = contact };
            db.Emails.Add(email);
            db.SaveChanges();
            BirthDay birthDay = new BirthDay { Id = contact.Id, Name = contact.Name, Birthday = "13 апреля", Contact = contact };
            db.BirthDays.Add(birthDay);
            db.SaveChanges();
        }
    }
}
