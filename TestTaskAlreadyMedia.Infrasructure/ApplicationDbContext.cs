using Microsoft.EntityFrameworkCore;
using TestTaskAlreadyMedia.Infrasructure.Models;

namespace TestTaskAlreadyMedia.Infrasructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() { }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public virtual DbSet<NasaObject> NasaObjects { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
