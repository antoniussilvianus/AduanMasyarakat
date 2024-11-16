using Microsoft.AspNetCore.Mvc;
using AduanMasyarakat.Models;

public class AdminController : Controller
{
    private readonly AduanContext _context;

    public AdminController(AduanContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var model = new
        {
            TotalAduan = _context.Tickets.Count(),
            AduanBerjalan = _context.Tickets.Count(t => t.Status == "Berjalan"),
            AduanSelesai = _context.Tickets.Count(t => t.Status == "Selesai")
        };
        return View(model);
    }
}
