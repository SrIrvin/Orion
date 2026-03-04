using Orión.Application.Interfaces;
using Orión.Domain.Entities;

namespace Orión.Application.Services;

public class UserSessionService : IUserSessionService
{
    public Usuario? CurrentUser { get; set; }

    public bool IsAdmin => CurrentUser?.Rol == "Admin";
}
