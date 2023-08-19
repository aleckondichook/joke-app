using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JokeApp.Models;

namespace JokeApp.Data
{
  public class ApplicationDbContext : IdentityDbContext
  {
    public DbSet<Joke> Jokes { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
    {
    }
  }
}