using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Konfigurasi koneksi MySQL
builder.Services.AddDbContext<AduanContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 32))
    ));

// Menambahkan layanan Controller dengan View
builder.Services.AddControllersWithViews();

// Menambahkan autentikasi menggunakan Cookie
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", config =>
    {
        config.LoginPath = "/Auth/Login";  // Redirect ke /Auth/Login jika belum login
        config.AccessDeniedPath = "/Auth/AccessDenied"; // Redirect jika akses ditolak
        config.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Sesi login selama 30 menit
    });

var app = builder.Build();

// Konfigurasi pipeline untuk Development dan Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Error handling untuk production
    app.UseHsts(); // Mengaktifkan HSTS
}

// Middleware umum
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Middleware untuk autentikasi dan otorisasi
app.UseAuthentication();
app.UseAuthorization();

// Middleware Role-Based Access Control (RBAC)
app.Use(async (context, next) =>
{
    var path = context.Request.Path;

    // Akses berdasarkan role
    if (path.StartsWithSegments("/Admin") &&
        !context.User.Claims.Any(c => c.Type == "Role" && c.Value == "Admin"))
    {
        context.Response.Redirect("/Auth/AccessDenied");
        return;
    }

    if (path.StartsWithSegments("/TeamLapangan") &&
        !context.User.Claims.Any(c => c.Type == "Role" && c.Value == "TeamLapangan"))
    {
        context.Response.Redirect("/Auth/AccessDenied");
        return;
    }

    await next.Invoke();
});

// Konfigurasi routing default
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Jalankan aplikasi
app.Run();
