using HV.BLL.DTO.Country;
using HV.BLL.Services;
using HV.BLL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace HV.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CountriesController(ICountryService countryService) : ControllerBase
{
    private readonly ICountryService _countryService = countryService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CountryListItemDto>>> GetCountries([FromQuery] GetCountriesQuery query)
    {
        var countries = await _countryService.GetListAsync(query);
        return Ok(countries);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CountryDetailsDto>> GetCountry(int id)
    {
        var country = await _countryService.GetByIdAsync(id);
        return Ok(country);
    }

    [HttpPost]
    public async Task<ActionResult<CountryDetailsDto>> CreateCountry([FromBody] CreateCountryRequest request)
    {
        var country = await _countryService.CreateAsync(request);
        return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CountryDetailsDto>> UpdateCountry(int id, [FromBody] UpdateCountryRequest request)
    {
        var country = await _countryService.UpdateAsync(id, request);
        return Ok(country);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCountry(int id)
    {
        await _countryService.DeleteAsync(id);
        return NoContent();
    }
}

