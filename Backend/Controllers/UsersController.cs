using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Controllers
{
  public class UsersController : BaseController
  {
    public UsersController(DataContext context, ILogger<BaseController> logger) : base(context, logger)
    {
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<User>>> getUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // api/users/3
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> getUser(int id)
    {
        return await _context.Users.FindAsync(id);
    }
  }
}