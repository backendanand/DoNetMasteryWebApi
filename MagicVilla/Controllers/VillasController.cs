using MagicVilla.Data;
using MagicVilla.Logging;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillasController : ControllerBase
    {
        //private readonly ILogger<VillasController> _logger;
        private readonly ILogging _logger;
        private readonly ApplicationDbContext _context;
        public VillasController(ApplicationDbContext context, ILogging logger)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.Log("Getting all villas", "info");
            return Ok(_context.Villas.ToList());
        }

        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0) { 
                _logger.Log("Get Villa Error with Id = " + id, "error");
                return BadRequest(); 
            }
            var villa = _context.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null) {
                _logger.Log("Get Villa Error with Id = " + id + " not found", "error");
                return NotFound(); 
            }
            _logger.Log("Getting Villa with Id = " + id, "info");
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {
            if(_context.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists!");
                return BadRequest(ModelState);
            }
            if (villaDTO == null) return BadRequest();
            if (villaDTO.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

            Villa newVilla = new ()
            {
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Sqft = villaDTO.Sqft,
                Rate = villaDTO.Rate,
                Occupancy = villaDTO.Occupancy,
                ImageUrl = villaDTO.ImageUrl,
                Amenity = villaDTO.Amenity,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            _context.Villas.Add(newVilla);
            _context.SaveChanges();
            return CreatedAtRoute("GetVilla", new {id = villaDTO.Id}, villaDTO);
        }

        [HttpDelete("{id:int}", Name ="DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0) return BadRequest();
            var villa = _context.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null) return NotFound();
            _context.Villas.Remove(villa);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
        {
            if (villaDTO == null || villaDTO.Id != id) return BadRequest();

            //var villa = _context.Villas.FirstOrDefault(u => u.Id == id);
            //villa.Name = villaDTO.Name;
            //villa.Sqft = villaDTO.Sqft;
            //villa.Occupancy = villaDTO.Occupancy;

            var villa = _context.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null) return BadRequest();
            villa.Id = villaDTO.Id;
            villa.Name = villaDTO.Name;
            villa.Details = villaDTO.Details;
            villa.Sqft = villaDTO.Sqft;
            villa.Rate = villaDTO.Rate;
            villa.Occupancy = villaDTO.Occupancy;
            villa.ImageUrl = villaDTO.ImageUrl;
            villa.Amenity = villaDTO.Amenity;
            villa.UpdatedDate = DateTime.Now;

            _context.Villas.Update(villa);
            _context.SaveChanges();
            return NoContent();

        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            //if (patchDTO == null || id == 0) return BadRequest();
            //var villa = _context.Villas.FirstOrDefault(u => u.Id == id);
            //if (villa == null) return BadRequest();
            //VillaDTO villaDTO = new()
            //{
            //    Id = villa.Id,
            //    Name = villa.Name,
            //    Details = villa.Details,
            //    Sqft = villa.Sqft,
            //    Rate = villa.Rate,
            //    Occupancy = villa.Occupancy,
            //    ImageUrl = villa.ImageUrl,
            //    Amenity = villa.Amenity,
            //    CreatedDate = villa.CreatedDate,
            //    UpdatedDate = DateTime.Now
            //};
            //patchDTO.ApplyTo(villaDTO, ModelState);
            //Villa updatedVilla = new Villa()
            //{
            //    Id = villaDTO.Id,
            //    Name = villaDTO.Name,
            //    Details = villaDTO.Details,
            //    Sqft = villaDTO.Sqft,
            //    Rate = villaDTO.Rate,
            //    Occupancy = villaDTO.Occupancy,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Amenity = villaDTO.Amenity,
            //    CreatedDate = villaDTO.CreatedDate,
            //    UpdatedDate = villaDTO.UpdatedDate
            //};
            //_context.Villas.Update(updatedVilla);
            //_context.SaveChanges();
            //return NoContent();

            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = _context.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            // Map Villa to VillaDTO
            VillaDTO villaDTO = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Details = villa.Details,
                Sqft = villa.Sqft,
                Rate = villa.Rate,
                Occupancy = villa.Occupancy,
                ImageUrl = villa.ImageUrl,
                Amenity = villa.Amenity,
                CreatedDate = villa.CreatedDate,
                UpdatedDate = villa.UpdatedDate
            };

            // Apply the patch to the VillaDTO
            patchDTO.ApplyTo(villaDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map VillaDTO back to Villa
            villa.Name = villaDTO.Name;
            villa.Details = villaDTO.Details;
            villa.Sqft = villaDTO.Sqft;
            villa.Rate = villaDTO.Rate;
            villa.Occupancy = villaDTO.Occupancy;
            villa.ImageUrl = villaDTO.ImageUrl;
            villa.Amenity = villaDTO.Amenity;
            villa.UpdatedDate = DateTime.Now;

            try
            {
                _context.Villas.Update(villa);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Villas.Any(v => v.Id == id))
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
    }
}
