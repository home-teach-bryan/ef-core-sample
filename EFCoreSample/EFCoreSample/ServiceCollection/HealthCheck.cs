﻿using EFCoreSample.HealthCheck;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EFCoreSample.ServiceCollection;

public static class HealthCheck
{
    public static IServiceCollection AddCustomHealthCheck(this IServiceCollection service)
    {
        service.AddHealthChecks().AddCheck("DataBaseHealthCheck",
            new DataBaseHealthCheck("ConnectionString"),
            HealthStatus.Unhealthy,
            new string[] { "DataBaseHealthCheck" });
        return service;
    }
}