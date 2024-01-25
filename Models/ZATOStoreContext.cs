using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ZATO.Models;

public partial class ZATOStoreContext : DbContext
{
    public ZATOStoreContext()
    {
    }
    
    public ZATOStoreContext(DbContextOptions<ZATOStoreContext> options)
        : base(options)
    {
        
    }
    public virtual DbSet<User> Users { get; set; } = null!;
    
    public virtual DbSet<Restaurants> Restaurants { get; set; } = null!;
    
    public virtual DbSet<Orders> Orders { get; set; } = null!;
    public virtual DbSet<MenuItems> MenuItems { get; set; } = null!;
    public virtual DbSet<OrderItems> OrderItems { get; set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=LAPTOP-F58JUSGE;Initial Catalog=ZATO;Integrated Security=True;Encrypt=False");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Restaurants>().ToTable("Restaurants");
        modelBuilder.Entity<Orders>().ToTable("Orders");
        modelBuilder.Entity<OrderItems>().ToTable("OrderItems");
        modelBuilder.Entity<DeliveryPersonnel>().ToTable("DeliveryPersonnel");
        modelBuilder.Entity<DeliveryAgents>().ToTable("DeliveryAgents");
        modelBuilder.Entity<DeliveryAssignments>().ToTable("DeliveryAssignments");
        modelBuilder.Entity<User>()
            .Property(x => x.Id).HasDefaultValueSql("NEWID()");
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}