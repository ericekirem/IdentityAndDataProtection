using IdentityAndDataProtection.Data;
using IdentityAndDataProtection.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAndDataProtection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IDataProtector _protector;

        public UsersController(AppDbContext context, IDataProtectionProvider dataProtectionProvider)
        {
            _context = context;
            _protector = dataProtectionProvider.CreateProtector("PasswordProtector");
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Şifreyi şifrele
            user.Password = _protector.Protect(user.Password);

            // Veritabanına ekle
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { Message = "User registered successfully!" });
        }
    }
}
