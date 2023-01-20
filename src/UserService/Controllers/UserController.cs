using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.CQRS.Commands;
using UserService.Features.Queries;

namespace UserService.Controllers;

[Controller]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("api1/user")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand model)
    {
        var identityResult = await _mediator.Send(model);
        if (identityResult.Succeeded)
        {
            return Ok(identityResult);
        }
        else
        {
            return BadRequest(identityResult);
        }
    }
    [HttpGet]
    [Route("api1/user")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _mediator.Send(new GetUsersQuery());
        return Ok(users);
    }
}