using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YourStudioFinal.Models;
using YourStudioFinal.Models.Booking;
using YourStudioFinal.Models.Gallery;

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
    public DbSet<Gallery> Gallery { get; set; }
    public DbSet<GalleryFile> GalleryFiles { get; set; }
    
    public DbSet<BookingModel> Booking { get; set; }
    public DbSet<PaymentModel> Payment { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<PaymentModel>(entity =>
        {
            entity.HasKey(e => e.Id); //primary key
        });
        
        modelBuilder.Entity<BookingModel>(entity =>
        {
            entity.HasKey(e => e.Id); //primary key
            entity.HasOne(e => e.accountUser).WithMany(e => e.Bookings).HasForeignKey(e => e.userID);
            entity.HasOne(e => e.payment).WithOne(e => e.Booking).HasForeignKey<PaymentModel>(e => e.BookingId);
        });
        
        modelBuilder.Entity<GalleryFile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.fileName).IsRequired();
        });
        
        modelBuilder.Entity<Gallery>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
            
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
        
        // modelBuilder.Entity<BookingModel>(entity =>
        // {
        //     entity.HasKey(e => e.Id); //primary key
        //     entity.HasOne(e => e.accountUser).WithMany(e => e.Bookings).HasForeignKey(e => e.userID);
        //     entity.Property(e => e.slctTime).IsRequired();
        //     entity.Property(e => e.dateCreated).IsRequired();
        //     entity.Property(e => e.myCalencarModel).IsRequired();
        // });

        // Add entity configurations here
        modelBuilder.Entity<MyLog>(entity =>
        {
            // Configuration for MyLog entity
            entity.Property(e => e.Message).IsRequired();
            // Add other configurations as needed
        });
    }

}