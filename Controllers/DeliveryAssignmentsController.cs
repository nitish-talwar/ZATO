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
    public class DeliveryAssignmentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<DeliveryAssignments> _deliveryAssignmentsRepository;
        private readonly IGenericRepository<Orders> _ordersRepository;
        private readonly IGenericRepository<DeliveryPersonnel> _deliveryPersonnelRepository;

        public DeliveryAssignmentsController(
            IMapper mapper,
            IGenericRepository<DeliveryAssignments> deliveryAssignmentsRepository,
            IGenericRepository<Orders> ordersRepository,
            IGenericRepository<DeliveryPersonnel> deliveryPersonnelRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _deliveryAssignmentsRepository = deliveryAssignmentsRepository ?? throw new ArgumentNullException(nameof(deliveryAssignmentsRepository));
            _ordersRepository = ordersRepository ?? throw new ArgumentNullException(nameof(ordersRepository));
            _deliveryPersonnelRepository = deliveryPersonnelRepository ?? throw new ArgumentNullException(nameof(deliveryPersonnelRepository));
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignDeliveryPerson([FromBody] DeliveryAssignmentDTO deliveryAssignmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the order exists
            var order = await _ordersRepository.GetByIdAsync(deliveryAssignmentDto.OrderID);
            if (order == null)
            {
                return NotFound($"Order with ID {deliveryAssignmentDto.OrderID} not found.");
            }

            // Check if the delivery person exists
            var deliveryPerson = await _deliveryPersonnelRepository.GetByIdAsync(deliveryAssignmentDto.DeliveryAgentID);
            if (deliveryPerson == null)
            {
                return NotFound($"Delivery person with ID {deliveryAssignmentDto.DeliveryAgentID} not found.");
            }

            // Create a delivery assignment entity
            var deliveryAssignmentEntity = _mapper.Map<DeliveryAssignments>(deliveryAssignmentDto);

            try
            {
                await _deliveryAssignmentsRepository.AddAsync(deliveryAssignmentEntity);
                var savedDeliveryAssignmentDto = _mapper.Map<DeliveryAssignmentDTO>(deliveryAssignmentEntity);
                return CreatedAtAction(nameof(GetDeliveryAssignmentById), new { assignmentId = deliveryAssignmentEntity.AssignmentID }, savedDeliveryAssignmentDto);
            }
            catch (Exception ex)
            {
                // Log the exception and return an error response
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{assignmentId}", Name = "GetDeliveryAssignmentById")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DeliveryAssignmentDTO>> GetDeliveryAssignmentById(int assignmentId)
        {
            var deliveryAssignment = await _deliveryAssignmentsRepository.GetByIdAsync(assignmentId);

            if (deliveryAssignment == null)
            {
                return NotFound($"Delivery assignment with ID {assignmentId} not found.");
            }

            var deliveryAssignmentDto = _mapper.Map<DeliveryAssignmentDTO>(deliveryAssignment);
            return Ok(deliveryAssignmentDto);
        }
    }
}
