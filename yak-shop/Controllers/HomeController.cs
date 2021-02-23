using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using yak_shop.Models;
using yak_shop;
using System.Web;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace yak_shop.Controllers
{
    //[Route("yak-shop/[controller]")]
    //[ApiController]
    //[FormatFilter]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
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
        [Route("Yak-Shop/herd/")]
        //[Produces("application/json")]
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

            YakUtilities YakUtilities = new YakUtilities();
            List<YakDetails> herdInfo = YakUtilities.GetAllData();

            ViewData["herdInfo"] = herdInfo;

            StockDetails stockInfo = new StockDetails();
            stockInfo = YakUtilities.GetHerdStatistics(ref herdInfo, T);

            var json = JsonConvert.SerializeObject(herdInfo.ToArray(), Formatting.Indented);
            System.IO.File.WriteAllText(@"DataHerd_JSON.txt", json);





            //string dateTime = DateTime.Now.ToShortDateString();
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
            List<YakDetails> herdInfo = YakUtilities.GetAllData();

            //ViewData["herdInfo"] = herdInfo;

            StockDetails stockInfo = new StockDetails();
            stockInfo = YakUtilities.GetHerdStatistics(ref herdInfo, T);
            var json = JsonConvert.SerializeObject(stockInfo, Formatting.Indented);
            System.IO.File.WriteAllText(@"DataStock_JSON.txt", json);

            //string dateTime = DateTime.Now.ToShortDateString();
            ViewData["stockInfoMilk"] = Math.Round(stockInfo.Milk, 2);
            ViewData["stockInfoSkins"] = stockInfo.Skins;
            ViewData["CurrentDay"] = T;
            return View();
        }

        [Route("Yak-Shop/order/")]
        //[Produces("application/json")]
        [HttpPost("{herdDays}")]
        public HttpResponseMessage Order(string customerName, string milkOrder, string skinsOrder, string herdDays)
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

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            YakUtilities YakUtilities = new YakUtilities();
            List<YakDetails> herdInfo = YakUtilities.GetAllData();

            //ViewData["herdInfo"] = herdInfo;

            StockDetails stockInfo = new StockDetails();
            stockInfo = YakUtilities.GetHerdStatistics(ref herdInfo, T);

            if ((stockInfo.Milk - milk) >= 0)
                enoughMilk = true;
            if ((stockInfo.Skins - skins) >= 0)
                enoughSkins = true;

            if(enoughMilk && enoughSkins)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else if(enoughMilk || enoughSkins)
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
    }
}
