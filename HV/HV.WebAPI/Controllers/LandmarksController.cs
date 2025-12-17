using HV.BLL.DTO.Landmark;
using HV.BLL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace HV.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class LandmarksController(ILandmarkService landmarkService) : ControllerBase
{
    private readonly ILandmarkService _landmarkService = landmarkService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LandmarkListItemDto>>> GetLandmarks([FromQuery] GetLandmarksQuery query)
    {
        var landmarks = await _landmarkService.GetListAsync(query);
        return Ok(landmarks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LandmarkDetailsDto>> GetLandmark(int id)
    {
        var landmark = await _landmarkService.GetByIdAsync(id);
        return Ok(landmark);
    }

    [HttpPost]
    public async Task<ActionResult<LandmarkDetailsDto>> CreateLandmark([FromBody] CreateLandmarkRequest request)
    {
        var landmark = await _landmarkService.CreateAsync(request);
        return CreatedAtAction(nameof(GetLandmark), new { id = landmark.Id }, landmark);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<LandmarkDetailsDto>> UpdateLandmark(int id, [FromBody] UpdateLandmarkRequest request)
    {
        var landmark = await _landmarkService.UpdateAsync(id, request);
        return Ok(landmark);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteLandmark(int id)
    {
        await _landmarkService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id:int}/image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(int id, IFormFile file)
    {
        await _landmarkService.UploadImageAsync(id, file);
        return NoContent();
    }

    [HttpDelete("{id:int}/image")]
    public async Task<IActionResult> DeleteImage(int id)
    {
        await _landmarkService.DeleteImageAsync(id);
        return NoContent();
    }
}

