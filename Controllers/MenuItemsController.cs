using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZATO.Abstractions;
using ZATO.Contracts;
using ZATO.DTOs;
using ZATO.Infrastructure;
using ZATO.Models;

namespace ZATO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<MenuItems> _genericRepository;
        public readonly ICacheService _CacheService;

        public MenuItemsController(IMapper mapper,IGenericRepository<MenuItems> genericRepository,ICacheService cacheService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _genericRepository = genericRepository ?? throw new ArgumentNullException(nameof(IGenericRepository<MenuItems>));
            _CacheService = cacheService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemsDTO>>> GetMenuItemsList()
        {
            IEnumerable<MenuItemsDTO>? menuItemsDtos =
                await _CacheService.GetAsync<IEnumerable<MenuItemsDTO>>("MenuItems");
            var menuItems = await _genericRepository.GetAll();
            var menuItemsDtoList = _mapper.Map<IEnumerable<MenuItemsDTO>>(menuItems);
            await _CacheService.SetAsync("MenuItems", menuItemsDtoList);
            return Ok(menuItemsDtoList);
        }

        [HttpGet("{itemId}", Name = "GetMenuItemById")]
        public async Task<ActionResult<MenuItemsDTO>> GetMenuItemById(int itemId)
        {
            var menuItem = await _genericRepository.GetByIdAsync(itemId);
            if (menuItem == null)
            {
                return NotFound($"MenuItem with ID {itemId} not found.");
            }

            var menuItemDto = _mapper.Map<MenuItemsDTO>(menuItem);
            return Ok(menuItemDto);
        }

        [HttpPost]
        public async Task<ActionResult<MenuItemsDTO>> SaveMenuItem([FromBody] MenuItemsDTO menuItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var menuItemEntity = _mapper.Map<MenuItems>(menuItemDto);
            await _genericRepository.AddAsync(menuItemEntity);

            var savedMenuItemDto = _mapper.Map<MenuItemsDTO>(menuItemEntity);
            return CreatedAtAction(nameof(GetMenuItemById), new { itemId = savedMenuItemDto.ItemID }, savedMenuItemDto);
        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateMenuItem(int itemId, [FromBody] MenuItemsDTO menuItemDto)
        {
            if (itemId != menuItemDto.ItemID)
            {
                return BadRequest("Mismatched itemId in the route and request body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingMenuItem = await _genericRepository.GetByIdAsync(itemId);
            if (existingMenuItem == null)
            {
                return NotFound($"MenuItem with ID {itemId} not found.");
            }

            _mapper.Map(menuItemDto, existingMenuItem);
            await _genericRepository.UpdateAsync(existingMenuItem);

            return NoContent();
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteMenuItem(int itemId)
        {
            var menuItem = await _genericRepository.GetByIdAsync(itemId);
            if (menuItem == null)
            {
                return NotFound($"MenuItem with ID {itemId} not found.");
            }

            await _genericRepository.DeleteAsync(menuItem);

            return NoContent();
        }
    }
}
