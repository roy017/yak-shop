using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using yak_shop.Models;
using yak_shop.DetailsAndUtilities;
using System.Web;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace yak_shop.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static IConfigurationRoot config;

        public HomeController(ILogger<HomeController> logger)
        {
            Initialize();
            _logger = logger;
        }

        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }
        [Route("Yak-Shop/herdInfo/")]
        [HttpGet("{herdDays}.{format?}")]
        public IActionResult Herd(string herdDays)
        {
            int T = 0;
            try
            {
                T = int.Parse(herdDays);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var repository = CreateRepository();
            List<YakDetails> herdInfo = repository.GetAll();

            YakUtilities YakUtilities = new YakUtilities();
            YakUtilities.GetHerdStatistics(ref herdInfo, T);

            //var json = JsonConvert.SerializeObject(herdInfo.ToArray(), Formatting.Indented);
            //System.IO.File.WriteAllText(@"JsonFiles\DataHerd_JSON.json", json);

            ViewData["herdInfo"] = herdInfo;
            ViewData["MaxAge"] = 10f;
            ViewData["CurrentDay"] = T;
            return View();
        }

        [Route("Yak-Shop/stock/")]
        //[Produces("application/json")]
        [HttpGet("{herdDays}.{format?}")]
        public IActionResult Stock(string herdDays)
        {
            int T = 0;
            try
            {
                T = int.Parse(herdDays);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            YakUtilities YakUtilities = new YakUtilities();
            var repository = CreateRepository();
            List<YakDetails> herdInfo = repository.GetAll();

            var stockInfo = YakUtilities.GetHerdStatistics(ref herdInfo, T);
            var json = JsonConvert.SerializeObject(stockInfo, Formatting.Indented);
            System.IO.File.WriteAllText(@"JsonFiles\DataStock_JSON.json", json);

            ViewData["stockInfoMilk"] = Math.Round(stockInfo.Milk, 2);
            ViewData["stockInfoSkins"] = stockInfo.Skins;
            ViewData["CurrentDay"] = T;
            return View();
        }

        [Route("Yak-Shop/order/")]
        [HttpPost("{herdDays}")]
        public IActionResult Order(string customerName, string milkOrder, string skinsOrder, string herdDays)
        {
            bool enoughMilk = false;
            bool enoughSkins = false;
            int T = 0;
            float milk = 0;
            int skins = 0;
            try
            {
                T = int.Parse(herdDays);
                milk = float.Parse(milkOrder);
                skins = int.Parse(skinsOrder);
                T = int.Parse(herdDays);
                milk = float.Parse(milkOrder);
                skins = int.Parse(skinsOrder);

                YakUtilities YakUtilities = new YakUtilities();
                var repository = CreateRepository();
                List<YakDetails> herdInfo = repository.GetAll();

                StockDetails stockInfo = new StockDetails();
                stockInfo = YakUtilities.GetHerdStatistics(ref herdInfo, T);

                if ((stockInfo.Milk - milk) >= 0)
                    enoughMilk = true;
                if ((stockInfo.Skins - skins) >= 0)
                    enoughSkins = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            HttpResponseMessage message = OrderMessage(enoughMilk, enoughSkins);
            HttpStatusCode statusCode = message.StatusCode;

            ViewData["CurrentDay"] = T;
            ViewData["StatusCode"] = (int)statusCode;
            ViewData["enoughMilk"] = enoughMilk;
            ViewData["enoughSkins"] = enoughSkins;
            ViewData["milk"] = milk;
            ViewData["skins"] = skins;

            OrderDetails orderInfo = new OrderDetails();
            orderInfo.status = (int)statusCode;
            Order order = new Order();
            if(enoughMilk)
                order.milk = milk;
            if(enoughSkins)
                order.skins = skins;
            orderInfo.order = order;

            var json = JsonConvert.SerializeObject(orderInfo, Formatting.Indented);
            System.IO.File.WriteAllText(@"JsonFiles\DataOrder_JSON.json", json);
            //System.IO.File.AppendAllText(@"JsonFiles\DataOrder_JSON.json", json);

            return View();
        }
        public HttpResponseMessage OrderMessage(bool enoughMilk, bool enoughSkins)
        {
                if (enoughMilk && enoughSkins)
                {
                    return new HttpResponseMessage(HttpStatusCode.Created);
                }
                else if (enoughMilk || enoughSkins)
                {
                    return new HttpResponseMessage(HttpStatusCode.PartialContent);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
