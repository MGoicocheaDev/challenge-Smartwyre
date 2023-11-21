using Autofac;
using Serilog;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        var containerBuilder = new ContainerBuilder();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        containerBuilder.RegisterType<ProductDataStore>().As<IProductDataStore>();
        containerBuilder.RegisterType<RebateDataStore>().As<IRebateDataStore>();
        containerBuilder.RegisterType<RebateService>().As<IRebateService>();
        containerBuilder.RegisterInstance(Log.Logger).As<ILogger>();

        var container = containerBuilder.Build();
        var rebateServices = container.Resolve<IRebateService>();

        var result = rebateServices.Calculate(new CalculateRebateRequest
        {
            ProductIdentifier = "product 1",
            RebateIdentifier = "rebate 1",
            Volume = 10
        });

        Console.WriteLine($"Result for rebate calculate is: {result.Success}");

        Console.WriteLine("Press a key to close the app");
        Console.ReadKey();
        Log.CloseAndFlush();
    }
}
