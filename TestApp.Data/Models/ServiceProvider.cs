namespace VerizonReports.Repository.Models;

public class ServiceProvider : Logic.Interfaces.IServiceProvider
{
    public int ServiceProviderId { get; set; }
    public int ProviderName { get; set; }
}