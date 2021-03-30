using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using yak_shop.DetailsAndUtilities;
using yak_shop.Models;

namespace yak_shop.Controllers
{
    [Route("Yak-Shop")]
    [ApiController]
    public class YakRESTControllerNew : ControllerBase
    {
        private readonly YakContext _context;
        //private readonly IYakDetailsRepository _yakDetailsRepository;
        //private static IConfigurationRoot config;

        public YakRESTControllerNew(YakContext context, IYakDetailsRepository yakDetailsRepository)
        {
            //Initialize();
            _context = context;
            //_yakDetailsRepository = yakDetailsRepository ?? throw new ArgumentNullException(nameof(yakDetailsRepository));
        }

        // GET: api/YakRESTControllerNew
        [HttpGet("herd")]
        public async Task<ActionResult<IEnumerable<YakDetails>>> GetAllYakDetails()
        {
            //var yakRepo = CreateRepository();
            //var yakDetails = yakRepo.GetAll();

            //if (yakDetails == null)
            //{
            //    return NotFound();
            //}

            //return yakDetails;
            return await _context.YakItems.ToListAsync();
        }

        // GET: api/YakRESTControllerNew/5
        [HttpGet("herd/{day}")]
        //public async Task<ActionResult<YakDetails>> GetYakDetails(int id)
        public async Task<ActionResult<IEnumerable<YakDetails>>> GetYakDetails(int day)
        {
            
            //var yakRepo = CreateRepository();
            //var yak = yakRepo.GetYak(id);

            var yakList = await _context.YakItems.ToListAsync();
            if (yakList == null)
            {
                return NotFound();
            }
            var yakUtilities = new YakUtilities();
            yakUtilities.GetHerdStatistics(ref yakList, day);
            return yakList;
        }



        //private static void Initialize()
        //{
        //    var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        //    config = builder.Build();
        //}

        //private static IYakDetailsRepository CreateRepository()
        //{
        //    return new YakDetailsRepository(config.GetConnectionString("DefaultConnection"));
        //}





        // PUT: api/YakRESTControllerNew/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutYakDetails(int id, YakDetails yakDetails)
        {
            if (id != yakDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(yakDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!YakDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/YakRESTControllerNew
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("herd")]
        public async Task<ActionResult<YakDetails>> PostYakDetails(YakDetails yakDetails)
        {
            _context.YakItems.Add(yakDetails);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetYakDetails", new { id = yakDetails.Id }, yakDetails);
            return CreatedAtAction(nameof(GetYakDetails), new { id = yakDetails.Id }, yakDetails);

            //var yakRepo = CreateRepository();
            //var id = yakRepo.AddYak(yakDetails);

            //var yak = yakRepo.GetYak(id);

            //if (yak == null)
            //{
            //    return NotFound();
            //}

            //return CreatedAtAction(nameof(GetYakDetails), new { id = yak.Id }, yak);
        }

        // DELETE: api/YakRESTControllerNew/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<YakDetails>> DeleteYakDetails(int id)
        {
            var yakDetails = await _context.YakItems.FindAsync(id);
            if (yakDetails == null)
            {
                return NotFound();
            }

            _context.YakItems.Remove(yakDetails);
            await _context.SaveChangesAsync();

            return yakDetails;
        }

        private bool YakDetailsExists(int id)
        {
            return _context.YakItems.Any(e => e.Id == id);
        }
    }
}
