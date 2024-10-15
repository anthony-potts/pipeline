// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pipeline;
using Pipeline.Interfaces;
using Pipeline.Pipelines;

Console.WriteLine("=== Pipeline Execution Started ===\n");

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();  // Optional: Clear default logging providers
        logging.AddConsole();      // Add console logger
        logging.SetMinimumLevel(LogLevel.Information);  // Set log level
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<PipelineAuditor>();  // Register auditor as a service
        services.AddSingleton<IPipelineRunner, PeopleParser>();
    })
    .Build();

// Instantiate the PeopleParser
var runner = host.Services.GetRequiredService<IPipelineRunner>();
runner.Run(args);

Console.WriteLine("\n=== Pipeline Execution Completed ===");