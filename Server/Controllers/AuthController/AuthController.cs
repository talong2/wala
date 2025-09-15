using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using PPDIS.Shared.Models.AuthModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Soil.Server.Controllers.AuthController
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMongoCollection<LoginClass> _login;

        public AuthController(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoConnection"));
            var db = client.GetDatabase("DataBase");

            // ✅ Initialize _login collection properly
            _login = db.GetCollection<LoginClass>("Login");
        }

        // GET: api/auth/get_user
        [HttpGet("get_user")]
        public async Task<ActionResult<List<LoginClass>>> GetAll()
        {
            return await _login.Find(q => true).ToListAsync();
        }

        // ✅ Login endpoint (used by QR login)
        [HttpPost("login")]
        public async Task<ActionResult<LoginClass>> Login([FromBody] LoginClass request)
        {
            if (string.IsNullOrWhiteSpace(request.email) || string.IsNullOrWhiteSpace(request.password))
            {
                return BadRequest("Email and password are required.");
            }

            var user = await _login
                .Find(u => u.email == request.email && u.password == request.password)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(user);
        }
    }
}