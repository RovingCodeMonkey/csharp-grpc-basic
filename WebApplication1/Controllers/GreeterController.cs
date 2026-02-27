using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services.gRPCClient;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class GreeterController : ControllerBase
{
    // GET /greeter/chat?user=Anthony&text=Hello
    [HttpGet("chat")]
    public async Task<IActionResult> Chat([FromQuery] string user, [FromQuery] string text)
    {
        using var client = new GreeterClient();
        var (responseUser, responseText) = await client.ChatAsync(user, text);
        return Ok(new { user = responseUser, text = responseText });
    }

}

public record ChatResponse(string User, string Text);
