using Microsoft.AspNetCore.Mvc;
using LibraryDemo.Models;
using WebAPIDemo.Repositories;

namespace WebAPIDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShirtsController : ControllerBase
    {
        private readonly ShirtRepository _shirtRepository;

		public ShirtsController(ShirtRepository shirtRepository)
        {
            _shirtRepository = shirtRepository;
        }
        [HttpGet]
        public IActionResult GetShirts()
        {
            return Ok(_shirtRepository.GetShirts());
        }

        [HttpGet("{id}")]
        //we use filter
        // we use IActionResult instead of Shirt because we are not returnig Shirt, we return NotFound() (err400) or Ok(shirt) (ok200), generally, we use IActionResult interface as a return type when we have more than one return type
        public IActionResult GetShirtById(int id)
        {
            // we do all validition using filter [Shirt_ValidateShirtIdFilter]
            return Ok(_shirtRepository.GetShirtById(id));
            
        }

        [HttpPost]
        public IActionResult CreateShirt([FromBody]Shirt shirt)
        {
            _shirtRepository.AddShirt(shirt);

            return CreatedAtAction(nameof(GetShirtById),
                new { id = shirt.ShirtId },
                shirt);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateShirt(int id, Shirt shirt)
        {
            _shirtRepository.UpdateShirt(shirt);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteShirt(int id)
        {
            var shirt = _shirtRepository.GetShirtById(id);
            _shirtRepository.DeleteShirt(id);
            return Ok(shirt);
        }

		[HttpPut("patch/{id}")]
		public IActionResult PatchShirt(int id, Shirt shirt)
		{
			_shirtRepository.PatchShirtRank(id, shirt.rank);
			return NoContent();
		}

	}
}
