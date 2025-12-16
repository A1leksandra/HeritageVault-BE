using HV.BLL.DTO.Country;
using HV.BLL.Services;
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
        var countries = await _countryService.GetCountriesAsync(query);
        return Ok(countries);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CountryDetailsDto>> GetCountry(int id)
    {
        var country = await _countryService.GetCountryByIdAsync(id);
        return Ok(country);
    }

    [HttpPost]
    public async Task<ActionResult<CountryDetailsDto>> CreateCountry([FromBody] CreateCountryRequest request)
    {
        var country = await _countryService.CreateCountryAsync(request);
        return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CountryDetailsDto>> UpdateCountry(int id, [FromBody] UpdateCountryRequest request)
    {
        var country = await _countryService.UpdateCountryAsync(id, request);
        return Ok(country);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCountry(int id)
    {
        await _countryService.DeleteCountryAsync(id);
        return NoContent();
    }
}

