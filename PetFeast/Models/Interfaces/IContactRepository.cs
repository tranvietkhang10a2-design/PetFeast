namespace PetFeast.Models.Interfaces
{
    public interface IContactRepository
    {
        void SendContact(Contacts.Contact contact);
    }
}
