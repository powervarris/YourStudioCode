using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YourStudioFinal.Models;

namespace YourStudioFinal.data;

public class MyLog
{
    public int Id { get; set; }
    public string Message { get; set; }
    // Add other properties as needed
}

public class ApplicationDbContext : IdentityDbContext<User>
{
    // public DbSet<Bookings> Bookings { get; set; }
    // public DbSet<MyLog> Logs { get; set; }
    //
    // public DbSet<UnverifiedBooking> UnverifiedBookings { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.Inquiries).WithOne(e => e.accountUser).HasForeignKey(e=>e.Id);
            entity.Property(e => e.password).IsRequired();
            entity.Property(e => e.mobileNumber).IsRequired();
            entity.Property(e => e.dateCreated).IsRequired();
        });
            
        modelBuilder.Entity<Inquiry>(entity =>
        {
            entity.HasKey(e => e.Id); //primary key
            entity.HasOne(e => e.accountUser).WithMany(e => e.Inquiries).HasForeignKey(e => e.userID);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Message).IsRequired();
        });

        // Add entity configurations here
        modelBuilder.Entity<MyLog>(entity =>
        {
            // Configuration for MyLog entity
            entity.Property(e => e.Message).IsRequired();
            // Add other configurations as needed
        });
    }

}