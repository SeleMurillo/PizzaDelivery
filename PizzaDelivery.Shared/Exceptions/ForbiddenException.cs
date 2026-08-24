using System;

namespace PizzaDelivery.Shared.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException() : base("No tiene permisos para acceder a este recurso.")
    {
    }

    public ForbiddenException(string message) : base(message)
    {
    }

    public ForbiddenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}