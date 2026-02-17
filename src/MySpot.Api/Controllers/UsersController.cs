using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;

namespace MySpot.Api.Controllers;

[ApiController]
[Route( "[controller]")]
public class UsersController(ICommandHandler<SignUpCommand> sighUpHandler
    ) : ControllerBase
{
    
    
    
    [HttpPost]
    public async Task<ActionResult> Post(SignUpCommand command)
    {
        command = command with { Id = Guid.NewGuid() };
        await sighUpHandler.HandleAsync(command);

        return NoContent(); // todo: change to created with proper header
    }
}