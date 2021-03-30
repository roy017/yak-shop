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
using Microsoft.Extensions.DependencyInjection;
using yak_shop.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace yak_shop
{

    public class Program
    {
        private static IConfigurationRoot config;
        public static void Main(string[] args)
        {
            Initialize();
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetService<YakContext>();

                    context.Database.EnsureDeleted();
                    context.Database.Migrate();
                    
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            XMLDataToDatabase();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        static void XMLDataToDatabase()
        {
            var herdInfo = GetXMLData();

            var repository = CreateRepository();
            

            foreach ( var yak in herdInfo)
            {
                repository.AddYak(yak);
            }
        }

        static List<YakDetails> GetXMLData()
        {
            XElement document = XElement.Load(@"DetailsAndUtilities\YakData.xml");
            string ResultText = document.FirstNode.ToString();
            IEnumerable<XElement> yakInfo = (from info in document.Elements("labyak") select info);

            List<YakDetails> herdInfo = new List<YakDetails>();
            if (yakInfo != null)
            {
                foreach (var info in yakInfo)
                {
                    string Name = info.Attribute("name").Value;

                    string strAge = info.Attribute("age").Value;
                    float Age = float.Parse(strAge);


                    string strSex = info.Attribute("sex").Value;
                    char Sex = char.Parse(strSex);

                    YakDetails newYak = new YakDetails(Name, Age, Sex, Age);
                    herdInfo.Add(newYak);
                }
            }
            return herdInfo;
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
