using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Link.Request;

namespace WebApplication1.Link.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/v1/[controller]")]
public class AudioLinkController : ControllerBase
{
    [HttpPost]
    [Produces("application/json")]
    public IActionResult Create([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { error = "File not provided or empty." });
        }

        
        var storagePath = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "Link");
        
        if (!Directory.Exists(storagePath))
        {
            Directory.CreateDirectory(storagePath);
        }
        
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(storagePath, fileName);
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }
        
        var fileUrl = $"{Request.Scheme}://{Request.Host}/Storage/Link/{fileName}";

        return Ok(new { message = "File saved successfully", fileUrl = fileUrl });
    }
}
