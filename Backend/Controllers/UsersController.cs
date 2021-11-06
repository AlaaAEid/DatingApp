using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UsersController : ControllerBase
  {
    private readonly DataContext _context;
    public UsersController(DataContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> getUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // api/users/3
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> getUser(int id)
    {
        return await _context.Users.FindAsync(id);
    }
  }
}