using Microsoft.EntityFrameworkCore;
using LibraryDemo.Models;


namespace WebAPIDemo.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Shirt> Shirts { get; set; }
    }
}
