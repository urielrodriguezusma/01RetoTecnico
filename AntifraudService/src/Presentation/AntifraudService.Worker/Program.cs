using AntifraudService.Worker.Configuration;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.ConfigureServices(hostContext.Configuration);
    });

var host = builder.Build();
host.Run();
