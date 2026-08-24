using Microsoft.EntityFrameworkCore;

namespace PizzaDelivery.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Domain.Entities.Pedido> Pedidos { get; }
        DbSet<Domain.Entities.Pizza> Pizzas { get; }
        DbSet<Domain.Entities.Cliente> Clientes { get; }
        DbSet<Domain.Entities.Ingrediente> Ingredientes { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
