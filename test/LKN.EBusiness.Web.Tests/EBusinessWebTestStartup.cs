using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace LKN.EBusiness;

public class EBusinessWebTestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<EBusinessWebTestModule>();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
