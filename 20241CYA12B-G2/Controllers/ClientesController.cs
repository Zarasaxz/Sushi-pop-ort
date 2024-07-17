using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _20241CYA12B_G2.Models;
using Microsoft.AspNetCore.Identity;

namespace _20241CYA12B_G2.Controllers
{
    public class ClientesController : Controller
    {
        private readonly DbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public ClientesController(DbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {

            return _context.Cliente != null ?
                View(await _context.Cliente.ToListAsync()) :
                Problem("Entity set 'DbContext.Cliente' is null.");

        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create(IdentityUser? user)
        {
            if (user == null) return NotFound();

            Cliente cliente = new Cliente
            {
                Email = user.Email
            };

            return View(cliente);
        }


        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,Direccion,Telefono,FechaNacimiento,Email")] Cliente cliente)
        {
            cliente.Activo = true;
            cliente.FechaAlta = DateTime.Now;
            cliente.NumeroCliente = await GenerarNumeroCliente();

            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        //  [Authorize (Roles = "EMPLEADO")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumeroCliente,Id,Nombre,Apellido,Direccion,Telefono,FechaNacimiento,FechaAlta,Activo,Email")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cliente == null)
            {
                return Problem("Entity set 'DbContext.Cliente'  is null.");
            }
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente != null)
            {
                _context.Cliente.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return (_context.Cliente?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        private async Task<int?> GenerarNumeroCliente()
        {
            var cliente = await _context.Cliente.OrderByDescending(c => c.NumeroCliente).FirstOrDefaultAsync();

            if (cliente == null)
            {
                return 4200000;
            }

            return (int)cliente.NumeroCliente + 1;
        }
    }
}
