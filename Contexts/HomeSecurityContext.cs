using Microsoft.EntityFrameworkCore;
using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Entities.Chats;

namespace Home_Security.Context;

public class HomeSecurityContext: DbContext
{
    public HomeSecurityContext(DbContextOptions<HomeSecurityContext> optionsBuilder): base(optionsBuilder)
    {
    }
    //User
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    //Facilities
    public DbSet<Section> Section { get; set; }
    public DbSet<Room> Room { get; set; }
    public DbSet<Door> Door { get; set; }
    public DbSet<Window> Window { get; set; }
    public DbSet<Light> Light { get; set; }
    public DbSet<Camera> Camera { get; set; }
    public DbSet<Appliance> Appliance { get; set; }
    public DbSet<External> External { get; set; }


    //Operations
    public DbSet<Entities.Action> Action { get; set; }
    public DbSet<Logs> Logs { get; set; }
    public DbSet<Recording> Recording { get; set; }

    //Hardware
    public DbSet<Hardware> Hardware { get; set; }

    //Person
    public DbSet<Person> Person { get; set; }
    public DbSet<PersonDetails> PersonDetails { get; set; }
    public DbSet<FingerPrint> FingerPrint { get; set; }

    //Contact Profiles
    public DbSet<Address> Address { get; set; }
    public DbSet<Contact> Contact { get; set; }
    public DbSet<ContactCategory> ContactCategory { get; set; }
    public DbSet<ContactDetails> ContactDetails { get; set; }

    //Token
    public DbSet<Token> Tokens { get; set; }

    //Chats
    public DbSet<Chat> Chat { get; set; }
    public DbSet<ChatParticipant> ChatParticipant { get; set; }
    public DbSet<Message> Message { get; set; }
    public DbSet<MessageAttachment> MessageAttachment { get; set; }
}
