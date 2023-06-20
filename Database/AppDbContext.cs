using Lesson01.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lesson01.API.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Student { get; set; }
        public DbSet<View_Student> View_Student { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var props = builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties());
            foreach (var property in props.Where(p => p.ClrType == typeof(Guid) || p.ClrType == typeof(Guid?)))
            {
                property.SetColumnType("char(36)");
            }

            foreach (var property in props.Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(14,4)");
            }

            builder.Entity<Student>();
            builder.Entity<View_Student>().ToView(nameof(View_Student));
        }
    }
}
