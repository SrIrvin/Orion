namespace Orión.Domain.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string username) 
        : base($"El nombre de usuario '{username}' ya está en uso.")
    {
    }
}
