using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using yak_shop.DetailsAndUtilities;

namespace yak_shop
{

    public class Program
    {
        private static IConfigurationRoot config;
        public static void Main(string[] args)
        {
            Initialize();
            //GetData();
            //AddYak();
            FindYak(1);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
        static void GetData()
        {
            var repository = CreateRepository();

            var yakDetails = repository.GetAll();

            Console.WriteLine($"Count:{ yakDetails.Count}");
            yakDetails.Output();
        }

        static void AddYak()
        {
            var repository = CreateRepository();
            var yak = new YakDetails
            {
                Name = "Jetty",
                Age = 3,
                Sex = 'f',
                ageLastShaved = 3
            };
            repository.AddYak(yak);
        }

        static void FindYak(int id)
        {
            var repository = CreateRepository();
            var yak = repository.FindYakData(id);
            yak.Output();
        }

        private static void Initialize()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            config = builder.Build();
        }

        private static IYakDetailsRepository CreateRepository()
        {
            return new YakDetailsRepository(config.GetConnectionString("DefaultConnection"));
        }
    }
}
