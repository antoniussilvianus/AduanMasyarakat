using Microsoft.AspNetCore.Mvc;
using AduanMasyarakat.Models;

public class TicketController : Controller
{
    private readonly AduanContext _context;

    public TicketController(AduanContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var tickets = _context.Tickets.ToList();
        return View(tickets);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Ticket ticket)
    {
        ticket.Status = "Berjalan";
        _context.Tickets.Add(ticket);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
