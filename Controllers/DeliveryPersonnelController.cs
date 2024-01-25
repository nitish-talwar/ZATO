using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZATO.Contracts;
using ZATO.DTOs;
using ZATO.Models;

namespace ZATO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryPersonnelController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<DeliveryPersonnel> _deliveryPersonnelRepository;

        public DeliveryPersonnelController(IMapper mapper, IGenericRepository<DeliveryPersonnel> deliveryPersonnelRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _deliveryPersonnelRepository = deliveryPersonnelRepository ?? throw new ArgumentNullException(nameof(deliveryPersonnelRepository));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<DeliveryPersonnelDTO>>> GetAllDeliveryPersonnel()
        {
            var deliveryPersonnel = await _deliveryPersonnelRepository.GetAll();
            var deliveryPersonnelDtos = _mapper.Map<IEnumerable<DeliveryPersonnelDTO>>(deliveryPersonnel);
            return Ok(deliveryPersonnelDtos);
        }

        [HttpGet("{deliveryPersonId}", Name = "GetDeliveryPersonById")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DeliveryPersonnelDTO>> GetDeliveryPersonById(int deliveryPersonId)
        {
            var deliveryPerson = await _deliveryPersonnelRepository.GetByIdAsync(deliveryPersonId);

            if (deliveryPerson == null)
            {
                return NotFound($"Delivery person with ID {deliveryPersonId} not found.");
            }

            var deliveryPersonDto = _mapper.Map<DeliveryPersonnelDTO>(deliveryPerson);
            return Ok(deliveryPersonDto);
        }

        // Other APIs (POST, PUT, DELETE) can be added here

        // For example:
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveDeliveryPerson([FromBody] DeliveryPersonnelDTO deliveryPersonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deliveryPersonEntity = _mapper.Map<DeliveryPersonnel>(deliveryPersonDto);

            try
            {
                await _deliveryPersonnelRepository.AddAsync(deliveryPersonEntity);
                var savedDeliveryPersonDto = _mapper.Map<DeliveryPersonnelDTO>(deliveryPersonEntity);
                return CreatedAtAction(nameof(GetDeliveryPersonById), new { deliveryPersonId = deliveryPersonEntity.DeliveryPersonID }, savedDeliveryPersonDto);
            }
            catch (Exception ex)
            {
                // Log the exception and return an error response
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
