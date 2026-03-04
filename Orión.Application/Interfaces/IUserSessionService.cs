using Orión.Domain.Entities;

namespace Orión.Application.Interfaces;

public interface IUserSessionService
{
    Usuario? CurrentUser { get; set; }
    bool IsAdmin { get; }
}
