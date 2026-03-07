using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecureAuthApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProtectedController: ControllerBase
{
    [HttpGet]
    public IActionResult GetProtectedData()
    {
        return Ok(new {
            Message = "This is protected data accessible only to authenticated users. You're authenticated successfully!", 
            UserEmail = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value});
    }
}