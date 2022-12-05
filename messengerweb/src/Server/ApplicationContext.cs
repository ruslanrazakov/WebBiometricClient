using MessengerWeb.Shared;
using Microsoft.EntityFrameworkCore;

namespace MessengerWeb.Server
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
          : base(options)
        {

        }
        public DbSet<Person> Persons { get; set; }
    }
}