using Microsoft.EntityFrameworkCore;

namespace MvcMovie.Models
{
    public class FirstDotNetWebAppContext : DbContext
    {
        public FirstDotNetWebAppContext (DbContextOptions<FirstDotNetWebAppContext> options)
            : base(options)
        {
        }

        public DbSet<MvcMovie.Models.Movie> Movie { get; set; }
    }
}