 using AutoMapper;
 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZATO.Contracts;
using ZATO.DTOs;
using ZATO.Models;
using ZATO.Repository;

namespace ZATO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Orders> _genericRepository;
        public OrderController(IGenericRepository<Orders> genericRepository,IMapper mapper)
        {
            _genericRepository = genericRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<OrderDTO>> GetOrderList()
        {
            IEnumerable<Orders> orders = await this._genericRepository.GetAll();
            IEnumerable<OrderDTO> orderDtos = _mapper.Map<IEnumerable<OrderDTO>>(orders);
            return orderDtos;
        }

        [HttpGet("{orderId}", Name = "GetOrderById")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int orderId)
        {
            // Use the repository to fetch the order from the database
            Orders order = await this._genericRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }
            // Map Orders entity to OrderDto using AutoMapper
            OrderDTO orderDto = _mapper.Map<OrderDTO>(order);
            return orderDto;
        }
        
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> SaveOrder([FromBody] OrderDTO orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map OrderDto to Orders entity
            Orders orderEntity = _mapper.Map<Orders>(orderDto);

            // Use the repository to add the order to the context
            await this._genericRepository.AddAsync(orderEntity);

            // Map the saved entity back to OrderDto
            OrderDTO savedOrderDto = _mapper.Map<OrderDTO>(orderEntity);

            return CreatedAtAction(nameof(GetOrderById), new { orderId = orderEntity.OrderID }, savedOrderDto);
        }
        
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderDTO orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Use the repository to find the existing order
            Orders existingOrder = await this._genericRepository.GetByIdAsync(orderId);

            if (existingOrder == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            // Map updated OrderDto to existing Orders entity
            _mapper.Map(orderDto, existingOrder);

            // Use the repository to update the context
            await this._genericRepository.UpdateAsync(existingOrder);

            return NoContent();
        }
    }
}
