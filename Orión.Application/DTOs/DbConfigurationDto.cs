namespace Orión.Application.DTOs;

public class DbConfigurationDto
{
    public string Provider { get; set; } = "PostgreSQL"; // PostgreSQL | Access
    public bool IsProduction { get; set; } = false;
    
    // Access
    public string? AccessFilePath { get; set; }
    
    // PostgreSQL
    public string? Host { get; set; }
    public int Port { get; set; } = 5432;
    public string? DatabaseName { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public bool SslMode { get; set; } = false;

    public string GetConnectionString()
    {
        if (Provider.Equals("Access", StringComparison.OrdinalIgnoreCase))
        {
            return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={AccessFilePath}";
        }
        
        var ssl = SslMode ? "Require" : "Disable";
        return $"Host={Host};Port={Port};Database={DatabaseName};Username={Username};Password={Password};Trust Server Certificate=true;SSL Mode={ssl}";
    }
}
