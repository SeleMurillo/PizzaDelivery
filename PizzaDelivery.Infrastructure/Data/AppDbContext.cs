using Microsoft.EntityFrameworkCore;
using PizzaDelivery.Application.Common.Interfaces;
using PizzaDelivery.Domain.Entities;
namespace PizzaDelivery.Infrastructure.Data;

public partial class AppDbContext : DbContext, IApplicationDbContext
{
    public AppDbContext()
    {
    }
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<Pizza> Pizzas => Set<Pizza>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Ingrediente> Ingredientes => Set<Ingrediente>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);

        base.OnModelCreating(modelBuilder);
        // Configurar Value Objects como propiedades
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.OwnsOne(p => p.DireccionEntrega, direccion =>
            {
                direccion.Property(d => d.Calle).HasColumnName("Calle");
                direccion.Property(d => d.Numero).HasColumnName("Numero");
                direccion.Property(d => d.Colonia).HasColumnName("Colonia");
                direccion.Property(d => d.Ciudad).HasColumnName("Ciudad");
            });
            
            entity.Property(p => p.Total)
                .HasPrecision(18, 2);
        });
        
        modelBuilder.Entity<Pizza>().HasData(
            new Pizza { Id = new Guid(), Nombre = "Margarita", Precio = 80 },
            new Pizza { Id = new Guid(), Nombre = "Pepperoni", Precio = 90 },
            new Pizza { Id = new Guid(), Nombre = "Hawaiana", Precio = 95 }
        );
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}