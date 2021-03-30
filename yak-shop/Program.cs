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
            //GetData();
            //AddYak();
            //FindYak(1);
            //UpdateYak(1);
            //DeleteYak(2);
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetService<YakContext>();
                    // for demo purposes, delete the database & migrate on startup so 
                    // we can start with a clean slate
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

        //static void GetData()
        //{
        //    var repository = CreateRepository();

        //    var yakDetails = repository.GetAll();

        //    Console.WriteLine($"Count:{ yakDetails.Count}");
        //    yakDetails.Output();
        //}

        //static void AddYak()
        //{
        //    var repository = CreateRepository();
        //    var yak = new YakDetails
        //    {
        //        Name = "Jetty",
        //        Age = 3,
        //        Sex = 'f',
        //        ageLastShaved = 3
        //    };
        //    repository.AddYak(yak);
        //}

        //static void FindYak(int id)
        //{
        //    var repository = CreateRepository();
        //    var yak = repository.FindYakData(id);
        //    yak.Output();
        //}

        //static void UpdateYak(int id)
        //{
        //    var repository = CreateRepository();
        //    var yak = repository.FindYakData(id);
        //    yak.Output();
        //    yak.Age = 6;
        //    repository.UpdateYakData(yak);

        //    //var repository2 = CreateRepository();
        //    //var yak2 = repository2.FindYakData(id);
        //    //yak2.Output();
        //}

        //static void DeleteYak(int id)
        //{
        //    var repository = CreateRepository();
        //    repository.FindYakData(id);
        //    repository.RemoveYakData(id);

        //    //var repository2 = CreateRepository();
        //    //repository2.FindYakData(id);
        //}

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
