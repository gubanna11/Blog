using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

//TODO: Consider endpoints and implement them
[Route("api/[controller]")]
[ApiController]
public sealed class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /*[HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestObjectResult))]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _mediator.Send(new LoginCommand(request));

        if (result is not null)
        {

            return Ok(result);
        }

        return BadRequest("Invalid credentials");
    }*/

    /*[HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestObjectResult))]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _mediator.Send(new RegisterCommand(request));

        if (!result.Any())
        {
            return Ok();
        }

        return BadRequest(new RegisterFailed(result));
    }*/

    /*[HttpGet("confirmEmail")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var result = await _mediator.Send(new ConfirmEmailCommand(userId, token));

        if (result)
        {
            return Ok();
        }

        return NotFound();
    }*/
}