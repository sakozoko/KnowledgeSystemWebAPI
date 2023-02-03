using IdentityInfrastructure.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Features.Commands;
using UserService.Features.Queries;

namespace UserService.Controllers;

[Controller]
public class AccountController : ControllerBase
{
    
    private readonly IMediator _mediator;
    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [AllowAnonymous]
    [HttpPost("api1/user/register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand model)
    {
        var result = await _mediator.Send(model);
        return Ok(result);
    }
    
    [AllowAnonymous]
    [HttpPost("api1/user/changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand model)
    {
        model.SetUser(User);
        var result = await _mediator.Send(model);
        return Ok(result);
    }

    [Authorize(Roles = Role.Admin)]
    [HttpGet("api1/users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _mediator.Send(new GetUsersQuery(User));
        return Ok(users);
    }
    [Authorize]
    [HttpGet("api1/user/{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = await _mediator.Send(new GetUserQuery(id, User));
        return user!=null ? Ok(user) : NotFound();;
    }
    [Authorize(Roles = Role.Admin)]
    [HttpDelete("api1/user/{id}")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        var result = await _mediator.Send(new DeleteUserCommand(id,User));
        return Ok(result);
    }
    [Authorize]
    [HttpPut("api1/user/{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserInfoCommand model)
    {
        model.SetId(id);
        model.SetUser(User);
        var result = await _mediator.Send(model);
        return Ok(result);
    }
}