using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace ChecklistAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((_context, _builder) => {

                    var _currentPath = AppDomain.CurrentDomain.BaseDirectory;
                    var _fileName = "myApp-{Date}.txt";
                    var _logPath = "";
                    var _stgPath = "\\\\10.88.140.68\\c$\\EquipmentChecklistLogs\\Staging";
                    var _prdPath = "\\\\10.88.140.68\\c$\\EquipmentChecklistLogs\\Production";

                    if (_context.HostingEnvironment.IsDevelopment()) _logPath = $"{ _currentPath}\\Logs\\{_fileName}";
                    if (_context.HostingEnvironment.IsStaging()) _logPath = $"{_stgPath}\\{_fileName}";
                    if (_context.HostingEnvironment.IsProduction()) _logPath = $"{_prdPath}\\{_fileName}";
                    _builder.AddFile(_logPath);
                }) 
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    
                });
    }
}
