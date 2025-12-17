using HV.BLL.DTO.City;
using HV.BLL.Services;
using HV.BLL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace HV.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CitiesController(ICityService cityService) : ControllerBase
{
    private readonly ICityService _cityService = cityService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityListItemDto>>> GetCities([FromQuery] GetCitiesQuery query)
    {
        var cities = await _cityService.GetListAsync(query);
        return Ok(cities);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CityDetailsDto>> GetCity(int id)
    {
        var city = await _cityService.GetByIdAsync(id);
        return Ok(city);
    }

    [HttpPost]
    public async Task<ActionResult<CityDetailsDto>> CreateCity([FromBody] CreateCityRequest request)
    {
        var city = await _cityService.CreateAsync(request);
        return CreatedAtAction(nameof(GetCity), new { id = city.Id }, city);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CityDetailsDto>> UpdateCity(int id, [FromBody] UpdateCityRequest request)
    {
        var city = await _cityService.UpdateAsync(id, request);
        return Ok(city);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCity(int id)
    {
        await _cityService.DeleteAsync(id);
        return NoContent();
    }
}

