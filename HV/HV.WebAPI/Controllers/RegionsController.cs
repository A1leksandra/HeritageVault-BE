using HV.BLL.DTO.Region;
using HV.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace HV.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class RegionsController(IRegionService regionService) : ControllerBase
{
    private readonly IRegionService _regionService = regionService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RegionListItemDto>>> GetRegions([FromQuery] GetRegionsQuery query)
    {
        var regions = await _regionService.GetRegionsAsync(query);
        return Ok(regions);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RegionDetailsDto>> GetRegion(int id)
    {
        var region = await _regionService.GetRegionByIdAsync(id);
        return Ok(region);
    }

    [HttpPost]
    public async Task<ActionResult<RegionDetailsDto>> CreateRegion([FromBody] CreateRegionRequest request)
    {
        var region = await _regionService.CreateRegionAsync(request);
        return CreatedAtAction(nameof(GetRegion), new { id = region.Id }, region);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RegionDetailsDto>> UpdateRegion(int id, [FromBody] UpdateRegionRequest request)
    {
        var region = await _regionService.UpdateRegionAsync(id, request);
        return Ok(region);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteRegion(int id)
    {
        await _regionService.DeleteRegionAsync(id);
        return NoContent();
    }
}

