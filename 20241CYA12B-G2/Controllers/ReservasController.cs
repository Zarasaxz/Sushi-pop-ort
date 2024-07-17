using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _20241CYA12B_G2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace _20241CYA12B_G2.Controllers
{
    public class ReservasController : Controller
    {
        private readonly DbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public ReservasController(DbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "EMPLEADO,CLIENTE")]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("EMPLEADO"))
            {
                var reservas = await _context.Reserva.Where(r => r.Confirmada == false).ToListAsync();
                return View(reservas);
            }

            var user = await _userManager.GetUserAsync(User);

            var reservasCliente = await _context.Reserva.Include(r => r.Cliente).Where(r => r.Cliente.Email.ToUpper() == user.NormalizedEmail).ToListAsync();

            return View(reservasCliente);
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reserva == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reserva
                .Include(r => r.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        [Authorize(Roles = "CLIENTE")]
        public IActionResult Create()
        {
            ViewBag.Locales = new List<SelectListItem>
    {
        new SelectListItem { Value = "PALERMO", Text = "Palermo"},
        new SelectListItem { Value = "RECOLETA", Text = "Recoleta"},
        new SelectListItem { Value = "BELGRANO", Text = "Belgrano"},
        new SelectListItem { Value = "CABALLITO", Text = "Caballito"},
    };

            // Depuración
            Debug.WriteLine("Locales asignados a ViewBag.Locales:");
            foreach (var item in ViewBag.Locales)
            {
                Debug.WriteLine($"Value: {item.Value}, Text: {item.Text}");
            }

            return View();
        }


        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Local,FechaHora")] Reserva reserva)
        {
            var user = await _userManager.GetUserAsync(User);
            var cliente = await _context.Cliente.FirstOrDefaultAsync(c => c.Email.ToUpper() == user.NormalizedEmail);

            Debug.WriteLine($"Usuario autenticado: {user.Email}, NormalizedEmail: {user.NormalizedEmail}");
            if (cliente == null)
            {
                // Log para depuración
                Debug.WriteLine("Cliente no encontrado para el correo: " + user.NormalizedEmail);
                ModelState.AddModelError(string.Empty, "No se encontró un cliente con el correo electrónico del usuario actual.");
                return View(reserva);
            }


            reserva.Nombre = cliente.Nombre;
            reserva.Apellido = cliente.Apellido;
            reserva.ClienteId = cliente.Id;
            reserva.Confirmada = false;

            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reserva == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reserva.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Id", reserva.ClienteId);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Local,FechaHora,Confirmada,Nombre,Apellido,ClienteId")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Id", reserva.ClienteId);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reserva == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reserva
                .Include(r => r.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reserva == null)
            {
                return Problem("Entity set 'DbContext.Reserva'  is null.");
            }
            var reserva = await _context.Reserva.FindAsync(id);
            if (reserva != null)
            {
                _context.Reserva.Remove(reserva);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return (_context.Reserva?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
