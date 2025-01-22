using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace MkDocsDatabaseGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Logging.Instance.ConfigureLogger(Wpf.Util.Extension.UserExtension.GetGuid() ?? default, typeof(App).Assembly?.GetName()?.Name ?? Assembly.GetExecutingAssembly()?.GetName()?.Name ?? "MkDocsDatabaseGenerator");
            IHost host = DependencyInjection.Configure();
            host.Start();
            StartupUri = new Uri("/MkDocsDatabaseGenerator;component/View/MainWindow.xaml", UriKind.Relative);
        }
    }
}