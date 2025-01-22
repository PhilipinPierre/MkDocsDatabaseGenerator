using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MkDocsDatabaseGenerator.Properties;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkDocsDatabaseGenerator
{
    internal static class DependencyInjection
    {
        private static IHost? _host { get; set; }

        public static IServiceProvider? ServiceProvider { get; private set; }

        internal static IHost Configure()
        {
            if (Directory.Exists(Logging.Instance.Assembly?.Location ?? System.AppDomain.CurrentDomain.BaseDirectory))
                _host = Host
                .CreateDefaultBuilder()
                .UseSerilog(logger: Logging.Instance.Logger)
                .ConfigureAppConfiguration((context, config)
                => config.SetBasePath(Path.GetDirectoryName(Logging.Instance.Assembly!.Location ?? System.AppDomain.CurrentDomain.BaseDirectory)!))
                .ConfigureServices(ConfigureServices).Build();
            else
                _host = Host
                .CreateDefaultBuilder()
                .UseSerilog(logger: Logging.Instance.Logger)
                .ConfigureServices(ConfigureServices).Build();
            ServiceProvider = _host.Services;
            return _host;
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
            => DependencyInjection.ConfigureServices(configuration: context.Configuration, services: services);

        internal static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            Logging.Instance.Function(configuration, services);

            ViewModel.DependencyInjection.ConfigureServices(configuration, services);
        }

        internal static void Start()
        {
            _host?.Start();
        }

        internal static async Task StartAsync()
        {
            if (_host != null)
                await _host.StartAsync();
        }

        internal static async Task StopAsync()
        {
            if (_host != null)
                await _host.StopAsync();
        }
    }
}