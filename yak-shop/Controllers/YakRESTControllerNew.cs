using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using yak_shop.DetailsAndUtilities;
using yak_shop.Models;

namespace yak_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YakRESTControllerNew : ControllerBase
    {
        private readonly YakContext _context;
        private readonly IYakDetailsRepository _yakDetailsRepository;

        public YakRESTControllerNew(YakContext context, IYakDetailsRepository yakDetailsRepository)
        {
            _context = context;
            _yakDetailsRepository = yakDetailsRepository ?? throw new ArgumentNullException(nameof(yakDetailsRepository));
        }

        // GET: api/YakRESTControllerNew
        [HttpGet]
        public async Task<ActionResult<IEnumerable<YakDetails>>> GetTodoItems()
        {
            return await _context.YakItems.ToListAsync();
        }

        // GET: api/YakRESTControllerNew/5
        [HttpGet("{id}")]
        public async Task<ActionResult<YakDetails>> GetYakDetails(int id)
        {
            //var yakDetails = await _context.YakItems.FindAsync(id);
            var yakDetails = _yakDetailsRepository.GetYak(id);

            if (yakDetails == null)
            {
                return NotFound();
            }

            return yakDetails;
        }

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
        [HttpPost]
        public async Task<ActionResult<YakDetails>> PostYakDetails(YakDetails yakDetails)
        {
            _context.YakItems.Add(yakDetails);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetYakDetails", new { id = yakDetails.Id }, yakDetails);
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
