using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZATO.Contracts;
using ZATO.DTOs;
using ZATO.Models;

namespace ZATO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Restaurants> _genericRepository;
        public RestaurantsController(IGenericRepository<Restaurants> genericRepository,IMapper mapper)
        {
            _genericRepository = genericRepository;
            this._mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDTO>>> GetAllRestaurants()
        {
            var restaurants = await _genericRepository.GetAll();
            var restaurantDtos = _mapper.Map<IEnumerable<RestaurantDTO>>(restaurants);
            return Ok(restaurantDtos);
        }

        [HttpGet("{restaurantId}", Name = "GetRestaurantById")]
        public async Task<ActionResult<RestaurantDTO>> GetRestaurantById(int restaurantId)
        {
            var restaurant = await _genericRepository.GetByIdAsync(restaurantId);

            if (restaurant == null)
            {
                return NotFound($"Restaurant with ID {restaurantId} not found.");
            }

            var restaurantDto = _mapper.Map<RestaurantDTO>(restaurant);
            return Ok(restaurantDto);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRestaurant([FromBody] RestaurantDTO restaurantDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var restaurantEntity = _mapper.Map<Restaurants>(restaurantDto);

            try
            {
                await _genericRepository.AddAsync(restaurantEntity);
                var savedRestaurantDto = _mapper.Map<RestaurantDTO>(restaurantEntity);
                return CreatedAtAction(nameof(GetRestaurantById), new { restaurantId = restaurantEntity.RestaurantID }, savedRestaurantDto);
            }
            catch (Exception ex)
            {
                // Log the exception and return an error response
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
