using Orión.Application.DTOs;

namespace Orión.Application.Interfaces;

public interface ISecureConfigService
{
    DbConfigurationDto LoadConfig();
    void SaveConfig(DbConfigurationDto config);
    void ClearSession();
    Task<bool> TestConnection(DbConfigurationDto config);
}
