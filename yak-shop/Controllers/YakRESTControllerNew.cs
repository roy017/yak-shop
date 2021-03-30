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

        public YakRESTControllerNew(YakContext context, IYakDetailsRepository yakDetailsRepository)
        {
            _context = context;
        }


        [HttpGet("herd")]
        public async Task<ActionResult<IEnumerable<YakDetails>>> GetAllYakDetails()
        {
            return await _context.YakItems.ToListAsync();
        }

        // GET: api/YakRESTControllerNew/5
        [HttpGet("herd/{day}")]
        public async Task<ActionResult<IEnumerable<YakDetails>>> GetYakDetails(int day)
        {
            var yakList = await _context.YakItems.ToListAsync();
            if (yakList == null)
            {
                return NotFound();
            }
            var yakUtilities = new YakUtilities();
            yakUtilities.GetHerdStatistics(ref yakList, day);
            return yakList;
        }

        [HttpGet("stock/{day}")]
        public async Task<ActionResult<StockDetails>> GetstockDetails(int day)
        {

            var yakList = await _context.YakItems.ToListAsync();
            if (yakList == null)
            {
                return NotFound();
            }
            var yakUtilities = new YakUtilities();
            
            var StockInfo = yakUtilities.GetHerdStatistics(ref yakList, day);
            
            return StockInfo;
        }

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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("herd")]
        public async Task<ActionResult<YakDetails>> PostYakDetails(YakDetails yakDetails)
        {
            _context.YakItems.Add(yakDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetYakDetails), new { id = yakDetails.Id }, yakDetails);
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
