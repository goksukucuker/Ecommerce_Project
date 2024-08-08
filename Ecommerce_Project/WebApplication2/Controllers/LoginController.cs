using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Security;

namespace WebApplication2.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{

    private readonly IConfiguration _configuration;

    public LoginController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    

    private static List<UserModel> Users = new List<UserModel>()
    {
        new UserModel { Username = "goksukucuker", Password = 123 },
       
    };
    

    [HttpGet("Users")]
    public List<UserModel> Get()
    {
        return Users;
    }
    
    [HttpPost("Login")]
    public IActionResult Login([FromBody] UserModel login)
    {
        var user =Users.FirstOrDefault(u=>u.Username == login.Username && u.Password==login.Password);
        if (user == null)
        {
            return Unauthorized("Invalid username or password");
        }

        Token token = TokenHandler.CreateToken(_configuration);

        return Ok(token);

    }

    [HttpPost("Insert")]
    public IActionResult Insert([FromBody] UserModel addedUser)
    {
        if (addedUser == null)
        {
            return BadRequest("Invalid user");
        }
        Users.Add(addedUser);
        return Ok(Users);
        
    }

    [HttpDelete("Delete")]
    public IActionResult Delete([FromBody] UserModel deletedUser)
    {
        var user = Users.FirstOrDefault(u => u.Username == deletedUser.Username && u.Password == deletedUser.Password);
        if(deletedUser==null)
        {
            return NotFound("No such user was found");
        }

        Users.Remove(deletedUser);
        return NoContent();
    }

    [HttpPost("Update")]
    public IActionResult Update([FromBody] UserModel updatedUser)
    {
        var user = Users.FirstOrDefault(u => u.Username == updatedUser.Username && u.Password == updatedUser.Password);
        if (updatedUser == user)
        {
            return NotFound("No changes have been made");
        }
        else if (user == null)
        {
            return NotFound("The user to be changed could not be found");
        }
        else if (user.Username=="x")
        {
            user.Password = 333;
        }
        else
        {
            user.Username = updatedUser.Username;
            user.Password = updatedUser.Password;
        }
        

        return Ok(user);
    }

  
    
}