using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MkDocsDatabaseGenerator.Properties;
using MkDocsDatabaseGenerator.ViewModel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkDocsDatabaseGenerator.ViewModel
{
    internal class DependencyInjection
    {
        internal static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            Logging.Instance.Function(configuration, services);
            services.AddSingleton<IMainWindowsViewModel, MainWindowViewModel>();
            services.AddSingleton<IGeneratorSettingsViewModel, GeneratorSettingsViewModel>();
            services.AddSingleton<IGeneratorViewModel, GeneratorViewModel>();
        }
    }
}