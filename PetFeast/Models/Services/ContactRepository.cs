using PetFeast.Models.Contacts;
using PetFeast.Data;
using PetFeast.Models.Interfaces;

namespace PetFeast.Models.Services
{
    public class ContactRepository : IContactRepository
    {
        private readonly PetFeastDBContext dbContext;
        public ContactRepository(PetFeastDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void SendContact(Contact contact)
        {
            dbContext.Contacts.Add(contact);
            dbContext.SaveChanges();
        }
    }
}
