using System;

namespace PizzaDelivery.Shared.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base("El recurso solicitado no fue encontrado.")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string name, object key)
        : base($"La entidad \"{name}\" ({key}) no fue encontrada.")
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
