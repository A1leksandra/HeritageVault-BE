using HV.BLL.DTO.LandmarkTag;
using HV.BLL.Services;
using HV.BLL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace HV.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class LandmarkTagsController(ILandmarkTagService tagService) : ControllerBase
{
    private readonly ILandmarkTagService _tagService = tagService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LandmarkTagListItemDto>>> GetLandmarkTags([FromQuery] GetLandmarkTagsQuery query)
    {
        var tags = await _tagService.GetListAsync(query);
        return Ok(tags);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LandmarkTagDetailsDto>> GetLandmarkTag(int id)
    {
        var tag = await _tagService.GetByIdAsync(id);
        return Ok(tag);
    }

    [HttpPost]
    public async Task<ActionResult<LandmarkTagDetailsDto>> CreateLandmarkTag([FromBody] CreateLandmarkTagRequest request)
    {
        var tag = await _tagService.CreateAsync(request);
        return CreatedAtAction(nameof(GetLandmarkTag), new { id = tag.Id }, tag);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<LandmarkTagDetailsDto>> UpdateLandmarkTag(int id, [FromBody] UpdateLandmarkTagRequest request)
    {
        var tag = await _tagService.UpdateAsync(id, request);
        return Ok(tag);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteLandmarkTag(int id)
    {
        await _tagService.DeleteAsync(id);
        return NoContent();
    }
}

