using DizuAutomation.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DizuAutomation
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        static async Task Main(string[] args)
        {



            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            var setting = new Settings();
            configuration.GetSection("settings").Bind(setting);


            await WebDriverExecucao.ExecuteAutomation(setting);


        }

      
    }
}
