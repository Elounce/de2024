using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Model;

namespace WebApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ShiftController : ControllerBase
{
    private readonly MkarpovDe2024Context _context;

    public ShiftController(MkarpovDe2024Context context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Getall()
    {
        var shifts = _context.Shifts.ToList();
        return Ok(shifts);
    }

    [HttpPost]
    public async Task<IActionResult> Post(HttpRequest request)
    {
        var shift = await request.ReadFromJsonAsync<Shift>();
        
        if (shift == null)
            return BadRequest();

        _context.Add(shift);
        await _context.SaveChangesAsync();
        
        return Created($"/orders/{shift.Shiftid}", shift);
    }
}