using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Application.Security;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace MySpot.Api.Controllers;

[ApiController]
[Authorize]
[Route( "[controller]")]
public class UsersController(
    ICommandHandler<SignUpCommand> sighUpHandler,
    IQueryHandler<GetUser, UserDto> getUserHandler,
    IQueryHandler<GetUsers, IEnumerable<UserDto>> getUsersHandler,
    ICommandHandler<SignInCommand> signInHandler,
    ITokenStorage tokenStorage
    ) : ControllerBase
{
    [HttpGet("{userId:guid}")]
    [Authorize(Policy = "is-admin")]
    [SwaggerOperation("Get single user by ID if exists.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto) )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> Get([FromRoute] Guid userId)
    {
        var user = await getUserHandler.HandleAsync(new GetUser { UserId = userId });
        
        return Ok(user);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> Get()
    {
        if (string.IsNullOrEmpty(HttpContext.User.Identity?.Name))
        {
            return NotFound();
        }
        var userId = Guid.Parse(HttpContext.User.Identity.Name);
        var user = await getUserHandler.HandleAsync(new GetUser { UserId = userId });
        
        return Ok(user);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserDto>>> Get([FromQuery] GetUsers query)
        => Ok(await getUsersHandler.HandleAsync(query));

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> Post(SignUpCommand command)
    {
        command = command with { Id = Guid.NewGuid() };
        await sighUpHandler.HandleAsync(command);

        return NoContent(); // todo: change to created with proper header
    }
    
    [HttpPost("sign-in")]
    [AllowAnonymous]
    public async Task<ActionResult<JwtDto>> Post(SignInCommand command)
    {
        await signInHandler.HandleAsync(command);

        return Ok(tokenStorage.Get());
    }
}