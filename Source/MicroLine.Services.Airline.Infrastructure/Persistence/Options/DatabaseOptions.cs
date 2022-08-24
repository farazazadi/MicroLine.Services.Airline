using Microsoft.Extensions.Configuration;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Options;

internal class DatabaseOptions
{
    public readonly string SectionName = "DatabaseOptions";

    private readonly IConfiguration _configuration;
    public string ConnectionString => _configuration.GetConnectionString("DefaultConnection");

    public int CommandTimeout { get; set; }
    public int MaxRetryCount { get; set; }
    public bool EnableDetailedErrors { get; set; }
    public bool EnableSensitiveDataLogging { get; set; }


    public DatabaseOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}