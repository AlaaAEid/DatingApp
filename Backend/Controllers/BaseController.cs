using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected readonly DataContext _context;
        protected readonly ILogger<BaseController> _logger;

        public BaseController(DataContext context,ILogger<BaseController> logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}