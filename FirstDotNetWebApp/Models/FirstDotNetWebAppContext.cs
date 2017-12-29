using Microsoft.EntityFrameworkCore;

namespace FirstDotNetWebApp.Models
{
    public class FirstDotNetWebAppContext : DbContext
    {
        public FirstDotNetWebAppContext (DbContextOptions<FirstDotNetWebAppContext> options)
            : base(options)
        {
        }

        public DbSet<FirstDotNetWebApp.Models.Movie> Movie { get; set; }
    }
}