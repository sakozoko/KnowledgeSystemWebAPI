using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Features.Commands;
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
    [HttpDelete]
    [Route("api1/user/{id}")] 
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _mediator.Send(new DeleteUserCommand(id));
        return Ok(result);
    }
    [HttpPut]
    [Route("api1/user/{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserCommand model)
    {
        model.SetId(id);
        var result = await _mediator.Send(model);
        return Ok(result);
    }
}