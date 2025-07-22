using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.Api.Data;
using TravelPlanner.Api.Models;

namespace TravelPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StopsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StopsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Stops/ByTrip/5
        [HttpGet("ByTrip/{tripId}")]
        public async Task<ActionResult<IEnumerable<Stop>>> GetStopsForTrip(int tripId)
        {
            return await _context.Stops.Where(s => s.TripId == tripId).ToListAsync();
        }

        // GET: api/Stops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Stop>> GetStop(int id)
        {
            var stop = await _context.Stops.FindAsync(id);

            if (stop == null)
            {
                return NotFound();
            }

            return stop;
        }

        // POST: api/Stops
        // POST: api/Stops
        [HttpPost]
        public async Task<ActionResult<Stop>> PostStop(Stop stop)
        {
            try
            {
                // Check if the ModelState is valid (catches validation attribute errors)
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if the parent trip exists
                var tripExists = await _context.Trips.AnyAsync(t => t.Id == stop.TripId);
                if (!tripExists)
                {
                    return BadRequest("The specified TripId does not exist.");
                }

                _context.Stops.Add(stop);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetStop), new { id = stop.Id }, stop);
            }
            catch (Exception ex)
            {
                // Return a detailed error if something else goes wrong
                return BadRequest($"An error occurred while saving the stop: {ex.Message}");
            }
        }

        // PUT: api/Stops/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStop(int id, Stop stop)
        {
            if (id != stop.Id)
            {
                return BadRequest();
            }

            _context.Entry(stop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StopExists(id))
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

        // DELETE: api/Stops/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStop(int id)
        {
            var stop = await _context.Stops.FindAsync(id);
            if (stop == null)
            {
                return NotFound();
            }

            _context.Stops.Remove(stop);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StopExists(int id)
        {
            return _context.Stops.Any(e => e.Id == id);
        }
    }
}