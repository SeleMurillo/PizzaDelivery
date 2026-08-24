using System;

namespace PizzaDelivery.Shared.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException() : base("No está autenticado. Por favor inicie sesión.")
    {
    }

    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
