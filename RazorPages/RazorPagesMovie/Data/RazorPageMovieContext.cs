#nullable disable
using Microsoft.EntityFrameworkCore;

public class RazorPageMovieContext : DbContext
    {
        public RazorPageMovieContext (DbContextOptions<RazorPageMovieContext> options)
            : base(options)
        {
        }

        public DbSet<RazorPagesMovie.Models.Movie> Movie { get; set; }
    }
