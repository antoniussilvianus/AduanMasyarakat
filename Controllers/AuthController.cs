using Microsoft.AspNetCore.Mvc;
using AduanMasyarakat.Models; // Sesuaikan dengan namespace
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

public class AuthController : Controller
{
    private readonly AduanContext _context;

    public AuthController(AduanContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login(string? role)
    {
        // Set default role menjadi 'User' jika tidak ada parameter 'role'
        ViewBag.Role = role switch
        {
            "Admin" => "Admin",
            "TeamLapangan" => "TeamLapangan",
            _ => "User"
        };
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string nik, string? role)
    {
        // Validasi input NIK
        if (string.IsNullOrEmpty(nik) || string.IsNullOrWhiteSpace(role))
        {
            ModelState.AddModelError("", "NIK dan Role wajib diisi.");
            return View();
        }

        // Cari user berdasarkan NIK di database
        var user = _context.Users.FirstOrDefault(u => u.NIK == nik);

        // Jika user tidak ditemukan atau diblokir
        if (user == null || user.IsBlocked)
        {
            ModelState.AddModelError("", "User tidak ditemukan atau telah diblokir.");
            return RedirectToAction("Login", new { role });
        }

        // Tentukan role berdasarkan parameter input
        var assignedRole = role switch
        {
            "Admin" => "Admin",
            "TeamLapangan" => "TeamLapangan",
            _ => "User"
        };

        // Cek apakah user memiliki role yang benar
        if (!ValidateUserRole(user, assignedRole))
        {
            ModelState.AddModelError("", $"Anda tidak memiliki akses sebagai {assignedRole}.");
            return RedirectToAction("Login", new { role });
        }

        // Buat klaim untuk autentikasi
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.NIK),
            new Claim(ClaimTypes.Name, user.Nama),
            new Claim("Role", assignedRole)  // Menyimpan role user
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties();

        // Lakukan sign-in menggunakan klaim
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity), authProperties);

        // Redirect ke halaman yang sesuai dengan role
        return assignedRole switch
        {
            "Admin" => RedirectToAction("Index", "Admin"),
            "TeamLapangan" => RedirectToAction("Index", "Admin"),//new edit
            _ => RedirectToAction("Index", "Ticket")
        };
    }

    // Fungsi untuk memvalidasi apakah user memiliki role yang sesuai
    private bool ValidateUserRole(User user, string role)
    {
        // Misalnya, kita cek apakah role user di database cocok dengan parameter role
        return user.Role.Equals(role, StringComparison.OrdinalIgnoreCase);
    }

    // Fungsi logout
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
